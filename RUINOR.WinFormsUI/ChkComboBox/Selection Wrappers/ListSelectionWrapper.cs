using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using System.Threading.Tasks;

namespace RUINOR.WinFormsUI.ChkComboBox
{
    /// <summary>
    /// Maintains an additional "Selected" & "Count" value for each item in a List.
    /// Useful in the CheckBoxComboBox. It holds a reference to the List[Index] Item and 
    /// whether it is selected or not.
    /// It also caters for a Count, if needed.
    /// </summary>
    /// <typeparam name="TSelectionWrapper"></typeparam>
    public class ListSelectionWrapper<T> : List<ObjectSelectionWrapper<T>>
    {
        #region CONSTRUCTOR

        /// <summary>
        /// No property on the object is specified for display purposes, so simple ToString() operation 
        /// will be performed. And no Counts will be displayed
        /// </summary>
        public ListSelectionWrapper(IEnumerable source) : this(source, false) { }
        /// <summary>
        /// No property on the object is specified for display purposes, so simple ToString() operation 
        /// will be performed.
        /// </summary>
        public ListSelectionWrapper(IEnumerable source, bool showCounts)
            : base()
        {
            _Source = source;
            _ShowCounts = showCounts;
            if (_Source is IBindingList)
                ((IBindingList)_Source).ListChanged += new ListChangedEventHandler(ListSelectionWrapper_ListChanged);
            Populate();
        }
        /// <summary>
        /// A Display "Name" property is specified. ToString() will not be performed on items.
        /// This is specifically useful on DataTable implementations, or where PropertyDescriptors are used to read the values.
        /// If a PropertyDescriptor is not found, a Property will be used.
        /// </summary>
        public ListSelectionWrapper(IEnumerable source, string usePropertyAsDisplayName) : this(source, false, usePropertyAsDisplayName) { }
        /// <summary>
        /// A Display "Name" property is specified. ToString() will not be performed on items.
        /// This is specifically useful on DataTable implementations, or where PropertyDescriptors are used to read the values.
        /// If a PropertyDescriptor is not found, a Property will be used.
        /// </summary>
        public ListSelectionWrapper(IEnumerable source, bool showCounts, string usePropertyAsDisplayName)
            : this(source, showCounts)
        {
            _DisplayNameProperty = usePropertyAsDisplayName;
        }

        #endregion

        #region PRIVATE PROPERTIES

        /// <summary>
        /// Is a Count indicator used.
        /// </summary>
        private bool _ShowCounts;
        /// <summary>
        /// The original List of values wrapped. A "Selected" and possibly "Count" functionality is added.
        /// </summary>
        private IEnumerable _Source;
        /// <summary>
        /// Used to indicate NOT to use ToString(), but read this property instead as a display value.
        /// </summary>
        private string _DisplayNameProperty = null;
        /// <summary>
        /// 缓存SelectedNames属性值，避免重复计算
        /// </summary>
        private string _selectedNamesCache = null;

        #endregion

        #region PUBLIC PROPERTIES

        /// <summary>
        /// When specified, indicates that ToString() should not be performed on the items. 
        /// This property will be read instead. 
        /// This is specifically useful on DataTable implementations, where PropertyDescriptors are used to read the values.
        /// </summary>
        public string DisplayNameProperty
        {
            get { return _DisplayNameProperty; }
            set 
            { 
                if (_DisplayNameProperty != value)
                {
                    _DisplayNameProperty = value;
                    // 清除SelectedNames缓存，因为子项的Name可能会变化
                    _selectedNamesCache = null;
                }
            }
        }
        /// <summary>
        /// Builds a concatenation list of selected items in the list.
        /// 使用缓存机制避免重复计算，提高性能
        /// </summary>
        public string SelectedNames
        {
            get
            {
                // 如果缓存为空，需要重新计算
                if (_selectedNamesCache == null)
                {
                    // 使用StringBuilder代替字符串连接，性能更好
                    StringBuilder sb = new StringBuilder();
                    foreach (ObjectSelectionWrapper<T> item in this)
                    {
                        if (item.Selected)
                        {
                            if (sb.Length > 0)
                                sb.Append(" & ");
                            
                            sb.AppendFormat("\"{0}\"", item.Name);
                        }
                    }
                    
                    _selectedNamesCache = sb.ToString();
                }
                
                return _selectedNamesCache;
            }
        }
        /// <summary>
        /// Indicates whether the Item display value (Name) should include a count.
        /// </summary>
        public bool ShowCounts
        {
            get { return _ShowCounts; }
            set 
            { 
                if (_ShowCounts != value)
                {
                    _ShowCounts = value;
                    // 清除SelectedNames缓存，因为子项的Name可能会变化
                    _selectedNamesCache = null;
                }
            }
        }

        #endregion

        #region HELPER MEMBERS

        /// <summary>
        /// Reset all counts to zero.
        /// </summary>
        public void ClearCounts()
        {
            foreach (ObjectSelectionWrapper<T> Item in this)
                Item.Count = 0;
                
            // 清除SelectedNames缓存，因为子项的Name可能会变化
            _selectedNamesCache = null;
        }
        /// <summary>
        /// Creates a ObjectSelectionWrapper item.
        /// Note that the constructor signature of sub classes classes are important.
        /// </summary>
        /// <param name="Object"></param>
        /// <returns></returns>
        private ObjectSelectionWrapper<T> CreateSelectionWrapper(IEnumerator Object)
        {
            return CreateSelectionWrapper((T)Object.Current);
        }
        
        private ObjectSelectionWrapper<T> CreateSelectionWrapper(T item)
        {
            // 创建新的选择包装器
            ObjectSelectionWrapper<T> wrapper = new ObjectSelectionWrapper<T>(item, this);
            // 订阅PropertyChanged事件
            wrapper.PropertyChanged += new PropertyChangedEventHandler(OnWrapperPropertyChanged);
            return wrapper;
        }
        
        /// <summary>
        /// 处理ObjectSelectionWrapper的属性变更事件
        /// 当子项的Selected或Count属性变化时，清除SelectedNames缓存
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">属性变更事件参数</param>
        private void OnWrapperPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // 清除SelectedNames缓存，因为子项的状态变化可能会影响SelectedNames的值
            _selectedNamesCache = null;
        }

        public ObjectSelectionWrapper<T> FindObjectWithItem(T Object)
        {
            return Find(new Predicate<ObjectSelectionWrapper<T>>(
                            delegate(ObjectSelectionWrapper<T> target)
                            {
                                return target.Item.Equals(Object);
                            }));
        }

        /*
        public TSelectionWrapper FindObjectWithKey(object key)
        {
            return FindObjectWithKey(new object[] { key });
        }

        public TSelectionWrapper FindObjectWithKey(object[] keys)
        {
            return Find(new Predicate<TSelectionWrapper>(
                            delegate(TSelectionWrapper target)
                            {
                                return
                                    ReflectionHelper.CompareKeyValues(
                                        ReflectionHelper.GetKeyValuesFromObject(target.Item, target.Item.TableInfo),
                                        keys);
                            }));
        }

        public object[] GetArrayOfSelectedKeys()
        {
            List<object> List = new List<object>();
            foreach (TSelectionWrapper Item in this)
                if (Item.Selected)
                {
                    if (Item.Item.TableInfo.KeyProperties.Length == 1)
                        List.Add(ReflectionHelper.GetKeyValueFromObject(Item.Item, Item.Item.TableInfo));
                    else
                        List.Add(ReflectionHelper.GetKeyValuesFromObject(Item.Item, Item.Item.TableInfo));
                }
            return List.ToArray();
        }

        public T[] GetArrayOfSelectedKeys<T>()
        {
            List<T> List = new List<T>();
            foreach (TSelectionWrapper Item in this)
                if (Item.Selected)
                {
                    if (Item.Item.TableInfo.KeyProperties.Length == 1)
                        List.Add((T)ReflectionHelper.GetKeyValueFromObject(Item.Item, Item.Item.TableInfo));
                    else
                        throw new LibraryException("This generator only supports single value keys.");
                    // List.Add((T)ReflectionHelper.GetKeyValuesFromObject(Item.Item, Item.Item.TableInfo));
                }
            return List.ToArray();
        }
        */
        private void Populate()
        {
            // 清除之前的缓存
            _selectedNamesCache = null;
            
            // 清空现有数据
            this.Clear();

            // 当使用BindingList作为数据源时，需要特别处理
            BindingList<T> bindingList = _Source as BindingList<T>;
            if (bindingList != null)
            {
                // 移除之前可能添加的事件处理
                bindingList.ListChanged -= new ListChangedEventHandler(bindingList_ListChanged);
                // 添加新的事件处理
                bindingList.ListChanged += new ListChangedEventHandler(bindingList_ListChanged);
            }

            try
            {
                // 尝试获取数据源的元素数量
                int itemCount;
                ICollection<T> collection = _Source as ICollection<T>;
                if (collection != null)
                {
                    itemCount = collection.Count;
                }
                else
                {
                    // 对于不支持ICollection接口的数据源，尝试使用LINQ Count()
                    // 注意：这可能会导致遍历整个集合
                    try
                    {
                        // 使用LINQ的Count扩展方法获取元素数量
                        itemCount = Enumerable.Count<T>(_Source as IEnumerable<T>);
                    }
                    catch
                    {
                        // 如果Count()不可用，使用默认值
                        itemCount = 0;
                    }
                }

                // 性能优化：对于大量数据，使用批量创建和添加
                if (itemCount > 100 || itemCount == 0) // 阈值可以根据实际情况调整
                {
                    // 预先分配容量，避免动态扩容
                    List<ObjectSelectionWrapper<T>> wrappers = new List<ObjectSelectionWrapper<T>>(itemCount > 0 ? itemCount : 100);
                    
                    // 对于大数据集，考虑使用并行处理
                    // 注意：只有当CreateSelectionWrapper是线程安全的时才能使用并行
                    if (itemCount > 500)
                    {
                        // 只有数据源是数组或列表等支持并行处理的类型时才使用并行
                        if (_Source is T[] || _Source is List<T>)
                        {
                            try
                            {
                                // 使用临时列表收集并行处理结果
                                List<ObjectSelectionWrapper<T>> tempWrappers = new List<ObjectSelectionWrapper<T>>(itemCount);
                                object syncLock = new object();
                                
                                // 显式指定类型参数
                                Parallel.ForEach<T>(_Source as IEnumerable<T>, item =>
                                {
                                    ObjectSelectionWrapper<T> wrapper = CreateSelectionWrapper(item);
                                    lock (syncLock)
                                    {
                                        tempWrappers.Add(wrapper);
                                    }
                                });
                                
                                wrappers.AddRange(tempWrappers);
                            }
                            catch
                            {
                                // 并行处理出错时，退回到串行处理
                                foreach (T item in _Source)
                                {
                                    wrappers.Add(CreateSelectionWrapper(item));
                                }
                            }
                        }
                        else
                        {
                            // 不支持并行处理的数据源，使用串行处理
                            foreach (T item in _Source)
                            {
                                wrappers.Add(CreateSelectionWrapper(item));
                            }
                        }
                    }
                    else
                    {
                        // 中等数据量或未知数据量，使用串行处理但批量添加
                        foreach (T item in _Source)
                        {
                            wrappers.Add(CreateSelectionWrapper(item));
                        }
                    }
                    
                    // 批量添加到集合
                    this.AddRange(wrappers);
                }
                else
                {
                    // 小数据量，保持原有逻辑
                    foreach (T item in _Source)
                    {
                        // 创建新的选择包装器
                        ObjectSelectionWrapper<T> wrapper = CreateSelectionWrapper(item);
                        this.Add(wrapper);
                    }
                }
            }
            catch (Exception ex)
            {
                // 发生异常时，使用简单的遍历方式
                foreach (T item in _Source)
                {
                    ObjectSelectionWrapper<T> wrapper = CreateSelectionWrapper(item);
                    this.Add(wrapper);
                }
            }
        }

        #endregion

        #region EVENT HANDLERS

        private void ListSelectionWrapper_ListChanged(object sender, ListChangedEventArgs e)
        {
            // 如果是BindingList，使用专门的处理方法
            if (_Source is BindingList<T>)
            {
                bindingList_ListChanged(sender, e);
                return;
            }
            
            switch (e.ListChangedType)
            {
                case ListChangedType.ItemAdded:
                    Add(CreateSelectionWrapper((IEnumerator)((IBindingList)_Source)[e.NewIndex]));
                    break;
                case ListChangedType.ItemDeleted:
                    Remove(FindObjectWithItem((T)((IBindingList)_Source)[e.OldIndex]));
                    break;
                case ListChangedType.Reset:
                    Populate();
                    break;
            }
            
            // 列表变化时清除缓存
            _selectedNamesCache = null;
        }
        
        void bindingList_ListChanged(object sender, ListChangedEventArgs e)
        {
            // 根据不同的列表变更类型进行处理
            switch (e.ListChangedType)
            {
                case ListChangedType.ItemAdded:
                    // 添加新项
                    if (e.NewIndex >= 0 && e.NewIndex < ((IList)_Source).Count)
                    {
                        T item = ((IList<T>)_Source)[e.NewIndex];
                        ObjectSelectionWrapper<T> wrapper = CreateSelectionWrapper(item);
                        this.Insert(e.NewIndex, wrapper);
                    }
                    break;
                case ListChangedType.ItemDeleted:
                    // 删除项
                    if (e.NewIndex >= 0 && e.NewIndex < this.Count)
                    {
                        // 移除事件订阅
                        ObjectSelectionWrapper<T> wrapper = this[e.NewIndex];
                        wrapper.PropertyChanged -= new PropertyChangedEventHandler(OnWrapperPropertyChanged);
                        // 从集合中移除
                        this.RemoveAt(e.NewIndex);
                    }
                    break;
                case ListChangedType.ItemChanged:
                    // 项变更
                    if (e.NewIndex >= 0 && e.NewIndex < ((IList)_Source).Count && e.NewIndex < this.Count)
                    {
                        T newItem = ((IList<T>)_Source)[e.NewIndex];
                        ObjectSelectionWrapper<T> wrapper = this[e.NewIndex];
                        // 更新包装器中的项
                        wrapper.Item = newItem;
                    }
                    break;
                case ListChangedType.Reset:
                    // 重置列表
                    Populate();
                    return; // 直接返回，Populate中已经清除了缓存
                case ListChangedType.ItemMoved:
                    // 项移动
                    if (e.NewIndex >= 0 && e.OldIndex >= 0 && e.OldIndex < this.Count && e.NewIndex <= this.Count)
                    {
                        ObjectSelectionWrapper<T> wrapper = this[e.OldIndex];
                        this.RemoveAt(e.OldIndex);
                        this.Insert(e.NewIndex, wrapper);
                    }
                    break;
            }
            
            // 清除缓存
            _selectedNamesCache = null;
        }

        #endregion
    }


    /// <summary>
    /// 绑定数据源时的，内部使用的一个名值对
    /// </summary>
    public class CmbChkItem
    {
        private string key;

        public string Key
        {
            get { return key; }
            set { key = value; }
        }
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public override string ToString()
        {
            // 处理Name为null的情况
            return Name?.ToString() ?? "";
        }

        public string desc { get; set; }


        public CmbChkItem(string pkey, string pname)
        {
            key = pkey ?? "";  // 处理pkey为null的情况
            name = pname ?? ""; // 处理pname为null的情况
        }



    }

}


