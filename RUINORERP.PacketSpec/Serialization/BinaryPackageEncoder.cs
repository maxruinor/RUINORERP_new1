using SuperSocket.ProtoBase;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Utils;
using System.Buffers;

namespace RUINORERP.PacketSpec.Serialization
{
    /// <summary>
    /// 二进制包编码器
    /// </summary>
    public class BinaryPackageEncoder : IPackageEncoder<BizPackageInfo>
    {
        /// <summary>
        /// 编码包信息为二进制数据
        /// </summary>
        public int Encode(IBufferWriter<byte> writer, BizPackageInfo package)
        {
            if (package == null || package.Body == null)
            {
                return 0;
            }

            try
            {
                // 写入包长度前缀
                var packageLength = package.Body.Length;
                var lengthBytes = EndianUtils.ToLittleEndianBytes(packageLength);
                
                // 写入包长度
                writer.Write(lengthBytes);

                // 写入包体数据
                writer.Write(package.Body);

                return packageLength + 4; // 返回总写入字节数（包体 + 4字节长度前缀）
            }
            catch (Exception ex)
            {
                // 记录编码错误
                throw new InvalidOperationException($"编码错误: {ex.Message}", ex);
            }
        }
    }
}