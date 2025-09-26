using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.UI.Network.RetryStrategy;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RUINORERP.UI.Network.Simplified
{
    /// <summary>
    /// 增强功能使用示例
    /// 展示如何使用所有增强的通信功能
    /// </summary>
    public class EnhancedUsageExample
    {
        /// <summary>
        /// 演示增强功能的使用
        /// </summary>
        public async Task DemonstrateEnhancedFeatures()
        {
            // 注意：这只是一个示例，实际使用时需要传入真实的通信服务
            // IClientCommunicationService communicationService = new YourCommunicationService();
            // var apiClient = new EnhancedFriendlyApiClient(communicationService);

            Console.WriteLine("=== 增强通信功能使用示例 ===");

            // 1. 使用默认重试策略的登录
            await DemonstrateLoginWithDefaultRetry();

            // 2. 使用自定义重试策略的登录
            await DemonstrateLoginWithCustomRetry();

            // 3. 发送心跳包
            await DemonstrateHeartbeat();

            // 4. 通用请求发送
            await DemonstrateGenericRequest();

            // 5. 批量请求发送
            await DemonstrateBatchRequest();

            Console.WriteLine("=== 示例结束 ===");
        }

        /// <summary>
        /// 演示使用默认重试策略的登录
        /// </summary>
        private async Task DemonstrateLoginWithDefaultRetry()
        {
            Console.WriteLine("1. 使用默认重试策略的登录示例");

            try
            {
                // var response = await apiClient.LoginAsync("testuser", "testpassword");
                // Console.WriteLine($"登录成功: 会话ID = {response.SessionId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"登录失败: {ex.Message}");
            }

            Console.WriteLine();
        }

        /// <summary>
        /// 演示使用自定义重试策略的登录
        /// </summary>
        private async Task DemonstrateLoginWithCustomRetry()
        {
            Console.WriteLine("2. 使用自定义重试策略的登录示例");

            // 创建自定义重试策略
            var customRetryPolicy = RetryPolicy.CreateExponentialBackoffRetry(
                maxRetryAttempts: 5,
                initialDelay: TimeSpan.FromSeconds(2),
                maxDelay: TimeSpan.FromSeconds(60));

            try
            {
                // var response = await apiClient.LoginAsync("testuser", "testpassword", customRetryPolicy);
                // Console.WriteLine($"登录成功: 会话ID = {response.SessionId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"登录失败: {ex.Message}");
            }

            Console.WriteLine();
        }

        /// <summary>
        /// 演示发送心跳包
        /// </summary>
        private async Task DemonstrateHeartbeat()
        {
            Console.WriteLine("3. 发送心跳包示例");

            try
            {
                // var success = await apiClient.SendHeartbeatAsync("session123");
                // Console.WriteLine($"心跳包发送{(success ? "成功" : "失败")}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"心跳包发送失败: {ex.Message}");
            }

            Console.WriteLine();
        }

        /// <summary>
        /// 演示通用请求发送
        /// </summary>
        private async Task DemonstrateGenericRequest()
        {
            Console.WriteLine("4. 通用请求发送示例");

            // 创建自定义重试策略
            var customRetryPolicy = RetryPolicy.CreateLinearRetry(
                maxRetryAttempts: 3,
                delay: TimeSpan.FromSeconds(1));

            try
            {
                // 创建示例请求
                var request = new SampleRequest
                {
                    Data = "示例数据",
                    Timestamp = DateTime.UtcNow
                };

                // var response = await apiClient.SendRequestAsync<SampleRequest, SampleResponse>(
                //     new CommandId(1001), // 示例命令ID
                //     request,
                //     customRetryPolicy);
                // 
                // Console.WriteLine($"请求成功: 响应数据 = {response.Result}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"请求失败: {ex.Message}");
            }

            Console.WriteLine();
        }

        /// <summary>
        /// 演示批量请求发送
        /// </summary>
        private async Task DemonstrateBatchRequest()
        {
            Console.WriteLine("5. 批量请求发送示例");

            try
            {
                // 创建示例请求数组
                var requests = new SampleRequest[]
                {
                    new SampleRequest { Data = "数据1", Timestamp = DateTime.UtcNow },
                    new SampleRequest { Data = "数据2", Timestamp = DateTime.UtcNow },
                    new SampleRequest { Data = "数据3", Timestamp = DateTime.UtcNow }
                };

                // var responses = await apiClient.SendBatchRequestAsync<SampleRequest, SampleResponse>(
                //     new CommandId(1002), // 示例命令ID
                //     requests);
                // 
                // Console.WriteLine($"批量请求成功: 处理了 {responses.Length} 个响应");
                // foreach (var response in responses)
                // {
                //     Console.WriteLine($"  响应: {response.Result}");
                // }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"批量请求失败: {ex.Message}");
            }

            Console.WriteLine();
        }
    }

    /// <summary>
    /// 示例请求类
    /// </summary>
    public class SampleRequest
    {
        public string Data { get; set; }
        public DateTime Timestamp { get; set; }
    }

    /// <summary>
    /// 示例响应类
    /// </summary>
    public class SampleResponse
    {
        public string Result { get; set; }
        public DateTime ProcessTime { get; set; }
    }
}