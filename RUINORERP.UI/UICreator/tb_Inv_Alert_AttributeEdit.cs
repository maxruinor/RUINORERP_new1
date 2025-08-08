
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:37
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
    /// 存货预警特性表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_Inv_Alert_AttributeEdit:UserControl
    {
     public tb_Inv_Alert_AttributeEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_Inv_Alert_Attribute UIToEntity()
        {
        tb_Inv_Alert_Attribute entity = new tb_Inv_Alert_Attribute();
                     entity.Inventory_ID = Int64.Parse(txtInventory_ID.Text);
                        entity.AlertPeriod = Int32.Parse(txtAlertPeriod.Text);
                        entity.Max_quantity = Int32.Parse(txtMax_quantity.Text);
                        entity.Min_quantity = Int32.Parse(txtMin_quantity.Text);
                        entity.Alert_Activation = Boolean.Parse(txtAlert_Activation.Text);
                        entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                                return entity;
}
        */

        
        private tb_Inv_Alert_Attribute _EditEntity;
        public tb_Inv_Alert_Attribute EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_Inv_Alert_Attribute entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_Inventory>(entity, k => k.Inventory_ID, v=>v.XXNAME, cmbInventory_ID);
           DataBindingHelper.BindData4TextBox<tb_Inv_Alert_Attribute>(entity, t => t.AlertPeriod, txtAlertPeriod, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_Inv_Alert_Attribute>(entity, t => t.Max_quantity, txtMax_quantity, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_Inv_Alert_Attribute>(entity, t => t.Min_quantity, txtMin_quantity, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CheckBox<tb_Inv_Alert_Attribute>(entity, t => t.Alert_Activation, chkAlert_Activation, false);
           DataBindingHelper.BindData4DataTime<tb_Inv_Alert_Attribute>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_Inv_Alert_Attribute>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_Inv_Alert_Attribute>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_Inv_Alert_Attribute>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



