using Krypton.Toolkit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.UCSourceGrid
{
    public partial class RichTextForm : KryptonForm
    {

        private string _InputValue = string.Empty;
   
        public string InputValue { get => _InputValue; set => _InputValue = value; }
       

        public RichTextForm()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            InputValue = kryptonRichTextBox1.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void RichTextForm_Load(object sender, EventArgs e)
        {
            // kryptonNavigator1.Button.CloseButtonDisplay = ButtonDisplay.Hide;
            kryptonRichTextBox1.Text = InputValue;
        }
    }
}
