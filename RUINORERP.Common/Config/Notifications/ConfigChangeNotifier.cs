/*************************************************************
 * 文件名：ConfigChangeNotifier.cs
 * 作者：系统生成
 * 日期：2024-04-26
 * 描述：配置变更通知器
 * Copyright (c) 2024 RUINOR. All rights reserved.
 *************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace RUINORERP.Common.Config.Notifications
{
    /// <summary>
    /// 配置变更事件参数
    /// </summary>
    public class ConfigChangedEventArgs : EventArgs
    {
        /// <summary>
        /// 配置类型
        /// </summary>
        public Type ConfigType { get; set; }
        
        /// <summary>
        /// 配置键/路径
        /// </summary>
        public string ConfigKey { get; set; }
        
        /// <summary>
        /// 变更前的配置
        /// </summary>
        public object OldValue { get; set; }
        
        /// <summary>
        /// 变更后的配置
        /// </summary>
        public object NewValue { get; set; }
        
        /// <summary>
        /// 变更时间
        /// </summary>
        public DateTime ChangeTime { get; set; } = DateTime.UtcNow;
        
        /// <summary>
        /// 变更原因
        /// </summary>
        public string ChangeReason { get; set; }
        
        /// <summary>
        /// 变更发起者标识
        /// </summary>
        public string ChangedBy { get; set; }
        
        /// <summary>
        /// 是否为本地变更（相对于分布式环境）
        /// </summary>
        public bool IsLocalChange { get; set; } = true;
    }

    /// <summary>
    /// 配置变更监听接口
    /// </summary>
    public interface IConfigChangeListener
    {
        /// <summary>
        /// 处理配置变更事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="args">配置变更事件参数</param>
        Task OnConfigChangedAsync(object sender, ConfigChangedEventArgs args);
        
        /// <summary>
        /// 获取监听的配置类型
        /// </summary>
        /// <returns>监听的配置类型集合</returns>
        IEnumerable<Type> GetListeningTypes();
    }

    /// <summary>
    /// 通用配置变更监听器
    /// 可以通过委托处理配置变更
    /// </summary>
    public class GenericConfigChangeListener : IConfigChangeListener
    {
        /// <summary>
        /// 变更处理委托
        /// </summary>
        private readonly Func<object, ConfigChangedEventArgs, Task> _handler;
        
        /// <summary>
        /// 监听的配置类型
        /// </summary>
        private readonly IEnumerable<Type> _listeningTypes;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="handler">变更处理委托</param>
        /// <param name="listeningTypes">监听的配置类型</param>
        public GenericConfigChangeListener(Func<object, ConfigChangedEventArgs, Task> handler, params Type[] listeningTypes)
        {
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
            _listeningTypes = listeningTypes ?? throw new ArgumentNullException(nameof(listeningTypes));
        }

        /// <summary>
        /// 处理配置变更事件
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="args">配置变更事件参数</param>
        public Task OnConfigChangedAsync(object sender, ConfigChangedEventArgs args)
        {
            return _handler(sender, args);
        }

        /// <summary>
        /// 获取监听的配置类型
        /// </summary>
        /// <returns>监听的配置类型集合</returns>
        public IEnumerable<Type> GetListeningTypes()
        {
            return _listeningTypes;
        }
    }

    /// <summary>
    /// 配置变更通知选项
    /// </summary>
    public class ConfigChangeNotifierOptions
    {
        /// <summary>
        /// 是否启用配置变更通知
        /// 默认true
        /// </summary>
        public bool Enabled { get; set; } = true;
        
        /// <summary>
        /// 是否在本地通知完成后发布到分布式环境
        /// 默认false，需要额外实现分布式通知
        /// </summary>
        public bool NotifyDistributed { get; set; } = false;
        
        /// <summary>
        /// 是否在通知失败时抛出异常
        /// 默认false，失败时记录日志但不中断流程
        /// </summary>
        public bool ThrowOnNotificationFailure { get; set; } = false;
    }

    /// <summary>
    /// 配置变更通知器接口
    /// 定义配置变更通知的基本操作
    /// </summary>
    public interface IConfigChangeNotifier
    {
        /// <summary>
        /// 注册配置变更监听器
        /// </summary>
        /// <param name="listener">配置变更监听器</param>
        void RegisterListener(IConfigChangeListener listener);
        
        /// <summary>
        /// 注销配置变更监听器
        /// </summary>
        /// <param name="listener">配置变更监听器</param>
        void UnregisterListener(IConfigChangeListener listener);
        
        /// <summary>
        /// 通知配置变更
        /// </summary>
        /// <typeparam name="TConfig">配置类型</typeparam>
        /// <param name="configKey">配置键/路径</param>
        /// <param name="oldValue">变更前的配置</param>
        /// <param name="newValue">变更后的配置</param>
        /// <param name="changeReason">变更原因</param>
        /// <param name="changedBy">变更发起者标识</param>
        /// <returns>异步任务</returns>
        Task NotifyConfigChangedAsync<TConfig>(string configKey, TConfig oldValue, TConfig newValue, string changeReason = null, string changedBy = null);
        
        /// <summary>
        /// 发布配置变更到分布式环境
        /// 注：此方法需要在分布式环境中实现
        /// </summary>
        /// <param name="args">配置变更事件参数</param>
        /// <returns>异步任务</returns>
        Task PublishToDistributedAsync(ConfigChangedEventArgs args);
    }

    /// <summary>
    /// 默认配置变更通知器
    /// 实现本地配置变更通知
    /// </summary>
    public class DefaultConfigChangeNotifier : IConfigChangeNotifier
    {
        /// <summary>
        /// 日志记录器
        /// </summary>
        protected readonly ILogger<DefaultConfigChangeNotifier> _logger;
        
        /// <summary>
        /// 配置选项
        /// </summary>
        protected readonly ConfigChangeNotifierOptions _options;
        
        /// <summary>
        /// 配置变更监听器字典
        /// 键：配置类型，值：监听器列表
        /// </summary>
        protected readonly Dictionary<Type, List<IConfigChangeListener>> _listeners;
        
        /// <summary>
        /// 全局监听器列表（监听所有配置类型变更）
        /// </summary>
        protected readonly List<IConfigChangeListener> _globalListeners;
        
        /// <summary>
        /// 线程锁，用于同步监听器的注册和注销
        /// </summary>
        protected readonly object _lockObj = new object();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        /// <param name="options">配置选项</param>
        public DefaultConfigChangeNotifier(
            ILogger<DefaultConfigChangeNotifier> logger,
            IOptions<ConfigChangeNotifierOptions> options = null)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _options = options?.Value ?? new ConfigChangeNotifierOptions();
            _listeners = new Dictionary<Type, List<IConfigChangeListener>>();
            _globalListeners = new List<IConfigChangeListener>();
            
            _logger.LogInformation("配置变更通知器初始化完成，启用状态: {Enabled}", _options.Enabled);
        }

        /// <summary>
        /// 注册配置变更监听器
        /// </summary>
        /// <param name="listener">配置变更监听器</param>
        public void RegisterListener(IConfigChangeListener listener)
        {
            if (!_options.Enabled)
            {
                _logger.LogDebug("配置变更通知已禁用，跳过注册监听器");
                return;
            }
            
            if (listener == null)
                throw new ArgumentNullException(nameof(listener));
            
            lock (_lockObj)
            {
                var listeningTypes = listener.GetListeningTypes();
                
                if (listeningTypes == null || !listeningTypes.Any())
                {
                    // 如果没有指定监听类型，则作为全局监听器
                    if (!_globalListeners.Contains(listener))
                    {
                        _globalListeners.Add(listener);
                        _logger.LogDebug("已注册全局配置变更监听器");
                    }
                }
                else
                {
                    // 为每种监听类型注册监听器
                    foreach (var type in listeningTypes)
                    {
                        if (!_listeners.ContainsKey(type))
                        {
                            _listeners[type] = new List<IConfigChangeListener>();
                        }
                        
                        if (!_listeners[type].Contains(listener))
                        {
                            _listeners[type].Add(listener);
                            _logger.LogDebug("已注册配置变更监听器，监听类型: {Type}", type.FullName);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 注销配置变更监听器
        /// </summary>
        /// <param name="listener">配置变更监听器</param>
        public void UnregisterListener(IConfigChangeListener listener)
        {
            if (!_options.Enabled)
            {
                _logger.LogDebug("配置变更通知已禁用，跳过注销监听器");
                return;
            }
            
            if (listener == null)
                throw new ArgumentNullException(nameof(listener));
            
            lock (_lockObj)
            {
                // 从全局监听器中移除
                if (_globalListeners.Contains(listener))
                {
                    _globalListeners.Remove(listener);
                    _logger.LogDebug("已注销全局配置变更监听器");
                }
                
                // 从特定类型监听器中移除
                var listeningTypes = listener.GetListeningTypes();
                if (listeningTypes != null && listeningTypes.Any())
                {
                    foreach (var type in listeningTypes)
                    {
                        if (_listeners.ContainsKey(type) && _listeners[type].Contains(listener))
                        {
                            _listeners[type].Remove(listener);
                            
                            // 如果类型没有监听器了，移除该类型
                            if (_listeners[type].Count == 0)
                            {
                                _listeners.Remove(type);
                            }
                            
                            _logger.LogDebug("已注销配置变更监听器，监听类型: {Type}", type.FullName);
                        }
                    }
                }
                else
                {
                    // 如果没有指定监听类型，则从所有类型中移除
                    foreach (var type in _listeners.Keys.ToList())
                    {
                        if (_listeners[type].Contains(listener))
                        {
                            _listeners[type].Remove(listener);
                            
                            if (_listeners[type].Count == 0)
                            {
                                _listeners.Remove(type);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 通知配置变更
        /// </summary>
        /// <typeparam name="TConfig">配置类型</typeparam>
        /// <param name="configKey">配置键/路径</param>
        /// <param name="oldValue">变更前的配置</param>
        /// <param name="newValue">变更后的配置</param>
        /// <param name="changeReason">变更原因</param>
        /// <param name="changedBy">变更发起者标识</param>
        /// <returns>异步任务</returns>
        public async Task NotifyConfigChangedAsync<TConfig>(string configKey, TConfig oldValue, TConfig newValue, string changeReason = null, string changedBy = null)
        {
            if (!_options.Enabled)
            {
                _logger.LogDebug("配置变更通知已禁用，跳过通知");
                return;
            }
            
            var configType = typeof(TConfig);
            
            // 创建变更事件参数
            var args = new ConfigChangedEventArgs
            {
                ConfigType = configType,
                ConfigKey = configKey,
                OldValue = oldValue,
                NewValue = newValue,
                ChangeReason = changeReason,
                ChangedBy = changedBy,
                IsLocalChange = true
            };
            
            _logger.LogInformation("通知配置变更: 类型={Type}, 键={Key}", configType.FullName, configKey);
            
            // 获取需要通知的监听器
            List<IConfigChangeListener> listenersToNotify = new List<IConfigChangeListener>();
            
            lock (_lockObj)
            {
                // 添加特定类型的监听器
                if (_listeners.ContainsKey(configType))
                {
                    listenersToNotify.AddRange(_listeners[configType]);
                }
                
                // 添加全局监听器
                listenersToNotify.AddRange(_globalListeners);
            }
            
            // 通知所有相关监听器
            var notificationTasks = listenersToNotify.Select(listener => 
                NotifyListenerAsync(listener, args)).ToList();
            
            await Task.WhenAll(notificationTasks);
            
            // 如果需要分布式通知
            if (_options.NotifyDistributed)
            {
                try
                {
                    await PublishToDistributedAsync(args);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "发布配置变更到分布式环境失败");
                    if (_options.ThrowOnNotificationFailure)
                    {
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// 发布配置变更到分布式环境
        /// 基础实现仅记录日志，需要在分布式环境中扩展
        /// </summary>
        /// <param name="args">配置变更事件参数</param>
        /// <returns>异步任务</returns>
        public virtual Task PublishToDistributedAsync(ConfigChangedEventArgs args)
        {
            _logger.LogWarning("基础配置变更通知器不支持分布式通知，请实现自定义的分布式通知器");
            return Task.CompletedTask;
        }

        /// <summary>
        /// 通知单个监听器
        /// </summary>
        /// <param name="listener">配置变更监听器</param>
        /// <param name="args">配置变更事件参数</param>
        /// <returns>异步任务</returns>
        protected async Task NotifyListenerAsync(IConfigChangeListener listener, ConfigChangedEventArgs args)
        {
            try
            {
                _logger.LogDebug("通知监听器配置变更: 监听器={Listener}, 类型={Type}", 
                    listener.GetType().FullName, args.ConfigType.FullName);
                
                await listener.OnConfigChangedAsync(this, args);
                
                _logger.LogDebug("监听器通知完成: {Listener}", listener.GetType().FullName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "通知监听器配置变更失败: 监听器={Listener}, 类型={Type}", 
                    listener.GetType().FullName, args.ConfigType.FullName);
                
                if (_options.ThrowOnNotificationFailure)
                {
                    throw;
                }
            }
        }
    }

    /// <summary>
    /// 无操作配置变更通知器
    /// 用于禁用配置变更通知的场景
    /// </summary>
    public class NoOpConfigChangeNotifier : IConfigChangeNotifier
    {
        /// <summary>
        /// 注册配置变更监听器
        /// 无操作实现
        /// </summary>
        /// <param name="listener">配置变更监听器</param>
        public void RegisterListener(IConfigChangeListener listener)
        {
            // 无操作
        }

        /// <summary>
        /// 注销配置变更监听器
        /// 无操作实现
        /// </summary>
        /// <param name="listener">配置变更监听器</param>
        public void UnregisterListener(IConfigChangeListener listener)
        {
            // 无操作
        }

        /// <summary>
        /// 通知配置变更
        /// 无操作实现
        /// </summary>
        /// <typeparam name="TConfig">配置类型</typeparam>
        /// <param name="configKey">配置键/路径</param>
        /// <param name="oldValue">变更前的配置</param>
        /// <param name="newValue">变更后的配置</param>
        /// <param name="changeReason">变更原因</param>
        /// <param name="changedBy">变更发起者标识</param>
        /// <returns>完成的任务</returns>
        public Task NotifyConfigChangedAsync<TConfig>(string configKey, TConfig oldValue, TConfig newValue, string changeReason = null, string changedBy = null)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// 发布配置变更到分布式环境
        /// 无操作实现
        /// </summary>
        /// <param name="args">配置变更事件参数</param>
        /// <returns>完成的任务</returns>
        public Task PublishToDistributedAsync(ConfigChangedEventArgs args)
        {
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// 配置变更通知扩展方法
    /// </summary>
    public static class ConfigChangeExtensions
    {
        /// <summary>
        /// 添加配置变更通知服务到依赖注入容器
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="configureOptions">配置选项</param>
        /// <returns>服务集合</returns>
        public static IServiceCollection AddConfigChangeNotifications(this IServiceCollection services, Action<ConfigChangeNotifierOptions> configureOptions = null)
        {
            // 配置通知选项
            services.Configure(configureOptions ?? (options => { }));
            
            // 添加配置变更通知器
            services.AddSingleton<IConfigChangeNotifier, DefaultConfigChangeNotifier>();
            
            return services;
        }
        
        /// <summary>
        /// 添加无操作配置变更通知服务（禁用通知）
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <returns>服务集合</returns>
        public static IServiceCollection AddNoOpConfigChangeNotifications(this IServiceCollection services)
        {
            services.AddSingleton<IConfigChangeNotifier, NoOpConfigChangeNotifier>();
            return services;
        }
        
        /// <summary>
        /// 注册通用配置变更监听器
        /// </summary>
        /// <typeparam name="TConfig">配置类型</typeparam>
        /// <param name="notifier">配置变更通知器</param>
        /// <param name="handler">变更处理委托</param>
        /// <returns>配置变更监听器实例，可以用于后续注销</returns>
        public static IConfigChangeListener RegisterListener<TConfig>(this IConfigChangeNotifier notifier, Func<object, ConfigChangedEventArgs, Task> handler)
        {
            if (notifier == null)
                throw new ArgumentNullException(nameof(notifier));
            
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));
            
            var listener = new GenericConfigChangeListener(handler, typeof(TConfig));
            notifier.RegisterListener(listener);
            
            return listener;
        }
        
        /// <summary>
        /// 注册同步配置变更监听器
        /// </summary>
        /// <typeparam name="TConfig">配置类型</typeparam>
        /// <param name="notifier">配置变更通知器</param>
        /// <param name="handler">同步变更处理委托</param>
        /// <returns>配置变更监听器实例，可以用于后续注销</returns>
        public static IConfigChangeListener RegisterSyncListener<TConfig>(this IConfigChangeNotifier notifier, Action<object, ConfigChangedEventArgs> handler)
        {
            if (notifier == null)
                throw new ArgumentNullException(nameof(notifier));
            
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));
            
            // 将同步处理委托包装为异步
            Func<object, ConfigChangedEventArgs, Task> asyncHandler = (sender, args) => 
            {
                handler(sender, args);
                return Task.CompletedTask;
            };
            
            return notifier.RegisterListener<TConfig>(asyncHandler);
        }
    }
}