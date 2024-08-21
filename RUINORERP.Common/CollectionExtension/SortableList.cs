using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace RUINORERP.Common.CollectionExtension
{
    /*
     
            SortableList<User> list = tempList as SortableList<User>;
            if (Direc == SortDirection.Descending)
            {
                Direc = SortDirection.Ascending;
            }
            else
            {
                Direc = SortDirection.Descending;
            }

            list.Sort("UserName", (Direc == SortDirection.Ascending));
     
     */



    /// <summary> 
    /// 可排序集合类 绑定datagridview排列 用HLH.Lib.List.BindingCollection
    /// 这个用于排列顺序计算,使用方法见上面代码实例
    /// </summary> 
    [Serializable]
    public class SortableList<T> : List<T>
    {
        private string _propertyName;
        private bool _ascending;

        //直接 KeyValuePair 排序
        //XXX.Sort(delegate(KeyValuePair<string, decimal> s1, KeyValuePair<string, decimal> s2)
        //       {
        //           return s2.Value.CompareTo(s1.Value);
        //       });
        // 顺序排列：只需要把变量 return s2.Value.CompareTo(s1.Value); 改为 return s1.Value.CompareTo(s2.Value); 即可。


        /// <summary> 
        /// 排序 
        /// </summary> 
        /// <param name="propertyName">属性名称</param> 
        /// <param name="ascending">如果设置<c>true</c> 升序</param> 
        public void Sort(string propertyName, bool ascending)
        {
            if (_propertyName == propertyName && _ascending == ascending)
                _ascending = !ascending;
            else
            {
                _propertyName = propertyName;
                _ascending = ascending;
            }

            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            PropertyDescriptor propertyDesc = properties.Find(propertyName, true);

            // 应用排序 
            PropertyComparer<T> pc = new PropertyComparer<T>(propertyDesc, (_ascending) ? ListSortDirection.Ascending : ListSortDirection.Descending);
            this.Sort(pc);
        }


        public string toString<k, v>(SortableList<KeyValuePair<k, v>> list)
        {
            string str = string.Empty;
            foreach (KeyValuePair<k, v> kv in list)
            {
                //str += "[" + kv.Key.ToString() + "," + kv.Value.ToString() + "]" + System.Environment.NewLine;
                str += "[" + String.Format("{0,-15}", kv.Key.ToString()) + "," + String.Format("{0,-10}", kv.Value.ToString()) + "]" + System.Environment.NewLine;
            }
            str = str.TrimEnd(System.Environment.NewLine.ToCharArray());
            return str;
        }


        public string toBestString<k, v>(SortableList<KeyValuePair<k, v>> list)
        {

            // String.Format("{0,-10} | {1,-10} | {2,5}", "Bill", "Gates", 51);
            string str = string.Empty;
            foreach (KeyValuePair<k, v> kv in list)
            {
                //str += "[" + kv.Key.ToString() + "," + kv.Value.ToString() + "]" + System.Environment.NewLine;
                str += "[" + String.Format("{0,-30}", kv.Key.ToString()) + "," + String.Format("{0,-10}", kv.Value.ToString()) + "]" + System.Environment.NewLine;
            }
            str = str.TrimEnd(System.Environment.NewLine.ToCharArray());
            return str;
        }


    }

}
