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

            // 初始化UI响应监控（消息循环检测）
            InitializeUIMonitoring();

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

            // 订阅数据库操作事件（重要事件实时上报）
            PerformanceMonitoringBridge.OnDatabaseOperation += (sender, data) =>
            {
                if (data != null)
                {
                    RecordDatabaseOperation(data.SqlText, data.OperationType, data.TableName, data.ExecutionTimeMs, data.AffectedRows, data.IsSuccess, data.ErrorMessage, data.IsDeadlock);

                    // 死锁事件立即触发上报
                    if (data.IsDeadlock)
                    {
                        TriggerImmediateUpload("死锁事件");
                    }
                    // 执行时间超过阈值也立即上报
                    else if (data.ExecutionTimeMs > 5000)
                    {
                        TriggerImmediateUpload("慢查询事件");
                    }
                }
            };

            // 订阅网络请求事件
            PerformanceMonitoringBridge.OnNetworkRequest += (sender, data) =>
            {
                if (data != null)
                {
                    RecordNetworkRequest(data.CommandName, data.CommandName, DateTime.Now.AddMilliseconds(-data.ExecutionTimeMs), DateTime.Now, 0, 0, data.IsSuccess, data.ErrorMessage);

                    // 网络请求失败立即上报
                    if (!data.IsSuccess)
                    {
                        TriggerImmediateUpload("网络请求失败");
                    }
                }
            };

            // 订阅死锁事件（最高优先级，立即上报）
            PerformanceMonitoringBridge.OnDeadlock += (sender, data) =>
            {
                if (data != null)
                {
                    _logger?.LogWarning("检测到死锁事件: {TableName}, {OperationType}", data.TableName, data.OperationType);
                    // 死锁事件最高优先级，立即触发上报
                    TriggerImmediateUpload("死锁事件-最高优先级");
                }
            };

            _logger?.LogDebug("已订阅PerformanceMonitoringBridge事件");
        }

        /// <summary>
        /// 触发立即上报（重要事件）
        /// </summary>
        private void TriggerImmediateUpload(string reason)
        {
            try
            {
                // 检查连接状态
                if (_communicationService?.ConnectionManager?.IsConnected != true)
                {
                    _logger?.LogWarning("重要事件无法上报: {Reason}, 网络未连接", reason ?? "未知");
                    return;
                }

                // 立即触发数据打包和上报
                _monitorManager.TriggerImmediateUpload();

                _logger?.LogDebug("重要事件已触发立即上报: {Reason}", reason ?? "未知");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "触发立即上报失败: {Reason}", reason ?? "未知");
            }
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
        /// 暂停上报（网络断开时调用）
        /// </summary>
        public void PauseUpload()
        {
            _monitorManager.PauseUpload();
            _logger?.LogInformation("客户端性能监控已暂停上报");
        }

        /// <summary>
        /// 恢复上报（网络重连时调用）
        /// </summary>
        public void ResumeUpload()
        {
            _monitorManager.ResumeUpload();
            _logger?.LogInformation("客户端性能监控已恢复上报");
        }

        /// <summary>
        /// 性能数据上报事件处理
        /// </summary>
        private async void OnPerformanceDataUpload(object sender, PerformanceDataPacket packet)
        {
            try
            {
                // ✅ 提前检查：网络连接状态
                if (_communicationService == null || _communicationService.ConnectionManager?.IsConnected != true)
                {
                    _logger?.LogDebug("网络连接不可用，跳过性能数据上报");
                    return;
                }

                // ✅ 提前检查：Token有效性（避免进入发送流程后才失败）
                var appContext = MainForm.Instance?.AppContext;
                if (appContext == null || string.IsNullOrEmpty(appContext.SessionId))
                {
                    _logger?.LogDebug("会话未初始化或Token无效，跳过性能数据上报");
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
                // ✅ async void方法的最后一道防线，确保任何未捕获的异常都不会导致程序崩溃
                // 对于Token相关错误，降低日志级别为Debug
                if (ex.Message.Contains("授权令牌") || ex.Message.Contains("Token"))
                {
                    _logger?.LogDebug("Token未就绪，跳过本次性能数据上报: {Message}", ex.Message);
                    return;
                }
                
                // ✅ 其他错误也降低为Warning级别，因为这是辅助功能
                _logger?.LogWarning(ex, "性能数据上报异常（非关键错误，已静默处理）");
            }
        }

        /// <summary>
        /// 发送性能数据到服务器
        /// </summary>
        private async Task SendPerformanceDataAsync(PerformanceDataUploadRequest request)
        {
            const int maxRetries = 1; // ✅ 辅助功能，只重试1次
            int retryCount = 0;
                    
            while (retryCount <= maxRetries)
            {
                try
                {
                    // ✅ 使用现有的通信接口发送请求，设置15秒超时（降低超时时间）
                    // ✅ 传递isNonCriticalCommand=true，减少日志输出和重试
                    var response = await _communicationService.SendCommandWithResponseAsync<PerformanceDataUploadResponse>(
                        SystemCommands.PerformanceDataUpload,
                        request,
                        CancellationToken.None,
                        15000, // 从20秒降低到15秒
                        true); // ✅ 标记为非关键命令
        
                    if (response?.IsSuccess == true)
                    {
                        _logger?.LogDebug($"性能数据上报成功: {request.MetricCount} 条指标");
                        return; // 成功则退出
                    }
                    else
                    {
                        // ✅ 失败时只记录Debug日志
                        _logger?.LogDebug($"性能数据上报失败: {response?.ErrorMessage}");
                        if (++retryCount > maxRetries) break;
                    }
                }
                catch (TimeoutException ex)
                {
                    retryCount++;
                    // ✅ 超时也只记录Debug日志
                    _logger?.LogDebug(ex, $"性能数据上报超时 (尝试 {retryCount}/{maxRetries + 1})");
                            
                    if (retryCount > maxRetries)
                    {
                        // ✅ 最终失败也不记录Error，只记录Warning
                        _logger?.LogWarning(ex, "性能数据上报最终失败，已达到最大重试次数");
                        return; // ✅ 不抛出异常，静默失败
                    }
                            
                    // 固定等待2秒
                    await Task.Delay(TimeSpan.FromSeconds(2));
                }
                catch (Exception ex)
                {
                    // ✅ Token相关错误直接返回，不重试
                    if (ex.Message.Contains("授权令牌") || ex.Message.Contains("Token"))
                    {
                        _logger?.LogDebug("Token无效，取消性能数据上报");
                        return;
                    }
                    
                    // ✅ 其他异常也只记录Warning
                    _logger?.LogWarning(ex, "发送性能数据请求失败");
                    return; // ✅ 不抛出异常，静默失败
                }
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
        /// 初始化UI响应监控
        /// </summary>
        private void InitializeUIMonitoring()
        {
            try
            {
                // 检查是否启用UI监控
                if (!PerformanceMonitorSwitch.IsMonitorEnabled(PerformanceMonitorType.UIResponse))
                    return;

                // 使用Application.Idle事件监控UI响应
                System.Windows.Forms.Application.Idle += OnApplicationIdle;
                _logger?.LogDebug("UI响应监控已初始化");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "UI监控初始化失败");
            }
        }

        /// <summary>
        /// Application.Idle事件处理
        /// </summary>
        private void OnApplicationIdle(object sender, EventArgs e)
        {
            // 此事件在应用程序空闲时触发
            // 可用于检测UI线程是否长时间无响应
            // 实际实现需要结合消息钩子来检测卡顿
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
            
            // 取消UI事件订阅
            try
            {
                System.Windows.Forms.Application.Idle -= OnApplicationIdle;
            }
            catch { }

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