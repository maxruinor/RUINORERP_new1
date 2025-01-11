
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/10/2025 18:41:48
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
    /// 用户角色菜单个性化设置表一个角色用户菜单 三个字段为联合主键 就一行数据数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_UIMenuPersonalizationEdit:UserControl
    {
     public tb_UIMenuPersonalizationEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_UIMenuPersonalization UIToEntity()
        {
        tb_UIMenuPersonalization entity = new tb_UIMenuPersonalization();
                     entity.MenuID = Int64.Parse(txtMenuID.Text);
                        entity.UserPersonalizedID = Int64.Parse(txtUserPersonalizedID.Text);
                        entity.QueryConditionCols = Int32.Parse(txtQueryConditionCols.Text);
                        entity.IsRelatedQuerySettings = Boolean.Parse(txtIsRelatedQuerySettings.Text);
                        entity.FavoritesMenu = SByte.Parse(txtFavoritesMenu.Text);
                        entity.Sort = Int32.Parse(txtSort.Text);
                        entity.DefaultLayout = txtDefaultLayout.Text ;
                       entity.DefaultLayout2 = txtDefaultLayout2.Text ;
                               return entity;
}
        */

        
        private tb_UIMenuPersonalization _EditEntity;
        public tb_UIMenuPersonalization EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_UIMenuPersonalization entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_MenuInfo>(entity, k => k.MenuID, v=>v.XXNAME, cmbMenuID);
          // DataBindingHelper.BindData4Cmb<tb_UserPersonalized>(entity, k => k.UserPersonalizedID, v=>v.XXNAME, cmbUserPersonalizedID);
           DataBindingHelper.BindData4TextBox<tb_UIMenuPersonalization>(entity, t => t.QueryConditionCols, txtQueryConditionCols, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CheckBox<tb_UIMenuPersonalization>(entity, t => t.IsRelatedQuerySettings, chkIsRelatedQuerySettings, false);
           //default  DataBindingHelper.BindData4TextBox<tb_UIMenuPersonalization>(entity, t => t.FavoritesMenu.ToString(), txtFavoritesMenu, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_UIMenuPersonalization>(entity, t => t.Sort, txtSort, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_UIMenuPersonalization>(entity, t => t.DefaultLayout, txtDefaultLayout, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_UIMenuPersonalization>(entity, t => t.DefaultLayout2, txtDefaultLayout2, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



