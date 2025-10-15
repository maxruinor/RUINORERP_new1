using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Serialization;
using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Responses;

namespace RUINORERP.PacketSpec.Models.Core
{
    /// <summary>
    /// PacketModel的扩展方法类
    /// </summary>
    public static class PacketExtensions
    {
        #region 数据操作扩展

        /// <summary>
        /// 快速设置数据（避免多次复制）
        /// </summary>
        /// <param name="packet">数据包实例</param>
        /// <param name="data">数据字节数组</param>
        /// <returns>当前数据包实例</returns>
        public static PacketModel WithDataDirect(this PacketModel packet, byte[] data)
        {
            packet.CommandData = data;
            // Size现在是计算属性，不需要手动设置
            packet.UpdateTimestamp();
            return packet;
        }

        /// <summary>
        /// 设置文本数据
        /// </summary>
        /// <param name="packet">数据包实例</param>
        /// <param name="text">文本数据</param>
        /// <param name="encoding">编码格式</param>
        /// <returns>当前数据包实例</returns>
        public static PacketModel WithTextData(this PacketModel packet, string text, Encoding encoding = null)
        {
            encoding ??= Encoding.UTF8;
            packet.CommandData = encoding.GetBytes(text);
            packet.UpdateTimestamp();
            return packet;
        }

        /// <summary>
        /// 获取数据切片（避免复制整个数组）
        /// </summary>
        /// <param name="packet">数据包实例</param>
        /// <param name="offset">偏移量</param>
        /// <param name="count">数量</param>
        /// <returns>数据切片</returns>
        public static byte[] GetDataSlice(this PacketModel packet, int offset, int count)
        {
            if (packet.CommandData == null || offset < 0 || count <= 0 || offset + count > packet.CommandData.Length)
                return Array.Empty<byte>();

            var slice = new byte[count];
            Buffer.BlockCopy(packet.CommandData, offset, slice, 0, count);
            return slice;
        }

        /// <summary>
        /// 计算数据包哈希值（用于快速比较）
        /// </summary>
        /// <param name="packet">数据包实例</param>
        /// <returns>哈希值</returns>
        public static string ComputeHash(this PacketModel packet)
        {
            if (packet.CommandData == null || packet.CommandData.Length == 0)
                return string.Empty;

            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var hash = sha256.ComputeHash(packet.CommandData);
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }

        #endregion

        #region 命令操作扩展

        /// <summary>
        /// 设置命令类型
        /// </summary>
        /// <param name="packet">数据包实例</param>
        /// <param name="command">命令类型</param>
        /// <returns>当前数据包实例</returns>
        public static PacketModel WithCommand(this PacketModel packet, CommandId command)
        {
            packet.CommandId = command;
            return packet;
        }

        /// <summary>
        /// 设置方向
        /// </summary>
        /// <param name="packet">数据包实例</param>
        /// <param name="direction">方向</param>
        /// <returns>当前数据包实例</returns>
        public static PacketModel WithDirection(this PacketModel packet, PacketDirection direction)
        {
            packet.Direction = direction;
            return packet;
        }

        /// <summary>
        /// 设置扩展属性
        /// </summary>
        /// <param name="packet">数据包实例</param>
        /// <param name="key">属性键</param>
        /// <param name="value">属性值</param>
        /// <returns>当前数据包实例</returns>
        public static PacketModel WithExtension(this PacketModel packet, string key, object value)
        {
            packet.Extensions[key] = value;
            return packet;
        }

        #endregion

        #region 响应转换扩展

        /// <summary>
        /// 转换为API响应
        /// </summary>
        /// <typeparam name="T">响应数据类型</typeparam>
        /// <param name="packet">数据包实例</param>
        /// <returns>API响应</returns>
        public static ResponseBase<T> ToApiResponse<T>(this PacketModel packet)
        {
            try
            {
                var data = packet.GetJsonData<T>();
                return ResponseBase<T>.CreateSuccess(data, "成功");
            }
            catch (Exception ex)
            {
                return ResponseBase<T>.Failure($"转换失败: {ex.Message}");
            }
        }

        #endregion

        #region 序列化扩展
        
        /// <summary>
        /// 序列化为JSON字符串
        /// </summary>
        /// <param name="packet">数据包实例</param>
        /// <param name="formatting">格式化选项</param>
        /// <returns>JSON字符串</returns>
        public static string ToJson(this PacketModel packet, Formatting formatting = Formatting.None)
        {
            return JsonConvert.SerializeObject(packet, formatting);
        }

        /// <summary>
        /// 从JSON字符串反序列化
        /// </summary>
        /// <param name="json">JSON字符串</param>
        /// <returns>数据包实例</returns>
        public static PacketModel FromJson(string json)
        {
            return JsonConvert.DeserializeObject<PacketModel>(json);
        }

        /// <summary>
        /// 转换为二进制格式（用于网络传输）
        /// </summary>
        /// <param name="packet">数据包实例</param>
        /// <returns>二进制数据</returns>
        public static byte[] ToBinary(this PacketModel packet)
        {
            var json = packet.ToJson();
            return Encoding.UTF8.GetBytes(json);
        }

        /// <summary>
        /// 使用MessagePack转换为二进制格式（用于网络传输）
        /// </summary>
        /// <param name="packet">数据包实例</param>
        /// <returns>二进制数据</returns>
        public static byte[] ToBinaryWithMessagePack(this PacketModel packet)
        {
            return MessagePackService.Serialize(packet);
        }

        /// <summary>
        /// 从二进制数据反序列化
        /// </summary>
        /// <param name="data">二进制数据</param>
        /// <returns>数据包实例</returns>
        public static PacketModel FromBinary(byte[] data)
        {
            var json = Encoding.UTF8.GetString(data);
            return FromJson(json);
        }

        /// <summary>
        /// 使用MessagePack从二进制数据反序列化
        /// </summary>
        /// <param name="data">二进制数据</param>
        /// <returns>数据包实例</returns>
        public static PacketModel FromBinaryWithMessagePack(byte[] data)
        {
            return MessagePackService.Deserialize<PacketModel>(data);
        }

        #endregion
    }
}