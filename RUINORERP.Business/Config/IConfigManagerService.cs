using RUINORERP.Model.ConfigModel;
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
        /// 加载配置文件
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="configType">配置类型名称</param>
        /// <returns>配置对象</returns>
        T LoadConfig<T>(string configType) where T : BaseConfig, new();

        /// <summary>
        /// 异步加载配置文件
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="configType">配置类型名称</param>
        /// <returns>配置对象</returns>
        Task<T> LoadConfigAsync<T>(string configType) where T : BaseConfig, new();

        /// <summary>
        /// 保存配置文件
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="config">配置对象</param>
        /// <param name="configType">配置类型名称</param>
        /// <returns>是否保存成功</returns>
        bool SaveConfig<T>(T config, string configType) where T : BaseConfig, new();

        /// <summary>
        /// 异步保存配置文件
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="config">配置对象</param>
        /// <param name="configType">配置类型名称</param>
        /// <returns>是否保存成功</returns>
        Task<bool> SaveConfigAsync<T>(T config, string configType) where T : BaseConfig, new();

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
        /// 解析路径中的环境变量
        /// </summary>
        /// <param name="path">包含环境变量的路径</param>
        /// <returns>解析后的实际路径</returns>
        string ResolveEnvironmentVariables(string path);
    }
}
