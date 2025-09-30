using RUINORERP.PacketSpec.Validation;
using FluentValidation;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 通用命令验证器
    /// </summary>
    /// <typeparam name="TPayload">负载类型</typeparam>
    public class GenericCommandValidator<TPayload> : CommandValidator<GenericCommand<TPayload>>
    {
        public GenericCommandValidator()
        {
            // 通用命令的验证规则
            RuleFor(command => command.Payload)
                .NotNull()
                .WithMessage("Payload不能为空");
        }
    }
}