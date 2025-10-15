using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RUINORERP.Model;
using RUINORERP.PacketSpec.Models.Responses;

namespace RUINORERP.PacketSpec.Models.Requests
{
    // 文件上传请求
    public class FileUploadRequest : RequestBase
    {
        public string FileName { get; set; }
        public string Category { get; set; } // 分类: Expenses/Products/Payments等
        public long FileSize { get; set; }
        public byte[] Data { get; set; }
        public int ChunkIndex { get; set; } // 分块索引
        public int TotalChunks { get; set; } // 总分块数
    }

    /// <summary>
    /// 文件上传响应 - 使用统一的ApiResponse模式
    /// </summary>
    public class FileUploadResponse : ResponseBase<FileUploadResponseData>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public FileUploadResponse() : base() { }

        /// <summary>
        /// 带参数的构造函数
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
    public class FileUploadResponseData
    {
        /// <summary>
        /// 文件唯一标识符
        /// </summary>
        public string FileId { get; set; }
    }


    // 文件信息类
    public class FileInfo
    {
        private string filePath;

        public FileInfo(string filePath)
        {
            this.filePath = filePath;
        }

        public string FileId { get; set; }
        public string OriginalName { get; set; }
        public string Category { get; set; }
        public long Size { get; set; }
        public DateTime UploadTime { get; set; }
        public DateTime LastModified { get; set; }
        public string Version { get; set; }

 

    }

    // 请求和响应类
    public class FileDeleteRequest : RequestBase
    {
        public string FileId { get; set; }
    }

    /// <summary>
    /// 文件删除响应 - 使用统一的ApiResponse模式
    /// </summary>
    public class FileDeleteResponse : ResponseBase
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public FileDeleteResponse() : base() { }

        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        public FileDeleteResponse(bool success, string message, int code = 200) 
        {
            IsSuccess = success;
            Message = message;
        }

        /// <summary>
        /// 创建成功结果
        /// </summary>
#pragma warning disable CS0108 // 成员隐藏继承的成员；缺少关键字 new
        public static FileDeleteResponse CreateSuccess(string message = "文件删除成功")
#pragma warning restore CS0108 // 成员隐藏继承的成员；缺少关键字 new
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

    public class FileInfoRequest : RequestBase
    {
        public string FileId { get; set; }
    }

    /// <summary>
    /// 文件信息响应 - 使用统一的ApiResponse模式
    /// </summary>
    public class FileInfoResponse : ResponseBase<FileInfo>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public FileInfoResponse() : base() { }

        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        public FileInfoResponse(bool success, string message, FileInfo data = null, int code = 200) 
        {
            this.IsSuccess = success;
            this.Message = message;
            this.Data = data;
            this.Timestamp = DateTime.Now;
        }

        /// <summary>
        /// 创建成功结果
        /// </summary>
#pragma warning disable CS0108 // 成员隐藏继承的成员；缺少关键字 new
        public static FileInfoResponse CreateSuccess(FileInfo fileInfo, string message = "获取文件信息成功")
#pragma warning restore CS0108 // 成员隐藏继承的成员；缺少关键字 new
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

    public class FileListRequest : RequestBase
    {
        public string Category { get; set; }
        public string Pattern { get; set; } // 文件名模式匹配
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

    /// <summary>
    /// 文件列表响应数据类
    /// </summary>
    public class FileListResponseData
    {
        /// <summary>
        /// 文件列表
        /// </summary>
        public List<FileInfo> Files { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页大小
        /// </summary>
        public int PageSize { get; set; }
    }

    /// <summary>
    /// 文件列表响应 - 使用统一的ApiResponse模式
    /// </summary>
    public class FileListResponse : ResponseBase<FileListResponseData>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public FileListResponse() : base() { }

        /// <summary>
        /// 带参数的构造函数
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
        public static FileListResponse CreateSuccess(List<FileInfo> files, int totalCount, int pageIndex, int pageSize, string message = "获取文件列表成功")
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
