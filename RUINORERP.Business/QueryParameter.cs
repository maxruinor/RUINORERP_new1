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

    /*
         /// 拉姆达表达式 追加 条件判断 Expression<Func<T, bool>>
    /// https://www.cnblogs.com/520cc/p/4094695.html
    /// https://www.cnblogs.com/fzhilong/articles/4360695.html
     */

    /// <summary>
    /// 2024-4-16 这个类要优化掉。暂时还没有处理完TODO
    /// 一个类实例就是一个字段的条件设置信息，
    /// 所有通用查询都是这个类来给出各种信息
    /// 生成的UI的默认顺序就是添加到查询条件时代码的顺序
    /// 比方采购入库时，要从采购订单引入。
    /// 这时采购订单会扩展一个查询的UI,UI筛选条件可能会有供应商，条件的个数可以统一控制 代码位于 业务控制器中 GetQueryParameters
    /// 目前这个订单 会保存成为一个集合，单行指向一个下拉对象时，就可能要扩展，子查询条件需要设置
    /// </summary>
    /// <typeparam name="M"></typeparam>
    public class QueryParameter<M>
    {
        public ApplicationContext _appContext;

        private List<string> subQueryParameter = new List<string>();

        /// <summary>
        /// 次级查询条件，这里架构不完善，暂时只方便用条件名称来搜索，不处理下拉等特殊情况
        /// </summary>
        public List<string> SubQueryParameter { get => subQueryParameter; set => subQueryParameter = value; }


        public static Expression<Func<dynamic, bool>> ConvertExpression<T>(Expression<Func<T, bool>> expold)
        {
            var param = Expression.Parameter(typeof(object), "t");
            var cast = Expression.Convert(param, typeof(T));
            var body = expold.Body.ReplaceParameter(expold.Parameters[0], cast);
            return Expression.Lambda<Func<dynamic, bool>>(body, param);
        }

        public QueryParameter()
        {

        }

        public QueryParameter(ApplicationContext appContext = null)
        {
            _appContext = appContext;
        }

        /// <summary>
        /// 没有特殊要求时
        /// </summary>
        /// <param name="queryFieldExp"></param>
        public QueryParameter(Expression<Func<M, object>> queryFieldExp)
        {
            QueryFieldExpression = queryFieldExp;
            string tableName = typeof(M).Name;

            /*
            //次級的
            //如果M是表，查可以查出这个字段是否为外键关联的，如果是则可以在这里统计处理关联表的查询条件，即子查询条件集合
            if (tableName.Contains("tb_"))
            {
                //主键是long  64位
                foreach (var field in typeof(T).GetProperties().Where(c => c.PropertyType.Name == ""))
                {
                    //获取指定类型的自定义特性
                    object[] attrs = field.GetCustomAttributes(false);
                    foreach (var attr in attrs)
                    {
                        if (attr is FKRelationAttribute)
                        {
                            FKRelationAttribute fkrattr = attr as FKRelationAttribute;

                            var conlist = _appContext.GetRequiredService<tb_CustomerVendorController<tb_CustomerVendor>>().GetQueryParameters();

                            //parameter.SubQueryParameter = new List<string>(conlist.Select(t => t.QueryField).ToList()); ;

                            break;
                            //fkrattr.FK_ValueColName, fkrattr.FKTableName
                        }
                    }
                }
            }*/


        }

        /// <summary>
        /// 如果是视图时使用，
        /// </summary>
        /// <param name="queryFieldExp"></param>
        /// <param name="RelatedTableTypeForView">这个字段来源于的表实体类型</param>
        public QueryParameter(Expression<Func<M, object>> queryFieldExp, Type RelatedTableTypeForView)
        {
            QueryFieldExpression = queryFieldExp;
            RelatedTableExpType = RelatedTableTypeForView;
            IsView = true;
        }

        private Expression<Func<M, object>> _QueryFieldExpression;

        public Type limitedExpType { get; set; }

        /// <summary>
        /// 标记是否为视图
        /// </summary>
        public bool IsView { get; set; }

        /// <summary>
        /// 相对于视图关联表的类型，限制条件也在这
        /// </summary>
        public Type RelatedTableExpType { get; set; }

        /// <summary>
        /// 查询的字段，用表达式的方式避免字段名错误，编译阶段即可发现
        /// </summary>
        private Expression<Func<M, object>> QueryFieldExpression
        {
            get { return _QueryFieldExpression; }
            set
            {
                _QueryFieldExpression = value;
                //指定到字符类型，方便使用
                QueryField = ExpressionHelper.ExpressionToString<M>(_QueryFieldExpression);
            }
        }


        /// <summary>
        /// 查询的字段
        /// </summary>
        public string QueryField { get; set; }

        /// <summary>
        /// 根据不同的字段类型。给出对应的数据信息
        /// 以这个类型为标准判断应该处理的方式
        /// 这块应该可以重构，比方 用接口定义
        /// </summary>
        public QueryFieldType QueryFieldType { get; set; }

        /// <summary>
        /// 指定了查询字段的数据类型后，就要指定具体数据
        /// </summary>
   


        /// <summary>
        /// 字段限制条件
        /// </summary>
        public Expression<Func<dynamic, bool>> FieldLimitCondition { get; set; }




        ///// <summary>
        ///// 查询视图时，视图中如类型id,不会像表一样在实体中加入关联关系。所以在这里查询条件时手动指定
        ///// </summary>
        //public Expression<Func<dynamic, string>> RelatedTableFieldForView { get; set; }


        ///// <summary>
        ///// 字段限制条件  和下面重复了。 要优化一下
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="limitexp"></param>
        ///// <returns></returns>
        //public Expression<Func<T, bool>> FieldLimitConditions<T>(Expression<Func<T, bool>> limitexp)
        //{
        //    limitedExpType = typeof(T);
        //    Expression<Func<dynamic, bool>> expnew = ConvertExpression(limitexp);
        //    FieldLimitCondition = expnew;
        //    return f => true;
        //}

        /// <summary>
        /// 设置字段绑定时的值集合的过滤条件和上面重复了。 要优化一下
        /// </summary>
        /// <typeparam name="C"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public void SetFieldLimitCondition<C>(Expression<Func<C, bool>> expression)
        {
            limitedExpType = typeof(C);
            Expression<Func<dynamic, bool>> expnew = ConvertExpression(expression);
            FieldLimitCondition = expnew;
        }





    }

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
