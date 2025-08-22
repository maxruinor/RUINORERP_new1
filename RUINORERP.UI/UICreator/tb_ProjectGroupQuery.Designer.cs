
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/20/2025 16:08:16
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 项目组信息 用于业务分组小团队
    /// </summary>
    partial class tb_ProjectGroupQuery
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

this.lblProjectGroupCode = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtProjectGroupCode = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblProjectGroupName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtProjectGroupName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblResponsiblePerson = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtResponsiblePerson = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblPhone = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPhone = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtPhone.Multiline = true;

this.lblNotes = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotes = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

this.lblCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblModified_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblStartDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpStartDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblIs_enabled = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIs_enabled = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIs_enabled.Values.Text ="";
this.chkIs_enabled.Checked = true;
this.chkIs_enabled.CheckState = System.Windows.Forms.CheckState.Checked;

this.lblEndDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpEndDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

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

           //#####50ProjectGroupCode###String
this.lblProjectGroupCode.AutoSize = true;
this.lblProjectGroupCode.Location = new System.Drawing.Point(100,50);
this.lblProjectGroupCode.Name = "lblProjectGroupCode";
this.lblProjectGroupCode.Size = new System.Drawing.Size(41, 12);
this.lblProjectGroupCode.TabIndex = 2;
this.lblProjectGroupCode.Text = "项目组代号";
this.txtProjectGroupCode.Location = new System.Drawing.Point(173,46);
this.txtProjectGroupCode.Name = "txtProjectGroupCode";
this.txtProjectGroupCode.Size = new System.Drawing.Size(100, 21);
this.txtProjectGroupCode.TabIndex = 2;
this.Controls.Add(this.lblProjectGroupCode);
this.Controls.Add(this.txtProjectGroupCode);

           //#####50ProjectGroupName###String
this.lblProjectGroupName.AutoSize = true;
this.lblProjectGroupName.Location = new System.Drawing.Point(100,75);
this.lblProjectGroupName.Name = "lblProjectGroupName";
this.lblProjectGroupName.Size = new System.Drawing.Size(41, 12);
this.lblProjectGroupName.TabIndex = 3;
this.lblProjectGroupName.Text = "项目组名称";
this.txtProjectGroupName.Location = new System.Drawing.Point(173,71);
this.txtProjectGroupName.Name = "txtProjectGroupName";
this.txtProjectGroupName.Size = new System.Drawing.Size(100, 21);
this.txtProjectGroupName.TabIndex = 3;
this.Controls.Add(this.lblProjectGroupName);
this.Controls.Add(this.txtProjectGroupName);

           //#####50ResponsiblePerson###String
this.lblResponsiblePerson.AutoSize = true;
this.lblResponsiblePerson.Location = new System.Drawing.Point(100,100);
this.lblResponsiblePerson.Name = "lblResponsiblePerson";
this.lblResponsiblePerson.Size = new System.Drawing.Size(41, 12);
this.lblResponsiblePerson.TabIndex = 4;
this.lblResponsiblePerson.Text = "负责人";
this.txtResponsiblePerson.Location = new System.Drawing.Point(173,96);
this.txtResponsiblePerson.Name = "txtResponsiblePerson";
this.txtResponsiblePerson.Size = new System.Drawing.Size(100, 21);
this.txtResponsiblePerson.TabIndex = 4;
this.Controls.Add(this.lblResponsiblePerson);
this.Controls.Add(this.txtResponsiblePerson);

           //#####255Phone###String
this.lblPhone.AutoSize = true;
this.lblPhone.Location = new System.Drawing.Point(100,125);
this.lblPhone.Name = "lblPhone";
this.lblPhone.Size = new System.Drawing.Size(41, 12);
this.lblPhone.TabIndex = 5;
this.lblPhone.Text = "电话";
this.txtPhone.Location = new System.Drawing.Point(173,121);
this.txtPhone.Name = "txtPhone";
this.txtPhone.Size = new System.Drawing.Size(100, 21);
this.txtPhone.TabIndex = 5;
this.Controls.Add(this.lblPhone);
this.Controls.Add(this.txtPhone);

           //#####255Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,150);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 6;
this.lblNotes.Text = "备注";
this.txtNotes.Location = new System.Drawing.Point(173,146);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 6;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,175);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 7;
this.lblCreated_at.Text = "创建时间";
//111======175
this.dtpCreated_at.Location = new System.Drawing.Point(173,171);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 7;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
//属性测试200Created_by

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,225);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 9;
this.lblModified_at.Text = "修改时间";
//111======225
this.dtpModified_at.Location = new System.Drawing.Point(173,221);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 9;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
//属性测试250Modified_by

           //#####StartDate###DateTime
this.lblStartDate.AutoSize = true;
this.lblStartDate.Location = new System.Drawing.Point(100,275);
this.lblStartDate.Name = "lblStartDate";
this.lblStartDate.Size = new System.Drawing.Size(41, 12);
this.lblStartDate.TabIndex = 11;
this.lblStartDate.Text = "启动时间";
//111======275
this.dtpStartDate.Location = new System.Drawing.Point(173,271);
this.dtpStartDate.Name ="dtpStartDate";
this.dtpStartDate.ShowCheckBox =true;
this.dtpStartDate.Size = new System.Drawing.Size(100, 21);
this.dtpStartDate.TabIndex = 11;
this.Controls.Add(this.lblStartDate);
this.Controls.Add(this.dtpStartDate);

           //#####Is_enabled###Boolean
this.lblIs_enabled.AutoSize = true;
this.lblIs_enabled.Location = new System.Drawing.Point(100,300);
this.lblIs_enabled.Name = "lblIs_enabled";
this.lblIs_enabled.Size = new System.Drawing.Size(41, 12);
this.lblIs_enabled.TabIndex = 12;
this.lblIs_enabled.Text = "是否启用";
this.chkIs_enabled.Location = new System.Drawing.Point(173,296);
this.chkIs_enabled.Name = "chkIs_enabled";
this.chkIs_enabled.Size = new System.Drawing.Size(100, 21);
this.chkIs_enabled.TabIndex = 12;
this.Controls.Add(this.lblIs_enabled);
this.Controls.Add(this.chkIs_enabled);

           //#####EndDate###DateTime
this.lblEndDate.AutoSize = true;
this.lblEndDate.Location = new System.Drawing.Point(100,325);
this.lblEndDate.Name = "lblEndDate";
this.lblEndDate.Size = new System.Drawing.Size(41, 12);
this.lblEndDate.TabIndex = 13;
this.lblEndDate.Text = "结束时间";
//111======325
this.dtpEndDate.Location = new System.Drawing.Point(173,321);
this.dtpEndDate.Name ="dtpEndDate";
this.dtpEndDate.ShowCheckBox =true;
this.dtpEndDate.Size = new System.Drawing.Size(100, 21);
this.dtpEndDate.TabIndex = 13;
this.Controls.Add(this.lblEndDate);
this.Controls.Add(this.dtpEndDate);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblDepartmentID );
this.Controls.Add(this.cmbDepartmentID );

                this.Controls.Add(this.lblProjectGroupCode );
this.Controls.Add(this.txtProjectGroupCode );

                this.Controls.Add(this.lblProjectGroupName );
this.Controls.Add(this.txtProjectGroupName );

                this.Controls.Add(this.lblResponsiblePerson );
this.Controls.Add(this.txtResponsiblePerson );

                this.Controls.Add(this.lblPhone );
this.Controls.Add(this.txtPhone );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                
                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                
                this.Controls.Add(this.lblStartDate );
this.Controls.Add(this.dtpStartDate );

                this.Controls.Add(this.lblIs_enabled );
this.Controls.Add(this.chkIs_enabled );

                this.Controls.Add(this.lblEndDate );
this.Controls.Add(this.dtpEndDate );

                    
            this.Name = "tb_ProjectGroupQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDepartmentID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbDepartmentID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProjectGroupCode;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtProjectGroupCode;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProjectGroupName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtProjectGroupName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblResponsiblePerson;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtResponsiblePerson;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPhone;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPhone;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModified_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblStartDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpStartDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIs_enabled;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIs_enabled;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblEndDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpEndDate;

    
    
   
 





    }
}


