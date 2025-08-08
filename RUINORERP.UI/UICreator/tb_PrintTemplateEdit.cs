
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
    /// 打印模板数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_PrintTemplateEdit:UserControl
    {
     public tb_PrintTemplateEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_PrintTemplate UIToEntity()
        {
        tb_PrintTemplate entity = new tb_PrintTemplate();
                     entity.PrintConfigID = Int64.Parse(txtPrintConfigID.Text);
                        entity.Template_Name = txtTemplate_Name.Text ;
                       entity.BizType = Int32.Parse(txtBizType.Text);
                        entity.BizName = txtBizName.Text ;
                       entity.TemplateFileData = txtTemplateFileData.Text ;
                       entity.TemplateFileStream = Binary.Parse(txtTemplateFileStream.Text);
                        entity.IsDefaultTemplate = Boolean.Parse(txtIsDefaultTemplate.Text);
                        entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                        entity.isdeleted = Boolean.Parse(txtisdeleted.Text);
                                return entity;
}
        */

        
        private tb_PrintTemplate _EditEntity;
        public tb_PrintTemplate EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_PrintTemplate entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_PrintConfig>(entity, k => k.PrintConfigID, v=>v.XXNAME, cmbPrintConfigID);
           DataBindingHelper.BindData4TextBox<tb_PrintTemplate>(entity, t => t.Template_Name, txtTemplate_Name, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_PrintTemplate>(entity, t => t.BizType, txtBizType, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_PrintTemplate>(entity, t => t.BizName, txtBizName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_PrintTemplate>(entity, t => t.TemplateFileData, txtTemplateFileData, BindDataType4TextBox.Text,false);
           //default  DataBindingHelper.BindData4TextBox<tb_PrintTemplate>(entity, t => t.TemplateFileStream.ToString(), txtTemplateFileStream, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4CheckBox<tb_PrintTemplate>(entity, t => t.IsDefaultTemplate, chkIsDefaultTemplate, false);
           DataBindingHelper.BindData4DataTime<tb_PrintTemplate>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_PrintTemplate>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_PrintTemplate>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_PrintTemplate>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CheckBox<tb_PrintTemplate>(entity, t => t.isdeleted, chkisdeleted, false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



