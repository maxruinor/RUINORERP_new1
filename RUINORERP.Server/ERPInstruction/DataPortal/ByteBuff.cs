using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TransInstruction.DataPortal
{
    /// <summary>
    /// 针对读取和写入
    /// </summary>
    public class ByteBuff
    {
        private byte[] buff;
        private int popP; //push
        private int pushP;
        //private int initSize; //初始化时的大小
        private  int initSize = 100; // 初始大小，可以根据需要调整
        private  int growSize = 1024; // 增长大小，每次扩展的大小
        public ByteBuff(byte[] arr)
        {
            buff = arr;
            if (buff == null)
            {
                initSize = 0;
            }
            else
            {
                initSize = buff.Length;
            }

            popP = 0;
            pushP = 0;
        }


        /// <summary>
        /// 是动态变化吗？
        /// </summary>
        /// <param name="size"></param>
        public ByteBuff(int size)
        {
            if (size == 0)
            {
                size = 100;//默认申请50个空间
            }
            buff = new byte[size];
            popP = 0;
            pushP = 0;
            initSize = size;
        }
        public int Length { get { return buff.Length; } }
        //public void PushBytes(byte[] val)
        //{
        //    foreach (byte by in val)
        //    {
        //        PushByte(by);
        //    }
        //}

        public void PushBytes(byte[] vals)
        {
            if (vals == null || vals.Length == 0) return;

            int bytesNeeded = pushP + vals.Length - buff.Length;
            if (bytesNeeded > 0)
            {
                Array.Resize(ref buff, buff.Length + Math.Max(growSize, bytesNeeded));
            }
            Array.Copy(vals, 0, buff, pushP, vals.Length);
            pushP += vals.Length;
        }

        public void PushBytes4String(string hexstr)
        {
            byte[] t;
            string msg;
            Tool4DataProcess.StrToHex(hexstr, 0, hexstr.Length, out t, out msg);
            PushBytes(t);
        }
        public byte this[int index]
        {
            get
            {
                if (index >= pushP)
                {
                    throw new Exception("超出范围");
                }
                return buff[index];
            }
            set
            {
                buff[index] = value;
            }
        }
        /// <summary>
        /// 压入一个浮点数4位
        /// </summary>
        /// <param name="val"></param>
        public void PushFloat(double val)
        {
            PushBytes(BitConverter.GetBytes((float)val));
        }
        public void PushByte(byte val)
        {
            if (pushP >= buff.Length)
            {
                 

                // 扩展缓冲区
                //byte[] newBuff = new byte[buff.Length + growSize];
                //Array.Copy(buff, newBuff, buff.Length);
                //buff = newBuff;
                //比上面手动复制更高效
                Array.Resize(ref buff, buff.Length + growSize);
            }
            buff[pushP++] = val;
        }
        public void PushBool(bool val)
        {
            PushByte(val == true ? (byte)1 : (byte)0);
        }

        public void PushInt16(int val)
        {
            PushInt16((Int16)val);
        }
        public void PushInt16(UInt16 val)
        {
            PushInt16((Int16)val);
        }
        public void PushInt16(Int16 val)
        {
            PushByte((byte)val);
            val >>= 8;
            PushByte((byte)val);
        }

        public void PushInt(Int32 val)
        {
            PushInt((UInt32)val);
        }
        public void PushInt(UInt32 val)
        {
            PushByte((byte)val);
            val >>= 8;
            PushByte((byte)val);
            val >>= 8;
            PushByte((byte)val);
            val >>= 8;
            PushByte((byte)val);
        }
        public void PushInt64(Int64 val)
        {
            PushByte((byte)val);
            val >>= 8;
            PushByte((byte)val);
            val >>= 8;
            PushByte((byte)val);
            val >>= 8;
            PushByte((byte)val);
            val >>= 8;
            PushByte((byte)val);
            val >>= 8;
            PushByte((byte)val);
            val >>= 8;
            PushByte((byte)val);
            val >>= 8;
            PushByte((byte)val);
        }

        /// <summary>
        /// 长度有限制int的最大值
        /// 这个在业务打包时要成对使用。比方 客户端发送一个长字符串，服务端解析时也要用长字符串的方法来解析
        /// </summary>
        /// <param name="val"></param>
        public void PushString(string val)
        {
            if (val == null)
            {
                PushInt(0);
                return;
            }
             
            byte[] byteArray = Encoding.UTF8.GetBytes(val);
            PushInt(byteArray.Length);
            // 推送整个字节数组
            PushBytes(byteArray);
            PushByte(0); //最后是结束符

            //var arr = Tool4DataProcess.StrToBytes(val);
            //PushInt(arr.Length + 1);
            //foreach (byte by in arr)
            //{
            //    PushByte(by);
            //}
            //PushByte(0); //最后是结束符
        }

        /// <summary>
        /// 添加长字符串，以0结尾
        /// 这个在业务打包时要成对使用。比方 客户端发送一个长字符串，服务端解析时也要用长字符串的方法来解析
        /// 本来想 int64来保存长度。缓存太多。实际int32足够了暂时不用这个方法。省得 没有成对使用时。解析出错。
        /// </summary>
        /// <param name="val"></param>
        //public void PushLongString(string val)
        //{
        //    var arr = Tool4DataProcess.StrToBytes(val);
        //    PushInt64(arr.Length + 1);
        //    foreach (byte by in arr)
        //    {
        //        PushByte(by);
        //    }
        //    PushByte(0); //最后是结束符
        //}



        /// <summary>
        /// 将全部数据导出，已经pop的数据也导出
        /// </summary>
        /// <returns></returns>
        //public byte[] toByte()
        //{
        //    byte[] ret = new byte[pushP];
        //    for (int i = 0; i < pushP; i++)
        //    {
        //        ret[i] = buff[i];
        //    }
        //    return ret;
        //}

        public byte[] toByte()
        {
            // 返回buff的一个副本，避免外部对内部状态的影响
            byte[] ret = new byte[pushP];
            Array.Copy(buff, ret, pushP);
            return ret;
        }
        //或者，如果可以接受返回 buff 的一个视图，可以使用 ArraySegment<byte>：
        //public ArraySegment<byte> toByte()
        //{
        //    // 返回buff的一个视图，不创建新的数组
        //    return new ArraySegment<byte>(buff, 0, pushP);
        //}

        public void Step(int step)
        {
            popP += step;
        }
        public bool GetBool()
        {
            var ret = false;
            if (buff[popP++] == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public byte GetByte()
        {
            return buff[popP++];
        }
        public UInt16 GetUint16()
        {
            UInt16 ret = (UInt16)buff[popP + 1];
            ret <<= 8;
            ret |= (UInt16)buff[popP];
            popP += 2;
            return ret;
        }
        public int GetInt()
        {
            int ret = (int)buff[popP + 3];
            ret <<= 8;
            ret |= (int)buff[popP + 2];
            ret <<= 8;
            ret |= (int)buff[popP + 1];
            ret <<= 8;
            ret |= (int)buff[popP];
            popP += 4;
            return ret;
        }
        //public float GetFloat()
        //{
        //    return BitConverter.ToSingle(GetInt());
        //}
        public string GetString()
        {
            int len = GetInt();
            string ret = Tool4DataProcess.toStr(buff, popP, len - 1);
            popP += len;
            return ret;
        }
    }
}
