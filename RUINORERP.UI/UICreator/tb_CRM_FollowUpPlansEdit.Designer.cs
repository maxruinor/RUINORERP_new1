// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：12/09/2024 21:09:55
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
            this.lblNotes = new Krypton.Toolkit.KryptonLabel();
            this.txtNotes = new Krypton.Toolkit.KryptonTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCustomer_id)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEmployee_ID)).BeginInit();
            this.SuspendLayout();
            // 
            // lblCustomer_id
            // 
            this.lblCustomer_id.Location = new System.Drawing.Point(100, 25);
            this.lblCustomer_id.Name = "lblCustomer_id";
            this.lblCustomer_id.Size = new System.Drawing.Size(62, 20);
            this.lblCustomer_id.TabIndex = 1;
            this.lblCustomer_id.Values.Text = "目标客户";
            // 
            // cmbCustomer_id
            // 
            this.cmbCustomer_id.DropDownWidth = 100;
            this.cmbCustomer_id.IntegralHeight = false;
            this.cmbCustomer_id.Location = new System.Drawing.Point(173, 21);
            this.cmbCustomer_id.Name = "cmbCustomer_id";
            this.cmbCustomer_id.Size = new System.Drawing.Size(100, 21);
            this.cmbCustomer_id.TabIndex = 1;
            // 
            // lblEmployee_ID
            // 
            this.lblEmployee_ID.Location = new System.Drawing.Point(100, 50);
            this.lblEmployee_ID.Name = "lblEmployee_ID";
            this.lblEmployee_ID.Size = new System.Drawing.Size(49, 20);
            this.lblEmployee_ID.TabIndex = 2;
            this.lblEmployee_ID.Values.Text = "执行人";
            // 
            // cmbEmployee_ID
            // 
            this.cmbEmployee_ID.DropDownWidth = 100;
            this.cmbEmployee_ID.IntegralHeight = false;
            this.cmbEmployee_ID.Location = new System.Drawing.Point(173, 46);
            this.cmbEmployee_ID.Name = "cmbEmployee_ID";
            this.cmbEmployee_ID.Size = new System.Drawing.Size(100, 21);
            this.cmbEmployee_ID.TabIndex = 2;
            // 
            // lblPlanStartDate
            // 
            this.lblPlanStartDate.Location = new System.Drawing.Point(100, 75);
            this.lblPlanStartDate.Name = "lblPlanStartDate";
            this.lblPlanStartDate.Size = new System.Drawing.Size(62, 20);
            this.lblPlanStartDate.TabIndex = 3;
            this.lblPlanStartDate.Values.Text = "开始日期";
            // 
            // dtpPlanStartDate
            // 
            this.dtpPlanStartDate.Location = new System.Drawing.Point(173, 71);
            this.dtpPlanStartDate.Name = "dtpPlanStartDate";
            this.dtpPlanStartDate.Size = new System.Drawing.Size(100, 21);
            this.dtpPlanStartDate.TabIndex = 3;
            // 
            // lblPlanEndDate
            // 
            this.lblPlanEndDate.Location = new System.Drawing.Point(100, 100);
            this.lblPlanEndDate.Name = "lblPlanEndDate";
            this.lblPlanEndDate.Size = new System.Drawing.Size(62, 20);
            this.lblPlanEndDate.TabIndex = 4;
            this.lblPlanEndDate.Values.Text = "结束日期";
            // 
            // dtpPlanEndDate
            // 
            this.dtpPlanEndDate.Location = new System.Drawing.Point(173, 96);
            this.dtpPlanEndDate.Name = "dtpPlanEndDate";
            this.dtpPlanEndDate.Size = new System.Drawing.Size(100, 21);
            this.dtpPlanEndDate.TabIndex = 4;
            // 
            // lblPlanStatus
            // 
            this.lblPlanStatus.Location = new System.Drawing.Point(100, 125);
            this.lblPlanStatus.Name = "lblPlanStatus";
            this.lblPlanStatus.Size = new System.Drawing.Size(62, 20);
            this.lblPlanStatus.TabIndex = 5;
            this.lblPlanStatus.Values.Text = "计划状态";
            // 
            // txtPlanStatus
            // 
            this.txtPlanStatus.Location = new System.Drawing.Point(173, 121);
            this.txtPlanStatus.Name = "txtPlanStatus";
            this.txtPlanStatus.Size = new System.Drawing.Size(100, 23);
            this.txtPlanStatus.TabIndex = 5;
            // 
            // lblPlanSubject
            // 
            this.lblPlanSubject.Location = new System.Drawing.Point(100, 150);
            this.lblPlanSubject.Name = "lblPlanSubject";
            this.lblPlanSubject.Size = new System.Drawing.Size(62, 20);
            this.lblPlanSubject.TabIndex = 6;
            this.lblPlanSubject.Values.Text = "计划主题";
            // 
            // txtPlanSubject
            // 
            this.txtPlanSubject.Location = new System.Drawing.Point(173, 146);
            this.txtPlanSubject.Name = "txtPlanSubject";
            this.txtPlanSubject.Size = new System.Drawing.Size(100, 23);
            this.txtPlanSubject.TabIndex = 6;
            // 
            // lblPlanContent
            // 
            this.lblPlanContent.Location = new System.Drawing.Point(100, 175);
            this.lblPlanContent.Name = "lblPlanContent";
            this.lblPlanContent.Size = new System.Drawing.Size(62, 20);
            this.lblPlanContent.TabIndex = 7;
            this.lblPlanContent.Values.Text = "计划内容";
            // 
            // txtPlanContent
            // 
            this.txtPlanContent.Location = new System.Drawing.Point(173, 171);
            this.txtPlanContent.Multiline = true;
            this.txtPlanContent.Name = "txtPlanContent";
            this.txtPlanContent.Size = new System.Drawing.Size(100, 21);
            this.txtPlanContent.TabIndex = 7;
            // 
            // lblNotes
            // 
            this.lblNotes.Location = new System.Drawing.Point(100, 200);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(36, 20);
            this.lblNotes.TabIndex = 8;
            this.lblNotes.Values.Text = "备注";
            // 
            // txtNotes
            // 
            this.txtNotes.Location = new System.Drawing.Point(173, 196);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(100, 21);
            this.txtNotes.TabIndex = 8;
            // 
            // tb_CRM_FollowUpPlansEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblCustomer_id);
            this.Controls.Add(this.cmbCustomer_id);
            this.Controls.Add(this.lblEmployee_ID);
            this.Controls.Add(this.cmbEmployee_ID);
            this.Controls.Add(this.lblPlanStartDate);
            this.Controls.Add(this.dtpPlanStartDate);
            this.Controls.Add(this.lblPlanEndDate);
            this.Controls.Add(this.dtpPlanEndDate);
            this.Controls.Add(this.lblPlanStatus);
            this.Controls.Add(this.txtPlanStatus);
            this.Controls.Add(this.lblPlanSubject);
            this.Controls.Add(this.txtPlanSubject);
            this.Controls.Add(this.lblPlanContent);
            this.Controls.Add(this.txtPlanContent);
            this.Controls.Add(this.lblNotes);
            this.Controls.Add(this.txtNotes);
            this.Name = "tb_CRM_FollowUpPlansEdit";
            this.Size = new System.Drawing.Size(911, 490);
            ((System.ComponentModel.ISupportInitialize)(this.cmbCustomer_id)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEmployee_ID)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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






    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

