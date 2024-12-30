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
    public partial class frmEncryptionDecryption : Form
    {
        public frmEncryptionDecryption()
        {
            InitializeComponent();
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            if (txtPassword.Text.Length > 0)
            {
                txtPassword.Text = HLH.Lib.Security.AesEncryptionNew.EncryptString(txtOldData.Text, txtKey.Text);
            }
            else
            {
                txtNewData.Text = HLH.Lib.Security.EncryptionHelper.AesEncrypt(txtOldData.Text, txtKey.Text);
            }
            
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            if (txtPassword.Text.Length > 0)
            {
                txtPassword.Text = HLH.Lib.Security.AesEncryptionNew.EncryptString(txtOldData.Text, txtKey.Text);
            }
            else
            {
                txtNewData.Text = HLH.Lib.Security.EncryptionHelper.AesDecrypt(txtOldData.Text, txtKey.Text);
            }
        }

        private void btnGenerateAesKey256_Click(object sender, EventArgs e)
        {
            txtKey.Text = BitConverter.ToString(HLH.Lib.Security.AesKeyGenerator.GenerateAesKey256(txtPassword.Text));
        }
    }
}
