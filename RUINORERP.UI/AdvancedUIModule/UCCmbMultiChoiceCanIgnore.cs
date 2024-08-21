using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.AdvancedUIModule
{
    /// <summary>
    /// 带使用状态的多选，可以忽略。勾选生效
    /// </summary>
    public partial class UCCmbMultiChoiceCanIgnore : UserControl
    {
        public UCCmbMultiChoiceCanIgnore()
        {
            InitializeComponent();
            chkCanIgnore.CheckedChanged += chkCanIgnore_CheckedChanged;
           
        }
        ContextMenuStrip contentMenu1;
        private void chkCanIgnore_CheckedChanged(object sender, EventArgs e)
        {
            kryptonPanelQuery.Visible = chkCanIgnore.Checked;
        }

        private void UCCmbMultiChoiceCanIgnore_Load(object sender, EventArgs e)
        {
            chkCanIgnore.Checked=true;

            contentMenu1 = new ContextMenuStrip();
            contentMenu1.Items.Add("全选");
            contentMenu1.Items.Add("全不选");
            contentMenu1.Items.Add("反选");
            contentMenu1.Items[0].Click += new EventHandler(contentMenu1_CheckAll);
            contentMenu1.Items[1].Click += new EventHandler(contentMenu1_CheckNo);
            contentMenu1.Items[2].Click += new EventHandler(contentMenu1_Inverse);

            chkMulti.ContextMenuStrip = contentMenu1;
        }

        private void contentMenu1_CheckAll(object sender, EventArgs e)
        {
            foreach (ListViewItem item in chkMulti.Items)
                item.Checked = true;
        }
        private void contentMenu1_CheckNo(object sender, EventArgs e)
        {
            foreach (ListViewItem item in chkMulti.Items)
            {
                item.Checked = false;
            }
        }
        private void contentMenu1_Inverse(object sender, EventArgs e)
        {
            foreach (ListViewItem item in chkMulti.Items)
            {
                if (item.Checked == true)
                    item.Checked = false;
                else
                    item.Checked = true;
            }

        }

    }
}
