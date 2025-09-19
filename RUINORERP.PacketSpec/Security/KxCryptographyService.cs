using System;
using System.Buffers;
using System.Text;
using RUINORERP.PacketSpec.Models;
using RUINORERP.PacketSpec.Protocol;

namespace RUINORERP.PacketSpec.Security
{
    /// <summary>
    /// 加密服务层 - KX协议加密解密服务（单例模式）
    /// 提供数据包的加密和解密功能
    /// </summary>
    public sealed class KxCryptographyService
    {
        private const int KeySize = 256;
        private const int HeaderLength = 18;
        private static readonly Lazy<KxCryptographyService> _instance = 
            new Lazy<KxCryptographyService>(() => new KxCryptographyService());
        
        private readonly object _dateLock = new object();
        private int _currentDay = -1;
        
        private byte[]? _yesterdayKey;
        private byte[]? _todayKey; 
        private byte[]? _tomorrowKey;
        private byte[]? _constantKey;

        /// <summary>
        /// 获取单例实例
        /// </summary>
        public static KxCryptographyService Instance => _instance.Value;

        private KxCryptographyService() { }
        
        /// <summary>
        /// 分析客户端包头部，获取包体长度
        /// </summary>
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
        /// 解密KX协议数据包
        /// </summary>
        public KxData DecryptClientPackage(ReadOnlySpan<byte> packageData)
        {
            if (packageData.Length < HeaderLength)
                throw new ArgumentException("Package data too short");
                
            var kxData = new KxData();
            byte[] header = new byte[HeaderLength];
            packageData.Slice(0, HeaderLength).CopyTo(header);
            
            // 解密头部
            if (!TryDecryptHeader(header, out int dateKey))
                throw new InvalidOperationException("Failed to decrypt package header");
                
            kxData.Command = header[0];
            
            // 解析长度信息
            int partOneLength = BitConverter.ToInt32(header, 2);
            int partTwoLength = BitConverter.ToInt32(header, 6);
            
            // 解密第一部分数据
            if (partOneLength > 0 && packageData.Length >= HeaderLength + partOneLength)
            {
                kxData.PartOne = new byte[partOneLength];
                packageData.Slice(HeaderLength, partOneLength).CopyTo(kxData.PartOne);
                DecryptData(kxData.PartOne, dateKey);
            }
            
            // 解密第二部分数据
            if (partTwoLength > 0 && packageData.Length >= HeaderLength + partOneLength + partTwoLength)
            {
                kxData.PartTwo = new byte[partTwoLength];
                packageData.Slice(HeaderLength + partOneLength, partTwoLength).CopyTo(kxData.PartTwo);
                DecryptData(kxData.PartTwo, dateKey);
            }
            
            return kxData;
        }
        
        /// <summary>
        /// 加密数据包
        /// </summary>
        public byte[] EncryptPackage(KxData kxData)
        {
            byte[] header = new byte[HeaderLength];
            header[0] = kxData.Command;
            
            // 设置长度信息
            int partOneLength = kxData.PartOne?.Length ?? 0;
            int partTwoLength = kxData.PartTwo?.Length ?? 0;
            
            Buffer.BlockCopy(BitConverter.GetBytes(partOneLength), 0, header, 2, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(partTwoLength), 0, header, 6, 4);
            
            // 计算校验和
            CalculateChecksum(header);
            
            // 加密头部
            int dateKey = GetCurrentDateKey();
            EncryptData(header, dateKey);
            
            // 准备完整数据包
            int totalLength = HeaderLength + partOneLength + partTwoLength;
            byte[] package = new byte[totalLength];
            
            // 复制头部
            Buffer.BlockCopy(header, 0, package, 0, HeaderLength);
            
            // 加密并复制第一部分数据
            if (partOneLength > 0)
            {
                byte[] encryptedPartOne = kxData.PartOne!;
                EncryptData(encryptedPartOne, dateKey);
                Buffer.BlockCopy(encryptedPartOne, 0, package, HeaderLength, partOneLength);
            }
            
            // 加密并复制第二部分数据
            if (partTwoLength > 0)
            {
                byte[] encryptedPartTwo = kxData.PartTwo!;
                EncryptData(encryptedPartTwo, dateKey);
                Buffer.BlockCopy(encryptedPartTwo, 0, package, HeaderLength + partOneLength, partTwoLength);
            }
            
            return package;
        }
        
        /// <summary>
        /// 从OriginalData加密数据包
        /// </summary>
        public byte[] EncryptPackage(OriginalData originalData)
        {
            var kxData = KxData.FromOriginalData(originalData);
            return EncryptPackage(kxData);
        }
        
        /// <summary>
        /// 从EncryptedData解密数据包
        /// </summary>
        public KxData DecryptClientPackage(EncryptedData encryptedData)
        {
            // 将EncryptedData转换为byte数组进行解密
            int totalLength = (encryptedData.Head?.Length ?? 0) + (encryptedData.One?.Length ?? 0) + (encryptedData.Two?.Length ?? 0);
            byte[] packageData = new byte[totalLength];
            
            int offset = 0;
            if (encryptedData.Head != null)
            {
                Buffer.BlockCopy(encryptedData.Head, 0, packageData, offset, encryptedData.Head.Length);
                offset += encryptedData.Head.Length;
            }
            
            if (encryptedData.One != null)
            {
                Buffer.BlockCopy(encryptedData.One, 0, packageData, offset, encryptedData.One.Length);
                offset += encryptedData.One.Length;
            }
            
            if (encryptedData.Two != null)
            {
                Buffer.BlockCopy(encryptedData.Two, 0, packageData, offset, encryptedData.Two.Length);
            }
            
            return DecryptClientPackage(packageData);
        }
        
        /// <summary>
        /// 尝试解密头部
        /// </summary>
        private bool TryDecryptHeader(byte[] header, out int dateKey)
        {
            for (int i = 0; i < 4; i++)
            {
                byte[] key = GetKey(i);
                byte[] testHeader = (byte[])header.Clone();
                
                DecryptData(testHeader, i);
                
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
        /// 验证头部校验和
        /// </summary>
        private bool VerifyHeaderChecksum(byte[] header)
        {
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
        private void CalculateChecksum(byte[] header)
        {
            byte hi = 0, low = 0;
            for (int i = 0; i < 9; i++)
            {
                hi ^= header[i * 2];
                low ^= header[i * 2 + 1];
            }
            header[16] = hi;
            header[17] = low;
        }
        
        /// <summary>
        /// 加密数据
        /// </summary>
        private void EncryptData(byte[] data, int dateKey)
        {
            byte[] key = GetKey(dateKey);
            uint mask = 0;
            
            for (int i = 0; i < data.Length; i++)
            {
                byte position = (byte)(i + 1);
                byte keyByte = key[position];
                
                byte temp3 = (byte)(keyByte + (byte)(mask >> 24));
                byte temp2 = key[temp3];
                
                key[position] = temp2;
                key[temp3] = keyByte;
                
                byte temp1 = key[(byte)(keyByte + temp2)];
                data[i] = (byte)(data[i] ^ temp1);
                
                mask = (uint)(temp1 << 24) | (uint)(temp2 << 16) | (uint)(temp3 << 8) | data[i];
            }
        }
        
        /// <summary>
        /// 解密数据
        /// </summary>
        private void DecryptData(byte[] data, int dateKey)
        {
            // 解密使用相同的算法
            EncryptData(data, dateKey);
        }
        
        /// <summary>
        /// 获取加密密钥
        /// </summary>
        private byte[] GetKey(int step)
        {
            InitializeDateKeys();
            
            return step switch
            {
                0 => _constantKey!,
                1 => _todayKey!,
                2 => _yesterdayKey!,
                3 => _tomorrowKey!,
                _ => throw new ArgumentOutOfRangeException(nameof(step))
            };
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
        
        /// <summary>
        /// 从字符串生成密钥
        /// </summary>
        private byte[] GenerateKeyFromString(string s)
        {
            byte[] key = new byte[KeySize];
            byte[] sourceBytes = Encoding.UTF8.GetBytes(s);
            int sourceLength = sourceBytes.Length;
            
            // 初始化密钥
            for (int i = 0; i < KeySize; i++)
            {
                key[i] = (byte)i;
            }
            
            // 密钥调度算法
            byte temp1 = 0, temp2 = 0, temp3 = 0;
            for (int i = 0; i < KeySize; i++)
            {
                temp1 = sourceBytes[i % sourceLength];
                temp2 = key[i];
                temp3 = (byte)(temp1 + temp3 + temp2);
                
                byte temp4 = key[temp3];
                key[i] = temp4;
                key[temp3] = temp2;
            }
            
            return key;
        }
        
        /// <summary>
        /// 获取当前日期密钥索引
        /// </summary>
        private int GetCurrentDateKey()
        {
            return 1; // 默认使用今天密钥
        }
    }
    
    /// <summary>
    /// 加密服务层 - KX协议数据模型
    /// 用于在加密服务层和业务逻辑层之间传递数据
    /// </summary>
    public class KxData
    {
        /// <summary>
        /// 指令码 - 新架构使用
        /// </summary>
        public byte Command { get; set; }
        
        /// <summary>
        /// 第一部分数据 - 新架构使用
        /// </summary>
        public byte[]? PartOne { get; set; }
        
        /// <summary>
        /// 第二部分数据 - 新架构使用
        /// </summary>
        public byte[]? PartTwo { get; set; }

        /// <summary>
        /// 指令码 - 兼容旧系统使用
        /// </summary>
        public byte cmd { get => Command; set => Command = value; }
        
        /// <summary>
        /// 第一部分数据 - 兼容旧系统使用
        /// </summary>
        public byte[] One { get => PartOne; set => PartOne = value; }
        
        /// <summary>
        /// 第二部分数据 - 兼容旧系统使用
        /// </summary>
        public byte[] Two { get => PartTwo; set => PartTwo = value; }
        
        ///// <summary>
        ///// 从BusinessPacket创建KxData
        ///// </summary>
        ///// <param name="businessPacket">业务数据包</param>
        ///// <returns>KxData对象</returns>
        //public static KxData FromBusinessPacket(BusinessPacket businessPacket)
        //{
        //    return new KxData
        //    {
        //        Command = (byte)Math.Min(businessPacket.Cmd, 255),
        //        PartOne = businessPacket.One,
        //        PartTwo = businessPacket.Two
        //    };
        //}
        
        /// <summary>
        /// 从OriginalData创建KxData
        /// </summary>
        /// <param name="originalData">原始数据包</param>
        /// <returns>KxData对象</returns>
        public static KxData FromOriginalData(OriginalData originalData)
        {
            return new KxData
            {
                Command = originalData.Cmd,
                PartOne = originalData.One,
                PartTwo = originalData.Two
            };
        }
        
        /// <summary>
        /// 转换为OriginalData
        /// </summary>
        /// <returns>OriginalData结构体</returns>
        public OriginalData ToOriginalData()
        {
            return new OriginalData(
                Command,
                PartOne,
                PartTwo
            );
        }
    }

    /// <summary>
    /// KX协议加密静态工具类 - 为向后兼容提供静态方法
    /// </summary>
    public static class KxCryptographyStatic
    {
        private static readonly object _dateLock = new object();
        private static int _currentDay = -1;
        
        private static byte[]? _yesterdayKey;
        private static byte[]? _todayKey; 
        private static byte[]? _tomorrowKey;
        private static byte[]? _constantKey;

        /// <summary>
        /// 解码/加密方法 - 向后兼容
        /// </summary>
        public static uint Encrypt(byte[] KEY, byte[] buff, int start, uint mask)
        {
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
                temp3 = (byte)(temp1 + temp3);//保存变量
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
        /// 获取KEY - 向后兼容
        /// </summary>
        public static byte[] GetKey(int step)
        {
            var ret = new byte[256];
            if (_currentDay != DateTime.Now.Day)
            {
                lock (_dateLock)
                {
                    if (_currentDay != DateTime.Now.Day)
                    {
                        var tm = DateTime.Now;
                        tm = tm.AddDays(-1);
                        var s = tm.Year + "年" + tm.Month + "月" + tm.Day + "日";
                        _yesterdayKey = GetPassword(s);

                        tm = tm.AddDays(1);
                        s = tm.Year + "年" + tm.Month + "月" + tm.Day + "日";
                        _todayKey = GetPassword(s);

                        tm = tm.AddDays(1);
                        s = tm.Year + "年" + tm.Month + "月" + tm.Day + "日";
                        _tomorrowKey = GetPassword(s);

                        s = "woaikaixuan";
                        _constantKey = GetPassword(s);
                        _currentDay = DateTime.Now.Day;
                    }
                }
            }
            
            switch (step)
            {
                case 0:
                    Array.Copy(_constantKey, ret, 256);
                    break;
                case 1:
                    Array.Copy(_todayKey, ret, 256);
                    break;
                case 2:
                    Array.Copy(_yesterdayKey, ret, 256);
                    break;
                case 3:
                    Array.Copy(_tomorrowKey, ret, 256);
                    break;
            }
            return ret;
        }

        /// <summary>
        /// 获取密码 - 向后兼容
        /// </summary>
        public static byte[] GetPassword(string s)
        {
            var ret = new byte[256];
            byte[] bys = Encoding.UTF8.GetBytes(s);
            int tmlen = bys.Length;
            byte temp1, temp2, temp3, temp4;
            temp1 = temp2 = temp3 = temp4 = 0;

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
    }
}
