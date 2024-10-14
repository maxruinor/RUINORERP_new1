// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/14/2024 18:29:33
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 调拨单-两个仓库之间的库存转移
    /// </summary>
    partial class tb_StockTransferEdit
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
     Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。this.lblLocation_ID_from = new Krypton.Toolkit.KryptonLabel();
this.txtLocation_ID_from = new Krypton.Toolkit.KryptonTextBox();

Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。this.lblLocation_ID_to = new Krypton.Toolkit.KryptonLabel();
this.txtLocation_ID_to = new Krypton.Toolkit.KryptonTextBox();

Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。this.lblEmployee_ID = new Krypton.Toolkit.KryptonLabel();
this.txtEmployee_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblStockTransferNo = new Krypton.Toolkit.KryptonLabel();
this.txtStockTransferNo = new Krypton.Toolkit.KryptonTextBox();

Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。this.lblTotalQty = new Krypton.Toolkit.KryptonLabel();
this.txtTotalQty = new Krypton.Toolkit.KryptonTextBox();

this.lblTotalCost = new Krypton.Toolkit.KryptonLabel();
this.txtTotalCost = new Krypton.Toolkit.KryptonTextBox();

this.lblTotalTransferAmount = new Krypton.Toolkit.KryptonLabel();
this.txtTotalTransferAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblBill_Date = new Krypton.Toolkit.KryptonLabel();
this.dtpBill_Date = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblOut_date = new Krypton.Toolkit.KryptonLabel();
this.dtpOut_date = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblCreated_at = new Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new Krypton.Toolkit.KryptonDateTimePicker();

Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。this.lblCreated_by = new Krypton.Toolkit.KryptonLabel();
this.txtCreated_by = new Krypton.Toolkit.KryptonTextBox();

this.lblModified_at = new Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new Krypton.Toolkit.KryptonDateTimePicker();

Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。this.lblModified_by = new Krypton.Toolkit.KryptonLabel();
this.txtModified_by = new Krypton.Toolkit.KryptonTextBox();

this.lblNotes = new Krypton.Toolkit.KryptonLabel();
this.txtNotes = new Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

this.lblisdeleted = new Krypton.Toolkit.KryptonLabel();
this.chkisdeleted = new Krypton.Toolkit.KryptonCheckBox();
this.chkisdeleted.Values.Text ="";

Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。this.lblDataStatus = new Krypton.Toolkit.KryptonLabel();
this.txtDataStatus = new Krypton.Toolkit.KryptonTextBox();

this.lblApprovalOpinions = new Krypton.Toolkit.KryptonLabel();
this.txtApprovalOpinions = new Krypton.Toolkit.KryptonTextBox();
this.txtApprovalOpinions.Multiline = true;

Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。this.lblApprover_by = new Krypton.Toolkit.KryptonLabel();
this.txtApprover_by = new Krypton.Toolkit.KryptonTextBox();

this.lblApprover_at = new Krypton.Toolkit.KryptonLabel();
this.dtpApprover_at = new Krypton.Toolkit.KryptonDateTimePicker();


this.lblApprovalResults = new Krypton.Toolkit.KryptonLabel();
this.chkApprovalResults = new Krypton.Toolkit.KryptonCheckBox();
this.chkApprovalResults.Values.Text ="";

Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。this.lblPrintStatus = new Krypton.Toolkit.KryptonLabel();
this.txtPrintStatus = new Krypton.Toolkit.KryptonTextBox();

    
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
     
            //#####Location_ID_from###Int64
//属性测试25Location_ID_from
Location_ID_from主外字段不一致。//属性测试25Location_ID_from
Location_ID_to主外字段不一致。this.lblLocation_ID_from.AutoSize = true;
this.lblLocation_ID_from.Location = new System.Drawing.Point(100,25);
this.lblLocation_ID_from.Name = "lblLocation_ID_from";
this.lblLocation_ID_from.Size = new System.Drawing.Size(41, 12);
this.lblLocation_ID_from.TabIndex = 1;
this.lblLocation_ID_from.Text = "";
this.txtLocation_ID_from.Location = new System.Drawing.Point(173,21);
this.txtLocation_ID_from.Name = "txtLocation_ID_from";
this.txtLocation_ID_from.Size = new System.Drawing.Size(100, 21);
this.txtLocation_ID_from.TabIndex = 1;
this.Controls.Add(this.lblLocation_ID_from);
this.Controls.Add(this.txtLocation_ID_from);

           //#####Location_ID_to###Int64
//属性测试50Location_ID_to
Location_ID_from主外字段不一致。//属性测试50Location_ID_to
Location_ID_to主外字段不一致。this.lblLocation_ID_to.AutoSize = true;
this.lblLocation_ID_to.Location = new System.Drawing.Point(100,50);
this.lblLocation_ID_to.Name = "lblLocation_ID_to";
this.lblLocation_ID_to.Size = new System.Drawing.Size(41, 12);
this.lblLocation_ID_to.TabIndex = 2;
this.lblLocation_ID_to.Text = "";
this.txtLocation_ID_to.Location = new System.Drawing.Point(173,46);
this.txtLocation_ID_to.Name = "txtLocation_ID_to";
this.txtLocation_ID_to.Size = new System.Drawing.Size(100, 21);
this.txtLocation_ID_to.TabIndex = 2;
this.Controls.Add(this.lblLocation_ID_to);
this.Controls.Add(this.txtLocation_ID_to);

           //#####Employee_ID###Int64
//属性测试75Employee_ID
Location_ID_from主外字段不一致。//属性测试75Employee_ID
Location_ID_to主外字段不一致。this.lblEmployee_ID.AutoSize = true;
this.lblEmployee_ID.Location = new System.Drawing.Point(100,75);
this.lblEmployee_ID.Name = "lblEmployee_ID";
this.lblEmployee_ID.Size = new System.Drawing.Size(41, 12);
this.lblEmployee_ID.TabIndex = 3;
this.lblEmployee_ID.Text = "经办人";
this.txtEmployee_ID.Location = new System.Drawing.Point(173,71);
this.txtEmployee_ID.Name = "txtEmployee_ID";
this.txtEmployee_ID.Size = new System.Drawing.Size(100, 21);
this.txtEmployee_ID.TabIndex = 3;
this.Controls.Add(this.lblEmployee_ID);
this.Controls.Add(this.txtEmployee_ID);

           //#####50StockTransferNo###String
this.lblStockTransferNo.AutoSize = true;
this.lblStockTransferNo.Location = new System.Drawing.Point(100,100);
this.lblStockTransferNo.Name = "lblStockTransferNo";
this.lblStockTransferNo.Size = new System.Drawing.Size(41, 12);
this.lblStockTransferNo.TabIndex = 4;
this.lblStockTransferNo.Text = "调拨单号";
this.txtStockTransferNo.Location = new System.Drawing.Point(173,96);
this.txtStockTransferNo.Name = "txtStockTransferNo";
this.txtStockTransferNo.Size = new System.Drawing.Size(100, 21);
this.txtStockTransferNo.TabIndex = 4;
this.Controls.Add(this.lblStockTransferNo);
this.Controls.Add(this.txtStockTransferNo);

           //#####TotalQty###Int32
//属性测试125TotalQty
Location_ID_from主外字段不一致。//属性测试125TotalQty
Location_ID_to主外字段不一致。this.lblTotalQty.AutoSize = true;
this.lblTotalQty.Location = new System.Drawing.Point(100,125);
this.lblTotalQty.Name = "lblTotalQty";
this.lblTotalQty.Size = new System.Drawing.Size(41, 12);
this.lblTotalQty.TabIndex = 5;
this.lblTotalQty.Text = "总数量";
this.txtTotalQty.Location = new System.Drawing.Point(173,121);
this.txtTotalQty.Name = "txtTotalQty";
this.txtTotalQty.Size = new System.Drawing.Size(100, 21);
this.txtTotalQty.TabIndex = 5;
this.Controls.Add(this.lblTotalQty);
this.Controls.Add(this.txtTotalQty);

           //#####TotalCost###Decimal
this.lblTotalCost.AutoSize = true;
this.lblTotalCost.Location = new System.Drawing.Point(100,150);
this.lblTotalCost.Name = "lblTotalCost";
this.lblTotalCost.Size = new System.Drawing.Size(41, 12);
this.lblTotalCost.TabIndex = 6;
this.lblTotalCost.Text = "总成本";
//111======150
this.txtTotalCost.Location = new System.Drawing.Point(173,146);
this.txtTotalCost.Name ="txtTotalCost";
this.txtTotalCost.Size = new System.Drawing.Size(100, 21);
this.txtTotalCost.TabIndex = 6;
this.Controls.Add(this.lblTotalCost);
this.Controls.Add(this.txtTotalCost);

           //#####TotalTransferAmount###Decimal
this.lblTotalTransferAmount.AutoSize = true;
this.lblTotalTransferAmount.Location = new System.Drawing.Point(100,175);
this.lblTotalTransferAmount.Name = "lblTotalTransferAmount";
this.lblTotalTransferAmount.Size = new System.Drawing.Size(41, 12);
this.lblTotalTransferAmount.TabIndex = 7;
this.lblTotalTransferAmount.Text = "调拨金额";
//111======175
this.txtTotalTransferAmount.Location = new System.Drawing.Point(173,171);
this.txtTotalTransferAmount.Name ="txtTotalTransferAmount";
this.txtTotalTransferAmount.Size = new System.Drawing.Size(100, 21);
this.txtTotalTransferAmount.TabIndex = 7;
this.Controls.Add(this.lblTotalTransferAmount);
this.Controls.Add(this.txtTotalTransferAmount);

           //#####Bill_Date###DateTime
this.lblBill_Date.AutoSize = true;
this.lblBill_Date.Location = new System.Drawing.Point(100,200);
this.lblBill_Date.Name = "lblBill_Date";
this.lblBill_Date.Size = new System.Drawing.Size(41, 12);
this.lblBill_Date.TabIndex = 8;
this.lblBill_Date.Text = "单据日期";
//111======200
this.dtpBill_Date.Location = new System.Drawing.Point(173,196);
this.dtpBill_Date.Name ="dtpBill_Date";
this.dtpBill_Date.ShowCheckBox =true;
this.dtpBill_Date.Size = new System.Drawing.Size(100, 21);
this.dtpBill_Date.TabIndex = 8;
this.Controls.Add(this.lblBill_Date);
this.Controls.Add(this.dtpBill_Date);

           //#####Out_date###DateTime
this.lblOut_date.AutoSize = true;
this.lblOut_date.Location = new System.Drawing.Point(100,225);
this.lblOut_date.Name = "lblOut_date";
this.lblOut_date.Size = new System.Drawing.Size(41, 12);
this.lblOut_date.TabIndex = 9;
this.lblOut_date.Text = "出库日期";
//111======225
this.dtpOut_date.Location = new System.Drawing.Point(173,221);
this.dtpOut_date.Name ="dtpOut_date";
this.dtpOut_date.ShowCheckBox =true;
this.dtpOut_date.Size = new System.Drawing.Size(100, 21);
this.dtpOut_date.TabIndex = 9;
this.Controls.Add(this.lblOut_date);
this.Controls.Add(this.dtpOut_date);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,250);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 10;
this.lblCreated_at.Text = "创建时间";
//111======250
this.dtpCreated_at.Location = new System.Drawing.Point(173,246);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 10;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
//属性测试275Created_by
Location_ID_from主外字段不一致。//属性测试275Created_by
Location_ID_to主外字段不一致。this.lblCreated_by.AutoSize = true;
this.lblCreated_by.Location = new System.Drawing.Point(100,275);
this.lblCreated_by.Name = "lblCreated_by";
this.lblCreated_by.Size = new System.Drawing.Size(41, 12);
this.lblCreated_by.TabIndex = 11;
this.lblCreated_by.Text = "创建人";
this.txtCreated_by.Location = new System.Drawing.Point(173,271);
this.txtCreated_by.Name = "txtCreated_by";
this.txtCreated_by.Size = new System.Drawing.Size(100, 21);
this.txtCreated_by.TabIndex = 11;
this.Controls.Add(this.lblCreated_by);
this.Controls.Add(this.txtCreated_by);

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,300);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 12;
this.lblModified_at.Text = "修改时间";
//111======300
this.dtpModified_at.Location = new System.Drawing.Point(173,296);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 12;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
//属性测试325Modified_by
Location_ID_from主外字段不一致。//属性测试325Modified_by
Location_ID_to主外字段不一致。this.lblModified_by.AutoSize = true;
this.lblModified_by.Location = new System.Drawing.Point(100,325);
this.lblModified_by.Name = "lblModified_by";
this.lblModified_by.Size = new System.Drawing.Size(41, 12);
this.lblModified_by.TabIndex = 13;
this.lblModified_by.Text = "修改人";
this.txtModified_by.Location = new System.Drawing.Point(173,321);
this.txtModified_by.Name = "txtModified_by";
this.txtModified_by.Size = new System.Drawing.Size(100, 21);
this.txtModified_by.TabIndex = 13;
this.Controls.Add(this.lblModified_by);
this.Controls.Add(this.txtModified_by);

           //#####1500Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,350);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 14;
this.lblNotes.Text = "备注";
this.txtNotes.Location = new System.Drawing.Point(173,346);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 14;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,375);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 15;
this.lblisdeleted.Text = "逻辑删除";
this.chkisdeleted.Location = new System.Drawing.Point(173,371);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 15;
this.Controls.Add(this.lblisdeleted);
this.Controls.Add(this.chkisdeleted);

           //#####DataStatus###Int32
//属性测试400DataStatus
Location_ID_from主外字段不一致。//属性测试400DataStatus
Location_ID_to主外字段不一致。this.lblDataStatus.AutoSize = true;
this.lblDataStatus.Location = new System.Drawing.Point(100,400);
this.lblDataStatus.Name = "lblDataStatus";
this.lblDataStatus.Size = new System.Drawing.Size(41, 12);
this.lblDataStatus.TabIndex = 16;
this.lblDataStatus.Text = "数据状态";
this.txtDataStatus.Location = new System.Drawing.Point(173,396);
this.txtDataStatus.Name = "txtDataStatus";
this.txtDataStatus.Size = new System.Drawing.Size(100, 21);
this.txtDataStatus.TabIndex = 16;
this.Controls.Add(this.lblDataStatus);
this.Controls.Add(this.txtDataStatus);

           //#####500ApprovalOpinions###String
this.lblApprovalOpinions.AutoSize = true;
this.lblApprovalOpinions.Location = new System.Drawing.Point(100,425);
this.lblApprovalOpinions.Name = "lblApprovalOpinions";
this.lblApprovalOpinions.Size = new System.Drawing.Size(41, 12);
this.lblApprovalOpinions.TabIndex = 17;
this.lblApprovalOpinions.Text = "审批意见";
this.txtApprovalOpinions.Location = new System.Drawing.Point(173,421);
this.txtApprovalOpinions.Name = "txtApprovalOpinions";
this.txtApprovalOpinions.Size = new System.Drawing.Size(100, 21);
this.txtApprovalOpinions.TabIndex = 17;
this.Controls.Add(this.lblApprovalOpinions);
this.Controls.Add(this.txtApprovalOpinions);

           //#####Approver_by###Int64
//属性测试450Approver_by
Location_ID_from主外字段不一致。//属性测试450Approver_by
Location_ID_to主外字段不一致。this.lblApprover_by.AutoSize = true;
this.lblApprover_by.Location = new System.Drawing.Point(100,450);
this.lblApprover_by.Name = "lblApprover_by";
this.lblApprover_by.Size = new System.Drawing.Size(41, 12);
this.lblApprover_by.TabIndex = 18;
this.lblApprover_by.Text = "审批人";
this.txtApprover_by.Location = new System.Drawing.Point(173,446);
this.txtApprover_by.Name = "txtApprover_by";
this.txtApprover_by.Size = new System.Drawing.Size(100, 21);
this.txtApprover_by.TabIndex = 18;
this.Controls.Add(this.lblApprover_by);
this.Controls.Add(this.txtApprover_by);

           //#####Approver_at###DateTime
this.lblApprover_at.AutoSize = true;
this.lblApprover_at.Location = new System.Drawing.Point(100,475);
this.lblApprover_at.Name = "lblApprover_at";
this.lblApprover_at.Size = new System.Drawing.Size(41, 12);
this.lblApprover_at.TabIndex = 19;
this.lblApprover_at.Text = "审批时间";
//111======475
this.dtpApprover_at.Location = new System.Drawing.Point(173,471);
this.dtpApprover_at.Name ="dtpApprover_at";
this.dtpApprover_at.ShowCheckBox =true;
this.dtpApprover_at.Size = new System.Drawing.Size(100, 21);
this.dtpApprover_at.TabIndex = 19;
this.Controls.Add(this.lblApprover_at);
this.Controls.Add(this.dtpApprover_at);

           //#####ApprovalStatus###SByte

           //#####ApprovalResults###Boolean
this.lblApprovalResults.AutoSize = true;
this.lblApprovalResults.Location = new System.Drawing.Point(100,525);
this.lblApprovalResults.Name = "lblApprovalResults";
this.lblApprovalResults.Size = new System.Drawing.Size(41, 12);
this.lblApprovalResults.TabIndex = 21;
this.lblApprovalResults.Text = "审批结果";
this.chkApprovalResults.Location = new System.Drawing.Point(173,521);
this.chkApprovalResults.Name = "chkApprovalResults";
this.chkApprovalResults.Size = new System.Drawing.Size(100, 21);
this.chkApprovalResults.TabIndex = 21;
this.Controls.Add(this.lblApprovalResults);
this.Controls.Add(this.chkApprovalResults);

           //#####PrintStatus###Int32
//属性测试550PrintStatus
Location_ID_from主外字段不一致。//属性测试550PrintStatus
Location_ID_to主外字段不一致。this.lblPrintStatus.AutoSize = true;
this.lblPrintStatus.Location = new System.Drawing.Point(100,550);
this.lblPrintStatus.Name = "lblPrintStatus";
this.lblPrintStatus.Size = new System.Drawing.Size(41, 12);
this.lblPrintStatus.TabIndex = 22;
this.lblPrintStatus.Text = "打印状态";
this.txtPrintStatus.Location = new System.Drawing.Point(173,546);
this.txtPrintStatus.Name = "txtPrintStatus";
this.txtPrintStatus.Size = new System.Drawing.Size(100, 21);
this.txtPrintStatus.TabIndex = 22;
this.Controls.Add(this.lblPrintStatus);
this.Controls.Add(this.txtPrintStatus);

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
           // this.kryptonPanel1.TabIndex = 22;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。this.Controls.Add(this.lblLocation_ID_from );
this.Controls.Add(this.txtLocation_ID_from );

                Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。this.Controls.Add(this.lblLocation_ID_to );
this.Controls.Add(this.txtLocation_ID_to );

                Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。this.Controls.Add(this.lblEmployee_ID );
this.Controls.Add(this.txtEmployee_ID );

                this.Controls.Add(this.lblStockTransferNo );
this.Controls.Add(this.txtStockTransferNo );

                Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。this.Controls.Add(this.lblTotalQty );
this.Controls.Add(this.txtTotalQty );

                this.Controls.Add(this.lblTotalCost );
this.Controls.Add(this.txtTotalCost );

                this.Controls.Add(this.lblTotalTransferAmount );
this.Controls.Add(this.txtTotalTransferAmount );

                this.Controls.Add(this.lblBill_Date );
this.Controls.Add(this.dtpBill_Date );

                this.Controls.Add(this.lblOut_date );
this.Controls.Add(this.dtpOut_date );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。this.Controls.Add(this.lblCreated_by );
this.Controls.Add(this.txtCreated_by );

                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。this.Controls.Add(this.lblModified_by );
this.Controls.Add(this.txtModified_by );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblisdeleted );
this.Controls.Add(this.chkisdeleted );

                Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。this.Controls.Add(this.lblDataStatus );
this.Controls.Add(this.txtDataStatus );

                this.Controls.Add(this.lblApprovalOpinions );
this.Controls.Add(this.txtApprovalOpinions );

                Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。this.Controls.Add(this.lblApprover_by );
this.Controls.Add(this.txtApprover_by );

                this.Controls.Add(this.lblApprover_at );
this.Controls.Add(this.dtpApprover_at );

                
                this.Controls.Add(this.lblApprovalResults );
this.Controls.Add(this.chkApprovalResults );

                Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。this.Controls.Add(this.lblPrintStatus );
this.Controls.Add(this.txtPrintStatus );

                            // 
            // "tb_StockTransferEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_StockTransferEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。private Krypton.Toolkit.KryptonLabel lblLocation_ID_from;
private Krypton.Toolkit.KryptonTextBox txtLocation_ID_from;

    
        
              Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。private Krypton.Toolkit.KryptonLabel lblLocation_ID_to;
private Krypton.Toolkit.KryptonTextBox txtLocation_ID_to;

    
        
              Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。private Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private Krypton.Toolkit.KryptonTextBox txtEmployee_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblStockTransferNo;
private Krypton.Toolkit.KryptonTextBox txtStockTransferNo;

    
        
              Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。private Krypton.Toolkit.KryptonLabel lblTotalQty;
private Krypton.Toolkit.KryptonTextBox txtTotalQty;

    
        
              private Krypton.Toolkit.KryptonLabel lblTotalCost;
private Krypton.Toolkit.KryptonTextBox txtTotalCost;

    
        
              private Krypton.Toolkit.KryptonLabel lblTotalTransferAmount;
private Krypton.Toolkit.KryptonTextBox txtTotalTransferAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblBill_Date;
private Krypton.Toolkit.KryptonDateTimePicker dtpBill_Date;

    
        
              private Krypton.Toolkit.KryptonLabel lblOut_date;
private Krypton.Toolkit.KryptonDateTimePicker dtpOut_date;

    
        
              private Krypton.Toolkit.KryptonLabel lblCreated_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。private Krypton.Toolkit.KryptonLabel lblCreated_by;
private Krypton.Toolkit.KryptonTextBox txtCreated_by;

    
        
              private Krypton.Toolkit.KryptonLabel lblModified_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。private Krypton.Toolkit.KryptonLabel lblModified_by;
private Krypton.Toolkit.KryptonTextBox txtModified_by;

    
        
              private Krypton.Toolkit.KryptonLabel lblNotes;
private Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private Krypton.Toolkit.KryptonLabel lblisdeleted;
private Krypton.Toolkit.KryptonCheckBox chkisdeleted;

    
        
              Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。private Krypton.Toolkit.KryptonLabel lblDataStatus;
private Krypton.Toolkit.KryptonTextBox txtDataStatus;

    
        
              private Krypton.Toolkit.KryptonLabel lblApprovalOpinions;
private Krypton.Toolkit.KryptonTextBox txtApprovalOpinions;

    
        
              Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。private Krypton.Toolkit.KryptonLabel lblApprover_by;
private Krypton.Toolkit.KryptonTextBox txtApprover_by;

    
        
              private Krypton.Toolkit.KryptonLabel lblApprover_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpApprover_at;

    
        
              
    
        
              private Krypton.Toolkit.KryptonLabel lblApprovalResults;
private Krypton.Toolkit.KryptonCheckBox chkApprovalResults;

    
        
              Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。private Krypton.Toolkit.KryptonLabel lblPrintStatus;
private Krypton.Toolkit.KryptonTextBox txtPrintStatus;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

