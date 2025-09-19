using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace RUINORERP.PacketSpec.Core.DataProcessing
{
    /// <summary>
    /// 高性能字节操作工具类
    /// 整合了 ByteHelper、ByteDataBuilder 和 EndianUtils 的功能
    /// 提供字节数组的读写、转换、校验、构建和字节序处理功能
    /// </summary>
    public static class ByteOperations
    {
        /// <summary>
        /// 默认编码格式 (UTF-8)
        /// </summary>
        public static readonly Encoding DefaultEncoding = Encoding.UTF8;

        #region 字节数组读取操作

        /// <summary>
        /// 从字节数组获取32位整数（小端序）
        /// </summary>
        /// <param name="data">字节数组</param>
        /// <param name="index">读取位置索引（引用传递，读取后自动递增）</param>
        /// <returns>读取的整数值</returns>
        /// <exception cref="ArgumentException">数据长度不足时抛出</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetInt32(byte[] data, ref int index)
        {
            if (data == null || index + 4 > data.Length)
                throw new ArgumentException("数据长度不足");

            int value = BinaryPrimitives.ReadInt32LittleEndian(data.AsSpan(index));
            index += 4;
            return value;
        }

        /// <summary>
        /// 从字节数组获取16位短整数（小端序）
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short GetInt16(byte[] data, ref int index)
        {
            if (data == null || index + 2 > data.Length)
                throw new ArgumentException("数据长度不足");

            short value = BinaryPrimitives.ReadInt16LittleEndian(data.AsSpan(index));
            index += 2;
            return value;
        }

        /// <summary>
        /// 从字节数组获取字节
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte GetByte(byte[] data, ref int index)
        {
            if (data == null || index >= data.Length)
                throw new ArgumentException("数据长度不足");

            return data[index++];
        }

        /// <summary>
        /// 从字节数组获取64位长整数（小端序）
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long GetInt64(byte[] data, ref int index)
        {
            if (data == null || index + 8 > data.Length)
                throw new ArgumentException("数据长度不足");

            long value = BinaryPrimitives.ReadInt64LittleEndian(data.AsSpan(index));
            index += 8;
            return value;
        }

        /// <summary>
        /// 从字节数组获取指定长度的字符串
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
        /// 从字节数组获取以null结尾的字符串
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

        #endregion

        #region 字节数组写入操作

        /// <summary>
        /// 将32位整数写入字节数组（小端序）
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteInt32(byte[] buffer, ref int index, int value)
        {
            if (buffer == null || index + 4 > buffer.Length)
                throw new ArgumentException("缓冲区空间不足");

            BinaryPrimitives.WriteInt32LittleEndian(buffer.AsSpan(index), value);
            index += 4;
        }

        /// <summary>
        /// 将16位短整数写入字节数组（小端序）
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteInt16(byte[] buffer, ref int index, short value)
        {
            if (buffer == null || index + 2 > buffer.Length)
                throw new ArgumentException("缓冲区空间不足");

            BinaryPrimitives.WriteInt16LittleEndian(buffer.AsSpan(index), value);
            index += 2;
        }

        /// <summary>
        /// 将字节写入字节数组
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteByte(byte[] buffer, ref int index, byte value)
        {
            if (buffer == null || index >= buffer.Length)
                throw new ArgumentException("缓冲区空间不足");

            buffer[index++] = value;
        }

        /// <summary>
        /// 将64位长整数写入字节数组（小端序）
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteInt64(byte[] buffer, ref int index, long value)
        {
            if (buffer == null || index + 8 > buffer.Length)
                throw new ArgumentException("缓冲区空间不足");

            BinaryPrimitives.WriteInt64LittleEndian(buffer.AsSpan(index), value);
            index += 8;
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

        #endregion

        #region 字节序处理

        /// <summary>
        /// 将整数转换为小端序字节数组
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte[] ToLittleEndianBytes(int value)
        {
            byte[] bytes = new byte[4];
            BinaryPrimitives.WriteInt32LittleEndian(bytes, value);
            return bytes;
        }

        /// <summary>
        /// 将小端序字节数组转换为整数
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int FromLittleEndianBytes(byte[] bytes, int startIndex = 0)
        {
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));

            if (startIndex + 4 > bytes.Length)
                throw new ArgumentException("字节数组长度不足");

            return BinaryPrimitives.ReadInt32LittleEndian(bytes.AsSpan(startIndex));
        }

        /// <summary>
        /// 写入小端序整数到缓冲区
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteLittleEndian(int value, byte[] buffer, int offset)
        {
            BinaryPrimitives.WriteInt32LittleEndian(buffer.AsSpan(offset), value);
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

        #endregion

        #region 校验和计算

        /// <summary>
        /// 计算字节数组的校验和（异或校验）
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ValidateChecksum(byte[] data, byte expectedChecksum)
        {
            return CalculateChecksum(data) == expectedChecksum;
        }

        #endregion

        #region 十六进制转换

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

        #endregion

        #region 字节数组比较

        /// <summary>
        /// 比较两个字节数组是否相等
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        #endregion

        #region 字节数据构建器功能

        /// <summary>
        /// 创建字节数据构建器
        /// </summary>
        public static ByteDataBuilder CreateBuilder(int capacity = 1024)
        {
            return new ByteDataBuilder(capacity);
        }

        /// <summary>
        /// 高性能字节数据构建器
        /// </summary>
        public sealed class ByteDataBuilder
        {
            private readonly List<byte> _buffer;

            /// <summary>
            /// 当前数据长度
            /// </summary>
            public int Length => _buffer.Count;

            /// <summary>
            /// 初始化字节数据构建器
            /// </summary>
            public ByteDataBuilder(int capacity = 1024)
            {
                _buffer = new List<byte>(capacity);
            }

            /// <summary>
            /// 添加字符串（包含长度前缀）
            /// </summary>
            public void AddString(string value)
            {
                if (string.IsNullOrEmpty(value))
                {
                    AddInt32(0);
                    return;
                }

                byte[] stringBytes = DefaultEncoding.GetBytes(value);
                AddInt32(stringBytes.Length);
                _buffer.AddRange(stringBytes);
            }

            /// <summary>
            /// 添加32位整数（小端序）
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void AddInt32(int value)
            {
                byte[] bytes = ToLittleEndianBytes(value);
                _buffer.AddRange(bytes);
            }

            /// <summary>
            /// 添加64位整数（小端序）
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void AddInt64(long value)
            {
                byte[] bytes = new byte[8];
                BinaryPrimitives.WriteInt64LittleEndian(bytes, value);
                _buffer.AddRange(bytes);
            }

            /// <summary>
            /// 添加字节
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void AddByte(byte value)
            {
                _buffer.Add(value);
            }

            /// <summary>
            /// 添加字节数组
            /// </summary>
            public void AddBytes(byte[] value)
            {
                if (value != null && value.Length > 0)
                {
                    _buffer.AddRange(value);
                }
            }

            /// <summary>
            /// 转换为字节数组
            /// </summary>
            public byte[] ToArray()
            {
                return _buffer.ToArray();
            }

            /// <summary>
            /// 清空缓冲区
            /// </summary>
            public void Clear()
            {
                _buffer.Clear();
            }
        }

        #endregion
    }
}
