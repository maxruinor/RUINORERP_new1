// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/08/2025 19:05:26
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 售后交付单
    /// </summary>
    partial class tb_AS_AfterSaleDeliveryEdit
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
            this.lblASDeliveryNo = new Krypton.Toolkit.KryptonLabel();
            this.txtASDeliveryNo = new Krypton.Toolkit.KryptonTextBox();
            this.lblASApplyID = new Krypton.Toolkit.KryptonLabel();
            this.cmbASApplyID = new Krypton.Toolkit.KryptonComboBox();
            this.lblASApplyNo = new Krypton.Toolkit.KryptonLabel();
            this.txtASApplyNo = new Krypton.Toolkit.KryptonTextBox();
            this.lblEmployee_ID = new Krypton.Toolkit.KryptonLabel();
            this.cmbEmployee_ID = new Krypton.Toolkit.KryptonComboBox();
            this.lblCustomerVendor_ID = new Krypton.Toolkit.KryptonLabel();
            this.cmbCustomerVendor_ID = new Krypton.Toolkit.KryptonComboBox();
            this.lblProjectGroup_ID = new Krypton.Toolkit.KryptonLabel();
            this.cmbProjectGroup_ID = new Krypton.Toolkit.KryptonComboBox();
            this.lblTotalDeliveryQty = new Krypton.Toolkit.KryptonLabel();
            this.txtTotalDeliveryQty = new Krypton.Toolkit.KryptonTextBox();
            this.lblDeliveryDate = new Krypton.Toolkit.KryptonLabel();
            this.dtpDeliveryDate = new Krypton.Toolkit.KryptonDateTimePicker();
            this.lblShippingAddress = new Krypton.Toolkit.KryptonLabel();
            this.txtShippingAddress = new Krypton.Toolkit.KryptonTextBox();
            this.lblShippingWay = new Krypton.Toolkit.KryptonLabel();
            this.txtShippingWay = new Krypton.Toolkit.KryptonTextBox();
            this.lblTrackNo = new Krypton.Toolkit.KryptonLabel();
            this.txtTrackNo = new Krypton.Toolkit.KryptonTextBox();
            this.lblDataStatus = new Krypton.Toolkit.KryptonLabel();
            this.txtDataStatus = new Krypton.Toolkit.KryptonTextBox();
            this.lblApprovalOpinions = new Krypton.Toolkit.KryptonLabel();
            this.txtApprovalOpinions = new Krypton.Toolkit.KryptonTextBox();
            this.lblApprover_by = new Krypton.Toolkit.KryptonLabel();
            this.txtApprover_by = new Krypton.Toolkit.KryptonTextBox();
            this.lblApprover_at = new Krypton.Toolkit.KryptonLabel();
            this.dtpApprover_at = new Krypton.Toolkit.KryptonDateTimePicker();
            this.lblApprovalResults = new Krypton.Toolkit.KryptonLabel();
            this.chkApprovalResults = new Krypton.Toolkit.KryptonCheckBox();
            this.lblPrintStatus = new Krypton.Toolkit.KryptonLabel();
            this.txtPrintStatus = new Krypton.Toolkit.KryptonTextBox();
            this.lblNotes = new Krypton.Toolkit.KryptonLabel();
            this.txtNotes = new Krypton.Toolkit.KryptonTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.cmbASApplyID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEmployee_ID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCustomerVendor_ID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbProjectGroup_ID)).BeginInit();
            this.SuspendLayout();
            // 
            // lblASDeliveryNo
            // 
            this.lblASDeliveryNo.Location = new System.Drawing.Point(330, 22);
            this.lblASDeliveryNo.Name = "lblASDeliveryNo";
            this.lblASDeliveryNo.Size = new System.Drawing.Size(88, 20);
            this.lblASDeliveryNo.TabIndex = 1;
            this.lblASDeliveryNo.Values.Text = "售后交付单号";
            // 
            // txtASDeliveryNo
            // 
            this.txtASDeliveryNo.Location = new System.Drawing.Point(403, 18);
            this.txtASDeliveryNo.Name = "txtASDeliveryNo";
            this.txtASDeliveryNo.Size = new System.Drawing.Size(100, 23);
            this.txtASDeliveryNo.TabIndex = 1;
            // 
            // lblASApplyID
            // 
            this.lblASApplyID.Location = new System.Drawing.Point(330, 47);
            this.lblASApplyID.Name = "lblASApplyID";
            this.lblASApplyID.Size = new System.Drawing.Size(75, 20);
            this.lblASApplyID.TabIndex = 2;
            this.lblASApplyID.Values.Text = "售后申请单";
            // 
            // cmbASApplyID
            // 
            this.cmbASApplyID.DropDownWidth = 100;
            this.cmbASApplyID.IntegralHeight = false;
            this.cmbASApplyID.Location = new System.Drawing.Point(403, 43);
            this.cmbASApplyID.Name = "cmbASApplyID";
            this.cmbASApplyID.Size = new System.Drawing.Size(100, 21);
            this.cmbASApplyID.TabIndex = 2;
            // 
            // lblASApplyNo
            // 
            this.lblASApplyNo.Location = new System.Drawing.Point(330, 72);
            this.lblASApplyNo.Name = "lblASApplyNo";
            this.lblASApplyNo.Size = new System.Drawing.Size(88, 20);
            this.lblASApplyNo.TabIndex = 3;
            this.lblASApplyNo.Values.Text = "售后申请编号";
            // 
            // txtASApplyNo
            // 
            this.txtASApplyNo.Location = new System.Drawing.Point(403, 68);
            this.txtASApplyNo.Name = "txtASApplyNo";
            this.txtASApplyNo.Size = new System.Drawing.Size(100, 23);
            this.txtASApplyNo.TabIndex = 3;
            // 
            // lblEmployee_ID
            // 
            this.lblEmployee_ID.Location = new System.Drawing.Point(330, 97);
            this.lblEmployee_ID.Name = "lblEmployee_ID";
            this.lblEmployee_ID.Size = new System.Drawing.Size(49, 20);
            this.lblEmployee_ID.TabIndex = 4;
            this.lblEmployee_ID.Values.Text = "业务员";
            // 
            // cmbEmployee_ID
            // 
            this.cmbEmployee_ID.DropDownWidth = 100;
            this.cmbEmployee_ID.IntegralHeight = false;
            this.cmbEmployee_ID.Location = new System.Drawing.Point(403, 93);
            this.cmbEmployee_ID.Name = "cmbEmployee_ID";
            this.cmbEmployee_ID.Size = new System.Drawing.Size(100, 21);
            this.cmbEmployee_ID.TabIndex = 4;
            // 
            // lblCustomerVendor_ID
            // 
            this.lblCustomerVendor_ID.Location = new System.Drawing.Point(330, 122);
            this.lblCustomerVendor_ID.Name = "lblCustomerVendor_ID";
            this.lblCustomerVendor_ID.Size = new System.Drawing.Size(36, 20);
            this.lblCustomerVendor_ID.TabIndex = 5;
            this.lblCustomerVendor_ID.Values.Text = "客户";
            // 
            // cmbCustomerVendor_ID
            // 
            this.cmbCustomerVendor_ID.DropDownWidth = 100;
            this.cmbCustomerVendor_ID.IntegralHeight = false;
            this.cmbCustomerVendor_ID.Location = new System.Drawing.Point(403, 118);
            this.cmbCustomerVendor_ID.Name = "cmbCustomerVendor_ID";
            this.cmbCustomerVendor_ID.Size = new System.Drawing.Size(100, 21);
            this.cmbCustomerVendor_ID.TabIndex = 5;
            // 
            // lblProjectGroup_ID
            // 
            this.lblProjectGroup_ID.Location = new System.Drawing.Point(330, 147);
            this.lblProjectGroup_ID.Name = "lblProjectGroup_ID";
            this.lblProjectGroup_ID.Size = new System.Drawing.Size(49, 20);
            this.lblProjectGroup_ID.TabIndex = 6;
            this.lblProjectGroup_ID.Values.Text = "项目组";
            // 
            // cmbProjectGroup_ID
            // 
            this.cmbProjectGroup_ID.DropDownWidth = 100;
            this.cmbProjectGroup_ID.IntegralHeight = false;
            this.cmbProjectGroup_ID.Location = new System.Drawing.Point(403, 143);
            this.cmbProjectGroup_ID.Name = "cmbProjectGroup_ID";
            this.cmbProjectGroup_ID.Size = new System.Drawing.Size(100, 21);
            this.cmbProjectGroup_ID.TabIndex = 6;
            // 
            // lblTotalDeliveryQty
            // 
            this.lblTotalDeliveryQty.Location = new System.Drawing.Point(330, 172);
            this.lblTotalDeliveryQty.Name = "lblTotalDeliveryQty";
            this.lblTotalDeliveryQty.Size = new System.Drawing.Size(75, 20);
            this.lblTotalDeliveryQty.TabIndex = 7;
            this.lblTotalDeliveryQty.Values.Text = "总交付数量";
            // 
            // txtTotalDeliveryQty
            // 
            this.txtTotalDeliveryQty.Location = new System.Drawing.Point(403, 168);
            this.txtTotalDeliveryQty.Name = "txtTotalDeliveryQty";
            this.txtTotalDeliveryQty.Size = new System.Drawing.Size(100, 23);
            this.txtTotalDeliveryQty.TabIndex = 7;
            // 
            // lblDeliveryDate
            // 
            this.lblDeliveryDate.Location = new System.Drawing.Point(330, 197);
            this.lblDeliveryDate.Name = "lblDeliveryDate";
            this.lblDeliveryDate.Size = new System.Drawing.Size(62, 20);
            this.lblDeliveryDate.TabIndex = 8;
            this.lblDeliveryDate.Values.Text = "出库日期";
            // 
            // dtpDeliveryDate
            // 
            this.dtpDeliveryDate.Location = new System.Drawing.Point(403, 193);
            this.dtpDeliveryDate.Name = "dtpDeliveryDate";
            this.dtpDeliveryDate.ShowCheckBox = true;
            this.dtpDeliveryDate.Size = new System.Drawing.Size(100, 21);
            this.dtpDeliveryDate.TabIndex = 8;
            // 
            // lblShippingAddress
            // 
            this.lblShippingAddress.Location = new System.Drawing.Point(330, 222);
            this.lblShippingAddress.Name = "lblShippingAddress";
            this.lblShippingAddress.Size = new System.Drawing.Size(62, 20);
            this.lblShippingAddress.TabIndex = 9;
            this.lblShippingAddress.Values.Text = "收货地址";
            // 
            // txtShippingAddress
            // 
            this.txtShippingAddress.Location = new System.Drawing.Point(403, 218);
            this.txtShippingAddress.Multiline = true;
            this.txtShippingAddress.Name = "txtShippingAddress";
            this.txtShippingAddress.Size = new System.Drawing.Size(100, 21);
            this.txtShippingAddress.TabIndex = 9;
            // 
            // lblShippingWay
            // 
            this.lblShippingWay.Location = new System.Drawing.Point(330, 247);
            this.lblShippingWay.Name = "lblShippingWay";
            this.lblShippingWay.Size = new System.Drawing.Size(62, 20);
            this.lblShippingWay.TabIndex = 10;
            this.lblShippingWay.Values.Text = "发货方式";
            // 
            // txtShippingWay
            // 
            this.txtShippingWay.Location = new System.Drawing.Point(403, 243);
            this.txtShippingWay.Name = "txtShippingWay";
            this.txtShippingWay.Size = new System.Drawing.Size(100, 23);
            this.txtShippingWay.TabIndex = 10;
            // 
            // lblTrackNo
            // 
            this.lblTrackNo.Location = new System.Drawing.Point(98, 75);
            this.lblTrackNo.Name = "lblTrackNo";
            this.lblTrackNo.Size = new System.Drawing.Size(62, 20);
            this.lblTrackNo.TabIndex = 11;
            this.lblTrackNo.Values.Text = "物流单号";
            // 
            // txtTrackNo
            // 
            this.txtTrackNo.Location = new System.Drawing.Point(171, 71);
            this.txtTrackNo.Name = "txtTrackNo";
            this.txtTrackNo.Size = new System.Drawing.Size(100, 23);
            this.txtTrackNo.TabIndex = 11;
            // 
            // lblDataStatus
            // 
            this.lblDataStatus.Location = new System.Drawing.Point(98, 225);
            this.lblDataStatus.Name = "lblDataStatus";
            this.lblDataStatus.Size = new System.Drawing.Size(62, 20);
            this.lblDataStatus.TabIndex = 17;
            this.lblDataStatus.Values.Text = "数据状态";
            // 
            // txtDataStatus
            // 
            this.txtDataStatus.Location = new System.Drawing.Point(171, 221);
            this.txtDataStatus.Name = "txtDataStatus";
            this.txtDataStatus.Size = new System.Drawing.Size(100, 23);
            this.txtDataStatus.TabIndex = 17;
            // 
            // lblApprovalOpinions
            // 
            this.lblApprovalOpinions.Location = new System.Drawing.Point(98, 250);
            this.lblApprovalOpinions.Name = "lblApprovalOpinions";
            this.lblApprovalOpinions.Size = new System.Drawing.Size(62, 20);
            this.lblApprovalOpinions.TabIndex = 18;
            this.lblApprovalOpinions.Values.Text = "审批意见";
            // 
            // txtApprovalOpinions
            // 
            this.txtApprovalOpinions.Location = new System.Drawing.Point(171, 246);
            this.txtApprovalOpinions.Name = "txtApprovalOpinions";
            this.txtApprovalOpinions.Size = new System.Drawing.Size(100, 23);
            this.txtApprovalOpinions.TabIndex = 18;
            // 
            // lblApprover_by
            // 
            this.lblApprover_by.Location = new System.Drawing.Point(98, 275);
            this.lblApprover_by.Name = "lblApprover_by";
            this.lblApprover_by.Size = new System.Drawing.Size(49, 20);
            this.lblApprover_by.TabIndex = 19;
            this.lblApprover_by.Values.Text = "审批人";
            // 
            // txtApprover_by
            // 
            this.txtApprover_by.Location = new System.Drawing.Point(171, 271);
            this.txtApprover_by.Name = "txtApprover_by";
            this.txtApprover_by.Size = new System.Drawing.Size(100, 23);
            this.txtApprover_by.TabIndex = 19;
            // 
            // lblApprover_at
            // 
            this.lblApprover_at.Location = new System.Drawing.Point(98, 300);
            this.lblApprover_at.Name = "lblApprover_at";
            this.lblApprover_at.Size = new System.Drawing.Size(62, 20);
            this.lblApprover_at.TabIndex = 20;
            this.lblApprover_at.Values.Text = "审批时间";
            // 
            // dtpApprover_at
            // 
            this.dtpApprover_at.Location = new System.Drawing.Point(171, 296);
            this.dtpApprover_at.Name = "dtpApprover_at";
            this.dtpApprover_at.ShowCheckBox = true;
            this.dtpApprover_at.Size = new System.Drawing.Size(100, 21);
            this.dtpApprover_at.TabIndex = 20;
            // 
            // lblApprovalResults
            // 
            this.lblApprovalResults.Location = new System.Drawing.Point(98, 350);
            this.lblApprovalResults.Name = "lblApprovalResults";
            this.lblApprovalResults.Size = new System.Drawing.Size(62, 20);
            this.lblApprovalResults.TabIndex = 22;
            this.lblApprovalResults.Values.Text = "审批结果";
            // 
            // chkApprovalResults
            // 
            this.chkApprovalResults.Location = new System.Drawing.Point(171, 346);
            this.chkApprovalResults.Name = "chkApprovalResults";
            this.chkApprovalResults.Size = new System.Drawing.Size(19, 13);
            this.chkApprovalResults.TabIndex = 22;
            this.chkApprovalResults.Values.Text = "";
            // 
            // lblPrintStatus
            // 
            this.lblPrintStatus.Location = new System.Drawing.Point(98, 375);
            this.lblPrintStatus.Name = "lblPrintStatus";
            this.lblPrintStatus.Size = new System.Drawing.Size(62, 20);
            this.lblPrintStatus.TabIndex = 23;
            this.lblPrintStatus.Values.Text = "打印状态";
            // 
            // txtPrintStatus
            // 
            this.txtPrintStatus.Location = new System.Drawing.Point(171, 371);
            this.txtPrintStatus.Name = "txtPrintStatus";
            this.txtPrintStatus.Size = new System.Drawing.Size(100, 23);
            this.txtPrintStatus.TabIndex = 23;
            // 
            // lblNotes
            // 
            this.lblNotes.Location = new System.Drawing.Point(98, 400);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(36, 20);
            this.lblNotes.TabIndex = 24;
            this.lblNotes.Values.Text = "备注";
            // 
            // txtNotes
            // 
            this.txtNotes.Location = new System.Drawing.Point(171, 396);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(100, 21);
            this.txtNotes.TabIndex = 24;
            // 
            // tb_AS_AfterSaleDeliveryEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblASDeliveryNo);
            this.Controls.Add(this.txtASDeliveryNo);
            this.Controls.Add(this.lblASApplyID);
            this.Controls.Add(this.cmbASApplyID);
            this.Controls.Add(this.lblASApplyNo);
            this.Controls.Add(this.txtASApplyNo);
            this.Controls.Add(this.lblEmployee_ID);
            this.Controls.Add(this.cmbEmployee_ID);
            this.Controls.Add(this.lblCustomerVendor_ID);
            this.Controls.Add(this.cmbCustomerVendor_ID);
            this.Controls.Add(this.lblProjectGroup_ID);
            this.Controls.Add(this.cmbProjectGroup_ID);
            this.Controls.Add(this.lblTotalDeliveryQty);
            this.Controls.Add(this.txtTotalDeliveryQty);
            this.Controls.Add(this.lblDeliveryDate);
            this.Controls.Add(this.dtpDeliveryDate);
            this.Controls.Add(this.lblShippingAddress);
            this.Controls.Add(this.txtShippingAddress);
            this.Controls.Add(this.lblShippingWay);
            this.Controls.Add(this.txtShippingWay);
            this.Controls.Add(this.lblTrackNo);
            this.Controls.Add(this.txtTrackNo);
            this.Controls.Add(this.lblDataStatus);
            this.Controls.Add(this.txtDataStatus);
            this.Controls.Add(this.lblApprovalOpinions);
            this.Controls.Add(this.txtApprovalOpinions);
            this.Controls.Add(this.lblApprover_by);
            this.Controls.Add(this.txtApprover_by);
            this.Controls.Add(this.lblApprover_at);
            this.Controls.Add(this.dtpApprover_at);
            this.Controls.Add(this.lblApprovalResults);
            this.Controls.Add(this.chkApprovalResults);
            this.Controls.Add(this.lblPrintStatus);
            this.Controls.Add(this.txtPrintStatus);
            this.Controls.Add(this.lblNotes);
            this.Controls.Add(this.txtNotes);
            this.Name = "tb_AS_AfterSaleDeliveryEdit";
            this.Size = new System.Drawing.Size(911, 490);
            ((System.ComponentModel.ISupportInitialize)(this.cmbASApplyID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEmployee_ID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbCustomerVendor_ID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbProjectGroup_ID)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblASDeliveryNo;
private Krypton.Toolkit.KryptonTextBox txtASDeliveryNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblASApplyID;
private Krypton.Toolkit.KryptonComboBox cmbASApplyID;

    
        
              private Krypton.Toolkit.KryptonLabel lblASApplyNo;
private Krypton.Toolkit.KryptonTextBox txtASApplyNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblCustomerVendor_ID;
private Krypton.Toolkit.KryptonComboBox cmbCustomerVendor_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblProjectGroup_ID;
private Krypton.Toolkit.KryptonComboBox cmbProjectGroup_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblTotalDeliveryQty;
private Krypton.Toolkit.KryptonTextBox txtTotalDeliveryQty;

    
        
              private Krypton.Toolkit.KryptonLabel lblDeliveryDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpDeliveryDate;

    
        
              private Krypton.Toolkit.KryptonLabel lblShippingAddress;
private Krypton.Toolkit.KryptonTextBox txtShippingAddress;

    
        
              private Krypton.Toolkit.KryptonLabel lblShippingWay;
private Krypton.Toolkit.KryptonTextBox txtShippingWay;

    
        
              private Krypton.Toolkit.KryptonLabel lblTrackNo;
private Krypton.Toolkit.KryptonTextBox txtTrackNo;






    
        
              private Krypton.Toolkit.KryptonLabel lblDataStatus;
private Krypton.Toolkit.KryptonTextBox txtDataStatus;

    
        
              private Krypton.Toolkit.KryptonLabel lblApprovalOpinions;
private Krypton.Toolkit.KryptonTextBox txtApprovalOpinions;

    
        
              private Krypton.Toolkit.KryptonLabel lblApprover_by;
private Krypton.Toolkit.KryptonTextBox txtApprover_by;

    
        
              private Krypton.Toolkit.KryptonLabel lblApprover_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpApprover_at;

    
        
              
    
        
              private Krypton.Toolkit.KryptonLabel lblApprovalResults;
private Krypton.Toolkit.KryptonCheckBox chkApprovalResults;

    
        
              private Krypton.Toolkit.KryptonLabel lblPrintStatus;
private Krypton.Toolkit.KryptonTextBox txtPrintStatus;

    
        
              private Krypton.Toolkit.KryptonLabel lblNotes;
private Krypton.Toolkit.KryptonTextBox txtNotes;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

