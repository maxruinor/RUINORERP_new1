using System;

namespace TransInstruction
{

    /// <summary>
    /// 其中数据类型转换不一定是适应其他。主要是为了解析PushInt这个类中的操作反操作
    /// </summary>
    public static class DataConversion
    {

        #region float bytes



        #endregion
        /// <summary>
        /// 得到4个字节
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] int2Bytes(int value)
        {
            byte[] src = new byte[4];
            src[3] = (byte)((value >> 24) & 0xFF);
            src[2] = (byte)((value >> 16) & 0xFF);
            src[1] = (byte)((value >> 8) & 0xFF);
            src[0] = (byte)(value & 0xFF);
            return src;
        }


        /// <summary>
        /// byte数组长度为4, bytes[3]为高8位
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static int bytes2Int(byte[] bytes)
        {
            int value = 0;
            value = ((bytes[3] & 0xff) << 24) |
                    ((bytes[2] & 0xff) << 16) |
                    ((bytes[1] & 0xff) << 8) |
                    (bytes[0] & 0xff);
            return value;
        }

        /*
        int i = 123;
        byte[] intBuff = BitConverter.GetBytes(i);     // 将 int 转换成字节数组
        lob.Write(intBuff, 0, 4);
i = BitConverter.ToInt32(intBuff, 0);           // 从字节数组转换成 int

double x = 123.456;
        byte[] doubleBuff = BitConverter.GetBytes(x);  // 将 double 转换成字节数组
        lob.Write(doubleBuff, 0, 8);
x = BitConverter.ToDouble(doubleBuff, 0);       // 从字节数组转换成 double
        */
        public static void Int16ToByte(Int16[] arrInt16, int nInt16Count, ref Byte[] destByteArr)
        {
            //遵守X86规则，低字节放在前面，高字节放在后面
            for (int i = 0; i < nInt16Count; i++)
            {
                destByteArr[2 * i + 0] = Convert.ToByte((arrInt16[i] & 0x00FF));
                destByteArr[2 * i + 1] = Convert.ToByte((arrInt16[i] & 0xFF00) >> 8);
            }
        }

        /// <summary>
        /// 因为可能多个int16
        /// </summary>
        /// <param name="arrByte"></param>
        /// <param name="nByteCount"></param>
        /// <param name="destInt16Arr"></param>
        public static void ByteToInt16s(Byte[] arrByte, int nByteCount, ref Int16[] destInt16Arr)
        {
            int i = 0;
            try
            {
                //按两个字节一个整数解析，前一字节当做整数低位，后一字节当做整数高位，调用系统函数转化
                for (i = 0; i < nByteCount / 2; i++)
                {
                    Byte[] tmpBytes = new Byte[2] { arrByte[2 * i + 0], arrByte[2 * i + 1] };
                    destInt16Arr[i] = BitConverter.ToInt16(tmpBytes, 0);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }




    }
}
