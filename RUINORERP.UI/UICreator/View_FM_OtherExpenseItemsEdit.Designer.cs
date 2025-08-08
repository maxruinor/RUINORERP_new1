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
    /// 其他费用统计分析
    /// </summary>
    partial class View_FM_OtherExpenseItemsEdit
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
     this.lblExpenseNo = new Krypton.Toolkit.KryptonLabel();
this.txtExpenseNo = new Krypton.Toolkit.KryptonTextBox();

this.lblDocumentDate = new Krypton.Toolkit.KryptonLabel();
this.dtpDocumentDate = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblTotalAmount = new Krypton.Toolkit.KryptonLabel();
this.txtTotalAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblEXPOrINC = new Krypton.Toolkit.KryptonLabel();
this.chkEXPOrINC = new Krypton.Toolkit.KryptonCheckBox();
this.chkEXPOrINC.Values.Text ="";

this.lblNotes = new Krypton.Toolkit.KryptonLabel();
this.txtNotes = new Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

this.lblCreated_at = new Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblisdeleted = new Krypton.Toolkit.KryptonLabel();
this.chkisdeleted = new Krypton.Toolkit.KryptonCheckBox();
this.chkisdeleted.Values.Text ="";

this.lblDataStatus = new Krypton.Toolkit.KryptonLabel();
this.txtDataStatus = new Krypton.Toolkit.KryptonTextBox();


this.lblApprovalResults = new Krypton.Toolkit.KryptonLabel();
this.chkApprovalResults = new Krypton.Toolkit.KryptonCheckBox();
this.chkApprovalResults.Values.Text ="";

this.lblCurrency_ID = new Krypton.Toolkit.KryptonLabel();
this.txtCurrency_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblExpenseName = new Krypton.Toolkit.KryptonLabel();
this.txtExpenseName = new Krypton.Toolkit.KryptonTextBox();
this.txtExpenseName.Multiline = true;

this.lblEmployee_ID = new Krypton.Toolkit.KryptonLabel();
this.txtEmployee_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblDepartmentID = new Krypton.Toolkit.KryptonLabel();
this.txtDepartmentID = new Krypton.Toolkit.KryptonTextBox();

this.lblExpenseType_id = new Krypton.Toolkit.KryptonLabel();
this.txtExpenseType_id = new Krypton.Toolkit.KryptonTextBox();

this.lblAccount_id = new Krypton.Toolkit.KryptonLabel();
this.txtAccount_id = new Krypton.Toolkit.KryptonTextBox();

this.lblCustomerVendor_ID = new Krypton.Toolkit.KryptonLabel();
this.txtCustomerVendor_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblSingleTotalAmount = new Krypton.Toolkit.KryptonLabel();
this.txtSingleTotalAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblSubject_id = new Krypton.Toolkit.KryptonLabel();
this.txtSubject_id = new Krypton.Toolkit.KryptonTextBox();

this.lblCheckOutDate = new Krypton.Toolkit.KryptonLabel();
this.dtpCheckOutDate = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblIncludeTax = new Krypton.Toolkit.KryptonLabel();
this.chkIncludeTax = new Krypton.Toolkit.KryptonCheckBox();
this.chkIncludeTax.Values.Text ="";

this.lblSummary = new Krypton.Toolkit.KryptonLabel();
this.txtSummary = new Krypton.Toolkit.KryptonTextBox();

this.lblTaxAmount = new Krypton.Toolkit.KryptonLabel();
this.txtTaxAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblUntaxedAmount = new Krypton.Toolkit.KryptonLabel();
this.txtUntaxedAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblProjectGroup_ID = new Krypton.Toolkit.KryptonLabel();
this.txtProjectGroup_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblCreated_by = new Krypton.Toolkit.KryptonLabel();
this.txtCreated_by = new Krypton.Toolkit.KryptonTextBox();

    
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
     
            //#####30ExpenseNo###String
this.lblExpenseNo.AutoSize = true;
this.lblExpenseNo.Location = new System.Drawing.Point(100,25);
this.lblExpenseNo.Name = "lblExpenseNo";
this.lblExpenseNo.Size = new System.Drawing.Size(41, 12);
this.lblExpenseNo.TabIndex = 1;
this.lblExpenseNo.Text = "";
this.txtExpenseNo.Location = new System.Drawing.Point(173,21);
this.txtExpenseNo.Name = "txtExpenseNo";
this.txtExpenseNo.Size = new System.Drawing.Size(100, 21);
this.txtExpenseNo.TabIndex = 1;
this.Controls.Add(this.lblExpenseNo);
this.Controls.Add(this.txtExpenseNo);

           //#####DocumentDate###DateTime
this.lblDocumentDate.AutoSize = true;
this.lblDocumentDate.Location = new System.Drawing.Point(100,50);
this.lblDocumentDate.Name = "lblDocumentDate";
this.lblDocumentDate.Size = new System.Drawing.Size(41, 12);
this.lblDocumentDate.TabIndex = 2;
this.lblDocumentDate.Text = "";
//111======50
this.dtpDocumentDate.Location = new System.Drawing.Point(173,46);
this.dtpDocumentDate.Name ="dtpDocumentDate";
this.dtpDocumentDate.ShowCheckBox =true;
this.dtpDocumentDate.Size = new System.Drawing.Size(100, 21);
this.dtpDocumentDate.TabIndex = 2;
this.Controls.Add(this.lblDocumentDate);
this.Controls.Add(this.dtpDocumentDate);

           //#####TotalAmount###Decimal
this.lblTotalAmount.AutoSize = true;
this.lblTotalAmount.Location = new System.Drawing.Point(100,75);
this.lblTotalAmount.Name = "lblTotalAmount";
this.lblTotalAmount.Size = new System.Drawing.Size(41, 12);
this.lblTotalAmount.TabIndex = 3;
this.lblTotalAmount.Text = "";
//111======75
this.txtTotalAmount.Location = new System.Drawing.Point(173,71);
this.txtTotalAmount.Name ="txtTotalAmount";
this.txtTotalAmount.Size = new System.Drawing.Size(100, 21);
this.txtTotalAmount.TabIndex = 3;
this.Controls.Add(this.lblTotalAmount);
this.Controls.Add(this.txtTotalAmount);

           //#####EXPOrINC###Boolean
this.lblEXPOrINC.AutoSize = true;
this.lblEXPOrINC.Location = new System.Drawing.Point(100,100);
this.lblEXPOrINC.Name = "lblEXPOrINC";
this.lblEXPOrINC.Size = new System.Drawing.Size(41, 12);
this.lblEXPOrINC.TabIndex = 4;
this.lblEXPOrINC.Text = "";
this.chkEXPOrINC.Location = new System.Drawing.Point(173,96);
this.chkEXPOrINC.Name = "chkEXPOrINC";
this.chkEXPOrINC.Size = new System.Drawing.Size(100, 21);
this.chkEXPOrINC.TabIndex = 4;
this.Controls.Add(this.lblEXPOrINC);
this.Controls.Add(this.chkEXPOrINC);

           //#####1000Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,125);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 5;
this.lblNotes.Text = "";
this.txtNotes.Location = new System.Drawing.Point(173,121);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 5;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,150);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 6;
this.lblCreated_at.Text = "";
//111======150
this.dtpCreated_at.Location = new System.Drawing.Point(173,146);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 6;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,175);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 7;
this.lblisdeleted.Text = "";
this.chkisdeleted.Location = new System.Drawing.Point(173,171);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 7;
this.Controls.Add(this.lblisdeleted);
this.Controls.Add(this.chkisdeleted);

           //#####DataStatus###Int32
this.lblDataStatus.AutoSize = true;
this.lblDataStatus.Location = new System.Drawing.Point(100,200);
this.lblDataStatus.Name = "lblDataStatus";
this.lblDataStatus.Size = new System.Drawing.Size(41, 12);
this.lblDataStatus.TabIndex = 8;
this.lblDataStatus.Text = "";
this.txtDataStatus.Location = new System.Drawing.Point(173,196);
this.txtDataStatus.Name = "txtDataStatus";
this.txtDataStatus.Size = new System.Drawing.Size(100, 21);
this.txtDataStatus.TabIndex = 8;
this.Controls.Add(this.lblDataStatus);
this.Controls.Add(this.txtDataStatus);

           //#####ApprovalStatus###SByte

           //#####ApprovalResults###Boolean
this.lblApprovalResults.AutoSize = true;
this.lblApprovalResults.Location = new System.Drawing.Point(100,250);
this.lblApprovalResults.Name = "lblApprovalResults";
this.lblApprovalResults.Size = new System.Drawing.Size(41, 12);
this.lblApprovalResults.TabIndex = 10;
this.lblApprovalResults.Text = "";
this.chkApprovalResults.Location = new System.Drawing.Point(173,246);
this.chkApprovalResults.Name = "chkApprovalResults";
this.chkApprovalResults.Size = new System.Drawing.Size(100, 21);
this.chkApprovalResults.TabIndex = 10;
this.Controls.Add(this.lblApprovalResults);
this.Controls.Add(this.chkApprovalResults);

           //#####Currency_ID###Int64
this.lblCurrency_ID.AutoSize = true;
this.lblCurrency_ID.Location = new System.Drawing.Point(100,275);
this.lblCurrency_ID.Name = "lblCurrency_ID";
this.lblCurrency_ID.Size = new System.Drawing.Size(41, 12);
this.lblCurrency_ID.TabIndex = 11;
this.lblCurrency_ID.Text = "";
this.txtCurrency_ID.Location = new System.Drawing.Point(173,271);
this.txtCurrency_ID.Name = "txtCurrency_ID";
this.txtCurrency_ID.Size = new System.Drawing.Size(100, 21);
this.txtCurrency_ID.TabIndex = 11;
this.Controls.Add(this.lblCurrency_ID);
this.Controls.Add(this.txtCurrency_ID);

           //#####300ExpenseName###String
this.lblExpenseName.AutoSize = true;
this.lblExpenseName.Location = new System.Drawing.Point(100,300);
this.lblExpenseName.Name = "lblExpenseName";
this.lblExpenseName.Size = new System.Drawing.Size(41, 12);
this.lblExpenseName.TabIndex = 12;
this.lblExpenseName.Text = "";
this.txtExpenseName.Location = new System.Drawing.Point(173,296);
this.txtExpenseName.Name = "txtExpenseName";
this.txtExpenseName.Size = new System.Drawing.Size(100, 21);
this.txtExpenseName.TabIndex = 12;
this.Controls.Add(this.lblExpenseName);
this.Controls.Add(this.txtExpenseName);

           //#####Employee_ID###Int64
this.lblEmployee_ID.AutoSize = true;
this.lblEmployee_ID.Location = new System.Drawing.Point(100,325);
this.lblEmployee_ID.Name = "lblEmployee_ID";
this.lblEmployee_ID.Size = new System.Drawing.Size(41, 12);
this.lblEmployee_ID.TabIndex = 13;
this.lblEmployee_ID.Text = "";
this.txtEmployee_ID.Location = new System.Drawing.Point(173,321);
this.txtEmployee_ID.Name = "txtEmployee_ID";
this.txtEmployee_ID.Size = new System.Drawing.Size(100, 21);
this.txtEmployee_ID.TabIndex = 13;
this.Controls.Add(this.lblEmployee_ID);
this.Controls.Add(this.txtEmployee_ID);

           //#####DepartmentID###Int64
this.lblDepartmentID.AutoSize = true;
this.lblDepartmentID.Location = new System.Drawing.Point(100,350);
this.lblDepartmentID.Name = "lblDepartmentID";
this.lblDepartmentID.Size = new System.Drawing.Size(41, 12);
this.lblDepartmentID.TabIndex = 14;
this.lblDepartmentID.Text = "";
this.txtDepartmentID.Location = new System.Drawing.Point(173,346);
this.txtDepartmentID.Name = "txtDepartmentID";
this.txtDepartmentID.Size = new System.Drawing.Size(100, 21);
this.txtDepartmentID.TabIndex = 14;
this.Controls.Add(this.lblDepartmentID);
this.Controls.Add(this.txtDepartmentID);

           //#####ExpenseType_id###Int64
this.lblExpenseType_id.AutoSize = true;
this.lblExpenseType_id.Location = new System.Drawing.Point(100,375);
this.lblExpenseType_id.Name = "lblExpenseType_id";
this.lblExpenseType_id.Size = new System.Drawing.Size(41, 12);
this.lblExpenseType_id.TabIndex = 15;
this.lblExpenseType_id.Text = "";
this.txtExpenseType_id.Location = new System.Drawing.Point(173,371);
this.txtExpenseType_id.Name = "txtExpenseType_id";
this.txtExpenseType_id.Size = new System.Drawing.Size(100, 21);
this.txtExpenseType_id.TabIndex = 15;
this.Controls.Add(this.lblExpenseType_id);
this.Controls.Add(this.txtExpenseType_id);

           //#####Account_id###Int64
this.lblAccount_id.AutoSize = true;
this.lblAccount_id.Location = new System.Drawing.Point(100,400);
this.lblAccount_id.Name = "lblAccount_id";
this.lblAccount_id.Size = new System.Drawing.Size(41, 12);
this.lblAccount_id.TabIndex = 16;
this.lblAccount_id.Text = "";
this.txtAccount_id.Location = new System.Drawing.Point(173,396);
this.txtAccount_id.Name = "txtAccount_id";
this.txtAccount_id.Size = new System.Drawing.Size(100, 21);
this.txtAccount_id.TabIndex = 16;
this.Controls.Add(this.lblAccount_id);
this.Controls.Add(this.txtAccount_id);

           //#####CustomerVendor_ID###Int64
this.lblCustomerVendor_ID.AutoSize = true;
this.lblCustomerVendor_ID.Location = new System.Drawing.Point(100,425);
this.lblCustomerVendor_ID.Name = "lblCustomerVendor_ID";
this.lblCustomerVendor_ID.Size = new System.Drawing.Size(41, 12);
this.lblCustomerVendor_ID.TabIndex = 17;
this.lblCustomerVendor_ID.Text = "";
this.txtCustomerVendor_ID.Location = new System.Drawing.Point(173,421);
this.txtCustomerVendor_ID.Name = "txtCustomerVendor_ID";
this.txtCustomerVendor_ID.Size = new System.Drawing.Size(100, 21);
this.txtCustomerVendor_ID.TabIndex = 17;
this.Controls.Add(this.lblCustomerVendor_ID);
this.Controls.Add(this.txtCustomerVendor_ID);

           //#####SingleTotalAmount###Decimal
this.lblSingleTotalAmount.AutoSize = true;
this.lblSingleTotalAmount.Location = new System.Drawing.Point(100,450);
this.lblSingleTotalAmount.Name = "lblSingleTotalAmount";
this.lblSingleTotalAmount.Size = new System.Drawing.Size(41, 12);
this.lblSingleTotalAmount.TabIndex = 18;
this.lblSingleTotalAmount.Text = "";
//111======450
this.txtSingleTotalAmount.Location = new System.Drawing.Point(173,446);
this.txtSingleTotalAmount.Name ="txtSingleTotalAmount";
this.txtSingleTotalAmount.Size = new System.Drawing.Size(100, 21);
this.txtSingleTotalAmount.TabIndex = 18;
this.Controls.Add(this.lblSingleTotalAmount);
this.Controls.Add(this.txtSingleTotalAmount);

           //#####Subject_id###Int64
this.lblSubject_id.AutoSize = true;
this.lblSubject_id.Location = new System.Drawing.Point(100,475);
this.lblSubject_id.Name = "lblSubject_id";
this.lblSubject_id.Size = new System.Drawing.Size(41, 12);
this.lblSubject_id.TabIndex = 19;
this.lblSubject_id.Text = "";
this.txtSubject_id.Location = new System.Drawing.Point(173,471);
this.txtSubject_id.Name = "txtSubject_id";
this.txtSubject_id.Size = new System.Drawing.Size(100, 21);
this.txtSubject_id.TabIndex = 19;
this.Controls.Add(this.lblSubject_id);
this.Controls.Add(this.txtSubject_id);

           //#####CheckOutDate###DateTime
this.lblCheckOutDate.AutoSize = true;
this.lblCheckOutDate.Location = new System.Drawing.Point(100,500);
this.lblCheckOutDate.Name = "lblCheckOutDate";
this.lblCheckOutDate.Size = new System.Drawing.Size(41, 12);
this.lblCheckOutDate.TabIndex = 20;
this.lblCheckOutDate.Text = "";
//111======500
this.dtpCheckOutDate.Location = new System.Drawing.Point(173,496);
this.dtpCheckOutDate.Name ="dtpCheckOutDate";
this.dtpCheckOutDate.Size = new System.Drawing.Size(100, 21);
this.dtpCheckOutDate.TabIndex = 20;
this.Controls.Add(this.lblCheckOutDate);
this.Controls.Add(this.dtpCheckOutDate);

           //#####IncludeTax###Boolean
this.lblIncludeTax.AutoSize = true;
this.lblIncludeTax.Location = new System.Drawing.Point(100,525);
this.lblIncludeTax.Name = "lblIncludeTax";
this.lblIncludeTax.Size = new System.Drawing.Size(41, 12);
this.lblIncludeTax.TabIndex = 21;
this.lblIncludeTax.Text = "";
this.chkIncludeTax.Location = new System.Drawing.Point(173,521);
this.chkIncludeTax.Name = "chkIncludeTax";
this.chkIncludeTax.Size = new System.Drawing.Size(100, 21);
this.chkIncludeTax.TabIndex = 21;
this.Controls.Add(this.lblIncludeTax);
this.Controls.Add(this.chkIncludeTax);

           //#####100Summary###String
this.lblSummary.AutoSize = true;
this.lblSummary.Location = new System.Drawing.Point(100,550);
this.lblSummary.Name = "lblSummary";
this.lblSummary.Size = new System.Drawing.Size(41, 12);
this.lblSummary.TabIndex = 22;
this.lblSummary.Text = "";
this.txtSummary.Location = new System.Drawing.Point(173,546);
this.txtSummary.Name = "txtSummary";
this.txtSummary.Size = new System.Drawing.Size(100, 21);
this.txtSummary.TabIndex = 22;
this.Controls.Add(this.lblSummary);
this.Controls.Add(this.txtSummary);

           //#####TaxAmount###Decimal
this.lblTaxAmount.AutoSize = true;
this.lblTaxAmount.Location = new System.Drawing.Point(100,575);
this.lblTaxAmount.Name = "lblTaxAmount";
this.lblTaxAmount.Size = new System.Drawing.Size(41, 12);
this.lblTaxAmount.TabIndex = 23;
this.lblTaxAmount.Text = "";
//111======575
this.txtTaxAmount.Location = new System.Drawing.Point(173,571);
this.txtTaxAmount.Name ="txtTaxAmount";
this.txtTaxAmount.Size = new System.Drawing.Size(100, 21);
this.txtTaxAmount.TabIndex = 23;
this.Controls.Add(this.lblTaxAmount);
this.Controls.Add(this.txtTaxAmount);

           //#####UntaxedAmount###Decimal
this.lblUntaxedAmount.AutoSize = true;
this.lblUntaxedAmount.Location = new System.Drawing.Point(100,600);
this.lblUntaxedAmount.Name = "lblUntaxedAmount";
this.lblUntaxedAmount.Size = new System.Drawing.Size(41, 12);
this.lblUntaxedAmount.TabIndex = 24;
this.lblUntaxedAmount.Text = "";
//111======600
this.txtUntaxedAmount.Location = new System.Drawing.Point(173,596);
this.txtUntaxedAmount.Name ="txtUntaxedAmount";
this.txtUntaxedAmount.Size = new System.Drawing.Size(100, 21);
this.txtUntaxedAmount.TabIndex = 24;
this.Controls.Add(this.lblUntaxedAmount);
this.Controls.Add(this.txtUntaxedAmount);

           //#####ProjectGroup_ID###Int64
this.lblProjectGroup_ID.AutoSize = true;
this.lblProjectGroup_ID.Location = new System.Drawing.Point(100,625);
this.lblProjectGroup_ID.Name = "lblProjectGroup_ID";
this.lblProjectGroup_ID.Size = new System.Drawing.Size(41, 12);
this.lblProjectGroup_ID.TabIndex = 25;
this.lblProjectGroup_ID.Text = "";
this.txtProjectGroup_ID.Location = new System.Drawing.Point(173,621);
this.txtProjectGroup_ID.Name = "txtProjectGroup_ID";
this.txtProjectGroup_ID.Size = new System.Drawing.Size(100, 21);
this.txtProjectGroup_ID.TabIndex = 25;
this.Controls.Add(this.lblProjectGroup_ID);
this.Controls.Add(this.txtProjectGroup_ID);

           //#####Created_by###Int64
this.lblCreated_by.AutoSize = true;
this.lblCreated_by.Location = new System.Drawing.Point(100,650);
this.lblCreated_by.Name = "lblCreated_by";
this.lblCreated_by.Size = new System.Drawing.Size(41, 12);
this.lblCreated_by.TabIndex = 26;
this.lblCreated_by.Text = "";
this.txtCreated_by.Location = new System.Drawing.Point(173,646);
this.txtCreated_by.Name = "txtCreated_by";
this.txtCreated_by.Size = new System.Drawing.Size(100, 21);
this.txtCreated_by.TabIndex = 26;
this.Controls.Add(this.lblCreated_by);
this.Controls.Add(this.txtCreated_by);

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
           // this.kryptonPanel1.TabIndex = 26;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblExpenseNo );
this.Controls.Add(this.txtExpenseNo );

                this.Controls.Add(this.lblDocumentDate );
this.Controls.Add(this.dtpDocumentDate );

                this.Controls.Add(this.lblTotalAmount );
this.Controls.Add(this.txtTotalAmount );

                this.Controls.Add(this.lblEXPOrINC );
this.Controls.Add(this.chkEXPOrINC );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                this.Controls.Add(this.lblisdeleted );
this.Controls.Add(this.chkisdeleted );

                this.Controls.Add(this.lblDataStatus );
this.Controls.Add(this.txtDataStatus );

                
                this.Controls.Add(this.lblApprovalResults );
this.Controls.Add(this.chkApprovalResults );

                this.Controls.Add(this.lblCurrency_ID );
this.Controls.Add(this.txtCurrency_ID );

                this.Controls.Add(this.lblExpenseName );
this.Controls.Add(this.txtExpenseName );

                this.Controls.Add(this.lblEmployee_ID );
this.Controls.Add(this.txtEmployee_ID );

                this.Controls.Add(this.lblDepartmentID );
this.Controls.Add(this.txtDepartmentID );

                this.Controls.Add(this.lblExpenseType_id );
this.Controls.Add(this.txtExpenseType_id );

                this.Controls.Add(this.lblAccount_id );
this.Controls.Add(this.txtAccount_id );

                this.Controls.Add(this.lblCustomerVendor_ID );
this.Controls.Add(this.txtCustomerVendor_ID );

                this.Controls.Add(this.lblSingleTotalAmount );
this.Controls.Add(this.txtSingleTotalAmount );

                this.Controls.Add(this.lblSubject_id );
this.Controls.Add(this.txtSubject_id );

                this.Controls.Add(this.lblCheckOutDate );
this.Controls.Add(this.dtpCheckOutDate );

                this.Controls.Add(this.lblIncludeTax );
this.Controls.Add(this.chkIncludeTax );

                this.Controls.Add(this.lblSummary );
this.Controls.Add(this.txtSummary );

                this.Controls.Add(this.lblTaxAmount );
this.Controls.Add(this.txtTaxAmount );

                this.Controls.Add(this.lblUntaxedAmount );
this.Controls.Add(this.txtUntaxedAmount );

                this.Controls.Add(this.lblProjectGroup_ID );
this.Controls.Add(this.txtProjectGroup_ID );

                this.Controls.Add(this.lblCreated_by );
this.Controls.Add(this.txtCreated_by );

                            // 
            // "View_FM_OtherExpenseItemsEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "View_FM_OtherExpenseItemsEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblExpenseNo;
private Krypton.Toolkit.KryptonTextBox txtExpenseNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblDocumentDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpDocumentDate;

    
        
              private Krypton.Toolkit.KryptonLabel lblTotalAmount;
private Krypton.Toolkit.KryptonTextBox txtTotalAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblEXPOrINC;
private Krypton.Toolkit.KryptonCheckBox chkEXPOrINC;

    
        
              private Krypton.Toolkit.KryptonLabel lblNotes;
private Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private Krypton.Toolkit.KryptonLabel lblCreated_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              private Krypton.Toolkit.KryptonLabel lblisdeleted;
private Krypton.Toolkit.KryptonCheckBox chkisdeleted;

    
        
              private Krypton.Toolkit.KryptonLabel lblDataStatus;
private Krypton.Toolkit.KryptonTextBox txtDataStatus;

    
        
              
    
        
              private Krypton.Toolkit.KryptonLabel lblApprovalResults;
private Krypton.Toolkit.KryptonCheckBox chkApprovalResults;

    
        
              private Krypton.Toolkit.KryptonLabel lblCurrency_ID;
private Krypton.Toolkit.KryptonTextBox txtCurrency_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblExpenseName;
private Krypton.Toolkit.KryptonTextBox txtExpenseName;

    
        
              private Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private Krypton.Toolkit.KryptonTextBox txtEmployee_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblDepartmentID;
private Krypton.Toolkit.KryptonTextBox txtDepartmentID;

    
        
              private Krypton.Toolkit.KryptonLabel lblExpenseType_id;
private Krypton.Toolkit.KryptonTextBox txtExpenseType_id;

    
        
              private Krypton.Toolkit.KryptonLabel lblAccount_id;
private Krypton.Toolkit.KryptonTextBox txtAccount_id;

    
        
              private Krypton.Toolkit.KryptonLabel lblCustomerVendor_ID;
private Krypton.Toolkit.KryptonTextBox txtCustomerVendor_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblSingleTotalAmount;
private Krypton.Toolkit.KryptonTextBox txtSingleTotalAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblSubject_id;
private Krypton.Toolkit.KryptonTextBox txtSubject_id;

    
        
              private Krypton.Toolkit.KryptonLabel lblCheckOutDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpCheckOutDate;

    
        
              private Krypton.Toolkit.KryptonLabel lblIncludeTax;
private Krypton.Toolkit.KryptonCheckBox chkIncludeTax;

    
        
              private Krypton.Toolkit.KryptonLabel lblSummary;
private Krypton.Toolkit.KryptonTextBox txtSummary;

    
        
              private Krypton.Toolkit.KryptonLabel lblTaxAmount;
private Krypton.Toolkit.KryptonTextBox txtTaxAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblUntaxedAmount;
private Krypton.Toolkit.KryptonTextBox txtUntaxedAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblProjectGroup_ID;
private Krypton.Toolkit.KryptonTextBox txtProjectGroup_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblCreated_by;
private Krypton.Toolkit.KryptonTextBox txtCreated_by;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

