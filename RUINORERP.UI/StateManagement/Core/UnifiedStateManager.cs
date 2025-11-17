/**
 * 文件: UnifiedStateManager.cs
 * 说明: 统一状态管理器实现 - v3版本
 * 创建日期: 2024年
 * 作者: RUINOR ERP开发团队
 */

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.Model.Base.StatusManager.Core;

namespace RUINORERP.UI.StateManagement.Core
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

        #region 实体状态管理

        /// <summary>
        /// 获取实体的状态信息
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>实体状态信息</returns>
        public EntityStatus GetEntityStatus(BaseEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var entityStatus = new EntityStatus();
            
            // 获取数据性状态
            entityStatus.dataStatus = GetDataStatus(entity);
            
            // 获取操作状态
            entityStatus.actionStatus = GetActionStatus(entity);
            
            // 获取所有业务性状态
            var entityType = entity.GetType();
            var properties = entityType.GetProperties()
                .Where(p => p.PropertyType.IsEnum && 
                           !p.PropertyType.Equals(typeof(DataStatus)) && 
                           !p.PropertyType.Equals(typeof(ActionStatus)))
                .ToList();
            
            foreach (var property in properties)
            {
                var value = property.GetValue(entity);
                if (value != null)
                {
                    entityStatus.SetBusinessStatus(property.PropertyType, value);
                }
            }
            
            return entityStatus;
        }

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

            return Task.FromResult(StateTransitionResult.Success(message: string.Empty));
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
        /// 检查数据性状态是否可以转换
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>是否可以转换</returns>
        public async Task<bool> CanTransitionDataStatusAsync(BaseEntity entity, DataStatus targetStatus)
        {
            var result = await ValidateDataStatusTransitionAsync(entity, targetStatus);
            return result.IsValid;
        }

        #endregion

        #region 业务性状态管理

        /// <summary>
        /// 获取当前业务性状态
        /// </summary>
        /// <typeparam name="T">状态类型</typeparam>
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
        /// 获取当前业务性状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">状态类型</param>
        /// <returns>当前业务性状态</returns>
        public object GetBusinessStatus(BaseEntity entity, Type statusType)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            // 使用缓存获取业务状态属性
            var entityType = entity.GetType();
            var properties = _businessStatusPropertyCache.GetOrAdd(entityType, type =>
            {
                return type.GetProperties()
                    .Where(p => p.PropertyType == statusType)
                    .ToList();
            });
            
            var property = properties.FirstOrDefault();
            return property?.GetValue(entity);
        }

        /// <summary>
        /// 设置业务性状态
        /// </summary>
        /// <typeparam name="T">状态类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="status">状态值</param>
        /// <param name="reason">变更原因</param>
        /// <returns>设置是否成功</returns>
        public async Task<bool> SetBusinessStatusAsync<T>(BaseEntity entity, T status, string reason = null) where T : Enum
        {
            return await SetBusinessStatusAsync(entity, typeof(T), status, reason);
        }

        /// <summary>
        /// 设置业务性状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">状态类型</param>
        /// <param name="status">状态值</param>
        /// <param name="reason">变更原因</param>
        /// <returns>设置是否成功</returns>
        public async Task<bool> SetBusinessStatusAsync(BaseEntity entity, Type statusType, object status, string reason = null)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var currentStatus = GetBusinessStatus(entity, statusType);
            if (Equals(currentStatus, status))
                return true;

            var validationResult = await ValidateBusinessStatusTransitionAsync(entity, statusType, status);
            if (!validationResult.IsValid)
            {
                return false;
            }

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
        /// <typeparam name="T">状态类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>验证结果</returns>
        public Task<StateTransitionResult> ValidateBusinessStatusTransitionAsync<T>(BaseEntity entity, T targetStatus) where T : Enum
        {
            return ValidateBusinessStatusTransitionAsync(entity, typeof(T), targetStatus);
        }

        /// <summary>
        /// 验证业务性状态转换
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">状态类型</param>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>验证结果</returns>
        public Task<StateTransitionResult> ValidateBusinessStatusTransitionAsync(BaseEntity entity, Type statusType, object targetStatus)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var currentStatus = GetBusinessStatus(entity, statusType);
            
            // 检查转换规则
            if (_transitionRules.TryGetValue(statusType, out var rules))
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

            return Task.FromResult(StateTransitionResult.Success(message: string.Empty));
        }

        /// <summary>
        /// 获取可转换的业务性状态列表
        /// </summary>
        /// <typeparam name="T">状态类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <returns>可转换的状态列表</returns>
        public IEnumerable<T> GetAvailableBusinessStatusTransitions<T>(BaseEntity entity) where T : Enum
        {
            var transitions = GetAvailableBusinessStatusTransitions(entity, typeof(T));
            return transitions.Cast<T>();
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
                throw new ArgumentNullException(nameof(entity));

            var currentStatus = GetBusinessStatus(entity, statusType);
            
            if (_transitionRules.TryGetValue(statusType, out var rules))
            {
                if (rules.TryGetValue(currentStatus, out var allowedTransitions))
                {
                    return allowedTransitions;
                }
            }

            return Enumerable.Empty<object>();
        }

        /// <summary>
        /// 检查业务性状态是否可以转换
        /// </summary>
        /// <typeparam name="T">状态类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>是否可以转换</returns>
        public async Task<bool> CanTransitionBusinessStatusAsync<T>(BaseEntity entity, T targetStatus) where  T : Enum
        {
            // 检查T是否为枚举类型
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T必须是枚举类型", nameof(T));
            }
            
            var result = await ValidateBusinessStatusTransitionAsync<T>(entity, targetStatus);
            return result.IsValid;
        }

        /// <summary>
        /// 检查业务性状态是否可以转换
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">状态类型</param>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>是否可以转换</returns>
        public async Task<bool> CanTransitionBusinessStatusAsync(BaseEntity entity, Type statusType, object targetStatus)
        {
            var result = await ValidateBusinessStatusTransitionAsync(entity, statusType, targetStatus);
            return result.IsValid;
        }

        #endregion

        #region 操作性状态管理

        /// <summary>
        /// 获取当前操作性状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>当前操作性状态</returns>
        public ActionStatus GetActionStatus(BaseEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            return entity.ActionStatus;
        }

        /// <summary>
        /// 设置操作性状态
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
        /// 验证操作性状态转换
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

            return Task.FromResult(StateTransitionResult.Success(message: string.Empty));
        }

        /// <summary>
        /// 获取可转换的操作性状态列表
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
        /// 检查操作性状态是否可以转换
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>是否可以转换</returns>
        public async Task<bool> CanTransitionActionStatusAsync(BaseEntity entity, ActionStatus targetStatus)
        {
            var result = await ValidateActionStatusTransitionAsync(entity, targetStatus);
            return result.IsValid;
        }

        #endregion

        #region 综合状态管理

        /// <summary>
        /// 获取实体所有状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>实体所有状态</returns>
        public EntityStatus GetAllStatus(BaseEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var entityStatus = new EntityStatus
            {
                dataStatus = GetDataStatus(entity),
                actionStatus = GetActionStatus(entity)
            };
            
            // 获取所有业务状态
            var entityType = entity.GetType();
            if (_businessStatusPropertyCache.TryGetValue(entityType, out var businessStatusProperties))
            {
                foreach (var property in businessStatusProperties)
                {
                    if (property.PropertyType.IsEnum)
                    {
                        var value = property.GetValue(entity);
                        if (value != null)
                        {
                            entityStatus.SetBusinessStatus(property.PropertyType, value);
                        }
                    }
                }
            }
            
            return entityStatus;
        }

        /// <summary>
        /// 批量设置实体状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="status">实体状态</param>
        /// <param name="reason">变更原因</param>
        /// <returns>设置是否成功</returns>
        public async Task<bool> SetAllStatusAsync(BaseEntity entity, EntityStatus status, string reason = null)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (status == null)
                throw new ArgumentNullException(nameof(status));

            var result = true;

            // 设置数据性状态
            if (status.dataStatus.HasValue)
            {
                result &= await SetDataStatusAsync(entity, status.dataStatus.Value, reason);
            }

            // 设置操作性状态
            if (status.actionStatus.HasValue)
            {
                result &= await SetActionStatusAsync(entity, status.actionStatus.Value, reason);
            }

            return result;
        }

        /// <summary>
        /// 重置实体所有状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="reason">重置原因</param>
        /// <returns>重置是否成功</returns>
        public async Task<bool> ResetAllStatusAsync(BaseEntity entity, string reason = null)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            var result = true;

            // 重置数据性状态为默认值
            result &= await SetDataStatusAsync(entity, DataStatus.草稿, reason);

            // 重置操作性状态为默认值
            result &= await SetActionStatusAsync(entity, ActionStatus.无操作, reason);

            return result;
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

        /// <summary>
        /// 检查是否可以转换到目标业务性状态
        /// </summary>
        /// <typeparam name="T">业务状态枚举类型</typeparam>
        /// <param name="entity">实体对象</param>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>是否可以转换</returns>
        public async Task<bool> CanTransitionToBusinessStatus<T>(BaseEntity entity, T targetStatus) where T : Enum
        {
            var result = await ValidateBusinessStatusTransitionAsync<T>(entity, targetStatus);
            return result.IsValid;
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
                throw new ArgumentNullException(nameof(context));

            var entity = context.Entity;
            if (entity == null)
                return StateTransitionResult.Failure("状态转换上下文中缺少实体对象");

            var statusType = context.StatusType;
            if (statusType == null)
                return StateTransitionResult.Failure("状态转换上下文中缺少状态类型");

            try
            {
                // 根据状态类型执行相应的转换
                if (statusType == typeof(DataStatus))
                {
                    var dataStatus = (DataStatus)targetStatus;
                    var validationResult = ValidateDataStatusTransitionAsync(entity, dataStatus).Result;
                    if (!validationResult.IsValid)
                        return validationResult;

                    SetDataStatusAsync(entity, dataStatus, reason).Wait();
                    return StateTransitionResult.Success(message: string.Empty);
                }
                else if (statusType.IsEnum)
                {
                    // 检查是否为ActionStatus
                    if (statusType == typeof(ActionStatus))
                    {
                        var actionStatus = (ActionStatus)targetStatus;
                        var validationResult = ValidateActionStatusTransitionAsync(entity, actionStatus).Result;
                        if (!validationResult.IsValid)
                            return validationResult;

                        SetActionStatusAsync(entity, actionStatus, reason).Wait();
                        return StateTransitionResult.Success(message: string.Empty);
                    }
                    else
                    {
                        // 处理其他枚举类型的业务状态
                        var validationResult = ValidateBusinessStatusTransitionAsync(entity, statusType, targetStatus).Result;
                        if (!validationResult.IsValid)
                            return validationResult;

                        SetBusinessStatusAsync(entity, statusType, targetStatus, reason).Wait();
                        return StateTransitionResult.Success(message: string.Empty);
                    }
                }

                return StateTransitionResult.Failure($"不支持的状态类型: {statusType.Name}");
            }
            catch (Exception ex)
            {
                return StateTransitionResult.Failure($"状态转换失败: {ex.Message}");
            }
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
            if (_options?.TransitionRules != null)
            {
                foreach (var customRule in _options.TransitionRules)
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
    }
}