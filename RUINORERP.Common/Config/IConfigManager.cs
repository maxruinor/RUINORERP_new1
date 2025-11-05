using System;
using System.Threading.Tasks;

namespace RUINORERP.Common.Config
{
    /// <summary>
    /// 配置管理器接口（简化版）
    /// 提供基本的配置管理功能，包括加载、保存配置
    /// </summary>
    public interface IConfigManager
    {
        /// <summary>
        /// 初始化配置管理器
        /// 设置必要的依赖和状态
        /// </summary>
        void Initialize();
        
        /// <summary>
        /// 加载指定类型的配置
        /// 从文件中读取并反序列化为指定类型的配置对象
        /// </summary>
        /// <typeparam name="TConfig">配置对象类型</typeparam>
        /// <param name="configType">配置路径类型</param>
        /// <param name="configTypeName">配置类型名称，如为空则使用TConfig的名称</param>
        /// <returns>配置对象实例</returns>
        TConfig LoadConfig<TConfig>(ConfigPathType configType = ConfigPathType.Server, string configTypeName = null) where TConfig : class, new();
        
        /// <summary>
        /// 异步加载指定类型的配置
        /// 异步从文件中读取并反序列化为指定类型的配置对象
        /// </summary>
        /// <typeparam name="TConfig">配置对象类型</typeparam>
        /// <param name="configType">配置路径类型</param>
        /// <param name="configTypeName">配置类型名称，如为空则使用TConfig的名称</param>
        /// <returns>配置对象实例的任务</returns>
        Task<TConfig> LoadConfigAsync<TConfig>(ConfigPathType configType = ConfigPathType.Server, string configTypeName = null) where TConfig : class, new();
        
        /// <summary>
        /// 保存配置到文件
        /// 将配置对象序列化并保存到文件
        /// </summary>
        /// <typeparam name="TConfig">配置对象类型</typeparam>
        /// <param name="config">配置对象实例</param>
        /// <param name="configType">配置路径类型</param>
        /// <param name="configTypeName">配置类型名称，如为空则使用TConfig的名称</param>
        void SaveConfig<TConfig>(TConfig config, ConfigPathType configType = ConfigPathType.Server, string configTypeName = null) where TConfig : class;
        
        /// <summary>
        /// 异步保存配置到文件
        /// 异步将配置对象序列化并保存到文件
        /// </summary>
        /// <typeparam name="TConfig">配置对象类型</typeparam>
        /// <param name="config">配置对象实例</param>
        /// <param name="configType">配置路径类型</param>
        /// <param name="configTypeName">配置类型名称，如为空则使用TConfig的名称</param>
        /// <returns>保存任务</returns>
        Task SaveConfigAsync<TConfig>(TConfig config, ConfigPathType configType = ConfigPathType.Server, string configTypeName = null) where TConfig : class;
        
        /// <summary>
        /// 刷新配置
        /// 强制重新加载配置文件
        /// </summary>
        /// <typeparam name="TConfig">配置对象类型</typeparam>
        /// <param name="configType">配置路径类型</param>
        /// <param name="configTypeName">配置类型名称，如为空则使用TConfig的名称</param>
        /// <returns>刷新后的配置对象</returns>
        TConfig RefreshConfig<TConfig>(ConfigPathType configType = ConfigPathType.Server, string configTypeName = null) where TConfig : class, new();
        
        /// <summary>
        /// 异步刷新配置
        /// 异步强制重新加载配置文件
        /// </summary>
        /// <typeparam name="TConfig">配置对象类型</typeparam>
        /// <param name="configType">配置路径类型</param>
        /// <param name="configTypeName">配置类型名称，如为空则使用TConfig的名称</param>
        /// <returns>刷新后的配置对象的任务</returns>
        Task<TConfig> RefreshConfigAsync<TConfig>(ConfigPathType configType = ConfigPathType.Server, string configTypeName = null) where TConfig : class, new();
        
        /// <summary>
        /// 验证配置
        /// 检查配置对象的有效性
        /// </summary>
        /// <typeparam name="TConfig">配置对象类型</typeparam>
        /// <param name="config">配置对象实例</param>
        /// <returns>配置是否有效</returns>
        bool ValidateConfig<TConfig>(TConfig config) where TConfig : class;
    }
}