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
using System.Data.SqlClient;

namespace RUINORERP.Repository.UnitOfWorks
{
    public class UnitOfWorkManage : IUnitOfWorkManage, IDependencyRepository
    {
        private readonly ILogger<UnitOfWorkManage> _logger;
        private readonly ISqlSugarClient _sqlSugarClient;
        private readonly bool _supportsNestedTransactions;
        public ApplicationContext _appContext;

        // 使用 AsyncLocal 保证异步安全
        private readonly AsyncLocal<ConcurrentDictionary<string, object>> _transactionContext =
            new AsyncLocal<ConcurrentDictionary<string, object>>();
 
        public readonly ConcurrentStack<string> TranStack = new ConcurrentStack<string>();

        public UnitOfWorkManage(ISqlSugarClient sqlSugarClient, ILogger<UnitOfWorkManage> logger, ApplicationContext appContext = null)
        {
            _sqlSugarClient = sqlSugarClient;
            _logger = logger;
            //_logger.Debug("UnitOfWorkManage初始化成功1");
            //_logger.LogDebug("UnitOfWorkManage初始化成功2");
            //_logger.LogDebug("UnitOfWorkManage初始化成功3");

            // 检测数据库是否支持嵌套事务
            _supportsNestedTransactions = DetectNestedTransactionSupport();

           // _logger.LogDebug($"事务模式: {(_supportsNestedTransactions ? "嵌套事务(SAVEPOINT)" : "单层事务")}");

            _appContext = appContext;
        }


        // 在UnitOfWorkManage类中添加以下方法

        /// <summary>
        /// 获取当前事务状态
        /// </summary>
        public TransactionState GetTransactionState()
        {
            return new TransactionState
            {
                Depth = PrivateTransactionDepth,
                ShouldRollback = ShouldRollback,
                IsActive = _sqlSugarClient.Ado.Transaction != null
            };
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

        /// <summary>
        /// 在指定状态下恢复事务
        /// </summary>
        public void RestoreTransactionState(TransactionState state)
        {
            if (state == null) return;

            lock (this)
            {
                PrivateTransactionDepth = state.Depth;
                ShouldRollback = state.ShouldRollback;

                // 如果深度为0但事务仍活动，强制回滚
                if (state.Depth <= 0 && state.IsActive)
                {
                    ForceRollback();
                }
            }
        }


        private bool DetectNestedTransactionSupport()
        {
            try
            {
                // SQL Server 始终支持保存点
                return _sqlSugarClient.Ado.Connection is SqlConnection;
            }
            catch
            {
                return false;
            }
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

        /// <summary>
        /// 获取当前事务上下文
        /// </summary>
        private ConcurrentDictionary<string, object> TransactionContext
        {
            get
            {
                if (_transactionContext.Value == null)
                {
                    _transactionContext.Value = new ConcurrentDictionary<string, object>();
                }
                return _transactionContext.Value;
            }
        }

        /// <summary>
        /// 事务深度计数器
        /// </summary>
        private int TransactionDepth
        {
            get => TransactionContext.TryGetValue("Depth", out var depth) ? (int)depth : 0;
            set => TransactionContext["Depth"] = value;
        }

        /// <summary>
        /// 事务深度计数器（私有）
        /// </summary>
        private int PrivateTransactionDepth
        {
            get => TransactionContext.TryGetValue("Depth", out var depth) ? (int)depth : 0;
            set => TransactionContext["Depth"] = value;
        }

        /// <summary>
        /// 公共事务深度计数器（只读）
        /// </summary>
        public int TranCount => PrivateTransactionDepth;

        /// <summary>
        /// 回滚标记
        /// </summary>
        private bool ShouldRollback
        {
            get => TransactionContext.TryGetValue("ShouldRollback", out var flag) && (bool)flag;
            set => TransactionContext["ShouldRollback"] = value;
        }

        /// <summary>
        /// 保存点名称栈（仅用于嵌套事务模式）
        /// </summary>
        private ConcurrentStack<string> SavePointStack
        {
            get
            {
                if (!TransactionContext.TryGetValue("SavePoints", out var stack))
                {
                    stack = new ConcurrentStack<string>();
                    TransactionContext["SavePoints"] = stack;
                }
                return (ConcurrentStack<string>)stack;
            }
        }

        //[Obsolete("请使用新的事务管理API")]
        //public UnitOfWork CreateUnitOfWork()
        //{
        //    UnitOfWork uow = new UnitOfWork();
        //    uow.Logger = _logger;
        //    uow.Db = _sqlSugarClient;
        //    uow.Tenant = (ITenant)_sqlSugarClient;
        //    uow.IsTran = true;

        //    uow.Db.Open();
        //    uow.Tenant.BeginTran();
        //    _logger.LogDebug("UnitOfWork Begin");
        //    return uow;
        //}

        //public void BeginTran()
        //{
        //    lock (this)
        //    {
        //        _tranCount++;
        //        if (_tranCount > 1)
        //        {
        //            System.Diagnostics.Debug.WriteLine($"_tranCount：{_tranCount}");
        //            _logger.LogDebug($"_tranCount：{ _tranCount}");
        //        }
        //        GetDbClient().BeginTran();
        //    }
        //}

        public void BeginTran()
        {
            lock (this)
            {
                try
                {
                    int newDepth = PrivateTransactionDepth + 1;
                    PrivateTransactionDepth = newDepth;

                    //_logger.LogDebug($"事务开始 (深度: {newDepth})");

                    // 最外层事务：开启物理事务
                    if (newDepth == 1)
                    {
                        _sqlSugarClient.Ado.BeginTran();
                        //_logger.LogDebug("物理事务已开启");
                        return;
                    }

                    // 嵌套事务支持：创建保存点
                    if (_supportsNestedTransactions)
                    {
                        var savePointName = $"SAVEPOINT_{newDepth}";
                        _sqlSugarClient.Ado.ExecuteCommand($"SAVE TRANSACTION {savePointName}");
                        SavePointStack.Push(savePointName);
                        _logger.LogDebug($"创建保存点: {savePointName}");
                    }
                }
                catch (Exception ex)
                {
                    PrivateTransactionDepth = Math.Max(0, PrivateTransactionDepth - 1);
                    _logger.LogError(ex, "事务开启失败");
                    throw;
                }
            }
        }


   

        //public void CommitTran()
        //{
        //    lock (this)
        //    {
        //        _tranCount--;
        //        if (_tranCount == 0)
        //        {
        //            try
        //            {
        //                GetDbClient().CommitTran();
        //            }
        //            catch (Exception ex)
        //            {
        //                System.Diagnostics.Debug.WriteLine(ex.Message);
        //                GetDbClient().RollbackTran();
        //            }
        //        }
        //    }
        //}

        public void CommitTran()
        {
            lock (this)
            {
                try
                {
                    // 检查回滚标记
                    if (ShouldRollback)
                    {
                        _logger.LogWarning("事务已被标记为回滚，跳过提交");
                        return;
                    }

                    int currentDepth = PrivateTransactionDepth;
                    if (currentDepth <= 0)
                    {
                        _logger.LogWarning("提交请求但无活动事务");
                        return;
                    }

                    int newDepth = currentDepth - 1;
                    PrivateTransactionDepth = newDepth;

                    _logger.LogDebug($"提交事务 (新深度: {newDepth})");

                    // 内层提交：仅更新深度
                    if (newDepth > 0) return;

                    // === 最外层提交逻辑 ===
                    _sqlSugarClient.Ado.CommitTran();
                    //_logger.LogDebug("事务提交成功");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "事务提交失败");
                    ForceRollback();
                    throw;
                }
                finally
                {
                    // 仅在最外层重置状态
                    if (PrivateTransactionDepth <= 0)
                    {
                        ResetTransactionState();
                    }
                }
            }
        }

        public void RollbackTran()
        {
            lock (this)
            {
                try
                {
                    // 设置回滚标记（阻止后续提交）
                    ShouldRollback = true;

                    int currentDepth = PrivateTransactionDepth;
                    if (currentDepth <= 0)
                    {
                        _logger.LogWarning("回滚请求但无活动事务");
                        return;
                    }

                    int newDepth = currentDepth - 1;
                    PrivateTransactionDepth = newDepth;

                    //_logger.LogWarning($"回滚事务 (原深度: {currentDepth}, 新深度: {newDepth})");

                    // 最外层回滚
                    if (currentDepth == 1)
                    {
                        _sqlSugarClient.Ado.RollbackTran();
                        //_logger.LogWarning("物理事务已回滚");
                        return;
                    }

                    // 嵌套事务支持：回滚到保存点
                    if (_supportsNestedTransactions && SavePointStack.TryPop(out var savePoint))
                    {
                        _sqlSugarClient.Ado.ExecuteCommand($"ROLLBACK TRANSACTION {savePoint}");
                        _logger.LogWarning($"回滚到保存点: {savePoint}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "回滚操作失败");
                    ForceRollback();
                    throw;
                }
                finally
                {
                    // 仅在最外层重置状态
                    if (PrivateTransactionDepth <= 0)
                    {
                        ResetTransactionState();
                    }
                }
            }
        }

        /// <summary>
        /// 强制回滚（安全最后防线）
        /// </summary>
        private void ForceRollback()
        {
            try
            {
                if (_sqlSugarClient.Ado.Transaction != null)
                {
                    _sqlSugarClient.Ado.RollbackTran();
                    _logger.LogCritical("强制回滚已执行");
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "强制回滚失败");
            }
            finally
            {
                ResetTransactionState();
            }
        }





        // 在UnitOfWorkManage类中添加

        /// <summary>
        /// 标记事务需要回滚（不实际执行回滚操作）
        /// </summary>
        public void MarkForRollback()
        {
            lock (this)
            {
                ShouldRollback = true;
                _logger.LogWarning("事务已标记为需要回滚");
            }
        }


        /// <summary>
        /// 重置事务状态
        /// </summary>
        private void ResetTransactionState()
        {
            TransactionContext.Clear();
            _transactionContext.Value = null;
            _logger.LogDebug("事务状态已重置");
        }



        //public void RollbackTran()
        //{
        //    lock (this)
        //    {
        //        _tranCount--;
        //        GetDbClient().RollbackTran();
        //    }
        //}

       
    }
}