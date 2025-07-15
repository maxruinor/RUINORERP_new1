
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/11/2025 15:53:34
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
    /// 维修入库单明细数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_AS_RepairInStockDetailEdit:UserControl
    {
     public tb_AS_RepairInStockDetailEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_AS_RepairInStockDetail UIToEntity()
        {
        tb_AS_RepairInStockDetail entity = new tb_AS_RepairInStockDetail();
                     entity.RepairInStockID = Int64.Parse(txtRepairInStockID.Text);
                        entity.ProdDetailID = Int64.Parse(txtProdDetailID.Text);
                        entity.Location_ID = Int64.Parse(txtLocation_ID.Text);
                        entity.Rack_ID = Int64.Parse(txtRack_ID.Text);
                        entity.CustomerPartNo = txtCustomerPartNo.Text ;
                       entity.property = txtproperty.Text ;
                       entity.Quantity = Int32.Parse(txtQuantity.Text);
                        entity.Summary = txtSummary.Text ;
                       entity.RepairOrderDetailID = Int64.Parse(txtRepairOrderDetailID.Text);
                                return entity;
}
        */

        
        private tb_AS_RepairInStockDetail _EditEntity;
        public tb_AS_RepairInStockDetail EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_AS_RepairInStockDetail entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_AS_RepairInStock>(entity, k => k.RepairInStockID, v=>v.XXNAME, cmbRepairInStockID);
          // DataBindingHelper.BindData4Cmb<tb_ProdDetail>(entity, k => k.ProdDetailID, v=>v.XXNAME, cmbProdDetailID);
          // DataBindingHelper.BindData4Cmb<tb_Location>(entity, k => k.Location_ID, v=>v.XXNAME, cmbLocation_ID);
          // DataBindingHelper.BindData4Cmb<tb_StorageRack>(entity, k => k.Rack_ID, v=>v.XXNAME, cmbRack_ID);
           DataBindingHelper.BindData4TextBox<tb_AS_RepairInStockDetail>(entity, t => t.CustomerPartNo, txtCustomerPartNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_AS_RepairInStockDetail>(entity, t => t.property, txtproperty, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_AS_RepairInStockDetail>(entity, t => t.Quantity, txtQuantity, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_AS_RepairInStockDetail>(entity, t => t.Summary, txtSummary, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_AS_RepairInStockDetail>(entity, t => t.RepairOrderDetailID, txtRepairOrderDetailID, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



