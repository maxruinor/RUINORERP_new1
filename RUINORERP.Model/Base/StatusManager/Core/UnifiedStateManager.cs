/**
 * 文件: UnifiedStateManager.cs
 * 说明: 统一状态管理器实现 - v3版本（优化版）
 * 创建日期: 2024年
 * 作者: RUINOR ERP开发团队
 * 优化说明: 添加批量状态设置、增强反射缓存、提供异步方法支持
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
using RUINORERP.Model.Base.StatusManager.Factory;

namespace RUINORERP.Model.Base.StatusManager.Core
{
    /// <summary>
    /// 统一状态管理器实现 - 整合数据性状态和业务性状态管理
    /// </summary>
    public class UnifiedStateManager : IUnifiedStateManager, IDisposable
    {
        #region IUnifiedStateManager 接口实现
        
        /// <summary>
        /// 检查是否可以转换到指定的数据状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="targetStatus">目标数据状态</param>
        /// <returns>包含转换结果和错误消息的元组</returns>
        public async Task<(bool CanTransition, string ErrorMessage)> CanTransitionToDataStatusAsync(BaseEntity entity, DataStatus targetStatus)
        {
            string errorMessage = null;
            
            try
            {
                // 获取当前数据状态
                var currentStatus = GetDataStatus(entity);
                
                // 检查状态转换规则
                var transitionResult = await CanTransitionToDataStatusAsync(entity, targetStatus);
                var canTransition = transitionResult.CanTransition;
                
                if (!canTransition)
                {
                    errorMessage = transitionResult.ErrorMessage ?? GetTransitionErrorMessage(entity, targetStatus);
                }
                
                return (canTransition, errorMessage);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "检查数据状态转换时发生错误");
                errorMessage = "状态转换检查失败: " + ex.Message;
                return (false, errorMessage);
            }
        }
        
        #endregion
        
        #region 私有字段

        private readonly ILogger<UnifiedStateManager> _logger;
        private readonly StateManagerOptions _options;
        private readonly Dictionary<Type, Dictionary<object, List<object>>> _transitionRules;
        private readonly Dictionary<Type, Func<BaseEntity, object>> _statusGetters;
        private readonly Dictionary<Type, Action<BaseEntity, object>> _statusSetters;
        
        // 优化的反射缓存，使用委托提高性能
        private static readonly ConcurrentDictionary<Type, Func<BaseEntity, DataStatus>> _dataStatusGetterCache = 
            new ConcurrentDictionary<Type, Func<BaseEntity, DataStatus>>();
        
        private static readonly ConcurrentDictionary<Type, Action<BaseEntity, DataStatus>> _dataStatusSetterCache = 
            new ConcurrentDictionary<Type, Action<BaseEntity, DataStatus>>();
        
        // 业务状态缓存，使用泛型委托
        private static readonly ConcurrentDictionary<Type, Dictionary<Type, Func<BaseEntity, object>>> _businessStatusGetterCache = 
            new ConcurrentDictionary<Type, Dictionary<Type, Func<BaseEntity, object>>>();
        
        private static readonly ConcurrentDictionary<Type, Dictionary<Type, Action<BaseEntity, object>>> _businessStatusSetterCache = 
            new ConcurrentDictionary<Type, Dictionary<Type, Action<BaseEntity, object>>>();
        
        // 枚举类型缓存
        private static readonly ConcurrentDictionary<Type, bool> _isEnumTypeCache = 
            new ConcurrentDictionary<Type, bool>();
        
        // 状态转换错误消息缓存
        private static readonly ConcurrentDictionary<Tuple<Type, object, object>, string> _transitionErrorMessageCache = 
            new ConcurrentDictionary<Tuple<Type, object, object>, string>();
        
        // 属性信息缓存
        private static readonly ConcurrentDictionary<Type, PropertyInfo> _dataStatusPropertyCache = 
            new ConcurrentDictionary<Type, PropertyInfo>();
        
        private static readonly ConcurrentDictionary<Type, List<PropertyInfo>> _businessStatusPropertyCache = 
            new ConcurrentDictionary<Type, List<PropertyInfo>>();
        
        private static readonly ConcurrentDictionary<Type, PropertyInfo> _actionStatusPropertyCache = 
            new ConcurrentDictionary<Type, PropertyInfo>();
        
        // 所有属性缓存
        private static readonly ConcurrentDictionary<Type, Dictionary<string, PropertyInfo>> _allPropertiesCache = 
            new ConcurrentDictionary<Type, Dictionary<string, PropertyInfo>>();
        
        // ActionStatus相关缓存
        private static readonly ConcurrentDictionary<Type, Func<BaseEntity, ActionStatus>> _actionStatusGetterCache = 
            new ConcurrentDictionary<Type, Func<BaseEntity, ActionStatus>>();
        
        private static readonly ConcurrentDictionary<Type, Action<BaseEntity, ActionStatus>> _actionStatusSetterCache = 
            new ConcurrentDictionary<Type, Action<BaseEntity, ActionStatus>>();

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public UnifiedStateManager() : this(new StateManagerOptions())
        {
            // 预初始化通用枚举类型缓存
            _isEnumTypeCache.TryAdd(typeof(DataStatus), true);
            _isEnumTypeCache.TryAdd(typeof(ActionStatus), true);
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
        /// 批量设置实体状态（基于EntityStatus）
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

            // 使用优化的批量设置方法
            var states = new Dictionary<Type, object>();
            
            // 添加数据性状态
            if (status.dataStatus.HasValue)
            {
                states[typeof(DataStatus)] = status.dataStatus.Value;
            }

            // 添加操作性状态
            if (status.actionStatus.HasValue)
            {
                states[typeof(ActionStatus)] = status.actionStatus.Value;
            }
            
            // 添加业务性状态
            if (status.BusinessStatuses != null)
            {
                foreach (var kvp in status.BusinessStatuses)
                {
                    states[kvp.Key] = kvp.Value;
                }
            }
            
            return await SetStatesAsync(entity, states, reason);
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
        /// 检查是否可以转换到目标数据性状态（同步版本）
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
        /// 检查是否可以转换到目标数据性状态，并输出错误信息
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="targetStatus">目标状态</param>
        /// <param name="errorMessage">错误信息输出参数</param>
        /// <returns>是否可以转换</returns>
        public bool CanTransitionToDataStatus(BaseEntity entity, DataStatus targetStatus, out string errorMessage)
        {
            var result = ValidateDataStatusTransitionAsync(entity, targetStatus).Result;
            errorMessage = result.IsValid ? string.Empty : result.Message;
            return result.IsValid;
        }
        
        /// <summary>
        /// 批量设置实体状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="states">要设置的状态集合，键为状态类型，值为状态值</param>
        /// <param name="reason">变更原因</param>
        /// <returns>设置是否成功</returns>
        public async Task<bool> SetStatesAsync(BaseEntity entity, Dictionary<Type, object> states, string reason = null)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (states == null || states.Count == 0)
                return true;

            var result = true;
            
            // 先验证所有状态转换是否有效
            var validationResults = new Dictionary<Type, StateTransitionResult>();
            foreach (var kvp in states)
            {
                var statusType = kvp.Key;
                var targetStatus = kvp.Value;
                
                if (statusType == typeof(DataStatus) && targetStatus is DataStatus dataStatus)
                {
                    var validation = await ValidateDataStatusTransitionAsync(entity, dataStatus);
                    validationResults[statusType] = validation;
                    if (!validation.IsValid)
                    {
                        result = false;
                        // 可以选择继续验证其他状态，或者立即返回
                    }
                }
                else if (statusType == typeof(ActionStatus) && targetStatus is ActionStatus actionStatus)
                {
                    var validation = await ValidateActionStatusTransitionAsync(entity, actionStatus);
                    validationResults[statusType] = validation;
                    if (!validation.IsValid)
                    {
                        result = false;
                    }
                }
                else if (_isEnumTypeCache.GetOrAdd(statusType, t => t.IsEnum))
                {
                    var validation = await ValidateBusinessStatusTransitionAsync(entity, statusType, targetStatus);
                    validationResults[statusType] = validation;
                    if (!validation.IsValid)
                    {
                        result = false;
                    }
                }
            }
 
            
            // 检查是否有验证错误
            var hasValidationErrors = validationResults.Values.Any(r => !r.IsValid);
            
            // 执行状态设置
            foreach (var kvp in states)
            {
                var statusType = kvp.Key;
                var targetStatus = kvp.Value;
                var success = false;
                
                // 只有验证通过的状态才执行设置
                if (!validationResults.ContainsKey(statusType) || validationResults[statusType].IsValid)
                {
                    if (statusType == typeof(DataStatus) && targetStatus is DataStatus dataStatus)
                    {
                        success = await SetDataStatusAsync(entity, dataStatus, reason);
                    }
                    else if (statusType == typeof(ActionStatus) && targetStatus is ActionStatus actionStatus)
                    {
                        success = await SetActionStatusAsync(entity, actionStatus, reason);
                    }
                    else if (_isEnumTypeCache[statusType])
                    {
                        // 使用反射动态调用泛型方法以避免CS0311错误
                        var method = typeof(UnifiedStateManager).GetMethod("SetBusinessStatusAsync", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic, null, new[] { typeof(BaseEntity), typeof(object), typeof(string) }, null);
                        var genericMethod = method.MakeGenericMethod(statusType);
                        success = await (Task<bool>)genericMethod.Invoke(this, new object[] { entity, targetStatus, reason });
                    }
                }
                
                result &= success;
            }
            
            return result;
        }
        
        /// <summary>
        /// 批量设置实体状态
        /// 支持同时设置数据性状态、业务状态和操作状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="dataStatus">数据状态</param>
        /// <param name="businessStatus">业务状态</param>
        /// <param name="actionStatus">操作状态</param>
        /// <returns>设置是否成功</returns>
        public async Task<bool> SetStatesAsync(BaseEntity entity, DataStatus dataStatus, Enum businessStatus, ActionStatus actionStatus)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

 
            
            // 验证数据状态转换
            var dataValidationResult = await ValidateDataStatusTransitionAsync(entity, dataStatus);
            
            // 验证业务状态转换
            var businessValidationResult = await ValidateBusinessStatusTransitionAsync(entity, businessStatus.GetType(), businessStatus);
            
            // 验证操作状态转换
            var actionValidationResult = await ValidateActionStatusTransitionAsync(entity, actionStatus);
            
 
            
            // 3. 执行各个状态的设置（只设置验证通过的状态）
            if (dataValidationResult.IsValid)
            {
                await SetDataStatusAsync(entity, dataStatus);
            }
            
            if (businessValidationResult.IsValid)
            {
                // 使用动态调用避免泛型约束问题
                var method = typeof(UnifiedStateManager).GetMethod("SetBusinessStatusAsync", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                var genericMethod = method.MakeGenericMethod(businessStatus.GetType());
                await (Task<bool>)genericMethod.Invoke(this, new object[] { entity, businessStatus, null });
            }
            
            if (actionValidationResult.IsValid)
            {
                await SetActionStatusAsync(entity, actionStatus);
            }
            
            // 4. 如果有验证错误但不是严格模式，抛出异常
            var hasValidationErrors = !dataValidationResult.IsValid || !businessValidationResult.IsValid || !actionValidationResult.IsValid;
            var validationResults = new List<StateTransitionResult> { dataValidationResult, businessValidationResult, actionValidationResult };
            
            if (hasValidationErrors && _options?.StrictMode != true)
            {
                throw new InvalidOperationException("部分状态转换验证失败: " + string.Join(", ", validationResults.Where(r => !r.IsValid).Select(r => r.Message)));
            }
            
            return true;
        }
        
        /// <summary>
        /// 批量设置实体状态（支持object类型参数）
        /// 支持同时设置数据性状态、业务状态和操作状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="dataStatus">数据状态</param>
        /// <param name="businessStatus">业务状态</param>
        /// <param name="actionStatus">操作状态</param>
        /// <returns>设置是否成功</returns>
        public async Task<bool> SetStatesAsync(object entity, DataStatus dataStatus, Enum businessStatus, ActionStatus actionStatus)
        {
            if (entity is BaseEntity baseEntity)
            {
                return await SetStatesAsync(baseEntity, dataStatus, businessStatus, actionStatus);
            }
            else
            {
                throw new ArgumentException("实体对象必须是BaseEntity类型", nameof(entity));
            }
        }
        
        /// <summary>
        /// 检查状态是否可以更改（支持object类型参数和错误信息输出）
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="targetStatus">目标状态</param>
        /// <param name="errorMessage">错误信息输出参数</param>
        /// <returns>是否可以更改</returns>
        public bool CanChangeStatus(object entity, Enum targetStatus, out string errorMessage)
        {
            errorMessage = string.Empty;
            
            if (entity is BaseEntity baseEntity)
            {
                // 根据状态类型执行不同的验证逻辑
                if (targetStatus is DataStatus dataStatus)
                {
                    var result = ValidateDataStatusTransitionAsync(baseEntity, dataStatus).Result;
                    errorMessage = result.IsValid ? string.Empty : result.Message;
                    return result.IsValid;
                }
                else if (targetStatus is ActionStatus actionStatus)
                {
                    var result = ValidateActionStatusTransitionAsync(baseEntity, actionStatus).Result;
                    errorMessage = result.IsValid ? string.Empty : result.Message;
                    return result.IsValid;
                }
                else
                {   // 业务状态
                    var result = ValidateBusinessStatusTransitionAsync(baseEntity, targetStatus.GetType(), targetStatus).Result;
                    errorMessage = result.IsValid ? string.Empty : result.Message;
                    return result.IsValid;
                }
            }
            else
            {
                errorMessage = "实体对象必须是BaseEntity类型";
                return false;
            }
        }
        
        /// <summary>
        /// 检查状态是否可以更改
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">状态类型</param>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>是否可以更改</returns>
        public async Task<bool> CanChangeStatus(BaseEntity entity, Type statusType, object targetStatus)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (statusType == null)
                throw new ArgumentNullException(nameof(statusType));
            if (targetStatus == null)
                throw new ArgumentNullException(nameof(targetStatus));
            
            // 检查当前状态是否与目标状态相同
            object currentStatus = null;
            
            if (statusType == typeof(DataStatus) && targetStatus is DataStatus)
            {
                currentStatus = GetDataStatus(entity);
            }
            else if (statusType == typeof(ActionStatus) && targetStatus is ActionStatus)
            {
                currentStatus = GetActionStatus(entity);
            }
            else if (_isEnumTypeCache.GetOrAdd(statusType, t => t.IsEnum))
            {
                currentStatus = GetBusinessStatus(entity, statusType);
            }
            else
            {
                return false;
            }
            
            // 如果当前状态与目标状态相同，则不需要更改
            if (Equals(currentStatus, targetStatus))
            {
                return true;
            }
            
            // 检查是否可以转换
            if (statusType == typeof(DataStatus) && targetStatus is DataStatus dataStatus)
            {
                return await CanTransitionDataStatusAsync(entity, dataStatus);
            }
            else if (statusType == typeof(ActionStatus) && targetStatus is ActionStatus actionStatus)
            {
                return await CanTransitionActionStatusAsync(entity, actionStatus);
            }
            else if (_isEnumTypeCache[statusType])
            {
                return await CanTransitionBusinessStatusAsync(entity, statusType, targetStatus);
            }
            
            return false;
        }
        
        /// <summary>
        /// 检查实体是否处于指定状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="statuses">状态值数组</param>
        /// <returns>是否处于指定状态</returns>
        public bool IsInStatus(object entity, params Enum[] statuses)
        {
            if (entity is BaseEntity baseEntity && statuses.Length > 0)
            {
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
                    {   // 业务状态
                        try
                        {
                            var currentBusinessStatus = GetBusinessStatus(baseEntity, status.GetType());
                            if (Equals(currentBusinessStatus, status))
                                return true;
                        }
                        catch { /* 忽略无法获取的业务状态 */ }
                    }
                }
            }
            return false;
        }
        
        /// <summary>
        /// 检查实体是否处于指定状态
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">状态类型</param>
        /// <param name="statusValue">状态值</param>
        /// <returns>是否处于指定状态</returns>
        public bool IsInStatus(BaseEntity entity, Type statusType, object statusValue)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (statusType == null)
                throw new ArgumentNullException(nameof(statusType));
            
            object currentStatus = null;
            
            if (statusType == typeof(DataStatus) && statusValue is DataStatus)
            {
                currentStatus = GetDataStatus(entity);
            }
            else if (statusType == typeof(ActionStatus) && statusValue is ActionStatus)
            {
                currentStatus = GetActionStatus(entity);
            }
            else if (_isEnumTypeCache.GetOrAdd(statusType, t => t.IsEnum))
            {
                currentStatus = GetBusinessStatus(entity, statusType);
            }
            else
            {
                return false;
            }
            
            return Equals(currentStatus, statusValue);
        }
        
        /// <summary>
        /// 获取状态转换错误消息（支持object类型参数）
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>错误消息，如果可以转换则返回空字符串</returns>
        public string GetTransitionErrorMessage(object entity, Enum targetStatus)
        {
            if (entity is BaseEntity baseEntity)
            {
                if (targetStatus is DataStatus dataStatus)
                {
                    var result = ValidateDataStatusTransitionAsync(baseEntity, dataStatus).Result;
                    return result.IsValid ? string.Empty : result.Message;
                }
                else if (targetStatus is ActionStatus actionStatus)
                {
                    var result = ValidateActionStatusTransitionAsync(baseEntity, actionStatus).Result;
                    return result.IsValid ? string.Empty : result.Message;
                }
                else
                {
                    var result = ValidateBusinessStatusTransitionAsync(baseEntity, targetStatus.GetType(), targetStatus).Result;
                    return result.IsValid ? string.Empty : result.Message;
                }
            }
            return "实体对象必须是BaseEntity类型";
        }
        
        /// <summary>
        /// 获取状态转换错误消息
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">状态类型</param>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>错误消息，如果可以转换则返回空字符串</returns>
        public async Task<string> GetTransitionErrorMessage(BaseEntity entity, Type statusType, object targetStatus)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));
            if (statusType == null)
                throw new ArgumentNullException(nameof(statusType));
            if (targetStatus == null)
                throw new ArgumentNullException(nameof(targetStatus));
            
            // 获取当前状态
            object currentStatus = null;
            StateTransitionResult validationResult = null;
            
            if (statusType == typeof(DataStatus) && targetStatus is DataStatus dataStatus)
            {
                currentStatus = GetDataStatus(entity);
                validationResult = await ValidateDataStatusTransitionAsync(entity, dataStatus);
            }
            else if (statusType == typeof(ActionStatus) && targetStatus is ActionStatus actionStatus)
            {
                currentStatus = GetActionStatus(entity);
                validationResult = await ValidateActionStatusTransitionAsync(entity, actionStatus);
            }
            else if (_isEnumTypeCache.GetOrAdd(statusType, t => t.IsEnum))
            {
                currentStatus = GetBusinessStatus(entity, statusType);
                validationResult = await ValidateBusinessStatusTransitionAsync(entity, statusType, targetStatus);
            }
            else
            {
                return "不支持的状态类型";
            }
            
            // 如果状态相同，不需要转换
            if (Equals(currentStatus, targetStatus))
            {
                return string.Empty;
            }
            
            // 缓存错误消息以提高性能
            if (validationResult != null)
            {
                var cacheKey = Tuple.Create(statusType, currentStatus, targetStatus);
                
                if (!validationResult.IsValid && !string.IsNullOrEmpty(validationResult.Message))
                {
                    // 缓存错误消息
                    _transitionErrorMessageCache[cacheKey] = validationResult.Message;
                    return validationResult.Message;
                }
                else
                {
                    // 缓存成功状态
                    _transitionErrorMessageCache[cacheKey] = string.Empty;
                }
            }
            
            return string.Empty;
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
            // 注意：这是重复初始化点，应该使用单例工厂中的规则
            // 但由于UnifiedStateManager可能被直接实例化，暂时保留原有逻辑
            
            // 检查是否可以通过工厂获取规则
            try
            {
                var factory = StateManagerFactoryV3.Instance;
                // 如果工厂已初始化，可以尝试从工厂获取规则
                // 这里需要根据实际API调整
            }
            catch
            {
                // 如果工厂不可用，使用默认规则
            }
            
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
        
        /// <summary>
        /// 优化的属性获取方法，使用缓存提高性能
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="propertyName">属性名</param>
        /// <returns>属性信息</returns>
        private PropertyInfo GetCachedProperty(Type entityType, string propertyName)
        {
            var typeProperties = _allPropertiesCache.GetOrAdd(entityType, type =>
            {
                return type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    .ToDictionary(p => p.Name, StringComparer.OrdinalIgnoreCase);
            });
            
            typeProperties.TryGetValue(propertyName, out var property);
            return property;
        }



        #endregion

        #region 清理和缓存管理

        /// <summary>
        /// 资源释放标志
        /// </summary>
        private bool _disposed = false;

        /// <summary>
        /// 清理所有缓存
        /// </summary>
        public void ClearCache()
        {
            try
            {
                // 清理反射缓存
                _dataStatusGetterCache.Clear();
                _dataStatusSetterCache.Clear();
                _businessStatusGetterCache.Clear();
                _businessStatusSetterCache.Clear();
                _actionStatusGetterCache.Clear();
                _actionStatusSetterCache.Clear();
                _dataStatusPropertyCache.Clear();
                _businessStatusPropertyCache.Clear();
                _actionStatusPropertyCache.Clear();
                _allPropertiesCache.Clear();

                // 清理转换规则
                _transitionRules.Clear();

                _logger?.LogInformation("UnifiedStateManager缓存清理完成");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "清理UnifiedStateManager缓存时发生错误");
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="disposing">是否释放托管资源</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // 释放托管资源
                    ClearCache();
                    
                    // 清理事件订阅
                    StatusChanged = null;
                    
                    _logger?.LogInformation("UnifiedStateManager资源已释放");
                }

                _disposed = true;
            }
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~UnifiedStateManager()
        {
            Dispose(false);
        }

        #endregion
    }
}