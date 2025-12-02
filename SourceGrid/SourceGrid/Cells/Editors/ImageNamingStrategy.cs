using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace SourceGrid.Cells.Editors
{
    /// <summary>
    /// 图片命名策略
    /// 统一处理图片文件的命名规则，解决客户端和服务端命名冲突问题
    /// </summary>
    public static class ImageNamingStrategy
    {
        /// <summary>
        /// 生成唯一文件标识符
        /// 格式: {日期}/{8位哈希}_{时间戳}_{8位随机字符}
        /// 例如: 24/12/a1b2c3d4_20241201143052_x7y9z2w5
        /// </summary>
        /// <param name="imageBytes">图片字节数据</param>
        /// <param name="originalFileName">原始文件名（可选）</param>
        /// <returns>唯一文件标识符</returns>
        public static string GenerateUniqueFileId(byte[] imageBytes, string originalFileName = null)
        {
            if (imageBytes == null || imageBytes.Length == 0)
                throw new ArgumentException("图片数据不能为空", nameof(imageBytes));

            // 1. 生成内容哈希（前8位）
            string contentHash = GenerateContentHash(imageBytes).Substring(0, 8);

            // 2. 生成时间戳（精确到秒）
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");

            // 3. 生成随机字符（8位）
            string randomPart = GenerateRandomString(8);

            // 4. 生成日期路径
            string datePath = DateTime.Now.ToString("yy/MM");

            // 5. 组合文件ID
            string fileId = $"{datePath}/{contentHash}_{timestamp}_{randomPart}";

            return fileId;
        }

        /// <summary>
        /// 生成临时文件标识符
        /// 用于客户端临时标识，上传成功后会替换为服务端生成的正式ID
        /// </summary>
        /// <returns>临时文件ID</returns>
        public static string GenerateTempFileId()
        {
            return $"TEMP_{DateTime.Now:yyyyMMddHHmmss}_{GenerateRandomString(6)}";
        }

        /// <summary>
        /// 解析文件ID获取信息
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <returns>解析后的文件信息</returns>
        public static FileIdInfo ParseFileId(string fileId)
        {
            if (string.IsNullOrEmpty(fileId))
                return null;

            // 检查是否为临时ID
            if (fileId.StartsWith("TEMP_"))
            {
                return new FileIdInfo
                {
                    IsTemp = true,
                    FileId = fileId,
                    Date = DateTime.Now,
                    Hash = "TEMP",
                    Timestamp = fileId.Substring(5, 14),
                    RandomPart = fileId.Substring(22)
                };
            }

            // 解析正式ID格式: 24/12/a1b2c3d4_20241201143052_x7y9z2w5
            string[] parts = fileId.Split('/');
            if (parts.Length != 2)
                return null;

            string datePath = parts[0];
            string fileName = parts[1];
            string[] fileNameParts = fileName.Split('_');

            if (fileNameParts.Length != 3)
                return null;

            string hash = fileNameParts[0];
            string timestamp = fileNameParts[1];
            string randomPart = fileNameParts[2];

            // 解析日期
            try
            {
                int year = 2000 + int.Parse(datePath.Substring(0, 2));
                int month = int.Parse(datePath.Substring(3, 2));
                var date = new DateTime(year, month, 1);
            }
            catch
            {
                return null;
            }

            return new FileIdInfo
            {
                IsTemp = false,
                FileId = fileId,
                DatePath = datePath,
                Hash = hash,
                Timestamp = timestamp,
                RandomPart = randomPart
            };
        }

        /// <summary>
        /// 生成完整的文件名
        /// </summary>
        /// <param name="fileId">文件ID</param>
        /// <param name="extension">文件扩展名</param>
        /// <returns>完整文件名</returns>
        public static string GenerateFileName(string fileId, string extension = ".jpg")
        {
            if (string.IsNullOrEmpty(fileId))
                throw new ArgumentException("文件ID不能为空", nameof(fileId));

            // 确保扩展名以点开头
            if (!extension.StartsWith("."))
                extension = "." + extension;

            return $"{fileId}{extension.ToLower()}";
        }

        /// <summary>
        /// 比较两个文件ID是否表示同一文件
        /// </summary>
        /// <param name="fileId1">文件ID1</param>
        /// <param name="fileId2">文件ID2</param>
        /// <returns>是否为同一文件</returns>
        public static bool IsSameFile(string fileId1, string fileId2)
        {
            if (string.IsNullOrEmpty(fileId1) || string.IsNullOrEmpty(fileId2))
                return false;

            // 如果其中一个为临时ID，不能直接比较
            if (fileId1.StartsWith("TEMP_") || fileId2.StartsWith("TEMP_"))
                return false;

            // 解析并比较哈希值
            var info1 = ParseFileId(fileId1);
            var info2 = ParseFileId(fileId2);

            return info1?.Hash == info2?.Hash;
        }

        /// <summary>
        /// 生成内容哈希
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns>哈希值</returns>
        private static string GenerateContentHash(byte[] data)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(data);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
            }
        }

        /// <summary>
        /// 生成随机字符串
        /// </summary>
        /// <param name="length">长度</param>
        /// <returns>随机字符串</returns>
        private static string GenerateRandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var result = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                result.Append(chars[random.Next(chars.Length)]);
            }

            return result.ToString();
        }
    }

    /// <summary>
    /// 文件ID信息
    /// </summary>
    public class FileIdInfo
    {
        /// <summary>
        /// 是否为临时文件ID
        /// </summary>
        public bool IsTemp { get; set; }

        /// <summary>
        /// 完整文件ID
        /// </summary>
        public string FileId { get; set; }

        /// <summary>
        /// 日期路径（如：24/12）
        /// </summary>
        public string DatePath { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// 内容哈希（8位）
        /// </summary>
        public string Hash { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public string Timestamp { get; set; }

        /// <summary>
        /// 随机部分
        /// </summary>
        public string RandomPart { get; set; }

        /// <summary>
        /// 重写ToString方法
        /// </summary>
        /// <returns>字符串表示</returns>
        public override string ToString()
        {
            return $"FileId: {FileId}, IsTemp: {IsTemp}, Hash: {Hash}";
        }
    }
}