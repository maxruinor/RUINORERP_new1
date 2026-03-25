using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.TopServer.Network;
using RUINORERP.TopServer.ServerManagement;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Common;
using RUINORERP.PacketSpec.Models.ServerManagement;

namespace RUINORERP.TopServer.Network.CommandHandlers
{
    /// <summary>
    /// TopServer管理命令处理器
    /// 处理子服务器的注册、心跳、状态上报等管理命令
    /// </summary>
    [CommandHandler("ManagementCommandHandler", priority: 100)]
    public class ManagementCommandHandler : BaseCommandHandler
    {
        private readonly ServerManager _serverManager;
        private readonly ILogger<ManagementCommandHandler> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ManagementCommandHandler(
            ServerManager serverManager,
            ILogger<ManagementCommandHandler> logger = null) : base(logger)
        {
            _serverManager = serverManager ?? throw new ArgumentNullException(nameof(serverManager));
            _logger = logger;

            // 设置支持的命令
            SetSupportedCommands(
                ManagementCommands.RegisterServer,
                ManagementCommands.Heartbeat,
                ManagementCommands.ReportStatus,
                ManagementCommands.ReportUsers,
                ManagementCommands.ReportConfiguration,
                ManagementCommands.QueryServerInstances,
                ManagementCommands.ServerStatistics
            );
        }

        /// <summary>
        /// 核心处理方法，根据命令类型分发到对应的处理函数
        /// </summary>
        protected override async Task<IResponse> OnHandleAsync(QueuedCommand cmd, CancellationToken cancellationToken)
        {
            try
            {
                var commandId = cmd.Packet.CommandId;

                if (commandId == ManagementCommands.RegisterServer)
                {
                    return await HandleRegisterServerAsync(cmd, cancellationToken);
                }
                else if (commandId == ManagementCommands.Heartbeat)
                {
                    return await HandleHeartbeatAsync(cmd, cancellationToken);
                }
                else if (commandId == ManagementCommands.ReportStatus)
                {
                    return await HandleReportStatusAsync(cmd, cancellationToken);
                }
                else if (commandId == ManagementCommands.ReportUsers)
                {
                    return await HandleReportUsersAsync(cmd, cancellationToken);
                }
                else if (commandId == ManagementCommands.ReportConfiguration)
                {
                    return await HandleReportConfigurationAsync(cmd, cancellationToken);
                }
                else if (commandId == ManagementCommands.QueryServerInstances)
                {
                    return await HandleQueryServerInstancesAsync(cmd, cancellationToken);
                }
                else if (commandId == ManagementCommands.ServerStatistics)
                {
                    return await HandleServerStatisticsAsync(cmd, cancellationToken);
                }
                else
                {
                    return ResponseFactory.CreateSpecificErrorResponse(cmd.Packet, $"不支持的管理命令: {commandId.Name}");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理管理命令时出错: {CommandName}, 错误: {Message}", cmd.Packet.CommandId.Name, ex.Message);
                return ResponseFactory.CreateSpecificErrorResponse(cmd.Packet.ExecutionContext, ex, $"处理管理命令时出错: {cmd.Packet.CommandId.Name}");
            }
        }

        /// <summary>
        /// 处理服务器注册命令
        /// </summary>
        private async Task<ServerRegisterResponse> HandleRegisterServerAsync(QueuedCommand cmd, CancellationToken cancellationToken)
        {
            try
            {
                if (cmd.Packet.Request is ServerRegisterRequest request)
                {
                    // 获取会话信息
                    var session = GetServerSession(cmd.Packet.ExecutionContext.SessionId);
                    if (session == null)
                    {
                        return new ServerRegisterResponse { IsSuccess = false, Message = "会话不存在", ResponseTime = DateTime.Now };
                    }

                    // 创建服务器实例信息（直接使用PacketSpec的公共模型）
                    var instanceInfo = new ServerInstanceInfo
                    {
                        ServerId = request.ServerId,
                        InstanceName = request.ServerName,
                        IpAddress = request.IpAddress,
                        Port = request.Port,
                        Version = request.Version,
                        ServerType = request.ServerType,
                        Capabilities = request.Capabilities
                    };

                    // 注册服务器实例
                    var success = _serverManager.RegisterServerInstance(session, instanceInfo);
                    if (!success)
                    {
                        return new ServerRegisterResponse { IsSuccess = false, Message = "注册失败", ResponseTime = DateTime.Now };
                    }

                    _logger?.LogInformation("服务器注册成功 - ServerId: {ServerId}, InstanceId: {InstanceId}, IP: {IpAddress}:{Port}",
                        request.ServerId, instanceInfo.InstanceId, request.IpAddress, request.Port);

                    // 构建响应
                    var response = new ServerRegisterResponse
                    {
                        IsSuccess = true,
                        RegistrationSuccessful = true,
                        AssignedInstanceId = instanceInfo.InstanceId,
                        PeerServers = GetPeerServers(instanceInfo.InstanceId).ToList()
                    };

                    return response;
                }
                else
                {
                    return new ServerRegisterResponse { IsSuccess = false, Message = "请求格式错误", ResponseTime = DateTime.Now };
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理服务器注册命令时出错");
                return new ServerRegisterResponse { IsSuccess = false, Message = $"处理失败: {ex.Message}", ResponseTime = DateTime.Now };
            }
        }

        /// <summary>
        /// 处理心跳命令
        /// </summary>
        private async Task<ServerHeartbeatResponse> HandleHeartbeatAsync(QueuedCommand cmd, CancellationToken cancellationToken)
        {
            try
            {
                var session = GetServerSession(cmd.Packet.ExecutionContext.SessionId);
                if (session?.ServerInstance == null)
                {
                    return new ServerHeartbeatResponse { IsSuccess = false, Message = "服务器实例未注册", ResponseTime = DateTime.Now };
                }

                // 更新心跳
                _serverManager.UpdateHeartbeat(session);

                // 更新指标（如果有）- 直接赋值，不需要转换
                if (cmd.Packet.Request is ServerHeartbeatRequest heartbeatRequest && heartbeatRequest.Metrics != null)
                {
                    session.ServerInstance.Metrics = heartbeatRequest.Metrics;
                }

                var response = new ServerHeartbeatResponse
                {
                    IsSuccess = true,
                    HeartbeatConfirmed = true,
                    NextHeartbeatInterval = 30
                };

                return response;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理心跳命令时出错");
                return new ServerHeartbeatResponse { IsSuccess = false, Message = $"处理失败: {ex.Message}", ResponseTime = DateTime.Now };
            }
        }

        /// <summary>
        /// 处理状态上报命令
        /// </summary>
        private async Task<StatusReportResponse> HandleReportStatusAsync(QueuedCommand cmd, CancellationToken cancellationToken)
        {
            try
            {
                var session = GetServerSession(cmd.Packet.ExecutionContext.SessionId);
                if (session?.ServerInstance == null)
                {
                    return new StatusReportResponse { IsSuccess = false, Message = "服务器实例未注册", ResponseTime = DateTime.Now };
                }

                if (cmd.Packet.Request is StatusReportRequest request)
                {
                    // 更新服务器状态 - 直接转换枚举值
                    session.ServerInstance.Status = ConvertServerStatus(request.Status);

                    // 更新指标 - 直接赋值，因为类型已经统一
                    if (request.Metrics != null)
                    {
                        session.ServerInstance.Metrics = request.Metrics;
                    }

                    var response = new StatusReportResponse
                    {
                        IsSuccess = true,
                        StatusConfirmed = true
                    };

                    return response;
                }
                else
                {
                    return new StatusReportResponse { IsSuccess = false, Message = "请求格式错误", ResponseTime = DateTime.Now };
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理状态上报命令时出错");
                return new StatusReportResponse { IsSuccess = false, Message = $"处理失败: {ex.Message}", ResponseTime = DateTime.Now };
            }
        }

        /// <summary>
        /// 处理用户信息上报命令
        /// </summary>
        private async Task<UsersReportResponse> HandleReportUsersAsync(QueuedCommand cmd, CancellationToken cancellationToken)
        {
            try
            {
                var session = GetServerSession(cmd.Packet.ExecutionContext.SessionId);
                if (session?.ServerInstance == null)
                {
                    return new UsersReportResponse { IsSuccess = false, Message = "服务器实例未注册", ResponseTime = DateTime.Now };
                }

                if (cmd.Packet.Request is UsersReportRequest request)
                {
                    _logger?.LogInformation("接收到用户信息上报 - ServerId: {ServerId}, 用户数: {UserCount}",
                        session.ServerInstance.ServerId, request.OnlineUsers.Count);

                    var response = new UsersReportResponse
                    {
                        IsSuccess = true,
                        ReportAccepted = true,
                        ReceivedUserCount = request.OnlineUsers.Count
                    };

                    return response;
                }
                else
                {
                    return new UsersReportResponse { IsSuccess = false, Message = "请求格式错误", ResponseTime = DateTime.Now };
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理用户信息上报命令时出错");
                return new UsersReportResponse { IsSuccess = false, Message = $"处理失败: {ex.Message}", ResponseTime = DateTime.Now };
            }
        }

        /// <summary>
        /// 处理配置上报命令
        /// </summary>
        private async Task<ConfigurationReportResponse> HandleReportConfigurationAsync(QueuedCommand cmd, CancellationToken cancellationToken)
        {
            try
            {
                var session = GetServerSession(cmd.Packet.ExecutionContext.SessionId);
                if (session?.ServerInstance == null)
                {
                    return new ConfigurationReportResponse { IsSuccess = false, Message = "服务器实例未注册", ResponseTime = DateTime.Now };
                }

                if (cmd.Packet.Request is ConfigurationReportRequest request)
                {
                    _logger?.LogInformation("接收到配置上报 - ServerId: {ServerId}, 版本: {ConfigVersion}, 类型: {ConfigType}",
                        session.ServerInstance.ServerId, request.ConfigVersion, request.ConfigType);

                    var response = new ConfigurationReportResponse
                    {
                        IsSuccess = true,
                        ReportAccepted = true,
                        ConfigUpdateRequired = false
                    };

                    return response;
                }
                else
                {
                    return new ConfigurationReportResponse { IsSuccess = false, Message = "请求格式错误", ResponseTime = DateTime.Now };
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理配置上报命令时出错");
                return new ConfigurationReportResponse { IsSuccess = false, Message = $"处理失败: {ex.Message}", ResponseTime = DateTime.Now };
            }
        }

        /// <summary>
        /// 处理服务器实例列表查询命令
        /// </summary>
        private async Task<QueryServerInstancesResponse> HandleQueryServerInstancesAsync(QueuedCommand cmd, CancellationToken cancellationToken)
        {
            try
            {
                IEnumerable<ServerInstanceInfo> servers;

                if (cmd.Packet.Request is QueryServerInstancesRequest request)
                {
                    // 根据过滤条件查询
                    servers = _serverManager.AllServerInstances;

                    if (!string.IsNullOrEmpty(request.ServerTypeFilter))
                    {
                        servers = servers.Where(s => s.ServerType == request.ServerTypeFilter);
                    }

                    if (request.StatusFilter.HasValue)
                    {
                        var status = ConvertServerStatus(request.StatusFilter.Value);
                        servers = servers.Where(s => s.Status == status);
                    }
                }
                else
                {
                    servers = _serverManager.AllServerInstances;
                }

                var response = new QueryServerInstancesResponse
                {
                    IsSuccess = true,
                    ServerInstances = servers.ToList(),
                    TotalCount = servers.Count()
                };

                return response;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理服务器实例列表查询命令时出错");
                var errorResponse = new QueryServerInstancesResponse();
                return errorResponse.CreateErrorResponse($"处理失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 处理服务器统计命令
        /// </summary>
        private async Task<ServerStatisticsResponse> HandleServerStatisticsAsync(QueuedCommand cmd, CancellationToken cancellationToken)
        {
            try
            {
                var allServers = _serverManager.AllServerInstances.ToList();
                var onlineServers = _serverManager.OnlineServerInstances.ToList();

                var response = new ServerStatisticsResponse
                {
                    IsSuccess = true,
                    TotalServers = allServers.Count,
                    OnlineServers = onlineServers.Count,
                    OfflineServers = _serverManager.OfflineServerInstances.Count(),
                    ExceptionServers = _serverManager.ExceptionServerInstances.Count(),
                    TotalUsers = onlineServers.Sum(s => 0), // TODO: 从ServerInstance获取在线用户数
                    OnlineUsers = 0, // TODO: 汇总所有在线用户
                    TotalConnections = onlineServers.Sum(s => s.Metrics?.CurrentConnections ?? 0),
                    AverageCpuUsage = onlineServers.Any() ? onlineServers.Average(s => s.Metrics?.CpuUsage ?? 0) : 0,
                    AverageMemoryUsage = onlineServers.Any() ? onlineServers.Average(s => s.Metrics?.MemoryUsage ?? 0) : 0
                };

                return response;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理服务器统计命令时出错");
                var errorResponse = new ServerStatisticsResponse();
                return errorResponse.CreateErrorResponse($"处理失败: {ex.Message}");
            }
        }

        #region 辅助方法

        /// <summary>
        /// 获取ServerSession
        /// </summary>
        private ServerSession GetServerSession(string sessionId)
        {
            // TODO: 从SessionService获取ServerSession
            return null;
        }

        /// <summary>
        /// 获取同级服务器
        /// </summary>
        private IEnumerable<ServerInstanceInfo> GetPeerServers(Guid currentInstanceId)
        {
            return _serverManager.OnlineServerInstances.Where(s => s.InstanceId != currentInstanceId);
        }

        /// <summary>
        /// 转换服务器状态枚举
        /// </summary>
        private ServerInstanceStatus ConvertServerStatus(ManagementCommands.ServerStatus status)
        {
            return status switch
            {
                ManagementCommands.ServerStatus.Running => ServerInstanceStatus.Online,
                ManagementCommands.ServerStatus.Stopped => ServerInstanceStatus.Offline,
                ManagementCommands.ServerStatus.Error => ServerInstanceStatus.Exception,
                ManagementCommands.ServerStatus.Starting => ServerInstanceStatus.Online,
                ManagementCommands.ServerStatus.Stopping => ServerInstanceStatus.Offline,
                _ => ServerInstanceStatus.Offline
            };
        }

        #endregion
    }
}
