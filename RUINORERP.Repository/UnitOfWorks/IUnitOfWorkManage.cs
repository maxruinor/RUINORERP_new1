using System;
using System.Reflection;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using SqlSugar;

namespace RUINORERP.Repository.UnitOfWorks
{
    /// <summary>
    /// 事务管理接口1
    /// ✅ P2优化: 支持自动超时机制
    /// </summary>
    public interface IUnitOfWorkManage : IAsyncDisposable
    {
        /// <summary>
        /// 获取数据库客户端
        /// </summary>
        ISqlSugarClient GetDbClient();

        /// <summary>
        /// 事务深度
        /// </summary>
        int TranCount { get; }

        /// <summary>
        /// 开始事务
        /// ✅ P2优化: 支持自定义超时时间
        /// </summary>
        /// <param name="isolationLevel">可选的隔离级别</param>
        /// <param name="timeoutSeconds">可选的超时时间(秒)，null则使用配置默认值</param>
        void BeginTran(IsolationLevel? isolationLevel = null, int? timeoutSeconds = null);

        /// <summary>
        /// ✅ P1新增: 异步开始事务
        /// ✅ P2优化: 支持自定义超时时间
        /// </summary>
        /// <param name="isolationLevel">可选的隔离级别</param>
        /// <param name="cancellationToken">取消令牌</param>
        /// <param name="timeoutSeconds">可选的超时时间(秒)，null则使用配置默认值</param>
        Task BeginTranAsync(IsolationLevel? isolationLevel = null, CancellationToken cancellationToken = default, int? timeoutSeconds = null);

        /// <summary>
        /// 提交事务
        /// </summary>
        void CommitTran();

        /// <summary>
        /// ✅ P1新增: 异步提交事务
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        Task CommitTranAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 回滚事务
        /// </summary>
        void RollbackTran();

        /// <summary>
        /// ✅ P1新增: 异步回滚事务
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        Task RollbackTranAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 恢复事务状态
        /// </summary>
        void RestoreTransactionState(UnitOfWorkManage.TransactionState originalState);

        /// <summary>
        /// 标记事务需要回滚
        /// </summary>
        void MarkForRollback();

        /// <summary>
        /// 获取事务状态
        /// </summary>
        UnitOfWorkManage.TransactionState GetTransactionState();

        /// <summary>
        /// 带重试的执行方法（用于处理死锁等瞬态故障）
        /// </summary>
        void ExecuteWithRetry(Action action, int maxRetryCount = 3);

        /// <summary>
        /// 异步版本的带重试执行方法
        /// ✅ P7优化: 支持CancellationToken和配置化重试次数
        /// </summary>
        /// <param name="action">要执行的异步操作</param>
        /// <param name="maxRetryCount">最大重试次数，null则使用配置默认值</param>
        /// <param name="cancellationToken">取消令牌</param>
        Task ExecuteWithRetryAsync(Func<Task> action, int? maxRetryCount = null, CancellationToken cancellationToken = default);
    }
}
