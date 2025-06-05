using RUINORERP.Common.Extensions;
using RUINORERP.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;

namespace RUINORERP.Common.CollectionExtension
{
    //winform dataGridView 排序
    //用法 直接用 BindingSortCollection<>  代替 List<>
    [System.ComponentModel.DesignerCategory("Code")]
    public class BindingSortCollection<T> : BindingList<T>, System.Collections.Generic.IList<T>, IBindingListView
    {
        private bool isSorted;
        private PropertyDescriptor sortProperty;
        private ListSortDirection sortDirection;
        private string filterString = "";
        private List<T> originalItems = new List<T>();

        private bool _suppressListChangedEvents;
        private readonly SynchronizationContext _syncContext;


        public BindingSortCollection() : base(new List<T>())
        {
            // 捕获当前同步上下文（通常是UI线程）
            _syncContext = SynchronizationContext.Current;
            originalItems = new List<T>(this.Items);
        }

        public BindingSortCollection(IEnumerable<T> list) : base(list.ToList())
        {
            // 捕获当前同步上下文（通常是UI线程）
            _syncContext = SynchronizationContext.Current;
            originalItems = new List<T>(this.Items);
        }

        /// <summary>
        /// 获取一个值，指示列表是否已排序
        /// </summary>
        protected override bool IsSortedCore
        {
            get { return isSorted; }
        }

        /// <summary>
        /// 获取一个值，指示列表是否支持排序
        /// </summary>
        protected override bool SupportsSortingCore
        {
            get { return true; }
        }

        /// <summary>
        /// 获取一个只，指定类别排序方向
        /// </summary>
        protected override ListSortDirection SortDirectionCore
        {
            get { return sortDirection; }
        }

        /// <summary>
        /// 获取排序属性说明符
        /// </summary>
        protected override PropertyDescriptor SortPropertyCore
        {
            get { return sortProperty; }
        }

        protected override bool SupportsSearchingCore
        {
            get { return true; }
        }

        public void AddRange(List<T> list)
        {
            originalItems.AddRange(list);
            foreach (var item in list)
            {
                this.Add(item);
            }
        }

        public void AddRange(T[] list)
        {
            originalItems.AddRange(list);
            foreach (var item in list)
            {
                this.Add(item);
            }
        }



        /// <summary>
        ///自定义排序操作
        /// </summary>
        /// <param name="property"></param>
        /// <param name="direction"></param>
        protected override void ApplySortCore(PropertyDescriptor property, ListSortDirection direction)
        {
            List<T> items = this.Items as List<T>;

            if (items != null)
            {
                ObjectPropertyCompare<T> pc = new ObjectPropertyCompare<T>(property, direction);
                items.Sort(pc);
                isSorted = true;
            }
            else
            {
                isSorted = false;
            }

            sortProperty = property;
            sortDirection = direction;

            this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }

        /// <summary>
        /// 移除默认实现的排序
        /// </summary>
        protected override void RemoveSortCore()
        {
            isSorted = false;
            this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }
        protected override void OnListChanged(ListChangedEventArgs e)
        {
            if (!_suppressListChangedEvents)
            {
                // 确保在UI线程触发事件
                if (_syncContext != null && _syncContext != SynchronizationContext.Current)
                {
                    _syncContext.Send(_ => base.OnListChanged(e), null);
                }
                else
                {
                    base.OnListChanged(e);
                }
            }
        }
        /// <summary>
        /// 用法   PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(new RelationResult());
        ///        PropertyDescriptor property = properties.Find("lastMessageId", false);
        /// </summary>
        /// <param name="property"></param>
        /// <param name="direction"></param>
        public void Sort(PropertyDescriptor property, ListSortDirection direction)
        {
            this.ApplySortCore(property, direction);
        }
        // 实现 IBindingListView 接口
        public void ApplySort(ListSortDescriptionCollection sorts)
        {
            throw new System.NotImplementedException();
        }

        public string Filter
        {
            get => filterString;
            set
            {
                if (filterString != value)
                {
                    filterString = value;
                    ApplyFilter();
                }
            }
        }

        public void RemoveFilter()
        {
            Filter = string.Empty;
        }

        public ListSortDescriptionCollection SortDescriptions { get; } = null;
        public bool SupportsAdvancedSorting => false;
        public bool SupportsFiltering => true;

        // 应用过滤
        private void ApplyFilter_old()
        {
            if (string.IsNullOrWhiteSpace(filterString))
            {
                // 清除过滤
                this.ClearItems();
                foreach (var item in originalItems)
                {
                    this.Add(item);
                }
            }
            else
            {
                // 使用 LINQ 动态过滤
                //var filtered = originalItems.AsQueryable().Where(filterString).ToList();
                // 使用安全的过滤方法
                var filtered = FilterItems(originalItems, filterString);

                this.ClearItems();
                foreach (var item in filtered)
                {
                    this.Add(item);
                }
            }

            // 重新应用之前的排序
            if (isSorted && sortProperty != null)
            {
                ApplySortCore(sortProperty, sortDirection);
            }

            OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        }
        private void ApplyFilter_old1()
        {
            try
            {
                // 临时禁用变更通知
                _suppressListChangedEvents = true;

                // 清除当前项但不触发UI更新
                base.ClearItems();

                IEnumerable<T> filteredItems = originalItems;

                if (!string.IsNullOrWhiteSpace(filterString))
                {
                    try
                    {
                        // 使用安全的过滤方法
                        filteredItems = FilterItems(originalItems, filterString);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Filter error: {ex.Message}");
                        // 出错时使用原始集合
                        filteredItems = originalItems;
                    }
                }

                // 批量添加项
                foreach (var item in filteredItems)
                {
                    base.Add(item);
                }

                // 重新应用之前的排序
                if (isSorted && sortProperty != null)
                {
                    ApplySortCore(sortProperty, sortDirection);
                }
            }
            finally
            {
                _suppressListChangedEvents = false;
                // 发送一次重置通知
                OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
            }
        }

        private void ApplyFilter()
        {
            try
            {
                _suppressListChangedEvents = true;
                base.ClearItems();

                IEnumerable<T> filteredItems = originalItems;

                var filtered = string.IsNullOrWhiteSpace(filterString)
                    ? originalItems
                    : FilterItems(originalItems, filterString);

                foreach (var item in filtered)
                {
                    base.Add(item);
                }

                if (isSorted && sortProperty != null)
                {
                    ApplySortCore(sortProperty, sortDirection);
                }
            }
            finally
            {
                _suppressListChangedEvents = false;
                OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
            }
        }

        #region

        private List<T> FilterItems_old(List<T> items, string filter)
        {
            try
            {
                return items.AsQueryable().Where(filter).ToList();
            }
            catch
            {
                // 备用过滤方案
                return items.Where(item =>
                {
                    try
                    {
                        var param = Expression.Parameter(typeof(T), "x");
                        var lambda = DynamicExpressionParserBinding.ParseLambda<T>(new[] { param }, filter);
                        var func = lambda.Compile().DynamicInvoke(item);
                        return func is bool result && result;
                    }
                    catch
                    {
                        return false;
                    }
                }).ToList();
            }
        }

        private List<T> FilterItems(List<T> items, string filter)
        {
            //try
            //{
            //    // 使用LINQ动态过滤
            //    return items.AsQueryable().Where(filter).ToList();
            //}
            //catch
            //{
            //    // 备用过滤方案：手动编译表达式
            //    var param = Expression.Parameter(typeof(T), "x");
            //    var lambda = DynamicExpressionParserBinding.ParseLambda<T>(new[] { param }, filter);
            //    var func = (Func<T, bool>)lambda.Compile();

            //    return items.Where(func).ToList();
            //}

            // 备用过滤方案：手动编译表达式
            //var param = Expression.Parameter(typeof(T), "x");
            //var lambda = DynamicExpressionParserBinding.ParseLambda<T>(new[] { param }, filter);
            //var func = (Func<T, bool>)lambda.Compile();


            var filterexp = FilterParser.Parse<T>(filter);
            return items.AsQueryable().Where(filterexp).ToList();

        }

        protected override void InsertItem(int index, T item)
        {

            // 确保在UI线程执行
            if (_syncContext != null && _syncContext != SynchronizationContext.Current)
            {
                _syncContext.Send(_ =>
                {
                    if (!originalItems.Contains(item)) originalItems.Add(item);
                    if (ShouldInclude(item)) base.InsertItem(index, item);
                }, null);
            }
            else
            {
                if (!originalItems.Contains(item)) originalItems.Add(item);
                if (ShouldInclude(item)) base.InsertItem(index, item);
            }

            //originalItems.Add(item);  // 保持原始集合同步
            //if (string.IsNullOrWhiteSpace(filterString) ||
            //    ShouldIncludeInFilter(item))
            //{
            //    base.InsertItem(index, item);
            //}
        }

        //protected override void RemoveItem(int index)
        //{
        //    var item = this[index];
        //    originalItems.Remove(item);  // 保持原始集合同步
        //    base.RemoveItem(index);
        //}

        //protected override void ClearItems()
        //{
        //    originalItems.Clear();  // 清空原始集合
        //    base.ClearItems();
        //}
        protected override void RemoveItem(int index)
        {
            if (index < 0 || index >= Count) return;

            var item = this[index];
            originalItems.Remove(item);

            if (_syncContext != null && _syncContext != SynchronizationContext.Current)
            {
                _syncContext.Send(_ => base.RemoveItem(index), null);
            }
            else
            {
                base.RemoveItem(index);
            }

        }

        protected override void ClearItems()
        {
            originalItems.Clear();

            if (_syncContext != null && _syncContext != SynchronizationContext.Current)
            {
                _syncContext.Send(_ => base.ClearItems(), null);
            }
            else
            {
                base.ClearItems();
            }
        }
        //protected override void SetItem(int index, T item)
        //{
        //    var oldItem = this[index];
        //    originalItems.Remove(oldItem);
        //    originalItems.Add(item);  // 更新原始集合

        //    base.SetItem(index, item);
        //}
        protected override void SetItem(int index, T item)
        {
            if (index < 0 || index >= Count) return;
            var oldItem = this[index];

            if (_syncContext != null && _syncContext != SynchronizationContext.Current)
            {
                _syncContext.Send(_ =>
                {
                    originalItems.Remove(oldItem);

                    if (!originalItems.Contains(item)) originalItems.Add(item);

                    if (ShouldIncludeInFilter(item))
                        base.SetItem(index, item);
                    else
                        base.RemoveItem(index);
                }, null);
            }
            else
            {
                originalItems.Remove(oldItem);
                if (!originalItems.Contains(item)) originalItems.Add(item);

                if (ShouldIncludeInFilter(item))
                    base.SetItem(index, item);
                else
                    base.RemoveItem(index);
            }
        }
        //public void AddRange(IEnumerable<T> collection)
        //{
        //    foreach (var item in collection)
        //    {
        //        originalItems.Add(item);
        //        if (string.IsNullOrWhiteSpace(filterString) ||
        //            ShouldIncludeInFilter(item))
        //        {
        //            Add(item);
        //        }
        //    }
        //}
        private bool ShouldInclude(T item)
        {
            return string.IsNullOrWhiteSpace(filterString) ||
                   ShouldIncludeInFilter(item);
        }
        public void AddRange(IEnumerable<T> collection)
        {
            try
            {
                _suppressListChangedEvents = true;
                foreach (var item in collection)
                {
                    if (!originalItems.Contains(item))
                        originalItems.Add(item);

                    if (ShouldInclude(item))
                        base.Add(item);
                }
            }
            finally
            {
                _suppressListChangedEvents = false;
                OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
            }
        }

        private bool ShouldIncludeInFilter(T item)
        {
            if (string.IsNullOrWhiteSpace(filterString))
                return true;

            try
            {
                var param = Expression.Parameter(typeof(T), "x");
                var expr = DynamicExpressionParserBinding.ParseLambda<T>(
                    new[] { param }, filterString);
                // 编译过滤表达式并执行
                var func = (Func<T, bool>)expr.Compile();
                return func(item);
            }
            catch
            {
                return false;
            }
        }


        #endregion



    }



    #region 过滤 


    // 动态 LINQ 过滤辅助类
    public static class DynamicLinqExtensions
    {

        /// <summary>
        /// 手动构建
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IQueryable<T> Where11<T>(this IQueryable<T> source, string predicate)
        {
            if (string.IsNullOrEmpty(predicate)) return source;
            var param = Expression.Parameter(typeof(T), "x");
            var lambda = DynamicExpressionParserBinding.ParseLambda<T>(new[] { param }, predicate);
            var predicateExpr = Expression.Lambda<Func<T, bool>>(lambda.Body, lambda.Parameters);

            return Queryable.Where(source, predicateExpr);

            var parameter = Expression.Parameter(typeof(T), "x");
            var expression = DynamicExpressionParserBinding.ParseLambda<T>(new[] { parameter }, predicate);
            //return source.Provider.CreateQuery<T>(
            //    Expression.Call(
            //        typeof(Queryable), "Where",
            //        new[] { typeof(T) },
            //        source.Expression, expression));

            // 修复：添加 Expression.Quote
            var call = Expression.Call(
                typeof(Queryable),
                "Where",
                new[] { typeof(T) },
                source.Expression,
                Expression.Quote(expression));

            return source.Provider.CreateQuery<T>(call);
        }

        /// <summary>
        /// 使用系统的Queryable.Where
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IQueryable<T> Where<T>(this IQueryable<T> source, string predicate)
        {
            if (string.IsNullOrEmpty(predicate)) return source;

            //var param = Expression.Parameter(typeof(T), "x");
            //var lambda = DynamicExpressionParserBinding.ParseLambda<T>(new[] { param }, predicate);

            //// 确保表达式返回布尔值
            //if (lambda.ReturnType != typeof(bool))
            //{
            //    throw new ArgumentException(
            //        $"Filter expression must return boolean, but returns {lambda.ReturnType.Name}");
            //}

            //// 创建强类型的谓词表达式
            //var predicateExpr = Expression.Lambda<Func<T, bool>>(
            //    lambda.Body,
            //    lambda.Parameters);


            //var results = employees.AsQueryable().Where(filter);

            // 解析RowFilter
            var filter = FilterParser.Parse<T>(predicate);
            return Queryable.Where(source, filter);

            //var predicateExpr = (Expression<Func<T, bool>>)lambda;

            //return Queryable.Where(source, predicateExpr);
        }
    }

    // 简单的动态表达式解析器
    public static class DynamicExpressionParserBinding
    {



        internal class DynamicExpressionParserCore<TItem>
        {
            private readonly string expression;
            private readonly ParameterExpression[] parameters;
            private int index;

            public DynamicExpressionParserCore(string expression, ParameterExpression[] parameters)
            {
                this.expression = expression;
                this.parameters = parameters;
                this.index = 0;
            }

            public Expression Parse()
            {
                return ParseExpression();
            }

            private Expression ParseExpression()
            {
                return ParseLogicalOr();
            }

            // 解析逻辑 OR 操作 (||)
            private Expression ParseLogicalOr()
            {
                var left = ParseLogicalAnd();

                while (PeekToken("||"))
                {
                    NextToken(); // 跳过 ||
                    var right = ParseLogicalAnd();
                    left = Expression.OrElse(left, right);
                }

                return left;
            }

            // 解析逻辑 AND 操作 (&&)
            private Expression ParseLogicalAnd()
            {
                var left = ParseComparison();

                while (PeekToken("&&"))
                {
                    NextToken(); // 跳过 &&
                    var right = ParseComparison();
                    left = Expression.AndAlso(left, right);
                }

                return left;
            }

            // 解析比较操作 (>, <, ==, !=, >=, <=, LIKE)
            private Expression ParseComparison()
            {
                if (PeekToken("("))
                {
                    NextToken(); // 跳过 (
                    var exp = ParseExpression();
                    ExpectToken(")");
                    return exp;
                }
                // 处理带通配符的字符串字面量
                if (PeekToken("'%") || PeekToken("'"))
                {
                    return ParseStringLiteral();
                }
                if (char.IsLetter(PeekChar()))
                {
                    // 属性访问
                    var identifier = ParseIdentifier();
                    var param = parameters[0];
                    var property = typeof(TItem).GetProperty(identifier,
                        BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                    if (property == null)
                        throw new ArgumentException($"Property '{identifier}' not found on type {typeof(TItem).Name}");

                    return Expression.Property(param, property);
                }

                if (char.IsDigit(PeekChar()) || PeekChar() == '-')
                {
                    // 数值（支持负数）
                    return ParseNumberLiteral();
                }


                var left = ParsePrimary();

                // 如果后面没有操作符，尝试自动完成表达式
                if (!IsComparisonOperator())
                {
                    // 自动添加 "!= null" 检查
                    if (left.Type.IsClass || Nullable.GetUnderlyingType(left.Type) != null)
                    {
                        return Expression.NotEqual(left, Expression.Constant(null, left.Type));
                    }

                    // 自动添加 "!= false" 检查（针对布尔属性）
                    if (left.Type == typeof(bool))
                    {
                        return Expression.NotEqual(left, Expression.Constant(false));
                    }

                    throw new InvalidOperationException(
                        $"Incomplete expression. Property '{GetPropertyName(left)}' requires a comparison operator");
                }

                // 原有比较操作符处理...
                var op = NextToken();
                var right = ParsePrimary();

                switch (op.ToUpper())
                {
                    case ">": return Expression.GreaterThan(left, right);
                    case "<": return Expression.LessThan(left, right);
                    case "==": return Expression.Equal(left, right);
                    case "!=": return Expression.NotEqual(left, right);
                    case ">=": return Expression.GreaterThanOrEqual(left, right);
                    case "<=": return Expression.LessThanOrEqual(left, right);
                    case "LIKE":
                        //var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                        //return Expression.Call(left, containsMethod, right);
                        return HandleLikeExpression(left, right);
                    default:
                        throw new InvalidOperationException($"Unsupported operator: {op}");
                }
            }

            private Expression HandleLikeExpression(Expression left, Expression right)
            {
                // 获取模式字符串的元数据
                var patternData = ((ConstantExpression)right).Value;
                var patternInfo = new
                {
                    Value = "",
                    CaseInsensitive = false
                };

                if (patternData != null)
                {
                    patternInfo = new
                    {
                        Value = patternData.GetType().GetProperty("Value")?.GetValue(patternData) as string ?? "",
                        CaseInsensitive = (bool)(patternData.GetType().GetProperty("CaseInsensitive")?.GetValue(patternData) ?? false)
                    };
                }

                string pattern = patternInfo.Value;
                bool caseInsensitive = patternInfo.CaseInsensitive;

                // 处理通配符
                Expression body;
                MethodInfo method;

                if (pattern.StartsWith("%") && pattern.EndsWith("%"))
                {
                    string inner = pattern.Substring(1, pattern.Length - 2);
                    method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                    body = Expression.Call(left, method, Expression.Constant(inner));
                }
                else if (pattern.StartsWith("%"))
                {
                    string inner = pattern.Substring(1);
                    method = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
                    body = Expression.Call(left, method, Expression.Constant(inner));
                }
                else if (pattern.EndsWith("%"))
                {
                    string inner = pattern.Substring(0, pattern.Length - 1);
                    method = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
                    body = Expression.Call(left, method, Expression.Constant(inner));
                }
                else
                {
                    // 没有通配符，直接比较相等
                    return Expression.Equal(left, Expression.Constant(pattern));
                }

                // 处理大小写不敏感
                if (caseInsensitive)
                {
                    var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
                    var leftLower = Expression.Call(left, toLowerMethod);
                    var patternLower = Expression.Constant(pattern.ToLower());
                    body = Expression.Call(leftLower, method, patternLower);
                }

                return body;
            }

            private Expression HandleLikeExpression_old(Expression left, Expression right)
            {
                // 确保右侧是常量表达式
                if (!(right is ConstantExpression rightConst))
                {
                    throw new InvalidOperationException("LIKE operator requires a string literal on the right side");
                }

                string pattern = rightConst.Value?.ToString() ?? string.Empty;
                bool caseInsensitive = false;

                // 检查是否使用不区分大小写的 LIKE
                if (pattern.StartsWith("'") && pattern.EndsWith("'"))
                {
                    pattern = pattern.Substring(1, pattern.Length - 2);
                }
                else if (pattern.StartsWith("i'") && pattern.EndsWith("'"))
                {
                    pattern = pattern.Substring(2, pattern.Length - 3);
                    caseInsensitive = true;
                }

                // 处理通配符
                Expression body = null;
                MethodInfo method = null;

                if (pattern.StartsWith("%") && pattern.EndsWith("%"))
                {
                    string inner = pattern.Substring(1, pattern.Length - 2);
                    method = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                    body = Expression.Call(left, method, Expression.Constant(inner));
                }
                else if (pattern.StartsWith("%"))
                {
                    string inner = pattern.Substring(1);
                    method = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
                    body = Expression.Call(left, method, Expression.Constant(inner));
                }
                else if (pattern.EndsWith("%"))
                {
                    string inner = pattern.Substring(0, pattern.Length - 1);
                    method = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
                    body = Expression.Call(left, method, Expression.Constant(inner));
                }
                else
                {
                    // 没有通配符，直接比较相等
                    return Expression.Equal(left, Expression.Constant(pattern));
                }

                // 处理大小写不敏感
                if (caseInsensitive)
                {
                    var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
                    var leftLower = Expression.Call(left, toLowerMethod);
                    var patternLower = Expression.Constant(pattern.ToLower());
                    body = Expression.Call(leftLower, method, patternLower);
                }

                return body;
            }

            private bool IsComparisonOperator()
            {
                return PeekToken(">") || PeekToken("<") || PeekToken("==") ||
                       PeekToken("!=") || PeekToken(">=") || PeekToken("<=") ||
                       PeekToken("LIKE", true);
            }

            private string GetPropertyName(Expression expr)
            {
                if (expr is MemberExpression memberExpr)
                {
                    return memberExpr.Member.Name;
                }
                return "expression";
            }

            // 解析基础表达式 (属性、值、括号表达式)
            private Expression ParsePrimary()
            {
                if (PeekToken("("))
                {
                    NextToken(); // 跳过 (
                    var exp = ParseExpression();
                    ExpectToken(")");
                    return exp;
                }

                if (char.IsLetter(PeekChar()))
                {
                    // 属性访问
                    var identifier = ParseIdentifier();
                    var param = parameters[0];
                    var property = typeof(TItem).GetProperty(identifier, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                    if (property == null)
                        throw new System.ArgumentException($"Property '{identifier}' not found on type {typeof(TItem).Name}");

                    return Expression.Property(param, property);
                }

                if (PeekChar() == '\'')
                {
                    // 字符串值
                    return ParseStringLiteral();
                }

                if (char.IsDigit(PeekChar()))
                {
                    // 数值
                    return ParseNumberLiteral();
                }

                throw new System.Exception($"Unexpected token at position {index}");
            }

            private string ParseIdentifier()
            {
                var start = index;
                while (index < expression.Length && (char.IsLetterOrDigit(expression[index]) || expression[index] == '_'))
                {
                    index++;
                }
                return expression.Substring(start, index - start);
            }

            private Expression ParseStringLiteral_old()
            {
                NextToken(); // 跳过开头的 '
                var start = index;

                while (index < expression.Length && expression[index] != '\'')
                {
                    index++;
                }

                if (index >= expression.Length)
                    throw new System.Exception("Unterminated string literal");

                var value = expression.Substring(start, index - start);
                NextToken(); // 跳过结尾的 '
                return Expression.Constant(value);
            }

            private Expression ParseStringLiteral()
            {
                bool caseInsensitive = false;

                // 检查是否大小写不敏感标记 (i')
                if (PeekToken("i'", true))
                {
                    caseInsensitive = true;
                    NextToken(); // 跳过 i'
                }
                else if (PeekToken("'"))
                {
                    NextToken(); // 跳过开头的 '
                }
                else
                {
                    throw new Exception("Invalid string literal format");
                }

                var start = index;
                var inEscape = false;
                var sb = new StringBuilder();

                while (index < expression.Length)
                {
                    char c = expression[index];

                    if (inEscape)
                    {
                        sb.Append(c);
                        inEscape = false;
                        index++;
                        continue;
                    }

                    if (c == '\\')
                    {
                        inEscape = true;
                        index++;
                        continue;
                    }

                    if (c == '\'')
                    {
                        index++; // 跳过结尾的 '
                        break;
                    }

                    sb.Append(c);
                    index++;
                }

                string stringValue = sb.ToString();

                // 返回包含元数据的对象
                return Expression.Constant(new
                {
                    Value = stringValue,
                    CaseInsensitive = caseInsensitive
                }, typeof(object));
            }

            private Expression ParseNumberLiteral()
            {
                var start = index;
                bool isDecimal = false;
                bool isNegative = false;

                // 处理负数
                if (PeekChar() == '-')
                {
                    isNegative = true;
                    index++;
                }

                while (index < expression.Length)
                {
                    char c = expression[index];

                    if (char.IsDigit(c) || c == '.' || c == 'e' || c == 'E')
                    {
                        if (c == '.') isDecimal = true;
                        index++;
                    }
                    else
                    {
                        break;
                    }
                }

                var valueStr = expression.Substring(start, index - start);

                try
                {
                    if (isDecimal)
                    {
                        if (decimal.TryParse(valueStr, out decimal decimalValue))
                        {
                            return Expression.Constant(decimalValue);
                        }
                    }
                    else
                    {
                        if (long.TryParse(valueStr, out long longValue))
                        {
                            return Expression.Constant(longValue);
                        }
                        else if (int.TryParse(valueStr, out int intValue))
                        {
                            return Expression.Constant(intValue);
                        }
                    }
                }
                catch
                {
                    // 解析失败，尝试作为double
                    if (double.TryParse(valueStr, out double doubleValue))
                    {
                        return Expression.Constant(doubleValue);
                    }
                }

                throw new Exception($"Invalid number: {valueStr}");
            }




            private Expression ParseNumberLiteral_old()
            {
                var start = index;
                while (index < expression.Length && char.IsDigit(expression[index]))
                {
                    index++;
                }

                var value = expression.Substring(start, index - start);
                if (int.TryParse(value, out int intValue))
                {
                    return Expression.Constant(intValue);
                }

                throw new System.Exception($"Invalid number: {value}");
            }

            private char PeekChar()
            {
                if (index >= expression.Length) return '\0';
                return expression[index];
            }

            private bool PeekToken(string token, bool ignoreCase = false)
            {
                var comparison = ignoreCase ? System.StringComparison.OrdinalIgnoreCase : System.StringComparison.Ordinal;
                return expression.IndexOf(token, index, comparison) == index;
            }

            private string NextToken()
            {
                SkipWhitespace();

                if (index >= expression.Length)
                    return null;

                // 检查多字符操作符
                var multiCharOps = new[] { "==", "!=", ">=", "<=", "&&", "||", "LIKE" };
                foreach (var op in multiCharOps)
                {
                    if (expression.Length - index >= op.Length &&
                        expression.Substring(index, op.Length).Equals(op, System.StringComparison.OrdinalIgnoreCase))
                    {
                        index += op.Length;
                        return op;
                    }
                }

                // 单字符操作符
                var singleCharOps = new[] { '>', '<', '=', '(', ')' };
                if (singleCharOps.Contains(expression[index]))
                {
                    return expression[index++].ToString();
                }

                // 标识符或值
                var start = index;
                while (index < expression.Length && !char.IsWhiteSpace(expression[index]) &&
                       !singleCharOps.Contains(expression[index]))
                {
                    index++;
                }

                return expression.Substring(start, index - start);
            }

            private void ExpectToken(string token)
            {
                var next = NextToken();
                if (next != token)
                    throw new System.Exception($"Expected '{token}', found '{next}'");
            }

            private void SkipWhitespace()
            {
                while (index < expression.Length && char.IsWhiteSpace(expression[index]))
                {
                    index++;
                }
            }
        }

        public static LambdaExpression ParseLambda<TItem>(
       ParameterExpression[] parameters,
       string expression)
        {
            var parser = new DynamicExpressionParserCore<TItem>(expression, parameters);
            var body = parser.Parse();

            // 确保表达式主体是布尔类型
            if (body.Type != typeof(bool))
            {
                // 尝试自动转换为布尔表达式
                body = ConvertToBooleanExpression(body);
            }

            return Expression.Lambda(body, parameters);
        }




        //public static LambdaExpression ParseLambda<TItem>(
        //ParameterExpression[] parameters,
        //Type resultType,
        //string expression)
        //{
        //    // 传递类型参数 TItem 给解析器
        //    var parser = new DynamicExpressionParserCore<TItem>(expression, parameters);
        //    return Expression.Lambda(parser.Parse(), parameters);
        //}
        // 自动转换表达式为布尔类型
        private static Expression ConvertToBooleanExpression(Expression expression)
        {
            // 1. 处理布尔属性直接使用的情况
            if (expression.Type == typeof(bool))
            {
                return expression;
            }

            // 2. 处理非空检查
            if (expression.Type.IsClass || Nullable.GetUnderlyingType(expression.Type) != null)
            {
                return Expression.NotEqual(expression, Expression.Constant(null, expression.Type));
            }

            // 3. 处理数值类型
            if (expression.Type.IsValueType && expression.Type != typeof(bool))
            {
                var zero = Expression.Constant(Activator.CreateInstance(expression.Type), expression.Type);
                return Expression.NotEqual(expression, zero);
            }

            // 4. 处理字符串
            if (expression.Type == typeof(string))
            {
                var nullCheck = Expression.NotEqual(expression, Expression.Constant(null, typeof(string)));
                var emptyCheck = Expression.NotEqual(expression, Expression.Constant(string.Empty));
                return Expression.AndAlso(nullCheck, emptyCheck);
            }

            throw new InvalidOperationException(
                $"Cannot automatically convert expression of type {expression.Type.Name} to boolean");
        }
    }



    #endregion
}

/*
 
 * BindingCollection<object > objList = new BindingCollection<object>(); 
objList =你的结果集; 
this.dataGridView1.DataSource = objList; 
 
 * 
 */
