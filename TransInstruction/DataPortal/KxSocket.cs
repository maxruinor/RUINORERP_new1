using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TransInstruction.DataPortal
{
    public struct KxData
    {
        public byte cmd;
        public byte[] One;
        public byte[] Two;
    }
    public class MyQueue<T> : Queue<T>
    {
        Queue<T> ts = new Queue<T>();
        object lock_ts = new object();
        #region

        /// <summary>
        ///  入队
        /// </summary>
        /// <param name="item"></param>
        public new void Enqueue(T item)
        {
            lock (lock_ts)
            {
                ts.Enqueue(item);
            }
        }
        #endregion

        /// <summary>
        /// 出队
        /// </summary>
        /// <returns></returns>
        public new T Dequeue()
        {
            lock (lock_ts)
            {
                return ts.Dequeue();
            }
        }
        public new int Count
        {
            get
            {
                lock (lock_ts)
                {
                    return ts.Count;
                }
            }
        }
    }
    public class KxSocket
    {
        // 自己需要处理的信息
        public MyQueue<KxData> m_list=new MyQueue<KxData>();

        public int m_日期KEY;
        private object lock_tcp = new object();
        private System.Net.Sockets.Socket tcp;
        public bool Debug = true;

        public KxSocket()
        {

        }
        public KxSocket(System.Net.Sockets.Socket tcp)
        {
            this.tcp = tcp;
        }
        /// <summary>
        /// 直接发送，不处理
        /// </summary>
        /// <param name="bys"></param>
        public void Write(byte[] bys)
        {
            lock (lock_tcp)
            {
                this.tcp.Send(bys);
            }
        }
        // 打包一个数据
        public static KxData Pack(byte cmd, byte[] one, byte[] two)
        {
            KxData ret = new KxData();
            ret.cmd = cmd;
            ret.One = one;
            ret.Two = two;
            return ret;
        }
        public static KxData Pack(byte cmd, int id, byte[] two)
        {
            var tx = new ByteBuff(4);
            tx.PushInt(id);
            return Pack(cmd, tx.toByte(), two);
        }
        public static KxData Pack(byte cmd, int id)
        {
            var tx = new ByteBuff(4);
            tx.PushInt(id);
            return Pack(cmd, tx.toByte(), new byte[] { });
        }
        public void ReadWithLength(int length, int timeout)
        {
            int rxi = 0;
            var Rx = new byte[length];
            tcp.ReceiveTimeout = timeout;
            try
            {   
                while (rxi < length)
                {
                    rxi += tcp.Receive(Rx, rxi, length - rxi, System.Net.Sockets.SocketFlags.None);
                    tcp.ReceiveTimeout = 1000;
                }
            }
            catch (Exception)
            {}
            var outs = Tool4DataProcess.Hex2Str(Rx, 0, rxi, true);
            //ShowMsg($"----[%{outs.Length / 3}]:{outs}\r\n");
        }
        public void WriteClientData(KxData data)
        {
            WriteClientData(data.cmd, data.One, data.Two);
        }
        
        //public void f提示消息(StructMap.E创建角色提示消息 代码)
        //{ 
        //    ByteBuff bb = new ByteBuff(8);
        //    bb.PushInt16(0x00E2);
        //    bb.PushInt(0);
        //    bb.PushInt16((Int16)代码);
        //    WriteClientData(0x0A, null, bb.toByte()); //发送错误消息
        //}
        public void 提示删除成功()
        {
            ByteBuff bb = new ByteBuff(8);
            bb.PushInt16(0x00E3);
            bb.PushInt(0);
            bb.PushInt16((Int16)00);
            WriteClientData(0x0A, null, bb.toByte()); //发送错误消息
        }
        public void 发送服务器名称(string 名称)
        {
            ByteBuff tx = new ByteBuff(名称.Length+12);
            tx.PushInt16(0x00E1);
            tx.PushByte(0);
            tx.PushInt(0);
            tx.PushString(名称);
            tx.PushString("");
            WriteClientData(0x0A, null, tx.toByte());
        }


      

        //将数据打包发送，自动添加标头和标尾,自动进行加密
        public void WriteClientData(byte cmd, byte[] one, byte[] two)
        {
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
            for (int i = 0; i <8; i++)
            {
                hi ^= head[i * 2];
                low ^= head[i * 2 + 1];
            }
            head[16]=hi;
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
               // Tool4DataProcess.ShowMsg($"+++[{outs.Length / 3}]:{outs}\r\n");
            }

            加密(GetKey(m_日期KEY), head, 0, 0);//先加入加密的头部
            加密(GetKey(m_日期KEY), one, 0, 0);//先加入加密的头部
            加密(GetKey(m_日期KEY), two, 0, 0);//先加入加密的头部
            lock (lock_tcp)
            {
                tcp.Send(head);
                tcp.Send(one);
                tcp.Send(two);
            }
            
        }

        public void ReadClientData()
        {            
            while (true)
            {
                KxData ret = new KxData();
                int rxi = 0;
                byte[] Head = new byte[18];
                tcp.ReceiveTimeout = 60000; //限制60秒读取不出来就退出
                //System.Net.Sockets.SocketAsyncEventArgs args = new System.Net.Sockets.SocketAsyncEventArgs();
                while (rxi < 18)
                {
                    try
                    {
                        /*tcp.BeginReceive(Head, 0, 18, System.Net.Sockets.SocketFlags.None, new AsyncCallback(p => {

                        }), null);
   */
                        rxi += tcp.Receive(Head, 0, 18 - rxi, System.Net.Sockets.SocketFlags.None);
                    }
                    catch (System.Net.Sockets.SocketException ex)
                    {

                    }catch(Exception)
                    {}
                }
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
                tcp.ReceiveTimeout = 10000; //10秒
                rxi = 0;
                if (len != 0)
                {
                    ret.One = new byte[len];
                    while (rxi < len)
                    {
                        rxi += tcp.Receive(ret.One, len - rxi, System.Net.Sockets.SocketFlags.None);
                    }
                    掩码 = 加密(KEY, ret.One, pi, 掩码);
                    pi += len;
                    outs += Tool4DataProcess.Hex2Str(ret.One, 0, len, true);
                }

                len = (int)Head[5];
                len <<= 8;
                len |= (int)Head[4];
                tcp.ReceiveTimeout = 10000; //10秒
                rxi = 0;
                if (len != 0)
                {
                    ret.Two = new byte[len];
                    while (rxi < len)
                    {
                        rxi += tcp.Receive(ret.Two, len - rxi, System.Net.Sockets.SocketFlags.None);
                    }
                    try
                    {
                        掩码 = 加密(KEY, ret.Two, pi, 掩码);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.ToString());
                    }
                    outs += Tool4DataProcess.Hex2Str(ret.Two, 0, len, true);
                }
              //  Tool4DataProcess.ShowMsg($"---[{outs.Length / 3}]:{outs}\r\n");
                m_list.Enqueue(ret);
            }
        }
        public KxData ReadClientData(int timeOut)
        {
            KxData ret = new KxData();
            int rxi = 0;
            byte[] Head = new byte[18];
            tcp.ReceiveTimeout = timeOut;
            while (rxi < 18)
            {
                rxi += tcp.Receive(Head, 18 - rxi, System.Net.Sockets.SocketFlags.None);
            }
            //先把头解密了，后面的就好说了
            byte[] KEY=null;
            uint 掩码 = 0;
            var 解码 = false;
            int pi = 0; //已经解密到哪个点了
            for (int i = 0; i < 4; i++)
            {
                KEY = GetKey((i+m_日期KEY)%4);
                掩码=加密(KEY, Head, 0, 0);

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
            tcp.ReceiveTimeout = 10000; //10秒
            rxi = 0;
            if (len != 0)
            {
                ret.One = new byte[len];
                while (rxi < len)
                {
                    rxi += tcp.Receive(ret.One, len - rxi, System.Net.Sockets.SocketFlags.None);
                }
                掩码=加密(KEY, ret.One, pi, 掩码);
                pi += len;
                outs += Tool4DataProcess.Hex2Str(ret.One, 0, len, true);
            }

            len = (int)Head[5];
            len <<= 8;
            len |= (int)Head[4];
            tcp.ReceiveTimeout = 10000; //10秒
            rxi = 0;
            if (len != 0)
            {
                ret.Two = new byte[len];
                while (rxi < len)
                {
                    rxi += tcp.Receive(ret.Two, len - rxi, System.Net.Sockets.SocketFlags.None);
                }
                try
                {
                    掩码 = 加密(KEY, ret.Two, pi, 掩码);
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
                    outs += Tool4DataProcess.Hex2Str(ret.Two, 0, len, true);
            }
            //Tool4DataProcess.ShowMsg($"---[{outs.Length/3}]:{outs}\r\n");
            return ret;
        }
        public void OFF()
        {
            ByteBuff bb = new ByteBuff(8);
            bb.PushInt16(0x00E4);
            bb.PushInt(0);
            bb.PushInt16((Int16)00); //这里以前是BYTE
            WriteClientData(0x0A, null, bb.toByte()); //发送错误消息
        }
        static byte[] 昨天;
        static byte[] 今天;
        static byte[] 明天;
        static byte[] 不变;
        static int 日期 = 0;
        static object lock_日期 = new object();


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
        //获取KEY
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
       
    }
}
