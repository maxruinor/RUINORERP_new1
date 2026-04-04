using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RUINORERP.Model.Base.StatusManager.PerformanceMonitoring;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Commands.CommandDefinitions;
using RUINORERP.PacketSpec.Models.Core;

namespace RUINORERP.UI.Network.Services
{
    /// <summary>
    /// 客户端性能监控服务
    /// 负责性能数据的采集、本地缓存和上报到服务器
    /// </summary>
    public class ClientPerformanceMonitorService : IDisposable
    {
        private readonly IClientCommunicationService _communicationService;
        private readonly ILogger<ClientPerformanceMonitorService> _logger;
        private readonly PerformanceMonitorManager _monitorManager;
        private readonly Timer _memoryMonitorTimer;
        private bool _disposed = false;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ClientPerformanceMonitorService(
            IClientCommunicationService communicationService,
            ILogger<ClientPerformanceMonitorService> logger = null)
        {
            _communicationService = communicationService ?? throw new ArgumentNullException(nameof(communicationService));
            _logger = logger;
            _monitorManager = new PerformanceMonitorManager();

            // 订阅数据上报事件
            _monitorManager.OnDataUpload += OnPerformanceDataUpload;

            // 订阅PerformanceMonitoringBridge事件，实现跨层数据采集
            SubscribeToBridgeEvents();

            // 初始化内存监控定时器（每30秒采集一次）
            _memoryMonitorTimer = new Timer(MemoryMonitorCallback, null, TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(30));

            _logger?.LogInformation("客户端性能监控服务初始化完成");
        }

        /// <summary>
        /// 订阅PerformanceMonitoringBridge事件，实现跨层数据采集
        /// </summary>
        private void SubscribeToBridgeEvents()
        {
            // 订阅方法执行事件
            PerformanceMonitoringBridge.OnMethodExecution += (sender, data) =>
            {
                if (data != null)
                {
                    RecordMethodExecution(data.MethodName, data.ClassName, data.ExecutionTimeMs, data.IsSuccess, data.ExceptionMessage);
                }
            };

            // 订阅数据库操作事件
            PerformanceMonitoringBridge.OnDatabaseOperation += (sender, data) =>
            {
                if (data != null)
                {
                    RecordDatabaseOperation(data.SqlText, data.OperationType, data.TableName, data.ExecutionTimeMs, data.AffectedRows, data.IsSuccess, data.ErrorMessage, data.IsDeadlock);
                }
            };

            // 订阅网络请求事件
            PerformanceMonitoringBridge.OnNetworkRequest += (sender, data) =>
            {
                if (data != null)
                {
                    RecordNetworkRequest(data.CommandName, data.CommandName, DateTime.Now.AddMilliseconds(-data.ExecutionTimeMs), DateTime.Now, 0, 0, data.IsSuccess, data.ErrorMessage);
                }
            };

            // 订阅死锁事件
            PerformanceMonitoringBridge.OnDeadlock += (sender, data) =>
            {
                if (data != null)
                {
                    _logger?.LogWarning("检测到死锁事件: {TableName}, {OperationType}", data.TableName, data.OperationType);
                }
            };

            _logger?.LogDebug("已订阅PerformanceMonitoringBridge事件");
        }

        /// <summary>
        /// 性能监控是否已启动
        /// </summary>
        private bool _isStarted = false;

        /// <summary>
        /// 启动或更新性能监控
        /// 根据当前配置决定是否启用监控
        /// </summary>
        public void Start()
        {
            // 幂等性检查：避免重复启动
            if (_isStarted)
            {
                _logger?.LogDebug("性能监控已经启动，跳过");
                return;
            }

            // 重新初始化配置（从配置文件或服务器配置加载最新设置）
            PerformanceMonitorSwitch.Initialize();

            // 如果配置中禁用了监控，不启用
            if (!PerformanceMonitorSwitch.IsEnabled)
            {
                _logger?.LogInformation("性能监控在配置中被禁用，不启动监控");
                _isStarted = true; // 标记已处理，避免重复检查
                return;
            }

            _isStarted = true;
            _logger?.LogInformation("客户端性能监控已启动（已启用状态）");
        }

        /// <summary>
        /// 重置启动状态（用于解锁后重新登录）
        /// </summary>
        public void Reset()
        {
            _isStarted = false;
            _logger?.LogDebug("客户端性能监控启动状态已重置");
        }

        /// <summary>
        /// 停止性能监控
        /// </summary>
        public void Stop()
        {
            PerformanceMonitorSwitch.Disable();
            _logger?.LogInformation("客户端性能监控已停止");
        }

        /// <summary>
        /// 性能数据上报事件处理
        /// </summary>
        private async void OnPerformanceDataUpload(object sender, PerformanceDataPacket packet)
        {
            try
            {
                if (_communicationService == null || _communicationService.ConnectionManager?.IsConnected != true)
                {
                    _logger?.LogWarning("网络连接不可用，性能数据上报失败");
                    return;
                }

                var request = new PerformanceDataUploadRequest
                {
                    ClientId = packet.ClientId,
                    PerformanceDataJson = JsonConvert.SerializeObject(packet),
                    PacketSizeBytes = packet.PacketSizeBytes,
                    MetricCount = packet.MetricsJson.Count,
                    MachineName = Environment.MachineName,
                    ClientIpAddress = GetClientIpAddress()
                };

                // 使用现有的 SendCommandWithResponseAsync 方法发送数据
                await SendPerformanceDataAsync(request);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "性能数据上报异常");
            }
        }

        /// <summary>
        /// 发送性能数据到服务器
        /// </summary>
        private async Task SendPerformanceDataAsync(PerformanceDataUploadRequest request)
        {
            try
            {
                // 使用现有通信接口发送请求
                var response = await _communicationService.SendCommandWithResponseAsync<PerformanceDataUploadResponse>(
                    SystemCommands.PerformanceDataUpload,
                    request,
                    CancellationToken.None,
                    10000);

                if (response?.IsSuccess == true)
                {
                    _logger?.LogDebug($"性能数据上报成功: {request.MetricCount} 条指标");
                }
                else
                {
                    _logger?.LogWarning($"性能数据上报失败: {response?.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "发送性能数据请求失败");
            }
        }

        /// <summary>
        /// 内存监控回调
        /// </summary>
        private void MemoryMonitorCallback(object state)
        {
            if (!PerformanceMonitorSwitch.IsMonitorEnabled(PerformanceMonitorType.Memory))
                return;

            try
            {
                _monitorManager.RecordMemoryUsage();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "内存监控采集失败");
            }
        }

        /// <summary>
        /// 记录方法执行性能
        /// </summary>
        public void RecordMethodExecution(string methodName, string className, long executionTimeMs, bool isSuccess, string exceptionMessage = null)
        {
            _monitorManager.RecordMethodExecution(methodName, className, executionTimeMs, isSuccess, exceptionMessage);
        }

        /// <summary>
        /// 记录数据库操作性能
        /// </summary>
        public void RecordDatabaseOperation(string sqlText, string operationType, string tableName, long executionTimeMs, int affectedRows, bool isSuccess, string errorMessage = null, bool isDeadlock = false)
        {
            _monitorManager.RecordDatabaseOperation(sqlText, operationType, tableName, executionTimeMs, affectedRows, isSuccess, errorMessage, isDeadlock);
        }

        /// <summary>
        /// 记录网络请求性能
        /// </summary>
        public void RecordNetworkRequest(string requestId, string commandType, DateTime startTime, DateTime endTime, long bytesSent, long bytesReceived, bool isSuccess, string errorMessage = null, int retryCount = 0)
        {
            _monitorManager.RecordNetworkRequest(requestId, commandType, startTime, endTime, bytesSent, bytesReceived, isSuccess, errorMessage, retryCount);
        }

        /// <summary>
        /// 记录事务性能
        /// </summary>
        public void RecordTransaction(string transactionId, string operationType, DateTime? startTime, DateTime? endTime, bool isCommitted, bool isDeadlock = false, string deadlockInfo = null)
        {
            _monitorManager.RecordTransaction(transactionId, operationType, startTime, endTime, isCommitted, isDeadlock, deadlockInfo);
        }

        /// <summary>
        /// 记录死锁信息
        /// </summary>
        public void RecordDeadlock(string deadlockId, DateTime deadlockTime, System.Collections.Generic.List<string> involvedResources, System.Collections.Generic.List<string> involvedProcesses, string victimProcess, string deadlockGraphXml = null, string deadlockDetails = null)
        {
            _monitorManager.RecordDeadlock(deadlockId, deadlockTime, involvedResources, involvedProcesses, victimProcess, deadlockGraphXml, deadlockDetails);
        }

        /// <summary>
        /// 获取客户端IP地址
        /// </summary>
        private string GetClientIpAddress()
        {
            try
            {
                var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                    }
                }
                return "127.0.0.1";
            }
            catch
            {
                return "Unknown";
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;
            
            _memoryMonitorTimer?.Dispose();
            _monitorManager?.Dispose();
            
            _logger?.LogInformation("客户端性能监控服务已释放");
        }
    }

    /// <summary>
    /// 性能监控拦截器扩展
    /// 用于AOP方式记录方法执行性能
    /// </summary>
    public static class PerformanceMonitorExtensions
    {
        /// <summary>
        /// 执行带性能监控的方法
        /// </summary>
        public static T ExecuteWithMonitoring<T>(this ClientPerformanceMonitorService service, string methodName, string className, Func<T> action)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                var result = action();
                service.RecordMethodExecution(methodName, className, stopwatch.ElapsedMilliseconds, true);
                return result;
            }
            catch (Exception ex)
            {
                service.RecordMethodExecution(methodName, className, stopwatch.ElapsedMilliseconds, false, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 异步执行带性能监控的方法
        /// </summary>
        public static async Task<T> ExecuteWithMonitoringAsync<T>(this ClientPerformanceMonitorService service, string methodName, string className, Func<Task<T>> action)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                var result = await action();
                service.RecordMethodExecution(methodName, className, stopwatch.ElapsedMilliseconds, true);
                return result;
            }
            catch (Exception ex)
            {
                service.RecordMethodExecution(methodName, className, stopwatch.ElapsedMilliseconds, false, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 执行带性能监控的异步Action
        /// </summary>
        public static async Task ExecuteWithMonitoringAsync(this ClientPerformanceMonitorService service, string methodName, string className, Func<Task> action)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                await action();
                service.RecordMethodExecution(methodName, className, stopwatch.ElapsedMilliseconds, true);
            }
            catch (Exception ex)
            {
                service.RecordMethodExecution(methodName, className, stopwatch.ElapsedMilliseconds, false, ex.Message);
                throw;
            }
        }
    }
}