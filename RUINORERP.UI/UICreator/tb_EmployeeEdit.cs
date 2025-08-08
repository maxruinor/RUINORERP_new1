
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:22
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
    /// 员工表数据编辑
    /// </summary>
     [MenuAttrAssemblyInfo( "库位编辑", true, UIType.单表数据)]
    public partial class tb_EmployeeEdit:UserControl
    {
     public tb_EmployeeEdit() {
     
                         InitializeComponent();
      
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        

         }
/*
        
        tb_Employee UIToEntity()
        {
        tb_Employee entity = new tb_Employee();
                     entity.DepartmentID = Int64.Parse(txtDepartmentID.Text);
                        entity.Employee_NO = txtEmployee_NO.Text ;
                       entity.Employee_Name = txtEmployee_Name.Text ;
                       entity.Gender = Boolean.Parse(txtGender.Text);
                        entity.Position = txtPosition.Text ;
                       entity.Marriage = SByte.Parse(txtMarriage.Text);
                        entity.Birthday = DateTime.Parse(txtBirthday.Text);
                        entity.StartDate = DateTime.Parse(txtStartDate.Text);
                        entity.JobTitle = txtJobTitle.Text ;
                       entity.Address = txtAddress.Text ;
                       entity.Email = txtEmail.Text ;
                       entity.Education = txtEducation.Text ;
                       entity.LanguageSkills = txtLanguageSkills.Text ;
                       entity.University = txtUniversity.Text ;
                       entity.IDNumber = txtIDNumber.Text ;
                       entity.EndDate = DateTime.Parse(txtEndDate.Text);
                        entity.salary = Decimal.Parse(txtsalary.Text);
                        entity.Notes = txtNotes.Text ;
                       entity.Is_enabled = Boolean.Parse(txtIs_enabled.Text);
                        entity.Is_available = Boolean.Parse(txtIs_available.Text);
                        entity.Created_at = DateTime.Parse(txtCreated_at.Text);
                        entity.Created_by = Int64.Parse(txtCreated_by.Text);
                        entity.Modified_at = DateTime.Parse(txtModified_at.Text);
                        entity.Modified_by = Int64.Parse(txtModified_by.Text);
                        entity.PhoneNumber = txtPhoneNumber.Text ;
                               return entity;
}
        */

        
        private tb_Employee _EditEntity;
        public tb_Employee EditEntity { get => _EditEntity; set => _EditEntity = value; }
        public void BindData(tb_Employee entity)
        {
        _EditEntity = entity;
                       // DataBindingHelper.BindData4Cmb<tb_Department>(entity, k => k.DepartmentID, v=>v.XXNAME, cmbDepartmentID);
           DataBindingHelper.BindData4TextBox<tb_Employee>(entity, t => t.Employee_NO, txtEmployee_NO, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Employee>(entity, t => t.Employee_Name, txtEmployee_Name, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CheckBox<tb_Employee>(entity, t => t.Gender, chkGender, false);
           DataBindingHelper.BindData4TextBox<tb_Employee>(entity, t => t.Position, txtPosition, BindDataType4TextBox.Text,false);
           //default  DataBindingHelper.BindData4TextBox<tb_Employee>(entity, t => t.Marriage.ToString(), txtMarriage, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4DataTime<tb_Employee>(entity, t => t.Birthday, dtpBirthday,false);
           DataBindingHelper.BindData4DataTime<tb_Employee>(entity, t => t.StartDate, dtpStartDate,false);
           DataBindingHelper.BindData4TextBox<tb_Employee>(entity, t => t.JobTitle, txtJobTitle, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Employee>(entity, t => t.Address, txtAddress, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Employee>(entity, t => t.Email, txtEmail, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Employee>(entity, t => t.Education, txtEducation, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Employee>(entity, t => t.LanguageSkills, txtLanguageSkills, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Employee>(entity, t => t.University, txtUniversity, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4TextBox<tb_Employee>(entity, t => t.IDNumber, txtIDNumber, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4DataTime<tb_Employee>(entity, t => t.EndDate, dtpEndDate,false);
           DataBindingHelper.BindData4TextBox<tb_Employee>(entity, t => t.salary.ToString(), txtsalary, BindDataType4TextBox.Money,false);
           DataBindingHelper.BindData4TextBox<tb_Employee>(entity, t => t.Notes, txtNotes, BindDataType4TextBox.Text,false);
           DataBindingHelper.BindData4CheckBox<tb_Employee>(entity, t => t.Is_enabled, chkIs_enabled, false);
//有默认值
           DataBindingHelper.BindData4CheckBox<tb_Employee>(entity, t => t.Is_available, chkIs_available, false);
//有默认值
           DataBindingHelper.BindData4DataTime<tb_Employee>(entity, t => t.Created_at, dtpCreated_at,false);
           DataBindingHelper.BindData4TextBox<tb_Employee>(entity, t => t.Created_by, txtCreated_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4DataTime<tb_Employee>(entity, t => t.Modified_at, dtpModified_at,false);
           DataBindingHelper.BindData4TextBox<tb_Employee>(entity, t => t.Modified_by, txtModified_by, BindDataType4TextBox.Qty,false);
           DataBindingHelper.BindData4TextBox<tb_Employee>(entity, t => t.PhoneNumber, txtPhoneNumber, BindDataType4TextBox.Text,false);
}




        private void btnOk_Click(object sender, EventArgs e)
        {
        
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
        
        }




    }
}



