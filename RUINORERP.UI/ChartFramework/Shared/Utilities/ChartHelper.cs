using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;
using System.Globalization;

namespace RUINORERP.UI.ChartFramework
{

    // 使用示例
    //var skColor = ColorHelper.HexToSKColor("#4CAF50");
    public static class ChartHelper
    {
        /// <summary>
        /// 将十六进制颜色字符串转换为 SKColor
        /// </summary>
        /// <param name="hexColor">十六进制颜色字符串，如 "#4CAF50"</param>
        /// <returns>SKColor 对象</returns>
        public static SKColor HexToSKColor(string hexColor)
        {
            // 去掉 # 符号
            hexColor = hexColor.Replace("#", string.Empty);

            // 验证长度
            if (hexColor.Length != 6 && hexColor.Length != 8)
            {
                throw new ArgumentException("Invalid hex color format. Must be 6 or 8 characters long.");
            }

            // 解析颜色值
            byte r = byte.Parse(hexColor.Substring(0, 2), NumberStyles.HexNumber);
            byte g = byte.Parse(hexColor.Substring(2, 2), NumberStyles.HexNumber);
            byte b = byte.Parse(hexColor.Substring(4, 2), NumberStyles.HexNumber);
            byte a = hexColor.Length == 8
                ? byte.Parse(hexColor.Substring(6, 2), NumberStyles.HexNumber)
                : (byte)255;

            return new SKColor(r, g, b, a);
        }

        /// <summary>
        /// 将十六进制颜色字符串转换为 SKColor
        /// </summary>
        /// <param name="hexColor">十六进制颜色字符串，如 "#4CAF50"</param>
        /// <returns>SKColor 对象</returns>
        public static SKColor ToSKColor(this string hexColor)
        {
            return HexToSKColor(hexColor);
        }
    }


}
