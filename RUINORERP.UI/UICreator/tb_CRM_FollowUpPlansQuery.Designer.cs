
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/12/2024 10:37:29
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 跟进计划表
    /// </summary>
    partial class tb_CRM_FollowUpPlansQuery
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

this.lblEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblPlanStartDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpPlanStartDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblPlanEndDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpPlanEndDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblPlanSubject = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPlanSubject = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblPlanContent = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPlanContent = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtPlanContent.Multiline = true;

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

           //#####Employee_ID###Int64
//属性测试50Employee_ID
this.lblEmployee_ID.AutoSize = true;
this.lblEmployee_ID.Location = new System.Drawing.Point(100,50);
this.lblEmployee_ID.Name = "lblEmployee_ID";
this.lblEmployee_ID.Size = new System.Drawing.Size(41, 12);
this.lblEmployee_ID.TabIndex = 2;
this.lblEmployee_ID.Text = "执行人";
//111======50
this.cmbEmployee_ID.Location = new System.Drawing.Point(173,46);
this.cmbEmployee_ID.Name ="cmbEmployee_ID";
this.cmbEmployee_ID.Size = new System.Drawing.Size(100, 21);
this.cmbEmployee_ID.TabIndex = 2;
this.Controls.Add(this.lblEmployee_ID);
this.Controls.Add(this.cmbEmployee_ID);

           //#####PlanStartDate###DateTime
this.lblPlanStartDate.AutoSize = true;
this.lblPlanStartDate.Location = new System.Drawing.Point(100,75);
this.lblPlanStartDate.Name = "lblPlanStartDate";
this.lblPlanStartDate.Size = new System.Drawing.Size(41, 12);
this.lblPlanStartDate.TabIndex = 3;
this.lblPlanStartDate.Text = "开始日期";
//111======75
this.dtpPlanStartDate.Location = new System.Drawing.Point(173,71);
this.dtpPlanStartDate.Name ="dtpPlanStartDate";
this.dtpPlanStartDate.Size = new System.Drawing.Size(100, 21);
this.dtpPlanStartDate.TabIndex = 3;
this.Controls.Add(this.lblPlanStartDate);
this.Controls.Add(this.dtpPlanStartDate);

           //#####PlanEndDate###DateTime
this.lblPlanEndDate.AutoSize = true;
this.lblPlanEndDate.Location = new System.Drawing.Point(100,100);
this.lblPlanEndDate.Name = "lblPlanEndDate";
this.lblPlanEndDate.Size = new System.Drawing.Size(41, 12);
this.lblPlanEndDate.TabIndex = 4;
this.lblPlanEndDate.Text = "结束日期";
//111======100
this.dtpPlanEndDate.Location = new System.Drawing.Point(173,96);
this.dtpPlanEndDate.Name ="dtpPlanEndDate";
this.dtpPlanEndDate.Size = new System.Drawing.Size(100, 21);
this.dtpPlanEndDate.TabIndex = 4;
this.Controls.Add(this.lblPlanEndDate);
this.Controls.Add(this.dtpPlanEndDate);

           //#####PlanStatus###Int32
//属性测试125PlanStatus
//属性测试125PlanStatus

           //#####200PlanSubject###String
this.lblPlanSubject.AutoSize = true;
this.lblPlanSubject.Location = new System.Drawing.Point(100,150);
this.lblPlanSubject.Name = "lblPlanSubject";
this.lblPlanSubject.Size = new System.Drawing.Size(41, 12);
this.lblPlanSubject.TabIndex = 6;
this.lblPlanSubject.Text = "计划主题";
this.txtPlanSubject.Location = new System.Drawing.Point(173,146);
this.txtPlanSubject.Name = "txtPlanSubject";
this.txtPlanSubject.Size = new System.Drawing.Size(100, 21);
this.txtPlanSubject.TabIndex = 6;
this.Controls.Add(this.lblPlanSubject);
this.Controls.Add(this.txtPlanSubject);

           //#####1000PlanContent###String
this.lblPlanContent.AutoSize = true;
this.lblPlanContent.Location = new System.Drawing.Point(100,175);
this.lblPlanContent.Name = "lblPlanContent";
this.lblPlanContent.Size = new System.Drawing.Size(41, 12);
this.lblPlanContent.TabIndex = 7;
this.lblPlanContent.Text = "计划内容";
this.txtPlanContent.Location = new System.Drawing.Point(173,171);
this.txtPlanContent.Name = "txtPlanContent";
this.txtPlanContent.Size = new System.Drawing.Size(100, 21);
this.txtPlanContent.TabIndex = 7;
this.Controls.Add(this.lblPlanContent);
this.Controls.Add(this.txtPlanContent);

           //#####255Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,200);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 8;
this.lblNotes.Text = "备注";
this.txtNotes.Location = new System.Drawing.Point(173,196);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 8;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,225);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 9;
this.lblCreated_at.Text = "创建时间";
//111======225
this.dtpCreated_at.Location = new System.Drawing.Point(173,221);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 9;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
//属性测试250Created_by
//属性测试250Created_by

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,275);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 11;
this.lblModified_at.Text = "修改时间";
//111======275
this.dtpModified_at.Location = new System.Drawing.Point(173,271);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 11;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
//属性测试300Modified_by
//属性测试300Modified_by

           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,325);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 13;
this.lblisdeleted.Text = "逻辑删除";
this.chkisdeleted.Location = new System.Drawing.Point(173,321);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 13;
this.Controls.Add(this.lblisdeleted);
this.Controls.Add(this.chkisdeleted);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblCustomer_id );
this.Controls.Add(this.cmbCustomer_id );

                this.Controls.Add(this.lblEmployee_ID );
this.Controls.Add(this.cmbEmployee_ID );

                this.Controls.Add(this.lblPlanStartDate );
this.Controls.Add(this.dtpPlanStartDate );

                this.Controls.Add(this.lblPlanEndDate );
this.Controls.Add(this.dtpPlanEndDate );

                
                this.Controls.Add(this.lblPlanSubject );
this.Controls.Add(this.txtPlanSubject );

                this.Controls.Add(this.lblPlanContent );
this.Controls.Add(this.txtPlanContent );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                
                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                
                this.Controls.Add(this.lblisdeleted );
this.Controls.Add(this.chkisdeleted );

                    
            this.Name = "tb_CRM_FollowUpPlansQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCustomer_id;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbCustomer_id;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPlanStartDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpPlanStartDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPlanEndDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpPlanEndDate;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPlanSubject;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPlanSubject;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPlanContent;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPlanContent;

    
        
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


