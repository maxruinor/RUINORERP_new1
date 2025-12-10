using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Models.Lock;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network.Services
{
    /// <summary>
    /// 锁状态通知服务
    /// 用于在锁命令处理器和UI窗体之间传递锁状态变化
    /// 实现观察者模式，支持多个窗体订阅锁状态变化
    /// </summary>
    public class LockStatusNotificationService : IDisposable
    {
        #region 私有字段

        private readonly ILogger<LockStatusNotificationService> _logger;
        private readonly ConcurrentDictionary<long, List<LockStatusSubscriber>> _subscribers;
        private readonly ConcurrentDictionary<long, LockInfo> _latestLockInfo;
        private readonly object _lockObj = new object();
        private bool _isDisposed = false;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        public LockStatusNotificationService(ILogger<LockStatusNotificationService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _subscribers = new ConcurrentDictionary<long, List<LockStatusSubscriber>>();
            _latestLockInfo = new ConcurrentDictionary<long, LockInfo>();
            
            _logger.LogDebug("锁状态通知服务已初始化");
        }

        #endregion

        #region 订阅管理

        /// <summary>
        /// 订阅锁状态变化
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="formId">窗体唯一标识</param>
        /// <param name="onLockStatusChanged">锁状态变化回调</param>
        /// <returns>订阅ID，用于取消订阅</returns>
        public string SubscribeToLockStatus(long billId, string formId, Action<LockStatusChangeEventArgs> onLockStatusChanged)
        {
            if (string.IsNullOrEmpty(formId))
                throw new ArgumentException("窗体ID不能为空", nameof(formId));
            
            if (onLockStatusChanged == null)
                throw new ArgumentNullException(nameof(onLockStatusChanged));

            var subscriber = new LockStatusSubscriber
            {
                Id = Guid.NewGuid().ToString(),
                FormId = formId,
                BillId = billId,
                Callback = onLockStatusChanged,
                SubscribeTime = DateTime.Now
            };

            // 添加到订阅列表
            var subscribers = _subscribers.GetOrAdd(billId, _ => new List<LockStatusSubscriber>());
            
            lock (subscribers)
            {
                // 检查是否已存在相同的窗体订阅，避免重复订阅
                var existingSubscriber = subscribers.Find(s => s.FormId == formId);
                if (existingSubscriber != null)
                {
                    // 如果已存在，先移除旧的订阅
                    subscribers.Remove(existingSubscriber);
                }
                
                subscribers.Add(subscriber);
            }

            // 如果已有锁信息，立即触发一次回调
            if (_latestLockInfo.TryGetValue(billId, out var currentLockInfo))
            {
                try
                {
                    var args = new LockStatusChangeEventArgs
                    {
                        BillId = billId,
                        LockInfo = currentLockInfo,
                        ChangeType = LockStatusChangeType.StatusUpdated,
                        Timestamp = DateTime.Now
                    };
                    onLockStatusChanged(args);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "触发锁状态变化回调时发生异常: 单据ID={BillId}, 订阅ID={SubscriberId}", billId, subscriber.Id);
                }
            }

            _logger.LogDebug("窗体 {FormId} 已订阅单据 {BillId} 的锁状态变化", formId, billId);
            return subscriber.Id;
        }

        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="subscriptionId">订阅ID</param>
        public void UnsubscribeFromLockStatus(long billId, string subscriptionId)
        {
            if (string.IsNullOrEmpty(subscriptionId))
                return;

            if (_subscribers.TryGetValue(billId, out var subscribers))
            {
                lock (subscribers)
                {
                    var subscriber = subscribers.Find(s => s.Id == subscriptionId);
                    if (subscriber != null)
                    {
                        subscribers.Remove(subscriber);
                        _logger.LogDebug("窗体 {FormId} 已取消订阅单据 {BillId} 的锁状态变化", subscriber.FormId, billId);
                        
                        // 如果没有订阅者了，移除整个列表
                        if (subscribers.Count == 0)
                        {
                            _subscribers.TryRemove(billId, out _);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 取消窗体的所有订阅
        /// </summary>
        /// <param name="formId">窗体ID</param>
        public void UnsubscribeForm(string formId)
        {
            if (string.IsNullOrEmpty(formId))
                return;

            var unsubscribedCount = 0;
            
            // 遍历所有单据的订阅列表
            foreach (var kvp in _subscribers)
            {
                var billId = kvp.Key;
                var subscribers = kvp.Value;
                
                lock (subscribers)
                {
                    var toRemove = subscribers.FindAll(s => s.FormId == formId);
                    foreach (var subscriber in toRemove)
                    {
                        subscribers.Remove(subscriber);
                        unsubscribedCount++;
                    }
                    
                    // 如果没有订阅者了，移除整个列表
                    if (subscribers.Count == 0)
                    {
                        _subscribers.TryRemove(billId, out _);
                    }
                }
            }

            if (unsubscribedCount > 0)
            {
                _logger.LogDebug("窗体 {FormId} 已取消 {Count} 个锁状态订阅", formId, unsubscribedCount);
            }
        }

        #endregion

        #region 通知方法

        /// <summary>
        /// 通知锁状态变化
        /// 由命令处理器调用，当收到服务器广播时调用此方法
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="lockInfo">锁信息</param>
        /// <param name="changeType">变化类型</param>
        public void NotifyLockStatusChanged(long billId, LockInfo lockInfo, LockStatusChangeType changeType)
        {
            if (_isDisposed)
                return;

            try
            {
                // 更新最新的锁信息
                _latestLockInfo.AddOrUpdate(billId, lockInfo, (key, oldValue) => lockInfo);

                // 如果没有订阅者，直接返回
                if (!_subscribers.TryGetValue(billId, out var subscribers))
                    return;

                // 创建事件参数
                var args = new LockStatusChangeEventArgs
                {
                    BillId = billId,
                    LockInfo = lockInfo,
                    ChangeType = changeType,
                    Timestamp = DateTime.Now
                };

                List<LockStatusSubscriber> subscribersToNotify;
                lock (subscribers)
                {
                    // 创建副本，避免在回调过程中修改集合
                    subscribersToNotify = new List<LockStatusSubscriber>(subscribers);
                }

                // 通知所有订阅者
                foreach (var subscriber in subscribersToNotify)
                {
                    try
                    {
                        subscriber.Callback(args);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "通知订阅者锁状态变化时发生异常: 单据ID={BillId}, 订阅ID={SubscriberId}, 窗体ID={FormId}",
                            billId, subscriber.Id, subscriber.FormId);
                    }
                }

                _logger.LogDebug("已通知 {SubscriberCount} 个订阅者单据 {BillId} 的锁状态变化: {ChangeType}", 
                    subscribersToNotify.Count, billId, changeType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "通知锁状态变化时发生异常: 单据ID={BillId}", billId);
            }
        }

        /// <summary>
        /// 批量通知锁状态变化
        /// 用于处理批量锁状态广播
        /// </summary>
        /// <param name="lockInfos">锁信息列表</param>
        public void NotifyBatchLockStatusChanged(List<LockInfo> lockInfos)
        {
            if (_isDisposed || lockInfos == null || lockInfos.Count == 0)
                return;

            foreach (var lockInfo in lockInfos)
            {
                var changeType = lockInfo.IsLocked ? LockStatusChangeType.Locked : LockStatusChangeType.Unlocked;
                NotifyLockStatusChanged(lockInfo.BillID, lockInfo, changeType);
            }
        }

        #endregion

        #region 查询方法

        /// <summary>
        /// 获取最新的锁信息
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <returns>锁信息，如果没有则返回null</returns>
        public LockInfo GetLatestLockInfo(long billId)
        {
            _latestLockInfo.TryGetValue(billId, out var lockInfo);
            return lockInfo;
        }

        /// <summary>
        /// 获取指定单据的订阅者数量
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <returns>订阅者数量</returns>
        public int GetSubscriberCount(long billId)
        {
            if (_subscribers.TryGetValue(billId, out var subscribers))
            {
                lock (subscribers)
                {
                    return subscribers.Count;
                }
            }
            return 0;
        }

        /// <summary>
        /// 获取所有订阅的单据ID列表
        /// </summary>
        /// <returns>单据ID列表</returns>
        public List<long> GetAllSubscribedBillIds()
        {
            return new List<long>(_subscribers.Keys);
        }

        /// <summary>
        /// 检查指定窗体是否订阅了指定单据的锁状态
        /// </summary>
        /// <param name="billId">单据ID</param>
        /// <param name="formId">窗体ID</param>
        /// <returns>是否已订阅</returns>
        public bool IsFormSubscribed(long billId, string formId)
        {
            if (_subscribers.TryGetValue(billId, out var subscribers))
            {
                lock (subscribers)
                {
                    return subscribers.Exists(s => s.FormId == formId);
                }
            }
            return false;
        }

        #endregion

        #region IDisposable 实现

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (_isDisposed)
                return;

            try
            {
                // 清理所有订阅者
                _subscribers.Clear();
                _latestLockInfo.Clear();
                _isDisposed = true;
                
                _logger.LogDebug("锁状态通知服务已释放");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "释放锁状态通知服务资源时发生异常");
            }
        }

        #endregion
    }

    #region 辅助类和枚举

    /// <summary>
    /// 锁状态订阅者
    /// </summary>
    internal class LockStatusSubscriber
    {
        public string Id { get; set; } = string.Empty;
        public string FormId { get; set; } = string.Empty;
        public long BillId { get; set; }
        public Action<LockStatusChangeEventArgs> Callback { get; set; } = null!;
        public DateTime SubscribeTime { get; set; }
    }

    /// <summary>
    /// 锁状态变化事件参数
    /// </summary>
    public class LockStatusChangeEventArgs : EventArgs
    {
        /// <summary>
        /// 单据ID
        /// </summary>
        public long BillId { get; set; }

        /// <summary>
        /// 锁信息
        /// </summary>
        public LockInfo LockInfo { get; set; } = null!;

        /// <summary>
        /// 变化类型
        /// </summary>
        public LockStatusChangeType ChangeType { get; set; }

        /// <summary>
        /// 变化时间
        /// </summary>
        public DateTime Timestamp { get; set; }
    }

    /// <summary>
    /// 锁状态变化类型
    /// </summary>
    public enum LockStatusChangeType
    {
        /// <summary>
        /// 未知变化
        /// </summary>
        Unknown,

        /// <summary>
        /// 锁定
        /// </summary>
        Locked,

        /// <summary>
        /// 解锁
        /// </summary>
        Unlocked,

        /// <summary>
        /// 锁状态更新
        /// </summary>
        StatusUpdated,

        /// <summary>
        /// 锁持有者变更
        /// </summary>
        OwnerChanged
    }

    #endregion
}