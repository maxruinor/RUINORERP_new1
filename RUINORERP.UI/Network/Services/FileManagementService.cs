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
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using RUINOR.WinFormsUI.CustomPictureBox;
using RUINORERP.Model;
using System.Drawing;
using RUINORERP.PacketSpec.Models.FileManagement;

namespace RUINORERP.UI.Network.Services
{
    /// <summary>
    /// 文件管理服务类
    /// 提供文件上传、下载、删除、查询等核心功能
    /// 参考UserLoginService的设计模式实现
    /// </summary>
    public sealed class FileManagementService : IDisposable
    {
        private readonly ClientCommunicationService _communicationService;
        private readonly ILogger<FileManagementService> _log;
        private readonly SemaphoreSlim _fileOperationLock = new SemaphoreSlim(1, 1); // 防止并发文件操作请求
        private bool _isDisposed = false;
        // 图片扩展名常量，避免重复创建数组
        private static readonly string[] ImageExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff", ".webp" };

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
        /// </summary>
        /// <param name="fileStorageInfo">文件存储信息对象</param>
        /// <param name="bizType">业务类型</param>
        /// <param name="bizNo">业务编号</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>文件删除响应</returns>
        public async Task<FileDeleteResponse> DeleteImageAsync(tb_FS_FileStorageInfo fileStorageInfo, int BizType, string BizNo, CancellationToken ct = default)
        {
            // 参数验证
            if (fileStorageInfo == null)
                throw new ArgumentNullException(nameof(fileStorageInfo));

            if (fileStorageInfo.FileId <= 0)
                throw new ArgumentException("文件ID必须大于0", nameof(fileStorageInfo.FileId));

            // 验证是否为图片文件
            if (!IsValidImageFile(fileStorageInfo))
                return FileDeleteResponse.CreateFailure("只能删除图片文件");

            // 创建删除请求
            var deleteRequest = new FileDeleteRequest();
            deleteRequest.InitializeCompatibility();
            deleteRequest.BusinessNo = BizNo;
            deleteRequest.BusinessType = BizType;
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
            // 验证参数
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request.FileStorageInfos == null || request.FileStorageInfos.Count == 0)
                throw new ArgumentException("文件数据不能为空");

            // 使用信号量确保同一时间只有一个文件操作请求，并添加超时保护
            if (!await _fileOperationLock.WaitAsync(TimeSpan.FromSeconds(30), ct))
            {
                _log?.LogWarning("获取文件操作锁超时");
                return ResponseFactory.CreateSpecificErrorResponse<FileUploadResponse>("系统繁忙，请稍后重试");
            }

            bool lockAcquired = true;
            try
            {
                // 检查连接状态
                if (!_communicationService.ConnectionManager.IsConnected)
                {
                    _log?.LogWarning("文件上传失败：未连接到服务器");
                    return ResponseFactory.CreateSpecificErrorResponse<FileUploadResponse>("未连接到服务器，请检查网络连接后重试");
                }

                // 只记录关键信息，移除详细的文件名日志
                _log?.LogDebug("开始文件上传请求");

                // 发送文件上传命令并获取响应
                var response = await _communicationService.SendCommandWithResponseAsync<FileUploadResponse>(
                    FileCommands.FileUpload, request, ct);

                // 检查响应数据是否为空
                if (response == null)
                {
                    _log?.LogError("文件上传失败：服务器返回了空的响应数据");
                    return ResponseFactory.CreateSpecificErrorResponse<FileUploadResponse>("服务器返回了空的响应数据，请联系系统管理员");
                }

                // 检查响应是否成功
                if (!response.IsSuccess)
                {
                    _log?.LogWarning("文件上传失败: {ErrorMessage}", response.ErrorMessage);
                    return ResponseFactory.CreateSpecificErrorResponse<FileUploadResponse>($"文件上传失败: {response.ErrorMessage}");
                }

                // 只记录关键信息，简化日志内容
                return response;
            }
            catch (OperationCanceledException ex)
            {
                return ResponseFactory.CreateSpecificErrorResponse<FileUploadResponse>("文件上传操作已取消");
            }
            catch (TimeoutException ex)
            {
                _log?.LogWarning(ex, "文件上传请求超时");
                return ResponseFactory.CreateSpecificErrorResponse<FileUploadResponse>("文件上传请求超时，请检查网络连接后重试");
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "文件上传过程中发生未预期的异常");
                return ResponseFactory.CreateSpecificErrorResponse<FileUploadResponse>("文件上传过程中发生错误，请稍后重试");
            }
            finally
            {
                // 检查信号量是否被占用，避免重复释放
                if (lockAcquired && _fileOperationLock.CurrentCount == 0)
                {
                    _fileOperationLock.Release();
                }
            }
        }

        /// <summary>
        /// 文件下载
        /// </summary>
        /// <param name="request">文件下载请求</param>
        /// <param name="ct">取消令牌</param>
        /// <returns>文件下载响应</returns>
        public async Task<FileDownloadResponse> DownloadFileAsync(FileDownloadRequest request, CancellationToken ct = default)
        {
            // 验证参数
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request.FileStorageInfo.FileId == 0)
                throw new ArgumentException("文件ID不能为空", nameof(request.FileStorageInfo));

            // 使用信号量确保同一时间只有一个文件操作请求，并添加超时保护
            if (!await _fileOperationLock.WaitAsync(TimeSpan.FromSeconds(30), ct))
            {
                _log?.LogWarning("获取文件操作锁超时");
                return FileDownloadResponse.CreateFailure("系统繁忙，请稍后重试");
            }

            bool lockAcquired = true;
            try
            {
                // 检查连接状态
                if (!_communicationService.ConnectionManager.IsConnected)
                {
                    _log?.LogWarning("文件下载失败：未连接到服务器");
                    return FileDownloadResponse.CreateFailure("未连接到服务器，请检查网络连接后重试");
                }

                // 只记录关键信息，移除详细的文件ID日志
                _log?.LogDebug("开始文件下载请求");

                // 发送文件下载命令并获取响应
                var response = await _communicationService.SendCommandWithResponseAsync<FileDownloadResponse>(
                    FileCommands.FileDownload, request, ct);

                // 检查响应数据是否为空
                if (response == null)
                {
                    _log?.LogError("文件下载失败：服务器返回了空的响应数据");
                    return FileDownloadResponse.CreateFailure("服务器返回了空的响应数据，请联系系统管理员");
                }

                // 检查响应是否成功
                if (!response.IsSuccess)
                {
                    _log?.LogWarning("文件下载失败: {ErrorMessage}", response.ErrorMessage);
                    return FileDownloadResponse.CreateFailure($"文件下载失败: {response.ErrorMessage}");
                }

                return response;
            }
            catch (OperationCanceledException ex)
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
                // 检查信号量是否被占用，避免重复释放
                if (lockAcquired && _fileOperationLock.CurrentCount == 0)
                {
                    _fileOperationLock.Release();
                    lockAcquired = false;
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
            // 验证参数
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            // 使用信号量确保同一时间只有一个文件操作请求，并添加超时保护
            if (!await _fileOperationLock.WaitAsync(TimeSpan.FromSeconds(30), ct))
            {
                _log?.LogWarning("获取文件操作锁超时");
                return FileDeleteResponse.CreateFailure("系统繁忙，请稍后重试");
            }

            bool lockAcquired = true;
            try
            {
                // 检查连接状态
                if (!_communicationService.ConnectionManager.IsConnected)
                {
                    _log?.LogWarning("文件删除失败：未连接到服务器");
                    return FileDeleteResponse.CreateFailure("未连接到服务器，请检查网络连接后重试");
                }

                // 只记录关键信息，移除详细的文件ID日志
                _log?.LogDebug("开始文件删除请求");

                // 发送文件删除命令并获取响应
                var response = await _communicationService.SendCommandWithResponseAsync<FileDeleteResponse>(
                    FileCommands.FileDelete, request, ct);

                // 检查响应数据是否为空
                if (response == null)
                {
                    _log?.LogError("文件删除失败：服务器返回了空的响应数据");
                    return FileDeleteResponse.CreateFailure("服务器返回了空的响应数据，请联系系统管理员");
                }

                // 检查响应是否成功
                if (!response.IsSuccess)
                {
                    _log?.LogWarning("文件删除失败: {ErrorMessage}", response.ErrorMessage);
                    return FileDeleteResponse.CreateFailure($"文件删除失败: {response.ErrorMessage}");
                }

                // 只记录关键信息，简化日志内容
                return response;
            }
            catch (OperationCanceledException ex)
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
                // 检查信号量是否被占用，避免重复释放
                if (lockAcquired && _fileOperationLock.CurrentCount == 0)
                {
                    _fileOperationLock.Release();
                    lockAcquired = false;
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
            // 验证参数
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request.FileStorageInfo.FileId == 0)
                throw new ArgumentException("文件ID不能为空", nameof(request.FileStorageInfo.FileId));

            // 使用信号量确保同一时间只有一个文件操作请求，带30秒超时
            bool lockAcquired = false;
            try
            {
                if (!await _fileOperationLock.WaitAsync(TimeSpan.FromSeconds(30), ct))
                {
                    _log?.LogWarning("获取文件操作锁超时，当前锁状态: {CurrentCount}", _fileOperationLock.CurrentCount);
                    return FileInfoResponse.CreateFailure("文件操作队列繁忙，请稍后重试");
                }
                lockAcquired = true;

                // 检查连接状态
                if (!_communicationService.ConnectionManager.IsConnected)
                {
                    _log?.LogWarning("获取文件信息失败：未连接到服务器");
                    return FileInfoResponse.CreateFailure("未连接到服务器，请检查网络连接后重试");
                }

                // 只记录关键信息，移除详细的文件ID日志
                _log?.LogDebug("开始获取文件信息请求");

                // 发送获取文件信息命令并获取响应
                var response = await _communicationService.SendCommandWithResponseAsync<FileInfoResponse>(
                    FileCommands.FileInfoQuery, request, ct);

                // 检查响应数据是否为空
                if (response == null)
                {
                    _log?.LogError("获取文件信息失败：服务器返回了空的响应数据");
                    return FileInfoResponse.CreateFailure("服务器返回了空的响应数据，请联系系统管理员");
                }

                // 检查响应是否成功
                if (!response.IsSuccess)
                {
                    _log?.LogWarning("获取文件信息失败: {ErrorMessage}", response.ErrorMessage);
                    return FileInfoResponse.CreateFailure($"获取文件信息失败: {response.ErrorMessage}");
                }

                // 只记录关键信息，简化日志内容
                return response;
            }
            catch (OperationCanceledException ex)
            {
                return FileInfoResponse.CreateFailure("获取文件信息操作已取消");
            }
            catch (TimeoutException ex)
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
                // 确保只在成功获取锁的情况下才尝试释放，避免重复释放
                if (lockAcquired && _fileOperationLock.CurrentCount == 0)
                {
                    _fileOperationLock.Release();
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
            // 验证参数
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            // 使用信号量确保同一时间只有一个文件操作请求，带30秒超时
            bool lockAcquired = false;
            try
            {
                if (!await _fileOperationLock.WaitAsync(TimeSpan.FromSeconds(30), ct))
                {
                    _log?.LogWarning("获取文件操作锁超时，当前锁状态: {CurrentCount}", _fileOperationLock.CurrentCount);
                    return FileListResponse.CreateFailure("文件操作队列繁忙，请稍后重试");
                }
                lockAcquired = true;

                // 检查连接状态
                if (!_communicationService.ConnectionManager.IsConnected)
                {
                    _log?.LogWarning("获取文件列表失败：未连接到服务器");
                    return FileListResponse.CreateFailure("未连接到服务器，请检查网络连接后重试");
                }

                // 只记录关键信息，移除详细的操作日志
                _log?.LogDebug("开始获取文件列表请求");

                // 发送获取文件列表命令并获取响应
                var response = await _communicationService.SendCommandWithResponseAsync<FileListResponse>(
                    FileCommands.FileList, request, ct);

                // 检查响应数据是否为空
                if (response == null)
                {
                    _log?.LogError("获取文件列表失败：服务器返回了空的响应数据");
                    return FileListResponse.CreateFailure("服务器返回了空的响应数据，请联系系统管理员");
                }

                // 检查响应是否成功
                if (!response.IsSuccess)
                {
                    _log?.LogWarning("获取文件列表失败: {ErrorMessage}", response.ErrorMessage);
                    return FileListResponse.CreateFailure($"获取文件列表失败: {response.ErrorMessage}");
                }

                // 只记录关键信息，简化日志内容
                return response;
            }
            catch (OperationCanceledException ex)
            {
                return FileListResponse.CreateFailure("获取文件列表操作已取消");
            }
            catch (TimeoutException ex)
            {
                _log?.LogWarning("获取文件列表过程中发生超时异常");
                return FileListResponse.CreateFailure("文件操作超时，请稍后重试");
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "获取文件列表过程中发生未预期的异常");
                return FileListResponse.CreateFailure("获取文件列表过程中发生错误，请稍后重试");
            }
            finally
            {
                // 确保只在成功获取锁的情况下才尝试释放，避免重复释放
                if (lockAcquired && _fileOperationLock.CurrentCount == 0)
                {
                    _fileOperationLock.Release();
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
            // 使用信号量确保同一时间只有一个文件操作请求，带30秒超时
            bool lockAcquired = false;
            try
            {
                if (!await _fileOperationLock.WaitAsync(TimeSpan.FromSeconds(30), ct))
                {
                    _log?.LogWarning("获取文件操作锁超时，当前锁状态: {CurrentCount}", _fileOperationLock.CurrentCount);
                    return new StorageUsageInfoData();
                }
                lockAcquired = true;

                // 检查连接状态
                if (!_communicationService.ConnectionManager.IsConnected)
                {
                    _log?.LogWarning("获取存储使用信息失败：未连接到服务器");
                    return new StorageUsageInfoData();
                }

                // 创建存储使用信息请求
                var request = new FileInfoRequest();

                // 发送获取文件信息命令并获取响应
                var response = await _communicationService.SendCommandWithResponseAsync<FileInfoResponse>(
                    FileCommands.FileInfoQuery, request, ct);

                if (response != null && response.IsSuccess)
                {
                    // 创建存储使用信息
                    var usageInfo = new StorageUsageInfoData();

                    // 计算存储使用情况
                    if (response.FileStorageInfos != null)
                    {
                        foreach (var fileInfo in response.FileStorageInfos)
                        {
                            // 直接累加文件大小
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
                // 确保只在成功获取锁的情况下才尝试释放，避免重复释放
                if (lockAcquired && _fileOperationLock.CurrentCount == 0)
                {
                    _fileOperationLock.Release();
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
                // 释放资源
                _fileOperationLock.Dispose();
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "释放FileManagementService资源时发生异常");
            }
        }
    }
}