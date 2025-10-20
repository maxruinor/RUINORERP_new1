using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RUINORERP.Common.Helper;
using RUINORERP.Extensions.Middlewares;
using Microsoft.Extensions.Logging;
using CacheManager.Core;
using Newtonsoft.Json.Linq;
using RUINORERP.Model;

namespace RUINORERP.Business.Cache
{
    /// <summary>
    /// 库存缓存管理器
    /// 专门处理库存流水和库存快照的缓存逻辑
    /// 历史数据特性：查询频繁、更新少、多维度查询（时间/仓库/商品）
    /// </summary>
    public class InventoryCacheManager
    {
        // 基础缓存管理器（复用现有缓存能力）
        private readonly MyCacheManager _baseCache;
        private readonly ILogger<InventoryCacheManager> _logger;

        // 缓存键前缀（避免键冲突）
        private const string SnapshotKeyPrefix = "InventorySnapshot:";
        private const string TransactionKeyPrefix = "InventoryTransaction:";

        // 缓存过期策略（历史数据可设置较长有效期）
        private readonly TimeSpan _snapshotExpiration = TimeSpan.FromHours(24);
        private readonly TimeSpan _transactionExpiration = TimeSpan.FromHours(12);

        /// <summary>
        /// 构造函数
        /// </summary>
        public InventoryCacheManager(MyCacheManager baseCache, ILogger<InventoryCacheManager> logger)
        {
            _baseCache = baseCache ?? throw new ArgumentNullException(nameof(baseCache));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #region 库存快照缓存操作

        /// <summary>
        /// 获取指定条件的库存快照缓存
        /// </summary>
        /// <param name="warehouseId">仓库ID（可选）</param>
        /// <param name="productId">商品ID（可选）</param>
        /// <param name="snapshotDate">快照日期（可选）</param>
        public List<tb_InventorySnapshot> GetSnapshotCache(long? warehouseId = null, long? productId = null, DateTime? snapshotDate = null)
        {
            // 生成多维度缓存键（支持部分条件查询）
            var cacheKey = GenerateSnapshotKey(warehouseId, productId, snapshotDate);

            // 尝试从缓存获取
            if (_baseCache.CacheEntityList.Exists(cacheKey))
            {
                var cachedData = _baseCache.CacheEntityList.Get(cacheKey);
                _logger.LogDebug($"库存快照缓存命中：{cacheKey}");
                return ConvertToSnapshotList(cachedData);
            }

            return null;
        }

        /// <summary>
        /// 设置库存快照缓存
        /// </summary>
        public void SetSnapshotCache(List<tb_InventorySnapshot> snapshots, long? warehouseId = null, long? productId = null, DateTime? snapshotDate = null)
        {
            if (snapshots == null || !snapshots.Any())
                return;

            var cacheKey = GenerateSnapshotKey(warehouseId, productId, snapshotDate);

            // 转换为可缓存格式并设置过期时间
            var cacheData = snapshots.Cast<object>().ToList();
            _baseCache.CacheEntityList.Add(cacheKey, cacheData);
            _baseCache.CacheEntityList.Expire(cacheKey, ExpirationMode.Absolute, _snapshotExpiration);

            _logger.Debug($"设置库存快照缓存：{cacheKey}，记录数：{snapshots.Count}");
        }

        /// <summary>
        /// 批量删除相关快照缓存（如新增快照时）
        /// </summary>
        public void RemoveRelatedSnapshotCache(long productId)
        {
            // 模糊匹配删除相关缓存（如同一商品的所有快照）
            //var keysToRemove = _baseCache.CacheEntityList.Keys()
            //    .Where(k => k.StartsWith(SnapshotKeyPrefix) && k.Contains($"Product:{productId}"))
            //    .ToList();

            //foreach (var key in keysToRemove)
            //{
            //    _baseCache.CacheEntityList.Remove(key);
            //    _logger.LogDebug($"删除相关库存快照缓存：{key}");
            //}
        }

        #endregion

        #region 库存流水缓存操作

        /// <summary>
        /// 获取指定时间范围的库存流水缓存
        /// </summary>
        public List<tb_InventoryTransaction> GetTransactionCache(DateTime startTime, DateTime endTime,
            long? warehouseId = null, long? productId = null)
        {
            var cacheKey = GenerateTransactionKey(startTime, endTime, warehouseId, productId);

            if (_baseCache.CacheEntityList.Exists(cacheKey))
            {
                var cachedData = _baseCache.CacheEntityList.Get(cacheKey);
                _logger.LogDebug($"库存流水缓存命中：{cacheKey}");
                return ConvertToTransactionList(cachedData);
            }

            return null;
        }

        /// <summary>
        /// 设置库存流水缓存
        /// </summary>
        public void SetTransactionCache(List<tb_InventoryTransaction> transactions, DateTime startTime, DateTime endTime,
            long? warehouseId = null, long? productId = null)
        {
            if (transactions == null || !transactions.Any())
                return;

            var cacheKey = GenerateTransactionKey(startTime, endTime, warehouseId, productId);
            var cacheData = transactions.Cast<object>().ToList();

            _baseCache.CacheEntityList.Add(cacheKey, cacheData);
            _baseCache.CacheEntityList.Expire(cacheKey, ExpirationMode.Absolute, _transactionExpiration);

            _logger.Debug($"设置库存流水缓存：{cacheKey}，记录数：{transactions.Count}");
        }

        /// <summary>
        /// 按商品ID清除流水缓存（如发生新交易时）
        /// </summary>
        public void InvalidateTransactionCache(long productId)
        {
            //var keysToRemove = _baseCache.CacheEntityList.GetKeys()
            //    .Where(k => k.StartsWith(TransactionKeyPrefix) && k.Contains($"Product:{productId}"))
            //    .ToList();

            //foreach (var key in keysToRemove)
            //{
            //    _baseCache.CacheEntityList.Remove(key);
            //    _logger.LogDebug($"失效库存流水缓存：{key}");
            //}
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 生成库存快照缓存键（支持多维度组合）
        /// 格式：InventorySnapshot:Warehouse:1:Product:2:Date:20240813
        /// </summary>
        private string GenerateSnapshotKey(long? warehouseId, long? productId, DateTime? snapshotDate)
        {
            var keyParts = new List<string> { SnapshotKeyPrefix };

            if (warehouseId.HasValue)
                keyParts.Add($"Warehouse:{warehouseId}");

            if (productId.HasValue)
                keyParts.Add($"Product:{productId}");

            if (snapshotDate.HasValue)
                keyParts.Add($"Date:{snapshotDate.Value:yyyyMMdd}");

            return string.Join(":", keyParts);
        }

        /// <summary>
        /// 生成库存流水缓存键（按时间范围+维度）
        /// 格式：InventoryTransaction:Start:20240801:End:20240813:Warehouse:1
        /// </summary>
        private string GenerateTransactionKey(DateTime startTime, DateTime endTime, long? warehouseId, long? productId)
        {
            var keyParts = new List<string>
            {
                TransactionKeyPrefix,
                $"Start:{startTime:yyyyMMdd}",
                $"End:{endTime:yyyyMMdd}"
            };

            if (warehouseId.HasValue)
                keyParts.Add($"Warehouse:{warehouseId}");

            if (productId.HasValue)
                keyParts.Add($"Product:{productId}");

            return string.Join(":", keyParts);
        }

        /// <summary>
        /// 将缓存数据转换为快照列表
        /// </summary>
        private List<tb_InventorySnapshot> ConvertToSnapshotList(object cachedData)
        {
            return cachedData switch
            {
                List<object> objList => objList.OfType<tb_InventorySnapshot>().ToList(),
                JArray jArray => jArray.ToObject<List<tb_InventorySnapshot>>(),
                _ => new List<tb_InventorySnapshot>()
            };
        }

        /// <summary>
        /// 将缓存数据转换为流水列表
        /// </summary>
        private List<tb_InventoryTransaction> ConvertToTransactionList(object cachedData)
        {
            return cachedData switch
            {
                List<object> objList => objList.OfType<tb_InventoryTransaction>().ToList(),
                JArray jArray => jArray.ToObject<List<tb_InventoryTransaction>>(),
                _ => new List<tb_InventoryTransaction>()
            };
        }

        #endregion
    }


}
