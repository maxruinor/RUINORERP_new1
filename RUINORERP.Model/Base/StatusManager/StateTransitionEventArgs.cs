/**
 * 文件: StateTransitionEventArgs.cs
 * 版本: 简化版 - 状态转换事件参数
 * 说明: 简化版状态转换事件参数类，提供状态变更事件处理功能
 * 创建日期: 2024年
 * 作者: RUINOR ERP开发团队
 */

using System;
using System.Collections.Generic;
using RUINORERP.Global;

namespace RUINORERP.Model.Base.StatusManager
{
    /// <summary>
    /// 简化版状态转换事件参数
    /// 提供状态转换事件的基本信息
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

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
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

        #endregion
    }
}