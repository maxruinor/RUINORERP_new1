using System;
using System.ComponentModel;
using System.Reflection;
namespace HLH.Lib.List
{


    /// <summary> 
    /// 属性比较类 
    /// </summary> 
    public class PropertyComparer<T> : System.Collections.Generic.IComparer<T>
    {

        private PropertyDescriptor _property;
        private ListSortDirection _direction;

        public PropertyComparer(PropertyDescriptor property, ListSortDirection direction)
        {
            _property = property;
            _direction = direction;
        }

        #region IComparer<T>

        public int Compare(T xWord, T yWord)
        {
            // 获取属性 
            object xValue = GetPropertyValue(xWord, _property.Name);
            object yValue = GetPropertyValue(yWord, _property.Name);

            // 调用升序或降序方法 
            if (_direction == ListSortDirection.Ascending)
            {
                return CompareAscending(xValue, yValue);
            }
            else
            {
                return CompareDescending(xValue, yValue);
            }
        }

        public bool Equals(T xWord, T yWord)
        {
            return xWord.Equals(yWord);
        }

        public int GetHashCode(T obj)
        {
            return obj.GetHashCode();
        }

        #endregion

        /// <summary> 
        /// 比较任意类型属性升序 
        /// </summary> 
        /// <param name="xValue">X值</param> 
        /// <param name="yValue">Y值</param> 
        /// <returns></returns> 
        /// 2007-2-16 23:41 KOSTECH-ACER 
        private int CompareAscending(object xValue, object yValue)
        {
            int result;

            // 如果值实现了IComparer接口 
            if (xValue is IComparable)
            {
                result = ((IComparable)xValue).CompareTo(yValue);
            }
            // 如果值没有实现IComparer接口，但是它们是相等的 
            else if (xValue.Equals(yValue))
            {
                result = 0;
            }
            // 值没有实现IComparer接口且它们是不相等的, 按照字符串进行比较 
            else result = xValue.ToString().CompareTo(yValue.ToString());

            return result;
        }

        /// <summary> 
        /// 比较任意类型属性降序 
        /// </summary> 
        /// <param name="xValue">X值</param> 
        /// <param name="yValue">Y值</param> 
        /// <returns></returns> 
        /// 2007-2-16 23:42 KOSTECH-ACER 
        private int CompareDescending(object xValue, object yValue)
        {
            return CompareAscending(xValue, yValue) * -1;
        }

        /// <summary> 
        /// 获取属性值 
        /// </summary> 
        /// <param name="value">对象</param> 
        /// <param name="property">属性</param> 
        /// <returns></returns> 
        /// 2007-2-16 23:42 KOSTECH-ACER 
        private object GetPropertyValue(T value, string property)
        {
            // 获取属性 
            PropertyInfo propertyInfo = value.GetType().GetProperty(property);

            // 返回值 
            return propertyInfo.GetValue(value, null);
        }
    }

}
