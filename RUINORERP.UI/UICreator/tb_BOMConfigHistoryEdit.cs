
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/13/2024 11:11:36
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
    /// BOM配置历史 数据保存在BOM中 只是多份一样，细微区别用版本号标识数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_BOMConfigHistoryEdit:UserControl
    {
     public tb_BOMConfigHistoryEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_BOMConfigHistory UIToEntity()
        {
        tb_BOMConfigHistory entity = new tb_BOMConfigHistory();
                     entity.VerNo = txtVerNo.Text ;
                       entity.Effective_at = DateTime.Parse(txtEffective_at.Text);
                        entity.is_enabled = Boolean.Parse(txtis_enabled.Text);
                        entity.is_available = Boolean.Parse(txtis_available.Text);
                        entity.Notes = txtNotes.Text ;
                       entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                                return entity;
}
        */

        
        private tb_BOMConfigHistory _EditEntity;
        public tb_BOMConfigHistory EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_BOMConfigHistory entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_BOMConfigHistory>(entity, t => t.VerNo, txtVerNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<tb_BOMConfigHistory>(entity, t => t.Effective_at, dtpEffective_at,false);
           DataBindingHelper.BindData4CehckBox<tb_BOMConfigHistory>(entity, t => t.is_enabled, chkis_enabled, false);
           DataBindingHelper.BindData4CehckBox<tb_BOMConfigHistory>(entity, t => t.is_available, chkis_available, false);
           DataBindingHelper.BindData4TextBox<tb_BOMConfigHistory>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<tb_BOMConfigHistory>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_BOMConfigHistory>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_BOMConfigHistory>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_BOMConfigHistory>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



