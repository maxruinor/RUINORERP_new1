using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using CacheManager.Core;
using global::RUINORERP.Model;
using RUINORERP.Business.Cache.Attributes;
namespace RUINORERP.Business.Cache
{

    /// <summary>
    /// 库存缓存实现
    /// </summary>
    [CacheName("Inventory")]
    public class InventoryCache : CacheBase, IInventoryCache
    {
        // 缓存键前缀
        private const string SnapshotPrefix = "Snapshot";
        private const string TransactionPrefix = "Transaction";

        public InventoryCache(ICacheManager<object> cacheManager, ILogger<InventoryCache> logger, CachePolicy defaultPolicy)
            : base(cacheManager, logger, defaultPolicy)
        {
        }

        public override string CacheName => "Inventory";

        /// <summary>
        /// 生成库存快照缓存键
        /// </summary>
        private string GenerateSnapshotKey(long? warehouseId, long? productId, DateTime? snapshotDate)
        {
            var parts = new List<string> { SnapshotPrefix };

            if (warehouseId.HasValue)
                parts.Add($"W:{warehouseId}");

            if (productId.HasValue)
                parts.Add($"P:{productId}");

            if (snapshotDate.HasValue)
                parts.Add($"D:{snapshotDate.Value:yyyyMMdd}");

            return string.Join(":", parts);
        }

        /// <summary>
        /// 生成库存流水缓存键
        /// </summary>
        private string GenerateTransactionKey(DateTime startTime, DateTime endTime, long? warehouseId, long? productId)
        {
            var parts = new List<string>
            {
                TransactionPrefix,
                $"S:{startTime:yyyyMMdd}",
                $"E:{endTime:yyyyMMdd}"
            };

            if (warehouseId.HasValue)
                parts.Add($"W:{warehouseId}");

            if (productId.HasValue)
                parts.Add($"P:{productId}");

            return string.Join(":", parts);
        }

        public List<tb_InventorySnapshot> GetSnapshots(long? warehouseId = null, long? productId = null, DateTime? snapshotDate = null)
        {
            var key = GenerateSnapshotKey(warehouseId, productId, snapshotDate);
            return Get<List<tb_InventorySnapshot>>(key) ?? new List<tb_InventorySnapshot>();
        }

        public async Task<List<tb_InventorySnapshot>> GetSnapshotsAsync(long? warehouseId = null, long? productId = null, DateTime? snapshotDate = null)
        {
            var key = GenerateSnapshotKey(warehouseId, productId, snapshotDate);
            var result = await GetAsync<List<tb_InventorySnapshot>>(key);
            return result ?? new List<tb_InventorySnapshot>();
        }

        public void SetSnapshots(List<tb_InventorySnapshot> snapshots, long? warehouseId = null, long? productId = null, DateTime? snapshotDate = null, TimeSpan? expiration = null)
        {
            if (snapshots == null || !snapshots.Any())
                return;

            var key = GenerateSnapshotKey(warehouseId, productId, snapshotDate);
            Set(key, snapshots, expiration);
        }

        public async Task SetSnapshotsAsync(List<tb_InventorySnapshot> snapshots, long? warehouseId = null, long? productId = null, DateTime? snapshotDate = null, TimeSpan? expiration = null)
        {
            if (snapshots == null || !snapshots.Any())
                return;

            var key = GenerateSnapshotKey(warehouseId, productId, snapshotDate);
            await SetAsync(key, snapshots, expiration);
        }

        public List<tb_InventoryTransaction> GetTransactions(DateTime startTime, DateTime endTime, long? warehouseId = null, long? productId = null)
        {
            var key = GenerateTransactionKey(startTime, endTime, warehouseId, productId);
            return Get<List<tb_InventoryTransaction>>(key) ?? new List<tb_InventoryTransaction>();
        }

        public async Task<List<tb_InventoryTransaction>> GetTransactionsAsync(DateTime startTime, DateTime endTime, long? warehouseId = null, long? productId = null)
        {
            var key = GenerateTransactionKey(startTime, endTime, warehouseId, productId);
            var result = await GetAsync<List<tb_InventoryTransaction>>(key);
            return result ?? new List<tb_InventoryTransaction>();
        }

        public void SetTransactions(List<tb_InventoryTransaction> transactions, DateTime startTime, DateTime endTime, long? warehouseId = null, long? productId = null, TimeSpan? expiration = null)
        {
            if (transactions == null || !transactions.Any())
                return;

            var key = GenerateTransactionKey(startTime, endTime, warehouseId, productId);
            Set(key, transactions, expiration);
        }

        public async Task SetTransactionsAsync(List<tb_InventoryTransaction> transactions, DateTime startTime, DateTime endTime, long? warehouseId = null, long? productId = null, TimeSpan? expiration = null)
        {
            if (transactions == null || !transactions.Any())
                return;

            var key = GenerateTransactionKey(startTime, endTime, warehouseId, productId);
            await SetAsync(key, transactions, expiration);
        }

        public void RemoveProductInventoryCache(long productId)
        {
            // 移除所有包含该产品ID的缓存项
            RemoveByPattern($"*{SnapshotPrefix}:*P:{productId}*");
            RemoveByPattern($"*{TransactionPrefix}:*P:{productId}*");
        }

        public async Task RemoveProductInventoryCacheAsync(long productId)
        {
            // 异步版本简化实现，实际项目可能需要更高效的方式
            RemoveProductInventoryCache(productId);
            await Task.CompletedTask;
        }
    }


}
