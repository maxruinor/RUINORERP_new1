using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using SourceGrid2.Cells;
using System.Drawing.Printing;
//using Excel;
using System.Runtime.InteropServices;
namespace SourceGrid2
{
	/// <summary>
	/// The mai grid control with static data.
	/// </summary>
	[System.ComponentModel.ToolboxItem(true)]
	[Description("牛B的格子.")]
	public class Grid : GridVirtual
	{
		#region Constructor
		/// <summary>
		/// Constructor
		/// </summary>
		public Grid()
		{
			this.Name = "Grid";
			this.Size = new Size(100,100);
			

			Rows.RowsAdded += new IndexRangeEventHandler(m_Rows_RowsAdded);
			Rows.RowsRemoved += new IndexRangeEventHandler(m_Rows_RowsRemoved);

			Columns.ColumnsAdded += new IndexRangeEventHandler(m_Columns_ColumnsAdded);
			Columns.ColumnsRemoved += new IndexRangeEventHandler(m_Columns_ColumnsRemoved);

			Selection.SelectionChange += new SelectionChangeEventHandler(Selection_SelectionChange);
			m_Summary=new  ICell[100];
		}

		#endregion

		#region Cells
		/// <summary>
		/// Return the Cell at the specified Row and Col position.
		/// </summary>
		/// <param name="p_iRow"></param>
		/// <param name="p_iCol"></param>
		/// <returns></returns>
		public override Cells.ICellVirtual GetCell(int p_iRow, int p_iCol)
		{
			return this[p_iRow, p_iCol];
		}

		/// <summary>
		/// Set the specified cell int he specified position. Abstract method of the GridVirtual control
		/// </summary>
		/// <param name="p_iRow"></param>
		/// <param name="p_iCol"></param>
		/// <param name="p_Cell"></param>
		public override void SetCell(int p_iRow, int p_iCol, Cells.ICellVirtual p_Cell)
		{
			if (p_Cell is ICell)
				InsertCell(p_iRow, p_iCol, (ICell)p_Cell);
			else if (p_Cell == null)
				InsertCell(p_iRow, p_iCol, null);
			else
				throw new SourceGridException("Expected ICell class");
		}

		/// <summary>
		/// Array of cells
		/// </summary>
		private ICell[,] m_Cells = null;
		private ICell[]  m_Summary=null;
		public  ICell[] SummaryCells 
		{
			get
			{
				return m_Summary;
			}
			set
			{
				m_Summary=value;
			}
		}
		private int CellsRows
		{
			get
			{
				if (m_Cells==null)
					return 0;
				else
					return m_Cells.GetLength(0);
			}
		}
		private int CellsCols
		{
			get
			{
				if (m_Cells==null)
					return 0;
				else
					return m_Cells.GetLength(1);
			}
		}

		/// <summary>
		/// Returns or set a cell at the specified row and col. If you get a ICell position occupied by a row/col span cell, and EnableRowColSpan is true, this method returns the cell with Row/Col span.
		/// </summary>
		public ICell this[int row, int col]
		{
			get
			{
				if (EnableRowColSpan==false)
					return m_Cells[row,col];
				else //enable Row Col Span search
				{
					#region Search Row Col Span
					ICell l_RetCell = m_Cells[row,col];
					if (l_RetCell==null)
					{
						int l_StartRow = row;
						int l_StartCol;
						if (col==0)
						{
							l_StartCol = 0;
							if (l_StartRow > 0)
								l_StartRow--;
						}
						else
							l_StartCol = col-1;

						int l_EndCol = col;
						int l_EndRow;
						if (row==0)
						{
							l_EndRow = 0;
							if (l_EndCol > 0)
								l_EndCol--;
						}
						else
							l_EndRow = row-1;

						Position l_ReqPos = new Position(row, col);

						//ciclo fino a che non raggiungo la fine della griglia (con un massimo di MaxSpanSearch)
						for (int l_Search = 0; l_Search < m_MaxSpanSearch; l_Search++)
						{
							bool l_bAllFull = true;//se trovo tutta una diagonale occupata da celle non valide mi fermo

							//ciclo sulla diagonale
							for (int r = l_StartRow, c = l_StartCol; r >= l_EndRow && c <= l_EndCol; r--, c++)
							{
								if (m_Cells[r,c] != null)
								{
									if (m_Cells[r,c].ContainsPosition(l_ReqPos))//se la cella richiesta fa parte di questa cella
										return m_Cells[r,c];
									else
										l_bAllFull &= true;
								}
								else
									l_bAllFull = false;
							}
						
							if (l_bAllFull)
								return null;

							if (l_StartCol > 0)
								l_StartCol--;
							else
								l_StartRow--;

							if (l_EndRow > 0)
								l_EndRow--;
							else
								l_EndCol--;

							if (l_EndCol < 0 || l_StartRow < 0)
								return null;
						}

						return null;
					}
					else
						return l_RetCell;
					#endregion
				}
			}
			set
			{					
				InsertCell(row,col,value);
			}
		}

		/// <summary>
		/// Remove the specified cell
		/// </summary>
		/// <param name="row"></param>
		/// <param name="col"></param>
		public virtual void RemoveCell(int row, int col)
		{
			ICell tmp = m_Cells[row,col];

			if (tmp!= null)
			{
				//se per caso la cella era quella correntemente con il mouse metta quest'ultima a null
				if (tmp == MouseCell)
					ChangeMouseCell(null);

				tmp.Select = false; //deseleziono la cella (per evitare che venga rimossa senza essere stata aggiunta alla lista di selection
				tmp.LeaveFocus(); //tolgo l'eventuale focus dalla cella

				tmp.UnBindToGrid();

				m_Cells[row,col] = null;
			}
		}

		/// <summary>
		/// Insert the specified cell (for best performance set Redraw property to false)
		/// </summary>
		/// <param name="row"></param>
		/// <param name="col"></param>
		/// <param name="p_cell"></param>
		public virtual void InsertCell(int row, int col, ICell p_cell)
		{
			RemoveCell(row,col);
			m_Cells[row,col] = p_cell;

			if (p_cell!=null)
			{
				if (p_cell.Grid!=null)
					throw new ArgumentException("This cell already have a linked grid","p_cell");

				p_cell.BindToGrid(this,new Position(row, col));

				if (Redraw)
					p_cell.Invalidate();
			}
		}

		/// <summary>
		/// Returns all the cells at specified column position
		/// </summary>
		/// <param name="p_ColumnIndex"></param>
		/// <returns></returns>
		public override ICellVirtual[] GetCellsAtColumn(int p_ColumnIndex)
		{
			Cells.ICellVirtual[] l_Cells = new Cells.ICellVirtual[Rows.Count];

			for (int r = 0; r < Rows.Count;)
			{
				ICell l_Cell = this[r, p_ColumnIndex];
				if (l_Cell != null &&
					l_Cell.Range.Start == new Position(r, p_ColumnIndex))
				{
					l_Cells[r] = l_Cell;
					r += l_Cell.RowSpan;
				}
				else
				{
					l_Cells[r] = null;
					r++;
				}
			}

			return l_Cells;
		}

		/// <summary>
		/// Returns all the cells at specified row position
		/// </summary>
		/// <param name="p_RowIndex"></param>
		/// <returns></returns>
		public override ICellVirtual[] GetCellsAtRow(int p_RowIndex)
		{
			Cells.ICellVirtual[] l_Cells = new Cells.ICellVirtual[Columns.Count];

			for (int c = 0; c < Columns.Count;)
			{
				ICell l_Cell = this[p_RowIndex, c];
				if (l_Cell != null &&
					l_Cell.Range.Start == new Position(p_RowIndex, c))
				{
					l_Cells[c] = l_Cell;
					c += l_Cell.ColumnSpan;
				}
				else
				{
					l_Cells[c] = null;
					c++;
				}
			}

			return l_Cells;
		}


		#endregion

		#region AddRow/Col, RemoveRow/Col

		private void m_Rows_RowsAdded(object sender, IndexRangeEventArgs e)
		{
			//N.B. Uso m_Cells.GetLength(0) anziche' RowsCount e
			// m_Cells.GetLength(1) anziche' ColumnsCount per essere sicuro di lavorare sulle righe effetivamente allocate

			RedimCellsMatrix(CellsRows + e.Count, CellsCols);

			//dopo aver ridimensionato la matrice sposto le celle in modo da fare spazio alla nuove righe
			for (int r = CellsRows-1; r > (e.StartIndex + e.Count-1); r--)
			{
				for (int c = 0; c < CellsCols; c++)
				{
					ICell tmp = m_Cells[r - e.Count, c];
					RemoveCell(r - e.Count, c);
					InsertCell(r,c,tmp);
				}
			}

			if (Redraw)
				RefreshGridLayout();
		}

		private void m_Rows_RowsRemoved(object sender, IndexRangeEventArgs e)
		{
			//N.B. Uso m_Cells.GetLength(0) anziche' RowsCount e
			// m_Cells.GetLength(1) anziche' ColumnsCount per essere sicuro di lavorare sulle righe effetivamente allocate

			for (int r = (e.StartIndex + e.Count); r < CellsRows; r++)
			{
				for (int c = 0; c < CellsCols; c++)
				{
					ICell tmp = m_Cells[r,c];
					RemoveCell(r,c);
					InsertCell(r - e.Count, c, tmp);
				}
			}

			RedimCellsMatrix(CellsRows-e.Count, CellsCols);

			if (Redraw)
				RefreshGridLayout();
		}

		private void m_Columns_ColumnsAdded(object sender, IndexRangeEventArgs e)
		{
			//N.B. Uso m_Cells.GetLength(0) anziche' RowsCount e
			// m_Cells.GetLength(1) anziche' ColumnsCount per essere sicuro di lavorare sulle righe effetivamente allocate

			RedimCellsMatrix(CellsRows, CellsCols+e.Count);

			//dopo aver ridimensionato la matrice sposto le celle in modo da fare spazio alla nuove righe
			for (int c = CellsCols-1; c > (e.StartIndex + e.Count - 1); c--)
			{
				for (int r = 0; r < CellsRows; r++)
				{
					ICell tmp = m_Cells[r, c - e.Count];
					RemoveCell(r, c - e.Count);
					InsertCell(r,c,tmp);
				}
			}

			if (Redraw)
				RefreshGridLayout();
		}

		private void m_Columns_ColumnsRemoved(object sender, IndexRangeEventArgs e)
		{
			//N.B. Uso m_Cells.GetLength(0) anziche' RowsCount e
			// m_Cells.GetLength(1) anziche' ColumnsCount per essere sicuro di lavorare sulle righe effetivamente allocate

			for (int c = (e.StartIndex + e.Count); c < CellsCols; c++)
			{
				for (int r = 0; r < CellsRows; r++)
				{
					ICell tmp = m_Cells[r,c];
					RemoveCell(r,c);
					InsertCell(r, c - e.Count, tmp);
				}
			}

			RedimCellsMatrix(CellsRows, CellsCols-e.Count);

			if (Redraw)
				RefreshGridLayout();
		}

	
		/// <summary>
		/// Ridimensiona la matrice di celle e copia le eventuali vecchie celle presenti nella nuova matrice
		/// </summary>
		/// <param name="rows"></param>
		/// <param name="cols"></param>
		private void RedimCellsMatrix(int rows, int cols)
		{
			if (m_Cells == null)
			{
				m_Cells = new ICell[rows,cols];
			}
			else
			{
				if (rows != m_Cells.GetLength(0) || cols != m_Cells.GetLength(1))
				{
					ICell[,] l_tmp = m_Cells;
					int l_minRows = Math.Min(l_tmp.GetLength(0),rows);
					int l_minCols = Math.Min(l_tmp.GetLength(1),cols);

					//cancello le celle non pi?utilizzate
					for (int i = l_minRows; i <l_tmp.GetLength(0); i++)
						for (int j = 0; j < l_tmp.GetLength(1); j++)
							RemoveCell(i,j);
					for (int i = 0; i <l_minRows; i++)
						for (int j = l_minCols; j < l_tmp.GetLength(1); j++)
							RemoveCell(i,j);

					m_Cells = new ICell[rows,cols];

					//copio le vecchie celle
					for (int i = 0; i <l_minRows; i++)
						for (int j = 0; j < l_minCols; j++)
							m_Cells[i,j] = l_tmp[i,j];
				}
			}

			//			m_iRows = m_Cells.GetLength(0);
			//			m_iCols = m_Cells.GetLength(1);
		}

		#endregion

		#region Row/Col Span

		/// <summary>
		/// Get if Row/Col Span is enabled. This value is automatically calculated based on the current cells.
		/// </summary>
		[Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool EnableRowColSpan
		{
			get{return (m_MaxSpanSearch > 0);}
		}

		private int m_MaxSpanSearch = 0;
		/// <summary>
		/// Gets the maximum rows or columns number to search when using Row/Col Span. This value is automatically calculated based on the current cells. Do not change this value manually.
		/// </summary>
		[Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int MaxSpanSearch
		{
			get{return m_MaxSpanSearch;}
		}

		/// <summary>
		/// Loads the MaxSpanSearch property.
		/// </summary>
		/// <param name="p_MaxSpanSearch"></param>
		/// <param name="p_Reset"></param>
		public void SetMaxSpanSearch(int p_MaxSpanSearch, bool p_Reset)
		{
			if (p_MaxSpanSearch > m_MaxSpanSearch || p_Reset)
				m_MaxSpanSearch = p_MaxSpanSearch;
		}
		#endregion

		#region Cell Rectangle
		/// <summary>
		/// Get the Rectangle of the cell respect all the scrollable area. Using the Cell Row/Col Span.
		/// </summary>
		/// <param name="p_Position"></param>
		/// <returns></returns>
		public override Rectangle PositionToAbsoluteRect(Position p_Position)
		{
			ICell l_Cell = this[p_Position.Row, p_Position.Column];
			if (l_Cell!=null)
			{
				return base.RangeToAbsoluteRect( l_Cell.Range );
			}
			else
				return base.PositionToAbsoluteRect(p_Position);
		}

		/// <summary>
		/// Returns the absolute rectangle relative to the total scrollable area of the specified Range. Returns a 0 rectangle if the Range is not valid
		/// </summary>
		/// <param name="p_Range"></param>
		/// <returns></returns>
		public override Rectangle RangeToAbsoluteRect(Range p_Range)
		{
			if (EnableRowColSpan)
			{
				//cerco il range anche tra le celle in rowspan o colspan
				Range l_RealRange = p_Range;
				for (int r = p_Range.Start.Row; r <= p_Range.End.Row; r++)
					for (int c = p_Range.Start.Column; c <= p_Range.End.Column; c++)
					{
						ICell l_Cell = this[r, c];
						if (l_Cell != null && (l_Cell.RowSpan > 1 || l_Cell.ColumnSpan > 1))
							l_RealRange = Range.Union(l_RealRange, l_Cell.Range);
					}

				return base.RangeToAbsoluteRect(l_RealRange);
			}
			else
				return base.RangeToAbsoluteRect(p_Range);
		}
		#endregion

		#region Cell Visible
		/// <summary>
		/// Returns true if the specified cell is visible otherwise false
		/// </summary>
		/// <param name="p_Cell"></param>
		/// <returns></returns>
		public bool IsCellVisible(ICell p_Cell)
		{
			if (p_Cell!=null)
				return base.IsCellVisible(p_Cell.Range.Start);
			else
				return true;
		}
		/// <summary>
		/// Scroll the view to show the specified cell
		/// </summary>
		/// <param name="p_CellToShow"></param>
		/// <returns></returns>
		public bool ShowCell(ICell p_CellToShow)
		{
			if (p_CellToShow!=null)
				return base.ShowCell(p_CellToShow.Range.Start);
			else
				return true;
		}
		
		#endregion
	
		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus (e);
			this.SetFocusStatus();
			
		}
	
		protected override void OnLostFocus(EventArgs e)
		{
			//if (this.FocusCell!=null) this.SetFocusCell(null);
		}

		private int defaultheight=18;
		public  int DefaultHeight 
		{
			get
			{
				return this.defaultheight;
			}
			set
			{
				this.defaultheight=value;
			}
		}
		/// <summary>
		/// Default cell width
		/// </summary>
		///
		private int defaultwidth=50;
		public  int DefaultWidth 
		{
			get
			{
				return this.defaultwidth;
			}
			set
			{
				this.defaultwidth=value;
			}
		}
		

		public  override void SetFocusStatus()
		{
			base.SetFocusStatus ();
			//如果当前 没有 focuscell 那么选中第一个
			try 
			{
				if (this.FocusCell!=null) return ;
				if (!this.KeepFocus)
				{
					this.SetFocusCell(this[1,0]);
				}
			}
			catch(Exception)
			{}
		}
		public override void OnLostFocusStatus()
		{
			base.OnLostFocusStatus ();
			this.InvalidateCells();
			try 
			{
//				if (this.ContainsFocus) return ;
//				if (this.KeepFocus) return ;
//				if (this.FocusCell==null) return ;
//				this.SetFocusCell(null);


			}
			catch(Exception)
			{}

		}


		#region InvalidateCell
		/// <summary>
		/// Force a redraw of the specified cell
		/// </summary>
		/// <param name="p_Cell"></param>
		public virtual void InvalidateCell(ICell p_Cell)
		{
			if (p_Cell!=null)
				base.InvalidateRange(p_Cell.Range);
		}
		
		/// <summary>
		/// Force a cell to redraw. If Redraw is set to false this function has no effects. If ColSpan or RowSpan is greater than 0 this function invalidate the complete range with InvalidateRange
		/// </summary>
		/// <param name="p_Position"></param>
		public override void InvalidateCell(Position p_Position)
		{
			ICell l_Cell = this[p_Position.Row, p_Position.Column];
			if (l_Cell==null)
				base.InvalidateCell(p_Position);
			else
				InvalidateRange(l_Cell.Range);
		}

		#endregion

		#region PaintCell
		/// <summary>
		/// Draw the specified Cell
		/// </summary>
		/// <param name="p_Panel"></param>
		/// <param name="e"></param>
		/// <param name="p_Cell"></param>
		/// <param name="p_CellPosition"></param>
		/// <param name="p_PanelDrawRectangle"></param>
		protected override void PaintCell(GridSubPanel p_Panel,PaintEventArgs e, Cells.ICellVirtual p_Cell, Position p_CellPosition, Rectangle p_PanelDrawRectangle)
		{
			ICell l_Cell = (ICell)p_Cell;
			Range l_CellRange = l_Cell.Range;
			if ( l_CellRange.RowsCount == 1 && l_CellRange.ColumnsCount == 1 )
				base.PaintCell(p_Panel, e, p_Cell, p_CellPosition, p_PanelDrawRectangle);
			else //Row/Col Span > 1
			{
				Rectangle l_Rect = p_Panel.RectangleGridToPanel( PositionToDisplayRect(l_CellRange.Start) );
				base.PaintCell(p_Panel, e, p_Cell, l_CellRange.Start, l_Rect);//uso come rettangolo di disegno il rettanglo calcolato dalla posizione originale
			}
		}
		protected override void PaintSummary(GridSubPanel p_Panel,PaintEventArgs e)
		{
			//在 最上面 画 一条线
			//e.Graphics.DrawLine(Pens.Black,0,this.Height-45,this.Width,this.Height-45);

			for (int i=0;i<this.Columns.Count;i++)
			{
				ICellVirtual l_Cell=(ICellVirtual)this.m_Summary[i];
				if (l_Cell==null) continue;
				if (l_Cell!=null)
				{
					int sx=this.Columns[i].Left+this.CustomScrollPosition.X;
					
					int width=this.Columns[i].Width;
					if (i>0) sx-=this.BorderWidth;
					else width-=this.BorderWidth;
					int sy=p_Panel.Height-this.SummaryHeight;
					//sx-=this.ScrollablePanel.AutoScrollPosition.X;
					Rectangle l_Rect=new Rectangle(sx,sy,width,this.SummaryHeight);
					//l_Cell.VisualModel.BackColor=this.SummaryColor;
					Position pp=new Position(0,0);
					base.PaintCell(p_Panel, e, l_Cell, pp, l_Rect);//uso come rettangolo di disegno il rettanglo calcolato dalla posizione originale
				
		
				}
			}
		
		}

		#endregion

		#region FocusCell

		/// <summary>
		/// Change the focus of the grid. The calls order is: (the user select CellX) Grid.CellGotFocus(CellX), CellX.Enter, (the user select CellY), Grid.CellLostFocus(CellX), CellX.Leave, Grid.CellGotFocus(CellY), CellY.Enter
		/// </summary>
		/// <param name="p_CellToSetFocus">Must be a valid cell linked to the grid or null of you want to remove the focus</param>
		/// <param name="p_DeselectOtherCells">True to deselect others selected cells</param>
		/// <returns>Return true if the grid can select the cell specified, otherwise false</returns>
		public override bool SetFocusCell(Position p_CellToSetFocus, bool p_DeselectOtherCells)
		{
			if (EnableRowColSpan==false || p_CellToSetFocus.IsEmpty())
				return base.SetFocusCell (p_CellToSetFocus, p_DeselectOtherCells);
			else
			{
				ICell l_Cell = this[p_CellToSetFocus.Row,p_CellToSetFocus.Column];//N.B. questo metodo mi restituisce la cella reale (anche se questa posizione ?occupata solo perch?in merge con Row/Col Span)
				if (l_Cell!=null)
					return base.SetFocusCell(l_Cell.Range.Start, p_DeselectOtherCells); //chiamo il set focus con la cella reale
				else
					return base.SetFocusCell(p_CellToSetFocus, p_DeselectOtherCells);
			}
		}
		public void MoveFocusToPanel()
		{
			try 
			{
				int x=this.CustomScrollPosition.X;
				int y=this.CustomScrollPosition.Y;
				ICell cell=this.FocusCell ;
				if (cell==null) return ;
				int row=cell.Row;
				row--;
				if (row<0) row=0;

				//滚动到相应的行
				int hh=this.Rows[row].Top;
				ScrollToY(hh);
				this.Invalidate();

			}
			catch(Exception)
			{}
			
			
			
		}


		/// <summary>
		/// Returns the active cell. Null if no cell are active
		/// </summary>
		[Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ICell FocusCell
		{
			get
			{
				if (base.FocusCellPosition.IsEmpty())
					return null;
				else
					return this[base.FocusCellPosition.Row, base.FocusCellPosition.Column];
			}
		}
		
		/// <summary>
		/// Set the focus to the specified cell (the specified cell became the active cell, FocusCell property).
		/// </summary>
		/// <param name="p_CellToSetFocus"></param>
		/// <returns></returns>
		public bool SetFocusCell(ICell p_CellToSetFocus)
		{
			Position p=Position.Empty;
			bool ok=false;
			if (p_CellToSetFocus==null)
				ok= base.SetFocusCell(Position.Empty);
			else
			{
					ok= base.SetFocusCell(p_CellToSetFocus.Range.Start);
					p=p_CellToSetFocus.Range.Start;
			}
//			if (this.FocusCellEdit)
//			{
//				if (this.FocusCell!=null&& this.FocusCell.DataModel.EditableMode!=EditableMode.f) this.FocusCell.StartEdit(p,this.FocusCell.Value);
//			}
			return ok;
		}

		/// <summary>
		/// Get the real position for the specified position. For example when p_Position is a merged cell this method returns the starting position of the merged cells.
		/// Usually this method returns the same cell specified as parameter. This method is used for processing arrow keys, to find a valid cell when the focus is in a merged cell.
		/// </summary>
		/// <param name="p_Position"></param>
		/// <returns></returns>
		public override Position GetStartingPosition(Position p_Position)
		{
			ICell l_tmp = (ICell)GetCell(p_Position);
			if (l_tmp!=null)
				return l_tmp.Range.Start;
			else
				return p_Position;
		}
		#endregion

		#region MouseCell
		/// <summary>
		/// The cell currently under the mouse cursor. Null if no cell are under the mouse cursor.
		/// </summary>
		[Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public ICell MouseCell
		{
			get
			{
				if (base.MouseCellPosition.IsEmpty())
					return null;
				else
					return this[base.MouseCellPosition.Row, base.MouseCellPosition.Column];
			}
		}

		/// <summary>
		/// Fired when the cell under the mouse change. For internal use only.
		/// </summary>
		/// <param name="p_Cell"></param>
		protected void ChangeMouseCell(ICell p_Cell)
		{
			if (p_Cell==null)
				base.ChangeMouseCell(Position.Empty);
			else
				base.ChangeMouseCell(p_Cell.Range.Start);
		}

		#endregion

		#region Sort

		private bool m_CustomSort = false;

		/// <summary>
		/// Gets or sets if when calling SortRangeRows method use a custom sort or an automatic sort. Default = false (automatic)
		/// 
		/// </summary>
		[Browsable(true)]
		[Description("获取或设置调用SortRangeRows方法时是否使用自定义排序或自动排序。默认值=false（自动）")]
		public bool CustomSort
		{
			get{return m_CustomSort;}
			set{m_CustomSort = value;}
		}

		/// <summary>
		/// Fired when calling SortRangeRows method
		/// </summary>
		/// <param name="e"></param>
		protected override void OnSortingRangeRows(SortRangeRowsEventArgs e)
		{
			base.OnSortingRangeRows(e);

			if (CustomSort == false)
			{
				if (e.AbsoluteColKeys>e.Range.End.Column && e.AbsoluteColKeys<e.Range.Start.Column)
					throw new ArgumentException("Invalid range","e.AbsoluteColKeys");

				ICell[][] l_RangeSort = new ICell[e.Range.End.Row-e.Range.Start.Row+1][];
				ICell[] l_CellsKeys = new ICell[e.Range.End.Row-e.Range.Start.Row+1];

				int l_sR = 0;
				for (int r = e.Range.Start.Row; r <= e.Range.End.Row;r++)
				{
					l_CellsKeys[l_sR] = this[r,e.AbsoluteColKeys];

					int l_sC = 0;
					l_RangeSort[l_sR] = new ICell[e.Range.End.Column-e.Range.Start.Column+1];
					for (int c = e.Range.Start.Column; c <= e.Range.End.Column; c++)
					{
						l_RangeSort[l_sR][l_sC] = this[r,c];
						l_sC++;
					}
					l_sR++;
				}

				IComparer l_CellComparer = e.CellComparer;
				if (l_CellComparer==null)
					l_CellComparer = new ValueCellComparer();
				Array.Sort(l_CellsKeys,l_RangeSort,l_CellComparer);

				//Apply sort
				l_sR = 0;
				if (e.Ascending)
				{
					for (int r = e.Range.Start.Row; r <= e.Range.End.Row;r++)
					{
						int l_sC = 0;
						for (int c = e.Range.Start.Column; c <= e.Range.End.Column; c++)
						{
							RemoveCell(r,c);//rimuovo qualunque cella nella posizione corrente
							ICell tmp = l_RangeSort[l_sR][l_sC];

							if (tmp!=null && tmp.Grid!=null && tmp.Range.Start.Row>=0 && tmp.Range.Start.Column>=0) //verifico che la cella sia valida
								RemoveCell(tmp.Range.Start.Row, tmp.Range.Start.Column);//la rimuovo dalla posizione precedente

							this[r,c] = tmp;
							l_sC++;
						}
						l_sR++;
					}			
				}
				else //desc
				{
					for (int r = e.Range.End.Row; r >= e.Range.Start.Row;r--)
					{
						int l_sC = 0;
						for (int c = e.Range.Start.Column; c <= e.Range.End.Column; c++)
						{
							RemoveCell(r,c);//rimuovo qualunque cella nella posizione corrente
							ICell tmp = l_RangeSort[l_sR][l_sC];

							if (tmp!=null && tmp.Grid!=null && tmp.Range.Start.Row >= 0 && tmp.Range.Start.Column >= 0) //verifico che la cella sia valida
								RemoveCell(tmp.Range.Start.Row, tmp.Range.Start.Column);//la rimuovo dalla posizione precedente

							this[r,c] = tmp;
							l_sC++;
						}
						l_sR++;
					}
				}
			}
		}

		#endregion

		#region HTMLExport
		/// <summary>
		/// Export the specified cell to HTML
		/// </summary>
		/// <param name="p_CurrentPosition"></param>
		/// <param name="p_Cell"></param>
		/// <param name="p_Export"></param>
		/// <param name="p_Writer"></param>
		protected override void ExportHTMLCell(Position p_CurrentPosition, Cells.ICellVirtual p_Cell, IHTMLExport p_Export, System.Xml.XmlTextWriter p_Writer)
		{
			//export only real cell (not position occupied by rowspan or colspan
			if (p_Cell != null && ((ICell)p_Cell).Range.Start == p_CurrentPosition )
				p_Cell.VisualModel.ExportHTML(p_Cell, p_CurrentPosition, p_Export, p_Writer);
		}
		#endregion

		#region Selection
		private void Selection_SelectionChange(object sender, SelectionChangeEventArgs e)
		{
			//se ?abilitato RowColSpan devo assicurarmi di selezionare la cella di origine e non quella che sfrutta il RowCol Span
			if (EnableRowColSpan)
			{
				if (e.EventType == SelectionChangeEventType.Add)
				{
					for (int r = e.Range.Start.Row; r <= e.Range.End.Row; r++)
						for (int c = e.Range.Start.Column; c <= e.Range.End.Column; c++)
						{
							ICell l_Cell = this[r,c];//N.B. questo metodo mi restituisce la cella reale (anche se questa posizione ?occupata slo perch?in mee con Row/Col Span)
							if (l_Cell!=null)
							{
								Range l_Range = l_Cell.Range;

								if (l_Range != new Range(new Position(r,c)) ) //se la cella occupa pi?righe o colonne
									Selection.AddRange(l_Range); //la seleziono tutta
							}
						}
				}
				else if (e.EventType == SelectionChangeEventType.Remove)
				{
					for (int r = e.Range.Start.Row; r <= e.Range.End.Row; r++)
						for (int c = e.Range.Start.Column; c <= e.Range.End.Column; c++)
						{
							ICell l_Cell = this[r,c];//N.B. questo metodo mi restituisce la cella reale (anche se questa posizione ?occupata slo perch?in mee con Row/Col Span)
							if (l_Cell!=null)
							{
								Range l_Range = l_Cell.Range;

								if (l_Range != new Range(new Position(r,c)) ) //se la cella occupa pi?righe o colonne
									Selection.RemoveRange(l_Range); //la seleziono tutta
							}
						}
				}
			}
		}
		#endregion

		virtual public void Print()
		{
			initprint();
			PrintDocument pd=new PrintDocument();
			pd.PrintPage+=new PrintPageEventHandler(pd_PrintPage);
			pd.Print();

		}
		private void initprint()
		{
			line=0;
			page=0;
			startline=0;
		}
		virtual public void PrintPreview()
		{
		
			initprint();
			PrintDocument pd=new PrintDocument();
			pd.PrintPage+=new PrintPageEventHandler(pd_PrintPage);
			PrintPreviewDialog ppd=new PrintPreviewDialog();
			ppd.Document=pd;
			ppd.ShowDialog();

		}
		private int line=0;
		private int startline=0;
		private int page=0;
		private void PrintLine(int theline,PrintPageEventArgs e)
		{
			for (int i=0;i<this.Columns.Count;i++)
			{
				//打印所有的单元格
				if (this[theline,i]!=null)
				{
					this.PrintCell(this[theline,i],e);
				}
			}
		}
		private void PrintCell(ICell cell,PrintPageEventArgs e)
		{
				//获得 cell的大小位置
			int row=cell.Row;
			int col=cell.Column;
			int endrow=row+cell.RowSpan-1;
			int endcol=col+cell.ColumnSpan-1;
			
			Rectangle rect=new Rectangle(this.Columns[col].Left+e.MarginBounds.Left ,this.Rows[row].Top-this.Rows[startline].Top+e.MarginBounds.Top,this.Columns[endcol].Right-this.Columns[col].Left,this.Rows[endrow].Bottom-this.Rows[row].Top);
			//画 出对应的Cell
			//DrawCell_Border(cell,e.Graphics,rect);
			e.Graphics.DrawRectangle(Pens.Black,rect);
		
			StringFormat sf=new  StringFormat();
			sf.LineAlignment=StringAlignment.Center;
			//sf.co=cell.VisualModel.TextAlignment ;
			e.Graphics.DrawString(cell.DisplayText,this.Font,Brushes.Black,rect,sf);
//			Utility.PaintImageAndText(e.Graphics,
//				rect,
//				null,
//				ImageAlignment, 
//				ImageStretch, 
//				cell.DisplayText,
//				StringFormat,
//				false,
//				l_Border,
//				l_ForeColor, 
//				l_CurrentFont);
				
		   
		}
		protected virtual void DrawCell_Border(Cells.ICellVirtual p_Cell,
		
			Graphics  g, 
			Rectangle p_ClientRectangle
			)
		{

			if (!p_Cell.Grid.DrawGrid) return;

			RectangleBorder l_Border = p_Cell.VisualModel.Border;
		
			ControlPaint.DrawBorder(g, p_ClientRectangle,
				l_Border.Left.Color,
				l_Border.Left.Width,
				ButtonBorderStyle.Solid,
				l_Border.Top.Color,
				l_Border.Top.Width,
				ButtonBorderStyle.Solid,
				l_Border.Right.Color,
				l_Border.Right.Width,
				ButtonBorderStyle.Solid,
				l_Border.Bottom.Color,
				l_Border.Bottom.Width,
				ButtonBorderStyle.Solid);
		}

		private void pd_PrintPage(object sender, PrintPageEventArgs e)
		{
			//从上到下画所有的Cell
			int shift=page*e.PageBounds.Height;
			while(line<=this.Rows.Count-1)
			{
				PrintLine(line,e);

				if (this.Rows[line].Bottom-shift>e.PageBounds.Height)
				{
					startline=line;
					e.HasMorePages=true;
					return ;
				}
				line++;
				
			}
		}

		#region Excel 导出

		/*
		Excel.Application ExcelObj=null;
		private bool CreateObject()
		{
			ExcelObj = new Excel.Application();
		

			
			if (ExcelObj  == null) 
			{
				MessageBox.Show("ERROR: EXCEL couldn't be started!");
				return  false ;
			}
			return true;

		}
		private void ClearObj()
		{
			//关闭 数据
			
			Marshal.ReleaseComObject(this.ExcelObj);
			ExcelObj = null;
			GC.Collect();
			
		}
		public void ExportToExcel()
		{
			
			
			// 获得 这种类型的 所有数据定义


			int r=this.Rows.Count;
			int col=this.Columns.Count;
			
			Array a=Array.CreateInstance(typeof(string),r,col);
			for (int i=0;i<r;i++)
			{
				for (int j=0;j<col;j++)
				{
					string s=null;
					if (this[i,j]!=null) s=this[i,j].DisplayText;
					a.SetValue(s,i,j);
				}
			}
			//把这一数组 复制到Excel 表中
			if (!this.CreateObject()) return ;
			ExcelObj.Visible=true;
			//获得这种类型的所有数据

			Excel.Workbooks workbooks = ExcelObj.Workbooks;

			Console.WriteLine ("Adding a new workbook");
		
			Excel.Workbook newWorkbook = ExcelObj.Workbooks.Add(Excel.XlWBATemplate.xlWBATWorksheet);
			// get the workbook from the existing spreadsheet
			

			// get the collection of sheets in the workbook
			Excel.Sheets excelSheets = newWorkbook.Worksheets;

			// get the first and only worksheet from the collection 
			// of worksheets
			string currentSheet = "Sheet1";
			Excel.Worksheet excelWorksheet = (Excel.Worksheet)excelSheets.get_Item(currentSheet);
			
			//int startx=1;
			int starty=1;
			int endx=col;
			int endy=r;

			// 保存 
			
			string ss="A";
			if (endx>=26) 
			{
				ss=Convert.ToString(Convert.ToChar('A'+(endx/26)))+Convert.ToString(Convert.ToChar('A'+(endx%26)));

				ss+=endy.ToString();
			}
			else
			{
				char ec=Convert.ToChar(Convert.ToInt32('A')+endx);
				ss=ec.ToString()+endy.ToString();
			}


		


			string s1="A"+starty.ToString();
			// 

			
			Excel.Range range=excelWorksheet.get_Range(s1,ss);
			range.Value2=a;

		
			//保存
			//ExcelObj.Visible=true;
			//	this.saveFileDialog1.FileName = "*.xls";
		
		
			this.ClearObj();
			//MessageBox.Show("导出完毕");
		}
		
		 */
		#endregion
	}
}
