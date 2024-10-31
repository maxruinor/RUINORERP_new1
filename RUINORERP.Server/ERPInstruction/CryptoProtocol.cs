using System;
using System.Collections.Generic;
using System.Text;
using TransInstruction.DataPortal;

namespace TransInstruction
{
    /// <summary>
    /// 一个加密解密协议的工具类，所有的加密解密都通过这个类。解析的过程也通过这个类
    /// 注意：cmd,one,two。
    /// cmd：指令码   长度：一个字节（byte）的取值范围是 0 到 255 ，共 256 个不同的值。
    /// one：子指令码 长度：int 通常占用 4 个字节（32 位）。
    /// two：数据包   长度：int 通常占用 4 个字节（32 位）。在C#中，int 类型是一个32位有符号整数，其最大值是 2147483647。如果数据包长度超过了int最大值。则应该业务端要优化
    /// 
    /// 目前的包18位长度。需要优化保存包体长度。
    /// </summary>
    public static class CryptoProtocol
    {

        #region  服务端使用的方法
        /// <summary>
        /// 服务器端使用的加密方法
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="one"></param>
        /// <param name="two"></param>
        /// <returns></returns>
        public static EncryptedData EncryptionServerPackToClient(byte cmd, byte[] one, byte[] two)
        {
            OriginalData gd = new OriginalData();
            gd.cmd = cmd;
            gd.One = one;
            gd.Two = two;
            return EncryptionServerPackToClient(gd);
        }

        /// <summary>
        /// 服务器端使用的加密方法
        /// </summary>
        /// <param name="gd"></param>
        /// <returns></returns>
        public static EncryptedData EncryptionServerPackToClient(OriginalData gd)
        {
            EncryptedData Egd = new EncryptedData();

            //上面数据要在加密前处理。加密后就是乱码
            byte[] head = new byte[] { 0x00, 0x14, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x14 };
            head[0] = gd.cmd;
            int 申请数组长度 = 0;
            var KEY = new byte[256];

            if (gd.One == null)
            {
                gd.One = new byte[0];
            }
            if (gd.Two == null)
            {
                gd.Two = new byte[0];
            }
            申请数组长度 += gd.One.Length + gd.Two.Length;
            // 使用小端字节序存储长度
            Buffer.BlockCopy(BitConverter.GetBytes((uint)gd.One.Length), 0, head, 2, 4);
            Buffer.BlockCopy(BitConverter.GetBytes((uint)gd.Two.Length), 0, head, 6, 4);

            //head[2] = (byte)gd.One.Length;
            //head[3] = (byte)(gd.One.Length >> 8);

            //head[4] = (byte)gd.Two.Length;
            //head[5] = (byte)(gd.Two.Length >> 8);

            var arr = new ByteBuff(申请数组长度);
            Defer defer = new Defer();
            byte hi = 0;
            byte low = 0;
            for (int i = 0; i < 8; i++)
            {
                hi ^= head[i * 2];
                low ^= head[i * 2 + 1];
            }
            head[16] = hi;
            head[17] = low;

            加密(GetKey(m_日期KEY), head, 0, 0);//先加入加密的头部
            加密(GetKey(m_日期KEY), gd.One, 0, 0);//先加入加密的头部
            加密(GetKey(m_日期KEY), gd.Two, 0, 0);//先加入加密的头部
            lock (lock_tcp)
            {
                Egd.head = head;
                Egd.one = gd.One;
                Egd.two = gd.Two;
            }
            return Egd;
        }


        /// <summary>
        /// 服务器端使用：解决来自客户端的加密数据包的包头得到包体长度
        /// </summary>
        /// <param name="Head"></param>
        /// <returns></returns>
        public static int AnalyzeClientPackHeader(byte[] Head)
        {
            int HeaderLen = 18;
            //kx数据包，以18个头，通过头 解析 得到 第一部分长度和第二部分长度。


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
                return 256 - HeaderLen;
            }
            // 使用BitConverter.ToInt32来获取one的长度
            int lenOne = BitConverter.ToInt32(new byte[] { Head[2], Head[3], Head[4], Head[5] }, 0);

            //lenOne = (int)Head[3];
            //lenOne <<= 8;
            //lenOne |= (int)Head[2];

            // 使用BitConverter.ToInt32来获取two的长度
            //int lenTwo = BitConverter.ToInt32(, 0);
            //BitConverter.ToInt32(sz, 0);
            byte[] twobytes = new byte[] { Head[6], Head[7], Head[8], Head[9] };

            int lenTwo = BitConverter.ToInt32(twobytes, 0);

            //int lenTwo = (int)Head[5];
            //lenTwo <<= 8;
            //lenTwo |= (int)Head[4];

            return lenOne + lenTwo;
        }

        /// <summary>
        /// 服务器解来自客户端的包（SEP)
        /// </summary>
        /// <param name="Head"></param>
        /// <param name="HeaderLen"></param>
        /// <param name="alldata"></param>
        /// <returns></returns>
        public static KxData DecryptionClientPack(byte[] Head, int HeaderLen, byte[] alldata)
        {
            #region  解包体 包括包头重新解一次

            KxData ret = new KxData();
            int rxi = 0;
            //先把头解密了，后面的就好说了
            byte[] KEY = null;
            uint 掩码 = 0;
            var 解码 = false;
            int pi = 0; //已经解密到哪个点了
            for (int i = 0; i < 4; i++)
            {
                KEY = GetKey((i + m_日期KEY) % 4);
                掩码 = 加密(KEY, Head, 0, 0);

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
                throw new Exception($"非法字符串:{TransInstruction.Tool4DataProcess.Hex2Str(Head, 0, Head.Length, true)}");
            }

            ret.cmd = Head[0];

            // 使用BitConverter.ToInt32来获取one的长度
            int oneLength = BitConverter.ToInt32(new byte[] { Head[2], Head[3], Head[4], Head[5] }, 0);


            //int len = (int)Head[3];
            //len <<= 8;
            //len |= (int)Head[2];

            if (oneLength != 0)
            {
                ret.One = new byte[oneLength];
                Array.Copy(alldata, 18, ret.One, 0, oneLength);
                掩码 = 加密(KEY, ret.One, pi, 掩码);
                pi += oneLength;
            }

            //len = (int)Head[5];
            //len <<= 8;
            //len |= (int)Head[4];


            // 使用BitConverter.ToInt32来获取two的长度
            int twoLength = BitConverter.ToInt32(new byte[] { Head[6], Head[7], Head[8], Head[9] }, 0);
            if (twoLength != 0)
            {
                ret.Two = new byte[twoLength];
                if (ret.One == null)
                {
                    Array.Copy(alldata, 18, ret.Two, 0, twoLength);
                }
                else
                {

                    Array.Copy(alldata, 18 + ret.One.Length, ret.Two, 0, twoLength);
                }

                try
                {
                    掩码 = 加密(KEY, ret.Two, pi, 掩码);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
            }

            #endregion
            return ret;
        }

        #endregion


        #region 客户端使用的方法

        /// <summary>
        /// 解析服务器包的头部得到包体长度
        /// </summary>
        /// <param name="_Head"></param>
        /// <returns></returns>
        public static int AnalyzeSeverPackHeader(byte[] _Head)
        {
            byte[] Head = new byte[18];
            Array.Copy(_Head, Head, 18);
            int HeaderLen = 18;

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
                for (int ii = 0; ii < 8; ii++)
                {
                    hi ^= Head[ii * 2];
                    low ^= Head[ii * 2 + 1];
                }
                if (Head[16] == hi && Head[17] == low)
                {
                    m_日期KEY = i;
                    解码 = true;
                    pi += 18;
                    break;
                }
            }
            if (解码 == false)
            {
                //
                return 256 - HeaderLen;
            }

            // var outs = Tools.Hex2Str(Head, 0, 18, true);
            // ret.cmd = Head[0];
            // 使用BitConverter.ToInt32来获取one的长度
            int oneLength = BitConverter.ToInt32(new byte[] { Head[2], Head[3], Head[4], Head[5] }, 0);


            //    int lenOne = (int)Head[3];
            //lenOne <<= 8;
            //lenOne |= (int)Head[2];
            int twoLength = BitConverter.ToInt32(new byte[] { Head[6], Head[7], Head[8], Head[9] }, 0);
            //int lenTwo = (int)Head[5];
            //lenTwo <<= 8;
            //lenTwo |= (int)Head[4];

            return oneLength + twoLength;
        }

        /// <summary>
        /// 解密来自服务器的包
        /// </summary>
        /// <param name="Head"></param>
        /// <param name="HeaderLen"></param>
        /// <param name="alldata"></param>
        /// <returns></returns>
        public static OriginalData DecryptServerPack(byte[] PackData)
        {

            OriginalData ret = new OriginalData();

            try
            {
                #region  解包体 包括包头重新解一次
                byte[] Head = new byte[18];
                Array.Copy(PackData, Head, 18);

                int rxi = 0;
                //先把头解密了，后面的就好说了
                byte[] KEY = null;
                uint 掩码 = 0;
                var 解码 = false;
                int pi = 0; //已经解密到哪个点了
                for (int i = 0; i < 4; i++)
                {
                    KEY = GetKey((i + m_日期KEY) % 4);
                    掩码 = 加密(KEY, Head, 0, 0);

                    byte hi = 0, low = 0;
                    for (int ii = 0; ii < 8; ii++)
                    {
                        hi ^= Head[ii * 2];
                        low ^= Head[ii * 2 + 1];
                    }
                    if (Head[16] == hi && Head[17] == low)
                    {
                        m_日期KEY = i;
                        解码 = true;
                        pi += 18;
                        break;
                    }
                }
                if (解码 == false)
                {
                    //throw new Exception($"解码失败，非法字符串:{Tool4DataProcess.Hex2Str(Head, 0, Head.Length, true)}");
                }
                else
                {

                }

                // var outs = Tool4DataProcess.Hex2Str(Head, 0, rxi, true);
                ret.cmd = Head[0];
                // 使用BitConverter.ToInt32来获取one的长度
                int oneLength = BitConverter.ToInt32(new byte[] { Head[2], Head[3], Head[4], Head[5] }, 0);

                //len = (int)Head[3];
                //len <<= 8;
                //len |= (int)Head[2];
                rxi = 0;
                if (oneLength != 0)
                {
                    ret.One = new byte[oneLength];
                    Array.Copy(PackData, 18, ret.One, 0, oneLength);
                    掩码 = 加密(KEY, ret.One, pi, 掩码);
                    pi += oneLength;
                    // outs += Tool4DataProcess.Hex2Str(ret.One, 0, len, true);
                }
                int twoLength = BitConverter.ToInt32(new byte[] { Head[6], Head[7], Head[8], Head[9] }, 0);
                //len = (int)Head[5];
                //len <<= 8;
                //len |= (int)Head[4];

                rxi = 0;
                if (twoLength != 0)
                {
                    ret.Two = new byte[twoLength];
                    if (ret.One == null)
                    {
                        Array.Copy(PackData, 18, ret.Two, 0, twoLength);
                        //reader.Advance(HeaderLen);
                    }
                    else
                    {
                        // reader.Advance(HeaderLen + ret.One.Length);、//如果这里长度不够。可能数据包很大。分多次传过来的。
                        Array.Copy(PackData, 18 + ret.One.Length, ret.Two, 0, twoLength);
                    }
                    //reader.TryCopyTo(ret.Two);
                    try
                    {
                        KEY = GetKey(m_日期KEY);
                        掩码 = 加密(KEY, ret.Two, 0, 0);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.ToString());
                    }
                    //  outs += Tool4DataProcess.Hex2Str(ret.Two, 0, len, true);
                }
                #endregion
            }
            catch (Exception exxx)
            {
                //throw new Exception(string.Format("需要长度{0},提供的数据不够，不是完整的业务逻辑包。", len) + exxx.Message + exxx.StackTrace);
            }
            return ret;
        }


        /// <summary>
        /// 客户端将生成的数据打包加密码准备给服务器
        /// </summary>
        /// <param name="gd"></param>
        /// <returns></returns>
        public static byte[] EncryptClientPackToServer(OriginalData gd)
        {
        
            byte cmd = gd.cmd;
            byte[] one = gd.One;
            byte[] two = gd.Two;


            byte[] head = new byte[] { 0x00, 0x14, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x14 };
            head[0] = cmd;
            int 申请数组长度 = 0;
            var KEY = new byte[256];

            if (one == null)
            {
                one = new byte[0];
            }
            if (two == null)
            {
                two = new byte[0];
            }
            申请数组长度 += one.Length + two.Length;

            // 使用小端字节序存储长度
            Buffer.BlockCopy(BitConverter.GetBytes((uint)one.Length), 0, head, 2, 4);
            Buffer.BlockCopy(BitConverter.GetBytes((uint)two.Length), 0, head, 6, 4);

            //head[2] = (byte)one.Length;
            //head[3] = (byte)(one.Length >> 8);

            //head[4] = (byte)two.Length;
            //head[5] = (byte)(two.Length >> 8);

            var arr = new ByteBuff(申请数组长度);
            Defer defer = new Defer();
            byte hi = 0;
            byte low = 0;
            for (int i = 0; i < 8; i++)
            {
                hi ^= head[i * 2];
                low ^= head[i * 2 + 1];
            }
            head[16] = hi;
            head[17] = low;

            UInt32 掩码 = 0;
            KEY = GetKey(m_日期KEY);

            string stringKey = string.Empty;
            for (int i = 0; i < KEY.Length; i++)
            {
                stringKey += string.Format("{0:X2}", KEY[i]);
            }
            stringKey += "前面是key1  ";
            掩码 = 加密(KEY, head, 0, 0);//先加入加密的头部

            //key和掩码都会变化 。有关联。key是引用 类型


            for (int i = 0; i < KEY.Length; i++)
            {
                stringKey += string.Format("{0:X2}", KEY[i]);
            }
            stringKey += "  前面是key2  ";

            //掩码 = 加密(GetKey(m_日期KEY), one, head.Length, 0);//先加入加密的头部
            //掩码 = 加密(GetKey(m_日期KEY), two, 18, 0);//先加入加密的头部

            if (one != null && one.Length > 0)
            {
                掩码 = 加密(KEY, one, head.Length, 掩码);//先加入加密的头部
            }
            else
            {
                one = new byte[0];
            }

            if (two != null && two.Length > 0)
            {
                掩码 = 加密(KEY, two, one.Length + 18, 掩码);//先加入加密的头部
            }
            byte[] buffer= new byte[head.Length + one.Length + two.Length];
            Buffer.BlockCopy(head, 0, buffer, 0, head.Length);
            Buffer.BlockCopy(one, 0, buffer, head.Length, one.Length);
            Buffer.BlockCopy(two, 0, buffer, head.Length + one.Length, two.Length);
            return buffer; 
        }



        #endregion


        #region 加密解密
        public static int m_日期KEY;

        private static object lock_tcp = new object();

        public static byte[] StrToBytes(string val)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            if (val == null)
            {
                return new byte[0];
            }
            return System.Text.Encoding.GetEncoding("GB2312").GetBytes(val);
        }
        public static UInt32 加密(byte[] KEY, byte[] buff, int start, UInt32 掩码)  //加解密函数 调用这个即可 数组指针 长度
        {
            byte temp1, temp2, temp3, temp4;
            temp4 = (byte)掩码;
            掩码 >>= 8;
            temp3 = (byte)(掩码);
            掩码 >>= 8;
            temp2 = (byte)(掩码);
            掩码 >>= 8;
            temp1 = (byte)(掩码);
            for (int i = 0; i < buff.Length; i++)
            {
                byte pi = (byte)(i + 1 + start);
                temp1 = KEY[pi];
                temp3 = (byte)(temp1 + temp3);//保存变量
                temp2 = KEY[temp3];
                KEY[pi] = temp2;
                KEY[temp3] = temp1;

                temp2 = (byte)(temp1 + temp2);
                temp1 = KEY[temp2];
                temp4 = buff[i];

                temp4 = (byte)(temp4 ^ temp1);
                buff[i] = temp4;

            }
            掩码 = (UInt32)(temp1);
            掩码 <<= 8;
            掩码 |= (UInt32)(temp2);
            掩码 <<= 8;
            掩码 |= (UInt32)(temp3);
            掩码 <<= 8;
            掩码 |= (UInt32)(temp4);
            return 掩码;
        }
        //获取KEY

        static byte[] 昨天;
        static byte[] 今天;
        static byte[] 明天;
        static byte[] 不变;
        static int 日期 = 0;
        static object lock_日期 = new object();

        public static byte[] GetKey(int step)
        {
            var ret = new byte[256];
            if (日期 != DateTime.Now.Day)
            {
                lock (lock_日期)
                {
                    var tm = DateTime.Now;
                    tm = tm.AddDays(-1);
                    var s = tm.Year + "年" + tm.Month + "月" + tm.Day + "日";
                    昨天 = GetPassword(s);

                    tm = tm.AddDays(1);
                    s = tm.Year + "年" + tm.Month + "月" + tm.Day + "日";
                    今天 = GetPassword(s);

                    tm = tm.AddDays(1);
                    s = tm.Year + "年" + tm.Month + "月" + tm.Day + "日";
                    明天 = GetPassword(s);

                    s = "woaikaixuan"; //fmt.Sprintf("woaikaixuan", tm.Year(), tm.Month(), tm.Day())
                    不变 = GetPassword(s);
                    日期 = DateTime.Now.Day;
                }
            }
            switch (step)
            {
                case 0:
                    Array.Copy(不变, ret, 256);
                    break;
                case 1:
                    Array.Copy(今天, ret, 256);
                    break;
                case 2:
                    Array.Copy(昨天, ret, 256);
                    break;
                case 3:
                    Array.Copy(明天, ret, 256);
                    break;
            }
            return ret;
        }


        public static byte[] GetPassword(string s)
        {
            var ret = new byte[256];
            byte[] bys = StrToBytes(s);
            int tmlen = bys.Length;                                         //获取长度
            byte temp1, temp2, temp3, temp4;
            temp1 = temp2 = temp3 = temp4 = 0;

            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = (byte)i;
            }
            for (int i = 0; i < ret.Length; i++)
            {
                temp1 = bys[i % tmlen];   //秘钥
                temp2 = ret[i];                //key

                temp3 = (byte)(temp1 + temp3 + temp2); //计算偏移值
                temp4 = ret[temp3];//读取偏移后数据

                ret[i] = temp4;
                ret[temp3] = temp2;
            }
            return ret;
        }

        #endregion


    }
}
