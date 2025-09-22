using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Server.Network.Core
{
    namespace RUINORERP.PacketSpec.Server
    {
        public sealed class NetworkCommandMiddleware : MiddlewareBase<MySession>
        {
            private readonly ICommandDispatcher _dispatcher;
            private readonly IEncryptionService _crypto;

            public NetworkCommandMiddleware(ICommandDispatcher dispatcher,
                                          IEncryptionService crypto = null)
            {
                _dispatcher = dispatcher;
                _crypto = crypto ?? new EncryptedProtocolV2Adapter();
            }

            protected override async Task OnMessageReceived(MySession session, ReadOnlySequence<byte> data)
            {
                byte[] cipher = data.ToArray();
                var original = _crypto.DecryptionClientPack(cipher);

                var packet = UnifiedSerializationService.DeserializeWithMessagePack<PacketModel>(original.One);
                var command = CommandFactory.CreateCommand(packet); // 内部用 ICommandFactory

                var result = await _dispatcher.DispatchAsync(command);

                await SendResponseAsync(session, result, packet);
            }

            private async Task SendResponseAsync(MySession session, CommandResult result, PacketModel request)
            {
                var respPacket = PacketBuilder.Create()
                                              .WithCommand(request.Command) // 回相同命令码
                                              .WithDirection(PacketDirection.ServerToClient)
                                              .WithJsonData(result.Data)
                                              .WithRequestId(request.Extensions["RequestId"]?.ToString())
                                              .Build();

                byte[] plainBytes = UnifiedSerializationService.SerializeWithMessagePack(respPacket);
                
                // 将CommandId正确分解为Category和OperationCode
                byte category = (byte)request.Command.Category;
                byte operationCode = request.Command.OperationCode;
                
                var original = new OriginalData(category, new byte[] { operationCode }, plainBytes);
                byte[] cipher = _crypto.EncryptionServerPackToClient(original);

                await session.SendAsync(cipher);
            }
        }
    }
}
