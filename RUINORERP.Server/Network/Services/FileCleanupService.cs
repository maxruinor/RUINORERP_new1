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
    /// 注意: 定时调度由工作流系统(FileCleanupWorkflow)处理,本服务只提供清理方法
    /// 工作流系统会创建独立的执行上下文,避免数据库连接问题
    /// </summary>
    public class FileCleanupService : IDisposable
    {
        private readonly IUnitOfWorkManage _unitOfWorkManage;
        private readonly ILogger<FileCleanupService> _logger;
        private readonly ServerGlobalConfig _serverConfig;
        private readonly SemaphoreSlim _dbLock = new SemaphoreSlim(1, 1); // 数据库操作锁
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
            
            _logger.LogInformation("文件清理服务已初始化(定时调度由工作流系统处理)");
        }

        /// <summary>
        /// 清理过期文件 - 将文件状态标记为过期
        /// </summary>
        /// <param name="daysThreshold">过期天数阈值(默认30天)</param>
        /// <param name="physicalDelete">是否物理删除(默认false)</param>
        /// <returns>清理的文件数量</returns>
        public async Task<int> CleanupExpiredFilesAsync(int daysThreshold = 30, bool physicalDelete = false)
        {
            // 使用数据库操作锁,避免并发访问导致的连接冲突
            if (!await _dbLock.WaitAsync(TimeSpan.FromMinutes(10)))
            {
                _logger.LogWarning("数据库操作繁忙,跳过本次清理");
                return 0;
            }

            try
            {
                var db = _unitOfWorkManage.GetDbClient();
                var expireDate = DateTime.Now.AddDays(-daysThreshold);

                // 使用 ToList 而不是 ToListAsync,避免异步操作在特定场景下的连接问题
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
                    // 逻辑删除模式:仅标记状态 - 使用批量更新减少数据库连接次数
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
                _dbLock.Release();
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
            // 使用数据库操作锁,避免并发访问导致的连接冲突
            if (!await _dbLock.WaitAsync(TimeSpan.FromMinutes(10)))
            {
                _logger.LogWarning("数据库操作繁忙,跳过本次清理");
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
                    .Select((f, r) => f.FileId) // 只选择FileId避免text类型比较问题
                    .ToListAsync();

                // 根据FileId查询完整文件信息
                var fileIds = orphanedFiles.Distinct().ToList();
                var fullFileList = new List<tb_FS_FileStorageInfo>();
                if (fileIds.Any())
                {
                    fullFileList = await db.Queryable<tb_FS_FileStorageInfo>()
                        .Where(f => fileIds.Contains(f.FileId))
                        .ToListAsync();
                }

                int cleanedCount = 0;

                foreach (var file in fullFileList)
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
                _dbLock.Release();
            }
        }

        /// <summary>
        /// 清理物理目录中的孤立文件 - 数据库中不存在但物理目录中存在的文件
        /// </summary>
        /// <returns>清理的文件数量</returns>
        public async Task<int> CleanupPhysicalOrphanedFilesAsync()
        {
            // 使用数据库操作锁,避免并发访问导致的连接冲突
            if (!await _dbLock.WaitAsync(TimeSpan.FromMinutes(30)))
            {
                _logger.LogWarning("数据库操作繁忙,跳过本次清理");
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
                                cleanedCount++;
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "删除孤立物理文件失败: {FilePath}", filePath);
                        }
                    }
                }

                _logger.LogInformation("物理孤立文件清理完成,共清理 {Count} 个文件", cleanedCount);
                return cleanedCount;
            }
            finally
            {
                _dbLock.Release();
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
                }

                // 4. 逻辑删除文件元数据
                fileInfo.isdeleted = true;
                fileInfo.Modified_at = DateTime.Now;
                await db.Updateable(fileInfo).ExecuteCommandAsync();
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
        /// 按方案A：以关联表 isdeleted 为准统计已删除文件
        /// </summary>
        /// <returns>文件统计信息</returns>
        public async Task<FileCleanupStatistics> GetCleanupStatisticsAsync()
        {
            var db = _unitOfWorkManage.GetDbClient();

            // 查询所有未删除的文件信息（用于基础统计）
            var files = await db.Queryable<tb_FS_FileStorageInfo>()
                .Where(f => f.isdeleted == false)
                .Select(f => new { f.FileStatus, FileSize = f.FileSize > 0 ? f.FileSize : 0 })
                .ToListAsync();

            // 按方案A：统计已删除的关联记录数
            var deletedRelationsCount = await db.Queryable<tb_FS_BusinessRelation>()
                .Where(r => r.isdeleted == true)
                .CountAsync();

            // 内存中分组统计
            var stats = new FileCleanupStatistics
            {
                TotalFiles = files.Count,
                ActiveFiles = files.Count(f => f.FileStatus == (int)FileStatus.Active),
                ExpiredFiles = files.Count(f => f.FileStatus == (int)FileStatus.Expired),
                OrphanedFiles = files.Count(f => f.FileStatus == (int)FileStatus.Orphaned),
                DeletedFiles = deletedRelationsCount, // 改为统计已删除的关联记录数
                TotalStorageSize = files.Sum(f => f.FileSize)
            };

            return stats;
        }

        /// <summary>
        /// 获取已删除的文件和业务关联记录
        /// 按方案A：以关联表 isdeleted 为准查询
        /// </summary>
        /// <returns>已删除文件信息列表</returns>
        public async Task<List<DeletedFileInfo>> GetDeletedFilesAsync()
        {
            var db = _unitOfWorkManage.GetDbClient();

            // 调试：查询所有已删除的关联记录
            var allDeletedRelations = await db.Queryable<tb_FS_BusinessRelation>()
                .Where(r => r.isdeleted == true)
                .CountAsync();
            _logger?.LogDebug("已删除的关联记录总数: {Count}", allDeletedRelations);

            // 调试：查询所有文件记录
            var allFiles = await db.Queryable<tb_FS_FileStorageInfo>()
                .CountAsync();
            _logger?.LogDebug("所有文件记录总数: {Count}", allFiles);

            // 调试：查询未删除的文件记录
            var activeFiles = await db.Queryable<tb_FS_FileStorageInfo>()
                .Where(f => f.isdeleted == false)
                .CountAsync();
            _logger?.LogDebug("未删除的文件记录数: {Count}", activeFiles);

            // 使用 INNER JOIN 查询已删除的业务关联及其文件信息
            // 关键：以关联表 isdeleted=true 为准，文件表 isdeleted=false (排除已物理删除的文件)
            var deletedFiles = await db.Queryable<tb_FS_BusinessRelation>()
                .InnerJoin<tb_FS_FileStorageInfo>((r, f) => r.FileId == f.FileId)
                .Where((r, f) => r.isdeleted == true)  // 以关联表为准
                .Where((r, f) => f.isdeleted == false) // 排除已物理删除的文件
                .OrderByDescending((r, f) => r.Modified_at)
                .Select((r, f) => new DeletedFileInfo
                {
                    RelationId = r.RelationId,
                    FileId = r.FileId,
                    OriginalFileName = f.OriginalFileName,
                    FileSize = f.FileSize,
                    StoragePath = f.StoragePath ?? "",
                    BusinessNo = r.BusinessNo ?? "",
                    BusinessId = r.BusinessId,
                    BusinessType = r.BusinessType,
                    RelatedField = r.RelatedField ?? "",
                    FileStatus = f.FileStatus,
                    DeletedTime = r.Modified_at ?? r.Created_at ?? DateTime.MinValue
                })
                .ToListAsync();

            _logger?.LogDebug("查询到的已删除文件数: {Count}", deletedFiles?.Count ?? 0);
            return deletedFiles ?? new List<DeletedFileInfo>();
        }

        /// <summary>
        /// 物理删除已删除的关联记录和对应的物理文件
        /// </summary>
        /// <param name="relationIds">要删除的关联记录ID列表</param>
        /// <returns>删除的文件数量</returns>
        public async Task<int> PhysicalDeleteDeletedFilesAsync(List<long> relationIds)
        {
            if (relationIds == null || relationIds.Count == 0)
                return 0;

            try
            {
                var db = _unitOfWorkManage.GetDbClient();
                int deletedCount = 0;

                // 查询要删除的关联记录
                var relationsToDelete = await db.Queryable<tb_FS_BusinessRelation>()
                    .Where(r => relationIds.Contains(r.RelationId))
                    .Where(r => r.isdeleted == true)
                    .ToListAsync();

                if (relationsToDelete == null || relationsToDelete.Count == 0)
                    return 0;

                // 获取关联的文件ID
                var fileIds = relationsToDelete.Select(r => r.FileId).Distinct().ToList();

                // 查询这些文件是否还有其他未删除的关联
                var stillReferencedFileIds = await db.Queryable<tb_FS_BusinessRelation>()
                    .Where(r => fileIds.Contains(r.FileId))
                    .Where(r => !relationIds.Contains(r.RelationId)) // 排除本次要删除的关联
                    .Where(r => r.isdeleted == false)
                    .Select(r => r.FileId)
                    .Distinct()
                    .ToListAsync();

                // 找出可以物理删除的文件（没有被其他未删除关联引用的文件）
                var filesToDelete = fileIds.Except(stillReferencedFileIds).ToList();

                // 物理删除文件和关联记录
                foreach (var fileId in filesToDelete)
                {
                    try
                    {
                        // 查询文件信息
                        var fileInfo = await db.Queryable<tb_FS_FileStorageInfo>()
                            .Where(f => f.FileId == fileId)
                            .FirstAsync();

                        if (fileInfo != null)
                        {
                            // 删除物理文件
                            if (!string.IsNullOrEmpty(fileInfo.StoragePath))
                            {
                                var filePath = FileStorageHelper.ResolveToAbsolutePath(fileInfo.StoragePath);
                                if (File.Exists(filePath))
                                {
                                    File.Delete(filePath);
                                }
                            }

                            // 逻辑删除文件记录（设置isdeleted=true）
                            fileInfo.isdeleted = true;
                            fileInfo.Modified_at = DateTime.Now;
                            await db.Updateable(fileInfo).ExecuteCommandAsync();
                            _logger?.LogInformation("逻辑删除文件记录成功, FileId: {FileId}", fileId);

                            deletedCount++;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "物理删除文件失败,FileId: {FileId}", fileId);
                    }
                }

                // 删除已删除的业务关联记录（物理删除）
                await db.Deleteable<tb_FS_BusinessRelation>()
                    .Where(r => relationIds.Contains(r.RelationId))
                    .ExecuteCommandAsync();

                return deletedCount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "物理删除已删除文件失败");
                return 0;
            }
        }

        /// <summary>
        /// 恢复已删除的文件关联
        /// </summary>
        /// <param name="relationIds">要恢复的关联记录ID列表</param>
        /// <returns>恢复的关联记录数量</returns>
        public async Task<int> RestoreDeletedFilesAsync(List<long> relationIds)
        {
            if (relationIds == null || relationIds.Count == 0)
                return 0;

            try
            {
                var db = _unitOfWorkManage.GetDbClient();

                // 查询要恢复的关联记录
                var relationsToRestore = await db.Queryable<tb_FS_BusinessRelation>()
                    .Where(r => relationIds.Contains(r.RelationId))
                    .Where(r => r.isdeleted == true)
                    .ToListAsync();

                if (relationsToRestore == null || relationsToRestore.Count == 0)
                    return 0;

                int restoredCount = 0;

                foreach (var relation in relationsToRestore)
                {
                    try
                    {
                        // 恢复关联记录（设置isdeleted=false）
                        relation.isdeleted = false;
                        relation.Modified_at = DateTime.Now;
                        await db.Updateable(relation).ExecuteCommandAsync();

                        // 检查该文件是否还有其他已删除的关联
                        var otherDeletedRelations = await db.Queryable<tb_FS_BusinessRelation>()
                            .Where(r => r.FileId == relation.FileId)
                            .Where(r => r.isdeleted == true)
                            .CountAsync();

                        // 恢复业务关联记录（设置isdeleted=false）
                        // 按方案A：恢复时只需要修改关联表的isdeleted字段，不需要修改文件状态

                        restoredCount++;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "恢复已删除文件关联失败,RelationId: {RelationId}", relation.RelationId);
                    }
                }

                return restoredCount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "恢复已删除文件关联失败");
                return 0;
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            // 释放数据库锁
            _dbLock?.Dispose();
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
        /// 已删除文件数 - 统计tb_FS_BusinessRelation.isdeleted=true的记录数
        /// 按方案A：删除逻辑由关联表isdeleted字段控制
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

    /// <summary>
    /// 已删除文件信息
    /// </summary>
    public class DeletedFileInfo
    {
        /// <summary>
        /// 关联记录ID
        /// </summary>
        public long RelationId { get; set; }

        /// <summary>
        /// 文件ID
        /// </summary>
        public long FileId { get; set; }

        /// <summary>
        /// 原始文件名
        /// </summary>
        public string OriginalFileName { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// 存储路径
        /// </summary>
        public string StoragePath { get; set; }

        /// <summary>
        /// 业务编号
        /// </summary>
        public string BusinessNo { get; set; }

        /// <summary>
        /// 业务主键ID
        /// </summary>
        public long? BusinessId { get; set; }

        /// <summary>
        /// 业务类型
        /// </summary>
        public int? BusinessType { get; set; }

        /// <summary>
        /// 关联字段
        /// </summary>
        public string RelatedField { get; set; }

        /// <summary>
        /// 文件状态
        /// </summary>
        public int FileStatus { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        public DateTime DeletedTime { get; set; }

        /// <summary>
        /// 文件大小(格式化)
        /// </summary>
        public string FileSizeFormatted => FormatFileSize(FileSize);

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

        /// <summary>
        /// 获取业务类型名称
        /// </summary>
        public string BusinessTypeName => BusinessType.HasValue ? GetBusinessTypeName(BusinessType.Value) : "未知";

        /// <summary>
        /// 获取文件状态名称
        /// </summary>
        public string FileStatusName => GetFileStatusName(FileStatus);

        private string GetBusinessTypeName(int businessType)
        {
            return businessType switch
            {
                1 => "产品图片",
                2 => "报销凭证",
                3 => "付款凭证",
                4 => "合同文件",
                5 => "技术文档",
                6 => "客户资料",
                7 => "财务报表",
                8 => "人事档案",
                9 => "采购文件",
                10 => "销售资料",
                _ => "其他文件"
            };
        }

        private string GetFileStatusName(int status)
        {
            return status switch
            {
                0 => "正常",
                1 => "已删除",
                2 => "过期",
                3 => "孤立",
                _ => "未知"
            };
        }
    }
}
