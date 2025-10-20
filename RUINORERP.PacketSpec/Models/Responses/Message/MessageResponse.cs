using MessagePack;
using RUINORERP.PacketSpec.Models.Responses;
using System;

namespace RUINORERP.PacketSpec.Models.Responses.Message
{
    /// <summary>
    /// 消息响应 - 表示消息命令的处理结果
    /// </summary>
    [MessagePackObject]
    public class MessageResponse : ResponseBase
    {
        /// <summary>
        /// 命令类型
        /// </summary>
        [Key(10)]
        public uint CommandType { get; set; }

        /// <summary>
        /// 响应数据
        /// </summary>
        [Key(11)]
        public object Data { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public MessageResponse()
        {}

        /// <summary>
        /// 创建成功响应
        /// </summary>
        /// <param name="commandType">命令类型</param>
        /// <param name="data">响应数据</param>
        /// <returns>成功的响应实例</returns>
        public static MessageResponse Success(uint commandType, object data)
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
        /// <returns>失败的响应实例</returns>
        public static MessageResponse Fail(uint commandType, int errorCode, string errorMessage)
        {
            return new MessageResponse
            {
                IsSuccess = false,
                ErrorCode = errorCode,
                ErrorMessage = errorMessage,
                Message = "消息处理失败",
                CommandType = commandType
            };
        }
    }
}