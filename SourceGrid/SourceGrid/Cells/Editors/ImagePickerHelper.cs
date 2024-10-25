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


namespace SourceGrid.Cells.Editors
{
    /// <summary>
    /// 用于比较图片哈希值的帮助类
    /// </summary>
    public class ImageHashHelper
    {

        public static bool AreHashesEqual(string hash1, string hash2)
        {
            return hash1.Equals(hash2, StringComparison.OrdinalIgnoreCase);
        }
 

            public static string GenerateHash(byte[] data)
        {
            using (MD5 md5 = MD5.Create())
            {
                return BitConverter.ToString(md5.ComputeHash(data)).Replace("-", "").ToLowerInvariant();
            }
        }
        public static string GetImageHash(string filePath)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filePath))
                {
                    var hashBytes = md5.ComputeHash(stream);
                    return BitConverter.ToString(hashBytes).Replace("-", String.Empty).ToLowerInvariant();
                }
            }
        }
        public static string GenerateHash(string filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                return GenerateHash(fs);
            }
        }

        public static string GenerateHash(Stream stream)
        {
            using (MD5 md5 = MD5.Create())
            {
                return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "").ToLowerInvariant();
            }
        }

        public static string GenerateHash(System.Drawing.Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, image.RawFormat);
                return GenerateHash(ms.ToArray());
            }
        }

        public static bool AreImagesEqual(string filePath1, string filePath2)
        {
            var hash1 = GetImageHash(filePath1);
            var hash2 = GetImageHash(filePath2);
            return hash1 == hash2;
        }
    }
}
