
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/27/2025 17:49:30
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
    /// 文件版本表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_FS_FileStorageVersionEdit:UserControl
    {
     public tb_FS_FileStorageVersionEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        

         }
/*
        
        tb_FS_FileStorageVersion UIToEntity()
        {
        tb_FS_FileStorageVersion entity = new tb_FS_FileStorageVersion();
                     entity.FileId = Int64.Parse(txtFileId.Text);
                        entity.VersionNo = Int32.Parse(txtVersionNo.Text);
                        entity.UpdateReason = txtUpdateReason.Text ;
                       entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                                return entity;
}
        */

        
        private tb_FS_FileStorageVersion _EditEntity;
        public tb_FS_FileStorageVersion EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_FS_FileStorageVersion entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_FS_FileStorageInfo>(entity, k => k.FileId, v=>v.XXNAME, cmbFileId);
           DataBindingHelper.BindData4TextBox<tb_FS_FileStorageVersion>(entity, t => t.VersionNo, txtVersionNo, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FS_FileStorageVersion>(entity, t => t.UpdateReason, txtUpdateReason, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<tb_FS_FileStorageVersion>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_FS_FileStorageVersion>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_FS_FileStorageVersion>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_FS_FileStorageVersion>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



