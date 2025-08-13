
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/13/2025 17:09:48
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
    /// 库存流水表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_InventoryTransactionEdit:UserControl
    {
     public tb_InventoryTransactionEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_InventoryTransaction UIToEntity()
        {
        tb_InventoryTransaction entity = new tb_InventoryTransaction();
                     entity.ProdDetailID = Int64.Parse(txtProdDetailID.Text);
                        entity.Location_ID = Int64.Parse(txtLocation_ID.Text);
                        entity.BizType = Int32.Parse(txtBizType.Text);
                        entity.ReferenceId = Int64.Parse(txtReferenceId.Text);
                        entity.QuantityChange = Int32.Parse(txtQuantityChange.Text);
                        entity.AfterQuantity = Int32.Parse(txtAfterQuantity.Text);
                        entity.BatchNumber = Int32.Parse(txtBatchNumber.Text);
                        entity.UnitCost = Decimal.Parse(txtUnitCost.Text);
                        entity.TransactionTime = DateTime.Parse(txtTransactionTime.Text);
                        entity.OperatorId = Int64.Parse(txtOperatorId.Text);
                        entity.Notes = txtNotes.Text ;
                               return entity;
}
        */

        
        private tb_InventoryTransaction _EditEntity;
        public tb_InventoryTransaction EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_InventoryTransaction entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_InventoryTransaction>(entity, t => t.ProdDetailID, txtProdDetailID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_InventoryTransaction>(entity, t => t.Location_ID, txtLocation_ID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_InventoryTransaction>(entity, t => t.BizType, txtBizType, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_InventoryTransaction>(entity, t => t.ReferenceId, txtReferenceId, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_InventoryTransaction>(entity, t => t.QuantityChange, txtQuantityChange, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_InventoryTransaction>(entity, t => t.AfterQuantity, txtAfterQuantity, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_InventoryTransaction>(entity, t => t.BatchNumber, txtBatchNumber, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_InventoryTransaction>(entity, t => t.UnitCost.ToString(), txtUnitCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4DataTime<tb_InventoryTransaction>(entity, t => t.TransactionTime, dtpTransactionTime,false);
           DataBindingHelper.BindData4TextBox<tb_InventoryTransaction>(entity, t => t.OperatorId, txtOperatorId, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_InventoryTransaction>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



