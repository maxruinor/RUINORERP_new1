using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessagePack;
using RUINORERP.PacketSpec.Models.Responses;

namespace RUINORERP.PacketSpec.Models
{
    internal class FileStorageInfo
    {
    }

    /// <summary>
    /// 存储使用信息数据类
    /// </summary>
    [MessagePackObject]
    public class StorageUsageInfoData
    {
        /// <summary>
        /// 总存储大小（字节）
        /// </summary>
        [Key(0)]
        public long TotalSize { get; set; }

        /// <summary>
        /// 总文件数量
        /// </summary>
        [Key(1)]
        public int TotalFileCount { get; set; }

        /// <summary>
        /// 分类使用情况字典
        /// </summary>
        [Key(2)]
        public Dictionary<string, CategoryUsage> CategoryUsage { get; set; } = new Dictionary<string, CategoryUsage>();
    }

    /// <summary>
    /// 存储使用信息响应类 - 使用统一的ApiResponse模式
    /// </summary>
    public class StorageUsageInfo : ResponseBase<StorageUsageInfoData>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public StorageUsageInfo() : base() { }

        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        public StorageUsageInfo(bool success, string message, StorageUsageInfoData data = null, int code = 200) 
        {
            this.IsSuccess = success;
            this.Message = message;
            this.Data = data;
            this.TimestampUtc = DateTime.UtcNow;
        }

        /// <summary>
        /// 创建成功结果
        /// </summary>
        public static StorageUsageInfo CreateSuccess(long totalSize, int totalFileCount, Dictionary<string, CategoryUsage> categoryUsage, string message = "获取存储使用信息成功")
        {
            var data = new StorageUsageInfoData
            {
                TotalSize = totalSize,
                TotalFileCount = totalFileCount,
                CategoryUsage = categoryUsage
            };
            return new StorageUsageInfo(true, message, data, 200);
        }

        /// <summary>
        /// 创建失败结果
        /// </summary>
        public static StorageUsageInfo CreateFailure(string message, int code = 500)
        {
            return new StorageUsageInfo(false, message, null, code);
        }
    }

    [MessagePackObject]
    public class CategoryUsage
    {
        [Key(0)]
        public int FileCount { get; set; }
        [Key(1)]
        public long TotalSize { get; set; }
    }

}
