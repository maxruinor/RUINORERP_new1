using RUINORERP.PacketSpec.Validation;
using FluentValidation;

namespace RUINORERP.PacketSpec.Commands.Message
{
    /// <summary>
    /// 消息命令验证器
    /// </summary>
    public class MessageCommandValidator : CommandValidator<MessageCommand>
    {
        public MessageCommandValidator()
        {
            // 消息命令的验证规则
            RuleFor(command => command.CommandType)
                .NotEqual(0u)
                .WithMessage("命令类型不能为空");
        }
    }
}