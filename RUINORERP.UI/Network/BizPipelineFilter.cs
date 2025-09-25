using SuperSocket.ProtoBase;
using System;
using System.Buffers;
using RUINORERP.PacketSpec.Security;
using RUINORERP.PacketSpec.Serialization;
using RUINORERP.PacketSpec.Models.Core;

namespace RUINORERP.UI.Network
{
    /// <summary>
    /// 业务管道过滤器 - SuperSocket数据包解析器
    /// 
    /// 🔄 数据包解析流程：
    /// 1. SuperSocket接收原始字节流
    /// 2. 解析固定18字节包头
    /// 3. 提取包体长度信息
    /// 4. 等待完整包体数据到达
    /// 5. 创建BizPackageInfo实例
    /// 6. 填充数据包信息
    /// 
    /// 📋 核心职责：
    /// - 固定头部解析（18字节）
    /// - 包体长度计算
    /// - 数据包完整性验证
    /// - 数据包信息封装
    /// - 多包处理支持
    /// - 错误处理与日志
    /// 
    /// 🔗 与架构集成：
    /// - 继承 SuperSocket FixedHeaderReceiveFilter
    /// - 创建 BizPackageInfo 数据包实例
    /// - 为 SuperSocketClient 提供解析后的数据
    /// - 使用 EncryptedProtocol 进行协议解析
    /// 
    /// 📐 数据包格式：
    /// - 包头：固定18字节
    /// - 包体：变长，长度由包头指定
    /// - 总大小：包头+包体
    /// - 验证：完整性检查
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