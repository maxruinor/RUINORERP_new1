using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Core;
using System;

namespace RUINORERP.UI.Network
{
    /// <summary>
    /// PacketBuilder适配器
    /// 适配PacketSpec中的PacketBuilder，提供更便捷的API
    /// </summary>
    public static class PacketBuilderAdapter
    {
        /// <summary>
        /// 创建一个新的数据包构建器
        /// </summary>
        /// <returns>PacketBuilder实例</returns>
        public static PacketBuilder Create()
        {
            return RUINORERP.PacketSpec.Models.Core.PacketBuilder.Create();
        }

        /// <summary>
        /// 创建一个登录请求数据包
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="clientVersion">客户端版本</param>
        /// <returns>构建好的数据包</returns>
        public static PacketModel CreateLoginRequest(string username, string password, string clientVersion = "1.0.0")
        {
            return Create()
                .WithCommand(RUINORERP.PacketSpec.Commands.AuthenticationCommands.LoginRequest)
                .WithPriority(CommandPriority.High)
                .WithJsonData(new { Username = username, Password = password, ClientVersion = clientVersion })
                .WithResponseRequired(true)
                .WithDirection(PacketDirection.ClientToServer)
                .Build();
        }

        /// <summary>
        /// 创建一个心跳请求数据包
        /// </summary>
        /// <param name="clientId">客户端ID</param>
        /// <returns>构建好的数据包</returns>
        public static PacketModel CreateHeartbeatRequest(string clientId)
        {
            return Create()
                .WithCommand(RUINORERP.PacketSpec.Commands.SystemCommands.Heartbeat)
                .WithPriority(CommandPriority.Low)
                .WithJsonData(new { ClientId = clientId, Timestamp = DateTime.Now.Ticks })
                .WithResponseRequired(false)
                .WithDirection(PacketDirection.ClientToServer)
                .Build();
        }

        /// <summary>
        /// 创建一个通用请求数据包
        /// </summary>
        /// <typeparam name="TData">请求数据类型</typeparam>
        /// <param name="commandId">命令ID</param>
        /// <param name="data">请求数据</param>
        /// <param name="priority">命令优先级</param>
        /// <param name="responseRequired">是否需要响应</param>
        /// <returns>构建好的数据包</returns>
        public static PacketModel CreateRequest<TData>(
            CommandId commandId, 
            TData data, 
            CommandPriority priority = CommandPriority.Normal,
            bool responseRequired = true)
        {
            return Create()
                .WithCommand(commandId)
                .WithPriority(priority)
                .WithJsonData(data)
                .WithResponseRequired(responseRequired)
                .WithDirection(PacketDirection.ClientToServer)
                .Build();
        }
    }
}