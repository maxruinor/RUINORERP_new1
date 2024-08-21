using System.Collections.Generic;
using System.ComponentModel;

namespace RUINORERP.Common.CollectionExtension
{
    //winform dataGridView 排序
    //用法 直接用 BindingSortCollection<>  代替 List<>
    [System.ComponentModel.DesignerCategory("Code")]
    public class BindingSortCollection<T> : BindingList<T>, System.Collections.Generic.IList<T>
    {


        private bool isSorted;
        private PropertyDescriptor sortProperty;
        private ListSortDirection sortDirection;


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
