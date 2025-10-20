using MessagePack;
using RUINORERP.PacketSpec.Models.Responses;
using System;

namespace RUINORERP.PacketSpec.Models.Responses.Workflow
{
    /// <summary>
    /// 工作流审批响应 - 表示工作流审批操作的结果
    /// </summary>
    [MessagePackObject]
    public class WorkflowApproveResponse : ResponseBase
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
        /// 审批结果
        /// </summary>
        [Key(12)]
        public bool Approved { get; set; }

        /// <summary>
        /// 新的工作流状态
        /// </summary>
        [Key(13)]
        public string NewWorkflowState { get; set; }

        /// <summary>
        /// 下一审批人ID（如果有）
        /// </summary>
        [Key(14)]
        public string NextApproverId { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public WorkflowApproveResponse()
        {}

        /// <summary>
        /// 创建成功响应
        /// </summary>
        /// <param name="workflowInstanceId">工作流实例ID</param>
        /// <param name="taskId">任务ID</param>
        /// <param name="approved">审批结果</param>
        /// <param name="newWorkflowState">新的工作流状态</param>
        /// <returns>成功的响应实例</returns>
        public static WorkflowApproveResponse Success(string workflowInstanceId, string taskId, bool approved, string newWorkflowState)
        {
            return new WorkflowApproveResponse
            {
                IsSuccess = true,
                ErrorCode = 0,
                Message = approved ? "审批通过" : "审批拒绝",
                WorkflowInstanceId = workflowInstanceId,
                TaskId = taskId,
                Approved = approved,
                NewWorkflowState = newWorkflowState
            };
        }

        /// <summary>
        /// 创建失败响应
        /// </summary>
        /// <param name="errorCode">错误码</param>
        /// <param name="errorMessage">错误消息</param>
        /// <returns>失败的响应实例</returns>
        public static WorkflowApproveResponse Fail(int errorCode, string errorMessage)
        {
            return new WorkflowApproveResponse
            {
                IsSuccess = false,
                ErrorCode = errorCode,
                ErrorMessage = errorMessage,
                Message = "审批操作失败"
            };
        }
    }
}