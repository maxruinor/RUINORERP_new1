using System;
using System.Collections.Generic;
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
        private int initSize; //初始化时的大小
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
                size = 50;//默认申请50个空间
            }
            buff = new byte[size];
            popP = 0;
            pushP = 0;
            initSize = size;
        }
        public int Length { get { return buff.Length; } }
        public void PushBytes(byte[] val)
        {
            foreach (byte by in val)
            {
                PushByte(by);
            }
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
                var newt = new byte[buff.Length + initSize];
                Array.Copy(buff, newt, buff.Length);
                buff = newt;
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
            var arr = Tool4DataProcess.StrToBytes(val);
            PushInt(arr.Length + 1);
            foreach (byte by in arr)
            {
                PushByte(by);
            }
            PushByte(0); //最后是结束符
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
        public byte[] toByte()
        {
            byte[] ret = new byte[pushP];
            for (int i = 0; i < pushP; i++)
            {
                ret[i] = buff[i];
            }
            return ret;
        }
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
