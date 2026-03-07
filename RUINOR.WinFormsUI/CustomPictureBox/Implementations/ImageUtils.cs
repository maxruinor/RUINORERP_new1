using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace RUINOR.WinFormsUI.CustomPictureBox.Implementations
{
    /// <summary>
    /// 图片工具类
    /// 提供通用的图片处理辅助方法
    /// </summary>
    public static class ImageUtils
    {
        /// <summary>
        /// 获取图片格式的扩展名
        /// </summary>
        /// <param name="imageFormat">图片格式</param>
        /// <returns>扩展名（包含点号）</returns>
        /// <exception cref="ArgumentNullException">当imageFormat为null时抛出</exception>
        public static string GetFormatExtension(ImageFormat imageFormat)
        {
            return ImageProcessingConstants.GetFormatExtension(imageFormat);
        }

        /// <summary>
        /// 计算调整后的图片尺寸，保持宽高比
        /// </summary>
        /// <param name="originalSize">原始尺寸</param>
        /// <param name="maxWidth">最大宽度</param>
        /// <param name="maxHeight">最大高度</param>
        /// <returns>调整后的尺寸</returns>
        public static Size CalculateResizedDimensions(Size originalSize, int maxWidth, int maxHeight)
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
                Math.Max(ImageProcessingConstants.MinimumImageSize, (int)(originalSize.Width * scaleRatio)),
                Math.Max(ImageProcessingConstants.MinimumImageSize, (int)(originalSize.Height * scaleRatio)));
        }

        /// <summary>
        /// 安全地释放图片资源
        /// </summary>
        /// <param name="image">要释放的图片（引用参数）</param>
        /// <exception cref="System.NullReferenceException">释放时可能抛出</exception>
        public static void SafeDispose(ref Image image)
        {
            if (image != null)
            {
                Image imageToDispose = image;
                image = null;

                try
                {
                    imageToDispose.Dispose();
                }
                catch (Exception)
                {
                    // 忽略释放过程中的异常
                }
            }
        }

        /// <summary>
        /// 检查文件是否为有效的图片文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>是否为有效图片</returns>
        public static bool IsValidImageFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                return false;

            try
            {
                using (var image = Image.FromFile(filePath))
                {
                    // 如果能成功加载，则是有效的图片
                    return image.Width > 0 && image.Height > 0;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取图片文件的格式
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>图片格式</returns>
        /// <exception cref="ArgumentNullException">当filePath为null或空时抛出</exception>
        /// <exception cref="FileNotFoundException">当文件不存在时抛出</exception>
        public static ImageFormat GetImageFormat(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return ImageFormat.Jpeg;

            if (!File.Exists(filePath))
                return ImageFormat.Jpeg;

            try
            {
                using (var image = Image.FromFile(filePath))
                {
                    return image.RawFormat;
                }
            }
            catch
            {
                return ImageFormat.Jpeg;
            }
        }

        /// <summary>
        /// 生成安全的文件名
        /// </summary>
        /// <param name="baseName">基础文件名</param>
        /// <returns>安全的文件名</returns>
        public static string GenerateSafeFileName(string baseName)
        {
            if (string.IsNullOrEmpty(baseName))
                throw new ArgumentNullException(nameof(baseName), "基础文件名不能为空");

            // 移除或替换文件名中的非法字符
            string invalidChars = new string(Path.GetInvalidFileNameChars());
            string safeName = baseName;

            foreach (char c in invalidChars)
            {
                safeName = safeName.Replace(c, '_');
            }

            return safeName;
        }
    }
}