using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Windows.Forms;




namespace SHControls.MyDataGrid
{
    /// <summary>
    /// Summary description for UserControl1.
    /// </summary>

    // Declare a delegate for the event that disables the cells of the DataGrid control.
    public delegate void DataGridDisableCellHandler(object sender, DataGridDisableCellEventArgs e);

    public class DataGridControlCS : System.Windows.Forms.DataGrid
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        // Declare private variables for your DataGrid control.
        private int RowCount;
        private int ColCount;
        private int SortedColNum;

        private bool Ascending;
        private bool CellValueChanged;

        private string SourceTable;

        private DataView MyDataView;
        private DataSet MyDataSet;
        private DataRow MyDataRow;
        private DataTable MyDataTable;

        private ArrayList SummaryCols;
        private DataGridCell CurrentDataGridCellLocation;

        private static Brush FooterBackColor;
        private static Brush FooterForeColor;



        public DataGridControlCS()
        {
            InitializeComponent();

            RowCount = 0;
            ColCount = 0;

            CellValueChanged = false;
            Ascending = false;

            MyDataRow = null;

            MyDataTable = new DataTable("NewTable");
            CurrentDataGridCellLocation = new DataGridCell();
            SummaryCols = new ArrayList();

        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                    components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // Associate events with the corresponding event handlers.
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DataGridControlCS_MouseDown);
            this.CurrentCellChanged += new System.EventHandler(this.DataGridControlCS_CurrentCellChanged);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
        }
        #endregion


        #region 属性
        public DataSet GridDataSet
        {
            set
            {
                MyDataSet = value;
            }
        }

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

        public string DataSourceTable
        {
            get
            {
                return SourceTable;
            }
            set
            {
                SourceTable = value;
            }
        }

        public static Brush FooterColor
        {
            get
            {
                return FooterBackColor;
            }
            set
            {
                FooterBackColor = value;
            }
        }

        public static Brush FooterFontColor
        {
            get
            {
                return FooterForeColor;
            }
            set
            {
                FooterForeColor = value;
            }
        }

        #endregion


        private void DataGridControlCS_Paint(object sender, PaintEventArgs e)
        {
            string text;
            for (int i = 1; i < RowCount + 1; i++)
            {

                int X = this.GetCellBounds(i - 1, 0).X - this.RowHeaderWidth + 1;
                int Y = this.GetCellBounds(i - 1, 0).Y + 1;

                if (i == RowCount)
                {
                    text = "总计:";
                }
                else
                {
                    text = i.ToString();
                }
                e.Graphics.DrawString(text, this.Font, new SolidBrush(Color.Black), X, Y);
            }

        }


        /// <summary>
        ///数据绑定
        /// </summary>
        public void BindDataGrid()
        {
            MyDataTable = ((DataSet)this.DataSource).Tables[0];
            MyDataView = MyDataTable.DefaultView;
            this.DataSource = MyDataView;

            DataGridTableStyle TableStyle = new DataGridTableStyle();
            TableStyle.MappingName = DataSourceTable;

            // Add a Boolean data type column to the DataTable object.
            // You can use this column during your custom sorting.
            MyDataTable.Columns.Add("ID", System.Type.GetType("System.Boolean"));
            MyDataTable.Columns["ID"].DefaultValue = false;
            MyDataTable.Columns["ID"].ColumnMapping = MappingType.Hidden;
            ColCount = MyDataTable.Columns.Count;

            // Create a footer row for the DataTable object.
            MyDataRow = MyDataTable.NewRow();

            // Set the footer value as an empty string for all columns that contains string values.
            for (int MyIterator = 0; MyIterator < ColCount; MyIterator++)
            {
                if (MyDataTable.Columns[MyIterator].DataType.ToString() == "System.String")
                {
                    MyDataRow[MyIterator] = "";
                }
            }

            // Add the footer row to the DataTable object.
            MyDataTable.Rows.Add(MyDataRow);
            RowCount = MyDataTable.Rows.Count;

            // Add a MyDataGridTextBox control to each cell of the DataGrid control.
            MyDataGridTextBox TempDataGridTextBox;
            for (int MyIterator = 0; MyIterator < ColCount - 1; MyIterator++)
            {
                TempDataGridTextBox = new MyDataGridTextBox(MyIterator);
                TempDataGridTextBox.HeaderText = MyDataTable.Columns[MyIterator].ColumnName;
                TempDataGridTextBox.MappingName = MyDataTable.Columns[MyIterator].ColumnName;
                TempDataGridTextBox.DataGridDisableCell += new DataGridDisableCellHandler(SetEnableValues);

                // Disable the default sorting feature of the DataGrid control.
                TableStyle.AllowSorting = false;
                TableStyle.GridColumnStyles.Add(TempDataGridTextBox);
            }

            this.TableStyles.Add(TableStyle);
            this.DataSource = MyDataView;
            MyDataView.ApplyDefaultSort = false;
            MyDataView.AllowNew = false;

            // Set the value of the footer cell.
            DataGridCell MyCell = new DataGridCell();
            MyCell.RowNumber = MyDataTable.Rows.Count - 1;


            // Calculate the value for each of the cells in the footer.
            string[] MyArray = new string[2];
            foreach (String MyString in SummaryCols)
            {
                MyArray = MyString.Split(',');
                MyCell.ColumnNumber = Convert.ToInt32(MyArray[0]);
                this[MyCell] = MyDataTable.Compute(MyArray[1], "ID is null").ToString();
            }

            // Associate the ColumnChanged event of the MyDataTable object with the corresponding event handler.
            this.MyDataTable.ColumnChanged += new DataColumnChangeEventHandler(this.MyDataTable_ColumnChanged);
            this.Paint += new PaintEventHandler(DataGridControlCS_Paint);

        }

        // Handle the DataTable object's ColumnChanged event
        // to track whether the value in a cell has changed.
        private void MyDataTable_ColumnChanged(object sender, DataColumnChangeEventArgs e)
        {
            int Row, Col;
            Row = 0;
            Col = 0;

            // Determine the row that contains the changed cell.
            foreach (DataRow TempDataRow in MyDataTable.Rows)
            {
                if (TempDataRow.Equals(e.Row))
                {
                    CurrentDataGridCellLocation.RowNumber = Row;
                    CellValueChanged = true;
                    break;
                }
                Row++;
            }

            // Determine the column that contains the changed cell.
            foreach (DataColumn TempDataColumn in MyDataTable.Columns)
            {
                if (TempDataColumn.Equals(e.Column))
                {
                    CurrentDataGridCellLocation.ColumnNumber = Col;
                    CellValueChanged = true;
                    break;
                }
                Col++;
            }
        }

        // Handle the CurrentCellChanged event of the DataGrid control.
        private void DataGridControlCS_CurrentCellChanged(object sender, System.EventArgs e)
        {
            if (CellValueChanged == true)
            {
                DataGridCell MyCell = new DataGridCell();
                MyCell.RowNumber = MyDataTable.Rows.Count - 1;

                // Calculate the value for each cell in the footer.
                string[] MyArray = new string[2];
                foreach (String MyString in SummaryCols)
                {
                    MyArray = MyString.Split(',');
                    MyCell.ColumnNumber = Convert.ToInt32(MyArray[0]);
                    this[MyCell] = MyDataTable.Compute(MyArray[1], "ID is null").ToString();
                }
            }
            CellValueChanged = false;
        }

        //// Handle the MouseDown event to perform custom sorting.
        private void DataGridControlCS_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            System.Windows.Forms.DataGrid.HitTestInfo MyHitTestInfo;
            MyHitTestInfo = this.HitTest(e.X, e.Y);

            string ColName;

            if (MyHitTestInfo.Type == System.Windows.Forms.DataGrid.HitTestType.ColumnHeader)
            {
                int ColNum = MyHitTestInfo.Column;

                if (ColNum != -1)
                {
                    ColName = MyDataTable.Columns[ColNum].ColumnName;

                    // Perform custom sorting. To do this, always sort the Boolean data type column in
                    // ascending order so that the footer row stays at the end.
                    char[] MyChar = { '◆', '□' };

                    string NewString = this.TableStyles[0].GridColumnStyles[SortedColNum].HeaderText.TrimEnd(MyChar).Trim();
                    this.TableStyles[0].GridColumnStyles[SortedColNum].HeaderText = NewString;

                    if (Ascending == true)
                    {
                        MyDataView.Sort = "ID Asc," + ColName + " desc";
                        Ascending = false;
                        this.TableStyles[0].GridColumnStyles[ColNum].HeaderText =
                            this.TableStyles[0].GridColumnStyles[ColNum].HeaderText + " ◆";
                        SortedColNum = ColNum;
                    }
                    else
                    {
                        MyDataView.Sort = "ID Asc," + ColName + " asc";
                        Ascending = true;
                        this.TableStyles[0].GridColumnStyles[ColNum].HeaderText =
                            this.TableStyles[0].GridColumnStyles[ColNum].HeaderText + " □";
                        SortedColNum = ColNum;
                    }
                }
            }
        }

        // Disable the footer row of the DataGrid control.
        public void SetEnableValues(object sender, DataGridDisableCellEventArgs e)
        {
            // Disable a footer cell of the DataGrid control.
            if (e.Row == RowCount - 1)
            {
                e.EnableValue = false;
            }
            else
            {
                e.EnableValue = true;
            }
        }
    }

    // Define a custom event arguments class that inherits from the EventArgs class.
    public class DataGridDisableCellEventArgs : EventArgs
    {
        private int MyCol;
        private int MyRow;
        private bool MyEnableValue;

        public DataGridDisableCellEventArgs(int Row, int Col)
        {
            MyRow = Row;
            MyCol = Col;
            MyEnableValue = true;
        }

        public int Column
        {
            get
            {
                return MyCol;
            }
            set
            {
                MyCol = value;
            }
        }

        public int Row
        {
            get
            {
                return MyRow;
            }
            set
            {
                MyRow = value;
            }
        }

        public bool EnableValue
        {
            get
            {
                return MyEnableValue;
            }
            set
            {
                MyEnableValue = value;
            }
        }
    }

    public class MyDataGridTextBox : DataGridTextBoxColumn
    {
        // Declare an event for the DataGridDisableCellEventHandler delegate that you have defined.
        public event DataGridDisableCellHandler DataGridDisableCell;

        private int MyCol;

        // Save the column number of the column to add the MyDataGridTextBox control to.
        public MyDataGridTextBox(int column)
        {
            MyCol = column;
        }

        // Override the Paint method to set colors for the footer row.
        protected override void Paint(
            System.Drawing.Graphics g,
            System.Drawing.Rectangle bounds,
            System.Windows.Forms.CurrencyManager source,
            int rowNum,
            System.Drawing.Brush backBrush,
            System.Drawing.Brush foreBrush,
            bool alignToRight)
        {
            if (DataGridDisableCell != null)
            {
                // Initialize the event arguments by using the number
                // of the current row and the current column.
                DataGridDisableCellEventArgs e = new DataGridDisableCellEventArgs(rowNum, MyCol);

                // Raise the DataGridDisableCell event.
                DataGridDisableCell(this, e);

                // Set the foreground color and the background color for the footer row.
                if (!e.EnableValue)
                {
                    if ((DataGridControlCS.FooterColor == null) || (DataGridControlCS.FooterFontColor == null))
                    {
                        //backBrush = Brushes.White;
                        //foreBrush = Brushes.Black;
                        backBrush = Brushes.Orange;
                        foreBrush = Brushes.Black;

                    }
                    else
                    {
                        backBrush = DataGridControlCS.FooterColor;
                        foreBrush = DataGridControlCS.FooterFontColor;
                    }
                }
            }

            // Call the Paint event of the DataGridTextBoxColumn class.
            base.Paint(g, bounds, source, rowNum, backBrush, foreBrush, alignToRight);
        }

        // Override the Edit method to disable the footer row.
        protected override void Edit(
            System.Windows.Forms.CurrencyManager source,
            int rowNum,
            System.Drawing.Rectangle bounds,
            bool readOnlyFlag,
            string instantText,
            bool cellIsVisible)
        {
            DataGridDisableCellEventArgs e = null;

            if (DataGridDisableCell != null)
            {
                // Initialize the event arguments by using the number
                // of the current row and the current column.
                e = new DataGridDisableCellEventArgs(rowNum, MyCol);

                // Raise the DataGridDisableCell event.
                DataGridDisableCell(this, e);
            }

            // Call the Edit event of the DataGridTextBoxColumn
            // class for all rows other than the footer row.
            if (e.EnableValue)
            {
                base.Edit(source, rowNum, bounds, readOnlyFlag, instantText, cellIsVisible);
            }
        }
    }
}