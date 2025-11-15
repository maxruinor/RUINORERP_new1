/**
 * 文件: StateTransitionResult.cs
 * 说明: 状态转换结果
 * 创建日期: 2024年
 * 作者: RUINOR ERP开发团队
 */

using System;

namespace RUINORERP.Model.Base.StatusManager.Core
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
        public StateTransitionResult(bool isValid, string message = null)
        {
            IsValid = isValid;
            Message = message;
        }

        /// <summary>
        /// 转换是否有效
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 异常信息
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