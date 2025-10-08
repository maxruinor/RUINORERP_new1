﻿using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Responses;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using MessagePack;

namespace RUINORERP.PacketSpec.Commands.Lock
{
    /// <summary>
    /// 广播锁状态命令 - 服务器向所有客户端广播锁状态变化
    /// </summary>
    [PacketCommand("BroadcastLockStatus", CommandCategory.Lock)]
    public class BroadcastLockStatusCommand : BaseCommand
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
            Direction = PacketDirection.ServerToClient;
            TimeoutMs = 30000; // 默认超时时间30秒
            CommandIdentifier = LockCommands.BroadcastLockStatus;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="lockedDocuments">锁定的单据信息列表</param>
        public BroadcastLockStatusCommand(List<LockedInfo> lockedDocuments)
        {
            LockedDocuments = lockedDocuments;
            Direction = PacketDirection.ServerToClient;
            TimeoutMs = 30000; // 默认超时时间30秒
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
