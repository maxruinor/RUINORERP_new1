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
    public partial class UCAdvYesOrNO : UserControl
    {
        public UCAdvYesOrNO()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 是否启用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chk_CheckedChanged(object sender, EventArgs e)
        {
            groupBox1.Visible = chk.Checked;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
