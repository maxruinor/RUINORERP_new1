using System;
using System.Threading;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Extensions.Logging;
using SqlSugar;
using System.Diagnostics;
using System.Linq;
using System.Data.SqlClient;

namespace RUINORERP.Repository.UnitOfWorks
{
    /// <summary>
    /// UnitOfWorkManage 异步方法扩展
    /// ✅ P1优化: 完整的异步事务支持
    /// ✅ P2优化: 集成自动超时机制
    /// </summary>
    public partial class UnitOfWorkManage
    {
        /// <summary>
        /// ✅ P1新增: 异步提交事务
        /// ✅ P2优化: 包含长事务检查和超时清理
        /// </summary>
        public async Task CommitTranAsync(CancellationToken cancellationToken = default)
        {
            var context = CurrentTransactionContext;
            if (context == null)
            {
                _logger.LogWarning("[CommitTranAsync] 提交请求但无事务上下文");
                return;
            }
                
            var dbClient = GetDbClient();
            var stopwatch = Stopwatch.StartNew();
                
            await context.LockSemaphore.WaitAsync(cancellationToken);
            try
            {
                if (context.Depth <= 0)
                {
                    _logger.LogWarning($"[Transaction-{context.TransactionId}] 提交请求但事务深度为 {context.Depth}，跳过提交");
                    ResetTransactionState();
                    return;
                }
                
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
                
                context.Depth--;
                _tranDepth.Value--;
                context.UpdateActivityTime();
                
                if (context.Depth > 0)
                {
                    return;
                }
                
                try
                {
                    // ✅ P2: 检查长事务警告
                    CheckLongTransactionWarning(context);
                    
                    if (dbClient.Ado.Transaction == null)
                    {
                        _logger.LogWarning($"[Transaction-{context.TransactionId}] 事务对象已为空，无法提交");
                        context.Status = TransactionStatus.Committed;
                    }
                    else
                    {
                        var transactionConnection = dbClient.Ado.Transaction.Connection;
                        if (transactionConnection == null || transactionConnection.State != ConnectionState.Open)
                        {
                            _logger.LogWarning($"[Transaction-{context.TransactionId}] 事务连接已关闭或无效");
                            context.Status = TransactionStatus.Committed;
                        }
                        else
                        {
                            await dbClient.Ado.CommitTranAsync();
                            context.Status = TransactionStatus.Committed;
                            _logger.LogInformation($"[Transaction-{context.TransactionId}] 异步事务提交成功");
                        }
                    }
                    
                    var duration = context.GetDuration().TotalSeconds;
                    if (duration > 10)
                    {
                        _logger.LogWarning($"[Transaction-{context.TransactionId}] 长事务提交耗时：{duration:F2}秒");
                    }
                            
                    TransactionMetrics.RecordTransaction("commit", context.CallerMethod, duration, true, ExtractTableName(context));
                }
                catch (InvalidOperationException invEx) when (invEx.Message.Contains("已完成") || invEx.Message.Contains("Zombie"))
                {
                    _logger.LogWarning(invEx, $"[Transaction-{context.TransactionId}] 事务已完成，忽略此异常");
                    context.Status = TransactionStatus.Committed;
                    TransactionMetrics.RecordTransaction("commit", context.CallerMethod, stopwatch.Elapsed.TotalSeconds, true);
                }
                catch (Exception commitEx)
                {
                    context.Status = TransactionStatus.RolledBack;
                    _logger.LogError(commitEx, $"[Transaction-{context.TransactionId}] 异步事务提交失败");
                    TransactionMetrics.RecordTransaction("commit", context.CallerMethod, stopwatch.Elapsed.TotalSeconds, false);
                    throw;
                }
                finally
                {
                    // ✅ P2: 清理超时机制
                    CleanupTransactionTimeout(context);
                    
                    ResetTransactionState();
                }
            }
            catch (Exception ex)
            {
                context.Status = TransactionStatus.RolledBack;
                ForceRollback(dbClient);
                TransactionMetrics.RecordTransaction("commit", context.CallerMethod, stopwatch.Elapsed.TotalSeconds, false);
                throw new InvalidOperationException($"异步事务提交失败：{ex.Message}", ex);
            }
            finally
            {
                context.LockSemaphore.Release();
            }
        }

        /// <summary>
        /// ✅ P1新增: 异步回滚事务
        /// ✅ P2优化: 包含长事务检查和超时清理
        /// </summary>
        public async Task RollbackTranAsync(CancellationToken cancellationToken = default)
        {
            var context = CurrentTransactionContext;
            if (context == null)
            {
                _logger.LogWarning("回滚请求但无事务上下文");
                return;
            }
        
            var dbClient = GetDbClient();
            var stopwatch = Stopwatch.StartNew();
        
            await context.LockSemaphore.WaitAsync(cancellationToken);
            try
            {
                if (context.Depth <= 0)
                {
                    _logger.LogWarning($"[Transaction-{context.TransactionId}] 回滚请求但事务深度为 {context.Depth}，跳过回滚");
                    ResetTransactionState();
                    return;
                }
        
                context.ShouldRollback = true;
                context.UpdateActivityTime();
        
                context.Depth--;
                _tranDepth.Value--;
        
                if (context.Depth == 0)
                {
                    try
                    {
                        // ✅ P2: 检查长事务警告
                        CheckLongTransactionWarning(context);
                        
                        if (dbClient.Ado.Transaction == null)
                        {
                            _logger.LogWarning($"[Transaction-{context.TransactionId}] 事务对象已为空，跳过回滚");
                            context.Status = TransactionStatus.RolledBack;
                        }
                        else
                        {
                            var transactionConnection = dbClient.Ado.Transaction.Connection;
                            if (transactionConnection == null || transactionConnection.State != ConnectionState.Open)
                            {
                                _logger.LogWarning($"[Transaction-{context.TransactionId}] 事务连接已关闭或无效");
                                context.Status = TransactionStatus.RolledBack;
                            }
                            else
                            {
                                await dbClient.Ado.RollbackTranAsync();
                                context.Status = TransactionStatus.RolledBack;
                                _logger.LogInformation($"[Transaction-{context.TransactionId}] 异步事务回滚成功");
                            }
                        }
                        
                        TransactionMetrics.RecordTransaction("rollback", context.CallerMethod, stopwatch.Elapsed.TotalSeconds, false, ExtractTableName(context));
                    }
                    catch (InvalidOperationException invEx) when (invEx.Message.Contains("已完成") || invEx.Message.Contains("Zombie"))
                    {
                        _logger.LogWarning(invEx, $"[Transaction-{context.TransactionId}] 事务已完成，忽略此异常");
                        context.Status = TransactionStatus.RolledBack;
                        TransactionMetrics.RecordTransaction("rollback", context.CallerMethod, stopwatch.Elapsed.TotalSeconds, false);
                    }
                    catch (Exception rollbackEx)
                    {
                        _logger.LogError(rollbackEx, $"[Transaction-{context.TransactionId}] 异步事务回滚失败");
                        TransactionMetrics.RecordTransaction("rollback", context.CallerMethod, stopwatch.Elapsed.TotalSeconds, false);
                        throw;
                    }
                    finally
                    {
                        // ✅ P2: 清理超时机制
                        CleanupTransactionTimeout(context);
                        
                        ResetTransactionState();
                    }
                }
                else if (context.SavePointStack.TryPop(out var savePoint))
                {
                    await dbClient.Ado.ExecuteCommandAsync($"ROLLBACK TRANSACTION {savePoint}", null, cancellationToken);
                    _logger.LogDebug($"[Transaction-{context.TransactionId}] 保存点回滚成功：{savePoint}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[Transaction-{context?.TransactionId.ToString() ?? "Unknown"}] RollbackTranAsync 外层异常");
                ForceRollback(dbClient);
                TransactionMetrics.RecordTransaction("rollback", context.CallerMethod, stopwatch.Elapsed.TotalSeconds, false);
                throw new InvalidOperationException($"异步事务回滚失败：{ex.Message}", ex);
            }
            finally
            {
                context.LockSemaphore.Release();
            }
        }

        /// <summary>
        /// ✅ P7优化: 增强的重试机制 - 支持CancellationToken和内部异常遍历
        /// </summary>
        public async Task ExecuteWithRetryAsync(Func<Task> action, int? maxRetryCount = null, CancellationToken cancellationToken = default)
        {
            int retryCount = 0;
            var maxRetries = maxRetryCount ?? _options.MaxRetryCount;  // ✅ 使用配置
            var context = CurrentTransactionContext;
            
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();
                
                try
                {
                    await action();
                    return;
                }
                catch (Exception ex)
                {
                    // ✅ 递归查找内部的SqlException
                    var sqlEx = GetInnermostSqlException(ex);
                    bool isRetryable = sqlEx != null && IsRetryableSqlError(sqlEx.Number);
                    
                    if (!isRetryable || retryCount >= maxRetries)
                    {
                        _logger.LogError(ex, $"[Transaction-{context?.TransactionId}] 执行失败，不再重试");
                        throw;
                    }

                    retryCount++;
                    var delayMs = (int)(100 * Math.Pow(2, retryCount)); // 指数退避
                    
                    _logger.LogWarning(sqlEx, 
                        $"[Transaction-{context?.TransactionId}] 检测到可重试错误(Number={sqlEx.Number})，正在进行第 {retryCount}/{maxRetries} 次重试，延迟 {delayMs}ms...");
                    
                    // ✅ 重试前重置事务状态
                    ResetTransactionState();
                    
                    await Task.Delay(delayMs, cancellationToken);
                }
            }
        }

        /// <summary>
        /// ✅ P7优化: 递归获取最内层的SqlException
        /// </summary>
        private SqlException GetInnermostSqlException(Exception ex)
        {
            while (ex != null)
            {
                if (ex is SqlException sqlEx)
                    return sqlEx;
                
                ex = ex.InnerException;
            }
            return null;
        }

        /// <summary>
        /// ✅ P13优化: 扩展可重试的SQL错误码
        /// </summary>
        private bool IsRetryableSqlError(int errorNumber)
        {
            // 使用传统if-else替代switch表达式以兼容C# 7.3
            if (errorNumber == 1205) return true;  // 死锁
            if (errorNumber == 1222) return true;  // 锁超时
            if (errorNumber == 40197) return true; // Azure SQL: 服务繁忙
            if (errorNumber == 40501) return true; // Azure SQL: 资源限制
            if (errorNumber == 40613) return true; // Azure SQL: 数据库不可用
            if (errorNumber == -2) return true;    // 查询超时
            return false;
        }
    }
}
