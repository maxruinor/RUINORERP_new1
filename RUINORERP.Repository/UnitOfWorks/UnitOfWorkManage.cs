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
    /// 事务管理类,确认后的优化版本
    /// ✅ P1优化: 实现IAsyncDisposable支持异步释放
    /// ✅ P2优化: 支持自动超时机制
    /// </summary>
    public partial class UnitOfWorkManage : IUnitOfWorkManage, IDependencyRepository, IDisposable, IAsyncDisposable
    {
        private readonly ILogger<UnitOfWorkManage> _logger;
        private readonly ISqlSugarClient _sqlSugarClient;
        private readonly ApplicationContext _appContext;
        private readonly UnitOfWorkOptions _options;

        // ✅ P0修复: 移除ReaderWriterLockSlim,改用轻量级Monitor锁
        // ✅ P1优化: TransactionContext内部已升级为SemaphoreSlim,支持异步
        // private readonly ReaderWriterLockSlim _rwLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);

        // 使用 AsyncLocal 保证异步安全
        private readonly AsyncLocal<TransactionContext> _currentTransactionContext =
            new AsyncLocal<TransactionContext>();

        // 每个异步上下文独立的连接实例（关键修复！替换ThreadLocal为AsyncLocal）
        private readonly AsyncLocal<ISqlSugarClient> _asyncLocalClient =
            new AsyncLocal<ISqlSugarClient>();

        // 简单的嵌套计数器（兼容旧代码）
        private readonly AsyncLocal<int> _tranDepth = new AsyncLocal<int>();
        
        /// <summary>
        /// ✅ P2新增: 初始化事务超时机制
        /// </summary>
        private void InitializeTransactionTimeout(TransactionContext context, int? timeoutSeconds = null)
        {
            if (!_options.EnableAutoTransactionTimeout)
            {
                _logger.LogDebug($"[Transaction-{context.TransactionId}] 自动超时已禁用");
                return;
            }

            var timeout = timeoutSeconds ?? _options.DefaultTransactionTimeoutSeconds;
            context.StartTime = DateTime.UtcNow;
            
            // 创建超时令牌
            context.TimeoutCancellationTokenSource = new CancellationTokenSource();
            context.TimeoutCancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(timeout));
            
            // 注册超时回调
            context.TimeoutCancellationTokenSource.Token.Register(() =>
            {
                OnTransactionTimeout(context, timeout);
            });
            
            _logger.LogDebug($"[Transaction-{context.TransactionId}] 超时机制已启用: {timeout}秒");
        }

        /// <summary>
        /// ✅ P2新增: 事务超时回调
        /// </summary>
        private void OnTransactionTimeout(TransactionContext context, int timeoutSeconds)
        {
            // ✅ 关键修复: 检查事务是否已经结束
            if (context == null || context.Status == TransactionStatus.Committed || 
                context.Status == TransactionStatus.RolledBack || context.Depth == 0)
            {
                _logger.LogDebug($"[Transaction-{context?.TransactionId}] 超时回调触发,但事务已结束(Status={context?.Status}, Depth={context?.Depth}),忽略");
                return;
            }
            
            var duration = (DateTime.UtcNow - context.StartTime).TotalSeconds;
            
            _logger.LogError(
                $"[Transaction-{context.TransactionId}] ⚠️ 事务超时! " +
                $"配置超时={timeoutSeconds}秒, 实际运行={duration:F1}秒, " +
                $"调用方={context.CallerMethod}, 深度={context.Depth}");
            
            // 记录详细上下文信息
            _logger.LogError($"事务上下文: {context.GetDebugInfo()}");
            
            // ✅ 强制回滚(如果配置启用)
            if (_options.ForceRollbackOnTimeout && context.Depth > 0)
            {
                _logger.LogWarning($"[Transaction-{context.TransactionId}] 执行强制回滚");
                try
                {
                    var dbClient = GetDbClient();
                    if (dbClient.Ado.Transaction != null)
                    {
                        dbClient.Ado.RollbackTran();
                        _logger.LogInformation($"[Transaction-{context.TransactionId}] 超时强制回滚成功");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"[Transaction-{context.TransactionId}] 超时强制回滚失败");
                }
                finally
                {
                    context.ShouldRollback = true;
                    ResetTransactionState();
                }
            }
            else
            {
                _logger.LogWarning($"[Transaction-{context.TransactionId}] 超时但未执行强制回滚(ForceRollbackOnTimeout={_options.ForceRollbackOnTimeout})");
            }
        }

        /// <summary>
        /// ✅ P2新增: 检查并记录长事务警告
        /// </summary>
        private void CheckLongTransactionWarning(TransactionContext context)
        {
            if (context.StartTime == DateTime.MinValue)
                return;
                
            var duration = (DateTime.UtcNow - context.StartTime).TotalSeconds;
            
            // 超长事务 - 错误级别
            if (duration > _options.CriticalTransactionWarningSeconds)
            {
                _logger.LogError(
                    $"[Transaction-{context.TransactionId}] 🚨 超长事务警告! " +
                    $"已运行 {duration:F0}秒 (>{_options.CriticalTransactionWarningSeconds}秒), " +
                    $"调用方={context.CallerMethod}");
            }
            // 长事务 - 警告级别
            else if (duration > _options.LongTransactionWarningSeconds)
            {
                _logger.LogWarning(
                    $"[Transaction-{context.TransactionId}] ⚠️ 长事务警告: " +
                    $"已运行 {duration:F0}秒 (>{_options.LongTransactionWarningSeconds}秒), " +
                    $"调用方={context.CallerMethod}");
            }
        }

        /// <summary>
        /// ✅ P2新增: 清理超时机制
        /// </summary>
        private void CleanupTransactionTimeout(TransactionContext context)
        {
            if (context?.TimeoutCancellationTokenSource != null)
            {
                try
                {
                    // 取消超时令牌(防止超时回调在事务完成后执行)
                    if (!context.TimeoutCancellationTokenSource.IsCancellationRequested)
                    {
                        context.TimeoutCancellationTokenSource.Cancel();
                    }
                    context.TimeoutCancellationTokenSource.Dispose();
                    context.TimeoutCancellationTokenSource = null;
                    
                    _logger.LogDebug($"[Transaction-{context.TransactionId}] 超时机制已清理");
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, $"[Transaction-{context.TransactionId}] 清理超时机制时发生异常");
                }
            }
        }
        
        // 最大重试次数（针对死锁等瞬态故障）
        private const int MAX_RETRY_COUNT = 3;

        public UnitOfWorkManage(
            ISqlSugarClient sqlSugarClient, 
            ILogger<UnitOfWorkManage> logger, 
            ApplicationContext appContext = null,
            Microsoft.Extensions.Options.IOptions<UnitOfWorkOptions> options = null)
        {
            _sqlSugarClient = sqlSugarClient ?? throw new ArgumentNullException(nameof(sqlSugarClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _appContext = appContext;
            _options = options?.Value ?? new UnitOfWorkOptions();  // ✅ P2: 注入配置

            _logger.LogDebug($"UnitOfWorkManage 已创建，线程ID: {Thread.CurrentThread.ManagedThreadId}");
            _logger.LogDebug($"事务超时配置: EnableAutoTimeout={_options.EnableAutoTransactionTimeout}, " +
                           $"DefaultTimeout={_options.DefaultTransactionTimeoutSeconds}s, " +
                           $"ForceRollback={_options.ForceRollbackOnTimeout}");
        }

        /// <summary>
        /// 获取异步上下文独立的数据库客户端 - 不持有锁，直接返回
        /// 注意：此方法在事务操作前调用，不与事务锁冲突
        /// </summary>
        public ISqlSugarClient GetDbClient()
        {
            // ✅ P0修复: 每个异步上下文使用独立的连接实例
            // 关键: AsyncLocal确保不同异步流有独立的连接,避免"挂起请求"冲突
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
                        IsAutoCloseConnection = false, // ⚠️ 关键: 事务期间不自动关闭
                        InitKeyType = originalConfig.InitKeyType,
                        MoreSettings = originalConfig.MoreSettings,
                        ConfigureExternalServices = originalConfig.ConfigureExternalServices,
                        AopEvents = originalConfig.AopEvents
                    };
        
                    _asyncLocalClient.Value = new SqlSugarClient(newConfig);
                    _logger.LogInformation($"[Connection-{Guid.NewGuid().ToString("N").Substring(0, 8)}] 为异步上下文创建了新的数据库连接实例，线程 ID: {Thread.CurrentThread.ManagedThreadId}, AsyncLocal HashCode: {_asyncLocalClient.GetHashCode()}");
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
                    _logger.LogInformation($"[Connection-{Guid.NewGuid().ToString("N").Substring(0, 8)}] 使用默认配置创建新连接，线程 ID: {Thread.CurrentThread.ManagedThreadId}, AsyncLocal HashCode: {_asyncLocalClient.GetHashCode()}");
                }
            }
        
            return _asyncLocalClient.Value;
        }

        /// <summary>
        /// 开始事务 - 使用SemaphoreSlim确保独占访问
        /// ✅ P1优化: 改用异步信号量锁
        /// ✅ P2优化: 支持自动超时机制
        /// </summary>
        /// <param name="isolationLevel">可选的隔离级别，不指定则使用数据库默认</param>
        /// <param name="timeoutSeconds">可选的超时时间(秒)，null则使用配置默认值</param>
        public void BeginTran(IsolationLevel? isolationLevel = null, int? timeoutSeconds = null)
        {
            var context = GetOrCreateTransactionContext();
            var dbClient = GetDbClient();
        
            // ✅ P1优化: 使用SemaphoreSlim.Wait()替代lock
            context.LockSemaphore.Wait();
            try
            {
                // 增加事务深度
                context.Depth++;
                _tranDepth.Value++;

                // 超过10层时记录警告，包含调用栈信息
                if (context.Depth > 10)
                {
                    var warningMsg = $"[Transaction-{context.TransactionId}] 事务嵌套深度已达到 {context.Depth} 层，超过建议的10层限制";
                    
                    var stackTrace = new System.Diagnostics.StackTrace(3, true);
                    var frames = stackTrace.GetFrames().Take(5).ToArray();
                    var callStack = string.Join("\n  ", frames.Select(f => 
                    {
                        var method = f.GetMethod();
                        var fileName = System.IO.Path.GetFileName(f.GetFileName());
                        return $"{method.DeclaringType?.Name}.{method.Name}({fileName}:{f.GetFileLineNumber()})";
                    }));
                    
                    _logger.LogWarning($"{warningMsg}\n调用栈:\n  {callStack}");
                }
                
                // 超过15层时抛出异常
                if (context.Depth >= 15)
                {
                    throw new InvalidOperationException(
                        $"[Transaction-{context.TransactionId}] 事务嵌套深度超过最大限制 (15)，请检查业务逻辑是否存在循环调用或过度嵌套");
                }
    
                // 最外层事务：开启物理事务
                if (context.Depth == 1)
                {
                    // ✅ P3 关键修复: 先清理任何挂起的 DataReader
                    ClearPendingDataReader(dbClient);
                    
                    // ✅ 关键修复: 确保连接打开且无挂起的DataReader
                    if (dbClient.Ado.Connection.State != System.Data.ConnectionState.Open)
                    {
                        // ✅ 新增：如果连接处于 Connecting 状态，先关闭再重新打开
                        if (dbClient.Ado.Connection.State == System.Data.ConnectionState.Connecting)
                        {
                            _logger.LogWarning($"[Transaction-{context.TransactionId}] 检测到连接处于 Connecting 状态，强制关闭后重连");
                            try
                            {
                                dbClient.Ado.Connection.Close();
                            }
                            catch (Exception closeEx)
                            {
                                _logger.LogWarning(closeEx, $"[Transaction-{context.TransactionId}] 关闭 Connecting 状态的连接时异常");
                            }
                        }
                        
                        dbClient.Ado.Connection.Open();
                        _logger.LogInformation($"[Transaction-{context.TransactionId}] 数据库连接已打开，当前状态：{dbClient.Ado.Connection.State}");
                    }
                    
                    // ✅ 防御性检查: 清理可能存在的挂起命令
                    if (dbClient.Ado.Transaction != null)
                    {
                        _logger.LogWarning($"[Transaction-{context.TransactionId}] 检测到残留事务对象，先回滚");
                        try { dbClient.Ado.RollbackTran(); } catch { }
                    }
                    
                    // ✅ P3 再次清理: 确保在开启事务前没有其他挂起操作
                    ClearPendingDataReader(dbClient);
        
                    // 设置命令超时时间
                    dbClient.Ado.CommandTimeOut = _options.DefaultTransactionTimeoutSeconds;
                                
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
                    // ✅ P2修复: 缩短GUID为8位避免超过SQL Server 32字符限制
                    // 格式: SP_<8位GUID>_<深度> = 3+8+1+2 = 14字符(安全)
                    var guidShort = context.TransactionId.ToString("N").Substring(0, 8);
                    var savePointName = $"SP_{guidShort}_{context.Depth}";
                    dbClient.Ado.ExecuteCommand($"SAVE TRANSACTION {savePointName}");
                    context.SavePointStack.Push(savePointName);
                    _logger.LogDebug($"[Transaction-{context.TransactionId}] 创建保存点：{savePointName}");
                }
    
                context.Status = TransactionStatus.Active;
                context.UpdateActivityTime();
                
                // ✅ P2: 初始化超时机制
                InitializeTransactionTimeout(context, timeoutSeconds);
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
                context.LockSemaphore.Release();
            }
        }

        /// <summary>
        /// ✅ P1新增: 异步开始事务方法
        /// ✅ P2优化: 支持自动超时机制
        /// </summary>
        /// <param name="isolationLevel">可选的隔离级别</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <param name="timeoutSeconds">可选的超时时间(秒)，null则使用配置默认值</param>
        public async Task BeginTranAsync(
            IsolationLevel? isolationLevel = null, 
            CancellationToken cancellationToken = default,
            int? timeoutSeconds = null)
        {
            var context = GetOrCreateTransactionContext();
            var dbClient = GetDbClient();
        
            // ✅ P1优化: 异步等待锁
            await context.LockSemaphore.WaitAsync(cancellationToken);
            try
            {
                context.Depth++;
                _tranDepth.Value++;

                if (context.Depth > 10)
                {
                    var warningMsg = $"[Transaction-{context.TransactionId}] 事务嵌套深度已达到 {context.Depth} 层，超过建议的10层限制";
                    
                    var stackTrace = new System.Diagnostics.StackTrace(3, true);
                    var frames = stackTrace.GetFrames().Take(5).ToArray();
                    var callStack = string.Join("\n  ", frames.Select(f => 
                    {
                        var method = f.GetMethod();
                        var fileName = System.IO.Path.GetFileName(f.GetFileName());
                        return $"{method.DeclaringType?.Name}.{method.Name}({fileName}:{f.GetFileLineNumber()})";
                    }));
                    
                    _logger.LogWarning($"{warningMsg}\n调用栈:\n  {callStack}");
                }
                
                if (context.Depth >= 15)
                {
                    throw new InvalidOperationException(
                        $"[Transaction-{context.TransactionId}] 事务嵌套深度超过最大限制 (15)");
                }
    
                if (context.Depth == 1)
                {
                    // ✅ P3 关键修复: 先清理任何挂起的 DataReader
                    ClearPendingDataReader(dbClient);
                    
                    // 确保连接打开
                    if (dbClient.Ado.Connection.State != System.Data.ConnectionState.Open)
                    {
                        // IDbConnection 没有 OpenAsync,使用同步Open
                        dbClient.Ado.Connection.Open();
                    }
                    
                    // 清理残留事务
                    if (dbClient.Ado.Transaction != null)
                    {
                        _logger.LogWarning($"[Transaction-{context.TransactionId}] 检测到残留事务对象，先回滚");
                        try { await dbClient.Ado.RollbackTranAsync(); } catch { }
                    }
                    
                    // ✅ P3 再次清理: 确保在开启事务前没有其他挂起操作
                    ClearPendingDataReader(dbClient);
        
                    dbClient.Ado.CommandTimeOut = _options.DefaultTransactionTimeoutSeconds;
                                
                    // 异步开启事务
                    if (isolationLevel.HasValue)
                    {
                        await dbClient.Ado.BeginTranAsync(isolationLevel.Value);
                        _logger.LogInformation($"[Transaction-{context.TransactionId}] 异步事务已开启，隔离级别：{isolationLevel.Value}");
                    }
                    else
                    {
                        await dbClient.Ado.BeginTranAsync();
                        _logger.LogInformation($"[Transaction-{context.TransactionId}] 异步事务已开启");
                    }
                }
                else
                {
                    // ✅ P2修复: 缩短GUID为8位避免超过SQL Server 32字符限制
                    var guidShort = context.TransactionId.ToString("N").Substring(0, 8);
                    var savePointName = $"SP_{guidShort}_{context.Depth}";
                    await dbClient.Ado.ExecuteCommandAsync($"SAVE TRANSACTION {savePointName}");
                    context.SavePointStack.Push(savePointName);
                }
    
                context.Status = TransactionStatus.Active;
                context.UpdateActivityTime();
                
                // ✅ P2: 初始化超时机制
                InitializeTransactionTimeout(context, timeoutSeconds);
            }
            catch (Exception ex)
            {
                context.Depth = Math.Max(0, context.Depth - 1);
                _tranDepth.Value = Math.Max(0, _tranDepth.Value - 1);
    
                _logger.LogError(ex, $"[Transaction-{context.TransactionId}] 异步事务开启失败");
                throw;
            }
            finally
            {
                context.LockSemaphore.Release();
            }
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public void CommitTran() => CommitTranInternal();
                
        /// <summary>
        /// 提交事务内部实现 - 使用SemaphoreSlim保护
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
            var stopwatch = Stopwatch.StartNew();
                
            // ✅ P1修复: 使用SemaphoreSlim替代已移除的LockObject
            context.LockSemaphore.Wait();
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
                    // ✅ P2: 检查长事务警告
                    CheckLongTransactionWarning(context);
                                        
                    // ✅ 防御性检查：验证事务对象状态
                    if (dbClient.Ado.Transaction == null)
                    {
                        _logger.LogError($"[Transaction-{context.TransactionId}] ❌ 事务对象已为空，无法提交！数据未保存！");
                        context.Status = TransactionStatus.RolledBack;  // ✅ 修复：标记为回滚而非提交
                        throw new InvalidOperationException("事务对象不存在，提交失败，数据未保存");
                    }
                    else
                    {
                        var transactionConnection = dbClient.Ado.Transaction.Connection;
                        if (transactionConnection == null || transactionConnection.State != System.Data.ConnectionState.Open)
                        {
                            _logger.LogError($"[Transaction-{context.TransactionId}] ❌ 事务连接已关闭或无效（State={transactionConnection?.State}），无法提交！数据未保存！");
                            context.Status = TransactionStatus.RolledBack;  // ✅ 修复：标记为回滚而非提交
                            throw new InvalidOperationException($"事务连接状态异常（{transactionConnection?.State}），提交失败，数据未保存");
                        }
                        else
                        {
                            // ✅ P3 关键修复：检查事务健康状态
                            if (!IsTransactionHealthy(dbClient, context))
                            {
                                _logger.LogError($"[Transaction-{context.TransactionId}] 事务健康检查失败，强制回滚");
                                ForceRollback(dbClient);
                                throw new InvalidOperationException("事务状态异常，已强制回滚");
                            }
                            
                            // ✅ P3 关键修复：确保没有挂起的 DataReader
                            ClearPendingDataReader(dbClient);
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
                    // ✅ P2: 清理超时机制
                    CleanupTransactionTimeout(context);
                                            
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
                context.LockSemaphore.Release();
            }
        }

        /// <summary>
        /// 回滚事务 - 使用SemaphoreSlim保护
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
            var stopwatch = Stopwatch.StartNew();
        
            // ✅ P1修复: 使用SemaphoreSlim替代已移除的LockObject
            context.LockSemaphore.Wait();
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
                        // ✅ P2: 检查长事务警告
                        CheckLongTransactionWarning(context);
                                            
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
                        // ✅ P2: 清理超时机制
                        CleanupTransactionTimeout(context);
                        
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
                context.LockSemaphore.Release();
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
                        // ✅ P3 关键修复：清理挂起的 DataReader
                        ClearPendingDataReader(dbClient);
                        dbClient.Ado.RollbackTran();
                        _logger.LogWarning($"[Transaction-{txId}] 强制回滚已执行");
                    }
                }
            }
            catch (SqlException sqlEx) when (sqlEx.Message.Contains("挂起请求"))
            {
                // ✅ P3 关键修复：捕获"挂起请求"错误，重置连接
                _logger.LogWarning(sqlEx, $"[Transaction-{txId}] 强制回滚失败 - 存在挂起的 DataReader，重置连接");
                ResetDatabaseConnection();
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
        /// ✅ P7优化: 已移至AsyncMethods.cs,支持CancellationToken和配置化
        /// </summary>
        /// <param name="action">要执行的操作</param>
        /// <param name="maxRetryCount">最大重试次数，默认使用配置值</param>
        /// <param name="cancellationToken">取消令牌</param>
        // 注意: 此方法已在 UnitOfWorkManage.AsyncMethods.cs 中实现
        // public async Task ExecuteWithRetryAsync(Func<Task> action, int? maxRetryCount = null, CancellationToken cancellationToken = default)

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
        
                // ✅ P2: 检查长事务警告(如果还没检查过)
                CheckLongTransactionWarning(context);
                
                // ✅ P2: 清理超时机制
                CleanupTransactionTimeout(context);
        
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
                        
                        // ✅ 新增：先清理挂起的 DataReader
                        ClearPendingDataReader(dbClient);
                        
                        if (dbClient.Ado.Connection != null && dbClient.Ado.Connection.State == System.Data.ConnectionState.Open)
                        {
                            dbClient.Ado.Connection.Close();
                            _logger.LogDebug($"[Transaction-{transactionId}] 数据库连接已关闭");
                        }
                        else if (dbClient.Ado.Connection != null && dbClient.Ado.Connection.State == System.Data.ConnectionState.Connecting)
                        {
                            _logger.LogWarning($"[Transaction-{transactionId}] 检测到连接处于 Connecting 状态，强制关闭");
                            try
                            {
                                dbClient.Ado.Connection.Close();
                            }
                            catch (Exception closeEx)
                            {
                                _logger.LogWarning(closeEx, $"[Transaction-{transactionId}] 关闭 Connecting 状态的连接时异常");
                            }
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
        /// ✅ P2优化: 清理超时机制
        /// </summary>
        public void Dispose()
        {
            var dbClient = _asyncLocalClient.Value;
            if (dbClient != null)
            {
                try
                {
                    var context = _currentTransactionContext.Value;
                            
                    // ✅ P2: 如果有未完成的事务且已超时,记录警告
                    if (context != null && context.Depth > 0)
                    {
                        var duration = (DateTime.UtcNow - context.StartTime).TotalSeconds;
                        if (duration > _options.LongTransactionWarningSeconds)
                        {
                            _logger.LogWarning(
                                $"[Transaction-{context.TransactionId}] ⚠️ Dispose时发现长事务! " +
                                $"已运行 {duration:F0}秒, 深度={context.Depth}, " +
                                $"调用方={context.CallerMethod}");
                        }
                        
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
                    // ✅ P2: 清理超时机制
                    var context = _currentTransactionContext.Value;
                    CleanupTransactionTimeout(context);
                    
                    // 清理 AsyncLocal 引用
                    _asyncLocalClient.Value = null;
                    _currentTransactionContext.Value = null;
                    _tranDepth.Value = 0;
                }
            }
            
            // 释放读写锁（已移除）
            // _rwLock?.Dispose();
        }

        /// <summary>
        /// ✅ P1新增: 异步释放资源
        /// ✅ P2优化: 清理超时机制
        /// </summary>
        public async ValueTask DisposeAsync()
        {
            var dbClient = _asyncLocalClient.Value;
            if (dbClient != null)
            {
                try
                {
                    var context = _currentTransactionContext.Value;
                    
                    // ✅ P2: 如果有未完成的事务且已超时,记录警告
                    if (context != null && context.Depth > 0)
                    {
                        var duration = (DateTime.UtcNow - context.StartTime).TotalSeconds;
                        if (duration > _options.LongTransactionWarningSeconds)
                        {
                            _logger.LogWarning(
                                $"[Transaction-{context.TransactionId}] ⚠️ DisposeAsync时发现长事务! " +
                                $"已运行 {duration:F0}秒, 深度={context.Depth}, " +
                                $"调用方={context.CallerMethod}");
                        }
                        
                        _logger.LogWarning($"[Transaction-{context.TransactionId}] 检测到未完成的事务，执行强制回滚");
                        if (dbClient.Ado.Transaction != null)
                        {
                            await dbClient.Ado.RollbackTranAsync();
                        }
                    }
                    
                    // 异步关闭并释放连接
                    if (dbClient.Ado.Connection != null)
                    {
                        if (dbClient.Ado.Connection.State == ConnectionState.Open)
                        {
                            dbClient.Ado.Connection.Close();
                        }
                    }
                    
                    // 释放SqlSugarClient
                    ((IDisposable)dbClient).Dispose();
                    _logger.LogDebug("DisposeAsync 时已释放数据库客户端");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "DisposeAsync 时发生错误");
                }
                finally
                {
                    // ✅ P2: 清理超时机制
                    var context = _currentTransactionContext.Value;
                    CleanupTransactionTimeout(context);
                    
                    _asyncLocalClient.Value = null;
                    _currentTransactionContext.Value?.Dispose();
                    _currentTransactionContext.Value = null;
                    _tranDepth.Value = 0;
                }
            }
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