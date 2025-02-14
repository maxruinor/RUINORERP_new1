using HLH.WinControl.MyTypeConverter;
using Krypton.Toolkit;
using RUINORERP.Business.Processor;
using RUINORERP.Model;
using RUINORERP.Model.Models;
using RUINORERP.UI.UControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Common.Extensions;
using RUINORERP.UI.Common;
using Netron.GraphLib;
using RUINORERP.Global.EnumExt;
using RUINORERP.Common;
using FastReport.DevComponents.WinForms.Drawing;
using RUINORERP.UI.UCSourceGrid;

namespace RUINORERP.UI.UserPersonalized
{
    /// <summary>
    /// 表格显示
    /// </summary>
    public partial class frmGridViewColSetting : KryptonForm
    {
        public frmGridViewColSetting()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
        }



        public List<ColDisplayController> ColumnDisplays { get; set; } = new List<ColDisplayController>();

        public ColDisplayController[] oldColumnDisplays;

        ContextMenuStrip contentMenu1;
        public tb_MenuInfo CurMenuInfo { get; set; }

        private void btnOk_Click(object sender, EventArgs e)
        {
            string shou = string.Empty;

            foreach (ListViewItem item in listView1.Items)
            {
                if (item.Checked)
                {
                    shou += item.Name + ",";
                }
            }
            shou = shou.TrimEnd(',');
            if (shou == "")
            {
                MessageBox.Show("不能隐藏所有列！", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int sortindex = 0;
            //上面临时保存了一个之前的序列数组
            foreach (ListViewItem item in listView1.Items)
            {
                if (item.Tag is ColDisplayController columnDisplays)
                {
                    if (columnDisplays != null)
                    {
                        ColDisplayController cdc = ColumnDisplays.Where(c => c.ColName == columnDisplays.ColName).FirstOrDefault();
                        if (cdc != null)
                        {
                            cdc.ColDisplayIndex = sortindex;
                        }
                        else
                        {
                            cdc.ColDisplayIndex = -1;
                        }
                    }
                }
                if (string.IsNullOrEmpty(item.Text))
                {
                    sortindex++;
                    continue;
                }

                sortindex++;
            }



            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        public string MenuPathKey { get; set; }

        private async void frmMenuPersonalization_Load(object sender, EventArgs e)
        {
            if (ConfiguredGrid.GetType().Name == "NewSumDataGridView")
            {
                dataGridView = ConfiguredGrid as NewSumDataGridView;
            }


            listView1.AllowDrop = true;
            if (dataGridView != null)
            {
                //添加列宽显示模式
                dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
                //
                // 摘要:
                //     列宽不会自动调整。
                //
                // 摘要:
                //     列宽调整到适合列标头单元格的内容。
                //
                // 摘要:
                //     列宽调整到适合列中除标头单元格以外所有单元格的内容。
                //
                // 摘要:
                //     列宽调整到适合列中所有单元格（包括标头单元格）的内容。
                //
                // 摘要:
                //     列宽调整到适合位于屏幕上当前显示的行中的列的所有单元格（不包括标头单元格）的内容。
                //
                // 摘要:
                //     列宽调整到适合位于屏幕上当前显示的行中的列的所有单元格（包括标头单元格）的内容。
                //
                // 摘要:
                //     列宽调整到使所有列宽精确填充控件的显示区域，要求使用水平滚动的目的只是保持列宽大于 System.Windows.Forms.DataGridViewColumn.MinimumWidth
                //     属性值。相对列宽由相对 System.Windows.Forms.DataGridViewColumn.FillWeight 属性值决定。
            }

            Dictionary<int, string> valuePairs = new Dictionary<int, string>();
            valuePairs.Add(1, "无");
            valuePairs.Add(2, "列头");
            valuePairs.Add(4, "所有单元格内容(不含列头)");
            valuePairs.Add(6, "所有单元格内容");
            valuePairs.Add(8, "显示的单元格内容(不含列头)");
            valuePairs.Add(10, "显示的单元格内容");
            valuePairs.Add(16, "填充");

            //AllCells = 6,
            //AllCellsExceptHeader = 4,
            //DisplayedCells = 10,
            //DisplayedCellsExceptHeader = 8,
            //None = 1,
            //ColumnHeader = 2,
            //Fill = 16

            DataBindingHelper.BindData4CmbByDictionary<tb_UIGridSetting>(GridSetting, k => k.ColumnsMode, valuePairs, cmbColsDisplayModel, false);
            LoadColumnDisplayList();
            this.listView1.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView1_ItemChecked);
            //将LoadEditGridCol延迟1秒后执行
            await Task.Delay(500);
            LoadEditGridCol();

            listView1.ItemDrag += ListView1_ItemDrag;
            listView1.DragEnter += listView1_DragEnter;
            listView1.DragOver += listView1_DragOver;
            listView1.DragDrop += listView1_DragDrop;
            listView1.DragLeave += listView1_DragLeave;

        }

        private void LoadColumnDisplayList()
        {
            listView1.Columns.Clear();
            listView1.Items.Clear();


            listView1.Columns.Add("显示列名");
            listView1.Columns[0].TextAlign = HorizontalAlignment.Center;
            listView1.Columns[0].Width = -2; //-1 -2 

            oldColumnDisplays = new ColDisplayController[ColumnDisplays.Count];
            ColumnDisplays.CopyTo(oldColumnDisplays);

            //将不可用的显示排序设置为-1
            //var uniqueItems = ColumnDisplays1.Except(ColumnDisplays2).ToList();
            ColumnDisplays.Where(c => c.Disable).ToList().ForEach(f => f.ColDisplayIndex = 1000);

            //重新得到一个集合的方法
            // 对 ColumnDisplays 按 ColDisplayIndex 排序（升序，小的排在前面）OrderBy / 降序OrderByDescending
            //ColumnDisplays = ColumnDisplays.OrderBy(c => c.ColDisplayIndex).ToList();
            //对原始集合排序
            ColumnDisplays.Sort((x, y) => x.ColDisplayIndex.CompareTo(y.ColDisplayIndex));

            foreach (ColDisplayController keyValue in ColumnDisplays)
            {
                if (!keyValue.Disable)
                {
                    //listView1.Items.Insert(0, new ListViewItem(item.Key.ToString()));
                    ListViewItem lvi = new ListViewItem();
                    lvi.Checked = keyValue.Visible;
                    lvi.Name = keyValue.ColName;
                    lvi.Tag = keyValue;
                    lvi.Text = keyValue.ColDisplayText;
                    //用这个来保存
                    lvi.ImageKey = keyValue.ColDisplayIndex.ToString();
                    listView1.Items.Add(lvi);
                }
            }


            //添加悬浮提示  
            ToolTip tt = new ToolTip();
            tt.InitialDelay = 200;
            tt.AutomaticDelay = 200;
            tt.ReshowDelay = 200;
            tt.ShowAlways = true;

            //tt.SetToolTip(pictureBox_down, "下移选中字段（已选择字段列表）");  
            //tt.SetToolTip(pictureBox_downdown, "置底选中字段（已选择字段列表）");  
            //tt.SetToolTip(pictureBox_left, "左移选中字段");  
            //tt.SetToolTip(pictureBox_right, "右移选中字段");  
            //tt.SetToolTip(pictureBox_up, "上移选中字段（已选择字段列表）");  
            //tt.SetToolTip(pictureBox_upup, "置顶选中字段（已选择字段列表）");  
            //tt.SetToolTip(listView1, "双击从未选择字段列表中移除");  
            //tt.SetToolTip(listView2, "双击从已选择字段列表中移除");  

            contentMenu1 = new ContextMenuStrip();
            contentMenu1.Items.Add("全选");
            contentMenu1.Items.Add("全不选");
            contentMenu1.Items.Add("反选");
            contentMenu1.Items[0].Click -= new EventHandler(contentMenu1_CheckAll);
            contentMenu1.Items[0].Click += new EventHandler(contentMenu1_CheckAll);
            contentMenu1.Items[1].Click -= new EventHandler(contentMenu1_CheckNo);
            contentMenu1.Items[1].Click += new EventHandler(contentMenu1_CheckNo);
            contentMenu1.Items[2].Click -= new EventHandler(contentMenu1_Inverse);
            contentMenu1.Items[2].Click += new EventHandler(contentMenu1_Inverse);
            listView1.ContextMenuStrip = contentMenu1;
        }

        #region 拖拽事件
        //启动拖拽，设置拖拽的数据和效果。
        private void ListView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            listView1.DoDragDrop(e.Item, DragDropEffects.Move);
        }

        //拖拽进入ListView，判断拖拽的数据格式，并设置拖拽的效果。
        private void listView1_DragEnter(object sender, DragEventArgs e)
        {
            //e.Effect = e.AllowedEffect;
            e.Effect = DragDropEffects.Move;
        }

        //拖动经过ListView时，设置拖动的效果，显示拖放位置线
        private void listView1_DragOver(object sender, DragEventArgs e)
        {
            System.Drawing.Point ptScreen = new System.Drawing.Point(e.X, e.Y);
            System.Drawing.Point pt = listView1.PointToClient(ptScreen);
            ListViewItem item = listView1.GetItemAt(pt.X, pt.Y);
            if (item != null)
                item.Selected = true;


        }

        //拖拽释放，移动行
        private void listView1_DragDrop(object sender, DragEventArgs e)
        {
            ListViewItem draggedItem = (ListViewItem)e.Data.GetData(typeof(ListViewItem));
            System.Drawing.Point ptScreen = new System.Drawing.Point(e.X, e.Y);
            System.Drawing.Point pt = listView1.PointToClient(ptScreen);
            ListViewItem TargetItem = listView1.GetItemAt(pt.X, pt.Y); // 拖动的项将放置于该项之前    
            if (TargetItem == null)
            {
                return;
            }
            listView1.Items.Insert(TargetItem.Index, (ListViewItem)draggedItem.Clone());
            listView1.Items.Remove(draggedItem);

        }


        private void listView1_DragLeave(object sender, EventArgs e)
        {
            listView1.InsertionMark.Index = -1;
        }

        #endregion
        private class ListViewIndexComparer : System.Collections.IComparer
        {
            public int Compare(object x, object y)
            {
                return ((ListViewItem)x).Index - ((ListViewItem)y).Index;
            }
        }


        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //提前统一加载就不闪屏？
            if (listView1.SelectedItems != null && listView1.SelectedItems.Count > 0)
            {
                var entity = listView1.SelectedItems[0].Tag as ColDisplayController;
                if (panelConditionEdit.Controls.ContainsKey(entity.ColName.ToString()))
                {
                    var uCQuery = panelConditionEdit.Controls.CastToList<Control>().FirstOrDefault(c => c.Name == entity.ColName.ToString());
                    if (uCQuery != null)
                    {
                        uCQuery.Visible = true;
                    }
                    //其它隐藏
                    panelConditionEdit.Controls.CastToList<Control>().Where(c => c.Name != entity.ColName.ToString()).ToList().ForEach(c => c.Visible = false);
                }
                else
                {
                    //有些闪屏。后面优化是不是加载时就全部加进去 
                    UCGridColSetting uCGridColSet = new UCGridColSetting();
                    uCGridColSet.Name = entity.ColName;
                    uCGridColSet.dataGridView = dataGridView;
                    uCGridColSet.BindData(entity);
                    uCGridColSet.OnSynchronizeUI += UCQuery_OnSynchronizeUI;

                    uCGridColSet.Visible = true;//这里是当前编辑的字段显示
                    uCGridColSet.TopLevel = false;
                    uCGridColSet.Dock = DockStyle.Fill;
                    panelConditionEdit.Controls.Add(uCGridColSet as Control);
                    //其它隐藏
                    panelConditionEdit.Controls.CastToList<Control>().Where(c => c.Name != entity.ColName.ToString()).ToList().ForEach(c => c.Visible = false);
                }

            }

        }


        /// <summary>
        /// 加载要编辑的列的UI
        /// </summary>
        private void LoadEditGridCol()
        {

            if (listView1.SelectedItems != null && listView1.SelectedItems.Count > 0)
            {
                for (int i = 0; i < listView1.SelectedItems.Count; i++)
                {
                    var entity = listView1.SelectedItems[i].Tag as ColDisplayController;
                    if (panelConditionEdit.Controls.ContainsKey(entity.ColName.ToString()))
                    {
                        var uCQuery = panelConditionEdit.Controls.CastToList<Control>().FirstOrDefault(c => c.Name == entity.ColName.ToString());
                        if (uCQuery != null)
                        {
                            uCQuery.Visible = true;
                        }
                        //其它隐藏
                        panelConditionEdit.Controls.CastToList<Control>().Where(c => c.Name != entity.ColName.ToString()).ToList().ForEach(c => c.Visible = false);
                    }
                    else
                    {
                        //有些闪屏。后面优化是不是加载时就全部加进去 
                        UCGridColSetting uCGridColSet = new UCGridColSetting();
                        uCGridColSet.Name = entity.ColName;
                        uCGridColSet.dataGridView = dataGridView;
                        uCGridColSet.OnSynchronizeUI += UCQuery_OnSynchronizeUI;
                        uCGridColSet.BindData(entity);
                        uCGridColSet.Visible = true;//这里是当前编辑的字段显示
                        uCGridColSet.TopLevel = false;
                        uCGridColSet.Dock = DockStyle.Fill;
                        panelConditionEdit.Controls.Add(uCGridColSet as Control);
                        //其它隐藏
                        panelConditionEdit.Controls.CastToList<Control>().Where(c => c.Name != entity.ColName.ToString()).ToList().ForEach(c => c.Visible = false);
                    }
                }
            }

        }


        private void contentMenu1_CheckAll(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
                item.Checked = true;
        }
        private void contentMenu1_CheckNo(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                item.Checked = false;
            }
        }
        private void contentMenu1_Inverse(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                if (item.Checked == true)
                    item.Checked = false;
                else
                    item.Checked = true;
            }

        }


        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            //获取listview1当前焦点  
            ListViewItem item = listView1.FocusedItem;
            listView1.Items.Remove(item);
            listView1.SelectedItems.Clear();
        }

        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
                item.Checked = true;
        }

        private void chkReverseSelection_CheckedChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                item.Checked = !item.Checked;
            }
        }


        /// <summary>
        /// 创建编辑的这个列的控制数据时 同步到UI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UCQuery_OnSynchronizeUI(object sender, object e)
        {
            if (sender is ColDisplayController target)
            {
                foreach (ListViewItem item in listView1.Items)
                {
                    if (item.Tag is ColDisplayController column)
                    {
                        if (column.ColName == target.ColName)
                        {
                            item.Tag = target;
                            item.Checked = target.Visible;
                            break;
                        }
                    }
                }
            }
        }

        #region 初始化使用的属性
        public Type gridviewType { get; set; }

        public Control ConfiguredGrid { get; set; }

        private NewSumDataGridView dataGridView { get; set; }

        public tb_UIGridSetting GridSetting { get; set; }


        #endregion


        private void btnInitCol_Click(object sender, EventArgs e)
        {
            UIBizSrvice.InitDataGridViewColumnDisplays(ColumnDisplays, dataGridView, gridviewType);
            LoadColumnDisplayList();
        }

        private void cmbGridViewList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbColsDisplayModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is KryptonComboBox kcmb)
            {
                if (kcmb.SelectedValue != null)
                {
                    if (dataGridView != null)
                    {
                        dataGridView.AutoSizeColumnsMode = (DataGridViewAutoSizeColumnsMode)GridSetting.ColumnsMode;
                        if (dataGridView.AutoSizeColumnsMode == DataGridViewAutoSizeColumnsMode.None)
                        {

                        }
                        else
                        {

                        }
                    }

                }

            }

        }

        private void listView1_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            //只有他有焦点人点时才生效。或全选 全不选
            if ((sender is ListView target && listView1.Focused) || (chkAll.Checked || chkReverseSelection.Checked))
            {
                if (e.Item.Tag is ColDisplayController column)
                {
                    column.Visible = e.Item.Checked;
                }
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            //保存设置到DB
            UIBizSrvice.SaveGridSettingData(CurMenuInfo, dataGridView, gridviewType);
        }
    }
}
