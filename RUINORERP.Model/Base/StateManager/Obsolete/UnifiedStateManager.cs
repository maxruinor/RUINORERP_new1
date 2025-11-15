using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.Global;
using RUINORERP.Model.Base.StatusManager.Core;
using StatusTransitionResult = StateTransitionResult;

namespace RUINORERP.Model.Base.StateManager
{
    /// <summary>
    /// 统一状态管理器实现 - 整合数据性状态和业务性状态管理
    /// </summary>
    public class UnifiedStateManager : IUnifiedStateManager
    {
        #region 私有字段

        private readonly ILogger<UnifiedStateManager> _logger;
        private readonly StateManagerOptions _options;
        private readonly Dictionary<Type, Dictionary<object, List<object>>> _transitionRules;
        private readonly Dictionary<Type, Func<BaseEntity, object>> _statusGetters;
        private readonly Dictionary<Type, Action<BaseEntity, object>> _statusSetters;
        
        // 属性缓存，提高反射性能
        private static readonly ConcurrentDictionary<Type, PropertyInfo> _dataStatusPropertyCache = 
            new ConcurrentDictionary<Type, PropertyInfo>();
        
        private static readonly ConcurrentDictionary<Type, List<PropertyInfo>> _businessStatusPropertyCache = 
            new ConcurrentDictionary<Type, List<PropertyInfo>>();

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public UnifiedStateManager() : this(new StateManagerOptions())
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="options">状态管理器配置选项</param>
        public UnifiedStateManager(StateManagerOptions options)
        {
            _options = options ?? new StateManagerOptions();
            _transitionRules = new Dictionary<Type, Dictionary<object, List<object>>>();
            _statusGetters = new Dictionary<Type, Func<BaseEntity, object>>();
            _statusSetters = new Dictionary<Type, Action<BaseEntity, object>>();

            InitializeTransitionRules();
            InitializeStatusAccessors();
        }

        #endregion

        #region 事件

        /// <summary>
        /// 状态变更事件
        /// </summary>
        public event EventHandler<StateTransitionEventArgs> StatusChanged;

        #endregion

        #region 数据性状态管理

        /// <summary>
        /// 获取当前数据性状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>当前数据性状态</returns>
        public DataStatus GetDataStatus(BaseEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            // 使用缓存获取DataStatus属性
            var entityType = entity.GetType();
            var property = _dataStatusPropertyCache.GetOrAdd(entityType, type =>
            {
                var prop = type.GetProperty("DataStatus");
                // 验证属性类型是否为DataStatus?
                return (prop != null && prop.PropertyType == typeof(DataStatus?)) ? prop : null;
            });
            
            if (property != null)
            {
                var value = (DataStatus?)property.GetValue(entity);
                return value.HasValue ? value.Value : DataStatus.草稿;
            }

            // 如果没有DataStatus属性，返回默认值
            return DataStatus.草稿;
        }

        /// <summary>
        /// 设置数据性状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="status">状态值</param>
        /// <param name="reason">变更原因</param>
        /// <returns>设置是否成功</returns>
        public async Task<bool> SetDataStatusAsync(BaseEntity entity, DataStatus status, string reason = null)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var currentStatus = GetDataStatus(entity);
            if (currentStatus == status)
                return true;

            var validationResult = await ValidateDataStatusTransitionAsync(entity, status);
            if (!validationResult.IsValid)
            {
                return false;
            }

            // 使用缓存获取DataStatus属性
            var entityType = entity.GetType();
            var property = _dataStatusPropertyCache.GetOrAdd(entityType, type =>
            {
                var prop = type.GetProperty("DataStatus");
                // 验证属性类型是否为DataStatus?
                return (prop != null && prop.PropertyType == typeof(DataStatus?)) ? prop : null;
            });
            
            if (property != null)
            {
                var oldValue = property.GetValue(entity);
                property.SetValue(entity, status);
                
                // 触发状态变更事件
                OnStatusChanged(new StateTransitionEventArgs(
                    entity, 
                    typeof(DataStatus), 
                    oldValue, 
                    status, 
                    reason));
                
                return true;
            }

            return false;
        }

        /// <summary>
        /// 验证数据性状态转换
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>验证结果</returns>
        public Task<StateTransitionResult> ValidateDataStatusTransitionAsync(BaseEntity entity, DataStatus targetStatus)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var currentStatus = GetDataStatus(entity);
            
            // 检查转换规则
            if (_transitionRules.TryGetValue(typeof(DataStatus), out var rules))
            {
                if (rules.TryGetValue(currentStatus, out var allowedTransitions))
                {
                    if (!allowedTransitions.Contains(targetStatus))
                    {
                        return Task.FromResult(StateTransitionResult.Failure(
                            $"不能从状态 {currentStatus} 转换到状态 {targetStatus}"));
                    }
                }
            }

            return Task.FromResult(StateTransitionResult.Success());
        }

        /// <summary>
        /// 获取可转换的数据性状态列表
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>可转换的状态列表</returns>
        public IEnumerable<DataStatus> GetAvailableDataStatusTransitions(BaseEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var currentStatus = GetDataStatus(entity);
            
            if (_transitionRules.TryGetValue(typeof(DataStatus), out var rules))
            {
                if (rules.TryGetValue(currentStatus, out var allowedTransitions))
                {
                    return allowedTransitions.Cast<DataStatus>();
                }
            }

            return Enumerable.Empty<DataStatus>();
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
            return result.IsValid;
        }

        #endregion

        #region 业务性状态管理

        /// <summary>
        /// 获取当前业务性状态
        /// </summary>
        /// <typeparam name="T">业务状态枚举类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <returns>当前业务性状态</returns>
        public T GetBusinessStatus<T>(BaseEntity entity) where T : Enum
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var statusType = typeof(T);
            
            // 使用缓存获取业务状态属性
            var entityType = entity.GetType();
            var properties = _businessStatusPropertyCache.GetOrAdd(entityType, type =>
            {
                return type.GetProperties()
                    .Where(p => p.PropertyType == statusType)
                    .ToList();
            });
            
            var property = properties.FirstOrDefault();
            if (property != null)
            {
                var value = property.GetValue(entity);
                return value != null ? (T)value : default;
            }

            return default;
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
                throw new ArgumentNullException(nameof(entity));

            var currentStatus = GetBusinessStatus<T>(entity);
            if (EqualityComparer<T>.Default.Equals(currentStatus, status))
                return true;

            var validationResult = await ValidateBusinessStatusTransitionAsync(entity, status);
            if (!validationResult.IsValid)
            {
                return false;
            }

            var statusType = typeof(T);
            
            // 使用缓存获取业务状态属性
            var entityType = entity.GetType();
            var properties = _businessStatusPropertyCache.GetOrAdd(entityType, type =>
            {
                return type.GetProperties()
                    .Where(p => p.PropertyType == statusType)
                    .ToList();
            });
            
            var property = properties.FirstOrDefault();
            if (property != null)
            {
                var oldValue = property.GetValue(entity);
                property.SetValue(entity, status);
                
                // 触发状态变更事件
                OnStatusChanged(new StateTransitionEventArgs(
                    entity, 
                    statusType, 
                    oldValue, 
                    status, 
                    reason));
                
                return true;
            }

            return false;
        }

        /// <summary>
        /// 验证业务性状态转换
        /// </summary>
        /// <typeparam name="T">业务状态枚举类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>验证结果</returns>
        public Task<StateTransitionResult> ValidateBusinessStatusTransitionAsync<T>(BaseEntity entity, T targetStatus) where T : Enum
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var currentStatus = GetBusinessStatus<T>(entity);
            
            // 检查转换规则
            if (_transitionRules.TryGetValue(typeof(T), out var rules))
            {
                if (rules.TryGetValue(currentStatus, out var allowedTransitions))
                {
                    if (!allowedTransitions.Contains(targetStatus))
                    {
                        return Task.FromResult(StateTransitionResult.Failure(
                            $"不能从状态 {currentStatus} 转换到状态 {targetStatus}"));
                    }
                }
            }

            return Task.FromResult(StateTransitionResult.Success());
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
                throw new ArgumentNullException(nameof(entity));

            var currentStatus = GetBusinessStatus<T>(entity);
            
            if (_transitionRules.TryGetValue(typeof(T), out var rules))
            {
                if (rules.TryGetValue(currentStatus, out var allowedTransitions))
                {
                    return allowedTransitions.Cast<T>();
                }
            }

            return Enumerable.Empty<T>();
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
            return result.IsValid;
        }

        #endregion

        #region 操作状态管理

        /// <summary>
        /// 获取当前操作状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>当前操作状态</returns>
        public ActionStatus GetActionStatus(BaseEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            return entity.ActionStatus;
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
                throw new ArgumentNullException(nameof(entity));

            var currentStatus = GetActionStatus(entity);
            if (currentStatus == status)
                return true;

            var validationResult = await ValidateActionStatusTransitionAsync(entity, status);
            if (!validationResult.IsValid)
            {
                return false;
            }

            var oldValue = entity.ActionStatus;
            entity.ActionStatus = status;
            
            // 触发状态变更事件
            OnStatusChanged(new StateTransitionEventArgs(
                entity, 
                typeof(ActionStatus), 
                oldValue, 
                status, 
                reason));
            
            return true;
        }

        /// <summary>
        /// 验证操作状态转换
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>验证结果</returns>
        public Task<StateTransitionResult> ValidateActionStatusTransitionAsync(BaseEntity entity, ActionStatus targetStatus)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var currentStatus = GetActionStatus(entity);
            
            // 检查转换规则
            if (_transitionRules.TryGetValue(typeof(ActionStatus), out var rules))
            {
                if (rules.TryGetValue(currentStatus, out var allowedTransitions))
                {
                    if (!allowedTransitions.Contains(targetStatus))
                    {
                        return Task.FromResult(StateTransitionResult.Failure(
                            $"不能从状态 {currentStatus} 转换到状态 {targetStatus}"));
                    }
                }
            }

            return Task.FromResult(StateTransitionResult.Success());
        }

        /// <summary>
        /// 获取可转换的操作状态列表
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>可转换的状态列表</returns>
        public IEnumerable<ActionStatus> GetAvailableActionStatusTransitions(BaseEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var currentStatus = GetActionStatus(entity);
            
            if (_transitionRules.TryGetValue(typeof(ActionStatus), out var rules))
            {
                if (rules.TryGetValue(currentStatus, out var allowedTransitions))
                {
                    return allowedTransitions.Cast<ActionStatus>();
                }
            }

            return Enumerable.Empty<ActionStatus>();
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
            return result.IsValid;
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 触发状态变更事件
        /// </summary>
        /// <param name="e">事件参数</param>
        protected virtual void OnStatusChanged(StateTransitionEventArgs e)
        {
            StatusChanged?.Invoke(this, e);
        }

        /// <summary>
        /// 初始化状态转换规则
        /// </summary>
        private void InitializeTransitionRules()
        {
            // 使用预定义的状态转换规则
            StateTransitionRules.InitializeDefaultRules(_transitionRules);
            
            // 如果有自定义规则，则应用自定义规则
            if (_options?.CustomTransitionRules != null)
            {
                foreach (var customRule in _options.CustomTransitionRules)
                {
                    if (!_transitionRules.ContainsKey(customRule.Key))
                    {
                        _transitionRules[customRule.Key] = customRule.Value;
                    }
                    else
                    {
                        // 合并规则，自定义规则覆盖默认规则
                        foreach (var rule in customRule.Value)
                        {
                            _transitionRules[customRule.Key][rule.Key] = rule.Value;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 初始化状态访问器
        /// </summary>
        private void InitializeStatusAccessors()
        {
            // 这里可以添加自定义的状态访问器
        }

        #endregion
  
        #region 通用状态转换方法

        /// <summary>
        /// 请求状态转换
        /// </summary>
        /// <param name="context">状态转换上下文</param>
        /// <param name="targetStatus">目标状态</param>
        /// <param name="reason">转换原因</param>
        /// <param name="userId">操作用户ID</param>
        /// <returns>状态转换结果</returns>
        public StatusTransitionResult RequestTransition(IStatusTransitionContext context, object targetStatus, string reason = null, long userId = 0)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            
            if (targetStatus == null)
                throw new ArgumentNullException(nameof(targetStatus));

            try
            {
                // 获取当前状态
                var currentStatus = context.CurrentStatus;
                
                // 检查状态类型是否匹配
                if (currentStatus != null && currentStatus.GetType() != targetStatus.GetType())
                {
                    return StatusTransitionResult.Failure(
                        $"状态类型不匹配。当前状态类型: {currentStatus.GetType().Name}, 目标状态类型: {targetStatus.GetType().Name}");
                }

                // 根据状态类型执行相应的验证和转换
                var statusType = targetStatus.GetType();
                StateTransitionResult validationResult = null;
                
                if (statusType == typeof(DataStatus))
                {
                    var targetDataStatus = (DataStatus)targetStatus;
                    validationResult = ValidateDataStatusTransitionAsync(context.Entity, targetDataStatus).Result;
                }
                else if (statusType.IsEnum)
                {
                    // 使用泛型方法处理业务状态
                    var method = typeof(UnifiedStateManager).GetMethod(nameof(ValidateBusinessStatusTransitionAsync), 
                        System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                    var genericMethod = method.MakeGenericMethod(statusType);
                    validationResult = (StateTransitionResult)genericMethod.Invoke(this, new object[] { context.Entity, targetStatus });
                }
                else if (statusType == typeof(ActionStatus))
                {
                    var targetActionStatus = (ActionStatus)targetStatus;
                    validationResult = ValidateActionStatusTransitionAsync(context.Entity, targetActionStatus).Result;
                }
                else
                {
                    return StatusTransitionResult.Failure($"不支持的状态类型: {statusType.Name}");
                }

                // 如果验证失败，返回验证结果
                if (validationResult != null && !validationResult.IsValid)
                {
                    return validationResult;
                }

                // 执行状态转换
                var transitionResult = context.TransitionTo(targetStatus, reason).Result;
                
                // 如果转换成功，触发状态变更事件
                if (transitionResult.IsValid)
                {
                    OnStatusChanged(new StateTransitionEventArgs(
                        context.Entity,
                        statusType,
                        currentStatus,
                        targetStatus,
                        reason));
                }

                return transitionResult;
            }
            catch (Exception ex)
            {
                return StatusTransitionResult.Failure($"状态转换过程中发生错误: {ex.Message}", ex);
            }
        }

        #endregion

       
    }
}