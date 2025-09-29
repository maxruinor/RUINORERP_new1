using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.PacketSpec.Commands.Lock
{
    /// <summary>
    /// 拒绝解锁命令 - 锁定用户拒绝其他用户的解锁请求
    /// </summary>
    [PacketCommand("RefuseUnlock", CommandCategory.Lock)]
    public class RefuseUnlockCommand : BaseCommand
    {
        /// <summary>
        /// 命令标识符
        /// </summary>
        public override CommandId CommandIdentifier => LockCommands.RefuseUnlock;

        /// <summary>
        /// 拒绝解锁信息
        /// </summary>
        public RefuseUnLockInfo RefuseInfo { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public RefuseUnlockCommand()
        {
            Direction = PacketDirection.ClientToServer;
            TimeoutMs = 30000; // 默认超时时间30秒
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="refuseInfo">拒绝解锁信息</param>
        public RefuseUnlockCommand(RefuseUnLockInfo refuseInfo)
        {
            RefuseInfo = refuseInfo;
            Direction = PacketDirection.ClientToServer;
            TimeoutMs = 30000; // 默认超时时间30秒
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
        public override CommandValidationResult Validate()
        {
            var result = base.Validate();
            if (!result.IsValid)
            {
                return result;
            }

            // 验证拒绝信息
            if (RefuseInfo == null)
            {
                return CommandValidationResult.Failure("拒绝解锁信息不能为空", "INVALID_REFUSE_INFO");
            }

            // 验证单据ID
            if (RefuseInfo.BillID <= 0)
            {
                return CommandValidationResult.Failure("单据ID必须大于0", "INVALID_BILL_ID");
            }

            // 验证请求用户ID
            if (RefuseInfo.RequestUserID <= 0)
            {
                return CommandValidationResult.Failure("请求用户ID必须大于0", "INVALID_REQUEST_USER_ID");
            }

            // 验证拒绝用户ID
            if (RefuseInfo.RefuseUserID <= 0)
            {
                return CommandValidationResult.Failure("拒绝用户ID必须大于0", "INVALID_REFUSE_USER_ID");
            }

            // 验证单据信息
            if (RefuseInfo.BillData == null)
            {
                return CommandValidationResult.Failure("单据信息不能为空", "INVALID_BILL_DATA");
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
            // 拒绝解锁命令契约只定义数据结构，实际的业务逻辑在Handler中实现
            // 创建并返回成功响应
            return Task.FromResult(ResponseBase.CreateSuccess("拒绝解锁操作成功"));
        }
    }
}
