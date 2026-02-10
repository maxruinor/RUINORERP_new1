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
using System.IO;

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
            if (selected != null)
            {
                int selectRealIndex = _sgdefine.grid.Columns.GetColumnInfo(selected.UniqueId).Index;
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
        
        /// <summary>
        /// 是否单图模式（true:单图，false:多图）
        /// 单图模式下，有图片时显示"替换图片"，无图片时显示"上传图片"
        /// 多图模式下，始终显示"上传图片"和"删除图片"
        /// </summary>
        public bool IsSingleImageMode { get; set; } = true;
        public PopupMenuForRemoteImageView(SourceGrid.Cells.Cell cell, SourceGridDefine _sgdefine)
        {
            MyMenu.ShowCheckMargin = false;
            MyMenu.ShowImageMargin = false;
            MyMenu.MinimumSize = new Size(120, 0); // 设置最小宽度为 120，高度不限
                                                   // 为 ContextMenuStrip 控件添加 Opening 事件处理程序
            MyMenu.Opening += new CancelEventHandler(MyMenu_Opening);
            
            _cell = cell;
            grid = _sgdefine.grid;
            sgdefine = _sgdefine;
            MyMenu.Width = 100;

            // 初始化编辑状态
            UpdateCellEditState();
        }

        /// <summary>
        /// 双击事件处理：有图片查看大图，无图片进入编辑模式
        /// </summary>
        public override void OnDoubleClick(SourceGrid.CellContext sender, EventArgs e)
        {
            if (sender.Cell != null)
            {
                // 尝试从多个来源获取图片数据
                byte[] imageBytes = null;

                // 方法1：从 ValueImageWeb 的 CellImageBytes 获取
                var model = sender.Cell.Model.FindModel(typeof(SourceGrid.Cells.Models.ValueImageWeb));
                if (model is SourceGrid.Cells.Models.ValueImageWeb valueImageWeb)
                {
                    if (valueImageWeb.CellImageBytes != null && valueImageWeb.CellImageBytes.Length > 0)
                    {
                        imageBytes = valueImageWeb.CellImageBytes;
                        MainForm.Instance.PrintInfoLog($"找到图片数据，大小: {imageBytes.Length} bytes");
                    }
                    else
                    {
                        MainForm.Instance.PrintInfoLog("ValueImageWeb.CellImageBytes 为空或长度为0");
                    }
                }
                else
                {
                    MainForm.Instance.PrintInfoLog("未找到 ValueImageWeb 模型");
                }

                // 方法2：从 sender.Value 获取（如果是 byte[] 类型）
                if (imageBytes == null && sender.Value is byte[] valueBytes && valueBytes.Length > 0)
                {
                    imageBytes = valueBytes;
                    MainForm.Instance.PrintInfoLog($"从 sender.Value 找到图片数据，大小: {imageBytes.Length} bytes");
                }

                // 方法3：从 sender.Value 获取（如果是 Bitmap 类型）
                if (imageBytes == null && sender.Value is System.Drawing.Bitmap bitmap)
                {
                    using (var ms = new System.IO.MemoryStream())
                    {
                        bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        imageBytes = ms.ToArray();
                        MainForm.Instance.PrintInfoLog($"从 sender.Value(Bitmap) 转换图片数据，大小: {imageBytes.Length} bytes");
                    }
                }

                // 方法4：从 RemoteImageView.GridImage 获取（异步加载的图片）
                if (imageBytes == null && sender.Cell.View is RemoteImageView imageView && imageView.GridImage != null)
                {
                    MainForm.Instance.PrintInfoLog($"从 RemoteImageView.GridImage 转换图片数据: {imageView.GridImage.Size}");
                    try
                    {
                        using (var ms = new System.IO.MemoryStream())
                        {
                            imageView.GridImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                            imageBytes = ms.ToArray();
                            MainForm.Instance.PrintInfoLog($"GridImage 转换成功，大小: {imageBytes.Length} bytes");
                        }
                    }
                    catch (Exception ex)
                    {
                        MainForm.Instance.PrintInfoLog($"GridImage 转换失败: {ex.Message}");
                    }
                }

                if (imageBytes != null && imageBytes.Length > 0)
                {
                    // 有图片，查看大图，不调用基类方法（避免进入编辑模式）
                    MainForm.Instance.PrintInfoLog("双击：有图片，显示大图，不进入编辑模式");
                    ImageGridHelper.ShowImageInViewer(imageBytes);
                    return;
                }
                else
                {
                    MainForm.Instance.PrintInfoLog("双击：无图片数据，准备进入编辑模式");
                }
            }
            // 没有图片，调用基类方法进入编辑模式
            base.OnDoubleClick(sender, e);
        }

        /// <summary>
        /// 鼠标松开事件：右键显示菜单
        /// </summary>
        public override void OnMouseUp(SourceGrid.CellContext sender, MouseEventArgs e)
        {
            base.OnMouseUp(sender, e);

            if (e.Button == MouseButtons.Right)
            {
                MyMenu.Show(sender.Grid, new Point(e.X, e.Y));
            }
        }

        private void SiCustom_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.ToolStripMenuItem item = (System.Windows.Forms.ToolStripMenuItem)sender;
            SourceGrid.Cells.Cell cell = (SourceGrid.Cells.Cell)item.Tag;

            // 尝试从多个来源获取图片数据
            byte[] imageBytes = null;

            // 方法1：从 ValueImageWeb 的 CellImageBytes 获取
            var model = cell.Model.FindModel(typeof(SourceGrid.Cells.Models.ValueImageWeb));
            if (model is SourceGrid.Cells.Models.ValueImageWeb valueImageWeb)
            {
                if (valueImageWeb.CellImageBytes != null && valueImageWeb.CellImageBytes.Length > 0)
                {
                    imageBytes = valueImageWeb.CellImageBytes;
                }
            }

            // 注意：无法直接访问 Cell.Value，因为 ICellVirtual 没有此属性
            // 只能通过 ValueImageWeb.CellImageBytes 获取图片数据

            // 显示图片
            if (imageBytes != null && imageBytes.Length > 0)
            {
                ImageGridHelper.ShowImageInViewer(imageBytes);
            }
            else
            {
                MainForm.Instance.PrintInfoLog("未找到图片数据");
            }

            // 强制重绘
            grid.Refresh();
        }

        private async void SiUpload_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.ToolStripMenuItem item = (System.Windows.Forms.ToolStripMenuItem)sender;
            SourceGrid.Cells.Cell cell = (SourceGrid.Cells.Cell)item.Tag;
            if (cell.View is RemoteImageView)
            {
                RemoteImageView imageView = cell.View as RemoteImageView;
                if (imageView != null)
                {
                    // 打开文件选择对话框
                    OpenFileDialog openFileDialog = new OpenFileDialog
                    {
                        Filter = "图片文件|*.jpg;*.jpeg;*.png;*.gif;*.bmp",
                        Title = "选择要上传的图片"
                    };

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            // 读取图片文件
                            byte[] imageBytes = File.ReadAllBytes(openFileDialog.FileName);
                            string fileName = Path.GetFileName(openFileDialog.FileName);

                            // 上传图片
                            string imageId = await imageView.UploadImageAsync(imageBytes, fileName);
                            if (!string.IsNullOrEmpty(imageId))
                            {
                            // 更新单元格值
                                cell.Value = imageId;
                                
                                // 强制重绘
                                grid.Refresh();

                                // 更新编辑状态
                                UpdateCellEditState();
                                
                                MainForm.Instance.PrintInfoLog("图片上传成功: " + fileName);
                            }
                        }
                        catch (Exception ex)
                        {
                            MainForm.Instance.PrintInfoLog("图片上传失败: " + ex.Message);
                        }
                    }
                }
            }
        }

        private async void SiDelete_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.ToolStripMenuItem item = (System.Windows.Forms.ToolStripMenuItem)sender;
            SourceGrid.Cells.Cell cell = (SourceGrid.Cells.Cell)item.Tag;
            if (cell.View is RemoteImageView)
            {
                RemoteImageView imageView = cell.View as RemoteImageView;
                if (imageView != null)
                {
                    // 确认删除
                    if (MessageBox.Show("确定要删除此图片吗？", "确认删除", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        try
                        {
                            // 获取图片ID
                            string imageId = cell.Value as string;
                            if (!string.IsNullOrEmpty(imageId))
                            {
                            // 删除图片
                                bool result = await imageView.DeleteImageAsync(imageId);
                                if (result)
                                {
                                    // 清空单元格值和图片数据
                                    cell.Value = null;
                                    
                                    var model = cell.Model.FindModel(typeof(SourceGrid.Cells.Models.ValueImageWeb));
                                    if (model is SourceGrid.Cells.Models.ValueImageWeb valueImageWeb)
                                    {
                                        valueImageWeb.CellImageBytes = null;
                                        valueImageWeb.CellImageHashName = null;
                                    }
                                    
                                    imageView.GridImage = null;
                                    
                                    // 强制重绘
                                    grid.Refresh();

                                    // 更新编辑状态
                                    UpdateCellEditState();
                                    
                                    MainForm.Instance.PrintInfoLog("图片删除成功");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MainForm.Instance.PrintInfoLog("图片删除失败: " + ex.Message);
                        }
                    }
                }
            }
        }

        private async void SiReplace_Click(object sender, EventArgs e)
        {
            // 替换图片：先删除旧图片，再上传新图片
            System.Windows.Forms.ToolStripMenuItem item = (System.Windows.Forms.ToolStripMenuItem)sender;
            SourceGrid.Cells.Cell cell = (SourceGrid.Cells.Cell)item.Tag;
            
            // 先删除旧图片
            string oldImageId = cell.Value as string;
            if (!string.IsNullOrEmpty(oldImageId))
            {
                try
                {
                    var model = cell.Model.FindModel(typeof(SourceGrid.Cells.Models.ValueImageWeb));
                    if (model is SourceGrid.Cells.Models.ValueImageWeb valueImageWeb)
                    {
                        if (cell.View is RemoteImageView imageView)
                        {
                            // 删除旧图片
                            bool deleteResult = await imageView.DeleteImageAsync(oldImageId);
                            if (!deleteResult)
                            {
                                MainForm.Instance.PrintInfoLog("删除旧图片失败，取消替换操作");
                                return;
                            }
                            
                            // 清空单元格值和图片数据
                            cell.Value = null;
                            valueImageWeb.CellImageBytes = null;
                            valueImageWeb.CellImageHashName = null;
                            imageView.GridImage = null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MainForm.Instance.PrintInfoLog("删除旧图片失败: " + ex.Message);
                    return;
                }
            }
            
            // 再上传新图片
            SiUpload_Click(sender, e);
        }

        private void SiUpdate_Click(object sender, EventArgs e)
        {
            // 更新图片功能可以复用上传图片的逻辑
            SiUpload_Click(sender, e);
        }

        public override void OnClick(CellContext sender, EventArgs e)
        {
            base.OnClick(sender, e);
        }




        private void MyMenu_Opening(object sender, CancelEventArgs e)
        {
            // 先清空现有菜单项
            MyMenu.Items.Clear();
            
            // 检查是否应该显示菜单
            bool shouldShowMenu = CheckConditionToShowMenu();
            if (!shouldShowMenu)
            {
                e.Cancel = true;
                return;
            }
            
            // 动态创建菜单项
            CreateDynamicMenuItems();
        }
        
        /// <summary>
        /// 动态创建菜单项
        /// </summary>
        private void CreateDynamicMenuItems()
        {
            // 查看大图菜单项（始终显示）
            ToolStripMenuItem siViewLarge = new ToolStripMenuItem("查看大图");
            siViewLarge.Tag = _cell;
            siViewLarge.Click += SiCustom_Click;
            MyMenu.Items.Add(siViewLarge);
            
            MyMenu.Items.Add(new ToolStripSeparator());
            
            bool hasImage = HasImageInCell();
            
            if (IsSingleImageMode)
            {
                // 单图模式
                if (hasImage)
                {
                    // 有图片时显示"替换图片"
                    ToolStripMenuItem siReplace = new ToolStripMenuItem("替换图片");
                    siReplace.Tag = _cell;
                    siReplace.Click += SiReplace_Click;
                    MyMenu.Items.Add(siReplace);
                    
                    // 删除图片
                    ToolStripMenuItem siDelete = new ToolStripMenuItem("删除图片");
                    siDelete.Tag = _cell;
                    siDelete.Click += SiDelete_Click;
                    MyMenu.Items.Add(siDelete);
                }
                else
                {
                    // 无图片时显示"上传图片"
                    ToolStripMenuItem siUpload = new ToolStripMenuItem("上传图片");
                    siUpload.Tag = _cell;
                    siUpload.Click += SiUpload_Click;
                    MyMenu.Items.Add(siUpload);
                }
            }
            else
            {
                // 多图模式
                // 始终显示"上传图片"
                ToolStripMenuItem siUpload = new ToolStripMenuItem("上传图片");
                siUpload.Tag = _cell;
                siUpload.Click += SiUpload_Click;
                MyMenu.Items.Add(siUpload);
                
                if (hasImage)
                {
                    // 有图片时显示"删除图片"
                    ToolStripMenuItem siDelete = new ToolStripMenuItem("删除图片");
                    siDelete.Tag = _cell;
                    siDelete.Click += SiDelete_Click;
                    MyMenu.Items.Add(siDelete);
                }
            }
        }
        
        /// <summary>
        /// 检查单元格是否有图片
        /// </summary>
        private bool HasImageInCell()
        {
            // 检查 ValueImageWeb 的 CellImageBytes
            var model = _cell.Model.FindModel(typeof(SourceGrid.Cells.Models.ValueImageWeb));
            if (model is SourceGrid.Cells.Models.ValueImageWeb valueImageWeb)
            {
                if (valueImageWeb.CellImageBytes != null && valueImageWeb.CellImageBytes.Length > 0)
                {
                    return true;
                }
            }

            // 检查 RemoteImageView.GridImage
            if (_cell.View is RemoteImageView imageView && imageView.GridImage != null)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 检查是否应该显示右键菜单
        /// 有图片时显示（查看大图、替换、删除）
        /// 无图片时也显示（上传图片）
        /// </summary>
        private bool CheckConditionToShowMenu()
        {
            SourceGrid.Cells.Cell cell = _cell;
            // 只要是 RemoteImageView 类型的单元格，就显示右键菜单
            return cell.View is RemoteImageView;
        }

        /// <summary>
        /// 根据图片状态更新单元格的编辑状态
        /// 有图片时禁止编辑（通过右键菜单替换），无图片时允许编辑
        /// </summary>
        private void UpdateCellEditState()
        {
            if (_cell == null)
                return;

            bool hasImage = HasImageInCell();

            // 如果是 ImageWebPickEditor，设置其 EditableMode
            if (_cell.Editor is SourceGrid.Cells.Editors.ImageWebPickEditor imageEditor)
            {
                // 有图片时禁止直接编辑，替换通过右键菜单
                // 无图片时允许编辑（可以双击进入编辑模式上传图片）
                imageEditor.EnableEdit = !hasImage;
            }
        }

    }



}
