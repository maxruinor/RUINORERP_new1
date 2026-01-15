using System;

namespace RUINORERP.UI.HelpSystem.Core
{
    /// <summary>
    /// 帮助搜索结果类
    /// 表示帮助搜索返回的单个结果项
    /// 包含帮助键、标题、内容摘要、完整内容和相关度分数等信息
    /// </summary>
    public class HelpSearchResult
    {
        #region 公共属性

        /// <summary>
        /// 帮助键
        /// 唯一标识帮助内容的键值,用于定位具体的帮助文件
        /// 格式示例: "field.tb_SaleOrder.CustomerID"
        /// </summary>
        public string HelpKey { get; set; }

        /// <summary>
        /// 帮助标题
        /// 帮助内容的标题,通常从HTML的h1标签或Markdown的#标题提取
        /// 用于在搜索结果列表中显示
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 内容摘要
        /// 帮助内容的简短摘要,通常是前200个字符
        /// 用于在搜索结果列表中快速预览帮助内容
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 完整内容
        /// 帮助的完整HTML或Markdown内容
        /// 用于在帮助面板中完整显示
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 相关度分数
        /// 表示搜索结果与查询关键词的匹配程度
        /// 分数越高表示相关度越高,用于结果排序
        /// </summary>
        public double RelevanceScore { get; set; }

        /// <summary>
        /// 帮助级别
        /// 标识该帮助内容的级别(字段、控件、窗体、模块)
        /// 用于分类显示和过滤
        /// </summary>
        public HelpLevel Level { get; set; }

        /// <summary>
        /// 帮助文件路径
        /// 帮助内容文件的完整路径
        /// 用于快速定位和加载帮助文件
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 最后修改时间
        /// 帮助内容的最后修改时间
        /// 用于判断帮助内容是否需要更新
        /// </summary>
        public DateTime LastModified { get; set; }

        #endregion

        #region 构造函数

        /// <summary>
        /// 默认构造函数
        /// 初始化帮助搜索结果对象
        /// </summary>
        public HelpSearchResult()
        {
            LastModified = DateTime.Now;
        }

        /// <summary>
        /// 完整构造函数
        /// </summary>
        /// <param name="helpKey">帮助键</param>
        /// <param name="title">标题</param>
        /// <param name="summary">摘要</param>
        /// <param name="content">完整内容</param>
        /// <param name="level">帮助级别</param>
        public HelpSearchResult(string helpKey, string title, string summary, string content, HelpLevel level)
            : this()
        {
            HelpKey = helpKey;
            Title = title;
            Summary = summary;
            Content = content;
            Level = level;
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 获取帮助的简要描述
        /// </summary>
        /// <returns>简短描述字符串</returns>
        public string GetBriefDescription()
        {
            return string.IsNullOrEmpty(Title) ? HelpKey : Title;
        }

        /// <summary>
        /// 判断帮助内容是否有效
        /// </summary>
        /// <returns>有效返回true,否则返回false</returns>
        public bool IsValid()
        {
            return !string.IsNullOrEmpty(HelpKey) && !string.IsNullOrEmpty(Content);
        }

        /// <summary>
        /// 获取帮助级别名称
        /// </summary>
        /// <returns>帮助级别的中文名称</returns>
        public string GetLevelName()
        {
            switch (Level)
            {
                case HelpLevel.Field:
                    return "字段";
                case HelpLevel.Control:
                    return "控件";
                case HelpLevel.Form:
                    return "窗体";
                case HelpLevel.Module:
                    return "模块";
                default:
                    return "未知";
            }
        }

        #endregion

        #region 重写方法

        /// <summary>
        /// 重写ToString方法,返回搜索结果的字符串表示
        /// </summary>
        /// <returns>搜索结果字符串</returns>
        public override string ToString()
        {
            return $"[{GetLevelName()}] {GetBriefDescription()} (相关度: {RelevanceScore:F2})";
        }

        /// <summary>
        /// 重写Equals方法,比较两个搜索结果是否相等
        /// </summary>
        /// <param name="obj">比较对象</param>
        /// <returns>相等返回true,否则返回false</returns>
        public override bool Equals(object obj)
        {
            if (obj is HelpSearchResult other)
            {
                return string.Equals(HelpKey, other.HelpKey, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        /// <summary>
        /// 重写GetHashCode方法
        /// </summary>
        /// <returns>哈希值</returns>
        public override int GetHashCode()
        {
            return HelpKey?.GetHashCode() ?? 0;
        }

        #endregion
    }
}
