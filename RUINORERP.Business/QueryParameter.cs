using Microsoft.Extensions.Logging;
using RUINORERP.Common.CustomAttribute;
using RUINORERP.Common.Extensions;
using RUINORERP.Common.Helper;
using RUINORERP.Global;
using RUINORERP.Global.CustomAttribute;
using RUINORERP.Model;
using RUINORERP.Model.Base;
using RUINORERP.Model.Context;
using RUINORERP.Repository.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Business
{



    /// <summary>
    /// 用反射 将type转换为T 
    /// </summary>
    public class ExpConverter
    {
        public Expression<Func<T, bool>> ExportToT<T>(Expression<Func<dynamic, bool>> expression)
        {
            var expCondition = ExpressionConverter.Convert<T>(expression);
            return expCondition;
        }
        public object ExportByClassNameToT(Type t, Expression<Func<dynamic, bool>> expCondition)
        {
            MethodInfo mi = this.GetType().GetMethod("ExportToT").MakeGenericMethod(new Type[] { t });
            object[] args1 = new object[1] { expCondition };
            return mi.Invoke(this, args1);
        }

        //=======================================


        public Expression<Func<dynamic, bool>> ExportToDyn<T>(Expression<Func<T, bool>> expression)
        {
            var expCondition = ExpressionConverter.ConvertToDynamic<T>(expression);
            return expCondition;
        }
        public object ExportByClassNameToDyn<R>(Type t, Expression<Func<R, bool>> expCondition)
        {
            MethodInfo mi = this.GetType().GetMethod("ExportToDyn").MakeGenericMethod(new Type[] { t });
            object[] args1 = new object[1] { expCondition };
            return mi.Invoke(this, args1);
        }

        //=================================



        // 定义一个辅助函数，用于进行类型转换
        public LambdaExpression ConvertToLambdaExpression<T>(Expression<Func<T, bool>> expold)
        {
            if (expold==null)
            {
                return null;
            }
            // 创建一个新的参数表达式，类型为 T
            var parameterExpression = Expression.Parameter(typeof(T), "t");

            // 将原始表达式中的参数替换为新的参数表达式
            var newBody = expold.Body.ReplaceParameter(expold.Parameters[0], parameterExpression);

            // 假设我们有一个用于示例的委托类型 Func<string, bool>
            Type delegateType = typeof(Func<T, bool>);

            //var expnewnew=  LambdaExpression lambda = Expression.Lambda(delegateType, body, paramExpr);
            var expNew = Expression.Lambda(delegateType, newBody, parameterExpression);

            return expNew;
        }


        public Expression<Func<T, bool>> ConvertToFunc<T>(LambdaExpression lambda)
        {
            // 检查输入的 LambdaExpression 是否为预期的参数类型
            if (lambda.Parameters.Count != 1 || lambda.Parameters[0].Type != typeof(T))
                throw new ArgumentException("LambdaExpression 参数类型不匹配");

            // 创建表达式对象
            var parameter = Expression.Parameter(typeof(T));
            var body = lambda.Body.ReplaceParameter(lambda.Parameters[0], parameter);

            // 返回 Expression<Func<T, bool>>
            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        public object ConvertToFuncByClassName(Type t, LambdaExpression lambda)
        {
            if (t==null || lambda==null)
            {
                return true;
            }
            MethodInfo mi = this.GetType().GetMethod("ConvertToFunc").MakeGenericMethod(new Type[] { t });
            object[] args1 = new object[1] { lambda };
            return mi.Invoke(this, args1);
        }


        // 示例用法
        // Expression<Func<int, bool>> intExpression = x => x > 5;
        // Expression<Func<dynamic, bool>> dynamicExpression = Convert(intExpression);

        // 可以使用动态表达式进行操作



    }

    public static class ExpressionConverter
    {

        public static Expression<Func<T, bool>> Convert<T>(Expression<Func<dynamic, bool>> expold)
        {
            if (expold == null)
            {
                throw new ArgumentNullException(nameof(expold));
            }

            // 创建一个新的参数表达式，类型为 T
            var parameterExpression = Expression.Parameter(typeof(T), "t");

            // 将原始表达式中的参数替换为新的参数表达式
            var newBody = expold.Body.ReplaceParameter(expold.Parameters[0], parameterExpression);

            // 创建一个新的表达式树，类型为 Expression<Func<T, bool>>
            var expnew = Expression.Lambda<Func<T, bool>>(newBody, parameterExpression);

            return expnew;
        }

        public static Expression<Func<dynamic, bool>> ConvertToDynamic<T>(Expression<Func<T, bool>> expold)
        {
            if (expold == null)
            {
                throw new ArgumentNullException(nameof(expold));
            }

            // 创建一个新的参数表达式，类型为 T
            var parameterExpression = Expression.Parameter(typeof(T), "t");

            // 将原始表达式中的参数替换为新的参数表达式
            var newBody = expold.Body.ReplaceParameter(expold.Parameters[0], parameterExpression);

            // 创建一个新的表达式树，类型为 Expression<Func<T, bool>>
            var expnew = Expression.Lambda<Func<dynamic, bool>>(newBody, parameterExpression);


            return expnew;
        }

    }

    public static class ExpressionExtensions
    {
        public static Expression ReplaceParameter(this Expression expression, ParameterExpression source, Expression target)
        {
            return new ParameterReplacer { Source = source, Target = target }.Visit(expression);
        }
    }

    public class ParameterReplacer : ExpressionVisitor
    {
        public ParameterExpression Source;
        public Expression Target;

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return node == Source ? Target : base.VisitParameter(node);
        }
    }

    #region 数据处理块

    /// <summary>
    /// 查询字段的数据接口
    /// </summary>
    /// <typeparam name="M"></typeparam>
    [NoWantIOCAttribute]
    public interface IQueryFieldData
    {

    }


    /*
    public class QueryFieldEnumData : IQueryFieldData
    {
        public Type EnumType { get; set; }


        //private Expression<Func<M, int?>> _expEnumValueColName;

        ///// <summary>
        ///// 枚举在实体中的字段名，通过表达设置
        ///// 类型匹配上调用方法中的类型即可
        ///// </summary>
        //public Expression<Func<M, int?>> expEnumValueColName
        //{
        //    get { return _expEnumValueColName; }
        //    set
        //    {
        //        _expEnumValueColName = value;
        //        //指定到字符类型，方便使用
        //        MemberInfo minfo = _expEnumValueColName.GetMemberInfo();
        //        string key = minfo.Name;
        //        EnumValueColName = key;
        //    }
        //}


        /// <summary>
        /// 枚举在实体中的字段名，通过表达设置
        /// </summary>
        public string EnumValueColName { get; set; }


        /// <summary>
        /// 枚举在实体中的字段名，通过表达设置
        /// </summary>
        //public string EnumDisplayColName { get; set; }


        /// <summary>
        /// 是否添加【请选择】下拉结果
        /// </summary>
        public bool AddSelectItem { get; set; } = false;

        public List<EnumEntityMember> BindDataSource { get; set; }

    }   */
    #endregion

}
