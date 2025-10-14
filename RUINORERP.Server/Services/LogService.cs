using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.Server.Services
{
    /// <summary>
    /// 日志服务实现类
    /// </summary>
    public class LogService : ILogService
    {
        private readonly ILogger<LogService> _logger;
        private RichTextBox _logTextBox;

        public LogService(ILogger<LogService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 设置日志文本框控件
        /// </summary>
        /// <param name="logTextBox">日志文本框</param>
        public void SetLogTextBox(RichTextBox logTextBox)
        {
            _logTextBox = logTextBox;
        }

        /// <summary>
        /// 打印信息日志
        /// </summary>
        /// <param name="message">日志消息</param>
        public void PrintInfoLog(string message)
        {
            // 记录到文件日志
            _logger?.LogInformation(message);

            // 记录到UI日志控件
            if (_logTextBox != null && !_logTextBox.IsDisposed && _logTextBox.IsHandleCreated)
            {
                try
                {
                    // 确保最多只有1000行
                    EnsureMaxLines(_logTextBox, 1000);

                    // 将消息格式化为带时间戳和行号的字符串
                    string formattedMsg = $"[{DateTime.Now:HH:mm:ss}] {message}\r\n";

                    if (_logTextBox.InvokeRequired)
                    {
                        _logTextBox.BeginInvoke(new MethodInvoker(() =>
                        {
                            _logTextBox.SelectionColor = Color.Black;
                            _logTextBox.AppendText(formattedMsg);
                            _logTextBox.SelectionColor = Color.Black;
                            _logTextBox.ScrollToCaret(); // 滚动到最新的消息
                        }));
                    }
                    else
                    {
                        _logTextBox.SelectionColor = Color.Black;
                        _logTextBox.AppendText(formattedMsg);
                        _logTextBox.SelectionColor = Color.Black;
                        _logTextBox.ScrollToCaret(); // 滚动到最新的消息
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("PrintInfoLog时出错" + ex.Message);
                }
            }
        }

        /// <summary>
        /// 打印错误日志
        /// </summary>
        /// <param name="message">错误消息</param>
        public void PrintErrorLog(string message)
        {
            // 记录到文件日志
            _logger?.LogError(message);

            // 记录到UI日志控件
            if (_logTextBox != null && !_logTextBox.IsDisposed && _logTextBox.IsHandleCreated)
            {
                try
                {
                    // 确保最多只有1000行
                    EnsureMaxLines(_logTextBox, 1000);

                    // 将消息格式化为带时间戳和行号的字符串
                    string formattedMsg = $"[{DateTime.Now:HH:mm:ss}] [错误] {message}\r\n";

                    if (_logTextBox.InvokeRequired)
                    {
                        _logTextBox.BeginInvoke(new MethodInvoker(() =>
                        {
                            _logTextBox.SelectionColor = Color.Red;
                            _logTextBox.AppendText(formattedMsg);
                            _logTextBox.SelectionColor = Color.Black;
                            _logTextBox.ScrollToCaret(); // 滚动到最新的消息
                        }));
                    }
                    else
                    {
                        _logTextBox.SelectionColor = Color.Red;
                        _logTextBox.AppendText(formattedMsg);
                        _logTextBox.SelectionColor = Color.Black;
                        _logTextBox.ScrollToCaret(); // 滚动到最新的消息
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("PrintErrorLog时出错" + ex.Message);
                }
            }
        }

        /// <summary>
        /// 确保日志行数不超过最大限制
        /// </summary>
        /// <param name="rtb">日志文本框</param>
        /// <param name="maxLines">最大行数</param>
        private void EnsureMaxLines(RichTextBox rtb, int maxLines)
        {
            // 确保所有控件访问都在UI线程中进行
            if (rtb.InvokeRequired)
            {
                rtb.BeginInvoke(new MethodInvoker(() =>
                {
                    EnsureMaxLines(rtb, maxLines);
                }));
                return;
            }

            try
            {
                // 计算当前的行数
                int currentLines = rtb.GetLineFromCharIndex(rtb.Text.Length) + 1;

                // 如果行数超过了最大限制，则删除旧的行
                if (currentLines > maxLines)
                {
                    int linesToRemove = currentLines - maxLines;
                    int start = rtb.GetFirstCharIndexFromLine(0);
                    int end = rtb.GetFirstCharIndexFromLine(linesToRemove);

                    // 确保索引有效
                    if (start >= 0 && end > start && end <= rtb.Text.Length)
                    {
                        rtb.Text = rtb.Text.Remove(start, end - start);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EnsureMaxLines时出错: {ex.Message}");
            }
        }
    }
}