using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Markdig;

namespace RUINORERP.UI.HelpSystem.Core
{
    /// <summary>
    /// 本地缓存帮助提供者
    /// 当网络不可用时，从本地Markdown文件加载帮助内容
    /// 作为WebHelpProvider的备用方案
    /// </summary>
    public class LocalCacheHelpProvider : IHelpProvider
    {
        #region 私有字段

        private readonly string _cacheDirectory;
        private readonly Dictionary<string, string> _fileMapping;
        private readonly MarkdownPipeline _markdownPipeline;
        private bool _disposed = false;

        #endregion

        #region 公共属性

        public string ProviderName => "本地缓存帮助提供者";
        
        public string HelpContentRootPath => _cacheDirectory;
        
        public int HelpCount => _fileMapping.Count;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cacheDirectory">本地缓存目录，包含Markdown文件</param>
        public LocalCacheHelpProvider(string cacheDirectory)
        {
            _cacheDirectory = cacheDirectory ?? throw new ArgumentNullException(nameof(cacheDirectory));
            
            if (!Directory.Exists(_cacheDirectory))
            {
                Directory.CreateDirectory(_cacheDirectory);
            }

            // 初始化Markdown管道
            _markdownPipeline = new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .UseBootstrap()
                .Build();

            // 初始化文件映射
            _fileMapping = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            BuildFileIndex();
        }

        #endregion

        #region IHelpProvider 实现

        public string GetHelpContent(HelpContext context)
        {
            if (context == null || string.IsNullOrEmpty(context.HelpKey))
                return null;

            string filePath = GetFilePath(context.HelpKey);
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                return null;

            try
            {
                string markdown = File.ReadAllText(filePath);
                string html = ConvertMarkdownToHtml(markdown, context.HelpKey);
                return html;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"读取本地帮助失败: {ex.Message}");
                return null;
            }
        }

        public List<HelpSearchResult> Search(string keyword, HelpContext context = null)
        {
            var results = new List<HelpSearchResult>();

            foreach (var mapping in _fileMapping)
            {
                try
                {
                    if (mapping.Key.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        results.Add(new HelpSearchResult
                        {
                            Title = mapping.Key,
                            Content = mapping.Value,
                            HelpKey = mapping.Key,
                            RelevanceScore = CalculateRelevance(mapping.Key, keyword)
                        });
                        continue;
                    }

                    // 搜索文件内容
                    if (File.Exists(mapping.Value))
                    {
                        string content = File.ReadAllText(mapping.Value);
                        if (content.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            results.Add(new HelpSearchResult
                            {
                                Title = mapping.Key,
                                Content = mapping.Value,
                                HelpKey = mapping.Key,
                                RelevanceScore = 0.5
                            });
                        }
                    }
                }
                catch { /* 忽略读取错误 */ }
            }

            return results.OrderByDescending(r => r.RelevanceScore).ToList();
        }

        public bool HelpExists(HelpContext context)
        {
            if (context == null || string.IsNullOrEmpty(context.HelpKey))
                return false;

            string filePath = GetFilePath(context.HelpKey);
            return !string.IsNullOrEmpty(filePath) && File.Exists(filePath);
        }

        public void ReloadIndex()
        {
            _fileMapping.Clear();
            BuildFileIndex();
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _fileMapping.Clear();
                _disposed = true;
            }
        }

        #endregion

        #region 本地缓存专用方法

        /// <summary>
        /// 显示本地帮助
        /// </summary>
        public bool ShowLocalHelp(HelpContext context)
        {
            string html = GetHelpContent(context);
            if (string.IsNullOrEmpty(html))
                return false;

            try
            {
                // 创建帮助窗口
                var helpForm = new Form
                {
                    Text = "RUINOR ERP 帮助（本地缓存）",
                    Width = 1000,
                    Height = 700,
                    StartPosition = FormStartPosition.CenterScreen,
                    Icon = System.Drawing.SystemIcons.Information
                };

                // 使用WebBrowser控件显示HTML
                var browser = new WebBrowser
                {
                    Dock = DockStyle.Fill,
                    DocumentText = html
                };

                helpForm.Controls.Add(browser);
                helpForm.Show();

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"显示本地帮助失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 同步在线内容到本地缓存
        /// </summary>
        public void SyncFromOnline(string helpKey, string markdownContent)
        {
            try
            {
                string filePath = GetCacheFilePath(helpKey);
                string directory = Path.GetDirectoryName(filePath);
                
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                File.WriteAllText(filePath, markdownContent);
                
                // 更新索引
                if (!_fileMapping.ContainsKey(helpKey))
                {
                    _fileMapping[helpKey] = filePath;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"同步到本地缓存失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 获取缓存统计信息
        /// </summary>
        public string GetCacheStatistics()
        {
            int totalFiles = _fileMapping.Count;
            long totalSize = 0;

            foreach (var file in _fileMapping.Values)
            {
                try
                {
                    if (File.Exists(file))
                    {
                        var info = new FileInfo(file);
                        totalSize += info.Length;
                    }
                }
                catch { }
            }

            return $"缓存文件数: {totalFiles}, 总大小: {totalSize / 1024} KB";
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 构建文件索引
        /// </summary>
        private void BuildFileIndex()
        {
            if (!Directory.Exists(_cacheDirectory))
                return;

            // 遍历所有Markdown文件
            var mdFiles = Directory.GetFiles(_cacheDirectory, "*.md", SearchOption.AllDirectories);
            
            foreach (var file in mdFiles)
            {
                try
                {
                    // 从文件路径生成帮助键
                    string relativePath = file.Substring(_cacheDirectory.Length + 1);
                    string helpKey = ConvertPathToHelpKey(relativePath);
                    
                    if (!string.IsNullOrEmpty(helpKey))
                    {
                        _fileMapping[helpKey] = file;
                    }
                }
                catch { /* 忽略错误 */ }
            }
        }

        /// <summary>
        /// 将文件路径转换为帮助键
        /// </summary>
        private string ConvertPathToHelpKey(string relativePath)
        {
            // 移除扩展名
            string pathWithoutExt = relativePath.Replace(".md", "").Replace("\\", "/");
            
            // 根据路径结构生成帮助键
            if (pathWithoutExt.StartsWith("forms/"))
            {
                return pathWithoutExt.Substring(6); // 移除 "forms/"
            }
            else if (pathWithoutExt.StartsWith("modules/"))
            {
                return pathWithoutExt.Substring(8); // 移除 "modules/"
            }
            else if (pathWithoutExt.StartsWith("fields/"))
            {
                // Fields/tb_SaleOrder/CustomerVendor_ID -> Fields.tb_SaleOrder.CustomerVendor_ID
                return pathWithoutExt.Replace("/", ".");
            }
            else if (pathWithoutExt.StartsWith("quickstart/"))
            {
                // quickstart/login -> QuickStart.Login
                var parts = pathWithoutExt.Substring(11).Split('/');
                return "QuickStart." + string.Join(".", parts.Select(p => 
                    char.ToUpper(p[0]) + p.Substring(1)));
            }
            
            return pathWithoutExt;
        }

        /// <summary>
        /// 获取文件路径
        /// </summary>
        private string GetFilePath(string helpKey)
        {
            // 精确匹配
            if (_fileMapping.TryGetValue(helpKey, out string filePath))
            {
                return filePath;
            }

            // 尝试部分匹配
            foreach (var mapping in _fileMapping)
            {
                if (helpKey.StartsWith(mapping.Key, StringComparison.OrdinalIgnoreCase) ||
                    mapping.Key.StartsWith(helpKey, StringComparison.OrdinalIgnoreCase))
                {
                    return mapping.Value;
                }
            }

            // 根据帮助键构建文件路径
            return BuildFilePathFromHelpKey(helpKey);
        }

        /// <summary>
        /// 根据帮助键构建文件路径
        /// </summary>
        private string BuildFilePathFromHelpKey(string helpKey)
        {
            if (helpKey.StartsWith("Fields."))
            {
                // Fields.tb_SaleOrder.CustomerVendor_ID -> fields/tb_SaleOrder/CustomerVendor_ID.md
                return Path.Combine(_cacheDirectory, helpKey.Replace(".", "\\") + ".md");
            }
            else if (helpKey.StartsWith("Forms."))
            {
                // Forms.UCSaleOrder -> forms/UCSaleOrder.md
                return Path.Combine(_cacheDirectory, "forms", helpKey.Substring(6) + ".md");
            }
            else if (helpKey.StartsWith("QuickStart."))
            {
                // QuickStart.Login -> quickstart/login.md
                return Path.Combine(_cacheDirectory, "quickstart", 
                    helpKey.Substring(11).ToLower() + ".md");
            }
            else
            {
                // 默认映射到forms目录
                return Path.Combine(_cacheDirectory, "forms", helpKey + ".md");
            }
        }

        /// <summary>
        /// 获取缓存文件路径
        /// </summary>
        private string GetCacheFilePath(string helpKey)
        {
            return BuildFilePathFromHelpKey(helpKey);
        }

        /// <summary>
        /// 转换Markdown为HTML
        /// </summary>
        private string ConvertMarkdownToHtml(string markdown, string helpKey)
        {
            string htmlContent = Markdown.ToHtml(markdown, _markdownPipeline);
            
            // 包装成完整HTML文档
            return $@"<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <title>{helpKey} - RUINOR ERP帮助</title>
    <style>
        body {{ font-family: 'Microsoft YaHei', Arial, sans-serif; padding: 20px; line-height: 1.6; }}
        h1 {{ color: #333; border-bottom: 2px solid #0078d4; padding-bottom: 10px; }}
        h2 {{ color: #555; margin-top: 30px; }}
        h3 {{ color: #666; }}
        table {{ border-collapse: collapse; width: 100%; margin: 15px 0; }}
        th, td {{ border: 1px solid #ddd; padding: 8px; text-align: left; }}
        th {{ background-color: #f5f5f5; }}
        code {{ background-color: #f4f4f4; padding: 2px 6px; border-radius: 3px; font-family: Consolas, monospace; }}
        pre {{ background-color: #f8f9fa; border: 1px solid #e9ecef; border-radius: 4px; padding: 12px; overflow-x: auto; }}
        pre code {{ background-color: transparent; padding: 0; }}
        blockquote {{ border-left: 4px solid #0078d4; padding-left: 16px; margin: 16px 0; color: #6c757d; }}
        ul, ol {{ padding-left: 24px; }}
        li {{ margin: 8px 0; }}
        a {{ color: #0078d4; text-decoration: none; }}
        a:hover {{ text-decoration: underline; }}
        img {{ max-width: 100%; height: auto; }}
        .offline-notice {{ background-color: #fff3cd; border-left: 4px solid #ffc107; padding: 12px; margin-bottom: 20px; }}
    </style>
</head>
<body>
    <div class='offline-notice'>
        <strong>⚠️ 离线模式</strong> - 当前显示的是本地缓存内容，可能不是最新版本。<br>
        连接网络后可获取最新帮助文档。
    </div>
    {htmlContent}
</body>
</html>";
        }

        private double CalculateRelevance(string key, string keyword)
        {
            if (key.Equals(keyword, StringComparison.OrdinalIgnoreCase))
                return 1.0;
            
            if (key.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                return 0.8;
            
            return 0.5;
        }

        #endregion
    }
}
