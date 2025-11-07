using RUINORERP.Model.ConfigModel;
using System;
using System.Threading.Tasks;

namespace RUINORERP.IServices.Config
{
    /// <summary>
    /// 配置管理服务接口
    /// 提供对不同类型配置的统一访问入口，支持配置刷新、事件通知和灵活加载
    /// </summary>
    public interface IConfigManagerService
    {
        /// <summary>
        /// 获取配置
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <returns>配置对象</returns>
        T GetConfig<T>() where T : BaseConfig, new();

        /// <summary>
        /// 加载配置
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <returns>配置对象</returns>
        T LoadConfig<T>() where T : BaseConfig, new();

        /// <summary>
        /// 异步加载配置
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
        /// 保存配置
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="config">配置对象</param>
        /// <returns>是否保存成功</returns>
        bool SaveConfig<T>(T config) where T : BaseConfig, new();

        /// <summary>
        /// 异步保存配置
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="config">配置对象</param>
        /// <returns>是否保存成功</returns>
        Task<bool> SaveConfigAsync<T>(T config) where T : BaseConfig, new();

        /// <summary>
        /// 创建默认配置
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <returns>默认配置对象</returns>
        T CreateDefaultConfig<T>() where T : BaseConfig, new();

        /// <summary>
        /// 重置为默认配置
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <returns>重置后的默认配置对象</returns>
        T ResetToDefault<T>() where T : BaseConfig, new();

        /// <summary>
        /// 解析环境变量
        /// </summary>
        /// <param name="path">包含环境变量的路径字符串</param>
        /// <returns>解析后的路径</returns>
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