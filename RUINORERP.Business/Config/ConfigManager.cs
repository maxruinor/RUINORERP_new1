using RUINORERP.Model.ConfigModel;
using System;

namespace RUINORERP.Business.Config
{
    /// <summary>
    /// 配置管理器静态类
    /// 提供全局访问配置的便捷方法
    /// </summary>
    public static class ConfigManager
    {
        private static IConfigManagerService _configManagerService;
        private static readonly object _lock = new object();

        /// <summary>
        /// 初始化配置管理器
        /// </summary>
        /// <param name="configManagerService">配置管理服务</param>
        public static void Initialize(IConfigManagerService configManagerService)
        {
            lock (_lock)
            {
                _configManagerService = configManagerService ?? throw new ArgumentNullException(nameof(configManagerService));
            }
        }

        /// <summary>
        /// 获取配置管理服务
        /// </summary>
        private static IConfigManagerService GetService()
        {
            if (_configManagerService == null)
            {
                // 提供明确的异常信息，指导用户正确初始化
                throw new InvalidOperationException("请先调用ConfigManager.Initialize()方法初始化配置服务");
            }
            return _configManagerService;
        }

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <returns>配置对象</returns>
        public static T GetConfig<T>() where T : BaseConfig, new()
        {
            return GetService().GetConfig<T>();
        }

        /// <summary>
        /// 获取系统全局配置
        /// </summary>
        /// <returns>系统全局配置对象</returns>
        public static SystemGlobalConfig GetSystemGlobalConfig()
        {
            return GetConfig<SystemGlobalConfig>();
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="config">配置对象</param>
        /// <returns>是否保存成功</returns>
        public static bool SaveConfig<T>(T config) where T : BaseConfig, new()
        {
            return GetService().SaveConfig(config);
        }

        /// <summary>
        /// 刷新配置
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <returns>刷新后的配置对象</returns>
        public static T RefreshConfig<T>() where T : BaseConfig, new()
        {
            return GetService().RefreshConfig<T>();
        }
    }
}