using RUINORERP.PacketSpec.Validation;
using FluentValidation;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 回显命令验证器
    /// </summary>
    public class EchoCommandValidator : CommandValidator<EchoCommand>
    {
        public EchoCommandValidator()
        {
            // 回显命令的验证规则
            RuleFor(command => command.Message)
                .NotEmpty()
                .WithMessage("消息不能为空");
        }
    }
}