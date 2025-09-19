using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RUINORERP.Server.Network.Core
{
    /// <summary>
    /// ✅ [统一架构] 数据处理器 - 整合所有数据处理功能
    /// 提供字符串与字节数组转换、数值类型与字节数组转换等数据处理功能
    /// </summary>
    public class UnifiedDataProcessor
    {
        #region 字符串与字节数组转换

        /// <summary>
        /// 字符串转换为字节数组（UTF8编码）
        /// </summary>
        public static byte[] StringToBytes(string value)
        {
            return value != null ? Encoding.UTF8.GetBytes(value) : Array.Empty<byte>();
        }

        /// <summary>
        /// 字节数组转换为字符串（UTF8编码）
        /// </summary>
        public static string BytesToString(byte[] bytes)
        {
            return bytes != null && bytes.Length > 0 ? Encoding.UTF8.GetString(bytes) : string.Empty;
        }

        /// <summary>
        /// 字节数组转换为字符串（指定编码）
        /// </summary>
        public static string BytesToString(byte[] bytes, Encoding encoding)
        {
            return bytes != null && bytes.Length > 0 ? encoding.GetString(bytes) : string.Empty;
        }

        /// <summary>
        /// 从字节数组指定位置获取字符串
        /// </summary>
        public static string GetStringFromBytes(byte[] bytes, int startIndex, int length)
        {
            if (bytes == null || startIndex < 0 || length <= 0 || startIndex + length > bytes.Length)
                return string.Empty;

            return Encoding.UTF8.GetString(bytes, startIndex, length);
        }

        #endregion

        #region 数值类型与字节数组转换

        /// <summary>
        /// 32位整数转换为字节数组（小端序）
        /// </summary>
        public static byte[] Int32ToBytes(int value)
        {
            byte[] bytes = new byte[4];
            BinaryPrimitives.WriteInt32LittleEndian(bytes, value);
            return bytes;
        }

        /// <summary>
        /// 字节数组转换为32位整数（小端序）
        /// </summary>
        public static int BytesToInt32(byte[] bytes, int startIndex = 0)
        {
            if (bytes == null || startIndex < 0 || startIndex + 4 > bytes.Length)
                return 0;

            return BinaryPrimitives.ReadInt32LittleEndian(bytes.AsSpan(startIndex));
        }

        /// <summary>
        /// 64位整数转换为字节数组（小端序）
        /// </summary>
        public static byte[] Int64ToBytes(long value)
        {
            byte[] bytes = new byte[8];
            BinaryPrimitives.WriteInt64LittleEndian(bytes, value);
            return bytes;
        }

        /// <summary>
        /// 字节数组转换为64位整数（小端序）
        /// </summary>
        public static long BytesToInt64(byte[] bytes, int startIndex = 0)
        {
            if (bytes == null || startIndex < 0 || startIndex + 8 > bytes.Length)
                return 0;

            return BinaryPrimitives.ReadInt64LittleEndian(bytes.AsSpan(startIndex));
        }

        /// <summary>
        /// 16位整数转换为字节数组（小端序）
        /// </summary>
        public static byte[] Int16ToBytes(short value)
        {
            byte[] bytes = new byte[2];
            BinaryPrimitives.WriteInt16LittleEndian(bytes, value);
            return bytes;
        }

        /// <summary>
        /// 字节数组转换为16位整数（小端序）
        /// </summary>
        public static short BytesToInt16(byte[] bytes, int startIndex = 0)
        {
            if (bytes == null || startIndex < 0 || startIndex + 2 > bytes.Length)
                return 0;

            return BinaryPrimitives.ReadInt16LittleEndian(bytes.AsSpan(startIndex));
        }

        /// <summary>
        /// 单精度浮点数转换为字节数组
        /// </summary>
        public static byte[] FloatToBytes(float value)
        {
            byte[] bytes = new byte[4];
            BinaryPrimitives.WriteSingleLittleEndian(bytes, value);
            return bytes;
        }

        /// <summary>
        /// 字节数组转换为单精度浮点数
        /// </summary>
        public static float BytesToFloat(byte[] bytes, int startIndex = 0)
        {
            if (bytes == null || startIndex < 0 || startIndex + 4 > bytes.Length)
                return 0;

            return BinaryPrimitives.ReadSingleLittleEndian(bytes.AsSpan(startIndex));
        }

        /// <summary>
        /// 双精度浮点数转换为字节数组
        /// </summary>
        public static byte[] DoubleToBytes(double value)
        {
            byte[] bytes = new byte[8];
            BinaryPrimitives.WriteDoubleLittleEndian(bytes, value);
            return bytes;
        }

        /// <summary>
        /// 字节数组转换为双精度浮点数
        /// </summary>
        public static double BytesToDouble(byte[] bytes, int startIndex = 0)
        {
            if (bytes == null || startIndex < 0 || startIndex + 8 > bytes.Length)
                return 0;

            return BinaryPrimitives.ReadDoubleLittleEndian(bytes.AsSpan(startIndex));
        }

        #endregion

        #region 字节数组操作

        /// <summary>
        /// 合并多个字节数组
        /// </summary>
        public static byte[] CombineBytes(params byte[][] arrays)
        {
            if (arrays == null || arrays.Length == 0)
                return Array.Empty<byte>();

            int totalLength = arrays.Sum(arr => arr?.Length ?? 0);
            byte[] result = new byte[totalLength];
            int offset = 0;

            foreach (byte[] array in arrays)
            {
                if (array != null && array.Length > 0)
                {
                    Buffer.BlockCopy(array, 0, result, offset, array.Length);
                    offset += array.Length;
                }
            }

            return result;
        }

        /// <summary>
        /// 从字节数组中提取子数组
        /// </summary>
        public static byte[] GetSubBytes(byte[] source, int startIndex, int length)
        {
            if (source == null || startIndex < 0 || length <= 0 || startIndex + length > source.Length)
                return Array.Empty<byte>();

            byte[] result = new byte[length];
            Buffer.BlockCopy(source, startIndex, result, 0, length);
            return result;
        }

        /// <summary>
        /// 反转字节序
        /// </summary>
        public static byte[] ReverseBytes(byte[] bytes)
        {
            if (bytes == null)
                return Array.Empty<byte>();

            byte[] reversed = new byte[bytes.Length];
            Array.Copy(bytes, reversed, bytes.Length);
            Array.Reverse(reversed);
            return reversed;
        }

        /// <summary>
        /// 比较两个字节数组是否相等
        /// </summary>
        public static bool BytesEqual(byte[] a, byte[] b)
        {
            if (a == null && b == null) return true;
            if (a == null || b == null) return false;
            if (a.Length != b.Length) return false;

            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i]) return false;
            }

            return true;
        }

        #endregion

        #region 字节数组读取功能

        /// <summary>
        /// 从字节数组中获取字符串（UTF8编码）
        /// </summary>
        public static string GetString(byte[] bytes, int startIndex, int length)
        {
            if (bytes == null || startIndex < 0 || length <= 0 || startIndex + length > bytes.Length)
                return string.Empty;

            return Encoding.UTF8.GetString(bytes, startIndex, length);
        }

        /// <summary>
        /// 从字节数组中获取32位整数
        /// </summary>
        public static int GetInt32(byte[] bytes, int startIndex = 0)
        {
            return BytesToInt32(bytes, startIndex);
        }

        /// <summary>
        /// 从字节数组中获取64位整数
        /// </summary>
        public static long GetInt64(byte[] bytes, int startIndex = 0)
        {
            return BytesToInt64(bytes, startIndex);
        }

        /// <summary>
        /// 从字节数组中获取16位整数
        /// </summary>
        public static short GetInt16(byte[] bytes, int startIndex = 0)
        {
            return BytesToInt16(bytes, startIndex);
        }

        /// <summary>
        /// 从字节数组中获取单精度浮点数
        /// </summary>
        public static float GetFloat(byte[] bytes, int startIndex = 0)
        {
            return BytesToFloat(bytes, startIndex);
        }

        /// <summary>
        /// 从字节数组中获取双精度浮点数
        /// </summary>
        public static double GetDouble(byte[] bytes, int startIndex = 0)
        {
            return BytesToDouble(bytes, startIndex);
        }

        /// <summary>
        /// 从字节数组中获取布尔值
        /// </summary>
        public static bool GetBoolean(byte[] bytes, int startIndex = 0)
        {
            if (bytes == null || startIndex < 0 || startIndex >= bytes.Length)
                return false;

            return bytes[startIndex] != 0;
        }

        /// <summary>
        /// 从字节数组中获取字节
        /// </summary>
        public static byte GetByte(byte[] bytes, int startIndex = 0)
        {
            if (bytes == null || startIndex < 0 || startIndex >= bytes.Length)
                return 0;

            return bytes[startIndex];
        }

        #endregion

        #region 高级数据处理功能

        /// <summary>
        /// 将对象序列化为字节数组
        /// </summary>
        public static byte[] SerializeObject(object obj)
        {
            if (obj == null)
                return Array.Empty<byte>();

            try
            {
                string json = System.Text.Json.JsonSerializer.Serialize(obj);
                return Encoding.UTF8.GetBytes(json);
            }
            catch
            {
                return Array.Empty<byte>();
            }
        }

        /// <summary>
        /// 从字节数组反序列化对象
        /// </summary>
        public static T DeserializeObject<T>(byte[] bytes) where T : class
        {
            if (bytes == null || bytes.Length == 0)
                return null;

            try
            {
                string json = Encoding.UTF8.GetString(bytes);
                return System.Text.Json.JsonSerializer.Deserialize<T>(json);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 计算字节数组的CRC32校验和
        /// </summary>
        public static uint CalculateCrc32(byte[] data)
        {
            if (data == null || data.Length == 0)
                return 0;

            const uint polynomial = 0xEDB88320;
            uint crc = 0xFFFFFFFF;

            foreach (byte b in data)
            {
                crc ^= b;
                for (int i = 0; i < 8; i++)
                {
                    if ((crc & 1) != 0)
                        crc = (crc >> 1) ^ polynomial;
                    else
                        crc >>= 1;
                }
            }

            return crc ^ 0xFFFFFFFF;
        }

        /// <summary>
        /// 计算字节数组的MD5哈希值
        /// </summary>
        public static string CalculateMd5(byte[] data)
        {
            if (data == null || data.Length == 0)
                return string.Empty;

            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(data);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
            }
        }

        #endregion
    }
}