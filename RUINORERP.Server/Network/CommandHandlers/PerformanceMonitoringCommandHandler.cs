using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RUINORERP.Model.Base.StatusManager.PerformanceMonitoring;
using RUINORERP.Model.ConfigModel;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Commands.CommandDefinitions;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.Server.Network.Services;

namespace RUINORERP.Server.Network.CommandHandlers
{
    /// <summary>
    /// 性能监控命令处理器
    /// 处理客户端上报的性能监控数据和查询请求
    /// </summary>
    [CommandHandler("PerformanceMonitoringCommandHandler", priority: 5)]
    public class PerformanceMonitoringCommandHandler : BaseCommandHandler
    {
        private readonly PerformanceDataStorageService _storageService;

        /// <summary>
        /// 构造函数
        /// </summary>
        public PerformanceMonitoringCommandHandler(
            PerformanceDataStorageService storageService,
            ILogger<PerformanceMonitoringCommandHandler> logger) : base(logger)
        {
            _storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));

            SetSupportedCommands(
                SystemCommands.PerformanceDataUpload,
                SystemCommands.PerformanceDataQuery,
                SystemCommands.PerformanceStatistics,
                SystemCommands.PerformanceMonitorControl,
                SystemCommands.PerformanceMonitorStatus
            );
        }

        /// <summary>
        /// 具体的命令处理逻辑
        /// </summary>
        protected override async Task<IResponse> OnHandleAsync(QueuedCommand cmd, CancellationToken cancellationToken)
        {
            var commandId = cmd.Packet.CommandId;

            try
            {
                if (commandId == SystemCommands.PerformanceDataUpload)
                {
                    return await HandlePerformanceDataUploadAsync(cmd, cancellationToken);
                }
                else if (commandId == SystemCommands.PerformanceDataQuery)
                {
                    return await HandlePerformanceDataQueryAsync(cmd, cancellationToken);
                }
                else if (commandId == SystemCommands.PerformanceStatistics)
                {
                    return await HandlePerformanceStatisticsAsync(cmd, cancellationToken);
                }
                else if (commandId == SystemCommands.PerformanceMonitorControl)
                {
                    return await HandlePerformanceMonitorControlAsync(cmd, cancellationToken);
                }
                else if (commandId == SystemCommands.PerformanceMonitorStatus)
                {
                    return await HandlePerformanceMonitorStatusAsync(cmd, cancellationToken);
                }
                else
                {
                    return ResponseFactory.CreateSpecificErrorResponse(cmd.Packet, $"不支持的性能监控命令: {commandId}");
                }
            }
            catch (Exception ex)
            {
                LogError($"处理性能监控命令异常: {commandId}", ex);
                return ResponseFactory.CreateSpecificErrorResponse(cmd.Packet.ExecutionContext, ex, "处理性能监控命令异常");
            }
        }

        /// <summary>
        /// 处理性能数据上报
        /// </summary>
        private async Task<IResponse> HandlePerformanceDataUploadAsync(QueuedCommand cmd, CancellationToken cancellationToken)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                var request = cmd.Packet.Request as PerformanceDataUploadRequest;
                if (request == null)
                {
                    return ResponseFactory.CreateSpecificErrorResponse(cmd.Packet, "无效的性能数据上报请求");
                }

                if (string.IsNullOrEmpty(request.ClientId) && cmd.Packet?.ExecutionContext?.UserId > 0)
                {
                    request.ClientId = cmd.Packet.ExecutionContext.UserId.ToString();
                }

                LogInfo($"开始处理性能数据上传: 客户端 {request.ClientId}, 机器 {request.MachineName}, IP {request.ClientIpAddress}");

                var response = _storageService.StorePerformanceData(request);

                stopwatch.Stop();
                LogInfo($"完成处理性能数据上传: 客户端 {request.ClientId}, {response.ProcessedMetricCount} 条指标, 总耗时 {stopwatch.ElapsedMilliseconds}ms");

                CheckAndTriggerAlerts(request);

                return response;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                LogError($"处理性能数据上报失败: 耗时 {stopwatch.ElapsedMilliseconds}ms", ex);
                return ResponseFactory.CreateSpecificErrorResponse(cmd.Packet.ExecutionContext, ex, "处理性能数据上报失败");
            }
        }

        /// <summary>
        /// 处理性能数据查询
        /// </summary>
        private async Task<IResponse> HandlePerformanceDataQueryAsync(QueuedCommand cmd, CancellationToken cancellationToken)
        {
            try
            {
                var request = cmd.Packet.Request as PerformanceDataQueryRequest;
                if (request == null)
                {
                    return ResponseFactory.CreateSpecificErrorResponse(cmd.Packet, "无效的查询请求");
                }

                var response = _storageService.QueryPerformanceData(request);

                LogDebug($"查询性能数据: 客户端 {request.TargetClientId ?? "All"}, 返回 {response.TotalRecords} 条记录");

                return response;
            }
            catch (Exception ex)
            {
                LogError("处理性能数据查询失败", ex);
                return ResponseFactory.CreateSpecificErrorResponse(cmd.Packet.ExecutionContext, ex, "处理性能数据查询失败");
            }
        }

        /// <summary>
        /// 处理统计摘要请求
        /// </summary>
        private async Task<IResponse> HandlePerformanceStatisticsAsync(QueuedCommand cmd, CancellationToken cancellationToken)
        {
            try
            {
                var request = cmd.Packet.Request as PerformanceStatisticsRequest;
                if (request == null)
                {
                    return ResponseFactory.CreateSpecificErrorResponse(cmd.Packet, "无效的统计请求");
                }

                var response = _storageService.GetStatisticsSummary(request);

                LogDebug($"生成统计摘要: 客户端 {request.TargetClientId ?? "All"}, 时间范围 {request.TimeRangeHours} 小时");

                return response;
            }
            catch (Exception ex)
            {
                LogError("处理统计摘要请求失败", ex);
                return ResponseFactory.CreateSpecificErrorResponse(cmd.Packet.ExecutionContext, ex, "处理统计摘要请求失败");
            }
        }

        /// <summary>
        /// 处理性能监控开关控制
        /// </summary>
        private async Task<IResponse> HandlePerformanceMonitorControlAsync(QueuedCommand cmd, CancellationToken cancellationToken)
        {
            try
            {
                var request = cmd.Packet.Request as PerformanceMonitorControlRequest;
                if (request == null)
                {
                    return ResponseFactory.CreateSpecificErrorResponse(cmd.Packet, "无效的控制请求");
                }

                var response = new PerformanceMonitorControlResponse
                {
                    AffectedClientCount = string.IsNullOrEmpty(request.TargetClientId) ? 0 : 1,
                    ServerMonitorEnabled = request.Enable
                };

                LogInfo($"性能监控开关控制: 目标 {request.TargetClientId ?? "All"}, 启用: {request.Enable}");

                return response;
            }
            catch (Exception ex)
            {
                LogError("处理性能监控开关控制失败", ex);
                return ResponseFactory.CreateSpecificErrorResponse(cmd.Packet.ExecutionContext, ex, "处理性能监控开关控制失败");
            }
        }

        /// <summary>
        /// 处理性能监控状态上报
        /// </summary>
        private async Task<IResponse> HandlePerformanceMonitorStatusAsync(QueuedCommand cmd, CancellationToken cancellationToken)
        {
            try
            {
                var request = cmd.Packet.Request as PerformanceMonitorStatusRequest;
                if (request == null)
                {
                    return ResponseFactory.CreateSpecificErrorResponse(cmd.Packet, "无效的状态上报请求");
                }

                var response = new PerformanceMonitorStatusResponse
                {
                    ServerAckTime = DateTime.Now
                };

                LogDebug($"接收性能监控状态: 客户端 {request.ClientId}, 启用: {request.IsEnabled}, 缓冲区: {request.BufferMetricCount}");

                return response;
            }
            catch (Exception ex)
            {
                LogError("处理性能监控状态上报失败", ex);
                return ResponseFactory.CreateSpecificErrorResponse(cmd.Packet.ExecutionContext, ex, "处理性能监控状态上报失败");
            }
        }

        /// <summary>
        /// 检查并触发告警
        /// </summary>
        private void CheckAndTriggerAlerts(PerformanceDataUploadRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.PerformanceDataJson))
                {
                    return;
                }

                var packet = JsonConvert.DeserializeObject<PerformanceDataPacket>(request.PerformanceDataJson);
                if (packet?.MetricsJson == null || packet.MetricsJson.Count == 0)
                {
                    return;
                }

                var config = PerformanceMonitorSwitch.Config;
                foreach (var metricJson in packet.MetricsJson)
                {
                    try
                    {
                        var metricType = GetMetricType(metricJson);
                        if (metricType == null) continue;

                        CheckMetricAlert(metricType.Value, metricJson, config);
                    }
                    catch (Exception ex)
                    {
                        LogWarning($"检查单个指标告警时发生异常，跳过该指标: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("检查告警失败", ex);
            }
        }

        /// <summary>
        /// 从JSON获取指标类型
        /// </summary>
        private PerformanceMonitorType? GetMetricType(string metricJson)
        {
            try
            {
                var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(metricJson);
                if (json != null && json.TryGetValue("MetricType", out var typeObj))
                {
                    var typeValue = Convert.ToInt32(typeObj);
                    return (PerformanceMonitorType)typeValue;
                }
            }
            catch (Exception ex)
            {
                LogDebug($"解析指标类型失败，返回null: {ex.Message}");
            }
            return null;
        }

        /// <summary>
        /// 检查单个指标是否需要告警
        /// </summary>
        private void CheckMetricAlert(PerformanceMonitorType metricType, string metricJson, PerformanceMonitorConfig config)
        {
            try
            {
                var json = JsonConvert.DeserializeObject<Dictionary<string, object>>(metricJson);
                if (json == null) return;

                bool shouldAlert = false;
                string alertTitle = "";
                string alertDetails = "";

                switch (metricType)
                {
                    case PerformanceMonitorType.MethodExecution:
                        if (json.TryGetValue("ExecutionTimeMs", out var execTime) && Convert.ToInt64(execTime) > config.MethodExecutionThresholdMs)
                        {
                            shouldAlert = true;
                            alertTitle = "方法执行超时";
                            alertDetails = $"执行时间 {execTime}ms 超过阈值 {config.MethodExecutionThresholdMs}ms";
                        }
                        break;

                    case PerformanceMonitorType.Database:
                        if (json.TryGetValue("ExecutionTimeMs", out var dbTime) && Convert.ToInt64(dbTime) > config.DatabaseQueryThresholdMs)
                        {
                            shouldAlert = true;
                            alertTitle = "数据库查询缓慢";
                            alertDetails = $"SQL执行时间 {dbTime}ms 超过阈值 {config.DatabaseQueryThresholdMs}ms";
                        }
                        if (json.TryGetValue("IsDeadlock", out var isDeadlock) && Convert.ToBoolean(isDeadlock))
                        {
                            shouldAlert = true;
                            alertTitle = "死锁检测";
                            alertDetails = "检测到数据库死锁";
                        }
                        break;

                    case PerformanceMonitorType.Network:
                        if (json.TryGetValue("ResponseTimeMs", out var netTime) && Convert.ToInt64(netTime) > config.NetworkRequestThresholdMs)
                        {
                            shouldAlert = true;
                            alertTitle = "网络请求超时";
                            alertDetails = $"响应时间 {netTime}ms 超过阈值 {config.NetworkRequestThresholdMs}ms";
                        }
                        break;

                    case PerformanceMonitorType.Memory:
                        if (json.TryGetValue("WorkingSetBytes", out var memBytes))
                        {
                            var workingSetMB = Convert.ToInt64(memBytes) / (1024 * 1024);
                            if (workingSetMB > config.MemoryCriticalThresholdMB)
                            {
                                shouldAlert = true;
                                alertTitle = "内存使用临界";
                                alertDetails = $"内存使用 {workingSetMB}MB 超过临界阈值 {config.MemoryCriticalThresholdMB}MB";
                            }
                            else if (workingSetMB > config.MemoryWarningThresholdMB)
                            {
                                shouldAlert = true;
                                alertTitle = "内存使用过高";
                                alertDetails = $"内存使用 {workingSetMB}MB 超过警告阈值 {config.MemoryWarningThresholdMB}MB";
                            }
                        }
                        break;

                    case PerformanceMonitorType.Transaction:
                        if (json.TryGetValue("IsDeadlock", out var txDeadlock) && Convert.ToBoolean(txDeadlock))
                        {
                            shouldAlert = true;
                            alertTitle = "事务死锁";
                            alertDetails = "检测到事务死锁";
                        }
                        else if (json.TryGetValue("DurationMs", out var duration) && Convert.ToInt64(duration) > 10000)
                        {
                            shouldAlert = true;
                            alertTitle = "长事务";
                            alertDetails = $"事务执行时间 {duration}ms 超过10秒";
                        }
                        break;
                }

                if (shouldAlert)
                {
                    LogWarning($"性能告警: {alertTitle} - {alertDetails}");
                }
            }
            catch (Exception ex)
            {
                LogError("检查指标告警失败", ex);
            }
        }
    }
}
