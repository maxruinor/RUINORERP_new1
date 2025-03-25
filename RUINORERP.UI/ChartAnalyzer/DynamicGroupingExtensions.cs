using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.ChartAnalyzer
{
 

    //public static class QueryableExtensions1
    //{
    //    public static IQueryable<T> ApplyFilters<T>(this IQueryable<T> query, IEnumerable<QueryFilter> filters)
    //    {
    //        foreach (var filter in filters)
    //        {
    //            query = filter.Operator switch
    //            {
    //                ">" => query.WhereDynamic($"{filter.Field} > @0", filter.Value),
    //                "<" => query.WhereDynamic($"{filter.Field} < @0", filter.Value),
    //                ">=" => query.WhereDynamic($"{filter.Field} >= @0", filter.Value),
    //                "<=" => query.WhereDynamic($"{filter.Field} <= @0", filter.Value),
    //                "==" => query.WhereDynamic($"{filter.Field} == @0", filter.Value),
    //                _ => query
    //            };
    //        }
    //        return query;
    //    }
    //}

    public static class DynamicGroupingExtensions
    {
       
        public static IQueryable<IGrouping<DynamicGroupKey, T>> GroupByDynamic<T>(
            this IQueryable<T> source,
            IEnumerable<string> groupProperties)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var keyExpr = BuildGroupKeyExpression(parameter, groupProperties.ToArray());

            var lambda = Expression.Lambda<Func<T, DynamicGroupKey>>(
                keyExpr,
                parameter
            );

            return source.GroupBy(lambda);
        }

        private static Expression BuildGroupKeyExpression(
            ParameterExpression parameter,
            params string[] properties)
        {
            // 获取DynamicGroupKey的构造函数
            var ctor = typeof(DynamicGroupKey).GetConstructor(new[] { typeof(object[]) });

            // 创建对象数组的初始化表达式
            var propertyExpressions = properties
                .Select(prop => BuildPropertyExpression(parameter, prop))
                .ToArray();

            var arrayExpr = Expression.NewArrayInit(
                typeof(object),
                propertyExpressions.Select(ConvertToObject)
            );

            // 创建new DynamicGroupKey(objects)表达式
            return Expression.New(ctor, arrayExpr);
        }

        private static Expression ConvertToObject(Expression expr)
        {
            if (expr.Type.IsValueType)
                return Expression.Convert(expr, typeof(object));
            return expr;
        }

        private static Expression BuildPropertyExpression(
            Expression parameter,
            string propertyPath)
        {
            return propertyPath.Split('.')
                .Aggregate(parameter, (expr, member) =>
                    Expression.PropertyOrField(expr, member)
                );
        }
    }

    /// <summary>
    /// SqlSugar 动态分组扩展方法
    /// </summary>
    public static class SqlSugarDynamicGroupingExtensions
    {
        /// <summary>
        /// SqlSugar 动态分组扩展方法
        /// </summary>
        public static ISugarQueryable<T> GroupByDynamic<T>(
            this ISugarQueryable<T> source,
            params string[] groupProperties)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (groupProperties == null || !groupProperties.Any())
                throw new ArgumentException("至少需要一个分组属性", nameof(groupProperties));

            // 获取实体元数据
            var entityInfo = source.Context.EntityMaintenance.GetEntityInfo<T>();

            // 构建 GroupByModel 列表
            var groupByModels = groupProperties.Select(prop =>
            {
                var columnName = GetColumnName(entityInfo, prop);
                return new GroupByModel
                {
                    FieldName = columnName
                };
            }).ToList();

            return source.GroupBy(groupByModels);
        }

        /// <summary>
        /// 动态分组扩展方法（支持导航属性）
        /// </summary>
        public static ISugarQueryable<T> GroupByDynamic<T>(
            this ISugarQueryable<T> source,
            IEnumerable<string> groupProperties)
        {
            return source.GroupByDynamic(groupProperties.ToArray());
        }

        /// <summary>
        /// 获取属性对应的数据库列名
        /// </summary>
        private static string GetColumnName(EntityInfo entityInfo, string propertyPath)
        {
            var currentType = entityInfo.Type;
            var columnName = new StringBuilder();

            foreach (var propName in propertyPath.Split('.'))
            {
                var propInfo = currentType.GetProperty(propName,
                    BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (propInfo == null) break;

                var navProp = entityInfo.Columns.FirstOrDefault(c =>
                    c.PropertyName.Equals(propName, StringComparison.OrdinalIgnoreCase));
                if (navProp != null)
                {
                    columnName.Append(navProp.DbColumnName);
                    currentType = propInfo.PropertyType;
                }
                else
                {
                    columnName.Append(propName); // 找不到映射时回退到属性名
                    break;
                }

                columnName.Append('_'); // 导航属性分隔符
            }

            return columnName.ToString().TrimEnd('_');
        }

        /// <summary>
        /// 添加默认聚合操作
        /// </summary>
        public static ISugarQueryable<T> WithDefaultAggregates<T>(this ISugarQueryable<T> query)
        {
            return query.Select("COUNT(1) AS Count");
        }
    }

    public class DynamicGroupKey : IEquatable<DynamicGroupKey>
    {
        public object[] Keys { get; }

        public DynamicGroupKey(params object[] keys)
        {
            Keys = keys;
        }

        public bool Equals(DynamicGroupKey other)
        {
            if (other == null) return false;
            if (Keys.Length != other.Keys.Length) return false;

            return Keys.Zip(other.Keys, (a, b) => Equals(a, b))
                       .All(result => result);
        }

        public override bool Equals(object obj) => Equals(obj as DynamicGroupKey);

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                foreach (var key in Keys)
                {
                    hash = hash * 23 + (key?.GetHashCode() ?? 0);
                }
                return hash;
            }
        }

        public override string ToString() =>
            $"Key({string.Join(", ", Keys.Select(k => k?.ToString() ?? "null"))})";
    }



}
