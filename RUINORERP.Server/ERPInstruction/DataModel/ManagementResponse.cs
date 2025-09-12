using System;
using System.Collections.Generic;
using System.Text;

namespace TransInstruction.DataModel
{
    public class ManagementResponse
    {
        public bool Success { get; set; }
        public string Data { get; set; } // JSON序列化的响应数据
        public string Message { get; set; }
    }

    public class StorageUsageInfo
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public long TotalSize { get; set; }
        public int TotalFileCount { get; set; }
        public Dictionary<string, CategoryUsage> CategoryUsage { get; set; } = new Dictionary<string, CategoryUsage>();
    }

    public class CategoryUsage
    {
        public int FileCount { get; set; }
        public long TotalSize { get; set; }
    }



}
