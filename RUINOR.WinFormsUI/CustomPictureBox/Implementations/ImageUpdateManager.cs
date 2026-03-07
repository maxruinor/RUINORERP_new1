using System;
using System.Collections.Generic;
using System.Drawing;
using RUINOR.WinFormsUI.CustomPictureBox;
using RUINORERP.Common.BusinessImage; // 用于访问ImageInfo类

namespace RUINOR.WinFormsUI.CustomPictureBox.Implementations
{
    /// <summary>
    /// 图片更新管理器
    /// 用于跟踪和管理图片的更新状态
    /// </summary>
    public class ImageUpdateManager
    {
        /// <summary>
        /// 图片哈希计算器
        /// </summary>
        private readonly SHA256HashCalculator _hashCalculator;

        /// <summary>
        /// 图片处理器
        /// </summary>
        private readonly ImageProcessor _imageProcessor;

        /// <summary>
        /// 需要更新的图片集合（以哈希值为键）
        /// </summary>
        private readonly Dictionary<string, ImageInfo> _imagesNeedingUpdate;

        /// <summary>
        /// 线程锁对象
        /// </summary>
        private readonly object _lockObject = new object();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="hashCalculator">图片哈希计算器</param>
        /// <param name="imageProcessor">图片处理器</param>
        /// <exception cref="ArgumentNullException">当依赖项为null时抛出</exception>
        public ImageUpdateManager(SHA256HashCalculator hashCalculator, ImageProcessor imageProcessor)
        {
            _hashCalculator = hashCalculator ?? throw new ArgumentNullException(nameof(hashCalculator));
            _imageProcessor = imageProcessor ?? throw new ArgumentNullException(nameof(imageProcessor));
            _imagesNeedingUpdate = new Dictionary<string, ImageInfo>();
        }

        /// <summary>
        /// 标记图片需要更新
        /// </summary>
        /// <param name="imageInfo">图片信息</param>
        /// <exception cref="ArgumentNullException">当imageInfo为null时抛出</exception>
        public void MarkImageAsUpdated(ImageInfo imageInfo)
        {
            if (imageInfo == null)
                throw new ArgumentNullException(nameof(imageInfo), "图片信息不能为空");

            lock (_lockObject)
            {
                // 无论是否有哈希值，都标记为需要更新
                imageInfo.Metadata["UpdateMarker"] = ImageProcessingConstants.UpdateMarker;
                imageInfo.Status = RUINORERP.Common.BusinessImage.ImageStatus.PendingUpload;
                imageInfo.ModifiedAt = DateTime.Now;

                // 如果有哈希值，也添加到需要更新的集合中
                if (!string.IsNullOrEmpty(imageInfo.HashValue))
                {
                    _imagesNeedingUpdate[imageInfo.HashValue] = imageInfo;
                }
            }
        }

        /// <summary>
        /// 检查图片是否需要更新
        /// </summary>
        /// <param name="imageInfo">图片信息</param>
        /// <returns>是否需要更新</returns>
        /// <exception cref="ArgumentNullException">当imageInfo为null时抛出</exception>
        public bool IsImageNeedingUpdate(ImageInfo imageInfo)
        {
            if (imageInfo == null)
                throw new ArgumentNullException(nameof(imageInfo), "图片信息不能为空");

            // 有以下情况之一则认为需要更新：
            // 1. 有更新标记
            // 2. 有哈希值且在需要更新的集合中
            // 3. 没有FileId（新添加的图片）
            // 4. 没有哈希值（新添加的图片）
            // 5. Status为PendingUpload（需要上传的图片）

            bool hasUpdateMarker;
            bool isInUpdateSet;
            bool isNewImage;
            bool isPendingUpload;

            lock (_lockObject)
            {
                hasUpdateMarker = HasUpdateMarker(imageInfo);
                isInUpdateSet = (!string.IsNullOrEmpty(imageInfo.HashValue) && _imagesNeedingUpdate.ContainsKey(imageInfo.HashValue));
                isNewImage = IsNewImage(imageInfo);
            }

            isPendingUpload = imageInfo.Status == RUINORERP.Common.BusinessImage.ImageStatus.PendingUpload;

            return hasUpdateMarker || isInUpdateSet || isNewImage || isPendingUpload;
        }

        /// <summary>
        /// 检查图片是否有更新标记
        /// </summary>
        /// <param name="imageInfo">图片信息</param>
        /// <returns>是否有更新标记</returns>
        private bool HasUpdateMarker(ImageInfo imageInfo)
        {
            return imageInfo.Metadata.TryGetValue("UpdateMarker", out var marker) && marker == ImageProcessingConstants.UpdateMarker;
        }

        /// <summary>
        /// 检查图片是否为新图片
        /// </summary>
        /// <param name="imageInfo">图片信息</param>
        /// <returns>是否为新图片</returns>
        private bool IsNewImage(ImageInfo imageInfo)
        {
            return imageInfo.FileId == 0 || string.IsNullOrEmpty(imageInfo.HashValue);
        }

        /// <summary>
        /// 获取需要更新的图片列表
        /// </summary>
        /// <returns>需要更新的图片信息列表</returns>
        public List<ImageInfo> GetImagesNeedingUpdate()
        {
            lock (_lockObject)
            {
                return new List<ImageInfo>(_imagesNeedingUpdate.Values);
            }
        }

        /// <summary>
        /// 重置图片的更新状态
        /// </summary>
        /// <param name="imageInfo">图片信息</param>
        /// <exception cref="ArgumentNullException">当imageInfo为null时抛出</exception>
        public void ResetImageUpdateStatus(ImageInfo imageInfo)
        {
            if (imageInfo == null)
                throw new ArgumentNullException(nameof(imageInfo), "图片信息不能为空");

            lock (_lockObject)
            {
                imageInfo.Metadata.Clear();
                if (!string.IsNullOrEmpty(imageInfo.HashValue))
                {
                    _imagesNeedingUpdate.Remove(imageInfo.HashValue);
                }
            }
        }

        /// <summary>
        /// 重置所有图片的更新状态
        /// </summary>
        public void ResetAllImageUpdateStatuses()
        {
            lock (_lockObject)
            {
                foreach (var imageInfo in _imagesNeedingUpdate.Values)
                {
                    imageInfo.Metadata.Clear();
                }
                _imagesNeedingUpdate.Clear();
            }
        }

        /// <summary>
        /// 检查图片内容是否已更改
        /// </summary>
        /// <param name="imageInfo">图片信息</param>
        /// <param name="newImage">新的图片对象</param>
        /// <returns>图片内容是否已更改</returns>
        /// <exception cref="ArgumentNullException">当参数为null时抛出</exception>
        public bool HasImageContentChanged(ImageInfo imageInfo, Image newImage)
        {
            if (imageInfo == null)
                throw new ArgumentNullException(nameof(imageInfo), "图片信息不能为空");

            if (newImage == null)
                throw new ArgumentNullException(nameof(newImage), "图片对象不能为空");

            // 如果图片信息没有哈希值，则认为内容已更改
            if (string.IsNullOrEmpty(imageInfo.HashValue))
                return true;

            // 计算新图片的哈希值
            string newHash = _hashCalculator.CalculateHash(newImage);

            // 比较哈希值
            return !string.Equals(imageInfo.HashValue, newHash, StringComparison.Ordinal);
        }
    }
}