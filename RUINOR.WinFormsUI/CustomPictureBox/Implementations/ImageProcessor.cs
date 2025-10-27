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
    /// </summary>
    public class ImageProcessor
    {
        /// <summary>
        /// 将Image对象转换为字节数组
        /// </summary>
        /// <param name="image">要转换的Image对象</param>
        /// <returns>转换后的字节数组</returns>
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
        /// <returns>转换后的Image对象</returns>
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
        /// <returns>压缩后的图片</returns>
        public Image CompressImage(Image image, int quality = 80)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image), "图片对象不能为空");

            // 确保质量值在有效范围内
            quality = Math.Max(0, Math.Min(100, quality));

            // 获取编码器信息
            ImageCodecInfo jpegCodec = GetEncoder(ImageFormat.Jpeg);
            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, quality);

            // 创建压缩后的图片流
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, jpegCodec, encoderParams);
                ms.Seek(0, SeekOrigin.Begin);
                return Image.FromStream(ms);
            }
        }

        /// <summary>
        /// 缩放图片
        /// </summary>
        /// <param name="image">原始图片</param>
        /// <param name="newWidth">新宽度</param>
        /// <param name="newHeight">新高度</param>
        /// <returns>缩放后的图片</returns>
        public Image ResizeImage(Image image, int newWidth, int newHeight)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image), "图片对象不能为空");

            // 计算宽高比，保持图片比例
            double aspectRatio = (double)image.Width / image.Height;
            if (newWidth > 0 && newHeight > 0)
            {
                double targetRatio = (double)newWidth / newHeight;
                if (aspectRatio > targetRatio)
                {
                    newHeight = (int)(newWidth / aspectRatio);
                }
                else
                {
                    newWidth = (int)(newHeight * aspectRatio);
                }
            }
            else if (newWidth > 0)
            {
                newHeight = (int)(newWidth / aspectRatio);
            }
            else if (newHeight > 0)
            {
                newWidth = (int)(newHeight * aspectRatio);
            }

            // 创建缩放后的图片
            Bitmap resizedBitmap = new Bitmap(newWidth, newHeight);
            using (Graphics g = Graphics.FromImage(resizedBitmap))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(image, 0, 0, newWidth, newHeight);
            }

            return resizedBitmap;
        }

        /// <summary>
        /// 裁剪图片
        /// </summary>
        /// <param name="image">原始图片</param>
        /// <param name="cropRect">裁剪区域</param>
        /// <returns>裁剪后的图片</returns>
        public Image CropImage(Image image, Rectangle cropRect)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image), "图片对象不能为空");

            // 确保裁剪区域有效
            Rectangle validCropRect = new Rectangle(
                Math.Max(0, cropRect.X),
                Math.Max(0, cropRect.Y),
                Math.Min(image.Width - cropRect.X, cropRect.Width),
                Math.Min(image.Height - cropRect.Y, cropRect.Height));

            // 创建裁剪后的图片
            Bitmap croppedBitmap = new Bitmap(validCropRect.Width, validCropRect.Height);
            using (Graphics g = Graphics.FromImage(croppedBitmap))
            {
                g.DrawImage(image, new Rectangle(0, 0, validCropRect.Width, validCropRect.Height), validCropRect, GraphicsUnit.Pixel);
            }

            return croppedBitmap;
        }

        /// <summary>
        /// 旋转图片
        /// </summary>
        /// <param name="image">原始图片</param>
        /// <param name="angle">旋转角度(度)</param>
        /// <returns>旋转后的图片</returns>
        public Image RotateImage(Image image, float angle)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image), "图片对象不能为空");

            // 计算旋转后的图片尺寸
            double radians = angle * Math.PI / 180;
            double cos = Math.Abs(Math.Cos(radians));
            double sin = Math.Abs(Math.Sin(radians));
            int newWidth = (int)(image.Width * cos + image.Height * sin);
            int newHeight = (int)(image.Width * sin + image.Height * cos);

            // 创建旋转后的图片
            Bitmap rotatedBitmap = new Bitmap(newWidth, newHeight);
            using (Graphics g = Graphics.FromImage(rotatedBitmap))
            {
                g.TranslateTransform(newWidth / 2, newHeight / 2);
                g.RotateTransform(angle);
                g.TranslateTransform(-image.Width / 2, -image.Height / 2);
                g.DrawImage(image, 0, 0);
            }

            return rotatedBitmap;
        }

        /// <summary>
        /// 获取图片格式对应的扩展名
        /// </summary>
        /// <param name="format">图片格式</param>
        /// <returns>扩展名</returns>
        public string GetFormatExtension(ImageFormat format)
        {
            if (format == ImageFormat.Jpeg)
                return "jpg";
            if (format == ImageFormat.Png)
                return "png";
            if (format == ImageFormat.Gif)
                return "gif";
            if (format == ImageFormat.Bmp)
                return "bmp";
            if (format == ImageFormat.Tiff)
                return "tiff";
            return "jpg"; // 默认返回jpg
        }

        /// <summary>
        /// 从文件加载图片
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>加载的图片</returns>
        public Image LoadImageFromFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException(nameof(filePath), "文件路径不能为空");

            if (!File.Exists(filePath))
                throw new FileNotFoundException("文件不存在", filePath);

            return Image.FromFile(filePath);
        }

        /// <summary>
        /// 保存图片到文件
        /// </summary>
        /// <param name="image">要保存的图片</param>
        /// <param name="filePath">文件路径</param>
        /// <param name="format">图片格式</param>
        public void SaveImageToFile(Image image, string filePath, ImageFormat format)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image), "图片对象不能为空");

            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException(nameof(filePath), "文件路径不能为空");

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
        private ImageCodecInfo GetEncoder(ImageFormat format)
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
    }
}