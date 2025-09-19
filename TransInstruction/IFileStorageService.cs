using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TransInstruction.DataModel;

namespace TransInstruction
{
    public interface IFileStorageService
    {
        Task<FileUploadResponse> UploadFileAsync(FileUploadRequest request);
        Task<FileDownloadResponse> DownloadFileAsync(FileDownloadRequest request);
        Task<FileDeleteResponse> DeleteFileAsync(FileDeleteRequest request);
        Task<FileInfoResponse> GetFileInfoAsync(FileInfoRequest request);
        Task<FileListResponse> ListFilesAsync(FileListRequest request);
    }



    // 在Autofac中注册
    //builder.RegisterType<FileStorageService>()
    //   .As<IFileStorageService>()
    //   .SingleInstance();
}
