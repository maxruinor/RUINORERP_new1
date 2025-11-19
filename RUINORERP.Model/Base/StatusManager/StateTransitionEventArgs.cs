/**
 * 文件: StateTransitionEventArgs.cs
 * 版本: 公共 - 状态转换事件参数（V3/V4共用）
 * 说明: 状态转换事件参数类，提供状态变更事件处理功能，V3和V4架构共用
 * 创建日期: 2024年
 * 作者: RUINOR ERP开发团队
 * 
 * 版本标识：
 * 公共代码: V3和V4架构共用的事件参数类
 * 通用性: 状态转换事件处理，两个版本保持一致
 */

using System;
using System.Collections.Generic;
using RUINORERP.Global;

namespace RUINORERP.Model.Base.StatusManager
{
    /// <summary>
    /// 状态转换事件参数
    /// 合并了StateChangeBaseEventArgs的功能，提供完整的状态转换事件信息
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
        /// 变更时间
        /// </summary>
        public DateTime ChangeTime { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 转换是否成功
        /// </summary>
        public bool IsSuccess { get; set; } = true;

        /// <summary>
        /// 转换错误信息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 转换异常
        /// </summary>
        public Exception Exception { get; set; }

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

        /// <summary>
        /// 获取附加数据值
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="key">键名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>数据值</returns>
        public T GetAdditionalData<T>(string key, T defaultValue = default)
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

        /// <summary>
        /// 创建失败的事件参数
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">状态类型</param>
        /// <param name="oldStatus">旧状态</param>
        /// <param name="newStatus">新状态</param>
        /// <param name="errorMessage">错误信息</param>
        /// <param name="exception">异常</param>
        /// <param name="reason">转换原因</param>
        /// <param name="userId">用户ID</param>
        /// <returns>失败的事件参数</returns>
        public static StateTransitionEventArgs CreateFailure(
            BaseEntity entity,
            Type statusType,
            object oldStatus,
            object newStatus,
            string errorMessage,
            Exception exception = null,
            string reason = null,
            string userId = null)
        {
            return new StateTransitionEventArgs(entity, statusType, oldStatus, newStatus, reason, userId)
            {
                IsSuccess = false,
                ErrorMessage = errorMessage,
                Exception = exception
            };
        }

        #endregion
    }
}