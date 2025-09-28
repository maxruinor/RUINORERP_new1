using System;
using System.ComponentModel;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 命令ID结构体，提供类型安全的命令标识
    /// </summary>
    public readonly struct CommandId : IEquatable<CommandId>
    {
        /// <summary>
        /// 命令类别
        /// </summary>
        public CommandCategory Category { get; }

        /// <summary>
        /// 操作码
        /// </summary>
        public byte OperationCode { get; }

        /// <summary>
        /// 完整的命令码
        /// </summary>
        public ushort FullCode => (ushort)(((byte)Category << 8) | OperationCode);

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="category">命令类别</param>
        /// <param name="operationCode">操作码</param>
        public CommandId(CommandCategory category, byte operationCode)
        {
            Category = category;
            OperationCode = operationCode;
        }

        /// <summary>
        /// 从ushort创建CommandId
        /// </summary>
        /// <param name="value">ushort值</param>
        /// <returns>CommandId实例</returns>
        public static CommandId FromUInt16(ushort value)
        {
            byte category = (byte)(value >> 8);
            byte operationCode = (byte)(value & 0xFF);
            return new CommandId((CommandCategory)category, operationCode);
        }

        /// <summary>
        /// 隐式转换为ushort
        /// </summary>
        /// <param name="id">CommandId实例</param>
        public static implicit operator ushort(CommandId id) => id.FullCode;

        /// <summary>
        /// 隐式转换为uint
        /// </summary>
        /// <param name="id">CommandId实例</param>
        public static implicit operator uint(CommandId id) => id.FullCode;

        /// <summary>
        /// 重写ToString方法
        /// </summary>
        /// <returns>字符串表示</returns>
        public override string ToString() => $"0x{FullCode:X4}";

        /// <summary>
        /// 重写Equals方法
        /// </summary>
        /// <param name="other">比较对象</param>
        /// <returns>是否相等</returns>
        public bool Equals(CommandId other) => FullCode == other.FullCode;
 
        /// <summary>
        /// 重写Equals方法
        /// </summary>
        /// <param name="obj">比较对象</param>
        /// <returns>是否相等</returns>
        public override bool Equals(object obj) => obj is CommandId other && Equals(other);

        /// <summary>
        /// 重写GetHashCode方法
        /// </summary>
        /// <returns>哈希码</returns>
        public override int GetHashCode() => FullCode.GetHashCode();
    }



    /// <summary>
    /// 命令类别枚举
    /// </summary>
    public enum CommandCategory : byte
    {
        /// <summary>
        /// 系统命令
        /// </summary>
        [Description("系统命令")]
        System = 0x00,

        /// <summary>
        /// 认证命令
        /// </summary>
        [Description("认证命令")]
        Authentication = 0x01,

        /// <summary>
        /// 缓存命令
        /// </summary>
        [Description("缓存命令")]
        Cache = 0x02,

        /// <summary>
        /// 消息命令
        /// </summary>
        [Description("消息命令")]
        Message = 0x03,

        /// <summary>
        /// 工作流命令
        /// </summary>
        [Description("工作流命令")]
        Workflow = 0x04,

        /// <summary>
        /// 异常处理命令
        /// </summary>
        [Description("异常处理命令")]
        Exception = 0x05,

        /// <summary>
        /// 文件操作命令
        /// </summary>
        [Description("文件操作命令")]
        File = 0x06,

        /// <summary>
        /// 数据同步命令
        /// </summary>
        [Description("数据同步命令")]
        DataSync = 0x07,

        /// <summary>
        /// 锁管理命令
        /// </summary>
        [Description("锁管理命令")]
        Lock = 0x08,

        /// <summary>
        /// 系统管理命令
        /// </summary>
        [Description("系统管理命令")]
        SystemManagement = 0x09,

        /// <summary>
        /// 复合型命令
        /// </summary>
        [Description("复合型命令")]
        Composite = 0x10,

        /// <summary>
        /// 连接管理命令
        /// </summary>
        [Description("连接管理命令")]
        Connection = 0x11,

        /// <summary>
        /// 特殊功能命令
        /// </summary>
        [Description("特殊功能命令")]
        Special = 0x90
    }
}