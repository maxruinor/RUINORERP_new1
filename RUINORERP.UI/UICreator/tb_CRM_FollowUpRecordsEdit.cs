
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/09/2024 21:23:57
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
    /// 跟进记录表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_CRM_FollowUpRecordsEdit:UserControl
    {
     public tb_CRM_FollowUpRecordsEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_CRM_FollowUpRecords UIToEntity()
        {
        tb_CRM_FollowUpRecords entity = new tb_CRM_FollowUpRecords();
                     entity.Customer_id = Int64.Parse(txtCustomer_id.Text);
                        entity.LeadID = Int64.Parse(txtLeadID.Text);
                        entity.PlanID = Int64.Parse(txtPlanID.Text);
                        entity.NextPlanID = Int64.Parse(txtNextPlanID.Text);
                        entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.FollowUpDate = DateTime.Parse(txtFollowUpDate.Text);
                        entity.FollowUpMethod = Int32.Parse(txtFollowUpMethod.Text);
                        entity.FollowUpSubject = txtFollowUpSubject.Text ;
                       entity.FollowUpContent = txtFollowUpContent.Text ;
                       entity.Notes = txtNotes.Text ;
                       entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                        entity.isdeleted = Boolean.Parse(txtisdeleted.Text);
                                return entity;
}
        */

        
        private tb_CRM_FollowUpRecords _EditEntity;
        public tb_CRM_FollowUpRecords EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_CRM_FollowUpRecords entity)
        {
        _EditEntity = entity;
                       NextPlanID主外字段不一致。// DataBindingHelper.BindData4Cmb<tb_CRM_Customer>(entity, k => k.Customer_id, v=>v.XXNAME, cmbCustomer_id);
          NextPlanID主外字段不一致。// DataBindingHelper.BindData4Cmb<tb_CRM_Leads>(entity, k => k.LeadID, v=>v.XXNAME, cmbLeadID);
          // DataBindingHelper.BindData4Cmb<tb_CRM_FollowUpPlans>(entity, k => k.PlanID, v=>v.XXNAME, cmbPlanID);
NextPlanID主外字段不一致。// DataBindingHelper.BindData4Cmb<tb_CRM_FollowUpPlans>(entity, k => k.PlanID, v=>v.XXNAME, cmbPlanID);
          NextPlanID主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_CRM_FollowUpRecords>(entity, t => t.NextPlanID, txtNextPlanID, BindDataType4TextBox.Qty,false);
          NextPlanID主外字段不一致。// DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v=>v.XXNAME, cmbEmployee_ID);
           DataBindingHelper.BindData4DataTime<tb_CRM_FollowUpRecords>(entity, t => t.FollowUpDate, dtpFollowUpDate,false);
          NextPlanID主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_CRM_FollowUpRecords>(entity, t => t.FollowUpMethod, txtFollowUpMethod, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_CRM_FollowUpRecords>(entity, t => t.FollowUpSubject, txtFollowUpSubject, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_CRM_FollowUpRecords>(entity, t => t.FollowUpContent, txtFollowUpContent, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_CRM_FollowUpRecords>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<tb_CRM_FollowUpRecords>(entity, t => t.Created_at, dtpCreated_at,false);
          NextPlanID主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_CRM_FollowUpRecords>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_CRM_FollowUpRecords>(entity, t => t.Modified_at, dtpModified_at,false);
          NextPlanID主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_CRM_FollowUpRecords>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CehckBox<tb_CRM_FollowUpRecords>(entity, t => t.isdeleted, chkisdeleted, false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



