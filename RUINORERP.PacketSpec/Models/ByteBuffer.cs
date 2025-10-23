using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RUINORERP.PacketSpec.Models
{
    /// <summary>
    /// 高性能字节缓冲区处理类 - 用于读取和写入字节数据
    /// 支持动态扩容、多种数据类型序列化和反序列化
    /// </summary>
    public class ByteBuffer
    {
        private byte[] _buffer;
        private int _popPosition; // 读取位置指针
        private int _pushPosition; // 写入位置指针
        private const int DefaultInitialSize = 100;
        private const int DefaultGrowSize = 1024;

        #region 构造函数

        /// <summary>
        /// 从现有字节数组创建缓冲区
        /// </summary>
        /// <param name="array">初始字节数组</param>
        public ByteBuffer(byte[] array)
        {
            _buffer = array ?? throw new ArgumentNullException(nameof(array));
            _popPosition = 0;
            _pushPosition = array.Length;
        }

        /// <summary>
        /// 创建指定大小的缓冲区
        /// </summary>
        /// <param name="size">初始缓冲区大小</param>
        public ByteBuffer(int size = DefaultInitialSize)
        {
            if (size <= 0) size = DefaultInitialSize;
            _buffer = new byte[size];
            _popPosition = 0;
            _pushPosition = 0;
        }

        #endregion

        #region 属性

        /// <summary>
        /// 缓冲区总容量
        /// </summary>
        public int Capacity => _buffer.Length;

        /// <summary>
        /// 当前写入的数据长度
        /// </summary>
        public int Length => _pushPosition;

        /// <summary>
        /// 剩余可读取的字节数
        /// </summary>
        public int BytesAvailable => _pushPosition - _popPosition;

        /// <summary>
        /// 索引器访问缓冲区内容
        /// </summary>
        public byte this[int index]
        {
            get
            {
                if (index >= _pushPosition)
                    throw new IndexOutOfRangeException("索引超出缓冲区范围");
                return _buffer[index];
            }
            set
            {
                if (index >= _pushPosition)
                    throw new IndexOutOfRangeException("索引超出缓冲区范围");
                _buffer[index] = value;
            }
        }

        #endregion

        #region 写入方法

        /// <summary>
        /// 确保缓冲区有足够的空间
        /// </summary>
        private void EnsureCapacity(int requiredBytes)
        {
            if (_pushPosition + requiredBytes <= _buffer.Length) return;

            int newSize = Math.Max(_buffer.Length + DefaultGrowSize, _pushPosition + requiredBytes);
            Array.Resize(ref _buffer, newSize);
        }

        /// <summary>
        /// 写入单个字节
        /// </summary>
        public void PushByte(byte value)
        {
            EnsureCapacity(1);
            _buffer[_pushPosition++] = value;
        }

        /// <summary>
        /// 写入布尔值
        /// </summary>
        public void PushBool(bool value)
        {
            PushByte(value ? (byte)1 : (byte)0);
        }

        /// <summary>
        /// 写入字节数组
        /// </summary>
        public void PushBytes(byte[] values)
        {
            if (values == null || values.Length == 0) return;

            EnsureCapacity(values.Length);
            Array.Copy(values, 0, _buffer, _pushPosition, values.Length);
            _pushPosition += values.Length;
        }

        /// <summary>
        /// 写入16位整数
        /// </summary>
        public void PushInt16(short value)
        {
            EnsureCapacity(2);
            _buffer[_pushPosition++] = (byte)value;
            _buffer[_pushPosition++] = (byte)(value >> 8);
        }

        /// <summary>
        /// 写入16位无符号整数
        /// </summary>
        public void PushUInt16(ushort value)
        {
            PushInt16((short)value);
        }

        /// <summary>
        /// 写入32位整数
        /// </summary>
        public void PushInt(int value)
        {
            EnsureCapacity(4);
            _buffer[_pushPosition++] = (byte)value;
            _buffer[_pushPosition++] = (byte)(value >> 8);
            _buffer[_pushPosition++] = (byte)(value >> 16);
            _buffer[_pushPosition++] = (byte)(value >> 24);
        }

        /// <summary>
        /// 写入32位无符号整数
        /// </summary>
        public void PushUInt(uint value)
        {
            PushInt((int)value);
        }

        /// <summary>
        /// 写入64位整数
        /// </summary>
        public void PushInt64(long value)
        {
            EnsureCapacity(8);
            for (int i = 0; i < 8; i++)
            {
                _buffer[_pushPosition++] = (byte)value;
                value >>= 8;
            }
        }

        /// <summary>
        /// 写入单精度浮点数
        /// </summary>
        public void PushFloat(float value)
        {
            PushBytes(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// 写入双精度浮点数
        /// </summary>
        public void PushDouble(double value)
        {
            PushBytes(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// 写入字符串（UTF8编码，带长度前缀）
        /// </summary>
        public void PushString(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                PushInt(0);
                PushByte(0); // 结束符
                return;
            }

            byte[] byteArray = Encoding.UTF8.GetBytes(value);
            PushInt(byteArray.Length);
            PushBytes(byteArray);
            PushByte(0); // 结束符
        }

        /// <summary>
        /// 写入十六进制字符串
        /// </summary>
        public void PushHexString(string hexString)
        {
            if (string.IsNullOrEmpty(hexString)) return;

            byte[] bytes = new byte[hexString.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }
            PushBytes(bytes);
        }

        #endregion

        #region 读取方法

        /// <summary>
        /// 检查是否有足够的数据可读
        /// </summary>
        private void CheckReadable(int bytesRequired)
        {
            if (_popPosition + bytesRequired > _pushPosition)
                throw new EndOfStreamException("缓冲区数据不足");
        }

        /// <summary>
        /// 读取单个字节
        /// </summary>
        public byte PopByte()
        {
            CheckReadable(1);
            return _buffer[_popPosition++];
        }

        /// <summary>
        /// 读取布尔值
        /// </summary>
        public bool PopBool()
        {
            return PopByte() != 0;
        }

        /// <summary>
        /// 读取指定长度的字节数组
        /// </summary>
        public byte[] PopBytes(int length)
        {
            CheckReadable(length);
            byte[] result = new byte[length];
            Array.Copy(_buffer, _popPosition, result, 0, length);
            _popPosition += length;
            return result;
        }

        /// <summary>
        /// 读取16位整数
        /// </summary>
        public short PopInt16()
        {
            CheckReadable(2);
            short result = (short)(_buffer[_popPosition] | (_buffer[_popPosition + 1] << 8));
            _popPosition += 2;
            return result;
        }

        /// <summary>
        /// 读取16位无符号整数
        /// </summary>
        public ushort PopUInt16()
        {
            return (ushort)PopInt16();
        }

        /// <summary>
        /// 读取32位整数
        /// </summary>
        public int PopInt()
        {
            CheckReadable(4);
            int result = _buffer[_popPosition] |
                        (_buffer[_popPosition + 1] << 8) |
                        (_buffer[_popPosition + 2] << 16) |
                        (_buffer[_popPosition + 3] << 24);
            _popPosition += 4;
            return result;
        }

        /// <summary>
        /// 读取32位无符号整数
        /// </summary>
        public uint PopUInt()
        {
            return (uint)PopInt();
        }

        /// <summary>
        /// 读取64位整数
        /// </summary>
        public long PopInt64()
        {
            CheckReadable(8);
            long result = 0;
            for (int i = 0; i < 8; i++)
            {
                result |= (long)_buffer[_popPosition + i] << (i * 8);
            }
            _popPosition += 8;
            return result;
        }

        /// <summary>
        /// 读取单精度浮点数
        /// </summary>
        public float PopFloat()
        {
            return BitConverter.ToSingle(PopBytes(4), 0);
        }

        /// <summary>
        /// 读取双精度浮点数
        /// </summary>
        public double PopDouble()
        {
            return BitConverter.ToDouble(PopBytes(8), 0);
        }

        /// <summary>
        /// 读取字符串
        /// </summary>
        public string PopString()
        {
            int length = PopInt();
            if (length == 0) return string.Empty;

            CheckReadable(length + 1); // 包括结束符
            string result = Encoding.UTF8.GetString(_buffer, _popPosition, length);
            _popPosition += length + 1; // 跳过结束符
            return result;
        }

        #endregion

        #region 工具方法

        /// <summary>
        /// 将缓冲区内容转换为字节数组
        /// </summary>
        public byte[] ToByteArray()
        {
            byte[] result = new byte[_pushPosition];
            Array.Copy(_buffer, result, _pushPosition);
            return result;
        }

        /// <summary>
        /// 清空缓冲区
        /// </summary>
        public void Clear()
        {
            _popPosition = 0;
            _pushPosition = 0;
        }

        /// <summary>
        /// 重置读取位置
        /// </summary>
        public void ResetReadPosition()
        {
            _popPosition = 0;
        }

        /// <summary>
        /// 跳过指定字节数
        /// </summary>
        public void Skip(int bytes)
        {
            if (_popPosition + bytes > _pushPosition)
                throw new ArgumentOutOfRangeException(nameof(bytes), "跳过字节数超出缓冲区范围");
            _popPosition += bytes;
        }

        /// <summary>
        /// 获取缓冲区的十六进制表示
        /// </summary>
        public string ToHexString()
        {
            return BitConverter.ToString(_buffer, 0, _pushPosition).Replace("-", "");
        }

        #endregion

        #region 静态工具方法

        /// <summary>
        /// 从十六进制字符串创建字节数组
        /// </summary>
        public static byte[] HexStringToBytes(string hexString)
        {
            if (string.IsNullOrEmpty(hexString)) return Array.Empty<byte>();

            int length = hexString.Length;
            byte[] bytes = new byte[length / 2];
            for (int i = 0; i < length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
            }
            return bytes;
        }

        /// <summary>
        /// 将字节数组转换为十六进制字符串
        /// </summary>
        public static string BytesToHexString(byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", "");
        }

        #endregion
    }
}



