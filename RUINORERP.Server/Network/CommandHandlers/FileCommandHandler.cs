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
using RUINORERP.Model.ConfigModel;
using RUINORERP.Server.Helpers;

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
        private readonly ServerConfig _serverConfig;

        public FileCommandHandler(SessionService sessionService, ILogger<FileCommandHandler> logger = null)
        {
            _sessionService = sessionService;
            _logger = logger;
            
            // 从配置管理器获取服务器配置
            _serverConfig = Startup.GetFromFac<ServerConfig>() ?? new ServerConfig();
            _fileStoragePath = _serverConfig.FileStoragePath;

            // 确保文件存储目录存在
            if (!Directory.Exists(_fileStoragePath))
            {
                Directory.CreateDirectory(_fileStoragePath);
            }
            
            // 初始化文件存储路径
            FileStorageHelper.InitializeStoragePath(_serverConfig);

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
        protected override async Task<IResponse> OnHandleAsync(QueuedCommand cmd, CancellationToken cancellationToken)
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
                    return ResponseFactory.CreateSpecificErrorResponse(cmd.Packet, "不支持的文件命令");
                }
            }
            catch (Exception ex)
            {
                return ResponseFactory.CreateSpecificErrorResponse(cmd.Packet.ExecutionContext, ex, "处理文件命令时出错");
            }
        }

        /// <summary>
        /// 获取文件存储路径
        /// </summary>
        private string GetCategoryPath(string category)
        {
            switch (category?.ToLower())
            {
                case "paymentvoucher":
                    return Path.Combine(_fileStoragePath, _serverConfig.PaymentVoucherPath);
                case "productimage":
                    return Path.Combine(_fileStoragePath, _serverConfig.ProductImagePath);
                case "bommanual":
                    return Path.Combine(_fileStoragePath, _serverConfig.BOMManualPath);
                default:
                    return _fileStoragePath;
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

                // 检查文件大小限制
                if (uploadRequest.FileSize > _serverConfig.MaxFileSizeMB * 1024 * 1024)
                {
                    return FileUploadResponse.CreateFailure($"文件大小超过限制 ({_serverConfig.MaxFileSizeMB}MB)");
                }

                // 生成唯一文件名
                var fileId = Guid.NewGuid().ToString();
                var fileExtension = Path.GetExtension(uploadRequest.FileName);
                var savedFileName = $"{fileId}{fileExtension}";
                
                // 根据分类确定存储路径
                var categoryPath = GetCategoryPath(uploadRequest.Category);
                if (!Directory.Exists(categoryPath))
                {
                    Directory.CreateDirectory(categoryPath);
                }
                
                var filePath = Path.Combine(categoryPath, savedFileName);

                // 保存文件
                await File.WriteAllBytesAsync(filePath, uploadRequest.Data, cancellationToken);

                // 只记录关键信息，移除详细的文件ID日志
                _logger?.LogDebug("文件上传成功: {FileId}", fileId);

                // 返回文件路径信息
                var responseData = new FileUploadResponseData
                {
                    FileId = fileId,
                    FilePath = filePath,
                    FileUrl = $"/files/{uploadRequest.Category}/{fileId}{fileExtension}"
                };

                return new FileUploadResponse(true, "文件上传成功", responseData, 200);
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

                // 根据分类确定存储路径
                var categoryPath = GetCategoryPath(downloadRequest.Category);
                var files = Directory.GetFiles(categoryPath, $"{downloadRequest.FileId}.*");
                
                // 如果在分类目录中找不到，尝试在根目录查找
                if (files.Length == 0 && !string.IsNullOrEmpty(downloadRequest.Category))
                {
                    files = Directory.GetFiles(_fileStoragePath, $"{downloadRequest.FileId}.*");
                }
                
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
                    LastModified = File.GetLastWriteTime(filePath),
                    Category = downloadRequest.Category,
                    BusinessId = downloadRequest.BusinessId
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

                // 查找并删除文件（在所有可能的目录中）
                var deletedCount = 0;
                var searchPaths = new List<string> { _fileStoragePath };
                
                // 添加分类目录
                searchPaths.Add(GetCategoryPath("paymentvoucher"));
                searchPaths.Add(GetCategoryPath("productimage"));
                searchPaths.Add(GetCategoryPath("bommanual"));

                foreach (var searchPath in searchPaths)
                {
                    if (Directory.Exists(searchPath))
                    {
                        var files = Directory.GetFiles(searchPath, $"{deleteRequest.FileId}.*");
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
                var fileList = new List<FileStorageInfo>();
                
                // 确定搜索路径
                var searchPaths = new List<string>();
                
                if (!string.IsNullOrEmpty(listRequest.Category))
                {
                    // 如果指定了分类，只搜索该分类目录
                    searchPaths.Add(GetCategoryPath(listRequest.Category));
                }
                else
                {
                    // 如果未指定分类，搜索所有目录
                    searchPaths.Add(_fileStoragePath);
                    searchPaths.Add(GetCategoryPath("paymentvoucher"));
                    searchPaths.Add(GetCategoryPath("productimage"));
                    searchPaths.Add(GetCategoryPath("bommanual"));
                }

                // 收集所有文件信息
                foreach (var searchPath in searchPaths)
                {
                    if (Directory.Exists(searchPath))
                    {
                        var files = Directory.GetFiles(searchPath);
                        foreach (var filePath in files)
                        {
                            var fileInfo = new FileInfo(filePath);
                            var fileNameWithoutExt = Path.GetFileNameWithoutExtension(filePath);

                            fileList.Add(new FileStorageInfo(filePath)
                            {
                                FileId = fileNameWithoutExt,
                                OriginalName = fileInfo.Name,
                                Size = fileInfo.Length,
                                UploadTime = fileInfo.CreationTime,
                                LastModified = fileInfo.LastWriteTime,
                                FilePath = filePath,
                                Category = GetCategoryFromPath(searchPath)
                            });
                        }
                    }
                }

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
        /// 根据路径获取分类名称
        /// </summary>
        private string GetCategoryFromPath(string path)
        {
            if (path.EndsWith(_serverConfig.PaymentVoucherPath))
                return "PaymentVoucher";
            if (path.EndsWith(_serverConfig.ProductImagePath))
                return "ProductImage";
            if (path.EndsWith(_serverConfig.BOMManualPath))
                return "BOMManual";
            return "General";
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

                // 查找文件（在所有可能的目录中）
                string filePath = null;
                var searchPaths = new List<string> { _fileStoragePath };
                
                // 添加分类目录
                searchPaths.Add(GetCategoryPath("paymentvoucher"));
                searchPaths.Add(GetCategoryPath("productimage"));
                searchPaths.Add(GetCategoryPath("bommanual"));

                foreach (var searchPath in searchPaths)
                {
                    if (Directory.Exists(searchPath))
                    {
                        var files = Directory.GetFiles(searchPath, $"{infoRequest.FileId}.*");
                        if (files.Length > 0)
                        {
                            filePath = files[0];
                            break;
                        }
                    }
                }

                if (string.IsNullOrEmpty(filePath))
                {
                    return FileInfoResponse.CreateFailure("文件不存在");
                }

                var fileInfo = new FileInfo(filePath);
                var category = GetCategoryFromPath(Path.GetDirectoryName(filePath));

                var fileInfoData = new FileStorageInfo(filePath)
                {
                    FileId = infoRequest.FileId,
                    OriginalName = fileInfo.Name,
                    Size = fileInfo.Length,
                    UploadTime = fileInfo.CreationTime,
                    LastModified = fileInfo.LastWriteTime,
                    FilePath = filePath,
                    MimeType = "application/octet-stream",
                    Category = category
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
    }
}