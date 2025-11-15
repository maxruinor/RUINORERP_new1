using System;
using System.Collections.Generic;
using RUINORERP.PacketSpec.Models.Common;
using RUINORERP.Model;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.PacketSpec.Models.Core;

namespace RUINORERP.PacketSpec.Models.FileManagement
{
    /// <summary>
    /// 文件下载请求类
    /// 客户端通过此请求从服务器下载文件
    /// 下载可以单个下载
    /// </summary>
    public class FileDownloadRequest : RequestBase
    {
        /// <summary>
        /// 文件唯一标识符
        /// </summary>
        public tb_FS_FileStorageInfo FileStorageInfo { get; set; }

        /// <summary>
        /// 请求时间戳（用于验证请求有效性）
        /// </summary>
        public DateTime RequestTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 验证请求是否有效
        /// </summary>
        public bool IsValid()
        {
            return FileStorageInfo != null && FileStorageInfo.FileId != 0 &&
                   RequestTime > DateTime.Now.AddMinutes(-5); // 请求有效期5分钟
        }
    }



    /// <summary>
    /// 文件下载响应类
    /// 使用统一的ApiResponse模式
    /// </summary>
    public class FileDownloadResponse : ResponseBase
    {

        /// <summary>
        /// 文件数据
        /// </summary>
        public List<tb_FS_FileStorageInfo> FileStorageInfos { get; set; }

        /// <summary>
        /// 下载令牌
        /// </summary>
        public string DownloadToken { get; set; }
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public FileDownloadResponse() : base() { }

        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        public FileDownloadResponse(bool success, string message, List<tb_FS_FileStorageInfo> fileStorageInfos = null, int code = 200)
        {
            this.IsSuccess = success;
            this.Message = message;
            this.FileStorageInfos = fileStorageInfos;
            this.Timestamp = DateTime.Now;
        }

        /// <summary>
        /// 创建成功结果
        /// </summary>
        public static FileDownloadResponse CreateSuccess(List<tb_FS_FileStorageInfo> fileStorageInfos , string message = "文件下载成功")
        {
            return new FileDownloadResponse(true, message, fileStorageInfos, 200);
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
    /// 下载配置选项
    /// </summary>
    public class DownloadOptions
    {
        /// <summary>
        /// 是否启用断点续传
        /// </summary>
        public bool EnableResume { get; set; } = true;

        /// <summary>
        /// 下载超时时间（秒）
        /// </summary>
        public int Timeout { get; set; } = 300; // 5分钟

        /// <summary>
        /// 是否启用压缩传输
        /// </summary>
        public bool EnableCompression { get; set; } = true;
    }
}
