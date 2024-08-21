using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace RUINORERP.Global
{
    public class Utils
    {
        public static string GetMemberName<T>(Expression<Func<T>> expr)
        {
            var bodyExpr = expr.Body as System.Linq.Expressions.MemberExpression;
            if (bodyExpr == null)
                return string.Empty;
            return bodyExpr.Member.Name;
        }
    }
}
