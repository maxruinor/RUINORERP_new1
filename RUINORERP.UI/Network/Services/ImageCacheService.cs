using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using RUINORERP.Model;
using RUINORERP.Repository.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network.Services
{
    /// <summary>
    /// 图片缓存服务 - 提供高频图片的内存缓存
    /// 策略：
    /// - 产品主图：永久缓存（24小时，更新时失效）
    /// - 订单凭证图：1小时缓存
    /// - 辅助图片：不缓存（延迟加载）
    /// </summary>
    public class ImageCacheService : IDisposable
    {
        private readonly IMemoryCache _cache;
        private readonly IUnitOfWorkManage _unitOfWorkManage;
        private readonly ILogger<ImageCacheService> _logger;
        private readonly SemaphoreSlim _cacheLock = new SemaphoreSlim(1, 1);
        private bool _disposed = false;

        /// <summary>
        /// 缓存键前缀常量
        /// </summary>
        private static class CacheKeys
        {
            public const string ProductMainImage = "product_main_image_";
            public const string SaleOrderVoucher = "saleorder_voucher_";
            public const string ExpenseClaimEvidence = "expenseclaim_evidence_";
            public const string PaymentRecordVoucher = "paymentrecord_voucher_";
        }

        /// <summary>
        /// 缓存过期时间配置
        /// </summary>
        private static class CacheExpirations
        {
            public static readonly TimeSpan ProductMainImage = TimeSpan.FromHours(24);
            public static readonly TimeSpan SaleOrderVoucher = TimeSpan.FromHours(1);
            public static readonly TimeSpan ExpenseClaimEvidence = TimeSpan.FromHours(1);
            public static readonly TimeSpan PaymentRecordVoucher = TimeSpan.FromHours(1);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cache">内存缓存实例</param>
        /// <param name="unitOfWorkManage">工作单元管理</param>
        /// <param name="logger">日志记录器</param>
        public ImageCacheService(
            IMemoryCache cache,
            IUnitOfWorkManage unitOfWorkManage,
            ILogger<ImageCacheService> logger)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _unitOfWorkManage = unitOfWorkManage ?? throw new ArgumentNullException(nameof(unitOfWorkManage));
            _logger = logger;
        }

        #region 产品主图缓存

        /// <summary>
        /// 获取产品主图（带缓存）
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <returns>产品主图路径</returns>
        public async Task<string> GetProductMainImageAsync(long productId)
        {
            if (productId <= 0)
                return null;

            string cacheKey = $"{CacheKeys.ProductMainImage}{productId}";

            return await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                _logger?.LogDebug("缓存未命中，从数据库查询产品主图: ProductId={ProductId}", productId);

                try
                {
                    var db = _unitOfWorkManage.GetDbClient().CopyNew();
                    var product = await db.Queryable<tb_Prod>()
                        .Where(p => p.Prod_ID == productId)
                        .Select(p => p.ImagesPath)
                        .FirstAsync();

                    // 设置缓存过期时间
                    entry.AbsoluteExpirationRelativeToNow = CacheExpirations.ProductMainImage;
                    entry.RegisterPostEvictionCallback((key, value, reason, state) =>
                    {
                        _logger?.LogDebug("产品主图缓存失效: Key={Key}, Reason={Reason}", key, reason);
                    });

                    return product;
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "查询产品主图失败: ProductId={ProductId}", productId);
                    return null;
                }
            });
        }

        /// <summary>
        /// 刷新产品主图缓存
        /// </summary>
        /// <param name="productId">产品ID</param>
        public void RefreshProductMainImageCache(long productId)
        {
            if (productId <= 0)
                return;

            string cacheKey = $"{CacheKeys.ProductMainImage}{productId}";
            _cache.Remove(cacheKey);
            _logger?.LogDebug("已移除产品主图缓存: ProductId={ProductId}", productId);
        }

        /// <summary>
        /// 批量获取产品主图（带缓存）
        /// </summary>
        /// <param name="productIds">产品ID列表</param>
        /// <returns>产品ID到主图路径的字典</returns>
        public async Task<Dictionary<long, string>> GetProductMainImagesBatchAsync(List<long> productIds)
        {
            if (productIds == null || productIds.Count == 0)
                return new Dictionary<long, string>();

            var result = new Dictionary<long, string>();
            var uncachedIds = new List<long>();

            // 首先从缓存中获取已缓存的图片
            foreach (var productId in productIds)
            {
                string cacheKey = $"{CacheKeys.ProductMainImage}{productId}";
                if (_cache.TryGetValue(cacheKey, out string imagePath))
                {
                    result[productId] = imagePath;
                }
                else
                {
                    uncachedIds.Add(productId);
                }
            }

            // 批量查询未缓存的产品
            if (uncachedIds.Count > 0)
            {
                try
                {
                    var db = _unitOfWorkManage.GetDbClient().CopyNew();
                    var products = await db.Queryable<tb_Prod>()
                        .Where(p => uncachedIds.Contains(p.Prod_ID))
                        .Select(p => new { p.Prod_ID, p.ImagesPath })
                        .ToListAsync();

                    foreach (var product in products)
                    {
                        result[product.Prod_ID] = product.ImagesPath;

                        // 缓存查询结果
                        string cacheKey = $"{CacheKeys.ProductMainImage}{product.Prod_ID}";
                        _cache.Set(cacheKey, product.ImagesPath, CacheExpirations.ProductMainImage);
                    }
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "批量查询产品主图失败");
                }
            }

            return result;
        }

        #endregion

        #region 订单凭证图缓存

        /// <summary>
        /// 获取销售订单凭证图（带缓存）
        /// </summary>
        /// <param name="orderId">订单ID</param>
        /// <returns>凭证图路径</returns>
        public async Task<string> GetSaleOrderVoucherImageAsync(long orderId)
        {
            if (orderId <= 0)
                return null;

            string cacheKey = $"{CacheKeys.SaleOrderVoucher}{orderId}";

            return await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                _logger?.LogDebug("缓存未命中，从数据库查询订单凭证图: OrderId={OrderId}", orderId);

                try
                {
                    var db = _unitOfWorkManage.GetDbClient().CopyNew();
                    var order = await db.Queryable<tb_SaleOrder>()
                        .Where(o => o.SOrder_ID == orderId)
                        .Select(o => o.VoucherImage)
                        .FirstAsync();

                    // 设置缓存过期时间
                    entry.AbsoluteExpirationRelativeToNow = CacheExpirations.SaleOrderVoucher;

                    return order;
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "查询订单凭证图失败: OrderId={OrderId}", orderId);
                    return null;
                }
            });
        }

        /// <summary>
        /// 刷新销售订单凭证图缓存
        /// </summary>
        /// <param name="orderId">订单ID</param>
        public void RefreshSaleOrderVoucherImageCache(long orderId)
        {
            if (orderId <= 0)
                return;

            string cacheKey = $"{CacheKeys.SaleOrderVoucher}{orderId}";
            _cache.Remove(cacheKey);
            _logger?.LogDebug("已移除订单凭证图缓存: OrderId={OrderId}", orderId);
        }

        #endregion

        #region 通用缓存操作

        /// <summary>
        /// 清空所有缓存
        /// </summary>
        public void ClearAllCache()
        {
            if (_cache is MemoryCache memoryCache)
            {
                memoryCache.Clear();
                _logger?.LogInformation("已清空所有图片缓存");
            }
        }

        /// <summary>
        /// 获取缓存统计信息
        /// </summary>
        /// <returns>缓存统计信息</returns>
        public CacheStatistics GetCacheStatistics()
        {
            if (!(_cache is MemoryCache memoryCache))
                return null;

            var stats = new CacheStatistics
            {
                TotalEntries = memoryCache.Count,
                Timestamp = DateTime.Now
            };

            _logger?.LogDebug("图片缓存统计: {Statistics}", stats);
            return stats;
        }

        #endregion

        #region IDisposable实现

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;
            _cacheLock?.Dispose();
            _logger?.LogInformation("图片缓存服务已释放");
        }

        #endregion
    }

    /// <summary>
    /// 缓存统计信息
    /// </summary>
    public class CacheStatistics
    {
        /// <summary>
        /// 缓存条目总数
        /// </summary>
        public long TotalEntries { get; set; }

        /// <summary>
        /// 统计时间戳
        /// </summary>
        public DateTime Timestamp { get; set; }

        public override string ToString()
        {
            return $"TotalEntries: {TotalEntries}, Timestamp: {Timestamp:yyyy-MM-dd HH:mm:ss}";
        }
    }
}
