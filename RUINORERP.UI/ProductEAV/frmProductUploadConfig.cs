using SMTAPI.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.ProductEAV
{
    public partial class frmProductUploadConfig : Form
    {
        public frmProductUploadConfig()
        {
            InitializeComponent();
        }

        private void btn产品库本地图片路径_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txt产品库本地图片路径.Text = folderBrowserDialog1.SelectedPath;

            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            //if (MultiUser.Instance.ProductLibraryImagesPath != null)
            //{
            MultiUser.Instance.ProductLibraryImagesPath = txt产品库本地图片路径.Text;
            MultiUser.Instance.Serialize(MultiUser.Instance);
            this.Close();
            //}
        }

        private void frmProductUploadConfig_Load(object sender, EventArgs e)
        {
            if (MultiUser.Instance.ProductLibraryImagesPath != null)
            {
                txt产品库本地图片路径.Text = MultiUser.Instance.ProductLibraryImagesPath;
            }
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
