using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Commands.Cache;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Requests.Cache;
using RUINORERP.PacketSpec.Models.Responses.Cache;
using System;

namespace RUINORERP.UI.Network.PacketAdapter
{
    /// <summary>
    /// CacheRequest -> CacheResponse 的 PacketModel 适配器
    /// 无业务逻辑，只做 DTO 与 PacketModel 互转
    /// </summary>
    public sealed class CachePacketAdapter : IPacketAdapter<CacheRequest, CacheResponse>
    {
        private static readonly CommandId CommandId = CacheCommands.CacheRequest;

        public PacketModel Pack(CacheRequest req, string clientId, string sessionId)
            => PacketBuilder.Create()
                            .WithCommand(CommandId)
                            .WithSession(sessionId, clientId)
                            .WithJsonData(req)
                            .Build();

        public CacheResponse Unpack(PacketModel packet)
            => packet.GetJsonData<CacheResponse>();
    }
}