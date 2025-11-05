using System;
using System.IO;

namespace RUINORERP.Common.Config
{
    /// <summary>
    /// 默认配置路径解析器实现
    /// 提供统一的配置路径管理，消除硬编码路径
    /// </summary>
    public class DefaultConfigPathResolver : IConfigPathResolver
    {
        /// <summary>
        /// 服务端配置目录名
        /// </summary>
        private const string SERVER_CONFIG_DIR = "SysConfigFiles";
        
        /// <summary>
        /// 客户端配置根目录
        /// </summary>
        private const string CLIENT_CONFIG_ROOT_DIR = "RUINORERP";
        
        /// <summary>
        /// 客户端配置子目录
        /// </summary>
        private const string CLIENT_CONFIG_SUB_DIR = "Configs";
        
        /// <summary>
        /// 版本配置子目录
        /// </summary>
        private const string VERSION_CONFIG_DIR = "Versions";
        
        /// <summary>
        /// 缓存配置子目录
        /// </summary>
        private const string CACHE_CONFIG_DIR = "Cache";
        
        /// <summary>
        /// 自定义配置路径
        /// </summary>
        private string _customConfigPath = string.Empty;
        
        /// <summary>
        /// 获取配置基础目录
        /// 根据不同的配置类型返回对应的基础目录路径
        /// </summary>
        /// <param name="pathType">配置路径类型</param>
        /// <returns>配置基础目录的完整路径</returns>
        /// <exception cref="ArgumentOutOfRangeException">当pathType值不在定义范围内时抛出</exception>
        /// <exception cref="InvalidOperationException">当使用Custom类型但自定义路径未设置时抛出</exception>
        public string GetConfigDirectory(ConfigPathType pathType = ConfigPathType.Server)
        {
            switch (pathType)
            {
                case ConfigPathType.Server:
                    // 服务端配置存储在应用程序基目录下
                    return Path.Combine(AppContext.BaseDirectory, SERVER_CONFIG_DIR);
                case ConfigPathType.Client:
                    // 客户端配置存储在用户本地应用数据目录下，避免权限问题
                    var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                    return Path.Combine(appDataPath, CLIENT_CONFIG_ROOT_DIR, CLIENT_CONFIG_SUB_DIR);
                case ConfigPathType.Version:
                    // 版本配置存储在服务端配置目录的子目录中
                    return Path.Combine(GetConfigDirectory(ConfigPathType.Server), VERSION_CONFIG_DIR);
                case ConfigPathType.Cache:
                    // 缓存配置存储在客户端配置目录的子目录中
                    return Path.Combine(GetConfigDirectory(ConfigPathType.Client), CACHE_CONFIG_DIR);
                case ConfigPathType.Custom:
                    // 自定义配置路径
                    if (string.IsNullOrEmpty(_customConfigPath))
                    {
                        throw new InvalidOperationException("自定义配置路径未设置");
                    }
                    return _customConfigPath;
                default:
                    throw new ArgumentOutOfRangeException(nameof(pathType), pathType, "无效的配置路径类型");
            }
        }
        
        /// <summary>
        /// 获取指定类型配置文件的完整路径
        /// 组合基础目录和配置类型名称生成文件路径
        /// </summary>
        /// <param name="configTypeName">配置类型名称</param>
        /// <param name="pathType">路径类型</param>
        /// <returns>配置文件的完整路径</returns>
        /// <exception cref="ArgumentException">当configTypeName为空或null时抛出</exception>
        public string GetConfigFilePath(string configTypeName, ConfigPathType pathType = ConfigPathType.Server)
        {
            if (string.IsNullOrEmpty(configTypeName))
            {
                throw new ArgumentException("配置类型名称不能为空", nameof(configTypeName));
            }
            
            // 安全处理配置类型名称，避免路径遍历攻击
            string safeConfigTypeName = Path.GetFileNameWithoutExtension(configTypeName);
            
            // 确保目录存在
            EnsureConfigDirectoryExists(pathType);
            
            // 构建完整路径
            return Path.Combine(GetConfigDirectory(pathType), $"{safeConfigTypeName}.json");
        }
        
        /// <summary>
        /// 确保配置目录存在
        /// 检查并创建指定类型的配置目录，确保目录结构存在
        /// </summary>
        /// <param name="pathType">路径类型</param>
        /// <exception cref="IOException">当无法创建目录时抛出</exception>
        public void EnsureConfigDirectoryExists(ConfigPathType pathType = ConfigPathType.Server)
        {
            string directory = GetConfigDirectory(pathType);
            
            if (!Directory.Exists(directory))
            {
                try
                {
                    // 创建目录及其所有父目录
                    Directory.CreateDirectory(directory);
                }
                catch (Exception ex)
                {
                    throw new IOException($"无法创建配置目录: {directory}", ex);
                }
            }
        }
        
        /// <summary>
        /// 设置自定义配置路径
        /// 仅对ConfigPathType.Custom类型有效
        /// </summary>
        /// <param name="customPath">自定义配置路径</param>
        /// <exception cref="ArgumentNullException">当customPath为空或null时抛出</exception>
        /// <exception cref="InvalidOperationException">当路径无效时抛出</exception>
        public void SetCustomConfigPath(string customPath)
        {
            if (string.IsNullOrEmpty(customPath))
            {
                throw new ArgumentNullException(nameof(customPath), "自定义配置路径不能为空");
            }
            
            // 解析路径中的环境变量
            string resolvedPath = ResolveEnvironmentVariables(customPath);
            
            // 验证路径
            if (!ValidatePath(resolvedPath))
            {
                throw new InvalidOperationException($"无效的自定义配置路径: {resolvedPath}");
            }
            
            _customConfigPath = resolvedPath;
        }
        
        /// <summary>
        /// 解析路径中的环境变量
        /// </summary>
        /// <param name="path">包含环境变量的路径</param>
        /// <returns>解析后的路径</returns>
        /// <exception cref="ArgumentException">当解析失败时抛出</exception>
        public string ResolveEnvironmentVariables(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return path;
            }
            
            try
            {
                // 解析%环境变量%格式的变量
                return Environment.ExpandEnvironmentVariables(path);
            }
            catch (Exception ex)
            {
                // 解析失败时返回原始路径
                throw new ArgumentException($"解析环境变量失败: {path}", ex);
            }
        }
        
        /// <summary>
        /// 验证路径是否有效
        /// 检查路径是否可读写
        /// </summary>
        /// <param name="path">要验证的路径</param>
        /// <returns>路径是否有效</returns>
        public bool ValidatePath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return false;
            }
            
            try
            {
                // 检查路径是否为有效路径
                string fullPath = Path.GetFullPath(path);
                
                // 检查路径是否包含无效字符
                if (fullPath.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
                {
                    return false;
                }
                
                // 检查目录是否存在或可创建
                DirectoryInfo directoryInfo = new DirectoryInfo(fullPath);
                if (!directoryInfo.Exists)
                {
                    try
                    {
                        directoryInfo.Create();
                        directoryInfo.Delete();
                    }
                    catch
                    {
                        return false;
                    }
                }
                
                // 检查目录是否可写
                string testFile = Path.Combine(fullPath, $"test_write_access_{Guid.NewGuid()}.tmp");
                try
                {
                    using (File.Create(testFile)) { }
                    File.Delete(testFile);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}