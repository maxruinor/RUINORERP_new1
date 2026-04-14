using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace RUINORERP.Lib.BusinessImage
{
    /// <summary>
    /// 图片状态管理器（简化版）1
    /// 只管理两种状态：待上传、待删除
    /// </summary>
    public class ImageStateManager
    {
        private static readonly ImageStateManager _instance = new ImageStateManager();
        private readonly Dictionary<long, ImageInfo> _images = new Dictionary<long, ImageInfo>();
        private readonly object _lock = new object();

        /// <summary>
        /// 是否启用详细日志
        /// </summary>
        public static bool EnableVerboseLogging { get; set; } = true;

        /// <summary>
        /// 单例实例
        /// </summary>
        public static ImageStateManager Instance => _instance;

        /// <summary>
        /// 获取当前队列中的图片总数
        /// </summary>
        public int TotalImageCount
        {
            get
            {
                lock (_lock)
                {
                    return _images.Count;
                }
            }
        }

        /// <summary>
        /// 获取待上传图片数量
        /// </summary>
        public int PendingUploadCount
        {
            get
            {
                lock (_lock)
                {
                    return _images.Values.Count(img => img.Status == ImageStatus.PendingUpload);
                }
            }
        }

        /// <summary>
        /// 获取待删除图片数量
        /// </summary>
        public int PendingDeleteCount
        {
            get
            {
                lock (_lock)
                {
                    return _images.Values.Count(img => img.Status == ImageStatus.PendingDelete);
                }
            }
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        private static void Log(string message)
        {
            if (EnableVerboseLogging)
            {
                Debug.WriteLine($"[ImageStateManager][{DateTime.Now:HH:mm:ss.fff}] {message}");
            }
        }

        /// <summary>
        /// 生成默认ID
        /// 当FileId无效时使用文件名的哈希值作为唯一标识
        /// </summary>
        /// <param name="imageInfo">图片信息</param>
        /// <returns>默认ID</returns>
        private static long GenerateDefaultId(ImageInfo imageInfo)
        {
            if (!string.IsNullOrEmpty(imageInfo.OriginalFileName))
            {
                return Math.Abs(imageInfo.OriginalFileName.GetHashCode());
            }
            if (!string.IsNullOrEmpty(imageInfo.StorageFileName))
            {
                return Math.Abs(imageInfo.StorageFileName.GetHashCode());
            }
            return DateTime.Now.Ticks;
        }

        /// <summary>
        /// 添加图片
        /// </summary>
        /// <param name="imageInfo">图片信息</param>
        public void AddImage(ImageInfo imageInfo)
        {
            lock (_lock)
            {
                if (imageInfo != null)
                {
                    long key = imageInfo.FileId > 0 ? imageInfo.FileId : GenerateDefaultId(imageInfo);
                    if (key > 0)
                    {
                        bool isUpdate = _images.ContainsKey(key);
                        _images[key] = imageInfo;
                        Log(isUpdate ? $"更新图片: ID={key}" : $"添加图片: ID={key}");
                    }
                }
            }
        }

        /// <summary>
        /// 移除图片
        /// </summary>
        /// <param name="fileId">图片ID</param>
        public void RemoveImage(long fileId)
        {
            lock (_lock)
            {
                if (fileId > 0 && _images.ContainsKey(fileId))
                {
                    _images.Remove(fileId);
                    Log($"移除图片: ID={fileId}");
                }
            }
        }

        /// <summary>
        /// 获取待上传图片
        /// </summary>
        /// <returns>待上传图片列表</returns>
        public List<ImageInfo> GetPendingUploadImages()
        {
            lock (_lock)
            {
                var result = _images.Values.Where(img => img.Status == ImageStatus.PendingUpload).ToList();
                Log($"获取待上传图片: {result.Count} 张");
                return result;
            }
        }

        /// <summary>
        /// 获取待删除图片
        /// </summary>
        /// <returns>待删除图片列表</returns>
        public List<ImageInfo> GetPendingDeleteImages()
        {
            lock (_lock)
            {
                var result = _images.Values.Where(img => img.Status == ImageStatus.PendingDelete).ToList();
                Log($"获取待删除图片: {result.Count} 张");
                return result;
            }
        }

        /// <summary>
        /// 获取图片状态
        /// </summary>
        /// <param name="fileId">图片ID</param>
        /// <returns>图片状态</returns>
        public ImageStatus GetImageStatus(long fileId)
        {
            lock (_lock)
            {
                if (fileId > 0 && _images.ContainsKey(fileId))
                {
                    return _images[fileId].Status;
                }
                return ImageStatus.Normal;
            }
        }

        /// <summary>
        /// 获取图片信息
        /// </summary>
        /// <param name="fileId">图片ID</param>
        /// <returns>图片信息</returns>
        public ImageInfo GetImageInfo(long fileId)
        {
            lock (_lock)
            {
                if (fileId > 0 && _images.ContainsKey(fileId))
                {
                    return _images[fileId];
                }
                return null;
            }
        }

        /// <summary>
        /// 更新图片状态
        /// </summary>
        /// <param name="fileId">图片ID</param>
        /// <param name="status">新状态</param>
        public void UpdateImageStatus(long fileId, ImageStatus status)
        {
            lock (_lock)
            {
                if (fileId > 0 && _images.ContainsKey(fileId))
                {
                    _images[fileId].Status = status;
                    Log($"更新图片状态: ID={fileId}, 状态={status}");
                }
            }
        }

        /// <summary>
        /// 清空所有图片状态
        /// </summary>
        public void Clear()
        {
            lock (_lock)
            {
                int count = _images.Count;
                _images.Clear();
                Log($"清空所有图片状态: 移除 {count} 张图片");
            }
        }

        /// <summary>
        /// 重置状态
        /// </summary>
        public void ResetStatus()
        {
            Clear();
        }

        /// <summary>
        /// 添加图片（7参数重载）
        /// </summary>
        public void AddImage(object cell, long imageId, string fileName, byte[] imageData, ImageStatus status, long businessId, string OwnerTableName, string storagePath = "")
        {
            var imageInfo = new ImageInfo
            {
                FileId = imageId,
                OriginalFileName = fileName,
                FileName = fileName,
                OwnerTableName = OwnerTableName,
                ImageData = imageData,
                Status = status,
                BusinessId = businessId,
                StoragePath = storagePath,
                Cell = cell,
                CreateTime = System.DateTime.Now,
                ModifiedAt = System.DateTime.Now
            };
            AddImage(imageInfo);
        }



        /// <summary>
        /// 移除已处理的图片（操作完成后调用）
        /// </summary>
        public void RemoveProcessedImages()
        {
            lock (_lock)
            {
                int count = _images.Count;
                _images.Clear();
                Log($"移除已处理图片: {count} 张");
            }
        }

        /// <summary>
        /// 移除已处理的图片（按单元格）
        /// </summary>
        public void RemoveProcessedImages(object cell)
        {
            lock (_lock)
            {
                var toRemove = _images.Values.Where(img => img.Cell == cell).Select(img => img.FileId > 0 ? img.FileId : img.FileId).ToList();
                foreach (var id in toRemove)
                {
                    _images.Remove(id);
                }
                if (toRemove.Count > 0)
                {
                    Log($"移除已处理图片(按单元格): {toRemove.Count} 张");
                }
            }
        }

        /// <summary>
        /// 移除已处理的图片（按图片ID列表）
        /// </summary>
        public void RemoveProcessedImages(List<long> imageIds)
        {
            if (imageIds == null || imageIds.Count == 0)
                return;

            lock (_lock)
            {
                foreach (var imageId in imageIds)
                {
                    _images.Remove(imageId);
                }
                Log($"移除已处理图片(按ID列表): {imageIds.Count} 张");
            }
        }

        /// <summary>
        /// 获取待上传图片（获取后直接移除，由调用方保证操作成功）
        /// </summary>
        public List<ImageInfo> GetAndLockPendingUploadImages()
        {
            lock (_lock)
            {
                var pendingImages = _images.Values.Where(img => img.Status == ImageStatus.PendingUpload && img.BusinessId > 0).ToList();
                Log($"获取待上传图片: {pendingImages.Count} 张");
                return pendingImages;
            }
        }

        /// <summary>
        /// 获取待删除图片（获取后直接移除，由调用方保证操作成功）
        /// </summary>
        public List<ImageInfo> GetAndLockPendingDeleteImages()
        {
            lock (_lock)
            {
                var pendingImages = _images.Values.Where(img => img.Status == ImageStatus.PendingDelete).ToList();
                Log($"获取待删除图片: {pendingImages.Count} 张");
                return pendingImages;
            }
        }

        /// <summary>
        /// 根据单元格获取图片
        /// </summary>
        public List<ImageInfo> GetImagesByCell(object cell)
        {
            lock (_lock)
            {
                return _images.Values.Where(img => img.Cell == cell).ToList();
            }
        }

        /// <summary>
        /// 获取所有图片
        /// </summary>
        public List<ImageInfo> GetAllImages()
        {
            lock (_lock)
            {
                return _images.Values.ToList();
            }
        }

        /// <summary>
        /// 批量移除图片
        /// </summary>
        public void RemoveImages(List<long> fileIds)
        {
            lock (_lock)
            {
                if (fileIds != null)
                {
                    foreach (var fileId in fileIds)
                    {
                        _images.Remove(fileId);
                    }
                }
            }
        }

        /// <summary>
        /// 清理过期的图片信息（超过指定分钟数且仍为待处理状态的图片）
        /// </summary>
        public void CleanupExpiredImages(int expireMinutes = 30)
        {
            lock (_lock)
            {
                var expireTime = DateTime.Now.AddMinutes(-expireMinutes);
                var expiredImages = _images.Where(kv =>
                    kv.Value.ModifiedAt < expireTime &&
                    (kv.Value.Status == ImageStatus.PendingUpload || kv.Value.Status == ImageStatus.PendingDelete))
                    .ToList();

                foreach (var kv in expiredImages)
                {
                    _images.Remove(kv.Key);
                }
                if (expiredImages.Count > 0)
                {
                    Log($"清理过期图片: {expiredImages.Count} 张");
                }
            }
        }

        /// <summary>
        /// 获取统计信息
        /// </summary>
        public ImageManagerStatistics GetStatistics()
        {
            lock (_lock)
            {
                return new ImageManagerStatistics
                {
                    TotalImages = _images.Count,
                    PendingUpload = PendingUploadCount,
                    PendingDelete = PendingDeleteCount
                };
            }
        }
    }

    /// <summary>
    /// 图片管理器统计信息（简化版）
    /// </summary>
    public class ImageManagerStatistics
    {
        public int TotalImages { get; set; }
        public int PendingUpload { get; set; }
        public int PendingDelete { get; set; }
    }
}
