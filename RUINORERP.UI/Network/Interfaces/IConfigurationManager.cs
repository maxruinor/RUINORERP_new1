using System.Threading.Tasks;

namespace RUINORERP.UI.Network.Interfaces
{
    /// <summary>
    /// 配置管理器接口
    /// 负责管理客户端配置的加载、保存和重载
    /// </summary>
    public interface IConfigurationManager
    {
        /// <summary>
        /// 保存配置到本地文件
        /// </summary>
        /// <param name="configType">配置类型</param>
        /// <param name="configData">配置数据（JSON格式）</param>
        /// <param name="version">配置版本</param>
        /// <returns>是否保存成功</returns>
        Task<bool> SaveConfigurationAsync(string configType, string configData, string version);

        /// <summary>
        /// 加载配置
        /// </summary>
        /// <param name="configType">配置类型</param>
        /// <returns>配置数据（JSON格式）</returns>
        Task<string> LoadConfigurationAsync(string configType);

        /// <summary>
        /// 获取配置版本
        /// </summary>
        /// <param name="configType">配置类型</param>
        /// <returns>配置版本</returns>
        Task<string> GetConfigurationVersionAsync(string configType);

        /// <summary>
        /// 重载配置
        /// </summary>
        /// <param name="configType">配置类型</param>
        /// <returns>是否重载成功</returns>
        Task<bool> ReloadConfigurationAsync(string configType);

        /// <summary>
        /// 检查配置是否存在
        /// </summary>
        /// <param name="configType">配置类型</param>
        /// <returns>是否存在</returns>
        Task<bool> ConfigurationExistsAsync(string configType);

        /// <summary>
        /// 删除配置
        /// </summary>
        /// <param name="configType">配置类型</param>
        /// <returns>是否删除成功</returns>
        Task<bool> DeleteConfigurationAsync(string configType);
    }
}