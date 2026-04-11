using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Linq;
using System.Data;
using System.Data.Common;
using Microsoft.Extensions.Logging;
using SqlSugar;
using RUINORERP.Common.DI;
using RUINORERP.Model.Context;
using System.Data.SqlClient;

namespace RUINORERP.Repository.UnitOfWorks
{


    /// <summary>
    /// 事务管理类，确认后的优化版本
    /// </summary>
    public class UnitOfWorkManage : IUnitOfWorkManage, IDependencyRepository, IDisposable
    {
        private readonly ILogger<UnitOfWorkManage> _logger;
        private readonly ISqlSugarClient _sqlSugarClient;
        private readonly ApplicationContext _appContext;

        // 读写锁：允许并发读，独占写，优化高并发性能
        private readonly ReaderWriterLockSlim _rwLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);

        // 使用 AsyncLocal 保证异步安全
        private readonly AsyncLocal<TransactionContext> _currentTransactionContext =
            new AsyncLocal<TransactionContext>();

        // 每个异步上下文独立的连接实例（关键修复！替换ThreadLocal为AsyncLocal）
        private readonly AsyncLocal<ISqlSugarClient> _asyncLocalClient =
            new AsyncLocal<ISqlSugarClient>();

        // 简单的嵌套计数器（兼容旧代码）
        private readonly AsyncLocal<int> _tranDepth = new AsyncLocal<int>();
        
        // 默认事务超时时间（秒）
        private const int DEFAULT_TRANSACTION_TIMEOUT = 60;
        
        // 最大重试次数（针对死锁等瞬态故障）
        private const int MAX_RETRY_COUNT = 3;

        public UnitOfWorkManage(ISqlSugarClient sqlSugarClient, ILogger<UnitOfWorkManage> logger, ApplicationContext appContext = null)
        {
            _sqlSugarClient = sqlSugarClient ?? throw new ArgumentNullException(nameof(sqlSugarClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _appContext = appContext;

            _logger.LogDebug($"UnitOfWorkManage 已创建，线程ID: {Thread.CurrentThread.ManagedThreadId}");
        }

        /// <summary>
        /// 获取异步上下文独立的数据库客户端 - 不持有锁，直接返回
        /// 注意：此方法在事务操作前调用，不与事务锁冲突
        /// </summary>
        public ISqlSugarClient GetDbClient()
        {
            // 每个异步上下文使用独立的连接实例
            // 注意：此处不再加锁，因为每个AsyncLocal上下文是独立的，
            // 不存在并发访问同一个AsyncLocal.Value的情况
            // 锁的职责交给需要事务保护的方法（BeginTran/CommitTran/RollbackTran）
            if (_asyncLocalClient.Value == null)
            {
                // 复制原始连接配置，创建新的连接实例
                var originalConfig = (_sqlSugarClient as SqlSugarScope)?.CurrentConnectionConfig;
        
                if (originalConfig != null)
                {
                    var newConfig = new ConnectionConfig
                    {
                        ConnectionString = originalConfig.ConnectionString,
                        DbType = (SqlSugar.DbType)originalConfig.DbType,
                        IsAutoCloseConnection = false,
                        InitKeyType = originalConfig.InitKeyType,
                        MoreSettings = originalConfig.MoreSettings,
                        ConfigureExternalServices = originalConfig.ConfigureExternalServices,
                        AopEvents = originalConfig.AopEvents
                    };
        
                    _asyncLocalClient.Value = new SqlSugarClient(newConfig);
                }
                else
                {
                    _asyncLocalClient.Value = new SqlSugarClient(new ConnectionConfig
                    {
                        ConnectionString = _sqlSugarClient.Ado.Connection.ConnectionString,
                        DbType = SqlSugar.DbType.SqlServer,
                        IsAutoCloseConnection = false,
                        InitKeyType = InitKeyType.Attribute
                    });
                }
        
                _logger.LogDebug($"为异步上下文创建了新的数据库连接实例，线程 ID: {Thread.CurrentThread.ManagedThreadId}");
            }
        
            return _asyncLocalClient.Value;
        }

        /// <summary>
        /// 开始事务 - 使用写锁确保独占访问
        /// </summary>
        /// <param name="isolationLevel">可选的隔离级别，不指定则使用数据库默认</param>
        public void BeginTran(IsolationLevel? isolationLevel = null)
        {
            var context = GetOrCreateTransactionContext();
            var dbClient = GetDbClient();
        
            // 使用写锁保护事务开始操作
            _rwLock.EnterWriteLock();
            try
            {
                // 检查嵌套深度
                if (context.Depth >= 15) // 限制最大嵌套深度为15，满足复杂ERP业务场景需求
                {
                    throw new InvalidOperationException(
                        $"[Transaction-{context.TransactionId}] 事务嵌套深度超过最大限制 (15)，请检查业务逻辑是否存在循环调用或过度嵌套");
                }

                // 增加事务深度
                context.Depth++;
                _tranDepth.Value++;

                // 超过10层时记录警告日志，便于后续优化
                if (context.Depth > 10)
                {
                    _logger.LogWarning($"[Transaction-{context.TransactionId}] 事务嵌套深度已达到 {context.Depth} 层，超过建议的10层限制，建议检查业务逻辑是否可以优化");
                }
        
                // 最外层事务：开启物理事务
                if (context.Depth == 1)
                {
                    // 确保连接是打开的
                    if (dbClient.Ado.Connection.State != System.Data.ConnectionState.Open)
                    {
                        dbClient.Ado.Connection.Open();
                    }
        
                    // 设置命令超时时间
                    dbClient.Ado.CommandTimeOut = DEFAULT_TRANSACTION_TIMEOUT;
                                
                    // 根据隔离级别开启事务
                    if (isolationLevel.HasValue)
                    {
                        dbClient.Ado.BeginTran(isolationLevel.Value);
                        _logger.LogInformation($"[Transaction-{context.TransactionId}] 事务已开启，隔离级别：{isolationLevel.Value}，线程 ID: {Thread.CurrentThread.ManagedThreadId}，调用栈：{context.CallerMethod}");
                    }
                    else
                    {
                        dbClient.Ado.BeginTran();
                        _logger.LogInformation($"[Transaction-{context.TransactionId}] 事务已开启，线程 ID: {Thread.CurrentThread.ManagedThreadId}，调用栈：{context.CallerMethod}");
                    }
                }
                // 嵌套事务：使用保存点（SQL Server 支持）
                else
                {
                    var savePointName = $"SP_{context.TransactionId.ToString("N").Substring(0, 8)}_{context.Depth}";
                    dbClient.Ado.ExecuteCommand($"SAVE TRANSACTION {savePointName}");
                    context.SavePointStack.Push(savePointName);
                    _logger.LogDebug($"[Transaction-{context.TransactionId}] 创建保存点：{savePointName}");
                }
        
                context.Status = TransactionStatus.Active;
                context.UpdateActivityTime();
            }
            catch (Exception ex)
            {
                // 回滚深度计数
                context.Depth = Math.Max(0, context.Depth - 1);
                _tranDepth.Value = Math.Max(0, _tranDepth.Value - 1);
        
                _logger.LogError(ex, $"[Transaction-{context.TransactionId}] 事务开启失败");
                throw new InvalidOperationException("事务开启失败", ex);
            }
            finally
            {
                _rwLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public void CommitTran() => CommitTranInternal();
                
        /// <summary>
        /// 提交事务内部实现 - 使用写锁保护
        /// </summary>
        private void CommitTranInternal()
        {
            var context = CurrentTransactionContext;
            if (context == null)
            {
                _logger.LogWarning("[CommitTran] 提交请求但无事务上下文");
                return;
            }
                
            var dbClient = GetDbClient();
            var stopwatch = Stopwatch.StartNew(); // 性能监控开始
                
            // 使用写锁保护事务提交操作
            _rwLock.EnterWriteLock();
            try
            {
                if (context.Depth <= 0)
                {
                    _logger.LogWarning($"[Transaction-{context.TransactionId}] 提交请求但事务深度为 {context.Depth}，跳过提交");
                    ResetTransactionState();
                    return;
                }
                
                // 检查回滚标记
                if (context.ShouldRollback)
                {
                    _logger.LogWarning($"[Transaction-{context.TransactionId}] 事务已标记为回滚，跳过提交");
                    context.Depth--;
                    _tranDepth.Value--;
                    if (context.Depth == 0)
                    {
                        ResetTransactionState();
                    }
                    return;
                }
                
                // 减少深度
                context.Depth--;
                _tranDepth.Value--;
                context.UpdateActivityTime();
                
                _logger.LogDebug($"[Transaction-{context.TransactionId}] 提交操作：Depth={context.Depth}");
                
                // 内层提交：仅更新深度
                if (context.Depth > 0)
                {
                    return;
                }
                
                // 最外层提交
                try
                {
                    // ✅ 防御性检查：验证事务对象状态
                    if (dbClient.Ado.Transaction == null)
                    {
                        _logger.LogWarning($"[Transaction-{context.TransactionId}] 事务对象已为空，无法提交");
                        context.Status = TransactionStatus.Committed;
                    }
                    else
                    {
                        var transactionConnection = dbClient.Ado.Transaction.Connection;
                        if (transactionConnection == null || transactionConnection.State != System.Data.ConnectionState.Open)
                        {
                            _logger.LogWarning($"[Transaction-{context.TransactionId}] 事务连接已关闭或无效，无法提交");
                            context.Status = TransactionStatus.Committed;
                        }
                        else
                        {
                            dbClient.Ado.CommitTran();
                            context.Status = TransactionStatus.Committed;
                            _logger.LogInformation($"[Transaction-{context.TransactionId}] 事务提交成功");
                        }
                    }
                    
                    var duration = context.GetDuration().TotalSeconds;
                    if (duration > 10)
                    {
                        _logger.LogWarning($"[Transaction-{context.TransactionId}] 长事务提交耗时：{duration:F2}秒");
                    }
                            
                    // ✅ 性能监控：记录事务指标
                    TransactionMetrics.RecordTransaction(
                        "commit", 
                        context.CallerMethod, 
                        duration, 
                        true,
                        ExtractTableName(context)); // 从事务上下文中提取表名
                }
                catch (InvalidOperationException invEx) when (invEx.Message.Contains("已完成") || invEx.Message.Contains("Zombie"))
                {
                    // ✅ 关键修复：捕获 "事务已完成" 异常
                    _logger.LogWarning(invEx, $"[Transaction-{context.TransactionId}] 事务已完成（可能已被其他地方处理），忽略此异常");
                    context.Status = TransactionStatus.Committed;
                    
                    // ✅ 性能监控：记录事务
                    TransactionMetrics.RecordTransaction(
                        "commit", 
                        context.CallerMethod, 
                        stopwatch.Elapsed.TotalSeconds, 
                        true);
                }
                catch (Exception commitEx)
                {
                    context.Status = TransactionStatus.RolledBack;
                    _logger.LogError(commitEx, $"[Transaction-{context.TransactionId}] 事务提交失败");
                            
                    // ✅ 性能监控：记录失败事务
                    TransactionMetrics.RecordTransaction(
                        "commit", 
                        context.CallerMethod, 
                        stopwatch.Elapsed.TotalSeconds, 
                        false);
                            
                    throw;
                }
                finally
                {
                    // ✅ 关键修复：无论成功与否，都要重置事务状态
                    ResetTransactionState();
                }
            }
            catch (Exception ex)
            {
                context.Status = TransactionStatus.RolledBack;
                ForceRollback(dbClient);
                        
                // ✅ 性能监控：记录异常事务
                TransactionMetrics.RecordTransaction(
                    "commit", 
                    context.CallerMethod, 
                    stopwatch.Elapsed.TotalSeconds, 
                    false);
                        
                throw new InvalidOperationException($"事务提交失败：{ex.Message}", ex);
            }
            finally
            {
                _rwLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// 回滚事务 - 使用写锁保护
        /// </summary>
        public void RollbackTran()
        {
            var context = CurrentTransactionContext;
            if (context == null)
            {
                _logger.LogWarning("回滚请求但无事务上下文");
                return;
            }
        
            var dbClient = GetDbClient();
            var stopwatch = Stopwatch.StartNew(); // 性能监控开始
        
            // 使用写锁保护事务回滚操作
            _rwLock.EnterWriteLock();
            try
            {
                if (context.Depth <= 0)
                {
                    _logger.LogWarning($"[Transaction-{context.TransactionId}] 回滚请求但事务深度为 {context.Depth}，跳过回滚");
                    ResetTransactionState();
                    return;
                }
        
                // 设置回滚标记
                context.ShouldRollback = true;
                context.UpdateActivityTime();
        
                // 减少深度
                context.Depth--;
                _tranDepth.Value--;
        
                _logger.LogDebug($"[Transaction-{context.TransactionId}] 回滚操作：Depth={context.Depth}, ShouldRollback={context.ShouldRollback}");
        
                // 最外层回滚
                if (context.Depth == 0)
                {
                    try
                    {
                        // ✅ 防御性检查：验证事务对象状态
                        if (dbClient.Ado.Transaction == null)
                        {
                            _logger.LogWarning($"[Transaction-{context.TransactionId}] 事务对象已为空，跳过回滚");
                            context.Status = TransactionStatus.RolledBack;
                        }
                        else
                        {
                            // 检查事务是否已经完成（提交或回滚）
                            var transactionConnection = dbClient.Ado.Transaction.Connection;
                            if (transactionConnection == null || transactionConnection.State != System.Data.ConnectionState.Open)
                            {
                                _logger.LogWarning($"[Transaction-{context.TransactionId}] 事务连接已关闭或无效，跳过回滚");
                                context.Status = TransactionStatus.RolledBack;
                            }
                            else
                            {
                                dbClient.Ado.RollbackTran();
                                context.Status = TransactionStatus.RolledBack;
                                _logger.LogInformation($"[Transaction-{context.TransactionId}] 事务回滚成功");
                            }
                        }
                        
                        // ✅ 性能监控：记录回滚事务
                        TransactionMetrics.RecordTransaction(
                            "rollback", 
                            context.CallerMethod, 
                            stopwatch.Elapsed.TotalSeconds, 
                            false,
                            ExtractTableName(context));
                    }
                    catch (InvalidOperationException invEx) when (invEx.Message.Contains("已完成") || invEx.Message.Contains("Zombie"))
                    {
                        // ✅ 关键修复：捕获 "事务已完成" 异常，这是正常现象（可能已被其他地方回滚）
                        _logger.LogWarning(invEx, $"[Transaction-{context.TransactionId}] 事务已完成（可能已被其他地方回滚），忽略此异常");
                        context.Status = TransactionStatus.RolledBack;
                        
                        // ✅ 性能监控：记录回滚事务
                        TransactionMetrics.RecordTransaction(
                            "rollback", 
                            context.CallerMethod, 
                            stopwatch.Elapsed.TotalSeconds, 
                            false);
                    }
                    catch (Exception rollbackEx)
                    {
                        _logger.LogError(rollbackEx, $"[Transaction-{context.TransactionId}] 事务回滚失败");
                        
                        // ✅ 性能监控：记录失败回滚
                        TransactionMetrics.RecordTransaction(
                            "rollback", 
                            context.CallerMethod, 
                            stopwatch.Elapsed.TotalSeconds, 
                            false);
                        
                        throw;
                    }
                    finally
                    {
                        // ✅ 关键修复：无论成功与否，都要重置事务状态
                        ResetTransactionState();
                    }
                }
                // 嵌套事务：回滚到保存点
                else if (context.SavePointStack.TryPop(out var savePoint))
                {
                    try
                    {
                        dbClient.Ado.ExecuteCommand($"ROLLBACK TRANSACTION {savePoint}");
                        _logger.LogDebug($"[Transaction-{context.TransactionId}] 保存点回滚成功：{savePoint}");
                    }
                    catch (Exception savePointEx)
                    {
                        _logger.LogError(savePointEx, $"[Transaction-{context.TransactionId}] 保存点回滚失败：{savePoint}");
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[Transaction-{context?.TransactionId.ToString() ?? "Unknown"}] RollbackTran 外层异常");
                ForceRollback(dbClient);
                
                // ✅ 性能监控：记录异常回滚
                TransactionMetrics.RecordTransaction(
                    "rollback", 
                    context.CallerMethod, 
                    stopwatch.Elapsed.TotalSeconds, 
                    false);
                
                throw new InvalidOperationException($"事务回滚失败：{ex.Message}", ex);
            }
            finally
            {
                _rwLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// 强制回滚
        /// </summary>
        private void ForceRollback(ISqlSugarClient dbClient)
        {
            var context = CurrentTransactionContext;
            var txId = context?.TransactionId.ToString() ?? "Unknown";
            try
            {
                if (dbClient.Ado.Transaction == null)
                {
                    _logger.LogWarning($"[Transaction-{txId}] 强制回滚：事务对象已为空");
                }
                else
                {
                    var transactionConnection = dbClient.Ado.Transaction.Connection;
                    if (transactionConnection == null || transactionConnection.State != System.Data.ConnectionState.Open)
                    {
                        _logger.LogWarning($"[Transaction-{txId}] 强制回滚：事务连接已关闭或无效");
                    }
                    else
                    {
                        dbClient.Ado.RollbackTran();
                        _logger.LogWarning($"[Transaction-{txId}] 强制回滚已执行");
                    }
                }
            }
            catch (InvalidOperationException invEx) when (invEx.Message.Contains("已完成") || invEx.Message.Contains("Zombie"))
            {
                _logger.LogWarning(invEx, $"[Transaction-{txId}] 强制回滚：事务已完成，忽略此异常");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[Transaction-{txId}] 强制回滚失败");
            }
            finally
            {
                ResetTransactionState();
            }
        }
        
        /// <summary>
        /// 带重试的执行方法（用于处理死锁等瞬态故障）
        /// </summary>
        /// <param name="action">要执行的操作</param>
        /// <param name="maxRetryCount">最大重试次数，默认 3 次</param>
        public void ExecuteWithRetry(Action action, int maxRetryCount = MAX_RETRY_COUNT)
        {
            int retryCount = 0;
            var context = CurrentTransactionContext;
            
            while (true)
            {
                try
                {
                    action();
                    return;
                }
                catch (SqlException sqlEx) when (sqlEx.Number == 1205 && retryCount < maxRetryCount)
                {
                    retryCount++;
                    var delayMs = (int)(100 * Math.Pow(2, retryCount)); // 指数退避：200ms, 400ms, 800ms
                    
                    _logger.LogWarning(sqlEx, 
                        $"[Transaction-{context?.TransactionId}] 检测到数据库死锁，正在进行第 {retryCount}/{maxRetryCount} 次重试，延迟 {delayMs}ms...");
                    
                    // 记录死锁上下文信息
                    if (context != null)
                    {
                        _logger.LogWarning($"死锁事务上下文：{context.GetDebugInfo()}");
                    }
                    
                    Thread.Sleep(delayMs);
                }
                catch (Exception ex)
                {
                    // 非死锁异常或其他异常，直接抛出
                    _logger.LogError(ex, $"[Transaction-{context?.TransactionId}] 执行失败，不再重试");
                    throw;
                }
            }
        }
        
        /// <summary>
        /// 异步版本的带重试执行方法
        /// </summary>
        /// <param name="action">要执行的操作</param>
        /// <param name="maxRetryCount">最大重试次数，默认 3 次</param>
        public async Task ExecuteWithRetryAsync(Func<Task> action, int maxRetryCount = MAX_RETRY_COUNT)
        {
            int retryCount = 0;
            var context = CurrentTransactionContext;
            
            while (true)
            {
                try
                {
                    await action();
                    return;
                }
                catch (SqlException sqlEx) when (sqlEx.Number == 1205 && retryCount < maxRetryCount)
                {
                    retryCount++;
                    var delayMs = (int)(100 * Math.Pow(2, retryCount)); // 指数退避：200ms, 400ms, 800ms
                    
                    _logger.LogWarning(sqlEx, 
                        $"[Transaction-{context?.TransactionId}] 检测到数据库死锁，正在进行第 {retryCount}/{maxRetryCount} 次重试，延迟 {delayMs}ms...");
                    
                    // 记录死锁上下文信息
                    if (context != null)
                    {
                        _logger.LogWarning($"死锁事务上下文：{context.GetDebugInfo()}");
                    }
                    
                    await Task.Delay(delayMs);
                }
                catch (Exception ex)
                {
                    // 非死锁异常或其他异常，直接抛出
                    _logger.LogError(ex, $"[Transaction-{context?.TransactionId}] 执行失败，不再重试");
                    throw;
                }
            }
        }

        /// <summary>
        /// 获取或创建事务上下文
        /// </summary>
        private TransactionContext GetOrCreateTransactionContext()
        {
            var context = _currentTransactionContext.Value;
            if (context == null)
            {
                context = new TransactionContext
                {
                    CallerMethod = GetCallerMethod(),
                    StackTrace = Environment.StackTrace
                };
                _currentTransactionContext.Value = context;
            }
            return context;
        }

        /// <summary>
        /// 获取调用方法信息
        /// </summary>
        private string GetCallerMethod()
        {
            try
            {
                var stackTrace = new StackTrace(2, false);
                var frame = stackTrace.GetFrames()
                    ?.FirstOrDefault(f =>
                    {
                        var methodInfo = f.GetMethod();
                        if (methodInfo == null) return false;
                        var declaringType = methodInfo.DeclaringType?.FullName;
                        return !string.IsNullOrEmpty(declaringType) &&
                               !declaringType.StartsWith("RUINORERP.Repository.UnitOfWorks") &&
                               !declaringType.StartsWith("System") &&
                               !declaringType.StartsWith("Microsoft");
                    });
                if (frame?.GetMethod() is MethodInfo method)
                {
                    return $"{method.DeclaringType?.Name}.{method.Name}";
                }
            }
            catch
            {
                // 忽略异常
            }

            return "UnknownMethod";
        }

        /// <summary>
        /// 重置事务状态
        /// </summary>
        private void ResetTransactionState()
        {
            var context = _currentTransactionContext.Value;
            if (context != null)
            {
                var transactionId = context.TransactionId;
                var duration = context.GetDuration().TotalSeconds;
        
                // 长事务警告
                if (duration > 60)
                {
                    _logger.LogWarning($"[Transaction-{transactionId}] 长事务警告：{duration:F2}秒");
                }
        
                // 清理上下文
                _currentTransactionContext.Value = null;
                _tranDepth.Value = 0;
        
                // 关键修复：清理数据库连接实例，避免连接泄漏
                if (_asyncLocalClient.Value != null)
                {
                    try
                    {
                        var dbClient = _asyncLocalClient.Value;
                        if (dbClient.Ado.Connection != null && dbClient.Ado.Connection.State == System.Data.ConnectionState.Open)
                        {
                            dbClient.Ado.Connection.Close();
                            _logger.LogDebug($"[Transaction-{transactionId}] 数据库连接已关闭");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, $"[Transaction-{transactionId}] 关闭数据库连接时发生异常");
                    }
                    finally
                    {
                        _asyncLocalClient.Value = null;
                    }
                }
        
                _logger.LogDebug($"[Transaction-{transactionId}] 事务状态已重置");
            }
        }
                
        /// <summary>
        /// 释放资源（确保连接及时释放）
        /// </summary>
        public void Dispose()
        {
            var dbClient = _asyncLocalClient.Value;
            if (dbClient != null)
            {
                try
                {
                    var context = _currentTransactionContext.Value;
                            
                    // 如果有未完成的事务，强制回滚
                    if (context != null && context.Depth > 0)
                    {
                        _logger.LogWarning($"[Transaction-{context.TransactionId}] 检测到未完成的事务，执行强制回滚");
                        if (dbClient.Ado.Transaction != null)
                        {
                            dbClient.Ado.RollbackTran();
                        }
                    }
                            
                    // 关闭并释放连接
                    if (dbClient.Ado.Connection != null)
                    {
                        if (dbClient.Ado.Connection.State == ConnectionState.Open)
                        {
                            dbClient.Ado.Connection.Close();
                            _logger.LogDebug("Dispose 时已关闭数据库连接");
                        }
                        dbClient.Ado.Connection.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Dispose 时发生错误");
                }
                finally
                {
                    // 清理 AsyncLocal 引用
                    _asyncLocalClient.Value = null;
                    _currentTransactionContext.Value = null;
                    _tranDepth.Value = 0;
                }
            }
            
            // 释放读写锁
            _rwLock?.Dispose();
        }

        /// <summary>
        /// 获取当前事务上下文
        /// </summary>
        public TransactionContext CurrentTransactionContext => _currentTransactionContext.Value;

        /// <summary>
        /// 标记事务需要回滚
        /// </summary>
        public void MarkForRollback()
        {
            var context = _currentTransactionContext.Value;
            if (context == null)
            {
                _logger.LogWarning("标记事务回滚但无事务上下文");
                return;
            }

            context.ShouldRollback = true;
            _logger.LogWarning($"[Transaction-{context.TransactionId}] 事务已标记为需要回滚");
        }

        /// <summary>
        /// 获取事务状态
        /// </summary>
        public TransactionState GetTransactionState()
        {
            var context = _currentTransactionContext.Value;
            if (context == null)
            {
                return new TransactionState
                {
                    Depth = 0,
                    ShouldRollback = false,
                    IsActive = false
                };
            }

            return new TransactionState
            {
                Depth = context.Depth,
                ShouldRollback = context.ShouldRollback,
                IsActive = context.Status == TransactionStatus.Active
            };
        }

        /// <summary>
        /// 恢复事务状态
        /// </summary>
        public void RestoreTransactionState(TransactionState state)
        {
            if (state == null) return;

            var context = GetOrCreateTransactionContext();
            context.Depth = state.Depth;
            context.ShouldRollback = state.ShouldRollback;

            _tranDepth.Value = state.Depth;
        }

        /// <summary>
        /// 事务深度（兼容旧代码）
        /// </summary>
        public int TranCount => _tranDepth.Value;
        
        /// <summary>
        /// 从事务上下文中提取表名（用于性能监控）
        /// </summary>
        private string ExtractTableName(TransactionContext context)
        {
            try
            {
                // 从调用者方法名中提取实体类型
                if (!string.IsNullOrEmpty(context.CallerMethod))
                {
                    // 示例：tb_SaleOutController.RefundProcessAsync
                    var parts = context.CallerMethod.Split('.');
                    if (parts.Length > 0)
                    {
                        var controllerName = parts[0];
                        // tb_SaleOutController -> tb_SaleOut
                        if (controllerName.EndsWith("Controller"))
                        {
                            return controllerName.Substring(0, controllerName.Length - 10);
                        }
                    }
                }
            }
            catch { }
            
            return null;
        }

        /// <summary>
        /// 事务状态快照
        /// </summary>
        public class TransactionState
        {
            public int Depth { get; internal set; }
            public bool ShouldRollback { get; internal set; }
            public bool IsActive { get; internal set; }
            public override string ToString()
            {
                return $"Depth={Depth}, Rollback={ShouldRollback}, Active={IsActive}";
            }
        }
    }

    /// <summary>
    /// UnitOfWorkManage 扩展方法（死锁优化）
    /// </summary>
    public static class UnitOfWorkManageExtensions
    {
        /// <summary>
        /// 带重试的事务执行
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="unitOfWork">事务管理器</param>
        /// <param name="action">业务操作</param>
        /// <param name="maxRetries">最大重试次数，默认3</param>
        /// <param name="retryDelayMs">初始重试延迟（毫秒），默认100</param>
        /// <returns>操作结果</returns>
        public static T ExecuteWithRetry<T>(
            this IUnitOfWorkManage unitOfWork, 
            Func<T> action, 
            int maxRetries = 3,
            int retryDelayMs = 100)
        {
            int retryCount = 0;
            int currentDelay = retryDelayMs;

            while (true)
            {
                try
                {
                    return action();
                }
                catch (Exception ex)
                {
                    bool isRetryable = IsRetryableException(ex);
                    
                    if (!isRetryable || retryCount >= maxRetries)
                    {
                        throw;
                    }

                    retryCount++;
                    Thread.Sleep(currentDelay);
                    
                    // 指数退避
                    currentDelay = Math.Min(currentDelay * 2, 2000);
                }
            }
        }

        /// <summary>
        /// 带重试的异步事务执行
        /// </summary>
        public static async Task<T> ExecuteWithRetryAsync<T>(
            this IUnitOfWorkManage unitOfWork, 
            Func<Task<T>> action, 
            int maxRetries = 3,
            int retryDelayMs = 100)
        {
            int retryCount = 0;
            int currentDelay = retryDelayMs;

            while (true)
            {
                try
                {
                    return await action();
                }
                catch (Exception ex)
                {
                    bool isRetryable = IsRetryableException(ex);
                    
                    if (!isRetryable || retryCount >= maxRetries)
                    {
                        throw;
                    }

                    retryCount++;
                    await Task.Delay(currentDelay);
                    
                    // 指数退避
                    currentDelay = Math.Min(currentDelay * 2, 2000);
                }
            }
        }

        /// <summary>
        /// 判断异常是否可重试
        /// </summary>
        private static bool IsRetryableException(Exception ex)
        {
            if (ex is SqlException sqlEx)
            {
                // SQL Server 死锁错误码: 1205
                // 锁超时错误码: 1222
                return sqlEx.Number == 1205 || sqlEx.Number == 1222;
            }
            
            // 检查消息中是否包含死锁相关关键词
            string message = ex.Message?.ToLower() ?? "";
            return message.Contains("deadlock") || 
                   message.Contains("dead locked") ||
                   message.Contains("lock timeout") ||
                   message.Contains("1205") ||
                   message.Contains("1222");
        }
    }
}