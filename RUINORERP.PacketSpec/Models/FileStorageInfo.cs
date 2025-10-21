using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessagePack;
using System.Runtime.Serialization;
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
        
        /// <summary>
        /// 分类列表（用于UI显示）
        /// </summary>
        [IgnoreDataMember]
        public List<StorageCategoryInfo> Categories
        {
            get
            {
                var categories = new List<StorageCategoryInfo>();
                if (CategoryUsage != null)
                {
                    foreach (var kvp in CategoryUsage)
                    {
                        categories.Add(new StorageCategoryInfo
                        {
                            CategoryName = kvp.Key,
                            FileCount = kvp.Value.FileCount,
                            StorageSize = kvp.Value.TotalSize,
                            LastSyncTime = DateTime.Now, // 由于CategoryUsage中没有LastSyncTime，这里使用当前时间作为默认值
                            SyncStatus = "已同步", // 设置默认同步状态
                            ErrorMessage = null // 设置默认错误信息
                        });
                    }
                }
                return categories;
            }
        }
    }
    
    /// <summary>
    /// 存储分类信息（用于UI显示）
    /// </summary>
    public class StorageCategoryInfo
    {
        /// <summary>
        /// 分类名称
        /// </summary>
        public string CategoryName { get; set; }
        
        /// <summary>
        /// 文件数量
        /// </summary>
        public int FileCount { get; set; }
        
        /// <summary>
        /// 存储大小
        /// </summary>
        public long StorageSize { get; set; }
        
        /// <summary>
        /// 最后同步时间
        /// </summary>
        public DateTime LastSyncTime { get; set; }
        
        /// <summary>
        /// 同步状态
        /// </summary>
        public string SyncStatus { get; set; }
        
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; set; }
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
            this.Timestamp = DateTime.Now;
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
