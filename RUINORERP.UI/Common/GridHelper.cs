using System;
using System.Collections;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using cenetcom;
using SourceGrid2;
using SourceGrid2.DataModels;
using SourceGrid2.BehaviorModels;
using SourceGrid2.Cells.Real;
using cenetcom.control;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace RUINORERP.UI.Common
{
    /// <summary>
    /// 单据相关的表格，复杂报表统计
    /// 有业务关联性不通用
    /// </summary>
    public class GridHelper
    {
        public GridHelper()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }


        public static EditableMode _editableMode = EditableMode.Focus | EditableMode.SingleClick | EditableMode.AnyKey;
        public static bool dtnow = true;

        public static GridDefine _gridDefind;

        public static double Sum(ArrayList list, string propname)
        {
            double sum = 0;
            for (int i = 0; i < list.Count; i++)
            {
                object obj = list[i];
                double xx = 0;
                try
                {
                    //xx=Convert.ToDouble(cenetcom.util.refutil.tools.propfieldvalue(obj,propname));
                    throw new Exception("等待完善Sum");
                }
                catch (Exception)
                { }
                sum += xx;
            }
            return sum;
        }

        //public static Type  getsubtype(Type ctype)
        //{
        //	//获得 带有MasterDetail属性的项目

        //	MasterDetailAttribute att=(MasterDetailAttribute) cenetcom.util.refutil.tools.getsubtype(ctype,typeof(MasterDetailAttribute));
        //	if (att==null) return null;
        //	string subname=att.Itemname;
        //	return cenetcom.util.typeloader.loadtype(subname);
        //}

        public static double Sum(ArrayList list, string propname, string cond)
        {
            double sum = 0;
            for (int i = 0; i < list.Count; i++)
            {
                object obj = list[i];
                double xx = 0;
                try
                {
                    object objv = RUINORERP.Common.Helper.ReflectionHelper.GetPropertyValue(obj, cond);
                    bool ok = Convert.ToBoolean(objv);
                    if (ok)
                    {
                        object objx = RUINORERP.Common.Helper.ReflectionHelper.GetPropertyValue(obj, propname);
                        xx = Convert.ToDouble(objx);
                    }
                }
                catch (Exception)
                { }
                sum += xx;
            }
            return sum;
        }
        public static double SumNo(ArrayList list, string propname, string cond)
        {
            double sum = 0;
            for (int i = 0; i < list.Count; i++)
            {
                object obj = list[i];
                double xx = 0;
                try
                {
                    object objv = RUINORERP.Common.Helper.ReflectionHelper.GetPropertyValue(obj, cond);
                    bool ok = Convert.ToBoolean(objv);
                    if (!ok)
                    {
                        object objx = RUINORERP.Common.Helper.ReflectionHelper.GetPropertyValue(obj, propname);
                        xx = Convert.ToDouble(objx);
                    }
                }
                catch (Exception)
                { }
                sum += xx;
            }
            return sum;
        }
        public static bool EditObject(object o)
        {
            /*
			if (o==null) return false;
			if (!单据窗口.确认可以修改(o.GetType().FullName)) return false;
			Type type=o.GetType();
			gridvisual gv=(gridvisual)cenetcom.util.refutil.tools.getattoftype(type,typeof(gridvisual));
		    if (gv==null) return false;
		
			string editorname=gv.editname;
			if (editorname==""||editorname==null) return false;
			
	
			
			IObjectEditor editor=(IObjectEditor)Objects.CreateObj(editorname);
			editor.Value=o;
		
			if (editor is Form)
			{
				((Form)editor).StartPosition=FormStartPosition.CenterScreen;
				if (((Form)editor).ShowDialog()==DialogResult.OK)
				{
					return true;
				}
			}*/
            return false;
        }
        public static bool EditAndSaveObject(object o)
        {
            if (EditObject(o))
            {
                //Objects.Save(o);
                return true;
            }
            return false;
        }

        public static void initemptygrid(SourceGrid2.Grid grid)
        {
            GridDefine define2 = new GridDefine(new string[] { "         ", "      ", "      ", "     ", "     ", "      " }, new int[] { 90, 90, 90, 90, 90, 90 }, null, null);
            define2.DisableEdit();
            GridHelper.initgrid(grid, define2, true);
        }
        public static void disabledit(Grid grid, GridDefine define)
        {
            for (int i = 0; i < grid.Rows.Count; i++)
            {
                disablerow(grid, i, define);
                DisableEditRow(grid, i, define);
            }
        }
        public static void enabledit(Grid grid, GridDefine define)
        {
            for (int i = 0; i < grid.Rows.Count; i++)
            {
                object o = define.Get(i);
                if (o != null)
                {
                    GridHelper.enablerow(grid, i, define);
                    GridHelper.EnableEditRow(grid, i, define);
                }

            }
        }
        public static void updatecol(string name, GridDefine define, object val, Grid grid)
        {
            for (int i = 1; i < grid.Rows.Count; i++)
            {

                for (int j = 0; j < define.Count; j++)
                {
                    //获得相应的数据

                    //如果设定了 newline 属性并且 为空跳过
                    if (define[j].newline && (Convert.ToString(grid[i, j].Value) != "" && grid[i, j].Value != null))
                    {
                        //设定 这行的值
                        grid[i, define.IndexOf(name)].Value = val;

                    }
                }
            }


        }
        public static void clearobjs(SourceGrid2.Grid grid, GridDefine griddefine)
        {
            griddefine.ClearObjs();
        }

        public static void initgrid(SourceGrid2.Grid grid, GridDefine griddefine)
        {
            _gridDefind = griddefine;
            _gridDefind.grid = grid;
            //创建头

            grid.Redim(1, griddefine.Count);
            grid.FixedRows = 1;
            if (grid.Tag is GridDefine)
            {
                GridDefine dd = (GridDefine)grid.Tag;
                grid.ArrowClick -= new EventHandler(dd.OnArrowClick);
            }
            grid.Tag = griddefine;
            griddefine.grid = grid;
            for (int i = 0; i < griddefine.Count; i++)
            {
                SourceGrid2.Cells.Real.ColumnHeader c = new SourceGrid2.Cells.Real.ColumnHeader();
                //c.EnableSort = false;

                c.BackColor = griddefine.HeadBackColor;
                c.ForeColor = griddefine.HeadForecolor;
                c.Value = griddefine[i].name;

                if (griddefine.HeadTextCenter)
                    c.TextAlignment = ContentAlignment.MiddleCenter;
                else c.TextAlignment = ContentAlignment.MiddleLeft;
                grid[0, i] = c;
                grid.Columns[i].Width = griddefine[i].width;
            }

            grid.BackColor = griddefine.backColor;
            grid.BorderWidth = 0;
            //	grid.ArrowClick-=new EventHandler(griddefine.OnArrowClick);
            grid.ArrowClick += new EventHandler(griddefine.OnArrowClick);
        }


        public static void ResetColWidth(SourceGrid2.Grid grid, GridDefine griddefine)
        {
            int width = 0;
            for (int i = 0; i < griddefine.Count; i++)
            {
                int cw = 0;
                Cell c = new SourceGrid2.Cells.Real.ColumnHeader();
                c.BackColor = griddefine.HeadBackColor;
                c.Value = griddefine[i].name;

                grid[0, i] = c;
                if (griddefine[i].visible)
                {
                    cw = griddefine[i].width;

                }
                if (i == griddefine.Count - 1)
                {
                    cw = grid.Width - width;

                }
                grid.Columns[i].Width = cw;//

                width += cw;
            }


        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="griddefine"></param>
        /// <param name="autofill">自动填充宽度</param>
        public static void initgrid(SourceGrid2.Grid grid, GridDefine griddefine, bool autofill)
        {
            GridHelper.initgrid(grid, griddefine);

            if (autofill)
            {
                //重新设定 最后一个的大小
                int x = grid.Width - grid.BorderWidth - griddefine.length();
                if (x > 0)
                    griddefine[griddefine.Count - 1].width += x;
                if (grid.Columns[griddefine.Count - 1].Width < griddefine[griddefine.Count - 1].width)
                    grid.Columns[griddefine.Count - 1].Width = griddefine[griddefine.Count - 1].width;
            }


            //GridHelper.AddSummaryRow(grid, griddefine);

            //grid.Redim(20,grid.Cols);
            // 按照 grid的 大小 添加行数
            int r = GridHelper.gridmrows(grid);
            //grid.BorderWidth=1;
            //grid.BorderStyle=BorderStyle.Fixed3D;
            r = grid.Height / grid.DefaultHeight - 2;
            r = 10;
            for (int i = 1; i < r; i++)
            {
                AddRow(grid, griddefine, true);
            }
            grid.Height = (r + 2) * grid.DefaultHeight + grid.BorderWidth * 2;
            griddefine.needheight = (r + 1) * grid.DefaultHeight + grid.BorderWidth * 2 + grid.SummaryHeight + 4;



        }


        public static void InitGridForEdit(SourceGrid2.Grid grid, GridDefine griddefine, bool autofill)
        {
            if (griddefine.Count == 0)
            {
                return;
            }

            GridHelper.initgrid(grid, griddefine);

            if (autofill)
            {
                //重新设定 最后一个的大小
                int x = grid.Width - grid.BorderWidth - griddefine.length();
                if (x > 0)
                    griddefine[griddefine.Count - 1].width += x;
                if (grid.Columns[griddefine.Count - 1].Width < griddefine[griddefine.Count - 1].width)
                    grid.Columns[griddefine.Count - 1].Width = griddefine[griddefine.Count - 1].width;
            }


            GridHelper.AddSummaryRow(grid, griddefine);

            //grid.Redim(20,grid.Cols);
            // 按照 grid的 大小 添加行数
            int r = GridHelper.gridmrows(grid);
            //grid.BorderWidth=1;
            //grid.BorderStyle=BorderStyle.Fixed3D;
            r = grid.Height / grid.DefaultHeight - 2;
            if (r <= 2)
            {
                r = 15;
            }
            for (int i = 1; i < r; i++)
            {
                AddRowForEdit(grid, griddefine, false);
            }
            grid.Height = (r + 2) * grid.DefaultHeight + grid.BorderWidth * 2;
            griddefine.needheight = (r + 1) * grid.DefaultHeight + grid.BorderWidth * 2 + grid.SummaryHeight + 4;

        }


        public static void initgrid2(SourceGrid2.Grid grid, GridDefine griddefine, bool autofill)
        {
            GridHelper.initgrid(grid, griddefine);

            if (autofill)
            {
                //重新设定 最后一个的大小
                int x = grid.Width - grid.BorderWidth - griddefine.length();

                griddefine[griddefine.Count - 1].width += x;
                grid.Columns[griddefine.Count - 1].Width = griddefine[griddefine.Count - 1].width;
            }
            //	GridHelper.AddSummaryRow(grid,griddefine);


            //grid.Redim(20,grid.Cols);
            // 按照 grid的 大小 添加行数
            int r = GridHelper.gridmrows(grid);
            r = grid.Height / grid.DefaultHeight - 1;
            for (int i = 0; i < r; i++)
            {
                AddRow(grid, griddefine, true);
            }
            grid.Height = (r + 1) * grid.DefaultHeight + grid.BorderWidth * 2;




        }
        public static int gridheight(SourceGrid2.Grid grid)
        {
            int h = 0;
            for (int i = 0; i < grid.Rows.Count; i++)
            {
                h += grid.Rows[i].Height;
                //h+=grid.GetRowHeight(i);
            }
            return h;
        }

        public static int gridmrows(SourceGrid2.Grid grid)
        {
            int hh = 0;

            for (int i = 0; i < grid.Rows.Count; i++)
            {

                hh += grid.Rows[i].Height;
                if (hh > grid.Height)
                {
                    return i;
                }
            }


            return grid.Rows.Count;
        }


        public static void DebugTime(string action)
        {
            if (!dtnow) return;
            DateTime t = DateTime.Now;
            System.Diagnostics.Debug.WriteLine(action + "--" + t.ToString() + ":" + t.Millisecond.ToString());
        }

        public static ArrayList buildarrayList(string typename, Grid grid, GridDefine define)
        {
            //逐一构建对象
            ArrayList list = new ArrayList();
            for (int i = 1; i < grid.Rows.Count; i++)
            {
                DebugTime("builditem" + i.ToString());
                object o = null;
                o = define.Get(i);
                //DebugTime("createitem"+i.ToString());
                if (o == null)
                {
                    //o = Objects.FastCreateObj(typeof(GridHelper).Assembly, typename);
                }
                //DebugTime("endcreateitem"+i.ToString());
                if (define.GetAdd(i) != null)
                {
                    //cenetcom.util.refutil.tools.setpropfieldvalue(o,"Tag",define.GetAdd(i));
                }
                //设定 项目的所有属性
                bool add = true;
                //DebugTime("builditem propvalues start"+i.ToString());
                for (int j = 0; j < define.Count; j++)
                {
                    //获得相应的数据
                    string sname = define[j].storename;
                    if (sname == null || sname == "") sname = define[j].name;
                    //cenetcom.util.refutil.tools.setpropfieldvalue(o,sname,grid[i,j].Value);
                    //如果设定了 newline 属性并且 为空跳过
                    if (define[j].newline && (Convert.ToString(grid[i, j].Value) == "" || grid[i, j].Value == null))
                    {
                        add = false;
                    }
                    if (define[j].addlink)
                    {
                        //	cenetcom.util.refutil.tools.setpropfieldvalue(o,define[j].linkprop,grid[i,j].Key);

                    }

                }

                //	DebugTime("builditem propvalues end"+i.ToString());
                if (add)
                {
                    list.Add(o);
                    define.Set(i, o);
                }



            }
            return list;

        }
        public static void enablerow(Grid grid, int row, GridDefine define)
        {
            for (int i = 0; i < define.Count; i++)
            {
                Cell c = (Cell)grid[row, i];
                if (c.IsEditing()) continue;
                if (!(define[i].ReadOnly || define[i].check))
                {

                    c.EditableMode = _editableMode;
                    c.InputType = define[i].type;
                    c.DataModel = new SourceGrid2.DataModels.EditorTextBox(typeof(string));



                }
                if (define[i].select)
                {
                    c.vals = define[i].vals;
                    c.EditableMode = _editableMode;
                    c.DataModel = new SourceGrid2.DataModels.EditorComboBox(typeof(string));
                }
                InputType it = define[i].type;
                if (it == InputType.Double || it == InputType.DoubleAboveZero || it == InputType.DoubleZero || it == InputType.Int)
                {
                    c.DataModel = new SourceGrid2.DataModels.EditorTextBox(typeof(double));

                    if (it == InputType.Int)
                    {
                        c.DataModel = new SourceGrid2.DataModels.EditorTextBoxNumeric(typeof(int));
                    }
                }
                else
                {
                    if (!define[i].check)
                        c.DataModel = new SourceGrid2.DataModels.EditorTextBox(typeof(string));

                }
                if (define[i].percent || define[i].type == InputType.Percent)
                {


                    SourceGrid2.DataModels.EditorTextBox l_PercentEditor = new SourceGrid2.DataModels.EditorTextBox(typeof(double));

                    l_PercentEditor.AllowNull = false;
                    l_PercentEditor.DefaultValue = 0.0;
                    //l_PercentEditor.TypeConverter = new SourceLibrary.Converter.PercentTypeConverter(typeof(double));
                    c.DataModel = l_PercentEditor;
                }
                if (define[i].time)
                {
                    c.DataModel = new SourceGrid2.DataModels.EditorDateTime();
                }
                //				if (define[i].check)
                //				{
                //					c=new CellCheckBox(false);
                //				}
                if (define[i].selectobject != null)
                {
                    //c.DataModel=new  PickerEditor(define[i].selectobject,define[i].selectfield,define[i].filter,define[i].fastkey);
                }

                if (define.report) c.report = true;

                if (define[i].check)
                {
                    c.EnableEdit = true;

                }

                if (define[i].ReadOnly)//||define[i].check)
                {
                    c.EditableMode = EditableMode.None;
                    c.EnableEdit = false;
                }
                else
                {



                    if (c.DataModel != null && !define[i].check) c.EditableMode = _editableMode;


                }
            }
        }

        /// <summary>
        /// 禁止编辑行
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="row"></param>
        /// <param name="define"></param>
        public static void DisableEditRow(Grid grid, int row, GridDefine define)
        {
            for (int i = 0; i < define.Count; i++)
            {
                if (grid[row, i] != null)
                {
                    Cell c = (Cell)grid[row, i];
                    c.EditableMode = EditableMode.None;
                    c.EnableEdit = false;
                }

            }

        }

        public static void disablerow(Grid grid, int row, GridDefine define)
        {
            for (int i = 0; i < define.Count; i++)
            {
                if (!define[i].newline)
                {
                    if (grid[row, i] != null)
                    {
                        Cell c = (Cell)grid[row, i];
                        c.EditableMode = EditableMode.None;
                        c.EnableEdit = false;
                    }
                }
            }
        }

        /// <summary>
        /// 启用编辑行
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="row"></param>
        /// <param name="define"></param>
        public static void EnableEditRow(Grid grid, int row, GridDefine define)
        {
            for (int i = 0; i < define.Count; i++)
            {
                if (define[i].newline)
                {
                    if (grid.Rows.Count - 1 < row) return;

                    if (grid[row, i] != null)
                    {
                        Cell c = (Cell)grid[row, i];

                        c.EditableMode = _editableMode;
                        c.EnableEdit = true;
                    }
                }
            }
        }



        public static void clearrow(Grid grid, int row, GridDefine define)
        {
            define.SetXz(row, 0);
            define.Set(row, null);
            for (int i = 0; i < define.Count; i++)
            {
                if (!define[i].newline)
                {
                    if (grid[row, i].Value is Boolean) grid[row, i].Value = false;
                    else grid[row, i].Value = null;
                }
            }


        }
        public static int addrowwithdata(Grid grid, object o, GridDefine define)
        {
            return GridHelper.addrowwithdata(grid, o, define, false);
        }
        public static void clear(Grid grid, GridDefine define)
        {

            for (int i = 0; i < define.Count; i++)
            {
                define.Set(i, null);
                for (int j = 1; j < grid.Rows.Count; j++)
                {
                    if (grid[j, i] != null)
                    {
                        if (grid[j, i].Value is Boolean) grid[j, i].Value = false;
                        else grid[j, i].Value = null;
                    }

                }

            }
            try
            {
                for (int i = 0; i < grid.SummaryCells.Length; i++)
                {
                    if (i == 0) grid.SummaryCells[i].Value = "合计";
                    else if (grid.SummaryCells[i] != null) grid.SummaryCells[i].Value = null;
                }
                grid.InvalidateCells();
            }
            catch (Exception)
            { }
        }
        public static void checkline(Grid grid, GridDefine define, int row)
        {
            int l = row;

            //添加数据
            //如果行不够那么需要添加新的行
            if (grid.Rows.Count - 1 < row)
            {
                grid.Redim(row, grid.Columns.Count);
                GridHelper.AddRow(grid, define, true);
                GridHelper.enablerow(grid, row, define);
            }

        }
        public static void filllargegrid(Grid grid, string typename, bool edit, string cond)
        {
            /*
			Type type=cenetcom.util.typeloader.loadtype(typename);
			griddefine define=new griddefine();
			gridvisual gv=(largegridvisual)cenetcom.util.refutil.tools.getattoftype(type,typeof(largegridvisual));
			if (gv==null)  gv=(gridvisual)cenetcom.util.refutil.tools.getattoftype(type,typeof(gridvisual));
			 define=new griddefine(gv.names,gv.sizes,null,null);
			//添加数据

			define.DisableEdit();
			ObjectSet os=Objects.Search(new SearchCond(typename,cond));

			GridHelper.initgrid(grid,define);
			fillgrid(grid,os.List,define);*/

        }
        public static void fillgrid(Grid grid, ArrayList list, GridDefine define)
        {

            if (list == null) return;
            for (int i = 0; i < list.Count; i++)
            {
                //if (grid.Rows.Count<list.Count) GridHelper.addrow(grid,define,false);
                GridHelper.updaterowwithdata(grid, list[i], define, i + 1);
            }
        }
        public static void fillgrid(Grid grid, ArrayList list, string typename)
        {
            /*
			Type type=cenetcom.util.typeloader.loadtype(typename);
			
			gridvisual gv=(gridvisual)cenetcom.util.refutil.tools.getattoftype(type,typeof(gridvisual));
			griddefine define=new griddefine(gv.names,gv.sizes,null,null);
			for (int i=0;i<define.Count;i++)
			{
				Type subtype=cenetcom.util.refutil.tools.memberType(type,define[i].name);
				if (subtype!=null && subtype.IsEnum)
				{
					string[] names=Enum.GetNames(subtype);
					define[i].enums=names;
				}

			}
			define.DisableEdit();
			GridHelper.initgrid(grid,define);
			fillgrid(grid,list,define);*/
        }
        public static GridDefine fillgridwitcheck(Grid grid, ArrayList list, string typename)
        {
            /*
			Type type=cenetcom.util.typeloader.loadtype(typename);
			
			gridvisual gv=(gridvisual)cenetcom.util.refutil.tools.getattoftype(type,typeof(gridvisual));
			griddefine define=new griddefine(gv.names,gv.sizes,null,null);
			
			for (int i=0;i<define.Count;i++)
			{
				Type subtype=cenetcom.util.refutil.tools.memberType(type,define[i].name);
				if (subtype!=null && subtype.IsEnum)
				{
					string[] names=Enum.GetNames(subtype);
					define[i].enums=names;
				}

			}
		
			define.DisableEdit();
			griddefineitem gdi=new griddefineitem("选择",50,false,null);
			gdi.check=true;
			define.Insert(0,gdi);
			GridHelper.initgrid(grid,define);
			fillgrid(grid,list,define);
			return define;	*/
            return null;
        }


        public static void fillgrid(Grid grid, string typename, bool edit)
        {
            //设定所有的项目
            /*
			Type type=cenetcom.util.typeloader.loadtype(typename);
			//生成所有的项目
			shadow sh=new shadow(type);
			griddefine define=new griddefine();
			for (int i=0;i<sh.Count;i++)
			{
				griddefineitem item=new griddefineitem(sh[i].Name,100,false,null);
				if (sh[i].Type.FullName==typeof(bool).FullName )
				{
					item.check=true;
					item.width=30;
				}
				define.Add(item);
			}
			if (!edit) define.DisableEdit();
			else define.CellValueChange+=new EventHandler(define_CellValueChange);
			define.inreload=true;
			ObjectSet os=Objects.Search(new SearchCond(typename,null));
			GridHelper.initgrid(grid,define);
			GridHelper.fillgrid(grid,os.List,define);
			grid.Tag=define;
			define.inreload=false;
			*/



        }
        public static void fillgrid(Grid grid, DataSet ds, GridDefine define)
        {
            //GridHelper.clear(grid,define);

            if (ds == null) return;

            if (ds.Tables.Count == 0) return;
            DataTable dt = ds.Tables[0];

            //更新所有的数据
            initgrid(grid, define);
            grid.Redim(dt.Rows.Count + 1, define.Count);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //updaterowwithdata(grid,dt,define,i+1);
                for (int j = 0; j < define.Count; j++)
                {
                    grid[i + 1, j] = new Cell(Convert.ToString(dt.Rows[i][define[j].name]));
                }
            }
        }
        public static void initgrid(SourceGrid2.Grid grid, DataTable dt)
        {
            //创建头
            grid.Redim(1, dt.Columns.Count);
            grid.FixedRows = 1;
            //griddefine.grid=grid;
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                SourceGrid2.Cells.Real.ColumnHeader c = new SourceGrid2.Cells.Real.ColumnHeader();
                c.EnableSort = false;
                //c.BackColor=griddefine.HeadColor;
                c.Value = dt.Columns[i].ColumnName;

                grid[0, i] = c;
                //grid.Columns[i].Width=griddefine[i].width;
                grid.Columns[i].Width = 100;
            }
            //grid.BackColor=griddefine.backColor;
        }

        public static void InitGrid(SourceGrid2.Grid grid, ConcurrentDictionary<string, string> dc)
        {
            //创建头
            grid.Redim(1, dc.Count);
            grid.FixedRows = 1;
            //griddefine.grid=grid;
            int i = 0;
            foreach (var item in dc)
            {
                SourceGrid2.Cells.Real.ColumnHeader c = new SourceGrid2.Cells.Real.ColumnHeader();
                c.EnableSort = false;
                //c.BackColor=griddefine.HeadColor;
                c.Value = item.Value;
                grid[0, i] = c;
                c.EnableSort = true;
                //grid.Columns[i].Width=griddefine[i].width;
                grid.Columns[i].Width = 100;
                i++;
            }
            //grid.BackColor=griddefine.backColor;

        }

        public static void fillgrid(Grid grid, DataSet ds)
        {
            //GridHelper.clear(grid,define);

            if (ds == null) return;

            if (ds.Tables.Count == 0) return;
            DataTable dt = ds.Tables[0];

            //更新所有的数据
            initgrid(grid, dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                updaterowwithdata(grid, dt, i + 1);
            }
        }



        public static void updaterowwithdata(Grid grid, DataTable dt, GridDefine define, int row)
        {
            int l = row;

            //添加数据
            //如果行不够那么需要添加新的行
            if (grid.Rows.Count - 1 < row)
            {
                grid.Redim(row, grid.Columns.Count);
                GridHelper.AddRow(grid, define, true);
                //	GridHelper.enablerow(grid,row,define);
            }
            if (grid[row, 0] == null) GridHelper.AddRow(grid, define, true);

            //define.Set(l,o);
            for (int i = 0; i < define.Count; i++)
            {
                if (grid[l, i] != null)
                {
                    object oo = dt.Rows[row - 1][define[i].name];
                    grid[l, i].Value = oo;
                }
            }
            GridHelper.EnableEditRow(grid, row, define);
        }
        public static void updaterowwithdata(Grid grid, DataTable dt, int row)
        {
            int l = row;

            //添加数据
            //如果行不够那么需要添加新的行
            DataRow dr = dt.Rows[row - 1];
            if (grid.Rows.Count - 1 < row)
            {
                grid.Redim(row + 1, dt.Columns.Count);

                //	GridHelper.enablerow(grid,row,define);
            }

            //define.Set(l,o);

            for (int i = 0; i < dr.ItemArray.Length; i++)
            {
                if (grid[l, i] == null) grid[l, i] = new Cell();

                {
                    object oo = dt.Rows[row - 1][i];
                    grid[l, i].Value = oo;
                }
            }

        }




        public static void updaterowwithdata<T>(Grid grid, List<T> list, int row, int colNums)
        {
            int l = row;

            //添加数据
            //如果行不够那么需要添加新的行

            if (grid.Rows.Count - 1 < row)
            {
                grid.Redim(row + 1, colNums);
                //	GridHelper.enablerow(grid,row,define);
            }
            throw new Exception("updaterowwithdata");
            //for (int i = 0; i < colNums; i++)
            //{
            //    if (grid[l, i] == null) grid[l, i] = new Cell();
            //    {
            //        object oo = dt.Rows[row - 1][i];
            //        grid[l, i].Value = oo;
            //    }
            //}

        }

        public static void updaterowwithdata(Grid grid, object o, GridDefine define, int row)
        {
            int l = row;

            //添加数据
            //如果行不够那么需要添加新的行
            if (grid.Rows.Count - 1 < row)
            {
                grid.Redim(row, grid.Columns.Count);
                GridHelper.AddRow(grid, define, true);
                GridHelper.enablerow(grid, row, define);
            }


            define.Set(l, o);
            define.inreload = true;
            GridHelper.enablerow(grid, row, define);
            for (int i = 0; i < define.Count; i++)
            {
                if (grid[l, i] != null)
                {
                    object obj = null;
                    if (define[i].storename == null)
                    {
                        //obj = cenetcom.util.refutil.tools.propfieldvalue(o, define[i].name);
                    }
                    else
                    {
                        //obj = cenetcom.util.refutil.tools.propfieldvalue(o, define[i].storename);
                    }
                    grid[l, i].Value = obj;

                    if (define[i].addlink)
                    {
                        //添加到 Cell上的附加信息
                        string prop = define[i].linkprop;
                        //grid[l,i].Key=cenetcom.util.refutil.tools.propfieldvalue(o,prop);
                    }
                }
            }
            define.inreload = false;
            GridHelper.EnableEditRow(grid, row + 1, define);
            define.UpdateSummary();
        }
        public static int addrowwithdata(Grid grid, object o, GridDefine define, bool edit)
        {
            int l = findemptyrow(grid, define);
            if (l == -1)
            {
                AddRow(grid, define, edit);
                l = grid.Rows.Count - 1;
                GridHelper.enablerow(grid, l, define);

            }

            define.Set(l, o);
            //添加数据

            for (int i = 0; i < define.Count; i++)
            {
                //grid[l,i].Value=cenetcom.util.refutil.tools.propfieldvalue(o,define[i].name);
            }
            return l;
        }
        public static int findemptyrow(Grid grid, GridDefine define)
        {
            //从 1 到 当前行 发现 newline 属性为空的行
            int x = -1;
            for (int i = 0; i < define.Count; i++)
            {
                if (define[i].newline) x = i;
            }
            if (!define.linecontrol || x == -1) return -1;

            for (int i = 1; i < grid.Rows.Count; i++)
            {
                if (Convert.ToString(grid[i, x].Value) == "" || grid[i, x].Value == null) return i;
            }
            return -1;

        }
        public static void AddRow(Grid grid, DataSet ds, bool edit)
        {
            if (ds == null || ds.Tables.Count == 0) return;
            int row = grid.Rows.Count - 1;
            grid.Rows.Insert(row + 1);
            for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
            {
                grid[row + 1, i] = new SourceGrid2.Cells.Real.Cell();
            }
        }
        public static void AddRow(Grid grid, GridDefine define, bool edit)
        {
            AddRow(grid, define, edit, false);
        }



        public static void AddRow(Grid grid, GridDefine define, bool edit, bool startline)
        {

            int row = grid.Rows.Count - 1;
            grid.Rows.Insert(row + 1);

            //创建本列上所有的单元格
            for (int i = 0; i < define.Count; i++)
            {
                #region 创建单元格
                Cell c = new Cell();


                if (!define[i].ReadOnly)
                {
                    if (edit)
                    {
                        //c.EditableMode=
                        c.InputType = define[i].type;
                        if (!define[i].check)
                        {
                            c.DataModel = new SourceGrid2.DataModels.EditorTextBox(typeof(string));
                            c.DataModel.EditableMode = _editableMode;
                        }
                    }
                }
                c.DecLen = define[i].declen;
                InputType it = define[i].type;
                if (it == InputType.Double || it == InputType.DoubleAboveZero || it == InputType.DoubleZero || it == InputType.Int)
                {
                    c.DataModel = new SourceGrid2.DataModels.EditorTextBox(typeof(double));
                    if (it == InputType.Int)
                    {
                        c.DataModel = new SourceGrid2.DataModels.EditorTextBoxNumeric(typeof(int));
                    }
                }
                else
                {
                    if (!define[i].check)
                        c.DataModel = new SourceGrid2.DataModels.EditorTextBox(typeof(string));

                }
                if (define[i].select)
                {
                    c.vals = define[i].vals;

                    c.DataModel = new SourceGrid2.DataModels.EditorComboBox(typeof(string));
                    c.DataModel.EditableMode = _editableMode;
                }

                if (define[i].percent || define[i].type == InputType.Percent)
                {


                    SourceGrid2.DataModels.EditorTextBox l_PercentEditor = new SourceGrid2.DataModels.EditorTextBox(typeof(double));

                    l_PercentEditor.AllowNull = false;
                    l_PercentEditor.DefaultValue = 0.0;
                    c.DataModel = new SourceGrid2.DataModels.EditorTextBoxNumeric(typeof(double));

                    c.DataModel = l_PercentEditor;
                }
                if (define[i].time)
                {
                    c.DataModel = new SourceGrid2.DataModels.EditorDateTime();

                }
                if (define[i].check)
                {

                    c = new SourceGrid2.Cells.Real.CheckBox(false);
                }
                if (define[i].selectobject != null)
                {
                    //c.DataModel=new PickerEditor(define[i].selectobject,define[i].selectfield,define[i].filter,define[i].fastkey);
                    c.DataModel.EditableMode = _editableMode;
                    c.Arrow = true;
                }


                if (define[i].currency)
                {
                    c.BackGround = true;
                }

                if ((row) % 2 == 0)
                {
                    c.BackColor = define.lightColor;
                }
                else { c.BackColor = define.darkColor; }
                //
                grid[row + 1, i] = c;
                //grid[row + 1, i] = new SourceGrid2.Cells.Real.Cell("Hello " , typeof(string));

                //c.ValueChanged += new EventHandler(define.OnValueChange);
                if (define[i].ReadOnly)//||define[i].check)
                {
                    c.EditableMode = EditableMode.None;
                    c.EnableEdit = false;
                }
                else
                {

                    if (c.DataModel != null) c.DataModel.EditableMode = _editableMode;

                }
                if (define.report) c.report = true;
                #endregion

            }

            if (define.linecontrol)
            {
                disablerow(grid, row + 1, define);
            }

            if (startline)
            {
                DisableEditRow(grid, row + 1, define);
            }
        }

        public static void AddRowForEdit(Grid grid, GridDefine define, bool startline)
        {
            bool edit = true;
            int row = grid.Rows.Count - 1;
            grid.Rows.Insert(row + 1);

            //创建本列上所有的单元格
            for (int i = 0; i < define.Count; i++)
            {
                #region 创建单元格

                Cell c = new Cell();

                //var m_DataModel = new SourceGrid2.DataModels.EditorTextBox(typeof(string));
                c.Value = "";
                //图片特殊处理
                if (define[i].ColPropertyInfo == null)
                {
                    return;
                }
                if (define[i].ColPropertyInfo.PropertyType.FullName == "System.Byte[]")
                {
                    SourceGrid2.VisualModels.Common m_VisualModel1 = new SourceGrid2.VisualModels.Common();
                    //m_VisualModel1.Image =
                    //m_VisualModel1.ImageAlignment = ContentAlignment.MiddleRight;
                }

                //c.DataModel = GetDataModel(define[i]);

                c.EditableMode = SourceGrid2.EditableMode.Focus | SourceGrid2.EditableMode.AnyKey | SourceGrid2.EditableMode.SingleClick; 
                c = GetGridCell(define[i]);
                grid[row + 1, i] = c;
                if (grid[row + 1, i].DataModel == null)
                {
                    grid[row + 1, i].DataModel = SourceGrid2.Utility.CreateDataModel(define[i].ColPropertyInfo.PropertyType);
                }
                grid[row + 1, i].DataModel.Validating += _Editor_Validating;
                grid[row + 1, i].DataModel.Validated += _Editor_Validated;
                grid[row + 1, i].DataModel.ValidatingForTagValue += DataModel_ValidatingForTagValue;
                grid[row + 1, i].DataModel.ValidatedForTagValue += DataModel_ValidatedForTagValue; 

                /* 转换
                 	m_CellEditor_Price = new SourceGrid2.DataModels.EditorTextBox(typeof(double));
			m_CellEditor_Price.TypeConverter = new SourceLibrary.ComponentModel.Converter.CurrencyTypeConverter(typeof(double));
                 */
                continue;




                if (!define[i].ReadOnly)
                {
                    if (edit)
                    {

                        c.InputType = define[i].type;
                        if (!define[i].check)
                        {
                            c.DataModel = new SourceGrid2.DataModels.EditorTextBox(typeof(string));
                            c.DataModel.EditableMode = _editableMode;
                        }
                    }
                }
                c.DecLen = define[i].declen;
                InputType it = define[i].type;
                if (it == InputType.Double || it == InputType.DoubleAboveZero || it == InputType.DoubleZero || it == InputType.Int)
                {
                    c.DataModel = new SourceGrid2.DataModels.EditorTextBox(typeof(double));
                    if (it == InputType.Int)
                    {
                        c.DataModel = new SourceGrid2.DataModels.EditorTextBoxNumeric(typeof(int));
                    }
                }
                else
                {
                    if (!define[i].check)
                        c.DataModel = new SourceGrid2.DataModels.EditorTextBox(typeof(string));

                }
                if (define[i].select)
                {
                    c.vals = define[i].vals;

                    c.DataModel = new SourceGrid2.DataModels.EditorComboBox(typeof(string));
                    c.DataModel.EditableMode = _editableMode;
                }

                if (define[i].percent || define[i].type == InputType.Percent)
                {


                    SourceGrid2.DataModels.EditorTextBox l_PercentEditor = new SourceGrid2.DataModels.EditorTextBox(typeof(double));

                    l_PercentEditor.AllowNull = false;
                    l_PercentEditor.DefaultValue = 0.0;
                    c.DataModel = new SourceGrid2.DataModels.EditorTextBoxNumeric(typeof(double));

                    c.DataModel = l_PercentEditor;
                }
                if (define[i].time)
                {
                    c.DataModel = new SourceGrid2.DataModels.EditorDateTime();

                }
                if (define[i].check)
                {

                    c = new SourceGrid2.Cells.Real.CheckBox(false);
                }
                if (define[i].selectobject != null)
                {
                    //c.DataModel=new PickerEditor(define[i].selectobject,define[i].selectfield,define[i].filter,define[i].fastkey);
                    c.DataModel.EditableMode = _editableMode;
                    c.Arrow = true;
                }


                if (define[i].currency)
                {
                    c.BackGround = true;
                }

                if ((row) % 2 == 0)
                {
                    c.BackColor = define.lightColor;
                }
                else { c.BackColor = define.darkColor; }
                //
                grid[row + 1, i] = c;
                //grid[row + 1, i] = new SourceGrid2.Cells.Real.Cell("Hello " , typeof(string));

                //                c.ValueChanged += new EventHandler(define.OnValueChanged);
                if (define[i].ReadOnly)//||define[i].check)
                {
                    c.EditableMode = EditableMode.None;
                    c.EnableEdit = false;
                }
                else
                {

                    if (c.DataModel != null) c.DataModel.EditableMode = _editableMode;

                }
                if (define.report) c.report = true;
                #endregion

            }

            if (define.linecontrol)
            {
                disablerow(grid, row + 1, define);
            }

            if (startline)
            {
                DisableEditRow(grid, row + 1, define);
            }
        }

        private static void DataModel_ValidatedForTagValue(object sender, CellEventArgs e)
        {
            Cell ce = e.Cell as Cell;
            if (ce.TagValue != null)
            {
                if (ce.TagValue is RUINORERP.Model.tb_Prod)
                {
                    //
                    if (_gridDefind.grid.Columns[0].ColumnName != null)
                    {

                    }

                    for (int c = 0; c < _gridDefind.grid.Columns.Count; c++)
                    {
                        if (true)
                        {

                        }
                    }
                }
            }

        }

        public static void UpdateDataToRow<T>(T obj, int row)
        {
            int l = row;

            //添加数据
            //如果行不够那么需要添加新的行

            if (_gridDefind.grid.Rows.Count - 1 < row)
            {
                _gridDefind.grid.Redim(row + 1, _gridDefind.grid.Columns.Count);
                //	GridHelper.enablerow(grid,row,define);
            }

            for (int i = 0; i < _gridDefind.grid.Columns.Count; i++)
            {
                if (_gridDefind.grid[l, i] == null)
                {
                    _gridDefind.grid[l, i] = new Cell();
                }
                else
                {

                    _gridDefind.grid[l, i].Value = 1;
                }
            }

        }



        private static void DataModel_ValidatingForTagValue(object sender, ValidatingTagValueEventArgs e)
        {

        }

        private static Cell GetGridCell(GridDefineColumnItem dci)
        {
            //不同情况会有多种类型，先逻辑处理得到最终的类型
            Type newcolType;

            Cell c = new Cell();
            //c.Value = "";
            c.EnableEdit = true;
            c.EditableMode = SourceGrid2.EditableMode.Focus | SourceGrid2.EditableMode.AnyKey | SourceGrid2.EditableMode.SingleClick;
            SourceGrid2.DataModels.IDataModel idatamodel = new SourceGrid2.DataModels.EditorTextBox(typeof(string));
            System.Reflection.PropertyInfo pi = dci.ColPropertyInfo;
            //==
            if (pi.Name == "Unit_ID")
            {

            }

            // We need to check whether the property is NULLABLE
            if (pi.PropertyType.IsGenericType && pi.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                // If it is NULLABLE, then get the underlying type. eg if "Nullable<int>" then this will return just "int"
                newcolType = pi.PropertyType.GetGenericArguments()[0];
            }
            else
            {
                newcolType = pi.PropertyType;
            }

            #region 参考
            /*
            if (!pi.PropertyType.IsGenericType)
            {
                //非泛型
            }
            else
            {
                //泛型Nullable<>
                Type genericTypeDefinition = pi.PropertyType.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(Nullable<>))
                {
                }
                else
                {
                }
            }
            */
            #endregion
            //string[] l_CountryList = new string[] { "Italy", "France", "Spain", "UK", "Argentina", "Mexico", "Switzerland", "Brazil", "Germany", "Portugal", "Sweden", "Austria" };
            //idatamodel = new SourceGrid2.DataModels.EditorComboBox(typeof(string), l_CountryList, false);

            switch (newcolType.FullName)
            {

                case "System.Char":
                case "System.String":
                    //idatamodel = new SourceGrid2.DataModels.EditorTextBox(typeof(string));
                    idatamodel = new SourceGrid2.DataModels.EditorTextBox(typeof(string));
                    if (pi.Name == "Name")
                    {
                        EditorTextBoxButtonQuery _Editor = new EditorTextBoxButtonQuery(typeof(string));
                        idatamodel = _Editor;
                        //_editor = new SourceGrid.Cells.Editors.EditorQuery(typeof(string));
                        c.Value = "pingning";

                    }
                    c.DataModel = idatamodel;
                    break;

                case "System.Guid":

                    break;
                case "System.Decimal":
                case "System.Int16":
                case "System.Int32":
                case "System.Int64":
                    //实体中，判断如果是外键，特别是属性指定绑在中的。加上一个特性 给出一些参数方便后面自动加载
                    //idatamodel = new SourceGrid2.DataModels.EditorTextBoxButton(typeof(string));
                    idatamodel = new SourceGrid2.DataModels.EditorTextBoxNumeric(typeof(string));
                    c.DataModel = idatamodel;
                    break;
                case "System.Byte[]":

                    break;
                case "System.Boolean":
                    c = new SourceGrid2.Cells.Real.CheckBox(true);
                    break;
                case "System.Double":
                    c.DataModel = new SourceGrid2.DataModels.EditorTextBox(typeof(double));
                    break;
                case "System.DateTime":

                    //idatamodel = new SourceGrid2.DataModels.EditorDateTime(typeof(string));
                    //c = new SourceGrid2.Cells.Real.Cell(DateTime.Today, typeof(DateTime));
                    c.DataModel = SourceGrid2.Utility.CreateDataModel(typeof(DateTime));
                    //c.DataModel = idatamodel;
                    break;

                default:
                    c.DataModel = new SourceGrid2.DataModels.EditorTextBox(typeof(string));
                    //c.DataModel = idatamodel;
                    break;
            }
            //==
            return c;
        }

        private static void _Editor_Validated(object sender, CellEventArgs e)
        {

        }

        private static void _Editor_Validating(object sender, ValidatingCellEventArgs e)
        {

        }

        private static SourceGrid2.DataModels.IDataModel GetDataModel(GridDefineColumnItem dci, Cell c)
        {
            SourceGrid2.DataModels.IDataModel idatamodel = new SourceGrid2.DataModels.EditorTextBox(typeof(string));
            System.Reflection.PropertyInfo pi = dci.ColPropertyInfo;
            //==
            if (pi.PropertyType.FullName.Contains("System.Nullable"))
            {
                string[] l_CountryList = new string[] { "Italy", "France", "Spain", "UK", "Argentina", "Mexico", "Switzerland", "Brazil", "Germany", "Portugal", "Sweden", "Austria" };
                idatamodel = new SourceGrid2.DataModels.EditorComboBox(typeof(string), l_CountryList, false);
            }
            else
            {
                switch (pi.PropertyType.FullName)
                {

                    case "System.Char":
                    case "System.String":
                        idatamodel = new SourceGrid2.DataModels.EditorTextBox(typeof(string));

                        break;

                    case "System.Guid":

                        break;
                    case "System.Decimal":
                    case "System.Int16":
                    case "System.Int32":
                    case "System.Int64":
                        idatamodel = new SourceGrid2.DataModels.EditorTextBoxButton(typeof(string));

                        break;
                    case "System.Byte[]":

                        break;
                    case "System.Boolean":

                        c = new SourceGrid2.Cells.Real.CheckBox(true);
                        break;
                    case "System.Double":

                        break;
                    case "System.DateTime":
                        //idatamodel = new SourceGrid2.DataModels.EditorDateTime(typeof(string));
                        c = new SourceGrid2.Cells.Real.Cell(DateTime.Today, typeof(DateTime));
                        break;

                    default:
                        idatamodel = new SourceGrid2.DataModels.EditorTextBox(typeof(string));
                        break;
                }
            }
            //==
            return idatamodel;
        }

        private static SourceGrid2.DataModels.IDataModel GetDataModel(GridDefineColumnItem dci)
        {
            SourceGrid2.DataModels.IDataModel idatamodel = new SourceGrid2.DataModels.EditorTextBox(typeof(string));
            System.Reflection.PropertyInfo pi = dci.ColPropertyInfo;
            //==
            if (pi.PropertyType.FullName.Contains("System.Nullable"))
            {
                string[] l_CountryList = new string[] { "Italy", "France", "Spain", "UK", "Argentina", "Mexico", "Switzerland", "Brazil", "Germany", "Portugal", "Sweden", "Austria" };
                idatamodel = new SourceGrid2.DataModels.EditorComboBox(typeof(string), l_CountryList, false);
            }
            else
            {
                switch (pi.PropertyType.FullName)
                {

                    case "System.Char":
                    case "System.String":
                        idatamodel = new SourceGrid2.DataModels.EditorTextBox(typeof(string));

                        break;

                    case "System.Guid":

                        break;
                    case "System.Decimal":
                    case "System.Int16":
                    case "System.Int32":
                    case "System.Int64":
                        idatamodel = new SourceGrid2.DataModels.EditorTextBoxNumeric(typeof(string));

                        break;
                    case "System.Byte[]":

                        break;
                    case "System.Boolean":

                        //c = new SourceGrid2.Cells.Real.CheckBox(true);
                        break;
                    case "System.Double":

                        break;
                    case "System.DateTime":
                        //idatamodel = new SourceGrid2.DataModels.EditorDateTime(typeof(string));
                        //c = new SourceGrid2.Cells.Real.Cell(DateTime.Today, typeof(DateTime));
                        break;

                    default:
                        idatamodel = new SourceGrid2.DataModels.EditorTextBox(typeof(string));
                        break;
                }
            }
            //==
            return idatamodel;
        }


        /// <summary>
        /// 增加总计行
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="define"></param>
        public static void AddSummaryRow(Grid grid, GridDefine define)
        {
            grid.Summary = true;

            //添加  总和列
            for (int i = 0; i < define.Count; i++)
            {
                Cell c = new Cell();
                c.BackColor = define.SummaryColor;
                grid.SummaryCells[i] = c;
                if (i == 0)
                {
                    grid.SummaryCells[i].Value = "合  计:";
                    c.TextAlignment = ContentAlignment.MiddleCenter;
                    c.Font = new Font("宋体", 13);
                }
                grid.SummaryCells[i].BindToGrid(grid);
                if (define[i].currency) grid.SummaryCells[i].BackGround = true;
            }
        }

        public static void addrow(Grid grid, GridDefine define)
        {
            //添加一列 
            GridHelper.AddRow(grid, define, false);
        }
        public static void drawhead(SourceGrid2.Grid grid, string[] headname)
        {
            for (int i = 0; i < headname.Length; i++)
            {
                grid[0, i] = new SourceGrid2.Cells.Real.ColumnHeader(headname[i]);
            }
        }
        public static void drawhead(SourceGrid2.Grid grid, ArrayList headname)
        {
            for (int i = 0; i < headname.Count; i++)
            {
                grid[0, i] = new SourceGrid2.Cells.Real.ColumnHeader(headname[i]);
                //grid[0,i].Solids=true;
                //grid[0,i].BackColor=Color.LightSteelBlue;
            }

        }
        public static void drawhead(SourceGrid2.Grid grid, int i, string headname)
        {

            grid[0, i] = new SourceGrid2.Cells.Real.ColumnHeader(headname);
            //grid[0,i].Solids=true;
            //grid[0,i].BackColor=Color.LightSteelBlue;


        }
        public static Cell drawcell(SourceGrid2.Grid grid, int col, int row, object val)
        {

            Cell c = new Cell();
            c.Value = val;
            grid[row, col] = c;
            return c;
        }

        public static void drawwidth(SourceGrid2.Grid grid, int[] rowwidth)
        {
            for (int i = 0; i < rowwidth.Length; i++)
            {
                grid.Columns[i].Width = rowwidth[i];
            }
        }

        public static void ShowToGrid(Grid grid, DataSet ds, bool autofill)
        {
            ShowToGrid(grid, ds);
            if (autofill)
            {
                int r = GridHelper.gridmrows(grid);
                r = grid.Height / grid.DefaultHeight - 2;
                for (int i = 0; i < r; i++)
                {
                    AddRow(grid, ds, false);
                }
                grid.Height = (r + 2) * grid.DefaultHeight + grid.BorderWidth * 2;

            }
        }
        public static void ShowToGrid(Grid grid, DataSet ds)
        {
            grid.Redim(0, 0);
            if (ds == null || ds.Tables.Count == 0) return;
            DataTable dt = ds.Tables[0];
            grid.Redim(dt.Rows.Count + 1, dt.Columns.Count);
            //创建表头
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                Cell c = new SourceGrid2.Cells.Real.ColumnHeader(dt.Columns[i].ColumnName);

                //if (i!=dt.Columns.Count-1) c.Border.Right.Width=0;

                //c.Solids=true;
                c.BackColor = Color.LightSteelBlue;
                grid.Columns[i].Width = 100;//(i,100);
                grid[0, i] = c;
            }
            //添加所有的数据
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    Cell c = createcell(false);
                    //c.Border.Top.Width=0;
                    //if (i!=dt.Columns.Count-1) c.Border.Right.Width=0;
                    c.Value = dt.Rows[j].ItemArray[i];
                    grid[j + 1, i] = c;

                }
            }

        }
        public static SourceGrid2.Cells.Real.Cell createcell(bool editor)
        {
            Cell c = new SourceGrid2.Cells.Real.Cell();

            if (editor)
            {
                //c.CellEditor=new SourceGrid.CellEditor.TextBoxEditor();
                c.DataModel = new SourceGrid2.DataModels.EditorTextBox(typeof(string));
                c.EditableMode = _editableMode;
            }
            return c;
        }

        private static void define_CellValueChange(object sender, EventArgs e)
        {
            //更新对应的值
            Cell c = (Cell)sender;
            GridDefine gd = (GridDefine)c.Grid.Tag;
            // 获得对应行的数据

            object o = gd.Get(c.Row);
            if (o != null)
            {
                string name = gd[c.Column].name;
                //cenetcom.util.refutil.tools.setpropfieldvalue(o,name,c.Value);
                //Objects.Save(o);
            }
        }
        public static void 打印对象(object o)
        {
            if (o == null) return;
            string typename = o.GetType().FullName;
            //新打印样式定义  printd=new  新打印样式定义(o);
            //printd.PrintReport(o);

        }
        public static void 打印定义(object o)
        {
            if (o == null) return;
            string typename = o.GetType().FullName;
            //新打印样式定义  printd=new  新打印样式定义(o);
            //printd.ShowDialog();

        }

    }
}
