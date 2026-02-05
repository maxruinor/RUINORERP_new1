using System;
using System.Reflection;
using System.Threading;
using System.Diagnostics;
using System.Linq;
using Microsoft.Extensions.Logging;
using SqlSugar;
using RUINORERP.Common.DI;
using RUINORERP.Model.Context;

namespace RUINORERP.Repository.UnitOfWorks
{
    public class UnitOfWorkManage : IUnitOfWorkManage, IDependencyRepository
    {
        private readonly ILogger<UnitOfWorkManage> _logger;
        private readonly ISqlSugarClient _sqlSugarClient;
        private readonly ApplicationContext _appContext;

        // 使用 AsyncLocal 保证异步安全
        private readonly AsyncLocal<TransactionContext> _currentTransactionContext =
            new AsyncLocal<TransactionContext>();

        // 每个线程独立的连接实例（关键修复！）
        private static readonly ThreadLocal<ISqlSugarClient> _threadLocalClient =
            new ThreadLocal<ISqlSugarClient>();

        // 简单的嵌套计数器（兼容旧代码）
        private readonly AsyncLocal<int> _tranDepth = new AsyncLocal<int>();

        public UnitOfWorkManage(ISqlSugarClient sqlSugarClient, ILogger<UnitOfWorkManage> logger, ApplicationContext appContext = null)
        {
            _sqlSugarClient = sqlSugarClient ?? throw new ArgumentNullException(nameof(sqlSugarClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _appContext = appContext;

            _logger.LogDebug($"UnitOfWorkManage 已创建，线程ID: {Thread.CurrentThread.ManagedThreadId}");
        }

        /// <summary>
        /// 获取线程独立的数据库客户端
        /// </summary>
        public SqlSugarScope GetDbClient()
        {
            // 每个线程使用独立的连接实例
            if (!_threadLocalClient.IsValueCreated)
            {
                // 复制原始连接配置，创建新的连接实例
                var originalConfig = (_sqlSugarClient as SqlSugarScope)?.CurrentConnectionConfig;

                if (originalConfig != null)
                {
                    var newConfig = new ConnectionConfig
                    {
                        ConnectionString = originalConfig.ConnectionString,
                        DbType = originalConfig.DbType,
                        IsAutoCloseConnection = true, // 重要：让每个线程自己管理连接
                        InitKeyType = originalConfig.InitKeyType,
                        MoreSettings = originalConfig.MoreSettings,
                        ConfigureExternalServices = originalConfig.ConfigureExternalServices,
                        AopEvents = originalConfig.AopEvents
                    };

                    _threadLocalClient.Value = new SqlSugarScope(newConfig);
                }
                else
                {
                    // 后备方案：创建新实例
                    _threadLocalClient.Value = new SqlSugarScope(new ConnectionConfig
                    {
                        ConnectionString = _sqlSugarClient.Ado.Connection.ConnectionString,
                        DbType = DbType.SqlServer,
                        IsAutoCloseConnection = true,
                        InitKeyType = InitKeyType.Attribute
                    });
                }

                _logger.LogDebug($"为线程 {Thread.CurrentThread.ManagedThreadId} 创建了新的数据库连接实例");
            }

            return _threadLocalClient.Value as SqlSugarScope;
        }

        /// <summary>
        /// 开始事务
        /// </summary>
        public void BeginTran()
        {
            var context = GetOrCreateTransactionContext();
            var dbClient = GetDbClient();

            // 锁住当前线程的连接
            lock (dbClient) // 这是关键！每个连接有自己的锁
            {
                try
                {
                    // 检查嵌套深度
                    if (context.Depth >= 5) // 限制最大嵌套深度
                    {
                        throw new InvalidOperationException(
                            $"事务嵌套深度超过最大限制(5)，请检查业务逻辑");
                    }

                    // 增加事务深度
                    context.Depth++;
                    _tranDepth.Value++;

                    // 最外层事务：开启物理事务
                    if (context.Depth == 1)
                    {
                        // 确保连接是打开的
                        if (dbClient.Ado.Connection.State != System.Data.ConnectionState.Open)
                        {
                            dbClient.Ado.Connection.Open();
                        }

                        dbClient.Ado.BeginTran();
                        _logger.LogDebug($"[Transaction-{context.TransactionId}] 事务已开启，线程ID: {Thread.CurrentThread.ManagedThreadId}");
                    }
                    // 嵌套事务：使用保存点（SQL Server支持）
                    else
                    {
                        var savePointName = $"SP_{context.TransactionId.ToString("N").Substring(0, 8)}_{context.Depth}";
                        dbClient.Ado.ExecuteCommand($"SAVE TRANSACTION {savePointName}");
                        context.SavePointStack.Push(savePointName);
                        _logger.LogDebug($"[Transaction-{context.TransactionId}] 创建保存点: {savePointName}");
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
            }
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public void CommitTran()
        {
            var context = CurrentTransactionContext;
            if (context == null)
            {
                _logger.LogWarning("提交请求但无事务上下文");
                return;
            }

            var dbClient = GetDbClient();

            // 锁住当前线程的连接
            lock (dbClient)
            {
                try
                {
                    if (context.Depth <= 0)
                    {
                        return;
                    }

                    // 检查回滚标记
                    if (context.ShouldRollback)
                    {
                        _logger.LogWarning($"[Transaction-{context.TransactionId}] 事务已标记为回滚，跳过提交");
                        context.Depth--;
                        _tranDepth.Value--;
                        return;
                    }

                    // 减少深度
                    context.Depth--;
                    _tranDepth.Value--;
                    context.UpdateActivityTime();

                    // 内层提交：仅更新深度
                    if (context.Depth > 0)
                    {
                        return;
                    }

                    // 最外层提交
                    try
                    {
                        dbClient.Ado.CommitTran();
                        context.Status = TransactionStatus.Committed;

                        var duration = context.GetDuration().TotalSeconds;
                        if (duration > 10)
                        {
                            _logger.LogWarning($"[Transaction-{context.TransactionId}] 长事务提交耗时: {duration:F2}秒");
                        }
                    }
                    catch (Exception commitEx)
                    {
                        context.Status = TransactionStatus.RolledBack;
                        _logger.LogError(commitEx, $"[Transaction-{context.TransactionId}] 事务提交失败");
                        throw;
                    }
                    finally
                    {
                        ResetTransactionState();
                    }
                }
                catch (Exception ex)
                {
                    context.Status = TransactionStatus.RolledBack;
                    ForceRollback(dbClient);
                    throw new InvalidOperationException($"事务提交失败: {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// 回滚事务
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

            // 锁住当前线程的连接
            lock (dbClient)
            {
                try
                {
                    if (context.Depth <= 0)
                    {
                        return;
                    }

                    // 设置回滚标记
                    context.ShouldRollback = true;
                    context.UpdateActivityTime();

                    // 减少深度
                    context.Depth--;
                    _tranDepth.Value--;

                    // 最外层回滚
                    if (context.Depth == 0)
                    {
                        try
                        {
                            dbClient.Ado.RollbackTran();
                            context.Status = TransactionStatus.RolledBack;
                        }
                        catch (Exception rollbackEx)
                        {
                            _logger.LogError(rollbackEx, $"[Transaction-{context.TransactionId}] 事务回滚失败");
                            throw;
                        }
                    }
                    // 嵌套事务：回滚到保存点
                    else if (context.SavePointStack.TryPop(out var savePoint))
                    {
                        try
                        {
                            dbClient.Ado.ExecuteCommand($"ROLLBACK TRANSACTION {savePoint}");
                        }
                        catch (Exception savePointEx)
                        {
                            _logger.LogError(savePointEx, $"[Transaction-{context.TransactionId}] 保存点回滚失败: {savePoint}");
                            throw;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"[Transaction-{context.TransactionId}] 回滚操作失败");
                    ForceRollback(dbClient);
                    throw new InvalidOperationException($"事务回滚失败: {ex.Message}", ex);
                }
                finally
                {
                    if (context.Depth <= 0)
                    {
                        ResetTransactionState();
                    }
                }
            }
        }

        /// <summary>
        /// 强制回滚
        /// </summary>
        private void ForceRollback(ISqlSugarClient dbClient)
        {
            try
            {
                if (dbClient.Ado.Transaction != null)
                {
                    dbClient.Ado.RollbackTran();
                    _logger.LogWarning("强制回滚已执行");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "强制回滚失败");
            }
            finally
            {
                ResetTransactionState();
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
                    _logger.LogWarning($"[Transaction-{transactionId}] 长事务警告: {duration:F2}秒");
                }

                // 清理上下文
                _currentTransactionContext.Value = null;
                _tranDepth.Value = 0;

                _logger.LogDebug($"[Transaction-{transactionId}] 事务状态已重置");
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
}