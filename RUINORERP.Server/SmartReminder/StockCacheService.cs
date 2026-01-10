using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using RUINORERP.Model;
using RUINORERP.Model.Context;
using RUINORERP.Repository.UnitOfWorks;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;

namespace RUINORERP.Server.SmartReminder
{
    public interface IStockCacheService
    {
        /// <summary>
        /// 获取单个产品库存信息
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <returns>库存信息</returns>
        Task<tb_Inventory> GetStockAsync(long productId);
        
        /// <summary>
        /// 批量获取产品库存信息
        /// </summary>
        /// <param name="productIds">产品ID列表</param>
        /// <returns>库存信息字典</returns>
        Task<Dictionary<long, tb_Inventory>> GetStocksAsync(IEnumerable<long> productIds);
        
        /// <summary>
        /// 刷新单个产品的缓存
        /// </summary>
        /// <param name="productId">产品ID</param>
        Task RefreshStockCacheAsync(long productId);
        
        /// <summary>
        /// 刷新多个产品的缓存
        /// </summary>
        /// <param name="productIds">产品ID列表</param>
        Task RefreshStockCachesAsync(IEnumerable<long> productIds);
        
        /// <summary>
        /// 移除单个产品缓存
        /// </summary>
        /// <param name="productId">产品ID</param>
        void RemoveStockCache(long productId);
        
        /// <summary>
        /// 预热缓存
        /// </summary>
        Task PreheatCacheAsync(int batchSize = 100);
        
        /// <summary>
        /// 获取缓存统计信息
        /// </summary>
        CacheStatistics GetCacheStatistics();
    }
    
    /// <summary>
    /// 缓存统计信息类
    /// </summary>
    public class CacheStatistics
    {
        public int CacheHits { get; set; }
        public int CacheMisses { get; set; }
        public int TotalRequests { get; set; }
        public double HitRatio => TotalRequests > 0 ? (double)CacheHits / TotalRequests : 0;
        public int CurrentCacheSize { get; set; }
    }

    public class StockCacheService : IStockCacheService
    {
        private readonly IMemoryCache _cache;
        public readonly IUnitOfWorkManage _unitOfWorkManage;
        private readonly ApplicationContext _appContext;
        private readonly ILogger<StockCacheService> _logger;
        
        // 缓存配置常量
        private const string STOCK_CACHE_PREFIX = "stock_";
        private const int DEFAULT_CACHE_EXPIRATION_SECONDS = 30;
        private const int HIGH_PRIORITY_CACHE_EXPIRATION_SECONDS = 60;
        private const int BULK_QUERY_BATCH_SIZE = 100;
        private const int PREHEAT_BATCH_SIZE = 100;  // 降低批次大小从500到100，减少启动时内存占用
        private const int MAX_PREHEAT_COUNT = 10000;  // 最大预热数量限制，防止过度预热
        private const int PREHEAT_DELAY_MS = 100;  // 批次间延迟，避免数据库压力过大
        
        // 缓存统计信息
        private readonly CacheStatistics _statistics = new CacheStatistics();
        private readonly ConcurrentDictionary<string, bool> _cacheKeys = new ConcurrentDictionary<string, bool>();
        private readonly ReaderWriterLockSlim _statisticsLock = new ReaderWriterLockSlim();
        
        // 预热相关配置
        private bool _isPreheating = false;
        private readonly SemaphoreSlim _preheatSemaphore = new SemaphoreSlim(1, 1);

        public StockCacheService(
            IMemoryCache cache,
         ApplicationContext _AppContextData, IUnitOfWorkManage unitOfWorkManage,
            ILogger<StockCacheService> logger)
        {
            _cache = cache;
            _appContext = _AppContextData;
            _unitOfWorkManage = unitOfWorkManage;
            _logger = logger;
            
            // 初始化缓存配置
            InitializeCache();
        }
        
        private void InitializeCache()
        {
            // 可以在这里添加缓存条目优先级策略
            _logger.LogInformation("库存缓存服务已初始化");
        }
        
        public async Task<tb_Inventory> GetStockAsync(long productId)
        {
            try
            {
                IncrementRequestCount();
                
                string cacheKey = $"{STOCK_CACHE_PREFIX}{productId}";
                
                // 先检查缓存是否存在
                if (_cache.TryGetValue(cacheKey, out tb_Inventory cachedStock))
                {
                    IncrementCacheHit();
                    _logger.LogDebug("缓存命中: ProductID={ProductId}", productId);
                    return cachedStock;
                }
                
                IncrementCacheMiss();
                _logger.LogDebug("缓存未命中: ProductID={ProductId}", productId);
                
                // 缓存未命中，从数据库获取
                tb_Inventory stock = await LoadStockFromDatabaseAsync(productId);
                
                if (stock != null)
                {
                    // 根据业务重要性设置不同的过期时间
                    TimeSpan expiration = IsHighPriorityProduct(productId) 
                        ? TimeSpan.FromSeconds(HIGH_PRIORITY_CACHE_EXPIRATION_SECONDS) 
                        : TimeSpan.FromSeconds(DEFAULT_CACHE_EXPIRATION_SECONDS);
                    
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(expiration)
                        .SetSlidingExpiration(TimeSpan.FromSeconds(10))
                        .RegisterPostEvictionCallback(OnCacheEvicted)
                        .SetPriority(IsHighPriorityProduct(productId) 
                            ? CacheItemPriority.High 
                            : CacheItemPriority.Normal);
                    
                    _cache.Set(cacheKey, stock, cacheEntryOptions);
                    _cacheKeys.TryAdd(cacheKey, true);
                    
                    // 更新统计信息
                    UpdateCacheSize();
                }
                
                return stock;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取库存缓存失败 ProductID={ProductId}", productId);
                return null;
            }
        }
        
        public async Task<Dictionary<long, tb_Inventory>> GetStocksAsync(IEnumerable<long> productIds)
        {
            if (productIds == null || !productIds.Any())
                return new Dictionary<long, tb_Inventory>();
            
            var result = new Dictionary<long, tb_Inventory>();
            var missingProductIds = new List<long>();
            
            // 统计请求
            IncrementRequestCount(productIds.Count());
            
            // 首先尝试从缓存获取
            foreach (long productId in productIds)
            {
                string cacheKey = $"{STOCK_CACHE_PREFIX}{productId}";
                if (_cache.TryGetValue(cacheKey, out tb_Inventory cachedStock))
                {
                    result[productId] = cachedStock;
                    IncrementCacheHit();
                    _logger.LogDebug("批量获取缓存命中: ProductID={ProductId}", productId);
                }
                else
                {
                    missingProductIds.Add(productId);
                    IncrementCacheMiss();
                }
            }
            
            // 批量查询缺失的数据
            if (missingProductIds.Any())
            {
                var missingStocks = await LoadStocksFromDatabaseAsync(missingProductIds).ConfigureAwait(false);

                // 更新缓存
                foreach (var stock in missingStocks)
                {
                    if (stock != null)
                    {
                        await RefreshStockCacheInternalAsync(stock).ConfigureAwait(false);
                        result[stock.ProdDetailID] = stock;
                    }
                }
            }
            
            return result;
        }
        
        public async Task RefreshStockCacheAsync(long productId)
        {
            tb_Inventory stock = await LoadStockFromDatabaseAsync(productId).ConfigureAwait(false);
            if (stock != null)
            {
                await RefreshStockCacheInternalAsync(stock).ConfigureAwait(false);
                _logger.LogInformation("库存缓存已刷新: ProductID={ProductId}", productId);
            }
        }
        
        public async Task RefreshStockCachesAsync(IEnumerable<long> productIds)
        {
            if (productIds == null || !productIds.Any())
                return;

            var stocks = await LoadStocksFromDatabaseAsync(productIds).ConfigureAwait(false);

            foreach (var stock in stocks)
            {
                if (stock != null)
                {
                    await RefreshStockCacheInternalAsync(stock).ConfigureAwait(false);
                }
            }

            _logger.LogInformation("批量库存缓存已刷新: 数量={Count}", stocks.Count);
        }
        
        public void RemoveStockCache(long productId)
        {
            string cacheKey = $"{STOCK_CACHE_PREFIX}{productId}";
            _cache.Remove(cacheKey);
            _cacheKeys.TryRemove(cacheKey, out _);
            UpdateCacheSize();
            _logger.LogInformation("库存缓存已移除: ProductID={ProductId}", productId);
        }
        
        public async Task PreheatCacheAsync(int batchSize = 100)
        {
            // 使用信号量防止并发预热
            if (!await _preheatSemaphore.WaitAsync(0))
            {
                _logger.LogInformation("缓存预热已经在进行中");
                return;
            }
            
            try
            {
                if (_isPreheating)
                {
                    _logger.LogInformation("缓存预热已经在进行中");
                    return;
                }
                
                _isPreheating = true;
                _logger.LogInformation("开始缓存预热");

                int totalPreheated = 0;
                int batchNumber = 1;

                // 分批获取产品ID并预热缓存，添加最大数量限制
                while (totalPreheated < MAX_PREHEAT_COUNT)
                {
                    var productIds = await GetProductIdsForPreheatAsync(batchNumber, batchSize).ConfigureAwait(false);
                    if (!productIds.Any())
                        break;

                    await GetStocksAsync(productIds).ConfigureAwait(false);
                    totalPreheated += productIds.Count;

                    _logger.LogInformation("缓存预热批次 {BatchNumber} 已完成，预热数量: {Count}, 累计预热: {Total}",
                        batchNumber, productIds.Count, totalPreheated);

                    batchNumber++;

                    // 达到最大预热数量，停止预热
                    if (totalPreheated >= MAX_PREHEAT_COUNT)
                    {
                        _logger.LogInformation("已达到最大预热数量限制 {MaxCount}", MAX_PREHEAT_COUNT);
                        break;
                    }

                    // 使用配置的批次间延迟，避免数据库压力过大
                    await Task.Delay(PREHEAT_DELAY_MS).ConfigureAwait(false);
                }

                _logger.LogInformation("缓存预热完成，共预热 {Count} 条记录", totalPreheated);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "缓存预热失败");
            }
            finally
            {
                _isPreheating = false;
                _preheatSemaphore.Release();
            }
        }
        
        public CacheStatistics GetCacheStatistics()
        {
            _statisticsLock.EnterReadLock();
            try
            {
                return new CacheStatistics
                {
                    CacheHits = _statistics.CacheHits,
                    CacheMisses = _statistics.CacheMisses,
                    TotalRequests = _statistics.TotalRequests,
                    CurrentCacheSize = _cacheKeys.Count
                };
            }
            finally
            {
                _statisticsLock.ExitReadLock();
            }
        }
        
        #region 私有辅助方法
        
        private async Task<tb_Inventory> LoadStockFromDatabaseAsync(long productId)
        {
            try
            {
                return await _unitOfWorkManage.GetDbClient().Queryable<tb_Inventory>()
                    .FirstAsync(p => p.ProdDetailID == productId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "从数据库加载库存失败 ProductID={ProductId}", productId);
                return null;
            }
        }
        
        private async Task<List<tb_Inventory>> LoadStocksFromDatabaseAsync(IEnumerable<long> productIds)
        {
            try
            {
                // 分批查询以避免SQL查询过大
                var batches = productIds
                    .Select((id, index) => new { id, index })
                    .GroupBy(x => x.index / BULK_QUERY_BATCH_SIZE)
                    .Select(g => g.Select(x => x.id).ToList());
                
                var result = new List<tb_Inventory>();
                
                foreach (var batch in batches)
                {
                    var stocks = await _unitOfWorkManage.GetDbClient().Queryable<tb_Inventory>()
                        .Where(p => batch.Contains(p.ProdDetailID))
                        .ToListAsync();
                    
                    result.AddRange(stocks);
                }
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "从数据库批量加载库存失败");
                return new List<tb_Inventory>();
            }
        }
        
        private async Task<List<long>> GetProductIdsForPreheatAsync(int pageNumber, int pageSize)
        {
            try
            {
                // 获取需要预热的产品ID列表
                // 这里可以根据业务需求排序，优先预热重要产品
                return await _unitOfWorkManage.GetDbClient().Queryable<tb_Inventory>()
                    .OrderBy(p => p.ProdDetailID)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(p => p.ProdDetailID)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取预热产品ID失败");
                return new List<long>();
            }
        }
        
        private bool IsHighPriorityProduct(long productId)
        {
            // 根据业务规则判断产品优先级
            // 这里可以根据实际需求实现，例如：
            // 1. 热销产品
            // 2. 低库存产品
            // 3. 特定类别的产品
            return false; // 默认返回普通优先级
        }
        
        private async Task RefreshStockCacheInternalAsync(tb_Inventory stock)
        {
            if (stock == null)
                return;
                
            string cacheKey = $"{STOCK_CACHE_PREFIX}{stock.ProdDetailID}";
            
            // 根据业务重要性设置不同的过期时间
            TimeSpan expiration = IsHighPriorityProduct(stock.ProdDetailID) 
                ? TimeSpan.FromSeconds(HIGH_PRIORITY_CACHE_EXPIRATION_SECONDS) 
                : TimeSpan.FromSeconds(DEFAULT_CACHE_EXPIRATION_SECONDS);
            
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(expiration)
                .SetSlidingExpiration(TimeSpan.FromSeconds(10))
                .RegisterPostEvictionCallback(OnCacheEvicted)
                .SetPriority(IsHighPriorityProduct(stock.ProdDetailID) 
                    ? CacheItemPriority.High 
                    : CacheItemPriority.Normal);
            
            _cache.Set(cacheKey, stock, cacheEntryOptions);
            _cacheKeys.TryAdd(cacheKey, true);
            UpdateCacheSize();
        }
        
        private void OnCacheEvicted(object key, object value, EvictionReason reason, object state)
        {
            string cacheKey = key.ToString();
            _cacheKeys.TryRemove(cacheKey, out _);
            UpdateCacheSize();
            
            // 可以根据不同的移除原因进行不同的处理
            if (reason == EvictionReason.Capacity)
            {
                _logger.LogWarning("缓存由于容量限制被移除: {Key}", cacheKey);
            }
            else if (reason == EvictionReason.Expired)
            {
                _logger.LogDebug("缓存已过期: {Key}", cacheKey);
            }
        }
        
        #region 统计信息更新方法
        
        private void IncrementRequestCount(int count = 1)
        {
            _statisticsLock.EnterWriteLock();
            try
            {
                _statistics.TotalRequests += count;
            }
            finally
            {
                _statisticsLock.ExitWriteLock();
            }
        }
        
        private void IncrementCacheHit()
        {
            _statisticsLock.EnterWriteLock();
            try
            {
                _statistics.CacheHits++;
            }
            finally
            {
                _statisticsLock.ExitWriteLock();
            }
        }
        
        private void IncrementCacheMiss()
        {
            _statisticsLock.EnterWriteLock();
            try
            {
                _statistics.CacheMisses++;
            }
            finally
            {
                _statisticsLock.ExitWriteLock();
            }
        }
        
        private void UpdateCacheSize()
        {
            _statisticsLock.EnterWriteLock();
            try
            {
                _statistics.CurrentCacheSize = _cacheKeys.Count;
            }
            finally
            {
                _statisticsLock.ExitWriteLock();
            }
        }
        
        #endregion
        
        #endregion
    }
}
