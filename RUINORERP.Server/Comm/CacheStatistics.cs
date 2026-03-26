using System;
using System.Collections.Generic;
using System.Threading;

namespace RUINORERP.Server.Comm
{
    /// <summary>
    /// 缓存统计数据
    /// </summary>
    public class CacheStatistics
    {
        /// <summary>
        /// 总请求数
        /// </summary>
        public long TotalRequests;

        /// <summary>
        /// 命中次数
        /// </summary>
        public long HitCount;

        /// <summary>
        /// 未命中次数
        /// </summary>
        public long MissCount;

        /// <summary>
        /// 错误次数
        /// </summary>
        public long ErrorCount;

        /// <summary>
        /// 命中率
        /// </summary>
        public double HitRate => TotalRequests > 0 ? (double)HitCount / TotalRequests : 0;

        /// <summary>
        /// 最后重置时间
        /// </summary>
        public DateTime LastResetTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 各键访问次数统计
        /// </summary>
        public Dictionary<string, long> KeyAccessCount { get; set; } = new Dictionary<string, long>();

        /// <summary>
        /// 记录命中
        /// </summary>
        /// <param name="key">缓存键</param>
        public void RecordHit(string key)
        {
            Interlocked.Increment(ref TotalRequests);
            Interlocked.Increment(ref HitCount);
            RecordKeyAccess(key);
        }

        /// <summary>
        /// 记录未命中
        /// </summary>
        public void RecordMiss()
        {
            Interlocked.Increment(ref TotalRequests);
            Interlocked.Increment(ref MissCount);
        }

        /// <summary>
        /// 记录错误
        /// </summary>
        public void RecordError()
        {
            Interlocked.Increment(ref TotalRequests);
            Interlocked.Increment(ref ErrorCount);
        }

        /// <summary>
        /// 记录键访问
        /// </summary>
        private void RecordKeyAccess(string key)
        {
            lock (KeyAccessCount)
            {
                if (KeyAccessCount.ContainsKey(key))
                    KeyAccessCount[key]++;
                else
                    KeyAccessCount[key] = 1;
            }
        }

        /// <summary>
        /// 重置统计
        /// </summary>
        public void Reset()
        {
            Interlocked.Exchange(ref TotalRequests, 0);
            Interlocked.Exchange(ref HitCount, 0);
            Interlocked.Exchange(ref MissCount, 0);
            Interlocked.Exchange(ref ErrorCount, 0);
            lock (KeyAccessCount)
            {
                KeyAccessCount.Clear();
            }
            LastResetTime = DateTime.Now;
        }
    }
}
