using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RUINORERP.UI.Properties;
using SourceGrid.Selection;
using ContentAlignment = DevAge.Drawing.ContentAlignment;

namespace RUINORERP.UI.UControls
{
    public partial class GridUc : UserControl
    {
        public SourceGrid.Grid DataGrid => grid;
        public int GridHeight { get; set; }
        /// <summary>
        /// 最大行数10000
        /// </summary>
        public int RowMax { get; set; }
        /// <summary>
        /// 有无列头，0无，1有
        /// </summary>
        private int _top = 1;
        /// <summary>
        /// 多选状态记录
        /// </summary>
        private bool[] IsChoiceLst = new bool[30];
        ///切图回调函数,一个是开锁信息，一个是选中还是不选中
        public Action<Object, bool> LCheckBoxAction;

        ///点击显示完整文字事件，文字，第几列
        public Action<object, int> LTxtBoxAction;

        ///点击显示完整文字事件，文字，第几列
        public Action<object> LDeleteAction;
        //单击控制器
        SourceGrid.Cells.Controllers.Button buttonClickEvent = new SourceGrid.Cells.Controllers.Button();
        //单击控制器
        SourceGrid.Cells.Controllers.Button buttonTxtClickEvent = new SourceGrid.Cells.Controllers.Button();

        //单击控制器
        SourceGrid.Cells.Controllers.Button buttonDeleteClickEvent = new SourceGrid.Cells.Controllers.Button();
        /// <summary>
        /// 记录上一个点击cell
        /// </summary>
        private static SourceGrid.Cells.Cell _cell;
        /// <summary>
        /// 
        /// </summary>
        public GridUc()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 添加到第一位状态
        /// </summary>
        private void AddIsChoice(bool isChoice)
        {
            for (int i = IsChoiceLst.Length - 1; i > 0; i--)
            {
                IsChoiceLst[i] = IsChoiceLst[i - 1];
            }
            IsChoiceLst[0] = isChoice;
        }

        #region 加载grid数据
        /// <summary>
        /// 加载grid数据
        /// </summary>
        public void Grid_Load(List<Heaher> hearherLst, int height, int rowMax = 10000)
        {
            buttonClickEvent.Executed += new EventHandler(CellButton_Click);
            buttonTxtClickEvent.Executed += new EventHandler(CellButtonTxt_Click);
            buttonDeleteClickEvent.Executed += new EventHandler(CellButtonDelete_Click);
            RowMax = rowMax;
            if (hearherLst == null)
            {
                _top = 0;
                return;
            }
            _top = 1;
            grid.Rows.Clear();
            grid.ColumnsCount = hearherLst.Count;

            #region 标头样式

            SourceGrid.Cells.Views.Cell titleModel = new SourceGrid.Cells.Views.Cell
            {
                Font = new Font("微软雅黑", 15F, FontStyle.Regular,
                    GraphicsUnit.Point, 134),
                BackColor = Color.FromArgb(14, 144, 210),
                ForeColor = Color.White,
                TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter
            };

            #endregion
            grid.Rows.Insert(0);
            for (int i = 0; i < hearherLst.Count; i++)
            {
                grid[0, i] = new SourceGrid.Cells.Cell(hearherLst[i].Title) { View = titleModel };
                grid.Columns[i].Width = hearherLst[i].Width;
            }
            grid.Rows[0].Height = height;
        }

        /// <summary>
        /// 加载grid数据
        /// </summary>
        /// <param name="widthInts">列宽</param>
        /// <param name="height">高度</param>
        /// <param name="maxCount">不能超过的数量</param>
        public void Grid_Load(int[] widthInts, int height = 35, int maxCount = 10)
        {
            _top = 0;
            grid.Rows.Clear();
            grid.ColumnsCount = widthInts.Length;
            GridHeight = height;
            RowMax = maxCount;

            for (int i = 0; i < widthInts.Length; i++)
            {
                grid.Columns[i].Width = widthInts[i];
            }
        }
        #endregion

        #region 添加一条数据至第一位

        /// <summary>
        /// 添加一条数据至第一位
        /// </summary>
        /// <param name="stLst"></param>
        /// <param name="tagObj">tag记录的对象</param>
        public void AddItem(List<string> stLst, params Object[] tagObj)
        {
            if (stLst == null) return;

            //设置列
            if (grid.ColumnsCount < stLst.Count)
            {
                grid.ColumnsCount = stLst.Count;
            }

            SourceGrid.Cells.Views.Cell captionModel = new SourceGrid.Cells.Views.Cell
            {
                TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter,
                ForeColor = Color.FromArgb(33, 33, 33),
                Font = new Font("微软雅黑", 15F, FontStyle.Regular,
                    GraphicsUnit.Point, 134)
            };

            if (grid.Rows.Count % 2 == 0)
            {
                captionModel.BackColor = Color.White;
            }
            else
            {
                captionModel.BackColor = Color.FromArgb(227, 241, 251);
            }

            grid.Rows.Insert(_top);
            for (var i = 0; i < stLst.Count && i < grid.ColumnsCount; i++)
            {
                //获取关键信息，这个关键信息只跟整条数据有关，
                //如果多个就分别放在该条的单元格内 跟该单元格并没什么关系
                Object tag = "";
                if (i < tagObj.Length)
                {
                    tag = tagObj[i];
                }

                if (stLst[i] == "False" || stLst[i] == "True")
                {
                    grid[_top, i] = CreateImgCell(stLst[i], captionModel);

                }
                else
                {
                    grid[_top, i] = new SourceGrid.Cells.Cell(stLst[i]) { View = captionModel };
                }
                grid[_top, i].Tag = tag;
            }
            grid.Rows[_top].Height = GridHeight;

            //不能超过23
            if (grid.RowsCount > RowMax)
            {
                grid.Rows.Remove(grid.RowsCount - 1);
            }

            //grid.AutoSizeCells();
        }

        /// <summary>
        /// 添加一条数据至第一位，数据加颜色
        /// </summary>
        /// <param name="stLst"></param>
        /// <param name="tagObj">tag记录的对象</param>
        public void AddItemColor(List<string> stLst, int[] intColors, Color color, params Object[] tagObj)
        {
            if (stLst == null) return;

            //设置列
            if (grid.ColumnsCount < stLst.Count)
            {
                grid.ColumnsCount = stLst.Count;
            }
            DevAge.Drawing.RectangleBorder b = new DevAge.Drawing.RectangleBorder();
            b.SetWidth(0);
            SourceGrid.Cells.Views.Cell captionModel = new SourceGrid.Cells.Views.Cell
            {
                TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter,
                ForeColor = color,
                Font = new Font("微软雅黑", 15F, FontStyle.Regular,
                    GraphicsUnit.Point, 134),
                Border = b
            };
            SourceGrid.Cells.Views.Cell captionModel2 = new SourceGrid.Cells.Views.Cell
            {
                TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter,
                ForeColor = Color.FromArgb(33, 33, 33),
                Font = new Font("微软雅黑", 15F, FontStyle.Regular,
                   GraphicsUnit.Point, 134),
                Border = b
            };

            //if (grid.Rows.Count % 2 == 0)
            //{
            //    captionModel.BackColor = Color.White;
            //}
            //else
            //{
            //    captionModel.BackColor = Color.FromArgb(227, 241, 251);
            //}
            captionModel.BackColor = Color.FromArgb(211, 211, 211);
            captionModel2.BackColor = Color.FromArgb(211, 211, 211);
            grid.Rows.Insert(_top);
            for (var i = 0; i < stLst.Count && i < grid.ColumnsCount; i++)
            {


                //获取关键信息，这个关键信息只跟整条数据有关，如果多个就分别放在该条的单元格内
                //跟该单元格并没什么关系
                Object tag = "";
                if (i < tagObj.Length)
                {
                    tag = tagObj[i];
                }

                if (stLst[i] == "False" || stLst[i] == "True")
                {
                    grid[_top, i] = CreateImgCell(stLst[i], captionModel);

                }
                else
                {
                    grid[_top, i] = new SourceGrid.Cells.Cell(stLst[i]) { View = captionModel };
                }
                grid[_top, i].Tag = tag;
                if (intColors.Contains(i))
                {
                    grid[_top, i].View = captionModel;
                }
                else
                {
                    grid[_top, i].View = captionModel2;
                }
            }
            grid.Rows[_top].Height = GridHeight;

            //不能超过23
            if (grid.RowsCount > RowMax)
            {
                grid.Rows.Remove(grid.RowsCount - 1);
            }

            //grid.AutoSizeCells();
        }

        /// <summary>
        /// 添加一条数据至第一位,多选
        /// </summary>
        /// <param name="stLst"></param>
        /// <param name="tagObj">tag记录的对象</param>
        public void AddItem2(List<string> stLst, params Object[] tagObj)
        {
            if (stLst == null) return;

            //设置列
            if (grid.ColumnsCount < stLst.Count)
            {
                grid.ColumnsCount = stLst.Count;
            }

            SourceGrid.Cells.Views.Cell captionModel = new SourceGrid.Cells.Views.Cell
            {
                TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter,
                ForeColor = Color.FromArgb(33, 33, 33),
                Font = new Font("微软雅黑", 15F, FontStyle.Regular,
                    GraphicsUnit.Point, 134)
            };

            if (grid.Rows.Count % 2 == 0)
            {

                captionModel.BackColor = Color.White;
            }
            else
            {
                captionModel.BackColor = Color.FromArgb(227, 241, 251);
            }
            //提示控件
            //SourceGrid.Cells.Controllers.ToolTipText toolTipController = new SourceGrid.Cells.Controllers.ToolTipText();
            //toolTipController.ToolTipTitle = "完整数据";
            //toolTipController.ToolTipIcon = ToolTipIcon.Info;
            //toolTipController.IsBalloon = true;
            grid.Rows.Insert(_top);
            for (var i = 0; i < stLst.Count && i < grid.ColumnsCount; i++)
            {
                //获取关键信息，这个关键信息只跟整条数据有关，如果多个就分别放在该条的单元格内
                //跟该单元格并没什么关系
                Object tag = "";
                if (i < tagObj.Length)
                {
                    tag = tagObj[i];
                }

                if (stLst[i] == "False" || stLst[i] == "True" || stLst[i] == "No")
                {
                    //每行只有一个
                    if (i == 0)
                    {
                        AddIsChoice(stLst[i] == "True");
                    }
                    grid[_top, i] = CreateImgCell(stLst[i], captionModel);
                }
                else
                {
                    grid[_top, i] = new SourceGrid.Cells.Cell(stLst[i]) { View = captionModel };

                    //grid[_top, i].ToolTipText = stLst[i];
                    //grid[_top, i].AddController(toolTipController);
                }
                grid[_top, i].Tag = tag;
            }
            grid.Rows[_top].Height = GridHeight;

            //不能超过23
            if (grid.RowsCount > RowMax)
            {
                grid.Rows.Remove(grid.RowsCount - 1);
            }

            //grid.AutoSizeCells();
        }

        /// <summary>
        /// 添加一条数据至第一位，显示文字版
        /// </summary>
        /// <param name="stLst"></param>
        /// <param name="tagObj">tag记录的对象</param>
        public void AddItem3(List<string> stLst, int[] colInts, params Object[] tagObj)
        {
            if (stLst == null) return;

            //设置列
            if (grid.ColumnsCount < stLst.Count)
            {
                grid.ColumnsCount = stLst.Count;
            }

            SourceGrid.Cells.Views.Cell captionModel = new SourceGrid.Cells.Views.Cell
            {
                TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft,
                ForeColor = Color.FromArgb(33, 33, 33),
                Font = new Font("微软雅黑", 15F, FontStyle.Regular,
                    GraphicsUnit.Point, 134)
            };

            if (grid.Rows.Count % 2 == 0)
            {

                captionModel.BackColor = Color.White;
            }
            else
            {
                captionModel.BackColor = Color.FromArgb(227, 241, 251);
            }
            //提示控件
            //SourceGrid.Cells.Controllers.ToolTipText toolTipController = new SourceGrid.Cells.Controllers.ToolTipText();
            //toolTipController.ToolTipTitle = "完整数据";
            //toolTipController.ToolTipIcon = ToolTipIcon.Info;
            //toolTipController.IsBalloon = true;
            grid.Rows.Insert(_top);
            for (var i = 0; i < stLst.Count && i < grid.ColumnsCount; i++)
            {
                //获取关键信息，这个关键信息只跟整条数据有关，如果多个就分别放在该条的单元格内
                //跟该单元格并没什么关系
                Object tag = "";
                if (i < tagObj.Length)
                {
                    tag = tagObj[i];
                }

                if (stLst[i] == "False" || stLst[i] == "True")
                {
                    //每行只有一个
                    if (i == 0)
                    {
                        AddIsChoice(stLst[i] == "True");
                    }
                    grid[_top, i] = CreateImgCell(stLst[i], captionModel);
                }
                else
                {
                    grid[_top, i] = new SourceGrid.Cells.Cell(stLst[i]) { View = captionModel };
                    if (colInts.Contains(i))
                    {
                        //为按钮增加事件
                        grid[_top, i].AddController(buttonTxtClickEvent);
                    }
                    //grid[_top, i].ToolTipText = stLst[i];
                    //grid[_top, i].AddController(toolTipController);
                }
                grid[_top, i].Tag = tag;
            }
            grid.Rows[_top].Height = GridHeight;

            //不能超过23
            if (grid.RowsCount > RowMax)
            {
                grid.Rows.Remove(grid.RowsCount - 1);
            }

            //grid.AutoSizeCells();
        }

        /// <summary>
        /// 添加一条数据至第一位,指定位置是删除图标
        /// </summary>
        /// <param name="stLst"></param>
        /// <param name="col">删除位置</param>
        /// <param name="tagObj">tag记录的对象</param>
        public void AddItemDelete(List<string> stLst, int col, Object tagObj)
        {
            if (stLst == null) return;

            //设置列
            if (grid.ColumnsCount < stLst.Count)
            {
                grid.ColumnsCount = stLst.Count;
            }
            DevAge.Drawing.RectangleBorder b = new DevAge.Drawing.RectangleBorder();
            b.SetWidth(0);
            SourceGrid.Cells.Views.Cell captionModel = new SourceGrid.Cells.Views.Cell
            {
                TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft,
                ForeColor = Color.FromArgb(255, 255, 255),
                Font = new Font("微软雅黑", 15F, FontStyle.Regular,
                    GraphicsUnit.Point, 134),
                Border = b
            };

            captionModel.BackColor = Color.FromArgb(2, 168, 243);
            //提示控件
            //SourceGrid.Cells.Controllers.ToolTipText toolTipController = new SourceGrid.Cells.Controllers.ToolTipText();
            //toolTipController.ToolTipTitle = "完整数据";
            //toolTipController.ToolTipIcon = ToolTipIcon.Info;
            //toolTipController.IsBalloon = true;
            grid.Rows.Insert(_top);
            for (var i = 0; i < stLst.Count && i < grid.ColumnsCount; i++)
            {
                //获取关键信息，这个关键信息只跟整条数据有关，如果多个就分别放在该条的单元格内
                //跟该单元格并没什么关系
                Object tag = "";
                if (i == col)
                {
                    tag = tagObj;
                }

                if (stLst[i] == "delete")
                {
                    //每行只有一个
                    grid[_top, i] = CreateDeleteImgCell(captionModel);
                }
                else
                {
                    grid[_top, i] = new SourceGrid.Cells.Cell(stLst[i]) { View = captionModel };
                }
                grid[_top, i].Tag = tag;
            }
            grid.Rows[_top].Height = GridHeight;

            //不能超过23
            if (grid.RowsCount > RowMax)
            {
                grid.Rows.Remove(grid.RowsCount - 1);
            }

        }
        #region 点击事件
        private SourceGrid.Cells.Cell CreateImgCell(string isCheck, SourceGrid.Cells.Views.Cell captionModel)
        {
            //单击控制器
            SourceGrid.Cells.Controllers.Button buttonClickEvent = new SourceGrid.Cells.Controllers.Button();
            buttonClickEvent.Executed += new EventHandler(CellButton_Click);

            SourceGrid.Cells.Cell cell = new SourceGrid.Cells.Cell("");
            switch (isCheck)
            {
                case "True":
                case "False":
                    //cell.Image = isCheck == "True" ? Resources.check_box : Resources.check_def;
                    //为按钮增加事件
                    cell.AddController(buttonClickEvent);
                    break;
                case "No":
                    //cell.Image = Resources.check_grey;
                    break;
            }
            captionModel.ImageAlignment = ContentAlignment.MiddleCenter;
            cell.View = captionModel;
            return cell;
        }

        private SourceGrid.Cells.Cell CreateDeleteImgCell(SourceGrid.Cells.Views.Cell captionModel)
        {
            //单击控制器
            SourceGrid.Cells.Controllers.Button buttonClickEvent = new SourceGrid.Cells.Controllers.Button();
            buttonClickEvent.Executed += new EventHandler(CellButtonDelete_Click);

            SourceGrid.Cells.Cell cell = new SourceGrid.Cells.Cell("");
            //cell.Image = Resources.deleted;
            captionModel.ImageAlignment = ContentAlignment.MiddleCenter;
            cell.View = captionModel;
            //为按钮增加事件
            cell.AddController(buttonClickEvent);

            return cell;
        }
        #endregion


        #endregion

        #region 按钮单击事件

        /// <summary>
        /// 按钮选中单击删除事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CellButtonDelete_Click(object sender, EventArgs e)
        {
            SourceGrid.CellContext context = (SourceGrid.CellContext)sender;
            SourceGrid.Cells.Cell btnCell = (SourceGrid.Cells.Cell)context.Cell;

            //MessageBox.Show(btnCell.Column.Index.ToString() + ":" + btnCell.Row.Index.ToString());
            int currenRow = btnCell.Row.Index;
            grid.Columns.StretchToFit();
            LDeleteAction?.Invoke(btnCell.Tag);

        }

        /// <summary>
        /// 按钮选中单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CellButton_Click(object sender, EventArgs e)
        {
            SourceGrid.CellContext context = (SourceGrid.CellContext)sender;
            SourceGrid.Cells.Cell btnCell = (SourceGrid.Cells.Cell)context.Cell;
            #region 切换图片，单选形式

            if (IsChoiceLst[btnCell.Row.Index])
            {
                // btnCell.Image = Resources.check_def;
            }
            else
            {
                // btnCell.Image = Resources.check_box;
            }
            IsChoiceLst[btnCell.Row.Index] = !IsChoiceLst[btnCell.Row.Index];
            #endregion

            //MessageBox.Show(btnCell.Column.Index.ToString() + ":" + btnCell.Row.Index.ToString());
            int currenRow = btnCell.Row.Index;
            grid.Columns.StretchToFit();
            LCheckBoxAction?.Invoke(btnCell.Tag, IsChoiceLst[btnCell.Row.Index]);

        }
        /// <summary>
        /// 按钮单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CellButtonTxt_Click(object sender, EventArgs e)
        {
            SourceGrid.CellContext context = (SourceGrid.CellContext)sender;
            SourceGrid.Cells.Cell btnCell = (SourceGrid.Cells.Cell)context.Cell;

            //MessageBox.Show(btnCell.Column.Index.ToString() + ":" + btnCell.Row.Index.ToString());
            int currenCol = btnCell.Column.Index;
            grid.Columns.StretchToFit();
            LTxtBoxAction?.Invoke(btnCell.DisplayText, currenCol);

        }
        #endregion

        #region 改变指定列数据的位置的数据
        /*
        /// <summary>
        /// 改变指定列数据的位置的数据,该项目只有取详情里用到
        /// 
        /// </summary>
        /// <param name="col"></param>
        /// <param name="data"></param>
        /// <param name="newSt"></param>
        public void SetCell(int col, string data, int changeCol, string newSt, Color fColor, Color bColor)
        {
            //ForeColor = Color.FromArgb(124, 178, 7),
            //    Font = new Font("微软雅黑", 15F, FontStyle.Regular,
            //        GraphicsUnit.Point, 134),
            //    BackColor = Color.FromArgb(211, 211, 211)
            SourceGrid.Cells.Views.Cell captionModel3 = new SourceGrid.Cells.Views.Cell
            {
                TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter,
                ForeColor = fColor,
                Font = new Font("微软雅黑", 15F, FontStyle.Regular,
                    GraphicsUnit.Point, 134),
                BackColor = bColor
            };

            for (int j = 0; j < DataGrid.RowsCount; j++)
            {
                var temp = DataGrid[j, col].Tag as Check;
                if (temp.epcId == data)
                {
                    DataGrid[j, changeCol] = new SourceGrid.Cells.Cell(newSt) { View = captionModel3 };
                }
            }
            grid.Columns.StretchToFit();
        }
        /// <summary>
        /// 还原选中列为可选
        /// </summary>
        /// <param name="col">指定列</param>
        /// <param name="data">找到标识行</param>
        /// <param name="changeCol">改变的列</param>
        /// <param name="tagObj">该列带的对象</param>
        public void ReductionCell(int col, string data, int changeCol, object tagObj)
        {
            SourceGrid.Cells.Views.Cell captionModel3 = new SourceGrid.Cells.Views.Cell
            {
                TextAlignment = DevAge.Drawing.ContentAlignment.MiddleLeft,
                ForeColor = Color.FromArgb(124, 178, 7),
                Font = new Font("微软雅黑", 15F, FontStyle.Regular,
                    GraphicsUnit.Point, 134),
                BackColor = Color.FromArgb(211, 211, 211)
            };

            for (int j = 0; j < DataGrid.RowsCount; j++)
            {
                var temp = DataGrid[j, col].Tag as FindInfoPageRp;
                if (temp.id == data)
                {
                    if (j % 2 == 0)
                    {

                        captionModel3.BackColor = Color.White;
                    }
                    else
                    {
                        captionModel3.BackColor = Color.FromArgb(227, 241, 251);
                    }
                    DataGrid[j, changeCol] = CreateImgCell("False", captionModel3);
                    DataGrid[j, changeCol].Tag = tagObj;
                }
            }
            grid.Columns.StretchToFit();
        }
        */
        #endregion
        #region 清掉除了标头
        /// <summary>
        /// 清掉除了标头
        /// </summary>
        public void Clear()
        {
            int count = grid.RowsCount - _top;
            for (int i = 0; i < count; i++)
            {
                grid.Rows.Remove(_top);
            }
        }

        #endregion

        private void GridUc_Load(object sender, EventArgs e)
        {
            grid.Redim(3, 2);

            SourceGrid.Cells.Views.Cell titleModel =
                new SourceGrid.Cells.Views.Cell
                {
                    BackColor = Color.FromArgb(14, 144, 210),
                    ForeColor = Color.White,
                    TextAlignment = DevAge.Drawing.ContentAlignment.MiddleCenter,
                    Font = new Font("微软雅黑", 15F, FontStyle.Regular,
                        GraphicsUnit.Point, 134)
                };


            SourceGrid.Cells.Views.Cell captionModel = new SourceGrid.Cells.Views.Cell { BackColor = grid.BackColor };

            int currentRow = 0;

            //#region Base Types

            //grid.Rows.Insert(0);
            //grid[currentRow, 0] = new SourceGrid.Cells.Cell("编码") {View = titleModel};
            //grid[currentRow, 1] = new SourceGrid.Cells.Cell("打印") {View = titleModel};
            //grid[currentRow, 2] = new SourceGrid.Cells.Cell("校验") { View = titleModel };
            //grid[currentRow, 3] = new SourceGrid.Cells.Cell("发行时间") { View = titleModel };

            //#endregion

            GridHeight = 48;
            //grid.Rows[0].Height = GridHeight;
            //grid.Columns[0].Width = 225;
            //grid.Columns[1].Width = 75;
            //grid.Columns[2].Width = 75;
            //grid.Columns[3].Width = 125;
            // 选择行
            grid.SelectionMode = SourceGrid.GridSelectionMode.Cell;

            //选择框的大小
            var selection = grid.Selection as SelectionBase;
            if (selection != null)
            {
                DevAge.Drawing.RectangleBorder b = selection.Border;
                b.SetWidth(0);
                selection.Border = b;
            }
            grid.Columns.StretchToFit();
            grid.AutoSizeCells();
            //List<string> lst = new List<string>();
            //lst.Add("False");
            //lst.Add("身份证身份证");
            //AddItem(lst, "1");

            //List<string> lst = new List<string>();
            //lst.Add("32132132132");
            //lst.Add("false");
            //lst.Add("true");
            //lst.Add(DateTime.Now.ToString());
            //AddItem(lst);

            //int[] wInts = new[] { 80, 80, 120, 220, 90 };
            //Grid_Load(wInts, 45, 6);
            //List<string> stList = new List<string>();
            //stList.Add("False");
            //stList.Add("白正伟");
            //stList.Add("*****5526");
            //stList.Add("2018-01-08 18:07");
            //stList.Add("在库");
            //for (int i = 0; i < 20; i++)
            //{
            //    AddItem(stList);
            //}

            //int[] wInts = new[] { 70, 100, 110, 80 };
            //Grid_Load(wInts, 45, 7);
            //List<string> stList = new List<string>();
            //stList.Add("槽1");
            //stList.Add("白正伟");
            //stList.Add("*****5526");
            //stList.Add("待取");
            //for (int i = 0; i < 20; i++)
            //{
            //    AddItemColor(stList,new int[] { 3}, Color.FromArgb(2, 168, 243));
            //}
            //查看
            int[] wInts = new[] { 200, 321, 300, 120 };
            Grid_Load(wInts, 35, 7);
            List<string> stList = new List<string>();
            stList.Add("白正伟");
            stList.Add("*****5526");
            stList.Add("2018-01-08 18:07");
            stList.Add("12");
            for (int i = 0; i < 20; i++)
            {
                AddItem(stList);
            }
        }
    }

    public class Heaher
    {
        /// <summary>
        /// 文字
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 高度
        /// </summary>
        public int Width { get; set; }
    }
}