using MessagePack;
using RUINORERP.PacketSpec.Models.Requests;
using System;

namespace RUINORERP.PacketSpec.Models.Requests.Workflow
{
    /// <summary>
    /// 工作流审批请求 - 用于处理工作流审批任务
    /// </summary>
    [MessagePackObject]
    public class WorkflowApproveRequest : RequestBase
    {
        /// <summary>
        /// 工作流实例ID
        /// </summary>
        [Key(10)]
        public string WorkflowInstanceId { get; set; }

        /// <summary>
        /// 任务ID
        /// </summary>
        [Key(11)]
        public string TaskId { get; set; }

        /// <summary>
        /// 审批意见
        /// </summary>
        [Key(12)]
        public string ApprovalComment { get; set; }

        /// <summary>
        /// 审批结果
        /// </summary>
        [Key(13)]
        public bool Approved { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public WorkflowApproveRequest()
        {
            Approved = true; // 默认审批通过
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="workflowInstanceId">工作流实例ID</param>
        /// <param name="taskId">任务ID</param>
        /// <param name="approved">审批结果</param>
        /// <param name="approvalComment">审批意见</param>
        public WorkflowApproveRequest(string workflowInstanceId, string taskId, bool approved, string approvalComment = "")
        {
            WorkflowInstanceId = workflowInstanceId;
            TaskId = taskId;
            Approved = approved;
            ApprovalComment = approvalComment;
        }
    }
}