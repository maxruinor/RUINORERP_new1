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
            if (context.Cell is SourceGrid.Cells.ImageCell || context.Value is Bitmap || context.Value is Image || context.Value is byte[] || context.Value is ImageCellValue)
            {
                //end by watson 2024-08-28 TODO:
                //PrepareVisualElementImage(context);

                //Read the image
                if (context.Value is ImageCellValue icv)
                {
                    if (icv.IsBinary && icv.ImageData != null)
                    {
                        byte[] buffByte = icv.ImageData;
                        System.Drawing.Image img = null;
                        using (MemoryStream stream = new MemoryStream(buffByte))
                        {
                            try
                            {
                                System.Drawing.Image tempImg = System.Drawing.Image.FromStream(stream);
                                if (tempImg != null)
                                {
                                    img = new Bitmap(tempImg);
                                    tempImg.Dispose();
                                }
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine("从ImageCellValue创建图片失败: " + ex.Message);
                            }
                        }

                        if (GridImage != null)
                        {
                            GridImage.Dispose();
                            GridImage = null;
                        }
                        GridImage = img;
                    }
                    else if (!string.IsNullOrEmpty(icv.ImagePath))
                    {
                        _pendingFileId = icv.ImagePath;
                        CurrentFileId = _pendingFileId;
                        if (_enableAsyncLoading)
                        {
                            LoadImageAsync(_pendingFileId, context);
                        }
                        else
                        {
                            LoadImageSync(_pendingFileId, context);
                        }
                    }
                }
                else if (context.Value is byte[])
                {
                    //将图像读入到字节数组
                    byte[] buffByte = context.Value as byte[];
                    System.Drawing.Image img = null;
                    // 使用 MemoryStream 从字节数组创建流
                    // 注意：Image.FromStream创建的Image对象会保持对这个流的引用
                    // 为了避免内存泄漏，使用 Clone 创建新的 Image 副本，不再依赖流
                    using (MemoryStream stream = new MemoryStream(context.Value as byte[]))
                    {
                        try
                        {
                            System.Drawing.Image tempImg = System.Drawing.Image.FromStream(stream);
                            if (tempImg != null)
                            {
                                img = new Bitmap(tempImg);
                                tempImg.Dispose();
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine("从字节数组创建图片失败: " + ex.Message);
                        }
                    }

                    if (GridImage != null)
                    {
                        GridImage.Dispose();
                        GridImage = null;
                    }
                    GridImage = img;
                }

                if (context.Value is Bitmap || context.Value is System.Drawing.Image)
                {
                    // 使用 MemoryStream 从字节数组创建流
                    if (GridImage != null && GridImage != (System.Drawing.Image)context.Value)
                    {
                        GridImage.Dispose();
                    }
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
            if (context.Cell is SourceGrid.Cells.ImageCell || context.Value is Bitmap || context.Value is Image || context.Value is byte[] || context.Value is ImageCellValue)
            {
                //end by watson 2024-08-28 TODO:
                //PrepareVisualElementImage(context);

                //Read the image
                if (context.Value is ImageCellValue icv)
                {
                    if (icv.IsBinary && icv.ImageData != null)
                    {
                        byte[] buffByte = icv.ImageData;
                        System.Drawing.Image img = null;
                        using (MemoryStream stream = new MemoryStream(buffByte))
                        {
                            try
                            {
                                System.Drawing.Image tempImg = System.Drawing.Image.FromStream(stream);
                                if (tempImg != null)
                                {
                                    img = new Bitmap(tempImg);
                                    tempImg.Dispose();
                                }
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine("从ImageCellValue创建图片失败: " + ex.Message);
                            }
                        }

                        if (GridImage != null)
                        {
                            GridImage.Dispose();
                            GridImage = null;
                        }
                        GridImage = img;
                    }
                    else if (!string.IsNullOrEmpty(icv.ImagePath))
                    {
                        _pendingFileId = icv.ImagePath;
                        CurrentFileId = _pendingFileId;
                        if (_enableAsyncLoading)
                        {
                            LoadImageAsync(_pendingFileId, context);
                        }
                        else
                        {
                            LoadImageSync(_pendingFileId, context);
                        }
                    }
                }
                else if (context.Value is byte[])
                {
                    //将图像读入到字节数组
                    byte[] buffByte = context.Value as byte[];
                    System.Drawing.Image img = null;
                    // 使用 MemoryStream 从字节数组创建流
                    // 注意：Image.FromStream创建的Image对象会保持对这个流的引用
                    // 为了避免内存泄漏，使用 Clone 创建新的 Image 副本，不再依赖流
                    using (MemoryStream stream = new MemoryStream(context.Value as byte[]))
                    {
                        try
                        {
                            System.Drawing.Image tempImg = System.Drawing.Image.FromStream(stream);
                            if (tempImg != null)
                            {
                                img = new Bitmap(tempImg);
                                tempImg.Dispose();
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine("从字节数组创建图片失败: " + ex.Message);
                        }
                    }

                    if (GridImage != null)
                    {
                        GridImage.Dispose();
                        GridImage = null;
                    }
                    GridImage = img;
                }

                if (context.Value is Bitmap || context.Value is System.Drawing.Image)
                {
                    // 使用 MemoryStream 从字节数组创建流
                    if (GridImage != null && GridImage != (System.Drawing.Image)context.Value)
                    {
                        GridImage.Dispose();
                    }
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
                // 使用局部变量避免多线程问题
                var currentImage = GridImage;
                
                // 检查GridImage是否有效
                if (currentImage != null)
                {
                    try
                    {
                        // 先检查图片是否有效，避免 ArgumentException
                        if (IsImageValid(currentImage))
                        {
                            graphics.Graphics.DrawImage(currentImage, Rectangle.Round(area)); //1Note: 如果我不做矩形。有时，图像会以奇怪的拉伸方式绘制（不清晰）。这个问题可能是由于使用浮点重载的图形代码中的某些舍入引起的
                        }
                        else
                        {
                            // 图片无效，释放并设置为null
                            GridImage = null;
                        }
                    }
                    catch (Exception ex)
                    {
                        // 绘制失败，将GridImage设置为null
                        GridImage = null;
                        System.Diagnostics.Debug.WriteLine("绘制图片失败: " + ex.Message);
                    }
                }
                else
                {
                    if (OnLoadImage != null)
                    {
                        OnLoadImage(currentImage, null);
                        // 重新获取GridImage
                        currentImage = GridImage;
                        // 确保GridImage不为null后再绘制
                        if (currentImage != null)
                        {
                            try
                            {
                                if (IsImageValid(currentImage))
                                {
                                    graphics.Graphics.DrawImage(currentImage, Rectangle.Round(area)); //Note: 如果我不做矩形。有时，图像会以奇怪的拉伸方式绘制（不清晰）。这个问题可能是由于使用浮点重载的图形代码中的某些舍入引起的
                                }
                                else
                                {
                                    GridImage = null;
                                }
                            }
                            catch (Exception ex)
                            {
                                // 绘制失败，将GridImage设置为null
                                GridImage = null;
                                System.Diagnostics.Debug.WriteLine("绘制图片失败: " + ex.Message);
                            }
                        }
                    }
                }

                // 绘制图片状态标记
                DrawImageStatus(graphics, area);
            }
        }

        /// <summary>
        /// 绘制图片状态标记
        /// </summary>
        /// <param name="graphics">图形对象</param>
        /// <param name="area">绘制区域</param>
        private void DrawImageStatus(GraphicsCache graphics, RectangleF area)
        {
            try
            {
                // 获取图片状态
                var status = GetImageStatus();
                
                // 根据状态绘制不同的标记
                switch (status)
                {
                    case ImageStatus.PendingDelete:
                        // 绘制待删除标记
                        DrawPendingDeleteMark(graphics, area);
                        break;
                    case ImageStatus.PendingUpload:
                        // 绘制待上传标记
                        DrawPendingUploadMark(graphics, area);
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("绘制图片状态标记失败: " + ex.Message);
            }
        }

        /// <summary>
        /// 获取图片状态
        /// </summary>
        /// <returns>图片状态</returns>
        private ImageStatus GetImageStatus()
        {
            try
            {
                // 检查是否已加载 ImageStateManager 类型
                var imageStateManagerType = Type.GetType("RUINORERP.UI.UCSourceGrid.ImageStateManager, RUINORERP.UI");
                if (imageStateManagerType != null)
                {
                    // 使用反射获取单例实例
                    var instanceProperty = imageStateManagerType.GetProperty("Instance", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                    if (instanceProperty != null)
                    {
                        var instance = instanceProperty.GetValue(null);
                        if (instance != null)
                        {
                            // 使用反射获取 GetImageInfo 方法
                            var getImageInfoMethod = imageStateManagerType.GetMethod("GetImageInfo");
                            if (getImageInfoMethod != null)
                            {
                                var imageInfo = getImageInfoMethod.Invoke(instance, new object[] { CurrentFileId });
                                if (imageInfo != null)
                                {
                                    // 使用反射获取 Status 属性
                                    var statusProperty = imageInfo.GetType().GetProperty("Status");
                                    if (statusProperty != null)
                                    {
                                        return (ImageStatus)statusProperty.GetValue(imageInfo);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("获取图片状态失败: " + ex.Message);
            }

            return ImageStatus.Normal;
        }

        /// <summary>
        /// 绘制待删除标记
        /// </summary>
        /// <param name="graphics">图形对象</param>
        /// <param name="area">绘制区域</param>
        private void DrawPendingDeleteMark(GraphicsCache graphics, RectangleF area)
        {
            // 绘制红色边框
            using (var pen = new Pen(Color.Red, 2))
            {
                graphics.Graphics.DrawRectangle(pen, Rectangle.Round(area));
            }

            // 绘制红色对角线
            using (var pen = new Pen(Color.Red, 2))
            {
                var rect = Rectangle.Round(area);
                var topLeft = new System.Drawing.Point(rect.X, rect.Y);
                var bottomRight = new System.Drawing.Point(rect.X + rect.Width, rect.Y + rect.Height);
                var topRight = new System.Drawing.Point(rect.X + rect.Width, rect.Y);
                var bottomLeft = new System.Drawing.Point(rect.X, rect.Y + rect.Height);
                graphics.Graphics.DrawLine(pen, topLeft, bottomRight);
                graphics.Graphics.DrawLine(pen, topRight, bottomLeft);
            }

            // 绘制"待删除"文字
            using (var font = new Font("Arial", 10, FontStyle.Bold))
            using (var brush = new SolidBrush(Color.Red))
            {
                var text = "待删除";
                var textSize = graphics.Graphics.MeasureString(text, font);
                var textX = area.X + (area.Width - textSize.Width) / 2;
                var textY = area.Y + (area.Height - textSize.Height) / 2;
                graphics.Graphics.DrawString(text, font, brush, textX, textY);
            }
        }

        /// <summary>
        /// 绘制待上传标记
        /// </summary>
        /// <param name="graphics">图形对象</param>
        /// <param name="area">绘制区域</param>
        private void DrawPendingUploadMark(GraphicsCache graphics, RectangleF area)
        {
            // 绘制蓝色边框
            using (var pen = new Pen(Color.Blue, 2))
            {
                graphics.Graphics.DrawRectangle(pen, Rectangle.Round(area));
            }

            // 绘制蓝色点状边框
            using (var pen = new Pen(Color.Blue, 1))
            {
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                var rect = Rectangle.Round(area);
                rect.Inflate(-2, -2);
                graphics.Graphics.DrawRectangle(pen, rect);
            }

            // 绘制"待上传"文字
            using (var font = new Font("Arial", 10, FontStyle.Bold))
            using (var brush = new SolidBrush(Color.Blue))
            {
                var text = "待上传";
                var textSize = graphics.Graphics.MeasureString(text, font);
                var textX = area.X + (area.Width - textSize.Width) / 2;
                var textY = area.Y + (area.Height - textSize.Height) / 2;
                graphics.Graphics.DrawString(text, font, brush, textX, textY);
            }
        }

        /// <summary>
        /// 图片状态枚举
        /// </summary>
        private enum ImageStatus
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
        /// 检查图片是否有效
        /// </summary>
        private bool IsImageValid(System.Drawing.Image image)
        {
            if (image == null)
                return false;

            try
            {
                // 尝试访问图片属性，如果无效会抛出异常
                var size = image.Size;
                var format = image.RawFormat;
                return size.Width > 0 && size.Height > 0;
            }
            catch
            {
                return false;
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

        private DevAge.Drawing.VisualElements.IVisualElement mImage = new DevAge.Drawing.VisualElements.VisualImage();
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
                ImageUploaded = null;
                ImageDeleted = null;

                _disposed = true;
            }
        }

        #endregion

        #region 图片管理功能

        /// <summary>
        /// 图片上传完成事件
        /// </summary>
        public event EventHandler<ImageUploadEventArgs> ImageUploaded;

        /// <summary>
        /// 图片删除完成事件
        /// </summary>
        public event EventHandler<ImageDeleteEventArgs> ImageDeleted;

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="imageBytes">图片字节数据</param>
        /// <param name="fileName">文件名</param>
        /// <param name="businessType">业务类型</param>
        /// <returns>上传结果</returns>
        public async Task<string> UploadImageAsync(byte[] imageBytes, string fileName, int businessType = 0)
        {
            try
            {
                // 使用GridImageService上传图片
                string imageId = await GridImageServiceManager.CurrentService.UploadImageAsync(imageBytes, fileName, businessType);
                
                // 触发上传完成事件
                ImageUploaded?.Invoke(this, new ImageUploadEventArgs(fileName, true));
                return imageId;
            }
            catch (Exception ex)
            {
                // 触发上传失败事件
                ImageUploaded?.Invoke(this, new ImageUploadEventArgs(fileName, false, ex));
                return null;
            }
        }

        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="imageId">图片ID</param>
        /// <returns>删除结果</returns>
        public async Task<bool> DeleteImageAsync(string imageId)
        {
            try
            {
                // 使用GridImageService删除图片
                bool result = await GridImageServiceManager.CurrentService.DeleteImageAsync(imageId);
                
                // 触发删除完成事件
                ImageDeleted?.Invoke(this, new ImageDeleteEventArgs(imageId, result));
                return result;
            }
            catch (Exception ex)
            {
                // 触发删除失败事件
                ImageDeleted?.Invoke(this, new ImageDeleteEventArgs(imageId, false, ex));
                return false;
            }
        }

        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="imageId">图片ID</param>
        /// <returns>图片字节数据</returns>
        public async Task<byte[]> DownloadImageAsync(string imageId)
        {
            try
            {
                // 使用GridImageService下载图片
                return await GridImageServiceManager.CurrentService.DownloadImageAsync(imageId);
            }
            catch (Exception ex)
            {
                // 触发下载失败事件
                ImageLoadError?.Invoke(this, new ImageLoadErrorEventArgs(imageId, ex));
                return null;
            }
        }

        /// <summary>
        /// 获取图片信息
        /// </summary>
        /// <param name="imageId">图片ID</param>
        /// <returns>图片信息</returns>
        public async Task<GridImageInfo> GetImageInfoAsync(string imageId)
        {
            try
            {
                // 使用GridImageService获取图片信息
                return await GridImageServiceManager.CurrentService.GetImageInfoAsync(imageId);
            }
            catch (Exception ex)
            {
                // 触发获取信息失败事件
                ImageLoadError?.Invoke(this, new ImageLoadErrorEventArgs(imageId, ex));
                return null;
            }
        }

        /// <summary>
        /// 检查图片是否需要更新
        /// </summary>
        /// <returns>是否需要更新</returns>
        public bool IsImageNeedingUpdate()
        {
            // 这里可以实现图片更新检测逻辑
            // 例如比较图片哈希值等
            return false;
        }

        /// <summary>
        /// 重置图片状态
        /// </summary>
        public void ResetImageStatus()
        {
            // 这里可以实现重置图片状态的逻辑
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

    /// <summary>
    /// 图片上传事件参数
    /// </summary>
    public class ImageUploadEventArgs : EventArgs
    {
        public string FileName { get; }
        public bool Success { get; }
        public Exception Error { get; }

        public ImageUploadEventArgs(string fileName, bool success, Exception error = null)
        {
            FileName = fileName;
            Success = success;
            Error = error;
        }
    }

    /// <summary>
    /// 图片删除事件参数
    /// </summary>
    public class ImageDeleteEventArgs : EventArgs
    {
        public string ImageId { get; }
        public bool Success { get; }
        public Exception Error { get; }

        public ImageDeleteEventArgs(string imageId, bool success, Exception error = null)
        {
            ImageId = imageId;
            Success = success;
            Error = error;
        }
    }
}
