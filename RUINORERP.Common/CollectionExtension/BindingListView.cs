using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.ComponentModel;
using System.Linq.Dynamic.Core; // 添加动态LINQ支持

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
        private ListSortDescriptionCollection _sortDescriptions;
        private PropertyDescriptor _sortProperty;
        private ListSortDirection _sortDirection;
        private bool _isSorted;
        private bool _supportsFiltering = true;
        private bool _isApplyingFilter; // 防止递归调用

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
            Console.WriteLine("原始数据行：" + _originalList.Count);

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
                ApplyFilter();
            }
        }

        public void ApplySort(ListSortDescriptionCollection sorts)
        {
            _sortDescriptions = sorts;
            _isSorted = true;
            ApplyFilter(); // 重新应用过滤和排序
        }

        public ListSortDescriptionCollection SortDescriptions => _sortDescriptions;

        public bool SupportsAdvancedSorting => true;

        public bool SupportsFiltering => _supportsFiltering;

        public void RemoveFilter()
        {
            Filter = string.Empty;
        }

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
                        var query = _originalList.AsQueryable().Where(_filter);
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
                    base.Add(item);
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
            if (!_isApplyingFilter)
            {
                _originalList.Add(item);
                ApplyFilter();
            }
            else
            {
                base.InsertItem(index, item);
            }
        }

        protected override void RemoveItem(int index)
        {
            if (index >= 0 && index < base.Count && !_isApplyingFilter)
            {
                var item = base[index];
                _originalList.Remove(item);
                ApplyFilter();
            }
            else
            {
                base.RemoveItem(index);
            }
        }

        protected override void SetItem(int index, T item)
        {
            if (!_isApplyingFilter)
            {
                var originalItem = base[index];
                _originalList.Remove(originalItem);
                _originalList.Add(item);
                ApplyFilter();
            }
            else
            {
                base.SetItem(index, item);
            }
        }

        protected override void ClearItems()
        {
            if (!_isApplyingFilter)
            {
                _originalList.Clear();
                ApplyFilter();
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


}
