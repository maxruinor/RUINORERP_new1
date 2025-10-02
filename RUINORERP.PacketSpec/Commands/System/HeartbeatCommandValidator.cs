using RUINORERP.PacketSpec.Validation;
using FluentValidation;
namespace RUINORERP.PacketSpec.Commands.System
{
    /// <summary>
    /// 心跳命令验证器
    /// </summary>
    public class HeartbeatCommandValidator : CommandValidator<HeartbeatCommand>
    {
        public HeartbeatCommandValidator()
        {
            // 添加心跳命令特定的验证规则
            //RuleFor(command => command.ClientId)
            //    .NotEmpty()
            //    .WithMessage("客户端ID不能为空");

            //RuleFor(command => command.UserId)
            //    .GreaterThanOrEqualTo(0)
            //    .WithMessage("用户ID不能为负数");

            //// 如果用户ID大于0，则会话令牌不能为空
            //RuleFor(command => command.SessionToken)
            //    .NotEmpty()
            //    .When(command => command.UserId > 0)
            //    .WithMessage("已登录用户必须提供会话令牌");
        }
    }
}
