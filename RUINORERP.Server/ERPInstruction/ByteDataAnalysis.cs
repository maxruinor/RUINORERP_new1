using System;
using System.Collections.Generic;
using System.Linq;

namespace TransInstruction
{
    /// <summary>
    /// 工具类 想法是减少对包的解析过程的复杂度
    /// </summary>
    public class ByteDataAnalysis
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="Index">已经解析掉的数据长度</param>
        /// <returns></returns>
        public static bool Getbool(byte[] buffer, ref int Index)
        {
            bool b = false;
            try
            {
                byte[] sz = new byte[1];
                sz = buffer.Skip(Index).Take(1).ToArray();
                if (sz[0] == 1)
                {
                    b = true;
                }
                Index = Index + 1;
            }
            catch (Exception ex)
            {

            }
            return b;
        }

        public static int TryGetInt(byte[] source, out bool success, ref int Index)
        {
            success = false;
            byte[] buffer = new byte[4];
            buffer = source.Skip(Index).Take(4).ToArray();
            int rs = 0;
            bool isAllZero = true;
            int ZeroCounter = 0;
            int ZeroIndex = -1;

            List<KeyValuePair<int, int>> zeroFlagList = new List<KeyValuePair<int, int>>();
            for (int i = 0; i < buffer.Length; i++)
            {
                if (buffer[i] == 0)
                {
                    KeyValuePair<int, int> kv = new KeyValuePair<int, int>(i, 0);
                    zeroFlagList.Add(kv);
                    ZeroCounter++;
                }
                else
                {
                    isAllZero = false;
                }
            }
            //如果就一个0 ,并且在最后，其他三个有数字 就认为是int数字
            if (ZeroCounter == 1 && zeroFlagList[0].Key == 3)
            {
                int Value1 = ByteDataAnalysis.GetInt(buffer, ref Index);
            }

            //如果2个0 ,看位置 
            if (ZeroCounter == 2)
            {
                //0 1  2  3
                int 前两位为零 = 0;
                int 后两位为零 = 0;
                for (int f = 0; f < zeroFlagList.Count; f++)
                {
                    if (zeroFlagList[f].Key > 1)
                    {
                        后两位为零++;
                    }
                    else
                    {
                        前两位为零++;
                    }
                }

                if (后两位为零 == 2)
                {
                    success = false;
                    return rs;
                }
            }

            if (ZeroCounter == 3)
            {
                //&& zeroFlagList[0].Key == 3
            }

            //不用处理
            if (ZeroCounter == 4)
            {
                //&& zeroFlagList[0].Key == 3

            }

            success = true;
            return rs;
        }

        public static string TryGetString(byte[] source, out bool success, ref int Index)
        {
            success = false;
            //4000+12 05 14 15+0
            string rs = string.Empty;
            if (source.Length < Index)
            {
                return rs = "长度不够";
            }

            if (source.Length == Index)
            {
                return rs = "已经全部解析";
            }

            try
            {
                byte[] lenByte = new byte[4];
                lenByte = source.Skip(Index).Take(4).ToArray();
                int len = BitConverter.ToInt32(lenByte, 0);
                if (len > 255)
                {
                    return "";
                }
                //消息长度？ 暂时用下面直接减
                Index = Index + 4;

                byte[] msg = new byte[len];
                //msg.Length-1减1是去掉结束符
                msg = source.Skip(Index).Take(msg.Length - 1).ToArray();
                rs = System.Text.Encoding.GetEncoding("GB2312").GetString(msg);
                Index = Index + msg.Length + 1;//减1是去掉结束符,这里要加上去
            }
            catch (Exception ex)
            {

            }
            return rs;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="UnparsedData"></param>
        /// <param name="Index">前面多少位不是索引</param>
        /// <returns></returns>
        public static string GetShortString(byte[] buffer, out byte[] UnparsedData, ref int Index)
        {   //4000+12 05 14 15+0
            string rs = string.Empty;
            UnparsedData = new byte[1];//只是因为下面提前返回。要设置一个默认值
            if (buffer.Length < Index)
            {
                return rs = "长度不够";
            }

            if (buffer.Length == Index)
            {
                return rs = "已经全部解析";
            }

            try
            {
                byte[] lenByte = new byte[4];
                lenByte = buffer.Skip(Index).Take(4).ToArray();
                int len = BitConverter.ToInt32(lenByte, 0);
                if (len > 255)
                {
                    return "";
                }
                //消息长度？ 暂时用下面直接减
                Index = Index + 4;

                byte[] msg = new byte[len];
                //msg.Length-1减1是去掉结束符
                msg = buffer.Skip(Index).Take(msg.Length - 1).ToArray();
                rs = System.Text.Encoding.GetEncoding("GB2312").GetString(msg);
                Index = Index + msg.Length + 1;//减1是去掉结束符,这里要加上去

                UnparsedData = new byte[buffer.Length - Index];//只是因为下面提前返回。要设置一个默认值

                //Array.Copy(buffer, UnparsedData, Index);
                Buffer.BlockCopy(buffer, Index, UnparsedData, 0, UnparsedData.Length);
            }
            catch (Exception ex)
            {

            }
            return rs;
        }


        /// <summary>
        /// 解析字符串不超过255字节 ，为什么？忘记了
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="Index"></param>
        /// <returns></returns>
        public static string GetShortString(byte[] buffer, ref int Index)
        {   //4000+12 05 14 15+0
            string rs = string.Empty;
            if (buffer.Length < Index)
            {
                return rs = "长度不够";
            }

            if (buffer.Length == Index)
            {
                return rs = "已经全部解析";
            }

            try
            {
                byte[] lenByte = new byte[4];
                lenByte = buffer.Skip(Index).Take(4).ToArray();
                int len = BitConverter.ToInt32(lenByte, 0);
                if (len > 255)
                {
                    return "";
                }
                //消息长度？ 暂时用下面直接减
                Index = Index + 4;

                byte[] msg = new byte[len];
                //msg.Length-1减1是去掉结束符
                msg = buffer.Skip(Index).Take(msg.Length - 1).ToArray();
                rs = System.Text.Encoding.GetEncoding("GB2312").GetString(msg);
                Index = Index + msg.Length + 1;//减1是去掉结束符,这里要加上去
            }
            catch (Exception ex)
            {

            }
            return rs;
        }

        /// <summary>
        /// 解析字符串不超过255字节 ，为什么？忘记了
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="Index"></param>
        /// <returns></returns>
        public static string GetLongString(byte[] buffer, ref int Index)
        {   //4000+12 05 14 15+0
            string rs = string.Empty;
            if (buffer.Length < Index)
            {
                return rs = "长度不够";
            }

            if (buffer.Length == Index)
            {
                return rs = "已经全部解析";
            }

            try
            {
                byte[] lenByte = new byte[4];
                lenByte = buffer.Skip(Index).Take(4).ToArray();
                int len = BitConverter.ToInt32(lenByte, 0);
               
                //消息长度？ 暂时用下面直接减
                Index = Index + 4;

                byte[] msg = new byte[len];
                //msg.Length-1减1是去掉结束符
                msg = buffer.Skip(Index).Take(msg.Length - 1).ToArray();
                rs = System.Text.Encoding.GetEncoding("GB2312").GetString(msg);
                Index = Index + msg.Length + 1;//减1是去掉结束符,这里要加上去
            }
            catch (Exception ex)
            {

            }
            return rs;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="UnparsedData"></param>
        /// <param name="Index">最后一位处理过的标记0开始</param>
        /// <returns></returns>
        public static int Getint(byte[] buffer, out byte[] UnparsedData, ref int Index)
        {
            int y = 0;
            UnparsedData = new byte[1];//只是因为下面提前返回。要设置一个默认值
            if (buffer.Length < Index)
            {
                // rs = "长度不够";
                return 0;
            }

            if (buffer.Length == Index)
            {
                // return rs = "已经全部解析";
                return 0;
            }

            try
            {
                byte[] intByte = new byte[4];
                intByte = buffer.Skip(Index).Take(4).ToArray();
                y = BitConverter.ToInt32(intByte, 0);
                Index = Index + 4;
                UnparsedData = new byte[buffer.Length - Index];//只是因为下面提前返回。要设置一个默认值
                Buffer.BlockCopy(buffer, Index, UnparsedData, 0, UnparsedData.Length);
            }
            catch (Exception ex)
            {

            }
            return y;
        }



        public static int GetInt(byte[] buffer, ref int Index)
        {
            int y = 0;
            try
            {
                byte[] sz = new byte[4];
                sz = buffer.Skip(Index).Take(4).ToArray();
                if (sz.Length == 0)
                {
                    return y;
                }
                y = BitConverter.ToInt32(sz, 0);
                Index = Index + 4;
            }
            catch (Exception ex)
            {

            }
            return y;
        }

        public static Int64 GetInt64(byte[] buffer, ref int Index)
        {
            Int64 y = 0;
            try
            {
                byte[] sz = new byte[8];
                sz = buffer.Skip(Index).Take(8).ToArray();
                y = BitConverter.ToInt64(sz, 0);
                Index = Index + 8;
            }
            catch (Exception ex)
            {

            }
            return y;
        }

        public static float GetFloat(byte[] buffer, ref int Index)
        {
            float f = 0;
            try
            {
                byte[] sz = new byte[4];
                sz = buffer.Skip(Index).Take(4).ToArray();
                f = BitConverter.ToSingle(sz, 0);
                Index = Index + 4;
            }
            catch (Exception ex)
            {

            }
            return f;
        }


        public static Int16 GetInt16(byte[] buffer, ref int Index)
        {
            Int16 y = 0;
            try
            {
                byte[] sz = new byte[2];
                sz = buffer.Skip(Index).Take(2).ToArray();
                y = BitConverter.ToInt16(sz, 0);
                Index = Index + 2;
            }
            catch (Exception ex)
            {

            }
            return y;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="Index">已经解析掉的数据长度</param>
        /// <returns></returns>
        public static byte Getbyte(byte[] buffer, ref int Index)
        {
            byte b = 0;
            try
            {
                byte[] sz = new byte[1];
                sz = buffer.Skip(Index).Take(1).ToArray();
                b = sz[0];
                Index = Index + 1;
            }
            catch (Exception ex)
            {

            }
            return b;
        }


        /// <summary>
        /// 获取指定长度的字节数组
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="Index">已经解析掉的数据长度</param>
        /// <returns></returns>
        public static byte[] Getbytes(byte[] buffer, int DataLen, ref int Index)
        {
            byte[] rs = new byte[DataLen];
            try
            {
                rs = buffer.Skip(Index).Take(DataLen).ToArray();
                Index = Index + 1;
            }
            catch (Exception ex)
            {

            }
            return rs;
        }


    }
}
