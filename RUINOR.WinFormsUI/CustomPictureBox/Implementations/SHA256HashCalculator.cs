using System;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;

namespace RUINOR.WinFormsUI.CustomPictureBox.Implementations
{
    /// <summary>
    /// SHA256图片哈希计算器
    /// 使用SHA256算法计算图片的唯一哈希值
    /// </summary>
    public class SHA256HashCalculator
    {
        private readonly ImageProcessor _imageProcessor;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="imageProcessor">图片处理器实例</param>
        public SHA256HashCalculator(ImageProcessor imageProcessor)
        {
            _imageProcessor = imageProcessor;
        }

        /// <summary>
        /// 计算图片字节数据的哈希值
        /// </summary>
        /// <param name="imageBytes">图片字节数据</param>
        /// <returns>计算得到的SHA256哈希值字符串</returns>
        public string CalculateHash(byte[] imageBytes)
        {
            if (imageBytes == null || imageBytes.Length == 0)
            {
                throw new ArgumentNullException(nameof(imageBytes), "图片字节数据不能为空");
            }

            using (var sha256 = SHA256.Create())
            {
                var hashBytes = sha256.ComputeHash(imageBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }

        /// <summary>
        /// 计算Image对象的哈希值
        /// </summary>
        /// <param name="image">Image对象</param>
        /// <returns>计算得到的SHA256哈希值字符串</returns>
        public string CalculateHash(Image image)
        {
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image), "Image对象不能为空");
            }

            // 将Image转换为字节数组，然后计算哈希值
            var imageBytes = _imageProcessor.ImageToBytes(image);
            return CalculateHash(imageBytes);
        }

        /// <summary>
        /// 比较两张图片是否相同（基于哈希值）
        /// </summary>
        /// <param name="imageBytes1">第一张图片的字节数据</param>
        /// <param name="imageBytes2">第二张图片的字节数据</param>
        /// <returns>如果图片相同则返回true，否则返回false</returns>
        public bool AreImagesEqual(byte[] imageBytes1, byte[] imageBytes2)
        {
            if (imageBytes1 == null && imageBytes2 == null)
            {
                return true;
            }

            if (imageBytes1 == null || imageBytes2 == null)
            {
                return false;
            }

            var hash1 = CalculateHash(imageBytes1);
            var hash2 = CalculateHash(imageBytes2);

            return string.Equals(hash1, hash2, StringComparison.Ordinal);
        }

        /// <summary>
        /// 比较两个Image对象是否相同（基于哈希值）
        /// </summary>
        /// <param name="image1">第一个Image对象</param>
        /// <param name="image2">第二个Image对象</param>
        /// <returns>如果图片相同则返回true，否则返回false</returns>
        public bool AreImagesEqual(Image image1, Image image2)
        {
            if (image1 == null && image2 == null)
            {
                return true;
            }

            if (image1 == null || image2 == null)
            {
                return false;
            }

            var hash1 = CalculateHash(image1);
            var hash2 = CalculateHash(image2);

            return string.Equals(hash1, hash2, StringComparison.Ordinal);
        }
    }
}