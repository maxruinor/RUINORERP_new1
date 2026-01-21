using Azure;
using Microsoft.Extensions.Logging;
using RUINORERP.Business;
using RUINORERP.Business.Config;
using RUINORERP.Global;
using RUINORERP.IServices;
using RUINORERP.Model;
using RUINORERP.Model.ConfigModel;
using RUINORERP.Model.Context;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Common;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.FileManagement;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.Repository.UnitOfWorks;
using RUINORERP.Server.Helpers;
using RUINORERP.Server.Network.Models;
using RUINORERP.Server.Network.Services;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static RUINORERP.Server.Network.Services.FileCleanupService;
using ApplicationContext = RUINORERP.Model.Context.ApplicationContext;

namespace RUINORERP.Server.Network.CommandHandlers
{
    /// <summary>
    /// 文件处理器 - 处理文件上传、下载、删除等业务逻辑
    /// 集成了文件更新服务和清理服务,支持完整的文件生命周期管理
    /// </summary>
    [CommandHandler("FileCommandHandler", priority: 50)]
    public class FileCommandHandler : BaseCommandHandler
    {
        private readonly SessionService _sessionService;
        private readonly ILogger<FileCommandHandler> _logger;
        private readonly string _fileStoragePath;
        private readonly ServerGlobalConfig _serverConfig;
        public readonly IUnitOfWorkManage _unitOfWorkManage;
        // 添加业务控制器用于数据库操作
        private readonly tb_FS_FileStorageInfoController<tb_FS_FileStorageInfo> _fileStorageInfoController;
        private readonly tb_FS_BusinessRelationController<tb_FS_BusinessRelation> _businessRelationController;
        private readonly tb_FS_FileStorageVersionController<tb_FS_FileStorageVersion> _fileStorageVersionController;
        private readonly ApplicationContext _applicationContext;

        // 新增服务:文件更新和清理
        private readonly FileUpdateService _fileUpdateService;
        private readonly FileCleanupService _fileCleanupService;
        private readonly HasAttachmentSyncService _hasAttachmentSyncService;
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
            IUnitOfWorkManage unitOfWorkManage,
            ILogger<FileCommandHandler> logger = null,
            tb_FS_FileStorageInfoController<tb_FS_FileStorageInfo> fileStorageInfoController = null,
            tb_FS_BusinessRelationController<tb_FS_BusinessRelation> businessRelationController = null,
            tb_FS_FileStorageVersionController<tb_FS_FileStorageVersion> fileStorageVersionController = null,
            FileUpdateService fileUpdateService = null,
            FileCleanupService fileCleanupService = null,
            HasAttachmentSyncService hasAttachmentSyncService = null)
        {
            _applicationContext = applicationContext;
            _sessionService = sessionService;
            _logger = logger;
            _unitOfWorkManage = unitOfWorkManage;
            // 统一路径初始化逻辑
            try
            {
                _serverConfig = Startup.GetFromFac<ServerGlobalConfig>();
                _fileStoragePath = ResolveEnvironmentVariables(_serverConfig.FileStoragePath);

                // 确保存储目录存在
                if (!string.IsNullOrEmpty(_fileStoragePath) && !Directory.Exists(_fileStoragePath))
                {
                    Directory.CreateDirectory(_fileStoragePath);
                    _logger.Debug("创建文件存储目录: {FileStoragePath}", _fileStoragePath);
                }

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

            // 注入文件更新和清理服务(通过DI容器)
            _fileUpdateService = fileUpdateService ?? throw new ArgumentNullException(nameof(fileUpdateService));
            _fileCleanupService = fileCleanupService ?? throw new ArgumentNullException(nameof(fileCleanupService));
            _hasAttachmentSyncService = hasAttachmentSyncService;

            // 初始化文件存储路径
            FileStorageHelper.InitializeStoragePath(_serverConfig);

            // 设置支持的命令
            SetSupportedCommands(
                FileCommands.FileUpload,
                FileCommands.FileDownload,
                FileCommands.FileDelete,
                FileCommands.FileInfoQuery,
                FileCommands.FileList,
                FileCommands.FilePermissionCheck,
                FileCommands.FileStorageInfo
            // FileCommands.FileUpdate, // 新增文件更新命令(待实现)
            // FileCommands.FileCleanup, // 新增文件清理命令(待实现)
            // FileCommands.FileVersionHistory // 新增版本历史命令(待实现)
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
                else if (cmd.Packet.Request is FileListRequest listRequest && commandId == FileCommands.FileList)
                {
                    return await HandleFileListAsync(listRequest, cmd.Packet.ExecutionContext, cancellationToken);
                }
                else if (cmd.Packet.Request is FileInfoRequest infoRequest && commandId == FileCommands.FileInfoQuery)
                {
                    return await HandleFileInfoAsync(infoRequest, cmd.Packet.ExecutionContext, cancellationToken);
                }
                else if (cmd.Packet.Request is FilePermissionCheckRequest permissionRequest && commandId == FileCommands.FilePermissionCheck)
                {
                    return await HandleFilePermissionCheckAsync(permissionRequest, cmd.Packet.ExecutionContext, cancellationToken);
                }
                else if (commandId == FileCommands.FileStorageInfo)
                {
                    return await HandleStorageInfoAsync(cmd.Packet.ExecutionContext, cancellationToken);
                }
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
        /// 集成事务处理，确保文件保存、业务关联、HasAttachment同步的原子性
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

                // 开启事务
                _unitOfWorkManage.BeginTran();
                try
                {
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

                        // 将绝对路径转换为相对路径保存到数据库
                        var relativeStoragePath = FileStorageHelper.ConvertToRelativePath(filePath);

                        // 创建文件信息实体并保存到数据库
                        var fileStorageInfo = FileManagementHelper.CreateFileStorageInfo(
                            FileStorageInfo.OriginalFileName,  // fileName
                            FileStorageInfo.FileData.Length,   // fileSize
                            fileExtension.TrimStart('.'),     // fileType
                            relativeStoragePath,              // storagePath（保存相对路径）
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
                                BusinessId = uploadRequest.BusinessId.Value, // 新增:业务主键ID
                                FileId = fileStorageInfo.FileId,
                                IsMainFile = (i == 0), // 第一个文件为主文件
                                RelatedField = uploadRequest.RelatedField ?? "MainFile", // 设置关联字段，必填项
                                IsDetailTable = uploadRequest.IsDetailTable, // 新增:是否明细表
                                DetailId = uploadRequest.DetailId, // 新增:明细表主键
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
                                _logger?.Debug("业务关联创建成功: FileId={FileId}, BusinessNo={BusinessNo}, BusinessId={BusinessId}, BusinessType={BusinessType}, RelatedField={RelatedField}",
                                    fileStorageInfo.FileId, uploadRequest.BusinessNo, uploadRequest.BusinessId, uploadRequest.BusinessType.Value, uploadRequest.RelatedField);

                                // 同步HasAttachment标志（在事务内执行）
                                if (_hasAttachmentSyncService != null && uploadRequest.BusinessType.HasValue && uploadRequest.BusinessId.HasValue)
                                {
                                    await _hasAttachmentSyncService.SyncOnFileUploadAsync(
                                        uploadRequest.BusinessType.Value,
                                        uploadRequest.BusinessId.Value,
                                        uploadRequest.BusinessNo,
                                        cancellationToken,
                                        useTransaction: false); // 已经在外部事务中，不需要开启新事务
                                }
                            }
                        }

                        #endregion
                    }

                    // 提交事务
                    _unitOfWorkManage.CommitTran();

                    responseData.Message = $"文件上传成功，共成功上传 {responseData.FileStorageInfos.Count} 个文件";
                    _logger?.Debug("文件上传处理完成，成功上传 {SuccessCount} 个文件", responseData.FileStorageInfos.Count);
                    responseData.IsSuccess = true;
                    return responseData;
                }
                catch (Exception ex)
                {
                    // 回滚事务
                    _unitOfWorkManage.RollbackTran();
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "文件上传处理失败");
                return ResponseFactory.CreateSpecificErrorResponse<FileUploadResponse>($"文件上传失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 处理文件下载
        /// 方式1: 按文件ID下载
        /// 方式2: 按业务信息下载 (BusinessId/BusinessNo + RelatedField)
        /// 方式3: 下载单据所有图片 (DownloadAllImages=true)
        /// </summary>
        private async Task<FileDownloadResponse> HandleFileDownloadAsync(FileDownloadRequest downloadRequest, CommandContext executionContext, CancellationToken cancellationToken)
        {
            try
            {
                if (downloadRequest == null || !downloadRequest.IsValid())
                {
                    _logger?.LogWarning("文件下载请求格式错误或参数不完整");
                    return FileDownloadResponse.CreateFailure("文件下载请求格式错误或参数不完整");
                }

                List<tb_FS_FileStorageInfo> fileList = new List<tb_FS_FileStorageInfo>();

                // 方式1: 按文件ID下载
                if (downloadRequest.FileStorageInfo != null && downloadRequest.FileStorageInfo.FileId > 0)
                {
                    var fileInfo = await DownloadByFileIdAsync(downloadRequest.FileStorageInfo.FileId, cancellationToken);
                    if (fileInfo != null)
                    {
                        fileList.Add(fileInfo);
                    }
                }
                // 方式2/3: 按业务信息下载
                else if (downloadRequest.BusinessType.HasValue)
                {
                    if (downloadRequest.DownloadAllImages)
                    {
                        // 方式3: 下载单据所有关联图片
                        fileList = await DownloadAllImagesForBusinessAsync(
                            downloadRequest.BusinessType.Value,
                            downloadRequest.BusinessNo,
                            downloadRequest.BusinessId,
                            downloadRequest.IsDetailTable,
                            downloadRequest.DetailId,
                            cancellationToken);
                    }
                    else
                    {
                        // 方式2: 按业务信息+字段下载单个图片
                        var fileInfo = await DownloadByFieldAsync(
                            downloadRequest.BusinessType.Value,
                            downloadRequest.BusinessNo,
                            downloadRequest.BusinessId,
                            downloadRequest.RelatedField,
                            downloadRequest.IsDetailTable,
                            downloadRequest.DetailId,
                            cancellationToken);
                        if (fileInfo != null)
                        {
                            fileList.Add(fileInfo);
                        }
                    }
                }

                if (fileList == null || fileList.Count == 0)
                {
                    _logger?.LogWarning("未找到符合条件的文件");
                    return FileDownloadResponse.CreateFailure("未找到符合条件的文件");
                }

                // 加载文件数据
                foreach (var fileInfo in fileList)
                {
                    string filePath = FileStorageHelper.ResolveToAbsolutePath(fileInfo.StoragePath);
                    if (!File.Exists(filePath))
                    {
                        _logger?.LogWarning("文件不存在: {FilePath}", filePath);
                        return FileDownloadResponse.CreateFailure($"文件 {fileInfo.OriginalFileName} 不存在或无法访问");
                    }

                    fileInfo.FileData = await File.ReadAllBytesAsync(filePath, cancellationToken);
                }

                // 创建响应数据
                var response = new FileDownloadResponse
                {
                    FileStorageInfos = fileList,
                    Message = fileList.Count == 1 ? "文件下载成功" : $"成功下载 {fileList.Count} 个文件",
                    IsSuccess = true
                };

                _logger?.Debug("文件下载成功,共 {Count} 个文件", fileList.Count);
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
        /// 按文件ID下载文件
        /// </summary>
        private async Task<tb_FS_FileStorageInfo> DownloadByFileIdAsync(long fileId, CancellationToken cancellationToken)
        {
            var fileInfos = await _fileStorageInfoController.QueryByNavAsync(c => c.FileStatus == (int)FileStatus.Active && c.FileId == fileId && c.isdeleted == false); 
            if (fileInfos != null && fileInfos.Count > 0)
            {
                return fileInfos[0] as tb_FS_FileStorageInfo;
            }
            return null;
        }

        /// <summary>
        /// 按业务信息+字段下载单个图片
        /// </summary>
        private async Task<tb_FS_FileStorageInfo> DownloadByFieldAsync(
            int businessType,
            string businessNo,
            long? businessId,
            string relatedField,
            bool isDetailTable,
            long? detailId,
            CancellationToken cancellationToken)
        {
            var db = _unitOfWorkManage.GetDbClient();

            // 构建查询条件
            var query = db.Queryable<tb_FS_BusinessRelation>()
                .Where(c => c.BusinessType == businessType)
                .Where(c => c.IsActive == true)
                .Where(c => c.RelatedField == relatedField);

            // 优先使用 BusinessId 查询（性能更好）
            if (businessId.HasValue)
            {
                query = query.Where(c => c.BusinessId == businessId.Value);
            }
            else if (!string.IsNullOrEmpty(businessNo))
            {
                query = query.Where(c => c.BusinessNo == businessNo);
            }

            // 明细表相关条件
            if (isDetailTable)
            {
                query = query.Where(c => c.IsDetailTable == true);
                if (detailId.HasValue)
                {
                    query = query.Where(c => c.DetailId == detailId.Value);
                }
            }
            else
            {
                query = query.Where(c => c.IsDetailTable == false);
            }

            var relations = await query
                .Includes(t => t.tb_fs_filestorageinfo)
                .ToListAsync();

            if (relations != null && relations.Count > 0)
            {
                var relation = relations[0] as tb_FS_BusinessRelation;
                if (relation != null)
                {
                    var fileInfos = await _fileStorageInfoController.QueryByNavAsync(c => c.FileStatus == (int)FileStatus.Active && c.FileId == relation.FileId && c.isdeleted == false); 
             
                    if (fileInfos != null && fileInfos.Count > 0)
                    {
                        return fileInfos[0] as tb_FS_FileStorageInfo;
                    }
                }
            }

            _logger?.LogWarning("未找到文件, 业务类型: {BusinessType}, 字段: {RelatedField}", businessType, relatedField);
            return null;
        }

        /// <summary>
        /// 下载单据所有关联图片
        /// </summary>
        private async Task<List<tb_FS_FileStorageInfo>> DownloadAllImagesForBusinessAsync(
            int businessType,
            string businessNo,
            long? businessId,
            bool isDetailTable,
            long? detailId,
            CancellationToken cancellationToken)
        {
            var fileInfos = new List<tb_FS_FileStorageInfo>();
            var db = _unitOfWorkManage.GetDbClient();

            // 构建查询条件
            var query = db.Queryable<tb_FS_BusinessRelation>()
                .Where(c => c.BusinessType == businessType)
                .Where(c => c.IsActive == true);

            // 优先使用 BusinessId 查询
            if (businessId.HasValue)
            {
                query = query.Where(c => c.BusinessId == businessId.Value);
            }
            else if (!string.IsNullOrEmpty(businessNo))
            {
                query = query.Where(c => c.BusinessNo == businessNo);
            }

            // 明细表相关条件
            if (isDetailTable)
            {
                query = query.Where(c => c.IsDetailTable == true);
                if (detailId.HasValue)
                {
                    query = query.Where(c => c.DetailId == detailId.Value);
                }
            }
            else
            {
                query = query.Where(c => c.IsDetailTable == false);
            }

            var relations = await query
                .Includes(t => t.tb_fs_filestorageinfo)
                .ToListAsync();

            if (relations != null && relations.Count > 0)
            {
                foreach (var relation in relations)
                {
                    var bizRelation = relation as tb_FS_BusinessRelation;
                    if (bizRelation != null)
                    {
                        var files = await _fileStorageInfoController.QueryByNavAsync(c => c.FileStatus == (int)FileStatus.Active && c.FileId == bizRelation.FileId && c.isdeleted == false); 
                        
            
                        if (files != null && files.Count > 0)
                        {
                            fileInfos.Add(files[0] as tb_FS_FileStorageInfo);
                        }
                    }
                }
            }

            return fileInfos;
        }

        /// <summary>
        /// 根据文件信息查找物理文件
        /// 优化的搜索策略：先精确匹配，后通配符匹配；优先搜索最可能存在的目录
        /// </summary>
        /// <param name="fileInfo">文件信息</param>
        /// <returns>找到的文件路径，找不到返回null</returns>
        private async Task<string> FindPhysicalFileAsync(tb_FS_FileStorageInfo fileInfo)
        {
            _logger?.Debug("开始执行文件查找策略，FileId: {FileId}", fileInfo?.FileId);

            // 优化搜索模式 - 分为精确匹配和通配符匹配两组
            var exactMatchPatterns = new List<string>();
            var wildcardPatterns = new List<string>();

            // 1. 优先使用精确匹配模式
            // 从StoragePath直接尝试（精确路径）
            if (!string.IsNullOrEmpty(fileInfo.StoragePath))
            {
                // 首先尝试将相对路径解析为绝对路径
                var resolvedPath = FileStorageHelper.ResolveToAbsolutePath(fileInfo.StoragePath);
                if (File.Exists(resolvedPath))
                {
                    _logger?.Debug("解析后的路径存在，返回完整路径: {Path}", resolvedPath);
                    return resolvedPath;
                }

                // 如果解析后的路径不存在，尝试原路径（可能是绝对路径）
                if (File.Exists(fileInfo.StoragePath))
                {
                    _logger?.Debug("StoragePath直接存在，返回完整路径: {Path}", fileInfo.StoragePath);
                    return fileInfo.StoragePath;
                }
            }

            // 从StoragePath中提取文件名（精确匹配）
            if (!string.IsNullOrEmpty(fileInfo.StoragePath))
            {
                var fileNameFromPath = Path.GetFileName(fileInfo.StoragePath);
                if (!string.IsNullOrEmpty(fileNameFromPath))
                {
                    exactMatchPatterns.Add(fileNameFromPath);
                    _logger?.LogDebug("添加StoragePath中的文件名到精确匹配模式: {FileName}", fileNameFromPath);
                }
            }

            // 存储文件名（精确匹配）
            if (!string.IsNullOrEmpty(fileInfo.StorageFileName))
            {
                exactMatchPatterns.Add(fileInfo.StorageFileName);

                // 仅在必要时使用通配符
                if (!fileInfo.StorageFileName.Contains('.'))
                {
                    wildcardPatterns.Add($"{fileInfo.StorageFileName}.*");
                }
            }

            // 2. 备用通配符模式
            // 根据文件ID搜索
            wildcardPatterns.Add($"{fileInfo.FileId}.*");

            // 如果有哈希值，也作为搜索条件
            if (!string.IsNullOrEmpty(fileInfo.HashValue))
            {
                wildcardPatterns.Add($"{fileInfo.HashValue}.*");
            }

            // 搜索目录列表 - 按照优先级排序
            var searchDirectories = new List<string>();

            // 1. 首先尝试从StoragePath中提取目录信息（最可能的位置）
            if (!string.IsNullOrEmpty(fileInfo.StoragePath))
            {
                var directoryFromPath = Path.GetDirectoryName(fileInfo.StoragePath);
                if (!string.IsNullOrEmpty(directoryFromPath) && Directory.Exists(directoryFromPath))
                {
                    searchDirectories.Add(directoryFromPath);
                    _logger?.LogDebug("添加StoragePath中的目录到搜索路径: {Directory}", directoryFromPath);
                }
            }

            // 2. 业务类型目录 - 减少子目录搜索数量，只搜索最近3个月
            if (fileInfo.BusinessType.HasValue)
            {
                var baseBusinessPath = Path.Combine(_fileStoragePath, fileInfo.BusinessType.Value.ToString());
                if (Directory.Exists(baseBusinessPath))
                {
                    searchDirectories.Add(baseBusinessPath);

                    // 获取并添加YYMM子目录（按时间倒序，减少数量）
                    try
                    {
                        var subDirs = Directory.GetDirectories(baseBusinessPath)
                                             .Where(dir => Directory.Exists(dir))
                                             .OrderByDescending(dir => dir)
                                             .Take(3); // 只搜索最近3个月的目录
                        searchDirectories.AddRange(subDirs);
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogWarning(ex, "获取业务目录子目录失败: {Directory}", baseBusinessPath);
                    }
                }
            }

            // 3. 最后添加根目录作为兜底
            if (!searchDirectories.Contains(_fileStoragePath))
            {
                searchDirectories.Add(_fileStoragePath);
            }

            // 去重
            searchDirectories = searchDirectories.Distinct().ToList();

            // 优化搜索策略：先使用精确模式在所有目录搜索，再使用通配符模式
            // 这样可以在找到精确匹配时立即返回，减少不必要的通配符搜索
            var allPatterns = new List<List<string>> { exactMatchPatterns, wildcardPatterns };

            foreach (var patternGroup in allPatterns)
            {
                if (patternGroup.Count == 0) continue;

                foreach (var directory in searchDirectories)
                {
                    if (!Directory.Exists(directory)) continue;

                    _logger?.LogDebug("在目录中搜索: {Directory}", directory);
                    foreach (var pattern in patternGroup)
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
        /// 集成事务处理，确保删除操作的原子性
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

                // 开启事务
                _unitOfWorkManage.BeginTran();
                try
                {
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
                            if (!string.IsNullOrEmpty(fileStorageInfo.StoragePath))
                            {
                                // 首先尝试将相对路径解析为绝对路径
                                var resolvedPath = FileStorageHelper.ResolveToAbsolutePath(fileStorageInfo.StoragePath);
                                if (File.Exists(resolvedPath))
                                {
                                    try
                                    {
                                        _logger?.LogDebug("使用解析后的路径删除文件: {FilePath}", resolvedPath);
                                        File.Delete(resolvedPath);
                                        fileDeleted = true;
                                        _logger?.LogDebug("使用解析后的路径删除文件成功: {FilePath}", resolvedPath);
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger?.LogError(ex, "使用解析后的路径删除文件失败: {FilePath}", resolvedPath);
                                    }
                                }

                                // 如果解析后的路径不存在，尝试原路径（可能是绝对路径）
                                if (!fileDeleted && File.Exists(fileStorageInfo.StoragePath))
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
                            }

                            if (!fileDeleted)
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
                        fileStorageInfo.FileStatus = (int)FileStatus.Deleted; // 标记为删除
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

                    // 同步HasAttachment标志（在删除关联后，事务内执行）
                    // 从关联记录中获取BusinessType和BusinessId（用于单据级别的HasAttachment同步）
                    if (relationDeletedCount > 0 && _hasAttachmentSyncService != null && relationsToDelete.Count > 0)
                    {
                        // 获取第一个关联记录的BusinessType和BusinessId
                        var firstRelation = relationsToDelete.FirstOrDefault();
                        if (firstRelation != null)
                        {
                            await _hasAttachmentSyncService.SyncOnFileDeleteAsync(
                                firstRelation.BusinessType,
                                firstRelation.BusinessId,
                                cancellationToken,
                                useTransaction: false); // 已经在外部事务中，不需要开启新事务
                        }
                    }

                    // 提交事务
                    _unitOfWorkManage.CommitTran();

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
                    // 回滚事务
                    _unitOfWorkManage.RollbackTran();
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "文件删除处理失败");
                return FileDeleteResponse.CreateFailure($"文件删除失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 处理文件列表查询
        /// </summary>
        private async Task<IResponse> HandleFileListAsync(FileListRequest listRequest, CommandContext executionContext, CancellationToken cancellationToken)
        {
            try
            {
                _logger?.Debug("开始处理文件列表查询请求");

                var response = new FileListResponse();
                var fileInfos = new List<tb_FS_FileStorageInfo>();

                // 如果指定了业务编号,查询业务关联的文件
                if (!string.IsNullOrEmpty(listRequest.BusinessId))
                {
                    // 优化后: 使用 INNER JOIN 一次查询完成,避免 N+1 查询问题
                    var dbClient = _unitOfWorkManage.GetDbClient();
                    var fileQueryResult = await dbClient
                        .Queryable<tb_FS_BusinessRelation>()
                        .InnerJoin<tb_FS_FileStorageInfo>((br, fs) => br.FileId == fs.FileId)
                        .Where((br, fs) => br.BusinessNo == listRequest.BusinessId
                                          && br.IsActive
                                          && fs.FileStatus == (int)FileStatus.Active
                                          && fs.isdeleted == false)
                        .OrderBy((br, fs) => br.Created_at, OrderByType.Desc)
                        .Select((br, fs) => new { br, fs })
                        .ToListAsync();

                    fileInfos = fileQueryResult.Select(x => x.fs).ToList();
                }
                // 否则按分类查询所有文件
                else if (listRequest.BusinessType.HasValue)
                {
                    var files = await _fileStorageInfoController.QueryByNavAsync(c => c.FileStatus == (int)FileStatus.Active && c.BusinessType == listRequest.BusinessType);
                     
                    if (files != null)
                    {
                        fileInfos.AddRange(files.Cast<tb_FS_FileStorageInfo>());
                    }
                }
                else
                {
                    _logger?.LogWarning("文件列表请求中未指定业务编号或分类");
                    return FileListResponse.CreateFailure("必须指定业务编号或分类");
                }

                // 应用分页
                int totalCount = fileInfos.Count;
                if (listRequest.PageSize > 0)
                {
                    fileInfos = fileInfos
                        .Skip((listRequest.PageIndex - 1) * listRequest.PageSize)
                        .Take(listRequest.PageSize)
                        .ToList();
                }

                // 创建响应数据
                var responseData = new FileListResponseData
                {
                    FileStorageInfos = fileInfos,
                    TotalCount = totalCount,
                    PageIndex = listRequest.PageIndex,
                    PageSize = listRequest.PageSize,
                    TotalPages = listRequest.PageSize > 0 ? (int)Math.Ceiling((double)totalCount / listRequest.PageSize) : 1
                };

                response.IsSuccess = true;
                response.Data = responseData;
                response.Message = $"找到 {totalCount} 个文件";
                _logger?.Debug("文件列表查询成功,共找到 {totalCount} 个文件");
                return response;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "文件列表查询处理失败");
                return FileListResponse.CreateFailure($"文件列表查询失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 处理文件信息查询
        /// </summary>
        private async Task<IResponse> HandleFileInfoAsync(FileInfoRequest infoRequest, CommandContext executionContext, CancellationToken cancellationToken)
        {
            try
            {
                _logger?.Debug("开始处理文件信息查询请求");

                var fileInfos = new List<tb_FS_FileStorageInfo>();

                // 多文件查询
                if (infoRequest.IsMultiFile && infoRequest.FileStorageInfos != null && infoRequest.FileStorageInfos.Count > 0)
                {
                    foreach (var fileInfo in infoRequest.FileStorageInfos)
                    {
                        if (fileInfo.FileId > 0)
                        {
                            var files = await _fileStorageInfoController.QueryByNavAsync(c => c.FileStatus == (int)FileStatus.Active && c.FileId == fileInfo.FileId);


                            if (files != null && files.Count > 0)
                            {
                                var storedFileInfo = files[0] as tb_FS_FileStorageInfo;
                                if (storedFileInfo != null)
                                {
                                    fileInfos.Add(storedFileInfo);
                                }
                            }
                        }
                    }
                }
                // 单文件查询
                else if (infoRequest.FileStorageInfo != null && infoRequest.FileStorageInfo.FileId > 0)
                {
                    var files = await _fileStorageInfoController.QueryByNavAsync(c => c.FileStatus == (int)FileStatus.Active && c.FileId == infoRequest.FileStorageInfo.FileId);

                    if (files != null && files.Count > 0)
                    {
                        var storedFileInfo = files[0] as tb_FS_FileStorageInfo;
                        if (storedFileInfo != null)
                        {
                            fileInfos.Add(storedFileInfo);
                        }
                    }
                }
                else
                {
                    _logger?.LogWarning("文件信息请求中未指定文件ID");
                    return FileInfoResponse.CreateFailure("必须指定文件ID");
                }

                var response = new FileInfoResponse
                {
                    IsSuccess = true,
                    FileStorageInfos = fileInfos,
                    Message = $"成功获取 {fileInfos.Count} 个文件信息"
                };
                _logger?.Debug("文件信息查询成功,共 {fileInfos.Count} 个文件");
                return response;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "文件信息查询处理失败");
                return FileInfoResponse.CreateFailure($"文件信息查询失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 处理文件权限检查
        /// </summary>
        private async Task<IResponse> HandleFilePermissionCheckAsync(FilePermissionCheckRequest permissionRequest, CommandContext executionContext, CancellationToken cancellationToken)
        {
            try
            {
                _logger?.Debug("开始处理文件权限检查请求");

                if (permissionRequest.FileId <= 0)
                {
                    _logger?.LogWarning("权限检查请求中未指定文件ID");
                    return FilePermissionCheckResponse.CreateFailure("必须指定文件ID");
                }

                // 查询文件信息
                var files = await _fileStorageInfoController.QueryByNavAsync(c => c.FileStatus == (int)FileStatus.Active && c.FileId == permissionRequest.FileId);
                if (files == null || files.Count == 0)
                {
                    _logger?.LogWarning("文件不存在或已删除,FileId: {FileId}", permissionRequest.FileId);
                    return FilePermissionCheckResponse.CreateFailure("文件不存在或已删除");
                }

                var fileInfo = files[0] as tb_FS_FileStorageInfo;

                // 根据检查类型进行验证
                bool hasPermission = false;
                switch (permissionRequest.PermissionType?.ToLower())
                {
                    case "read":
                    case "download":
                        // 所有有效状态的文件都可以读取/下载
                        hasPermission = fileInfo.FileStatus == (int)FileStatus.Active;
                        break;

                    case "write":
                    case "update":
                        // 检查文件是否属于当前用户
                        hasPermission = fileInfo.FileStatus == (int)FileStatus.Active &&
                            (fileInfo.Created_by == executionContext.UserId ||
                             fileInfo.Modified_by == executionContext.UserId);
                        break;

                    case "delete":
                        // 删除权限检查:文件属于当前用户或文件未被其他业务引用
                        hasPermission = fileInfo.FileStatus == (int)FileStatus.Active;
                        if (hasPermission && fileInfo.Created_by != executionContext.UserId)
                        {
                            // 检查是否被其他业务引用
                            var relations = await _businessRelationController.QueryByNavAsync(
                                c => c.FileId == permissionRequest.FileId);
                            hasPermission = relations == null || relations.Count <= 1;
                        }
                        break;

                    default:
                        _logger?.LogWarning("未知的权限检查类型: {PermissionType}", permissionRequest.PermissionType);
                        return FilePermissionCheckResponse.CreateFailure("未知的权限检查类型");
                }

                var response = new FilePermissionCheckResponse
                {
                    IsSuccess = true,
                    HasPermission = hasPermission,
                    Message = hasPermission ? "权限验证通过" : "权限验证失败"
                };
                _logger?.Debug("文件权限检查完成,结果: {HasPermission}", hasPermission);
                return response;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "文件权限检查处理失败");
                return FilePermissionCheckResponse.CreateFailure($"文件权限检查失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 处理存储信息查询
        /// </summary>
        private async Task<IResponse> HandleStorageInfoAsync(CommandContext executionContext, CancellationToken cancellationToken)
        {
            try
            {
                _logger?.Debug("开始处理存储信息查询请求");

                var usageData = new StorageUsageInfoData
                {
                    TotalSize = 0,
                    TotalFileCount = 0,
                    CategoryUsage = new Dictionary<string, CategoryUsage>()
                };

                if (!Directory.Exists(_fileStoragePath))
                {
                    _logger?.LogWarning("文件存储路径不存在: {FileStoragePath}", _fileStoragePath);
                    return StorageUsageInfo.CreateSuccess(0, 0, usageData.CategoryUsage, "文件存储路径不存在");
                }

                // 从数据库统计各业务类型的文件使用情况
                try
                {
                    var allFiles = await _fileStorageInfoController.QueryByNavAsync(c => c.FileStatus == (int)FileStatus.Active); ;
                    if (allFiles != null && allFiles.Count > 0)
                    {
                        var fileGroups = allFiles.Cast<tb_FS_FileStorageInfo>()
                            .GroupBy(f => f.BusinessType?.ToString() ?? "Unknown");

                        foreach (var group in fileGroups)
                        {
                            var categoryUsage = new CategoryUsage
                            {
                                FileCount = group.Count(),
                                TotalSize = group.Sum(f => f.FileSize)
                            };
                            usageData.CategoryUsage[group.Key] = categoryUsage;
                            usageData.TotalSize += categoryUsage.TotalSize;
                            usageData.TotalFileCount += categoryUsage.FileCount;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "从数据库统计文件使用信息失败");
                }

                var response = StorageUsageInfo.CreateSuccess(
                    usageData.TotalSize,
                    usageData.TotalFileCount,
                    usageData.CategoryUsage,
                    "获取存储使用信息成功"
                );

                _logger?.Debug("存储信息查询成功,总大小: {TotalSize}, 总文件数: {TotalFileCount}",
                    usageData.TotalSize, usageData.TotalFileCount);

                return response;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "存储信息查询处理失败");
                return StorageUsageInfo.CreateFailure($"存储信息查询失败: {ex.Message}");
            }
        }


    }
}