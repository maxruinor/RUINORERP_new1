using System;
using System.Drawing;
using System.IO;
using RUINORERP.Common.BusinessImage;

namespace SourceGrid.Cells.Utilities
{
    /// <summary>
    /// 图片数据验证器
    /// 提供图片数据的验证功能
    /// </summary>
    public static class ImageDataValidator
    {
        #region FileId Validation

        /// <summary>
        /// 验证图片ID
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <returns>是否有效</returns>
        public static bool ValidateFileId(long fileId)
        {
            return fileId > 0;
        }

        /// <summary>
        /// 验证图片ID并抛出异常
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <param name="paramName">参数名</param>
        /// <exception cref="ArgumentException">文件ID无效时抛出</exception>
        public static void ValidateFileIdOrThrow(long fileId, string paramName = "fileId")
        {
            if (!ValidateFileId(fileId))
            {
                throw new ArgumentException("文件ID必须大于0", paramName);
            }
        }

        #endregion

        #region Image Data Validation

        /// <summary>
        /// 验证图片字节数据
        /// </summary>
        /// <param name="imageData">图片数据</param>
        /// <returns>是否有效</returns>
        public static bool ValidateImageData(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0)
                return false;

            try
            {
                using (var ms = new MemoryStream(imageData))
                using (var img = Image.FromStream(ms))
                {
                    return img.Width > 0 && img.Height > 0;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 验证图片字节数据并抛出异常
        /// </summary>
        /// <param name="imageData">图片数据</param>
        /// <param name="paramName">参数名</param>
        /// <exception cref="ArgumentException">图片数据无效时抛出</exception>
        public static void ValidateImageDataOrThrow(byte[] imageData, string paramName = "imageData")
        {
            if (!ValidateImageData(imageData))
            {
                throw new ArgumentException("图片数据无效", paramName);
            }
        }

        #endregion

        #region Image Object Validation

        /// <summary>
        /// 验证图片对象
        /// </summary>
        /// <param name="image">图片对象</param>
        /// <returns>是否有效</returns>
        public static bool ValidateImageObject(Image image)
        {
            if (image == null)
                return false;

            try
            {
                return image.Width > 0 && image.Height > 0;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 验证图片对象并抛出异常
        /// </summary>
        /// <param name="image">图片对象</param>
        /// <param name="paramName">参数名</param>
        /// <exception cref="ArgumentException">图片对象无效时抛出</exception>
        public static void ValidateImageObjectOrThrow(Image image, string paramName = "image")
        {
            if (!ValidateImageObject(image))
            {
                throw new ArgumentException("图片对象无效", paramName);
            }
        }

        #endregion

        #region ImageInfo Validation

        /// <summary>
        /// 验证图片信息
        /// </summary>
        /// <param name="imageInfo">图片信息</param>
        /// <returns>是否有效</returns>
        public static bool ValidateImageInfo(ImageInfo imageInfo)
        {
            if (imageInfo == null)
                return false;

            return ValidateFileId(imageInfo.FileId) &&
                   ValidateImageData(imageInfo.ImageData);
        }

        /// <summary>
        /// 验证图片信息并抛出异常
        /// </summary>
        /// <param name="imageInfo">图片信息</param>
        /// <param name="paramName">参数名</param>
        /// <exception cref="ArgumentException">图片信息无效时抛出</exception>
        public static void ValidateImageInfoOrThrow(ImageInfo imageInfo, string paramName = "imageInfo")
        {
            if (!ValidateImageInfo(imageInfo))
            {
                throw new ArgumentException("图片信息无效", paramName);
            }
        }

        #endregion

        #region File Path Validation

        /// <summary>
        /// 验证文件路径
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>是否有效</returns>
        public static bool ValidateFilePath(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return false;

            return File.Exists(filePath);
        }

        /// <summary>
        /// 验证文件路径是否为支持的图片格式
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="supportedExtensions">支持的扩展名列表</param>
        /// <returns>是否有效</returns>
        public static bool ValidateFileExtension(string filePath, string[] supportedExtensions = null)
        {
            if (string.IsNullOrEmpty(filePath))
                return false;

            string extension = Path.GetExtension(filePath)?.ToLowerInvariant();
            if (string.IsNullOrEmpty(extension))
                return false;

            if (supportedExtensions == null || supportedExtensions.Length == 0)
            {
                // 默认支持的图片格式
                supportedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff", ".webp", ".ico" };
            }

            return Array.Exists(supportedExtensions, ext => ext.Equals(extension, StringComparison.OrdinalIgnoreCase));
        }

        #endregion
    }
}
