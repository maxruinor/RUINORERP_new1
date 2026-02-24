using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using SourceGrid;
using RUINORERP.UI.BaseForm;
using RUINOR.WinFormsUI.CustomPictureBox;
using RUINORERP.Global;

namespace RUINORERP.UI.UCSourceGrid
{
    /// <summary>
    /// 图片状态枚举
    /// </summary>
    public enum ImageStatus
    {
        /// <summary>
        /// 正常状态
        /// </summary>
        Normal,
        /// <summary>
        /// 待删除状态
        /// </summary>
        PendingDelete,
        /// <summary>
        /// 待上传状态
        /// </summary>
        PendingUpload
    }

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
        /// 基础图片信息
        /// </summary>
        public ImageInfo BaseImageInfo { get; set; }

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
    }

    /// <summary>
    /// 图片状态管理器
    /// 用于管理图片的状态，支持正常、待删除、待上传状态
    /// </summary>
    public class ImageStateManager : IExcludeFromRegistration
    {
        /// <summary>
        /// 单例实例
        /// </summary>
        private static ImageStateManager _instance;

        /// <summary>
        /// 图片信息字典，key为图片ID
        /// </summary>
        private Dictionary<string, ExtendedImageInfo> _imageInfos;

        /// <summary>
        /// 单元格图片映射，key为单元格唯一标识，value为图片ID列表
        /// </summary>
        private Dictionary<string, List<string>> _cellImageMap;

        /// <summary>
        /// 锁对象
        /// </summary>
        private readonly object _lockObject = new object();

        /// <summary>
        /// 私有构造函数
        /// </summary>
        private ImageStateManager()
        {
            _imageInfos = new Dictionary<string, ExtendedImageInfo>();
            _cellImageMap = new Dictionary<string, List<string>>();
        }

        /// <summary>
        /// 获取单例实例
        /// </summary>
        public static ImageStateManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (typeof(ImageStateManager))
                    {
                        if (_instance == null)
                        {
                            _instance = new ImageStateManager();
                        }
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// 获取单元格的唯一标识
        /// </summary>
        /// <param name="cell">单元格</param>
        /// <returns>单元格唯一标识</returns>
        private string GetCellKey(SourceGrid.Cells.Cell cell)
        {
            if (cell == null)
                return string.Empty;

            // 使用单元格的行索引和列索引作为唯一标识
            var row = cell.Row.Index;
            var col = cell.Column.Index;
            return $"{cell.Grid.GetHashCode()}_{row}_{col}";
        }

        /// <summary>
        /// 添加图片信息
        /// </summary>
        /// <param name="cell">单元格</param>
        /// <param name="imageId">图片ID</param>
        /// <param name="fileName">文件名</param>
        /// <param name="imageData">图片数据</param>
        /// <param name="status">图片状态</param>
        public void AddImage(SourceGrid.Cells.Cell cell, string imageId, string fileName, byte[] imageData, ImageStatus status = ImageStatus.Normal)
        {
            lock (_lockObject)
            {
                // 创建基础图片信息
                var baseImageInfo = new ImageInfo
                {
                    OriginalFileName = fileName,
                    FileSize = imageData?.Length ?? 0,
                    CreateTime = DateTime.Now,
                    FileExtension = Path.GetExtension(fileName)?.TrimStart('.') ?? string.Empty
                };

                // 创建扩展图片信息
                var extendedImageInfo = new ExtendedImageInfo
                {
                    ImageId = imageId,
                    BaseImageInfo = baseImageInfo,
                    ImageData = imageData,
                    Status = status
                };

                // 添加到图片信息字典
                _imageInfos[imageId] = extendedImageInfo;

                // 更新单元格图片映射
                var cellKey = GetCellKey(cell);
                if (!_cellImageMap.ContainsKey(cellKey))
                {
                    _cellImageMap[cellKey] = new List<string>();
                }

                if (!_cellImageMap[cellKey].Contains(imageId))
                {
                    _cellImageMap[cellKey].Add(imageId);
                }

                // 生成预览图片
                if (imageData != null && imageData.Length > 0)
                {
                    try
                    {
                        using (var ms = new MemoryStream(imageData))
                        {
                            extendedImageInfo.PreviewImage = Image.FromStream(ms);
                        }
                    }
                    catch (Exception ex)
                    {
                        MainForm.Instance.PrintInfoLog($"生成图片预览失败: {ex.Message}");
                    }
                }
            }
        }

        /// <summary>
        /// 更新图片状态
        /// </summary>
        /// <param name="imageId">图片ID</param>
        /// <param name="status">新状态</param>
        public void UpdateImageStatus(string imageId, ImageStatus status)
        {
            lock (_lockObject)
            {
                if (_imageInfos.ContainsKey(imageId))
                {
                    _imageInfos[imageId].Status = status;
                }
            }
        }

        /// <summary>
        /// 获取图片信息
        /// </summary>
        /// <param name="imageId">图片ID</param>
        /// <returns>图片信息</returns>
        public ExtendedImageInfo GetImageInfo(string imageId)
        {
            lock (_lockObject)
            {
                if (_imageInfos.ContainsKey(imageId))
                {
                    return _imageInfos[imageId];
                }
                return null;
            }
        }

        /// <summary>
        /// 获取单元格的所有图片
        /// </summary>
        /// <param name="cell">单元格</param>
        /// <returns>图片信息列表</returns>
        public List<ExtendedImageInfo> GetCellImages(SourceGrid.Cells.Cell cell)
        {
            lock (_lockObject)
            {
                var cellKey = GetCellKey(cell);
                if (_cellImageMap.ContainsKey(cellKey))
                {
                    return _cellImageMap[cellKey]
                        .Where(imageId => _imageInfos.ContainsKey(imageId))
                        .Select(imageId => _imageInfos[imageId])
                        .ToList();
                }
                return new List<ExtendedImageInfo>();
            }
        }

        /// <summary>
        /// 获取所有待删除的图片
        /// </summary>
        /// <returns>待删除图片ID列表</returns>
        public List<string> GetPendingDeleteImages()
        {
            lock (_lockObject)
            {
                return _imageInfos
                    .Where(kv => kv.Value.Status == ImageStatus.PendingDelete)
                    .Select(kv => kv.Key)
                    .ToList();
            }
        }

        /// <summary>
        /// 获取所有待上传的图片
        /// </summary>
        /// <returns>待上传图片信息列表</returns>
        public List<ExtendedImageInfo> GetPendingUploadImages()
        {
            lock (_lockObject)
            {
                return _imageInfos
                    .Where(kv => kv.Value.Status == ImageStatus.PendingUpload)
                    .Select(kv => kv.Value)
                    .ToList();
            }
        }

        /// <summary>
        /// 清除图片状态
        /// </summary>
        public void ClearStatus()
        {
            lock (_lockObject)
            {
                // 清除所有待删除和待上传状态
                foreach (var imageInfo in _imageInfos.Values)
                {
                    if (imageInfo.Status == ImageStatus.PendingDelete || imageInfo.Status == ImageStatus.PendingUpload)
                    {
                        imageInfo.Status = ImageStatus.Normal;
                    }
                }
            }
        }

        /// <summary>
        /// 重置图片状态
        /// </summary>
        public void ResetStatus()
        {
            lock (_lockObject)
            {
                // 移除所有待删除和待上传的图片
                var pendingImages = _imageInfos
                    .Where(kv => kv.Value.Status == ImageStatus.PendingDelete || kv.Value.Status == ImageStatus.PendingUpload)
                    .Select(kv => kv.Key)
                    .ToList();

                foreach (var imageId in pendingImages)
                {
                    _imageInfos.Remove(imageId);
                }

                // 清理单元格图片映射
                foreach (var cellKey in _cellImageMap.Keys.ToList())
                {
                    var remainingImages = _cellImageMap[cellKey]
                        .Where(imageId => _imageInfos.ContainsKey(imageId))
                        .ToList();

                    if (remainingImages.Count == 0)
                    {
                        _cellImageMap.Remove(cellKey);
                    }
                    else
                    {
                        _cellImageMap[cellKey] = remainingImages;
                    }
                }
            }
        }

        /// <summary>
        /// 清理资源
        /// </summary>
        public void Dispose()
        {
            lock (_lockObject)
            {
                // 释放预览图片资源
                foreach (var imageInfo in _imageInfos.Values)
                {
                    if (imageInfo.PreviewImage != null)
                    {
                        imageInfo.PreviewImage.Dispose();
                        imageInfo.PreviewImage = null;
                    }
                }

                _imageInfos.Clear();
                _cellImageMap.Clear();
            }
        }
    }
}
