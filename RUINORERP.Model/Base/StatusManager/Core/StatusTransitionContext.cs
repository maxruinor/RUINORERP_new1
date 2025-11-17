/**
 * 文件: StatusTransitionContext.cs
 * 说明: 状态转换上下文实现 - v3版本
 * 创建日期: 2024年
 * 作者: RUINOR ERP开发团队
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.Global;

namespace RUINORERP.Model.Base.StatusManager.Core
{
    /// <summary>
    /// 状态转换上下文实现 - v3版本
    /// 提供状态转换过程中的上下文信息
    /// </summary>
    public class StatusTransitionContext : IStatusTransitionContext
    {
        #region 字段

        /// <summary>
        /// 日志记录器
        /// </summary>
        private readonly ILogger<StatusTransitionContext> _logger;

        /// <summary>
        /// 状态转换引擎
        /// </summary>
        private readonly IStatusTransitionEngine _transitionEngine;

        /// <summary>
        /// 状态管理器
        /// </summary>
        private readonly IUnifiedStateManager _statusManager;

        /// <summary>
        /// 转换历史记录
        /// </summary>
        private readonly List<IStatusTransitionRecord> _transitionHistory;

        #endregion

        #region 属性

        /// <summary>
        /// 实体对象
        /// </summary>
        public BaseEntity Entity { get; }

        /// <summary>
        /// 状态类型
        /// </summary>
        public Type StatusType { get; }

        /// <summary>
        /// 当前状态
        /// </summary>
        public object CurrentStatus { get; private set; }

        /// <summary>
        /// 转换原因
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 转换时间
        /// </summary>
        public DateTime TransitionTime { get; private set; }

        /// <summary>
        /// 附加数据
        /// </summary>
        public Dictionary<string, object> AdditionalData { get; }

        /// <summary>
        /// 状态变更事件
        /// </summary>
        public event EventHandler<StateTransitionEventArgs> StatusChanged;

        #endregion

        #region 构造函数

        /// <summary>
        /// 初始化状态转换上下文
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">状态类型</param>
        /// <param name="initialStatus">初始状态</param>
        /// <param name="statusManager">状态管理器</param>
        /// <param name="transitionEngine">状态转换引擎</param>
        /// <param name="logger">日志记录器</param>
        public StatusTransitionContext(
            BaseEntity entity,
            Type statusType,
            object initialStatus,
            IUnifiedStateManager statusManager = null,
            IStatusTransitionEngine transitionEngine = null,
            ILogger<StatusTransitionContext> logger = null)
        {
            Entity = entity ?? throw new ArgumentNullException(nameof(entity));
            StatusType = statusType ?? throw new ArgumentNullException(nameof(statusType));
            CurrentStatus = initialStatus;
            _statusManager = statusManager;
            _transitionEngine = transitionEngine ?? new StatusTransitionEngine();
            _logger = logger;
            _transitionHistory = new List<IStatusTransitionRecord>();
            AdditionalData = new Dictionary<string, object>();
            TransitionTime = DateTime.Now;
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 获取数据性状态
        /// </summary>
        /// <returns>数据性状态</returns>
        public DataStatus GetDataStatus()
        {
            if (StatusType == typeof(DataStatus))
            {
                return (DataStatus)CurrentStatus;
            }

            return _statusManager.GetDataStatus(Entity);
        }

        /// <summary>
        /// 获取业务性状态
        /// </summary>
        /// <typeparam name="T">业务状态枚举类型</typeparam>
        /// <returns>业务性状态</returns>
        public T GetBusinessStatus<T>() where T : Enum
        {
            if (StatusType == typeof(T))
            {
                return (T)CurrentStatus;
            }

            return _statusManager.GetBusinessStatus<T>(Entity);
        }

        /// <summary>
        /// 获取业务性状态
        /// </summary>
        /// <param name="statusType">业务状态枚举类型</param>
        /// <returns>业务性状态</returns>
        public object GetBusinessStatus(Type statusType)
        {
            if (statusType == null)
                throw new ArgumentNullException(nameof(statusType));
            
            if (StatusType == statusType)
            {
                return CurrentStatus;
            }

            // 获取实体的状态信息
            var entityStatus = _statusManager.GetEntityStatus(Entity);
            if (entityStatus?.BusinessStatuses != null && entityStatus.BusinessStatuses.TryGetValue(statusType, out var status))
            {
                return status;
            }

            return null;
        }

        /// <summary>
        /// 获取操作状态
        /// </summary>
        /// <returns>操作状态</returns>
        public ActionStatus GetActionStatus()
        {
            if (StatusType == typeof(ActionStatus))
            {
                return (ActionStatus)CurrentStatus;
            }

            return _statusManager.GetActionStatus(Entity);
        }

        /// <summary>
        /// 设置数据性状态
        /// </summary>
        /// <param name="status">状态值</param>
        /// <param name="reason">变更原因</param>
        /// <returns>设置是否成功</returns>
        public async Task<bool> SetDataStatusAsync(DataStatus status, string reason = null)
        {
            var oldStatus = GetDataStatus();
            var result = await _statusManager.SetDataStatusAsync(Entity, status, reason);
            
            if (result)
            {
                CurrentStatus = status;
                LogTransition(oldStatus, status, reason);
                OnStatusChanged(new StateTransitionEventArgs(Entity, typeof(DataStatus), oldStatus, status, reason));
            }

            return result;
        }

        /// <summary>
        /// 设置业务性状态
        /// </summary>
        /// <typeparam name="T">业务状态枚举类型</typeparam>
        /// <param name="status">状态值</param>
        /// <param name="reason">变更原因</param>
        /// <returns>设置是否成功</returns>
        public async Task<bool> SetBusinessStatusAsync<T>(T status, string reason = null) where T : Enum
        {
            var oldStatus = GetBusinessStatus<T>();
            var result = await _statusManager.SetBusinessStatusAsync(Entity, status, reason);
            
            if (result)
            {
                CurrentStatus = status;
                LogTransition(oldStatus, status, reason);
                OnStatusChanged(new StateTransitionEventArgs(Entity, typeof(T), oldStatus, status, reason));
            }

            return result;
        }

        /// <summary>
        /// 设置操作状态
        /// </summary>
        /// <param name="status">状态值</param>
        /// <param name="reason">变更原因</param>
        /// <returns>设置是否成功</returns>
        public async Task<bool> SetActionStatusAsync(ActionStatus status, string reason = null)
        {
            var oldStatus = GetActionStatus();
            var result = await _statusManager.SetActionStatusAsync(Entity, status, reason);
            
            if (result)
            {
                CurrentStatus = status;
                LogTransition(oldStatus, status, reason);
                OnStatusChanged(new StateTransitionEventArgs(Entity, typeof(ActionStatus), oldStatus, status, reason));
            }

            return result;
        }

        /// <summary>
        /// 获取转换历史
        /// </summary>
        /// <returns>转换历史记录</returns>
        public IEnumerable<IStatusTransitionRecord> GetTransitionHistory()
        {
            return _transitionHistory.AsReadOnly();
        }

        /// <summary>
        /// 请求状态转换
        /// </summary>
        /// <param name="targetStatus">目标状态</param>
        /// <param name="reason">转换原因</param>
        /// <returns>转换结果</returns>
        public async Task<StateTransitionResult> RequestTransitionAsync(object targetStatus, string reason = null)
        {
            if (targetStatus == null)
                throw new ArgumentNullException(nameof(targetStatus));

            var oldStatus = CurrentStatus;
            
            // 根据状态类型调用适当的转换方法
            StateTransitionResult result;
            if (StatusType == typeof(DataStatus))
            {
                result = await _transitionEngine.ExecuteTransitionAsync((DataStatus)oldStatus, (DataStatus)targetStatus, this);
            }
            else if (StatusType == typeof(ActionStatus))
            {
                result = await _transitionEngine.ExecuteTransitionAsync((ActionStatus)oldStatus, (ActionStatus)targetStatus, this);
            }
            else
            {
                // 对于其他枚举类型，使用反射调用泛型方法
                var method = _transitionEngine.GetType().GetMethod(nameof(_transitionEngine.ExecuteTransitionAsync));
                var genericMethod = method?.MakeGenericMethod(StatusType);
                result = await (Task<StateTransitionResult>)genericMethod?.Invoke(_transitionEngine, new[] { oldStatus, targetStatus, this });
            }
            
            if (result.IsValid)
            {
                CurrentStatus = targetStatus;
                LogTransition(oldStatus, targetStatus, reason);
                OnStatusChanged(new StateTransitionEventArgs(Entity, StatusType, oldStatus, targetStatus, reason));
            }

            return result;
        }

        /// <summary>
        /// 验证状态转换
        /// </summary>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>验证结果</returns>
        public async Task<StateTransitionResult> ValidateTransitionAsync(object targetStatus)
        {
            if (targetStatus == null)
                throw new ArgumentNullException(nameof(targetStatus));

            // 根据状态类型调用适当的验证方法
            if (StatusType == typeof(DataStatus))
            {
                return await _transitionEngine.ValidateTransitionAsync((DataStatus)CurrentStatus, (DataStatus)targetStatus, this);
            }
            else if (StatusType == typeof(ActionStatus))
            {
                return await _transitionEngine.ValidateTransitionAsync((ActionStatus)CurrentStatus, (ActionStatus)targetStatus, this);
            }
            else
            {
                // 对于其他枚举类型，使用反射调用泛型方法
                var method = _transitionEngine.GetType().GetMethod(nameof(_transitionEngine.ValidateTransitionAsync));
                var genericMethod = method?.MakeGenericMethod(StatusType);
                return await (Task<StateTransitionResult>)genericMethod?.Invoke(_transitionEngine, new[] { CurrentStatus, targetStatus, this });
            }
        }

        /// <summary>
        /// 记录转换
        /// </summary>
        /// <param name="fromStatus">源状态</param>
        /// <param name="toStatus">目标状态</param>
        /// <param name="reason">转换原因</param>
        public void LogTransition(object fromStatus, object toStatus, string reason = null)
        {
            var record = new StatusTransitionRecord
            {
                Id = Guid.NewGuid().ToString(),
                EntityId = Entity.PrimaryKeyID,
                EntityType = Entity.GetType(),
                StatusType = StatusType,
                FromStatus = fromStatus,
                ToStatus = toStatus,
                TransitionTime = DateTime.Now,
                UserId = UserId,
                Reason = reason ?? Reason,
                AdditionalData = new Dictionary<string, object>(AdditionalData)
            };

            _transitionHistory.Add(record);
            _logger?.LogInformation("状态转换记录: {EntityType} {EntityId} 从 {FromStatus} 转换到 {ToStatus}, 原因: {Reason}",
                record.EntityType, record.EntityId, record.FromStatus, record.ToStatus, record.Reason);
        }

        /// <summary>
        /// 转换到指定状态
        /// </summary>
        /// <param name="targetStatus">目标状态</param>
        /// <param name="reason">转换原因</param>
        /// <returns>转换结果</returns>
        public async Task<StateTransitionResult> TransitionTo(object targetStatus, string reason = null)
        {
            try
            {
                // 根据状态类型调用适当的泛型方法
                StateTransitionResult validationResult;
                
                if (StatusType == typeof(DataStatus))
                {
                    validationResult = await _transitionEngine.ValidateTransitionAsync((DataStatus)CurrentStatus, (DataStatus)targetStatus, this);
                }
                else if (StatusType == typeof(ActionStatus))
                {
                    validationResult = await _transitionEngine.ValidateTransitionAsync((ActionStatus)CurrentStatus, (ActionStatus)targetStatus, this);
                }
                else if (StatusType.IsEnum)
                {
                    // 使用反射调用泛型方法
                    var method = _transitionEngine.GetType().GetMethod(nameof(_transitionEngine.ValidateTransitionAsync));
                    var genericMethod = method.MakeGenericMethod(StatusType);
                    validationResult = await (Task<StateTransitionResult>)genericMethod.Invoke(_transitionEngine, new[] { CurrentStatus, targetStatus, this });
                }
                else
                {
                    return StateTransitionResult.Failure("不支持的状态类型");
                }
                
                if (!validationResult.IsValid)
                {
                    return validationResult;
                }

                // 执行转换
                StateTransitionResult result;
                
                if (StatusType == typeof(DataStatus))
                {
                    result = await _transitionEngine.ExecuteTransitionAsync((DataStatus)CurrentStatus, (DataStatus)targetStatus, this);
                }
                else if (StatusType == typeof(ActionStatus))
                {
                    result = await _transitionEngine.ExecuteTransitionAsync((ActionStatus)CurrentStatus, (ActionStatus)targetStatus, this);
                }
                else if (StatusType.IsEnum)
                {
                    // 使用反射调用泛型方法
                    var method = _transitionEngine.GetType().GetMethod(nameof(_transitionEngine.ExecuteTransitionAsync));
                    var genericMethod = method.MakeGenericMethod(StatusType);
                    result = await (Task<StateTransitionResult>)genericMethod.Invoke(_transitionEngine, new[] { CurrentStatus, targetStatus, this });
                }
                else
                {
                    return StateTransitionResult.Failure("不支持的状态类型");
                }
                
                if (result.IsValid)
                {
                    var oldStatus = CurrentStatus;
                    CurrentStatus = targetStatus;
                    Reason = reason ?? Reason;
                    TransitionTime = DateTime.Now;
                    
                    LogTransition(oldStatus, targetStatus, reason);
                    OnStatusChanged(new StateTransitionEventArgs(Entity, StatusType, oldStatus, targetStatus, reason));
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "状态转换失败：从 {FromStatus} 转换到 {ToStatus}", CurrentStatus, targetStatus);
                return StateTransitionResult.Failure($"状态转换失败：{ex.Message}", ex);
            }
        }

        /// <summary>
        /// 获取可转换状态列表
        /// </summary>
        /// <returns>可转换状态列表</returns>
        public IEnumerable<object> GetAvailableTransitions()
        {
            // 根据状态类型调用适当的泛型方法
            if (StatusType == typeof(DataStatus))
            {
                return _transitionEngine.GetAvailableTransitions((DataStatus)CurrentStatus, this).Cast<object>();
            }
            else if (StatusType == typeof(ActionStatus))
            {
                return _transitionEngine.GetAvailableTransitions((ActionStatus)CurrentStatus, this).Cast<object>();
            }
            else if (StatusType.IsEnum)
            {
                // 使用反射调用泛型方法
                var method = _transitionEngine.GetType().GetMethod(nameof(_transitionEngine.GetAvailableTransitions));
                var genericMethod = method.MakeGenericMethod(StatusType);
                var result = genericMethod.Invoke(_transitionEngine, new[] { CurrentStatus, this });
                return (IEnumerable<object>)result;
            }
            else
            {
                return new List<object>();
            }
        }

        /// <summary>
        /// 检查是否可以转换到指定状态
        /// </summary>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>是否可以转换</returns>
        public async Task<bool> CanTransitionTo(object targetStatus)
        {
            // 根据状态类型调用适当的泛型方法
            StateTransitionResult result;
            
            if (StatusType == typeof(DataStatus))
            {
                result = await _transitionEngine.ValidateTransitionAsync((DataStatus)CurrentStatus, (DataStatus)targetStatus, this);
            }
            else if (StatusType == typeof(ActionStatus))
            {
                result = await _transitionEngine.ValidateTransitionAsync((ActionStatus)CurrentStatus, (ActionStatus)targetStatus, this);
            }
            else if (StatusType.IsEnum)
            {
                // 使用反射调用泛型方法
                var method = _transitionEngine.GetType().GetMethod(nameof(_transitionEngine.ValidateTransitionAsync));
                var genericMethod = method.MakeGenericMethod(StatusType);
                result = await (Task<StateTransitionResult>)genericMethod.Invoke(_transitionEngine, new[] { CurrentStatus, targetStatus, this });
            }
            else
            {
                return false;
            }
            
            return result.IsValid;
        }

        #endregion

        #region 事件处理

        /// <summary>
        /// 触发状态变更事件
        /// </summary>
        /// <param name="e">事件参数</param>
        protected virtual void OnStatusChanged(StateTransitionEventArgs e)
        {
            StatusChanged?.Invoke(this, e);
        }

        #endregion

        #region 内部类

        /// <summary>
        /// 状态转换记录实现
        /// </summary>
        private class StatusTransitionRecord : IStatusTransitionRecord
        {
            public string Id { get; set; }
            public long EntityId { get; set; }
            public Type StatusType { get; set; }
            public object FromStatus { get; set; }
            public object ToStatus { get; set; }
            public DateTime TransitionTime { get; set; }
            public string UserId { get; set; }
            public string Reason { get; set; }
            public Dictionary<string, object> AdditionalData { get; set; }
            
            /// <summary>
            /// 实体类型
            /// </summary>
            public Type EntityType { get; set; }
        }

        #endregion
    }
}