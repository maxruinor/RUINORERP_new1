using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoUpdate
{
    public static class ZipArchiveExtensions
    {
        /// <summary>
        /// zip文件解压到指定目录
        /// </summary>
        /// <param name="archive">zip文件</param>
        /// <param name="destinationDirectoryName">解压目录</param>
        /// <param name="overwrite">是否覆盖</param>
        public static void ExtractToDirectory(this ZipArchive archive, string destinationDirectoryName, bool overwrite)
        {
            if (!overwrite)
            {
                archive.ExtractToDirectory(destinationDirectoryName);
                return;
            }

            DirectoryInfo di = Directory.CreateDirectory(destinationDirectoryName);
            string destinationDirectoryFullPath = di.FullName;

            foreach (ZipArchiveEntry file in archive.Entries)
            {
                string completeFileName = Path.GetFullPath(Path.Combine(destinationDirectoryFullPath, file.FullName));
                if (!Directory.Exists(Path.GetDirectoryName(completeFileName)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(completeFileName));
                }

                if (!completeFileName.StartsWith(destinationDirectoryFullPath, StringComparison.OrdinalIgnoreCase))
                {
                    throw new IOException("Trying to extract file outside of destination directory. See this link for more info: https://snyk.io/research/zip-slip-vulnerability");
                }

                if (file.Name == "")
                {// Assuming Empty for Directory
                    Directory.CreateDirectory(Path.GetDirectoryName(completeFileName));
                    continue;
                }
                if (File.Exists(completeFileName))
                {
                    file.ExtractToFile(completeFileName, true);
                }
                else
                {
                    file.ExtractToFile(completeFileName,false);
                }
            }
        }
    }
}

