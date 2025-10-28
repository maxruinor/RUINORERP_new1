using Newtonsoft.Json;
using RUINORERP.PacketSpec.Models.Requests;
using System;

namespace RUINORERP.PacketSpec.Models.Requests.Message
{
    /// <summary>
    /// 消息请求 - 用于传递通用消息命令
    /// </summary>
    public class MessageRequest : RequestBase
    {
        /// <summary>
        /// 命令类型
        /// </summary>
        public MessageCommandType CommandType { get; set; }

        /// <summary>
        /// 命令数据
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public MessageRequest()
        { }


        public MessageRequest(object data)
        {
            Data = data;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="commandType">命令类型</param>
        /// <param name="data">命令数据</param>
        public MessageRequest(MessageCommandType commandType, object data)
        {
            CommandType = commandType;
            Data = data;
        }
    }


    public enum MessageCommandType
    {


        None = 0,

        /// <summary>
        /// 获取文件下载信息
        /// </summary>
        GetFileDownloadInfo = 1,

        /// <summary>
        /// 获取文件上传信息
        /// </summary>
        GetFileUploadInfo = 2,

        /// <summary>
        /// 获取文件信息
        /// </summary>
        GetFileInfo = 3,

        /// <summary>
        /// 删除文件
        /// </summary>
        DeleteFile = 4,

        /// <summary>
        /// 获取文件下载信息
        /// </summary>
        GetFileDownloadInfoByFileId = 5,

        /// <summary>
        /// 获取文件上传信息
    }

}
