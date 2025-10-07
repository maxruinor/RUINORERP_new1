using System;
using System.Threading.Tasks;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;

namespace Examples
{
    /// <summary>
    /// 简单请求/响应测试类
    /// </summary>
    public class SimpleRequestResponseTest
    {
        /// <summary>
        /// 测试简单请求和响应的创建与使用
        /// </summary>
        public static void TestSimpleRequestResponse()
        {
            Console.WriteLine("=== 测试简单请求/响应 ===");

            // 测试字符串请求
            var stringRequest = SimpleRequest.CreateString("Hello World", "GREETING");
            Console.WriteLine($"字符串请求: Value={stringRequest.GetStringValue()}, Type={stringRequest.DataType}");

            // 测试布尔值请求
            var boolRequest = SimpleRequest.CreateBool(true, "STATUS_CHECK");
            Console.WriteLine($"布尔值请求: Value={boolRequest.GetBoolValue()}, Type={boolRequest.DataType}");

            // 测试整数值请求
            var intRequest = SimpleRequest.CreateInt(42, "NUMBER_PROCESS");
            Console.WriteLine($"整数值请求: Value={intRequest.GetIntValue()}, Type={intRequest.DataType}");

            // 测试成功响应
            var successResponse = SimpleResponse.CreateSuccessString("操作成功", "数据处理完成");
            Console.WriteLine($"成功响应: Success={successResponse.IsSuccess}, Message={successResponse.Message}, Data={successResponse.GetStringValue()}");

            // 测试失败响应
            var failureResponse = SimpleResponse.CreateFailure("操作失败", 404);
            Console.WriteLine($"失败响应: Success={failureResponse.IsSuccess}, Message={failureResponse.Message}, ErrorCode={failureResponse.ErrorCode}");

            Console.WriteLine("=== 测试完成 ===");
        }

        /// <summary>
        /// 演示如何在实际场景中使用简单请求
        /// </summary>
        public static void DemonstrateUsage()
        {
            Console.WriteLine("\n=== 实际使用场景演示 ===");

            // 场景1: 简单的状态检查
            var statusCheckRequest = SimpleRequest.CreateBool(true, "HEARTBEAT");
            Console.WriteLine($"心跳检测请求 - 数据类型: {statusCheckRequest.DataType}, 值: {statusCheckRequest.GetBoolValue()}");

            // 场景2: 简单的ID查询
            var idQueryRequest = SimpleRequest.CreateString("USER_123", "USER_QUERY");
            Console.WriteLine($"用户查询请求 - 数据类型: {idQueryRequest.DataType}, 值: {idQueryRequest.GetStringValue()}");

            // 场景3: 简单的计数器更新
            var counterRequest = SimpleRequest.CreateInt(1, "INCREMENT");
            Console.WriteLine($"计数器请求 - 数据类型: {counterRequest.DataType}, 值: {counterRequest.GetIntValue()}");

            // 场景4: 简单的配置开关
            var configRequest = SimpleRequest.CreateBool(false, "FEATURE_TOGGLE");
            Console.WriteLine($"配置开关请求 - 数据类型: {configRequest.DataType}, 值: {configRequest.GetBoolValue()}");

            Console.WriteLine("\n=== 演示完成 ===");
        }

        /// <summary>
        /// 主测试入口点
        /// </summary>
        public static async Task Main(string[] args)
        {
            try
            {
                TestSimpleRequestResponse();
                DemonstrateUsage();
                
                Console.WriteLine("\n所有测试通过！简单请求/响应系统工作正常。");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"测试失败: {ex.Message}");
            }
        }
    }
}