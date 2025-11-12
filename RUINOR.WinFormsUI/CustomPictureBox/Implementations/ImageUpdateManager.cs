using System;
using System.Collections.Generic;
using System.Drawing;
using RUINOR.WinFormsUI.CustomPictureBox; // 用于访问ImageInfo类

namespace RUINOR.WinFormsUI.CustomPictureBox.Implementations
{
    /// <summary>
    /// 图片更新管理器
    /// 用于跟踪和管理图片的更新状态
    /// </summary>
    public class ImageUpdateManager
    {
        // 更新标记常量
        private const string UPDATE_MARKER = "NEEDS_UPDATE";
        // 图片哈希计算器
        private readonly SHA256HashCalculator _hashCalculator;
        // 图片处理器
        private readonly ImageProcessor _imageProcessor;
        // 需要更新的图片集合
        private readonly Dictionary<string, ImageInfo> _imagesNeedingUpdate;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="hashCalculator">图片哈希计算器</param>
        /// <param name="imageProcessor">图片处理器</param>
        public ImageUpdateManager(SHA256HashCalculator hashCalculator, ImageProcessor imageProcessor)
        {
            _hashCalculator = hashCalculator ?? throw new System.ArgumentNullException(nameof(hashCalculator));
            _imageProcessor = imageProcessor ?? throw new System.ArgumentNullException(nameof(imageProcessor));
            _imagesNeedingUpdate = new Dictionary<string, ImageInfo>();
        }

        /// <summary>
        /// 标记图片需要更新
        /// </summary>
        /// <param name="imageInfo">图片信息</param>
        public void MarkImageAsUpdated(ImageInfo imageInfo)
        {
            if (imageInfo == null)
                throw new System.ArgumentNullException(nameof(imageInfo), "图片信息不能为空");

            // 无论是否有哈希值，都标记为需要更新
            imageInfo.Metadata["UpdateMarker"] = UPDATE_MARKER;
            imageInfo.IsUpdated = true;
            imageInfo.ModifiedAt = DateTime.Now;
            
            // 如果有哈希值，也添加到需要更新的集合中
            if (!string.IsNullOrEmpty(imageInfo.HashValue))
            {
                _imagesNeedingUpdate[imageInfo.HashValue] = imageInfo;
            }
        }

        /// <summary>
        /// 检查图片是否需要更新
        /// </summary>
        /// <param name="imageInfo">图片信息</param>
        /// <returns>是否需要更新</returns>
        public bool IsImageNeedingUpdate(ImageInfo imageInfo)
        {
            if (imageInfo == null)
                throw new System.ArgumentNullException(nameof(imageInfo), "图片信息不能为空");

            // 有以下情况之一则认为需要更新：
            // 1. 有更新标记
            // 2. 有哈希值且在需要更新的集合中
            // 3. 没有FileId（新添加的图片）
            // 4. 没有哈希值（新添加的图片）
            // 5. IsUpdated标志为true
            return imageInfo.Metadata.ContainsKey("UpdateMarker") && imageInfo.Metadata["UpdateMarker"] == UPDATE_MARKER || 
                   (!string.IsNullOrEmpty(imageInfo.HashValue) && _imagesNeedingUpdate.ContainsKey(imageInfo.HashValue)) ||
                  imageInfo.FileId==0 || 
                   string.IsNullOrEmpty(imageInfo.HashValue) ||
                   imageInfo.IsUpdated;
        }

        /// <summary>
        /// 获取需要更新的图片列表
        /// </summary>
        /// <returns>需要更新的图片信息列表</returns>
        public List<ImageInfo> GetImagesNeedingUpdate()
        {
            return new List<ImageInfo>(_imagesNeedingUpdate.Values);
        }

        /// <summary>
        /// 重置图片的更新状态
        /// </summary>
        /// <param name="imageInfo">图片信息</param>
        public void ResetImageUpdateStatus(ImageInfo imageInfo)
        {
            if (imageInfo == null)
                throw new System.ArgumentNullException(nameof(imageInfo), "图片信息不能为空");

            imageInfo.Metadata.Clear();
            if (!string.IsNullOrEmpty(imageInfo.HashValue))
            {
                _imagesNeedingUpdate.Remove(imageInfo.HashValue);
            }
        }

        /// <summary>
        /// 重置所有图片的更新状态
        /// </summary>
        public void ResetAllImageUpdateStatuses()
        {
            foreach (var imageInfo in _imagesNeedingUpdate.Values)
            {
                imageInfo.Metadata.Clear();
            }
            _imagesNeedingUpdate.Clear();
        }

        /// <summary>
        /// 检查图片内容是否已更改
        /// </summary>
        /// <param name="imageInfo">图片信息</param>
        /// <param name="newImage">新的图片对象</param>
        /// <returns>图片内容是否已更改</returns>
        public bool HasImageContentChanged(ImageInfo imageInfo, Image newImage)
        {
            if (imageInfo == null)
                throw new System.ArgumentNullException(nameof(imageInfo), "图片信息不能为空");

            if (newImage == null)
                throw new System.ArgumentNullException(nameof(newImage), "图片对象不能为空");

            // 如果图片信息没有哈希值，则认为内容已更改
            if (string.IsNullOrEmpty(imageInfo.HashValue))
                return true;

            // 计算新图片的哈希值
            string newHash = _hashCalculator.CalculateHash(newImage);

            // 比较哈希值
            return !imageInfo.HashValue.Equals(newHash);
        }
    }
}