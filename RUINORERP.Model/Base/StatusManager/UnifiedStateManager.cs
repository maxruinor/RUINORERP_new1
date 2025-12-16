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
        /// 状态转换规则字典
        /// </summary>
        private Dictionary<Type, Dictionary<object, List<object>>> _transitionRules;

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

            // 复制规则字典到本地缓存
            _transitionRules = new Dictionary<Type, Dictionary<object, List<object>>>(GlobalStateRulesManager.Instance.StateTransitionRules.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToDictionary(innerKvp => innerKvp.Key, innerKvp => innerKvp.Value)));
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
        /// 获取实体的统一状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>统一状态</returns>
        public EntityStatus GetUnifiedStatus(BaseEntity entity)
        {
            if (entity == null)
                return null;

            var entityStatus = new EntityStatus();
            var statusType = entityStatus.GetStatusType(entity);
            if (statusType != null)
            {
                entityStatus.SetBusinessStatus(statusType, entity.GetPropertyValue(statusType.Name));
            }
            entityStatus.actionStatus = entity.ActionStatus;

            // 处理审批相关状态
            if (entity.ContainsProperty("ApprovalResults"))
            {
                entityStatus.ApprovalResults = entity.GetPropertyValue("ApprovalResults").ObjToBool();
            }
            if (entity.ContainsProperty("ApprovalStatus"))
            {
                entityStatus.ApprovalStatus = entity.GetPropertyValue("ApprovalStatus").ObjToInt();
            }
            return entityStatus;
        }

        /// <summary>
        /// 获取实体的状态类型
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>状态类型</returns>
        public Type GetStatusType(BaseEntity entity)
        {
            if (entity == null)
                return null;

            var entityStatus = new EntityStatus();
            return entityStatus.GetStatusType(entity);
        }

        /// <summary>
        /// 获取实体的业务状态（整合版本）
        /// </summary>
        /// <typeparam name="T">业务状态枚举类型（可选）</typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">业务状态类型（可选）</param>
        /// <returns>业务状态值</returns>
        public object GetBusinessStatus<T>(BaseEntity entity, Type statusType = null) where T : struct, Enum
        {
            if (entity == null)
                return default;

            // 如果没有指定状态类型，则使用泛型参数类型
            if (statusType == null)
                statusType = typeof(T);

            // 获取实体的状态类型
            var currentStatusType = new EntityStatus().GetStatusType(entity);

            // 如果指定了状态类型且与实体当前状态类型匹配，直接返回该状态
            if (statusType != null && currentStatusType == statusType)
            {
                return entity.GetPropertyValue(statusType.Name);
            }

            // 如果没有指定状态类型（或类型不匹配），返回当前状态类型的整数值
            if (currentStatusType != null)
            {
                try
                {
                    dynamic status = entity.GetPropertyValue(currentStatusType.Name);
                    return (int)status;
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
        /// <returns>业务状态值</returns>
        public object GetBusinessStatus(BaseEntity entity, Type statusType = null)
        {
            if (entity == null)
                return null;

            // 如果指定了状态类型且它是枚举类型，使用泛型方法
            if (statusType != null && statusType.IsEnum)
            {
                // 获取当前状态类型
                var currentStatusType = GetStatusType(entity);

                // 返回当前状态值
                if (currentStatusType != null)
                {
                    try
                    {
                        dynamic status = entity.GetPropertyValue(currentStatusType.Name);
                        return (int)status;
                    }
                    catch (Exception ex) when (_logger != null)
                    {
                        _logger.LogError(ex, "获取业务状态值时发生错误");
                    }
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
            object oldStatus = GetBusinessStatus(entity, statusType);
            // 自动转换：如果状态值是int且状态类型是枚举，则进行转换
            object OldStatusEnum = oldStatus;
            if (oldStatus is int && statusType.IsEnum)
            {
                try
                {
                    OldStatusEnum = Enum.ToObject(statusType, oldStatus);
                }
                catch
                {
                    // 转换失败时使用原始状态值
                }
            }

            // 检查状态是否实际发生了变更
            if (Equals(OldStatusEnum, newStatus))
                return StateTransitionResult.Success(OldStatusEnum, newStatus, statusType, "状态未发生变更");

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

        #endregion

        #region 操作权限检查方法

        /// <summary>
        /// 检查是否可以执行指定操作，并返回详细消息
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="action">操作类型</param>
        /// <returns>操作结果</returns>
        public (bool CanExecute, string Message) CanExecuteActionWithMessage(BaseEntity entity, MenuItemEnums action)
        {
            if (entity == null)
                return (false, "实体不能为空");

            // 使用增强规则检查操作权限
            var canExecute = CanExecuteAction(entity, action);

            // 根据操作类型和当前状态返回相应的消息
            if (canExecute)
                return (true, GetSuccessMessage(action));
            else
                return (false, GetFailureMessage(entity, action));
        }

        /// <summary>
        /// 使用增强规则检查操作权限
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="action">操作类型</param>
        /// <returns>是否可以执行</returns>
        private bool CanExecuteAction(BaseEntity entity, MenuItemEnums action)
        {
            // 获取实体的统一状态
            var entityStatus = GetUnifiedStatus(entity);

            // 如果没有状态信息，默认允许
            if (entityStatus == null)
                return true;

            // 获取当前状态类型和值
            var statusType = GetStatusType(entity);
            var statusValue = entityStatus;

            // 获取操作权限规则
            var allowedActions = GetActionPermissionRules(statusType, statusValue);

            // 检查当前状态是否允许执行指定操作
            return allowedActions.Contains(action);
        }

        /// <summary>
        /// 获取操作权限规则
        /// </summary>
        /// <param name="statusType">状态类型</param>
        /// <param name="statusValue">状态值</param>
        /// <returns>操作权限列表</returns>
        private List<MenuItemEnums> GetActionPermissionRules(Type statusType, object statusValue)
        {
            // 使用GlobalStateRulesManager中定义的规则
            return GlobalStateRulesManager.Instance.ActionPermissionRules.ContainsKey(statusType) &&
                   GlobalStateRulesManager.Instance.ActionPermissionRules[statusType].ContainsKey(statusValue) ?
                   GlobalStateRulesManager.Instance.ActionPermissionRules[statusType][statusValue] : new List<MenuItemEnums>();
        }

        /// <summary>
        /// 获取操作权限规则（基于DataStatus）
        /// </summary>
        /// <param name="status">数据状态</param>
        /// <returns>操作权限列表</returns>
        private List<MenuItemEnums> GetActionPermissionRules(DataStatus status)
        {
            // 使用GlobalStateRulesManager中定义的规则
            var statusType = typeof(DataStatus);
            return GlobalStateRulesManager.Instance.ActionPermissionRules.ContainsKey(statusType) &&
                   GlobalStateRulesManager.Instance.ActionPermissionRules[statusType].ContainsKey(status) ?
                   GlobalStateRulesManager.Instance.ActionPermissionRules[statusType][status] : new List<MenuItemEnums>();
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
        /// 获取操作失败消息
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="action">操作类型</param>
        /// <returns>失败消息</returns>
        private string GetFailureMessage(BaseEntity entity, MenuItemEnums action)
        {
            if (entity == null)
                return "实体不存在，无法执行操作";

            var statusType = GetStatusType(entity);
            if (statusType == null)
                return "当前状态不允许执行此操作";

            object statusValue = GetBusinessStatus(entity, statusType);

            if (statusType == typeof(DataStatus))
                return GetDataStatusFailureMessage((DataStatus)statusValue, action);

            return "当前状态不允许执行此操作";
        }

        /// <summary>
        /// 获取数据状态操作失败消息
        /// </summary>
        /// <param name="status">数据状态</param>
        /// <param name="action">操作类型</param>
        /// <returns>失败消息</returns>
        private string GetDataStatusFailureMessage(DataStatus status, MenuItemEnums action)
        {
            switch (status)
            {
                case DataStatus.草稿:
                    switch (action)
                    {
                        case MenuItemEnums.删除:
                            return "草稿状态下不能删除";
                        case MenuItemEnums.反审:
                            return "草稿状态下不能反审核";
                        case MenuItemEnums.关闭:
                            return "草稿状态下不能关闭";
                        default:
                            return "草稿状态下不能执行此操作";
                    }
                case DataStatus.新建:
                    switch (action)
                    {
                        case MenuItemEnums.删除:
                            return "已提交状态下不能删除";
                        case MenuItemEnums.修改:
                            return "已提交状态下不能编辑";
                        case MenuItemEnums.新增:
                            return "已提交状态下不能新建";
                        default:
                            return "已提交状态下不能执行此操作";
                    }
                case DataStatus.确认:
                    switch (action)
                    {
                        case MenuItemEnums.删除:
                            return "已审核状态下不能删除";
                        case MenuItemEnums.修改:
                            return "已审核状态下不能编辑";
                        case MenuItemEnums.提交:
                            return "已审核状态下不能提交";
                        default:
                            return "已审核状态下不能执行此操作";
                    }
                case DataStatus.完结:
                    switch (action)
                    {
                        case MenuItemEnums.删除:
                            return "已关闭状态下不能删除";
                        case MenuItemEnums.修改:
                            return "已关闭状态下不能编辑";
                        case MenuItemEnums.提交:
                            return "已关闭状态下不能提交";
                        case MenuItemEnums.审核:
                            return "已关闭状态下不能审核";
                        default:
                            return "已关闭状态下不能执行此操作";
                    }
                default:
                    return "当前状态不允许执行此操作";
            }
        }

        #endregion

        #region UI控制方法

        /// <summary>
        /// 获取UI控件状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>UI控件状态</returns>
        public Dictionary<string, bool> GetUIControlStates(BaseEntity entity)
        {
            try
            {
                // 获取实体的统一状态
                EntityStatus entityStatus = GetUnifiedStatus(entity);

                // 创建结果字典
                var result = new Dictionary<string, bool>();

                // 尝试获取状态规则管理器的按钮状态并转换格式
                var statusType = entityStatus.GetType();
                var buttonStates = GlobalStateRulesManager.Instance.UIButtonRules.TryGetValue(statusType, out var statusRules) &&
                                  statusRules.TryGetValue(entityStatus, out var buttonRules) ?
                                  buttonRules : new Dictionary<string, bool>();
                foreach (var state in buttonStates)
                {
                    // 使用已有的元组值
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

        /// <summary>
        /// 获取UI控件变更
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="action">操作类型</param>
        /// <returns>UI控件变更</returns>
        public Dictionary<string, bool> GetUIControlChanges(BaseEntity entity, MenuItemEnums action)
        {
            if (entity == null)
                return new Dictionary<string, bool>();

            // 获取当前UI状态
            var currentStates = GetUIControlStates(entity);

            // 模拟执行操作后的UI状态
            var postActionStates = new Dictionary<string, bool>();

            // 根据操作类型预测UI状态变更
            // 这里需要根据实际业务逻辑实现

            // 比较并返回变更
            var changes = new Dictionary<string, bool>();
            foreach (var state in currentStates)
            {
                if (postActionStates.TryGetValue(state.Key, out var newState) && state.Value != newState)
                {
                    changes[state.Key] = newState;
                }
            }

            return changes;
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
                // 获取业务状态类型
                Type businessStatusType = GetStatusType(entity);
                if (businessStatusType == null)
                    return false;

                // 获取业务状态值
                object businessStatus = GetBusinessStatus(entity, businessStatusType);
                if (businessStatus == null)
                    return false;

                // 使用反射调用GlobalStateRulesManager的IsFinalStatus方法
                var methodInfo = typeof(GlobalStateRulesManager).GetMethod("IsFinalStatus", BindingFlags.Public | BindingFlags.Instance);
                var genericMethod = methodInfo?.MakeGenericMethod(businessStatusType);
                return genericMethod != null && (bool)genericMethod.Invoke(GlobalStateRulesManager.Instance, new[] { businessStatus });
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "判断实体终态状态时发生异常");
                return false;
            }
        }

        /// <summary>
        /// 判断指定实体是否可以修改
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <returns>是否可以修改</returns>
        public bool CanModify<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            // 检查是否为终态状态
            bool isFinalStatus = IsFinalStatus(entity);
            if (isFinalStatus)
                return false;

            var canModify = CanExecuteAction(entity, MenuItemEnums.修改);

            // 检查提交后是否允许修改
            if (canModify && !GlobalStateRulesManager.Instance.AllowModifyAfterSubmit(canModify))
                return false;

            return true;
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

            // 清除状态缓存管理器中的相关缓存

            // 清理规则字典
            if (_transitionRules != null)
                _transitionRules.Clear();
        }

        #endregion
    }
}