using RUINORERP.PacketSpec.Protocol;
using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace RUINORERP.PacketSpec.Security
{
    /// <summary>
    /// 加密解密协议工具类，用于socket通讯系统中的数据处理
    /// 注意：cmd,one,two。
    /// cmd：指令码   长度：一个字节（byte）的取值范围是 0 到 255 ，共 256 个不同的值。
    /// one：子指令码 长度：int 通常占用 4 个字节（32 位）。
    /// two：数据包   长度：int 通常占用 4 个字节（32 位）。
    /// </summary>
    /// <summary>
    /// 高性能Socket通信加密解密工具类
    /// 保持原始加密解密逻辑，但进行了性能优化和内存管理改进
    /// </summary>
    public static class EncryptedProtocolV2
    {
        #region 常量定义

        private const int HEADER_LENGTH = 18;
        private const int KEY_SIZE = 256;
        private const int MAX_DECRYPT_ATTEMPTS = 4;

        #endregion

        #region 静态字段和初始化

        private static int m_dateKey = 0;
        private static readonly object _dateLock = new object();
        private static int _currentDay = 0;

        private static byte[] _yesterdayKey;
        private static byte[] _todayKey;
        private static byte[] _tomorrowKey;
        private static byte[] _constantKey;

        // 线程本地存储，避免每个线程重复创建临时数组
        private static readonly System.Threading.ThreadLocal<byte[]> _threadLocalTempHeader =
            new System.Threading.ThreadLocal<byte[]>(() => ArrayPool<byte>.Shared.Rent(HEADER_LENGTH));

        private static readonly System.Threading.ThreadLocal<byte[]> _threadLocalTempKey =
            new System.Threading.ThreadLocal<byte[]>(() => ArrayPool<byte>.Shared.Rent(KEY_SIZE));

        #endregion

        #region 服务端方法

        /// <summary>
        /// 服务器端使用的加密方法
        /// </summary>
        public static EncryptedData EncryptionServerPackToClient(byte cmd, byte[] one, byte[] two)
        {
            OriginalData gd = new OriginalData
            {
                Cmd = cmd,
                One = one ?? Array.Empty<byte>(),
                Two = two ?? Array.Empty<byte>()
            };
            return EncryptionServerPackToClient(gd);
        }

        /// <summary>
        /// 服务器端使用的加密方法
        /// </summary>
        public static EncryptedData EncryptionServerPackToClient(OriginalData gd)
        {
            EncryptedData encryptedData = new EncryptedData();

            // 使用内存池获取头部数组
            byte[] head = ArrayPool<byte>.Shared.Rent(HEADER_LENGTH);
            try
            {
                // 初始化头部
                head.AsSpan().Clear();
                head[0] = gd.Cmd;

                // 使用小端字节序存储长度
                Buffer.BlockCopy(BitConverter.GetBytes(gd.One.Length), 0, head, 2, 4);
                Buffer.BlockCopy(BitConverter.GetBytes(gd.Two.Length), 0, head, 6, 4);

                // 计算校验和
                byte hi = 0, low = 0;
                for (int i = 0; i < 8; i++)
                {
                    hi ^= head[i * 2];
                    low ^= head[i * 2 + 1];
                }
                head[16] = hi;
                head[17] = low;

                // 获取密钥并加密
                byte[] key = GetKey(m_dateKey);
                try
                {
                    Encrypt(key, head, 0, 0);
                    Encrypt(key, gd.One, 0, 0);
                    Encrypt(key, gd.Two, 0, 0);
                }
                finally
                {
                    ArrayPool<byte>.Shared.Return(key);
                }

                encryptedData.Head = head;
                encryptedData.One = gd.One;
                encryptedData.Two = gd.Two;

                return encryptedData;
            }
            catch
            {
                ArrayPool<byte>.Shared.Return(head);
                throw;
            }
        }

        /// <summary>
        /// 服务器端使用：解决来自客户端的加密数据包的包头得到包体长度
        /// </summary>
        public static int AnalyzeClientPackHeader(byte[] head)
        {
            if (head == null || head.Length < HEADER_LENGTH)
                return 256 - HEADER_LENGTH;

            // 使用线程本地存储的临时数组
            byte[] tempHead = _threadLocalTempHeader.Value;
            byte[] tempKey = _threadLocalTempKey.Value;

            // 尝试使用不同的密钥解密头部
            for (int i = 0; i < MAX_DECRYPT_ATTEMPTS; i++)
            {
                // 复制头部数据
                head.AsSpan(0, HEADER_LENGTH).CopyTo(tempHead);

                // 获取密钥
                byte[] key = GetKey((i + m_dateKey) % 4);
                key.AsSpan().CopyTo(tempKey);

                // 尝试解密
                Encrypt(tempKey, tempHead, 0, 0);

                // 验证校验和
                byte hi = 0, low = 0;
                for (int ii = 0; ii < 9; ii++)
                {
                    hi ^= tempHead[ii * 2];
                    low ^= tempHead[ii * 2 + 1];
                }

                if (hi == 0 && low == 0)
                {
                    m_dateKey = i;

                    // 解析长度信息
                    int lenOne = BitConverter.ToInt32(tempHead, 2);
                    int lenTwo = BitConverter.ToInt32(tempHead, 6);

                    return lenOne + lenTwo;
                }
            }

            return 256 - HEADER_LENGTH;
        }

        /// <summary>
        /// 服务器解来自客户端的包
        /// </summary>
        public static OriginalData DecryptionClientPack(byte[] head, byte[] allData)
        {
            if (head == null || head.Length < HEADER_LENGTH || allData == null)
                throw new ArgumentException("Invalid input data");

            OriginalData result = new OriginalData();

            // 使用线程本地存储的临时数组
            byte[] tempHead = _threadLocalTempHeader.Value;
            byte[] tempKey = _threadLocalTempKey.Value;

            // 尝试解密头部
            bool decoded = false;
            byte[] key = null;

            for (int i = 0; i < MAX_DECRYPT_ATTEMPTS; i++)
            {
                // 复制头部数据
                head.AsSpan(0, HEADER_LENGTH).CopyTo(tempHead);

                // 获取密钥
                key = GetKey((i + m_dateKey) % 4);
                key.AsSpan().CopyTo(tempKey);

                // 尝试解密
                Encrypt(tempKey, tempHead, 0, 0);

                // 验证校验和
                byte hi = 0, low = 0;
                for (int ii = 0; ii < 9; ii++)
                {
                    hi ^= tempHead[ii * 2];
                    low ^= tempHead[ii * 2 + 1];
                }

                if (hi == 0 && low == 0)
                {
                    m_dateKey = i;
                    decoded = true;
                    result.Cmd = tempHead[0];
                    break;
                }
            }

            if (!decoded)
                throw new Exception("非法字符串");

            // 解析长度信息
            int oneLength = BitConverter.ToInt32(head, 2);
            int twoLength = BitConverter.ToInt32(head, 6);

            // 检查数据长度是否足够
            if (allData.Length < HEADER_LENGTH + oneLength + twoLength)
                throw new Exception("数据长度不足");

            // 解密第一部分数据
            if (oneLength > 0)
            {
                result.One = ArrayPool<byte>.Shared.Rent(oneLength);
                try
                {
                    allData.AsSpan(HEADER_LENGTH, oneLength).CopyTo(result.One);
                    Encrypt(key, result.One, 0, 0);
                }
                catch
                {
                    ArrayPool<byte>.Shared.Return(result.One);
                    throw;
                }
            }

            // 解密第二部分数据
            if (twoLength > 0)
            {
                result.Two = ArrayPool<byte>.Shared.Rent(twoLength);
                try
                {
                    allData.AsSpan(HEADER_LENGTH + oneLength, twoLength).CopyTo(result.Two);
                    Encrypt(key, result.Two, 0, 0);
                }
                catch
                {
                    ArrayPool<byte>.Shared.Return(result.Two);
                    throw;
                }
            }

            return result;
        }

        #endregion

        #region 客户端方法

        /// <summary>
        /// 解析服务器包的头部得到包体长度
        /// </summary>
        public static int AnalyzeSeverPackHeader(byte[] head)
        {
            if (head == null || head.Length < HEADER_LENGTH)
                return 256 - HEADER_LENGTH;

            // 使用线程本地存储的临时数组
            byte[] tempHead = _threadLocalTempHeader.Value;
            byte[] tempKey = _threadLocalTempKey.Value;

            // 尝试使用不同的密钥解密头部
            for (int i = 0; i < MAX_DECRYPT_ATTEMPTS; i++)
            {
                // 复制头部数据
                head.AsSpan(0, HEADER_LENGTH).CopyTo(tempHead);

                // 获取密钥
                byte[] key = GetKey((i + m_dateKey) % 4);
                key.AsSpan().CopyTo(tempKey);

                // 尝试解密
                Encrypt(tempKey, tempHead, 0, 0);

                // 验证校验和
                byte hi = 0, low = 0;
                for (int ii = 0; ii < 8; ii++)
                {
                    hi ^= tempHead[ii * 2];
                    low ^= tempHead[ii * 2 + 1];
                }

                if (tempHead[16] == hi && tempHead[17] == low)
                {
                    m_dateKey = i;

                    // 解析长度信息
                    int lenOne = BitConverter.ToInt32(tempHead, 2);
                    int lenTwo = BitConverter.ToInt32(tempHead, 6);

                    return lenOne + lenTwo;
                }
            }

            return 256 - HEADER_LENGTH;
        }

        /// <summary>
        /// 解密来自服务器的包
        /// </summary>
        public static OriginalData DecryptServerPack(byte[] packData)
        {
            if (packData == null || packData.Length < HEADER_LENGTH)
                throw new ArgumentException("Invalid packet data");

            OriginalData result = new OriginalData();

            // 使用线程本地存储的临时数组
            byte[] tempHead = _threadLocalTempHeader.Value;
            byte[] tempKey = _threadLocalTempKey.Value;

            // 尝试解密头部
            bool decoded = false;
            byte[] key = null;

            for (int i = 0; i < MAX_DECRYPT_ATTEMPTS; i++)
            {
                // 复制头部数据
                packData.AsSpan(0, HEADER_LENGTH).CopyTo(tempHead);

                // 获取密钥
                key = GetKey((i + m_dateKey) % 4);
                key.AsSpan().CopyTo(tempKey);

                // 尝试解密
                Encrypt(tempKey, tempHead, 0, 0);

                // 验证校验和
                byte hi = 0, low = 0;
                for (int ii = 0; ii < 8; ii++)
                {
                    hi ^= tempHead[ii * 2];
                    low ^= tempHead[ii * 2 + 1];
                }

                if (tempHead[16] == hi && tempHead[17] == low)
                {
                    m_dateKey = i;
                    decoded = true;
                    result.Cmd = tempHead[0];
                    break;
                }
            }

            if (!decoded)
                throw new Exception("解码失败");

            // 解析长度信息
            int oneLength = BitConverter.ToInt32(tempHead, 2);
            int twoLength = BitConverter.ToInt32(tempHead, 6);

            // 检查数据长度是否足够
            if (packData.Length < HEADER_LENGTH + oneLength + twoLength)
                throw new Exception("数据长度不足");

            // 解密第一部分数据
            if (oneLength > 0)
            {
                result.One = ArrayPool<byte>.Shared.Rent(oneLength);
                try
                {
                    packData.AsSpan(HEADER_LENGTH, oneLength).CopyTo(result.One);
                    Encrypt(key, result.One, 0, 0);
                }
                catch
                {
                    ArrayPool<byte>.Shared.Return(result.One);
                    throw;
                }
            }

            // 解密第二部分数据
            if (twoLength > 0)
            {
                result.Two = ArrayPool<byte>.Shared.Rent(twoLength);
                try
                {
                    packData.AsSpan(HEADER_LENGTH + oneLength, twoLength).CopyTo(result.Two);
                    Encrypt(key, result.Two, 0, 0);
                }
                catch
                {
                    ArrayPool<byte>.Shared.Return(result.Two);
                    throw;
                }
            }

            return result;
        }

        /// <summary>
        /// 客户端将生成的数据打包加密码准备给服务器
        /// </summary>
        public static byte[] EncryptClientPackToServer(OriginalData gd)
        {
            byte cmd = gd.Cmd;
            byte[] one = gd.One ?? Array.Empty<byte>();
            byte[] two = gd.Two ?? Array.Empty<byte>();

            int totalLength = HEADER_LENGTH + one.Length + two.Length;

            // 使用内存池获取结果数组
            byte[] result = ArrayPool<byte>.Shared.Rent(totalLength);
            try
            {
                // 设置头部
                result.AsSpan().Clear();
                result[0] = cmd;

                // 写入长度信息
                Buffer.BlockCopy(BitConverter.GetBytes(one.Length), 0, result, 2, 4);
                Buffer.BlockCopy(BitConverter.GetBytes(two.Length), 0, result, 6, 4);

                // 计算校验和
                byte hi = 0, low = 0;
                for (int i = 0; i < 8; i++)
                {
                    hi ^= result[i * 2];
                    low ^= result[i * 2 + 1];
                }
                result[16] = hi;
                result[17] = low;

                // 复制数据部分
                if (one.Length > 0)
                    one.AsSpan().CopyTo(result.AsSpan(HEADER_LENGTH));
                if (two.Length > 0)
                    two.AsSpan().CopyTo(result.AsSpan(HEADER_LENGTH + one.Length));

                // 获取密钥并加密
                byte[] key = GetKey(m_dateKey);
                try
                {
                    uint mask = Encrypt(key, result.AsSpan(0, HEADER_LENGTH).ToArray(), 0, 0);

                    if (one.Length > 0)
                        mask = Encrypt(key, result.AsSpan(HEADER_LENGTH, one.Length).ToArray(), HEADER_LENGTH, mask);

                    if (two.Length > 0)
                        Encrypt(key, result.AsSpan(HEADER_LENGTH + one.Length, two.Length).ToArray(), HEADER_LENGTH + one.Length, mask);
                }
                finally
                {
                    ArrayPool<byte>.Shared.Return(key);
                }

                // 返回正确大小的数组
                byte[] finalResult = new byte[totalLength];
                result.AsSpan(0, totalLength).CopyTo(finalResult);
                return finalResult;
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(result);
            }
        }

        #endregion

        #region 加密解密核心方法

        /// <summary>
        /// 加密/解密方法
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Encrypt(byte[] KEY, byte[] buff, int start, uint mask)
        {
            if (KEY == null) throw new ArgumentNullException(nameof(KEY));
            if (buff == null) throw new ArgumentNullException(nameof(buff));

            byte temp1, temp2, temp3, temp4;
            temp4 = (byte)mask;
            mask >>= 8;
            temp3 = (byte)(mask);
            mask >>= 8;
            temp2 = (byte)(mask);
            mask >>= 8;
            temp1 = (byte)(mask);

            for (int i = 0; i < buff.Length; i++)
            {
                byte pi = (byte)(i + 1 + start);
                temp1 = KEY[pi];
                temp3 = (byte)(temp1 + temp3);
                temp2 = KEY[temp3];
                KEY[pi] = temp2;
                KEY[temp3] = temp1;

                temp2 = (byte)(temp1 + temp2);
                temp1 = KEY[temp2];
                temp4 = buff[i];

                temp4 = (byte)(temp4 ^ temp1);
                buff[i] = temp4;
            }

            mask = (uint)(temp1);
            mask <<= 8;
            mask |= (uint)(temp2);
            mask <<= 8;
            mask |= (uint)(temp3);
            mask <<= 8;
            mask |= (uint)(temp4);
            return mask;
        }

        /// <summary>
        /// 获取指定步骤的密钥
        /// </summary>
        public static byte[] GetKey(int step)
        {
            // 检查日期是否变化，如有变化则重新生成密钥
            if (_currentDay != DateTime.Now.Day)
            {
                lock (_dateLock)
                {
                    if (_currentDay != DateTime.Now.Day)
                    {
                        var tm = DateTime.Now.AddDays(-1);
                        var s = tm.Year + "年" + tm.Month + "月" + tm.Day + "日";
                        _yesterdayKey = GetPassword(s);

                        tm = DateTime.Now;
                        s = tm.Year + "年" + tm.Month + "月" + tm.Day + "日";
                        _todayKey = GetPassword(s);

                        tm = DateTime.Now.AddDays(1);
                        s = tm.Year + "年" + tm.Month + "月" + tm.Day + "日";
                        _tomorrowKey = GetPassword(s);

                        s = "woaikaixuan";
                        _constantKey = GetPassword(s);

                        _currentDay = DateTime.Now.Day;
                    }
                }
            }

            // 使用内存池获取密钥副本
            byte[] result = ArrayPool<byte>.Shared.Rent(KEY_SIZE);
            try
            {
                switch (step % 4)
                {
                    case 0:
                        _constantKey.CopyTo(result, 0);
                        break;
                    case 1:
                        _todayKey.CopyTo(result, 0);
                        break;
                    case 2:
                        _yesterdayKey.CopyTo(result, 0);
                        break;
                    case 3:
                        _tomorrowKey.CopyTo(result, 0);
                        break;
                }

                return result;
            }
            catch
            {
                ArrayPool<byte>.Shared.Return(result);
                throw;
            }
        }

        /// <summary>
        /// 从密码生成密钥
        /// </summary>
        public static byte[] GetPassword(string s)
        {
            var ret = new byte[KEY_SIZE];
            byte[] bys = StrToBytes(s);
            int tmlen = bys.Length;
            byte temp1 = 0, temp2 = 0, temp3 = 0, temp4 = 0;

            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = (byte)i;
            }

            for (int i = 0; i < ret.Length; i++)
            {
                temp1 = bys[i % tmlen];
                temp2 = ret[i];
                temp3 = (byte)(temp1 + temp3 + temp2);
                temp4 = ret[temp3];

                ret[i] = temp4;
                ret[temp3] = temp2;
            }

            return ret;
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 将字符串转换为字节数组
        /// </summary>
        public static byte[] StrToBytes(string value)
        {
            if (string.IsNullOrEmpty(value))
                return Array.Empty<byte>();

            return Encoding.UTF8.GetBytes(value);
        }

        /// <summary>
        /// 将字节数组转换为十六进制字符串
        /// </summary>
        public static string Hex2Str(byte[] data, int offset, int length, bool addSpace)
        {
            if (data == null || offset < 0 || length <= 0 || offset + length > data.Length)
                return string.Empty;

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                sb.AppendFormat("{0:X2}", data[offset + i]);
                if (addSpace && i < length - 1)
                    sb.Append(" ");
            }
            return sb.ToString();
        }

        #endregion
    }

}

