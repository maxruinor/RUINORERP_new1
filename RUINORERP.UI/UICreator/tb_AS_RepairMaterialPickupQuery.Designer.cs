
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/22/2025 18:02:27
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 维修领料单
    /// </summary>
    partial class tb_AS_RepairMaterialPickupQuery
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
     
     this.lblRepairOrderID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbRepairOrderID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblMaterialPickupNO = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtMaterialPickupNO = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblDeliveryDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpDeliveryDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblRepairOrderNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtRepairOrderNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbEmployee_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblTotalPrice = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalPrice = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTotalCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblTotalSendQty = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalSendQty = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblReApply = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkReApply = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkReApply.Values.Text ="";

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


this.lblApprover_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpApprover_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblApprovalResults = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkApprovalResults = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkApprovalResults.Values.Text ="";


this.lblGeneEvidence = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkGeneEvidence = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkGeneEvidence.Values.Text ="";


    //for end
    this.SuspendLayout();
    
         //for start
                 //#####RepairOrderID###Int64
//属性测试25RepairOrderID
this.lblRepairOrderID.AutoSize = true;
this.lblRepairOrderID.Location = new System.Drawing.Point(100,25);
this.lblRepairOrderID.Name = "lblRepairOrderID";
this.lblRepairOrderID.Size = new System.Drawing.Size(41, 12);
this.lblRepairOrderID.TabIndex = 1;
this.lblRepairOrderID.Text = "维修工单";
//111======25
this.cmbRepairOrderID.Location = new System.Drawing.Point(173,21);
this.cmbRepairOrderID.Name ="cmbRepairOrderID";
this.cmbRepairOrderID.Size = new System.Drawing.Size(100, 21);
this.cmbRepairOrderID.TabIndex = 1;
this.Controls.Add(this.lblRepairOrderID);
this.Controls.Add(this.cmbRepairOrderID);

           //#####50MaterialPickupNO###String
this.lblMaterialPickupNO.AutoSize = true;
this.lblMaterialPickupNO.Location = new System.Drawing.Point(100,50);
this.lblMaterialPickupNO.Name = "lblMaterialPickupNO";
this.lblMaterialPickupNO.Size = new System.Drawing.Size(41, 12);
this.lblMaterialPickupNO.TabIndex = 2;
this.lblMaterialPickupNO.Text = "领料单号";
this.txtMaterialPickupNO.Location = new System.Drawing.Point(173,46);
this.txtMaterialPickupNO.Name = "txtMaterialPickupNO";
this.txtMaterialPickupNO.Size = new System.Drawing.Size(100, 21);
this.txtMaterialPickupNO.TabIndex = 2;
this.Controls.Add(this.lblMaterialPickupNO);
this.Controls.Add(this.txtMaterialPickupNO);

           //#####DeliveryDate###DateTime
this.lblDeliveryDate.AutoSize = true;
this.lblDeliveryDate.Location = new System.Drawing.Point(100,75);
this.lblDeliveryDate.Name = "lblDeliveryDate";
this.lblDeliveryDate.Size = new System.Drawing.Size(41, 12);
this.lblDeliveryDate.TabIndex = 3;
this.lblDeliveryDate.Text = "领取日期";
//111======75
this.dtpDeliveryDate.Location = new System.Drawing.Point(173,71);
this.dtpDeliveryDate.Name ="dtpDeliveryDate";
this.dtpDeliveryDate.ShowCheckBox =true;
this.dtpDeliveryDate.Size = new System.Drawing.Size(100, 21);
this.dtpDeliveryDate.TabIndex = 3;
this.Controls.Add(this.lblDeliveryDate);
this.Controls.Add(this.dtpDeliveryDate);

           //#####100RepairOrderNo###String
this.lblRepairOrderNo.AutoSize = true;
this.lblRepairOrderNo.Location = new System.Drawing.Point(100,100);
this.lblRepairOrderNo.Name = "lblRepairOrderNo";
this.lblRepairOrderNo.Size = new System.Drawing.Size(41, 12);
this.lblRepairOrderNo.TabIndex = 4;
this.lblRepairOrderNo.Text = "维修工单";
this.txtRepairOrderNo.Location = new System.Drawing.Point(173,96);
this.txtRepairOrderNo.Name = "txtRepairOrderNo";
this.txtRepairOrderNo.Size = new System.Drawing.Size(100, 21);
this.txtRepairOrderNo.TabIndex = 4;
this.Controls.Add(this.lblRepairOrderNo);
this.Controls.Add(this.txtRepairOrderNo);

           //#####Employee_ID###Int64
//属性测试125Employee_ID
//属性测试125Employee_ID
this.lblEmployee_ID.AutoSize = true;
this.lblEmployee_ID.Location = new System.Drawing.Point(100,125);
this.lblEmployee_ID.Name = "lblEmployee_ID";
this.lblEmployee_ID.Size = new System.Drawing.Size(41, 12);
this.lblEmployee_ID.TabIndex = 5;
this.lblEmployee_ID.Text = "经办人";
//111======125
this.cmbEmployee_ID.Location = new System.Drawing.Point(173,121);
this.cmbEmployee_ID.Name ="cmbEmployee_ID";
this.cmbEmployee_ID.Size = new System.Drawing.Size(100, 21);
this.cmbEmployee_ID.TabIndex = 5;
this.Controls.Add(this.lblEmployee_ID);
this.Controls.Add(this.cmbEmployee_ID);

           //#####TotalPrice###Decimal
this.lblTotalPrice.AutoSize = true;
this.lblTotalPrice.Location = new System.Drawing.Point(100,150);
this.lblTotalPrice.Name = "lblTotalPrice";
this.lblTotalPrice.Size = new System.Drawing.Size(41, 12);
this.lblTotalPrice.TabIndex = 6;
this.lblTotalPrice.Text = "总金额";
//111======150
this.txtTotalPrice.Location = new System.Drawing.Point(173,146);
this.txtTotalPrice.Name ="txtTotalPrice";
this.txtTotalPrice.Size = new System.Drawing.Size(100, 21);
this.txtTotalPrice.TabIndex = 6;
this.Controls.Add(this.lblTotalPrice);
this.Controls.Add(this.txtTotalPrice);

           //#####TotalCost###Decimal
this.lblTotalCost.AutoSize = true;
this.lblTotalCost.Location = new System.Drawing.Point(100,175);
this.lblTotalCost.Name = "lblTotalCost";
this.lblTotalCost.Size = new System.Drawing.Size(41, 12);
this.lblTotalCost.TabIndex = 7;
this.lblTotalCost.Text = "总成本";
//111======175
this.txtTotalCost.Location = new System.Drawing.Point(173,171);
this.txtTotalCost.Name ="txtTotalCost";
this.txtTotalCost.Size = new System.Drawing.Size(100, 21);
this.txtTotalCost.TabIndex = 7;
this.Controls.Add(this.lblTotalCost);
this.Controls.Add(this.txtTotalCost);

           //#####TotalReQty###Int32
//属性测试200TotalReQty
//属性测试200TotalReQty

           //#####TotalSendQty###Decimal
this.lblTotalSendQty.AutoSize = true;
this.lblTotalSendQty.Location = new System.Drawing.Point(100,225);
this.lblTotalSendQty.Name = "lblTotalSendQty";
this.lblTotalSendQty.Size = new System.Drawing.Size(41, 12);
this.lblTotalSendQty.TabIndex = 9;
this.lblTotalSendQty.Text = "总发数量";
//111======225
this.txtTotalSendQty.Location = new System.Drawing.Point(173,221);
this.txtTotalSendQty.Name ="txtTotalSendQty";
this.txtTotalSendQty.Size = new System.Drawing.Size(100, 21);
this.txtTotalSendQty.TabIndex = 9;
this.Controls.Add(this.lblTotalSendQty);
this.Controls.Add(this.txtTotalSendQty);

           //#####ReApply###Boolean
this.lblReApply.AutoSize = true;
this.lblReApply.Location = new System.Drawing.Point(100,250);
this.lblReApply.Name = "lblReApply";
this.lblReApply.Size = new System.Drawing.Size(41, 12);
this.lblReApply.TabIndex = 10;
this.lblReApply.Text = "是否补领";
this.chkReApply.Location = new System.Drawing.Point(173,246);
this.chkReApply.Name = "chkReApply";
this.chkReApply.Size = new System.Drawing.Size(100, 21);
this.chkReApply.TabIndex = 10;
this.Controls.Add(this.lblReApply);
this.Controls.Add(this.chkReApply);

           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,275);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 11;
this.lblisdeleted.Text = "逻辑删除";
this.chkisdeleted.Location = new System.Drawing.Point(173,271);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 11;
this.Controls.Add(this.lblisdeleted);
this.Controls.Add(this.chkisdeleted);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,300);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 12;
this.lblCreated_at.Text = "创建时间";
//111======300
this.dtpCreated_at.Location = new System.Drawing.Point(173,296);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 12;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
//属性测试325Created_by
//属性测试325Created_by

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,350);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 14;
this.lblModified_at.Text = "修改时间";
//111======350
this.dtpModified_at.Location = new System.Drawing.Point(173,346);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 14;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
//属性测试375Modified_by
//属性测试375Modified_by

           //#####255Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,400);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 16;
this.lblNotes.Text = "备注";
this.txtNotes.Location = new System.Drawing.Point(173,396);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 16;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####200ApprovalOpinions###String
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
//属性测试450Approver_by

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

           //#####DataStatus###Int32
//属性测试550DataStatus
//属性测试550DataStatus

           //#####GeneEvidence###Boolean
this.lblGeneEvidence.AutoSize = true;
this.lblGeneEvidence.Location = new System.Drawing.Point(100,575);
this.lblGeneEvidence.Name = "lblGeneEvidence";
this.lblGeneEvidence.Size = new System.Drawing.Size(41, 12);
this.lblGeneEvidence.TabIndex = 23;
this.lblGeneEvidence.Text = "产生凭证";
this.chkGeneEvidence.Location = new System.Drawing.Point(173,571);
this.chkGeneEvidence.Name = "chkGeneEvidence";
this.chkGeneEvidence.Size = new System.Drawing.Size(100, 21);
this.chkGeneEvidence.TabIndex = 23;
this.Controls.Add(this.lblGeneEvidence);
this.Controls.Add(this.chkGeneEvidence);

           //#####PrintStatus###Int32
//属性测试600PrintStatus
//属性测试600PrintStatus

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblRepairOrderID );
this.Controls.Add(this.cmbRepairOrderID );

                this.Controls.Add(this.lblMaterialPickupNO );
this.Controls.Add(this.txtMaterialPickupNO );

                this.Controls.Add(this.lblDeliveryDate );
this.Controls.Add(this.dtpDeliveryDate );

                this.Controls.Add(this.lblRepairOrderNo );
this.Controls.Add(this.txtRepairOrderNo );

                this.Controls.Add(this.lblEmployee_ID );
this.Controls.Add(this.cmbEmployee_ID );

                this.Controls.Add(this.lblTotalPrice );
this.Controls.Add(this.txtTotalPrice );

                this.Controls.Add(this.lblTotalCost );
this.Controls.Add(this.txtTotalCost );

                
                this.Controls.Add(this.lblTotalSendQty );
this.Controls.Add(this.txtTotalSendQty );

                this.Controls.Add(this.lblReApply );
this.Controls.Add(this.chkReApply );

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

                
                this.Controls.Add(this.lblApprover_at );
this.Controls.Add(this.dtpApprover_at );

                
                this.Controls.Add(this.lblApprovalResults );
this.Controls.Add(this.chkApprovalResults );

                
                this.Controls.Add(this.lblGeneEvidence );
this.Controls.Add(this.chkGeneEvidence );

                
                    
            this.Name = "tb_AS_RepairMaterialPickupQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRepairOrderID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbRepairOrderID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblMaterialPickupNO;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtMaterialPickupNO;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDeliveryDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpDeliveryDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRepairOrderNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtRepairOrderNo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalPrice;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalPrice;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalCost;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalSendQty;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalSendQty;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblReApply;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkReApply;

    
        
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

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprover_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpApprover_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprovalResults;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkApprovalResults;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblGeneEvidence;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkGeneEvidence;

    
        
              
    
    
   
 





    }
}


