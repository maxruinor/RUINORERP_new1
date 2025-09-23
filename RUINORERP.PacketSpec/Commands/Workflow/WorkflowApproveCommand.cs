using RUINORERP.PacketSpec.Models.Core;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.PacketSpec.Commands.Workflow
{
    /// <summary>
    /// 工作流审批命令 - 用于处理工作流审批任务
    /// </summary>
    [PacketCommand("WorkflowApprove", CommandCategory.Workflow)]
    public class WorkflowApproveCommand : BaseCommand
    {
        /// <summary>
        /// 命令标识符
        /// </summary>
        public override CommandId CommandIdentifier => WorkflowCommands.WorkflowApproval;

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
            Direction = CommandDirection.Send;
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
            Direction = CommandDirection.Send;
        }

        /// <summary>
        /// 验证命令参数
        /// </summary>
        /// <returns>验证结果</returns>
        public override CommandValidationResult Validate()
        {
            var result = base.Validate();
            if (!result.IsValid)
            {
                return result;
            }

            // 验证工作流实例ID
            if (string.IsNullOrWhiteSpace(WorkflowInstanceId))
            {
                return CommandValidationResult.Failure("工作流实例ID不能为空", "INVALID_WORKFLOW_INSTANCE_ID");
            }

            // 验证任务ID
            if (string.IsNullOrWhiteSpace(TaskId))
            {
                return CommandValidationResult.Failure("任务ID不能为空", "INVALID_TASK_ID");
            }

            return CommandValidationResult.Success();
        }

        /// <summary>
        /// 执行命令的核心逻辑
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>命令执行结果</returns>
        protected override Task<CommandResult> OnExecuteAsync(CancellationToken cancellationToken)
        {
            // 工作流审批命令契约只定义数据结构，实际的业务逻辑在Handler中实现
            var result = CommandResult.Success(
                data: new { WorkflowInstanceId = WorkflowInstanceId, TaskId = TaskId, Approved = Approved, ApprovalComment = ApprovalComment },
                message: "工作流审批命令构建成功"
            );
            return Task.FromResult(result);
        }
    }
}