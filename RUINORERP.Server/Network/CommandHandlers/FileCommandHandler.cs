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
using RUINORERP.Model.Context;
using System.Windows.Forms;
using ApplicationContext = RUINORERP.Model.Context.ApplicationContext;

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
        private readonly ApplicationContext _applicationContext;
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
                // 统一使用System.Environment.ExpandEnvironmentVariables
                return System.Environment.ExpandEnvironmentVariables(path);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "解析环境变量失败，使用原始路径: {Path}", path);
                return path;
            }
        }


        public FileCommandHandler(
            ApplicationContext applicationContext,
            SessionService sessionService,
            ILogger<FileCommandHandler> logger = null,
            tb_FS_FileStorageInfoController<tb_FS_FileStorageInfo> fileStorageInfoController = null,
            tb_FS_BusinessRelationController<tb_FS_BusinessRelation> businessRelationController = null,
            tb_FS_FileStorageVersionController<tb_FS_FileStorageVersion> fileStorageVersionController = null)
        {
            _applicationContext = applicationContext;
            _sessionService = sessionService;
            _logger = logger;

            // 统一路径初始化逻辑
            try
            {
                _serverConfig = Startup.GetFromFac<ServerConfig>();
                _fileStoragePath = ResolveEnvironmentVariables(_serverConfig.FileStoragePath);

                // 确保存储目录存在
                if (!string.IsNullOrEmpty(_fileStoragePath) && !Directory.Exists(_fileStoragePath))
                {
                    Directory.CreateDirectory(_fileStoragePath);
                    _logger.LogInformation("创建文件存储目录: {FileStoragePath}", _fileStoragePath);
                }

                _logger.LogInformation("文件存储路径已设置为: {FileStoragePath}", _fileStoragePath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "初始化文件存储路径失败");
                _fileStoragePath = "Files"; // 默认路径
                if (!Directory.Exists(_fileStoragePath))
                {
                    Directory.CreateDirectory(_fileStoragePath);
                }
            }

            // 注入业务控制器
            _fileStorageInfoController = fileStorageInfoController;
            _businessRelationController = businessRelationController;
            _fileStorageVersionController = fileStorageVersionController;

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
            _logger?.LogInformation("接收到文件命令: {CommandId}", cmd.Packet.CommandId);

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
                    _logger?.LogWarning("接收到不支持的文件命令: {CommandId}", commandId);
                    return ResponseFactory.CreateSpecificErrorResponse(cmd.Packet, $"不支持的文件命令: {commandId}");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理文件命令时出错，命令: {CommandId}", cmd.Packet.CommandId);
                return ResponseFactory.CreateSpecificErrorResponse(cmd.Packet.ExecutionContext, ex, $"处理文件命令 {cmd.Packet.CommandId} 时出错: {ex.Message}");
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
            _logger?.LogInformation("开始处理文件上传请求，文件数量: {FileCount}", uploadRequest.FileStorageInfos?.Count ?? 0);

            var responseData = new FileUploadResponse();
            try
            {
                if (uploadRequest.FileStorageInfos == null || uploadRequest.FileStorageInfos.Count == 0)
                {
                    _logger?.LogWarning("文件上传请求中未包含任何文件数据");
                    return ResponseFactory.CreateSpecificErrorResponse<FileUploadResponse>("文件上传请求中未包含任何文件数据");
                }

                for (int i = 0; i < uploadRequest.FileStorageInfos.Count; i++)
                {
                    var FileStorageInfo = uploadRequest.FileStorageInfos[i];
                    #region 保存单文件 
                    _logger?.LogInformation("处理文件[{Index}]：{FileName}", i + 1, FileStorageInfo.OriginalFileName);

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
                        _logger?.LogInformation("创建分类存储目录: {CategoryPath}", categoryPath);
                    }

                    var filePath = Path.Combine(categoryPath, savedFileName);
                    _logger?.LogInformation("文件将保存至: {FilePath}", filePath);

                    // 直接计算文件内容哈希值，无需保存后再计算
                    var contentHash = FileManagementHelper.CalculateContentHash(FileStorageInfo.FileData);
                    _logger?.LogInformation("文件哈希计算完成: {ContentHash}, 文件大小: {FileSize} bytes", contentHash, FileStorageInfo.FileData.Length);

                    // 保存文件
                    await File.WriteAllBytesAsync(filePath, FileStorageInfo.FileData, cancellationToken);
                    _logger?.LogInformation("文件物理保存成功");

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
                        _logger?.LogWarning("文件信息保存到数据库失败: {FileName}, 错误: {Error}",
                            FileStorageInfo.OriginalFileName, saveResult.ErrorMsg);
                        continue;
                    }

                    responseData.FileStorageInfos.Add(saveResult.ReturnObject);
                    _logger?.LogInformation("文件信息保存到数据库成功，FileId: {FileId}", fileStorageInfo.FileId);

                    // 在服务器端创建业务关联
                    if (!string.IsNullOrEmpty(uploadRequest.BusinessNo) && uploadRequest.BusinessType.HasValue)
                    {
                        var businessRelation = new tb_FS_BusinessRelation
                        {
                            BusinessType = uploadRequest.BusinessType.Value,
                            BusinessNo = uploadRequest.BusinessNo,
                            FileId = fileStorageInfo.FileId,
                            IsMainFile = (i == 0), // 第一个文件为主文件
                            RelatedField = uploadRequest.RelatedField, // 设置关联字段，必填项
                            Created_at = DateTime.Now,
                            Created_by = uploadRequest.Created_by ?? executionContext.UserId
                        };

                        // 保存业务关联
                        var relationResult = await _businessRelationController.SaveOrUpdate(businessRelation);
                        if (!relationResult.Succeeded)
                        {
                            _logger?.LogWarning("业务关联保存失败: {FileName}, 错误: {Error}",
                                FileStorageInfo.OriginalFileName, relationResult.ErrorMsg);
                        }
                        else
                        {
                            _logger?.LogInformation("业务关联创建成功: FileId={FileId}, BusinessNo={BusinessNo}, BusinessType={BusinessType}",
                                fileStorageInfo.FileId, uploadRequest.BusinessNo, uploadRequest.BusinessType.Value);
                        }
                    }

                    #endregion
                }
                responseData.Message = $"文件上传成功，共成功上传 {responseData.FileStorageInfos.Count} 个文件";
                _logger?.LogInformation("文件上传处理完成，成功上传 {SuccessCount} 个文件", responseData.FileStorageInfos.Count);
                return responseData;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "文件上传处理失败");
                return ResponseFactory.CreateSpecificErrorResponse<FileUploadResponse>($"文件上传失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 处理文件下载
        /// </summary>
        private async Task<FileDownloadResponse> HandleFileDownloadAsync(FileDownloadRequest downloadRequest, CommandContext executionContext, CancellationToken cancellationToken)
        {
            _logger?.LogInformation("开始处理文件下载请求，FileId: {FileId}",
                downloadRequest?.FileStorageInfo?.FileId);

            try
            {
                if (downloadRequest == null || downloadRequest.FileStorageInfo == null)
                {
                    _logger?.LogWarning("文件下载请求格式错误");
                    return FileDownloadResponse.CreateFailure("文件下载请求格式错误");
                }

                // 优先从数据库获取准确的文件路径
                string filePath = null;
                if (downloadRequest.FileStorageInfo.FileId > 0)
                {
                    _logger?.LogInformation("从数据库查询文件信息，FileId: {FileId}", downloadRequest.FileStorageInfo.FileId);
                    var fileInfos = await _fileStorageInfoController.BaseQueryAsync(
                        $"FileId = {downloadRequest.FileStorageInfo.FileId} AND Status = 1");
                    if (fileInfos != null && fileInfos.Count > 0)
                    {
                        var fileInfo = fileInfos[0] as tb_FS_FileStorageInfo;
                        if (fileInfo != null && !string.IsNullOrEmpty(fileInfo.StoragePath))
                        {
                            filePath = fileInfo.StoragePath;
                            _logger?.LogInformation("从数据库获取到文件路径: {FilePath}", filePath);
                        }
                    }
                    else
                    {
                        _logger?.LogWarning("数据库中未找到对应的文件记录，FileId: {FileId}", downloadRequest.FileStorageInfo.FileId);
                    }
                }

                // 如果数据库中没有路径，尝试根据存储文件名查找
                if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                {
                    _logger?.LogInformation("尝试在文件系统中查找文件");
                    filePath = await FindPhysicalFileAsync(downloadRequest.FileStorageInfo);
                }

                if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                {
                    _logger?.LogWarning("文件不存在或无法访问");
                    return FileDownloadResponse.CreateFailure("文件不存在或无法访问");
                }

                _logger?.LogInformation("文件找到，开始读取文件数据: {FilePath}", filePath);
                var fileData = await File.ReadAllBytesAsync(filePath, cancellationToken);
                downloadRequest.FileStorageInfo.FileData = fileData;
                _logger?.LogInformation("文件数据读取成功，大小: {FileSize} bytes", fileData.Length);

                // 创建响应数据
                var response = new FileDownloadResponse
                {
                    FileStorageInfos = new List<tb_FS_FileStorageInfo> { downloadRequest.FileStorageInfo },
                    Message = "文件下载成功",
                    IsSuccess = true
                };
                _logger?.LogInformation("文件下载处理完成，FileId: {FileId}", downloadRequest.FileStorageInfo.FileId);
                return response;
            }
            catch (IOException ioEx)
            {
                _logger?.LogError(ioEx, "文件IO操作失败");
                return FileDownloadResponse.CreateFailure($"文件读写失败: {ioEx.Message}");
            }
            catch (UnauthorizedAccessException authEx)
            {
                _logger?.LogError(authEx, "文件访问权限不足");
                return FileDownloadResponse.CreateFailure($"文件访问权限不足: {authEx.Message}");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "文件下载处理失败");
                return FileDownloadResponse.CreateFailure($"文件下载失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 根据文件信息查找物理文件
        /// 搜索策略：1. 分类目录 2. 根目录
        /// </summary>
        /// <param name="fileInfo">文件信息</param>
        /// <returns>找到的文件路径，找不到返回null</returns>
        private async Task<string> FindPhysicalFileAsync(tb_FS_FileStorageInfo fileInfo)
        {
            _logger?.LogInformation("开始执行文件查找策略，FileId: {FileId}", fileInfo?.FileId);
            // 搜索策略：1. 分类目录 2. 根目录

            var searchPatterns = new List<string>();

            // 根据存储文件名搜索（带扩展名）
            if (!string.IsNullOrEmpty(fileInfo.StorageFileName))
            {
                searchPatterns.Add($"{fileInfo.StorageFileName}.*");
            }

            // 根据文件ID搜索
            searchPatterns.Add($"{fileInfo.FileId}.*");

            // 搜索目录列表
            var searchDirectories = new List<string>();

            // 分类目录
            if (fileInfo.BusinessType.HasValue)
            {
                searchDirectories.Add(GetCategoryPath(fileInfo.BusinessType.Value.ToString()));
            }

            // 根目录
            searchDirectories.Add(_fileStoragePath);

            // 搜索所有可能的目录和模式
            foreach (var directory in searchDirectories)
            {
                if (!Directory.Exists(directory)) continue;

                _logger?.LogDebug("在目录中搜索: {Directory}", directory);
                foreach (var pattern in searchPatterns)
                {
                    try
                    {
                        _logger?.LogDebug("使用模式搜索: {Pattern}", pattern);
                        var files = Directory.GetFiles(directory, pattern);
                        if (files.Length > 0)
                        {
                            _logger?.LogInformation("找到匹配的文件: {FilePath}", files[0]);
                            return files[0];
                        }
                    }
                    catch (Exception ex)
                    {
                        // 记录错误但继续搜索
                        _logger?.LogWarning(ex, "搜索文件时出错: 目录={Directory}, 模式={Pattern}", directory, pattern);
                    }
                }
            }

            _logger?.LogWarning("未找到匹配的文件");
            return null;
        }

        /// <summary>
        /// 处理文件删除
        /// </summary>
        private async Task<ResponseBase> HandleFileDeleteAsync(FileDeleteRequest deleteRequest, CommandContext executionContext, CancellationToken cancellationToken)
        {
            _logger?.LogInformation("开始处理文件删除请求，FileId: {FileId}", deleteRequest?.FileId);

            try
            {
                if (deleteRequest == null || string.IsNullOrEmpty(deleteRequest.FileId))
                {
                    _logger?.LogWarning("文件删除请求格式错误");
                    return FileDeleteResponse.CreateFailure("文件删除请求格式错误");
                }

                // 查找并删除文件（在所有可能的目录中）
                var deletedCount = 0;
                var searchPaths = new List<string> { _fileStoragePath };

                // 添加分类目录
                searchPaths.Add(GetCategoryPath("paymentvoucher"));
                searchPaths.Add(GetCategoryPath("productimage"));
                searchPaths.Add(GetCategoryPath("bommanual"));
                _logger?.LogInformation("删除搜索目录数: {DirectoryCount}", searchPaths.Count);

                foreach (var searchPath in searchPaths)
                {
                    if (Directory.Exists(searchPath))
                    {
                        _logger?.LogDebug("在目录中搜索待删除文件: {Directory}", searchPath);
                        var files = Directory.GetFiles(searchPath, $"{deleteRequest.FileId}.*");
                        _logger?.LogDebug("找到待删除文件数: {FileCount}", files.Length);

                        foreach (var filePath in files)
                        {
                            try
                            {
                                _logger?.LogInformation("删除文件: {FilePath}", filePath);
                                File.Delete(filePath);
                                deletedCount++;
                                _logger?.LogInformation("文件删除成功: {FilePath}", filePath);
                            }
                            catch (Exception ex)
                            {
                                // 记录错误但继续删除其他文件
                                _logger?.LogError(ex, "删除文件失败: {FilePath}", filePath);
                            }
                        }
                    }
                    else
                    {
                        _logger?.LogDebug("跳过不存在的目录: {Directory}", searchPath);
                    }
                }

                // 更新数据库记录状态
                try
                {
                    _logger?.LogInformation("开始更新数据库文件状态");
                    // 查找文件信息
                    var fileInfoQuery = await _fileStorageInfoController.BaseQueryAsync($"HashValue = '{deleteRequest.FileId}'");
                    if (fileInfoQuery != null && fileInfoQuery.Count > 0)
                    {
                        var fileInfo = fileInfoQuery[0] as tb_FS_FileStorageInfo;
                        fileInfo.Status = 0; // 标记为删除
                        await _fileStorageInfoController.SaveOrUpdate(fileInfo);
                        _logger?.LogInformation("数据库文件状态更新成功，FileId: {FileId}", fileInfo.FileId);
                    }
                    else
                    {
                        _logger?.LogWarning("未在数据库中找到对应的文件记录");
                    }
                }
                catch (Exception ex)
                {
                    // 记录数据库错误
                    _logger?.LogError(ex, "更新数据库文件状态失败");
                }

                // 删除相关的业务关联记录
                try
                {
                    _logger?.LogInformation("开始删除业务关联记录");

                    //{deleteRequest.FileId}
                    //await _applicationContext.Db.DeleteNav<tb_FS_BusinessRelation>(m => m.FileId == entity.RelationId)
                    //.IncludesAllFirstLayer()//自动更新导航 只能两层。这里项目中有时会失效，具体看文档
                    //              .ExecuteCommandAsync();

                    _logger?.LogInformation("业务关联记录删除成功");
                }
                catch (Exception ex)
                {
                    // 记录数据库错误
                    _logger?.LogError(ex, "删除业务关联记录失败");
                }

                if (deletedCount > 0)
                {
                    _logger?.LogInformation("文件删除处理完成，成功删除 {DeletedCount} 个文件", deletedCount);
                    return FileDeleteResponse.CreateSuccess("文件删除成功");
                }
                else
                {
                    return FileDeleteResponse.CreateFailure("文件不存在");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "文件删除处理失败");
                return FileDeleteResponse.CreateFailure($"文件删除失败: {ex.Message}");
            }
        }


    }
}