using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Responses;

namespace RUINORERP.PacketSpec.Commands.Examples
{
    /// <summary>
    /// CommandId使用示例 - 展示新的CommandId类型的完整用法
    /// </summary>
    public class CommandIdUsageExamples
    {
        /// <summary>
        /// 示例1: 创建CommandId实例
        /// </summary>
        public static void Example1_CreateCommandId()
        {
            // 方法1: 使用构造函数
            var cmd1 = new CommandId(CommandCategory.System, 0x01, "系统初始化");
            
            // 方法2: 从ushort转换
            var cmd2 = CommandId.FromUInt16(0x0001); // 系统类别，操作码0x01
            
            // 方法3: 使用隐式转换
            CommandId cmd3 = (ushort)0x0101; // 认证类别，操作码0x01
            
            Console.WriteLine($"命令1: {cmd1} (名称: {cmd1.Name})");
            Console.WriteLine($"命令2: {cmd2} (名称: {cmd2.Name})");
            Console.WriteLine($"命令3: {cmd3} (名称: {cmd3.Name})");
        }

        /// <summary>
        /// 示例2: 在命令处理器中使用CommandId
        /// </summary>
        public class SystemCommandHandler : BaseCommandHandler
        {
            public SystemCommandHandler()
            {
                Name = "系统命令处理器";
                
                // 方法1: 使用CommandId数组
                SetSupportedCommands(
                    new CommandId(CommandCategory.System, 0x01, "系统初始化"),
                    new CommandId(CommandCategory.System, 0x02, "系统配置"),
                    new CommandId(CommandCategory.System, 0x03, "系统状态")
                );
                
                // 方法2: 使用uint数组（向后兼容）
                // SetSupportedCommands(0x0001, 0x0002, 0x0003);
                
                // 方法3: 使用IEnumerable<CommandId>
                // var commands = new List<CommandId>
                // {
                //     new CommandId(CommandCategory.System, 0x01),
                //     new CommandId(CommandCategory.System, 0x02),
                //     new CommandId(CommandCategory.System, 0x03)
                // };
                // SetSupportedCommands(commands);
            }

            protected override async Task<BaseCommand<IResponse>> OnHandleAsync(QueuedCommand cmd, CancellationToken cancellationToken)
            {
                // 处理逻辑
                return new BaseCommand<IResponse>().CreateSuccess("处理成功");
            }
        }

        /// <summary>
        /// 示例3: 使用CanHandle方法
        /// </summary>
        public static void Example3_CanHandleUsage()
        {
            var handler = new SystemCommandHandler();
            
            // 创建测试命令
            var testCommand = new QueuedCommand
            {
                Command = new BaseCommand<IResponse>
                {
                    CommandIdentifier = new CommandId(CommandCategory.System, 0x01)
                }
            };
            
            // 方法1: 使用QueuedCommand参数
            bool canHandle1 = handler.CanHandle(testCommand);
            
            // 方法2: 使用uint参数（向后兼容）
            bool canHandle2 = handler.CanHandle(0x0001u);
            
            Console.WriteLine($"可以处理方法1: {canHandle1}");
            Console.WriteLine($"可以处理方法2: {canHandle2}");
        }

        /// <summary>
        /// 示例4: 命令ID比较和转换
        /// </summary>
        public static void Example4_ComparisonAndConversion()
        {
            var cmd1 = new CommandId(CommandCategory.System, 0x01);
            var cmd2 = new CommandId(CommandCategory.System, 0x01);
            var cmd3 = new CommandId(CommandCategory.Authentication, 0x01);
            
            // 相等性比较
            Console.WriteLine($"cmd1 == cmd2: {cmd1.Equals(cmd2)}");
            Console.WriteLine($"cmd1 == cmd3: {cmd1.Equals(cmd3)}");
            
            // 隐式转换
            ushort ushortValue = cmd1;  // 转换为ushort
            uint uintValue = cmd1;      // 转换为uint
            
            Console.WriteLine($"ushort转换: 0x{ushortValue:X4}");
            Console.WriteLine($"uint转换: 0x{uintValue:X8}");
            
            // 从数值转换回CommandId
            var convertedBack = CommandId.FromUInt16(ushortValue);
            Console.WriteLine($"转换回来: {convertedBack} (相等: {cmd1.Equals(convertedBack)})");
        }

        /// <summary>
        /// 示例5: 复杂命令处理器
        /// </summary>
        public class MultiCategoryHandler : BaseCommandHandler
        {
            public MultiCategoryHandler()
            {
                Name = "多类别命令处理器";
                
                // 支持多个类别的命令
                var supportedCommands = new List<CommandId>
                {
                    new CommandId(CommandCategory.System, 0x01, "系统初始化"),
                    new CommandId(CommandCategory.System, 0x02, "系统配置"),
                    new CommandId(CommandCategory.Authentication, 0x01, "用户登录"),
                    new CommandId(CommandCategory.Authentication, 0x02, "用户注销"),
                    new CommandId(CommandCategory.Cache, 0x01, "缓存清理"),
                    new CommandId(CommandCategory.Cache, 0x02, "缓存预热")
                };
                
                SetSupportedCommands(supportedCommands);
            }

            protected override async Task<BaseCommand<IResponse>> OnHandleAsync(QueuedCommand cmd, CancellationToken cancellationToken)
            {
                var commandId = cmd.Command.CommandIdentifier;
                
                // 根据命令类别和操作码进行不同处理
                switch (commandId.Category)
                {
                    case CommandCategory.System:
                        return await HandleSystemCommand(commandId.OperationCode, cancellationToken);
                        
                    case CommandCategory.Authentication:
                        return await HandleAuthCommand(commandId.OperationCode, cancellationToken);
                        
                    case CommandCategory.Cache:
                        return await HandleCacheCommand(commandId.OperationCode, cancellationToken);
                        
                    default:
                        return new BaseCommand<IResponse>().CreateError($"不支持的命令类别: {commandId.Category}");
                }
            }

            private Task<BaseCommand<IResponse>> HandleSystemCommand(byte operationCode, CancellationToken cancellationToken)
            {
                // 系统命令处理逻辑
                return Task.FromResult(new BaseCommand<IResponse>().CreateSuccess($"系统命令处理完成: 0x{operationCode:X2}"));
            }

            private Task<BaseCommand<IResponse>> HandleAuthCommand(byte operationCode, CancellationToken cancellationToken)
            {
                // 认证命令处理逻辑
                return Task.FromResult(new BaseCommand<IResponse>().CreateSuccess($"认证命令处理完成: 0x{operationCode:X2}"));
            }

            private Task<BaseCommand<IResponse>> HandleCacheCommand(byte operationCode, CancellationToken cancellationToken)
            {
                // 缓存命令处理逻辑
                return Task.FromResult(new BaseCommand<IResponse>().CreateSuccess($"缓存命令处理完成: 0x{operationCode:X2}"));
            }
        }

        /// <summary>
        /// 运行所有示例
        /// </summary>
        public static void RunAllExamples()
        {
            Console.WriteLine("=== CommandId 使用示例 ===\n");
            
            Console.WriteLine("示例1: 创建CommandId实例");
            Example1_CreateCommandId();
            Console.WriteLine();
            
            Console.WriteLine("示例3: 使用CanHandle方法");
            Example3_CanHandleUsage();
            Console.WriteLine();
            
            Console.WriteLine("示例4: 命令ID比较和转换");
            Example4_ComparisonAndConversion();
            Console.WriteLine();
            
            Console.WriteLine("所有示例运行完成！");
        }
    }

    /// <summary>
    /// 扩展示例：自定义命令处理器
    /// </summary>
    public class CustomCommandHandler : BaseCommandHandler
    {
        private readonly Dictionary<CommandId, Func<CancellationToken, Task<IResponse>>> _handlers;

        public CustomCommandHandler()
        {
            Name = "自定义命令处理器";
            _handlers = new Dictionary<CommandId, Func<CancellationToken, Task<IResponse>>>();
            
            // 注册命令处理函数
            RegisterHandler(new CommandId(CommandCategory.System, 0x01, "心跳"), HandleHeartbeat);
            RegisterHandler(new CommandId(CommandCategory.System, 0x02, "状态检查"), HandleStatusCheck);
            RegisterHandler(new CommandId(CommandCategory.Authentication, 0x01, "登录"), HandleLogin);
        }

        private void RegisterHandler(CommandId commandId, Func<CancellationToken, Task<IResponse>> handler)
        {
            _handlers[commandId] = handler;
            
            // 更新支持的命令列表
            var supportedCommands = new List<CommandId>(SupportedCommands ?? Array.Empty<CommandId>())
            {
                commandId
            };
            SetSupportedCommands(supportedCommands);
        }

        protected override async Task<BaseCommand<IResponse>> OnHandleAsync(QueuedCommand cmd, CancellationToken cancellationToken)
        {
            var commandId = cmd.Command.CommandIdentifier;
            
            if (_handlers.TryGetValue(commandId, out var handler))
            {
                var response = await handler(cancellationToken);
                return new BaseCommand<IResponse>().WithResponse(response);
            }
            
            return new BaseCommand<IResponse>().CreateError($"未找到命令处理函数: {commandId}");
        }

        private Task<IResponse> HandleHeartbeat(CancellationToken cancellationToken)
        {
            return Task.FromResult<IResponse>(new SimpleResponse { Success = true, Message = "心跳正常" });
        }

        private Task<IResponse> HandleStatusCheck(CancellationToken cancellationToken)
        {
            return Task.FromResult<IResponse>(new SimpleResponse { Success = true, Message = "状态正常" });
        }

        private Task<IResponse> HandleLogin(CancellationToken cancellationToken)
        {
            return Task.FromResult<IResponse>(new SimpleResponse { Success = true, Message = "登录成功" });
        }
    }

    /// <summary>
    /// 简单的响应实现
    /// </summary>
    public class SimpleResponse : IResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string ErrorCode { get; set; }
        public object Data { get; set; }
    }
}