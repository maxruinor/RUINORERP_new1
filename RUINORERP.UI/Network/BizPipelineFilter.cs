using SuperSocket.ProtoBase;
using System;
using System.Buffers;
using RUINORERP.PacketSpec.Security;
using RUINORERP.PacketSpec.Serialization;
using RUINORERP.PacketSpec.Models.Core;

namespace RUINORERP.UI.Network
{
    /// <summary>
   
    /// </summary>
    public class BizPipelineFilter : FixedHeaderReceiveFilter<BizPackageInfo>
    {
        /// <summary>
        /// 业务上固定了包头的大小是18个字节
        /// </summary>
        private static readonly int HeaderLen = 18;

        /// <summary>
        /// 构造函数
        /// </summary>
        public BizPipelineFilter() : base(HeaderLen)
        {
        }

        /// <summary>
        /// 通过包头内容解析出包体长度
        /// </summary>
        /// <param name="bufferStream">缓冲区流</param>
        /// <param name="length">长度</param>
        /// <returns>包体长度</returns>
        protected override int GetBodyLengthFromHeader(IBufferStream bufferStream, int length)
        {
            byte[] head = new byte[HeaderLen];
            bufferStream.Read(head, 0, HeaderLen);

            // 使用加密协议解析包头
            int bodyLength = EncryptedProtocol.AnalyzeSeverPackHeader(head);
            return bodyLength;
        }

        /// <summary>
        /// 解析数据包
        /// </summary>
        /// <param name="bufferStream">缓冲区流</param>
        /// <returns>业务数据包信息</returns>
        public override BizPackageInfo ResolvePackage(IBufferStream bufferStream)
        {
            byte[] packageContents = new byte[bufferStream.Length];
            bufferStream.Read(packageContents, 0, (int)bufferStream.Length);
            BizPackageInfo packageInfo = new BizPackageInfo();
            try
            {
                var package = EncryptedProtocol.DecryptServerPack(packageContents);
                packageInfo.Packet = UnifiedSerializationService.DeserializeWithMessagePack<PacketModel>(package.Two);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"解析数据包时出错: {ex.Message}");
            }

            return packageInfo;
        }
    }
}