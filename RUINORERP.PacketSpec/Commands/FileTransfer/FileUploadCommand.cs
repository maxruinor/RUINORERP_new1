﻿using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using RUINORERP.PacketSpec.Enums.Core;
using RUINORERP.PacketSpec.Models.Requests;

namespace RUINORERP.PacketSpec.Commands.FileTransfer
{
    /// <summary>
    /// 文件上传命令
    /// </summary>
    public class FileUploadCommand : BaseCommand<FileUploadRequest, FileUploadResponse>
    {
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }

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
            // 注意：移除了 Direction 的设置，因为方向已由 PacketModel 统一控制
            CommandIdentifier = FileCommands.FileUpload;
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
            // 注意：移除了 Direction 的设置，因为方向已由 PacketModel 统一控制
            CommandIdentifier = FileCommands.FileUpload;
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


    }
}
