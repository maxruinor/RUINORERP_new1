using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.PacketSpec.Commands.Lock
{
    /// <summary>
    /// 请求解锁命令 - 用户请求解锁被其他用户锁定的单据
    /// </summary>
    [PacketCommand("RequestUnlock", CommandCategory.Lock)]
    public class RequestUnlockCommand : BaseCommand
    {
        /// <summary>
        /// 命令标识符
        /// </summary>
        public override CommandId CommandIdentifier => LockCommands.RequestUnlock;

        /// <summary>
        /// 请求解锁信息
        /// </summary>
        public RequestUnLockInfo RequestInfo { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public RequestUnlockCommand()
        {
            Direction = PacketDirection.ClientToServer;
            TimeoutMs = 30000; // 默认超时时间30秒
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="requestInfo">请求解锁信息</param>
        public RequestUnlockCommand(RequestUnLockInfo requestInfo)
        {
            RequestInfo = requestInfo;
            Direction = PacketDirection.ClientToServer;
            TimeoutMs = 30000; // 默认超时时间30秒
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
        public override CommandValidationResult Validate()
        {
            var result = base.Validate();
            if (!result.IsValid)
            {
                return result;
            }

            // 验证请求信息
            if (RequestInfo == null)
            {
                return CommandValidationResult.Failure("请求解锁信息不能为空", "INVALID_REQUEST_INFO");
            }

            // 验证单据ID
            if (RequestInfo.BillID <= 0)
            {
                return CommandValidationResult.Failure("单据ID必须大于0", "INVALID_BILL_ID");
            }

            // 验证锁定用户ID
            if (RequestInfo.LockedUserID <= 0)
            {
                return CommandValidationResult.Failure("锁定用户ID必须大于0", "INVALID_LOCKED_USER_ID");
            }

            // 验证请求用户ID
            if (RequestInfo.RequestUserID <= 0)
            {
                return CommandValidationResult.Failure("请求用户ID必须大于0", "INVALID_REQUEST_USER_ID");
            }

            // 验证单据信息
            if (RequestInfo.BillData == null)
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
            // 请求解锁命令契约只定义数据结构，实际的业务逻辑在Handler中实现
            // 创建并返回成功响应
            return Task.FromResult(ResponseBase.CreateSuccess("请求解锁操作成功"));
        }
    }
}
