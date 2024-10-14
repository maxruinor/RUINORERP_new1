
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
    partial class tb_StockTransferQuery
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
     
     Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。
Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。
Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。
this.lblStockTransferNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtStockTransferNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。
this.lblTotalCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTotalTransferAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalTransferAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblBill_Date = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpBill_Date = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblOut_date = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpOut_date = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。
this.lblModified_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。
this.lblNotes = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotes = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

this.lblisdeleted = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkisdeleted = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkisdeleted.Values.Text ="";

Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。
this.lblApprovalOpinions = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtApprovalOpinions = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtApprovalOpinions.Multiline = true;

Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。
this.lblApprover_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpApprover_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblApprovalResults = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkApprovalResults = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkApprovalResults.Values.Text ="";

Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。
    //for end
    this.SuspendLayout();
    
         //for start
                 //#####Location_ID_from###Int64
//属性测试25Location_ID_from
Location_ID_from主外字段不一致。//属性测试25Location_ID_from
Location_ID_to主外字段不一致。
           //#####Location_ID_to###Int64
//属性测试50Location_ID_to
Location_ID_from主外字段不一致。//属性测试50Location_ID_to
Location_ID_to主外字段不一致。
           //#####Employee_ID###Int64
//属性测试75Employee_ID
Location_ID_from主外字段不一致。//属性测试75Employee_ID
Location_ID_to主外字段不一致。
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
Location_ID_to主外字段不一致。
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
Location_ID_to主外字段不一致。
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
Location_ID_to主外字段不一致。
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
Location_ID_to主外字段不一致。
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
Location_ID_to主外字段不一致。
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
Location_ID_to主外字段不一致。
          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。
                Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。
                Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。
                this.Controls.Add(this.lblStockTransferNo );
this.Controls.Add(this.txtStockTransferNo );

                Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。
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

                Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。
                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。
                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblisdeleted );
this.Controls.Add(this.chkisdeleted );

                Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。
                this.Controls.Add(this.lblApprovalOpinions );
this.Controls.Add(this.txtApprovalOpinions );

                Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。
                this.Controls.Add(this.lblApprover_at );
this.Controls.Add(this.dtpApprover_at );

                
                this.Controls.Add(this.lblApprovalResults );
this.Controls.Add(this.chkApprovalResults );

                Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。
                    
            this.Name = "tb_StockTransferQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。
    
        
              Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。
    
        
              Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblStockTransferNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtStockTransferNo;

    
        
              Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalCost;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalTransferAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalTransferAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblBill_Date;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpBill_Date;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblOut_date;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpOut_date;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModified_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblisdeleted;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkisdeleted;

    
        
              Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprovalOpinions;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtApprovalOpinions;

    
        
              Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprover_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpApprover_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprovalResults;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkApprovalResults;

    
        
              Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。
    
    
   
 





    }
}


