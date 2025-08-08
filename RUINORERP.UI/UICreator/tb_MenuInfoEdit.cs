
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:42
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
    /// 菜单程序集信息表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_MenuInfoEdit:UserControl
    {
     public tb_MenuInfoEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_MenuInfo UIToEntity()
        {
        tb_MenuInfo entity = new tb_MenuInfo();
                     entity.ModuleID = Int64.Parse(txtModuleID.Text);
                        entity.MenuName = txtMenuName.Text ;
                       entity.MenuType = txtMenuType.Text ;
                       entity.UIPropertyIdentifier = txtUIPropertyIdentifier.Text ;
                       entity.BizInterface = txtBizInterface.Text ;
                       entity.BIBizBaseForm = txtBIBizBaseForm.Text ;
                       entity.BIBaseForm = txtBIBaseForm.Text ;
                       entity.BizType = Int32.Parse(txtBizType.Text);
                        entity.UIType = Int32.Parse(txtUIType.Text);
                        entity.CaptionCN = txtCaptionCN.Text ;
                       entity.CaptionEN = txtCaptionEN.Text ;
                       entity.FormName = txtFormName.Text ;
                       entity.ClassPath = txtClassPath.Text ;
                       entity.EntityName = txtEntityName.Text ;
                       entity.IsVisble = Boolean.Parse(txtIsVisble.Text);
                        entity.IsEnabled = Boolean.Parse(txtIsEnabled.Text);
                        entity.Parent_id = Int64.Parse(txtParent_id.Text);
                        entity.Discription = txtDiscription.Text ;
                       entity.MenuNo = txtMenuNo.Text ;
                       entity.MenuLevel = Int32.Parse(txtMenuLevel.Text);
                        entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                        entity.Sort = Int32.Parse(txtSort.Text);
                        entity.HotKey = txtHotKey.Text ;
                       entity.DefaultLayout = txtDefaultLayout.Text ;
                               return entity;
}
        */

        
        private tb_MenuInfo _EditEntity;
        public tb_MenuInfo EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_MenuInfo entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_ModuleDefinition>(entity, k => k.ModuleID, v=>v.XXNAME, cmbModuleID);
           DataBindingHelper.BindData4TextBox<tb_MenuInfo>(entity, t => t.MenuName, txtMenuName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_MenuInfo>(entity, t => t.MenuType, txtMenuType, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_MenuInfo>(entity, t => t.UIPropertyIdentifier, txtUIPropertyIdentifier, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_MenuInfo>(entity, t => t.BizInterface, txtBizInterface, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_MenuInfo>(entity, t => t.BIBizBaseForm, txtBIBizBaseForm, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_MenuInfo>(entity, t => t.BIBaseForm, txtBIBaseForm, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_MenuInfo>(entity, t => t.BizType, txtBizType, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_MenuInfo>(entity, t => t.UIType, txtUIType, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_MenuInfo>(entity, t => t.CaptionCN, txtCaptionCN, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_MenuInfo>(entity, t => t.CaptionEN, txtCaptionEN, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_MenuInfo>(entity, t => t.FormName, txtFormName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_MenuInfo>(entity, t => t.ClassPath, txtClassPath, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_MenuInfo>(entity, t => t.EntityName, txtEntityName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CheckBox<tb_MenuInfo>(entity, t => t.IsVisble, chkIsVisble, false);
           DataBindingHelper.BindData4CheckBox<tb_MenuInfo>(entity, t => t.IsEnabled, chkIsEnabled, false);
//有默认值
           DataBindingHelper.BindData4TextBox<tb_MenuInfo>(entity, t => t.Parent_id, txtParent_id, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_MenuInfo>(entity, t => t.Discription, txtDiscription, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_MenuInfo>(entity, t => t.MenuNo, txtMenuNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_MenuInfo>(entity, t => t.MenuLevel, txtMenuLevel, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_MenuInfo>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_MenuInfo>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_MenuInfo>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_MenuInfo>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_MenuInfo>(entity, t => t.Sort, txtSort, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_MenuInfo>(entity, t => t.HotKey, txtHotKey, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_MenuInfo>(entity, t => t.DefaultLayout, txtDefaultLayout, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



