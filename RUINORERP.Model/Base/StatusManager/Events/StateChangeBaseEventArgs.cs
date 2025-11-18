using System;
using System.Collections.Generic;

namespace RUINORERP.Model.Base.StatusManager.Events
{
    /// <summary>
    /// 状态变更事件参数基类
    /// 为所有状态变更相关的事件参数提供通用基础属性和方法
    /// </summary>
    public abstract class StateChangeBaseEventArgs : EventArgs
    {
        /// <summary>
        /// 实体对象
        /// </summary>
        public object Entity { get; protected set; }

        /// <summary>
        /// 状态类型
        /// </summary>
        public Type StatusType { get; protected set; }

        /// <summary>
        /// 旧状态
        /// </summary>
        public object OldStatus { get; protected set; }

        /// <summary>
        /// 新状态
        /// </summary>
        public object NewStatus { get; protected set; }

        /// <summary>
        /// 变更原因
        /// </summary>
        public string Reason { get; protected set; }

        /// <summary>
        /// 变更时间
        /// </summary>
        public DateTime ChangeTime { get; protected set; }

        /// <summary>
        /// 执行状态变更的用户ID
        /// </summary>
        public string UserId { get; protected set; }

        /// <summary>
        /// 附加数据
        /// </summary>
        public Dictionary<string, object> AdditionalData { get; protected set; }

        /// <summary>
        /// 初始化状态变更事件参数基类
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">状态类型</param>
        /// <param name="oldStatus">旧状态</param>
        /// <param name="newStatus">新状态</param>
        /// <param name="reason">变更原因</param>
        /// <param name="userId">用户ID</param>
        /// <param name="additionalData">附加数据</param>
        protected StateChangeBaseEventArgs(object entity, Type statusType, object oldStatus, object newStatus, string reason = null, string userId = null, Dictionary<string, object> additionalData = null)
        {
            Entity = entity ?? throw new ArgumentNullException(nameof(entity));
            StatusType = statusType ?? throw new ArgumentNullException(nameof(statusType));
            OldStatus = oldStatus;
            NewStatus = newStatus;
            Reason = reason;
            UserId = userId;
            AdditionalData = additionalData ?? new Dictionary<string, object>();
            ChangeTime = DateTime.Now;
        }

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
        /// 获取附加数据值
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">键名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>数据值</returns>
        public T GetAdditionalData<T>(string key, T defaultValue = default(T))
        {
            if (AdditionalData != null && AdditionalData.TryGetValue(key, out var value) && value is T)
            {
                return (T)value;
            }
            
            return defaultValue;
        }

        /// <summary>
        /// 设置附加数据
        /// </summary>
        /// <param name="key">键名</param>
        /// <param name="value">值</param>
        public void SetAdditionalData(string key, object value)
        {
            if (AdditionalData != null)
            {
                AdditionalData[key] = value;
            }
        }
    }
}