
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/14/2024 18:29:32
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
    /// 调拨单-两个仓库之间的库存转移数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_StockTransferEdit:UserControl
    {
     public tb_StockTransferEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_StockTransfer UIToEntity()
        {
        tb_StockTransfer entity = new tb_StockTransfer();
                     entity.Location_ID_from = Int64.Parse(txtLocation_ID_from.Text);
                        entity.Location_ID_to = Int64.Parse(txtLocation_ID_to.Text);
                        entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.StockTransferNo = txtStockTransferNo.Text ;
                       entity.TotalQty = Int32.Parse(txtTotalQty.Text);
                        entity.TotalCost = Decimal.Parse(txtTotalCost.Text);
                        entity.TotalTransferAmount = Decimal.Parse(txtTotalTransferAmount.Text);
                        entity.Bill_Date = DateTime.Parse(txtBill_Date.Text);
                        entity.Out_date = DateTime.Parse(txtOut_date.Text);
                        entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                        entity.Notes = txtNotes.Text ;
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

        
        private tb_StockTransfer _EditEntity;
        public tb_StockTransfer EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_StockTransfer entity)
        {
        _EditEntity = entity;
                       Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_StockTransfer>(entity, t => t.Location_ID_from, txtLocation_ID_from, BindDataType4TextBox.Qty,false);
          Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_StockTransfer>(entity, t => t.Location_ID_to, txtLocation_ID_to, BindDataType4TextBox.Qty,false);
          Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_StockTransfer>(entity, t => t.Employee_ID, txtEmployee_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_StockTransfer>(entity, t => t.StockTransferNo, txtStockTransferNo, BindDataType4TextBox.Text,false);
          Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_StockTransfer>(entity, t => t.TotalQty, txtTotalQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_StockTransfer>(entity, t => t.TotalCost.ToString(), txtTotalCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_StockTransfer>(entity, t => t.TotalTransferAmount.ToString(), txtTotalTransferAmount, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4DataTime<tb_StockTransfer>(entity, t => t.Bill_Date, dtpBill_Date,false);
           DataBindingHelper.BindData4DataTime<tb_StockTransfer>(entity, t => t.Out_date, dtpOut_date,false);
           DataBindingHelper.BindData4DataTime<tb_StockTransfer>(entity, t => t.Created_at, dtpCreated_at,false);
          Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_StockTransfer>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_StockTransfer>(entity, t => t.Modified_at, dtpModified_at,false);
          Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_StockTransfer>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_StockTransfer>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CehckBox<tb_StockTransfer>(entity, t => t.isdeleted, chkisdeleted, false);
          Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_StockTransfer>(entity, t => t.DataStatus, txtDataStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_StockTransfer>(entity, t => t.ApprovalOpinions, txtApprovalOpinions, BindDataType4TextBox.Text,false);
          Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_StockTransfer>(entity, t => t.Approver_by, txtApprover_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_StockTransfer>(entity, t => t.Approver_at, dtpApprover_at,false);
           //default  DataBindingHelper.BindData4TextBox<tb_StockTransfer>(entity, t => t.ApprovalStatus.ToString(), txtApprovalStatus, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CehckBox<tb_StockTransfer>(entity, t => t.ApprovalResults, chkApprovalResults, false);
          Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。 DataBindingHelper.BindData4TextBox<tb_StockTransfer>(entity, t => t.PrintStatus, txtPrintStatus, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



