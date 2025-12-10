using Microsoft.Extensions.Logging;
using RUINORERP.Global; // 添加此行以引用DataStatus枚举
using RUINORERP.Global.EnumExt;
using RUINORERP.Model.Base;
using RUINORERP.Model.Base.StatusManager;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace RUINORERP.Model.Base.StatusManager
{
    /// <summary>
    /// 统一状态管理器实现类 - 简化版
    /// 负责管理实体的各种状态转换，包括数据状态、业务状态和操作状态
    /// </summary>
    public class UnifiedStateManager : IUnifiedStateManager, IDisposable
    {
        #region 字段

        /// <summary>
        /// 日志记录器
        /// </summary>
        private readonly ILogger<UnifiedStateManager> _logger;

        /// <summary>
        /// 状态转换规则字典
        /// </summary>
        private readonly Dictionary<Type, Dictionary<object, List<object>>> _transitionRules;

        /// <summary>
        /// 缓存管理器
        /// </summary>
        private readonly StatusCacheManager _cacheManager;

        /// <summary>
        /// 状态变更锁
        /// </summary>
        private readonly object _statusChangeLock = new object();

        /// <summary>
        /// 最近的状态变更记录（用于去重）
        /// </summary>
        private readonly Dictionary<string, DateTime> _recentStatusChanges = new Dictionary<string, DateTime>();

        /// <summary>
        /// 是否已释放资源
        /// </summary>
        private bool _disposed = false;

        #endregion

        /// <summary>
        /// 状态变更事件
        /// </summary>
        public event EventHandler<StateTransitionEventArgs> StatusChanged;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="cacheManager">缓存管理器</param>
        public UnifiedStateManager(
            ILogger<UnifiedStateManager> logger,
            StatusCacheManager cacheManager = null)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _cacheManager = cacheManager ?? new StatusCacheManager();

            // 初始化状态转换规则
            _transitionRules = new Dictionary<Type, Dictionary<object, List<object>>>();
            StateTransitionRules.InitializeDefaultRules(_transitionRules);
        }

        #region 状态获取方法

        /// <summary>
        /// 获取实体的完整状态信息
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>实体状态信息</returns>
        public EntityStatus GetEntityStatus(BaseEntity entity)
        {
            if (entity == null)
                return null;

            return new EntityStatus
            {
                dataStatus = GetDataStatus(entity),
                BusinessStatuses = new Dictionary<Type, object>(),
                actionStatus = GetActionStatus(entity)
            };
        }

        /// <summary>
        /// 获取实体的数据状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>数据状态</returns>
        public DataStatus GetDataStatus(BaseEntity entity)
        {
            if (entity == null)
                return DataStatus.草稿;

            try
            {
                var property = entity.GetType().GetProperty("DataStatus");
                var value = property?.GetValue(entity);

                // 处理int类型到DataStatus枚举的转换
                if (value is int intValue)
                {
                    return (DataStatus)intValue;
                }

                return value as DataStatus? ?? DataStatus.草稿;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "获取数据状态失败：实体类型 {EntityType}", entity.GetType().Name);
                return DataStatus.草稿;
            }
        }


        /// <summary>获取实体的状态类型</summary>
        public Type GetStatusType(BaseEntity entity)
        {
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

            return null;
        }



        /// <summary>
        /// 获取实体的业务状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>业务状态</returns>
        public object GetBusinessStatus(BaseEntity entity)
        {
            if (entity == null)
                return null;
            try
            {
                var statusType = GetStatusType(entity);
                var property = entity.GetType().GetProperty(statusType.Name);
                return property?.GetValue(entity);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "获取业务状态失败：实体类型 {EntityType}", entity.GetType().Name);
                return null;
            }
        }

        /// <summary>
        /// 获取实体的业务状态（泛型版本）
        /// </summary>
        /// <typeparam name="T">业务状态枚举类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <returns>业务状态</returns>
        public T GetBusinessStatus<T>(BaseEntity entity) where T : struct, Enum
        {
            var status = GetBusinessStatus(entity);
            return status is T ? (T)status : default;
        }

        /// <summary>
        /// 获取实体的业务状态（指定类型版本）
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">业务状态类型</param>
        /// <returns>业务状态</returns>
        public object GetBusinessStatus(BaseEntity entity, Type statusType)
        {
            if (entity == null)
                return null;

            if (statusType == null)
                return null;

            try
            {
                // 首先尝试从实体的Status属性获取
                var property = entity.GetType().GetProperty(statusType.Name);
                var status = property?.GetValue(entity);

                // 如果获取到的状态类型与请求的类型匹配，直接返回
                if (status != null && status.GetType() == statusType)
                {
                    return status;
                }

                // 如果不匹配，尝试从EntityStatus中获取
                var entityStatusProperty = entity.GetType().GetProperty("EntityStatus");
                if (entityStatusProperty != null)
                {
                    var entityStatus = entityStatusProperty.GetValue(entity) as EntityStatus;
                    if (entityStatus != null && entityStatus.BusinessStatuses.TryGetValue(statusType, out var businessStatus))
                    {
                        return businessStatus;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "获取业务状态失败：实体类型 {EntityType}, 状态类型 {StatusType}", entity.GetType().Name, statusType.Name);
                return null;
            }
        }

        /// <summary>
        /// 获取实体的操作状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>操作状态</returns>
        public ActionStatus GetActionStatus(BaseEntity entity)
        {
            if (entity == null)
                return ActionStatus.无操作;

            try
            {
                var property = entity.GetType().GetProperty("ActionStatus");
                var value = property?.GetValue(entity);

                // 处理int类型到ActionStatus枚举的转换
                if (value is int intValue)
                {
                    return (ActionStatus)intValue;
                }

                return value as ActionStatus? ?? ActionStatus.无操作;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "获取操作状态失败：实体类型 {EntityType}", entity.GetType().Name);
                return ActionStatus.无操作;
            }
        }

        #endregion

        #region 状态验证方法

        /// <summary>
        /// 验证数据状态转换
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>验证结果</returns>
        public async Task<StateTransitionResult> ValidateDataStatusTransitionAsync(BaseEntity entity, DataStatus targetStatus)
        {
            if (entity == null)
                return StateTransitionResult.Failure("实体对象不能为空");

            try
            {
                var currentStatus = GetDataStatus(entity);

                // 直接使用StateTransitionRules验证转换
                if (StateTransitionRules.IsTransitionAllowed(_transitionRules, typeof(DataStatus), currentStatus, targetStatus))
                {
                    return StateTransitionResult.Success();
                }
                else
                {
                    return StateTransitionResult.Failure($"不允许从 {currentStatus} 转换到 {targetStatus}");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "验证数据状态转换失败：实体类型 {EntityType}, 目标状态 {TargetStatus}", entity.GetType().Name, targetStatus);
                return StateTransitionResult.Failure($"验证数据状态转换时发生错误: {ex.Message}");
            }
        }

        /// <summary>
        /// 验证业务状态转换
        /// </summary>
        /// <typeparam name="T">业务状态枚举类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>验证结果</returns>
        public async Task<StateTransitionResult> ValidateBusinessStatusTransitionAsync<T>(BaseEntity entity, T targetStatus) where T : struct, Enum
        {
            if (entity == null)
                return StateTransitionResult.Failure("实体对象不能为空");

            try
            {
                var currentStatus = GetBusinessStatus<T>(entity);

                // 直接使用StateTransitionRules验证转换
                if (StateTransitionRules.IsTransitionAllowed(_transitionRules, typeof(T), currentStatus, targetStatus))
                {
                    return StateTransitionResult.Success();
                }
                else
                {
                    return StateTransitionResult.Failure($"不允许从 {currentStatus} 转换到 {targetStatus}");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "验证业务状态转换失败：实体类型 {EntityType}, 目标状态 {TargetStatus}", entity.GetType().Name, targetStatus);
                return StateTransitionResult.Failure($"验证业务状态转换时发生错误: {ex.Message}");
            }
        }

        /// <summary>
        /// 验证业务状态转换
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">状态类型</param>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>验证结果</returns>
        public async Task<StateTransitionResult> ValidateBusinessStatusTransitionAsync(BaseEntity entity, Type statusType, object targetStatus)
        {
            if (entity == null)
                return StateTransitionResult.Failure("实体对象不能为空");

            if (statusType == null)
                return StateTransitionResult.Failure("状态类型不能为空");

            try
            {
                var currentStatus = GetBusinessStatus(entity, statusType);

                // 直接使用StateTransitionRules验证转换
                if (currentStatus is Enum currentEnumStatus && targetStatus is Enum targetEnumStatus &&
                    StateTransitionRules.IsTransitionAllowed(_transitionRules, statusType, currentEnumStatus, targetEnumStatus))
                {
                    return StateTransitionResult.Success();
                }
                else
                {
                    return StateTransitionResult.Failure($"不允许从 {currentStatus} 转换到 {targetStatus}");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "验证业务状态转换失败：实体类型 {EntityType}, 目标状态 {TargetStatus}", entity.GetType().Name, targetStatus);
                return StateTransitionResult.Failure($"验证业务状态转换时发生错误: {ex.Message}");
            }
        }

        /// <summary>
        /// 验证操作状态转换
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>验证结果</returns>
        public async Task<StateTransitionResult> ValidateActionStatusTransitionAsync(BaseEntity entity, ActionStatus targetStatus)
        {
            if (entity == null)
                return StateTransitionResult.Failure("实体对象不能为空");

            try
            {
                var currentStatus = GetActionStatus(entity);

                // 直接使用StateTransitionRules验证转换
                if (StateTransitionRules.IsTransitionAllowed(_transitionRules, typeof(ActionStatus), currentStatus, targetStatus))
                {
                    return StateTransitionResult.Success();
                }
                else
                {
                    return StateTransitionResult.Failure($"不允许从 {currentStatus} 转换到 {targetStatus}");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "验证操作状态转换失败：实体类型 {EntityType}, 目标状态 {TargetStatus}", entity.GetType().Name, targetStatus);
                return StateTransitionResult.Failure($"验证操作状态转换时发生错误: {ex.Message}");
            }
        }

        /// <summary>
        /// 检查是否可以转换到目标数据状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>是否可以转换</returns>
        public async Task<bool> CanTransitionToDataStatus(BaseEntity entity, DataStatus targetStatus)
        {
            var result = await ValidateDataStatusTransitionAsync(entity, targetStatus);
            return result.IsSuccess;
        }

        /// <summary>
        /// 检查是否可以转换到目标业务状态
        /// </summary>
        /// <typeparam name="T">业务状态枚举类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>是否可以转换</returns>
        public async Task<bool> CanTransitionToBusinessStatus<T>(BaseEntity entity, T targetStatus) where T : struct, Enum
        {
            var result = await ValidateBusinessStatusTransitionAsync<T>(entity, targetStatus);
            return result.IsSuccess;
        }

        /// <summary>
        /// 检查是否可以转换到目标操作状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>是否可以转换</returns>
        public async Task<bool> CanTransitionToActionStatus(BaseEntity entity, ActionStatus targetStatus)
        {
            var result = await ValidateActionStatusTransitionAsync(entity, targetStatus);
            return result.IsSuccess;
        }

        /// <summary>
        /// 获取可转换的数据状态列表
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>可转换的状态列表</returns>
        public IEnumerable<DataStatus> GetAvailableDataStatusTransitions(BaseEntity entity)
        {
            if (entity == null)
                return Enumerable.Empty<DataStatus>();

            try
            {
                var currentStatus = GetDataStatus(entity);
                var availableStatuses = new List<DataStatus>();

                // 获取所有可能的状态
                var allStatuses = Enum.GetValues(typeof(DataStatus)).Cast<DataStatus>();

                // 检查每个状态是否可以转换
                foreach (var status in allStatuses)
                {
                    if (currentStatus is Enum currentEnumStatus && status is Enum enumStatus &&
                        StateTransitionRules.IsTransitionAllowed(_transitionRules, typeof(DataStatus), currentEnumStatus, enumStatus))
                    {
                        availableStatuses.Add(status);
                    }
                }

                return availableStatuses;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "获取可转换的数据状态列表失败：实体类型 {EntityType}", entity.GetType().Name);
                return Enumerable.Empty<DataStatus>();
            }
        }

        /// <summary>
        /// 获取可转换的业务状态列表
        /// </summary>
        /// <typeparam name="T">业务状态枚举类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <returns>可转换的状态列表</returns>
        public IEnumerable<T> GetAvailableBusinessStatusTransitions<T>(BaseEntity entity) where T : struct, Enum
        {
            if (entity == null)
                return Enumerable.Empty<T>();

            try
            {
                var currentStatus = GetBusinessStatus<T>(entity);
                var availableStatuses = new List<T>();

                // 获取所有可能的状态
                var allStatuses = Enum.GetValues(typeof(T)).Cast<T>();

                // 检查每个状态是否可以转换
                foreach (var status in allStatuses)
                {
                    if (currentStatus is Enum currentEnumStatus && status is Enum enumStatus &&
                        StateTransitionRules.IsTransitionAllowed(_transitionRules, typeof(T), currentEnumStatus, enumStatus))
                    {
                        availableStatuses.Add(status);
                    }
                }

                return availableStatuses;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "获取可转换的业务状态列表失败：实体类型 {EntityType}", entity.GetType().Name);
                return Enumerable.Empty<T>();
            }
        }

        /// <summary>
        /// 获取可转换的业务状态列表
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">状态类型</param>
        /// <returns>可转换的状态列表</returns>
        public IEnumerable<object> GetAvailableBusinessStatusTransitions(BaseEntity entity, Type statusType)
        {
            if (entity == null || statusType == null)
                return Enumerable.Empty<object>();

            try
            {
                var currentStatus = GetBusinessStatus(entity, statusType);
                var availableStatuses = new List<object>();

                // 获取所有可能的状态
                var allStatuses = Enum.GetValues(statusType).Cast<object>();

                // 检查每个状态是否可以转换
                foreach (var status in allStatuses)
                {
                    if (StateTransitionRules.IsTransitionAllowed(_transitionRules, statusType, currentStatus as Enum, status as Enum))
                    {
                        availableStatuses.Add(status);
                    }
                }

                return availableStatuses;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "获取可转换的业务状态列表失败：实体类型 {EntityType}", entity.GetType().Name);
                return Enumerable.Empty<object>();
            }
        }

        /// <summary>
        /// 获取可转换的操作状态列表
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>可转换的状态列表</returns>
        public IEnumerable<ActionStatus> GetAvailableActionStatusTransitions(BaseEntity entity)
        {
            if (entity == null)
                return Enumerable.Empty<ActionStatus>();

            try
            {
                var currentStatus = GetActionStatus(entity);
                var availableStatuses = new List<ActionStatus>();

                // 获取所有可能的状态
                var allStatuses = Enum.GetValues(typeof(ActionStatus)).Cast<ActionStatus>();

                // 检查每个状态是否可以转换
                foreach (var status in allStatuses)
                {
                    if (StateTransitionRules.IsTransitionAllowed(_transitionRules, typeof(ActionStatus), currentStatus, status))
                    {
                        availableStatuses.Add(status);
                    }
                }

                return availableStatuses;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "获取可转换的操作状态列表失败：实体类型 {EntityType}", entity.GetType().Name);
                return Enumerable.Empty<ActionStatus>();
            }
        }

        #endregion

        #region 状态更新和事件方法

        /// <summary>
        /// 更新实体状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">状态类型</param>
        /// <param name="status">状态值</param>
        private void UpdateEntityStatus(BaseEntity entity, Type statusType, object status)
        {
            if (entity == null || statusType == null || status == null)
                return;

            try
            {
                // 根据状态类型更新对应的属性
                if (statusType == typeof(DataStatus))
                {
                    var property = entity.GetType().GetProperty("DataStatus");
                    if (property != null && property.CanWrite)
                    {
                        // 如果属性是int类型，需要转换枚举为int
                        if (property.PropertyType == typeof(int))
                        {
                            property.SetValue(entity, Convert.ToInt32(status));
                        }
                        else
                        {
                            property.SetValue(entity, status);
                        }
                    }
                }
                else if (statusType == typeof(ActionStatus))
                {
                    var property = entity.GetType().GetProperty("ActionStatus");
                    if (property != null && property.CanWrite)
                    {
                        // 如果属性是int类型，需要转换枚举为int
                        if (property.PropertyType == typeof(int))
                        {
                            property.SetValue(entity, Convert.ToInt32(status));
                        }
                        else
                        {
                            property.SetValue(entity, status);
                        }
                    }
                }
                else
                {
                    // 业务状态处理
                    var property = entity.GetType().GetProperty("Status");
                    if (property != null && property.CanWrite)
                    {
                        property.SetValue(entity, status);
                    }

                    // 同时更新EntityStatus中的业务状态
                    var entityStatusProperty = entity.GetType().GetProperty("EntityStatus");
                    if (entityStatusProperty != null)
                    {
                        var entityStatus = entityStatusProperty.GetValue(entity) as EntityStatus;
                        if (entityStatus != null)
                        {
                            entityStatus.BusinessStatuses[statusType] = status;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "更新实体状态失败：实体类型 {EntityType}, 状态类型 {StatusType}", entity.GetType().Name, statusType.Name);
            }
        }

        /// <summary>
        /// 触发状态变更事件
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">状态类型</param>
        /// <param name="oldStatus">旧状态</param>
        /// <param name="newStatus">新状态</param>
        /// <param name="reason">变更原因</param>
        /// <summary>
        /// 触发状态变更事件
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">状态类型</param>
        /// <param name="oldStatus">旧状态</param>
        /// <param name="newStatus">新状态</param>
        /// <param name="reason">变更原因</param>
        public void TriggerStatusChangedEvent(BaseEntity entity, Type statusType, object oldStatus, object newStatus, string reason)
        {
            try
            {
                StatusChanged?.Invoke(this, new StateTransitionEventArgs(
                    entity,
                    statusType,
                    oldStatus,
                    newStatus,
                    reason,
                    userId: null,
                    changeTime: DateTime.Now,
                    additionalData: null));
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "触发状态变更事件失败：实体类型 {EntityType}, 状态类型 {StatusType}", entity.GetType().Name, statusType.Name);
            }
        }

        #endregion

        #region 状态设置方法

        /// <summary>
        /// 设置实体的数据状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="status">状态值</param>
        /// <param name="reason">变更原因</param>
        /// <returns>设置是否成功</returns>
        public async Task<bool> SetDataStatusAsync(BaseEntity entity, DataStatus status, string reason = null)
        {
            if (entity == null)
                return false;

            try
            {
                var currentStatus = GetDataStatus(entity);
                if (currentStatus == status)
                    return true; // 状态未变化，直接返回成功

                // 验证状态转换
                var validationResult = await ValidateDataStatusTransitionAsync(entity, status);
                if (!validationResult.IsSuccess)
                {
                    _logger?.LogWarning("数据状态转换验证失败：{ErrorMessage}", validationResult.ErrorMessage);
                    return false;
                }

                // 创建状态转换上下文
                var context = new StatusTransitionContext(entity, typeof(DataStatus), currentStatus, this);

                // 直接使用StateTransitionRules验证转换
                if (StateTransitionRules.IsTransitionAllowed(_transitionRules, typeof(DataStatus), currentStatus, status))
                {
                    // 更新实体状态
                    UpdateEntityStatus(entity, typeof(DataStatus), status);

                    // 集中触发状态变更事件 - 仅在UnifiedStateManager中触发
                    TriggerStatusChangedEvent(entity, typeof(DataStatus), currentStatus, status, reason);

                    return true;
                }
                else
                {
                    _logger?.LogWarning("数据状态转换失败：不允许从 {CurrentStatus} 转换到 {TargetStatus}", currentStatus, status);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "设置数据状态失败：实体类型 {EntityType}, 目标状态 {TargetStatus}", entity.GetType().Name, status);
                return false;
            }
        }

        /// <summary>
        /// 设置实体数据状态（同步版本）
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="status">状态值</param>
        public void SetEntityDataStatus(BaseEntity entity, DataStatus status)
        {
            if (entity == null)
                return;

            try
            {
                // 获取当前状态
                var currentStatus = GetDataStatus(entity);

                // 如果状态没有变化，直接返回
                if (currentStatus == status)
                    return;

                var property = entity.GetType().GetProperty("DataStatus");
                if (property != null && property.CanWrite)
                {
                    // 如果属性是int类型，需要转换枚举为int
                    if (property.PropertyType == typeof(int))
                    {
                        property.SetValue(entity, Convert.ToInt32(status));
                    }
                    else
                    {
                        property.SetValue(entity, status);
                    }

                    // 集中触发状态变更事件 - 仅在UnifiedStateManager中触发
                    TriggerStatusChangedEvent(entity, typeof(DataStatus), currentStatus, status, "直接设置数据状态");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "设置实体数据状态失败：实体类型 {EntityType}, 目标状态 {TargetStatus}", entity.GetType().Name, status);
            }
        }

        /// <summary>
        /// 设置实体的业务状态
        /// </summary>
        /// <typeparam name="T">业务状态枚举类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="status">状态值</param>
        /// <param name="reason">变更原因</param>
        /// <returns>设置是否成功</returns>
        public async Task<bool> SetBusinessStatusAsync<T>(BaseEntity entity, T status, string reason = null) where T : struct, Enum
        {
            if (entity == null)
                return false;

            try
            {
                var currentStatus = GetBusinessStatus<T>(entity);
                if (EqualityComparer<T>.Default.Equals(currentStatus, status))
                    return true; // 状态未变化，直接返回成功

                // 验证状态转换
                var validationResult = await ValidateBusinessStatusTransitionAsync(entity, status);
                if (!validationResult.IsSuccess)
                {
                    _logger?.LogWarning("业务状态转换验证失败：{ErrorMessage}", validationResult.ErrorMessage);
                    return false;
                }

                // 创建状态转换上下文
                var context = new StatusTransitionContext(entity, typeof(T), currentStatus, this);

                // 直接使用StateTransitionRules验证转换
                if (StateTransitionRules.IsTransitionAllowed(_transitionRules, typeof(T), currentStatus, status))
                {
                    // 更新实体状态
                    UpdateEntityStatus(entity, typeof(T), status);

                    // 集中触发状态变更事件 - 仅在UnifiedStateManager中触发
                    TriggerStatusChangedEvent(entity, typeof(T), currentStatus, status, reason);

                    return true;
                }
                else
                {
                    _logger?.LogWarning("业务状态转换失败：不允许从 {CurrentStatus} 转换到 {TargetStatus}", currentStatus, status);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "设置业务状态失败：实体类型 {EntityType}, 目标状态 {TargetStatus}", entity.GetType().Name, status);
                return false;
            }
        }

        /// <summary>
        /// 设置实体的业务状态（非泛型版本）
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">业务状态枚举类型</param>
        /// <param name="status">状态值</param>
        /// <param name="reason">变更原因</param>
        /// <returns>设置是否成功</returns>
        public async Task<bool> SetBusinessStatusAsync(BaseEntity entity, Type statusType, object status, string reason = null)
        {
            if (entity == null)
                return false;

            if (statusType == null)
                throw new ArgumentNullException(nameof(statusType));

            if (!statusType.IsEnum)
                throw new ArgumentException("statusType必须是枚举类型", nameof(statusType));

            try
            {
                var currentStatus = GetBusinessStatus(entity, statusType);
                if (Equals(currentStatus, status))
                    return true; // 状态未变化，直接返回成功

                // 验证状态转换
                var validationResult = await ValidateBusinessStatusTransitionAsync(entity, statusType, status);
                if (!validationResult.IsSuccess)
                {
                    _logger?.LogWarning("业务状态转换验证失败：{ErrorMessage}", validationResult.ErrorMessage);
                    return false;
                }

                // 创建状态转换上下文
                var context = new StatusTransitionContext(entity, statusType, currentStatus, this);

                // 直接使用StateTransitionRules验证转换
                if (StateTransitionRules.IsTransitionAllowed(_transitionRules, statusType, currentStatus as Enum, status as Enum))
                {
                    // 更新实体状态
                    UpdateEntityStatus(entity, statusType, status);

                    // 集中触发状态变更事件 - 仅在UnifiedStateManager中触发
                    TriggerStatusChangedEvent(entity, statusType, currentStatus, status, reason);

                    return true;
                }
                else
                {
                    _logger?.LogWarning("业务状态转换失败：不允许从 {CurrentStatus} 转换到 {TargetStatus}", currentStatus, status);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "设置业务状态失败：实体类型 {EntityType}, 目标状态 {TargetStatus}", entity.GetType().Name, status);
                return false;
            }
        }

        /// <summary>
        /// 设置实体的操作状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="status">状态值</param>
        /// <param name="reason">变更原因</param>
        /// <returns>设置是否成功</returns>
        public async Task<bool> SetActionStatusAsync(BaseEntity entity, ActionStatus status, string reason = null)
        {
            if (entity == null)
                return false;

            try
            {
                var currentStatus = GetActionStatus(entity);
                if (currentStatus == status)
                    return true; // 状态未变化，直接返回成功

                // 验证状态转换
                var validationResult = await ValidateActionStatusTransitionAsync(entity, status);
                if (!validationResult.IsSuccess)
                {
                    _logger?.LogWarning("操作状态转换验证失败：{ErrorMessage}", validationResult.ErrorMessage);
                    return false;
                }

                // 创建状态转换上下文
                var context = new StatusTransitionContext(entity, typeof(ActionStatus), currentStatus, this);

                // 直接使用StateTransitionRules验证转换
                if (StateTransitionRules.IsTransitionAllowed(_transitionRules, typeof(ActionStatus), currentStatus, status))
                {
                    // 更新实体状态
                    UpdateEntityStatus(entity, typeof(ActionStatus), status);

                    // 集中触发状态变更事件 - 仅在UnifiedStateManager中触发
                    TriggerStatusChangedEvent(entity, typeof(ActionStatus), currentStatus, status, reason);

                    return true;
                }
                else
                {
                    _logger?.LogWarning("操作状态转换失败：不允许从 {CurrentStatus} 转换到 {TargetStatus}", currentStatus, status);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "设置操作状态失败：实体类型 {EntityType}, 目标状态 {TargetStatus}", entity.GetType().Name, status);
                return false;
            }
        }

        /// <summary>
        /// 执行状态转换
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="currentStatus">当前状态</param>
        /// <param name="serviceProvider">服务提供程序</param>
        /// <param name="reason">转换原因</param>
        /// <returns>状态转换上下文</returns>
        public virtual IStatusTransitionContext CreateDataStatusContext(object entity, DataStatus currentStatus, IServiceProvider serviceProvider, string reason = null)
        {
            try
            {
                var context = new StatusTransitionContext(
                    entity as BaseEntity,
                    typeof(DataStatus),
                    currentStatus,
                    this,
                    null); // 传递null，因为不再需要transitionEngine

                return context;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "创建数据状态转换上下文失败");
                throw;
            }
        }

        /// <summary>
        /// 创建业务状态转换上下文
        /// </summary>
        /// <typeparam name="TBusinessStatus">业务状态类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="currentStatus">当前状态</param>
        /// <param name="serviceProvider">服务提供程序</param>
        /// <param name="reason">转换原因</param>
        /// <returns>状态转换上下文</returns>
        public virtual IStatusTransitionContext CreateBusinessStatusContext<TBusinessStatus>(object entity, TBusinessStatus currentStatus, IServiceProvider serviceProvider, string reason = null)
        {
            try
            {
                var context = new StatusTransitionContext(
                    entity as BaseEntity,
                    typeof(TBusinessStatus),
                    currentStatus,
                    this,
                    null); // 传递null，因为不再需要transitionEngine

                return context;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "创建业务状态转换上下文失败");
                throw;
            }
        }

        /// <summary>
        /// 创建操作状态转换上下文
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="currentStatus">当前状态</param>
        /// <param name="serviceProvider">服务提供程序</param>
        /// <param name="reason">转换原因</param>
        /// <returns>状态转换上下文</returns>
        public virtual IStatusTransitionContext CreateActionStatusContext(object entity, ActionStatus currentStatus, IServiceProvider serviceProvider, string reason = null)
        {
            try
            {
                var context = new StatusTransitionContext(
                    entity as BaseEntity,
                    typeof(ActionStatus),
                    currentStatus,
                    this,
                    null); // 传递null，因为不再需要transitionEngine

                return context;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "创建操作状态转换上下文失败");
                throw;
            }
        }

        /// <summary>
        /// 创建UI更新状态转换上下文
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="currentStatus">当前状态</param>
        /// <param name="serviceProvider">服务提供程序</param>
        /// <param name="reason">转换原因</param>
        /// <returns>状态转换上下文</returns>
        public virtual IStatusTransitionContext CreateUIUpdateContext(object entity, DataStatus currentStatus, IServiceProvider serviceProvider, string reason = null)
        {
            try
            {
                var context = new StatusTransitionContext(
                    entity as BaseEntity,
                    typeof(DataStatus),
                    currentStatus,
                    this,
                    null); // 传递null，因为不再需要transitionEngine

                return context;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "创建UI更新状态转换上下文失败");
                throw;
            }
        }

        /// <summary>
        /// 创建通用状态转换上下文
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">状态类型</param>
        /// <param name="currentStatus">当前状态</param>
        /// <param name="serviceProvider">服务提供程序</param>
        /// <param name="reason">转换原因</param>
        /// <returns>状态转换上下文</returns>
        public virtual IStatusTransitionContext CreateContext(object entity, Type statusType, object currentStatus, IServiceProvider serviceProvider, string reason = null)
        {
            try
            {
                var context = new StatusTransitionContext(
                    entity as BaseEntity,
                    statusType,
                    currentStatus,
                    this,
                    null); // 传递null，因为不再需要transitionEngine

                return context;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "创建通用状态转换上下文失败");
                throw;
            }
        }

        /// <summary>
        /// 清理状态缓存
        /// </summary>
        public virtual void ClearCache()
        {
            try
            {
                // 直接使用缓存管理器清理缓存
                _cacheManager.ClearAllCache();
                _logger.LogInformation("状态缓存已清理");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "清理状态缓存失败");
                throw;
            }
        }

        /// <summary>
        /// 检查是否可以执行指定操作（带详细消息）
        /// </summary>
        /// <param name="stateManager">状态管理器</param>
        /// <param name="statusType">状态类型</param>
        /// <param name="status">当前状态</param>
        /// <param name="action">操作类型</param>
        /// <returns>包含操作权限和友好提示消息的结果</returns>
        public StateTransitionResult CanExecuteActionWithMessage(IUnifiedStateManager stateManager, BaseEntity entity, MenuItemEnums action)
        {
            try
            {
                // 获取当前实体的状态类型和状态值
                var statusType = entity.GetStatusType();
                var currentStatus = entity.GetCurrentStatus();
                
                if (currentStatus is Enum statusEnum)
                {
                    // 使用增强的规则检查
                    bool canExecute = CanExecuteActionWithEnhancedRules(action, entity, statusType, statusEnum);
                    
                    // 生成友好的提示消息
                    string message = canExecute ? GetSuccessMessage(action) : GetFailureMessage(action, GetDataStatus(entity));

                    return canExecute
                        ? StateTransitionResult.Success(message)
                        : StateTransitionResult.Failure(message);
                }
                else
                {
                    return StateTransitionResult.Failure($"不支持的状态类型: {statusType.Name}");
                }
            }
            catch (Exception ex)
            {
                return StateTransitionResult.Failure($"检查操作权限时发生错误: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 获取操作权限规则
        /// </summary>
        /// <returns>操作权限规则字典</returns>
        private Dictionary<DataStatus, List<MenuItemEnums>> GetActionPermissionRules()
        {
            // 定义操作权限规则
            var rules = new Dictionary<DataStatus, List<MenuItemEnums>>
            {
                [DataStatus.草稿] = new List<MenuItemEnums>
                {
                    MenuItemEnums.新增,
                    MenuItemEnums.修改,
                    MenuItemEnums.删除,
                    MenuItemEnums.保存,
                    MenuItemEnums.提交
                },
                [DataStatus.新建] = new List<MenuItemEnums>
                {
                    MenuItemEnums.修改,
                    MenuItemEnums.删除,
                    MenuItemEnums.保存,
                    MenuItemEnums.提交,
                    MenuItemEnums.审核
                },
                [DataStatus.确认] = new List<MenuItemEnums>
                {
                    MenuItemEnums.反审,
                    MenuItemEnums.结案
                },
                [DataStatus.完结] = new List<MenuItemEnums>(),
                [DataStatus.作废] = new List<MenuItemEnums>()
            };

            return rules;
        }

        /// <summary>
        /// 获取操作成功的提示消息
        /// </summary>
        /// <param name="action">操作类型</param>
        /// <returns>成功提示消息</returns>
        private string GetSuccessMessage(MenuItemEnums action)
        {
            switch (action)
            {
                case MenuItemEnums.新增:
                    return "可以新增当前单据";
                case MenuItemEnums.修改:
                    return "可以修改当前单据";
                case MenuItemEnums.删除:
                    return "可以删除当前单据";
                case MenuItemEnums.保存:
                    return "可以保存当前单据";
                case MenuItemEnums.提交:
                    return "可以提交当前单据";
                case MenuItemEnums.审核:
                    return "可以审核当前单据";
                case MenuItemEnums.反审:
                    return "可以反审核当前单据";
                case MenuItemEnums.结案:
                    return "可以结案当前单据";
                default:
                    return "可以执行当前操作";
            }
        }

        /// <summary>
        /// 获取操作失败的提示消息
        /// </summary>
        /// <param name="action">操作类型</param>
        /// <param name="dataStatus">当前数据状态</param>
        /// <returns>失败提示消息</returns>
        private string GetFailureMessage(MenuItemEnums action, DataStatus dataStatus)
        {
            // 基本状态检查失败消息
            string basicStatusMessage = GetBasicStatusFailureMessage(action, dataStatus);
            if (!string.IsNullOrEmpty(basicStatusMessage))
            {
                return basicStatusMessage;
            }
            
            // 根据操作类型返回更具体的失败消息
            switch (action)
            {
                case MenuItemEnums.新增:
                    return dataStatus == DataStatus.草稿 || dataStatus == DataStatus.完结
                        ? "可以新增当前单据"
                        : $"只有草稿状态或完结状态的单据才能新增，当前状态：{dataStatus}";

                case MenuItemEnums.修改:
                    return GetModifyFailureMessage(dataStatus);

                case MenuItemEnums.删除:
                    return GetDeleteFailureMessage(dataStatus);

                case MenuItemEnums.保存:
                    return GetSaveFailureMessage(dataStatus);

                case MenuItemEnums.提交:
                    return GetSubmitFailureMessage(dataStatus);

                case MenuItemEnums.审核:
                    return GetApproveFailureMessage(dataStatus);

                case MenuItemEnums.反审:
                    return GetUnapproveFailureMessage(dataStatus);

                case MenuItemEnums.结案:
                    return GetCloseFailureMessage(dataStatus);

                default:
                    return "无法执行当前操作";
            }
        }

        /// <summary>
        /// 获取基本状态检查失败的提示消息
        /// </summary>
        /// <param name="action">操作类型</param>
        /// <param name="dataStatus">当前数据状态</param>
        /// <returns>失败提示消息</returns>
        private string GetBasicStatusFailureMessage(MenuItemEnums action, DataStatus dataStatus)
        {
            switch (action)
            {
                case MenuItemEnums.修改:
                case MenuItemEnums.删除:
                case MenuItemEnums.保存:
                    if (dataStatus != DataStatus.草稿 && dataStatus != DataStatus.新建)
                    {
                        return $"只有草稿状态或新建状态的单据才能{action}，当前状态：{dataStatus}";
                    }
                    break;

                case MenuItemEnums.提交:
                    if (dataStatus != DataStatus.草稿)
                    {
                        return $"只有草稿状态的单据才能提交，当前状态：{dataStatus}";
                    }
                    break;

                case MenuItemEnums.审核:
                    if (dataStatus != DataStatus.新建)
                    {
                        return $"只有新建状态的单据才能审核，当前状态：{dataStatus}";
                    }
                    break;

                case MenuItemEnums.反审:
                case MenuItemEnums.结案:
                    if (dataStatus != DataStatus.确认)
                    {
                        return $"只有确认状态的单据才能{action}，当前状态：{dataStatus}";
                    }
                    break;

                case MenuItemEnums.反结案:
                    if (dataStatus != DataStatus.完结)
                    {
                        return $"只有完结状态的单据才能反结案，当前状态：{dataStatus}";
                    }
                    break;
            }

            return string.Empty;
        }

        /// <summary>
        /// 获取修改操作失败的提示消息
        /// </summary>
        /// <param name="dataStatus">当前数据状态</param>
        /// <returns>失败提示消息</returns>
        private string GetModifyFailureMessage(DataStatus dataStatus)
        {
            if (dataStatus != DataStatus.草稿 && dataStatus != DataStatus.新建)
            {
                return $"只有草稿状态或新建状态的单据才能修改，当前状态：{dataStatus}";
            }

            return "当前单据已被锁定，无法修改";
        }

        /// <summary>
        /// 获取删除操作失败的提示消息
        /// </summary>
        /// <param name="dataStatus">当前数据状态</param>
        /// <returns>失败提示消息</returns>
        private string GetDeleteFailureMessage(DataStatus dataStatus)
        {
            if (dataStatus != DataStatus.草稿 && dataStatus != DataStatus.新建)
            {
                return $"只有草稿状态或新建状态的单据才能删除，当前状态：{dataStatus}";
            }

            return "当前单据已被关联或使用，无法删除";
        }

        /// <summary>
        /// 获取保存操作失败的提示消息
        /// </summary>
        /// <param name="dataStatus">当前数据状态</param>
        /// <returns>失败提示消息</returns>
        private string GetSaveFailureMessage(DataStatus dataStatus)
        {
            if (dataStatus != DataStatus.草稿 && dataStatus != DataStatus.新建)
            {
                return $"只有草稿状态或新建状态的单据才能保存，当前状态：{dataStatus}";
            }

            return "当前单据没有变更，无需保存";
        }

        /// <summary>
        /// 获取提交操作失败的提示消息
        /// </summary>
        /// <param name="dataStatus">当前数据状态</param>
        /// <returns>失败提示消息</returns>
        private string GetSubmitFailureMessage(DataStatus dataStatus)
        {
            if (dataStatus != DataStatus.草稿)
            {
                return $"只有草稿状态的单据才能提交，当前状态：{dataStatus}";
            }

            return "当前单据信息不完整，无法提交";
        }

        /// <summary>
        /// 获取审核操作失败的提示消息
        /// </summary>
        /// <param name="dataStatus">当前数据状态</param>
        /// <returns>失败提示消息</returns>
        private string GetApproveFailureMessage(DataStatus dataStatus)
        {
            if (dataStatus != DataStatus.新建)
            {
                return $"只有新建状态的单据才能审核，当前状态：{dataStatus}";
            }

            return "当前单据不满足审核条件";
        }

        /// <summary>
        /// 获取反审核操作失败的提示消息
        /// </summary>
        /// <param name="dataStatus">当前数据状态</param>
        /// <returns>失败提示消息</returns>
        private string GetUnapproveFailureMessage(DataStatus dataStatus)
        {
            if (dataStatus != DataStatus.确认)
            {
                return $"只有确认状态的单据才能反审核，当前状态：{dataStatus}";
            }

            return "当前单据已完成后续业务流程（如支付、发货等），无法反审核";
        }

        /// <summary>
        /// 获取结案操作失败的提示消息
        /// </summary>
        /// <param name="dataStatus">当前数据状态</param>
        /// <returns>失败提示消息</returns>
        private string GetCloseFailureMessage(DataStatus dataStatus)
        {
            if (dataStatus != DataStatus.确认)
            {
                return $"只有确认状态的单据才能结案，当前状态：{dataStatus}";
            }

            return "当前单据尚有未完成的业务流程（如待支付、部分支付等），无法结案";
        }

        /// <summary>
        /// 检查是否可以执行指定操作
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="action">操作类型</param>
        /// <returns>是否可以执行</returns>
        public virtual bool CanExecuteAction<TEntity>(TEntity entity, MenuItemEnums action) where TEntity : class
        {
            if (entity == null)
                return false;

            try
            {
                // 如果是BaseEntity，使用现有的方法
                if (entity is BaseEntity baseEntity)
                {
                    return CanExecuteAction(action, baseEntity, typeof(DataStatus), GetDataStatus(baseEntity));
                }

                // 对于非BaseEntity类型，默认允许操作
                // 在实际应用中，可能需要根据具体业务逻辑进行调整
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "检查操作权限失败：操作 {Action}，实体类型 {EntityType}", action, entity.GetType().Name);
                return false;
            }
        }

        /// <summary>
        /// 检查是否可以执行指定操作
        /// </summary>
        /// <param name="action">操作类型</param>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">状态类型</param>
        /// <param name="status">当前状态</param>
        /// <returns>是否可以执行操作</returns>
        public virtual bool CanExecuteAction(MenuItemEnums action, BaseEntity entity, Type statusType, object status)
        {
            if (entity == null || status == null)
                return false;

            try
            {
                // 优先使用UIControlRules检查按钮权限
                if (statusType == typeof(DataStatus) && status is DataStatus dataStatus)
                {
                    var buttonRules = UIControlRules.GetButtonRules(dataStatus);

                    // 将MenuItemEnums转换为按钮名称
                    var buttonName = ConvertActionToButtonName(action);
                    if (!string.IsNullOrEmpty(buttonName) &&
                        buttonRules.TryGetValue(buttonName, out var buttonState))
                    {
                        return buttonState.Enabled;
                    }
                }

                // 如果UIControlRules没有相关规则，使用增强的规则检查
                return CanExecuteActionWithEnhancedRules(action, entity, statusType, status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "检查操作权限失败：操作 {Action}，实体类型 {EntityType}", action, entity.GetType().Name);
                return false;
            }
        }

        /// <summary>
        /// 使用增强的规则检查是否可以执行指定操作
        /// </summary>
        /// <param name="action">操作类型</param>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">状态类型</param>
        /// <param name="status">当前状态</param>
        /// <returns>是否可以执行操作</returns>
        private bool CanExecuteActionWithEnhancedRules(MenuItemEnums action, BaseEntity entity, Type statusType, object status)
        {
            // 获取当前数据状态
            var currentDataStatus = GetDataStatus(entity);
            
            // 获取操作权限规则
            var actionRules = GetActionPermissionRules();

            // 检查当前状态是否允许执行该操作
            bool canExecute = actionRules.TryGetValue(currentDataStatus, out var allowedActions) &&
                             allowedActions.Contains(action);

            // 如果基本规则允许，进行更详细的业务规则检查
            if (canExecute)
            {
                canExecute = CheckBusinessSpecificRules(action, entity, currentDataStatus);
            }

            return canExecute;
        }

        /// <summary>
        /// 检查业务特定的规则
        /// </summary>
        /// <param name="action">操作类型</param>
        /// <param name="entity">实体对象</param>
        /// <param name="currentDataStatus">当前数据状态</param>
        /// <returns>是否可以执行操作</returns>
        private bool CheckBusinessSpecificRules(MenuItemEnums action, BaseEntity entity, DataStatus currentDataStatus)
        {
            // 根据操作类型进行特定的业务规则检查
            switch (action)
            {
                case MenuItemEnums.修改:
                    return CanModifyEntity(entity, currentDataStatus);
                    
                case MenuItemEnums.删除:
                    return CanDeleteEntity(entity, currentDataStatus);
                    
                case MenuItemEnums.保存:
                    return CanSaveEntity(entity, currentDataStatus);
                    
                case MenuItemEnums.提交:
                    return CanSubmitEntity(entity, currentDataStatus);
                    
                case MenuItemEnums.审核:
                    return CanApproveEntity(entity, currentDataStatus);
                    
                case MenuItemEnums.反审:
                    return CanUnapproveEntity(entity, currentDataStatus);
                    
                case MenuItemEnums.结案:
                    return CanCloseEntity(entity, currentDataStatus);
                    
                case MenuItemEnums.反结案:
                    return CanUncloseEntity(entity, currentDataStatus);
                    
                default:
                    // 对于其他操作，使用默认规则
                    return true;
            }
        }

        /// <summary>
        /// 检查是否可以修改实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="currentDataStatus">当前数据状态</param>
        /// <returns>是否可以修改</returns>
        private bool CanModifyEntity(BaseEntity entity, DataStatus currentDataStatus)
        {
            // 基本状态检查
            if (currentDataStatus != DataStatus.草稿 && currentDataStatus != DataStatus.新建)
            {
                return false;
            }

            // 检查是否有特殊业务状态限制修改
            var businessStatuses = entity.StatusInfo?.BusinessStatuses;
            if (businessStatuses != null)
            {
                // 检查是否有锁定状态
                foreach (var businessStatus in businessStatuses)
                {
                    if (businessStatus.Value != null && businessStatus.Value.ToString().Contains("锁定"))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 检查是否可以删除实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="currentDataStatus">当前数据状态</param>
        /// <returns>是否可以删除</returns>
        private bool CanDeleteEntity(BaseEntity entity, DataStatus currentDataStatus)
        {
            // 基本状态检查
            if (currentDataStatus != DataStatus.草稿 && currentDataStatus != DataStatus.新建)
            {
                return false;
            }

            // 检查是否有特殊业务状态限制删除
            var businessStatuses = entity.StatusInfo?.BusinessStatuses;
            if (businessStatuses != null)
            {
                // 检查是否有已关联状态，已关联的单据不能删除
                foreach (var businessStatus in businessStatuses)
                {
                    if (businessStatus.Value != null && 
                        (businessStatus.Value.ToString().Contains("已关联") || 
                         businessStatus.Value.ToString().Contains("已使用")))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 检查是否可以保存实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="currentDataStatus">当前数据状态</param>
        /// <returns>是否可以保存</returns>
        private bool CanSaveEntity(BaseEntity entity, DataStatus currentDataStatus)
        {
            // 基本状态检查
            if (currentDataStatus != DataStatus.草稿 && currentDataStatus != DataStatus.新建)
            {
                return false;
            }

            // 检查实体是否有变更
            if (!entity.HasChanged)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 检查是否可以提交实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="currentDataStatus">当前数据状态</param>
        /// <returns>是否可以提交</returns>
        private bool CanSubmitEntity(BaseEntity entity, DataStatus currentDataStatus)
        {
            // 基本状态检查
            if (currentDataStatus != DataStatus.草稿)
            {
                return false;
            }

            // 检查必填字段是否完整
            if (!IsEntityCompleteForSubmission(entity))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 检查是否可以审核实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="currentDataStatus">当前数据状态</param>
        /// <returns>是否可以审核</returns>
        private bool CanApproveEntity(BaseEntity entity, DataStatus currentDataStatus)
        {
            // 基本状态检查
            if (currentDataStatus != DataStatus.新建)
            {
                return false;
            }

            // 检查是否有待审核业务状态
            var businessStatuses = entity.StatusInfo?.BusinessStatuses;
            if (businessStatuses != null)
            {
                foreach (var businessStatus in businessStatuses)
                {
                    if (businessStatus.Value != null && businessStatus.Value.ToString().Contains("待审核"))
                    {
                        return true;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 检查是否可以反审核实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="currentDataStatus">当前数据状态</param>
        /// <returns>是否可以反审核</returns>
        private bool CanUnapproveEntity(BaseEntity entity, DataStatus currentDataStatus)
        {
            // 基本状态检查
            if (currentDataStatus != DataStatus.确认)
            {
                return false;
            }

            // 检查是否有后续业务流程限制反审核
            var businessStatuses = entity.StatusInfo?.BusinessStatuses;
            if (businessStatuses != null)
            {
                foreach (var businessStatus in businessStatuses)
                {
                    // 已支付、已发货、已入库等状态不能反审核
                    if (businessStatus.Value != null && 
                        (businessStatus.Value.ToString().Contains("已支付") || 
                         businessStatus.Value.ToString().Contains("已发货") ||
                         businessStatus.Value.ToString().Contains("已入库")))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 检查是否可以结案实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="currentDataStatus">当前数据状态</param>
        /// <returns>是否可以结案</returns>
        private bool CanCloseEntity(BaseEntity entity, DataStatus currentDataStatus)
        {
            // 基本状态检查
            if (currentDataStatus != DataStatus.确认)
            {
                return false;
            }

            // 检查是否满足结案条件
            var businessStatuses = entity.StatusInfo?.BusinessStatuses;
            if (businessStatuses != null)
            {
                foreach (var businessStatus in businessStatuses)
                {
                    // 检查是否有未完成的业务流程
                    if (businessStatus.Value != null && 
                        (businessStatus.Value.ToString().Contains("待支付") || 
                         businessStatus.Value.ToString().Contains("部分支付") ||
                         businessStatus.Value.ToString().Contains("待发货")))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 检查是否可以反结案实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="currentDataStatus">当前数据状态</param>
        /// <returns>是否可以反结案</returns>
        private bool CanUncloseEntity(BaseEntity entity, DataStatus currentDataStatus)
        {
            // 基本状态检查
            if (currentDataStatus != DataStatus.完结)
            {
                return false;
            }

            // 检查是否有权限反结案
            // 这里可以添加更复杂的权限检查逻辑
            return true;
        }

        /// <summary>
        /// 检查实体是否满足提交条件（必填字段等）
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>是否满足提交条件</returns>
        private bool IsEntityCompleteForSubmission(BaseEntity entity)
        {
            // 这里可以添加更复杂的业务逻辑检查
            // 例如：检查必填字段是否完整、业务规则是否满足等
            
            // 基本检查：实体必须有变更
            if (!entity.HasChanged)
            {
                return false;
            }

            // TODO: 添加更详细的业务逻辑检查
            // 可以通过反射检查实体是否有[Required]标记的属性
            // 或者根据特定业务类型进行不同的检查
            
            return true;
        }

        /// <summary>
        /// 将MenuItemEnums操作转换为按钮名称
        /// </summary>
        /// <param name="action">操作类型</param>
        /// <returns>按钮名称</returns>
        private string ConvertActionToButtonName(MenuItemEnums action)
        {
            switch (action)
            {
                case MenuItemEnums.新增:
                    return "toolStripbtnAdd";
                case MenuItemEnums.修改:
                    return "toolStripbtnModify";
                case MenuItemEnums.保存:
                    return "toolStripButtonSave";
                case MenuItemEnums.删除:
                    return "toolStripbtnDelete";
                case MenuItemEnums.提交:
                    return "toolStripbtnSubmit";
                case MenuItemEnums.审核:
                    return "toolStripbtnReview";
                case MenuItemEnums.反审:
                    return "toolStripBtnReverseReview";
                case MenuItemEnums.结案:
                    return "toolStripButtonCaseClosed";
                case MenuItemEnums.反结案:
                    return "toolStripButtonAntiClosed";
                case MenuItemEnums.打印:
                    return "toolStripbtnPrint";
                case MenuItemEnums.导出:
                    return "toolStripButtonExport";
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// 获取可用的操作列表（带详细消息）
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>可执行的操作列表及其状态消息</returns>
        public virtual Dictionary<MenuItemEnums, string> GetAvailableActionsWithMessages(BaseEntity entity)
        {
            var result = new Dictionary<MenuItemEnums, string>();
            
            if (entity == null)
                return result;

            try
            {
                // 获取当前实体的状态类型和状态值
                var statusType = entity.GetStatusType();
                var currentStatus = entity.GetCurrentStatus();
                
                if (currentStatus is Enum statusEnum)
                {
                    // 获取所有可能的操作
                    var allActions = Enum.GetValues(typeof(MenuItemEnums)).Cast<MenuItemEnums>();
                    
                    foreach (var action in allActions)
                    {
                        // 使用增强的规则检查
                        bool canExecute = CanExecuteActionWithEnhancedRules(action, entity, statusType, statusEnum);
                        
                        // 生成友好的提示消息
                        string message = canExecute ? GetSuccessMessage(action) : GetFailureMessage(action, GetDataStatus(entity));
                        
                        result[action] = message;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取可用操作列表失败：实体类型 {EntityType}", entity.GetType().Name);
            }

            return result;
        }

        /// <summary>
        /// 获取可用的操作列表
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">状态类型</param>
        /// <param name="status">当前状态</param>
        /// <returns>可执行的操作列表</returns>
        public virtual IEnumerable<MenuItemEnums> GetAvailableActions(BaseEntity entity, Type statusType, object status)
        {
            if (entity == null || status == null)
                return Enumerable.Empty<MenuItemEnums>();

            try
            {
                // 优先使用UIControlRules获取可用操作
                if (statusType == typeof(DataStatus) && status is DataStatus dataStatus)
                {
                    var buttonRules = UIControlRules.GetButtonRules(dataStatus);
                    var availableActions = new List<MenuItemEnums>();

                    // 将按钮规则转换为MenuItemEnums
                    foreach (var rule in buttonRules.Where(r => r.Value.Enabled))
                    {
                        var action = ConvertButtonNameToAction(rule.Key);
                        if (action.HasValue)
                        {
                            availableActions.Add(action.Value);
                        }
                    }

                    if (availableActions.Any())
                    {
                        return availableActions;
                    }
                }

                // 如果UIControlRules没有相关规则，使用增强的规则检查
                var allActions = Enum.GetValues(typeof(MenuItemEnums)).Cast<MenuItemEnums>();
                var lastAvailableActions = new List<MenuItemEnums>();

                foreach (var action in allActions)
                {
                    if (CanExecuteActionWithEnhancedRules(action, entity, statusType, status))
                    {
                        lastAvailableActions.Add(action);
                    }
                }

                return lastAvailableActions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取可用操作列表失败：实体类型 {EntityType}", entity.GetType().Name);
                return Enumerable.Empty<MenuItemEnums>();
            }
        }

        /// <summary>
        /// 将按钮名称转换为MenuItemEnums操作
        /// </summary>
        /// <param name="buttonName">按钮名称</param>
        /// <returns>操作类型</returns>
        private MenuItemEnums? ConvertButtonNameToAction(string buttonName)
        {
            switch (buttonName)
            {
                case "toolStripbtnAdd":
                    return MenuItemEnums.新增;
                case "toolStripbtnModify":
                    return MenuItemEnums.修改;
                case "toolStripButtonSave":
                    return MenuItemEnums.保存;
                case "toolStripbtnDelete":
                    return MenuItemEnums.删除;
                case "toolStripbtnSubmit":
                    return MenuItemEnums.提交;
                case "toolStripbtnReview":
                    return MenuItemEnums.审核;
                case "toolStripBtnReverseReview":
                    return MenuItemEnums.反审;
                case "toolStripButtonCaseClosed":
                    return MenuItemEnums.结案;
                case "toolStripButtonAntiClosed":
                    return MenuItemEnums.反结案;
                case "toolStripbtnPrint":
                    return MenuItemEnums.打印;
                case "toolStripButtonExport":
                    return MenuItemEnums.导出;
                default:
                    return null;
            }
        }

        ///// <summary>
        ///// 获取实体在特定状态下的所有可用转换
        ///// </summary>
        ///// <param name="entity">实体对象</param>
        ///// <param name="statusType">状态类型</param>
        ///// <param name="status">当前状态</param>
        ///// <returns>可转换到的状态列表</returns>
        //public virtual IEnumerable<object> GetAvailableTransitions(BaseEntity entity, Type statusType, object status)
        //{
        //    if (entity == null || status == null)
        //        return Enumerable.Empty<object>();

        //    try
        //    {
        //        // 获取状态转换规则
        //        var transitionRules = GetTransitionRules();
                
        //        // 根据状态类型获取转换规则
        //        if (statusType == typeof(DataStatus) && status is DataStatus dataStatus)
        //        {
        //            return StateTransitionRules.GetAvailableTransitions(transitionRules, dataStatus);
        //        }
        //        else if (statusType == typeof(ActionStatus) && status is ActionStatus actionStatus)
        //        {
        //            return StateTransitionRules.GetAvailableTransitions(transitionRules, actionStatus);
        //        }
        //        else if (statusType.IsEnum && status is Enum enumStatus)
        //        {
        //            // 对于其他枚举类型，尝试使用通用方法
        //            return StateTransitionRules.GetAvailableTransitions(transitionRules, enumStatus);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "获取可用转换列表失败：实体类型 {EntityType}，状态类型 {StatusType}", 
        //            entity.GetType().Name, statusType?.Name);
        //    }

        //    return Enumerable.Empty<object>();
        //}

        /// <summary>
        /// 检查实体是否可以执行指定的UI操作
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="action">要执行的操作</param>
        /// <returns>是否可以执行操作</returns>
        public virtual bool CanExecuteAction<TEntity>(BaseEntity entity, MenuItemEnums action) where TEntity : class
        {
            try
            {
                // 获取当前实体的状态类型和状态值
                var statusType = entity.GetStatusType();
                var currentStatus = entity.GetCurrentStatus();
                
                if (currentStatus is Enum statusEnum)
                {
                    // 使用增强的规则检查
                    return CanExecuteActionWithEnhancedRules(action, entity, statusType, statusEnum);
                }
                
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "检查操作权限失败：实体类型 {EntityType}，操作 {Action}", 
                    typeof(TEntity).Name, action);
                return false;
            }
        }

        #endregion

        #region IDisposable实现

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放资源的具体实现
        /// </summary>
        /// <param name="disposing">是否正在释放托管资源</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // 释放托管资源 - 不调用接口的Dispose方法，因为它们可能没有实现
                    // 不再需要释放_transitionEngine资源
                    // _ruleConfiguration?.Dispose();
                }

                _disposed = true;
            }
        }

        #endregion
    }
}