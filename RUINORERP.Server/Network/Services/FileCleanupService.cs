using RUINORERP.Model;
using RUINORERP.Model.ConfigModel;
using RUINORERP.Repository.UnitOfWorks;
using Microsoft.Extensions.Logging;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RUINORERP.Server.Helpers;
using RUINORERP.Global;

namespace RUINORERP.Server.Network.Services
{
    /// <summary>
    /// 文件清理服务 - 负责处理过期文件和孤立文件的清理
    /// 支持自动清理和手动清理两种模式
    /// </summary>
    public class FileCleanupService : IDisposable
    {
        private readonly IUnitOfWorkManage _unitOfWorkManage;
        private readonly ILogger<FileCleanupService> _logger;
        private readonly ServerGlobalConfig _serverConfig;
        private readonly Timer _cleanupTimer;
        private readonly SemaphoreSlim _cleanupLock = new SemaphoreSlim(1, 1);
        private bool _disposed = false;



        /// <summary>
        /// 构造函数
        /// </summary>
        public FileCleanupService(
            IUnitOfWorkManage unitOfWorkManage,
            ILogger<FileCleanupService> logger,
            ServerGlobalConfig serverConfig)
        {
            _unitOfWorkManage = unitOfWorkManage;
            _logger = logger;
            _serverConfig = serverConfig;

            // 启动定时清理任务(每天凌晨2点执行)
            var now = DateTime.Now;
            var nextRun = new DateTime(now.Year, now.Month, now.Day, 2, 0, 0);
            if (now > nextRun)
            {
                nextRun = nextRun.AddDays(1);
            }
            var dueTime = nextRun - now;

            _cleanupTimer = new Timer(
                async _ => await ExecuteAutoCleanupAsync(),
                null,
                dueTime,
                TimeSpan.FromDays(1));

            _logger.LogInformation($"文件清理服务已启动,首次清理时间: {nextRun:yyyy-MM-dd HH:mm:ss}");
        }

        /// <summary>
        /// 执行自动清理任务
        /// </summary>
        private async Task ExecuteAutoCleanupAsync()
        {
            try
            {
                _logger.LogInformation("开始执行自动文件清理任务");
                await CleanupExpiredFilesAsync();
                await CleanupOrphanedFilesAsync();
                _logger.LogInformation("自动文件清理任务完成");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "自动文件清理任务执行失败");
            }
        }

        /// <summary>
        /// 清理过期文件 - 将文件状态标记为过期
        /// </summary>
        /// <param name="daysThreshold">过期天数阈值(默认30天)</param>
        /// <param name="physicalDelete">是否物理删除(默认false)</param>
        /// <returns>清理的文件数量</returns>
        public async Task<int> CleanupExpiredFilesAsync(int daysThreshold = 30, bool physicalDelete = false)
        {
            if (!await _cleanupLock.WaitAsync(TimeSpan.FromMinutes(10)))
            {
                _logger.LogWarning("清理任务正在进行中,跳过本次执行");
                return 0;
            }

            try
            {
                var db = _unitOfWorkManage.GetDbClient();
                var expireDate = DateTime.Now.AddDays(-daysThreshold);

                // 查询过期且未逻辑删除的文件
                var expiredFiles = await db.Queryable<tb_FS_FileStorageInfo>()
                    .Where(f => f.ExpireTime < DateTime.Now)
                    .Where(f => f.FileStatus == (int)FileStatus.Active) // 0=正常状态
                    .Where(f => f.isdeleted == false)
                    .ToListAsync();

                int cleanedCount = 0;

                if (physicalDelete)
                {
                    // 物理删除模式:同时删除物理文件和数据库记录
                    foreach (var file in expiredFiles)
                    {
                        try
                        {
                            // 检查是否有业务关联
                            var hasRelation = await db.Queryable<tb_FS_BusinessRelation>()
                                .Where(r => r.FileId == file.FileId)
                                .Where(r => r.isdeleted == false)
                                .AnyAsync();

                            if (!hasRelation)
                            {
                                // 无关联,物理删除
                                if (await DeletePhysicalFileAsync(file))
                                {
                                    cleanedCount++;
                                }
                            }
                            else
                            {
                                // 有关联,仅标记过期
                                file.FileStatus = (int)FileStatus.Expired;
                                await db.Updateable(file).ExecuteCommandAsync();
                                cleanedCount++;
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "清理过期文件失败,FileId: {FileId}", file.FileId);
                        }
                    }
                }
                else
                {
                    // 逻辑删除模式:仅标记状态
                    await db.Updateable<tb_FS_FileStorageInfo>()
                        .SetColumns(f => f.FileStatus == (int)FileStatus.Expired)
                        .Where(f => f.ExpireTime < DateTime.Now)
                        .Where(f => f.FileStatus == (int)FileStatus.Active)
                        .Where(f => f.isdeleted == false)
                        .ExecuteCommandAsync();

                    cleanedCount = expiredFiles.Count;
                }

                _logger.LogInformation("过期文件清理完成,共清理 {Count} 个文件", cleanedCount);
                return cleanedCount;
            }
            finally
            {
                _cleanupLock.Release();
            }
        }

        /// <summary>
        /// 清理孤立文件 - 无业务关联的文件
        /// </summary>
        /// <param name="daysThreshold">孤立天数阈值(默认7天)</param>
        /// <param name="physicalDelete">是否物理删除(默认false)</param>
        /// <returns>清理的文件数量</returns>
        public async Task<int> CleanupOrphanedFilesAsync(int daysThreshold = 7, bool physicalDelete = false)
        {
            if (!await _cleanupLock.WaitAsync(TimeSpan.FromMinutes(10)))
            {
                _logger.LogWarning("清理任务正在进行中,跳过本次执行");
                return 0;
            }

            try
            {
                var db = _unitOfWorkManage.GetDbClient();
                var thresholdDate = DateTime.Now.AddDays(-daysThreshold);

                // 查询孤立文件:存在数据库但无业务关联且创建时间超过阈值
                var orphanedFiles = await db.Queryable<tb_FS_FileStorageInfo>()
                    .LeftJoin<tb_FS_BusinessRelation>((f, r) => f.FileId == r.FileId)
                    .Where((f, r) => r.RelationId == null) // 无关联
                    .Where((f, r) => f.FileStatus ==(int) FileStatus.Active) // 正常状态
                    .Where((f, r) => f.isdeleted == false)
                    .Where((f, r) => f.Created_at < thresholdDate)
                    .Select((f, r) => f)
                    .Distinct()
                    .ToListAsync();

                int cleanedCount = 0;

                foreach (var file in orphanedFiles)
                {
                    try
                    {
                        if (physicalDelete)
                        {
                            // 物理删除
                            if (await DeletePhysicalFileAsync(file))
                            {
                                cleanedCount++;
                            }
                        }
                        else
                        {
                            // 标记为孤立文件
                            file.FileStatus = (int)FileStatus.Orphaned;
                            await db.Updateable(file).ExecuteCommandAsync();
                            cleanedCount++;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "清理孤立文件失败,FileId: {FileId}", file.FileId);
                    }
                }

                _logger.LogInformation("孤立文件清理完成,共清理 {Count} 个文件", cleanedCount);
                return cleanedCount;
            }
            finally
            {
                _cleanupLock.Release();
            }
        }

        /// <summary>
        /// 清理物理目录中的孤立文件 - 数据库中不存在但物理目录中存在的文件
        /// </summary>
        /// <returns>清理的文件数量</returns>
        public async Task<int> CleanupPhysicalOrphanedFilesAsync()
        {
            if (!await _cleanupLock.WaitAsync(TimeSpan.FromMinutes(30)))
            {
                _logger.LogWarning("清理任务正在进行中,跳过本次执行");
                return 0;
            }

            try
            {
                int cleanedCount = 0;
                var storagePath = FileStorageHelper.GetStoragePath();

                if (!Directory.Exists(storagePath))
                {
                    _logger.LogWarning("文件存储目录不存在: {Path}", storagePath);
                    return 0;
                }

                var db = _unitOfWorkManage.GetDbClient();
                var allFileIds = await db.Queryable<tb_FS_FileStorageInfo>()
                    .Where(f => f.isdeleted == false)
                    .Select(f => f.StorageFileName)
                    .ToListAsync();

                var existingFileNames = new HashSet<string>(allFileIds, StringComparer.OrdinalIgnoreCase);

                // 遍历所有文件
                foreach (var filePath in Directory.EnumerateFiles(storagePath, "*.*", SearchOption.AllDirectories))
                {
                    var fileName = Path.GetFileName(filePath);

                    // 跳过非管理文件(如.gitkeep, .DS_Store等)
                    if (fileName.StartsWith(".") || fileName == "desktop.ini" || fileName == "Thumbs.db")
                    {
                        continue;
                    }

                    // 检查文件是否在数据库中
                    if (!existingFileNames.Contains(fileName))
                    {
                        try
                        {
                            // 检查文件是否超过30天未访问
                            var fileInfo = new FileInfo(filePath);
                            if (fileInfo.LastAccessTime < DateTime.Now.AddDays(-30))
                            {
                                File.Delete(filePath);
                                _logger.LogInformation("已删除孤立物理文件: {FilePath}", filePath);
                                cleanedCount++;
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "删除孤立物理文件失败: {FilePath}", filePath);
                        }
                    }
                }

                _logger.LogInformation("孤立物理文件清理完成,共清理 {Count} 个文件", cleanedCount);
                return cleanedCount;
            }
            finally
            {
                _cleanupLock.Release();
            }
        }

        /// <summary>
        /// 物理删除单个文件及其关联
        /// </summary>
        /// <param name="fileInfo">文件信息</param>
        /// <returns>是否删除成功</returns>
        private async Task<bool> DeletePhysicalFileAsync(tb_FS_FileStorageInfo fileInfo)
        {
            try
            {
                var db = _unitOfWorkManage.GetDbClient();

                // 1. 删除业务关联记录
                await db.Deleteable<tb_FS_BusinessRelation>()
                    .Where(r => r.FileId == fileInfo.FileId)
                    .ExecuteCommandAsync();

                // 2. 删除文件版本记录(如果有)
                await db.Deleteable<tb_FS_FileStorageVersion>()
                    .Where(v => v.FileId == fileInfo.FileId)
                    .ExecuteCommandAsync();

                // 3. 删除物理文件
                var filePath = FileStorageHelper.ResolveToAbsolutePath(fileInfo.StoragePath);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    _logger.LogInformation("已删除物理文件: {FilePath}", filePath);
                }

                // 4. 逻辑删除文件元数据
                fileInfo.isdeleted = true;
                fileInfo.FileStatus = (int)FileStatus.Deleted;
                fileInfo.Modified_at = DateTime.Now;
                await db.Updateable(fileInfo).ExecuteCommandAsync();

                _logger.LogInformation("物理删除文件成功,FileId: {FileId}, FileName: {FileName}",
                    fileInfo.FileId, fileInfo.OriginalFileName);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "物理删除文件失败,FileId: {FileId}", fileInfo.FileId);
                return false;
            }
        }

        /// <summary>
        /// 恢复被标记的文件(过期或孤立)
        /// </summary>
        /// <param name="fileIds">文件ID列表</param>
        /// <returns>恢复的文件数量</returns>
        public async Task<int> RestoreFilesAsync(List<long> fileIds)
        {
            try
            {
                var db = _unitOfWorkManage.GetDbClient();
                var result = await db.Updateable<tb_FS_FileStorageInfo>()
                    .SetColumns(f => f.FileStatus == 0) // 恢复为正常状态
                    .SetColumns(f => f.ExpireTime == DateTime.MaxValue) // 重置过期时间
                    .Where(f => fileIds.Contains(f.FileId))
                    .Where(f => f.isdeleted == false)
                    .ExecuteCommandAsync();

                _logger.LogInformation("已恢复 {Count} 个文件", result);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "恢复文件失败");
                return 0;
            }
        }

        /// <summary>
        /// 获取文件统计信息
        /// </summary>
        /// <returns>文件统计信息</returns>
        public async Task<FileCleanupStatistics> GetCleanupStatisticsAsync()
        {
            var db = _unitOfWorkManage.GetDbClient();

            var stats = new FileCleanupStatistics
            {
                TotalFiles = await db.Queryable<tb_FS_FileStorageInfo>()
                    .Where(f => f.isdeleted == false)
                    .CountAsync(),

                ActiveFiles = await db.Queryable<tb_FS_FileStorageInfo>()
                    .Where(f => f.FileStatus == (int)FileStatus.Active && f.isdeleted == false)
                    .CountAsync(),

                ExpiredFiles = await db.Queryable<tb_FS_FileStorageInfo>()
                    .Where(f => f.FileStatus == (int)FileStatus.Expired && f.isdeleted == false)
                    .CountAsync(),

                OrphanedFiles = await db.Queryable<tb_FS_FileStorageInfo>()
                    .Where(f => f.FileStatus == (int)FileStatus.Orphaned && f.isdeleted == false)
                    .CountAsync(),

                DeletedFiles = await db.Queryable<tb_FS_FileStorageInfo>()
                    .Where(f => f.FileStatus == (int)FileStatus.Deleted && f.isdeleted == false)
                    .CountAsync(),

                TotalStorageSize = await db.Queryable<tb_FS_FileStorageInfo>()
                    .Where(f => f.isdeleted == false)
                    .SumAsync(f => f.FileSize)
            };

            return stats;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;
            _cleanupTimer?.Dispose();
            _cleanupLock?.Dispose();
        }
    }

    /// <summary>
    /// 文件清理统计信息
    /// </summary>
    public class FileCleanupStatistics
    {
        /// <summary>
        /// 总文件数
        /// </summary>
        public int TotalFiles { get; set; }

        /// <summary>
        /// 正常文件数
        /// </summary>
        public int ActiveFiles { get; set; }

        /// <summary>
        /// 过期文件数
        /// </summary>
        public int ExpiredFiles { get; set; }

        /// <summary>
        /// 孤立文件数
        /// </summary>
        public int OrphanedFiles { get; set; }

        /// <summary>
        /// 已删除文件数
        /// </summary>
        public int DeletedFiles { get; set; }

        /// <summary>
        /// 总存储空间(字节)
        /// </summary>
        public long TotalStorageSize { get; set; }

        /// <summary>
        /// 总存储空间(格式化字符串)
        /// </summary>
        public string TotalStorageSizeFormatted => FormatFileSize(TotalStorageSize);

        /// <summary>
        /// 格式化文件大小
        /// </summary>
        private string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            int order = 0;
            double size = bytes;

            while (size >= 1024 && order < sizes.Length - 1)
            {
                order++;
                size /= 1024;
            }

            return $"{size:0.##} {sizes[order]}";
        }
    }
}
