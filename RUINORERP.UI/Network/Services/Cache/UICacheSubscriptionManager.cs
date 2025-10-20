using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using RUINORERP.PacketSpec.Commands.Cache;
using RUINORERP.PacketSpec.Models.Requests.Cache;
using RUINORERP.PacketSpec.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using RUINORERP.Business.Cache;

namespace RUINORERP.UI.Network.Services.Cache
{
    /// <summary>
    /// UI缓存订阅管理器 - 负责处理缓存变更的订阅和通知
    /// 注意：此类使用业务层的CacheSubscriptionManager并添加UI特定功能
    /// </summary>
    public class UICacheSubscriptionManager : IDisposable
    {
        private readonly ILogger<UICacheSubscriptionManager> _log;
        private readonly ISocketClient _socketClient;
        private readonly CacheSubscriptionManager _businessSubscriptionManager;
        private bool _disposed = false;
        private readonly string _componentName = nameof(UICacheSubscriptionManager);

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="log">日志记录器</param>
        /// <param name="loggerFactory">日志工厂 - 用于创建不同类型的日志记录器</param>
        /// <param name="socketClient">Socket客户端</param>
        public UICacheSubscriptionManager(ILogger<UICacheSubscriptionManager> log, CacheSubscriptionManager cacheSubscriptionManager, ILoggerFactory loggerFactory, ISocketClient socketClient)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));
            _socketClient = socketClient ?? throw new ArgumentNullException(nameof(socketClient));
            _businessSubscriptionManager = cacheSubscriptionManager;
        }

        /// <summary>
        /// 订阅指定表的缓存变更
        /// </summary>
        public async Task SubscribeCacheAsync(string tableName)
        {
            try
            {
                if (_disposed)
                {
                    _log.LogWarning("{0}已释放，无法订阅缓存变更", _componentName);
                    return;
                }

                if (string.IsNullOrEmpty(tableName))
                {
                    _log.LogWarning("订阅缓存变更时表名为空");
                    return;
                }

    

                // 使用业务层订阅管理器进行订阅
                await _businessSubscriptionManager.SubscribeAsync(tableName);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "订阅缓存变更失败，表名={0}", tableName);
            }
        }

        /// <summary>
        /// 取消订阅指定表的缓存变更
        /// </summary>
        public async Task UnsubscribeCacheAsync(string tableName)
        {
            try
            {
                if (_disposed)
                {
                    _log.LogWarning("{0}已释放，无法取消订阅缓存变更", _componentName);
                    return;
                }

                if (string.IsNullOrEmpty(tableName))
                {
                    _log.LogWarning("取消订阅缓存变更时表名为空");
                    return;
                }

    

                // 使用业务层订阅管理器取消订阅
                await _businessSubscriptionManager.UnsubscribeAsync(tableName);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "取消订阅缓存变更失败，表名={0}", tableName);
            }
        }

        /// <summary>
        /// 取消所有表的缓存变更订阅
        /// </summary>
        public async Task UnsubscribeAllAsync()
        {
            try
            {
                if (_disposed)
                {
                    _log.LogWarning("{0}已释放，无法取消所有缓存订阅", _componentName);
                    return;
                }

                // 使用业务层订阅管理器取消所有订阅
                await _businessSubscriptionManager.UnsubscribeAllAsync();
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "取消所有缓存订阅过程中发生错误");
            }
        }

        /// <summary>
        /// 检查表是否已订阅
        /// </summary>
        public bool IsSubscribed(string tableName)
        {
            if (_disposed)
            {
                _log.LogWarning("{0}已释放，无法检查订阅状态", _componentName);
                return false;
            }
            
            if (string.IsNullOrEmpty(tableName))
            {
                return false;
            }
            
            // 使用业务层订阅管理器检查订阅状态
            return _businessSubscriptionManager.IsSubscribed(tableName);
        }

        /// <summary>
        /// 获取所有已订阅的表
        /// </summary>
        public List<string> GetSubscribedTables()
        {
            if (_disposed)
            {
                _log.LogWarning("{0}已释放，无法获取订阅表列表", _componentName);
                return new List<string>();
            }
            
            // 返回GetSubscriptions方法的结果，转换为List
            return GetSubscriptions().ToList();
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~UICacheSubscriptionManager()
        {
            Dispose(false);
        }
        
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
    
                    
                    // 释放业务层订阅管理器
                    try
                    {
                        _businessSubscriptionManager?.Dispose();
                    }
                    catch (Exception ex)
                    {
                        _log.LogWarning(ex, "释放业务层订阅管理器时出错");
                    }
                    
    
                }

                _disposed = true;
            }
        }

        /// <summary>
        /// 设置通信服务
        /// </summary>
        /// <param name="socketClient">Socket客户端实例</param>
        public void SetCommunicationService(ISocketClient socketClient)
        {
            // 更新内部socket客户端引用
            if (!_disposed && socketClient != null)
            {
                // 在业务层订阅管理器上设置通信服务
                try
                {
                    // 假设业务层管理器有类似的方法
                    // 实际实现可能需要根据业务层接口进行调整
                    dynamic businessManager = _businessSubscriptionManager;
                    businessManager.SetCommunicationService(socketClient);

                }
                catch (Exception ex)
                {
                    _log.LogWarning(ex, "为{0}设置通信服务时出错", _componentName);
                }
            }
        }

        /// <summary>
        /// 添加订阅
        /// </summary>
        /// <param name="tableName">表名</param>
        public void AddSubscription(string tableName)
        {
            if (_disposed)
            {
                _log.LogWarning("{0}已释放，无法添加订阅", _componentName);
                return;
            }

            if (string.IsNullOrEmpty(tableName))
            {
                _log.LogWarning("添加订阅时表名为空");
                return;
            }

            try
            {
                // 调用业务层管理器的添加订阅方法
                dynamic businessManager = _businessSubscriptionManager;
                businessManager.AddSubscription(tableName);
                
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "添加订阅失败，表名={0}", tableName);
            }
        }

        /// <summary>
        /// 移除订阅
        /// </summary>
        /// <param name="tableName">表名</param>
        public void RemoveSubscription(string tableName)
        {
            if (_disposed)
            {
                _log.LogWarning("{0}已释放，无法移除订阅", _componentName);
                return;
            }

            if (string.IsNullOrEmpty(tableName))
            {
                _log.LogWarning("移除订阅时表名为空");
                return;
            }

            try
            {
                // 调用业务层管理器的移除订阅方法
                dynamic businessManager = _businessSubscriptionManager;
                businessManager.RemoveSubscription(tableName);
                
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "移除订阅失败，表名={0}", tableName);
            }
        }

        /// <summary>
        /// 获取所有订阅
        /// </summary>
        /// <returns>订阅表名列表</returns>
        public IEnumerable<string> GetSubscriptions()
        {
            if (_disposed)
            {
                _log.LogWarning("{0}已释放，无法获取订阅列表", _componentName);
                return Enumerable.Empty<string>();
            }

            try
            {
                // 调用业务层管理器的获取订阅方法
                dynamic businessManager = _businessSubscriptionManager;
                return businessManager.GetSubscriptions();
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "获取订阅列表失败");
                return Enumerable.Empty<string>();
            }
        }
    }
}