
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：09/04/2025 18:02:21
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 对账单
    /// </summary>
    partial class tb_FM_StatementQuery
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
     
     this.lblStatementNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtStatementNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCustomerVendor_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbCustomerVendor_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblARAPNos = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtARAPNos = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtARAPNos.Multiline = true;

this.lblAccount_id = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbAccount_id = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblPayeeInfoID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbPayeeInfoID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblPayeeAccountNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPayeeAccountNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblStartDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpStartDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblEndDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpEndDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblOpeningBalanceForeignAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtOpeningBalanceForeignAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblOpeningBalanceLocalAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtOpeningBalanceLocalAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTotalReceivableForeignAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalReceivableForeignAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTotalReceivableLocalAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalReceivableLocalAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTotalPayableForeignAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalPayableForeignAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTotalPayableLocalAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalPayableLocalAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTotalReceivedForeignAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalReceivedForeignAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTotalReceivedLocalAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalReceivedLocalAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTotalPaidForeignAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalPaidForeignAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTotalPaidLocalAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalPaidLocalAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblClosingBalanceForeignAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtClosingBalanceForeignAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblClosingBalanceLocalAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtClosingBalanceLocalAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();


this.lblCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblModified_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblisdeleted = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkisdeleted = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkisdeleted.Values.Text ="";

this.lblApprovalOpinions = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtApprovalOpinions = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtApprovalOpinions.Multiline = true;


this.lblApprover_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpApprover_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblApprovalResults = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkApprovalResults = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkApprovalResults.Values.Text ="";

this.lblSummary = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSummary = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtSummary.Multiline = true;


    //for end
    this.SuspendLayout();
    
         //for start
                 //#####30StatementNo###String
this.lblStatementNo.AutoSize = true;
this.lblStatementNo.Location = new System.Drawing.Point(100,25);
this.lblStatementNo.Name = "lblStatementNo";
this.lblStatementNo.Size = new System.Drawing.Size(41, 12);
this.lblStatementNo.TabIndex = 1;
this.lblStatementNo.Text = "对账单号";
this.txtStatementNo.Location = new System.Drawing.Point(173,21);
this.txtStatementNo.Name = "txtStatementNo";
this.txtStatementNo.Size = new System.Drawing.Size(100, 21);
this.txtStatementNo.TabIndex = 1;
this.Controls.Add(this.lblStatementNo);
this.Controls.Add(this.txtStatementNo);

           //#####CustomerVendor_ID###Int64
//属性测试50CustomerVendor_ID
this.lblCustomerVendor_ID.AutoSize = true;
this.lblCustomerVendor_ID.Location = new System.Drawing.Point(100,50);
this.lblCustomerVendor_ID.Name = "lblCustomerVendor_ID";
this.lblCustomerVendor_ID.Size = new System.Drawing.Size(41, 12);
this.lblCustomerVendor_ID.TabIndex = 2;
this.lblCustomerVendor_ID.Text = "往来单位";
//111======50
this.cmbCustomerVendor_ID.Location = new System.Drawing.Point(173,46);
this.cmbCustomerVendor_ID.Name ="cmbCustomerVendor_ID";
this.cmbCustomerVendor_ID.Size = new System.Drawing.Size(100, 21);
this.cmbCustomerVendor_ID.TabIndex = 2;
this.Controls.Add(this.lblCustomerVendor_ID);
this.Controls.Add(this.cmbCustomerVendor_ID);

           //#####1000ARAPNos###String
this.lblARAPNos.AutoSize = true;
this.lblARAPNos.Location = new System.Drawing.Point(100,75);
this.lblARAPNos.Name = "lblARAPNos";
this.lblARAPNos.Size = new System.Drawing.Size(41, 12);
this.lblARAPNos.TabIndex = 3;
this.lblARAPNos.Text = "应收付单号";
this.txtARAPNos.Location = new System.Drawing.Point(173,71);
this.txtARAPNos.Name = "txtARAPNos";
this.txtARAPNos.Size = new System.Drawing.Size(100, 21);
this.txtARAPNos.TabIndex = 3;
this.Controls.Add(this.lblARAPNos);
this.Controls.Add(this.txtARAPNos);

           //#####Account_id###Int64
//属性测试100Account_id
//属性测试100Account_id
this.lblAccount_id.AutoSize = true;
this.lblAccount_id.Location = new System.Drawing.Point(100,100);
this.lblAccount_id.Name = "lblAccount_id";
this.lblAccount_id.Size = new System.Drawing.Size(41, 12);
this.lblAccount_id.TabIndex = 4;
this.lblAccount_id.Text = "公司账户";
//111======100
this.cmbAccount_id.Location = new System.Drawing.Point(173,96);
this.cmbAccount_id.Name ="cmbAccount_id";
this.cmbAccount_id.Size = new System.Drawing.Size(100, 21);
this.cmbAccount_id.TabIndex = 4;
this.Controls.Add(this.lblAccount_id);
this.Controls.Add(this.cmbAccount_id);

           //#####PayeeInfoID###Int64
//属性测试125PayeeInfoID
//属性测试125PayeeInfoID
//属性测试125PayeeInfoID
this.lblPayeeInfoID.AutoSize = true;
this.lblPayeeInfoID.Location = new System.Drawing.Point(100,125);
this.lblPayeeInfoID.Name = "lblPayeeInfoID";
this.lblPayeeInfoID.Size = new System.Drawing.Size(41, 12);
this.lblPayeeInfoID.TabIndex = 5;
this.lblPayeeInfoID.Text = "收款信息";
//111======125
this.cmbPayeeInfoID.Location = new System.Drawing.Point(173,121);
this.cmbPayeeInfoID.Name ="cmbPayeeInfoID";
this.cmbPayeeInfoID.Size = new System.Drawing.Size(100, 21);
this.cmbPayeeInfoID.TabIndex = 5;
this.Controls.Add(this.lblPayeeInfoID);
this.Controls.Add(this.cmbPayeeInfoID);

           //#####100PayeeAccountNo###String
this.lblPayeeAccountNo.AutoSize = true;
this.lblPayeeAccountNo.Location = new System.Drawing.Point(100,150);
this.lblPayeeAccountNo.Name = "lblPayeeAccountNo";
this.lblPayeeAccountNo.Size = new System.Drawing.Size(41, 12);
this.lblPayeeAccountNo.TabIndex = 6;
this.lblPayeeAccountNo.Text = "收款账号";
this.txtPayeeAccountNo.Location = new System.Drawing.Point(173,146);
this.txtPayeeAccountNo.Name = "txtPayeeAccountNo";
this.txtPayeeAccountNo.Size = new System.Drawing.Size(100, 21);
this.txtPayeeAccountNo.TabIndex = 6;
this.Controls.Add(this.lblPayeeAccountNo);
this.Controls.Add(this.txtPayeeAccountNo);

           //#####ReceivePaymentType###Int32
//属性测试175ReceivePaymentType
//属性测试175ReceivePaymentType
//属性测试175ReceivePaymentType
//属性测试175ReceivePaymentType

           //#####StartDate###DateTime
this.lblStartDate.AutoSize = true;
this.lblStartDate.Location = new System.Drawing.Point(100,200);
this.lblStartDate.Name = "lblStartDate";
this.lblStartDate.Size = new System.Drawing.Size(41, 12);
this.lblStartDate.TabIndex = 8;
this.lblStartDate.Text = "对账周期起";
//111======200
this.dtpStartDate.Location = new System.Drawing.Point(173,196);
this.dtpStartDate.Name ="dtpStartDate";
this.dtpStartDate.ShowCheckBox =true;
this.dtpStartDate.Size = new System.Drawing.Size(100, 21);
this.dtpStartDate.TabIndex = 8;
this.Controls.Add(this.lblStartDate);
this.Controls.Add(this.dtpStartDate);

           //#####EndDate###DateTime
this.lblEndDate.AutoSize = true;
this.lblEndDate.Location = new System.Drawing.Point(100,225);
this.lblEndDate.Name = "lblEndDate";
this.lblEndDate.Size = new System.Drawing.Size(41, 12);
this.lblEndDate.TabIndex = 9;
this.lblEndDate.Text = "对账周期止";
//111======225
this.dtpEndDate.Location = new System.Drawing.Point(173,221);
this.dtpEndDate.Name ="dtpEndDate";
this.dtpEndDate.ShowCheckBox =true;
this.dtpEndDate.Size = new System.Drawing.Size(100, 21);
this.dtpEndDate.TabIndex = 9;
this.Controls.Add(this.lblEndDate);
this.Controls.Add(this.dtpEndDate);

           //#####OpeningBalanceForeignAmount###Decimal
this.lblOpeningBalanceForeignAmount.AutoSize = true;
this.lblOpeningBalanceForeignAmount.Location = new System.Drawing.Point(100,250);
this.lblOpeningBalanceForeignAmount.Name = "lblOpeningBalanceForeignAmount";
this.lblOpeningBalanceForeignAmount.Size = new System.Drawing.Size(41, 12);
this.lblOpeningBalanceForeignAmount.TabIndex = 10;
this.lblOpeningBalanceForeignAmount.Text = "期初余额外币";
//111======250
this.txtOpeningBalanceForeignAmount.Location = new System.Drawing.Point(173,246);
this.txtOpeningBalanceForeignAmount.Name ="txtOpeningBalanceForeignAmount";
this.txtOpeningBalanceForeignAmount.Size = new System.Drawing.Size(100, 21);
this.txtOpeningBalanceForeignAmount.TabIndex = 10;
this.Controls.Add(this.lblOpeningBalanceForeignAmount);
this.Controls.Add(this.txtOpeningBalanceForeignAmount);

           //#####OpeningBalanceLocalAmount###Decimal
this.lblOpeningBalanceLocalAmount.AutoSize = true;
this.lblOpeningBalanceLocalAmount.Location = new System.Drawing.Point(100,275);
this.lblOpeningBalanceLocalAmount.Name = "lblOpeningBalanceLocalAmount";
this.lblOpeningBalanceLocalAmount.Size = new System.Drawing.Size(41, 12);
this.lblOpeningBalanceLocalAmount.TabIndex = 11;
this.lblOpeningBalanceLocalAmount.Text = "期初余额本币";
//111======275
this.txtOpeningBalanceLocalAmount.Location = new System.Drawing.Point(173,271);
this.txtOpeningBalanceLocalAmount.Name ="txtOpeningBalanceLocalAmount";
this.txtOpeningBalanceLocalAmount.Size = new System.Drawing.Size(100, 21);
this.txtOpeningBalanceLocalAmount.TabIndex = 11;
this.Controls.Add(this.lblOpeningBalanceLocalAmount);
this.Controls.Add(this.txtOpeningBalanceLocalAmount);

           //#####TotalReceivableForeignAmount###Decimal
this.lblTotalReceivableForeignAmount.AutoSize = true;
this.lblTotalReceivableForeignAmount.Location = new System.Drawing.Point(100,300);
this.lblTotalReceivableForeignAmount.Name = "lblTotalReceivableForeignAmount";
this.lblTotalReceivableForeignAmount.Size = new System.Drawing.Size(41, 12);
this.lblTotalReceivableForeignAmount.TabIndex = 12;
this.lblTotalReceivableForeignAmount.Text = "期间应收外币";
//111======300
this.txtTotalReceivableForeignAmount.Location = new System.Drawing.Point(173,296);
this.txtTotalReceivableForeignAmount.Name ="txtTotalReceivableForeignAmount";
this.txtTotalReceivableForeignAmount.Size = new System.Drawing.Size(100, 21);
this.txtTotalReceivableForeignAmount.TabIndex = 12;
this.Controls.Add(this.lblTotalReceivableForeignAmount);
this.Controls.Add(this.txtTotalReceivableForeignAmount);

           //#####TotalReceivableLocalAmount###Decimal
this.lblTotalReceivableLocalAmount.AutoSize = true;
this.lblTotalReceivableLocalAmount.Location = new System.Drawing.Point(100,325);
this.lblTotalReceivableLocalAmount.Name = "lblTotalReceivableLocalAmount";
this.lblTotalReceivableLocalAmount.Size = new System.Drawing.Size(41, 12);
this.lblTotalReceivableLocalAmount.TabIndex = 13;
this.lblTotalReceivableLocalAmount.Text = "期间应收本币";
//111======325
this.txtTotalReceivableLocalAmount.Location = new System.Drawing.Point(173,321);
this.txtTotalReceivableLocalAmount.Name ="txtTotalReceivableLocalAmount";
this.txtTotalReceivableLocalAmount.Size = new System.Drawing.Size(100, 21);
this.txtTotalReceivableLocalAmount.TabIndex = 13;
this.Controls.Add(this.lblTotalReceivableLocalAmount);
this.Controls.Add(this.txtTotalReceivableLocalAmount);

           //#####TotalPayableForeignAmount###Decimal
this.lblTotalPayableForeignAmount.AutoSize = true;
this.lblTotalPayableForeignAmount.Location = new System.Drawing.Point(100,350);
this.lblTotalPayableForeignAmount.Name = "lblTotalPayableForeignAmount";
this.lblTotalPayableForeignAmount.Size = new System.Drawing.Size(41, 12);
this.lblTotalPayableForeignAmount.TabIndex = 14;
this.lblTotalPayableForeignAmount.Text = "期间应付外币";
//111======350
this.txtTotalPayableForeignAmount.Location = new System.Drawing.Point(173,346);
this.txtTotalPayableForeignAmount.Name ="txtTotalPayableForeignAmount";
this.txtTotalPayableForeignAmount.Size = new System.Drawing.Size(100, 21);
this.txtTotalPayableForeignAmount.TabIndex = 14;
this.Controls.Add(this.lblTotalPayableForeignAmount);
this.Controls.Add(this.txtTotalPayableForeignAmount);

           //#####TotalPayableLocalAmount###Decimal
this.lblTotalPayableLocalAmount.AutoSize = true;
this.lblTotalPayableLocalAmount.Location = new System.Drawing.Point(100,375);
this.lblTotalPayableLocalAmount.Name = "lblTotalPayableLocalAmount";
this.lblTotalPayableLocalAmount.Size = new System.Drawing.Size(41, 12);
this.lblTotalPayableLocalAmount.TabIndex = 15;
this.lblTotalPayableLocalAmount.Text = "期间应付本币";
//111======375
this.txtTotalPayableLocalAmount.Location = new System.Drawing.Point(173,371);
this.txtTotalPayableLocalAmount.Name ="txtTotalPayableLocalAmount";
this.txtTotalPayableLocalAmount.Size = new System.Drawing.Size(100, 21);
this.txtTotalPayableLocalAmount.TabIndex = 15;
this.Controls.Add(this.lblTotalPayableLocalAmount);
this.Controls.Add(this.txtTotalPayableLocalAmount);

           //#####TotalReceivedForeignAmount###Decimal
this.lblTotalReceivedForeignAmount.AutoSize = true;
this.lblTotalReceivedForeignAmount.Location = new System.Drawing.Point(100,400);
this.lblTotalReceivedForeignAmount.Name = "lblTotalReceivedForeignAmount";
this.lblTotalReceivedForeignAmount.Size = new System.Drawing.Size(41, 12);
this.lblTotalReceivedForeignAmount.TabIndex = 16;
this.lblTotalReceivedForeignAmount.Text = "期间收款外币";
//111======400
this.txtTotalReceivedForeignAmount.Location = new System.Drawing.Point(173,396);
this.txtTotalReceivedForeignAmount.Name ="txtTotalReceivedForeignAmount";
this.txtTotalReceivedForeignAmount.Size = new System.Drawing.Size(100, 21);
this.txtTotalReceivedForeignAmount.TabIndex = 16;
this.Controls.Add(this.lblTotalReceivedForeignAmount);
this.Controls.Add(this.txtTotalReceivedForeignAmount);

           //#####TotalReceivedLocalAmount###Decimal
this.lblTotalReceivedLocalAmount.AutoSize = true;
this.lblTotalReceivedLocalAmount.Location = new System.Drawing.Point(100,425);
this.lblTotalReceivedLocalAmount.Name = "lblTotalReceivedLocalAmount";
this.lblTotalReceivedLocalAmount.Size = new System.Drawing.Size(41, 12);
this.lblTotalReceivedLocalAmount.TabIndex = 17;
this.lblTotalReceivedLocalAmount.Text = "期间收款本币";
//111======425
this.txtTotalReceivedLocalAmount.Location = new System.Drawing.Point(173,421);
this.txtTotalReceivedLocalAmount.Name ="txtTotalReceivedLocalAmount";
this.txtTotalReceivedLocalAmount.Size = new System.Drawing.Size(100, 21);
this.txtTotalReceivedLocalAmount.TabIndex = 17;
this.Controls.Add(this.lblTotalReceivedLocalAmount);
this.Controls.Add(this.txtTotalReceivedLocalAmount);

           //#####TotalPaidForeignAmount###Decimal
this.lblTotalPaidForeignAmount.AutoSize = true;
this.lblTotalPaidForeignAmount.Location = new System.Drawing.Point(100,450);
this.lblTotalPaidForeignAmount.Name = "lblTotalPaidForeignAmount";
this.lblTotalPaidForeignAmount.Size = new System.Drawing.Size(41, 12);
this.lblTotalPaidForeignAmount.TabIndex = 18;
this.lblTotalPaidForeignAmount.Text = "期间付款外币";
//111======450
this.txtTotalPaidForeignAmount.Location = new System.Drawing.Point(173,446);
this.txtTotalPaidForeignAmount.Name ="txtTotalPaidForeignAmount";
this.txtTotalPaidForeignAmount.Size = new System.Drawing.Size(100, 21);
this.txtTotalPaidForeignAmount.TabIndex = 18;
this.Controls.Add(this.lblTotalPaidForeignAmount);
this.Controls.Add(this.txtTotalPaidForeignAmount);

           //#####TotalPaidLocalAmount###Decimal
this.lblTotalPaidLocalAmount.AutoSize = true;
this.lblTotalPaidLocalAmount.Location = new System.Drawing.Point(100,475);
this.lblTotalPaidLocalAmount.Name = "lblTotalPaidLocalAmount";
this.lblTotalPaidLocalAmount.Size = new System.Drawing.Size(41, 12);
this.lblTotalPaidLocalAmount.TabIndex = 19;
this.lblTotalPaidLocalAmount.Text = "期间付款本币";
//111======475
this.txtTotalPaidLocalAmount.Location = new System.Drawing.Point(173,471);
this.txtTotalPaidLocalAmount.Name ="txtTotalPaidLocalAmount";
this.txtTotalPaidLocalAmount.Size = new System.Drawing.Size(100, 21);
this.txtTotalPaidLocalAmount.TabIndex = 19;
this.Controls.Add(this.lblTotalPaidLocalAmount);
this.Controls.Add(this.txtTotalPaidLocalAmount);

           //#####ClosingBalanceForeignAmount###Decimal
this.lblClosingBalanceForeignAmount.AutoSize = true;
this.lblClosingBalanceForeignAmount.Location = new System.Drawing.Point(100,500);
this.lblClosingBalanceForeignAmount.Name = "lblClosingBalanceForeignAmount";
this.lblClosingBalanceForeignAmount.Size = new System.Drawing.Size(41, 12);
this.lblClosingBalanceForeignAmount.TabIndex = 20;
this.lblClosingBalanceForeignAmount.Text = "期末余额外币";
//111======500
this.txtClosingBalanceForeignAmount.Location = new System.Drawing.Point(173,496);
this.txtClosingBalanceForeignAmount.Name ="txtClosingBalanceForeignAmount";
this.txtClosingBalanceForeignAmount.Size = new System.Drawing.Size(100, 21);
this.txtClosingBalanceForeignAmount.TabIndex = 20;
this.Controls.Add(this.lblClosingBalanceForeignAmount);
this.Controls.Add(this.txtClosingBalanceForeignAmount);

           //#####ClosingBalanceLocalAmount###Decimal
this.lblClosingBalanceLocalAmount.AutoSize = true;
this.lblClosingBalanceLocalAmount.Location = new System.Drawing.Point(100,525);
this.lblClosingBalanceLocalAmount.Name = "lblClosingBalanceLocalAmount";
this.lblClosingBalanceLocalAmount.Size = new System.Drawing.Size(41, 12);
this.lblClosingBalanceLocalAmount.TabIndex = 21;
this.lblClosingBalanceLocalAmount.Text = "期末余额本币";
//111======525
this.txtClosingBalanceLocalAmount.Location = new System.Drawing.Point(173,521);
this.txtClosingBalanceLocalAmount.Name ="txtClosingBalanceLocalAmount";
this.txtClosingBalanceLocalAmount.Size = new System.Drawing.Size(100, 21);
this.txtClosingBalanceLocalAmount.TabIndex = 21;
this.Controls.Add(this.lblClosingBalanceLocalAmount);
this.Controls.Add(this.txtClosingBalanceLocalAmount);

           //#####Employee_ID###Int64
//属性测试550Employee_ID
//属性测试550Employee_ID
//属性测试550Employee_ID
//属性测试550Employee_ID
this.lblEmployee_ID.AutoSize = true;
this.lblEmployee_ID.Location = new System.Drawing.Point(100,550);
this.lblEmployee_ID.Name = "lblEmployee_ID";
this.lblEmployee_ID.Size = new System.Drawing.Size(41, 12);
this.lblEmployee_ID.TabIndex = 22;
this.lblEmployee_ID.Text = "经办人";
//111======550
this.cmbEmployee_ID.Location = new System.Drawing.Point(173,546);
this.cmbEmployee_ID.Name ="cmbEmployee_ID";
this.cmbEmployee_ID.Size = new System.Drawing.Size(100, 21);
this.cmbEmployee_ID.TabIndex = 22;
this.Controls.Add(this.lblEmployee_ID);
this.Controls.Add(this.cmbEmployee_ID);

           //#####StatementStatus###Int32
//属性测试575StatementStatus
//属性测试575StatementStatus
//属性测试575StatementStatus
//属性测试575StatementStatus

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,600);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 24;
this.lblCreated_at.Text = "创建时间";
//111======600
this.dtpCreated_at.Location = new System.Drawing.Point(173,596);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 24;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
//属性测试625Created_by
//属性测试625Created_by
//属性测试625Created_by
//属性测试625Created_by

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,650);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 26;
this.lblModified_at.Text = "修改时间";
//111======650
this.dtpModified_at.Location = new System.Drawing.Point(173,646);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 26;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
//属性测试675Modified_by
//属性测试675Modified_by
//属性测试675Modified_by
//属性测试675Modified_by

           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,700);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 28;
this.lblisdeleted.Text = "逻辑删除";
this.chkisdeleted.Location = new System.Drawing.Point(173,696);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 28;
this.Controls.Add(this.lblisdeleted);
this.Controls.Add(this.chkisdeleted);

           //#####255ApprovalOpinions###String
this.lblApprovalOpinions.AutoSize = true;
this.lblApprovalOpinions.Location = new System.Drawing.Point(100,725);
this.lblApprovalOpinions.Name = "lblApprovalOpinions";
this.lblApprovalOpinions.Size = new System.Drawing.Size(41, 12);
this.lblApprovalOpinions.TabIndex = 29;
this.lblApprovalOpinions.Text = "审批意见";
this.txtApprovalOpinions.Location = new System.Drawing.Point(173,721);
this.txtApprovalOpinions.Name = "txtApprovalOpinions";
this.txtApprovalOpinions.Size = new System.Drawing.Size(100, 21);
this.txtApprovalOpinions.TabIndex = 29;
this.Controls.Add(this.lblApprovalOpinions);
this.Controls.Add(this.txtApprovalOpinions);

           //#####Approver_by###Int64
//属性测试750Approver_by
//属性测试750Approver_by
//属性测试750Approver_by
//属性测试750Approver_by

           //#####Approver_at###DateTime
this.lblApprover_at.AutoSize = true;
this.lblApprover_at.Location = new System.Drawing.Point(100,775);
this.lblApprover_at.Name = "lblApprover_at";
this.lblApprover_at.Size = new System.Drawing.Size(41, 12);
this.lblApprover_at.TabIndex = 31;
this.lblApprover_at.Text = "审批时间";
//111======775
this.dtpApprover_at.Location = new System.Drawing.Point(173,771);
this.dtpApprover_at.Name ="dtpApprover_at";
this.dtpApprover_at.ShowCheckBox =true;
this.dtpApprover_at.Size = new System.Drawing.Size(100, 21);
this.dtpApprover_at.TabIndex = 31;
this.Controls.Add(this.lblApprover_at);
this.Controls.Add(this.dtpApprover_at);

           //#####ApprovalStatus###SByte

           //#####ApprovalResults###Boolean
this.lblApprovalResults.AutoSize = true;
this.lblApprovalResults.Location = new System.Drawing.Point(100,825);
this.lblApprovalResults.Name = "lblApprovalResults";
this.lblApprovalResults.Size = new System.Drawing.Size(41, 12);
this.lblApprovalResults.TabIndex = 33;
this.lblApprovalResults.Text = "审批结果";
this.chkApprovalResults.Location = new System.Drawing.Point(173,821);
this.chkApprovalResults.Name = "chkApprovalResults";
this.chkApprovalResults.Size = new System.Drawing.Size(100, 21);
this.chkApprovalResults.TabIndex = 33;
this.Controls.Add(this.lblApprovalResults);
this.Controls.Add(this.chkApprovalResults);

           //#####500Summary###String
this.lblSummary.AutoSize = true;
this.lblSummary.Location = new System.Drawing.Point(100,850);
this.lblSummary.Name = "lblSummary";
this.lblSummary.Size = new System.Drawing.Size(41, 12);
this.lblSummary.TabIndex = 34;
this.lblSummary.Text = "备注";
this.txtSummary.Location = new System.Drawing.Point(173,846);
this.txtSummary.Name = "txtSummary";
this.txtSummary.Size = new System.Drawing.Size(100, 21);
this.txtSummary.TabIndex = 34;
this.Controls.Add(this.lblSummary);
this.Controls.Add(this.txtSummary);

           //#####PrintStatus###Int32
//属性测试875PrintStatus
//属性测试875PrintStatus
//属性测试875PrintStatus
//属性测试875PrintStatus

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblStatementNo );
this.Controls.Add(this.txtStatementNo );

                this.Controls.Add(this.lblCustomerVendor_ID );
this.Controls.Add(this.cmbCustomerVendor_ID );

                this.Controls.Add(this.lblARAPNos );
this.Controls.Add(this.txtARAPNos );

                this.Controls.Add(this.lblAccount_id );
this.Controls.Add(this.cmbAccount_id );

                this.Controls.Add(this.lblPayeeInfoID );
this.Controls.Add(this.cmbPayeeInfoID );

                this.Controls.Add(this.lblPayeeAccountNo );
this.Controls.Add(this.txtPayeeAccountNo );

                
                this.Controls.Add(this.lblStartDate );
this.Controls.Add(this.dtpStartDate );

                this.Controls.Add(this.lblEndDate );
this.Controls.Add(this.dtpEndDate );

                this.Controls.Add(this.lblOpeningBalanceForeignAmount );
this.Controls.Add(this.txtOpeningBalanceForeignAmount );

                this.Controls.Add(this.lblOpeningBalanceLocalAmount );
this.Controls.Add(this.txtOpeningBalanceLocalAmount );

                this.Controls.Add(this.lblTotalReceivableForeignAmount );
this.Controls.Add(this.txtTotalReceivableForeignAmount );

                this.Controls.Add(this.lblTotalReceivableLocalAmount );
this.Controls.Add(this.txtTotalReceivableLocalAmount );

                this.Controls.Add(this.lblTotalPayableForeignAmount );
this.Controls.Add(this.txtTotalPayableForeignAmount );

                this.Controls.Add(this.lblTotalPayableLocalAmount );
this.Controls.Add(this.txtTotalPayableLocalAmount );

                this.Controls.Add(this.lblTotalReceivedForeignAmount );
this.Controls.Add(this.txtTotalReceivedForeignAmount );

                this.Controls.Add(this.lblTotalReceivedLocalAmount );
this.Controls.Add(this.txtTotalReceivedLocalAmount );

                this.Controls.Add(this.lblTotalPaidForeignAmount );
this.Controls.Add(this.txtTotalPaidForeignAmount );

                this.Controls.Add(this.lblTotalPaidLocalAmount );
this.Controls.Add(this.txtTotalPaidLocalAmount );

                this.Controls.Add(this.lblClosingBalanceForeignAmount );
this.Controls.Add(this.txtClosingBalanceForeignAmount );

                this.Controls.Add(this.lblClosingBalanceLocalAmount );
this.Controls.Add(this.txtClosingBalanceLocalAmount );

                this.Controls.Add(this.lblEmployee_ID );
this.Controls.Add(this.cmbEmployee_ID );

                
                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                
                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                
                this.Controls.Add(this.lblisdeleted );
this.Controls.Add(this.chkisdeleted );

                this.Controls.Add(this.lblApprovalOpinions );
this.Controls.Add(this.txtApprovalOpinions );

                
                this.Controls.Add(this.lblApprover_at );
this.Controls.Add(this.dtpApprover_at );

                
                this.Controls.Add(this.lblApprovalResults );
this.Controls.Add(this.chkApprovalResults );

                this.Controls.Add(this.lblSummary );
this.Controls.Add(this.txtSummary );

                
                    
            this.Name = "tb_FM_StatementQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblStatementNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtStatementNo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCustomerVendor_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbCustomerVendor_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblARAPNos;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtARAPNos;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblAccount_id;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbAccount_id;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPayeeInfoID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbPayeeInfoID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPayeeAccountNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPayeeAccountNo;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblStartDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpStartDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblEndDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpEndDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblOpeningBalanceForeignAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtOpeningBalanceForeignAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblOpeningBalanceLocalAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtOpeningBalanceLocalAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalReceivableForeignAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalReceivableForeignAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalReceivableLocalAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalReceivableLocalAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalPayableForeignAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalPayableForeignAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalPayableLocalAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalPayableLocalAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalReceivedForeignAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalReceivedForeignAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalReceivedLocalAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalReceivedLocalAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalPaidForeignAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalPaidForeignAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalPaidLocalAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalPaidLocalAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblClosingBalanceForeignAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtClosingBalanceForeignAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblClosingBalanceLocalAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtClosingBalanceLocalAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModified_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblisdeleted;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkisdeleted;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprovalOpinions;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtApprovalOpinions;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprover_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpApprover_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprovalResults;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkApprovalResults;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSummary;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSummary;

    
        
              
    
    
   
 





    }
}


