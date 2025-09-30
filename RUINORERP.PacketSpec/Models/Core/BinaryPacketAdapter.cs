using RUINORERP.PacketSpec.Commands;
using System;
using System.Buffers;

namespace RUINORERP.PacketSpec.Models.Core
{
    /// <summary>
    /// 二进制数据包适配器 - 提供基于二进制流的转换实现
    /// 适用于文件上传/下载等需要直接处理二进制数据的场景
    /// </summary>
    /// <typeparam name="TReq">请求DTO类型</typeparam>
    /// <typeparam name="TResp">响应DTO类型</typeparam>
    public class BinaryPacketAdapter<TReq, TResp> : IPacketAdapter<TReq, TResp>
    {
        private readonly CommandId _commandId;

        /// <summary>
        /// 初始化BinaryPacketAdapter实例
        /// </summary>
        /// <param name="commandId">命令ID</param>
        public BinaryPacketAdapter(CommandId commandId)
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
            // 检查请求对象是否为Memory<byte>类型
            if (request is Memory<byte> binaryData)
            {
                return PacketBuilder.Create()
                                    .WithCommand(_commandId)
                                    .WithSession(sessionId, clientId)
                                    .WithBinaryData(binaryData.ToArray())
                                    .Build();
            }
            
            // 如果请求对象不是Memory<byte>类型，则抛出异常
            throw new InvalidOperationException($"BinaryPacketAdapter only supports Memory<byte> as request type. Current type: {typeof(TReq)}");
        }

        /// <summary>
        /// 将PacketModel解包为响应DTO
        /// </summary>
        /// <param name="packet">接收到的PacketModel实例</param>
        /// <returns>转换后的响应DTO对象</returns>
        public TResp Unpack(PacketModel packet)
        {
            // 检查响应类型是否为Memory<byte>
            if (typeof(TResp) == typeof(Memory<byte>))
            {
                // 直接返回PacketModel的Data部分作为Memory<byte>
                var memory = new Memory<byte>(packet.Data ?? Array.Empty<byte>());
                return (TResp)(object)memory;
            }
            
            // 如果响应类型不是Memory<byte>类型，则抛出异常
            throw new InvalidOperationException($"BinaryPacketAdapter only supports Memory<byte> as response type. Current type: {typeof(TResp)}");
        }
    }
}
