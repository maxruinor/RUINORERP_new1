
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:42
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
    /// 退料单(包括生产和托工） 在生产过程中或结束后，我们会根据加工任务（制令单）进行生产退料。这时就需要使用生产退料这个单据进行退料。生产退料单会影响到制令单的直接材料成本，它会冲减该制令单所发生的原料成本数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_MaterialReturnEdit:UserControl
    {
     public tb_MaterialReturnEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_MaterialReturn UIToEntity()
        {
        tb_MaterialReturn entity = new tb_MaterialReturn();
                     entity.BillNo = txtBillNo.Text ;
                       entity.BillType = Int32.Parse(txtBillType.Text);
                        entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.DepartmentID = Int64.Parse(txtDepartmentID.Text);
                        entity.Outgoing = Boolean.Parse(txtOutgoing.Text);
                        entity.CustomerVendor_ID = Int64.Parse(txtCustomerVendor_ID.Text);
                        entity.TotalQty = Decimal.Parse(txtTotalQty.Text);
                        entity.TotalCostAmount = Decimal.Parse(txtTotalCostAmount.Text);
                        entity.TotalAmount = Decimal.Parse(txtTotalAmount.Text);
                        entity.ReturnDate = DateTime.Parse(txtReturnDate.Text);
                        entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                        entity.Notes = txtNotes.Text ;
                       entity.MR_ID = Int64.Parse(txtMR_ID.Text);
                        entity.MaterialRequisitionNO = txtMaterialRequisitionNO.Text ;
                       entity.isdeleted = Boolean.Parse(txtisdeleted.Text);
                        entity.DataStatus = Int32.Parse(txtDataStatus.Text);
                        entity.ApprovalOpinions = txtApprovalOpinions.Text ;
                       entity.Approver_by = Int64.Parse(txtApprover_by.Text);
                        entity.Approver_at = DateTime.Parse(txtApprover_at.Text);
                        entity.ApprovalStatus = SByte.Parse(txtApprovalStatus.Text);
                        entity.GeneEvidence = Boolean.Parse(txtGeneEvidence.Text);
                        entity.ApprovalResults = Boolean.Parse(txtApprovalResults.Text);
                        entity.PrintStatus = Int32.Parse(txtPrintStatus.Text);
                                return entity;
}
        */

        
        private tb_MaterialReturn _EditEntity;
        public tb_MaterialReturn EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_MaterialReturn entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_MaterialReturn>(entity, t => t.BillNo, txtBillNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_MaterialReturn>(entity, t => t.BillType, txtBillType, BindDataType4TextBox.Qty,false);
          // DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v=>v.XXNAME, cmbEmployee_ID);
          // DataBindingHelper.BindData4Cmb<tb_Department>(entity, k => k.DepartmentID, v=>v.XXNAME, cmbDepartmentID);
           DataBindingHelper.BindData4CheckBox<tb_MaterialReturn>(entity, t => t.Outgoing, chkOutgoing, false);
          // DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v=>v.XXNAME, cmbCustomerVendor_ID);
           DataBindingHelper.BindData4TextBox<tb_MaterialReturn>(entity, t => t.TotalQty.ToString(), txtTotalQty, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_MaterialReturn>(entity, t => t.TotalCostAmount.ToString(), txtTotalCostAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_MaterialReturn>(entity, t => t.TotalAmount.ToString(), txtTotalAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4DataTime<tb_MaterialReturn>(entity, t => t.ReturnDate, dtpReturnDate,false);
           DataBindingHelper.BindData4DataTime<tb_MaterialReturn>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_MaterialReturn>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_MaterialReturn>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_MaterialReturn>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_MaterialReturn>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
          // DataBindingHelper.BindData4Cmb<tb_MaterialRequisition>(entity, k => k.MR_ID, v=>v.XXNAME, cmbMR_ID);
           DataBindingHelper.BindData4TextBox<tb_MaterialReturn>(entity, t => t.MaterialRequisitionNO, txtMaterialRequisitionNO, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CheckBox<tb_MaterialReturn>(entity, t => t.isdeleted, chkisdeleted, false);
           DataBindingHelper.BindData4TextBox<tb_MaterialReturn>(entity, t => t.DataStatus, txtDataStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_MaterialReturn>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_MaterialReturn>(entity, t => t.Approver_by, txtApprover_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_MaterialReturn>(entity, t => t.Approver_at, dtpApprover_at,false);
           //default  DataBindingHelper.BindData4TextBox<tb_MaterialReturn>(entity, t => t.ApprovalStatus.ToString(), txtApprovalStatus, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<tb_MaterialReturn>(entity, t => t.GeneEvidence, chkGeneEvidence, false);
           DataBindingHelper.BindData4CheckBox<tb_MaterialReturn>(entity, t => t.ApprovalResults, chkApprovalResults, false);
           DataBindingHelper.BindData4TextBox<tb_MaterialReturn>(entity, t => t.PrintStatus, txtPrintStatus, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



