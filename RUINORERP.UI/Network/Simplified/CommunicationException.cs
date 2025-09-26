using System;

namespace RUINORERP.UI.Network.Simplified
{
    /// <summary>
    /// 通信异常类
    /// 用于封装通信过程中发生的错误
    /// </summary>
    public class CommunicationException : Exception
    {
        /// <summary>
        /// 错误代码
        /// </summary>
        public int Code { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">错误消息</param>
        public CommunicationException(string message) : base(message)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <param name="code">错误代码</param>
        public CommunicationException(string message, int code) : base(message)
        {
            Code = code;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <param name="innerException">内部异常</param>
        public CommunicationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <param name="code">错误代码</param>
        /// <param name="innerException">内部异常</param>
        public CommunicationException(string message, int code, Exception innerException) : base(message, innerException)
        {
            Code = code;
        }
    }
}