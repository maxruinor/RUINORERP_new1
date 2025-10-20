using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using MessagePack;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 命令ID结构体，提供类型安全的命令标识
    /// </summary>
    [MessagePackObject]
    public struct CommandId : IEquatable<CommandId>
    {
        /// <summary>
        /// 空命令ID（用于表示无效或未设置的命令）
        /// </summary>
        public static readonly CommandId Empty = new CommandId(CommandCategory.System, 0x00, "Empty");

        /// <summary>
        /// 命令类别
        /// </summary>
        [Key(0)]
        public CommandCategory Category { get; set; }


        // 添加命令名称属性
        [Key(1)]
        public string Name { get; set; }


        /// <summary>
        /// 操作码
        /// </summary>
        [Key(2)]
        public byte OperationCode { get; set; }

        /// <summary>
        /// 完整的命令码
        /// </summary>
        [Key(3)]
        public ushort FullCode => (ushort)(((byte)Category << 8) | OperationCode);

        /// <summary>
        /// MessagePack序列化所需的默认构造函数
        /// </summary>
        /// <remarks>
        /// MessagePack反序列化需要无参构造函数，所有属性必须有setter
        /// </remarks>
        [SerializationConstructor]
        public CommandId()
        {
            Category = CommandCategory.System;
            OperationCode = 0;
            Name = string.Empty;
        }

        /// <summary>
        /// 构造函数 - 允许显式指定命令名称
        /// </summary>
        /// <param name="category">命令类别</param>
        /// <param name="operationCode">操作码</param>
        /// <param name="name">命令名称</param>
        public CommandId(CommandCategory category, byte operationCode, string name = null)
        {
            Category = category;
            OperationCode = operationCode;
            Name = name ?? GetDefaultName(category, operationCode);
        }

        /// <summary>
        /// 从特性或类型信息获取默认名称的辅助方法
        /// </summary>
        /// <param name="category">命令类别</param>
        /// <param name="operationCode">操作码</param>
        /// <returns>命令名称，如果无法获取则返回null</returns>
        private static string GetDefaultName(CommandCategory category, byte operationCode)
        {
            try
            {
                // 构建完整的命令码
                ushort fullCode = (ushort)(((byte)category << 8) | operationCode);

                // 获取CommandCatalog类的所有公共常量字段
                var fields = typeof(CommandCatalog).GetFields(BindingFlags.Public |
                                                         BindingFlags.Static |
                                                         BindingFlags.FlattenHierarchy);

                // 查找与完整命令码匹配的常量
                foreach (var field in fields)
                {
                    if (field.FieldType == typeof(ushort) &&
                        (ushort)field.GetValue(null) == fullCode)
                    {
                        // 从常量名称中提取命令名称（去掉类别前缀）
                        string fieldName = field.Name;
                        int underscoreIndex = fieldName.IndexOf('_');
                        if (underscoreIndex > 0 && underscoreIndex < fieldName.Length - 1)
                        {
                            return fieldName.Substring(underscoreIndex + 1);
                        }
                        return fieldName;
                    }
                }
            }
            catch (Exception)
            {
                // 如果发生异常，返回null
            }

            // 如果没有找到匹配的命令，返回null
            return null;
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
        /// 尝试从字符串解析CommandId
        /// </summary>
        /// <param name="value">字符串值</param>
        /// <param name="result">解析结果</param>
        /// <returns>是否解析成功</returns>
        public static bool TryParse(string value, out CommandId result)
        {
            result = default;
            if (string.IsNullOrEmpty(value))
                return false;

            try
            {
                // 尝试解析十六进制字符串
                if (value.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                {
                    if (ushort.TryParse(value.Substring(2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out ushort ushortValue))
                    {
                        result = FromUInt16(ushortValue);
                        return true;
                    }
                }
                // 尝试解析十进制字符串
                else if (ushort.TryParse(value, out ushort ushortValue))
                {
                    result = FromUInt16(ushortValue);
                    return true;
                }
            }
            catch
            {
                // 忽略解析异常
            }

            return false;
        }

        /// <summary>
        /// 重写ToString方法
        /// </summary>
        /// <returns>字符串表示</returns>
        public override string ToString() => $"0x{FullCode:X4}:{Name}";

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
