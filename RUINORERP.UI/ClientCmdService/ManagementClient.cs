using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransInstruction.CommandService;
using TransInstruction.DataModel;
using TransInstruction.Enums;
using SuperSocket.ClientEngine;
using System.Net;
using SuperSocket.ProtoBase;

namespace RUINORERP.UI.ClientCmdService
{
    // 客户端代码 - 发送管理指令
    public class ManagementClient
    {
        private readonly EasyClient<StringPackageInfo> _easyClient;

        public ManagementClient(EasyClient<StringPackageInfo> easyClient)
        {
            _easyClient = easyClient;
        }

        public async Task<StorageUsageInfo> GetStorageUsageAsync()
        {
            var command = new ManagementCommand
            {
                CommandType = ManagementCommandType.GetStorageUsage,
                Parameters = new Dictionary<string, object>()
            };

            // 发送命令
            var commandData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(command));
             _easyClient.Send(commandData);

            // 等待并接收响应
            var response = await WaitForResponseAsync();
            
            var responseObj = JsonConvert.DeserializeObject<dynamic>(Encoding.UTF8.GetString(response));

            if (responseObj.Success)
            {
                return JsonConvert.DeserializeObject<StorageUsageInfo>(responseObj.Data.ToString());
            }

            throw new Exception(responseObj.Message.ToString());
        }

        public async Task<bool> CleanTempFilesAsync(int days = 7)
        {
            var command = new ManagementCommand
            {
                CommandType = ManagementCommandType.CleanTempFiles,
                Parameters = new Dictionary<string, object>
                {
                    { "days", days }
                }
            };

            // 发送命令
            var commandData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(command));
             _easyClient.Send(commandData);

            // 等待并接收响应
            var response = await WaitForResponseAsync();
            
            var responseObj = JsonConvert.DeserializeObject<dynamic>(Encoding.UTF8.GetString(response));
            return responseObj.Success;
        }

        // 简化的等待响应方法（实际项目中应该有更完善的实现）
        private async Task<byte[]> WaitForResponseAsync()
        {
            // 这里应该实现一个响应等待机制
            // 例如使用TaskCompletionSource或其他同步机制
            await Task.Delay(1000); // 临时等待
            return Encoding.UTF8.GetBytes("{\"Success\":true,\"Data\":\"{}\"}");
        }
    }
}