
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:17
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
    /// 成本单价表 参考天思货品基本资料中的价格部分数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_CostUnitPriceEdit:UserControl
    {
     public tb_CostUnitPriceEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_CostUnitPrice UIToEntity()
        {
        tb_CostUnitPrice entity = new tb_CostUnitPrice();
                     entity.ProdDetailID = Int64.Parse(txtProdDetailID.Text);
                        entity.Group_id = Int64.Parse(txtGroup_id.Text);
                        entity.SpecInstructions = txtSpecInstructions.Text ;
                       entity.Notes = txtNotes.Text ;
                       entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                                return entity;
}
        */

        
        private tb_CostUnitPrice _EditEntity;
        public tb_CostUnitPrice EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_CostUnitPrice entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_CostUnitPrice>(entity, t => t.ProdDetailID, txtProdDetailID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_CostUnitPrice>(entity, t => t.Group_id, txtGroup_id, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_CostUnitPrice>(entity, t => t.SpecInstructions, txtSpecInstructions, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_CostUnitPrice>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<tb_CostUnitPrice>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_CostUnitPrice>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_CostUnitPrice>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_CostUnitPrice>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



