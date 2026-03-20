using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RUINORERP.Model;
using RUINORERP.Model.CommonModel;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RUINORERP.Model.Context;
using RUINORERP.Business.BizMapperService;
using RUINORERP.Business.EntityLoadService;

namespace RUINORERP.Business.CommService
{
    /// <summary>
    /// 审计日志服务接口1
    /// </summary>
    public interface IAuditLogService
    {
        Task LogAsync<T>(string action, T entity, string description = "") where T : class;
        Task LogAsync(string action, string description = "");
        AuditLogMetrics GetMetrics();
        Task FlushQueueAsync();
    }

    /// <summary>
    /// 审计日志服务实现 - 优化重构版
    /// </summary>
    public class AuditLogService : IAuditLogService, IDisposable
    {
        private readonly ConcurrentQueue<tb_AuditLogs> _auditLogQueue = new ConcurrentQueue<tb_AuditLogs>();
        private readonly SemaphoreSlim _queueSemaphore;
        private readonly Lazy<IEntityMappingService> _mapper;
        private readonly Lazy<tb_AuditLogsController<tb_AuditLogs>> _AuditLogsController;
        private readonly Timer _flushTimer;
        private readonly AuditLogOptions _options;
        private readonly ILogger<AuditLogService> _logger;
        private readonly AuditLogMetrics _metrics;
        private int _isFlushing = 0;
        public ApplicationContext _appContext;

        /// <summary>
        /// 构造函数
        /// </summary>
        public AuditLogService(
            IOptions<AuditLogOptions> options,
            ILogger<AuditLogService> logger,
            ApplicationContext appContext)
        {
            _appContext = appContext;
            _options = options.Value;
            _logger = logger;

            _options.Validate();
            _metrics = new AuditLogMetrics(logger);
            _queueSemaphore = new SemaphoreSlim(_options.MaxQueueSize, _options.MaxQueueSize);

            _mapper = new Lazy<IEntityMappingService>(
                () => _appContext.GetRequiredService<IEntityMappingService>());

            _AuditLogsController = new Lazy<tb_AuditLogsController<tb_AuditLogs>>(
                () => _appContext.GetRequiredService<tb_AuditLogsController<tb_AuditLogs>>());

            _flushTimer = new Timer(
                callback: FlushQueueCallback,
                state: null,
                dueTime: _options.FlushInterval,
                period: _options.FlushInterval);

            _logger.LogInformation(
                $"审计日志服务已启动 | BatchSize:{_options.BatchSize} | " +
                $"FlushInterval:{_options.FlushInterval}ms | MaxQueueSize:{_options.MaxQueueSize}");
        }

        /// <summary>
        /// 记录实体审计日志（异步）
        /// </summary>
        public async Task LogAsync<T>(string action, T entity, string description = "") where T : class
        {
            if (!_options.EnableAudit) return;
            if (entity == null) return;

            try
            {
                if (!await _queueSemaphore.WaitAsync(TimeSpan.FromMilliseconds(100)))
                {
                    _metrics.QueueSize = _auditLogQueue.Count;
                    _metrics.LogDropped("队列已满");
                    _options.OnLogDropped?.Invoke(action, entity);
                    return;
                }

                try
                {
                    var auditLog = CreateAuditLog(action, entity, description);
                    _auditLogQueue.Enqueue(auditLog);
                    _metrics.LogEnqueue(1);
                    _queueSemaphore.Release();
                    _metrics.QueueSize = _auditLogQueue.Count;

                    if (_auditLogQueue.Count >= _options.BatchSize)
                    {
                        _ = Task.Run(() => FlushQueueAsync());
                    }
                }
                catch
                {
                    _queueSemaphore.Release();
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"创建审计日志失败 | Action:{action}");
            }
        }

        /// <summary>
        /// 记录简单审计日志（异步）
        /// </summary>
        public async Task LogAsync(string action, string description = "")
        {
            if (!_options.EnableAudit) return;

            try
            {
                if (!await _queueSemaphore.WaitAsync(TimeSpan.FromMilliseconds(100)))
                {
                    _metrics.QueueSize = _auditLogQueue.Count;
                    _metrics.LogDropped("队列已满");
                    _options.OnLogDropped?.Invoke(action, description);
                    return;
                }

                try
                {
                    var auditLog = CreateAuditLog(action, description);
                    if (auditLog != null)
                    {
                        _auditLogQueue.Enqueue(auditLog);
                        _metrics.LogEnqueue(1);
                        _queueSemaphore.Release();
                        _metrics.QueueSize = _auditLogQueue.Count;

                        if (_auditLogQueue.Count >= _options.BatchSize)
                        {
                            _ = Task.Run(() => FlushQueueAsync());
                        }
                    }
                    else
                    {
                        _queueSemaphore.Release();
                    }
                }
                catch
                {
                    _queueSemaphore.Release();
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"创建审计日志失败 | Action:{action}");
            }
        }

        /// <summary>
        /// 获取性能监控指标
        /// </summary>
        public AuditLogMetrics GetMetrics()
        {
            _metrics.QueueSize = _auditLogQueue.Count;
            return _metrics;
        }

        private tb_AuditLogs CreateAuditLog<T>(string action, T entity, string description) where T : class
        {
            if (_appContext.CurUserInfo == null)
            {
                _logger.LogWarning("当前用户信息为空，跳过审计日志记录");
                return null;
            }

            var auditLog = new tb_AuditLogs
            {
                UserName = _appContext.CurUserInfo.UserInfo.UserName,
                Employee_ID = _appContext.CurUserInfo.UserInfo.Employee_ID,
                ActionType = action,
                ActionTime = DateTime.Now,
                Notes = description
            };

            try
            {
                var bizType = _mapper.Value.GetBizType(typeof(T), entity);
                auditLog.ObjectType = (int)bizType;

                var (idField, noField) = _mapper.Value.GetIdAndName(entity);
                auditLog.ObjectId = idField;
                auditLog.ObjectNo = noField;

                var dataContent = EntityDataExtractor.ExtractDataContent(entity);
                auditLog.DataContent = dataContent;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取审计对象信息失败");
                auditLog.Notes += $" | 获取对象信息失败: {ex.Message}";
            }

            return auditLog;
        }

        private tb_AuditLogs CreateAuditLog(string action, string description)
        {
            if (_appContext.CurUserInfo == null ||
                string.IsNullOrEmpty(_appContext.CurUserInfo.UserInfo.UserName))
            {
                return null;
            }

            return new tb_AuditLogs
            {
                UserName = _appContext.CurUserInfo.UserInfo.UserName,
                Employee_ID = _appContext.CurUserInfo.UserInfo.Employee_ID,
                ActionType = action,
                ActionTime = DateTime.Now,
                Notes = description,
                ObjectType = -1
            };
        }

        private void FlushQueueCallback(object state)
        {
            _ = Task.Run(async () =>
            {
                try
                {
                    await FlushQueueAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "定时刷新审计日志队列失败");
                }
            });
        }

        public async Task FlushQueueAsync()
        {
            if (Interlocked.CompareExchange(ref _isFlushing, 1, 0) != 0)
            {
                return;
            }

            try
            {
                _metrics.LogFlushStart();

                if (_auditLogQueue.IsEmpty) return;

                var logsToSave = new List<tb_AuditLogs>();
                int count = 0;

                while (count < _options.BatchSize && _auditLogQueue.TryDequeue(out var log))
                {
                    logsToSave.Add(log);
                    count++;
                }

                if (logsToSave.Any())
                {
                    await FlushBatchToDatabaseAsync(logsToSave);
                }

                if (_options.EnableMetrics)
                {
                    var alert = _metrics.CheckAlerts(_options.MaxQueueSize);
                    if (!string.IsNullOrEmpty(alert))
                    {
                        _logger.LogWarning($"审计日志告警: {alert}");
                    }
                }
            }
            finally
            {
                Interlocked.Exchange(ref _isFlushing, 0);
            }
        }

        private async Task FlushBatchToDatabaseAsync(List<tb_AuditLogs> logs)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            int retryCount = 0;

            while (retryCount <= _options.MaxRetryCount)
            {
                try
                {
                    using var cts = new CancellationTokenSource(_options.FlushTimeout);

                    await _appContext.Db.CopyNew()
                        .Insertable(logs)
                        .ExecuteReturnSnowflakeIdListAsync(cts.Token);

                    stopwatch.Stop();
                    _metrics.LogFlushSuccess(stopwatch.Elapsed, logs.Count);
                    return;
                }
                catch (OperationCanceledException) when (stopwatch.ElapsedMilliseconds >= _options.FlushTimeout)
                {
                    _metrics.LogFlushFailure(
                        new TimeoutException($"刷新操作超时 ({_options.FlushTimeout}ms)"));
                    retryCount++;

                    if (retryCount > _options.MaxRetryCount)
                    {
                        _logger.LogError(
                            $"审计日志刷新超时，已达到最大重试次数 ({_options.MaxRetryCount})，丢弃{logs.Count}条日志");
                        break;
                    }

                    await Task.Delay(TimeSpan.FromMilliseconds(100 * (1 << Math.Min(retryCount, 5))));
                }
                catch (Exception ex)
                {
                    _metrics.LogFlushFailure(ex);
                    retryCount++;

                    if (retryCount > _options.MaxRetryCount)
                    {
                        _logger.LogError(
                            ex,
                            $"审计日志刷新失败，已达到最大重试次数 ({_options.MaxRetryCount})，丢弃{logs.Count}条日志");
                        break;
                    }

                    await Task.Delay(TimeSpan.FromMilliseconds(100 * (1 << Math.Min(retryCount, 5))));
                }
            }

            foreach (var log in logs)
            {
                var validator = _AuditLogsController.Value.BaseValidator(log);
                if (validator.IsValid)
                {
                    _auditLogQueue.Enqueue(log);
                }
                else
                {
                    _metrics.LogDropped($"验证失败: {string.Join(", ", validator.Errors)}");
                }
            }
        }

        public void Dispose()
        {
            _flushTimer?.Dispose();
            _queueSemaphore?.Dispose();

            try
            {
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
                FlushQueueAsync().Wait(cts.Token);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Dispose时刷新审计日志队列失败");
            }

            if (_options.EnableMetrics)
            {
                _logger.LogInformation(_metrics.GetPerformanceReport());
            }
        }
    }

    /// <summary>
    /// 财务审计日志服务接口
    /// </summary>
    public interface IFMAuditLogService
    {
        Task LogAsync<T>(string action, T entity, string description = "") where T : class;
        Task LogAsync(string action, string description = "");
        AuditLogMetrics GetMetrics();
        Task FlushQueueAsync();
    }

    /// <summary>
    /// 财务审计日志服务实现 - 优化重构版
    /// </summary>
    public class FMAuditLogService : IFMAuditLogService, IDisposable
    {
        private readonly ConcurrentQueue<tb_FM_AuditLogs> _auditLogQueue = new ConcurrentQueue<tb_FM_AuditLogs>();
        private readonly SemaphoreSlim _queueSemaphore;
        private readonly Lazy<IEntityMappingService> _mapper;
        private readonly Lazy<tb_FM_AuditLogsController<tb_FM_AuditLogs>> _AuditLogsController;
        private readonly Timer _flushTimer;
        private readonly AuditLogOptions _options;
        private readonly ILogger<FMAuditLogService> _logger;
        private readonly AuditLogMetrics _metrics;
        private int _isFlushing = 0;
        public ApplicationContext _appContext;

        /// <summary>
        /// 构造函数
        /// </summary>
        public FMAuditLogService(
            IOptions<AuditLogOptions> options,
            ILogger<FMAuditLogService> logger,
            ApplicationContext appContext)
        {
            _appContext = appContext;
            _options = options.Value;
            _logger = logger;

            _options.Validate();
            _metrics = new AuditLogMetrics(logger);
            _queueSemaphore = new SemaphoreSlim(_options.MaxQueueSize, _options.MaxQueueSize);

            _mapper = new Lazy<IEntityMappingService>(
                () => _appContext.GetRequiredService<IEntityMappingService>());

            _AuditLogsController = new Lazy<tb_FM_AuditLogsController<tb_FM_AuditLogs>>(
                () => appContext.GetRequiredService<tb_FM_AuditLogsController<tb_FM_AuditLogs>>());

            _flushTimer = new Timer(
                callback: FlushQueueCallback,
                state: null,
                dueTime: _options.FlushInterval,
                period: _options.FlushInterval);

            _logger.LogInformation(
                $"财务审计日志服务已启动 | BatchSize:{_options.BatchSize} | " +
                $"FlushInterval:{_options.FlushInterval}ms | MaxQueueSize:{_options.MaxQueueSize}");
        }

        /// <summary>
        /// 记录实体审计日志（异步）
        /// </summary>
        public async Task LogAsync<T>(string action, T entity, string description = "") where T : class
        {
            if (!_options.EnableAudit) return;

            try
            {
                if (!await _queueSemaphore.WaitAsync(TimeSpan.FromMilliseconds(100)))
                {
                    _metrics.QueueSize = _auditLogQueue.Count;
                    _metrics.LogDropped("队列已满");
                    _options.OnLogDropped?.Invoke(action, entity);
                    return;
                }

                try
                {
                    var auditLog = CreateAuditLog(action, entity, description);
                    _auditLogQueue.Enqueue(auditLog);
                    _metrics.LogEnqueue(1);
                    _queueSemaphore.Release();
                    _metrics.QueueSize = _auditLogQueue.Count;

                    if (_auditLogQueue.Count >= _options.BatchSize)
                    {
                        _ = Task.Run(() => FlushQueueAsync());
                    }
                }
                catch
                {
                    _queueSemaphore.Release();
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"创建财务审计日志失败 | Action:{action}");
            }
        }

        /// <summary>
        /// 记录简单审计日志（异步）
        /// </summary>
        public async Task LogAsync(string action, string description = "")
        {
            if (!_options.EnableAudit) return;

            try
            {
                if (!await _queueSemaphore.WaitAsync(TimeSpan.FromMilliseconds(100)))
                {
                    _metrics.QueueSize = _auditLogQueue.Count;
                    _metrics.LogDropped("队列已满");
                    _options.OnLogDropped?.Invoke(action, description);
                    return;
                }

                try
                {
                    var auditLog = CreateAuditLog(action, description);
                    if (auditLog != null)
                    {
                        _auditLogQueue.Enqueue(auditLog);
                        _metrics.LogEnqueue(1);
                        _queueSemaphore.Release();
                        _metrics.QueueSize = _auditLogQueue.Count;

                        if (_auditLogQueue.Count >= _options.BatchSize)
                        {
                            _ = Task.Run(() => FlushQueueAsync());
                        }
                    }
                    else
                    {
                        _queueSemaphore.Release();
                    }
                }
                catch
                {
                    _queueSemaphore.Release();
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"创建财务审计日志失败 | Action:{action}");
            }
        }

        /// <summary>
        /// 获取性能监控指标
        /// </summary>
        public AuditLogMetrics GetMetrics()
        {
            _metrics.QueueSize = _auditLogQueue.Count;
            return _metrics;
        }

        private tb_FM_AuditLogs CreateAuditLog<T>(string action, T entity, string description) where T : class
        {
            if (_appContext.CurUserInfo == null)
            {
                _logger.LogWarning("当前用户信息为空，跳过财务审计日志记录");
                return null;
            }

            var auditLog = new tb_FM_AuditLogs
            {
                UserName = _appContext.CurUserInfo.UserInfo.UserName,
                Employee_ID = _appContext.CurUserInfo.UserInfo.Employee_ID,
                ActionType = action,
                ActionTime = DateTime.Now,
                Notes = description
            };

            try
            {
                var bizType = _mapper.Value.GetBizType(typeof(T), entity);
                auditLog.ObjectType = (int)bizType;

                var (idField, noField) = _mapper.Value.GetIdAndName(entity);
                auditLog.ObjectId = idField;
                auditLog.ObjectNo = noField;

                var dataContent = EntityDataExtractor.ExtractDataContent(entity);
                auditLog.DataContent = dataContent;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取财务审计对象信息失败");
                auditLog.Notes += $" | 获取对象信息失败: {ex.Message}";
            }

            return auditLog;
        }

        private tb_FM_AuditLogs CreateAuditLog(string action, string description)
        {
            if (_appContext.CurUserInfo == null ||
                string.IsNullOrEmpty(_appContext.CurUserInfo.UserInfo.UserName))
            {
                return null;
            }

            return new tb_FM_AuditLogs
            {
                UserName = _appContext.CurUserInfo.UserInfo.UserName,
                Employee_ID = _appContext.CurUserInfo.UserInfo.Employee_ID,
                ActionType = action,
                ActionTime = DateTime.Now,
                Notes = description,
                ObjectType = -1
            };
        }

        private void FlushQueueCallback(object state)
        {
            _ = Task.Run(async () =>
            {
                try
                {
                    await FlushQueueAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "定时刷新财务审计日志队列失败");
                }
            });
        }

        public async Task FlushQueueAsync()
        {
            if (Interlocked.CompareExchange(ref _isFlushing, 1, 0) != 0)
            {
                return;
            }

            try
            {
                _metrics.LogFlushStart();

                if (_auditLogQueue.IsEmpty) return;

                var logsToSave = new List<tb_FM_AuditLogs>();
                int count = 0;

                while (count < _options.BatchSize && _auditLogQueue.TryDequeue(out var log))
                {
                    logsToSave.Add(log);
                    count++;
                }

                if (logsToSave.Any())
                {
                    await FlushBatchToDatabaseAsync(logsToSave);
                }

                if (_options.EnableMetrics)
                {
                    var alert = _metrics.CheckAlerts(_options.MaxQueueSize);
                    if (!string.IsNullOrEmpty(alert))
                    {
                        _logger.LogWarning($"财务审计日志告警: {alert}");
                    }
                }
            }
            finally
            {
                Interlocked.Exchange(ref _isFlushing, 0);
            }
        }

        private async Task FlushBatchToDatabaseAsync(List<tb_FM_AuditLogs> logs)
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            int retryCount = 0;

            while (retryCount <= _options.MaxRetryCount)
            {
                try
                {
                    using var cts = new CancellationTokenSource(_options.FlushTimeout);

                    await _appContext.Db.CopyNew()
                        .Insertable(logs)
                        .ExecuteReturnSnowflakeIdListAsync(cts.Token);

                    stopwatch.Stop();
                    _metrics.LogFlushSuccess(stopwatch.Elapsed, logs.Count);
                    return;
                }
                catch (OperationCanceledException) when (stopwatch.ElapsedMilliseconds >= _options.FlushTimeout)
                {
                    _metrics.LogFlushFailure(
                        new TimeoutException($"刷新操作超时 ({_options.FlushTimeout}ms)"));
                    retryCount++;

                    if (retryCount > _options.MaxRetryCount)
                    {
                        _logger.LogError(
                            $"财务审计日志刷新超时，已达到最大重试次数 ({_options.MaxRetryCount})，丢弃{logs.Count}条日志");
                        break;
                    }

                    await Task.Delay(TimeSpan.FromMilliseconds(100 * (1 << Math.Min(retryCount, 5))));
                }
                catch (Exception ex)
                {
                    _metrics.LogFlushFailure(ex);
                    retryCount++;

                    if (retryCount > _options.MaxRetryCount)
                    {
                        _logger.LogError(
                            ex,
                            $"财务审计日志刷新失败，已达到最大重试次数 ({_options.MaxRetryCount})，丢弃{logs.Count}条日志");
                        break;
                    }

                    await Task.Delay(TimeSpan.FromMilliseconds(100 * (1 << Math.Min(retryCount, 5))));
                }
            }

            foreach (var log in logs)
            {
                var validator = _AuditLogsController.Value.BaseValidator(log);
                if (validator.IsValid)
                {
                    _auditLogQueue.Enqueue(log);
                }
                else
                {
                    _metrics.LogDropped($"验证失败: {string.Join(", ", validator.Errors)}");
                }
            }
        }

        public void Dispose()
        {
            _flushTimer?.Dispose();
            _queueSemaphore?.Dispose();

            try
            {
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
                FlushQueueAsync().Wait(cts.Token);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Dispose时刷新财务审计日志队列失败");
            }

            if (_options.EnableMetrics)
            {
                _logger.LogInformation(_metrics.GetPerformanceReport());
            }
        }
    }

    /// <summary>
    /// 审计日志帮助类（适配原有接口，逐步迁移到服务）
    /// </summary>
    public class FMAuditLogHelper
    {
        private readonly IFMAuditLogService _auditLogService;

        /// <summary>
        /// 构造函数
        /// </summary>
        public FMAuditLogHelper(IFMAuditLogService auditLogService)
        {
            _auditLogService = auditLogService;
        }

        public async Task CreateAuditLog<T>(string action, T entity, string description) where T : class
        {
            try
            {
                await _auditLogService.LogAsync(action, entity, description);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"FMAuditLogHelper.CreateAuditLog失败: {ex.Message}");
            }
        }

        public void CreateAuditLog<T>(string action, T entity) where T : class
        {
            _ = Task.Run(() => CreateAuditLog(action, entity, ""));
        }

        public async Task CreateAuditLog(string action, string description)
        {
            try
            {
                await _auditLogService.LogAsync(action, description);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"FMAuditLogHelper.CreateAuditLog失败: {ex.Message}");
            }
        }
    }
}
