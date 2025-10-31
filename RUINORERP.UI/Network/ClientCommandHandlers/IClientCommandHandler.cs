using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Models.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network.ClientCommandHandlers
{
    /// <summary>
    /// 客户端命令处理器接口
    /// 所有客户端命令处理器都必须实现此接口
    /// 负责处理服务器发送到客户端的特定命令
    /// </summary>
    public interface IClientCommandHandler
    {
        /// <summary>
        /// 处理器唯一标识
        /// </summary>
        string HandlerId { get; }

        /// <summary>
        /// 处理器名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 处理器优先级
        /// </summary>
        int Priority { get; set; }

        /// <summary>
        /// 处理器支持的命令ID列表
        /// </summary>
        IReadOnlyList<CommandId> SupportedCommands { get; }

        /// <summary>
        /// 处理器状态
        /// </summary>
        ClientHandlerStatus Status { get; }

        /// <summary>
        /// 初始化处理器
        /// </summary>
        /// <returns>初始化是否成功</returns>
        Task<bool> InitializeAsync();

        /// <summary>
        /// 启动处理器
        /// </summary>
        /// <returns>启动是否成功</returns>
        Task<bool> StartAsync();

        /// <summary>
        /// 停止处理器
        /// </summary>
        /// <returns>停止是否成功</returns>
        Task<bool> StopAsync();

        /// <summary>
        /// 检查是否能处理指定的命令
        /// </summary>
        /// <param name="packet">数据包</param>
        /// <returns>是否能处理</returns>
        bool CanHandle(PacketModel packet);

        /// <summary>
        /// 处理命令
        /// </summary>
        /// <param name="packet">数据包</param>
        /// <returns>处理结果</returns>
        Task HandleAsync(PacketModel packet);
    }

    /// <summary>
    /// 客户端处理器状态枚举
    /// </summary>
    public enum ClientHandlerStatus
    {
        /// <summary>
        /// 未初始化
        /// </summary>
        Uninitialized,

        /// <summary>
        /// 已初始化
        /// </summary>
        Initialized,

        /// <summary>
        /// 运行中
        /// </summary>
        Running,

        /// <summary>
        /// 已停止
        /// </summary>
        Stopped,

        /// <summary>
        /// 出错
        /// </summary>
        Error
    }
}