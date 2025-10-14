using System;

namespace RUINORERP.UI.Common.HelpSystem
{
    /// <summary>
    /// 帮助映射特性，用于标记窗体对应的帮助页面
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class HelpMappingAttribute : Attribute
    {
        /// <summary>
        /// 帮助页面路径
        /// </summary>
        public string HelpPage { get; }

        /// <summary>
        /// 帮助页面标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="helpPage">帮助页面路径</param>
        public HelpMappingAttribute(string helpPage)
        {
            HelpPage = helpPage ?? throw new ArgumentNullException(nameof(helpPage));
        }
    }
}