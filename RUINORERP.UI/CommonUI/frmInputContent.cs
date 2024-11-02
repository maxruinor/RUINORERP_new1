using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.CommonUI
{
    public partial class frmInputContent : Krypton.Toolkit.KryptonForm
    {
        public frmInputContent()
        {
            InitializeComponent();
        }

        public string DefaultTitle { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        private void frmInputContent_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(DefaultTitle))
            {
                this.Text = DefaultTitle;
            }
            txtContent.Text = Content;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (Content.IsNullOrEmpty())
            {
                MessageBox.Show("请填写要解决的问题！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (Content.Length<3)
            {
                MessageBox.Show("请详细说明要解决的问题！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            Content = txtContent.Text;
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
