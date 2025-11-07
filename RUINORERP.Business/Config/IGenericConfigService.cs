using RUINORERP.Model.ConfigModel;
using System;
using System.Threading.Tasks;

namespace RUINORERP.Business.Config
{
    /// <summary>
    /// 通用配置服务接口
    /// 提供对特定类型配置的加载、保存、验证、刷新和事件通知等功能
    /// </summary>
    /// <typeparam name="T">配置类型，必须继承自BaseConfig且有公共无参构造函数</typeparam>
    public interface IGenericConfigService<T> where T : BaseConfig, new()
    {
        /// <summary>
        /// 配置变更事件
        /// 当配置被加载、保存、刷新或重置时触发
        /// </summary>
        event EventHandler<ConfigChangedEventArgs<T>> ConfigChanged;
        
        /// <summary>
        /// 加载配置
        /// </summary>
        /// <returns>配置对象</returns>
        T LoadConfig();
        
        /// <summary>
        /// 异步加载配置
        /// </summary>
        /// <returns>配置对象</returns>
        Task<T> LoadConfigAsync();
        
        /// <summary>
        /// 从指定路径加载配置
        /// </summary>
        /// <param name="filePath">配置文件路径</param>
        /// <returns>配置对象</returns>
        T LoadConfigFromPath(string filePath);
        
        /// <summary>
        /// 异步从指定路径加载配置
        /// </summary>
        /// <param name="filePath">配置文件路径</param>
        /// <returns>配置对象</returns>
        Task<T> LoadConfigFromPathAsync(string filePath);
        
        /// <summary>
        /// 从JSON字符串加载配置
        /// </summary>
        /// <param name="jsonContent">JSON格式的配置内容</param>
        /// <returns>配置对象</returns>
        T LoadConfigFromJson(string jsonContent);
        
        /// <summary>
        /// 异步从JSON字符串加载配置
        /// </summary>
        /// <param name="jsonContent">JSON格式的配置内容</param>
        /// <returns>配置对象</returns>
        Task<T> LoadConfigFromJsonAsync(string jsonContent);
        
        /// <summary>
        /// 刷新配置（重新从持久化存储加载）
        /// </summary>
        /// <returns>刷新后的配置对象</returns>
        T RefreshConfig();
        
        /// <summary>
        /// 异步刷新配置（重新从持久化存储加载）
        /// </summary>
        /// <returns>刷新后的配置对象</returns>
        Task<T> RefreshConfigAsync();
        
        /// <summary>
        /// 保存配置
        /// </summary>
        /// <param name="config">要保存的配置对象</param>
        /// <returns>是否保存成功</returns>
        bool SaveConfig(T config);
        
        /// <summary>
        /// 异步保存配置
        /// </summary>
        /// <param name="config">要保存的配置对象</param>
        /// <returns>是否保存成功</returns>
        Task<bool> SaveConfigAsync(T config);
        
        /// <summary>
        /// 创建默认配置
        /// </summary>
        /// <returns>默认配置对象</returns>
        T CreateDefaultConfig();
        
        /// <summary>
        /// 重置为默认配置
        /// </summary>
        /// <returns>重置后的默认配置对象</returns>
        T ResetToDefault();
        
        /// <summary>
        /// 验证配置
        /// </summary>
        /// <param name="config">要验证的配置对象</param>
        /// <returns>验证结果</returns>
        ConfigValidationResult ValidateConfig(T config);
        
        /// <summary>
        /// 获取配置文件路径
        /// </summary>
        /// <returns>配置文件路径</returns>
        string GetConfigFilePath();
        
        /// <summary>
        /// 获取当前内存中的配置对象
        /// </summary>
        T CurrentConfig { get; }
    }
}