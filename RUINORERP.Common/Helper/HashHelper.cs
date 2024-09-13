using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Common.Helper
{
    public class HashHelper
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
    }
}
