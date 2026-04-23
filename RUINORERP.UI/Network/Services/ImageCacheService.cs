using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using RUINORERP.Model;
using RUINORERP.Model.BusinessImage;
using RUINORERP.Repository.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network.Services
{
    /// <summary>
    /// 图片缓存服务 - 客户端实现1
    /// 策略:
    /// - 产品主图:永久缓存(默认24小时,更新时失效)
    /// - 订单凭证图:短期缓存(默认1小时)
    /// - 费用报销单凭证图:短期缓存(默认1小时)
    /// - 辅助图片:不缓存(延迟加载)
    /// 支持配置化,可通过appsettings.json调整缓存策略
    /// 继承自ImageCacheServiceBase,提供通用的FileId缓存能力 + 业务场景扩展
    /// </summary>
    public class ImageCacheService : ImageCacheServiceBase
    {
        private readonly IUnitOfWorkManage _unitOfWorkManage;
        private readonly ILogger<ImageCacheService> _logger;
        private readonly ImageCacheConfiguration _config;
    
        /// <summary>
        /// 缓存键前缀常量(业务场景专用)
        /// </summary>
        private static class CacheKeys
        {
            public const string ProductMainImage = "product_main_image_";
            public const string SaleOrderVoucher = "saleorder_voucher_";
            public const string ExpenseClaimEvidence = "expenseclaim_evidence_";
            public const string PaymentRecordVoucher = "paymentrecord_voucher_";
        }
    
        /// <summary>
        /// 缓存过期时间配置(从配置读取)
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
        /// <param name="config">缓存配置(可选,如未提供则使用默认值)</param>
        public ImageCacheService(
            IMemoryCache cache,
            IUnitOfWorkManage unitOfWorkManage,
            ILogger<ImageCacheService> logger,
            ImageCacheConfiguration config = null)
            : base(cache) // 调用基类构造函数,使用默认的4小时滑动过期和8小时绝对过期
        {
            _unitOfWorkManage = unitOfWorkManage ?? throw new ArgumentNullException(nameof(unitOfWorkManage));
            _logger = logger;
            _config = config ?? new ImageCacheConfiguration(); // 使用默认配置
        
            _logger?.LogInformation("图片缓存服务初始化完成,是否启用:{IsEnabled},产品图过期:{ProductExpiration}小时,订单图过期:{OrderExpiration}小时",
                IsCacheEnabled,
                _config?.ProductMainImageExpiration.TotalHours ?? 24,
                _config?.SaleOrderVoucherExpiration.TotalHours ?? 1);
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

            return await _memoryCache.GetOrCreateAsync(cacheKey, async entry =>
            {
                _logger?.LogDebug("缓存未命中，从数据库查询产品主图: ProductId={ProductId}", productId);

                var imagePath = await QueryProductMainImageFromDbAsync(productId);

                // 设置缓存过期时间（从配置读取）
                entry.AbsoluteExpirationRelativeToNow = ProductMainImageExpiration;
                entry.SetSize(1); // 修复：指定缓存项大小（必需，因为可能使用了带SizeLimit的IMemoryCache）
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
                    .Where(p => p.ProdBaseID == productId)
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
            _memoryCache.Remove(cacheKey);
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
                if (_memoryCache.TryGetValue(cacheKey, out string imagePath))
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
                        .Where(p => uncachedIds.Contains(p.ProdBaseID))
                        .Select(p => new { p.ProdBaseID, p.ImagesPath })
                        .ToListAsync();

                    foreach (var product in products)
                    {
                        result[product.ProdBaseID] = product.ImagesPath;

                        // 缓存查询结果(如果启用缓存)
                        string cacheKey = $"{CacheKeys.ProductMainImage}{product.ProdBaseID}";
                        if (IsCacheEnabled)
                        {
                            _memoryCache.Set(cacheKey, product.ImagesPath, ProductMainImageExpiration);
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

            return await _memoryCache.GetOrCreateAsync(cacheKey, async entry =>
            {
                _logger?.LogDebug("缓存未命中，从数据库查询订单凭证图: OrderId={OrderId}", orderId);

                var voucherPath = await QuerySaleOrderVoucherFromDbAsync(orderId);

                // 设置缓存过期时间（从配置读取）
                entry.AbsoluteExpirationRelativeToNow = SaleOrderVoucherExpiration;
                entry.SetSize(1); // 修复：指定缓存项大小（必需，因为可能使用了带SizeLimit的IMemoryCache）

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
            _memoryCache.Remove(cacheKey);
            _logger?.LogDebug("已移除订单凭证图缓存: OrderId={OrderId}", orderId);
        }

        #endregion

        #region 通用缓存操作

        /// <summary>
        /// ✅ 新增: 获取SKU图片列表(带缓存)
        /// 用于frmProductEdit中显示SKU图片缩略图
        /// </summary>
        public async Task<List<ImageInfo>> GetSKUImagesByProdDetailIdAsync(long prodDetailId)
        {
            if (prodDetailId <= 0)
                return new List<ImageInfo>();

            // 1. 先检查缓存中是否已有该SKU的所有图片
            var cachedImages = new List<ImageInfo>();
            
            // 2. 查询业务关联获取FileId列表
            var db = _unitOfWorkManage.GetDbClient().CopyNew();
            var relations = await db.Queryable<tb_FS_BusinessRelation>()
                .Where(br => br.OwnerTableName == "tb_ProdDetail" 
                          && br.BusinessId == prodDetailId 
                          && br.IsActive)
                .ToListAsync();

            if (relations == null || relations.Count == 0)
                return cachedImages;

            // 3. 批量从缓存获取或数据库查询
            var fileIds = relations.Select(r => r.FileId).ToList();
            var imageInfos = await GetImageInfosBatchAsync(fileIds);

            foreach (var fileId in fileIds)
            {
                if (imageInfos.TryGetValue(fileId, out var info))
                {
                    cachedImages.Add(info);
                }
            }

            return cachedImages;
        }

        /// <summary>
        /// ✅ 新增: 根据FileId获取图片信息(带缓存)
        /// 用于查询列表时的缩略图显示
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <returns>图片信息(包含ImageData)</returns>
        public async Task<ImageInfo> GetImageInfoByFileIdAsync(long fileId)
        {
            if (fileId <= 0)
                return null;

            // ✅ 先检查基类的FileId缓存
            var cachedStorageInfo = GetImageInfo(fileId);
            if (cachedStorageInfo != null && cachedStorageInfo is tb_FS_FileStorageInfo storageInfo && storageInfo.FileData != null)
            {
                _logger?.LogDebug("✅ 缓存命中: FileId={FileId}", fileId);
                return ConvertToImageInfo(storageInfo);
            }

            _logger?.LogDebug("❌ 缓存未命中,从数据库查询: FileId={FileId}", fileId);

            // 缓存未命中,从数据库查询
            try
            {
                var db = _unitOfWorkManage.GetDbClient().CopyNew();
                var fileStorageInfo = await db.Queryable<tb_FS_FileStorageInfo>()
                    .Where(f => f.FileId == fileId)
                    .FirstAsync();

                if (fileStorageInfo != null)
                {
                    // ✅ 添加到基类缓存
                    AddImageInfo(fileStorageInfo);
                    _logger?.LogDebug("📦 从数据库加载并缓存: FileId={FileId}, Size={Size} bytes", fileId, fileStorageInfo.FileData?.Length ?? 0);

                    return ConvertToImageInfo(fileStorageInfo);
                }
                else
                {
                    _logger?.LogWarning("⚠️ 数据库中未找到图片: FileId={FileId}", fileId);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "❌ 查询图片信息失败: FileId={FileId}", fileId);
            }

            return null;
        }

        /// <summary>
        /// ✅ 新增: 批量获取图片信息(优化版)
        /// 一次性查询多个FileId,减少数据库往返
        /// </summary>
        /// <param name="fileIds">文件ID列表</param>
        /// <returns>FileId到ImageInfo的字典</returns>
        public async Task<Dictionary<long, ImageInfo>> GetImageInfosBatchAsync(List<long> fileIds)
        {
            if (fileIds == null || fileIds.Count == 0)
                return new Dictionary<long, ImageInfo>();

            var result = new Dictionary<long, ImageInfo>();
            var uncachedIds = new List<long>();

            // 1. 先从缓存中获取
            foreach (var fileId in fileIds.Where(id => id > 0))
            {
                var cachedStorageInfo = GetImageInfo(fileId);
                if (cachedStorageInfo != null && cachedStorageInfo is tb_FS_FileStorageInfo storageInfo && storageInfo.FileData != null)
                {
                    result[fileId] = ConvertToImageInfo(cachedStorageInfo as tb_FS_FileStorageInfo);
                }
                else
                {
                    uncachedIds.Add(fileId);
                }
            }

            // ✅ 记录缓存命中情况
            int cacheHitCount = fileIds.Count - uncachedIds.Count;
            _logger?.LogInformation("📊 批量获取图片: 总请求{Total}, 缓存命中{Hit}, 需DB查询{Miss}, 命中率{Rate}%",
                fileIds.Count, cacheHitCount, uncachedIds.Count, 
                fileIds.Count > 0 ? (cacheHitCount * 100 / fileIds.Count) : 0);

            // 2. 批量查询未缓存的
            if (uncachedIds.Count > 0)
            {
                try
                {
                    _logger?.LogDebug("🔍 开始从数据库查询{Count}个未缓存的图片", uncachedIds.Count);
                    var db = _unitOfWorkManage.GetDbClient().CopyNew();
                    var fileStorageInfos = await db.Queryable<tb_FS_FileStorageInfo>()
                        .Where(f => uncachedIds.Contains(f.FileId))
                        .ToListAsync();

                    _logger?.LogDebug("✅ 数据库返回{Count}条记录", fileStorageInfos?.Count ?? 0);

                    foreach (var fsi in fileStorageInfos)
                    {
                        // 添加到缓存
                        AddImageInfo(fsi);
                        
                        result[fsi.FileId] = ConvertToImageInfo(fsi);
                    }

                    _logger?.LogDebug("📦 已将{Count}个图片加入缓存", fileStorageInfos?.Count ?? 0);
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "❌ 批量查询图片信息失败");
                }
            }

            return result;
        }

        /// <summary>
        /// ✅ 辅助方法: 将tb_FS_FileStorageInfo转换为ImageInfo
        /// </summary>
        private ImageInfo ConvertToImageInfo(tb_FS_FileStorageInfo storageInfo)
        {
            if (storageInfo == null)
                return null;

            return new ImageInfo
            {
                FileId = storageInfo.FileId,
                OriginalFileName = storageInfo.OriginalFileName,
                ImageData = storageInfo.FileData,
                FileSize = storageInfo.FileSize,
                FileType = storageInfo.FileType,
                FileExtension = storageInfo.FileExtension,
                HashValue = storageInfo.HashValue,
                CreateTime = storageInfo.Created_at ?? DateTime.Now,
                ModifiedAt = storageInfo.Modified_at ?? DateTime.Now
            };
        }

        /// <summary>
        /// 清空所有缓存
        /// </summary>
        public void ClearAllCache()
        {
            if (_memoryCache is MemoryCache memoryCache)
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
            if (!(_memoryCache is MemoryCache memoryCache))
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
        /// 产品主图绝对过期时间（默认7天）
        /// </summary>
        public TimeSpan ProductMainImageAbsoluteExpiration { get; set; } = TimeSpan.FromDays(7);

        /// <summary>
        /// SKU图片缓存过期时间（默认12小时）
        /// </summary>
        public TimeSpan SkuImageExpiration { get; set; } = TimeSpan.FromHours(12);
        
        /// <summary>
        /// SKU图片绝对过期时间（默认3天）
        /// </summary>
        public TimeSpan SkuImageAbsoluteExpiration { get; set; } = TimeSpan.FromDays(3);

        /// <summary>
        /// 销售订单凭证图缓存过期时间（默认1小时）
        /// </summary>
        public TimeSpan SaleOrderVoucherExpiration { get; set; } = TimeSpan.FromHours(1);
        
        /// <summary>
        /// 销售订单凭证图绝对过期时间（默认24小时）
        /// </summary>
        public TimeSpan SaleOrderVoucherAbsoluteExpiration { get; set; } = TimeSpan.FromHours(24);

        /// <summary>
        /// 费用报销单凭证图缓存过期时间（默认1小时）
        /// </summary>
        public TimeSpan ExpenseClaimEvidenceExpiration { get; set; } = TimeSpan.FromHours(1);
        
        /// <summary>
        /// 费用报销单凭证图绝对过期时间（默认24小时）
        /// </summary>
        public TimeSpan ExpenseClaimEvidenceAbsoluteExpiration { get; set; } = TimeSpan.FromHours(24);

        /// <summary>
        /// 付款记录凭证图缓存过期时间（默认1小时）
        /// </summary>
        public TimeSpan PaymentRecordVoucherExpiration { get; set; } = TimeSpan.FromHours(1);
        
        /// <summary>
        /// 付款记录凭证图绝对过期时间（默认24小时）
        /// </summary>
        public TimeSpan PaymentRecordVoucherAbsoluteExpiration { get; set; } = TimeSpan.FromHours(24);

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
