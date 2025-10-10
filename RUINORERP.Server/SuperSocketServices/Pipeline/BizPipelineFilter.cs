using SuperSocket.ProtoBase;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;
using RUINORERP.Server.Commands;

using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Security;
using RUINORERP.Server.Network.Core;

namespace RUINORERP.Server.ServerSession
{
    //https://www.cnblogs.com/springsnow/p/9544285.html#_label0_3
    //SuperSocket入门（五）-常用协议实现模版及FixedSizeReceiveFilter示例
    //https://docs.supersocket.net/v2-0/zh-CN/The-Built-in-Command-Line-PipelineFilter
    //https://blog.csdn.net/weixin_38083655/article/details/111467821  这个可以看一下
    
    /// <summary>
    /// ⚠️ [已过时] 业务数据包管道过滤器 - 旧架构实现
    /// 已由新架构 Network/Core/UnifiedPipelineFilter.cs 替代
    /// 
    /// 迁移说明:
    /// - 管道过滤已迁移到: RUINORERP.Server.Network.Core.UnifiedPipelineFilter
    /// - 数据包解析已迁移到: RUINORERP.PacketSpec.Serialization.UnifiedPacketSerializer
    /// - 协议处理已迁移到: RUINORERP.PacketSpec.Protocol.ProtocolVersion
    /// 
    /// 建议使用新架构进行开发，此过滤器将在未来版本中移除
    /// </summary>
    [Obsolete("此过滤器已过时，请使用 Network/Core/UnifiedPipelineFilter.cs 替代", false)]
    public class BizPipelineFilter : FixedHeaderPipelineFilter<BizPackageInfo>
    {
        static int HeaderLen = 18;
        public BizPipelineFilter() : base(HeaderLen) // 包头的大小是3字节，所以将3传如基类的构造方法中去
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

            //解包头
#pragma warning disable CS0219 // 变量已被赋值，但从未使用过它的值
            OriginalData kx = new OriginalData();
#pragma warning restore CS0219 // 变量已被赋值，但从未使用过它的值
            int lenOne = 0;
            //byte[] src = { 1, 2, 3, 4, 5 };
            //byte[] dest = new byte[src.Length];
            //Array.Copy(src, dest, src.Length);

            byte[] Head = new byte[HeaderLen];
            // Array.Copy(buffer, 0 , Head, 0, 18);
            //return BitConverter.ToInt32(Head, 0);

            //这个方法就从数据包开始的头部读取，计算出后面还多少数据流，才完。不然会一直等待。
            //所以这里要处理好。
            var reader = new SequenceReader<byte>(buffer);
            //reader.Advance(1); // skip the first byte
            reader.TryCopyTo(Head);

            int bodylen = UnifiedCryptographyService.Instance.AnalyzeClientPackHeader(Head);
            return bodylen;

            ////先把头解密了，后面的就好说了
            //byte[] KEY = null;
            //uint 掩码 = 0;
            //var 解码 = false;
            //int pi = 0; //已经解密到哪个点了
            //for (int i = 0; i < 4; i++)
            //{
            //    KEY = KxSocket.GetKey((i + m_日期KEY) % 4);
            //    掩码 = KxSocket.加密(KEY, Head, 0, 0);

            //    byte hi = 0, low = 0;
            //    for (int ii = 0; ii < 9; ii++)
            //    {
            //        hi ^= Head[ii * 2];
            //        low ^= Head[ii * 2 + 1];
            //    }
            //    if ((hi == 0) && (low == 0))
            //    {
            //        m_日期KEY = i;
            //        解码 = true;
            //        pi += 18;
            //        break;
            //    }
            //}
            //if (解码 == false)
            //{
            //    return 256 - HeaderLen;
            //    // throw new Exception($"非法字符串:{Tools.Hex2Str(Head, 0, Head.Length, true)}");
            //}

            //// var outs = Tools.Hex2Str(Head, 0, 18, true);
            //// ret.cmd = Head[0];
            //lenOne = (int)Head[3];
            //lenOne <<= 8;
            //lenOne |= (int)Head[2];

            //int lenTwo = (int)Head[5];
            //lenTwo <<= 8;
            //lenTwo |= (int)Head[4];

            //return lenOne + lenTwo;
        }


        public int m_日期KEY;

        public override BizPackageInfo Filter(ref SequenceReader<byte> reader)
        {
            return base.Filter(ref reader);
        }


        /*
        /// <summary>
        /// 解包  登陆 器 暂时 约定   数据+||\n  结尾一个请求。
        /// 在这里解这个包。是一个重点
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        protected override BizPackageInfo DecodePackage(ref ReadOnlySequence<byte> buffer)
        {
            BizPackageInfo gpi = new BizPackageInfo();
            try
            {
                //得到的是整个包
                byte[] PackageContents = new byte[buffer.Length];
                KxData kd = new KxData();


                var reader = new SequenceReader<byte>(buffer);
                reader.TryCopyTo(PackageContents);
                gpi.Body = PackageContents;
                //Key就是两种指令 在注册中 指定不同的处理类
                //连接后，服务器发送固定256后，就会收到一个256长度，内容会变化的值，暂时不知道规则
                //这里不是游戏。暂时不用这样
                //if (PackageContents.Length == 256)
                //{
                //    gpi.Key = "XT";
                //    //gpi.Key = "KXGame";
                //    gpi.ecode = SpecialOrder.固定256;
                //    return gpi;
                //}
                if (PackageContents.Length == 18)
                {
                    gpi.Key = "XT";
                    //gpi.Key = "KXGame";
                    gpi.ecode = LegacySpecialOrder.长度等于18;
                    return gpi;
                }
                if (PackageContents.Length < 18)
                {
                    gpi.Key = "XT";
                    //gpi.Key = "KXGame";
                    gpi.ecode = LegacySpecialOrder.长度小于18;
                    gpi.Flag = "空包";
                    gpi.Body = PackageContents;
                    kd.cmd = 1;//0可以算空包吗？
                    return gpi;
                }
                else
                {
                    try
                    {
                        gpi.Key = "KXGame";
                        #region  解包体 包括包头重新解一次
                        byte[] Head = new byte[HeaderLen];
                        // Array.Copy(buffer, 0 , Head, 0, 18);
                        //return BitConverter.ToInt32(Head, 0);
                        //reader.Advance(1); // skip the first byte
                        reader.TryCopyTo(Head);
                        // reader.TryReadBigEndian(out short packageKey);
                        //var outs = Tools.Hex2Str(Head, 0, 18, true);

                        gpi.Body = PackageContents;
                       gpi.kd= KxCryptographyService.Instance.DecryptClientPackage(;
                      //  gpi.kd = CryptoProtocol.DecryptDataPacket(Head, Head, PackageContents);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("服务器解来自客户端的包时出错" + ex.Message);
                        //frmMain.Instance._logger.LogError("服务器解来自客户端的包（SEP):" + ex.ToString());
                        gpi.kd = new KxData();
                    }
                    finally
                    {

                    }
                    gpi.ecode = LegacySpecialOrder.正常;
                    #endregion

                }
            }
            catch (Exception exx)
            {
                frmMain.Instance._logger.LogError("DecodePackage（SEP):" + exx.ToString());
            }
            return gpi;
        }
        */
    }
}
