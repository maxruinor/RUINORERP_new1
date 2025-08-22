
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/20/2025 16:08:02
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 员工表
    /// </summary>
    partial class tb_EmployeeQuery
    {
    
    
            /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        
                /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
        
     //for start
     
     this.lblDepartmentID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbDepartmentID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblEmployee_NO = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtEmployee_NO = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblEmployee_Name = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtEmployee_Name = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblGender = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkGender = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkGender.Values.Text ="";

this.lblPosition = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPosition = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblBirthday = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpBirthday = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblStartDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpStartDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblJobTitle = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtJobTitle = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblAddress = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtAddress = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtAddress.Multiline = true;

this.lblEmail = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtEmail = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblEducation = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtEducation = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblLanguageSkills = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtLanguageSkills = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblUniversity = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtUniversity = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblIDNumber = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtIDNumber = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblEndDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpEndDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblsalary = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtsalary = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblNotes = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotes = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblIs_enabled = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIs_enabled = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIs_enabled.Values.Text ="";
this.chkIs_enabled.Checked = true;
this.chkIs_enabled.CheckState = System.Windows.Forms.CheckState.Checked;

this.lblIs_available = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIs_available = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIs_available.Values.Text ="";
this.chkIs_available.Checked = true;
this.chkIs_available.CheckState = System.Windows.Forms.CheckState.Checked;

this.lblCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblModified_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblPhoneNumber = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPhoneNumber = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####DepartmentID###Int64
//属性测试25DepartmentID
this.lblDepartmentID.AutoSize = true;
this.lblDepartmentID.Location = new System.Drawing.Point(100,25);
this.lblDepartmentID.Name = "lblDepartmentID";
this.lblDepartmentID.Size = new System.Drawing.Size(41, 12);
this.lblDepartmentID.TabIndex = 1;
this.lblDepartmentID.Text = "部门";
//111======25
this.cmbDepartmentID.Location = new System.Drawing.Point(173,21);
this.cmbDepartmentID.Name ="cmbDepartmentID";
this.cmbDepartmentID.Size = new System.Drawing.Size(100, 21);
this.cmbDepartmentID.TabIndex = 1;
this.Controls.Add(this.lblDepartmentID);
this.Controls.Add(this.cmbDepartmentID);

           //#####20Employee_NO###String
this.lblEmployee_NO.AutoSize = true;
this.lblEmployee_NO.Location = new System.Drawing.Point(100,50);
this.lblEmployee_NO.Name = "lblEmployee_NO";
this.lblEmployee_NO.Size = new System.Drawing.Size(41, 12);
this.lblEmployee_NO.TabIndex = 2;
this.lblEmployee_NO.Text = "员工编号";
this.txtEmployee_NO.Location = new System.Drawing.Point(173,46);
this.txtEmployee_NO.Name = "txtEmployee_NO";
this.txtEmployee_NO.Size = new System.Drawing.Size(100, 21);
this.txtEmployee_NO.TabIndex = 2;
this.Controls.Add(this.lblEmployee_NO);
this.Controls.Add(this.txtEmployee_NO);

           //#####100Employee_Name###String
this.lblEmployee_Name.AutoSize = true;
this.lblEmployee_Name.Location = new System.Drawing.Point(100,75);
this.lblEmployee_Name.Name = "lblEmployee_Name";
this.lblEmployee_Name.Size = new System.Drawing.Size(41, 12);
this.lblEmployee_Name.TabIndex = 3;
this.lblEmployee_Name.Text = "姓名";
this.txtEmployee_Name.Location = new System.Drawing.Point(173,71);
this.txtEmployee_Name.Name = "txtEmployee_Name";
this.txtEmployee_Name.Size = new System.Drawing.Size(100, 21);
this.txtEmployee_Name.TabIndex = 3;
this.Controls.Add(this.lblEmployee_Name);
this.Controls.Add(this.txtEmployee_Name);

           //#####Gender###Boolean
this.lblGender.AutoSize = true;
this.lblGender.Location = new System.Drawing.Point(100,100);
this.lblGender.Name = "lblGender";
this.lblGender.Size = new System.Drawing.Size(41, 12);
this.lblGender.TabIndex = 4;
this.lblGender.Text = "性别";
this.chkGender.Location = new System.Drawing.Point(173,96);
this.chkGender.Name = "chkGender";
this.chkGender.Size = new System.Drawing.Size(100, 21);
this.chkGender.TabIndex = 4;
this.Controls.Add(this.lblGender);
this.Controls.Add(this.chkGender);

           //#####20Position###String
this.lblPosition.AutoSize = true;
this.lblPosition.Location = new System.Drawing.Point(100,125);
this.lblPosition.Name = "lblPosition";
this.lblPosition.Size = new System.Drawing.Size(41, 12);
this.lblPosition.TabIndex = 5;
this.lblPosition.Text = "职位";
this.txtPosition.Location = new System.Drawing.Point(173,121);
this.txtPosition.Name = "txtPosition";
this.txtPosition.Size = new System.Drawing.Size(100, 21);
this.txtPosition.TabIndex = 5;
this.Controls.Add(this.lblPosition);
this.Controls.Add(this.txtPosition);

           //#####Marriage###SByte

           //#####Birthday###DateTime
this.lblBirthday.AutoSize = true;
this.lblBirthday.Location = new System.Drawing.Point(100,175);
this.lblBirthday.Name = "lblBirthday";
this.lblBirthday.Size = new System.Drawing.Size(41, 12);
this.lblBirthday.TabIndex = 7;
this.lblBirthday.Text = "生日";
//111======175
this.dtpBirthday.Location = new System.Drawing.Point(173,171);
this.dtpBirthday.Name ="dtpBirthday";
this.dtpBirthday.ShowCheckBox =true;
this.dtpBirthday.Size = new System.Drawing.Size(100, 21);
this.dtpBirthday.TabIndex = 7;
this.Controls.Add(this.lblBirthday);
this.Controls.Add(this.dtpBirthday);

           //#####StartDate###DateTime
this.lblStartDate.AutoSize = true;
this.lblStartDate.Location = new System.Drawing.Point(100,200);
this.lblStartDate.Name = "lblStartDate";
this.lblStartDate.Size = new System.Drawing.Size(41, 12);
this.lblStartDate.TabIndex = 8;
this.lblStartDate.Text = "入职时间";
//111======200
this.dtpStartDate.Location = new System.Drawing.Point(173,196);
this.dtpStartDate.Name ="dtpStartDate";
this.dtpStartDate.ShowCheckBox =true;
this.dtpStartDate.Size = new System.Drawing.Size(100, 21);
this.dtpStartDate.TabIndex = 8;
this.Controls.Add(this.lblStartDate);
this.Controls.Add(this.dtpStartDate);

           //#####50JobTitle###String
this.lblJobTitle.AutoSize = true;
this.lblJobTitle.Location = new System.Drawing.Point(100,225);
this.lblJobTitle.Name = "lblJobTitle";
this.lblJobTitle.Size = new System.Drawing.Size(41, 12);
this.lblJobTitle.TabIndex = 9;
this.lblJobTitle.Text = "职称";
this.txtJobTitle.Location = new System.Drawing.Point(173,221);
this.txtJobTitle.Name = "txtJobTitle";
this.txtJobTitle.Size = new System.Drawing.Size(100, 21);
this.txtJobTitle.TabIndex = 9;
this.Controls.Add(this.lblJobTitle);
this.Controls.Add(this.txtJobTitle);

           //#####255Address###String
this.lblAddress.AutoSize = true;
this.lblAddress.Location = new System.Drawing.Point(100,250);
this.lblAddress.Name = "lblAddress";
this.lblAddress.Size = new System.Drawing.Size(41, 12);
this.lblAddress.TabIndex = 10;
this.lblAddress.Text = "联络地址";
this.txtAddress.Location = new System.Drawing.Point(173,246);
this.txtAddress.Name = "txtAddress";
this.txtAddress.Size = new System.Drawing.Size(100, 21);
this.txtAddress.TabIndex = 10;
this.Controls.Add(this.lblAddress);
this.Controls.Add(this.txtAddress);

           //#####100Email###String
this.lblEmail.AutoSize = true;
this.lblEmail.Location = new System.Drawing.Point(100,275);
this.lblEmail.Name = "lblEmail";
this.lblEmail.Size = new System.Drawing.Size(41, 12);
this.lblEmail.TabIndex = 11;
this.lblEmail.Text = "邮件";
this.txtEmail.Location = new System.Drawing.Point(173,271);
this.txtEmail.Name = "txtEmail";
this.txtEmail.Size = new System.Drawing.Size(100, 21);
this.txtEmail.TabIndex = 11;
this.Controls.Add(this.lblEmail);
this.Controls.Add(this.txtEmail);

           //#####100Education###String
this.lblEducation.AutoSize = true;
this.lblEducation.Location = new System.Drawing.Point(100,300);
this.lblEducation.Name = "lblEducation";
this.lblEducation.Size = new System.Drawing.Size(41, 12);
this.lblEducation.TabIndex = 12;
this.lblEducation.Text = "教育程度";
this.txtEducation.Location = new System.Drawing.Point(173,296);
this.txtEducation.Name = "txtEducation";
this.txtEducation.Size = new System.Drawing.Size(100, 21);
this.txtEducation.TabIndex = 12;
this.Controls.Add(this.lblEducation);
this.Controls.Add(this.txtEducation);

           //#####50LanguageSkills###String
this.lblLanguageSkills.AutoSize = true;
this.lblLanguageSkills.Location = new System.Drawing.Point(100,325);
this.lblLanguageSkills.Name = "lblLanguageSkills";
this.lblLanguageSkills.Size = new System.Drawing.Size(41, 12);
this.lblLanguageSkills.TabIndex = 13;
this.lblLanguageSkills.Text = "外语能力";
this.txtLanguageSkills.Location = new System.Drawing.Point(173,321);
this.txtLanguageSkills.Name = "txtLanguageSkills";
this.txtLanguageSkills.Size = new System.Drawing.Size(100, 21);
this.txtLanguageSkills.TabIndex = 13;
this.Controls.Add(this.lblLanguageSkills);
this.Controls.Add(this.txtLanguageSkills);

           //#####100University###String
this.lblUniversity.AutoSize = true;
this.lblUniversity.Location = new System.Drawing.Point(100,350);
this.lblUniversity.Name = "lblUniversity";
this.lblUniversity.Size = new System.Drawing.Size(41, 12);
this.lblUniversity.TabIndex = 14;
this.lblUniversity.Text = "毕业院校";
this.txtUniversity.Location = new System.Drawing.Point(173,346);
this.txtUniversity.Name = "txtUniversity";
this.txtUniversity.Size = new System.Drawing.Size(100, 21);
this.txtUniversity.TabIndex = 14;
this.Controls.Add(this.lblUniversity);
this.Controls.Add(this.txtUniversity);

           //#####30IDNumber###String
this.lblIDNumber.AutoSize = true;
this.lblIDNumber.Location = new System.Drawing.Point(100,375);
this.lblIDNumber.Name = "lblIDNumber";
this.lblIDNumber.Size = new System.Drawing.Size(41, 12);
this.lblIDNumber.TabIndex = 15;
this.lblIDNumber.Text = "身份证号";
this.txtIDNumber.Location = new System.Drawing.Point(173,371);
this.txtIDNumber.Name = "txtIDNumber";
this.txtIDNumber.Size = new System.Drawing.Size(100, 21);
this.txtIDNumber.TabIndex = 15;
this.Controls.Add(this.lblIDNumber);
this.Controls.Add(this.txtIDNumber);

           //#####EndDate###DateTime
this.lblEndDate.AutoSize = true;
this.lblEndDate.Location = new System.Drawing.Point(100,400);
this.lblEndDate.Name = "lblEndDate";
this.lblEndDate.Size = new System.Drawing.Size(41, 12);
this.lblEndDate.TabIndex = 16;
this.lblEndDate.Text = "离职日期";
//111======400
this.dtpEndDate.Location = new System.Drawing.Point(173,396);
this.dtpEndDate.Name ="dtpEndDate";
this.dtpEndDate.ShowCheckBox =true;
this.dtpEndDate.Size = new System.Drawing.Size(100, 21);
this.dtpEndDate.TabIndex = 16;
this.Controls.Add(this.lblEndDate);
this.Controls.Add(this.dtpEndDate);

           //#####salary###Decimal
this.lblsalary.AutoSize = true;
this.lblsalary.Location = new System.Drawing.Point(100,425);
this.lblsalary.Name = "lblsalary";
this.lblsalary.Size = new System.Drawing.Size(41, 12);
this.lblsalary.TabIndex = 17;
this.lblsalary.Text = "工资";
//111======425
this.txtsalary.Location = new System.Drawing.Point(173,421);
this.txtsalary.Name ="txtsalary";
this.txtsalary.Size = new System.Drawing.Size(100, 21);
this.txtsalary.TabIndex = 17;
this.Controls.Add(this.lblsalary);
this.Controls.Add(this.txtsalary);

           //#####200Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,450);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 18;
this.lblNotes.Text = "备注说明";
this.txtNotes.Location = new System.Drawing.Point(173,446);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 18;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####Is_enabled###Boolean
this.lblIs_enabled.AutoSize = true;
this.lblIs_enabled.Location = new System.Drawing.Point(100,475);
this.lblIs_enabled.Name = "lblIs_enabled";
this.lblIs_enabled.Size = new System.Drawing.Size(41, 12);
this.lblIs_enabled.TabIndex = 19;
this.lblIs_enabled.Text = "是否启用";
this.chkIs_enabled.Location = new System.Drawing.Point(173,471);
this.chkIs_enabled.Name = "chkIs_enabled";
this.chkIs_enabled.Size = new System.Drawing.Size(100, 21);
this.chkIs_enabled.TabIndex = 19;
this.Controls.Add(this.lblIs_enabled);
this.Controls.Add(this.chkIs_enabled);

           //#####Is_available###Boolean
this.lblIs_available.AutoSize = true;
this.lblIs_available.Location = new System.Drawing.Point(100,500);
this.lblIs_available.Name = "lblIs_available";
this.lblIs_available.Size = new System.Drawing.Size(41, 12);
this.lblIs_available.TabIndex = 20;
this.lblIs_available.Text = "是否可用";
this.chkIs_available.Location = new System.Drawing.Point(173,496);
this.chkIs_available.Name = "chkIs_available";
this.chkIs_available.Size = new System.Drawing.Size(100, 21);
this.chkIs_available.TabIndex = 20;
this.Controls.Add(this.lblIs_available);
this.Controls.Add(this.chkIs_available);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,525);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 21;
this.lblCreated_at.Text = "创建时间";
//111======525
this.dtpCreated_at.Location = new System.Drawing.Point(173,521);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 21;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
//属性测试550Created_by

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,575);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 23;
this.lblModified_at.Text = "修改时间";
//111======575
this.dtpModified_at.Location = new System.Drawing.Point(173,571);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 23;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
//属性测试600Modified_by

           //#####50PhoneNumber###String
this.lblPhoneNumber.AutoSize = true;
this.lblPhoneNumber.Location = new System.Drawing.Point(100,625);
this.lblPhoneNumber.Name = "lblPhoneNumber";
this.lblPhoneNumber.Size = new System.Drawing.Size(41, 12);
this.lblPhoneNumber.TabIndex = 25;
this.lblPhoneNumber.Text = "手机号";
this.txtPhoneNumber.Location = new System.Drawing.Point(173,621);
this.txtPhoneNumber.Name = "txtPhoneNumber";
this.txtPhoneNumber.Size = new System.Drawing.Size(100, 21);
this.txtPhoneNumber.TabIndex = 25;
this.Controls.Add(this.lblPhoneNumber);
this.Controls.Add(this.txtPhoneNumber);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblDepartmentID );
this.Controls.Add(this.cmbDepartmentID );

                this.Controls.Add(this.lblEmployee_NO );
this.Controls.Add(this.txtEmployee_NO );

                this.Controls.Add(this.lblEmployee_Name );
this.Controls.Add(this.txtEmployee_Name );

                this.Controls.Add(this.lblGender );
this.Controls.Add(this.chkGender );

                this.Controls.Add(this.lblPosition );
this.Controls.Add(this.txtPosition );

                
                this.Controls.Add(this.lblBirthday );
this.Controls.Add(this.dtpBirthday );

                this.Controls.Add(this.lblStartDate );
this.Controls.Add(this.dtpStartDate );

                this.Controls.Add(this.lblJobTitle );
this.Controls.Add(this.txtJobTitle );

                this.Controls.Add(this.lblAddress );
this.Controls.Add(this.txtAddress );

                this.Controls.Add(this.lblEmail );
this.Controls.Add(this.txtEmail );

                this.Controls.Add(this.lblEducation );
this.Controls.Add(this.txtEducation );

                this.Controls.Add(this.lblLanguageSkills );
this.Controls.Add(this.txtLanguageSkills );

                this.Controls.Add(this.lblUniversity );
this.Controls.Add(this.txtUniversity );

                this.Controls.Add(this.lblIDNumber );
this.Controls.Add(this.txtIDNumber );

                this.Controls.Add(this.lblEndDate );
this.Controls.Add(this.dtpEndDate );

                this.Controls.Add(this.lblsalary );
this.Controls.Add(this.txtsalary );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblIs_enabled );
this.Controls.Add(this.chkIs_enabled );

                this.Controls.Add(this.lblIs_available );
this.Controls.Add(this.chkIs_available );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                
                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                
                this.Controls.Add(this.lblPhoneNumber );
this.Controls.Add(this.txtPhoneNumber );

                    
            this.Name = "tb_EmployeeQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDepartmentID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbDepartmentID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblEmployee_NO;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtEmployee_NO;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblEmployee_Name;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtEmployee_Name;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblGender;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkGender;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPosition;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPosition;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblBirthday;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpBirthday;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblStartDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpStartDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblJobTitle;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtJobTitle;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblAddress;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtAddress;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblEmail;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtEmail;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblEducation;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtEducation;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblLanguageSkills;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtLanguageSkills;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblUniversity;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtUniversity;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIDNumber;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtIDNumber;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblEndDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpEndDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblsalary;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtsalary;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIs_enabled;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIs_enabled;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIs_available;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIs_available;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModified_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPhoneNumber;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPhoneNumber;

    
    
   
 





    }
}


