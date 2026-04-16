using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Core;
using RUINORERP.PacketSpec.Errors;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.UI.Network;
using RUINORERP.UI.Network.Exceptions;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using RUINORERP.Model;
using System.Drawing;
using RUINORERP.PacketSpec.Models.FileManagement;
using RUINORERP.Global;

namespace RUINORERP.UI.Network.Services
{
    /// <summary>
    /// 文件管理服务类
    /// 提供文件上传、下载、删除、查询、更新等核心功能
    /// </summary>
    public sealed class FileManagementService : IDisposable
    {
        private readonly ClientCommunicationService _communicationService;
        private readonly ILogger<FileManagementService> _log;
        private readonly ConcurrentDictionary<long, SemaphoreSlim> _businessLocks = new();
        private readonly SemaphoreSlim _lockDictionaryLock = new SemaphoreSlim(1, 1);
        private bool _isDisposed = false;
        private static readonly string[] ImageExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff", ".webp" };

        /// <summary>
        /// 文件上传超时时间（毫秒）
        /// </summary>
        private const int UploadTimeoutMs = 60000;
        
        /// <summary>
        /// 单个业务锁等待超时时间（秒）
        /// </summary>
        private const int BusinessLockWaitTimeoutSeconds = 30;
        
        /// <summary>
        /// 最大并发上传数（同一业务ID）
        /// </summary>
        private const int MaxConcurrentUploadsPerBusiness = 1;
        
        /// <summary>
        /// 锁清理定时器（每5分钟清理一次空闲锁）
        /// </summary>
        private readonly System.Threading.Timer _lockCleanupTimer;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="communicationService">通信服务</param>
        /// <param name="logger">日志记录器</param>
        /// <exception cref="ArgumentNullException">当必要参数为空时抛出</exception>
        public FileManagementService(
            ClientCommunicationService communicationService,
            ILogger<FileManagementService> logger = null)
        {
            _communicationService = communicationService ?? throw new ArgumentNullException(nameof(communicationService));
            _log = logger;
            
            // 启动定期清理空闲锁的定时器（5分钟后开始，每5分钟执行一次）
            _lockCleanupTimer = new System.Threading.Timer(
                _ => CleanupIdleLocks(),
                null,
                TimeSpan.FromMinutes(5),
                TimeSpan.FromMinutes(5)
            );
        }

        /// <summary>
        /// 获取指定业务ID的锁，如果不存在则创建
        /// </summary>
        /// <param name="businessId">业务ID</param>
        /// <returns>信号量</returns>
        private SemaphoreSlim GetOrCreateBusinessLock(long businessId)
        {
            return _businessLocks.GetOrAdd(businessId, _ => new SemaphoreSlim(MaxConcurrentUploadsPerBusiness, MaxConcurrentUploadsPerBusiness));
        }

        /// <summary>
        /// 清理不再使用的业务锁
        /// </summary>
        /// <param name="businessId">业务ID</param>
        private void TryCleanupBusinessLock(long businessId)
        {
            if (_businessLocks.TryRemove(businessId, out var removed))
            {
                try
                {
                    removed?.Dispose();
                    _log?.LogDebug("已清理业务锁: BusinessId={BusinessId}", businessId);
                }
                catch (Exception ex)
                {
                    _log?.LogWarning(ex, "清理业务锁失败: BusinessId={BusinessId}", businessId);
                }
            }
        }
        
        /// <summary>
        /// 定期清理空闲的业务锁（防止内存泄漏）
        /// </summary>
        private void CleanupIdleLocks()
        {
            try
            {
                var idleLocks = _businessLocks
                    .Where(kvp => kvp.Value.CurrentCount == MaxConcurrentUploadsPerBusiness)
                    .Select(kvp => kvp.Key)
                    .ToList();
                
                foreach (var businessId in idleLocks)
                {
                    // 再次检查，避免竞态条件
                    if (_businessLocks.TryGetValue(businessId, out var lockObj) &&
                        lockObj.CurrentCount == MaxConcurrentUploadsPerBusiness)
                    {
                        TryCleanupBusinessLock(businessId);
                    }
                }
                
                if (idleLocks.Count > 0)
                {
                    _log?.LogDebug("清理了{Count}个空闲业务锁，当前活跃锁数量: {ActiveCount}",
                        idleLocks.Count, _businessLocks.Count);
                }
            }
            catch (Exception ex)
            {
                _log?.LogWarning(ex, "清理空闲锁时发生异常");
            }
        }

        /// <summary>
        /// 验证文件是否为有效的图片文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>是否为有效的图片文件</returns>
        private bool IsValidImageFile(string filePath)
        {
            try
            {
                // 首先检查扩展名
                string extension = Path.GetExtension(filePath)?.ToLowerInvariant();
                if (string.IsNullOrEmpty(extension) || !ImageExtensions.Contains(extension))
                {
                    return false;
                }

                // 然后验证文件内容
                using (var image = Image.FromFile(filePath))
                {
                    // 检查图像的宽度和高度以确保是有效的图像
                    return image.Width > 0 && image.Height > 0;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 验证文件存储信息对象是否为有效的图片文件
        /// </summary>
        /// <param name="fileStorageInfo">文件存储信息</param>
        /// <returns>是否为有效的图片文件</returns>
        private bool IsValidImageFile(tb_FS_FileStorageInfo fileStorageInfo)
        {
            if (fileStorageInfo == null)
                return false;

            // 检查扩展名
            bool isImageExtension = !string.IsNullOrEmpty(fileStorageInfo.FileExtension) &&
                                   ImageExtensions.Contains(fileStorageInfo.FileExtension.ToLower());

            // 检查文件类型
            bool isImageType = !string.IsNullOrEmpty(fileStorageInfo.FileType) &&
                              (fileStorageInfo.FileType.ToLower().Contains("image/") ||
                               fileStorageInfo.FileType.Contains("图片"));

            // 检查文件数据
            bool isValidImageContent = fileStorageInfo.FileData != null &&
                                       fileStorageInfo.FileData.Length > 0 &&
                                       IsValidImageFile(fileStorageInfo.FileData);

            // 满足任一条件即可
            return isImageExtension || isImageType || isValidImageContent;
        }

        /// <summary>
        /// 验证文件数据是否为有效的图片文件
        /// </summary>
        /// <param name="fileData">文件数据</param>
        /// <returns>如果是有效的图片文件返回true，否则返回false</returns>
        private bool IsValidImageFile(byte[] fileData)
        {
            if (fileData == null || fileData.Length == 0)
                return false;

            try
            {
                // 尝试从字节数组创建图像以验证是否为有效的图片文件
                using (var ms = new MemoryStream(fileData))
                {
                    using (var image = Image.FromStream(ms))
                    {
                        // 检查图像的宽度和高度以确保是有效的图像
                        return image.Width > 0 && image.Height > 0;
                    }
                }
            }
            catch
            {
                // 如果出现任何异常，说明不是有效的图片文件
                return false;
            }
        }

        private bool IsValidImageFile(Stream stream)
        {
            try
            {
                using (var image = Image.FromStream(stream))
                {
                    // 检查图像的宽度和高度以确保是有效的图像
                    return image.Width > 0 && image.Height > 0;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 删除图片文件（重载方法，直接使用文件存储信息对象）
        /// 注意：此方法已废弃，建议使用 FileBusinessService.DeleteImagesAsync
        /// </summary>
        /// <param name="fileStorageInfo">文件存储信息对象</param>
        /// <param name="BusinessId">业务ID</param>
        /// <param name="OwnerTableName">业务表名</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>文件删除响应</returns>
        [Obsolete("建议使用 FileBusinessService.DeleteImagesAsync 方法")]
        public async Task<FileDeleteResponse> DeleteImageAsync(tb_FS_FileStorageInfo fileStorageInfo, long BusinessId, string OwnerTableName, CancellationToken ct = default)
        {
            // 参数验证
            if (fileStorageInfo == null)
                throw new ArgumentNullException(nameof(fileStorageInfo));

            if (fileStorageInfo.FileId <= 0)
                throw new ArgumentException("文件ID必须大于0", nameof(fileStorageInfo.FileId));

            // 验证是否为图片文件
            if (!IsValidImageFile(fileStorageInfo))
                return FileDeleteResponse.CreateFailure("只能删除图片文件");

            // 确保文件存储信息的所有必要属性都有值
            fileStorageInfo.OwnerTableName = OwnerTableName;
            fileStorageInfo.StorageProvider = fileStorageInfo.StorageProvider ?? "Local";
            fileStorageInfo.StoragePath = fileStorageInfo.StoragePath ?? string.Empty;
            fileStorageInfo.StorageFileName = fileStorageInfo.StorageFileName ?? $"{fileStorageInfo.FileId}_{DateTime.Now:yyyyMMddHHmmssfff}{Path.GetExtension(fileStorageInfo.OriginalFileName)}";
            fileStorageInfo.FileStatus = fileStorageInfo.FileStatus > 0 ? fileStorageInfo.FileStatus : (int)FileStatus.Active;
            fileStorageInfo.CurrentVersion = fileStorageInfo.CurrentVersion > 0 ? fileStorageInfo.CurrentVersion : 1;
            fileStorageInfo.ExpireTime = fileStorageInfo.ExpireTime > DateTime.MinValue ? fileStorageInfo.ExpireTime : DateTime.MaxValue;
            fileStorageInfo.Description = fileStorageInfo.Description ?? string.Empty;
            fileStorageInfo.Metadata = fileStorageInfo.Metadata ?? string.Empty;

            // 创建删除请求
            var deleteRequest = new FileDeleteRequest();
            deleteRequest.InitializeCompatibility();
            deleteRequest.BusinessId = BusinessId;
            deleteRequest.AddDeleteFileStorageInfo(fileStorageInfo);

            // 调用通用删除方法
            return await DeleteFileAsync(deleteRequest, ct);
        }





        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="request">文件上传请求</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>文件上传响应</returns>
        public async Task<FileUploadResponse> UploadFileAsync(FileUploadRequest request, CancellationToken ct = default)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request.FileStorageInfos == null || request.FileStorageInfos.Count == 0)
                throw new ArgumentException("文件数据不能为空");

            long businessId = request.BusinessId ?? 0L;
            var businessLock = GetOrCreateBusinessLock(businessId);
            bool lockAcquired = false;

            try
            {
                lockAcquired = await businessLock.WaitAsync(TimeSpan.FromSeconds(BusinessLockWaitTimeoutSeconds), ct);
                if (!lockAcquired)
                {
                    _log?.LogWarning("获取业务锁超时({Timeout}秒)，BusinessId: {BusinessId}", BusinessLockWaitTimeoutSeconds, businessId);
                    return ResponseFactory.CreateSpecificErrorResponse<FileUploadResponse>("系统繁忙，请稍后重试");
                }

                if (!_communicationService.ConnectionManager.IsConnected)
                {
                    _log?.LogWarning("文件上传失败：未连接到服务器");
                    return ResponseFactory.CreateSpecificErrorResponse<FileUploadResponse>("未连接到服务器，请检查网络连接后重试");
                }

                _log?.LogDebug("开始文件上传请求, BusinessId: {BusinessId}", businessId);

                var response = await _communicationService.SendCommandWithResponseAsync<FileUploadResponse>(
                    FileCommands.FileUpload, request, ct, timeoutMs: UploadTimeoutMs);

                if (response == null)
                {
                    return ResponseFactory.CreateSpecificErrorResponse<FileUploadResponse>("服务器返回了空的响应数据");
                }

                if (!response.IsSuccess)
                {
                    if (response.ErrorMessage?.Contains("格式不支持") == true || 
                        response.ErrorMessage?.Contains("超过限制") == true)
                    {
                        return response;
                    }
                    _log?.LogWarning("文件上传失败: {Error}", response.ErrorMessage);
                    return ResponseFactory.CreateSpecificErrorResponse<FileUploadResponse>(response.ErrorMessage ?? "上传失败");
                }

                _log?.LogDebug("文件上传成功, BusinessId: {BusinessId}, FileCount: {FileCount}", 
                    businessId, response.FileStorageInfos?.Count ?? 0);
                return response;
            }
            catch (OperationCanceledException)
            {
                _log?.LogWarning("文件上传已取消, BusinessId: {BusinessId}", businessId);
                throw;
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "文件上传异常, BusinessId: {BusinessId}", businessId);
                return ResponseFactory.CreateSpecificErrorResponse<FileUploadResponse>($"文件上传异常: {ex.Message}");
            }
            finally
            {
                if (lockAcquired)
                {
                    try { businessLock.Release(); } catch { }
                }
            }
        }

        /// <summary>
        /// 文件下载1
        /// </summary>
        /// <param name="request">文件下载请求</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>文件下载响应</returns>
        public async Task<FileDownloadResponse> DownloadFileAsync(FileDownloadRequest request, CancellationToken ct = default)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request.FileStorageInfo.FileId == 0)
                throw new ArgumentException("文件ID不能为空", nameof(request.FileStorageInfo));

            long businessId = request.FileStorageInfo.FileId;
            var businessLock = GetOrCreateBusinessLock(businessId);
            bool lockAcquired = false;

            try
            {
                lockAcquired = await businessLock.WaitAsync(TimeSpan.FromSeconds(BusinessLockWaitTimeoutSeconds), ct);
                if (!lockAcquired)
                {
                    return FileDownloadResponse.CreateFailure("系统繁忙，请稍后重试");
                }

                if (!_communicationService.ConnectionManager.IsConnected)
                {
                    return FileDownloadResponse.CreateFailure("未连接到服务器，请检查网络连接后重试");
                }

                var response = await _communicationService.SendCommandWithResponseAsync<FileDownloadResponse>(
                    FileCommands.FileDownload, request, ct, timeoutMs: UploadTimeoutMs);

                if (response == null)
                {
                    return FileDownloadResponse.CreateFailure("服务器返回了空的响应数据，请联系系统管理员");
                }

                if (!response.IsSuccess)
                {
                    return FileDownloadResponse.CreateFailure($"文件下载失败: {response.Message}{response.ErrorMessage}");
                }

                return response;
            }
            catch (OperationCanceledException)
            {
                return FileDownloadResponse.CreateFailure("文件下载操作已取消");
            }
            catch (TimeoutException ex)
            {
                _log?.LogWarning(ex, "文件下载请求超时");
                return FileDownloadResponse.CreateFailure("文件下载请求超时，请检查网络连接后重试");
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "文件下载过程中发生未预期的异常");
                return FileDownloadResponse.CreateFailure("文件下载过程中发生错误，请稍后重试");
            }
            finally
            {
                if (lockAcquired)
                {
                    try { businessLock.Release(); } catch { }
                }
            }
        }

        /// <summary>
        /// 文件删除
        /// </summary>
        /// <param name="request">文件删除请求</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>文件删除响应</returns>
        public async Task<FileDeleteResponse> DeleteFileAsync(FileDeleteRequest request, CancellationToken ct = default)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            long businessId = request.BusinessId;
            var businessLock = GetOrCreateBusinessLock(businessId);
            bool lockAcquired = false;

            try
            {
                lockAcquired = await businessLock.WaitAsync(TimeSpan.FromSeconds(BusinessLockWaitTimeoutSeconds), ct);
                if (!lockAcquired)
                {
                    _log?.LogWarning("获取业务锁超时({Timeout}秒)，BusinessId: {BusinessId}", BusinessLockWaitTimeoutSeconds, businessId);
                    return FileDeleteResponse.CreateFailure("系统繁忙，请稍后重试");
                }

                if (!_communicationService.ConnectionManager.IsConnected)
                {
                    _log?.LogWarning("文件删除失败：未连接到服务器");
                    return FileDeleteResponse.CreateFailure("未连接到服务器，请检查网络连接后重试");
                }

                _log?.LogDebug("开始文件删除请求, BusinessId: {BusinessId}", businessId);

                var response = await _communicationService.SendCommandWithResponseAsync<FileDeleteResponse>(
                    FileCommands.FileDelete, request, ct);

                if (response == null)
                {
                    _log?.LogError("文件删除失败：服务器返回了空的响应数据");
                    return FileDeleteResponse.CreateFailure("服务器返回了空的响应数据，请联系系统管理员");
                }

                if (!response.IsSuccess)
                {
                    _log?.LogWarning("文件删除失败: {ErrorMessage}", response.ErrorMessage);
                    return FileDeleteResponse.CreateFailure($"文件删除失败: {response.ErrorMessage}");
                }

                return response;
            }
            catch (OperationCanceledException)
            {
                return FileDeleteResponse.CreateFailure("文件删除操作已取消");
            }
            catch (TimeoutException ex)
            {
                _log?.LogWarning(ex, "文件删除请求超时");
                return FileDeleteResponse.CreateFailure("文件删除请求超时，请检查网络连接后重试");
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "文件删除过程中发生未预期的异常");
                return FileDeleteResponse.CreateFailure("文件删除过程中发生错误，请稍后重试");
            }
            finally
            {
                if (lockAcquired)
                {
                    try { businessLock.Release(); } catch { }
                }
            }
        }

        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <param name="request">文件信息请求</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>文件信息响应</returns>
        public async Task<FileInfoResponse> GetFileInfoAsync(FileInfoRequest request, CancellationToken ct = default)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request.FileStorageInfo.FileId == 0)
                throw new ArgumentException("文件ID不能为空", nameof(request.FileStorageInfo.FileId));

            long businessId = request.FileStorageInfo.FileId;
            var businessLock = GetOrCreateBusinessLock(businessId);
            bool lockAcquired = false;

            try
            {
                lockAcquired = await businessLock.WaitAsync(TimeSpan.FromSeconds(BusinessLockWaitTimeoutSeconds), ct);
                if (!lockAcquired)
                {
                    _log?.LogWarning("获取业务锁超时({Timeout}秒)，BusinessId: {BusinessId}", BusinessLockWaitTimeoutSeconds, businessId);
                    return FileInfoResponse.CreateFailure("文件操作队列繁忙，请稍后重试");
                }

                if (!_communicationService.ConnectionManager.IsConnected)
                {
                    _log?.LogWarning("获取文件信息失败：未连接到服务器");
                    return FileInfoResponse.CreateFailure("未连接到服务器，请检查网络连接后重试");
                }

                _log?.LogDebug("开始获取文件信息请求, FileId: {FileId}", businessId);

                var response = await _communicationService.SendCommandWithResponseAsync<FileInfoResponse>(
                    FileCommands.FileInfoQuery, request, ct);

                if (response == null)
                {
                    _log?.LogError("获取文件信息失败：服务器返回了空的响应数据");
                    return FileInfoResponse.CreateFailure("服务器返回了空的响应数据，请联系系统管理员");
                }

                if (!response.IsSuccess)
                {
                    _log?.LogWarning("获取文件信息失败: {ErrorMessage}", response.ErrorMessage);
                    return FileInfoResponse.CreateFailure($"获取文件信息失败: {response.ErrorMessage}");
                }

                return response;
            }
            catch (OperationCanceledException)
            {
                return FileInfoResponse.CreateFailure("获取文件信息操作已取消");
            }
            catch (TimeoutException)
            {
                _log?.LogWarning("获取文件信息过程中发生超时异常");
                return FileInfoResponse.CreateFailure("文件操作超时，请稍后重试");
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "获取文件信息过程中发生未预期的异常");
                return FileInfoResponse.CreateFailure("获取文件信息过程中发生错误，请稍后重试");
            }
            finally
            {
                if (lockAcquired)
                {
                    try { businessLock.Release(); } catch { }
                }
            }
        }

        /// <summary>
        /// 获取文件列表
        /// </summary>
        /// <param name="request">文件列表请求</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>文件列表响应</returns>
        public async Task<FileListResponse> GetFileListAsync(FileListRequest request, CancellationToken ct = default)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            // ✅ 修复: 使用请求中的BusinessId，如果没有则使用0作为全局锁
            long businessId = request.BusinessId;
            var businessLock = GetOrCreateBusinessLock(businessId);
            bool lockAcquired = false;

            try
            {
                lockAcquired = await businessLock.WaitAsync(TimeSpan.FromSeconds(BusinessLockWaitTimeoutSeconds), ct);
                if (!lockAcquired)
                {
                    return FileListResponse.CreateFailure("文件操作队列繁忙，请稍后重试");
                }

                if (!_communicationService.ConnectionManager.IsConnected)
                {
                    return FileListResponse.CreateFailure("未连接到服务器，请检查网络连接后重试");
                }

                var response = await _communicationService.SendCommandWithResponseAsync<FileListResponse>(
                    FileCommands.FileList, request, ct);

                if (response == null)
                {
                    return FileListResponse.CreateFailure("服务器返回了空的响应数据，请联系系统管理员");
                }

                if (!response.IsSuccess)
                {
                    return FileListResponse.CreateFailure($"获取文件列表失败: {response.ErrorMessage}");
                }

                return response;
            }
            catch (OperationCanceledException)
            {
                return FileListResponse.CreateFailure("获取文件列表操作已取消");
            }
            catch (TimeoutException)
            {
                return FileListResponse.CreateFailure("文件操作超时，请稍后重试");
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "获取文件列表过程中发生未预期的异常");
                return FileListResponse.CreateFailure("获取文件列表过程中发生错误，请稍后重试");
            }
            finally
            {
                if (lockAcquired)
                {
                    try { businessLock.Release(); } catch { }
                }
            }
        }

        /// <summary>
        /// 获取文件存储使用情况信息
        /// </summary>
        /// <param name="ct">取消令牌</param>
        /// <returns>存储使用信息</returns>
        public async Task<StorageUsageInfoData> GetStorageUsageInfoAsync(CancellationToken ct = default)
        {
            // ✅ 修复: 存储使用情况是全局信息，使用特殊的锁ID(-1)避免与业务锁冲突
            var businessLock = GetOrCreateBusinessLock(-1);
            bool lockAcquired = false;

            try
            {
                lockAcquired = await businessLock.WaitAsync(TimeSpan.FromSeconds(BusinessLockWaitTimeoutSeconds), ct);
                if (!lockAcquired)
                {
                    return new StorageUsageInfoData();
                }

                if (!_communicationService.ConnectionManager.IsConnected)
                {
                    return new StorageUsageInfoData();
                }

                var request = new FileInfoRequest();

                var response = await _communicationService.SendCommandWithResponseAsync<FileInfoResponse>(
                    FileCommands.FileInfoQuery, request, ct);

                if (response != null && response.IsSuccess)
                {
                    var usageInfo = new StorageUsageInfoData();

                    if (response.FileStorageInfos != null)
                    {
                        foreach (var fileInfo in response.FileStorageInfos)
                        {
                            usageInfo.TotalSize += (fileInfo.FileSize > 0 ? fileInfo.FileSize : 0);
                            usageInfo.TotalFileCount++;
                        }
                    }

                    return usageInfo;
                }

                return new StorageUsageInfoData();
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "获取存储使用信息时发生异常");
                return new StorageUsageInfoData();
            }
            finally
            {
                if (lockAcquired)
                {
                    try { businessLock.Release(); } catch { }
                }
            }
        }

        /// <summary>
        /// 格式化文件大小
        /// </summary>
        /// <param name="bytes">字节数</param>
        /// <returns>格式化后的字符串</returns>
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
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (_isDisposed)
                return;

            _isDisposed = true;

            try
            {
                // 停止清理定时器
                _lockCleanupTimer?.Dispose();
                
                // 清理所有业务锁
                foreach (var kvp in _businessLocks)
                {
                    try
                    {
                        kvp.Value?.Dispose();
                    }
                    catch (Exception ex)
                    {
                        _log?.LogWarning(ex, "释放业务锁失败: BusinessId={BusinessId}", kvp.Key);
                    }
                }
                _businessLocks.Clear();
                
                _lockDictionaryLock?.Dispose();
                
                _log?.LogInformation("FileManagementService资源已释放");
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "释放FileManagementService资源时发生异常");
            }
        }
    }
}