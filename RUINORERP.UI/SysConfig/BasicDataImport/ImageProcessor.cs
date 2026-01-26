using System;using System.Collections.Generic;using System.Drawing;using System.Drawing.Imaging;using System.IO;using System.Linq;using System.Text;using System.Threading.Tasks;

namespace RUINORERP.UI.SysConfig.BasicDataImport
{
    /// <summary>
    /// 图片处理器
    /// 负责图片的压缩、调整大小和保存
    /// </summary>
    public class ImageProcessor
    {
        /// <summary>
        /// 图片最大宽度（像素）
        /// </summary>
        private const int MaxImageWidth = 1024;
        
        /// <summary>
        /// 图片最大高度（像素）
        /// </summary>
        private const int MaxImageHeight = 1024;
        
        /// <summary>
        /// 图片质量（0-100）
        /// </summary>
        private const long ImageQuality = 85;
        
        /// <summary>
        /// 图片保存目录
        /// </summary>
        private readonly string _imageSavePath;
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="imageSavePath">图片保存路径</param>
        public ImageProcessor(string imageSavePath)
        {
            if (string.IsNullOrWhiteSpace(imageSavePath))
            {
                throw new ArgumentException("图片保存路径不能为空", nameof(imageSavePath));
            }
            
            _imageSavePath = imageSavePath;
            
            // 确保保存目录存在
            if (!Directory.Exists(_imageSavePath))
            {
                Directory.CreateDirectory(_imageSavePath);
            }
        }
        
        /// <summary>
        /// 处理图片并保存
        /// </summary>
        /// <param name="imagePath">原始图片路径</param>
        /// <param name="productCode">产品编码，用于生成图片文件名</param>
        /// <returns>保存后的图片相对路径</returns>
        public string ProcessAndSaveImage(string imagePath, string productCode)
        {
            if (string.IsNullOrWhiteSpace(imagePath))
            {
                return string.Empty;
            }
            
            if (!File.Exists(imagePath))
            {
                return string.Empty;
            }
            
            try
            {
                using (var originalImage = Image.FromFile(imagePath))
                {
                    // 调整图片大小
                    using (var resizedImage = ResizeImage(originalImage))
                    {
                        // 生成新的文件名
                        string extension = Path.GetExtension(imagePath);
                        string fileName = $"{productCode}_{Guid.NewGuid().ToString().Substring(0, 8)}{extension}";
                        string saveFullPath = Path.Combine(_imageSavePath, fileName);
                        
                        // 保存图片
                        SaveImageWithQuality(resizedImage, saveFullPath, ImageQuality);
                        
                        // 返回相对路径
                        return fileName;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"图片处理失败: {ex.Message}", ex);
            }
        }
        
        /// <summary>
        /// 批量处理图片
        /// </summary>
        /// <param name="imagePaths">原始图片路径列表</param>
        /// <param name="productCode">产品编码</param>
        /// <returns>保存后的图片相对路径列表</returns>
        public List<string> ProcessAndSaveImages(List<string> imagePaths, string productCode)
        {
            var savedImagePaths = new List<string>();
            
            if (imagePaths == null || imagePaths.Count == 0)
            {
                return savedImagePaths;
            }
            
            foreach (var imagePath in imagePaths)
            {
                string savedPath = ProcessAndSaveImage(imagePath, productCode);
                if (!string.IsNullOrWhiteSpace(savedPath))
                {
                    savedImagePaths.Add(savedPath);
                }
            }
            
            return savedImagePaths;
        }
        
        /// <summary>
        /// 调整图片大小
        /// </summary>
        /// <param name="originalImage">原始图片</param>
        /// <returns>调整大小后的图片</returns>
        private Image ResizeImage(Image originalImage)
        {
            int width = originalImage.Width;
            int height = originalImage.Height;
            
            // 计算新的尺寸，保持原始比例
            if (width > MaxImageWidth || height > MaxImageHeight)
            {
                double widthRatio = (double)MaxImageWidth / width;
                double heightRatio = (double)MaxImageHeight / height;
                double ratio = Math.Min(widthRatio, heightRatio);
                
                width = (int)(width * ratio);
                height = (int)(height * ratio);
            }
            
            // 创建新的图片
            var resizedImage = new Bitmap(width, height);
            using (var graphics = Graphics.FromImage(resizedImage))
            {
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                
                graphics.DrawImage(originalImage, 0, 0, width, height);
            }
            
            return resizedImage;
        }
        
        /// <summary>
        /// 保存图片并设置质量
        /// </summary>
        /// <param name="image">图片对象</param>
        /// <param name="savePath">保存路径</param>
        /// <param name="quality">图片质量（0-100）</param>
        private void SaveImageWithQuality(Image image, string savePath, long quality)
        {
            // 获取图片编码信息
            ImageCodecInfo jpegCodec = GetEncoderInfo(ImageFormat.Jpeg);
            if (jpegCodec == null)
            {
                // 如果没有找到JPEG编码器，直接保存
                image.Save(savePath);
                return;
            }
            
            // 设置图片质量参数
            using (var encoderParameters = new System.Drawing.Imaging.EncoderParameters(1))
            {
                using (var encoderParameter = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality))
                {
                    encoderParameters.Param[0] = encoderParameter;
                    image.Save(savePath, jpegCodec, encoderParameters);
                }
            }
        }
        
        /// <summary>
        /// 获取图片编码器信息
        /// </summary>
        /// <param name="format">图片格式</param>
        /// <returns>图片编码器信息</returns>
        private ImageCodecInfo GetEncoderInfo(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            
            foreach (var codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            
            return null;
        }
        
        /// <summary>
        /// 从图片路径字符串中提取图片路径列表
        /// </summary>
        /// <param name="imagePathsString">图片路径字符串，多个路径用分号分隔</param>
        /// <returns>图片路径列表</returns>
        public List<string> ExtractImagePaths(string imagePathsString)
        {
            var imagePaths = new List<string>();
            
            if (string.IsNullOrWhiteSpace(imagePathsString))
            {
                return imagePaths;
            }
            
            // 分割字符串并去除空白
            string[] paths = imagePathsString.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            
            foreach (var path in paths)
            {
                string trimmedPath = path.Trim();
                if (File.Exists(trimmedPath))
                {
                    imagePaths.Add(trimmedPath);
                }
            }
            
            return imagePaths;
        }
    }
}