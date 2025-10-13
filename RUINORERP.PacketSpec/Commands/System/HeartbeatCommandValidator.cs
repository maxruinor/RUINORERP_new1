using RUINORERP.PacketSpec.Validation;
using FluentValidation;
using RUINORERP.PacketSpec.Models.Requests;
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
            RuleFor(command => command.Request)
                .NotNull()
                .WithMessage("心跳请求数据不能为空");

            RuleFor(command => command.Request.SessionToken)
                .NotEmpty()
                .When(command => command.Request.UserId > 0)
                .WithMessage("已登录用户必须提供会话令牌");

            RuleFor(command => command.Request.UserId)
                .GreaterThanOrEqualTo(0)
                .WithMessage("用户ID不能为负数");

            RuleFor(command => command.Request.ClientId)
                .NotEmpty()
                .When(command => !string.IsNullOrEmpty(command.Request.ClientId))
                .WithMessage("客户端ID不能为空");

            // 响应验证规则
            RuleFor(command => command.Response)
                .NotNull()
                .When(command => command.Response != null)
                .WithMessage("心跳响应数据不能为空");

            RuleFor(command => command.Response.Status)
                .NotEmpty()
                .When(command => command.Response != null)
                .WithMessage("响应状态不能为空");

            RuleFor(command => command.Response.ServerTimestamp)
                .GreaterThan(0)
                .When(command => command.Response != null)
                .WithMessage("服务器时间戳必须大于0");

            RuleFor(command => command.Response.NextIntervalMs)
                .GreaterThan(0)
                .When(command => command.Response != null)
                .WithMessage("下次心跳间隔必须大于0");
        }
    }
}
