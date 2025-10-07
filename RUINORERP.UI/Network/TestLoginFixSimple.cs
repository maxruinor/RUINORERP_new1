using System;
using System.Threading;
using System.Threading.Tasks;
using RUINORERP.PacketSpec.Commands.Authentication;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.PacketSpec.Models.Core;

namespace RUINORERP.UI.Network.Test
{
    /// <summary>
    /// 简单测试登录命令的Request属性访问
    /// </summary>
    public class TestLoginFixSimple
    {
        /// <summary>
        /// 测试LoginCommand的Request属性访问
        /// </summary>
        public static void TestRequestPropertyAccess()
        {
            Console.WriteLine("=== 测试LoginCommand的Request属性访问 ===");

            // 创建登录请求
            var loginRequest = LoginRequest.Create("testuser", "testpass");
            loginRequest.RequestId = 12345;

            // 创建登录命令
            var loginCommand = new LoginCommand("testuser", "testpass");
            loginCommand.Request = loginRequest;
            loginCommand.ExecutionContext = new CommandExecutionContext();
            loginCommand.ExecutionContext.RequestId = loginCommand.Request.RequestId;

            Console.WriteLine($"LoginCommand类型: {loginCommand.GetType().FullName}");
            Console.WriteLine($"LoginCommand.BaseType: {loginCommand.GetType().BaseType?.FullName}");
            
            // 测试直接访问Request属性（通过LoginCommand的LoginRequest属性）
            var directRequest = loginCommand.LoginRequest;
            Console.WriteLine($"通过LoginRequest属性访问: {(directRequest != null ? "成功" : "失败")}");
            
            if (directRequest != null)
            {
                Console.WriteLine($"Request.RequestId: {directRequest.RequestId}");
                Console.WriteLine($"Request类型: {directRequest.GetType().FullName}");
            }

            // 测试通过基类访问Request属性
            var baseCommand = (BaseCommand)loginCommand;
            var baseRequest = baseCommand.Request;
            Console.WriteLine($"通过基类BaseCommand访问Request: {(baseRequest != null ? "成功" : "失败")}");
            
            // 测试强制转换后访问
            if (baseCommand is LoginCommand castedCommand)
            {
                var castedRequest = castedCommand.Request;
                Console.WriteLine($"强制转换为LoginCommand后访问Request: {(castedRequest != null ? "成功" : "失败")}");
                if (castedRequest != null)
                {
                    Console.WriteLine($"强制转换后的Request.RequestId: {castedRequest.RequestId}");
                }
            }

            // 测试泛型基类访问
            if (baseCommand is BaseCommand<LoginRequest, LoginResponse> genericCommand)
            {
                var genericRequest = genericCommand.Request;
                Console.WriteLine($"转换为泛型BaseCommand<TRequest, TResponse>后访问Request: {(genericRequest != null ? "成功" : "失败")}");
                if (genericRequest != null)
                {
                    Console.WriteLine($"泛型基类的Request.RequestId: {genericRequest.RequestId}");
                }
            }

            Console.WriteLine("=== 测试完成 ===");
        }

        /// <summary>
        /// 模拟ClientCommunicationService中的问题场景
        /// </summary>
        public static void SimulateProblemScenario()
        {
            Console.WriteLine("\n=== 模拟问题场景 ===");

            // 创建登录请求
            var loginRequest = LoginRequest.Create("testuser", "testpass");
            loginRequest.RequestId = 12345;

            // 创建登录命令
            var loginCommand = new LoginCommand("testuser", "testpass");
            loginCommand.Request = loginRequest;
            loginCommand.ExecutionContext = new CommandExecutionContext();
            loginCommand.ExecutionContext.RequestId = loginCommand.Request.RequestId;

            // 模拟SendRequestAsync方法中的问题
            Console.WriteLine("模拟SendRequestAsync方法中的访问方式:");
            
            // 方法签名: SendRequestAsync(BaseCommand command, ...)
            BaseCommand baseCommand = loginCommand; // 这里发生隐式转换
            
            // 问题：直接访问baseCommand.Request
            var requestFromBase = baseCommand.Request;
            Console.WriteLine($"baseCommand.Request: {(requestFromBase != null ? "不为null" : "为null")}");
            
            if (requestFromBase != null)
            {
                Console.WriteLine($"baseCommand.Request.RequestId: {requestFromBase.RequestId}");
            }
            else
            {
                Console.WriteLine("这就是问题所在！baseCommand.Request为null");
            }

            // 解决方案：使用反射或类型检查
            Console.WriteLine("\n解决方案 - 使用反射获取泛型属性:");
            var requestData = GetCommandRequestData(loginCommand);
            Console.WriteLine($"通过反射获取Request: {(requestData != null ? "成功" : "失败")}");
            
            if (requestData is IRequest irequest)
            {
                Console.WriteLine($"反射获取的Request.RequestId: {irequest.RequestId}");
            }

            Console.WriteLine("=== 问题场景测试完成 ===");
        }

        /// <summary>
        /// 模拟ClientCommunicationService中的辅助方法
        /// </summary>
        private static object GetCommandRequestData(BaseCommand command)
        {
            if (command == null) return null;

            var commandType = command.GetType();
            
            // 检查是否是泛型BaseCommand<TRequest, TResponse>的子类
            var baseType = commandType.BaseType;
            while (baseType != null && baseType != typeof(object))
            {
                if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == typeof(BaseCommand<,>))
                {
                    // 获取泛型参数类型
                    var genericArgs = baseType.GetGenericArguments();
                    if (genericArgs.Length >= 1)
                    {
                        var requestType = genericArgs[0];
                        
                        // 获取Request属性
                        var requestProperty = baseType.GetProperty("Request");
                        if (requestProperty != null)
                        {
                            return requestProperty.GetValue(command);
                        }
                    }
                }
                baseType = baseType.BaseType;
            }

            return null;
        }
    }
}