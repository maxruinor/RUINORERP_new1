/**
 * 文件: StateTransitionResult.cs
 * 说明: 状态转换结果，合并了BaseEntity内部类和StatusManager.Core类的功能
 * 创建日期: 2024年
 * 作者: RUINOR ERP开发团队
 */

using RUINORERP.Global;
using System;

namespace RUINORERP.Model.Base.StatusManager
{
    /// <summary>
    /// 状态转换结果
    /// </summary>
    public class StateTransitionResult
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public StateTransitionResult()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="isValid">转换是否有效</param>
        /// <param name="message">消息</param>
        public StateTransitionResult(bool isValid = false, string message = null)
        {
            IsValid = isValid;
            Message = message;
        }

        /// <summary>
        /// 转换是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 旧状态
        /// </summary>
        public DataStatus? OldStatus { get; set; }

        /// <summary>
        /// 新状态
        /// </summary>
        public DataStatus? NewStatus { get; set; }

        /// <summary>
        /// 转换时间
        /// </summary>
        public DateTime TransitionTime { get; set; }

        /// <summary>
        /// 转换原因
        /// </summary>
        public string TransitionReason { get; set; }

        /// <summary>
        /// 转换是否有效（兼容StatusManager.Core版本）
        /// </summary>
        public bool IsValid
        {
            get => IsSuccess;
            set => IsSuccess = value;
        }

        /// <summary>
        /// 消息（兼容StatusManager.Core版本）
        /// </summary>
        public string Message
        {
            get => ErrorMessage;
            set => ErrorMessage = value;
        }

        /// <summary>
        /// 异常信息（来自StatusManager.Core版本）
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// 创建成功结果
        /// </summary>
        /// <param name="message">消息</param>
        /// <returns>状态转换结果</returns>
        public static StateTransitionResult Success(string message = null)
        {
            return new StateTransitionResult(true, message);
        }

        /// <summary>
        /// 创建失败结果
        /// </summary>
        /// <param name="message">消息</param>
        /// <param name="exception">异常</param>
        /// <returns>状态转换结果</returns>
        public static StateTransitionResult Failure(string message, Exception exception = null)
        {
            return new StateTransitionResult(false, message)
            {
                Exception = exception
            };
        }
    }
}