using RUINORERP.PacketSpec.Validation;
using FluentValidation;
using RUINORERP.PacketSpec.Models.Requests.Cache;

namespace RUINORERP.PacketSpec.Commands.Cache
{
    /// <summary>
    /// 缓存命令验证器 - 基于泛型命令的请求数据验证
    /// </summary>
    public class CacheCommandValidator : CommandValidator<CacheCommand>
    {
        public CacheCommandValidator()
        {
            // 验证缓存请求数据
            RuleFor(command => command.Request)
                .NotNull()
                .WithMessage("缓存请求数据不能为空");

            // 验证表名 - 当请求存在时
            RuleFor(command => command.Request.TableName)
                .NotEmpty()
                .WithMessage("表名不能为空")
                .When(command => command.Request != null);

            // 验证会话ID - 当请求存在时
            RuleFor(command => command.Request.SessionId)
                .NotEmpty()
                .WithMessage("会话ID不能为空")
                .When(command => command.Request != null && string.IsNullOrEmpty(command.Request.SessionId));

            // 验证操作类型
            RuleFor(command => command.Request.OperationType)
                .NotEmpty()
                .WithMessage("操作类型不能为空")
                .When(command => command.Request != null);
        }
    }
}
