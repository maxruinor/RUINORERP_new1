using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static RUINORERP.UI.Common.DuplicateFinder;

namespace RUINORERP.UI.Common
{

    public static class EnumerableExtensions
    {
        public static IEnumerable<IGrouping<object, T>> GroupByProperties<T>(
            this IEnumerable<T> source, string pkName) where T : class
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            // 创建一个用于获取所有键属性值的匿名函数
            Func<T, object> keySelector = item =>
            {
                PropertyInfo[] keyProperties = item.GetType().GetProperties()
                    .Where(prop => prop.GetCustomAttribute<SugarColumn>()?.IsIgnore == false && prop.Name != pkName)
                    .ToArray();
                var values = keyProperties.Select(prop => prop.GetValue(item, null)).ToArray();
                return values.Length == 1 ? values[0] : Tuple.Create(values);
            };

            // 使用显式类型参数调用 GroupBy
            return source.GroupBy(keySelector, new CustomSugarColumnEqualityComparer<object>(new string[] { pkName }));
        }
    }

    /*
    public static class EnumerableExtensions
    {
        public static IEnumerable<IGrouping<object, T>> GroupByProperties<T>(
            this IEnumerable<T> source, IEnumerable<string> keyPropertyNames)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keyPropertyNames == null)
                throw new ArgumentNullException(nameof(keyPropertyNames));

            var keyProperties = keyPropertyNames.Select(name => typeof(T).GetProperty(name))
                .Where(prop => prop != null)
                .ToArray();

            if (keyProperties.Length == 0)
                throw new ArgumentException("At least one property must be found.", nameof(keyPropertyNames));

            // 创建一个用于获取所有键属性值的匿名函数
            Func<T, object> keySelector = item =>
            {
                var values = keyProperties.Select(prop => prop.GetValue(item, null)).ToArray();
                return values.Length == 1 ? values[0] : Tuple.Create(values);
            };

            // 使用显式类型参数调用 GroupBy
            return source.GroupBy(keySelector);
        }
    }*/
}