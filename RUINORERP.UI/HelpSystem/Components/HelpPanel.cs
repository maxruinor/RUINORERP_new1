using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace RUINORERP.UI.HelpSystem.Components
{
    /// <summary>
    /// 帮助面板窗体
    /// 用于显示详细的HTML帮助内容
    /// 支持工具栏、打印、打开CHM等功能
    /// </summary>
    public class HelpPanel : Form
    {
        #region 私有字段

        /// <summary>
        /// Web浏览器控件,用于显示HTML内容
        /// </summary>
        private WebBrowser _webBrowser;

        /// <summary>
        /// 工具栏
        /// </summary>
        private ToolStrip _toolStrip;

        /// <summary>
        /// 关闭按钮
        /// </summary>
        private ToolStripButton _btnClose;

        /// <summary>
        /// 打印按钮
        /// </summary>
        private ToolStripButton _btnPrint;

        /// <summary>
        /// 打开CHM按钮
        /// </summary>
        private ToolStripButton _btnOpenCHM;

        /// <summary>
        /// 帮助上下文
        /// </summary>
        private Core.HelpContext _helpContext;

        /// <summary>
        /// 已释放标志
        /// </summary>
        private bool _disposed = false;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="content">帮助内容(HTML格式)</param>
        /// <param name="context">帮助上下文</param>
        public HelpPanel(string content, Core.HelpContext context)
        {
            _helpContext = context;
            InitializeComponent();
            LoadHelpContent(content, context);
        }

        #endregion

        #region 初始化方法

        /// <summary>
        /// 初始化组件
        /// </summary>
        private void InitializeComponent()
        {
            // 设置窗体属性
            this.Text = "帮助";
            this.Size = new Size(900, 700);
            this.StartPosition = FormStartPosition.CenterParent;
            this.MinimumSize = new Size(600, 400);
            this.KeyPreview = true;

            // 创建工具栏
            _toolStrip = new ToolStrip
            {
                GripStyle = ToolStripGripStyle.Hidden,
                RenderMode = ToolStripRenderMode.System
            };

            // 创建工具栏按钮
            _btnClose = new ToolStripButton("关闭");
            _btnClose.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _btnClose.Click += (s, e) => this.Close();

            _btnPrint = new ToolStripButton("打印");
            _btnPrint.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _btnPrint.Click += BtnPrint_Click;

            _btnOpenCHM = new ToolStripButton("打开CHM");
            _btnOpenCHM.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _btnOpenCHM.Click += BtnOpenCHM_Click;

            // 添加按钮到工具栏
            _toolStrip.Items.AddRange(new ToolStripItem[] 
            { 
                _btnClose, 
                new ToolStripSeparator(),
                _btnPrint, 
                _btnOpenCHM 
            });

            // 创建Web浏览器控件
            _webBrowser = new WebBrowser
            {
                Dock = DockStyle.Fill,
                WebBrowserShortcutsEnabled = false,
                IsWebBrowserContextMenuEnabled = true,
                AllowWebBrowserDrop = false,
                ScriptErrorsSuppressed = true
            };

            // 布局控件
            this.Controls.Add(_webBrowser);
            this.Controls.Add(_toolStrip);
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 加载帮助内容
        /// </summary>
        /// <param name="content">帮助内容</param>
        /// <param name="context">帮助上下文</param>
        public void LoadHelpContent(string content, Core.HelpContext context)
        {
            try
            {
                // 设置窗体标题
                this.Text = GenerateTitle(context);

                // 包装内容为完整的HTML
                string html = WrapContent(content, context);

                // 创建临时文件
                string tempFile = Path.Combine(
                    Path.GetTempPath(), 
                    $"RUINOR_Help_{Guid.NewGuid()}.html");

                // 写入文件
                File.WriteAllText(tempFile, html, Encoding.UTF8);

                // 加载到浏览器
                _webBrowser.Navigate(new Uri($"file:///{tempFile.Replace('\\', '/')}"));

                // 注册临时文件清理事件
                this.FormClosed += (s, e) => CleanupTempFile(tempFile);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"加载帮助内容失败: {ex.Message}", 
                    "错误", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 刷新帮助内容
        /// </summary>
        public void RefreshContent()
        {
            try
            {
                _webBrowser.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"刷新帮助内容失败: {ex.Message}", 
                    "错误", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 生成窗体标题
        /// </summary>
        /// <param name="context">帮助上下文</param>
        /// <returns>标题文本</returns>
        private string GenerateTitle(Core.HelpContext context)
        {
            if (context == null)
            {
                return "帮助";
            }

            switch (context.Level)
            {
                case Core.HelpLevel.Form:
                    return $"帮助 - {context.FormType?.Name ?? "窗体"}";

                case Core.HelpLevel.Control:
                    return $"帮助 - {context.ControlName ?? "控件"}";

                case Core.HelpLevel.Field:
                    return $"帮助 - {context.FieldName ?? "字段"}";

                case Core.HelpLevel.Module:
                    return $"帮助 - {context.ModuleName ?? "模块"}";

                default:
                    return "帮助";
            }
        }

        /// <summary>
        /// 将内容包装为完整的HTML
        /// </summary>
        /// <param name="content">原始内容</param>
        /// <param name="context">帮助上下文</param>
        /// <returns>完整的HTML</returns>
        private string WrapContent(string content, Core.HelpContext context)
        {
            string title = GenerateTitle(context);

            return $@"<!DOCTYPE html>
<html lang=""zh-CN"">
<head>
    <meta charset=""utf-8"" />
    <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"" />
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"" />
    <title>{title}</title>
    <style>
        * {{
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }}
        
        body {{
            font-family: 'Microsoft YaHei', 'Segoe UI', Arial, sans-serif;
            font-size: 14px;
            line-height: 1.8;
            color: #333;
            padding: 20px;
            background: #fff;
        }}
        
        h1 {{
            color: #2c3e50;
            font-size: 28px;
            margin: 0 0 20px 0;
            padding-bottom: 10px;
            border-bottom: 3px solid #3498db;
        }}
        
        h2 {{
            color: #34495e;
            font-size: 22px;
            margin: 30px 0 15px 0;
            padding-left: 10px;
            border-left: 4px solid #3498db;
        }}
        
        h3 {{
            color: #2c3e50;
            font-size: 18px;
            margin: 20px 0 10px 0;
        }}
        
        p {{
            margin: 10px 0;
            text-align: justify;
        }}
        
        ul, ol {{
            margin: 10px 0 10px 30px;
        }}
        
        li {{
            margin: 5px 0;
        }}
        
        code {{
            background: #f4f4f4;
            padding: 2px 6px;
            border-radius: 3px;
            font-family: 'Consolas', 'Courier New', monospace;
            font-size: 0.9em;
            color: #c7254e;
        }}
        
        pre {{
            background: #f4f4f4;
            padding: 15px;
            border-radius: 5px;
            overflow-x: auto;
            margin: 15px 0;
        }}
        
        pre code {{
            background: none;
            padding: 0;
            color: #333;
        }}
        
        table {{
            border-collapse: collapse;
            width: 100%;
            margin: 20px 0;
            font-size: 13px;
        }}
        
        th, td {{
            border: 1px solid #ddd;
            padding: 12px 15px;
            text-align: left;
        }}
        
        th {{
            background: #f2f2f2;
            font-weight: bold;
            color: #333;
        }}
        
        tr:nth-child(even) {{
            background: #f9f9f9;
        }}
        
        tr:hover {{
            background: #f5f5f5;
        }}
        
        blockquote {{
            background: #f8f9fa;
            border-left: 4px solid #3498db;
            padding: 10px 15px;
            margin: 15px 0;
            color: #555;
        }}
        
        strong {{
            color: #2c3e50;
        }}
        
        a {{
            color: #3498db;
            text-decoration: none;
        }}
        
        a:hover {{
            text-decoration: underline;
        }}
        
        .note {{
            background: #fff3cd;
            border-left: 4px solid #ffc107;
            padding: 10px 15px;
            margin: 15px 0;
        }}
        
        .tip {{
            background: #d4edda;
            border-left: 4px solid #28a745;
            padding: 10px 15px;
            margin: 15px 0;
        }}
        
        .warning {{
            background: #f8d7da;
            border-left: 4px solid #dc3545;
            padding: 10px 15px;
            margin: 15px 0;
        }}
    </style>
</head>
<body>
    {content}
    
    <script>
        // 禁用右键菜单中的某些选项
        document.addEventListener('contextmenu', function(e) {{
            e.preventDefault();
        }});
        
        // 处理链接点击,在新窗口打开
        document.addEventListener('click', function(e) {{
            if (e.target.tagName === 'A' && e.target.href) {{
                e.preventDefault();
                window.open(e.target.href, '_blank');
            }}
        }});
    </script>
</body>
</html>";
        }

        /// <summary>
        /// 清理临时文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        private void CleanupTempFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"删除临时文件失败: {ex.Message}");
            }
        }

        #endregion

        #region 事件处理

        /// <summary>
        /// 打印按钮点击事件
        /// </summary>
        private void BtnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                _webBrowser.Print();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"打印失败: {ex.Message}", 
                    "错误", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 打开CHM按钮点击事件
        /// </summary>
        private void BtnOpenCHM_Click(object sender, EventArgs e)
        {
            try
            {
                string chmPath = Path.Combine(
                    Application.StartupPath, 
                    "HelpOutput", 
                    "CHM", 
                    "Help.chm");

                if (File.Exists(chmPath))
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = chmPath,
                        UseShellExecute = true
                    });
                }
                else
                {
                    MessageBox.Show(
                        "未找到CHM帮助文件。\n\n" +
                        "文件路径: " + chmPath, 
                        "提示", 
                        MessageBoxButtons.OK, 
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"打开CHM文件失败: {ex.Message}", 
                    "错误", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 重写ProcessCmdKey方法,处理快捷键
        /// </summary>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }

            if (keyData == (Keys.Control | Keys.P))
            {
                BtnPrint_Click(null, EventArgs.Empty);
                return true;
            }

            if (keyData == Keys.F5)
            {
                RefreshContent();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion

        #region 重写方法

        /// <summary>
        /// 重写OnFormClosing方法
        /// </summary>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // 停止Web浏览器导航
            try
            {
                _webBrowser.Stop();
            }
            catch
            {
                // 忽略异常
            }

            base.OnFormClosing(e);
        }

        #endregion

        #region IDisposable 实现

        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // 释放托管资源
                    _toolStrip?.Dispose();
                    _btnClose?.Dispose();
                    _btnPrint?.Dispose();
                    _btnOpenCHM?.Dispose();
                    _webBrowser?.Dispose();
                }

                _disposed = true;
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
