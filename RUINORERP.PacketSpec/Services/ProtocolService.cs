using System;
using RUINORERP.PacketSpec.Enums;
using RUINORERP.PacketSpec.Protocol;
using RUINORERP.PacketSpec.Security;
using RUINORERP.PacketSpec.Utilities;

namespace RUINORERP.PacketSpec.Services
{
    /// <summary>
    /// 综合协议处理服务
    /// </summary>
    public class ProtocolService
    {
        private readonly ICommandService _commandService;
        private readonly ProtocolProcessor _protocolProcessor;

        public ProtocolService()
        {
            _commandService = new CommandService();
            _protocolProcessor = new ProtocolProcessor();
        }

        public ProtocolService(ICommandService commandService)
        {
            _commandService = commandService;
            _protocolProcessor = new ProtocolProcessor();
        }

        /// <summary>
        /// 处理接收到的数据包
        /// </summary>
        public string ProcessReceivedPacket(byte[] rawData, bool isEncrypted = false)
        {
            try
            {
                // 解密数据（如果需要）
                byte[] processedData = isEncrypted 
                    ? CryptoProtocol.DecryptServerPack(rawData).ToByteArray()
                    : rawData;

                // 解析数据包
                var originalData = PacketProcessor.UnpackClientData(processedData);

                // 验证数据包格式
                if (!PacketAnalyzer.ValidatePacketFormat(originalData))
                {
                    return "Invalid packet format";
                }

                // 分析指令类型
                var (sourceType, message) = ProtocolProcessor.CheckCommandType(originalData.Cmd);

                string result = message;

                // 根据指令来源类型处理
                if (sourceType == PackageSourceType.Client)
                {
                    var clientCommand = ProtocolProcessor.GetClientCommand(originalData);
                    result += _commandService.ProcessClientCommand(clientCommand, originalData);
                }
                else
                {
                    var serverCommand = ProtocolProcessor.GetServerCommand(originalData);
                    result += _commandService.ProcessServerCommand(serverCommand, originalData);
                }

                // 添加数据包分析信息
                result += "\n" + PacketAnalyzer.AnalyzePacketStructure(originalData);

                return result;
            }
            catch (Exception ex)
            {
                return $"Error processing packet: {ex.Message}";
            }
        }

        /// <summary>
        /// 创建响应数据包
        /// </summary>
        public byte[] CreateResponsePacket(ServerCommand command, byte[] responseData, bool encrypt = true)
        {
            var originalData = new OriginalData(
                cmd: (byte)command,
                one: responseData,
                two: null
            );

            byte[] packedData = PacketProcessor.PackServerData(originalData);

            return encrypt 
                ? CryptoProtocol.EncryptClientToServer(originalData) 
                : packedData;
        }

        /// <summary>
        /// 创建客户端请求数据包
        /// </summary>
        public byte[] CreateClientRequestPacket(ClientCommand command, byte[] requestData, bool encrypt = true)
        {
            var originalData = new OriginalData(
                cmd: (byte)command,
                one: requestData,
                two: null
            );

            byte[] packedData = PacketProcessor.PackClientData(originalData);

            return encrypt 
                ? CryptoProtocol.EncryptClientToServer(originalData) 
                : packedData;
        }

        /// <summary>
        /// 注册自定义指令处理器
        /// </summary>
        public void RegisterClientCommandHandler(ClientCommand command, ClientCommandHandler handler)
        {
            _commandService.RegisterClientHandler(command, handler);
        }

        /// <summary>
        /// 注册自定义指令处理器
        /// </summary>
        public void RegisterServerCommandHandler(ServerCommand command, ServerCommandHandler handler)
        {
            _commandService.RegisterServerHandler(command, handler);
        }

        /// <summary>
        /// 获取指令描述信息
        /// </summary>
        public string GetCommandDescription(uint command)
        {
            return ProtocolProcessor.GetCommandDescription(command);
        }

        /// <summary>
        /// 验证指令是否有效
        /// </summary>
        public bool IsValidCommand(uint command)
        {
            return ProtocolProcessor.IsValidCommand(command);
        }

        /// <summary>
        /// 分析数据包内容
        /// </summary>
        public string AnalyzePacket(byte[] data)
        {
            try
            {
                var originalData = PacketProcessor.UnpackClientData(data);
                return PacketAnalyzer.AnalyzePacketStructure(originalData);
            }
            catch (Exception ex)
            {
                return $"Error analyzing packet: {ex.Message}";
            }
        }

        /// <summary>
        /// 设置加密密钥
        /// </summary>
        public void SetEncryptionKeys(byte[] key, byte[] iv)
        {
            CryptoProtocol.SetEncryptionKeys(key, iv);
        }
    }

    /// <summary>
    /// OriginalData扩展方法
    /// </summary>
    public static class OriginalDataExtensions
    {
        /// <summary>
        /// 将OriginalData转换为字节数组
        /// </summary>
        public static byte[] ToByteArray(this OriginalData data)
        {
            return PacketProcessor.PackClientData(data);
        }

        /// <summary>
        /// 从字节数组创建OriginalData
        /// </summary>
        public static OriginalData FromByteArray(byte[] data)
        {
            return PacketProcessor.UnpackClientData(data);
        }

        /// <summary>
        /// 获取数据包大小信息
        /// </summary>
        public static (int TotalSize, int OneSize, int TwoSize) GetSizeInfo(this OriginalData data)
        {
            return PacketAnalyzer.GetPacketSizeInfo(data);
        }

        /// <summary>
        /// 提取文本内容
        /// </summary>
        public static Dictionary<int, string> ExtractText(this OriginalData data)
        {
            return PacketAnalyzer.ExtractTextContent(data);
        }
    }
}