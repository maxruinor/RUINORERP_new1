using Newtonsoft.Json;
using RUINORERP.Model.TransModel;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Responses;
using System;
using System.Data;

namespace RUINORERP.PacketSpec.Models.Messaging
{
    /// <summary>
    /// 消息响应
    /// </summary>
    public class MessageResponse : ResponseBase
    {
        /// <summary>
        /// 命令类型
        /// </summary>
        public MessageType CommandType { get; set; }

        /// <summary>
        /// 响应数据
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 构造函
        /// </summary>
        public MessageResponse()
        { }

        /// <summary>
        /// 创建成功响应
        /// </summary>
        /// <param name="commandType">命令类型</param>
        /// <param name="data">响应数据</param>
        /// <returns>成功的响应实例</returns>
        public static MessageResponse Success(MessageType commandType, object data)
        {
            return new MessageResponse
            {
                IsSuccess = true,
                ErrorCode = 0,
                Message = "消息处理成功",
                CommandType = commandType,
                Data = data
            };
        }

        /// <summary>
        /// 创建失败响应
        /// </summary>
        /// <param name="commandType">命令类型</param>
        /// <param name="errorCode">错误码</param>
        /// <param name="errorMessage">错误消息</param>
        /// <returns>失败的响应实</returns>
        public static MessageResponse Fail(MessageType commandType, string errorMessage)
        {
            return new MessageResponse
            {
                IsSuccess = false,
                ErrorMessage = errorMessage,
                Message = "消息处理失败",
                CommandType = commandType
            };
        }
    }

    /// <summary>
    /// 消息响应事件参数
    /// </summary>
    public class MessageResponseEventArgs : EventArgs
    {
        /// <summary>
        /// 会话ID
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// 命令ID
        /// </summary>
        public CommandId CommandId { get; set; }

        /// <summary>
        /// 响应数据
        /// </summary>
        public object ResponseData { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public DateTime Timestamp { get; set; }


    }

}
