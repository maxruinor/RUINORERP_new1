﻿using System;
using System.Threading;
using System.Threading.Tasks;
using RUINORERP.PacketSpec.Protocol;
using RUINORERP.PacketSpec.Core;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.PacketSpec.Enums.Core;
using FluentValidation.Results;

namespace RUINORERP.PacketSpec.Commands
{
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
        /// 命令标识符（类型安全命令系统）
        /// </summary>
        CommandId CommandIdentifier { get; }

        /// <summary>
        /// 命令创建时间
        /// </summary>
        DateTime CreatedTimeUtc { get; }

        DateTime? LastUpdatedTime { get; set; }

        /// <summary>
        /// 时间戳（UTC时间）
        /// </summary>
        DateTime TimestampUtc { get; set; }


        /// <summary>
        /// 命令优先级
        /// </summary>
        CommandPriority Priority { get; set; }

        /// <summary>
        /// 命令超时时间（毫秒）
        /// </summary>
        int TimeoutMs { get; set; }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>命令执行结果</returns>
        Task<ResponseBase> ExecuteAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 业务数据
        /// 当命令从网络层接收数据时，我们先将字节数组存储到BizData，然后当业务逻辑需要时，我们再反序列化为TRequest。
        /// </summary>
        byte[] BizData { get; set; }
      
   
        /// <summary>
        /// 验证命令
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>验证结果</returns>
        Task<ValidationResult> ValidateAsync(CancellationToken cancellationToken = default);

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
}
