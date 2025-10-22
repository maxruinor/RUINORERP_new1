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
            //int bodyLength = EncryptedProtocol.AnalyzeSeverPackHeader(head);
            int bodyLength = UnifiedEncryptionProtocol.AnalyzeServerPacketHeader(head);
            return bodyLength;
        }

        /// <summary>
        /// 解析网络数据包，提取业务信息
        /// </summary>
        /// <param name="bufferStream">包含数据包的缓冲流</param>
        /// <returns>解析后的业务包信息</returns>
        public override BizPackageInfo ResolvePackage(IBufferStream bufferStream)
        {
            byte[] packageContents = new byte[bufferStream.Length];
            bufferStream.Read(packageContents, 0, (int)bufferStream.Length);
            BizPackageInfo packageInfo = new BizPackageInfo();
            
            try
            {
                // 解密数据包
                var package = UnifiedEncryptionProtocol.DecryptServerPacket(packageContents);
                
                if (package.Two != null && package.Two.Length > 0)
                {
                    try
                    {
                        // 首先尝试使用MessagePack反序列化
                        packageInfo.Packet = UnifiedSerializationService.DeserializeWithMessagePack<PacketModel>(package.Two);
                    }
                    catch (Exception msgPackEx)
                    {
                        // MessagePack反序列化失败，尝试使用JSON作为备选方案
                        System.Diagnostics.Debug.WriteLine($"MessagePack反序列化失败，尝试JSON: {msgPackEx.Message}");
                        try
                        {
                            packageInfo.Packet = UnifiedSerializationService.DeserializeWithJson<PacketModel>(package.Two);
                        }
                        catch (Exception jsonEx)
                        {
                            // 如果两种反序列化都失败，记录详细错误
                            System.Diagnostics.Debug.WriteLine($"JSON反序列化也失败: {jsonEx.Message}");
                            // 可以在这里添加日志记录或通知机制
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // 记录解密或其他过程中的错误
                System.Diagnostics.Debug.WriteLine($"数据包处理过程出错: {ex.Message}");
                // 可以在这里添加更详细的日志记录
            }

            return packageInfo;
        }
    }
}