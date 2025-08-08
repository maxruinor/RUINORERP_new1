
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:23
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
    /// 收藏表 收藏订单 产品 库存报警等数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_FavoriteEdit:UserControl
    {
     public tb_FavoriteEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_Favorite UIToEntity()
        {
        tb_Favorite entity = new tb_Favorite();
                     entity.ReferenceID = Int64.Parse(txtReferenceID.Text);
                        entity.Ref_Table_Name = txtRef_Table_Name.Text ;
                       entity.ModuleName = txtModuleName.Text ;
                       entity.BusinessType = txtBusinessType.Text ;
                       entity.Public_enabled = Boolean.Parse(txtPublic_enabled.Text);
                        entity.is_enabled = Boolean.Parse(txtis_enabled.Text);
                        entity.is_available = Boolean.Parse(txtis_available.Text);
                        entity.Notes = txtNotes.Text ;
                       entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Owner_by = Int64.Parse(txtOwner_by.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                                return entity;
}
        */

        
        private tb_Favorite _EditEntity;
        public tb_Favorite EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_Favorite entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_Favorite>(entity, t => t.ReferenceID, txtReferenceID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_Favorite>(entity, t => t.Ref_Table_Name, txtRef_Table_Name, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Favorite>(entity, t => t.ModuleName, txtModuleName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Favorite>(entity, t => t.BusinessType, txtBusinessType, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CheckBox<tb_Favorite>(entity, t => t.Public_enabled, chkPublic_enabled, false);
           DataBindingHelper.BindData4CheckBox<tb_Favorite>(entity, t => t.is_enabled, chkis_enabled, false);
           DataBindingHelper.BindData4CheckBox<tb_Favorite>(entity, t => t.is_available, chkis_available, false);
           DataBindingHelper.BindData4TextBox<tb_Favorite>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<tb_Favorite>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_Favorite>(entity, t => t.Owner_by, txtOwner_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_Favorite>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_Favorite>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_Favorite>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



