using AutoMapper.QueryableExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Windows.Media.Media3D;

namespace RUINORERP.Common.Helper
{

    /// <summary>
    /// 下面还有一个ExpressionHelper帮助类 还没有重构掉
    /// </summary>
    public static class ExpressionUtils
    {


        #region 这种方法假设表达式树是按照逻辑与（AND）构建的。如果表达式树包含其他类型的逻辑运算符，你可能需要扩展这个方法来处理这些情况。

        /// <summary>
        /// 移除表达式中的指定条件,这里的条件是一个等式所以不对。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <param name="propertiesToExclude"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> RemoveConditions<T>(Expression<Func<T, bool>> expression, params string[] propertiesToExclude)
        {
            var parameter = expression.Parameters.Single();
            var body = RemoveConditions(expression.Body, parameter, propertiesToExclude);
            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        public static Expression RemoveConditions(Expression expression, ParameterExpression parameter, string[] propertiesToExclude)
        {
            //两个等式时  and 连接
            if (expression is BinaryExpression binaryExpression && binaryExpression.NodeType == ExpressionType.AndAlso)
            {
                Expression left = binaryExpression.Left;
                Expression right = binaryExpression.Right; ;

                // 检查是否需要排除这个条件
                if (IsPropertyCondition(binaryExpression.Left, parameter, propertiesToExclude))
                {
                 
                    left= RemoveConditions(binaryExpression.Right, parameter, propertiesToExclude);
                }
               
                if (IsPropertyCondition(binaryExpression.Left, parameter, propertiesToExclude))
                {
                    right= RemoveConditions(binaryExpression.Right, parameter, propertiesToExclude);
                }
                return Expression.AndAlso(left, right);
            }

            //判断是不是一个等式。如果是的话看左边是不是属性属于要排除的属性
            if (expression is BinaryExpression ebinaryExpression && ebinaryExpression.NodeType == ExpressionType.Equal)
            {
                // 递归处理左右子节点
                if (IsPropertyCondition(expression, parameter, propertiesToExclude))
                {
                    return Expression.Constant(true);
                }
                else
                {
                    return expression;
                }
            }

            if (expression is ConstantExpression constantExpression)
            {

            }

            return expression;
        }


        /// <summary>
        /// 检查是否是要排除的属性
        /// </summary>
        /// <param name="expression">表达式要为等式</param>
        /// <param name="parameter"></param>
        /// <param name="propertiesToExclude"></param>
        /// <returns></returns>
        private static bool IsPropertyCondition(Expression expression, ParameterExpression parameter, string[] propertiesToExclude)
        {
            if (expression is BinaryExpression binary && binary.NodeType == ExpressionType.Equal)
            {
                var member = binary.Left as MemberExpression;
                if (member != null && member.Expression == parameter && propertiesToExclude.Contains(member.Member.Name))
                {
                    return true;
                }
            }

            return false;
        }


        //优化后

        /// <summary>
        /// 移除表达式中的指定等式条件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <param name="propertiesToExclude"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> RemoveEqualityConditions<T>(Expression<Func<T, bool>> expression, params string[] propertiesToExclude)
        {
            var parameter = expression.Parameters.Single();
            var body = RemoveEqualityConditions(expression.Body, parameter, propertiesToExclude);
            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        private static Expression RemoveEqualityConditions(Expression expression, ParameterExpression parameter, string[] propertiesToExclude)
        {
            if (expression is BinaryExpression binaryExpression && binaryExpression.NodeType == ExpressionType.AndAlso)
            {
                var left = RemoveEqualityConditions(binaryExpression.Left, parameter, propertiesToExclude);
                var right = RemoveEqualityConditions(binaryExpression.Right, parameter, propertiesToExclude);

                // 检查左右两边是否都是等式条件，并且是否在要排除的属性列表中
                if (IsEqualityCondition(left, parameter, propertiesToExclude) && IsEqualityCondition(right, parameter, propertiesToExclude))
                {
                    // 如果两边都是要排除的等式条件，则返回始终为真的表达式（即不改变逻辑的简化表达式）
                    return Expression.Constant(true);
                }

                return Expression.AndAlso(left, right);
            }

            else if (IsEqualityCondition(expression, parameter, propertiesToExclude))
            {
                // 如果两边都是要排除的等式条件，则返回始终为真的表达式（即不改变逻辑的简化表达式）
                return Expression.Constant(true);
            }


            return expression;
        }

        /// <summary>
        /// 判断是否为等式条件
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="parameter"></param>
        /// <param name="propertiesToExclude"></param>
        /// <returns></returns>
        private static bool IsEqualityCondition(Expression expression, ParameterExpression parameter, string[] propertiesToExclude)
        {
            if (expression is BinaryExpression binary && binary.NodeType == ExpressionType.Equal)
            {
                var member = binary.Left as MemberExpression;
                var constant = binary.Right as ConstantExpression;
                if (member != null && constant != null && member.Expression == parameter && propertiesToExclude.Contains(member.Member.Name))
                //if (member != null && constant != null && member.Expression == parameter && propertiesToExclude.Any(k => k.Key.Contains(member.Member.Name)))
                {
                    return true;
                }
            }

            return false;
        }


        #endregion

        public static Expression<Func<T, TResult>> Combine<T, TIntermediate, TResult>(
    this Expression<Func<T, TIntermediate>> first,
    Expression<Func<TIntermediate, TResult>> second)
        {
            var param = Expression.Parameter(typeof(T), "t");
            var newFirstBody = new ReplaceExpressionVisitor(first.Parameters[0], param).Visit(first.Body);
            var newSecondBody = new ReplaceExpressionVisitor(second.Parameters[0], newFirstBody).Visit(second.Body);

            return Expression.Lambda<Func<T, TResult>>(newSecondBody, param);
        }


        private class ReplaceExpressionVisitor : ExpressionVisitor
        {
            private Expression _oldValue;
            private Expression _newValue;

            public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
            {
                _oldValue = oldValue;
                _newValue = newValue;
            }

            public override Expression Visit(Expression node)
            {
                if (node == _oldValue)
                    return _newValue;
                return base.Visit(node);
            }
        }


        private static Expression<T> Combine<T>(this Expression<T> first, Expression<T> second, Func<Expression, Expression, Expression> merge)
        {
            MyExpressionVisitor visitor = new MyExpressionVisitor(first.Parameters[0]);
            Expression bodyone = visitor.Visit(first.Body);
            Expression bodytwo = visitor.Visit(second.Body);
            return Expression.Lambda<T>(merge(bodyone, bodytwo), first.Parameters[0]);
        }
        /// <summary>
        /// 条件合并，以and 的逻辑合并两个表达式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> ExpressionAnd<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Combine(second, Expression.And);
        }
        /// <summary>
        /// 条件合并，以or 的逻辑合并两个表达式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> ExpressionOr<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Combine(second, Expression.Or);
        }

        /// <summary>
        /// 查询语句，查询字段在 keywords中， 值为T 实体中与其对应的关键字包含值的数据。
        /// </summary>
        /// <typeparam name="T">实体类型 </typeparam>
        /// <param name="item">实体</param>
        /// <param name="Keywords">判断关键字</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> EntityEquel_Expression<T>(T item, List<string> Keywords)
        {
            Type entityType = typeof(T);
            Expression<Func<T, bool>> expression = null;
            ParameterExpression paramExpr = Expression.Parameter(typeof(T), "e");
            foreach (var keyWord in Keywords)
            {
                Expression<Func<T, bool>> lambda;
                string field = keyWord;//关键字
                PropertyInfo proInfo = entityType.GetProperty(field);
                var keyValue = proInfo.GetValue(item);
                MemberExpression mExpr = Expression.Property(paramExpr, field);
                ConstantExpression cExpr = Expression.Constant(keyValue, proInfo.PropertyType);
                BinaryExpression bExpr = Expression.Equal(mExpr, cExpr);
                Expression whereExpr = bExpr;
                lambda = Expression.Lambda<Func<T, bool>>(whereExpr, paramExpr);
                if (expression != null)
                {
                    expression = expression.ExpressionAnd(lambda);
                }
                else
                {
                    expression = lambda;
                }
            }
            return expression;
        }
        /// <summary>
        /// 包含查询关键字 是否包含在list中 相当于sql中的 select key in (设定的值)
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="KeyValues"> 数据判断 字段 ，string 为 实体内的某个字符，list<string> 为包含该字符的条件 </param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> In_Expression<T>(Dictionary<string, List<string>> KeyValues)
        {
            Type entityType = typeof(T);
            Expression<Func<T, bool>> expression = null;
            ParameterExpression paramExpr = Expression.Parameter(typeof(T), "e");
            foreach (var keyItem in KeyValues)
            {
                foreach (var keyValue in KeyValues[keyItem.Key])
                {
                    Expression<Func<T, bool>> lambda;
                    string field = keyItem.Key;//关键字
                    PropertyInfo proInfo = entityType.GetProperty(field);
                    MemberExpression mExpr = Expression.Property(paramExpr, field);
                    ConstantExpression cExpr = Expression.Constant(keyValue, proInfo.PropertyType);
                    BinaryExpression bExpr = Expression.Equal(mExpr, cExpr);
                    Expression whereExpr = bExpr;
                    lambda = Expression.Lambda<Func<T, bool>>(whereExpr, paramExpr);
                    if (expression != null)
                    {
                        expression = expression.ExpressionOr(lambda);
                    }
                    else
                    {
                        expression = lambda;
                    }
                }
            }
            return expression;
        }


        /*
        /// <summary>
        /// 等于
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="key">关键字</param>
        /// <param name="keyItem">关键字对应的值</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> Equel_Expression<T>(string key, object keyItem)
        {

            Type entityType = typeof(T);
            ParameterExpression paramExpr = Expression.Parameter(typeof(T), "e");
            Expression<Func<T, bool>> lambda;
            lambda = Equel128_GetExpression<T>(key, keyItem, true);
            if (lambda != null)
            {
                return lambda;
            }
            string field = key;//关键字
            PropertyInfo proInfo = entityType.GetProperty(field);
            var keyValue = keyItem;
            MemberExpression mExpr = Expression.Property(paramExpr, field);
            ConstantExpression cExpr = Expression.Constant(keyValue, proInfo.PropertyType);

            BinaryExpression bExpr = Expression.Equal(mExpr, cExpr);

            Expression whereExpr = bExpr;
            lambda = Expression.Lambda<Func<T, bool>>(whereExpr, paramExpr);
            return lambda;
        }
        */
        /// <summary>
        /// 大于
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="key">关键字</param>
        /// <param name="keyItem">关键字对应的值</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> Greaterthan_Expression<T>(string key, object keyItem)
        {
            Type entityType = typeof(T);
            ParameterExpression paramExpr = Expression.Parameter(typeof(T), "e");
            Expression<Func<T, bool>> lambda;
            string field = key;//关键字
            PropertyInfo proInfo = entityType.GetProperty(field);
            var keyValue = keyItem;
            MemberExpression mExpr = Expression.Property(paramExpr, field);
            ConstantExpression cExpr = Expression.Constant(keyValue, proInfo.PropertyType);
            BinaryExpression bExpr = Expression.GreaterThan(mExpr, cExpr);
            Expression whereExpr = bExpr;
            lambda = Expression.Lambda<Func<T, bool>>(whereExpr, paramExpr);

            return lambda;
        }
        /// <summary>
        /// 大于等于
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="key">关键字</param>
        /// <param name="keyItem">关键字对应的值</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> GreaterThanOrEqual_Expression<T>(string key, object keyItem)
        {
            Type entityType = typeof(T);
            ParameterExpression paramExpr = Expression.Parameter(typeof(T), "e");
            Expression<Func<T, bool>> lambda;
            string field = key;//关键字
            PropertyInfo proInfo = entityType.GetProperty(field);
            var keyValue = keyItem;
            MemberExpression mExpr = Expression.Property(paramExpr, field);
            ConstantExpression cExpr = Expression.Constant(keyValue, proInfo.PropertyType);
            BinaryExpression bExpr = Expression.GreaterThanOrEqual(mExpr, cExpr);
            Expression whereExpr = bExpr;
            lambda = Expression.Lambda<Func<T, bool>>(whereExpr, paramExpr);

            return lambda;
        }

        /// <summary>
        /// 小于
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="key">关键字</param>
        /// <param name="keyItem">关键字对应的值</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> LessThan_Expression<T>(string key, object keyItem)
        {
            Type entityType = typeof(T);
            ParameterExpression paramExpr = Expression.Parameter(typeof(T), "e");
            Expression<Func<T, bool>> lambda;
            string field = key;//关键字
            PropertyInfo proInfo = entityType.GetProperty(field);
            var keyValue = keyItem;
            MemberExpression mExpr = Expression.Property(paramExpr, field);
            ConstantExpression cExpr = Expression.Constant(keyValue, proInfo.PropertyType);
            BinaryExpression bExpr = Expression.LessThan(mExpr, cExpr);
            Expression whereExpr = bExpr;
            lambda = Expression.Lambda<Func<T, bool>>(whereExpr, paramExpr);
            return lambda;
        }

        /// <summary>
        /// 小于等于
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="key">关键字</param>
        /// <param name="keyItem">关键字对应的值</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> LessThanOrEqual_Expression<T>(string key, object keyItem)
        {
            Type entityType = typeof(T);
            ParameterExpression paramExpr = Expression.Parameter(typeof(T), "e");
            Expression<Func<T, bool>> lambda;
            string field = key;//关键字
            PropertyInfo proInfo = entityType.GetProperty(field);
            var keyValue = keyItem;
            MemberExpression mExpr = Expression.Property(paramExpr, field);
            ConstantExpression cExpr = Expression.Constant(keyValue, proInfo.PropertyType);
            BinaryExpression bExpr = Expression.LessThanOrEqual(mExpr, cExpr);
            Expression whereExpr = bExpr;
            lambda = Expression.Lambda<Func<T, bool>>(whereExpr, paramExpr);
            return lambda;
        }

        /*
        /// <summary>
        /// 不等于
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="key">关键字</param>
        /// <param name="keyItem">关键字对应的值</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> NotEqual_Expression<T>(string key, object keyItem)
        {
            Type entityType = typeof(T);
            ParameterExpression paramExpr = Expression.Parameter(typeof(T), "e");
            Expression<Func<T, bool>> lambda;
            lambda = Equel128_GetExpression<T>(key, keyItem, false);
            if (lambda != null)
            {
                return lambda;
            }
            string field = key;//关键字
            PropertyInfo proInfo = entityType.GetProperty(field);
            var keyValue = keyItem;
            MemberExpression mExpr = Expression.Property(paramExpr, field);
            ConstantExpression cExpr = Expression.Constant(keyValue, proInfo.PropertyType);
            BinaryExpression bExpr = Expression.NotEqual(mExpr, cExpr);
            Expression whereExpr = bExpr;
            lambda = Expression.Lambda<Func<T, bool>>(whereExpr, paramExpr);
            return lambda;
        }
        */
        #region 自定义字段
        /*
        /// <summary>
        ///判断 decimal128 格式并进行处理
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="name">关键字</param>
        /// <param name="keyItem">值</param>
        /// <param name="isEquel">是否相等 true等于 false 不等于</param>
        /// <returns></returns>
        private static Expression<Func<T, bool>> Equel128_GetExpression<T>(string name, object keyItem, bool isEquel)
        {
            Expression<Func<T, bool>> lambda = null;
            Type entityType = typeof(T);
            var param = Expression.Parameter(typeof(T), "x");
            PropertyInfo proInfo = entityType.GetProperty(name);
            var type = proInfo.PropertyType.FullName;
            try
            {
                if (type.Contains("System.Decimal") && type != "System.Decimal")//包含但是不等于相当于 为decimal? 格式。否则相当于decimal格式
                {
                    Decimal128 kk = Decimal128.Parse(keyItem.ToString());
                    BsonDecimal128 aa = new BsonDecimal128(kk);
                    ParameterExpression parameter = Expression.Parameter(typeof(T), "p");
                    MemberExpression member = Expression.PropertyOrField(parameter, name);
                    var addMethod = typeof(decimal?).GetMethod("Equals");
                    ConstantExpression constant = Expression.Constant(aa, typeof(BsonDecimal128));
                    if (isEquel)
                    {
                        lambda = Expression.Lambda<Func<T, bool>>(Expression.Call(member, addMethod, constant), parameter);
                    }
                    else
                    {
                        lambda = Expression.Lambda<Func<T, bool>>(Expression.Not(Expression.Call(member, addMethod, constant)), parameter);
                    }
                }
            }
            catch (Exception ex)
            {
                lambda = null;
            }
            return lambda;
        }
        */
        #endregion

    }


    public class ExpressionHelper
    {

        /// <summary>
        /// 取成员名称,要带Expression，FUNC只是一个委托方法。试过了。取不到名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="QueryConditions"></param>
        /// <returns></returns>
        public static List<string> ExpressionListToStringList<T>(List<Expression<Func<T, object>>> QueryConditions)
        {
            List<string> queryConditions = new List<string>();
            if (QueryConditions != null)
            {
                foreach (Expression<Func<T, object>> item in QueryConditions)
                {
                    queryConditions.Add(ExpressionToString(item));
                    /*
                   Expression newexp = item.Body;
                   if (newexp.NodeType == ExpressionType.MemberAccess)
                   {
                       if (newexp is MemberExpression member)
                       {
                           queryConditions.Add(member.Member.Name);
                       }
                   }
                   else
                   {
                       if (newexp.NodeType == ExpressionType.Convert)
                       {
                           var cexp = (newexp as UnaryExpression).Operand;
                           if (cexp is MemberExpression member)
                           {
                               queryConditions.Add(member.Member.Name);
                           }
                       }
                   }
                        */
                }

            }
            return queryConditions;
        }


        /// <summary>
        /// 取成员名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="QueryConditions"></param>
        /// <returns></returns>
        public static string ExpressionToString<T>(Expression<Func<T, object>> QueryCondition)
        {
            string Conditions = string.Empty;
            if (QueryCondition != null)
            {
                Expression newexp = QueryCondition.Body;
                if (newexp.NodeType == ExpressionType.MemberAccess)
                {
                    if (newexp is MemberExpression member)
                    {
                        Conditions = member.Member.Name;
                    }
                }
                else
                {
                    if (newexp.NodeType == ExpressionType.Convert)
                    {
                        var cexp = (newexp as UnaryExpression).Operand;
                        if (cexp is MemberExpression member)
                        {
                            Conditions = member.Member.Name;
                        }
                    }
                }
            }

            return Conditions;
        }
        


        /// <summary>
        /// 表达式转换为委托  一般用于查询条件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static Func<T, bool> ConvertToFunc<T>(Expression<Func<T, bool>> expression)
        {
            // 创建一个表达式树
            var expressionTree = expression.Body;
            //expression.Compile();

            // 编译表达式树并创建一个委托
            var delegateType = typeof(Func<T, bool>);
            //var compiled = Expression.Lambda(delegateType, expressionTree, null).Compile();
            var compiled = expression.Compile();
            // 返回编译后的委托
            return (Func<T, bool>)compiled;
        }


        /// <summary> 解析Expression，取回Member名稱及Parameter Type </summary>
        public static ExpressionMemberInfo GetMemberName(Expression exp)
        {
            switch (exp.NodeType)
            {
                case ExpressionType.Convert:
                case ExpressionType.TypeAs:
                    return GetMemberName(((UnaryExpression)exp).Operand);
                case ExpressionType.Constant:
                    return GetExpressionMemberInfo(((ConstantExpression)exp).Value.ToString());
                case ExpressionType.MemberAccess:
                    return GetExpressionMemberInfo(((MemberExpression)exp).Member.Name);
                case ExpressionType.Call:
                    return VisitMethod((MethodCallExpression)exp);
                case ExpressionType.Lambda:
                    return GetMemberName(((LambdaExpression)exp).Body);
                case ExpressionType.New:
                    return VisitNew((NewExpression)exp);
                default:
                    throw new NotSupportedException(String.Format("未處理的Expression : '{0}'", exp.NodeType));
            }
        }

        private static ExpressionMemberInfo GetExpressionMemberInfo(string memberName)
        {
            return new ExpressionMemberInfo() { MemberName = memberName };
        }

        /// <summary> 解析MethodCallExpression </summary>
        protected static ExpressionMemberInfo VisitMethod(MethodCallExpression methodExp)
        {
            return new ExpressionMemberInfo()
            {
                MemberName = methodExp.Method.Name,
                ParameterTypes = methodExp.Arguments.Select(p => p.Type)
            };
        }

        /// <summary> 解析NewExpression </summary>
        protected static ExpressionMemberInfo VisitNew(NewExpression newExp)
        {
            return new ExpressionMemberInfo()
            {
                MemberName = ".ctor",
                ParameterTypes = newExp.Arguments.Select(p => p.Type)
            };
        }

        /// <summary> 解析完成後，回傳的類別(包含Member名稱跟Type的陣列) </summary>
        public class ExpressionMemberInfo
        {
            public string MemberName { get; set; }

            public IEnumerable<Type> ParameterTypes { get; set; }
        }
    }


    public class MyExpressionVisitor : ExpressionVisitor
    {
        public ParameterExpression _Parameter { get; set; }

        public MyExpressionVisitor(ParameterExpression Parameter)
        {
            _Parameter = Parameter;
        }
        protected override Expression VisitParameter(ParameterExpression p)
        {
            return _Parameter;
        }

        public override Expression Visit(Expression node)
        {
            return base.Visit(node);//Visit会根据VisitParameter()方法返回的Expression修改这里的node变量
        }
    }
}
