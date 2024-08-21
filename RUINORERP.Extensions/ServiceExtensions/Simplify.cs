using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Extensions.ServiceExtensions
{
    public class SimplifyExpression : ExpressionVisitor
    {
        protected override Expression VisitMember(MemberExpression node)
        {
            var expr = Visit(node.Expression);
            if (expr is ConstantExpression c)
            {
                if (node.Member is PropertyInfo prop)
                    return Expression.Constant(prop.GetValue(c.Value), prop.PropertyType);
                if (node.Member is FieldInfo field)
                    return Expression.Constant(field.GetValue(c.Value), field.FieldType);
            }
            return node.Update(expr);
        }
    }
}
