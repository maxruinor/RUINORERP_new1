using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace RUINORERP.UI.HelpSystem
{
    /// <summary>
    /// 帮助内容搜索管理器
    /// </summary>
    public static class HelpSearchManager
    {
        /// <summary>
        /// 搜索结果项
        /// </summary>
        public class SearchResultItem
        {
            /// <summary>
            /// 帮助页面路径
            /// </summary>
            public string HelpPage { get; set; }

            /// <summary>
            /// 页面标题
            /// </summary>
            public string Title { get; set; }

            /// <summary>
            /// 匹配的文本片段
            /// </summary>
            public string Snippet { get; set; }

            /// <summary>
            /// 匹配度评分
            /// </summary>
            public double Score { get; set; }
        }

        /// <summary>
        /// 在帮助内容中搜索
        /// </summary>
        /// <param name="keywords">搜索关键词</param>
        /// <param name="maxResults">最大返回结果数</param>
        /// <returns>搜索结果列表</returns>
        public static List<SearchResultItem> Search(string keywords, int maxResults = 60)
        {
            var results = new List<SearchResultItem>();
            
            if (string.IsNullOrEmpty(keywords) || !HelpManager.Config.IsSearchEnabled)
                return results;

            try
            {
                // 获取帮助文件路径
                string helpFilePath = HelpManager.HelpFilePath;
                if (string.IsNullOrEmpty(helpFilePath) || !File.Exists(helpFilePath))
                    return results;

                // 解析CHM文件并搜索内容
                // 注意：这里是一个简化的实现，实际应用中可能需要使用专门的CHM解析库
                // 或者预先生成索引文件来提高搜索性能
                
                // 模拟搜索过程
                var searchResults = PerformSearchInHelpContent(keywords, maxResults);
                results.AddRange(searchResults);
                
                // 按评分排序
                return results.OrderByDescending(r => r.Score).ToList();
            }
            catch (Exception ex)
            {
                // 记录日志但不抛出异常
                System.Diagnostics.Debug.WriteLine($"帮助内容搜索时出错: {ex.Message}");
                return results;
            }
        }

        /// <summary>
        /// 在帮助内容中执行搜索
        /// </summary>
        /// <param name="keywords">搜索关键词</param>
        /// <param name="maxResults">最大返回结果数</param>
        /// <returns>搜索结果列表</returns>
        private static List<SearchResultItem> PerformSearchInHelpContent(string keywords, int maxResults)
        {
            var results = new List<SearchResultItem>();
            
            // 这里应该实现实际的CHM文件内容搜索逻辑
            // 由于CHM文件格式复杂，通常需要：
            // 1. 使用CHM解析库读取文件内容
            // 2. 建立内容索引以提高搜索性能
            // 3. 执行文本搜索并提取相关片段
            
            // 作为示例，我们返回一些模拟结果
            string[] samplePages = {
                "forms/main_form.html",
                "controls/button_save.html",
                "controls/textbox_name.html",
                "basics/product_management.html",
                "documents/sales_order.html",
                "lists/customer_list.html"
            };
            
            string[] sampleTitles = {
                "主窗体",
                "保存按钮",
                "姓名文本框",
                "产品管理",
                "销售订单",
                "客户列表"
            };
            
            Random random = new Random();
            for (int i = 0; i < Math.Min(samplePages.Length, maxResults); i++)
            {
                results.Add(new SearchResultItem
                {
                    HelpPage = samplePages[i],
                    Title = sampleTitles[i],
                    Snippet = $"这是关于{sampleTitles[i]}的帮助内容片段，包含关键词'{keywords}'的相关信息。",
                    Score = random.NextDouble() * 100
                });
            }
            
            return results;
        }

        /// <summary>
        /// 从HTML内容中提取纯文本
        /// </summary>
        /// <param name="html">HTML内容</param>
        /// <returns>纯文本</returns>
        private static string ExtractTextFromHtml(string html)
        {
            // 移除HTML标签
            string text = Regex.Replace(html, "<.*?>", string.Empty);
            
            // 解码HTML实体
            text = System.Net.WebUtility.HtmlDecode(text);
            
            // 移除多余的空白字符
            text = Regex.Replace(text, @"\s+", " ").Trim();
            
            return text;
        }

        /// <summary>
        /// 从文本中提取包含关键词的片段
        /// </summary>
        /// <param name="text">文本内容</param>
        /// <param name="keywords">关键词</param>
        /// <param name="snippetLength">片段长度</param>
        /// <returns>文本片段</returns>
        private static string ExtractSnippet(string text, string keywords, int snippetLength = 150)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(keywords))
                return string.Empty;

            int index = text.IndexOf(keywords, StringComparison.OrdinalIgnoreCase);
            if (index < 0)
                return string.Empty;

            int start = Math.Max(0, index - snippetLength / 2);
            int length = Math.Min(snippetLength, text.Length - start);
            
            string snippet = text.Substring(start, length);
            
            // 如果不是从文本开头截取，添加前缀
            if (start > 0)
                snippet = "..." + snippet;
                
            // 如果不是到文本结尾截取，添加后缀
            if (start + length < text.Length)
                snippet = snippet + "...";
                
            return snippet;
        }

        /// <summary>
        /// 计算文本匹配度评分
        /// </summary>
        /// <param name="text">文本内容</param>
        /// <param name="keywords">关键词</param>
        /// <returns>评分</returns>
        private static double CalculateMatchScore(string text, string keywords)
        {
            if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(keywords))
                return 0.0;

            // 简单的评分算法：
            // 1. 包含关键词的次数
            // 2. 关键词在文本中的位置
            // 3. 关键词与文本长度的比例
            
            int keywordCount = Regex.Matches(text, Regex.Escape(keywords), RegexOptions.IgnoreCase).Count;
            int firstOccurrence = text.IndexOf(keywords, StringComparison.OrdinalIgnoreCase);
            
            double score = keywordCount * 10; // 每出现一次加10分
            
            if (firstOccurrence >= 0)
            {
                // 出现在文本前面的关键词加分更多
                double positionScore = (1.0 - (double)firstOccurrence / text.Length) * 20;
                score += positionScore;
            }
            
            // 关键词密度加分
            double density = (double)keywords.Length * keywordCount / text.Length;
            score += density * 100;
            
            return score;
        }
    }
}