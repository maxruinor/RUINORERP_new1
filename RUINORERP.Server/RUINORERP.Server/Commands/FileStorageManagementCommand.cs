using SuperSocket.Command;
using SuperSocket.ProtoBase;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RUINORERP.Server.ServerSession;
using Newtonsoft.Json;
using RUINORERP.Server.CommandService;
using TransInstruction.DataModel;
using TransInstruction.Enums;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using TransInstruction.CommandService;
using TransInstruction;
using System.Threading;

namespace RUINORERP.Server.Commands
{
    [Command("XT")]
    public class FileStorageManagementCommand : IAsyncCommand<SessionforBiz, BizPackageInfo>
    {
        public async ValueTask ExecuteAsync(SessionforBiz session, BizPackageInfo package, CancellationToken cancellationToken)
        {
            try
            {
                if (package.ecode == SpecialOrder.长度等于18)
                {
                    // 处理长度为18的特殊包
                    await HandleSpecialPackage(session, package);
                    return;
                }

                // 解析管理指令
                var commandJson = Encoding.UTF8.GetString(package.Body);
                var managementCommand = JsonConvert.DeserializeObject<ManagementCommand>(commandJson);

                switch (managementCommand.CommandType)
                {
                    case ManagementCommandType.GetStorageUsage:
                        await HandleGetStorageUsageCommand(session, managementCommand);
                        break;

                    case ManagementCommandType.CleanTempFiles:
                        await HandleCleanTempFilesCommand(session, managementCommand);
                        break;

                    case ManagementCommandType.ListFiles:
                        await HandleListFilesCommand(session, managementCommand);
                        break;

                    default:
                        await SendErrorResponse(session, "不支持的管理指令");
                        break;
                }
            }
            catch (Exception ex)
            {
                await SendErrorResponse(session, $"处理管理指令失败: {ex.Message}");
            }
        }

        private async Task HandleSpecialPackage(SessionforBiz session, BizPackageInfo package)
        {
            // 处理特殊包，例如心跳包或者其他固定格式的包
            // 这里可以发送一个响应包
            await session.SendAsync(package.Body);
        }

        private async Task HandleGetStorageUsageCommand(SessionforBiz session, ManagementCommand command)
        {
            var fileService = Startup.GetFromFac<FileStorageManager>();

            var usageInfo = await fileService.GetStorageUsageAsync();

            var response = new
            {
                Success = true,
                Data = usageInfo,
                Message = "获取存储使用情况成功"
            };

            var responseData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response));
            await session.SendAsync(responseData);
        }

        private async Task HandleCleanTempFilesCommand(SessionforBiz session, ManagementCommand command)
        {
            var fileService = Startup.GetFromFac<FileStorageManager>();

            int days = 7; // 默认清理7天前的临时文件
            if (command.Parameters != null && command.Parameters.ContainsKey("days"))
            {
                days = Convert.ToInt32(command.Parameters["days"]);
            }

            var ts = TimeSpan.FromDays(days);
            var result = fileService.CleanTempFilesAsync(ts);

            var response = new
            {
                Success = result,
                Message = true ? "清理临时文件成功" : "清理临时文件失败"
            };

            var responseData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response));
            await session.SendAsync(responseData);
        }

        private async Task HandleListFilesCommand(SessionforBiz session, ManagementCommand command)
        {
            var fileService = Startup.GetFromFac<IFileStorageService>();

            string category = "";
            if (command.Parameters != null && command.Parameters.ContainsKey("category"))
            {
                category = command.Parameters["category"].ToString();
            }
            FileListRequest fileListRequest = new FileListRequest();
            fileListRequest.Category = category;
            var files = await fileService.ListFilesAsync(fileListRequest);

            var response = new
            {
                Success = true,
                Data = files,
                Message = "获取文件列表成功"
            };

            var responseData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response));
            await session.SendAsync(responseData);
        }

        private async Task SendErrorResponse(SessionforBiz session, string message)
        {
            var errorResponse = new
            {
                Success = false,
                Message = message
            };

            var responseData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(errorResponse));
            await session.SendAsync(responseData);
        }
    }
}