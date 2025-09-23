using RUINORERP.PacketSpec.Models.Core;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.PacketSpec.Commands.Lock
{
    /// <summary>
    /// 申请锁命令 - 客户端向服务器申请获取资源锁
    /// </summary>
    [PacketCommand("LockApply", CommandCategory.Lock)]
    public class LockApplyCommand : BaseCommand
    {
        /// <summary>
        /// 命令标识符
        /// </summary>
        public override CommandId CommandIdentifier => LockCommands.LockRequest;

        /// <summary>
        /// 资源标识符
        /// </summary>
        public string ResourceId { get; set; }

        /// <summary>
        /// 锁类型
        /// </summary>
        public string LockType { get; set; }

        /// <summary>
        /// 超时时间（毫秒）
        /// </summary>
        public int TimeoutMs { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public LockApplyCommand()
        {
            LockType = "EXCLUSIVE"; // 默认排他锁
            TimeoutMs = 30000; // 默认超时时间30秒
            Direction = CommandDirection.Send;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="resourceId">资源标识符</param>
        /// <param name="lockType">锁类型</param>
        /// <param name="timeoutMs">超时时间</param>
        public LockApplyCommand(string resourceId, string lockType = "EXCLUSIVE", int timeoutMs = 30000)
        {
            ResourceId = resourceId;
            LockType = lockType;
            TimeoutMs = timeoutMs;
            Direction = CommandDirection.Send;
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

            // 验证资源标识符
            if (string.IsNullOrWhiteSpace(ResourceId))
            {
                return CommandValidationResult.Failure("资源标识符不能为空", "INVALID_RESOURCE_ID");
            }

            // 验证锁类型
            if (string.IsNullOrWhiteSpace(LockType))
            {
                return CommandValidationResult.Failure("锁类型不能为空", "INVALID_LOCK_TYPE");
            }

            // 验证超时时间
            if (TimeoutMs <= 0)
            {
                return CommandValidationResult.Failure("超时时间必须大于0", "INVALID_TIMEOUT");
            }

            return CommandValidationResult.Success();
        }

        /// <summary>
        /// 执行命令的核心逻辑
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>命令执行结果</returns>
        protected override Task<CommandResult> OnExecuteAsync(CancellationToken cancellationToken)
        {
            // 申请锁命令契约只定义数据结构，实际的业务逻辑在Handler中实现
            var result = CommandResult.Success(
                data: new { ResourceId = ResourceId, LockType = LockType, TimeoutMs = TimeoutMs },
                message: "申请锁命令构建成功"
            );
            return Task.FromResult(result);
        }
    }
}