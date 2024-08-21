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
    public partial class UCHtmltagProcess : UCMyBase, IUCBase
    {
        public UCHtmltagProcess()
        {
            InitializeComponent();
        }


        [Browsable(true), Description("引发外部事件")]
        public event OtherHandler OtherEvent;


        public void SaveDataFromUI(UCBasePara aa)
        {
            UCHtmltagProcessPara para = new UCHtmltagProcessPara();
            para = aa as UCHtmltagProcessPara;
            para.ProcessForHtmlTags.Clear();
            foreach (ListViewItem lv in lvForHtml.Items)
            {
                if (lv != null)
                {
                    if (lv.Checked)
                    {
                        para.ProcessForHtmlTags.Add(lv.Text);
                    }
                }

            }
        }

        public void LoadDataToUI(UCBasePara aa)
        {
            UCHtmltagProcessPara para = new UCHtmltagProcessPara();
            para = aa as UCHtmltagProcessPara;
            foreach (string kv in para.ProcessForHtmlTags)
            {
                foreach (ListViewItem item in lvForHtml.Items)
                {
                    if (item.Text == kv)
                    {
                        item.Checked = true;
                    }
                }
            }

        }

        private void btnNoSelectAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvForHtml.Items)
            {
                item.Checked = false;
            }
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvForHtml.Items)
            {
                item.Checked = true;
            }
        }

        private void UCHtmltagProcess_Load(object sender, EventArgs e)
        {
           HLH.Lib.Helper.HtmlTagProcess.LoadHtmlTag(lvForHtml);


        }
    }
}
