using System;
using System.Collections.Generic;
using System.Text;
using TransInstruction.Enums;

namespace TransInstruction.DataModel
{
    // 文件指令基类
    public class FileCommand
    {
        public FileOperationCommand Operation { get; set; }
        public string Data { get; set; } // JSON序列化的具体请求数据
    }

    public class FileDownloadRequest
    {
        public string Category { get; set; }
        public string FileId { get; set; }
    }
    public class FileDownloadResponse
    {
        public bool Success { get; set; }
        public string FileName { get; set; }
        public object Data { get; set; }
        public string Message { get; set; }
    }
}
