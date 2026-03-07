using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace RUINOR.WinFormsUI.CustomPictureBox.Implementations
{
    /// <summary>
    /// 图片处理器
    /// 提供图片格式转换、压缩、缩放、裁剪、旋转等功能
    /// 注意：本类方法返回的Bitmap需要由调用方负责释放
    /// </summary>
    public class ImageProcessor : IDisposable
    {
        private bool _disposed = false;
        private static readonly ImageCodecInfo _jpegCodec;
        private static readonly EncoderParameters _encoderParamsDefault;

        static ImageProcessor()
        {
            // 静态初始化Jpeg编码器，提升性能
            _jpegCodec = GetEncoderInfo(ImageFormat.Jpeg);
            _encoderParamsDefault = new EncoderParameters(1);
            _encoderParamsDefault.Param[0] = new EncoderParameter(Encoder.Quality, ImageProcessingConstants.DefaultJpegQuality);
        }
        /// <summary>
        /// 将Image对象转换为字节数组
        /// </summary>
        /// <param name="image">要转换的Image对象</param>
        /// <returns>转换后的字节数组</returns>
        /// <exception cref="ArgumentNullException">当image为null时抛出</exception>
        public byte[] ImageToBytes(Image image)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image), "图片对象不能为空");

            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, image.RawFormat);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// 将字节数组转换为Image对象
        /// </summary>
        /// <param name="bytes">要转换的字节数组</param>
        /// <returns>转换后的Image对象，调用方需负责释放</returns>
        /// <exception cref="ArgumentNullException">当bytes为null或空时抛出</exception>
        public Image BytesToImage(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
                throw new ArgumentNullException(nameof(bytes), "字节数组不能为空");

            using (MemoryStream ms = new MemoryStream(bytes))
            {
                return Image.FromStream(ms);
            }
        }

        /// <summary>
        /// 压缩图片
        /// </summary>
        /// <param name="image">原始图片</param>
        /// <param name="quality">压缩质量(0-100)</param>
        /// <returns>压缩后的Bitmap对象，调用方需负责释放</returns>
        /// <exception cref="ArgumentNullException">当image为null时抛出</exception>
        public Bitmap CompressImage(Image image, int quality = 80)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image), "图片对象不能为空");

            // 确保质量值在有效范围内
            quality = Math.Max(0, Math.Min(100, quality));

            // 获取或创建编码器参数
            EncoderParameters encoderParams;
            if (quality == ImageProcessingConstants.DefaultJpegQuality)
            {
                encoderParams = _encoderParamsDefault;
            }
            else
            {
                encoderParams = new EncoderParameters(1);
                encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, quality);
            }

            // 创建压缩后的图片流
            using (MemoryStream ms = new MemoryStream())
            {
                if (_jpegCodec != null)
                {
                    image.Save(ms, _jpegCodec, encoderParams);
                }
                else
                {
                    image.Save(ms, ImageFormat.Jpeg);
                }
                ms.Seek(0, SeekOrigin.Begin);
                var compressed = (Bitmap)Image.FromStream(ms);

                // 如果创建的是临时的编码器参数，需要释放
                if (encoderParams != _encoderParamsDefault)
                {
                    encoderParams.Dispose();
                }

                return compressed;
            }
        }

        /// <summary>
        /// 缩放图片
        /// </summary>
        /// <param name="image">原始图片</param>
        /// <param name="newWidth">新宽度</param>
        /// <param name="newHeight">新高度</param>
        /// <returns>缩放后的Bitmap对象，调用方需负责释放</returns>
        /// <exception cref="ArgumentNullException">当image为null时抛出</exception>
        public Bitmap ResizeImage(Image image, int newWidth, int newHeight)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image), "图片对象不能为空");

            // 计算宽高比，保持图片比例
            double aspectRatio = (double)image.Width / image.Height;
            int targetWidth = newWidth;
            int targetHeight = newHeight;

            if (newWidth > 0 && newHeight > 0)
            {
                double targetRatio = (double)newWidth / newHeight;
                if (aspectRatio > targetRatio)
                {
                    targetHeight = (int)(newWidth / aspectRatio);
                }
                else
                {
                    targetWidth = (int)(newHeight * aspectRatio);
                }
            }
            else if (newWidth > 0)
            {
                targetHeight = (int)(newWidth / aspectRatio);
            }
            else if (newHeight > 0)
            {
                targetWidth = (int)(newHeight * aspectRatio);
            }
            else
            {
                // 如果宽高都为0或负数，返回原图的副本
                return new Bitmap(image);
            }

            // 确保尺寸至少为1
            targetWidth = Math.Max(ImageProcessingConstants.MinimumImageSize, targetWidth);
            targetHeight = Math.Max(ImageProcessingConstants.MinimumImageSize, targetHeight);

            // 创建缩放后的图片
            Bitmap resizedBitmap = new Bitmap(targetWidth, targetHeight);
            using (Graphics g = Graphics.FromImage(resizedBitmap))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.DrawImage(image, 0, 0, targetWidth, targetHeight);
            }

            return resizedBitmap;
        }

        /// <summary>
        /// 裁剪图片
        /// </summary>
        /// <param name="image">原始图片</param>
        /// <param name="cropRect">裁剪区域</param>
        /// <returns>裁剪后的Bitmap对象，调用方需负责释放</returns>
        /// <exception cref="ArgumentNullException">当image为null时抛出</exception>
        /// <exception cref="ArgumentException">当裁剪区域无效时抛出</exception>
        public Bitmap CropImage(Image image, Rectangle cropRect)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image), "图片对象不能为空");

            // 确保裁剪区域有效
            Rectangle validCropRect = new Rectangle(
                Math.Max(0, cropRect.X),
                Math.Max(0, cropRect.Y),
                Math.Max(ImageProcessingConstants.MinimumImageSize, Math.Min(image.Width - cropRect.X, cropRect.Width)),
                Math.Max(ImageProcessingConstants.MinimumImageSize, Math.Min(image.Height - cropRect.Y, cropRect.Height)));

            // 创建裁剪后的图片
            Bitmap croppedBitmap = new Bitmap(validCropRect.Width, validCropRect.Height);
            using (Graphics g = Graphics.FromImage(croppedBitmap))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(image, new Rectangle(0, 0, validCropRect.Width, validCropRect.Height), validCropRect, GraphicsUnit.Pixel);
            }

            return croppedBitmap;
        }

        /// <summary>
        /// 旋转图片
        /// </summary>
        /// <param name="image">原始图片</param>
        /// <param name="angle">旋转角度(度)</param>
        /// <returns>旋转后的Bitmap对象，调用方需负责释放</returns>
        /// <exception cref="ArgumentNullException">当image为null时抛出</exception>
        public Bitmap RotateImage(Image image, float angle)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image), "图片对象不能为空");

            // 计算旋转后的图片尺寸
            double radians = angle * Math.PI / 180;
            double cos = Math.Abs(Math.Cos(radians));
            double sin = Math.Abs(Math.Sin(radians));
            int newWidth = (int)(image.Width * cos + image.Height * sin);
            int newHeight = (int)(image.Width * sin + image.Height * cos);

            // 确保尺寸至少为1
            newWidth = Math.Max(ImageProcessingConstants.MinimumImageSize, newWidth);
            newHeight = Math.Max(ImageProcessingConstants.MinimumImageSize, newHeight);

            // 创建旋转后的图片
            Bitmap rotatedBitmap = new Bitmap(newWidth, newHeight);
            using (Graphics g = Graphics.FromImage(rotatedBitmap))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.TranslateTransform(newWidth / 2f, newHeight / 2f);
                g.RotateTransform(angle);
                g.TranslateTransform(-image.Width / 2f, -image.Height / 2f);
                g.DrawImage(image, 0, 0);
            }

            return rotatedBitmap;
        }

        /// <summary>
        /// 获取图片格式对应的扩展名
        /// </summary>
        /// <param name="format">图片格式</param>
        /// <returns>扩展名（不包含点号）</returns>
        /// <exception cref="ArgumentNullException">当format为null时抛出</exception>
        public string GetFormatExtension(ImageFormat format)
        {
            return ImageProcessingConstants.GetFormatExtension(format).TrimStart('.');
        }

        /// <summary>
        /// 从文件加载图片
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>加载的Image对象，调用方需负责释放</returns>
        /// <exception cref="ArgumentNullException">当filePath为null或空时抛出</exception>
        /// <exception cref="FileNotFoundException">当文件不存在时抛出</exception>
        public Image LoadImageFromFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException(nameof(filePath), "文件路径不能为空");

            if (!File.Exists(filePath))
                throw new FileNotFoundException("文件不存在", filePath);

            // 使用FromFile，不使用using，调用方负责释放
            return Image.FromFile(filePath);
        }

        /// <summary>
        /// 保存图片到文件
        /// </summary>
        /// <param name="image">要保存的图片</param>
        /// <param name="filePath">文件路径</param>
        /// <param name="format">图片格式</param>
        /// <exception cref="ArgumentNullException">当image或filePath为null时抛出</exception>
        public void SaveImageToFile(Image image, string filePath, ImageFormat format)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image), "图片对象不能为空");

            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException(nameof(filePath), "文件路径不能为空");

            if (format == null)
                throw new ArgumentNullException(nameof(format), "图片格式不能为空");

            // 确保目录存在
            string directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            image.Save(filePath, format);
        }

        /// <summary>
        /// 获取指定图片格式的编码器
        /// </summary>
        /// <param name="format">图片格式</param>
        /// <returns>编码器信息</returns>
        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取指定图片格式的编码器（静态辅助方法）
        /// </summary>
        /// <param name="format">图片格式</param>
        /// <returns>编码器信息</returns>
        private static ImageCodecInfo GetEncoderInfo(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="disposing">是否正在释放托管资源</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // 释放托管资源
                    // 静态资源在类卸载时自动释放
                }
                _disposed = true;
            }
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~ImageProcessor()
        {
            Dispose(false);
        }
    }
}