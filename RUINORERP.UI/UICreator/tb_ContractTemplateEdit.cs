
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:16
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
    /// 合同模板表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_ContractTemplateEdit:UserControl
    {
     public tb_ContractTemplateEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_ContractTemplate UIToEntity()
        {
        tb_ContractTemplate entity = new tb_ContractTemplate();
                     entity.TemplateName = Int64.Parse(txtTemplateName.Text);
                        entity.TemplateFile = Binary.Parse(txtTemplateFile.Text);
                        entity.FieldsConfig = txtFieldsConfig.Text ;
                       entity.Remarks = txtRemarks.Text ;
                       entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                                return entity;
}
        */

        
        private tb_ContractTemplate _EditEntity;
        public tb_ContractTemplate EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_ContractTemplate entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_ContractTemplate>(entity, t => t.TemplateName, txtTemplateName, BindDataType4TextBox.Qty,false);
           //default  DataBindingHelper.BindData4TextBox<tb_ContractTemplate>(entity, t => t.TemplateFile.ToString(), txtTemplateFile, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_ContractTemplate>(entity, t => t.FieldsConfig, txtFieldsConfig, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_ContractTemplate>(entity, t => t.Remarks, txtRemarks, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<tb_ContractTemplate>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_ContractTemplate>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_ContractTemplate>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_ContractTemplate>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



