using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RUINORERP.PacketSpec.Models.Responses;

namespace RUINORERP.PacketSpec.Models.Requests
{
    /// <summary>
    /// 文件下载请求类
    /// 客户端通过此请求从服务器下载文件
    /// </summary>
    public class FileDownloadRequest
    {
        /// <summary>
        /// 文件唯一标识符（服务器存储的文件名）
        /// </summary>
        [Required(ErrorMessage = "文件ID不能为空")]
        public string FileId { get; set; }

        /// <summary>
        /// 文件分类（用于确定文件存储路径）
        /// 可选参数，如果为空，服务器会尝试在所有分类中查找文件
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// 是否返回文件内容
        /// 如果为false，则只返回文件信息而不返回文件数据
        /// 默认值为true
        /// </summary>
        public bool IncludeData { get; set; } = true;

        /// <summary>
        /// 客户端请求的版本号（用于支持文件版本控制）
        /// 如果为null或空字符串，则返回最新版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 客户端标识（用于统计和日志记录）
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// 请求时间戳（用于验证请求有效性）
        /// </summary>
        public DateTime RequestTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 验证请求是否有效
        /// </summary>
        public bool IsValid()
        {
            return !string.IsNullOrEmpty(FileId) &&
                   RequestTime > DateTime.UtcNow.AddMinutes(-5); // 请求有效期5分钟
        }
    }

    /// <summary>
    /// 文件下载响应数据类
    /// </summary>
    public class FileDownloadResponseData
    {
        /// <summary>
        /// 原始文件名（上传时的文件名）
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 文件数据（如果IncludeData为false，则此字段为空）
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>
        /// 文件大小（字节数）
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// 文件内容类型（MIME类型）
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// 文件最后修改时间
        /// </summary>
        public DateTime LastModified { get; set; }

        /// <summary>
        /// 文件版本标识（如果支持版本控制）
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 下载令牌（用于后续操作，如断点续传）
        /// </summary>
        public string DownloadToken { get; set; }
    }

    /// <summary>
    /// 文件下载响应类
    /// 服务器返回给客户端的下载结果 - 使用统一的ApiResponse模式
    /// </summary>
    public class FileDownloadResponse : ApiResponse<FileDownloadResponseData>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public FileDownloadResponse() : base() { }

        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        public FileDownloadResponse(bool success, string message, FileDownloadResponseData data = null, int code = 200) 
            : base(success, message, data, code) { }

        /// <summary>
        /// 创建成功结果
        /// </summary>
        public static FileDownloadResponse CreateSuccess(FileDownloadResponseData data, string message = "文件下载成功")
        {
            return new FileDownloadResponse(true, message, data, 200);
        }

        /// <summary>
        /// 创建失败结果
        /// </summary>
        public static FileDownloadResponse CreateFailure(string message, int code = 500)
        {
            return new FileDownloadResponse(false, message, null, code);
        }
    }

    /// <summary>
    /// 文件信息类（用于返回文件元数据）
    /// </summary>
    public class FileMetadata
    {
        /// <summary>
        /// 文件唯一标识符
        /// </summary>
        public string FileId { get; set; }

        /// <summary>
        /// 原始文件名
        /// </summary>
        public string OriginalName { get; set; }

        /// <summary>
        /// 文件分类
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// 文件大小（字节数）
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// 文件内容类型（MIME类型）
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// 文件上传时间
        /// </summary>
        public DateTime UploadTime { get; set; }

        /// <summary>
        /// 文件最后修改时间
        /// </summary>
        public DateTime LastModified { get; set; }

        /// <summary>
        /// 文件版本（如果支持版本控制）
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 文件哈希值（用于校验完整性）
        /// </summary>
        public string Hash { get; set; }
    }

    /// <summary>
    /// 文件下载选项类（用于扩展下载功能）
    /// </summary>
    public class DownloadOptions
    {
        /// <summary>
        /// 是否启用断点续传
        /// </summary>
        public bool EnableResume { get; set; } = true;

        /// <summary>
        /// 下载速度限制（字节/秒），0表示无限制
        /// </summary>
        public int SpeedLimit { get; set; } = 0;

        /// <summary>
        /// 下载超时时间（秒）
        /// </summary>
        public int Timeout { get; set; } = 300; // 5分钟

        /// <summary>
        /// 是否启用压缩传输
        /// </summary>
        public bool EnableCompression { get; set; } = true;

        /// <summary>
        /// 是否启用加密传输
        /// </summary>
        public bool EnableEncryption { get; set; } = false;
    }

    /// <summary>
    /// 分块下载请求类（支持大文件分块下载）
    /// </summary>
    public class ChunkedDownloadRequest : FileDownloadRequest
    {
        /// <summary>
        /// 分块大小（字节数）
        /// </summary>
        public int ChunkSize { get; set; } = 1024 * 1024; // 默认1MB

        /// <summary>
        /// 请求的分块索引（从0开始）
        /// </summary>
        public int ChunkIndex { get; set; }

        /// <summary>
        /// 下载令牌（用于标识同一个文件的连续下载请求）
        /// </summary>
        public string DownloadToken { get; set; }
    }

    /// <summary>
    /// 分块下载响应类
    /// </summary>
    public class ChunkedDownloadResponse : FileDownloadResponse
    {
        /// <summary>
        /// 当前分块索引
        /// </summary>
        public int ChunkIndex { get; set; }

        /// <summary>
        /// 总分块数
        /// </summary>
        public int TotalChunks { get; set; }

        /// <summary>
        /// 当前分块大小
        /// </summary>
        public int ChunkSize { get; set; }

        /// <summary>
        /// 文件总大小
        /// </summary>
        public new long FileSize { get; set; }

        /// <summary>
        /// 下载令牌（用于后续分块请求）
        /// </summary>
        public new string DownloadToken { get; set; }
    }
}
