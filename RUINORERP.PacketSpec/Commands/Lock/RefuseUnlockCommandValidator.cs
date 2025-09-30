using RUINORERP.PacketSpec.Validation;
using FluentValidation;

namespace RUINORERP.PacketSpec.Commands.Lock
{
    /// <summary>
    /// 拒绝解锁命令验证器
    /// </summary>
    public class RefuseUnlockCommandValidator : CommandValidator<RefuseUnlockCommand>
    {
        public RefuseUnlockCommandValidator()
        {
            // 拒绝解锁命令的验证规则
            RuleFor(command => command.RefuseInfo)
                .NotNull()
                .WithMessage("拒绝解锁信息不能为空");

            RuleFor(command => command.RefuseInfo)
                .Must(info => info == null || info.BillID > 0)
                .WithMessage("单据ID必须大于0");

            RuleFor(command => command.RefuseInfo)
                .Must(info => info == null || info.RequestUserID > 0)
                .WithMessage("请求用户ID必须大于0");

            RuleFor(command => command.RefuseInfo)
                .Must(info => info == null || info.RefuseUserID > 0)
                .WithMessage("拒绝用户ID必须大于0");

            RuleFor(command => command.RefuseInfo)
                .Must(info => info == null || info.BillData != null)
                .WithMessage("单据信息不能为空");
        }
    }
}