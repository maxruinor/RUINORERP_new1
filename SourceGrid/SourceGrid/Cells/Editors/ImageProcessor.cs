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
    public static class ImageProcessor
    {
        public static byte[] CompressImage(string filePath)
        {
            using (System.Drawing.Image image = System.Drawing.Image.FromFile(filePath))
            {
                return CompressImage(image);
            }
        }
 
        public static byte[] CompressImage(System.Drawing.Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                try
                {
                    // 检查图片大小是否超过设置的最大值
                    if (image.Width > 0 && image.Height > 0)
                    {
                        double imageSize = Math.Round((image.Width * image.HorizontalResolution + image.Height * image.VerticalResolution) / 10000);
                        if (imageSize > MaxImageSize)
                        {
                            float ratio = (float)(MaxImageSize / imageSize);

                            int newWidth = (int)(image.Width * ratio);
                            int newHeight = (int)(image.Height * ratio);

                            using (Bitmap compressedImage = new Bitmap(newWidth, newHeight))
                            {
                                using (Graphics g = Graphics.FromImage(compressedImage))
                                {
                                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                    g.CompositingQuality = CompositingQuality.HighQuality;
                                    g.SmoothingMode = SmoothingMode.HighQuality;

                                    g.DrawImage(image, 0, 0, newWidth, newHeight);

                                    compressedImage.Save(ms, GetEncoderInfo(ImageFormat.Jpeg), GetEncoderParameters(Quality));
                                }
                            }
                        }
                        else
                        {
                            image.Save(ms, ImageFormat.Jpeg);
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Invalid image dimensions.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing image: {ex.Message}");
                    throw;
                }

                ms.Flush();
                ms.Close();

                return ms.ToArray();
            }
        }
        public static void DisplayImageFromBytes(byte[] imageBytes, PictureBox pictureBox)
        {
            if (imageBytes == null || imageBytes.Length == 0)
            {
                throw new ArgumentException("Image bytes cannot be null or empty.", nameof(imageBytes));
            }

            using (MemoryStream ms = new MemoryStream(imageBytes))
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(ms);
                pictureBox.Image = image;  // 设置 PictureBox 的 Image 属性
            }
        }
        public static System.Drawing.Image ByteArrayToImage(byte[] byteArray)
        {
            if (byteArray == null || byteArray.Length == 0)
            {
                throw new ArgumentException("Byte array is null or empty.", nameof(byteArray));
            }

            using (MemoryStream ms = new MemoryStream(byteArray))
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(ms);
                return image;
            }
        }
        #region 图片压缩  75L CompressImage 方法接受图片文件路径和最大尺寸（默认为1MB）。它首先检查图片大小，如果大于1MB，则按比例压缩图片。压缩时，使用高质量的插值模式和压缩参数以保持图片质量。        要使用这个方法，只需传入图片路径：
        // 定义属性
        public static double MaxImageSize { get; set; } = 1024 * 1024; // 默认1MB
        public static long Quality { get; set; } = 75L; // 默认质量为75

        public static byte[] CompressImage(System.Drawing.Image image, int maxSizeInBytes = 1024 * 1024)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // 检查图片大小是否超过1M
                if (image.Width > 0 && image.Height > 0)
                {
                    // 计算压缩比例
                    float ratio = Math.Min((float)maxSizeInBytes / (image.Width * image.Height), 1.0f);

                    // 获取原始图片的宽度和高度
                    int originalWidth = image.Width;
                    int originalHeight = image.Height;

                    // 计算新的宽度和高度
                    int newWidth = (int)(originalWidth * ratio);
                    int newHeight = (int)(originalHeight * ratio);

                    // 创建一个新的Bitmap对象，其大小是原始图片按比例缩小后的大小
                    using (Bitmap compressedImage = new Bitmap(newWidth, newHeight))
                    {
                        using (Graphics g = Graphics.FromImage(compressedImage))
                        {
                            // 设置质量
                            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            g.CompositingQuality = CompositingQuality.HighQuality;
                            g.SmoothingMode = SmoothingMode.HighQuality;

                            // 在新Bitmap上绘制缩小后的图片
                            g.DrawImage(image, 0, 0, newWidth, newHeight);

                            // 将压缩后的图片保存到内存流中
                            compressedImage.Save(ms, ImageFormat.Jpeg);

                            // 将内存流转换为byte数组
                            return ms.ToArray();
                        }
                    }
                }
                else
                {
                    // 图片尺寸为0，直接保存原图
                    image.Save(ms, image.RawFormat);
                    return ms.ToArray();
                }
            }
        }

        // 处理传入 byte[] 数组的方法
        public static byte[] CompressImage(byte[] imageBytes, int maxSizeInBytes = 1024 * 1024)
        {
            using (MemoryStream msInput = new MemoryStream(imageBytes))
            {
                using (System.Drawing.Image image = System.Drawing.Image.FromStream(msInput, true))
                {
                    return CompressImage(image, maxSizeInBytes);
                }
            }
        }
        public static byte[] CompressImage(string filePath, int maxSizeInBytes = 1024 * 1024)
        {
            using (System.Drawing.Image image = System.Drawing.Image.FromFile(filePath))
            {
                // 检查图片大小是否超过1M
                long imageSize = new FileInfo(filePath).Length;
                if (imageSize > maxSizeInBytes)
                {
                    // 计算压缩比例
                    float ratio = (float)maxSizeInBytes / imageSize;

                    // 获取原始图片的宽度和高度
                    int originalWidth = image.Width;
                    int originalHeight = image.Height;

                    // 计算新的宽度和高度
                    int newWidth = (int)(originalWidth * ratio);
                    int newHeight = (int)(originalHeight * ratio);

                    // 创建一个新的Bitmap对象，其大小是原始图片按比例缩小后的大小
                    using (Bitmap compressedImage = new Bitmap(newWidth, newHeight))
                    {
                        using (Graphics g = Graphics.FromImage(compressedImage))
                        {
                            // 设置质量
                            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            g.CompositingQuality = CompositingQuality.HighQuality;
                            g.SmoothingMode = SmoothingMode.HighQuality;

                            // 在新Bitmap上绘制缩小后的图片
                            g.DrawImage(image, 0, 0, newWidth, newHeight);

                            // 将压缩后的图片保存到内存流中
                            using (MemoryStream ms = new MemoryStream())
                            {
                                // 选择JPEG格式并设置质量
                                compressedImage.Save(ms, GetEncoderInfo(ImageFormat.Jpeg), GetEncoderParameters(75L)); // 75% quality

                                // 将内存流转换为byte数组
                                return ms.ToArray();
                            }
                        }
                    }
                }
                else
                {
                    // 如果图片不需要压缩，直接读取原图片的byte数组
                    using (MemoryStream ms = new MemoryStream())
                    {
                        image.Save(ms, ImageFormat.Jpeg);
                        return ms.ToArray();
                    }
                }
            }
        }

        private static ImageCodecInfo GetEncoderInfo(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        private static EncoderParameters GetEncoderParameters(long quality)
        {
            EncoderParameters encoderParameters = new EncoderParameters(1);
            EncoderParameter parameter = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            encoderParameters.Param[0] = parameter;
            return encoderParameters;
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
    }

}
 
