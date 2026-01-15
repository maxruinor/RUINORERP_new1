using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;

namespace RUINORERP.UI.HelpSystem.Components
{
    /// <summary>
    /// WebView2 帮助面板窗体
    /// 使用 WebView2 控件提供现代化的帮助显示体验
    /// 支持富文本、Markdown 渲染、代码语法高亮等功能
    /// </summary>
    public class WebView2HelpPanel : Form
    {
        #region 私有字段

        /// <summary>
        /// WebView2 控件
        /// </summary>
        private WebView2 _webView2;

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
        /// 导航后退按钮
        /// </summary>
        private ToolStripButton _btnBack;

        /// <summary>
        /// 导航前进按钮
        /// </summary>
        private ToolStripButton _btnForward;

        /// <summary>
        /// 刷新按钮
        /// </summary>
        private ToolStripButton _btnRefresh;

        /// <summary>
        /// 缩小按钮
        /// </summary>
        private ToolStripButton _btnZoomOut;

        /// <summary>
        /// 放大按钮
        /// </summary>
        private ToolStripButton _btnZoomIn;

        /// <summary>
        /// 帮助上下文
        /// </summary>
        private Core.HelpContext _helpContext;

        /// <summary>
        /// Markdown 渲染器
        /// </summary>
        private MarkdownRenderer _markdownRenderer;

        /// <summary>
        /// 当前缩放级别
        /// </summary>
        private double _zoomLevel = 100;

        /// <summary>
        /// WebView2 初始化完成标志
        /// </summary>
        private bool _webViewInitialized = false;

        /// <summary>
        /// 待加载的内容
        /// </summary>
        private string _pendingContent;

        /// <summary>
        /// 已释放标志
        /// </summary>
        private bool _disposed = false;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="content">帮助内容</param>
        /// <param name="context">帮助上下文</param>
        public WebView2HelpPanel(string content, Core.HelpContext context)
        {

            InitializeComponent();
            if (!this.DesignMode)
            {
                InitializeWebView2Async();
                _helpContext = context;
                _markdownRenderer = new MarkdownRenderer();
                // 保存待加载内容
                _pendingContent = content;
            }
            
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
            this.Size = new Size(1000, 800);
            this.StartPosition = FormStartPosition.CenterParent;
            this.MinimumSize = new Size(700, 500);
            this.KeyPreview = true;
            // this.Icon = System.Drawing.Icon.FromHandle(Properties.Resources.FolderOpen_32x32.GetHicon()); // 注释掉不存在的资源

            // 创建工具栏
            _toolStrip = new ToolStrip
            {
                GripStyle = ToolStripGripStyle.Hidden,
                RenderMode = ToolStripRenderMode.System,
                BackColor = Color.FromArgb(245, 245, 245)
            };

            // 创建工具栏按钮
            _btnClose = new ToolStripButton("关闭");
            _btnClose.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _btnClose.Click += (s, e) => this.Close();

            _btnBack = new ToolStripButton("后退");
            _btnBack.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _btnBack.Enabled = false;
            _btnBack.Click += (s, e) => _webView2.CoreWebView2.GoBack();

            _btnForward = new ToolStripButton("前进");
            _btnForward.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _btnForward.Enabled = false;
            _btnForward.Click += (s, e) => _webView2.CoreWebView2.GoForward();

            _btnRefresh = new ToolStripButton("刷新");
            _btnRefresh.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _btnRefresh.Click += (s, e) => 
            {
                _webView2.CoreWebView2.Reload();
                if (!string.IsNullOrEmpty(_pendingContent))
                {
                    LoadHelpContent(_pendingContent, _helpContext);
                }
            };

            _btnZoomOut = new ToolStripButton("-");
            _btnZoomOut.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _btnZoomOut.Click += (s, e) => ChangeZoom(-10);

            _btnZoomIn = new ToolStripButton("+");
            _btnZoomIn.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _btnZoomIn.Click += (s, e) => ChangeZoom(10);

            _btnPrint = new ToolStripButton("打印");
            _btnPrint.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _btnPrint.Click += async (s, e) => await BtnPrint_ClickAsync();

            _btnOpenCHM = new ToolStripButton("打开CHM");
            _btnOpenCHM.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _btnOpenCHM.Click += BtnOpenCHM_Click;

            // 添加按钮到工具栏
            _toolStrip.Items.AddRange(new ToolStripItem[] 
            { 
                _btnClose,
                new ToolStripSeparator(),
                _btnBack, 
                _btnForward, 
                _btnRefresh,
                new ToolStripSeparator(),
                _btnZoomOut,
                _btnZoomIn,
                new ToolStripSeparator(),
                _btnPrint, 
                _btnOpenCHM 
            });

            // 创建 WebView2 控件
            _webView2 = new WebView2
            {
                Dock = DockStyle.Fill,
                DefaultBackgroundColor = Color.White
            };

            // 布局控件
            this.Controls.Add(_webView2);
            this.Controls.Add(_toolStrip);
        }

        /// <summary>
        /// 异步初始化 WebView2
        /// </summary>
        private async void InitializeWebView2Async()
        {
            try
            {
                // 创建 WebView2 环境
                var environment = await CoreWebView2Environment.CreateAsync();
                
                // 初始化 WebView2
                await _webView2.EnsureCoreWebView2Async(environment);
                
                // 配置 WebView2
                ConfigureWebView2();
                
                // 标记为已初始化
                _webViewInitialized = true;
                
                // 如果有待加载内容，现在加载
                if (!string.IsNullOrEmpty(_pendingContent))
                {
                    LoadHelpContent(_pendingContent, _helpContext);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"初始化 WebView2 失败: {ex.Message}\n\n" +
                    "请确保已安装 Microsoft Edge WebView2 Runtime。\n" +
                    "下载地址: https://go.microsoft.com/fwlink/p/?LinkId=2124703",
                    "错误",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                
                // 降级到 WebBrowser
                FallbackToWebBrowser();
            }
        }

        /// <summary>
        /// 配置 WebView2 设置
        /// </summary>
        private void ConfigureWebView2()
        {
            try
            {
                // 禁用默认上下文菜单
                _webView2.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
                
                // 启用开发工具（调试用）
                _webView2.CoreWebView2.Settings.AreDevToolsEnabled = true;
                
                // 禁用弹窗阻止
                _webView2.CoreWebView2.Settings.AreHostObjectsAllowed = true;
                
                // 设置用户代理
                _webView2.CoreWebView2.Settings.UserAgent = "RUINORERP-HelpSystem/1.0";
                
                // 监听导航事件
                _webView2.CoreWebView2.NavigationStarting += CoreWebView2_NavigationStarting;
                _webView2.CoreWebView2.NavigationCompleted += CoreWebView2_NavigationCompleted;
                _webView2.CoreWebView2.NewWindowRequested += CoreWebView2_NewWindowRequested;
                
                // 注入 JavaScript 脚本
                InjectJavaScript();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"配置 WebView2 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 注入 JavaScript 脚本
        /// </summary>
        private void InjectJavaScript()
        {
            try
            {
                // 注入语法高亮脚本
                string script = @"
                    // 代码语法高亮函数
                    function highlightCode() {
                        var codeBlocks = document.querySelectorAll('pre code');
                        codeBlocks.forEach(function(block) {
                            // 简单的语法高亮
                            var code = block.textContent;
                            code = code
                                .replace(/&/g, '&amp;')
                                .replace(/</g, '&lt;')
                                .replace(/>/g, '&gt;')
                                .replace(/""(.*?)""/g, '<span class=""string"">""$1""</span>')
                                .replace(/'(.*?)'/g, '<span class=""string"">'$1'</span>')
                                .replace(/\b(function|var|let|const|if|else|for|while|return|class|public|private|protected|static|void|int|string|bool|true|false|null)\b/g, '<span class=""keyword"">$1</span>')
                                .replace(/\/\/(.*)/g, '<span class=""comment"">//$1</span>')
                                .replace(/\/\*([\s\S]*?)\*\//g, '<span class=""comment"">/*$1*/</span>')
                                .replace(/\b(\d+)\b/g, '<span class=""number"">$1</span>');
                            block.innerHTML = code;
                        });
                    }
                    
                    // 在页面加载完成后执行高亮
                    window.addEventListener('load', function() {
                        setTimeout(highlightCode, 100);
                    });
                ";
                
                _webView2.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync(script);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"注入 JavaScript 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 降级到 WebBrowser 控件
        /// </summary>
        private void FallbackToWebBrowser()
        {
            try
            {
                // 移除 WebView2
                this.Controls.Remove(_webView2);
                _webView2?.Dispose();
                
                // 创建 WebBrowser
                var webBrowser = new System.Windows.Forms.WebBrowser
                {
                    Dock = DockStyle.Fill,
                    ScriptErrorsSuppressed = true
                };
                
                this.Controls.Add(webBrowser);
                
                // 使用旧方式加载内容
                if (!string.IsNullOrEmpty(_pendingContent))
                {
                    string html = WrapContent(_pendingContent, _helpContext);
                    string tempFile = Path.Combine(
                        Path.GetTempPath(),
                        $"RUINOR_Help_{Guid.NewGuid()}.html");
                    File.WriteAllText(tempFile, html, Encoding.UTF8);
                    webBrowser.Navigate(new Uri($"file:///{tempFile.Replace('\\', '/')}"));
                    
                    this.FormClosed += (s, e) =>
                    {
                        try
                        {
                            if (File.Exists(tempFile))
                            {
                                File.Delete(tempFile);
                            }
                        }
                        catch { }
                    };
                }
                
                // 更新工具栏
                _btnBack.Enabled = false;
                _btnForward.Enabled = false;
                _btnRefresh.Click += (s, e) => webBrowser.Refresh();
                _btnPrint.Click += (s, e) => webBrowser.Print();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"降级到 WebBrowser 失败: {ex.Message}");
            }
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
                if (!_webViewInitialized)
                {
                    // WebView2 尚未初始化，保存内容等待初始化完成
                    _pendingContent = content;
                    return;
                }

                // 设置窗体标题
                this.Text = GenerateTitle(context);

                // 判断内容类型并转换
                string html;
                if (IsMarkdownContent(content))
                {
                    // Markdown 内容转换为 HTML
                    html = _markdownRenderer.ToHtml(content);
                }
                else
                {
                    // 普通内容包装为 HTML
                    html = WrapContent(content, context);
                }

                // 清除待加载内容
                _pendingContent = null;

                // 加载到 WebView2
                _webView2.NavigateToString(html);
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
        /// 加载 URL 内容
        /// </summary>
        /// <param name="url">URL 地址</param>
        public async Task LoadUrlAsync(string url)
        {
            try
            {
                if (!_webViewInitialized)
                {
                    System.Diagnostics.Debug.WriteLine("WebView2 尚未初始化");
                    return;
                }

                _webView2.CoreWebView2.Navigate(url);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"加载 URL 失败: {ex.Message}",
                    "错误",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 刷新帮助内容
        /// </summary>
        public async Task RefreshContentAsync()
        {
            try
            {
                if (_webViewInitialized)
                {
                    _webView2.CoreWebView2.Reload();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"刷新帮助内容失败: {ex.Message}");
            }
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 判断是否为 Markdown 内容
        /// </summary>
        private bool IsMarkdownContent(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return false;
            }

            // 检查常见的 Markdown 语法标记
            return content.StartsWith("#") ||
                   content.Contains("##") ||
                   content.Contains("###") ||
                   content.Contains("**") ||
                   content.Contains("```");
        }

        /// <summary>
        /// 生成窗体标题
        /// </summary>
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
        /// 将内容包装为完整的 HTML
        /// </summary>
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
            font-family: 'Microsoft YaHei', 'Segoe UI', 'PingFang SC', -apple-system, BlinkMacSystemFont, Arial, sans-serif;
            font-size: 15px;
            line-height: 1.8;
            color: #2c3e50;
            padding: 30px;
            background: #fff;
            max-width: 1200px;
            margin: 0 auto;
        }}
        
        h1 {{
            color: #2c3e50;
            font-size: 32px;
            margin: 0 0 25px 0;
            padding-bottom: 15px;
            border-bottom: 3px solid #3498db;
            font-weight: 600;
        }}
        
        h2 {{
            color: #34495e;
            font-size: 24px;
            margin: 35px 0 20px 0;
            padding-left: 15px;
            border-left: 5px solid #3498db;
            font-weight: 600;
        }}
        
        h3 {{
            color: #2c3e50;
            font-size: 20px;
            margin: 25px 0 15px 0;
            font-weight: 600;
        }}
        
        h4 {{
            color: #34495e;
            font-size: 18px;
            margin: 20px 0 12px 0;
            font-weight: 600;
        }}
        
        p {{
            margin: 12px 0;
            text-align: justify;
            line-height: 1.8;
        }}
        
        ul, ol {{
            margin: 12px 0 12px 35px;
        }}
        
        li {{
            margin: 8px 0;
            line-height: 1.8;
        }}
        
        code {{
            background: #f8f9fa;
            padding: 3px 8px;
            border-radius: 4px;
            font-family: 'Consolas', 'Monaco', 'Courier New', monospace;
            font-size: 0.9em;
            color: #e74c3c;
            border: 1px solid #e9ecef;
        }}
        
        pre {{
            background: #282c34;
            color: #abb2bf;
            padding: 20px;
            border-radius: 8px;
            overflow-x: auto;
            margin: 20px 0;
            box-shadow: 0 2px 8px rgba(0,0,0,0.1);
        }}
        
        pre code {{
            background: none;
            padding: 0;
            color: inherit;
            border: none;
            font-size: 14px;
            line-height: 1.6;
        }}
        
        .keyword {{
            color: #c678dd;
            font-weight: bold;
        }}
        
        .string {{
            color: #98c379;
        }}
        
        .comment {{
            color: #5c6370;
            font-style: italic;
        }}
        
        .number {{
            color: #d19a66;
        }}
        
        table {{
            border-collapse: collapse;
            width: 100%;
            margin: 25px 0;
            font-size: 14px;
            box-shadow: 0 2px 8px rgba(0,0,0,0.05);
        }}
        
        th, td {{
            border: 1px solid #ddd;
            padding: 15px 20px;
            text-align: left;
        }}
        
        th {{
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            font-weight: 600;
            color: white;
        }}
        
        tr:nth-child(even) {{
            background: #f8f9fa;
        }}
        
        tr:hover {{
            background: #e9ecef;
        }}
        
        blockquote {{
            background: linear-gradient(135deg, #667eea15 0%, #764ba215 100%);
            border-left: 5px solid #667eea;
            padding: 15px 20px;
            margin: 20px 0;
            color: #495057;
            border-radius: 0 8px 8px 0;
        }}
        
        strong {{
            color: #2c3e50;
            font-weight: 600;
        }}
        
        em {{
            color: #6c757d;
        }}
        
        a {{
            color: #3498db;
            text-decoration: none;
            border-bottom: 1px dotted #3498db;
            transition: all 0.3s;
        }}
        
        a:hover {{
            color: #2980b9;
            border-bottom: 1px solid #2980b9;
        }}
        
        .note {{
            background: #fff3cd;
            border-left: 5px solid #ffc107;
            padding: 15px 20px;
            margin: 20px 0;
            border-radius: 0 8px 8px 0;
        }}
        
        .tip {{
            background: #d4edda;
            border-left: 5px solid #28a745;
            padding: 15px 20px;
            margin: 20px 0;
            border-radius: 0 8px 8px 0;
        }}
        
        .warning {{
            background: #f8d7da;
            border-left: 5px solid #dc3545;
            padding: 15px 20px;
            margin: 20px 0;
            border-radius: 0 8px 8px 0;
        }}
        
        .info {{
            background: #d1ecf1;
            border-left: 5px solid #17a2b8;
            padding: 15px 20px;
            margin: 20px 0;
            border-radius: 0 8px 8px 0;
        }}
        
        hr {{
            border: none;
            height: 2px;
            background: linear-gradient(90deg, #667eea 0%, #764ba2 100%);
            margin: 30px 0;
        }}
        
        @media print {{
            body {{
                padding: 0;
            }}
            
            .no-print {{
                display: none;
            }}
        }}
    </style>
</head>
<body>
    {content}
</body>
</html>";
        }

        /// <summary>
        /// 改变缩放级别
        /// </summary>
        private void ChangeZoom(int delta)
        {
            try
            {
                _zoomLevel = Math.Max(50, Math.Min(200, _zoomLevel + delta));
                // 使用 ExecuteScript 来设置页面缩放
                string script = $"document.body.style.zoom = '{_zoomLevel / 100.0}'";
                _webView2.CoreWebView2.ExecuteScriptAsync(script);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"改变缩放失败: {ex.Message}");
            }
        }

        #endregion

        #region WebView2 事件处理

        /// <summary>
        /// 导航开始事件
        /// </summary>
        private void CoreWebView2_NavigationStarting(object sender, CoreWebView2NavigationStartingEventArgs e)
        {
            try
            {
                // 更新导航按钮状态
                UpdateNavigationButtons();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"导航开始事件处理失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 导航完成事件
        /// </summary>
        private void CoreWebView2_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            try
            {
                // 更新导航按钮状态
                UpdateNavigationButtons();
                
                // 执行代码高亮
                _webView2.ExecuteScriptAsync("highlightCode();");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"导航完成事件处理失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 新窗口请求事件
        /// </summary>
        private void CoreWebView2_NewWindowRequested(object sender, CoreWebView2NewWindowRequestedEventArgs e)
        {
            try
            {
                // 在默认浏览器中打开链接
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = e.Uri,
                    UseShellExecute = true
                });
                
                // 取消在 WebView2 中打开
                e.Handled = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"新窗口请求处理失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 更新导航按钮状态
        /// </summary>
        private void UpdateNavigationButtons()
        {
            try
            {
                if (_webViewInitialized && _webView2.CoreWebView2 != null)
                {
                    _btnBack.Enabled = _webView2.CoreWebView2.CanGoBack;
                    _btnForward.Enabled = _webView2.CoreWebView2.CanGoForward;
                }
            }
            catch
            {
                _btnBack.Enabled = false;
                _btnForward.Enabled = false;
            }
        }

        #endregion

        #region 事件处理

        /// <summary>
        /// 打印按钮点击事件
        /// </summary>
        private async System.Threading.Tasks.Task BtnPrint_ClickAsync()
        {
            try
            {
                if (_webViewInitialized)
                {
                    await _webView2.CoreWebView2.ExecuteScriptAsync("window.print();");
                }
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
                BtnPrint_ClickAsync();
                return true;
            }

            if (keyData == Keys.F5)
            {
                RefreshContentAsync();
                return true;
            }

            if (keyData == (Keys.Control | Keys.Add) || keyData == (Keys.Control | Keys.Oemplus))
            {
                ChangeZoom(10);
                return true;
            }

            if (keyData == (Keys.Control | Keys.Subtract) || keyData == (Keys.Control | Keys.OemMinus))
            {
                ChangeZoom(-10);
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
            try
            {
                if (_webViewInitialized && _webView2.CoreWebView2 != null)
                {
                    _webView2.CoreWebView2.Stop();
                }
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
                    _btnBack?.Dispose();
                    _btnForward?.Dispose();
                    _btnRefresh?.Dispose();
                    _btnZoomOut?.Dispose();
                    _btnZoomIn?.Dispose();
                    _btnPrint?.Dispose();
                    _btnOpenCHM?.Dispose();
                    
                    if (_webViewInitialized)
                    {
                        _webView2?.Dispose();
                    }
                    
                    _markdownRenderer?.Dispose();
                }

                _disposed = true;
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
