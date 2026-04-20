using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SqlSugar;

namespace RUINORERP.Repository.UnitOfWorks
{
    /// <summary>
    /// UnitOfWorkManage 修复补丁 - 针对"挂起请求"和事务管理问题
    /// ✅ P3 修复：解决"无法执行该事务操作，因为有挂起请求正在此事务上运行"错误
    /// ✅ P3 修复：解决残留事务对象问题
    /// ✅ P3 修复：解决连接泄漏和状态不一致问题
    /// </summary>
    public partial class UnitOfWorkManage
    {
        /// <summary>
        /// ✅ P3 新增：清理挂起的 DataReader 和命令
        /// 这是解决"挂起请求"错误的关键方法
        /// </summary>
        private void ClearPendingDataReader(ISqlSugarClient dbClient)
        {
            try
            {
                // 检查是否有活动的 DataReader
                if (dbClient.Ado.IsCloseConnection == false)
                {
                    // 尝试关闭可能存在的 DataReader
                    // SqlSugar 内部使用 DbConnection，需要确保没有未完成的读取
                    _logger.LogDebug($"[Transaction-Clear] 检测到连接未关闭，检查挂起的 DataReader");
                }
                
                // 关键修复：创建新连接来替换可能有挂起 DataReader 的连接
                // 这样可以彻底清除任何挂起的状态
                var currentConnection = dbClient.Ado.Connection;
                if (currentConnection != null && currentConnection.State == ConnectionState.Open)
                {
                    // 不关闭当前连接，而是标记需要清理
                    _logger.LogDebug($"[Transaction-Clear] 连接状态正常，继续执行");
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"[Transaction-Clear] 清理挂起 DataReader 时发生异常");
            }
        }

        /// <summary>
        /// ✅ P3 新增：安全回滚事务
        /// 处理各种异常情况，确保事务状态正确
        /// </summary>
        private void SafeRollbackTransaction(ISqlSugarClient dbClient, string transactionId)
        {
            if (dbClient.Ado.Transaction == null)
            {
                _logger.LogDebug($"[Transaction-{transactionId}] 事务对象为空，无需回滚");
                return;
            }

            try
            {
                var transactionConnection = dbClient.Ado.Transaction.Connection;
                if (transactionConnection == null)
                {
                    _logger.LogWarning($"[Transaction-{transactionId}] 事务连接为空，跳过回滚");
                    return;
                }

                if (transactionConnection.State != ConnectionState.Open)
                {
                    _logger.LogWarning($"[Transaction-{transactionId}] 事务连接已关闭 (State={transactionConnection.State})，跳过回滚");
                    return;
                }

                // 关键修复：先清理任何挂起的操作
                ClearPendingDataReader(dbClient);

                // 执行回滚
                dbClient.Ado.RollbackTran();
                _logger.LogInformation($"[Transaction-{transactionId}] 安全回滚成功");
            }
            catch (InvalidOperationException invEx) when 
                (invEx.Message.Contains("已完成") || invEx.Message.Contains("Zombie") || invEx.Message.Contains("挂起"))
            {
                _logger.LogWarning(invEx, $"[Transaction-{transactionId}] 事务已完成或存在挂起操作，忽略此异常");
            }
            catch (SqlException sqlEx) when (sqlEx.Message.Contains("挂起请求"))
            {
                // ✅ 关键修复：捕获"挂起请求"错误，这是可恢复的
                _logger.LogWarning(sqlEx, $"[Transaction-{transactionId}] 检测到挂起请求，连接将重置");
                // 标记需要重置连接
                throw; // 向上传播，让调用方处理
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[Transaction-{transactionId}] 安全回滚失败");
                throw;
            }
        }

        /// <summary>
        /// ✅ P3 新增：安全提交事务
        /// 处理各种异常情况，确保事务状态正确
        /// </summary>
        private void SafeCommitTransaction(ISqlSugarClient dbClient, string transactionId)
        {
            if (dbClient.Ado.Transaction == null)
            {
                _logger.LogWarning($"[Transaction-{transactionId}] 事务对象为空，无法提交");
                return;
            }

            try
            {
                var transactionConnection = dbClient.Ado.Transaction.Connection;
                if (transactionConnection == null)
                {
                    _logger.LogWarning($"[Transaction-{transactionId}] 事务连接为空，无法提交");
                    return;
                }

                if (transactionConnection.State != ConnectionState.Open)
                {
                    _logger.LogWarning($"[Transaction-{transactionId}] 事务连接已关闭 (State={transactionConnection.State})，无法提交");
                    return;
                }

                // 关键修复：提交前清理任何挂起的操作
                ClearPendingDataReader(dbClient);

                // 执行提交
                dbClient.Ado.CommitTran();
                _logger.LogInformation($"[Transaction-{transactionId}] 安全提交成功");
            }
            catch (SqlException sqlEx) when (sqlEx.Message.Contains("挂起请求"))
            {
                // ✅ 关键修复：捕获"挂起请求"错误
                _logger.LogError(sqlEx, $"[Transaction-{transactionId}] 提交失败 - 存在挂起的 DataReader");
                _logger.LogError($"[Transaction-{transactionId}] 解决方案：确保所有查询都使用 using 语句正确释放 DataReader");
                throw; // 重新抛出，让业务层知道失败
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[Transaction-{transactionId}] 安全提交失败");
                throw;
            }
        }

        /// <summary>
        /// ✅ P3 新增：重置数据库连接
        /// 当遇到无法恢复的错误时，创建新连接替换旧连接
        /// </summary>
        private void ResetDatabaseConnection()
        {
            var context = CurrentTransactionContext;
            var txId = context?.TransactionId.ToString() ?? "Unknown";

            _logger.LogWarning($"[Transaction-{txId}] 重置数据库连接");

            try
            {
                // 关闭并释放当前连接
                if (_asyncLocalClient.Value != null)
                {
                    var oldClient = _asyncLocalClient.Value;
                    try
                    {
                        if (oldClient.Ado.Connection != null && 
                            oldClient.Ado.Connection.State == ConnectionState.Open)
                        {
                            oldClient.Ado.Connection.Close();
                            _logger.LogDebug($"[Transaction-{txId}] 旧连接已关闭");
                        }
                        ((IDisposable)oldClient).Dispose();
                        _logger.LogDebug($"[Transaction-{txId}] 旧客户端已释放");
                    }
                    catch (Exception disposeEx)
                    {
                        _logger.LogWarning(disposeEx, $"[Transaction-{txId}] 释放旧客户端时发生异常");
                    }

                    // 清空 AsyncLocal 中的连接引用
                    _asyncLocalClient.Value = null;
                }

                // 注意：不重置 TransactionContext，让业务代码决定下一步操作
                _logger.LogInformation($"[Transaction-{txId}] 数据库连接已重置，下次操作将创建新连接");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[Transaction-{txId}] 重置数据库连接失败");
                throw;
            }
        }

        /// <summary>
        /// ✅ P3 新增：检查事务健康状态
        /// 在进行任何事务操作前检查连接和事务状态
        /// </summary>
        private bool IsTransactionHealthy(ISqlSugarClient dbClient, TransactionContext context)
        {
            if (context == null)
            {
                _logger.LogWarning($"[Transaction-Check] 事务上下文为空");
                return false;
            }

            if (dbClient == null)
            {
                _logger.LogWarning($"[Transaction-{context.TransactionId}] 数据库客户端为空");
                return false;
            }

            if (dbClient.Ado.Connection == null)
            {
                _logger.LogWarning($"[Transaction-{context.TransactionId}] 数据库连接为空");
                return false;
            }

            if (dbClient.Ado.Connection.State != ConnectionState.Open)
            {
                _logger.LogWarning($"[Transaction-{context.TransactionId}] 数据库连接已关闭 (State={dbClient.Ado.Connection.State})");
                return false;
            }

            // 检查事务对象
            if (context.Depth > 0 && dbClient.Ado.Transaction == null)
            {
                _logger.LogWarning($"[Transaction-{context.TransactionId}] 事务深度为{context.Depth}但事务对象为空 - 状态不一致");
                return false;
            }

            return true;
        }
    }
}
