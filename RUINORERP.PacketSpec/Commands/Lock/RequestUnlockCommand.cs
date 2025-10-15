﻿using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Responses;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using MessagePack;

namespace RUINORERP.PacketSpec.Commands.Lock
{
    /// <summary>
    /// 请求解锁命令 - 用户请求解锁被其他用户锁定的单据
    /// </summary>
    [PacketCommand("RequestUnlock", CommandCategory.Lock)]
    public class RequestUnlockCommand : BaseCommand
    {
 

        /// <summary>
        /// 请求解锁信息
        /// </summary>
        public RequestUnLockInfo RequestInfo { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public RequestUnlockCommand()
        {
            // 注意：移除了 TimeoutMs 的设置，因为指令本身不应该关心超时
            // 超时应该是执行环境的问题，由网络层或业务处理层处理
            CommandIdentifier = LockCommands.RequestUnlock;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="requestInfo">请求解锁信息</param>
        public RequestUnlockCommand(RequestUnLockInfo requestInfo)
        {
            RequestInfo = requestInfo;
            // 注意：移除了 TimeoutMs 的设置，因为指令本身不应该关心超时
            // 超时应该是执行环境的问题，由网络层或业务处理层处理
            CommandIdentifier = LockCommands.RequestUnlock;
        }

        /// <summary>
        /// 获取可序列化的数据
        /// </summary>
        /// <returns>可序列化的请求解锁数据</returns>
        public override object GetSerializableData()
        {
            return this.RequestInfo;
        }

        /// <summary>
        /// 验证命令参数
        /// </summary>
        /// <returns>验证结果</returns>
        public override async Task<ValidationResult> ValidateAsync(CancellationToken cancellationToken = default)
        {
            // 调用基类验证方法，将使用独立的验证器类进行验证
            var result = await base.ValidateAsync(cancellationToken);
            return result;
        }

        
    }
}
