﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/24/2025 20:25:38
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
    /// 维修领料单数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_AS_RepairMaterialPickupEdit:UserControl
    {
     public tb_AS_RepairMaterialPickupEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_AS_RepairMaterialPickup UIToEntity()
        {
        tb_AS_RepairMaterialPickup entity = new tb_AS_RepairMaterialPickup();
                     entity.RepairOrderID = Int64.Parse(txtRepairOrderID.Text);
                        entity.MaterialPickupNO = txtMaterialPickupNO.Text ;
                       entity.DeliveryDate = DateTime.Parse(txtDeliveryDate.Text);
                        entity.RepairOrderNo = txtRepairOrderNo.Text ;
                       entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.TotalPrice = Decimal.Parse(txtTotalPrice.Text);
                        entity.TotalCost = Decimal.Parse(txtTotalCost.Text);
                        entity.TotalReQty = Int32.Parse(txtTotalReQty.Text);
                        entity.TotalSendQty = Decimal.Parse(txtTotalSendQty.Text);
                        entity.ReApply = Boolean.Parse(txtReApply.Text);
                        entity.isdeleted = Boolean.Parse(txtisdeleted.Text);
                        entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                        entity.Notes = txtNotes.Text ;
                       entity.ApprovalOpinions = txtApprovalOpinions.Text ;
                       entity.Approver_by = Int64.Parse(txtApprover_by.Text);
                        entity.Approver_at = DateTime.Parse(txtApprover_at.Text);
                        entity.ApprovalStatus = SByte.Parse(txtApprovalStatus.Text);
                        entity.ApprovalResults = Boolean.Parse(txtApprovalResults.Text);
                        entity.DataStatus = Int32.Parse(txtDataStatus.Text);
                        entity.GeneEvidence = Boolean.Parse(txtGeneEvidence.Text);
                        entity.PrintStatus = Int32.Parse(txtPrintStatus.Text);
                                return entity;
}
        */

        
        private tb_AS_RepairMaterialPickup _EditEntity;
        public tb_AS_RepairMaterialPickup EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_AS_RepairMaterialPickup entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_AS_RepairOrder>(entity, k => k.RepairOrderID, v=>v.XXNAME, cmbRepairOrderID);
           DataBindingHelper.BindData4TextBox<tb_AS_RepairMaterialPickup>(entity, t => t.MaterialPickupNO, txtMaterialPickupNO, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<tb_AS_RepairMaterialPickup>(entity, t => t.DeliveryDate, dtpDeliveryDate,false);
           DataBindingHelper.BindData4TextBox<tb_AS_RepairMaterialPickup>(entity, t => t.RepairOrderNo, txtRepairOrderNo, BindDataType4TextBox.Text,false);
          // DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v=>v.XXNAME, cmbEmployee_ID);
           DataBindingHelper.BindData4TextBox<tb_AS_RepairMaterialPickup>(entity, t => t.TotalPrice.ToString(), txtTotalPrice, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_AS_RepairMaterialPickup>(entity, t => t.TotalCost.ToString(), txtTotalCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_AS_RepairMaterialPickup>(entity, t => t.TotalReQty, txtTotalReQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_AS_RepairMaterialPickup>(entity, t => t.TotalSendQty.ToString(), txtTotalSendQty, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<tb_AS_RepairMaterialPickup>(entity, t => t.ReApply, chkReApply, false);
           DataBindingHelper.BindData4CheckBox<tb_AS_RepairMaterialPickup>(entity, t => t.isdeleted, chkisdeleted, false);
           DataBindingHelper.BindData4DataTime<tb_AS_RepairMaterialPickup>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_AS_RepairMaterialPickup>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_AS_RepairMaterialPickup>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_AS_RepairMaterialPickup>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_AS_RepairMaterialPickup>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_AS_RepairMaterialPickup>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_AS_RepairMaterialPickup>(entity, t => t.Approver_by, txtApprover_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_AS_RepairMaterialPickup>(entity, t => t.Approver_at, dtpApprover_at,false);
           //default  DataBindingHelper.BindData4TextBox<tb_AS_RepairMaterialPickup>(entity, t => t.ApprovalStatus.ToString(), txtApprovalStatus, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<tb_AS_RepairMaterialPickup>(entity, t => t.ApprovalResults, chkApprovalResults, false);
           DataBindingHelper.BindData4TextBox<tb_AS_RepairMaterialPickup>(entity, t => t.DataStatus, txtDataStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CheckBox<tb_AS_RepairMaterialPickup>(entity, t => t.GeneEvidence, chkGeneEvidence, false);
           DataBindingHelper.BindData4TextBox<tb_AS_RepairMaterialPickup>(entity, t => t.PrintStatus, txtPrintStatus, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



