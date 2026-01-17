using SuperSocket.ProtoBase;
using System;
using System.Buffers;
using RUINORERP.PacketSpec.Security;
using RUINORERP.PacketSpec.Serialization;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Common;
using log4net;
using System.Reflection;


namespace RUINORERP.UI.Network
{
    /// <summary>
   
    /// </summary>
    public class BizPipelineFilter : FixedHeaderReceiveFilter<BizPackageInfo>
    {
        /// <summary>
        /// 日志记录器
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

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
                        packageInfo.Packet = JsonCompressionSerializationService.Deserialize<PacketModel>(package.Two);
                    }
                    catch (Exception deserializationEx)
                    {
                        Logger.Error($"JSON反序列化失败: {deserializationEx.Message}");
                        if (deserializationEx.InnerException != null)
                        {
                            Logger.Error($"内部异常: {deserializationEx.InnerException.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error($"数据包处理过程出错: {ex.Message}, 类型: {ex.GetType().Name}");
            }

            return packageInfo;
        }
    }
}