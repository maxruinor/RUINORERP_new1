using RUINORERP.PacketSpec.Models.Core;
using System.Buffers;
using System;
using RUINORERP.PacketSpec.Core.DataProcessing;
using RUINORERP.PacketSpec.Models;

namespace RUINORERP.PacketSpec.Serialization
{
    /// <summary>
    /// 二进制包编码器
    /// </summary>
    public class BinaryPackageEncoder
    {
        /// <summary>
        /// 编码统一数据包为二进制数据
        /// </summary>
        /// <param name="writer">缓冲区写入器</param>
        /// <param name="packet">统一数据包</param>
        /// <returns>写入的字节数</returns>
        public int Encode(IBufferWriter<byte> writer, PacketModel packet)
        {
            if (packet == null || packet.Body == null)
            {
                return 0;
            }

            try
            {
                // 写入包长度前缀
                var packageLength = packet.Body.Length;
                var lengthBytes = ByteOperations.ToLittleEndianBytes(packageLength);
                
                // 写入包长度
                writer.Write(lengthBytes);

                // 写入包体数据
                writer.Write(packet.Body);

                return packageLength + 4; // 返回总写入字节数（包体 + 4字节长度前缀）
            }
            catch (Exception ex)
            {
                // 记录编码错误
                throw new InvalidOperationException($"统一数据包编码错误: {ex.Message}", ex);
            }
        }
    }

    /// <summary>
    /// 统一数据包二进制编码器
    /// </summary>
    public class UnifiedPacketEncoder
    {
        /// <summary>
        /// 编码统一数据包为二进制数据
        /// </summary>
        /// <param name="writer">缓冲区写入器</param>
        /// <param name="packet">统一数据包</param>
        /// <returns>写入的字节数</returns>
        public int Encode(IBufferWriter<byte> writer, PacketModel packet)
        {
            if (packet == null)
                return 0;

            try
            {
                var binaryData = UnifiedPacketSerializer.SerializeToBinary(packet);
                writer.Write(binaryData);
                return binaryData.Length;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"统一数据包编码错误: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 编码业务数据包为二进制数据
        /// </summary>
        /// <param name="writer">缓冲区写入器</param>
        /// <param name="packet">业务数据包</param>
        /// <returns>写入的字节数</returns>
        public int Encode(IBufferWriter<byte> writer, BusinessPacket packet)
        {
            if (packet == null)
                return 0;

            return Encode(writer, packet.UnifiedPacket);
        }
    }
}
