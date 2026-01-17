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
using System.Linq;


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

            Logger.Info($"GetBodyLengthFromHeader被调用，包头字节: {BitConverter.ToString(head)}");

            // 使用加密协议解析包头
            //int bodyLength = EncryptedProtocol.AnalyzeSeverPackHeader(head);
            int bodyLength = UnifiedEncryptionProtocol.AnalyzeServerPacketHeader(head);
            
            Logger.Info($"解析包体长度: {bodyLength}");
            
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

            Logger.Info($"ResolvePackage被调用，数据长度: {packageContents.Length}");
            Logger.Info($"数据包前50字节: {BitConverter.ToString(packageContents.Take(50).ToArray())}");
            
            try
            {
                // 记录解密前的数据包长度
                Logger.Debug($"开始解密服务器数据包，数据长度: {packageContents.Length}");
                
                // 解密数据包
                var package = UnifiedEncryptionProtocol.DecryptServerPacket(packageContents);
                
                // 记录解密结果
                if (!package.IsValid)
                {
                    Logger.Error("解密失败: 返回结果无效");
                }
                else
                {
                    Logger.Debug($"解密成功: Two字段长度 = {(package.Two?.Length ?? 0)}");
                }
                
                if (package.Two != null && package.Two.Length > 0)
                {   // 只有一种反序列化方式：JSON序列化
                    try
                    {   // 使用JsonCompressionSerializationService进行反序列化
                        packageInfo.Packet = JsonCompressionSerializationService.Deserialize<PacketModel>(package.Two);
                        
                        // 验证反序列化结果
                        if (packageInfo.Packet == null)
                        {
                            System.Diagnostics.Debug.WriteLine("反序列化成功，但结果为null");
                        }
                        else
                        {
                            if (packageInfo.Packet.CommandId!=SystemCommands.Heartbeat)
                            {

                            }
                        }
                    }
                    catch (Exception deserializationEx)  //重复登录时  服务器返回 增加错误信息的响应。这里捕获反序列化异常 ,  服务器断开。客户端要重连！！！
                    {   // 记录详细的反序列化错误信息
                        Logger.Error($"JSON反序列化失败: {deserializationEx.Message}");
                        
                        // 记录内部异常（如果有），这对于诊断类型解析问题非常重要
                        if (deserializationEx.InnerException != null)
                        {
                            Logger.Error($"内部异常: {deserializationEx.InnerException.Message}");
                        }
                        
                        // 记录堆栈跟踪，便于定位问题
                        Logger.Error($"堆栈跟踪: {deserializationEx.StackTrace}");
                    }
                }
            }
            catch (Exception ex)
            {
                // 记录解密或其他过程中的错误
                Logger.Error($"数据包处理过程出错: {ex.Message}");
                Logger.Error($"异常类型: {ex.GetType().Name}");
                
                // 记录内部异常（如果有）
                if (ex.InnerException != null)
                {
                    Logger.Error($"内部异常: {ex.InnerException.Message}");
                }
                
                // 记录原始数据包信息
                Logger.Error($"原始数据包长度: {packageContents?.Length ?? 0}");
                
                // 记录堆栈跟踪，便于定位问题
                Logger.Error($"堆栈跟踪: {ex.StackTrace}");
            }

            return packageInfo;
        }
    }
}