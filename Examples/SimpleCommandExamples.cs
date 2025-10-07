using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Core;
using System;
using System.Threading.Tasks;

namespace Examples
{
    /// <summary>
    /// 简单命令使用示例 - 演示如何使用SimpleRequest/SimpleResponse避免创建单独请求实体类
    /// </summary>
    public class SimpleCommandExamples
    {

        /// <summary>
        /// 示例1: 创建和使用简单字符串请求 - 传统方式 vs 新方式
        /// 传统方式需要创建：StringRequest类、StringResponse类、StringCommand类
        /// 新方式：直接使用SimpleRequest/SimpleResponse
        /// </summary>
        public void Example1_StringRequest()
        {
            Console.WriteLine("=== 示例1: 字符串请求 ===");

            // 传统方式（需要创建多个类）
            // var request = new StringRequest { Value = "Hello ERP" };
            // var response = await someService.Process(request);

            // 新方式（一行代码创建请求）
            var request = SimpleRequest.CreateString("Hello ERP", "GREETING");
            Console.WriteLine($"创建字符串请求: Value={request.GetStringValue()}, Type={request.DataType}");

            // 模拟响应处理
            var response = SimpleResponse.CreateSuccessString("处理成功", "Hello ERP Response");
            if (response != null && response.IsSuccess)
            {
                Console.WriteLine($"成功响应: {response.GetStringValue()}");
            }
            else
            {
                Console.WriteLine($"失败: {response?.Message ?? "无响应"}");
            }
        }

        /// <summary>
        /// 示例2: 创建和使用简单布尔值请求 - 开关状态切换
        /// </summary>
        public void Example2_BoolRequest()
        {
            Console.WriteLine("=== 示例2: 布尔值请求 ===");

            // 创建布尔值请求，比如开关某个功能
            var request = SimpleRequest.CreateBool(true, "FEATURE_TOGGLE");
            Console.WriteLine($"创建布尔值请求: Value={request.GetBoolValue()}, Type={request.DataType}");

            // 模拟响应处理
            var response = SimpleResponse.CreateSuccessBool("功能开关成功", true);
            if (response != null && response.IsSuccess)
            {
                Console.WriteLine($"成功响应: 功能开关状态={response.GetBoolValue()}");
            }
            else
            {
                Console.WriteLine($"失败: {response?.Message ?? "无响应"}");
            }
        }

        /// <summary>
        /// 示例3: 发送简单整数值指令
        /// </summary>
        public async Task<int> Example3_SendIntCommand()
        {
            // 定义一个简单的命令ID
            var commandId = new CommandId("GET_USER_COUNT", "获取用户数量");

            // 发送整数值指令，不需要创建单独的请求类
            try
            {
                int result = await _simpleCommandService.SendIntCommandAsync(
                    commandId,
                    1,  // 部门ID
                    "DepartmentUserCount"  // 操作类型
                );

                Console.WriteLine($"部门用户数量: {result}");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"获取用户数量失败: {ex.Message}");
                return 0;
            }
        }

        /// <summary>
        /// 示例4: 发送通用简单指令
        /// </summary>
        public async Task<T> Example4_SendGenericCommand<T>(T value)
        {
            // 定义一个简单的命令ID
            var commandId = new CommandId("SIMPLE_DATA_PROCESS", "简单数据处理");

            // 发送通用简单指令，不需要创建单独的请求类
            try
            {
                T result = await _simpleCommandService.SendSimpleCommandAsync<T>(
                    commandId,
                    value,  // 任意类型的值
                    "DataProcess"  // 操作类型
                );

                Console.WriteLine($"数据处理结果: {result}");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"数据处理失败: {ex.Message}");
                return default(T);
            }
        }

        /// <summary>
        /// 示例5: 使用GenericCommand直接发送简单请求
        /// </summary>
        public async Task<SimpleResponse> Example5_DirectGenericCommand()
        {
            // 定义一个简单的命令ID
            var commandId = new CommandId("QUICK_STATUS_UPDATE", "快速状态更新");

            try
            {
                // 创建简单请求
                var request = SimpleRequest.CreateString("online", "StatusUpdate");
                request.RequestId = IdGenerator.GenerateRequestId(commandId);

                // 使用GenericCommand直接发送
                var command = new GenericCommand<SimpleRequest, SimpleResponse>(commandId, request);

                // 这里假设有一个通信服务实例
                // var response = await communicationService.SendCommandAsync<SimpleRequest, SimpleResponse>(command);

                Console.WriteLine($"状态更新请求已发送: {request.Data}");
                
                // 模拟返回响应
                return SimpleResponse.CreateSuccessString("updated", "状态更新成功");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"状态更新失败: {ex.Message}");
                return SimpleResponse.CreateFailure($"状态更新失败: {ex.Message}", 400);
            }
        }

        /// <summary>
        /// 示例6: 批量处理简单指令
        /// </summary>
        public async Task Example6_BatchSimpleCommands()
        {
            // 批量发送多个简单指令
            var tasks = new[]
            {
                Example1_SendStringCommand(),
                Example2_SendBoolCommand(),
                Example3_SendIntCommand(),
                Example4_SendGenericCommand("test data"),
                Example4_SendGenericCommand(123),
                Example4_SendGenericCommand(true)
            };

            try
            {
                var results = await Task.WhenAll(tasks);
                Console.WriteLine("批量简单指令执行完成");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"批量执行失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 示例7: 传统方式对比 - 需要创建单独请求类
        /// </summary>
        /// <remarks>
        /// 传统方式需要为每个简单请求创建单独的类：
        /// 
        /// public class UserStatusRequest : RequestBase
        /// {
        ///     public string UserId { get; set; }
        /// }
        /// 
        /// public class UserStatusResponse : ResponseBase  
        /// {
        ///     public string Status { get; set; }
        /// }
        /// 
        /// 使用方式：
        /// var request = new UserStatusRequest { UserId = "user123" };
        /// var command = new GenericCommand&lt;UserStatusRequest, UserStatusResponse&gt;(commandId, request);
        /// </remarks>
        public void Example7_CompareWithTraditionalWay()
        {
            Console.WriteLine("=== 传统方式 vs 简单方式对比 ===");
            Console.WriteLine("传统方式需要为每个简单请求创建单独的请求/响应类");
            Console.WriteLine("简单方式使用SimpleRequest/SimpleResponse，无需创建新类");
            Console.WriteLine("对于简单类型的数据传输，简单方式更加灵活高效");
        }
    }
}