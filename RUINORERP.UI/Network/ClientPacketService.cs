using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.PacketSpec.Security;
using RUINORERP.PacketSpec.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.SessionState;

namespace RUINORERP.UI.Network
{
    public class ClientPacketService
    {
        private readonly ICommandFactory _commandFactory;
        private readonly ISocketClient _socketClient;

        public ClientPacketService(ICommandFactory commandFactory, ISocketClient socketClient)
        {
            _commandFactory = commandFactory;
            _socketClient = socketClient;
        }

        // 构建业务请求
        public async Task<ApiResponse<UserData>> GetUserDataAsync(long userId, bool includeProfile = false)
        {
            try
            {
                // 1. 创建命令
                var command = new GetUserDataCommand(userId, includeProfile);

                // 2. 转换为数据包
                var packet = PacketBuilder.Create()
                    .WithCommand(command.CommandIdentifier)
                    .WithJsonData(command.GetSerializableData())
                    .WithSession(SessionManager.CurrentSessionId)
                    .WithRequestId(Guid.NewGuid().ToString())
                    .Build();

                // 3. 序列化并加密
                var binaryData = UnifiedPacketSerializer.SerializeToBinary(packet);
                
                // 将CommandId正确分解为Category和OperationCode
                byte category = (byte)command.CommandIdentifier.Category;
                byte operationCode = command.CommandIdentifier.OperationCode;
                
                var encryptedData = EncryptedProtocolV2.EncryptClientPackToServer(
                    new OriginalData(category, new byte[] { operationCode }, binaryData));

                // 4. 发送到服务器
                var responseData = await _socketClient.SendAsync(encryptedData);

                // 5. 处理响应
                return ProcessResponse<UserData>(responseData);
            }
            catch (Exception ex)
            {
                return ApiResponse<UserData>.Failure($"获取用户数据失败: {ex.Message}");
            }
        }

        private ApiResponse<T> ProcessResponse<T>(byte[] responseData)
        {
            // 解密和反序列化响应
            var decryptedData = EncryptedProtocolV2.DecryptServerPack(responseData);
            var responsePacket = UnifiedPacketSerializer.DeserializeFromBinary<PacketModel>(decryptedData.Two);

            return responsePacket.GetJsonData<ApiResponse<T>>();
        }
    }
}
