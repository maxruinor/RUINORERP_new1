// Copyright 2006 Herre Kuijpers - <herre@xs4all.nl>
//
// This source file(s) may be redistributed, altered and customized
// by any means PROVIDING the authors name and all copyright
// notices remain intact.
// THIS SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED. USE IT AT YOUR OWN RISK. THE AUTHOR ACCEPTS NO
// LIABILITY FOR ANY DATA DAMAGE/LOSS THAT THIS PRODUCT MAY CAUSE.
//-----------------------------------------------------------------------
using System;
using System.Collections;
using System.Text;
using System.Windows.Forms;

namespace RUINOR.WinFormsUI.OutlookGrid
{
    #region IOutlookGridGroup - declares the arrange/grouping interface here

    /// <summary>
    /// IOutlookGridGroup specifies the interface of any implementation of a OutlookGridGroup class
    /// Each implementation of the IOutlookGridGroup can override the behaviour of the grouping mechanism
    /// Notice also that ICloneable must be implemented. The OutlookGrid makes use of the Clone method of the Group
    /// to create new Group clones. Related to this is the OutlookGrid.GroupTemplate property, which determines what
    /// type of Group must be cloned.
    /// </summary>
    public interface IOutlookGridGroup : IComparable, ICloneable
    {
        /// <summary>
        /// the text to be displayed in the group row
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// determines the value of the current group. this is used to compare the group value
        /// against each item's value.
        /// </summary>
        object Value { get; set; }

        /// <summary>
        /// indicates whether the group is collapsed. If it is collapsed, it group items (rows) will
        /// not be displayed.
        /// </summary>
        bool Collapsed { get; set; }

        /// <summary>
        /// specifies which column is associated with this group
        /// </summary>
        DataGridViewColumn Column { get; set; }

        /// <summary>
        /// specifies the number of items that are part of the current group
        /// this value is automatically filled each time the grid is re-drawn
        /// e.g. after sorting the grid.
        /// </summary>
        int ItemCount { get; set; }

        /// <summary>
        /// specifies the default height of the group
        /// each group is cloned from the GroupStyle object. Setting the height of this object
        /// will also set the default height of each group.
        /// </summary>
        int Height { get; set; }
    }
    #endregion define the arrange/grouping interface here

    #region OutlookGridDefaultGroup - implementation of the default grouping style

    /// <summary>
    /// each arrange/grouping class must implement the IOutlookGridGroup interface
    /// the Group object will determine for each object in the grid, whether it
    /// falls in or outside its group.
    /// It uses the IComparable.CompareTo function to determine if the item is in the group.
    /// </summary>
    public class OutlookGridDefaultGroup : IOutlookGridGroup
    {
        /// <summary>
        /// ����е�ֵ
        /// </summary>
        protected object val;
        /// <summary>
        /// �������
        /// </summary>
        protected string text;
        /// <summary>
        /// �Ƿ��۵���ǰ����
        /// </summary>
        protected bool collapsed;
        /// <summary>
        /// DataGridViewColumn��
        /// </summary>
        protected DataGridViewColumn column;
        /// <summary>
        /// ��ǰ��������ļ�¼��Ŀ
        /// </summary>
        protected int itemCount;
        /// <summary>
        /// �����и߶�
        /// </summary>
        protected int height;

        /// <summary>
        /// OutlookgGridDefaultGroup���� Ĭ�Ϲ��캯��
        /// </summary>
        public OutlookGridDefaultGroup()
        {
            val = null;

            this.column = null;
            height = 34; // default height
        }

        #region IOutlookGridGroup Members

        /// <summary>
        /// ���������ʾ���ı�
        /// </summary>
        public virtual string Text
        {
            get {
                if (column == null)
                    return string.Format("Unbound group: {0} ({1})", Value.ToString(), itemCount == 1 ? "1 item" : itemCount.ToString() + " items");
                else
                    return string.Format("{0}: {1} ({2})", column.HeaderText, Value.ToString(), itemCount == 1 ? "1 item" : itemCount.ToString() + " items"); 
                }
            set { text = value; }
        }

        /// <summary>
        /// ֵ
        /// </summary>
        public virtual object Value
        {
            get { return val; }
            set { val = value; }
        }

        /// <summary>
        /// �Ƿ��۵�
        /// </summary>
        public virtual bool Collapsed
        {
            get { return collapsed; }
            set { collapsed = value; }
        }

        /// <summary>
        /// DataGridViewColumn�ж���
        /// </summary>
        public virtual DataGridViewColumn Column
        {
            get { return column; }
            set { column = value; }
        }

        /// <summary>
        /// �����а����ļ�¼��Ŀ
        /// </summary>
        public virtual int ItemCount
        {
            get { return itemCount; }
            set { itemCount = value; }
        }

        /// <summary>
        /// �����и߶�
        /// </summary>
        public virtual int Height
        {
            get { return height; }
            set { height = value; }
        }

        #endregion

        #region ICloneable Members

        /// <summary>
        /// ʵ��ICloneable�ӿ�
        /// </summary>
        /// <returns></returns>
        public virtual object Clone()
        {
            OutlookGridDefaultGroup gr = new OutlookGridDefaultGroup();
            gr.column = this.column;
            gr.val = this.val;
            gr.collapsed = this.collapsed;
            gr.text = this.text;
            gr.height = this.height;
            return gr;
        }

        #endregion

        #region IComparable Members

        /// <summary>
        /// this is a basic string comparison operation. 
        /// all items are grouped and categorised based on their string-appearance.
        /// </summary>
        /// <param name="obj">the value in the related column of the item to compare to</param>
        /// <returns></returns>
        public virtual int CompareTo(object obj)
        {
            return string.Compare(val.ToString(), obj.ToString());
        }

        #endregion
    }
    #endregion OutlookGridDefaultGroup - implementation of the default grouping style

    #region OutlookGridAlphabeticGroup - an alphabet group implementation
    /// <summary>
    /// this group simple example of an implementation which groups the items into Alphabetic categories
    /// based only on the first letter of each item
    /// 
    /// for this we need to override the Value property (used for comparison)
    /// and the CompareTo function.
    /// Also the Clone method must be overriden, so this Group object can create clones of itself.
    /// Cloning of the group is used by the OutlookGrid
    /// </summary>
    public class OutlookGridAlphabeticGroup : OutlookGridDefaultGroup
    {
        /// <summary>
        /// ��ֵ����ĸ����
        /// </summary>
        public OutlookGridAlphabeticGroup()
            : base()
        {
            
        }

        /// <summary>
        /// ��д����OutlookgGridDefaultGroup��Text����
        /// </summary>
        public override string Text
        {
            get
            {
                return string.Format("Alphabetic: {1} ({2})", column.HeaderText, Value.ToString(), itemCount == 1 ? "1 item" : itemCount.ToString() + " items");
            }
            set { text = value; }
        }

        /// <summary>
        /// ��д����OutlookgGridDefaultGroup��Value����
        /// </summary>
        public override object Value
        {
            get { return val; }
            set { val = value.ToString().Substring(0,1).ToUpper(); }
        }

        #region ICloneable Members
        /// <summary>
        /// each group class must implement the clone function
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            OutlookGridAlphabeticGroup gr = new OutlookGridAlphabeticGroup();
            gr.column = this.column;
            gr.val = this.val;
            gr.collapsed = this.collapsed;
            gr.text = this.text;
            gr.height = this.height;
            return gr;
        }

        #endregion

        #region IComparable Members
        /// <summary>
        /// overide the CompareTo, so only the first character is compared, instead of the whole string
        /// this will result in classifying each item into a letter of the Alphabet.
        /// for instance, this is usefull when grouping names, they will be categorized under the letters A, B, C etc..
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override int CompareTo(object obj)
        {
            return string.Compare(val.ToString(), obj.ToString().Substring(0, 1).ToUpper());
        }

        #endregion IComparable Members

    }
    #endregion OutlookGridAlphabeticGroup - an alphabet group implementation

    #region �����ܷ���

    /// <summary>
    /// each arrange/grouping class must implement the IOutlookGridGroup interface
    /// the Group object will determine for each object in the grid, whether it
    /// falls in or outside its group.
    /// It uses the IComparable.CompareTo function to determine if the item is in the group.
    /// </summary>
    public class OutlookGridMoneyGroup : IOutlookGridGroup
    {
        /// <summary>
        /// ����е�ֵ
        /// </summary>
        protected object val;
        /// <summary>
        /// �������
        /// </summary>
        protected string text;
        /// <summary>
        /// �Ƿ��۵���ǰ����
        /// </summary>
        protected bool collapsed;
        /// <summary>
        /// DataGridViewColumn��
        /// </summary>
        protected DataGridViewColumn column;
        /// <summary>
        /// ��ǰ��������ļ�¼��Ŀ
        /// </summary>
        protected int itemCount;
        /// <summary>
        /// �����и߶�
        /// </summary>
        protected int height;
        /// <summary>
        /// ��������
        /// </summary>
        protected int sumColumn = -1;
        /// <summary>
        /// ���ܽ����Ŀ
        /// </summary>
        protected decimal total = 0.0m;

        /// <summary>
        /// ���ܽ��
        /// </summary>
        public decimal Total
        {
            get { return total; }
            set { total = value; }
        }

        /// <summary>
        /// ��������,Ĭ��û��-1
        /// </summary>
        public int SumColumn
        {
            get { return sumColumn; }
            set { sumColumn = value; }
        }

        /// <summary>
        /// ��������� Ĭ�Ϲ��캯��
        /// </summary>
        public OutlookGridMoneyGroup()
        {
            val = null;

            this.column = null;
            height = 34; // default height
        }

        #region IOutlookGridGroup Members

        /// <summary>
        /// ���������ʾ���ı�
        /// </summary>
        public virtual string Text
        {
            get
            {
                if (column == null)
                    return string.Format("δ����: {0}  ��¼��Ŀ: {1} (�ϼƽ��:{2})", Value.ToString(), itemCount, total.ToString("C"));
                else
                    return string.Format("��{0}����: {1}  ��¼��Ŀ: {2} (�ϼƽ��:{3})", column.HeaderText, Value.ToString(), itemCount, total.ToString("C"));
            }
            set { text = value; }
        }

        /// <summary>
        /// ֵ
        /// </summary>
        public virtual object Value
        {
            get { return val; }
            set { val = value; }
        }

        /// <summary>
        /// �Ƿ��۵�
        /// </summary>
        public virtual bool Collapsed
        {
            get { return collapsed; }
            set { collapsed = value; }
        }

        /// <summary>
        /// DataGridViewColumn�ж���
        /// </summary>
        public virtual DataGridViewColumn Column
        {
            get { return column; }
            set { column = value; }
        }

        /// <summary>
        /// �����а����ļ�¼��Ŀ
        /// </summary>
        public virtual int ItemCount
        {
            get { return itemCount; }
            set { itemCount = value; }
        }

        /// <summary>
        /// �����и߶�
        /// </summary>
        public virtual int Height
        {
            get { return height; }
            set { height = value; }
        }

        #endregion

        #region ICloneable Members

        /// <summary>
        /// ʵ��ICloneable�ӿ�
        /// </summary>
        /// <returns></returns>
        public virtual object Clone()
        {
            OutlookGridMoneyGroup gr = new OutlookGridMoneyGroup();
            gr.column = this.column;
            gr.val = this.val;
            gr.collapsed = this.collapsed;
            gr.text = this.text;
            gr.height = this.height;
            gr.sumColumn = this.sumColumn;
            gr.total = this.total;
            return gr;
        }

        #endregion

        #region IComparable Members

        /// <summary>
        /// this is a basic string comparison operation. 
        /// all items are grouped and categorised based on their string-appearance.
        /// </summary>
        /// <param name="obj">the value in the related column of the item to compare to</param>
        /// <returns></returns>
        public virtual int CompareTo(object obj)
        {
            return string.Compare(val.ToString(), obj.ToString());
        }

        #endregion
    }
    #endregion �����ܷ���

    #region ���ڷ���(�������ܹ���) �̳�OutlookGridMoneyGroup����

    /// <summary>
    /// each arrange/grouping class must implement the IOutlookGridGroup interface
    /// the Group object will determine for each object in the grid, whether it
    /// falls in or outside its group.
    /// It uses the IComparable.CompareTo function to determine if the item is in the group.
    /// </summary>
    public class OutlookGridDateGroup : OutlookGridMoneyGroup
    {
        /// <summary>
        /// ���ڷ�������
        /// </summary>
        protected DateGroupType groupType=DateGroupType.Day;

        /// <summary>
        /// ���ڷ���(�������ܹ���) �̳�OutlookGridMoneyGroup����
        /// </summary>
        public OutlookGridDateGroup()
            : base()
        {
            
        }

        /// <summary>
        /// ���ڷ�������
        /// </summary>
        public DateGroupType GroupType
        {
          get { return groupType; }
          set { groupType = value; }
        }

        /// <summary>
        /// ��д����OutlookGridMoneyGroup��Text����
        /// </summary>
        public override string Text
        {
            get
            {
                //�ж��Ƿ��ǰ�����Դ
                if (base.column == null)
                {
                    //�ж��Ƿ��н������Ҫ����
                    if (base.SumColumn < 0)
                    {
                        return string.Format("δ����: {0}  ��¼��Ŀ: {1} ", Value.ToString(), itemCount);
                    }
                    else
                    {
                        return string.Format("δ����: {0}  ��¼��Ŀ: {1} (�ϼƽ��:{2})", Value.ToString(), itemCount, total.ToString("C"));
                    }
                }
                else
                {
                    if (base.SumColumn < 0)
                    {
                        return string.Format("��{0}����: {1}  ��¼��Ŀ: {2} ", column.HeaderText, val.ToString(), itemCount);
                    }
                    else
                    {
                        return string.Format("��{0}����: {1}  ��¼��Ŀ: {2} (�ϼƽ��:{3})", column.HeaderText, val.ToString(), itemCount, total.ToString("C"));
                    }
                }
            }
            set { text = value; }
        }

        /// <summary>
        /// ��д����OutlookGridMoneyGroup��Value����
        /// </summary>
        public override object Value
        {
            get { return val; }
            set 
            {
                val = GetFormatDate(value.ToString()); 
            }
        }

        /// <summary>
        /// �õ���ʽ���������
        /// </summary>
        /// <param name="date">����</param>
        /// <returns></returns>
        private string GetFormatDate(string date)
        {
            string fomatValue;
            try
            {
                DateTime dt = DateTime.Parse(date);
                switch (groupType)
                {
                    case DateGroupType.Day:
                        fomatValue = dt.ToString("yyyy-M-d");
                        break;
                    case DateGroupType.Week:
                        //��ʾ������������
                        //dt.DayOfWeek.ToString();   
                        fomatValue = dt.ToString("dddd", new System.Globalization.CultureInfo("zh-CN"));
                        break;
                    case DateGroupType.Month:
                        fomatValue = dt.ToString("yyyy��M��");
                        break;
                    case DateGroupType.Quarter:
                        fomatValue = dt.Year + "���" + ((dt.Month % 3 == 0) ? (dt.Month / 3) : (dt.Month / 3 + 1)) + "����";
                        break;
                    case DateGroupType.Year:
                        fomatValue = dt.Year + "���";
                        break;
                    default:
                        fomatValue = dt.ToString("yyyy-M-d");
                        break;
                }
            }
            catch
            {
                fomatValue = date;
            }
            return fomatValue;
        }

        /// <summary>
        /// ���������������
        /// </summary>
        /// <param name="dt">���ڶ���</param>
        /// <returns></returns>
        public static string GetCnWeek(DateTime dt)
        {
            switch (dt.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    return "����һ";
                case DayOfWeek.Tuesday:
                    return "���ڶ�";
                case DayOfWeek.Wednesday:
                    return "������";
                case DayOfWeek.Thursday:
                    return "������";
                case DayOfWeek.Friday:
                    return "������";
                case DayOfWeek.Saturday:
                    return "������";
                case DayOfWeek.Sunday:
                    return "������";
                default:
                    return "";
            }
        }
        
        //��C#��WinForm�����У�ʹ��DateTime.Now.DayOfWeek�������ʱ������Ӣ�ĵģ�
        //�����������أ���ʵ�ܼ򵥣�����ĵ�һ��СС�ĺ�������ʵ�֣�
        public static string WeekDayOfCN(DateTime dt)
        {
            string[] arrCnNames = new string[] { "��", "һ", "��", "��", "��", "��", "��" };
            return "����" + arrCnNames[(int)dt.DayOfWeek];
            //System.DateTime.Now.GetDateTimeFormats('D')[2].Substring(0, 3).ToString(); 
        }
        #region ICloneable Members

        /// <summary>
        /// ʵ��ICloneable�ӿ�
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            OutlookGridDateGroup gr = new OutlookGridDateGroup();
            gr.column = this.column;
            gr.val = this.val;
            gr.collapsed = this.collapsed;
            gr.text = this.text;
            gr.height = this.height;
            gr.sumColumn = this.sumColumn;
            gr.total = this.total;
            gr.groupType = this.groupType;
            return gr;
        }

        #endregion

        #region IComparable Members

        /// <summary>
        /// this is a basic string comparison operation. 
        /// all items are grouped and categorised based on their string-appearance.
        /// </summary>
        /// <param name="obj">the value in the related column of the item to compare to</param>
        /// <returns></returns>
        public override int CompareTo(object obj)
        {
            return string.Compare(val.ToString(), GetFormatDate(obj.ToString()));
        }

        #endregion
    }
    #endregion ���ڷ���(�������ܹ���)

    #region ���ڷ�������
    /// <summary>
    /// ���ڷ�������
    /// </summary>
    public enum DateGroupType
    {
        /// <summary>
        /// �������
        /// </summary>
        Day,
        /// <summary>
        /// �����ڷ���
        /// </summary>
        Week,
        /// <summary>
        /// ���·ݷ���
        /// </summary>
        Month,
        /// <summary>
        /// �����ȷ���
        /// </summary>
        Quarter,
        /// <summary>
        /// ����ȷ���
        /// </summary>
        Year
    }

    #endregion ���ڷ�������
}
