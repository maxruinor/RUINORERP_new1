using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace RUINORERP.PacketSpec.Enums
{
    /// <summary>
    /// 枚举辅助工具类
    /// 提供获取枚举描述信息的方法，支持模块化枚举架构
    /// </summary>
    public static class EnumHelper
    {
        /// <summary>
        /// 获取枚举值的中文描述
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="enumValue">枚举值</param>
        /// <returns>中文描述，如果没有Description特性则返回枚举名称</returns>
        public static string GetDescription<T>(this T enumValue) where T : Enum
        {
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
            if (fieldInfo == null)
                return enumValue.ToString();

            var attributes = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];
            
            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            
            return enumValue.ToString();
        }

        /// <summary>
        /// 根据描述获取枚举值
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="description">中文描述</param>
        /// <returns>枚举值，如果找不到则返回默认值</returns>
        public static T GetValueFromDescription<T>(string description) where T : Enum
        {
            var type = typeof(T);
            if (!type.IsEnum)
                throw new InvalidOperationException("T must be an enumerated type");

            foreach (var field in type.GetFields())
            {
                if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
            }

            return default;
        }

        /// <summary>
        /// 获取枚举的所有值及其描述
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <returns>包含枚举值和描述的字典</returns>
        public static Dictionary<T, string> GetAllDescriptions<T>() where T : Enum
        {
            var result = new Dictionary<T, string>();
            var type = typeof(T);
            
            foreach (var value in Enum.GetValues(type).Cast<T>())
            {
                result[value] = value.GetDescription();
            }
            
            return result;
        }

        /// <summary>
        /// 获取所有命令枚举类型
        /// </summary>
        /// <returns>命令枚举类型列表</returns>
        public static List<Type> GetAllCommandEnumTypes()
        {
            return new List<Type>
            {
                typeof(Core.SystemCommand),
                typeof(Auth.AuthenticationCommand),
                typeof(Cache.CacheCommand),
                typeof(Message.MessageCommand),
                typeof(Workflow.WorkflowCommand),
                typeof(Exception.ExceptionCommand),
                typeof(File.FileCommand),
                typeof(DataSync.DataSyncCommand),
                typeof(Lock.LockCommand),
                typeof(SystemManagement.SystemManagementCommand)
            };
        }

        /// <summary>
        /// 根据命令值获取对应的描述
        /// </summary>
        /// <param name="commandValue">命令值</param>
        /// <returns>命令描述，如果找不到返回空字符串</returns>
        public static string GetCommandDescription(uint commandValue)
        {
            foreach (var enumType in GetAllCommandEnumTypes())
            {
                if (Enum.IsDefined(enumType, commandValue))
                {
                    var enumValue = Enum.ToObject(enumType, commandValue);
                    var field = enumType.GetField(enumValue.ToString());
                    var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
                    return attribute?.Description ?? enumValue.ToString();
                }
            }
            
            return string.Empty;
        }

        /// <summary>
        /// 检查命令值是否有效
        /// </summary>
        /// <param name="commandValue">命令值</param>
        /// <returns>是否有效</returns>
        public static bool IsValidCommand(uint commandValue)
        {
            return GetAllCommandEnumTypes().Any(enumType => Enum.IsDefined(enumType, commandValue));
        }

        /// <summary>
        /// 获取所有命令值及其描述的映射
        /// </summary>
        /// <returns>命令值到描述的映射字典</returns>
        public static Dictionary<uint, string> GetAllCommandDescriptions()
        {
            var result = new Dictionary<uint, string>();
            
            foreach (var enumType in GetAllCommandEnumTypes())
            {
                foreach (var value in Enum.GetValues(enumType))
                {
                    var commandValue = Convert.ToUInt32(value);
                    var description = GetCommandDescription(commandValue);
                    result[commandValue] = description;
                }
            }
            
            return result;
        }
    }
}