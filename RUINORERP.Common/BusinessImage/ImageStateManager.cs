using RUINORERP.Common.BusinessImage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace RUINORERP.Common.BusinessImage
{
    /// <summary>
    /// 图片状态管理器
    /// 负责管理图片的上传队列和删除队列，确保图片操作的原子性和唯一性
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
        /// 获取处理中图片数量
        /// </summary>
        public int ProcessingCount
        {
            get
            {
                lock (_lock)
                {
                    return _images.Values.Count(img => img.Status == ImageStatus.Processing);
                }
            }
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="message">日志消息</param>
        /// <param name="level">日志级别</param>
        private static void Log(string message, string level = "INFO")
        {
            if (EnableVerboseLogging)
            {
                Debug.WriteLine($"[ImageStateManager][{level}][{DateTime.Now:HH:mm:ss.fff}] {message}");
            }
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
                    // 如果FileId为0，使用ImageId作为键
                    long key = imageInfo.FileId > 0 ? imageInfo.FileId : imageInfo.ImageId;
                    if (key > 0)
                    {
                        bool isUpdate = _images.ContainsKey(key);
                        // 直接赋值，会覆盖已存在的记录，避免重复添加
                        _images[key] = imageInfo;
                        
                        if (isUpdate)
                        {
                            Log($"更新图片: ID={key}, 状态={imageInfo.Status}, 文件名={imageInfo.FileName ?? imageInfo.OriginalFileName}");
                        }
                        else
                        {
                            Log($"添加图片: ID={key}, 状态={imageInfo.Status}, 文件名={imageInfo.FileName ?? imageInfo.OriginalFileName}");
                        }
                    }
                    else
                    {
                        Log($"添加图片失败: 无效的图片ID (FileId={imageInfo.FileId}, ImageId={imageInfo.ImageId})", "WARN");
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
                    var imageInfo = _images[fileId];
                    _images.Remove(fileId);
                    Log($"移除图片: ID={fileId}, 文件名={imageInfo?.FileName ?? imageInfo?.OriginalFileName}");
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
                return ImageStatus.Deleted;
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
                    var oldStatus = _images[fileId].Status;
                    _images[fileId].Status = status;
                    Log($"更新图片状态: ID={fileId}, {oldStatus} -> {status}");
                }
                else
                {
                    Log($"更新图片状态失败: 图片不存在 ID={fileId}", "WARN");
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
        /// 添加图片（7参数重载）
        /// </summary>
        /// <param name="cell">单元格对象</param>
        /// <param name="imageId">图片ID</param>
        /// <param name="fileName">文件名</param>
        /// <param name="imageData">图片数据</param>
        /// <param name="status">图片状态</param>
        /// <param name="businessId">业务ID</param>
        /// <param name="storagePath">存储路径</param>
        public void AddImage(object cell, long imageId, string fileName, byte[] imageData, ImageStatus status, long businessId, string storagePath)
        {
            var imageInfo = new ImageInfo
            {
                FileId = imageId,
                ImageId = imageId,
                OriginalFileName = fileName,
                FileName = fileName,
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
        /// 添加图片（5参数重载）
        /// </summary>
        /// <param name="cell">单元格对象</param>
        /// <param name="imageId">图片ID</param>
        /// <param name="fileName">文件名</param>
        /// <param name="imageData">图片数据</param>
        /// <param name="status">图片状态</param>
        public void AddImage(object cell, long imageId, string fileName, byte[] imageData, ImageStatus status)
        {
            AddImage(cell, imageId, fileName, imageData, status, 0, string.Empty);
        }
        
        /// <summary>
        /// 移除已处理的图片
        /// </summary>
        public void RemoveProcessedImages()
        {
            lock (_lock)
            {
                var processedImages = _images.Where(kv => kv.Value.Status == ImageStatus.Uploaded || kv.Value.Status == ImageStatus.Deleted).ToList();
                foreach (var kv in processedImages)
                {
                    _images.Remove(kv.Key);
                }
            }
        }
        
        /// <summary>
        /// 移除已处理的图片（重载 - 按单元格）
        /// </summary>
        /// <param name="cell">单元格对象</param>
        public void RemoveProcessedImages(object cell)
        {
            lock (_lock)
            {
                var processedImages = _images.Where(kv => 
                    (kv.Value.Status == ImageStatus.Uploaded || kv.Value.Status == ImageStatus.Deleted) && 
                    kv.Value.Cell == cell).ToList();
                foreach (var kv in processedImages)
                {
                    _images.Remove(kv.Key);
                }
                if (processedImages.Count > 0)
                {
                    Log($"移除已处理图片(按单元格): {processedImages.Count} 张");
                }
            }
        }
        
        /// <summary>
        /// 移除已处理的图片（重载 - 按图片ID列表）
        /// </summary>
        /// <param name="imageIds">图片ID列表</param>
        public void RemoveProcessedImages(List<long> imageIds)
        {
            if (imageIds == null || imageIds.Count == 0)
                return;
                
            lock (_lock)
            {
                int removedCount = 0;
                foreach (var imageId in imageIds)
                {
                    if (imageId > 0 && _images.ContainsKey(imageId))
                    {
                        _images.Remove(imageId);
                        removedCount++;
                    }
                }
                Log($"移除已处理图片(按ID列表): 请求 {imageIds.Count} 张, 实际移除 {removedCount} 张");
            }
        }
        
        /// <summary>
        /// 标记图片为处理中状态（防止重复处理）
        /// </summary>
        /// <param name="imageIds">图片ID列表</param>
        /// <returns>成功标记的数量</returns>
        public int MarkImagesAsProcessing(List<long> imageIds)
        {
            if (imageIds == null || imageIds.Count == 0)
                return 0;
                
            int markedCount = 0;
            lock (_lock)
            {
                foreach (var imageId in imageIds)
                {
                    if (imageId > 0 && _images.ContainsKey(imageId))
                    {
                        var imageInfo = _images[imageId];
                        if (imageInfo.Status == ImageStatus.PendingUpload || imageInfo.Status == ImageStatus.PendingDelete)
                        {
                            imageInfo.Status = ImageStatus.Processing;
                            markedCount++;
                        }
                    }
                }
            }
            Log($"标记图片为处理中: 请求 {imageIds.Count} 张, 成功标记 {markedCount} 张");
            return markedCount;
        }
        
        /// <summary>
        /// 获取待上传图片（原子操作：获取并标记为处理中）
        /// </summary>
        /// <returns>待上传图片列表</returns>
        public List<ImageInfo> GetAndLockPendingUploadImages()
        {
            lock (_lock)
            {
                var pendingImages = _images.Values.Where(img => img.Status == ImageStatus.PendingUpload).ToList();
                foreach (var image in pendingImages)
                {
                    image.Status = ImageStatus.Processing;
                }
                Log($"原子获取待上传图片: {pendingImages.Count} 张, 已标记为处理中");
                return pendingImages;
            }
        }
        
        /// <summary>
        /// 获取待删除图片（原子操作：获取并标记为处理中）
        /// </summary>
        /// <returns>待删除图片列表</returns>
        public List<ImageInfo> GetAndLockPendingDeleteImages()
        {
            lock (_lock)
            {
                var pendingImages = _images.Values.Where(img => img.Status == ImageStatus.PendingDelete).ToList();
                foreach (var image in pendingImages)
                {
                    image.Status = ImageStatus.Processing;
                }
                Log($"原子获取待删除图片: {pendingImages.Count} 张, 已标记为处理中");
                return pendingImages;
            }
        }
        
        /// <summary>
        /// 根据单元格获取图片
        /// </summary>
        /// <param name="cell">单元格对象</param>
        /// <returns>图片信息列表</returns>
        public System.Collections.Generic.List<ImageInfo> GetImagesByCell(object cell)
        {
            lock (_lock)
            {
                return _images.Values.Where(img => img.Cell == cell).ToList();
            }
        }
        
        /// <summary>
        /// 获取所有图片
        /// </summary>
        /// <returns>所有图片信息列表</returns>
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
        /// <param name="fileIds">图片ID列表</param>
        public void RemoveImages(List<long> fileIds)
        {
            lock (_lock)
            {
                if (fileIds != null)
                {
                    foreach (var fileId in fileIds)
                    {
                        if (fileId > 0 && _images.ContainsKey(fileId))
                        {
                            _images.Remove(fileId);
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// 获取指定状态的图片数量
        /// </summary>
        /// <param name="status">图片状态</param>
        /// <returns>图片数量</returns>
        public int GetImageCountByStatus(ImageStatus status)
        {
            lock (_lock)
            {
                return _images.Values.Count(img => img.Status == status);
            }
        }
        
        /// <summary>
        /// 清理过期的图片信息
        /// </summary>
        /// <param name="expireMinutes">过期分钟数</param>
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
            }
        }
    }
}


// 移除所有过度设计的复杂类，回归简单清晰的设计
// 核心功能只需要 ImageStatus 和基本的 ImageInfo 管理即可