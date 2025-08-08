
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:58
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
    /// 产品拆分单数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_ProdSplitEdit:UserControl
    {
     public tb_ProdSplitEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_ProdSplit UIToEntity()
        {
        tb_ProdSplit entity = new tb_ProdSplit();
                     entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.Location_ID = Int64.Parse(txtLocation_ID.Text);
                        entity.SplitNo = txtSplitNo.Text ;
                       entity.SplitDate = DateTime.Parse(txtSplitDate.Text);
                        entity.ProdDetailID = Int64.Parse(txtProdDetailID.Text);
                        entity.SKU = txtSKU.Text ;
                       entity.SplitParentQty = Int32.Parse(txtSplitParentQty.Text);
                        entity.SplitChildTotalQty = Int32.Parse(txtSplitChildTotalQty.Text);
                        entity.property = txtproperty.Text ;
                       entity.Notes = txtNotes.Text ;
                       entity.BOM_ID = Int64.Parse(txtBOM_ID.Text);
                        entity.BOM_No = txtBOM_No.Text ;
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
                        entity.PrintStatus = Int32.Parse(txtPrintStatus.Text);
                                return entity;
}
        */

        
        private tb_ProdSplit _EditEntity;
        public tb_ProdSplit EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_ProdSplit entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v=>v.XXNAME, cmbEmployee_ID);
          // DataBindingHelper.BindData4Cmb<tb_Location>(entity, k => k.Location_ID, v=>v.XXNAME, cmbLocation_ID);
           DataBindingHelper.BindData4TextBox<tb_ProdSplit>(entity, t => t.SplitNo, txtSplitNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<tb_ProdSplit>(entity, t => t.SplitDate, dtpSplitDate,false);
          // DataBindingHelper.BindData4Cmb<tb_ProdDetail>(entity, k => k.ProdDetailID, v=>v.XXNAME, cmbProdDetailID);
           DataBindingHelper.BindData4TextBox<tb_ProdSplit>(entity, t => t.SKU, txtSKU, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ProdSplit>(entity, t => t.SplitParentQty, txtSplitParentQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_ProdSplit>(entity, t => t.SplitChildTotalQty, txtSplitChildTotalQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_ProdSplit>(entity, t => t.property, txtproperty, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ProdSplit>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
          // DataBindingHelper.BindData4Cmb<tb_BOM_S>(entity, k => k.BOM_ID, v=>v.XXNAME, cmbBOM_ID);
           DataBindingHelper.BindData4TextBox<tb_ProdSplit>(entity, t => t.BOM_No, txtBOM_No, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<tb_ProdSplit>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_ProdSplit>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_ProdSplit>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_ProdSplit>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CheckBox<tb_ProdSplit>(entity, t => t.isdeleted, chkisdeleted, false);
           DataBindingHelper.BindData4TextBox<tb_ProdSplit>(entity, t => t.DataStatus, txtDataStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_ProdSplit>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ProdSplit>(entity, t => t.Approver_by, txtApprover_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_ProdSplit>(entity, t => t.Approver_at, dtpApprover_at,false);
           //default  DataBindingHelper.BindData4TextBox<tb_ProdSplit>(entity, t => t.ApprovalStatus.ToString(), txtApprovalStatus, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<tb_ProdSplit>(entity, t => t.ApprovalResults, chkApprovalResults, false);
           DataBindingHelper.BindData4TextBox<tb_ProdSplit>(entity, t => t.PrintStatus, txtPrintStatus, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



