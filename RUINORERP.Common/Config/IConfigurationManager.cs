/*************************************************************
 * 文件名：IConfigurationManager.cs
 * 作者：系统生成
 * 日期：2024-04-26
 * 描述：泛型配置管理器接口，定义配置管理的核心功能
 * Copyright (c) 2024 RUINOR. All rights reserved.
 *************************************************************/

using System;
using System.Threading.Tasks;

namespace RUINORERP.Common.Config
{
    /// <summary>
    /// 配置管理器核心接口
    /// 定义所有配置操作的标准接口，支持泛型配置类型
    /// 采用黑盒处理模式，统一管理任意类型的配置
    /// </summary>
    /// <typeparam name="TConfig">配置类型</typeparam>
    public interface IConfigurationManager<TConfig> : IDisposable where TConfig : class, new()
    {
        /// <summary>
        /// 当前配置实例
        /// </summary>
        TConfig CurrentConfig { get; }

        /// <summary>
        /// 配置变更事件
        /// </summary>
        event EventHandler<ConfigurationChangedEventArgs<TConfig>> ConfigurationChanged;

        /// <summary>
        /// 加载配置（同步方法，已过时，请使用异步方法）
        /// </summary>
        /// <returns>配置是否加载成功</returns>
        bool Load();

        /// <summary>
        /// 加载配置（同步方法，已过时，请使用异步方法）
        /// </summary>
        /// <param name="pathType">配置路径类型</param>
        /// <returns>配置是否加载成功</returns>
        bool Load(ConfigPathType pathType);

        /// <summary>
        /// 保存配置（同步方法，已过时，请使用异步方法）
        /// </summary>
        /// <returns>配置是否保存成功</returns>
        bool Save();

        /// <summary>
        /// 保存配置（同步方法，已过时，请使用异步方法）
        /// </summary>
        /// <param name="config">要保存的配置</param>
        /// <returns>配置是否保存成功</returns>
        bool Save(TConfig config);

        /// <summary>
        /// 保存配置（同步方法，已过时，请使用异步方法）
        /// </summary>
        /// <param name="config">要保存的配置</param>
        /// <param name="pathType">配置路径类型</param>
        /// <returns>配置是否保存成功</returns>
        bool Save(TConfig config, ConfigPathType pathType);

        /// <summary>
        /// 刷新配置（同步方法，已过时，请使用异步方法）
        /// </summary>
        /// <returns>配置是否刷新成功</returns>
        bool Refresh();

        /// <summary>
        /// 刷新配置（同步方法，已过时，请使用异步方法）
        /// </summary>
        /// <param name="pathType">配置路径类型</param>
        /// <returns>配置是否刷新成功</returns>
        bool Refresh(ConfigPathType pathType);

        /// <summary>
        /// 重置配置为默认值（同步方法，已过时，请使用异步方法）
        /// </summary>
        /// <returns>配置是否重置成功</returns>
        bool Reset();
        
        #region 异步方法支持
        
        /// <summary>
        /// 获取当前配置
        /// </summary>
        TConfig GetCurrentConfig();
        
        /// <summary>
        /// 异步加载配置
        /// </summary>
        /// <param name="pathType">配置存储路径类型</param>
        /// <returns>加载的配置对象</returns>
        Task<TConfig> LoadConfigAsync(ConfigPathType pathType = ConfigPathType.Server);
        
        /// <summary>
        /// 异步保存配置
        /// </summary>
        /// <param name="config">要保存的配置对象</param>
        /// <param name="pathType">配置存储路径类型</param>
        /// <returns>保存是否成功</returns>
        Task<bool> SaveConfigAsync(TConfig config, ConfigPathType pathType = ConfigPathType.Server);
        
        /// <summary>
        /// 异步刷新配置
        /// </summary>
        /// <param name="pathType">配置存储路径类型</param>
        /// <returns>刷新是否成功</returns>
        Task<bool> RefreshAsync(ConfigPathType pathType = ConfigPathType.Server);
        
        /// <summary>
        /// 创建默认配置
        /// </summary>
        /// <returns>默认配置对象</returns>
        TConfig CreateDefaultConfig();
        
        /// <summary>
        /// 异步创建默认配置并保存
        /// </summary>
        /// <returns>默认配置对象</returns>
        Task<TConfig> CreateDefaultConfigAsync();
        
        /// <summary>
        /// 订阅配置变更
        /// </summary>
        /// <param name="onChange">配置变更回调函数</param>
        /// <returns>用于取消订阅的IDisposable对象</returns>
        IDisposable Subscribe(Action<TConfig> onChange);
        
        /// <summary>
        /// 检查配置文件是否存在
        /// </summary>
        /// <param name="pathType">配置存储路径类型</param>
        /// <returns>配置文件是否存在</returns>
        bool ConfigExists(ConfigPathType pathType = ConfigPathType.Server);
        
        /// <summary>
        /// 获取配置文件路径
        /// </summary>
        /// <param name="pathType">配置存储路径类型</param>
        /// <returns>配置文件完整路径</returns>
        string GetConfigFilePath(ConfigPathType pathType = ConfigPathType.Server);
        
        /// <summary>
        /// 通知配置已变更
        /// </summary>
        void NotifyConfigChanged();
        
        /// <summary>
        /// 设置配置类型名称
        /// </summary>
        /// <param name="configTypeName">自定义配置类型名称</param>
        void SetConfigTypeName(string configTypeName);
        
        #endregion
    }
    
    /// <summary>
    /// 配置初始化接口
    /// 支持配置类在创建后进行自定义初始化
    /// </summary>
    public interface IConfigInitializable
    {
        /// <summary>
        /// 初始化配置
        /// 设置配置的默认值和初始状态
        /// </summary>
        void Initialize();
    }
}