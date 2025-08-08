
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:26
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
    /// 数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class View_BOMEdit:UserControl
    {
     public View_BOMEdit() {
     
             
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        View_BOM UIToEntity()
        {
        View_BOM entity = new View_BOM();
                     entity.BOM_ID = Int64.Parse(txtBOM_ID.Text);
                        entity.BOM_No = txtBOM_No.Text ;
                       entity.BOM_Name = txtBOM_Name.Text ;
                       entity.SKU = txtSKU.Text ;
                       entity.Type_ID = Int64.Parse(txtType_ID.Text);
                        entity.CNName = txtCNName.Text ;
                       entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.ProdDetailID = Int64.Parse(txtProdDetailID.Text);
                        entity.DepartmentID = Int64.Parse(txtDepartmentID.Text);
                        entity.Doc_ID = Int64.Parse(txtDoc_ID.Text);
                        entity.BOM_S_VERID = Int64.Parse(txtBOM_S_VERID.Text);
                        entity.Effective_at = DateTime.Parse(txtEffective_at.Text);
                        entity.is_enabled = Boolean.Parse(txtis_enabled.Text);
                        entity.is_available = Boolean.Parse(txtis_available.Text);
                        entity.OutApportionedCost = Decimal.Parse(txtOutApportionedCost.Text);
                        entity.SelfApportionedCost = Decimal.Parse(txtSelfApportionedCost.Text);
                        entity.TotalSelfManuCost = Decimal.Parse(txtTotalSelfManuCost.Text);
                        entity.TotalOutManuCost = Decimal.Parse(txtTotalOutManuCost.Text);
                        entity.TotalMaterialCost = Decimal.Parse(txtTotalMaterialCost.Text);
                        entity.TotalMaterialQty = Decimal.Parse(txtTotalMaterialQty.Text);
                        entity.OutputQty = Decimal.Parse(txtOutputQty.Text);
                        entity.PeopleQty = Decimal.Parse(txtPeopleQty.Text);
                        entity.WorkingHour = Decimal.Parse(txtWorkingHour.Text);
                        entity.MachineHour = Decimal.Parse(txtMachineHour.Text);
                        entity.ExpirationDate = DateTime.Parse(txtExpirationDate.Text);
                        entity.DailyQty = Decimal.Parse(txtDailyQty.Text);
                        entity.SelfProductionAllCosts = Decimal.Parse(txtSelfProductionAllCosts.Text);
                        entity.OutProductionAllCosts = Decimal.Parse(txtOutProductionAllCosts.Text);
                        entity.BOM_Iimage = Binary.Parse(txtBOM_Iimage.Text);
                        entity.Notes = txtNotes.Text ;
                       entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                        entity.isdeleted = Boolean.Parse(txtisdeleted.Text);
                        entity.DataStatus = Int32.Parse(txtDataStatus.Text);
                        entity.ApprovalOpinions = txtApprovalOpinions.Text ;
                       entity.Approver_by = Int64.Parse(txtApprover_by.Text);
                        entity.Approver_at = DateTime.Parse(txtApprover_at.Text);
                        entity.ApprovalStatus = SByte.Parse(txtApprovalStatus.Text);
                        entity.ApprovalResults = Boolean.Parse(txtApprovalResults.Text);
                                return entity;
}
        */

        
        private View_BOM _EditEntity;
        public View_BOM EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(View_BOM entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<View_BOM>(entity, t => t.BOM_ID, txtBOM_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_BOM>(entity, t => t.BOM_No, txtBOM_No, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_BOM>(entity, t => t.BOM_Name, txtBOM_Name, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_BOM>(entity, t => t.SKU, txtSKU, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_BOM>(entity, t => t.Type_ID, txtType_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_BOM>(entity, t => t.CNName, txtCNName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_BOM>(entity, t => t.Employee_ID, txtEmployee_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_BOM>(entity, t => t.ProdDetailID, txtProdDetailID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_BOM>(entity, t => t.DepartmentID, txtDepartmentID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_BOM>(entity, t => t.Doc_ID, txtDoc_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_BOM>(entity, t => t.BOM_S_VERID, txtBOM_S_VERID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<View_BOM>(entity, t => t.Effective_at, dtpEffective_at,false);
           DataBindingHelper.BindData4CheckBox<View_BOM>(entity, t => t.is_enabled, chkis_enabled, false);
           DataBindingHelper.BindData4CheckBox<View_BOM>(entity, t => t.is_available, chkis_available, false);
           DataBindingHelper.BindData4TextBox<View_BOM>(entity, t => t.OutApportionedCost.ToString(), txtOutApportionedCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_BOM>(entity, t => t.SelfApportionedCost.ToString(), txtSelfApportionedCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_BOM>(entity, t => t.TotalSelfManuCost.ToString(), txtTotalSelfManuCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_BOM>(entity, t => t.TotalOutManuCost.ToString(), txtTotalOutManuCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_BOM>(entity, t => t.TotalMaterialCost.ToString(), txtTotalMaterialCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_BOM>(entity, t => t.TotalMaterialQty.ToString(), txtTotalMaterialQty, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_BOM>(entity, t => t.OutputQty.ToString(), txtOutputQty, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_BOM>(entity, t => t.PeopleQty.ToString(), txtPeopleQty, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_BOM>(entity, t => t.WorkingHour.ToString(), txtWorkingHour, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_BOM>(entity, t => t.MachineHour.ToString(), txtMachineHour, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4DataTime<View_BOM>(entity, t => t.ExpirationDate, dtpExpirationDate,false);
           DataBindingHelper.BindData4TextBox<View_BOM>(entity, t => t.DailyQty.ToString(), txtDailyQty, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_BOM>(entity, t => t.SelfProductionAllCosts.ToString(), txtSelfProductionAllCosts, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_BOM>(entity, t => t.OutProductionAllCosts.ToString(), txtOutProductionAllCosts, BindDataType4TextBox.Money,false);
           //default  DataBindingHelper.BindData4TextBox<View_BOM>(entity, t => t.BOM_Iimage.ToString(), txtBOM_Iimage, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<View_BOM>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<View_BOM>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<View_BOM>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<View_BOM>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<View_BOM>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CheckBox<View_BOM>(entity, t => t.isdeleted, chkisdeleted, false);
           DataBindingHelper.BindData4TextBox<View_BOM>(entity, t => t.DataStatus, txtDataStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_BOM>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_BOM>(entity, t => t.Approver_by, txtApprover_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<View_BOM>(entity, t => t.Approver_at, dtpApprover_at,false);
           //default  DataBindingHelper.BindData4TextBox<View_BOM>(entity, t => t.ApprovalStatus.ToString(), txtApprovalStatus, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<View_BOM>(entity, t => t.ApprovalResults, chkApprovalResults, false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



