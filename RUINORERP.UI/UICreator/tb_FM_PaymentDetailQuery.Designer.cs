
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/22/2025 12:16:09
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 付款申请单明细-对应的应付单据项目
    /// </summary>
    partial class tb_FM_PaymentDetailQuery
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
     
     tb__DepartmentID主外字段不一致。this.lblPaymentID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbPaymentID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

tb__DepartmentID主外字段不一致。this.lblProjectGroup_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbProjectGroup_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

tb__DepartmentID主外字段不一致。this.lblDepartmentID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbDepartmentID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
this.lblDepartmentID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbDepartmentID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

tb__DepartmentID主外字段不一致。
tb__DepartmentID主外字段不一致。
tb__DepartmentID主外字段不一致。
this.lblSourceBillNO = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSourceBillNO = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblIsAdvancePayment = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIsAdvancePayment = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIsAdvancePayment.Values.Text ="";

this.lblPayReasonItems = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPayReasonItems = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSummary = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSummary = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtSummary.Multiline = true;

this.lblSubAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSubAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSubPamountInWords = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSubPamountInWords = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

tb__DepartmentID主外字段不一致。
this.lblModified_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

tb__DepartmentID主外字段不一致。
    //for end
    this.SuspendLayout();
    
         //for start
                 //#####PaymentID###Int64
//属性测试25PaymentID
tb__DepartmentID主外字段不一致。//属性测试25PaymentID
this.lblPaymentID.AutoSize = true;
this.lblPaymentID.Location = new System.Drawing.Point(100,25);
this.lblPaymentID.Name = "lblPaymentID";
this.lblPaymentID.Size = new System.Drawing.Size(41, 12);
this.lblPaymentID.TabIndex = 1;
this.lblPaymentID.Text = "付款申请单";
//111======25
this.cmbPaymentID.Location = new System.Drawing.Point(173,21);
this.cmbPaymentID.Name ="cmbPaymentID";
this.cmbPaymentID.Size = new System.Drawing.Size(100, 21);
this.cmbPaymentID.TabIndex = 1;
this.Controls.Add(this.lblPaymentID);
this.Controls.Add(this.cmbPaymentID);

           //#####ProjectGroup_ID###Int64
//属性测试50ProjectGroup_ID
tb__DepartmentID主外字段不一致。//属性测试50ProjectGroup_ID
//属性测试50ProjectGroup_ID
//属性测试50ProjectGroup_ID
this.lblProjectGroup_ID.AutoSize = true;
this.lblProjectGroup_ID.Location = new System.Drawing.Point(100,50);
this.lblProjectGroup_ID.Name = "lblProjectGroup_ID";
this.lblProjectGroup_ID.Size = new System.Drawing.Size(41, 12);
this.lblProjectGroup_ID.TabIndex = 2;
this.lblProjectGroup_ID.Text = "项目组";
//111======50
this.cmbProjectGroup_ID.Location = new System.Drawing.Point(173,46);
this.cmbProjectGroup_ID.Name ="cmbProjectGroup_ID";
this.cmbProjectGroup_ID.Size = new System.Drawing.Size(100, 21);
this.cmbProjectGroup_ID.TabIndex = 2;
this.Controls.Add(this.lblProjectGroup_ID);
this.Controls.Add(this.cmbProjectGroup_ID);

           //#####DepartmentID###Int64
//属性测试75DepartmentID
tb__DepartmentID主外字段不一致。this.lblDepartmentID.AutoSize = true;
this.lblDepartmentID.Location = new System.Drawing.Point(100,75);
this.lblDepartmentID.Name = "lblDepartmentID";
this.lblDepartmentID.Size = new System.Drawing.Size(41, 12);
this.lblDepartmentID.TabIndex = 3;
this.lblDepartmentID.Text = "部门";
//111======75
this.cmbDepartmentID.Location = new System.Drawing.Point(173,71);
this.cmbDepartmentID.Name ="cmbDepartmentID";
this.cmbDepartmentID.Size = new System.Drawing.Size(100, 21);
this.cmbDepartmentID.TabIndex = 3;
this.Controls.Add(this.lblDepartmentID);
this.Controls.Add(this.cmbDepartmentID);

           //#####tb__DepartmentID###Int64
//属性测试100tb__DepartmentID
tb__DepartmentID主外字段不一致。//属性测试100tb__DepartmentID
//属性测试100tb__DepartmentID
//属性测试100tb__DepartmentID

           //#####SourceBill_BizType###Int32
//属性测试125SourceBill_BizType
tb__DepartmentID主外字段不一致。//属性测试125SourceBill_BizType
//属性测试125SourceBill_BizType
//属性测试125SourceBill_BizType

           //#####SourceBill_ID###Int64
//属性测试150SourceBill_ID
tb__DepartmentID主外字段不一致。//属性测试150SourceBill_ID
//属性测试150SourceBill_ID
//属性测试150SourceBill_ID

           //#####30SourceBillNO###String
this.lblSourceBillNO.AutoSize = true;
this.lblSourceBillNO.Location = new System.Drawing.Point(100,175);
this.lblSourceBillNO.Name = "lblSourceBillNO";
this.lblSourceBillNO.Size = new System.Drawing.Size(41, 12);
this.lblSourceBillNO.TabIndex = 7;
this.lblSourceBillNO.Text = "来源单号";
this.txtSourceBillNO.Location = new System.Drawing.Point(173,171);
this.txtSourceBillNO.Name = "txtSourceBillNO";
this.txtSourceBillNO.Size = new System.Drawing.Size(100, 21);
this.txtSourceBillNO.TabIndex = 7;
this.Controls.Add(this.lblSourceBillNO);
this.Controls.Add(this.txtSourceBillNO);

           //#####IsAdvancePayment###Boolean
this.lblIsAdvancePayment.AutoSize = true;
this.lblIsAdvancePayment.Location = new System.Drawing.Point(100,200);
this.lblIsAdvancePayment.Name = "lblIsAdvancePayment";
this.lblIsAdvancePayment.Size = new System.Drawing.Size(41, 12);
this.lblIsAdvancePayment.TabIndex = 8;
this.lblIsAdvancePayment.Text = "为预付款";
this.chkIsAdvancePayment.Location = new System.Drawing.Point(173,196);
this.chkIsAdvancePayment.Name = "chkIsAdvancePayment";
this.chkIsAdvancePayment.Size = new System.Drawing.Size(100, 21);
this.chkIsAdvancePayment.TabIndex = 8;
this.Controls.Add(this.lblIsAdvancePayment);
this.Controls.Add(this.chkIsAdvancePayment);

           //#####200PayReasonItems###String
this.lblPayReasonItems.AutoSize = true;
this.lblPayReasonItems.Location = new System.Drawing.Point(100,225);
this.lblPayReasonItems.Name = "lblPayReasonItems";
this.lblPayReasonItems.Size = new System.Drawing.Size(41, 12);
this.lblPayReasonItems.TabIndex = 9;
this.lblPayReasonItems.Text = "付款项目/原因";
this.txtPayReasonItems.Location = new System.Drawing.Point(173,221);
this.txtPayReasonItems.Name = "txtPayReasonItems";
this.txtPayReasonItems.Size = new System.Drawing.Size(100, 21);
this.txtPayReasonItems.TabIndex = 9;
this.Controls.Add(this.lblPayReasonItems);
this.Controls.Add(this.txtPayReasonItems);

           //#####300Summary###String
this.lblSummary.AutoSize = true;
this.lblSummary.Location = new System.Drawing.Point(100,250);
this.lblSummary.Name = "lblSummary";
this.lblSummary.Size = new System.Drawing.Size(41, 12);
this.lblSummary.TabIndex = 10;
this.lblSummary.Text = "摘要";
this.txtSummary.Location = new System.Drawing.Point(173,246);
this.txtSummary.Name = "txtSummary";
this.txtSummary.Size = new System.Drawing.Size(100, 21);
this.txtSummary.TabIndex = 10;
this.Controls.Add(this.lblSummary);
this.Controls.Add(this.txtSummary);

           //#####SubAmount###Decimal
this.lblSubAmount.AutoSize = true;
this.lblSubAmount.Location = new System.Drawing.Point(100,275);
this.lblSubAmount.Name = "lblSubAmount";
this.lblSubAmount.Size = new System.Drawing.Size(41, 12);
this.lblSubAmount.TabIndex = 11;
this.lblSubAmount.Text = "付款金额";
//111======275
this.txtSubAmount.Location = new System.Drawing.Point(173,271);
this.txtSubAmount.Name ="txtSubAmount";
this.txtSubAmount.Size = new System.Drawing.Size(100, 21);
this.txtSubAmount.TabIndex = 11;
this.Controls.Add(this.lblSubAmount);
this.Controls.Add(this.txtSubAmount);

           //#####100SubPamountInWords###String
this.lblSubPamountInWords.AutoSize = true;
this.lblSubPamountInWords.Location = new System.Drawing.Point(100,300);
this.lblSubPamountInWords.Name = "lblSubPamountInWords";
this.lblSubPamountInWords.Size = new System.Drawing.Size(41, 12);
this.lblSubPamountInWords.TabIndex = 12;
this.lblSubPamountInWords.Text = "大写金额";
this.txtSubPamountInWords.Location = new System.Drawing.Point(173,296);
this.txtSubPamountInWords.Name = "txtSubPamountInWords";
this.txtSubPamountInWords.Size = new System.Drawing.Size(100, 21);
this.txtSubPamountInWords.TabIndex = 12;
this.Controls.Add(this.lblSubPamountInWords);
this.Controls.Add(this.txtSubPamountInWords);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,325);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 13;
this.lblCreated_at.Text = "创建时间";
//111======325
this.dtpCreated_at.Location = new System.Drawing.Point(173,321);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 13;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
//属性测试350Created_by
tb__DepartmentID主外字段不一致。//属性测试350Created_by
//属性测试350Created_by
//属性测试350Created_by

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,375);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 15;
this.lblModified_at.Text = "修改时间";
//111======375
this.dtpModified_at.Location = new System.Drawing.Point(173,371);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 15;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
//属性测试400Modified_by
tb__DepartmentID主外字段不一致。//属性测试400Modified_by
//属性测试400Modified_by
//属性测试400Modified_by

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                tb__DepartmentID主外字段不一致。this.Controls.Add(this.lblPaymentID );
this.Controls.Add(this.cmbPaymentID );

                tb__DepartmentID主外字段不一致。this.Controls.Add(this.lblProjectGroup_ID );
this.Controls.Add(this.cmbProjectGroup_ID );

                tb__DepartmentID主外字段不一致。this.Controls.Add(this.lblDepartmentID );
this.Controls.Add(this.cmbDepartmentID );

                tb__DepartmentID主外字段不一致。
                tb__DepartmentID主外字段不一致。
                tb__DepartmentID主外字段不一致。
                this.Controls.Add(this.lblSourceBillNO );
this.Controls.Add(this.txtSourceBillNO );

                this.Controls.Add(this.lblIsAdvancePayment );
this.Controls.Add(this.chkIsAdvancePayment );

                this.Controls.Add(this.lblPayReasonItems );
this.Controls.Add(this.txtPayReasonItems );

                this.Controls.Add(this.lblSummary );
this.Controls.Add(this.txtSummary );

                this.Controls.Add(this.lblSubAmount );
this.Controls.Add(this.txtSubAmount );

                this.Controls.Add(this.lblSubPamountInWords );
this.Controls.Add(this.txtSubPamountInWords );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                tb__DepartmentID主外字段不一致。
                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                tb__DepartmentID主外字段不一致。
                    
            this.Name = "tb_FM_PaymentDetailQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              tb__DepartmentID主外字段不一致。private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPaymentID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbPaymentID;

    
        
              tb__DepartmentID主外字段不一致。private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProjectGroup_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbProjectGroup_ID;

    
        
              tb__DepartmentID主外字段不一致。private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDepartmentID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbDepartmentID;
private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDepartmentID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbDepartmentID;

    
        
              tb__DepartmentID主外字段不一致。
    
        
              tb__DepartmentID主外字段不一致。
    
        
              tb__DepartmentID主外字段不一致。
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSourceBillNO;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSourceBillNO;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIsAdvancePayment;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIsAdvancePayment;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPayReasonItems;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPayReasonItems;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSummary;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSummary;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSubAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSubAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSubPamountInWords;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSubPamountInWords;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              tb__DepartmentID主外字段不一致。
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModified_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              tb__DepartmentID主外字段不一致。
    
    
   
 





    }
}


