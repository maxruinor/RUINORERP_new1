using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Global.Extensions
{
    public static class ExpressionExt
    {
        /// <summary>
        /// 为了这里能取到列名。冗余了这个方法
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static MemberInfo GetMemberInfoGlobal(this Expression expression)
        {

            var visitor = new GetMemberVisitorGlobal();
            visitor.Visit(expression);
            return visitor.Member;
        }
    }

    public class GetMemberVisitorGlobal : ExpressionVisitor
    {
        public MemberInfo Member { get; set; }

        protected override Expression VisitMember(MemberExpression node)
        {
            //静态成员
            if (node.Expression == null)
            {
                //if (node.Member.MemberType == MemberTypes.Property)
                //    Member = node.Type.GetProperty(node.Member.Name, BindingFlags.Static | BindingFlags.Public)
                //        ?.GetValue(null);
                //else if (node.Member.MemberType == MemberTypes.Field)
                //    Member = node.Type.GetField(node.Member.Name, BindingFlags.Static | BindingFlags.Public)?.GetValue(null);
            }
            else //对象成员
            {


                if (node.Expression is System.Linq.Expressions.ParameterExpression)
                {
                    var obj = (node.Expression as ParameterExpression).Name;
                    Member = node.Member;

                }
                if (node.Expression is System.Linq.Expressions.ConstantExpression)
                {

                }

            }

            return base.VisitMember(node);
        }
    }
}
