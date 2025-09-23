using System;
using System.Text;
using Newtonsoft.Json;
using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.PacketSpec.Serialization;
using RUINORERP.PacketSpec.Commands;
using static RUINORERP.PacketSpec.Commands.FileTransfer.FileCommands;

namespace RUINORERP.PacketSpec.Models.Core
{
    /// <summary>
    /// 数据包扩展方法
    /// </summary>
    public static class PacketExtensions
    {
        #region 数据包创建扩展
        
        /// <summary>
        /// 设置数据包命令类型
        /// </summary>
        /// <param name="packet">数据包实例</param>
        /// <param name="command">命令类型</param>
        /// <returns>当前数据包实例</returns>
        public static PacketModel WithCommand(this PacketModel packet, CommandId command)
        {
            packet.Command = command;
            return packet;
        }

        /// <summary>
        /// 设置数据包优先级
        /// </summary>
        /// <param name="packet">数据包实例</param>
        /// <param name="priority">优先级</param>
        /// <returns>当前数据包实例</returns>
        public static PacketModel WithPriority(this PacketModel packet, PacketPriority priority)
        {
            packet.Priority = priority;
            return packet;
        }

        /// <summary>
        /// 设置数据包方向
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
        /// 设置数据包状态
        /// </summary>
        /// <param name="packet">数据包实例</param>
        /// <param name="status">状态</param>
        /// <returns>当前数据包实例</returns>
        public static PacketModel WithStatus(this PacketModel packet, PacketStatus status)
        {
            packet.Status = status;
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

        #region 数据操作扩展
        
        /// <summary>
        /// 设置字符串数据
        /// </summary>
        /// <param name="packet">数据包实例</param>
        /// <param name="text">文本数据</param>
        /// <param name="encoding">编码格式</param>
        /// <returns>当前数据包实例</returns>
        public static PacketModel WithTextData(this PacketModel packet, string text, Encoding encoding = null)
        {
            encoding ??= Encoding.UTF8;
            packet.SetData(encoding.GetBytes(text));
            return packet;
        }

        /// <summary>
        /// 设置JSON数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="packet">数据包实例</param>
        /// <param name="data">数据对象</param>
        /// <returns>当前数据包实例</returns>
        public static PacketModel WithJsonData<T>(this PacketModel packet, T data)
        {
            packet.SetJsonData(data);
            return packet;
        }

        /// <summary>
        /// 获取字符串数据
        /// </summary>
        /// <param name="packet">数据包实例</param>
        /// <param name="encoding">编码格式</param>
        /// <returns>文本数据</returns>
        public static string GetTextData(this PacketModel packet, Encoding encoding = null)
        {
            return packet.GetDataAsText(encoding);
        }

        /// <summary>
        /// 获取JSON数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="packet">数据包实例</param>
        /// <returns>数据对象</returns>
        public static T GetJsonData<T>(this PacketModel packet)
        {
            return packet.GetJsonData<T>();
        }

        /// <summary>
        /// 转换为API响应
        /// </summary>
        /// <typeparam name="T">响应数据类型</typeparam>
        /// <param name="packet">数据包实例</param>
        /// <returns>API响应实例</returns>
        public static ApiResponse<T> ToApiResponse<T>(this PacketModel packet)
        {
            try
            {
                var data = packet.GetJsonData<T>();
                return ApiResponse<T>.CreateSuccess(data, "数据包处理成功");
            }
            catch (Exception ex)
            {
                return ApiResponse<T>.Failure($"数据包解析失败: {ex.Message}");
            }
        }

        #endregion

        #region 验证和状态检查
        
        /// <summary>
        /// 检查数据包是否有效
        /// </summary>
        /// <param name="packet">数据包实例</param>
        /// <returns>是否有效</returns>
        public static bool IsValidPacket(this PacketModel packet)
        {
            return packet != null && packet.IsValid();
        }

        /// <summary>
        /// 检查数据包是否包含特定命令
        /// </summary>
        /// <param name="packet">数据包实例</param>
        /// <param name="command">命令类型</param>
        /// <returns>是否包含</returns>
        public static bool HasCommand(this PacketModel packet, CommandId command)
        {
            return packet?.Command == command;
        }

        /// <summary>
        /// 检查数据包是否处于特定状态
        /// </summary>
        /// <param name="packet">数据包实例</param>
        /// <param name="status">状态</param>
        /// <returns>是否处于该状态</returns>
        public static bool HasStatus(this PacketModel packet, PacketStatus status)
        {
            return packet?.Status == status;
        }

        /// <summary>
        /// 检查数据包是否已完成
        /// </summary>
        /// <param name="packet">数据包实例</param>
        /// <returns>是否完成</returns>
        public static bool IsCompleted(this PacketModel packet)
        {
            return packet?.Status == PacketStatus.Completed;
        }

        /// <summary>
        /// 检查数据包是否失败
        /// </summary>
        /// <param name="packet">数据包实例</param>
        /// <returns>是否失败</returns>
        public static bool IsFailed(this PacketModel packet)
        {
            return packet?.Status == PacketStatus.Failed;
        }

        #endregion

        #region 性能优化扩展
        
        /// <summary>
        /// 快速设置数据（避免多次复制）
        /// </summary>
        /// <param name="packet">数据包实例</param>
        /// <param name="data">数据字节数组</param>
        /// <returns>当前数据包实例</returns>
        public static PacketModel WithDataDirect(this PacketModel packet, byte[] data)
        {
            packet.Body = data;
            packet.Size = data?.Length ?? 0;
            packet.LastUpdatedTime = DateTime.UtcNow;
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
            if (packet.Body == null || offset < 0 || count <= 0 || offset + count > packet.Body.Length)
                return Array.Empty<byte>();

            var slice = new byte[count];
            Buffer.BlockCopy(packet.Body, offset, slice, 0, count);
            return slice;
        }

        /// <summary>
        /// 计算数据包哈希值（用于快速比较）
        /// </summary>
        /// <param name="packet">数据包实例</param>
        /// <returns>哈希值</returns>
        public static string ComputeHash(this PacketModel packet)
        {
            if (packet.Body == null || packet.Body.Length == 0)
                return string.Empty;

            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var hash = sha256.ComputeHash(packet.Body);
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
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
