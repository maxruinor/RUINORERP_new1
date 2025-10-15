using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;

namespace RUINORERP.PacketSpec.Commands.Lock
{
    /// <summary>
    /// 锁申请命令
    /// </summary>
    public class LockApplyCommand : BaseCommand<LockRequest, LockResponse>
    {
        /// <summary>
        /// 资源标识符
        /// </summary>
        public string ResourceId { get; set; }

        /// <summary>
        /// 锁类型
        /// </summary>
        public string LockType { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public LockApplyCommand()
        {
            LockType = "EXCLUSIVE"; // 默认排他锁
            // 注意：移除了 TimeoutMs 的设置，因为指令本身不应该关心超时
            // 超时应该是执行环境的问题，由网络层或业务处理层处理
            // 注意：移除了 Direction 的设置，因为方向已由 PacketModel 统一控制
            CommandIdentifier = LockCommands.LockRequest;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="resourceId">资源标识符</param>
        /// <param name="lockType">锁类型</param>
        /// <param name="timeoutMs">超时时间（已移除，仅保留参数以保持兼容性）</param>
        public LockApplyCommand(string resourceId, string lockType = "EXCLUSIVE", int timeoutMs = 30000)
        {
            ResourceId = resourceId;
            LockType = lockType;
            // 注意：移除了 TimeoutMs 的设置，因为指令本身不应该关心超时
            // 超时应该是执行环境的问题，由网络层或业务处理层处理
            // 注意：移除了 Direction 的设置，因为方向已由 PacketModel 统一控制
            CommandIdentifier = LockCommands.LockRequest;
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

            // 验证资源标识符
            if (string.IsNullOrWhiteSpace(ResourceId))
            {
                return new ValidationResult(new[] { new ValidationFailure(nameof(ResourceId), "资源标识符不能为空") });
            }

            // 验证锁类型
            if (string.IsNullOrWhiteSpace(LockType))
            {
                return new ValidationResult(new[] { new ValidationFailure(nameof(LockType), "锁类型不能为空") });
            }

            // 注意：移除了对TimeoutMs的验证，因为指令本身不应该关心超时
            // 超时应该是执行环境的问题，由网络层或业务处理层处理

            return new ValidationResult();
        }

        
    }
}
