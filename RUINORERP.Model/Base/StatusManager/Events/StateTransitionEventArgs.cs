/**
 * 文件: StateTransitionEventArgs.cs
 * 说明: 状态转换事件参数
 * 创建日期: 2024年
 * 作者: RUINOR ERP开发团队
 */

using System;

namespace RUINORERP.Model.Base.StatusManager.Core
{
    /// <summary>
    /// 状态转换事件参数
    /// </summary>
    public class StateTransitionEventArgs : EventArgs
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">状态类型</param>
        /// <param name="oldStatus">旧状态</param>
        /// <param name="newStatus">新状态</param>
        /// <param name="reason">变更原因</param>
        public StateTransitionEventArgs(
            BaseEntity entity, 
            Type statusType, 
            object oldStatus, 
            object newStatus, 
            string reason = null)
        {
            Entity = entity;
            StatusType = statusType;
            OldStatus = oldStatus;
            NewStatus = newStatus;
            Reason = reason;
            TransitionTime = DateTime.Now;
        }

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
        /// 转换时间
        /// </summary>
        public DateTime TransitionTime { get; }
    }
}