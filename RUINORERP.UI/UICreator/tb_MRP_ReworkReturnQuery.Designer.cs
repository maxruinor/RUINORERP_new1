
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：01/04/2025 19:45:30
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 返工退库
    /// </summary>
    partial class tb_MRP_ReworkReturnQuery
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
     
     this.lblReworkReturnNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtReworkReturnNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCustomerVendor_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbCustomerVendor_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblDepartmentID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbDepartmentID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblMOID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbMOID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();


this.lblTotalReworkFee = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalReworkFee = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTotalCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblReturnDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpReturnDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblExpectedReturnDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpExpectedReturnDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblReasonForRework = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtReasonForRework = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtReasonForRework.Multiline = true;

this.lblisdeleted = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkisdeleted = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkisdeleted.Values.Text ="";

this.lblCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblModified_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblNotes = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotes = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

this.lblApprovalOpinions = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtApprovalOpinions = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblApprovalResults = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkApprovalResults = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkApprovalResults.Values.Text ="";

this.lblCloseCaseOpinions = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCloseCaseOpinions = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblReceiptInvoiceClosed = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkReceiptInvoiceClosed = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkReceiptInvoiceClosed.Values.Text ="";



this.lblApprover_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpApprover_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblGenerateVouchers = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkGenerateVouchers = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkGenerateVouchers.Values.Text ="";


    //for end
    this.SuspendLayout();
    
         //for start
                 //#####50ReworkReturnNo###String
this.lblReworkReturnNo.AutoSize = true;
this.lblReworkReturnNo.Location = new System.Drawing.Point(100,25);
this.lblReworkReturnNo.Name = "lblReworkReturnNo";
this.lblReworkReturnNo.Size = new System.Drawing.Size(41, 12);
this.lblReworkReturnNo.TabIndex = 1;
this.lblReworkReturnNo.Text = "退回单号";
this.txtReworkReturnNo.Location = new System.Drawing.Point(173,21);
this.txtReworkReturnNo.Name = "txtReworkReturnNo";
this.txtReworkReturnNo.Size = new System.Drawing.Size(100, 21);
this.txtReworkReturnNo.TabIndex = 1;
this.Controls.Add(this.lblReworkReturnNo);
this.Controls.Add(this.txtReworkReturnNo);

           //#####CustomerVendor_ID###Int64
//属性测试50CustomerVendor_ID
this.lblCustomerVendor_ID.AutoSize = true;
this.lblCustomerVendor_ID.Location = new System.Drawing.Point(100,50);
this.lblCustomerVendor_ID.Name = "lblCustomerVendor_ID";
this.lblCustomerVendor_ID.Size = new System.Drawing.Size(41, 12);
this.lblCustomerVendor_ID.TabIndex = 2;
this.lblCustomerVendor_ID.Text = "生产单位";
//111======50
this.cmbCustomerVendor_ID.Location = new System.Drawing.Point(173,46);
this.cmbCustomerVendor_ID.Name ="cmbCustomerVendor_ID";
this.cmbCustomerVendor_ID.Size = new System.Drawing.Size(100, 21);
this.cmbCustomerVendor_ID.TabIndex = 2;
this.Controls.Add(this.lblCustomerVendor_ID);
this.Controls.Add(this.cmbCustomerVendor_ID);

           //#####DepartmentID###Int64
//属性测试75DepartmentID
//属性测试75DepartmentID
//属性测试75DepartmentID
//属性测试75DepartmentID
this.lblDepartmentID.AutoSize = true;
this.lblDepartmentID.Location = new System.Drawing.Point(100,75);
this.lblDepartmentID.Name = "lblDepartmentID";
this.lblDepartmentID.Size = new System.Drawing.Size(41, 12);
this.lblDepartmentID.TabIndex = 3;
this.lblDepartmentID.Text = "需求部门";
//111======75
this.cmbDepartmentID.Location = new System.Drawing.Point(173,71);
this.cmbDepartmentID.Name ="cmbDepartmentID";
this.cmbDepartmentID.Size = new System.Drawing.Size(100, 21);
this.cmbDepartmentID.TabIndex = 3;
this.Controls.Add(this.lblDepartmentID);
this.Controls.Add(this.cmbDepartmentID);

           //#####Employee_ID###Int64
//属性测试100Employee_ID
//属性测试100Employee_ID
this.lblEmployee_ID.AutoSize = true;
this.lblEmployee_ID.Location = new System.Drawing.Point(100,100);
this.lblEmployee_ID.Name = "lblEmployee_ID";
this.lblEmployee_ID.Size = new System.Drawing.Size(41, 12);
this.lblEmployee_ID.TabIndex = 4;
this.lblEmployee_ID.Text = "经办人";
//111======100
this.cmbEmployee_ID.Location = new System.Drawing.Point(173,96);
this.cmbEmployee_ID.Name ="cmbEmployee_ID";
this.cmbEmployee_ID.Size = new System.Drawing.Size(100, 21);
this.cmbEmployee_ID.TabIndex = 4;
this.Controls.Add(this.lblEmployee_ID);
this.Controls.Add(this.cmbEmployee_ID);

           //#####MOID###Int64
//属性测试125MOID
//属性测试125MOID
//属性测试125MOID
this.lblMOID.AutoSize = true;
this.lblMOID.Location = new System.Drawing.Point(100,125);
this.lblMOID.Name = "lblMOID";
this.lblMOID.Size = new System.Drawing.Size(41, 12);
this.lblMOID.TabIndex = 5;
this.lblMOID.Text = "制令单";
//111======125
this.cmbMOID.Location = new System.Drawing.Point(173,121);
this.cmbMOID.Name ="cmbMOID";
this.cmbMOID.Size = new System.Drawing.Size(100, 21);
this.cmbMOID.TabIndex = 5;
this.Controls.Add(this.lblMOID);
this.Controls.Add(this.cmbMOID);

           //#####TotalQty###Int32
//属性测试150TotalQty
//属性测试150TotalQty
//属性测试150TotalQty
//属性测试150TotalQty

           //#####TotalReworkFee###Decimal
this.lblTotalReworkFee.AutoSize = true;
this.lblTotalReworkFee.Location = new System.Drawing.Point(100,175);
this.lblTotalReworkFee.Name = "lblTotalReworkFee";
this.lblTotalReworkFee.Size = new System.Drawing.Size(41, 12);
this.lblTotalReworkFee.TabIndex = 7;
this.lblTotalReworkFee.Text = "返工费用";
//111======175
this.txtTotalReworkFee.Location = new System.Drawing.Point(173,171);
this.txtTotalReworkFee.Name ="txtTotalReworkFee";
this.txtTotalReworkFee.Size = new System.Drawing.Size(100, 21);
this.txtTotalReworkFee.TabIndex = 7;
this.Controls.Add(this.lblTotalReworkFee);
this.Controls.Add(this.txtTotalReworkFee);

           //#####TotalCost###Decimal
this.lblTotalCost.AutoSize = true;
this.lblTotalCost.Location = new System.Drawing.Point(100,200);
this.lblTotalCost.Name = "lblTotalCost";
this.lblTotalCost.Size = new System.Drawing.Size(41, 12);
this.lblTotalCost.TabIndex = 8;
this.lblTotalCost.Text = "成本金额";
//111======200
this.txtTotalCost.Location = new System.Drawing.Point(173,196);
this.txtTotalCost.Name ="txtTotalCost";
this.txtTotalCost.Size = new System.Drawing.Size(100, 21);
this.txtTotalCost.TabIndex = 8;
this.Controls.Add(this.lblTotalCost);
this.Controls.Add(this.txtTotalCost);

           //#####ReturnDate###DateTime
this.lblReturnDate.AutoSize = true;
this.lblReturnDate.Location = new System.Drawing.Point(100,225);
this.lblReturnDate.Name = "lblReturnDate";
this.lblReturnDate.Size = new System.Drawing.Size(41, 12);
this.lblReturnDate.TabIndex = 9;
this.lblReturnDate.Text = "退回日期";
//111======225
this.dtpReturnDate.Location = new System.Drawing.Point(173,221);
this.dtpReturnDate.Name ="dtpReturnDate";
this.dtpReturnDate.Size = new System.Drawing.Size(100, 21);
this.dtpReturnDate.TabIndex = 9;
this.Controls.Add(this.lblReturnDate);
this.Controls.Add(this.dtpReturnDate);

           //#####ExpectedReturnDate###DateTime
this.lblExpectedReturnDate.AutoSize = true;
this.lblExpectedReturnDate.Location = new System.Drawing.Point(100,250);
this.lblExpectedReturnDate.Name = "lblExpectedReturnDate";
this.lblExpectedReturnDate.Size = new System.Drawing.Size(41, 12);
this.lblExpectedReturnDate.TabIndex = 10;
this.lblExpectedReturnDate.Text = "预完工期";
//111======250
this.dtpExpectedReturnDate.Location = new System.Drawing.Point(173,246);
this.dtpExpectedReturnDate.Name ="dtpExpectedReturnDate";
this.dtpExpectedReturnDate.ShowCheckBox =true;
this.dtpExpectedReturnDate.Size = new System.Drawing.Size(100, 21);
this.dtpExpectedReturnDate.TabIndex = 10;
this.Controls.Add(this.lblExpectedReturnDate);
this.Controls.Add(this.dtpExpectedReturnDate);

           //#####500ReasonForRework###String
this.lblReasonForRework.AutoSize = true;
this.lblReasonForRework.Location = new System.Drawing.Point(100,275);
this.lblReasonForRework.Name = "lblReasonForRework";
this.lblReasonForRework.Size = new System.Drawing.Size(41, 12);
this.lblReasonForRework.TabIndex = 11;
this.lblReasonForRework.Text = "返工原因";
this.txtReasonForRework.Location = new System.Drawing.Point(173,271);
this.txtReasonForRework.Name = "txtReasonForRework";
this.txtReasonForRework.Size = new System.Drawing.Size(100, 21);
this.txtReasonForRework.TabIndex = 11;
this.Controls.Add(this.lblReasonForRework);
this.Controls.Add(this.txtReasonForRework);

           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,300);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 12;
this.lblisdeleted.Text = "逻辑删除";
this.chkisdeleted.Location = new System.Drawing.Point(173,296);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 12;
this.Controls.Add(this.lblisdeleted);
this.Controls.Add(this.chkisdeleted);

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
//属性测试350Created_by
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
//属性测试400Modified_by
//属性测试400Modified_by
//属性测试400Modified_by

           //#####1500Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,425);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 17;
this.lblNotes.Text = "备注";
this.txtNotes.Location = new System.Drawing.Point(173,421);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 17;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####200ApprovalOpinions###String
this.lblApprovalOpinions.AutoSize = true;
this.lblApprovalOpinions.Location = new System.Drawing.Point(100,450);
this.lblApprovalOpinions.Name = "lblApprovalOpinions";
this.lblApprovalOpinions.Size = new System.Drawing.Size(41, 12);
this.lblApprovalOpinions.TabIndex = 18;
this.lblApprovalOpinions.Text = "审批意见";
this.txtApprovalOpinions.Location = new System.Drawing.Point(173,446);
this.txtApprovalOpinions.Name = "txtApprovalOpinions";
this.txtApprovalOpinions.Size = new System.Drawing.Size(100, 21);
this.txtApprovalOpinions.TabIndex = 18;
this.Controls.Add(this.lblApprovalOpinions);
this.Controls.Add(this.txtApprovalOpinions);

           //#####ApprovalStatus###SByte

           //#####ApprovalResults###Boolean
this.lblApprovalResults.AutoSize = true;
this.lblApprovalResults.Location = new System.Drawing.Point(100,500);
this.lblApprovalResults.Name = "lblApprovalResults";
this.lblApprovalResults.Size = new System.Drawing.Size(41, 12);
this.lblApprovalResults.TabIndex = 20;
this.lblApprovalResults.Text = "审批结果";
this.chkApprovalResults.Location = new System.Drawing.Point(173,496);
this.chkApprovalResults.Name = "chkApprovalResults";
this.chkApprovalResults.Size = new System.Drawing.Size(100, 21);
this.chkApprovalResults.TabIndex = 20;
this.Controls.Add(this.lblApprovalResults);
this.Controls.Add(this.chkApprovalResults);

           //#####200CloseCaseOpinions###String
this.lblCloseCaseOpinions.AutoSize = true;
this.lblCloseCaseOpinions.Location = new System.Drawing.Point(100,525);
this.lblCloseCaseOpinions.Name = "lblCloseCaseOpinions";
this.lblCloseCaseOpinions.Size = new System.Drawing.Size(41, 12);
this.lblCloseCaseOpinions.TabIndex = 21;
this.lblCloseCaseOpinions.Text = "审批意见";
this.txtCloseCaseOpinions.Location = new System.Drawing.Point(173,521);
this.txtCloseCaseOpinions.Name = "txtCloseCaseOpinions";
this.txtCloseCaseOpinions.Size = new System.Drawing.Size(100, 21);
this.txtCloseCaseOpinions.TabIndex = 21;
this.Controls.Add(this.lblCloseCaseOpinions);
this.Controls.Add(this.txtCloseCaseOpinions);

           //#####KeepAccountsType###Int32
//属性测试550KeepAccountsType
//属性测试550KeepAccountsType
//属性测试550KeepAccountsType
//属性测试550KeepAccountsType

           //#####ReceiptInvoiceClosed###Boolean
this.lblReceiptInvoiceClosed.AutoSize = true;
this.lblReceiptInvoiceClosed.Location = new System.Drawing.Point(100,575);
this.lblReceiptInvoiceClosed.Name = "lblReceiptInvoiceClosed";
this.lblReceiptInvoiceClosed.Size = new System.Drawing.Size(41, 12);
this.lblReceiptInvoiceClosed.TabIndex = 23;
this.lblReceiptInvoiceClosed.Text = "立帐结案";
this.chkReceiptInvoiceClosed.Location = new System.Drawing.Point(173,571);
this.chkReceiptInvoiceClosed.Name = "chkReceiptInvoiceClosed";
this.chkReceiptInvoiceClosed.Size = new System.Drawing.Size(100, 21);
this.chkReceiptInvoiceClosed.TabIndex = 23;
this.Controls.Add(this.lblReceiptInvoiceClosed);
this.Controls.Add(this.chkReceiptInvoiceClosed);

           //#####DataStatus###Int32
//属性测试600DataStatus
//属性测试600DataStatus
//属性测试600DataStatus
//属性测试600DataStatus

           //#####Approver_by###Int64
//属性测试625Approver_by
//属性测试625Approver_by
//属性测试625Approver_by
//属性测试625Approver_by

           //#####Approver_at###DateTime
this.lblApprover_at.AutoSize = true;
this.lblApprover_at.Location = new System.Drawing.Point(100,650);
this.lblApprover_at.Name = "lblApprover_at";
this.lblApprover_at.Size = new System.Drawing.Size(41, 12);
this.lblApprover_at.TabIndex = 26;
this.lblApprover_at.Text = "审批时间";
//111======650
this.dtpApprover_at.Location = new System.Drawing.Point(173,646);
this.dtpApprover_at.Name ="dtpApprover_at";
this.dtpApprover_at.ShowCheckBox =true;
this.dtpApprover_at.Size = new System.Drawing.Size(100, 21);
this.dtpApprover_at.TabIndex = 26;
this.Controls.Add(this.lblApprover_at);
this.Controls.Add(this.dtpApprover_at);

           //#####PrintStatus###Int32
//属性测试675PrintStatus
//属性测试675PrintStatus
//属性测试675PrintStatus
//属性测试675PrintStatus

           //#####GenerateVouchers###Boolean
this.lblGenerateVouchers.AutoSize = true;
this.lblGenerateVouchers.Location = new System.Drawing.Point(100,700);
this.lblGenerateVouchers.Name = "lblGenerateVouchers";
this.lblGenerateVouchers.Size = new System.Drawing.Size(41, 12);
this.lblGenerateVouchers.TabIndex = 28;
this.lblGenerateVouchers.Text = "生成凭证";
this.chkGenerateVouchers.Location = new System.Drawing.Point(173,696);
this.chkGenerateVouchers.Name = "chkGenerateVouchers";
this.chkGenerateVouchers.Size = new System.Drawing.Size(100, 21);
this.chkGenerateVouchers.TabIndex = 28;
this.Controls.Add(this.lblGenerateVouchers);
this.Controls.Add(this.chkGenerateVouchers);

           //#####VoucherID###Int64
//属性测试725VoucherID
//属性测试725VoucherID
//属性测试725VoucherID
//属性测试725VoucherID

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblReworkReturnNo );
this.Controls.Add(this.txtReworkReturnNo );

                this.Controls.Add(this.lblCustomerVendor_ID );
this.Controls.Add(this.cmbCustomerVendor_ID );

                this.Controls.Add(this.lblDepartmentID );
this.Controls.Add(this.cmbDepartmentID );

                this.Controls.Add(this.lblEmployee_ID );
this.Controls.Add(this.cmbEmployee_ID );

                this.Controls.Add(this.lblMOID );
this.Controls.Add(this.cmbMOID );

                
                this.Controls.Add(this.lblTotalReworkFee );
this.Controls.Add(this.txtTotalReworkFee );

                this.Controls.Add(this.lblTotalCost );
this.Controls.Add(this.txtTotalCost );

                this.Controls.Add(this.lblReturnDate );
this.Controls.Add(this.dtpReturnDate );

                this.Controls.Add(this.lblExpectedReturnDate );
this.Controls.Add(this.dtpExpectedReturnDate );

                this.Controls.Add(this.lblReasonForRework );
this.Controls.Add(this.txtReasonForRework );

                this.Controls.Add(this.lblisdeleted );
this.Controls.Add(this.chkisdeleted );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                
                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                
                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblApprovalOpinions );
this.Controls.Add(this.txtApprovalOpinions );

                
                this.Controls.Add(this.lblApprovalResults );
this.Controls.Add(this.chkApprovalResults );

                this.Controls.Add(this.lblCloseCaseOpinions );
this.Controls.Add(this.txtCloseCaseOpinions );

                
                this.Controls.Add(this.lblReceiptInvoiceClosed );
this.Controls.Add(this.chkReceiptInvoiceClosed );

                
                
                this.Controls.Add(this.lblApprover_at );
this.Controls.Add(this.dtpApprover_at );

                
                this.Controls.Add(this.lblGenerateVouchers );
this.Controls.Add(this.chkGenerateVouchers );

                
                    
            this.Name = "tb_MRP_ReworkReturnQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblReworkReturnNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtReworkReturnNo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCustomerVendor_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbCustomerVendor_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDepartmentID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbDepartmentID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblMOID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbMOID;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalReworkFee;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalReworkFee;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalCost;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblReturnDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpReturnDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblExpectedReturnDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpExpectedReturnDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblReasonForRework;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtReasonForRework;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblisdeleted;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkisdeleted;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModified_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprovalOpinions;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtApprovalOpinions;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprovalResults;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkApprovalResults;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCloseCaseOpinions;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCloseCaseOpinions;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblReceiptInvoiceClosed;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkReceiptInvoiceClosed;

    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprover_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpApprover_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblGenerateVouchers;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkGenerateVouchers;

    
        
              
    
    
   
 





    }
}


