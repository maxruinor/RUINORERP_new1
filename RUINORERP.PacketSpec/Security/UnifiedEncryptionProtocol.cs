using System;
using System.Collections.Generic;
using System.Text;
using RUINORERP.PacketSpec.Models.Common;
using System;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;

namespace RUINORERP.PacketSpec.Security
{
    /// <summary>
        /// 统一加密协议类 - 修复版本
        /// 为Socket通讯提供高效的数据加解密和数据包处理功能
        /// </summary>
        public static class UnifiedEncryptionProtocol
        {
            #region 常量定义

            /// <summary>
            /// 数据包头部长度
            /// </summary>
            public const int HEADER_LENGTH = 18;

            /// <summary>
            /// 加密密钥长度
            /// </summary>
            private const int KEY_LENGTH = 256;

            /// <summary>
            /// 最大数据包大小（字节）
            /// 默认设置为10MB以支持更大的数据传输
            /// </summary>
            public const int MAX_PACKET_SIZE = 100 * 1024 * 1024; // 100MB

            #endregion

            #region 字段

            /// <summary>
            /// 日期密钥索引
            /// </summary>
            private static int _dateKeyIndex = 0;

            /// <summary>
            /// 昨天的密钥
            /// </summary>
            private static byte[] _yesterdayKey;

            /// <summary>
            /// 今天的密钥
            /// </summary>
            private static byte[] _todayKey;

            /// <summary>
            /// 明天的密钥
            /// </summary>
            private static byte[] _tomorrowKey;

            /// <summary>
            /// 固定密钥
            /// </summary>
            private static byte[] _fixedKey;

            /// <summary>
            /// 上次更新日期
            /// </summary>
            private static int _lastUpdateDay = 0;

            /// <summary>
            /// 日期锁对象
            /// </summary>
            private static readonly object _dateLock = new object();

            #endregion

            #region 构造函数

            /// <summary>
            /// 静态构造函数 - 初始化密钥
            /// </summary>
            static UnifiedEncryptionProtocol()
            {
                UpdateKeysIfNeeded();
            }

            #endregion

            #region 公共方法 - 服务器端

            /// <summary>
            /// 服务器端使用的加密方法 - 将原始数据加密为发送给客户端的数据
            /// </summary>
            /// <param name="cmd">命令字节</param>
            /// <param name="one">第一部分数据</param>
            /// <param name="two">第二部分数据</param>
            /// <returns>加密后的数据</returns>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static EncryptedData EncryptServerDataToClient(byte cmd, byte[] one, byte[] two)
            {
                var originalData = new OriginalData(cmd, one ?? Array.Empty<byte>(), two ?? Array.Empty<byte>());
                return EncryptServerDataToClient(originalData);
            }

            /// <summary>
            /// 服务器端使用的加密方法 - 将原始数据加密为发送给客户端的数据
            /// </summary>
            /// <param name="originalData">原始数据</param>
            /// <returns>加密后的数据</returns>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static EncryptedData EncryptServerDataToClient(OriginalData originalData)
            {
                // 更新密钥（如果需要）
                UpdateKeysIfNeeded();

                // 创建头部数据
                var head = new byte[HEADER_LENGTH];

                // 初始化头部
                InitializeHeader(head, originalData.Cmd, originalData.One?.Length ?? 0, originalData.Two?.Length ?? 0);

                // 计算校验和
                CalculateChecksum(head);

                // 获取当前密钥
                var key = GetCurrentKey(_dateKeyIndex);

                // 加密头部 - 使用新的掩码处理方法
                EncryptDataWithMask(key, head, 0, 0);

                // 加密数据部分
                byte[] encryptedOne = null;
                byte[] encryptedTwo = null;
                uint currentMask = 0;

                if (originalData.One != null && originalData.One.Length > 0)
                {
                    encryptedOne = new byte[originalData.One.Length];
                    Array.Copy(originalData.One, encryptedOne, originalData.One.Length);
                    currentMask = EncryptDataWithMask(key, encryptedOne, 0, currentMask);
                }

                if (originalData.Two != null && originalData.Two.Length > 0)
                {
                    encryptedTwo = new byte[originalData.Two.Length];
                    Array.Copy(originalData.Two, encryptedTwo, originalData.Two.Length);
                    currentMask = EncryptDataWithMask(key, encryptedTwo, 0, currentMask);
                }

                // 创建并返回加密数据
                return new EncryptedData(
                    head,
                    encryptedOne ?? Array.Empty<byte>(),
                    encryptedTwo ?? Array.Empty<byte>()
                );
            }

            /// <summary>
            /// 服务器端使用：分析来自客户端的加密数据包头部，获取包体长度
            /// </summary>
            /// <param name="head">数据包头部</param>
            /// <returns>包体长度</returns>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static int AnalyzeClientPacketHeader(ReadOnlySpan<byte> head)
            {
                if (head.Length < HEADER_LENGTH)
                    return 256 - HEADER_LENGTH;

                // 更新密钥（如果需要）
                UpdateKeysIfNeeded();

                // 尝试解密头部
                for (int i = 0; i < 4; i++)
                {
                    var key = GetCurrentKey(i); // 直接使用i，而不是(i + _dateKeyIndex) % 4
                    var headCopy = new byte[HEADER_LENGTH];
                    head.Slice(0, HEADER_LENGTH).CopyTo(headCopy);

                    // 解密头部
                    DecryptDataWithMask(key, headCopy, 0, 0);

                    // 验证校验和
                    if (VerifyChecksum(headCopy))
                    {
                        _dateKeyIndex = i;

                        // 获取数据长度
                        int oneLength = BinaryPrimitives.ReadInt32LittleEndian(headCopy.AsSpan(2, 4));
                        int twoLength = BinaryPrimitives.ReadInt32LittleEndian(headCopy.AsSpan(6, 4));

                        // 验证长度是否有效
                        if (oneLength >= 0 && twoLength >= 0 && oneLength + twoLength <= MAX_PACKET_SIZE)
                        {
                            return oneLength + twoLength;
                        }
                    }
                }

                return 256 - HEADER_LENGTH;
            }

            /// <summary>
            /// 服务器端使用：解密来自客户端的数据包
            /// </summary>
            /// <param name="head">数据包头部</param>
            /// <param name="packetData">完整的数据包数据</param>
            /// <returns>解密后的原始数据</returns>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static OriginalData DecryptClientPacket(ReadOnlySpan<byte> head, ReadOnlySpan<byte> packetData)
            {
                if (head.Length < HEADER_LENGTH)
                    throw new ArgumentException("头部长度不足");

                // 更新密钥（如果需要）
                UpdateKeysIfNeeded();

                // 尝试解密头部
                byte[] key = null;
                bool decoded = false;
                byte[] headCopy = new byte[HEADER_LENGTH];

                for (int i = 0; i < 4; i++)
                {
                    key = GetCurrentKey(i);
                    head.Slice(0, HEADER_LENGTH).CopyTo(headCopy);
                    DecryptDataWithMask(key, headCopy, 0, 0);

                    if (VerifyChecksum(headCopy))
                    {
                        _dateKeyIndex = i;
                        decoded = true;
                        break;
                    }
                }

                if (!decoded)
                {
                    throw new InvalidOperationException("无法解密数据包头部");
                }

                var cmd = headCopy[0];

                // 获取数据长度
                int oneLength = BinaryPrimitives.ReadInt32LittleEndian(headCopy.AsSpan(2, 4));
                int twoLength = BinaryPrimitives.ReadInt32LittleEndian(headCopy.AsSpan(6, 4));

                // 验证数据长度是否有效
                if (oneLength < 0 || twoLength < 0 || oneLength + twoLength > MAX_PACKET_SIZE)
                {
                    throw new InvalidOperationException("无效的数据长度");
                }

                // 检查数据包总长度
                int expectedTotalLength = HEADER_LENGTH + oneLength + twoLength;
                if (packetData.Length < expectedTotalLength)
                {
                    throw new ArgumentOutOfRangeException(nameof(packetData),
                        $"数据包长度不足。期望: {expectedTotalLength}, 实际: {packetData.Length}");
                }

                // 创建返回数据
                var result = new OriginalData
                {
                    Cmd = cmd
                };

                // 解密数据部分
                uint currentMask = 0;

                // 解密第一部分数据
                if (oneLength > 0)
                {
                    result.One = new byte[oneLength];
                    packetData.Slice(HEADER_LENGTH, oneLength).CopyTo(result.One);
                    currentMask = DecryptDataWithMask(key, result.One, 0, currentMask);
                }

                // 解密第二部分数据
                if (twoLength > 0)
                {
                    int twoStartIndex = HEADER_LENGTH + oneLength;
                    result.Two = new byte[twoLength];
                    packetData.Slice(twoStartIndex, twoLength).CopyTo(result.Two);
                    currentMask = DecryptDataWithMask(key, result.Two, 0, currentMask);
                }

                return result;
            }

            #endregion

            #region 公共方法 - 客户端

            /// <summary>
            /// 客户端使用：分析来自服务器的数据包头部，获取包体长度
            /// </summary>
            /// <param name="head">数据包头部</param>
            /// <returns>包体长度</returns>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static int AnalyzeServerPacketHeader(ReadOnlySpan<byte> head)
            {
                if (head.Length < HEADER_LENGTH)
                    return 256 - HEADER_LENGTH;

                // 更新密钥（如果需要）
                UpdateKeysIfNeeded();

                // 尝试解密头部
                for (int i = 0; i < 4; i++)
                {
                    var key = GetCurrentKey(i);
                    var headCopy = new byte[HEADER_LENGTH];
                    head.Slice(0, HEADER_LENGTH).CopyTo(headCopy);
                    DecryptDataWithMask(key, headCopy, 0, 0);

                    if (VerifyChecksum(headCopy))
                    {
                        _dateKeyIndex = i;

                        // 获取数据长度
                        int oneLength = BinaryPrimitives.ReadInt32LittleEndian(headCopy.AsSpan(2, 4));
                        int twoLength = BinaryPrimitives.ReadInt32LittleEndian(headCopy.AsSpan(6, 4));

                        // 验证长度是否有效
                        if (oneLength >= 0 && twoLength >= 0 && oneLength + twoLength <= MAX_PACKET_SIZE)
                        {
                            return oneLength + twoLength;
                        }
                    }
                }

                return 256 - HEADER_LENGTH;
            }

            /// <summary>
            /// 客户端使用：解密来自服务器的数据包
            /// </summary>
            /// <param name="packetData">完整的数据包数据</param>
            /// <returns>解密后的原始数据</returns>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static OriginalData DecryptServerPacket(ReadOnlySpan<byte> packetData)
            {
                if (packetData.Length < HEADER_LENGTH)
                    throw new ArgumentException("数据包长度不足");

                // 更新密钥（如果需要）
                UpdateKeysIfNeeded();

                // 提取头部
                var head = packetData.Slice(0, HEADER_LENGTH).ToArray();

                // 尝试解密头部
                byte[] key = null;
                bool decoded = false;

                for (int i = 0; i < 4; i++)
                {
                    key = GetCurrentKey(i);
                    var headCopy = new byte[HEADER_LENGTH];
                    head.CopyTo(headCopy, 0);
                    DecryptDataWithMask(key, headCopy, 0, 0);

                    if (VerifyChecksum(headCopy))
                    {
                        _dateKeyIndex = i;
                        decoded = true;
                        head = headCopy; // 使用解密后的头部
                        break;
                    }
                }

                if (!decoded)
                {
                    throw new InvalidOperationException("无法解密数据包头部");
                }

                var cmd = head[0];

                // 获取数据长度
                int oneLength = BinaryPrimitives.ReadInt32LittleEndian(head.AsSpan(2, 4));
                int twoLength = BinaryPrimitives.ReadInt32LittleEndian(head.AsSpan(6, 4));

                // 验证数据长度是否有效
                if (oneLength < 0 || twoLength < 0 || oneLength + twoLength > MAX_PACKET_SIZE)
                {
                    throw new InvalidOperationException("无效的数据长度");
                }

                // 检查数据包总长度
                int expectedTotalLength = HEADER_LENGTH + oneLength + twoLength;
                if (packetData.Length < expectedTotalLength)
                {
                    throw new ArgumentOutOfRangeException(nameof(packetData),
                        $"数据包长度不足。期望: {expectedTotalLength}, 实际: {packetData.Length}");
                }

                // 创建返回数据
                var result = new OriginalData
                {
                    Cmd = cmd
                };

                // 解密数据部分
                uint currentMask = 0;

                // 解密第一部分数据
                if (oneLength > 0)
                {
                    result.One = new byte[oneLength];
                    packetData.Slice(HEADER_LENGTH, oneLength).CopyTo(result.One);
                    currentMask = DecryptDataWithMask(key, result.One, 0, currentMask);
                }

                // 解密第二部分数据
                if (twoLength > 0)
                {
                    int twoStartIndex = HEADER_LENGTH + oneLength;
                    result.Two = new byte[twoLength];
                    packetData.Slice(twoStartIndex, twoLength).CopyTo(result.Two);
                    currentMask = DecryptDataWithMask(key, result.Two, 0, currentMask);
                }

                return result;
            }

            /// <summary>
            /// 客户端使用：加密数据包准备发送给服务器
            /// </summary>
            /// <param name="originalData">原始数据</param>
            /// <returns>加密后的字节数组</returns>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static byte[] EncryptClientDataToServer(OriginalData originalData)
            {
                // 更新密钥（如果需要）
                UpdateKeysIfNeeded();

                var cmd = originalData.Cmd;
                var one = originalData.One ?? Array.Empty<byte>();
                var two = originalData.Two ?? Array.Empty<byte>();

                // 创建头部
                var head = new byte[HEADER_LENGTH];

                // 初始化头部
                InitializeHeader(head, cmd, one.Length, two.Length);

                // 计算校验和
                CalculateChecksum(head);

                // 获取当前密钥
                var key = GetCurrentKey(_dateKeyIndex);

                // 加密头部
                EncryptDataWithMask(key, head, 0, 0);

                // 加密数据部分
                byte[] encryptedOne = new byte[one.Length];
                byte[] encryptedTwo = new byte[two.Length];
                uint currentMask = 0;

                if (one.Length > 0)
                {
                    Array.Copy(one, encryptedOne, one.Length);
                    currentMask = EncryptDataWithMask(key, encryptedOne, 0, currentMask);
                }

                if (two.Length > 0)
                {
                    Array.Copy(two, encryptedTwo, two.Length);
                    currentMask = EncryptDataWithMask(key, encryptedTwo, 0, currentMask);
                }

                // 组合所有数据
                var result = new byte[HEADER_LENGTH + one.Length + two.Length];
                Buffer.BlockCopy(head, 0, result, 0, HEADER_LENGTH);
                if (one.Length > 0)
                {
                    Buffer.BlockCopy(encryptedOne, 0, result, HEADER_LENGTH, one.Length);
                }
                if (two.Length > 0)
                {
                    int offset = HEADER_LENGTH + one.Length;
                    Buffer.BlockCopy(encryptedTwo, 0, result, offset, two.Length);
                }

                return result;
            }

            #endregion

            #region 私有方法

            /// <summary>
            /// 初始化数据包头部
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static void InitializeHeader(byte[] head, byte cmd, int oneLength, int twoLength)
            {
                if (head == null || head.Length < HEADER_LENGTH)
                    throw new ArgumentException($"头部数组长度必须至少为{HEADER_LENGTH}", nameof(head));

                // 清零头部
                Array.Clear(head, 0, HEADER_LENGTH);

                // 设置命令字节
                head[0] = cmd;

                // 设置版本标识
                head[1] = 0x14;

                // 使用小端字节序存储长度
                int safeOneLength = Math.Max(0, Math.Min(int.MaxValue, oneLength));
                int safeTwoLength = Math.Max(0, Math.Min(int.MaxValue, twoLength));

                BinaryPrimitives.WriteInt32LittleEndian(head.AsSpan(2, 4), safeOneLength);
                BinaryPrimitives.WriteInt32LittleEndian(head.AsSpan(6, 4), safeTwoLength);

                // 预留字段置零
                for (int i = 10; i < 16; i++)
                {
                    head[i] = 0;
                }
            }

            /// <summary>
            /// 计算校验和
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static void CalculateChecksum(byte[] head)
            {
                byte hi = 0, low = 0;
                for (int i = 0; i < 8; i++)
                {
                    hi ^= head[i * 2];
                    low ^= head[i * 2 + 1];
                }
                head[16] = hi;
                head[17] = low;
            }

            /// <summary>
            /// 验证校验和
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static bool VerifyChecksum(byte[] head)
            {
                byte hi = 0, low = 0;
                for (int i = 0; i < 8; i++)
                {
                    hi ^= head[i * 2];
                    low ^= head[i * 2 + 1];
                }
                return hi == head[16] && low == head[17];
            }

            /// <summary>
            /// 更新密钥（如果需要）
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static void UpdateKeysIfNeeded()
            {
                var now = DateTime.Now;
                var currentDateKey = now.Year * 10000 + now.Month * 100 + now.Day;

                if (_lastUpdateDay != currentDateKey)
                {
                    lock (_dateLock)
                    {
                        if (_lastUpdateDay != currentDateKey)
                        {
                            var yesterday = now.AddDays(-1);
                            var tomorrow = now.AddDays(1);

                            // 使用统一格式生成密钥字符串
                            var yesterdayStr = $"{yesterday.Year:0000}-{yesterday.Month:00}-{yesterday.Day:00}";
                            var todayStr = $"{now.Year:0000}-{now.Month:00}-{now.Day:00}";
                            var tomorrowStr = $"{tomorrow.Year:0000}-{tomorrow.Month:00}-{tomorrow.Day:00}";
                            var fixedStr = "RUINOR-ERP-2024-SECURE-KEY";

                            // 生成新的密钥
                            _yesterdayKey = GenerateKey(yesterdayStr);
                            _todayKey = GenerateKey(todayStr);
                            _tomorrowKey = GenerateKey(tomorrowStr);
                            _fixedKey = GenerateKey(fixedStr);

                            // 更新日期键
                            _lastUpdateDay = currentDateKey;
                        }
                    }
                }
            }

            /// <summary>
            /// 获取当前密钥
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static byte[] GetCurrentKey(int index)
            {
                // 确保所有密钥都已初始化
                if (_fixedKey == null)
                    UpdateKeysIfNeeded();

                // 使用索引获取对应的密钥
                byte[] sourceKey = null;
                switch (index % 4)
                {
                    case 0:
                        sourceKey = _fixedKey;
                        break;
                    case 1:
                        sourceKey = _todayKey;
                        break;
                    case 2:
                        sourceKey = _yesterdayKey;
                        break;
                    case 3:
                        sourceKey = _tomorrowKey;
                        break;
                }

                // 创建密钥副本并返回
                var key = new byte[KEY_LENGTH];
                if (sourceKey != null && sourceKey.Length == KEY_LENGTH)
                {
                    Array.Copy(sourceKey, key, KEY_LENGTH);
                }
                return key;
            }

            /// <summary>
            /// 生成密钥
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static byte[] GenerateKey(string password)
            {
                if (string.IsNullOrEmpty(password))
                    return new byte[KEY_LENGTH];

                var passwordBytes = Encoding.UTF8.GetBytes(password);
                var key = new byte[KEY_LENGTH];
                var passwordLength = passwordBytes.Length;

                // 初始化密钥数组
                for (int i = 0; i < KEY_LENGTH; i++)
                {
                    key[i] = (byte)i;
                }

                // 改进的密钥扰乱算法
                int j;
                byte temp;
                uint seed = 0;

                // 首先使用密码计算初始种子
                for (int i = 0; i < passwordLength; i++)
                {
                    seed = seed * 131 + passwordBytes[i];
                }

                // 使用种子和密码对密钥数组进行洗牌
                for (int i = KEY_LENGTH - 1; i > 0; i--)
                {
                    seed = (seed * 1103515245 + 12345) & 0xFFFFFFFF;
                    j = (int)((seed >> 16) % (i + 1));

                    j = (j + passwordBytes[i % passwordLength]) % (i + 1);

                    // 交换元素
                    temp = key[i];
                    key[i] = key[j];
                    key[j] = temp;
                }

                // 对密钥进行二次处理
                for (int i = 0; i < KEY_LENGTH; i++)
                {
                    key[i] = (byte)((key[i] ^ passwordBytes[i % passwordLength]) & 0xFF);
                }

                return key;
            }

            /// <summary>
            /// 加密数据 - 使用增强的XOR算法
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static uint EncryptDataWithMask(byte[] key, byte[] data, int start, uint mask)
            {
                if (data == null || data.Length == 0 || key == null || key.Length == 0)
                    return mask;

                uint resultMask = mask;
                int keyIndex;
                byte temp;

                for (int i = 0; i < data.Length; i++)
                {
                    // 计算密钥索引
                    keyIndex = (i + start + (int)(resultMask & 0xFF)) % KEY_LENGTH;

                    // 使用密钥和掩码进行XOR操作
                    temp = (byte)(data[i] ^ key[keyIndex]);
                    temp = (byte)(temp ^ ((byte)(resultMask >> 8)));

                    // 更新掩码
                    resultMask = (resultMask * 131 + data[i] + 1) & 0xFFFFFFFF; // 使用原始数据更新掩码

                    // 存储加密结果
                    data[i] = temp;
                }

                return resultMask;
            }

            /// <summary>
            /// 解密数据 - 与加密过程对称
            /// </summary>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static uint DecryptDataWithMask(byte[] key, byte[] data, int start, uint mask)
            {
                if (data == null || data.Length == 0 || key == null || key.Length == 0)
                    return mask;

                uint resultMask = mask;
                int keyIndex;
                byte temp, originalByte;

                for (int i = 0; i < data.Length; i++)
                {
                    // 计算密钥索引（与加密时相同）
                    keyIndex = (i + start + (int)(resultMask & 0xFF)) % KEY_LENGTH;

                    // 保存加密后的字节
                    temp = data[i];

                    // 解密操作（与加密相同，因为XOR是对称的）
                    originalByte = (byte)(temp ^ key[keyIndex]);
                    originalByte = (byte)(originalByte ^ ((byte)(resultMask >> 8)));

                    // 更新掩码（使用解密前的数据，与加密时保持一致）
                    resultMask = (resultMask * 131 + originalByte + 1) & 0xFFFFFFFF;

                    // 存储解密结果
                    data[i] = originalByte;
                }

                return resultMask;
            }

            #endregion

            #region 测试方法

            /// <summary>
            /// 测试加密解密功能是否正常工作
            /// </summary>
            /// <returns>测试是否成功</returns>
            public static bool TestEncryptionDecryption()
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine("开始加密解密测试...");

                    // 测试1: 基本数据加密解密
                    byte cmd = 0x01;
                    byte[] testData1 = Encoding.UTF8.GetBytes("测试数据第一部分");
                    byte[] testData2 = Encoding.UTF8.GetBytes("测试数据第二部分");
                    var originalData = new OriginalData(cmd, testData1, testData2);

                    System.Diagnostics.Debug.WriteLine("原始数据:");
                    System.Diagnostics.Debug.WriteLine($"Cmd: {originalData.Cmd}");
                    System.Diagnostics.Debug.WriteLine($"Data1: {Encoding.UTF8.GetString(originalData.One)}");
                    System.Diagnostics.Debug.WriteLine($"Data2: {Encoding.UTF8.GetString(originalData.Two)}");

                    // 服务器加密
                    var encryptedData = EncryptServerDataToClient(originalData);
                    System.Diagnostics.Debug.WriteLine($"加密完成，头部长度: {encryptedData.Head.Length}, 数据1长度: {encryptedData.One.Length}, 数据2长度: {encryptedData.Two.Length}");

                    // 构建完整的数据包
                    var packetBytes = new byte[encryptedData.Length];
                    Buffer.BlockCopy(encryptedData.Head, 0, packetBytes, 0, encryptedData.Head.Length);
                    Buffer.BlockCopy(encryptedData.One, 0, packetBytes, encryptedData.Head.Length, encryptedData.One.Length);
                    Buffer.BlockCopy(encryptedData.Two, 0, packetBytes, encryptedData.Head.Length + encryptedData.One.Length, encryptedData.Two.Length);

                    // 客户端解密
                    var decryptedData = DecryptServerPacket(packetBytes);

                    System.Diagnostics.Debug.WriteLine("解密数据:");
                    System.Diagnostics.Debug.WriteLine($"Cmd: {decryptedData.Cmd}");
                    System.Diagnostics.Debug.WriteLine($"Data1: {Encoding.UTF8.GetString(decryptedData.One)}");
                    System.Diagnostics.Debug.WriteLine($"Data2: {Encoding.UTF8.GetString(decryptedData.Two)}");

                    // 验证
                    bool cmdMatch = decryptedData.Cmd == cmd;
                    bool data1Match = AreByteArraysEqual(decryptedData.One, testData1);
                    bool data2Match = AreByteArraysEqual(decryptedData.Two, testData2);

                    if (cmdMatch && data1Match && data2Match)
                    {
                        System.Diagnostics.Debug.WriteLine("✅ 加密解密测试成功!");
                        return true;
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"❌ 加密解密测试失败! Cmd匹配: {cmdMatch}, 数据1匹配: {data1Match}, 数据2匹配: {data2Match}");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"❌ 测试过程中发生错误: {ex.Message}");
                    System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                    return false;
                }
            }

            /// <summary>
            /// 比较两个字节数组是否相等
            /// </summary>
            private static bool AreByteArraysEqual(byte[] a, byte[] b)
            {
                if (ReferenceEquals(a, b))
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
        }
   
}
