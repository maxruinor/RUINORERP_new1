/**
 * 文件: StateTransitionRecord.cs
 * 说明: 状态转换记录类
 * 创建日期: 2024年
 * 作者: RUINOR ERP开发团队
 */

using System;

namespace RUINORERP.Model.Base.StatusManager.Core
{
    /// <summary>
    /// 状态转换记录类
    /// </summary>
    public class StateTransitionRecord
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="entityId">实体ID</param>
        /// <param name="statusType">状态类型</param>
        /// <param name="oldStatus">旧状态</param>
        /// <param name="newStatus">新状态</param>
        /// <param name="reason">变更原因</param>
        public StateTransitionRecord(
            Type entityType, 
            object entityId, 
            Type statusType, 
            object oldStatus, 
            object newStatus, 
            string reason = null)
        {
            EntityType = entityType;
            EntityId = entityId;
            StatusType = statusType;
            OldStatus = oldStatus;
            NewStatus = newStatus;
            Reason = reason;
            TransitionTime = DateTime.Now;
        }

        /// <summary>
        /// 实体类型
        /// </summary>
        public Type EntityType { get; }

        /// <summary>
        /// 实体ID
        /// </summary>
        public object EntityId { get; }

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