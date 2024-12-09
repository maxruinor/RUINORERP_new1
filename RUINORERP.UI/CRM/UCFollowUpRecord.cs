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

namespace RUINORERP.UI.CRM
{
    public partial class UCFollowUpRecord : UserControl
    {
        public UCFollowUpRecord()
        {
            InitializeComponent();
        }

        tb_CRM_FollowUpRecords RecordEntity = null;
        public void BindData(tb_CRM_FollowUpRecords entity, ActionStatus actionStatus = ActionStatus.无操作)
        {
            RecordEntity = entity;
            DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v => v.Employee_Name, cmbEmployee_ID);
            DataBindingHelper.BindData4DataTime<tb_CRM_FollowUpRecords>(entity, t => t.FollowUpDate, dtpFollowUpDate, false);
            DataBindingHelper.BindData4CmbByEnum<tb_CRM_FollowUpRecords>(entity, k => k.FollowUpMethod, typeof(FollowUpMethod), cmbFollowUpMethod, false);
            DataBindingHelper.BindData4TextBox<tb_CRM_FollowUpRecords>(entity, t => t.FollowUpSubject, txtFollowUpSubject, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_CRM_FollowUpRecords>(entity, t => t.FollowUpContent, txtFollowUpContent, BindDataType4TextBox.Text, false);
        }

        private void klinklblDetail_LinkClicked(object sender, EventArgs e)
        {
            //打开详情
        }
    }
}
