﻿using FluentValidation.Results;
using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Responses;
using System.Threading;
using System.Threading.Tasks;
using MessagePack;
namespace RUINORERP.PacketSpec.Commands.Lock
{
    /// <summary>
    /// 拒绝解锁命令 - 锁定用户拒绝其他用户的解锁请求
    /// </summary>
    [PacketCommand("RefuseUnlock", CommandCategory.Lock)]
    public class RefuseUnlockCommand : BaseCommand
    {
 

        /// <summary>
        /// 拒绝解锁信息
        /// </summary>
        public RefuseUnLockInfo RefuseInfo { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public RefuseUnlockCommand()
        {
            // 注意：移除了 TimeoutMs 的设置，因为指令本身不应该关心超时
            // 超时应该是执行环境的问题，由网络层或业务处理层处理
            CommandIdentifier = LockCommands.RefuseUnlock;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="refuseInfo">拒绝解锁信息</param>
        public RefuseUnlockCommand(RefuseUnLockInfo refuseInfo)
        {
            RefuseInfo = refuseInfo;
            // 注意：移除了 TimeoutMs 的设置，因为指令本身不应该关心超时
            // 超时应该是执行环境的问题，由网络层或业务处理层处理
            CommandIdentifier = LockCommands.RefuseUnlock;
        }

        /// <summary>
        /// 获取可序列化的数据
        /// </summary>
        /// <returns>可序列化的拒绝解锁数据</returns>
        public override object GetSerializableData()
        {
            return this.RefuseInfo;
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

            // 验证拒绝信息
            if (RefuseInfo == null)
            {
                return new ValidationResult(new[] { new ValidationFailure(nameof(RefuseInfo), "拒绝解锁信息不能为空") });
            }

            // 验证单据ID
            if (RefuseInfo.BillID <= 0)
            {
                return new ValidationResult(new[] { new ValidationFailure("RefuseInfo.BillID", "单据ID必须大于0") });
            }

            // 验证请求用户ID
            if (RefuseInfo.RequestUserID <= 0)
            {
                return new ValidationResult(new[] { new ValidationFailure("RefuseInfo.RequestUserID", "请求用户ID必须大于0") });
            }

            // 验证拒绝用户ID
            if (RefuseInfo.RefuseUserID <= 0)
            {
                return new ValidationResult(new[] { new ValidationFailure("RefuseInfo.RefuseUserID", "拒绝用户ID必须大于0") });
            }

            // 验证单据信息
            if (RefuseInfo.BillData == null)
            {
                return new ValidationResult(new[] { new ValidationFailure("RefuseInfo.BillData", "单据信息不能为空") });
            }

            return new ValidationResult();
        }

       
    }
}
