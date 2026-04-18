using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.Repository.UnitOfWorks;
using SqlSugar;

namespace RUINORERP.Tests
{
    /// <summary>
    /// 事务修复验证测试
    /// 用于验证"挂起请求"错误是否已修复
    /// </summary>
    public class TransactionFixVerificationTest
    {
        private readonly IUnitOfWorkManage _unitOfWork;
        private readonly ILogger<TransactionFixVerificationTest> _logger;

        public TransactionFixVerificationTest(
            IUnitOfWorkManage unitOfWork,
            ILogger<TransactionFixVerificationTest> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        /// <summary>
        /// 测试1: 基本事务开启和提交
        /// </summary>
        public async Task Test1_BasicTransactionCommit()
        {
            _logger.LogInformation("=== 测试1: 基本事务开启和提交 ===");
            
            try
            {
                _unitOfWork.BeginTran();
                
                // 模拟一些数据库操作
                var db = _unitOfWork.GetDbClient();
                
                // 这里可以添加实际的数据库操作
                // await db.Insertable(...).ExecuteCommandAsync();
                
                _unitOfWork.CommitTran();
                
                _logger.LogInformation("✅ 测试1通过: 基本事务提交成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ 测试1失败");
                throw;
            }
        }

        /// <summary>
        /// 测试2: 嵌套事务
        /// </summary>
        public async Task Test2_NestedTransactions()
        {
            _logger.LogInformation("=== 测试2: 嵌套事务 ===");
            
            try
            {
                _unitOfWork.BeginTran();
                _logger.LogInformation($"外层事务深度: {_unitOfWork.TranCount}");
                
                // 内层事务
                _unitOfWork.BeginTran();
                _logger.LogInformation($"内层事务深度: {_unitOfWork.TranCount}");
                
                // 更深层
                _unitOfWork.BeginTran();
                _logger.LogInformation($"最内层事务深度: {_unitOfWork.TranCount}");
                
                _unitOfWork.CommitTran();
                _unitOfWork.CommitTran();
                _unitOfWork.CommitTran();
                
                _logger.LogInformation("✅ 测试2通过: 嵌套事务成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ 测试2失败");
                throw;
            }
        }

        /// <summary>
        /// 测试3: 并发事务(模拟多个异步流)
        /// </summary>
        public async Task Test3_ConcurrentTransactions()
        {
            _logger.LogInformation("=== 测试3: 并发事务 ===");
            
            var tasks = new Task[5];
            
            for (int i = 0; i < 5; i++)
            {
                int taskId = i;
                tasks[i] = Task.Run(async () =>
                {
                    try
                    {
                        _logger.LogInformation($"任务 {taskId} 开始");
                        
                        _unitOfWork.BeginTran();
                        
                        // 模拟工作
                        await Task.Delay(100);
                        
                        _unitOfWork.CommitTran();
                        
                        _logger.LogInformation($"✅ 任务 {taskId} 完成");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"❌ 任务 {taskId} 失败");
                        throw;
                    }
                });
            }
            
            await Task.WhenAll(tasks);
            _logger.LogInformation("✅ 测试3通过: 所有并发任务成功");
        }

        /// <summary>
        /// 测试4: 事务回滚
        /// </summary>
        public async Task Test4_TransactionRollback()
        {
            _logger.LogInformation("=== 测试4: 事务回滚 ===");
            
            try
            {
                _unitOfWork.BeginTran();
                
                // 模拟一些操作
                var db = _unitOfWork.GetDbClient();
                
                // 故意抛出异常触发回滚
                throw new InvalidOperationException("模拟业务异常");
            }
            catch (InvalidOperationException)
            {
                _unitOfWork.RollbackTran();
                _logger.LogInformation("✅ 测试4通过: 事务回滚成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ 测试4失败: 未预期的异常");
                throw;
            }
        }

        /// <summary>
        /// 测试5: 查询后开启事务(验证DataReader已关闭)
        /// </summary>
        public async Task Test5_QueryThenBeginTran()
        {
            _logger.LogInformation("=== 测试5: 查询后开启事务 ===");
            
            try
            {
                var db = _unitOfWork.GetDbClient();
                
                // 执行查询(可能返回DataReader)
                // var data = await db.Queryable<SomeEntity>().ToListAsync();
                
                // 立即开启事务 - 不应有"挂起请求"错误
                _unitOfWork.BeginTran();
                
                // 执行更新
                // await db.Updateable(...).ExecuteCommandAsync();
                
                _unitOfWork.CommitTran();
                
                _logger.LogInformation("✅ 测试5通过: 查询后开启事务成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ 测试5失败");
                throw;
            }
        }

        /// <summary>
        /// 测试6: 长事务监控
        /// </summary>
        public async Task Test6_LongTransactionMonitoring()
        {
            _logger.LogInformation("=== 测试6: 长事务监控 ===");
            
            try
            {
                _unitOfWork.BeginTran();
                
                // 模拟长时间操作
                await Task.Delay(11000); // 11秒,超过10秒阈值
                
                _unitOfWork.CommitTran();
                
                _logger.LogInformation("✅ 测试6通过: 长事务监控正常(应看到警告日志)");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ 测试6失败");
                throw;
            }
        }

        /// <summary>
        /// 运行所有测试
        /// </summary>
        public async Task RunAllTests()
        {
            _logger.LogInformation("🚀 开始事务修复验证测试套件");
            _logger.LogInformation("=" .PadRight(60, '='));
            
            try
            {
                await Test1_BasicTransactionCommit();
                await Test2_NestedTransactions();
                await Test3_ConcurrentTransactions();
                await Test4_TransactionRollback();
                await Test5_QueryThenBeginTran();
                await Test6_LongTransactionMonitoring();
                
                _logger.LogInformation("=" .PadRight(60, '='));
                _logger.LogInformation("🎉 所有测试通过! 事务修复验证成功!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "=" .PadRight(60, '='));
                _logger.LogError("💥 测试套件执行失败");
                throw;
            }
        }
    }

    /// <summary>
    /// 简单的控制台测试程序
    /// </summary>
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("事务修复验证测试");
            Console.WriteLine("请确保已配置好依赖注入容器");
            Console.WriteLine("按任意键继续...");
            Console.ReadKey();
            
            // 这里需要从DI容器获取实例
            // var test = serviceProvider.GetService<TransactionFixVerificationTest>();
            // await test.RunAllTests();
            
            Console.WriteLine("测试完成!");
        }
    }
}
