
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 11:11:32
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
    /// 审核配置表 对于所有单据审核，并且提供明细，每个明细通过则主表通过主表中对应一个业务单据的主ID https://www.likecs.com/show-747870.html 数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_ApprovalEdit:UserControl
    {
     public tb_ApprovalEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        

         }
/*
        
        tb_Approval UIToEntity()
        {
        tb_Approval entity = new tb_Approval();
                     entity.BillType = txtBillType.Text ;
                       entity.BillName = txtBillName.Text ;
                       entity.BillEntityClassName = txtBillEntityClassName.Text ;
                       entity.ApprovalResults = Int32.Parse(txtApprovalResults.Text);
                        entity.GradedAudit = Boolean.Parse(txtGradedAudit.Text);
                        entity.Module = Int32.Parse(txtModule.Text);
                                return entity;
}
        */

        
        private tb_Approval _EditEntity;
        public tb_Approval EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_Approval entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_Approval>(entity, t => t.BillType, txtBillType, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Approval>(entity, t => t.BillName, txtBillName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Approval>(entity, t => t.BillEntityClassName, txtBillEntityClassName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Approval>(entity, t => t.ApprovalResults, txtApprovalResults, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CehckBox<tb_Approval>(entity, t => t.GradedAudit, chkGradedAudit, false);
           DataBindingHelper.BindData4TextBox<tb_Approval>(entity, t => t.Module, txtModule, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



