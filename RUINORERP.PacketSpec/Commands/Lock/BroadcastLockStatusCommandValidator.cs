using FluentValidation;
using RUINORERP.PacketSpec.Validation;

namespace RUINORERP.PacketSpec.Commands.Lock
{
    /// <summary>
    /// 广播锁状态命令验证器
    /// </summary>
    public class BroadcastLockStatusCommandValidator : CommandValidator<BroadcastLockStatusCommand>
    {
        public BroadcastLockStatusCommandValidator()
        {
            // 广播锁状态命令的验证规则
            // 锁定文档列表可以为空，但不能为null，在验证时确保它不为null
            RuleFor(command => command.LockedDocuments)
                .NotNull()
                .WithMessage("锁定的单据信息列表不能为空");
        }
    }
}
