using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using SourceGrid.Cells;
using SourceGrid.Cells.Editors;

namespace SourceGrid
{
    /// <summary>
    /// 扩展的图片信息类，添加状态管理功能
    /// </summary>
    public class ExtendedImageInfo
    {
        /// <summary>
        /// 图片ID
        /// </summary>
        public string ImageId { get; set; }

        /// <summary>
        /// 图片文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 图片字节数据
        /// </summary>
        public byte[] ImageData { get; set; }

        /// <summary>
        /// 图片状态
        /// </summary>
        public ImageStatus Status { get; set; }

        /// <summary>
        /// 图片预览
        /// </summary>
        public Image PreviewImage { get; set; }

        /// <summary>
        /// 单元格引用
        /// </summary>
        public Cell Cell { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }

    /// <summary>
    /// 图片状态管理器
    /// 统一管理表格控件中图片的状态和上传队列
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
            _imageInfoDict = new ConcurrentDictionary<string, ExtendedImageInfo>();
            _cellToImageDict = new ConcurrentDictionary<Cell, List<string>>();
        }

        #endregion

        #region 私有字段

        private readonly ConcurrentDictionary<string, ExtendedImageInfo> _imageInfoDict;
        private readonly ConcurrentDictionary<Cell, List<string>> _cellToImageDict;

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
        public void AddImage(Cell cell, string imageId, string fileName, byte[] imageData, ImageStatus status = ImageStatus.Normal)
        {
            lock (_lockObject)
            {
                // 创建扩展图片信息
                var extendedInfo = new ExtendedImageInfo
                {
                    ImageId = imageId,
                    FileName = fileName,
                    ImageData = imageData,
                    Status = status,
                    Cell = cell,
                    CreateTime = DateTime.Now
                };

                // 添加到图片字典
                _imageInfoDict.AddOrUpdate(imageId, extendedInfo, (key, existing) => extendedInfo);

                // 建立单元格到图片的映射
                if (!_cellToImageDict.ContainsKey(cell))
                {
                    _cellToImageDict[cell] = new List<string>();
                }
                
                if (!_cellToImageDict[cell].Contains(imageId))
                {
                    _cellToImageDict[cell].Add(imageId);
                }

                // 触发状态变更事件
                OnImageStateChanged?.Invoke(this, new ImageStateChangedEventArgs
                {
                    Cell = cell,
                    ImageId = imageId,
                    FileName = fileName,
                    ImageData = imageData,
                    Status = status
                });
            }
        }

        /// <summary>
        /// 获取图片信息
        /// </summary>
        /// <param name="imageId">图片ID</param>
        /// <returns>图片信息</returns>
        public ExtendedImageInfo GetImageInfo(string imageId)
        {
            _imageInfoDict.TryGetValue(imageId, out var imageInfo);
            return imageInfo;
        }

        /// <summary>
        /// 获取单元格关联的所有图片
        /// </summary>
        /// <param name="cell">单元格</param>
        /// <returns>图片列表</returns>
        public List<ExtendedImageInfo> GetImagesByCell(Cell cell)
        {
            var result = new List<ExtendedImageInfo>();
            
            if (_cellToImageDict.TryGetValue(cell, out var imageIds))
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
        public void UpdateImageStatus(string imageId, ImageStatus newStatus)
        {
            lock (_lockObject)
            {
                if (_imageInfoDict.TryGetValue(imageId, out var imageInfo))
                {
                    imageInfo.Status = newStatus;
                    
                    // 触发状态变更事件
                    OnImageStateChanged?.Invoke(this, new ImageStateChangedEventArgs
                    {
                        Cell = imageInfo.Cell,
                        ImageId = imageId,
                        FileName = imageInfo.FileName,
                        ImageData = imageInfo.ImageData,
                        Status = newStatus
                    });
                }
            }
        }

        /// <summary>
        /// 删除图片信息
        /// </summary>
        /// <param name="imageId">图片ID</param>
        public void RemoveImage(string imageId)
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

                    // 触发删除事件
                    OnImageStateChanged?.Invoke(this, new ImageStateChangedEventArgs
                    {
                        Cell = imageInfo.Cell,
                        ImageId = imageId,
                        FileName = imageInfo.FileName,
                        ImageData = imageInfo.ImageData,
                        Status = ImageStatus.PendingDelete
                    });
                }
            }
        }

        /// <summary>
        /// 获取所有待上传的图片
        /// </summary>
        /// <returns>待上传图片列表</returns>
        public List<ExtendedImageInfo> GetPendingUploadImages()
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
        public List<ExtendedImageInfo> GetPendingDeleteImages()
        {
            return _imageInfoDict.Values
                .Where(x => x.Status == ImageStatus.PendingDelete)
                .OrderBy(x => x.CreateTime)
                .ToList();
        }

        /// <summary>
        /// 获取所有待删除的图片ID列表
        /// </summary>
        /// <returns>待删除图片ID列表</returns>
        public List<string> GetPendingDeleteImageIds()
        {
            return _imageInfoDict.Values
                .Where(x => x.Status == ImageStatus.PendingDelete)
                .Select(x => x.ImageId)
                .OrderBy(x => x)
                .ToList();
        }

 
        /// <summary>
        /// 获取指定单元格的待删除图片ID列表
        /// </summary>
        /// <param name="cell">单元格</param>
        /// <returns>待删除图片ID列表</returns>
        public List<string> GetPendingDeleteImagesByCell(Cell cell)
        {
            var result = new List<string>();
            
            if (_cellToImageDict.TryGetValue(cell, out var imageIds))
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
        public List<ExtendedImageInfo> GetPendingUploadImagesByCell(Cell cell)
        {
            var result = new List<ExtendedImageInfo>();
            
            if (_cellToImageDict.TryGetValue(cell, out var imageIds))
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
                _imageInfoDict.Clear();
                _cellToImageDict.Clear();
            }
        }

        /// <summary>
        /// 批量删除已处理的图片
        /// </summary>
        /// <param name="processedImageIds">已处理的图片ID列表</param>
        public void RemoveProcessedImages(List<string> processedImageIds)
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