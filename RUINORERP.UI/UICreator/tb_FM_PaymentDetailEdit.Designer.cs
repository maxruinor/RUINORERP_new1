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
    partial class tb_FM_PaymentDetailEdit
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
     tb__DepartmentID主外字段不一致。this.lblPaymentID = new Krypton.Toolkit.KryptonLabel();
this.cmbPaymentID = new Krypton.Toolkit.KryptonComboBox();

tb__DepartmentID主外字段不一致。this.lblProjectGroup_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbProjectGroup_ID = new Krypton.Toolkit.KryptonComboBox();

tb__DepartmentID主外字段不一致。this.lblDepartmentID = new Krypton.Toolkit.KryptonLabel();
this.cmbDepartmentID = new Krypton.Toolkit.KryptonComboBox();
this.lblDepartmentID = new Krypton.Toolkit.KryptonLabel();
this.cmbDepartmentID = new Krypton.Toolkit.KryptonComboBox();

tb__DepartmentID主外字段不一致。this.lbltb__DepartmentID = new Krypton.Toolkit.KryptonLabel();
this.txttb__DepartmentID = new Krypton.Toolkit.KryptonTextBox();

tb__DepartmentID主外字段不一致。this.lblSourceBill_BizType = new Krypton.Toolkit.KryptonLabel();
this.txtSourceBill_BizType = new Krypton.Toolkit.KryptonTextBox();

tb__DepartmentID主外字段不一致。this.lblSourceBill_ID = new Krypton.Toolkit.KryptonLabel();
this.txtSourceBill_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblSourceBillNO = new Krypton.Toolkit.KryptonLabel();
this.txtSourceBillNO = new Krypton.Toolkit.KryptonTextBox();

this.lblIsAdvancePayment = new Krypton.Toolkit.KryptonLabel();
this.chkIsAdvancePayment = new Krypton.Toolkit.KryptonCheckBox();
this.chkIsAdvancePayment.Values.Text ="";

this.lblPayReasonItems = new Krypton.Toolkit.KryptonLabel();
this.txtPayReasonItems = new Krypton.Toolkit.KryptonTextBox();

this.lblSummary = new Krypton.Toolkit.KryptonLabel();
this.txtSummary = new Krypton.Toolkit.KryptonTextBox();
this.txtSummary.Multiline = true;

this.lblSubAmount = new Krypton.Toolkit.KryptonLabel();
this.txtSubAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblSubPamountInWords = new Krypton.Toolkit.KryptonLabel();
this.txtSubPamountInWords = new Krypton.Toolkit.KryptonTextBox();

this.lblCreated_at = new Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new Krypton.Toolkit.KryptonDateTimePicker();

tb__DepartmentID主外字段不一致。this.lblCreated_by = new Krypton.Toolkit.KryptonLabel();
this.txtCreated_by = new Krypton.Toolkit.KryptonTextBox();

this.lblModified_at = new Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new Krypton.Toolkit.KryptonDateTimePicker();

tb__DepartmentID主外字段不一致。this.lblModified_by = new Krypton.Toolkit.KryptonLabel();
this.txtModified_by = new Krypton.Toolkit.KryptonTextBox();

    
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
this.lbltb__DepartmentID.AutoSize = true;
this.lbltb__DepartmentID.Location = new System.Drawing.Point(100,100);
this.lbltb__DepartmentID.Name = "lbltb__DepartmentID";
this.lbltb__DepartmentID.Size = new System.Drawing.Size(41, 12);
this.lbltb__DepartmentID.TabIndex = 4;
this.lbltb__DepartmentID.Text = "部门";
this.txttb__DepartmentID.Location = new System.Drawing.Point(173,96);
this.txttb__DepartmentID.Name = "txttb__DepartmentID";
this.txttb__DepartmentID.Size = new System.Drawing.Size(100, 21);
this.txttb__DepartmentID.TabIndex = 4;
this.Controls.Add(this.lbltb__DepartmentID);
this.Controls.Add(this.txttb__DepartmentID);

           //#####SourceBill_BizType###Int32
//属性测试125SourceBill_BizType
tb__DepartmentID主外字段不一致。//属性测试125SourceBill_BizType
//属性测试125SourceBill_BizType
//属性测试125SourceBill_BizType
this.lblSourceBill_BizType.AutoSize = true;
this.lblSourceBill_BizType.Location = new System.Drawing.Point(100,125);
this.lblSourceBill_BizType.Name = "lblSourceBill_BizType";
this.lblSourceBill_BizType.Size = new System.Drawing.Size(41, 12);
this.lblSourceBill_BizType.TabIndex = 5;
this.lblSourceBill_BizType.Text = "来源业务";
this.txtSourceBill_BizType.Location = new System.Drawing.Point(173,121);
this.txtSourceBill_BizType.Name = "txtSourceBill_BizType";
this.txtSourceBill_BizType.Size = new System.Drawing.Size(100, 21);
this.txtSourceBill_BizType.TabIndex = 5;
this.Controls.Add(this.lblSourceBill_BizType);
this.Controls.Add(this.txtSourceBill_BizType);

           //#####SourceBill_ID###Int64
//属性测试150SourceBill_ID
tb__DepartmentID主外字段不一致。//属性测试150SourceBill_ID
//属性测试150SourceBill_ID
//属性测试150SourceBill_ID
this.lblSourceBill_ID.AutoSize = true;
this.lblSourceBill_ID.Location = new System.Drawing.Point(100,150);
this.lblSourceBill_ID.Name = "lblSourceBill_ID";
this.lblSourceBill_ID.Size = new System.Drawing.Size(41, 12);
this.lblSourceBill_ID.TabIndex = 6;
this.lblSourceBill_ID.Text = "来源单据";
this.txtSourceBill_ID.Location = new System.Drawing.Point(173,146);
this.txtSourceBill_ID.Name = "txtSourceBill_ID";
this.txtSourceBill_ID.Size = new System.Drawing.Size(100, 21);
this.txtSourceBill_ID.TabIndex = 6;
this.Controls.Add(this.lblSourceBill_ID);
this.Controls.Add(this.txtSourceBill_ID);

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
this.lblCreated_by.AutoSize = true;
this.lblCreated_by.Location = new System.Drawing.Point(100,350);
this.lblCreated_by.Name = "lblCreated_by";
this.lblCreated_by.Size = new System.Drawing.Size(41, 12);
this.lblCreated_by.TabIndex = 14;
this.lblCreated_by.Text = "创建人";
this.txtCreated_by.Location = new System.Drawing.Point(173,346);
this.txtCreated_by.Name = "txtCreated_by";
this.txtCreated_by.Size = new System.Drawing.Size(100, 21);
this.txtCreated_by.TabIndex = 14;
this.Controls.Add(this.lblCreated_by);
this.Controls.Add(this.txtCreated_by);

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
this.lblModified_by.AutoSize = true;
this.lblModified_by.Location = new System.Drawing.Point(100,400);
this.lblModified_by.Name = "lblModified_by";
this.lblModified_by.Size = new System.Drawing.Size(41, 12);
this.lblModified_by.TabIndex = 16;
this.lblModified_by.Text = "修改人";
this.txtModified_by.Location = new System.Drawing.Point(173,396);
this.txtModified_by.Name = "txtModified_by";
this.txtModified_by.Size = new System.Drawing.Size(100, 21);
this.txtModified_by.TabIndex = 16;
this.Controls.Add(this.lblModified_by);
this.Controls.Add(this.txtModified_by);

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
           // this.kryptonPanel1.TabIndex = 16;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                tb__DepartmentID主外字段不一致。this.Controls.Add(this.lblPaymentID );
this.Controls.Add(this.cmbPaymentID );

                tb__DepartmentID主外字段不一致。this.Controls.Add(this.lblProjectGroup_ID );
this.Controls.Add(this.cmbProjectGroup_ID );

                tb__DepartmentID主外字段不一致。this.Controls.Add(this.lblDepartmentID );
this.Controls.Add(this.cmbDepartmentID );

                tb__DepartmentID主外字段不一致。this.Controls.Add(this.lbltb__DepartmentID );
this.Controls.Add(this.txttb__DepartmentID );

                tb__DepartmentID主外字段不一致。this.Controls.Add(this.lblSourceBill_BizType );
this.Controls.Add(this.txtSourceBill_BizType );

                tb__DepartmentID主外字段不一致。this.Controls.Add(this.lblSourceBill_ID );
this.Controls.Add(this.txtSourceBill_ID );

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

                tb__DepartmentID主外字段不一致。this.Controls.Add(this.lblCreated_by );
this.Controls.Add(this.txtCreated_by );

                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                tb__DepartmentID主外字段不一致。this.Controls.Add(this.lblModified_by );
this.Controls.Add(this.txtModified_by );

                            // 
            // "tb_FM_PaymentDetailEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_FM_PaymentDetailEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              tb__DepartmentID主外字段不一致。private Krypton.Toolkit.KryptonLabel lblPaymentID;
private Krypton.Toolkit.KryptonComboBox cmbPaymentID;

    
        
              tb__DepartmentID主外字段不一致。private Krypton.Toolkit.KryptonLabel lblProjectGroup_ID;
private Krypton.Toolkit.KryptonComboBox cmbProjectGroup_ID;

    
        
              tb__DepartmentID主外字段不一致。private Krypton.Toolkit.KryptonLabel lblDepartmentID;
private Krypton.Toolkit.KryptonComboBox cmbDepartmentID;
private Krypton.Toolkit.KryptonLabel lblDepartmentID;
private Krypton.Toolkit.KryptonComboBox cmbDepartmentID;

    
        
              tb__DepartmentID主外字段不一致。private Krypton.Toolkit.KryptonLabel lbltb__DepartmentID;
private Krypton.Toolkit.KryptonTextBox txttb__DepartmentID;

    
        
              tb__DepartmentID主外字段不一致。private Krypton.Toolkit.KryptonLabel lblSourceBill_BizType;
private Krypton.Toolkit.KryptonTextBox txtSourceBill_BizType;

    
        
              tb__DepartmentID主外字段不一致。private Krypton.Toolkit.KryptonLabel lblSourceBill_ID;
private Krypton.Toolkit.KryptonTextBox txtSourceBill_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblSourceBillNO;
private Krypton.Toolkit.KryptonTextBox txtSourceBillNO;

    
        
              private Krypton.Toolkit.KryptonLabel lblIsAdvancePayment;
private Krypton.Toolkit.KryptonCheckBox chkIsAdvancePayment;

    
        
              private Krypton.Toolkit.KryptonLabel lblPayReasonItems;
private Krypton.Toolkit.KryptonTextBox txtPayReasonItems;

    
        
              private Krypton.Toolkit.KryptonLabel lblSummary;
private Krypton.Toolkit.KryptonTextBox txtSummary;

    
        
              private Krypton.Toolkit.KryptonLabel lblSubAmount;
private Krypton.Toolkit.KryptonTextBox txtSubAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblSubPamountInWords;
private Krypton.Toolkit.KryptonTextBox txtSubPamountInWords;

    
        
              private Krypton.Toolkit.KryptonLabel lblCreated_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              tb__DepartmentID主外字段不一致。private Krypton.Toolkit.KryptonLabel lblCreated_by;
private Krypton.Toolkit.KryptonTextBox txtCreated_by;

    
        
              private Krypton.Toolkit.KryptonLabel lblModified_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              tb__DepartmentID主外字段不一致。private Krypton.Toolkit.KryptonLabel lblModified_by;
private Krypton.Toolkit.KryptonTextBox txtModified_by;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

