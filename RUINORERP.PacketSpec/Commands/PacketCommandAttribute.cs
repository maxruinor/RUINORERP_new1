using System;
using RUINORERP.PacketSpec.Commands;  // 添加对命令系统的引用以使用CommandCategory

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 命令特性，用于标记命令类
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class PacketCommandAttribute : Attribute
    {
        /// <summary>
        /// 命令名称
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 命令类别
        /// </summary>
        public CommandCategory Category { get; }

        /// <summary>
        /// 命令描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">命令名称</param>
        /// <param name="category">命令类别</param>
        public PacketCommandAttribute(string name, CommandCategory category)
        {
            Name = name;
            Category = category;
        }
    }

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
        public uint[] SupportedCommands { get; }

        /// <summary>
        /// 是否为默认处理器
        /// </summary>
        public bool IsDefault { get; }

        public CommandHandlerAttribute(string name = null, int priority = 0, bool isDefault = false, params uint[] supportedCommands)
        {
            Name = name;
            Priority = priority;
            IsDefault = isDefault;
            SupportedCommands = supportedCommands ?? Array.Empty<uint>();
        }
    }
}
