using RUINORERP.PacketSpec.Models.FileManagement;
using RUINORERP.PacketSpec.Models.Requests;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Server.Network.Interfaces.Services
{
    /// <summary>
    /// 文件存储服务接口
    /// 迁移自: RUINORERP.PacketSpec.Services.IFileStorageService
    /// </summary>
    public interface IFileStorageService
    {
        // 客户端通过Socket指令调用的方法
        Task<FileUploadResponse> UploadFileAsync(FileUploadRequest request);
        Task<FileDownloadResponse> DownloadFileAsync(FileDownloadRequest request);
        Task<FileDeleteResponse> DeleteFileAsync(FileDeleteRequest request);
        Task<FileInfoResponse> GetFileInfoAsync(FileInfoRequest request);
        Task<FileListResponse> ListFilesAsync(FileListRequest request);

        //Task<FileStorageResult> SaveFileAsync(string fileName, byte[] data, string userId);
        Task<byte[]> ReadFileAsync(string filePath);
        //Task<FileStorageResult> DeleteFileAsync(string filePath, string userId);
        Task<FileListResponse> GetFileListAsync(FileListRequest request);
        Task<bool> CheckDownloadPermissionAsync(string filePath, string userId);
        Task<bool> CheckDeletePermissionAsync(string filePath, string userId);
    }
}