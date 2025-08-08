
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:46
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
    /// 订单装箱单，针对特别订单详细处理 也用主子表来做。暂时不搞数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_OrderPackingEdit:UserControl
    {
     public tb_OrderPackingEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_OrderPacking UIToEntity()
        {
        tb_OrderPacking entity = new tb_OrderPacking();
                     entity.SOrder_ID = Int64.Parse(txtSOrder_ID.Text);
                        entity.BoxNo = txtBoxNo.Text ;
                       entity.BoxMark = txtBoxMark.Text ;
                       entity.Remarks = txtRemarks.Text ;
                       entity.QuantityPerBox = Int32.Parse(txtQuantityPerBox.Text);
                        entity.Length = Decimal.Parse(txtLength.Text);
                        entity.Width = Decimal.Parse(txtWidth.Text);
                        entity.Height = Decimal.Parse(txtHeight.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.BoxMaterial = txtBoxMaterial.Text ;
                       entity.Volume = Decimal.Parse(txtVolume.Text);
                        entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                        entity.GrossWeight = Decimal.Parse(txtGrossWeight.Text);
                        entity.NetWeight = Decimal.Parse(txtNetWeight.Text);
                        entity.PackingMethod = txtPackingMethod.Text ;
                               return entity;
}
        */

        
        private tb_OrderPacking _EditEntity;
        public tb_OrderPacking EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_OrderPacking entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_SaleOrder>(entity, k => k.SOrder_ID, v=>v.XXNAME, cmbSOrder_ID);
           DataBindingHelper.BindData4TextBox<tb_OrderPacking>(entity, t => t.BoxNo, txtBoxNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_OrderPacking>(entity, t => t.BoxMark, txtBoxMark, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_OrderPacking>(entity, t => t.Remarks, txtRemarks, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_OrderPacking>(entity, t => t.QuantityPerBox, txtQuantityPerBox, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_OrderPacking>(entity, t => t.Length.ToString(), txtLength, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_OrderPacking>(entity, t => t.Width.ToString(), txtWidth, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_OrderPacking>(entity, t => t.Height.ToString(), txtHeight, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_OrderPacking>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_OrderPacking>(entity, t => t.BoxMaterial, txtBoxMaterial, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_OrderPacking>(entity, t => t.Volume.ToString(), txtVolume, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4DataTime<tb_OrderPacking>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4DataTime<tb_OrderPacking>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_OrderPacking>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_OrderPacking>(entity, t => t.GrossWeight.ToString(), txtGrossWeight, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_OrderPacking>(entity, t => t.NetWeight.ToString(), txtNetWeight, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_OrderPacking>(entity, t => t.PackingMethod, txtPackingMethod, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



