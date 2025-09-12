using System;
using System.Collections.Generic;
using RUINORERP.PacketSpec.Enums;
using RUINORERP.PacketSpec.Protocol;

namespace RUINORERP.PacketSpec.Services
{
    /// <summary>
    /// 基础指令服务实现
    /// </summary>
    public class CommandService : ICommandService
    {
        private readonly Dictionary<ClientCommand, ClientCommandHandler> _clientHandlers;
        private readonly Dictionary<ServerCommand, ServerCommandHandler> _serverHandlers;

        public CommandService()
        {
            _clientHandlers = new Dictionary<ClientCommand, ClientCommandHandler>();
            _serverHandlers = new Dictionary<ServerCommand, ServerCommandHandler>();
            
            InitializeDefaultHandlers();
        }

        /// <summary>
        /// 初始化默认的指令处理器
        /// </summary>
        private void InitializeDefaultHandlers()
        {
            // 注册所有客户端指令的默认处理器
            foreach (ClientCommand command in Enum.GetValues(typeof(ClientCommand)))
            {
                _clientHandlers[command] = DefaultClientHandler;
            }

            // 注册所有服务器指令的默认处理器
            foreach (ServerCommand command in Enum.GetValues(typeof(ServerCommand)))
            {
                _serverHandlers[command] = DefaultServerHandler;
            }
        }

        public string ProcessClientCommand(ClientCommand command, OriginalData data)
        {
            if (_clientHandlers.TryGetValue(command, out var handler))
            {
                return handler(command, data);
            }
            return $"No handler found for client command: {command}";
        }

        public string ProcessServerCommand(ServerCommand command, OriginalData data)
        {
            if (_serverHandlers.TryGetValue(command, out var handler))
            {
                return handler(command, data);
            }
            return $"No handler found for server command: {command}";
        }

        public void RegisterClientHandler(ClientCommand command, ClientCommandHandler handler)
        {
            _clientHandlers[command] = handler;
        }

        public void RegisterServerHandler(ServerCommand command, ServerCommandHandler handler)
        {
            _serverHandlers[command] = handler;
        }

        public ClientCommandHandler GetDefaultClientHandler(ClientCommand command)
        {
            return DefaultClientHandler;
        }

        public ServerCommandHandler GetDefaultServerHandler(ServerCommand command)
        {
            return DefaultServerHandler;
        }

        /// <summary>
        /// 默认的客户端指令处理器
        /// </summary>
        private string DefaultClientHandler(ClientCommand command, OriginalData data)
        {
            return $"Client command processed: {command} (0x{(uint)command:X})";
        }

        /// <summary>
        /// 默认的服务器指令处理器
        /// </summary>
        private string DefaultServerHandler(ServerCommand command, OriginalData data)
        {
            return $"Server command processed: {command} (0x{(uint)command:X})";
        }

        /// <summary>
        /// 处理心跳包
        /// </summary>
        private string HandleHeartbeat(ClientCommand command, OriginalData data)
        {
            return "Heartbeat received and processed";
        }

        /// <summary>
        /// 处理登录请求
        /// </summary>
        private string HandleLogin(ClientCommand command, OriginalData data)
        {
            return "Login request processed";
        }

        /// <summary>
        /// 处理缓存相关指令
        /// </summary>
        private string HandleCacheOperation(ClientCommand command, OriginalData data)
        {
            return $"Cache operation: {command} processed";
        }

        /// <summary>
        /// 处理工作流指令
        /// </summary>
        private string HandleWorkflow(ClientCommand command, OriginalData data)
        {
            return $"Workflow operation: {command} processed";
        }
    }
}