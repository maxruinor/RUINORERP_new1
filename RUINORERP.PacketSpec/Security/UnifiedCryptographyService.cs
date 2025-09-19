using System;
using System.Buffers;
using System.Text;
using RUINORERP.PacketSpec.Core.DataProcessing;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Protocol;

namespace RUINORERP.PacketSpec.Security
{
    /// <summary>
    /// 统一加密服务 - 提供完整的KX协议加密解密功能
    /// 合并了原有的CryptographyService、KxCryptographyService、CryptographyCore和KeyManagementService功能
    /// </summary>
    public sealed class UnifiedCryptographyService
    {
        private const int HeaderLength = 18;
        private const int KeySize = 256;
        private static readonly Lazy<UnifiedCryptographyService> _instance = 
            new Lazy<UnifiedCryptographyService>(() => new UnifiedCryptographyService());

        private readonly object _dateLock = new object();
        private int _currentDay = -1;
        
        private byte[]? _yesterdayKey;
        private byte[]? _todayKey; 
        private byte[]? _tomorrowKey;
        private byte[]? _constantKey;

        /// <summary>
        /// 获取单例实例
        /// </summary>
        public static UnifiedCryptographyService Instance => _instance.Value;

        private UnifiedCryptographyService()
        {
            // 初始化密钥
            InitializeDateKeys();
        }

        #region CryptographyCore功能

        /// <summary>
        /// 加密数据
        /// </summary>
        /// <param name="data">要加密的数据</param>
        /// <param name="key">加密密钥</param>
        /// <param name="startIndex">起始索引</param>
        /// <param name="initialMask">初始掩码</param>
        /// <returns>加密后的掩码</returns>
        public static uint EncryptData(byte[] data, byte[] key, int startIndex = 0, uint initialMask = 0)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (startIndex < 0) throw new ArgumentOutOfRangeException(nameof(startIndex));

            byte temp1 = (byte)(initialMask >> 24);
            byte temp2 = (byte)(initialMask >> 16);
            byte temp3 = (byte)(initialMask >> 8);
            byte temp4 = (byte)initialMask;

            for (int i = 0; i < data.Length; i++)
            {
                byte position = (byte)(i + 1 + startIndex);
                temp1 = key[position];
                temp3 = (byte)(temp1 + temp3);
                temp2 = key[temp3];
                key[position] = temp2;
                key[temp3] = temp1;

                temp2 = (byte)(temp1 + temp2);
                temp1 = key[temp2];
                temp4 = data[i];

                temp4 = (byte)(temp4 ^ temp1);
                data[i] = temp4;
            }

            return ((uint)temp1 << 24) | ((uint)temp2 << 16) | ((uint)temp3 << 8) | temp4;
        }

        /// <summary>
        /// 解密数据（加密算法的逆操作）
        /// </summary>
        /// <param name="data">要解密的数据</param>
        /// <param name="key">解密密钥</param>
        /// <param name="startIndex">起始索引</param>
        /// <param name="initialMask">初始掩码</param>
        /// <returns>解密后的掩码</returns>
        public static uint DecryptData(byte[] data, byte[] key, int startIndex = 0, uint initialMask = 0)
        {
            // KX协议加密解密使用相同算法
            return EncryptData(data, key, startIndex, initialMask);
        }

        /// <summary>
        /// 从字符串生成密钥
        /// </summary>
        /// <param name="seedString">种子字符串</param>
        /// <returns>生成的密钥</returns>
        public static byte[] GenerateKeyFromString(string seedString)
        {
            if (string.IsNullOrEmpty(seedString))
                throw new ArgumentException("种子字符串不能为空", nameof(seedString));

            byte[] key = new byte[KeySize];
            byte[] sourceBytes = System.Text.Encoding.UTF8.GetBytes(seedString);
            int sourceLength = sourceBytes.Length;

            // 初始化密钥
            for (int i = 0; i < KeySize; i++)
            {
                key[i] = (byte)i;
            }

            // 密钥调度算法
            byte temp3 = 0;
            for (int i = 0; i < KeySize; i++)
            {
                byte temp1 = sourceBytes[i % sourceLength];
                byte temp2 = key[i];
                temp3 = (byte)(temp1 + temp3 + temp2);

                byte temp4 = key[temp3];
                key[i] = temp4;
                key[temp3] = temp2;
            }

            return key;
        }

        /// <summary>
        /// 验证头部校验和
        /// </summary>
        /// <param name="header">包头部数据</param>
        /// <returns>校验是否通过</returns>
        public static bool VerifyHeaderChecksum(byte[] header)
        {
            if (header == null || header.Length < 18)
                return false;

            byte hi = 0, low = 0;
            for (int i = 0; i < 9; i++)
            {
                hi ^= header[i * 2];
                low ^= header[i * 2 + 1];
            }
            return hi == 0 && low == 0;
        }

        /// <summary>
        /// 计算头部校验和
        /// </summary>
        /// <param name="header">包头部数据</param>
        public static void CalculateHeaderChecksum(byte[] header)
        {
            if (header == null || header.Length < 18)
                throw new ArgumentException("头部数据长度不足", nameof(header));

            byte hi = 0, low = 0;
            for (int i = 0; i < 9; i++)
            {
                hi ^= header[i * 2];
                low ^= header[i * 2 + 1];
            }
            header[16] = hi;
            header[17] = low;
        }

        #endregion

        #region KeyManagementService功能

        /// <summary>
        /// 获取指定步骤的密钥
        /// </summary>
        /// <param name="step">密钥步骤 (0=常量密钥, 1=今天密钥, 2=昨天密钥, 3=明天密钥)</param>
        /// <returns>对应的密钥</returns>
        public byte[] GetKey(int step)
        {
            InitializeDateKeys();
            
            return step switch
            {
                0 => _constantKey!,
                1 => _todayKey!,
                2 => _yesterdayKey!,
                3 => _tomorrowKey!,
                _ => throw new ArgumentOutOfRangeException(nameof(step), "无效的密钥步骤")
            };
        }

        /// <summary>
        /// 获取当前日期密钥索引
        /// </summary>
        /// <returns>当前日期密钥索引</returns>
        public int GetCurrentDateKeyIndex()
        {
            return 1; // 默认使用今天密钥
        }

        /// <summary>
        /// 从密码字符串生成密钥
        /// </summary>
        /// <param name="password">密码字符串</param>
        /// <returns>生成的密钥</returns>
        public byte[] GenerateKeyFromPassword(string password)
        {
            return GenerateKeyFromString(password);
        }

        /// <summary>
        /// 获取常量密钥
        /// </summary>
        /// <returns>常量密钥</returns>
        public byte[] GetConstantKey()
        {
            InitializeDateKeys();
            return _constantKey!;
        }

        /// <summary>
        /// 获取今天密钥
        /// </summary>
        /// <returns>今天密钥</returns>
        public byte[] GetTodayKey()
        {
            InitializeDateKeys();
            return _todayKey!;
        }

        /// <summary>
        /// 获取昨天密钥
        /// </summary>
        /// <returns>昨天密钥</returns>
        public byte[] GetYesterdayKey()
        {
            InitializeDateKeys();
            return _yesterdayKey!;
        }

        /// <summary>
        /// 获取明天密钥
        /// </summary>
        /// <returns>明天密钥</returns>
        public byte[] GetTomorrowKey()
        {
            InitializeDateKeys();
            return _tomorrowKey!;
        }

        /// <summary>
        /// 强制重新生成所有密钥
        /// </summary>
        public void RegenerateAllKeys()
        {
            lock (_dateLock)
            {
                _currentDay = -1;
                InitializeDateKeys();
            }
        }

        /// <summary>
        /// 初始化日期相关的密钥
        /// </summary>
        private void InitializeDateKeys()
        {
            lock (_dateLock)
            {
                if (_currentDay == DateTime.Now.Day)
                    return;
                
                var today = DateTime.Now;
                
                _yesterdayKey = GenerateDateKey(today.AddDays(-1));
                _todayKey = GenerateDateKey(today);
                _tomorrowKey = GenerateDateKey(today.AddDays(1));
                _constantKey = GenerateConstantKey();
                
                _currentDay = today.Day;
            }
        }

        /// <summary>
        /// 生成日期相关的密钥
        /// </summary>
        private byte[] GenerateDateKey(DateTime date)
        {
            string dateString = $"{date:yyyy年MM月dd日}";
            return GenerateKeyFromString(dateString);
        }

        /// <summary>
        /// 生成常量密钥
        /// </summary>
        private byte[] GenerateConstantKey()
        {
            return GenerateKeyFromString("woaikaixuan");
        }

        #endregion

        /// <summary>
        /// 分析客户端包头部，获取包体长度
        /// </summary>
        /// <param name="header">包头部数据</param>
        /// <returns>包体长度</returns>
        public int AnalyzeClientPackHeader(ReadOnlySpan<byte> header)
        {
            if (header.Length < HeaderLength)
                throw new ArgumentException("Header length must be at least 18 bytes");
                
            byte[] decryptedHeader = new byte[HeaderLength];
            header.Slice(0, HeaderLength).CopyTo(decryptedHeader);
            
            // 解密头部
            if (!TryDecryptHeader(decryptedHeader, out int dateKey))
                return 256 - HeaderLength;
                
            // 解析长度信息
            int partOneLength = BitConverter.ToInt32(decryptedHeader, 2);
            int partTwoLength = BitConverter.ToInt32(decryptedHeader, 6);
            
            return partOneLength + partTwoLength;
        }

        /// <summary>
        /// 分析客户端包头部信息（详细版本）
        /// </summary>
        /// <param name="header">包头部数据</param>
        /// <returns>包含密钥步骤、数据长度和命令的元组</returns>
        public (int keyStep, int dataLength, int command) AnalyzeClientPackHeaderDetailed(byte[] header)
        {
            if (header == null || header.Length < HeaderLength)
                throw new ArgumentException("头部数据长度不足", nameof(header));

            // 验证头部校验和
            if (!VerifyHeaderChecksum(header))
                throw new InvalidOperationException("包头部校验和验证失败");

            // 解析密钥步骤、数据长度和命令
            int keyStep = header[0];
            int dataLength = (header[1] << 8) | header[2];
            int command = (header[3] << 8) | header[4];

            return (keyStep, dataLength, command);
        }

        /// <summary>
        /// 解密客户端数据包（字节数组版本）
        /// </summary>
        /// <param name="packetData">包数据</param>
        /// <param name="keyStep">密钥步骤</param>
        /// <returns>解密后的数据</returns>
        public byte[] DecryptClientPackage(byte[] packetData, int keyStep)
        {
            if (packetData == null)
                throw new ArgumentNullException(nameof(packetData));

            byte[] key = GetKey(keyStep);
            byte[] decryptedData = new byte[packetData.Length];
            Buffer.BlockCopy(packetData, 0, decryptedData, 0, packetData.Length);

            DecryptData(decryptedData, key);
            return decryptedData;
        }

        /// <summary>
        /// 加密数据包（字节数组版本）
        /// </summary>
        /// <param name="packetData">包数据</param>
        /// <param name="keyStep">密钥步骤</param>
        /// <returns>加密后的数据</returns>
        public byte[] EncryptPackage(byte[] packetData, int keyStep)
        {
            if (packetData == null)
                throw new ArgumentNullException(nameof(packetData));

            byte[] key = GetKey(keyStep);
            byte[] encryptedData = new byte[packetData.Length];
            Buffer.BlockCopy(packetData, 0, encryptedData, 0, packetData.Length);

            EncryptData(encryptedData, key);
            return encryptedData;
        }

        /// <summary>
        /// 加密数据（公共接口）
        /// </summary>
        /// <param name="data">要加密的数据</param>
        /// <param name="password">密码</param>
        /// <returns>加密后的数据</returns>
        public byte[] Encrypt(byte[] data, string password)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (string.IsNullOrEmpty(password)) throw new ArgumentException("密码不能为空", nameof(password));

            byte[] key = GenerateKeyFromPassword(password);
            byte[] encryptedData = new byte[data.Length];
            Buffer.BlockCopy(data, 0, encryptedData, 0, data.Length);

            EncryptData(encryptedData, key);
            return encryptedData;
        }

        /// <summary>
        /// 解密数据（公共接口）
        /// </summary>
        /// <param name="data">要解密的数据</param>
        /// <param name="password">密码</param>
        /// <returns>解密后的数据</returns>
        public byte[] Decrypt(byte[] data, string password)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (string.IsNullOrEmpty(password)) throw new ArgumentException("密码不能为空", nameof(password));

            byte[] key = GenerateKeyFromPassword(password);
            byte[] decryptedData = new byte[data.Length];
            Buffer.BlockCopy(data, 0, decryptedData, 0, data.Length);

            DecryptData(decryptedData, key);
            return decryptedData;
        }

        /// <summary>
        /// 获取密码（向后兼容）
        /// </summary>
        /// <param name="password">密码字符串</param>
        /// <returns>密码字节数组</returns>
        public byte[] GetPassword(string password)
        {
            return GenerateKeyFromPassword(password);
        }

 
        #region 私有方法

        /// <summary>
        /// 尝试解密头部
        /// </summary>
        private bool TryDecryptHeader(byte[] header, out int dateKey)
        {
            for (int i = 0; i < 4; i++)
            {
                byte[] key = GetKey(i);
                byte[] testHeader = (byte[])header.Clone();
                
                DecryptData(testHeader, key);
                
                if (VerifyHeaderChecksum(testHeader))
                {
                    dateKey = i;
                    Buffer.BlockCopy(testHeader, 0, header, 0, HeaderLength);
                    return true;
                }
            }
            
            dateKey = -1;
            return false;
        }

        /// <summary>
        /// 加密数据
        /// </summary>
        private void EncryptData(byte[] data, int dateKey)
        {
            byte[] key = GetKey(dateKey);
            EncryptData(data, key);
        }

        /// <summary>
        /// 解密数据
        /// </summary>
        private void DecryptData(byte[] data, int dateKey)
        {
            byte[] key = GetKey(dateKey);
            DecryptData(data, key);
        }

        /// <summary>
        /// 加密数据（重载版本）
        /// </summary>
        private void EncryptData(byte[] data, byte[] key)
        {
            EncryptData(data, key, 0, 0);
        }

        /// <summary>
        /// 解密数据（重载版本）
        /// </summary>
        private void DecryptData(byte[] data, byte[] key)
        {
            DecryptData(data, key, 0, 0);
        }

        public byte[] EncryptPackage(OriginalData originalData)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
