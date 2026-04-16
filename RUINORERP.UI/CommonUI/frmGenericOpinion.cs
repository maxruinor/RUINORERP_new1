using NPOI.SS.Formula.Functions;
using RUINORERP.Business.BizMapperService;
using RUINORERP.Common.Helper;
using RUINORERP.Global;
using RUINORERP.Model;
using RUINORERP.Model.CommonModel;
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

        /// <summary>
        /// 结案凭证图片上传后的文件ID
        /// </summary>
        public long? UploadedFileId { get; private set; }

        /// <summary>
        /// 结案凭证图片上传后的存储路径
        /// </summary>
        public string UploadedStoragePath { get; private set; }

        #endregion

        #region 事件处理

        private async void btnOk_Click(object sender, EventArgs e)
        {
            if (!AllowEmptyOpinion && string.IsNullOrEmpty(OpinionText))
            {
                MessageBox.Show("请填写" + OpinionLabelText, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtOpinion.Focus();
                return;
            }

            if (ShowAttachment && picBoxAttachment.Image != null)
            {
                string currentHash = ImageHelper.GetImageHash(picBoxAttachment.Image);
                if (!currentHash.Equals(_originalImageHash))
                {
                    try
                    {
                        MainForm.Instance.PrintInfoLog("正在上传结案凭证图片...");
                        var fileService = Startup.GetFromFac<RUINORERP.UI.Network.Services.FileBusinessService>();

                        // 获取业务实体ID，如果是新单据可能为0，服务器端会处理关联
                        long businessId = (Entity as RUINORERP.Model.BaseEntity)?.PrimaryKeyID ?? 0;

                        // 将图片转换为二进制并上传
                        byte[] imageData = ImageHelper.ImageToByteArray(picBoxAttachment.Image);
                        var response = await fileService.UploadImageAsync(
                            Entity as RUINORERP.Model.BaseEntity,
                            $"CloseCase_{DateTime.Now:yyyyMMddHHmmss}.png",
                            imageData,
                            "CloseCaseImagePath" // 默认字段名，实际由父窗体通过返回的ID处理
                        );

                        if (response.IsSuccess && response.FileStorageInfos != null && response.FileStorageInfos.Count > 0)
                        {
                            var fileInfo = response.FileStorageInfos[0];
                            this.UploadedFileId = fileInfo.FileId;
                            this.UploadedStoragePath = fileInfo.StoragePath;
                            MainForm.Instance.PrintInfoLog("凭证图片上传成功。");
                        }
                        else
                        {
                            MessageBox.Show($"图片上传失败: {response.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return; // 阻断提交
                        }
                    }
                    catch (Exception ex)
                    {
                        MainForm.Instance.uclog.AddLog($"结案图片上传异常: {ex.Message}", Global.UILogType.错误);
                        MessageBox.Show($"上传图片时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
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
            Expression<Func<T, object>> opinionExpression = null)
        {
            Entity = entity;

            // 绑定单据编号
            if (billNoExpression != null)
            {
                DataBindingHelper.BindData4TextBox(entity, billNoExpression, txtBillNO, BindDataType4TextBox.Text, false);
                txtBillNO.ReadOnly = true;
            }

            CommBillData cbd = EntityMappingHelper.GetBillData<T>(entity);
            //ae.BillNo = cbd.BillNo;
            //ae.bizType = cbd.BizType;
            //ae.bizName = cbd.BizName;

            txtBillType.Text = cbd.BizName;
            // 绑定单据类型
            //if (billTypeExpression != null)
            //{
            //    DataBindingHelper.BindData4TextBox(entity, billTypeExpression, txtBillType, BindDataType4TextBox.Text, false);
            //    txtBillType.ReadOnly = true;
            //}

            // 绑定意见内容
            if (opinionExpression != null)
            {
                DataBindingHelper.BindData4TextBox(entity, opinionExpression, txtOpinion, BindDataType4TextBox.Text, false);
                txtOpinion.MaxLength = OpinionMaxLength;
            }

            // 设置错误提供器
            errorProviderForAllInput.DataSource = entity;
        }

        /// <summary>
        /// 窗体关闭时清理资源，防止内存泄漏
        /// </summary>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (picBoxAttachment.Image != null)
            {
                picBoxAttachment.Image.Dispose();
                picBoxAttachment.Image = null;
            }
        }

        #endregion
    }
}

