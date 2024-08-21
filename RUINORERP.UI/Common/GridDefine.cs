using System;
using System.Collections;
using SourceGrid2;
using SourceGrid2.Cells.Real;
using System.Drawing;
using cenetcom;
using System.Collections.Generic;

namespace RUINORERP.UI.Common
{
    /// <summary>
    /// griddefine �й�Grid�Ķ���
    /// </summary>
    /// 
    //public delegate void EventHandler CellValueChange(object sender,EventArgs e);
    public class GridDefine : ArrayList
    {

        public  List<string> GetColumns()
        {
            List<string> cols = new List<string>();
            for (int i = 0; i < this.Count; i++)
            {
                cols.Add(this[i].name);
            }
            return cols;
        }
        public GridDefine()
        {

        }
        new void Add(object o)
        {
            if (o is GridDefineColumnItem)
            {
                ((GridDefineColumnItem)o).parent = this;

            }
            base.Add(o);
        }

        void Add(GridDefineColumnItem o)
        {
            if (o is GridDefineColumnItem)
            {
                o.parent = this;
            }
            base.Add(o);
        }

        public void RemoveValue(int line)
        {
            //object key=this.values.GetKey(line);
            this.values.Remove(line);
        }
        public void RemoveLine(int line)
        {
            for (int i = 0; i < this.values.Count; i++)
            {
                int key = Convert.ToInt32(this.values.GetKey(i));
                object obj = this.values.GetByIndex(i);
                //���� 
                if (key > line)
                {
                    values.RemoveAt(i);
                    key--;
                    values.Add(key, obj);
                }

            }
        }
        public void ClearObjs()
        {
            this.values.Clear();
        }
        public bool report = false;

        public bool HeadTextCenter = true;
        public int needheight = 0;
        public Grid grid = null;
        public Color HeadForecolor = Color.Black;
        public Color HeadBackColor = Color.FromArgb(152, 152, 200);
        //		public  Color HeadColor =Color.FromArgb(148,148,160);
        public Color lightColor = Color.White;
        public Color darkColor = Color.FromArgb(233, 235, 255);
        public Color stepColor = Color.FromArgb(0, 0, 1);
        public Color rborderColor = Color.FromArgb(182, 182, 182);
        public Color backColor = Color.FromArgb(252, 252, 255);
        public Color SummaryColor = Color.FromArgb(217, 217, 255);
        private SortedList values = new SortedList();
        private SortedList addvalues = new SortedList();
        private SortedList rates = new SortedList();
        private SortedList xz = new SortedList();

        public string typename;
        public void SetRate(int index, double v)
        {
            if (rates.ContainsKey(index))
                rates[index] = v;
            else rates.Add(index, v);

        }
        public void SetXz(int index, double v)
        {
            if (xz.ContainsKey(index))
                xz[index] = v;
            else xz.Add(index, v);

        }
        public double GetXz(int index)
        {
            return Convert.ToDouble(xz[index]);
        }
        public void VisibleChange()
        {
            //���� ���� ���е� ��С

            if (this.grid != null)
            {
                GridHelper.ResetColWidth(this.grid, this);
            }


        }
        public double GetRate(int index)
        {
            if (rates.ContainsKey(index))
                return (Convert.ToDouble(rates[index]));
            else return 1;

        }

        public void Set(int index, object o)
        {
            if (values.ContainsKey(index))
                values[index] = o;
            else this.Add(index, o);
        }
        public void SetAdd(int index, object o)
        {
            if (addvalues.ContainsKey(index))
                addvalues[index] = o;
            else addvalues.Add(index, o);

        }
        public object GetAdd(int index)
        {
            return addvalues[index];
        }
        public object GetAddObj(object o)
        {
            if (this.values.ContainsValue(o))
            {
                int i = this.values.IndexOfValue(o);
                return this.GetAdd(i);
            }
            return null;
        }
        public void Add(int index, object o)
        {
            values.Add(index, o);
        }
        public object Get(int index)
        {
            return values[index];
        }
        public bool inreload = false;
        public bool linecontrol = false;
        public GridDefine(string[] names, int[] width, bool[] currencys, string[] sobjects, bool linecontrol)
        {
            for (int i = 0; i < names.Length; i++)
            {
                GridDefineColumnItem item = new GridDefineColumnItem(names[i], 0, false, null);
                item.name = names[i];
                if (currencys != null) item.currency = currencys[i];
                if (sobjects != null) item.selectobject = sobjects[i];
                if (width != null) item.width = width[i];
                this.Add(item);
            }
            this.linecontrol = linecontrol;
        }

        /// <summary>
        /// ��ֹ�༭
        /// </summary>
        public void DisableEdit()
        {
            for (int i = 0; i < this.Count; i++)
            {
                this[i].ReadOnly = true;
            }
        }

        public GridDefine(GridDefineColumnItem[] cols)
        {
            for (int i = 0; i < cols.Length; i++)
            {
                this.Add(cols[i]);
            }

            //for (int i = 0; i < names.Length; i++)
            //{
            //    GridDefineColumnItem item = new GridDefineColumnItem(names[i], 0, false, null);
            //    item.name = names[i];
            //    if (currencys != null) item.currency = currencys[i];
            //    if (sobjects != null) item.selectobject = sobjects[i];
            //    if (width != null) item.width = width[i];

            //}
            //this.linecontrol = linecontrol;
        }

        public GridDefine(string[] names, int[] width, bool[] currencys, string[] sobjects) : this(names, width, currencys, sobjects, false)
        {

        }

        public bool autonewline = false;

        public void OnValueChange(object sender, EventArgs e)
        {
            Cell c = (Cell)sender;
            int col = c.Column;
            //��������༭ 

            int row = c.Row;

            if (this.inreload) return;
            object itemo = null;
            if (this.Get(row) == null)
            {
                //����һ������
                throw new Exception("�ȴ�����TODO");
                //TODO
                //itemo=Objects.CreateObj(typename);
                //this.Set(row,itemo);


            }
            else itemo = this.Get(row);
            GridDefineColumnItem gditem = this[col];
            if (gditem.addlink)
            {
                throw new Exception("�ȴ�����TODO");
                //cenetcom.util.refutil.tools.setpropfieldvalue(itemo,gditem.linkprop,c.Key);

            }

            if (this.CellValueChangeVal != null) this.CellValueChangeVal(sender, e);



            //��������� ���� multi ��ô ����result�е�ֵ


            //���� 
            if (this[col].upper)
            {
                //��� ��ǰ����ֵС�� ����ֵ  ��ֵ�޸�Ϊ ����ֵ
                try
                {
                    double xx = Convert.ToDouble(c.Value);
                    double xz = this.GetXz(c.Row);

                    if (xx < xz)
                    {
                        c.Value = xz;
                        this.CellMessage((Grid)c.Grid, c, "���ܵ����޼�");
                    }
                }
                catch (Exception)
                { }
            }
            if (this[col].newline && this.linecontrol)
            {
                //��� ֵ ��Ϊ�� ��ô ��ʼ���Ժ�� �༭����
                if (c.Value == null || Convert.ToString(c.Value) == "")
                {
                    this.inreload = true;
                    GridHelper.disablerow((Grid)c.Grid, c.Row, this);
                    //������е���������
                    GridDefineColumnItem item = this[c.Column];
                    if (item.newline)
                    {
                        GridHelper.clearrow((Grid)c.Grid, c.Row, this);
                    }
                    this.inreload = false;

                }
                else
                {

                    if (c.Row == c.Grid.Rows.Count - 1)
                    {
                        GridHelper.addrow((Grid)c.Grid, this);
                        GridHelper.disablerow((Grid)c.Grid, c.Row + 1, this);
                    }
                    GridHelper.enablerow((Grid)c.Grid, c.Row, this);
                    GridHelper.EnableEditRow((Grid)c.Grid, c.Row + 1, this);

                }

            }
            if (this[col].Summary)
            {
                this.UpdateSummary();
            }
            if (this[col].multi)
            {
                UpdateMulti((Grid)c.Grid, c.Row);

            }

            this.inreload = true;
            if (CellValueChange != null) this.CellValueChange(sender, e);
            this.inreload = false;
        }

        public void OnValueChanged(object sender, EventArgs e)
        {
            Cell c = (Cell)sender;
            int col = c.Column;
            //��������༭ 

            int row = c.Row;

            if (this.inreload) return;
            object itemo = null;
            if (this.Get(row) == null)
            {
                //����һ������
                throw new Exception("�ȴ�����TODO");
                //TODO
                //itemo=Objects.CreateObj(typename);
                //this.Set(row,itemo);


            }
            else itemo = this.Get(row);
            GridDefineColumnItem gditem = this[col];
            if (gditem.addlink)
            {
                throw new Exception("�ȴ�����TODO");
                //cenetcom.util.refutil.tools.setpropfieldvalue(itemo,gditem.linkprop,c.Key);

            }

            if (this.CellValueChangeVal != null) this.CellValueChangeVal(sender, e);



            //��������� ���� multi ��ô ����result�е�ֵ


            //���� 
            if (this[col].upper)
            {
                //��� ��ǰ����ֵС�� ����ֵ  ��ֵ�޸�Ϊ ����ֵ
                try
                {
                    double xx = Convert.ToDouble(c.Value);
                    double xz = this.GetXz(c.Row);

                    if (xx < xz)
                    {
                        c.Value = xz;
                        this.CellMessage((Grid)c.Grid, c, "���ܵ����޼�");
                    }
                }
                catch (Exception)
                { }
            }
            if (this[col].newline && this.linecontrol)
            {
                //��� ֵ ��Ϊ�� ��ô ��ʼ���Ժ�� �༭����
                if (c.Value == null || Convert.ToString(c.Value) == "")
                {
                    this.inreload = true;
                    GridHelper.disablerow((Grid)c.Grid, c.Row, this);
                    //������е���������
                    GridDefineColumnItem item = this[c.Column];
                    if (item.newline)
                    {
                        GridHelper.clearrow((Grid)c.Grid, c.Row, this);
                    }
                    this.inreload = false;

                }
                else
                {

                    if (c.Row == c.Grid.Rows.Count - 1)
                    {
                        GridHelper.addrow((Grid)c.Grid, this);
                        GridHelper.disablerow((Grid)c.Grid, c.Row + 1, this);
                    }
                    GridHelper.enablerow((Grid)c.Grid, c.Row, this);
                    GridHelper.EnableEditRow((Grid)c.Grid, c.Row + 1, this);

                }

            }
            if (this[col].Summary)
            {
                this.UpdateSummary();
            }
            if (this[col].multi)
            {
                UpdateMulti((Grid)c.Grid, c.Row);

            }

            this.inreload = true;
            if (CellValueChange != null) this.CellValueChange(sender, e);
            this.inreload = false;
        }

        public void UpdateSummary()
        {

            //���� ���е� �ܺ�
            for (int x = 0; x < this.Count; x++)
            {
                if (!this[x].Summary) continue;
                double nv = 0;
                for (int i = 1; i < this.grid.Rows.Count; i++)
                {

                    try
                    {
                        double xx = Convert.ToDouble(this.grid[i, x].Value);
                        nv += xx;
                    }
                    catch (Exception)
                    { }

                }
                grid.SummaryCells[x].Value = nv;
            }
            //	grid.Invalidate();
        }

        public void UpdateMulti(Grid g, int row)
        {
            try
            {
                double x = 1;
                for (int i = 0; i < this.Count; i++)
                {
                    if (this[i].multi) x *= Convert.ToDouble(g[row, i].Value);
                }
                x *= this.GetRate(row);
                for (int i = 0; i < this.Count; i++)
                {
                    if (this[i].result)
                    {
                        g[row, i].Value = x;

                    }
                }
                this.UpdateSummary();


            }
            catch (Exception)
            {
            }

        }
        public void CellMessage(Grid grid, Cell c, string message)
        {
            Point l = c.DisplayRectangle.Location;
            int x = l.X;
            int y = l.Y;
            if (x < 0) x = 0;
            if (y < 0) y = 0;
            l = new Point(x, y);

            Point p = grid.PointToScreen(l);
            throw new Exception("�ȴ�����");
            //CheckControl.Message(message,p);


        }
        public int length()
        {
            int l = 0;

            for (int i = 0; i < this.Count; i++)
            {
                l += this[i].width;
            }
            return l;
        }
        public event EventHandler CellValueChangeVal;
        public event EventHandler CellValueChange;
        new public GridDefineColumnItem this[int index]
        {
            get
            {
                return (GridDefineColumnItem)base[index];
            }
        }

        public void OnArrowClick(object sender, EventArgs e)
        {
            Cell c = (Cell)sender;
            //ȡ�� ��Ӧ������ 
            int col = c.Column;
            int r = c.Row;
            GridDefineColumnItem item = this[col];
            if (item.selectobject != null)
            {
                throw new Exception("�ȴ�����OnArrowClick");
                /*
				if (item.selectfield=="" ||item.selectfield==null)
				{
					//ȡ������ 
					object o =this.Get(r);
					if (o is ���̿ɹ������)
					{
						���̿ɹ������ flowobj=(���̿ɹ������)o;
						o=flowobj.ԭʼ����();
						if (o!=null) gtools.EditAndSaveObject(o);
						
					}
				
				}
				else 
				{
					object o=cenetcom.util.FastObject.PickObject(item.selectobject,item.selectfield,Convert.ToString(c.Value));
					gtools.EditAndSaveObject(o);
		
				}
				*/



            }
            //��� �������� �������� ��ô���� ����
        }
        public int IndexOf(string name)
        {
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].name == name) return i;
            }
            return -1;

        }
        public GridDefineColumnItem this[string name]
        {
            get
            {
                for (int i = 0; i < this.Count; i++)
                {
                    if (this[i].name == name) return this[i];
                }
                return null;
            }
        }

    }
}
