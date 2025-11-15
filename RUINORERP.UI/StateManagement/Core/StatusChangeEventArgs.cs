/**
 * 文件: StatusChangeEventArgs.cs
 * 说明: 状态变更事件参数类 - v3版本
 * 创建日期: 2024年
 * 作者: RUINOR ERP开发团队
 */

using System;
using RUINORERP.Model;
using RUINORERP.Model.Base;

namespace RUINORERP.UI.StateManagement.Core
{
    /// <summary>
    /// 状态变更事件参数类 - v3版本
    /// 用于传递状态变更事件的相关信息
    /// </summary>
    public class StatusChangeEventArgs : EventArgs
    {
        #region 属性

        /// <summary>
        /// 发生状态变更的实体对象
        /// </summary>
        public BaseEntity Entity { get; }

        /// <summary>
        /// 状态类型
        /// </summary>
        public Type StatusType { get; }

        /// <summary>
        /// 变更前的状态
        /// </summary>
        public object OldStatus { get; }

        /// <summary>
        /// 变更后的状态
        /// </summary>
        public object NewStatus { get; }

        /// <summary>
        /// 状态变更的原因
        /// </summary>
        public string Reason { get; }

        /// <summary>
        /// 状态变更的时间
        /// </summary>
        public DateTime ChangeTime { get; }

        /// <summary>
        /// 执行状态变更的用户ID
        /// </summary>
        public string UserId { get; }

        /// <summary>
        /// 附加数据
        /// </summary>
        public System.Collections.Generic.Dictionary<string, object> AdditionalData { get; }

        #endregion

        #region 构造函数

        /// <summary>
        /// 初始化状态变更事件参数
        /// </summary>
        /// <param name="entity">发生状态变更的实体对象</param>
        /// <param name="statusType">状态类型</param>
        /// <param name="oldStatus">变更前的状态</param>
        /// <param name="newStatus">变更后的状态</param>
        /// <param name="reason">状态变更的原因</param>
        /// <param name="userId">执行状态变更的用户ID</param>
        /// <param name="additionalData">附加数据</param>
        public StatusChangeEventArgs(
            BaseEntity entity,
            Type statusType,
            object oldStatus,
            object newStatus,
            string reason = null,
            string userId = null,
            System.Collections.Generic.Dictionary<string, object> additionalData = null)
        {
            Entity = entity ?? throw new ArgumentNullException(nameof(entity));
            StatusType = statusType ?? throw new ArgumentNullException(nameof(statusType));
            OldStatus = oldStatus;
            NewStatus = newStatus;
            Reason = reason;
            UserId = userId;
            AdditionalData = additionalData ?? new System.Collections.Generic.Dictionary<string, object>();
            ChangeTime = DateTime.Now;
        }

        #endregion

        #region 公共方法

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

        #endregion
    }
}