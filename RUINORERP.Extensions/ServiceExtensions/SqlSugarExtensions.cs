using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Extensions.ServiceExtensions
{
    public static class SqlSugarExtensions
    {
        // 添加对非泛型查询的支持
        //public static ISugarQueryable Includes(
        //    this ISugarQueryable queryable,
        //    Type mainType,
        //    string navigationProperty)
        //{
        //    var method = typeof(SqlSugarExtensions).GetMethod(
        //        nameof(GenericIncludes),
        //        BindingFlags.Static | BindingFlags.Public);

        //    var genericMethod = method.MakeGenericMethod(mainType);
        //    return (ISugarQueryable)genericMethod.Invoke(
        //        null,
        //        new object[] { queryable, navigationProperty });
        //}

        public static ISugarQueryable<TMain> GenericIncludes<TMain>(
            this ISugarQueryable<TMain> queryable,
            string navigationProperty)
            where TMain : class
        {
            // 获取导航属性类型
            var propertyInfo = typeof(TMain).GetProperty(navigationProperty);
            if (propertyInfo == null) return queryable;

            // 创建表达式
            var param = Expression.Parameter(typeof(TMain), "x");
            var property = Expression.Property(param, propertyInfo);

            // 动态调用 Includes
            var methodCall = Expression.Call(
                typeof(SqlSugarExtensions),
                nameof(GenericIncludesImpl),
                new[] { typeof(TMain), propertyInfo.PropertyType.GenericTypeArguments[0] },
                Expression.Constant(queryable),
                Expression.Lambda(property, param));

            return Expression.Lambda<Func<ISugarQueryable<TMain>>>(methodCall).Compile().Invoke();
        }

        private static ISugarQueryable<TMain> GenericIncludesImpl<TMain, TDetail>(
            ISugarQueryable<TMain> queryable,
            Expression<Func<TMain, object>> expression)
            where TMain : class
            where TDetail : class
        {
            return queryable.Includes(expression);
        }
    }
}
