using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.IIS
{
    public class LogViewer : RichTextBox
    {
        public LogViewer()
        {
            // 设置 RichTextBox 控件的属性
            this.ReadOnly = false; // 只读，不允许用户编辑
            this.WordWrap = false; // 不自动换行
            this.ScrollBars = RichTextBoxScrollBars.Vertical; // 只显示垂直滚动条
            this.BackColor = Color.White; // 背景颜色
            this.Font = new Font("Consolas", 9); // 设置字体
        }

        public void AddLog(string message)
        {
            List<string> linesList = new List<string>(this.Lines); // Convert array to List

            // 获取当前行数
            int currentLineCount = linesList.Count;

            // 如果超过 100 行，则移除最老的日志
            while (currentLineCount >= 100)
            {
                linesList.RemoveAt(0);
                currentLineCount--;
            }

            this.Lines = linesList.ToArray(); // Convert back to array

            // 添加新日志到顶部
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            this.AppendText($"{timestamp} - {message}\r\n");
            this.SelectionStart = this.TextLength; // 移动光标到文本朾
            this.ScrollToCaret(); // 滚动到光标位置
        }
    }
}
