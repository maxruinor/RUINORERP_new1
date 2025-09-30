using RUINORERP.PacketSpec.Validation;
using FluentValidation;

namespace RUINORERP.PacketSpec.Commands.Lock
{
    /// <summary>
    /// 请求解锁命令验证器
    /// </summary>
    public class RequestUnlockCommandValidator : CommandValidator<RequestUnlockCommand>
    {
        public RequestUnlockCommandValidator()
        {
            // 请求解锁命令的验证规则
            RuleFor(command => command.RequestInfo)
                .NotNull()
                .WithMessage("请求解锁信息不能为空");

            RuleFor(command => command.RequestInfo)
                .Must(info => info == null || info.BillID > 0)
                .WithMessage("单据ID必须大于0");

            RuleFor(command => command.RequestInfo)
                .Must(info => info == null || info.LockedUserID > 0)
                .WithMessage("锁定用户ID必须大于0");

            RuleFor(command => command.RequestInfo)
                .Must(info => info == null || info.RequestUserID > 0)
                .WithMessage("请求用户ID必须大于0");

            RuleFor(command => command.RequestInfo)
                .Must(info => info == null || info.BillData != null)
                .WithMessage("单据信息不能为空");
        }
    }
}