using NPOI.SS.Formula.Functions;
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
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.CommonUI
{


    /// <summary>
    /// 通用意见输入窗体 支持自定义标题、意见标签、是否显示附件功能等
    /// </summary>
    public partial class frmOpinion : frmBase
    {
        public frmOpinion()
        {
            InitializeComponent();
            // 设置默认值
             
            _opinionLabelText = "意见内容：";
            _attachmentLabelText = "附件图片：";
            _allowEmptyOpinion = false;
        }

        #region 可配置属性

        /// <summary>
        /// 窗体标题
        /// </summary>
        public string FormTitle
        {
            get => this.Text;
            set => this.Text = value;
        }

        /// <summary>
        /// 意见标签文本（默认："意见内容："）
        /// </summary>
        private string _opinionLabelText;
        public string OpinionLabelText
        {
            get => _opinionLabelText;
            set
            {
                _opinionLabelText = value;
                lblOpinion.Text = value;
            }
        }

        /// <summary>
        /// 附件标签文本（默认："附件图片："）
        /// </summary>
        private string _attachmentLabelText;
        public string AttachmentLabelText
        {
            get => _attachmentLabelText;
            set
            {
                _attachmentLabelText = value;
                lblAttachment.Text = value;
            }
        }

 
        /// <summary>
        /// 是否允许意见为空（默认：false）
        /// </summary>
        private bool _allowEmptyOpinion = false;
        public bool AllowEmptyOpinion
        {
            get => _allowEmptyOpinion;
            set => _allowEmptyOpinion = value;
        }

        /// <summary>
        /// 意见最大长度限制（默认：500）
        /// </summary>
        public int OpinionMaxLength { get; set; } = 500;

        #endregion

        #region 数据属性

    
        /// <summary>
        /// 附件图片
        /// </summary>
        public Image AttachmentImage { get; private set; }

        /// <summary>
        /// 原始图片哈希值（用于检测图片是否修改）
        /// </summary>
        private string _originalImageHash;

        #endregion


        private Image _CloseCaseImage;
        private string DefaultImageHash = string.Empty;

        private bool _ShowCloseCaseImage = false;
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!AllowEmptyOpinion && string.IsNullOrEmpty(OpinionText))
            {
                MessageBox.Show("请填写" + OpinionLabelText, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtOpinion.Focus();
                return;
            }

          

            this.DialogResult = DialogResult.OK;
            this.Close();
            /*
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
      
          */
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void frmApproval_Load(object sender, EventArgs e)
        {
            //DefaultImageHash = RUINORERP.Common.Helper.ImageHelper.GetImageHash(picBoxAttachment.Image);
            //picBoxAttachment.Visible = ShowCloseCaseImage;
            //lblAttachment.Visible = ShowCloseCaseImage;

           
        }
        private void pictureBoxAttachment_DragEnter(object sender, DragEventArgs e)
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

        private void pictureBoxAttachment_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files != null && files.Length > 0)
                {
                    string filePath = files[0];
                    if (IsValidImageFile(filePath))
                    {
                        picBoxAttachment.Image = RUINORERP.UI.Common.ImageHelper.GetImage(filePath, 800, 600);
                    }
                    else
                    {
                        MessageBox.Show("只能接受图片文件（.png/.jpg/.jpeg/.bmp）。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            DataBindingHelper.BindData4TextBox<ApprovalEntity>(entity, t => t.BillNo, billNoExpression, BindDataType4TextBox.Text, false);
            //这个只是显示给用户看。不会修改
            DataBindingHelper.BindData4TextBox<ApprovalEntity>(entity, t => t.bizName, txtBillType, BindDataType4TextBox.Text, false);
            txtBillType.ReadOnly = true;
            entity.ApprovalResults = true;
            DataBindingHelper.BindData4TextBox<ApprovalEntity>(entity, t => t.CloseCaseOpinions, txtOpinion, BindDataType4TextBox.Text, false);
            errorProviderForAllInput.DataSource = entity;
        }

        #region 辅助方法

        /// <summary>
        /// 验证文件是否为有效图片格式
        /// </summary>
        private bool IsValidImageFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return false;
            string ext = System.IO.Path.GetExtension(filePath).ToLower();
            return ext == ".png" || ext == ".jpg" || ext == ".jpeg" || ext == ".bmp";
        }

        /// <summary>
        /// 获取用户输入的意见文本
        /// </summary>
        public string OpinionText => txtOpinion.Text;

        

        #endregion
    }
}
