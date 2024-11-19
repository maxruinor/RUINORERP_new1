using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography;


namespace SecurityCore
{
    public partial class frmEncryDecry : Form
    {
        public frmEncryDecry()
        {
            InitializeComponent();
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
             txtNewData.Text = DataProtection.Encrypt(txtOldData.Text.Trim(), DataProtection.Store.Machine);

        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {

         txtOldData.Text = DataProtection.Decrypt(txtNewData.Text.Trim(), DataProtection.Store.Machine);
            //int i = txtNewData.Text.Trim().Length;
        }




        #region 加密密码，UserMd5(string str1)
        protected string UserMd5(string str1)
        {
            string cl1 = str1;
            string pwd = "";
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            // 加密后是一个字节类型的数组 
            byte[] s = md5.ComputeHash(Encoding.Unicode.GetBytes(cl1));
            // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得 
            for (int i = 0; i < s.Length; i++)
            {
                // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 
                pwd = pwd + s[i].ToString("x");
            }
            return pwd;
        }
        #endregion

        private void btnCreateCode_Click(object sender, EventArgs e)
        {
            CreateRegisterCode code = new CreateRegisterCode();
            this.textBox1.Text = code.transform(txtOldData.Text.Trim(), txtNewData.Text);
        }

      


    }
}