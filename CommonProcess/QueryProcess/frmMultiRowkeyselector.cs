using HLH.Lib.Helper;
using HLH.Lib.List;
using HLH.Lib.Office.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonProcess.QueryProcess
{
    public partial class frmMultiRowkeyselector : Form
    {
        public frmMultiRowkeyselector()
        {
            InitializeComponent();
        }

        private void 粘贴来自excel的数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            //要把列复制  所以第一行认为是列
            if (string.IsNullOrEmpty(Clipboard.GetText().Trim()))
            {
                MessageBox.Show("剪切板为空！");
                return;
            }

            string copyText = System.Windows.Forms.Clipboard.GetText();

            DataTable DtTable = new DataTable();

            //string[] allRow = copyText.Trim().Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            string[] allRow = copyText.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            //复制数据时，建议不需要标题了，因为标题不规则。 
            //判断是否为好的数据 为一行中 每列的值不为空。 如果第一行就这样，则默认给出列名

            //为了处理列名。特别处理第一行
            #region
            string[] ColumnsContent = allRow[0].Split(new string[] { "\t" }, StringSplitOptions.None);
            bool firstRowIsValue = false;//默认第一行为列名。不是值
            //处理列名
            for (int c = 0; c < ColumnsContent.Length; c++)
            {
                //判断是否为好的数据 为一行中 每列的值不为空。 如果第一行就这样，则默认给出列名
                foreach (string item in ColumnsContent.ToArray())
                {
                    if (item.Trim().Length == 0)
                    {
                        firstRowIsValue = true;
                        break;
                    }
                }

                //第二个特殊 。一般列名不包括数字
                foreach (string item in ColumnsContent.ToArray())
                {
                    if (HLH.Lib.Helper.RegexProcessHelper.IsContainDdigital(item.ToString(), out int temp))
                    {
                        firstRowIsValue = true;
                        break;
                    }
                }
                DataColumn dc = new DataColumn();

                if (firstRowIsValue)
                {
                    dc.ColumnName = "列" + (c + 1).ToString();
                }
                else
                {
                    if (ColumnsContent[c].Trim().Length > 0)
                    {
                        dc.ColumnName = ColumnsContent[c];
                    }
                }
                DtTable.Columns.Add(dc);
            }
            #endregion
            int row = 0;
            if (firstRowIsValue)
            {
                row = 0;
            }
            else
            {
                row = 1;
            }
            for (int i = row; i < allRow.Length; i++)
            {
                //把每行的数据按单元格截取，放到一个string数组里，第二个参数是不返回空字符  这里要返回空。因为有些没有列名。这时需要程序给出默认值。才能匹配列。
                string[] content = allRow[i].Trim().Split(new string[] { "\t" }, StringSplitOptions.None);

                //新增行
                DataRowView dr = DtTable.DefaultView.AddNew();
                //复制的数据列大于等于当前表格列
                if (content.Length >= DtTable.Columns.Count)
                {
                    for (int j = 0; j < DtTable.Columns.Count; j++)
                    {
                        dr[j] = content[j];
                    }
                }
                //赋值的数据列小于当前表格列
                else if (content.Length < DtTable.Columns.Count)
                {
                    for (int j = 0; j < content.Length; j++)
                    {
                        dr[j] = content[j];
                    }
                }
                dr.EndEdit();
            }

            dataGridView1.DataSource = DtTable;
        }

        private void 清空数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
        }

        private void 处理当前列值ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<KeyValue<string, string>> processInputResult = new List<KeyValue<string, string>>();
            //进出处理值 。变更到DG中
            foreach (DataGridViewCell item in dataGridView1.SelectedCells)
            {
                if (item.Selected)
                {
                    KeyValue<string, string> kv = new KeyValue<string, string>();
                    kv.Key = item.Value;
                    processInputResult.Add(kv);
                }
            }

            StringProcess.frmTextProcesserTest frm = new StringProcess.frmTextProcesserTest();
            frm.ProcessInputResult = processInputResult;
            frm.OtherEvent += Frm_OtherEvent;
            frm.Show();
        }

        private void Frm_OtherEvent(Form frmPro, object Parameters)
        {
            StringProcess.frmTextProcesserTest frm = frmPro as StringProcess.frmTextProcesserTest;
            List<KeyValue<string, string>> processInputResult = new List<KeyValue<string, string>>();
            processInputResult = Parameters as List<KeyValue<string, string>>;
            //进出处理值 。变更到DG中
            foreach (DataGridViewCell item in dataGridView1.SelectedCells)
            {
                if (item.Selected)
                {
                    if (item.Selected)
                    {
                        item.Value = frm.ProcessData("", item.Value.ToString());
                    }

                }
            }
        }

        private void frmMultiRowkeyselector_Load(object sender, EventArgs e)
        {
            dataGridView1.AllowUserToDeleteRows = true;
            dataGridView1.AllowUserToAddRows = true;
        }

        public string MultiKeys { get; set; }

        private void btnok_Click(object sender, EventArgs e)
        {

            //可以将这个功能 变为通用控件， 结果格式 可以设置

            string mulitxt = string.Empty;
            //只取第一列，
            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {
                if (dr.Cells[0].Value != null)
                {
                    string keyTxt = dr.Cells[0].Value.ToString().Trim();
                    if (keyTxt.Length > 0)
                    {
                        mulitxt += "'" + keyTxt + "',";
                    }
                }
                
            }
            mulitxt = mulitxt.TrimEnd(',');
            MultiKeys = mulitxt;
            this.DialogResult = DialogResult.OK;
            this.Close();

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
