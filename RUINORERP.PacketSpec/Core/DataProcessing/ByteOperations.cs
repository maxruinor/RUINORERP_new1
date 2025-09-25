using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Newtonsoft.Json;
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

        /// <summary>
        /// 从字节数组获取字符串（自动从数据中读取长度前缀）
        /// 兼容原始ByteDataAnalysis类的业务逻辑
        /// </summary>
        /// <param name="data">字节数组</param>
        /// <param name="index">读取位置索引（引用传递，读取后自动递增）</param>
        /// <returns>读取的字符串</returns>
        /// <exception cref="InvalidOperationException">数据长度不足时抛出</exception>
        public static string GetString(byte[] data, ref int index)
        {
            if (data == null)
            {
                return string.Empty;
            }
            if (data.Length < index + 4)
                throw new InvalidOperationException("Buffer underflow.");

            int len = GetInt32(data, ref index);

            if (data.Length < index + len)
                throw new InvalidOperationException("Buffer underflow.");

            byte[] msg = new byte[len];
            Buffer.BlockCopy(data, index, msg, 0, len);
            index += len;
            index += 1; // 跳过结束符
            return DefaultEncoding.GetString(msg);
        }

        /// <summary>
        /// 从字节数组获取字符串（自动从数据中读取长度前缀）并返回未解析的数据
        /// 兼容原始ByteDataAnalysis类的业务逻辑
        /// </summary>
        /// <param name="data">字节数组</param>
        /// <param name="unparsedData">未解析的数据</param>
        /// <param name="index">读取位置索引（引用传递，读取后自动递增）</param>
        /// <returns>读取的字符串</returns>
        /// <exception cref="InvalidOperationException">数据长度不足时抛出</exception>
        public static string GetString(byte[] data, out byte[] unparsedData, ref int index)
        {
            if (data.Length < index + 4)
                throw new InvalidOperationException("Buffer underflow.");

            int len = GetInt32(data, ref index);

            if (data.Length < index + len)
                throw new InvalidOperationException("Buffer underflow.");

            byte[] msg = new byte[len];
            Buffer.BlockCopy(data, index, msg, 0, len);
            index += len;
            index += 1; // 跳过结束符
            
            unparsedData = new byte[data.Length - index];
            if (unparsedData.Length > 0)
            {
                Buffer.BlockCopy(data, index, unparsedData, 0, unparsedData.Length);
            }

            return DefaultEncoding.GetString(msg);
        }

        /// <summary>
        /// 尝试从字节数组获取32位整数，不抛出异常
        /// 兼容原始ByteDataAnalysis类的业务逻辑
        /// </summary>
        /// <param name="data">字节数组</param>
        /// <param name="success">是否成功读取</param>
        /// <param name="index">读取位置索引（引用传递，读取后自动递增）</param>
        /// <returns>读取的整数值，如果读取失败则返回0</returns>
        public static int TryGetInt(byte[] data, out bool success, ref int index)
        {
            success = true;
            if (data == null || data.Length < index + 4)
            {
                success = false;
                return 0;
            }

            int value = GetInt32(data, ref index);
            return value;
        }

        /// <summary>
        /// 尝试从字节数组获取字符串，不抛出异常
        /// 兼容原始ByteDataAnalysis类的业务逻辑
        /// </summary>
        /// <param name="data">字节数组</param>
        /// <param name="success">是否成功读取</param>
        /// <param name="index">读取位置索引（引用传递，读取后自动递增）</param>
        /// <returns>读取的字符串，如果读取失败则返回null</returns>
        public static string TryGetString(byte[] data, out bool success, ref int index)
        {
            success = false;
            if (data == null || data.Length < index + 4)
                return null;

            try
            {
                int len = GetInt32(data, ref index);

                if (data.Length < index + len)
                    return null;

                byte[] msg = new byte[len];
                Buffer.BlockCopy(data, index, msg, 0, len);
                index += len;
                
                success = true;
                return DefaultEncoding.GetString(msg);
            }
            catch
            {
                success = false;
                return null;
            }
        }

        /// <summary>
        /// 从字节数组获取32位整数并返回未解析的数据
        /// 兼容原始ByteDataAnalysis类的业务逻辑
        /// </summary>
        /// <param name="data">字节数组</param>
        /// <param name="unparsedData">未解析的数据</param>
        /// <param name="index">读取位置索引（引用传递，读取后自动递增）</param>
        /// <returns>读取的整数值</returns>
        public static int GetInt(byte[] data, out byte[] unparsedData, ref int index)
        {
            // 检查是否有足够的数据来读取一个int
            if (data == null || data.Length < index + 4)
            {
                unparsedData = Array.Empty<byte>(); // 没有足够的数据，返回空数组
                return 0;
            }

            // 读取int值
            int value = GetInt32(data, ref index);

            // 获取剩余的未解析数据
            unparsedData = new byte[data.Length - index];
            if (unparsedData.Length > 0)
            {
                Buffer.BlockCopy(data, index, unparsedData, 0, unparsedData.Length);
            }

            return value;
        }

        /// <summary>
        /// 获取指定长度的字节数组
        /// 兼容原始ByteDataAnalysis类的业务逻辑
        /// </summary>
        /// <param name="data">字节数组</param>
        /// <param name="dataLen">要获取的字节长度</param>
        /// <param name="index">读取位置索引（引用传递，读取后自动递增）</param>
        /// <returns>读取的字节数组</returns>
        /// <exception cref="InvalidOperationException">数据长度不足时抛出</exception>
        public static byte[] GetBytes(byte[] data, int dataLen, ref int index)
        {
            if (data == null || data.Length < index + dataLen)
                throw new InvalidOperationException("Buffer underflow.");

            byte[] bytes = new byte[dataLen];
            Buffer.BlockCopy(data, index, bytes, 0, dataLen);
            index += dataLen;
            return bytes;
        }

        /// <summary>
        /// 从字节数组获取布尔值（1表示true，0表示false）
        /// 兼容原始ByteDataAnalysis类的业务逻辑
        /// </summary>
        /// <param name="data">字节数组</param>
        /// <param name="index">读取位置索引（引用传递，读取后自动递增）</param>
        /// <returns>读取的布尔值</returns>
        /// <exception cref="InvalidOperationException">数据长度不足时抛出</exception>
        public static bool GetBool(byte[] data, ref int index)
        {
            if (data == null || index >= data.Length)
                throw new InvalidOperationException("Buffer underflow.");

            bool b = data[index++] == 1;
            return b;
        }

        /// <summary>
        /// 从字节数组获取单精度浮点数
        /// 兼容原始ByteDataAnalysis类的业务逻辑
        /// </summary>
        /// <param name="data">字节数组</param>
        /// <param name="index">读取位置索引（引用传递，读取后自动递增）</param>
        /// <returns>读取的浮点数值</returns>
        /// <exception cref="InvalidOperationException">数据长度不足时抛出</exception>
        /// <summary>
        /// 从指定索引位置的字节数组中读取一个float值（单精度浮点数）
        /// </summary>
        /// <param name="data">包含数据的字节数组</param>
        /// <param name="index">当前读取索引，会在方法执行后更新</param>
        /// <returns>读取到的float值</returns>
        /// <exception cref="InvalidOperationException">当缓冲区不足或为空时抛出</exception>
        public static float GetFloat(byte[] data, ref int index)
        {
            if (data == null || index + 4 > data.Length)
                throw new InvalidOperationException("Buffer underflow.");

            // 在.NET Standard 2.0中手动实现小端序读取float
            uint intValue = (uint)(data[index] | (data[index + 1] << 8) | (data[index + 2] << 16) | (data[index + 3] << 24));
            float value = BitConverter.ToSingle(BitConverter.GetBytes(intValue), 0);
            index += 4;
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
            /// 添加字符串（包含长度前缀和结束符）
            /// 兼容原始ByteDataAnalysis类的业务逻辑
            /// </summary>
            /// <param name="value">要添加的字符串</param>
            public void AddStringWithTerminator(string value)
            {
                if (string.IsNullOrEmpty(value))
                {
                    AddInt32(0);
                    AddByte(0); // 添加结束符
                    return;
                }

                byte[] stringBytes = DefaultEncoding.GetBytes(value);
                AddInt32(stringBytes.Length);
                _buffer.AddRange(stringBytes);
                AddByte(0); // 添加结束符
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

        #region 字符串与字节数组转换

        /// <summary>
        /// 字符串转换为字节数组（UTF8编码）
        /// </summary>
        /// <param name="value">要转换的字符串</param>
        /// <returns>转换后的字节数组，如果输入为null则返回空数组</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte[] StringToBytes(string value)
        {
            return value != null ? DefaultEncoding.GetBytes(value) : Array.Empty<byte>();
        }

        /// <summary>
        /// 字节数组转换为字符串（UTF8编码）
        /// </summary>
        /// <param name="bytes">要转换的字节数组</param>
        /// <returns>转换后的字符串，如果输入为null或空则返回空字符串</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string BytesToString(byte[] bytes)
        {
            return bytes != null && bytes.Length > 0 ? DefaultEncoding.GetString(bytes) : string.Empty;
        }

        /// <summary>
        /// 字节数组转换为字符串（指定编码）
        /// </summary>
        /// <param name="bytes">要转换的字节数组</param>
        /// <param name="encoding">字符编码</param>
        /// <returns>转换后的字符串，如果输入为null、空或编码为null则返回空字符串</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetStringFromBytes(byte[] bytes, int startIndex, int length)
        {
            if (bytes == null || startIndex < 0 || length <= 0 || startIndex + length > bytes.Length)
                return string.Empty;
            return DefaultEncoding.GetString(bytes, startIndex, length);
        }

        #endregion

        #region 数值类型与字节数组转换

        /// <summary>
        /// 32位整数转换为字节数组（小端序）
        /// </summary>
        /// <param name="value">要转换的整数</param>
        /// <returns>转换后的字节数组</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte[] Int32ToBytes(int value)
        {
            byte[] bytes = new byte[4];
            BinaryPrimitives.WriteInt32LittleEndian(bytes, value);
            return bytes;
        }

        /// <summary>
        /// 64位整数转换为字节数组（小端序）
        /// </summary>
        /// <param name="value">要转换的长整数</param>
        /// <returns>转换后的字节数组</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte[] Int64ToBytes(long value)
        {
            byte[] bytes = new byte[8];
            BinaryPrimitives.WriteInt64LittleEndian(bytes, value);
            return bytes;
        }

        /// <summary>
        /// 16位整数转换为字节数组（小端序）
        /// </summary>
        /// <param name="value">要转换的短整数</param>
        /// <returns>转换后的字节数组</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte[] Int16ToBytes(short value)
        {
            byte[] bytes = new byte[2];
            BinaryPrimitives.WriteInt16LittleEndian(bytes, value);
            return bytes;
        }

        /// <summary>
        /// 单精度浮点数转换为字节数组
        /// </summary>
        /// <param name="value">要转换的浮点数</param>
        /// <returns>转换后的字节数组</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte[] FloatToBytes(float value)
        {
            byte[] bytes = new byte[4];
            // 在.NET Standard 2.0中手动实现小端序写入float
            byte[] floatBytes = BitConverter.GetBytes(value);
            if (!BitConverter.IsLittleEndian)
                Array.Reverse(floatBytes);
            Buffer.BlockCopy(floatBytes, 0, bytes, 0, 4);
            return bytes;
        }

        /// <summary>
        /// 双精度浮点数转换为字节数组
        /// </summary>
        /// <param name="value">要转换的双精度浮点数</param>
        /// <returns>转换后的字节数组</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte[] DoubleToBytes(double value)
        {
            byte[] bytes = new byte[8];
            // 在.NET Standard 2.0中手动实现小端序写入double
            byte[] doubleBytes = BitConverter.GetBytes(value);
            if (!BitConverter.IsLittleEndian)
                Array.Reverse(doubleBytes);
            Buffer.BlockCopy(doubleBytes, 0, bytes, 0, 8);
            return bytes;
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

            int totalLength = 0;
            foreach (byte[] array in arrays)
            {
                if (array != null)
                    totalLength += array.Length;
            }

            if (totalLength == 0)
                return Array.Empty<byte>();

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
        /// <param name="source">源字节数组</param>
        /// <param name="startIndex">起始索引</param>
        /// <param name="length">要提取的长度</param>
        /// <returns>提取的子数组，如果参数无效则返回空数组</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        /// <param name="bytes">要反转的字节数组</param>
        /// <returns>反转后的字节数组，如果输入为null则返回空数组</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte[] ReverseBytes(byte[] bytes)
        {
            if (bytes == null)
                return Array.Empty<byte>();

            byte[] reversed = new byte[bytes.Length];
            Buffer.BlockCopy(bytes, 0, reversed, 0, bytes.Length);
            Array.Reverse(reversed);
            return reversed;
        }

        /// <summary>
        /// 比较两个字节数组是否相等
        /// </summary>
        /// <param name="a">第一个字节数组</param>
        /// <param name="b">第二个字节数组</param>
        /// <returns>如果两个数组相等则返回true，否则返回false</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool BytesEqual(byte[] a, byte[] b)
        {
            if (ReferenceEquals(a, b))
                return true;
            if (a == null || b == null)
                return false;
            if (a.Length != b.Length)
                return false;
            
            return a.AsSpan().SequenceEqual(b.AsSpan());
        }

        #endregion

        #region 高级数据处理功能

         
 

        /// <summary>
        /// 计算字节数组的CRC32校验和
        /// </summary>
        /// <param name="data">源数据</param>
        /// <returns>CRC32校验和</returns>
        public static uint CalculateCrc32(byte[] data)
        {
            if (data == null || data.Length == 0)
                return 0;

            const uint polynomial = 0xEDB88320;
            uint crc = 0xFFFFFFFF;

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
            catch
            {
                return string.Empty;
            }
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetString(byte[] bytes, int startIndex, int length)
        {
            if (bytes == null || startIndex < 0 || length <= 0 || startIndex + length > bytes.Length)
                return string.Empty;
            return DefaultEncoding.GetString(bytes, startIndex, length);
        }

        /// <summary>
        /// 从字节数组中获取32位整数
        /// </summary>
        /// <param name="bytes">源字节数组</param>
        /// <param name="startIndex">起始索引</param>
        /// <returns>读取的整数，如果参数无效则返回0</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetInt32(byte[] bytes, int startIndex = 0)
        {
            if (bytes == null || startIndex < 0 || startIndex + 4 > bytes.Length)
                return 0;
            return BinaryPrimitives.ReadInt32LittleEndian(bytes.AsSpan(startIndex));
        }

        /// <summary>
        /// 从字节数组中获取64位整数
        /// </summary>
        /// <param name="bytes">源字节数组</param>
        /// <param name="startIndex">起始索引</param>
        /// <returns>读取的长整数，如果参数无效则返回0</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long GetInt64(byte[] bytes, int startIndex = 0)
        {
            if (bytes == null || startIndex < 0 || startIndex + 8 > bytes.Length)
                return 0;
            return BinaryPrimitives.ReadInt64LittleEndian(bytes.AsSpan(startIndex));
        }

        /// <summary>
        /// 从字节数组中获取16位整数
        /// </summary>
        /// <param name="bytes">源字节数组</param>
        /// <param name="startIndex">起始索引</param>
        /// <returns>读取的短整数，如果参数无效则返回0</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short GetInt16(byte[] bytes, int startIndex = 0)
        {
            if (bytes == null || startIndex < 0 || startIndex + 2 > bytes.Length)
                return 0;
            return BinaryPrimitives.ReadInt16LittleEndian(bytes.AsSpan(startIndex));
        }

        /// <summary>
        /// 从字节数组中获取单精度浮点数
        /// </summary>
        /// <param name="bytes">源字节数组</param>
        /// <param name="startIndex">起始索引</param>
        /// <returns>读取的浮点数，如果参数无效则返回0</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetFloat(byte[] bytes, int startIndex = 0)
        {
            if (bytes == null || startIndex < 0 || startIndex + 4 > bytes.Length)
                return 0;
            // 在.NET Standard 2.0中手动实现小端序读取float
            uint intValue = (uint)(bytes[startIndex] | (bytes[startIndex + 1] << 8) | (bytes[startIndex + 2] << 16) | (bytes[startIndex + 3] << 24));
            float value = BitConverter.ToSingle(BitConverter.GetBytes(intValue), 0);
            return value;
        }

        /// <summary>
        /// 从字节数组中获取双精度浮点数
        /// </summary>
        /// <param name="bytes">源字节数组</param>
        /// <param name="startIndex">起始索引</param>
        /// <returns>读取的双精度浮点数，如果参数无效则返回0</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double GetDouble(byte[] bytes, int startIndex = 0)
        {
            if (bytes == null || startIndex < 0 || startIndex + 8 > bytes.Length)
                return 0;
            // 在.NET Standard 2.0中手动实现小端序读取double
            byte[] doubleBytes = new byte[8];
            Buffer.BlockCopy(bytes, startIndex, doubleBytes, 0, 8);
            if (!BitConverter.IsLittleEndian)
                Array.Reverse(doubleBytes);
            double value = BitConverter.ToDouble(doubleBytes, 0);
            return value;
        }

        /// <summary>
        /// 从字节数组中获取布尔值
        /// </summary>
        /// <param name="bytes">源字节数组</param>
        /// <param name="startIndex">起始索引</param>
        /// <returns>读取的布尔值，如果参数无效则返回false</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte GetByte(byte[] bytes, int startIndex = 0)
        {
            if (bytes == null || startIndex < 0 || startIndex >= bytes.Length)
                return 0;
            return bytes[startIndex];
        }

        #endregion
    }
}
