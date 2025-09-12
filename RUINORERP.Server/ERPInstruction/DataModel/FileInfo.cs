using System;
using System.Collections.Generic;
using System.Text;

namespace TransInstruction.DataModel
{
    // 文件信息类
    public class UploadFileInfo
    {
        public string FileId { get; set; }
        public string OriginalName { get; set; }
        public string Category { get; set; }
        public long Size { get; set; }
        public DateTime UploadTime { get; set; }
        public DateTime LastModified { get; set; }
    }

    // 请求和响应类
    public class FileDeleteRequest
    {
        public string FileId { get; set; }
    }

    public class FileDeleteResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class FileInfoRequest
    {
        public string FileId { get; set; }
    }

    public class FileInfoResponse
    {
        public bool Success { get; set; }
        public UploadFileInfo FileInfo { get; set; }
        public string Message { get; set; }
    }

    public class FileListRequest
    {
        public string Category { get; set; }
        public string Pattern { get; set; } // 文件名模式匹配
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

    public class FileListResponse
    {
        public bool Success { get; set; }
        public List<UploadFileInfo> Files { get; set; }
        public int TotalCount { get; set; }
        public string Message { get; set; }
    }
}
