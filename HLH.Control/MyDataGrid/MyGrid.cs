using System.Drawing;
using System.Windows.Forms;

namespace SHControls.DataGrid
{
    /// <summary>
    /// MyGrid表头自由表格 的摘要说明。
    /// </summary>
    /// 
    public delegate void GridCell_Click(object sender, System.EventArgs e);
    public delegate void GridCell_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e);
    public delegate void GridCell_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e);
    public delegate void GridCell_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e);
    public delegate void GridCell_Enter(object sender, System.EventArgs e);
    public delegate void GridCell_Leave(object sender, System.EventArgs e);

    public delegate void GridCell_MouseEnter(object sender, System.EventArgs e);
    public delegate void GridCell_MouseLeave(object sender, System.EventArgs e);
    public delegate void GridCell_TextChanged(object sender, System.EventArgs e);
    public delegate void GridCell_DoubleClick(object sender, System.EventArgs e);
    public delegate void GridCell_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e);
    public delegate void GridCell_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e);


    //	public event AlarmEventHandler Alarm;

    public partial class MyGrid : System.Windows.Forms.UserControl
    {


        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.Container components = null;
        // 定义新事件
        public event GridCell_MouseDown Cell_MouseDown;
        public event GridCell_MouseUp Cell_MouseUp;
        public event GridCell_MouseEnter Cell_MouseEnter;
        public event GridCell_MouseLeave Cell_MouseLeave;
        public event GridCell_Enter Cell_Enter;
        public event GridCell_Leave Cell_Leave;
        public event GridCell_TextChanged Cell_TextChanged;
        public event GridCell_DoubleClick Cell_DoubleClick;
        public event GridCell_KeyPress Cell_KeyPress;
        public event GridCell_KeyUp Cell_KeyUp;
        public event GridCell_KeyDown Cell_KeyDown;
        public event GridCell_Click Cell_Click;

        //给产生的文本框定义相对位置的纵坐标
        private int locY1;
        //		private int locY ; 
        //		private int locX ; 
        private int borderWidth = 0;
        TextBox[,] ArrCellText;
        //		Label [,] ArrCellText;
        private string setcellWidth = "100,100,100,100,100,100,100,100,100,100,100,100,100,100,100,100";
        private int row;
        private int col;
        private int rows = 2;
        private int cols = 6;
        private int cellColLength = -1; //单元格列间距
        private int cellRowLength = -1; //单元格行间距
        private int cellWidth;
        private int cellTop;
        private int cellLeft;
        private int cellHeight;
        private Color readCellBackColor = Color.White; //只读单元格背景色
        private Color readCellForeColor = Color.Black; //只读单元格前景色
        private Color cellBackColor = Color.White;

        private Color cellForeColor = Color.Black;
        private int gridWidth;
        private int gridHeight;
        private string cellTag;
        private int cellIndex;      //单元格索引
        private System.Windows.Forms.Label lblBorder;
        private System.Windows.Forms.TextBox Cell;

        private bool gridStyle = true;
        private bool borderStyle = false;
        private bool enterToTab = true;
        private int rowHeightMin = 20;
        private float cellFontSize = 9;

        /// <summary>
        /// 设置单元格字体大小
        /// </summary>
        public float CellFontSize
        {
            get { return cellFontSize; }
            set
            {
                cellFontSize = value;
                //				this.Cell.Font = new System.Drawing.Font("宋体", cellFontSize, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(134)));
                ////				Cell.Font.Size =cellFontSize;
            }
        }
        /// <summary>
        /// 设置只读单元格背景色
        /// </summary>
        public Color ReadCellBackColor
        {
            get { return readCellBackColor; }
            set { readCellBackColor = value; }
        }
        /// <summary>
        /// 设置只读单元格前景色
        /// </summary>
        public Color ReadCellForeColor
        {
            get { return readCellForeColor; }
            set { readCellForeColor = value; }
        }
        /// <summary>
        /// 设置单元格背景色
        /// </summary>
        public Color CellBackColor
        {
            get { return cellBackColor; }
            set { cellBackColor = value; }
        }

        /// <summary>
        /// 设置单元格前景色
        /// </summary>
        public Color CellForeColor
        {
            get { return cellForeColor; }
            set { cellForeColor = value; }
        }
        /// <summary>
        /// 在单元格中是否接收回车调用tab键
        /// </summary>
        public bool EnterToTab
        {
            get { return enterToTab; }
            set { enterToTab = value; }
        }

        /// <summary>
        /// 外边框与内边框的 宽度。
        /// </summary>
        public int BorderWidth
        {
            get { return borderWidth; }
            set { borderWidth = value; }
        }

        /// <summary>
        /// 是否有外边框。
        /// </summary>
        public bool BorderStyle
        {
            get { return borderStyle; }
            set { borderStyle = value; }
        }
        /// <summary>
        /// 单元格边框类型。
        /// </summary>
        public bool GridStyle
        {
            get { return gridStyle; }
            set { gridStyle = value; }
        }
        /// <summary>
        /// 得到单元格索引。
        /// </summary>
        public int CellIndex
        {
            get { return cellIndex; }
        }

        /// <summary>
        /// 当前行。
        /// </summary>
        public int Row
        {
            get { return row; }
            set { row = value; }

        }
        /// <summary>
        ///当前列。
        /// </summary>
        public int Col
        {
            get { return col; }
            set { col = value; }

        }
        /// <summary>
        /// 网格行数。
        /// </summary>
        public int Rows
        {
            get { return rows; }
            set { rows = value; } //ShowGrid();

        }
        /// <summary>
        /// 网格列数。
        /// </summary>
        public int Cols
        {

            get { return cols; }
            set { cols = value; } //ShowGrid();
        }
        /// <summary>
        /// 单元格列间距。
        /// </summary>
        public int CellColLength
        {
            get { return cellColLength; }
            set { cellColLength = value; }
        }
        /// <summary>
        /// 单元格行间距。
        /// </summary>
        public int CellRowLength
        {
            get { return cellRowLength; }
            set { cellRowLength = value; }
        }
        /// <summary>
        /// 单元格宽。
        /// </summary>
        public int CellWidth
        {
            get { return cellWidth; }
        }
        /// <summary>
        /// 单元格高。
        /// </summary>
        public int CellHeight
        {
            get { return cellHeight; }
            set { cellHeight = value; }
        }
        /// <summary>
        /// 单元格顶边距。
        /// </summary>
        public int CellTop
        {
            get { return cellTop; }
        }
        /// <summary>
        /// 单元格左边距。
        /// </summary>
        public int CellLeft
        {
            get { return cellLeft; }
        }
        /// <summary>
        /// 网格列宽值信息。
        /// </summary>
        public string SetCellWidth
        {
            get { return setcellWidth; }
            set
            {
                if (value == null)
                    setcellWidth = "100,100,100,100,100,100,100,100,100,100,100,100,100,100,100,100";
                else
                    setcellWidth = value;
            }
        }
        /// <summary>
        /// 最小行高格高。
        /// </summary>
        public int RowHeightMin
        {
            get { return rowHeightMin; }
            set
            {
                rowHeightMin = value;
                Cell.Height = rowHeightMin;
            }
        }
        /// <summary>
        /// 单元格扩展信息。
        /// </summary>
        public string CellTag
        {
            get { return cellTag; }
            set
            {
                if (value != null)
                    cellTag = value;
            }
        }

        public MyGrid()
        {
            // 该调用是 Windows.Forms 窗体设计器所必需的。
            InitializeComponent();
            ArrCellText = new TextBox[10, 10];
            //			ArrCellText=new Label [10,10];

            // TODO: 在 InitComponent 调用后添加任何初始化

        }
        public MyGrid(int r, int c)
        {
            InitializeComponent();
            rows = r;
            cols = c;

        }
        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                    components.Dispose();
            }
            //			for (int j=0;j<rows;j++)
            //			{
            //				for (int i=0;i<cols;i++)
            //				{
            //					ArrCellText[j,i].Dispose() ;
            //				}
            //
            //			}
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码
        /// <summary>
        /// 设计器支持所需的方法 - 不要使用代码编辑器 
        /// 修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.lblBorder = new System.Windows.Forms.Label();
            this.Cell = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblBorder
            // 
            this.lblBorder.BackColor = System.Drawing.Color.Transparent;
            this.lblBorder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblBorder.Location = new System.Drawing.Point(372, 54);
            this.lblBorder.Name = "lblBorder";
            this.lblBorder.Size = new System.Drawing.Size(50, 24);
            this.lblBorder.TabIndex = 9;
            this.lblBorder.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblBorder.Visible = false;
            this.lblBorder.Resize += new System.EventHandler(this.lblBorder_Resize);
            // 
            // Cell
            // 
            this.Cell.AccessibleDescription = "";
            this.Cell.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Cell.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.Cell.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Cell.Location = new System.Drawing.Point(226, 2);
            this.Cell.Name = "Cell";
            this.Cell.Size = new System.Drawing.Size(102, 22);
            this.Cell.TabIndex = 8;
            this.Cell.Text = "我的网格";
            this.Cell.Visible = false;
            this.Cell.Enter += new System.EventHandler(this.txt_Cell_Enter);
            this.Cell.MouseLeave += new System.EventHandler(this.txt_Cell_MouseLeave);
            this.Cell.DoubleClick += new System.EventHandler(this.txt_Cell_DoubleClick);
            this.Cell.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txt_Cell_MouseDown);
            this.Cell.Leave += new System.EventHandler(this.txt_Cell_Leave);
            this.Cell.MouseEnter += new System.EventHandler(this.txt_Cell_MouseEnter);
            this.Cell.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txt_Cell_KeyUp);
            this.Cell.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_Cell_KeyPress);
            this.Cell.TextChanged += new System.EventHandler(this.txt_Cell_TextChanged);
            this.Cell.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_Cell_KeyDown);
            // 
            // MyGrid
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.lblBorder);
            this.Controls.Add(this.Cell);
            this.Name = "MyGrid";
            this.Size = new System.Drawing.Size(128, 40);
            this.Load += new System.EventHandler(this.MyGrid_Load);
            this.Resize += new System.EventHandler(this.MyGrid_Resize);
            this.ResumeLayout(false);

        }
        #endregion

        /// <summary>
        /// 得到可视网格的宽。
        /// </summary>	
        public int GridWidth()
        {
            if (cols < 1)
            { gridWidth = 50; }
            else
            {
                if (borderStyle == true)
                { gridWidth = borderWidth * 2; }
                else
                { gridWidth = 0; }
                for (int i = 0; i < cols; i++)
                {
                    gridWidth += ArrCellText[0, i].Width;
                }
                gridWidth += (cols - 1) * cellColLength;
            }
            return gridWidth;

        }
        /// <summary>
        /// 得到可视网格的高。
        /// </summary>	
        public int GridHeight()
        {
            if (rows < 1)
            {
                gridHeight = 50;
            }
            else
            {
                if (borderStyle == true)
                { gridHeight = borderWidth * 2; }
                else
                { gridHeight = 0; }
                for (int i = 0; i < rows; i++)
                {
                    gridHeight += ArrCellText[i, 0].Height;
                }
                gridHeight += (rows - 1) * cellRowLength;
            }
            return gridHeight;

        }
        /// <summary>
        /// 返回指定的单元格内容。
        /// </summary>

        public string CellText(int r, int c)
        {
            { return ArrCellText[r, c].Text; }

        }

        /// <summary>
        /// 设定单元格的内容。
        /// </summary>	
        public void CellText(int r, int c, string t)
        {
            ArrCellText[r, c].Text = t;
        }
        /// <summary>
        /// 返回指定单元格对象。
        /// </summary>	
        public System.Windows.Forms.TextBox GridCell(int r, int c)
        {
            return ArrCellText[r, c];
        }
        /// <summary>
        /// 返回当前单元格对象。
        /// </summary>	
        public System.Windows.Forms.TextBox GetCell()
        {
            return ArrCellText[row, col];
        }

        /// <summary>
        /// 设定网格列的背景颜色 FALSE 基数列  TRUE偶数列。
        /// </summary>	
        public void SetColBackColor(Color ColBackColor, bool TureOrFalse)
        {
            for (int r = 0; r < ArrCellText.GetUpperBound(0) + 1; r++)
            {
                for (int c = 0; c < ArrCellText.GetUpperBound(1) + 1; c++)
                {
                    if (c % 2 == 0 && TureOrFalse == true)   //可读写列
                    {
                        ArrCellText[r, c].BackColor = ColBackColor;
                    }
                    if (c % 2 != 0 && TureOrFalse == false)   //可读列
                    {
                        ArrCellText[r, c].BackColor = ColBackColor;
                    }

                }

            }
        }


        /// <summary>
        /// 显示网格。
        /// </summary>	
        public void ShowGrid()
        {
            //			this.Controls.Clear();
            //			Label lblBorder=new Label();
            //			lblBorder.Location = new Point ( 0,0) ; 
            //			this.Controls.Add ( lblBorder ) ;
            //			TextBox Cell=new TextBox();
            //			Cell.Location = new Point ( 0,0) ; 
            //			this.Controls.Add ( Cell ) ;
            try
            {

                string s;
                ArrCellText = new TextBox[rows, cols];
                //			ArrCellText=new Label [rows,cols];

                string[] sArr = new string[cols];

                //是否有边框
                if (borderStyle == true)
                {
                    Cell.Left = borderWidth;
                    Cell.Top = borderWidth;
                    lblBorder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                }
                else
                {
                    Cell.Left = 0;
                    Cell.Top = 0;
                    lblBorder.BorderStyle = System.Windows.Forms.BorderStyle.None;
                }
                s = setcellWidth;//"50,200,50,150,50,150,50,200";
                sArr = s.Split(',');
                //是否显示网格
                if (gridStyle == false)
                {
                    Cell.BorderStyle = System.Windows.Forms.BorderStyle.None;
                    //						Cell.Height=Cell.Height+2;
                }
                else
                {
                    Cell.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                }
                for (int j = 0; j < rows; j++)
                {
                    locY1 = borderWidth;
                    for (int i = 0; i < cols; i++)
                    {
                        InitCell(j, i, int.Parse(sArr[i]));
                    }
                    Cell.Top = Cell.Top + Cell.Height + cellRowLength;
                }
                //计算出总的宽度
                int w = 0;
                for (int i = 0; i < cols; i++)
                {
                    w += int.Parse(sArr[i]);
                }
                //计算出最后一列的宽度
                for (int r = 0; r < rows; r++)
                {
                    ArrCellText[r, cols - 1].Width = w - ArrCellText[r, cols - 1].Left;
                }
                lblBorder.SendToBack();
                lblBorder.Width = GridWidth();
                lblBorder.Height = GridHeight();
                this.Width = lblBorder.Width;
                this.Height = lblBorder.Height;
            }
            catch
            {

            }

        }

        /// <summary>
        /// 设置网格大小(宽高)无滚动边框。
        /// </summary>	
        public void GridSize()
        {
            this.Width = GridWidth();
            this.Height = GridHeight();
        }

        /// <summary>
        /// 设置(行列)单元格。
        /// </summary>	
        private void InitCell(int r, int c, int w)
        {
            try
            {
                //创建一个新的TextBox组件
                TextBox myBox = new TextBox();
                //			Label myBox = new Label ( ) ;

                //设定他的名称和Text属性，以及产生的位置
                myBox.Name = "Cell_" + r.ToString() + "_" + c.ToString();
                myBox.Tag = null;//r.ToString() +c.ToString();//保存索引
                myBox.AutoSize = false;

                if (c % 2 == 0)   //可读写列
                {
                    myBox.ReadOnly = true;
                    myBox.HideSelection = true;
                    myBox.Cursor = System.Windows.Forms.Cursors.Arrow;
                    myBox.CausesValidation = true;
                    myBox.TabStop = false;
                    myBox.BackColor = Color.White;
                }
                else  //只读列
                {
                    myBox.ReadOnly = false;
                    myBox.Text = "";
                    myBox.BackColor = readCellBackColor;
                    myBox.ForeColor = readCellForeColor;
                }
                //			myBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                myBox.BorderStyle = Cell.BorderStyle;
                myBox.ForeColor = cellForeColor;
                myBox.BackColor = cellBackColor;
                myBox.Font = this.Font;// new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(134)));
                myBox.Width = w;
                myBox.Height = rowHeightMin;
                myBox.Location = new Point(locY1, Cell.Location.Y);
                //将文本空件添加到控件数组中
                ArrCellText[r, c] = myBox;
                //为产生的新的TextBox组件设定事件
                myBox.Click += new System.EventHandler(this.txt_Cell_Click);
                myBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_Cell_KeyPress);
                myBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txt_Cell_MouseDown);
                myBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.txt_Cell_MouseUp);
                myBox.Enter += new System.EventHandler(this.txt_Cell_Enter);
                myBox.Leave += new System.EventHandler(this.txt_Cell_Leave);
                myBox.MouseEnter += new System.EventHandler(this.txt_Cell_MouseEnter);
                myBox.MouseLeave += new System.EventHandler(this.txt_Cell_MouseLeave);
                myBox.TextChanged += new System.EventHandler(this.txt_Cell_TextChanged);
                myBox.DoubleClick += new System.EventHandler(this.txt_Cell_DoubleClick);
                myBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txt_Cell_KeyUp);
                myBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_Cell_KeyDown);
                //在窗体中显示此文本框
                this.Controls.Add(myBox);
                //对要产生的文本框的纵坐标的相对位置是前一个产生按钮的相对位置的纵坐标加"3
                if (GridStyle == true) { locY1 += w + cellColLength; }
                else { locY1 += w; }
                myBox.BringToFront();    //置前
                myBox.AutoSize = false;   //不与字体大小自动调整单元格大小
            }
            catch
            {
            }

        }

        /// <summary>
        /// 得到网格基本属性值。
        /// </summary>	
        private void GetCellAttribute(object sender)
        {
            //			if (sender.GetType()== typeof(Label))
            //			{
            //				Label control = (Label) sender ;
            //			}
            //			else 
            //			{
            TextBox control = (TextBox)sender;

            //			}			
            string S;
            S = control.Name;

            string[] sArray = S.Split('_');
            row = int.Parse(sArray[1]);
            col = int.Parse(sArray[2]);
            //			cellTag=control.Tag.ToString()  ;
            cellIndex = col;
            cellWidth = control.Width;
            cellTop = control.Location.Y;		//Top ; 
            cellLeft = control.Location.X;	//Left ; 
            cellHeight = control.Height;
        }
        /// <summary>
        /// 得到网格数组
        /// </summary>
        /// <returns></returns>
        public string[,] GetGridArrary()
        {
            string[,] ArrText = null;
            ArrText = new string[rows, cols];
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    ArrText[r, c] = ArrCellText[r, c].Text;
                }
            }
            return ArrText;
        }


        /// <summary>
        /// 定义事件过程。
        /// </summary>	
        private void txt_Cell_MouseEnter(object sender, System.EventArgs e)
        {
            //出箱
            TextBox control = (TextBox)sender;
            //			control.Text=control.Tag.ToString()  ;
            //设定按钮的背景色
            //			control.BackColor = Color.SkyBlue ;
            if (Cell_MouseEnter != null)
            {
                Cell_MouseEnter(sender, e);
            }
        }
        //
        //
        //
        private void txt_Cell_MouseLeave(object sender, System.EventArgs e)
        {
            //			//出箱
            //					TextBox control = ( TextBox ) sender ; 
            //					control.BackColor = Control.DefaultBackColor ;
            if (Cell_MouseLeave != null)
            {
                Cell_MouseLeave(sender, e);
            }
        }

        /// <summary>
        /// 单元格鼠标按下事件过程。
        /// </summary>	
        public void txt_Cell_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (sender.GetType() == typeof(Label))
            {
                Label control = (Label)sender;
            }
            else
            {
                TextBox control = (TextBox)sender;

            }
            //			GetCellAttribute(sender);
            if (Cell_MouseDown != null)
            {
                Cell_MouseDown(sender, e);
            }
        }
        public void txt_Cell_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {

            if (Cell_MouseUp != null)
            {
                Cell_MouseUp(sender, e);
            }
        }
        /// <summary>
        /// 单元格按键事件过程。
        /// </summary>	
        private void txt_Cell_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (sender.GetType() == typeof(Label))
            {
                Label control = (Label)sender;
            }
            else
            {
                TextBox control = (TextBox)sender;

            }

            //			GetCellAttribute(sender);
            if (Cell_KeyPress != null)
            {
                Cell_KeyPress(sender, e);
            }
            if (e.KeyChar == (char)13 && EnterToTab == true)
            { SendKeys.Send("{TAB}"); }

            //				MessageBox.Show ( control.Text + "被按动了！");
        }
        /// <summary>
        /// 单元格单击事件过程。
        /// </summary>	
        private void txt_Cell_Click(object sender, System.EventArgs e)
        {

            if (sender.GetType() == typeof(Label))
            {
                Label control = (Label)sender;
            }
            else
            {
                TextBox control = (TextBox)sender;

            }
            //			GetCellAttribute(sender);
            if (Cell_Click != null)
            {
                Cell_Click(sender, e);
            }
        }
        /// <summary>
        /// 单元格得到焦点事件过程。
        /// </summary>	
        private void txt_Cell_Enter(object sender, System.EventArgs e)
        {
            GetCellAttribute(sender);
            if (Cell_Enter != null)
            {
                Cell_Enter(sender, e);
            }
        }
        /// <summary>
        ///单元格焦点离开事件过程。
        /// </summary>	
        private void txt_Cell_Leave(object sender, System.EventArgs e)
        {
            //			GetCellAttribute(sender);
            if (Cell_Leave != null)
            {
                Cell_Leave(sender, e);
            }
        }



        private void MyGrid_SizeChanged(object sender, System.EventArgs e)
        {

            //			if (this.Width > gridWidth)
            //			{
            //			this.Width = gridWidth;
            //			}
            //			if (this.Height > gridHeight)
            //			{
            //			this.Height  = gridHeight;
            //			}

        }

        private void lblBorder_Resize(object sender, System.EventArgs e)
        {

        }

        private void MyGrid_Resize(object sender, System.EventArgs e)
        {
            //			lblBorder.Top=0;
            //			lblBorder.Left=0;
            //			lblBorder.Width=this.Width ;
            //			lblBorder.Height=this.Height  ;

            //			lblRec.Top =0;
            //			lblRec.Left =0;
            //			lblRec.Width =this.Width ;
            //			lblRec.Height =this.Height ;
        }

        private void txt_Cell_TextChanged(object sender, System.EventArgs e)
        {
            if (Cell_TextChanged != null)
            {
                Cell_TextChanged(sender, e);
            }
        }

        private void txt_Cell_DoubleClick(object sender, System.EventArgs e)
        {
            if (Cell_DoubleClick != null)
            {
                Cell_DoubleClick(sender, e);
            }
        }

        private void txt_Cell_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (Cell_KeyUp != null)
            {
                Cell_KeyUp(sender, e);
            }
        }

        private void txt_Cell_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (Cell_KeyDown != null)
            {
                Cell_KeyDown(sender, e);
            }
        }

        private void MyGrid_Load(object sender, System.EventArgs e)
        {

        }









    }
}
