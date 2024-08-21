using System.Drawing;
using System.Windows.Forms;

namespace SHControls.DataGrid
{
    /// <summary>
    /// MyGrid��ͷ���ɱ�� ��ժҪ˵����
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
        /// ����������������
        /// </summary>
        private System.ComponentModel.Container components = null;
        // �������¼�
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

        //���������ı��������λ�õ�������
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
        private int cellColLength = -1; //��Ԫ���м��
        private int cellRowLength = -1; //��Ԫ���м��
        private int cellWidth;
        private int cellTop;
        private int cellLeft;
        private int cellHeight;
        private Color readCellBackColor = Color.White; //ֻ����Ԫ�񱳾�ɫ
        private Color readCellForeColor = Color.Black; //ֻ����Ԫ��ǰ��ɫ
        private Color cellBackColor = Color.White;

        private Color cellForeColor = Color.Black;
        private int gridWidth;
        private int gridHeight;
        private string cellTag;
        private int cellIndex;      //��Ԫ������
        private System.Windows.Forms.Label lblBorder;
        private System.Windows.Forms.TextBox Cell;

        private bool gridStyle = true;
        private bool borderStyle = false;
        private bool enterToTab = true;
        private int rowHeightMin = 20;
        private float cellFontSize = 9;

        /// <summary>
        /// ���õ�Ԫ�������С
        /// </summary>
        public float CellFontSize
        {
            get { return cellFontSize; }
            set
            {
                cellFontSize = value;
                //				this.Cell.Font = new System.Drawing.Font("����", cellFontSize, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(134)));
                ////				Cell.Font.Size =cellFontSize;
            }
        }
        /// <summary>
        /// ����ֻ����Ԫ�񱳾�ɫ
        /// </summary>
        public Color ReadCellBackColor
        {
            get { return readCellBackColor; }
            set { readCellBackColor = value; }
        }
        /// <summary>
        /// ����ֻ����Ԫ��ǰ��ɫ
        /// </summary>
        public Color ReadCellForeColor
        {
            get { return readCellForeColor; }
            set { readCellForeColor = value; }
        }
        /// <summary>
        /// ���õ�Ԫ�񱳾�ɫ
        /// </summary>
        public Color CellBackColor
        {
            get { return cellBackColor; }
            set { cellBackColor = value; }
        }

        /// <summary>
        /// ���õ�Ԫ��ǰ��ɫ
        /// </summary>
        public Color CellForeColor
        {
            get { return cellForeColor; }
            set { cellForeColor = value; }
        }
        /// <summary>
        /// �ڵ�Ԫ�����Ƿ���ջس�����tab��
        /// </summary>
        public bool EnterToTab
        {
            get { return enterToTab; }
            set { enterToTab = value; }
        }

        /// <summary>
        /// ��߿����ڱ߿�� ��ȡ�
        /// </summary>
        public int BorderWidth
        {
            get { return borderWidth; }
            set { borderWidth = value; }
        }

        /// <summary>
        /// �Ƿ�����߿�
        /// </summary>
        public bool BorderStyle
        {
            get { return borderStyle; }
            set { borderStyle = value; }
        }
        /// <summary>
        /// ��Ԫ��߿����͡�
        /// </summary>
        public bool GridStyle
        {
            get { return gridStyle; }
            set { gridStyle = value; }
        }
        /// <summary>
        /// �õ���Ԫ��������
        /// </summary>
        public int CellIndex
        {
            get { return cellIndex; }
        }

        /// <summary>
        /// ��ǰ�С�
        /// </summary>
        public int Row
        {
            get { return row; }
            set { row = value; }

        }
        /// <summary>
        ///��ǰ�С�
        /// </summary>
        public int Col
        {
            get { return col; }
            set { col = value; }

        }
        /// <summary>
        /// ����������
        /// </summary>
        public int Rows
        {
            get { return rows; }
            set { rows = value; } //ShowGrid();

        }
        /// <summary>
        /// ����������
        /// </summary>
        public int Cols
        {

            get { return cols; }
            set { cols = value; } //ShowGrid();
        }
        /// <summary>
        /// ��Ԫ���м�ࡣ
        /// </summary>
        public int CellColLength
        {
            get { return cellColLength; }
            set { cellColLength = value; }
        }
        /// <summary>
        /// ��Ԫ���м�ࡣ
        /// </summary>
        public int CellRowLength
        {
            get { return cellRowLength; }
            set { cellRowLength = value; }
        }
        /// <summary>
        /// ��Ԫ���
        /// </summary>
        public int CellWidth
        {
            get { return cellWidth; }
        }
        /// <summary>
        /// ��Ԫ��ߡ�
        /// </summary>
        public int CellHeight
        {
            get { return cellHeight; }
            set { cellHeight = value; }
        }
        /// <summary>
        /// ��Ԫ�񶥱߾ࡣ
        /// </summary>
        public int CellTop
        {
            get { return cellTop; }
        }
        /// <summary>
        /// ��Ԫ����߾ࡣ
        /// </summary>
        public int CellLeft
        {
            get { return cellLeft; }
        }
        /// <summary>
        /// �����п�ֵ��Ϣ��
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
        /// ��С�и߸�ߡ�
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
        /// ��Ԫ����չ��Ϣ��
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
            // �õ����� Windows.Forms ���������������ġ�
            InitializeComponent();
            ArrCellText = new TextBox[10, 10];
            //			ArrCellText=new Label [10,10];

            // TODO: �� InitComponent ���ú�����κγ�ʼ��

        }
        public MyGrid(int r, int c)
        {
            InitializeComponent();
            rows = r;
            cols = c;

        }
        /// <summary>
        /// ������������ʹ�õ���Դ��
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

        #region �����������ɵĴ���
        /// <summary>
        /// �����֧������ķ��� - ��Ҫʹ�ô���༭�� 
        /// �޸Ĵ˷��������ݡ�
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
            this.Cell.Font = new System.Drawing.Font("����", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Cell.Location = new System.Drawing.Point(226, 2);
            this.Cell.Name = "Cell";
            this.Cell.Size = new System.Drawing.Size(102, 22);
            this.Cell.TabIndex = 8;
            this.Cell.Text = "�ҵ�����";
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
        /// �õ���������Ŀ�
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
        /// �õ���������ĸߡ�
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
        /// ����ָ���ĵ�Ԫ�����ݡ�
        /// </summary>

        public string CellText(int r, int c)
        {
            { return ArrCellText[r, c].Text; }

        }

        /// <summary>
        /// �趨��Ԫ������ݡ�
        /// </summary>	
        public void CellText(int r, int c, string t)
        {
            ArrCellText[r, c].Text = t;
        }
        /// <summary>
        /// ����ָ����Ԫ�����
        /// </summary>	
        public System.Windows.Forms.TextBox GridCell(int r, int c)
        {
            return ArrCellText[r, c];
        }
        /// <summary>
        /// ���ص�ǰ��Ԫ�����
        /// </summary>	
        public System.Windows.Forms.TextBox GetCell()
        {
            return ArrCellText[row, col];
        }

        /// <summary>
        /// �趨�����еı�����ɫ FALSE ������  TRUEż���С�
        /// </summary>	
        public void SetColBackColor(Color ColBackColor, bool TureOrFalse)
        {
            for (int r = 0; r < ArrCellText.GetUpperBound(0) + 1; r++)
            {
                for (int c = 0; c < ArrCellText.GetUpperBound(1) + 1; c++)
                {
                    if (c % 2 == 0 && TureOrFalse == true)   //�ɶ�д��
                    {
                        ArrCellText[r, c].BackColor = ColBackColor;
                    }
                    if (c % 2 != 0 && TureOrFalse == false)   //�ɶ���
                    {
                        ArrCellText[r, c].BackColor = ColBackColor;
                    }

                }

            }
        }


        /// <summary>
        /// ��ʾ����
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

                //�Ƿ��б߿�
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
                //�Ƿ���ʾ����
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
                //������ܵĿ��
                int w = 0;
                for (int i = 0; i < cols; i++)
                {
                    w += int.Parse(sArr[i]);
                }
                //��������һ�еĿ��
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
        /// ���������С(���)�޹����߿�
        /// </summary>	
        public void GridSize()
        {
            this.Width = GridWidth();
            this.Height = GridHeight();
        }

        /// <summary>
        /// ����(����)��Ԫ��
        /// </summary>	
        private void InitCell(int r, int c, int w)
        {
            try
            {
                //����һ���µ�TextBox���
                TextBox myBox = new TextBox();
                //			Label myBox = new Label ( ) ;

                //�趨�������ƺ�Text���ԣ��Լ�������λ��
                myBox.Name = "Cell_" + r.ToString() + "_" + c.ToString();
                myBox.Tag = null;//r.ToString() +c.ToString();//��������
                myBox.AutoSize = false;

                if (c % 2 == 0)   //�ɶ�д��
                {
                    myBox.ReadOnly = true;
                    myBox.HideSelection = true;
                    myBox.Cursor = System.Windows.Forms.Cursors.Arrow;
                    myBox.CausesValidation = true;
                    myBox.TabStop = false;
                    myBox.BackColor = Color.White;
                }
                else  //ֻ����
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
                myBox.Font = this.Font;// new System.Drawing.Font("����", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(134)));
                myBox.Width = w;
                myBox.Height = rowHeightMin;
                myBox.Location = new Point(locY1, Cell.Location.Y);
                //���ı��ռ���ӵ��ؼ�������
                ArrCellText[r, c] = myBox;
                //Ϊ�������µ�TextBox����趨�¼�
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
                //�ڴ�������ʾ���ı���
                this.Controls.Add(myBox);
                //��Ҫ�������ı��������������λ����ǰһ��������ť�����λ�õ��������"3
                if (GridStyle == true) { locY1 += w + cellColLength; }
                else { locY1 += w; }
                myBox.BringToFront();    //��ǰ
                myBox.AutoSize = false;   //���������С�Զ�������Ԫ���С
            }
            catch
            {
            }

        }

        /// <summary>
        /// �õ������������ֵ��
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
        /// �õ���������
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
        /// �����¼����̡�
        /// </summary>	
        private void txt_Cell_MouseEnter(object sender, System.EventArgs e)
        {
            //����
            TextBox control = (TextBox)sender;
            //			control.Text=control.Tag.ToString()  ;
            //�趨��ť�ı���ɫ
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
            //			//����
            //					TextBox control = ( TextBox ) sender ; 
            //					control.BackColor = Control.DefaultBackColor ;
            if (Cell_MouseLeave != null)
            {
                Cell_MouseLeave(sender, e);
            }
        }

        /// <summary>
        /// ��Ԫ����갴���¼����̡�
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
        /// ��Ԫ�񰴼��¼����̡�
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

            //				MessageBox.Show ( control.Text + "�������ˣ�");
        }
        /// <summary>
        /// ��Ԫ�񵥻��¼����̡�
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
        /// ��Ԫ��õ������¼����̡�
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
        ///��Ԫ�񽹵��뿪�¼����̡�
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
