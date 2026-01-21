using System;
using System.Collections.Generic;
using RUINORERP.Model;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Responses;

namespace RUINORERP.PacketSpec.Models.FileManagement
{
    /// <summary>
    /// 文件删除请求 - 支持单文件和多文件删除
    /// 删除的图片是来自一个单据下的文件
    /// </summary>
    public class FileDeleteRequest : RequestBase
    {
        /// <summary>
        /// 唯一的业务编号,如订单编号、合同编号,产品SKU码
        /// </summary>
        public string BusinessNo { get; set; }

        /// <summary>
        /// 业务主键ID (单据主表ID)
        /// 单表业务时使用此项,默认为主表
        /// </summary>
        public long? BusinessId { get; set; }
        /// <summary>
        /// 业务类型
        /// </summary>
        public int? BusinessType { get; set; }

        /// <summary>
        /// 多文件模式下的文件存储信息列表
        /// 有显示就查询出来了,如果是列表中删除则为空。去服务器来填充这个值
        /// </summary>
        public List<tb_FS_FileStorageInfo> FileStorageInfos { get; set; } = new List<tb_FS_FileStorageInfo>();

        /// <summary>
        /// 是否物理删除文件
        /// true: 物理删除(删除实际文件和文件元数据)
        /// false: 逻辑删除(仅标记文件状态为删除)
        /// 默认值:false(使用逻辑删除)
        /// </summary>
        public bool PhysicalDelete { get; set; } = false;

        /// <summary>
        /// 批量添加要删除的文件信息
        /// </summary>
        /// <param name="fileStorageInfos">要删除的文件存储信息列表</param>
        public void AddDeleteFileStorageInfo(List<tb_FS_FileStorageInfo> fileStorageInfos)
        {
            if (fileStorageInfos == null)
                return;

            foreach (var fileStorageInfo in fileStorageInfos)
            {
                AddDeleteFileStorageInfo(fileStorageInfo);
            }
        }

        /// <summary>
        /// 添加单个要删除的文件信息
        /// </summary>
        /// <param name="fileStorageInfo">要删除的文件存储信息</param>
        public void AddDeleteFileStorageInfo(tb_FS_FileStorageInfo fileStorageInfo)
        {
            if (fileStorageInfo == null)
                return;

            if (FileStorageInfos == null)
                FileStorageInfos = new List<tb_FS_FileStorageInfo>();

            // 跳过无效的文件ID
            if (fileStorageInfo.FileId <= 0)
                return;

            // 避免重复添加
            if (FileStorageInfos.Contains(fileStorageInfo))
                return;

            FileStorageInfos.Add(fileStorageInfo);
        }

        /// <summary>
        /// 初始化兼容数据结构
        /// 确保新旧API都能正常工作
        /// </summary>
        public void InitializeCompatibility()
        {
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
        /// 成功删除的文件ID列表
        /// </summary>
        public List<string> DeletedFileIds { get; set; } = new List<string>();

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public FileDeleteResponse() : base() { }

        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        /// <param name="success">是否成功</param>
        /// <param name="message">消息</param>
        /// <param name="code">状态码</param>
        public FileDeleteResponse(bool success, string message, int code = 200)
        {
            IsSuccess = success;
            Message = message;
            Timestamp = DateTime.Now;
        }

        /// <summary>
        /// 创建单文件删除成功结果
        /// </summary>
        /// <param name="message">成功消息</param>
        /// <returns>删除响应</returns>
        public static FileDeleteResponse CreateSuccess(string message = "文件删除成功")
        {
            return new FileDeleteResponse(true, message, 200);
        }

        /// <summary>
        /// 创建多文件删除成功结果
        /// </summary>
        /// <param name="deletedFileIds">成功删除的文件ID列表</param>
        /// <param name="message">成功消息</param>
        /// <returns>删除响应</returns>
        public static FileDeleteResponse CreateMultiFileSuccess(List<string> deletedFileIds, string message = "文件删除成功")
        {
            var response = new FileDeleteResponse(true, message, 200);
            response.DeletedFileIds = deletedFileIds ?? new List<string>();
            return response;
        }

        /// <summary>
        /// 创建失败结果
        /// </summary>
        /// <param name="message">失败消息</param>
        /// <param name="code">状态码</param>
        /// <returns>删除响应</returns>
        public static FileDeleteResponse CreateFailure(string message, int code = 500)
        {
            return new FileDeleteResponse(false, message, code);
        }
    }
}
