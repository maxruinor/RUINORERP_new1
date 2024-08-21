using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;



namespace SHControls.MyDataGrid.SumGrid
{


    /// <summary>
    /// 实现功能有.显示行号,显示总行数,底部显示总计行,突显色,排序不变
    /// 使用方法如下:
    /// DataSet ds = new DataSet();
    /// ds = SHBusiness.Door.DataDoor.GetDataSetQuery("select top 100 d_price as 调拨价,code,item,classno,s_price from tb_product", "tableName");
    /// this.mySumDataGridView1.DataSource = ds;
    /// ArrayList summary = new ArrayList();
    /// summary.Add("0,sum(调拨价)");
    /// summary.Add("4,sum(s_price)");
    /// mySumDataGridView1.SummaryColumns = summary;
    /// mySumDataGridView1.BindDataGrid();
    /// </summary>
    [Obsolete]
    public partial class MySumDataGridView : System.Windows.Forms.DataGridView
    {
        /// <summary>
        /// 要合计的列的集合
        /// </summary>
        private ArrayList SummaryCols;
        private DataView MyDataView;
        private bool Ascending;
        private int SortedColNum;
        public System.Windows.Forms.BindingSource BindingSource1 = new BindingSource();
        #region 属性


        /// <summary>
        /// 合计列: ArrayList summary = new ArrayList();
        /// summary.Add("0,sum(调拨价)");
        /// summary.Add("4,sum(s_price)");
        /// mySumDataGridView1.SummaryColumns = summary;
        /// </summary>
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



        #endregion


        public MySumDataGridView()
        {
            InitializeComponent();
            Ascending = false;
        }

        public MySumDataGridView(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
            this.RowPostPaint += new DataGridViewRowPostPaintEventHandler(MySumDataGridView_RowPostPaint);
            this.CellPainting += new DataGridViewCellPaintingEventHandler(MySumDataGridView_CellPainting);
            this.MouseDown += new MouseEventHandler(MySumDataGridView_MouseDown);
            this.Scroll += new ScrollEventHandler(MySumDataGridView_Scroll);
            SetDataGridView();
        }


        /// <summary>
        /// 设定DataGridView的一些属性
        /// </summary>
        private void SetDataGridView()
        {
            //this.AllowUserToAddRows = false;
            //this.AllowUserToDeleteRows = false;
            //this.AutoGenerateColumns = false;
            //this.SortOrder = SortOrder.None;
            // this.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            //this.MultiSelect = false;
            //this.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            //this.AlternatingRowsDefaultCellStyle.BackColor = SystemColors.InactiveCaptionText;
            this.ReadOnly = true;
            this.BackgroundColor = System.Drawing.SystemColors.AppWorkspace; ;
            //this.RowTemplate.Height = 23;
            //this.TabIndex = 0;

            //System.Windows.Forms.DataGridViewHeaderCell
            //System.Windows.Forms.DataGridViewRowHeaderCell
            //System.Windows.Forms.DataGridViewTopLeftHeaderCell

        }






        private void MySumDataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                DataGridViewPaintParts paintParts =
                    e.PaintParts & ~DataGridViewPaintParts.Focus;

                e.Paint(e.ClipBounds, paintParts);
                e.Handled = true;
            }

            if (e.ColumnIndex < 0 && e.RowIndex >= 0)
            {
                if (e.RowIndex != this.Rows.Count - 1)
                {
                    e.Paint(e.ClipBounds, DataGridViewPaintParts.All);
                    Rectangle indexRect = e.CellBounds;
                    indexRect.Inflate(-2, -2);

                    TextRenderer.DrawText(e.Graphics,
                        (e.RowIndex + 1).ToString(),
                        e.CellStyle.Font,
                        indexRect,
                        e.CellStyle.ForeColor,
                        TextFormatFlags.Right | TextFormatFlags.VerticalCenter);
                    e.Handled = true;
                }

            }

            if (this.Rows.Count > 0)
            {

                if (e.RowIndex == this.Rows.Count - 1 && e.ColumnIndex == -1)
                {
                    this.Rows[e.RowIndex].HeaderCell.OwningRow.DefaultCellStyle.BackColor = Color.MistyRose;

                    e.Paint(e.ClipBounds, DataGridViewPaintParts.All);
                    Rectangle indexRect = e.CellBounds;
                    indexRect.Inflate(-2, -2);

                    TextRenderer.DrawText(e.Graphics,
                        "总计:",
                        e.CellStyle.Font,
                        indexRect,
                        e.CellStyle.ForeColor,
                        TextFormatFlags.Right | TextFormatFlags.VerticalCenter);
                    e.Handled = true;

                }
            }

        }

        private void MySumDataGridView_Scroll(object sender, ScrollEventArgs e)
        {


        }





        private void MySumDataGridView_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            System.Windows.Forms.DataGridView.HitTestInfo MyHitTestInfo;
            MyHitTestInfo = this.HitTest(e.X, e.Y);

            string ColName;

            for (int i = 0; i < this.Columns.Count; i++)
            {
                this.Columns[i].HeaderText = this.Columns[i].Name;
            }


            if (MyHitTestInfo.Type == System.Windows.Forms.DataGridViewHitTestType.ColumnHeader)
            {
                int ColNum = MyHitTestInfo.ColumnIndex;

                if (ColNum != -1)
                {



                    ColName = this.Columns[ColNum].Name;

                    // Perform custom sorting. To do this, always sort the Boolean data type column in
                    // ascending order so that the footer row stays at the end.
                    char[] MyChar = { '↑', '↓' };



                    if (Ascending == true)
                    {
                        MyDataView.Sort = "ID Asc," + ColName + " desc";
                        Ascending = false;
                        this.Columns[ColNum].HeaderText =
                            ColName + " ↑";
                        SortedColNum = ColNum;
                    }
                    else
                    {
                        MyDataView.Sort = "ID Asc," + ColName + " asc";
                        Ascending = true;
                        this.Columns[ColNum].HeaderText =
                             ColName + " ↓";
                        SortedColNum = ColNum;
                    }
                    if (MyDataView.Sort != null)
                    {
                        this.Tag = MyDataView.Sort;
                    }
                }

            }
        }
        //=================


        /// <summary>
        ///数据绑定
        /// </summary>
        public void BindDataGrid()
        {
            DataTable MyDataTable = new DataTable();
            DataRow MyDataRow;
            if (this.DataSource == null)
            {
                MessageBox.Show("数据源不能为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (((DataSet)this.DataSource).Tables.Count == 0)
            {
                MessageBox.Show("数据源没有表集合,显示失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            MyDataTable = ((DataSet)this.DataSource).Tables[0];
            MyDataView = MyDataTable.DefaultView;
            // this.DataSource = MyDataView;

            // Add a Boolean data type column to the DataTable object.
            // You can use this column during your custom sorting.
            MyDataTable.Columns.Add("ID", System.Type.GetType("System.Boolean"));
            MyDataTable.Columns["ID"].DefaultValue = false;
            MyDataTable.Columns["ID"].ColumnMapping = MappingType.Hidden;


            // Create a footer row for the DataTable object.
            MyDataRow = MyDataTable.NewRow();

            // Set the footer value as an empty string for all columns that contains string values.
            for (int MyIterator = 0; MyIterator < this.ColumnCount; MyIterator++)
            {
                if (MyDataTable.Columns[MyIterator].DataType.ToString() == "System.String")
                {
                    MyDataRow[MyIterator] = "";
                }
                switch (MyDataTable.Columns[MyIterator].DataType.ToString())
                {
                    case "System.Boolean":
                    case "System.Byte":
                        {
                            MyDataRow[MyIterator] = false;
                            break;
                        }
                    case "System.Decimal":
                    case "System.Double":
                    case "System.Int16":
                    case "System.Int32":
                        {
                            MyDataRow[MyIterator] = 0;
                            this.Columns[MyIterator].DefaultCellStyle.Format = "N";
                            this.Columns[MyIterator].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            break;
                        }
                    case "System.DateTime":
                        {
                            //MyDataRow[MyIterator] = System.DateTime.Now.ToShortDateString();
                            break;

                        }
                    case "System.String":
                    default:
                        {
                            MyDataRow[MyIterator] = "";
                            break;
                        }
                }
            }


            // Set the value of the footer cell.


            // Calculate the value for each of the cells in the footer.
            string[] MyArray = new string[2];
            foreach (String MyString in SummaryCols)
            {
                MyArray = MyString.Split(',');
                MyDataRow[Convert.ToInt32(MyArray[0])] = MyDataTable.Compute(MyArray[1], "ID is null");
            }


            // Add the footer row to the DataTable object.
            // MyDataTable.Rows.InsertAt(MyDataRow,MyDataTable.Rows.Count - 1);
            if (MyDataTable.Rows.Count > 0)
            {
                MyDataTable.Rows.Add(MyDataRow);
            }
            BindingSource1.DataSource = MyDataTable;
            this.DataSource = BindingSource1;

            MyDataView.ApplyDefaultSort = false;
            MyDataView.AllowNew = false;

            // Associate the ColumnChanged event of the MyDataTable object with the corresponding event handler.
            //this.MyDataTable.ColumnChanged += new DataColumnChangeEventHandler(this.MyDataTable_ColumnChanged);
            //this.Paint += new PaintEventHandler(DataGridControlCS_Paint);

            for (int i = 0; i < this.Columns.Count - 1; i++)
            {
                this.Columns[i].SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;

            }

            // this.Columns["ID"].Visible = false;
        }

















        //==============
        #region 注释了的代码,学习用

        /// <summary>
        /// 显示行号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MySumDataGridView_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            //using (SolidBrush b = new SolidBrush(this.RowHeadersDefaultCellStyle.ForeColor))
            //{
            //    //e.Graphics.DrawString(e.RowIndex.ToString(System.Globalization.CultureInfo.CurrentUICulture), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 20, e.RowBounds.Location.Y + 4);
            //    //this.Rows[e.RowIndex].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            //     if (e.RowIndex == Rows.Count - 1)
            //     {
            //         //e.Graphics.DrawString("总计:", e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            //         this.Rows[this.RowCount - 1].DefaultCellStyle.BackColor = Color.Orange;
            //         this.Rows[e.RowIndex].HeaderCell.Style.Alignment = DataGridViewContentAlignment.BottomCenter;
            //         this.Rows[e.RowIndex].HeaderCell.Value = "总计:";
            //         //e.Graphics.FillRectangle(b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4, 10, 20);
            //     }
            //     else
            //     {
            //         int header = (e.RowIndex + 1);
            //         this.Rows[e.RowIndex].HeaderCell.Value = header.ToString();
            //        // e.Graphics.DrawString(Convert.ToString(e.RowIndex + 1, System.Globalization.CultureInfo.CurrentUICulture), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 20, e.RowBounds.Location.Y + 4);
            //     }
            //}

            this.TopLeftHeaderCell.Value = (this.Rows.Count - 1).ToString("#行");


        }


        #endregion
    }
}



