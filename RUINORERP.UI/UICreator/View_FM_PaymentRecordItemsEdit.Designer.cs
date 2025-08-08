// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:28
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 收付款单明细统计
    /// </summary>
    partial class View_FM_PaymentRecordItemsEdit
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
     this.lblPaymentId = new Krypton.Toolkit.KryptonLabel();
this.txtPaymentId = new Krypton.Toolkit.KryptonTextBox();

this.lblPaymentNo = new Krypton.Toolkit.KryptonLabel();
this.txtPaymentNo = new Krypton.Toolkit.KryptonTextBox();

this.lblSourceBillNo = new Krypton.Toolkit.KryptonLabel();
this.txtSourceBillNo = new Krypton.Toolkit.KryptonTextBox();

this.lblSourceBizType = new Krypton.Toolkit.KryptonLabel();
this.txtSourceBizType = new Krypton.Toolkit.KryptonTextBox();

this.lblReceivePaymentType = new Krypton.Toolkit.KryptonLabel();
this.txtReceivePaymentType = new Krypton.Toolkit.KryptonTextBox();

this.lblAccount_id = new Krypton.Toolkit.KryptonLabel();
this.txtAccount_id = new Krypton.Toolkit.KryptonTextBox();

this.lblCustomerVendor_ID = new Krypton.Toolkit.KryptonLabel();
this.txtCustomerVendor_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblPayeeInfoID = new Krypton.Toolkit.KryptonLabel();
this.txtPayeeInfoID = new Krypton.Toolkit.KryptonTextBox();

this.lblPayeeAccountNo = new Krypton.Toolkit.KryptonLabel();
this.txtPayeeAccountNo = new Krypton.Toolkit.KryptonTextBox();

this.lblCurrency_ID = new Krypton.Toolkit.KryptonLabel();
this.txtCurrency_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblTotalForeignAmount = new Krypton.Toolkit.KryptonLabel();
this.txtTotalForeignAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblTotalLocalAmount = new Krypton.Toolkit.KryptonLabel();
this.txtTotalLocalAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblPaymentDate = new Krypton.Toolkit.KryptonLabel();
this.dtpPaymentDate = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblEmployee_ID = new Krypton.Toolkit.KryptonLabel();
this.txtEmployee_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblPaytype_ID = new Krypton.Toolkit.KryptonLabel();
this.txtPaytype_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblPaymentStatus = new Krypton.Toolkit.KryptonLabel();
this.txtPaymentStatus = new Krypton.Toolkit.KryptonTextBox();

this.lblPaymentImagePath = new Krypton.Toolkit.KryptonLabel();
this.txtPaymentImagePath = new Krypton.Toolkit.KryptonTextBox();
this.txtPaymentImagePath.Multiline = true;

this.lblReferenceNo = new Krypton.Toolkit.KryptonLabel();
this.txtReferenceNo = new Krypton.Toolkit.KryptonTextBox();
this.txtReferenceNo.Multiline = true;

this.lblIsReversed = new Krypton.Toolkit.KryptonLabel();
this.chkIsReversed = new Krypton.Toolkit.KryptonCheckBox();
this.chkIsReversed.Values.Text ="";

this.lblReversedOriginalId = new Krypton.Toolkit.KryptonLabel();
this.txtReversedOriginalId = new Krypton.Toolkit.KryptonTextBox();

this.lblReversedOriginalNo = new Krypton.Toolkit.KryptonLabel();
this.txtReversedOriginalNo = new Krypton.Toolkit.KryptonTextBox();

this.lblReversedByPaymentId = new Krypton.Toolkit.KryptonLabel();
this.txtReversedByPaymentId = new Krypton.Toolkit.KryptonTextBox();

this.lblReversedByPaymentNo = new Krypton.Toolkit.KryptonLabel();
this.txtReversedByPaymentNo = new Krypton.Toolkit.KryptonTextBox();

this.lblPaymentDetailId = new Krypton.Toolkit.KryptonLabel();
this.txtPaymentDetailId = new Krypton.Toolkit.KryptonTextBox();

this.lblDepartmentID = new Krypton.Toolkit.KryptonLabel();
this.txtDepartmentID = new Krypton.Toolkit.KryptonTextBox();

this.lblProjectGroup_ID = new Krypton.Toolkit.KryptonLabel();
this.txtProjectGroup_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblForeignAmount = new Krypton.Toolkit.KryptonLabel();
this.txtForeignAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblLocalAmount = new Krypton.Toolkit.KryptonLabel();
this.txtLocalAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblSummary = new Krypton.Toolkit.KryptonLabel();
this.txtSummary = new Krypton.Toolkit.KryptonTextBox();
this.txtSummary.Multiline = true;

this.lblRemark = new Krypton.Toolkit.KryptonLabel();
this.txtRemark = new Krypton.Toolkit.KryptonTextBox();
this.txtRemark.Multiline = true;

this.lblCreated_at = new Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblCreated_by = new Krypton.Toolkit.KryptonLabel();
this.txtCreated_by = new Krypton.Toolkit.KryptonTextBox();

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
     
            //#####PaymentId###Int64
this.lblPaymentId.AutoSize = true;
this.lblPaymentId.Location = new System.Drawing.Point(100,25);
this.lblPaymentId.Name = "lblPaymentId";
this.lblPaymentId.Size = new System.Drawing.Size(41, 12);
this.lblPaymentId.TabIndex = 1;
this.lblPaymentId.Text = "";
this.txtPaymentId.Location = new System.Drawing.Point(173,21);
this.txtPaymentId.Name = "txtPaymentId";
this.txtPaymentId.Size = new System.Drawing.Size(100, 21);
this.txtPaymentId.TabIndex = 1;
this.Controls.Add(this.lblPaymentId);
this.Controls.Add(this.txtPaymentId);

           //#####30PaymentNo###String
this.lblPaymentNo.AutoSize = true;
this.lblPaymentNo.Location = new System.Drawing.Point(100,50);
this.lblPaymentNo.Name = "lblPaymentNo";
this.lblPaymentNo.Size = new System.Drawing.Size(41, 12);
this.lblPaymentNo.TabIndex = 2;
this.lblPaymentNo.Text = "";
this.txtPaymentNo.Location = new System.Drawing.Point(173,46);
this.txtPaymentNo.Name = "txtPaymentNo";
this.txtPaymentNo.Size = new System.Drawing.Size(100, 21);
this.txtPaymentNo.TabIndex = 2;
this.Controls.Add(this.lblPaymentNo);
this.Controls.Add(this.txtPaymentNo);

           //#####30SourceBillNo###String
this.lblSourceBillNo.AutoSize = true;
this.lblSourceBillNo.Location = new System.Drawing.Point(100,75);
this.lblSourceBillNo.Name = "lblSourceBillNo";
this.lblSourceBillNo.Size = new System.Drawing.Size(41, 12);
this.lblSourceBillNo.TabIndex = 3;
this.lblSourceBillNo.Text = "";
this.txtSourceBillNo.Location = new System.Drawing.Point(173,71);
this.txtSourceBillNo.Name = "txtSourceBillNo";
this.txtSourceBillNo.Size = new System.Drawing.Size(100, 21);
this.txtSourceBillNo.TabIndex = 3;
this.Controls.Add(this.lblSourceBillNo);
this.Controls.Add(this.txtSourceBillNo);

           //#####SourceBizType###Int32
this.lblSourceBizType.AutoSize = true;
this.lblSourceBizType.Location = new System.Drawing.Point(100,100);
this.lblSourceBizType.Name = "lblSourceBizType";
this.lblSourceBizType.Size = new System.Drawing.Size(41, 12);
this.lblSourceBizType.TabIndex = 4;
this.lblSourceBizType.Text = "";
this.txtSourceBizType.Location = new System.Drawing.Point(173,96);
this.txtSourceBizType.Name = "txtSourceBizType";
this.txtSourceBizType.Size = new System.Drawing.Size(100, 21);
this.txtSourceBizType.TabIndex = 4;
this.Controls.Add(this.lblSourceBizType);
this.Controls.Add(this.txtSourceBizType);

           //#####ReceivePaymentType###Int32
this.lblReceivePaymentType.AutoSize = true;
this.lblReceivePaymentType.Location = new System.Drawing.Point(100,125);
this.lblReceivePaymentType.Name = "lblReceivePaymentType";
this.lblReceivePaymentType.Size = new System.Drawing.Size(41, 12);
this.lblReceivePaymentType.TabIndex = 5;
this.lblReceivePaymentType.Text = "";
this.txtReceivePaymentType.Location = new System.Drawing.Point(173,121);
this.txtReceivePaymentType.Name = "txtReceivePaymentType";
this.txtReceivePaymentType.Size = new System.Drawing.Size(100, 21);
this.txtReceivePaymentType.TabIndex = 5;
this.Controls.Add(this.lblReceivePaymentType);
this.Controls.Add(this.txtReceivePaymentType);

           //#####Account_id###Int64
this.lblAccount_id.AutoSize = true;
this.lblAccount_id.Location = new System.Drawing.Point(100,150);
this.lblAccount_id.Name = "lblAccount_id";
this.lblAccount_id.Size = new System.Drawing.Size(41, 12);
this.lblAccount_id.TabIndex = 6;
this.lblAccount_id.Text = "";
this.txtAccount_id.Location = new System.Drawing.Point(173,146);
this.txtAccount_id.Name = "txtAccount_id";
this.txtAccount_id.Size = new System.Drawing.Size(100, 21);
this.txtAccount_id.TabIndex = 6;
this.Controls.Add(this.lblAccount_id);
this.Controls.Add(this.txtAccount_id);

           //#####CustomerVendor_ID###Int64
this.lblCustomerVendor_ID.AutoSize = true;
this.lblCustomerVendor_ID.Location = new System.Drawing.Point(100,175);
this.lblCustomerVendor_ID.Name = "lblCustomerVendor_ID";
this.lblCustomerVendor_ID.Size = new System.Drawing.Size(41, 12);
this.lblCustomerVendor_ID.TabIndex = 7;
this.lblCustomerVendor_ID.Text = "";
this.txtCustomerVendor_ID.Location = new System.Drawing.Point(173,171);
this.txtCustomerVendor_ID.Name = "txtCustomerVendor_ID";
this.txtCustomerVendor_ID.Size = new System.Drawing.Size(100, 21);
this.txtCustomerVendor_ID.TabIndex = 7;
this.Controls.Add(this.lblCustomerVendor_ID);
this.Controls.Add(this.txtCustomerVendor_ID);

           //#####PayeeInfoID###Int64
this.lblPayeeInfoID.AutoSize = true;
this.lblPayeeInfoID.Location = new System.Drawing.Point(100,200);
this.lblPayeeInfoID.Name = "lblPayeeInfoID";
this.lblPayeeInfoID.Size = new System.Drawing.Size(41, 12);
this.lblPayeeInfoID.TabIndex = 8;
this.lblPayeeInfoID.Text = "";
this.txtPayeeInfoID.Location = new System.Drawing.Point(173,196);
this.txtPayeeInfoID.Name = "txtPayeeInfoID";
this.txtPayeeInfoID.Size = new System.Drawing.Size(100, 21);
this.txtPayeeInfoID.TabIndex = 8;
this.Controls.Add(this.lblPayeeInfoID);
this.Controls.Add(this.txtPayeeInfoID);

           //#####100PayeeAccountNo###String
this.lblPayeeAccountNo.AutoSize = true;
this.lblPayeeAccountNo.Location = new System.Drawing.Point(100,225);
this.lblPayeeAccountNo.Name = "lblPayeeAccountNo";
this.lblPayeeAccountNo.Size = new System.Drawing.Size(41, 12);
this.lblPayeeAccountNo.TabIndex = 9;
this.lblPayeeAccountNo.Text = "";
this.txtPayeeAccountNo.Location = new System.Drawing.Point(173,221);
this.txtPayeeAccountNo.Name = "txtPayeeAccountNo";
this.txtPayeeAccountNo.Size = new System.Drawing.Size(100, 21);
this.txtPayeeAccountNo.TabIndex = 9;
this.Controls.Add(this.lblPayeeAccountNo);
this.Controls.Add(this.txtPayeeAccountNo);

           //#####Currency_ID###Int64
this.lblCurrency_ID.AutoSize = true;
this.lblCurrency_ID.Location = new System.Drawing.Point(100,250);
this.lblCurrency_ID.Name = "lblCurrency_ID";
this.lblCurrency_ID.Size = new System.Drawing.Size(41, 12);
this.lblCurrency_ID.TabIndex = 10;
this.lblCurrency_ID.Text = "";
this.txtCurrency_ID.Location = new System.Drawing.Point(173,246);
this.txtCurrency_ID.Name = "txtCurrency_ID";
this.txtCurrency_ID.Size = new System.Drawing.Size(100, 21);
this.txtCurrency_ID.TabIndex = 10;
this.Controls.Add(this.lblCurrency_ID);
this.Controls.Add(this.txtCurrency_ID);

           //#####TotalForeignAmount###Decimal
this.lblTotalForeignAmount.AutoSize = true;
this.lblTotalForeignAmount.Location = new System.Drawing.Point(100,275);
this.lblTotalForeignAmount.Name = "lblTotalForeignAmount";
this.lblTotalForeignAmount.Size = new System.Drawing.Size(41, 12);
this.lblTotalForeignAmount.TabIndex = 11;
this.lblTotalForeignAmount.Text = "";
//111======275
this.txtTotalForeignAmount.Location = new System.Drawing.Point(173,271);
this.txtTotalForeignAmount.Name ="txtTotalForeignAmount";
this.txtTotalForeignAmount.Size = new System.Drawing.Size(100, 21);
this.txtTotalForeignAmount.TabIndex = 11;
this.Controls.Add(this.lblTotalForeignAmount);
this.Controls.Add(this.txtTotalForeignAmount);

           //#####TotalLocalAmount###Decimal
this.lblTotalLocalAmount.AutoSize = true;
this.lblTotalLocalAmount.Location = new System.Drawing.Point(100,300);
this.lblTotalLocalAmount.Name = "lblTotalLocalAmount";
this.lblTotalLocalAmount.Size = new System.Drawing.Size(41, 12);
this.lblTotalLocalAmount.TabIndex = 12;
this.lblTotalLocalAmount.Text = "";
//111======300
this.txtTotalLocalAmount.Location = new System.Drawing.Point(173,296);
this.txtTotalLocalAmount.Name ="txtTotalLocalAmount";
this.txtTotalLocalAmount.Size = new System.Drawing.Size(100, 21);
this.txtTotalLocalAmount.TabIndex = 12;
this.Controls.Add(this.lblTotalLocalAmount);
this.Controls.Add(this.txtTotalLocalAmount);

           //#####PaymentDate###DateTime
this.lblPaymentDate.AutoSize = true;
this.lblPaymentDate.Location = new System.Drawing.Point(100,325);
this.lblPaymentDate.Name = "lblPaymentDate";
this.lblPaymentDate.Size = new System.Drawing.Size(41, 12);
this.lblPaymentDate.TabIndex = 13;
this.lblPaymentDate.Text = "";
//111======325
this.dtpPaymentDate.Location = new System.Drawing.Point(173,321);
this.dtpPaymentDate.Name ="dtpPaymentDate";
this.dtpPaymentDate.ShowCheckBox =true;
this.dtpPaymentDate.Size = new System.Drawing.Size(100, 21);
this.dtpPaymentDate.TabIndex = 13;
this.Controls.Add(this.lblPaymentDate);
this.Controls.Add(this.dtpPaymentDate);

           //#####Employee_ID###Int64
this.lblEmployee_ID.AutoSize = true;
this.lblEmployee_ID.Location = new System.Drawing.Point(100,350);
this.lblEmployee_ID.Name = "lblEmployee_ID";
this.lblEmployee_ID.Size = new System.Drawing.Size(41, 12);
this.lblEmployee_ID.TabIndex = 14;
this.lblEmployee_ID.Text = "";
this.txtEmployee_ID.Location = new System.Drawing.Point(173,346);
this.txtEmployee_ID.Name = "txtEmployee_ID";
this.txtEmployee_ID.Size = new System.Drawing.Size(100, 21);
this.txtEmployee_ID.TabIndex = 14;
this.Controls.Add(this.lblEmployee_ID);
this.Controls.Add(this.txtEmployee_ID);

           //#####Paytype_ID###Int64
this.lblPaytype_ID.AutoSize = true;
this.lblPaytype_ID.Location = new System.Drawing.Point(100,375);
this.lblPaytype_ID.Name = "lblPaytype_ID";
this.lblPaytype_ID.Size = new System.Drawing.Size(41, 12);
this.lblPaytype_ID.TabIndex = 15;
this.lblPaytype_ID.Text = "";
this.txtPaytype_ID.Location = new System.Drawing.Point(173,371);
this.txtPaytype_ID.Name = "txtPaytype_ID";
this.txtPaytype_ID.Size = new System.Drawing.Size(100, 21);
this.txtPaytype_ID.TabIndex = 15;
this.Controls.Add(this.lblPaytype_ID);
this.Controls.Add(this.txtPaytype_ID);

           //#####PaymentStatus###Int32
this.lblPaymentStatus.AutoSize = true;
this.lblPaymentStatus.Location = new System.Drawing.Point(100,400);
this.lblPaymentStatus.Name = "lblPaymentStatus";
this.lblPaymentStatus.Size = new System.Drawing.Size(41, 12);
this.lblPaymentStatus.TabIndex = 16;
this.lblPaymentStatus.Text = "";
this.txtPaymentStatus.Location = new System.Drawing.Point(173,396);
this.txtPaymentStatus.Name = "txtPaymentStatus";
this.txtPaymentStatus.Size = new System.Drawing.Size(100, 21);
this.txtPaymentStatus.TabIndex = 16;
this.Controls.Add(this.lblPaymentStatus);
this.Controls.Add(this.txtPaymentStatus);

           //#####300PaymentImagePath###String
this.lblPaymentImagePath.AutoSize = true;
this.lblPaymentImagePath.Location = new System.Drawing.Point(100,425);
this.lblPaymentImagePath.Name = "lblPaymentImagePath";
this.lblPaymentImagePath.Size = new System.Drawing.Size(41, 12);
this.lblPaymentImagePath.TabIndex = 17;
this.lblPaymentImagePath.Text = "";
this.txtPaymentImagePath.Location = new System.Drawing.Point(173,421);
this.txtPaymentImagePath.Name = "txtPaymentImagePath";
this.txtPaymentImagePath.Size = new System.Drawing.Size(100, 21);
this.txtPaymentImagePath.TabIndex = 17;
this.Controls.Add(this.lblPaymentImagePath);
this.Controls.Add(this.txtPaymentImagePath);

           //#####300ReferenceNo###String
this.lblReferenceNo.AutoSize = true;
this.lblReferenceNo.Location = new System.Drawing.Point(100,450);
this.lblReferenceNo.Name = "lblReferenceNo";
this.lblReferenceNo.Size = new System.Drawing.Size(41, 12);
this.lblReferenceNo.TabIndex = 18;
this.lblReferenceNo.Text = "";
this.txtReferenceNo.Location = new System.Drawing.Point(173,446);
this.txtReferenceNo.Name = "txtReferenceNo";
this.txtReferenceNo.Size = new System.Drawing.Size(100, 21);
this.txtReferenceNo.TabIndex = 18;
this.Controls.Add(this.lblReferenceNo);
this.Controls.Add(this.txtReferenceNo);

           //#####IsReversed###Boolean
this.lblIsReversed.AutoSize = true;
this.lblIsReversed.Location = new System.Drawing.Point(100,475);
this.lblIsReversed.Name = "lblIsReversed";
this.lblIsReversed.Size = new System.Drawing.Size(41, 12);
this.lblIsReversed.TabIndex = 19;
this.lblIsReversed.Text = "";
this.chkIsReversed.Location = new System.Drawing.Point(173,471);
this.chkIsReversed.Name = "chkIsReversed";
this.chkIsReversed.Size = new System.Drawing.Size(100, 21);
this.chkIsReversed.TabIndex = 19;
this.Controls.Add(this.lblIsReversed);
this.Controls.Add(this.chkIsReversed);

           //#####ReversedOriginalId###Int64
this.lblReversedOriginalId.AutoSize = true;
this.lblReversedOriginalId.Location = new System.Drawing.Point(100,500);
this.lblReversedOriginalId.Name = "lblReversedOriginalId";
this.lblReversedOriginalId.Size = new System.Drawing.Size(41, 12);
this.lblReversedOriginalId.TabIndex = 20;
this.lblReversedOriginalId.Text = "";
this.txtReversedOriginalId.Location = new System.Drawing.Point(173,496);
this.txtReversedOriginalId.Name = "txtReversedOriginalId";
this.txtReversedOriginalId.Size = new System.Drawing.Size(100, 21);
this.txtReversedOriginalId.TabIndex = 20;
this.Controls.Add(this.lblReversedOriginalId);
this.Controls.Add(this.txtReversedOriginalId);

           //#####30ReversedOriginalNo###String
this.lblReversedOriginalNo.AutoSize = true;
this.lblReversedOriginalNo.Location = new System.Drawing.Point(100,525);
this.lblReversedOriginalNo.Name = "lblReversedOriginalNo";
this.lblReversedOriginalNo.Size = new System.Drawing.Size(41, 12);
this.lblReversedOriginalNo.TabIndex = 21;
this.lblReversedOriginalNo.Text = "";
this.txtReversedOriginalNo.Location = new System.Drawing.Point(173,521);
this.txtReversedOriginalNo.Name = "txtReversedOriginalNo";
this.txtReversedOriginalNo.Size = new System.Drawing.Size(100, 21);
this.txtReversedOriginalNo.TabIndex = 21;
this.Controls.Add(this.lblReversedOriginalNo);
this.Controls.Add(this.txtReversedOriginalNo);

           //#####ReversedByPaymentId###Int64
this.lblReversedByPaymentId.AutoSize = true;
this.lblReversedByPaymentId.Location = new System.Drawing.Point(100,550);
this.lblReversedByPaymentId.Name = "lblReversedByPaymentId";
this.lblReversedByPaymentId.Size = new System.Drawing.Size(41, 12);
this.lblReversedByPaymentId.TabIndex = 22;
this.lblReversedByPaymentId.Text = "";
this.txtReversedByPaymentId.Location = new System.Drawing.Point(173,546);
this.txtReversedByPaymentId.Name = "txtReversedByPaymentId";
this.txtReversedByPaymentId.Size = new System.Drawing.Size(100, 21);
this.txtReversedByPaymentId.TabIndex = 22;
this.Controls.Add(this.lblReversedByPaymentId);
this.Controls.Add(this.txtReversedByPaymentId);

           //#####30ReversedByPaymentNo###String
this.lblReversedByPaymentNo.AutoSize = true;
this.lblReversedByPaymentNo.Location = new System.Drawing.Point(100,575);
this.lblReversedByPaymentNo.Name = "lblReversedByPaymentNo";
this.lblReversedByPaymentNo.Size = new System.Drawing.Size(41, 12);
this.lblReversedByPaymentNo.TabIndex = 23;
this.lblReversedByPaymentNo.Text = "";
this.txtReversedByPaymentNo.Location = new System.Drawing.Point(173,571);
this.txtReversedByPaymentNo.Name = "txtReversedByPaymentNo";
this.txtReversedByPaymentNo.Size = new System.Drawing.Size(100, 21);
this.txtReversedByPaymentNo.TabIndex = 23;
this.Controls.Add(this.lblReversedByPaymentNo);
this.Controls.Add(this.txtReversedByPaymentNo);

           //#####PaymentDetailId###Int64
this.lblPaymentDetailId.AutoSize = true;
this.lblPaymentDetailId.Location = new System.Drawing.Point(100,600);
this.lblPaymentDetailId.Name = "lblPaymentDetailId";
this.lblPaymentDetailId.Size = new System.Drawing.Size(41, 12);
this.lblPaymentDetailId.TabIndex = 24;
this.lblPaymentDetailId.Text = "";
this.txtPaymentDetailId.Location = new System.Drawing.Point(173,596);
this.txtPaymentDetailId.Name = "txtPaymentDetailId";
this.txtPaymentDetailId.Size = new System.Drawing.Size(100, 21);
this.txtPaymentDetailId.TabIndex = 24;
this.Controls.Add(this.lblPaymentDetailId);
this.Controls.Add(this.txtPaymentDetailId);

           //#####DepartmentID###Int64
this.lblDepartmentID.AutoSize = true;
this.lblDepartmentID.Location = new System.Drawing.Point(100,625);
this.lblDepartmentID.Name = "lblDepartmentID";
this.lblDepartmentID.Size = new System.Drawing.Size(41, 12);
this.lblDepartmentID.TabIndex = 25;
this.lblDepartmentID.Text = "";
this.txtDepartmentID.Location = new System.Drawing.Point(173,621);
this.txtDepartmentID.Name = "txtDepartmentID";
this.txtDepartmentID.Size = new System.Drawing.Size(100, 21);
this.txtDepartmentID.TabIndex = 25;
this.Controls.Add(this.lblDepartmentID);
this.Controls.Add(this.txtDepartmentID);

           //#####ProjectGroup_ID###Int64
this.lblProjectGroup_ID.AutoSize = true;
this.lblProjectGroup_ID.Location = new System.Drawing.Point(100,650);
this.lblProjectGroup_ID.Name = "lblProjectGroup_ID";
this.lblProjectGroup_ID.Size = new System.Drawing.Size(41, 12);
this.lblProjectGroup_ID.TabIndex = 26;
this.lblProjectGroup_ID.Text = "";
this.txtProjectGroup_ID.Location = new System.Drawing.Point(173,646);
this.txtProjectGroup_ID.Name = "txtProjectGroup_ID";
this.txtProjectGroup_ID.Size = new System.Drawing.Size(100, 21);
this.txtProjectGroup_ID.TabIndex = 26;
this.Controls.Add(this.lblProjectGroup_ID);
this.Controls.Add(this.txtProjectGroup_ID);

           //#####ForeignAmount###Decimal
this.lblForeignAmount.AutoSize = true;
this.lblForeignAmount.Location = new System.Drawing.Point(100,675);
this.lblForeignAmount.Name = "lblForeignAmount";
this.lblForeignAmount.Size = new System.Drawing.Size(41, 12);
this.lblForeignAmount.TabIndex = 27;
this.lblForeignAmount.Text = "";
//111======675
this.txtForeignAmount.Location = new System.Drawing.Point(173,671);
this.txtForeignAmount.Name ="txtForeignAmount";
this.txtForeignAmount.Size = new System.Drawing.Size(100, 21);
this.txtForeignAmount.TabIndex = 27;
this.Controls.Add(this.lblForeignAmount);
this.Controls.Add(this.txtForeignAmount);

           //#####LocalAmount###Decimal
this.lblLocalAmount.AutoSize = true;
this.lblLocalAmount.Location = new System.Drawing.Point(100,700);
this.lblLocalAmount.Name = "lblLocalAmount";
this.lblLocalAmount.Size = new System.Drawing.Size(41, 12);
this.lblLocalAmount.TabIndex = 28;
this.lblLocalAmount.Text = "";
//111======700
this.txtLocalAmount.Location = new System.Drawing.Point(173,696);
this.txtLocalAmount.Name ="txtLocalAmount";
this.txtLocalAmount.Size = new System.Drawing.Size(100, 21);
this.txtLocalAmount.TabIndex = 28;
this.Controls.Add(this.lblLocalAmount);
this.Controls.Add(this.txtLocalAmount);

           //#####300Summary###String
this.lblSummary.AutoSize = true;
this.lblSummary.Location = new System.Drawing.Point(100,725);
this.lblSummary.Name = "lblSummary";
this.lblSummary.Size = new System.Drawing.Size(41, 12);
this.lblSummary.TabIndex = 29;
this.lblSummary.Text = "";
this.txtSummary.Location = new System.Drawing.Point(173,721);
this.txtSummary.Name = "txtSummary";
this.txtSummary.Size = new System.Drawing.Size(100, 21);
this.txtSummary.TabIndex = 29;
this.Controls.Add(this.lblSummary);
this.Controls.Add(this.txtSummary);

           //#####300Remark###String
this.lblRemark.AutoSize = true;
this.lblRemark.Location = new System.Drawing.Point(100,750);
this.lblRemark.Name = "lblRemark";
this.lblRemark.Size = new System.Drawing.Size(41, 12);
this.lblRemark.TabIndex = 30;
this.lblRemark.Text = "";
this.txtRemark.Location = new System.Drawing.Point(173,746);
this.txtRemark.Name = "txtRemark";
this.txtRemark.Size = new System.Drawing.Size(100, 21);
this.txtRemark.TabIndex = 30;
this.Controls.Add(this.lblRemark);
this.Controls.Add(this.txtRemark);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,775);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 31;
this.lblCreated_at.Text = "";
//111======775
this.dtpCreated_at.Location = new System.Drawing.Point(173,771);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 31;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
this.lblCreated_by.AutoSize = true;
this.lblCreated_by.Location = new System.Drawing.Point(100,800);
this.lblCreated_by.Name = "lblCreated_by";
this.lblCreated_by.Size = new System.Drawing.Size(41, 12);
this.lblCreated_by.TabIndex = 32;
this.lblCreated_by.Text = "";
this.txtCreated_by.Location = new System.Drawing.Point(173,796);
this.txtCreated_by.Name = "txtCreated_by";
this.txtCreated_by.Size = new System.Drawing.Size(100, 21);
this.txtCreated_by.TabIndex = 32;
this.Controls.Add(this.lblCreated_by);
this.Controls.Add(this.txtCreated_by);

           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,825);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 33;
this.lblisdeleted.Text = "";
this.chkisdeleted.Location = new System.Drawing.Point(173,821);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 33;
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
           // this.kryptonPanel1.TabIndex = 33;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblPaymentId );
this.Controls.Add(this.txtPaymentId );

                this.Controls.Add(this.lblPaymentNo );
this.Controls.Add(this.txtPaymentNo );

                this.Controls.Add(this.lblSourceBillNo );
this.Controls.Add(this.txtSourceBillNo );

                this.Controls.Add(this.lblSourceBizType );
this.Controls.Add(this.txtSourceBizType );

                this.Controls.Add(this.lblReceivePaymentType );
this.Controls.Add(this.txtReceivePaymentType );

                this.Controls.Add(this.lblAccount_id );
this.Controls.Add(this.txtAccount_id );

                this.Controls.Add(this.lblCustomerVendor_ID );
this.Controls.Add(this.txtCustomerVendor_ID );

                this.Controls.Add(this.lblPayeeInfoID );
this.Controls.Add(this.txtPayeeInfoID );

                this.Controls.Add(this.lblPayeeAccountNo );
this.Controls.Add(this.txtPayeeAccountNo );

                this.Controls.Add(this.lblCurrency_ID );
this.Controls.Add(this.txtCurrency_ID );

                this.Controls.Add(this.lblTotalForeignAmount );
this.Controls.Add(this.txtTotalForeignAmount );

                this.Controls.Add(this.lblTotalLocalAmount );
this.Controls.Add(this.txtTotalLocalAmount );

                this.Controls.Add(this.lblPaymentDate );
this.Controls.Add(this.dtpPaymentDate );

                this.Controls.Add(this.lblEmployee_ID );
this.Controls.Add(this.txtEmployee_ID );

                this.Controls.Add(this.lblPaytype_ID );
this.Controls.Add(this.txtPaytype_ID );

                this.Controls.Add(this.lblPaymentStatus );
this.Controls.Add(this.txtPaymentStatus );

                this.Controls.Add(this.lblPaymentImagePath );
this.Controls.Add(this.txtPaymentImagePath );

                this.Controls.Add(this.lblReferenceNo );
this.Controls.Add(this.txtReferenceNo );

                this.Controls.Add(this.lblIsReversed );
this.Controls.Add(this.chkIsReversed );

                this.Controls.Add(this.lblReversedOriginalId );
this.Controls.Add(this.txtReversedOriginalId );

                this.Controls.Add(this.lblReversedOriginalNo );
this.Controls.Add(this.txtReversedOriginalNo );

                this.Controls.Add(this.lblReversedByPaymentId );
this.Controls.Add(this.txtReversedByPaymentId );

                this.Controls.Add(this.lblReversedByPaymentNo );
this.Controls.Add(this.txtReversedByPaymentNo );

                this.Controls.Add(this.lblPaymentDetailId );
this.Controls.Add(this.txtPaymentDetailId );

                this.Controls.Add(this.lblDepartmentID );
this.Controls.Add(this.txtDepartmentID );

                this.Controls.Add(this.lblProjectGroup_ID );
this.Controls.Add(this.txtProjectGroup_ID );

                this.Controls.Add(this.lblForeignAmount );
this.Controls.Add(this.txtForeignAmount );

                this.Controls.Add(this.lblLocalAmount );
this.Controls.Add(this.txtLocalAmount );

                this.Controls.Add(this.lblSummary );
this.Controls.Add(this.txtSummary );

                this.Controls.Add(this.lblRemark );
this.Controls.Add(this.txtRemark );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                this.Controls.Add(this.lblCreated_by );
this.Controls.Add(this.txtCreated_by );

                this.Controls.Add(this.lblisdeleted );
this.Controls.Add(this.chkisdeleted );

                            // 
            // "View_FM_PaymentRecordItemsEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "View_FM_PaymentRecordItemsEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblPaymentId;
private Krypton.Toolkit.KryptonTextBox txtPaymentId;

    
        
              private Krypton.Toolkit.KryptonLabel lblPaymentNo;
private Krypton.Toolkit.KryptonTextBox txtPaymentNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblSourceBillNo;
private Krypton.Toolkit.KryptonTextBox txtSourceBillNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblSourceBizType;
private Krypton.Toolkit.KryptonTextBox txtSourceBizType;

    
        
              private Krypton.Toolkit.KryptonLabel lblReceivePaymentType;
private Krypton.Toolkit.KryptonTextBox txtReceivePaymentType;

    
        
              private Krypton.Toolkit.KryptonLabel lblAccount_id;
private Krypton.Toolkit.KryptonTextBox txtAccount_id;

    
        
              private Krypton.Toolkit.KryptonLabel lblCustomerVendor_ID;
private Krypton.Toolkit.KryptonTextBox txtCustomerVendor_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblPayeeInfoID;
private Krypton.Toolkit.KryptonTextBox txtPayeeInfoID;

    
        
              private Krypton.Toolkit.KryptonLabel lblPayeeAccountNo;
private Krypton.Toolkit.KryptonTextBox txtPayeeAccountNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblCurrency_ID;
private Krypton.Toolkit.KryptonTextBox txtCurrency_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblTotalForeignAmount;
private Krypton.Toolkit.KryptonTextBox txtTotalForeignAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblTotalLocalAmount;
private Krypton.Toolkit.KryptonTextBox txtTotalLocalAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblPaymentDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpPaymentDate;

    
        
              private Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private Krypton.Toolkit.KryptonTextBox txtEmployee_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblPaytype_ID;
private Krypton.Toolkit.KryptonTextBox txtPaytype_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblPaymentStatus;
private Krypton.Toolkit.KryptonTextBox txtPaymentStatus;

    
        
              private Krypton.Toolkit.KryptonLabel lblPaymentImagePath;
private Krypton.Toolkit.KryptonTextBox txtPaymentImagePath;

    
        
              private Krypton.Toolkit.KryptonLabel lblReferenceNo;
private Krypton.Toolkit.KryptonTextBox txtReferenceNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblIsReversed;
private Krypton.Toolkit.KryptonCheckBox chkIsReversed;

    
        
              private Krypton.Toolkit.KryptonLabel lblReversedOriginalId;
private Krypton.Toolkit.KryptonTextBox txtReversedOriginalId;

    
        
              private Krypton.Toolkit.KryptonLabel lblReversedOriginalNo;
private Krypton.Toolkit.KryptonTextBox txtReversedOriginalNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblReversedByPaymentId;
private Krypton.Toolkit.KryptonTextBox txtReversedByPaymentId;

    
        
              private Krypton.Toolkit.KryptonLabel lblReversedByPaymentNo;
private Krypton.Toolkit.KryptonTextBox txtReversedByPaymentNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblPaymentDetailId;
private Krypton.Toolkit.KryptonTextBox txtPaymentDetailId;

    
        
              private Krypton.Toolkit.KryptonLabel lblDepartmentID;
private Krypton.Toolkit.KryptonTextBox txtDepartmentID;

    
        
              private Krypton.Toolkit.KryptonLabel lblProjectGroup_ID;
private Krypton.Toolkit.KryptonTextBox txtProjectGroup_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblForeignAmount;
private Krypton.Toolkit.KryptonTextBox txtForeignAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblLocalAmount;
private Krypton.Toolkit.KryptonTextBox txtLocalAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblSummary;
private Krypton.Toolkit.KryptonTextBox txtSummary;

    
        
              private Krypton.Toolkit.KryptonLabel lblRemark;
private Krypton.Toolkit.KryptonTextBox txtRemark;

    
        
              private Krypton.Toolkit.KryptonLabel lblCreated_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              private Krypton.Toolkit.KryptonLabel lblCreated_by;
private Krypton.Toolkit.KryptonTextBox txtCreated_by;

    
        
              private Krypton.Toolkit.KryptonLabel lblisdeleted;
private Krypton.Toolkit.KryptonCheckBox chkisdeleted;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

