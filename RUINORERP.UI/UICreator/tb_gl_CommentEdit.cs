
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:36
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
    /// 全局级批注表-对于重点关注的业务帮助记录和跟踪相关的额外信息，提高沟通效率和透明度数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_gl_CommentEdit:UserControl
    {
     public tb_gl_CommentEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_gl_Comment UIToEntity()
        {
        tb_gl_Comment entity = new tb_gl_Comment();
                     entity.Employee_ID = Int64.Parse(txtEmployee_ID.Text);
                        entity.BizTypeID = Int32.Parse(txtBizTypeID.Text);
                        entity.BusinessID = Int64.Parse(txtBusinessID.Text);
                        entity.DbTableName = txtDbTableName.Text ;
                       entity.CommentContent = txtCommentContent.Text ;
                       entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                                return entity;
}
        */

        
        private tb_gl_Comment _EditEntity;
        public tb_gl_Comment EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_gl_Comment entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_Employee>(entity, k => k.Employee_ID, v=>v.XXNAME, cmbEmployee_ID);
           DataBindingHelper.BindData4TextBox<tb_gl_Comment>(entity, t => t.BizTypeID, txtBizTypeID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_gl_Comment>(entity, t => t.BusinessID, txtBusinessID, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_gl_Comment>(entity, t => t.DbTableName, txtDbTableName, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_gl_Comment>(entity, t => t.CommentContent, txtCommentContent, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<tb_gl_Comment>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_gl_Comment>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_gl_Comment>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_gl_Comment>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



