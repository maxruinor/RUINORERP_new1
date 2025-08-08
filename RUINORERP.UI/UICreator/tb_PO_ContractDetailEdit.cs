
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:49
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
    /// 合同明细数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_PO_ContractDetailEdit:UserControl
    {
     public tb_PO_ContractDetailEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_PO_ContractDetail UIToEntity()
        {
        tb_PO_ContractDetail entity = new tb_PO_ContractDetail();
                     entity.POContractID = Int64.Parse(txtPOContractID.Text);
                        entity.ProdDetailID = Int64.Parse(txtProdDetailID.Text);
                        entity.ItemName = txtItemName.Text ;
                       entity.ItemNumber = txtItemNumber.Text ;
                       entity.Specification = txtSpecification.Text ;
                       entity.Unit = txtUnit.Text ;
                       entity.Quantity = Int32.Parse(txtQuantity.Text);
                        entity.UnitPrice = Decimal.Parse(txtUnitPrice.Text);
                        entity.SubtotalAmount = Decimal.Parse(txtSubtotalAmount.Text);
                        entity.IsIncludeTax = Boolean.Parse(txtIsIncludeTax.Text);
                        entity.TaxRate = Decimal.Parse(txtTaxRate.Text);
                        entity.TaxAmount = Decimal.Parse(txtTaxAmount.Text);
                        entity.Remarks = txtRemarks.Text ;
                       entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                                return entity;
}
        */

        
        private tb_PO_ContractDetail _EditEntity;
        public tb_PO_ContractDetail EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_PO_ContractDetail entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_PO_ContractDetail>(entity, t => t.POContractID, txtPOContractID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_PO_ContractDetail>(entity, t => t.ProdDetailID, txtProdDetailID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_PO_ContractDetail>(entity, t => t.ItemName, txtItemName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_PO_ContractDetail>(entity, t => t.ItemNumber, txtItemNumber, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_PO_ContractDetail>(entity, t => t.Specification, txtSpecification, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_PO_ContractDetail>(entity, t => t.Unit, txtUnit, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_PO_ContractDetail>(entity, t => t.Quantity, txtQuantity, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_PO_ContractDetail>(entity, t => t.UnitPrice.ToString(), txtUnitPrice, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_PO_ContractDetail>(entity, t => t.SubtotalAmount.ToString(), txtSubtotalAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<tb_PO_ContractDetail>(entity, t => t.IsIncludeTax, chkIsIncludeTax, false);
           DataBindingHelper.BindData4TextBox<tb_PO_ContractDetail>(entity, t => t.TaxRate.ToString(), txtTaxRate, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_PO_ContractDetail>(entity, t => t.TaxAmount.ToString(), txtTaxAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_PO_ContractDetail>(entity, t => t.Remarks, txtRemarks, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<tb_PO_ContractDetail>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_PO_ContractDetail>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_PO_ContractDetail>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_PO_ContractDetail>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



