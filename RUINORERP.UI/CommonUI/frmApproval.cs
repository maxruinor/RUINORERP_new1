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
    public partial class frmApproval : frmBase
    {
        public frmApproval()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            _entity.ApprovalStatus = (int)ApprovalStatus.已审核;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _entity.ApprovalStatus = (int)ApprovalStatus.未审核;
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void frmApproval_Load(object sender, EventArgs e)
        {
            rdbis_Yes.CheckedChanged+=rdbis_Yes_CheckedChanged;
            rdbis_No.CheckedChanged += rdbis_Yes_CheckedChanged;
            if (rdbis_Yes.Checked)
            {
                txtOpinion.Text = MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee.Employee_Name + "【同意】";
            }
            else
            {
                txtOpinion.Text = MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee.Employee_Name + "【不同意】";
            }
        }

        private ApprovalEntity _entity;
        public void BindData(ApprovalEntity entity)
        {
            _entity = entity;
            DataBindingHelper.BindData4TextBox<ApprovalEntity>(entity, t => t.BillNo, txtBillNO, BindDataType4TextBox.Text, false);
            //这个只是显示给用户看。不会修改
            DataBindingHelper.BindData4TextBox<ApprovalEntity>(entity, t => t.bizName, txtBillType, BindDataType4TextBox.Text, false);
            txtBillType.ReadOnly = true;
            entity.ApprovalResults = true;
            DataBindingHelper.BindData4RadioGroupTrueFalse<ApprovalEntity>(entity, t => t.ApprovalResults, rdbis_Yes, rdbis_No);
            DataBindingHelper.BindData4TextBox<ApprovalEntity>(entity, t => t.ApprovalComments, txtOpinion, BindDataType4TextBox.Text, false);
            errorProviderForAllInput.DataSource = entity;
        }

        private void rdbis_Yes_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbis_Yes.Checked)
            {
                txtOpinion.Text = MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee.Employee_Name + "【同意】";
            }
            else
            {
                txtOpinion.Text = MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee.Employee_Name + "【不同意】";
            }

        }
    }
}
