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
    /// - 产品主图：永久缓存（默认24小时，更新时失效）
    /// - 订单凭证图：短期缓存（默认1小时）
    /// - 费用报销单凭证图：短期缓存（默认1小时）
    /// - 辅助图片：不缓存（延迟加载）
    /// 支持配置化，可通过appsettings.json调整缓存策略
    /// </summary>
    public class ImageCacheService : IDisposable
    {
        private readonly IMemoryCache _cache;
        private readonly IUnitOfWorkManage _unitOfWorkManage;
        private readonly ILogger<ImageCacheService> _logger;
        private readonly SemaphoreSlim _cacheLock = new SemaphoreSlim(1, 1);
        private readonly ImageCacheConfiguration _config;
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
        /// 缓存过期时间配置（从配置读取）
        /// </summary>
        private TimeSpan ProductMainImageExpiration => _config?.ProductMainImageExpiration ?? TimeSpan.FromHours(24);
        private TimeSpan SaleOrderVoucherExpiration => _config?.SaleOrderVoucherExpiration ?? TimeSpan.FromHours(1);
        private TimeSpan ExpenseClaimEvidenceExpiration => _config?.ExpenseClaimEvidenceExpiration ?? TimeSpan.FromHours(1);
        private TimeSpan PaymentRecordVoucherExpiration => _config?.PaymentRecordVoucherExpiration ?? TimeSpan.FromHours(1);

        /// <summary>
        /// 是否启用缓存
        /// </summary>
        private bool IsCacheEnabled => _config?.IsEnabled ?? true;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cache">内存缓存实例</param>
        /// <param name="unitOfWorkManage">工作单元管理</param>
        /// <param name="logger">日志记录器</param>
        /// <param name="config">缓存配置（可选，如未提供则使用默认值）</param>
        public ImageCacheService(
            IMemoryCache cache,
            IUnitOfWorkManage unitOfWorkManage,
            ILogger<ImageCacheService> logger,
            ImageCacheConfiguration config = null)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _unitOfWorkManage = unitOfWorkManage ?? throw new ArgumentNullException(nameof(unitOfWorkManage));
            _logger = logger;
            _config = config ?? new ImageCacheConfiguration(); // 使用默认配置

            _logger?.LogInformation("图片缓存服务初始化完成,是否启用:{IsEnabled},产品图过期:{ProductExpiration}小时,订单图过期:{OrderExpiration}小时",
                IsCacheEnabled,
                _config?.ProductMainImageExpiration?.TotalHours ?? 24,
                _config?.SaleOrderVoucherExpiration?.TotalHours ?? 1);
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

            // 如果缓存未启用，直接查询数据库
            if (!IsCacheEnabled)
            {
                return await QueryProductMainImageFromDbAsync(productId);
            }

            string cacheKey = $"{CacheKeys.ProductMainImage}{productId}";

            return await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                _logger?.LogDebug("缓存未命中，从数据库查询产品主图: ProductId={ProductId}", productId);

                var imagePath = await QueryProductMainImageFromDbAsync(productId);

                // 设置缓存过期时间（从配置读取）
                entry.AbsoluteExpirationRelativeToNow = ProductMainImageExpiration;
                entry.RegisterPostEvictionCallback((key, value, reason, state) =>
                {
                    _logger?.LogDebug("产品主图缓存失效: Key={Key}, Reason={Reason}", key, reason);
                });

                return imagePath;
            });
        }

        /// <summary>
        /// 从数据库查询产品主图
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <returns>产品主图路径</returns>
        private async Task<string> QueryProductMainImageFromDbAsync(long productId)
        {
            try
            {
                var db = _unitOfWorkManage.GetDbClient().CopyNew();
                var product = await db.Queryable<tb_Prod>()
                    .Where(p => p.Prod_ID == productId)
                    .Select(p => p.ImagesPath)
                    .FirstAsync();

                return product;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "查询产品主图失败: ProductId={ProductId}", productId);
                return null;
            }
        }

        /// <summary>
        /// 刷新产品主图缓存
        /// </summary>
        /// <param name="productId">产品ID</param>
        public void RefreshProductMainImageCache(long productId)
        {
            if (productId <= 0)
                return;

            if (!IsCacheEnabled)
            {
                _logger?.LogDebug("缓存未启用，跳过刷新产品主图缓存");
                return;
            }

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

                        // 缓存查询结果（如果启用缓存）
                        string cacheKey = $"{CacheKeys.ProductMainImage}{product.Prod_ID}";
                        if (IsCacheEnabled)
                        {
                            _cache.Set(cacheKey, product.ImagesPath, ProductMainImageExpiration);
                        }
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

            // 如果缓存未启用，直接查询数据库
            if (!IsCacheEnabled)
            {
                return await QuerySaleOrderVoucherFromDbAsync(orderId);
            }

            string cacheKey = $"{CacheKeys.SaleOrderVoucher}{orderId}";

            return await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                _logger?.LogDebug("缓存未命中，从数据库查询订单凭证图: OrderId={OrderId}", orderId);

                var voucherPath = await QuerySaleOrderVoucherFromDbAsync(orderId);

                // 设置缓存过期时间（从配置读取）
                entry.AbsoluteExpirationRelativeToNow = SaleOrderVoucherExpiration;

                return voucherPath;
            });
        }

        /// <summary>
        /// 从数据库查询销售订单凭证图
        /// </summary>
        /// <param name="orderId">订单ID</param>
        /// <returns>凭证图路径</returns>
        private async Task<string> QuerySaleOrderVoucherFromDbAsync(long orderId)
        {
            try
            {
                var db = _unitOfWorkManage.GetDbClient().CopyNew();
                var order = await db.Queryable<tb_SaleOrder>()
                    .Where(o => o.SOrder_ID == orderId)
                    .Select(o => o.VoucherImage)
                    .FirstAsync();

                return order;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "查询订单凭证图失败: OrderId={OrderId}", orderId);
                return null;
            }
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

    /// <summary>
    /// 图片缓存配置类
    /// 可通过appsettings.json配置缓存策略
    /// </summary>
    public class ImageCacheConfiguration
    {
        /// <summary>
        /// 是否启用缓存（默认true）
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// 产品主图缓存过期时间（默认24小时）
        /// </summary>
        public TimeSpan ProductMainImageExpiration { get; set; } = TimeSpan.FromHours(24);

        /// <summary>
        /// 销售订单凭证图缓存过期时间（默认1小时）
        /// </summary>
        public TimeSpan SaleOrderVoucherExpiration { get; set; } = TimeSpan.FromHours(1);

        /// <summary>
        /// 费用报销单凭证图缓存过期时间（默认1小时）
        /// </summary>
        public TimeSpan ExpenseClaimEvidenceExpiration { get; set; } = TimeSpan.FromHours(1);

        /// <summary>
        /// 付款记录凭证图缓存过期时间（默认1小时）
        /// </summary>
        public TimeSpan PaymentRecordVoucherExpiration { get; set; } = TimeSpan.FromHours(1);

        /// <summary>
        /// 从配置字典创建配置对象
        /// </summary>
        /// <param name="configDict">配置字典</param>
        /// <returns>配置对象</returns>
        public static ImageCacheConfiguration FromDictionary(Dictionary<string, string> configDict)
        {
            var config = new ImageCacheConfiguration();

            if (configDict == null)
                return config;

            if (configDict.TryGetValue("IsEnabled", out string enabledStr) && bool.TryParse(enabledStr, out bool enabled))
            {
                config.IsEnabled = enabled;
            }

            if (configDict.TryGetValue("ProductMainImageExpirationHours", out string productExpStr) && int.TryParse(productExpStr, out int productHours))
            {
                config.ProductMainImageExpiration = TimeSpan.FromHours(productHours);
            }

            if (configDict.TryGetValue("SaleOrderVoucherExpirationHours", out string orderExpStr) && int.TryParse(orderExpStr, out int orderHours))
            {
                config.SaleOrderVoucherExpiration = TimeSpan.FromHours(orderHours);
            }

            if (configDict.TryGetValue("ExpenseClaimEvidenceExpirationHours", out string expenseExpStr) && int.TryParse(expenseExpStr, out int expenseHours))
            {
                config.ExpenseClaimEvidenceExpiration = TimeSpan.FromHours(expenseHours);
            }

            return config;
        }

        /// <summary>
        /// 转换为配置字典
        /// </summary>
        /// <returns>配置字典</returns>
        public Dictionary<string, string> ToDictionary()
        {
            return new Dictionary<string, string>
            {
                { "IsEnabled", IsEnabled.ToString() },
                { "ProductMainImageExpirationHours", ((int)ProductMainImageExpiration.TotalHours).ToString() },
                { "SaleOrderVoucherExpirationHours", ((int)SaleOrderVoucherExpiration.TotalHours).ToString() },
                { "ExpenseClaimEvidenceExpirationHours", ((int)ExpenseClaimEvidenceExpiration.TotalHours).ToString() }
            };
        }
    }
}
