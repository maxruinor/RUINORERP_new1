/**
 * 文件: StateTransitionEventArgs.cs
 * 说明: 状态转换事件参数
 * 创建日期: 2024年
 * 作者: RUINOR ERP开发团队
 */

using RUINORERP.Model.Base.StatusManager.Events;
using System;
using System.Collections.Generic;

namespace RUINORERP.Model.Base.StatusManager.Core
{
    /// <summary>
    /// 状态转换事件参数
    /// </summary>
    public class StateTransitionEventArgs : StateChangeBaseEventArgs
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="statusType">状态类型</param>
        /// <param name="oldStatus">旧状态</param>
        /// <param name="newStatus">新状态</param>
        /// <param name="reason">变更原因</param>
        /// <param name="userId">用户ID</param>
        /// <param name="transitionTime">转换时间</param>
        public StateTransitionEventArgs(
            BaseEntity entity, 
            Type statusType, 
            object oldStatus, 
            object newStatus, 
            string reason = null,
            string userId = null,
            DateTime? transitionTime = null)
            : base(entity, statusType, oldStatus, newStatus, reason)
        {
            UserId = userId;
            TransitionTime = transitionTime ?? DateTime.Now;
            AdditionalData = new Dictionary<string, object>();
        }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; }

        /// <summary>
        /// 转换时间
        /// </summary>
        public DateTime TransitionTime { get; }

        /// <summary>
        /// 附加数据
        /// </summary>
        public Dictionary<string, object> AdditionalData { get; }

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
    }
}