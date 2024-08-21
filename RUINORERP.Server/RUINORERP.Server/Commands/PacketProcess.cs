using TransInstruction;
using RUINORERP.Server.Lib;
using RUINORERP.Server.ServerSession;
using System;
using System.Collections.Generic;
using System.Text;
using TransInstruction.DataPortal;

namespace RUINORERP.Server.Commands
{

    /// <summary>
    /// 网络数据包的处理 工具类
    /// </summary>
    public class PacketProcess
    {
        SessionforBiz player;
        public PacketProcess(SessionforBiz _player)
        {
            this.player = _player;
        }
        /// <summary>
        /// 没有发送数据操作时才能用。
        /// </summary>
        public PacketProcess()
        {

        }

        /// <summary>
        /// 解来自SEP的包
        /// </summary>
        /// <param name="Head"></param>
        /// <param name="HeaderLen"></param>
        /// <param name="alldata"></param>
        /// <returns></returns>
        public KxData UnClientPack(byte[] Head, int HeaderLen, byte[] alldata)
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
                throw new Exception($"非法字符串:{Tools.Hex2Str(Head, 0, Head.Length, true)}");
            }

            var outs = Tools.Hex2Str(Head, 0, rxi, true);
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
                outs += Tools.Hex2Str(ret.One, 0, len, true);
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
                outs += Tools.Hex2Str(ret.Two, 0, len, true);
            }
            //排除收到的心跳数据
            if (ret.cmd != 12)
            {
                //Tools.ShowMsg($"---==[{outs.Length / 3}]:{outs}\r\n");
            }

            #endregion
            return ret;
        }







        public static int m_日期KEY;

        private static object lock_tcp = new object();



        /// <summary>
        /// 加密方法
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="one"></param>
        /// <param name="two"></param>
        /// <returns></returns>
        public static EncryptedData EncryptedDataPack(byte cmd, byte[] one, byte[] two)
        {
            EncryptedData Egd = new EncryptedData();
            TransInstruction.TransProtocol tester = new TransInstruction.TransProtocol();
            TransInstruction.OriginalData gd = new TransInstruction.OriginalData();
            gd.cmd = cmd;
            gd.One = one;
            gd.Two = two;

            /*
            ///有一个调试才解析
            if (frmServerUI.Instance.chk封包调试.Checked || frmServerUI.Instance.chk指令调试.Checked)
            {
                string checkrs = tester.CheckServerCmdNew(gd, true);
                if (!(checkrs.Contains("心跳") || checkrs.Contains("等待")))
                {
                    if (frmServerUI.Instance.chk封包调试.Checked)
                    {
                        StringBuilder debugmsg = new StringBuilder();
                        debugmsg.Append("==EncryptedDataPack==" + checkrs);
                        #region 输出调试数据

                        if (gd.One != null)
                        {
                            debugmsg.Append("One:" + gd.One.Length.ToString() + "|");
                        }
                        if (gd.Two != null)
                        {
                            debugmsg.Append("Two:" + gd.Two.Length.ToString() + "|");
                        }
                        debugmsg.Append("\r\n");
                        debugmsg.Append("cmd:=" + gd.cmd + "," + string.Format("0x{0:X2}", gd.cmd)).Append("\r\n");
                        if (gd.One != null)
                        {
                            foreach (var o in gd.One)
                            {
                                debugmsg.Append("one:=" + o + "," + string.Format("0x{0:X2}", o)).Append("\r\n");
                            }
                        }
                        if (gd.Two != null)
                        {
                            foreach (var o in gd.Two)
                            {
                                debugmsg.Append("Two:=" + o + "," + string.Format("0x{0:X2}", o)).Append("\r\n");
                            }
                        }
                        #endregion
                        Tools.ShowMsg(Guid.NewGuid().ToString() + "服务器发送指令测试结果2：" + debugmsg.ToString());
                        string sstemp = string.Empty;
                        sstemp = debugmsg.ToString();


                    }
                    if (frmServerUI.Instance.chk指令调试.Checked)
                    {
                        Tools.ShowMsg("服务器发送指令测试结果3：" + checkrs.ToString());
                    }
                }
            }
            */

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
                outs += Tools.Hex2Str(head, 0, head.Length, true);
                if (one.Length != 0)
                {
                    outs += Tools.Hex2Str(one, 0, one.Length, true);
                }
                if (two.Length != 0)
                {
                    outs += Tools.Hex2Str(two, 0, two.Length, true);
                }
                //排除心跳回复
                if (cmd != 13)
                {
                    Tools.ShowMsg($"+++[{outs.Length / 3}]:{outs}\r\n");
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

        public void WriteClientData(KxData data)
        {
            WriteClientData(data.cmd, data.One, data.Two);
        }

        //临时用一下 将数据打包发送，自动添加标头和标尾,自动进行加密
        public void WriteClientData(byte cmd, byte[] one, byte[] two)
        {
            WriteClientData(this.player, cmd, one, two);
        }

        public void WriteClientData(SessionforBiz player, TransInstruction.OriginalData gd)
        {
            WriteClientData(this.player, gd.cmd, gd.One, gd.Two);
        }

        public void WriteClientData(SessionforBiz player, byte cmd, byte[] one, byte[] two)
        {
            TransInstruction.TransProtocol tester = new TransInstruction.TransProtocol();
            TransInstruction.OriginalData gd = new TransInstruction.OriginalData();
            gd.cmd = cmd;
            gd.One = one;
            gd.Two = two;

            string checkrs = tester.CheckServerCmd(gd);
            /*
            if (!(checkrs.Contains("心跳") || checkrs.Contains("等待")))
            {
                if (frmMain.Instance.chk封包调试.Checked)
                {
                    StringBuilder debugmsg = new StringBuilder();
                    debugmsg.Append("==WriteClientData==" + checkrs);
                    #region 输出调试数据

                    if (gd.One != null)
                    {
                        debugmsg.Append("One:" + gd.One.Length.ToString() + "|");
                    }
                    if (gd.Two != null)
                    {
                        debugmsg.Append("Two:" + gd.Two.Length.ToString() + "|");
                    }
                    debugmsg.Append("\r\n");
                    debugmsg.Append("cmd:=" + gd.cmd + "," + string.Format("0x{0:X2}", gd.cmd)).Append("\r\n");
                    if (gd.One != null)
                    {
                        foreach (var o in gd.One)
                        {
                            debugmsg.Append("one:=" + o + "," + string.Format("0x{0:X2}", o)).Append("\r\n");
                        }
                    }
                    if (gd.Two != null)
                    {
                        foreach (var o in gd.Two)
                        {
                            debugmsg.Append("Two:=" + o + "," + string.Format("0x{0:X2}", o)).Append("\r\n");
                        }
                    }
                    #endregion
                    Tools.ShowMsg("服务器发送指令测试结果4：" + debugmsg.ToString());
                    string sstemp = string.Empty;
                    sstemp = debugmsg.ToString();


                }
            }
            */
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
                outs += Tools.Hex2Str(head, 0, head.Length, true);
                if (one.Length != 0)
                {
                    outs += Tools.Hex2Str(one, 0, one.Length, true);
                }
                if (two.Length != 0)
                {
                    outs += Tools.Hex2Str(two, 0, two.Length, true);
                }
                //排除心跳回复
                if (cmd != 13)
                {
                    Tools.ShowMsg($"+++[{outs.Length / 3}]:{outs}\r\n");
                }

            }

            加密(GetKey(m_日期KEY), head, 0, 0);//先加入加密的头部
            加密(GetKey(m_日期KEY), one, 0, 0);//先加入加密的头部
            加密(GetKey(m_日期KEY), two, 0, 0);//先加入加密的头部
            lock (lock_tcp)
            {
                player.Send(head);
                player.Send(one);
                player.Send(two);
            }


            //监控输出的数据
            string str1 = Tools.byteToHexStr(one, true);
            string str2 = Tools.byteToHexStr(head, true);
            string str3 = Tools.byteToHexStr(two, true);
            //ServiceforGame<SephirothServer.Server.GamePackageInfo> sg = (this as IAppSession).Server as ServiceforGame<SephirothServer.Server.GamePackageInfo>;
            //if (GlobalSettings._debug == DebugController.Debug)
            //{
            //    log4netHelper.debug("WriteClientData:" + head.Length + "\r\n " + str1);
            //    log4netHelper.debug("WriteClientData:" + one.Length + "\r\n " + str2);
            //    log4netHelper.debug("WriteClientData:" + two.Length + "\r\n " + str3);
            //}
            //监控输出的数据end
        }




        //解码
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
            byte[] bys = Tools.StrToBytes(s);
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

    }



}
