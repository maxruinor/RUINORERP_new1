using System;
using System.IO;
using RUINORERP.Model.ConfigModel;
using System.Collections.Generic;
using RUINORERP.PacketSpec.Models.FileManagement;

namespace RUINORERP.Server.Helpers
{
    /// <summary>
    /// 文件存储帮助类
    /// 提供文件路径管理、存储初始化等辅助功能
    /// </summary>
    public static class FileStorageHelper
    {
        private static ServerGlobalConfig _serverConfig;
        
        /// <summary>
        /// 初始化文件存储路径
        /// </summary>
        /// <param name="serverConfig">服务器配置</param>
        public static void InitializeStoragePath(ServerGlobalConfig serverConfig)
        {
            _serverConfig = serverConfig;
            
            // 确保根存储目录存在
            if (!string.IsNullOrEmpty(_serverConfig.FileStoragePath))
            {
                var resolvedPath = ResolveEnvironmentVariables(_serverConfig.FileStoragePath);
                
                // 安全检查：确保存储路径不在程序运行目录或其子目录中
                if (IsPathInProgramDirectory(resolvedPath))
                {
                    throw new InvalidOperationException($"文件存储路径 '{resolvedPath}' 不能设置在程序运行目录或其子目录中。请选择其他目录以防止重新部署时误删文件。");
                }
                
                if (!Directory.Exists(resolvedPath))
                {
                    Directory.CreateDirectory(resolvedPath);
                }
            }
            
            
        }
        
        /// <summary>
        /// 解析路径中的环境变量
        /// </summary>
        /// <param name="path">包含环境变量的路径</param>
        /// <returns>解析后的实际路径</returns>
        public static string ResolveEnvironmentVariables(string path)
        {
            if (string.IsNullOrEmpty(path))
                return path;
                
            // 解析路径中的环境变量（如 %APPDATA%）
            return System.Environment.ExpandEnvironmentVariables(path);
        }
        
     
        
       
        
        /// <summary>
        /// 验证文件大小是否符合限制
        /// </summary>
        /// <param name="fileSize">文件大小（字节）</param>
        /// <returns>是否符合限制</returns>
        public static bool ValidateFileSize(long fileSize)
        {
            if (_serverConfig == null)
                return true;
                
            return fileSize <= _serverConfig.MaxFileSizeMB * 1024 * 1024;
        }
        
        /// <summary>
        /// 获取存储使用情况
        /// </summary>
        /// <returns>存储使用信息</returns>
        public static StorageUsageInfoData GetStorageUsageInfo()
        {
            var usageInfo = new StorageUsageInfoData
            {
                CategoryUsage = new Dictionary<string, CategoryUsage>()
            };
            
            if (_serverConfig == null)
                return usageInfo;
                
            try
            {
                // 计算根目录使用情况
                var resolvedRootPath = ResolveEnvironmentVariables(_serverConfig.FileStoragePath);
                var rootDir = new DirectoryInfo(resolvedRootPath);
                if (rootDir.Exists)
                {
                    usageInfo.TotalSize = GetDirectorySize(rootDir);
                    usageInfo.TotalFileCount = GetFileCount(rootDir);
                }
                
                // 计算各类别使用情况
                //var categoryPaths = GetCategoryPaths();
                //foreach (var category in categoryPaths)
                //{
                //    var categoryDir = new DirectoryInfo(category.Value);
                //    if (categoryDir.Exists)
                //    {
                //        var categoryUsage = new CategoryUsage
                //        {
                //            FileCount = GetFileCount(categoryDir),
                //            TotalSize = GetDirectorySize(categoryDir)
                //        };
                        
                //        usageInfo.CategoryUsage[category.Key] = categoryUsage;
                //    }
                //}
            }
            catch (Exception ex)
            {
                // 记录日志但不抛出异常
                System.Diagnostics.Debug.WriteLine($"获取存储使用信息时出错: {ex.Message}");
            }
            
            return usageInfo;
        }
        
        /// <summary>
        /// 获取目录大小
        /// </summary>
        /// <param name="directory">目录信息</param>
        /// <returns>目录大小（字节）</returns>
        private static long GetDirectorySize(DirectoryInfo directory)
        {
            long size = 0;
            
            try
            {
                // 计算文件大小
                foreach (var file in directory.GetFiles())
                {
                    size += file.Length;
                }
                
                // 递归计算子目录大小
                foreach (var subDirectory in directory.GetDirectories())
                {
                    size += GetDirectorySize(subDirectory);
                }
            }
            catch (Exception ex)
            {
                // 记录日志但不抛出异常
                System.Diagnostics.Debug.WriteLine($"计算目录大小时出错: {ex.Message}");
            }
            
            return size;
        }
        
        /// <summary>
        /// 获取目录中文件数量
        /// </summary>
        /// <param name="directory">目录信息</param>
        /// <returns>文件数量</returns>
        private static int GetFileCount(DirectoryInfo directory)
        {
            int count = 0;
            
            try
            {
                count = directory.GetFiles().Length;
                
                // 递归计算子目录中的文件数量
                foreach (var subDirectory in directory.GetDirectories())
                {
                    count += GetFileCount(subDirectory);
                }
            }
            catch (Exception ex)
            {
                // 记录日志但不抛出异常
                System.Diagnostics.Debug.WriteLine($"计算文件数量时出错: {ex.Message}");
            }
            
            return count;
        }

        /// <summary>
        /// 检查路径是否在程序运行目录或其子目录中
        /// </summary>
        /// <param name="filePath">要检查的文件路径</param>
        /// <returns>true=路径在程序目录中，false=路径安全</returns>
        private static bool IsPathInProgramDirectory(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return false;

            try
            {
                // 如果路径不是绝对路径，无法进行目录关系检查
                if (!Path.IsPathRooted(filePath))
                    return false;

                // 获取程序运行目录
                string programDirectory = AppDomain.CurrentDomain.BaseDirectory;
                
                // 检查路径是否在程序运行目录或其子目录中
                bool isInProgramDirectory = filePath.StartsWith(programDirectory, StringComparison.OrdinalIgnoreCase);
                
                return isInProgramDirectory;
            }
            catch (Exception ex)
            {
                // 如果检查失败，记录日志并返回false（避免安全检查导致系统问题）
                System.Diagnostics.Debug.WriteLine($"检查路径安全性时出错: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 将绝对路径转换为相对于存储根目录的相对路径
        /// </summary>
        /// <param name="absolutePath">绝对路径</param>
        /// <returns>相对路径</returns>
        public static string ConvertToRelativePath(string absolutePath)
        {
            if (string.IsNullOrEmpty(absolutePath) || !Path.IsPathRooted(absolutePath))
                return absolutePath;

            try
            {
                if (_serverConfig == null || string.IsNullOrEmpty(_serverConfig.FileStoragePath))
                    return absolutePath;

                var resolvedRootPath = ResolveEnvironmentVariables(_serverConfig.FileStoragePath);
                
                // 确保路径格式一致
                var rootUri = new Uri(resolvedRootPath.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar);
                var fileUri = new Uri(absolutePath);
                
                // 如果文件路径在根目录下，返回相对路径
                if (rootUri.IsBaseOf(fileUri))
                {
                    var relativeUri = rootUri.MakeRelativeUri(fileUri);
                    return Uri.UnescapeDataString(relativeUri.ToString()).Replace('/', Path.DirectorySeparatorChar);
                }
                
                // 如果不在根目录下，返回原路径
                return absolutePath;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"转换相对路径失败: {ex.Message}");
                return absolutePath;
            }
        }

        /// <summary>
        /// 将相对路径解析为绝对路径
        /// </summary>
        /// <param name="relativePath">相对路径</param>
        /// <returns>绝对路径</returns>
        public static string ResolveToAbsolutePath(string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath))
                return relativePath;

            try
            {
                // 如果已经是绝对路径，直接返回
                if (Path.IsPathRooted(relativePath))
                    return relativePath;

                if (_serverConfig == null || string.IsNullOrEmpty(_serverConfig.FileStoragePath))
                    return relativePath;

                var resolvedRootPath = ResolveEnvironmentVariables(_serverConfig.FileStoragePath);
                
                // 组合根目录和相对路径
                var absolutePath = Path.Combine(resolvedRootPath, relativePath);
                
                // 规范化路径（移除多余的..和.）
                return Path.GetFullPath(absolutePath);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"解析绝对路径失败: {ex.Message}");
                return relativePath;
            }
        }

        /// <summary>
        /// 检查文件是否存在（支持相对路径和绝对路径）
        /// </summary>
        /// <param name="filePath">文件路径（相对或绝对）</param>
        /// <returns>文件是否存在</returns>
        public static bool FileExists(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return false;

            try
            {
                var absolutePath = ResolveToAbsolutePath(filePath);
                return File.Exists(absolutePath);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"检查文件存在性失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 读取文件内容（支持相对路径和绝对路径）
        /// </summary>
        /// <param name="filePath">文件路径（相对或绝对）</param>
        /// <returns>文件内容字节数组</returns>
        public static byte[] ReadFileBytes(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return null;

            try
            {
                var absolutePath = ResolveToAbsolutePath(filePath);
                return File.ReadAllBytes(absolutePath);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"读取文件失败: {ex.Message}");
                return null;
            }
        }
    }
}