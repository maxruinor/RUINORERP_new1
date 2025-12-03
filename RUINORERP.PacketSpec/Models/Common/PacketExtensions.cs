using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RUINORERP.PacketSpec.Serialization;
using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Responses;
using Newtonsoft.Json.Linq;

namespace RUINORERP.PacketSpec.Models.Common
{
    /// <summary>
    /// PacketModel的扩展方法类
    /// </summary>
    public static class PacketExtensions
    {
        #region 数据操作扩展


 

        /// <summary>
        /// 计算数据包哈希值（用于快速比较）
        /// </summary>
        /// <param name="packet">数据包实例</param>
        /// <returns>哈希值</returns>
        public static string ComputeHash(this PacketModel packet)
        {
            if (packet.Request == null)
                return string.Empty;

            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var hash = sha256.ComputeHash(packet.ToBinary());
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
            packet.Extensions[key] = JToken.FromObject(value);
            return packet;
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
        /// 从二进制数据反序列化
        /// </summary>
        /// <param name="data">二进制数据</param>
        /// <returns>数据包实例</returns>
        public static PacketModel FromBinary(byte[] data)
        {
            var json = Encoding.UTF8.GetString(data);
            return FromJson(json);
        }


        #endregion
    }
}
