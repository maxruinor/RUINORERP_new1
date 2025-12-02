using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SourceGrid.Cells.Editors
{
    /// <summary>
    /// 图片处理器
    /// 提供图片压缩、格式转换、尺寸调整等功能
    /// 专注于图片处理本身，不涉及业务逻辑
    /// </summary>
    public static class ImageProcessor
    {
        #region 配置常量

        /// <summary>
        /// 默认最大文件大小（2MB）
        /// </summary>
        public const long DefaultMaxFileSize = 2 * 1024 * 1024;
        
        /// <summary>
        /// 默认最大图片尺寸（2048x2048）
        /// </summary>
        public const int DefaultMaxDimension = 2048;
        
        /// <summary>
        /// 默认JPEG质量
        /// </summary>
        public const long DefaultQuality = 85L;

        #endregion

        #region 配置属性

        /// <summary>
        /// 最大文件大小（字节）
        /// </summary>
        public static long MaxFileSize { get; set; } = DefaultMaxFileSize;

        /// <summary>
        /// 最大图片尺寸（像素）
        /// </summary>
        public static int MaxDimension { get; set; } = DefaultMaxDimension;

        /// <summary>
        /// JPEG压缩质量（1-100）
        /// </summary>
        public static long Quality { get; set; } = DefaultQuality;

        /// <summary>
        /// 是否启用智能压缩
        /// </summary>
        public static bool EnableSmartCompression { get; set; } = true;

        #endregion

        #region 基础压缩方法

        /// <summary>
        /// 从文件路径压缩图片
        /// </summary>
        /// <param name="filePath">图片文件路径</param>
        /// <returns>压缩后的图片字节数据</returns>
        public static byte[] CompressImage(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("图片文件不存在", filePath);
            }

            using (var image = System.Drawing.Image.FromFile(filePath))
            {
                return CompressImage(image);
            }
        }
 
        /// <summary>
        /// 压缩图片对象
        /// 增强版：支持智能压缩、多格式处理
        /// </summary>
        /// <param name="image">原始图片对象</param>
        /// <returns>压缩后的图片字节数据</returns>
        public static byte[] CompressImage(System.Drawing.Image image)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image));

            using (var ms = new MemoryStream())
            {
                try
                {
                    // 1. 验证图片尺寸
                    if (image.Width <= 0 || image.Height <= 0)
                    {
                        throw new ArgumentException("无效的图片尺寸");
                    }

                    // 2. 预处理图片（调整尺寸等）
                    var processedImage = PreprocessImage(image);

                    // 3. 智能压缩
                    if (EnableSmartCompression)
                    {
                        return SmartCompress(processedImage, ms);
                    }
                    else
                    {
                        // 标准压缩
                        var encoderInfo = GetEncoderInfo(ImageFormat.Jpeg);
                        if (encoderInfo != null)
                        {
                            var encoderParams = GetEncoderParameters(Quality);
                            processedImage.Save(ms, encoderInfo, encoderParams);
                        }
                        else
                        {
                            processedImage.Save(ms, ImageFormat.Jpeg);
                        }
                        return ms.ToArray();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"压缩图片时出错: {ex.Message}");
                    throw new InvalidOperationException("图片压缩失败", ex);
                }
                finally
                {
                    ms.Flush();
                }
            }
        }

        /// <summary>
        /// 压缩图片字节数组
        /// </summary>
        /// <param name="imageBytes">原始图片字节数据</param>
        /// <param name="format">目标格式，默认为JPEG</param>
        /// <returns>压缩后的字节数据</returns>
        public static byte[] CompressImage(byte[] imageBytes, ImageFormat format = null)
        {
            if (imageBytes == null || imageBytes.Length == 0)
                throw new ArgumentException("图片字节数据无效");

            using (var ms = new MemoryStream(imageBytes))
            using (var image = System.Drawing.Image.FromStream(ms))
            {
                return CompressImage(image);
            }
        }

        #endregion

        #region 高级压缩方法

        /// <summary>
        /// 智能压缩：根据图片特征选择最佳压缩策略
        /// </summary>
        /// <param name="image">要压缩的图片</param>
        /// <param name="outputStream">输出流</param>
        /// <returns>压缩后的字节数据</returns>
        private static byte[] SmartCompress(System.Drawing.Image image, MemoryStream outputStream)
        {
            // 计算图片特征
            var imageSize = image.Width * image.Height;
            var aspectRatio = (float)image.Width / image.Height;

            // 根据图片特征调整压缩参数
            long quality = CalculateOptimalQuality(image, imageSize);
            ImageFormat outputFormat = DetermineOptimalFormat(image, imageSize, aspectRatio);

            // 执行压缩
            var encoderInfo = GetEncoderInfo(outputFormat);
            if (encoderInfo != null)
            {
                var encoderParams = GetEncoderParameters(quality);
                image.Save(outputStream, encoderInfo, encoderParams);
            }
            else
            {
                image.Save(outputStream, outputFormat);
            }
            return outputStream.ToArray();
        }

        /// <summary>
        /// 计算最优压缩质量
        /// </summary>
        /// <param name="image">图片对象</param>
        /// <param name="imageSize">图片大小（像素数）</param>
        /// <returns>最优质量值</returns>
        private static long CalculateOptimalQuality(System.Drawing.Image image, int imageSize)
        {
            // 大图片使用较低质量，小图片保持高质量
            if (imageSize > 4_000_000) // 4MP以上
                return Math.Max(60L, Quality - 25L);
            else if (imageSize > 1_000_000) // 1MP-4MP
                return Math.Max(70L, Quality - 15L);
            else
                return Quality; // 小图片保持原质量
        }

        /// <summary>
        /// 确定最优输出格式
        /// </summary>
        /// <param name="image">图片对象</param>
        /// <param name="imageSize">图片大小</param>
        /// <param name="aspectRatio">宽高比</param>
        /// <returns>最优格式</returns>
        private static ImageFormat DetermineOptimalFormat(System.Drawing.Image image, int imageSize, float aspectRatio)
        {
            // 如果原图是PNG且包含透明度，保持PNG格式
            if (image.RawFormat == ImageFormat.Png && HasTransparency(image))
            {
                return ImageFormat.Png;
            }

            // 其他情况统一使用JPEG以获得更好的压缩比
            return ImageFormat.Jpeg;
        }

        /// <summary>
        /// 检查图片是否包含透明度
        /// </summary>
        /// <param name="image">图片对象</param>
        /// <returns>是否包含透明度</returns>
        private static bool HasTransparency(System.Drawing.Image image)
        {
            if (image.RawFormat != ImageFormat.Png)
                return false;

            var bitmap = new Bitmap(image);
            try
            {
                // 检查alpha通道
                for (int y = 0; y < bitmap.Height; y++)
                {
                    for (int x = 0; x < bitmap.Width; x++)
                    {
                        var pixel = bitmap.GetPixel(x, y);
                        if (pixel.A < 255)
                            return true;
                    }
                }
                return false;
            }
            finally
            {
                bitmap.Dispose();
            }
        }

        /// <summary>
        /// 图片预处理
        /// </summary>
        /// <param name="image">原始图片</param>
        /// <returns>预处理后的图片</returns>
        private static System.Drawing.Image PreprocessImage(System.Drawing.Image image)
        {
            // 检查是否需要调整尺寸
            if (image.Width <= MaxDimension && image.Height <= MaxDimension)
                return image;

            // 计算新尺寸，保持宽高比
            float ratioX = (float)MaxDimension / image.Width;
            float ratioY = (float)MaxDimension / image.Height;
            float ratio = Math.Min(ratioX, ratioY);

            int newWidth = (int)(image.Width * ratio);
            int newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);
            using (var graphics = Graphics.FromImage(newImage))
            {
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.CompositingQuality = CompositingQuality.HighQuality;

                graphics.DrawImage(image, 0, 0, newWidth, newHeight);
            }

            return newImage;
        }

        #endregion

        #region 图片转换方法

        /// <summary>
        /// 从字节数组显示图片到PictureBox
        /// </summary>
        /// <param name="imageBytes">图片字节数组</param>
        /// <param name="pictureBox">PictureBox控件</param>
        public static void DisplayImageFromBytes(byte[] imageBytes, PictureBox pictureBox)
        {
            if (imageBytes == null || imageBytes.Length == 0)
            {
                throw new ArgumentException("Image bytes cannot be null or empty.", nameof(imageBytes));
            }

            if (pictureBox == null)
            {
                throw new ArgumentNullException(nameof(pictureBox));
            }

            try
            {
                using (MemoryStream ms = new MemoryStream(imageBytes))
                {
                    var image = System.Drawing.Image.FromStream(ms);
                    
                    // 释放原有图片
                    if (pictureBox.Image != null)
                    {
                        pictureBox.Image.Dispose();
                    }
                    
                    pictureBox.Image = image;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("显示图片失败", ex);
            }
        }

        /// <summary>
        /// 将字节数组转换为Image对象
        /// </summary>
        /// <param name="byteArray">图片字节数组</param>
        /// <returns>Image对象</returns>
        public static System.Drawing.Image ByteArrayToImage(byte[] byteArray)
        {
            if (byteArray == null || byteArray.Length == 0)
            {
                throw new ArgumentException("Byte array is null or empty.", nameof(byteArray));
            }

            using (MemoryStream ms = new MemoryStream(byteArray))
            {
                var image = System.Drawing.Image.FromStream(ms);
                return (System.Drawing.Image)image.Clone();
            }
        }

        /// <summary>
        /// 将Image对象转换为字节数组
        /// </summary>
        /// <param name="image">Image对象</param>
        /// <param name="format">图片格式，默认使用原图格式</param>
        /// <param name="quality">压缩质量，默认使用配置值</param>
        /// <returns>字节数组</returns>
        public static byte[] ImageToByteArray(System.Drawing.Image image, ImageFormat format = null, long? quality = null)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image));

            using (var ms = new MemoryStream())
            {
                var outputFormat = format ?? image.RawFormat ?? ImageFormat.Jpeg;
                var outputQuality = quality ?? Quality;

                var encoderInfo = GetEncoderInfo(outputFormat);
                if (encoderInfo != null)
                {
                    var encoderParams = GetEncoderParameters(outputQuality);
                    image.Save(ms, encoderInfo, encoderParams);
                }
                else
                {
                    image.Save(ms, outputFormat);
                }

                return ms.ToArray();
            }
        }

        /// <summary>
        /// 调整图片尺寸
        /// </summary>
        /// <param name="image">原始图片</param>
        /// <param name="width">目标宽度</param>
        /// <param name="height">目标高度</param>
        /// <param name="maintainAspectRatio">是否保持宽高比</param>
        /// <returns>调整后的图片</returns>
        public static System.Drawing.Image ResizeImage(System.Drawing.Image image, int width, int height, bool maintainAspectRatio = true)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image));

            if (width <= 0 || height <= 0)
                throw new ArgumentException("目标尺寸必须大于0");

            int targetWidth = width;
            int targetHeight = height;

            // 保持宽高比
            if (maintainAspectRatio)
            {
                float ratioX = (float)width / image.Width;
                float ratioY = (float)height / image.Height;
                float ratio = Math.Min(ratioX, ratioY);

                targetWidth = (int)(image.Width * ratio);
                targetHeight = (int)(image.Height * ratio);
            }

            var resizedImage = new Bitmap(targetWidth, targetHeight);
            using (var graphics = Graphics.FromImage(resizedImage))
            {
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.CompositingQuality = CompositingQuality.HighQuality;

                graphics.DrawImage(image, 0, 0, targetWidth, targetHeight);
            }

            return resizedImage;
        }

        /// <summary>
        /// 按比例调整图片尺寸
        /// </summary>
        /// <param name="image">原始图片</param>
        /// <param name="scaleFactor">缩放比例</param>
        /// <returns>调整后的图片</returns>
        public static System.Drawing.Image ScaleImage(System.Drawing.Image image, float scaleFactor)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image));

            if (scaleFactor <= 0)
                throw new ArgumentException("缩放比例必须大于0");

            int newWidth = (int)(image.Width * scaleFactor);
            int newHeight = (int)(image.Height * scaleFactor);

            return ResizeImage(image, newWidth, newHeight, false);
        }

        /// <summary>
        /// 旋转图片
        /// </summary>
        /// <param name="image">原始图片</param>
        /// <param name="angle">旋转角度（度）</param>
        /// <returns>旋转后的图片</returns>
        public static System.Drawing.Image RotateImage(System.Drawing.Image image, float angle)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image));

            // 计算旋转后的尺寸
            var radAngle = angle * Math.PI / 180.0;
            double cos = Math.Abs(Math.Cos(radAngle));
            double sin = Math.Abs(Math.Sin(radAngle));

            int newWidth = (int)Math.Ceiling(image.Width * cos + image.Height * sin);
            int newHeight = (int)Math.Ceiling(image.Width * sin + image.Height * cos);

            var rotatedImage = new Bitmap(newWidth, newHeight);
            using (var graphics = Graphics.FromImage(rotatedImage))
            {
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;

                graphics.TranslateTransform(newWidth / 2.0f, newHeight / 2.0f);
                graphics.RotateTransform(angle);
                graphics.TranslateTransform(-image.Width / 2.0f, -image.Height / 2.0f);

                graphics.DrawImage(image, 0, 0);
            }

            return rotatedImage;
        }

        /// <summary>
        /// 裁剪图片
        /// </summary>
        /// <param name="image">原始图片</param>
        /// <param name="cropRect">裁剪区域</param>
        /// <returns>裁剪后的图片</returns>
        public static System.Drawing.Image CropImage(System.Drawing.Image image, Rectangle cropRect)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image));

            // 验证裁剪区域
            if (cropRect.X < 0 || cropRect.Y < 0 ||
                cropRect.X + cropRect.Width > image.Width ||
                cropRect.Y + cropRect.Height > image.Height)
            {
                throw new ArgumentException("裁剪区域超出图片范围");
            }

            var croppedImage = new Bitmap(cropRect.Width, cropRect.Height);
            using (var graphics = Graphics.FromImage(croppedImage))
            {
                graphics.DrawImage(image, 
                    new Rectangle(0, 0, cropRect.Width, cropRect.Height),
                    cropRect, 
                    GraphicsUnit.Pixel);
            }

            return croppedImage;
        }

        #endregion

        public static void SaveBytesAsImage(byte[] imageBytes, string filePath)
        {
            // 确保文件路径有效
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));
            }
            // 确保文件所在的目录存在
            EnsureDirectoryExists(filePath);
            // 确保字节数组不为空
            if (imageBytes == null || imageBytes.Length == 0)
            {
                throw new ArgumentException("Image bytes cannot be null or empty.", nameof(imageBytes));
            }

            // 使用 File.WriteAllBytes 方法保存字节数组为文件,
            //如果目标文件已经存在，它会自动覆盖该文件。因此，你不需要执行任何特殊操作来实现覆盖性保存。
            File.WriteAllBytes(filePath, imageBytes);
        }

        public static void SaveImageAsFile(System.Drawing.Image image, string filePath)
        {

            // 确保文件路径有效
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));
            }
            // 确保文件所在的目录存在
            EnsureDirectoryExists(filePath);
            // 确保图像对象不为空
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image), "Image cannot be null.");
            }

            // 确保图像有有效的格式
            if (image.RawFormat == null)
            {
                throw new InvalidOperationException("Image format cannot be determined.");
            }

            // 使用 MemoryStream 保存图像为字节数组
            using (MemoryStream ms = new MemoryStream())
            {
                // 保存图像到 MemoryStream
                image.Save(ms, image.RawFormat);

                // 将 MemoryStream 的位置重置为开始，以便读取字节
                ms.Position = 0;

                // 读取字节数组
                byte[] imageBytes = ms.ToArray();

                // 使用 File.WriteAllBytes 方法保存字节数组为文件
                File.WriteAllBytes(filePath, imageBytes);
            }
        }


        public static void EnsureDirectoryExists(string filePath)
        {
            // 获取文件所在的目录路径
            string directoryPath = Path.GetDirectoryName(filePath);

            // 检查目录是否存在
            if (directoryPath != null && !Directory.Exists(directoryPath))
            {
                // 如果目录不存在，则创建它
                Directory.CreateDirectory(directoryPath);
                Console.WriteLine($"Directory created: {directoryPath}");
            }
        }

        /// <summary>
        /// 获取图片编码器信息
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
        /// 获取编码器参数
        /// </summary>
        /// <param name="quality">压缩质量（1-100）</param>
        /// <returns>编码器参数</returns>
        private static EncoderParameters GetEncoderParameters(long quality)
        {
            var encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)quality);
            return encoderParams;
        }
    }

}
 
