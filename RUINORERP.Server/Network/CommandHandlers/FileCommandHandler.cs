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
using RUINORERP.Model;
using RUINORERP.Business;
using RUINORERP.IServices;
using Azure;
using SqlSugar;
using RUINORERP.Business.Config;

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
        // 添加业务控制器用于数据库操作
        private readonly tb_FS_FileStorageInfoController<tb_FS_FileStorageInfo> _fileStorageInfoController;
        private readonly tb_FS_BusinessRelationController<tb_FS_BusinessRelation> _businessRelationController;
        private readonly tb_FS_FileStorageVersionController<tb_FS_FileStorageVersion> _fileStorageVersionController;

        /// <summary>
        /// 解析路径中的环境变量
        /// 支持 %ENV_VAR% 的格式
        /// </summary>
        /// <param name="path">包含环境变量的路径</param>
        /// <returns>解析后的实际路径</returns>
        private string ResolveEnvironmentVariables(string path)
        {
            if (string.IsNullOrEmpty(path))
                return path;

            try
            {
                return Environment.ExpandEnvironmentVariables(path);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "解析环境变量失败，使用原始路径: {Path}", path);
                return path;
            }
        }


        public FileCommandHandler(
            SessionService sessionService,
            ILogger<FileCommandHandler> logger = null,
            tb_FS_FileStorageInfoController<tb_FS_FileStorageInfo> fileStorageInfoController = null,
            tb_FS_BusinessRelationController<tb_FS_BusinessRelation> businessRelationController = null,
            tb_FS_FileStorageVersionController<tb_FS_FileStorageVersion> fileStorageVersionController = null)
        {
            _sessionService = sessionService;
            _logger = logger;

            // 从配置管理器服务获取服务器配置
            try
            {
                _serverConfig = Startup.GetFromFac<ServerConfig>();
                // 处理环境变量并验证路径
                _fileStoragePath = ResolveEnvironmentVariables(_serverConfig.FileStoragePath);

                // 确保存储目录存在
                if (!string.IsNullOrEmpty(_fileStoragePath) && !Directory.Exists(_fileStoragePath))
                {
                    Directory.CreateDirectory(_fileStoragePath);
                }

                _logger.LogInformation("文件存储路径已设置为: {FileStoragePath}", _fileStoragePath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取服务器配置失败，使用默认路径");
                _serverConfig = new ServerConfig();
                _fileStoragePath = ResolveEnvironmentVariables(_serverConfig.FileStoragePath);
            }

            // 注入业务控制器
            _fileStorageInfoController = fileStorageInfoController;
            _businessRelationController = businessRelationController;
            _fileStorageVersionController = fileStorageVersionController;


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
                //else if (cmd.Packet.Request is FileListRequest listRequest && commandId == FileCommands.FileList)
                //{
                //    return await HandleFileListAsync(listRequest, cmd.Packet.ExecutionContext, cancellationToken);
                //}
                //else if (cmd.Packet.Request is FileInfoRequest infoRequest && commandId == FileCommands.FileInfoQuery)
                //{
                //    return await HandleFileInfoAsync(infoRequest, cmd.Packet.ExecutionContext, cancellationToken);
                //}
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
        private string GetCategoryPath(string BizCategory)
        {
            return Path.Combine(_fileStoragePath, BizCategory);



        }

        /// <summary>
        /// 处理文件上传
        /// </summary>
        private async Task<IResponse> HandleFileUploadAsync(FileUploadRequest uploadRequest, CommandContext executionContext, CancellationToken cancellationToken)
        {

            var responseData = new FileUploadResponse();
            try
            {

                for (int i = 0; i < uploadRequest.FileStorageInfos.Count; i++)
                {
                    var FileStorageInfo = uploadRequest.FileStorageInfos[i];
                    #region 保存单文件 

                    // 生成唯一文件名
                    var fileId = Guid.NewGuid().ToString();
                    var fileExtension = Path.GetExtension(FileStorageInfo.OriginalFileName);
                    //扩展名。看如何取好。原来名有没有带？
                    var savedFileName = $"{fileId}{fileExtension}";

                    // 根据分类确定存储路径
                    var categoryPath = GetCategoryPath(FileStorageInfo.BusinessType.ToString());
                    if (!Directory.Exists(categoryPath))
                    {
                        Directory.CreateDirectory(categoryPath);
                    }

                    var filePath = Path.Combine(categoryPath, savedFileName);

                    // 直接计算文件内容哈希值，无需保存后再计算
                    var contentHash = FileManagementHelper.CalculateContentHash(FileStorageInfo.FileData);

                    // 保存文件
                    await File.WriteAllBytesAsync(filePath, FileStorageInfo.FileData, cancellationToken);

                    // 创建文件信息实体并保存到数据库
                    var fileStorageInfo = FileManagementHelper.CreateFileStorageInfo(
                        FileStorageInfo.OriginalFileName,  // fileName
                        FileStorageInfo.FileData.Length,   // fileSize
                        fileExtension.TrimStart('.'),     // fileType
                        filePath,                          // storagePath
                        FileStorageInfo.BusinessType != null ? FileStorageInfo.BusinessType.GetHashCode() : 0, // businessType
                        executionContext.UserId,          // userId
                        contentHash);                     // contentHash (可选)

                    // 设置哈希值（保持与旧代码兼容）
                    fileStorageInfo.HashValue = contentHash;

                    // 保存文件信息到数据库

                    var saveResult = await _fileStorageInfoController.SaveOrUpdate(fileStorageInfo);
                    if (!saveResult.Succeeded)
                    {
                        _logger?.LogWarning("文件信息保存到数据库失败: {FileName}", FileStorageInfo.OriginalFileName);
                    }
                    else
                    {
                        responseData.FileStorageInfos.Add(saveResult.ReturnObject);
                        // 创建业务关联记录
                        var businessRelation = new tb_FS_BusinessRelation
                        {
                            BusinessType = (int)uploadRequest.BusinessType,
                            BusinessNo = uploadRequest.BusinessNo,
                            FileId = fileStorageInfo.FileId,
                            IsMainFile = (i == 0),
                            Created_at = DateTime.Now,
                            Created_by = uploadRequest.Created_by
                        };

                        // 保存业务关联
                        await _businessRelationController.SaveOrUpdate(businessRelation);
                    }

                    #endregion
                }
                responseData.Message = "文件上传成功";
                return responseData;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "文件上传时出错");
                return ResponseFactory.CreateSpecificErrorResponse<FileUploadResponse>(ex.Message);
            }
        }

        /// <summary>
        /// 处理文件下载
        /// </summary>
        private async Task<FileDownloadResponse> HandleFileDownloadAsync(FileDownloadRequest downloadRequest, CommandContext executionContext, CancellationToken cancellationToken)
        {
            FileDownloadResponse downloadResponse = new FileDownloadResponse();
            try
            {
                if (downloadRequest == null || downloadRequest.FileStorageInfo == null)
                {
                    return FileDownloadResponse.CreateFailure("文件下载请求格式错误");
                }
                // 根据分类确定存储路径
                var categoryPath = GetCategoryPath(downloadRequest.FileStorageInfo.BusinessType.Value.ToString());

                var files = Directory.GetFiles(categoryPath, $"{downloadRequest.FileStorageInfo.StorageFileName}.*");

                // 如果在分类目录中找不到，尝试在根目录查找
                if (files.Length == 0 && downloadRequest.FileStorageInfo.BusinessType.HasValue)
                {
                    files = Directory.GetFiles(_fileStoragePath, $"{downloadRequest.FileStorageInfo.FileId}.*");
                }

                if (files.Length == 0)
                {
                    return FileDownloadResponse.CreateFailure("文件不存在");
                }

                var filePath = files[0];
                var fileData = await File.ReadAllBytesAsync(filePath, cancellationToken);
                var fileName = Path.GetFileName(filePath);
                downloadRequest.FileStorageInfo.FileData = fileData;
                // 创建响应数据
                downloadResponse.FileStorageInfos.Add(downloadRequest.FileStorageInfo);
                downloadResponse.Message = "文件下载成功";

                return downloadResponse;
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


    }
}