using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
namespace RUINORERP.Common.Extensions
{
    /*
     然后就是接口的定义

 

1
2
3
4
public virtual List<T> GetMoreParamesPagedList<TKey>(int pageIndex, int pageSize, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderBy)
{
return db.Set<T>().Where(whereLambda.Compile()).AsQueryable().OrderBy(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
}
　　接下来就是实现了

1
2
3
4
5
6
var where = PredicateBuilder.True<BaoGaiTouBit>();
where = where.And(e => e.IsEnable);
where = where.And(e => e.DeadLine >= mindate);
where = where.And(e => e.DeadLine < maxdate);
list = OperateContext.Current.BLLSession.IBaoGaiTouBitBLL.GetMoreParamesPagedList(pageIndex, pageSize, where, b => b.CreateTime);
return Json(list);

     
     */
    public static class PredicateBuilder
    {

        /// <summary>
        /// 机关函数应用True时：单个AND有效，多个AND有效；单个OR无效，多个OR无效；混应时写在AND后的OR有效
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> True<T>() { return f => true; }

        /// <summary>
        /// 机关函数应用False时：单个AND无效，多个AND无效；单个OR有效，多个OR有效；混应时写在OR后面的AND有效
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> False<T>() { return f => false; }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1,
        Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
            (Expression.Or(expr1.Body, invokedExpr), expr1.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1,
        Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
            return Expression.Lambda<Func<T, bool>>
            (Expression.And(expr1.Body, invokedExpr), expr1.Parameters);
        }
    }

    //代码要进一步检验
    /// <summary>
    /// 合并表达式 And Or  Not扩展
    /// </summary>
    public static class ExpressionMergeHelper
    {
        /// <summary>
        /// 合并表达式 expr1 AND expr2
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expr1"></param>
        /// <param name="expr2"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            if (expr1 == null)
                return expr2;
            else if (expr2 == null)
                return expr1;
            ParameterExpression newParameter = Expression.Parameter(typeof(T), "c");
            NewExpressionVisitor visitor = new NewExpressionVisitor(newParameter);
            var left = visitor.Replace(expr1.Body);
            var right = visitor.Replace(expr2.Body);
            var body = Expression.And(left, right);
            return Expression.Lambda<Func<T, bool>>(body, newParameter);
        }
        /// <summary>
        /// 合并表达式 expr1 or expr2
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expr1"></param>
        /// <param name="expr2"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            if (expr1 == null)
                return expr2;
            else if (expr2 == null)
                return expr1;
            ParameterExpression newParameter = Expression.Parameter(typeof(T), "c");
            NewExpressionVisitor visitor = new NewExpressionVisitor(newParameter);

            var left = visitor.Replace(expr1.Body);
            var right = visitor.Replace(expr2.Body);
            var body = Expression.Or(left, right);
            return Expression.Lambda<Func<T, bool>>(body, newParameter);
        }
        /// <summary>
        /// 取反表达式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expr"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> expr)
        {
            if (expr == null) return null;
            var candidateExpr = expr.Parameters[0];
            var body = Expression.Not(expr.Body);

            return Expression.Lambda<Func<T, bool>>(body, candidateExpr);
        }
        /// <summary>
        /// 获取表达式属性列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expr"></param>
        /// <returns></returns>
        public static List<MemberInfo> SelectMembers<T>(this Expression<Func<T, object>> expr)
        {
            return SelectMembers(expr.Body);
        }
        private static List<MemberInfo> SelectMembers(Expression properties)
        {
            var newExp = (NewExpression)properties;
            if (newExp != null)
            {
                return newExp.Members.ToList();
            }
            else
            {
                var newArr = (NewArrayExpression)properties;
                if (newArr != null)
                {
                    List<MemberInfo> newArrMembers = new List<MemberInfo>();
                    foreach (var newArrExp in newArr.Expressions)
                        newArrMembers.AddRange(SelectMembers(newArrExp));
                    return newArrMembers.Distinct().ToList();
                }
                return null;
            }
        }
    }
    public class NewExpressionVisitor : ExpressionVisitor
    {
        public ParameterExpression Parameter { get; set; }
        public NewExpressionVisitor() { }
        public NewExpressionVisitor(ParameterExpression parameter)
        {
            this.Parameter = parameter;
        }
        public Expression Replace(Expression exp)
        {
            return this.Visit(exp);
        }
        protected override Expression VisitParameter(ParameterExpression parameter)
        {
            return this.Parameter;
        }
    }
}
