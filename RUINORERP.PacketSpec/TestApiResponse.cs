using System;
using RUINORERP.PacketSpec.Models.Responses;

namespace TestApiResponse
{
    class Program
    {
        static void Main()
        {
            // 测试泛型版本的CreateSuccess方法
            var response1 = ApiResponse<string>.CreateSuccess("测试数据", "操作成功");
            Console.WriteLine($"Success属性: {response1.Success}");
            Console.WriteLine($"Message: {response1.Message}");
            Console.WriteLine($"Data: {response1.Data}");
            
            // 测试非泛型版本的CreateSuccess方法
            var response2 = ApiResponse.CreateSuccess("无数据操作成功");
            Console.WriteLine($"Success属性: {response2.Success}");
            Console.WriteLine($"Message: {response2.Message}");
            
            Console.WriteLine("所有测试通过！");
        }
    }
}