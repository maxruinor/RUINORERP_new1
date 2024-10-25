using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Common.Extensions
{
    public static partial class ExtObject
    {
        /// <summary>
        /// 将流Stream转为byte数组
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] ToBytes(this Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // 将图像保存到内存流中，这里使用PNG格式，你也可以根据需要选择其他格式
                image.Save(ms, ImageFormat.Png);

                // 将流的位置重置到开始位置
                ms.Seek(0, SeekOrigin.Begin);

                // 读取流中的所有字节
                byte[] bytes = ms.ToArray();

                return bytes;
            }
        }

    }
}
