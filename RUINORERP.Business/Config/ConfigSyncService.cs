using RUINORERP.Model.ConfigModel;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace RUINORERP.Business.Config
{
    /// <summary>
    /// 配置同步服务
    /// 管理配置实例的更新，确保DI容器中的单例实例与最新配置保持同步
    /// </summary>
    public class ConfigSyncService : IConfigSyncService
    {
        private readonly ILogger<ConfigSyncService> _logger;
        private readonly Dictionary<Type, object> _configInstances = new Dictionary<Type, object>();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger">日志记录器</param>
        public ConfigSyncService(ILogger<ConfigSyncService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 注册配置实例
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="instance">配置实例</param>
        public void RegisterConfigInstance<T>(T instance) where T : BaseConfig
        {
            _configInstances[typeof(T)] = instance;
            _logger.LogDebug("已注册配置实例: {ConfigType}", typeof(T).Name);
        }

        /// <summary>
        /// 获取配置实例
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <returns>配置实例</returns>
        public T GetConfigInstance<T>() where T : BaseConfig
        {
            if (_configInstances.TryGetValue(typeof(T), out var instance))
            {
                return (T)instance;
            }

            _logger.LogWarning("未找到配置实例: {ConfigType}", typeof(T).Name);
            return null;
        }

        /// <summary>
        /// 更新配置实例
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="newInstance">新的配置实例</param>
        public void UpdateConfigInstance<T>(T newInstance) where T : BaseConfig
        {
            if (newInstance == null)
            {
                _logger.LogWarning("更新配置实例失败: 新实例为空");
                return;
            }

            _configInstances[typeof(T)] = newInstance;
            _logger.LogDebug("已更新配置实例: {ConfigType}", typeof(T).Name);
        }

        /// <summary>
        /// 处理配置变更事件
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">配置变更事件参数</param>
        public void HandleConfigChanged<T>(object sender, ConfigChangedEventArgs<T> e) where T : BaseConfig, new()
        {
            if (e?.NewConfig == null)
            {
                _logger.LogWarning("处理配置变更事件失败: 新配置为空");
                return;
            }

            UpdateConfigInstance(e.NewConfig);
        }
    }

    /// <summary>
    /// 配置同步服务接口
    /// </summary>
    public interface IConfigSyncService
    {
        /// <summary>
        /// 注册配置实例
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="instance">配置实例</param>
        void RegisterConfigInstance<T>(T instance) where T : BaseConfig;

        /// <summary>
        /// 获取配置实例
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <returns>配置实例</returns>
        T GetConfigInstance<T>() where T : BaseConfig;

        /// <summary>
        /// 更新配置实例
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="newInstance">新的配置实例</param>
        void UpdateConfigInstance<T>(T newInstance) where T : BaseConfig;

        /// <summary>
        /// 处理配置变更事件
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">配置变更事件参数</param>
        void HandleConfigChanged<T>(object sender, ConfigChangedEventArgs<T> e) where T : BaseConfig, new();
    }
}