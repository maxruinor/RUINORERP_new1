
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
    /// 出入库类型  非生产领料/退料  借出，归还  报损报溢？单独处理？数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_OutInStockTypeEdit:UserControl
    {
     public tb_OutInStockTypeEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        

         }
/*
        
        tb_OutInStockType UIToEntity()
        {
        tb_OutInStockType entity = new tb_OutInStockType();
                     entity.TypeName = txtTypeName.Text ;
                       entity.TypeDesc = txtTypeDesc.Text ;
                       entity.OutIn = Boolean.Parse(txtOutIn.Text);
                        entity.Is_enabled = Boolean.Parse(txtIs_enabled.Text);
                                return entity;
}
        */

        
        private tb_OutInStockType _EditEntity;
        public tb_OutInStockType EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_OutInStockType entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_OutInStockType>(entity, t => t.TypeName, txtTypeName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_OutInStockType>(entity, t => t.TypeDesc, txtTypeDesc, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CheckBox<tb_OutInStockType>(entity, t => t.OutIn, chkOutIn, false);
           DataBindingHelper.BindData4CheckBox<tb_OutInStockType>(entity, t => t.Is_enabled, chkIs_enabled, false);
//有默认值
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



