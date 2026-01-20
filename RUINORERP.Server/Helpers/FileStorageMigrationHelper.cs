using System;
using System.IO;
using System.Threading.Tasks;
using RUINORERP.Model;
using RUINORERP.Business;
using RUINORERP.Model.ConfigModel;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using RUINORERP.Global;

namespace RUINORERP.Server.Helpers
{
    /// <summary>
    /// 文件存储路径迁移帮助类
    /// 用于将现有数据库中的绝对路径转换为相对路径
    /// </summary>
    public static class FileStorageMigrationHelper
    {
        /// <summary>
        /// 迁移所有文件存储路径为相对路径
        /// </summary>
        /// <param name="serverConfig">服务器配置</param>
        /// <param name="fileStorageInfoController">文件信息控制器</param>
        /// <param name="logger">日志记录器</param>
        /// <returns>迁移结果统计</returns>
        public static async Task<MigrationResult> MigrateToRelativePathsAsync(
            ServerGlobalConfig serverConfig,
            tb_FS_FileStorageInfoController<tb_FS_FileStorageInfo> fileStorageInfoController,
            ILogger logger = null)
        {
            var result = new MigrationResult();

            if (serverConfig == null || string.IsNullOrEmpty(serverConfig.FileStoragePath))
            {
                logger?.LogError("服务器配置或文件存储路径为空，无法执行迁移");
                result.Success = false;
                result.ErrorMessage = "服务器配置或文件存储路径为空";
                return result;
            }

            try
            {
                var resolvedRootPath = FileStorageHelper.ResolveEnvironmentVariables(serverConfig.FileStoragePath);

                // 获取所有文件记录
                var fileInfos = await fileStorageInfoController.QueryByNavAsync(c => c.FileStatus == (int)FileStatus.Active);

                if (fileInfos == null || fileInfos.Count == 0)
                {

                    result.Success = true;
                    result.Message = "没有找到需要迁移的文件记录";
                    return result;
                }

                foreach (var fileInfoObj in fileInfos)
                {
                    if (fileInfoObj is tb_FS_FileStorageInfo fileInfo)
                    {
                        await ProcessFileRecord(fileInfo, resolvedRootPath, fileStorageInfoController, result, logger);
                    }
                }

                result.Success = true;
                result.Message = $"文件存储路径迁移完成。成功迁移: {result.SuccessfulMigrations}，跳过: {result.SkippedRecords}，失败: {result.FailedMigrations}";

                return result;
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "文件存储路径迁移失败");
                result.Success = false;
                result.ErrorMessage = $"迁移失败: {ex.Message}";
                return result;
            }
        }

        /// <summary>
        /// 处理单个文件记录
        /// </summary>
        private static async Task ProcessFileRecord(
            tb_FS_FileStorageInfo fileInfo,
            string rootPath,
            tb_FS_FileStorageInfoController<tb_FS_FileStorageInfo> controller,
            MigrationResult result,
            ILogger logger)
        {
            try
            {
                // 检查是否已经是相对路径
                if (string.IsNullOrEmpty(fileInfo.StoragePath) || !Path.IsPathRooted(fileInfo.StoragePath))
                {
                    logger?.LogDebug("文件 {FileId} 的路径已经是相对路径或为空，跳过", fileInfo.FileId);
                    result.SkippedRecords++;
                    return;
                }

                // 检查路径是否在根目录下
                var rootUri = new Uri(rootPath.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar);
                var fileUri = new Uri(fileInfo.StoragePath);

                if (!rootUri.IsBaseOf(fileUri))
                {
                    logger?.LogWarning("文件 {FileId} 的路径不在根目录下，跳过: {FilePath}", fileInfo.FileId, fileInfo.StoragePath);
                    result.SkippedRecords++;
                    return;
                }

                // 转换为相对路径
                var relativePath = FileStorageHelper.ConvertToRelativePath(fileInfo.StoragePath);

                // 验证转换后的路径是否有效
                if (string.IsNullOrEmpty(relativePath) || relativePath == fileInfo.StoragePath)
                {
                    logger?.LogWarning("文件 {FileId} 路径转换失败，原路径: {OriginalPath}", fileInfo.FileId, fileInfo.StoragePath);
                    result.FailedMigrations++;
                    return;
                }

                // 验证文件是否存在
                var absolutePath = FileStorageHelper.ResolveToAbsolutePath(relativePath);
                if (!File.Exists(absolutePath))
                {
                    logger?.LogWarning("文件 {FileId} 在转换后的路径中不存在，跳过", fileInfo.FileId);
                    result.FailedMigrations++;
                    return;
                }

                // 更新数据库记录
                var originalPath = fileInfo.StoragePath;
                fileInfo.StoragePath = relativePath;

                var updateResult = await controller.SaveOrUpdate(fileInfo);
                if (updateResult.Succeeded)
                {
                    result.SuccessfulMigrations++;
                }
                else
                {
                    logger?.LogError("文件 {FileId} 数据库更新失败: {Error}", fileInfo.FileId, updateResult.ErrorMsg);
                    result.FailedMigrations++;
                }
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "处理文件 {FileId} 时发生错误", fileInfo.FileId);
                result.FailedMigrations++;
            }
        }

        /// <summary>
        /// 验证迁移后的路径是否有效
        /// </summary>
        public static async Task<ValidationResult> ValidateMigrationAsync(
            ServerGlobalConfig serverConfig,
            tb_FS_FileStorageInfoController<tb_FS_FileStorageInfo> fileStorageInfoController,
            ILogger logger = null)
        {
            var result = new ValidationResult();

            try
            {
                var fileInfos = await fileStorageInfoController.QueryByNavAsync(c => c.FileStatus == (int)FileStatus.Active);

                if (fileInfos == null || fileInfos.Count == 0)
                {
                    result.IsValid = true;
                    result.Message = "没有需要验证的文件记录";
                    return result;
                }

                foreach (var fileInfoObj in fileInfos)
                {
                    if (fileInfoObj is tb_FS_FileStorageInfo fileInfo)
                    {
                        await ValidateFileRecord(fileInfo, serverConfig, result, logger);
                    }
                }

                result.IsValid = result.InvalidPaths.Count == 0;
                result.Message = $"路径验证完成。有效路径: {result.ValidPaths}，无效路径: {result.InvalidPaths.Count}";

                return result;
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "路径验证失败");
                result.IsValid = false;
                result.ErrorMessage = $"验证失败: {ex.Message}";
                return result;
            }
        }

        /// <summary>
        /// 验证单个文件记录的路径
        /// </summary>
        private static async Task ValidateFileRecord(
            tb_FS_FileStorageInfo fileInfo,
            ServerGlobalConfig serverConfig,
            ValidationResult result,
            ILogger logger)
        {
            try
            {
                if (string.IsNullOrEmpty(fileInfo.StoragePath))
                {
                    result.InvalidPaths.Add(new InvalidPathInfo
                    {
                        FileId = fileInfo.FileId,
                        FilePath = "",
                        Error = "存储路径为空"
                    });
                    return;
                }

                // 检查文件是否存在
                var absolutePath = FileStorageHelper.ResolveToAbsolutePath(fileInfo.StoragePath);
                if (File.Exists(absolutePath))
                {
                    result.ValidPaths++;
                    logger?.LogDebug("文件 {FileId} 路径有效: {Path}", fileInfo.FileId, fileInfo.StoragePath);
                }
                else
                {
                    result.InvalidPaths.Add(new InvalidPathInfo
                    {
                        FileId = fileInfo.FileId,
                        FilePath = fileInfo.StoragePath,
                        Error = "文件不存在"
                    });
                    logger?.LogWarning("文件 {FileId} 路径无效，文件不存在: {Path}", fileInfo.FileId, fileInfo.StoragePath);
                }
            }
            catch (Exception ex)
            {
                result.InvalidPaths.Add(new InvalidPathInfo
                {
                    FileId = fileInfo.FileId,
                    FilePath = fileInfo.StoragePath ?? "",
                    Error = $"验证异常: {ex.Message}"
                });
                logger?.LogError(ex, "验证文件 {FileId} 路径时发生错误", fileInfo.FileId);
            }
        }
    }

    /// <summary>
    /// 迁移结果
    /// </summary>
    public class MigrationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public int SuccessfulMigrations { get; set; }
        public int FailedMigrations { get; set; }
        public int SkippedRecords { get; set; }
    }

    /// <summary>
    /// 验证结果
    /// </summary>
    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public string Message { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public int ValidPaths { get; set; }
        public List<InvalidPathInfo> InvalidPaths { get; set; } = new List<InvalidPathInfo>();
    }

    /// <summary>
    /// 无效路径信息
    /// </summary>
    public class InvalidPathInfo
    {
        public long FileId { get; set; }
        public string FilePath { get; set; } = string.Empty;
        public string Error { get; set; } = string.Empty;
    }
}