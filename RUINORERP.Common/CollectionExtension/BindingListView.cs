using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.ComponentModel;
using System.Linq.Dynamic.Core;
using RUINORERP.Common.Extensions;
using System.Linq.Expressions; // 添加动态LINQ支持

namespace RUINORERP.Common.CollectionExtension
{

    /// <summary>
    /// 支持高级过滤和排序的BindingList实现
    /// </summary>
    /// <typeparam name="T">集合元素类型</typeparam>
    public class BindingListView<T> : BindingList<T>, IBindingListView
    {
        private readonly List<T> _originalList; // 原始数据备份
        private string _filter = string.Empty;
        private string _sort = string.Empty;
        private ListSortDescriptionCollection _sortDescriptions;
        private PropertyDescriptor _sortProperty;
        private ListSortDirection _sortDirection;
        private bool _isSorted;
        private bool _supportsFiltering = true;
        private bool _isApplyingFilter; // 防止递归调用
        private bool _isApplyingSort; // 防止递归调用

        //public BindingListView(IEnumerable<T> source)
        //{
        //    if (source == null)
        //        throw new ArgumentNullException(nameof(source));

        //    _originalList = source.ToList();

        //    // 初始化内部列表（不触发事件）
        //    this.RaiseListChangedEvents = false;
        //    foreach (var item in _originalList)
        //        base.Add(item);
        //    this.RaiseListChangedEvents = true;
        //}

        public BindingListView(IEnumerable<T> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            _originalList = source.ToList();
            //Console.WriteLine("原始数据行：" + _originalList.Count);

            // 使用临时列表避免枚举时修改
            var tempList = new List<T>(_originalList);

            // 初始化内部列表（不触发事件）
            this.RaiseListChangedEvents = false;
            foreach (var item in tempList)
            {
                base.Add(item);
            }
            Console.WriteLine("base.Count：" + this.Count);
            this.RaiseListChangedEvents = true;
        }


        #region IBindingListView 实现

        public string Filter
        {
            get => _filter;
            set
            {
                if (_filter == value) return;
                if (_isApplyingFilter) return;

                _filter = value;
                ApplySmartSort();
            }
        }
        public bool SupportsAdvancedSorting => true;

        public bool SupportsFiltering => true;

        public void RemoveFilter()
        {
            Filter = string.Empty;
        }


        #region 核心功能实现 - 智能排序

        /// <summary>
        /// 应用智能排序（不删除项，只改变顺序）
        /// </summary>
        private void ApplySmartSort()
        {
            if (_isApplyingSort) return;
            _isApplyingSort = true;

            try
            {
                // 始终使用完整的原始数据
                List<T> sortedItems = new List<T>(_originalList);

                // 构建排序字符串（智能排序+用户定义排序）
                string orderByString = BuildSmartSortString();

                if (!string.IsNullOrWhiteSpace(orderByString))
                {
                    try
                    {
                        // 应用智能排序
                        sortedItems = sortedItems.AsQueryable().OrderBy(orderByString).ToList();
                    }
                    catch (Exception ex)
                    {
                        // 排序失败时触发事件
                        OnSortError(new SortErrorEventArgs(ex, _filter));
                    }
                }

                // 更新绑定列表（不触发事件）
                this.RaiseListChangedEvents = false;
                base.ClearItems();
                foreach (var item in sortedItems)
                {
                    if (!Contains(item))
                    {
                        base.Add(item);
                    }
                }
                this.RaiseListChangedEvents = true;

                // 触发一次重置事件
                OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
            }
            finally
            {
                _isApplyingSort = false;
                _needsSorting = false; // 重置标志
            }
        }

        /// <summary>
        /// 构建智能排序字符串
        /// </summary>
        private string BuildSmartSortString()
        {
            List<string> sortExpressions = new List<string>();

            // 1. 智能搜索排序：匹配项在前，非匹配项在后
            if (!string.IsNullOrWhiteSpace(_filter))
            {
                // 使用三元表达式：匹配项=1（在前），非匹配项=0（在后）
                sortExpressions.Add($"(({_filter}) ? 1 : 0) DESC");
            }

            // 2. 用户定义的高级排序
            if (_sortDescriptions != null && _sortDescriptions.Count > 0)
            {
                foreach (ListSortDescription sortDesc in _sortDescriptions)
                {
                    var property = sortDesc.PropertyDescriptor.Name;
                    var direction = sortDesc.SortDirection == ListSortDirection.Ascending ? "ASC" : "DESC";
                    sortExpressions.Add($"{property} {direction}");
                }
            }
            // 3. 用户定义的基础排序
            else if (_sortProperty != null)
            {
                var direction = _sortDirection == ListSortDirection.Ascending ? "ASC" : "DESC";
                sortExpressions.Add($"{_sortProperty.Name} {direction}");
            }

            return string.Join(", ", sortExpressions);
        }

        #endregion
        public string Sort
        {
            get => _sort;
            set
            {
                if (_sort == value) return;
                if (_isApplyingSort) return;

                _sort = value;
                ApplySmartSort();
            }
        }

        public void ApplySort(ListSortDescriptionCollection sorts)
        {
            _sortDescriptions = sorts;
            _isSorted = true;
            ApplySmartSort(); // 重新应用过滤和排序
        }


        public ListSortDescriptionCollection SortDescriptions => _sortDescriptions;





        #endregion

        #region 核心功能实现

        /// <summary>
        /// 应用过滤条件
        /// </summary>
        private void ApplyFilter()
        {
            if (_isApplyingFilter) return;
            _isApplyingFilter = true;

            try
            {
                List<T> filteredItems;

                if (string.IsNullOrWhiteSpace(_filter))
                {
                    // 无过滤条件 - 使用原始数据
                    filteredItems = new List<T>(_originalList);
                }
                else
                {
                    try
                    {
                        // 使用动态LINQ进行过滤
                        var query = _originalList.AsQueryable().WhereCustom(_filter);
                        filteredItems = query.ToList();
                    }
                    catch (Exception ex)
                    {
                        // 过滤失败时恢复原始数据
                        _supportsFiltering = false;
                        filteredItems = new List<T>(_originalList);
                        OnFilterError(new FilterErrorEventArgs(ex, _filter));
                    }
                }

                // 应用排序
                if (_isSorted && filteredItems.Count > 0)
                {
                    ApplySortInternal(ref filteredItems);
                }

                // 更新绑定列表（不触发事件）
                this.RaiseListChangedEvents = false;
                base.ClearItems();
                foreach (var item in filteredItems)
                {
                    if (!Contains(item))
                    {
                        base.Add(item);
                    }
                    
                }
                this.RaiseListChangedEvents = true;

                // 触发一次重置事件
                OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
            }
            finally
            {
                _isApplyingFilter = false;
            }
        }

        /// <summary>
        /// 应用排序（对传入的列表进行排序，通过引用修改）
        /// </summary>
        private void ApplySortInternal(ref List<T> items)
        {
            if (_sortDescriptions != null && _sortDescriptions.Count > 0)
            {
                IOrderedQueryable<T> orderedQuery = null;
                var query = items.AsQueryable();

                foreach (ListSortDescription sortDesc in _sortDescriptions)
                {
                    var property = sortDesc.PropertyDescriptor;
                    var direction = sortDesc.SortDirection;

                    if (orderedQuery == null)
                    {
                        orderedQuery = direction == ListSortDirection.Ascending
                            ? query.OrderBy($"{property.Name}")
                            : query.OrderBy($"{property.Name} DESC");
                    }
                    else
                    {
                        orderedQuery = direction == ListSortDirection.Ascending
                            ? orderedQuery.ThenBy($"{property.Name}")
                            : orderedQuery.ThenBy($"{property.Name} DESC");
                    }
                }

                if (orderedQuery != null)
                {
                    items = orderedQuery.ToList();
                }
            }
            else if (_sortProperty != null)
            {
                items = _sortDirection == ListSortDirection.Ascending
                    ? items.OrderBy(x => _sortProperty.GetValue(x)).ToList()
                    : items.OrderByDescending(x => _sortProperty.GetValue(x)).ToList();
            }
        }

        #endregion

        #region 重写基类方法

        protected override void InsertItem(int index, T item)
        {
            if (!_isApplyingSort)
            {
                _originalList.Add(item);

                // 1. 添加到原始列表
                _originalList.Add(item);

                // 2. 如果当前没有排序条件，直接添加到显示列表末尾
                if (string.IsNullOrEmpty(_filter) &&
                    _sortDescriptions == null &&
                    _sortProperty == null)
                {
                    base.InsertItem(base.Count, item);
                }
                // 3. 如果有排序条件，但列表较小，直接插入到正确位置
                else if (_originalList.Count < 100) // 小列表阈值
                {
                    InsertItemWithSort(item);
                }
                // 4. 大列表或复杂条件，延迟排序
                else
                {
                    _needsSorting = true;
                    base.InsertItem(base.Count, item);
                }
                ////ApplyFilter();
                //ApplySmartSort();
            }
            else
            {
                base.InsertItem(index, item);
            }
        }


        // 新方法：直接插入到排序位置
        private void InsertItemWithSort(T item)
        {
            // 构建当前排序条件
            string orderByString = BuildSmartSortString();

            if (!string.IsNullOrEmpty(orderByString))
            {
                try
                {
                    // 使用二分查找找到正确插入位置
                    int insertIndex = FindSortedInsertIndex(item, orderByString);
                    base.InsertItem(insertIndex, item);
                    return;
                }
                catch (Exception ex)
                {
                    // 排序失败时回退到完整排序
                    OnSortError(new SortErrorEventArgs(ex, orderByString));
                }
            }

            // 回退：添加到末尾
            base.InsertItem(base.Count, item);
        }

        // 二分查找插入位置
        private int FindSortedInsertIndex(T item, string orderBy)
        {
            var comparer = new DynamicComparer<T>(orderBy);
            int low = 0;
            int high = base.Count - 1;

            while (low <= high)
            {
                int mid = (low + high) / 2;
                int comparison = comparer.Compare(item, base[mid]);

                if (comparison < 0)
                {
                    high = mid - 1;
                }
                else if (comparison > 0)
                {
                    low = mid + 1;
                }
                else
                {
                    return mid; // 相等时插入在相同项前面
                }
            }

            return low;
        }
        // 动态比较器类
        private class DynamicComparer<T> : IComparer<T>
        {
            private readonly Func<T, T, int> _compareFunc;

            public DynamicComparer(string orderBy)
            {
                var param1 = Expression.Parameter(typeof(T), "x");
                var param2 = Expression.Parameter(typeof(T), "y");

                // 构建动态比较表达式
                var body = DynamicExpressionParser.ParseLambda(
                    new[] { param1, param2 },
                    typeof(int),
                    orderBy);

                _compareFunc = body.Compile() as Func<T, T, int>;
            }

            public int Compare(T x, T y) => _compareFunc(x, y);
        }

        // 在类中添加状态标志
        private bool _needsSorting = false;

        // 添加延迟排序机制
        public void CommitSorting()
        {
            if (_needsSorting)
            {
                ApplySmartSort();
                _needsSorting = false;
            }
        }
        protected override void RemoveItem(int index)
        {
            if (index >= 0 && index < base.Count && !_isApplyingSort)
            {
                var item = base[index];
                _originalList.Remove(item);
                //ApplyFilter();
                ApplySmartSort();
            }
            else
            {
                base.RemoveItem(index);
            }
        }


        protected override void SetItem(int index, T item)
        {
            if (!_isApplyingSort)
            {
                var originalItem = base[index];
                _originalList.Remove(originalItem);
                _originalList.Add(item);
                //ApplyFilter();
                ApplySmartSort();
            }
            else
            {
                base.SetItem(index, item);
            }
        }

        protected override void ClearItems()
        {
            if (!_isApplyingSort)
            {
                _originalList.Clear();
                //ApplyFilter();
                ApplySmartSort();
            }
            else
            {
                base.ClearItems();
            }
        }

        #endregion

        #region 事件增强

        /// <summary>
        /// 过滤错误事件
        /// </summary>
        public event EventHandler<FilterErrorEventArgs> FilterError;

        protected virtual void OnFilterError(FilterErrorEventArgs e)
        {
            FilterError?.Invoke(this, e);
        }
        /// <summary>
        /// 排序错误事件
        /// </summary>
        public event EventHandler<SortErrorEventArgs> SortError;

        protected virtual void OnSortError(SortErrorEventArgs e)
        {
            SortError?.Invoke(this, e);
        }
        #endregion
    }

    /// <summary>
    /// 过滤错误事件参数
    /// </summary>
    public class FilterErrorEventArgs : EventArgs
    {
        public Exception Error { get; }
        public string FilterExpression { get; }

        public FilterErrorEventArgs(Exception error, string filterExpression)
        {
            Error = error;
            FilterExpression = filterExpression;
        }
    }
    /// <summary>
    /// 排序错误事件参数
    /// </summary>
    public class SortErrorEventArgs : EventArgs
    {
        public Exception Error { get; }
        public string SortExpression { get; }

        public SortErrorEventArgs(Exception error, string sortExpression)
        {
            Error = error;
            SortExpression = sortExpression;
        }
    }

}
