using MessagePack;
using RUINORERP.PacketSpec.Models.Requests;
using System;

namespace RUINORERP.PacketSpec.Models.Requests.Message
{
    /// <summary>
    /// 消息请求 - 用于传递通用消息命令
    /// </summary>
    [MessagePackObject]
    public class MessageRequest : RequestBase
    {
        /// <summary>
        /// 命令类型
        /// </summary>
        [Key(10)]
        public uint CommandType { get; set; }

        /// <summary>
        /// 命令数据
        /// </summary>
        [Key(11)]
        public object Data { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public MessageRequest()
        {}

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="commandType">命令类型</param>
        /// <param name="data">命令数据</param>
        public MessageRequest(uint commandType, object data)
        {
            CommandType = commandType;
            Data = data;
        }
    }
}