using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace CommonProcess.StringProcess
{
    public partial class UCSetSpecFieldValue : UCMyBase
    {
        public UCSetSpecFieldValue()
        {
            InitializeComponent();
        }

        private void UCFieldCopy_Load(object sender, EventArgs e)
        {

        }

        public void LoadProcessColumns(DataGridView dataGridView1)
        {
            DataTable dt = new DataTable();
            dt = dataGridView1.DataSource as DataTable;

            List<string> tclist = new List<string>();

            foreach (DataColumn field in dt.Columns)
            {
                tclist.Add(field.Caption);
            }
            //加载表的字段
            cmbtarget.Items.Clear();
            HLH.Lib.Helper.DropDownListHelper.InitDropList(tclist, cmbtarget, true);

        }
    }
}
