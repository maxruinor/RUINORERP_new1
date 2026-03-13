using Markdig;
using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RUINORERP.UI.ToolForm
{
    /// <summary>
    /// 消息内容类型枚举
    /// </summary>
    public enum MessageContentType
    {
        /// <summary>
        /// 纯文本
        /// </summary>
        Text,
        /// <summary>
        /// Markdown格式
        /// </summary>
        Markdown
    }

    /// <summary>
    /// 消息查看器窗体
    /// 用于显示大量提示信息，支持Markdown和纯文本格式
    /// </summary>
    public partial class frmMessageViewer : Krypton.Toolkit.KryptonForm
    {
        /// <summary>
        /// 消息内容类型
        /// </summary>
        public MessageContentType ContentType { get; set; } = MessageContentType.Text;

        /// <summary>
        /// 消息内容
        /// </summary>
        public string MessageContent { get; set; }

        /// <summary>
        /// 确定按钮文本
        /// </summary>
        public string OkButtonText
        {
            get => btnOk.Values.Text;
            set => btnOk.Values.Text = value;
        }

        /// <summary>
        /// 取消按钮文本
        /// </summary>
        public string CancelButtonText
        {
            get => btnCancel.Values.Text;
            set => btnCancel.Values.Text = value;
        }

        /// <summary>
        /// 是否显示取消按钮
        /// </summary>
        public bool ShowCancelButton
        {
            get => btnCancel.Visible;
            set => btnCancel.Visible = value;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public frmMessageViewer()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        private void frmMessageViewer_Load(object sender, EventArgs e)
        {
            DisplayContent();
        }

        /// <summary>
        /// 显示内容
        /// </summary>
        private void DisplayContent()
        {
            if (string.IsNullOrEmpty(MessageContent))
            {
                txtContent.Text = "无内容";
                return;
            }

            if (ContentType == MessageContentType.Markdown)
            {
                try
                {
                    var pipeline = new MarkdownPipelineBuilder()
                        .UseAdvancedExtensions()
                        .Build();

                    string html = Markdown.ToHtml(MessageContent, pipeline);
                    string plainText = ConvertHtmlToPlainText(html);
                    txtContent.Text = plainText;
                }
                catch (Exception ex)
                {
                    txtContent.Text = $"Markdown解析失败：{ex.Message}\r\n\r\n原始内容：\r\n{MessageContent}";
                }
            }
            else
            {
                txtContent.Text = MessageContent;
            }
        }

        /// <summary>
        /// 将HTML转换为纯文本
        /// </summary>
        /// <param name="html">HTML内容</param>
        /// <returns>纯文本内容</returns>
        private string ConvertHtmlToPlainText(string html)
        {
            if (string.IsNullOrEmpty(html))
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder();
            bool inTag = false;

            foreach (char c in html)
            {
                switch (c)
                {
                    case '<':
                        inTag = true;
                        break;
                    case '>':
                        inTag = false;
                        sb.Append(' ');
                        break;
                    case '&':
                        if (html.Contains("&nbsp;"))
                        {
                            sb.Append(' ');
                        }
                        break;
                    default:
                        if (!inTag)
                        {
                            sb.Append(c);
                        }
                        break;
                }
            }

            string result = sb.ToString();
            result = System.Net.WebUtility.HtmlDecode(result);
            result = System.Text.RegularExpressions.Regex.Replace(result, @"\s+", " ").Trim();
            result = result.Replace("<br/>", "\r\n").Replace("<br>", "\r\n");
            result = System.Text.RegularExpressions.Regex.Replace(result, @"</p>\s*<p>", "\r\n\r\n");
            result = System.Text.RegularExpressions.Regex.Replace(result, @"<[^>]+>", "");
            result = System.Text.RegularExpressions.Regex.Replace(result, @"&nbsp;", " ");

            return result;
        }

        /// <summary>
        /// 显示消息查看器（纯文本）
        /// </summary>
        /// <param name="title">窗体标题</param>
        /// <param name="message">消息内容</param>
        /// <param name="owner">父窗口</param>
        /// <returns>对话框结果</returns>
        public static DialogResult Show(string title, string message, IWin32Window owner = null)
        {
            return Show(title, message, MessageContentType.Text, "确定", "取消", true, owner);
        }

        /// <summary>
        /// 显示消息查看器（支持Markdown）
        /// </summary>
        /// <param name="title">窗体标题</param>
        /// <param name="message">消息内容</param>
        /// <param name="contentType">内容类型</param>
        /// <param name="owner">父窗口</param>
        /// <returns>对话框结果</returns>
        public static DialogResult Show(string title, string message, MessageContentType contentType, IWin32Window owner = null)
        {
            return Show(title, message, contentType, "确定", "取消", true, owner);
        }

        /// <summary>
        /// 显示消息查看器（完整参数）
        /// </summary>
        /// <param name="title">窗体标题</param>
        /// <param name="message">消息内容</param>
        /// <param name="contentType">内容类型</param>
        /// <param name="okButtonText">确定按钮文本</param>
        /// <param name="cancelButtonText">取消按钮文本</param>
        /// <param name="showCancelButton">是否显示取消按钮</param>
        /// <param name="owner">父窗口</param>
        /// <returns>对话框结果</returns>
        public static DialogResult Show(string title, string message, MessageContentType contentType,
            string okButtonText, string cancelButtonText, bool showCancelButton, IWin32Window owner = null)
        {
            using (var form = new frmMessageViewer())
            {
                form.Text = title;
                form.MessageContent = message;
                form.ContentType = contentType;
                form.OkButtonText = okButtonText;
                form.CancelButtonText = cancelButtonText;
                form.ShowCancelButton = showCancelButton;

                if (owner != null)
                {
                    return form.ShowDialog(owner);
                }
                else
                {
                    return form.ShowDialog();
                }
            }
        }

        /// <summary>
        /// 确定按钮点击事件
        /// </summary>
        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// 取消按钮点击事件
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
