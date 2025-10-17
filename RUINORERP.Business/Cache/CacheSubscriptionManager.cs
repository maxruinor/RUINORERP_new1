using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Commands.Cache;
using RUINORERP.PacketSpec.Models.Requests.Cache;
using RUINORERP.PacketSpec.Models.Responses.Cache;

namespace RUINORERP.Business.CommService
{
    /// <summary>
    /// 缓存订阅管理器 - 可以被服务器和客户端共同使用
    /// 通过IsServerMode属性区分服务器端和客户端模式
    /// </summary>
    public class CacheSubscriptionManager
    {
        // 服务器端使用：管理每个表的订阅者（会话ID）
        private readonly ConcurrentDictionary<string, HashSet<string>> _tableSubscribers;
        // 客户端使用：管理订阅的表
        private readonly ConcurrentDictionary<string, bool> _subscriptions;
        private readonly object _lock = new object();
        private readonly ILogger<CacheSubscriptionManager> _logger;
        
        // 客户端特有字段
        private object _commService; // 客户端通信服务，避免直接引用导致的依赖问题

        /// <summary>
        /// 是否为服务器端模式（管理多个会话）
        /// </summary>
        public bool IsServerMode { get;  set; }

        public CacheSubscriptionManager(ILogger<CacheSubscriptionManager> logger, bool isServerMode = false)
        {
            _logger = logger;
            IsServerMode = isServerMode;

            if (isServerMode)
            {
                _tableSubscribers = new ConcurrentDictionary<string, HashSet<string>>();
                _subscriptions = null;
            }
            else
            {
                _subscriptions = new ConcurrentDictionary<string, bool>();
                _tableSubscribers = null;
            }
        }
        
        #region 服务器端方法

        /// <summary>
        /// 服务器端：订阅缓存变更通知
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <param name="tableName">表名</param>
        /// <returns>是否订阅成功</returns>
        public bool Subscribe(string sessionId, string tableName)
        {
            if (!IsServerMode)
            {
                throw new InvalidOperationException("此方法仅在服务器模式下可用");
            }

            try
            {
                lock (_lock)
                {
                    // 添加会话到表订阅者列表
                    _tableSubscribers.AddOrUpdate(
                        tableName,
                        _ => new HashSet<string> { sessionId },
                        (_, subscribers) =>
                        {
                            subscribers.Add(sessionId);
                            return subscribers;
                        });

                    _logger.Debug($"会话 {sessionId} 订阅了表 {tableName} 的缓存变更通知");
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"会话 {sessionId} 订阅表 {tableName} 失败");
                return false;
            }
        }

        /// <summary>
        /// 服务器端：取消订阅缓存变更通知
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <param name="tableName">表名</param>
        /// <returns>是否取消订阅成功</returns>
        public bool Unsubscribe(string sessionId, string tableName)
        {
            if (!IsServerMode)
            {
                throw new InvalidOperationException("此方法仅在服务器模式下可用");
            }

            try
            {
                lock (_lock)
                {
                    // 从表订阅者列表中移除会话
                    if (_tableSubscribers.TryGetValue(tableName, out var subscribers))
                    {
                        subscribers.Remove(sessionId);
                        if (subscribers.Count == 0)
                        {
                            _tableSubscribers.TryRemove(tableName, out _);
                        }
                    }

                    _logger.Debug($"会话 {sessionId} 取消订阅了表 {tableName} 的缓存变更通知");
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"会话 {sessionId} 取消订阅表 {tableName} 失败");
                return false;
            }
        }

        /// <summary>
        /// 服务器端：获取订阅指定表的所有会话ID
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>订阅该表的会话ID列表</returns>
        public IEnumerable<string> GetSubscribers(string tableName)
        {
            if (!IsServerMode)
            {
                throw new InvalidOperationException("此方法仅在服务器模式下可用");
            }

            if (_tableSubscribers.TryGetValue(tableName, out var subscribers))
            {
                return subscribers.ToList();
            }
            return Enumerable.Empty<string>();
        }

        /// <summary>
        /// 服务器端：取消会话的所有订阅（会话断开时调用）
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        public void UnsubscribeAll(string sessionId)
        {
            if (!IsServerMode)
            {
                throw new InvalidOperationException("此方法仅在服务器模式下可用");
            }

            try
            {
                lock (_lock)
                {
                    // 遍历所有表，从订阅者列表中移除该会话
                    foreach (var kvp in _tableSubscribers)
                    {
                        var tableName = kvp.Key;
                        var subscribers = kvp.Value;
                        
                        if (subscribers.Remove(sessionId) && subscribers.Count == 0)
                        {
                            _tableSubscribers.TryRemove(tableName, out _);
                        }
                    }

                    _logger.Debug($"会话 {sessionId} 的所有订阅已取消");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"取消会话 {sessionId} 的所有订阅时发生错误");
            }
        }

        #endregion

        #region 客户端方法

        /// <summary>
        /// 客户端：设置通信服务
        /// </summary>
        /// <param name="commService">通信服务</param>
        public void SetCommunicationService(object commService)
        {
            if (IsServerMode)
            {
                throw new InvalidOperationException("此方法仅在客户端模式下可用");
            }
            
            _commService = commService;
        }

        /// <summary>
        /// 客户端：订阅缓存变更通知
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>是否订阅成功</returns>
        public async Task<bool> SubscribeAsync(string tableName)
        {
            if (IsServerMode)
            {
                throw new InvalidOperationException("此方法仅在客户端模式下可用");
            }

            try
            {
                // 如果已经订阅，则直接返回
                if (IsSubscribed(tableName))
                {
                    _logger.Debug($"表 {tableName} 已经订阅，无需重复订阅");
                    return true;
                }

                // 如果有通信服务，发送订阅命令
                if (_commService != null)
                {
                    // 创建订阅请求
                    var request = new CacheRequest
                    {
                        TableName = tableName,
                        Operation =CacheOperation.Get
                    };

                    // 发送订阅命令（通过反射调用，避免直接依赖）
                    var commType = _commService.GetType();
                    var sendMethod = commType.GetMethod("SendCommandWithResponseAsync", 
                        new[] { typeof(CacheCommand), typeof(System.Threading.CancellationToken), typeof(int) });
                    
                    if (sendMethod != null)
                    {
                        var cacheCommand = new CacheCommand
                        {
                            Request = request
                        };
                        
                        var task = (Task)sendMethod.Invoke(_commService, new object[] { cacheCommand, System.Threading.CancellationToken.None, 30000 });
                        await task;
                        
                        var resultProperty = task.GetType().GetProperty("Result");
                        var response = resultProperty?.GetValue(task);
                        
                        var isSuccessProperty = response?.GetType().GetProperty("IsSuccess");
                        var isSuccess = (bool?)isSuccessProperty?.GetValue(response) ?? false;
                        
                        if (!isSuccess)
                        {
                            _logger.LogWarning($"订阅表 {tableName} 失败");
                            return false;
                        }
                    }
                }

                // 添加本地订阅
                AddSubscription(tableName);
                _logger.Debug($"成功订阅表 {tableName} 的缓存变更通知");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"订阅表 {tableName} 时发生异常");
                return false;
            }
        }

        /// <summary>
        /// 客户端：取消订阅缓存变更通知
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>是否取消订阅成功</returns>
        public async Task<bool> UnsubscribeAsync(string tableName)
        {
            if (IsServerMode)
            {
                throw new InvalidOperationException("此方法仅在客户端模式下可用");
            }

            try
            {
                // 如果未订阅，则直接返回
                if (!IsSubscribed(tableName))
                {
                    _logger.Debug($"表 {tableName} 未订阅，无需取消订阅");
                    return true;
                }

                // 如果有通信服务，发送取消订阅命令
                if (_commService != null)
                {
                    // 创建取消订阅请求
                    var request = new CacheRequest
                    {
                        TableName = tableName,
                        //Operation = CacheOperation.un "Cache.Unsubscribe"
                    };

                    // 发送取消订阅命令（通过反射调用，避免直接依赖）
                    var commType = _commService.GetType();
                    var sendMethod = commType.GetMethod("SendCommandWithResponseAsync", 
                        new[] { typeof(CacheCommand), typeof(System.Threading.CancellationToken), typeof(int) });
                    
                    if (sendMethod != null)
                    {
                        var cacheCommand = new CacheCommand
                        {
                            Request = request
                        };
                        
                        var task = (Task)sendMethod.Invoke(_commService, new object[] { cacheCommand, System.Threading.CancellationToken.None, 30000 });
                        await task;
                        
                        var resultProperty = task.GetType().GetProperty("Result");
                        var response = resultProperty?.GetValue(task);
                        
                        var isSuccessProperty = response?.GetType().GetProperty("IsSuccess");
                        var isSuccess = (bool?)isSuccessProperty?.GetValue(response) ?? false;
                        
                        if (!isSuccess)
                        {
                            _logger.LogWarning($"取消订阅表 {tableName} 失败");
                            return false;
                        }
                    }
                }

                // 移除本地订阅
                RemoveSubscription(tableName);
                _logger.Debug($"成功取消订阅表 {tableName} 的缓存变更通知");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"取消订阅表 {tableName} 时发生异常");
                return false;
            }
        }

        /// <summary>
        /// 客户端：添加订阅
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>是否添加成功</returns>
        public bool AddSubscription(string tableName)
        {
            if (IsServerMode)
            {
                throw new InvalidOperationException("此方法仅在客户端模式下可用");
            }

            lock (_lock)
            {
                return _subscriptions.TryAdd(tableName, true);
            }
        }

        /// <summary>
        /// 客户端：移除订阅
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>是否移除成功</returns>
        public bool RemoveSubscription(string tableName)
        {
            if (IsServerMode)
            {
                throw new InvalidOperationException("此方法仅在客户端模式下可用");
            }

            lock (_lock)
            {
                return _subscriptions.TryRemove(tableName, out _);
            }
        }

        /// <summary>
        /// 客户端：检查是否已订阅指定表
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <returns>是否已订阅</returns>
        public bool IsSubscribed(string tableName)
        {
            if (IsServerMode)
            {
                throw new InvalidOperationException("此方法仅在客户端模式下可用");
            }

            lock (_lock)
            {
                return _subscriptions.ContainsKey(tableName);
            }
        }

        /// <summary>
        /// 客户端：获取所有订阅的表名
        /// </summary>
        /// <returns>订阅的表名列表</returns>
        public IEnumerable<string> GetSubscriptions()
        {
            if (IsServerMode)
            {
                throw new InvalidOperationException("此方法仅在客户端模式下可用");
            }

            lock (_lock)
            {
                return _subscriptions.Keys.ToList();
            }
        }

        /// <summary>
        /// 客户端：清空所有订阅
        /// </summary>
        public void ClearSubscriptions()
        {
            if (IsServerMode)
            {
                throw new InvalidOperationException("此方法仅在客户端模式下可用");
            }

            lock (_lock)
            {
                _subscriptions.Clear();
            }
        }

        /// <summary>
        /// 客户端：取消所有订阅
        /// </summary>
        /// <returns>取消订阅的表数量</returns>
        public async Task<int> UnsubscribeAllAsync()
        {
            if (IsServerMode)
            {
                throw new InvalidOperationException("此方法仅在客户端模式下可用");
            }

            try
            {
                var subscriptions = GetSubscriptions().ToList();
                int successCount = 0;

                foreach (var tableName in subscriptions)
                {
                    if (await UnsubscribeAsync(tableName))
                    {
                        successCount++;
                    }
                }

                _logger.Debug($"取消了 {successCount}/{subscriptions.Count} 个表的订阅");
                return successCount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取消所有订阅时发生异常");
                return 0;
            }
        }

        #endregion

        /// <summary>
        /// 获取订阅统计信息
        /// </summary>
        /// <returns>订阅统计信息</returns>
        public SubscriptionStatistics GetStatistics()
        {
            lock (_lock)
            {
                if (IsServerMode)
                {
                    return new SubscriptionStatistics
                    {
                        TotalSubscriptions = _tableSubscribers.Sum(kvp => kvp.Value.Count),
                        TotalSubscribers = _tableSubscribers.Count,
                        TableSubscriptionCount = _tableSubscribers.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Count)
                    };
                }
                else
                {
                    return new SubscriptionStatistics
                    {
                        TotalSubscriptions = _subscriptions.Count,
                        TotalSubscribers = 1, // 客户端只有一个订阅者
                        TableSubscriptionCount = _subscriptions.ToDictionary(kvp => kvp.Key, kvp => 1)
                    };
                }
            }
        }
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
        /// 总订阅者数（会话数）
        /// </summary>
        public int TotalSubscribers { get; set; }

        /// <summary>
        /// 每个表的订阅者数量
        /// </summary>
        public Dictionary<string, int> TableSubscriptionCount { get; set; } = new Dictionary<string, int>();
    }
}