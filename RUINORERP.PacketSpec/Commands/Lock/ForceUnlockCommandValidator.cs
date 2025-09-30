using RUINORERP.PacketSpec.Validation;
using FluentValidation;

namespace RUINORERP.PacketSpec.Commands.Lock
{
    /// <summary>
    /// 强制解锁命令验证器
    /// </summary>
    public class ForceUnlockCommandValidator : CommandValidator<ForceUnlockCommand>
    {
        public ForceUnlockCommandValidator()
        {
            // 强制解锁命令的验证规则
            RuleFor(command => command.BillId)
                .GreaterThan(0)
                .WithMessage("单据ID必须大于0");
        }
    }
}