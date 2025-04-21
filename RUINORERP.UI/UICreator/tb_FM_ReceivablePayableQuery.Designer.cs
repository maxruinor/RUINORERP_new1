
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/21/2025 20:12:35
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 应收应付表
    /// </summary>
    partial class tb_FM_ReceivablePayableQuery
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
     
     this.lblARAPNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtARAPNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblPreRPID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbPreRPID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblCustomerVendor_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbCustomerVendor_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblCurrency_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbCurrency_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblExchangeRate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtExchangeRate = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblTotalForeignPayableAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalForeignPayableAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTotalLocalPayableAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalLocalPayableAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblForeignPaidAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtForeignPaidAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblLocalPaidAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtLocalPaidAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblForeignBalanceAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtForeignBalanceAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblLocalBalanceAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtLocalBalanceAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblPaymentDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpPaymentDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblDueDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpDueDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();




this.lblIsIncludeTax = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIsIncludeTax = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIsIncludeTax.Values.Text ="";

this.lblTaxTotalAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTaxTotalAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblUntaxedTotalAmont = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtUntaxedTotalAmont = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblRemark = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtRemark = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtRemark.Multiline = true;

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

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####30ARAPNo###String
this.lblARAPNo.AutoSize = true;
this.lblARAPNo.Location = new System.Drawing.Point(100,25);
this.lblARAPNo.Name = "lblARAPNo";
this.lblARAPNo.Size = new System.Drawing.Size(41, 12);
this.lblARAPNo.TabIndex = 1;
this.lblARAPNo.Text = "单据编号";
this.txtARAPNo.Location = new System.Drawing.Point(173,21);
this.txtARAPNo.Name = "txtARAPNo";
this.txtARAPNo.Size = new System.Drawing.Size(100, 21);
this.txtARAPNo.TabIndex = 1;
this.Controls.Add(this.lblARAPNo);
this.Controls.Add(this.txtARAPNo);

           //#####PreRPID###Int64
//属性测试50PreRPID
//属性测试50PreRPID
//属性测试50PreRPID
this.lblPreRPID.AutoSize = true;
this.lblPreRPID.Location = new System.Drawing.Point(100,50);
this.lblPreRPID.Name = "lblPreRPID";
this.lblPreRPID.Size = new System.Drawing.Size(41, 12);
this.lblPreRPID.TabIndex = 2;
this.lblPreRPID.Text = "预收付款单";
//111======50
this.cmbPreRPID.Location = new System.Drawing.Point(173,46);
this.cmbPreRPID.Name ="cmbPreRPID";
this.cmbPreRPID.Size = new System.Drawing.Size(100, 21);
this.cmbPreRPID.TabIndex = 2;
this.Controls.Add(this.lblPreRPID);
this.Controls.Add(this.cmbPreRPID);

           //#####CustomerVendor_ID###Int64
//属性测试75CustomerVendor_ID
//属性测试75CustomerVendor_ID
this.lblCustomerVendor_ID.AutoSize = true;
this.lblCustomerVendor_ID.Location = new System.Drawing.Point(100,75);
this.lblCustomerVendor_ID.Name = "lblCustomerVendor_ID";
this.lblCustomerVendor_ID.Size = new System.Drawing.Size(41, 12);
this.lblCustomerVendor_ID.TabIndex = 3;
this.lblCustomerVendor_ID.Text = "往来单位";
//111======75
this.cmbCustomerVendor_ID.Location = new System.Drawing.Point(173,71);
this.cmbCustomerVendor_ID.Name ="cmbCustomerVendor_ID";
this.cmbCustomerVendor_ID.Size = new System.Drawing.Size(100, 21);
this.cmbCustomerVendor_ID.TabIndex = 3;
this.Controls.Add(this.lblCustomerVendor_ID);
this.Controls.Add(this.cmbCustomerVendor_ID);

           //#####Currency_ID###Int64
//属性测试100Currency_ID
this.lblCurrency_ID.AutoSize = true;
this.lblCurrency_ID.Location = new System.Drawing.Point(100,100);
this.lblCurrency_ID.Name = "lblCurrency_ID";
this.lblCurrency_ID.Size = new System.Drawing.Size(41, 12);
this.lblCurrency_ID.TabIndex = 4;
this.lblCurrency_ID.Text = "币别";
//111======100
this.cmbCurrency_ID.Location = new System.Drawing.Point(173,96);
this.cmbCurrency_ID.Name ="cmbCurrency_ID";
this.cmbCurrency_ID.Size = new System.Drawing.Size(100, 21);
this.cmbCurrency_ID.TabIndex = 4;
this.Controls.Add(this.lblCurrency_ID);
this.Controls.Add(this.cmbCurrency_ID);

           //#####ExchangeRate###Decimal
this.lblExchangeRate.AutoSize = true;
this.lblExchangeRate.Location = new System.Drawing.Point(100,125);
this.lblExchangeRate.Name = "lblExchangeRate";
this.lblExchangeRate.Size = new System.Drawing.Size(41, 12);
this.lblExchangeRate.TabIndex = 5;
this.lblExchangeRate.Text = "汇率";
//111======125
this.txtExchangeRate.Location = new System.Drawing.Point(173,121);
this.txtExchangeRate.Name ="txtExchangeRate";
this.txtExchangeRate.Size = new System.Drawing.Size(100, 21);
this.txtExchangeRate.TabIndex = 5;
this.Controls.Add(this.lblExchangeRate);
this.Controls.Add(this.txtExchangeRate);

           //#####ReceivePaymentType###Int64
//属性测试150ReceivePaymentType
//属性测试150ReceivePaymentType
//属性测试150ReceivePaymentType

           //#####TotalForeignPayableAmount###Decimal
this.lblTotalForeignPayableAmount.AutoSize = true;
this.lblTotalForeignPayableAmount.Location = new System.Drawing.Point(100,175);
this.lblTotalForeignPayableAmount.Name = "lblTotalForeignPayableAmount";
this.lblTotalForeignPayableAmount.Size = new System.Drawing.Size(41, 12);
this.lblTotalForeignPayableAmount.TabIndex = 7;
this.lblTotalForeignPayableAmount.Text = "总金额外币";
//111======175
this.txtTotalForeignPayableAmount.Location = new System.Drawing.Point(173,171);
this.txtTotalForeignPayableAmount.Name ="txtTotalForeignPayableAmount";
this.txtTotalForeignPayableAmount.Size = new System.Drawing.Size(100, 21);
this.txtTotalForeignPayableAmount.TabIndex = 7;
this.Controls.Add(this.lblTotalForeignPayableAmount);
this.Controls.Add(this.txtTotalForeignPayableAmount);

           //#####TotalLocalPayableAmount###Decimal
this.lblTotalLocalPayableAmount.AutoSize = true;
this.lblTotalLocalPayableAmount.Location = new System.Drawing.Point(100,200);
this.lblTotalLocalPayableAmount.Name = "lblTotalLocalPayableAmount";
this.lblTotalLocalPayableAmount.Size = new System.Drawing.Size(41, 12);
this.lblTotalLocalPayableAmount.TabIndex = 8;
this.lblTotalLocalPayableAmount.Text = "总金额本币";
//111======200
this.txtTotalLocalPayableAmount.Location = new System.Drawing.Point(173,196);
this.txtTotalLocalPayableAmount.Name ="txtTotalLocalPayableAmount";
this.txtTotalLocalPayableAmount.Size = new System.Drawing.Size(100, 21);
this.txtTotalLocalPayableAmount.TabIndex = 8;
this.Controls.Add(this.lblTotalLocalPayableAmount);
this.Controls.Add(this.txtTotalLocalPayableAmount);

           //#####ForeignPaidAmount###Decimal
this.lblForeignPaidAmount.AutoSize = true;
this.lblForeignPaidAmount.Location = new System.Drawing.Point(100,225);
this.lblForeignPaidAmount.Name = "lblForeignPaidAmount";
this.lblForeignPaidAmount.Size = new System.Drawing.Size(41, 12);
this.lblForeignPaidAmount.TabIndex = 9;
this.lblForeignPaidAmount.Text = "已核销外币";
//111======225
this.txtForeignPaidAmount.Location = new System.Drawing.Point(173,221);
this.txtForeignPaidAmount.Name ="txtForeignPaidAmount";
this.txtForeignPaidAmount.Size = new System.Drawing.Size(100, 21);
this.txtForeignPaidAmount.TabIndex = 9;
this.Controls.Add(this.lblForeignPaidAmount);
this.Controls.Add(this.txtForeignPaidAmount);

           //#####LocalPaidAmount###Decimal
this.lblLocalPaidAmount.AutoSize = true;
this.lblLocalPaidAmount.Location = new System.Drawing.Point(100,250);
this.lblLocalPaidAmount.Name = "lblLocalPaidAmount";
this.lblLocalPaidAmount.Size = new System.Drawing.Size(41, 12);
this.lblLocalPaidAmount.TabIndex = 10;
this.lblLocalPaidAmount.Text = "已核销本币";
//111======250
this.txtLocalPaidAmount.Location = new System.Drawing.Point(173,246);
this.txtLocalPaidAmount.Name ="txtLocalPaidAmount";
this.txtLocalPaidAmount.Size = new System.Drawing.Size(100, 21);
this.txtLocalPaidAmount.TabIndex = 10;
this.Controls.Add(this.lblLocalPaidAmount);
this.Controls.Add(this.txtLocalPaidAmount);

           //#####ForeignBalanceAmount###Decimal
this.lblForeignBalanceAmount.AutoSize = true;
this.lblForeignBalanceAmount.Location = new System.Drawing.Point(100,275);
this.lblForeignBalanceAmount.Name = "lblForeignBalanceAmount";
this.lblForeignBalanceAmount.Size = new System.Drawing.Size(41, 12);
this.lblForeignBalanceAmount.TabIndex = 11;
this.lblForeignBalanceAmount.Text = "未核销外币";
//111======275
this.txtForeignBalanceAmount.Location = new System.Drawing.Point(173,271);
this.txtForeignBalanceAmount.Name ="txtForeignBalanceAmount";
this.txtForeignBalanceAmount.Size = new System.Drawing.Size(100, 21);
this.txtForeignBalanceAmount.TabIndex = 11;
this.Controls.Add(this.lblForeignBalanceAmount);
this.Controls.Add(this.txtForeignBalanceAmount);

           //#####LocalBalanceAmount###Decimal
this.lblLocalBalanceAmount.AutoSize = true;
this.lblLocalBalanceAmount.Location = new System.Drawing.Point(100,300);
this.lblLocalBalanceAmount.Name = "lblLocalBalanceAmount";
this.lblLocalBalanceAmount.Size = new System.Drawing.Size(41, 12);
this.lblLocalBalanceAmount.TabIndex = 12;
this.lblLocalBalanceAmount.Text = "未核销本币";
//111======300
this.txtLocalBalanceAmount.Location = new System.Drawing.Point(173,296);
this.txtLocalBalanceAmount.Name ="txtLocalBalanceAmount";
this.txtLocalBalanceAmount.Size = new System.Drawing.Size(100, 21);
this.txtLocalBalanceAmount.TabIndex = 12;
this.Controls.Add(this.lblLocalBalanceAmount);
this.Controls.Add(this.txtLocalBalanceAmount);

           //#####PaymentDate###DateTime
this.lblPaymentDate.AutoSize = true;
this.lblPaymentDate.Location = new System.Drawing.Point(100,325);
this.lblPaymentDate.Name = "lblPaymentDate";
this.lblPaymentDate.Size = new System.Drawing.Size(41, 12);
this.lblPaymentDate.TabIndex = 13;
this.lblPaymentDate.Text = "支付日期";
//111======325
this.dtpPaymentDate.Location = new System.Drawing.Point(173,321);
this.dtpPaymentDate.Name ="dtpPaymentDate";
this.dtpPaymentDate.ShowCheckBox =true;
this.dtpPaymentDate.Size = new System.Drawing.Size(100, 21);
this.dtpPaymentDate.TabIndex = 13;
this.Controls.Add(this.lblPaymentDate);
this.Controls.Add(this.dtpPaymentDate);

           //#####DueDate###DateTime
this.lblDueDate.AutoSize = true;
this.lblDueDate.Location = new System.Drawing.Point(100,350);
this.lblDueDate.Name = "lblDueDate";
this.lblDueDate.Size = new System.Drawing.Size(41, 12);
this.lblDueDate.TabIndex = 14;
this.lblDueDate.Text = "到期日";
//111======350
this.dtpDueDate.Location = new System.Drawing.Point(173,346);
this.dtpDueDate.Name ="dtpDueDate";
this.dtpDueDate.ShowCheckBox =true;
this.dtpDueDate.Size = new System.Drawing.Size(100, 21);
this.dtpDueDate.TabIndex = 14;
this.Controls.Add(this.lblDueDate);
this.Controls.Add(this.dtpDueDate);

           //#####Employee_ID###Int64
//属性测试375Employee_ID
//属性测试375Employee_ID
//属性测试375Employee_ID

           //#####DepartmentID###Int64
//属性测试400DepartmentID
//属性测试400DepartmentID
//属性测试400DepartmentID

           //#####ProjectGroup_ID###Int64
//属性测试425ProjectGroup_ID
//属性测试425ProjectGroup_ID
//属性测试425ProjectGroup_ID

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

           //#####TaxTotalAmount###Decimal
this.lblTaxTotalAmount.AutoSize = true;
this.lblTaxTotalAmount.Location = new System.Drawing.Point(100,475);
this.lblTaxTotalAmount.Name = "lblTaxTotalAmount";
this.lblTaxTotalAmount.Size = new System.Drawing.Size(41, 12);
this.lblTaxTotalAmount.TabIndex = 19;
this.lblTaxTotalAmount.Text = "税额总计";
//111======475
this.txtTaxTotalAmount.Location = new System.Drawing.Point(173,471);
this.txtTaxTotalAmount.Name ="txtTaxTotalAmount";
this.txtTaxTotalAmount.Size = new System.Drawing.Size(100, 21);
this.txtTaxTotalAmount.TabIndex = 19;
this.Controls.Add(this.lblTaxTotalAmount);
this.Controls.Add(this.txtTaxTotalAmount);

           //#####UntaxedTotalAmont###Decimal
this.lblUntaxedTotalAmont.AutoSize = true;
this.lblUntaxedTotalAmont.Location = new System.Drawing.Point(100,500);
this.lblUntaxedTotalAmont.Name = "lblUntaxedTotalAmont";
this.lblUntaxedTotalAmont.Size = new System.Drawing.Size(41, 12);
this.lblUntaxedTotalAmont.TabIndex = 20;
this.lblUntaxedTotalAmont.Text = "未税总计";
//111======500
this.txtUntaxedTotalAmont.Location = new System.Drawing.Point(173,496);
this.txtUntaxedTotalAmont.Name ="txtUntaxedTotalAmont";
this.txtUntaxedTotalAmont.Size = new System.Drawing.Size(100, 21);
this.txtUntaxedTotalAmont.TabIndex = 20;
this.Controls.Add(this.lblUntaxedTotalAmont);
this.Controls.Add(this.txtUntaxedTotalAmont);

           //#####FMPaymentStatus###Int32
//属性测试525FMPaymentStatus
//属性测试525FMPaymentStatus
//属性测试525FMPaymentStatus

           //#####300Remark###String
this.lblRemark.AutoSize = true;
this.lblRemark.Location = new System.Drawing.Point(100,550);
this.lblRemark.Name = "lblRemark";
this.lblRemark.Size = new System.Drawing.Size(41, 12);
this.lblRemark.TabIndex = 22;
this.lblRemark.Text = "备注";
this.txtRemark.Location = new System.Drawing.Point(173,546);
this.txtRemark.Name = "txtRemark";
this.txtRemark.Size = new System.Drawing.Size(100, 21);
this.txtRemark.TabIndex = 22;
this.Controls.Add(this.lblRemark);
this.Controls.Add(this.txtRemark);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,575);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 23;
this.lblCreated_at.Text = "创建时间";
//111======575
this.dtpCreated_at.Location = new System.Drawing.Point(173,571);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 23;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
//属性测试600Created_by
//属性测试600Created_by
//属性测试600Created_by

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,625);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 25;
this.lblModified_at.Text = "修改时间";
//111======625
this.dtpModified_at.Location = new System.Drawing.Point(173,621);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 25;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
//属性测试650Modified_by
//属性测试650Modified_by
//属性测试650Modified_by

           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,675);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 27;
this.lblisdeleted.Text = "逻辑删除";
this.chkisdeleted.Location = new System.Drawing.Point(173,671);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 27;
this.Controls.Add(this.lblisdeleted);
this.Controls.Add(this.chkisdeleted);

           //#####255ApprovalOpinions###String
this.lblApprovalOpinions.AutoSize = true;
this.lblApprovalOpinions.Location = new System.Drawing.Point(100,700);
this.lblApprovalOpinions.Name = "lblApprovalOpinions";
this.lblApprovalOpinions.Size = new System.Drawing.Size(41, 12);
this.lblApprovalOpinions.TabIndex = 28;
this.lblApprovalOpinions.Text = "审批意见";
this.txtApprovalOpinions.Location = new System.Drawing.Point(173,696);
this.txtApprovalOpinions.Name = "txtApprovalOpinions";
this.txtApprovalOpinions.Size = new System.Drawing.Size(100, 21);
this.txtApprovalOpinions.TabIndex = 28;
this.Controls.Add(this.lblApprovalOpinions);
this.Controls.Add(this.txtApprovalOpinions);

           //#####Approver_by###Int64
//属性测试725Approver_by
//属性测试725Approver_by
//属性测试725Approver_by

           //#####Approver_at###DateTime
this.lblApprover_at.AutoSize = true;
this.lblApprover_at.Location = new System.Drawing.Point(100,750);
this.lblApprover_at.Name = "lblApprover_at";
this.lblApprover_at.Size = new System.Drawing.Size(41, 12);
this.lblApprover_at.TabIndex = 30;
this.lblApprover_at.Text = "审批时间";
//111======750
this.dtpApprover_at.Location = new System.Drawing.Point(173,746);
this.dtpApprover_at.Name ="dtpApprover_at";
this.dtpApprover_at.ShowCheckBox =true;
this.dtpApprover_at.Size = new System.Drawing.Size(100, 21);
this.dtpApprover_at.TabIndex = 30;
this.Controls.Add(this.lblApprover_at);
this.Controls.Add(this.dtpApprover_at);

           //#####ApprovalStatus###SByte

           //#####ApprovalResults###Boolean
this.lblApprovalResults.AutoSize = true;
this.lblApprovalResults.Location = new System.Drawing.Point(100,800);
this.lblApprovalResults.Name = "lblApprovalResults";
this.lblApprovalResults.Size = new System.Drawing.Size(41, 12);
this.lblApprovalResults.TabIndex = 32;
this.lblApprovalResults.Text = "审批结果";
this.chkApprovalResults.Location = new System.Drawing.Point(173,796);
this.chkApprovalResults.Name = "chkApprovalResults";
this.chkApprovalResults.Size = new System.Drawing.Size(100, 21);
this.chkApprovalResults.TabIndex = 32;
this.Controls.Add(this.lblApprovalResults);
this.Controls.Add(this.chkApprovalResults);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblARAPNo );
this.Controls.Add(this.txtARAPNo );

                this.Controls.Add(this.lblPreRPID );
this.Controls.Add(this.cmbPreRPID );

                this.Controls.Add(this.lblCustomerVendor_ID );
this.Controls.Add(this.cmbCustomerVendor_ID );

                this.Controls.Add(this.lblCurrency_ID );
this.Controls.Add(this.cmbCurrency_ID );

                this.Controls.Add(this.lblExchangeRate );
this.Controls.Add(this.txtExchangeRate );

                
                this.Controls.Add(this.lblTotalForeignPayableAmount );
this.Controls.Add(this.txtTotalForeignPayableAmount );

                this.Controls.Add(this.lblTotalLocalPayableAmount );
this.Controls.Add(this.txtTotalLocalPayableAmount );

                this.Controls.Add(this.lblForeignPaidAmount );
this.Controls.Add(this.txtForeignPaidAmount );

                this.Controls.Add(this.lblLocalPaidAmount );
this.Controls.Add(this.txtLocalPaidAmount );

                this.Controls.Add(this.lblForeignBalanceAmount );
this.Controls.Add(this.txtForeignBalanceAmount );

                this.Controls.Add(this.lblLocalBalanceAmount );
this.Controls.Add(this.txtLocalBalanceAmount );

                this.Controls.Add(this.lblPaymentDate );
this.Controls.Add(this.dtpPaymentDate );

                this.Controls.Add(this.lblDueDate );
this.Controls.Add(this.dtpDueDate );

                
                
                
                this.Controls.Add(this.lblIsIncludeTax );
this.Controls.Add(this.chkIsIncludeTax );

                this.Controls.Add(this.lblTaxTotalAmount );
this.Controls.Add(this.txtTaxTotalAmount );

                this.Controls.Add(this.lblUntaxedTotalAmont );
this.Controls.Add(this.txtUntaxedTotalAmont );

                
                this.Controls.Add(this.lblRemark );
this.Controls.Add(this.txtRemark );

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

                    
            this.Name = "tb_FM_ReceivablePayableQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblARAPNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtARAPNo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPreRPID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbPreRPID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCustomerVendor_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbCustomerVendor_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCurrency_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbCurrency_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblExchangeRate;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtExchangeRate;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalForeignPayableAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalForeignPayableAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalLocalPayableAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalLocalPayableAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblForeignPaidAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtForeignPaidAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblLocalPaidAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtLocalPaidAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblForeignBalanceAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtForeignBalanceAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblLocalBalanceAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtLocalBalanceAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPaymentDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpPaymentDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDueDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpDueDate;

    
        
              
    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIsIncludeTax;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIsIncludeTax;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTaxTotalAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTaxTotalAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblUntaxedTotalAmont;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtUntaxedTotalAmont;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRemark;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtRemark;

    
        
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

    
    
   
 





    }
}


