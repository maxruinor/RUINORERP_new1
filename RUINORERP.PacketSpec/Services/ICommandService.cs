using RUINORERP.PacketSpec.Enums;
using RUINORERP.PacketSpec.Protocol;

namespace RUINORERP.PacketSpec.Services
{
    /// <summary>
    /// 客户端指令处理委托
    /// </summary>
    public delegate string ClientCommandHandler(ClientCommand command, OriginalData data);

    /// <summary>
    /// 服务器指令处理委托
    /// </summary>
    public delegate string ServerCommandHandler(ServerCommand command, OriginalData data);

    /// <summary>
    /// 指令服务接口
    /// </summary>
    public interface ICommandService
    {
        /// <summary>
        /// 处理客户端指令
        /// </summary>
        string ProcessClientCommand(ClientCommand command, OriginalData data);

        /// <summary>
        /// 处理服务器指令
        /// </summary>
        string ProcessServerCommand(ServerCommand command, OriginalData data);

        /// <summary>
        /// 注册客户端指令处理器
        /// </summary>
        void RegisterClientHandler(ClientCommand command, ClientCommandHandler handler);

        /// <summary>
        /// 注册服务器指令处理器
        /// </summary>
        void RegisterServerHandler(ServerCommand command, ServerCommandHandler handler);

        /// <summary>
        /// 获取默认的客户端指令处理器
        /// </summary>
        ClientCommandHandler GetDefaultClientHandler(ClientCommand command);

        /// <summary>
        /// 获取默认的服务器指令处理器
        /// </summary>
        ServerCommandHandler GetDefaultServerHandler(ServerCommand command);
    }
}