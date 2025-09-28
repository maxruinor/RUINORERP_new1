using System.ComponentModel;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 工作流相关命令
    /// </summary>
    public static class WorkflowCommands
    {
        #region 工作流命令 (0x04xx)
        /// <summary>
        /// 工作流提醒 - 提醒用户处理工作流任务
        /// </summary>
        public static readonly CommandId WorkflowReminder = new CommandId(CommandCategory.Workflow, (byte)(CommandCatalog.Workflow_WorkflowReminder & 0xFF));
        
        /// <summary>
        /// 工作流状态更新 - 更新工作流任务状态
        /// </summary>
        public static readonly CommandId WorkflowStatusUpdate = new CommandId(CommandCategory.Workflow, (byte)(CommandCatalog.Workflow_WorkflowStatusUpdate & 0xFF));
        
        /// <summary>
        /// 工作流审批 - 处理工作流审批任务
        /// </summary>
        public static readonly CommandId WorkflowApproval = new CommandId(CommandCategory.Workflow, (byte)(CommandCatalog.Workflow_WorkflowApproval & 0xFF));
        
        /// <summary>
        /// 工作流启动 - 启动一个新的工作流实例
        /// </summary>
        public static readonly CommandId WorkflowStart = new CommandId(CommandCategory.Workflow, (byte)(CommandCatalog.Workflow_WorkflowStart & 0xFF));
        
        /// <summary>
        /// 工作流指令 - 工作流相关指令操作
        /// </summary>
        public static readonly CommandId WorkflowCommand = new CommandId(CommandCategory.Workflow, (byte)(CommandCatalog.Workflow_WorkflowCommand & 0xFF));
        
        /// <summary>
        /// 通知审批人审批 - 通知审批人处理审批任务
        /// </summary>
        public static readonly CommandId NotifyApprover = new CommandId(CommandCategory.Workflow, (byte)(CommandCatalog.Workflow_NotifyApprover & 0xFF));
        
        /// <summary>
        /// 通知审批完成 - 通知相关人员审批已完成
        /// </summary>
        public static readonly CommandId NotifyApprovalComplete = new CommandId(CommandCategory.Workflow, (byte)(CommandCatalog.Workflow_NotifyApprovalComplete & 0xFF));
        
        /// <summary>
        /// 通知启动成功 - 通知工作流启动成功
        /// </summary>
        public static readonly CommandId NotifyStartSuccess = new CommandId(CommandCategory.Workflow, (byte)(CommandCatalog.Workflow_NotifyStartSuccess & 0xFF));
        
        /// <summary>
        /// 工作流提醒请求 - 请求工作流提醒信息
        /// </summary>
        public static readonly CommandId WorkflowReminderRequest = new CommandId(CommandCategory.Workflow, (byte)(CommandCatalog.Workflow_WorkflowReminderRequest & 0xFF));
        
        /// <summary>
        /// 工作流提醒变化 - 工作流提醒状态发生变化
        /// </summary>
        public static readonly CommandId WorkflowReminderChanged = new CommandId(CommandCategory.Workflow, (byte)(CommandCatalog.Workflow_WorkflowReminderChanged & 0xFF));
        
        /// <summary>
        /// 工作流提醒回复 - 对工作流提醒的回复
        /// </summary>
        public static readonly CommandId WorkflowReminderReply = new CommandId(CommandCategory.Workflow, (byte)(CommandCatalog.Workflow_WorkflowReminderReply & 0xFF));
        #endregion
    }
}