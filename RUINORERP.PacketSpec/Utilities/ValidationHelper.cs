using System;
using System.Text.RegularExpressions;

namespace RUINORERP.PacketSpec.Utilities
{
    /// <summary>
    /// 数据验证工具类
    /// </summary>
    public static class ValidationHelper
    {



        /// <summary>
        /// 会话令牌正则表达式
        /// </summary>
        private static readonly Regex SessionTokenRegex = new Regex(@"^[a-zA-Z0-9_\-]{32,64}$", RegexOptions.Compiled);

        /// <summary>
        /// 用户名正则表达式
        /// </summary>
        private static readonly Regex UsernameRegex = new Regex(@"^[a-zA-Z0-9_\-]{3,50}$", RegexOptions.Compiled);

        /// <summary>
        /// 设备ID正则表达式
        /// </summary>
        private static readonly Regex DeviceIdRegex = new Regex(@"^[a-zA-Z0-9_\-]{8,100}$", RegexOptions.Compiled);

        /// <summary>
        /// IP地址正则表达式
        /// </summary>
        private static readonly Regex IpAddressRegex = new Regex(
            @"^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$", 
            RegexOptions.Compiled);

        /// <summary>
        /// 验证会话令牌格式
        /// </summary>
        public static bool ValidateSessionToken(string token)
        {
            return !string.IsNullOrEmpty(token) && SessionTokenRegex.IsMatch(token);
        }

        /// <summary>
        /// 验证用户名格式
        /// </summary>
        public static bool ValidateUsername(string username)
        {
            return !string.IsNullOrEmpty(username) && UsernameRegex.IsMatch(username);
        }

        /// <summary>
        /// 验证设备ID格式
        /// </summary>
        public static bool ValidateDeviceId(string deviceId)
        {
            return !string.IsNullOrEmpty(deviceId) && DeviceIdRegex.IsMatch(deviceId);
        }

        /// <summary>
        /// 验证IP地址格式
        /// </summary>
        public static bool ValidateIpAddress(string ipAddress)
        {
            return !string.IsNullOrEmpty(ipAddress) && IpAddressRegex.IsMatch(ipAddress);
        }

        /// <summary>
        /// 验证时间戳有效性（允许5分钟误差）
        /// </summary>
        public static bool ValidateTimestamp(DateTime timestamp, TimeSpan? allowedDrift = null)
        {
            var drift = allowedDrift ?? TimeSpan.FromMinutes(5);
            var now = DateTime.Now;
            return timestamp >= now - drift && timestamp <= now + drift;
        }

        /// <summary>
        /// 验证数值范围
        /// </summary>
        public static bool ValidateRange(int value, int min, int max)
        {
            return value >= min && value <= max;
        }

        /// <summary>
        /// 验证数值范围
        /// </summary>
        public static bool ValidateRange(long value, long min, long max)
        {
            return value >= min && value <= max;
        }

        /// <summary>
        /// 验证数值范围
        /// </summary>
        public static bool ValidateRange(float value, float min, float max)
        {
            return value >= min && value <= max;
        }

        /// <summary>
        /// 验证字符串长度
        /// </summary>
        public static bool ValidateStringLength(string value, int minLength, int maxLength)
        {
            if (value == null)
                return minLength == 0;

            return value.Length >= minLength && value.Length <= maxLength;
        }

        /// <summary>
        /// 验证字节数组长度
        /// </summary>
        public static bool ValidateByteArrayLength(byte[] data, int minLength, int maxLength)
        {
            if (data == null)
                return minLength == 0;

            return data.Length >= minLength && data.Length <= maxLength;
        }

        /// <summary>
        /// 验证枚举值有效性
        /// </summary>
        public static bool ValidateEnumValue<TEnum>(int value) where TEnum : Enum
        {
            return Enum.IsDefined(typeof(TEnum), value);
        }

        /// <summary>
        /// 验证枚举值有效性
        /// </summary>
        public static bool ValidateEnumValue<TEnum>(string value) where TEnum : struct
        {
            return Enum.TryParse<TEnum>(value, out _);
        }

        /// <summary>
        /// 验证指令编码有效性
        /// </summary>
        public static bool ValidateCommandEncoding(uint encodedCommand)
        {
            // 指令不能为0
            if (encodedCommand == 0)
                return false;

            // 主指令部分应该在合理范围内
            var mainCommand = encodedCommand >> 16;
            return mainCommand > 0 && mainCommand < 0xFFFFF;
        }

        /// <summary>
        /// 验证数据包大小
        /// </summary>
        public static bool ValidatePacketSize(int packetSize, int maxSize = 1024 * 64)
        {
            return packetSize > 0 && packetSize <= maxSize;
        }

        /// <summary>
        /// 安全解析整数
        /// </summary>
        public static int SafeParseInt(string value, int defaultValue = 0)
        {
            if (int.TryParse(value, out int result))
                return result;
            return defaultValue;
        }

        /// <summary>
        /// 安全解析布尔值
        /// </summary>
        public static bool SafeParseBool(string value, bool defaultValue = false)
        {
            if (bool.TryParse(value, out bool result))
                return result;
            return defaultValue;
        }

        /// <summary>
        /// 安全解析枚举值
        /// </summary>
        public static TEnum SafeParseEnum<TEnum>(string value, TEnum defaultValue) where TEnum : struct
        {
            if (Enum.TryParse<TEnum>(value, out TEnum result))
                return result;
            return defaultValue;
        }

  
        /// <summary>
        /// 清理敏感数据
        /// </summary>
        public static void ClearSensitiveData(ref string sensitiveData)
        {
            sensitiveData = null;
        }

        /// <summary>
        /// 清理敏感字节数组
        /// </summary>
        public static void ClearSensitiveData(ref byte[] sensitiveData)
        {
            if (sensitiveData != null)
            {
                Array.Clear(sensitiveData, 0, sensitiveData.Length);
                sensitiveData = null;
            }
        }
    }
}
