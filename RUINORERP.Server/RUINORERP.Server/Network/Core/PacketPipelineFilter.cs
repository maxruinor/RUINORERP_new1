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
using Fireasy.Common.ComponentModel;

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
        int datakey = 0;

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
                bodyLength = EncryptedProtocol.AnalyzeClientPackHeader(headCopy2);
            }
            catch (Exception)
            {


            }


            // 3. 为第一个解密方法创建副本1
            byte[] headCopy3 = (byte[])originalHead.Clone();
            int dataKey3 = 0;

            try
            {
                int bodyLength3 = PacketSpec.Security.EncryptedProtocolV2.AnalyzeClientPackHeader(headCopy3);
            }
            catch (Exception)
            {


            }
            return bodyLength;
        }









        //54才是对的



        /// <summary>
        /// 解码数据包
        /// </summary>
        protected override ServerPackageInfo DecodePackage(ref ReadOnlySequence<byte> buffer)
        {
            /*
            #region
            try
            {
                // 将缓冲区转换为字节数组
                var packageBytes = buffer.ToArray();

                // 使用PacketSpec的统一序列化器
                var packet = UnifiedPacketSerializer.DeserializeFromBinary(packageBytes);

                if (packet != null && packet.IsValid())
                {
                    // 创建并返回ServerPackageInfo对象
                    return new ServerPackageInfo
                    {
                        Command = packet.Command,
                        Key = packet.PacketId,
                        Body = packageBytes
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

            #endregion
            */
            try
            {
                // 将缓冲区转换为字节数组
                var packageBytes = buffer.ToArray();

                byte[] Head = new byte[HeaderLength];
                //取出18位包头长的数据
                Array.Copy(packageBytes, 0, Head, 0, HeaderLength);


                TestDecode_deep(Head, packageBytes);


                var kxData = PacketSpec.Security.EncryptedProtocol.DecryptionClientPack(Head, HeaderLength, packageBytes);
                //ReadOnlySpan<byte> span = Head.AsSpan();
                //ReadOnlySpan<byte> body = packageBytes.AsSpan();
                int index = 0;
                string sendTime2 = ByteOperations.GetString(kxData.Two, ref index);
                string u2 = ByteOperations.GetString(kxData.Two, ref index);
                string p2 = ByteOperations.GetString(kxData.Two, ref index);
                //if (kxData.Cmd == )
                //{

                //}

                //return new ServerPackageInfo
                //{
                //    //Command = kxData.Cmd, // 假设Command是uint类型
                //    Key = "SimplifiedSocketCommand",    // 使用PacketId作为Key
                //    Body = packageBytes      // 保存原始数据包
                //};


                return new ServerPackageInfo
                {
                    //Command = kxData.Cmd, // 假设Command是uint类型
                    Key = "SuperSocketCommandAdapter",    // 使用PacketId作为Key
                    Body = packageBytes      // 保存原始数据包
                };


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




     

        private void TestDecode_deep(byte[] head, byte[] alldata)
        {
            try
            {

                byte[] headCopy = (byte[])head.Clone();
                byte[] alldataCopy = (byte[])alldata.Clone();
                var kxData_v2 = PacketSpec.Security.EncryptedProtocolV2.DecryptionClientPack(headCopy, alldataCopy);
                int index = 0;
                string sendTime2 = ByteOperations.GetString(kxData_v2.Two, ref index);
                string u2 = ByteOperations.GetString(kxData_v2.Two, ref index);
                string p2 = ByteOperations.GetString(kxData_v2.Two, ref index);
            }
            catch (Exception ex)
            {

            }
        }





        public override ServerPackageInfo Filter(ref SequenceReader<byte> reader)
        {
            return base.Filter(ref reader);
        }
    }
}