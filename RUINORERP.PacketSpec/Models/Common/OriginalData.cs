using System;
using Newtonsoft.Json;
using RUINORERP.PacketSpec.Security;

namespace RUINORERP.PacketSpec.Models.Common
{
    /// <summary>
    /// 原始数据包结构 - 表示未加密的数据
    /// </summary>
    public struct OriginalData
    {
        /// <summary>
        /// 命令字节
        /// CommandId的CommandCategory
        /// </summary>
        [JsonProperty("cmd")]
        public byte Cmd;

        /// <summary>
        /// 第一部分数据
        /// CommandId的OperationCode子指令
        /// </summary>
        [JsonProperty("one")]
        public byte[] One;

        /// <summary>
        /// 第二部分数据
        /// </summary>
        [JsonProperty("two")]
        public byte[] Two;

        /// <summary>
        /// 数据包总长度
        /// </summary>
        public int Length
        {
            get
            {
                int len = 1; // Cmd 字节
                if (One != null)
                {
                    len += One.Length;
                }
                if (Two != null)
                {
                    len += Two.Length;
                }
                return len;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public OriginalData(byte cmd, byte[] one, byte[] two)
        {
            Cmd = cmd;
            One = one ?? Array.Empty<byte>();
            Two = two ?? Array.Empty<byte>();
        }

        /// <summary>
        /// 构造函数 - 只包含命令和第一部分数据
        /// </summary>
        public OriginalData(byte cmd, byte[] one)
            : this(cmd, one, Array.Empty<byte>())
        { }

        /// <summary>
        /// 构造函数 - 只包含命令
        /// </summary>
        public OriginalData(byte cmd)
            : this(cmd, Array.Empty<byte>(), Array.Empty<byte>())
        { }

        /// <summary>
        /// 判断数据包是否有效
        /// </summary>
        public bool IsValid
        {
            get
            {
                return Cmd > 0;
            }
        }

        /// <summary>
        /// 创建空的数据包
        /// </summary>
        public static OriginalData Empty
        {
            get
            {
                return new OriginalData(0, Array.Empty<byte>(), Array.Empty<byte>());
            }
        }

        /// <summary>
        /// 从命令和字符串数据创建数据包
        /// </summary>
        public static OriginalData FromString(byte cmd, string data)
        {
            return new OriginalData(cmd, data != null ? System.Text.Encoding.UTF8.GetBytes(data) : Array.Empty<byte>());
        }

        /// <summary>
        /// 获取第一部分数据的字符串表示
        /// </summary>
        public string GetOneAsString()
        {
            return One != null && One.Length > 0 ? System.Text.Encoding.UTF8.GetString(One) : string.Empty;
        }

        /// <summary>
        /// 获取第二部分数据的字符串表示
        /// </summary>
        public string GetTwoAsString()
        {
            return Two != null && Two.Length > 0 ? System.Text.Encoding.UTF8.GetString(Two) : string.Empty;
        }

        /// <summary>
        /// 转换为字节数组
        /// </summary>
        public byte[] ToByteArray()
        {
            byte[] result = new byte[Length];
            result[0] = Cmd;
            int offset = 1;

            if (One != null && One.Length > 0)
            {
                Buffer.BlockCopy(One, 0, result, offset, One.Length);
                offset += One.Length;
            }

            if (Two != null && Two.Length > 0)
            {
                Buffer.BlockCopy(Two, 0, result, offset, Two.Length);
            }

            return result;
        }

        /// <summary>
        /// 复制当前数据包
        /// </summary>
        public OriginalData Clone()
        {
            byte[] oneCopy = One != null ? (byte[])One.Clone() : Array.Empty<byte>();
            byte[] twoCopy = Two != null ? (byte[])Two.Clone() : Array.Empty<byte>();
            return new OriginalData(Cmd, oneCopy, twoCopy);
        }
    }

    /// <summary>
    /// 加密数据包结构 - 表示已加密的数据
    /// </summary>
    public struct EncryptedData
    {
        /// <summary>
        /// 包头部数据
        /// </summary>
        public byte[] Head;

        /// <summary>
        /// 第一部分加密数据
        /// </summary>
        public byte[] One;

        /// <summary>
        /// 第二部分加密数据
        /// </summary>
        public byte[] Two;

        /// <summary>
        /// 数据包总长度
        /// </summary>
        public int Length
        {
            get
            {
                int len = 0;
                if (Head != null) len += Head.Length;
                if (One != null) len += One.Length;
                if (Two != null) len += Two.Length;
                return len;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public EncryptedData(byte[] head, byte[] one, byte[] two)
        {
            Head = head ?? Array.Empty<byte>();
            One = one ?? Array.Empty<byte>();
            Two = two ?? Array.Empty<byte>();
        }

        /// <summary>
        /// 判断数据包是否有效
        /// </summary>
        public bool IsValid
        {
            get
            {
                return Head != null && Head.Length > 0;
            }
        }

        /// <summary>
        /// 创建空的加密数据包
        /// </summary>
        public static EncryptedData Empty
        {
            get
            {
                return new EncryptedData(Array.Empty<byte>(), Array.Empty<byte>(), Array.Empty<byte>());
            }
        }

        /// <summary>
        /// 将加密后的数据包转换为字节数组
        /// </summary>
        /// <returns>按顺序包含Head、One和Two数据的ReadOnlyMemory</returns>
        public ReadOnlyMemory<byte> ToByteArray()
        {
            return new ReadOnlyMemory<byte>(BuildByteArray());
        }
        
        /// <summary>
        /// 将加密后的数据包转换为字节数组
        /// </summary>
        /// <returns>按顺序包含Head、One和Two数据的byte[]</returns>
        public byte[] ToArray()
        {
            return BuildByteArray();
        }
        
        /// <summary>
        /// 构建字节数组的私有辅助方法
        /// </summary>
        /// <returns>按顺序包含Head、One和Two数据的byte[]</returns>
        private byte[] BuildByteArray()
        {
            // 创建足够大的字节数组来存储所有数据
            byte[] result = new byte[Length];
            int offset = 0;

            // 按顺序复制数据
            if (Head != null && Head.Length > 0)
            {
                Buffer.BlockCopy(Head, 0, result, offset, Head.Length);
                offset += Head.Length;
            }

            if (One != null && One.Length > 0)
            {
                Buffer.BlockCopy(One, 0, result, offset, One.Length);
                offset += One.Length;
            }

            if (Two != null && Two.Length > 0)
            {
                Buffer.BlockCopy(Two, 0, result, offset, Two.Length);
            }

            return result;
        }
    }
}
