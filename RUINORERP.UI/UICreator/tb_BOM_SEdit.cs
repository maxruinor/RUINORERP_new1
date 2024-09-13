
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 11:11:34
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
    /// 标准物料表BOM_BillOfMateria_S-要适当冗余? 生产是从0开始的。先有下级才有上级。数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_BOM_SEdit:UserControl
    {
     public tb_BOM_SEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_BOM_S UIToEntity()
        {
        tb_BOM_S entity = new tb_BOM_S();
                     entity.BOM_No = txtBOM_No.Text ;
                       entity.property = txtproperty.Text ;
                       entity.BOM_Name = txtBOM_Name.Text ;
                       entity.SKU = txtSKU.Text ;
                       entity.ProdDetailID = Int64.Parse(txtProdDetailID.Text);
                        entity.DepartmentID = Int64.Parse(txtDepartmentID.Text);
                        entity.Doc_ID = Int64.Parse(txtDoc_ID.Text);
                        entity.BOM_S_VERID = Int64.Parse(txtBOM_S_VERID.Text);
                        entity.Effective_at = DateTime.Parse(txtEffective_at.Text);
                        entity.is_enabled = Boolean.Parse(txtis_enabled.Text);
                        entity.is_available = Boolean.Parse(txtis_available.Text);
                        entity.ManufacturingCost = Decimal.Parse(txtManufacturingCost.Text);
                        entity.OutManuCost = Decimal.Parse(txtOutManuCost.Text);
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

        
        private tb_BOM_S _EditEntity;
        public tb_BOM_S EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_BOM_S entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_BOM_S>(entity, t => t.BOM_No, txtBOM_No, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_BOM_S>(entity, t => t.property, txtproperty, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_BOM_S>(entity, t => t.BOM_Name, txtBOM_Name, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_BOM_S>(entity, t => t.SKU, txtSKU, BindDataType4TextBox.Text,false);
          // DataBindingHelper.BindData4Cmb<tb_ProdDetail>(entity, k => k.ProdDetailID, v=>v.XXNAME, cmbProdDetailID);
          // DataBindingHelper.BindData4Cmb<tb_Department>(entity, k => k.DepartmentID, v=>v.XXNAME, cmbDepartmentID);
          // DataBindingHelper.BindData4Cmb<tb_Files>(entity, k => k.Doc_ID, v=>v.XXNAME, cmbDoc_ID);
          // DataBindingHelper.BindData4Cmb<tb_BOMConfigHistory>(entity, k => k.BOM_S_VERID, v=>v.XXNAME, cmbBOM_S_VERID);
           DataBindingHelper.BindData4DataTime<tb_BOM_S>(entity, t => t.Effective_at, dtpEffective_at,false);
           DataBindingHelper.BindData4CehckBox<tb_BOM_S>(entity, t => t.is_enabled, chkis_enabled, false);
//有默认值
           DataBindingHelper.BindData4CehckBox<tb_BOM_S>(entity, t => t.is_available, chkis_available, false);
//有默认值
           DataBindingHelper.BindData4TextBox<tb_BOM_S>(entity, t => t.ManufacturingCost.ToString(), txtManufacturingCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_BOM_S>(entity, t => t.OutManuCost.ToString(), txtOutManuCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_BOM_S>(entity, t => t.TotalMaterialCost.ToString(), txtTotalMaterialCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_BOM_S>(entity, t => t.TotalMaterialQty.ToString(), txtTotalMaterialQty, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_BOM_S>(entity, t => t.OutputQty.ToString(), txtOutputQty, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_BOM_S>(entity, t => t.PeopleQty.ToString(), txtPeopleQty, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_BOM_S>(entity, t => t.WorkingHour.ToString(), txtWorkingHour, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_BOM_S>(entity, t => t.MachineHour.ToString(), txtMachineHour, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4DataTime<tb_BOM_S>(entity, t => t.ExpirationDate, dtpExpirationDate,false);
           DataBindingHelper.BindData4TextBox<tb_BOM_S>(entity, t => t.DailyQty.ToString(), txtDailyQty, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_BOM_S>(entity, t => t.SelfProductionAllCosts.ToString(), txtSelfProductionAllCosts, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_BOM_S>(entity, t => t.OutProductionAllCosts.ToString(), txtOutProductionAllCosts, BindDataType4TextBox.Money,false);
           //default  DataBindingHelper.BindData4TextBox<tb_BOM_S>(entity, t => t.BOM_Iimage.ToString(), txtBOM_Iimage, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_BOM_S>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<tb_BOM_S>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_BOM_S>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_BOM_S>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_BOM_S>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CehckBox<tb_BOM_S>(entity, t => t.isdeleted, chkisdeleted, false);
           DataBindingHelper.BindData4TextBox<tb_BOM_S>(entity, t => t.DataStatus, txtDataStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_BOM_S>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_BOM_S>(entity, t => t.Approver_by, txtApprover_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_BOM_S>(entity, t => t.Approver_at, dtpApprover_at,false);
           //default  DataBindingHelper.BindData4TextBox<tb_BOM_S>(entity, t => t.ApprovalStatus.ToString(), txtApprovalStatus, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CehckBox<tb_BOM_S>(entity, t => t.ApprovalResults, chkApprovalResults, false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



