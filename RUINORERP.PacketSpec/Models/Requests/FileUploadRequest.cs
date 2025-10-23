using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RUINORERP.Model;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.PacketSpec.Models.Core;
using Newtonsoft.Json;

namespace RUINORERP.PacketSpec.Models.Requests
{
    /// <summary>
    /// 文件上传请求 - 用于处理文件上传操作
    /// </summary>
    [JsonObject]
    public class FileUploadRequest : RequestBase
    {
        /// <summary>
        /// 文件�?
        /// </summary>
        [JsonProperty(Order=10)]
        public string FileName { get; set; }

        /// <summary>
        /// 文件分类: Expenses/Products/Payments�?
        /// </summary>
        [JsonProperty(Order=11)]
        public string Category { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        [JsonProperty(Order=12)]
        public long FileSize { get; set; }

        /// <summary>
        /// 文件数据
        /// </summary>
        [JsonProperty(Order=13)]
        public byte[] Data { get; set; }

        /// <summary>
        /// 分块索引
        /// </summary>
        [JsonProperty(Order=14)]
        public int ChunkIndex { get; set; }

        /// <summary>
        /// 总分块数
        /// </summary>
        [JsonProperty(Order=15)]
        public int TotalChunks { get; set; }

        /// <summary>
        /// 目标路径
        /// </summary>
        [JsonProperty(Order=16)]
        public string TargetPath { get; set; }

        /// <summary>
        /// 上传用户
        /// </summary>
        [JsonProperty(Order=17)]
        public string UploadedBy { get; set; }

        /// <summary>
        /// 上传时间
        /// </summary>
        [JsonProperty(Order=18)]
        public DateTime UploadTime { get; set; } = DateTime.Now;
    }

    /// <summary>
    /// 文件上传响应 - 使用统一的ApiResponse模式
    /// </summary>
    [JsonObject]
    public class FileUploadResponse : ResponseBase<FileUploadResponseData>
    {
        /// <summary>
        /// 默认构造函�?
        /// </summary>
        public FileUploadResponse() : base() { }

        /// <summary>
        /// 带参数的构造函�?
        /// </summary>
        public FileUploadResponse(bool success, string message, FileUploadResponseData data = null, int code = 200) 
        {
            this.IsSuccess = success;
            this.Message = message;
            this.Data = data;
            this.Timestamp = DateTime.Now;
        }

        /// <summary>
        /// 创建成功结果
        /// </summary>
        public static FileUploadResponse CreateSuccess(string fileId, string message = "文件上传成功")
        {
            return new FileUploadResponse(true, message, new FileUploadResponseData
            {
                FileId = fileId
            }, 200);
        }

        /// <summary>
        /// 创建失败结果
        /// </summary>
        public static FileUploadResponse CreateFailure(string message, int code = 500)
        {
            return new FileUploadResponse(false, message, null, code);
        }
    }

    /// <summary>
    /// 文件上传响应数据
    /// </summary>
    [JsonObject]
    public class FileUploadResponseData
    {
        /// <summary>
        /// 文件唯一标识�?
        /// </summary>
        [JsonProperty(Order=0)]
        public string FileId { get; set; }
    }


    // 文件信息�?
    [JsonObject]
    public class FileStorageInfo
    {
        private string filePath;

        public FileStorageInfo(string filePath)
        {
            this.filePath = filePath;
        }

        [JsonProperty(Order=0)]
        public string FileId { get; set; }
        
        [JsonProperty(Order=1)]
        public string OriginalName { get; set; }
        
        [JsonProperty(Order=2)]
        public string Category { get; set; }
        
        [JsonProperty(Order=3)]
        public long Size { get; set; }
        
        [JsonProperty(Order=4)]
        public DateTime UploadTime { get; set; }
        
        [JsonProperty(Order=5)]
        public DateTime LastModified { get; set; }
        
        [JsonProperty(Order=6)]
        public string Version { get; set; }

        [JsonProperty(Order=7)]
        public string UploadedBy { get; set; }

        [JsonProperty(Order=8)]
        public string MimeType { get; set; }

        [JsonProperty(Order=9)]
        public string FilePath { get; set; }

    }

    // 请求和响应类
    [JsonObject]
    public class FileDeleteRequest : RequestBase
    {
        [JsonProperty(Order=10)]
        public string FileId { get; set; }
    }

    /// <summary>
    /// 文件删除响应 - 使用统一的ApiResponse模式
    /// </summary>
    [JsonObject]
    public class FileDeleteResponse : ResponseBase
    {
        /// <summary>
        /// 默认构造函�?
        /// </summary>
        public FileDeleteResponse() : base() { }

        /// <summary>
        /// 带参数的构造函�?
        /// </summary>
        public FileDeleteResponse(bool success, string message, int code = 200) 
        {
            IsSuccess = success;
            Message = message;
        }

        /// <summary>
        /// 创建成功结果
        /// </summary>
        public static FileDeleteResponse CreateSuccess(string message = "文件删除成功")
        {
            return new FileDeleteResponse(true, message, 200);
        }

        /// <summary>
        /// 创建失败结果
        /// </summary>
        public static FileDeleteResponse CreateFailure(string message, int code = 500)
        {
            return new FileDeleteResponse(false, message, code);
        }
    }

    [JsonObject]
    public class FileInfoRequest : RequestBase
    {
        [JsonProperty(Order=10)]
        public string FileId { get; set; }
    }

    /// <summary>
    /// 文件信息响应 - 使用统一的ApiResponse模式
    /// </summary>
    [JsonObject]
    public class FileInfoResponse : ResponseBase<FileStorageInfo>
    {
        /// <summary>
        /// 默认构造函�?
        /// </summary>
        public FileInfoResponse() : base() { }

        /// <summary>
        /// 带参数的构造函�?
        /// </summary>
        public FileInfoResponse(bool success, string message, FileStorageInfo data = null, int code = 200) 
        {
            this.IsSuccess = success;
            this.Message = message;
            this.Data = data;
            this.Timestamp = DateTime.Now;
        }

        /// <summary>
        /// 创建成功结果
        /// </summary>
        public static FileInfoResponse CreateSuccess(FileStorageInfo fileInfo, string message = "获取文件信息成功")
        {
            return new FileInfoResponse(true, message, fileInfo, 200);
        }

        /// <summary>
        /// 创建失败结果
        /// </summary>
        public static FileInfoResponse CreateFailure(string message, int code = 500)
        {
            return new FileInfoResponse(false, message, null, code);
        }
    }

    [JsonObject]
    public class FileListRequest : RequestBase
    {
        [JsonProperty(Order=10)]
        public string Category { get; set; }
        
        [JsonProperty(Order=11)]
        public string Pattern { get; set; } // 文件名模式匹�?
        
        [JsonProperty(Order=12)]
        public int PageIndex { get; set; } = 1;
        
        [JsonProperty(Order=13)]
        public int PageSize { get; set; } = 20;
    }

    /// <summary>
    /// 文件列表响应数据�?
    /// </summary>
    [JsonObject]
    public class FileListResponseData
    {
        /// <summary>
        /// 文件列表
        /// </summary>
        [JsonProperty(Order=0)]
        public List<FileStorageInfo> Files { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        [JsonProperty(Order=1)]
        public int TotalCount { get; set; }

        /// <summary>
        /// 当前页码
        /// </summary>
        [JsonProperty(Order=2)]
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页大小
        /// </summary>
        [JsonProperty(Order=3)]
        public int PageSize { get; set; }
    }

    /// <summary>
    /// 文件列表响应 - 使用统一的ApiResponse模式
    /// </summary>
    [JsonObject]
    public class FileListResponse : ResponseBase<FileListResponseData>
    {
        /// <summary>
        /// 默认构造函�?
        /// </summary>
        public FileListResponse() : base() { }

        /// <summary>
        /// 带参数的构造函�?
        /// </summary>
        public FileListResponse(bool success, string message, FileListResponseData data = null, int code = 200) 
        {
            this.IsSuccess = success;
            this.Message = message;
            this.Data = data;
            this.Timestamp = DateTime.Now;
        }

        /// <summary>
        /// 创建成功结果
        /// </summary>
        public static FileListResponse CreateSuccess(List<FileStorageInfo> files, int totalCount, int pageIndex, int pageSize, string message = "获取文件列表成功")
        {
            var data = new FileListResponseData
            {
                Files = files,
                TotalCount = totalCount,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            return new FileListResponse(true, message, data, 200);
        }

        /// <summary>
        /// 创建失败结果
        /// </summary>
        public static FileListResponse CreateFailure(string message, int code = 500)
        {
            return new FileListResponse(false, message, null, code);
        }
    }

}


