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
        private static string ResolveEnvironmentVariables(string path)
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
    }
}