using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using global::RUINORERP.Model;




namespace RUINORERP.Business.Cache
{



        /// <summary>
        /// 库存缓存接口
        /// 专门处理库存相关数据的缓存
        /// </summary>
        public interface IInventoryCache : ICache
        {
            /// <summary>
            /// 获取库存快照
            /// </summary>
            List<tb_InventorySnapshot> GetSnapshots(long? warehouseId = null, long? productId = null, DateTime? snapshotDate = null);

            /// <summary>
            /// 异步获取库存快照
            /// </summary>
            Task<List<tb_InventorySnapshot>> GetSnapshotsAsync(long? warehouseId = null, long? productId = null, DateTime? snapshotDate = null);

            /// <summary>
            /// 设置库存快照缓存
            /// </summary>
            void SetSnapshots(List<tb_InventorySnapshot> snapshots, long? warehouseId = null, long? productId = null, DateTime? snapshotDate = null, TimeSpan? expiration = null);

            /// <summary>
            /// 异步设置库存快照缓存
            /// </summary>
            Task SetSnapshotsAsync(List<tb_InventorySnapshot> snapshots, long? warehouseId = null, long? productId = null, DateTime? snapshotDate = null, TimeSpan? expiration = null);

            /// <summary>
            /// 获取库存流水
            /// </summary>
            List<tb_InventoryTransaction> GetTransactions(DateTime startTime, DateTime endTime, long? warehouseId = null, long? productId = null);

            /// <summary>
            /// 异步获取库存流水
            /// </summary>
            Task<List<tb_InventoryTransaction>> GetTransactionsAsync(DateTime startTime, DateTime endTime, long? warehouseId = null, long? productId = null);

            /// <summary>
            /// 设置库存流水缓存
            /// </summary>
            void SetTransactions(List<tb_InventoryTransaction> transactions, DateTime startTime, DateTime endTime, long? warehouseId = null, long? productId = null, TimeSpan? expiration = null);

            /// <summary>
            /// 异步设置库存流水缓存
            /// </summary>
            Task SetTransactionsAsync(List<tb_InventoryTransaction> transactions, DateTime startTime, DateTime endTime, long? warehouseId = null, long? productId = null, TimeSpan? expiration = null);

            /// <summary>
            /// 移除商品相关的库存缓存
            /// </summary>
            void RemoveProductInventoryCache(long productId);

            /// <summary>
            /// 异步移除商品相关的库存缓存
            /// </summary>
            Task RemoveProductInventoryCacheAsync(long productId);
        }
    


}
