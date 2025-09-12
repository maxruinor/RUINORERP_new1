using SuperSocket.ProtoBase;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Utils;
using System.Buffers;
using RUINORERP.PacketSpec.Protocol;

namespace RUINORERP.PacketSpec.Serialization
{
    /// <summary>
    /// 二进制包解码器
    /// </summary>
    public class BinaryPackageDecoder : IPackageDecoder<BizPackageInfo>
    {
        /// <summary>
        /// 解码二进制数据为包信息
        /// </summary>
        public BizPackageInfo Decode(ref ReadOnlySequence<byte> buffer, object context)
        {
            try
            {
                var reader = new SequenceReader<byte>(buffer);
                
                // 读取包长度（4字节小端序）
                if (!reader.TryReadLittleEndian(out int packageLength) || packageLength <= 0)
                {
                    return null;
                }

                // 检查是否有足够的数据
                if (reader.Remaining < packageLength)
                {
                    return null;
                }

                // 读取包数据
                var packageData = buffer.Slice(reader.Position, packageLength).ToArray();
                reader.Advance(packageLength);

                // 创建包信息对象
                return new BizPackageInfo
                {
                    Key = "binary_package",
                    Body = packageData,
                    PackageType = PackageType.Normal,
                    Timestamp = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                // 记录解码错误
                if (context is BasePipelineFilter<BizPackageInfo> filter)
                {
                    filter.LastError = $"解码错误: {ex.Message}";
                }
                return null;
            }
        }
    }
}