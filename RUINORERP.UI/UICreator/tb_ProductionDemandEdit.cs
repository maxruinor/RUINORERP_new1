
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:00
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
    /// 生产需求分析表 是一个中间表，由计划生产单或销售订单带入数据来分析，产生采购订单再产生制令单，分析时有三步，库存不足项（包括有成品材料所有项），采购商品建议，自制品成品建议,中间表保存记录而已，操作UI上会有生成采购订单，或生产单等操作数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_ProductionDemandEdit:UserControl
    {
     public tb_ProductionDemandEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_ProductionDemand UIToEntity()
        {
        tb_ProductionDemand entity = new tb_ProductionDemand();
                     entity.PDNo = txtPDNo.Text ;
                       entity.AnalysisDate = DateTime.Parse(txtAnalysisDate.Text);
                        entity.PPNo = txtPPNo.Text ;
                       entity.PPID = Int64.Parse(txtPPID.Text);
                        entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.DataStatus = Int32.Parse(txtDataStatus.Text);
                        entity.ApprovalOpinions = txtApprovalOpinions.Text ;
                       entity.ApprovalStatus = SByte.Parse(txtApprovalStatus.Text);
                        entity.ApprovalResults = Boolean.Parse(txtApprovalResults.Text);
                        entity.Notes = txtNotes.Text ;
                       entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                        entity.Approver_by = Int64.Parse(txtApprover_by.Text);
                        entity.Approver_at = DateTime.Parse(txtApprover_at.Text);
                        entity.PrintStatus = Int32.Parse(txtPrintStatus.Text);
                        entity.PurAllItems = Boolean.Parse(txtPurAllItems.Text);
                        entity.SuggestBasedOn = Boolean.Parse(txtSuggestBasedOn.Text);
                        entity.isdeleted = Boolean.Parse(txtisdeleted.Text);
                                return entity;
}
        */

        
        private tb_ProductionDemand _EditEntity;
        public tb_ProductionDemand EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_ProductionDemand entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_ProductionDemand>(entity, t => t.PDNo, txtPDNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<tb_ProductionDemand>(entity, t => t.AnalysisDate, dtpAnalysisDate,false);
           DataBindingHelper.BindData4TextBox<tb_ProductionDemand>(entity, t => t.PPNo, txtPPNo, BindDataType4TextBox.Text,false);
          // DataBindingHelper.BindData4Cmb<tb_ProductionPlan>(entity, k => k.PPID, v=>v.XXNAME, cmbPPID);
          // DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v=>v.XXNAME, cmbEmployee_ID);
           DataBindingHelper.BindData4TextBox<tb_ProductionDemand>(entity, t => t.DataStatus, txtDataStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_ProductionDemand>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text,false);
           //default  DataBindingHelper.BindData4TextBox<tb_ProductionDemand>(entity, t => t.ApprovalStatus.ToString(), txtApprovalStatus, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<tb_ProductionDemand>(entity, t => t.ApprovalResults, chkApprovalResults, false);
           DataBindingHelper.BindData4TextBox<tb_ProductionDemand>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<tb_ProductionDemand>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_ProductionDemand>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_ProductionDemand>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_ProductionDemand>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_ProductionDemand>(entity, t => t.Approver_by, txtApprover_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_ProductionDemand>(entity, t => t.Approver_at, dtpApprover_at,false);
           DataBindingHelper.BindData4TextBox<tb_ProductionDemand>(entity, t => t.PrintStatus, txtPrintStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CheckBox<tb_ProductionDemand>(entity, t => t.PurAllItems, chkPurAllItems, false);
           DataBindingHelper.BindData4CheckBox<tb_ProductionDemand>(entity, t => t.SuggestBasedOn, chkSuggestBasedOn, false);
           DataBindingHelper.BindData4CheckBox<tb_ProductionDemand>(entity, t => t.isdeleted, chkisdeleted, false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



