using RUINORERP.Model.ConfigModel;
using System;
using System.Threading.Tasks;

namespace RUINORERP.Business.Config
{
    /// <summary>
    /// 配置管理服务接口
    /// 提供配置文件的加载、保存和默认值创建功能
    /// </summary>
    public interface IConfigManagerService
    {
        /// <summary>
        /// 获取配置对象
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="configType">配置类型名称</param>
        /// <returns>配置对象</returns>
        T GetConfig<T>(string configType) where T : BaseConfig, new();

        /// <summary>
        /// 获取配置（无参数版本）
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <returns>配置对象</returns>
        T GetConfig<T>() where T : BaseConfig, new();

        /// <summary>
        /// 加载配置文件
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="configType">配置类型名称</param>
        /// <returns>配置对象</returns>
        T LoadConfig<T>(string configType) where T : BaseConfig, new();

        /// <summary>
        /// 加载配置（无参数版本）
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <returns>配置对象</returns>
        T LoadConfig<T>() where T : BaseConfig, new();

        /// <summary>
        /// 异步加载配置文件
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="configType">配置类型名称</param>
        /// <returns>配置对象</returns>
        Task<T> LoadConfigAsync<T>(string configType) where T : BaseConfig, new();

        /// <summary>
        /// 异步加载配置（无参数版本）
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <returns>配置对象</returns>
        Task<T> LoadConfigAsync<T>() where T : BaseConfig, new();

        /// <summary>
        /// 从指定路径加载配置
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="filePath">配置文件路径</param>
        /// <returns>配置对象</returns>
        T LoadConfigFromPath<T>(string filePath) where T : BaseConfig, new();

        /// <summary>
        /// 异步从指定路径加载配置
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="filePath">配置文件路径</param>
        /// <returns>配置对象</returns>
        Task<T> LoadConfigFromPathAsync<T>(string filePath) where T : BaseConfig, new();

        /// <summary>
        /// 从JSON字符串加载配置
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="jsonContent">JSON格式的配置内容</param>
        /// <returns>配置对象</returns>
        T LoadConfigFromJson<T>(string jsonContent) where T : BaseConfig, new();

        /// <summary>
        /// 异步从JSON字符串加载配置
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="jsonContent">JSON格式的配置内容</param>
        /// <returns>配置对象</returns>
        Task<T> LoadConfigFromJsonAsync<T>(string jsonContent) where T : BaseConfig, new();

        /// <summary>
        /// 刷新配置（重新从持久化存储加载）
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <returns>刷新后的配置对象</returns>
        T RefreshConfig<T>() where T : BaseConfig, new();

        /// <summary>
        /// 异步刷新配置（重新从持久化存储加载）
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <returns>刷新后的配置对象</returns>
        Task<T> RefreshConfigAsync<T>() where T : BaseConfig, new();

        /// <summary>
        /// 保存配置文件
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="config">配置对象</param>
        /// <param name="configType">配置类型名称</param>
        /// <returns>是否保存成功</returns>
        bool SaveConfig<T>(T config, string configType) where T : BaseConfig, new();

        /// <summary>
        /// 保存配置（无参数版本）
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="config">配置对象</param>
        /// <returns>是否保存成功</returns>
        bool SaveConfig<T>(T config) where T : BaseConfig, new();

        /// <summary>
        /// 异步保存配置文件
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="config">配置对象</param>
        /// <param name="configType">配置类型名称</param>
        /// <returns>是否保存成功</returns>
        Task<bool> SaveConfigAsync<T>(T config, string configType) where T : BaseConfig, new();

        /// <summary>
        /// 异步保存配置（无参数版本）
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="config">配置对象</param>
        /// <returns>是否保存成功</returns>
        Task<bool> SaveConfigAsync<T>(T config) where T : BaseConfig, new();

        /// <summary>
        /// 创建配置默认值
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <returns>默认配置对象</returns>
        T CreateDefaultConfig<T>() where T : BaseConfig, new();

        /// <summary>
        /// 检查配置文件是否存在
        /// </summary>
        /// <param name="configType">配置类型名称</param>
        /// <returns>是否存在</returns>
        bool ConfigFileExists(string configType);

        /// <summary>
        /// 获取配置文件路径
        /// </summary>
        /// <param name="configType">配置类型名称</param>
        /// <returns>文件路径</returns>
        string GetConfigFilePath(string configType);

        /// <summary>
        /// 重置配置为默认值
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="configType">配置类型名称</param>
        /// <returns>重置后的配置对象</returns>
        T ResetToDefault<T>(string configType) where T : BaseConfig, new();

        /// <summary>
        /// 重置为默认配置（无参数版本）
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <returns>重置后的默认配置对象</returns>
        T ResetToDefault<T>() where T : BaseConfig, new();
        
        /// <summary>
        /// 解析路径中的环境变量
        /// </summary>
        /// <param name="path">包含环境变量的路径</param>
        /// <returns>解析后的实际路径</returns>
        string ResolveEnvironmentVariables(string path);

        /// <summary>
        /// 注册配置变更事件处理器
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="handler">配置变更事件处理器</param>
        void RegisterConfigChangedHandler<T>(EventHandler<ConfigChangedEventArgs<T>> handler) where T : BaseConfig, new();

        /// <summary>
        /// 注销配置变更事件处理器
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="handler">配置变更事件处理器</param>
        void UnregisterConfigChangedHandler<T>(EventHandler<ConfigChangedEventArgs<T>> handler) where T : BaseConfig, new();
    }
}
