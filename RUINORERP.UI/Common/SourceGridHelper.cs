using DevAge.Drawing;
using RUINORERP.UI.UCSourceGrid;
using SourceGrid;
using SourceGrid.Cells;
using SourceGrid.Selection;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RUINORERP.UI.Common
{
    public class SourceGridHelper
    {

        private SourceGridDefine _sourceGridDefine;

        public SourceGridDefine SourceGridDefine { get => _sourceGridDefine; set => _sourceGridDefine = value; }

        public void InitGrid(SourceGrid.Grid grid, SourceGridDefine griddefine, bool autofill)
        {
            _sourceGridDefine = griddefine;
            if (griddefine.Count == 0)
            {
                return;
            }
            InitGrid(grid, griddefine);

            //if (autofill)
            //{
            //    //重新设定 最后一个的大小
            //    int x = grid.Width - grid.BorderWidth - griddefine.length();
            //    if (x > 0)
            //        griddefine[griddefine.Count - 1].width += x;
            //    if (grid.Columns[griddefine.Count - 1].Width < griddefine[griddefine.Count - 1].width)
            //        grid.Columns[griddefine.Count - 1].Width = griddefine[griddefine.Count - 1].width;
            //}


            //GridHelper.AddSummaryRow(grid, griddefine);
            //grid.Redim(20,grid.Cols);
            // 按照 grid的 大小 添加行数
            //int r = GridHelper.gridmrows(grid);
            //grid.BorderWidth=1;
            //grid.BorderStyle=BorderStyle.Fixed3D;
            //r = grid.Height / grid.DefaultHeight - 2;
            //if (r <= 2)
            //{
            //    r = 15;
            //}
            //for (int i = 1; i < r; i++)
            //{
            //    AddRowForEdit(grid, griddefine, false);
            //}
            // grid.Height = (r + 2) * grid.DefaultHeight + grid.BorderWidth * 2;
            //  griddefine.needheight = (r + 1) * grid.DefaultHeight + grid.BorderWidth * 2 + grid.SummaryHeight + 4;

        }

        public void InitGrid(SourceGrid.Grid grid, SourceGridDefine griddefine)
        {
            //启动时默认无选中
            grid.Selection.FocusStyle = SourceGrid.FocusStyle.None;
            //创建头
            grid.Redim(1, griddefine.Count);
            grid.FixedColumns = 1;
            grid.FixedRows = 1;
            //grid.AutoStretchColumnsToFitWidth = true;
            //grid.AutoStretchRowsToFitHeight = true;
            //grid.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;

            //grid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            grid[0, 0] = new SourceGrid.Cells.Header(null);
            //if (HasRowHeader)
            //{
            grid.Columns.SetWidth(0, 40);
            grid.Columns[0].AutoSizeMode = SourceGrid.AutoSizeMode.None;
            grid.Columns[0].Width = 25;

            //ColumnHeader view
            SourceGrid.Cells.Views.ColumnHeader viewColumnHeader = new SourceGrid.Cells.Views.ColumnHeader();
            DevAge.Drawing.VisualElements.ColumnHeader backHeader = new DevAge.Drawing.VisualElements.ColumnHeader();
            //backHeader.BackColor = Color.Maroon;
            backHeader.Style = ControlDrawStyle.Disabled;
            backHeader.BackColor = Color.FromArgb(152, 152, 200);
            backHeader.Border = DevAge.Drawing.RectangleBorder.NoBorder;
            viewColumnHeader.Background = backHeader;
            viewColumnHeader.ForeColor = Color.Black;
            viewColumnHeader.Font = new Font("宋体", 10, FontStyle.Bold);
            viewColumnHeader.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
            #region 创建列
            //排除列头，因为有行头
            for (int i = 0; i < griddefine.Count; i++)
            {
                //HasHeader
                SourceGrid.Cells.ColumnHeader columnHeader = new SourceGrid.Cells.ColumnHeader();
                columnHeader.View = viewColumnHeader;
                columnHeader.Value = griddefine[i].name;
                columnHeader.AutomaticSortEnabled = false;//禁止排序
                GetColumnEditor(griddefine[i]);
                if (true && i == 0)
                {
                    //左上角
                    columnHeader.Value = "项";
                    columnHeader.AutomaticSortEnabled = false;
                }
                else
                {
                    columnHeader.Value = griddefine[i].name;
                }
                //===列宽控制作
                //if (i != grid.ColumnsCount - 1)//不是最后一列

                //{

                //    grid.Columns[i].Width = 90; colsWidth[i];

                //    otherColsWidth += colsWidth[i];

                //}

                //else //设置最后一列铺满整个grid

                //    grid.Columns[i].Width = grid1.Width - otherColsWidth - 2 * i;

                //==
                grid[0, i] = columnHeader;


                //grid.Columns[i].Width = griddefine[i].width;
                /*
                SourceGrid.Cells.ColumnHeader header = new SourceGrid.Cells.ColumnHeader("Header ");
                header.AutomaticSortEnabled = false;
                DevAge.Drawing.RectangleBorder border = new DevAge.Drawing.RectangleBorder();
                border.SetColor(Color.FromArgb(255, 128, 128, 192)); //边框颜色
                border.SetWidth(1);//边框
                header.View.Border = border;
                header.View.BackColor = (Color.FromArgb(255, 128, 128, 192)); //颜色
                header.View.ForeColor = (Color.FromArgb(255, 128, 128, 192)); //颜色

                //header.ColumnSelectorEnabled = true;
                //header.ColumnFocusEnabled = true;
                header.View.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
                grid[0, i] = header;
                */
            }
            #endregion

            SetColumnsWidth(grid);
            if (grid.Tag is SourceGridDefine)
            {
                SourceGridDefine dd = (SourceGridDefine)grid.Tag;
            }
            grid.Tag = griddefine;
            griddefine.grid = grid;



            grid.AutoSizeCells();
        }


        public void AddRowForEdit(SourceGrid.Grid grid, SourceGridDefine define, bool HasRowHeader)
        {
            bool edit = true;
            int row = grid.Rows.Count - 1;
            grid.Rows.Insert(row + 1);

            //grid.Rows.SetHeight(行数, 高度);
            if (HasRowHeader)
            {
                grid.Columns.SetWidth(0, 40);
            }
            //Grid_Minerals.Columns[0].Width = 20; 某一列宽度
            //创建本列上所有的单元格的值等属性
            for (int i = 0; i < define.Count; i++)
            {
                if (HasRowHeader && i == 0)
                {
                    if (true)
                    {

                    }
                    //行号
                    //SourceGrid.Cells.RowHeader rh = new SourceGrid.Cells.RowHeader((row + 1).ToString());
                    SourceGrid.Cells.RowHeader rh = new SourceGrid.Cells.RowHeader("".ToString());
                    grid[row + 1, i] = rh;
                    continue;
                }
                else
                {
                    //值
                    define[i].EditorForColumn.EnableEdit = true;
                    //第二列不是行头
                    SourceGrid.Cells.Cell c = new SourceGrid.Cells.Cell(null, define[i].EditorForColumn);
                    //这里要重构
                    if (define[i].name == "品名")
                    {
                        //                        DependencyColumn boolToStatus = new DependencyColumn(1);
                        DependencyColumn boolToStatus = new DependencyColumn(define,1);
                        boolToStatus.ConvertFunction = delegate (object valValue)
                        {
                            if (valValue is RUINORERP.Model.tb_Product)
                            {
                                return (valValue as Model.tb_Product).Name;
                            }
                            else
                            {
                                return valValue;
                            }
                        };
                        c.AddController(boolToStatus);
                    }
                    //c = GetGridCell(define[i]);
                    c.Value = row.ToString() + i.ToString();

                    //c.Editor = GetColumnEditor(define[i]);
                    grid[row + 1, i] = c;
                    grid.Rows[row + 1].Height = 20;

                    //图片特殊处理
                    if (define[i].ColPropertyInfo == null)
                    {
                        return;
                    }
                }


            }

            //隔行显示 要优化不能每次执行 或循环
            SourceGrid.Cells.Views.Cell transparentView = new SourceGrid.Cells.Views.Cell();
            transparentView.BackColor = Color.White;
            //字体
            //transparentView.Font = new Font(FontFamily.GenericSansSerif, 9, FontStyle.Bold);
            transparentView.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
            transparentView.ImageAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
            SourceGrid.Cells.Views.Cell semiTransparentView = new SourceGrid.Cells.Views.Cell();
            semiTransparentView.BackColor = Color.LightCyan;
            semiTransparentView.TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
            semiTransparentView.ImageAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter;
            CheckBoxBackColorAlternate viewCheckBox = new CheckBoxBackColorAlternate(Color.LightCyan, Color.White);
            for (int r = 1; r < grid.RowsCount; r++)
            {
                for (int j = 1; j < grid.ColumnsCount; j++)
                {
                    if ((r - 1) % 2 == 0) grid[r, j].View = transparentView;
                    else grid[r, j].View = semiTransparentView;
                }
            }
        }


        public void AddRow(SourceGrid.Grid grid, SourceGridDefine define, bool HasRowHeader)
        {
            int row = grid.RowsCount;
            grid.Rows.Insert(row);

            for (int i = 0; i < define.Count; i++)
            {
                if (i == 0 && HasRowHeader)
                {
                    grid[row, 0] = new SourceGrid.Cells.RowHeader(null);
                    continue;
                }
                grid[row, i] = new SourceGrid.Cells.Cell("", define[i].EditorForColumn);
            }
            grid.Selection.FocusRow(row);
        }






        private Cell GetGridCell(SourceGridDefineColumnItem dci)
        {
            //不同情况会有多种类型，先逻辑处理得到最终的类型
            Type newcolType;

            SourceGrid.Cells.Cell c = new SourceGrid.Cells.Cell(null, dci.EditorForColumn);
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


            switch (newcolType.FullName)
            {

                case "System.Char":
                case "System.String":
                    c = new SourceGrid.Cells.Cell("", dci.EditorForColumn);


                    break;

                case "System.Guid":

                    break;
                case "System.Decimal":
                case "System.Int16":
                case "System.Int32":
                case "System.Int64":
                    //实体中，判断如果是外键，特别是属性指定绑在中的。加上一个特性 给出一些参数方便后面自动加载
                    //idatamodel = new SourceGrid2.DataModels.EditorTextBoxButton(typeof(string));
                    c = new SourceGrid.Cells.Cell(5, dci.EditorForColumn);
                    break;
                case "System.Byte[]":

                    break;
                case "System.Boolean":
                    //c = new SourceGrid.Cells.CheckBox(null, true);
                    c = new SourceGrid.Cells.Cell(true, typeof(Boolean));
                    break;
                case "System.Double":
                    c = new SourceGrid.Cells.Cell(1.5, typeof(double));
                    break;
                case "System.DateTime":
                    //idatamodel = new SourceGrid2.DataModels.EditorDateTime(typeof(string));
                    //                    c = new SourceGrid.Cells.Cell(DateTime.Now, typeof(DateTime));
                    c = new SourceGrid.Cells.Cell(null, typeof(DateTime));
                    //c.DataModel = idatamodel;
                    break;
                default:
                    break;
            }

            //==


            return c;
        }

        private SourceGrid.Cells.Editors.EditorBase GetColumnEditor(SourceGridDefineColumnItem dci)
        {
            //SourceGrid.Cells.Views.Cell captionModel = new SourceGrid.Cells.Views.Cell();
            //captionModel.BackColor = grid.BackColor;

            SourceGrid.Cells.Editors.EditorBase _editor = new SourceGrid.Cells.Editors.TextBox(typeof(string));
            _editor.EditableMode = SourceGrid.EditableMode.Focus | SourceGrid.EditableMode.AnyKey | SourceGrid.EditableMode.SingleClick;

            System.Reflection.PropertyInfo pi = dci.ColPropertyInfo;


            Type newcolType;
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

            switch (newcolType.FullName)
            {

                case "System.Char":
                case "System.String":
                    _editor = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                    if (pi.Name == "Name")
                    {
                        _editor = new UI.UCSourceGrid.EditorQuery(typeof(Model.tb_Product));
                        //_editor = new SourceGrid.Cells.Editors.EditorQuery(typeof(string));

                    }
                    break;

                case "System.Guid":

                    break;
                case "System.Decimal":
                    _editor = new SourceGrid.Cells.Editors.TextBoxCurrency(typeof(double));
                    break;
                case "System.Int16":
                case "System.Int32":
                case "System.Int64":
                    _editor = new SourceGrid.Cells.Editors.TextBoxNumeric(typeof(int));

                    break;
                case "System.Byte[]":
                    _editor = new SourceGrid.Cells.Editors.ImagePicker();
                    break;
                case "System.Boolean":
                    //_editor = new SourceGrid.Cells.CheckBox(null, true);
                    //c = new CheckBox(true);
                    break;
                case "System.Double":
                    _editor = new SourceGrid.Cells.Editors.TextBoxCurrency(typeof(double));
                    break;
                case "System.DateTime":
                    _editor = new SourceGrid.Cells.Editors.TextBoxUITypeEditor(typeof(DateTime));
                    //idatamodel = new SourceGrid2.DataModels.EditorDateTime(typeof(string));
                    // c = new SourceGrid2.Cells.Real.Cell(DateTime.Today, typeof(DateTime));
                    break;

                default:
                    _editor = new SourceGrid.Cells.Editors.TextBox(typeof(string));
                    break;
            }
            //这里不设置，验证会生效
            _editor.AllowNull = true;
            _editor.EnableEdit = true;
            dci.EditorForColumn = _editor;

            //==
            return _editor;
        }


        /// <summary>
        /// 增加总计行
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="define"></param>
        public static void AddSummaryRow(Grid grid, SourceGridDefine define)
        {
            //grid.Summary = true;

            //添加  总和列
            for (int i = 0; i < define.Count; i++)
            {
                Cell c = new Cell();
                //c.View.BackColor = define.SummaryColor;
                //grid.SummaryCells[i] = c;
                //if (i == 0)
                //{
                //    grid.SummaryCells[i].Value = "合  计:";
                //    c.TextAlignment = ContentAlignment.MiddleCenter;
                //    c.Font = new Font("宋体", 13);
                //}
                //grid.SummaryCells[i].BindToGrid(grid);
                //if (define[i].currency) grid.SummaryCells[i].BackGround = true;
            }
        }

        public void SetAppearance()
        {

        }

        public void SetColumnsWidth(SourceGrid.Grid grid)
        {
            for (int i = 0; i < grid.Columns.Count; i++)
            {
                if (i == 0)
                {
                    grid.Columns[0].Width = 30;
                }
                else
                {
                    grid.Columns[i].Width = 90;
                }
                grid.Columns[i].AutoSizeMode = SourceGrid.AutoSizeMode.None; //禁止调整宽



            }
        }


    }
}
