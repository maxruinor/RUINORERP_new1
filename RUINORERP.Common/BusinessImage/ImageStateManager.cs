using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace RUINORERP.Common.BusinessImage
{
    /// <summary>
    /// 图片状态管理器
    /// 统一管理图片的状态和上传队列
    /// </summary>
    public class ImageStateManager
    {
        #region 单例模式

        private static ImageStateManager _instance;
        private static readonly object _lockObject = new object();

        /// <summary>
        /// 获取图片状态管理器实例
        /// </summary>
        public static ImageStateManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockObject)
                    {
                        if (_instance == null)
                            _instance = new ImageStateManager();
                    }
                }
                return _instance;
            }
        }

        private ImageStateManager()
        {
            _imageInfoDict = new ConcurrentDictionary<long, ImageInfo>();
            _cellToImageDict = new ConcurrentDictionary<object, List<long>>();
        }

        #endregion

        #region 私有字段

        private readonly ConcurrentDictionary<long, ImageInfo> _imageInfoDict;
        private readonly ConcurrentDictionary<object, List<long>> _cellToImageDict;

        #endregion

        #region 公共方法

        /// <summary>
        /// 添加图片信息
        /// </summary>
        /// <param name="cell">单元格</param>
        /// <param name="imageId">图片ID</param>
        /// <param name="fileName">文件名</param>
        /// <param name="imageData">图片数据</param>
        /// <param name="status">图片状态</param>
        public void AddImage(object cell, long imageId, string fileName, byte[] imageData, ImageStatus status = ImageStatus.Normal, long businessId = 0, string storagePath = null)
        {
            lock (_lockObject)
            {
                // 创建图片信息
                var imageInfo = new ImageInfo
                {
                    ImageId = imageId,
                    BusinessId = businessId,
                    FileName = fileName,
                    OriginalFileName = fileName,
                    ImageData = imageData,
                    Status = status,
                    StoragePath = storagePath,
                    Cell = cell,
                    CreateTime = DateTime.Now
                };

                // 添加到图片字典
                _imageInfoDict.AddOrUpdate(imageId, imageInfo, (key, existing) => imageInfo);

                // 建立单元格到图片的映射
                if (cell != null && !_cellToImageDict.ContainsKey(cell))
                {
                    _cellToImageDict[cell] = new List<long>();
                }

                if (cell != null && !_cellToImageDict[cell].Contains(imageId))
                {
                    _cellToImageDict[cell].Add(imageId);
                }
            }
        }

        /// <summary>
        /// 获取图片信息
        /// </summary>
        /// <param name="imageId">图片ID</param>
        /// <returns>图片信息</returns>
        public ImageInfo GetImageInfo(long imageId)
        {
            _imageInfoDict.TryGetValue(imageId, out var imageInfo);
            return imageInfo;
        }

        /// <summary>
        /// 获取单元格关联的所有图片
        /// </summary>
        /// <param name="cell">单元格</param>
        /// <returns>图片列表</returns>
        public List<ImageInfo> GetImagesByCell(object cell)
        {
            var result = new List<ImageInfo>();

            if (cell != null && _cellToImageDict.TryGetValue(cell, out var imageIds))
            {
                foreach (var imageId in imageIds)
                {
                    if (_imageInfoDict.TryGetValue(imageId, out var imageInfo))
                    {
                        result.Add(imageInfo);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 更新图片状态
        /// </summary>
        /// <param name="imageId">图片ID</param>
        /// <param name="newStatus">新状态</param>
        public void UpdateImageStatus(long imageId, ImageStatus newStatus)
        {
            lock (_lockObject)
            {
                if (_imageInfoDict.TryGetValue(imageId, out var imageInfo))
                {
                    imageInfo.Status = newStatus;
                }
            }
        }

        /// <summary>
        /// 删除图片信息
        /// </summary>
        /// <param name="imageId">图片ID</param>
        public void RemoveImage(long imageId)
        {
            lock (_lockObject)
            {
                if (_imageInfoDict.TryRemove(imageId, out var imageInfo))
                {
                    // 从单元格映射中移除
                    if (imageInfo.Cell != null && _cellToImageDict.TryGetValue(imageInfo.Cell, out var imageIds))
                    {
                        imageIds.Remove(imageId);
                        if (imageIds.Count == 0)
                        {
                            _cellToImageDict.TryRemove(imageInfo.Cell, out _);
                        }
                    }

                    // 释放图片资源
                    if (imageInfo.PreviewImage != null)
                    {
                        imageInfo.PreviewImage.Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// 获取所有待上传的图片
        /// </summary>
        /// <returns>待上传图片列表</returns>
        public List<ImageInfo> GetPendingUploadImages()
        {
            return _imageInfoDict.Values
                .Where(x => x.Status == ImageStatus.PendingUpload)
                .OrderBy(x => x.CreateTime)
                .ToList();
        }

        /// <summary>
        /// 获取所有待删除的图片
        /// </summary>
        /// <returns>待删除图片列表</returns>
        public List<ImageInfo> GetPendingDeleteImages()
        {
            return _imageInfoDict.Values
                .Where(x => x.Status == ImageStatus.PendingDelete)
                .OrderBy(x => x.CreateTime)
                .ToList();
        }

        /// <summary>
        /// 获取指定单元格的待删除图片ID列表
        /// </summary>
        /// <param name="cell">单元格</param>
        /// <returns>待删除图片ID列表</returns>
        public List<long> GetPendingDeleteImagesByCell(object cell)
        {
            var result = new List<long>();

            if (cell != null && _cellToImageDict.TryGetValue(cell, out var imageIds))
            {
                foreach (var imageId in imageIds)
                {
                    if (_imageInfoDict.TryGetValue(imageId, out var imageInfo) &&
                        imageInfo.Status == ImageStatus.PendingDelete)
                    {
                        result.Add(imageId);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 获取指定单元格的待上传图片
        /// </summary>
        /// <param name="cell">单元格</param>
        /// <returns>待上传图片信息列表</returns>
        public List<ImageInfo> GetPendingUploadImagesByCell(object cell)
        {
            var result = new List<ImageInfo>();

            if (cell != null && _cellToImageDict.TryGetValue(cell, out var imageIds))
            {
                foreach (var imageId in imageIds)
                {
                    if (_imageInfoDict.TryGetValue(imageId, out var imageInfo) &&
                        imageInfo.Status == ImageStatus.PendingUpload)
                    {
                        result.Add(imageInfo);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 获取所有待处理图片的统计信息
        /// </summary>
        /// <returns>统计信息（待删除数量、待上传数量等）</returns>
        public Dictionary<string, int> GetPendingImageStats()
        {
            var stats = new Dictionary<string, int>
            {
                { "PendingDelete", _imageInfoDict.Count(kv => kv.Value.Status == ImageStatus.PendingDelete) },
                { "PendingUpload", _imageInfoDict.Count(kv => kv.Value.Status == ImageStatus.PendingUpload) },
                { "Normal", _imageInfoDict.Count(kv => kv.Value.Status == ImageStatus.Normal) },
                { "Total", _imageInfoDict.Count }
            };
            return stats;
        }

        /// <summary>
        /// 清理资源
        /// </summary>
        public void Dispose()
        {
            lock (_lockObject)
            {
                // 释放预览图片资源
                foreach (var imageInfo in _imageInfoDict.Values)
                {
                    if (imageInfo.PreviewImage != null)
                    {
                        imageInfo.PreviewImage.Dispose();
                        imageInfo.PreviewImage = null;
                    }
                }

                _imageInfoDict.Clear();
                _cellToImageDict.Clear();
            }
        }

        /// <summary>
        /// 清空所有图片状态
        /// </summary>
        public void ClearAll()
        {
            lock (_lockObject)
            {
                // 释放预览图片资源
                foreach (var imageInfo in _imageInfoDict.Values)
                {
                    if (imageInfo.PreviewImage != null)
                    {
                        imageInfo.PreviewImage.Dispose();
                    }
                }

                _imageInfoDict.Clear();
                _cellToImageDict.Clear();
            }
        }

        /// <summary>
        /// 批量删除已处理的图片
        /// </summary>
        /// <param name="processedImageIds">已处理的图片ID列表</param>
        public void RemoveProcessedImages(List<long> processedImageIds)
        {
            if (processedImageIds == null || processedImageIds.Count == 0)
                return;

            lock (_lockObject)
            {
                foreach (var imageId in processedImageIds)
                {
                    RemoveImage(imageId);
                }
            }
        }

        #endregion
    }
}
