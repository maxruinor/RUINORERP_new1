using System;
using System.ComponentModel;
using System.Reflection;
namespace HLH.Lib.List
{


    /// <summary> 
    /// ���ԱȽ��� 
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
            // ��ȡ���� 
            object xValue = GetPropertyValue(xWord, _property.Name);
            object yValue = GetPropertyValue(yWord, _property.Name);

            // ����������򷽷� 
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
        /// �Ƚ����������������� 
        /// </summary> 
        /// <param name="xValue">Xֵ</param> 
        /// <param name="yValue">Yֵ</param> 
        /// <returns></returns> 
        /// 2007-2-16 23:41 KOSTECH-ACER 
        private int CompareAscending(object xValue, object yValue)
        {
            int result;

            // ���ֵʵ����IComparer�ӿ� 
            if (xValue is IComparable)
            {
                result = ((IComparable)xValue).CompareTo(yValue);
            }
            // ���ֵû��ʵ��IComparer�ӿڣ�������������ȵ� 
            else if (xValue.Equals(yValue))
            {
                result = 0;
            }
            // ֵû��ʵ��IComparer�ӿ��������ǲ���ȵ�, �����ַ������бȽ� 
            else result = xValue.ToString().CompareTo(yValue.ToString());

            return result;
        }

        /// <summary> 
        /// �Ƚ������������Խ��� 
        /// </summary> 
        /// <param name="xValue">Xֵ</param> 
        /// <param name="yValue">Yֵ</param> 
        /// <returns></returns> 
        /// 2007-2-16 23:42 KOSTECH-ACER 
        private int CompareDescending(object xValue, object yValue)
        {
            return CompareAscending(xValue, yValue) * -1;
        }

        /// <summary> 
        /// ��ȡ����ֵ 
        /// </summary> 
        /// <param name="value">����</param> 
        /// <param name="property">����</param> 
        /// <returns></returns> 
        /// 2007-2-16 23:42 KOSTECH-ACER 
        private object GetPropertyValue(T value, string property)
        {
            // ��ȡ���� 
            PropertyInfo propertyInfo = value.GetType().GetProperty(property);

            // ����ֵ 
            return propertyInfo.GetValue(value, null);
        }
    }

}
