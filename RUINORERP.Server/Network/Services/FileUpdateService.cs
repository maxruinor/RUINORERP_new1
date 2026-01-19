using Microsoft.Extensions.Logging;
using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.Model.ConfigModel;
using RUINORERP.Repository.UnitOfWorks;
using RUINORERP.Server.Helpers;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static RUINORERP.Server.Network.Services.FileCleanupService;

namespace RUINORERP.Server.Network.Services
{
    /// <summary>
    /// 文件更新服务 - 处理图片的替换、版本管理和关联关系维护
    /// 支持三种更新策略:
    /// 1. 仅新增:保留旧文件,创建新关联
    /// 2. 替换模式:保留历史版本,更新关联到新文件
    /// 3. 覆盖模式:删除旧文件,创建新文件
    /// </summary>
    public class FileUpdateService
    {
       
        private readonly IUnitOfWorkManage _unitOfWorkManage;
        private readonly ILogger<FileUpdateService> _logger;
        private readonly ServerGlobalConfig _serverConfig;
        private readonly SemaphoreSlim _updateLock = new SemaphoreSlim(1, 1);

        /// <summary>
        /// 构造函数
        /// </summary>
        public FileUpdateService(
            IUnitOfWorkManage unitOfWorkManage,
            ILogger<FileUpdateService> logger,
            ServerGlobalConfig serverConfig)
        {
            _unitOfWorkManage = unitOfWorkManage;
            _logger = logger;
            _serverConfig = serverConfig;
        }

        /// <summary>
        /// 更新业务单据的图片
        /// </summary>
        /// <param name="businessType">业务类型</param>
        /// <param name="businessNo">业务编号</param>
        /// <param name="businessId">业务主键ID</param>
        /// <param name="relatedField">关联字段</param>
        /// <param name="newFileData">新文件数据</param>
        /// <param name="newFileName">新文件名</param>
        /// <param name="strategy">更新策略</param>
        /// <param name="userId">操作用户ID</param>
        /// <param name="isDetailTable">是否明细表</param>
        /// <param name="detailId">明细表ID</param>
        /// <returns>更新后的文件信息</returns>
        public async Task<(tb_FS_FileStorageInfo newFileInfo, tb_FS_BusinessRelation newRelation, List<tb_FS_FileStorageInfo> oldFiles)> UpdateBusinessFileAsync(
            int businessType,
            string businessNo,
            long businessId,
            string relatedField,
            byte[] newFileData,
            string newFileName,
            UpdateStrategy strategy = UpdateStrategy.AppendOnly,
            long? userId = null,
            bool isDetailTable = false,
            long? detailId = null)
        {
            if (!await _updateLock.WaitAsync(TimeSpan.FromMinutes(5)))
            {
                throw new TimeoutException("文件更新操作繁忙,请稍后重试");
            }

            try
            {
                var db = _unitOfWorkManage.GetDbClient();
                var oldFiles = new List<tb_FS_FileStorageInfo>();

                // 1. 查询当前关联的文件
                var currentRelations = await db.Queryable<tb_FS_BusinessRelation>()
                    .Where(r => r.BusinessType == businessType)
                    .Where(r => r.BusinessId == businessId)
                    .Where(r => r.RelatedField == relatedField)
                    .Where(r => r.IsDetailTable == isDetailTable)
                    .Where(r => !detailId.HasValue || r.DetailId == detailId.Value)
                    .Where(r => r.IsActive == true)
                    .Where(r => r.isdeleted == false)
                    .Includes(r => r.tb_fs_filestorageinfo)
                    .ToListAsync();

                // 2. 根据策略处理旧文件
                if (currentRelations.Count > 0)
                {
                    oldFiles = currentRelations.Select(r => r.tb_fs_filestorageinfo).Where(f => f != null).ToList();

                    switch (strategy)
                    {
                        case UpdateStrategy.Replace:
                            await HandleReplaceStrategyAsync(db, currentRelations, userId);
                            break;

                        case UpdateStrategy.Overwrite:
                            await HandleOverwriteStrategyAsync(db, currentRelations);
                            break;

                        case UpdateStrategy.AppendOnly:
                            // 不处理旧文件,保留所有关联
                            break;
                    }
                }

                // 3. 保存新文件
                var newFileInfo = await SaveNewFileAsync(newFileData, newFileName, businessType, userId);
                if (newFileInfo == null)
                {
                    throw new Exception("新文件保存失败");
                }

                // 4. 创建新的业务关联
                var newRelation = await CreateNewRelationAsync(
                    db,
                    newFileInfo.FileId,
                    businessType,
                    businessNo,
                    businessId,
                    relatedField,
                    userId,
                    isDetailTable,
                    detailId);

                _logger.LogInformation(
                    "文件更新成功,Strategy: {Strategy}, BusinessNo: {BusinessNo}, RelatedField: {RelatedField}, OldFileCount: {OldCount}, NewFileId: {NewFileId}",
                    strategy, businessNo, relatedField, oldFiles.Count, newFileInfo.FileId);

                return (newFileInfo, newRelation, oldFiles);
            }
            finally
            {
                _updateLock.Release();
            }
        }

        /// <summary>
        /// 批量更新业务单据的多个图片
        /// </summary>
        /// <param name="businessType">业务类型</param>
        /// <param name="businessNo">业务编号</param>
        /// <param name="businessId">业务主键ID</param>
        /// <param name="relatedField">关联字段</param>
        /// <param name="newFiles">新文件列表</param>
        /// <param name="strategy">更新策略</param>
        /// <param name="userId">操作用户ID</param>
        /// <param name="isDetailTable">是否明细表</param>
        /// <param name="detailId">明细表ID</param>
        /// <returns>更新结果</returns>
        public async Task<FileBatchUpdateResult> BatchUpdateBusinessFilesAsync(
            int businessType,
            string businessNo,
            long businessId,
            string relatedField,
            List<(byte[] fileData, string fileName)> newFiles,
            UpdateStrategy strategy = UpdateStrategy.AppendOnly,
            long? userId = null,
            bool isDetailTable = false,
            long? detailId = null)
        {
            var result = new FileBatchUpdateResult();
            var oldFiles = new List<tb_FS_FileStorageInfo>();
            var newFilesList = new List<tb_FS_FileStorageInfo>();

            if (!await _updateLock.WaitAsync(TimeSpan.FromMinutes(10)))
            {
                throw new TimeoutException("批量文件更新操作繁忙,请稍后重试");
            }

            try
            {
                var db = _unitOfWorkManage.GetDbClient();

                // 1. 查询并处理旧文件
                var currentRelations = await db.Queryable<tb_FS_BusinessRelation>()
                    .Where(r => r.BusinessType == businessType)
                    .Where(r => r.BusinessId == businessId)
                    .Where(r => r.RelatedField == relatedField)
                    .Where(r => r.IsDetailTable == isDetailTable)
                    .Where(r => !detailId.HasValue || r.DetailId == detailId.Value)
                    .Where(r => r.IsActive == true)
                    .Where(r => r.isdeleted == false)
                    .Includes(r => r.tb_fs_filestorageinfo)
                    .ToListAsync();

                if (currentRelations.Count > 0)
                {
                    oldFiles = currentRelations.Select(r => r.tb_fs_filestorageinfo).Where(f => f != null).ToList();

                    switch (strategy)
                    {
                        case UpdateStrategy.Replace:
                            await HandleReplaceStrategyAsync(db, currentRelations, userId);
                            break;

                        case UpdateStrategy.Overwrite:
                            await HandleOverwriteStrategyAsync(db, currentRelations);
                            break;

                        case UpdateStrategy.AppendOnly:
                            // 不处理旧文件
                            break;
                    }
                }

                // 2. 批量保存新文件
                foreach (var (fileData, fileName) in newFiles)
                {
                    try
                    {
                        var newFileInfo = await SaveNewFileAsync(fileData, fileName, businessType, userId);
                        if (newFileInfo != null)
                        {
                            newFilesList.Add(newFileInfo);

                            // 创建新关联
                            var relation = await CreateNewRelationAsync(
                                db,
                                newFileInfo.FileId,
                                businessType,
                                businessNo,
                                businessId,
                                relatedField,
                                userId,
                                isDetailTable,
                                detailId);

                            if (relation == null)
                            {
                                result.FailedFiles.Add(fileName);
                            }
                            else
                            {
                                result.SuccessFiles.Add(fileName);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "保存文件失败: {FileName}", fileName);
                        result.FailedFiles.Add(fileName);
                    }
                }

                result.OldFiles = oldFiles;
                result.NewFiles = newFilesList;

                _logger.LogInformation(
                    "批量文件更新完成,Strategy: {Strategy}, Success: {SuccessCount}, Failed: {FailedCount}",
                    strategy, result.SuccessFiles.Count, result.FailedFiles.Count);

                return result;
            }
            finally
            {
                _updateLock.Release();
            }
        }

        /// <summary>
        /// 处理替换策略 - 保留历史版本,更新关联到新文件
        /// </summary>
        private async Task HandleReplaceStrategyAsync(
            ISqlSugarClient db,
            List<tb_FS_BusinessRelation> currentRelations,
            long? userId)
        {
            foreach (var relation in currentRelations)
            {
                // 将旧关联标记为不活跃,但保留记录
                relation.IsActive = false;
                relation.Modified_at = DateTime.Now;
                relation.Modified_by = userId;
                await db.Updateable(relation).ExecuteCommandAsync();

                _logger.Debug(
                    "已标记旧关联为不活跃,RelationId: {RelationId}, FileId: {FileId}",
                    relation.RelationId, relation.FileId);
            }
        }

        /// <summary>
        /// 处理覆盖策略 - 删除旧文件
        /// </summary>
        private async Task HandleOverwriteStrategyAsync(
            ISqlSugarClient db,
            List<tb_FS_BusinessRelation> currentRelations)
        {
            foreach (var relation in currentRelations)
            {
                try
                {
                    // 删除关联记录
                    await db.Deleteable<tb_FS_BusinessRelation>()
                        .Where(r => r.RelationId == relation.RelationId)
                        .ExecuteCommandAsync();

                    // 删除物理文件
                    if (relation.tb_fs_filestorageinfo != null)
                    {
                        var filePath = FileStorageHelper.ResolveToAbsolutePath(relation.tb_fs_filestorageinfo.StoragePath);
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                            _logger.Debug("已删除物理文件: {FilePath}", filePath);
                        }

                        // 逻辑删除文件元数据
                        relation.tb_fs_filestorageinfo.isdeleted = true;
                        relation.tb_fs_filestorageinfo.Modified_at = DateTime.Now;
                        await db.Updateable(relation.tb_fs_filestorageinfo).ExecuteCommandAsync();
                    }

                    _logger.Debug(
                        "已删除旧文件及关联,RelationId: {RelationId}, FileId: {FileId}",
                        relation.RelationId, relation.FileId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "删除旧文件失败,RelationId: {RelationId}", relation.RelationId);
                }
            }
        }

        /// <summary>
        /// 保存新文件
        /// </summary>
        private async Task<tb_FS_FileStorageInfo> SaveNewFileAsync(
            byte[] fileData,
            string fileName,
            int? businessType,
            long? userId)
        {
            try
            {
                // 生成唯一文件名
                var fileId = Guid.NewGuid().ToString();
                var fileExtension = Path.GetExtension(fileName);
                var savedFileName = $"{fileId}{fileExtension}";

                // 确定存储路径
                var fileCreateTime = DateTime.Now;
                var categoryPath = GetCategoryPath(businessType?.ToString() ?? "General", fileCreateTime);
                if (!Directory.Exists(categoryPath))
                {
                    Directory.CreateDirectory(categoryPath);
                }

                var filePath = Path.Combine(categoryPath, savedFileName);

                // 计算哈希值
                var contentHash = FileManagementHelper.CalculateContentHash(fileData);

                // 保存物理文件
                await File.WriteAllBytesAsync(filePath, fileData);

                // 保存文件元数据
                var relativePath = FileStorageHelper.ConvertToRelativePath(filePath);
                var fileInfo = new tb_FS_FileStorageInfo
                {
                    OriginalFileName = fileName,
                    StorageFileName = savedFileName,
                    FileExtension = fileExtension.TrimStart('.'),
                    FileSize = fileData.Length,
                    FileType = System.Net.Mime.MediaTypeNames.Application.Octet,
                    BusinessType = businessType,
                    StorageProvider = "Local",
                    StoragePath = relativePath,
                    HashValue = contentHash,
                    CurrentVersion = 1,
                    FileStatus = (int)FileStatus.Active, // 正常状态
                    ExpireTime = DateTime.MaxValue,
                    Created_at = DateTime.Now,
                    Created_by = userId,
                    Modified_at = DateTime.Now,
                    Modified_by = userId
                };

                await _unitOfWorkManage.GetDbClient().Insertable(fileInfo).ExecuteCommandAsync();

                _logger.Debug("新文件保存成功,FileId: {FileId}, FileName: {FileName}",
                    fileInfo.FileId, fileName);

                return fileInfo;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "保存新文件失败,FileName: {FileName}", fileName);
                return null;
            }
        }

        /// <summary>
        /// 创建新的业务关联
        /// </summary>
        private async Task<tb_FS_BusinessRelation> CreateNewRelationAsync(
            ISqlSugarClient db,
            long fileId,
            int businessType,
            string businessNo,
            long businessId,
            string relatedField,
            long? userId,
            bool isDetailTable,
            long? detailId)
        {
            try
            {
                var relation = new tb_FS_BusinessRelation
                {
                    FileId = fileId,
                    BusinessType = businessType,
                    BusinessNo = businessNo,
                    BusinessId = businessId,
                    RelatedField = relatedField,
                    IsDetailTable = isDetailTable,
                    DetailId = detailId,
                    IsActive = true,
                    VersionNo = 1,
                    IsMainFile = true,
                    Created_at = DateTime.Now,
                    Created_by = userId,
                    Modified_at = DateTime.Now,
                    Modified_by = userId
                };

                await db.Insertable(relation).ExecuteCommandAsync();

                _logger.Debug("新关联创建成功,RelationId: {RelationId}, FileId: {FileId}",
                    relation.RelationId, fileId);

                return relation;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "创建新关联失败,FileId: {FileId}", fileId);
                return null;
            }
        }

        /// <summary>
        /// 获取分类存储路径
        /// </summary>
        private string GetCategoryPath(string bizCategory, DateTime? dateTime = null)
        {
            var now = dateTime ?? DateTime.Now;
            var timeFolder = now.ToString("yyyyMM");
            var storagePath = FileStorageHelper.GetStoragePath();
            return Path.Combine(storagePath, bizCategory, timeFolder);
        }

        /// <summary>
        /// 获取文件的版本历史
        /// </summary>
        /// <param name="businessType">业务类型</param>
        /// <param name="businessId">业务主键ID</param>
        /// <param name="relatedField">关联字段</param>
        /// <returns>版本历史列表</returns>
        public async Task<List<FileVersionHistory>> GetFileVersionHistoryAsync(
            int businessType,
            long businessId,
            string relatedField)
        {
            try
            {
                var db = _unitOfWorkManage.GetDbClient();

                var relations = await db.Queryable<tb_FS_BusinessRelation>()
                    .Where(r => r.BusinessType == businessType)
                    .Where(r => r.BusinessId == businessId)
                    .Where(r => r.RelatedField == relatedField)
                    .Where(r => r.isdeleted == false)
                    .OrderBy(r => r.RelationId, OrderByType.Desc)
                    .Includes(r => r.tb_fs_filestorageinfo)
                    .ToListAsync();

                var history = relations.Select(r => new FileVersionHistory
                {
                    FileId = r.FileId,
                    FileName = r.tb_fs_filestorageinfo?.OriginalFileName,
                    VersionNo = r.VersionNo,
                    IsActive = r.IsActive,
                    Created_at = r.Created_at,
                    Created_by = r.Created_by,
                    Modified_at = r.Modified_at,
                    Modified_by = r.Modified_by
                }).ToList();

                return history;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取文件版本历史失败");
                return new List<FileVersionHistory>();
            }
        }
    }

    /// <summary>
    /// 批量文件更新结果
    /// </summary>
    public class FileBatchUpdateResult
    {
        /// <summary>
        /// 成功的文件列表
        /// </summary>
        public List<string> SuccessFiles { get; set; } = new List<string>();

        /// <summary>
        /// 失败的文件列表
        /// </summary>
        public List<string> FailedFiles { get; set; } = new List<string>();

        /// <summary>
        /// 旧文件列表
        /// </summary>
        public List<tb_FS_FileStorageInfo> OldFiles { get; set; } = new List<tb_FS_FileStorageInfo>();

        /// <summary>
        /// 新文件列表
        /// </summary>
        public List<tb_FS_FileStorageInfo> NewFiles { get; set; } = new List<tb_FS_FileStorageInfo>();
    }

    /// <summary>
    /// 文件版本历史
    /// </summary>
    public class FileVersionHistory
    {
        /// <summary>
        /// 文件ID
        /// </summary>
        public long FileId { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public int VersionNo { get; set; }

        /// <summary>
        /// 是否活跃
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? Created_at { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public long? Created_by { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? Modified_at { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public long? Modified_by { get; set; }
    }
}
