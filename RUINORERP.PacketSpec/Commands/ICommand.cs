using System;
using System.Threading;
using System.Threading.Tasks;
using RUINORERP.PacketSpec.Protocol;
using RUINORERP.PacketSpec.Core;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 命令操作类型
    /// </summary>
    public enum CommandDirection
    {
        /// <summary>
        /// 接收命令（从客户端到服务器）
        /// </summary>
        Receive = 0,

        /// <summary>
        /// 发送命令（从服务器到客户端）
        /// </summary>
        Send = 1,

        /// <summary>
        /// 双向命令（可发送可接收）
        /// </summary>
        Bidirectional = 2
    }

    /// <summary>
    /// 命令优先级
    /// </summary>
    public enum CommandPriority
    {
        /// <summary>
        /// 低优先级
        /// </summary>
        Low = 0,
        
        /// <summary>
        /// 普通优先级
        /// </summary>
        Normal = 1,
        
        /// <summary>
        /// 高优先级
        /// </summary>
        High = 2,
        
        /// <summary>
        /// 紧急优先级
        /// </summary>
        Critical = 3
    }

    /// <summary>
    /// 命令状态
    /// </summary>
    public enum CommandStatus
    {
        /// <summary>
        /// 已创建
        /// </summary>
        Created = 0,
        
        /// <summary>
        /// 等待处理
        /// </summary>
        Pending = 1,
        
        /// <summary>
        /// 正在处理
        /// </summary>
        Processing = 2,
        
        /// <summary>
        /// 处理完成
        /// </summary>
        Completed = 3,
        
        /// <summary>
        /// 处理失败
        /// </summary>
        Failed = 4,
        
        /// <summary>
        /// 已取消
        /// </summary>
        Cancelled = 5
    }

    /// <summary>
    /// 统一命令接口 - 定义所有命令的基本契约
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// 每个指令都存在于一个对应的socket（连接）会话
        /// </summary>
        string SessionID { get; set; }

        /// <summary>
        /// 命令唯一标识（字符串形式）
        /// </summary>
        string CommandId { get; }

        /// <summary>
        /// 命令标识符（类型安全命令系统）
        /// </summary>
        CommandId CommandIdentifier { get; }

        /// <summary>
        /// 命令方向
        /// </summary>
        CommandDirection Direction { get; set; }

        /// <summary>
        /// 命令优先级
        /// </summary>
        CommandPriority Priority { get; set; }

        /// <summary>
        /// 命令状态
        /// </summary>
        CommandStatus Status { get; set; }

        /// <summary>
        /// 原始数据包
        /// </summary>
        OriginalData OriginalData { get; set; }

        /// <summary>
        /// 命令创建时间
        /// </summary>
        DateTime CreatedAt { get; }

        /// <summary>
        /// 命令超时时间（毫秒）
        /// </summary>
        int TimeoutMs { get; set; }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>执行结果</returns>
        Task<CommandResult> ExecuteAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 验证命令是否可以执行
        /// </summary>
        /// <returns>验证结果</returns>
        CommandValidationResult Validate();

        /// <summary>
        /// 序列化命令数据
        /// </summary>
        /// <returns>序列化后的数据</returns>
        byte[] Serialize();

        /// <summary>
        /// 反序列化命令数据
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns>反序列化是否成功</returns>
        bool Deserialize(byte[] data);
    }

    /// <summary>
    /// 命令验证结果
    /// </summary>
    public class CommandValidationResult
    {
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 错误代码
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// 创建成功结果
        /// </summary>
        public static CommandValidationResult Success()
        {
            return new CommandValidationResult { IsValid = true };
        }

        /// <summary>
        /// 创建失败结果
        /// </summary>
        public static CommandValidationResult Failure(string message, string code = null)
        {
            return new CommandValidationResult
            {
                IsValid = false,
                ErrorMessage = message,
                ErrorCode = code
            };
        }
    }

    /// <summary>
    /// 命令执行结果
    /// </summary>
    public class CommandResult : ITraceable, IValidatable
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }
        
        /// <summary>
        /// 结果消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 错误代码
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// 返回数据
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 执行时间（毫秒）
        /// </summary>
        public long ExecutionTimeMs { get; set; }

        /// <summary>
        /// 异常信息
        /// </summary>
        public Exception Exception { get; set; }

        #region ITraceable 接口实现
        /// <summary>
        /// 创建时间（UTC时间）
        /// </summary>
        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 最后更新时间（UTC时间）
        /// </summary>
        public DateTime? LastUpdatedTime { get; set; }

        /// <summary>
        /// 时间戳（UTC时间）
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// 模型版本
        /// </summary>
        public string Version { get; set; } = "2.0";

        /// <summary>
        /// 更新时间戳
        /// </summary>
        public void UpdateTimestamp()
        {
            Timestamp = DateTime.UtcNow;
            LastUpdatedTime = Timestamp;
        }
        #endregion

        #region IValidatable 接口实现
        /// <summary>
        /// 验证模型有效性
        /// </summary>
        /// <returns>是否有效</returns>
        public bool IsValid()
        {
            return CreatedTime <= DateTime.UtcNow &&
                   CreatedTime >= DateTime.UtcNow.AddYears(-1); // 创建时间在1年内
        }
        #endregion

        /// <summary>
        /// 响应数据包（用于发送回客户端）
        /// </summary>
        OriginalData ResponseData { get; set; }

        /// <summary>
        /// 创建成功结果
        /// </summary>
        public static CommandResult Success(object data = null, string message = "操作成功")
        {
            var result = new CommandResult
            {
                IsSuccess = true,
                Message = message,
                Data = data,
                CreatedTime = DateTime.UtcNow,
                Timestamp = DateTime.UtcNow,
                Version = "2.0"
            };
            return result;
        }

        /// <summary>
        /// 创建失败结果
        /// </summary>
        public static CommandResult Failure(string message, string errorCode = null, Exception exception = null)
        {
            var result = new CommandResult
            {
                IsSuccess = false,
                Message = message,
                ErrorCode = errorCode,
                Exception = exception,
                CreatedTime = DateTime.UtcNow,
                Timestamp = DateTime.UtcNow,
                Version = "2.0"
            };
            return result;
        }

        /// <summary>
        /// 创建带响应数据的成功结果
        /// </summary>
        public static CommandResult SuccessWithResponse(OriginalData responseData, object data = null, string message = "操作成功")
        {
            var result = new CommandResult
            {
                IsSuccess = true,
                Message = message,
                Data = data,
                ResponseData = responseData,
                CreatedTime = DateTime.UtcNow,
                Timestamp = DateTime.UtcNow,
                Version = "2.0"
            };
            return result;
        }


        /// <summary>
        /// 创建错误结果
        /// </summary>
        public static CommandResult CreateError(string message, string errorCode = null, Exception ex = null)
        {
            return Failure(message, errorCode, ex);
        }
    }
}
