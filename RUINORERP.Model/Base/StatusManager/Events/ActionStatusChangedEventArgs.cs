/**
 * 文件: ActionStatusChangedEventArgs.cs
 * 说明: 操作状态变更事件参数
 * 创建日期: 2025年
 * 作者: RUINOR ERP开发团队
 */

using System;

namespace RUINORERP.Model.Base.StatusManager.Core
{
    /// <summary>
    /// 操作状态变更事件参数
    /// </summary>
    public class ActionStatusChangedEventArgs : EventArgs
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="oldValue">旧的操作状态</param>
        /// <param name="newValue">新的操作状态</param>
        /// <param name="reason">变更原因</param>
        public ActionStatusChangedEventArgs(
            ActionStatus oldValue, 
            ActionStatus newValue, 
            string reason = null)
        {
            OldValue = oldValue;
            NewValue = newValue;
            Reason = reason;
            ChangeTime = DateTime.Now;
        }

        /// <summary>
        /// 旧的操作状态
        /// </summary>
        public ActionStatus OldValue { get; }

        /// <summary>
        /// 新的操作状态
        /// </summary>
        public ActionStatus NewValue { get; }

        /// <summary>
        /// 变更原因
        /// </summary>
        public string Reason { get; }

        /// <summary>
        /// 变更时间
        /// </summary>
        public DateTime ChangeTime { get; }
    }
}