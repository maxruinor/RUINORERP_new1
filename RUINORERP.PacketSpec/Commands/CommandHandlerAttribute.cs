using System;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 命令处理器特性，用于标记命令处理器类
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class CommandHandlerAttribute : Attribute
    {
        /// <summary>
        /// 处理器名称
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 处理器优先级（数值越大优先级越高）
        /// </summary>
        public int Priority { get; }

        /// <summary>
        /// 支持的命令类型
        /// </summary>
        public CommandId[] SupportedCommands { get; }

        /// <summary>
        /// 是否为默认处理器
        /// </summary>
        public bool IsDefault { get; }

        public CommandHandlerAttribute(string name = null, int priority = 1, bool isDefault = false)
        {
            Priority = priority;
            Name = name;
            IsDefault = isDefault;
           
        }
    }
}
