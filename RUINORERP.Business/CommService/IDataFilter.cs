using RUINORERP.Common.CustomAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Business.CommService
{
    /// <summary>
    /// 运行时统一过滤接口（给缓存里的任意实体用）
    /// </summary>
    [NoWantIOC]
    public interface IDataFilter
    {
        bool Match(object entity);
    }

    /// <summary>
    /// 泛型强类型接口
    /// </summary>
    [NoWantIOC]
    public interface IDataFilter<T> : IDataFilter where T : class
    {
        bool Match(T entity);
    }

    /// <summary>
    /// 列表保留/排除
    /// </summary>
    [NoWantIOC]
    public sealed class ListDataFilter<T> : IDataFilter<T> where T : class
    {
        private readonly HashSet<T> _set;
        private readonly bool _exclude;
        public ListDataFilter(IEnumerable<T> list, bool exclude)
        {
            _set = new HashSet<T>(list);
            _exclude = exclude;
        }
        public bool Match(T entity) => _exclude ? !_set.Contains(entity) : _set.Contains(entity);
        bool IDataFilter.Match(object entity) => entity is T t && Match(t);
    }

    /// <summary>
    /// 任意表达式
    /// </summary>
    [NoWantIOC]
    public sealed class ExpressionDataFilter<T> : IDataFilter<T> where T : class
    {
        private readonly Func<T, bool> _predicate;
        public ExpressionDataFilter(Func<T, bool> predicate) => _predicate = predicate;
        public bool Match(T entity) => _predicate(entity);
        bool IDataFilter.Match(object entity) => entity is T t && Match(t);
    }

    /* 如有需要可再加 RangeDataFilter / GreaterThanDataFilter 等 */
    [NoWantIOC]
    public readonly struct DataFilter<T> where T : class
    {
        public readonly IDataFilter<T> Impl;
        private DataFilter(IDataFilter<T> impl) => Impl = impl;

        public static DataFilter<T> Where(Func<T, bool> predicate) =>
            new(new ExpressionDataFilter<T>(predicate));

        public static DataFilter<T> Include(params T[] rows) =>
            new(new ListDataFilter<T>(rows, false));

        public static DataFilter<T> Exclude(params T[] rows) =>
            new(new ListDataFilter<T>(rows, true));

        public static DataFilter<T> None => default;
    }

}
