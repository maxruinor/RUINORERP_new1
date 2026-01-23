using System.Reflection;
using SqlSugar;

namespace RUINORERP.Repository.UnitOfWorks
{
    /// <summary>
    /// 1
    /// </summary>
    public interface IUnitOfWorkManage
    {
        SqlSugarScope GetDbClient();

        int TranCount { get; }
        void BeginTran();
        void CommitTran();
        void RollbackTran();
        void RestoreTransactionState(UnitOfWorkManage.TransactionState originalState);
        void MarkForRollback();
        UnitOfWorkManage.TransactionState GetTransactionState();
    }
}