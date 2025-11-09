using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;
using System.Data;

namespace RUINOR.WinFormsUI.ChkComboBox
{
    /// <summary>
    /// Used together with the ListSelectionWrapper in order to wrap data sources for a CheckBoxComboBox.
    /// It helps to ensure you don't add an extra "Selected" property to a class that don't really need or want that information.
    /// </summary>
    public class ObjectSelectionWrapper<T> : INotifyPropertyChanged
    {
        public ObjectSelectionWrapper(T item, ListSelectionWrapper<T> container)
            : base()
        {
            _Container = container;
            _Item = item;
            // 注册容器的DisplayNameProperty变更事件
            if (container != null)
            {
                // 注意：此处可能需要为ListSelectionWrapper添加PropertyChanged事件支持
                // 目前先初始化_nameCache为null，表示需要首次计算
                _nameCache = null;
            }
        }


        #region PRIVATE PROPERTIES

        /// <summary>
        /// Used as a count indicator for the item.
        /// </summary>
        private int _Count = 0;
        /// <summary>
        /// Is this item selected.
        /// </summary>
        private bool _Selected = false;
        /// <summary>
        /// A reference to the wrapped item.
        /// </summary>
        private T _Item;
        /// <summary>
        /// The containing list for these selections.
        /// </summary>
        private ListSelectionWrapper<T> _Container;
        /// <summary>
        /// 缓存Name属性值，避免重复计算
        /// </summary>
        private string _nameCache = null;
        /// <summary>
        /// 缓存之前的ShowCounts值，用于检测是否需要重新计算Name
        /// </summary>
        private bool _lastShowCounts = false;
        /// <summary>
        /// 缓存之前的Count值，用于检测是否需要重新计算Name
        /// </summary>
        private int _lastCount = 0;

        #endregion

        #region PUBLIC PROPERTIES

        /// <summary>
        /// An indicator of how many items with the specified status is available for the current filter level.
        /// </summary>
        public int Count
        {
            get { return _Count; }
            set 
            { 
                if (_Count != value)
                {
                    _Count = value;
                    // 清除Name缓存，因为Count变化会影响显示
                    _nameCache = null;
                }
            }
        }
        /// <summary>
        /// A reference to the item wrapped.
        /// </summary>
        public T Item
        {
            get { return _Item; }
            set 
            { 
                if (!object.Equals(_Item, value))
                {
                    _Item = value;
                    // 清除Name缓存，因为Item变化会影响显示
                    _nameCache = null;
                }
            }
        }
        /// <summary>
        /// The item display value. If ShowCount is true, it displays the "Name [Count]".
        /// 使用缓存机制避免重复计算，提高性能
        /// </summary>
        public string Name
        {
            get 
            {
                // 检查是否需要重新计算Name值
                bool needRecalculate = _nameCache == null || 
                                      _lastShowCounts != _Container.ShowCounts || 
                                      _lastCount != _Count;
                
                if (!needRecalculate)
                    return _nameCache;
                
                string nameValue = null;
                if (string.IsNullOrEmpty(_Container.DisplayNameProperty))
                    nameValue = Item.ToString();
                else if (Item is DataRow) // A specific implementation for DataRow
                {
                    object value = ((DataRow)((Object)Item))[_Container.DisplayNameProperty];
                    nameValue = value != null ? value.ToString() : string.Empty;
                }
                else
                {
                    // 优化：使用TryGetPropertyValue方法获取属性值，避免多次反射
                    nameValue = TryGetPropertyValue(Item, _Container.DisplayNameProperty);
                }
                
                // 计算最终的显示名称
                if (_Container.ShowCounts)
                {
                    _nameCache = string.Format("{0} [{1}]", nameValue, _Count);
                }
                else
                {
                    _nameCache = nameValue;
                }
                
                // 更新缓存的状态
                _lastShowCounts = _Container.ShowCounts;
                _lastCount = _Count;
                
                return _nameCache;
            }
        }
        
        /// <summary>
        /// 尝试获取对象的属性值，优化反射性能
        /// </summary>
        /// <param name="obj">目标对象</param>
        /// <param name="propertyName">属性名</param>
        /// <returns>属性值的字符串表示</returns>
        private string TryGetPropertyValue(object obj, string propertyName)
        {
            if (obj == null || string.IsNullOrEmpty(propertyName))
                return string.Empty;
            
            try
            {
                // 优先使用TypeDescriptor（性能比直接反射好）
                PropertyDescriptor pd = TypeDescriptor.GetProperties(obj).Find(propertyName, true);
                if (pd != null)
                {
                    object value = pd.GetValue(obj);
                    return value != null ? value.ToString() : string.Empty;
                }
                
                // 如果TypeDescriptor找不到，再使用反射
                PropertyInfo pi = obj.GetType().GetProperty(propertyName);
                if (pi == null)
                    throw new Exception(String.Format(
                              "Property {0} cannot be found on {1}.",
                              propertyName,
                              obj.GetType()));
                
                object propValue = pi.GetValue(obj, null);
                return propValue != null ? propValue.ToString() : string.Empty;
            }
            catch (Exception)
            {
                // 发生异常时返回空字符串，避免整个控件崩溃
                return string.Empty;
            }
        }
        /// <summary>
        /// The textbox display value. The names concatenated.
        /// </summary>
        public string NameConcatenated
        {
            get { return _Container.SelectedNames; }
        }
        /// <summary>
        /// Indicates whether the item is selected.
        /// </summary>
        public bool Selected
        {
            get { return _Selected; }
            set 
            {
                if (_Selected != value)
                {
                    _Selected = value;
                    OnPropertyChanged("Selected");
                    OnPropertyChanged("NameConcatenated");
                }
            }
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
