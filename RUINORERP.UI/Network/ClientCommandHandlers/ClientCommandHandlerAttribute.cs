using System;

namespace RUINORERP.UI.Network.ClientCommandHandlers
{
    /// <summary>
    /// 客户端命令处理器特性
    /// 用于标记类为客户端命令处理器，并设置名称和优先级
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
        /// 构造函数
        /// </summary>
        /// <param name="name">处理器名称</param>
        /// <param name="priority">优先级，默认为50</param>
        public ClientCommandHandlerAttribute(string name, int priority = 50)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Priority = priority;
        }
    }
}