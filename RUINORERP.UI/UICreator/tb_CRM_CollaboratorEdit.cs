
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:17
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
    /// 协作人记录表-记录内部人员介绍客户的情况数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_CRM_CollaboratorEdit:UserControl
    {
     public tb_CRM_CollaboratorEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_CRM_Collaborator UIToEntity()
        {
        tb_CRM_Collaborator entity = new tb_CRM_Collaborator();
                     entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.Customer_id = Int64.Parse(txtCustomer_id.Text);
                        entity.Notes = txtNotes.Text ;
                       entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                        entity.isdeleted = Boolean.Parse(txtisdeleted.Text);
                                return entity;
}
        */

        
        private tb_CRM_Collaborator _EditEntity;
        public tb_CRM_Collaborator EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_CRM_Collaborator entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v=>v.XXNAME, cmbEmployee_ID);
          // DataBindingHelper.BindData4Cmb<tb_CRM_Customer>(entity, k => k.Customer_id, v=>v.XXNAME, cmbCustomer_id);
           DataBindingHelper.BindData4TextBox<tb_CRM_Collaborator>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<tb_CRM_Collaborator>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_CRM_Collaborator>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_CRM_Collaborator>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_CRM_Collaborator>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CheckBox<tb_CRM_Collaborator>(entity, t => t.isdeleted, chkisdeleted, false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



