using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.UCSourceGrid
{
    /// <summary>
    /// 字段映射关系（包含计算逻辑）
    /// </summary>
    public class QueryColumnMapping
    {
        /// <summary>
        /// 源字段名称
        /// </summary>
        public string SourceColumnName { get; set; }

        /// <summary>
        /// 目标列定义
        /// </summary>
        public SGDefineColumnItem TargetColumn { get; set; }

        /// <summary>
        /// 计算委托：接收当前明细对象，返回计算后的值
        /// </summary>
        public Func<object, object> ValueCalculator { get; set; }
    }
}
