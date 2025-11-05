using System;
using System.IO;

namespace RUINORERP.Common.Config
{
    /// <summary>
    /// 配置路径类型枚举
    /// 定义系统中不同类型配置的存储位置分类
    /// </summary>
    public enum ConfigPathType
    {
        /// <summary>
        /// 服务端配置
        /// 存储在应用程序目录下的配置文件
        /// </summary>
        Server,
        
        /// <summary>
        /// 客户端配置
        /// 存储在用户本地应用数据目录下的配置文件
        /// </summary>
        Client,
        
        /// <summary>
        /// 版本配置
        /// 存储配置版本信息的目录
        /// </summary>
        Version,
        
        /// <summary>
        /// 缓存配置
        /// 存储临时缓存配置的目录
        /// </summary>
        Cache,
        
        /// <summary>
        /// 自定义配置路径
        /// 通过配置指定的自定义路径
        /// </summary>
        Custom
    }

    /// <summary>
    /// 配置路径解析器接口
    /// 提供统一的配置文件路径管理，避免硬编码路径
    /// </summary>
    public interface IConfigPathResolver
    {
        /// <summary>
        /// 获取配置基础目录
        /// </summary>
        /// <param name="pathType">配置路径类型</param>
        /// <returns>配置基础目录的完整路径</returns>
        string GetConfigDirectory(ConfigPathType pathType = ConfigPathType.Server);
        
        /// <summary>
        /// 获取指定类型配置文件的完整路径
        /// </summary>
        /// <param name="configTypeName">配置类型名称</param>
        /// <param name="pathType">路径类型</param>
        /// <returns>配置文件的完整路径</returns>
        string GetConfigFilePath(string configTypeName, ConfigPathType pathType = ConfigPathType.Server);
        
        /// <summary>
        /// 确保配置目录存在
        /// 如果目录不存在，则创建该目录
        /// </summary>
        /// <param name="pathType">路径类型</param>
        void EnsureConfigDirectoryExists(ConfigPathType pathType = ConfigPathType.Server);
        
        /// <summary>
        /// 设置自定义配置路径
        /// 仅对ConfigPathType.Custom类型有效
        /// </summary>
        /// <param name="customPath">自定义配置路径</param>
        void SetCustomConfigPath(string customPath);
        
        /// <summary>
        /// 解析路径中的环境变量
        /// </summary>
        /// <param name="path">包含环境变量的路径</param>
        /// <returns>解析后的路径</returns>
        string ResolveEnvironmentVariables(string path);
        
        /// <summary>
        /// 验证路径是否有效
        /// 检查路径是否可读写
        /// </summary>
        /// <param name="path">要验证的路径</param>
        /// <returns>路径是否有效</returns>
        bool ValidatePath(string path);
    }
}