using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransInstruction
{

    public class ByteDataAnalysis
    {
        public static bool Getbool(byte[] buffer, ref int Index)
        {
            if (buffer.Length <= Index)
                throw new InvalidOperationException("Buffer underflow.");

            bool b = buffer[Index++] == 1;
            return b;
        }

        public static int GetInt(byte[] buffer, ref int Index)
        {
            if (buffer.Length < Index + 4)
                throw new InvalidOperationException("Buffer underflow.");

            int value = BitConverter.ToInt32(buffer, Index);
            Index += 4;
            return value;
        }

        public static int GetInt(byte[] buffer, out byte[] UnparsedData, ref int Index)
        {
            // 检查是否有足够的数据来读取一个int
            if (buffer.Length < Index + 4)
            {
                UnparsedData = Array.Empty<byte>(); // 没有足够的数据，返回空数组
                return 0; // 或者抛出异常，取决于你的错误处理策略
            }

            // 读取int值
            int value = BitConverter.ToInt32(buffer, Index);
            Index += 4; // 更新索引位置

            // 获取剩余的未解析数据
            UnparsedData = new byte[buffer.Length - Index];
            if (UnparsedData.Length > 0)
            {
                Array.Copy(buffer, Index, UnparsedData, 0, UnparsedData.Length);
            }

            return value;
        }

        public static Int64 GetInt64(byte[] buffer, ref int Index)
        {
            if (buffer.Length < Index + 8)
                throw new InvalidOperationException("Buffer underflow.");

            Int64 value = BitConverter.ToInt64(buffer, Index);
            Index += 8;
            return value;
        }

        public static float GetFloat(byte[] buffer, ref int Index)
        {
            if (buffer.Length < Index + 4)
                throw new InvalidOperationException("Buffer underflow.");

            float value = BitConverter.ToSingle(buffer, Index);
            Index += 4;
            return value;
        }

        public static Int16 GetInt16(byte[] buffer, ref int Index)
        {
            if (buffer.Length < Index + 2)
                throw new InvalidOperationException("Buffer underflow.");

            Int16 value = BitConverter.ToInt16(buffer, Index);
            Index += 2;
            return value;
        }

        public static byte GetByte(byte[] buffer, ref int Index)
        {
            if (buffer.Length <= Index)
                throw new InvalidOperationException("Buffer underflow.");

            byte value = buffer[Index++];
            return value;
        }

        public static byte[] GetBytes(byte[] buffer, int DataLen, ref int Index)
        {
            if (buffer.Length < Index + DataLen)
                throw new InvalidOperationException("Buffer underflow.");

            byte[] bytes = new byte[DataLen];
            Array.Copy(buffer, Index, bytes, 0, DataLen);
            Index += DataLen;
            return bytes;
        }

        public static string GetString(byte[] buffer, ref int Index)
        {
            if (buffer == null)
            {
                return string.Empty;
            }
            if (buffer.Length < Index + 4)
                throw new InvalidOperationException("Buffer underflow.");

            //byte[] lenByte = new byte[4];
            //lenByte = buffer.Skip(Index).Take(4).ToArray();
            //int len = BitConverter.ToInt32(lenByte, 0);

            int len = BitConverter.ToInt32(buffer, Index);
            Index += 4;

            if (buffer.Length < Index + len)
                throw new InvalidOperationException("Buffer underflow.");

            byte[] msg = new byte[len];
            Array.Copy(buffer, Index, msg, 0, len);
            Index += len;
            Index += 1;//减1是去掉结束符,这里要加上去
            return Encoding.UTF8.GetString(msg);
        }

        public static string GetString(byte[] buffer, out byte[] UnparsedData, ref int Index)
        {
            if (buffer.Length < Index + 4)
                throw new InvalidOperationException("Buffer underflow.");

            int len = BitConverter.ToInt32(buffer, Index);
            Index += 4;

            if (buffer.Length < Index + len)
                throw new InvalidOperationException("Buffer underflow.");

            byte[] msg = new byte[len];
            Array.Copy(buffer, Index, msg, 0, len);
            Index += len;
            Index += 1;//减1是去掉结束符,这里要加上去
            UnparsedData = new byte[buffer.Length - Index];
            Array.Copy(buffer, Index, UnparsedData, 0, UnparsedData.Length);

            return Encoding.UTF8.GetString(msg);
        }

        public static int TryGetInt(byte[] buffer, out bool success, ref int Index)
        {
            success = true;
            if (buffer.Length < Index + 4)
            {
                success = false;
                return 0;
            }

            int value = BitConverter.ToInt32(buffer, Index);
            Index += 4;
            return value;
        }

        public static string TryGetString(byte[] buffer, out bool success, ref int Index)
        {
            success = false;
            if (buffer.Length < Index + 4)
                return null;

            int len = BitConverter.ToInt32(buffer, Index);
            Index += 4;

            if (buffer.Length < Index + len)
                return null;

            byte[] msg = new byte[len];
            Array.Copy(buffer, Index, msg, 0, len);
            Index += len;

            success = true;
            return Encoding.UTF8.GetString(msg);
        }
    }


    /// <summary>
    /// 工具类 想法是减少对包的解析过程的复杂度
    /// </summary>
    public class ByteDataAnalysisold
    {
        /*
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
                */
        public static bool Getbool(byte[] buffer, ref int Index)
        {
            if (buffer.Length < Index + 1)
            {
                return false; // 或者抛出异常
            }

            bool b = buffer[Index] == 1;
            Index++;
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
                //if (len > 255)
                //{
                //    return "";
                //}
                //消息长度？ 暂时用下面直接减
                Index = Index + 4;

                byte[] msg = new byte[len];
                //msg.Length-1减1是去掉结束符
                msg = source.Skip(Index).Take(msg.Length - 1).ToArray();
                //                rs = System.Text.Encoding.GetEncoding("GB2312").GetString(msg);
                rs = System.Text.Encoding.UTF8.GetString(msg);
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
        public static string GetString(byte[] buffer, out byte[] UnparsedData, ref int Index)
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
                //rs = System.Text.Encoding.GetEncoding("GB2312").GetString(msg);
                rs = System.Text.Encoding.UTF8.GetString(msg);
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



        /*
        /// <summary>
        /// 解析字符串不超过255字节 ，为什么？忘记了
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="Index"></param>
        /// <returns></returns>
        public static string GetString(byte[] buffer, ref int Index)
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
                //if (len > 255)
                //{
                //    return "";
                //}
                //消息长度？ 暂时用下面直接减
                Index = Index + 4;

                byte[] msg = new byte[len];
                //msg.Length-1减1是去掉结束符
                msg = buffer.Skip(Index).Take(msg.Length - 1).ToArray();
                //rs = System.Text.Encoding.GetEncoding("GB2312").GetString(msg);
                rs = System.Text.Encoding.UTF8.GetString(msg);
                Index = Index + msg.Length + 1;//减1是去掉结束符,这里要加上去
            }
            catch (Exception ex)
            {

            }
            return rs;
        }

        */

        public static string GetString(byte[] buffer, ref int Index)
        {
            if (buffer.Length < Index + 4)
            {
                return string.Empty; // 或者抛出异常，取决于你的错误处理策略
            }

            int len = BitConverter.ToInt32(buffer, Index);
            Index += 4; // 移动索引，跳过长度字段

            if (buffer.Length < Index + len)
            {
                return string.Empty; // 或者抛出异常
            }

            byte[] msg = new byte[len];
            Buffer.BlockCopy(buffer, Index, msg, 0, len);
            Index += len; // 移动索引，跳过消息体

            return System.Text.Encoding.UTF8.GetString(msg);
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


        /*
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
        */

        public static int GetInt(byte[] buffer, ref int Index)
        {
            if (buffer.Length < Index + 4)
            {
                return 0; // 或者抛出异常
            }

            int value = BitConverter.ToInt32(buffer, Index);
            Index += 4;
            return value;
        }


        public static Int64 GetInt64(byte[] buffer, ref int Index)
        {
            if (buffer.Length < Index + 8)
            {
                // 如果剩余的缓冲区长度不足以读取一个 Int64，返回 0 或者抛出异常
                return 0; // 或者抛出异常，取决于你的错误处理策略
            }

            Int64 value = BitConverter.ToInt64(buffer, Index);
            Index += 8; // 移动索引，跳过 8 个字节的 Int64 字段
            return value;
        }

        //public static Int64 GetInt64(byte[] buffer, ref int Index)
        //{
        //    Int64 y = 0;
        //    try
        //    {
        //        byte[] sz = new byte[8];
        //        sz = buffer.Skip(Index).Take(8).ToArray();
        //        y = BitConverter.ToInt64(sz, 0);
        //        Index = Index + 8;
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return y;
        //}

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
