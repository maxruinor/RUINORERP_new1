using RUINORERP.PacketSpec.Validation;
using FluentValidation;

namespace RUINORERP.PacketSpec.Commands.Lock
{
    /// <summary>
    /// 查询锁状态命令验证器
    /// </summary>
    public class QueryLockStatusCommandValidator : CommandValidator<QueryLockStatusCommand>
    {
        public QueryLockStatusCommandValidator()
        {
            // 查询锁状态命令的验证规则
            RuleFor(command => command.BillId)
                .GreaterThan(0L)
                .WithMessage("单据ID必须大于0");
        }
    }
}
