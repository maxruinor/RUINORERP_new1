using Krypton.Toolkit;
using RUINORERP.UI.UControls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Collections.Concurrent;
using System.Linq;
namespace RUINORERP.UI.ForCustomizeGrid
{
    public partial class frmColumnsSets : KryptonForm
    {
        public frmColumnsSets()
        {
            InitializeComponent();
        }

 

        public List<ColDisplayController> ColumnDisplays { get; set; } = new List<ColDisplayController>();

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
            //遍历hashtable 分别加载到两个listview  
            //IDictionaryEnumerator myEnumerator = YesTable.GetEnumerator();
            //while (myEnumerator.MoveNext())
            //{
            //    listView1.Items.Insert(0, new ListViewItem(myEnumerator.Value.ToString()));
            //}

            listView1.Columns.Add("显示列名");
            listView1.Columns[0].TextAlign = HorizontalAlignment.Center;
            listView1.Columns[0].Width = -2; //-1 -2 
            /*
            ColumnDisplays.Sort((left, right) =>
            {
                int x = 0;
                if (left.ColDisplayIndex > right.ColDisplayIndex)
                    x = 1;
                else if (left.ColDisplayIndex == right.ColDisplayIndex)
                    x = 0;
                else
                    x = -1;

                return x;
            });
            */
            //控制显示的顺序和表格中显示的一样 sort比方这个比较详细介绍 https://blog.csdn.net/qq_42672770/article/details/123344526
            /*
            ColumnDisplays.Sort((left, right) =>
            {
                int x = 0;
                if (left.ColDisplayIndex > right.ColDisplayIndex)
                    x = 1;
                else if (left.ColDisplayIndex == right.ColDisplayIndex)
                    x = 0;
                else
                    x = -1;

                return x;
            });
            */
            #region 

            //peopleList.Sort((left, right) =>
            //{
            //    //先按姓名排序，如果姓名相同再按年龄排序
            //    int x = left.Name.CompareTo(right.Name);
            //    if (x == 0)
            //    {
            //        if (left.Age > right.Age)
            //            x = 1;
            //        else if (left.Age == right.Age)
            //            x = 0;
            //        else
            //            x = -1;
            //    }
            //    return x;
            //});
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

            /*
            System.Drawing.Point ptScreen = new System.Drawing.Point(e.X, e.Y);
            System.Drawing.Point pt = listView1.PointToClient(ptScreen);
            ListViewItem item = listView1.GetItemAt(pt.X, pt.Y);

            int targetIndex = listView1.InsertionMark.NearestIndex(pt);
            if (targetIndex > -1)
            {
                System.Drawing.Rectangle itemBounds = listView1.GetItemRect(targetIndex);
                if (pt.X > itemBounds.Left + (itemBounds.Width / 2))
                {
                    listView1.InsertionMark.AppearsAfterItem = true;
                }
                else
                {
                    listView1.InsertionMark.AppearsAfterItem = false;
                }
            }
            listView1.InsertionMark.Index = targetIndex;
            //if (item != null)
            //    item.Checked = true;*/
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
            /*
            int targetIndex = listView1.InsertionMark.Index;
            if (targetIndex == -1)
            {
                return;
            }
            if (listView1.InsertionMark.AppearsAfterItem)
            {
                targetIndex++;
            }

            ListViewItem draggedItem = (ListViewItem)e.Data.GetData(typeof(ListViewItem));
            //Point ptScreen = new Point(e.X, e.Y);
            //Point pt = listView1.PointToClient(ptScreen);
            //ListViewItem targetItem = listView1.GetItemAt(pt.X, pt.Y);//拖动的项将放置于该项之前

            //if (null == targetItem || targetItem == draggedItem)
            //    return;

            // NeedForFix: 项实际已经交换，但是显示没有交换
            listView1.BeginUpdate();

            listView1.Items.Insert(targetIndex, (ListViewItem)draggedItem.Clone());
            listView1.Items.Remove(draggedItem);
            listView1.EndUpdate();*/
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
    }
}

