
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:20
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
    /// 盘点表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_StocktakeEdit:UserControl
    {
     public tb_StocktakeEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_Stocktake UIToEntity()
        {
        tb_Stocktake entity = new tb_Stocktake();
                     entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.Location_ID = Int64.Parse(txtLocation_ID.Text);
                        entity.CheckNo = txtCheckNo.Text ;
                       entity.CheckMode = Int32.Parse(txtCheckMode.Text);
                        entity.Adjust_Type = Int32.Parse(txtAdjust_Type.Text);
                        entity.CheckResult = Int32.Parse(txtCheckResult.Text);
                        entity.CarryingTotalQty = Int32.Parse(txtCarryingTotalQty.Text);
                        entity.CarryingTotalAmount = Decimal.Parse(txtCarryingTotalAmount.Text);
                        entity.Check_date = DateTime.Parse(txtCheck_date.Text);
                        entity.CarryingDate = DateTime.Parse(txtCarryingDate.Text);
                        entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                        entity.Notes = txtNotes.Text ;
                       entity.DiffTotalQty = Int32.Parse(txtDiffTotalQty.Text);
                        entity.DiffTotalAmount = Decimal.Parse(txtDiffTotalAmount.Text);
                        entity.CheckTotalQty = Int32.Parse(txtCheckTotalQty.Text);
                        entity.CheckTotalAmount = Decimal.Parse(txtCheckTotalAmount.Text);
                        entity.isdeleted = Boolean.Parse(txtisdeleted.Text);
                        entity.DataStatus = Int32.Parse(txtDataStatus.Text);
                        entity.ApprovalOpinions = txtApprovalOpinions.Text ;
                       entity.Approver_by = Int64.Parse(txtApprover_by.Text);
                        entity.Approver_at = DateTime.Parse(txtApprover_at.Text);
                        entity.ApprovalStatus = SByte.Parse(txtApprovalStatus.Text);
                        entity.ApprovalResults = Boolean.Parse(txtApprovalResults.Text);
                        entity.PrintStatus = Int32.Parse(txtPrintStatus.Text);
                                return entity;
}
        */

        
        private tb_Stocktake _EditEntity;
        public tb_Stocktake EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_Stocktake entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v=>v.XXNAME, cmbEmployee_ID);
          // DataBindingHelper.BindData4Cmb<tb_Location>(entity, k => k.Location_ID, v=>v.XXNAME, cmbLocation_ID);
           DataBindingHelper.BindData4TextBox<tb_Stocktake>(entity, t => t.CheckNo, txtCheckNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Stocktake>(entity, t => t.CheckMode, txtCheckMode, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_Stocktake>(entity, t => t.Adjust_Type, txtAdjust_Type, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_Stocktake>(entity, t => t.CheckResult, txtCheckResult, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_Stocktake>(entity, t => t.CarryingTotalQty, txtCarryingTotalQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_Stocktake>(entity, t => t.CarryingTotalAmount.ToString(), txtCarryingTotalAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4DataTime<tb_Stocktake>(entity, t => t.Check_date, dtpCheck_date,false);
           DataBindingHelper.BindData4DataTime<tb_Stocktake>(entity, t => t.CarryingDate, dtpCarryingDate,false);
           DataBindingHelper.BindData4DataTime<tb_Stocktake>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_Stocktake>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_Stocktake>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_Stocktake>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_Stocktake>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Stocktake>(entity, t => t.DiffTotalQty, txtDiffTotalQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_Stocktake>(entity, t => t.DiffTotalAmount.ToString(), txtDiffTotalAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_Stocktake>(entity, t => t.CheckTotalQty, txtCheckTotalQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_Stocktake>(entity, t => t.CheckTotalAmount.ToString(), txtCheckTotalAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<tb_Stocktake>(entity, t => t.isdeleted, chkisdeleted, false);
           DataBindingHelper.BindData4TextBox<tb_Stocktake>(entity, t => t.DataStatus, txtDataStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_Stocktake>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Stocktake>(entity, t => t.Approver_by, txtApprover_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_Stocktake>(entity, t => t.Approver_at, dtpApprover_at,false);
           //default  DataBindingHelper.BindData4TextBox<tb_Stocktake>(entity, t => t.ApprovalStatus.ToString(), txtApprovalStatus, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<tb_Stocktake>(entity, t => t.ApprovalResults, chkApprovalResults, false);
           DataBindingHelper.BindData4TextBox<tb_Stocktake>(entity, t => t.PrintStatus, txtPrintStatus, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



