using System;
using System.Collections.Generic;
using System.Text;

namespace RUINORERP.Global.CustomAttribute
{
    /// <summary>
    /// 表格的外观控制
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class GridVisualAttribute : Attribute
    {
        /// <summary>
        /// 缓存绝对过期时间（分钟）
        /// </summary>
        //public int AbsoluteExpiration { get; set; } = 30;

    }
}
