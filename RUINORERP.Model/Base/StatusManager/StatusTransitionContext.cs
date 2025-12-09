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
        /// <param name="logger">日志记录器</param>
        public StatusTransitionContext(
            BaseEntity entity,
            Type statusType,
            object initialStatus,
            IUnifiedStateManager statusManager = null,
            ILogger<StatusTransitionContext> logger = null)
        {
            Entity = entity ?? throw new ArgumentNullException(nameof(entity));
            StatusType = statusType ?? throw new ArgumentNullException(nameof(statusType));
            CurrentStatus = initialStatus;
            _statusManager = statusManager;
            
            // 不再使用状态转换引擎，直接使用StateTransitionRules
            // 直接使用StateTransitionRules，不再依赖_transitionEngine
            
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
            var entityStatus = _statusManager?.GetEntityStatus(Entity);
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
            var entityStatus = _statusManager?.GetEntityStatus(Entity);
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

            return _statusManager?.GetActionStatus(Entity) ?? default(ActionStatus);
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

            return _statusManager?.GetDataStatus(Entity) ?? default(DataStatus);
        }

        /// <summary>
        /// 执行状态转换
        /// </summary>
        /// <param name="targetStatus">目标状态</param>
        /// <param name="reason">转换原因</param>
        /// <returns>转换结果</returns>
        public async Task<StateTransitionResult> TransitionTo(object targetStatus, string reason = null)
        {
            try
            {
                if (targetStatus == null)
                    return StateTransitionResult.Failure("目标状态不能为空");

                if (StatusType == null)
                    return StateTransitionResult.Failure("状态类型未设置");

                // 检查状态类型是否匹配
                if (targetStatus.GetType() != StatusType)
                    return StateTransitionResult.Failure($"目标状态类型 {targetStatus.GetType().Name} 与当前状态类型 {StatusType.Name} 不匹配");

                // 使用状态管理器验证状态转换是否允许
                StateTransitionResult result = null;
                
                if (StatusType == typeof(DataStatus))
                {
                    result = await _statusManager.ValidateDataStatusTransitionAsync(Entity, (DataStatus)targetStatus);
                }
                else if (StatusType == typeof(ActionStatus))
                {
                    result = await _statusManager.ValidateActionStatusTransitionAsync(Entity, (ActionStatus)targetStatus);
                }
                else
                {
                    // 对于其他业务状态类型，使用非泛型方法
                    result = await _statusManager.ValidateBusinessStatusTransitionAsync(Entity, StatusType, targetStatus);
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
            // 根据状态类型调用状态管理器提供的相应方法
            if (StatusType == typeof(DataStatus))
            {
                return _statusManager.GetAvailableDataStatusTransitions(Entity).Cast<object>();
            }
            else if (StatusType == typeof(ActionStatus))
            {
                return _statusManager.GetAvailableActionStatusTransitions(Entity).Cast<object>();
            }
            else
            {
                // 对于其他业务状态类型，使用非泛型方法
                return _statusManager.GetAvailableBusinessStatusTransitions(Entity, StatusType);
            }
        }

        /// <summary>
        /// 检查是否可以转换到指定状态
        /// </summary>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>是否可以转换</returns>
        public Task<bool> CanTransitionTo(object targetStatus)
        {
            // 获取可用的转换状态列表，检查目标状态是否在其中
            var availableTransitions = GetAvailableTransitions();
            return Task.FromResult(availableTransitions.Any(t => t.Equals(targetStatus)));
        }

        /// <summary>
        /// 检查是否可以转换到指定状态，并返回详细的提示消息
        /// </summary>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>包含是否可以转换和提示消息的结果对象</returns>
        public Task<StateTransitionResult> CanTransitionToWithMessage(object targetStatus)
        {
            try
            {
                // 检查目标状态是否为空
                if (targetStatus == null)
                    return Task.FromResult(StateTransitionResult.Failure("目标状态不能为空"));

                // 检查状态类型是否匹配
                if (targetStatus.GetType() != StatusType)
                    return Task.FromResult(StateTransitionResult.Failure($"目标状态类型 {targetStatus.GetType().Name} 与当前状态类型 {StatusType.Name} 不匹配"));

                // 获取可用的转换状态列表
                var availableTransitions = GetAvailableTransitions();
                var canTransition = availableTransitions.Any(t => t.Equals(targetStatus));

                if (canTransition)
                {
                    return Task.FromResult(StateTransitionResult.Success($"可以从 {CurrentStatus} 转换到 {targetStatus}"));
                }
                else
                {
                    var availableStatuses = string.Join(", ", availableTransitions);
                    return Task.FromResult(StateTransitionResult.Failure($"从 {CurrentStatus} 无法转换到 {targetStatus}，可用的转换状态为：{availableStatuses}"));
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "检查状态转换可行性失败: {EntityId}, 从 {CurrentStatus} 到 {TargetStatus}", 
                    Entity?.PrimaryKeyID, CurrentStatus, targetStatus);
                return Task.FromResult(StateTransitionResult.Failure($"检查状态转换可行性时发生错误: {ex.Message}"));
            }
        }

        /// <summary>
        /// 设置实体数据状态
        /// </summary>
        /// <param name="dataStatus">数据状态</param>
        /// <returns>设置结果</returns>
        public Task<bool> SetEntityDataStatus(DataStatus dataStatus)
        {
            try
            {
                if (_statusManager == null)
                {
                    _logger?.LogWarning("状态管理器未初始化，无法设置实体数据状态");
                    return Task.FromResult(false);
                }

                _statusManager.SetDataStatusAsync(Entity, dataStatus).Wait();
                CurrentStatus = dataStatus;
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "设置实体数据状态失败: {EntityId}, {DataStatus}", Entity.PrimaryKeyID, dataStatus);
                return Task.FromResult(false);
            }
        }

        /// <summary>
        /// 验证状态转换
        /// </summary>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>验证结果</returns>
        public Task<bool> ValidateTransitionAsync(object targetStatus)
        {
            try
            {
                if (targetStatus == null)
                {
                    _logger?.LogWarning("目标状态为空，无法验证状态转换");
                    return Task.FromResult(false);
                }

                // 检查状态类型是否匹配
                if (targetStatus.GetType() != StatusType)
                {
                    _logger?.LogWarning("目标状态类型 {TargetType} 与当前状态类型 {CurrentType} 不匹配", 
                        targetStatus.GetType().Name, StatusType.Name);
                    return Task.FromResult(false);
                }

                // 检查是否可以转换到目标状态
                return CanTransitionTo(targetStatus);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "验证状态转换失败: {EntityId}, 从 {CurrentStatus} 到 {TargetStatus}", 
                    Entity.PrimaryKeyID, CurrentStatus, targetStatus);
                return Task.FromResult(false);
            }
        }

        /// <summary>
        /// 设置数据状态
        /// </summary>
        /// <param name="status">数据状态</param>
        /// <param name="reason">转换原因</param>
        /// <returns>设置结果</returns>
        public Task<bool> SetDataStatusAsync(DataStatus status, string reason = null)
        {
            try
            {
                if (_statusManager == null)
                {
                    _logger?.LogWarning("状态管理器未初始化，无法设置数据状态");
                    return Task.FromResult(false);
                }

                var oldStatus = CurrentStatus;
                
                // 通过状态管理器设置状态，状态管理器会负责触发事件
                _statusManager.SetDataStatusAsync(Entity, status).Wait();
                CurrentStatus = status;
                Reason = reason ?? Reason;
                TransitionTime = DateTime.Now;

                // 注意：状态变更事件由状态管理器统一管理，不再在此处直接触发
                // 状态管理器会在设置状态时自动触发相应的事件

                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "设置数据状态失败: {EntityId}, {DataStatus}", Entity.PrimaryKeyID, status);
                return Task.FromResult(false);
            }
        }

        /// <summary>
        /// 设置业务状态
        /// </summary>
        /// <param name="status">业务状态</param>
        /// <param name="reason">转换原因</param>
        /// <returns>设置结果</returns>
        public Task<bool> SetBusinessStatusAsync(Enum status, string reason = null)
        {
            try
            {
                if (_statusManager == null)
                {
                    _logger?.LogWarning("状态管理器未初始化，无法设置业务状态");
                    return Task.FromResult(false);
                }

                var oldStatus = CurrentStatus;
                
                // 通过状态管理器设置状态，状态管理器会负责触发事件
                _statusManager.SetBusinessStatusAsync(Entity, status.GetType(), status).Wait();
                CurrentStatus = status;
                Reason = reason ?? Reason;
                TransitionTime = DateTime.Now;

                // 注意：状态变更事件由状态管理器统一管理，不再在此处直接触发
                // 状态管理器会在设置状态时自动触发相应的事件

                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "设置业务状态失败: {EntityId}, {BusinessStatus}", Entity.PrimaryKeyID, status);
                return Task.FromResult(false);
            }
        }

        /// <summary>
        /// 设置操作状态
        /// </summary>
        /// <param name="status">操作状态</param>
        /// <param name="reason">转换原因</param>
        /// <returns>设置结果</returns>
        public Task<bool> SetActionStatusAsync(ActionStatus status, string reason = null)
        {
            try
            {
                if (_statusManager == null)
                {
                    _logger?.LogWarning("状态管理器未初始化，无法设置操作状态");
                    return Task.FromResult(false);
                }

                var oldStatus = CurrentStatus;
                
                // 通过状态管理器设置状态，状态管理器会负责触发事件
                _statusManager.SetActionStatusAsync(Entity, status).Wait();
                CurrentStatus = status;
                Reason = reason ?? Reason;
                TransitionTime = DateTime.Now;

                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "设置操作状态失败: {EntityId}, {ActionStatus}", Entity.PrimaryKeyID, status);
                return Task.FromResult(false);
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