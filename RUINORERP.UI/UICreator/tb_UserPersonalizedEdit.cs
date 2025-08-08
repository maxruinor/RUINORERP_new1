
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:25
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
    /// 用户角色个性化设置表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_UserPersonalizedEdit:UserControl
    {
     public tb_UserPersonalizedEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        

         }
/*
        
        tb_UserPersonalized UIToEntity()
        {
        tb_UserPersonalized entity = new tb_UserPersonalized();
                     entity.WorkCellSettings = txtWorkCellSettings.Text ;
                       entity.WorkCellLayout = txtWorkCellLayout.Text ;
                       entity.ID = Int64.Parse(txtID.Text);
                        entity.UseUserOwnPrinter = Boolean.Parse(txtUseUserOwnPrinter.Text);
                        entity.PrinterName = txtPrinterName.Text ;
                       entity.SelectTemplatePrint = Boolean.Parse(txtSelectTemplatePrint.Text);
                        entity.UserFavoriteMenu = txtUserFavoriteMenu.Text ;
                               return entity;
}
        */

        
        private tb_UserPersonalized _EditEntity;
        public tb_UserPersonalized EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_UserPersonalized entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_UserPersonalized>(entity, t => t.WorkCellSettings, txtWorkCellSettings, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_UserPersonalized>(entity, t => t.WorkCellLayout, txtWorkCellLayout, BindDataType4TextBox.Text,false);
          // DataBindingHelper.BindData4Cmb<tb_User_Role>(entity, k => k.ID, v=>v.XXNAME, cmbID);
           DataBindingHelper.BindData4CheckBox<tb_UserPersonalized>(entity, t => t.UseUserOwnPrinter, chkUseUserOwnPrinter, false);
           DataBindingHelper.BindData4TextBox<tb_UserPersonalized>(entity, t => t.PrinterName, txtPrinterName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CheckBox<tb_UserPersonalized>(entity, t => t.SelectTemplatePrint, chkSelectTemplatePrint, false);
           DataBindingHelper.BindData4TextBox<tb_UserPersonalized>(entity, t => t.UserFavoriteMenu, txtUserFavoriteMenu, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



