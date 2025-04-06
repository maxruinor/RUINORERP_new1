using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoUpdateTools.XmlCompare
{
    using System.Drawing;

    namespace AutoUpdateTools.XmlCompare
    {
        /// <summary>
        /// XML差异显示颜色方案配置
        /// </summary>
        public static class XmlDiffColorScheme
        {
            // 新增文本颜色
            public static Color AddedTextColor { get; } = Color.FromArgb(0, 128, 0); // 深绿
            public static Color RemovedTextColor { get; } = Color.FromArgb(128, 0, 0); // 深红
            public static Color ModifiedTextColor { get; } = Color.Blue;
            public static Color UnchangedTextColor { get; } = Color.Black;

            // 背景色
            public static Color AddedBackColor { get; } = Color.FromArgb(220, 255, 220); // 浅绿
            public static Color RemovedBackColor { get; } = Color.FromArgb(255, 220, 220); // 浅红
            public static Color ModifiedBackColor { get; } = Color.FromArgb(255, 255, 200); // 浅黄
            public static Color UnchangedBackColor { get; } = Color.White;

            // 删除线样式
            public static FontStyle RemovedFontStyle { get; } = FontStyle.Strikeout;
        }
    }
}
