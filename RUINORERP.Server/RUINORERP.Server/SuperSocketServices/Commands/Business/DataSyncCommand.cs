using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Enums;
using RUINORERP.PacketSpec.Protocol;
using RUINORERP.Server.SuperSocketServices;
using RUINORERP.PacketSpec.Models.Responses;

namespace RUINORERP.Server.Commands
{
    /// <summary>
    /// 数据同步命令处理器
    /// </summary>
    public class DataSyncCommand : IServerCommand
    {
        private readonly ILogger<DataSyncCommand> _logger;
        private readonly ICacheManager _cacheManager;
        private readonly IDataSyncService _dataSyncService;

        public SyncOperationType SyncType { get; set; }
        public string TableName { get; set; }
        public string SyncKey { get; set; }
        public object SyncData { get; set; }
        public DateTime LastSyncTime { get; set; }

        // ICommand 接口属性
        public string CommandId { get; set; } = Guid.NewGuid().ToString();
        public CmdOperation OperationType { get; set; } = CmdOperation.Receive;
        public RUINORERP.PacketSpec.Protocol.OriginalData? DataPacket { get; set; }
        public SessionInfo SessionInfo { get; set; }

        // IServerCommand 接口属性
        public int Priority { get; set; } = 3;
        public string Description => $"数据同步: {SyncType}";
        public bool RequiresAuthentication => true;
        public int TimeoutMs { get; set; } = 45000;

        public DataSyncCommand(
            ILogger<DataSyncCommand> logger,
            ICacheManager cacheManager,
            IDataSyncService dataSyncService)
        {
            _logger = logger;
            _cacheManager = cacheManager;
            _dataSyncService = dataSyncService;
        }

        public virtual bool CanExecute()
        {
            return SessionInfo?.IsAuthenticated == true &&
                   SyncType != SyncOperationType.Unknown &&
                   !string.IsNullOrEmpty(SyncKey);
        }

        public async Task<ApiResponse> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation($"开始处理数据同步: Type={SyncType}, Key={SyncKey}, SessionId={SessionInfo?.SessionId}");

                // 验证参数
                var validationResult = ValidateParameters();
                if (!validationResult.IsValid)
                {
                    return ApiResponse.CreateError(validationResult.ErrorMessage, 400).WithMetadata("ErrorCode", "VALIDATION_FAILED");
                }

                // 根据同步类型执行相应操作
                var result = SyncType switch
                {
                    SyncOperationType.PullChanges => await HandlePullChangesAsync(),
                    SyncOperationType.PushChanges => await HandlePushChangesAsync(),
                    SyncOperationType.GetSyncStatus => await HandleGetSyncStatusAsync(),
                    SyncOperationType.ResetSync => await HandleResetSyncAsync(),
                    SyncOperationType.ValidateData => await HandleValidateDataAsync(),
                    _ => ApiResponse.CreateError("不支持的同步操作类型", 400).WithMetadata("ErrorCode", "UNSUPPORTED_SYNC_TYPE")
                };

                if (result.IsSuccess)
                {
                    _logger.LogInformation($"数据同步成功: Type={SyncType}, Key={SyncKey}");
                }
                else
                {
                    _logger.LogWarning($"数据同步失败: Type={SyncType}, Key={SyncKey}, Error={result.Message}");
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"数据同步异常: Type={SyncType}, Key={SyncKey}");
                return ApiResponse.CreateError("数据同步异常", 500)
                    .WithMetadata("ErrorCode", "SYNC_EXCEPTION")
                    .WithMetadata("Exception", ex.Message)
                    .WithMetadata("StackTrace", ex.StackTrace);
            }
        }

        public RUINORERP.PacketSpec.Commands.ValidationResult ValidateParameters()
        {
            if (SessionInfo?.IsAuthenticated != true)
                return RUINORERP.PacketSpec.Commands.ValidationResult.Failure("用户未认证");

            if (string.IsNullOrWhiteSpace(SyncKey))
                return RUINORERP.PacketSpec.Commands.ValidationResult.Failure("同步键不能为空");

            // 根据同步类型进行特定验证
            switch (SyncType)
            {
                case SyncOperationType.PushChanges:
                    if (SyncData == null)
                        return RUINORERP.PacketSpec.Commands.ValidationResult.Failure("推送数据不能为空");
                    break;

                case SyncOperationType.PullChanges:
                    if (LastSyncTime == default)
                        LastSyncTime = DateTime.MinValue; // 设置默认值表示获取所有数据
                    break;
            }

            return RUINORERP.PacketSpec.Commands.ValidationResult.Success();
        }

        public string GetCommandId()
        {
            return CommandId;
        }
        
        /// <summary>
        /// 解析数据包
        /// </summary>
        public bool AnalyzeDataPacket(RUINORERP.PacketSpec.Protocol.OriginalData data, SessionInfo sessionInfo)
        {
            try
            {
                SessionInfo = sessionInfo;
                DataPacket = data;
                
                // 这里应该根据实际协议解析数据同步信息
                // 为了简化，这里使用示例数据
                SyncType = SyncOperationType.PullChanges;
                SyncKey = "demo_sync";
                TableName = "DemoTable";
                
                return true;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "解析数据同步数据包失败");
                return false;
            }
        }
        
        /// <summary>
        /// 构建数据包
        /// </summary>
        public void BuildDataPacket(object request = null)
        {
            try
            {
                // 这里应该根据数据同步响应构建数据包
                if (request is DataSyncResponse response)
                {
                    var responseBytes = System.Text.Encoding.UTF8.GetBytes(
                        Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    DataPacket = new RUINORERP.PacketSpec.Protocol.OriginalData(0x03, responseBytes, null);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "构建数据同步响应数据包失败");
            }
        }

        /// <summary>
        /// 处理拉取变更
        /// </summary>
        private async Task<ApiResponse> HandlePullChangesAsync()
        {
            try
            {
                // 从缓存或数据库获取变更数据
                var changes = await _dataSyncService.GetChangesAsync(SyncKey, LastSyncTime, SessionInfo.UserId.ToString());
                
                // 检查是否有变更
                if (changes == null || !changes.HasChanges)
                {
                    var noChangesResponse = new DataSyncResponse
                    {
                        Success = true,
                        SyncKey = SyncKey,
                        HasChanges = false,
                        LastSyncTime = DateTime.Now,
                        Message = "没有新的变更"
                    };

                    return ApiResponse<DataSyncResponse>.CreateSuccess(noChangesResponse, "没有新的变更");
                }

                // 更新缓存中的同步状态
                await _cacheManager.SetSyncStatusAsync(SyncKey, SessionInfo.UserId.ToString(), DateTime.Now);

                var response = new DataSyncResponse
                {
                    Success = true,
                    SyncKey = SyncKey,
                    HasChanges = true,
                    Changes = changes.Data,
                    ChangeCount = changes.Count,
                    LastSyncTime = DateTime.Now,
                    Message = $"获取到 {changes.Count} 条变更"
                };

                return ApiResponse<DataSyncResponse>.CreateSuccess(response, "拉取变更成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"拉取变更处理异常: {SyncKey}");
                return ApiResponse.CreateError("拉取变更处理异常", 500)
                    .WithMetadata("ErrorCode", "PULL_CHANGES_ERROR")
                    .WithMetadata("Exception", ex.Message)
                    .WithMetadata("StackTrace", ex.StackTrace);
            }
        }

        /// <summary>
        /// 处理推送变更
        /// </summary>
        private async Task<ApiResponse> HandlePushChangesAsync()
        {
            try
            {
                // 验证推送数据
                if (SyncData == null)
                {
                    return ApiResponse.CreateError("推送数据不能为空", 400).WithMetadata("ErrorCode", "EMPTY_PUSH_DATA");
                }

                // 验证推送数据的完整性
                var validationResult = await _dataSyncService.ValidateChangesAsync(SyncData);
                if (!validationResult.IsValid)
                {
                    return ApiResponse.CreateError($"数据验证失败: {validationResult.ErrorMessage}", 400).WithMetadata("ErrorCode", "VALIDATION_FAILED");
                }

                // 执行数据推送
                var pushResult = await _dataSyncService.ApplyChangesAsync(SyncKey, SyncData, SessionInfo.UserId.ToString());
                if (!pushResult.Success)
                {
                    return ApiResponse.CreateError($"推送变更失败: {pushResult.Message}", 400).WithMetadata("ErrorCode", "PUSH_PROCESSING_FAILED");
                }

                // 更新缓存
                await _cacheManager.InvalidateCacheAsync(SyncKey);
                await _cacheManager.SetSyncStatusAsync(SyncKey, SessionInfo.UserId.ToString(), DateTime.Now);

                // 通知其他客户端数据变更
                await NotifyOtherClientsAsync(SyncKey, SessionInfo.SessionId);

                var response = new DataSyncResponse
                {
                    Success = true,
                    SyncKey = SyncKey,
                    ProcessedCount = pushResult.ProcessedCount,
                    LastSyncTime = DateTime.Now,
                    Message = $"成功推送 {pushResult.ProcessedCount} 条变更"
                };

                return ApiResponse<DataSyncResponse>.CreateSuccess(response, "推送变更成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"推送变更处理异常: {SyncKey}");
                return ApiResponse.CreateError("推送变更处理异常", 500)
                    .WithMetadata("ErrorCode", "PUSH_CHANGES_ERROR")
                    .WithMetadata("Exception", ex.Message)
                    .WithMetadata("StackTrace", ex.StackTrace);
            }
        }

        /// <summary>
        /// 处理获取同步状态
        /// </summary>
        private async Task<ApiResponse> HandleGetSyncStatusAsync()
        {
            try
            {
                var syncStatus = await _cacheManager.GetSyncStatusAsync(SyncKey, SessionInfo.UserId.ToString());

                var response = new DataSyncResponse
                {
                    Success = true,
                    SyncKey = SyncKey,
                    LastSyncTime = syncStatus?.LastSyncTime ?? DateTime.MinValue,
                    IsSyncing = syncStatus?.IsSyncing ?? false,
                    Message = syncStatus != null ? "获取同步状态成功" : "未找到同步状态"
                };

                return ApiResponse<DataSyncResponse>.CreateSuccess(response, "获取同步状态成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"获取同步状态异常: {SyncKey}");
                return ApiResponse.CreateError("获取同步状态异常", 500)
                    .WithMetadata("ErrorCode", "GET_SYNC_STATUS_ERROR")
                    .WithMetadata("Exception", ex.Message)
                    .WithMetadata("StackTrace", ex.StackTrace);
            }
        }

        /// <summary>
        /// 处理重置同步
        /// </summary>
        private async Task<ApiResponse> HandleResetSyncAsync()
        {
            try
            {
                // 重置同步状态
                var resetResult = await _dataSyncService.ResetSyncAsync(SyncKey, SessionInfo.UserId.ToString());
                if (!resetResult.Success)
                {
                    return ApiResponse.CreateError($"重置同步失败: {resetResult.Message}", 400).WithMetadata("ErrorCode", "RESET_SYNC_FAILED");
                }

                // 清除相关缓存
                await _cacheManager.InvalidateCacheAsync(SyncKey);

                var response = new DataSyncResponse
                {
                    Success = true,
                    SyncKey = SyncKey,
                    LastSyncTime = DateTime.Now,
                    Message = "同步重置成功"
                };

                return ApiResponse<DataSyncResponse>.CreateSuccess(response, "同步重置成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"重置同步异常: {SyncKey}");
                return ApiResponse.CreateError("重置同步异常", 500)
                    .WithMetadata("ErrorCode", "RESET_SYNC_ERROR")
                    .WithMetadata("Exception", ex.Message)
                    .WithMetadata("StackTrace", ex.StackTrace);
            }
        }

        /// <summary>
        /// 处理数据验证
        /// </summary>
        private async Task<ApiResponse> HandleValidateDataAsync()
        {
            try
            {
                var validationResult = await _dataSyncService.ValidateDataIntegrityAsync(SyncKey, SessionInfo.UserId.ToString());
                
                var response = new DataValidationResponse
                {
                    Success = true,
                    SyncKey = SyncKey,
                    IsValid = validationResult.IsValid,
                    ValidationErrors = validationResult.Errors,
                    ChecksumMatch = validationResult.ChecksumMatch,
                    Message = validationResult.IsValid ? "数据验证通过" : "数据验证失败"
                };

                return ApiResponse<DataValidationResponse>.CreateSuccess(response, "数据验证完成");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"数据验证异常: {SyncKey}");
                return ApiResponse.CreateError("数据验证异常", 500)
                    .WithMetadata("ErrorCode", "VALIDATE_DATA_ERROR")
                    .WithMetadata("Exception", ex.Message)
                    .WithMetadata("StackTrace", ex.StackTrace);
            }
        }

        /// <summary>
        /// 通知其他客户端数据变更
        /// </summary>
        private async Task NotifyOtherClientsAsync(string syncKey, string excludeSessionId)
        {
            try
            {
                // 这里可以通过消息队列或其他方式通知其他客户端
                _logger.LogDebug($"通知其他客户端数据变更: SyncKey={syncKey}, ExcludeSession={excludeSessionId}");
                
                // 实际实现可能需要调用通知服务
                // await _notificationService.NotifyDataChangedAsync(syncKey, excludeSessionId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"通知其他客户端失败: {syncKey}");
            }

            await Task.CompletedTask;
        }
    }

    /// <summary>
    /// 同步操作类型
    /// </summary>
    public enum SyncOperationType
    {
        Unknown = 0,
        PullChanges = 1,
        PushChanges = 2,
        GetSyncStatus = 3,
        ResetSync = 4,
        ValidateData = 5
    }

    /// <summary>
    /// 数据同步响应
    /// </summary>
    public class DataSyncResponse
    {
        public bool Success { get; set; }
        public string SyncKey { get; set; }
        public bool HasChanges { get; set; }
        public object Changes { get; set; }
        public int ChangeCount { get; set; }
        public int ProcessedCount { get; set; }
        public DateTime LastSyncTime { get; set; }
        public string Message { get; set; }
    }

    /// <summary>
    /// 同步状态响应
    /// </summary>
    public class SyncStatusResponse
    {
        public bool Success { get; set; }
        public string SyncKey { get; set; }
        public DateTime LastSyncTime { get; set; }
        public bool IsSyncing { get; set; }
        public int PendingChanges { get; set; }
        public string SyncVersion { get; set; }
        public string Message { get; set; }
    }

    /// <summary>
    /// 数据验证响应
    /// </summary>
    public class DataValidationResponse
    {
        public bool Success { get; set; }
        public string SyncKey { get; set; }
        public bool IsValid { get; set; }
        public string[] ValidationErrors { get; set; }
        public bool ChecksumMatch { get; set; }
        public string Message { get; set; }
    }

    /// <summary>
    /// 缓存管理器接口（临时定义）
    /// </summary>
    public interface ICacheManager
    {
        Task SetSyncStatusAsync(string syncKey, string userId, DateTime lastSyncTime);
        Task InvalidateCacheAsync(string syncKey);
    }

    /// <summary>
    /// 数据同步服务接口（临时定义）
    /// </summary>
    public interface IDataSyncService
    {
        Task<SyncChanges> GetChangesAsync(string syncKey, DateTime lastSyncTime, string userId);
        Task<ValidationResult> ValidateChangesAsync(object data);
        Task<SyncApplyResult> ApplyChangesAsync(string syncKey, object data, string userId);
        Task<SyncStatusInfo> GetSyncStatusAsync(string syncKey, string userId);
        Task<SyncApplyResult> ResetSyncAsync(string syncKey, string userId);
        Task<DataValidationResult> ValidateDataIntegrityAsync(string syncKey, string userId);
    }

    /// <summary>
    /// 同步变更数据
    /// </summary>
    public class SyncChanges
    {
        public bool HasChanges { get; set; }
        public object Data { get; set; }
        public int Count { get; set; }
    }

    /// <summary>
    /// 同步应用结果
    /// </summary>
    public class SyncApplyResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int ProcessedCount { get; set; }
    }

    /// <summary>
    /// 同步状态信息
    /// </summary>
    public class SyncStatusInfo
    {
        public DateTime LastSyncTime { get; set; }
        public bool IsSyncing { get; set; }
        public int PendingChanges { get; set; }
        public string Version { get; set; }
    }

    /// <summary>
    /// 数据验证结果
    /// </summary>
    public class DataValidationResult
    {
        public bool IsValid { get; set; }
        public string[] Errors { get; set; }
        public bool ChecksumMatch { get; set; }
    }
}