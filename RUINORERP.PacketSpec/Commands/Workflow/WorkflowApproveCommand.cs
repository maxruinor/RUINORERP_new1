using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.PacketSpec.Enums.Core;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;

namespace RUINORERP.PacketSpec.Commands.Workflow
{
    /// <summary>
    /// 工作流审批命令 - 用于处理工作流审批任务
    /// </summary>
    [PacketCommand("WorkflowApprove", CommandCategory.Workflow)]
    public class WorkflowApproveCommand : BaseCommand
    {


        /// <summary>
        /// 工作流实例ID
        /// </summary>
        public string WorkflowInstanceId { get; set; }

        /// <summary>
        /// 任务ID
        /// </summary>
        public string TaskId { get; set; }

        /// <summary>
        /// 审批意见
        /// </summary>
        public string ApprovalComment { get; set; }

        /// <summary>
        /// 审批结果
        /// </summary>
        public bool Approved { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public WorkflowApproveCommand()
        {
            Approved = true; // 默认审批通过
            Direction = PacketDirection.ClientToServer;
            CommandIdentifier = WorkflowCommands.WorkflowApproval;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="workflowInstanceId">工作流实例ID</param>
        /// <param name="taskId">任务ID</param>
        /// <param name="approved">审批结果</param>
        /// <param name="approvalComment">审批意见</param>
        public WorkflowApproveCommand(string workflowInstanceId, string taskId, bool approved, string approvalComment = "")
        {
            WorkflowInstanceId = workflowInstanceId;
            TaskId = taskId;
            Approved = approved;
            ApprovalComment = approvalComment;
            Direction = PacketDirection.ClientToServer;
            CommandIdentifier = WorkflowCommands.WorkflowApproval;
        }

        /// <summary>
        /// 验证命令参数
        /// </summary>
        /// <returns>验证结果</returns>
        public override async Task<ValidationResult> ValidateAsync(CancellationToken cancellationToken = default)
        {
            // 调用基类验证方法，将使用独立的验证器类进行验证
            var result = await base.ValidateAsync(cancellationToken);
            return result;
        }

       
    }
}
