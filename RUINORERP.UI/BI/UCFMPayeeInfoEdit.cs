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
            // 添加下拉框选中事件处理程序
            cmbEmployee_ID.SelectedIndexChanged += CmbEmployee_ID_SelectedIndexChanged;
            cmbCustomerVendor_ID.SelectedIndexChanged += CmbCustomerVendor_ID_SelectedIndexChanged;
        }

        private tb_FM_PayeeInfo _EditEntity;

        public override void BindData(BaseEntity entity)
        {
            if (entity == null)
            {
                return;
            }
            _EditEntity = entity as tb_FM_PayeeInfo;

            DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID, c => c.Is_enabled == true);
            DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v => v.CVName, cmbCustomerVendor_ID, c => c.IsVendor == true);
            //DataBindingHelper.BindData4CmbByEnum<tb_FM_PayeeInfo>(entity, k => k.Account_type, typeof(AccountType), cmbAccount_type, false);
            DataBindingHelper.BindData4CmbByEnum<tb_FM_PayeeInfo>(entity, k => k.Account_type, typeof(AccountType), cmbAccount_type, true);
            DataBindingHelper.BindData4TextBox<tb_FM_PayeeInfo>(entity, t => t.Account_name, txtAccount_name, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_FM_PayeeInfo>(entity, t => t.Account_No, txtAccount_No, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_FM_PayeeInfo>(entity, t => t.BelongingBank, txtBelongingBank, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_FM_PayeeInfo>(entity, t => t.OpeningBank, txtOpeningBank, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_FM_PayeeInfo>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_FM_PayeeInfo>(entity, t => t.Details, txtDetails, BindDataType4TextBox.Text, false);
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

            // 预先获取需要的属性名称，避免重复计算
            string detailsPropertyName = entity.GetPropertyName<tb_FM_PayeeInfo>(c => c.Details);
            string employeeIdPropertyName = entity.GetPropertyName<tb_FM_PayeeInfo>(c => c.Employee_ID);
            string customerVendorIdPropertyName = entity.GetPropertyName<tb_FM_PayeeInfo>(c => c.CustomerVendor_ID);
            string accountTypePropertyName = entity.GetPropertyName<tb_FM_PayeeInfo>(c => c.Account_type);
            string accountNamePropertyName = entity.GetPropertyName<tb_FM_PayeeInfo>(c => c.Account_name);
            string accountNoPropertyName = entity.GetPropertyName<tb_FM_PayeeInfo>(c => c.Account_No);
            string belongingBankPropertyName = entity.GetPropertyName<tb_FM_PayeeInfo>(c => c.BelongingBank);
            
            entity.PropertyChanged += (sender, s2) =>
            {
                if (_EditEntity == null)
                {
                    return;
                }
                
                string propertyName = s2.PropertyName;
                
                // 如果是Details属性自身变化，直接返回，避免循环更新
                if (propertyName == detailsPropertyName)
                {
                    return;
                }
                
                // 当员工ID或客户供应商ID变化时，统一更新UI控件可见性
                if (propertyName == employeeIdPropertyName || propertyName == customerVendorIdPropertyName)
                {
                    UpdateUIControlVisibility();
                }
                
                // 只有在相关字段变化时才更新Details
                if (propertyName == employeeIdPropertyName ||
                    propertyName == customerVendorIdPropertyName ||
                    propertyName == accountTypePropertyName ||
                    propertyName == accountNamePropertyName ||
                    propertyName == accountNoPropertyName ||
                    propertyName == belongingBankPropertyName)
                {
                    // 获取显示名称
                    string shortName = string.Empty;
                    if (_EditEntity.CustomerVendor_ID.HasValue && _EditEntity.CustomerVendor_ID.Value > 0)
                    {
                        shortName = cmbCustomerVendor_ID.Text;
                    }
                    else if (_EditEntity.Employee_ID.HasValue && _EditEntity.Employee_ID.Value > 0)
                    {
                        shortName = cmbEmployee_ID.Text;
                    }
                    
                    // 处理可能为null的字段
                    string accountType = ((AccountType)_EditEntity.Account_type).ToString();
                    string accountName = _EditEntity.Account_name ?? "未命名";
                    string accountNo = _EditEntity.Account_No ?? "无账号";
                    string belongingBank = _EditEntity.BelongingBank ?? "无银行信息";
                    
                    // 组合所有字段更新Details
                    _EditEntity.Details = $"{shortName}-{accountType}-{accountName}-{accountNo}-{belongingBank}";
                }

            };

            // 设置初始UI状态：根据当前实体值决定显示哪些控件
            UpdateUIControlVisibility();
            
            base.BindData(entity);
        }
        
        /// <summary>
        /// 根据实体当前值更新UI控件的可见性
        /// </summary>
        /// <summary>
        /// 员工下拉框选中事件处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmbEmployee_ID_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 只要除第一次加载时外，有选中项目的变化就隐藏另一个控件
            bool hasSelection = cmbEmployee_ID.SelectedValue != null && cmbEmployee_ID.SelectedValue != DBNull.Value;
            
            // 立即隐藏往来单位控件
            cmbCustomerVendor_ID.Visible = !hasSelection;
            lblCustomerVendor_ID.Visible = !hasSelection;
            
            // 如果有选择且实体存在，清空往来单位的值
            if (_EditEntity != null && hasSelection)
            {
                _EditEntity.CustomerVendor_ID = null;
            }
        }

        /// <summary>
        /// 往来单位下拉框选中事件处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmbCustomerVendor_ID_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 只要除第一次加载时外，有选中项目的变化就隐藏另一个控件
            bool hasSelection = cmbCustomerVendor_ID.SelectedValue != null && cmbCustomerVendor_ID.SelectedValue != DBNull.Value;
            
            // 立即隐藏员工控件
            cmbEmployee_ID.Visible = !hasSelection;
            lblEmployee_ID.Visible = !hasSelection;
            
            // 如果有选择且实体存在，清空员工的值
            if (_EditEntity != null && hasSelection)
            {
                _EditEntity.Employee_ID = null;
            }
        }

        /// <summary>
        /// 根据实体当前值更新UI控件的可见性（主要用于初始化时设置）
        /// </summary>
        private void UpdateUIControlVisibility()
        {
            if (_EditEntity == null)
                return;
                
            bool hasEmployeeId = _EditEntity.Employee_ID.HasValue && _EditEntity.Employee_ID.Value > 0;
            bool hasCustomerVendorId = _EditEntity.CustomerVendor_ID.HasValue && _EditEntity.CustomerVendor_ID.Value > 0;
            
            // 员工ID和客户供应商ID二选一显示
            cmbEmployee_ID.Visible = !hasCustomerVendorId;
            lblEmployee_ID.Visible = !hasCustomerVendorId;
            cmbEmployee_ID.Enabled = true;
            
            cmbCustomerVendor_ID.Visible = !hasEmployeeId;
            lblCustomerVendor_ID.Visible = !hasEmployeeId;
            cmbCustomerVendor_ID.Enabled = true;
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
