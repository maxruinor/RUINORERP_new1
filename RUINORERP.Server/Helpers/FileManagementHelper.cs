using System;
using System.Threading.Tasks;
using RUINORERP.Model;
using RUINORERP.Business;
using System.IO;
using Microsoft.Extensions.Logging;
using RUINORERP.IServices;
using System.Linq;
using System.Collections.Generic;

namespace RUINORERP.Server.Helpers
{
    /// <summary>
    /// 文件管理帮助类
    /// 提供文件与数据库实体之间的转换和操作
    /// </summary>
    public static class FileManagementHelper
    {
        /// <summary>
        /// 创建文件存储信息实体
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="fileSize">文件大小</param>
        /// <param name="fileType">文件类型</param>
        /// <param name="storagePath">存储路径</param>
        /// <param name="businessType">业务类型</param>
        /// <param name="userId">用户ID</param>
        /// <param name="contentHash">文件内容哈希值（可选）</param>
        /// <returns>文件存储信息实体</returns>
        public static tb_FS_FileStorageInfo CreateFileStorageInfo(
            string fileName, 
            long fileSize, 
            string fileType, 
            string storagePath, 
            int businessType, 
            long userId,
            string contentHash = null)
        {
            
            // 如果未提供内容哈希值，则使用传统的哈希码生成方式
            string hashValue = contentHash ?? GenerateHashCode(fileName, fileSize, fileType);
            
            var fileExtension = Path.GetExtension(fileName).TrimStart('.').ToLower();
            
            var fileStorageInfo = new tb_FS_FileStorageInfo
            {
                OriginalFileName = fileName,
                StorageFileName = GenerateStorageFileName(fileName),
                BusinessType = businessType,
                FileType = fileType,
                FileExtension = fileExtension, // 设置文件扩展名
                FileSize = fileSize,
                HashValue = hashValue,
               // ContentHash = contentHash, // 存储基于内容的哈希值
                StorageProvider = "Local",
                StoragePath = storagePath,
                CurrentVersion = 1,
                Status = 1, // 1表示正常状态
                ExpireTime = DateTime.Now.AddYears(10), // 默认10年过期
                Created_at = DateTime.Now,
                Created_by = userId,
                Modified_at = DateTime.Now,
                Modified_by = userId,
            };

            return fileStorageInfo;
        }


        /// <summary>
        /// 生成文件哈希值（基于文件名、大小、类型）- 为兼容旧代码保留
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="fileSize">文件大小</param>
        /// <param name="fileType">文件类型</param>
        /// <returns>哈希字符串</returns>
        public static string GenerateHashCode(string fileName, long fileSize, string fileType)
        {
            var raw = $"{fileName}|{fileSize}|{fileType}";
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(raw));
                return BitConverter.ToString(bytes).Replace("-", "").ToLowerInvariant();
            }
        }

        /// <summary>
        /// 基于文件内容计算哈希值
        /// </summary>
        /// <param name="fileBytes">文件字节数组</param>
        /// <returns>文件内容哈希值</returns>
        public static string CalculateContentHash(byte[] fileBytes)
        {
            if (fileBytes == null || fileBytes.Length == 0)
            {
                throw new ArgumentException("文件字节数组不能为空", nameof(fileBytes));
            }

            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var hashBytes = sha256.ComputeHash(fileBytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
            }
        }


        /// <summary>
        /// 创建业务关联实体
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <param name="businessType">业务类型</param>
        /// <param name="businessNo">业务编号</param>
        /// <param name="isMainFile">是否为主文件</param>
        /// <param name="relatedField">关联字段名（可选）</param>
        /// <param name="description">描述信息（可选）</param>
        /// <param name="userId">用户ID（可选）</param>
        /// <returns>业务关联实体</returns>
        public static tb_FS_BusinessRelation CreateBusinessRelation(
            long fileId, 
            int businessType, 
            string businessNo, 
            bool isMainFile = true,
            string relatedField = null,
            string description = null,
            long userId = 0)
        {
            var businessRelation = new tb_FS_BusinessRelation
            {
                FileId = fileId,
                BusinessType = businessType,
                BusinessNo = businessNo,
                IsMainFile = isMainFile,
                RelatedField = relatedField,
                Created_by = userId,
                Created_at = DateTime.Now,
                IsActive = true // 默认为激活状态
            };

            return businessRelation;
        }

        /// <summary>
        /// 检查业务关联是否已存在
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <param name="businessType">业务类型</param>
        /// <param name="businessNo">业务编号</param>
        /// <param name="businessRelationController">业务关联控制器</param>
        /// <returns>是否存在</returns>
        public static async Task<bool> CheckBusinessRelationExistsAsync(
            long fileId,
            int businessType,
            string businessNo,
            tb_FS_BusinessRelationController<tb_FS_BusinessRelation> businessRelationController)
        {
            var relations = await businessRelationController.BaseQueryAsync(
                $"FileId = {fileId} AND BusinessType = {businessType} AND BusinessNo = '{businessNo}'");
            return relations != null && relations.Count > 0;
        }

        /// <summary>
        /// 获取指定业务的所有关联文件
        /// </summary>
        /// <param name="businessType">业务类型</param>
        /// <param name="businessNo">业务编号</param>
        /// <param name="businessRelationController">业务关联控制器</param>
        /// <returns>业务关联列表</returns>
        public static async Task<List<tb_FS_BusinessRelation>> GetBusinessRelationsAsync(
            int businessType,
            string businessNo,
            tb_FS_BusinessRelationController<tb_FS_BusinessRelation> businessRelationController)
        {
            var relations = await businessRelationController.BaseQueryAsync(
                $"BusinessType = {businessType} AND BusinessNo = '{businessNo}' AND IsActive = 1");
            return relations?.Cast<tb_FS_BusinessRelation>().ToList() ?? new List<tb_FS_BusinessRelation>();
        }

        /// <summary>
        /// 创建文件版本实体
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <param name="versionNo">版本号（如果为0，将自动计算）</param>
        /// <param name="updateReason">更新原因</param>
        /// <param name="userId">用户ID</param>
        /// <returns>文件版本实体</returns>
        public static tb_FS_FileStorageVersion CreateFileStorageVersion(
            long fileId, 
            int versionNo, 
            string updateReason, 
            long userId)
        {
            var fileStorageVersion = new tb_FS_FileStorageVersion
            {
                FileId = fileId,
                VersionNo = versionNo,
                UpdateReason = updateReason,
                Modified_at = DateTime.Now,
                Modified_by = userId
            };

            return fileStorageVersion;
        }

        /// <summary>
        /// 自动计算新版本号
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <param name="fileStorageVersionController">文件版本控制器</param>
        /// <returns>新版本号</returns>
        public static async Task<int> CalculateNewVersionNoAsync(
            long fileId,
            tb_FS_FileStorageVersionController<tb_FS_FileStorageVersion> fileStorageVersionController)
        {
            // 查询该文件的所有版本，获取最大版本号
            var versions = await fileStorageVersionController.BaseQueryAsync($"FileId = {fileId}");
            if (versions != null && versions.Count > 0)
            {
                // 转换为具体类型并获取最大版本号
                var maxVersion = versions.Cast<tb_FS_FileStorageVersion>().Max(v => v.VersionNo);
                return maxVersion + 1;
            }
            return 1; // 默认从1开始
        }

        /// <summary>
        /// 生成存储文件名
        /// 不带扩展名
        /// </summary>
        /// <param name="originalFileName">原始文件名</param>
        /// <returns>存储文件名</returns>
        public static string GenerateStorageFileName(string originalFileName)
        {
           // var fileExtension = Path.GetExtension(originalFileName);
            var fileId = Guid.NewGuid().ToString("N");
            return $"{fileId}";
            //return $"{fileId}{fileExtension}";
        }

        /// <summary>
        /// 计算文件哈希值
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>文件哈希值</returns>
        public static string CalculateFileHash(string filePath)
        {
            try
            {
                byte[] fileBytes = File.ReadAllBytes(filePath);
                return CalculateContentHash(fileBytes);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 保存文件信息到数据库
        /// </summary>
        /// <param name="fileStorageInfo">文件存储信息</param>
        /// <param name="fileStorageInfoController">文件存储信息控制器</param>
        /// <param name="logger">日志记录器</param>
        /// <returns>是否保存成功</returns>
        public static async Task<bool> SaveFileStorageInfoAsync(
            tb_FS_FileStorageInfo fileStorageInfo,
            tb_FS_FileStorageInfoController<tb_FS_FileStorageInfo> fileStorageInfoController,
            ILogger logger = null)
        {
            try
            {
                var result = await fileStorageInfoController.SaveOrUpdate(fileStorageInfo);
                return result.Succeeded;
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "保存文件存储信息到数据库时出错");
                return false;
            }
        }

        /// <summary>
        /// 保存业务关联信息到数据库
        /// </summary>
        /// <param name="businessRelation">业务关联信息</param>
        /// <param name="businessRelationController">业务关联控制器</param>
        /// <param name="fileStorageInfoController">文件存储信息控制器（用于验证文件存在性）</param>
        /// <param name="allowDuplicate">是否允许重复关联</param>
        /// <param name="setAsMain">如果是同一业务的新关联，是否自动设置为主文件</param>
        /// <param name="logger">日志记录器</param>
        /// <returns>是否保存成功</returns>
        public static async Task<bool> SaveBusinessRelationAsync(
            tb_FS_BusinessRelation businessRelation,
            tb_FS_BusinessRelationController<tb_FS_BusinessRelation> businessRelationController,
            tb_FS_FileStorageInfoController<tb_FS_FileStorageInfo> fileStorageInfoController,
            bool allowDuplicate = false,
            bool setAsMain = true,
            ILogger logger = null)
        {
            try
            {
                // 验证参数
                if (businessRelation == null)
                {
                    logger?.LogWarning("业务关联信息为空，保存失败");
                    return false;
                }

                if (businessRelation.FileId <= 0 || string.IsNullOrEmpty(businessRelation.BusinessNo))
                {
                    logger?.LogWarning("文件ID或业务编号为空，保存失败");
                    return false;
                }

                // 验证文件是否存在
                var fileExists = await ValidateFileExistsAsync(
                    businessRelation.FileId, 
                    fileStorageInfoController,
                    logger);
                if (!fileExists)
                {
                    logger?.LogWarning("关联的文件不存在: FileId={FileId}", businessRelation.FileId);
                    return false;
                }

                // 检查是否重复关联
                if (!allowDuplicate)
                {
                    var exists = await CheckBusinessRelationExistsAsync(
                        businessRelation.FileId,
                        businessRelation.BusinessType,
                        businessRelation.BusinessNo,
                        businessRelationController);
                    if (exists)
                    {
                        logger?.LogWarning("业务关联已存在: FileId={FileId}, BusinessType={BusinessType}, BusinessNo={BusinessNo}",
                            businessRelation.FileId, businessRelation.BusinessType, businessRelation.BusinessNo);
                        return false;
                    }
                }

                // 如果设置为主文件，先将该业务的其他主文件设置为非主文件
                if (businessRelation.IsMainFile && setAsMain)
                {
                    await UpdateOtherMainFilesAsync(
                        businessRelation.BusinessType,
                        businessRelation.BusinessNo,
                        businessRelation.FileId, // 排除当前文件
                        businessRelationController,
                        logger);
                }

                // 保存业务关联
                var result = await businessRelationController.SaveOrUpdate(businessRelation);
                logger?.LogInformation("业务关联保存成功: FileId={FileId}, BusinessType={BusinessType}, BusinessNo={BusinessNo}",
                    businessRelation.FileId, businessRelation.BusinessType, businessRelation.BusinessNo);
                
                return result.Succeeded;
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "保存业务关联信息到数据库时出错: FileId={FileId}, BusinessType={BusinessType}, BusinessNo={BusinessNo}",
                    businessRelation?.FileId, businessRelation?.BusinessType, businessRelation?.BusinessNo);
                return false;
            }
        }

        /// <summary>
        /// 批量保存业务关联
        /// </summary>
        /// <param name="businessRelations">业务关联列表</param>
        /// <param name="businessRelationController">业务关联控制器</param>
        /// <param name="fileStorageInfoController">文件存储信息控制器</param>
        /// <param name="logger">日志记录器</param>
        /// <returns>保存结果字典，key为关联ID，value为是否保存成功</returns>
        public static async Task<Dictionary<long, bool>> BatchSaveBusinessRelationsAsync(
            List<tb_FS_BusinessRelation> businessRelations,
            tb_FS_BusinessRelationController<tb_FS_BusinessRelation> businessRelationController,
            tb_FS_FileStorageInfoController<tb_FS_FileStorageInfo> fileStorageInfoController,
            ILogger logger = null)
        {
            var results = new Dictionary<long, bool>();
            
            foreach (var relation in businessRelations)
            {
                bool success = await SaveBusinessRelationAsync(
                    relation,
                    businessRelationController,
                    fileStorageInfoController,
                    allowDuplicate: false,
                    setAsMain: true,
                    logger);
                
                results[relation.RelationId] = success;
            }
            
            return results;
        }

        /// <summary>
        /// 验证文件是否存在
        /// </summary>
        private static async Task<bool> ValidateFileExistsAsync(
            long fileId,
            tb_FS_FileStorageInfoController<tb_FS_FileStorageInfo> fileController,
            ILogger logger = null)
        {
            try
            {
                var fileInfos = await fileController.BaseQueryAsync($"FileId = {fileId} AND Status = 1");
                return fileInfos != null && fileInfos.Count > 0;
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "验证文件存在性时出错: FileId={FileId}", fileId);
                return false;
            }
        }

        /// <summary>
        /// 更新同一业务的其他主文件为非主文件
        /// </summary>
        private static async Task UpdateOtherMainFilesAsync(
            int businessType,
            string businessNo,
            long excludeFileId,
            tb_FS_BusinessRelationController<tb_FS_BusinessRelation> relationController,
            ILogger logger = null)
        {
            try
            {
                var otherRelations = await relationController.BaseQueryAsync(
                    $"BusinessType = {businessType} AND BusinessNo = '{businessNo}' AND IsMainFile = 1 AND FileId != {excludeFileId} AND IsActive = 1");
                
                foreach (var relation in otherRelations)
                {
                    var businessRelation = relation as tb_FS_BusinessRelation;
                    if (businessRelation != null)
                    {
                        businessRelation.IsMainFile = false;
                        await relationController.SaveOrUpdate(businessRelation);
                        logger?.LogInformation("已将原主文件设置为非主文件: RelationId={RelationId}, FileId={FileId}",
                            businessRelation.RelationId, businessRelation.FileId);
                    }
                }
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "更新其他主文件状态时出错: BusinessType={BusinessType}, BusinessNo={BusinessNo}",
                    businessType, businessNo);
                // 不抛出异常，避免影响主要操作
            }
        }

        /// <summary>
        /// 保存文件版本信息到数据库
        /// </summary>
        /// <param name="fileStorageVersion">文件版本信息</param>
        /// <param name="fileStorageVersionController">文件版本控制器</param>
        /// <param name="fileStorageInfoController">文件存储信息控制器（用于更新主文件当前版本）</param>
        /// <param name="maxVersions">保留的最大版本数（默认50）</param>
        /// <param name="logger">日志记录器</param>
        /// <returns>是否保存成功</returns>
        public static async Task<bool> SaveFileStorageVersionAsync(
            tb_FS_FileStorageVersion fileStorageVersion,
            tb_FS_FileStorageVersionController<tb_FS_FileStorageVersion> fileStorageVersionController,
            tb_FS_FileStorageInfoController<tb_FS_FileStorageInfo> fileStorageInfoController,
            int maxVersions = 50,
            ILogger logger = null)
        {
            try
            {
                // 如果版本号为0，则自动计算新版本号
                if (fileStorageVersion.VersionNo <= 0)
                {
                    fileStorageVersion.VersionNo = await CalculateNewVersionNoAsync(
                        fileStorageVersion.FileId, 
                        fileStorageVersionController);
                }

                // 开始事务处理
                fileStorageVersionController._unitOfWorkManage.BeginTran();
                {
                    try
                    {
                        // 将该文件的所有其他版本设置为非激活，确保新版本为当前活跃版本
                        await DeactivateOldVersionsAsync(
                            fileStorageVersion.FileId,
                            fileStorageVersionController,
                            logger);

                        // 保存新版本
                var result = await fileStorageVersionController.SaveOrUpdate(fileStorageVersion);
                        if (!result.Succeeded)
                        {
                            fileStorageVersionController._unitOfWorkManage.RollbackTran();
                            logger?.LogWarning("保存文件版本失败: {ErrorMessage}", result.ErrorMsg);
                            return false;
                        }

                        // 更新主文件的当前版本号
                        await UpdateFileCurrentVersionAsync(
                            fileStorageVersion.FileId,
                            fileStorageVersion.VersionNo,
                            fileStorageInfoController,
                            logger);

                        // 清理旧版本，保留最近的maxVersions个版本
                        await CleanupOldVersionsAsync(
                            fileStorageVersion.FileId,
                            fileStorageVersionController,
                            maxVersions,
                            logger);

                        fileStorageVersionController._unitOfWorkManage.CommitTran();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        fileStorageVersionController._unitOfWorkManage.RollbackTran();
                        logger?.LogError(ex, "保存文件版本时发生事务错误");
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "保存文件版本信息到数据库时出错");
                return false;
            }
        }

        /// <summary>
        /// 版本控制逻辑已调整：不再使用IsActive标记
        /// 新版本始终为当前版本，通过VersionNo标识最新版本
        /// </summary>
        private static async Task DeactivateOldVersionsAsync(
            long fileId,
            tb_FS_FileStorageVersionController<tb_FS_FileStorageVersion> versionController,
            ILogger logger = null)
        {
            try
            {
                // 获取该文件的所有版本
                var versions = await versionController.BaseQueryAsync($"FileId = {fileId}");
                // 不再需要设置IsActive属性，版本控制通过VersionNo实现
                // 新版本总是通过UpdateFileCurrentVersionAsync设置为当前版本
                logger?.LogInformation("处理文件版本控制: FileId={FileId}, 版本数={VersionCount}", 
                    fileId, versions.Count);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "处理文件版本时出错");
                throw;
            }
        }

        /// <summary>
        /// 更新文件存储信息中的当前版本号
        /// </summary>
        private static async Task UpdateFileCurrentVersionAsync(
            long fileId,
            int currentVersion,
            tb_FS_FileStorageInfoController<tb_FS_FileStorageInfo> fileController,
            ILogger logger = null)
        {
            try
            {
                var fileInfo = await fileController.BaseQueryAsync($"FileId = {fileId}");
                if (fileInfo != null && fileInfo.Count > 0)
                {
                    var file = fileInfo[0] as tb_FS_FileStorageInfo;
                    if (file != null)
                    {
                        file.CurrentVersion = currentVersion;
                        await fileController.SaveOrUpdate(file);
                    }
                }
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "更新文件当前版本号时出错");
                throw;
            }
        }

        /// <summary>
        /// 清理旧版本，保留最近的指定数量版本
        /// </summary>
        private static async Task CleanupOldVersionsAsync(
            long fileId,
            tb_FS_FileStorageVersionController<tb_FS_FileStorageVersion> versionController,
            int maxVersions,
            ILogger logger = null)
        {
            try
            {
                // 获取所有版本，按版本号降序排列
                var versions = await versionController.BaseQueryAsync($"FileId = {fileId} ORDER BY VersionNo DESC");
                var fileVersions = versions.Cast<tb_FS_FileStorageVersion>().ToList();

                // 如果版本数量超过限制，删除最旧的版本
                if (fileVersions.Count > maxVersions)
                {
                    var versionsToDelete = fileVersions.Skip(maxVersions).ToList();
                    foreach (var version in versionsToDelete)
                    {
                        // 不再需要检查IsActive属性，直接删除超出限制的旧版本
                        await versionController.BaseDeleteAsync(version);
                        logger?.LogInformation("已删除旧文件版本: FileId={FileId}, VersionNo={VersionNo}", 
                            version.FileId, version.VersionNo);
                    }
                }
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "清理旧版本时出错");
                // 清理失败不应影响主要操作，记录错误后继续
            }
        }
    }
}