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

namespace RUINORERP.UI.BI
{


    [MenuAttrAssemblyInfo("用户信息编辑", true, UIType.单表数据)]
    public partial class UCUserInfoEdit : BaseEditGeneric<tb_UserInfo>
    {
        public UCUserInfoEdit()
        {
            InitializeComponent();
        }

        public async void BindDataBackup(tb_UserInfo entity)
        {

            DataBindingHelper.BindData4TextBox<tb_UserInfo>(entity, t => t.UserName, txtUserName, BindDataType4TextBox.Text, true);
            DataBindingHelper.BindData4TextBox<tb_UserInfo>(entity, t => t.Password, txtPassword, BindDataType4TextBox.Text, true);
            #region 员工
            tb_EmployeeController<tb_UserInfo> de = Startup.GetFromFac<tb_EmployeeController<tb_UserInfo>>();
            BindingSource bs = new BindingSource();
            bs.DataSource = await de.QueryInUse();
            ComboBoxHelper.InitDropList(bs, cmbEmployee, "Employee_ID", "Employee_Name", ComboBoxStyle.DropDownList, false);

            var depa = new Binding("SelectedValue", entity, "Employee_ID", true, DataSourceUpdateMode.OnValidation);
            //数据源的数据类型转换为控件要求的数据类型。
            depa.Format += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            //将控件的数据类型转换为数据源要求的数据类型。
            depa.Parse += (s, args) => args.Value = args.Value == null ? -1 : args.Value;
            cmbEmployee.DataBindings.Add(depa);
            #endregion

            // Set initial values

            rdbis_enabledYes.Checked = entity.is_enabled;
            rdbis_enabledNo.Checked = !entity.is_enabled;


            // Change on event
            rdbis_enabledYes.CheckedChanged += delegate
            {
                entity.is_enabled = rdbis_enabledYes.Checked;
            };
            rdbis_enabledNo.CheckedChanged += delegate
            {
                entity.is_enabled = !rdbis_enabledNo.Checked;
            };


            rdbis_enabledYes.DataBindings.Add("Checked", entity, "is_enabled", true, DataSourceUpdateMode.OnValidation);



            // Set initial values
            rdbis_availableYes.Checked = entity.is_available;
            rdbis_availableNo.Checked = !entity.is_available;

            // Change on event
            rdbis_availableYes.CheckedChanged += delegate
            {
                entity.is_available = rdbis_availableYes.Checked;
            };
            rdbis_availableNo.CheckedChanged += delegate
            {
                entity.is_available = !rdbis_availableNo.Checked;
            };
            Binding is_availableBinding = rdbis_availableYes.DataBindings.Add("Checked", entity, "is_available", true, DataSourceUpdateMode.OnValidation);

            DataBindingHelper.BindData4TextBox<tb_UserInfo>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, true);

            //后面这些依赖于控件绑定的数据源和字段。所以要在绑定后执行。
            if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
            {
                base.InitRequiredToControl(new tb_UserInfoValidator(), kryptonPanel1.Controls);
                base.InitEditItemToControl(entity, kryptonPanel1.Controls);
            }
            base.errorProviderForAllInput.DataSource = entity;
        }

        public override void BindData(BaseEntity entity)
        {

            DataBindingHelper.BindData4TextBox<tb_UserInfo>(entity, t => t.UserName, txtUserName, BindDataType4TextBox.Text, true);
            DataBindingHelper.BindData4TextBox<tb_UserInfo>(entity, t => t.Password, txtPassword, BindDataType4TextBox.Text, true);
            DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v => v.Employee_Name, cmbEmployee);

            DataBindingHelper.BindData4RadioGroupTrueFalse<tb_UserInfo>(entity, t => t.is_enabled, rdbis_enabledYes, rdbis_enabledNo);
            DataBindingHelper.BindData4RadioGroupTrueFalse<tb_UserInfo>(entity, t => t.is_available, rdbis_availableYes, rdbis_availableNo);
            DataBindingHelper.BindData4RadioGroupTrueFalse<tb_UserInfo>(entity, t => t.IsSuperUser, rdbIsSuperUserYes, rdbIsSuperUserNo);

            DataBindingHelper.BindData4TextBox<tb_UserInfo>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text, true);

            //后面这些依赖于控件绑定的数据源和字段。所以要在绑定后执行。
            if (entity.ActionStatus == ActionStatus.新增 || entity.ActionStatus == ActionStatus.修改)
            {
                base.InitRequiredToControl(new tb_UserInfoValidator(), kryptonPanel1.Controls);
                base.InitEditItemToControl(entity, kryptonPanel1.Controls);
            }
            base.errorProviderForAllInput.DataSource = entity;
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
                if (rdbIsSuperUserYes.Checked)
                {
                    if (MessageBox.Show("当前用户【" + txtUserName.Text + "】" + "将设置为超级用户！\r\n您确定吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                    {
                        return;
                    }
                }


                bindingSourceEdit.EndEdit();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }


    }
}
