using System;
using System.Text;
using System.Text.RegularExpressions;

namespace RUINORERP.UI.HelpSystem.Components
{
    /// <summary>
    /// Markdown 渲染器
    /// 将 Markdown 格式的文本转换为 HTML
    /// 支持常见 Markdown 语法：标题、列表、代码块、表格、链接、图片等
    /// </summary>
    public class MarkdownRenderer : IDisposable
    {
        #region 私有字段

        /// <summary>
        /// 已释放标志
        /// </summary>
        private bool _disposed = false;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public MarkdownRenderer()
        {
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 将 Markdown 文本转换为 HTML
        /// </summary>
        /// <param name="markdown">Markdown 文本</param>
        /// <returns>HTML 文本</returns>
        public string ToHtml(string markdown)
        {
            if (string.IsNullOrEmpty(markdown))
            {
                return string.Empty;
            }

            try
            {
                // 规范化换行符
                string text = NormalizeLineEndings(markdown);

                // 处理代码块（必须在其他处理之前）
                text = ProcessCodeBlocks(text);

                // 处理块级元素
                text = ProcessHeaders(text);
                text = ProcessHorizontalRules(text);
                text = ProcessBlockquotes(text);
                text = ProcessLists(text);
                text = ProcessTables(text);

                // 处理行内元素
                text = ProcessInlineCode(text);
                text = ProcessBoldAndItalic(text);
                text = ProcessStrikethrough(text);
                text = ProcessLinks(text);
                text = ProcessImages(text);

                // 处理特殊提示框
                text = ProcessAlertBoxes(text);

                // 处理段落
                text = ProcessParagraphs(text);

                // 处理换行
                text = ProcessLineBreaks(text);

                return text;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Markdown 渲染失败: {ex.Message}");
                return $"<p>Markdown 渲染错误: {EscapeHtml(ex.Message)}</p>";
            }
        }

        #endregion

        #region 私有方法 - 规范化

        /// <summary>
        /// 规范化换行符
        /// </summary>
        private string NormalizeLineEndings(string text)
        {
            return text.Replace("\r\n", "\n").Replace('\r', '\n');
        }

        /// <summary>
        /// HTML 转义
        /// </summary>
        private string EscapeHtml(string text)
        {
            return text
                .Replace("&", "&amp;")
                .Replace("<", "&lt;")
                .Replace(">", "&gt;")
                .Replace("\"", "&quot;")
                .Replace("'", "&#39;");
        }

        #endregion

        #region 私有方法 - 块级元素处理

        /// <summary>
        /// 处理代码块
        /// </summary>
        private string ProcessCodeBlocks(string text)
        {
            // 匹配 ```language code ``` 格式的代码块
            string pattern = @"```(\w*)\n([\s\S]*?)```";
            
            return Regex.Replace(
                text,
                pattern,
                match =>
                {
                    string language = match.Groups[1].Value;
                    string code = match.Groups[2].Value.Trim();
                    
                    // 转义 HTML
                    string escapedCode = EscapeHtml(code);
                    
                    return $"<pre><code>{escapedCode}</code></pre>";
                },
                RegexOptions.Multiline);
        }

        /// <summary>
        /// 处理标题
        /// </summary>
        private string ProcessHeaders(string text)
        {
            // 处理 ATX 风格标题 (# ## ###)
            text = Regex.Replace(text, @"^#{1} (.+)$", "<h1>$1</h1>", RegexOptions.Multiline);
            text = Regex.Replace(text, @"^#{2} (.+)$", "<h2>$1</h2>", RegexOptions.Multiline);
            text = Regex.Replace(text, @"^#{3} (.+)$", "<h3>$1</h3>", RegexOptions.Multiline);
            text = Regex.Replace(text, @"^#{4} (.+)$", "<h4>$1</h4>", RegexOptions.Multiline);
            text = Regex.Replace(text, @"^#{5} (.+)$", "<h5>$1</h5>", RegexOptions.Multiline);
            text = Regex.Replace(text, @"^#{6} (.+)$", "<h6>$1</h6>", RegexOptions.Multiline);

            return text;
        }

        /// <summary>
        /// 处理水平线
        /// </summary>
        private string ProcessHorizontalRules(string text)
        {
            // 匹配 --- 或 *** 或 ___
            string pattern = @"^(?:\*\*\*|---|___)\s*$";
            
            return Regex.Replace(
                text,
                pattern,
                "<hr />",
                RegexOptions.Multiline);
        }

        /// <summary>
        /// 处理引用块
        /// </summary>
        private string ProcessBlockquotes(string text)
        {
            // 匹配 > 引用块
            string pattern = @"^(>\s*.+)$";
            
            return Regex.Replace(
                text,
                pattern,
                match =>
                {
                    string content = match.Groups[1].Value.Substring(1).Trim();
                    return $"<blockquote>{content}</blockquote>";
                },
                RegexOptions.Multiline);
        }

        /// <summary>
        /// 处理列表
        /// </summary>
        private string ProcessLists(string text)
        {
            // 处理有序列表
            text = Regex.Replace(
                text,
                @"^\d+\.\s+(.+)$",
                "<li>$1</li>",
                RegexOptions.Multiline);

            // 处理无序列表
            text = Regex.Replace(
                text,
                @"^[\-\*]\s+(.+)$",
                "<li>$1</li>",
                RegexOptions.Multiline);

            // 将连续的 li 包装成 ul/ol
            text = Regex.Replace(
                text,
                @"(<li>.*</li>\n?)+",
                match =>
                {
                    string listContent = match.Value;
                    
                    // 判断是有序还是无序列表
                    bool isOrdered = Regex.IsMatch(listContent, @"^\d+\.");
                    string tag = isOrdered ? "ol" : "ul";
                    
                    return $"<{tag}>{listContent}</{tag}>";
                },
                RegexOptions.Multiline);

            return text;
        }

        /// <summary>
        /// 处理表格
        /// </summary>
        private string ProcessTables(string text)
        {
            // 简单的表格匹配
            // | Header 1 | Header 2 |
            // |----------|----------|
            // | Cell 1   | Cell 2   |
            
            // 匹配表格行
            string rowPattern = @"^\|(.+)\|$";
            var lines = text.Split('\n');
            var result = new StringBuilder();
            int i = 0;

            while (i < lines.Length)
            {
                string line = lines[i];
                
                // 检查是否是表格行
                if (Regex.IsMatch(line, rowPattern))
                {
                    var tableRows = new StringBuilder();
                    bool isHeader = true;
                    bool hasSeparator = false;

                    // 收集表格行
                    while (i < lines.Length)
                    {
                        line = lines[i];
                        
                        if (!Regex.IsMatch(line, rowPattern))
                        {
                            break;
                        }

                        // 检查是否是分隔线（---）
                        if (Regex.IsMatch(line, @"^\|[\s\-\:]+\|$"))
                        {
                            hasSeparator = true;
                            i++;
                            isHeader = false;
                            continue;
                        }

                        // 提取单元格
                        string cells = Regex.Match(line, rowPattern).Groups[1].Value;
                        string[] cellArray = cells.Split('|');
                        
                        var rowBuilder = new StringBuilder();
                        foreach (string cell in cellArray)
                        {
                            string trimmedCell = cell.Trim();
                            rowBuilder.Append($"<td>{trimmedCell}</td>");
                        }

                        if (isHeader)
                        {
                            // 表头使用 th
                            rowBuilder.Replace("<td>", "<th>").Replace("</td>", "</th>");
                        }

                        tableRows.AppendLine($"<tr>{rowBuilder}</tr>");
                        i++;
                    }

                    // 如果有分隔线，则认为是表格
                    if (hasSeparator)
                    {
                        result.AppendLine($"<table>{tableRows}</table>");
                    }
                }
                else
                {
                    result.AppendLine(line);
                    i++;
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// 处理段落
        /// </summary>
        private string ProcessParagraphs(string text)
        {
            // 将未标记的文本行包装为段落
            string[] lines = text.Split('\n');
            var result = new StringBuilder();
            var paragraphBuilder = new StringBuilder();

            foreach (string line in lines)
            {
                string trimmed = line.Trim();

                // 跳过空行
                if (string.IsNullOrEmpty(trimmed))
                {
                    if (paragraphBuilder.Length > 0)
                    {
                        result.AppendLine($"<p>{paragraphBuilder}</p>");
                        paragraphBuilder.Clear();
                    }
                    continue;
                }

                // 跳过已经是 HTML 标签的行
                if (trimmed.StartsWith("<"))
                {
                    if (paragraphBuilder.Length > 0)
                    {
                        result.AppendLine($"<p>{paragraphBuilder}</p>");
                        paragraphBuilder.Clear();
                    }
                    result.AppendLine(trimmed);
                    continue;
                }

                // 添加到段落构建器
                if (paragraphBuilder.Length > 0)
                {
                    paragraphBuilder.Append(" ");
                }
                paragraphBuilder.Append(trimmed);
            }

            // 处理最后一个段落
            if (paragraphBuilder.Length > 0)
            {
                result.AppendLine($"<p>{paragraphBuilder}</p>");
            }

            return result.ToString();
        }

        /// <summary>
        /// 处理换行
        /// </summary>
        private string ProcessLineBreaks(string text)
        {
            // 在段落内的两个空格后添加换行
            return Regex.Replace(
                text,
                @"  \n",
                "<br />\n");
        }

        #endregion

        #region 私有方法 - 行内元素处理

        /// <summary>
        /// 处理行内代码
        /// </summary>
        private string ProcessInlineCode(string text)
        {
            // 匹配 `code`
            return Regex.Replace(
                text,
                @"`([^`]+)`",
                "<code>$1</code>");
        }

        /// <summary>
        /// 处理粗体和斜体
        /// </summary>
        private string ProcessBoldAndItalic(string text)
        {
            // 处理粗体 **text**
            text = Regex.Replace(text, @"\*\*([^*]+)\*\*", "<strong>$1</strong>");
            
            // 处理粗体 __text__
            text = Regex.Replace(text, @"__([^_]+)__", "<strong>$1</strong>");
            
            // 处理斜体 *text*
            text = Regex.Replace(text, @"\*([^*]+)\*", "<em>$1</em>");
            
            // 处理斜体 _text_
            text = Regex.Replace(text, @"_([^_]+)_", "<em>$1</em>");

            return text;
        }

        /// <summary>
        /// 处理删除线
        /// </summary>
        private string ProcessStrikethrough(string text)
        {
            // 匹配 ~~text~~
            return Regex.Replace(
                text,
                @"~~(.+?)~~",
                "<del>$1</del>");
        }

        /// <summary>
        /// 处理链接
        /// </summary>
        private string ProcessLinks(string text)
        {
            // 匹配 [text](url)
            return Regex.Replace(
                text,
                @"\[([^\]]+)\]\(([^\)]+)\)",
                "<a href=\"$2\" target=\"_blank\">$1</a>");
        }

        /// <summary>
        /// 处理图片
        /// </summary>
        private string ProcessImages(string text)
        {
            // 匹配 ![alt](url)
            return Regex.Replace(
                text,
                @"!\[([^\]]*)\]\(([^\)]+)\)",
                "<img src=\"$2\" alt=\"$1\" />");
        }

        /// <summary>
        /// 处理特殊提示框
        /// </summary>
        private string ProcessAlertBoxes(string text)
        {
            // 匹配 > [NOTE] text
            text = Regex.Replace(text, @"^>\s*\[NOTE\]\s*(.+)$", "<div class=\"note\">$1</div>", RegexOptions.Multiline);
            
            // 匹配 > [TIP] text
            text = Regex.Replace(text, @"^>\s*\[TIP\]\s*(.+)$", "<div class=\"tip\">$1</div>", RegexOptions.Multiline);
            
            // 匹配 > [WARNING] text
            text = Regex.Replace(text, @"^>\s*\[WARNING\]\s*(.+)$", "<div class=\"warning\">$1</div>", RegexOptions.Multiline);
            
            // 匹配 > [INFO] text
            text = Regex.Replace(text, @"^>\s*\[INFO\]\s*(.+)$", "<div class=\"info\">$1</div>", RegexOptions.Multiline);

            return text;
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
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // 释放托管资源
                }

                _disposed = true;
            }
        }

        #endregion
    }
}
