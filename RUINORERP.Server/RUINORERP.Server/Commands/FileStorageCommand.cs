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
using TransInstruction;
using System.Threading;

namespace RUINORERP.Server.Commands
{
    [Command("KXGame")]
    public class FileStorageCommand : IAsyncCommand<SessionforBiz, BizPackageInfo>
    {
        public async ValueTask ExecuteAsync(SessionforBiz session, BizPackageInfo package, CancellationToken cancellationToken)
        {
            try
            {
                // 解析指令
                var command = JsonConvert.DeserializeObject<FileCommand>(Encoding.UTF8.GetString(package.Body));

                switch (command.Operation)
                {
                    case FileOperationCommand.UploadFile:
                        await HandleUploadCommand(session, command);
                        break;

                    case FileOperationCommand.DownloadFile:
                        await HandleDownloadCommand(session, command);
                        break;

                    case FileOperationCommand.DeleteFile:
                        await HandleDeleteCommand(session, command);
                        break;

                    default:
                        await SendErrorResponse(session, "不支持的文件操作指令");
                        break;
                }
            }
            catch (Exception ex)
            {
                await SendErrorResponse(session, $"处理文件指令失败: {ex.Message}");
            }
        }
       

        private async Task HandleUploadCommand(SessionforBiz session, FileCommand command)
        {
            
            var fileService = Startup.GetFromFac<IFileStorageService>();
            
            var request = JsonConvert.DeserializeObject<FileUploadRequest>(command.Data);
            var response = await fileService.UploadFileAsync(request);

            // 发送响应
            var responseData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response));
            await session.SendAsync(responseData);
        }

        private async Task HandleDownloadCommand(SessionforBiz session, FileCommand command)
        {
            var fileService = Startup.GetFromFac<IFileStorageService>();

            var request = JsonConvert.DeserializeObject<FileDownloadRequest>(command.Data);
            var response = await fileService.DownloadFileAsync(request);

            // 发送响应
            var responseData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response));
            await session.SendAsync(responseData);
        }

        private async Task HandleDeleteCommand(SessionforBiz session, FileCommand command)
        {
            var fileService = Startup.GetFromFac<IFileStorageService>();

            var request = JsonConvert.DeserializeObject<FileDeleteRequest>(command.Data);
            var response = await fileService.DeleteFileAsync(request);

            // 发送响应
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