
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:38
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
    /// 制令单-生产工单 ，工单(MO)各种企业的叫法不一样，比如生产单，制令单，生产指导单，裁单，等等。其实都是同一个东西–MO,    来源于 销售订单，计划单，生产需求单，我在下文都以工单简称。https://blog.csdn.net/qq_37365475/article/details/106612960数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_ManufacturingOrderEdit:UserControl
    {
     public tb_ManufacturingOrderEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_ManufacturingOrder UIToEntity()
        {
        tb_ManufacturingOrder entity = new tb_ManufacturingOrder();
                     entity.MONO = txtMONO.Text ;
                       entity.PDNO = txtPDNO.Text ;
                       entity.PDCID = Int64.Parse(txtPDCID.Text);
                        entity.PDID = Int64.Parse(txtPDID.Text);
                        entity.ProdDetailID = Int64.Parse(txtProdDetailID.Text);
                        entity.SKU = txtSKU.Text ;
                       entity.Specifications = txtSpecifications.Text ;
                       entity.property = txtproperty.Text ;
                       entity.CNName = txtCNName.Text ;
                       entity.QuantityDelivered = Int32.Parse(txtQuantityDelivered.Text);
                        entity.ManufacturingQty = Int32.Parse(txtManufacturingQty.Text);
                        entity.Priority = Int32.Parse(txtPriority.Text);
                        entity.PreStartDate = DateTime.Parse(txtPreStartDate.Text);
                        entity.PreEndDate = DateTime.Parse(txtPreEndDate.Text);
                        entity.BOM_ID = Int64.Parse(txtBOM_ID.Text);
                        entity.BOM_No = txtBOM_No.Text ;
                       entity.Type_ID = Int64.Parse(txtType_ID.Text);
                        entity.Unit_ID = Int64.Parse(txtUnit_ID.Text);
                        entity.CustomerPartNo = txtCustomerPartNo.Text ;
                       entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.Location_ID = Int64.Parse(txtLocation_ID.Text);
                        entity.DepartmentID = Int64.Parse(txtDepartmentID.Text);
                        entity.CustomerVendor_ID_Out = Int64.Parse(txtCustomerVendor_ID_Out.Text);
                        entity.CustomerVendor_ID = Int64.Parse(txtCustomerVendor_ID.Text);
                        entity.IsCustomizedOrder = Boolean.Parse(txtIsCustomizedOrder.Text);
                        entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                        entity.CloseCaseOpinions = txtCloseCaseOpinions.Text ;
                       entity.Notes = txtNotes.Text ;
                       entity.ApportionedCost = Decimal.Parse(txtApportionedCost.Text);
                        entity.TotalManuFee = Decimal.Parse(txtTotalManuFee.Text);
                        entity.TotalMaterialCost = Decimal.Parse(txtTotalMaterialCost.Text);
                        entity.TotalProductionCost = Decimal.Parse(txtTotalProductionCost.Text);
                        entity.PeopleQty = Decimal.Parse(txtPeopleQty.Text);
                        entity.WorkingHour = Decimal.Parse(txtWorkingHour.Text);
                        entity.MachineHour = Decimal.Parse(txtMachineHour.Text);
                        entity.IncludeSubBOM = Boolean.Parse(txtIncludeSubBOM.Text);
                        entity.IsOutSourced = Boolean.Parse(txtIsOutSourced.Text);
                        entity.isdeleted = Boolean.Parse(txtisdeleted.Text);
                        entity.Approver_by = Int64.Parse(txtApprover_by.Text);
                        entity.Approver_at = DateTime.Parse(txtApprover_at.Text);
                        entity.ApprovalOpinions = txtApprovalOpinions.Text ;
                       entity.ApproverTime = DateTime.Parse(txtApproverTime.Text);
                        entity.ApprovalStatus = SByte.Parse(txtApprovalStatus.Text);
                        entity.DataStatus = Int32.Parse(txtDataStatus.Text);
                        entity.ApprovalResults = Boolean.Parse(txtApprovalResults.Text);
                        entity.PrintStatus = Int32.Parse(txtPrintStatus.Text);
                                return entity;
}
        */

        
        private tb_ManufacturingOrder _EditEntity;
        public tb_ManufacturingOrder EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_ManufacturingOrder entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_ManufacturingOrder>(entity, t => t.MONO, txtMONO, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ManufacturingOrder>(entity, t => t.PDNO, txtPDNO, BindDataType4TextBox.Text,false);
          CustomerVendor_ID_Out主外字段不一致。// DataBindingHelper.BindData4Cmb<tb_ProduceGoodsRecommendDetail>(entity, k => k.PDCID, v=>v.XXNAME, cmbPDCID);
          CustomerVendor_ID_Out主外字段不一致。// DataBindingHelper.BindData4Cmb<tb_ProductionDemand>(entity, k => k.PDID, v=>v.XXNAME, cmbPDID);
          CustomerVendor_ID_Out主外字段不一致。// DataBindingHelper.BindData4Cmb<tb_ProdDetail>(entity, k => k.ProdDetailID, v=>v.XXNAME, cmbProdDetailID);
           DataBindingHelper.BindData4TextBox<tb_ManufacturingOrder>(entity, t => t.SKU, txtSKU, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ManufacturingOrder>(entity, t => t.Specifications, txtSpecifications, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ManufacturingOrder>(entity, t => t.property, txtproperty, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ManufacturingOrder>(entity, t => t.CNName, txtCNName, BindDataType4TextBox.Text,false);
          CustomerVendor_ID_Out主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_ManufacturingOrder>(entity, t => t.QuantityDelivered, txtQuantityDelivered, BindDataType4TextBox.Qty,false);
          CustomerVendor_ID_Out主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_ManufacturingOrder>(entity, t => t.ManufacturingQty, txtManufacturingQty, BindDataType4TextBox.Qty,false);
          CustomerVendor_ID_Out主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_ManufacturingOrder>(entity, t => t.Priority, txtPriority, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_ManufacturingOrder>(entity, t => t.PreStartDate, dtpPreStartDate,false);
           DataBindingHelper.BindData4DataTime<tb_ManufacturingOrder>(entity, t => t.PreEndDate, dtpPreEndDate,false);
          CustomerVendor_ID_Out主外字段不一致。// DataBindingHelper.BindData4Cmb<tb_BOM_S>(entity, k => k.BOM_ID, v=>v.XXNAME, cmbBOM_ID);
           DataBindingHelper.BindData4TextBox<tb_ManufacturingOrder>(entity, t => t.BOM_No, txtBOM_No, BindDataType4TextBox.Text,false);
          CustomerVendor_ID_Out主外字段不一致。// DataBindingHelper.BindData4Cmb<tb_ProductType>(entity, k => k.Type_ID, v=>v.XXNAME, cmbType_ID);
          CustomerVendor_ID_Out主外字段不一致。// DataBindingHelper.BindData4Cmb<tb_Unit>(entity, k => k.Unit_ID, v=>v.XXNAME, cmbUnit_ID);
           DataBindingHelper.BindData4TextBox<tb_ManufacturingOrder>(entity, t => t.CustomerPartNo, txtCustomerPartNo, BindDataType4TextBox.Text,false);
          CustomerVendor_ID_Out主外字段不一致。// DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v=>v.XXNAME, cmbEmployee_ID);
          CustomerVendor_ID_Out主外字段不一致。// DataBindingHelper.BindData4Cmb<tb_Location>(entity, k => k.Location_ID, v=>v.XXNAME, cmbLocation_ID);
          CustomerVendor_ID_Out主外字段不一致。// DataBindingHelper.BindData4Cmb<tb_Department>(entity, k => k.DepartmentID, v=>v.XXNAME, cmbDepartmentID);
          CustomerVendor_ID_Out主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_ManufacturingOrder>(entity, t => t.CustomerVendor_ID_Out, txtCustomerVendor_ID_Out, BindDataType4TextBox.Qty,false);
          CustomerVendor_ID_Out主外字段不一致。// DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v=>v.XXNAME, cmbCustomerVendor_ID);
// DataBindingHelper.BindData4Cmb<tb_CustomerVendor>(entity, k => k.CustomerVendor_ID, v=>v.XXNAME, cmbCustomerVendor_ID);
           DataBindingHelper.BindData4CheckBox<tb_ManufacturingOrder>(entity, t => t.IsCustomizedOrder, chkIsCustomizedOrder, false);
           DataBindingHelper.BindData4DataTime<tb_ManufacturingOrder>(entity, t => t.Created_at, dtpCreated_at,false);
          CustomerVendor_ID_Out主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_ManufacturingOrder>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_ManufacturingOrder>(entity, t => t.Modified_at, dtpModified_at,false);
          CustomerVendor_ID_Out主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_ManufacturingOrder>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_ManufacturingOrder>(entity, t => t.CloseCaseOpinions, txtCloseCaseOpinions, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ManufacturingOrder>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ManufacturingOrder>(entity, t => t.ApportionedCost.ToString(), txtApportionedCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_ManufacturingOrder>(entity, t => t.TotalManuFee.ToString(), txtTotalManuFee, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_ManufacturingOrder>(entity, t => t.TotalMaterialCost.ToString(), txtTotalMaterialCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_ManufacturingOrder>(entity, t => t.TotalProductionCost.ToString(), txtTotalProductionCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_ManufacturingOrder>(entity, t => t.PeopleQty.ToString(), txtPeopleQty, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_ManufacturingOrder>(entity, t => t.WorkingHour.ToString(), txtWorkingHour, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_ManufacturingOrder>(entity, t => t.MachineHour.ToString(), txtMachineHour, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<tb_ManufacturingOrder>(entity, t => t.IncludeSubBOM, chkIncludeSubBOM, false);
           DataBindingHelper.BindData4CheckBox<tb_ManufacturingOrder>(entity, t => t.IsOutSourced, chkIsOutSourced, false);
           DataBindingHelper.BindData4CheckBox<tb_ManufacturingOrder>(entity, t => t.isdeleted, chkisdeleted, false);
          CustomerVendor_ID_Out主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_ManufacturingOrder>(entity, t => t.Approver_by, txtApprover_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_ManufacturingOrder>(entity, t => t.Approver_at, dtpApprover_at,false);
           DataBindingHelper.BindData4TextBox<tb_ManufacturingOrder>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<tb_ManufacturingOrder>(entity, t => t.ApproverTime, dtpApproverTime,false);
           //default  DataBindingHelper.BindData4TextBox<tb_ManufacturingOrder>(entity, t => t.ApprovalStatus.ToString(), txtApprovalStatus, BindDataType4TextBox.Money,false);
          CustomerVendor_ID_Out主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_ManufacturingOrder>(entity, t => t.DataStatus, txtDataStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CheckBox<tb_ManufacturingOrder>(entity, t => t.ApprovalResults, chkApprovalResults, false);
          CustomerVendor_ID_Out主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_ManufacturingOrder>(entity, t => t.PrintStatus, txtPrintStatus, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



