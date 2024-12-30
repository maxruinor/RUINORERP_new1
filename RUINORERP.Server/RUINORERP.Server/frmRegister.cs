using FluentValidation.Results;
using HLH.Lib.Security;
using Mapster;
using RUINORERP.Business;
using RUINORERP.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace RUINORERP.Server
{
    public partial class frmRegister : frmBase
    {
        public frmRegister()
        {
            InitializeComponent();
        }

        private async void frmRegister_Load(object sender, EventArgs e)
        {

            if (EditEntity == null)
            {
                ////没有返回Null，如果结果大于1条会抛出错误
                EditEntity = await Program.AppContextData.Db.CopyNew().Queryable<tb_sys_RegistrationInfo>().SingleAsync();
            }
            //加载注册的信息
            BindData(EditEntity);
            btnRegister.Enabled = !EditEntity.IsRegistered;
        }


        private tb_sys_RegistrationInfo _EditEntity;
        public tb_sys_RegistrationInfo EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_sys_RegistrationInfo entity)
        {
            txtCompanyName.DataBindings.Add(new Binding("Text", entity, nameof(entity.CompanyName), true, DataSourceUpdateMode.OnValidation));
            txtContactName.DataBindings.Add(new Binding("Text", entity, nameof(entity.ContactName), true, DataSourceUpdateMode.OnValidation));
            txtPhoneNumber.DataBindings.Add(new Binding("Text", entity, nameof(entity.PhoneNumber), true, DataSourceUpdateMode.OnValidation));
            txtMachineCode.DataBindings.Add(new Binding("Text", entity, nameof(entity.MachineCode), true, DataSourceUpdateMode.OnValidation));
            txtRegistrationCode.DataBindings.Add(new Binding("Text", entity, nameof(entity.RegistrationCode), true, DataSourceUpdateMode.OnValidation));
            txtConcurrentUsers.DataBindings.Add(new Binding("Text", entity, nameof(entity.ConcurrentUsers), true, DataSourceUpdateMode.OnValidation));
            dtpExpirationDate.DataBindings.Add(new Binding("Value", entity, nameof(entity.ExpirationDate), true, DataSourceUpdateMode.OnValidation));
            txtProductVersion.DataBindings.Add(new Binding("Text", entity, nameof(entity.ProductVersion), true, DataSourceUpdateMode.OnValidation));
            cmbLicenseType.DataBindings.Add(new Binding("SelectedValue", entity, nameof(entity.LicenseType), true, DataSourceUpdateMode.OnValidation));
            dtpPurchaseDate.DataBindings.Add(new Binding("Value", entity, nameof(entity.PurchaseDate), true, DataSourceUpdateMode.OnValidation));
            dtpRegistrationDate.DataBindings.Add(new Binding("Value", entity, nameof(entity.RegistrationDate), true, DataSourceUpdateMode.OnValidation));
            chkIsRegistered.DataBindings.Add(new Binding("Checked", entity, nameof(entity.IsRegistered), true, DataSourceUpdateMode.OnValidation));
            txtRemarks.DataBindings.Add(new Binding("Text", entity, nameof(entity.Remarks), true, DataSourceUpdateMode.OnValidation));
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            this.ValidateChildren(System.Windows.Forms.ValidationConstraints.None);

            BaseController<tb_sys_RegistrationInfo> ctr = Startup.GetFromFacByName<BaseController<tb_sys_RegistrationInfo>>(typeof(tb_sys_RegistrationInfo).Name + "Controller");
            var results = ctr.BaseValidator(EditEntity);

            IList<ValidationFailure> failures = results.Errors;
            //validator.ValidateAndThrow(info);
            StringBuilder msg = new StringBuilder();
            int counter = 1;
            foreach (var item in failures)
            {
                msg.Append(counter.ToString() + ") ");
                msg.Append(item.ErrorMessage).Append("\r\n");
                counter++;
            }
            if (!results.IsValid)
            {
                MessageBox.Show(msg.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            string machineCode = AesEncryption.EncryptString("0", "MACHINECODE");
            MessageBox.Show(machineCode);
            if (results.IsValid)
            {
                //生成机器码。除了使用了这里的注册信息。还绑定电脑的唯一的ID像CPUID等

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
        private void btnGenerateMachineCode_Click(object sender, EventArgs e)
        {
            //如果用GetRequiredService取。则是注册时才用的验证。无参数才是机器生成的。
            if (CheckBaseInfo())
            {
                string machineCode = AesEncryption.EncryptString("0", "MACHINECODE");
                MessageBox.Show(machineCode);
            }
        }

        private bool CheckBaseInfo()
        {
            tb_sys_RegistrationInfoValidator validator = new tb_sys_RegistrationInfoValidator();
            ValidationResult results = validator.Validate(EditEntity);
            IList<ValidationFailure> failures = results.Errors;
            //validator.ValidateAndThrow(info);
            StringBuilder msg = new StringBuilder();
            int counter = 1;
            foreach (var item in failures)
            {
                msg.Append(counter.ToString() + ") ");
                msg.Append(item.ErrorMessage).Append("\r\n");
                counter++;
            }
            if (!results.IsValid)
            {
                MessageBox.Show(msg.ToString(), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return results.IsValid;
        }




        private async void tsbtnSaveRegInfo_Click(object sender, EventArgs e)
        {
            if (CheckBaseInfo())
            {
                var entiry = await Program.AppContextData.Db.Storageable(EditEntity).DefaultAddElseUpdate().ExecuteReturnEntityAsync();
                if (entiry.RegistrationInfoD > 0)
                {
                    MessageBox.Show("保存成功");
                }
                else
                {
                    MessageBox.Show("保存失败");
                }
            }

        }
    }
}
