using RUINORERP.PacketSpec.Validation;
using FluentValidation;
namespace RUINORERP.PacketSpec.Commands.Lock
{
    /// <summary>
    /// 申请锁命令验证器
    /// </summary>
    public class LockApplyCommandValidator : CommandValidator<LockApplyCommand>
    {
        public LockApplyCommandValidator()
        {
            // 申请锁命令的验证规则
            RuleFor(command => command.ResourceId)
                .NotEmpty()
                .WithMessage("资源标识符不能为空");

            RuleFor(command => command.LockType)
                .NotEmpty()
                .WithMessage("锁类型不能为空");

            RuleFor(command => command.TimeoutMs)
                .GreaterThan(0)
                .WithMessage("超时时间必须大于0");
        }
    }
}
