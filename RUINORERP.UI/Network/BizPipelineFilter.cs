using SuperSocket.ProtoBase;
using System;
using System.Buffers;
using RUINORERP.PacketSpec.Security;

namespace RUINORERP.UI.Network
{
    /// <summary>
    /// 业务管道过滤器
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

            BizPackageInfo packageInfo = new BizPackageInfo
            {
                Body = packageContents
            };

            try
            {
                // 如果是固定256字节的包
                if (packageContents.Length == 256)
                {
                    packageInfo.Key = "XT";
                    return packageInfo;
                }

                // 如果是18字节的包
                if (packageContents.Length == 18)
                {
                    packageInfo.Key = "XT";
                    return packageInfo;
                }

                // 如果是小于18字节的包
                if (packageContents.Length < 18)
                {
                    packageInfo.Key = "XT";
                    packageInfo.Flag = "空包";
                    return packageInfo;
                }
                else
                {
                    // 解密数据包
                    packageInfo.Key = "KXGame";
                    packageInfo.od = EncryptedProtocol.DecryptServerPack(packageInfo.Body);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"解析数据包时出错: {ex.Message}");
            }

            return packageInfo;
        }
    }
}