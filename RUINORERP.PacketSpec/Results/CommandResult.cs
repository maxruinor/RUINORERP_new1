using RUINORERP.PacketSpec.Core;
using System;

namespace RUINORERP.PacketSpec.Results
{
    /// <summary>
    /// 命令执行结果
    /// </summary>
    public class CommandResult : BaseCommandResult
    {
        /// <summary>
        /// 命令ID
        /// </summary>
        public string CommandId { get; set; }

        /// <summary>
        /// 命令代码
        /// </summary>
        public uint CommandCode { get; set; }

        /// <summary>
        /// 结果数据
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 创建成功结果
        /// </summary>
        /// <param name="message">成功消息</param>
        /// <param name="data">结果数据</param>
        /// <returns>命令结果对象</returns>
        public static CommandResult Success(string message = null, object data = null)
        {
            return new CommandResult
            {
                IsSuccess = true,
                Message = message,
                Data = data,
                ErrorCode = null
            };
        }

        /// <summary>
        /// 创建失败结果
        /// </summary>
        /// <param name="message">失败消息</param>
        /// <param name="errorCode">错误代码</param>
        /// <param name="exception">异常对象</param>
        /// <returns>命令结果对象</returns>
        public static CommandResult Failure(string message, string errorCode = null, Exception exception = null)
        {
            return new CommandResult
            {
                IsSuccess = false,
                Message = message,
                ErrorCode = errorCode,
                Data = exception?.ToString()
            };
        }

        /// <summary>
        /// 转换为字符串表示
        /// </summary>
        /// <returns>结果信息字符串</returns>
        public override string ToString()
        {
            if (IsSuccess)
            {
                return $"CommandResult[Success: {Message}]";
            }
            else
            {
                return $"CommandResult[Failure: {Message}, ErrorCode: {ErrorCode}]";
            }
        }
    }

    /// <summary>
    /// 错误代码常量
    /// </summary>
    public static class ErrorCodes
    {
        public const string NullCommand = "NULL_COMMAND";
        public const string DispatcherNotInitialized = "DISPATCHER_NOT_INITIALIZED";
        public const string DispatcherBusy = "DISPATCHER_BUSY";
        public const string NoHandlerFound = "NO_HANDLER_FOUND";
        public const string HandlerSelectionFailed = "HANDLER_SELECTION_FAILED";
        public const string NullResult = "NULL_RESULT";
        public const string DispatchCancelled = "DISPATCH_CANCELLED";
        public const string DispatchError = "DISPATCH_ERROR";
        public const string InvalidCommand = "INVALID_COMMAND";
        public const string CommandTimeout = "COMMAND_TIMEOUT";
        public const string AuthorizationFailed = "AUTHORIZATION_FAILED";
        public const string ValidationFailed = "VALIDATION_FAILED";
    }
}
