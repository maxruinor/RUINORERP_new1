using System;
using System.Threading.Tasks;
using RUINORERP.Model;
using RUINORERP.Business;
using System.IO;
using Microsoft.Extensions.Logging;
using RUINORERP.IServices;

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
        /// <param name="businessId">业务ID</param>
        /// <param name="userId">用户ID</param>
        /// <returns>文件存储信息实体</returns>
        public static tb_FS_FileStorageInfo CreateFileStorageInfo(
            string fileName, 
            long fileSize, 
            string fileType, 
            string storagePath, 
            int businessType, 
            long userId)
        {
            var fileStorageInfo = new tb_FS_FileStorageInfo
            {
                OriginalFileName = fileName,
                StorageFileName = GenerateStorageFileName(fileName),
                BusinessType = businessType,
                FileType = fileType,
                FileSize = fileSize,
                StorageProvider = "Local",
                StoragePath = storagePath,
                CurrentVersion = 1,
                Status = 1, // 1表示正常状态
                ExpireTime = DateTime.Now.AddYears(10), // 默认10年过期
                Created_at = DateTime.Now,
                Created_by = userId,
                Modified_at = DateTime.Now,
                Modified_by = userId
            };

            return fileStorageInfo;
        }

        /// <summary>
        /// 创建业务关联实体
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <param name="businessType">业务类型</param>
        /// <param name="businessNo">业务编号</param>
        /// <param name="isMainFile">是否为主文件</param>
        /// <returns>业务关联实体</returns>
        public static tb_FS_BusinessRelation CreateBusinessRelation(
            long fileId, 
            int businessType, 
            string businessNo, 
            bool isMainFile = true)
        {
            var businessRelation = new tb_FS_BusinessRelation
            {
                FileId = fileId,
                BusinessType = businessType,
                BusinessNo = businessNo,
                IsMainFile = isMainFile,
                Created_at = DateTime.Now
            };

            return businessRelation;
        }

        /// <summary>
        /// 创建文件版本实体
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <param name="versionNo">版本号</param>
        /// <param name="storageFileName">存储文件名</param>
        /// <param name="updateReason">更新原因</param>
        /// <param name="hashValue">哈希值</param>
        /// <param name="userId">用户ID</param>
        /// <returns>文件版本实体</returns>
        public static tb_FS_FileStorageVersion CreateFileStorageVersion(
            long fileId, 
            int versionNo, 
            string storageFileName, 
            string updateReason, 
            string hashValue, 
            long userId)
        {
            var fileStorageVersion = new tb_FS_FileStorageVersion
            {
                FileId = fileId,
                VersionNo = versionNo,
                StorageFileName = storageFileName,
                UpdateReason = updateReason,
                HashValue = hashValue,
                Modified_at = DateTime.Now,
                Modified_by = userId
            };

            return fileStorageVersion;
        }

        /// <summary>
        /// 生成存储文件名
        /// </summary>
        /// <param name="originalFileName">原始文件名</param>
        /// <returns>存储文件名</returns>
        public static string GenerateStorageFileName(string originalFileName)
        {
            var fileExtension = Path.GetExtension(originalFileName);
            var fileId = Guid.NewGuid().ToString("N");
            return $"{fileId}{fileExtension}";
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
                using (var stream = File.OpenRead(filePath))
                {
                    var sha256 = System.Security.Cryptography.SHA256.Create();
                    var hashBytes = sha256.ComputeHash(stream);
                    return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
                }
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
        /// <param name="logger">日志记录器</param>
        /// <returns>是否保存成功</returns>
        public static async Task<bool> SaveBusinessRelationAsync(
            tb_FS_BusinessRelation businessRelation,
            tb_FS_BusinessRelationController<tb_FS_BusinessRelation> businessRelationController,
            ILogger logger = null)
        {
            try
            {
                var result = await businessRelationController.SaveOrUpdate(businessRelation);
                return result.Succeeded;
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "保存业务关联信息到数据库时出错");
                return false;
            }
        }

        /// <summary>
        /// 保存文件版本信息到数据库
        /// </summary>
        /// <param name="fileStorageVersion">文件版本信息</param>
        /// <param name="fileStorageVersionController">文件版本控制器</param>
        /// <param name="logger">日志记录器</param>
        /// <returns>是否保存成功</returns>
        public static async Task<bool> SaveFileStorageVersionAsync(
            tb_FS_FileStorageVersion fileStorageVersion,
            tb_FS_FileStorageVersionController<tb_FS_FileStorageVersion> fileStorageVersionController,
            ILogger logger = null)
        {
            try
            {
                var result = await fileStorageVersionController.SaveOrUpdate(fileStorageVersion);
                return result.Succeeded;
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "保存文件版本信息到数据库时出错");
                return false;
            }
        }
    }
}