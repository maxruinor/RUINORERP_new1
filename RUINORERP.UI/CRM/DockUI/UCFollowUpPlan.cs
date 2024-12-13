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
using RUINORERP.Global.EnumExt.CRM;
using RUINORERP.UI.SysConfig;
using System.Diagnostics;
using RUINORERP.Common.Extensions;
using RUINORERP.UI.BI;

namespace RUINORERP.UI.CRM.DockUI
{
    public partial class UCFollowUpPlan : UserControl
    {
        public UCFollowUpPlan()
        {
            InitializeComponent();
        }

        tb_CRM_FollowUpPlans RecordEntity = null;
        public void BindData(tb_CRM_FollowUpPlans entity, ActionStatus actionStatus = ActionStatus.无操作)
        {
            RecordEntity = entity;
            DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID, true);
            DataBindingHelper.BindData4DataTime<tb_CRM_FollowUpPlans>(entity, t => t.PlanStartDate, dtpPlanStartDate, false);
            DataBindingHelper.BindData4DataTime<tb_CRM_FollowUpPlans>(entity, t => t.PlanEndDate, dtpPlanEndDate, false);

            DataBindingHelper.BindData4CmbByEnum<tb_CRM_FollowUpPlans>(entity, k => k.PlanStatus, typeof(FollowUpPlanStatus), cmbPlanStatus, false);
            DataBindingHelper.BindData4TextBox<tb_CRM_FollowUpPlans>(entity, t => t.PlanSubject, txtPlanSubject, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_CRM_FollowUpPlans>(entity, t => t.PlanContent, txtPlanContent, BindDataType4TextBox.Text, false);
        }

        private void klinklblDetail_LinkClicked(object sender, EventArgs e)
        {
            //打开详情
        }
    }
}
