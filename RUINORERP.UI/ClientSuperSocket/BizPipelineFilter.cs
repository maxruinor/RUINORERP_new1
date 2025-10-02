
using RUINORERP.PacketSpec.Models.Core;
using SuperSocket.ProtoBase;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;





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
            OriginalData ret = new OriginalData();
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

            int len =PacketSpec.Security.EncryptedProtocol.AnalyzeSeverPackHeader(Head);
            return len;
            try
            {

                //int intValue = 123456;
                //byte[] intBytes = BitConverter.GetBytes(intValue);

                //long longValue = 1234567890123456789;
                //byte[] longBytes = BitConverter.GetBytes(longValue);

                //OriginalData od = tpp.UnServerPack(Head);
            }
            catch (Exception)
            {

            }

           
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
                   // gpi.ecode = SpecialOrder.固定256;
                    return gpi;
                }
                if (PackageContents.Length == 18)
                {
                    gpi.Key = "XT";
                    //gpi.Key = "KXGame";
                  //  gpi.ecode = SpecialOrder.长度等于18;
                    return gpi;
                }
                if (PackageContents.Length < 18)
                {
                    gpi.Key = "XT";
                    //gpi.Key = "KXGame";
                   // gpi.ecode = SpecialOrder.长度小于18;
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


                    ///  TransPackProcess pp = new TransPackProcess();
                    gpi.Body = PackageContents;
                    if (PackageContents.Length > 338420)
                    {

                    }
                   // gpi.od = TransInstruction.CryptoProtocol.DecryptServerPack(gpi.Body);
                    //  gpi.od = pp.UnServerPack(gpi.Body);

                  //  gpi.ecode = SpecialOrder.正常;
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
