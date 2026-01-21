using System;
using System.Collections.Generic;
using System.Linq;
using RUINORERP.Model;
using RUINORERP.PacketSpec.Models.Common;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Responses;

namespace RUINORERP.PacketSpec.Models.FileManagement
{
    /// <summary>
    /// 文件上传请求 - 支持单文件和多文件上传
    /// </summary>
    public class FileUploadRequest : RequestBase
    {
        /// <summary>
        /// 文件存储信息
        /// </summary>
        public List<tb_FS_FileStorageInfo> FileStorageInfos { get; set; } = new List<tb_FS_FileStorageInfo>();

        /// <summary>
        /// 业务类型 (BizType枚举值)
        /// </summary>
        public int? BusinessType { get; set; }

        /// <summary>
        /// 关联字段 - 支持按字段关联(如 VoucherImage、PaymentImagePath)
        /// </summary>
        public string RelatedField { get; set; }

        /// <summary>
        /// 业务编号 (兼容旧版)
        /// </summary>
        public string BusinessNo { get; set; }

        /// <summary>
        /// 业务主键ID (单据主表ID)
        /// 单表业务时使用此项,默认为主表
        /// </summary>
        public long? BusinessId { get; set; }

        /// <summary>
        /// 是否明细表文件 (false=主表, true=明细表)
        /// 默认false(主表)
        /// </summary>
        public bool IsDetailTable { get; set; } = false;

        /// <summary>
        /// 明细表主键ID (仅当IsDetailTable=true时有效)
        /// </summary>
        public long? DetailId { get; set; }

        public long? Created_by { get; set; }
    }



    /// <summary>
    /// 文件上传响应 - 使用统一的ApiResponse模式
    /// 与FileDownloadResponse结构保持一致
    /// </summary>
    public class FileUploadResponse : ResponseBase
    {
        /// <summary>
        /// 文件存储信息列表
        /// </summary>
        public List<tb_FS_FileStorageInfo> FileStorageInfos { get; set; } = new List<tb_FS_FileStorageInfo>();

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public FileUploadResponse() : base() { }

        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        public FileUploadResponse(bool success, string message, List<tb_FS_FileStorageInfo> fileStorageInfos = null)
        {
            this.IsSuccess = success;
            this.Message = message;
            this.FileStorageInfos = fileStorageInfos ?? new List<tb_FS_FileStorageInfo>();
            this.Timestamp = DateTime.Now;
        }

        /// <summary>
        /// 创建单文件成功结果
        /// </summary>
        public static FileUploadResponse CreateSuccess(tb_FS_FileStorageInfo fileStorageInfo, string message = "文件上传成功")
        {
            return new FileUploadResponse(true, message, new List<tb_FS_FileStorageInfo> { fileStorageInfo });
        }



    }



    /// <summary>
    /// 文件信息查询请求 - 支持单文件和多文件查询
    /// 整合tb_FS_FileStorageInfo实体
    /// </summary>
    public class FileInfoRequest : RequestBase
    {
        /// <summary>
        /// 是否为多文件查询
        /// </summary>
        public bool IsMultiFile { get; set; } = false;

        /// <summary>
        /// 文件存储信息（单文件模式）
        /// </summary>
        public tb_FS_FileStorageInfo FileStorageInfo { get; set; }



        /// <summary>
        /// 多文件模式下的文件存储信息列表
        /// </summary>
        public List<tb_FS_FileStorageInfo> FileStorageInfos { get; set; } = new List<tb_FS_FileStorageInfo>();



        /// <summary>
        /// 初始化兼容数据结构
        /// 确保新旧API都能正常工作
        /// </summary>
        public void InitializeCompatibility()
        {
            if (FileStorageInfo == null)
                FileStorageInfo = new tb_FS_FileStorageInfo();

            if (FileStorageInfos == null)
                FileStorageInfos = new List<tb_FS_FileStorageInfo>();
        }
    }

    /// <summary>
    /// 文件信息响应 - 使用统一的ApiResponse模式
    /// 与FileDownloadResponse结构保持一致
    /// </summary>
    public class FileInfoResponse : ResponseBase
    {
        /// <summary>
        /// 文件存储信息列表
        /// </summary>
        public List<tb_FS_FileStorageInfo> FileStorageInfos { get; set; } = new List<tb_FS_FileStorageInfo>();

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public FileInfoResponse() : base() { }

        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        public FileInfoResponse(bool success, string message, List<tb_FS_FileStorageInfo> fileStorageInfos = null, int code = 200)
        {
            this.IsSuccess = success;
            this.Message = message;
            this.FileStorageInfos = fileStorageInfos ?? new List<tb_FS_FileStorageInfo>();
            this.Timestamp = DateTime.Now;
        }

        /// <summary>
        /// 创建单文件信息成功结果
        /// </summary>
        public static FileInfoResponse CreateSuccess(tb_FS_FileStorageInfo fileInfo, string message = "获取文件信息成功")
        {
            return new FileInfoResponse(true, message, new List<tb_FS_FileStorageInfo> { fileInfo }, 200);
        }



        /// <summary>
        /// 创建失败结果
        /// </summary>
        public static FileInfoResponse CreateFailure(string message, int code = 500)
        {
            return new FileInfoResponse(false, message, null, code);
        }
    }

    /// <summary>
    /// 文件列表请求 - 保持与原接口兼容
    /// </summary>
    public class FileListRequest : RequestBase
    {
        public int? BusinessType { get; set; }

        public string BusinessId { get; set; }

        public int PageSize { get; set; } = 10;

        public int PageIndex { get; set; } = 1;
    }

    /// <summary>
    /// 文件列表响应数据
    /// </summary>
    public class FileListResponseData
    {
        /// <summary>
        /// 文件存储信息列表
        /// </summary>
        public List<tb_FS_FileStorageInfo> FileStorageInfos { get; set; } = new List<tb_FS_FileStorageInfo>();

        /// <summary>
        /// 总数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页条数
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
        public static FileListResponse CreateSuccess(FileListResponseData data, string message = "获取文件列表成功")
        {
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

    /// <summary>
    /// 向后兼容性辅助类
    /// 用于在系统升级过程中处理旧版本请求和确保新旧API兼容
    /// </summary>
    public static class FileOperationCompatibilityHelper
    {
        /// <summary>
        /// 检查请求是否为多文件操作
        /// </summary>
        /// <param name="fileCount">文件数量</param>
        /// <returns>是否为多文件操作</returns>
        public static bool IsMultiFileOperation(int fileCount)
        {
            return fileCount > 1;
        }

        /// <summary>
        /// 将文件ID列表转换为文件存储信息列表
        /// 用于从旧版API兼容到新版API
        /// </summary>
        /// <param name="fileIds">文件ID列表</param>
        /// <returns>文件存储信息列表</returns>
        public static List<tb_FS_FileStorageInfo> ConvertFileIdsToStorageInfos(List<string> fileIds)
        {
            var storageInfos = new List<tb_FS_FileStorageInfo>();
            if (fileIds != null)
            {
                foreach (var fileId in fileIds)
                {
                    if (long.TryParse(fileId, out long id))
                    {
                        storageInfos.Add(new tb_FS_FileStorageInfo { FileId = id });
                    }
                }
            }
            return storageInfos;
        }

        /// <summary>
        /// 将文件存储信息列表转换为文件ID列表
        /// 用于从新版API兼容到旧版API
        /// </summary>
        /// <param name="storageInfos">文件存储信息列表</param>
        /// <returns>文件ID列表</returns>
        public static List<string> ConvertStorageInfosToFileIds(List<tb_FS_FileStorageInfo> storageInfos)
        {
            if (storageInfos == null)
                return new List<string>();

            return storageInfos.Select(s => s.FileId.ToString()).ToList();
        }

        /// <summary>
        /// 创建单文件上传成功结果（向后兼容）
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <param name="message">消息</param>
        /// <returns>文件上传响应</returns>
        public static FileUploadResponse CreateLegacySuccess(long fileId, string message = "文件上传成功")
        {
            var fileInfo = new tb_FS_FileStorageInfo { FileId = fileId };
            return FileUploadResponse.CreateSuccess(fileInfo, message);
        }
    }

    /// <summary>
    /// 文件权限检查请求
    /// </summary>
    public class FilePermissionCheckRequest : RequestBase
    {
        /// <summary>
        /// 文件ID
        /// </summary>
        public long FileId { get; set; }

        /// <summary>
        /// 权限类型: read(读), write(写), delete(删除), download(下载)
        /// </summary>
        public string PermissionType { get; set; }

        /// <summary>
        /// 请求时间戳（用于验证请求有效性）
        /// </summary>
        public DateTime RequestTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 验证请求是否有效
        /// </summary>
        public bool IsValid()
        {
            return FileId > 0 &&
                   !string.IsNullOrEmpty(PermissionType) &&
                   RequestTime > DateTime.Now.AddMinutes(-5); // 请求有效期5分钟
        }
    }

    /// <summary>
    /// 文件权限检查响应
    /// </summary>
    public class FilePermissionCheckResponse : ResponseBase
    {
        /// <summary>
        /// 是否有权限
        /// </summary>
        public bool HasPermission { get; set; }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public FilePermissionCheckResponse() : base() { }

        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        public FilePermissionCheckResponse(bool success, string message, bool hasPermission = false, int code = 200)
        {
            this.IsSuccess = success;
            this.Message = message;
            this.HasPermission = hasPermission;
            this.Timestamp = DateTime.Now;
        }

        /// <summary>
        /// 创建成功结果
        /// </summary>
        public static FilePermissionCheckResponse CreateSuccess(bool hasPermission, string message = "权限验证通过")
        {
            return new FilePermissionCheckResponse(true, message, hasPermission, 200);
        }

        /// <summary>
        /// 创建失败结果
        /// </summary>
        public static FilePermissionCheckResponse CreateFailure(string message, int code = 500)
        {
            return new FilePermissionCheckResponse(false, message, false, code);
        }
    }

}
