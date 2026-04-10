using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RUINORERP.UI.HelpSystem.Core;

namespace RUINORERP.UI.HelpSystem.Components
{
    /// <summary>
    /// 帮助面板窗体（增强版）
    /// 用于显示详细的HTML帮助内容
    /// 支持左侧导航树、搜索、目录等功能
    /// </summary>
    public class HelpPanel : Form
    {
        #region 私有字段

        private SplitContainer _splitContainer;
        private Panel _leftPanel;
        private Panel _rightPanel;
        private TreeView _treeView;
        private TextBox _searchBox;
        private ListBox _searchResults;
        private WebBrowser _webBrowser;
        private ToolStrip _toolStrip;
        private ToolStripButton _btnClose;
        private ToolStripButton _btnPrint;
        private ToolStripButton _btnBack;
        private ToolStripButton _btnForward;
        private ToolStripButton _btnHome;
        private ToolStripTextBox _searchInput;
        private ToolStripButton _btnSearch;

        private HelpContext _helpContext;
        private List<TreeNode> _allNodes = new List<TreeNode>();
        private string _currentContent = string.Empty;
        private bool _disposed = false;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="content">帮助内容(HTML格式)</param>
        /// <param name="context">帮助上下文</param>
        public HelpPanel(string content, HelpContext context)
        {
            _helpContext = context;
            InitializeComponent();
            LoadHelpContent(content, context);
            BuildNavigationTree();
        }

        #endregion

        #region 初始化方法

        /// <summary>
        /// 初始化组件
        /// </summary>
        private void InitializeComponent()
        {
            this.Text = "帮助 - RUINOR ERP";
            this.Size = new Size(1100, 750);
            this.StartPosition = FormStartPosition.CenterParent;
            this.MinimumSize = new Size(800, 500);
            this.KeyPreview = true;

            // 创建分割容器
            _splitContainer = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Horizontal,
                IsSplitterFixed = false,
                BorderStyle = BorderStyle.FixedSingle
            };

            // 左侧面板（导航）
            _leftPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(5)
            };

            // 右侧面板（内容）
            _rightPanel = new Panel
            {
                Dock = DockStyle.Fill
            };

            // 创建搜索框
            _searchBox = new TextBox
            {
                Dock = DockStyle.Top,
                Height = 30,
                Text = "搜索帮助...（按回车搜索）",
                Font = new Font("Segoe UI", 10F),
                ForeColor = SystemColors.GrayText
            };
            _searchBox.Enter += (s, e) =>
            {
                if (_searchBox.Text == "搜索帮助...（按回车搜索）")
                {
                    _searchBox.Text = "";
                    _searchBox.ForeColor = SystemColors.WindowText;
                }
            };
            _searchBox.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(_searchBox.Text))
                {
                    _searchBox.Text = "搜索帮助...（按回车搜索）";
                    _searchBox.ForeColor = SystemColors.GrayText;
                }
            };
            _searchBox.KeyPress += SearchBox_KeyPress;

            // 创建搜索结果列表
            _searchResults = new ListBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9F),
                IntegralHeight = false,
                Visible = false
            };
            _searchResults.DoubleClick += SearchResults_DoubleClick;

            // 创建树形导航
            _treeView = new TreeView
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9F),
                CheckBoxes = false,
                FullRowSelect = true,
                ShowLines = true,
                HideSelection = false
            };
            _treeView.AfterSelect += TreeView_AfterSelect;
            _treeView.NodeMouseDoubleClick += TreeView_NodeMouseDoubleClick;

            // 创建工具栏
            CreateToolStrip();

            // 创建Web浏览器
            _webBrowser = new WebBrowser
            {
                Dock = DockStyle.Fill,
                WebBrowserShortcutsEnabled = true,
                IsWebBrowserContextMenuEnabled = true,
                AllowWebBrowserDrop = false,
                ScriptErrorsSuppressed = false
            };
            _webBrowser.Navigating += WebBrowser_Navigating;

            // 组装左侧面板
            _leftPanel.Controls.Add(_treeView);
            _leftPanel.Controls.Add(_searchResults);
            _leftPanel.Controls.Add(_searchBox);

            // 组装右侧面板
            _rightPanel.Controls.Add(_toolStrip);
            _rightPanel.Controls.Add(_webBrowser);

            // 设置分割容器
            _splitContainer.Panel1.Controls.Add(_leftPanel);
            _splitContainer.Panel2.Controls.Add(_rightPanel);
            _splitContainer.SplitterDistance = 280;

            // 添加到窗体
            this.Controls.Add(_splitContainer);
        }

        /// <summary>
        /// 创建工具栏
        /// </summary>
        private void CreateToolStrip()
        {
            _toolStrip = new ToolStrip
            {
                Dock = DockStyle.Top,
                GripStyle = ToolStripGripStyle.Hidden,
                RenderMode = ToolStripRenderMode.System,
                ImageScalingSize = new Size(20, 20),
                Padding = new Padding(5)
            };

            // 后退按钮
            _btnBack = new ToolStripButton("◀");
            _btnBack.ToolTipText = "后退";
            _btnBack.Click += (s, e) => { if (_webBrowser.CanGoBack) _webBrowser.GoBack(); };
            _btnBack.Enabled = false;

            // 前进按钮
            _btnForward = new ToolStripButton("▶");
            _btnForward.ToolTipText = "前进";
            _btnForward.Click += (s, e) => { if (_webBrowser.CanGoForward) _webBrowser.GoForward(); };
            _btnForward.Enabled = false;

            // 主页按钮
            _btnHome = new ToolStripButton("🏠");
            _btnHome.ToolTipText = "帮助首页";
            _btnHome.Click += BtnHome_Click;

            // 打印按钮
            _btnPrint = new ToolStripButton("🖨");
            _btnPrint.ToolTipText = "打印";
            _btnPrint.Click += BtnPrint_Click;

            // 关闭按钮
            _btnClose = new ToolStripButton("✕");
            _btnClose.ToolTipText = "关闭";
            _btnClose.Click += (s, e) => this.Close();

            // 搜索输入框
            _searchInput = new ToolStripTextBox
            {
                Width = 200,
                ToolTipText = "输入关键词搜索"
            };
            _searchInput.KeyPress += SearchInput_KeyPress;

            // 搜索按钮
            _btnSearch = new ToolStripButton("🔍");
            _btnSearch.ToolTipText = "搜索";
            _btnSearch.Click += BtnSearch_Click;

            // 添加到工具栏
            _toolStrip.Items.AddRange(new ToolStripItem[]
            {
                _btnBack,
                _btnForward,
                _btnHome,
                new ToolStripSeparator(),
                _btnPrint,
                new ToolStripSeparator(),
                _searchInput,
                _btnSearch,
                new ToolStripSeparator(),
                _btnClose
            });
        }

        #endregion

        #region 导航树构建

        /// <summary>
        /// 构建导航树
        /// </summary>
        private void BuildNavigationTree()
        {
            _treeView.Nodes.Clear();
            _allNodes.Clear();

            try
            {
                // 获取帮助内容路径
                string helpContentPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "HelpContent");
                if (!Directory.Exists(helpContentPath))
                {
                    return;
                }

                // 模块级节点
                TreeNode modulesNode = new TreeNode("📚 模块帮助", 0, 0);
                modulesNode.Name = "Modules";

                // 窗体级节点
                TreeNode formsNode = new TreeNode("📝 窗体帮助", 1, 1);
                formsNode.Name = "Forms";

                // 字段级节点
                TreeNode fieldsNode = new TreeNode("🔤 字段帮助", 2, 2);
                fieldsNode.Name = "Fields";

                // 添加目录节点
                _treeView.Nodes.Add(modulesNode);
                _treeView.Nodes.Add(formsNode);
                _treeView.Nodes.Add(fieldsNode);

                // 遍历目录
                string[] categories = { "Modules", "Forms", "Fields" };
                TreeNode[] categoryNodes = { modulesNode, formsNode, fieldsNode };

                for (int i = 0; i < categories.Length; i++)
                {
                    string categoryPath = Path.Combine(helpContentPath, categories[i]);
                    if (Directory.Exists(categoryPath))
                    {
                        BuildCategoryNodes(categoryNodes[i], categoryPath, categories[i]);
                    }
                }

                // 展开第一级
                foreach (TreeNode node in _treeView.Nodes)
                {
                    node.Expand();
                }

                // 保存所有节点用于搜索
                SaveAllNodes(_treeView.Nodes);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"构建导航树失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 递归构建分类节点
        /// </summary>
        private void BuildCategoryNodes(TreeNode parentNode, string path, string category)
        {
            try
            {
                // 获取子目录
                foreach (var dir in Directory.GetDirectories(path))
                {
                    string dirName = Path.GetFileName(dir);
                    TreeNode dirNode = new TreeNode(dirName, 1, 1);
                    dirNode.Tag = Path.Combine(category, dirName);
                    dirNode.Name = Path.Combine(category, dirName);

                    // 递归添加子目录
                    BuildCategoryNodes(dirNode, dir, Path.Combine(category, dirName));

                    // 添加该目录下的文件
                    foreach (var file in Directory.GetFiles(dir, "*.md"))
                    {
                        string fileName = Path.GetFileNameWithoutExtension(file);
                        TreeNode fileNode = new TreeNode(fileName, 3, 3);
                        fileNode.Tag = file;
                        fileNode.Name = fileName;

                        // 保存帮助键
                        string helpKey = $"{category}.{dirName}.{fileName}";
                        fileNode.ToolTipText = helpKey;

                        dirNode.Nodes.Add(fileNode);
                    }

                    if (dirNode.Nodes.Count > 0)
                    {
                        parentNode.Nodes.Add(dirNode);
                    }
                }

                // 添加该目录下的文件（无子目录）
                foreach (var file in Directory.GetFiles(path, "*.md"))
                {
                    string fileName = Path.GetFileNameWithoutExtension(file);
                    TreeNode fileNode = new TreeNode(fileName, 3, 3);
                    fileNode.Tag = file;
                    fileNode.Name = fileName;

                    string helpKey = $"{category}.{fileName}";
                    fileNode.ToolTipText = helpKey;

                    parentNode.Nodes.Add(fileNode);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"构建分类节点失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 保存所有节点用于搜索
        /// </summary>
        private void SaveAllNodes(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                _allNodes.Add(node);
                if (node.Nodes.Count > 0)
                {
                    SaveAllNodes(node.Nodes);
                }
            }
        }

        #endregion

        #region 搜索功能

        /// <summary>
        /// 搜索框按键事件
        /// </summary>
        private void SearchBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                PerformSearch(_searchBox.Text);
            }
        }

        /// <summary>
        /// 工具栏搜索输入按键事件
        /// </summary>
        private void SearchInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                PerformSearch(_searchInput.Text);
            }
        }

        /// <summary>
        /// 搜索按钮点击事件
        /// </summary>
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            PerformSearch(_searchInput.Text);
        }

        /// <summary>
        /// 执行搜索
        /// </summary>
        private void PerformSearch(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                _treeView.Visible = true;
                _searchResults.Visible = false;
                return;
            }

            _searchResults.Items.Clear();
            _treeView.Visible = false;
            _searchResults.Visible = true;

            try
            {
                string helpContentPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "HelpContent");
                if (!Directory.Exists(helpContentPath))
                {
                    return;
                }

                // 搜索文件
                var files = Directory.GetFiles(helpContentPath, "*.md", SearchOption.AllDirectories);
                foreach (var file in files)
                {
                    string fileName = Path.GetFileNameWithoutExtension(file);
                    string relativePath = file.Substring(helpContentPath.Length).Replace("\\", "/");

                    // 检查文件名是否包含关键词
                    if (fileName.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        _searchResults.Items.Add(new SearchResultItem
                        {
                            Text = fileName,
                            FilePath = file,
                            HelpKey = relativePath.Replace("/", ".").Replace(".md", "")
                        });
                        continue;
                    }

                    // 检查文件内容
                    try
                    {
                        string content = File.ReadAllText(file);
                        if (content.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            _searchResults.Items.Add(new SearchResultItem
                            {
                                Text = fileName + " (相关内容)",
                                FilePath = file,
                                HelpKey = relativePath.Replace("/", ".").Replace(".md", "")
                            });
                        }
                    }
                    catch { }
                }

                if (_searchResults.Items.Count == 0)
                {
                    _searchResults.Items.Add($"未找到包含 \"{keyword}\" 的帮助内容");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"搜索失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 搜索结果双击事件
        /// </summary>
        private void SearchResults_DoubleClick(object sender, EventArgs e)
        {
            if (_searchResults.SelectedItem is SearchResultItem item)
            {
                LoadHelpFile(item.FilePath);
            }
        }

        #endregion

        #region 树形导航事件

        /// <summary>
        /// 树节点选中事件
        /// </summary>
        private void TreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node?.Tag is string filePath && File.Exists(filePath))
            {
                LoadHelpFile(filePath);
            }
        }

        /// <summary>
        /// 树节点双击事件
        /// </summary>
        private void TreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node?.Tag is string filePath && File.Exists(filePath))
            {
                LoadHelpFile(filePath);
            }
        }

        #endregion

        #region WebBrowser事件

        /// <summary>
        /// WebBrowser导航事件（处理内部链接）
        /// </summary>
        private void WebBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            // 更新前进/后退按钮状态
            _btnBack.Enabled = _webBrowser.CanGoBack;
            _btnForward.Enabled = _webBrowser.CanGoForward;
        }

        #endregion

        #region 工具栏按钮事件

        /// <summary>
        /// 主页按钮点击
        /// </summary>
        private void BtnHome_Click(object sender, EventArgs e)
        {
            string indexPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "HelpContent", "index.md");
            if (File.Exists(indexPath))
            {
                LoadHelpFile(indexPath);
            }
        }

        /// <summary>
        /// 打印按钮点击
        /// </summary>
        private void BtnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                _webBrowser.ShowPrintDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"打印失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 加载帮助内容
        /// </summary>
        public void LoadHelpContent(string content, HelpContext context)
        {
            _currentContent = content;
            _helpContext = context;

            if (!string.IsNullOrEmpty(content))
            {
                _webBrowser.DocumentText = content;
            }
        }

        /// <summary>
        /// 加载帮助文件
        /// </summary>
        public void LoadHelpFile(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    return;
                }

                string content = File.ReadAllText(filePath, Encoding.UTF8);

                // 如果是Markdown，转换为HTML
                if (filePath.EndsWith(".md", StringComparison.OrdinalIgnoreCase))
                {
                    content = MarkdownToHtmlWithStyle(content);
                }

                _currentContent = content;
                _webBrowser.DocumentText = content;

                // 显示树，隐藏搜索结果
                _treeView.Visible = true;
                _searchResults.Visible = false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"加载帮助文件失败: {ex.Message}");
            }
        }

        /// <summary>
        /// Markdown转HTML（带样式）
        /// </summary>
        private string MarkdownToHtmlWithStyle(string markdown)
        {
            if (string.IsNullOrEmpty(markdown)) return string.Empty;

            try
            {
                // 标题
                markdown = System.Text.RegularExpressions.Regex.Replace(markdown, @"^#\s+(.+)$", "<h1>$1</h1>", System.Text.RegularExpressions.RegexOptions.Multiline);
                markdown = System.Text.RegularExpressions.Regex.Replace(markdown, @"^##\s+(.+)$", "<h2>$1</h2>", System.Text.RegularExpressions.RegexOptions.Multiline);
                markdown = System.Text.RegularExpressions.Regex.Replace(markdown, @"^###\s+(.+)$", "<h3>$1</h3>", System.Text.RegularExpressions.RegexOptions.Multiline);

                // 粗体斜体
                markdown = System.Text.RegularExpressions.Regex.Replace(markdown, @"\*\*(.+?)\*\*", "<strong>$1</strong>");
                markdown = System.Text.RegularExpressions.Regex.Replace(markdown, @"\*(.+?)\*", "<em>$1</em>");

                // 代码
                markdown = System.Text.RegularExpressions.Regex.Replace(markdown, @"`([^`]+)`", "<code>$1</code>");

                // 列表
                markdown = System.Text.RegularExpressions.Regex.Replace(markdown, @"^\s*-\s+(.+)$", "<li>$1</li>", System.Text.RegularExpressions.RegexOptions.Multiline);
                markdown = System.Text.RegularExpressions.Regex.Replace(markdown, @"(<li>.*</li>\s*)+", "<ul>$&</ul>");

                // 段落
                markdown = System.Text.RegularExpressions.Regex.Replace(markdown, @"^(?!<[hul])\s*(.+)$", "<p>$1</p>", System.Text.RegularExpressions.RegexOptions.Multiline);

                return WrapHtmlWithStyle(markdown);
            }
            catch
            {
                return markdown;
            }
        }

        /// <summary>
        /// HTML样式包装
        /// </summary>
        private string WrapHtmlWithStyle(string bodyContent)
        {
            return $@"<!DOCTYPE html>
<html>
<head>
    <meta charset=""UTF-8"">
    <style>
        body {{
            font-family: 'Segoe UI', 'Microsoft YaHei', Arial, sans-serif;
            font-size: 14px;
            line-height: 1.6;
            color: #333;
            background-color: #f5f5f5;
            padding: 20px;
            margin: 0;
        }}
        .help-container {{
            max-width: 100%;
            margin: 0 auto;
            background: white;
            padding: 25px;
            border-radius: 8px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
        }}
        h1 {{ color: #1a73e8; border-bottom: 2px solid #1a73e8; padding-bottom: 10px; }}
        h2 {{ color: #202124; margin-top: 25px; border-left: 4px solid #1a73e8; padding-left: 10px; }}
        h3 {{ color: #5f6368; }}
        table {{ width: 100%; border-collapse: collapse; margin: 15px 0; }}
        th, td {{ border: 1px solid #ddd; padding: 10px; text-align: left; }}
        th {{ background-color: #1a73e8; color: white; }}
        code {{ background: #f5f5f5; padding: 2px 6px; border-radius: 4px; color: #d63384; }}
        li {{ margin: 5px 0; }}
        a {{ color: #1a73e8; }}
    </style>
</head>
<body><div class=""help-container"">{bodyContent}</div></body>
</html>";
        }

        #endregion

        #region 辅助类

        /// <summary>
        /// 搜索结果项
        /// </summary>
        private class SearchResultItem
        {
            public string Text { get; set; }
            public string FilePath { get; set; }
            public string HelpKey { get; set; }

            public override string ToString() => Text;
        }

        #endregion
    }
}
