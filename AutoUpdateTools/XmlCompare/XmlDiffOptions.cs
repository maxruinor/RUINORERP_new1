using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoUpdateTools.XmlCompare
{
    /// <summary>
    /// XML比较选项配置
    /// </summary>
    public class XmlDiffOptions
    {
        /// <summary>
        /// 是否忽略XML命名空间
        /// </summary>
        public bool IgnoreNamespaces { get; set; } = true;

        /// <summary>
        /// 是否忽略属性顺序
        /// </summary>
        public bool IgnoreAttributeOrder { get; set; } = true;

        /// <summary>
        /// 是否忽略注释
        /// </summary>
        public bool IgnoreComments { get; set; } = true;

        /// <summary>
        /// 是否忽略空白节点
        /// </summary>
        public bool IgnoreWhitespace { get; set; } = true;
    }
}
