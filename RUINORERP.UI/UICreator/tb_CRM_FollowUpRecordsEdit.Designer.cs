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
    partial class tb_CRM_FollowUpRecordsEdit
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

this.lblLeadID = new Krypton.Toolkit.KryptonLabel();
this.cmbLeadID = new Krypton.Toolkit.KryptonComboBox();

this.lblPlanID = new Krypton.Toolkit.KryptonLabel();
this.cmbPlanID = new Krypton.Toolkit.KryptonComboBox();

this.lblEmployee_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbEmployee_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblFollowUpDate = new Krypton.Toolkit.KryptonLabel();
this.dtpFollowUpDate = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblFollowUpMethod = new Krypton.Toolkit.KryptonLabel();
this.txtFollowUpMethod = new Krypton.Toolkit.KryptonTextBox();

this.lblFollowUpSubject = new Krypton.Toolkit.KryptonLabel();
this.txtFollowUpSubject = new Krypton.Toolkit.KryptonTextBox();

this.lblFollowUpContent = new Krypton.Toolkit.KryptonLabel();
this.txtFollowUpContent = new Krypton.Toolkit.KryptonTextBox();
this.txtFollowUpContent.Multiline = true;

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
this.lblFollowUpMethod.AutoSize = true;
this.lblFollowUpMethod.Location = new System.Drawing.Point(100,150);
this.lblFollowUpMethod.Name = "lblFollowUpMethod";
this.lblFollowUpMethod.Size = new System.Drawing.Size(41, 12);
this.lblFollowUpMethod.TabIndex = 6;
this.lblFollowUpMethod.Text = "跟进方式";
this.txtFollowUpMethod.Location = new System.Drawing.Point(173,146);
this.txtFollowUpMethod.Name = "txtFollowUpMethod";
this.txtFollowUpMethod.Size = new System.Drawing.Size(100, 21);
this.txtFollowUpMethod.TabIndex = 6;
this.Controls.Add(this.lblFollowUpMethod);
this.Controls.Add(this.txtFollowUpMethod);

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
this.lblCreated_by.AutoSize = true;
this.lblCreated_by.Location = new System.Drawing.Point(100,275);
this.lblCreated_by.Name = "lblCreated_by";
this.lblCreated_by.Size = new System.Drawing.Size(41, 12);
this.lblCreated_by.TabIndex = 11;
this.lblCreated_by.Text = "创建人";
this.txtCreated_by.Location = new System.Drawing.Point(173,271);
this.txtCreated_by.Name = "txtCreated_by";
this.txtCreated_by.Size = new System.Drawing.Size(100, 21);
this.txtCreated_by.TabIndex = 11;
this.Controls.Add(this.lblCreated_by);
this.Controls.Add(this.txtCreated_by);

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
this.lblModified_by.AutoSize = true;
this.lblModified_by.Location = new System.Drawing.Point(100,325);
this.lblModified_by.Name = "lblModified_by";
this.lblModified_by.Size = new System.Drawing.Size(41, 12);
this.lblModified_by.TabIndex = 13;
this.lblModified_by.Text = "修改人";
this.txtModified_by.Location = new System.Drawing.Point(173,321);
this.txtModified_by.Name = "txtModified_by";
this.txtModified_by.Size = new System.Drawing.Size(100, 21);
this.txtModified_by.TabIndex = 13;
this.Controls.Add(this.lblModified_by);
this.Controls.Add(this.txtModified_by);

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
           // this.kryptonPanel1.TabIndex = 14;

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

                this.Controls.Add(this.lblFollowUpMethod );
this.Controls.Add(this.txtFollowUpMethod );

                this.Controls.Add(this.lblFollowUpSubject );
this.Controls.Add(this.txtFollowUpSubject );

                this.Controls.Add(this.lblFollowUpContent );
this.Controls.Add(this.txtFollowUpContent );

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
            // "tb_CRM_FollowUpRecordsEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_CRM_FollowUpRecordsEdit";
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

    
        
              private Krypton.Toolkit.KryptonLabel lblLeadID;
private Krypton.Toolkit.KryptonComboBox cmbLeadID;

    
        
              private Krypton.Toolkit.KryptonLabel lblPlanID;
private Krypton.Toolkit.KryptonComboBox cmbPlanID;

    
        
              private Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblFollowUpDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpFollowUpDate;

    
        
              private Krypton.Toolkit.KryptonLabel lblFollowUpMethod;
private Krypton.Toolkit.KryptonTextBox txtFollowUpMethod;

    
        
              private Krypton.Toolkit.KryptonLabel lblFollowUpSubject;
private Krypton.Toolkit.KryptonTextBox txtFollowUpSubject;

    
        
              private Krypton.Toolkit.KryptonLabel lblFollowUpContent;
private Krypton.Toolkit.KryptonTextBox txtFollowUpContent;

    
        
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

