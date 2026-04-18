using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using RUINORERP.Repository.UnitOfWorks;

namespace RUINORERP.Tests
{
    /// <summary>
    /// 自动超时功能使用示例
    /// </summary>
    public class AutoTimeoutExamples
    {
        private readonly IUnitOfWorkManage _unitOfWork;
        
        public AutoTimeoutExamples(IUnitOfWorkManage unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region 示例1: 基本用法 - 默认超时

        /// <summary>
        /// 使用配置的默认超时时间(60秒)
        /// 如果忘记提交/回滚,60秒后会自动回滚
        /// </summary>
        public async Task Example1_BasicUsage()
        {
            // ✅ 推荐: 使用 using 语句确保资源释放
            await _unitOfWork.BeginTranAsync();  // 默认60秒超时
            try
            {
                // 执行业务逻辑
                await DoSomeWork();
                
                // 提交事务
                await _unitOfWork.CommitTranAsync();
            }
            catch (Exception ex)
            {
                // 发生异常时回滚
                await _unitOfWork.RollbackTranAsync();
                
                // 记录日志或重新抛出
                Console.WriteLine($"事务失败: {ex.Message}");
                throw;
            }
            
            // ⚠️ 如果忘记调用 Commit/Rollback:
            // - 60秒后会触发超时回调
            // - 自动执行回滚
            // - 记录错误日志
        }

        #endregion

        #region 示例2: 自定义超时时间

        /// <summary>
        /// 为不同场景设置不同的超时时间
        /// </summary>
        public async Task Example2_CustomTimeout()
        {
            // 场景1: 快速操作 - 30秒超时
            await _unitOfWork.BeginTranAsync(timeoutSeconds: 30);
            try
            {
                await QuickOperation();
                await _unitOfWork.CommitTranAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTranAsync();
                throw;
            }

            // 场景2: 复杂业务 - 5分钟超时
            await _unitOfWork.BeginTranAsync(timeoutSeconds: 300);
            try
            {
                await ComplexBusinessLogic();
                await _unitOfWork.CommitTranAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTranAsync();
                throw;
            }

            // 场景3: 禁用超时(不推荐,仅用于特殊场景)
            await _unitOfWork.BeginTranAsync(timeoutSeconds: null);
            try
            {
                await VeryLongOperation();
                await _unitOfWork.CommitTranAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTranAsync();
                throw;
            }
        }

        #endregion

        #region 示例3: 同步版本

        /// <summary>
        /// 同步方法也支持超时
        /// </summary>
        public void Example3_SyncVersion()
        {
            // 同步版本同样支持超时参数
            _unitOfWork.BeginTran(timeoutSeconds: 60);
            try
            {
                DoSyncWork();
                _unitOfWork.CommitTran();
            }
            catch
            {
                _unitOfWork.RollbackTran();
                throw;
            }
        }

        #endregion

        #region 示例4: 配合 using 语句(最佳实践)

        /// <summary>
        /// 使用 using 语句是最安全的做法
        /// 即使忘记提交/回滚,Dispose 也会处理
        /// </summary>
        public async Task Example4_UsingStatement(IServiceProvider serviceProvider)
        {
            // ✅ 最佳实践: using + 超时
            using var unitOfWork = serviceProvider.GetRequiredService<IUnitOfWorkManage>();
            
            await unitOfWork.BeginTranAsync(timeoutSeconds: 60);
            try
            {
                await DoWork();
                await unitOfWork.CommitTranAsync();
            }
            catch
            {
                await unitOfWork.RollbackTranAsync();
                throw;
            }
            
            // 离开作用域时:
            // 1. 如果事务未完成,Dispose 会检测到
            // 2. 记录警告日志
            // 3. 执行强制回滚
            // 4. 清理所有资源(包括超时机制)
        }

        #endregion

        #region 示例5: 嵌套事务

        /// <summary>
        /// 嵌套事务的超时由最外层决定
        /// </summary>
        public async Task Example5_NestedTransaction()
        {
            // 外层事务: 120秒超时
            await _unitOfWork.BeginTranAsync(timeoutSeconds: 120);
            try
            {
                await OuterWork();
                
                // 内层事务: timeoutSeconds 参数会被忽略
                // 超时仍由外层的120秒控制
                await _unitOfWork.BeginTranAsync(timeoutSeconds: 30);  // 这个30秒不会生效
                try
                {
                    await InnerWork();
                    await _unitOfWork.CommitTranAsync();  // 内层提交(实际是保存点)
                }
                catch
                {
                    await _unitOfWork.RollbackTranAsync();  // 内层回滚(回滚到保存点)
                    throw;
                }
                
                await _unitOfWork.CommitTranAsync();  // 外层提交
            }
            catch
            {
                await _unitOfWork.RollbackTranAsync();  // 外层回滚
                throw;
            }
        }

        #endregion

        #region 示例6: 重试机制 + 超时

        /// <summary>
        /// 结合重试机制和超时
        /// </summary>
        public async Task Example6_RetryWithTimeout()
        {
            int maxRetries = 3;
            
            for (int retry = 0; retry < maxRetries; retry++)
            {
                try
                {
                    // 每次重试都使用新的超时
                    await _unitOfWork.BeginTranAsync(timeoutSeconds: 60);
                    try
                    {
                        await RiskyOperation();  // 可能死锁的操作
                        await _unitOfWork.CommitTranAsync();
                        return;  // 成功,退出
                    }
                    catch
                    {
                        await _unitOfWork.RollbackTranAsync();
                        throw;
                    }
                }
                catch (Exception ex) when (IsDeadlock(ex) && retry < maxRetries - 1)
                {
                    // 死锁,等待后重试
                    Console.WriteLine($"检测到死锁,第 {retry + 1} 次重试...");
                    await Task.Delay(100 * (int)Math.Pow(2, retry));  // 指数退避
                }
            }
            
            throw new InvalidOperationException("重试次数已用尽");
        }

        private bool IsDeadlock(Exception ex)
        {
            // 检查是否是死锁异常
            return ex.Message.Contains("deadlock") || ex.Message.Contains("1205");
        }

        #endregion

        #region 示例7: 监控长事务

        /// <summary>
        /// 演示长事务的监控和告警
        /// </summary>
        public async Task Example7_MonitorLongTransaction()
        {
            // 配置: LongTransactionWarningSeconds = 60
            //         CriticalTransactionWarningSeconds = 300
            
            await _unitOfWork.BeginTranAsync(timeoutSeconds: 600);  // 10分钟超时
            try
            {
                // 模拟长时间运行的操作
                await Task.Delay(70000);  // 70秒
                
                // 此时会看到警告日志:
                // "[Transaction-xxx] ⚠️ 长事务警告: 已运行 70秒 (>60秒)"
                
                await DoLongWork();
                
                await Task.Delay(250000);  // 再等250秒,总共320秒
                
                // 此时会看到错误日志:
                // "[Transaction-xxx] 🚨 超长事务警告! 已运行 320秒 (>300秒)"
                
                await _unitOfWork.CommitTranAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTranAsync();
                throw;
            }
        }

        #endregion

        #region 示例8: 批量处理中的超时

        /// <summary>
        /// 批量处理时,每个批次独立事务和超时
        /// </summary>
        public async Task Example8_BatchProcessing()
        {
            var items = GetLargeDataSet();  // 假设有10000条数据
            int batchSize = 100;
            
            for (int i = 0; i < items.Count; i += batchSize)
            {
                var batch = items.GetRange(i, Math.Min(batchSize, items.Count - i));
                
                // 每个批次独立事务,独立超时
                await _unitOfWork.BeginTranAsync(timeoutSeconds: 30);
                try
                {
                    foreach (var item in batch)
                    {
                        await ProcessItem(item);
                    }
                    
                    await _unitOfWork.CommitTranAsync();
                    Console.WriteLine($"批次 {i / batchSize + 1} 处理完成");
                }
                catch
                {
                    await _unitOfWork.RollbackTranAsync();
                    Console.WriteLine($"批次 {i / batchSize + 1} 处理失败,已回滚");
                    throw;
                }
            }
        }

        #endregion

        #region 辅助方法

        private Task DoSomeWork() => Task.CompletedTask;
        private Task QuickOperation() => Task.CompletedTask;
        private Task ComplexBusinessLogic() => Task.CompletedTask;
        private Task VeryLongOperation() => Task.CompletedTask;
        private void DoSyncWork() { }
        private Task DoWork() => Task.CompletedTask;
        private Task OuterWork() => Task.CompletedTask;
        private Task InnerWork() => Task.CompletedTask;
        private Task RiskyOperation() => Task.CompletedTask;
        private Task DoLongWork() => Task.CompletedTask;
        private System.Collections.Generic.List<object> GetLargeDataSet() => new();
        private Task ProcessItem(object item) => Task.CompletedTask;

        #endregion
    }

    /// <summary>
    /// 配置示例
    /// </summary>
    public static class ConfigurationExample
    {
        /// <summary>
        /// 在 Program.cs 或 Startup.cs 中配置
        /// </summary>
        public static void ConfigureUnitOfWork(Microsoft.Extensions.DependencyInjection.IServiceCollection services, 
            Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            // 方式1: 从配置文件读取
            services.Configure<UnitOfWorkOptions>(
                configuration.GetSection("UnitOfWork"));

            // 方式2: 硬编码配置
            services.Configure<UnitOfWorkOptions>(options =>
            {
                // 启用自动超时
                options.EnableAutoTransactionTimeout = true;
                
                // 超时后强制回滚
                options.ForceRollbackOnTimeout = true;
                
                // 默认超时60秒
                options.DefaultTransactionTimeoutSeconds = 60;
                
                // 超过60秒记录警告
                options.LongTransactionWarningSeconds = 60;
                
                // 超过300秒记录错误
                options.CriticalTransactionWarningSeconds = 300;
                
                // 其他配置...
                options.MaxTransactionDepth = 15;
                options.MaxRetryCount = 3;
                options.EnableTransactionMetrics = true;
            });
        }
    }
}

/*
 * appsettings.json 配置示例:
 * 
{
  "UnitOfWork": {
    "EnableAutoTransactionTimeout": true,
    "ForceRollbackOnTimeout": true,
    "DefaultTransactionTimeoutSeconds": 60,
    "LongTransactionWarningSeconds": 60,
    "CriticalTransactionWarningSeconds": 300,
    "MaxTransactionDepth": 15,
    "MaxRetryCount": 3,
    "InitialRetryDelayMs": 100,
    "MaxRetryDelayMs": 2000,
    "EnableTransactionMetrics": true,
    "RetryableSqlErrorCodes": [1205, 1222, 40197, 40501, 40613, -2]
  }
}
 */
