using HLH.Lib.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SecurityCore
{
    public partial class frmCreateKey : Form
    {
        public frmCreateKey()
        {
            InitializeComponent();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            CreateRegisterCode code = new CreateRegisterCode();
            txtKey.Text = code.transform(txtServerID.Text.Trim(), txtClientID.Text.Trim()+txtClient.Text.Trim());
        }
    }
}