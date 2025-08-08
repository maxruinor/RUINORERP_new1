
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:20
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Common;
using RUINORERP.Model;
using RUINORERP.UI.Common;

namespace RUINORERP.UI
{
    /// <summary>
    /// 客户关系配置表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_CRMConfigEdit:UserControl
    {
     public tb_CRMConfigEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_CRMConfig UIToEntity()
        {
        tb_CRMConfig entity = new tb_CRMConfig();
                     entity.CS_UseLeadsFunction = Boolean.Parse(txtCS_UseLeadsFunction.Text);
                        entity.CS_NewCustToLeadsCustDays = Int32.Parse(txtCS_NewCustToLeadsCustDays.Text);
                        entity.CS_SleepingCustomerDays = Int32.Parse(txtCS_SleepingCustomerDays.Text);
                        entity.CS_LostCustomersDays = Int32.Parse(txtCS_LostCustomersDays.Text);
                        entity.CS_ActiveCustomers = Int32.Parse(txtCS_ActiveCustomers.Text);
                        entity.LS_ConvCustHasFollowUpDays = Int32.Parse(txtLS_ConvCustHasFollowUpDays.Text);
                        entity.LS_ConvCustNoTransDays = Int32.Parse(txtLS_ConvCustNoTransDays.Text);
                        entity.LS_ConvCustLostDays = Int32.Parse(txtLS_ConvCustLostDays.Text);
                        entity.NoFollToPublicPoolDays = Int32.Parse(txtNoFollToPublicPoolDays.Text);
                        entity.CustomerNoOrderDays = Int32.Parse(txtCustomerNoOrderDays.Text);
                        entity.CustomerNoFollowUpDays = Int32.Parse(txtCustomerNoFollowUpDays.Text);
                        entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                                return entity;
}
        */

        
        private tb_CRMConfig _EditEntity;
        public tb_CRMConfig EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_CRMConfig entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4CheckBox<tb_CRMConfig>(entity, t => t.CS_UseLeadsFunction, chkCS_UseLeadsFunction, false);
           DataBindingHelper.BindData4TextBox<tb_CRMConfig>(entity, t => t.CS_NewCustToLeadsCustDays, txtCS_NewCustToLeadsCustDays, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_CRMConfig>(entity, t => t.CS_SleepingCustomerDays, txtCS_SleepingCustomerDays, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_CRMConfig>(entity, t => t.CS_LostCustomersDays, txtCS_LostCustomersDays, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_CRMConfig>(entity, t => t.CS_ActiveCustomers, txtCS_ActiveCustomers, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_CRMConfig>(entity, t => t.LS_ConvCustHasFollowUpDays, txtLS_ConvCustHasFollowUpDays, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_CRMConfig>(entity, t => t.LS_ConvCustNoTransDays, txtLS_ConvCustNoTransDays, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_CRMConfig>(entity, t => t.LS_ConvCustLostDays, txtLS_ConvCustLostDays, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_CRMConfig>(entity, t => t.NoFollToPublicPoolDays, txtNoFollToPublicPoolDays, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_CRMConfig>(entity, t => t.CustomerNoOrderDays, txtCustomerNoOrderDays, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_CRMConfig>(entity, t => t.CustomerNoFollowUpDays, txtCustomerNoFollowUpDays, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_CRMConfig>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_CRMConfig>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_CRMConfig>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_CRMConfig>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



