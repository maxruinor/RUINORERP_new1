using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Models.FileManagement;
using RUINORERP.UI.Network.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RUINORERP.Model;
using RUINORERP.Global;

namespace RUINORERP.UI.Network.Services
{
    /// <summary>
    /// 客户端文件更新服务 - 处理图片的替换、更新和版本管理
    /// 支持三种更新策略:
    /// 1. 仅新增(AppendOnly): 保留旧文件,创建新关联
    /// 2. 替换(Replace): 保留历史版本,更新关联到新文件
    /// 3. 覆盖(Overwrite): 删除旧文件,创建新文件
    /// </summary>
    public sealed class FileUpdateClientService : IDisposable
    {
        private readonly ClientCommunicationService _communicationService;
        private readonly FileManagementService _fileManagementService;
        private readonly ILogger<FileUpdateClientService> _logger;
        private readonly SemaphoreSlim _operationLock = new SemaphoreSlim(1, 1);
        private bool _isDisposed = false;

        /// <summary>
        /// 构造函数 - 通过依赖注入获取服务
        /// </summary>
        public FileUpdateClientService(
            ClientCommunicationService communicationService,
            FileManagementService fileManagementService,
            ILogger<FileUpdateClientService> logger = null)
        {
            _communicationService = communicationService ?? throw new ArgumentNullException(nameof(communicationService));
            _fileManagementService = fileManagementService ?? throw new ArgumentNullException(nameof(fileManagementService));
            _logger = logger;
        }

        /// <summary>
        /// 更新业务单据的图片
        /// </summary>
        /// <param name="businessType">业务类型</param>
        /// <param name="businessNo">业务编号</param>
        /// <param name="businessId">业务主键ID</param>
        /// <param name="relatedField">关联字段(如VoucherImage)</param>
        /// <param name="newFileData">新文件数据</param>
        /// <param name="newFileName">新文件名</param>
        /// <param name="strategy">更新策略(默认Replace)</param>
        /// <param name="isDetailTable">是否明细表</param>
        /// <param name="detailId">明细表ID</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>更新结果</returns>
        public async Task<FileUpdateResult> UpdateBusinessFileAsync(
            int businessType,
            string businessNo,
            long businessId,
            string relatedField,
            byte[] newFileData,
            string newFileName,
            FileUpdateStrategy strategy = FileUpdateStrategy.Replace,
            bool isDetailTable = false,
            long? detailId = null,
            CancellationToken ct = default)
        {
            // 参数验证
            if (newFileData == null || newFileData.Length == 0)
                throw new ArgumentException("文件数据不能为空", nameof(newFileData));

            if (string.IsNullOrEmpty(newFileName))
                throw new ArgumentException("文件名不能为空", nameof(newFileName));

            if (!await _operationLock.WaitAsync(TimeSpan.FromSeconds(30), ct))
            {
                _logger?.LogWarning("文件更新操作繁忙");
                return FileUpdateResult.CreateFailure("系统繁忙,请稍后重试");
            }

            bool lockAcquired = true;
            try
            {
                // 检查连接状态
                if (!_communicationService.ConnectionManager.IsConnected)
                {
                    _logger?.LogWarning("文件更新失败:未连接到服务器");
                    return FileUpdateResult.CreateFailure("未连接到服务器,请检查网络连接后重试");
                }

                _logger?.LogDebug("开始更新文件,BusinessNo:{BusinessNo},RelatedField:{RelatedField},Strategy:{Strategy}",
                    businessNo, relatedField, strategy);

                // 使用注入的文件管理服务
                // var fileService = new FileManagementService(_communicationService, _logger);
                var fileService = _fileManagementService;

                // 创建上传请求
                var uploadRequest = new FileUploadRequest
                {
                    BusinessType = businessType,
                    BusinessNo = businessNo,
                    BusinessId = businessId,
                    RelatedField = relatedField,
                    IsDetailTable = isDetailTable,
                    DetailId = detailId
                };

                // 根据策略处理旧文件
                if (strategy != FileUpdateStrategy.AppendOnly)
                {
                    // 先删除旧文件的关联关系
                    await DeleteOldFileRelationsAsync(businessType, businessId, relatedField, isDetailTable, detailId, ct);
                }

                // 添加新文件
                var fileStorageInfo = new tb_FS_FileStorageInfo
                {
                    OriginalFileName = newFileName,
                    FileData = newFileData
                };
                uploadRequest.FileStorageInfos.Add(fileStorageInfo);

                // 上传新文件
                var uploadResponse = await fileService.UploadFileAsync(uploadRequest, ct);

                if (!uploadResponse.IsSuccess || uploadResponse.FileStorageInfos.Count == 0)
                {
                    _logger?.LogWarning("文件更新失败:{ErrorMessage}", uploadResponse.ErrorMessage);
                    return FileUpdateResult.CreateFailure($"文件更新失败:{uploadResponse.ErrorMessage}");
                }

                var newFileInfo = uploadResponse.FileStorageInfos[0];

                _logger?.LogInformation("文件更新成功,FileId:{FileId},Strategy:{Strategy}", newFileInfo.FileId, strategy);

                return FileUpdateResult.CreateSuccess(
                    newFileInfo.FileId,
                    newFileInfo.OriginalFileName,
                    newFileInfo.FileSize,
                    strategy,
                    "文件更新成功");
            }
            catch (OperationCanceledException)
            {
                return FileUpdateResult.CreateFailure("文件更新操作已取消");
            }
            catch (TimeoutException ex)
            {
                _logger?.LogWarning(ex, "文件更新请求超时");
                return FileUpdateResult.CreateFailure("文件更新请求超时,请检查网络连接后重试");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "文件更新过程中发生未预期的异常");
                return FileUpdateResult.CreateFailure($"文件更新过程中发生错误:{ex.Message}");
            }
            finally
            {
                if (lockAcquired && _operationLock.CurrentCount == 0)
                {
                    _operationLock.Release();
                }
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
        /// <param name="strategy">更新策略(默认Replace)</param>
        /// <param name="isDetailTable">是否明细表</param>
        /// <param name="detailId">明细表ID</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>批量更新结果</returns>
        public async Task<FileBatchUpdateResult> BatchUpdateBusinessFilesAsync(
            int businessType,
            string businessNo,
            long businessId,
            string relatedField,
            List<(byte[] fileData, string fileName)> newFiles,
            FileUpdateStrategy strategy = FileUpdateStrategy.Replace,
            bool isDetailTable = false,
            long? detailId = null,
            CancellationToken ct = default)
        {
            // 参数验证
            if (newFiles == null || newFiles.Count == 0)
                throw new ArgumentException("文件列表不能为空", nameof(newFiles));

            if (!await _operationLock.WaitAsync(TimeSpan.FromSeconds(60), ct))
            {
                _logger?.LogWarning("批量文件更新操作繁忙");
                return FileBatchUpdateResult.CreateFailure("系统繁忙,请稍后重试");
            }

            bool lockAcquired = true;
            try
            {
                // 检查连接状态
                if (!_communicationService.ConnectionManager.IsConnected)
                {
                    _logger?.LogWarning("批量文件更新失败:未连接到服务器");
                    return FileBatchUpdateResult.CreateFailure("未连接到服务器,请检查网络连接后重试");
                }

                var result = new FileBatchUpdateResult
                {
                    Strategy = strategy
                };

                // 根据策略处理旧文件
                if (strategy != FileUpdateStrategy.AppendOnly)
                {
                    await DeleteOldFileRelationsAsync(businessType, businessId, relatedField, isDetailTable, detailId, ct);
                }

                // 使用注入的文件管理服务
                var fileService = _fileManagementService;

                // 批量上传新文件
                foreach (var (fileData, fileName) in newFiles)
                {
                    try
                    {
                        var uploadRequest = new FileUploadRequest
                        {
                            BusinessType = businessType,
                            BusinessNo = businessNo,
                            BusinessId = businessId,
                            RelatedField = relatedField,
                            IsDetailTable = isDetailTable,
                            DetailId = detailId
                        };

                        uploadRequest.FileStorageInfos.Add(new tb_FS_FileStorageInfo
                        {
                            OriginalFileName = fileName,
                            FileData = fileData
                        });

                        var uploadResponse = await fileService.UploadFileAsync(uploadRequest, ct);

                        if (uploadResponse.IsSuccess && uploadResponse.FileStorageInfos.Count > 0)
                        {
                            result.SuccessFiles.Add(fileName);
                            result.NewFiles.Add(uploadResponse.FileStorageInfos[0]);
                        }
                        else
                        {
                            _logger?.LogWarning("文件上传失败:{FileName},Error:{Error}", fileName, uploadResponse.ErrorMessage);
                            result.FailedFiles.Add(fileName);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, "批量上传文件失败:{FileName}", fileName);
                        result.FailedFiles.Add(fileName);
                    }
                }

                _logger?.LogInformation("批量文件更新完成,Success:{SuccessCount},Failed:{FailedCount}",
                    result.SuccessFiles.Count, result.FailedFiles.Count);

                result.IsSuccess = result.FailedFiles.Count == 0;
                return result;
            }
            catch (OperationCanceledException)
            {
                return FileBatchUpdateResult.CreateFailure("批量文件更新操作已取消");
            }
            catch (TimeoutException ex)
            {
                _logger?.LogWarning(ex, "批量文件更新请求超时");
                return FileBatchUpdateResult.CreateFailure("批量文件更新请求超时,请检查网络连接后重试");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "批量文件更新过程中发生未预期的异常");
                return FileBatchUpdateResult.CreateFailure($"批量文件更新过程中发生错误:{ex.Message}");
            }
            finally
            {
                if (lockAcquired && _operationLock.CurrentCount == 0)
                {
                    _operationLock.Release();
                }
            }
        }

        /// <summary>
        /// 删除旧文件的关联关系(不删除物理文件,保留历史)
        /// </summary>
        private async Task DeleteOldFileRelationsAsync(
            int businessType,
            long businessId,
            string relatedField,
            bool isDetailTable,
            long? detailId,
            CancellationToken ct)
        {
            try
            {
                _logger?.LogDebug("准备删除旧文件关联,BusinessType:{BusinessType},BusinessId:{BusinessId},RelatedField:{RelatedField}",
                    businessType, businessId, relatedField);

                // 查询当前业务关联的旧文件
                var listRequest = new FileListRequest
                {
                    BusinessType = businessType,
                    BusinessId = businessId.ToString() // 使用BusinessId查询
                };

                var listResponse = await _fileManagementService.GetFileListAsync(listRequest, ct);

                if (listResponse.IsSuccess && listResponse.Data?.FileStorageInfos != null)
                {
                    // 构建删除请求，仅删除关联关系
                    var deleteRequest = new FileDeleteRequest();
                    deleteRequest.BusinessType = businessType;
                    deleteRequest.PhysicalDelete = false; // 不物理删除文件，只删除关联

                    // 添加所有旧文件到删除列表
                    foreach (var fileInfo in listResponse.Data.FileStorageInfos)
                    {
                        deleteRequest.AddDeleteFileStorageInfo(fileInfo);
                    }

                    // 执行删除（仅删除关联）
                    if (deleteRequest.FileStorageInfos.Count > 0)
                    {
                        var deleteResponse = await _fileManagementService.DeleteFileAsync(deleteRequest, ct);
                        if (deleteResponse.IsSuccess)
                        {
                            _logger?.LogInformation("成功删除{Count}个旧文件关联", deleteRequest.FileStorageInfos.Count);
                        }
                        else
                        {
                            _logger?.LogWarning("删除旧文件关联失败:{ErrorMessage}", deleteResponse.ErrorMessage);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "删除旧文件关联失败");
            }
        }

        /// <summary>
        /// 获取文件的版本历史
        /// </summary>
        /// <param name="businessType">业务类型</param>
        /// <param name="businessId">业务主键ID</param>
        /// <param name="relatedField">关联字段</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>版本历史列表</returns>
        public async Task<List<FileVersionHistory>> GetFileVersionHistoryAsync(
            int businessType,
            long businessId,
            string relatedField,
            CancellationToken ct = default)
        {
            try
            {
                _logger?.LogDebug("获取文件版本历史,BusinessType:{BusinessType},BusinessId:{BusinessId},RelatedField:{RelatedField}",
                    businessType, businessId, relatedField);

                // 查询当前业务关联的所有文件
                var listRequest = new FileListRequest
                {
                    BusinessType = businessType,
                    BusinessId = businessId.ToString()
                };

                var listResponse = await _fileManagementService.GetFileListAsync(listRequest, ct);

                if (!listResponse.IsSuccess || listResponse.Data?.FileStorageInfos == null)
                {
                    _logger?.LogWarning("获取文件列表失败");
                    return new List<FileVersionHistory>();
                }

                // 转换为版本历史列表
                var versionHistoryList = new List<FileVersionHistory>();
                foreach (var fileInfo in listResponse.Data.FileStorageInfos)
                {
                    versionHistoryList.Add(new FileVersionHistory
                    {
                        FileId = fileInfo.FileId,
                        FileName = fileInfo.OriginalFileName,
                        VersionNo = fileInfo.CurrentVersion,
                        IsActive = fileInfo.FileStatus == (int)FileStatus.Active,
                        Created_at = fileInfo.Created_at,
                        Created_by = fileInfo.Created_by,
                        Modified_at = fileInfo.Modified_at,
                        Modified_by = fileInfo.Modified_by
                    });
                }

                // 按修改时间降序排列
                versionHistoryList = versionHistoryList
                    .OrderByDescending(v => v.Modified_at)
                    .ToList();

                _logger?.LogInformation("获取到{Count}个文件版本历史", versionHistoryList.Count);
                return versionHistoryList;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "获取文件版本历史失败");
                return new List<FileVersionHistory>();
            }
        }

        /// <summary>
        /// 恢复到指定版本
        /// </summary>
        /// <param name="businessType">业务类型</param>
        /// <param name="businessId">业务主键ID</param>
        /// <param name="relatedField">关联字段</param>
        /// <param name="fileId">要恢复的文件ID</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>是否恢复成功</returns>
        public async Task<bool> RestoreFileVersionAsync(
            int businessType,
            long businessId,
            string relatedField,
            long fileId,
            CancellationToken ct = default)
        {
            try
            {
                _logger?.LogDebug("恢复文件版本,FileId:{FileId},BusinessId:{BusinessId},RelatedField:{RelatedField}",
                    fileId, businessId, relatedField);

                // 先下载要恢复的文件数据
                var downloadRequest = new FileDownloadRequest
                {
                    FileStorageInfo = new tb_FS_FileStorageInfo { FileId = fileId }
                };

                var downloadResponse = await _fileManagementService.DownloadFileAsync(downloadRequest, ct);

                if (!downloadResponse.IsSuccess ||
                    downloadResponse.FileStorageInfos == null ||
                    downloadResponse.FileStorageInfos.Count == 0)
                {
                    _logger?.LogWarning("下载要恢复的文件失败");
                    return false;
                }

                var oldFileInfo = downloadResponse.FileStorageInfos[0];

                // 使用Replace策略重新上传旧文件
                var updateResult = await UpdateBusinessFileAsync(
                    businessType,
                    null, // BusinessNo，服务器会自动处理
                    businessId,
                    relatedField,
                    oldFileInfo.FileData,
                    oldFileInfo.OriginalFileName,
                    FileUpdateStrategy.Replace,
                    false,
                    null,
                    ct);

                if (updateResult.IsSuccess)
                {
                    _logger?.LogInformation("文件版本恢复成功,FileId:{FileId}", fileId);
                    return true;
                }
                else
                {
                    _logger?.LogWarning("文件版本恢复失败:{ErrorMessage}", updateResult.Message);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "恢复文件版本失败");
                return false;
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (_isDisposed)
                return;

            _isDisposed = true;
            _operationLock?.Dispose();
        }
    }

    /// <summary>
    /// 文件更新策略
    /// </summary>
    public enum FileUpdateStrategy
    {
        /// <summary>
        /// 仅新增 - 保留旧文件,创建新关联(默认)
        /// </summary>
        AppendOnly = 0,

        /// <summary>
        /// 替换模式 - 保留历史版本,更新关联到新文件
        /// </summary>
        Replace = 1,

        /// <summary>
        /// 覆盖模式 - 删除旧文件,创建新文件
        /// </summary>
        Overwrite = 2
    }

    /// <summary>
    /// 文件更新结果
    /// </summary>
    public class FileUpdateResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 新文件ID
        /// </summary>
        public long NewFileId { get; set; }

        /// <summary>
        /// 新文件名
        /// </summary>
        public string NewFileName { get; set; }

        /// <summary>
        /// 新文件大小
        /// </summary>
        public long NewFileSize { get; set; }

        /// <summary>
        /// 使用的更新策略
        /// </summary>
        public FileUpdateStrategy Strategy { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.Now;

        /// <summary>
        /// 创建成功结果
        /// </summary>
        public static FileUpdateResult CreateSuccess(
            long fileId,
            string fileName,
            long fileSize,
            FileUpdateStrategy strategy,
            string message = "文件更新成功")
        {
            return new FileUpdateResult
            {
                IsSuccess = true,
                Message = message,
                NewFileId = fileId,
                NewFileName = fileName,
                NewFileSize = fileSize,
                Strategy = strategy,
                Timestamp = DateTime.Now
            };
        }

        /// <summary>
        /// 创建失败结果
        /// </summary>
        public static FileUpdateResult CreateFailure(string message)
        {
            return new FileUpdateResult
            {
                IsSuccess = false,
                Message = message,
                Timestamp = DateTime.Now
            };
        }
    }

    /// <summary>
    /// 批量文件更新结果
    /// </summary>
    public class FileBatchUpdateResult
    {
        /// <summary>
        /// 是否成功(全部成功)
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 使用的更新策略
        /// </summary>
        public FileUpdateStrategy Strategy { get; set; }

        /// <summary>
        /// 成功的文件列表
        /// </summary>
        public List<string> SuccessFiles { get; set; } = new List<string>();

        /// <summary>
        /// 失败的文件列表
        /// </summary>
        public List<string> FailedFiles { get; set; } = new List<string>();

        /// <summary>
        /// 新文件信息列表
        /// </summary>
        public List<tb_FS_FileStorageInfo> NewFiles { get; set; } = new List<tb_FS_FileStorageInfo>();

        /// <summary>
        /// 时间戳
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.Now;

        /// <summary>
        /// 创建成功结果
        /// </summary>
        public static FileBatchUpdateResult CreateFailure(string message)
        {
            return new FileBatchUpdateResult
            {
                IsSuccess = false,
                Message = message,
                Timestamp = DateTime.Now
            };
        }
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
        /// 是否活跃(当前版本)
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
