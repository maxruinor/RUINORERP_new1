
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/12/2024 10:37:28
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
    /// 跟进计划表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_CRM_FollowUpPlansEdit:UserControl
    {
     public tb_CRM_FollowUpPlansEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_CRM_FollowUpPlans UIToEntity()
        {
        tb_CRM_FollowUpPlans entity = new tb_CRM_FollowUpPlans();
                     entity.Customer_id = Int64.Parse(txtCustomer_id.Text);
                        entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.PlanStartDate = DateTime.Parse(txtPlanStartDate.Text);
                        entity.PlanEndDate = DateTime.Parse(txtPlanEndDate.Text);
                        entity.PlanStatus = Int32.Parse(txtPlanStatus.Text);
                        entity.PlanSubject = txtPlanSubject.Text ;
                       entity.PlanContent = txtPlanContent.Text ;
                       entity.Notes = txtNotes.Text ;
                       entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                        entity.isdeleted = Boolean.Parse(txtisdeleted.Text);
                                return entity;
}
        */

        
        private tb_CRM_FollowUpPlans _EditEntity;
        public tb_CRM_FollowUpPlans EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_CRM_FollowUpPlans entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_CRM_Customer>(entity, k => k.Customer_id, v=>v.XXNAME, cmbCustomer_id);
          // DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v=>v.XXNAME, cmbEmployee_ID);
           DataBindingHelper.BindData4DataTime<tb_CRM_FollowUpPlans>(entity, t => t.PlanStartDate, dtpPlanStartDate,false);
           DataBindingHelper.BindData4DataTime<tb_CRM_FollowUpPlans>(entity, t => t.PlanEndDate, dtpPlanEndDate,false);
           DataBindingHelper.BindData4TextBox<tb_CRM_FollowUpPlans>(entity, t => t.PlanStatus, txtPlanStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_CRM_FollowUpPlans>(entity, t => t.PlanSubject, txtPlanSubject, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_CRM_FollowUpPlans>(entity, t => t.PlanContent, txtPlanContent, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_CRM_FollowUpPlans>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<tb_CRM_FollowUpPlans>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_CRM_FollowUpPlans>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_CRM_FollowUpPlans>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_CRM_FollowUpPlans>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CheckBox<tb_CRM_FollowUpPlans>(entity, t => t.isdeleted, chkisdeleted, false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



