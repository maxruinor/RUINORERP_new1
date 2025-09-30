using RUINORERP.PacketSpec.Validation;
using FluentValidation;
namespace RUINORERP.PacketSpec.Commands.Lock
{
    /// <summary>
    /// 解锁单据命令验证器
    /// </summary>
    public class DocumentUnlockCommandValidator : CommandValidator<DocumentUnlockCommand>
    {
        public DocumentUnlockCommandValidator()
        {
            // 添加解锁单据命令特定的验证规则
            RuleFor(command => command.BillId)
                .GreaterThan(0)
                .WithMessage("单据ID必须大于0");

            RuleFor(command => command.UserId)
                .GreaterThan(0)
                .WithMessage("用户ID必须大于0");
        }
    }
}
