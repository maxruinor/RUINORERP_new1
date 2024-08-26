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
    /// <summary>
    /// 审核审批窗口，应用于审核和反审核，字段标记区别
    /// </summary>
    public partial class frmReApproval : frmBase
    {
        private bool _IsApproval = true;

        public bool IsApproval { get => _IsApproval; set => _IsApproval = value; }


        public frmReApproval()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (txtOpinion.Text.Trim().Length <= 3)
            {
                //反审原因不能为空，并且不能小于3个字符。
                MessageBox.Show("反审原因不能为空，并且不能小于3个字符。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                _entity.ApprovalStatus = (int)ApprovalStatus.已审核;

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _entity.ApprovalStatus = (int)ApprovalStatus.未审核;
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void frmApproval_Load(object sender, EventArgs e)
        {
            //txtOpinion.Text = MainForm.Instance.AppContext.CurUserInfo.UserInfo.tb_employee.Employee_Name + "【反审】";
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
            DataBindingHelper.BindData4TextBox<ApprovalEntity>(entity, t => t.ApprovalComments, txtOpinion, BindDataType4TextBox.Text, false);
            errorProviderForAllInput.DataSource = entity;
        }


    }
}
