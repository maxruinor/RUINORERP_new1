using RUINORERP.PacketSpec.Commands;
using System;

namespace RUINORERP.UI.Network.Commands
{
    /// <summary>
    /// 命令特性
    /// 用于标记命令类的元数据
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class CommandAttribute : Attribute
    {
        /// <summary>
        /// 命令码
        /// </summary>
        public ushort CommandCode { get; }

        /// <summary>
        /// 命令类别
        /// </summary>
        public CommandCategory Category { get; }

        /// <summary>
        /// 命令描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 是否需要响应
        /// </summary>
        public bool RequireResponse { get; set; } = true;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="commandCode">命令码</param>
        public CommandAttribute(ushort commandCode)
        {
            CommandCode = commandCode;
            // 从命令码中提取类别
            Category = (CommandCategory)((commandCode >> 8) & 0xFF);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="category">命令类别</param>
        /// <param name="operationCode">操作码</param>
        public CommandAttribute(CommandCategory category, byte operationCode)
        {
            Category = category;
            CommandCode = (ushort)(((byte)category << 8) | operationCode);
        }
    }
}