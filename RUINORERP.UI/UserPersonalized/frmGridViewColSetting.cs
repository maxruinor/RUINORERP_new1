using HLH.WinControl.MyTypeConverter;
using Krypton.Toolkit;
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

namespace RUINORERP.UI.UserPersonalized
{
    /// <summary>
    /// QueryConditionSettings  查询条件设置，可以设置显示行数。条件排序，条件的默认值。条件显示情况
    /// </summary>
    public partial class frmGridViewColSetting : KryptonForm
    {
        public frmGridViewColSetting()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
        }

        public List<ColumnDisplayController> ColumnDisplays { get; set; } = new List<ColumnDisplayController>();

        public ColumnDisplayController[] oldColumnDisplays;


        ContextMenuStrip contentMenu1;



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
                if (item.Tag is ColumnDisplayController columnDisplays)
                {
                    if (columnDisplays != null)
                    {
                        ColumnDisplayController cdc = ColumnDisplays.Where(c => c.ColName == columnDisplays.ColName).FirstOrDefault();
                        cdc.Visible = item.Checked;
                        cdc.ColDisplayIndex = sortindex;
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
        MenuPersonalization mp = new MenuPersonalization();
        private void frmMenuPersonalization_Load(object sender, EventArgs e)
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
   
            oldColumnDisplays = new ColumnDisplayController[ColumnDisplays.Count];
            ColumnDisplays.CopyTo(oldColumnDisplays);
            foreach (ColumnDisplayController keyValue in ColumnDisplays)
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
    }
}
