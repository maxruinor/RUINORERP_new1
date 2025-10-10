using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SuperSocket.Server.Abstractions.Session;
using RUINORERP.Server.Network.Models;
using RUINORERP.PacketSpec.Enums;
using RUINORERP.Server.Network.Services;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Commands.FileTransfer;

namespace RUINORERP.Server.Network.Commands
{
    /// <summary>
    /// 文件处理器 - 处理文件上传、下载、删除等业务逻辑
    /// </summary>
    public class FileHandler
    {  
        private readonly SessionService _sessionService;
        private readonly ILogger<FileHandler> _logger;
        private readonly string _fileStoragePath;

        public FileHandler(SessionService sessionService, ILogger<FileHandler> logger = null)
        {
            _sessionService = sessionService;
            _logger = logger;
            _fileStoragePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FileStorage");
            
            // 确保文件存储目录存在
            if (!Directory.Exists(_fileStoragePath))
            {
                Directory.CreateDirectory(_fileStoragePath);
            }
        }

        /// <summary>
        /// 处理文件命令
        /// </summary>
        public async Task<PacketModel> HandleAsync(IAppSession session, PacketModel packet)
        {
            try
            {
                return packet.Command switch
                {
                    (uint)FileCommands.FileUpload => await HandleFileUpload(session, packet),
                    (uint)FileCommands.FileDownload => await HandleFileDownload(session, packet),
                    (uint)FileCommands.FileDelete => await HandleFileDelete(session, packet),
                    (uint)FileCommands.FileList => await HandleFileList(session, packet),
                    (uint)FileCommands.FileInfo => await HandleFileInfo(session, packet),
                    _ => packet.CreateResponse(new { Error = "Unsupported file command", Command = packet.Command })
                };
            }
            catch (Exception ex)
            {
                LogError($"处理文件命令时出错: Command={packet.Command}, 错误: {ex.Message}", ex);
                return packet.CreateResponse(new { Error = ex.Message, Success = false });
            }
        }

        /// <summary>
        /// 处理文件上传
        /// </summary>
        private async Task<PacketModel> HandleFileUpload(IAppSession session, PacketModel packet)
        {
            try
            {
                var uploadRequest = packet.GetDataAsJson<FileUploadRequest>();
                if (uploadRequest == null || uploadRequest.FileData == null)
                {
                    return packet.CreateResponse(new { Success = false, Message = "文件上传请求格式错误" });
                }

                LogInfo($"文件上传: SessionID={session.SessionID}, FileName={uploadRequest.FileName}, Size={uploadRequest.FileData.Length}");

                // 生成唯一文件名
                var fileId = Guid.NewGuid().ToString();
                var fileExtension = Path.GetExtension(uploadRequest.FileName);
                var savedFileName = $"{fileId}{fileExtension}";
                var filePath = Path.Combine(_fileStoragePath, savedFileName);

                // 保存文件
                await File.WriteAllBytesAsync(filePath, uploadRequest.FileData);

                // 记录文件信息
                var fileInfo = new FileInfoData
                {
                    FileId = fileId,
                    OriginalFileName = uploadRequest.FileName,
                    SavedFileName = savedFileName,
                    FilePath = filePath,
                    FileSize = uploadRequest.FileData.Length,
                    UploadTime = DateTime.UtcNow,
                    UploaderSessionId = session.SessionID,
                    UploaderUserName = _sessionService.GetSessionProperty<string>(session.SessionID, "UserName", "Unknown"),
                    ContentType = uploadRequest.ContentType
                };

                // 这里可以将文件信息保存到数据库
                // await SaveFileInfoToDatabase(fileInfo);

                LogInfo($"文件上传成功: FileId={fileId}, FileName={uploadRequest.FileName}");

                return packet.CreateResponse(new
                {
                    Success = true,
                    Message = "文件上传成功",
                    FileId = fileId,
                    FileName = uploadRequest.FileName,
                    FileSize = uploadRequest.FileData.Length,
                    UploadTime = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                LogError($"文件上传时出错: {ex.Message}", ex);
                return packet.CreateResponse(new { Success = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// 处理文件下载
        /// </summary>
        private async Task<PacketModel> HandleFileDownload(IAppSession session, PacketModel packet)
        {
            try
            {
                var downloadRequest = packet.GetDataAsJson<FileDownloadRequest>();
                if (downloadRequest == null || string.IsNullOrEmpty(downloadRequest.FileId))
                {
                    return packet.CreateResponse(new { Success = false, Message = "文件下载请求格式错误" });
                }

                LogInfo($"文件下载: SessionID={session.SessionID}, FileId={downloadRequest.FileId}");

                // 查找文件 (这里简化处理，实际应该从数据库查询)
                var files = Directory.GetFiles(_fileStoragePath, $"{downloadRequest.FileId}.*");
                if (files.Length == 0)
                {
                    return packet.CreateResponse(new { Success = false, Message = "文件不存在" });
                }

                var filePath = files[0];
                var fileData = await File.ReadAllBytesAsync(filePath);
                var fileName = Path.GetFileName(filePath);

                LogInfo($"文件下载成功: FileId={downloadRequest.FileId}, FileName={fileName}, Size={fileData.Length}");

                return packet.CreateResponse(new FileDownloadResponse
                {
                    Success = true,
                    Message = "文件下载成功",
                    FileId = downloadRequest.FileId,
                    FileName = fileName,
                    FileData = fileData,
                    FileSize = fileData.Length,
                    ContentType = "application/octet-stream"
                });
            }
            catch (Exception ex)
            {
                LogError($"文件下载时出错: {ex.Message}", ex);
                return packet.CreateResponse(new { Success = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// 处理文件删除
        /// </summary>
        private async Task<PacketModel> HandleFileDelete(IAppSession session, PacketModel packet)
        {
            try
            {
                var deleteRequest = packet.GetDataAsJson<FileDeleteRequest>();
                if (deleteRequest == null || string.IsNullOrEmpty(deleteRequest.FileId))
                {
                    return packet.CreateResponse(new { Success = false, Message = "文件删除请求格式错误" });
                }

                LogInfo($"文件删除: SessionID={session.SessionID}, FileId={deleteRequest.FileId}");

                // 查找并删除文件
                var files = Directory.GetFiles(_fileStoragePath, $"{deleteRequest.FileId}.*");
                var deletedCount = 0;

                foreach (var filePath in files)
                {
                    try
                    {
                        File.Delete(filePath);
                        deletedCount++;
                        LogInfo($"文件已删除: {filePath}");
                    }
                    catch (Exception ex)
                    {
                        LogError($"删除文件失败: {filePath}, 错误: {ex.Message}", ex);
                    }
                }

                // 这里可以从数据库删除文件记录
                // await DeleteFileInfoFromDatabase(deleteRequest.FileId);

                return packet.CreateResponse(new
                {
                    Success = deletedCount > 0,
                    Message = deletedCount > 0 ? "文件删除成功" : "文件不存在",
                    FileId = deleteRequest.FileId,
                    DeletedCount = deletedCount
                });
            }
            catch (Exception ex)
            {
                LogError($"文件删除时出错: {ex.Message}", ex);
                return packet.CreateResponse(new { Success = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// 处理文件列表
        /// </summary>
        private async Task<PacketModel> HandleFileList(IAppSession session, PacketModel packet)
        {
            try
            {
                LogInfo($"获取文件列表: SessionID={session.SessionID}");

                var files = Directory.GetFiles(_fileStoragePath);
                var fileList = files.Select(filePath =>
                {
                    var fileInfo = new FileInfo(filePath);
                    var fileNameWithoutExt = Path.GetFileNameWithoutExtension(filePath);
                    
                    return new FileListItem
                    {
                        FileId = fileNameWithoutExt,
                        FileName = fileInfo.Name,
                        FileSize = fileInfo.Length,
                        CreatedTimeUtc = fileInfo.CreationTime,
                        ModifiedTime = fileInfo.LastWriteTime
                    };
                }).ToList();

                return packet.CreateResponse(new
                {
                    Success = true,
                    Message = "获取文件列表成功",
                    Files = fileList,
                    Count = fileList.Count,
                    Timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                LogError($"获取文件列表时出错: {ex.Message}", ex);
                return packet.CreateResponse(new { Success = false, Message = ex.Message });
            }
        }

        /// <summary>
        /// 处理文件信息查询
        /// </summary>
        private async Task<PacketModel> HandleFileInfo(IAppSession session, PacketModel packet)
        {
            try
            {
                var infoRequest = packet.GetDataAsJson<FileInfoRequest>();
                if (infoRequest == null || string.IsNullOrEmpty(infoRequest.FileId))
                {
                    return packet.CreateResponse(new { Success = false, Message = "文件信息请求格式错误" });
                }

                LogInfo($"获取文件信息: SessionID={session.SessionID}, FileId={infoRequest.FileId}");

                var files = Directory.GetFiles(_fileStoragePath, $"{infoRequest.FileId}.*");
                if (files.Length == 0)
                {
                    return packet.CreateResponse(new { Success = false, Message = "文件不存在" });
                }

                var filePath = files[0];
                var fileInfo = new FileInfo(filePath);

                var fileInfoData = new FileInfoData
                {
                    FileId = infoRequest.FileId,
                    OriginalFileName = fileInfo.Name,
                    SavedFileName = fileInfo.Name,
                    FilePath = filePath,
                    FileSize = fileInfo.Length,
                    UploadTime = fileInfo.CreationTime,
                    ContentType = "application/octet-stream"
                };

                return packet.CreateResponse(new
                {
                    Success = true,
                    Message = "获取文件信息成功",
                    FileInfo = fileInfoData
                });
            }
            catch (Exception ex)
            {
                LogError($"获取文件信息时出错: {ex.Message}", ex);
                return packet.CreateResponse(new { Success = false, Message = ex.Message });
            }
        }

        private void LogInfo(string message)
        {
            _logger?.LogInformation($"[FileHandler] {message}");
            Console.WriteLine($"[FileHandler] INFO: {message}");
        }

        private void LogError(string message, Exception ex = null)
        {
            _logger?.LogError(ex, $"[FileHandler] {message}");
            Console.WriteLine($"[FileHandler] ERROR: {message}");
            if (ex != null)
            {
                Console.WriteLine($"[FileHandler] Exception: {ex}");
            }
        }
    }

    // 数据模型
    public class FileUploadRequest
    {
        public string FileName { get; set; }
        public byte[] FileData { get; set; }
        public string ContentType { get; set; }
        public Dictionary<string, object> Metadata { get; set; }
    }

    public class FileDownloadRequest
    {
        public string FileId { get; set; }
    }

    public class FileDownloadResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string FileId { get; set; }
        public string FileName { get; set; }
        public byte[] FileData { get; set; }
        public long FileSize { get; set; }
        public string ContentType { get; set; }
    }

    public class FileDeleteRequest
    {
        public string FileId { get; set; }
    }

    public class FileInfoRequest
    {
        public string FileId { get; set; }
    }

    public class FileInfoData
    {
        public string FileId { get; set; }
        public string OriginalFileName { get; set; }
        public string SavedFileName { get; set; }
        public string FilePath { get; set; }
        public long FileSize { get; set; }
        public DateTime UploadTime { get; set; }
        public string UploaderSessionId { get; set; }
        public string UploaderUserName { get; set; }
        public string ContentType { get; set; }
    }

    public class FileListItem
    {
        public string FileId { get; set; }
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public DateTime CreatedTimeUtc { get; set; }
        public DateTime ModifiedTime { get; set; }
    }
}

