using System;
using RUINOR.Framework.Core.Common.DI;
using RUINOR.Framework.Core.Common.Helper;
using SqlSugar;

namespace RUINOR.Framework.Core.SqlSugar
{

    public class UnitOfWork : IUnitOfWork, IDependencyRepository
    {
        private readonly ISqlSugarClient _sqlSugarClient;

        public UnitOfWork(ISqlSugarClient sqlSugarClient)
        {
            _sqlSugarClient = sqlSugarClient;
        }

        /// <summary>
        /// 获取DB，保证唯一性
        /// </summary>
        /// <returns></returns>
        public SqlSugarScope GetDbClient()
        {
            return _sqlSugarClient as SqlSugarScope;
        }

        public void BeginTran()
        {
            GetDbClient().BeginTran();
        }

        public void CommitTran()
        {
            try
            {
                GetDbClient().CommitTran();
            }
            catch (Exception ex)
            {
                GetDbClient().RollbackTran();
                ConsoleHelper.WriteLine(ex.Message, ConsoleColor.Red);
                throw;
            }
        }

        public void RollbackTran()
        {
            GetDbClient().RollbackTran();
        }
    }
}