using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace RUINOR.WinFormsUI.CustomPictureBox.Implementations
{
    /// <summary>
    /// 优化的图片加载器
    /// 提供高性能的图片加载功能，支持缓存和异步加载
    /// </summary>
    public class OptimizedImageLoader
    {
        // 图片缓存
        private readonly LRUImageCache _cache;
        // 图片处理器
        private readonly ImageProcessor _imageProcessor;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cache">图片缓存</param>
        /// <param name="imageProcessor">图片处理器</param>
        public OptimizedImageLoader(LRUImageCache cache, ImageProcessor imageProcessor)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _imageProcessor = imageProcessor ?? throw new ArgumentNullException(nameof(imageProcessor));
        }

        /// <summary>
        /// 从文件加载图片
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>加载的图片</returns>
        public Image LoadFromFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException(nameof(filePath), "文件路径不能为空");

            // 生成缓存键
            string cacheKey = _cache.GenerateCacheKey(filePath);

            // 尝试从缓存获取
            Image cachedImage = _cache.Get(cacheKey);
            if (cachedImage != null)
                return cachedImage;

            try
            {
                // 从文件加载图片
                Image image = _imageProcessor.LoadImageFromFile(filePath);

                // 添加到缓存
                _cache.Add(cacheKey, image);

                return image;
            }
            catch (Exception ex)
            {
                throw new Exception($"加载图片失败: {filePath}", ex);
            }
        }

        /// <summary>
        /// 异步从文件加载图片
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>加载的图片任务</returns>
        public async Task<Image> LoadFromFileAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException(nameof(filePath), "文件路径不能为空");

            // 生成缓存键
            string cacheKey = _cache.GenerateCacheKey(filePath);

            // 尝试从缓存获取
            Image cachedImage = _cache.Get(cacheKey);
            if (cachedImage != null)
                return cachedImage;

            try
            {
                // 异步加载图片
                Image image = await Task.Run(() =>
                {
                    return _imageProcessor.LoadImageFromFile(filePath);
                });

                // 添加到缓存
                _cache.Add(cacheKey, image);

                return image;
            }
            catch (Exception ex)
            {
                throw new Exception($"异步加载图片失败: {filePath}", ex);
            }
        }

        /// <summary>
        /// 从字节数组加载图片
        /// </summary>
        /// <param name="imageBytes">图片字节数组</param>
        /// <returns>加载的图片</returns>
        public Image LoadFromBytes(byte[] imageBytes)
        {
            if (imageBytes == null || imageBytes.Length == 0)
                throw new ArgumentNullException(nameof(imageBytes), "图片字节数组不能为空");

            // 生成缓存键
            string cacheKey = _cache.GenerateCacheKey(BitConverter.ToString(imageBytes));

            // 尝试从缓存获取
            Image cachedImage = _cache.Get(cacheKey);
            if (cachedImage != null)
                return cachedImage;

            try
            {
                // 从字节数组加载图片
                Image image = _imageProcessor.BytesToImage(imageBytes);

                // 添加到缓存
                _cache.Add(cacheKey, image);

                return image;
            }
            catch (Exception ex)
            {
                throw new Exception("从字节数组加载图片失败", ex);
            }
        }

        /// <summary>
        /// 加载并调整图片大小
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="maxWidth">最大宽度</param>
        /// <param name="maxHeight">最大高度</param>
        /// <returns>调整大小后的图片</returns>
        public Image LoadAndResize(string filePath, int maxWidth, int maxHeight)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException(nameof(filePath), "文件路径不能为空");

            // 生成带尺寸的缓存键
            string cacheKey = _cache.GenerateCacheKey($"{filePath}_{maxWidth}_{maxHeight}");

            // 尝试从缓存获取
            Image cachedImage = _cache.Get(cacheKey);
            if (cachedImage != null)
                return cachedImage;

            try
            {
                // 加载原图
                Image originalImage = LoadFromFile(filePath);

                // 计算新尺寸
                Size newSize = CalculateNewSize(originalImage.Size, maxWidth, maxHeight);

                // 调整大小
                Image resizedImage = _imageProcessor.ResizeImage(originalImage, newSize.Width, newSize.Height);

                // 添加到缓存
                _cache.Add(cacheKey, resizedImage);

                return resizedImage;
            }
            catch (Exception ex)
            {
                throw new Exception($"加载并调整图片大小失败: {filePath}", ex);
            }
        }

        /// <summary>
        /// 使缓存失效
        /// </summary>
        /// <param name="filePath">文件路径</param>
        public void InvalidateCache(string filePath)
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                string cacheKey = _cache.GenerateCacheKey(filePath);
                _cache.Remove(cacheKey);
            }
        }

        /// <summary>
        /// 清空缓存
        /// </summary>
        public void ClearCache()
        {
            _cache.Clear();
        }

        /// <summary>
        /// 计算调整后的图片尺寸
        /// </summary>
        /// <param name="originalSize">原始尺寸</param>
        /// <param name="maxWidth">最大宽度</param>
        /// <param name="maxHeight">最大高度</param>
        /// <returns>调整后的尺寸</returns>
        private Size CalculateNewSize(Size originalSize, int maxWidth, int maxHeight)
        {
            if (maxWidth <= 0 && maxHeight <= 0)
                return originalSize;

            double widthRatio = 1.0;
            double heightRatio = 1.0;

            if (maxWidth > 0 && originalSize.Width > maxWidth)
                widthRatio = (double)maxWidth / originalSize.Width;

            if (maxHeight > 0 && originalSize.Height > maxHeight)
                heightRatio = (double)maxHeight / originalSize.Height;

            // 使用较小的缩放比例，保证图片完全在限制范围内
            double scaleRatio = Math.Min(widthRatio, heightRatio);

            return new Size(
                (int)(originalSize.Width * scaleRatio),
                (int)(originalSize.Height * scaleRatio));
        }
    }
}