using HLH.Lib.Helper;
using Krypton.Toolkit;
using RUINORERP.UI.UControls;
using RUINORERP.UI.UCSourceGrid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace RUINORERP.UI.ToolForm
{

    /// <summary>
    /// https://www.cnblogs.com/unicornsir/p/16186537.html
    /// 因为一个特性不一样。暂时和gridview通用的不一样
    /// </summary>
    public partial class frmShowColumns : KryptonForm
    {
        public frmShowColumns()
        {
            InitializeComponent();
            listView1.AllowDrop = true;
            listView1.ItemDrag += ListView1_ItemDrag;
            listView1.DragEnter += listView1_DragEnter;
            listView1.DragOver += listView1_DragOver;
            listView1.DragDrop += listView1_DragDrop;
            listView1.DragLeave += listView1_DragLeave;
        }
        private SerializableDictionary<string, bool> _ConfigItems = new SerializableDictionary<string, bool>();
        public SerializableDictionary<string, bool> ConfigItems { get => _ConfigItems; set => _ConfigItems = value; }

        private List<KeyValuePair<string, SourceGridDefineColumnItem>> items = new List<KeyValuePair<string, SourceGridDefineColumnItem>>();
        public List<KeyValuePair<string, SourceGridDefineColumnItem>> Items { get => items; set => items = value; }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            //如果全不选中。不可以
            int total = 0;
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                if (listView1.Items[i].Checked)
                {
                    total++;
                }
            }

            if (total == 0)
            {
                MessageBox.Show("必须要有选择一个！");
                return;
            }
            //if (chkListBox.GetItemText(chkListBox.Items) == "你得到的值")
            //{
            //    chkListBox.SetItemChecked(i, true);
            //}

            for (int i = 0; i < listView1.Items.Count; i++)
            {
                if (listView1.Items[i].Checked)
                {
                    KeyValuePair<string, SourceGridDefineColumnItem> kvitem = Items.Find(kv => kv.Key == listView1.Items[i].Text);
                    kvitem.Value.Visible = true;

                }
                else
                {
                    KeyValuePair<string, SourceGridDefineColumnItem> kvitem = Items.Find(kv => kv.Key == listView1.Items[i].Text);
                    kvitem.Value.Visible = false;
                }

                //上面也可以优化为一行
                //同时也变更配置中的值
                ConfigItems[listView1.Items[i].ToString()] = listView1.Items[i].Checked;
            }
            //保存
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void frmShowColumns_Load(object sender, EventArgs e)
        {

            listView1.Items.Clear();
            listView1.Columns.Clear();
            


            listView1.Columns.Add("显示列名");
            listView1.Columns[0].TextAlign = HorizontalAlignment.Center;
            listView1.Columns[0].Width = -2; //-1 -2 
            foreach (var item in Items)
            {

                ListViewItem lvi = new ListViewItem();
                lvi.Checked = item.Value.Visible;
                lvi.Name = item.Key;
                lvi.Tag = item;
                lvi.Text = item.Key; ;
                //用这个来保存
                lvi.ImageKey = item.Value.ColIndex.ToString();
                listView1.Items.Add(lvi);

                //listView1.Items[i].Checked(listView1.Items.Count - 1, item.Value.Visible); //true改为false为没有选中。
            }
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
        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                listView1.Items[i].Checked = true;
            }
        }

        private void chkReverseSelection_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                listView1.Items[i].Checked = false;
            }
        }


        private string xmlFileName = string.Empty;

        /// <summary>
        ///用来保存配置自定义列
        /// </summary>
        [Browsable(false)]
        public string XmlFileName
        {
            get;
            set;
        }


    }
}
