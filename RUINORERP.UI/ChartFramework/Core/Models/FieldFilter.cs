using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.ChartFramework.Models
{

    public sealed class FieldFilter
    {
        public string Field { get; set; }
        public FilterOperator Operator { get; set; }
        public object Value { get; set; }

        // 生成SQL条件表达式
        public string ToSqlExpression()
        {
            return Operator switch
            {
                FilterOperator.Equals => $"{Field} = @Value",
                FilterOperator.GreaterThan => $"{Field} > @Value",
                FilterOperator.LessThan => $"{Field} < @Value",
                FilterOperator.NotEquals => $"{Field} <> @Value",
                FilterOperator.Contains => $"{Field} LIKE '%' + @Value + '%'",
                // 其他运算符...
                _ => throw new NotSupportedException()
            };
        }
    }

    public enum FilterOperator
    {
        Equals,
        NotEquals,
        GreaterThan,
        LessThan,
        Contains
    }
}
