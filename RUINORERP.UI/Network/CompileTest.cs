using System;
using System.Threading.Tasks;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Models.Requests;
using RUINORERP.PacketSpec.Models.Responses;
using RUINORERP.UI.Network;

namespace RUINORERP.UI.Network.Test
{
    /// <summary>
    /// 编译测试类 - 用于验证ClientCommunicationService的编译问题是否已修复
    /// </summary>
    public class CompileTest
    {
        private readonly ClientCommunicationService _communicationService;

        public CompileTest(ClientCommunicationService communicationService)
        {
            _communicationService = communicationService;
        }

        /// <summary>
        /// 测试SendRequestWithRetryAsync方法是否能正常编译
        /// </summary>
        public async Task TestSendRequestWithRetryAsync()
        {
            try
            {
                // 创建测试命令
                var command = new LoginCommand();
                
                // 测试简化版本的SendRequestWithRetryAsync
                var response = await _communicationService.SendRequestWithRetryAsync<LoginResponse>(
                    command,
                    null,
                    default);
                    
                Console.WriteLine($"测试通过，响应: {response != null}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"测试失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 测试SendCommandAsync方法是否能正常编译
        /// </summary>
        public async Task TestSendCommandAsync()
        {
            try
            {
                // 创建测试命令
                var command = new LoginCommand();
                
                // 测试SendCommandAsync<TResponse>方法
                var response = await _communicationService.SendCommandAsync<LoginResponse>(
                    command,
                    default,
                    30000);
                    
                Console.WriteLine($"测试通过，响应: {response != null}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"测试失败: {ex.Message}");
            }
        }
    }
}