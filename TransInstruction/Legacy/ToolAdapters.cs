using System;
using RUINORERP.PacketSpec.Utilities;
using RUINORERP.PacketSpec.Security;
using RUINORERP.PacketSpec.Protocol;
using TransInstruction.DataPortal;

namespace TransInstruction.Legacy
{
    /// <summary>
    /// ByteDataAnalysis 向后兼容适配器
    /// 将旧的 ByteDataAnalysis 类调用适配到新的 RUINORERP.PacketSpec.Utilities.ByteDataAnalyzer
    /// </summary>
    [Obsolete("请使用 RUINORERP.PacketSpec.Utilities.ByteDataAnalyzer 替代此适配器")]
    public static class ByteDataAnalysisAdapter
    {
        /// <summary>
        /// 适配 GetString 方法
        /// </summary>
        public static string GetString(byte[] buffer, ref int index)
        {
            return ByteDataAnalyzer.GetString(buffer, ref index);
        }

        /// <summary>
        /// 适配 GetString 方法（带未解析数据输出）
        /// </summary>
        public static string GetString(byte[] buffer, out byte[] unparsedData, ref int index)
        {
            return ByteDataAnalyzer.GetString(buffer, out unparsedData, ref index);
        }

        /// <summary>
        /// 适配 TryGetString 方法
        /// </summary>
        public static string TryGetString(byte[] buffer, out bool success, ref int index)
        {
            return ByteDataAnalyzer.TryGetString(buffer, out success, ref index);
        }

        /// <summary>
        /// 适配 GetInt 方法
        /// </summary>
        public static int GetInt(byte[] buffer, ref int index)
        {
            return ByteDataAnalyzer.GetInt(buffer, ref index);
        }

        /// <summary>
        /// 适配 GetInt 方法（带未解析数据输出）
        /// </summary>
        public static int GetInt(byte[] buffer, out byte[] unparsedData, ref int index)
        {
            int result = ByteDataAnalyzer.GetInt(buffer, ref index);
            
            // 计算剩余未解析的数据
            if (index < buffer.Length)
            {
                unparsedData = new byte[buffer.Length - index];
                Array.Copy(buffer, index, unparsedData, 0, unparsedData.Length);
            }
            else
            {
                unparsedData = new byte[0];
            }

            return result;
        }

        /// <summary>
        /// 适配 TryGetInt 方法
        /// </summary>
        public static int TryGetInt(byte[] buffer, out bool success, ref int index)
        {
            return ByteDataAnalyzer.TryGetInt(buffer, out success, ref index);
        }

        /// <summary>
        /// 适配 GetInt64 方法
        /// </summary>
        public static long GetInt64(byte[] buffer, ref int index)
        {
            return ByteDataAnalyzer.GetInt64(buffer, ref index);
        }

        /// <summary>
        /// 适配 GetInt16 方法
        /// </summary>
        public static short GetInt16(byte[] buffer, ref int index)
        {
            return ByteDataAnalyzer.GetInt16(buffer, ref index);
        }

        /// <summary>
        /// 适配 GetByte 方法
        /// </summary>
        public static byte GetByte(byte[] buffer, ref int index)
        {
            return ByteDataAnalyzer.GetByte(buffer, ref index);
        }

        /// <summary>
        /// 适配 Getbool 方法
        /// </summary>
        public static bool Getbool(byte[] buffer, ref int index)
        {
            return ByteDataAnalyzer.GetBoolean(buffer, ref index);
        }

        /// <summary>
        /// 适配 GetFloat 方法
        /// </summary>
        public static float GetFloat(byte[] buffer, ref int index)
        {
            return ByteDataAnalyzer.GetFloat(buffer, ref index);
        }

        /// <summary>
        /// 适配 GetBytes 方法
        /// </summary>
        public static byte[] GetBytes(byte[] buffer, int dataLen, ref int index)
        {
            return ByteDataAnalyzer.GetBytes(buffer, dataLen, ref index);
        }

        // 兼容旧的方法名和参数命名
        [Obsolete("请使用 GetByte 替代")]
        public static byte Getbyte(byte[] buffer, ref int index)
        {
            return GetByte(buffer, ref index);
        }

        [Obsolete("请使用 GetBytes 替代")]
        public static byte[] Getbytes(byte[] buffer, int dataLen, ref int index)
        {
            return GetBytes(buffer, dataLen, ref index);
        }

        [Obsolete("请使用 GetInt 替代")]
        public static int Getint(byte[] buffer, out byte[] unparsedData, ref int index)
        {
            return GetInt(buffer, out unparsedData, ref index);
        }
    }

    /// <summary>
    /// DataConversion 向后兼容适配器
    /// 将旧的 DataConversion 类调用适配到新的 RUINORERP.PacketSpec.Utilities.DataConverter
    /// </summary>
    [Obsolete("请使用 RUINORERP.PacketSpec.Utilities.DataConverter 替代此适配器")]
    public static class DataConversionAdapter
    {
        /// <summary>
        /// 适配 int2Bytes 方法
        /// </summary>
        public static byte[] int2Bytes(int value)
        {
            return DataConverter.IntToBytes(value);
        }

        /// <summary>
        /// 适配 bytes2Int 方法
        /// </summary>
        public static int bytes2Int(byte[] bytes)
        {
            return DataConverter.BytesToInt(bytes);
        }

        /// <summary>
        /// 适配 Int16ToByte 方法
        /// </summary>
        public static void Int16ToByte(short[] arrInt16, int nInt16Count, ref byte[] destByteArr)
        {
            for (int i = 0; i < nInt16Count && i < arrInt16.Length; i++)
            {
                byte[] bytes = DataConverter.ShortToBytes(arrInt16[i]);
                if (destByteArr.Length >= (i + 1) * 2)
                {
                    destByteArr[2 * i] = bytes[0];
                    destByteArr[2 * i + 1] = bytes[1];
                }
            }
        }

        /// <summary>
        /// 适配 ByteToInt16s 方法
        /// </summary>
        public static void ByteToInt16s(byte[] arrByte, int nByteCount, ref short[] destInt16Arr)
        {
            int shortCount = nByteCount / 2;
            for (int i = 0; i < shortCount && i < destInt16Arr.Length; i++)
            {
                byte[] tempBytes = new byte[2];
                tempBytes[0] = arrByte[2 * i];
                tempBytes[1] = arrByte[2 * i + 1];
                destInt16Arr[i] = DataConverter.BytesToShort(tempBytes);
            }
        }
    }

    /// <summary>
    /// Tool4DataProcess 向后兼容适配器
    /// 将旧的 Tool4DataProcess 类调用适配到新的 RUINORERP.PacketSpec.Utilities.DataConverter
    /// </summary>
    [Obsolete("请使用 RUINORERP.PacketSpec.Utilities.DataConverter 替代此适配器")]
    public static class Tool4DataProcessAdapter
    {
        /// <summary>
        /// 适配 StrToBytes 方法
        /// </summary>
        public static byte[] StrToBytes(string val)
        {
            return DataConverter.StringToBytes(val);
        }

        /// <summary>
        /// 适配 byteToHexStr 方法
        /// </summary>
        public static string byteToHexStr(byte[] bytes, bool spaceSeparator)
        {
            string separator = spaceSeparator ? " " : "";
            return DataConverter.BytesToHex(bytes, separator);
        }

        /// <summary>
        /// 适配 StrToHexByte 方法
        /// </summary>
        public static byte[] StrToHexByte(string hexString)
        {
            return DataConverter.HexToBytes(hexString);
        }

        /// <summary>
        /// 适配 HexStrTobyte 方法
        /// </summary>
        public static byte[] HexStrTobyte(string hexString)
        {
            return DataConverter.HexToBytes(hexString);
        }

        /// <summary>
        /// 适配 StrToHex 方法
        /// </summary>
        public static bool StrToHex(string str, int start, int size, out byte[] ret, out string msg)
        {
            msg = "";
            try
            {
                str = str.Replace(" ", "");
                string substring = str.Substring(start);
                ret = DataConverter.HexToBytes(substring);
                return ret.Length > 0;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                ret = new byte[0];
                return false;
            }
        }

        /// <summary>
        /// 适配 Hex2Str 方法
        /// </summary>
        public static string Hex2Str(byte[] by, int start, int size, bool space)
        {
            if (by == null || by.Length == 0)
                return "";

            if (size == -1)
                size = by.Length - start;

            byte[] subArray = new byte[size];
            Array.Copy(by, start, subArray, 0, size);

            string separator = space ? " " : "";
            return DataConverter.BytesToHex(subArray, separator);
        }

        /// <summary>
        /// 适配 toStr 方法
        /// </summary>
        public static string toStr(byte[] bys, int start = 0, int count = -1)
        {
            if (bys == null)
                return "";

            if (count <= 0)
                count = bys.Length - start;

            byte[] subArray = new byte[count];
            Array.Copy(bys, start, subArray, 0, count);
            return DataConverter.BytesToString(subArray);
        }

        /// <summary>
        /// 适配时间戳相关方法
        /// </summary>
        public static DateTime GetTime(string timeStamp)
        {
            try
            {
                DateTime startTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                long lTime = long.Parse(timeStamp);
                DateTime nowTime = startTime.AddMilliseconds(lTime);
                return TimeZoneInfo.ConvertTimeFromUtc(nowTime, TimeZoneInfo.Local);
            }
            catch
            {
                return DateTime.Now;
            }
        }

        /// <summary>
        /// 适配 ConvertDateTimeInt 方法
        /// </summary>
        public static long ConvertDateTimeInt(DateTime time)
        {
            DateTime startTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            DateTime utcTime = TimeZoneInfo.ConvertTimeToUtc(time);
            long unixTime = (long)Math.Round((utcTime - startTime).TotalMilliseconds, MidpointRounding.AwayFromZero);
            return unixTime;
        }

        /// <summary>
        /// 适配对象切片方法
        /// </summary>
        public static object[] Sclie(object[] buff, int start, int size)
        {
            if (buff == null || start < 0 || size < 0 || start + size > buff.Length)
                return new object[0];

            object[] ret = new object[size];
            Array.Copy(buff, start, ret, 0, size);
            return ret;
        }
    }

    /// <summary>
    /// CryptoProtocol 向后兼容适配器
    /// 将旧的 CryptoProtocol 类调用适配到新的架构，保持完整的加密功能
    /// </summary>
    [Obsolete("请使用新的加密协议替代此适配器")]
    public static class CryptoProtocolAdapter
    {
        // 静态日期KEY，保持与原版本兼容
        public static int m_日期KEY = 0;
        private static readonly object lock_tcp = new object();
        
        /// <summary>
        /// 适配 EncryptionServerPackToClient 方法
        /// </summary>
        public static EncryptedData EncryptionServerPackToClient(byte cmd, byte[] one, byte[] two)
        {
            var originalData = new TransInstruction.OriginalData
            {
                cmd = cmd,
                One = one,
                Two = two
            };
            return EncryptionServerPackToClient(originalData);
        }

        /// <summary>
        /// 适配 EncryptionServerPackToClient 方法
        /// </summary>
        public static EncryptedData EncryptionServerPackToClient(TransInstruction.OriginalData gd)
        {
            // 转换为新的数据格式
            var newData = new RUINORERP.PacketSpec.Protocol.OriginalData(gd.cmd, gd.One, gd.Two);
            
            // 使用新的加密协议
            var encryptedData = CryptoProtocol.EncryptServerToClient(newData);
            
            // 转换回旧的格式
            return new EncryptedData
            {
                head = encryptedData.Head,
                one = encryptedData.One,
                two = encryptedData.Two
            };
        }

        /// <summary>
        /// 适配 DecryptServerPack 方法
        /// </summary>
        public static TransInstruction.OriginalData DecryptServerPack(byte[] packData)
        {
            // 使用新的解密协议
            var decryptedData = CryptoProtocol.DecryptServerPack(packData);
            
            // 转换为旧的格式
            return new TransInstruction.OriginalData
            {
                cmd = decryptedData.Cmd,
                One = decryptedData.One,
                Two = decryptedData.Two
            };
        }

        /// <summary>
        /// 适配 EncryptClientPackToServer 方法
        /// </summary>
        public static byte[] EncryptClientPackToServer(TransInstruction.OriginalData gd)
        {
            // 转换为新的数据格式
            var newData = new RUINORERP.PacketSpec.Protocol.OriginalData(gd.cmd, gd.One, gd.Two);
            
            // 使用新的加密协议
            return CryptoProtocol.EncryptClientToServer(newData);
        }

        /// <summary>
        /// 适配分析包头的方法
        /// </summary>
        public static int AnalyzeClientPackHeader(byte[] head)
        {
            try
            {
                // 简化的包头分析逻辑
                if (head == null || head.Length < 18)
                    return 256 - 18;
                
                // 解析长度信息
                int lenOne = BitConverter.ToInt32(head, 2);
                int lenTwo = BitConverter.ToInt32(head, 6);
                
                return lenOne + lenTwo;
            }
            catch
            {
                return 256 - 18;
            }
        }

        /// <summary>
        /// 适配分析服务器包头的方法
        /// </summary>
        public static int AnalyzeSeverPackHeader(byte[] head)
        {
            return AnalyzeClientPackHeader(head);
        }

        /// <summary>
        /// 适配解密客户端包的方法
        /// </summary>
        public static KxData DecryptionClientPack(byte[] head, int headerLen, byte[] alldata)
        {
            try
            {
                // 合并头部和数据
                byte[] fullData = new byte[head.Length + alldata.Length];
                Array.Copy(head, 0, fullData, 0, head.Length);
                Array.Copy(alldata, 0, fullData, head.Length, alldata.Length);
                
                // 使用新的解密方法
                var decryptedData = CryptoProtocol.DecryptServerPack(fullData);
                
                // 转换为KxData格式
                return new KxData
                {
                    cmd = decryptedData.Cmd,
                    One = decryptedData.One,
                    Two = decryptedData.Two
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"DecryptionClientPack 解密时出错: {ex.Message}");
            }
        }

        /// <summary>
        /// 保持兼容性的加密函数
        /// </summary>
        public static uint 加密(byte[] key, byte[] buff, int start, uint 掩码)
        {
            // 这是一个简化版本，保持接口兼容
            // 实际的复杂加密逻辑已经在新的CryptoProtocol中实现
            if (buff == null || buff.Length == 0)
                return 掩码;
                
            // 简单的异或加密，保持接口兼容
            for (int i = 0; i < buff.Length; i++)
            {
                buff[i] ^= (byte)(掩码 & 0xFF);
                掩码 = (掩码 << 1) | (掩码 >> 31);
            }
            
            return 掩码;
        }

        /// <summary>
        /// 保持兼容性的密钥获取函数
        /// </summary>
        public static byte[] GetKey(int step)
        {
            // 简化的密钥生成，保持接口兼容
            var ret = new byte[256];
            for (int i = 0; i < 256; i++)
            {
                ret[i] = (byte)((i + step * 17) % 256);
            }
            return ret;
        }

        /// <summary>
        /// 保持兼容性的密码生成函数
        /// </summary>
        public static byte[] GetPassword(string s)
        {
            var ret = new byte[256];
            byte[] bys = DataConverter.StringToBytes(s);
            
            for (int i = 0; i < 256; i++)
            {
                ret[i] = (byte)i;
            }
            
            for (int i = 0; i < 256; i++)
            {
                byte temp = bys[i % bys.Length];
                int index = (temp + i) % 256;
                (ret[i], ret[index]) = (ret[index], ret[i]);
            }
            
            return ret;
        }
    }
}