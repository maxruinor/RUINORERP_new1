using System;
using System.Threading;
using System.Threading.Tasks;
using RUINORERP.PacketSpec.Enums;
using RUINORERP.PacketSpec.Models;
using OriginalData = RUINORERP.PacketSpec.Protocol.OriginalData;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 命令操作类型
    /// </summary>
    public enum CmdOperation
    {
        /// <summary>
        /// 接收命令
        /// </summary>
        Receive = 0,

        /// <summary>
        /// 发送命令
        /// </summary>
        Send = 1,

        /// <summary>
        /// 转发命令
        /// </summary>
        Forward = 2,

        /// <summary>
        /// 广播命令
        /// </summary>
        Broadcast = 3
    }

    /// <summary>
    /// 服务器命令接口 - 定义命令的基本结构和行为
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// 操作类型
        /// </summary>
        CmdOperation OperationType { get; set; }

        /// <summary>
        /// 数据包
        /// </summary>
        OriginalData? DataPacket { get; set; }

        /// <summary>
        /// 命令执行异步方法
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>执行结果</returns>
        Task<CommandResult> ExecuteAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 解析数据包
        /// </summary>
        /// <param name="data">原始数据</param>
        /// <param name="sessionInfo">会话信息</param>
        /// <returns>解析是否成功</returns>
        bool AnalyzeDataPacket(OriginalData data, SessionInfo sessionInfo);

        /// <summary>
        /// 构建数据包
        /// </summary>
        /// <param name="request">请求对象</param>
        void BuildDataPacket(object request = null);

        /// <summary>
        /// 验证命令是否可以处理
        /// </summary>
        /// <returns>是否可以处理</returns>
        bool CanExecute();
    }

    /// <summary>
    /// 命令执行结果
    /// </summary>
    public class CommandResult
    {
        /// <summary>
        /// 执行是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 结果消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 返回数据
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 错误代码
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// 执行时间（毫秒）
        /// </summary>
        public long ExecutionTimeMs { get; set; }

        /// <summary>
        /// 创建成功结果
        /// </summary>
        public static CommandResult CreateSuccess(object data = null, string message = "执行成功")
        {
            return new CommandResult
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        /// <summary>
        /// 创建失败结果
        /// </summary>
        public static CommandResult CreateError(string message, string errorCode = null)
        {
            return new CommandResult
            {
                Success = false,
                Message = message,
                ErrorCode = errorCode
            };
        }
    }

    /// <summary>
    /// 命令处理器基类
    /// </summary>
    public abstract class BaseCommand : ICommand
    {
        /// <summary>
        /// 操作类型
        /// </summary>
        public CmdOperation OperationType { get; set; }

        /// <summary>
        /// 数据包
        /// </summary>
        public OriginalData? DataPacket { get; set; }

        /// <summary>
        /// 会话信息
        /// </summary>
        public SessionInfo SessionInfo { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedTime { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        protected BaseCommand(CmdOperation operationType = CmdOperation.Receive)
        {
            OperationType = operationType;
            CreatedTime = DateTime.UtcNow;
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        public abstract Task<CommandResult> ExecuteAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 解析数据包
        /// </summary>
        public abstract bool AnalyzeDataPacket(OriginalData data, SessionInfo sessionInfo);

        /// <summary>
        /// 构建数据包
        /// </summary>
        public abstract void BuildDataPacket(object request = null);

        /// <summary>
        /// 验证命令是否可以处理
        /// </summary>
        public virtual bool CanExecute()
        {
            return DataPacket.HasValue || OperationType == CmdOperation.Send;
        }

        /// <summary>
        /// 记录执行日志
        /// </summary>
        protected virtual void LogExecution(string message, Exception ex = null)
        {
            // 这里可以集成实际的日志系统
            Console.WriteLine($"[Command] {GetType().Name}: {message}");
            if (ex != null)
            {
                Console.WriteLine($"[Command] Exception: {ex.Message}");
            }
        }
    }
}