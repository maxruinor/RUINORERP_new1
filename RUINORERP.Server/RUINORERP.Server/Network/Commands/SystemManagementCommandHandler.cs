using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Commands.System;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Protocol;
// using RUINORERP.Server.Network.Services; // 暂时注释，缺少ISystemManagementService接口定义

namespace RUINORERP.Server.Network.Commands
{
    /// <summary>
    /// 系统管理命令处理器 - 处理系统管理相关的命令
    /// </summary>
    [CommandHandler("SystemManagementCommandHandler", priority: 60)]
    public class SystemManagementCommandHandler : CommandHandlerBase
    {
        // private readonly ISystemManagementService _systemManagementService; // 暂时注释，缺少ISystemManagementService接口定义

        /// <summary>
        /// 无参构造函数，以支持Activator.CreateInstance创建实例
        /// </summary>
        public SystemManagementCommandHandler() : base()
        {
            // _systemManagementService = Program.ServiceProvider.GetRequiredService<ISystemManagementService>(); // 暂时注释，缺少ISystemManagementService接口定义
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public SystemManagementCommandHandler(
            // ISystemManagementService systemManagementService, // 暂时注释，缺少ISystemManagementService接口定义
            ILogger<SystemManagementCommandHandler> logger = null) : base(logger)
        {
            // _systemManagementService = systemManagementService; // 暂时注释，缺少ISystemManagementService接口定义
        }

        /// <summary>
        /// 支持的命令类型
        /// </summary>
        public override IReadOnlyList<uint> SupportedCommands => new uint[]
        {
            (uint)SystemManagementCommands.PushVersionUpdate,
            (uint)SystemManagementCommands.SwitchServer,
            (uint)SystemManagementCommands.Shutdown,
            (uint)SystemManagementCommands.DeleteColumnConfig
        };

        /// <summary>
        /// 处理器优先级
        /// </summary>
        public override int Priority => 60;

        /// <summary>
        /// 具体的命令处理逻辑
        /// </summary>
        protected override async Task<CommandResult> ProcessCommandAsync(ICommand command, CancellationToken cancellationToken)
        {
            try
            {
                var commandId = command.CommandIdentifier;

                if (commandId == SystemManagementCommands.PushVersionUpdate)
                {
                    return await HandlePushVersionUpdateAsync(command, cancellationToken);
                }
                else if (commandId == SystemManagementCommands.SwitchServer)
                {
                    return await HandleSwitchServerAsync(command, cancellationToken);
                }
                else if (commandId == SystemManagementCommands.Shutdown)
                {
                    return await HandleShutdownAsync(command, cancellationToken);
                }
                else if (commandId == SystemManagementCommands.DeleteColumnConfig)
                {
                    return await HandleDeleteColumnConfigAsync(command, cancellationToken);
                }
                else
                {
                    return CommandResult.Failure($"不支持的命令类型: {command.CommandIdentifier}", "UNSUPPORTED_COMMAND");
                }
            }
            catch (Exception ex)
            {
                LogError($"处理系统管理命令异常: {ex.Message}", ex);
                return CommandResult.Failure($"处理异常: {ex.Message}", "HANDLER_ERROR", ex);
            }
        }

        /// <summary>
        /// 处理推送版本更新命令
        /// </summary>
        private async Task<CommandResult> HandlePushVersionUpdateAsync(ICommand command, CancellationToken cancellationToken)
        {
            try
            {
                LogInfo($"处理推送版本更新命令 [会话: {command.SessionID}]");

                // 解析版本更新数据
                var versionData = command.Packet.GetJsonData<VersionUpdateData>();
                
                // 暂时返回模拟结果，因为缺少ISystemManagementService接口定义
                var pushResult = new SystemOperationResult
                {
                    IsSuccess = true,
                    SentCount = versionData.TargetClients?.Length ?? 0,
                    Message = "版本更新推送成功（模拟）"
                };

                // 创建响应数据
                var responseData = CreateSystemManagementResponse("VERSION_UPDATE", pushResult);

                return CommandResult.SuccessWithResponse(
                    responseData,
                    data: new { 
                        Version = versionData.Version,
                        TargetCount = versionData.TargetClients?.Length ?? 0,
                        SentCount = pushResult.SentCount,
                        IsSuccess = pushResult.IsSuccess,
                        SessionId = command.SessionID
                    },
                    message: pushResult.IsSuccess ? "版本更新推送成功" : "版本更新推送失败"
                );
            }
            catch (Exception ex)
            {
                LogError($"处理推送版本更新命令异常: {ex.Message}", ex);
                return CommandResult.Failure($"版本更新推送异常: {ex.Message}", "VERSION_UPDATE_ERROR", ex);
            }
        }

        /// <summary>
        /// 处理切换服务器命令
        /// </summary>
        private async Task<CommandResult> HandleSwitchServerAsync(ICommand command, CancellationToken cancellationToken)
        {
            try
            {
                LogInfo($"处理切换服务器命令 [会话: {command.SessionID}]");

                // 解析服务器切换数据
                var switchData = command.Packet.GetJsonData<SwitchServerData>();
                // 暂时返回模拟结果，因为缺少ISystemManagementService接口定义
                var switchResult = new SystemOperationResult
                {
                    IsSuccess = true,
                    Message = "服务器切换成功（模拟）"
                };

                // 创建响应数据
                var responseData = CreateSystemManagementResponse("SWITCH_SERVER", switchResult);

                return CommandResult.SuccessWithResponse(
                    responseData,
                    data: new { 
                        TargetServer = switchData.TargetServer,
                        Reason = switchData.Reason,
                        IsSuccess = switchResult.IsSuccess,
                        SessionId = command.SessionID
                    },
                    message: switchResult.IsSuccess ? "服务器切换成功" : "服务器切换失败"
                );
            }
            catch (Exception ex)
            {
                LogError($"处理切换服务器命令异常: {ex.Message}", ex);
                return CommandResult.Failure($"服务器切换异常: {ex.Message}", "SWITCH_SERVER_ERROR", ex);
            }
        }

        /// <summary>
        /// 处理关机命令
        /// </summary>
        private async Task<CommandResult> HandleShutdownAsync(ICommand command, CancellationToken cancellationToken)
        {
            try
            {
                LogInfo($"处理关机命令 [会话: {command.SessionID}]");

                // 解析关机数据
                var shutdownData = command.Packet.GetJsonData<ShutdownData>();
                // 暂时返回模拟结果，因为缺少ISystemManagementService接口定义
                var shutdownResult = new SystemOperationResult
                {
                    IsSuccess = true,
                    Message = "关机命令执行成功（模拟）"
                };

                // 创建响应数据
                var responseData = CreateSystemManagementResponse("SHUTDOWN", shutdownResult);

                return CommandResult.SuccessWithResponse(
                    responseData,
                    data: new { 
                        Reason = shutdownData.Reason,
                        DelaySeconds = shutdownData.DelaySeconds,
                        IsSuccess = shutdownResult.IsSuccess,
                        SessionId = command.SessionID
                    },
                    message: shutdownResult.IsSuccess ? "关机命令执行成功" : "关机命令执行失败"
                );
            }
            catch (Exception ex)
            {
                LogError($"处理关机命令异常: {ex.Message}", ex);
                return CommandResult.Failure($"关机命令异常: {ex.Message}", "SHUTDOWN_ERROR", ex);
            }
        }

        /// <summary>
        /// 处理删除列配置命令
        /// </summary>
        private async Task<CommandResult> HandleDeleteColumnConfigAsync(ICommand command, CancellationToken cancellationToken)
        {
            try
            {
                LogInfo($"处理删除列配置命令 [会话: {command.SessionID}]");

                // 解析删除列配置数据
                var deleteData = command.Packet.GetJsonData<DeleteColumnConfigData>();
                // 暂时返回模拟结果，因为缺少ISystemManagementService接口定义
                var deleteResult = new SystemOperationResult
                {
                    IsSuccess = true,
                    Message = "列配置删除成功（模拟）"
                };

                // 创建响应数据
                var responseData = CreateSystemManagementResponse("DELETE_CONFIG", deleteResult);

                return CommandResult.SuccessWithResponse(
                    responseData,
                    data: new { 
                        ConfigId = deleteData.ConfigId,
                        TableName = deleteData.TableName,
                        IsSuccess = deleteResult.IsSuccess,
                        SessionId = command.SessionID
                    },
                    message: deleteResult.IsSuccess ? "列配置删除成功" : "列配置删除失败"
                );
            }
            catch (Exception ex)
            {
                LogError($"处理删除列配置命令异常: {ex.Message}", ex);
                return CommandResult.Failure($"删除列配置异常: {ex.Message}", "DELETE_CONFIG_ERROR", ex);
            }
        }

        /// <summary>
        /// 解析版本更新数据
        /// </summary>
        private VersionUpdateData ParseVersionUpdateData(byte[] body)
        {
            try
            {
                if (body == null || body.Length == 0)
                    return new VersionUpdateData();

                var dataString = System.Text.Encoding.UTF8.GetString(body);
                var parts = dataString.Split('|');

                return new VersionUpdateData
                {
                    Version = parts.Length > 0 ? parts[0] : string.Empty,
                    UpdateMessage = parts.Length > 1 ? parts[1] : string.Empty,
                    TargetClients = parts.Length > 2 ? parts[2].Split(',') : new string[0]
                };
            }
            catch (Exception ex)
            {
                LogError($"解析版本更新数据异常: {ex.Message}", ex);
                return new VersionUpdateData();
            }
        }
 

       

       

        /// <summary>
        /// 创建系统管理响应
        /// </summary>
        private OriginalData CreateSystemManagementResponse(string operation, SystemOperationResult result)
        {
            var responseData = $"{operation}_RESULT|{result.IsSuccess}|{result.Message}";
            var data = System.Text.Encoding.UTF8.GetBytes(responseData);

            // 使用第一个支持的命令作为默认响应命令
            uint commandId = (uint)SystemManagementCommands.PushVersionUpdate;
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
    /// 版本更新数据
    /// </summary>
    public class VersionUpdateData
    {
        public string Version { get; set; }
        public string UpdateMessage { get; set; }
        public string[] TargetClients { get; set; }
    }

    /// <summary>
    /// 服务器切换数据
    /// </summary>
    public class SwitchServerData
    {
        public string TargetServer { get; set; }
        public string Reason { get; set; }
    }

    /// <summary>
    /// 关机数据
    /// </summary>
    public class ShutdownData
    {
        public string Reason { get; set; }
        public int DelaySeconds { get; set; }
    }

    /// <summary>
    /// 删除列配置数据
    /// </summary>
    public class DeleteColumnConfigData
    {
        public string ConfigId { get; set; }
        public string TableName { get; set; }
    }

    /// <summary>
    /// 系统操作结果
    /// </summary>
    public class SystemOperationResult
    {
        public bool IsSuccess { get; set; }
        public int SentCount { get; set; }
        public string Message { get; set; }
    }
}