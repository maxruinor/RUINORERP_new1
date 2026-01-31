using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Threading;
using System.Diagnostics;
using System.Linq;
using RUINORERP.Common.Extensions;
using Microsoft.Extensions.Logging;
using SqlSugar;
using RUINORERP.Common.DI;
using RUINORERP.Model.Context;
using System.Data.SqlClient;
using System.Data.Common;

namespace RUINORERP.Repository.UnitOfWorks
{
    public class UnitOfWorkManage : IUnitOfWorkManage, IDependencyRepository
    {
        private readonly ILogger<UnitOfWorkManage> _logger;
        private readonly ISqlSugarClient _sqlSugarClient;
        private readonly bool _supportsNestedTransactions;
        public ApplicationContext _appContext;

        // 使用 AsyncLocal 保证异步安全 - 增强的事务上下文
        private readonly AsyncLocal<TransactionContext> _currentTransactionContext =
            new AsyncLocal<TransactionContext>();

        // 全局活跃事务字典（用于监控和清理僵尸事务）
        private static readonly ConcurrentDictionary<Guid, TransactionContext> _activeTransactions = 
            new ConcurrentDictionary<Guid, TransactionContext>();

        // 僵尸事务清理定时器
        private static Timer _zombieCleanupTimer;
        private static readonly object _cleanupLock = new object();
        private static bool _cleanupInitialized = false;
 
        public readonly ConcurrentStack<string> TranStack = new ConcurrentStack<string>();

        public UnitOfWorkManage(ISqlSugarClient sqlSugarClient, ILogger<UnitOfWorkManage> logger, ApplicationContext appContext = null)
        {
            _sqlSugarClient = sqlSugarClient ?? throw new ArgumentNullException(nameof(sqlSugarClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            
            // 检测数据库是否支持嵌套事务
            _supportsNestedTransactions = DetectNestedTransactionSupport();

            _appContext = appContext;
            
            // 初始化僵尸事务清理机制（仅一次）
            InitializeZombieCleanup();
        }

        /// <summary>
        /// 初始化僵尸事务自动清理机制
        /// </summary>
        private void InitializeZombieCleanup()
        {
            lock (_cleanupLock)
            {
                if (!_cleanupInitialized)
                {
                    // 每5分钟检查一次僵尸事务
                    _zombieCleanupTimer = new Timer(CleanupZombieTransactions, null, 
                        TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(5));
                    _cleanupInitialized = true;
                    _logger.LogInformation("僵尸事务自动清理机制已启动（每5分钟检查一次）");
                }
            }
        }

        /// <summary>
        /// 清理僵尸事务的回调方法
        /// </summary>
        private void CleanupZombieTransactions(object state)
        {
            try
            {
                var zombieTransactions = _activeTransactions.Values
                    .Where(t => t.IsTimeout() && t.Status == TransactionStatus.Active)
                    .ToList();

                if (zombieTransactions.Any())
                {
                    _logger.LogWarning($"检测到 {zombieTransactions.Count} 个僵尸事务，正在清理...");
                    
                    foreach (var transaction in zombieTransactions)
                    {
                        if (_activeTransactions.TryRemove(transaction.TransactionId, out _))
                        {
                            transaction.Status = TransactionStatus.Zombie;
                            _logger.LogWarning($"已清理僵尸事务: {transaction.TransactionId}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "僵尸事务清理过程中发生错误");
            }
        }

        /// <summary>
        /// 获取当前事务上下文（增强版）
        /// </summary>
        public TransactionContext CurrentTransactionContext
        {
            get => _currentTransactionContext.Value;
            private set => _currentTransactionContext.Value = value;
        }

        /// <summary>
        /// 获取当前事务状态（兼容旧版本）
        /// </summary>
        public TransactionState GetTransactionState()
        {
            var context = CurrentTransactionContext;
            if (context == null)
            {
                return new TransactionState
                {
                    Depth = 0,
                    ShouldRollback = false,
                    IsActive = _sqlSugarClient.Ado.Transaction != null
                };
            }

            return new TransactionState
            {
                Depth = context.Depth,
                ShouldRollback = context.ShouldRollback,
                IsActive = _sqlSugarClient.Ado.Transaction != null
            };
        }

        /// <summary>
        /// 事务状态快照（兼容旧版本）
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
        /// 在指定状态下恢复事务（兼容旧版本）
        /// </summary>
        public void RestoreTransactionState(TransactionState state)
        {
            if (state == null) return;

            lock (this)
            {
                var context = CurrentTransactionContext;
                if (context != null)
                {
                    context.Depth = state.Depth;
                    context.ShouldRollback = state.ShouldRollback;
                }

                // 如果深度为0但事务仍活动，强制回滚
                if (state.Depth <= 0 && state.IsActive)
                {
                    ForceRollback();
                }
            }
        }

        /// <summary>
        /// 获取增强的事务上下文信息
        /// </summary>
        public TransactionContext GetEnhancedTransactionContext()
        {
            return CurrentTransactionContext;
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
        /// 获取当前事务深度（兼容旧版本）
        /// </summary>
        private int PrivateTransactionDepth
        {
            get
            {
                var context = CurrentTransactionContext;
                return context?.Depth ?? 0;
            }
            set
            {
                var context = CurrentTransactionContext;
                if (context != null)
                {
                    context.Depth = value;
                }
            }
        }

        /// <summary>
        /// 公共事务深度计数器（只读）
        /// </summary>
        public int TranCount => CurrentTransactionContext?.Depth ?? 0;

        /// <summary>
        /// 回滚标记（兼容旧版本）
        /// </summary>
        private bool ShouldRollback
        {
            get
            {
                var context = CurrentTransactionContext;
                return context?.ShouldRollback ?? false;
            }
            set
            {
                var context = CurrentTransactionContext;
                if (context != null)
                {
                    context.ShouldRollback = value;
                }
            }
        }

        /// <summary>
        /// 保存点名称栈（兼容旧版本）
        /// </summary>
        private ConcurrentStack<string> SavePointStack
        {
            get
            {
                var context = CurrentTransactionContext;
                return context?.SavePointStack ?? new ConcurrentStack<string>();
            }
        }

        /// <summary>
        /// 创建新的事务上下文
        /// </summary>
        private TransactionContext CreateTransactionContext()
        {
            var context = new TransactionContext
            {
                CallerMethod = GetCallerMethod(),
                StackTrace = Environment.StackTrace,
                OriginalSqlSugarClient = _sqlSugarClient
            };
            
            return context;
        }

        /// <summary>
        /// 获取调用方法信息
        /// </summary>
        private string GetCallerMethod()
        {
            try
            {
                var stackTrace = new StackTrace(2, false); // 跳过当前方法和调用者
                var frame = stackTrace.GetFrames()
                    ?.FirstOrDefault(f =>
                    {
                        var methodInfo = f.GetMethod();
                        if (methodInfo == null) return false;
                        var declaringType = methodInfo.DeclaringType?.FullName;
                        return !string.IsNullOrEmpty(declaringType) &&
                               !declaringType.StartsWith("RUINORERP.Repository.UnitOfWorks") &&
                               !declaringType.StartsWith("System") &&
                               !declaringType.StartsWith("Microsoft") &&
                               !declaringType.StartsWith("Autofac") &&
                               !declaringType.StartsWith("Castle");
                    });

                if (frame?.GetMethod() is MethodInfo method)
                {
                    return $"{method.DeclaringType?.Name}.{method.Name}";
                }
            }
            catch
            {
                // 忽略异常，返回默认值
            }
            
            return "UnknownMethod";
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

        //
        public void BeginTran()
        {
            lock (this)
            {
                try
                {
                    // 获取或创建事务上下文
                    var context = CurrentTransactionContext;
                    if (context == null)
                    {
                        context = CreateTransactionContext();
                        CurrentTransactionContext = context;
                        _logger.LogDebug($"[Transaction-{context.TransactionId}] 事务上下文已创建，Caller={context.CallerMethod}");
                    }
                    else
                    {
                        _logger.LogDebug($"[Transaction-{context.TransactionId}] 使用现有事务上下文");
                    }

                    // 双重检测：检查物理事务和逻辑状态的一致性
                    if (context.Depth == 0 && _sqlSugarClient.Ado.Transaction != null)
                    {
                        _logger.LogWarning($"[Transaction-{context.TransactionId}] 检测到僵尸事务，强制清理");
                        _logger.LogWarning($"僵尸事务详情: {{LogicalDepth={context.Depth}, PhysicalTransaction={_sqlSugarClient.Ado.Transaction != null}}}");
                        ForceRollback();
                    }

                    // 增加事务深度
                    context.Depth++;
                    context.UpdateActivityTime();
                    context.Status = TransactionStatus.Active;

                    var newDepth = context.Depth;
                    _logger.LogDebug($"[Transaction-{context.TransactionId}] 事务深度增加: {newDepth - 1} -> {newDepth}");

                    // 最外层事务：开启物理事务
                    if (newDepth == 1)
                    {
                        _logger.LogDebug($"[Transaction-{context.TransactionId}] 开启物理事务");
                        _logger.LogDebug($"调用堆栈: {context.CallerMethod}");
                        
                        try
                        {
                            _sqlSugarClient.Ado.BeginTran();
                            
                            // 注册到全局活跃事务字典
                            _activeTransactions.TryAdd(context.TransactionId, context);
                            _logger.LogInformation($"[Transaction-{context.TransactionId}] 物理事务已成功开启");
                        }
                        catch (Exception beginEx)
                        {
                            // 如果BeginTran失败，清理状态
                            context.Depth = 0;
                            context.Status = TransactionStatus.NotStarted;
                            _activeTransactions.TryRemove(context.TransactionId, out _);
                            
                            _logger.LogError(beginEx, $"[Transaction-{context.TransactionId}] 物理事务开启失败，已清理状态");
                            throw;
                        }
                    }
                    // 嵌套事务支持：创建保存点
                    else if (_supportsNestedTransactions)
                    {
                        var savePointName = $"SAVEPOINT_{context.TransactionId}_{newDepth}";
                        _sqlSugarClient.Ado.ExecuteCommand($"SAVE TRANSACTION {savePointName}");
                        context.SavePointStack.Push(savePointName);
                        
                        _logger.LogDebug($"[Transaction-{context.TransactionId}] 创建保存点: {savePointName}");
                    }
                }
                catch (Exception ex)
                {
                    var context = CurrentTransactionContext;
                    if (context != null && context.Depth > 0)
                    {
                        context.Depth = Math.Max(0, context.Depth - 1);
                        if (context.Depth == 0)
                        {
                            context.Status = TransactionStatus.NotStarted;
                        }
                    }
                    
                    _logger.LogError(ex, $"事务开启失败");
                    throw new InvalidOperationException("事务开启失败，请检查前置事务是否正确关闭", ex);
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
                var context = CurrentTransactionContext;
                if (context == null)
                {
                    _logger.LogWarning("提交请求但无事务上下文");
                    return;
                }

                try
                {
                    _logger.LogDebug($"[Transaction-{context.TransactionId}] 准备提交事务，当前深度: {context.Depth}");

                    // 检查回滚标记
                    if (context.ShouldRollback)
                    {
                        _logger.LogWarning($"[Transaction-{context.TransactionId}] 事务已被标记为回滚，跳过提交");
                        return;
                    }

                    if (context.Depth <= 0)
                    {
                        _logger.LogWarning($"[Transaction-{context.TransactionId}] 提交请求但无活动事务，深度={context.Depth}, PhysicalTransaction={_sqlSugarClient.Ado.Transaction != null}");
                        
                        // 如果深度为0但物理事务仍存在，说明状态不一致
                        if (_sqlSugarClient.Ado.Transaction != null)
                        {
                            _logger.LogError($"[Transaction-{context.TransactionId}] 状态不一致：逻辑深度为0但物理事务仍存在，将强制回滚");
                            ForceRollback();
                        }
                        return;
                    }

                    // 减少深度
                    context.Depth--;
                    context.UpdateActivityTime();

                    _logger.LogDebug($"[Transaction-{context.TransactionId}] 事务深度减少: {context.Depth + 1} -> {context.Depth}");

                    // 内层提交：仅更新深度，不提交物理事务
                    if (context.Depth > 0)
                    {
                        _logger.LogDebug($"[Transaction-{context.TransactionId}] 嵌套事务提交完成（未提交物理事务）");
                        return;
                    }

                    // === 最外层提交逻辑 ===
                    _logger.LogDebug($"[Transaction-{context.TransactionId}] 准备提交物理事务");
                    _logger.LogDebug($"事务持续时间: {context.GetDuration().TotalSeconds:F2} 秒");
                    
                    try
                    {
                        _sqlSugarClient.Ado.CommitTran();
                        context.Status = TransactionStatus.Committed;
                        
                        _logger.LogInformation($"[Transaction-{context.TransactionId}] 物理事务已成功提交");
                    }
                    catch (Exception commitEx)
                    {
                        context.Status = TransactionStatus.RolledBack;
                        _logger.LogError(commitEx, $"[Transaction-{context.TransactionId}] 物理事务提交失败");
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"[Transaction-{context.TransactionId}] 事务提交失败，将执行强制回滚");
                    context.Status = TransactionStatus.RolledBack;
                    ForceRollback();
                    throw new InvalidOperationException($"事务提交失败: {ex.Message}", ex);
                }
                finally
                {
                    // 仅在最外层重置状态
                    if (context.Depth <= 0)
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
                var context = CurrentTransactionContext;
                if (context == null)
                {
                    _logger.LogWarning("回滚请求但无事务上下文");
                    return;
                }

                try
                {
                    _logger.LogDebug($"[Transaction-{context.TransactionId}] 准备回滚事务，当前深度: {context.Depth}");

                    // 设置回滚标记（阻止后续提交）
                    context.ShouldRollback = true;
                    context.UpdateActivityTime();

                    if (context.Depth <= 0)
                    {
                        _logger.LogWarning($"[Transaction-{context.TransactionId}] 回滚请求但无活动事务，深度={context.Depth}, PhysicalTransaction={_sqlSugarClient.Ado.Transaction != null}");
                        
                        // 如果深度为0但仍有物理事务，说明是异常状态，强制清理
                        if (_sqlSugarClient.Ado.Transaction != null)
                        {
                            _logger.LogError($"[Transaction-{context.TransactionId}] 状态不一致：逻辑深度为0但物理事务仍存在");
                            ForceRollback();
                        }
                        return;
                    }

                    var currentDepth = context.Depth;
                    context.Depth--;

                    _logger.LogDebug($"[Transaction-{context.TransactionId}] 事务深度减少: {currentDepth} -> {context.Depth}");

                    // 最外层回滚
                    if (currentDepth == 1)
                    {
                        _logger.LogDebug($"[Transaction-{context.TransactionId}] 准备回滚物理事务");
                        
                        try
                        {
                            _sqlSugarClient.Ado.RollbackTran();
                            context.Status = TransactionStatus.RolledBack;
                            
                            _logger.LogInformation($"[Transaction-{context.TransactionId}] 物理事务已成功回滚");
                        }
                        catch (Exception rollbackEx)
                        {
                            context.Status = TransactionStatus.RolledBack;
                            _logger.LogError(rollbackEx, $"[Transaction-{context.TransactionId}] 物理事务回滚失败");
                            throw;
                        }
                        
                        return;
                    }

                    // 嵌套事务支持：回滚到保存点
                    if (_supportsNestedTransactions && context.SavePointStack.TryPop(out var savePoint))
                    {
                        try
                        {
                            _sqlSugarClient.Ado.ExecuteCommand($"ROLLBACK TRANSACTION {savePoint}");
                            _logger.LogWarning($"[Transaction-{context.TransactionId}] 回滚到保存点: {savePoint}");
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
                    _logger.LogError(ex, $"[Transaction-{context.TransactionId}] 回滚操作失败，将执行强制回滚");
                    context.Status = TransactionStatus.RolledBack;
                    ForceRollback();
                    throw new InvalidOperationException($"事务回滚失败: {ex.Message}", ex);
                }
                finally
                {
                    // 仅在最外层重置状态
                    if (context.Depth <= 0)
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
            var context = CurrentTransactionContext;
            if (context != null)
            {
                // 从全局活跃事务字典中移除
                _activeTransactions.TryRemove(context.TransactionId, out _);
                
                var transactionId = context.TransactionId;
                var duration = context.GetDuration().TotalSeconds;
                
                // 清理AsyncLocal上下文
                CurrentTransactionContext = null;
                
                _logger.LogDebug($"[Transaction-{transactionId}] 事务状态已重置（持续时间: {duration:F2} 秒）");
            }
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
