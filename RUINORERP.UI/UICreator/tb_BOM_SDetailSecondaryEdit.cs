
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 11:11:36
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
    /// 标准物料表次级产出明细数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_BOM_SDetailSecondaryEdit:UserControl
    {
     public tb_BOM_SDetailSecondaryEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_BOM_SDetailSecondary UIToEntity()
        {
        tb_BOM_SDetailSecondary entity = new tb_BOM_SDetailSecondary();
                     entity.ProdDetailID = Int64.Parse(txtProdDetailID.Text);
                        entity.BOM_ID = Int64.Parse(txtBOM_ID.Text);
                        entity.Location_ID = Int64.Parse(txtLocation_ID.Text);
                        entity.property = txtproperty.Text ;
                       entity.Qty = Decimal.Parse(txtQty.Text);
                        entity.Scale = Decimal.Parse(txtScale.Text);
                        entity.UnitCost = Decimal.Parse(txtUnitCost.Text);
                        entity.SubtotalCost = Decimal.Parse(txtSubtotalCost.Text);
                        entity.Remarks = txtRemarks.Text ;
                               return entity;
}
        */

        
        private tb_BOM_SDetailSecondary _EditEntity;
        public tb_BOM_SDetailSecondary EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_BOM_SDetailSecondary entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_ProdDetail>(entity, k => k.ProdDetailID, v=>v.XXNAME, cmbProdDetailID);
          // DataBindingHelper.BindData4Cmb<tb_BOM_S>(entity, k => k.BOM_ID, v=>v.XXNAME, cmbBOM_ID);
          // DataBindingHelper.BindData4Cmb<tb_Location>(entity, k => k.Location_ID, v=>v.XXNAME, cmbLocation_ID);
           DataBindingHelper.BindData4TextBox<tb_BOM_SDetailSecondary>(entity, t => t.property, txtproperty, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_BOM_SDetailSecondary>(entity, t => t.Qty.ToString(), txtQty, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_BOM_SDetailSecondary>(entity, t => t.Scale.ToString(), txtScale, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_BOM_SDetailSecondary>(entity, t => t.UnitCost.ToString(), txtUnitCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_BOM_SDetailSecondary>(entity, t => t.SubtotalCost.ToString(), txtSubtotalCost, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_BOM_SDetailSecondary>(entity, t => t.Remarks, txtRemarks, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



