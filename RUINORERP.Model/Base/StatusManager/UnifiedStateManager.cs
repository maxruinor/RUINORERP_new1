/**
 * 文件: UnifiedStateManager.cs
 * 说明: 简化版统一状态管理器 - 整合状态管理功能
 * 创建日期: 2024年
 * 作者: RUINOR ERP开发团队
 */

using RUINORERP.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace RUINORERP.Model.Base.StatusManager
{
    /// <summary>
    /// 简化版统一状态管理器 - 整合状态管理功能
    /// 移除了复杂的反射缓存，简化了状态访问逻辑
    /// </summary>
    public class UnifiedStateManager : IUnifiedStateManager, IDisposable
    {
        #region 字段和属性

        /// <summary>
        /// 日志记录器
        /// </summary>
        private readonly ILogger<UnifiedStateManager> _logger;

        /// <summary>
        /// 状态转换引擎
        /// </summary>
        private readonly IStatusTransitionEngine _transitionEngine;

        /// <summary>
        /// 状态管理选项
        /// </summary>
        private readonly StateManagerOptions _options;

        /// <summary>
        /// 是否已释放资源
        /// </summary>
        private bool _disposed = false;

        /// <summary>
        /// 状态变更事件
        /// </summary>
        public event EventHandler<StateTransitionEventArgs> StatusChanged;

        #endregion

        #region 构造函数

        /// <summary>
        /// 初始化统一状态管理器
        /// </summary>
        /// <param name="options">状态管理选项</param>
        /// <param name="logger">日志记录器</param>
        /// <param name="transitionEngine">状态转换引擎</param>
        public UnifiedStateManager(
            StateManagerOptions options = null,
            ILogger<UnifiedStateManager> logger = null,
            IStatusTransitionEngine transitionEngine = null)
        {
            _options = options ?? StateManagerOptions.CreateDefault();
            _logger = logger;
            _transitionEngine = transitionEngine;
        }

        #endregion

        #region IUnifiedStateManager 接口实现

        /// <summary>
        /// 获取实体的状态信息
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>实体状态信息</returns>
        public EntityStatus GetEntityStatus(BaseEntity entity)
        {
            if (entity == null)
                return null;

            try
            {
                var dataStatus = GetDataStatus(entity);
                var actionStatus = GetActionStatus(entity);

                return new EntityStatus
                {
                    dataStatus = dataStatus,
                    actionStatus = actionStatus
                };
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "获取实体状态失败：实体类型 {EntityType}", entity.GetType().Name);
                return null;
            }
        }

        /// <summary>
        /// 获取当前数据性状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>当前数据性状态</returns>
        public DataStatus GetDataStatus(BaseEntity entity)
        {
            if (entity == null)
                return DataStatus.草稿;

            try
            {
                // 首先检查是否有DataStatus属性
                var dataStatusProperty = entity.GetType().GetProperty("DataStatus");
                if (dataStatusProperty != null && dataStatusProperty.PropertyType == typeof(DataStatus))
                {
                    return (DataStatus)dataStatusProperty.GetValue(entity);
                }

                // 检查是否有Status属性
                var statusProperty = entity.GetType().GetProperty("Status");
                if (statusProperty != null && statusProperty.PropertyType == typeof(DataStatus))
                {
                    return (DataStatus)statusProperty.GetValue(entity);
                }

                // 默认返回草稿状态
                return DataStatus.草稿;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "获取实体数据状态失败：实体类型 {EntityType}", entity.GetType().Name);
                return DataStatus.草稿;
            }
        }

        /// <summary>
        /// 设置数据状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="status">数据状态</param>
        /// <param name="reason">状态变更原因</param>
        /// <returns>设置是否成功</returns>
        public async Task<bool> SetDataStatusAsync(BaseEntity entity, DataStatus status, string reason = null)
        {
            if (entity == null)
                return false;

            try
            {
                var result = await TransitionDataStatusAsync(entity, status);
                return result.IsSuccess;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "设置数据状态失败：实体类型 {EntityType}, 状态 {Status}", entity.GetType().Name, status);
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
                var oldStatus = GetDataStatus(entity);
                
                // 首先尝试设置DataStatus属性
                var dataStatusProperty = entity.GetType().GetProperty("DataStatus");
                if (dataStatusProperty != null && dataStatusProperty.PropertyType == typeof(DataStatus))
                {
                    dataStatusProperty.SetValue(entity, status);
                    return;
                }

                // 尝试设置Status属性
                var statusProperty = entity.GetType().GetProperty("Status");
                if (statusProperty != null && statusProperty.PropertyType == typeof(DataStatus))
                {
                    statusProperty.SetValue(entity, status);
                    return;
                }

                _logger?.LogWarning("无法设置实体数据状态：实体类型 {EntityType} 没有有效的状态属性", entity.GetType().Name);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "设置实体数据状态失败：实体类型 {EntityType}, 状态 {Status}", entity.GetType().Name, status);
            }
        }

        /// <summary>
        /// 获取业务性状态
        /// </summary>
        /// <typeparam name="T">业务状态枚举类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <returns>业务性状态</returns>
        public T GetBusinessStatus<T>(BaseEntity entity) where T : Enum
        {
            if (entity == null)
                return default;

            try
            {
                var statusType = typeof(T);
                var statusProperty = entity.GetType().GetProperties()
                    .FirstOrDefault(p => p.PropertyType == statusType);

                if (statusProperty != null)
                {
                    return (T)statusProperty.GetValue(entity);
                }

                return default;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "获取业务状态失败：实体类型 {EntityType}, 状态类型 {StatusType}", entity.GetType().Name, typeof(T).Name);
                return default;
            }
        }

        /// <summary>
        /// 设置业务性状态
        /// </summary>
        /// <typeparam name="T">业务状态枚举类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="status">状态值</param>
        /// <param name="reason">变更原因</param>
        /// <returns>设置是否成功</returns>
        public async Task<bool> SetBusinessStatusAsync<T>(BaseEntity entity, T status, string reason = null) where T : Enum
        {
            if (entity == null)
                return false;

            try
            {
                var currentStatus = GetBusinessStatus<T>(entity);

                // 检查状态是否需要变更
                if (EqualityComparer<T>.Default.Equals(currentStatus, status))
                    return true;

                // 验证状态转换
                var validationResult = await ValidateBusinessStatusTransitionAsync(entity, status);
                if (!validationResult.IsSuccess)
                {
                    _logger?.LogWarning("业务状态转换验证失败：{ErrorMessage}", validationResult.ErrorMessage);
                    return false;
                }

                // 设置状态
                var statusType = typeof(T);
                var statusProperty = entity.GetType().GetProperties()
                    .FirstOrDefault(p => p.PropertyType == statusType);

                if (statusProperty != null)
                {
                    statusProperty.SetValue(entity, status);

                    // 触发状态变更事件
                    OnStatusChanged(new StateTransitionEventArgs(
                        entity,
                        statusType,
                        currentStatus,
                        status,
                        reason));

                    return true;
                }

                _logger?.LogWarning("无法设置业务状态：实体类型 {EntityType} 没有类型为 {StatusType} 的属性", entity.GetType().Name, statusType.Name);
                return false;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "设置业务状态失败：实体类型 {EntityType}, 目标状态 {Status}", entity.GetType().Name, status);
                return false;
            }
        }

        /// <summary>
        /// 获取当前操作状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>当前操作状态</returns>
        public ActionStatus GetActionStatus(BaseEntity entity)
        {
            if (entity == null)
                return ActionStatus.无操作;

            try
            {
                var actionStatusProperty = entity.GetType().GetProperty("ActionStatus");
                if (actionStatusProperty != null && actionStatusProperty.PropertyType == typeof(ActionStatus))
                {
                    return (ActionStatus)actionStatusProperty.GetValue(entity);
                }

                return ActionStatus.无操作;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "获取操作状态失败：实体类型 {EntityType}", entity.GetType().Name);
                return ActionStatus.无操作;
            }
        }

        /// <summary>
        /// 设置操作状态
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

                // 检查状态是否需要变更
                if (currentStatus == status)
                    return true;

                // 验证状态转换
                var validationResult = await ValidateActionStatusTransitionAsync(entity, status);
                if (!validationResult.IsSuccess)
                {
                    _logger?.LogWarning("操作状态转换验证失败：{ErrorMessage}", validationResult.ErrorMessage);
                    return false;
                }

                // 设置状态
                var actionStatusProperty = entity.GetType().GetProperty("ActionStatus");
                if (actionStatusProperty != null && actionStatusProperty.PropertyType == typeof(ActionStatus))
                {
                    actionStatusProperty.SetValue(entity, status);

                    // 触发状态变更事件
                    OnStatusChanged(new StateTransitionEventArgs(
                        entity,
                        typeof(ActionStatus),
                        currentStatus,
                        status,
                        reason));

                    return true;
                }

                _logger?.LogWarning("无法设置操作状态：实体类型 {EntityType} 没有ActionStatus属性", entity.GetType().Name);
                return false;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "设置操作状态失败：实体类型 {EntityType}, 目标状态 {Status}", entity.GetType().Name, status);
                return false;
            }
        }

        /// <summary>
        /// 验证数据性状态转换
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
                // 创建状态转换上下文，不传递日志记录器参数，让StatusTransitionContext自己创建
                var context = new StatusTransitionContext(entity, typeof(DataStatus), currentStatus, this, _transitionEngine);
                return await _transitionEngine.ValidateTransitionAsync(currentStatus, targetStatus, context);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "验证数据状态转换失败：实体类型 {EntityType}, 当前状态 -> 目标状态 {TargetStatus}", entity.GetType().Name, targetStatus);
                return StateTransitionResult.Failure($"验证数据状态转换失败：{ex.Message}");
            }
        }

        /// <summary>
        /// 验证业务性状态转换
        /// </summary>
        /// <typeparam name="T">业务状态枚举类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>验证结果</returns>
        public async Task<StateTransitionResult> ValidateBusinessStatusTransitionAsync<T>(BaseEntity entity, T targetStatus) where T : Enum
        {
            if (entity == null)
                return StateTransitionResult.Failure("实体对象不能为空");

            try
            {
                var currentStatus = GetBusinessStatus<T>(entity);
                // 创建状态转换上下文，不传递日志记录器参数，让StatusTransitionContext自己创建
                var context = new StatusTransitionContext(entity, typeof(T), currentStatus, this, _transitionEngine);
                return await _transitionEngine.ValidateTransitionAsync(currentStatus, targetStatus, context);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "验证业务状态转换失败：实体类型 {EntityType}, 当前状态 -> 目标状态 {TargetStatus}", entity.GetType().Name, targetStatus);
                return StateTransitionResult.Failure($"验证业务状态转换失败：{ex.Message}");
            }
        }

        /// <summary>
        /// 验证业务性状态转换
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">状态类型</param>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>验证结果</returns>
        public async Task<StateTransitionResult> ValidateBusinessStatusTransitionAsync(BaseEntity entity, Type statusType, object targetStatus)
        {
            if (entity == null)
                return StateTransitionResult.Failure("实体对象不能为空");

            try
            {
                // 使用反射获取当前状态
                var currentStatusProperty = entity.GetType().GetProperties()
                    .FirstOrDefault(p => p.PropertyType == statusType);

                if (currentStatusProperty == null)
                {
                    return StateTransitionResult.Failure($"实体类型 {entity.GetType().Name} 没有类型为 {statusType.Name} 的属性");
                }

                var currentStatus = currentStatusProperty.GetValue(entity);
                // 创建状态转换上下文，不传递日志记录器参数，让StatusTransitionContext自己创建
                var context = new StatusTransitionContext(entity, statusType, currentStatus, this, _transitionEngine);
                
                // 根据状态类型调用相应的泛型方法
                if (statusType.IsEnum)
                {
                    var method = _transitionEngine.GetType().GetMethod(nameof(_transitionEngine.ValidateTransitionAsync))
                        .MakeGenericMethod(statusType);
                    return await (Task<StateTransitionResult>)method.Invoke(_transitionEngine, new object[] { currentStatus, targetStatus, context });
                }
                else
                {
                    return await _transitionEngine.ValidateTransitionAsync((Enum)currentStatus, (Enum)targetStatus, context);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "验证业务状态转换失败：实体类型 {EntityType}, 状态类型 {StatusType}, 当前状态 -> 目标状态 {TargetStatus}", entity.GetType().Name, statusType.Name, targetStatus);
                return StateTransitionResult.Failure($"验证业务状态转换失败：{ex.Message}");
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
                // 创建状态转换上下文，不传递日志记录器参数，让StatusTransitionContext自己创建
                var context = new StatusTransitionContext(entity, typeof(ActionStatus), currentStatus, this, _transitionEngine);
                return await _transitionEngine.ValidateTransitionAsync(currentStatus, targetStatus, context);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "验证操作状态转换失败：实体类型 {EntityType}, 当前状态 -> 目标状态 {TargetStatus}", entity.GetType().Name, targetStatus);
                return StateTransitionResult.Failure($"验证操作状态转换失败：{ex.Message}");
            }
        }

        /// <summary>
        /// 获取可转换的数据性状态列表
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
                // 创建状态转换上下文，不传递日志记录器参数，让StatusTransitionContext自己创建
                var context = new StatusTransitionContext(entity, typeof(DataStatus), (DataStatus)currentStatus, this, _transitionEngine);
                return _transitionEngine.GetAvailableTransitions((DataStatus)currentStatus, context);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "获取可转换数据状态列表失败：实体类型 {EntityType}", entity.GetType().Name);
                return Enumerable.Empty<DataStatus>();
            }
        }

        /// <summary>
        /// 获取可转换的业务性状态列表
        /// </summary>
        /// <typeparam name="T">业务状态枚举类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <returns>可转换的状态列表</returns>
        public IEnumerable<T> GetAvailableBusinessStatusTransitions<T>(BaseEntity entity) where T : Enum
        {
            if (entity == null)
                return Enumerable.Empty<T>();

            try
            {
                var currentStatus = GetBusinessStatus<T>(entity);
                // 创建状态转换上下文，不传递日志记录器参数，让StatusTransitionContext自己创建
                var context = new StatusTransitionContext(entity, typeof(T), currentStatus, this, _transitionEngine);
                return _transitionEngine.GetAvailableTransitions(currentStatus, context).Cast<T>();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "获取可转换业务状态列表失败：实体类型 {EntityType}, 状态类型 {StatusType}", entity.GetType().Name, typeof(T).Name);
                return Enumerable.Empty<T>();
            }
        }

        /// <summary>
        /// 获取可转换的业务性状态列表
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">状态类型</param>
        /// <returns>可转换的状态列表</returns>
        public IEnumerable<object> GetAvailableBusinessStatusTransitions(BaseEntity entity, Type statusType)
        {
            if (entity == null)
                return Enumerable.Empty<object>();

            try
            {
                // 使用反射获取当前状态
                var currentStatusProperty = entity.GetType().GetProperties()
                    .FirstOrDefault(p => p.PropertyType == statusType);

                if (currentStatusProperty == null)
                {
                    _logger?.LogWarning("获取可转换业务状态列表失败：实体类型 {EntityType} 没有类型为 {StatusType} 的属性", entity.GetType().Name, statusType.Name);
                    return Enumerable.Empty<object>();
                }

                var currentStatus = currentStatusProperty.GetValue(entity);
                // 创建状态转换上下文，不传递日志记录器参数，让StatusTransitionContext自己创建
                var context = new StatusTransitionContext(entity, statusType, currentStatus, this, _transitionEngine);
                
                // 根据状态类型调用相应的泛型方法
                if (statusType.IsEnum)
                {
                    var method = _transitionEngine.GetType().GetMethod(nameof(_transitionEngine.GetAvailableTransitions))
                        .MakeGenericMethod(statusType);
                    return (IEnumerable<object>)method.Invoke(_transitionEngine, new object[] { currentStatus, context });
                }
                else
                {
                    return _transitionEngine.GetAvailableTransitions((Enum)currentStatus, context);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "获取可转换业务状态列表失败：实体类型 {EntityType}, 状态类型 {StatusType}", entity.GetType().Name, statusType.Name);
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
                // 创建状态转换上下文，不传递日志记录器参数，让StatusTransitionContext自己创建
                var context = new StatusTransitionContext(entity, typeof(ActionStatus), currentStatus, this, _transitionEngine);
                return _transitionEngine.GetAvailableTransitions<ActionStatus>(currentStatus, context);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "获取可转换操作状态列表失败：实体类型 {EntityType}", entity.GetType().Name);
                return Enumerable.Empty<ActionStatus>();
            }
        }

        /// <summary>
        /// 检查是否可以转换到目标数据性状态
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
        /// 异步检查是否可以转换到目标数据性状态，并输出错误信息
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>包含转换结果和错误消息的元组</returns>
        public async Task<(bool CanTransition, string ErrorMessage)> CanTransitionToDataStatusAsync(BaseEntity entity, DataStatus targetStatus)
        {
            var result = await ValidateDataStatusTransitionAsync(entity, targetStatus);
            return (result.IsSuccess, result.ErrorMessage);
        }

        /// <summary>
        /// 检查是否可以转换到目标业务性状态
        /// </summary>
        /// <typeparam name="T">业务状态枚举类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>是否可以转换</returns>
        public async Task<bool> CanTransitionToBusinessStatus<T>(BaseEntity entity, T targetStatus) where T : Enum
        {
            var result = await ValidateBusinessStatusTransitionAsync(entity, targetStatus);
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
        /// 请求状态转换
        /// </summary>
        /// <param name="context">状态转换上下文</param>
        /// <param name="targetStatus">目标状态</param>
        /// <param name="reason">转换原因</param>
        /// <param name="userId">操作用户ID</param>
        /// <returns>状态转换结果</returns>
        public StateTransitionResult RequestTransition(IStatusTransitionContext context, object targetStatus, string reason = null, long userId = 0)
        {
            if (context == null)
                return StateTransitionResult.Failure("状态转换上下文不能为空");

            try
            {
                var currentStatus = context.CurrentStatus;
                StateTransitionResult result;

                // 根据状态类型调用相应的泛型方法
                if (context.StatusType == typeof(DataStatus))
                {
                    result = _transitionEngine.ExecuteTransitionAsync(
                        (DataStatus)currentStatus, 
                        (DataStatus)targetStatus, 
                        context).Result;
                }
                else if (context.StatusType == typeof(ActionStatus))
                {
                    result = _transitionEngine.ExecuteTransitionAsync(
                        (ActionStatus)currentStatus, 
                        (ActionStatus)targetStatus, 
                        context).Result;
                }
                else
                {
                    // 业务状态 - 使用反射调用泛型方法
                    var method = _transitionEngine.GetType().GetMethod(nameof(_transitionEngine.ExecuteTransitionAsync))
                        .MakeGenericMethod(context.StatusType);
                    result = (StateTransitionResult)method.Invoke(_transitionEngine, new object[] { currentStatus, targetStatus, context });
                }

                if (result.IsSuccess)
                {
                    // 更新实体状态
                    var entity = context.Entity as BaseEntity;
                    if (entity != null)
                    {
                        UpdateEntityStatus(entity, context.StatusType, targetStatus);

                        // 触发状态变更事件
                        OnStatusChanged(new StateTransitionEventArgs(
                            entity,
                            context.StatusType,
                            currentStatus,
                            targetStatus,
                            reason,
                            userId.ToString()));
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "请求状态转换失败：实体类型 {EntityType}, 当前状态 -> 目标状态 {TargetStatus}",
                    context.Entity?.GetType().Name, targetStatus);
                return StateTransitionResult.Failure($"请求状态转换失败：{ex.Message}");
            }
        }

        /// <summary>
        /// 批量设置实体状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="dataStatus">数据状态</param>
        /// <param name="businessStatus">业务状态（可选）</param>
        /// <param name="actionStatus">操作状态（可选）</param>
        /// <returns>设置是否成功</returns>
        public async Task<bool> SetStatesAsync(object entity, DataStatus dataStatus, Enum businessStatus = null, ActionStatus actionStatus = ActionStatus.无操作)
        {
            if (entity == null)
                return false;

            try
            {
                var baseEntity = entity as BaseEntity;
                if (baseEntity == null)
                {
                    _logger?.LogWarning("批量设置状态失败：实体不是BaseEntity类型");
                    return false;
                }

                // 设置数据状态
                var dataStatusResult = await SetDataStatusAsync(baseEntity, dataStatus);
                if (!dataStatusResult)
                {
                    _logger?.LogWarning("批量设置状态失败：设置数据状态失败");
                    return false;
                }

                // 设置业务状态
                if (businessStatus != null)
                {
                    var businessStatusResult = await SetBusinessStatusAsync(baseEntity, businessStatus);
                    if (!businessStatusResult)
                    {
                        _logger?.LogWarning("批量设置状态失败：设置业务状态失败");
                        return false;
                    }
                }

                // 设置操作状态
                var actionStatusResult = await SetActionStatusAsync(baseEntity, actionStatus);
                if (!actionStatusResult)
                {
                    _logger?.LogWarning("批量设置状态失败：设置操作状态失败");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "批量设置状态失败：实体类型 {EntityType}", entity.GetType().Name);
                return false;
            }
        }

        /// <summary>
        /// 检查实体是否可以更改为目标状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="targetStatus">目标状态</param>
        /// <param name="errorMessage">错误信息输出参数</param>
        /// <returns>是否可以更改</returns>
        public bool CanChangeStatus(object entity, Enum targetStatus, out string errorMessage)
        {
            errorMessage = null;

            if (entity == null)
            {
                errorMessage = "实体对象不能为空";
                return false;
            }

            try
            {
                var baseEntity = entity as BaseEntity;
                if (baseEntity == null)
                {
                    errorMessage = "实体不是BaseEntity类型";
                    return false;
                }

                // 根据目标状态类型进行不同的验证
                if (targetStatus is DataStatus dataStatus)
                {
                    var result = ValidateDataStatusTransitionAsync(baseEntity, dataStatus).Result;
                    errorMessage = result.ErrorMessage;
                    return result.IsSuccess;
                }
                else if (targetStatus is ActionStatus actionStatus)
                {
                    var result = ValidateActionStatusTransitionAsync(baseEntity, actionStatus).Result;
                    errorMessage = result.ErrorMessage;
                    return result.IsSuccess;
                }
                else
                {
                    // 业务状态
                    var statusType = targetStatus.GetType();
                    var result = ValidateBusinessStatusTransitionAsync(baseEntity, statusType, targetStatus).Result;
                    errorMessage = result.ErrorMessage;
                    return result.IsSuccess;
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"检查状态转换失败：{ex.Message}";
                _logger?.LogError(ex, "检查状态转换失败：实体类型 {EntityType}, 目标状态 {TargetStatus}", entity.GetType().Name, targetStatus);
                return false;
            }
        }

        /// <summary>
        /// 检查实体是否处于指定状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="statuses">要检查的状态列表</param>
        /// <returns>是否处于任一指定状态</returns>
        public bool IsInStatus(object entity, params Enum[] statuses)
        {
            if (entity == null || statuses == null || statuses.Length == 0)
                return false;

            try
            {
                var baseEntity = entity as BaseEntity;
                if (baseEntity == null)
                    return false;

                foreach (var status in statuses)
                {
                    if (status is DataStatus dataStatus)
                    {
                        if (GetDataStatus(baseEntity) == dataStatus)
                            return true;
                    }
                    else if (status is ActionStatus actionStatus)
                    {
                        if (GetActionStatus(baseEntity) == actionStatus)
                            return true;
                    }
                    else
                    {
                        // 业务状态
                        var statusType = status.GetType();
                        var method = typeof(UnifiedStateManager).GetMethod(nameof(GetBusinessStatus))
                            .MakeGenericMethod(statusType);
                        var currentStatus = method.Invoke(this, new object[] { baseEntity });

                        if (Equals(currentStatus, status))
                            return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "检查实体状态失败：实体类型 {EntityType}", entity.GetType().Name);
                return false;
            }
        }

        /// <summary>
        /// 获取状态转换失败的详细错误信息
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>错误信息</returns>
        public string GetTransitionErrorMessage(object entity, Enum targetStatus)
        {
            if (entity == null)
                return "实体对象不能为空";

            try
            {
                var baseEntity = entity as BaseEntity;
                if (baseEntity == null)
                    return "实体不是BaseEntity类型";

                // 根据目标状态类型进行不同的验证
                if (targetStatus is DataStatus dataStatus)
                {
                    var result = ValidateDataStatusTransitionAsync(baseEntity, dataStatus).Result;
                    return result.ErrorMessage ?? "状态转换验证失败";
                }
                else if (targetStatus is ActionStatus actionStatus)
                {
                    var result = ValidateActionStatusTransitionAsync(baseEntity, actionStatus).Result;
                    return result.ErrorMessage ?? "状态转换验证失败";
                }
                else
                {
                    // 业务状态
                    var statusType = targetStatus.GetType();
                    var result = ValidateBusinessStatusTransitionAsync(baseEntity, statusType, targetStatus).Result;
                    return result.ErrorMessage ?? "状态转换验证失败";
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "获取状态转换错误信息失败：实体类型 {EntityType}, 目标状态 {TargetStatus}", entity.GetType().Name, targetStatus);
                return $"获取状态转换错误信息失败：{ex.Message}";
            }
        }

        /// <summary>
        /// 清理缓存
        /// </summary>
        public void ClearCache()
        {
            // 简化版状态管理器没有缓存，此方法为空实现
            _logger?.LogDebug("清理缓存：简化版状态管理器没有缓存需要清理");
        }

        #endregion

        #region 私有辅助方法

        /// <summary>
        /// 转换实体的数据状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>转换结果</returns>
        private async Task<StateTransitionResult> TransitionDataStatusAsync(BaseEntity entity, DataStatus targetStatus)
        {
            if (entity == null)
                return StateTransitionResult.Failure("实体对象不能为空");

            try
            {
                var currentStatus = GetDataStatus(entity);
                // 创建状态转换上下文，不传递日志记录器参数，让StatusTransitionContext自己创建
                var context = new StatusTransitionContext(entity, typeof(DataStatus), currentStatus, this, _transitionEngine);

                // 执行状态转换
                var result = await _transitionEngine.ExecuteTransitionAsync(currentStatus, targetStatus, context);

                if (result.IsSuccess)
                {
                    // 更新实体状态
                    UpdateEntityStatus(entity, typeof(DataStatus), targetStatus);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "转换数据状态失败：实体类型 {EntityType}, 当前状态 -> 目标状态 {TargetStatus}", entity.GetType().Name, targetStatus);
                return StateTransitionResult.Failure($"转换数据状态失败：{ex.Message}", ex);
            }
        }

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
                var statusProperty = entity.GetType().GetProperties()
                    .FirstOrDefault(p => p.PropertyType == statusType);

                if (statusProperty != null)
                {
                    statusProperty.SetValue(entity, status);
                }
                else
                {
                    _logger?.LogWarning("无法更新实体状态：实体类型 {EntityType} 没有类型为 {StatusType} 的属性", entity.GetType().Name, statusType.Name);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "更新实体状态失败：实体类型 {EntityType}, 状态类型 {StatusType}, 目标状态 {Status}", entity.GetType().Name, statusType.Name, status);
            }
        }

        /// <summary>
        /// 触发状态变更事件
        /// </summary>
        /// <param name="e">事件参数</param>
        protected virtual void OnStatusChanged(StateTransitionEventArgs e)
        {
            StatusChanged?.Invoke(this, e);
        }

        #endregion

        #region 资源释放

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
            if (!_disposed && disposing)
            {
                if (_transitionEngine is IDisposable disposableEngine)
                {
                    disposableEngine.Dispose();
                }

                _disposed = true;
            }
        }

        #endregion
    }
}