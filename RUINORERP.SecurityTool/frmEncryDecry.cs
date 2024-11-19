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




        #region �������룬UserMd5(string str1)
        protected string UserMd5(string str1)
        {
            string cl1 = str1;
            string pwd = "";
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            // ���ܺ���һ���ֽ����͵����� 
            byte[] s = md5.ComputeHash(Encoding.Unicode.GetBytes(cl1));
            // ͨ��ʹ��ѭ�������ֽ����͵�����ת��Ϊ�ַ��������ַ����ǳ����ַ���ʽ������ 
            for (int i = 0; i < s.Length; i++)
            {
                // ���õ����ַ���ʹ��ʮ���������͸�ʽ����ʽ����ַ���Сд����ĸ�����ʹ�ô�д��X�����ʽ����ַ��Ǵ�д�ַ� 
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