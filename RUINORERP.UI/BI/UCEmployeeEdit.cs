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
        public  void BindData11111(tb_Employee entity)
        {
            _EditEntity = entity;

            #region 部门 性别  



            var depa = new Binding("SelectedValue", entity, "DepartmentID", true, DataSourceUpdateMode.OnValidation);
            //数据源的数据类型转换为控件要求的数据类型。
            depa.Format += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            //将控件的数据类型转换为数据源要求的数据类型。
            depa.Parse += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            cmbDepartment.DataBindings.Add(depa);


            /*
            // create binding between "Sex" property and RadioButton.Checked property 
            var maleBinding = new Binding("Checked", entity, "Gender"); 
            // when Formatting (reading from datasource), return true for M, else false 
            maleBinding.Format += (s, args) => args.Value = ((string)args.Value) == "男";
            // when Parsing (writing to datasource), return "M" for true, else "F" 
             maleBinding.Parse += (s, args) => args.Value = (bool)args.Value ? "男" : "女";
            // add the binding 
            rdbSexMale.DataBindings.Add(maleBinding);
            // you don't need to bind the Female radiobutton, just make it do the opposite 
            // of Male by handling the CheckedChanged event on Male: 
            rdbSexMale.CheckedChanged += (s, args) => rdbFemale.Checked = !rdbSexMale.Checked;
            rdbFemale.CheckedChanged += (s, args) => rdbSexMale.Checked = !rdbFemale.Checked;

            //rdbSexMale
            //txtGender.DataBindings.Add("Text", entity, "Gender", false, DataSourceUpdateMode.OnValidation);
            //rdb男.DataBindings.Add()
            */
            //entity.Gender = rdb男.Checked;


            var SexBinding = new Binding("Checked", entity, "Gender", false, DataSourceUpdateMode.OnValidation);
            SexBinding.Format += (s, args) => args.Value = ((string)args.Value) == "男";
            SexBinding.Parse += (s, args) => args.Value = (bool)args.Value ? "男" : "女";
            rdb男.DataBindings.Add(SexBinding);
            #endregion
            txtEmployee_NO.DataBindings.Add("Text", entity, "Employee_NO", false, DataSourceUpdateMode.OnValidation);
            txtEmployee_Name.DataBindings.Add("Text", entity, "Employee_Name", false, DataSourceUpdateMode.OnValidation);

            txtPosition.DataBindings.Add("Text", entity, "Position", false, DataSourceUpdateMode.OnValidation);

            txtMarriage.DataBindings.Add("Text", entity, "Marriage", false, DataSourceUpdateMode.OnValidation);
            //DataBindingHelper.BindData4TextBox<tb_Employee>(entity, t => t.Marriage, txtMarriage, BindDataType4TextBox.Text, false);

            #region 生日等日期

            birthBinding = new Binding("Value", entity, "Birthday", true, DataSourceUpdateMode.OnValidation);
            birthBinding.Format += (s, args) => args.Value = args.Value == null ? " " : args.Value;
            birthBinding.Parse += (s, args) => args.Value = !dtpBirthday.Checked ? null : args.Value;

            dtpBirthday.Validating += DtpBirth_Validating;
            if (entity.Birthday == null)
            {
                dtpBirthday.Format = DateTimePickerFormat.Custom;
                dtpBirthday.CustomFormat = "   ";
            }
            dtpBirthday.DataBindings.Add(birthBinding);

            //数据源的数据类型转换为控件要求的数据类型。
            //StartDate.Format += (s, args) => args.Value = args.Value == null ? " " : args.Value;
            //将控件的数据类型转换为数据源要求的数据类型。 下面设置了一个特殊值来代表 NULL
            //            StartDate.Parse += (s, args) => args.Value = args.Value.ToString() == "1901/1/1 0:00:00" ? null : args.Value;
            //StartDate.Format += (s, args) => args.Value = args.Value == null ? " " : args.Value;
            //birthBinding.Format += StartDate_Format;
            //birthBinding.Parse += (s, args) => args.Value = !txtStartDate.Checked ? null : args.Value;

            startDate = new Binding("Value", entity, "StartDate", true, DataSourceUpdateMode.OnValidation);
            //数据源的数据类型转换为控件要求的数据类型。
            //StartDate.Format += (s, args) => args.Value = args.Value == null ? " " : args.Value;
            //将控件的数据类型转换为数据源要求的数据类型。 下面设置了一个特殊值来代表 NULL
            //            StartDate.Parse += (s, args) => args.Value = args.Value.ToString() == "1901/1/1 0:00:00" ? null : args.Value;
            //StartDate.Format += (s, args) => args.Value = args.Value == null ? " " : args.Value;
            startDate.Format += StartDate_Format;
            startDate.Parse += (s, args) => args.Value = !txtStartDate.Checked ? null : args.Value;
            txtStartDate.Validating += TxtStartDate_Validating;
            if (entity.StartDate == null)
            {
                this.txtStartDate.Format = DateTimePickerFormat.Custom;
                this.txtStartDate.CustomFormat = "   ";
            }
            txtStartDate.DataBindings.Add(startDate);



            endDate = new Binding("Value", entity, "EndDate", true, DataSourceUpdateMode.OnValidation);
            endDate.Format += (s, args) => args.Value = args.Value == null ? " " : args.Value;
            endDate.Parse += (s, args) => args.Value = !dtpEndDate.Checked ? null : args.Value;
            dtpEndDate.Validating += DtpEndDate_Validating;
            if (entity.Birthday == null)
            {
                dtpEndDate.Format = DateTimePickerFormat.Custom;
                dtpEndDate.CustomFormat = "   ";
            }
            dtpEndDate.DataBindings.Add(endDate);
            #endregion

            txtJobTitle.DataBindings.Add("Text", entity, "JobTitle", false, DataSourceUpdateMode.OnValidation);
            txtAddress.DataBindings.Add("Text", entity, "Address", false, DataSourceUpdateMode.OnValidation);
            txtEmail.DataBindings.Add("Text", entity, "Email", false, DataSourceUpdateMode.OnValidation);
            txtEducation.DataBindings.Add("Text", entity, "Education", false, DataSourceUpdateMode.OnValidation);
            txtLanguageSkills.DataBindings.Add("Text", entity, "LanguageSkills", false, DataSourceUpdateMode.OnValidation);
            txtUniversity.DataBindings.Add("Text", entity, "University", false, DataSourceUpdateMode.OnValidation);
            txtIDNumber.DataBindings.Add("Text", entity, "IDNumber", false, DataSourceUpdateMode.OnValidation);



            // Set initial values
            rdbis_enabledYes.Checked = entity.Is_enabled;
            rdbis_enabledNo.Checked = !entity.Is_enabled;
            // Change on event
            rdbis_enabledYes.CheckedChanged += delegate
            {
                entity.Is_enabled = rdbis_enabledYes.Checked;
            };
            rdbis_enabledNo.CheckedChanged += delegate
            {
                entity.Is_enabled = !rdbis_enabledNo.Checked;
            };


            rdbis_enabledYes.DataBindings.Add("Checked", entity, "is_enabled", true, DataSourceUpdateMode.OnValidation);

            // Set initial values
            rdbis_availableYes.Checked = entity.Is_available;
            rdbis_availableNo.Checked = !entity.Is_available;

            // Change on event
            rdbis_availableYes.CheckedChanged += delegate
            {
                entity.Is_available = rdbis_availableYes.Checked;
            };
            rdbis_availableNo.CheckedChanged += delegate
            {
                entity.Is_available = !rdbis_availableNo.Checked;
            };
            Binding is_availableBinding = rdbis_availableYes.DataBindings.Add("Checked", entity, "is_available", true, DataSourceUpdateMode.OnValidation);


            txtNotes.DataBindings.Add("Text", entity, "Notes", false, DataSourceUpdateMode.OnValidation);

            if (_EditEntity.Employee_ID == 0)
            {
                _EditEntity.Employee_NO = BizCodeGenerator.Instance.GetBaseInfoNo(BaseInfoType.Employee);
            }
        }


        public override void BindData(BaseEntity entity)
        {
            _EditEntity = entity as tb_Employee;
            if (_EditEntity.Employee_ID == 0)
            {
                _EditEntity.Employee_NO = BizCodeGenerator.Instance.GetBaseInfoNo(BaseInfoType.Employee);
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
    }
}
