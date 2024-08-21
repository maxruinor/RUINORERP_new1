using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

namespace HLH.WinControl.Mycontrol.ForCustomizeGrid
{
    public partial class frmColumnsSets : Form
    {
        public frmColumnsSets()
        {
            InitializeComponent();
        }

        public SerializableDictionary<string, bool> QueryResult { get; set; }




        ContextMenuStrip contentMenu1;

        /// <summary>  
        ///   
        /// </summary>  
        /// <param name="yestable"></param>  
        /// <param name="notable"></param>  
        /// <param name="queryResult"></param>  
        public frmColumnsSets(Hashtable yestable, Hashtable notable, List<KeyValuePair<string, int>> QueryResult)
        {
            InitializeComponent();
        }



        private void frmColumnsSets_Load(object sender, EventArgs e)
        {

            //遍历hashtable 分别加载到两个listview  
            //IDictionaryEnumerator myEnumerator = YesTable.GetEnumerator();
            //while (myEnumerator.MoveNext())
            //{
            //    listView1.Items.Insert(0, new ListViewItem(myEnumerator.Value.ToString()));
            //}



            listView1.Columns.Add("显示列名");
            listView1.Columns[0].Width = -2; //-1 -2 
            foreach (KeyValuePair<string, bool> item in QueryResult)
            {
                //listView1.Items.Insert(0, new ListViewItem(item.Key.ToString()));
                ListViewItem lvi = new ListViewItem();
                lvi.Checked = item.Value;
                lvi.Name = item.Key;
                lvi.Text = item.Key;
                listView1.Items.Add(lvi);
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



            //contentMenu2 = new ContextMenuStrip();  

            //contentMenu2.Items.Add("全选");  
            //contentMenu2.Items.Add("全不选");  
            //contentMenu2.Items.Add("反选");  
            //contentMenu2.Items[0].Click += new EventHandler(contentMenu2_CheckAll);  
            //contentMenu2.Items[1].Click += new EventHandler(contentMenu2_CheckNo);  
            //contentMenu2.Items[2].Click += new EventHandler(contentMenu2_Inverse);  


            listView1.ContextMenuStrip = contentMenu1;

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

        private void contentMenu2_Inverse(object sender, EventArgs e)
        {

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
                    shou += item.Text + ",";
                }
            }
            shou = shou.TrimEnd(',');
            if (shou == "")
            {
                MessageBox.Show("不能隐藏所有列！", "提醒", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            for (int j = 0; j < QueryResult.Count; j++)
            {
                foreach (ListViewItem item in listView1.Items)
                {
                    if (item.Checked)
                    {
                        QueryResult[item.Text] = true;
                    }
                    else
                    {
                        QueryResult[item.Text] = false;
                    }
                }
            }


            DialogResult = DialogResult.OK;
        }






        //for (int j = 0; j < QQueryResult.dataGridView1.Columns.Count; j++)  
        //{  
        //    for (int i = 0; i < listView2.Items.Count; i++)  
        //    {  
        //        if (listView1.Items[i].Text == QQueryResult.dataGridView1.Columns[j].HeaderText)  
        //        {  
        //            QQueryResult.dataGridView1.Columns[j].Visible = true;  

        //        }  
        //    }  

        //  
        // this.Close();


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




    }
}

