using System;
using TransInstruction.DataPortal;

namespace TransInstruction
{
    /// <summary>
    /// 包的加密解密
    /// </summary>
    public class TransPackProcess
    {


        /// <summary>
        /// 服务器的加密方法，加密后发送到客户端
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="one"></param>
        /// <param name="two"></param>
        /// <returns></returns>
        public EncryptedData EncryptedDataPack(byte cmd, byte[] one, byte[] two)
        {
            EncryptedData Egd = new EncryptedData();
            TransInstruction.TransProtocol tester = new TransInstruction.TransProtocol();
            TransInstruction.OriginalData gd = new TransInstruction.OriginalData();
            gd.cmd = cmd;
            gd.One = one;
            gd.Two = two;

            //上面数据要在加密前处理。加密后就是乱码
            bool Debug = false;

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

            head[2] = (byte)one.Length;
            head[3] = (byte)(one.Length >> 8);

            head[4] = (byte)two.Length;
            head[5] = (byte)(two.Length >> 8);
            var outs = "";
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
            if (Debug == true)
            {
                outs += Tool4DataProcess.Hex2Str(head, 0, head.Length, true);
                if (one.Length != 0)
                {
                    outs += Tool4DataProcess.Hex2Str(one, 0, one.Length, true);
                }
                if (two.Length != 0)
                {
                    outs += Tool4DataProcess.Hex2Str(two, 0, two.Length, true);
                }
                //排除心跳回复
                if (cmd != 13)
                {
                    //Tools.ShowMsg($"+++[{outs.Length / 3}]:{outs}\r\n");
                }

            }

            加密(GetKey(m_日期KEY), head, 0, 0);//先加入加密的头部
            加密(GetKey(m_日期KEY), one, 0, 0);//先加入加密的头部
            加密(GetKey(m_日期KEY), two, 0, 0);//先加入加密的头部
            lock (lock_tcp)
            {
                Egd.head = head;
                Egd.one = one;
                Egd.two = two;
                //player.Send(head);
                //player.Send(one);
                //player.Send(two);
            }
            return Egd;
        }


        public static string PopAnalysisString(byte[] scr)
        {
            string rs = string.Empty;
            //  PushByte(0); //最后是结束符--》 直接去掉,开始4位保存的是长度去掉，真实数据在中间
            byte[] codes = new byte[scr.Length - 1 - 4];
            Array.Copy(scr, 4, codes, 0, codes.Length);
            //留了4位在下面，保存的是字串长度 长度是int型所有占了4位
            rs = System.Text.Encoding.GetEncoding("GB2312").GetString(codes, 0, codes.Length);
            // PushInt(arr.Length + 1);
            return rs;
        }



        /// <summary>
        /// 解来自SEP.exe的包
        /// </summary>
        /// <param name="Head"></param>
        /// <param name="HeaderLen"></param>
        /// <param name="alldata"></param>
        /// <returns></returns>
        public OriginalData UnClientPack(byte[] alldata)
        {
            OriginalData ret = new OriginalData();
            if (alldata.Length < 18)
            {
                return ret;
            }
            #region  解包体 包括包头重新解一次
            int HeaderLen = 18;
            byte[] Head = new byte[HeaderLen];
            Array.Copy(alldata, 0, Head, 0, HeaderLen);



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
                throw new Exception($"非法字符串:{Tool4DataProcess.Hex2Str(Head, 0, Head.Length, true)}");
            }

            var outs = Tool4DataProcess.Hex2Str(Head, 0, rxi, true);
            ret.cmd = Head[0];
            int len = (int)Head[3];
            len <<= 8;
            len |= (int)Head[2];
            rxi = 0;
            if (len != 0)
            {
                ret.One = new byte[len];
                Array.Copy(alldata, 18, ret.One, 0, len);
                //alldata.Advance(HeaderLen);
                // alldata.TryCopyTo(ret.One);
                掩码 = 加密(KEY, ret.One, pi, 掩码);
                pi += len;
                outs += Tool4DataProcess.Hex2Str(ret.One, 0, len, true);
            }

            len = (int)Head[5];
            len <<= 8;
            len |= (int)Head[4];

            rxi = 0;
            if (len != 0)
            {
                ret.Two = new byte[len];
                if (ret.One == null)
                {
                    Array.Copy(alldata, 18, ret.Two, 0, len);
                    //reader.Advance(HeaderLen);
                }
                else
                {
                    // reader.Advance(HeaderLen + ret.One.Length);
                    Array.Copy(alldata, 18 + ret.One.Length, ret.Two, 0, len);
                }
                //reader.TryCopyTo(ret.Two);
                try
                {
                    掩码 = 加密(KEY, ret.Two, pi, 掩码);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }

            }
            //排除收到的心跳数据
            if (ret.cmd != 12)
            {
                // Tools.ShowMsg($"---==[{outs.Length / 3}]:{outs}\r\n");
            }

            #endregion
            return ret;
        }




        /// <summary>
        /// 解来自SEP.exe的包
        /// </summary>
        /// <param name="Head"></param>
        /// <param name="HeaderLen"></param>
        /// <param name="alldata"></param>
        /// <returns></returns>
        public OriginalData UnClientPack情缘等其他服(byte[] alldata)
        {
            OriginalData ret = new OriginalData();
            if (alldata.Length < 18)
            {
                return ret;
            }
            #region  解包体 包括包头重新解一次
            int HeaderLen = 18;
            byte[] Head = new byte[HeaderLen];
            Array.Copy(alldata, 0, Head, 0, HeaderLen);



            int rxi = 0;
            //先把头解密了，后面的就好说了
            byte[] KEY = null;
            uint 掩码 = 0;
            var 解码 = false;
            int pi = 0; //已经解密到哪个点了
            for (int i = 0; i < 4; i++)
            {
                KEY = GetKey情缘((i + m_日期KEY) % 4);
                掩码 = 加密情缘(KEY, Head, 0, 0);

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
                throw new Exception($"非法字符串:{Tool4DataProcess.Hex2Str(Head, 0, Head.Length, true)}");
            }

            var outs = Tool4DataProcess.Hex2Str(Head, 0, rxi, true);
            ret.cmd = Head[0];
            int len = (int)Head[3];
            len <<= 8;
            len |= (int)Head[2];
            rxi = 0;
            if (len != 0)
            {
                ret.One = new byte[len];
                Array.Copy(alldata, 18, ret.One, 0, len);
                //alldata.Advance(HeaderLen);
                // alldata.TryCopyTo(ret.One);
                掩码 = 加密(KEY, ret.One, pi, 掩码);
                pi += len;
                outs += Tool4DataProcess.Hex2Str(ret.One, 0, len, true);
            }

            len = (int)Head[5];
            len <<= 8;
            len |= (int)Head[4];

            rxi = 0;
            if (len != 0)
            {
                ret.Two = new byte[len];
                if (ret.One == null)
                {
                    Array.Copy(alldata, 18, ret.Two, 0, len);
                    //reader.Advance(HeaderLen);
                }
                else
                {
                    // reader.Advance(HeaderLen + ret.One.Length);
                    Array.Copy(alldata, 18 + ret.One.Length, ret.Two, 0, len);
                }
                //reader.TryCopyTo(ret.Two);
                try
                {
                    掩码 = 加密(KEY, ret.Two, pi, 掩码);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }

            }
            //排除收到的心跳数据
            if (ret.cmd != 12)
            {
                // Tools.ShowMsg($"---==[{outs.Length / 3}]:{outs}\r\n");
            }

            #endregion
            return ret;
        }


        /// <summary>
        /// 解决来自服务的包。主要是测试其他服务器
        /// </summary>
        /// <param name="Head"></param>
        /// <param name="HeaderLen"></param>
        /// <param name="alldata"></param>
        /// <returns></returns>
        public OriginalData UnServerPack(byte[] PackData)
        {
            OriginalData ret = new OriginalData();
            int len = 0;
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
                    throw new Exception($"解码失败，非法字符串:{Tool4DataProcess.Hex2Str(Head, 0, Head.Length, true)}");
                }

                var outs = Tool4DataProcess.Hex2Str(Head, 0, rxi, true);
                ret.cmd = Head[0];
                //int len = (int)Head[3];
                len = (int)Head[3];
                len <<= 8;
                len |= (int)Head[2];
                rxi = 0;
                if (len != 0)
                {
                    ret.One = new byte[len];
                    Array.Copy(PackData, 18, ret.One, 0, len);


                    掩码 = 加密(KEY, ret.One, pi, 掩码);
                    pi += len;
                    outs += Tool4DataProcess.Hex2Str(ret.One, 0, len, true);
                }

                len = (int)Head[5];
                len <<= 8;
                len |= (int)Head[4];

                rxi = 0;
                if (len != 0)
                {
                    ret.Two = new byte[len];
                    if (ret.One == null)
                    {
                        Array.Copy(PackData, 18, ret.Two, 0, len);
                        //reader.Advance(HeaderLen);
                    }
                    else
                    {
                        // reader.Advance(HeaderLen + ret.One.Length);、//如果这里长度不够。可能数据包很大。分多次传过来的。
                        Array.Copy(PackData, 18 + ret.One.Length, ret.Two, 0, len);
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
                    outs += Tool4DataProcess.Hex2Str(ret.Two, 0, len, true);
                }
                #endregion
            }
            catch (Exception exxx)
            {
                throw new Exception(string.Format("需要长度{0},提供的数据不够，不是完整的业务逻辑包。", len) + exxx.Message + exxx.StackTrace);
            }
            return ret;
        }





        /// <summary>
        /// 解决来自服务的包。主要是测试其他服务器 没有加密的封包
        /// </summary>
        /// <param name="Head"></param>
        /// <param name="HeaderLen"></param>
        /// <param name="alldata"></param>
        /// <returns></returns>
        public OriginalData UnServerPackNoEncryption(byte[] PackData)
        {
            OriginalData ret = new OriginalData();
            int len = 0;
            try
            {
                #region  
                byte[] Head = new byte[18];
                Array.Copy(PackData, Head, 18);
                int rxi = 0;
                int pi = 0; //已经解密到哪个点了
                pi += 18;
                var outs = Tool4DataProcess.Hex2Str(Head, 0, rxi, true);
                ret.cmd = Head[0];
                len = (int)Head[3];
                len <<= 8;
                len |= (int)Head[2];
                rxi = 0;
                if (len != 0)
                {
                    ret.One = new byte[len];
                    Array.Copy(PackData, 18, ret.One, 0, len);
                    pi += len;
                    outs += Tool4DataProcess.Hex2Str(ret.One, 0, len, true);
                }

                len = (int)Head[5];
                len <<= 8;
                len |= (int)Head[4];

                rxi = 0;
                if (len != 0)
                {
                    ret.Two = new byte[len];
                    if (ret.One == null)
                    {
                        Array.Copy(PackData, 18, ret.Two, 0, len);
                    }
                    else
                    {
                        // reader.Advance(HeaderLen + ret.One.Length);、//如果这里长度不够。可能数据包很大。分多次传过来的。
                        Array.Copy(PackData, 18 + ret.One.Length, ret.Two, 0, len);
                    }
                    outs += Tool4DataProcess.Hex2Str(ret.Two, 0, len, true);
                }
                #endregion
            }
            catch (Exception exxx)
            {
                throw new Exception(string.Format("需要长度{0},提供的数据不够，不是完整的业务逻辑包。", len) + exxx.Message + exxx.StackTrace);
            }
            return ret;
        }

        /// <summary>
        /// 客户端将生成的数据打包加密码准备给服务器
        /// </summary>
        /// <param name="gd"></param>
        /// <returns></returns>
        public string ClientPackingAsHexString(OriginalData gd)
        {
            string HexStringWithSpace = string.Empty;
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

            head[2] = (byte)one.Length;
            head[3] = (byte)(one.Length >> 8);

            head[4] = (byte)two.Length;
            head[5] = (byte)(two.Length >> 8);
            var outs = "";
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


            HexStringWithSpace += Tool4DataProcess.byteToHexStr(head, false);
            //HexStringWithSpace += "\r\n";
            HexStringWithSpace += Tool4DataProcess.byteToHexStr(one, false);
            //HexStringWithSpace += "\r\n";
            HexStringWithSpace += Tool4DataProcess.byteToHexStr(two, false);

            return HexStringWithSpace;// + "====" + stringKey;
        }

        /// <summary>
        /// 服务端将生成的数据打包加密码准备发送
        /// </summary>
        /// <param name="gd"></param>
        /// <returns></returns>
        public string ServerPackingAsHexString(OriginalData gd)
        {
            string HexStringWithSpace = string.Empty;
            byte cmd = gd.cmd;
            byte[] one = gd.One;
            byte[] two = gd.Two;
            bool Debug = false;

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

            head[2] = (byte)one.Length;
            head[3] = (byte)(one.Length >> 8);

            head[4] = (byte)two.Length;
            head[5] = (byte)(two.Length >> 8);
            var outs = "";
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
            if (Debug == true)
            {
                outs += Tool4DataProcess.Hex2Str(head, 0, head.Length, true);
                if (one.Length != 0)
                {
                    outs += Tool4DataProcess.Hex2Str(one, 0, one.Length, true);
                }
                if (two.Length != 0)
                {
                    outs += Tool4DataProcess.Hex2Str(two, 0, two.Length, true);
                }
                //排除心跳回复
                if (cmd != 13)
                {
                    //Tools.ShowMsg($"+++[{outs.Length / 3}]:{outs}\r\n");
                }
            }

            加密(GetKey(m_日期KEY), head, 0, 0);//先加入加密的头部
            加密(GetKey(m_日期KEY), one, 0, 0);//先加入加密的头部
            加密(GetKey(m_日期KEY), two, 0, 0);//先加入加密的头部

            HexStringWithSpace += Tool4DataProcess.byteToHexStr(head, true);
            HexStringWithSpace += Tool4DataProcess.byteToHexStr(one, true);
            HexStringWithSpace += Tool4DataProcess.byteToHexStr(two, true);

            return HexStringWithSpace;
        }


        public int m_日期KEY;

        private object lock_tcp = new object();



        public void WriteClientData(byte cmd, byte[] one, byte[] two)
        {
            bool Debug = false;

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

            head[2] = (byte)one.Length;
            head[3] = (byte)(one.Length >> 8);

            head[4] = (byte)two.Length;
            head[5] = (byte)(two.Length >> 8);
            var outs = "";
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
            if (Debug == true)
            {
                outs += Tool4DataProcess.Hex2Str(head, 0, head.Length, true);
                if (one.Length != 0)
                {
                    outs += Tool4DataProcess.Hex2Str(one, 0, one.Length, true);
                }
                if (two.Length != 0)
                {
                    outs += Tool4DataProcess.Hex2Str(two, 0, two.Length, true);
                }
                //排除心跳回复
                if (cmd != 13)
                {
                    //Tools.ShowMsg($"+++[{outs.Length / 3}]:{outs}\r\n");
                }

            }

            加密(GetKey(m_日期KEY), head, 0, 0);//先加入加密的头部
            加密(GetKey(m_日期KEY), one, 0, 0);//先加入加密的头部
            加密(GetKey(m_日期KEY), two, 0, 0);//先加入加密的头部


        }


        //解码
        public static UInt32 加密(byte[] KEY, byte[] buff, int start, UInt32 掩码)  //加解密函数 调用这个即可 数组指针 长度
        {

            string stringKey = string.Empty;
            for (int i = 0; i < buff.Length; i++)
            {
                stringKey += string.Format("{0:X2}", buff[i]);
            }
            stringKey += "前面是buff  ";


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
                if (i == 12)
                {

                }
                if (i > 255)
                {

                }
                byte pi = (byte)(i + 1 + start);
                temp1 = KEY[pi];//temp1=231
                temp3 = (byte)(temp1 + temp3);//保存变量
                temp2 = KEY[temp3];//key[temp3]=187
                KEY[pi] = temp2;
                KEY[temp3] = temp1;
                //for (int k = 0; k < KEY.Length; k++)
                //{
                //    if (KEY[k] == 81)
                //    {
                //        //k=217
                //    }
                //}
                temp2 = (byte)(temp1 + temp2);
                temp1 = KEY[temp2];//KEY[217]=81
                temp4 = buff[i];

                temp4 = (byte)(temp4 ^ temp1);//19^81=66
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



        //解码
        public static UInt32 加密情缘(byte[] KEY, byte[] buff, int start, UInt32 掩码)  //加解密函数 调用这个即可 数组指针 长度
        {
            byte temp1, temp2, temp3, temp4;
            temp4 = (byte)掩码;
            掩码 >>= 8;
            temp3 = (byte)(掩码);
            掩码 >>= 8;
            temp2 = (byte)(掩码);
            掩码 >>= 8;
            temp1 = (byte)(掩码);
            for (int i = start; i < buff.Length; i++)
            {
                //byte pi = (byte)(i + 1 + start);
                temp1 = KEY[(i + 1) % 256];//temp1=231
                temp3 = (byte)(temp1 + temp3);//保存变量
                temp2 = KEY[temp3];//key[temp3]=187
                KEY[(i + 1) % 256] = temp2;
                KEY[temp3] = temp1;
                //for (int k = 0; k < KEY.Length; k++)
                //{
                //    if (KEY[k] == 81)
                //    {
                //        //k=217
                //    }
                //}
                temp2 = (byte)(temp1 + temp2);
                temp1 = KEY[temp2];//KEY[217]=81
                temp4 = buff[i];

                temp4 = (byte)(temp4 ^ temp1);//19^81=66
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

        public static byte[] GetKey情缘(int step)
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
            byte[] bys = Tool4DataProcess.StrToBytes(s);
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

        //=======
    }
}
