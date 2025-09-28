using RUINORERP.Model.CommonModel;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Responses;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.PacketSpec.Commands.Lock
{
    /// <summary>
    /// 申请锁定单据命令 - 客户端向服务器申请锁定业务单据
    /// </summary>
    [PacketCommand("DocumentLockApply", CommandCategory.Lock)]
    public class DocumentLockApplyCommand : BaseCommand
    {
        /// <summary>
        /// 命令标识符
        /// </summary>
        public override CommandId CommandIdentifier => LockCommands.RequestLock;

        /// <summary>
        /// 单据ID
        /// </summary>
        public long BillId { get; set; }

        /// <summary>
        /// 单据信息
        /// </summary>
        public CommBillData BillData { get; set; }

        /// <summary>
        /// 菜单ID
        /// </summary>
        public long MenuId { get; set; }
        
        /// <summary>
        /// 分布 式锁实例
        /// </summary>
        public static IDistributedLock DistributedLock { get; set; } = new LocalDistributedLock();

        /// <summary>
        /// 构造函数
        /// </summary>
        public DocumentLockApplyCommand()
        {
            Direction = CommandDirection.Send;
            TimeoutMs = 30000; // 默认超时时间30秒
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="billData">单据信息</param>
        /// <param name="menuId">菜单ID</param>
        public DocumentLockApplyCommand(long billId, CommBillData billData, long menuId)
        {
            BillId = billId;
            BillData = billData;
            MenuId = menuId;
            Direction = CommandDirection.Send;
            TimeoutMs = 30000; // 默认超时时间30秒
        }

        /// <summary>
        /// 获取可序列化的数据
        /// </summary>
        /// <returns>可序列化的锁定申请数据</returns>
        public override object GetSerializableData()
        {
            return new
            {
                BillId = this.BillId,
                BillData = this.BillData,
                MenuId = this.MenuId
            };
        }

        /// <summary>
        /// 验证命令参数
        /// </summary>
        /// <returns>验证结果</returns>
        public override CommandValidationResult Validate()
        {
            var result = base.Validate();
            if (!result.IsValid)
            {
                return result;
            }

            // 验证单据ID
            if (BillId <= 0)
            {
                return CommandValidationResult.Failure("单据ID必须大于0", "INVALID_BILL_ID");
            }

            // 验证单据信息
            if (BillData == null)
            {
                return CommandValidationResult.Failure("单据信息不能为空", "INVALID_BILL_DATA");
            }

            // 验证业务名称
            if (string.IsNullOrWhiteSpace(BillData.BizName))
            {
                return CommandValidationResult.Failure("业务名称不能为空", "INVALID_BIZ_NAME");
            }

            // 验证单据编号
            if (string.IsNullOrWhiteSpace(BillData.BillNo))
            {
                return CommandValidationResult.Failure("单据编号不能为空", "INVALID_BILL_NO");
            }

            return CommandValidationResult.Success();
        }

        /// <summary>
        /// 执行命令的核心逻辑
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>命令执行结果</returns>
        protected override Task<ResponseBase> OnExecuteAsync(CancellationToken cancellationToken)
        {
            // 申请锁定单据命令契约只定义数据结构，实际的业务逻辑在Handler中实现
            // 创建并返回成功响应
            return Task.FromResult(ResponseBase.CreateSuccess("锁定申请操作成功"));
        }
        
        /// <summary>
        /// 尝试获取分布式锁
        /// </summary>
        /// <param name="lockKey">锁的键</param>
        /// <param name="timeout">超时时间</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>如果成功获取锁则返回true，否则返回false</returns>
        public static async Task<bool> TryAcquireLockAsync(string lockKey, TimeSpan timeout, CancellationToken cancellationToken = default)
        {
            return await DistributedLock.TryAcquireAsync(lockKey, timeout, cancellationToken);
        }
        
        /// <summary>
        /// 释放分布式锁
        /// </summary>
        /// <param name="lockKey">锁的键</param>
        /// <returns>任务</returns>
        public static async Task ReleaseLockAsync(string lockKey)
        {
            await DistributedLock.ReleaseAsync(lockKey);
        }
        
        /// <summary>
        /// 检查锁是否存在
        /// </summary>
        /// <param name="lockKey">锁的键</param>
        /// <returns>如果锁存在则返回true，否则返回false</returns>
        public static async Task<bool> IsLockExistsAsync(string lockKey)
        {
            return await DistributedLock.ExistsAsync(lockKey);
        }
    }
}
