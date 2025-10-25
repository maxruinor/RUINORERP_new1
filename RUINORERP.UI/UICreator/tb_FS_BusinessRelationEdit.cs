﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/25/2025 15:32:17
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
    /// 文件业务关联表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_FS_BusinessRelationEdit:UserControl
    {
     public tb_FS_BusinessRelationEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        

         }
/*
        
        tb_FS_BusinessRelation UIToEntity()
        {
        tb_FS_BusinessRelation entity = new tb_FS_BusinessRelation();
                     entity.FileId = Int64.Parse(txtFileId.Text);
                        entity.BusinessType = Int32.Parse(txtBusinessType.Text);
                        entity.BusinessNo = txtBusinessNo.Text ;
                       entity.IsMainFile = Boolean.Parse(txtIsMainFile.Text);
                                return entity;
}
        */

        
        private tb_FS_BusinessRelation _EditEntity;
        public tb_FS_BusinessRelation EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_FS_BusinessRelation entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_FS_FileStorageInfo>(entity, k => k.FileId, v=>v.XXNAME, cmbFileId);
           DataBindingHelper.BindData4TextBox<tb_FS_BusinessRelation>(entity, t => t.BusinessType, txtBusinessType, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FS_BusinessRelation>(entity, t => t.BusinessNo, txtBusinessNo, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CheckBox<tb_FS_BusinessRelation>(entity, t => t.IsMainFile, chkIsMainFile, false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



