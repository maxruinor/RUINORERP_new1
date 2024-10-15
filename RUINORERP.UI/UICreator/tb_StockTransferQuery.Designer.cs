
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/15/2024 18:45:35
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
     
     this.lblStockTransferNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtStockTransferNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。
Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。
this.lblEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。
Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。
this.lblTotalCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTotalTransferAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalTransferAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTransfer_date = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpTransfer_date = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

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
                 //#####50StockTransferNo###String
this.lblStockTransferNo.AutoSize = true;
this.lblStockTransferNo.Location = new System.Drawing.Point(100,25);
this.lblStockTransferNo.Name = "lblStockTransferNo";
this.lblStockTransferNo.Size = new System.Drawing.Size(41, 12);
this.lblStockTransferNo.TabIndex = 1;
this.lblStockTransferNo.Text = "调拨单号";
this.txtStockTransferNo.Location = new System.Drawing.Point(173,21);
this.txtStockTransferNo.Name = "txtStockTransferNo";
this.txtStockTransferNo.Size = new System.Drawing.Size(100, 21);
this.txtStockTransferNo.TabIndex = 1;
this.Controls.Add(this.lblStockTransferNo);
this.Controls.Add(this.txtStockTransferNo);

           //#####Location_ID_from###Int64
//属性测试50Location_ID_from
//属性测试50Location_ID_from
Location_ID_from主外字段不一致。//属性测试50Location_ID_from
Location_ID_to主外字段不一致。
           //#####Location_ID_to###Int64
//属性测试75Location_ID_to
//属性测试75Location_ID_to
Location_ID_from主外字段不一致。//属性测试75Location_ID_to
Location_ID_to主外字段不一致。
           //#####Employee_ID###Int64
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

           //#####TotalQty###Int32
//属性测试125TotalQty
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

           //#####Transfer_date###DateTime
this.lblTransfer_date.AutoSize = true;
this.lblTransfer_date.Location = new System.Drawing.Point(100,200);
this.lblTransfer_date.Name = "lblTransfer_date";
this.lblTransfer_date.Size = new System.Drawing.Size(41, 12);
this.lblTransfer_date.TabIndex = 8;
this.lblTransfer_date.Text = "调拨日期";
//111======200
this.dtpTransfer_date.Location = new System.Drawing.Point(173,196);
this.dtpTransfer_date.Name ="dtpTransfer_date";
this.dtpTransfer_date.ShowCheckBox =true;
this.dtpTransfer_date.Size = new System.Drawing.Size(100, 21);
this.dtpTransfer_date.TabIndex = 8;
this.Controls.Add(this.lblTransfer_date);
this.Controls.Add(this.dtpTransfer_date);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,225);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 9;
this.lblCreated_at.Text = "创建时间";
//111======225
this.dtpCreated_at.Location = new System.Drawing.Point(173,221);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 9;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
//属性测试250Created_by
//属性测试250Created_by
Location_ID_from主外字段不一致。//属性测试250Created_by
Location_ID_to主外字段不一致。
           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,275);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 11;
this.lblModified_at.Text = "修改时间";
//111======275
this.dtpModified_at.Location = new System.Drawing.Point(173,271);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 11;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
//属性测试300Modified_by
//属性测试300Modified_by
Location_ID_from主外字段不一致。//属性测试300Modified_by
Location_ID_to主外字段不一致。
           //#####1500Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,325);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 13;
this.lblNotes.Text = "备注";
this.txtNotes.Location = new System.Drawing.Point(173,321);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 13;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,350);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 14;
this.lblisdeleted.Text = "逻辑删除";
this.chkisdeleted.Location = new System.Drawing.Point(173,346);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 14;
this.Controls.Add(this.lblisdeleted);
this.Controls.Add(this.chkisdeleted);

           //#####DataStatus###Int32
//属性测试375DataStatus
//属性测试375DataStatus
Location_ID_from主外字段不一致。//属性测试375DataStatus
Location_ID_to主外字段不一致。
           //#####500ApprovalOpinions###String
this.lblApprovalOpinions.AutoSize = true;
this.lblApprovalOpinions.Location = new System.Drawing.Point(100,400);
this.lblApprovalOpinions.Name = "lblApprovalOpinions";
this.lblApprovalOpinions.Size = new System.Drawing.Size(41, 12);
this.lblApprovalOpinions.TabIndex = 16;
this.lblApprovalOpinions.Text = "审批意见";
this.txtApprovalOpinions.Location = new System.Drawing.Point(173,396);
this.txtApprovalOpinions.Name = "txtApprovalOpinions";
this.txtApprovalOpinions.Size = new System.Drawing.Size(100, 21);
this.txtApprovalOpinions.TabIndex = 16;
this.Controls.Add(this.lblApprovalOpinions);
this.Controls.Add(this.txtApprovalOpinions);

           //#####Approver_by###Int64
//属性测试425Approver_by
//属性测试425Approver_by
Location_ID_from主外字段不一致。//属性测试425Approver_by
Location_ID_to主外字段不一致。
           //#####Approver_at###DateTime
this.lblApprover_at.AutoSize = true;
this.lblApprover_at.Location = new System.Drawing.Point(100,450);
this.lblApprover_at.Name = "lblApprover_at";
this.lblApprover_at.Size = new System.Drawing.Size(41, 12);
this.lblApprover_at.TabIndex = 18;
this.lblApprover_at.Text = "审批时间";
//111======450
this.dtpApprover_at.Location = new System.Drawing.Point(173,446);
this.dtpApprover_at.Name ="dtpApprover_at";
this.dtpApprover_at.ShowCheckBox =true;
this.dtpApprover_at.Size = new System.Drawing.Size(100, 21);
this.dtpApprover_at.TabIndex = 18;
this.Controls.Add(this.lblApprover_at);
this.Controls.Add(this.dtpApprover_at);

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

           //#####PrintStatus###Int32
//属性测试525PrintStatus
//属性测试525PrintStatus
Location_ID_from主外字段不一致。//属性测试525PrintStatus
Location_ID_to主外字段不一致。
          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblStockTransferNo );
this.Controls.Add(this.txtStockTransferNo );

                Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。
                Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。
                this.Controls.Add(this.lblEmployee_ID );
this.Controls.Add(this.cmbEmployee_ID );

                Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。
                this.Controls.Add(this.lblTotalCost );
this.Controls.Add(this.txtTotalCost );

                this.Controls.Add(this.lblTotalTransferAmount );
this.Controls.Add(this.txtTotalTransferAmount );

                this.Controls.Add(this.lblTransfer_date );
this.Controls.Add(this.dtpTransfer_date );

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
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblStockTransferNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtStockTransferNo;

    
        
              Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。
    
        
              Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;
Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。
    
        
              Location_ID_from主外字段不一致。Location_ID_to主外字段不一致。
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalCost;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalTransferAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalTransferAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTransfer_date;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpTransfer_date;

    
        
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


