using System;
using System.Buffers;
using Microsoft.Extensions.Logging;
using SuperSocket.ProtoBase;
using RUINORERP.Server.Network.Models;
using RUINORERP.PacketSpec.Serialization;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Security;
using RUINORERP.Model.ReminderModel.ReminderRules;
using System.Net.Sockets;
using RUINORERP.PacketSpec.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;
using System.IO;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualBasic;
using System.Drawing.Printing;
using RUINORERP.PacketSpec.Core.DataProcessing;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Common;

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
        
        // ⚠️ SuperSocket PipelineFilter通过反射创建，无法使用依赖注入
        // 这里使用静态LoggerFactory是唯一可行的方案
        private static readonly ILogger _logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<PacketPipelineFilter>();

        // 无参数构造函数，用于SuperSocket的反射创建
        public PacketPipelineFilter()
            : base(HeaderLength)
        {
        }

        /// <summary>
        /// 业务上通过包头18个里面的内容 解释出 还有多少len是一个完整的包。
        /// </summary>
        private void LogWarning(string message)
        {
            try { _logger?.LogWarning(message); } catch { }
        }
        
        /// <summary>
        /// 业务上通过包头18个里面的内容 解释出 还有多少len是一个完整的包。
        /// </summary>
        private void LogError(Exception ex, string message)
        {
            try { _logger?.LogError(ex, message); } catch { }
        }

        /// <summary>
        /// 业务上通过包头18个里面的内容 解释出 还有多少len是一个完整的包。
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        protected override int GetBodyLengthFromHeader(ref ReadOnlySequence<byte> buffer)
        {
            //kx数据包，以18个头，通过头 解析 得到 第一部分长度和第二部分长度。
            // 2. 从缓冲区读取原始包头（这是"干净的原始数据"，不做修改）
            byte[] originalHead = new byte[HeaderLength];
            var reader = new SequenceReader<byte>(buffer);
            // 读取18字节到originalHead（如果读取失败，需处理异常）
            if (!reader.TryCopyTo(originalHead) || originalHead.Length != HeaderLength)
            {
                throw new InvalidDataException("无法读取完整的包头数据");
            }

            int bodyLength = 0;
            byte[] headCopy2 = new byte[HeaderLength];
            Array.Copy(originalHead, headCopy2, HeaderLength);

            try
            {
                bodyLength = UnifiedEncryptionProtocol.AnalyzeClientPacketHeader(headCopy2);
                return bodyLength;  // 成功则立即返回
            }
            catch (Exception ex)
            {
                LogWarning("第一次尝试解析包头失败，尝试备用方法");
            }

            // 备用解析逻辑
            byte[] headCopy3 = (byte[])originalHead.Clone();

            try
            {
                bodyLength = UnifiedEncryptionProtocol.AnalyzeClientPacketHeader(headCopy3);
                return bodyLength;
            }
            catch (Exception ex)
            {
                LogError(ex, "所有包头解析方法均失败");
                throw new InvalidDataException("无法解析数据包头部", ex);
            }
        }

        /// <summary>
        /// 解码数据包,根据supersocket机制。这里的buffer的长度就是上面的bodyLength
        /// </summary>
        protected override ServerPackageInfo DecodePackage(ref ReadOnlySequence<byte> buffer)
        {
            try
            {
                // 将缓冲区转换为字节数组
                var packageBytes = buffer.ToArray();

                byte[] Head = new byte[HeaderLength];
                //取出18位包头长的数据
                Array.Copy(packageBytes, 0, Head, 0, HeaderLength);
                // 解密数据
                //var decryptedData = PacketSpec.Security.EncryptedProtocol.DecryptionClientPack(Head, HeaderLength, packageBytes);
                var decryptedData = PacketSpec.Security.UnifiedEncryptionProtocol.DecryptClientPacket(Head, packageBytes);
                // 反序列化数据包
                PacketModel packet;

                packet = JsonCompressionSerializationService.Deserialize<PacketModel>(decryptedData.Two);

                if (packet.IsValid())
                {

                    // 如果没有请求ID，或者对应的任务已完成，则视为服务器主动推送的命令
                    if (!packet.Extensions.ContainsKey("RequestId"))
                    {
                        // 服务器推送命令，正常处理
                    }
                    // 创建并返回ServerPackageInfo对象，包含所有必要的信息
                    return new ServerPackageInfo
                    {

                        Key = "SuperSocketCommandAdapter",
                        Packet = packet
                    };
                }
                else
                {
                    // 数据包无效，记录详细信息
                    LogWarning("接收到无效的数据包");
                    return null;
                }
            }
            catch (Exception ex)
            {
                // 记录详细的异常信息
                LogError(ex, "解码数据包时发生异常");
                // 记录异常但不抛出，避免影响其他处理
                return null;
            }
        }

        public override ServerPackageInfo Filter(ref SequenceReader<byte> reader)
        {
            return base.Filter(ref reader);
        }
    }
}