
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:56
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
    /// 组合单明细数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_ProdMergeDetailEdit:UserControl
    {
     public tb_ProdMergeDetailEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        

         }
/*
        
        tb_ProdMergeDetail UIToEntity()
        {
        tb_ProdMergeDetail entity = new tb_ProdMergeDetail();
                     entity.MergeID = Int64.Parse(txtMergeID.Text);
                        entity.Location_ID = Int64.Parse(txtLocation_ID.Text);
                        entity.ProdDetailID = Int64.Parse(txtProdDetailID.Text);
                        entity.property = txtproperty.Text ;
                       entity.Qty = Int32.Parse(txtQty.Text);
                        entity.Summary = txtSummary.Text ;
                       entity.UnitCost = Decimal.Parse(txtUnitCost.Text);
                                return entity;
}
        */

        
        private tb_ProdMergeDetail _EditEntity;
        public tb_ProdMergeDetail EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_ProdMergeDetail entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_ProdMerge>(entity, k => k.MergeID, v=>v.XXNAME, cmbMergeID);
          // DataBindingHelper.BindData4Cmb<tb_Location>(entity, k => k.Location_ID, v=>v.XXNAME, cmbLocation_ID);
          // DataBindingHelper.BindData4Cmb<tb_ProdDetail>(entity, k => k.ProdDetailID, v=>v.XXNAME, cmbProdDetailID);
           DataBindingHelper.BindData4TextBox<tb_ProdMergeDetail>(entity, t => t.property, txtproperty, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ProdMergeDetail>(entity, t => t.Qty, txtQty, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_ProdMergeDetail>(entity, t => t.Summary, txtSummary, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ProdMergeDetail>(entity, t => t.UnitCost.ToString(), txtUnitCost, BindDataType4TextBox.Money,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



