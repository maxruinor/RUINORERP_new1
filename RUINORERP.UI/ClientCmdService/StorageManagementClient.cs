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
    // 客户端代码 - 请求服务器执行管理操作
    public class StorageManagementClient
    {
        private readonly EasyClient<StringPackageInfo> _easyClient;

        public StorageManagementClient(EasyClient<StringPackageInfo> easyClient)
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

            // 等待并接收响应（在实际实现中可能需要一个响应队列或回调机制）
            // 这里简化处理，实际项目中应该有更完善的响应处理机制
            var response = await WaitForResponseAsync();
            
            var managementResponse = JsonConvert.DeserializeObject<ManagementResponse>(
                Encoding.UTF8.GetString(response));

            if (managementResponse.Success)
            {
                return JsonConvert.DeserializeObject<StorageUsageInfo>(managementResponse.Data);
            }

            throw new Exception(managementResponse.Message);
        }

        public async Task<bool> CleanTempFilesAsync(int olderThanDays)
        {
            var command = new ManagementCommand
            {
                CommandType = ManagementCommandType.CleanTempFiles,
                Parameters = new Dictionary<string, object>
                {
                    { "days", olderThanDays }
                }
            };

            // 发送命令
            var commandData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(command));
             _easyClient.Send(commandData);

            // 等待并接收响应
            var response = await WaitForResponseAsync();
            
            var managementResponse = JsonConvert.DeserializeObject<ManagementResponse>(
                Encoding.UTF8.GetString(response));

            return managementResponse.Success;
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