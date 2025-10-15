﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;

namespace RUINORERP.PacketSpec.Commands.Lock
{
    /// <summary>
    /// 广播锁定状态命令
    /// </summary>
    public class BroadcastLockStatusCommand : BaseCommand<BroadcastLockRequest, BroadcastLockResponse>
    {
        /// <summary>
        /// 锁定的单据信息列表
        /// </summary>
        public List<LockedInfo> LockedDocuments { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public BroadcastLockStatusCommand()
        {
            // 注意：移除了 TimeoutMs 的设置，因为指令本身不应该关心超时
            // 超时应该是执行环境的问题，由网络层或业务处理层处理
            // 注意：移除了 Direction 的设置，因为方向已由 PacketModel 统一控制
            CommandIdentifier = LockCommands.BroadcastLockStatus;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="lockedDocuments">锁定的单据信息列表</param>
        public BroadcastLockStatusCommand(List<LockedInfo> lockedDocuments)
        {
            LockedDocuments = lockedDocuments;
            // 注意：移除了 TimeoutMs 的设置，因为指令本身不应该关心超时
            // 超时应该是执行环境的问题，由网络层或业务处理层处理
            // 注意：移除了 Direction 的设置，因为方向已由 PacketModel 统一控制
            CommandIdentifier = LockCommands.BroadcastLockStatus;
        }

        /// <summary>
        /// 获取可序列化的数据
        /// </summary>
        /// <returns>可序列化的广播数据</returns>
        public override object GetSerializableData()
        {
            return this.LockedDocuments;
        }

        /// <summary>
        /// 验证命令参数
        /// </summary>
        /// <returns>验证结果</returns>
        public override async Task<ValidationResult> ValidateAsync(CancellationToken cancellationToken = default)
        {
            var result = await base.ValidateAsync(cancellationToken);
            if (!result.IsValid)
            {
                return result;
            }

            // 锁定文档列表可以为空，但不能为null
            if (LockedDocuments == null)
            {
                LockedDocuments = new List<LockedInfo>();
            }

            return new ValidationResult();
        }

       
    }
}
