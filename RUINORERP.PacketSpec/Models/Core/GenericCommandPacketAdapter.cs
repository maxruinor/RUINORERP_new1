using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Core;
using RUINORERP.PacketSpec.Models.Core;
using System;

namespace RUINORERP.PacketSpec.Models.Core
{
    /// <summary>
    /// 泛型数据包适配器 - 实现IPacketAdapter<TReq, TResp>接口
    /// 用于在业务DTO和传输层PacketModel之间进行转换
    /// </summary>
    /// <typeparam name="TReq">请求DTO类型</typeparam>
    /// <typeparam name="TResp">响应DTO类型</typeparam>
    public class GenericCommandPacketAdapter<TReq, TResp> : IPacketAdapter<TReq, TResp>
    {
        private readonly CommandId _commandId;
        private readonly CommandPacketAdapter _commandPacketAdapter;
        private readonly CommandDispatcher _commandDispatcher;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="commandId">命令标识符</param>
        /// <param name="commandPacketAdapter">命令数据包适配器实例</param>
        public GenericCommandPacketAdapter(CommandDispatcher commandDispatcher, CommandId commandId, CommandPacketAdapter commandPacketAdapter = null)
        {
            _commandDispatcher = commandDispatcher ?? throw new ArgumentNullException(nameof(commandDispatcher));
            _commandId = commandId;
            _commandPacketAdapter = commandPacketAdapter ?? new CommandPacketAdapter(commandDispatcher, new DefaultCommandFactory());
        }

        /// <summary>
        /// 将请求DTO转换为PacketModel
        /// </summary>
        /// <param name="request">请求DTO对象</param>
        /// <param name="clientId">客户端ID</param>
        /// <param name="sessionId">会话ID</param>
        /// <returns>转换后的PacketModel实例</returns>
        public PacketModel Pack(TReq request, string clientId, string sessionId)
        {
            // 创建一个包含请求数据的PacketModel
            var packet = PacketBuilder.Create()
                .WithJsonData(request)
                .WithCommand(_commandId)
                .Build();

            return packet;
        }

        /// <summary>
        /// 将PacketModel转换为响应DTO
        /// </summary>
        /// <param name="packet">接收到的PacketModel实例</param>
        /// <returns>转换后的响应DTO对象</returns>
        public TResp Unpack(PacketModel packet)
        {
            // 从PacketModel中提取并反序列化响应数据
            return packet.GetJsonData<TResp>();
        }
    }
}
