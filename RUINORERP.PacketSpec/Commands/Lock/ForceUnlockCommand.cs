﻿using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;

namespace RUINORERP.PacketSpec.Commands.Lock
{
    /// <summary>
    /// 强制解锁命令
    /// </summary>
    public class ForceUnlockCommand : BaseCommand<ForceUnlockRequest, ForceUnlockResponse>
    {
        /// <summary>
        /// 单据ID
        /// </summary>
        public long BillId { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ForceUnlockCommand()
        {
            // 注意：移除了 TimeoutMs 的设置，因为指令本身不应该关心超时
            // 超时应该是执行环境的问题，由网络层或业务处理层处理
            // 注意：移除了 Direction 的设置，因为方向已由 PacketModel 统一控制
            CommandIdentifier = LockCommands.ForceReleaseLock;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="billId">单据ID</param>
        public ForceUnlockCommand(long billId)
        {
            BillId = billId;
            // 注意：移除了 TimeoutMs 的设置，因为指令本身不应该关心超时
            // 超时应该是执行环境的问题，由网络层或业务处理层处理
            // 注意：移除了 Direction 的设置，因为方向已由 PacketModel 统一控制
            CommandIdentifier = LockCommands.ForceReleaseLock;
        }

        /// <summary>
        /// 获取可序列化的数据
        /// </summary>
        /// <returns>可序列化的强制解锁数据</returns>
        public override object GetSerializableData()
        {
            return new
            {
                BillId = this.BillId
            };
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

            // 验证单据ID
            if (BillId <= 0)
            {
                return new ValidationResult(new[] { new ValidationFailure(nameof(BillId), "单据ID必须大于0") });
            }

            return new ValidationResult();
        }

       
    }
}
