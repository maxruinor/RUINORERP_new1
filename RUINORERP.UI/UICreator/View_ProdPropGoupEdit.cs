
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:34
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
    /// 数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class View_ProdPropGoupEdit:UserControl
    {
     public View_ProdPropGoupEdit() {
     
             
        
        

         }
/*
        
        View_ProdPropGoup UIToEntity()
        {
        View_ProdPropGoup entity = new View_ProdPropGoup();
                     entity.ProdDetailID = Int64.Parse(txtProdDetailID.Text);
                        entity.prop = txtprop.Text ;
                       entity.ProdBaseID = Int64.Parse(txtProdBaseID.Text);
                                return entity;
}
        */

        
        private View_ProdPropGoup _EditEntity;
        public View_ProdPropGoup EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(View_ProdPropGoup entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<View_ProdPropGoup>(entity, t => t.ProdDetailID, txtProdDetailID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<View_ProdPropGoup>(entity, t => t.prop, txtprop, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<View_ProdPropGoup>(entity, t => t.ProdBaseID, txtProdBaseID, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



