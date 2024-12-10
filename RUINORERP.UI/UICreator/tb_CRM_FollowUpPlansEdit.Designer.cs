// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/10/2024 13:12:13
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
    partial class tb_CRM_FollowUpPlansEdit
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
        
     //for definition
     // this.btnOk = new ComponentFactory.Krypton.Toolkit.KryptonButton();
     // this.btnCancel = new ComponentFactory.Krypton.Toolkit.KryptonButton();
      //this.kryptonPanel1 = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
      //for definition
     this.lblCustomer_id = new Krypton.Toolkit.KryptonLabel();
this.cmbCustomer_id = new Krypton.Toolkit.KryptonComboBox();

this.lblEmployee_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbEmployee_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblPlanStartDate = new Krypton.Toolkit.KryptonLabel();
this.dtpPlanStartDate = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblPlanEndDate = new Krypton.Toolkit.KryptonLabel();
this.dtpPlanEndDate = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblPlanStatus = new Krypton.Toolkit.KryptonLabel();
this.txtPlanStatus = new Krypton.Toolkit.KryptonTextBox();

this.lblPlanSubject = new Krypton.Toolkit.KryptonLabel();
this.txtPlanSubject = new Krypton.Toolkit.KryptonTextBox();

this.lblPlanContent = new Krypton.Toolkit.KryptonLabel();
this.txtPlanContent = new Krypton.Toolkit.KryptonTextBox();
this.txtPlanContent.Multiline = true;

this.lblNotes = new Krypton.Toolkit.KryptonLabel();
this.txtNotes = new Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

this.lblCreated_at = new Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblCreated_by = new Krypton.Toolkit.KryptonLabel();
this.txtCreated_by = new Krypton.Toolkit.KryptonTextBox();

this.lblModified_at = new Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblModified_by = new Krypton.Toolkit.KryptonLabel();
this.txtModified_by = new Krypton.Toolkit.KryptonTextBox();

this.lblisdeleted = new Krypton.Toolkit.KryptonLabel();
this.chkisdeleted = new Krypton.Toolkit.KryptonCheckBox();
this.chkisdeleted.Values.Text ="";

    
    //for end
   // ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
   // this.kryptonPanel1.SuspendLayout();
    this.SuspendLayout();
    
            // 
            // btnOk
            // 
            //this.btnOk.Location = new System.Drawing.Point(126, 355);
            //this.btnOk.Name = "btnOk";
            //this.btnOk.Size = new System.Drawing.Size(90, 25);
            //this.btnOk.TabIndex = 0;
           // this.btnOk.Values.Text = "确定";
            //this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
           // this.btnCancel.Location = new System.Drawing.Point(244, 355);
            //this.btnCancel.Name = "btnCancel";
            //this.btnCancel.Size = new System.Drawing.Size(90, 25);
            //this.btnCancel.TabIndex = 1;
            //this.btnCancel.Values.Text = "取消";
           // this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            
         //for size
     
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
this.lblPlanStatus.AutoSize = true;
this.lblPlanStatus.Location = new System.Drawing.Point(100,125);
this.lblPlanStatus.Name = "lblPlanStatus";
this.lblPlanStatus.Size = new System.Drawing.Size(41, 12);
this.lblPlanStatus.TabIndex = 5;
this.lblPlanStatus.Text = "计划状态";
this.txtPlanStatus.Location = new System.Drawing.Point(173,121);
this.txtPlanStatus.Name = "txtPlanStatus";
this.txtPlanStatus.Size = new System.Drawing.Size(100, 21);
this.txtPlanStatus.TabIndex = 5;
this.Controls.Add(this.lblPlanStatus);
this.Controls.Add(this.txtPlanStatus);

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
this.lblCreated_by.AutoSize = true;
this.lblCreated_by.Location = new System.Drawing.Point(100,250);
this.lblCreated_by.Name = "lblCreated_by";
this.lblCreated_by.Size = new System.Drawing.Size(41, 12);
this.lblCreated_by.TabIndex = 10;
this.lblCreated_by.Text = "创建人";
this.txtCreated_by.Location = new System.Drawing.Point(173,246);
this.txtCreated_by.Name = "txtCreated_by";
this.txtCreated_by.Size = new System.Drawing.Size(100, 21);
this.txtCreated_by.TabIndex = 10;
this.Controls.Add(this.lblCreated_by);
this.Controls.Add(this.txtCreated_by);

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
this.lblModified_by.AutoSize = true;
this.lblModified_by.Location = new System.Drawing.Point(100,300);
this.lblModified_by.Name = "lblModified_by";
this.lblModified_by.Size = new System.Drawing.Size(41, 12);
this.lblModified_by.TabIndex = 12;
this.lblModified_by.Text = "修改人";
this.txtModified_by.Location = new System.Drawing.Point(173,296);
this.txtModified_by.Name = "txtModified_by";
this.txtModified_by.Size = new System.Drawing.Size(100, 21);
this.txtModified_by.TabIndex = 12;
this.Controls.Add(this.lblModified_by);
this.Controls.Add(this.txtModified_by);

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

        //for 加入到容器
            //components = new System.ComponentModel.Container();
           
            //this.Controls.Add(this.btnCancel);
            //this.Controls.Add(this.btnOk);
            // 
            // kryptonPanel1
            // 
          //  this.kryptonPanel1.Controls.Add(this.btnCancel);
         //   this.kryptonPanel1.Controls.Add(this.btnOk);
           // this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            //this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
           // this.kryptonPanel1.Name = "kryptonPanel1";
           // this.kryptonPanel1.Size = new System.Drawing.Size(404, 300);
           // this.kryptonPanel1.TabIndex = 13;

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

                this.Controls.Add(this.lblPlanStatus );
this.Controls.Add(this.txtPlanStatus );

                this.Controls.Add(this.lblPlanSubject );
this.Controls.Add(this.txtPlanSubject );

                this.Controls.Add(this.lblPlanContent );
this.Controls.Add(this.txtPlanContent );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                this.Controls.Add(this.lblCreated_by );
this.Controls.Add(this.txtCreated_by );

                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                this.Controls.Add(this.lblModified_by );
this.Controls.Add(this.txtModified_by );

                this.Controls.Add(this.lblisdeleted );
this.Controls.Add(this.chkisdeleted );

                            // 
            // "tb_CRM_FollowUpPlansEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_CRM_FollowUpPlansEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblCustomer_id;
private Krypton.Toolkit.KryptonComboBox cmbCustomer_id;

    
        
              private Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblPlanStartDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpPlanStartDate;

    
        
              private Krypton.Toolkit.KryptonLabel lblPlanEndDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpPlanEndDate;

    
        
              private Krypton.Toolkit.KryptonLabel lblPlanStatus;
private Krypton.Toolkit.KryptonTextBox txtPlanStatus;

    
        
              private Krypton.Toolkit.KryptonLabel lblPlanSubject;
private Krypton.Toolkit.KryptonTextBox txtPlanSubject;

    
        
              private Krypton.Toolkit.KryptonLabel lblPlanContent;
private Krypton.Toolkit.KryptonTextBox txtPlanContent;

    
        
              private Krypton.Toolkit.KryptonLabel lblNotes;
private Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private Krypton.Toolkit.KryptonLabel lblCreated_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              private Krypton.Toolkit.KryptonLabel lblCreated_by;
private Krypton.Toolkit.KryptonTextBox txtCreated_by;

    
        
              private Krypton.Toolkit.KryptonLabel lblModified_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              private Krypton.Toolkit.KryptonLabel lblModified_by;
private Krypton.Toolkit.KryptonTextBox txtModified_by;

    
        
              private Krypton.Toolkit.KryptonLabel lblisdeleted;
private Krypton.Toolkit.KryptonCheckBox chkisdeleted;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

