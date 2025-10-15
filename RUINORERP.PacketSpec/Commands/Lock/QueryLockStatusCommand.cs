﻿using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Responses;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using MessagePack;
namespace RUINORERP.PacketSpec.Commands.Lock
{
    /// <summary>
    /// 查询锁状态命令 - 客户端向服务器查询业务单据锁定状态
    /// </summary>
    [PacketCommand("QueryLockStatus", CommandCategory.Lock)]
    public class QueryLockStatusCommand : BaseCommand
    {
 

        /// <summary>
        /// 单据ID
        /// </summary>
        public long BillId { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public QueryLockStatusCommand()
        {
            // 注意：移除了 TimeoutMs 的设置，因为指令本身不应该关心超时
            // 超时应该是执行环境的问题，由网络层或业务处理层处理
            CommandIdentifier = LockCommands.QueryLockStatus;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="billId">单据ID</param>
        public QueryLockStatusCommand(long billId)
        {
            BillId = billId;
            // 注意：移除了 TimeoutMs 的设置，因为指令本身不应该关心超时
            // 超时应该是执行环境的问题，由网络层或业务处理层处理
            CommandIdentifier = LockCommands.QueryLockStatus;
        }

        /// <summary>
        /// 获取可序列化的数据
        /// </summary>
        /// <returns>可序列化的查询数据</returns>
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
