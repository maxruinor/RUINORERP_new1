using RUINORERP.PacketSpec.Models.Core;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network.ClientCommandHandlers
{
    /// <summary>
    /// 客户端命令调度器接口
    /// 负责管理所有的客户端命令处理器，并根据命令ID将命令分发到对应的处理器
    /// </summary>
    public interface IClientCommandDispatcher
    {
        /// <summary>
        /// 已注册的处理器列表
        /// </summary>
        IReadOnlyList<IClientCommandHandler> Handlers { get; }

        /// <summary>
        /// 调度器状态
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// 初始化调度器
        /// </summary>
        /// <returns>初始化是否成功</returns>
        Task<bool> InitializeAsync();

        /// <summary>
        /// 启动调度器
        /// </summary>
        /// <returns>启动是否成功</returns>
        Task<bool> StartAsync();

        /// <summary>
        /// 停止调度器
        /// </summary>
        /// <returns>停止是否成功</returns>
        Task<bool> StopAsync();

        /// <summary>
        /// 注册单个命令处理器
        /// </summary>
        /// <param name="handler">命令处理器</param>
        /// <returns>注册是否成功</returns>
        Task<bool> RegisterHandlerAsync(IClientCommandHandler handler);

        /// <summary>
        /// 批量注册命令处理器
        /// </summary>
        /// <param name="handlers">命令处理器集合</param>
        /// <returns>注册是否成功</returns>
        Task<bool> RegisterHandlersAsync(IEnumerable<IClientCommandHandler> handlers);

        /// <summary>
        /// 取消注册命令处理器
        /// </summary>
        /// <param name="handlerId">处理器ID</param>
        /// <returns>取消注册是否成功</returns>
        Task<bool> UnregisterHandlerAsync(string handlerId);

        /// <summary>
        /// 根据命令ID查找合适的处理器
        /// </summary>
        /// <param name="packet">数据包</param>
        /// <returns>找到的处理器，未找到返回null</returns>
        IClientCommandHandler FindHandler(PacketModel packet);

        /// <summary>
        /// 分发命令到合适的处理器
        /// </summary>
        /// <param name="packet">数据包</param>
        /// <returns>处理是否成功</returns>
        Task<bool> DispatchAsync(PacketModel packet);

        /// <summary>
        /// 扫描并注册程序集中的命令处理器
        /// </summary>
        /// <returns>扫描并注册的处理器数量</returns>
        Task<int> ScanAndRegisterHandlersAsync();
        
        /// <summary>
        /// 扫描并注册指定程序集中的命令处理器
        /// </summary>
        /// <param name="assemblies">要扫描的程序集列表</param>
        /// <returns>扫描并注册的处理器数量</returns>
        Task<int> ScanAndRegisterHandlersAsync(IEnumerable<Assembly> assemblies);
        
        /// <summary>
        /// 初始化并启动调度系统，包括自动扫描和注册处理器
        /// 此方法提供完整的一键式初始化流程
        /// </summary>
        /// <param name="assemblies">可选，指定要扫描的程序集，不指定则扫描当前程序集</param>
        /// <returns>初始化是否成功以及注册的处理器数量</returns>
        Task<(bool success, int registeredCount)> InitializeAndStartAsync(IEnumerable<Assembly> assemblies = null);
    }
}