using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace RUINORERP.Common.Services
{
    /// <summary>
    /// 图片处理服务类
    /// 负责图片的哈希计算、重复检测、文件名生成等处理逻辑
    /// </summary>
    public class ImageProcessingService
    {
        private readonly ILogger<ImageProcessingService> _logger;

        public ImageProcessingService(ILogger<ImageProcessingService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 计算图片的MD5哈希值
        /// </summary>
        /// <param name="imageBytes">图片字节数组</param>
        /// <returns>MD5哈希字符串，如果输入为空则返回空字符串</returns>
        public string CalculateImageHash(byte[] imageBytes)
        {
            if (imageBytes == null || imageBytes.Length == 0)
            {
                _logger.LogWarning("计算图片哈希值失败：图片字节数组为空");
                return string.Empty;
            }

            try
            {
                using (var md5 = MD5.Create())
                {
                    byte[] hashBytes = md5.ComputeHash(imageBytes);
                    return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "计算图片哈希值时发生异常");
                return string.Empty;
            }
        }

        /// <summary>
        /// 生成包含业务信息的图片文件名
        /// </summary>
        /// <param name="bizType">业务类型</param>
        /// <param name="entityId">实体ID</param>
        /// <param name="imageHash">图片哈希值</param>
        /// <param name="fileExtension">文件扩展名</param>
        /// <returns>生成的图片文件名</returns>
        public string GenerateImageFileName(string bizType, string entityId, string imageHash, string fileExtension = ".jpg")
        {
            if (string.IsNullOrEmpty(bizType))
                throw new ArgumentException("业务类型不能为空", nameof(bizType));
            if (string.IsNullOrEmpty(entityId))
                throw new ArgumentException("实体ID不能为空", nameof(entityId));

            // 如果哈希值有效，使用哈希值前8位作为文件名的一部分
            if (!string.IsNullOrEmpty(imageHash) && imageHash.Length >= 8)
            {
                return $"{bizType}_{entityId}_{imageHash.Substring(0, 8)}{fileExtension}";
            }

            // 哈希值无效时，使用时间戳和GUID作为文件名
            return $"{bizType}_{entityId}_{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid():N}{fileExtension}";
        }

        /// <summary>
        /// 检查图片是否已存在（基于哈希值）
        /// </summary>
        /// <param name="directoryPath">目录路径</param>
        /// <param name="imageHash">图片哈希值</param>
        /// <returns>如果存在重复图片返回true，否则返回false</returns>
        public bool CheckImageExists(string directoryPath, string imageHash)
        {
            if (string.IsNullOrEmpty(directoryPath) || !Directory.Exists(directoryPath))
                return false;

            if (string.IsNullOrEmpty(imageHash) || imageHash.Length < 8)
                return false;

            try
            {
                string hashPrefix = imageHash.Substring(0, 8);
                var existingFiles = Directory.GetFiles(directoryPath, "*.*", SearchOption.TopDirectoryOnly)
                    .Where(file => Path.GetFileName(file).Contains(hashPrefix));

                return existingFiles.Any();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "检查图片是否存在时发生异常，目录：{DirectoryPath}", directoryPath);
                return false;
            }
        }

        /// <summary>
        /// 获取已存在的图片文件名
        /// </summary>
        /// <param name="directoryPath">目录路径</param>
        /// <param name="imageHash">图片哈希值</param>
        /// <returns>已存在的图片文件名，如果不存在返回null</returns>
        public string GetExistingImageFileName(string directoryPath, string imageHash)
        {
            if (string.IsNullOrEmpty(directoryPath) || !Directory.Exists(directoryPath))
                return null;

            if (string.IsNullOrEmpty(imageHash) || imageHash.Length < 8)
                return null;

            try
            {
                string hashPrefix = imageHash.Substring(0, 8);
                var existingFiles = Directory.GetFiles(directoryPath, "*.*", SearchOption.TopDirectoryOnly)
                    .Where(file => Path.GetFileName(file).Contains(hashPrefix));

                return existingFiles.Any() ? Path.GetFileName(existingFiles.First()) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取已存在图片文件名时发生异常，目录：{DirectoryPath}", directoryPath);
                return null;
            }
        }

        /// <summary>
        /// 验证图片格式
        /// </summary>
        /// <param name="fileExtension">文件扩展名</param>
        /// <returns>如果是有效的图片格式返回true，否则返回false</returns>
        public bool IsValidImageFormat(string fileExtension)
        {
            if (string.IsNullOrEmpty(fileExtension))
                return false;

            string[] validExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
            return validExtensions.Contains(fileExtension.ToLower());
        }

        /// <summary>
        /// 验证图片大小
        /// </summary>
        /// <param name="imageBytes">图片字节数组</param>
        /// <param name="maxSizeMB">最大允许大小（MB）</param>
        /// <returns>如果图片大小在允许范围内返回true，否则返回false</returns>
        public bool ValidateImageSize(byte[] imageBytes, int maxSizeMB = 10)
        {
            if (imageBytes == null)
                return false;

            long maxSizeBytes = maxSizeMB * 1024 * 1024;
            return imageBytes.Length <= maxSizeBytes;
        }
    }
}