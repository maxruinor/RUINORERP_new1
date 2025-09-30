using RUINORERP.PacketSpec.Validation;
using FluentValidation;
namespace RUINORERP.PacketSpec.Commands.Cache
{
    /// <summary>
    /// 缓存命令验证器
    /// </summary>
    public class CacheCommandValidator : CommandValidator<CacheCommand>
    {
        public CacheCommandValidator()
        {
            // 添加缓存命令特定的验证规则
            RuleFor(command => command.SyncMode)
                .NotEmpty()
                .WithMessage("同步模式不能为空");

            RuleFor(command => command)
                .Must(command => command.CacheKeys != null || command.CacheKeysEnumerator != null)
                .WithMessage("缓存键列表不能为空");
        }
    }
}
