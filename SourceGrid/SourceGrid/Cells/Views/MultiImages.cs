using System;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading.Tasks;
using DevAge.Drawing;
using DevAge.Drawing.VisualElements;
using RUINORERP.Common.BusinessImage;

namespace SourceGrid.Cells.Views
{
    /// <summary>
    /// 多图片单元格视图
    /// 用于显示多张图片
    /// </summary>
    [Serializable]
    public class MultiImagesView : ImageCellBase, IImageCellView
    {
        #region Constructors

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public MultiImagesView() : base()
        {
            _images = new VisualElementList();
        }

        #endregion

        #region Private Fields

        /// <summary>
        /// 图片列表
        /// </summary>
        private readonly VisualElementList _images;

        /// <summary>
        /// 当前显示的图片索引
        /// </summary>
        private int _currentIndex = 0;

        /// <summary>
        /// 图片ID列表
        /// </summary>
        private readonly List<long> _imageIds = new List<long>();

        #endregion

        #region Public Properties

        /// <summary>
        /// 图片子元素列表
        /// </summary>
        public VisualElementList SubImages
        {
            get { return _images; }
        }

        /// <summary>
        /// 当前显示的图片索引
        /// </summary>
        public int CurrentIndex
        {
            get { return _currentIndex; }
            set
            {
                if (value >= 0 && value < _imageIds.Count)
                {
                    _currentIndex = value;
                    UpdateDisplayImage();
                }
            }
        }

        /// <summary>
        /// 图片数量
        /// </summary>
        public int ImageCount
        {
            get { return _imageIds.Count; }
        }

        #endregion

        #region IImageCellView Implementation

        /// <summary>
        /// 加载图片（多图片模式下添加图片到列表）
        /// </summary>
        /// <param name="fileId">文件ID</param>
        public void LoadImage(long fileId)
        {
            if (!_imageIds.Contains(fileId))
            {
                _imageIds.Add(fileId);
                _currentFileId = fileId;
                CurrentFileId = fileId;
                UpdateDisplayImage();
            }
        }

        /// <summary>
        /// 异步加载图片
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <returns>加载任务</returns>
        public Task LoadImageAsync(long fileId)
        {
            LoadImage(fileId);
            return Task.CompletedTask;
        }

        /// <summary>
        /// 清空所有图片
        /// </summary>
        public void ClearImage()
        {
            _images.Clear();
            _imageIds.Clear();
            _currentIndex = 0;
            GridImage = null;
            _currentImageHash = string.Empty;
        }

        /// <summary>
        /// 获取当前图片ID
        /// </summary>
        /// <returns>图片ID</returns>
        public long GetImageId()
        {
            return _currentIndex < _imageIds.Count ? _imageIds[_currentIndex] : 0;
        }

        /// <summary>
        /// 获取当前图片信息
        /// </summary>
        /// <returns>图片信息</returns>
        public ImageInfo GetImageInfo()
        {
            long currentImageId = GetImageId();
            return ImageStateManager.Instance.GetImageInfo(currentImageId);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 添加图片
        /// </summary>
        /// <param name="image">图片对象</param>
        public void AddImage(Image image)
        {
            if (image != null)
            {
                var visualImage = new VisualImage(image);
                _images.Add(visualImage);
            }
        }

        /// <summary>
        /// 移除指定索引的图片
        /// </summary>
        /// <param name="index">图片索引</param>
        public void RemoveImageAt(int index)
        {
            if (index >= 0 && index < _images.Count)
            {
                _images.RemoveAt(index);
                if (_imageIds.Count > index)
                {
                    _imageIds.RemoveAt(index);
                }

                // 调整当前索引
                if (_currentIndex >= _images.Count)
                {
                    _currentIndex = Math.Max(0, _images.Count - 1);
                }

                UpdateDisplayImage();
            }
        }

        /// <summary>
        /// 移除所有图片
        /// </summary>
        public void RemoveAllImages()
        {
            ClearImage();
        }

        /// <summary>
        /// 显示上一张图片
        /// </summary>
        public void ShowPreviousImage()
        {
            if (_currentIndex > 0)
            {
                _currentIndex--;
                UpdateDisplayImage();
            }
        }

        /// <summary>
        /// 显示下一张图片
        /// </summary>
        public void ShowNextImage()
        {
            if (_currentIndex < _imageIds.Count - 1)
            {
                _currentIndex++;
                UpdateDisplayImage();
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// 更新显示的图片
        /// </summary>
        protected virtual void UpdateDisplayImage()
        {
            if (_currentIndex < _images.Count && _images[_currentIndex] is VisualImage visualImage)
            {
                GridImage = visualImage.Value;
            }
            else
            {
                GridImage = null;
            }
        }

        /// <summary>
        /// 处理图片数据
        /// </summary>
        /// <param name="context">单元格上下文</param>
        protected override void ProcessImageData(CellContext context)
        {
            // 多图片模式下的特殊处理
            if (context.Value is long[] fileIds)
            {
                // 处理文件ID数组
                foreach (var fileId in fileIds)
                {
                    LoadImage(fileId);
                }
            }
            else
            {
                base.ProcessImageData(context);
            }
        }

        #endregion

        #region Override Methods

        /// <summary>
        /// 获取视觉元素
        /// </summary>
        protected override IEnumerable<IVisualElement> GetElements()
        {
            foreach (var element in base.GetElements())
            {
                yield return element;
            }

            foreach (var image in _images)
            {
                yield return image;
            }
        }

        /// <summary>
        /// 克隆当前对象
        /// </summary>
        public override object Clone()
        {
            var clone = new MultiImagesView();
            foreach (var imageId in _imageIds)
            {
                clone.LoadImage(imageId);
            }
            clone._currentIndex = _currentIndex;
            return clone;
        }

        /// <summary>
        /// 刷新视图
        /// </summary>
        public override void Refresh(CellContext context)
        {
            UpdateDisplayImage();
        }

        #endregion
    }

    /// <summary>
    /// MultiImages的兼容性类
    /// 保留旧类名以保持向后兼容
    /// </summary>
    [Serializable]
    [Obsolete("请使用MultiImagesView替代MultiImages")]
    public class MultiImages : MultiImagesView
    {
        #region Constructors

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public MultiImages() : base()
        {
        }

        /// <summary>
        /// 复制构造函数
        /// </summary>
        /// <param name="other">源对象</param>
        public MultiImages(MultiImages other) : base()
        {
            // 复制图片列表
            foreach (var visualElement in other.SubImages)
            {
                SubImages.Add(visualElement.Clone() as IVisualElement);
            }
        }

        #endregion

        #region Clone

        /// <summary>
        /// 克隆当前对象
        /// </summary>
        /// <returns>克隆的对象</returns>
        public override object Clone()
        {
            return new MultiImages(this);
        }

        #endregion
    }
}
