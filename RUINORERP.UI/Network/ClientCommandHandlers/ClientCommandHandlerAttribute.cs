using RUINORERP.PacketSpec.Commands;
using System;

namespace RUINORERP.UI.Network.ClientCommandHandlers
{
    /// <summary>
    /// 客户端命令处理器特性
    /// 用于标记类为客户端命令处理器，并设置名称、优先级和支持的命令
    /// 支持字符串类型的命令定义
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ClientCommandHandlerAttribute : Attribute
    {
        /// <summary>
        /// 处理器名称
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 处理器优先级，数值越大优先级越高
        /// </summary>
        public int Priority { get; }
        
        /// <summary>
        /// 处理器支持的命令ID列表（字符串形式）
        /// </summary>
        public string[] SupportedCommandIds { get; }

        /// <summary>
        /// 构造函数 - 支持字符串命令ID
        /// </summary>
        /// <param name="name">处理器名称</param>
        /// <param name="priority">优先级，默认为50</param>
        /// <param name="supportedCommandIds">支持的命令ID列表（字符串形式）</param>
        public ClientCommandHandlerAttribute(string name, int priority = 50, params string[] supportedCommandIds)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Priority = priority;
            SupportedCommandIds = supportedCommandIds ?? Array.Empty<string>();
        }
    }
}