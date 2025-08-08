
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:50
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
    /// 报表打印配置表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_PrintConfigEdit:UserControl
    {
     public tb_PrintConfigEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_PrintConfig UIToEntity()
        {
        tb_PrintConfig entity = new tb_PrintConfig();
                     entity.Config_Name = txtConfig_Name.Text ;
                       entity.BizType = Int32.Parse(txtBizType.Text);
                        entity.BizName = txtBizName.Text ;
                       entity.PrinterName = txtPrinterName.Text ;
                       entity.PrinterSelected = Boolean.Parse(txtPrinterSelected.Text);
                        entity.Landscape = Boolean.Parse(txtLandscape.Text);
                        entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                        entity.isdeleted = Boolean.Parse(txtisdeleted.Text);
                                return entity;
}
        */

        
        private tb_PrintConfig _EditEntity;
        public tb_PrintConfig EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_PrintConfig entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_PrintConfig>(entity, t => t.Config_Name, txtConfig_Name, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_PrintConfig>(entity, t => t.BizType, txtBizType, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_PrintConfig>(entity, t => t.BizName, txtBizName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_PrintConfig>(entity, t => t.PrinterName, txtPrinterName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CheckBox<tb_PrintConfig>(entity, t => t.PrinterSelected, chkPrinterSelected, false);
           DataBindingHelper.BindData4CheckBox<tb_PrintConfig>(entity, t => t.Landscape, chkLandscape, false);
           DataBindingHelper.BindData4DataTime<tb_PrintConfig>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_PrintConfig>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_PrintConfig>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_PrintConfig>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CheckBox<tb_PrintConfig>(entity, t => t.isdeleted, chkisdeleted, false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



