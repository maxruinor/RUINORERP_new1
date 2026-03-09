using DevAge.Drawing;
using DevAge.Drawing.VisualElements;
using RUINORERP.Common.BusinessImage;
using SourceGrid.Cells.Editors;
using SourceGrid.Cells.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SourceGrid.Cells.Views
{
    /// <summary>
    /// 远程图片单元格视图
    /// 支持从服务器下载并显示图片
    /// </summary>
    [Serializable]
    public class RemoteImageView : ImageCellBase, IImageCellView
    {
        #region 构造函数

        /// <summary>
        /// 使用默认设置
        /// </summary>
        public RemoteImageView() : base()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cache">图片缓存</param>
        public RemoteImageView(ImageCache cache)
            : base(cache)
        {
        }

        /// <summary>
        /// 使用图片对象构造
        /// </summary>
        /// <param name="image">图片对象</param>
        public RemoteImageView(Image image) : base(image)
        {
        }

        #endregion

        #region 视觉元素

        /// <summary>
        /// 视觉元素
        /// </summary>
        private readonly IVisualElement _visualElement = new VisualImage();

        /// <summary>
        /// 视觉元素
        /// </summary>
        public IVisualElement VisualElement
        {
            get { return _visualElement; }
        }

        /// <summary>
        /// 获取元素列表
        /// </summary>
        protected override IEnumerable<IVisualElement> GetElements()
        {
            foreach (var element in base.GetElements())
            {
                yield return element;
            }

            if (_visualElement != null)
            {
                yield return _visualElement;
            }
        }

        #endregion

        #region IImageCellView 实现

        /// <summary>
        /// 加载图片
        /// </summary>
        /// <param name="fileId">文件ID</param>
        public void LoadImage(long fileId)
        {
            _currentFileId = fileId;
            _pendingFileId = fileId;
            CurrentFileId = fileId;
        }

        /// <summary>
        /// 异步加载图片
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <returns>加载任务</returns>
        public async Task LoadImageAsync(long fileId)
        {
            _currentFileId = fileId;
            _pendingFileId = fileId;
            CurrentFileId = fileId;

            // 尝试从缓存或服务加载图片
            await LoadImageFromServiceAsync(fileId);
        }

        /// <summary>
        /// 清空图片
        /// </summary>
        public void ClearImage()
        {
            GridImage = null;
            _currentImageHash = string.Empty;
        }

        /// <summary>
        /// 获取图片ID
        /// </summary>
        /// <returns>图片ID</returns>
        public long GetImageId()
        {
            return CurrentFileId;
        }

        /// <summary>
        /// 获取图片信息
        /// </summary>
        /// <returns>图片信息</returns>
        public ImageInfo GetImageInfo()
        {
            return ImageStateManager.Instance.GetImageInfo(CurrentFileId);
        }

        #endregion

        #region PrepareView

        /// <summary>
        /// 准备视图
        /// </summary>
        protected override void PrepareView(CellContext context)
        {
            base.PrepareView(context);

            if (context.Value == null)
            {
                return;
            }

            // 处理ValueImageWeb对象
            if (context.Value is ValueImageWeb valueImageWeb)
            {
                ProcessValueImageWeb(context, valueImageWeb);
            }
            // 处理字节数组
            else if (context.Value is byte[] imageData)
            {
                string newHash = ImageHashHelper.GenerateHash(imageData);
                if (newHash != _currentImageHash)
                {
                    DisplayImageFromBytes(context, imageData);
                    _currentImageHash = newHash;
                }
            }
            // 处理Image对象
            else if (context.Value is Image image)
            {
                GridImage = image;
            }
        }

        #endregion

        #region 远程图片处理

        /// <summary>
        /// 处理ValueImageWeb对象
        /// </summary>
        /// <param name="context">单元格上下文</param>
        /// <param name="valueImageWeb">ValueImageWeb对象</param>
        protected override void ProcessValueImageWeb(CellContext context, ValueImageWeb valueImageWeb)
        {
            // 优先使用字节数据
            if (valueImageWeb.CellImageBytes != null && valueImageWeb.CellImageBytes.Length > 0)
            {
                string newHash = ImageHashHelper.GenerateHash(valueImageWeb.CellImageBytes);
                if (newHash != _currentImageHash)
                {
                    DisplayImageFromBytes(context, valueImageWeb.CellImageBytes);
                    _currentImageHash = newHash;
                }
            }
            // 其次使用路径数据
            else if (!string.IsNullOrEmpty(valueImageWeb.CellImageHashName))
            {
                if (valueImageWeb.CellImageHashName != _currentImageHash)
                {
                    LoadAndDisplayImageFromPath(context, valueImageWeb.CellImageHashName);
                    _currentImageHash = valueImageWeb.CellImageHashName;
                }
            }
            // 最后使用FileId
            else if (valueImageWeb.FileId > 0)
            {
                base.ProcessValueImageWeb(context, valueImageWeb);
            }
        }

        /// <summary>
        /// 从路径加载并显示图片
        /// </summary>
        /// <param name="context">单元格上下文</param>
        /// <param name="imagePath">图片路径</param>
        private async void LoadAndDisplayImageFromPath(CellContext context, string imagePath)
        {
            try
            {
                var imageService = GridImageServiceManager.CurrentService;
                if (imageService != null && long.TryParse(Path.GetFileNameWithoutExtension(imagePath), out long imageId))
                {
                    var imageData = await imageService.DownloadImageAsync(imageId);
                    if (imageData != null && imageData.Length > 0)
                    {
                        string newHash = ImageHashHelper.GenerateHash(imageData);
                        if (newHash != _currentImageHash)
                            if (context.Grid.InvokeRequired)
                            {
                                context.Grid.Invoke(new Action(() => DisplayImageFromBytes(context, imageData)));
                            }
                            else
                            {
                                DisplayImageFromBytes(context, imageData);
                            }
                    }
                }
            }

            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"加载远程图片失败: {ex.Message}");
            }
        }

        #endregion

        #region 克隆

        /// <summary>
        /// 克隆当前对象2
        /// </summary>
        public override object Clone()
        {
            var clone = new RemoteImageView();
            clone._currentFileId = _currentFileId;
            clone._pendingFileId = _pendingFileId;
            clone._currentImageHash = _currentImageHash;
            clone._cachedImageStatus = _cachedImageStatus;
            return clone;
        }

        #endregion

        #region 刷新

        /// <summary>
        /// 刷新视图
        /// </summary>
        public override void Refresh(CellContext context)
        {
            PrepareView(context);
        }

        /// <summary>
        /// 从服务异步加载图片
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <returns>加载任务</returns>
        private async Task LoadImageFromServiceAsync(long fileId)
        {
            try
            {
                var imageService = GridImageServiceManager.CurrentService;
                if (imageService != null)
                {
                    var imageData = await imageService.DownloadImageAsync(fileId);
                    if (imageData != null && imageData.Length > 0)
                    {
                        // 在UI线程中更新图片
                        if (Application.OpenForms.Count > 0)
                        {
                            Application.OpenForms[0].Invoke(new Action(() =>
                            {
                                using (var ms = new MemoryStream(imageData))
                                {
                                    GridImage = Image.FromStream(ms);
                                }
                            }));
                        }
                        else
                        {
                            using (var ms = new MemoryStream(imageData))
                            {
                                GridImage = Image.FromStream(ms);
                            }
                        }
                    }
                    else
                    {
                        // 显示占位符图片
                        DisplayPlaceholderImage();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"从服务加载图片失败: {ex.Message}");
                DisplayPlaceholderImage();
            }
        }

        /// <summary>
        /// 显示占位符图片
        /// </summary>
        private void DisplayPlaceholderImage()
        {
            try
            {
                // 创建简单的占位符图片
                var placeholder = new Bitmap(50, 50);
                using (var g = Graphics.FromImage(placeholder))
                {
                    g.Clear(Color.LightGray);
                    g.DrawString("图片", new Font("Arial", 8), Brushes.DarkGray, 5, 20);
                }
                GridImage = placeholder;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"显示占位符图片失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 验证图片缓存有效性
        /// </summary>
        /// <param name="imageId">图片ID</param>
        /// <returns>是否有效</returns>
        private bool IsCacheValid(long imageId)
        {
            try
            {
                var cache = ImageCache.Instance;
                if (cache != null)
                {
                    var cachedImage = cache.GetImage(imageId);
                    return cachedImage != null;
                }
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"验证缓存有效性失败: {ex.Message}");
                return false;
            }
        }

        #endregion
    }
}
