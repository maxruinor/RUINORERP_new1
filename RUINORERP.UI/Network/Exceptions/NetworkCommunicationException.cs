using System;
using RUINORERP.PacketSpec.Commands;

namespace RUINORERP.UI.Network.Exceptions
{
    /// <summary>
    /// 网络通信异常类，用于包装和传递网络通信过程中的异常信息
    /// 特别支持Token过期、认证失败等场景的识别和处理
    /// </summary>
    public class NetworkCommunicationException : Exception
    {
        /// <summary>
        /// 命令ID
        /// </summary>
        public CommandId CommandId { get; }

        /// <summary>
        /// 请求ID (字符串类型)
        /// </summary>
        public string RequestId { get; }

        /// <summary>
        /// 请求ID (长整型)
        /// </summary>
        public long? RequestIdLong { get; }

        /// <summary>
        /// 是否是认证相关的错误
        /// </summary>
        public bool IsAuthenticationError => 
            Message.IndexOf("token expired", StringComparison.OrdinalIgnoreCase) >= 0 ||
            Message.IndexOf("unauthorized", StringComparison.OrdinalIgnoreCase) >= 0 ||
            Message.IndexOf("认证失败", StringComparison.OrdinalIgnoreCase) >= 0 ||
            Message.IndexOf("未授权", StringComparison.OrdinalIgnoreCase) >= 0 ||
            Message.IndexOf("权限不足", StringComparison.OrdinalIgnoreCase) >= 0;

        /// <summary>
        /// 是否是连接相关的错误
        /// </summary>
        public bool IsConnectionError => 
            Message.IndexOf("连接已断开", StringComparison.OrdinalIgnoreCase) >= 0 ||
            Message.IndexOf("连接超时", StringComparison.OrdinalIgnoreCase) >= 0 ||
            Message.IndexOf("无法连接", StringComparison.OrdinalIgnoreCase) >= 0;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">异常消息</param>
        /// <param name="innerException">内部异常</param>
        /// <param name="commandId">命令ID</param>
        /// <param name="requestId">请求ID (字符串类型)</param>
        public NetworkCommunicationException(
            string message,
            Exception innerException,
            CommandId commandId,
            string requestId)
            : base(message, innerException)
        {
            CommandId = commandId;
            RequestId = requestId;
            RequestIdLong = null;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">异常消息</param>
        /// <param name="innerException">内部异常</param>
        /// <param name="commandId">命令ID</param>
        /// <param name="requestId">请求ID (长整型)</param>
        public NetworkCommunicationException(
            string message,
            Exception innerException,
            CommandId commandId,
            long requestId)
            : base(message, innerException)
        {
            CommandId = commandId;
            RequestId = requestId.ToString();
            RequestIdLong = requestId;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">异常消息</param>
        /// <param name="commandId">命令ID</param>
        public NetworkCommunicationException(string message, CommandId commandId)
            : base(message)
        {
            CommandId = commandId;
            RequestId = null;
            RequestIdLong = null;
        }

        /// <summary>
        /// 获取描述当前异常的字符串
        /// </summary>
        /// <returns>包含异常信息的字符串</returns>
        public override string ToString()
        {
            var baseString = base.ToString();
            string requestIdInfo = RequestIdLong.HasValue ? 
                $"RequestIdLong={RequestIdLong}" : 
                $"RequestId={RequestId}";
            
            return $"NetworkCommunicationException: CommandId={CommandId.FullCode}, {requestIdInfo}\n{baseString}";
        }
    }
}