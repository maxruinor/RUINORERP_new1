using System.Buffers;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;

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
        /// <param name="value">要转换的字符串</param>
        /// <returns>转换后的字节数组，如果输入为null则返回空数组</returns>
        public static byte[] StringToBytes(string value)
        {
            return value != null ? Encoding.UTF8.GetBytes(value) : Array.Empty<byte>();
        }

        /// <summary>
        /// 字节数组转换为字符串（UTF8编码）
        /// </summary>
        /// <param name="bytes">要转换的字节数组</param>
        /// <returns>转换后的字符串，如果输入为null或空则返回空字符串</returns>
        public static string BytesToString(byte[] bytes)
        {
            return bytes != null && bytes.Length > 0 ? Encoding.UTF8.GetString(bytes) : string.Empty;
        }

        /// <summary>
        /// 字节数组转换为字符串（指定编码）
        /// </summary>
        /// <param name="bytes">要转换的字节数组</param>
        /// <param name="encoding">字符编码</param>
        /// <returns>转换后的字符串，如果输入为null、空或编码为null则返回空字符串</returns>
        public static string BytesToString(byte[] bytes, Encoding encoding)
        {
            if (bytes == null || bytes.Length == 0 || encoding == null)
                return string.Empty;
            return encoding.GetString(bytes);
        }

        /// <summary>
        /// 从字节数组指定位置获取字符串
        /// </summary>
        /// <param name="bytes">源字节数组</param>
        /// <param name="startIndex">起始索引</param>
        /// <param name="length">要读取的字节长度</param>
        /// <returns>读取的字符串，如果参数无效则返回空字符串</returns>
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
        /// <param name="value">要转换的整数</param>
        /// <returns>转换后的字节数组</returns>
        public static byte[] Int32ToBytes(int value)
        {
            // 使用ArrayPool优化内存分配
            byte[] bytes = ArrayPool<byte>.Shared.Rent(4);
            BinaryPrimitives.WriteInt32LittleEndian(bytes, value);
            byte[] result = new byte[4];
            Buffer.BlockCopy(bytes, 0, result, 0, 4);
            ArrayPool<byte>.Shared.Return(bytes);
            return result;
        }

        /// <summary>
        /// 字节数组转换为32位整数（小端序）
        /// </summary>
        /// <param name="bytes">源字节数组</param>
        /// <param name="startIndex">起始索引</param>
        /// <returns>转换后的整数，如果参数无效则返回0</returns>
        public static int BytesToInt32(byte[] bytes, int startIndex = 0)
        {
            if (bytes == null || startIndex < 0 || startIndex + 4 > bytes.Length)
                return 0;
            return BinaryPrimitives.ReadInt32LittleEndian(bytes.AsSpan(startIndex));
        }

        /// <summary>
        /// 64位整数转换为字节数组（小端序）
        /// </summary>
        /// <param name="value">要转换的长整数</param>
        /// <returns>转换后的字节数组</returns>
        public static byte[] Int64ToBytes(long value)
        {
            // 使用ArrayPool优化内存分配
            byte[] bytes = ArrayPool<byte>.Shared.Rent(8);
            BinaryPrimitives.WriteInt64LittleEndian(bytes, value);
            byte[] result = new byte[8];
            Buffer.BlockCopy(bytes, 0, result, 0, 8);
            ArrayPool<byte>.Shared.Return(bytes);
            return result;
        }

        /// <summary>
        /// 字节数组转换为64位整数（小端序）
        /// </summary>
        /// <param name="bytes">源字节数组</param>
        /// <param name="startIndex">起始索引</param>
        /// <returns>转换后的长整数，如果参数无效则返回0</returns>
        public static long BytesToInt64(byte[] bytes, int startIndex = 0)
        {
            if (bytes == null || startIndex < 0 || startIndex + 8 > bytes.Length)
                return 0;
            return BinaryPrimitives.ReadInt64LittleEndian(bytes.AsSpan(startIndex));
        }

        /// <summary>
        /// 16位整数转换为字节数组（小端序）
        /// </summary>
        /// <param name="value">要转换的短整数</param>
        /// <returns>转换后的字节数组</returns>
        public static byte[] Int16ToBytes(short value)
        {
            // 使用ArrayPool优化内存分配
            byte[] bytes = ArrayPool<byte>.Shared.Rent(2);
            BinaryPrimitives.WriteInt16LittleEndian(bytes, value);
            byte[] result = new byte[2];
            Buffer.BlockCopy(bytes, 0, result, 0, 2);
            ArrayPool<byte>.Shared.Return(bytes);
            return result;
        }

        /// <summary>
        /// 字节数组转换为16位整数（小端序）
        /// </summary>
        /// <param name="bytes">源字节数组</param>
        /// <param name="startIndex">起始索引</param>
        /// <returns>转换后的短整数，如果参数无效则返回0</returns>
        public static short BytesToInt16(byte[] bytes, int startIndex = 0)
        {
            if (bytes == null || startIndex < 0 || startIndex + 2 > bytes.Length)
                return 0;
            return BinaryPrimitives.ReadInt16LittleEndian(bytes.AsSpan(startIndex));
        }

        /// <summary>
        /// 单精度浮点数转换为字节数组
        /// </summary>
        /// <param name="value">要转换的浮点数</param>
        /// <returns>转换后的字节数组</returns>
        public static byte[] FloatToBytes(float value)
        {
            // 使用ArrayPool优化内存分配
            byte[] bytes = ArrayPool<byte>.Shared.Rent(4);
            BinaryPrimitives.WriteSingleLittleEndian(bytes, value);
            byte[] result = new byte[4];
            Buffer.BlockCopy(bytes, 0, result, 0, 4);
            ArrayPool<byte>.Shared.Return(bytes);
            return result;
        }

        /// <summary>
        /// 字节数组转换为单精度浮点数
        /// </summary>
        /// <param name="bytes">源字节数组</param>
        /// <param name="startIndex">起始索引</param>
        /// <returns>转换后的浮点数，如果参数无效则返回0</returns>
        public static float BytesToFloat(byte[] bytes, int startIndex = 0)
        {
            if (bytes == null || startIndex < 0 || startIndex + 4 > bytes.Length)
                return 0;
            return BinaryPrimitives.ReadSingleLittleEndian(bytes.AsSpan(startIndex));
        }

        /// <summary>
        /// 双精度浮点数转换为字节数组
        /// </summary>
        /// <param name="value">要转换的双精度浮点数</param>
        /// <returns>转换后的字节数组</returns>
        public static byte[] DoubleToBytes(double value)
        {
            // 使用ArrayPool优化内存分配
            byte[] bytes = ArrayPool<byte>.Shared.Rent(8);
            BinaryPrimitives.WriteDoubleLittleEndian(bytes, value);
            byte[] result = new byte[8];
            Buffer.BlockCopy(bytes, 0, result, 0, 8);
            ArrayPool<byte>.Shared.Return(bytes);
            return result;
        }

        /// <summary>
        /// 字节数组转换为双精度浮点数
        /// </summary>
        /// <param name="bytes">源字节数组</param>
        /// <param name="startIndex">起始索引</param>
        /// <returns>转换后的双精度浮点数，如果参数无效则返回0</returns>
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
        /// <param name="arrays">要合并的字节数组集合</param>
        /// <returns>合并后的字节数组，如果输入为null或空则返回空数组</returns>
        public static byte[] CombineBytes(params byte[][] arrays)
        {
            if (arrays == null || arrays.Length == 0)
                return Array.Empty<byte>();

            int totalLength = arrays.Sum(arr => arr?.Length ?? 0);
            if (totalLength == 0)
                return Array.Empty<byte>();

            // 使用ArrayPool优化内存分配
            byte[] result = ArrayPool<byte>.Shared.Rent(totalLength);
            int offset = 0;
            try
            {
                foreach (byte[] array in arrays)
                {
                    if (array != null && array.Length > 0)
                    {
                        Buffer.BlockCopy(array, 0, result, offset, array.Length);
                        offset += array.Length;
                    }
                }

                // 创建正确大小的结果数组
                byte[] finalResult = new byte[offset];
                Buffer.BlockCopy(result, 0, finalResult, 0, offset);
                return finalResult;
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(result);
            }
        }

        /// <summary>
        /// 从字节数组中提取子数组
        /// </summary>
        /// <param name="source">源字节数组</param>
        /// <param name="startIndex">起始索引</param>
        /// <param name="length">要提取的长度</param>
        /// <returns>提取的子数组，如果参数无效则返回空数组</returns>
        public static byte[] GetSubBytes(byte[] source, int startIndex, int length)
        {
            if (source == null || startIndex < 0 || length <= 0 || startIndex + length > source.Length)
                return Array.Empty<byte>();

            // 使用ArrayPool优化内存分配
            byte[] result = ArrayPool<byte>.Shared.Rent(length);
            Buffer.BlockCopy(source, startIndex, result, 0, length);
            
            // 创建正确大小的结果数组
            byte[] finalResult = new byte[length];
            Buffer.BlockCopy(result, 0, finalResult, 0, length);
            ArrayPool<byte>.Shared.Return(result);
            return finalResult;
        }

        /// <summary>
        /// 反转字节序
        /// </summary>
        /// <param name="bytes">要反转的字节数组</param>
        /// <returns>反转后的字节数组，如果输入为null则返回空数组</returns>
        public static byte[] ReverseBytes(byte[] bytes)
        {
            if (bytes == null)
                return Array.Empty<byte>();

            // 使用ArrayPool优化内存分配
            byte[] reversed = ArrayPool<byte>.Shared.Rent(bytes.Length);
            Buffer.BlockCopy(bytes, 0, reversed, 0, bytes.Length);
            Array.Reverse(reversed);
            
            // 创建正确大小的结果数组
            byte[] finalResult = new byte[bytes.Length];
            Buffer.BlockCopy(reversed, 0, finalResult, 0, bytes.Length);
            ArrayPool<byte>.Shared.Return(reversed);
            return finalResult;
        }

        /// <summary>
        /// 比较两个字节数组是否相等
        /// </summary>
        /// <param name="a">第一个字节数组</param>
        /// <param name="b">第二个字节数组</param>
        /// <returns>如果两个数组相等则返回true，否则返回false</returns>
        public static bool BytesEqual(byte[] a, byte[] b)
        {
            if (ReferenceEquals(a, b))
                return true;
            if (a == null || b == null)
                return false;
            if (a.Length != b.Length)
                return false;
            
            // 使用Span提高性能
            return a.AsSpan().SequenceEqual(b.AsSpan());
        }

        #endregion

        #region 字节数组读取功能

        /// <summary>
        /// 从字节数组中获取字符串（UTF8编码）
        /// </summary>
        /// <param name="bytes">源字节数组</param>
        /// <param name="startIndex">起始索引</param>
        /// <param name="length">要读取的字节长度</param>
        /// <returns>读取的字符串，如果参数无效则返回空字符串</returns>
        public static string GetString(byte[] bytes, int startIndex, int length)
        {
            if (bytes == null || startIndex < 0 || length <= 0 || startIndex + length > bytes.Length)
                return string.Empty;
            return Encoding.UTF8.GetString(bytes, startIndex, length);
        }

        /// <summary>
        /// 从字节数组中获取32位整数
        /// </summary>
        /// <param name="bytes">源字节数组</param>
        /// <param name="startIndex">起始索引</param>
        /// <returns>读取的整数，如果参数无效则返回0</returns>
        public static int GetInt32(byte[] bytes, int startIndex = 0)
        {
            return BytesToInt32(bytes, startIndex);
        }

        /// <summary>
        /// 从字节数组中获取64位整数
        /// </summary>
        /// <param name="bytes">源字节数组</param>
        /// <param name="startIndex">起始索引</param>
        /// <returns>读取的长整数，如果参数无效则返回0</returns>
        public static long GetInt64(byte[] bytes, int startIndex = 0)
        {
            return BytesToInt64(bytes, startIndex);
        }

        /// <summary>
        /// 从字节数组中获取16位整数
        /// </summary>
        /// <param name="bytes">源字节数组</param>
        /// <param name="startIndex">起始索引</param>
        /// <returns>读取的短整数，如果参数无效则返回0</returns>
        public static short GetInt16(byte[] bytes, int startIndex = 0)
        {
            return BytesToInt16(bytes, startIndex);
        }

        /// <summary>
        /// 从字节数组中获取单精度浮点数
        /// </summary>
        /// <param name="bytes">源字节数组</param>
        /// <param name="startIndex">起始索引</param>
        /// <returns>读取的浮点数，如果参数无效则返回0</returns>
        public static float GetFloat(byte[] bytes, int startIndex = 0)
        {
            return BytesToFloat(bytes, startIndex);
        }

        /// <summary>
        /// 从字节数组中获取双精度浮点数
        /// </summary>
        /// <param name="bytes">源字节数组</param>
        /// <param name="startIndex">起始索引</param>
        /// <returns>读取的双精度浮点数，如果参数无效则返回0</returns>
        public static double GetDouble(byte[] bytes, int startIndex = 0)
        {
            return BytesToDouble(bytes, startIndex);
        }

        /// <summary>
        /// 从字节数组中获取布尔值
        /// </summary>
        /// <param name="bytes">源字节数组</param>
        /// <param name="startIndex">起始索引</param>
        /// <returns>读取的布尔值，如果参数无效则返回false</returns>
        public static bool GetBoolean(byte[] bytes, int startIndex = 0)
        {
            if (bytes == null || startIndex < 0 || startIndex >= bytes.Length)
                return false;
            return bytes[startIndex] != 0;
        }

        /// <summary>
        /// 从字节数组中获取字节
        /// </summary>
        /// <param name="bytes">源字节数组</param>
        /// <param name="startIndex">起始索引</param>
        /// <returns>读取的字节，如果参数无效则返回0</returns>
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
        /// <param name="obj">要序列化的对象</param>
        /// <returns>序列化后的字节数组，如果输入为null或序列化失败则返回空数组</returns>
        public static byte[] SerializeObject(object obj)
        {
            if (obj == null)
                return Array.Empty<byte>();

            try
            {
                string json = System.Text.Json.JsonSerializer.Serialize(obj);
                return Encoding.UTF8.GetBytes(json);
            }
            catch (Exception ex)
            {
                // 可以在这里添加日志记录
                Console.WriteLine($"序列化对象失败: {ex.Message}");
                return Array.Empty<byte>();
            }
        }

        /// <summary>
        /// 从字节数组反序列化对象
        /// </summary>
        /// <typeparam name="T">目标对象类型</typeparam>
        /// <param name="bytes">源字节数组</param>
        /// <returns>反序列化后的对象，如果输入为null、空或反序列化失败则返回null</returns>
        public static T DeserializeObject<T>(byte[] bytes) where T : class
        {
            if (bytes == null || bytes.Length == 0)
                return null;

            try
            {
                string json = Encoding.UTF8.GetString(bytes);
                return System.Text.Json.JsonSerializer.Deserialize<T>(json);
            }
            catch (Exception ex)
            {
                // 可以在这里添加日志记录
                Console.WriteLine($"反序列化对象失败: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 计算字节数组的CRC32校验和
        /// </summary>
        /// <param name="data">源数据</param>
        /// <returns>CRC32校验和</returns>
        public static uint CalculateCrc32(byte[] data)
        {
            if (data == null || data.Length == 0)
                return 0;

            // 使用ArrayPool优化内存分配
            const uint polynomial = 0xEDB88320;
            uint crc = 0xFFFFFFFF;

            try
            {
                // 使用Span提高性能
                ReadOnlySpan<byte> span = data.AsSpan();
                foreach (byte b in span)
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
            catch (Exception ex)
            {
                // 可以在这里添加日志记录
                Console.WriteLine($"计算CRC32校验和失败: {ex.Message}");
                return 0;
            }
        }

        /// <summary>
        /// 计算字节数组的MD5哈希值
        /// </summary>
        /// <param name="data">源数据</param>
        /// <returns>MD5哈希值字符串，如果输入为null或空则返回空字符串</returns>
        public static string CalculateMd5(byte[] data)
        {
            if (data == null || data.Length == 0)
                return string.Empty;

            try
            {
                using (var md5 = System.Security.Cryptography.MD5.Create())
                {
                    byte[] hashBytes = md5.ComputeHash(data);
                    return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
                }
            }
            catch (Exception ex)
            {
                // 可以在这里添加日志记录
                Console.WriteLine($"计算MD5哈希值失败: {ex.Message}");
                return string.Empty;
            }
        }

        #endregion
    }
}