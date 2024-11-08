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
using RUINORERP.Common.Helper;
using RUINORERP.UI.Common;
using RUINORERP.Global;
using FastReport.Utils;
using RUINORERP.Common.Extensions;

namespace RUINORERP.UI.BI
{


    [MenuAttrAssemblyInfo("员工信息编辑", true, UIType.单表数据)]
    public partial class UCEmployeeEdit : BaseEditGeneric<tb_Employee>
    {
        public UCEmployeeEdit()
        {
            InitializeComponent();
        }

        private tb_Employee _EditEntity;
        Binding startDate;
        Binding birthBinding;
        Binding endDate;
     

        public override void BindData(BaseEntity entity)
        {
            _EditEntity = entity as tb_Employee;
            if (_EditEntity.Employee_ID == 0)
            {
                _EditEntity.Employee_NO = BizCodeGenerator.Instance.GetBaseInfoNo(BaseInfoType.Employee);
                _EditEntity.Is_available = true;
                _EditEntity.Is_enabled = true;
            }

            DataBindingHelper.BindData4TextBox<tb_Employee>(entity, t => t.Employee_NO, txtEmployee_NO, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_Employee>(entity, t => t.Employee_Name, txtEmployee_Name, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4RadioGroupTrueFalse<tb_Employee>(entity, t => t.Gender, rdb男, rdb女);
            DataBindingHelper.BindData4TextBox<tb_Employee>(entity, t => t.Position, txtPosition, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4Cmb<tb_Department>(entity, k => k.DepartmentID, v => v.DepartmentName, cmbDepartment);
            DataBindingHelper.BindData4DataTime<tb_Employee>(entity, t => t.Birthday, dtpBirthday, false);
            DataBindingHelper.BindData4DataTime<tb_Employee>(entity, t => t.StartDate, txtStartDate, false);
            DataBindingHelper.BindData4TextBox<tb_Employee>(entity, t => t.JobTitle, txtJobTitle, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_Employee>(entity, t => t.PhoneNumber, txtPhoneNumber, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_Employee>(entity, t => t.Address, txtAddress, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_Employee>(entity, t => t.Email, txtEmail, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_Employee>(entity, t => t.Education, txtEducation, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_Employee>(entity, t => t.LanguageSkills, txtLanguageSkills, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_Employee>(entity, t => t.University, txtUniversity, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_Employee>(entity, t => t.IDNumber, txtIDNumber, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4DataTime<tb_Employee>(entity, t => t.EndDate, dtpEndDate, false);
            DataBindingHelper.BindData4RadioGroupTrueFalse<tb_Employee>(entity, t => t.Is_enabled, rdbis_enabledYes, rdbis_enabledNo);
            //有默认值
            DataBindingHelper.BindData4RadioGroupTrueFalse<tb_Employee>(entity, t => t.Is_available, rdbis_availableYes, rdbis_availableNo);
            //有默认值
            DataBindingHelper.BindData4TextBox<tb_Employee>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, false);
            //DataBindingHelper.BindData4TextBox<tb_Employee>(entity, t => t.salary.ToString(),txts txtsalary, BindDataType4TextBox.Money, false);
            //后面这些依赖于控件绑定的数据源和字段。所以要在绑定后执行。
            if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
            {
                base.InitRequiredToControl(new tb_EmployeeValidator(), kryptonPanel1.Controls);
                base.InitEditItemToControl(entity, kryptonPanel1.Controls);
            }
            if (_EditEntity.Employee_ID > 0)
            {
                btnAddPayeeInfo.Visible = true;
            }
            else
            {
                btnAddPayeeInfo.Visible = false;
            }

        }


        private void DtpEndDate_Validating(object sender, CancelEventArgs e)
        {
            if (!dtpEndDate.Checked)
            {
                e.Cancel = false;
                _EditEntity.EndDate = null;
                dtpEndDate.DataBindings.Clear();
            }
        }

        private void DtpBirth_Validating(object sender, CancelEventArgs e)
        {
            if (!dtpBirthday.Checked)
            {
                e.Cancel = false;
                _EditEntity.Birthday = null;
                dtpBirthday.DataBindings.Clear();
            }
        }

        private void TxtStartDate_Validating(object sender, CancelEventArgs e)
        {
            if (!txtStartDate.Checked)
            {
                e.Cancel = false;
                _EditEntity.StartDate = null;
                txtStartDate.DataBindings.Clear();
            }

        }

        private void StartDate_Format(object sender, ConvertEventArgs e)
        {
            if (e.Value == null)
            {
                //txtStartDate.Checked = false;
                // e.Value = DateTime.Now;
            }
            else
            {
                txtStartDate.Checked = true;
                //e.Value = e.Value;
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
                bindingSourceEdit.EndEdit();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }



        private void txtStartDate_CheckedChanged(object sender, EventArgs e)
        {
            if (!txtStartDate.Checked)
            {
                this.txtStartDate.Format = DateTimePickerFormat.Custom;
                this.txtStartDate.CustomFormat = "   ";
            }
            else
            {
                this.txtStartDate.Format = DateTimePickerFormat.Short;
                this.txtStartDate.CustomFormat = null;
                if (txtStartDate.DataBindings.Count == 0)
                {
                    txtStartDate.DataBindings.Add(startDate);
                }
            }
        }

        private void UCEmployeeEdit_Load(object sender, EventArgs e)
        {

        }

        private void dtpBirth_CheckedChanged(object sender, EventArgs e)
        {
            if (!dtpBirthday.Checked)
            {
                dtpBirthday.Format = DateTimePickerFormat.Custom;
                dtpBirthday.CustomFormat = "   ";
            }
            else
            {
                dtpBirthday.Format = DateTimePickerFormat.Short;
                dtpBirthday.CustomFormat = null;
                if (dtpBirthday.DataBindings.Count == 0)
                {
                    dtpBirthday.DataBindings.Add(birthBinding);
                }
            }
        }

        private void dtpEndDate_CheckedChanged(object sender, EventArgs e)
        {
            if (!dtpEndDate.Checked)
            {
                dtpEndDate.Format = DateTimePickerFormat.Custom;
                dtpEndDate.CustomFormat = "   ";
            }
            else
            {
                dtpEndDate.Format = DateTimePickerFormat.Short;
                dtpEndDate.CustomFormat = null;
                if (dtpEndDate.DataBindings.Count == 0)
                {
                    dtpEndDate.DataBindings.Add(endDate);
                }
            }
        }

        private async void btnAddPayeeInfo_Click(object sender, EventArgs e)
        {
            object frm = Activator.CreateInstance(typeof(UCFMPayeeInfoEdit));
            if (frm.GetType().BaseType.Name.Contains("BaseEditGeneric"))
            {
                BaseEditGeneric<tb_FM_PayeeInfo> frmaddg = frm as BaseEditGeneric<tb_FM_PayeeInfo>;
                frmaddg.Text = "收款账号编辑";
                frmaddg.bindingSourceEdit.DataSource = new List<tb_FM_PayeeInfo>();
                object obj = frmaddg.bindingSourceEdit.AddNew();
                tb_FM_PayeeInfo payeeInfo = obj as tb_FM_PayeeInfo;
                payeeInfo.Employee_ID = _EditEntity.Employee_ID;
                BaseEntity bty = payeeInfo as BaseEntity;
                bty.ActionStatus = ActionStatus.加载;
                BusinessHelper.Instance.EditEntity(bty);
                frmaddg.BindData(bty);
                if (frmaddg.ShowDialog() == DialogResult.OK)
                {
                    UIBizSrvice.SavePayeeInfo(payeeInfo);
                }
            }

        }
    }
}
