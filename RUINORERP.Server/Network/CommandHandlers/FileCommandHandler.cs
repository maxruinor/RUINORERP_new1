using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.Server.Network.Models;
using RUINORERP.Server.Network.Services;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Commands.FileTransfer;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.PacketSpec.Models.Requests;
using System.Collections.Generic;
using System.Linq;
using MessagePack;

namespace RUINORERP.Server.Network.CommandHandlers
{
    /// <summary>
    /// 文件处理器 - 处理文件上传、下载、删除等业务逻辑
    /// </summary>
    [CommandHandler("FileCommandHandler", priority: 50)]
    public class FileCommandHandler : BaseCommandHandler
    {
        private readonly SessionService _sessionService;
        private readonly ILogger<FileCommandHandler> _logger;
        private readonly string _fileStoragePath;

        public FileCommandHandler(SessionService sessionService, ILogger<FileCommandHandler> logger = null)
        {
            _sessionService = sessionService;
            _logger = logger;
            _fileStoragePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FileStorage");

            // 确保文件存储目录存在
            if (!Directory.Exists(_fileStoragePath))
            {
                Directory.CreateDirectory(_fileStoragePath);
            }

            // 设置支持的命令
            SetSupportedCommands(
                FileCommands.FileUpload,
                FileCommands.FileDownload,
                FileCommands.FileDelete,
                FileCommands.FileInfoQuery,
                FileCommands.FileList
            );
        }

        /// <summary>
        /// 核心处理方法，根据命令类型分发到对应的处理函数
        /// </summary>
        protected override async Task<ResponseBase> OnHandleAsync(QueuedCommand cmd, CancellationToken cancellationToken)
        {
            try
            {
                var commandId = cmd.Packet.CommandId;

                if (cmd.Packet.Request is FileUploadRequest uploadRequest && commandId == FileCommands.FileUpload)
                {
                    return await HandleFileUploadAsync(uploadRequest, cmd.Packet.ExecutionContext, cancellationToken);
                }
                else if (cmd.Packet.Request is FileDownloadRequest downloadRequest && commandId == FileCommands.FileDownload)
                {
                    return await HandleFileDownloadAsync(downloadRequest, cmd.Packet.ExecutionContext, cancellationToken);
                }
                else if (cmd.Packet.Request is FileDeleteRequest deleteRequest && commandId == FileCommands.FileDelete)
                {
                    return await HandleFileDeleteAsync(deleteRequest, cmd.Packet.ExecutionContext, cancellationToken);
                }
                else if (cmd.Packet.Request is FileListRequest listRequest && commandId == FileCommands.FileList)
                {
                    return await HandleFileListAsync(listRequest, cmd.Packet.ExecutionContext, cancellationToken);
                }
                else if (cmd.Packet.Request is FileInfoRequest infoRequest && commandId == FileCommands.FileInfoQuery)
                {
                    return await HandleFileInfoAsync(infoRequest, cmd.Packet.ExecutionContext, cancellationToken);
                }
                else
                {
                    return ResponseBase.CreateError($"不支持的文件命令: {commandId.Name}")
                        .WithMetadata("ErrorCode", "UNSUPPORTED_FILE_COMMAND");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理文件命令时出错: {Message}", ex.Message);
                return CreateExceptionResponse(ex, "FILE_HANDLER_ERROR");
            }
        }

        /// <summary>
        /// 处理文件上传
        /// </summary>
        private async Task<ResponseBase> HandleFileUploadAsync(FileUploadRequest uploadRequest, CommandContext executionContext, CancellationToken cancellationToken)
        {
            try
            {
                if (uploadRequest == null || uploadRequest.Data == null)
                {
                    return FileUploadResponse.CreateFailure("文件上传请求格式错误");
                }

                // 生成唯一文件名
                var fileId = Guid.NewGuid().ToString();
                var fileExtension = Path.GetExtension(uploadRequest.FileName);
                var savedFileName = $"{fileId}{fileExtension}";
                var filePath = Path.Combine(_fileStoragePath, savedFileName);

                // 保存文件
                await File.WriteAllBytesAsync(filePath, uploadRequest.Data, cancellationToken);

                // 只记录关键信息，移除详细的文件ID日志
                _logger?.LogDebug("文件上传成功: {FileId}", fileId);

                return FileUploadResponse.CreateSuccess(fileId, "文件上传成功");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "文件上传时出错");
                return FileUploadResponse.CreateFailure(ex.Message);
            }
        }

        /// <summary>
        /// 处理文件下载
        /// </summary>
        private async Task<ResponseBase> HandleFileDownloadAsync(FileDownloadRequest downloadRequest, CommandContext executionContext, CancellationToken cancellationToken)
        {
            try
            {
                if (downloadRequest == null || string.IsNullOrEmpty(downloadRequest.FileId))
                {
                    return FileDownloadResponse.CreateFailure("文件下载请求格式错误");
                }

                // 查找文件
                var files = Directory.GetFiles(_fileStoragePath, $"{downloadRequest.FileId}.*");
                if (files.Length == 0)
                {
                    return FileDownloadResponse.CreateFailure("文件不存在");
                }

                var filePath = files[0];
                var fileData = await File.ReadAllBytesAsync(filePath, cancellationToken);
                var fileName = Path.GetFileName(filePath);

                // 创建响应数据
                var responseData = new FileDownloadResponseData
                {
                    FileName = fileName,
                    Data = fileData,
                    FileSize = fileData.Length,
                    ContentType = "application/octet-stream",
                    LastModified = File.GetLastWriteTime(filePath)
                };

                // 只记录关键信息，移除详细的文件ID日志
                _logger?.LogDebug("文件下载成功: {FileId}", downloadRequest.FileId);

                return FileDownloadResponse.CreateSuccess(responseData, "文件下载成功");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "文件下载时出错");
                return FileDownloadResponse.CreateFailure(ex.Message);
            }
        }

        /// <summary>
        /// 处理文件删除
        /// </summary>
        private async Task<ResponseBase> HandleFileDeleteAsync(FileDeleteRequest deleteRequest, CommandContext executionContext, CancellationToken cancellationToken)
        {
            try
            {
                if (deleteRequest == null || string.IsNullOrEmpty(deleteRequest.FileId))
                {
                    return FileDeleteResponse.CreateFailure("文件删除请求格式错误");
                }

                // 查找并删除文件
                var files = Directory.GetFiles(_fileStoragePath, $"{deleteRequest.FileId}.*");
                var deletedCount = 0;

                foreach (var filePath in files)
                {
                    try
                    {
                        File.Delete(filePath);
                        deletedCount++;
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, "删除文件失败: {FilePath}", filePath);
                    }
                }

                if (deletedCount > 0)
                {
                    // 只记录关键信息，简化日志内容
                    return FileDeleteResponse.CreateSuccess("文件删除成功");
                }
                else
                {
                    return FileDeleteResponse.CreateFailure("文件不存在");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "文件删除时出错");
                return FileDeleteResponse.CreateFailure(ex.Message);
            }
        }

        /// <summary>
        /// 处理文件列表
        /// </summary>
        private async Task<ResponseBase> HandleFileListAsync(FileListRequest listRequest, CommandContext executionContext, CancellationToken cancellationToken)
        {
            try
            {
                var files = Directory.GetFiles(_fileStoragePath);
                var fileList = files.Select(filePath =>
                {
                    var fileInfo = new FileInfo(filePath);
                    var fileNameWithoutExt = Path.GetFileNameWithoutExtension(filePath);

                    return new FileStorageInfo(filePath)
                    {
                        FileId = fileNameWithoutExt,
                        OriginalName = fileInfo.Name,
                        Size = fileInfo.Length,
                        UploadTime = fileInfo.CreationTime,
                        LastModified = fileInfo.LastWriteTime,
                        FilePath = filePath
                    };
                }).ToList();

                // 只记录关键信息，简化日志内容
                _logger?.LogDebug("获取文件列表成功，共 {Count} 个文件", fileList.Count);

                return FileListResponse.CreateSuccess(fileList, fileList.Count, listRequest.PageIndex, listRequest.PageSize, "获取文件列表成功");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "获取文件列表时出错");
                return FileListResponse.CreateFailure(ex.Message);
            }
        }

        /// <summary>
        /// 处理文件信息查询
        /// </summary>
        private async Task<ResponseBase> HandleFileInfoAsync(FileInfoRequest infoRequest, CommandContext executionContext, CancellationToken cancellationToken)
        {
            try
            {
                if (infoRequest == null || string.IsNullOrEmpty(infoRequest.FileId))
                {
                    return FileInfoResponse.CreateFailure("文件信息请求格式错误");
                }

                var files = Directory.GetFiles(_fileStoragePath, $"{infoRequest.FileId}.*");
                if (files.Length == 0)
                {
                    return FileInfoResponse.CreateFailure("文件不存在");
                }

                var filePath = files[0];
                var fileInfo = new FileInfo(filePath);

                var fileInfoData = new FileStorageInfo(filePath)
                {
                    FileId = infoRequest.FileId,
                    OriginalName = fileInfo.Name,
                    Size = fileInfo.Length,
                    UploadTime = fileInfo.CreationTime,
                    LastModified = fileInfo.LastWriteTime,
                    FilePath = filePath,
                    MimeType = "application/octet-stream"
                };

                // 只记录关键信息，移除详细的文件ID日志
                _logger?.LogDebug("获取文件信息成功: {FileId}", infoRequest.FileId);

                return FileInfoResponse.CreateSuccess(fileInfoData, "获取文件信息成功");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "获取文件信息时出错");
                return FileInfoResponse.CreateFailure(ex.Message);
            }
        }

        /// <summary>
        /// 创建统一的异常响应
        /// </summary>
        private ResponseBase CreateExceptionResponse(Exception ex, string errorCode)
        {
            return ResponseBase.CreateError($"[{ex.GetType().Name}] {ex.Message}")
                .WithMetadata("ErrorCode", errorCode)
                .WithMetadata("Exception", ex.Message)
                .WithMetadata("StackTrace", ex.StackTrace);
        }
    }
}