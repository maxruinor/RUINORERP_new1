using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.Server.Network.Models;
using RUINORERP.Server.Network.Services;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Commands;
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
                    _logger.Debug("创建文件存储目录: {FileStoragePath}", _fileStoragePath);
                }

                _logger.Debug("文件存储路径已设置为: {FileStoragePath}", _fileStoragePath);
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
            _fileStorageInfoController = fileStorageInfoController ?? throw new ArgumentNullException(nameof(fileStorageInfoController));
            _businessRelationController = businessRelationController ?? throw new ArgumentNullException(nameof(businessRelationController));
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
            _logger?.Debug("接收到文件命令: {CommandId}", cmd.Packet.CommandId);

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
        /// 实现按业务类型和时间（YYMM）分目录的策略
        /// </summary>
        private string GetCategoryPath(string BizCategory, DateTime? dateTime = null)
        {
            // 使用传入的时间或当前时间
            var now = dateTime ?? DateTime.Now;
            // 格式化为YYMM
            var timeFolder = now.ToString("yyyyMM");

            // 构建路径：基础路径/业务类型/YYMM
            return Path.Combine(_fileStoragePath, BizCategory, timeFolder);
        }

        /// <summary>
        /// 获取文件存储路径（兼容旧版本调用）
        /// </summary>
        private string GetCategoryPath(string BizCategory)
        {
            return GetCategoryPath(BizCategory, null);
        }

        /// <summary>
        /// 处理文件上传
        /// </summary>
        private async Task<IResponse> HandleFileUploadAsync(FileUploadRequest uploadRequest, CommandContext executionContext, CancellationToken cancellationToken)
        {
            _logger?.Debug("开始处理文件上传请求，文件数量: {FileCount}", uploadRequest.FileStorageInfos?.Count ?? 0);

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
                    _logger?.Debug("处理文件[{Index}]：{FileName}", i + 1, FileStorageInfo.OriginalFileName);

                    // 生成唯一文件名
                    var fileId = Guid.NewGuid().ToString();
                    var fileExtension = Path.GetExtension(FileStorageInfo.OriginalFileName);
                    //扩展名。看如何取好。原来名有没有带？
                    var savedFileName = $"{fileId}{fileExtension}";

                    // 根据分类和当前时间确定存储路径（按YYMM分目录）
                    var fileCreateTime = DateTime.Now;
                    var categoryPath = GetCategoryPath(FileStorageInfo.BusinessType.ToString(), fileCreateTime);
                    if (!Directory.Exists(categoryPath))
                    {
                        Directory.CreateDirectory(categoryPath);
                        _logger?.Debug("创建分类存储目录: {CategoryPath}", categoryPath);
                    }

                    var filePath = Path.Combine(categoryPath, savedFileName);
                    _logger?.Debug("文件将保存至: {FilePath}", filePath);

                    // 直接计算文件内容哈希值，无需保存后再计算
                    var contentHash = FileManagementHelper.CalculateContentHash(FileStorageInfo.FileData);
                    _logger?.Debug("文件哈希计算完成: {ContentHash}, 文件大小: {FileSize} bytes", contentHash, FileStorageInfo.FileData.Length);

                    // 保存文件
                    await File.WriteAllBytesAsync(filePath, FileStorageInfo.FileData, cancellationToken);
                    _logger?.Debug("文件物理保存成功");

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
                    _logger?.Debug("文件信息保存到数据库成功，FileId: {FileId}", fileStorageInfo.FileId);

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
                            _logger?.Debug("业务关联创建成功: FileId={FileId}, BusinessNo={BusinessNo}, BusinessType={BusinessType}",
                                fileStorageInfo.FileId, uploadRequest.BusinessNo, uploadRequest.BusinessType.Value);
                        }
                    }

                    #endregion
                }
                responseData.Message = $"文件上传成功，共成功上传 {responseData.FileStorageInfos.Count} 个文件";
                _logger?.Debug("文件上传处理完成，成功上传 {SuccessCount} 个文件", responseData.FileStorageInfos.Count);
                responseData.IsSuccess = true;
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
                    var fileInfos = await _fileStorageInfoController.BaseQueryAsync(
                        $"FileId = {downloadRequest.FileStorageInfo.FileId} AND Status = 1");
                    if (fileInfos != null && fileInfos.Count > 0)
                    {
                        var fileInfo = fileInfos[0] as tb_FS_FileStorageInfo;
                        downloadRequest.FileStorageInfo = fileInfo;
                        if (fileInfo != null && !string.IsNullOrEmpty(fileInfo.StoragePath))
                        {
                            filePath = fileInfo.StoragePath;
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
                    _logger?.Debug("尝试在文件系统中查找文件");
                    filePath = await FindPhysicalFileAsync(downloadRequest.FileStorageInfo);
                }

                if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                {
                    _logger?.LogWarning("文件不存在或无法访问");
                    return FileDownloadResponse.CreateFailure("文件不存在或无法访问");
                }
                var fileData = await File.ReadAllBytesAsync(filePath, cancellationToken);
                downloadRequest.FileStorageInfo.FileData = fileData;

                // 创建响应数据
                var response = new FileDownloadResponse
                {
                    FileStorageInfos = new List<tb_FS_FileStorageInfo> { downloadRequest.FileStorageInfo },
                    Message = "文件下载成功",
                    IsSuccess = true
                };
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
            _logger?.Debug("开始执行文件查找策略，FileId: {FileId}", fileInfo?.FileId);
            // 搜索策略：1. 分类目录 2. 根目录

            // 优化搜索模式，提高性能
            var searchPatterns = new List<string>();
            
            // 优先使用精确匹配
            // 从StoragePath中提取文件名（如果有）
            if (!string.IsNullOrEmpty(fileInfo.StoragePath))
            {
                var fileNameFromPath = Path.GetFileName(fileInfo.StoragePath);
                if (!string.IsNullOrEmpty(fileNameFromPath))
                {
                    searchPatterns.Add(fileNameFromPath);
                    _logger?.LogDebug("添加StoragePath中的文件名到搜索模式: {FileName}", fileNameFromPath);
                }
            }
            
            // 其次是存储文件名（如果有）
            if (!string.IsNullOrEmpty(fileInfo.StorageFileName))
            {
                searchPatterns.Add(fileInfo.StorageFileName);
                // 仅在必要时使用通配符
                if (!fileInfo.StorageFileName.Contains('.'))
                {
                    searchPatterns.Add($"{fileInfo.StorageFileName}.*");
                }
            }
            
            // 根据文件ID搜索（作为备用方案）
            searchPatterns.Add($"{fileInfo.FileId}.*");
            
            // 如果有哈希值，也作为搜索条件
            if (!string.IsNullOrEmpty(fileInfo.HashValue))
            {
                searchPatterns.Add($"{fileInfo.HashValue}.*");
            }

            // 搜索目录列表
            var searchDirectories = new List<string>();

            // 首先尝试从StoragePath中提取目录信息（如果有）
            if (!string.IsNullOrEmpty(fileInfo.StoragePath))
            {
                var directoryFromPath = Path.GetDirectoryName(fileInfo.StoragePath);
                if (!string.IsNullOrEmpty(directoryFromPath) && Directory.Exists(directoryFromPath))
                {
                    searchDirectories.Add(directoryFromPath);
                    _logger?.LogDebug("添加StoragePath中的目录到搜索路径: {Directory}", directoryFromPath);
                }
            }

            // 分类目录（包括YYMM子目录）
            if (fileInfo.BusinessType.HasValue)
            {
                var baseBusinessPath = Path.Combine(_fileStoragePath, fileInfo.BusinessType.Value.ToString());
                if (Directory.Exists(baseBusinessPath))
                {
                    searchDirectories.Add(baseBusinessPath);

                    // 获取并添加YYMM子目录（按时间倒序）
                    try
                    {
                        var subDirs = Directory.GetDirectories(baseBusinessPath)
                                             .Where(dir => Directory.Exists(dir))
                                             .OrderByDescending(dir => dir)
                                             .Take(6); // 只搜索最近6个月的目录
                        searchDirectories.AddRange(subDirs);
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogWarning(ex, "获取业务目录子目录失败: {Directory}", baseBusinessPath);
                    }
                }
            }

            // 最后添加根目录作为兜底
            searchDirectories.Add(_fileStoragePath);

            // 去重
            searchDirectories = searchDirectories.Distinct().ToList();

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
                            _logger?.Debug("找到匹配的文件: {FilePath}", files[0]);
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
        /// 删除逻辑：
        /// 1. 如果没有其它业务再引用，根据PhysicalDelete属性决定删除方式
        ///    - PhysicalDelete=true：物理删除（删除文件元数据、关联记录和物理文件）
        ///    - PhysicalDelete=false：逻辑删除（标记文件状态为已删除）
        /// 2. 如果有别的业务引用文件，则只删除关联记录
        /// 所有数据库操作逻辑都在服务器端处理
        /// </summary>
        private async Task<ResponseBase> HandleFileDeleteAsync(FileDeleteRequest deleteRequest, CommandContext executionContext, CancellationToken cancellationToken)
        {
            try
            {
                if (deleteRequest == null || deleteRequest.FileStorageInfos == null || deleteRequest.FileStorageInfos.Count == 0)
                {
                    _logger?.LogWarning("文件删除请求中未包含任何文件信息");
                    return FileDeleteResponse.CreateFailure("文件删除请求中未包含任何文件信息");
                }

                //定义一个要删除的关联列表
                var relationsToDelete = new List<tb_FS_BusinessRelation>();

                //定义一个要删除的文件列表
                var filesToDelete = new List<tb_FS_FileStorageInfo>();

                // 记录删除结果
                var deletedFileIds = new List<string>();
                var deletedCount = 0;
                var relationDeletedCount = 0;

                var response = new FileDeleteResponse
                {
                    IsSuccess = true,
                    DeletedFileIds = deletedFileIds,
                    Message = ""
                };

                // 遍历处理每个文件
                for (int i = 0; i < deleteRequest.FileStorageInfos.Count; i++)
                {
                    long currentFileId = deleteRequest.FileStorageInfos[i].FileId;

                    // 从数据库获取文件信息
                    tb_FS_FileStorageInfo fileStorageInfo = deleteRequest.FileStorageInfos[i];

                    // 检查文件是否被其他业务引用
                    bool isReferencedByOtherBusiness = false;
                    int SelfRelationCount = 0;
                    try
                    {
                        var SelfRelations = await _businessRelationController.QueryByNavAsync(c => c.FileId == currentFileId && c.BusinessNo == deleteRequest.BusinessNo);
                        SelfRelationCount = SelfRelations?.Count ?? 0;
                        relationsToDelete.AddRange(SelfRelations);

                        var OtherRelations = await _businessRelationController.QueryByNavAsync(c => c.FileId == currentFileId && c.BusinessNo != deleteRequest.BusinessNo);
                        int OtherRelationCount = OtherRelations?.Count ?? 0;

                        // 检查文件是否被其他业务引用
                        if (OtherRelationCount > 0)
                        {
                            isReferencedByOtherBusiness = true;
                        }
                        else
                        {
                            filesToDelete.Add(fileStorageInfo);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, "检查文件业务关联失败，FileId: {FileId}", currentFileId);
                    }

                    // 1. 删除业务关联记录（无论是否被其他业务引用，都需要删除当前业务的关联）
                    try
                    {
                        for (int dr = 0; dr < relationsToDelete.Count; dr++)
                        {
                            var relationDeleteResult = await _businessRelationController.BaseDeleteAsync(relationsToDelete[dr]);
                            if (relationDeleteResult)
                            {
                                relationDeletedCount++;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, "删除业务关联记录失败，FileId: {FileId}", currentFileId);
                    }

                    // 2. 根据条件决定删除方式
                    // 根据用户要求和PhysicalDelete属性决定删除方式
                    bool shouldDeleteFileAndMetadata = !isReferencedByOtherBusiness;
                    bool isPhysicalDelete = deleteRequest.PhysicalDelete; // 获取是否物理删除的标识

                    if (shouldDeleteFileAndMetadata)
                    {
                        // 根据PhysicalDelete属性决定是否物理删除文件
                        bool fileDeleted = false;
                        if (isPhysicalDelete)
                        {
                            // 优先使用StoragePath直接删除文件
                            if (!string.IsNullOrEmpty(fileStorageInfo.StoragePath) && File.Exists(fileStorageInfo.StoragePath))
                            {
                                try
                                {
                                    _logger?.LogDebug("直接使用StoragePath删除文件: {FilePath}", fileStorageInfo.StoragePath);
                                    File.Delete(fileStorageInfo.StoragePath);
                                    fileDeleted = true;
                                    _logger?.LogDebug("使用StoragePath删除文件成功: {FilePath}", fileStorageInfo.StoragePath);
                                }
                                catch (Exception ex)
                                {
                                    _logger?.LogError(ex, "使用StoragePath删除文件失败: {FilePath}", fileStorageInfo.StoragePath);
                                    // 即使直接删除失败，也继续尝试搜索删除作为备用方案
                                }
                            }
                            else
                            {
                                _logger?.LogDebug("StoragePath不存在或文件不存在，尝试通过搜索模式查找并删除");
                            }

                            // 如果通过StoragePath删除失败或不存在，则使用搜索模式作为备用方案
                            if (!fileDeleted)
                            {
                                // 优化搜索策略：优先从StoragePath中提取路径信息
                                var searchPaths = new List<string>();

                                // 首先尝试从StoragePath中提取目录信息（最精确）
                                if (!string.IsNullOrEmpty(fileStorageInfo.StoragePath))
                                {
                                    var directoryFromPath = Path.GetDirectoryName(fileStorageInfo.StoragePath);
                                    if (!string.IsNullOrEmpty(directoryFromPath))
                                    {
                                        searchPaths.Add(directoryFromPath);
                                        _logger?.LogDebug("添加StoragePath中的目录到搜索路径: {Directory}", directoryFromPath);
                                    }
                                }

                                // 添加业务类型目录（如果有）
                                if (fileStorageInfo.BusinessType.HasValue)
                                {
                                    // 获取基础业务目录
                                    var baseBusinessPath = Path.Combine(_fileStoragePath, fileStorageInfo.BusinessType.Value.ToString());
                                    searchPaths.Add(baseBusinessPath);

                                    // 如果是较新的文件结构，可能存在于YYMM子目录中
                                    if (Directory.Exists(baseBusinessPath))
                                    {
                                        try
                                        {
                                            // 获取业务目录下的所有YYMM子目录（按时间倒序排列，优先搜索较新的目录）
                                            var subDirs = Directory.GetDirectories(baseBusinessPath)
                                                                 .Where(dir => Directory.Exists(dir))
                                                                 .OrderByDescending(dir => dir)
                                                                 .Take(6); // 只搜索最近6个月的目录
                                            searchPaths.AddRange(subDirs);
                                        }
                                        catch (Exception ex)
                                        {
                                            _logger?.LogWarning(ex, "获取业务目录子目录失败: {Directory}", baseBusinessPath);
                                        }
                                    }
                                }

                                // 最后才添加根目录作为兜底
                                if (!searchPaths.Contains(_fileStoragePath))
                                {
                                    searchPaths.Add(_fileStoragePath);
                                }

                                // 精简搜索模式，提高性能
                                var searchPatterns = new List<string>();

                                // 最精确的搜索模式：完整文件名（如果有）
                                if (!string.IsNullOrEmpty(fileStorageInfo.StoragePath))
                                {
                                    var fileNameFromPath = Path.GetFileName(fileStorageInfo.StoragePath);
                                    if (!string.IsNullOrEmpty(fileNameFromPath))
                                    {
                                        searchPatterns.Add(fileNameFromPath);
                                        _logger?.LogDebug("添加StoragePath中的文件名到搜索模式: {FileName}", fileNameFromPath);
                                    }
                                }

                                // 其次是存储文件名（如果有）
                                if (!string.IsNullOrEmpty(fileStorageInfo.StorageFileName))
                                {
                                    // 优先搜索精确的文件名
                                    searchPatterns.Add(fileStorageInfo.StorageFileName);
                                }

                                // 只在必要时使用通配符搜索
                                if (fileStorageInfo.FileId > 0)
                                {
                                    searchPatterns.Add($"{fileStorageInfo.FileId}.*");
                                }
                                if (!string.IsNullOrEmpty(fileStorageInfo.HashValue))
                                {
                                    searchPatterns.Add($"{fileStorageInfo.HashValue}.*");
                                }

                                // 在所有搜索路径中查找并删除文件，一旦找到并删除就停止搜索
                                foreach (var searchPath in searchPaths)
                                {
                                    if (!Directory.Exists(searchPath))
                                    {
                                        continue; // 静默跳过不存在的目录，减少日志输出
                                    }

                                    foreach (var pattern in searchPatterns)
                                    {
                                        try
                                        {
                                            _logger?.LogDebug("在目录{Directory}中使用模式{Pattern}搜索文件", searchPath, pattern);
                                            var files = Directory.GetFiles(searchPath, pattern);
                                            if (files.Length > 0)
                                            {
                                                _logger?.LogDebug("在目录{Directory}中找到匹配模式{Pattern}的文件", searchPath, pattern);

                                                // 物理删除：删除实际文件
                                                foreach (var filePath in files)
                                                {
                                                    try
                                                    {
                                                        _logger?.LogDebug("物理删除文件: {FilePath}", filePath);
                                                        File.Delete(filePath);
                                                        fileDeleted = true;
                                                        _logger?.LogDebug("物理删除文件成功: {FilePath}", filePath);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        // 记录错误但继续删除其他文件
                                                        _logger?.LogError(ex, "物理删除文件失败: {FilePath}", filePath);
                                                    }
                                                }

                                                // 删除成功后可以提前退出，减少不必要的搜索
                                                if (fileDeleted) break;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            _logger?.LogWarning(ex, "搜索文件时出错: 目录={Directory}, 模式={Pattern}", searchPath, pattern);
                                        }
                                    }

                                    // 如果已找到并删除文件，提前退出目录搜索
                                    if (fileDeleted) break;
                                }
                            }
                        }
                    }

                    // 更新数据库文件状态为已删除（逻辑删除）
                    try
                    {
                        fileStorageInfo.Status = 0; // 标记为删除
                        await _fileStorageInfoController.SaveOrUpdate(fileStorageInfo);
                        _logger?.LogDebug("数据库文件状态更新成功，FileId: {FileId}, 删除类型: {DeleteType}",
                            fileStorageInfo.FileId, isPhysicalDelete ? "物理删除" : "逻辑删除");

                        deletedCount++;
                        deletedFileIds.Add(fileStorageInfo.FileId.ToString());
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, "更新数据库文件状态失败，FileId: {FileId}", fileStorageInfo.FileId);
                    }
                }

                response.DeletedFileIds = deletedFileIds;
                // 设置响应消息
                if (relationDeletedCount > 0)
                {
                    if (deletedCount > 0)
                    {
                        // 根据删除请求中是否包含物理删除，调整响应消息
                        string deleteTypeDesc = deleteRequest.PhysicalDelete ? "物理删除" : "逻辑删除";
                        response.Message = $"文件处理完成，成功{deleteTypeDesc} {deletedCount} 个文件，删除 {relationDeletedCount} 个业务关联";
                    }
                    else
                    {
                        response.Message = $"业务关联删除成功，共删除 {relationDeletedCount} 个关联记录";
                    }
                }
                else
                {
                    response.Message = "未找到或未能删除任何业务关联记录";
                    _logger?.LogWarning("未找到或未能删除任何业务关联记录");
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "文件删除处理失败");
                return FileDeleteResponse.CreateFailure($"文件删除失败: {ex.Message}");
            }
        }


    }
}