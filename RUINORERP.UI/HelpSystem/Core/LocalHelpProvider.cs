using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace RUINORERP.UI.HelpSystem.Core
{
    /// <summary>
    /// 本地帮助提供者实现类
    /// 从本地文件系统加载和管理帮助内容
    /// 支持HTML和Markdown格式的帮助文件
    /// 支持从嵌入资源加载帮助内容（发布环境）
    /// 提供帮助索引、搜索和内容获取功能
    /// </summary>
    public class LocalHelpProvider : IHelpProvider
    {
        #region 私有字段

        /// <summary>
        /// 帮助内容根路径
        /// </summary>
        private readonly string _helpContentPath;

        /// <summary>
        /// 帮助索引字典
        /// 键: 帮助键, 值: 帮助文件完整路径或资源名称
        /// </summary>
        private readonly Dictionary<string, string> _helpIndex;

        /// <summary>
        /// 资源索引字典
        /// 键: 帮助键, 值: 嵌入式资源名称
        /// </summary>
        private readonly Dictionary<string, string> _resourceIndex;

        /// <summary>
        /// 资源名称前缀
        /// </summary>
        private readonly string _resourcePrefix;

        /// <summary>
        /// 程序集引用
        /// </summary>
        private readonly System.Reflection.Assembly _assembly;

        /// <summary>
        /// 索引锁对象,确保线程安全
        /// </summary>
        private readonly object _indexLock = new object();

        /// <summary>
        /// 已释放标志
        /// </summary>
        private bool _disposed = false;

        #endregion

        #region 公共属性 - 实现IHelpProvider接口

        /// <summary>
        /// 获取帮助提供者名称
        /// </summary>
        public string ProviderName => "本地帮助提供者";

        /// <summary>
        /// 获取帮助内容根路径
        /// </summary>
        public string HelpContentRootPath => _helpContentPath;

        /// <summary>
        /// 获取帮助总数
        /// </summary>
        public int HelpCount
        {
            get
            {
                lock (_indexLock)
                {
                    return _helpIndex.Count;
                }
            }
        }

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="helpContentPath">帮助内容根路径</param>
        /// <exception cref="ArgumentNullException">帮助路径为null时抛出</exception>
        public LocalHelpProvider(string helpContentPath)
        {
            _helpContentPath = helpContentPath ??
                throw new ArgumentNullException(nameof(helpContentPath));
            _helpIndex = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            _resourceIndex = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            _assembly = System.Reflection.Assembly.GetExecutingAssembly();
            _resourcePrefix = $"{_assembly.GetName().Name}.HelpContent";

            // 初始化帮助索引（优先文件系统，再资源）
            InitializeIndex();

            // 初始化资源索引
            InitializeResourceIndex();
        }

        #endregion

        #region 私有方法 - 索引初始化

        /// <summary>
        /// 初始化帮助索引
        /// 遍历所有帮助文件,建立帮助键到文件路径的映射
        /// </summary>
        private void InitializeIndex()
        {
            try
            {
                lock (_indexLock)
                {
                    _helpIndex.Clear();
                }

                // 检查帮助目录是否存在
                if (!Directory.Exists(_helpContentPath))
                {
                    // 如果目录不存在,创建它
                    Directory.CreateDirectory(_helpContentPath);
                    return;
                }

                // 遍历所有帮助文件并建立索引
                IndexDirectory(_helpContentPath);
            }
            catch (Exception ex)
            {
                // 记录日志
                System.Diagnostics.Debug.WriteLine($"初始化帮助索引失败: {ex.Message}");

                // 尝试记录到主窗体日志
                try
                {
                    System.Diagnostics.Debug.WriteLine($"初始化帮助索引失败: {ex.Message}");
                }
                catch
                {
                    // 忽略日志记录错误
                }
            }
        }

        /// <summary>
        /// 递归索引目录
        /// </summary>
        /// <param name="directory">要索引的目录</param>
        private void IndexDirectory(string directory)
        {
            try
            {
                // 获取所有HTML和Markdown文件
                var files = Directory.GetFiles(directory, "*.html", SearchOption.AllDirectories)
                    .Concat(Directory.GetFiles(directory, "*.md", SearchOption.AllDirectories));

                foreach (var file in files)
                {
                    try
                    {
                        // 计算相对路径作为帮助键
                        string relativePath = file.Substring(_helpContentPath.Length)
                            .TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                            .Replace('\\', '/')
                            .Replace(".html", "")
                            .Replace(".md", "");

                        // 将路径转换为帮助键格式
                        // 示例: "Forms/UCSaleOrder" -> "Forms.UCSaleOrder"
                        string helpKey = relativePath.Replace('/', '.');

                        lock (_indexLock)
                        {
                            // 如果键已存在,使用较新的文件
                            if (_helpIndex.ContainsKey(helpKey))
                            {
                                var existingFile = _helpIndex[helpKey];
                                if (File.GetLastWriteTime(file) > File.GetLastWriteTime(existingFile))
                                {
                                    _helpIndex[helpKey] = file;
                                }
                            }
                            else
                            {
                                _helpIndex[helpKey] = file;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"索引文件 {file} 失败: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"索引目录 {directory} 失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 移除字符串后缀
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="suffix">要移除的后缀</param>
        /// <returns>移除后缀后的字符串</returns>
        private static string RemoveSuffix(string input, string suffix)
        {
            if (input.EndsWith(suffix, StringComparison.OrdinalIgnoreCase))
            {
                return input.Substring(0, input.Length - suffix.Length);
            }
            return input;
        }

        /// <summary>
        /// 初始化资源索引
        /// 扫描所有嵌入的帮助资源
        /// </summary>
        private void InitializeResourceIndex()
        {
            try
            {
                // 扫描所有嵌入资源
                var resourceNames = _assembly.GetManifestResourceNames()
                    .Where(name => name.StartsWith(_resourcePrefix));

                foreach (var name in resourceNames)
                {
                    try
                    {
                        // 将资源路径转换为帮助键
                        // RUINORERP.UI.HelpContent.Forms.UCSaleOrder.md -> Forms.UCSaleOrder
                        string helpKey = name.Substring(_resourcePrefix.Length + 1)
                            .Replace('.', '/');
                        helpKey = RemoveSuffix(helpKey, ".md");
                        helpKey = RemoveSuffix(helpKey, ".html");
                        helpKey = helpKey.Replace('/', '.');

                        lock (_indexLock)
                        {
                            // 只有文件系统没有该帮助键时，才添加资源索引
                            if (!_helpIndex.ContainsKey(helpKey))
                            {
                                _resourceIndex[helpKey] = name;
                                _helpIndex[helpKey] = $"resource:{name}";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"索引资源 {name} 失败: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"初始化资源索引失败: {ex.Message}");
            }
        }

        #endregion

        #region 公共方法 - 实现IHelpProvider接口

        /// <summary>
        /// 获取帮助内容
        /// </summary>
        /// <param name="context">帮助上下文</param>
        /// <returns>帮助内容,未找到则返回null</returns>
        public string GetHelpContent(HelpContext context)
        {
            if (context == null || _disposed)
            {
                return null;
            }

            // 生成帮助键
            string helpKey = GenerateHelpKey(context);

            // 查找帮助文件
            string filePath = null;
            lock (_indexLock)
            {
                _helpIndex.TryGetValue(helpKey, out filePath);
            }

            if (string.IsNullOrEmpty(filePath))
            {
                return null;
            }

            try
            {
                string content = null;

                // 判断是从文件系统加载还是从嵌入资源加载
                if (filePath.StartsWith("resource:", StringComparison.OrdinalIgnoreCase))
                {
                    // 从嵌入资源加载
                    string resourceName = filePath.Substring("resource:".Length);
                    content = LoadFromEmbeddedResource(resourceName);
                }
                else if (File.Exists(filePath))
                {
                    // 从文件系统加载
                    content = File.ReadAllText(filePath, Encoding.UTF8);

                    // 如果是Markdown格式,转换为HTML(简单处理)
                    if (filePath.EndsWith(".md", StringComparison.OrdinalIgnoreCase))
                    {
                        content = MarkdownToHtml(content);
                    }
                }

                return content;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"读取帮助文件 {filePath} 失败: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 从嵌入资源加载帮助内容
        /// </summary>
        /// <param name="resourceName">资源名称</param>
        /// <returns>帮助内容,失败则返回null</returns>
        private string LoadFromEmbeddedResource(string resourceName)
        {
            try
            {
                using (var stream = _assembly.GetManifestResourceStream(resourceName))
                {
                    if (stream == null) return null;

                    using (var reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        string content = reader.ReadToEnd();

                        // 如果是Markdown格式,转换为HTML
                        if (resourceName.EndsWith(".md", StringComparison.OrdinalIgnoreCase))
                        {
                            content = MarkdownToHtml(content);
                        }

                        return content;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"从资源加载帮助失败: {resourceName}, {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 搜索帮助内容
        /// </summary>
        /// <param name="keyword">搜索关键词</param>
        /// <param name="context">当前帮助上下文(用于优先排序)</param>
        /// <returns>搜索结果列表</returns>
        public List<HelpSearchResult> Search(string keyword, HelpContext context = null)
        {
            var results = new List<HelpSearchResult>();

            if (string.IsNullOrEmpty(keyword) || _disposed)
            {
                return results;
            }

            try
            {
                // 获取所有帮助文件路径
                Dictionary<string, string> indexSnapshot;
                lock (_indexLock)
                {
                    indexSnapshot = new Dictionary<string, string>(_helpIndex);
                }

                // 遍历所有帮助文件进行搜索
                foreach (var kvp in indexSnapshot)
                {
                    try
                    {
                        string content = null;

                        // 判断是从文件系统加载还是从嵌入资源加载
                        if (kvp.Value.StartsWith("resource:", StringComparison.OrdinalIgnoreCase))
                        {
                            // 从嵌入资源加载
                            string resourceName = kvp.Value.Substring("resource:".Length);
                            content = LoadFromEmbeddedResource(resourceName);
                        }
                        else if (File.Exists(kvp.Value))
                        {
                            // 从文件系统加载
                            content = File.ReadAllText(kvp.Value, Encoding.UTF8);
                            if (kvp.Value.EndsWith(".md", StringComparison.OrdinalIgnoreCase))
                            {
                                content = MarkdownToHtml(content);
                            }
                        }

                        if (string.IsNullOrEmpty(content))
                        {
                            continue;
                        }

                        // 计算相关度分数
                        double relevanceScore = CalculateRelevance(keyword, kvp.Key, content);

                        // 如果相关度大于0,添加到结果列表
                        if (relevanceScore > 0)
                        {
                            results.Add(new HelpSearchResult
                            {
                                HelpKey = kvp.Key,
                                FilePath = kvp.Value,
                                Title = ExtractTitle(content),
                                Summary = ExtractSummary(content),
                                Content = content,
                                RelevanceScore = relevanceScore,
                                Level = DetermineLevel(kvp.Key),
                                LastModified = File.GetLastWriteTime(kvp.Value)
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"搜索文件 {kvp.Value} 失败: {ex.Message}");
                    }
                }

                // 按相关度从高到低排序
                return results.OrderByDescending(r => r.RelevanceScore).ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"搜索帮助失败: {ex.Message}");
                return results;
            }
        }

        /// <summary>
        /// 检查帮助是否存在
        /// </summary>
        /// <param name="context">帮助上下文</param>
        /// <returns>帮助存在返回true,否则返回false</returns>
        public bool HelpExists(HelpContext context)
        {
            if (context == null || _disposed)
            {
                return false;
            }

            string helpKey = GenerateHelpKey(context);

            lock (_indexLock)
            {
                if (!_helpIndex.TryGetValue(helpKey, out string filePath))
                {
                    return false;
                }

                return File.Exists(filePath);
            }
        }

        /// <summary>
        /// 重新加载帮助索引
        /// </summary>
        public void ReloadIndex()
        {
            InitializeIndex();
        }

        #endregion

        #region 私有方法 - 帮助键生成

        /// <summary>
        /// 根据帮助上下文生成帮助键
        /// </summary>
        /// <param name="context">帮助上下文</param>
        /// <returns>帮助键</returns>
        private string GenerateHelpKey(HelpContext context)
        {
            // 如果手动指定了帮助键,直接使用
            if (!string.IsNullOrEmpty(context.HelpKey))
            {
                return context.HelpKey;
            }

            // 根据帮助级别生成帮助键
            switch (context.Level)
            {
                case HelpLevel.Field:
                    return $"Fields.{context.EntityType?.Name}.{context.FieldName}";

                case HelpLevel.Control:
                    return $"Controls.{context.FormType?.Name}.{context.ControlName}";

                case HelpLevel.Form:
                    return $"Forms.{context.FormType?.Name}";

                case HelpLevel.Module:
                    return $"Modules.{context.ModuleName}";

                default:
                    return context.HelpKey ?? string.Empty;
            }
        }

        #endregion

        #region 私有方法 - 相关度计算

        /// <summary>
        /// 计算搜索相关度分数
        /// </summary>
        /// <param name="keyword">搜索关键词</param>
        /// <param name="helpKey">帮助键</param>
        /// <param name="content">帮助内容</param>
        /// <returns>相关度分数</returns>
        private double CalculateRelevance(string keyword, string helpKey, string content)
        {
            double score = 0;

            try
            {
                // 关键词在帮助键中的匹配(权重: 10)
                if (helpKey.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    score += 10;
                }

                // 关键词在内容中的出现次数(权重: 每次出现+2)
                int occurrences = CountOccurrences(content, keyword);
                score += occurrences * 2;

                // 标题匹配(权重: 5)
                string title = ExtractTitle(content);
                if (!string.IsNullOrEmpty(title) &&
                    title.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    score += 5;
                }

                // 如果是精确匹配(整个帮助键等于关键词),额外加分(权重: 15)
                if (helpKey.Equals(keyword, StringComparison.OrdinalIgnoreCase))
                {
                    score += 15;
                }
            }
            catch
            {
                // 计算失败,返回0
            }

            return score;
        }

        /// <summary>
        /// 统计关键词在文本中的出现次数
        /// </summary>
        /// <param name="text">要搜索的文本</param>
        /// <param name="keyword">关键词</param>
        /// <returns>出现次数</returns>
        private int CountOccurrences(string text, string keyword)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(keyword))
            {
                return 0;
            }

            // 使用Split方法统计出现次数
            return text.Split(new[] { keyword }, StringSplitOptions.None).Length - 1;
        }

        #endregion

        #region 私有方法 - 内容提取

        /// <summary>
        /// 从HTML内容中提取标题
        /// 提取第一个h1标签的内容
        /// </summary>
        /// <param name="html">HTML内容</param>
        /// <returns>标题,未找到则返回帮助键</returns>
        private string ExtractTitle(string html)
        {
            try
            {
                // 尝试提取h1标签
                var h1Match = Regex.Match(html, @"<h1[^>]*>(.*?)</h1>",
                    RegexOptions.IgnoreCase | RegexOptions.Singleline);

                if (h1Match.Success)
                {
                    // 移除HTML标签
                    return Regex.Replace(h1Match.Groups[1].Value, @"<[^>]+>", "").Trim();
                }

                // 尝试提取h2标签
                var h2Match = Regex.Match(html, @"<h2[^>]*>(.*?)</h2>",
                    RegexOptions.IgnoreCase | RegexOptions.Singleline);

                if (h2Match.Success)
                {
                    return Regex.Replace(h2Match.Groups[1].Value, @"<[^>]+>", "").Trim();
                }

                // 尝试提取title标签
                var titleMatch = Regex.Match(html, @"<title[^>]*>(.*?)</title>",
                    RegexOptions.IgnoreCase | RegexOptions.Singleline);

                if (titleMatch.Success)
                {
                    return titleMatch.Groups[1].Value.Trim();
                }

                // 尝试从Markdown提取标题
                var mdMatch = Regex.Match(html, @"^#\s+(.+)$", RegexOptions.Multiline);
                if (mdMatch.Success)
                {
                    return mdMatch.Groups[1].Value.Trim();
                }
            }
            catch
            {
                // 忽略异常
            }

            return "未命名";
        }

        /// <summary>
        /// 从内容中提取摘要
        /// 提取前200个字符作为摘要
        /// </summary>
        /// <param name="content">内容</param>
        /// <returns>摘要</returns>
        private string ExtractSummary(string content)
        {
            try
            {
                // 移除HTML标签
                string text = Regex.Replace(content, @"<[^>]+>", " ");
                // 移除多余空格和换行
                text = Regex.Replace(text, @"\s+", " ").Trim();
                // 截取前200个字符
                return text.Length > 200 ? text.Substring(0, 200) + "..." : text;
            }
            catch
            {
                return "摘要提取失败";
            }
        }

        /// <summary>
        /// 确定帮助级别
        /// </summary>
        /// <param name="helpKey">帮助键</param>
        /// <returns>帮助级别</returns>
        private HelpLevel DetermineLevel(string helpKey)
        {
            if (string.IsNullOrEmpty(helpKey))
            {
                return HelpLevel.Form; // 默认
            }

            // 根据帮助键前缀判断级别
            if (helpKey.StartsWith("Fields.", StringComparison.OrdinalIgnoreCase))
            {
                return HelpLevel.Field;
            }
            if (helpKey.StartsWith("Controls.", StringComparison.OrdinalIgnoreCase))
            {
                return HelpLevel.Control;
            }
            if (helpKey.StartsWith("Forms.", StringComparison.OrdinalIgnoreCase))
            {
                return HelpLevel.Form;
            }
            if (helpKey.StartsWith("Modules.", StringComparison.OrdinalIgnoreCase))
            {
                return HelpLevel.Module;
            }

            return HelpLevel.Form; // 默认
        }

        /// <summary>
        /// 简单的Markdown转HTML转换器
        /// </summary>
        /// <param name="markdown">Markdown文本</param>
        /// <returns>HTML文本</returns>
        private string MarkdownToHtml(string markdown)
        {
            if (string.IsNullOrEmpty(markdown))
            {
                return string.Empty;
            }

            try
            {
                // 简单的Markdown到HTML转换
                // 注意: 这是一个简化版本,仅支持基本的Markdown语法

                // 标题转换
                markdown = Regex.Replace(markdown, @"^#\s+(.+)$", "<h1>$1</h1>", RegexOptions.Multiline);
                markdown = Regex.Replace(markdown, @"^##\s+(.+)$", "<h2>$1</h2>", RegexOptions.Multiline);
                markdown = Regex.Replace(markdown, @"^###\s+(.+)$", "<h3>$1</h3>", RegexOptions.Multiline);

                // 粗体和斜体
                markdown = Regex.Replace(markdown, @"\*\*(.+?)\*\*", "<strong>$1</strong>");
                markdown = Regex.Replace(markdown, @"\*(.+?)\*", "<em>$1</em>");
                markdown = Regex.Replace(markdown, @"__(.+?)__", "<strong>$1</strong>");
                markdown = Regex.Replace(markdown, @"_(.+?)_", "<em>$1</em>");

                // 代码块
                markdown = Regex.Replace(markdown, @"```(\w+)?\n([\s\S]+?)```", "<pre><code>$2</code></pre>");
                markdown = Regex.Replace(markdown, @"`([^`]+)`", "<code>$1</code>");

                // 列表
                markdown = Regex.Replace(markdown, @"^\s*-\s+(.+)$", "<li>$1</li>", RegexOptions.Multiline);
                markdown = Regex.Replace(markdown, @"(<li>.*</li>\s*)+", "<ul>$&</ul>");

                // 链接
                markdown = Regex.Replace(markdown, @"\[([^\]]+)\]\(([^)]+)\)", "<a href=\"$2\">$1</a>");

                // 段落
                markdown = Regex.Replace(markdown, @"^(?!<[hup])\s*(.+)$", "<p>$1</p>", RegexOptions.Multiline);

                return markdown;
            }
            catch
            {
                return markdown; // 转换失败,返回原文本
            }
        }

        #endregion

        #region IDisposable 实现

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放资源的实际实现
        /// </summary>
        /// <param name="disposing">是否正在释放托管资源</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // 释放托管资源
                    lock (_indexLock)
                    {
                        _helpIndex.Clear();
                    }
                }

                _disposed = true;
            }
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~LocalHelpProvider()
        {
            Dispose(false);
        }

        #endregion
    }
}
