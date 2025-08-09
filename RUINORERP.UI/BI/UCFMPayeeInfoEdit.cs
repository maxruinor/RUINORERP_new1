using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Common;
using RUINORERP.UI.UCToolBar;
using RUINORERP.Model;
using Krypton.Toolkit;
using RUINORERP.UI.BaseForm;
using RUINORERP.Business.LogicaService;
using RUINORERP.Business;
using RUINORERP.UI.Common;
using RUINORERP.Global;
using RUINORERP.Global.EnumExt;
using RUINORERP.Common.Extensions;
using Netron.GraphLib;
using RUINOR.WinFormsUI.CustomPictureBox;
using SourceGrid.Cells.Models;
using static RUINOR.Runtime.InteropServices.APIs.APIsStructs;
using System.Diagnostics;
using RUINORERP.Global.Model;
using RUINORERP.Business.Security;
using RUINORERP.Business.Processor;
using SqlSugar;

namespace RUINORERP.UI.BI
{


    [MenuAttrAssemblyInfo("收款账号编辑", true, UIType.单表数据)]
    public partial class UCFMPayeeInfoEdit : BaseEditGeneric<tb_FM_PayeeInfo>
    {
        public UCFMPayeeInfoEdit()
        {
            InitializeComponent();
        }

        private tb_FM_PayeeInfo _EditEntity;

        public override void BindData(BaseEntity entity)
        {
            if (entity == null)
            {
                return;
            }
            _EditEntity = entity as tb_FM_PayeeInfo;

            DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID);
            DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v => v.CVName, cmbCustomerVendor_ID, c => c.IsVendor == true);
            DataBindingHelper.BindData4CmbByEnum<tb_FM_PayeeInfo>(entity, k => k.Account_type, typeof(AccountType), cmbAccount_type, false);
            DataBindingHelper.BindData4TextBox<tb_FM_PayeeInfo>(entity, t => t.Account_name, txtAccount_name, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_FM_PayeeInfo>(entity, t => t.Account_No, txtAccount_No, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_FM_PayeeInfo>(entity, t => t.BelongingBank, txtBelongingBank, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_FM_PayeeInfo>(entity, t => t.OpeningBank, txtOpeningBank, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_FM_PayeeInfo>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);
            //有默认值
            DataBindingHelper.BindData4RadioGroupTrueFalse<tb_FM_PayeeInfo>(entity, t => t.IsDefault, rdbIsDefaultYes, rdbIsDefaultNo);
            DataBindingHelper.BindData4RadioGroupTrueFalse<tb_FM_PayeeInfo>(entity, t => t.Is_enabled, rdbis_enabledYes, rdbis_enabledNo);
            //有默认值
            LoadImageData(_EditEntity.PaymentCodeImagePath);
            //后面这些依赖于控件绑定的数据源和字段。所以要在绑定后执行。
            if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
            {
                base.InitRequiredToControl(MainForm.Instance.AppContext.GetRequiredService<tb_FM_PayeeInfoValidator>(), kryptonPanel1.Controls);
                base.InitEditItemToControl(entity, kryptonPanel1.Controls);
                PicRowImage.Visible = true;

                //只能自己的
                if (AuthorizeController.GetOwnershipControl(MainForm.Instance.AppContext))
                {
                    _EditEntity.Employee_ID = MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID;
                    cmbEmployee_ID.Enabled = false;
                }

                //限制只看到自己的
            }

            if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
            {
                InitLoadSupplierData(_EditEntity);
            }

            entity.PropertyChanged += (sender, s2) =>
            {
                if (_EditEntity == null)
                {
                    return;
                }

                if (_EditEntity.Employee_ID.HasValue && _EditEntity.Employee_ID.Value > 0 && s2.PropertyName == entity.GetPropertyName<tb_FM_PayeeInfo>(c => c.Employee_ID))
                {
                    cmbCustomerVendor_ID.Visible = false;
                    lblCustomerVendor_ID.Visible = false;
                }
                if (_EditEntity.CustomerVendor_ID.HasValue && _EditEntity.CustomerVendor_ID.Value > 0 && s2.PropertyName == entity.GetPropertyName<tb_FM_PayeeInfo>(c => c.CustomerVendor_ID))
                {
                    cmbEmployee_ID.Visible = false;
                    lblEmployee_ID.Visible = false;
                }
            };

            //如果员工有值。则是来自于员工编辑UI。反之则是客户供应商编辑UI
            if (_EditEntity.Employee_ID.HasValue && _EditEntity.Employee_ID.Value > 0
                && !_EditEntity.CustomerVendor_ID.HasValue)
            {
                cmbCustomerVendor_ID.Visible = false;
                lblCustomerVendor_ID.Visible = false;
                cmbEmployee_ID.Enabled = false;
                //lblEmployee_ID.Visible = false;
            }
            if (_EditEntity.CustomerVendor_ID.HasValue && _EditEntity.CustomerVendor_ID.Value > 0
                && !_EditEntity.Employee_ID.HasValue)
            {
                cmbEmployee_ID.Visible = false;
                lblEmployee_ID.Visible = false;
                cmbCustomerVendor_ID.Enabled = false;
            }
            base.BindData(entity);

        }
        private void InitLoadSupplierData(tb_FM_PayeeInfo entity)
        {
            //创建表达式
            var lambdaSupplier = Expressionable.Create<tb_CustomerVendor>()
                            .And(t => t.IsVendor == true)
                            .AndIF(AuthorizeController.GetExclusiveLimitedAuth(MainForm.Instance.AppContext) && !MainForm.Instance.AppContext.IsSuperUser, t => t.Employee_ID == MainForm.Instance.AppContext.CurUserInfo.UserInfo.Employee_ID)//限制了销售只看到自己的客户
                            .ToExpression();//注意 这一句 不能少

            BaseProcessor baseProcessor = Startup.GetFromFacByName<BaseProcessor>(typeof(tb_CustomerVendor).Name + "Processor");
            QueryFilter queryFilterSupplier = baseProcessor.GetQueryFilter();
            queryFilterSupplier.FilterLimitExpressions.Add(lambdaSupplier);
            DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v => v.CVName, cmbCustomerVendor_ID, queryFilterSupplier.GetFilterExpression<tb_CustomerVendor>(), true);
            DataBindingHelper.InitFilterForControlByExp<tb_CustomerVendor>(entity, cmbCustomerVendor_ID, c => c.CVName, queryFilterSupplier);
        }
        /// <summary>
        /// 下载图片显示到控件中
        /// </summary>
        /// <param name="ImagePath"></param>
        private async Task LoadImageData(string ImagePath)
        {
            if (!string.IsNullOrWhiteSpace(ImagePath))
            {
                HttpWebService httpWebService = Startup.GetFromFac<HttpWebService>();
                try
                {
                    if (PicRowImage.RowImage == null)
                    {
                        PicRowImage.RowImage = new DataRowImage();
                    }
                    PicRowImage.RowImage.ImageFullName = ImagePath;
                    string ImageRealPath = string.Join("_", ImagePath.Split('_').Take(1));
                    byte[] img = await httpWebService.DownloadImgFileAsync(ImageRealPath);
                    PicRowImage.Image = UI.Common.ImageHelper.byteArrayToImage(img);
                    PicRowImage.RowImage.image = PicRowImage.Image;
                    PicRowImage.Visible = true;

                }
                catch (Exception ex)
                {
                    MainForm.Instance.uclog.AddLog(ex.Message, Global.UILogType.错误);
                }
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            bindingSourceEdit.CancelEdit();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (base.Validator())
            {
                if (PicRowImage.Image != null)
                {
                    //还要处理更新的情况。是不是命名类似于grid
                    SetValueToRowImage();
                }
                bindingSourceEdit.EndEdit();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }


        private void SetValueToRowImage()
        {

            PicRowImage.RowImage.image = PicRowImage.Image;
            PicRowImage.RowImage.ImageBytes = PicRowImage.Image.ToBytes();
            _EditEntity.RowImage = PicRowImage.RowImage;
            _EditEntity.PaymentCodeImagePath = _EditEntity.RowImage.ImageFullName;
        }

        private void UCFMPayeeInfoEdit_Load(object sender, EventArgs e)
        {

        }


    }
}
