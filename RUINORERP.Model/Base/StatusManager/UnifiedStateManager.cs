using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.Model.Base;
using RUINORERP.Model.Base.StatusManager;
using RUINORERP.Global; // 添加此行以引用DataStatus枚举

namespace RUINORERP.Model.Base.StatusManager
{
    /// <summary>
    /// 统一状态管理器实现类 - 简化版
    /// 负责管理实体的各种状态转换，包括数据状态、业务状态和操作状态
    /// </summary>
    public class UnifiedStateManager : IUnifiedStateManager, IDisposable
    {
        private readonly ILogger<UnifiedStateManager> _logger;
        private readonly IStatusTransitionEngine _transitionEngine;
        private readonly IStateRuleConfiguration _ruleConfiguration;
        private readonly SimpleCacheManager _cacheManager;
        private bool _disposed = false;

        /// <summary>
        /// 状态变更事件
        /// </summary>
        public event EventHandler<StateTransitionEventArgs> StatusChanged;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="transitionEngine">状态转换引擎</param>
        /// <param name="ruleConfiguration">状态规则配置</param>
        /// <param name="cacheManager">缓存管理器</param>
        public UnifiedStateManager(
            ILogger<UnifiedStateManager> logger,
            IStatusTransitionEngine transitionEngine,
            IStateRuleConfiguration ruleConfiguration,
            SimpleCacheManager cacheManager = null)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _transitionEngine = transitionEngine ?? throw new ArgumentNullException(nameof(transitionEngine));
            _ruleConfiguration = ruleConfiguration ?? throw new ArgumentNullException(nameof(ruleConfiguration));
            _cacheManager = cacheManager ?? new SimpleCacheManager();
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
                var property = entity.GetType().GetProperty("Status");
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
                var property = entity.GetType().GetProperty("Status");
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
                var context = new StatusTransitionContext(entity, typeof(DataStatus), currentStatus, this, _transitionEngine);

                // 执行状态转换
                var result = await _transitionEngine.ExecuteTransitionAsync(currentStatus, status, context);
                if (result.IsSuccess)
                {
                    // 更新实体状态
                    UpdateEntityStatus(entity, typeof(DataStatus), status);
                    
                    // 触发状态变更事件
                    OnStatusChanged(new StateTransitionEventArgs(entity, typeof(DataStatus), currentStatus, status, reason));
                    
                    return true;
                }
                else
                {
                    _logger?.LogWarning("数据状态转换失败：{ErrorMessage}", result.ErrorMessage);
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
                var context = new StatusTransitionContext(entity, typeof(T), currentStatus, this, _transitionEngine);

                // 执行状态转换
                var result = await _transitionEngine.ExecuteTransitionAsync(currentStatus, status, context);
                if (result.IsSuccess)
                {
                    // 更新实体状态
                    UpdateEntityStatus(entity, typeof(T), status);
                    
                    // 触发状态变更事件
                    OnStatusChanged(new StateTransitionEventArgs(entity, typeof(T), currentStatus, status, reason));
                    
                    return true;
                }
                else
                {
                    _logger?.LogWarning("业务状态转换失败：{ErrorMessage}", result.ErrorMessage);
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
                var context = new StatusTransitionContext(entity, typeof(ActionStatus), currentStatus, this, _transitionEngine);

                // 执行状态转换
                var result = await _transitionEngine.ExecuteTransitionAsync(currentStatus, status, context);
                if (result.IsSuccess)
                {
                    // 更新实体状态
                    UpdateEntityStatus(entity, typeof(ActionStatus), status);
                    
                    // 触发状态变更事件
                    OnStatusChanged(new StateTransitionEventArgs(entity, typeof(ActionStatus), currentStatus, status, reason));
                    
                    return true;
                }
                else
                {
                    _logger?.LogWarning("操作状态转换失败：{ErrorMessage}", result.ErrorMessage);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "设置操作状态失败：实体类型 {EntityType}, 目标状态 {TargetStatus}", entity.GetType().Name, status);
                return false;
            }
        }

        #endregion

        #region 状态转换验证方法

        /// <summary>
        /// 验证数据状态转换是否有效
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>验证结果</returns>
        public Task<StateTransitionResult> ValidateDataStatusTransitionAsync(BaseEntity entity, DataStatus targetStatus)
        {
            if (entity == null)
                return Task.FromResult(StateTransitionResult.Failure("实体不能为空"));

            var currentStatus = GetDataStatus(entity);
            
            // 基本验证规则
            if (currentStatus == targetStatus)
                return Task.FromResult(StateTransitionResult.Success());

            // 检查状态转换规则
            var availableTransitions = GetAvailableDataStatusTransitions(entity);
            if (!availableTransitions.Contains(targetStatus))
            {
                return Task.FromResult(StateTransitionResult.Failure($"不允许从 {currentStatus} 转换到 {targetStatus}"));
            }

            return Task.FromResult(StateTransitionResult.Success());
        }

        /// <summary>
        /// 验证业务状态转换是否有效
        /// </summary>
        /// <typeparam name="T">业务状态枚举类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>验证结果</returns>
        public Task<StateTransitionResult> ValidateBusinessStatusTransitionAsync<T>(BaseEntity entity, T targetStatus) where T : struct, Enum
        {
            if (entity == null)
                return Task.FromResult(StateTransitionResult.Failure("实体不能为空"));

            var currentStatus = GetBusinessStatus<T>(entity);
            
            // 基本验证规则
            if (EqualityComparer<T>.Default.Equals(currentStatus, targetStatus))
                return Task.FromResult(StateTransitionResult.Success());

            // 检查状态转换规则
            var availableTransitions = GetAvailableBusinessStatusTransitions<T>(entity);
            if (!availableTransitions.Contains(targetStatus))
            {
                return Task.FromResult(StateTransitionResult.Failure($"不允许从 {currentStatus} 转换到 {targetStatus}"));
            }

            return Task.FromResult(StateTransitionResult.Success());
        }

        /// <summary>
        /// 验证业务状态转换是否有效
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">状态类型</param>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>验证结果</returns>
        public Task<StateTransitionResult> ValidateBusinessStatusTransitionAsync(BaseEntity entity, Type statusType, object targetStatus)
        {
            if (entity == null)
                return Task.FromResult(StateTransitionResult.Failure("实体不能为空"));

            if (statusType == null)
                return Task.FromResult(StateTransitionResult.Failure("状态类型不能为空"));

            if (targetStatus == null)
                return Task.FromResult(StateTransitionResult.Failure("目标状态不能为空"));

            var currentStatus = GetBusinessStatus(entity);
            
            // 基本验证规则
            if (Equals(currentStatus, targetStatus))
                return Task.FromResult(StateTransitionResult.Success());

            // 检查状态转换规则
            var availableTransitions = GetAvailableBusinessStatusTransitions(entity, statusType);
            if (!availableTransitions.Contains(targetStatus))
            {
                return Task.FromResult(StateTransitionResult.Failure($"不允许从 {currentStatus} 转换到 {targetStatus}"));
            }

            return Task.FromResult(StateTransitionResult.Success());
        }

        /// <summary>
        /// 验证操作状态转换是否有效
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>验证结果</returns>
        public Task<StateTransitionResult> ValidateActionStatusTransitionAsync(BaseEntity entity, ActionStatus targetStatus)
        {
            if (entity == null)
                return Task.FromResult(StateTransitionResult.Failure("实体不能为空"));

            var currentStatus = GetActionStatus(entity);
            
            // 基本验证规则
            if (currentStatus == targetStatus)
                return Task.FromResult(StateTransitionResult.Success());

            // 检查状态转换规则
            var availableTransitions = GetAvailableActionStatusTransitions(entity);
            if (!availableTransitions.Contains(targetStatus))
            {
                return Task.FromResult(StateTransitionResult.Failure($"不允许从 {currentStatus} 转换到 {targetStatus}"));
            }

            return Task.FromResult(StateTransitionResult.Success());
        }

        #endregion

        #region 可转换状态列表获取方法

        /// <summary>
        /// 获取可转换的数据状态列表
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>可转换的状态列表</returns>
        public IEnumerable<DataStatus> GetAvailableDataStatusTransitions(BaseEntity entity)
        {
            if (entity == null)
                return Enumerable.Empty<DataStatus>();

            var currentStatus = GetDataStatus(entity);
            
            // 使用缓存获取状态转换规则
            var cacheKey = $"DataStatus_Transitions_{currentStatus}";
            var cachedTransitions = _cacheManager.GetTransitionRuleCache(cacheKey);
            
            if (cachedTransitions != null)
            {
                return cachedTransitions.Cast<DataStatus>();
            }
            
            // 从状态转换规则中获取可转换的状态
            var transitionRules = new Dictionary<Type, Dictionary<object, List<object>>>();
            StateTransitionRules.InitializeDefaultRules(transitionRules);
            
            if (transitionRules.TryGetValue(typeof(DataStatus), out var dataStatusRules) &&
                dataStatusRules.TryGetValue(currentStatus, out var availableTransitions))
            {
                var result = availableTransitions.Cast<DataStatus>().ToList();
                
                // 将结果存入缓存
                _cacheManager.SetTransitionRuleCache(cacheKey, result.Cast<object>().ToList());
                
                return result;
            }
            
            return Enumerable.Empty<DataStatus>();
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

            var currentStatus = GetBusinessStatus<T>(entity);
            var statusType = typeof(T);
            
            // 使用缓存获取状态转换规则
            var cacheKey = $"BusinessStatus_{statusType.Name}_Transitions_{currentStatus}";
            var cachedTransitions = _cacheManager.GetTransitionRuleCache(cacheKey);
            
            if (cachedTransitions != null)
            {
                return cachedTransitions.Cast<T>();
            }
            
            // 从状态转换规则中获取可转换的状态
            var transitionRules = new Dictionary<Type, Dictionary<object, List<object>>>();
            StateTransitionRules.InitializeDefaultRules(transitionRules);
            
            if (transitionRules.TryGetValue(statusType, out var businessStatusRules) &&
                businessStatusRules.TryGetValue(currentStatus, out var availableTransitions))
            {
                var result = availableTransitions.Cast<T>().ToList();
                
                // 将结果存入缓存
                _cacheManager.SetTransitionRuleCache(cacheKey, result.Cast<object>().ToList());
                
                return result;
            }
            
            // 如果没有找到转换规则，返回空列表
            return Enumerable.Empty<T>();
        }

        /// <summary>
        /// 获取可转换的业务状态列表
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">状态类型</param>
        /// <returns>可转换的状态列表</returns>
        public IEnumerable<object> GetAvailableBusinessStatusTransitions(BaseEntity entity, Type statusType)
        {
            if (entity == null)
                return Enumerable.Empty<object>();

            if (statusType == null)
                return Enumerable.Empty<object>();

            var currentStatus = GetBusinessStatus(entity, statusType);
            
            // 使用缓存获取状态转换规则
            var cacheKey = $"BusinessStatus_{statusType.Name}_Transitions_{currentStatus}";
            var cachedTransitions = _cacheManager.GetTransitionRuleCache(cacheKey);
            
            if (cachedTransitions != null)
            {
                return cachedTransitions;
            }
            
            // 从状态转换规则中获取可转换的状态
            var transitionRules = new Dictionary<Type, Dictionary<object, List<object>>>();
            StateTransitionRules.InitializeDefaultRules(transitionRules);
            
            if (transitionRules.TryGetValue(statusType, out var businessStatusRules) &&
                businessStatusRules.TryGetValue(currentStatus, out var availableTransitions))
            {
                var result = availableTransitions.ToList();
                
                // 将结果存入缓存
                _cacheManager.SetTransitionRuleCache(cacheKey, result);
                
                return result;
            }
            
            // 如果没有找到转换规则，返回空列表
            return Enumerable.Empty<object>();
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

            var currentStatus = GetActionStatus(entity);
            
            // 使用缓存获取状态转换规则
            var cacheKey = $"ActionStatus_Transitions_{currentStatus}";
            var cachedTransitions = _cacheManager.GetTransitionRuleCache(cacheKey);
            
            if (cachedTransitions != null)
            {
                return cachedTransitions.Cast<ActionStatus>();
            }
            
            // 从状态转换规则中获取可转换的状态
            var transitionRules = new Dictionary<Type, Dictionary<object, List<object>>>();
            StateTransitionRules.InitializeDefaultRules(transitionRules);
            
            if (transitionRules.TryGetValue(typeof(ActionStatus), out var actionStatusRules) &&
                actionStatusRules.TryGetValue(currentStatus, out var availableTransitions))
            {
                var result = availableTransitions.Cast<ActionStatus>().ToList();
                
                // 将结果存入缓存
                _cacheManager.SetTransitionRuleCache(cacheKey, result.Cast<object>().ToList());
                
                return result;
            }
            
            return Enumerable.Empty<ActionStatus>();
        }

        #endregion

        #region 状态转换检查方法

        /// <summary>
        /// 检查是否可以转换到指定的数据状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>是否可以转换</returns>
        public async Task<bool> CanTransitionToDataStatus(BaseEntity entity, DataStatus targetStatus)
        {
            if (entity == null)
                return false;

            try
            {
                var validationResult = await ValidateDataStatusTransitionAsync(entity, targetStatus);
                return validationResult.IsSuccess;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "检查数据状态转换失败：实体类型 {EntityType}, 目标状态 {TargetStatus}", entity.GetType().Name, targetStatus);
                return false;
            }
        }

        /// <summary>
        /// 检查是否可以转换到指定的业务状态
        /// </summary>
        /// <typeparam name="T">业务状态枚举类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>是否可以转换</returns>
        public async Task<bool> CanTransitionToBusinessStatus<T>(BaseEntity entity, T targetStatus) where T : struct, Enum
        {
            if (entity == null)
                return false;

            try
            {
                var validationResult = await ValidateBusinessStatusTransitionAsync(entity, targetStatus);
                return validationResult.IsSuccess;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "检查业务状态转换失败：实体类型 {EntityType}, 目标状态 {TargetStatus}", entity.GetType().Name, targetStatus);
                return false;
            }
        }

        /// <summary>
        /// 检查是否可以转换到指定的操作状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>是否可以转换</returns>
        public async Task<bool> CanTransitionToActionStatus(BaseEntity entity, ActionStatus targetStatus)
        {
            if (entity == null)
                return false;

            try
            {
                var validationResult = await ValidateActionStatusTransitionAsync(entity, targetStatus);
                return validationResult.IsSuccess;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "检查操作状态转换失败：实体类型 {EntityType}, 目标状态 {TargetStatus}", entity.GetType().Name, targetStatus);
                return false;
            }
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 更新实体状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">状态类型</param>
        /// <param name="statusValue">状态值</param>
        private void UpdateEntityStatus(BaseEntity entity, Type statusType, object statusValue)
        {
            try
            {
                string propertyName = statusType.Name;
                
                // 特殊处理某些状态类型
                if (statusType == typeof(DataStatus))
                    propertyName = "DataStatus";
                else if (statusType == typeof(ActionStatus))
                    propertyName = "ActionStatus";
                else if (statusType.Name.EndsWith("Status"))
                    propertyName = statusType.Name;

                var property = entity.GetType().GetProperty(propertyName);
                if (property != null && property.CanWrite)
                {
                    // 如果属性是int类型，需要转换枚举为int
                    if (property.PropertyType == typeof(int) && statusValue is Enum enumValue)
                    {
                        property.SetValue(entity, Convert.ToInt32(enumValue));
                    }
                    else
                    {
                        property.SetValue(entity, statusValue);
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
        /// <param name="e">事件参数</param>
        protected virtual void OnStatusChanged(StateTransitionEventArgs e)
        {
            StatusChanged?.Invoke(this, e);
        }

        /// <summary>
        /// 创建数据状态转换上下文
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
                    _transitionEngine,
                    null); // 传递null，因为StatusTransitionContext期望ILogger<StatusTransitionContext>类型

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
                    _transitionEngine,
                    null); // 传递null，因为StatusTransitionContext期望ILogger<StatusTransitionContext>类型

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
                    _transitionEngine,
                    null); // 传递null，因为StatusTransitionContext期望ILogger<StatusTransitionContext>类型

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
                    _transitionEngine,
                    null); // 传递null，因为StatusTransitionContext期望ILogger<StatusTransitionContext>类型

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
                    _transitionEngine,
                    null); // 传递null，因为StatusTransitionContext期望ILogger<StatusTransitionContext>类型

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
                // 注意：IStatusTransitionEngine接口没有定义ClearCache方法
                // 因此不能直接调用_transitionEngine.ClearCache()
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
        public StateTransitionResult CanExecuteActionWithMessage(IUnifiedStateManager stateManager, Type statusType, object status, MenuItemEnums action)
        {
            try
            {
                if (stateManager == null)
                {
                    return StateTransitionResult.Failure("状态管理器不能为空");
                }

                if (status == null)
                {
                    return StateTransitionResult.Failure("状态信息不能为空");
                }

                // 将状态转换为DataStatus枚举
                DataStatus dataStatus;
                if (status is DataStatus ds)
                {
                    dataStatus = ds;
                }
                else if (status is Enum statusEnum)
                {
                    // 尝试将枚举转换为DataStatus
                    if (Enum.TryParse<DataStatus>(status.ToString(), out var parsedStatus))
                    {
                        dataStatus = parsedStatus;
                    }
                    else
                    {
                        return StateTransitionResult.Failure($"无法识别的状态类型: {status}");
                    }
                }
                else
                {
                    return StateTransitionResult.Failure($"不支持的状态类型: {status.GetType().Name}");
                }

                // 获取操作权限规则
                var actionRules = GetActionPermissionRules();
                
                // 检查当前状态是否允许执行该操作
                bool canExecute = actionRules.TryGetValue(dataStatus, out var allowedActions) && 
                                 allowedActions.Contains(action);

                // 生成友好的提示消息
                string message = canExecute ? GetSuccessMessage(action) : GetFailureMessage(action, dataStatus);

                return canExecute 
                    ? StateTransitionResult.Success(message)
                    : StateTransitionResult.Failure(message);
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
            switch (action)
            {
                case MenuItemEnums.新增:
                    return dataStatus == DataStatus.草稿 || dataStatus == DataStatus.完结 
                        ? "可以新增当前单据" 
                        : $"只有草稿状态或完结状态的单据才能新增，当前状态：{dataStatus}";
                
                case MenuItemEnums.修改:
                    return dataStatus == DataStatus.草稿 || dataStatus == DataStatus.新建 
                        ? "可以修改当前单据" 
                        : $"只有草稿状态或新建状态的单据才能修改，当前状态：{dataStatus}";
                
                case MenuItemEnums.删除:
                    return dataStatus == DataStatus.草稿 || dataStatus == DataStatus.新建 
                        ? "可以删除当前单据" 
                        : $"只有草稿状态或新建状态的单据才能删除，当前状态：{dataStatus}";
                
                case MenuItemEnums.保存:
                    return dataStatus == DataStatus.草稿 || dataStatus == DataStatus.新建 
                        ? "可以保存当前单据" 
                        : $"只有草稿状态或新建状态的单据才能保存，当前状态：{dataStatus}";
                
                case MenuItemEnums.提交:
                    return dataStatus == DataStatus.草稿 
                        ? "可以提交当前单据" 
                        : $"只有草稿状态的单据才能提交，当前状态：{dataStatus}";
                
                case MenuItemEnums.审核:
                    return dataStatus == DataStatus.新建 
                        ? "可以审核当前单据" 
                        : $"只有新建状态的单据才能审核，当前状态：{dataStatus}";
                
                case MenuItemEnums.反审:
                    return dataStatus == DataStatus.确认 
                        ? "可以反审核当前单据" 
                        : $"只有确认状态的单据才能反审核，当前状态：{dataStatus}";
                
                case MenuItemEnums.结案:
                    return dataStatus == DataStatus.确认 
                        ? "可以结案当前单据" 
                        : $"只有确认状态的单据才能结案，当前状态：{dataStatus}";
                
                default:
                    return "无法执行当前操作";
            }
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
                // 获取当前数据状态
                var dataStatus = GetDataStatus(entity);
                
                // 获取操作权限规则
                var actionRules = GetActionPermissionRules();
                
                // 检查当前状态是否允许执行该操作
                bool canExecute = actionRules.TryGetValue(dataStatus, out var allowedActions) && 
                                 allowedActions.Contains(action);

                return canExecute;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "检查操作权限失败：操作 {Action}，实体类型 {EntityType}", action, entity.GetType().Name);
                return false;
            }
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
                // 获取当前数据状态
                var dataStatus = GetDataStatus(entity);
                
                // 获取操作权限规则
                var actionRules = GetActionPermissionRules();
                
                // 获取当前状态下的可用操作
                var availableActions = actionRules.TryGetValue(dataStatus, out var actions) ? actions : new List<MenuItemEnums>();
                
                return availableActions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取可用操作列表失败：实体类型 {EntityType}", entity.GetType().Name);
                return Enumerable.Empty<MenuItemEnums>();
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
                    // _transitionEngine?.Dispose();
                    // _ruleConfiguration?.Dispose();
                }

                _disposed = true;
            }
        }

        #endregion
    }
}