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
    /// 增强功能：操作历史记录、状态验证、批量操作等
    /// </summary>
    public class ImageStateManager
    {
        private static readonly ImageStateManager _instance = new ImageStateManager();
        private readonly Dictionary<long, ImageInfo> _images = new Dictionary<long, ImageInfo>();
        private readonly object _lock = new object();
        private readonly List<ImageOperationRecord> _operationHistory = new List<ImageOperationRecord>();
        private const int MaxHistoryRecords = 100;
        
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
        /// 记录操作历史
        /// </summary>
        /// <param name="operation">操作记录</param>
        private void RecordOperation(ImageOperationRecord operation)
        {
            lock (_lock)
            {
                _operationHistory.Add(operation);
                if (_operationHistory.Count > MaxHistoryRecords)
                {
                    _operationHistory.RemoveAt(0);
                }
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

                        RecordOperation(new ImageOperationRecord
                        {
                            OperationType = isUpdate ? "Update" : "Add",
                            ImageId = key,
                            FileName = imageInfo.FileName ?? imageInfo.OriginalFileName,
                            Status = imageInfo.Status,
                            Timestamp = DateTime.Now
                        });

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

                    RecordOperation(new ImageOperationRecord
                    {
                        OperationType = "Remove",
                        ImageId = fileId,
                        FileName = imageInfo?.FileName ?? imageInfo?.OriginalFileName,
                        Status = imageInfo?.Status ?? ImageStatus.Deleted,
                        Timestamp = DateTime.Now
                    });

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

        /// <summary>
        /// 获取图片操作历史记录
        /// </summary>
        /// <returns>操作历史记录</returns>
        public List<ImageOperationRecord> GetOperationHistory()
        {
            lock (_lock)
            {
                return new List<ImageOperationRecord>(_operationHistory);
            }
        }

        /// <summary>
        /// 验证图片状态一致性
        /// </summary>
        /// <returns>验证结果</returns>
        public ImageStateValidationResult ValidateStateConsistency()
        {
            lock (_lock)
            {
                var result = new ImageStateValidationResult();

                foreach (var kvp in _images)
                {
                    var imageInfo = kvp.Value;

                    // 检查1: FileId为0但状态不是PendingUpload
                    if (imageInfo.FileId == 0 && imageInfo.Status != ImageStatus.PendingUpload)
                    {
                        result.InconsistentImages.Add(new ImageStateIssue
                        {
                            ImageId = kvp.Key,
                            Issue = "FileId为0但状态不是PendingUpload",
                            CurrentStatus = imageInfo.Status,
                            ExpectedStatus = ImageStatus.PendingUpload
                        });
                    }

                    // 检查2: FileId大于0但状态是PendingUpload
                    if (imageInfo.FileId > 0 && imageInfo.Status == ImageStatus.PendingUpload)
                    {
                        result.InconsistentImages.Add(new ImageStateIssue
                        {
                            ImageId = kvp.Key,
                            Issue = "FileId大于0但状态是PendingUpload",
                            CurrentStatus = imageInfo.Status,
                            ExpectedStatus = ImageStatus.Uploaded
                        });
                    }

                    // 检查3: 没有哈希值
                    if (string.IsNullOrEmpty(imageInfo.HashValue) && imageInfo.Status != ImageStatus.PendingUpload)
                    {
                        result.InconsistentImages.Add(new ImageStateIssue
                        {
                            ImageId = kvp.Key,
                            Issue = "没有哈希值但状态不是PendingUpload",
                            CurrentStatus = imageInfo.Status
                        });
                    }
                }

                result.IsValid = result.InconsistentImages.Count == 0;
                return result;
            }
        }

        /// <summary>
        /// 批量更新图片状态
        /// </summary>
        /// <param name="imageIds">图片ID列表</param>
        /// <param name="newStatus">新状态</param>
        /// <returns>成功更新的数量</returns>
        public int BatchUpdateImageStatus(List<long> imageIds, ImageStatus newStatus)
        {
            if (imageIds == null || imageIds.Count == 0)
                return 0;

            int updatedCount = 0;
            lock (_lock)
            {
                foreach (var imageId in imageIds)
                {
                    if (imageId > 0 && _images.ContainsKey(imageId))
                    {
                        var oldStatus = _images[imageId].Status;
                        _images[imageId].Status = newStatus;
                        _images[imageId].ModifiedAt = DateTime.Now;
                        updatedCount++;
                        Log($"批量更新图片状态: ID={imageId}, {oldStatus} -> {newStatus}");
                    }
                }
            }
            return updatedCount;
        }

        /// <summary>
        /// 获取指定业务ID的所有图片
        /// </summary>
        /// <param name="businessId">业务ID</param>
        /// <returns>图片信息列表</returns>
        public List<ImageInfo> GetImagesByBusinessId(long businessId)
        {
            lock (_lock)
            {
                return _images.Values.Where(img => img.BusinessId == businessId).ToList();
            }
        }

        /// <summary>
        /// 重置指定业务ID的所有图片状态
        /// </summary>
        /// <param name="businessId">业务ID</param>
        /// <returns>重置的图片数量</returns>
        public int ResetImagesByBusinessId(long businessId)
        {
            lock (_lock)
            {
                var images = _images.Values.Where(img => img.BusinessId == businessId).ToList();
                foreach (var image in images)
                {
                    if (image.Status == ImageStatus.PendingUpload || image.Status == ImageStatus.PendingDelete)
                    {
                        image.Status = ImageStatus.Normal;
                        image.Metadata.Clear();
                    }
                }
                return images.Count;
            }
        }

        /// <summary>
        /// 获取统计信息
        /// </summary>
        /// <returns>统计信息</returns>
        public ImageManagerStatistics GetStatistics()
        {
            lock (_lock)
            {
                return new ImageManagerStatistics
                {
                    TotalImages = _images.Count,
                    PendingUpload = PendingUploadCount,
                    PendingDelete = PendingDeleteCount,
                    Processing = ProcessingCount,
                    Uploaded = GetImageCountByStatus(ImageStatus.Uploaded),
                    Deleted = GetImageCountByStatus(ImageStatus.Deleted),
                    Normal = GetImageCountByStatus(ImageStatus.Normal)
                };
            }
        }
    }

    /// <summary>
    /// 图片操作记录
    /// </summary>
    public class ImageOperationRecord
    {
        /// <summary>
        /// 操作类型
        /// </summary>
        public string OperationType { get; set; }

        /// <summary>
        /// 图片ID
        /// </summary>
        public long ImageId { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public ImageStatus Status { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public DateTime Timestamp { get; set; }
    }

    /// <summary>
    /// 图片状态验证结果
    /// </summary>
    public class ImageStateValidationResult
    {
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// 不一致的图片列表
        /// </summary>
        public List<ImageStateIssue> InconsistentImages { get; set; } = new List<ImageStateIssue>();
    }

    /// <summary>
    /// 图片状态问题
    /// </summary>
    public class ImageStateIssue
    {
        /// <summary>
        /// 图片ID
        /// </summary>
        public long ImageId { get; set; }

        /// <summary>
        /// 问题描述
        /// </summary>
        public string Issue { get; set; }

        /// <summary>
        /// 当前状态
        /// </summary>
        public ImageStatus CurrentStatus { get; set; }

        /// <summary>
        /// 期望状态
        /// </summary>
        public ImageStatus ExpectedStatus { get; set; }
    }

    /// <summary>
    /// 图片管理器统计信息
    /// </summary>
    public class ImageManagerStatistics
    {
        /// <summary>
        /// 总图片数
        /// </summary>
        public int TotalImages { get; set; }

        /// <summary>
        /// 待上传数
        /// </summary>
        public int PendingUpload { get; set; }

        /// <summary>
        /// 待删除数
        /// </summary>
        public int PendingDelete { get; set; }

        /// <summary>
        /// 处理中数
        /// </summary>
        public int Processing { get; set; }

        /// <summary>
        /// 已上传数
        /// </summary>
        public int Uploaded { get; set; }

        /// <summary>
        /// 已删除数
        /// </summary>
        public int Deleted { get; set; }

        /// <summary>
        /// 正常数
        /// </summary>
        public int Normal { get; set; }
    }
}