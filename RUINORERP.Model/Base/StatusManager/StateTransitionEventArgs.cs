/**
 * 文件: StateTransitionEventArgs.cs
 * 版本: V4 - 状态转换事件参数类
 * 说明: 状态转换事件参数类 - 支持状态转换事件的参数传递
 * 创建日期: 2024年
 * 作者: RUINOR ERP开发团队
 * 更新日期: 2025-01-12
 */

using System;
using System.Collections.Generic;
using RUINORERP.Model.Base;

namespace RUINORERP.Model.Base.StatusManager
{
    /// <summary>
    /// 状态转换事件参数类
    /// 版本: V4
    /// </summary>
    public class StateTransitionEventArgs : EventArgs
    {
        #region 属性

        /// <summary>
        /// 实体对象
        /// </summary>
        public BaseEntity Entity { get; set; }

        /// <summary>
        /// 状态类型
        /// </summary>
        public Type StatusType { get; set; }

        /// <summary>
        /// 旧状态
        /// </summary>
        public object OldStatus { get; set; }

        /// <summary>
        /// 新状态
        /// </summary>
        public object NewStatus { get; set; }

        /// <summary>
        /// 变更原因
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 变更时间
        /// </summary>
        public DateTime ChangeTime { get; set; }

        /// <summary>
        /// 附加数据
        /// </summary>
        public Dictionary<string, object> AdditionalData { get; set; }

        /// <summary>
        /// 状态转换结果
        /// </summary>
        public StateTransitionResult Result { get; set; }

        /// <summary>
        /// 是否为检查事件
        /// </summary>
        public bool IsCheckEvent { get; set; }

        /// <summary>
        /// 是否为转换事件
        /// </summary>
        public bool IsTransitionEvent { get; set; }

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数 - V3版本兼容
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">状态类型</param>
        /// <param name="oldStatus">旧状态</param>
        /// <param name="newStatus">新状态</param>
        /// <param name="reason">变更原因</param>
        /// <param name="userId">用户ID</param>
        public StateTransitionEventArgs(BaseEntity entity, Type statusType, object oldStatus, object newStatus, string reason = null, string userId = null)
        {
            this.Entity = entity;
            this.StatusType = statusType;
            this.OldStatus = oldStatus;
            this.NewStatus = newStatus;
            this.Reason = reason;
            this.UserId = userId;
            this.ChangeTime = DateTime.Now;
            this.AdditionalData = new Dictionary<string, object>();
            this.IsCheckEvent = false;
            this.IsTransitionEvent = true;
        }

        /// <summary>
        /// 构造函数 - V4版本
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="result">状态转换结果</param>
        /// <param name="reason">变更原因</param>
        /// <param name="additionalData">附加数据</param>
        public StateTransitionEventArgs(BaseEntity entity, StateTransitionResult result, string reason = null, Dictionary<string, object> additionalData = null)
        {
            this.Entity = entity;
            this.Result = result;
            this.StatusType = result?.StatusType;
            this.OldStatus = result?.OldStatus;
            this.NewStatus = result?.NewStatus;
            this.Reason = reason;
            this.AdditionalData = additionalData ?? new Dictionary<string, object>();
            this.ChangeTime = DateTime.Now;
            this.IsCheckEvent = false;
            this.IsTransitionEvent = true;
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 判断状态是否发生变化
        /// </summary>
        /// <returns>是否发生变化</returns>
        public bool HasChanged()
        {
            return !Equals(OldStatus, NewStatus);
        }

        /// <summary>
        /// 获取状态变更描述
        /// </summary>
        /// <returns>变更描述</returns>
        public string GetChangeDescription()
        {
            string statusTypeName = StatusType?.Name ?? "未知状态类型";
            string oldStatusName = OldStatus?.ToString() ?? "未设置";
            string newStatusName = NewStatus?.ToString() ?? "未设置";
            string changeType = HasChanged() ? "已变更" : "未变更";
            string entityName = Entity?.ToString() ?? "未知实体";

            return $"{entityName} - {statusTypeName}状态{changeType}: 从 '{oldStatusName}' 变更为 '{newStatusName}'";
        }

        /// <summary>
        /// 获取事件描述
        /// </summary>
        /// <returns>事件描述</returns>
        public string GetEventDescription()
        {
            string eventType = IsCheckEvent ? "状态检查" : IsTransitionEvent ? "状态转换" : "未知事件";
            return $"{eventType}事件: {GetChangeDescription()}";
        }

        /// <summary>
        /// 创建状态检查事件
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">状态类型</param>
        /// <param name="oldStatus">旧状态</param>
        /// <param name="newStatus">新状态</param>
        /// <param name="reason">变更原因</param>
        /// <returns>状态检查事件参数</returns>
        public static StateTransitionEventArgs CreateCheckEvent(BaseEntity entity, Type statusType, object oldStatus, object newStatus, string reason = null)
        {
            var eventArgs = new StateTransitionEventArgs(entity, statusType, oldStatus, newStatus, reason);
            eventArgs.IsCheckEvent = true;
            eventArgs.IsTransitionEvent = false;
            return eventArgs;
        }

        /// <summary>
        /// 创建状态转换事件
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">状态类型</param>
        /// <param name="oldStatus">旧状态</param>
        /// <param name="newStatus">新状态</param>
        /// <param name="reason">变更原因</param>
        /// <returns>状态转换事件参数</returns>
        public static StateTransitionEventArgs CreateTransitionEvent(BaseEntity entity, Type statusType, object oldStatus, object newStatus, string reason = null)
        {
            var eventArgs = new StateTransitionEventArgs(entity, statusType, oldStatus, newStatus, reason);
            eventArgs.IsCheckEvent = false;
            eventArgs.IsTransitionEvent = true;
            return eventArgs;
        }

        #endregion
    }
}