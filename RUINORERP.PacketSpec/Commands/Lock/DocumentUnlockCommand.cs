using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.PacketSpec.Commands.Lock
{
    /// <summary>
    /// 解锁单据命令 - 客户端向服务器请求解锁业务单据
    /// </summary>
    [PacketCommand("DocumentUnlock", CommandCategory.Lock)]
    public class DocumentUnlockCommand : BaseCommand
    {
        /// <summary>
        /// 命令标识符
        /// </summary>
        public override CommandId CommandIdentifier => LockCommands.ReleaseLock;

        /// <summary>
        /// 单据ID
        /// </summary>
        public long BillId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public DocumentUnlockCommand()
        {
            Direction = PacketDirection.ClientToServer;
            TimeoutMs = 30000; // 默认超时时间30秒
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="userId">用户ID</param>
        public DocumentUnlockCommand(long billId, long userId)
        {
            BillId = billId;
            UserId = userId;
            Direction = PacketDirection.ClientToServer;
            TimeoutMs = 30000; // 默认超时时间30秒
        }

        /// <summary>
        /// 获取可序列化的数据
        /// </summary>
        /// <returns>可序列化的解锁数据</returns>
        public override object GetSerializableData()
        {
            return new
            {
                BillId = this.BillId,
                UserId = this.UserId
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

            // 验证用户ID
            if (UserId <= 0)
            {
                return CommandValidationResult.Failure("用户ID必须大于0", "INVALID_USER_ID");
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
            // 解锁单据命令契约只定义数据结构，实际的业务逻辑在Handler中实现
            // 创建并返回成功响应
            return Task.FromResult(ResponseBase.CreateSuccess("解锁操作成功"));
        }
    }
}
