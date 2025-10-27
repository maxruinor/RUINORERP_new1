using System;
using System.Collections.Generic;
using System.Linq;
using RUINORERP.Model;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Responses;

namespace RUINORERP.PacketSpec.Models.Requests
{
    /// <summary>
    /// 通用文件操作结果类
    /// 用于统一处理单文件和多文件操作的结果
    /// </summary>
    /// <typeparam name="T">结果数据类型</typeparam>
    public class FileOperationResult<T>
    {
        /// <summary>
        /// 操作目标标识（文件名或ID）
        /// </summary>
        public string TargetId { get; set; }

        /// <summary>
        /// 是否操作成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 错误消息（仅在失败时有值）
        /// </summary>
        public string ErrorMessage { get; set; }
    }
    /// <summary>
    /// 文件上传请求 - 支持单文件和多文件上传
    /// 多文件是指一个单据下有多个文件
    /// </summary>
    public class FileUploadRequest : RequestBase
    {
        /// <summary>
        /// 文件存储信息
        /// </summary>
        public List<tb_FS_FileStorageInfo> FileStorageInfos { get; set; } = new List<tb_FS_FileStorageInfo>();

        public int? BusinessType { get; set; }
        /*
        ## 结论与建议
                从当前代码实现来看，RelatedField字段确实存在冗余 。在现有业务流程中，它几乎总是被设置为固定值"MainFile"，并且没有基于此字段的特殊业务逻辑处理。理论上可以只保留BusinessNo字段来完成文件与业务的关联。

        然而，在进行实际修改时需要考虑以下因素：

        1. 修改范围 ：删除或合并字段需要修改实体类、DTO、UI界面、验证器等多个地方
        2. 未来扩展性 ：RelatedField可能是为了将来支持在同一业务记录下关联不同类型的文件（如主文件、附件、说明文档等）而预留的字段
        3. 兼容性问题 ：需要考虑对现有数据的影响，可能需要数据迁移
        如果当前业务需求确实简单，且未来短期内没有扩展计划，可以考虑合并这两个字段，简化数据模型。否则，建议保留当前结构，以支持未来可能的业务扩展*/
        public string RelatedField { get; set; }

        /// <summary>
        /// 唯一的业务编号，如订单编号、合同编号，产品SKU码
        /// </summary>
        public string BusinessNo { get; set; }

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
        /// 多文件模式下的上传结果列表
        /// </summary>
        public List<FileOperationResult<tb_FS_FileStorageInfo>> OperationResults { get; set; } = new List<FileOperationResult<tb_FS_FileStorageInfo>>();

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
    /// 文件删除请求 - 支持单文件和多文件删除
    /// 整合tb_FS_FileStorageInfo实体
    /// </summary>
    public class FileDeleteRequest : RequestBase
    {
        /// <summary>
        /// 是否为多文件删除
        /// </summary>
        public bool IsMultiFile { get; set; } = false;

        /// <summary>
        /// 文件存储信息（单文件模式）
        /// </summary>
        public tb_FS_FileStorageInfo FileStorageInfo { get; set; }

        /// <summary>
        /// 文件分类
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// 多文件模式下的文件存储信息列表
        /// </summary>
        public List<tb_FS_FileStorageInfo> FileStorageInfos { get; set; } = new List<tb_FS_FileStorageInfo>();

        /// <summary>
        /// 文件ID（单文件模式）- 向后兼容
        /// </summary>
        public string FileId
        {
            get => FileStorageInfo?.FileId.ToString();
            set
            {
                if (string.IsNullOrEmpty(value)) return;
                if (FileStorageInfo == null) FileStorageInfo = new tb_FS_FileStorageInfo();
                if (long.TryParse(value, out long fileId)) FileStorageInfo.FileId = fileId;
            }
        }

        /// <summary>
        /// 多文件模式下的文件ID列表（向后兼容）
        /// </summary>
        public List<string> FileIds
        {
            get => FileOperationCompatibilityHelper.ConvertStorageInfosToFileIds(FileStorageInfos);
            set
            {
                if (value != null)
                {
                    FileStorageInfos = FileOperationCompatibilityHelper.ConvertFileIdsToStorageInfos(value);
                }
            }
        }

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
    /// 文件删除响应 - 使用统一的ApiResponse模式
    /// 与FileDownloadResponse结构保持一致
    /// </summary>
    public class FileDeleteResponse : ResponseBase
    {
        /// <summary>
        /// 多文件模式下的删除结果列表
        /// </summary>
        public List<FileOperationResult<string>> OperationResults { get; set; } = new List<FileOperationResult<string>>();

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
            Timestamp = DateTime.Now;
        }

        /// <summary>
        /// 创建单文件成功结果
        /// </summary>
        public static FileDeleteResponse CreateSuccess(string message = "文件删除成功")
        {
            return new FileDeleteResponse(true, message, 200);
        }

        /// <summary>
        /// 创建多文件成功结果
        /// </summary>
        public static FileDeleteResponse CreateMultiFileSuccess(List<FileOperationResult<string>> results, string message = "文件删除成功")
        {
            var response = new FileDeleteResponse(true, message, 200);
            response.OperationResults = results;
            return response;
        }

        /// <summary>
        /// 创建失败结果
        /// </summary>
        public static FileDeleteResponse CreateFailure(string message, int code = 500)
        {
            return new FileDeleteResponse(false, message, code);
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
        /// 多文件模式下的查询结果列表
        /// </summary>
        public List<FileOperationResult<tb_FS_FileStorageInfo>> OperationResults { get; set; } = new List<FileOperationResult<tb_FS_FileStorageInfo>>();

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
        public string Category { get; set; }

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
        /// 将旧版多文件操作结果转换为新版通用结果类
        /// </summary>
        /// <typeparam name="T">结果数据类型</typeparam>
        /// <param name="targetId">目标标识</param>
        /// <param name="isSuccess">是否成功</param>
        /// <param name="data">结果数据</param>
        /// <param name="errorMessage">错误消息</param>
        /// <returns>通用文件操作结果</returns>
        public static FileOperationResult<T> CreateOperationResult<T>(string targetId, bool isSuccess, T data = default, string errorMessage = null)
        {
            return new FileOperationResult<T>
            {
                TargetId = targetId,
                IsSuccess = isSuccess,
                ErrorMessage = errorMessage
            };
        }

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

}
