using FluentValidation;
using RUINORERP.PacketSpec.Validation;

namespace RUINORERP.PacketSpec.Commands.Workflow
{
    /// <summary>
    /// 工作流审批命令验证器
    /// </summary>
    public class WorkflowApproveCommandValidator : CommandValidator<WorkflowApproveCommand>
    {
        public WorkflowApproveCommandValidator()
        {
            // 添加工作流审批命令特定的验证规则
            RuleFor(command => command.WorkflowInstanceId)
                .NotEmpty()
                .WithMessage("工作流实例ID不能为空");

            RuleFor(command => command.TaskId)
                .NotEmpty()
                .WithMessage("任务ID不能为空");
        }
    }
}
