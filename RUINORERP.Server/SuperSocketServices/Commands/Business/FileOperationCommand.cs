using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Enums;
using RUINORERP.PacketSpec.Protocol;
using RUINORERP.Server.SuperSocketServices;
using RUINORERP.PacketSpec.Models.Requests;
using System.Collections.Generic;

namespace RUINORERP.PacketSpec.Services
{
    /// <summary>
    /// 文件操作命令处理器
    /// </summary>
    public class FileOperationCommand : BaseServerCommand
    {
        private readonly ILogger<FileOperationCommand> _logger;
        private readonly IFileStorageService _fileStorageService;

        public FileOperationType FileOperationType { get; set; }
        //public CmdOperation OperationType { get; set; } = CmdOperation.Receive;

        public string FileName { get; set; }
        public string FilePath { get; set; }
        public byte[] FileData { get; set; }
        public long FileSize { get; set; }
        public string FileHash { get; set; }

        // ICommand 接口属性
        //public string CommandId { get; set; } = Guid.NewGuid().ToString();

        //public OriginalData? DataPacket { get; set; }
        //public SessionInfo SessionInfo { get; set; }

        // IServerCommand 接口属性
        //public int Priority { get; set; } = 2;
        //public override string Description => $"文件操作: {FileOperationType}";
        //public bool RequiresAuthentication => true;
        //public int TimeoutMs { get; set; } = 60000; // 文件操作可能需要更长时间

        public FileOperationCommand(
            ILogger<FileOperationCommand> logger,
            IFileStorageService fileStorageService) : base(CmdOperation.Receive)
        {
            _logger = logger;
            _fileStorageService = fileStorageService;
        }

        public override bool CanExecute()
        {
            return SessionInfo?.IsAuthenticated == true &&
                   !string.IsNullOrEmpty(FileName) &&
                   FileOperationType != FileOperationType.Unknown;
        }

        public override async Task<CommandResult> ExecuteAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation($"开始处理文件操作: Type={FileOperationType}, File={FileName}, SessionId={SessionInfo?.SessionId}");

                // 验证参数
                var validationResult = ValidateParameters();
                if (!validationResult.IsValid)
                {
                    return CommandResult.CreateError(validationResult.ErrorMessage);
                }

                // 根据操作类型执行相应操作
                var result = FileOperationType switch
                {
                    FileOperationType.Upload => await HandleFileUploadAsync(),
                    FileOperationType.Download => await HandleFileDownloadAsync(),
                    FileOperationType.Delete => await HandleFileDeleteAsync(),
                    FileOperationType.List => await HandleFileListAsync(),
                    FileOperationType.GetInfo => await HandleGetFileInfoAsync(),
                    _ => CommandResult.CreateError("不支持的文件操作类型")
                };

                if (result.Success)
                {
                    _logger.LogInformation($"文件操作成功: Type={FileOperationType}, File={FileName}");
                }
                else
                {
                    _logger.LogWarning($"文件操作失败: Type={FileOperationType}, File={FileName}, Error={result.Message}");
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"文件操作异常: Type={FileOperationType}, File={FileName}");
                return CommandResult.CreateError("文件操作异常");
            }
        }

        public override RUINORERP.PacketSpec.Commands.ValidationResult ValidateParameters()
        {
            if (SessionInfo?.IsAuthenticated != true)
                return RUINORERP.PacketSpec.Commands.ValidationResult.Failure("用户未认证");

            if (string.IsNullOrWhiteSpace(FileName))
                return RUINORERP.PacketSpec.Commands.ValidationResult.Failure("文件名不能为空");

            // 验证文件名安全性
            if (ContainsInvalidPathChars(FileName))
                return RUINORERP.PacketSpec.Commands.ValidationResult.Failure("文件名包含非法字符");

            // 根据操作类型进行特定验证
            switch (FileOperationType)
            {
                case FileOperationType.Upload:
                    if (FileData == null || FileData.Length == 0)
                        return RUINORERP.PacketSpec.Commands.ValidationResult.Failure("上传文件数据不能为空");

                    if (FileSize > 100 * 1024 * 1024) // 100MB 限制
                        return RUINORERP.PacketSpec.Commands.ValidationResult.Failure("文件大小超过限制");
                    break;

                case FileOperationType.Download:
                case FileOperationType.Delete:
                case FileOperationType.GetInfo:
                    if (string.IsNullOrWhiteSpace(FilePath))
                        return RUINORERP.PacketSpec.Commands.ValidationResult.Failure("文件路径不能为空");
                    break;
            }

            return RUINORERP.PacketSpec.Commands.ValidationResult.Success();
        }

        public override string GetCommandId()
        {
            return $"{GetType().Name}_{Guid.NewGuid():N}";
        }

        /// <summary>
        /// 解析数据包
        /// </summary>
        public override bool AnalyzeDataPacket(OriginalData data, SessionInfo sessionInfo)
        {
            try
            {
                SessionInfo = sessionInfo;
                DataPacket = data;

                // 这里应该根据实际协议解析文件操作数据
                // 为了简化，这里使用示例数据
                FileName = "demo_file.txt";
                FilePath = "/uploads/demo_file.txt";
                FileOperationType = FileOperationType.Upload;

                return true;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "解析文件操作数据包失败");
                return false;
            }
        }

        /// <summary>
        /// 构建数据包
        /// </summary>
        public override void BuildDataPacket(object request = null)
        {
            try
            {
                // 这里应该根据文件操作响应构建数据包
                if (request is FileOperationResponse response)
                {
                    var responseBytes = System.Text.Encoding.UTF8.GetBytes(
                        Newtonsoft.Json.JsonConvert.SerializeObject(response));
                    DataPacket = new OriginalData(0x02, responseBytes, null);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "构建文件操作响应数据包失败");
            }
        }

        /// <summary>
        /// 处理文件上传
        /// </summary>
        private async Task<CommandResult> HandleFileUploadAsync()
        {
            try
            {
                // 验证文件哈希值
                if (!string.IsNullOrEmpty(FileHash))
                {
                    var calculatedHash = CalculateFileHash(FileData);
                    if (calculatedHash != FileHash)
                    {
                        return CommandResult.CreateError("文件完整性验证失败");
                    }
                }

                // 检查文件是否已存在
                var fileInfoRequest = new FileInfoRequest { FileId = FileName };
                var existingFile = await _fileStorageService.GetFileInfoAsync(fileInfoRequest);
                if (existingFile != null && existingFile.Success)
                {
                    return CommandResult.CreateError("文件已存在");
                }

                // 执行文件上传
                var uploadRequest = new FileUploadRequest
                {
                    FileName = FileName,
                    Data = FileData,
                    FileSize = FileSize
                };
                var uploadResult = await _fileStorageService.UploadFileAsync(uploadRequest);
                if (uploadResult.Success)
                {
                    var response = new FileOperationResponse
                    {
                        Success = true,
                        FileName = FileName,
                        FilePath = uploadResult.FileId, // 使用FileId作为FilePath
                        FileSize = FileData.Length,
                        Message = "文件上传成功"
                    };

                    return CommandResult.CreateSuccess(response);
                }
                else
                {
                    return CommandResult.CreateError($"文件上传失败: {uploadResult.Message}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"文件上传处理异常: {FileName}");
                return CommandResult.CreateError("文件上传处理异常");
            }
        }

        /// <summary>
        /// 处理文件下载
        /// </summary>
        private async Task<CommandResult> HandleFileDownloadAsync()
        {
            try
            {
                // 检查文件是否存在
                var fileInfoRequest = new FileInfoRequest { FileId = FilePath };
                var fileInfo = await _fileStorageService.GetFileInfoAsync(fileInfoRequest);
                if (fileInfo == null || !fileInfo.Success)
                {
                    return CommandResult.CreateError("文件不存在");
                }

                // 检查用户权限
                var hasPermission = await _fileStorageService.CheckDownloadPermissionAsync(FilePath, SessionInfo.UserId?.ToString());
                if (!hasPermission)
                {
                    return CommandResult.CreateError("没有下载权限");
                }

                // 读取文件数据
                var fileData = await _fileStorageService.ReadFileAsync(FilePath);
                if (fileData == null)
                {
                    return CommandResult.CreateError("读取文件失败");
                }

                var response = new FileOperationResponse
                {
                    Success = true,
                    FileName = fileInfo.FileInfo?.OriginalName ?? FileName,
                    FilePath = FilePath,
                    FileSize = fileData.Length,
                    FileData = fileData,
                    FileHash = CalculateFileHash(fileData),
                    Message = "文件下载成功"
                };

                return CommandResult.CreateSuccess(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"文件下载处理异常: {FilePath}");
                return CommandResult.CreateError("文件下载处理异常");
            }
        }

        /// <summary>
        /// 处理文件删除
        /// </summary>
        private async Task<CommandResult> HandleFileDeleteAsync()
        {
            try
            {
                // 检查文件是否存在
                var fileInfoRequest = new FileInfoRequest { FileId = FilePath };
                var fileInfo = await _fileStorageService.GetFileInfoAsync(fileInfoRequest);
                if (fileInfo == null || !fileInfo.Success)
                {
                    return CommandResult.CreateError("文件不存在");
                }

                // 检查用户权限
                var hasPermission = await _fileStorageService.CheckDeletePermissionAsync(FilePath, SessionInfo.UserId?.ToString());
                if (!hasPermission)
                {
                    return CommandResult.CreateError("没有删除权限");
                }

                // 执行删除
                var deleteRequest = new FileDeleteRequest { FileId = FilePath };
                var deleteResult = await _fileStorageService.DeleteFileAsync(deleteRequest);
                if (deleteResult.Success)
                {
                    var response = new FileOperationResponse
                    {
                        Success = true,
                        FileName = fileInfo.FileInfo?.OriginalName ?? FileName,
                        FilePath = FilePath,
                        Message = "文件删除成功"
                    };

                    return CommandResult.CreateSuccess(response);
                }
                else
                {
                    return CommandResult.CreateError($"文件删除失败: {deleteResult.Message}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"文件删除处理异常: {FilePath}");
                return CommandResult.CreateError("文件删除处理异常");
            }
        }

        /// <summary>
        /// 处理文件列表
        /// </summary>
        private async Task<CommandResult> HandleFileListAsync()
        {
            try
            {
                var listRequest = new FileListRequest { Category = FilePath }; // 使用FilePath作为Category
                var fileList = await _fileStorageService.ListFilesAsync(listRequest);

                var response = new FileListResponse
                {
                    Success = true,
                    Files = fileList?.Files,
                    Message = "获取文件列表成功"
                };

                return CommandResult.CreateSuccess(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"获取文件列表异常: {FilePath}");
                return CommandResult.CreateError("获取文件列表异常");
            }
        }

        /// <summary>
        /// 处理获取文件信息
        /// </summary>
        private async Task<CommandResult> HandleGetFileInfoAsync()
        {
            try
            {
                var fileInfoRequest = new FileInfoRequest { FileId = FilePath };
                var fileInfo = await _fileStorageService.GetFileInfoAsync(fileInfoRequest);
                if (fileInfo == null || !fileInfo.Success)
                {
                    return CommandResult.CreateError("文件不存在");
                }

                return CommandResult.CreateSuccess(fileInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"获取文件信息异常: {FilePath}");
                return CommandResult.CreateError("获取文件信息异常");
            }
        }

        /// <summary>
        /// 计算文件哈希值
        /// </summary>
        private string CalculateFileHash(byte[] data)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var hash = sha256.ComputeHash(data);
                return Convert.ToBase64String(hash);
            }
        }

        /// <summary>
        /// 检查文件名是否包含非法字符
        /// </summary>
        private bool ContainsInvalidPathChars(string fileName)
        {
            var invalidChars = System.IO.Path.GetInvalidFileNameChars();
            return fileName.IndexOfAny(invalidChars) >= 0;
        }
    }

    /// <summary>
    /// 文件操作类型
    /// </summary>
    public enum FileOperationType
    {
        Unknown = 0,
        Upload = 1,
        Download = 2,
        Delete = 3,
        List = 4,
        GetInfo = 5
    }

    /// <summary>
    /// 文件操作响应
    /// </summary>
    public class FileOperationResponse
    {
        public bool Success { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public long FileSize { get; set; }
        public byte[] FileData { get; set; }
        public string FileHash { get; set; }
        public string Message { get; set; }
    }

    /// <summary>
    /// 文件列表响应
    /// </summary>
    public class FileListResponse
    {
        public bool Success { get; set; }
        public List<FileInfo> Files { get; set; }
        public string Message { get; set; }
    }

    /// <summary>
    /// 文件存储结果
    /// </summary>
    public class FileStorageResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string FilePath { get; set; }
    }
}