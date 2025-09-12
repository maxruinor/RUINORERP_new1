using SuperSocket.ProtoBase;
using System;
using System.Buffers;
using System.Text;
using RUINORERP.PacketSpec.Enums;
using RUINORERP.PacketSpec.Models;

namespace RUINORERP.PacketSpec.Protocol
{
    /// <summary>
    /// KX协议专用管道过滤器
    /// </summary>
    public class KxPipelineFilter : FixedHeaderPipelineFilter<BizPackageInfo>
    {
        private const int HeaderLength = 18;
        private int _dateKey;
        
        public KxPipelineFilter() : base(HeaderLength)
        {
        }
        
        /// <summary>
        /// 从包头获取包体长度
        /// </summary>
        protected override int GetBodyLengthFromHeader(ref ReadOnlySequence<byte> buffer)
        {
            try
            {
                byte[] header = new byte[HeaderLength];
                var reader = new SequenceReader<byte>(buffer);
                
                if (!reader.TryCopyTo(header))
                    return 256 - HeaderLength;
                
                // 使用KX协议处理器分析包头部
                return KxProtocolProcessor.AnalyzeClientPackHeader(header);
            }
            catch (Exception ex)
            {
                // 记录错误信息
                return 256 - HeaderLength;
            }
        }
        
        /// <summary>
        /// 解码数据包
        /// </summary>
        protected override BizPackageInfo DecodePackage(ref ReadOnlySequence<byte> buffer)
        {
            try
            {
                byte[] packageData = new byte[buffer.Length];
                var reader = new SequenceReader<byte>(buffer);
                reader.TryCopyTo(packageData);
                
                // 使用KX协议处理器解码包
                return KxProtocolProcessor.DecodeKxPackage(packageData);
            }
            catch (Exception ex)
            {
                // 返回解码错误的包信息
                return new BizPackageInfo 
                { 
                    Key = "KX",
                    SpecialOrder = SpecialOrder.DecodeError,
                    PackageType = PackageType.KxProtocol
                };
            }
        }
    }
}