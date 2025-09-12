using System;
using System.Text;

namespace RUINORERP.PacketSpec.Serialization
{
    /// <summary>
    /// 字节操作工具类
    /// </summary>
    public static class ByteHelper
    {
        /// <summary>
        /// 默认编码格式
        /// </summary>
        public static readonly Encoding DefaultEncoding = Encoding.UTF8;

        /// <summary>
        /// 从字节数组获取整数
        /// </summary>
        public static int GetInt(byte[] data, ref int index)
        {
            if (data == null || index + 4 > data.Length)
                throw new ArgumentException("数据长度不足");

            int value = BitConverter.ToInt32(data, index);
            index += 4;
            return value;
        }

        /// <summary>
        /// 从字节数组获取短整数
        /// </summary>
        public static short GetShort(byte[] data, ref int index)
        {
            if (data == null || index + 2 > data.Length)
                throw new ArgumentException("数据长度不足");

            short value = BitConverter.ToInt16(data, index);
            index += 2;
            return value;
        }

        /// <summary>
        /// 从字节数组获取字节
        /// </summary>
        public static byte GetByte(byte[] data, ref int index)
        {
            if (data == null || index >= data.Length)
                throw new ArgumentException("数据长度不足");

            return data[index++];
        }

        /// <summary>
        /// 从字节数组获取字符串
        /// </summary>
        public static string GetString(byte[] data, ref int index, int length)
        {
            if (data == null || index + length > data.Length)
                throw new ArgumentException("数据长度不足");

            string value = DefaultEncoding.GetString(data, index, length);
            index += length;
            return value;
        }

        /// <summary>
        /// 从字节数组获取字符串（以null结尾）
        /// </summary>
        public static string GetNullTerminatedString(byte[] data, ref int index)
        {
            if (data == null || index >= data.Length)
                return string.Empty;

            int start = index;
            while (index < data.Length && data[index] != 0)
            {
                index++;
            }

            int length = index - start;
            string value = DefaultEncoding.GetString(data, start, length);
            
            // 跳过null终止符
            if (index < data.Length && data[index] == 0)
                index++;

            return value;
        }

        /// <summary>
        /// 将整数写入字节数组
        /// </summary>
        public static void WriteInt(byte[] buffer, ref int index, int value)
        {
            if (buffer == null || index + 4 > buffer.Length)
                throw new ArgumentException("缓冲区空间不足");

            byte[] bytes = BitConverter.GetBytes(value);
            Buffer.BlockCopy(bytes, 0, buffer, index, 4);
            index += 4;
        }

        /// <summary>
        /// 将短整数写入字节数组
        /// </summary>
        public static void WriteShort(byte[] buffer, ref int index, short value)
        {
            if (buffer == null || index + 2 > buffer.Length)
                throw new ArgumentException("缓冲区空间不足");

            byte[] bytes = BitConverter.GetBytes(value);
            Buffer.BlockCopy(bytes, 0, buffer, index, 2);
            index += 2;
        }

        /// <summary>
        /// 将字节写入字节数组
        /// </summary>
        public static void WriteByte(byte[] buffer, ref int index, byte value)
        {
            if (buffer == null || index >= buffer.Length)
                throw new ArgumentException("缓冲区空间不足");

            buffer[index++] = value;
        }

        /// <summary>
        /// 将字符串写入字节数组
        /// </summary>
        public static void WriteString(byte[] buffer, ref int index, string value)
        {
            if (string.IsNullOrEmpty(value))
                return;

            byte[] bytes = DefaultEncoding.GetBytes(value);
            if (index + bytes.Length > buffer.Length)
                throw new ArgumentException("缓冲区空间不足");

            Buffer.BlockCopy(bytes, 0, buffer, index, bytes.Length);
            index += bytes.Length;
        }

        /// <summary>
        /// 将字符串写入字节数组（以null结尾）
        /// </summary>
        public static void WriteNullTerminatedString(byte[] buffer, ref int index, string value)
        {
            WriteString(buffer, ref index, value);
            WriteByte(buffer, ref index, 0); // 写入null终止符
        }

        /// <summary>
        /// 计算字节数组的校验和
        /// </summary>
        public static byte CalculateChecksum(byte[] data)
        {
            if (data == null || data.Length == 0)
                return 0;

            byte checksum = 0;
            foreach (byte b in data)
            {
                checksum ^= b; // 简单的异或校验
            }
            return checksum;
        }

        /// <summary>
        /// 验证字节数组的校验和
        /// </summary>
        public static bool ValidateChecksum(byte[] data, byte expectedChecksum)
        {
            return CalculateChecksum(data) == expectedChecksum;
        }

        /// <summary>
        /// 字节数组转换为十六进制字符串
        /// </summary>
        public static string ToHexString(byte[] data)
        {
            if (data == null)
                return string.Empty;

            return BitConverter.ToString(data).Replace("-", " ");
        }

        /// <summary>
        /// 十六进制字符串转换为字节数组
        /// </summary>
        public static byte[] FromHexString(string hexString)
        {
            if (string.IsNullOrEmpty(hexString))
                return Array.Empty<byte>();

            hexString = hexString.Replace(" ", "").Replace("-", "");
            
            if (hexString.Length % 2 != 0)
                throw new ArgumentException("十六进制字符串长度必须为偶数");

            byte[] bytes = new byte[hexString.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }
            return bytes;
        }

        /// <summary>
        /// 比较两个字节数组是否相等
        /// </summary>
        public static bool ByteArrayEquals(byte[] a, byte[] b)
        {
            if (a == null && b == null)
                return true;
            if (a == null || b == null)
                return false;
            if (a.Length != b.Length)
                return false;

            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i])
                    return false;
            }
            return true;
        }
    }
}