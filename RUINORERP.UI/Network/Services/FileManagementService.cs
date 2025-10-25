using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Commands.FileTransfer;
using RUINORERP.PacketSpec.Core;
using RUINORERP.PacketSpec.Errors;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.UI.Network;
using RUINORERP.UI.Network.Exceptions;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

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

            // 使用信号量确保同一时间只有一个文件操作请求
            await _fileOperationLock.WaitAsync(ct);
            try
            {
                // 检查连接状态
                if (!_communicationService.IsConnected)
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
            catch (Exception ex)
            {
                _log?.LogError(ex, "文件上传过程中发生未预期的异常");
                return ResponseFactory.CreateSpecificErrorResponse<FileUploadResponse>("文件上传过程中发生错误，请稍后重试");
            }
            finally
            {
                _fileOperationLock.Release();
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

            if (request.FileStorageInfo.FileId==0)
                throw new ArgumentException("文件ID不能为空", nameof(request.FileStorageInfo));

            // 使用信号量确保同一时间只有一个文件操作请求
            await _fileOperationLock.WaitAsync(ct);
            try
            {
                // 检查连接状态
                if (!_communicationService.IsConnected)
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
            catch (Exception ex)
            {
                _log?.LogError(ex, "文件下载过程中发生未预期的异常");
                return FileDownloadResponse.CreateFailure("文件下载过程中发生错误，请稍后重试");
            }
            finally
            {
                _fileOperationLock.Release();
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

            if (string.IsNullOrEmpty(request.FileId))
                throw new ArgumentException("文件ID不能为空", nameof(request.FileId));

            // 使用信号量确保同一时间只有一个文件操作请求
            await _fileOperationLock.WaitAsync(ct);
            try
            {
                // 检查连接状态
                if (!_communicationService.IsConnected)
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
            catch (Exception ex)
            {
                _log?.LogError(ex, "文件删除过程中发生未预期的异常");
                return FileDeleteResponse.CreateFailure("文件删除过程中发生错误，请稍后重试");
            }
            finally
            {
                _fileOperationLock.Release();
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

            if (request.FileStorageInfo.FileId==0)
                throw new ArgumentException("文件ID不能为空", nameof(request.FileStorageInfo.FileId));

            // 使用信号量确保同一时间只有一个文件操作请求
            await _fileOperationLock.WaitAsync(ct);
            try
            {
                // 检查连接状态
                if (!_communicationService.IsConnected)
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
            catch (Exception ex)
            {
                _log?.LogError(ex, "获取文件信息过程中发生未预期的异常");
                return FileInfoResponse.CreateFailure("获取文件信息过程中发生错误，请稍后重试");
            }
            finally
            {
                _fileOperationLock.Release();
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

            // 使用信号量确保同一时间只有一个文件操作请求
            await _fileOperationLock.WaitAsync(ct);
            try
            {
                // 检查连接状态
                if (!_communicationService.IsConnected)
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
            catch (Exception ex)
            {
                _log?.LogError(ex, "获取文件列表过程中发生未预期的异常");
                return FileListResponse.CreateFailure("获取文件列表过程中发生错误，请稍后重试");
            }
            finally
            {
                _fileOperationLock.Release();
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