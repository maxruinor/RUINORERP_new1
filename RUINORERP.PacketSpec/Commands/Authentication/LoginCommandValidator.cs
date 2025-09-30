using RUINORERP.PacketSpec.Validation;
using FluentValidation;

namespace RUINORERP.PacketSpec.Commands.Authentication
{
    /// <summary>
    /// 登录命令验证器
    /// </summary>
    public class LoginCommandValidator : CommandValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            // 添加登录命令特定的验证规则
            RuleFor(command => command.LoginRequest)
                .NotNull()
                .WithMessage("登录请求数据不能为空");

            // 验证用户名
            RuleFor(command => command.LoginRequest.Username)
                .NotEmpty()
                .WithMessage("用户名不能为空")
                .MinimumLength(3)
                .WithMessage("用户名长度不能少于3个字符");

            // 验证密码
            RuleFor(command => command.LoginRequest.Password)
                .NotEmpty()
                .WithMessage("密码不能为空")
                .MinimumLength(6)
                .WithMessage("密码长度不能少于6个字符");
        }
    }
}