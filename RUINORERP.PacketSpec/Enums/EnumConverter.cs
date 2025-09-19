using System;
using System.Collections.Generic;
using System.Linq;
using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Enums.Auth;
using RUINORERP.PacketSpec.Enums.Cache;
using RUINORERP.PacketSpec.Enums.Message;
using RUINORERP.PacketSpec.Enums.Workflow;
using RUINORERP.PacketSpec.Enums.Exception;
using RUINORERP.PacketSpec.Enums.File;
using RUINORERP.PacketSpec.Enums.DataSync;
using RUINORERP.PacketSpec.Enums.Lock;
using RUINORERP.PacketSpec.Enums.SystemManagement;

namespace RUINORERP.PacketSpec.Enums
{
    /// <summary>
    /// 枚举转换器
    /// 提供不同枚举类型之间的转换功能
    /// </summary>
    public static class EnumConverter
    {
        /// <summary>
        /// 命令值到枚举类型的映射
        /// </summary>
        private static readonly Dictionary<CommandHelper.CommandCategory, Type> CommandCategoryToTypeMap = new Dictionary<CommandHelper.CommandCategory, Type>
        {
            { CommandHelper.CommandCategory.System, typeof(SystemCommand) },
            { CommandHelper.CommandCategory.Authentication, typeof(AuthenticationCommand) },
            { CommandHelper.CommandCategory.Cache, typeof(CacheCommand) },
            { CommandHelper.CommandCategory.Message, typeof(MessageCommand) },
            { CommandHelper.CommandCategory.Workflow, typeof(WorkflowCommand) },
            { CommandHelper.CommandCategory.Exception, typeof(ExceptionCommand) },
            { CommandHelper.CommandCategory.File, typeof(FileCommand) },
            { CommandHelper.CommandCategory.DataSync, typeof(DataSyncCommand) },
            { CommandHelper.CommandCategory.Lock, typeof(LockCommand) },
            { CommandHelper.CommandCategory.SystemManagement, typeof(SystemManagementCommand) }
        };

        /// <summary>
        /// 将命令值转换为具体的枚举值
        /// </summary>
        /// <typeparam name="T">目标枚举类型</typeparam>
        /// <param name="commandValue">命令值</param>
        /// <returns>枚举值</returns>
        /// <exception cref="ArgumentException">当命令值不属于目标枚举类型时抛出</exception>
        public static T ConvertTo<T>(uint commandValue) where T : Enum
        {
            if (Enum.IsDefined(typeof(T), commandValue))
                return (T)Enum.ToObject(typeof(T), commandValue);
            
            throw new ArgumentException($"命令值 {commandValue} 不属于枚举类型 {typeof(T).Name}");
        }

        /// <summary>
        /// 安全转换命令值为枚举值
        /// </summary>
        /// <typeparam name="T">目标枚举类型</typeparam>
        /// <param name="commandValue">命令值</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>枚举值，如果转换失败返回默认值</returns>
        public static T SafeConvertTo<T>(uint commandValue, T defaultValue = default) where T : Enum
        {
            try
            {
                return ConvertTo<T>(commandValue);
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 根据命令值自动推断并返回对应的枚举值
        /// </summary>
        /// <param name="commandValue">命令值</param>
        /// <returns>枚举值对象</returns>
        /// <exception cref="ArgumentException">当命令值无效时抛出</exception>
        public static object AutoConvert(uint commandValue)
        {
            var category = CommandHelper.GetCategory(commandValue);
            
            if (CommandCategoryToTypeMap.TryGetValue(category, out var enumType))
            {
                if (Enum.IsDefined(enumType, commandValue))
                    return Enum.ToObject(enumType, commandValue);
            }
            
            throw new ArgumentException($"无效的命令值: {commandValue}");
        }

        /// <summary>
        /// 获取命令值对应的枚举类型
        /// </summary>
        /// <param name="commandValue">命令值</param>
        /// <returns>枚举类型，如果找不到返回null</returns>
        public static Type GetEnumType(uint commandValue)
        {
            var category = CommandHelper.GetCategory(commandValue);
            return CommandCategoryToTypeMap.TryGetValue(category, out var enumType) ? enumType : null;
        }

        /// <summary>
        /// 检查命令值是否属于指定的枚举类型
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="commandValue">命令值</param>
        /// <returns>是否属于该枚举类型</returns>
        public static bool IsTypeOf<T>(uint commandValue) where T : Enum
        {
            return Enum.IsDefined(typeof(T), commandValue);
        }

        /// <summary>
        /// 获取所有可能的枚举值
        /// </summary>
        /// <returns>包含所有命令枚举值的列表</returns>
        public static List<uint> GetAllCommandValues()
        {
            var values = new List<uint>();
            
            foreach (var enumType in CommandCategoryToTypeMap.Values)
            {
                values.AddRange(Enum.GetValues(enumType).Cast<uint>());
            }
            
            return values;
        }

        /// <summary>
        /// 批量转换命令值为指定枚举类型
        /// </summary>
        /// <typeparam name="T">目标枚举类型</typeparam>
        /// <param name="commandValues">命令值列表</param>
        /// <returns>转换后的枚举值列表</returns>
        public static List<T> BatchConvertTo<T>(IEnumerable<uint> commandValues) where T : Enum
        {
            return commandValues
                .Where(value => IsTypeOf<T>(value))
                .Select(value => ConvertTo<T>(value))
                .ToList();
        }

        /// <summary>
        /// 获取命令值的分类信息
        /// </summary>
        /// <param name="commandValue">命令值</param>
        /// <returns>分类信息字符串</returns>
        public static string GetCategoryInfo(uint commandValue)
        {
            var category = CommandHelper.GetCategory(commandValue);
            var enumType = GetEnumType(commandValue);
            var enumName = enumType != null ? Enum.GetName(enumType, commandValue) : "未知";
            
            return $"命令值: {commandValue:X}, 分类: {category}, 枚举名称: {enumName}";
        }
    }
}