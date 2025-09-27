using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.PacketSpec.Commands.Lock
{
    /// <summary>
    /// 查询锁状态命令 - 客户端向服务器查询业务单据锁定状态
    /// </summary>
    [PacketCommand("QueryLockStatus", CommandCategory.Lock)]
    public class QueryLockStatusCommand : BaseCommand
    {
        /// <summary>
        /// 命令标识符
        /// </summary>
        public override CommandId CommandIdentifier => LockCommands.QueryLockStatus;

        /// <summary>
        /// 单据ID
        /// </summary>
        public long BillId { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public QueryLockStatusCommand()
        {
            Direction = CommandDirection.Send;
            TimeoutMs = 30000; // 默认超时时间30秒
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="billId">单据ID</param>
        public QueryLockStatusCommand(long billId)
        {
            BillId = billId;
            Direction = CommandDirection.Send;
            TimeoutMs = 30000; // 默认超时时间30秒
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

            return CommandValidationResult.Success();
        }

        /// <summary>
        /// 执行命令的核心逻辑
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>命令执行结果</returns>
        protected override Task<ResponseBase> OnExecuteAsync(CancellationToken cancellationToken)
        {
            // 查询锁状态命令契约只定义数据结构，实际的业务逻辑在Handler中实现
            // 创建并返回成功响应
            return Task.FromResult(ResponseBase.CreateSuccess("查询锁状态操作成功"));
        }
    }
}