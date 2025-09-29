﻿﻿using System;
using System.Threading;
using System.Threading.Tasks;
using RUINORERP.PacketSpec.Protocol;
using RUINORERP.PacketSpec.Core;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.PacketSpec.Enums.Core;

namespace RUINORERP.PacketSpec.Commands
{

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
        string SessionId { get; set; }

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
        PacketDirection Direction { get; set; }

        /// <summary>
        /// 命令优先级
        /// </summary>
        CommandPriority Priority { get; set; }

        /// <summary>
        /// 命令状态
        /// </summary>
        CommandStatus Status { get; set; }

        /// <summary>
        /// 数据包模型 - 包含完整的数据包信息和业务数据
        /// </summary>
        PacketModel Packet { get; set; }

        /// <summary>
        /// 命令创建时间
        /// </summary>
        DateTime CreatedAtUtc { get; }

        /// <summary>
        /// 命令超时时间（毫秒）
        /// </summary>
        int TimeoutMs { get; set; }

        /// <summary>
        /// 命令名称
        /// </summary>
        string CommandName { get; set; }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>命令执行结果</returns>
        Task<ResponseBase> ExecuteAsync(CancellationToken cancellationToken = default);

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

        /// <summary>
        /// 获取可序列化的数据
        /// </summary>
        /// <returns>可序列化的数据</returns>
        object GetSerializableData();
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

   
}
