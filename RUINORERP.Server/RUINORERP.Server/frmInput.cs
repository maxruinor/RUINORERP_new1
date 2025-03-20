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
    public partial class frmInput : Form
    {
        public frmInput()
        {
            InitializeComponent();
            
        }

        private string _InputContent = string.Empty;

        public string InputContent { get => _InputContent; set => _InputContent = value; }

        private void button1_Click(object sender, EventArgs e)
        {
            InputContent = txtInputContent.Text;
            this.DialogResult=DialogResult.OK;
        }
    }
}
