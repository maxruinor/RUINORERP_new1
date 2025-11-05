/*************************************************************
 * 文件名：IConfigSyncService.cs
 * 作者：系统生成
 * 日期：2024-04-26
 * 描述：配置同步服务接口，定义配置同步的核心功能
 * Copyright (c) 2024 RUINOR. All rights reserved.
 *************************************************************/

using System.Threading.Tasks;

namespace RUINORERP.Common.Config
{
    /// <summary>
    /// 配置同步服务接口，负责处理配置的同步逻辑
    /// </summary>
    public interface IConfigSyncService
    {
        /// <summary>
        /// 发布配置变更到所有客户端
        /// </summary>
        /// <typeparam name="TConfig">配置类型</typeparam>
        /// <param name="config">配置实例</param>
        /// <param name="forceApply">是否强制应用配置</param>
        /// <returns>是否发布成功</returns>
        Task<bool> PublishConfigChangeAsync<TConfig>(TConfig config, bool forceApply = true) where TConfig : class;

        /// <summary>
        /// 应用从服务端接收的配置变更
        /// </summary>
        /// <param name="configSyncData">配置同步数据</param>
        /// <returns>是否应用成功</returns>
        Task<bool> ApplyConfigSyncAsync(ConfigSyncData configSyncData);

        /// <summary>
        /// 向服务端请求最新配置
        /// </summary>
        /// <typeparam name="TConfig">配置类型</typeparam>
        /// <returns>配置实例，如果请求失败则返回null</returns>
        Task<TConfig> RequestLatestConfigAsync<TConfig>() where TConfig : class, new();

        /// <summary>
        /// 向服务端请求所有配置
        /// </summary>
        /// <returns>是否请求成功</returns>
        Task<bool> RequestAllConfigsAsync();
    }

    /// <summary>
    /// 配置同步数据结构
    /// </summary>
    public class ConfigSyncData
    {
        /// <summary>
        /// 配置类型名称
        /// </summary>
        public string ConfigType { get; set; }

        /// <summary>
        /// 配置数据的JSON表示
        /// </summary>
        public string ConfigData { get; set; }

        /// <summary>
        /// 配置版本
        /// </summary>
        public long Version { get; set; }

        /// <summary>
        /// 是否强制应用配置
        /// </summary>
        public bool ForceApply { get; set; }
    }
}