using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonProcess.StringProcess
{
    public partial class UCJson路径提取Find : UCMyBase, IUCBase
    {
        public UCJson路径提取Find()
        {
            InitializeComponent();
        }

        private void btnSetJsonPath_Click(object sender, EventArgs e)
        {
            frmJsonAnalyzer frm = new frmJsonAnalyzer();
            string rs = string.Empty;
            frm.txtKey.Text = txtJsonPickUPPath.Text;
            frm.jsonTextInput = rs;
            frm.richTextBoxinput.Text = rs;
            if (frm.ShowDialog() == DialogResult.Yes)
            {
                txtJsonPickUPPath.Text = frm.jsonPickPath;
            }
        }


        public  void SaveDataFromUI(UCBasePara bb)
        {
            UCJson路径提取Para aa = new UCJson路径提取Para();
            aa = bb as UCJson路径提取Para;
           // this.GUID = aa.GUID;
            //数组分割提取处理 aa = new 数组分割提取处理();
            aa.isJson格式化 = chkisJson格式化.Checked;
            aa.jsonPath = txtJsonPickUPPath.Text;
            //return aa;
        }

        public  void LoadDataToUI(UCBasePara bb)
        {
            UCJson路径提取Para aa = new UCJson路径提取Para();
            aa = bb as UCJson路径提取Para;
            //this.GUID = aa.GUID;
            chkisJson格式化.Checked = aa.isJson格式化;
            txtJsonPickUPPath.Text = aa.jsonPath;
        }

    }
}
