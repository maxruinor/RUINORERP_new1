using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace RUINORERP.Business.Cache.Core
{
    /// <summary>
    /// 缓存使用示例
    /// 演示如何使用统一缓存服务
    /// </summary>
    public class CacheUsageExample
    {
        private readonly UnifiedCacheService _cacheService;
        private readonly ILogger<CacheUsageExample> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cacheService">统一缓存服务</param>
        /// <param name="logger">日志记录器</param>
        public CacheUsageExample(UnifiedCacheService cacheService, ILogger<CacheUsageExample> logger)
        {
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// 运行缓存示例
        /// </summary>
        public async Task RunExampleAsync()
        {
            _logger.LogInformation("开始运行缓存使用示例");

            try
            {
                // 1. 基本缓存操作示例
                await BasicCacheOperationsExample();

                // 2. 客户端缓存管理示例
                await ClientCacheManagementExample();

                // 3. 缓存同步示例（需要实际的通讯服务）
                // await CacheSyncExample();

                _logger.LogInformation("完成运行缓存使用示例");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "运行缓存使用示例时发生错误");
            }
        }

        /// <summary>
        /// 基本缓存操作示例
        /// </summary>
        private async Task BasicCacheOperationsExample()
        {
            _logger.LogInformation("开始基本缓存操作示例");

            // 设置缓存项
            _cacheService.Set("user_name", "张三");
            _cacheService.Set("user_age", 25);
            _cacheService.Set("user_balance", 1000.50m, TimeSpan.FromMinutes(10));

            // 获取缓存项
            var userName = _cacheService.Get<string>("user_name");
            var userAge = _cacheService.Get<int>("user_age");
            var userBalance = _cacheService.Get<decimal>("user_balance");

            _logger.LogInformation($"获取缓存项: 姓名={userName}, 年龄={userAge}, 余额={userBalance}");

            // 异步设置缓存项
            await _cacheService.SetAsync("async_key", "异步值");

            // 异步获取缓存项
            var asyncValue = await _cacheService.GetAsync<string>("async_key");
            _logger.LogInformation($"异步获取缓存项: {asyncValue}");

            // 添加或更新缓存项
            var result = _cacheService.AddOrUpdate("counter", 1, value => value + 1);
            _logger.LogInformation($"添加或更新缓存项结果: {result}");

            var counterValue = _cacheService.Get<int>("counter");
            _logger.LogInformation($"计数器值: {counterValue}");

            // 检查缓存项是否存在
            var exists = _cacheService.Exists("user_name");
            _logger.LogInformation($"缓存项'user_name'是否存在: {exists}");

            // 获取所有缓存键
            var keys = _cacheService.GetKeys();
            _logger.LogInformation($"当前缓存键数量: {keys.Count()}");

            // 移除缓存项
            var removed = _cacheService.Remove("user_name");
            _logger.LogInformation($"移除缓存项'user_name'结果: {removed}");

            // 异步移除缓存项
            var asyncRemoved = await _cacheService.RemoveAsync("async_key");
            _logger.LogInformation($"异步移除缓存项'async_key'结果: {asyncRemoved}");

            _logger.LogInformation("完成基本缓存操作示例");
        }

        /// <summary>
        /// 客户端缓存管理示例
        /// </summary>
        private async Task ClientCacheManagementExample()
        {
            _logger.LogInformation("开始客户端缓存管理示例");

            // 使用客户端缓存管理器
            var clientCacheManager = _cacheService.ClientCacheManager;

            // 设置缓存项
            clientCacheManager.Set("client_key", "客户端值", TimeSpan.FromMinutes(5));

            // 异步设置缓存项
            await clientCacheManager.SetAsync("async_client_key", "异步客户端值");

            // 批量设置缓存项
            var batchItems = new System.Collections.Generic.Dictionary<string, int>
            {
                {"item1", 100},
                {"item2", 200},
                {"item3", 300}
            };
            clientCacheManager.SetBatch(batchItems, TimeSpan.FromMinutes(15));

            // 异步批量设置缓存项
            var asyncBatchItems = new System.Collections.Generic.Dictionary<string, string>
            {
                {"async_item1", "值1"},
                {"async_item2", "值2"},
                {"async_item3", "值3"}
            };
            await clientCacheManager.SetBatchAsync(asyncBatchItems);

            // 获取缓存项
            var clientValue = clientCacheManager.Get<string>("client_key");
            _logger.LogInformation($"客户端缓存值: {clientValue}");

            // 异步获取缓存项
            var asyncClientValue = await clientCacheManager.GetAsync<string>("async_client_key");
            _logger.LogInformation($"异步客户端缓存值: {asyncClientValue}");

            // 检查缓存项是否存在
            var exists = clientCacheManager.Exists("client_key");
            _logger.LogInformation($"客户端缓存项'client_key'是否存在: {exists}");

            // 获取缓存统计信息
            var statistics = clientCacheManager.GetStatistics();
            _logger.LogInformation($"客户端缓存统计: 总项数={statistics.TotalItems}, 已过期={statistics.ExpiredItems}");

            _logger.LogInformation("完成客户端缓存管理示例");
        }

        /// <summary>
        /// 缓存同步示例
        /// </summary>
        private async Task CacheSyncExample()
        {
            _logger.LogInformation("开始缓存同步示例");

            try
            {
                // 同步所有缓存到服务器
                await _cacheService.SyncAllCacheToServerAsync();

                // 从服务器同步缓存
                await _cacheService.SyncCacheFromServerAsync();

                // 订阅服务器缓存变更
                await _cacheService.SubscribeToServerCacheAsync(new[] { "user_*", "product_*" });

                // 取消订阅服务器缓存变更
                await _cacheService.UnsubscribeFromServerCacheAsync();

                _logger.LogInformation("完成缓存同步示例");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "缓存同步示例执行失败");
            }
        }
    }

    /// <summary>
    /// 缓存使用示例程序
    /// </summary>
    public class CacheUsageProgram
    {
        /// <summary>
        /// 主方法
        /// </summary>
        public static async Task Main(string[] args)
        {
            // 创建服务容器
            var services = new ServiceCollection();

            // 配置日志
            services.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.SetMinimumLevel(LogLevel.Debug);
            });

            // 添加统一缓存服务
            services.AddUnifiedCacheService();

            // 添加示例类
            services.AddTransient<CacheUsageExample>();

            // 构建服务提供者
            var serviceProvider = services.BuildServiceProvider();

            // 获取示例实例并运行
            var example = serviceProvider.GetRequiredService<CacheUsageExample>();
            await example.RunExampleAsync();
        }
    }
}