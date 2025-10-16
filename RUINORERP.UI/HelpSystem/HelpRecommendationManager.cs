using System;
using System.Collections.Generic;
using System.Linq;

namespace RUINORERP.UI.HelpSystem
{
    /// <summary>
    /// 帮助内容推荐管理器
    /// </summary>
    public static class HelpRecommendationManager
    {
        /// <summary>
        /// 推荐项
        /// </summary>
        public class RecommendationItem
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
            /// 推荐理由
            /// </summary>
            public string Reason { get; set; }

            /// <summary>
            /// 推荐权重
            /// </summary>
            public double Weight { get; set; }
        }

        /// <summary>
        /// 获取推荐的帮助内容
        /// </summary>
        /// <param name="currentHelpPage">当前查看的帮助页面</param>
        /// <param name="maxRecommendations">最大推荐数量</param>
        /// <returns>推荐列表</returns>
        public static List<RecommendationItem> GetRecommendations(string currentHelpPage = null, int maxRecommendations = 5)
        {
            var recommendations = new List<RecommendationItem>();
            
            if (!HelpManager.Config.IsRecommendationEnabled)
                return recommendations;

            try
            {
                // 1. 基于历史记录的推荐
                var historyBasedRecommendations = GetHistoryBasedRecommendations(currentHelpPage);
                recommendations.AddRange(historyBasedRecommendations);

                // 2. 基于相关内容的推荐
                var contentBasedRecommendations = GetContentBasedRecommendations(currentHelpPage);
                recommendations.AddRange(contentBasedRecommendations);

                // 3. 基于热门内容的推荐
                var popularRecommendations = GetPopularRecommendations(currentHelpPage);
                recommendations.AddRange(popularRecommendations);

                // 去重并按权重排序
                var uniqueRecommendations = recommendations
                    .GroupBy(r => r.HelpPage)
                    .Select(g => new RecommendationItem
                    {
                        HelpPage = g.Key,
                        Title = g.First().Title,
                        Reason = string.Join("; ", g.Select(r => r.Reason).Distinct()),
                        Weight = g.Sum(r => r.Weight)
                    })
                    .OrderByDescending(r => r.Weight)
                    .Take(maxRecommendations)
                    .ToList();

                return uniqueRecommendations;
            }
            catch (Exception ex)
            {
                // 记录日志但不抛出异常
                System.Diagnostics.Debug.WriteLine($"获取帮助推荐时出错: {ex.Message}");
                return recommendations;
            }
        }

        /// <summary>
        /// 基于历史记录的推荐
        /// </summary>
        /// <param name="currentHelpPage">当前查看的帮助页面</param>
        /// <returns>推荐列表</returns>
        private static List<RecommendationItem> GetHistoryBasedRecommendations(string currentHelpPage)
        {
            var recommendations = new List<RecommendationItem>();
            
            // 获取用户最常查看的帮助页面
            var mostViewed = HelpHistoryManager.GetMostViewed(10);
            
            foreach (var item in mostViewed)
            {
                // 排除当前页面
                if (item.HelpPage == currentHelpPage)
                    continue;
                    
                recommendations.Add(new RecommendationItem
                {
                    HelpPage = item.HelpPage,
                    Title = item.Title,
                    Reason = "您经常查看此帮助内容",
                    Weight = item.ViewCount * 2.0 // 查看次数越多权重越高
                });
            }
            
            return recommendations;
        }

        /// <summary>
        /// 基于相关内容的推荐
        /// </summary>
        /// <param name="currentHelpPage">当前查看的帮助页面</param>
        /// <returns>推荐列表</returns>
        private static List<RecommendationItem> GetContentBasedRecommendations(string currentHelpPage)
        {
            var recommendations = new List<RecommendationItem>();
            
            if (string.IsNullOrEmpty(currentHelpPage))
                return recommendations;

            // 基于当前页面内容推荐相关页面
            // 这里是一个简化的实现，实际应用中可以：
            // 1. 分析页面内容的相似性
            // 2. 基于页面分类进行推荐
            // 3. 基于用户行为模式进行推荐
            
            // 根据页面路径进行相关性推荐
            if (currentHelpPage.Contains("button"))
            {
                recommendations.Add(new RecommendationItem
                {
                    HelpPage = "controls/button_general.html",
                    Title = "按钮控件通用帮助",
                    Reason = "与当前按钮相关的内容",
                    Weight = 5.0
                });
            }
            else if (currentHelpPage.Contains("textbox"))
            {
                recommendations.Add(new RecommendationItem
                {
                    HelpPage = "controls/textbox_general.html",
                    Title = "文本框控件通用帮助",
                    Reason = "与当前文本框相关的内容",
                    Weight = 5.0
                });
            }
            else if (currentHelpPage.StartsWith("forms/"))
            {
                recommendations.Add(new RecommendationItem
                {
                    HelpPage = "forms/general/form_navigation.html",
                    Title = "窗体导航帮助",
                    Reason = "与窗体操作相关的内容",
                    Weight = 4.0
                });
            }
            else if (currentHelpPage.StartsWith("basics/"))
            {
                recommendations.Add(new RecommendationItem
                {
                    HelpPage = "basics/general/basic_operations.html",
                    Title = "基础操作帮助",
                    Reason = "与基础操作相关的内容",
                    Weight = 4.0
                });
            }

            return recommendations;
        }

        /// <summary>
        /// 基于热门内容的推荐
        /// </summary>
        /// <param name="currentHelpPage">当前查看的帮助页面</param>
        /// <returns>推荐列表</returns>
        private static List<RecommendationItem> GetPopularRecommendations(string currentHelpPage)
        {
            var recommendations = new List<RecommendationItem>();
            
            // 获取系统中最热门的帮助页面（模拟数据）
            var popularPages = new[]
            {
                new { Page = "forms/main_form.html", Title = "主窗体帮助" },
                new { Page = "controls/button_save.html", Title = "保存按钮帮助" },
                new { Page = "basics/product_management.html", Title = "产品管理帮助" },
                new { Page = "documents/sales_order.html", Title = "销售订单帮助" },
                new { Page = "lists/customer_list.html", Title = "客户列表帮助" }
            };
            
            foreach (var page in popularPages)
            {
                // 排除当前页面
                if (page.Page == currentHelpPage)
                    continue;
                    
                recommendations.Add(new RecommendationItem
                {
                    HelpPage = page.Page,
                    Title = page.Title,
                    Reason = "这是系统中最受欢迎的帮助内容",
                    Weight = 3.0
                });
            }
            
            return recommendations;
        }
    }
}