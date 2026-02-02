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
        /// </summary>
        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds,
            int rowIndex, DataGridViewElementStates cellState, object value,
            object formattedValue, string errorText, DataGridViewCellStyle cellStyle,
            DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
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
                DrawPlaceholder(graphics, cellBounds, "无图片");
                return;
            }

            // 解析图片数量
            var imagePaths = imagesPath.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            if (imagePaths.Length == 0)
            {
                DrawPlaceholder(graphics, cellBounds, "无图片");
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
                DrawPlaceholder(graphics, cellBounds, "双击添加图片");
            }
        }

        /// <summary>
        /// 绘制占位符文本
        /// </summary>
        private void DrawPlaceholder(Graphics graphics, Rectangle cellBounds, string text)
        {
            using (var brush = new SolidBrush(Color.Gray))
            using (var font = new Font("Microsoft YaHei", 8))
            {
                var size = graphics.MeasureString(text, font);
                var point = new PointF(
                    cellBounds.Left + (cellBounds.Width - size.Width) / 2,
                    cellBounds.Top + (cellBounds.Height - size.Height) / 2);
                graphics.DrawString(text, font, brush, point);
            }
        }

        /// <summary>
        /// 绘制图片
        /// </summary>
        private void DrawImage(Graphics graphics, Rectangle cellBounds, Image image, int imageCount, bool hasUnsavedChanges)
        {
            // 计算缩略图大小和位置
            int thumbnailSize = SKUImageColumn.ThumbnailSize;
            var imgRect = new Rectangle(
                cellBounds.Left + (cellBounds.Width - thumbnailSize) / 2,
                cellBounds.Top + (cellBounds.Height - thumbnailSize) / 2,
                thumbnailSize, thumbnailSize);

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
                    // 计算缩略图大小和位置
                    int thumbnailSize = SKUImageColumn.ThumbnailSize;
                    var imgRect = new Rectangle(
                        cellBounds.Left + (cellBounds.Width - thumbnailSize) / 2,
                        cellBounds.Top + (cellBounds.Height - thumbnailSize) / 2,
                        thumbnailSize, thumbnailSize);

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
        /// </summary>
        private void DrawUnsavedBadge(Graphics graphics, Rectangle cellBounds)
        {
            using (var brush = new SolidBrush(Color.Orange))
            using (var font = new Font("Arial", 10, FontStyle.Bold))
            {
                string text = "*";
                var size = graphics.MeasureString(text, font);
                graphics.DrawString(text, font, brush,
                    new PointF(cellBounds.Right - size.Width - 4, cellBounds.Top + 2));
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
        /// </summary>
        private void DrawImageCountBadge(Graphics graphics, Rectangle imgRect, int count)
        {
            // 角标背景
            var badgeRect = new Rectangle(
                imgRect.Right - 18,
                imgRect.Bottom - 18,
                16, 16);

            using (var brush = new SolidBrush(Color.Red))
            {
                graphics.FillEllipse(brush, badgeRect);
            }

            // 角标文字
            using (var brush = new SolidBrush(Color.White))
            using (var font = new Font("Arial", 7, FontStyle.Bold))
            {
                string text = $"+{count - 1}";
                var size = graphics.MeasureString(text, font);
                var point = new PointF(
                    badgeRect.Left + (badgeRect.Width - size.Width) / 2,
                    badgeRect.Top + (badgeRect.Height - size.Height) / 2);
                graphics.DrawString(text, font, brush, point);
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
