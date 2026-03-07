using System.Drawing.Imaging;

namespace RUINOR.WinFormsUI.CustomPictureBox.Implementations
{
    /// <summary>
    /// 图片处理常量类
    /// 统一管理图片处理相关的常量值
    /// </summary>
    public static class ImageProcessingConstants
    {
        /// <summary>
        /// 图片更新标记
        /// </summary>
        public const string UpdateMarker = "NEEDS_UPDATE";

        /// <summary>
        /// 默认JPEG压缩质量（0-100）
        /// </summary>
        public const long DefaultJpegQuality = 80;

        /// <summary>
        /// 默认缓存最大容量
        /// </summary>
        public const int DefaultMaxCacheCapacity = 100;

        /// <summary>
        /// 最小图片尺寸
        /// </summary>
        public const int MinimumImageSize = 1;

        /// <summary>
        /// 默认图片格式（当无法识别时的回退格式）
        /// </summary>
        public const string DefaultFileExtension = ".jpg";

        /// <summary>
        /// JPEG图片格式扩展名
        /// </summary>
        public const string JpegExtension = ".jpg";

        /// <summary>
        /// PNG图片格式扩展名
        /// </summary>
        public const string PngExtension = ".png";

        /// <summary>
        /// GIF图片格式扩展名
        /// </summary>
        public const string GifExtension = ".gif";

        /// <summary>
        /// BMP图片格式扩展名
        /// </summary>
        public const string BmpExtension = ".bmp";

        /// <summary>
        /// TIFF图片格式扩展名
        /// </summary>
        public const string TiffExtension = ".tiff";

        /// <summary>
        /// 获取图片格式对应的扩展名
        /// </summary>
        /// <param name="format">图片格式</param>
        /// <returns>扩展名（包含点号）</returns>
        public static string GetFormatExtension(ImageFormat format)
        {
            if (format == null)
                return DefaultFileExtension;

            if (format.Equals(ImageFormat.Jpeg))
                return JpegExtension;
            else if (format.Equals(ImageFormat.Png))
                return PngExtension;
            else if (format.Equals(ImageFormat.Gif))
                return GifExtension;
            else if (format.Equals(ImageFormat.Bmp))
                return BmpExtension;
            else if (format.Equals(ImageFormat.Tiff))
                return TiffExtension;
            else
                return DefaultFileExtension;
        }
    }
}
