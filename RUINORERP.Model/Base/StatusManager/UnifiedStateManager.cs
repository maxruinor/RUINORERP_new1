/**
 * 文件: UnifiedStateManager.cs
 * 版本: V4 - 优化版统一状态管理器
 * 说明: 统一状态管理器 - 基于V4版本架构优化，简化日志记录，提升性能
 * 创建日期: 2024年
 * 作者: RUINOR ERP开发团队
 * 更新日期: 2025-01-12 - V4版本优化，使用EntityStatus和StateTransitionResult
 */

using Microsoft.Extensions.Logging;
using RUINORERP.Global;
using RUINORERP.Global.EnumExt;
using SqlSugar.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace RUINORERP.Model.Base.StatusManager
{
    /// <summary>
    /// 统一状态管理器 - V4优化版
    /// 基于V4版本架构优化，简化日志记录，提升性能
    /// 支持MenuItemEnums，实现操作权限检查和UI控件影响
    /// </summary>
    public class UnifiedStateManager : IUnifiedStateManager, IDisposable
    {
        #region 私有字段

        /// <summary>
        /// 日志记录器
        /// </summary>
        private readonly ILogger<UnifiedStateManager> _logger;

        /// <summary>
        /// 状态转换事件
        /// </summary>
        public event EventHandler<StateTransitionEventArgs> StatusChanged;

        /// <summary>
        /// 反射方法缓存 - 用于缓存GlobalStateRulesManager的MethodInfo
        /// Key: 方法名称, Value: MethodInfo
        /// </summary>
        private static readonly Dictionary<string, MethodInfo> _methodCache = new Dictionary<string, MethodInfo>();

        /// <summary>
        /// 泛型方法缓存 - 用于缓存已构造的泛型方法
        /// Key: 方法名称 + 状态类型全名, Value: MethodInfo
        /// </summary>
        private static readonly Dictionary<string, MethodInfo> _genericMethodCache = new Dictionary<string, MethodInfo>();

        /// <summary>
        /// 缓存锁对象
        /// </summary>
        private static readonly object _cacheLock = new object();

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        public UnifiedStateManager(ILogger<UnifiedStateManager> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            // 不在构造函数中初始化规则，避免与Autofac冲突
            // 规则初始化由GlobalStateInitializer在DI注册时完成
        }

        #endregion

        #region 状态获取方法



        /// <summary>
        /// 获取实体的状态类型
        /// </summary>
        /// <returns>状态类型</returns>
        public virtual Type GetStatusType(BaseEntity entity)
        {
            try
            {
                // 检查实体是否包含各种状态类型的属性
                if (entity.ContainsProperty(typeof(DataStatus).Name))
                    return typeof(DataStatus);

                if (entity.ContainsProperty(typeof(PrePaymentStatus).Name))
                    return typeof(PrePaymentStatus);

                if (entity.ContainsProperty(typeof(ARAPStatus).Name))
                    return typeof(ARAPStatus);

                if (entity.ContainsProperty(typeof(PaymentStatus).Name))
                    return typeof(PaymentStatus);

                if (entity.ContainsProperty(typeof(StatementStatus).Name))
                    return typeof(StatementStatus);

                // 实体不包含任何状态属性时，记录警告并返回null
                return null;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "获取实体 {EntityType} 状态类型时发生异常", entity?.GetType().Name ?? "Unknown");
                return null;
            }
        }


        /// <summary>
        /// 获取实体的业务状态（整合版本）
        /// </summary>
        /// <typeparam name="T">业务状态枚举类型（可选）</typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">业务状态类型（可选）</param>
        /// <returns>业务状态枚举值</returns>
        public object GetBusinessStatus<T>(BaseEntity entity, Type statusType = null) where T : struct, Enum
        {
            if (entity == null)
                return default;

            // 如果没有指定状态类型，则使用泛型参数类型
            if (statusType == null)
                statusType = typeof(T);

            // 获取实体的状态类型
            var currentStatusType =  GetStatusType(entity);

            // 如果指定了状态类型且与实体当前状态类型匹配，直接返回该状态
            if (statusType != null && currentStatusType == statusType)
            {
                var statusValue = entity.GetPropertyValue(statusType.Name);
                // 如果状态值是int且状态类型是枚举，则转换为枚举值
                if (statusValue is int && statusType.IsEnum)
                {
                    try
                    {
                        return Enum.ToObject(statusType, statusValue);
                    }
                    catch (Exception ex) when (_logger != null)
                    {
                        _logger.LogError(ex, "转换状态值为枚举时发生错误");
                        return statusValue; // 转换失败时返回原始值
                    }
                }
                return statusValue;
            }

            // 如果没有指定状态类型（或类型不匹配），返回当前状态类型的枚举值
            if (currentStatusType != null && currentStatusType.IsEnum)
            {
                try
                {
                    dynamic status = entity.GetPropertyValue(currentStatusType.Name);
                    // 确保返回枚举值而不是int值
                    if (status is int)
                    {
                        return Enum.ToObject(currentStatusType, status);
                    }
                    return status;
                }
                catch (Exception ex) when (_logger != null)
                {
                    _logger.LogError(ex, "获取业务状态值时发生错误");
                }
            }

            return null;
        }

        /// <summary>
        /// 获取实体的业务状态（非泛型版本）
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">业务状态类型（可选）</param>
        /// <returns>业务状态枚举值</returns>
        public Enum GetBusinessStatus(BaseEntity entity, Type statusType = null)
        {
            if (entity == null)
                return null;
            if (statusType == null)
            {
                // 获取当前状态类型
                var currentStatusType = GetStatusType(entity);
                statusType = currentStatusType;
            }

            // 如果指定了状态类型且它是枚举类型，确保返回枚举值
            if (statusType != null && statusType.IsEnum)
            {
                try
                {
                    dynamic status = entity.GetPropertyValue(statusType.Name);
                    // 确保返回枚举值而不是int值
                    if (status is int)
                    {
                        return Enum.ToObject(statusType, status);
                    }
                    return status;
                }
                catch (Exception ex) when (_logger != null)
                {
                    _logger.LogError(ex, "获取业务状态值时发生错误");
                }
            }

            return null;
        }

        #endregion

        #region 状态验证方法

        // 使用项目中已有的状态缓存管理器
        private static readonly StatusCacheManager _statusCacheManager = new StatusCacheManager();

        /// <summary>
        /// 验证数据状态转换是否合法
        /// </summary>
        /// <param name="fromStatus">源状态</param>
        /// <param name="toStatus">目标状态</param>
        /// <returns>状态转换结果</returns>
        public StateTransitionResult ValidateBusinessStatusTransitionAsync(Enum fromStatus, Enum toStatus)
        {
            try
            {

                // 生成缓存键
                string cacheKey = $"{fromStatus?.GetType().FullName}:{fromStatus}:{toStatus}";

                // 检查缓存
                List<object> cachedItems;
                if (fromStatus != null && toStatus != null && (cachedItems = _statusCacheManager.GetTransitionRuleCache(cacheKey)) != null && cachedItems.Count > 0)
                {
                    return (StateTransitionResult)cachedItems[0];
                }

                // 参数校验
                if (fromStatus == null)
                    return StateTransitionResult.Allowed();

                if (toStatus == null)
                    return StateTransitionResult.Denied("目标状态不能为空");

                // 状态相同检查 - 避免不必要的验证（确保toStatus不为null）
                if (toStatus != null && fromStatus.Equals(toStatus))
                    return StateTransitionResult.Allowed();

                // 验证两个枚举类型是否一致
                if (fromStatus.GetType() != toStatus.GetType())
                    return StateTransitionResult.Denied($"源状态类型和目标状态类型不一致");

                // 动态检查状态转换是否有效（使用缓存优化性能）
                Type statusType = fromStatus.GetType();
                var isValidTransitionMethod = GetCachedGenericMethod("IsValidTransition", statusType);
                if (isValidTransitionMethod == null)
                {
                    return StateTransitionResult.Denied("无法获取状态转换验证方法");
                }
                bool isAllowed = (bool)isValidTransitionMethod.Invoke(GlobalStateRulesManager.Instance, new object[] { fromStatus, toStatus });

                StateTransitionResult result;
                if (isAllowed)
                    result = StateTransitionResult.Allowed();
                else
                    result = StateTransitionResult.Denied($"不允许从{fromStatus}转换到{toStatus}");

                // 缓存结果
                if (fromStatus != null && toStatus != null)
                {
                    _statusCacheManager.SetTransitionRuleCache(cacheKey, new List<object> { result });
                }

                return result;
            }
            catch (Exception ex) when (_logger != null)
            {
                _logger.LogError(ex, "验证状态转换时发生异常");
                return StateTransitionResult.Denied("验证状态转换时发生异常");
            }
        }

        #region 反射方法缓存辅助方法

        /// <summary>
        /// 获取缓存的MethodInfo（非泛型方法）
        /// </summary>
        /// <param name="methodName">方法名称</param>
        /// <returns>MethodInfo</returns>
        private MethodInfo GetCachedMethod(string methodName)
        {
            if (_methodCache.TryGetValue(methodName, out var method))
            {
                return method;
            }

            lock (_cacheLock)
            {
                // 双重检查锁定
                if (_methodCache.TryGetValue(methodName, out method))
                {
                    return method;
                }

                method = typeof(GlobalStateRulesManager).GetMethod(methodName);
                if (method == null)
                {
                    _logger?.LogError("未找到方法：{MethodName}", methodName);
                    return null;
                }

                _methodCache[methodName] = method;
                return method;
            }
        }

        /// <summary>
        /// 获取缓存的泛型MethodInfo
        /// </summary>
        /// <param name="methodName">方法名称</param>
        /// <param name="statusType">状态类型</param>
        /// <returns>构造后的泛型MethodInfo</returns>
        private MethodInfo GetCachedGenericMethod(string methodName, Type statusType)
        {
            if (statusType == null)
                return null;

            string cacheKey = $"{methodName}_{statusType.FullName}";

            if (_genericMethodCache.TryGetValue(cacheKey, out var genericMethod))
            {
                return genericMethod;
            }

            lock (_cacheLock)
            {
                // 双重检查锁定
                if (_genericMethodCache.TryGetValue(cacheKey, out genericMethod))
                {
                    return genericMethod;
                }

                var method = GetCachedMethod(methodName);
                if (method == null)
                {
                    return null;
                }

                genericMethod = method.MakeGenericMethod(statusType);
                _genericMethodCache[cacheKey] = genericMethod;
                return genericMethod;
            }
        }

        #endregion


        /// <summary>
        /// 验证业务状态转换是否合法（泛型版本）
        /// </summary>
        /// <typeparam name="T">业务状态枚举类型</typeparam>
        /// <param name="fromStatus">源状态</param>
        /// <param name="toStatus">目标状态</param>
        /// <returns>状态转换结果</returns>
        public StateTransitionResult ValidateBusinessStatusTransitionAsync<T>(T? fromStatus, T? toStatus) where T : struct, Enum
        {
            try
            {
                // 生成缓存键 - 使用typeof(T)而非fromStatus?.GetType()确保缓存键一致性
                string cacheKey = $"{typeof(T).FullName}:{fromStatus}:{toStatus}";

                // 检查缓存 - 使用状态缓存管理器
                List<object> cachedItems;
                if (fromStatus.HasValue && toStatus.HasValue && (cachedItems = _statusCacheManager.GetTransitionRuleCache(cacheKey)) != null && cachedItems.Count > 0)
                {
                    return (StateTransitionResult)cachedItems[0];
                }

                // 如果源状态为空，允许设置任何状态
                if (!fromStatus.HasValue)
                    return StateTransitionResult.Allowed();

                // 如果目标状态为空，不允许转换
                if (!toStatus.HasValue)
                    return StateTransitionResult.Denied("目标状态不能为空");

                // 状态相同检查 - 避免不必要的验证
                if (fromStatus.Value.Equals(toStatus.Value))
                    return StateTransitionResult.Allowed();

                // 动态检查状态转换是否有效
                Type statusType = fromStatus.Value.GetType();
                bool isAllowed = (bool)typeof(GlobalStateRulesManager)
                    .GetMethod("IsValidTransition")
                    .MakeGenericMethod(statusType)
                    .Invoke(GlobalStateRulesManager.Instance, new object[] { fromStatus.Value, toStatus.Value });

                StateTransitionResult result;
                if (isAllowed)
                    result = StateTransitionResult.Allowed();
                else
                    result = StateTransitionResult.Denied($"不允许从{fromStatus.Value}转换到{toStatus.Value}");

                // 缓存结果
                _statusCacheManager.SetTransitionRuleCache(cacheKey, new List<object> { result });

                return result;
            }
            catch (Exception ex) when (_logger != null)
            {
                _logger.LogError(ex, "验证状态转换时发生异常");
                return StateTransitionResult.Denied("验证状态转换时发生异常");
            }
        }

        /// <summary>
        /// 验证操作状态转换
        /// </summary>
        /// <param name="fromStatus">源状态</param>
        /// <param name="toStatus">目标状态</param>
        /// <returns>状态转换结果</returns>
        public async Task<StateTransitionResult> ValidateActionStatusTransitionAsync(ActionStatus fromStatus, ActionStatus? toStatus)
        {
            // 对于ActionStatus，不做特殊限制，只进行基本验证
            if (!toStatus.HasValue)
                return StateTransitionResult.Denied("目标状态不能为空");

            // 状态相同检查
            if (fromStatus.Equals(toStatus.Value))
                return StateTransitionResult.Allowed();

            return await Task.FromResult(StateTransitionResult.Allowed());
        }

        #endregion

        #region 状态更新和事件方法

        /// <summary>
        /// 更新实体状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">状态类型</param>
        /// <param name="newStatus">新状态</param>
        /// <param name="reason">变更原因</param>
        /// <param name="userId">用户ID</param>
        /// <returns>状态转换结果</returns>
        private StateTransitionResult UpdateBusinessStatus(BaseEntity entity, Type statusType, object newStatus, string reason = null, string userId = null)
        {
            if (entity == null)
                return StateTransitionResult.Failure(null, newStatus, statusType, "实体不能为空");

            if (statusType == null)
                return StateTransitionResult.Failure(null, newStatus, statusType, "状态类型不能为空");

            // 获取旧状态 - 统一使用相同的方法获取，不再对DataStatus特殊处理
            // GetBusinessStatus方法现在直接返回枚举值，无需额外转换
            object oldStatus = GetBusinessStatus(entity, statusType);
            // 直接使用oldStatus，它已经是枚举值（如果状态类型是枚举）

            // 检查状态是否实际发生了变更
            if (Equals(oldStatus, newStatus))
                return StateTransitionResult.Success(oldStatus, newStatus, statusType, "状态未发生变更");

            try
            {
                // 更新状态 - 统一使用属性名设置，不再对DataStatus特殊处理
                // 首先尝试使用类型名称查找属性
                var statusProperty = entity.GetType().GetProperty(statusType.Name);

                // 如果找不到，尝试使用"DataStatus"作为属性名（兼容现有的实体）
                if (statusProperty == null || !statusProperty.CanWrite)
                {
                    statusProperty = entity.GetType().GetProperty("DataStatus");
                }

                // 如果找到可写的属性，则设置值
                if (statusProperty != null && statusProperty.CanWrite)
                {
                    // 确保设置的值是int类型，将枚举转换为int
                    object valueToSet = newStatus;
                    if (newStatus.GetType().IsEnum)
                    {
                        valueToSet = Convert.ToInt32(newStatus);
                    }
                    statusProperty.SetValue(entity, valueToSet);
                }

                // 触发状态变更事件
                TriggerStatusChangedEvent(entity, statusType, oldStatus, newStatus, reason, userId);

                return StateTransitionResult.Success(oldStatus, newStatus, statusType);
            }
            catch (Exception ex) when (_logger != null)
            {
                _logger.LogError(ex, "更新实体状态时发生错误");
                return StateTransitionResult.Failure(oldStatus, newStatus, statusType, ex.Message, ex);
            }
        }

        /// <summary>
        /// 更新实体的操作状态（ActionStatus）
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="newStatus">新的操作状态</param>
        /// <param name="reason">状态变更原因</param>
        /// <param name="userId">执行操作的用户ID</param>
        /// <returns>状态转换结果对象</returns>
        private StateTransitionResult UpdateActionStatus(BaseEntity entity, ActionStatus newStatus, string reason = null, string userId = null)
        {
            // 增强验证：检查实体有效性
            if (entity == null)
                return StateTransitionResult.Failure(null, newStatus, typeof(ActionStatus), "实体对象不能为空");

            // 获取旧状态
            var oldStatus = entity.ActionStatus;

            // 性能优化：检查状态是否实际发生了变更
            if (Equals(oldStatus, newStatus))
                return StateTransitionResult.Success(oldStatus, newStatus, typeof(ActionStatus), "状态未发生变更");

            try
            {
                // 更新状态
                entity.ActionStatus = newStatus;

                // 优化事件触发：单独异常处理，避免事件处理器异常影响核心功能
                try
                {
                    // 触发状态变更事件
                    TriggerStatusChangedEvent(entity, typeof(ActionStatus), oldStatus, newStatus, reason, userId);
                }
                catch (Exception eventEx) when (_logger != null)
                {
                    // 记录事件触发异常，但不影响状态更新结果
                    _logger.LogError(eventEx, "触发操作状态变更事件时发生异常");
                }

                return StateTransitionResult.Success(oldStatus, newStatus, typeof(ActionStatus));
            }
            catch (Exception ex) when (_logger != null)
            {
                _logger.LogError(ex, "更新实体的操作状态时发生错误");
                return StateTransitionResult.Failure(oldStatus, newStatus, typeof(ActionStatus), ex.Message, ex);
            }
        }

        /// <summary>
        /// 触发状态变更事件（Winform桌面程序优化版）
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">状态类型</param>
        /// <param name="oldStatus">旧状态</param>
        /// <param name="newStatus">新状态</param>
        /// <param name="reason">变更原因</param>
        /// <param name="userId">用户ID</param>
        public void TriggerStatusChangedEvent(BaseEntity entity, Type statusType, object oldStatus, object newStatus, string reason = null, string userId = null)
        {
            // 快速检查是否有订阅者，避免不必要的对象创建
            if (StatusChanged == null) return;

            try
            {
                // 创建事件参数 - V4版本兼容
                var eventArgs = new StateTransitionEventArgs(
                    entity,
                    statusType,
                    oldStatus,
                    newStatus,
                    reason,
                    userId);

                // 获取事件委托副本，避免多线程情况下的空引用问题
                var statusChangedEvent = StatusChanged;

                // 触发事件
                statusChangedEvent?.Invoke(this, eventArgs);
            }
            catch (Exception ex) when (_logger != null)
            {
                // 记录错误信息，但不中断程序流程
                _logger.LogError(ex, "触发状态变更事件时发生错误");
            }
        }

        #endregion

        #region 状态设置方法

        /// <summary>
        /// 设置实体的业务状态
        /// </summary>
        /// <typeparam name="T">业务状态枚举类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="status">业务状态</param>
        /// <param name="reason">变更原因</param>
        /// <param name="userId">用户ID</param>
        /// <returns>状态转换结果</returns>
        public async Task<StateTransitionResult> SetBusinessStatusAsync<T>(BaseEntity entity, T? status, string reason = null, string userId = null) where T : struct, Enum
        {
            if (entity == null)
                return StateTransitionResult.Failure(null, status, typeof(T), "实体不能为空");

            if (!status.HasValue)
                return StateTransitionResult.Failure(null, null, typeof(T), "目标状态不能为空");

            // 验证状态转换
            var currentStatus = GetBusinessStatus<T>(entity);
            var validationResult = ValidateBusinessStatusTransitionAsync(currentStatus as Enum, status);
            if (!validationResult.IsSuccess)
                return validationResult;

            // 更新状态
            var result = UpdateBusinessStatus(entity, typeof(T), status, reason, userId);

            return await Task.FromResult(result);
        }

        /// <summary>
        /// 设置实体的业务状态（非泛型版本）
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">业务状态类型</param>
        /// <param name="status">业务状态</param>
        /// <param name="reason">变更原因</param>
        /// <param name="userId">用户ID</param>
        /// <returns>状态转换结果</returns>
        public async Task<StateTransitionResult> SetBusinessStatusAsync(BaseEntity entity, Type statusType, object status, string reason = null, string userId = null)
        {
            if (entity == null)
                return StateTransitionResult.Failure(null, status, statusType, "实体不能为空");

            if (statusType == null)
                return StateTransitionResult.Failure(null, status, statusType, "状态类型不能为空");

            // 验证状态转换
            var currentStatus = GetBusinessStatus(entity, statusType);
            var validationResult = ValidateBusinessStatusTransitionAsync(currentStatus as Enum, status as Enum);
            if (!validationResult.IsSuccess)
                return validationResult;

            // 更新状态
            var result = UpdateBusinessStatus(entity, statusType, status, reason, userId);

            return await Task.FromResult(result);
        }

        /// <summary>
        /// 设置实体的操作状态（ActionStatus）
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="status">目标操作状态</param>
        /// <param name="reason">状态变更原因</param>
        /// <param name="userId">执行操作的用户ID</param>
        /// <returns>状态转换结果对象</returns>
        public async Task<StateTransitionResult> SetActionStatusAsync(BaseEntity entity, ActionStatus? status, string reason = null, string userId = null)
        {
            // 前置验证
            if (entity == null)
                return StateTransitionResult.Failure(null, status, typeof(ActionStatus), "实体对象不能为空");

            // 验证目标状态非空
            if (!status.HasValue)
                return StateTransitionResult.Failure(entity.ActionStatus, null, typeof(ActionStatus), "目标操作状态不能为空");

            try
            {
                // 验证状态转换合法性
                var currentStatus = entity.ActionStatus;
                var validationResult = await ValidateActionStatusTransitionAsync(currentStatus, status);

                if (!validationResult.IsSuccess)
                    return validationResult;

                // 更新状态
                var result = UpdateActionStatus(entity, status.Value, reason, userId);

                return result;
            }
            catch (Exception ex) when (_logger != null)
            {
                _logger.LogError(ex, "设置操作状态时发生异常");
                return StateTransitionResult.Failure(entity.ActionStatus, status, typeof(ActionStatus), "设置操作状态时发生异常", ex);
            }
        }

        /// <summary>
        /// 通过UI操作类型设置实体状态（通用方法）
        /// 支持提交、审核、反审等所有预定义的UI操作
        /// 这是一个简化的通用接口，将UI操作自动映射到具体的状态值
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="action">操作类型（提交、审核、反审等）</param>
        /// <param name="reason">变更原因</param>
        /// <param name="userId">用户ID</param>
        /// <returns>状态转换结果</returns>
        public Task<StateTransitionResult> SetStatusByActionAsync(BaseEntity entity, MenuItemEnums action, string reason = null, string userId = null)
        {
            if (entity == null)
                return Task.FromResult(StateTransitionResult.Denied("实体不能为空"));

            try
            {
                // 获取状态类型
                var statusType = GetStatusType(entity);
                
                // 获取当前状态
                var currentStatus = GetBusinessStatus(entity, statusType);
                
                // 获取目标状态值
                var targetStatus = GlobalStateRulesManager.MapActionToStatus(entity, action);
                if (targetStatus == null)
                    return Task.FromResult(StateTransitionResult.Denied($"无法映射操作类型 '{action}' 到状态值"));

                // 如果目标状态为当前状态，返回提示
                if (Equals(currentStatus, targetStatus))
                    return Task.FromResult(StateTransitionResult.Allowed()); // 无需变更

                // 验证状态转换
                var validationResult = ValidateBusinessStatusTransitionAsync(currentStatus as Enum, targetStatus as Enum);
                if (!validationResult.IsSuccess)
                    return Task.FromResult(validationResult);

                // 更新状态
                var actionDesc = GlobalStateRulesManager.GetActionDescription(action);
                var finalReason = reason ?? actionDesc;

                var result = UpdateBusinessStatus(entity, statusType, targetStatus, finalReason, userId);

                return Task.FromResult(result);
            }
            catch (Exception ex)
            {
                return Task.FromResult(StateTransitionResult.Denied($"执行操作时发生错误: {ex.Message}"));
            }
        }

  

        #endregion

        #region 操作权限检查方法

        /// <summary>
        /// 验证操作权限（通过UI操作类型）
        /// 优化版本：针对不同操作类型采用不同判断逻辑
        /// 明确区分状态转换类操作和无目标状态操作
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="action">操作类型</param>
        /// <returns>验证结果</returns>
        public (bool CanExecute, string Message) CanExecuteActionWithMessage(BaseEntity entity, MenuItemEnums action)
        {
            if (entity == null)
                return (false, "实体不能为空");

            // 遵循终态不可修改原则：如果是终态，所有修改操作都不允许
            if (IsFinalStatus(entity))
            {
                // 终态只允许查询和打印等只读操作
                var readonlyActions = new[] { MenuItemEnums.查询, MenuItemEnums.打印, MenuItemEnums.导出 };
                if (readonlyActions.Contains(action))
                {
                    return (true, $"终态的单据可以执行{action}操作");
                }
                else
                {
                    return (false, $"单据已达到终态，不允许执行{action}操作");
                }
            }

            // 获取状态类型和当前状态
            var statusType = GetStatusType(entity);
            var currentStatus = GetBusinessStatus(entity, statusType);

            // 首先判断操作类型，采用不同的验证策略
            if (GlobalStateRulesManager.IsStateTransitionAction(action))
            {
                // 状态转换类操作：需要验证状态转换的合法性
                return CanExecuteStateTransitionAction(entity, action, currentStatus, statusType);
            }
            else if (GlobalStateRulesManager.IsNonStateTransitionAction(action))
            {
                // 无目标状态操作：仅基于当前状态和规则判断
                return CanExecuteNonStateTransitionAction(entity, action, currentStatus, statusType);
            }
            else
            {
                // 未知操作类型：默认不允许
                return (false, $"不支持的操作类型：{action}");
            }
        }

        /// <summary>
        /// 验证无目标状态操作权限（修改、删除、保存等）
        /// 仅基于当前状态和全局规则进行判断，不涉及状态转换验证
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="action">操作类型</param>
        /// <param name="currentStatus">当前状态</param>
        /// <param name="statusType">状态类型</param>
        /// <returns>验证结果</returns>
        private (bool CanExecute, string Message) CanExecuteNonStateTransitionAction(BaseEntity entity, MenuItemEnums action, object currentStatus, Type statusType)
        {
            if (currentStatus == null)
                return (false, "无法获取当前状态");

            try
            {
                // 使用反射调用GlobalStateRulesManager的泛型方法（使用缓存优化性能）
                var canExecuteMethod = GetCachedGenericMethod("CanExecuteNonStateTransitionAction", statusType);
                if (canExecuteMethod == null)
                {
                    return (false, "无法获取操作权限验证方法");
                }
                bool canExecute = (bool)canExecuteMethod.Invoke(GlobalStateRulesManager.Instance, new object[] { currentStatus, action });

                // 获取执行条件说明（使用缓存优化性能）
                var conditionMethod = GetCachedGenericMethod("GetNonStateTransitionActionCondition", statusType);
                string conditionMessage = conditionMethod != null
                    ? (string)conditionMethod.Invoke(GlobalStateRulesManager.Instance, new object[] { currentStatus, action })
                    : string.Empty;

                if (canExecute)
                {
                    // 对于修改操作，还需要检查提交后修改规则11
                    if (action == MenuItemEnums.修改)
                    {
                        var modifyCheck = CanModifyWithMessage(entity);
                        return modifyCheck.CanExecute ? (true, conditionMessage) : modifyCheck;
                    }

                    return (true, conditionMessage);
                }
                else
                {
                    return (false, conditionMessage);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"验证无目标状态操作权限时发生异常：{action}");
                return (false, $"验证操作权限时发生错误：{ex.Message}");
            }
        }

        /// <summary>
        /// 验证状态转换类操作权限（提交、审核、反审等）
        /// 需要验证状态转换的合法性，并获取相应的执行条件说明
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="action">操作类型</param>
        /// <param name="currentStatus">当前状态</param>
        /// <param name="statusType">状态类型</param>
        /// <returns>验证结果</returns>
        private (bool CanExecute, string Message) CanExecuteStateTransitionAction(BaseEntity entity, MenuItemEnums action, object currentStatus, Type statusType)
        {
            if (currentStatus == null)
                return (false, "无法获取当前状态");

            try
            {
                // 获取目标状态
                var targetStatus = GlobalStateRulesManager.MapActionToStatus(entity, action);
                if (targetStatus == null)
                    return (false, $"无法映射操作类型 '{action}' 到状态值");
                
                // 如果目标状态为当前状态，不允许重复执行该操作
                if (Equals(currentStatus, targetStatus))
                {
                    var statusName = currentStatus?.ToString() ?? "未知状态";
                    var actionName = GetActionName(action);
                    return (false, $"当前单据已为【{statusName}】状态,无需重复{actionName}");
                }
                
                // 验证状态转换
                var validationResult = ValidateBusinessStatusTransitionAsync(currentStatus as Enum, targetStatus as Enum);
                
                // 获取执行条件说明
                var conditionMethod = typeof(GlobalStateRulesManager).GetMethod("GetStateTransitionActionCondition");
                var genericConditionMethod = conditionMethod.MakeGenericMethod(statusType);
                var conditionMessage = (string)genericConditionMethod.Invoke(GlobalStateRulesManager.Instance, new object[] { currentStatus, action });

                // 根据验证结果返回相应的消息
                if (validationResult.IsSuccess)
                    return (true, conditionMessage);
                else
                    return (false, validationResult.ErrorMessage ?? conditionMessage);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"验证状态转换类操作权限时发生异常：{action}");
                return (false, $"验证操作权限时发生错误：{ex.Message}");
            }
        }

        /// <summary>
        /// 验证修改操作权限
        /// </summary>
        private (bool CanExecute, string Message) CanModifyWithMessage(BaseEntity entity)
        {
            // 获取状态类型和当前状态
            var statusType = GetStatusType(entity);
            var currentStatus = GetBusinessStatus(entity, statusType);
            var statusName = currentStatus?.ToString() ?? "未知状态";

            // 1. 检查是否为终态状态
            bool isFinalStatus = IsFinalStatus(entity);
            if (isFinalStatus)
                return (false, "终态状态下不允许修改");

            // 2. 检查是否为审核后状态（审核后的状态不允许修改）
            bool isApprovedStatus = IsApprovedStatus(entity);
            if (isApprovedStatus)
                return (false, $"状态为【{statusName}】的单据不允许修改");

            // 3. 检查提交后是否允许修改（灵活/严格模式）
            // 获取是否为已提交状态（非草稿状态视为已提交）
            bool isSubmittedStatus = !IsDraftStatus(entity);
            if (!AllowModifyAfterSubmit(isSubmittedStatus))
                return (false, "已提交状态下不允许修改");

            return (true, "可以修改当前记录");
        }

        /// <summary>
        /// 使用增强规则检查操作权限
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="action">操作类型</param>
        /// <returns>是否可以执行</returns>
        private bool CanExecuteAction(BaseEntity entity, MenuItemEnums action)
        {
            // 获取状态类型
            var statusType = GetStatusType(entity);
            if (statusType == null)
                return true;

            // 获取当前状态
            var currentStatus = GetBusinessStatus(entity, statusType);

            // 修改操作特殊处理：修改操作不改变状态，只要不是终态状态就应该允许
            if (action == MenuItemEnums.修改)
            {
                // 检查是否为终态状态，终态不允许修改
                bool isFinalStatus = IsFinalStatus(entity);
                return !isFinalStatus;
            }

            // 获取目标状态,只有提交，审核，反审核，结案，反结案这些才有目标状态。因为他们才是会改变状态的操作。其它没有目标状态！
            var targetStatus = GlobalStateRulesManager.MapActionToStatus(entity, action);
            if (targetStatus == null)
                return false;

            // 如果目标状态为当前状态，返回false
            if (Equals(currentStatus, targetStatus))
                return false;

            // 验证状态转换
            var validationResult = ValidateBusinessStatusTransitionAsync(currentStatus as Enum, targetStatus as Enum);

            return validationResult.IsSuccess;
        }

        /// <summary>
        /// 通过UI操作类型验证实体状态转换是否合法
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="action">操作类型</param>
        /// <returns>验证结果</returns>
        public Task<StateTransitionResult> ValidateActionTransitionAsync(BaseEntity entity, MenuItemEnums action)
        {
            if (entity == null)
                return Task.FromResult(StateTransitionResult.Denied("实体不能为空"));
                
            try
            {
                // 获取状态类型
                var statusType = GetStatusType(entity);
                
                // 获取当前状态
                var currentStatus = GetBusinessStatus(entity, statusType);
                
                // 获取目标状态
                var targetStatus = GlobalStateRulesManager.MapActionToStatus(entity, action);
                if (targetStatus == null)
                    return Task.FromResult(StateTransitionResult.Denied($"无法映射操作类型 '{action}' 到状态值"));
                
                // 验证状态转换
                var result = ValidateBusinessStatusTransitionAsync(currentStatus as Enum, targetStatus as Enum);
                return Task.FromResult(result);
            }
            catch (Exception ex)
            {
                return Task.FromResult(StateTransitionResult.Denied($"验证状态转换时发生错误: {ex.Message}"));
            }
        }

        /// <summary>
        /// 获取操作成功消息
        /// </summary>
        /// <param name="action">操作类型</param>
        /// <returns>成功消息</returns>
        private string GetSuccessMessage(MenuItemEnums action)
        {
            switch (action)
            {
                case MenuItemEnums.修改:
                    return "可以修改当前记录";
                case MenuItemEnums.删除:
                    return "可以删除当前记录";
                case MenuItemEnums.保存:
                    return "可以保存当前记录";
                case MenuItemEnums.提交:
                    return "可以提交当前记录";
                case MenuItemEnums.审核:
                    return "可以审核当前记录";
                case MenuItemEnums.反审:
                    return "可以取消审核当前记录";
                case MenuItemEnums.查询:
                    return "可以查看当前记录";
                case MenuItemEnums.打印:
                    return "可以打印当前记录";
                case MenuItemEnums.导出:
                    return "可以导出当前记录";
                default:
                    return "操作成功";
            }
        }

        /// <summary>
        /// 获取操作名称
        /// </summary>
        /// <param name="action">操作类型</param>
        /// <returns>操作名称</returns>
        private string GetActionName(MenuItemEnums action)
        {
            switch (action)
            {
                case MenuItemEnums.提交:
                    return "提交";
                case MenuItemEnums.审核:
                    return "审核";
                case MenuItemEnums.反审:
                    return "取消审核";
                case MenuItemEnums.结案:
                    return "结案";
                case MenuItemEnums.反结案:
                    return "取消结案";
                case MenuItemEnums.作废:
                    return "作废";
                default:
                    return action.ToString();
            }
        }

        #endregion

        #region UI控制方法

        /// <summary>
        /// 获取UI控件状态 - 统一接口版本
        /// 使用GlobalStateRulesManager的公共方法获取按钮状态，避免直接访问内部字段
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>UI控件状态</returns>
        public Dictionary<string, bool> GetUIControlStates(BaseEntity entity)
        {
            try
            {
                // 创建结果字典
                var result = new Dictionary<string, bool>();

                // 获取实体的状态类型
                var statusType = GetStatusType(entity);
                if (statusType == null)
                {
                    return result;
                }
                
                // 获取实体的业务状态值
                object businessStatus = GetBusinessStatus(entity, statusType);
                if (businessStatus == null)
                {
                    return result;
                }
                
                // 使用GlobalStateRulesManager的公共方法获取按钮规则，避免直接访问UIButtonRules
                var buttonStates = GlobalStateRulesManager.Instance.GetButtonRules(statusType, businessStatus);
                
                // 复制按钮状态到结果字典
                foreach (var state in buttonStates)
                {
                    result[state.Key] = state.Value;
                }

                return result;
            }
            catch (Exception ex) when (_logger != null)
            {
                _logger.LogError(ex, "获取UI控件状态失败");
                return new Dictionary<string, bool>();
            }
        }

        /// <summary>
        /// 获取按钮状态Enabled
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="buttonName">按钮名称</param>
        /// <returns>按钮状态</returns>
        public bool GetButtonState(BaseEntity entity, string buttonName)
        {
            try
            {
                // 获取所有按钮状态
                var buttonStates = GetUIControlStates(entity);

                // 返回指定按钮的状态
                if (buttonStates.TryGetValue(buttonName, out var state))
                {
                    return state;
                }
                return false; // 默认禁用，可见
            }
            catch (Exception ex) when (_logger != null)
            {
                _logger.LogError(ex, $"获取按钮状态失败: {buttonName}");
                return false;
            }
        }

        #endregion

        #region 新增方法 - 状态终态判断

        /// <summary>
        /// 判断指定实体的业务状态是否为终态
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <returns>是否为终态</returns>
        public bool IsFinalStatus<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                // 获取实体的状态类型
                Type statusType = GetStatusType(entity);
                if (statusType == null)
                    return false;

                // 获取实体的业务状态值
                object businessStatus = GetBusinessStatus(entity, statusType);
                if (businessStatus == null)
                    return false;

                // 针对不同状态类型进行终态判断
                if (statusType == typeof(DataStatus))
                {
                    DataStatus dataStatus = (DataStatus)businessStatus;
                    return dataStatus == DataStatus.完结 || dataStatus == DataStatus.作废;
                }
                else if (statusType == typeof(PaymentStatus))
                {
                    PaymentStatus paymentStatus = (PaymentStatus)businessStatus;
                    return paymentStatus == PaymentStatus.已支付;
                }
                else if (statusType == typeof(RefundStatus))
                {
                    RefundStatus refundStatus = (RefundStatus)businessStatus;
                    return refundStatus == RefundStatus.已退款已退货 || refundStatus == RefundStatus.已退款未退货 || refundStatus == RefundStatus.部分退款退货;
                }
                else if (statusType == typeof(PrePaymentStatus))
                {
                    PrePaymentStatus prepayStatus = (PrePaymentStatus)businessStatus;
                    return prepayStatus == PrePaymentStatus.全额核销 || prepayStatus == PrePaymentStatus.全额退款;
                }
                else if (statusType == typeof(ARAPStatus))
                {
                    ARAPStatus arapStatus = (ARAPStatus)businessStatus;
                    return arapStatus == ARAPStatus.全部支付 || arapStatus == ARAPStatus.已冲销;
                }
                else if (statusType == typeof(StatementStatus))
                {
                    StatementStatus statementStatus = (StatementStatus)businessStatus;
                    return statementStatus == StatementStatus.全部结清 || statementStatus == StatementStatus.已作废;
                }

                // 默认情况下，不是终态
                return false;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "判断实体状态是否为终态时发生异常");
                return false;
            }
        }

        /// <summary>
        /// 判断实体是否为审核后状态
        /// 审核后的状态（如确认、已生效）不允许修改
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>是否为审核后状态</returns>
        public bool IsApprovedStatus<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                // 获取实体的状态类型
                Type statusType = GetStatusType(entity);
                if (statusType == null)
                    return false;

                // 获取实体的业务状态值
                object businessStatus = GetBusinessStatus(entity, statusType);
                if (businessStatus == null)
                    return false;

                // 针对不同状态类型进行审核后状态判断
                if (statusType == typeof(DataStatus))
                {
                    DataStatus dataStatus = (DataStatus)businessStatus;
                    return dataStatus == DataStatus.确认;
                }
                else if (statusType == typeof(PaymentStatus))
                {
                    PaymentStatus paymentStatus = (PaymentStatus)businessStatus;
                    return paymentStatus == PaymentStatus.已支付;
                }
                else if (statusType == typeof(PrePaymentStatus))
                {
                    PrePaymentStatus prepayStatus = (PrePaymentStatus)businessStatus;
                    return prepayStatus == PrePaymentStatus.已生效;
                }
                else if (statusType == typeof(ARAPStatus))
                {
                    ARAPStatus arapStatus = (ARAPStatus)businessStatus;
                    return arapStatus == ARAPStatus.已冲销;
                }
                else if (statusType == typeof(RefundStatus))
                {
                    RefundStatus refundStatus = (RefundStatus)businessStatus;
                    return refundStatus == RefundStatus.已退款已退货 || refundStatus == RefundStatus.已退款未退货 || refundStatus == RefundStatus.部分退款退货;
                }
                else if (statusType == typeof(StatementStatus))
                {
                    StatementStatus statementStatus = (StatementStatus)businessStatus;
                    return statementStatus == StatementStatus.确认 || statementStatus == StatementStatus.全部结清;
                }

                // 默认情况下，不是审核后状态
                return false;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "判断实体状态是否为审核后状态时发生异常");
                return false;
            }
        }

        /// <summary>
        /// 判断实体是否为草稿状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>是否为草稿状态</returns>
        public bool IsDraftStatus<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                // 获取实体的状态类型
                Type statusType = GetStatusType(entity);
                if (statusType == null)
                    return false;

                // 获取实体的业务状态值
                object businessStatus = GetBusinessStatus(entity, statusType);
                if (businessStatus == null)
                    return false;

                // 针对不同状态类型进行草稿状态判断
                if (statusType == typeof(DataStatus))
                {
                    DataStatus dataStatus = (DataStatus)businessStatus;
                    return dataStatus == DataStatus.草稿;
                }
                else if (statusType == typeof(PaymentStatus))
                {
                    PaymentStatus paymentStatus = (PaymentStatus)businessStatus;
                    return paymentStatus == PaymentStatus.草稿;
                }
                else if (statusType == typeof(PrePaymentStatus))
                {
                    PrePaymentStatus prepayStatus = (PrePaymentStatus)businessStatus;
                    return prepayStatus == PrePaymentStatus.草稿;
                }

                // 其他状态类型默认不是草稿
                return false;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "判断实体状态是否为草稿状态时发生异常");
                return false;
            }
        }

        /// <summary>
        /// 判断指定实体是否可以修改，并返回详细消息
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <returns>是否可以修改及详细消息</returns>
        public (bool CanModify, string Message) CanModifyWithMessage<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            // 获取状态类型和当前状态
            var statusType = GetStatusType(entity);
            var currentStatus = GetBusinessStatus(entity, statusType);
            var statusName = currentStatus?.ToString() ?? "未知状态";

            // 1. 检查是否为终态状态
            bool isFinalStatus = IsFinalStatus<TEntity>(entity);
            if (isFinalStatus)
                return (false, "终态状态下不允许修改");

            // 2. 检查是否为审核后状态（审核后的状态不允许修改）
            bool isApprovedStatus = IsApprovedStatus(entity);
            if (isApprovedStatus)
                return (false, $"状态为【{statusName}】的单据不允许修改");

            // 3. 检查提交后是否允许修改（灵活/严格模式）
            // 获取是否为已提交状态（非草稿状态视为已提交）
            bool isSubmittedStatus = !IsDraftStatus(entity);
            if (!AllowModifyAfterSubmit(isSubmittedStatus))
                return (false, "已提交状态下不允许修改");

            return (true, "可以修改当前记录");
        }
 

        /// <summary>
        /// 检查是否需要关键操作二次确认
        /// 用于对关键操作（如删除已审核单据、作废单据等）进行二次确认
        /// </summary>
        /// <typeparam name="T">状态类型</typeparam>
        /// <param name="status">当前状态</param>
        /// <param name="operationType">操作类型（如"delete", "cancel", "reverseReview"等）</param>
        /// <returns>是否需要二次确认</returns>
        public bool NeedConfirmationForCriticalOperation<T>(T status, string operationType) where T : struct
        {
            return GlobalStateRulesManager.Instance.NeedConfirmationForCriticalOperation(status, operationType);
        }

        /// <summary>
        /// 获取关键操作确认提示信息
        /// 用于UI层调用以显示适当的二次确认对话框
        /// </summary>
        /// <typeparam name="T">状态类型</typeparam>
        /// <param name="status">当前状态</param>
        /// <param name="operationType">操作类型</param>
        /// <returns>确认提示信息文本</returns>
        public string GetCriticalOperationConfirmationMessage<T>(T status, string operationType) where T : struct
        {
            return GlobalStateRulesManager.Instance.GetCriticalOperationConfirmationMessage(status, operationType);
        }

        /// <summary>
        /// 记录状态变更操作日志
        /// 用于记录所有关键状态变更，便于审计追踪
        /// </summary>
        /// <typeparam name="T">状态类型</typeparam>
        /// <param name="entityId">实体ID</param>
        /// <param name="entityType">实体类型名称</param>
        /// <param name="fromStatus">原始状态</param>
        /// <param name="toStatus">目标状态</param>
        /// <param name="operatorId">操作用户ID</param>
        /// <param name="operatorName">操作用户名称</param>
        /// <param name="remarks">备注信息</param>
        public void LogStatusChangeOperation<T>(long entityId, string entityType, T fromStatus, T toStatus, long operatorId, string operatorName, string remarks = "") where T : struct
        {
            GlobalStateRulesManager.Instance.LogStatusChangeOperation(entityId, entityType, fromStatus, toStatus, operatorId, operatorName, remarks);
        }

        /// <summary>
        /// 记录关键操作
        /// 用于记录非状态变更的关键操作，如删除、打印等
        /// </summary>
        /// <param name="entityId">实体ID</param>
        /// <param name="entityType">实体类型名称</param>
        /// <param name="operationType">操作类型</param>
        /// <param name="operatorId">操作用户ID</param>
        /// <param name="operatorName">操作用户名称</param>
        /// <param name="remarks">备注信息</param>
        public void LogCriticalOperation(long entityId, string entityType, string operationType, long operatorId, string operatorName, string remarks = "")
        {
            GlobalStateRulesManager.Instance.LogCriticalOperation(entityId, entityType, operationType, operatorId, operatorName, remarks);
        }

        /// <summary>
        /// 获取状态类型的描述信息
        /// </summary>
        /// <param name="statusType">状态类型</param>
        /// <returns>状态类型描述</returns>
        public string GetStatusTypeDescription(Type statusType)
        {
            return GlobalStateRulesManager.Instance.GetStatusTypeDescription(statusType);
        }

        /// <summary>
        /// 检查提交后是否允许修改
        /// 根据全局模式设置和状态判断
        /// </summary>
        /// <param name="isSubmittedStatus">是否为已提交状态</param>
        /// <returns>是否允许修改</returns>
        public bool AllowModifyAfterSubmit(bool isSubmittedStatus)
        {
            // 如果不是已提交状态，始终允许修改
            if (!isSubmittedStatus)
                return true;

            // 根据全局模式设置判断
            // 严格模式：提交后不允许修改
            // 灵活模式：提交后允许修改
            return GlobalStateRulesManager.Instance.submitModifyRuleMode == SubmitModifyRuleMode.灵活模式;
        }

        /// <summary>
        /// 处理审核驳回操作
        /// 根据实体状态类型自动转换为驳回后的状态（DataStatus转为草稿，财务状态转为草稿）
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="reason">驳回原因</param>
        /// <param name="userId">用户ID</param>
        /// <returns>状态转换结果</returns>
        public async Task<StateTransitionResult> HandleApprovalRejectAsync(BaseEntity entity, string reason = null, string userId = null)
        {
            var statusType = GetStatusType(entity);
            if (statusType == null)
            {
                return new StateTransitionResult
                {
                    IsSuccess = false,
                    ErrorMessage = "无法获取实体的状态类型"
                };
            }

            // 根据状态类型决定驳回后的目标状态
            object targetStatus = null;
            
            if (statusType == typeof(DataStatus))
            {
                // 业务单据：驳回后回到草稿
                targetStatus = DataStatus.草稿;
            }
            else if (statusType == typeof(PaymentStatus))
            {
                targetStatus = PaymentStatus.草稿;
            }
            else if (statusType == typeof(PrePaymentStatus))
            {
                targetStatus = PrePaymentStatus.草稿;
            }
            else if (statusType == typeof(ARAPStatus))
            {
                targetStatus = ARAPStatus.草稿;
            }
            else if (statusType == typeof(StatementStatus))
            {
                targetStatus = StatementStatus.草稿;
            }
            else
            {
                // 未知状态类型，尝试通过反射获取"草稿"状态值
                var draftStatusValue = statusType.GetEnumValues().Cast<object>()
                    .FirstOrDefault(v => v.ToString().Contains("草稿") || v.ToString().Equals("Draft", StringComparison.OrdinalIgnoreCase));
                
                if (draftStatusValue == null)
                {
                    return new StateTransitionResult
                    {
                        IsSuccess = false,
                        ErrorMessage = $"状态类型 {statusType.Name} 未定义草稿状态"
                    };
                }
                targetStatus = draftStatusValue;
            }

            // 设置状态
            return await SetBusinessStatusAsync(entity, statusType, targetStatus, reason ?? "审核驳回", userId);
        }

        #endregion

        #region 缓存管理

        /// <summary>
        /// 清除所有缓存
        /// 用于全局规则变更（如提交修改模式变更）时强制刷新缓存
        /// </summary>
        public void ClearAllCache()
        {
            _statusCacheManager?.ClearAllCache();
            _logger?.LogDebug("已清除所有状态管理缓存");
        }

        /// <summary>
        /// 清除指定状态类型的缓存
        /// </summary>
        /// <param name="statusType">状态类型</param>
        public void ClearCacheByStatusType(Type statusType)
        {
            if (statusType != null)
            {
                _statusCacheManager?.ClearCacheByStatusType(statusType);
                _logger?.LogDebug("已清除状态类型 {StatusType} 的缓存", statusType.Name);
            }
        }

        #endregion

        #region 释放资源

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            // 清除事件订阅
            StatusChanged = null;
        }

        #endregion
    }
}