
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/19/2024 00:39:08
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
                     entity.parent_subject_id = Int64.Parse(txtparent_subject_id.Text);
                        entity.subject_code = txtsubject_code.Text ;
                       entity.subject_name = txtsubject_name.Text ;
                       entity.subject_en_name = txtsubject_en_name.Text ;
                       entity.Subject_Type = Int32.Parse(txtSubject_Type.Text);
                        entity.Balance_direction = Boolean.Parse(txtBalance_direction.Text);
                        entity.Is_enabled = Boolean.Parse(txtIs_enabled.Text);
                        entity.EndDate = DateTime.Parse(txtEndDate.Text);
                        entity.Sort = Int32.Parse(txtSort.Text);
                        entity.Images = Binary.Parse(txtImages.Text);
                        entity.Notes = txtNotes.Text ;
                               return entity;
}
        */

        
        private tb_FM_Subject _EditEntity;
        public tb_FM_Subject EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_FM_Subject entity)
        {
        _EditEntity = entity;
                        DataBindingHelper.BindData4TextBox<tb_FM_Subject>(entity, t => t.parent_subject_id, txtparent_subject_id, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_FM_Subject>(entity, t => t.subject_code, txtsubject_code, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_Subject>(entity, t => t.subject_name, txtsubject_name, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_Subject>(entity, t => t.subject_en_name, txtsubject_en_name, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_FM_Subject>(entity, t => t.Subject_Type, txtSubject_Type, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4CehckBox<tb_FM_Subject>(entity, t => t.Balance_direction, chkBalance_direction, false);
           DataBindingHelper.BindData4CehckBox<tb_FM_Subject>(entity, t => t.Is_enabled, chkIs_enabled, false);
//有默认值
           DataBindingHelper.BindData4DataTime<tb_FM_Subject>(entity, t => t.EndDate, dtpEndDate,false);
           DataBindingHelper.BindData4TextBox<tb_FM_Subject>(entity, t => t.Sort, txtSort, BindDataType4TextBox.Qty,false);
           //default  DataBindingHelper.BindData4TextBox<tb_FM_Subject>(entity, t => t.Images.ToString(), txtImages, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_FM_Subject>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



