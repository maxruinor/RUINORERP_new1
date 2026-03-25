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
        /// </summary>
        void BeginTran(IsolationLevel? isolationLevel = null);

        /// <summary>
        /// 提交事务
        /// </summary>
        void CommitTran();

        /// <summary>
        /// 回滚事务
        /// </summary>
        void RollbackTran();

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
        /// </summary>
        Task ExecuteWithRetryAsync(Func<Task> action, int maxRetryCount = 3);
    }
}
