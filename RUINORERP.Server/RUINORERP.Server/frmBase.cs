using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.Server
{
    public partial class frmBase : Form
    {
        public frmBase()
        {
            InitializeComponent();
        }

        //因为注册时都将窗体注册成单例了，所以窗体关闭时需要隐藏，而不是关闭
        private void frmBase_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
    }
}
