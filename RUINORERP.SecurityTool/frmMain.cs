using SecurityCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.SecurityTool
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btn加密解密_Click(object sender, EventArgs e)
        {
            frmEncryDecry encryDecry = new frmEncryDecry();
            encryDecry.Show();
        }

        private void btn注册码_Click(object sender, EventArgs e)
        {
            frmCreateKey frmCreateKey = new frmCreateKey();
            frmCreateKey.Show();
        }

        private void btnEnDE_Click(object sender, EventArgs e)
        {
            frmEncryptionDecryption frmCreateKey = new frmEncryptionDecryption();
            frmCreateKey.Show();
        }

        private void 注册码生成器ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmRUINORERPSecurity frmCreateKey = new frmRUINORERPSecurity();
            frmCreateKey.Show();
        }
    }
}
