
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
    /// 会计科目表，财务系统中使用数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_FM_SubjectEdit:UserControl
    {
     public tb_FM_SubjectEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_FM_Subject UIToEntity()
        {
        tb_FM_Subject entity = new tb_FM_Subject();
                     entity.Parent_subject_id = Int64.Parse(txtParent_subject_id.Text);
                        entity.subject_code = txtsubject_code.Text ;
                       entity.subject_name = txtsubject_name.Text ;
                       entity.subject_en_name = txtsubject_en_name.Text ;
                       entity.Subject_Type = Int32.Parse(txtSubject_Type.Text);
                        entity.Balance_direction = Boolean.Parse(txtBalance_direction.Text);
                        entity.Is_enabled = Boolean.Parse(txtIs_enabled.Text);
                        entity.Sort = Int32.Parse(txtSort.Text);
                        entity.Images = Binary.Parse(txtImages.Text);
                        entity.Notes = txtNotes.Text ;
                       entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                        entity.isdeleted = Boolean.Parse(txtisdeleted.Text);
                                return entity;
}
        */

        
        private tb_FM_Subject _EditEntity;
        public tb_FM_Subject EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_FM_Subject entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_FM_Subject>(entity, t => t.Parent_subject_id, txtParent_subject_id, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FM_Subject>(entity, t => t.subject_code, txtsubject_code, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_Subject>(entity, t => t.subject_name, txtsubject_name, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_Subject>(entity, t => t.subject_en_name, txtsubject_en_name, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_Subject>(entity, t => t.Subject_Type, txtSubject_Type, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CheckBox<tb_FM_Subject>(entity, t => t.Balance_direction, chkBalance_direction, false);
           DataBindingHelper.BindData4CheckBox<tb_FM_Subject>(entity, t => t.Is_enabled, chkIs_enabled, false);
//有默认值
           DataBindingHelper.BindData4TextBox<tb_FM_Subject>(entity, t => t.Sort, txtSort, BindDataType4TextBox.Qty,false);
           //default  DataBindingHelper.BindData4TextBox<tb_FM_Subject>(entity, t => t.Images.ToString(), txtImages, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_Subject>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<tb_FM_Subject>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_FM_Subject>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_FM_Subject>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_FM_Subject>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CheckBox<tb_FM_Subject>(entity, t => t.isdeleted, chkisdeleted, false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



