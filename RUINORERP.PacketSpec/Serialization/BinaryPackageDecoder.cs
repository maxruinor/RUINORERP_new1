using RUINORERP.PacketSpec.Models.Core;
using System.Buffers;
using RUINORERP.PacketSpec.Protocol;
using System;
using RUINORERP.PacketSpec.Models;
namespace RUINORERP.PacketSpec.Serialization
{
    /// <summary>
    /// 二进制包解码器
    /// </summary>
    public class BinaryPackageDecoder
    {
        /// <summary>
        /// 解码方法
        /// </summary>
        /// <param name="data">二进制数据</param>
        /// <param name="context">上下文</param>
        /// <returns>解码后的包信息</returns>
        public PacketModel Decode(ReadOnlySequence<byte> data, object context)
        {
            try
            {
                // 将ReadOnlySequence转换为字节数组进行处理
                byte[] bufferData = data.ToArray();
                
                // 检查数据长度
                if (bufferData.Length < 4)
                {
                    return null;
                }

                // 读取包长度（小端序）
                int packageLength = BitConverter.ToInt32(bufferData, 0);
                if (!BitConverter.IsLittleEndian)
                {
                    byte[] lengthBytes = new byte[4];
                    Array.Copy(bufferData, 0, lengthBytes, 0, 4);
                    Array.Reverse(lengthBytes);
                    packageLength = BitConverter.ToInt32(lengthBytes, 0);
                }
                
                if (packageLength <= 0)
                {
                    return null;
                }

                // 检查是否有足够的数据
                if (bufferData.Length < 4 + packageLength)
                {
                    return null;
                }

                // 读取包数据
                byte[] packageData = new byte[packageLength];
                Array.Copy(bufferData, 4, packageData, 0, packageLength);

                // 创建统一数据包对象
                return new PacketModel
                {
                    Body = packageData,
                    CreatedTime = DateTime.UtcNow,
                    LastUpdatedTime = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                // 记录解码错误（移除BasePipelineFilter引用）
                // 错误信息可以通过其他方式记录，如日志系统
                return null;
            }
        }
    }

    /// <summary>
    /// 统一数据包二进制解码器
    /// </summary>
    public class UnifiedPacketDecoder
    {
        /// <summary>
        /// 解码二进制数据为统一数据包
        /// </summary>
        /// <param name="buffer">数据缓冲区</param>
        /// <param name="context">上下文对象</param>
        /// <returns>统一数据包</returns>
        public PacketModel Decode(ref ReadOnlySequence<byte> buffer, object context)
        {
            try
            {
                byte[] data = buffer.ToArray();
                return UnifiedPacketSerializer.DeserializeFromBinary(data);
            }
            catch (Exception ex)
            {
                // 记录解码错误（移除BasePipelineFilter引用）
                // 错误信息可以通过其他方式记录，如日志系统
                return null;
            }
        }

        /// <summary>
        /// 使用MessagePack解码二进制数据为统一数据包
        /// </summary>
        /// <param name="buffer">数据缓冲区</param>
        /// <param name="context">上下文对象</param>
        /// <returns>统一数据包</returns>
        public PacketModel DecodeWithMessagePack(ref ReadOnlySequence<byte> buffer, object context)
        {
            try
            {
                byte[] data = buffer.ToArray();
                return UnifiedPacketSerializer.DeserializeFromBinaryWithMessagePack(data);
            }
            catch (Exception ex)
            {
                // 记录解码错误（移除BasePipelineFilter引用）
                // 错误信息可以通过其他方式记录，如日志系统
                return null;
            }
        }

        /// <summary>
        /// 解码二进制数据为业务数据包
        /// </summary>
        /// <param name="buffer">数据缓冲区</param>
        /// <param name="context">上下文对象</param>
        /// <returns>业务数据包</returns>
        public BusinessPacket DecodeBusinessPacket(ref ReadOnlySequence<byte> buffer, object context)
        {
            var unifiedPacket = Decode(ref buffer, context);
            return unifiedPacket != null ? BusinessPacket.FromUnifiedPacket(unifiedPacket) : null;
        }

        /// <summary>
        /// 使用MessagePack解码二进制数据为业务数据包
        /// </summary>
        /// <param name="buffer">数据缓冲区</param>
        /// <param name="context">上下文对象</param>
        /// <returns>业务数据包</returns>
        public BusinessPacket DecodeBusinessPacketWithMessagePack(ref ReadOnlySequence<byte> buffer, object context)
        {
            var unifiedPacket = DecodeWithMessagePack(ref buffer, context);
            return unifiedPacket != null ? BusinessPacket.FromUnifiedPacket(unifiedPacket) : null;
        }

        /// <summary>
        /// 安全解码（捕获异常返回null）
        /// </summary>
        /// <param name="buffer">数据缓冲区</param>
        /// <param name="context">上下文对象</param>
        /// <returns>统一数据包或null</returns>
        public PacketModel SafeDecode(ref ReadOnlySequence<byte> buffer, object context)
        {
            try
            {
                return Decode(ref buffer, context);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 使用MessagePack安全解码（捕获异常返回null）
        /// </summary>
        /// <param name="buffer">数据缓冲区</param>
        /// <param name="context">上下文对象</param>
        /// <returns>统一数据包或null</returns>
        public PacketModel SafeDecodeWithMessagePack(ref ReadOnlySequence<byte> buffer, object context)
        {
            try
            {
                return DecodeWithMessagePack(ref buffer, context);
            }
            catch
            {
                return null;
            }
        }
    }
}
