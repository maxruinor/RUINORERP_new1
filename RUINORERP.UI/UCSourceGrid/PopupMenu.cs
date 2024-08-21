using RUINORERP.Common.Extensions;
using RUINORERP.UI.ToolForm;
using RUINORERP.UI.UControls;
using SourceGrid;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Winista.Text.HtmlParser.Data;
using Color = System.Drawing.Color;

namespace RUINORERP.UI.UCSourceGrid
{

    /// <summary>
    /// 公共右键菜单
    /// </summary>
    public class PopupMenu : SourceGrid.Cells.Controllers.ControllerBase
    {

        public delegate void ColumnsVisibleDelegate(KeyValuePair<string, SourceGridDefineColumnItem> kv);
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
        private List<KeyValuePair<string, SourceGridDefineColumnItem>> items = new List<KeyValuePair<string, SourceGridDefineColumnItem>>();
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
        public void AddItems(KeyValuePair<string, SourceGridDefineColumnItem> item)
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
            KeyValuePair<string, SourceGridDefineColumnItem> item = items.Find(kv => kv.Key == sender.ToString());
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


    /// <summary>
    /// 带自定义列的右键菜单。后面也可能合并都有配置性
    /// </summary>
    public class PopupMenuWithCustomColumns : SourceGrid.Cells.Controllers.ControllerBase
    {

        public delegate void ColumnsVisibleDelegate(KeyValuePair<string, SourceGridDefineColumnItem> kv);
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
        private List<KeyValuePair<string, SourceGridDefineColumnItem>> items = new List<KeyValuePair<string, SourceGridDefineColumnItem>>();
        //private SerializableDictionary<string, bool> items = new SerializableDictionary<string, bool>();

        /// <summary>
        /// 用来保存配置自定义列
        /// </summary>
        private string _xmlfileName;
        public PopupMenuWithCustomColumns(string xmlfileName)
        {
            MyMenu.ShowCheckMargin = true;
            _xmlfileName = xmlfileName;
            ToolStripSeparator ss = new ToolStripSeparator();
            MyMenu.Items.Add(ss);
            ToolStripMenuItem siCustom = new ToolStripMenuItem("自定义");
            siCustom.Click += SiCustom_Click;
            MyMenu.Items.Add(siCustom);

            ConfigColItems = Common.UIHelper.LoadColumnsList(xmlfileName);

            //menu.MenuItems.Add("Menu 1", new EventHandler(Menu1_Click));
            //menu.MenuItems.Add("Menu 2", new EventHandler(Menu2_Click));
        }

        frmShowColumns frm = new frmShowColumns();
        private void SiCustom_Click(object sender, EventArgs e)
        {
            frm.XmlFileName = _xmlfileName;
            frm.Items = items;
            frm.ConfigItems = ConfigColItems;
            if (frm.ShowDialog() == DialogResult.OK)
            {
                //更新配置变化，体现到这里。思路是?
                foreach (var item in items)
                {
                    OnColumnsVisible(item);
                }
                Common.UIHelper.SaveColumnsList(ConfigColItems, _xmlfileName);
            }

        }


        private SerializableDictionary<string, bool> _ConfigColItems = new SerializableDictionary<string, bool>();

        /// <summary>
        /// 保存自定义列的集合
        /// </summary>
        public SerializableDictionary<string, bool> ConfigColItems { get => _ConfigColItems; set => _ConfigColItems = value; }


        /// <summary>
        /// 添加要控制的列,这个时候 就可以保存配置了
        /// </summary>
        /// <param name="item"></param>
        public void AddItems(KeyValuePair<string, SourceGridDefineColumnItem> item)
        {
            if (!item.Value.NeverVisible)
            {
                //缓存控制，添加，如果存在则修改状态，如果没有则添加
                if (ConfigColItems.ContainsKey(item.Key))
                {
                    ConfigColItems[item.Key] = item.Value.Visible;
                }
                else
                {
                    ConfigColItems.Add(item.Key, item.Value.Visible);
                }

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
            KeyValuePair<string, SourceGridDefineColumnItem> item = items.Find(kv => kv.Key == sender.ToString());
            item.Value.Visible = (sender as ToolStripMenuItem).Checked;
            //OnColumnsVisible(item.Value.ColIndex, item.Value.ColName, item.Value.Visible);
            OnColumnsVisible(item);
            //要更新到配置中
            if (ConfigColItems.ContainsKey(item.Key))
            {
                ConfigColItems[item.Key] = item.Value.Visible;
            }
            Common.UIHelper.SaveColumnsList(ConfigColItems, _xmlfileName);
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
        public delegate void ColumnsVisibleDelegate(KeyValuePair<string, SourceGridDefineColumnItem> kv);
        /// <summary>
        /// 验证行数据
        /// </summary>
        public event ColumnsVisibleDelegate OnColumnsVisible;


        ContextMenuStrip MyMenu = new ContextMenuStrip();

        private List<KeyValuePair<string, SourceGridDefineColumnItem>> items = new List<KeyValuePair<string, SourceGridDefineColumnItem>>();

        SourceGridDefine sgdefine;
        public PopupMenuForRowHeader(int addRowIndex, Grid _grid, SourceGridDefine _sgdefine)
        {
            MyMenu.ShowCheckMargin = true;
            //ToolStripSeparator ss = new ToolStripSeparator();
            //MyMenu.Items.Add(ss);
            ToolStripMenuItem siCustom = new ToolStripMenuItem("删除行【" + addRowIndex + "】");
            siCustom.Tag = addRowIndex;
            siCustom.Click += SiCustom_Click;
            MyMenu.Items.Add(siCustom);
            grid = _grid;
            sgdefine = _sgdefine;
        }

        private void SiCustom_Click(object sender, EventArgs e)
        {

            //思路先把数据移掉一行，再重复设置一下右键删除行的菜单，并且重新设置行号,并且添加一行

            System.Windows.Forms.ToolStripMenuItem item = (System.Windows.Forms.ToolStripMenuItem)sender;
            int addRowIndex = int.Parse(item.Tag.ToString());
            SourceGridHelper sh = new SourceGridHelper();
            sh.DeleteRow(sgdefine, addRowIndex);
            return;

            //注意这里可能有顺序，先删除数据行的数据，不然下面grid.remove可能是变化的索引
            sgdefine.BindingSourceLines.Remove(grid.Rows[addRowIndex].RowData);
            grid.Rows.Remove(addRowIndex);

            //
            sgdefine.UseCalculateTotalValue(grid.Tag as SourceGridDefine);

            grid.Selection.ResetSelection(true);
      
            //插入一行新的空行
            sh.InsertRow(grid, sgdefine, true);

            //删除一行后。后面的行的索引都会变化。要重新处理一次，并且行号重新显示
            foreach (GridRow row in grid.Rows)
            {
                //重新设置列行头跳过
                if (row.Index == 0)
                {
                    continue;
                }
                if (grid.HasSummary)
                {
                    //排除总计行
                    if (grid[row.Index, 0].Tag != null && grid[row.Index, 0].Tag.ToString() == "SummaryRow")
                    {
                        continue;
                    }
                }
                grid[row.Index, 0].View.BackColor = Color.LightGreen;

                //行头给右键菜单 不为空的才是已经设置过了。有正常数据的行
                PopupMenuForRowHeader pop = grid[row.Index, 0].FindController<PopupMenuForRowHeader>();
                if (pop == null)
                {
                    //没有设置过右键菜单的，非正常数据行
                    grid[row.Index, 0].Value = row.Index;
                    //先全部默认为紫色，再有正常数据的行再重新设置颜色为绿色
                    grid[row.Index, 0].View.BackColor = sgdefine.RowHeadBackColor;
                }
                else
                {
                    grid[row.Index, 0].Controller.RemoveController(pop);
                    PopupMenuForRowHeader menuController = new PopupMenuForRowHeader(row.Index, grid, sgdefine);
                    grid[row.Index, 0].Controller.AddController(menuController);
                    grid[row.Index, 0].Value = row.Index;
                    //grid[row.Index, 0].View.BackColor = Color.LightGreen;
                    //grid[row.Index, 0].View = new DevAge.Drawing.VisualElements.BackgroundLinearGradient(Color.ForestGreen, Color.White, 45);
                }

                if (row.RowData != null)
                {
                    //有数据行则为绿色
                    var pk = sgdefine.DefineColumns.FirstOrDefault(c => c.IsPrimaryBizKeyColumn);
                    if (pk != null)
                    {
                        var pkid = row.RowData.GetPropertyValue(pk.ColName);
                        if (pkid.ToDecimal() > 0)
                        {
                            grid[row.Index, 0].View.BackColor = Color.LightGreen;
                        }
                        else
                        {

                        }
                    }

                }
                else
                {

                }


            }



            //要更新总计行数据。实际这暂时是复制单据控制器里的代码。应该可以重构出一个公共的方法

            #region  总计 总计列
            if (grid.HasSummary)
            {
                foreach (SourceGridDefineColumnItem col in sgdefine)
                {
                    if (col.Summary)
                    {
                        #region 总计其中一列
                        decimal totalTemp = 0;
                        //去掉首尾行
                        for (int r = 1; r < grid.RowsCount - 1; r++)
                        {
                            if (grid[r, col.ColIndex].Value != null &&
                                grid.Rows[r].RowData != null
                                )
                            {
                                decimal CurrentTemp = 0;
                                if (decimal.TryParse(grid[r, col.ColIndex].Value.ToString(), out CurrentTemp))
                                {
                                    totalTemp = CurrentTemp + totalTemp;

                                }
                            }
                        }
                        //最后一行
                        grid[grid.RowsCount - 1, col.ColIndex].Value = totalTemp;
                        #endregion
                    }
                }

            }
            #endregion

            //强制重绘
            grid.Refresh();

        }

        public void AddItems(KeyValuePair<string, SourceGridDefineColumnItem> item)
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
            KeyValuePair<string, SourceGridDefineColumnItem> item = items.Find(kv => kv.Key == sender.ToString());
            item.Value.Visible = (sender as ToolStripMenuItem).Checked;
            OnColumnsVisible(item);
        }

        public override void OnClick(CellContext sender, EventArgs e)
        {
            base.OnClick(sender, e);
        }

        public List<KeyValuePair<string, SourceGridDefineColumnItem>> Items { get => items; set => items = value; }

        public override void OnMouseUp(SourceGrid.CellContext sender, MouseEventArgs e)
        {
            base.OnMouseUp(sender, e);

            if (e.Button == MouseButtons.Right)
                MyMenu.Show(sender.Grid, new Point(e.X, e.Y));
        }

    }



    /// <summary>
    /// 用于显示全选 全不选 反选
    /// </summary>
    public class PopupMenuForSelect : SourceGrid.Cells.Controllers.ControllerBase
    {
        private Grid grid;
        ContextMenuStrip MyMenu = new ContextMenuStrip();

        private List<KeyValuePair<string, SourceGridDefineColumnItem>> items = new List<KeyValuePair<string, SourceGridDefineColumnItem>>();

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
            SourceGridDefineColumnItem selected = _sgdefine.DefineColumns.Find(c => c.ColName == "Selected");
            if (selected != null)
            {
                foreach (GridRow row in grid.Rows)
                {
                    if (row.RowData == null)
                    {
                        continue;
                    }
                    GridColumn mColumn = grid.Columns[selected.ColIndex] as GridColumn;
                    if (mColumn != null)
                    {
                        string txt = sender.ToString();
                        switch (txt)
                        {
                            case "全选":
                                grid[row.Index, selected.ColIndex].Value = true;
                                break;
                            case "全不选":
                                grid[row.Index, selected.ColIndex].Value = false;
                                break;
                            case "反选"://_sgdefine.grid.Columns[selected.ColIndex].
                                if (grid[row.Index, selected.ColIndex].Value is Boolean bl)
                                {
                                    grid[row.Index, selected.ColIndex].Value = !bl;
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

        public void AddItems(KeyValuePair<string, SourceGridDefineColumnItem> item)
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
            KeyValuePair<string, SourceGridDefineColumnItem> item = items.Find(kv => kv.Key == sender.ToString());
            item.Value.Visible = (sender as ToolStripMenuItem).Checked;
        }

        public override void OnClick(CellContext sender, EventArgs e)
        {
            base.OnClick(sender, e);
        }

        public List<KeyValuePair<string, SourceGridDefineColumnItem>> Items { get => items; set => items = value; }

        public override void OnMouseUp(SourceGrid.CellContext sender, MouseEventArgs e)
        {
            base.OnMouseUp(sender, e);

            if (e.Button == MouseButtons.Right)
                MyMenu.Show(sender.Grid, new Point(e.X, e.Y));
        }

    }


    /// <summary>
    /// 用于项这个单元格的删除操作，也可以集成其它功能？
    /// </summary>
    public class PopupMenuForDeleteSelect : SourceGrid.Cells.Controllers.ControllerBase
    {
        private Grid grid;
        ContextMenuStrip MyMenu = new ContextMenuStrip();

        private List<KeyValuePair<string, SourceGridDefineColumnItem>> items = new List<KeyValuePair<string, SourceGridDefineColumnItem>>();

        SourceGridDefine sgdefine;
        public PopupMenuForDeleteSelect(Grid _grid, SourceGridDefine _sgdefine)
        {
            MyMenu.ShowCheckMargin = true;

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
            SourceGridDefineColumnItem selected = _sgdefine.DefineColumns.Find(c => c.ColName == "Selected");
            if (selected != null)
            {
                List<int> deleteRows = new List<int>();
                foreach (GridRow row in grid.Rows)
                {
                    if (row.RowData == null)
                    {
                        continue;
                    }
                    if (grid[row.Index, selected.ColIndex].Value is Boolean bl)
                    {
                        if (bl)
                        {
                            deleteRows.Add(row.Index);
                        }
                    }
                }

                GridColumn mColumn = grid.Columns[selected.ColIndex] as GridColumn;
                if (mColumn != null)
                {
                    string txt = sender.ToString();
                    switch (txt)
                    {
                        case "删除选中行":
                            sh.DeleteRow(_sgdefine,deleteRows.ToArray());
                            break;
                        case "显示多选":
                            if (selected != null)
                            {
                                grid.Columns[selected.ColIndex].Visible = item.Checked;
                                MyMenu.Items.Find("删除选中行", true)[0].Visible = item.Checked;
                            }
                            else
                            {
                                //没有实现选中列的功能，请在明细表加载时候处理指定选择列
                            }
                            break;
                        case "反选"://_sgdefine.grid.Columns[selected.ColIndex].
                            break;
                        default:
                            break;
                    }
                }


            }
        }




        public void AddItems(KeyValuePair<string, SourceGridDefineColumnItem> item)
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
            KeyValuePair<string, SourceGridDefineColumnItem> item = items.Find(kv => kv.Key == sender.ToString());
            item.Value.Visible = (sender as ToolStripMenuItem).Checked;
        }

        public override void OnClick(CellContext sender, EventArgs e)
        {
            base.OnClick(sender, e);
        }

        public List<KeyValuePair<string, SourceGridDefineColumnItem>> Items { get => items; set => items = value; }

        public override void OnMouseUp(SourceGrid.CellContext sender, MouseEventArgs e)
        {
            base.OnMouseUp(sender, e);

            if (e.Button == MouseButtons.Right)
                MyMenu.Show(sender.Grid, new Point(e.X, e.Y));
        }

    }

}
