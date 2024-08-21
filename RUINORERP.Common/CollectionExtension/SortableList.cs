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
    /// �����򼯺��� ��datagridview���� ��HLH.Lib.List.BindingCollection
    /// �����������˳�����,ʹ�÷������������ʵ��
    /// </summary> 
    [Serializable]
    public class SortableList<T> : List<T>
    {
        private string _propertyName;
        private bool _ascending;

        //ֱ�� KeyValuePair ����
        //XXX.Sort(delegate(KeyValuePair<string, decimal> s1, KeyValuePair<string, decimal> s2)
        //       {
        //           return s2.Value.CompareTo(s1.Value);
        //       });
        // ˳�����У�ֻ��Ҫ�ѱ��� return s2.Value.CompareTo(s1.Value); ��Ϊ return s1.Value.CompareTo(s2.Value); ���ɡ�


        /// <summary> 
        /// ���� 
        /// </summary> 
        /// <param name="propertyName">��������</param> 
        /// <param name="ascending">�������<c>true</c> ����</param> 
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

            // Ӧ������ 
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
