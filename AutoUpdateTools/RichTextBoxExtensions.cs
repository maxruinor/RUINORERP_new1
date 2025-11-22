using System;
using System.Drawing;
using System.Windows.Forms;

namespace AutoUpdateTools
{
    /// <summary>
    /// RichTextBox的扩展方法，方便添加带颜色的文本
    /// </summary>
    public static class RichTextBoxExtensions
    {
        /// <summary>
        /// 向RichTextBox添加带颜色的文本
        /// </summary>
        /// <param name="box">RichTextBox控件</param>
        /// <param name="text">要添加的文本</param>
        /// <param name="color">文本颜色</param>
        public static void AppendText(this RichTextBox box, string text, Color color)
        {
            if (box == null || string.IsNullOrEmpty(text)) return;
            
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;
            box.SelectionColor = color;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
        }

        /// <summary>
        /// 向RichTextBox添加文本
        /// </summary>
        /// <param name="box">RichTextBox控件</param>
        /// <param name="text">要添加的文本</param>
        public static void AppendText(this RichTextBox box, string text)
        {
            if (box == null || string.IsNullOrEmpty(text)) return;
            box.AppendText(text);
        }
    }
}