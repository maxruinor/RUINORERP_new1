/**
 * 文件: StateTransitionEventArgs.cs
 * 版本: V4 - 适配合并版状态转换结果的事件参数
 * 说明: 适配合并版StateTransitionResult的状态转换事件参数类，提供状态变更事件处理功能
 * 创建日期: 2024年
 * 作者: RUINOR ERP开发团队
 * 更新日期: 2025-01-12 - 适配合并版StateTransitionResult
 * 
 * 版本标识：
 * V4: 适配合并版StateTransitionResult，支持检查和转换两种场景的事件参数
 * 公共代码: 状态转换事件参数处理，V3和V4架构共用
 * 通用性: 状态转换事件参数处理，支持检查和实际转换两种场景
 */

using System;
using System.Collections.Generic;
using RUINORERP.Global;

namespace RUINORERP.Model.Base.StatusManager
{
    /// <summary>
    /// 状态转换事件参数
    /// 适配合并版StateTransitionResult，支持检查和转换两种场景的事件参数
    /// </summary>
    public class StateTransitionEventArgs : EventArgs
    {
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
        /// 旧状态
        /// </summary>
        public object OldStatus { get; }

        /// <summary>
        /// 新状态
        /// </summary>
        public object NewStatus { get; }

        /// <summary>
        /// 变更原因
        /// </summary>
        public string Reason { get; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; }

        /// <summary>
        /// 变更时间
        /// </summary>
        public DateTime ChangeTime { get; }

        /// <summary>
        /// 附加数据
        /// </summary>
        public Dictionary<string, object> AdditionalData { get; }

        /// <summary>
        /// 状态转换结果（V4新增）
        /// </summary>
        public StateTransitionResult Result { get; }

        /// <summary>
        /// 是否为检查事件（V4新增）
        /// </summary>
        public bool IsCheckEvent => Result?.ResultType == StateTransitionResultType.Check;

        /// <summary>
        /// 是否为转换事件（V4新增）
        /// </summary>
        public bool IsTransitionEvent => Result?.ResultType == StateTransitionResultType.Transition;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数（兼容V3）
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">状态类型</param>
        /// <param name="oldStatus">旧状态</param>
        /// <param name="newStatus">新状态</param>
        /// <param name="reason">变更原因</param>
        /// <param name="userId">用户ID</param>
        /// <param name="changeTime">变更时间</param>
        /// <param name="additionalData">附加数据</param>
        public StateTransitionEventArgs(
            BaseEntity entity,
            Type statusType,
            object oldStatus,
            object newStatus,
            string reason = null,
            string userId = null,
            DateTime? changeTime = null,
            Dictionary<string, object> additionalData = null)
        {
            Entity = entity ?? throw new ArgumentNullException(nameof(entity));
            StatusType = statusType ?? throw new ArgumentNullException(nameof(statusType));
            OldStatus = oldStatus;
            NewStatus = newStatus;
            Reason = reason;
            UserId = userId;
            ChangeTime = changeTime ?? DateTime.Now;
            AdditionalData = additionalData ?? new Dictionary<string, object>();
            
            // 创建默认的转换结果
            Result = new StateTransitionResult(StateTransitionResultType.Transition, true)
            {
                OldStatus = oldStatus,
                NewStatus = newStatus,
                StatusType = statusType,
                TransitionReason = reason,
                UserId = userId,
                TransitionTime = ChangeTime
            };
        }

        /// <summary>
        /// 构造函数（V4新增）
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="result">状态转换结果</param>
        /// <param name="reason">变更原因</param>
        /// <param name="userId">用户ID</param>
        /// <param name="changeTime">变更时间</param>
        /// <param name="additionalData">附加数据</param>
        public StateTransitionEventArgs(
            BaseEntity entity,
            StateTransitionResult result,
            string reason = null,
            string userId = null,
            DateTime? changeTime = null,
            Dictionary<string, object> additionalData = null)
        {
            Entity = entity ?? throw new ArgumentNullException(nameof(entity));
            Result = result ?? throw new ArgumentNullException(nameof(result));
            StatusType = result.StatusType;
            OldStatus = result.OldStatus;
            NewStatus = result.NewStatus;
            Reason = reason ?? result.ErrorMessage;
            UserId = userId ?? result.UserId;
            ChangeTime = changeTime ?? result.TransitionTime;
            AdditionalData = additionalData ?? new Dictionary<string, object>();
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 检查状态是否实际发生了变更
        /// </summary>
        /// <returns>如果旧状态和新状态不同，则返回true</returns>
        public bool HasChanged()
        {
            return !Equals(OldStatus, NewStatus);
        }

        /// <summary>
        /// 获取状态变更的描述信息
        /// </summary>
        /// <returns>状态变更描述</returns>
        public string GetChangeDescription()
        {
            var statusTypeName = StatusType?.Name ?? "Unknown";
            var oldStatusName = OldStatus?.ToString() ?? "null";
            var newStatusName = NewStatus?.ToString() ?? "null";

            return $"状态变更: {statusTypeName} 从 {oldStatusName} 变更为 {newStatusName}";
        }

        /// <summary>
        /// 获取事件描述（V4新增）
        /// </summary>
        /// <returns>事件描述</returns>
        public string GetEventDescription()
        {
            var eventType = IsCheckEvent ? "状态转换检查" : "状态转换";
            var status = Result?.IsSuccess == true ? "成功" : "失败";
            var entityInfo = Entity?.GetType().Name ?? "Unknown";
            
            var description = $"{eventType}{status} - 实体: {entityInfo}";
            
            if (!string.IsNullOrEmpty(Reason))
            {
                description += $", 原因: {Reason}";
            }
            
            if (!string.IsNullOrEmpty(Result?.ErrorMessage))
            {
                description += $", 错误: {Result.ErrorMessage}";
            }
            
            return description;
        }

        /// <summary>
        /// 创建检查事件参数（V4新增）
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">状态类型</param>
        /// <param name="oldStatus">旧状态</param>
        /// <param name="newStatus">新状态</param>
        /// <param name="isAllowed">是否允许转换</param>
        /// <param name="errorMessage">错误消息</param>
        /// <param name="additionalData">附加数据</param>
        /// <returns>检查事件参数</returns>
        public static StateTransitionEventArgs CreateCheckEvent(
            BaseEntity entity,
            Type statusType,
            object oldStatus,
            object newStatus,
            bool isAllowed,
            string errorMessage = null,
            Dictionary<string, object> additionalData = null)
        {
            var result = isAllowed 
                ? StateTransitionResult.Allowed()
                : StateTransitionResult.Denied(errorMessage);
                
            result.StatusType = statusType;
            result.OldStatus = oldStatus;
            result.NewStatus = newStatus;
            
            return new StateTransitionEventArgs(entity, result, additionalData: additionalData);
        }

        /// <summary>
        /// 创建转换事件参数（V4新增）
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">状态类型</param>
        /// <param name="oldStatus">旧状态</param>
        /// <param name="newStatus">新状态</param>
        /// <param name="isSuccess">是否转换成功</param>
        /// <param name="errorMessage">错误消息</param>
        /// <param name="exception">异常</param>
        /// <param name="userId">用户ID</param>
        /// <param name="additionalData">附加数据</param>
        /// <returns>转换事件参数</returns>
        public static StateTransitionEventArgs CreateTransitionEvent(
            BaseEntity entity,
            Type statusType,
            object oldStatus,
            object newStatus,
            bool isSuccess,
            string errorMessage = null,
            Exception exception = null,
            string userId = null,
            Dictionary<string, object> additionalData = null)
        {
            var result = isSuccess 
                ? StateTransitionResult.Success(oldStatus, newStatus, statusType, errorMessage)
                : StateTransitionResult.Failure(oldStatus, newStatus, statusType, errorMessage, exception);
                
            result.UserId = userId;
            
            return new StateTransitionEventArgs(entity, result, userId: userId, additionalData: additionalData);
        }

        #endregion
    }
}