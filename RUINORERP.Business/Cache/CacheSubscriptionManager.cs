using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace RUINORERP.Business.Cache
{
    /// <summary>
    /// 服务器的缓存订阅管理器
    /// 负责管理基于会话ID的缓存订阅关系，实现高效的缓存变更通知分发
    /// </summary>
    public class CacheSubscriptionManager : IDisposable
    {
        // 管理每个表的订阅者（会话ID）
        private readonly ConcurrentDictionary<string, HashSet<string>> _tableSubscribers;
        private readonly object _lock = new object();
        private readonly ILogger<CacheSubscriptionManager> _logger;

        public CacheSubscriptionManager(ILogger<CacheSubscriptionManager> logger)
        {
            _logger = logger;
            _tableSubscribers = new ConcurrentDictionary<string, HashSet<string>>();
        }

        #region 核心订阅管理方法

        /// <summary>
        /// 添加特定会话对指定表的订阅
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="sessionId">会话ID</param>
        /// <returns>异步任务</returns>
        public async Task AddSubscriptionAsync(string tableName, string sessionId)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentNullException(nameof(tableName), "表名不能为空");
            }
            if (string.IsNullOrEmpty(sessionId))
            {
                throw new ArgumentNullException(nameof(sessionId), "会话ID不能为空");
            }

            await Task.Run(() =>
            {
                lock (_lock)
                {
                    if (!_tableSubscribers.TryGetValue(tableName, out var subscribers))
                    {
                        subscribers = new HashSet<string>();
                        _tableSubscribers[tableName] = subscribers;
                    }
                    subscribers.Add(sessionId);
                }
            });
        }

        /// <summary>
        /// 移除特定会话对指定表的订阅
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="sessionId">会话ID</param>
        /// <returns>异步任务</returns>
        public void RemoveSubscriptionAsync(string tableName, string sessionId)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentNullException(nameof(tableName), "表名不能为空");
            }
            if (string.IsNullOrEmpty(sessionId))
            {
                throw new ArgumentNullException(nameof(sessionId), "会话ID不能为空");
            }

            lock (_lock)
            {
                if (_tableSubscribers.TryGetValue(tableName, out var subscribers))
                {
                    subscribers.Remove(sessionId);

                    // 如果没有会话订阅此表，则移除该表的订阅列表
                    if (subscribers.Count == 0)
                    {
                        _tableSubscribers.TryRemove(tableName, out _);
                    }
                }
            }
        }

        /// <summary>
        /// 移除特定会话的所有订阅（会话断开时调用）
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <returns>成功取消订阅的表数量</returns>
        public int RemoveAllSubscriptionsAsync(string sessionId)
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                throw new ArgumentNullException(nameof(sessionId), "会话ID不能为空");
            }

            int removedCount = 0;

            lock (_lock)
            {
                // 获取所有表名的副本，避免修改集合时的枚举问题
                var tableNames = _tableSubscribers.Keys.ToList();

                foreach (var tableName in tableNames)
                {
                    if (_tableSubscribers.TryGetValue(tableName, out var subscribers) && subscribers.Contains(sessionId))
                    {
                        subscribers.Remove(sessionId);
                        removedCount++;

                        // 如果没有会话订阅此表，则移除该表的订阅列表
                        if (subscribers.Count == 0)
                        {
                            _tableSubscribers.TryRemove(tableName, out _);
                        }
                    }
                }
            }

            return removedCount;
        }

        #endregion

        #region 查询方法

        /// <summary>
        /// 获取特定表的订阅会话集合
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>订阅会话ID集合</returns>
        public IEnumerable<string> GetSubscribers(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentNullException(nameof(tableName), "表名不能为空");
            }

            lock (_lock)
            {
                if (_tableSubscribers.TryGetValue(tableName, out var subscribers))
                {
                    return subscribers.ToList();
                }

                return Enumerable.Empty<string>();
            }
        }

        /// <summary>
        /// 检查会话是否订阅了指定表
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="sessionId">会话ID</param>
        /// <returns>是否订阅</returns>
        public bool IsSubscribed(string tableName, string sessionId)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentNullException(nameof(tableName), "表名不能为空");
            }
            if (string.IsNullOrEmpty(sessionId))
            {
                throw new ArgumentNullException(nameof(sessionId), "会话ID不能为空");
            }

            lock (_lock)
            {
                return _tableSubscribers.TryGetValue(tableName, out var subscribers) && subscribers.Contains(sessionId);
            }
        }

        /// <summary>
        /// 获取特定会话订阅的所有表名
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <returns>会话订阅的表名集合</returns>
        public IEnumerable<string> GetSubscribedTables(string sessionId)
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                throw new ArgumentNullException(nameof(sessionId), "会话ID不能为空");
            }

            List<string> subscribedTables = new List<string>();

            lock (_lock)
            {
                foreach (var tableEntry in _tableSubscribers)
                {
                    if (tableEntry.Value.Contains(sessionId))
                    {
                        subscribedTables.Add(tableEntry.Key);
                    }
                }
            }

            return subscribedTables;
        }

        /// <summary>
        /// 获取所有订阅的表名
        /// </summary>
        /// <returns>订阅的表名集合</returns>
        public IEnumerable<string> GetTables()
        {
            lock (_lock)
            {
                return _tableSubscribers.Keys.ToList();
            }
        }

        /// <summary>
        /// 获取订阅统计信息
        /// </summary>
        /// <returns>订阅统计信息</returns>
        public SubscriptionStatistics GetStatistics()
        {
            lock (_lock)
            {
                return new SubscriptionStatistics
                {
                    TotalSubscriptions = _tableSubscribers.Sum(kvp => kvp.Value.Count),
                    TotalTables = _tableSubscribers.Count,
                    TableSubscriptionCount = _tableSubscribers.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Count)
                };
            }
        }

        #endregion

        #region IDisposable 实现

        /// <summary>
        /// 标记是否已释放资源
        /// </summary>
        private bool _disposed = false;

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="disposing">是否释放托管资源</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_tableSubscribers != null)
                    {
                        _tableSubscribers.Clear();
                    }
                }

                _disposed = true;
            }
        }

        #endregion
    }

    /// <summary>
    /// 订阅统计信息
    /// </summary>
    public class SubscriptionStatistics
    {
        /// <summary>
        /// 总订阅数
        /// </summary>
        public int TotalSubscriptions { get; set; }

        /// <summary>
        /// 总表数
        /// </summary>
        public int TotalTables { get; set; }

        /// <summary>
        /// 每个表的订阅者数量
        /// </summary>
        public Dictionary<string, int> TableSubscriptionCount { get; set; } = new Dictionary<string, int>();
    }
}