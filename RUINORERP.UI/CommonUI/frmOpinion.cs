using RUINORERP.Common.Helper;
using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.UI.BaseForm;
using RUINORERP.UI.Common;
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
    public partial class frmOpinion : frmBase
    {
        public frmOpinion()
        {
            InitializeComponent();
        }


        private Image _CloseCaseImage;
        private string DefaultImageHash = string.Empty;

        private bool _ShowCloseCaseImage = false;
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (_entity.CloseCaseOpinions.IsNullOrEmpty())
            {
                MessageBox.Show("请填写结案意见！");
                return;
            }
            string newImageHash = RUINORERP.Common.Helper.ImageHelper.GetImageHash(picBoxCloseImage.Image);
            if (!newImageHash.Equals(DefaultImageHash))
            {
                _CloseCaseImage = picBoxCloseImage.Image;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void frmApproval_Load(object sender, EventArgs e)
        {
            DefaultImageHash = RUINORERP.Common.Helper.ImageHelper.GetImageHash(picBoxCloseImage.Image);
            picBoxCloseImage.Visible = ShowCloseCaseImage;
            lblCloseImage.Visible = ShowCloseCaseImage;
        }
        private void pictureBox1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void pictureBox1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files != null && files.Length > 0)
                {
                    string filePath = files[0];
                    if (filePath.ToLower().EndsWith(".png") || filePath.ToLower().EndsWith(".jpg") || filePath.ToLower().EndsWith(".jpeg") || filePath.ToLower().EndsWith(".bmp"))
                    {
                        picBoxCloseImage.Image = RUINORERP.UI.Common.ImageHelper.GetImage(filePath, 800, 600);
                    }
                    else
                    {
                        MessageBox.Show("只能接受图片文件。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private ApprovalEntity _entity;

        //public string CloseCaseImagePath { get => _CloseCaseImagePath; set => _CloseCaseImagePath = value; }
        public Image CloseCaseImage { get => _CloseCaseImage; set => _CloseCaseImage = value; }
        public bool ShowCloseCaseImage { get => _ShowCloseCaseImage; set => _ShowCloseCaseImage = value; }

        public void BindData(ApprovalEntity entity)
        {
            _entity = entity;
            DataBindingHelper.BindData4TextBox<ApprovalEntity>(entity, t => t.BillNo, txtBillNO, BindDataType4TextBox.Text, false);
            //这个只是显示给用户看。不会修改
            DataBindingHelper.BindData4TextBox<ApprovalEntity>(entity, t => t.bizName, txtBillType, BindDataType4TextBox.Text, false);
            txtBillType.ReadOnly = true;
            entity.ApprovalResults = true;
            DataBindingHelper.BindData4TextBox<ApprovalEntity>(entity, t => t.CloseCaseOpinions, txtOpinion, BindDataType4TextBox.Text, false);
            errorProviderForAllInput.DataSource = entity;
        }


    }
}
