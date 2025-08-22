
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/20/2025 16:08:01
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 部门表是否分层
    /// </summary>
    partial class tb_DepartmentQuery
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
     
     this.lblID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblDepartmentCode = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtDepartmentCode = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblDepartmentName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtDepartmentName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtDepartmentName.Multiline = true;

this.lblTEL = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTEL = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblNotes = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotes = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblDirector = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtDirector = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblModified_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblisdeleted = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkisdeleted = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkisdeleted.Values.Text ="";

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####ID###Int64
//属性测试25ID
this.lblID.AutoSize = true;
this.lblID.Location = new System.Drawing.Point(100,25);
this.lblID.Name = "lblID";
this.lblID.Size = new System.Drawing.Size(41, 12);
this.lblID.TabIndex = 1;
this.lblID.Text = "所属公司";
//111======25
this.cmbID.Location = new System.Drawing.Point(173,21);
this.cmbID.Name ="cmbID";
this.cmbID.Size = new System.Drawing.Size(100, 21);
this.cmbID.TabIndex = 1;
this.Controls.Add(this.lblID);
this.Controls.Add(this.cmbID);

           //#####50DepartmentCode###String
this.lblDepartmentCode.AutoSize = true;
this.lblDepartmentCode.Location = new System.Drawing.Point(100,50);
this.lblDepartmentCode.Name = "lblDepartmentCode";
this.lblDepartmentCode.Size = new System.Drawing.Size(41, 12);
this.lblDepartmentCode.TabIndex = 2;
this.lblDepartmentCode.Text = "部门代号";
this.txtDepartmentCode.Location = new System.Drawing.Point(173,46);
this.txtDepartmentCode.Name = "txtDepartmentCode";
this.txtDepartmentCode.Size = new System.Drawing.Size(100, 21);
this.txtDepartmentCode.TabIndex = 2;
this.Controls.Add(this.lblDepartmentCode);
this.Controls.Add(this.txtDepartmentCode);

           //#####255DepartmentName###String
this.lblDepartmentName.AutoSize = true;
this.lblDepartmentName.Location = new System.Drawing.Point(100,75);
this.lblDepartmentName.Name = "lblDepartmentName";
this.lblDepartmentName.Size = new System.Drawing.Size(41, 12);
this.lblDepartmentName.TabIndex = 3;
this.lblDepartmentName.Text = "部门名称";
this.txtDepartmentName.Location = new System.Drawing.Point(173,71);
this.txtDepartmentName.Name = "txtDepartmentName";
this.txtDepartmentName.Size = new System.Drawing.Size(100, 21);
this.txtDepartmentName.TabIndex = 3;
this.Controls.Add(this.lblDepartmentName);
this.Controls.Add(this.txtDepartmentName);

           //#####20TEL###String
this.lblTEL.AutoSize = true;
this.lblTEL.Location = new System.Drawing.Point(100,100);
this.lblTEL.Name = "lblTEL";
this.lblTEL.Size = new System.Drawing.Size(41, 12);
this.lblTEL.TabIndex = 4;
this.lblTEL.Text = "电话";
this.txtTEL.Location = new System.Drawing.Point(173,96);
this.txtTEL.Name = "txtTEL";
this.txtTEL.Size = new System.Drawing.Size(100, 21);
this.txtTEL.TabIndex = 4;
this.Controls.Add(this.lblTEL);
this.Controls.Add(this.txtTEL);

           //#####200Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,125);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 5;
this.lblNotes.Text = "备注";
this.txtNotes.Location = new System.Drawing.Point(173,121);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 5;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####20Director###String
this.lblDirector.AutoSize = true;
this.lblDirector.Location = new System.Drawing.Point(100,150);
this.lblDirector.Name = "lblDirector";
this.lblDirector.Size = new System.Drawing.Size(41, 12);
this.lblDirector.TabIndex = 6;
this.lblDirector.Text = "责任人";
this.txtDirector.Location = new System.Drawing.Point(173,146);
this.txtDirector.Name = "txtDirector";
this.txtDirector.Size = new System.Drawing.Size(100, 21);
this.txtDirector.TabIndex = 6;
this.Controls.Add(this.lblDirector);
this.Controls.Add(this.txtDirector);

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

           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,275);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 11;
this.lblisdeleted.Text = "逻辑删除";
this.chkisdeleted.Location = new System.Drawing.Point(173,271);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 11;
this.Controls.Add(this.lblisdeleted);
this.Controls.Add(this.chkisdeleted);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblID );
this.Controls.Add(this.cmbID );

                this.Controls.Add(this.lblDepartmentCode );
this.Controls.Add(this.txtDepartmentCode );

                this.Controls.Add(this.lblDepartmentName );
this.Controls.Add(this.txtDepartmentName );

                this.Controls.Add(this.lblTEL );
this.Controls.Add(this.txtTEL );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblDirector );
this.Controls.Add(this.txtDirector );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                
                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                
                this.Controls.Add(this.lblisdeleted );
this.Controls.Add(this.chkisdeleted );

                    
            this.Name = "tb_DepartmentQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDepartmentCode;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtDepartmentCode;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDepartmentName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtDepartmentName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTEL;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTEL;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDirector;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtDirector;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModified_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblisdeleted;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkisdeleted;

    
    
   
 





    }
}


