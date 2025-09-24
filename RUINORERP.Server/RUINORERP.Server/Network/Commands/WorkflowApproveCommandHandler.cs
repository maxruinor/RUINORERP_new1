using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Commands.Workflow;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Protocol;
// using RUINORERP.Server.Network.Services; // 暂时注释，缺少IWorkflowService接口定义

namespace RUINORERP.Server.Network.Commands
{
    /// <summary>
    /// 工作流审批命令处理器 - 处理工作流审批任务
    /// </summary>
    [CommandHandler("WorkflowApproveCommandHandler", priority: 75)]
    public class WorkflowApproveCommandHandler : UnifiedCommandHandlerBase
    {
        // private readonly IWorkflowService _workflowService; // 暂时注释，缺少IWorkflowService接口定义

        /// <summary>
        /// 无参构造函数，以支持Activator.CreateInstance创建实例
        /// </summary>
        public WorkflowApproveCommandHandler() : base()
        {
            // _workflowService = Program.ServiceProvider.GetRequiredService<IWorkflowService>(); // 暂时注释，缺少IWorkflowService接口定义
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public WorkflowApproveCommandHandler(
            // IWorkflowService workflowService, // 暂时注释，缺少IWorkflowService接口定义
            ILogger<WorkflowApproveCommandHandler> logger = null) : base(logger)
        {
            // _workflowService = workflowService; // 暂时注释，缺少IWorkflowService接口定义
        }

        /// <summary>
        /// 支持的命令类型
        /// </summary>
        public override IReadOnlyList<uint> SupportedCommands => new uint[]
        {
            (uint)WorkflowCommands.WorkflowApproval
        };

        /// <summary>
        /// 处理器优先级
        /// </summary>
        public override int Priority => 75;

        /// <summary>
        /// 具体的命令处理逻辑
        /// </summary>
        protected override async Task<CommandResult> ProcessCommandAsync(ICommand command, CancellationToken cancellationToken)
        {
            try
            {
                var commandId = command.CommandIdentifier;

                if (commandId == WorkflowCommands.WorkflowApproval)
                {
                    return await HandleWorkflowApproveAsync(command, cancellationToken);
                }
                else
                {
                    return CommandResult.Failure($"不支持的命令类型: {command.CommandIdentifier}", "UNSUPPORTED_COMMAND");
                }
            }
            catch (Exception ex)
            {
                LogError($"处理工作流审批命令异常: {ex.Message}", ex);
                return CommandResult.Failure($"处理异常: {ex.Message}", "HANDLER_ERROR", ex);
            }
        }

        /// <summary>
        /// 处理工作流审批命令
        /// </summary>
        private async Task<CommandResult> HandleWorkflowApproveAsync(ICommand command, CancellationToken cancellationToken)
        {
            try
            {
                LogInfo($"处理工作流审批命令 [会话: {command.SessionID}]");

                // 解析工作流审批数据
                var approveData = ParseWorkflowApproveData(command.Packet.Body);
                if (approveData == null)
                {
                    return CommandResult.Failure("工作流审批数据格式错误", "INVALID_APPROVE_DATA");
                }

                // 暂时返回模拟结果，因为缺少IWorkflowService接口定义
                var approveResult = new WorkflowApproveResult
                {
                    IsSuccess = true,
                    Message = "工作流审批成功（模拟）"
                };

                // 创建响应数据
                var responseData = CreateWorkflowApproveResponse(approveResult);

                return CommandResult.SuccessWithResponse(
                    responseData,
                    data: new { 
                        WorkflowInstanceId = approveData.WorkflowInstanceId,
                        TaskId = approveData.TaskId,
                        Approved = approveData.Approved,
                        IsSuccess = approveResult.IsSuccess,
                        SessionId = command.SessionID
                    },
                    message: approveResult.IsSuccess ? "工作流审批成功" : "工作流审批失败"
                );
            }
            catch (Exception ex)
            {
                LogError($"处理工作流审批命令异常: {ex.Message}", ex);
                return CommandResult.Failure($"工作流审批异常: {ex.Message}", "WORKFLOW_APPROVE_ERROR", ex);
            }
        }

        /// <summary>
        /// 解析工作流审批数据
        /// </summary>
        private WorkflowApproveData ParseWorkflowApproveData(byte[] body)
        {
            try
            {
                if (body == null || body.Length == 0)
                    return null;

                var dataString = System.Text.Encoding.UTF8.GetString(body);
                var parts = dataString.Split('|');

                if (parts.Length >= 4)
                {
                    return new WorkflowApproveData
                    {
                        WorkflowInstanceId = parts[0],
                        TaskId = parts[1],
                        Approved = bool.TryParse(parts[2], out var approved) ? approved : true,
                        ApprovalComment = parts[3]
                    };
                }

                return null;
            }
            catch (Exception ex)
            {
                LogError($"解析工作流审批数据异常: {ex.Message}", ex);
                return null;
            }
        }

        /// <summary>
        /// 创建工作流审批响应
        /// </summary>
        private OriginalData CreateWorkflowApproveResponse(WorkflowApproveResult approveResult)
        {
            var responseData = $"APPROVE_RESULT|{approveResult.IsSuccess}|{approveResult.Message}";
            var data = System.Text.Encoding.UTF8.GetBytes(responseData);

            // 将完整的CommandId正确分解为Category和OperationCode
            uint commandId = (uint)WorkflowCommands.WorkflowApproval;
            byte category = (byte)(commandId & 0xFF); // 取低8位作为Category
            byte operationCode = (byte)((commandId >> 8) & 0xFF); // 取次低8位作为OperationCode

            return new OriginalData(
                category,
                new byte[] { operationCode },
                data
            );
        }
    }

    /// <summary>
    /// 工作流审批数据
    /// </summary>
    public class WorkflowApproveData
    {
        public string WorkflowInstanceId { get; set; }
        public string TaskId { get; set; }
        public bool Approved { get; set; }
        public string ApprovalComment { get; set; }
    }

    /// <summary>
    /// 工作流审批结果
    /// </summary>
    public class WorkflowApproveResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}