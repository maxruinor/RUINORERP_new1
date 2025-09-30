using System;
using System.Threading.Tasks;
using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Commands.Authentication;
using RUINORERP.PacketSpec.Commands.Lock;
using RUINORERP.PacketSpec.Models.Responses;

namespace RUINORERP.PacketSpec.Commands
{
    /// <summary>
    /// 命令构建器使用示例
    /// </summary>
    public static class CommandBuilderExamples
    {
        /// <summary>
        /// 演示基本命令构建
        /// </summary>
        public static async Task DemonstrateBasicUsage()
        {
            Console.WriteLine("=== 基本命令构建示例 ===");

            // 使用构建器创建登录命令
            var loginCommand = CommandBuilder<LoginCommand>.Create()
                .WithDirection(PacketDirection.ClientToServer)
                .WithPriority(CommandPriority.High)
                .WithTimeout(30000)
                .Build();

            Console.WriteLine($"登录命令创建完成: {loginCommand.GetType().Name}");
            Console.WriteLine($"命令ID: {loginCommand.CommandId}");
            Console.WriteLine($"方向: {loginCommand.Direction}");
            Console.WriteLine($"优先级: {loginCommand.Priority}");
            Console.WriteLine($"超时时间: {loginCommand.TimeoutMs}ms");

            // 使用构建器创建文档锁定命令
            var lockCommand = CommandBuilder<DocumentLockApplyCommand>.Create()
                .WithHighPriority()
                .FromClientToServer()
                .WithTimeout(15000)
                .Build();

            Console.WriteLine($"\n文档锁定命令创建完成: {lockCommand.GetType().Name}");
            Console.WriteLine($"命令ID: {lockCommand.CommandId}");
            Console.WriteLine($"方向: {lockCommand.Direction}");
            Console.WriteLine($"优先级: {lockCommand.Priority}");
            Console.WriteLine($"超时时间: {lockCommand.TimeoutMs}ms");
        }

        /// <summary>
        /// 演示从数据包构建命令
        /// </summary>
        public static async Task DemonstrateFromPacket()
        {
            Console.WriteLine("\n=== 从数据包构建命令示例 ===");

            // 先创建一个数据包
            var packet = Models.Core.PacketBuilder.Create()
                .WithCommand(AuthenticationCommands.LoginRequest)
                .WithDirection(PacketDirection.ClientToServer)
                .WithSession("session_789", "client_012")
                .WithRequestId("req_345")
                .WithTimeout(30000)
                .WithJsonData(new { Username = "testuser", Password = "testpass" })
                .Build();

            // 从数据包构建命令
            var loginCommand = CommandBuilder<LoginCommand>.Create()
                .FromPacket(packet)
                .Build();

            Console.WriteLine($"从数据包构建的命令: {loginCommand.GetType().Name}");
            Console.WriteLine($"命令ID: {loginCommand.CommandId}");
            Console.WriteLine($"超时时间: {loginCommand.TimeoutMs}ms");
        }

        /// <summary>
        /// 演示构建并执行命令
        /// </summary>
        public static async Task DemonstrateBuildAndExecute()
        {
            Console.WriteLine("\n=== 构建并执行命令示例 ===");

            // 构建并执行登录命令
            var loginResult = await CommandBuilder<LoginCommand>.Create()
                .WithHighPriority()
                .FromClientToServer()
                .WithTimeout(30000)
                .BuildAndExecuteAsync();

            Console.WriteLine($"登录命令执行结果: {loginResult.IsSuccess}");
            Console.WriteLine($"响应消息: {loginResult.Message}");
            Console.WriteLine($"响应代码: {loginResult.Code}");

            // 构建并执行文档解锁命令
            var unlockResult = await CommandBuilder<DocumentUnlockCommand>.Create()
                .WithNormalPriority()
                .FromClientToServer()
                .WithTimeout(15000)
                .BuildAndExecuteAsync();

            Console.WriteLine($"\n文档解锁命令执行结果: {unlockResult.IsSuccess}");
            Console.WriteLine($"响应消息: {unlockResult.Message}");
            Console.WriteLine($"响应代码: {unlockResult.Code}");
        }

        /// <summary>
        /// 运行所有示例
        /// </summary>
        public static async Task RunAllExamples()
        {
            try
            {
                await DemonstrateBasicUsage();
                await DemonstrateFromPacket();
                await DemonstrateBuildAndExecute();

                Console.WriteLine("\n=== 所有命令构建器示例执行完成 ===");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"示例执行错误: {ex.Message}");
            }
        }
    }
}
