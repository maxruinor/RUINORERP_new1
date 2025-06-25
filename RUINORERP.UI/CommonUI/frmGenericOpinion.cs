using NPOI.SS.Formula.Functions;
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
using RUINORERP.Common.Helper;
using RUINORERP.Global;
using RUINORERP.Model;
using ImageHelper = RUINORERP.Common.Helper.ImageHelper;



namespace RUINORERP.UI.CommonUI
{
    /// <summary>
    /// 通用意见输入窗体
    /// 支持自定义标题、意见标签、是否显示附件功能等
    /// </summary>
    public partial class frmGenericOpinion<T> : frmBase
    {
        #region 构造函数和初始化

        public frmGenericOpinion()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
            InitializeDefaultSettings();
        }

        private void InitializeDefaultSettings()
        {
            // 设置默认值
            _showAttachment = true;
            _opinionLabelText = "意见内容：";
            _attachmentLabelText = "附件图片：";
            _allowEmptyOpinion = false;
        }

        #endregion

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
        /// 是否显示附件区域（默认：true）
        /// </summary>
        private bool _showAttachment = true;
        public bool ShowAttachment
        {
            get => _showAttachment;
            set
            {
                _showAttachment = value;
                panelAttachment.Visible = value;
                if (!value)
                {
                    // 调整意见文本框高度
                    txtOpinion.Height += panelAttachment.Height;
                    txtOpinion.Dock = DockStyle.Fill;
                }
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

        private T _entity;
        /// <summary>
        /// 绑定的实体对象（泛型支持）
        /// </summary>
        public T Entity
        {
            get => _entity;
            set => _entity = value;
        }

        /// <summary>
        /// 附件图片
        /// </summary>
        public Image AttachmentImage { get; private set; }

        /// <summary>
        /// 原始图片哈希值（用于检测图片是否修改）
        /// </summary>
        private string _originalImageHash;

        #endregion

        #region 事件处理

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!AllowEmptyOpinion && string.IsNullOrEmpty(OpinionText))
            {
                MessageBox.Show("请填写" + OpinionLabelText, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtOpinion.Focus();
                return;
            }

            if (ShowAttachment)
            {
                string newImageHash = ImageHelper.GetImageHash(picBoxAttachment.Image);
                if (!newImageHash.Equals(_originalImageHash))
                {
                    AttachmentImage = picBoxAttachment.Image;
                }
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void frmGenericOpinion_Load(object sender, EventArgs e)
        {
            if (ShowAttachment)
            {
                _originalImageHash = ImageHelper.GetImageHash(picBoxAttachment.Image);
            }
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
                        picBoxAttachment.Image = ImageHelper.GetImage(filePath, 800, 600);
                    }
                    else
                    {
                        MessageBox.Show("只能接受图片文件（.png/.jpg/.jpeg/.bmp）。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        #endregion

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

        /// <summary>
        /// 绑定数据到窗体控件
        /// </summary>
        public void BindData(T entity,
            Expression<Func<T, object>> billNoExpression = null,
            Expression<Func<T, object>> billTypeExpression = null,
            Expression<Func<T, object>> opinionExpression = null)
        {
            Entity = entity;

            // 绑定单据编号
            if (billNoExpression != null)
            {
                DataBindingHelper.BindData4TextBox(entity, billNoExpression, txtBillNO, BindDataType4TextBox.Text, false);
                txtBillNO.ReadOnly = true;
            }

            // 绑定单据类型
            if (billTypeExpression != null)
            {
                DataBindingHelper.BindData4TextBox(entity, billTypeExpression, txtBillType, BindDataType4TextBox.Text, false);
                txtBillType.ReadOnly = true;
            }

            // 绑定意见内容
            if (opinionExpression != null)
            {
                DataBindingHelper.BindData4TextBox(entity, opinionExpression, txtOpinion, BindDataType4TextBox.Text, false);
                txtOpinion.MaxLength = OpinionMaxLength;
            }

            // 设置错误提供器
            errorProviderForAllInput.DataSource = entity;
        }

        #endregion
    }
}
 
