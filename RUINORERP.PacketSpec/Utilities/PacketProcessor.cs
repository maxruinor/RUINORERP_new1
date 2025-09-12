using System;
using RUINORERP.PacketSpec.Protocol;

namespace RUINORERP.PacketSpec.Utilities
{
    /// <summary>
    /// 数据包处理器 - 负责数据包的打包和解包操作
    /// </summary>
    public static class PacketProcessor
    {
        /// <summary>
        /// 解包客户端数据包
        /// </summary>
        public static OriginalData UnpackClientData(byte[] allData)
        {
            if (allData == null || allData.Length < 18)
            {
                throw new ArgumentException("Invalid client data packet length");
            }

            var result = new OriginalData
            {
                Cmd = allData[0],
                One = new byte[allData.Length - 2],
                Two = new byte[2]
            };

            // 复制One数据
            Array.Copy(allData, 1, result.One, 0, result.One.Length);
            
            // 复制Two数据（最后2字节）
            if (allData.Length >= 2)
            {
                result.Two[0] = allData[^2];
                result.Two[1] = allData[^1];
            }

            return result;
        }

        /// <summary>
        /// 解包服务器数据包
        /// </summary>
        public static OriginalData UnpackServerData(byte[] packData)
        {
            if (packData == null || packData.Length < 4)
            {
                throw new ArgumentException("Invalid server data packet length");
            }

            var result = new OriginalData
            {
                Cmd = packData[0],
                One = new byte[packData.Length - 2],
                Two = new byte[2]
            };

            // 复制One数据
            Array.Copy(packData, 1, result.One, 0, result.One.Length);
            
            // 复制Two数据（最后2字节）
            if (packData.Length >= 2)
            {
                result.Two[0] = packData[^2];
                result.Two[1] = packData[^1];
            }

            return result;
        }

        /// <summary>
        /// 打包客户端数据包
        /// </summary>
        public static byte[] PackClientData(OriginalData data)
        {
            int totalLength = 1 + (data.One?.Length ?? 0) + (data.Two?.Length ?? 0);
            var result = new byte[totalLength];
            int index = 0;

            result[index++] = data.Cmd;

            if (data.One != null)
            {
                Array.Copy(data.One, 0, result, index, data.One.Length);
                index += data.One.Length;
            }

            if (data.Two != null)
            {
                Array.Copy(data.Two, 0, result, index, data.Two.Length);
            }

            return result;
        }

        /// <summary>
        /// 打包服务器数据包
        /// </summary>
        public static byte[] PackServerData(OriginalData data)
        {
            int totalLength = 1 + (data.One?.Length ?? 0) + (data.Two?.Length ?? 0);
            var result = new byte[totalLength];
            int index = 0;

            result[index++] = data.Cmd;

            if (data.One != null)
            {
                Array.Copy(data.One, 0, result, index, data.One.Length);
                index += data.One.Length;
            }

            if (data.Two != null)
            {
                Array.Copy(data.Two, 0, result, index, data.Two.Length);
            }

            return result;
        }

        /// <summary>
        /// 将数据包转换为十六进制字符串（用于调试）
        /// </summary>
        public static string ToHexString(byte[] data)
        {
            return BitConverter.ToString(data).Replace("-", " ");
        }

        /// <summary>
        /// 从十六进制字符串解析数据包
        /// </summary>
        public static byte[] FromHexString(string hexString)
        {
            hexString = hexString.Replace(" ", "").Replace("-", "");
            
            if (hexString.Length % 2 != 0)
            {
                throw new ArgumentException("Hex string must have even length");
            }

            byte[] result = new byte[hexString.Length / 2];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }

            return result;
        }

        /// <summary>
        /// 验证数据包完整性
        /// </summary>
        public static bool ValidatePacket(byte[] data, int expectedLength)
        {
            return data != null && data.Length >= expectedLength;
        }

        /// <summary>
        /// 计算数据包校验和
        /// </summary>
        public static byte CalculateChecksum(byte[] data)
        {
            if (data == null || data.Length == 0)
                return 0;

            byte checksum = 0;
            foreach (byte b in data)
            {
                checksum ^= b;
            }
            return checksum;
        }
    }
}