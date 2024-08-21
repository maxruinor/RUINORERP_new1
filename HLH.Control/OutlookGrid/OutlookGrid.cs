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
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace OutlookStyleControls
{
    #region implementation of the OutlookGrid!

    /// <summary>
    /// 分组统计表格控件 outlookGrid1.MyDataBind(ds.Tables[0]);
    /// </summary>
    public partial class OutlookGrid : DataGridView
    {
        #region OutlookGrid constructor
        public OutlookGrid()
        {
            InitializeComponent();

            // very important, this indicates that a new default row class is going to be used to fill the grid
            // in this case our custom OutlookGridRow class
            base.RowTemplate = new OutlookGridRow();
            this.groupTemplate = new OutlookgGridDefaultGroup();

            SetSkinOutlook();
            this.CellClick += new DataGridViewCellEventHandler(OutlookGrid_CellClick);

        }
        #endregion OutlookGrid constructor

        #region OutlookGrid property definitions
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new DataGridViewRow RowTemplate
        {
            get { return base.RowTemplate; }
        }

        private IOutlookGridGroup groupTemplate;
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
        [Category("Appearance")]
        public Image CollapseIcon
        {
            get { return iconCollapse; }
            set { iconCollapse = value; }
        }



        public DataTable SaveDataTable
        {
            get { return _SaveDataTable; }
            set { _SaveDataTable = value; }
        }


        /// <summary>
        /// 要合计的列的集合
        /// </summary>
        private ArrayList SummaryCols;

        private DataTable _SaveDataTable;





        /// <summary>
        /// 合计列: ArrayList summary = new ArrayList();
        /// summary.Add("售价,sum(售价)");
        ///  summary.Add("调拨价,sum(调拨价)");
        /// mySumDataGridView1.SummaryColumns = summary;
        /// </summary>
        [Browsable(true), Category("汇总"), Description("要汇总的字段,及位置,数组")]
        public ArrayList SummaryColumns
        {
            get
            {
                return SummaryCols;
            }
            set
            {
                SummaryCols = value;
            }
        }



        private Image iconExpand;
        [Category("Appearance")]
        public Image ExpandIcon
        {
            get { return iconExpand; }
            set { iconExpand = value; }
        }


        private DataSourceManager dataSource;
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
        #endregion OutlookGrid property definitions

        #region OutlookGrid new methods
        public void CollapseAll()
        {
            SetGroupCollapse(true);
        }

        public void ExpandAll()
        {
            SetGroupCollapse(false);
        }

        public void ClearGroups()
        {
            groupTemplate.Column = null; //reset
            FillGrid(null);
        }

        private void BindData(object dataSource, string dataMember)
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
        public override void Sort(System.Collections.IComparer comparer)
        {
            if (dataSource == null) // if no datasource is set, then bind to the grid itself
                dataSource = new DataSourceManager(this, null);

            dataSource.Sort(comparer);
            FillGrid(groupTemplate);
        }


        public override void Sort(DataGridViewColumn dataGridViewColumn, ListSortDirection direction)
        {
            if (dataSource == null) // if no datasource is set, then bind to the grid itself
                dataSource = new DataSourceManager(this, null);
            dataSource.Sort(new OutlookGridRowComparer(dataGridViewColumn.Index, direction));
            FillGrid(groupTemplate);

        }
        #endregion OutlookGrid new methods

        #region OutlookGrid event handlers
        protected override void OnCellBeginEdit(DataGridViewCellCancelEventArgs e)
        {
            OutlookGridRow row = (OutlookGridRow)base.Rows[e.RowIndex];
            if (row.IsGroupRow)
                e.Cancel = true;
            else
                base.OnCellBeginEdit(e);
        }

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

        // the OnCellMouseDown is overriden so the control can check to see if the
        // user clicked the + or - sign of the group-row
        protected override void OnCellMouseDown(DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return;

            OutlookGridRow row = (OutlookGridRow)base.Rows[e.RowIndex];
            if (row.IsGroupRow && row.IsIconHit(e))
            {
                System.Diagnostics.Debug.WriteLine("OnCellMouseDown " + DateTime.Now.Ticks.ToString());
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
                Columns[index].SortMode = DataGridViewColumnSortMode.Programmatic; // always programmatic!
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
                    row = (OutlookGridRow)this.RowTemplate.Clone();
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

                foreach (DataSourceRow r in list)
                {
                    row = (OutlookGridRow)this.RowTemplate.Clone();
                    result = r[groupingStyle.Column.Index];
                    if (groupCur != null && groupCur.CompareTo(result) == 0) // item is part of the group
                    {
                        row.Group = groupCur;
                        counter++;
                    }
                    else // item is not part of the group, so create new group
                    {
                        if (groupCur != null)
                            groupCur.ItemCount = counter;

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
                    }


                    foreach (object obj in r)
                    {
                        DataGridViewCell cell = new DataGridViewTextBoxCell();
                        cell.Value = obj.ToString();
                        row.Cells.Add(cell);
                    }
                    Rows.Add(row);
                    groupCur.ItemCount = counter;
                    if (groupCur.GroupListText != null)
                    {
                        groupCur.GroupListShowText = GetGroupListResult(groupCur.GroupListText, SaveDataTable, groupingStyle.Column.Name, groupCur.Value);
                    }

                }


            }

        }



        /// <summary>
        /// 得到汇总后的结果
        /// </summary>
        /// <param name="GroupListText"></param>
        /// <param name="dt"></param>
        /// <param name="ColumnsName"></param>
        /// <param name="ColumnsValues"></param>
        /// <returns></returns>
        private ArrayList GetGroupListResult(ArrayList GroupListText, DataTable dt, string ColumnsName, object ColumnsValues)
        {
            ArrayList Result = new ArrayList();
            string[] MyArray = new string[2];
            foreach (String MyString in GroupListText)
            {
                MyArray = MyString.Split(',');
                string Fileter = "";

                if (ColumnsValues != System.DBNull.Value)
                {
                    ///为数字
                    if (dt.Columns[ColumnsName].DataType == typeof(Int32) || dt.Columns[ColumnsName].DataType == typeof(Decimal))
                    { Fileter = ColumnsName + "=" + ColumnsValues.ToString(); }
                    else
                    {
                        if (dt.Columns[ColumnsName].DataType == typeof(string))
                        { Fileter = ColumnsName + "='" + ColumnsValues.ToString() + "'"; }

                    }

                }
                else
                {
                    Fileter = ColumnsName + " is null ";
                }

                Result.Add(MyArray[0] + "," + dt.Compute(MyArray[1], Fileter).ToString());

            }
            return Result;

        }




        #endregion Grid Fill functions

        #endregion implementation of the OutlookGrid!








        #region customer


        // remember the column index that was last sorted on //记住最后一个排序的列索引
        private int prevColIndex = -1;

        // remember the direction the rows were last sorted on (ascending/descending)
        private ListSortDirection prevSortDirection = ListSortDirection.Ascending;

        // specifies the current data view (bound/unbound, dataset)
        private string MyView = "UnboundContactInfo";
        private void OutlookGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //在列上
            if (e.RowIndex < 0 && e.ColumnIndex >= 0)
            {
                ListSortDirection direction = ListSortDirection.Ascending;

                if (e.ColumnIndex == prevColIndex) // reverse sort order
                    direction = prevSortDirection == ListSortDirection.Descending ? ListSortDirection.Ascending : ListSortDirection.Descending;

                // remember the column that was clicked and in which direction is ordered
                prevColIndex = e.ColumnIndex;
                prevSortDirection = direction;

                // set the column to be grouped
                this.GroupTemplate.Column = this.Columns[e.ColumnIndex];


                //设置要汇总的列表信息


                this.groupTemplate.GroupListText = SummaryColumns;



                //sort the grid (based on the selected view)
                switch (MyView)
                {
                    case "BoundContactInfo"://联系人
                        this.Sort(new ContactInfoComparer(e.ColumnIndex, direction));
                        break;
                    case "BoundCategory": //种类科目
                        this.Sort(new DataRowComparer(e.ColumnIndex, direction));
                        break;
                    case "BoundInvoices"://发票,单据
                        this.Sort(new DataRowComparer(e.ColumnIndex, direction));
                        break;
                    case "BoundQuarterly"://季度
                        // this is an example of overriding the default behaviour of the
                        // Group object. Instead of using the DefaultGroup behavious, we
                        // use the AlphabeticGroup, so items are grouped together based on
                        // their first character:
                        // all items starting with A or a will be put in the same group.
                        IOutlookGridGroup prevGroup = this.GroupTemplate;

                        if (e.ColumnIndex == 0) // execption when user pressed the customer name column
                        {
                            // simply override the GroupTemplate to use before sorting
                            this.GroupTemplate = new OutlookGridAlphabeticGroup();
                            this.GroupTemplate.Collapsed = prevGroup.Collapsed;
                        }

                        // set the column to be grouped
                        // this must always be done before sorting
                        this.GroupTemplate.Column = this.Columns[e.ColumnIndex];

                        // execute the sort, arrange and group function
                        this.Sort(new DataRowComparer(e.ColumnIndex, direction));

                        //after sorting, reset the GroupTemplate back to its default (if it was changed)
                        // this is needed just for this demo. We do not want the other
                        // columns to be grouped alphabetically.
                        this.GroupTemplate = prevGroup;
                        break;
                    default: //UnboundContactInfo
                        this.Sort(this.Columns[e.ColumnIndex], direction);
                        break;
                }
            }
        }


        /// <summary>
        /// 绑定数据 /引用本控件的入口
        /// </summary>
        /// <param name="dt"></param>
        public void MyDataBind(DataTable dt)
        {
            // this is an example of adding unbound data into the grid
            // while the grouping mechanism keeps functioning

            // first clear any previous bindings
            this.BindData(null, null);
            SaveDataTable = dt;


            // example of unbound items
            foreach (DataColumn dc in dt.Columns)
            {

                this.Columns.Add(dc.ColumnName, dc.ColumnName);
                this.Columns[dc.ColumnName].ValueType = dc.DataType;
            }

            // example of unbound items
            foreach (DataRow obj in dt.Rows)
            {
                // notice that the outlookgrid only works with OutlookGridRow objects
                OutlookGridRow row = new OutlookGridRow();

                row.CreateCells(this, obj.ItemArray);
                this.Rows.Add(row);
            }

            //set our view for sorting
            MyView = "UnboundContactInfo";
        }

        #region 设置网络外观


        public void SkinsChange(bool flag)
        {
            if (flag)
            {
                SetSkinDefault();
            }
            else
            {
                SetSkinOutlook();
            }
        }
        private void SetSkinDefault()
        {
            this.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;

            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Info;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.DefaultCellStyle = dataGridViewCellStyle2;
            this.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;

            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Desktop;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;

            this.GridColor = System.Drawing.SystemColors.ControlDarkDark;
            this.RowTemplate.Height = 23;
            this.BackgroundColor = System.Drawing.SystemColors.AppWorkspace;
            this.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            this.RowHeadersVisible = true;
            this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
            this.AllowUserToAddRows = true;
            this.AllowUserToDeleteRows = true;
            this.AllowUserToResizeRows = true;
            this.EditMode = DataGridViewEditMode.EditOnF2;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ReadOnly = true;
            this.AllowUserToAddRows = false;

        }

        private void SetSkinOutlook()
        {
            this.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;

            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.DefaultCellStyle = dataGridViewCellStyle2;
            this.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;

            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Desktop;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;

            this.GridColor = System.Drawing.SystemColors.Control;
            this.RowTemplate.Height = 19;
            this.BackgroundColor = System.Drawing.SystemColors.Window;
            this.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            this.RowHeadersVisible = false;
            this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.AllowUserToAddRows = false;
            this.AllowUserToDeleteRows = false;
            this.AllowUserToResizeRows = false;
            this.EditMode = DataGridViewEditMode.EditProgrammatically;
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            //this.ClearGroups(); // reset

        }

        #endregion

        #endregion


        #region Comparers(比较 - used to sort CustomerInfo objects and DataRows of a DataTable

        /// <summary>
        /// reusable custom DataRow comparer implementation, can be used to sort DataTables
        /// </summary>
        public class DataRowComparer : IComparer
        {
            ListSortDirection direction;
            int columnIndex;

            public DataRowComparer(int columnIndex, ListSortDirection direction)
            {
                this.columnIndex = columnIndex;
                this.direction = direction;
            }

            #region IComparer Members

            public int Compare(object x, object y)
            {

                DataRow obj1 = (DataRow)x;
                DataRow obj2 = (DataRow)y;
                return string.Compare(obj1[columnIndex].ToString(), obj2[columnIndex].ToString()) * (direction == ListSortDirection.Ascending ? 1 : -1);
            }
            #endregion
        }

        // custom object comparer implementation
        public class ContactInfoComparer : IComparer
        {
            private int propertyIndex;
            ListSortDirection direction;

            public ContactInfoComparer(int propertyIndex, ListSortDirection direction)
            {
                this.propertyIndex = propertyIndex;
                this.direction = direction;
            }

            #region IComparer Members

            public int Compare(object x, object y)
            {
                ContactInfo obj1 = (ContactInfo)x;
                ContactInfo obj2 = (ContactInfo)y;

                switch (propertyIndex)
                {
                    case 1:
                        return CompareStrings(obj1.Name, obj2.Name);
                    case 2:
                        return CompareDates(obj1.Date, obj2.Date);
                    case 3:
                        return CompareStrings(obj1.Subject, obj2.Subject);
                    case 4:
                        return CompareNumbers(obj1.Concentration, obj2.Concentration);
                    default:
                        return CompareNumbers((double)obj1.Id, (double)obj2.Id);
                }
            }
            #endregion

            private int CompareStrings(string val1, string val2)
            {
                return string.Compare(val1, val2) * (direction == ListSortDirection.Ascending ? 1 : -1);
            }

            private int CompareDates(DateTime val1, DateTime val2)
            {
                if (val1 > val2) return (direction == ListSortDirection.Ascending ? 1 : -1);
                if (val1 < val2) return (direction == ListSortDirection.Ascending ? -1 : 1);
                return 0;
            }

            private int CompareNumbers(double val1, double val2)
            {
                if (val1 > val2) return (direction == ListSortDirection.Ascending ? 1 : -1);
                if (val1 < val2) return (direction == ListSortDirection.Ascending ? -1 : 1);
                return 0;
            }
        }
        #endregion Comparers



        #region ContactInfo - example business object implementation
        public class ContactInfo
        {
            public ContactInfo()
            {
            }
            public ContactInfo(int id, string name, DateTime date, string subject, double con)
            {
                this.id = id;
                this.name = name;
                this.date = date;
                this.subject = subject;
                this.concentration = con;
            }

            private int id;

            public int Id
            {
                get { return id; }
                set { id = value; }
            }
            private string name;

            public string Name
            {
                get { return name; }
                set { name = value; }
            }
            private DateTime date;

            public DateTime Date
            {
                get { return date; }
                set { date = value; }
            }
            private string subject;

            public string Subject
            {
                get { return subject; }
                set { subject = value; }
            }
            private double concentration;

            public double Concentration
            {
                get { return concentration; }
                set { concentration = value; }
            }

        }

        #endregion
    }
}
