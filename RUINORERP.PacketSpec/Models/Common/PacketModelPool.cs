using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Core;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Serialization;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.PacketSpec.Models.Core;

namespace RUINORERP.PacketSpec.Models.Common
{
    /// <summary>
    /// 数据包对象池 - 用于减少GC压力，解决内存泄漏问题
    /// 通过复用 PacketModel 对象，减少频繁创建/销毁对象带来的内存碎片
    /// </summary>
    public static class PacketModelPool
    {
        private static readonly ConcurrentBag<PacketModel> _pool = new();
        private static readonly int MaxPoolSize = 1000;
        private static long _poolHits = 0;
        private static long _poolMisses = 0;

        /// <summary>
        /// 从对象池获取 PacketModel
        /// </summary>
        /// <returns>PacketModel 实例</returns>
        public static PacketModel Rent()
        {
            if (_pool.TryTake(out var item))
            {
                System.Threading.Interlocked.Increment(ref _poolHits);
                ResetPacketModel(item);
                return item;
            }

            System.Threading.Interlocked.Increment(ref _poolMisses);
            return new PacketModel();
        }

        /// <summary>
        /// 将 PacketModel 归还到对象池
        /// </summary>
        /// <param name="item">要归还的实例</param>
        public static void Return(PacketModel item)
        {
            if (item == null)
                return;

            // 清理数据
            ResetPacketModel(item);

            // 如果池未满，放回池中
            if (_pool.Count < MaxPoolSize)
            {
                _pool.Add(item);
            }
            // 否则让GC回收
        }

        /// <summary>
        /// 重置 PacketModel 到初始状态
        /// P0优化：彻底切断 JObject 引用链，防止 LOH 碎片化
        /// </summary>
        /// <param name="item">要重置的实例</param>
        private static void ResetPacketModel(PacketModel item)
        {
            item.PacketId = Guid.NewGuid().ToString();
            item.CommandId = default;
            item.Status = PacketStatus.Created;
            item.Direction = PacketDirection.Unknown;
            item.PacketPriority = PacketPriority.Normal;
            item.CreatedTime = DateTime.Now;
            item.TimestampUtc = DateTime.UtcNow;

            // P0优化：直接创建新实例以彻底切断 LOH 引用链
            // 避免 RemoveAll() 后内部节点仍滞留在 LOH
            item.Extensions = new JObject();

            // 清理执行上下文
            item.ExecutionContext = null;

            // 清理请求/响应
            item.Request = null;
            item.Response = null;
        }

        /// <summary>
        /// 获取对象池统计信息
        /// </summary>
        public static PoolStatistics GetStatistics()
        {
            var totalRequests = _poolHits + _poolMisses;
            var hitRate = totalRequests > 0 ? (double)_poolHits / totalRequests * 100 : 0;

            return new PoolStatistics
            {
                PoolSize = _pool.Count,
                PoolHits = _poolHits,
                PoolMisses = _poolMisses,
                HitRatePercent = hitRate,
                MaxPoolSize = MaxPoolSize
            };
        }

        /// <summary>
        /// 预热对象池
        /// </summary>
        /// <param name="count">预热的数量</param>
        public static void WarmUp(int count = 50)
        {
            for (int i = 0; i < count && _pool.Count < MaxPoolSize; i++)
            {
                _pool.Add(new PacketModel());
            }
        }

        /// <summary>
        /// 清空对象池
        /// </summary>
        public static void Clear()
        {
            while (_pool.TryTake(out _)) { }
        }

        /// <summary>
        /// 对象池统计信息
        /// </summary>
        public class PoolStatistics
        {
            public int PoolSize { get; set; }
            public long PoolHits { get; set; }
            public long PoolMisses { get; set; }
            public double HitRatePercent { get; set; }
            public int MaxPoolSize { get; set; }
        }
    }

    /// <summary>
    /// 池化的 PacketModel 包装器，自动归还到对象池
    /// </summary>
    public struct PooledPacketModel : IDisposable
    {
        private readonly PacketModel _model;

        public PooledPacketModel(PacketModel model)
        {
            _model = model;
        }

        public PacketModel Model => _model;

        public void Dispose()
        {
            PacketModelPool.Return(_model);
        }
    }
}
