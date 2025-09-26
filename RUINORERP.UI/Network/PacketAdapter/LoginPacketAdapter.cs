using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Commands.Authentication;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network.PacketAdapter
{
    /// <summary>
    /// LoginRequest -> LoginResponse 的 PacketModel 适配器
    /// 无业务逻辑，只做 DTO 与 PacketModel 互转
    /// </summary>
    public sealed class LoginPacketAdapter : IPacketAdapter<LoginRequest, LoginResponse>
    {
        private static readonly CommandId CommandId = AuthenticationCommands.Login;

        public PacketModel Pack(LoginRequest req, string clientId, string sessionId)
            => PacketBuilder.Create()
                            .WithCommand(CommandId)
                            .WithSession(sessionId, clientId)
                            .WithJsonData(req)
                            .Build();

        public LoginResponse Unpack(PacketModel packet)
            => packet.GetJsonData<LoginResponse>();
    }
}

