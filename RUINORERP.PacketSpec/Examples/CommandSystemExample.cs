using System;
using System.Threading.Tasks;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Commands.Handlers;


namespace RUINORERP.PacketSpec.Examples
{
    /// <summary>
    /// 命令系统使用示例
    /// 演示如何创建、注册和使用命令调度器来处理命令
    /// </summary>
    public class CommandSystemExample
    {
        private readonly ICommandDispatcher _commandDispatcher;

        /// <summary>
        /// 构造函数
        /// </summary>
        public CommandSystemExample()
        {
            // 创建命令调度器实例
            _commandDispatcher = null;  //new ServerCommandDispatcher();ID?
        }

        /// <summary>
        /// 初始化命令系统示例
        /// </summary>
        /// <returns>初始化任务</returns>
        public async Task InitializeAsync()
        {
            // 初始化命令调度器
            await _commandDispatcher.InitializeAsync();

            // 注册命令类型
            _commandDispatcher.RegisterCommandType(new CommandId(CommandCategory.Special, 0x01), typeof(EchoCommand));

            // 创建并注册命令处理器
           // var echoHandler = new(null); //DI;
                                         // 注意：实际应用中可能需要通过依赖注入或其他方式注册处理器
                                         // 这里为了简化示例，直接使用CommandDispatcher的注册方法

            Console.WriteLine("命令系统示例已初始化完成");
        }

        /// <summary>
        /// 运行回显命令示例
        /// </summary>
        /// <param name="message">要回显的消息</param>
        /// <returns>命令执行结果</returns>
        public async Task RunEchoCommandExample(string message)
        {
            try
            {
                Console.WriteLine($"\n执行回显命令，消息: '{message}'");

                // 创建命令实例
                var echoCommand = new EchoCommand
                {
                    Message = message
                };

                // 验证命令
                var validationResult = echoCommand.Validate();
                if (!validationResult.IsValid)
                {
                    Console.WriteLine($"命令验证失败: {validationResult.ErrorMessage} ({validationResult.ErrorCode})");
                    return;
                }

                // 使用命令调度器分发命令
                var result = await _commandDispatcher.DispatchAsync(echoCommand);

                // 处理命令结果
                if (result.IsSuccess)
                {
                    Console.WriteLine("命令执行成功!");
                    Console.WriteLine($"结果消息: {result.Message}");
                    Console.WriteLine($"响应数据: {result.Data}");
                    Console.WriteLine($"执行时间: {result.ExecutionTimeMs}ms");
                }
                else
                {
                    Console.WriteLine($"命令执行失败: {result.Message} ({result.ErrorCode})");
                    if (result.Exception != null)
                    {
                        Console.WriteLine($"异常信息: {result.Exception.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"运行示例时发生异常: {ex.Message}");
            }
        }

        /// <summary>
        /// 运行完整示例
        /// </summary>
        /// <returns>任务</returns>
        public static async Task RunExample()
        {
            var example = new CommandSystemExample();
            await example.InitializeAsync();

            // 测试成功的命令
            await example.RunEchoCommandExample("Hello, Command System!");

            // 测试失败的命令（空消息）
            await example.RunEchoCommandExample("");
        }
    }
}
