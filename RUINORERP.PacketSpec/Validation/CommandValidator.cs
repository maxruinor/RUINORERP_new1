using FluentValidation;
using RUINORERP.PacketSpec.Commands;

namespace RUINORERP.PacketSpec.Validation
{
    /// <summary>
    /// 命令验证器基类
    /// </summary>
    /// <typeparam name="TCommand">命令类型</typeparam>
    public abstract class CommandValidator<TCommand> : AbstractValidator<TCommand>
        where TCommand : ICommand
    {
        protected CommandValidator()
        {
            // 添加通用验证规则
            RuleFor(command => command.CommandIdentifier != 0)
                .NotEmpty()
                .WithMessage("命令ID不能为空");

            RuleFor(command => command.CommandIdentifier)
                .NotEqual(default(CommandId))
                .WithMessage("命令标识符不能为默认值");

            RuleFor(command => command.TimeoutMs)
                .GreaterThan(0)
                .WithMessage("超时时间必须大于0");
        }
    }
}
