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
    /// <summary>
    /// 调试信息窗口
    /// 用于显示更新过程中的详细日志信息
    /// </summary>
    public partial class frmDebugInfo : Form
    {
        /// <summary>
        /// 标记更新过程是否已完成
        /// </summary>
        public bool IsUpdateCompleted { get; set; } = false;
        
        /// <summary>
        /// 标记是否在调试模式下运行
        /// </summary>
        public bool IsDebugMode { get; set; } = false;

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
        
        /// <summary>
        /// 标记更新过程已完成
        /// </summary>
        /// <param name="success">是否成功</param>
        /// <param name="message">完成消息</param>
        public void MarkUpdateCompleted(bool success, string message = "")
        {
            IsUpdateCompleted = true;
            
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<bool, string>(MarkUpdateCompleted), success, message);
                return;
            }
            
            try
            {
                string status = success ? "成功" : "失败";
                string completionMsg = $"\r\n{"=".Repeat(50)}\r\n";
                completionMsg += $"更新过程已{status}完成! {message}\r\n";
                
                if (IsDebugMode)
                {
                    completionMsg += "调试模式: 窗口将保持打开状态，您可以手动关闭此窗口。\r\n";
                }
                else
                {
                    completionMsg += "窗口将在3秒后自动关闭...\r\n";
                }
                
                completionMsg += $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}\r\n";
                completionMsg += $"{ "=".Repeat(50)}\r\n";
                
                richtxt.AppendText(completionMsg);
                richtxt.SelectionStart = richtxt.TextLength;
                richtxt.ScrollToCaret();
                
                // 更新窗口标题
                this.Text = $"调试信息 - 更新已{status}";
                
                // 如果不是调试模式，设置定时器自动关闭
                if (!IsDebugMode)
                {
                    System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
                    timer.Interval = 3000; // 3秒
                    timer.Tick += (s, e) => {
                        timer.Stop();
                        this.Close();
                    };
                    timer.Start();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"标记更新完成状态失败: {ex.Message}");
            }
        }
    }
    
    /// <summary>
    /// 字符串扩展方法
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// 重复字符串指定次数
        /// </summary>
        /// <param name="str">要重复的字符串</param>
        /// <param name="count">重复次数</param>
        /// <returns>重复后的字符串</returns>
        public static string Repeat(this string str, int count)
        {
            if (count <= 0) return string.Empty;
            return string.Concat(Enumerable.Repeat(str, count));
        }
    }
}
