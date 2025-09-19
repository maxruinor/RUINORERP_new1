using System;
using System.Collections.Generic;
using System.Text;

namespace TransInstruction.DataModel
{
    // 文件上传请求
    public class FileUploadRequest
    {
        public string FileName { get; set; }
        public string Category { get; set; } // 分类: Expenses/Products/Payments等
        public long FileSize { get; set; }
        public byte[] Data { get; set; }
        public int ChunkIndex { get; set; } // 分块索引
        public int TotalChunks { get; set; } // 总分块数
    }

    // 文件上传响应
    public class FileUploadResponse
    {
        public bool Success { get; set; }
        public string FileId { get; set; }
        public string Message { get; set; }
    }

}
