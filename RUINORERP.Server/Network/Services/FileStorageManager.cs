using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RUINORERP.Server.Network.Models;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.Server.Network.Interfaces.Services;
using StackExchange.Redis;
using RUINORERP.PacketSpec.Models;
using RUINORERP.Model;

namespace RUINORERP.Server.Network.Services
{
    /// <summary>
    /// 文件存储管理器 - 服务器端文件存储管理服务
    /// 迁移自: RUINORERP.PacketSpec.Services.FileStorageManager
    /// </summary>
    public class FileStorageManager
    {
        private readonly ILogger<FileStorageManager> _logger;
        private readonly IFileStorageService _fileService;
        private readonly string _storageRoot; // 直接访问服务器文件系统
        private readonly IDatabase _redisDb;  // 直接访问服务器Redis
        private readonly IConfiguration _configuration;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fileService">文件存储服务</param>
        /// <param name="logger">日志记录器</param>
        /// <param name="redisConnection">Redis连接</param>
        /// <param name="configuration">配置接口</param>
        public FileStorageManager(
            IFileStorageService fileService,
            ILogger<FileStorageManager> logger,
            IConnectionMultiplexer redisConnection,
            IConfiguration configuration)
        {
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _redisDb = redisConnection?.GetDatabase() ?? throw new ArgumentNullException(nameof(redisConnection));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            // 从配置中获取文件存储根目录
            _storageRoot = _configuration["FileStorage:Root"] ?? @"G:\FileStorage";
        }

        /// <summary>
        /// 清理临时文件
        /// </summary>
        /// <param name="olderThan">时间间隔</param>
        /// <returns>清理任务</returns>
        public async Task CleanTempFilesAsync(TimeSpan olderThan)
        {
            try
            {
                var tempDir = Path.Combine(_storageRoot, "Temp");
                if (!Directory.Exists(tempDir))
                    return;

                var cutoffDate = DateTime.Now - olderThan;
                var files = Directory.GetFiles(tempDir);
                int deletedCount = 0;

                foreach (var file in files)
                {
                    //var fileInfo = new FileStorageInfo(file);
                    //if (fileInfo.LastModified < cutoffDate)
                    //{
                    //    try
                    //    {
                    //        File.Delete(file);
                    //        deletedCount++;

                    //        // 从Redis中删除文件信息
                    //        var fileId = Path.GetFileName(file);
                    //        await _redisDb.KeyDeleteAsync($"file:{fileId}");
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        _logger.LogWarning(ex, "删除临时文件失败: {FileName}", file);
                    //    }
                    //}
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "清理临时文件失败");
            }
        }

        /// <summary>
        /// 检查文件存储一致性
        /// 检查并修复文件与元数据的一致性
        /// </summary>
        /// <returns>一致性检查任务</returns>
        public async Task CheckConsistencyAsync()
        {
            try
            {
                var categories = new[] { "Expenses", "Products", "Payments", "Manuals", "Templates" };
                int orphanedFiles = 0;
                int missingFiles = 0;

                foreach (var category in categories)
                {
                    // 获取Redis中记录的文件列表
                    var categoryKey = $"files:{category}";
                    var fileIds = await _redisDb.SetMembersAsync(categoryKey);

                    // 检查每个文件是否存在
                    foreach (var fileId in fileIds)
                    {
                        var filePath = Path.Combine(_storageRoot, GetCategoryPath(category), fileId.ToString());
                        if (!File.Exists(filePath))
                        {
                            _logger.LogWarning("文件存在记录但物理文件不存在: {FileId}", fileId);
                            missingFiles++;

                            // 从Redis中删除无效记录
                            await _redisDb.SetRemoveAsync(categoryKey, fileId);
                            await _redisDb.KeyDeleteAsync($"file:{fileId}");
                        }
                    }

                    // 检查物理文件是否有对应记录
                    var categoryPath = GetCategoryPath(category);
                    var physicalFiles = Directory.GetFiles(Path.Combine(_storageRoot, categoryPath));

                    foreach (var file in physicalFiles)
                    {
                        var fileId = Path.GetFileName(file);
                        if (!fileIds.Contains(fileId))
                        {
                            _logger.LogWarning("物理文件存在但无记录: {FileId}", fileId);
                            orphanedFiles++;

                            // 尝试重建记录
                            //await RebuildFileRecordAsync(fileId, category);
                        }
                    }
                }

                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "一致性检查失败");
            }
        }

        ///// <summary>
        ///// 重建文件记录
        ///// </summary>
        ///// <param name="fileId">文件ID</param>
        ///// <param name="category">文件分类</param>
        ///// <returns>重建任务</returns>
        //private async Task RebuildFileRecordAsync(string fileId, string category)
        //{
        //    try
        //    {
        //        var filePath = Path.Combine(_storageRoot, GetCategoryPath(category), fileId);
        //        var fileInfo = new tb_FS_FileStorageInfo(filePath);

        //        var fileInfoObj = new tb_FS_FileStorageInfo(filePath)
        //        {
        //            FileId = fileId,
        //            OriginalName = fileId, // 无法恢复原始名称
        //            Category = category,
        //            Size = fileInfo.Size,
        //            UploadTime = fileInfo.UploadTime,
        //            LastModified = fileInfo.LastModified
        //        };

        //        // 重新缓存文件信息
        //        await _redisDb.StringSetAsync($"file:{fileId}", JsonConvert.SerializeObject(fileInfoObj));

        //        // 添加到分类文件列表
        //        var categoryKey = $"files:{category}";
        //        await _redisDb.SetAddAsync(categoryKey, fileId);

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "重建文件记录失败: {FileId}", fileId);
        //    }
        //}

        /// <summary>
        /// 获取分类路径
        /// </summary>
        /// <param name="category">文件分类</param>
        /// <returns>分类路径</returns>
        private string GetCategoryPath(string category)
        {
            return category switch
            {
                "Expenses" => "Images/Expenses",
                "Products" => "Images/Products",
                "Payments" => "Images/Payments",
                "Manuals" => "Documents/Manuals",
                "Templates" => "Documents/Templates",
                _ => "Temp"
            };
        }

        ///// <summary>
        ///// 获取存储使用情况统计
        ///// </summary>
        ///// <returns>存储使用信息</returns>
        //public async Task<StorageUsageInfo> GetStorageUsageAsync()
        //{
        //    // 初始化响应数据
        //    var data = new StorageUsageInfoData
        //    {
        //        CategoryUsage = new Dictionary<string, CategoryUsage>(),
        //        TotalSize = 0,
        //        TotalFileCount = 0
        //    };
            
        //    var usageInfo = new StorageUsageInfo(true, "获取存储使用信息成功", data);
            
        //    try
        //    {
        //        var categories = new[] { "Expenses", "Products", "Payments", "Manuals", "Templates", "Temp" };

        //        foreach (var category in categories)
        //        {
        //            var categoryPath = GetCategoryPath(category);
        //            var fullPath = Path.Combine(_storageRoot, categoryPath);

        //            if (Directory.Exists(fullPath))
        //            {
        //                var files = Directory.GetFiles(fullPath, "*.*", SearchOption.AllDirectories);
        //                var totalSize = files.Sum(file => new FileStorageInfo(file).Size);

        //                usageInfo.Data.CategoryUsage[category] = new CategoryUsage
        //                {
        //                    FileCount = files.Length,
        //                    TotalSize = totalSize
        //                };

        //                usageInfo.Data.TotalFileCount += files.Length;
        //                usageInfo.Data.TotalSize += totalSize;
        //            }
        //            else
        //            {
        //                usageInfo.Data.CategoryUsage[category] = new CategoryUsage
        //                {
        //                    FileCount = 0,
        //                    TotalSize = 0
        //                };
        //            }
        //        }

        //        return usageInfo;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "获取存储使用统计失败");
        //        return usageInfo;
        //    }
        //}

        /// <summary>
        /// 备份文件存储
        /// </summary>
        /// <param name="backupPath">备份路径</param>
        /// <returns>备份任务</returns>
        public async Task<bool> BackupStorageAsync(string backupPath)
        {
            try
            {
                if (!Directory.Exists(backupPath))
                {
                    Directory.CreateDirectory(backupPath);
                }

                // 备份文件数据
                foreach (var category in new[] { "Expenses", "Products", "Payments", "Manuals", "Templates" })
                {
                    var sourcePath = Path.Combine(_storageRoot, GetCategoryPath(category));
                    var destPath = Path.Combine(backupPath, category);

                    if (Directory.Exists(sourcePath))
                    {
                        CopyDirectory(sourcePath, destPath, true);
                    }
                }

                // 备份Redis数据
                var redisBackupFile = Path.Combine(backupPath, "redis_backup.rdb");
                await _redisDb.ExecuteAsync("SAVE");

                var redisDataPath = _configuration["Redis:DataPath"] ?? "/var/lib/redis/dump.rdb";
                if (File.Exists(redisDataPath))
                {
                    File.Copy(redisDataPath, redisBackupFile, true);
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "文件存储备份失败");
                return false;
            }
        }

        /// <summary>
        /// 恢复文件存储
        /// </summary>
        /// <param name="backupPath">备份路径</param>
        /// <returns>恢复任务</returns>
        public async Task<bool> RestoreStorageAsync(string backupPath)
        {
            try
            {
                if (!Directory.Exists(backupPath))
                {
                    _logger.LogError("备份路径不存在: {BackupPath}", backupPath);
                    return false;
                }

                // 恢复文件数据
                foreach (var category in new[] { "Expenses", "Products", "Payments", "Manuals", "Templates" })
                {
                    var sourcePath = Path.Combine(backupPath, category);
                    var destPath = Path.Combine(_storageRoot, GetCategoryPath(category));

                    if (Directory.Exists(sourcePath))
                    {
                        // 清空目标目录
                        if (Directory.Exists(destPath))
                        {
                            Directory.Delete(destPath, true);
                        }
                        Directory.CreateDirectory(destPath);

                        CopyDirectory(sourcePath, destPath, true);
                    }
                }

                // 恢复Redis数据
                var redisBackupFile = Path.Combine(backupPath, "redis_backup.rdb");
                if (File.Exists(redisBackupFile))
                {
                    var redisDataPath = _configuration["Redis:DataPath"] ?? "/var/lib/redis/dump.rdb";
                    File.Copy(redisBackupFile, redisDataPath, true);

                    // 重启Redis服务使备份生效
                    await _redisDb.ExecuteAsync("SHUTDOWN", "NOSAVE");
                    // 这里需要等待Redis重启完成
                    await Task.Delay(5000);
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "文件存储恢复失败");
                return false;
            }
        }

        #region Utility Methods

        /// <summary>
        /// 复制目录
        /// </summary>
        /// <param name="sourceDir">源目录</param>
        /// <param name="destinationDir">目标目录</param>
        /// <param name="recursive">是否递归</param>
        private void CopyDirectory(string sourceDir, string destinationDir, bool recursive)
        {
            // 获取源目录信息
            var dir = new DirectoryInfo(sourceDir);

            // 检查源目录是否存在
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException($"源目录不存在: {dir.FullName}");
            }

            // 确保目标目录存在
            Directory.CreateDirectory(destinationDir);

            // 获取源目录中的所有文件
            foreach (System.IO.FileInfo file in dir.GetFiles())
            {
                string targetFilePath = Path.Combine(destinationDir, file.Name);
                file.CopyTo(targetFilePath, true);
            }

            // 如果递归，复制所有子目录
            if (recursive)
            {
                foreach (DirectoryInfo subDir in dir.GetDirectories())
                {
                    string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                    CopyDirectory(subDir.FullName, newDestinationDir, true);
                }
            }
        }

        #endregion
    }

    

     
}