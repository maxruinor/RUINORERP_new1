using System.Collections.Generic;
using System.ComponentModel;

namespace HLH.Lib.List
{
    //winform dataGridView 排序
    //用法 直接用 BindingSortCollection<>  代替 List<>
    [System.ComponentModel.DesignerCategory("Code")]
    public class BindingSortCollection<T> : BindingList<T>, System.Collections.Generic.IList<T>
    {


        private bool isSorted;
        private PropertyDescriptor sortProperty;
        private ListSortDirection sortDirection;

        protected override bool IsSortedCore
        {
            get { return isSorted; }
        }

        protected override bool SupportsSortingCore
        {
            get { return true; }
        }

        protected override ListSortDirection SortDirectionCore
        {
            get { return sortDirection; }
        }

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
            foreach (var item in list)
            {
                this.Add(item);
            }
        }

        public void AddRange(T[] list)
        {
            foreach (var item in list)
            {
                this.Add(item);
            }
        }

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

        protected override void RemoveSortCore()
        {
            isSorted = false;
            this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
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
    }
}

/*
 
 * BindingCollection<object > objList = new BindingCollection<object>(); 
objList =你的结果集; 
this.dataGridView1.DataSource = objList; 

本文来自: 网页设计大本营(www.code-123.com) 详细出处参考：http://www.code-123.com/html/201025223400580.html
 * 
 * 
 */
