using RUINORERP.Model.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.Common.Extensions
{

    /// <summary>
    /// 仅作为调用入口，内部产生 IEnumFilter
    /// </summary>
    public readonly struct EnumFilter<TEnum> where TEnum : Enum
    {
        internal readonly IEnumFilter<TEnum> _impl;
        public IEnumFilter<TEnum> Impl => _impl;   // 公开只读属性

        private EnumFilter(IEnumFilter<TEnum> impl) => _impl = impl;

        /// <summary>
        /// 显式列表（排除）
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static EnumFilter<TEnum> Exclude(params TEnum[] values) =>
            new(new ListEnumFilter<TEnum>(values, true));


        /// <summary>
        /// 仅保留 
        /// </summary>
        public static EnumFilter<TEnum> IncludeOnly(params TEnum[] values) =>
            new(new ListEnumFilter<TEnum>(values, false));

        /// <summary>
        /// 范围过滤（最小值 ≤ value ≤ 最大值）
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static EnumFilter<TEnum> Range(TEnum min, TEnum max) =>
            new(new RangeEnumFilter<TEnum>(min, max));

        /*—— 任意表达式 ——*/
        public static EnumFilter<TEnum> Where(Func<TEnum, bool> predicate) =>
            new(new CompositeEnumFilter<TEnum>(predicate));

        /// <summary>
        /// 创建一个大于指定枚举值的过滤器。
        /// </summary>
        /// <typeparam name="TEnum">枚举类型。</typeparam>
        /// <param name="value">要比较的枚举值。</param>
        /// <returns>返回一个枚举过滤器实例。</returns>
        public static EnumFilter<TEnum> GreaterThan(TEnum value) =>
    new(new GreaterThanEnumFilter<TEnum>(value));


        /// <summary>
        /// 创建一个小于指定枚举值的过滤器。
        /// </summary>
        /// <typeparam name="TEnum">枚举类型。</typeparam>
        /// <param name="value">要比较的枚举值。</param>
        /// <returns>返回一个枚举过滤器实例。</returns>
        public static EnumFilter<TEnum> LessThan(TEnum value) =>
            new(new LessThanEnumFilter<TEnum>(value));

        /*—— 不过滤 ——*/
        public static EnumFilter<TEnum> None => default;
    }


    /// <summary>
    /// 所有枚举过滤器的统一契约
    /// </summary>
    public interface IEnumFilter<TEnum> where TEnum : Enum
    {
        /// <summary>
        /// 返回 true 表示该枚举值应被保留
        /// </summary>
        bool Match(TEnum value);
    }

    /// <summary>
    /// 非泛型的枚举过滤器，专门用于运行期
    /// </summary>
    public interface IRuntimeEnumFilter
    {
        /// <summary>
        /// 判断某个枚举值（object）是否保留
        /// </summary>
        bool Match(object value);
    }


    /// <summary>
    /// 显式列表（保留或排除）
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    public sealed class ListEnumFilter<TEnum> : IEnumFilter<TEnum>, IRuntimeEnumFilter where TEnum : Enum
    {
        private readonly HashSet<TEnum> _set;
        private readonly bool _exclude;

        public ListEnumFilter(IEnumerable<TEnum> values, bool exclude)
        {
            _set = new HashSet<TEnum>(values);
            _exclude = exclude;
        }

        public bool Match(TEnum value) => _exclude ? !_set.Contains(value) : _set.Contains(value);

        bool IRuntimeEnumFilter.Match(object value) =>
       value is TEnum v && Match(v);
    }

    /// <summary>
    /// 范围过滤（最小值 ≤ value ≤ 最大值）
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    public sealed class RangeEnumFilter<TEnum> : IEnumFilter<TEnum>, IRuntimeEnumFilter where TEnum : Enum
    {
        private readonly TEnum _min;
        private readonly TEnum _max;
        private readonly IComparer<TEnum> _cmp;

        public RangeEnumFilter(TEnum min, TEnum max)
        {
            _min = min;
            _max = max;
            _cmp = Comparer<TEnum>.Create((a, b) =>
            {
                // 统一用底层整型比较
                var ua = Convert.ToInt64(a);
                var ub = Convert.ToInt64(b);
                return ua.CompareTo(ub);
            });
        }

        public bool Match(TEnum value) =>
            _cmp.Compare(value, _min) >= 0 && _cmp.Compare(value, _max) <= 0;

        bool IRuntimeEnumFilter.Match(object value) =>
      value is TEnum v && Match(v);


    }

    /// <summary>
    /// 大于指定值（不含等于）
    /// </summary>
    public sealed class GreaterThanEnumFilter<TEnum> : IEnumFilter<TEnum>, IRuntimeEnumFilter  where TEnum : Enum
    {
        private readonly TEnum _threshold;
        private readonly IComparer<TEnum> _cmp = Comparer<TEnum>.Create((a, b) =>
            Convert.ToInt64(a).CompareTo(Convert.ToInt64(b)));

        public GreaterThanEnumFilter(TEnum threshold) => _threshold = threshold;

        public bool Match(TEnum value) => _cmp.Compare(value, _threshold) > 0;

        bool IRuntimeEnumFilter.Match(object value) =>
       value is TEnum v && Match(v);

    }

    /// <summary>
    /// 小于指定值（不含等于）
    /// </summary>
    public sealed class LessThanEnumFilter<TEnum> : IEnumFilter<TEnum>, IRuntimeEnumFilter where TEnum : Enum
    {
        private readonly TEnum _threshold;
        private readonly IComparer<TEnum> _cmp = Comparer<TEnum>.Create((a, b) =>
            Convert.ToInt64(a).CompareTo(Convert.ToInt64(b)));

        public LessThanEnumFilter(TEnum threshold) => _threshold = threshold;

        public bool Match(TEnum value) => _cmp.Compare(value, _threshold) < 0;

        bool IRuntimeEnumFilter.Match(object value) =>
       value is TEnum v && Match(v);

    }


    /// <summary>
    /// 组合过滤（与/或/非）（可选，预留）
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    public sealed class CompositeEnumFilter<TEnum> : IEnumFilter<TEnum>, IRuntimeEnumFilter where TEnum : Enum
    {
        private readonly Func<TEnum, bool> _predicate;
        public CompositeEnumFilter(Func<TEnum, bool> predicate) => _predicate = predicate;
        public bool Match(TEnum value) => _predicate(value);

        bool IRuntimeEnumFilter.Match(object value) =>
       value is TEnum v && Match(v);
    }


}
