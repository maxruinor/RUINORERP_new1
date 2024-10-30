using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RUINORERP.Server.ToolsUI
{
    public partial class frmMessager : Form
    {
        public frmMessager()
        {
            InitializeComponent();
        }
        string message = string.Empty;

        public string Message { get => message; set => message = value; }
        public bool MustDisplay { get => mustDisplay; set => mustDisplay = value; }

        bool mustDisplay = false;
        private void btnOK_Click(object sender, EventArgs e)
        {
            Message = txtMessage.Text;
            MustDisplay = chkMustDisplay.Checked;
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
