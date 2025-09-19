using System;
using System.Collections.Generic;
using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Enums.Auth;
using RUINORERP.PacketSpec.Enums.Cache;
using RUINORERP.PacketSpec.Enums.Message;

namespace RUINORERP.PacketSpec.Enums
{
    /// <summary>
    /// 枚举使用示例
    /// 展示如何在接口中方便地使用模块化枚举
    /// </summary>
    public static class UsageExamples
    {
        /// <summary>
        /// 示例1：基本枚举使用
        /// </summary>
        public static void BasicUsage()
        {
            // 直接使用特定模块的枚举
            var loginCommand = AuthenticationCommand.Login;
            var cacheCommand = CacheCommand.CacheUpdate;
            var systemCommand = SystemCommand.Heartbeat;

            Console.WriteLine($"登录命令: {loginCommand} (0x{(uint)loginCommand:X})");
            Console.WriteLine($"缓存命令: {cacheCommand.GetDescription()}");
            Console.WriteLine($"系统命令: {systemCommand}");
        }

        /// <summary>
        /// 示例2：使用CommandHelper进行命令分类
        /// </summary>
        public static void CommandHelperUsage()
        {
            uint commandValue = 0x0100; // PrepareLogin
            
            var category = CommandHelper.GetCategory(commandValue);
            var isClientCommand = CommandHelper.IsClientCommand(commandValue);
            var description = CommandHelper.GetDescription(commandValue);

            Console.WriteLine($"命令值: 0x{commandValue:X}");
            Console.WriteLine($"分类: {category}");
            Console.WriteLine($"是否为客户端命令: {isClientCommand}");
            Console.WriteLine($"描述: {description}");
        }

        /// <summary>
        /// 示例3：使用EnumHelper获取描述信息
        /// </summary>
        public static void EnumHelperUsage()
        {
            // 获取单个枚举的描述
            var loginDesc = AuthenticationCommand.Login.GetDescription();
            Console.WriteLine($"Login命令描述: {loginDesc}");

            // 获取所有命令的描述映射
            var allCommands = EnumHelper.GetAllCommandDescriptions();
            Console.WriteLine($"总命令数量: {allCommands.Count}");

            // 检查命令值是否有效
            bool isValid = EnumHelper.IsValidCommand(0x0100);
            Console.WriteLine($"命令0x0100是否有效: {isValid}");
        }

        /// <summary>
        /// 示例4：使用EnumConverter进行类型转换
        /// </summary>
        public static void EnumConverterUsage()
        {
            uint commandValue = 0x0100; // PrepareLogin
            
            try
            {
                // 转换为具体的枚举类型
                var authCommand = EnumConverter.ConvertTo<AuthenticationCommand>(commandValue);
                Console.WriteLine($"转换成功: {authCommand}");

                // 自动推断枚举类型
                var autoCommand = EnumConverter.AutoConvert(commandValue);
                Console.WriteLine($"自动推断: {autoCommand} ({autoCommand.GetType().Name})");

                // 安全转换（带默认值）
                var safeCommand = EnumConverter.SafeConvertTo<CacheCommand>(commandValue, CacheCommand.CacheRequest);
                Console.WriteLine($"安全转换: {safeCommand}");

            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"转换失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 示例5：在接口方法中使用
        /// </summary>
        /// <param name="commandValue">命令值</param>
        public static void ProcessCommand(uint commandValue)
        {
            // 1. 验证命令有效性
            if (!EnumHelper.IsValidCommand(commandValue))
            {
                Console.WriteLine($"无效的命令值: 0x{commandValue:X}");
                return;
            }

            // 2. 获取命令描述
            var description = EnumHelper.GetCommandDescription(commandValue);
            Console.WriteLine($"处理命令: {description} (0x{commandValue:X})");

            // 3. 根据分类进行不同处理
            var category = CommandHelper.GetCategory(commandValue);
            
            switch (category)
            {
                case CommandHelper.CommandCategory.Authentication:
                    ProcessAuthCommand(commandValue);
                    break;
                case CommandHelper.CommandCategory.Cache:
                    ProcessCacheCommand(commandValue);
                    break;
                case CommandHelper.CommandCategory.Message:
                    ProcessMessageCommand(commandValue);
                    break;
                default:
                    Console.WriteLine($"处理其他类型命令: {category}");
                    break;
            }
        }

        private static void ProcessAuthCommand(uint commandValue)
        {
            var authCommand = EnumConverter.ConvertTo<AuthenticationCommand>(commandValue);
            Console.WriteLine($"处理认证命令: {authCommand.GetDescription()}");
            
            // 具体的认证命令处理逻辑...
        }

        private static void ProcessCacheCommand(uint commandValue)
        {
            var cacheCommand = EnumConverter.ConvertTo<CacheCommand>(commandValue);
            Console.WriteLine($"处理缓存命令: {cacheCommand.GetDescription()}");
            
            // 具体的缓存命令处理逻辑...
        }

        private static void ProcessMessageCommand(uint commandValue)
        {
            var messageCommand = EnumConverter.ConvertTo<MessageCommand>(commandValue);
            Console.WriteLine($"处理消息命令: {messageCommand.GetDescription()}");
            
            // 具体的消息命令处理逻辑...
        }

        /// <summary>
        /// 示例6：批量处理命令
        /// </summary>
        /// <param name="commandValues">命令值列表</param>
        public static void BatchProcessCommands(IEnumerable<uint> commandValues)
        {
            foreach (var commandValue in commandValues)
            {
                if (EnumConverter.IsTypeOf<AuthenticationCommand>(commandValue))
                {
                    var authCommand = EnumConverter.ConvertTo<AuthenticationCommand>(commandValue);
                    Console.WriteLine($"批量处理认证命令: {authCommand}");
                }
                else if (EnumConverter.IsTypeOf<CacheCommand>(commandValue))
                {
                    var cacheCommand = EnumConverter.ConvertTo<CacheCommand>(commandValue);
                    Console.WriteLine($"批量处理缓存命令: {cacheCommand}");
                }
                // 其他类型命令处理...
            }
        }

        /// <summary>
        /// 示例7：获取命令的完整信息
        /// </summary>
        public static void GetCommandFullInfo()
        {
            uint commandValue = 0x0200; // CacheRequest
            
            var category = CommandHelper.GetCategory(commandValue);
            var description = CommandHelper.GetDescription(commandValue);
            var isClientCommand = CommandHelper.IsClientCommand(commandValue);
            var enumType = EnumConverter.GetEnumType(commandValue);
            var enumName = enumType != null ? Enum.GetName(enumType, commandValue) : "未知";

            Console.WriteLine($"命令完整信息:");
            Console.WriteLine($"  值: 0x{commandValue:X}");
            Console.WriteLine($"  分类: {category}");
            Console.WriteLine($"  枚举类型: {enumType?.Name}");
            Console.WriteLine($"  枚举名称: {enumName}");
            Console.WriteLine($"  描述: {description}");
            Console.WriteLine($"  客户端命令: {isClientCommand}");
        }
    }
}