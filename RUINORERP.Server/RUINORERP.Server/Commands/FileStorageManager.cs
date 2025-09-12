using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransInstruction;
using TransInstruction.DataModel;

namespace RUINORERP.Server.Commands
{
    // 服务器端的 FileStorageManager
    public class FileStorageManager
    {
        private readonly IDatabase _redisDb;
        private readonly ILogger<FileStorageManager> _logger;
        private readonly string _storageRoot;
        private readonly IFileStorageService _fileService;

        public FileStorageManager(
            IFileStorageService fileService,
            IConnectionMultiplexer redisConnection,
            ILogger<FileStorageManager> logger)
        {
            _fileService = fileService;
            _redisDb = redisConnection.GetDatabase();
            _logger = logger;
            _storageRoot = ConfigurationManager.AppSettings["FileStorageRoot"] ?? @"G:\FileStorage";
        }

        /// <summary>
        /// 服务器端内部使用的方法清理临时文件
        /// </summary>
        /// <param name="olderThan"></param>
        /// <returns></returns>
        public async Task CleanTempFilesAsync(TimeSpan olderThan)
        {
            try
            {
                var tempDir = Path.Combine(_storageRoot, "Temp");
                if (!Directory.Exists(tempDir))
                    return;

                var cutoffDate = DateTime.UtcNow - olderThan;
                var files = Directory.GetFiles(tempDir);
                int deletedCount = 0;

                foreach (var file in files)
                {
                    var fileInfo = new FileInfo(file);
                    if (fileInfo.LastWriteTimeUtc < cutoffDate)
                    {
                        try
                        {
                            File.Delete(file);
                            deletedCount++;

                            // 从Redis中删除文件信息
                            var fileId = Path.GetFileName(file);
                            await _redisDb.KeyDeleteAsync($"file:{fileId}");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex, "删除临时文件失败: {FileName}", file);
                        }
                    }
                }

                _logger.LogInformation("已清理 {DeletedCount} 个临时文件", deletedCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "清理临时文件失败");
            }
        }



        /// <summary>
        /// 检查文件存储 consistency
        /// 检查并修复文件与元数据的一致性
        /// </summary>
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
                            await RebuildFileRecordAsync(fileId, category);
                        }
                    }
                }

                _logger.LogInformation("一致性检查完成: {MissingFiles} 个文件缺失, {OrphanedFiles} 个文件无记录",
                    missingFiles, orphanedFiles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "一致性检查失败");
            }
        }

        private async Task RebuildFileRecordAsync(string fileId, string category)
        {
            try
            {
                var filePath = Path.Combine(_storageRoot, GetCategoryPath(category), fileId);
                var fileInfo = new FileInfo(filePath);

                var fileInfoObj = new UploadFileInfo
                {
                    FileId = fileId,
                    OriginalName = fileId, // 无法恢复原始名称
                    Category = category,
                    Size = fileInfo.Length,
                    UploadTime = fileInfo.LastWriteTimeUtc,
                    LastModified = fileInfo.LastWriteTimeUtc
                };

                // 重新缓存文件信息
                await _redisDb.StringSetAsync($"file:{fileId}", JsonConvert.SerializeObject(fileInfoObj));

                // 添加到分类文件列表
                var categoryKey = $"files:{category}";
                await _redisDb.SetAddAsync(categoryKey, fileId);

                _logger.LogInformation("已重建文件记录: {FileId}", fileId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "重建文件记录失败: {FileId}", fileId);
            }
        }

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

        // 获取存储使用情况统计
        public async Task<StorageUsageInfo> GetStorageUsageAsync()
        {
            var usageInfo = new StorageUsageInfo();

            try
            {
                var categories = new[] { "Expenses", "Products", "Payments", "Manuals", "Templates", "Temp" };

                foreach (var category in categories)
                {
                    var categoryPath = GetCategoryPath(category);
                    var dirInfo = new DirectoryInfo(Path.Combine(_storageRoot, categoryPath));

                    if (dirInfo.Exists)
                    {
                        long size = 0;
                        int count = 0;

                        // 计算目录大小和文件数量
                        foreach (var file in dirInfo.GetFiles("*", SearchOption.AllDirectories))
                        {
                            size += file.Length;
                            count++;
                        }

                        usageInfo.CategoryUsage[category] = new CategoryUsage
                        {
                            FileCount = count,
                            TotalSize = size
                        };

                        usageInfo.TotalFileCount += count;
                        usageInfo.TotalSize += size;
                    }
                }

                usageInfo.Success = true;
            }
            catch (Exception ex)
            {
                usageInfo.Success = false;
                usageInfo.Message = $"获取存储使用情况失败: {ex.Message}";
                _logger.LogError(ex, "获取存储使用情况失败");
            }

            return usageInfo;
        }
    }
}