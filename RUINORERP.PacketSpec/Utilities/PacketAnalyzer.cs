using System;
using System.Collections.Generic;
using System.Text;
using RUINORERP.PacketSpec.Models.Core;

namespace RUINORERP.PacketSpec.Utilities
{
    /// <summary>
    /// 数据包分析器 - 用于从数据包中提取文本信息和进行协议分析
    /// </summary>
    public static class PacketAnalyzer
    {
        /// <summary>
        /// 从数据包中提取文本内容
        /// </summary>
        public static Dictionary<int, string> ExtractTextContent(OriginalData data)
        {
            var result = new Dictionary<int, string>();

            if (data.One != null)
            {
                ExtractTextFromBytes(data.One, 0, result);
            }

            if (data.Two != null)
            {
                ExtractTextFromBytes(data.Two, data.One?.Length ?? 0, result);
            }

            return result;
        }

        /// <summary>
        /// 从字节数组中提取文本内容
        /// </summary>
        private static void ExtractTextFromBytes(byte[] bytes, int baseOffset, Dictionary<int, string> result)
        {
            int index = 0;
            while (index < bytes.Length)
            {
                // 查找可能的文本起始位置（可打印ASCII字符）
                if (IsPrintableAscii(bytes[index]))
                {
                    int startIndex = index;
                    StringBuilder textBuilder = new StringBuilder();

                    // 收集连续的文本字符
                    while (index < bytes.Length && IsPrintableAscii(bytes[index]))
                    {
                        textBuilder.Append((char)bytes[index]);
                        index++;
                    }

                    string text = textBuilder.ToString();
                    if (text.Length >= 2) // 只记录有意义的文本
                    {
                        result[baseOffset + startIndex] = text;
                    }
                }
                else
                {
                    index++;
                }
            }
        }

        /// <summary>
        /// 检查是否为可打印ASCII字符
        /// </summary>
        private static bool IsPrintableAscii(byte b)
        {
            return b >= 32 && b <= 126; // 可打印ASCII字符范围
        }

        /// <summary>
        /// 分析数据包结构
        /// </summary>
        public static string AnalyzePacketStructure(OriginalData data)
        {
            var analysis = new StringBuilder();
            analysis.AppendLine($"Command: 0x{data.Cmd:X2}");

            if (data.One != null)
            {
                analysis.AppendLine($"One Data Length: {data.One.Length} bytes");
                analysis.AppendLine($"One Data: {BitConverter.ToString(data.One).Replace("-", " ")}");
            }

            if (data.Two != null)
            {
                analysis.AppendLine($"Two Data Length: {data.Two.Length} bytes");
                analysis.AppendLine($"Two Data: {BitConverter.ToString(data.Two).Replace("-", " ")}");
            }

            // 提取文本内容
            var textContent = ExtractTextContent(data);
            if (textContent.Count > 0)
            {
                analysis.AppendLine("Text Content Found:");
                foreach (var kvp in textContent)
                {
                    analysis.AppendLine($"  Offset {kvp.Key}: \"{kvp.Value}\"");
                }
            }

            return analysis.ToString();
        }

        /// <summary>
        /// 解析整数数据
        /// </summary>
        public static int? ParseInt32(byte[] data, ref int index)
        {
            if (data == null || index + 4 > data.Length)
                return null;

            int value = BitConverter.ToInt32(data, index);
            index += 4;
            return value;
        }

        /// <summary>
        /// 解析短整数数据
        /// </summary>
        public static short? ParseInt16(byte[] data, ref int index)
        {
            if (data == null || index + 2 > data.Length)
                return null;

            short value = BitConverter.ToInt16(data, index);
            index += 2;
            return value;
        }

        /// <summary>
        /// 解析字节数据
        /// </summary>
        public static byte? ParseByte(byte[] data, ref int index)
        {
            if (data == null || index >= data.Length)
                return null;

            return data[index++];
        }

        /// <summary>
        /// 解析字符串数据
        /// </summary>
        public static string ParseString(byte[] data, ref int index, int length)
        {
            if (data == null || index + length > data.Length)
                return null;

            string result = Encoding.UTF8.GetString(data, index, length);
            index += length;
            return result;
        }

        /// <summary>
        /// 解析可变长度字符串（以null结尾）
        /// </summary>
        public static string ParseNullTerminatedString(byte[] data, ref int index)
        {
            if (data == null || index >= data.Length)
                return null;

            int startIndex = index;
            while (index < data.Length && data[index] != 0)
            {
                index++;
            }

            int length = index - startIndex;
            if (length == 0)
                return string.Empty;

            string result = Encoding.UTF8.GetString(data, startIndex, length);
            index++; // 跳过null终止符
            return result;
        }

        /// <summary>
        /// 计算数据包大小信息
        /// </summary>
        public static (int TotalSize, int OneSize, int TwoSize) GetPacketSizeInfo(OriginalData data)
        {
            int totalSize = 1; // cmd byte
            int oneSize = data.One?.Length ?? 0;
            int twoSize = data.Two?.Length ?? 0;

            totalSize += oneSize + twoSize;

            return (totalSize, oneSize, twoSize);
        }

        /// <summary>
        /// 验证数据包格式
        /// </summary>
        public static bool ValidatePacketFormat(OriginalData data)
        {
            // 基本验证：cmd不能为0，数据长度合理
            if (data.Cmd == 0)
                return false;

            // 检查数据长度（假设合理的数据包大小在1KB以内）
            int totalSize = 1 + (data.One?.Length ?? 0) + (data.Two?.Length ?? 0);
            if (totalSize > 1024)
                return false;

            return true;
        }
    }
}