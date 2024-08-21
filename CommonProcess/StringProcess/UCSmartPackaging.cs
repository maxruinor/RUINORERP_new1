using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace CommonProcess.StringProcess
{
    public partial class UCSmartPackaging : UCMyBase
    {
        public UCSmartPackaging()
        {
            InitializeComponent();
        }

        private void rdb手工指定_CheckedChanged(object sender, EventArgs e)
        {
            txtPackagingQty.Visible = rdb手工指定.Checked;
            isSmart = !rdb手工指定.Checked;
        }

        private void rdb智能分析_CheckedChanged(object sender, EventArgs e)
        {
            lblDescForSmart.Visible = rdb智能分析.Checked;
            isSmart = rdb智能分析.Checked;
        }


        /// <summary>
        /// 如果是智能分析则为真
        /// </summary>
        public bool isSmart { get; set; }


    }
}
