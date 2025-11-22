using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoUpdate
{
    public partial class frmDebugInfo : Form
    {
        public frmDebugInfo()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 追加日志信息到调试窗口
        /// </summary>
        /// <param name="logMessage">要追加的日志消息</param>
        public void AppendLog(string logMessage)
        {
            if (this.InvokeRequired)
            {
                // 在UI线程上执行
                this.Invoke(new Action<string>(AppendLog), logMessage);
                return;
            }

            try
            {
                // 添加时间戳
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                string formattedLog = $"[{timestamp}] {logMessage}\r\n";
                
                // 追加到RichTextBox
                richtxt.AppendText(formattedLog);
                
                // 滚动到底部
                richtxt.SelectionStart = richtxt.TextLength;
                richtxt.ScrollToCaret();
            }
            catch (Exception ex)
            {
                // 如果日志写入失败，简单地忽略错误以避免影响主程序
                System.Diagnostics.Debug.WriteLine($"写入调试日志失败: {ex.Message}");
            }
        }
    }
}
