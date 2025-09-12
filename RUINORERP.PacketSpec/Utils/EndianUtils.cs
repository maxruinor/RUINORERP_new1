using System;

namespace RUINORERP.PacketSpec.Utils
{
    /// <summary>
    /// 字节序处理工具类
    /// </summary>
    public static class EndianUtils
    {
        /// <summary>
        /// 将整数转换为小端序字节数组
        /// </summary>
        public static byte[] ToLittleEndianBytes(int value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }
            return bytes;
        }

        /// <summary>
        /// 将小端序字节数组转换为整数
        /// </summary>
        public static int FromLittleEndianBytes(byte[] bytes, int startIndex = 0)
        {
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));
                
            if (startIndex + 4 > bytes.Length)
                throw new ArgumentException("Not enough bytes for integer conversion");

            byte[] tempBytes = new byte[4];
            Array.Copy(bytes, startIndex, tempBytes, 0, 4);
            
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(tempBytes);
            }
            
            return BitConverter.ToInt32(tempBytes, 0);
        }

        /// <summary>
        /// 写入小端序整数到缓冲区
        /// </summary>
        public static void WriteLittleEndian(int value, byte[] buffer, int offset)
        {
            byte[] bytes = ToLittleEndianBytes(value);
            Buffer.BlockCopy(bytes, 0, buffer, offset, 4);
        }

        /// <summary>
        /// 确保字节序正确（如果需要转换）
        /// </summary>
        public static void EnsureLittleEndian(byte[] data)
        {
            if (!BitConverter.IsLittleEndian && data != null)
            {
                Array.Reverse(data);
            }
        }

        /// <summary>
        /// 交换字节序
        /// </summary>
        public static void SwapEndian(byte[] data)
        {
            if (data != null)
            {
                Array.Reverse(data);
            }
        }
    }
}