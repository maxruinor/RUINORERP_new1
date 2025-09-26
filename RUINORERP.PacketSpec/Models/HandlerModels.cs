using System;
using System.Collections.Generic;
using RUINORERP.Model;
using RUINORERP.PacketSpec.Models.Responses;

namespace RUINORERP.PacketSpec.Handlers
{
    // 系统操作枚举
    public enum SystemOperation
    {
        GetSystemInfo,
        GetServerStatus,
        GetConfiguration,
        UpdateConfiguration,
        RestartService,
        GetLogs,
        ClearCache,
        BackupDatabase,
        GetPerformanceMetrics,
        GetActiveConnections,
        KillConnection,
        MaintenanceMode
    }

    // 工作流相关枚举
    public enum WorkflowAction
    {
        StartWorkflow,
        CompleteTask,
        GetWorkflowStatus,
        GetPendingTasks,
        CancelWorkflow,
        ApproveTask,
        RejectTask,
        DelegateTask
    }

    public enum WorkflowStatus
    {
        Running,
        Completed,
        Cancelled,
        Failed,
        Suspended
    }

    public enum TaskType
    {
        Manual,
        Approval,
        System,
        Review
    }

    public enum TaskStatus
    {
        Pending,
        InProgress,
        Completed,
        Cancelled,
        Delegated
    }

    public enum TaskDecision
    {
        Approved,
        Rejected
    }

    public enum WorkflowResponseStatus
    {
        Success,
        Error,
        Warning
    }

    public enum SystemResponseStatus
    {
        Success,
        Error,
        Warning
    }

    // 数据同步相关枚举
    public enum SyncType
    {
        FullSync,
        IncrementalSync,
        ConflictResolution
    }

    public enum SyncStatus
    {
        Success,
        Error,
        PartialSuccess
    }

    public enum SyncOperation
    {
        FullReplace,
        IncrementalUpdate,
        Update,
        Delete,
        ConflictError
    }

    public enum ConflictResolutionStrategy
    {
        ServerWins,
        ClientWins,
        Merge
    }

 

    public class WorkflowInstance
    {
        public string Id { get; set; }
        public string WorkflowType { get; set; }
        public string InitiatorId { get; set; }
        public WorkflowStatus Status { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public object Data { get; set; }
        public string CurrentStep { get; set; }
    }

    public class WorkflowTask
    {
        public string Id { get; set; }
        public string WorkflowInstanceId { get; set; }
        public string TaskName { get; set; }
        public TaskType TaskType { get; set; }
        public TaskStatus Status { get; set; }
        public string AssignedToUserId { get; set; }
        public string RequiredRole { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public object TaskData { get; set; }
    }

    public class WorkflowDefinition
    {
        public string Id { get; set; }
        public List<WorkflowStep> Steps { get; set; }
    }

    public class WorkflowStep
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class WorkflowHistoryEntry
    {
        public string Id { get; set; }
        public string WorkflowInstanceId { get; set; }
        public string Action { get; set; }
        public string UserId { get; set; }
        public DateTime Timestamp { get; set; }
        public string Comments { get; set; }
        public object Data { get; set; }
    }

    // 请求响应模型
    public class AuthenticationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public tb_UserInfo User { get; set; }
        public string Token { get; set; }
        public List<tb_RoleInfo> Roles { get; set; }
        public List<tb_User_Role> Permissions { get; set; }
    }

    public class CreateUserRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<string> RoleIds { get; set; }
        public bool MustChangePassword { get; set; }
        public string CreatedBy { get; set; }
    }

    public class UpdateUserRequest
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsLocked { get; set; }
        public string ModifiedBy { get; set; }
    }

    public class ChangePasswordRequest
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }

    public class UserSearchCriteria
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsLocked { get; set; }
        public int PageSize { get; set; } = 50;
        public int PageNumber { get; set; } = 1;
    }

    // 系统相关模型
    public class DatabaseStatus
    {
        public bool IsConnected { get; set; }
        public string ConnectionString { get; set; }
        public int ActiveConnections { get; set; }
        public long DatabaseSize { get; set; }
        public DateTime LastBackup { get; set; }
    }

    public class ServiceStatus
    {
        public string ServiceName { get; set; }
        public string Status { get; set; }
        public DateTime LastStarted { get; set; }
        public bool IsHealthy { get; set; }
    }

    /// <summary>
    /// 操作结果 - 使用统一的ApiResponse模式
    /// </summary>
    public class OperationResult : ResponseBase<object>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public OperationResult() : base() { }

        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        public OperationResult(bool success, string message, object data = null, int code = 200) 
        {
            this.IsSuccess = success;
            this.Message = message;
            this.Data = data;
            this.Code = code;
            this.Timestamp = DateTime.UtcNow;
        }

        /// <summary>
        /// 创建成功结果
        /// </summary>
        public static OperationResult CreateSuccess(object data = null, string message = "操作成功")
        {
            return new OperationResult(true, message, data, 200);
        }

        /// <summary>
        /// 创建失败结果
        /// </summary>
        public static OperationResult CreateFailure(string message, int code = 500)
        {
            return new OperationResult(false, message, null, code);
        }
    }

    public class CacheResult
    {
        public int ItemsCleared { get; set; }
        public long MemoryFreed { get; set; }
        public TimeSpan Duration { get; set; }
    }

    public class LogRequest
    {
        public string LogLevel { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int MaxRecords { get; set; }
        public string SearchTerm { get; set; }
    }

    public class BackupOptions
    {
        public string BackupPath { get; set; }
        public bool CompressBackup { get; set; }
        public bool IncludeIndexes { get; set; }
        public string BackupName { get; set; }
    }

    public class CpuUsage
    {
        public double CurrentUsage { get; set; }
        public double AverageUsage { get; set; }
        public int ProcessorCount { get; set; }
    }

    public class DiskUsage
    {
        public long TotalSpace { get; set; }
        public long FreeSpace { get; set; }
        public long UsedSpace { get; set; }
        public double UsagePercentage { get; set; }
    }

    public class NetworkUsage
    {
        public long BytesSent { get; set; }
        public long BytesReceived { get; set; }
        public double SendRate { get; set; }
        public double ReceiveRate { get; set; }
    }
}
