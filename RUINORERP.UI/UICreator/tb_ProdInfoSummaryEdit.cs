
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
    /// 商品信息汇总数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_ProdInfoSummaryEdit:UserControl
    {
     public tb_ProdInfoSummaryEdit() {
     
                         InitializeComponent();
      
        
        
        
        

         }
/*
        
        tb_ProdInfoSummary UIToEntity()
        {
        tb_ProdInfoSummary entity = new tb_ProdInfoSummary();
                     entity.平均价格 = Decimal.Parse(txt平均价格.Text);
                        entity.总销售量 = Int32.Parse(txt总销售量.Text);
                        entity.库存总量 = Int32.Parse(txt库存总量.Text);
                                return entity;
}
        */

        
        private tb_ProdInfoSummary _EditEntity;
        public tb_ProdInfoSummary EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_ProdInfoSummary entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_ProdInfoSummary>(entity, t => t.平均价格.ToString(), txt平均价格, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_ProdInfoSummary>(entity, t => t.总销售量, txt总销售量, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_ProdInfoSummary>(entity, t => t.库存总量, txt库存总量, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



