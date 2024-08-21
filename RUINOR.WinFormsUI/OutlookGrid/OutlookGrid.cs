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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace RUINOR.WinFormsUI.OutlookGrid
{
    #region implementation of the OutlookGrid!
    /// <summary>
    /// �̳���DataGridView�����OutlookGrid
    /// </summary>
    public partial class OutlookGrid : DataGridView
    {
        #region OutlookGrid constructor
        /// <summary>
        /// Ĭ�Ϲ��캯��
        /// </summary>
        public OutlookGrid()
        {
            InitializeComponent();

            // very important, this indicates that a new default row class is going to be used to fill the grid
            // in this case our custom OutlookGridRow class
            base.RowTemplate = new OutlookGridRow();
            this.groupTemplate = new OutlookGridDefaultGroup();

        }
        #endregion OutlookGrid constructor

        #region OutlookGrid property definitions
        /// <summary>
        /// RowTemplate����
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new DataGridViewRow RowTemplate
        {
            get { return base.RowTemplate;}
        }

        private IOutlookGridGroup groupTemplate;
        /// <summary>
        /// ����ӿ�
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IOutlookGridGroup GroupTemplate
        {
            get
            {
                return groupTemplate;
            }
            set
            {
                groupTemplate = value;
            }
        }

        private Image iconCollapse;
        /// <summary>
        /// �۵�ʱ����ʾ��ͼƬ
        /// </summary>
        [Browsable(true), Description("�۵���������ʾ��ͼƬ"), Category("Appearance")]
        public Image CollapseIcon
        {
            get { return iconCollapse; }
            set { iconCollapse = value; }
        }

        private Image iconExpand;
        /// <summary>
        /// չ��ʱ����ʾ��ͼƬ
        /// </summary>
        [Browsable(true), Description("չ����������ʾ��ͼƬ"), Category("Appearance")]
        public Image ExpandIcon
        {
            get { return iconExpand; }
            set { iconExpand = value; }
        }


        private DataSourceManager dataSource;
        /// <summary>
        /// ��д������Դ DataSourceManager����
        /// </summary>
        [Browsable(true), Description("��д������Դ DataSourceManager����"), Category("Data")]
        public new object DataSource
        {
            get
            {
                if (dataSource == null) return null;

                // special case, datasource is bound to itself.
                // for client it must look like no binding is set,so return null in this case
                if (dataSource.DataSource.Equals(this)) return null;

                // return the origional datasource.
                return dataSource.DataSource;
            }
        }

        private int sumColumn = -1;
        /// <summary>
        /// ��������,Ĭ��û��-1
        /// </summary>
        [Browsable(true), Description("��������,Ĭ��û��-1"), Category("Other")]
        public int SumColumn
        {
            get
            {
                return sumColumn;
            }
            set
            {
                sumColumn = value;
            }
        }

        /// <summary>
        /// ���ڷ�������
        /// </summary>
        private DateGroupType groupType = DateGroupType.Day;

        /// <summary>
        /// ���ڷ�������
        /// </summary>
        [Browsable(true), Description("���ڷ�������"), Category("Other")]
        public DateGroupType GroupType
        {
            get { return groupType; }
            set { groupType = value; }
        }

       #endregion OutlookGrid property definitions

        #region OutlookGrid new methods
        /// <summary>
        /// �۵����з���
        /// </summary>
        public void CollapseAll()
        {
            SetGroupCollapse(true);
        }

        /// <summary>
        /// չ�����з���
        /// </summary>
        public void ExpandAll()
        {
            SetGroupCollapse(false);
        }

        /// <summary>
        /// ������з��� ��DataGridView��ʽ��ʾ����
        /// </summary>
        public void ClearGroups()
        {
            groupTemplate.Column = null; //reset
            FillGrid(null);
        }

        /// <summary>
        /// ������Դ�󶨵�OutlookGrid��
        /// </summary>
        /// <param name="dataSource">����Դ</param>
        /// <param name="dataMember">������(����ԴΪDataSetʱ,����д�ò���)</param>
        public void BindData(object dataSource, string dataMember)
        {
            this.DataMember = DataMember;
            if (dataSource == null)
            {
                this.dataSource = null;
                Columns.Clear();
            }
            else
            {
                this.dataSource = new DataSourceManager(dataSource, dataMember);
                SetupColumns();
                FillGrid(null);
            }
        }

        /// <summary>
        /// ��д�����򷽷�
        /// </summary>
        /// <param name="comparer">���˱ȽϽӿ�����</param>
        public override void Sort(System.Collections.IComparer comparer)
        {
            if (dataSource == null) // if no datasource is set, then bind to the grid itself
                dataSource = new DataSourceManager(this, null);

            dataSource.Sort(comparer);
            FillGrid(groupTemplate);
        }

        /// <summary>
        /// ��ָ��������
        /// </summary>
        /// <param name="dataGridViewColumn">DataGridViewColumn����</param>
        /// <param name="direction">����ʽ</param>
        public override void Sort(DataGridViewColumn dataGridViewColumn, ListSortDirection direction)
        {
            if (dataSource == null) // if no datasource is set, then bind to the grid itself
                dataSource = new DataSourceManager(this, null);

            dataSource.Sort(new OutlookGridRowComparer(dataGridViewColumn.Index, direction));
            FillGrid(groupTemplate);
        }
        #endregion OutlookGrid new methods

        #region OutlookGrid event handlers
        /// <summary>
        /// ��д��OnCellBeginEdit�¼�
        /// </summary>
        /// <param name="e"></param>
        protected override void OnCellBeginEdit(DataGridViewCellCancelEventArgs e)
        {
            OutlookGridRow row = (OutlookGridRow)base.Rows[e.RowIndex];
            if (row.IsGroupRow)
                e.Cancel = true;
            else
                base.OnCellBeginEdit(e);
        }

        /// <summary>
        /// ��дOnCellDoubleClick�¼�,���������
        /// </summary>
        /// <param name="e"></param>
        protected override void OnCellDoubleClick(DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                OutlookGridRow row = (OutlookGridRow)base.Rows[e.RowIndex];
                if (row.IsGroupRow)
                {
                    row.Group.Collapsed = !row.Group.Collapsed;

                    //this is a workaround to make the grid re-calculate it's contents and backgroun bounds
                    // so the background is updated correctly.
                    // this will also invalidate the control, so it will redraw itself
                    row.Visible = false;
                    row.Visible = true;
                    return;
                }
            }
            base.OnCellClick(e);
        }

        /// <summary>
        ///  the OnCellMouseDown is overriden so the control can check to see if the
        ///  user clicked the + or - sign of the group-row
        /// </summary>
        /// <param name="e"></param>
        protected override void OnCellMouseDown(DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return;

            OutlookGridRow row = (OutlookGridRow)base.Rows[e.RowIndex];
            if (row.IsGroupRow && row.IsIconHit(e))
            {
                //System.Diagnostics.Debug.WriteLine("OnCellMouseDown " + DateTime.Now.Ticks.ToString());
                row.Group.Collapsed = !row.Group.Collapsed;

                //this is a workaround to make the grid re-calculate it's contents and backgroun bounds
                // so the background is updated correctly.
                // this will also invalidate the control, so it will redraw itself
                row.Visible = false;
                row.Visible = true;
            }
            else
                base.OnCellMouseDown(e);
        }
        #endregion OutlookGrid event handlers

        #region Grid Fill functions
        /// <summary>
        /// �۵�/չ������
        /// </summary>
        /// <param name="collapsed"></param>
        private void SetGroupCollapse(bool collapsed)
        {
            if (Rows.Count == 0) return;
            if (groupTemplate == null) return;

            // set the default grouping style template collapsed property
            groupTemplate.Collapsed = collapsed;

            // loop through all rows to find the GroupRows
            foreach (OutlookGridRow row in Rows)
            {
                if (row.IsGroupRow)
                    row.Group.Collapsed = collapsed;
            }

            // workaround, make the grid refresh properly
            Rows[0].Visible = !Rows[0].Visible;
            Rows[0].Visible = !Rows[0].Visible;
        }

        /// <summary>
        /// ������
        /// </summary>
        private void SetupColumns()
        {
            ArrayList list;

            // clear all columns, this is a somewhat crude implementation
            // refinement may be welcome.
            Columns.Clear();

            // start filling the grid
            if (dataSource == null)
                return;
            else
                list = dataSource.Rows;
            if (list.Count <= 0) return;

            foreach (string c in dataSource.Columns)
            {
                int index;
                DataGridViewColumn column = Columns[c];
                if (column == null)
                    index = Columns.Add(c, c);
                else
                    index = column.Index;
                Columns[index].SortMode = DataGridViewColumnSortMode.Programmatic; // ��̷�ʽ���� always programmatic!
            }

        }

        /// <summary>
        /// the fill grid method fills the grid with the data from the DataSourceManager
        /// It takes the grouping style into account, if it is set.
        /// </summary>
        private void FillGrid(IOutlookGridGroup groupingStyle)
        {
            ArrayList list;
            OutlookGridRow row;

            this.Rows.Clear();

            // start filling the grid
            if (dataSource == null) 
                return; 
            else
                list = dataSource.Rows;
            if (list.Count <= 0) return;

            // this block is used of grouping is turned off
            // this will simply list all attributes of each object in the list
            if (groupingStyle == null)
            {
                foreach (DataSourceRow r in list)
                {
                    row = (OutlookGridRow) this.RowTemplate.Clone(); 
                    foreach (object val in r)
                    {
                        DataGridViewCell cell = new DataGridViewTextBoxCell();
                        cell.Value = val.ToString();
                        row.Cells.Add(cell);
                    }
                    Rows.Add(row);
                }
            }

            // this block is used when grouping is used
            // items in the list must be sorted, and then they will automatically be grouped
            else
            {
                IOutlookGridGroup groupCur = null;
                object result = null;
                int counter = 0; // counts number of items in the group
                decimal total = 0;// �ϼƽ��

                foreach (DataSourceRow r in list)
                {
                    row = (OutlookGridRow)this.RowTemplate.Clone();
                    result = r[groupingStyle.Column.Index];
                    if (groupCur != null && groupCur.CompareTo(result) == 0) // item is part of the group
                    {
                        row.Group = groupCur;
                        counter++;
                        if (sumColumn >= 0)
                        {
                            total += decimal.Parse(r[sumColumn].ToString());
                        }
                    }
                    else // item is not part of the group, so create new group
                    {
                        if (groupCur != null)
                        {
                            groupCur.ItemCount = counter;
                            if (sumColumn >= 0)
                            {
                                if (groupCur is OutlookGridDateGroup)
                                {
                                    ((OutlookGridDateGroup)groupCur).SumColumn = sumColumn;
                                    ((OutlookGridDateGroup)groupCur).Total = total;
                                    ((OutlookGridDateGroup)groupCur).GroupType = groupType;
                                }
                                else
                                {
                                    ((OutlookGridMoneyGroup)groupCur).SumColumn = sumColumn;
                                    ((OutlookGridMoneyGroup)groupCur).Total = total;
                                }
                            }
                        }

                        groupCur = (IOutlookGridGroup)groupingStyle.Clone(); // init
                        groupCur.Value = result;
                        row.Group = groupCur;
                        row.IsGroupRow = true;
                        row.Height = groupCur.Height;
                        row.CreateCells(this, groupCur.Value);
                        Rows.Add(row);

                        // add content row after this
                        row = (OutlookGridRow)this.RowTemplate.Clone();
                        row.Group = groupCur;
                        counter = 1; // reset counter for next group
                        total = decimal.Parse(r[sumColumn].ToString()); //���ý����Ŀ
                    }


                    foreach (object obj in r)
                    {
                        DataGridViewCell cell = new DataGridViewTextBoxCell();
                        cell.Value = obj.ToString();
                        row.Cells.Add(cell);
                    }
                    Rows.Add(row);
                    groupCur.ItemCount = counter;
                    if (sumColumn >= 0)
                    {
                        if (groupCur is OutlookGridDateGroup)
                        {
                            ((OutlookGridDateGroup)groupCur).SumColumn = sumColumn;
                            ((OutlookGridDateGroup)groupCur).Total = total;
                            ((OutlookGridDateGroup)groupCur).GroupType = groupType;
                        }
                        else
                        {
                            ((OutlookGridMoneyGroup)groupCur).SumColumn = sumColumn;
                            ((OutlookGridMoneyGroup)groupCur).Total = total;
                        }
                    }

                }
            }

        }
        #endregion Grid Fill functions
    }
    #endregion implementation of the OutlookGrid!
}
