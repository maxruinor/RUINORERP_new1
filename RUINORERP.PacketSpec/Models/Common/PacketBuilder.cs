using System;
using System.Text;
using Newtonsoft.Json;
using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Commands.Authentication;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Core;

namespace RUINORERP.PacketSpec.Models.Common
{
    /// <summary>
    /// 数据包构建器 - 提供流畅的API构建数据包
    /// 使用建造者模式简化数据包创建过程
    /// </summary>
    public class PacketBuilder
    {
        private PacketModel _packet;

        /// <summary>
        /// 私有构造函数
        /// </summary>
        private PacketBuilder()
        {
            _packet = new PacketModel();
        }


        /// <summary>
        /// 创建新的构建器实例
        /// </summary>
        /// <returns>构建器实例</returns>
        public static PacketBuilder Create(PacketModel packet)
        {
            PacketBuilder packetBuilder = new PacketBuilder();
            packetBuilder._packet = packet;
            return packetBuilder;
        }


        /// <summary>
        /// 创建新的构建器实例
        /// </summary>
        /// <returns>构建器实例</returns>
        public static PacketBuilder Create()
        {
            return new PacketBuilder();
        }

        /// <summary>
        /// 设置命令类型
        /// </summary>
        /// <param name="command">命令类型</param>
        /// <returns>当前构建器实例</returns>
        public PacketBuilder WithCommand(CommandId command)
        {
            _packet.CommandId = command;
            return this;
        }


        /// <summary>
        /// 设置方向
        /// </summary>
        /// <param name="direction">方向</param>
        /// <returns>当前构建器实例</returns>
        public PacketBuilder WithDirection(PacketDirection direction)
        {
            _packet.Direction = direction;
            return this;
        }


        /// <summary>
        /// 设置会话信息
        /// </summary>
        /// <param name="sessionId">会话ID</param>
        /// <param name="clientId">客户端ID</param>
        /// <returns>当前构建器实例</returns>
        public PacketBuilder WithSession(string sessionId, string clientId = null)
        {
            if (!string.IsNullOrEmpty(sessionId))
            {
                _packet.SessionId = sessionId;
                _packet.Extensions["SessionId"] = sessionId;
            }
            return this;
        }


 



        public PacketBuilder WithRequest(IRequest request)
        {
            _packet.Request= request;
            return this;
        }

        /// <summary>
        /// 设置扩展属性
        /// </summary>
        /// <param name="key">属性键</param>
        /// <param name="value">属性值</param>
        /// <returns>当前构建器实例</returns>
        public PacketBuilder WithExtension(string key, object value)
        {
            _packet.Extensions[key] = value;
            return this;
        }

        /// <summary>
        /// 设置请求标识
        /// </summary>
        /// <param name="requestId">请求ID</param>
        /// <returns>当前构建器实例</returns>
        public PacketBuilder WithRequestId(string requestId)
        {
            return WithExtension("RequestId", requestId);
        }

        /// <summary>
        /// 设置响应超时时间
        /// </summary>
        /// <param name="timeoutMs">超时时间（毫秒）</param>
        /// <returns>当前构建器实例</returns>
        public PacketBuilder WithTimeout(int timeoutMs)
        {
            return WithExtension("Timeout", timeoutMs);
        }

        /// <summary>
        /// 设置是否需要响应
        /// </summary>
        /// <param name="requiresResponse">是否需要响应</param>
        /// <returns>当前构建器实例</returns>
        public PacketBuilder WithResponseRequired(bool requiresResponse = true)
        {
            return WithExtension("RequiresResponse", requiresResponse);
        }

     

        /// <summary>
        /// 构建最终的数据包
        /// </summary>
        /// <returns>构建完成的数据包实例</returns>
        public PacketModel Build()
        {
            // 验证数据包有效性
            if (!_packet.IsValid())
            {
                throw new InvalidOperationException("构建的数据包无效");
            }

            return _packet;
        }

        /// <summary>
        /// 构建并克隆数据包（避免后续修改影响已构建的包）
        /// </summary>
        /// <returns>克隆的数据包实例</returns>
        public PacketModel BuildAndClone()
        {
            return Build().Clone();
        }

        /// <summary>
        /// 构建为JSON字符串
        /// </summary>
        /// <param name="formatting">格式化选项</param>
        /// <returns>JSON字符串</returns>
        public string BuildToJson(Formatting formatting = Formatting.None)
        {
            return Build().ToJson(formatting);
        }

        /// <summary>
        /// 构建为二进制数据
        /// </summary>
        /// <returns>二进制数据</returns>
        public byte[] BuildToBinary()
        {
            return Build().ToBinary();
        }
    }


}
