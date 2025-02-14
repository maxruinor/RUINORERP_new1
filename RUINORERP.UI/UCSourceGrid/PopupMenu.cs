using Netron.GraphLib;
using RUINORERP.Common.Extensions;
using RUINORERP.Model;
using RUINORERP.UI.Common;
using RUINORERP.UI.ToolForm;
using RUINORERP.UI.UControls;
using SourceGrid;
using SourceGrid.Cells.Editors;
using SourceGrid.Cells.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Winista.Text.HtmlParser.Data;
using static RUINORERP.UI.Log.UClog;
using Color = System.Drawing.Color;
using RUINOR.WinFormsUI.CustomPictureBox;
using static FastReport.Design.ToolWindows.DictionaryWindow;
using SqlSugar;
using MathNet.Numerics.LinearAlgebra.Factorization;

namespace RUINORERP.UI.UCSourceGrid
{

    /// <summary>
    /// 公共右键菜单
    /// </summary>
    public class PopupMenu : SourceGrid.Cells.Controllers.ControllerBase
    {

        public delegate void ColumnsVisibleDelegate(KeyValuePair<string, SGDefineColumnItem> kv);
        //string bool  cost true-->表示 成本 显示
        //public delegate void ColumnsVisibleDelegate(int colIndex, string colName, bool visible);
        /// <summary>
        /// 验证行数据
        /// </summary>
        public event ColumnsVisibleDelegate OnColumnsVisible;


        ContextMenuStrip MyMenu = new ContextMenuStrip();

        /// <summary>
        /// 保存了要控制的列
        /// </summary>
        private List<KeyValuePair<string, SGDefineColumnItem>> items = new List<KeyValuePair<string, SGDefineColumnItem>>();
        //private SerializableDictionary<string, bool> items = new SerializableDictionary<string, bool>();


        public PopupMenu()
        {
            MyMenu.ShowCheckMargin = true;
            ToolStripSeparator ss = new ToolStripSeparator();
            MyMenu.Items.Add(ss);
            ToolStripMenuItem siCustom = new ToolStripMenuItem("自定义");
            siCustom.Click += SiCustom_Click;
            MyMenu.Items.Add(siCustom);
            //menu.MenuItems.Add("Menu 1", new EventHandler(Menu1_Click));
            //menu.MenuItems.Add("Menu 2", new EventHandler(Menu2_Click));
        }

        private void SiCustom_Click(object sender, EventArgs e)
        {
            frmShowColumns frm = new frmShowColumns();

            frm.Items = items;
            if (frm.ShowDialog() == DialogResult.OK)
            {
                //更新配置变化，体现到这里。思路是?
                foreach (var item in items)
                {
                    OnColumnsVisible(item);
                }
            }

        }

        /// <summary>
        /// 添加要控制的列
        /// </summary>
        /// <param name="item"></param>
        public void AddItems(KeyValuePair<string, SGDefineColumnItem> item)
        {
            if (!item.Value.NeverVisible)
            {
                items.Add(item);
                ToolStripMenuItem si = new ToolStripMenuItem(item.Key);
                si.Checked = item.Value.Visible;
                si.CheckOnClick = true;
                si.Click += Si_Click;
                // MyMenu.Items.Add(si);
                //初始时有几个，就减几
                MyMenu.Items.Insert(MyMenu.Items.Count - 2, si);
            }
        }




        private void Si_Click(object sender, EventArgs e)
        {
            KeyValuePair<string, SGDefineColumnItem> item = items.Find(kv => kv.Key == sender.ToString());
            item.Value.Visible = (sender as ToolStripMenuItem).Checked;
            //OnColumnsVisible(item.Value.ColIndex, item.Value.ColName, item.Value.Visible);
            OnColumnsVisible(item);
        }



        public override void OnClick(CellContext sender, EventArgs e)
        {
            base.OnClick(sender, e);
        }


        public override void OnMouseUp(SourceGrid.CellContext sender, MouseEventArgs e)
        {
            base.OnMouseUp(sender, e);

            if (e.Button == MouseButtons.Right)
                MyMenu.Show(sender.Grid, new Point(e.X, e.Y));
        }

        private void Menu1_Click(object sender, EventArgs e)
        {
            //TODO Your code here
        }
        private void Menu2_Click(object sender, EventArgs e)
        {
            //TODO Your code here
        }
    }




    public class PopupMenuForRowHeader : SourceGrid.Cells.Controllers.ControllerBase
    {
        private Grid grid;

        #region 应用于转换单

        public delegate tb_ProdConversion GetTransferDataHandler(ToolStripItem sender, object rowObj, SourceGridDefine CurrGridDefine);

        /// <summary>
        /// 针对特殊情况下的右键菜单，如替换出库。转换出库等
        /// 
        /// </summary>
        public event GetTransferDataHandler OnGetTransferDataHandler;

        public delegate void TransferMenuHandler(ToolStripItem sender, object rowObj, SourceGridDefine CurrGridDefine);

        /// <summary>
        /// 针对特殊情况下的右键菜单，如替换出库。转换出库等
        /// 
        /// </summary>
        public event TransferMenuHandler OnTransferMenuHandler;

        #endregion


        MenuPowerHelper menuPowerHelper;

        public delegate void ColumnsVisibleDelegate(KeyValuePair<string, SGDefineColumnItem> kv);
        /// <summary>
        /// 验证行数据
        /// </summary>
        public event ColumnsVisibleDelegate OnColumnsVisible;


        ContextMenuStrip MyMenu = new ContextMenuStrip();

        private List<KeyValuePair<string, SGDefineColumnItem>> items = new List<KeyValuePair<string, SGDefineColumnItem>>();

        SourceGridDefine sgdefine;
        public PopupMenuForRowHeader(int addRowIndex, Grid _grid, SourceGridDefine _sgdefine)
        {
            MyMenu.ShowCheckMargin = false;
            MyMenu.ShowImageMargin = false;
            MyMenu.MinimumSize = new Size(100, 0); // 设置最小宽度为 120，高度不限
            //ToolStripSeparator ss = new ToolStripSeparator();
            //MyMenu.Items.Add(ss);
            ToolStripMenuItem siCustom = new ToolStripMenuItem("删除行【" + addRowIndex + "】");
            siCustom.Tag = addRowIndex;
            siCustom.Click += SiCustom_Click;
            MyMenu.Items.Add(siCustom);


            ToolStripMenuItem tsMiTransfer = new ToolStripMenuItem("转换出库【" + addRowIndex + "】");
            tsMiTransfer.Tag = addRowIndex;
            tsMiTransfer.Size = new System.Drawing.Size(153, 26);
            tsMiTransfer.Click += tsMiTransfer_Click;
            MyMenu.Items.Add(tsMiTransfer);

            grid = _grid;
            sgdefine = _sgdefine;
            menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();
        }
        private void tsMiTransfer_Click(object sender, EventArgs e)
        {
            //先得选择的行。再取当前选中的行数据，再传入到转换单UI
            System.Windows.Forms.ToolStripMenuItem item = (System.Windows.Forms.ToolStripMenuItem)sender;
            int RowIndex = int.Parse(item.Tag.ToString());
            var rowObj = grid.Rows[RowIndex].RowData;
            tb_MenuInfo RelatedMenuInfo = null;
            RelatedMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.EntityName == nameof(tb_ProdConversion)
            && m.BIBaseForm == "BaseBillEditGeneric`2").FirstOrDefault();
            tb_ProdConversion prodConversion = new tb_ProdConversion();
            prodConversion.tb_ProdConversionDetails = new List<tb_ProdConversionDetail>();
            if (OnGetTransferDataHandler != null)
            {
                var result = OnGetTransferDataHandler(item, rowObj, sgdefine);
                if (result != null)
                {
                    prodConversion = result;
                }
            }
            else
            {
                MainForm.Instance.PrintInfoLog("没有找到具体的转换单数据，只提供默认数据。");
            }

            menuPowerHelper.ExecuteEvents(RelatedMenuInfo, prodConversion);
        }

        private void SiCustom_Click(object sender, EventArgs e)
        {

            //思路先把数据移掉一行，再重复设置一下右键删除行的菜单，并且重新设置行号,并且添加一行

            System.Windows.Forms.ToolStripMenuItem item = (System.Windows.Forms.ToolStripMenuItem)sender;
            int addRowIndex = int.Parse(item.Tag.ToString());
            SourceGridHelper sh = new SourceGridHelper();
            sh.DeleteRow(sgdefine, addRowIndex);
        }

        public void AddItems(KeyValuePair<string, SGDefineColumnItem> item)
        {
            items.Add(item);
            ToolStripMenuItem si = new ToolStripMenuItem(item.Key);
            si.Checked = item.Value.Visible;
            si.CheckOnClick = true;
            si.Click += Si_Click;
            // MyMenu.Items.Add(si);
            //初始时有几个，就减几
            MyMenu.Items.Insert(MyMenu.Items.Count - 2, si);
        }

        private void Si_Click(object sender, EventArgs e)
        {
            KeyValuePair<string, SGDefineColumnItem> item = items.Find(kv => kv.Key == sender.ToString());
            item.Value.Visible = (sender as ToolStripMenuItem).Checked;
            OnColumnsVisible(item);
        }

        public override void OnClick(CellContext sender, EventArgs e)
        {
            base.OnClick(sender, e);
        }

        public List<KeyValuePair<string, SGDefineColumnItem>> Items { get => items; set => items = value; }

        public override void OnMouseUp(SourceGrid.CellContext sender, MouseEventArgs e)
        {
            base.OnMouseUp(sender, e);
            //这里判断右键显示的情况
            if (e.Button == MouseButtons.Right)
            {
                //默认不显示。只是要的时候业务UI处理显示
                MyMenu.Items[1].Visible = false;
                if (OnTransferMenuHandler != null)
                {
                    OnTransferMenuHandler(MyMenu.Items[1], sender.Tag, sgdefine);
                }
                MyMenu.Show(sender.Grid, new Point(e.X, e.Y));
            }

        }

    }

    /// <summary>
    /// 自定义右键菜单
    /// </summary>
    public class PopupMenuCustomize : SourceGrid.Cells.Controllers.ControllerBase
    {
        private Grid grid;

        #region 应用于转换单

        public delegate tb_ProdConversion GetTransferDataHandler(ToolStripItem sender, object rowObj, SourceGridDefine CurrGridDefine);

        /// <summary>
        /// 针对特殊情况下的右键菜单，如替换出库。转换出库等
        /// 
        /// </summary>
        public event GetTransferDataHandler OnGetTransferDataHandler;

        public delegate void TransferMenuHandler(ToolStripItem sender, object rowObj, SourceGridDefine CurrGridDefine);

        /// <summary>
        /// 针对特殊情况下的右键菜单，如替换出库。转换出库等
        /// 
        /// </summary>
        public event TransferMenuHandler OnTransferMenuHandler;

        #endregion


        MenuPowerHelper menuPowerHelper;

        public delegate void ColumnsVisibleDelegate(KeyValuePair<string, SGDefineColumnItem> kv);
        /// <summary>
        /// 验证行数据
        /// </summary>
        public event ColumnsVisibleDelegate OnColumnsVisible;


        public ContextMenuStrip MyMenu = new ContextMenuStrip();

        private List<KeyValuePair<string, SGDefineColumnItem>> items = new List<KeyValuePair<string, SGDefineColumnItem>>();

        SourceGridDefine sgdefine;
        public PopupMenuCustomize(int addRowIndex, Grid _grid, SourceGridDefine _sgdefine)
        {
            MyMenu.ShowCheckMargin = false;
            MyMenu.ShowImageMargin = false;
            MyMenu.MinimumSize = new Size(100, 0); // 设置最小宽度为 120，高度不限
            //ToolStripSeparator ss = new ToolStripSeparator();
            //MyMenu.Items.Add(ss);
            ToolStripMenuItem siCustom = new ToolStripMenuItem("删除行【" + addRowIndex + "】");
            siCustom.Tag = addRowIndex;
            siCustom.Click += SiCustom_Click;
            MyMenu.Items.Add(siCustom);


            ToolStripMenuItem tsMiTransfer = new ToolStripMenuItem("转换出库【" + addRowIndex + "】");
            tsMiTransfer.Tag = addRowIndex;
            tsMiTransfer.Size = new System.Drawing.Size(153, 26);
            tsMiTransfer.Click += tsMiTransfer_Click;
            MyMenu.Items.Add(tsMiTransfer);

            grid = _grid;
            sgdefine = _sgdefine;
            menuPowerHelper = Startup.GetFromFac<MenuPowerHelper>();
        }
        private void tsMiTransfer_Click(object sender, EventArgs e)
        {
            //先得选择的行。再取当前选中的行数据，再传入到转换单UI
            System.Windows.Forms.ToolStripMenuItem item = (System.Windows.Forms.ToolStripMenuItem)sender;
            int RowIndex = int.Parse(item.Tag.ToString());
            var rowObj = grid.Rows[RowIndex].RowData;
            tb_MenuInfo RelatedMenuInfo = null;
            RelatedMenuInfo = MainForm.Instance.MenuList.Where(m => m.IsVisble && m.EntityName == nameof(tb_ProdConversion)
            && m.BIBaseForm == "BaseBillEditGeneric`2").FirstOrDefault();
            tb_ProdConversion prodConversion = new tb_ProdConversion();
            prodConversion.tb_ProdConversionDetails = new List<tb_ProdConversionDetail>();
            if (OnGetTransferDataHandler != null)
            {
                var result = OnGetTransferDataHandler(item, rowObj, sgdefine);
                if (result != null)
                {
                    prodConversion = result;
                }
            }
            else
            {
                MainForm.Instance.PrintInfoLog("没有找到具体的转换单数据，只提供默认数据。");
            }

            menuPowerHelper.ExecuteEvents(RelatedMenuInfo, prodConversion);
        }

        private void SiCustom_Click(object sender, EventArgs e)
        {

            //思路先把数据移掉一行，再重复设置一下右键删除行的菜单，并且重新设置行号,并且添加一行

            System.Windows.Forms.ToolStripMenuItem item = (System.Windows.Forms.ToolStripMenuItem)sender;
            int addRowIndex = int.Parse(item.Tag.ToString());
            SourceGridHelper sh = new SourceGridHelper();
            sh.DeleteRow(sgdefine, addRowIndex);
            return;



        }

        public void AddItems(KeyValuePair<string, SGDefineColumnItem> item)
        {
            items.Add(item);
            ToolStripMenuItem si = new ToolStripMenuItem(item.Key);
            si.Checked = item.Value.Visible;
            si.CheckOnClick = true;
            si.Click += Si_Click;
            // MyMenu.Items.Add(si);
            //初始时有几个，就减几
            MyMenu.Items.Insert(MyMenu.Items.Count - 2, si);
        }

        private void Si_Click(object sender, EventArgs e)
        {
            KeyValuePair<string, SGDefineColumnItem> item = items.Find(kv => kv.Key == sender.ToString());
            item.Value.Visible = (sender as ToolStripMenuItem).Checked;
            OnColumnsVisible(item);
        }

        public override void OnClick(CellContext sender, EventArgs e)
        {
            base.OnClick(sender, e);
        }

        public List<KeyValuePair<string, SGDefineColumnItem>> Items { get => items; set => items = value; }

        public override void OnMouseUp(SourceGrid.CellContext sender, MouseEventArgs e)
        {
            base.OnMouseUp(sender, e);
            //这里判断右键显示的情况
            if (e.Button == MouseButtons.Right)
            {
                //默认不显示。只是要的时候业务UI处理显示
                MyMenu.Items[1].Visible = false;
                if (OnTransferMenuHandler != null)
                {
                    OnTransferMenuHandler(MyMenu.Items[1], sender.Tag, sgdefine);
                }
                MyMenu.Show(sender.Grid, new Point(e.X, e.Y));
            }

        }

    }

    /// <summary>
    /// 用于显示全选 全不选 反选
    /// </summary>
    public class PopupMenuForSelect : SourceGrid.Cells.Controllers.ControllerBase
    {
        private Grid grid;
        ContextMenuStrip MyMenu = new ContextMenuStrip();

        private List<KeyValuePair<string, SGDefineColumnItem>> items = new List<KeyValuePair<string, SGDefineColumnItem>>();

        SourceGridDefine sgdefine;
        public PopupMenuForSelect(Grid _grid, SourceGridDefine _sgdefine)
        {
            MyMenu.ShowCheckMargin = true;

            ToolStripMenuItem tsSelectAll = new ToolStripMenuItem("全选");
            tsSelectAll.Tag = _sgdefine;
            tsSelectAll.Click += SiCustom_Click;

            ToolStripMenuItem tsSelectNotAll = new ToolStripMenuItem("全不选");
            tsSelectNotAll.Tag = _sgdefine;
            tsSelectNotAll.Click += SiCustom_Click;

            ToolStripMenuItem tsReverseSelected = new ToolStripMenuItem("反选");
            tsReverseSelected.Tag = _sgdefine;
            tsReverseSelected.Click += SiCustom_Click;



            MyMenu.Items.Add(tsSelectAll);
            MyMenu.Items.Add(tsSelectNotAll);
            MyMenu.Items.Add(tsReverseSelected);

            grid = _grid;
            sgdefine = _sgdefine;
        }

        private void SiCustom_Click(object sender, EventArgs e)
        {

            System.Windows.Forms.ToolStripMenuItem item = (System.Windows.Forms.ToolStripMenuItem)sender;
            SourceGridDefine _sgdefine = item.Tag as SourceGridDefine;
            SGDefineColumnItem selected = _sgdefine.DefineColumns.Find(c => c.ColName == "Selected");
            int selectRealIndex = _sgdefine.grid.Columns.GetColumnInfo(selected.UniqueId).Index;
            if (selected != null)
            {
                foreach (GridRow row in grid.Rows)
                {
                    if (row.RowData == null)
                    {
                        continue;
                    }
                    GridColumn mColumn = grid.Columns[selectRealIndex] as GridColumn;
                    if (mColumn != null)
                    {
                        string txt = sender.ToString();
                        switch (txt)
                        {
                            case "全选":
                                grid[row.Index, selectRealIndex].Value = true;
                                break;
                            case "全不选":
                                grid[row.Index, selectRealIndex].Value = false;
                                break;
                            case "反选": 
                                if (grid[row.Index, selectRealIndex].Value is Boolean bl)
                                {
                                    grid[row.Index, selectRealIndex].Value = !bl;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }

            }
            ////
            ////sgdefine.UseCalculateTotalValue(grid.Tag as SourceGridDefine);



            ////删除一行后。后面的行的索引都会变化。要重新处理一次，并且行号重新显示
            //foreach (GridRow row in grid.Rows)
            //{
            //    sourceGridHelper.SetCellValue()








            //}

        }

        public void AddItems(KeyValuePair<string, SGDefineColumnItem> item)
        {
            items.Add(item);
            ToolStripMenuItem si = new ToolStripMenuItem(item.Key);
            si.Checked = item.Value.Visible;
            si.CheckOnClick = true;
            si.Click += Si_Click;
            // MyMenu.Items.Add(si);
            //初始时有几个，就减几
            MyMenu.Items.Insert(MyMenu.Items.Count - 2, si);
        }

        private void Si_Click(object sender, EventArgs e)
        {
            KeyValuePair<string, SGDefineColumnItem> item = items.Find(kv => kv.Key == sender.ToString());
            item.Value.Visible = (sender as ToolStripMenuItem).Checked;
        }

        public override void OnClick(CellContext sender, EventArgs e)
        {
            base.OnClick(sender, e);
        }

        public List<KeyValuePair<string, SGDefineColumnItem>> Items { get => items; set => items = value; }

        public override void OnMouseUp(SourceGrid.CellContext sender, MouseEventArgs e)
        {
            base.OnMouseUp(sender, e);

            if (e.Button == MouseButtons.Right)
                MyMenu.Show(sender.Grid, new Point(e.X, e.Y));
        }

    }


    /// <summary>
    /// 用于项这个单元格的删除操作，也可以集成其他功能？
    /// </summary>
    public class PopupMenuForDeleteSelect : SourceGrid.Cells.Controllers.ControllerBase
    {
        private Grid grid;
        ContextMenuStrip MyMenu = new ContextMenuStrip();

        private List<KeyValuePair<string, SGDefineColumnItem>> items = new List<KeyValuePair<string, SGDefineColumnItem>>();

        SourceGridDefine sgdefine;
        public PopupMenuForDeleteSelect(Grid _grid, SourceGridDefine _sgdefine)
        {
            MyMenu.ShowCheckMargin = false;
            MyMenu.ShowImageMargin = false;
            MyMenu.MinimumSize = new Size(100, 0); // 设置最小宽度为 120，高度不限

            ToolStripMenuItem tsDeleteSelect = new ToolStripMenuItem("删除选中行");
            tsDeleteSelect.Name = "删除选中行";
            tsDeleteSelect.Visible = false;
            tsDeleteSelect.Tag = _sgdefine;
            tsDeleteSelect.Click += SiCustom_Click;

            ToolStripMenuItem tsShowSelected = new ToolStripMenuItem("显示多选");
            tsShowSelected.Name = "显示多选";
            tsShowSelected.CheckOnClick = true;
            tsShowSelected.Tag = _sgdefine;
            tsShowSelected.Click += SiCustom_Click;

            //ToolStripMenuItem tsReverseSelected = new ToolStripMenuItem("反选");
            //tsReverseSelected.Tag = _sgdefine;
            //tsReverseSelected.Click += SiCustom_Click;
            MyMenu.Items.Add(tsShowSelected);
            MyMenu.Items.Add(tsDeleteSelect);
            // MyMenu.Items.Add(tsReverseSelected);

            grid = _grid;
            sgdefine = _sgdefine;
        }

        private void SiCustom_Click(object sender, EventArgs e)
        {
            SourceGridHelper sh = new SourceGridHelper();
            System.Windows.Forms.ToolStripMenuItem item = (System.Windows.Forms.ToolStripMenuItem)sender;
            SourceGridDefine _sgdefine = item.Tag as SourceGridDefine;
            SGDefineColumnItem selected = _sgdefine.DefineColumns.Find(c => c.ColName == "Selected");
            int selectRealIndex = _sgdefine.grid.Columns.GetColumnInfo(selected.UniqueId).Index;
            if (selected != null)
            {
                List<int> deleteRows = new List<int>();
                foreach (GridRow row in grid.Rows)
                {
                    if (row.RowData == null)
                    {
                        continue;
                    }
                    if (grid[row.Index, selectRealIndex].Value is Boolean bl)
                    {
                        if (bl)
                        {
                            deleteRows.Add(row.Index);
                        }
                    }
                }

                GridColumn mColumn = grid.Columns[selectRealIndex] as GridColumn;
                if (mColumn != null)
                {
                    string txt = sender.ToString();
                    switch (txt)
                    {
                        case "删除选中行":
                            sh.DeleteRow(_sgdefine, deleteRows.ToArray());
                            break;
                        case "显示多选":
                            if (selected != null)
                            {
                                grid.Columns[selectRealIndex].Visible = item.Checked;
                                MyMenu.Items.Find("删除选中行", true)[0].Visible = item.Checked;
                            }
                            else
                            {
                                //没有实现选中列的功能，请在明细表加载时候处理指定选择列
                            }
                            break;
                        case "反选":
                            break;
                        default:
                            break;
                    }
                }


            }
        }




        public void AddItems(KeyValuePair<string, SGDefineColumnItem> item)
        {
            items.Add(item);
            ToolStripMenuItem si = new ToolStripMenuItem(item.Key);
            si.Checked = item.Value.Visible;
            si.CheckOnClick = true;
            si.Click += Si_Click;
            // MyMenu.Items.Add(si);
            //初始时有几个，就减几
            MyMenu.Items.Insert(MyMenu.Items.Count - 2, si);
        }

        private void Si_Click(object sender, EventArgs e)
        {
            KeyValuePair<string, SGDefineColumnItem> item = items.Find(kv => kv.Key == sender.ToString());
            item.Value.Visible = (sender as ToolStripMenuItem).Checked;
        }

        public override void OnClick(CellContext sender, EventArgs e)
        {
            base.OnClick(sender, e);
        }

        public List<KeyValuePair<string, SGDefineColumnItem>> Items { get => items; set => items = value; }

        public override void OnMouseUp(SourceGrid.CellContext sender, MouseEventArgs e)
        {
            base.OnMouseUp(sender, e);

            if (e.Button == MouseButtons.Right)
                MyMenu.Show(sender.Grid, new Point(e.X, e.Y));
        }

    }


    /// <summary>
    /// 远程图片单元视图的右键菜单
    /// </summary>
    public class PopupMenuForRemoteImageView : SourceGrid.Cells.Controllers.ControllerBase
    {
        private Grid grid;
        public delegate void ColumnsVisibleDelegate(KeyValuePair<string, SGDefineColumnItem> kv);
        /// <summary>
        /// 验证行数据
        /// </summary>
        public event ColumnsVisibleDelegate OnColumnsVisible;
        ContextMenuStrip MyMenu = new ContextMenuStrip();
        SourceGrid.Cells.Cell _cell;

        private List<KeyValuePair<string, SGDefineColumnItem>> items = new List<KeyValuePair<string, SGDefineColumnItem>>();

        SourceGridDefine sgdefine;
        public PopupMenuForRemoteImageView(SourceGrid.Cells.Cell cell, SourceGridDefine _sgdefine)
        {
            MyMenu.ShowCheckMargin = false;
            MyMenu.ShowImageMargin = false;
            MyMenu.MinimumSize = new Size(120, 0); // 设置最小宽度为 120，高度不限
                                                   // 为 ContextMenuStrip 控件添加 Opening 事件处理程序
            MyMenu.Opening += new CancelEventHandler(MyMenu_Opening);

            //ToolStripSeparator ss = new ToolStripSeparator();
            //MyMenu.Items.Add(ss);
            ToolStripMenuItem siCustom = new ToolStripMenuItem("查看大图");
            _cell = cell;
            siCustom.Tag = cell;
            siCustom.Click += SiCustom_Click;
            MyMenu.Items.Add(siCustom);
            grid = _sgdefine.grid;
            sgdefine = _sgdefine;
            MyMenu.Width = 100;
        }

        private void SiCustom_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.ToolStripMenuItem item = (System.Windows.Forms.ToolStripMenuItem)sender;
            SourceGrid.Cells.Cell cell = (SourceGrid.Cells.Cell)item.Tag;
            if (cell.View is RemoteImageView)
            {
                RemoteImageView imageView = cell.View as RemoteImageView;
                if (imageView != null)
                {
                    if (imageView.GridImage != null)
                    {
                        var model = cell.Model.FindModel(typeof(SourceGrid.Cells.Models.ValueImageWeb));
                        SourceGrid.Cells.Models.ValueImageWeb valueImageWeb = (SourceGrid.Cells.Models.ValueImageWeb)model;
                        if (valueImageWeb.CellImageBytes != null && valueImageWeb.CellImageBytes.Length > 0)
                        {
                            frmPictureViewer frmPictureViewer = new frmPictureViewer();
                            frmPictureViewer.PictureBoxViewer.Image = ImageProcessor.ByteArrayToImage(valueImageWeb.CellImageBytes);
                            frmPictureViewer.ShowDialog();
                        }
                    }
                }

            }
            //return;
            ////强制重绘
            grid.Refresh();

        }

        public override void OnClick(CellContext sender, EventArgs e)
        {
            base.OnClick(sender, e);
        }

        public override void OnMouseUp(SourceGrid.CellContext sender, MouseEventArgs e)
        {
            base.OnMouseUp(sender, e);

            if (e.Button == MouseButtons.Right)
                MyMenu.Show(sender.Grid, new Point(e.X, e.Y));
        }




        private void MyMenu_Opening(object sender, CancelEventArgs e)
        {
            // 假设我们有一个条件判断，例如某个控件的状态
            bool shouldShowMenu = CheckConditionToShowMenu();
            // 如果不满足条件，则取消显示菜单
            if (!shouldShowMenu)
            {
                e.Cancel = true;
            }
        }

        // 这里是你的条件检查逻辑
        private bool CheckConditionToShowMenu()
        {
            //如果图片为空则不显示右键菜单
            bool rs = false;
            SourceGrid.Cells.Cell cell = _cell;
            if (cell.View is RemoteImageView)
            {
                RemoteImageView imageView = cell.View as RemoteImageView;
                if (imageView != null)
                {
                    if (imageView.GridImage != null)
                    {
                        var model = cell.Model.FindModel(typeof(SourceGrid.Cells.Models.ValueImageWeb));
                        SourceGrid.Cells.Models.ValueImageWeb valueImageWeb = (SourceGrid.Cells.Models.ValueImageWeb)model;
                        if (valueImageWeb.CellImageBytes != null && valueImageWeb.CellImageBytes.Length > 0)
                        {
                            rs = true;
                        }
                        else
                        {
                            rs = false;
                        }
                    }
                }

            }
            return rs;
        }

    }




}
