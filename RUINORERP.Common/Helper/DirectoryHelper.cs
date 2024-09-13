using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.Common.Helper
{
    public static class DirectoryHelper
    {
        public static string GetAppDataPath()
        {
            return Path.GetDirectoryName(Application.ExecutablePath);
        }

        public static string GetTempPath()
        {
            string appDataPath = GetAppDataPath();
            string tempPath = Path.Combine(appDataPath, "temp");

            // 检查 temp 文件夹是否存在，如果不存在则创建
            if (!Directory.Exists(tempPath))
            {
                Directory.CreateDirectory(tempPath);
            }

            return tempPath;
        }
    }
}
