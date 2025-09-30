using RUINORERP.PacketSpec.Validation;
using FluentValidation;

namespace RUINORERP.PacketSpec.Commands.Lock
{
    /// <summary>
    /// 申请锁定单据命令验证器
    /// </summary>
    public class DocumentLockApplyCommandValidator : CommandValidator<DocumentLockApplyCommand>
    {
        public DocumentLockApplyCommandValidator()
        {
            // 添加申请锁定单据命令特定的验证规则
            RuleFor(command => command.BillId)
                .GreaterThan(0)
                .WithMessage("单据ID必须大于0");

            RuleFor(command => command.BillData)
                .NotNull()
                .WithMessage("单据信息不能为空");

            RuleFor(command => command.BillData)
                .Must(billData => billData == null || !string.IsNullOrWhiteSpace(billData.BizName))
                .WithMessage("业务名称不能为空");

            RuleFor(command => command.BillData)
                .Must(billData => billData == null || !string.IsNullOrWhiteSpace(billData.BillNo))
                .WithMessage("单据编号不能为空");
        }
    }
}