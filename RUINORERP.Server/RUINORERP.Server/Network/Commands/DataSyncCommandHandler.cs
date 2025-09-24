using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Protocol;
// using RUINORERP.Server.Network.Services; // 暂时注释，缺少IDataSyncService接口定义

namespace RUINORERP.Server.Network.Commands
{
    /// <summary>
    /// 数据同步命令处理器 - 处理数据同步相关的命令
    /// </summary>
    [CommandHandler("DataSyncCommandHandler", priority: 65)]
    public class DataSyncCommandHandler : UnifiedCommandHandlerBase
    {
        // private readonly IDataSyncService _dataSyncService; // 暂时注释，缺少IDataSyncService接口定义

        /// <summary>
        /// 无参构造函数，以支持Activator.CreateInstance创建实例
        /// </summary>
        public DataSyncCommandHandler() : base()
        {
            // _dataSyncService = Program.ServiceProvider.GetRequiredService<IDataSyncService>(); // 暂时注释，缺少IDataSyncService接口定义
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public DataSyncCommandHandler(
            // IDataSyncService dataSyncService, // 暂时注释，缺少IDataSyncService接口定义
            ILogger<DataSyncCommandHandler> logger = null) : base(logger)
        {
            // _dataSyncService = dataSyncService; // 暂时注释，缺少IDataSyncService接口定义
        }

        /// <summary>
        /// 支持的命令类型
        /// </summary>
        public override IReadOnlyList<uint> SupportedCommands => new uint[]
        {
            (uint)DataSyncCommands.DataRequest,
            (uint)DataSyncCommands.FullSync,
            (uint)DataSyncCommands.IncrementalSync,
            (uint)DataSyncCommands.SyncStatus,
            (uint)DataSyncCommands.DataSyncRequest,
            (uint)DataSyncCommands.EntityDataTransfer,
            (uint)DataSyncCommands.UpdateDynamicConfig,
            (uint)DataSyncCommands.ForwardUpdateDynamicConfig,
            (uint)DataSyncCommands.UpdateGlobalConfig
        };

        /// <summary>
        /// 处理器优先级
        /// </summary>
        public override int Priority => 65;

        /// <summary>
        /// 具体的命令处理逻辑
        /// </summary>
        protected override async Task<CommandResult> ProcessCommandAsync(ICommand command, CancellationToken cancellationToken)
        {
            try
            {
                var commandId = command.CommandIdentifier;

                if (commandId == DataSyncCommands.DataRequest)
                {
                    return await HandleDataRequestAsync(command, cancellationToken);
                }
                else if (commandId == DataSyncCommands.FullSync)
                {
                    return await HandleFullSyncAsync(command, cancellationToken);
                }
                else if (commandId == DataSyncCommands.IncrementalSync)
                {
                    return await HandleIncrementalSyncAsync(command, cancellationToken);
                }
                else if (commandId == DataSyncCommands.SyncStatus)
                {
                    return await HandleSyncStatusAsync(command, cancellationToken);
                }
                else if (commandId == DataSyncCommands.DataSyncRequest)
                {
                    return await HandleDataSyncRequestAsync(command, cancellationToken);
                }
                else if (commandId == DataSyncCommands.EntityDataTransfer)
                {
                    return await HandleEntityDataTransferAsync(command, cancellationToken);
                }
                else if (commandId == DataSyncCommands.UpdateDynamicConfig)
                {
                    return await HandleUpdateDynamicConfigAsync(command, cancellationToken);
                }
                else if (commandId == DataSyncCommands.ForwardUpdateDynamicConfig)
                {
                    return await HandleForwardUpdateDynamicConfigAsync(command, cancellationToken);
                }
                else if (commandId == DataSyncCommands.UpdateGlobalConfig)
                {
                    return await HandleUpdateGlobalConfigAsync(command, cancellationToken);
                }
                else
                {
                    return CommandResult.Failure($"不支持的命令类型: {command.CommandIdentifier}", "UNSUPPORTED_COMMAND");
                }
            }
            catch (Exception ex)
            {
                LogError($"处理数据同步命令异常: {ex.Message}", ex);
                return CommandResult.Failure($"处理异常: {ex.Message}", "HANDLER_ERROR", ex);
            }
        }

        /// <summary>
        /// 处理数据请求命令
        /// </summary>
        private async Task<CommandResult> HandleDataRequestAsync(ICommand command, CancellationToken cancellationToken)
        {
            try
            {
                LogInfo($"处理数据请求命令 [会话: {command.SessionID}]");

                // 解析数据请求
                var requestData = command.Packet.GetJsonData<DataRequest>();
                
                // 暂时返回模拟结果，因为缺少IDataSyncService接口定义
                var requestResult = new DataSyncOperationResult
                {
                    IsSuccess = true,
                    RecordCount = 0,
                    Message = "数据请求成功（模拟）"
                };

                // 创建响应数据
                var responseData = CreateDataSyncResponse("DATA_REQUEST", requestResult);

                return CommandResult.SuccessWithResponse(
                    responseData,
                    data: new { 
                        DataType = requestData.DataType,
                        Filter = requestData.Filter,
                        RecordCount = requestResult.RecordCount,
                        IsSuccess = requestResult.IsSuccess,
                        SessionId = command.SessionID
                    },
                    message: requestResult.IsSuccess ? "数据请求成功（模拟）" : "数据请求失败（模拟）"
                );
            }
            catch (Exception ex)
            {
                LogError($"处理数据请求命令异常: {ex.Message}", ex);
                return CommandResult.Failure($"数据请求异常: {ex.Message}", "DATA_REQUEST_ERROR", ex);
            }
        }

        /// <summary>
        /// 处理全量同步命令
        /// </summary>
        private async Task<CommandResult> HandleFullSyncAsync(ICommand command, CancellationToken cancellationToken)
        {
            try
            {
                LogInfo($"处理全量同步命令 [会话: {command.SessionID}]");

                // 解析全量同步请求
                var syncData = command.Packet.GetJsonData<FullSyncRequest>();
                
                // 暂时返回模拟结果，因为缺少IDataSyncService接口定义
                var syncResult = new DataSyncOperationResult
                {
                    IsSuccess = true,
                    SyncedCount = 0,
                    Message = "全量同步成功（模拟）"
                };

                // 创建响应数据
                var responseData = CreateDataSyncResponse("FULL_SYNC", syncResult);

                return CommandResult.SuccessWithResponse(
                    responseData,
                    data: new { 
                        DataType = syncData.DataType,
                        TargetCount = syncData.TargetClients?.Length ?? 0,
                        SyncedCount = syncResult.SyncedCount,
                        IsSuccess = syncResult.IsSuccess,
                        SessionId = command.SessionID
                    },
                    message: syncResult.IsSuccess ? "全量同步成功（模拟）" : "全量同步失败（模拟）"
                );
            }
            catch (Exception ex)
            {
                LogError($"处理全量同步命令异常: {ex.Message}", ex);
                return CommandResult.Failure($"全量同步异常: {ex.Message}", "FULL_SYNC_ERROR", ex);
            }
        }

        /// <summary>
        /// 处理增量同步命令
        /// </summary>
        private async Task<CommandResult> HandleIncrementalSyncAsync(ICommand command, CancellationToken cancellationToken)
        {
            try
            {
                LogInfo($"处理增量同步命令 [会话: {command.SessionID}]");

                // 解析增量同步请求
                var syncData = command.Packet.GetJsonData<IncrementalSyncRequest>();
                
                // 暂时返回模拟结果，因为缺少IDataSyncService接口定义
                var syncResult = new DataSyncOperationResult
                {
                    IsSuccess = true,
                    SyncedCount = 0,
                    Message = "增量同步成功（模拟）"
                };

                // 创建响应数据
                var responseData = CreateDataSyncResponse("INCREMENTAL_SYNC", syncResult);

                return CommandResult.SuccessWithResponse(
                    responseData,
                    data: new { 
                        DataType = syncData.DataType,
                        LastSyncTime = syncData.LastSyncTime,
                        SyncedCount = syncResult.SyncedCount,
                        IsSuccess = syncResult.IsSuccess,
                        SessionId = command.SessionID
                    },
                    message: syncResult.IsSuccess ? "增量同步成功（模拟）" : "增量同步失败（模拟）"
                );
            }
            catch (Exception ex)
            {
                LogError($"处理增量同步命令异常: {ex.Message}", ex);
                return CommandResult.Failure($"增量同步异常: {ex.Message}", "INCREMENTAL_SYNC_ERROR", ex);
            }
        }

        /// <summary>
        /// 处理同步状态查询命令
        /// </summary>
        private async Task<CommandResult> HandleSyncStatusAsync(ICommand command, CancellationToken cancellationToken)
        {
            try
            {
                LogInfo($"处理同步状态查询命令 [会话: {command.SessionID}]");

                // 暂时返回模拟结果，因为缺少IDataSyncService接口定义
                var statusResult = new DataSyncOperationResult
                {
                    IsSuccess = true,
                    Status = "Synced",
                    LastSyncTime = DateTime.Now,
                    Message = "同步状态查询成功（模拟）"
                };

                // 创建响应数据
                var responseData = CreateDataSyncResponse("SYNC_STATUS", statusResult);

                return CommandResult.SuccessWithResponse(
                    responseData,
                    data: new { 
                        Status = statusResult.Status,
                        LastSyncTime = statusResult.LastSyncTime,
                        IsSuccess = statusResult.IsSuccess,
                        SessionId = command.SessionID
                    },
                    message: statusResult.IsSuccess ? "同步状态查询成功（模拟）" : "同步状态查询失败（模拟）"
                );
            }
            catch (Exception ex)
            {
                LogError($"处理同步状态查询命令异常: {ex.Message}", ex);
                return CommandResult.Failure($"同步状态查询异常: {ex.Message}", "SYNC_STATUS_ERROR", ex);
            }
        }

        /// <summary>
        /// 处理数据同步请求命令
        /// </summary>
        private async Task<CommandResult> HandleDataSyncRequestAsync(ICommand command, CancellationToken cancellationToken)
        {
            try
            {
                LogInfo($"处理数据同步请求命令 [会话: {command.SessionID}]");

                // 解析数据同步请求
                var syncData = ParseDataSyncRequest(command.Packet.Body);
                
                // 暂时返回模拟结果，因为缺少IDataSyncService接口定义
                var syncResult = new DataSyncOperationResult
                {
                    IsSuccess = true,
                    SyncedCount = 0,
                    Message = "数据同步成功（模拟）"
                };

                // 创建响应数据
                var responseData = CreateDataSyncResponse("DATA_SYNC", syncResult);

                return CommandResult.SuccessWithResponse(
                    responseData,
                    data: new { 
                        DataType = syncData.DataType,
                        SyncMode = syncData.SyncMode,
                        SyncedCount = syncResult.SyncedCount,
                        IsSuccess = syncResult.IsSuccess,
                        SessionId = command.SessionID
                    },
                    message: syncResult.IsSuccess ? "数据同步成功（模拟）" : "数据同步失败（模拟）"
                );
            }
            catch (Exception ex)
            {
                LogError($"处理数据同步请求命令异常: {ex.Message}", ex);
                return CommandResult.Failure($"数据同步异常: {ex.Message}", "DATA_SYNC_ERROR", ex);
            }
        }

        /// <summary>
        /// 处理实体数据传输命令
        /// </summary>
        private async Task<CommandResult> HandleEntityDataTransferAsync(ICommand command, CancellationToken cancellationToken)
        {
            try
            {
                LogInfo($"处理实体数据传输命令 [会话: {command.SessionID}]");

                // 解析实体数据传输请求
                var transferData = command.Packet.GetJsonData<EntityDataTransferRequest>();
                
                // 暂时返回模拟结果，因为缺少IDataSyncService接口定义
                var transferResult = new DataSyncOperationResult
                {
                    IsSuccess = true,
                    TransferredCount = 0,
                    Message = "实体数据传输成功（模拟）"
                };

                // 创建响应数据
                var responseData = CreateDataSyncResponse("ENTITY_TRANSFER", transferResult);

                return CommandResult.SuccessWithResponse(
                    responseData,
                    data: new { 
                        EntityType = transferData.EntityType,
                        RecordCount = transferData.EntityData?.Length ?? 0,
                        TransferredCount = transferResult.TransferredCount,
                        IsSuccess = transferResult.IsSuccess,
                        SessionId = command.SessionID
                    },
                    message: transferResult.IsSuccess ? "实体数据传输成功（模拟）" : "实体数据传输失败（模拟）"
                );
            }
            catch (Exception ex)
            {
                LogError($"处理实体数据传输命令异常: {ex.Message}", ex);
                return CommandResult.Failure($"实体数据传输异常: {ex.Message}", "ENTITY_TRANSFER_ERROR", ex);
            }
        }

        /// <summary>
        /// 处理更新动态配置命令
        /// </summary>
        private async Task<CommandResult> HandleUpdateDynamicConfigAsync(ICommand command, CancellationToken cancellationToken)
        {
            try
            {
                LogInfo($"处理更新动态配置命令 [会话: {command.SessionID}]");

                // 解析动态配置更新请求
                var configData = command.Packet.GetJsonData<DynamicConfigUpdateRequest>();
                
                // 暂时返回模拟结果，因为缺少IDataSyncService接口定义
                var updateResult = new DataSyncOperationResult
                {
                    IsSuccess = true,
                    Message = "动态配置更新成功（模拟）"
                };

                // 创建响应数据
                var responseData = CreateDataSyncResponse("DYNAMIC_CONFIG", updateResult);

                return CommandResult.SuccessWithResponse(
                    responseData,
                    data: new { 
                        ConfigKey = configData.ConfigKey,
                        IsSuccess = updateResult.IsSuccess,
                        SessionId = command.SessionID
                    },
                    message: updateResult.IsSuccess ? "动态配置更新成功（模拟）" : "动态配置更新失败（模拟）"
                );
            }
            catch (Exception ex)
            {
                LogError($"处理更新动态配置命令异常: {ex.Message}", ex);
                return CommandResult.Failure($"动态配置更新异常: {ex.Message}", "DYNAMIC_CONFIG_ERROR", ex);
            }
        }

        /// <summary>
        /// 处理转发动态配置更新命令
        /// </summary>
        private async Task<CommandResult> HandleForwardUpdateDynamicConfigAsync(ICommand command, CancellationToken cancellationToken)
        {
            try
            {
                LogInfo($"处理转发动态配置更新命令 [会话: {command.SessionID}]");

                // 解析转发动态配置更新请求
                var forwardData = command.Packet.GetJsonData<ForwardDynamicConfigUpdateRequest>();
                
                // 暂时返回模拟结果，因为缺少IDataSyncService接口定义
                var forwardResult = new DataSyncOperationResult
                {
                    IsSuccess = true,
                    ForwardedCount = 0,
                    Message = "动态配置转发成功（模拟）"
                };

                // 创建响应数据
                var responseData = CreateDataSyncResponse("FORWARD_CONFIG", forwardResult);

                return CommandResult.SuccessWithResponse(
                    responseData,
                    data: new { 
                        ConfigKey = forwardData.ConfigKey,
                        TargetCount = forwardData.TargetClients?.Length ?? 0,
                        ForwardedCount = forwardResult.ForwardedCount,
                        IsSuccess = forwardResult.IsSuccess,
                        SessionId = command.SessionID
                    },
                    message: forwardResult.IsSuccess ? "动态配置转发成功（模拟）" : "动态配置转发失败（模拟）"
                );
            }
            catch (Exception ex)
            {
                LogError($"处理转发动态配置更新命令异常: {ex.Message}", ex);
                return CommandResult.Failure($"动态配置转发异常: {ex.Message}", "FORWARD_CONFIG_ERROR", ex);
            }
        }

        /// <summary>
        /// 处理更新全局配置命令
        /// </summary>
        private async Task<CommandResult> HandleUpdateGlobalConfigAsync(ICommand command, CancellationToken cancellationToken)
        {
            try
            {
                LogInfo($"处理更新全局配置命令 [会话: {command.SessionID}]");

                // 解析全局配置更新请求
                var configData = command.Packet.GetJsonData<GlobalConfigUpdateRequest>();
                
                // 暂时返回模拟结果，因为缺少IDataSyncService接口定义
                var updateResult = new DataSyncOperationResult
                {
                    IsSuccess = true,
                    Message = "全局配置更新成功（模拟）"
                };

                // 创建响应数据
                var responseData = CreateDataSyncResponse("GLOBAL_CONFIG", updateResult);

                return CommandResult.SuccessWithResponse(
                    responseData,
                    data: new { 
                        ConfigKey = configData.ConfigKey,
                        IsSuccess = updateResult.IsSuccess,
                        SessionId = command.SessionID
                    },
                    message: updateResult.IsSuccess ? "全局配置更新成功（模拟）" : "全局配置更新失败（模拟）"
                );
            }
            catch (Exception ex)
            {
                LogError($"处理更新全局配置命令异常: {ex.Message}", ex);
                return CommandResult.Failure($"全局配置更新异常: {ex.Message}", "GLOBAL_CONFIG_ERROR", ex);
            }
        }

        /// <summary>
        /// 解析数据请求
        /// </summary>
        private DataRequest ParseDataRequest(OriginalData originalData)
        {
            try
            {
                if (originalData.One == null || originalData.One.Length == 0)
                    return new DataRequest();

                var dataString = System.Text.Encoding.UTF8.GetString(originalData.One);
                var parts = dataString.Split('|');

                return new DataRequest
                {
                    DataType = parts.Length > 0 ? parts[0] : string.Empty,
                    Filter = parts.Length > 1 ? parts[1] : string.Empty
                };
            }
            catch (Exception ex)
            {
                LogError($"解析数据请求异常: {ex.Message}", ex);
                return new DataRequest();
            }
        }

        /// <summary>
        /// 解析全量同步请求
        /// </summary>
        private FullSyncRequest ParseFullSyncRequest(OriginalData originalData)
        {
            try
            {
                if (originalData.One == null || originalData.One.Length == 0)
                    return new FullSyncRequest();

                var dataString = System.Text.Encoding.UTF8.GetString(originalData.One);
                var parts = dataString.Split('|');

                return new FullSyncRequest
                {
                    DataType = parts.Length > 0 ? parts[0] : string.Empty,
                    TargetClients = parts.Length > 1 ? parts[1].Split(',') : new string[0]
                };
            }
            catch (Exception ex)
            {
                LogError($"解析全量同步请求异常: {ex.Message}", ex);
                return new FullSyncRequest();
            }
        }

        /// <summary>
        /// 解析增量同步请求
        /// </summary>
        private IncrementalSyncRequest ParseIncrementalSyncRequest(OriginalData originalData)
        {
            try
            {
                if (originalData.One == null || originalData.One.Length == 0)
                    return new IncrementalSyncRequest();

                var dataString = System.Text.Encoding.UTF8.GetString(originalData.One);
                var parts = dataString.Split('|');

                return new IncrementalSyncRequest
                {
                    DataType = parts.Length > 0 ? parts[0] : string.Empty,
                    LastSyncTime = parts.Length > 1 && DateTime.TryParse(parts[1], out var time) ? time : DateTime.MinValue
                };
            }
            catch (Exception ex)
            {
                LogError($"解析增量同步请求异常: {ex.Message}", ex);
                return new IncrementalSyncRequest();
            }
        }

        /// <summary>
        /// 解析数据同步请求
        /// </summary>
        private DataSyncRequest ParseDataSyncRequest(byte[] body)
        {
            try
            {
                if (body == null || body.Length == 0)
                    return new DataSyncRequest();

                var dataString = System.Text.Encoding.UTF8.GetString(body);
                var parts = dataString.Split('|');

                return new DataSyncRequest
                {
                    DataType = parts.Length > 0 ? parts[0] : string.Empty,
                    SyncMode = parts.Length > 1 ? parts[1] : string.Empty
                };
            }
            catch (Exception ex)
            {
                LogError($"解析数据同步请求异常: {ex.Message}", ex);
                return new DataSyncRequest();
            }
        }

        /// <summary>
        /// 解析实体数据传输请求
        /// </summary>
        private EntityDataTransferRequest ParseEntityDataTransferRequest(OriginalData originalData)
        {
            try
            {
                if (originalData.One == null || originalData.One.Length == 0)
                    return new EntityDataTransferRequest();

                var dataString = System.Text.Encoding.UTF8.GetString(originalData.One);
                var parts = dataString.Split('|');

                return new EntityDataTransferRequest
                {
                    EntityType = parts.Length > 0 ? parts[0] : string.Empty,
                    EntityData = parts.Length > 1 ? parts[1] : string.Empty
                };
            }
            catch (Exception ex)
            {
                LogError($"解析实体数据传输请求异常: {ex.Message}", ex);
                return new EntityDataTransferRequest();
            }
        }

        /// <summary>
        /// 解析动态配置更新请求
        /// </summary>
        private DynamicConfigUpdateRequest ParseDynamicConfigUpdateRequest(OriginalData originalData)
        {
            try
            {
                if (originalData.One == null || originalData.One.Length == 0)
                    return new DynamicConfigUpdateRequest();

                var dataString = System.Text.Encoding.UTF8.GetString(originalData.One);
                var parts = dataString.Split('|');

                return new DynamicConfigUpdateRequest
                {
                    ConfigKey = parts.Length > 0 ? parts[0] : string.Empty,
                    ConfigValue = parts.Length > 1 ? parts[1] : string.Empty
                };
            }
            catch (Exception ex)
            {
                LogError($"解析动态配置更新请求异常: {ex.Message}", ex);
                return new DynamicConfigUpdateRequest();
            }
        }

 
        /// <summary>
        /// 解析全局配置更新请求
        /// </summary>
        private GlobalConfigUpdateRequest ParseGlobalConfigUpdateRequest(byte[] body)
        {
            try
            {
                if (body == null || body.Length == 0)
                    return new GlobalConfigUpdateRequest();

                var dataString = System.Text.Encoding.UTF8.GetString(body);
                var parts = dataString.Split('|');

                return new GlobalConfigUpdateRequest
                {
                    ConfigKey = parts.Length > 0 ? parts[0] : string.Empty,
                    ConfigValue = parts.Length > 1 ? parts[1] : string.Empty
                };
            }
            catch (Exception ex)
            {
                LogError($"解析全局配置更新请求异常: {ex.Message}", ex);
                return new GlobalConfigUpdateRequest();
            }
        }

        /// <summary>
        /// 创建数据同步响应
        /// </summary>
        private OriginalData CreateDataSyncResponse(string operation, DataSyncOperationResult result)
        {
            var responseData = $"{operation}_RESULT|{result.IsSuccess}|{result.Message}";
            var data = System.Text.Encoding.UTF8.GetBytes(responseData);

            // 使用第一个支持的命令作为默认响应命令
            uint commandId = (uint)DataSyncCommands.DataRequest;
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
    /// 数据请求
    /// </summary>
    public class DataRequest
    {
        public string DataType { get; set; }
        public string Filter { get; set; }
    }

    /// <summary>
    /// 全量同步请求
    /// </summary>
    public class FullSyncRequest
    {
        public string DataType { get; set; }
        public string[] TargetClients { get; set; }
    }

    /// <summary>
    /// 增量同步请求
    /// </summary>
    public class IncrementalSyncRequest
    {
        public string DataType { get; set; }
        public DateTime LastSyncTime { get; set; }
    }

    /// <summary>
    /// 数据同步请求
    /// </summary>
    public class DataSyncRequest
    {
        public string DataType { get; set; }
        public string SyncMode { get; set; }
    }

    /// <summary>
    /// 实体数据传输请求
    /// </summary>
    public class EntityDataTransferRequest
    {
        public string EntityType { get; set; }
        public string EntityData { get; set; }
    }

    /// <summary>
    /// 动态配置更新请求
    /// </summary>
    public class DynamicConfigUpdateRequest
    {
        public string ConfigKey { get; set; }
        public string ConfigValue { get; set; }
    }

    /// <summary>
    /// 转发动态配置更新请求
    /// </summary>
    public class ForwardDynamicConfigUpdateRequest
    {
        public string ConfigKey { get; set; }
        public string ConfigValue { get; set; }
        public string[] TargetClients { get; set; }
    }

    /// <summary>
    /// 全局配置更新请求
    /// </summary>
    public class GlobalConfigUpdateRequest
    {
        public string ConfigKey { get; set; }
        public string ConfigValue { get; set; }
    }

    /// <summary>
    /// 数据同步操作结果
    /// </summary>
    public class DataSyncOperationResult
    {
        public bool IsSuccess { get; set; }
        public int RecordCount { get; set; }
        public int SyncedCount { get; set; }
        public int TransferredCount { get; set; }
        public int ForwardedCount { get; set; }
        public string Status { get; set; }
        public DateTime LastSyncTime { get; set; }
        public string Message { get; set; }
    }
}