using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

namespace SHControls.OutlookGrid
{
    /// <summary>
    ///  数据分析控件,
    /// </summary>
    public partial class MyOutLookGrid : UserControl
    {


        public MyOutLookGrid()
        {
            InitializeComponent();

        }

        bool skinsFlag = false;
        private void toolStripCancelGroup_Click(object sender, EventArgs e)
        {
            if (!skinsFlag)
            {
                outlookGrid1.SkinsChange(skinsFlag);
                skinsFlag = true;
            }
            else
            {
                outlookGrid1.SkinsChange(skinsFlag);
                skinsFlag = false;
            }

        }

        private void 展开群组ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.outlookGrid1.ExpandAll();
        }

        private void 收拢群组ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.outlookGrid1.CollapseAll();
        }

        private void 取消群组ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.outlookGrid1.ClearGroups();
        }



        private void toolStripbtnManagerView_Click(object sender, EventArgs e)
        {
            this.listBox1.Items.Clear();
            this.listBox2.Items.Clear();

            if (outlookGrid1.SaveDataTable != null)
            {
                foreach (DataColumn Tdc in (outlookGrid1.SaveDataTable.Columns))
                {
                    int list1 = 0;
                    int list2 = 0;
                    foreach (DataGridViewColumn Ddc in outlookGrid1.Columns)
                    {
                        if (Tdc.ColumnName == Ddc.Name)
                        {
                            list1 = 1;
                            foreach (string Sum in outlookGrid1.SummaryColumns)
                            {
                                if (Tdc.ColumnName == Sum.Substring(0, Sum.IndexOf(",")))
                                {
                                    list1 = 0;
                                }

                            }
                        }



                        foreach (string Sum in outlookGrid1.SummaryColumns)
                        {
                            if (Tdc.ColumnName == Sum.Substring(0, Sum.IndexOf(",")))
                            {
                                list2 = 2;
                            }

                        }



                        if (Tdc.ColumnName == Ddc.Name)
                        {
                            list2 = 1;

                        }




                    }


                    if (list1 == 1)
                    {

                        this.listBox1.Items.Add(Tdc.ColumnName);
                    }


                    if (list2 == 0)
                    {
                        this.listBox2.Items.Add(Tdc.ColumnName);
                    }



                }

                this.panel1.Visible = true;
                panel1.BringToFront();
            }
        }



        private void btnYes_Click(object sender, EventArgs e)
        {

            outlookGrid1.MyDataBind(outlookGrid1.SaveDataTable);

            for (int i = 0; i < listBox2.Items.Count; i++)
            {

                outlookGrid1.Columns.Remove(outlookGrid1.Columns[listBox2.Items[i].ToString()]);

            }


            this.panel1.Visible = false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                listBox2.Items.Add(listBox1.SelectedItem.ToString());
                listBox1.Items.Remove(listBox1.SelectedItem.ToString());
            }
        }

        private void btnReMnve_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem != null)
            {
                listBox1.Items.Add(listBox2.SelectedItem.ToString());
                listBox2.Items.Remove(listBox2.SelectedItem.ToString());
            }
        }

        private void btnAgainSet_Click(object sender, EventArgs e)
        {
            this.listBox1.Items.Clear();
            foreach (DataGridViewColumn dc in outlookGrid1.Columns)
            {
                this.listBox1.Items.Add(dc.Name);
            }
            listBox2.Items.Clear();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.panel1.Visible = false;
        }

        private void toolStripbtnSort_Click(object sender, EventArgs e)
        {
            listBoxPX.Items.Clear();
            panel2.Visible = true;
            //BringToFront()     '最前端显示 
            //SendToBack()         '显示在最底层 
            panel2.BringToFront();

            foreach (DataGridViewColumn Ddc in outlookGrid1.Columns)
            {
                listBoxPX.Items.Add(Ddc.Name);
            }

        }

        private void btnSortOK_Click(object sender, EventArgs e)
        {

            if (listBoxPX.SelectedItem != null)
            {
                foreach (DataGridViewColumn Ddc in outlookGrid1.Columns)
                {
                    if (Ddc.Name == listBoxPX.SelectedItem.ToString())
                    {
                        if (radioButton1.Checked)
                        {
                            outlookGrid1.Sort(Ddc, ListSortDirection.Ascending);
                        }
                        else
                        {
                            outlookGrid1.Sort(Ddc, ListSortDirection.Descending);
                        }
                    }

                }

            }
            panel2.Visible = false;
        }

        private void btnSortNO_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
        }

        // 数据量不能太大,速度与性能,自定义查询,导出EXCEL 排序,统计图表,列视图
    }
}
