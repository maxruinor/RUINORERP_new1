using Krypton.Toolkit;
using RUINORERP.UI.UControls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Collections.Concurrent;
using System.Linq;
using RUINORERP.UI.BaseForm;
using RUINORERP.UI.Common;
namespace RUINORERP.UI.ForCustomizeGrid
{
    public partial class frmColumnsSets : KryptonForm
    {
        public frmColumnsSets()
        {
            InitializeComponent();
        }

        //恢复默认列的事件
        public delegate void InitializeDefaultColumnDelegate(List<ColDisplayController> ColumnDisplays);
        public event InitializeDefaultColumnDelegate InitializeDefaultColumn;

        public List<ColDisplayController> ColumnDisplays { get; set; } = new List<ColDisplayController>();

        /// <summary>
        /// 初始化时的列配置，用于恢复默认列配置
        /// </summary>
        public List<ColDisplayController> InitColumnDisplays { get; set; } = new List<ColDisplayController>();

        public ColDisplayController[] oldColumnDisplays;

        ContextMenuStrip contentMenu1;

        private void frmColumnsSets_Load(object sender, EventArgs e)
        {

            listView1.AllowDrop = true;
            listView1.ItemDrag += ListView1_ItemDrag;
            listView1.DragEnter += listView1_DragEnter;
            listView1.DragOver += listView1_DragOver;
            listView1.DragDrop += listView1_DragDrop;
            listView1.DragLeave += listView1_DragLeave;


            listView1.Columns.Add("显示列名");
            listView1.Columns[0].TextAlign = HorizontalAlignment.Center;
            listView1.Columns[0].Width = -2; //-1 -2 

            #region 


            #endregion
            oldColumnDisplays = new ColDisplayController[ColumnDisplays.Count];
            ColumnDisplays.CopyTo(oldColumnDisplays);
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



            contentMenu1 = new ContextMenuStrip();
            contentMenu1.Items.Add("全选");
            contentMenu1.Items.Add("全不选");
            contentMenu1.Items.Add("反选");
            contentMenu1.Items[0].Click += new EventHandler(contentMenu1_CheckAll);
            contentMenu1.Items[1].Click += new EventHandler(contentMenu1_CheckNo);
            contentMenu1.Items[2].Click += new EventHandler(contentMenu1_Inverse);

            listView1.ContextMenuStrip = contentMenu1;

        }


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
        private class ListViewIndexComparer : System.Collections.IComparer
        {
            public int Compare(object x, object y)
            {
                return ((ListViewItem)x).Index - ((ListViewItem)y).Index;
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {


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

        private void btnOK_Click(object sender, EventArgs e)
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
                            cdc.Visible = item.Checked;
                            cdc.ColDisplayIndex = sortindex;
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







        private void btn_cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void listView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            ListViewItem[] items = new ListViewItem[((ListView)(sender)).SelectedItems.Count];
            int i = 0;
            foreach (ListViewItem item in ((ListView)(sender)).SelectedItems)
            {
                items[i] = item;
                i++;
            }
            ((ListView)(sender)).DoDragDrop(new DataObject("System.Windows.Forms.ListViewItem()", items), DragDropEffects.Copy);
        }

        private void listView2_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("System.Windows.Forms.ListViewItem()"))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
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


        public NewSumDataGridView DataGridViewSetTarget
        { get; set; }

        public Type DataSourceType { get; set; }

        /// <summary>
        /// 系统硬编码不可见列集合
        /// </summary>
        public HashSet<string> InvisibleCols { get; set; } = new HashSet<string>();


        /// <summary>
        /// 恢复默认列配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRestoreDefaultConfig_Click(object sender, EventArgs e)
        {
            try
            {
                // 检查是否有初始化的列配置
                if (InitColumnDisplays == null || InitColumnDisplays.Count == 0)
                {
                    // 同时也触发外部事件（如果有的话），保持向后兼容
                    if (InitializeDefaultColumn != null && InitColumnDisplays.Count == 0)
                    {
                        InitializeDefaultColumn(InitColumnDisplays);
                    }
                }

                // 清空当前ListView
                listView1.Items.Clear();

                // 重新加载ListView内容 - 使用 InitColumnDisplays 作为默认配置
                foreach (ColDisplayController keyValue in InitColumnDisplays)
                {
                    if (!keyValue.Disable)
                    {
                        ListViewItem lvi = new ListViewItem();
                        lvi.Checked = keyValue.Visible;
                        lvi.Name = keyValue.ColName;
                        lvi.Tag = keyValue;
                        lvi.Text = keyValue.ColDisplayText;
                        lvi.ImageKey = keyValue.ColDisplayIndex.ToString();
                        listView1.Items.Add(lvi);
                    }
                }

          
 
            }
            catch (Exception ex)
            {
                MessageBox.Show($"恢复默认列配置失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

