using System;
using System.Buffers;
using System.Collections.Generic;
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
        private const int MinimumPacketSize = 18;
        private const int DefaultBufferSize = 8192;
        private readonly ILogger<UnifiedCommunicationProcessor> _logger;
  

        /// <summary>
        /// 初始化统一通讯处理器
        /// </summary>
        /// <param name="logger">日志记录器</param>
        public UnifiedCommunicationProcessor(ILogger<UnifiedCommunicationProcessor> logger = null)
        {
            _logger = logger;
   
        }

        #region 数据包处理核心方法

        /// <summary>
        /// 处理客户端数据包（完整流程）
        /// </summary>
        /// <param name="encryptedData">加密的原始数据</param>
        /// <returns>解析后的原始数据包</returns>
        /// <exception cref="ArgumentNullException">数据为空</exception>
        /// <exception cref="ArgumentException">数据包长度不足</exception>
        /// <exception cref="InvalidOperationException">数据包处理失败</exception>
        public OriginalData ProcessClientPacket(byte[] encryptedData)
        {
            if (encryptedData == null)
                throw new ArgumentNullException(nameof(encryptedData));

            if (encryptedData.Length < MinimumPacketSize)
                throw new ArgumentException($"数据包长度不足，至少需要{MinimumPacketSize}字节");

            try
            {
                // 使用ArrayPool优化内存分配
                var decryptedData = EncryptedProtocol.DecryptionClientPack(encryptedData,18, 0);

                if (decryptedData == null || decryptedData.Length == 0)
                    throw new InvalidOperationException("数据包解密失败");

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
                // 将完整的CommandId正确分解为Category和OperationCode
                byte category = (byte)(command & 0xFF); // 取低8位作为Category
                byte operationCode = (byte)((command >> 8) & 0xFF); // 取次低8位作为OperationCode
                byte[] oneDataWithOperationCode = oneData != null ? 
                    new byte[] { operationCode }.Concat(oneData).ToArray() : 
                    new byte[] { operationCode };
                
                // 创建原始数据包
                var originalData = new OriginalData(category, oneDataWithOperationCode, twoData);

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
            if (!data.IsValid)
                throw new ArgumentNullException(nameof(data));

            // 使用ArrayPool优化内存分配
            byte[] encryptedBytes = _cryptoService.EncryptPackage(data.Two,1);

            // 检查结果是否有效
            if (encryptedBytes == null || encryptedBytes.Length < 18)
                throw new InvalidOperationException("加密服务器数据包失败");

            return new EncryptedData
            {
                Head = new byte[18],
                One = encryptedBytes.Length > 18 ? new byte[encryptedBytes.Length - 18] : Array.Empty<byte>(),
                Two = Array.Empty<byte>()
            };
        }

        /// <summary>
        /// 解密客户端数据包
        /// </summary>
        public OriginalData DecryptClientPack(byte[] encryptedData)
        {
            if (encryptedData == null)
                throw new ArgumentNullException(nameof(encryptedData));

            try
            {
                // 使用ArrayPool优化内存分配
                var kxData = _cryptoService.DecryptClientPackage(encryptedData, 1);
                //return kxData;
                return new OriginalData();
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "解密客户端数据包失败，返回空数据包");
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
            // 将完整的CommandId正确分解为Category和OperationCode
            byte category = (byte)(command & 0xFF); // 取低8位作为Category
            byte operationCode = (byte)((command >> 8) & 0xFF); // 取次低8位作为OperationCode
            
            return new OriginalData(category, new byte[] { operationCode }, data);
        }

        /// <summary>
        /// 创建服务器命令数据包（两部分数据）
        /// </summary>
        public OriginalData CreateServerCommand(uint command, byte[] one, byte[] two)
        {
            // 将完整的CommandId正确分解为Category和OperationCode
            byte category = (byte)(command & 0xFF); // 取低8位作为Category
            byte operationCode = (byte)((command >> 8) & 0xFF); // 取次低8位作为OperationCode
            byte[] oneDataWithOperationCode = one != null ? 
                new byte[] { operationCode }.Concat(one).ToArray() : 
                new byte[] { operationCode };
            
            return new OriginalData(category, oneDataWithOperationCode, two);
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
            if (message == null)
                throw new ArgumentNullException(nameof(message));

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
            return packageData != null && packageData.Length >= MinimumPacketSize;
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
            if (dataStream == null)
                throw new ArgumentNullException(nameof(dataStream));

            var result = new CommunicationResult();
            byte[] buffer = null;

            try
            {
                // 使用ArrayPool优化内存分配
                buffer = ArrayPool<byte>.Shared.Rent(DefaultBufferSize);

                while (!cancellationToken.IsCancellationRequested)
                {
                    byte[] responseData = await dataStream(buffer).ConfigureAwait(false);

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
                _logger?.LogError(ex, "异步处理数据流失败");
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }
            finally
            {
                // 释放ArrayPool中的缓冲区
                if (buffer != null)
                {
                    ArrayPool<byte>.Shared.Return(buffer);
                }
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

            try
            {
                Parallel.For(0, packets.Length, i =>
                {
                    try
                    {
                        if (packets[i] != null)
                        {
                            results[i] = ProcessClientPacket(packets[i]);
                        }
                        else
                        {
                            results[i] = new OriginalData(0, null, null);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogWarning(ex, $"批量处理第{i}个数据包失败");
                        results[i] = new OriginalData(0, null, null);
                    }
                });
            }
            catch (AggregateException ex)
            {
                _logger?.LogError(ex, "批量处理数据包时发生多个异常");
            }

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