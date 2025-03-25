using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Common.Helper
{
    public class RuinorExpressionHelper
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
        /// 取成员名称,要带Expression，FUNC只是一个委托方法。试过了。取不到名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="QueryConditions"></param>
        /// <returns></returns>
        public static HashSet<string> ExpressionListToHashSet<T>(List<Expression<Func<T, object>>> QueryConditions)
        {
            HashSet<string> queryConditions = new HashSet<string>();
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
}
