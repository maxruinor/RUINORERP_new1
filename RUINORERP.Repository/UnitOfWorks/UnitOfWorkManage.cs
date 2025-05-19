using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Threading;
using RUINORERP.Common.Extensions;
using Microsoft.Extensions.Logging;
using SqlSugar;
using RUINORERP.Common.DI;
using RUINORERP.Model.Context;
using log4net.Repository.Hierarchy;

namespace RUINORERP.Repository.UnitOfWorks
{
    public class UnitOfWorkManage : IUnitOfWorkManage, IDependencyRepository
    {
        private readonly ILogger<UnitOfWorkManage> _logger;
        private readonly ISqlSugarClient _sqlSugarClient;

        public ApplicationContext _appContext;


        private int _tranCount { get; set; }
        public int TranCount => _tranCount;
        public readonly ConcurrentStack<string> TranStack = new ConcurrentStack<string>();

        public UnitOfWorkManage(ISqlSugarClient sqlSugarClient, ILogger<UnitOfWorkManage> logger, ApplicationContext appContext = null)
        {
            _sqlSugarClient = sqlSugarClient;
            _logger = logger;
            //_logger.Debug("UnitOfWorkManage初始化成功1");
            //_logger.LogDebug("UnitOfWorkManage初始化成功2");
            //_logger.LogInformation("UnitOfWorkManage初始化成功3");
            //这里的日志会执行，但是不会保存到数据库，是什么原因呢？是因为还没有准备好DB形式？
            _tranCount = 0;
            _tranCount = 0;
            _appContext = appContext;
        }

        /// <summary>
        /// 获取DB，保证唯一性
        /// </summary>
        /// <returns></returns>
        public SqlSugarScope GetDbClient()
        {
            // 必须要as，后边会用到切换数据库操作
            return _sqlSugarClient as SqlSugarScope;
        }


        public UnitOfWork CreateUnitOfWork()
        {
            UnitOfWork uow = new UnitOfWork();
            uow.Logger = _logger;
            uow.Db = _sqlSugarClient;
            uow.Tenant = (ITenant)_sqlSugarClient;
            uow.IsTran = true;

            uow.Db.Open();
            uow.Tenant.BeginTran();
            _logger.LogInformation("UnitOfWork Begin");
            return uow;
        }

        public void BeginTran()
        {
            lock (this)
            {
                _tranCount++;
                
                GetDbClient().BeginTran();
            }
        }

        public void BeginTran(MethodInfo method)
        {
            lock (this)
            {
                GetDbClient().BeginTran();
                TranStack.Push(method.GetFullName());
                _tranCount = TranStack.Count;
            }
        }

        public void CommitTran()
        {
            lock (this)
            {
                _tranCount--;
                if (_tranCount == 0)
                {
                    try
                    {
                        GetDbClient().CommitTran();
                    }
                    catch (Exception ex)
                    {
                        //Console.WriteLine(ex.Message);
                        GetDbClient().RollbackTran();
                    }
                }
            }
        }

        public void CommitTran(MethodInfo method)
        {
            lock (this)
            {
                string result = "";
                while (!TranStack.IsEmpty && !TranStack.TryPeek(out result))
                {
                    Thread.Sleep(1);
                }


                if (result == method.GetFullName())
                {
                    try
                    {
                        GetDbClient().CommitTran();
                        _logger.LogInformation($"Commit Transaction");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        GetDbClient().RollbackTran();
                        _logger.LogInformation($"Commit Error , Rollback Transaction");
                    }
                    finally
                    {
                        while (!TranStack.TryPop(out _))
                        {
                            Thread.Sleep(1);
                        }

                        _tranCount = TranStack.Count;
                    }
                }
            }
        }

        public void RollbackTran()
        {
            lock (this)
            {
                _tranCount--;
                GetDbClient().RollbackTran();
            }
        }

        public void RollbackTran(MethodInfo method)
        {
            lock (this)
            {
                string result = "";
                while (!TranStack.IsEmpty && !TranStack.TryPeek(out result))
                {
                    Thread.Sleep(1);
                }

                if (result == method.GetFullName())
                {
                    GetDbClient().RollbackTran();
                    _logger.LogInformation($"Rollback Transaction");
                    while (!TranStack.TryPop(out _))
                    {
                        Thread.Sleep(1);
                    }

                    _tranCount = TranStack.Count;
                }
            }
        }
    }
}