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

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        public UnifiedStateManager(ILogger<UnifiedStateManager> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            // 初始化所有规则
            InitializeAllRules();
        }

        /// <summary>
        /// 初始化所有规则
        /// </summary>
        private void InitializeAllRules()
        {
            GlobalStateRulesManager.Instance.InitializeAllRules();
            // 初始化退款状态转换规则
            GlobalStateRulesManager.Instance.InitializeRefundStatusTransitionRules();
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

                // 默认返回DataStatus类型
                return typeof(DataStatus);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"获取状态类型失败: {ex.Message}");
                return typeof(DataStatus);
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

                // 状态相同检查 - 避免不必要的验证
                if (fromStatus.Equals(toStatus))
                    return StateTransitionResult.Allowed();

                // 验证两个枚举类型是否一致
                if (fromStatus.GetType() != toStatus.GetType())
                    return StateTransitionResult.Denied($"源状态类型和目标状态类型不一致");

                // 动态检查状态转换是否有效
                Type statusType = fromStatus.GetType();
                bool isAllowed = (bool)typeof(GlobalStateRulesManager)
                    .GetMethod("IsValidTransition")
                    .MakeGenericMethod(statusType)
                    .Invoke(GlobalStateRulesManager.Instance, new object[] { fromStatus, toStatus });

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
                // 生成缓存键
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
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="action">操作类型</param>
        /// <returns>验证结果</returns>
        public (bool CanExecute, string Message) CanExecuteActionWithMessage(BaseEntity entity, MenuItemEnums action)
        {
            if (entity == null)
                return (false, "实体不能为空");

            // 获取状态类型
            var statusType = GetStatusType(entity);
            
            // 获取当前状态
            var currentStatus = GetBusinessStatus(entity, statusType);
            
            // 根据操作类型采用不同的判断逻辑
            switch (action)
            {
                case MenuItemEnums.修改:
                    // 修改操作：检查当前状态是否允许修改
                    return CanModifyWithMessage(entity, currentStatus, statusType);
                    
                case MenuItemEnums.删除:
                    // 删除操作：检查当前状态是否允许删除
                    return CanDeleteWithMessage(entity, currentStatus, statusType);
                    
                case MenuItemEnums.保存:
                    // 保存操作：通常总是允许，除非是终态
                    return CanSaveWithMessage(entity, currentStatus, statusType);
                    
                default:
                    // 其他操作（提交、审核、反审等）：使用状态转换验证
                    return CanExecuteStateTransitionAction(entity, action, currentStatus, statusType);
            }
        }

        /// <summary>
        /// 验证状态转换类操作权限（提交、审核、反审等）
        /// </summary>
        private (bool CanExecute, string Message) CanExecuteStateTransitionAction(BaseEntity entity, MenuItemEnums action, object currentStatus, Type statusType)
        {
            // 获取目标状态
            var targetStatus = GlobalStateRulesManager.MapActionToStatus(entity, action);
            if (targetStatus == null)
                return (false, $"无法映射操作类型 '{action}' 到状态值");
            
            // 如果目标状态为当前状态，返回提示
            if (Equals(currentStatus, targetStatus))
                return (true, GetSuccessMessage(action));
            
            // 验证状态转换
            var validationResult = ValidateBusinessStatusTransitionAsync(currentStatus as Enum, targetStatus as Enum);
            
            // 根据操作类型和当前状态返回相应的消息
            if (validationResult.IsSuccess)
                return (true, GetSuccessMessage(action));
            else
                return (false, validationResult.ErrorMessage ?? "操作失败");
        }

        /// <summary>
        /// 验证修改操作权限
        /// </summary>
        private (bool CanExecute, string Message) CanModifyWithMessage(BaseEntity entity, object currentStatus, Type statusType)
        {
            // 检查是否为终态状态
            bool isFinalStatus = IsFinalStatus(entity);
            if (isFinalStatus)
                return (false, "终态状态下不允许修改");

            // 检查是否允许修改
            var canModify = CanExecuteAction(entity, MenuItemEnums.修改);
            
            // 检查提交后是否允许修改
            if (canModify && !GlobalStateRulesManager.Instance.AllowModifyAfterSubmit(canModify))
                return (false, "已提交状态下不允许修改");

            return (true, "可以修改当前记录");
        }

        /// <summary>
        /// 验证删除操作权限
        /// </summary>
        private (bool CanExecute, string Message) CanDeleteWithMessage(BaseEntity entity, object currentStatus, Type statusType)
        {
            // 检查是否为终态状态
            bool isFinalStatus = IsFinalStatus(entity);
            if (isFinalStatus)
                return (false, "终态状态下不允许删除");

            // 检查是否允许删除
            var canDelete = CanExecuteAction(entity, MenuItemEnums.删除);
            
            return canDelete ? (true, "可以删除当前记录") : (false, "当前状态下不允许删除");
        }

        /// <summary>
        /// 验证保存操作权限
        /// </summary>
        private (bool CanExecute, string Message) CanSaveWithMessage(BaseEntity entity, object currentStatus, Type statusType)
        {
            // 检查是否为终态状态
            bool isFinalStatus = IsFinalStatus(entity);
            if (isFinalStatus)
                return (false, "终态状态下不允许保存");

            return (true, "可以保存当前记录");
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
            
            // 获取目标状态
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
                    return prepayStatus == PrePaymentStatus.已结案;
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
        /// 判断指定实体是否可以修改，并返回详细消息
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <returns>是否可以修改及详细消息</returns>
        public (bool CanModify, string Message) CanModifyWithMessage<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            // 检查是否为终态状态
            bool isFinalStatus = IsFinalStatus<TEntity>(entity);
            if (isFinalStatus)
                return (false, "终态状态下不允许修改");

            var canModify = CanExecuteAction(entity, MenuItemEnums.修改);

            // 检查提交后是否允许修改
            if (canModify && !GlobalStateRulesManager.Instance.AllowModifyAfterSubmit(canModify))
                return (false, "已提交状态下不允许修改");

            return (true, "可以修改当前记录");
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