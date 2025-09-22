using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Serialization;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network
{
         public sealed class PacketClient : IDisposable
        {
            private readonly ISession _session;
            private readonly IEncryptionService _crypto;
            private readonly ConcurrentDictionary<string, TaskCompletionSource<byte[]>>
                _pending = new();

            public PacketClient(ISession session, IEncryptionService crypto = null)
            {
                _session = session;
                _crypto = crypto ?? new EncryptedProtocolV2Adapter();
                session.Received += OnReceived;
            }

            public Task<TResponse> SendAsync<TRequest, TResponse>(TRequest request,
                                                                  CommandId command,
                                                                  CancellationToken ct = default)
            {
                var packet = PacketBuilder.Create()
                                          .WithCommand(command)
                                          .WithJsonData(request)
                                          .WithRequestId(Guid.NewGuid().ToString("N"))
                                          .Build();

                byte[] payload = UnifiedSerializationService.SerializeWithMessagePack(packet);
                
                // 将CommandId正确分解为Category和OperationCode
                byte category = (byte)command.Category;
                byte operationCode = command.OperationCode;
                
                var original = new OriginalData(category, new byte[] { operationCode }, payload);
                byte[] cipher = _crypto.EncryptClientPackToServer(original);

                var tcs = new TaskCompletionSource<byte[]>(TaskCreationOptions.RunContinuationsAsynchronously);
                _pending.TryAdd(packet.Extensions["RequestId"].ToString(), tcs);

                _session.Send(cipher, 0, cipher.Length);

                return tcs.Task.ContinueWith(t =>
                {
                    var respPacket = UnifiedSerializationService.DeserializeWithMessagePack<PacketModel>(t.Result);
                    return respPacket.GetJsonData<TResponse>();
                }, ct);
            }

            private void OnReceived(byte[] data)
            {
                var plain = _crypto.DecryptServerPack(data);
                var packet = UnifiedSerializationService.DeserializeWithMessagePack<PacketModel>(plain.One);
                if (packet.Extensions.TryGetValue("RequestId", out var rid) &&
                    _pending.TryRemove(rid.ToString(), out var tcs))
                {
                    tcs.TrySetResult(plain.One);
                }
            }

            public void Dispose() => _session.Received -= OnReceived;
        }
    }

