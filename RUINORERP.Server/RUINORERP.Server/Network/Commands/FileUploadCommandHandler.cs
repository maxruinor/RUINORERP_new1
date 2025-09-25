using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RUINORERP.PacketSpec.Commands;
using RUINORERP.PacketSpec.Commands.FileTransfer;
using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Protocol;
// using RUINORERP.Server.Network.Services; // 暂时注释，缺少IFileService接口定义

namespace RUINORERP.Server.Network.Commands
{
    /// <summary>
    /// 文件上传命令处理器 - 处理客户端的文件上传请求
    /// </summary>
    [CommandHandler("FileUploadCommandHandler", priority: 70)]
    public class FileUploadCommandHandler : CommandHandlerBase
    {
        // private readonly IFileService _fileService; // 暂时注释，缺少IFileService接口定义

        /// <summary>
        /// 无参构造函数，以支持Activator.CreateInstance创建实例
        /// </summary>
        public FileUploadCommandHandler() : base()
        {
            // _fileService = Program.ServiceProvider.GetRequiredService<IFileService>(); // 暂时注释，缺少IFileService接口定义
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public FileUploadCommandHandler(
            // IFileService fileService, // 暂时注释，缺少IFileService接口定义
            ILogger<FileUploadCommandHandler> logger = null) : base(logger)
        {
            // _fileService = fileService; // 暂时注释，缺少IFileService接口定义
        }

        /// <summary>
        /// 支持的命令类型
        /// </summary>
        public override IReadOnlyList<uint> SupportedCommands => new uint[]
        {
            (uint)FileCommands.FileUpload
        };

        /// <summary>
        /// 处理器优先级
        /// </summary>
        public override int Priority => 70;

        /// <summary>
        /// 具体的命令处理逻辑
        /// </summary>
        protected override async Task<CommandResult> ProcessCommandAsync(ICommand command, CancellationToken cancellationToken)
        {
            try
            {
                var commandId = command.CommandIdentifier;

                if (commandId == FileCommands.FileUpload)
                {
                    return await HandleFileUploadAsync(command, cancellationToken);
                }
                else
                {
                    return CommandResult.Failure($"不支持的命令类型: {command.CommandIdentifier}", "UNSUPPORTED_COMMAND");
                }
            }
            catch (Exception ex)
            {
                LogError($"处理文件上传命令异常: {ex.Message}", ex);
                return CommandResult.Failure($"处理异常: {ex.Message}", "HANDLER_ERROR", ex);
            }
        }

        /// <summary>
        /// 处理文件上传命令
        /// </summary>
        private async Task<CommandResult> HandleFileUploadAsync(ICommand command, CancellationToken cancellationToken)
        {
            try
            {
                LogInfo($"处理文件上传命令 [会话: {command.SessionID}]");

                // 解析文件上传数据
                var uploadData = command.Packet.GetJsonData<FileUploadData>();
                if (uploadData == null)
                {
                    return CommandResult.Failure("文件上传数据格式错误", "INVALID_UPLOAD_DATA");
                }

                // 暂时返回模拟结果，因为缺少IFileService接口定义
                var uploadResult = new FileUploadResult
                {
                    IsSuccess = true,
                    FileId = Guid.NewGuid().ToString(),
                    Message = "文件上传成功（模拟）"
                };

                // 创建响应数据
                var responseData = CreateFileUploadResponse(uploadResult);

                return CommandResult.SuccessWithResponse(
                    responseData,
                    data: new { 
                        FileName = uploadData.FileName,
                        FileSize = uploadData.FileSize,
                        TargetPath = uploadData.TargetPath,
                        FileId = uploadResult.FileId,
                        IsSuccess = uploadResult.IsSuccess,
                        SessionId = command.SessionID
                    },
                    message: uploadResult.IsSuccess ? "文件上传成功（模拟）" : "文件上传失败（模拟）"
                );
            }
            catch (Exception ex)
            {
                LogError($"处理文件上传命令异常: {ex.Message}", ex);
                return CommandResult.Failure($"文件上传异常: {ex.Message}", "FILE_UPLOAD_ERROR", ex);
            }
        }

        /// <summary>
        /// 解析文件上传数据
        /// </summary>
        private FileUploadData ParseFileUploadData(OriginalData originalData)
        {
            try
            {
                if (originalData.One == null || originalData.One.Length == 0)
                    return null;

                var dataString = System.Text.Encoding.UTF8.GetString(originalData.One);
                var parts = dataString.Split('|');

                if (parts.Length >= 4)
                {
                    return new FileUploadData
                    {
                        FileName = parts[0],
                        FileSize = long.TryParse(parts[1], out var size) ? size : 0,
                        FileContent = parts[2],
                        TargetPath = parts[3]
                    };
                }

                return null;
            }
            catch (Exception ex)
            {
                LogError($"解析文件上传数据异常: {ex.Message}", ex);
                return null;
            }
        }

        /// <summary>
        /// 创建文件上传响应
        /// </summary>
        private OriginalData CreateFileUploadResponse(FileUploadResult uploadResult)
        {
            var responseData = $"UPLOAD_RESULT|{uploadResult.IsSuccess}|{uploadResult.FileId}|{uploadResult.Message}";
            var data = System.Text.Encoding.UTF8.GetBytes(responseData);

            // 将完整的CommandId正确分解为Category和OperationCode
            uint commandId = (uint)FileCommands.FileUpload;
            byte category = (byte)(commandId & 0xFF); // 取低8位作为Category
            byte operationCode = (byte)((commandId >> 8) & 0xFF); // 取次低8位作为OperationCode

            return new OriginalData(
                category,
                new byte[] { operationCode },
                data
            );
        }
    }

    /// <summary>
    /// 文件上传数据
    /// </summary>
    public class FileUploadData
    {
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public string FileContent { get; set; }
        public string TargetPath { get; set; }
    }

    /// <summary>
    /// 文件上传结果
    /// </summary>
    public class FileUploadResult
    {
        public bool IsSuccess { get; set; }
        public string FileId { get; set; }
        public string Message { get; set; }
    }
}