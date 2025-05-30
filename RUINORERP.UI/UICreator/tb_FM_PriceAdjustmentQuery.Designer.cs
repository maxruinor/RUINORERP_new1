﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：05/24/2025 18:28:26
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 价格调整单
    /// </summary>
    partial class tb_FM_PriceAdjustmentQuery
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
     
     this.lblAdjustNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtAdjustNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCustomerVendor_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbCustomerVendor_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();




this.lblSourceBillNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSourceBillNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCurrency_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbCurrency_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblAdjustReason = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtAdjustReason = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtAdjustReason.Multiline = true;

this.lblExchangeRate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtExchangeRate = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblAdjustDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpAdjustDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblDepartmentID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbDepartmentID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblProjectGroup_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbProjectGroup_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();


this.lblInvoiced = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkInvoiced = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkInvoiced.Values.Text ="";

this.lblTotalForeignDiffAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalForeignDiffAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTotalLocalDiffAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalLocalDiffAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblIsIncludeTax = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIsIncludeTax = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIsIncludeTax.Values.Text ="";

this.lblTaxTotalDiffLocalAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTaxTotalDiffLocalAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblRemark = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtRemark = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtRemark.Multiline = true;

this.lblApprovalOpinions = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtApprovalOpinions = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtApprovalOpinions.Multiline = true;


this.lblApprover_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpApprover_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblApprovalResults = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkApprovalResults = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkApprovalResults.Values.Text ="";


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
                 //#####30AdjustNo###String
this.lblAdjustNo.AutoSize = true;
this.lblAdjustNo.Location = new System.Drawing.Point(100,25);
this.lblAdjustNo.Name = "lblAdjustNo";
this.lblAdjustNo.Size = new System.Drawing.Size(41, 12);
this.lblAdjustNo.TabIndex = 1;
this.lblAdjustNo.Text = "调整编号";
this.txtAdjustNo.Location = new System.Drawing.Point(173,21);
this.txtAdjustNo.Name = "txtAdjustNo";
this.txtAdjustNo.Size = new System.Drawing.Size(100, 21);
this.txtAdjustNo.TabIndex = 1;
this.Controls.Add(this.lblAdjustNo);
this.Controls.Add(this.txtAdjustNo);

           //#####CustomerVendor_ID###Int64
//属性测试50CustomerVendor_ID
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

           //#####ReceivePaymentType###Int32
//属性测试75ReceivePaymentType
//属性测试75ReceivePaymentType
//属性测试75ReceivePaymentType
//属性测试75ReceivePaymentType
//属性测试75ReceivePaymentType

           //#####SourceBizType###Int32
//属性测试100SourceBizType
//属性测试100SourceBizType
//属性测试100SourceBizType
//属性测试100SourceBizType
//属性测试100SourceBizType

           //#####SourceBillId###Int64
//属性测试125SourceBillId
//属性测试125SourceBillId
//属性测试125SourceBillId
//属性测试125SourceBillId
//属性测试125SourceBillId

           //#####30SourceBillNo###String
this.lblSourceBillNo.AutoSize = true;
this.lblSourceBillNo.Location = new System.Drawing.Point(100,150);
this.lblSourceBillNo.Name = "lblSourceBillNo";
this.lblSourceBillNo.Size = new System.Drawing.Size(41, 12);
this.lblSourceBillNo.TabIndex = 6;
this.lblSourceBillNo.Text = "来源单号";
this.txtSourceBillNo.Location = new System.Drawing.Point(173,146);
this.txtSourceBillNo.Name = "txtSourceBillNo";
this.txtSourceBillNo.Size = new System.Drawing.Size(100, 21);
this.txtSourceBillNo.TabIndex = 6;
this.Controls.Add(this.lblSourceBillNo);
this.Controls.Add(this.txtSourceBillNo);

           //#####Currency_ID###Int64
//属性测试175Currency_ID
this.lblCurrency_ID.AutoSize = true;
this.lblCurrency_ID.Location = new System.Drawing.Point(100,175);
this.lblCurrency_ID.Name = "lblCurrency_ID";
this.lblCurrency_ID.Size = new System.Drawing.Size(41, 12);
this.lblCurrency_ID.TabIndex = 7;
this.lblCurrency_ID.Text = "币别";
//111======175
this.cmbCurrency_ID.Location = new System.Drawing.Point(173,171);
this.cmbCurrency_ID.Name ="cmbCurrency_ID";
this.cmbCurrency_ID.Size = new System.Drawing.Size(100, 21);
this.cmbCurrency_ID.TabIndex = 7;
this.Controls.Add(this.lblCurrency_ID);
this.Controls.Add(this.cmbCurrency_ID);

           //#####400AdjustReason###String
this.lblAdjustReason.AutoSize = true;
this.lblAdjustReason.Location = new System.Drawing.Point(100,200);
this.lblAdjustReason.Name = "lblAdjustReason";
this.lblAdjustReason.Size = new System.Drawing.Size(41, 12);
this.lblAdjustReason.TabIndex = 8;
this.lblAdjustReason.Text = "调整原因";
this.txtAdjustReason.Location = new System.Drawing.Point(173,196);
this.txtAdjustReason.Name = "txtAdjustReason";
this.txtAdjustReason.Size = new System.Drawing.Size(100, 21);
this.txtAdjustReason.TabIndex = 8;
this.Controls.Add(this.lblAdjustReason);
this.Controls.Add(this.txtAdjustReason);

           //#####ExchangeRate###Decimal
this.lblExchangeRate.AutoSize = true;
this.lblExchangeRate.Location = new System.Drawing.Point(100,225);
this.lblExchangeRate.Name = "lblExchangeRate";
this.lblExchangeRate.Size = new System.Drawing.Size(41, 12);
this.lblExchangeRate.TabIndex = 9;
this.lblExchangeRate.Text = "汇率";
//111======225
this.txtExchangeRate.Location = new System.Drawing.Point(173,221);
this.txtExchangeRate.Name ="txtExchangeRate";
this.txtExchangeRate.Size = new System.Drawing.Size(100, 21);
this.txtExchangeRate.TabIndex = 9;
this.Controls.Add(this.lblExchangeRate);
this.Controls.Add(this.txtExchangeRate);

           //#####AdjustDate###DateTime
this.lblAdjustDate.AutoSize = true;
this.lblAdjustDate.Location = new System.Drawing.Point(100,250);
this.lblAdjustDate.Name = "lblAdjustDate";
this.lblAdjustDate.Size = new System.Drawing.Size(41, 12);
this.lblAdjustDate.TabIndex = 10;
this.lblAdjustDate.Text = "调整日期";
//111======250
this.dtpAdjustDate.Location = new System.Drawing.Point(173,246);
this.dtpAdjustDate.Name ="dtpAdjustDate";
this.dtpAdjustDate.Size = new System.Drawing.Size(100, 21);
this.dtpAdjustDate.TabIndex = 10;
this.Controls.Add(this.lblAdjustDate);
this.Controls.Add(this.dtpAdjustDate);

           //#####DepartmentID###Int64
//属性测试275DepartmentID
//属性测试275DepartmentID
//属性测试275DepartmentID
this.lblDepartmentID.AutoSize = true;
this.lblDepartmentID.Location = new System.Drawing.Point(100,275);
this.lblDepartmentID.Name = "lblDepartmentID";
this.lblDepartmentID.Size = new System.Drawing.Size(41, 12);
this.lblDepartmentID.TabIndex = 11;
this.lblDepartmentID.Text = "部门";
//111======275
this.cmbDepartmentID.Location = new System.Drawing.Point(173,271);
this.cmbDepartmentID.Name ="cmbDepartmentID";
this.cmbDepartmentID.Size = new System.Drawing.Size(100, 21);
this.cmbDepartmentID.TabIndex = 11;
this.Controls.Add(this.lblDepartmentID);
this.Controls.Add(this.cmbDepartmentID);

           //#####ProjectGroup_ID###Int64
//属性测试300ProjectGroup_ID
//属性测试300ProjectGroup_ID
//属性测试300ProjectGroup_ID
//属性测试300ProjectGroup_ID
//属性测试300ProjectGroup_ID
this.lblProjectGroup_ID.AutoSize = true;
this.lblProjectGroup_ID.Location = new System.Drawing.Point(100,300);
this.lblProjectGroup_ID.Name = "lblProjectGroup_ID";
this.lblProjectGroup_ID.Size = new System.Drawing.Size(41, 12);
this.lblProjectGroup_ID.TabIndex = 12;
this.lblProjectGroup_ID.Text = "项目组";
//111======300
this.cmbProjectGroup_ID.Location = new System.Drawing.Point(173,296);
this.cmbProjectGroup_ID.Name ="cmbProjectGroup_ID";
this.cmbProjectGroup_ID.Size = new System.Drawing.Size(100, 21);
this.cmbProjectGroup_ID.TabIndex = 12;
this.Controls.Add(this.lblProjectGroup_ID);
this.Controls.Add(this.cmbProjectGroup_ID);

           //#####Employee_ID###Int64
//属性测试325Employee_ID
//属性测试325Employee_ID
//属性测试325Employee_ID
//属性测试325Employee_ID
this.lblEmployee_ID.AutoSize = true;
this.lblEmployee_ID.Location = new System.Drawing.Point(100,325);
this.lblEmployee_ID.Name = "lblEmployee_ID";
this.lblEmployee_ID.Size = new System.Drawing.Size(41, 12);
this.lblEmployee_ID.TabIndex = 13;
this.lblEmployee_ID.Text = "经办人";
//111======325
this.cmbEmployee_ID.Location = new System.Drawing.Point(173,321);
this.cmbEmployee_ID.Name ="cmbEmployee_ID";
this.cmbEmployee_ID.Size = new System.Drawing.Size(100, 21);
this.cmbEmployee_ID.TabIndex = 13;
this.Controls.Add(this.lblEmployee_ID);
this.Controls.Add(this.cmbEmployee_ID);

           //#####InvoiceId###Int64
//属性测试350InvoiceId
//属性测试350InvoiceId
//属性测试350InvoiceId
//属性测试350InvoiceId
//属性测试350InvoiceId

           //#####Invoiced###Boolean
this.lblInvoiced.AutoSize = true;
this.lblInvoiced.Location = new System.Drawing.Point(100,375);
this.lblInvoiced.Name = "lblInvoiced";
this.lblInvoiced.Size = new System.Drawing.Size(41, 12);
this.lblInvoiced.TabIndex = 15;
this.lblInvoiced.Text = "已开票";
this.chkInvoiced.Location = new System.Drawing.Point(173,371);
this.chkInvoiced.Name = "chkInvoiced";
this.chkInvoiced.Size = new System.Drawing.Size(100, 21);
this.chkInvoiced.TabIndex = 15;
this.Controls.Add(this.lblInvoiced);
this.Controls.Add(this.chkInvoiced);

           //#####TotalForeignDiffAmount###Decimal
this.lblTotalForeignDiffAmount.AutoSize = true;
this.lblTotalForeignDiffAmount.Location = new System.Drawing.Point(100,400);
this.lblTotalForeignDiffAmount.Name = "lblTotalForeignDiffAmount";
this.lblTotalForeignDiffAmount.Size = new System.Drawing.Size(41, 12);
this.lblTotalForeignDiffAmount.TabIndex = 16;
this.lblTotalForeignDiffAmount.Text = "金额差总计外币";
//111======400
this.txtTotalForeignDiffAmount.Location = new System.Drawing.Point(173,396);
this.txtTotalForeignDiffAmount.Name ="txtTotalForeignDiffAmount";
this.txtTotalForeignDiffAmount.Size = new System.Drawing.Size(100, 21);
this.txtTotalForeignDiffAmount.TabIndex = 16;
this.Controls.Add(this.lblTotalForeignDiffAmount);
this.Controls.Add(this.txtTotalForeignDiffAmount);

           //#####TotalLocalDiffAmount###Decimal
this.lblTotalLocalDiffAmount.AutoSize = true;
this.lblTotalLocalDiffAmount.Location = new System.Drawing.Point(100,425);
this.lblTotalLocalDiffAmount.Name = "lblTotalLocalDiffAmount";
this.lblTotalLocalDiffAmount.Size = new System.Drawing.Size(41, 12);
this.lblTotalLocalDiffAmount.TabIndex = 17;
this.lblTotalLocalDiffAmount.Text = "金额差总计本币";
//111======425
this.txtTotalLocalDiffAmount.Location = new System.Drawing.Point(173,421);
this.txtTotalLocalDiffAmount.Name ="txtTotalLocalDiffAmount";
this.txtTotalLocalDiffAmount.Size = new System.Drawing.Size(100, 21);
this.txtTotalLocalDiffAmount.TabIndex = 17;
this.Controls.Add(this.lblTotalLocalDiffAmount);
this.Controls.Add(this.txtTotalLocalDiffAmount);

           //#####IsIncludeTax###Boolean
this.lblIsIncludeTax.AutoSize = true;
this.lblIsIncludeTax.Location = new System.Drawing.Point(100,450);
this.lblIsIncludeTax.Name = "lblIsIncludeTax";
this.lblIsIncludeTax.Size = new System.Drawing.Size(41, 12);
this.lblIsIncludeTax.TabIndex = 18;
this.lblIsIncludeTax.Text = "含税";
this.chkIsIncludeTax.Location = new System.Drawing.Point(173,446);
this.chkIsIncludeTax.Name = "chkIsIncludeTax";
this.chkIsIncludeTax.Size = new System.Drawing.Size(100, 21);
this.chkIsIncludeTax.TabIndex = 18;
this.Controls.Add(this.lblIsIncludeTax);
this.Controls.Add(this.chkIsIncludeTax);

           //#####TaxTotalDiffLocalAmount###Decimal
this.lblTaxTotalDiffLocalAmount.AutoSize = true;
this.lblTaxTotalDiffLocalAmount.Location = new System.Drawing.Point(100,475);
this.lblTaxTotalDiffLocalAmount.Name = "lblTaxTotalDiffLocalAmount";
this.lblTaxTotalDiffLocalAmount.Size = new System.Drawing.Size(41, 12);
this.lblTaxTotalDiffLocalAmount.TabIndex = 19;
this.lblTaxTotalDiffLocalAmount.Text = "税额差总计";
//111======475
this.txtTaxTotalDiffLocalAmount.Location = new System.Drawing.Point(173,471);
this.txtTaxTotalDiffLocalAmount.Name ="txtTaxTotalDiffLocalAmount";
this.txtTaxTotalDiffLocalAmount.Size = new System.Drawing.Size(100, 21);
this.txtTaxTotalDiffLocalAmount.TabIndex = 19;
this.Controls.Add(this.lblTaxTotalDiffLocalAmount);
this.Controls.Add(this.txtTaxTotalDiffLocalAmount);

           //#####DataStatus###Int32
//属性测试500DataStatus
//属性测试500DataStatus
//属性测试500DataStatus
//属性测试500DataStatus
//属性测试500DataStatus

           //#####300Remark###String
this.lblRemark.AutoSize = true;
this.lblRemark.Location = new System.Drawing.Point(100,525);
this.lblRemark.Name = "lblRemark";
this.lblRemark.Size = new System.Drawing.Size(41, 12);
this.lblRemark.TabIndex = 21;
this.lblRemark.Text = "备注";
this.txtRemark.Location = new System.Drawing.Point(173,521);
this.txtRemark.Name = "txtRemark";
this.txtRemark.Size = new System.Drawing.Size(100, 21);
this.txtRemark.TabIndex = 21;
this.Controls.Add(this.lblRemark);
this.Controls.Add(this.txtRemark);

           //#####255ApprovalOpinions###String
this.lblApprovalOpinions.AutoSize = true;
this.lblApprovalOpinions.Location = new System.Drawing.Point(100,550);
this.lblApprovalOpinions.Name = "lblApprovalOpinions";
this.lblApprovalOpinions.Size = new System.Drawing.Size(41, 12);
this.lblApprovalOpinions.TabIndex = 22;
this.lblApprovalOpinions.Text = "审批意见";
this.txtApprovalOpinions.Location = new System.Drawing.Point(173,546);
this.txtApprovalOpinions.Name = "txtApprovalOpinions";
this.txtApprovalOpinions.Size = new System.Drawing.Size(100, 21);
this.txtApprovalOpinions.TabIndex = 22;
this.Controls.Add(this.lblApprovalOpinions);
this.Controls.Add(this.txtApprovalOpinions);

           //#####Approver_by###Int64
//属性测试575Approver_by
//属性测试575Approver_by
//属性测试575Approver_by
//属性测试575Approver_by
//属性测试575Approver_by

           //#####Approver_at###DateTime
this.lblApprover_at.AutoSize = true;
this.lblApprover_at.Location = new System.Drawing.Point(100,600);
this.lblApprover_at.Name = "lblApprover_at";
this.lblApprover_at.Size = new System.Drawing.Size(41, 12);
this.lblApprover_at.TabIndex = 24;
this.lblApprover_at.Text = "审批时间";
//111======600
this.dtpApprover_at.Location = new System.Drawing.Point(173,596);
this.dtpApprover_at.Name ="dtpApprover_at";
this.dtpApprover_at.ShowCheckBox =true;
this.dtpApprover_at.Size = new System.Drawing.Size(100, 21);
this.dtpApprover_at.TabIndex = 24;
this.Controls.Add(this.lblApprover_at);
this.Controls.Add(this.dtpApprover_at);

           //#####ApprovalStatus###SByte

           //#####ApprovalResults###Boolean
this.lblApprovalResults.AutoSize = true;
this.lblApprovalResults.Location = new System.Drawing.Point(100,650);
this.lblApprovalResults.Name = "lblApprovalResults";
this.lblApprovalResults.Size = new System.Drawing.Size(41, 12);
this.lblApprovalResults.TabIndex = 26;
this.lblApprovalResults.Text = "审批结果";
this.chkApprovalResults.Location = new System.Drawing.Point(173,646);
this.chkApprovalResults.Name = "chkApprovalResults";
this.chkApprovalResults.Size = new System.Drawing.Size(100, 21);
this.chkApprovalResults.TabIndex = 26;
this.Controls.Add(this.lblApprovalResults);
this.Controls.Add(this.chkApprovalResults);

           //#####PrintStatus###Int32
//属性测试675PrintStatus
//属性测试675PrintStatus
//属性测试675PrintStatus
//属性测试675PrintStatus
//属性测试675PrintStatus

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,700);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 28;
this.lblCreated_at.Text = "创建时间";
//111======700
this.dtpCreated_at.Location = new System.Drawing.Point(173,696);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 28;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
//属性测试725Created_by
//属性测试725Created_by
//属性测试725Created_by
//属性测试725Created_by
//属性测试725Created_by

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,750);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 30;
this.lblModified_at.Text = "修改时间";
//111======750
this.dtpModified_at.Location = new System.Drawing.Point(173,746);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 30;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
//属性测试775Modified_by
//属性测试775Modified_by
//属性测试775Modified_by
//属性测试775Modified_by
//属性测试775Modified_by

           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,800);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 32;
this.lblisdeleted.Text = "逻辑删除";
this.chkisdeleted.Location = new System.Drawing.Point(173,796);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 32;
this.Controls.Add(this.lblisdeleted);
this.Controls.Add(this.chkisdeleted);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblAdjustNo );
this.Controls.Add(this.txtAdjustNo );

                this.Controls.Add(this.lblCustomerVendor_ID );
this.Controls.Add(this.cmbCustomerVendor_ID );

                
                
                
                this.Controls.Add(this.lblSourceBillNo );
this.Controls.Add(this.txtSourceBillNo );

                this.Controls.Add(this.lblCurrency_ID );
this.Controls.Add(this.cmbCurrency_ID );

                this.Controls.Add(this.lblAdjustReason );
this.Controls.Add(this.txtAdjustReason );

                this.Controls.Add(this.lblExchangeRate );
this.Controls.Add(this.txtExchangeRate );

                this.Controls.Add(this.lblAdjustDate );
this.Controls.Add(this.dtpAdjustDate );

                this.Controls.Add(this.lblDepartmentID );
this.Controls.Add(this.cmbDepartmentID );

                this.Controls.Add(this.lblProjectGroup_ID );
this.Controls.Add(this.cmbProjectGroup_ID );

                this.Controls.Add(this.lblEmployee_ID );
this.Controls.Add(this.cmbEmployee_ID );

                
                this.Controls.Add(this.lblInvoiced );
this.Controls.Add(this.chkInvoiced );

                this.Controls.Add(this.lblTotalForeignDiffAmount );
this.Controls.Add(this.txtTotalForeignDiffAmount );

                this.Controls.Add(this.lblTotalLocalDiffAmount );
this.Controls.Add(this.txtTotalLocalDiffAmount );

                this.Controls.Add(this.lblIsIncludeTax );
this.Controls.Add(this.chkIsIncludeTax );

                this.Controls.Add(this.lblTaxTotalDiffLocalAmount );
this.Controls.Add(this.txtTaxTotalDiffLocalAmount );

                
                this.Controls.Add(this.lblRemark );
this.Controls.Add(this.txtRemark );

                this.Controls.Add(this.lblApprovalOpinions );
this.Controls.Add(this.txtApprovalOpinions );

                
                this.Controls.Add(this.lblApprover_at );
this.Controls.Add(this.dtpApprover_at );

                
                this.Controls.Add(this.lblApprovalResults );
this.Controls.Add(this.chkApprovalResults );

                
                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                
                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                
                this.Controls.Add(this.lblisdeleted );
this.Controls.Add(this.chkisdeleted );

                    
            this.Name = "tb_FM_PriceAdjustmentQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblAdjustNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtAdjustNo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCustomerVendor_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbCustomerVendor_ID;

    
        
              
    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSourceBillNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSourceBillNo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCurrency_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbCurrency_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblAdjustReason;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtAdjustReason;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblExchangeRate;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtExchangeRate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblAdjustDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpAdjustDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDepartmentID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbDepartmentID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProjectGroup_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbProjectGroup_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblInvoiced;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkInvoiced;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalForeignDiffAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalForeignDiffAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalLocalDiffAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalLocalDiffAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIsIncludeTax;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIsIncludeTax;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTaxTotalDiffLocalAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTaxTotalDiffLocalAmount;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRemark;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtRemark;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprovalOpinions;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtApprovalOpinions;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprover_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpApprover_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprovalResults;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkApprovalResults;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModified_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblisdeleted;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkisdeleted;

    
    
   
 





    }
}


