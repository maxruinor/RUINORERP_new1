using RUINORERP.PacketSpec.Models.Core;
using RUINORERP.PacketSpec.Models.Responses;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using System.Collections.Generic;
using System.IO.Pipelines;
using RUINORERP.PacketSpec.Enums.Core;

namespace RUINORERP.PacketSpec.Commands.FileTransfer
{
    /// <summary>
    /// 文件上传命令 - 客户端向服务器上传文件
    /// </summary>
    [PacketCommand("FileUpload", CommandCategory.File)]
    public class FileUploadCommand : BaseCommand
    {
        /// <summary>
        /// 命令标识符
        /// </summary>
        public override CommandId CommandIdentifier => FileCommands.FileUpload;

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 文件大小（字节）
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// 文件内容管道读取器
        /// </summary>
        public PipeReader FileContentReader { get; set; }

        /// <summary>
        /// 目标路径
        /// </summary>
        public string TargetPath { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public FileUploadCommand()
        {
            Direction = PacketDirection.ClientToServer;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="fileContentReader">文件内容管道读取器</param>
        /// <param name="targetPath">目标路径</param>
        public FileUploadCommand(string fileName, PipeReader fileContentReader, string targetPath = "")
        {
            FileName = fileName;
            FileContentReader = fileContentReader;
            TargetPath = targetPath;
            Direction = PacketDirection.ClientToServer;
        }

        /// <summary>
        /// 获取文件内容的异步流
        /// </summary>
        /// <returns>文件内容的异步可枚举集合</returns>
        public async IAsyncEnumerable<ReadOnlyMemory<byte>> ChunkStream()
        {
            if (FileContentReader == null)
            {
                yield break;
            }

            try
            {
                ReadResult result;
                do
                {
                    result = await FileContentReader.ReadAsync();
                    var buffer = result.Buffer;

                    foreach (var memory in buffer)
                    {
                        yield return memory;
                    }

                    FileContentReader.AdvanceTo(buffer.End);
                }
                while (!result.IsCompleted);
            }
            finally
            {
                await FileContentReader.CompleteAsync();
            }
        }

        /// <summary>
        /// 验证命令参数
        /// </summary>
        /// <returns>验证结果</returns>
        public override CommandValidationResult Validate()
        {
            var result = base.Validate();
            if (!result.IsValid)
            {
                return result;
            }

            // 验证文件名
            if (string.IsNullOrWhiteSpace(FileName))
            {
                return CommandValidationResult.Failure("文件名不能为空", "INVALID_FILE_NAME");
            }

            // 验证文件内容读取器
            if (FileContentReader == null)
            {
                return CommandValidationResult.Failure("文件内容读取器不能为空", "INVALID_FILE_CONTENT");
            }

            // 验证文件大小
            if (FileSize <= 0)
            {
                return CommandValidationResult.Failure("文件大小无效", "INVALID_FILE_SIZE");
            }

            return CommandValidationResult.Success();
        }

        /// <summary>
        /// 执行命令的核心逻辑
        /// </summary>
        /// <param name="cancellationToken">取消令牌</param>
        /// <returns>命令执行结果</returns>
        protected override Task<ResponseBase> OnExecuteAsync(CancellationToken cancellationToken)
        {
            // 文件上传命令契约只定义数据结构，实际的业务逻辑在Handler中实现
            var result = (ResponseBase)ResponseBase.CreateSuccess(
               // new { FileName = FileName, FileSize = FileSize, TargetPath = TargetPath },
                "文件上传命令构建成功"
            );
            return Task.FromResult(result);
        }
    }
}
