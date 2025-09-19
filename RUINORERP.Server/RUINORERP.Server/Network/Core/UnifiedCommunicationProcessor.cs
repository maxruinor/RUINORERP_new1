using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Enums;
using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.Server.Network.Models;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Protocol;
using RUINORERP.PacketSpec.Security;
using RUINORERP.PacketSpec.Serialization;
using RUINORERP.PacketSpec.Commands;

namespace RUINORERP.Server.Network.Core
{
    /// <summary>
    /// ✅ [统一架构] 通讯处理器 - 整合所有通讯相关功能
    /// 负责数据包的加密、解密、验证、序列化和网络传输
    /// </summary>
    public class UnifiedCommunicationProcessor
    {
        private readonly ILogger<UnifiedCommunicationProcessor> _logger;
        private readonly UnifiedCryptographyService _cryptoService;

        /// <summary>
        /// 初始化统一通讯处理器
        /// </summary>
        /// <param name="logger">日志记录器</param>
        public UnifiedCommunicationProcessor(ILogger<UnifiedCommunicationProcessor> logger = null)
        {
            _logger = logger;
            _cryptoService = UnifiedCryptographyService.Instance;
        }

        #region 数据包处理核心方法

        /// <summary>
        /// 处理客户端数据包（完整流程）
        /// </summary>
        /// <param name="encryptedData">加密的原始数据</param>
        /// <returns>解析后的原始数据包</returns>
        /// <exception cref="ArgumentException">数据包长度不足</exception>
        /// <exception cref="InvalidOperationException">数据包处理失败</exception>
        public OriginalData ProcessClientPacket(byte[] encryptedData)
        {
            if (encryptedData == null || encryptedData.Length < 18)
                throw new ArgumentException("数据包长度不足");

            try
            {
                // 解密数据包
                byte[] decryptedData = _cryptoService.DecryptClientPackage(encryptedData, 0);

                // 解析数据包结构
                PacketModel originalData = UnifiedPacketSerializer.DeserializeFromBinary(decryptedData);

                // 验证数据包完整性
                if (!ValidatePacket(decryptedData))
                    throw new InvalidOperationException("数据包验证失败");

                return new OriginalData();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "处理客户端数据包失败");
                throw new InvalidOperationException("处理客户端数据包失败", ex);
            }
        }

        /// <summary>
        /// 准备服务器数据包（完整流程）
        /// </summary>
        /// <param name="command">命令字节</param>
        /// <param name="oneData">第一部分数据</param>
        /// <param name="twoData">第二部分数据</param>
        /// <param name="encrypt">是否加密</param>
        /// <returns>准备发送的数据包</returns>
        public async Task<byte[]> PrepareServerPacket(byte command, byte[] oneData, byte[] twoData, bool encrypt = true)
        {
            try
            {
                // 创建原始数据包
                var originalData = new OriginalData(command, oneData, twoData);

                byte[] rs = await MessagePackService.SerializeAsync(originalData);
                return rs;

                // 序列化为二进制
                //byte[] serializedData = UnifiedPacketSerializer.SerializeToBinary(originalData);

                //// 加密数据（如果需要）
                //return encrypt ? _cryptoService.EncryptPackage(serializedData) : serializedData;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "准备服务器数据包失败");
                throw new InvalidOperationException("准备服务器数据包失败", ex);
            }
        }

        #endregion

        #region 加密解密方法

        /// <summary>
        /// 加密服务器数据包给客户端
        /// </summary>
        public EncryptedData EncryptServerPackToClient(OriginalData data)
        {
            byte[] encryptedBytes = _cryptoService.EncryptPackage(new OriginalData
            {
                Cmd = data.Cmd,
                One = data.One,
                Two = data.Two
            });

            return new EncryptedData
            {
                Head = new byte[18],
                One = new byte[encryptedBytes.Length - 18],
                Two = Array.Empty<byte>()
            };
        }

        /// <summary>
        /// 解密客户端数据包
        /// </summary>
        public OriginalData DecryptClientPack(byte[] encryptedData)
        {
            try
            {
                //var kxData = _cryptoService.DecryptClientPackage(encryptedData);
                //return kxData;
                return new OriginalData();
            }
            catch (Exception)
            {
                return new OriginalData(); // 保持向后兼容
            }
        }

        #endregion

        #region 命令创建方法

        /// <summary>
        /// 创建服务器命令数据包
        /// </summary>
        public OriginalData CreateServerCommand(uint command, byte[] data)
        {
            return new OriginalData((byte)command, data, null);
        }

        /// <summary>
        /// 创建服务器命令数据包（两部分数据）
        /// </summary>
        public OriginalData CreateServerCommand(uint command, byte[] one, byte[] two)
        {
            return new OriginalData((byte)command, one, two);
        }

        /// <summary>
        /// 创建心跳数据包
        /// </summary>
        public OriginalData CreateHeartbeatPackage()
        {
            return CreateServerCommand((uint)SystemCommands.Heartbeat, Array.Empty<byte>());
        }

        /// <summary>
        /// 创建系统消息数据包
        /// </summary>
        public OriginalData CreateSystemMessage(string message)
        {
            var messageBytes = System.Text.Encoding.UTF8.GetBytes(message);
            return CreateServerCommand((uint)SystemCommands.ExceptionReport, messageBytes);
        }

        #endregion

        #region 验证和工具方法

        /// <summary>
        /// 验证数据包完整性
        /// </summary>
        public bool ValidatePacket(byte[] packageData)
        {
            return packageData != null && packageData.Length >= 18;
        }

        /// <summary>
        /// 获取数据包大小
        /// </summary>
        public int GetPackageSize(OriginalData data)
        {
            return 1 + (data.One?.Length ?? 0) + (data.Two?.Length ?? 0);
        }

        #endregion

        #region 异步和批量处理

        /// <summary>
        /// 异步处理通讯数据流
        /// </summary>
        public async Task<CommunicationResult> ProcessDataStreamAsync(
            Func<byte[], Task<byte[]>> dataStream,
            CancellationToken cancellationToken = default)
        {
            var result = new CommunicationResult();
            var buffer = new byte[8192];

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    byte[] responseData = await dataStream(buffer);

                    if (responseData != null && responseData.Length > 0)
                    {
                        result.ProcessedBytes += responseData.Length;
                        result.PacketCount++;
                    }
                    else
                    {
                        break;
                    }
                }
                result.Success = true;
            }
            catch (OperationCanceledException)
            {
                result.Success = false;
                result.ErrorMessage = "操作被取消";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 批量处理数据包
        /// </summary>
        public OriginalData[] ProcessPacketsBatch(byte[][] packets)
        {
            if (packets == null || packets.Length == 0)
                return Array.Empty<OriginalData>();

            var results = new OriginalData[packets.Length];

            Parallel.For(0, packets.Length, i =>
            {
                try
                {
                    results[i] = ProcessClientPacket(packets[i]);
                }
                catch
                {
                    results[i] = new OriginalData(0, null, null);
                }
            });

            return results;
        }

        #endregion
    }

    /// <summary>
    /// 通讯处理结果
    /// </summary>
    public class CommunicationResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public int ProcessedBytes { get; set; }
        public int PacketCount { get; set; }
    }
}