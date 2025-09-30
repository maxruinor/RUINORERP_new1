using RUINORERP.PacketSpec.Validation;
using FluentValidation;

namespace RUINORERP.PacketSpec.Commands.Message
{
    /// <summary>
    /// 发送弹窗消息命令验证器
    /// </summary>
    public class SendPopupMessageCommandValidator : CommandValidator<SendPopupMessageCommand>
    {
        public SendPopupMessageCommandValidator()
        {
            // 添加发送弹窗消息命令特定的验证规则
            RuleFor(command => command.Title)
                .NotEmpty()
                .WithMessage("消息标题不能为空");

            RuleFor(command => command.Content)
                .NotEmpty()
                .WithMessage("消息内容不能为空");

            RuleFor(command => command.MessageType)
                .NotEmpty()
                .WithMessage("消息类型不能为空");
        }
    }
}