using FluentValidation.Results;
using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Responses;
using System.Threading;
using System.Threading.Tasks;
using MessagePack;
namespace RUINORERP.PacketSpec.Commands.Lock
{
    /// <summary>
    /// 申请锁命令 - 客户端向服务器申请获取资源锁
    /// </summary>
    [PacketCommand("LockApply", CommandCategory.Lock)]
    [MessagePackObject]
    public class LockApplyCommand : BaseCommand
    {
   

        /// <summary>
        /// 资源标识符
        /// </summary>
        [Key(0)]
        public string ResourceId { get; set; }

        /// <summary>
        /// 锁类型
        /// </summary>
        [Key(1)]
        public string LockType { get; set; }

 

        /// <summary>
        /// 构造函数
        /// </summary>
        public LockApplyCommand()
        {
            LockType = "EXCLUSIVE"; // 默认排他锁
            TimeoutMs = 30000; // 默认超时时间30秒
            Direction = PacketDirection.ClientToServer;
            CommandIdentifier = LockCommands.LockRequest;
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
            Direction = PacketDirection.ClientToServer;
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

            // 验证超时时间
            if (TimeoutMs <= 0)
            {
                return new ValidationResult(new[] { new ValidationFailure(nameof(TimeoutMs), "超时时间必须大于0") });
            }

            return new ValidationResult();
        }

        
    }
}
