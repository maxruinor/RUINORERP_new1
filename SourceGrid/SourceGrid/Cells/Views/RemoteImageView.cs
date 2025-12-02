using System;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using DevAge.Drawing;
using System.IO;
using DevAge.Drawing.VisualElements;
using System.Drawing.Imaging;
using SourceGrid.Cells.Editors;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;

namespace SourceGrid.Cells.Views
{
    /// <summary>
    /// 单独的一个图片格子。区别于他原来提供的。这种方式，公用设置属性会将值全设置为一个样。得每次给值 都重新设置 所以用new每个cell
    /// 重新写一个适用于WEB远程的显示图片的用法
    /// 增强版：优化渲染效率、内存管理和异步加载
    /// </summary>
    [Serializable]
    public class RemoteImageView : Cell, IDisposable
    {
        #region Constructors



        /// <summary>
        /// Use default setting
        /// </summary>
        public RemoteImageView()
        {
            ElementsDrawMode = DevAge.Drawing.ElementsDrawMode.Covering;
            FirstBackground = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
            SecondBackground = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.LightCyan);
            DevAge.Drawing.BorderLine border = new DevAge.Drawing.BorderLine(Color.DarkKhaki, 1);
            DevAge.Drawing.RectangleBorder cellBorder = new DevAge.Drawing.RectangleBorder(border, border);
            Border = cellBorder;
        }

        private System.Drawing.Image _GridImage;
        private bool _disposed = false;
        private string _pendingFileId;
        private Task<System.Drawing.Image> _imageLoadTask;
        private bool _enableAsyncLoading = true;
        private bool _enableMemoryOptimization = true;

        /// <summary>
        /// 是否启用异步加载
        /// </summary>
        public bool EnableAsyncLoading
        {
            get => _enableAsyncLoading;
            set => _enableAsyncLoading = value;
        }

        /// <summary>
        /// 是否启用内存优化
        /// </summary>
        public bool EnableMemoryOptimization
        {
            get => _enableMemoryOptimization;
            set => _enableMemoryOptimization = value;
        }

        /// <summary>
        /// 当前文件ID
        /// </summary>
        public string CurrentFileId { get; private set; }

        /// <summary>
        /// 图片加载完成事件
        /// </summary>
        public event EventHandler<ImageLoadEventArgs> ImageLoaded;

        /// <summary>
        /// 图片加载失败事件
        /// </summary>
        public event EventHandler<ImageLoadErrorEventArgs> ImageLoadError;

        public RemoteImageView(System.Drawing.Image image)
        {
            _GridImage = image;
            FirstBackground = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.White);
            SecondBackground = new DevAge.Drawing.VisualElements.BackgroundSolid(Color.LightCyan);
            DevAge.Drawing.BorderLine border = new DevAge.Drawing.BorderLine(Color.DarkKhaki, 1);
            DevAge.Drawing.RectangleBorder cellBorder = new DevAge.Drawing.RectangleBorder(border, border);
            Border = cellBorder;
        }

        public delegate void LoadImageDelegate(System.Drawing.Image image, byte[] buffByte);

        private DevAge.Drawing.VisualElements.IVisualElement mFirstBackground;
        public DevAge.Drawing.VisualElements.IVisualElement FirstBackground
        {
            get { return mFirstBackground; }
            set { mFirstBackground = value; }
        }

        private DevAge.Drawing.VisualElements.IVisualElement mSecondBackground;
        public DevAge.Drawing.VisualElements.IVisualElement SecondBackground
        {
            get { return mSecondBackground; }
            set { mSecondBackground = value; }
        }


        /// <summary>
        /// 验证数据
        /// </summary>
        public event LoadImageDelegate OnLoadImage;

        //不显示图片的原因是第一次加载时先执行了 PrepareView，再draw内容。但是目前是值的变化事件中用了刷新


        protected override void PrepareView(CellContext context)
        {
            base.PrepareView(context);
            if (Math.IEEERemainder(context.Position.Row, 2) == 0)
                Background = FirstBackground;
            else
                Background = SecondBackground;

            //start by watson 2024-1-11
            if (context.Value == null)
            {
                GridImage = null;
                return;
            }
            //显示图片  要是图片列才处理
            if (context.Cell is SourceGrid.Cells.Image || context.Value is Bitmap || context.Value is Image || context.Value is byte[])
            {
                //end by watson 2024-08-28 TODO:
                //PrepareVisualElementImage(context);

                //Read the image
                if (context.Value is byte[])
                {
                    //将图像读入到字节数组
                    byte[] buffByte = context.Value as byte[];
                    System.Drawing.Image img = null;
                    // 使用 MemoryStream 从字节数组创建流
                    using (MemoryStream stream = new MemoryStream(context.Value as byte[]))
                    {
                        // 从流中创建 Image 对象
                        img = System.Drawing.Image.FromStream(stream);
                        if (img != null)
                        {
                            GridImage = img;
                            // context.Cell = new SourceGrid.Cells.Image(img);
                            //context.Cell.View = new SourceGrid.Cells.Views.SingleImage(img);
                        }
                    }

                }

                if (context.Value is Bitmap || context.Value is System.Drawing.Image)
                {
                    // 使用 MemoryStream 从字节数组创建流
                    GridImage = context.Value as System.Drawing.Image;
                }

            }
            else if (context.Value is string && GridImage == null)
            {
                _pendingFileId = context.Value.ToString();
                CurrentFileId = _pendingFileId;

                if (_enableAsyncLoading)
                {
                    // 异步加载图片
                    LoadImageAsync(_pendingFileId, context);
                }
                else
                {
                    // 同步加载图片
                    LoadImageSync(_pendingFileId, context);
                }
            }

        }

        /// <summary>
        /// 强制显示图片
        /// </summary>
        public override void Refresh(CellContext context)
        {
            if (context.Value == null)
            {
                return;
            }
            //显示图片  要是图片列才处理
            if (context.Cell is SourceGrid.Cells.Image || context.Value is Bitmap || context.Value is Image || context.Value is byte[])
            {
                //end by watson 2024-08-28 TODO:
                //PrepareVisualElementImage(context);

                //Read the image
                if (context.Value is byte[])
                {
                    //将图像读入到字节数组
                    byte[] buffByte = context.Value as byte[];
                    System.Drawing.Image img = null;
                    // 使用 MemoryStream 从字节数组创建流
                    using (MemoryStream stream = new MemoryStream(context.Value as byte[]))
                    {
                        // 从流中创建 Image 对象
                        img = System.Drawing.Image.FromStream(stream);
                        if (img != null)
                        {
                            GridImage = img;
                            // context.Cell = new SourceGrid.Cells.Image(img);
                            //context.Cell.View = new SourceGrid.Cells.Views.SingleImage(img);
                        }
                    }
                }

                if (context.Value is Bitmap || context.Value is System.Drawing.Image)
                {
                    // 使用 MemoryStream 从字节数组创建流
                    GridImage = context.Value as System.Drawing.Image;
                }

            }
            else if (context.Value is string && GridImage == null)
            {
                _pendingFileId = context.Value.ToString();
                CurrentFileId = _pendingFileId;

                if (_enableAsyncLoading)
                {
                    // 异步加载图片
                    LoadImageAsync(_pendingFileId, context);
                }
                else
                {
                    // 同步加载图片
                    LoadImageSync(_pendingFileId, context);
                }
            }
        }

        protected override void OnDraw(GraphicsCache graphics, RectangleF area)
        {
            base.OnDraw(graphics, area);
        }

        protected override void OnDrawContent(GraphicsCache graphics, RectangleF area)
        {
            base.OnDrawContent(graphics, area);
            using (MeasureHelper measure = new MeasureHelper(graphics))
            {
                if (GridImage != null)
                {
                    graphics.Graphics.DrawImage(GridImage, Rectangle.Round(area)); //Note: 如果我不做矩形。有时，图像会以奇怪的拉伸方式绘制（不清晰）。这个问题可能是由于使用浮点重载的图形代码中的某些舍入引起的
                }
                else
                {
                    if (OnLoadImage != null)
                    {
                        OnLoadImage(GridImage, null);
                        graphics.Graphics.DrawImage(GridImage, Rectangle.Round(area)); //Note: 如果我不做矩形。有时，图像会以奇怪的拉伸方式绘制（不清晰）。这个问题可能是由于使用浮点重载的图形代码中的某些舍入引起的
                    }
                }
            }
        }
        protected override void OnDrawBackground(GraphicsCache graphics, RectangleF area)
        {
            base.OnDrawBackground(graphics, area);
        }


        /// <summary>
        /// Copy constructor.  This method duplicate all the reference field (Image, Font, StringFormat) creating a new instance.
        /// </summary>
        /// <param name="other"></param>
        public RemoteImageView(RemoteImageView other)
            : base(other)
        {
            mImage = (DevAge.Drawing.VisualElements.IVisualElement)other.OneImage.Clone();
        }
        #endregion

        private DevAge.Drawing.VisualElements.IVisualElement mImage = new DevAge.Drawing.VisualElements.Image();
        /// <summary>
        /// Images of the cells
        /// </summary>
        public DevAge.Drawing.VisualElements.IVisualElement OneImage
        {
            get { return mImage; }
        }

        /// <summary>
        /// 图片对象（优化内存管理）
        /// </summary>
        public System.Drawing.Image GridImage 
        { 
            get => _GridImage; 
            set
            {
                // 释放旧图片
                if (_enableMemoryOptimization && _GridImage != null && _GridImage != value)
                {
                    _GridImage.Dispose();
                }
                _GridImage = value;
            }
        }

        protected override IEnumerable<DevAge.Drawing.VisualElements.IVisualElement> GetElements()
        {
            foreach (DevAge.Drawing.VisualElements.IVisualElement v in GetBaseElements())
                yield return v;
            if (OneImage != null)
            {
                yield return OneImage;
            }

        }



        private IEnumerable<DevAge.Drawing.VisualElements.IVisualElement> GetBaseElements()
        {
            return base.GetElements();
        }

        #region Clone
        /// <summary>
        /// Clone this object. This method duplicate all the reference field (Image, Font, StringFormat) creating a new instance.
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new RemoteImageView(this);
        }
        #endregion

        #region 异步加载方法

        /// <summary>
        /// 异步加载图片
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <param name="context">单元格上下文</param>
        private async void LoadImageAsync(string fileId, CellContext context)
        {
            if (string.IsNullOrEmpty(fileId))
                return;

            // 取消之前的加载任务
            if (_imageLoadTask != null)
            {
                try
                {
                    _imageLoadTask.Dispose();
                }
                catch { }
            }

            _imageLoadTask = System.Threading.Tasks.Task.Run(async () =>
            {
                try
                {
                    // 使用缓存管理器异步加载图片
                    return await ImageCacheManager.Instance.GetImageAsync(
                        fileId,
                        async (id) => await LoadImageDataAsync(id, context)
                    );
                }
                catch (Exception ex)
                {
                    // 触发错误事件
                    ImageLoadError?.Invoke(this, new ImageLoadErrorEventArgs(fileId, ex));
                    return null;
                }
            });

            try
            {
                var image = await _imageLoadTask;
                if (image != null && !_disposed)
                {
                    // 在UI线程中更新
                    context.Grid.Invoke((Action)(() =>
                    {
                        if (!_disposed)
                        {
                            GridImage = image;
                            ImageLoaded?.Invoke(this, new ImageLoadEventArgs(fileId, image));
                            // 触发重绘
                            context.Grid.InvalidateCell(context.Position);
                        }
                    }));
                }
            }
            catch (Exception ex)
            {
                ImageLoadError?.Invoke(this, new ImageLoadErrorEventArgs(fileId, ex));
            }
        }

        /// <summary>
        /// 同步加载图片
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <param name="context">单元格上下文</param>
        private void LoadImageSync(string fileId, CellContext context)
        {
            try
            {
                if (context.Cell.Editor is ImageWebPickEditor webPicker)
                {
                    var model = context.Cell.Model.FindModel(typeof(SourceGrid.Cells.Models.ValueImageWeb));
                    if (model is SourceGrid.Cells.Models.ValueImageWeb valueImageWeb)
                    {
                        if (valueImageWeb.CellImageBytes != null && valueImageWeb.CellImageBytes.Length > 0)
                        {
                            GridImage = ImageProcessor.ByteArrayToImage(valueImageWeb.CellImageBytes);
                        }
                        else
                        {
                            // 尝试从缓存加载
                            GridImage = ImageCacheManager.Instance.GetImage(
                                fileId,
                                (id) => LoadImageDataSync(id, context)
                            );
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ImageLoadError?.Invoke(this, new ImageLoadErrorEventArgs(fileId, ex));
            }
        }

        /// <summary>
        /// 异步加载图片数据
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <param name="context">单元格上下文</param>
        /// <returns>图片字节数据</returns>
        private async Task<byte[]> LoadImageDataAsync(string fileId, CellContext context)
        {
            return await Task.Run(() => LoadImageDataSync(fileId, context));
        }

        /// <summary>
        /// 同步加载图片数据
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <param name="context">单元格上下文</param>
        /// <returns>图片字节数据</returns>
        private byte[] LoadImageDataSync(string fileId, CellContext context)
        {
            try
            {
                // 从ValueImageWeb获取图片数据
                if (context.Cell.Editor is ImageWebPickEditor webPicker)
                {
                    var model = context.Cell.Model.FindModel(typeof(SourceGrid.Cells.Models.ValueImageWeb));
                    if (model is SourceGrid.Cells.Models.ValueImageWeb valueImageWeb)
                    {
                        return valueImageWeb.CellImageBytes;
                    }
                }

                // 尝试从本地文件加载
                if (File.Exists(fileId))
                {
                    return File.ReadAllBytes(fileId);
                }

                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"加载图片数据失败: {ex.Message}");
                return null;
            }
        }

        #endregion

        #region 内存优化

        /// <summary>
        /// 释放内存资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放资源的具体实现
        /// </summary>
        /// <param name="disposing">是否正在释放托管资源</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                // 取消加载任务
                if (_imageLoadTask != null)
                {
                    try
                    {
                        _imageLoadTask.Dispose();
                    }
                    catch { }
                }

                // 释放图片资源
                if (_GridImage != null)
                {
                    _GridImage.Dispose();
                    _GridImage = null;
                }

                // 清除事件
                ImageLoaded = null;
                ImageLoadError = null;

                _disposed = true;
            }
        }

        #endregion
    }

    /// <summary>
    /// 图片加载事件参数
    /// </summary>
    public class ImageLoadEventArgs : EventArgs
    {
        public string FileId { get; }
        public System.Drawing.Image Image { get; }

        public ImageLoadEventArgs(string fileId, System.Drawing.Image image)
        {
            FileId = fileId;
            Image = image;
        }
    }

    /// <summary>
    /// 图片加载错误事件参数
    /// </summary>
    public class ImageLoadErrorEventArgs : EventArgs
    {
        public string FileId { get; }
        public Exception Error { get; }

        public ImageLoadErrorEventArgs(string fileId, Exception error)
        {
            FileId = fileId;
            Error = error;
        }
    }
}
