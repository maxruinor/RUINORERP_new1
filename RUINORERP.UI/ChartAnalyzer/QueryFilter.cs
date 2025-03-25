using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.ChartAnalyzer
{
    // 辅助类
    /// <summary>
    /// 查询过滤器
    /// </summary>
    public class QueryFilter
    {
        public string Field { get; set; }
        public string Operator { get; set; } // >, <, >=, <=, ==, !=
        public object Value { get; set; }

        public QueryFilter() { }

        public QueryFilter(string field, string op, object value)
        {
            Field = field;
            Operator = op;
            Value = value;
        }
    }
}
