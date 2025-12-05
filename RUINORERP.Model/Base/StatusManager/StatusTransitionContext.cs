/**
 * 文件: StatusTransitionContext.cs
 * 说明: 简化版状态转换上下文 - v3版本
 * 创建日期: 2024年
 * 作者: RUINOR ERP开发团队
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.Global;

namespace RUINORERP.Model.Base.StatusManager
{
    /// <summary>
    /// 简化版状态转换上下文 - v3版本
    /// 提供状态转换过程中的上下文信息
    /// 移除了复杂的状态设置方法，简化了状态转换逻辑
    /// </summary>
    public class StatusTransitionContext : IStatusTransitionContext
    {
        #region 事件

        /// <summary>
        /// 状态变化事件
        /// </summary>
        public event EventHandler<StateTransitionEventArgs> StatusChanged;

        #endregion

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
            AdditionalData = new Dictionary<string, object>();
            TransitionTime = DateTime.Now;
        }

        #endregion

        #region 公共方法

       

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
        /// 获取当前状态（泛型版本）
        /// </summary>
        /// <typeparam name="T">状态枚举类型</typeparam>
        /// <returns>当前状态</returns>
        public T GetCurrentStatus<T>() where T : struct, Enum
        {
            if (CurrentStatus == null)
                return default(T);

            try
            {
                // 将CurrentStatus转换为T类型
                return (T)Enum.Parse(typeof(T), CurrentStatus.ToString());
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// 获取业务性状态（泛型版本）
        /// </summary>
        /// <typeparam name="T">业务状态枚举类型</typeparam>
        /// <returns>业务性状态</returns>
        public T GetBusinessStatus<T>() where T : struct, Enum
        {
            var statusType = typeof(T);
            
            if (StatusType == statusType)
            {
                return (T)CurrentStatus;
            }

            // 获取实体的状态信息
            var entityStatus = _statusManager.GetEntityStatus(Entity);
            if (entityStatus?.BusinessStatuses != null && entityStatus.BusinessStatuses.TryGetValue(statusType, out var status))
            {
                return (T)status;
            }

            return default(T);
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
        /// 转换到指定状态
        /// </summary>
        /// <param name="targetStatus">目标状态</param>
        /// <param name="reason">转换原因</param>
        /// <returns>转换结果</returns>
        public async Task<StateTransitionResult> TransitionTo(object targetStatus, string reason = null)
        {
            try
            {
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

                if (result.IsSuccess)
                {
                    var oldStatus = CurrentStatus;
                    CurrentStatus = targetStatus;
                    Reason = reason ?? Reason;
                    TransitionTime = DateTime.Now;

                    LogTransition(oldStatus, targetStatus, reason);
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
            // 获取可用的转换状态列表，检查目标状态是否在其中
            var availableTransitions = GetAvailableTransitions();
            return availableTransitions.Any(t => t.Equals(targetStatus));
        }

        /// <summary>
        /// 检查是否可以转换到指定状态，并返回详细的提示消息
        /// </summary>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>包含是否可以转换和提示消息的结果对象</returns>
        public async Task<StateTransitionResult> CanTransitionToWithMessage(object targetStatus)
        {
            try
            {
                // 检查目标状态是否为空
                if (targetStatus == null)
                    return StateTransitionResult.Failure("目标状态不能为空");

                // 检查状态类型是否匹配
                if (targetStatus.GetType() != StatusType)
                    return StateTransitionResult.Failure($"目标状态类型 {targetStatus.GetType().Name} 与当前状态类型 {StatusType.Name} 不匹配");

                // 获取可用的转换状态列表
                var availableTransitions = GetAvailableTransitions();
                var canTransition = availableTransitions.Any(t => t.Equals(targetStatus));

                if (canTransition)
                {
                    return StateTransitionResult.Success($"可以从 {CurrentStatus} 转换到 {targetStatus}");
                }
                else
                {
                    var availableStatuses = string.Join(", ", availableTransitions);
                    return StateTransitionResult.Failure($"从 {CurrentStatus} 无法转换到 {targetStatus}，可用的转换状态为：{availableStatuses}");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "检查状态转换可行性失败: {EntityId}, 从 {CurrentStatus} 到 {TargetStatus}", 
                    Entity?.PrimaryKeyID, CurrentStatus, targetStatus);
                return StateTransitionResult.Failure($"检查状态转换可行性时发生错误: {ex.Message}");
            }
        }

        /// <summary>
        /// 设置实体数据状态
        /// </summary>
        /// <param name="dataStatus">数据状态</param>
        /// <returns>设置结果</returns>
        public async Task<bool> SetEntityDataStatus(DataStatus dataStatus)
        {
            try
            {
                if (_statusManager == null)
                {
                    _logger?.LogWarning("状态管理器未初始化，无法设置实体数据状态");
                    return false;
                }

                await _statusManager.SetDataStatusAsync(Entity, dataStatus);
                CurrentStatus = dataStatus;
                return true;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "设置实体数据状态失败: {EntityId}, {DataStatus}", Entity.PrimaryKeyID, dataStatus);
                return false;
            }
        }

        /// <summary>
        /// 验证状态转换
        /// </summary>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>验证结果</returns>
        public async Task<bool> ValidateTransitionAsync(object targetStatus)
        {
            try
            {
                if (targetStatus == null)
                {
                    _logger?.LogWarning("目标状态为空，无法验证状态转换");
                    return false;
                }

                // 检查状态类型是否匹配
                if (targetStatus.GetType() != StatusType)
                {
                    _logger?.LogWarning("目标状态类型 {TargetType} 与当前状态类型 {CurrentType} 不匹配", 
                        targetStatus.GetType().Name, StatusType.Name);
                    return false;
                }

                // 检查是否可以转换到目标状态
                return await CanTransitionTo(targetStatus);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "验证状态转换失败: {EntityId}, 从 {CurrentStatus} 到 {TargetStatus}", 
                    Entity.PrimaryKeyID, CurrentStatus, targetStatus);
                return false;
            }
        }

        /// <summary>
        /// 设置数据状态
        /// </summary>
        /// <param name="status">数据状态</param>
        /// <param name="reason">转换原因</param>
        /// <returns>设置结果</returns>
        public async Task<bool> SetDataStatusAsync(DataStatus status, string reason = null)
        {
            try
            {
                if (_statusManager == null)
                {
                    _logger?.LogWarning("状态管理器未初始化，无法设置数据状态");
                    return false;
                }

                await _statusManager.SetDataStatusAsync(Entity, status);
                CurrentStatus = status;
                Reason = reason ?? Reason;
                TransitionTime = DateTime.Now;

                // 触发状态变化事件
                StatusChanged?.Invoke(this, new StateTransitionEventArgs(
                    Entity,
                    StatusType,
                    CurrentStatus,
                    status,
                    reason,
                    UserId,
                    TransitionTime,
                    AdditionalData));

                return true;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "设置数据状态失败: {EntityId}, {DataStatus}", Entity.PrimaryKeyID, status);
                return false;
            }
        }

        /// <summary>
        /// 设置业务状态
        /// </summary>
        /// <param name="status">业务状态</param>
        /// <param name="reason">转换原因</param>
        /// <returns>设置结果</returns>
        public async Task<bool> SetBusinessStatusAsync(Enum status, string reason = null)
        {
            try
            {
                if (_statusManager == null)
                {
                    _logger?.LogWarning("状态管理器未初始化，无法设置业务状态");
                    return false;
                }

                // 使用反射调用泛型方法
                var method = _statusManager.GetType().GetMethod(nameof(_statusManager.SetBusinessStatusAsync));
                var genericMethod = method?.MakeGenericMethod(status.GetType());
                
                if (genericMethod != null)
                {
                    await (Task)genericMethod.Invoke(_statusManager, new object[] { Entity, status });
                    CurrentStatus = status;
                    Reason = reason ?? Reason;
                    TransitionTime = DateTime.Now;

                    // 触发状态变化事件
                    StatusChanged?.Invoke(this, new StateTransitionEventArgs(
                        Entity,
                        StatusType,
                        CurrentStatus,
                        status,
                        reason,
                        UserId,
                        TransitionTime,
                        AdditionalData));

                    return true;
                }
                else
                {
                    _logger?.LogError("找不到设置业务状态的方法: {StatusType}", status.GetType().Name);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "设置业务状态失败: {EntityId}, {BusinessStatus}", Entity.PrimaryKeyID, status);
                return false;
            }
        }

        /// <summary>
        /// 设置操作状态
        /// </summary>
        /// <param name="status">操作状态</param>
        /// <param name="reason">转换原因</param>
        /// <returns>设置结果</returns>
        public async Task<bool> SetActionStatusAsync(ActionStatus status, string reason = null)
        {
            try
            {
                if (_statusManager == null)
                {
                    _logger?.LogWarning("状态管理器未初始化，无法设置操作状态");
                    return false;
                }

                await _statusManager.SetActionStatusAsync(Entity, status);
                CurrentStatus = status;
                Reason = reason ?? Reason;
                TransitionTime = DateTime.Now;

                // 触发状态变化事件
                StatusChanged?.Invoke(this, new StateTransitionEventArgs(
                    Entity,
                    StatusType,
                    CurrentStatus,
                    status,
                    reason,
                    UserId,
                    TransitionTime,
                    AdditionalData));

                return true;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "设置操作状态失败: {EntityId}, {ActionStatus}", Entity.PrimaryKeyID, status);
                return false;
            }
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 记录转换
        /// </summary>
        /// <param name="fromStatus">源状态</param>
        /// <param name="toStatus">目标状态</param>
        /// <param name="reason">转换原因</param>
        public void LogTransition(object fromStatus, object toStatus, string reason = null)
        {
            _logger?.LogInformation("状态转换记录: {EntityType} {EntityId} 从 {FromStatus} 转换到 {ToStatus}, 原因: {Reason}",
                Entity.GetType(), Entity.PrimaryKeyID, fromStatus, toStatus, reason ?? Reason);
        }

        #endregion
    }
}