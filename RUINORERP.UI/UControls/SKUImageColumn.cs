using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Model;
using RUINORERP.UI.Network.Services;

namespace RUINORERP.UI.UControls
{
    /// <summary>
    /// SKU图片列 - 专门用于显示SKU图片的DataGridView列
    /// 支持从ImagesPath路径加载图片，集成ImageCacheService缓存
    /// </summary>
    public class SKUImageColumn : DataGridViewColumn
    {
        /// <summary>
        /// 图片缓存服务
        /// </summary>
        private ImageCacheService _imageCacheService;

        /// <summary>
        /// SKU图片列宽度（固定宽度）
        /// </summary>
        public const int DefaultWidth = 80;

        /// <summary>
        /// 缩略图大小
        /// </summary>
        public const int ThumbnailSize = 60;

        public SKUImageColumn()
            : base(new SKUImageCell())
        {
            // 初始化图片缓存服务
            try
            {
                _imageCacheService = Startup.GetFromFac<ImageCacheService>();
            }
            catch
            {
                // 设计时模式下可能无法获取服务，忽略异常
            }

            // 设置列属性
            this.Width = DefaultWidth;
            this.HeaderText = "SKU图片";
            this.ReadOnly = true; // 只读，双击编辑
            this.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.DefaultCellStyle.NullValue = null;
        }

        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                // 确保单元格类型正确
                if (value != null && !value.GetType().IsAssignableFrom(typeof(SKUImageCell)))
                {
                    throw new InvalidCastException("Must be a SKUImageCell");
                }
                base.CellTemplate = value;
            }
        }

        /// <summary>
        /// 获取图片缓存服务
        /// </summary>
        public ImageCacheService ImageCacheService => _imageCacheService;
    }

    /// <summary>
    /// SKU图片单元格 - 支持异步加载和显示SKU图片
    /// </summary>
    public class SKUImageCell : DataGridViewImageCell
    {
        /// <summary>
        /// 是否正在加载图片
        /// </summary>
        private bool _isLoading = false;

        /// <summary>
        /// 加载错误标记
        /// </summary>
        private bool _loadError = false;

        /// <summary>
        /// 已加载的图片
        /// </summary>
        private Image _loadedImage = null;

        public SKUImageCell()
            : base()
        {
            // 设置默认值
            this.ValueType = typeof(string);
            this.ImageLayout = DataGridViewImageCellLayout.Zoom;
        }

        /// <summary>
        /// 重写Paint方法，自定义绘制图片
        /// 优化：确保图片在单元格内绘制，不覆盖其他行
        /// </summary>
        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds,
            int rowIndex, DataGridViewElementStates cellState, object value,
            object formattedValue, string errorText, DataGridViewCellStyle cellStyle,
            DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            // 设置裁剪区域，确保绘制不会超出单元格边界
            Rectangle oldClip = graphics.ClipBounds.IsEmpty ? Rectangle.Empty : Rectangle.Round(graphics.ClipBounds);
            graphics.SetClip(cellBounds);

            try
            {
                // 绘制背景
                base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value,
                    formattedValue, errorText, cellStyle, advancedBorderStyle,
                    paintParts & ~DataGridViewPaintParts.ContentForeground);

                // 获取SKU数据
                var detail = GetProdDetail(rowIndex);
                if (detail == null)
                {
                    DrawPlaceholder(graphics, cellBounds, "无图片");
                    return;
                }

                // 优先检查frmProductEdit中的缓存数据（新添加但未上传的图片）
                var frmProductEdit = this.DataGridView?.FindForm() as ProductEAV.frmProductEdit;
                if (frmProductEdit != null)
                {
                    // 使用反射获取私有缓存字段
                    var cacheField = typeof(ProductEAV.frmProductEdit).GetField("skuImageDataCache",
                        BindingFlags.NonPublic | BindingFlags.Instance);
                    if (cacheField != null)
                    {
                        var cache = cacheField.GetValue(frmProductEdit) as System.Collections.Generic.Dictionary<tb_ProdDetail, System.Collections.Generic.List<System.Tuple<byte[], RUINOR.WinFormsUI.CustomPictureBox.ImageInfo>>>;
                        if (cache != null && cache.TryGetValue(detail, out var cachedImages) && cachedImages.Count > 0)
                        {
                            // 绘制缓存的图片
                            DrawCachedImage(graphics, cellBounds, cachedImages, detail.HasUnsavedImageChanges);
                            return;
                        }
                    }
                }

                // 检查是否有图片路径
                string imagesPath = detail.ImagesPath;
                if (string.IsNullOrEmpty(imagesPath))
                {
                    DrawPlaceholder(graphics, cellBounds, "双击添加图片");
                    return;
                }

                // 解析图片数量
                var imagePaths = imagesPath.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                if (imagePaths.Length == 0)
                {
                    DrawPlaceholder(graphics, cellBounds, "双击添加图片");
                    return;
                }

                // 异步加载图片（只在需要时加载）
                if (!_isLoading && _loadedImage == null && !_loadError)
                {
                    _isLoading = true;
                    LoadImageAsync(detail.ProdDetailID, imagePaths[0]).ContinueWith(task =>
                    {
                        _isLoading = false;
                        if (task.Status == TaskStatus.RanToCompletion && task.Result != null)
                        {
                            // 创建缩略图副本
                            _loadedImage = task.Result;
                        }
                        else
                        {
                            _loadError = true;
                        }

                        // 刷新单元格显示
                        if (this.DataGridView != null && rowIndex < this.DataGridView.Rows.Count)
                        {
                            this.DataGridView.InvalidateCell(this.ColumnIndex, rowIndex);
                        }
                    }, TaskScheduler.FromCurrentSynchronizationContext());
                }

                // 绘制加载状态
                if (_isLoading)
                {
                    DrawPlaceholder(graphics, cellBounds, "加载中...");
                }
                else if (_loadError)
                {
                    DrawPlaceholder(graphics, cellBounds, "加载失败");
                }
                else if (_loadedImage != null)
                {
                    // 绘制图片
                    DrawImage(graphics, cellBounds, _loadedImage, imagePaths.Length, false);
                }
                else
                {
                    // 显示已上传图片数量
                    DrawImageInfoPlaceholder(graphics, cellBounds, imagePaths.Length);
                }
            }
            finally
            {
                // 恢复原始裁剪区域
                if (oldClip.IsEmpty)
                    graphics.ResetClip();
                else
                    graphics.SetClip(oldClip);
            }
        }

        /// <summary>
        /// 绘制占位符文本
        /// </summary>
        private void DrawPlaceholder(Graphics graphics, Rectangle cellBounds, string text)
        {
            // 确保在单元格边界内绘制，留出边距
            var drawRect = new Rectangle(
                cellBounds.X + 2,
                cellBounds.Y + 2,
                cellBounds.Width - 4,
                cellBounds.Height - 4);

            using (var brush = new SolidBrush(Color.Gray))
            {
                // 计算合适的字体大小
                float fontSize = 8;
                Font font = new Font("Microsoft YaHei", fontSize);
                try
                {
                    var size = graphics.MeasureString(text, font);
                    // 如果文本宽度超过单元格，缩小字体
                    if (size.Width > drawRect.Width)
                    {
                        float scale = drawRect.Width / size.Width;
                        fontSize = font.Size * scale;
                        font.Dispose();
                        font = new Font("Microsoft YaHei", fontSize);
                        size = graphics.MeasureString(text, font);
                    }

                    var point = new PointF(
                        drawRect.Left + (drawRect.Width - size.Width) / 2,
                        drawRect.Top + (drawRect.Height - size.Height) / 2);

                    // 使用TextRenderer确保清晰渲染
                    TextRenderer.DrawText(graphics, text, font, new Point((int)point.X, (int)point.Y), Color.Gray);
                }
                finally
                {
                    font?.Dispose();
                }
            }
        }

        /// <summary>
        /// 绘制图片数量信息占位符
        /// </summary>
        private void DrawImageInfoPlaceholder(Graphics graphics, Rectangle cellBounds, int imageCount)
        {
            var drawRect = new Rectangle(
                cellBounds.X + 2,
                cellBounds.Y + 2,
                cellBounds.Width - 4,
                cellBounds.Height - 4);

            // 绘制边框
            using (var pen = new Pen(Color.LightGray, 1))
            {
                graphics.DrawRectangle(pen, drawRect);
            }

            // 绘制图片数量和提示
            using (var brush = new SolidBrush(Color.FromArgb(100, 0, 120, 215)))
            {
                // 计算合适的字体大小
                float fontSize = 9;
                Font font = new Font("Microsoft YaHei", fontSize, FontStyle.Bold);
                try
                {
                    string text = $"{imageCount}张图片";
                    var size = graphics.MeasureString(text, font);

                    // 确保文本在单元格内
                    while (size.Width > drawRect.Width - 4 && fontSize > 6)
                    {
                        fontSize -= 0.5f;
                        font.Dispose();
                        font = new Font("Microsoft YaHei", fontSize, FontStyle.Bold);
                        size = graphics.MeasureString(text, font);
                    }

                    var point = new PointF(
                        drawRect.Left + (drawRect.Width - size.Width) / 2,
                        drawRect.Top + (drawRect.Height - size.Height) / 2 - 5);

                    TextRenderer.DrawText(graphics, text, font, new Point((int)point.X, (int)point.Y), Color.FromArgb(100, 0, 120, 215));
                }
                finally
                {
                    font?.Dispose();
                }
            }

            // 绘制提示文本
            using (var hintFont = new Font("Microsoft YaHei", 7))
            {
                string hint = "双击查看";
                var hintSize = graphics.MeasureString(hint, hintFont);
                var hintPoint = new PointF(
                    drawRect.Left + (drawRect.Width - hintSize.Width) / 2,
                    drawRect.Bottom - hintSize.Height - 2);

                TextRenderer.DrawText(graphics, hint, hintFont, new Point((int)hintPoint.X, (int)hintPoint.Y), Color.Gray);
            }
        }

        /// <summary>
        /// 绘制图片
        /// 优化：在单元格内合理缩放和定位，不超出边界
        /// </summary>
        private void DrawImage(Graphics graphics, Rectangle cellBounds, Image image, int imageCount, bool hasUnsavedChanges)
        {
            // 计算可用绘制区域（留出边距）
            int padding = 4;
            var availableRect = new Rectangle(
                cellBounds.X + padding,
                cellBounds.Y + padding,
                cellBounds.Width - padding * 2,
                cellBounds.Height - padding * 2);

            // 如果单元格太小，使用最小尺寸
            if (availableRect.Width < 20 || availableRect.Height < 20)
            {
                availableRect = cellBounds;
            }

            // 计算缩略图大小（保持宽高比）
            float ratio = Math.Min(
                (float)availableRect.Width / image.Width,
                (float)availableRect.Height / image.Height);

            int thumbWidth = Math.Max(1, (int)(image.Width * ratio));
            int thumbHeight = Math.Max(1, (int)(image.Height * ratio));

            // 居中定位
            var imgRect = new Rectangle(
                availableRect.X + (availableRect.Width - thumbWidth) / 2,
                availableRect.Y + (availableRect.Height - thumbHeight) / 2,
                thumbWidth, thumbHeight);

            // 确保图片不会超出单元格边界
            if (imgRect.X < cellBounds.X) imgRect.X = cellBounds.X + 1;
            if (imgRect.Y < cellBounds.Y) imgRect.Y = cellBounds.Y + 1;
            if (imgRect.Right > cellBounds.Right) imgRect.Width = cellBounds.Right - imgRect.X - 1;
            if (imgRect.Bottom > cellBounds.Bottom) imgRect.Height = cellBounds.Bottom - imgRect.Y - 1;

            // 设置高质量绘制模式
            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

            // 绘制图片（保持比例）
            graphics.DrawImage(image, imgRect);

            // 如果有多个图片，绘制数量角标
            if (imageCount > 1)
            {
                DrawImageCountBadge(graphics, imgRect, imageCount);
            }

            // 如果有未保存的更改，显示星号标记
            if (hasUnsavedChanges)
            {
                DrawUnsavedBadge(graphics, cellBounds);
            }
        }

        /// <summary>
        /// 绘制缓存的图片（从byte[]数据绘制）
        /// 优化：在单元格内合理缩放和定位，不超出边界
        /// </summary>
        private void DrawCachedImage(Graphics graphics, Rectangle cellBounds,
            System.Collections.Generic.List<System.Tuple<byte[], RUINOR.WinFormsUI.CustomPictureBox.ImageInfo>> cachedImages,
            bool hasUnsavedChanges)
        {
            try
            {
                // 使用第一张图片
                var firstImageData = cachedImages[0].Item1;
                using (var ms = new System.IO.MemoryStream(firstImageData))
                using (var originalImage = System.Drawing.Image.FromStream(ms))
                {
                    // 计算可用绘制区域（留出边距）
                    int padding = 4;
                    var availableRect = new Rectangle(
                        cellBounds.X + padding,
                        cellBounds.Y + padding,
                        cellBounds.Width - padding * 2,
                        cellBounds.Height - padding * 2);

                    // 如果单元格太小，使用最小尺寸
                    if (availableRect.Width < 20 || availableRect.Height < 20)
                    {
                        availableRect = cellBounds;
                    }

                    // 计算缩略图大小（保持宽高比）
                    float ratio = Math.Min(
                        (float)availableRect.Width / originalImage.Width,
                        (float)availableRect.Height / originalImage.Height);

                    int thumbWidth = Math.Max(1, (int)(originalImage.Width * ratio));
                    int thumbHeight = Math.Max(1, (int)(originalImage.Height * ratio));

                    // 居中定位
                    var imgRect = new Rectangle(
                        availableRect.X + (availableRect.Width - thumbWidth) / 2,
                        availableRect.Y + (availableRect.Height - thumbHeight) / 2,
                        thumbWidth, thumbHeight);

                    // 确保图片不会超出单元格边界
                    if (imgRect.X < cellBounds.X) imgRect.X = cellBounds.X + 1;
                    if (imgRect.Y < cellBounds.Y) imgRect.Y = cellBounds.Y + 1;
                    if (imgRect.Right > cellBounds.Right) imgRect.Width = cellBounds.Right - imgRect.X - 1;
                    if (imgRect.Bottom > cellBounds.Bottom) imgRect.Height = cellBounds.Bottom - imgRect.Y - 1;

                    // 设置高质量绘制模式
                    graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

                    // 绘制图片（保持比例）
                    graphics.DrawImage(originalImage, imgRect);

                    // 如果有多个图片，绘制数量角标
                    if (cachedImages.Count > 1)
                    {
                        DrawImageCountBadge(graphics, imgRect, cachedImages.Count);
                    }

                    // 如果有未保存的更改，显示星号标记
                    if (hasUnsavedChanges)
                    {
                        DrawUnsavedBadge(graphics, cellBounds);
                    }
                }
            }
            catch
            {
                DrawPlaceholder(graphics, cellBounds, "图片加载失败");
            }
        }

        /// <summary>
        /// 绘制未保存更改的星号标记
        /// 优化：确保标记在单元格内，不会覆盖其他内容
        /// </summary>
        private void DrawUnsavedBadge(Graphics graphics, Rectangle cellBounds)
        {
            // 根据单元格大小调整标记大小
            int fontSize = Math.Min(10, Math.Max(8, cellBounds.Height / 3));

            using (var brush = new SolidBrush(Color.Orange))
            using (var font = new Font("Arial", fontSize, FontStyle.Bold))
            {
                string text = "*";
                var size = graphics.MeasureString(text, font);

                // 计算位置，确保在单元格内
                float x = cellBounds.Right - size.Width - 2;
                float y = cellBounds.Top + 1;

                // 确保不会超出边界
                if (x < cellBounds.X) x = cellBounds.X + 1;
                if (y > cellBounds.Bottom - size.Height) y = cellBounds.Bottom - size.Height - 1;

                TextRenderer.DrawText(graphics, text, font,
                    new Point((int)x, (int)y), Color.Orange);
            }
        }

        /// <summary>
        /// 绘制图片
        /// </summary>
        private void DrawImage(Graphics graphics, Rectangle cellBounds, Image image, int imageCount)
        {
            DrawImage(graphics, cellBounds, image, imageCount, false);
        }

        /// <summary>
        /// 绘制图片数量角标
        /// 优化：确保角标在图片区域内，不会覆盖其他单元格
        /// </summary>
        private void DrawImageCountBadge(Graphics graphics, Rectangle imgRect, int count)
        {
            // 角标大小根据图片大小动态调整
            int badgeSize = Math.Min(16, Math.Min(imgRect.Width / 3, imgRect.Height / 3));
            if (badgeSize < 10) badgeSize = 10; // 最小尺寸

            // 角标背景位置（限制在图片区域内）
            var badgeRect = new Rectangle(
                Math.Max(imgRect.X, imgRect.Right - badgeSize - 2),
                Math.Max(imgRect.Y, imgRect.Bottom - badgeSize - 2),
                badgeSize, badgeSize);

            // 确保角标不会超出图片边界
            if (badgeRect.Right > imgRect.Right)
                badgeRect.X = imgRect.Right - badgeSize;
            if (badgeRect.Bottom > imgRect.Bottom)
                badgeRect.Y = imgRect.Bottom - badgeSize;

            // 绘制半透明红色背景
            using (var brush = new SolidBrush(Color.FromArgb(220, 255, 59, 48)))
            {
                graphics.FillEllipse(brush, badgeRect);
            }

            // 角标文字
            using (var brush = new SolidBrush(Color.White))
            {
                // 计算合适的字体大小
                float fontSize = Math.Max(6, badgeSize / 2 - 1);
                Font font = new Font("Arial", fontSize, FontStyle.Bold);
                try
                {
                    string text = $"+{count - 1}";
                    var size = graphics.MeasureString(text, font);

                    // 如果文本太大，调整字体
                    while (size.Width > badgeRect.Width - 2 && fontSize > 6)
                    {
                        fontSize -= 0.5f;
                        font.Dispose();
                        font = new Font("Arial", fontSize, FontStyle.Bold);
                        size = graphics.MeasureString(text, font);
                    }

                    var point = new PointF(
                        badgeRect.Left + (badgeRect.Width - size.Width) / 2,
                        badgeRect.Top + (badgeRect.Height - size.Height) / 2);

                    // 使用TextRenderer确保清晰
                    TextRenderer.DrawText(graphics, text, font,
                        new Point((int)point.X, (int)point.Y), Color.White);
                }
                finally
                {
                    font?.Dispose();
                }
            }
        }

        /// <summary>
        /// 异步加载图片
        /// </summary>
        private async Task<Image> LoadImageAsync(long prodDetailId, string imagePath)
        {
            try
            {
                // 获取图片缓存服务
                var column = this.OwningColumn as SKUImageColumn;
                if (column?.ImageCacheService == null)
                {
                    return null;
                }

                // 从缓存服务获取图片数据
                var images = await column.ImageCacheService.GetSKUImagesAsync(prodDetailId);
                if (images == null || images.Count == 0)
                {
                    return null;
                }

                // 生成缩略图
                using (var ms = new MemoryStream(images[0]))
                {
                    var original = Image.FromStream(ms);
                    return original.GetThumbnailImage(
                        SKUImageColumn.ThumbnailSize,
                        SKUImageColumn.ThumbnailSize,
                        null, IntPtr.Zero);
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 获取产品明细对象
        /// </summary>
        private tb_ProdDetail GetProdDetail(int rowIndex)
        {
            if (this.DataGridView == null || rowIndex < 0 || rowIndex >= this.DataGridView.Rows.Count)
            {
                return null;
            }

            var row = this.DataGridView.Rows[rowIndex];
            return row.DataBoundItem as tb_ProdDetail;
        }

        /// <summary>
        /// 清理资源
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _loadedImage?.Dispose();
                _loadedImage = null;
            }
            base.Dispose(disposing);
        }
    }

    /// <summary>
    /// ImageCacheService扩展方法 - 支持SKU图片
    /// </summary>
    public static class ImageCacheServiceExtensions
    {
        private const string SKUImageCacheKey = "sku_image_";

        /// <summary>
        /// 获取SKU图片（带缓存）
        /// </summary>
        public static async Task<List<byte[]>> GetSKUImagesAsync(this ImageCacheService cacheService, long prodDetailId)
        {
            if (prodDetailId <= 0)
                return new List<byte[]>();

            // 检查是否启用缓存
            if (!IsCacheEnabled(cacheService))
            {
                return await QuerySKUImagesFromDbAsync(prodDetailId);
            }

            // 构建缓存键
            string cacheKey = $"{SKUImageCacheKey}{prodDetailId}";

            // 从缓存或数据库获取
            return await GetOrCreateAsync(cacheService, cacheKey, async () =>
            {
                return await QuerySKUImagesFromDbAsync(prodDetailId);
            });
        }

        /// <summary>
        /// 刷新SKU图片缓存
        /// </summary>
        public static void RefreshSKUImageCache(this ImageCacheService cacheService, long prodDetailId)
        {
            if (prodDetailId <= 0)
                return;

            string cacheKey = $"{SKUImageCacheKey}{prodDetailId}";
            RemoveFromCache(cacheService, cacheKey);
        }

        /// <summary>
        /// 从数据库查询SKU图片
        /// </summary>
        private static async Task<List<byte[]>> QuerySKUImagesFromDbAsync(long prodDetailId)
        {
            try
            {
                // 从数据库获取图片路径
                var db = MainForm.Instance.AppContext.Db.CopyNew();
                var imagesPath = await db.Queryable<tb_ProdDetail>()
                    .Where(p => p.ProdDetailID == prodDetailId)
                    .Select(p => p.ImagesPath)
                    .FirstAsync();

                if (string.IsNullOrEmpty(imagesPath))
                    return new List<byte[]>();

                // 下载图片
                var fileService = Startup.GetFromFac<RUINORERP.UI.Network.Services.FileBusinessService>();
                if (fileService == null)
                    return new List<byte[]>();

                var response = await fileService.DownloadImageAsync(
                    new tb_ProdDetail { ProdDetailID = prodDetailId, ImagesPath = imagesPath },
                    "ImagesPath");

                if (response == null || response.Count == 0)
                    return new List<byte[]>();

                // 转换为byte[]列表
                return response.SelectMany(r => r.FileStorageInfos)
                              .Where(f => f.FileData != null)
                              .Select(f => f.FileData)
                              .ToList();
            }
            catch
            {
                return new List<byte[]>();
            }
        }

        /// <summary>
        /// 获取或创建缓存项
        /// </summary>
        private static async Task<List<byte[]>> GetOrCreateAsync(ImageCacheService cacheService, string cacheKey, Func<Task<List<byte[]>>> factory)
        {
            // 使用反射获取缓存实例
            var cacheField = typeof(ImageCacheService).GetField("_cache", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (cacheField == null)
                return await factory();

            var cache = cacheField.GetValue(cacheService);
            if (cache == null)
                return await factory();

            // 调用GetOrCreateAsync方法
            var method = cache.GetType().GetMethod("GetOrCreateAsync", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            if (method == null)
                return await factory();

            return await (Task<List<byte[]>>)method.Invoke(cache, new object[] { cacheKey, factory, null });
        }

        /// <summary>
        /// 从缓存移除
        /// </summary>
        private static void RemoveFromCache(ImageCacheService cacheService, string cacheKey)
        {
            var cacheField = typeof(ImageCacheService).GetField("_cache", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (cacheField == null)
                return;

            var cache = cacheField.GetValue(cacheService);
            if (cache == null)
                return;

            var method = cache.GetType().GetMethod("Remove");
            if (method != null)
            {
                method.Invoke(cache, new object[] { cacheKey });
            }
        }

        /// <summary>
        /// 检查缓存是否启用
        /// </summary>
        private static bool IsCacheEnabled(ImageCacheService cacheService)
        {
            var prop = typeof(ImageCacheService).GetProperty("IsCacheEnabled", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (prop == null)
                return false;

            return (bool)prop.GetValue(cacheService);
        }
    }
}
