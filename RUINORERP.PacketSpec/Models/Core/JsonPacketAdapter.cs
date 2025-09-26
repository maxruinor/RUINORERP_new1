using RUINORERP.PacketSpec.Commands;
using System.Text;
using Newtonsoft.Json;

namespace RUINORERP.PacketSpec.Models.Core
{
    /// <summary>
    /// JSON数据包适配器 - 提供基于JSON的默认转换实现
    /// 适用于90%的标准场景，将请求DTO转换为PacketModel并反之
    /// </summary>
    /// <typeparam name="TReq">请求DTO类型</typeparam>
    /// <typeparam name="TResp">响应DTO类型</typeparam>
    public class JsonPacketAdapter<TReq, TResp> : IPacketAdapter<TReq, TResp>
    {
        private readonly CommandId _commandId;

        /// <summary>
        /// 初始化JsonPacketAdapter实例
        /// </summary>
        /// <param name="commandId">命令ID</param>
        public JsonPacketAdapter(CommandId commandId)
        {
            _commandId = commandId;
        }

        /// <summary>
        /// 将请求DTO打包为PacketModel
        /// </summary>
        /// <param name="request">请求DTO对象</param>
        /// <param name="clientId">客户端ID</param>
        /// <param name="sessionId">会话ID</param>
        /// <returns>转换后的PacketModel实例</returns>
        public PacketModel Pack(TReq request, string clientId, string sessionId)
        {
            return PacketBuilder.Create()
                                .WithCommand(_commandId)
                                .WithSession(sessionId, clientId)
                                .WithJsonData(request)
                                .Build();
        }

        /// <summary>
        /// 将PacketModel解包为响应DTO
        /// </summary>
        /// <param name="packet">接收到的PacketModel实例</param>
        /// <returns>转换后的响应DTO对象</returns>
        public TResp Unpack(PacketModel packet)
        {
            return packet.GetJsonData<TResp>();
        }
    }
}