
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/12/2024 10:37:31
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 跟进记录表
    /// </summary>
    partial class tb_CRM_FollowUpRecordsQuery
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
     
     this.lblCustomer_id = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbCustomer_id = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblLeadID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbLeadID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblPlanID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbPlanID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblFollowUpDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpFollowUpDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblFollowUpSubject = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtFollowUpSubject = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblFollowUpContent = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtFollowUpContent = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtFollowUpContent.Multiline = true;

this.lblNotes = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotes = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

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
                 //#####Customer_id###Int64
//属性测试25Customer_id
//属性测试25Customer_id
//属性测试25Customer_id
this.lblCustomer_id.AutoSize = true;
this.lblCustomer_id.Location = new System.Drawing.Point(100,25);
this.lblCustomer_id.Name = "lblCustomer_id";
this.lblCustomer_id.Size = new System.Drawing.Size(41, 12);
this.lblCustomer_id.TabIndex = 1;
this.lblCustomer_id.Text = "目标客户";
//111======25
this.cmbCustomer_id.Location = new System.Drawing.Point(173,21);
this.cmbCustomer_id.Name ="cmbCustomer_id";
this.cmbCustomer_id.Size = new System.Drawing.Size(100, 21);
this.cmbCustomer_id.TabIndex = 1;
this.Controls.Add(this.lblCustomer_id);
this.Controls.Add(this.cmbCustomer_id);

           //#####LeadID###Int64
//属性测试50LeadID
//属性测试50LeadID
//属性测试50LeadID
//属性测试50LeadID
this.lblLeadID.AutoSize = true;
this.lblLeadID.Location = new System.Drawing.Point(100,50);
this.lblLeadID.Name = "lblLeadID";
this.lblLeadID.Size = new System.Drawing.Size(41, 12);
this.lblLeadID.TabIndex = 2;
this.lblLeadID.Text = "线索";
//111======50
this.cmbLeadID.Location = new System.Drawing.Point(173,46);
this.cmbLeadID.Name ="cmbLeadID";
this.cmbLeadID.Size = new System.Drawing.Size(100, 21);
this.cmbLeadID.TabIndex = 2;
this.Controls.Add(this.lblLeadID);
this.Controls.Add(this.cmbLeadID);

           //#####PlanID###Int64
//属性测试75PlanID
this.lblPlanID.AutoSize = true;
this.lblPlanID.Location = new System.Drawing.Point(100,75);
this.lblPlanID.Name = "lblPlanID";
this.lblPlanID.Size = new System.Drawing.Size(41, 12);
this.lblPlanID.TabIndex = 3;
this.lblPlanID.Text = "跟进计划";
//111======75
this.cmbPlanID.Location = new System.Drawing.Point(173,71);
this.cmbPlanID.Name ="cmbPlanID";
this.cmbPlanID.Size = new System.Drawing.Size(100, 21);
this.cmbPlanID.TabIndex = 3;
this.Controls.Add(this.lblPlanID);
this.Controls.Add(this.cmbPlanID);

           //#####Employee_ID###Int64
//属性测试100Employee_ID
//属性测试100Employee_ID
this.lblEmployee_ID.AutoSize = true;
this.lblEmployee_ID.Location = new System.Drawing.Point(100,100);
this.lblEmployee_ID.Name = "lblEmployee_ID";
this.lblEmployee_ID.Size = new System.Drawing.Size(41, 12);
this.lblEmployee_ID.TabIndex = 4;
this.lblEmployee_ID.Text = "跟进人";
//111======100
this.cmbEmployee_ID.Location = new System.Drawing.Point(173,96);
this.cmbEmployee_ID.Name ="cmbEmployee_ID";
this.cmbEmployee_ID.Size = new System.Drawing.Size(100, 21);
this.cmbEmployee_ID.TabIndex = 4;
this.Controls.Add(this.lblEmployee_ID);
this.Controls.Add(this.cmbEmployee_ID);

           //#####FollowUpDate###DateTime
this.lblFollowUpDate.AutoSize = true;
this.lblFollowUpDate.Location = new System.Drawing.Point(100,125);
this.lblFollowUpDate.Name = "lblFollowUpDate";
this.lblFollowUpDate.Size = new System.Drawing.Size(41, 12);
this.lblFollowUpDate.TabIndex = 5;
this.lblFollowUpDate.Text = "跟进日期";
//111======125
this.dtpFollowUpDate.Location = new System.Drawing.Point(173,121);
this.dtpFollowUpDate.Name ="dtpFollowUpDate";
this.dtpFollowUpDate.Size = new System.Drawing.Size(100, 21);
this.dtpFollowUpDate.TabIndex = 5;
this.Controls.Add(this.lblFollowUpDate);
this.Controls.Add(this.dtpFollowUpDate);

           //#####FollowUpMethod###Int32
//属性测试150FollowUpMethod
//属性测试150FollowUpMethod
//属性测试150FollowUpMethod
//属性测试150FollowUpMethod

           //#####200FollowUpSubject###String
this.lblFollowUpSubject.AutoSize = true;
this.lblFollowUpSubject.Location = new System.Drawing.Point(100,175);
this.lblFollowUpSubject.Name = "lblFollowUpSubject";
this.lblFollowUpSubject.Size = new System.Drawing.Size(41, 12);
this.lblFollowUpSubject.TabIndex = 7;
this.lblFollowUpSubject.Text = "跟进主题";
this.txtFollowUpSubject.Location = new System.Drawing.Point(173,171);
this.txtFollowUpSubject.Name = "txtFollowUpSubject";
this.txtFollowUpSubject.Size = new System.Drawing.Size(100, 21);
this.txtFollowUpSubject.TabIndex = 7;
this.Controls.Add(this.lblFollowUpSubject);
this.Controls.Add(this.txtFollowUpSubject);

           //#####1000FollowUpContent###String
this.lblFollowUpContent.AutoSize = true;
this.lblFollowUpContent.Location = new System.Drawing.Point(100,200);
this.lblFollowUpContent.Name = "lblFollowUpContent";
this.lblFollowUpContent.Size = new System.Drawing.Size(41, 12);
this.lblFollowUpContent.TabIndex = 8;
this.lblFollowUpContent.Text = "跟进内容";
this.txtFollowUpContent.Location = new System.Drawing.Point(173,196);
this.txtFollowUpContent.Name = "txtFollowUpContent";
this.txtFollowUpContent.Size = new System.Drawing.Size(100, 21);
this.txtFollowUpContent.TabIndex = 8;
this.Controls.Add(this.lblFollowUpContent);
this.Controls.Add(this.txtFollowUpContent);

           //#####255Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,225);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 9;
this.lblNotes.Text = "备注";
this.txtNotes.Location = new System.Drawing.Point(173,221);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 9;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,250);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 10;
this.lblCreated_at.Text = "创建时间";
//111======250
this.dtpCreated_at.Location = new System.Drawing.Point(173,246);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 10;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
//属性测试275Created_by
//属性测试275Created_by
//属性测试275Created_by
//属性测试275Created_by

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,300);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 12;
this.lblModified_at.Text = "修改时间";
//111======300
this.dtpModified_at.Location = new System.Drawing.Point(173,296);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 12;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
//属性测试325Modified_by
//属性测试325Modified_by
//属性测试325Modified_by
//属性测试325Modified_by

           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,350);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 14;
this.lblisdeleted.Text = "逻辑删除";
this.chkisdeleted.Location = new System.Drawing.Point(173,346);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 14;
this.Controls.Add(this.lblisdeleted);
this.Controls.Add(this.chkisdeleted);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblCustomer_id );
this.Controls.Add(this.cmbCustomer_id );

                this.Controls.Add(this.lblLeadID );
this.Controls.Add(this.cmbLeadID );

                this.Controls.Add(this.lblPlanID );
this.Controls.Add(this.cmbPlanID );

                this.Controls.Add(this.lblEmployee_ID );
this.Controls.Add(this.cmbEmployee_ID );

                this.Controls.Add(this.lblFollowUpDate );
this.Controls.Add(this.dtpFollowUpDate );

                
                this.Controls.Add(this.lblFollowUpSubject );
this.Controls.Add(this.txtFollowUpSubject );

                this.Controls.Add(this.lblFollowUpContent );
this.Controls.Add(this.txtFollowUpContent );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                
                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                
                this.Controls.Add(this.lblisdeleted );
this.Controls.Add(this.chkisdeleted );

                    
            this.Name = "tb_CRM_FollowUpRecordsQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCustomer_id;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbCustomer_id;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblLeadID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbLeadID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPlanID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbPlanID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblFollowUpDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpFollowUpDate;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblFollowUpSubject;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtFollowUpSubject;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblFollowUpContent;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtFollowUpContent;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModified_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblisdeleted;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkisdeleted;

    
    
   
 





    }
}


