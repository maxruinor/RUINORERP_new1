
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/27/2025 17:49:29
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
    /// 文件信息元数据表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_FS_FileStorageInfoEdit:UserControl
    {
     public tb_FS_FileStorageInfoEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_FS_FileStorageInfo UIToEntity()
        {
        tb_FS_FileStorageInfo entity = new tb_FS_FileStorageInfo();
                     entity.OriginalFileName = txtOriginalFileName.Text ;
                       entity.StorageFileName = txtStorageFileName.Text ;
                       entity.FileExtension = txtFileExtension.Text ;
                       entity.BusinessType = Int32.Parse(txtBusinessType.Text);
                        entity.FileType = txtFileType.Text ;
                       entity.FileSize = Int64.Parse(txtFileSize.Text);
                        entity.HashValue = txtHashValue.Text ;
                       entity.StorageProvider = txtStorageProvider.Text ;
                       entity.StoragePath = txtStoragePath.Text ;
                       entity.CurrentVersion = Int32.Parse(txtCurrentVersion.Text);
                        entity.Status = Int32.Parse(txtStatus.Text);
                        entity.ExpireTime = DateTime.Parse(txtExpireTime.Text);
                        entity.Description = txtDescription.Text ;
                       entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                        entity.Metadata = txtMetadata.Text ;
                               return entity;
}
        */

        
        private tb_FS_FileStorageInfo _EditEntity;
        public tb_FS_FileStorageInfo EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_FS_FileStorageInfo entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_FS_FileStorageInfo>(entity, t => t.OriginalFileName, txtOriginalFileName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FS_FileStorageInfo>(entity, t => t.StorageFileName, txtStorageFileName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FS_FileStorageInfo>(entity, t => t.FileExtension, txtFileExtension, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FS_FileStorageInfo>(entity, t => t.BusinessType, txtBusinessType, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FS_FileStorageInfo>(entity, t => t.FileType, txtFileType, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FS_FileStorageInfo>(entity, t => t.FileSize, txtFileSize, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FS_FileStorageInfo>(entity, t => t.HashValue, txtHashValue, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FS_FileStorageInfo>(entity, t => t.StorageProvider, txtStorageProvider, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FS_FileStorageInfo>(entity, t => t.StoragePath, txtStoragePath, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FS_FileStorageInfo>(entity, t => t.CurrentVersion, txtCurrentVersion, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FS_FileStorageInfo>(entity, t => t.Status, txtStatus, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_FS_FileStorageInfo>(entity, t => t.ExpireTime, dtpExpireTime,false);
           DataBindingHelper.BindData4TextBox<tb_FS_FileStorageInfo>(entity, t => t.Description, txtDescription, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<tb_FS_FileStorageInfo>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_FS_FileStorageInfo>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_FS_FileStorageInfo>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_FS_FileStorageInfo>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FS_FileStorageInfo>(entity, t => t.Metadata, txtMetadata, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



