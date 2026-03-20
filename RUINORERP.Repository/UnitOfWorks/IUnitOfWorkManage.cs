using System;
using System.Reflection;
using System.Data;
using System.Threading.Tasks;
using SqlSugar;

namespace RUINORERP.Repository.UnitOfWorks
{
    /// <summary>
    /// 事务管理接口
    /// </summary>
    public interface IUnitOfWorkManage
    {
        SqlSugarScope GetDbClient();

        int TranCount { get; }
        void BeginTran(IsolationLevel? isolationLevel = null);
        void CommitTran();
        void RollbackTran();
        void RestoreTransactionState(UnitOfWorkManage.TransactionState originalState);
        void MarkForRollback();
        UnitOfWorkManage.TransactionState GetTransactionState();

        /// <summary>
        /// 带重试的执行方法（用于处理死锁等瞬态故障）
        /// </summary>
        /// <param name="action">要执行的操作</param>
        /// <param name="maxRetryCount">最大重试次数，默认 3 次</param>
        void ExecuteWithRetry(Action action, int maxRetryCount = 3);

        /// <summary>
        /// 异步版本的带重试执行方法
        /// </summary>
        /// <param name="action">要执行的操作</param>
        /// <param name="maxRetryCount">最大重试次数，默认 3 次</param>
        Task ExecuteWithRetryAsync(Func<Task> action, int maxRetryCount = 3);
    }
}