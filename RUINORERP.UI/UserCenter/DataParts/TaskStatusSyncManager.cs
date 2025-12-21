using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using RUINORERP.Global;
using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Models.Common;

namespace RUINORERP.UI.UserCenter.DataParts
{
    /// <summary>
    /// 任务状态同步管理器 - 核心分发中心
    /// 单一职责：负责集中管理任务状态更新的订阅和高效分发
    /// 作为UI层状态更新的核心枢纽，连接网络通知和UI组件
    /// </summary>
    public class TodoSyncManager : IExcludeFromRegistration
    {
        #region 单例模式
        private static readonly Lazy<TodoSyncManager> _instance = 
            new Lazy<TodoSyncManager>(() => new TodoSyncManager());

        /// <summary>
        /// 获取TodoSyncManager的单例实例
        /// </summary>
        public static TodoSyncManager Instance => _instance.Value;

        private TodoSyncManager()
        {
            _subscribers = new ConcurrentDictionary<Guid, TodoSyncSubscriber>();
            _pendingUpdates = new ConcurrentQueue<TodoUpdate>();
            StartUpdateProcessor();
        }
        #endregion

        #region 字段
        private readonly ConcurrentDictionary<Guid, TodoSyncSubscriber> _subscribers;
        private readonly ConcurrentQueue<TodoUpdate> _pendingUpdates;
        private volatile bool _isProcessingUpdates = false;
        private readonly object _updateLock = new object();
        #endregion

        #region 公共方法

        /// <summary>
        /// 订阅任务状态更新通知
        /// </summary>
        /// <param name="subscriberKey">订阅者唯一标识符</param>
        /// <param name="updateCallback">状态更新回调函数</param>
        /// <param name="businessTypes">感兴趣的业务类型列表，若为空则接收所有业务类型的更新</param>
        /// <returns>订阅ID，用于取消订阅</returns>
        public Guid Subscribe(Guid subscriberKey, Action<List<TodoUpdate>> updateCallback, List<BizType> businessTypes = null)
        {
            if (updateCallback == null)
                throw new ArgumentNullException(nameof(updateCallback));

            var subscriber = new TodoSyncSubscriber
            {
                SubscriberKey = subscriberKey,
                UpdateCallback = updateCallback,
                BusinessTypes = businessTypes ?? new List<BizType>()
            };

            _subscribers.TryAdd(subscriberKey, subscriber);
            return subscriberKey;
        }

        /// <summary>
        /// 取消订阅任务状态更新通知
        /// </summary>
        /// <param name="subscriberId">订阅ID</param>
        /// <returns>取消订阅是否成功</returns>
        public bool Unsubscribe(Guid subscriberId)
        {
            return _subscribers.TryRemove(subscriberId, out _);
        }

        /// <summary>
        /// 发布任务状态更新
        /// </summary>
        /// <param name="update">任务状态更新信息</param>
        public void PublishUpdate(TodoUpdate update)
        {
            if (update == null)
                throw new ArgumentNullException(nameof(update));

            _pendingUpdates.Enqueue(update);
            ProcessPendingUpdates();
        }

        /// <summary>
        /// 批量发布任务状态更新
        /// </summary>
        /// <param name="updates">任务状态更新列表</param>
        public void PublishUpdates(List<TodoUpdate> updates)
        {
            if (updates == null || !updates.Any())
                return;

            foreach (var update in updates)
            {
                _pendingUpdates.Enqueue(update);
            }
            ProcessPendingUpdates();
        }

     
        #endregion

        #region 私有方法

        /// <summary>
        /// 处理待处理的状态更新
        /// </summary>
        private void ProcessPendingUpdates()
        {
            if (_isProcessingUpdates)
                return;

            lock (_updateLock)
            {
                if (_isProcessingUpdates)
                    return;

                _isProcessingUpdates = true;

                try
                {
                    var updatesToProcess = new List<TodoUpdate>();
                    TodoUpdate update;

                    // 尝试出队最多100个更新进行处理，避免处理时间过长
                    int maxUpdates = 100;
                    while (updatesToProcess.Count < maxUpdates && _pendingUpdates.TryDequeue(out update))
                    {
                        updatesToProcess.Add(update);
                    }

                    if (updatesToProcess.Any())
                    {
                        DistributeUpdatesToSubscribers(updatesToProcess);
                    }
                }
                finally
                {
                    _isProcessingUpdates = false;
                }

                // 如果还有未处理的更新，再次处理
                if (!_pendingUpdates.IsEmpty)
                {
                    Task.Run(() => ProcessPendingUpdates());
                }
            }
        }

        /// <summary>
        /// 分发更新给相关订阅者
        /// </summary>
        /// <param name="updates">需要分发的更新列表</param>
        private void DistributeUpdatesToSubscribers(List<TodoUpdate> updates)
        {
            // 对订阅者进行分组，根据其感兴趣的业务类型
            var subscribersByBusinessType = new Dictionary<BizType, List<TodoSyncSubscriber>>();
            var globalSubscribers = new List<TodoSyncSubscriber>(); // 订阅所有业务类型的订阅者

            foreach (var subscriber in _subscribers.Values)
            {
                if (subscriber.BusinessTypes == null || subscriber.BusinessTypes.Count == 0)
                {
                    globalSubscribers.Add(subscriber);
                }
                else
                {
                    foreach (var bizType in subscriber.BusinessTypes)
                    {
                        if (!subscribersByBusinessType.ContainsKey(bizType))
                        {
                            subscribersByBusinessType[bizType] = new List<TodoSyncSubscriber>();
                        }
                        subscribersByBusinessType[bizType].Add(subscriber);
                    }
                }
            }

            // 分发更新给全局订阅者
            foreach (var subscriber in globalSubscribers)
            {
                Task.Run(() =>
                {
                    try
                    {
                        // 在UI线程上执行回调
                        subscriber.InvokeCallback(updates);
                    }
                    catch (Exception ex)
                    {
                        // 记录错误日志，但不抛出异常，以免影响其他订阅者
                        LogError($"执行全局订阅者回调时出错: {ex.Message}", ex);
                    }
                });
            }

            // 按业务类型分发更新
            var updatesByBusinessType = updates.GroupBy(u => u.BusinessType).ToDictionary(g => g.Key, g => g.ToList());

            foreach (var kvp in updatesByBusinessType)
            {
                var bizType = kvp.Key;
                var businessUpdates = kvp.Value;

                if (subscribersByBusinessType.TryGetValue(bizType, out var subscribers))
                {
                    foreach (var subscriber in subscribers)
                    {
                        Task.Run(() =>
                        {
                            try
                            {
                                // 在UI线程上执行回调
                                subscriber.InvokeCallback(businessUpdates);
                            }
                            catch (Exception ex)
                            {
                                // 记录错误日志，但不抛出异常
                                LogError($"执行业务类型 {bizType} 的订阅者回调时出错: {ex.Message}", ex);
                            }
                        });
                    }
                }
            }
            
            // 处理网络任务状态更新
            foreach (var update in updates)
            {
                // 检查是否为来自服务器的更新
                if (update.IsFromServer)
                {
                    TodoMonitor.Instance.HandleNetworkTodoUpdate(update);
                }
            }
            
        }

        /// <summary>
        /// 启动更新处理器
        /// 定期检查是否有待处理的更新，确保更新不会丢失
        /// </summary>
        private void StartUpdateProcessor()
        {
            // 每100毫秒检查一次是否有待处理的更新
            System.Threading.Timer timer = new System.Threading.Timer(
                _ => ProcessPendingUpdates(),
                null,
                TimeSpan.FromMilliseconds(100),
                TimeSpan.FromMilliseconds(100));
        }

        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <param name="exception">异常信息</param>
        private void LogError(string message, Exception exception = null)
        {
            // 实际项目中应替换为项目的日志系统
            System.Diagnostics.Debug.WriteLine($"TodoSyncManager错误: {message}");
            if (exception != null)
            {
                System.Diagnostics.Debug.WriteLine($"异常详情: {exception}");
            }
        }
        #endregion
    }

    #region 辅助类和枚举

    /// <summary>
    /// 任务状态同步订阅者
    /// </summary>
    internal class TodoSyncSubscriber
    {
        /// <summary>
        /// 订阅者唯一标识符
        /// </summary>
        public Guid SubscriberKey { get; set; }

        /// <summary>
        /// 状态更新回调函数
        /// </summary>
        public Action<List<TodoUpdate>> UpdateCallback { get; set; }

        /// <summary>
        /// 感兴趣的业务类型列表
        /// 若为空，则接收所有业务类型的更新
        /// </summary>
        public List<BizType> BusinessTypes { get; set; }

        /// <summary>
        /// 在UI线程上调用回调函数
        /// </summary>
        /// <param name="updates">更新列表</param>
        public void InvokeCallback(List<TodoUpdate> updates)
        {
            if (UpdateCallback == null)
                return;

            // 如果当前线程是UI线程，直接执行
            if (Application.OpenForms.Count > 0 && Application.OpenForms[0].InvokeRequired)
            {
                Application.OpenForms[0].BeginInvoke(new Action(() =>
                {
                    try
                    {
                        UpdateCallback(updates);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"执行任务状态更新回调时出错: {ex.Message}");
                    }
                }));
            }
            else
            {
                // 否则直接执行
                UpdateCallback(updates);
            }
        }
    }
    
    #endregion
}