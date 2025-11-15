using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RUINORERP.Common.Helper;
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
    /// 使用统一的ICacheManager<object>进行缓存管理，支持更灵活的缓存策略
    /// </summary>
    public class InventoryCacheManager
    {
        private readonly ILogger<InventoryCacheManager> _logger;
        private readonly ICacheManager<object> _cacheManager;

        // 缓存键前缀（避免键冲突）
        private const string SnapshotKeyPrefix = "InventorySnapshot:";
        private const string TransactionKeyPrefix = "InventoryTransaction:";

        // 缓存过期策略（历史数据可设置较长有效期）
        private readonly TimeSpan _snapshotExpiration = TimeSpan.FromHours(24);
        private readonly TimeSpan _transactionExpiration = TimeSpan.FromHours(12);

        /// <summary>
        /// 构造函数
        /// </summary>
        public InventoryCacheManager(ILogger<InventoryCacheManager> logger, ICacheManager<object> cacheManager)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
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
            var cachedData = _cacheManager.Get(cacheKey);
            if (cachedData != null)
            {
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

            // 使用缓存管理器更新缓存，设置过期时间
            _cacheManager.Put(cacheKey, snapshots);
            _cacheManager.Expire(cacheKey, ExpirationMode.Absolute, _snapshotExpiration);

            _logger.LogDebug($"设置库存快照缓存：{cacheKey}，记录数：{snapshots.Count}");
        }

        /// <summary>
        /// 批量删除相关快照缓存（如新增快照时）
        /// </summary>
        public void RemoveRelatedSnapshotCache(long? warehouseId = null, long? productId = null)
        {
            // 由于无法直接获取所有缓存键，这里采用更精确的方式
            // 删除指定条件的缓存键
            var cacheKey = GenerateSnapshotKey(warehouseId, productId, null);
            
            // 如果提供了具体的仓库或商品ID，直接删除对应的缓存
            if (warehouseId.HasValue || productId.HasValue)
            {
                _cacheManager.Remove(cacheKey);
                _logger.LogDebug($"删除库存快照缓存：{cacheKey}");
            }
            else
            {
                // 如果没有提供具体条件，记录警告信息
                _logger.LogWarning("未提供具体的仓库或商品ID，无法批量删除库存快照缓存");
            }
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

            var cachedData = _cacheManager.Get(cacheKey);
            if (cachedData != null)
            {
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

            // 使用缓存管理器更新缓存，设置过期时间
            _cacheManager.Put(cacheKey, transactions);
            _cacheManager.Expire(cacheKey, ExpirationMode.Absolute, _transactionExpiration);

            _logger.LogDebug($"设置库存流水缓存：{cacheKey}，记录数：{transactions.Count}");
        }

        /// <summary>
        /// 按商品ID清除流水缓存（如发生新交易时）
        /// </summary>
        public void InvalidateTransactionCache(long productId)
        {
            // 由于无法直接获取所有缓存键，这里采用更精确的方式
            // 删除指定商品的流水缓存
            var cacheKey = GenerateTransactionKey(DateTime.MinValue, DateTime.MaxValue, null, productId);
            
            // 删除指定商品的流水缓存
            _cacheManager.Remove(cacheKey);
            _logger.LogDebug($"失效商品 {productId} 的库存流水缓存");
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
            if (cachedData is List<tb_InventorySnapshot> snapshots)
                return snapshots;

            // 处理缓存格式转换
            if (cachedData is List<object> objectList)
            {
                return objectList
                    .Where(obj => obj is tb_InventorySnapshot)
                    .Cast<tb_InventorySnapshot>()
                    .ToList();
            }

            _logger.LogWarning($"库存快照缓存数据格式异常");
            return new List<tb_InventorySnapshot>();
        }

        /// <summary>
        /// 将缓存数据转换为流水列表
        /// </summary>
        private List<tb_InventoryTransaction> ConvertToTransactionList(object cachedData)
        {
            if (cachedData is List<tb_InventoryTransaction> transactions)
                return transactions;

            // 处理缓存格式转换
            if (cachedData is List<object> objectList)
            {
                return objectList
                    .Where(obj => obj is tb_InventoryTransaction)
                    .Cast<tb_InventoryTransaction>()
                    .ToList();
            }

            _logger.LogWarning($"库存流水缓存数据格式异常");
            return new List<tb_InventoryTransaction>();
        }

        #endregion
    }


}
