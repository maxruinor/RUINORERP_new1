using SuperSocket.ProtoBase;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;
using TransInstruction;
using TransInstruction.DataPortal;


namespace RUINORERP.UI.SuperSocketClient
{
    //https://www.cnblogs.com/springsnow/p/9544285.html#_label0_3
    //SuperSocket入门（五）-常用协议实现模版及FixedSizeReceiveFilter示例
    //https://docs.supersocket.net/v2-0/zh-CN/The-Built-in-Command-Line-PipelineFilter
    //https://blog.csdn.net/weixin_38083655/article/details/111467821  这个可以看一下

    /// <summary>
    /// 过滤器
    /// </summary>
    public class BizPipelineFilter : FixedHeaderReceiveFilter<BizPackageInfo>
    {
        /// <summary>
        /// 业务上固定了包头的大小是18个字节
        /// </summary>
        static int HeaderLen = 18;
        public BizPipelineFilter() : base(HeaderLen) // 包头的大小是3字节，所以将3传如基类的构造方法中去
        {

        }

        /// <summary>
        /// 业务上通过包头18个里面的内容 解释出 还有多少len是一个完整的包。
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        protected override int GetBodyLengthFromHeader(IBufferStream bufferStream, int length)
        {
            //kx数据包，以18个头，通过头 解析 得到 第一部分长度和第二部分长度。

            //解包头
            KxData ret = new KxData();
            int lenOne = 0;
            //byte[] src = { 1, 2, 3, 4, 5 };
            //byte[] dest = new byte[src.Length];
            //Array.Copy(src, dest, src.Length);

            byte[] Head = new byte[HeaderLen];
            // Array.Copy(buffer, 0 , Head, 0, 18);
            //return BitConverter.ToInt32(Head, 0);

            //这个方法就从数据包开始的头部读取，计算出后面还多少数据流，才完。不然会一直等待。
            //所以这里要处理好。

            //2023-6-10
            bufferStream.Read(Head, 0, 18);

            // var reader = new SequenceReader<byte>(buffer);
            //reader.Advance(1); // skip the first byte
            // reader.TryCopyTo(Head);

            //先把头解密了，后面的就好说了
            byte[] KEY = null;
            uint 掩码 = 0;
            var 解码 = false;
            int pi = 0; //已经解密到哪个点了
            for (int i = 0; i < 4; i++)
            {
                KEY = KxSocket.GetKey((i + m_日期KEY) % 4);
                掩码 = KxSocket.加密(KEY, Head, 0, 0);

                byte hi = 0, low = 0;
                for (int ii = 0; ii < 9; ii++)
                {
                    hi ^= Head[ii * 2];
                    low ^= Head[ii * 2 + 1];
                }
                if ((hi == 0) && (low == 0))
                {
                    m_日期KEY = i;
                    解码 = true;
                    pi += 18;
                    break;
                }
            }
            if (解码 == false)
            {
                //这里用256是因为连接上 服务器就给了一个固定的256长的串
                return 256 - HeaderLen;
                // throw new Exception($"非法字符串:{Tools.Hex2Str(Head, 0, Head.Length, true)}");
            }

            // var outs = Tools.Hex2Str(Head, 0, 18, true);
            // ret.cmd = Head[0];
            lenOne = (int)Head[3];
            lenOne <<= 8;
            lenOne |= (int)Head[2];

            int lenTwo = (int)Head[5];
            lenTwo <<= 8;
            lenTwo |= (int)Head[4];

            return lenOne + lenTwo;
        }


        public int m_日期KEY;

        //public override BizPackageInfo Filter(ref SequenceReader<byte> reader)
        //{
        //    return base.Filter(ref reader);
        //}


        /// <summary>
        /// 解包  登陆 器 暂时 约定   数据+||\n  结尾一个请求。
        /// 在这里解这个包。是一个重点
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public override BizPackageInfo ResolvePackage(IBufferStream bufferStream)
        {

            //获取接收到的完整数据，包括头和尾
            //string body = bufferStream.ReadString((int)bufferStream.Length, Encoding.Default);
            //掐头去尾，只返回中间的数据
            //body = body.Substring(BeginMark.Length, body.Length - EndMark.Length - BeginMark.Length);
            //Skip(int count):从数据源跳过指定的字节个数。直接获取过滤后的数据
            // string body = bufferStream.Skip(BeginMark.Length).ReadString((int)bufferStream.Length - BeginMark.Length - EndMark.Length, Encoding.Default);
            // return new StringPackageInfo("", body, new string[] { });


            //得到的是整个包 为什么是固定256？
            byte[] PackageContents = new byte[bufferStream.Length];
            //2023-6-10
            bufferStream.Read(PackageContents, 0, (int)bufferStream.Length);
            //  var reader = new SequenceReader<byte>(buffer);
            // reader.TryCopyTo(PackageContents);
            BizPackageInfo gpi = new BizPackageInfo();
            gpi.Body = PackageContents;
            try
            {
                //Key就是两种指令 在注册中 指定不同的处理类
                //连接后，服务器发送固定256后，就会收到一个256长度，内容会变化的值，暂时不知道规则
                if (PackageContents.Length == 256)
                {
                    gpi.Key = "XT";
                    //gpi.Key = "KXGame";
                    gpi.ecode = SpecialOrder.固定256;
                    return gpi;
                }
                if (PackageContents.Length == 18)
                {
                    gpi.Key = "XT";
                    //gpi.Key = "KXGame";
                    gpi.ecode = SpecialOrder.长度等于18;
                    return gpi;
                }
                if (PackageContents.Length < 18)
                {
                    gpi.Key = "XT";
                    //gpi.Key = "KXGame";
                    gpi.ecode = SpecialOrder.长度小于18;
                    gpi.Flag = "空包";
                    gpi.Body = PackageContents;
                    //                    gpi..cmd = 1;//0可以算空包吗？
                    return gpi;
                }
                else
                {
                    gpi.Key = "KXGame";
                    #region  解包体 包括包头重新解一次
                    byte[] Head = new byte[HeaderLen];
                    // Array.Copy(buffer, 0 , Head, 0, 18);
                    //return BitConverter.ToInt32(Head, 0);
                    //reader.Advance(1); // skip the first byte
                    // reader.TryCopyTo(Head);
                    // reader.TryReadBigEndian(out short packageKey);
                    //var outs = Tools.Hex2Str(Head, 0, 18, true);
                    TransPackProcess pp = new TransPackProcess();
                    gpi.Body = PackageContents;
                    //gpi.od = pp.UnClientPack(gpi.Body);
                    gpi.od = pp.UnServerPack(gpi.Body);

                    //gpi.kd = pp.UnClientPack(Head, HeaderLen, PackageContents);
                    gpi.ecode = SpecialOrder.正常;
                    #endregion

                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.PrintInfoLog(ex.Message);
            }
            return gpi;
        }




    }
}
