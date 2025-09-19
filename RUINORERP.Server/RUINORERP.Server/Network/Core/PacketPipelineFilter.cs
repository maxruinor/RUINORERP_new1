using System;
using System.Buffers;
using Microsoft.Extensions.Logging;
using SuperSocket.ProtoBase;
using RUINORERP.Server.Network.Models;
using RUINORERP.PacketSpec.Serialization;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Security;

namespace RUINORERP.Server.Network.Core
{
    /// <summary>
    /// 数据包管道过滤器 - 直接使用PacketSpec序列化器
    /// 简洁高效，无冗余代码
    /// by watson
    /// </summary>
    public class PacketPipelineFilter : FixedHeaderPipelineFilter<ServerPackageInfo>
    {
        private static readonly int HeaderLength = 18; // 与PacketSerializer保持一致

        // 无参数构造函数，用于SuperSocket的反射创建
        public PacketPipelineFilter() 
            : base(HeaderLength)
        {
        }


        /// <summary>
        /// 业务上通过包头18个里面的内容 解释出 还有多少len是一个完整的包。
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        protected override int GetBodyLengthFromHeader(ref ReadOnlySequence<byte> buffer)
        {
            //kx数据包，以18个头，通过头 解析 得到 第一部分长度和第二部分长度。
            
            byte[] head = new byte[HeaderLength];
            var reader = new SequenceReader<byte>(buffer);
            reader.TryCopyTo(head);
            
            // 直接调用UnifiedCryptographyService中的AnalyzeClientPackHeader方法解析包体长度
            // 该方法内部会完成头部解密和长度解析工作
            int bodyLength = UnifiedCryptographyService.Instance.AnalyzeClientPackHeader(head);
            
            return bodyLength;
        }

        /// <summary>
        /// 解码数据包
        /// </summary>
        protected override ServerPackageInfo DecodePackage(ref ReadOnlySequence<byte> buffer)
        {
            try
            {
 

                // 将缓冲区转换为字节数组
                var packageBytes = buffer.ToArray();
               // UnifiedCommunicationProcessor communicationProcessor = new UnifiedCommunicationProcessor();
             
              //  UnifiedCryptographyService.Instance.AnalyzeClientPackHeaderDetailed(head);

                // 使用PacketSpec的统一序列化器
                var packet = UnifiedPacketSerializer.DeserializeFromBinary(packageBytes);
                
                if (packet != null && packet.IsValid())
                {
 
                    // 创建并返回ServerPackageInfo对象
                    return new ServerPackageInfo
                    {
                        Command = packet.Command, // 假设Command是uint类型
                        Key = packet.PacketId,    // 使用PacketId作为Key
                        Body = packageBytes      // 保存原始数据包
                    };
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public override ServerPackageInfo Filter(ref SequenceReader<byte> reader)
        {
            return base.Filter(ref reader);
        }
    }
}