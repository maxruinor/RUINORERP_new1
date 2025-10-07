using System;
using System.Threading;
using System.Threading.Tasks;
using RUINORERP.PacketSpec.Commands.Authentication;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.UI.Network;
using RUINORERP.UI.Network.Services;
using Microsoft.Extensions.Logging;

namespace RUINORERP.UI.Network.Test
{
    /// <summary>
    /// 测试登录修复的类
    /// </summary>
    public class TestLoginFix
    {
        private readonly ClientCommunicationService _communicationService;
        private readonly ILogger<TestLoginFix> _logger;

        public TestLoginFix(ClientCommunicationService communicationService, ILogger<TestLoginFix> logger)
        {
            _communicationService = communicationService;
            _logger = logger;
        }

        /// <summary>
        /// 测试登录命令的Request属性访问
        /// </summary>
        public async Task TestLoginCommandRequestAccess()
        {
            try
            {
                _logger.LogInformation("开始测试登录命令的Request属性访问...");

                // 创建登录请求
                var loginRequest = LoginRequest.Create("testuser", "testpass");
                
                // 创建登录命令
                var loginCommand = new LoginCommand("testuser", "testpass");
                loginCommand.Request = loginRequest;
                loginCommand.Request.RequestId = IdGenerator.GenerateRequestId(loginCommand.CommandIdentifier);
                loginCommand.ExecutionContext = new CommandExecutionContext();
                loginCommand.ExecutionContext.RequestId = loginCommand.Request.RequestId;

                _logger.LogInformation($"LoginCommand类型: {loginCommand.GetType().FullName}");
                _logger.LogInformation($"LoginCommand.BaseType: {loginCommand.GetType().BaseType?.FullName}");
                
                // 测试直接访问Request属性
                var directRequest = loginCommand.Request;
                _logger.LogInformation($"直接访问Request属性: {(directRequest != null ? "成功" : "失败")}");
                
                if (directRequest != null)
                {
                    _logger.LogInformation($"Request.RequestId: {directRequest.RequestId}");
                    _logger.LogInformation($"Request类型: {directRequest.GetType().FullName}");
                }

                // 测试通过基类访问Request属性
                var baseCommand = (BaseCommand)loginCommand;
                var baseRequest = baseCommand.Request;
                _logger.LogInformation($"通过基类访问Request属性: {(baseRequest != null ? "成功" : "失败")}");
                
                // 测试使用辅助方法获取请求数据
                var helperMethod = typeof(ClientCommunicationService).GetMethod("GetCommandRequestData", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                
                if (helperMethod != null)
                {
                    // 创建一个临时的ClientCommunicationService实例来测试
                    var tempService = new ClientCommunicationService(null, null, null, null, null);
                    var requestData = helperMethod.Invoke(tempService, new object[] { loginCommand });
                    _logger.LogInformation($"使用辅助方法获取Request数据: {(requestData != null ? "成功" : "失败")}");
                    
                    if (requestData != null)
                    {
                        _logger.LogInformation($"辅助方法获取的Request类型: {requestData.GetType().FullName}");
                        if (requestData is IRequest irequest)
                        {
                            _logger.LogInformation($"辅助方法获取的RequestId: {irequest.RequestId}");
                        }
                    }
                }

                _logger.LogInformation("测试完成！");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "测试登录命令Request属性访问时发生错误");
                throw;
            }
        }

        /// <summary>
        /// 模拟登录过程测试
        /// </summary>
        public async Task SimulateLoginProcess()
        {
            try
            {
                _logger.LogInformation("开始模拟登录过程测试...");

                // 创建登录请求
                var loginRequest = LoginRequest.Create("testuser", "testpass");
                
                // 创建登录命令
                var loginCommand = new LoginCommand("testuser", "testpass");
                loginCommand.Request = loginRequest;
                loginCommand.Request.RequestId = IdGenerator.GenerateRequestId(loginCommand.CommandIdentifier);
                loginCommand.ExecutionContext = new CommandExecutionContext();
                loginCommand.ExecutionContext.RequestId = loginCommand.Request.RequestId;

                _logger.LogInformation("准备调用SendCommandAsync方法...");
                
                // 注意：这里不会真正发送网络请求，因为我们没有连接到服务器
                // 我们只是测试类型转换和属性访问是否正常
                try
                {
                    // 测试泛型方法调用
                    var task = _communicationService.SendCommandAsync<LoginRequest, LoginResponse>(loginCommand, CancellationToken.None);
                    _logger.LogInformation("SendCommandAsync<LoginRequest, LoginResponse>调用成功");
                }
                catch (Exception ex)
                {
                    // 预期会失败，因为我们没有连接到服务器
                    _logger.LogInformation($"SendCommandAsync调用失败（预期）: {ex.Message}");
                }

                // 测试非泛型方法调用
                try
                {
                    var task = _communicationService.SendCommandAsync<LoginResponse>(loginCommand, CancellationToken.None);
                    _logger.LogInformation("SendCommandAsync<LoginResponse>调用成功");
                }
                catch (Exception ex)
                {
                    // 预期会失败，因为我们没有连接到服务器
                    _logger.LogInformation($"SendCommandAsync<LoginResponse>调用失败（预期）: {ex.Message}");
                }

                _logger.LogInformation("模拟登录过程测试完成！");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "模拟登录过程测试时发生错误");
                throw;
            }
        }
    }
}