
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:41
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 领料单(包括生产和托工)
    /// </summary>
    partial class tb_MaterialRequisitionQuery
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
     
     this.lblMaterialRequisitionNO = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtMaterialRequisitionNO = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblMONO = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtMONO = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblDeliveryDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpDeliveryDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();





this.lblMOID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbMOID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();


this.lblShippingAddress = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtShippingAddress = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtShippingAddress.Multiline = true;

this.lblshippingWay = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtshippingWay = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTotalPrice = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalPrice = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTotalCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotalCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();




this.lblTrackNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTrackNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblShipCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtShipCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

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

this.lblOutgoing = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkOutgoing = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkOutgoing.Values.Text ="";


    //for end
    this.SuspendLayout();
    
         //for start
                 //#####50MaterialRequisitionNO###String
this.lblMaterialRequisitionNO.AutoSize = true;
this.lblMaterialRequisitionNO.Location = new System.Drawing.Point(100,25);
this.lblMaterialRequisitionNO.Name = "lblMaterialRequisitionNO";
this.lblMaterialRequisitionNO.Size = new System.Drawing.Size(41, 12);
this.lblMaterialRequisitionNO.TabIndex = 1;
this.lblMaterialRequisitionNO.Text = "领料单号";
this.txtMaterialRequisitionNO.Location = new System.Drawing.Point(173,21);
this.txtMaterialRequisitionNO.Name = "txtMaterialRequisitionNO";
this.txtMaterialRequisitionNO.Size = new System.Drawing.Size(100, 21);
this.txtMaterialRequisitionNO.TabIndex = 1;
this.Controls.Add(this.lblMaterialRequisitionNO);
this.Controls.Add(this.txtMaterialRequisitionNO);

           //#####100MONO###String
this.lblMONO.AutoSize = true;
this.lblMONO.Location = new System.Drawing.Point(100,50);
this.lblMONO.Name = "lblMONO";
this.lblMONO.Size = new System.Drawing.Size(41, 12);
this.lblMONO.TabIndex = 2;
this.lblMONO.Text = "制令单号";
this.txtMONO.Location = new System.Drawing.Point(173,46);
this.txtMONO.Name = "txtMONO";
this.txtMONO.Size = new System.Drawing.Size(100, 21);
this.txtMONO.TabIndex = 2;
this.Controls.Add(this.lblMONO);
this.Controls.Add(this.txtMONO);

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

           //#####Location_ID###Int64
//属性测试100Location_ID

           //#####Employee_ID###Int64
//属性测试125Employee_ID

           //#####DepartmentID###Int64
//属性测试150DepartmentID

           //#####CustomerVendor_ID###Int64
//属性测试175CustomerVendor_ID

           //#####MOID###Int64
//属性测试200MOID
this.lblMOID.AutoSize = true;
this.lblMOID.Location = new System.Drawing.Point(100,200);
this.lblMOID.Name = "lblMOID";
this.lblMOID.Size = new System.Drawing.Size(41, 12);
this.lblMOID.TabIndex = 8;
this.lblMOID.Text = "制令单";
//111======200
this.cmbMOID.Location = new System.Drawing.Point(173,196);
this.cmbMOID.Name ="cmbMOID";
this.cmbMOID.Size = new System.Drawing.Size(100, 21);
this.cmbMOID.TabIndex = 8;
this.Controls.Add(this.lblMOID);
this.Controls.Add(this.cmbMOID);

           //#####ProjectGroup_ID###Int64
//属性测试225ProjectGroup_ID

           //#####255ShippingAddress###String
this.lblShippingAddress.AutoSize = true;
this.lblShippingAddress.Location = new System.Drawing.Point(100,250);
this.lblShippingAddress.Name = "lblShippingAddress";
this.lblShippingAddress.Size = new System.Drawing.Size(41, 12);
this.lblShippingAddress.TabIndex = 10;
this.lblShippingAddress.Text = "发货地址";
this.txtShippingAddress.Location = new System.Drawing.Point(173,246);
this.txtShippingAddress.Name = "txtShippingAddress";
this.txtShippingAddress.Size = new System.Drawing.Size(100, 21);
this.txtShippingAddress.TabIndex = 10;
this.Controls.Add(this.lblShippingAddress);
this.Controls.Add(this.txtShippingAddress);

           //#####50shippingWay###String
this.lblshippingWay.AutoSize = true;
this.lblshippingWay.Location = new System.Drawing.Point(100,275);
this.lblshippingWay.Name = "lblshippingWay";
this.lblshippingWay.Size = new System.Drawing.Size(41, 12);
this.lblshippingWay.TabIndex = 11;
this.lblshippingWay.Text = "发货方式";
this.txtshippingWay.Location = new System.Drawing.Point(173,271);
this.txtshippingWay.Name = "txtshippingWay";
this.txtshippingWay.Size = new System.Drawing.Size(100, 21);
this.txtshippingWay.TabIndex = 11;
this.Controls.Add(this.lblshippingWay);
this.Controls.Add(this.txtshippingWay);

           //#####TotalPrice###Decimal
this.lblTotalPrice.AutoSize = true;
this.lblTotalPrice.Location = new System.Drawing.Point(100,300);
this.lblTotalPrice.Name = "lblTotalPrice";
this.lblTotalPrice.Size = new System.Drawing.Size(41, 12);
this.lblTotalPrice.TabIndex = 12;
this.lblTotalPrice.Text = "总金额";
//111======300
this.txtTotalPrice.Location = new System.Drawing.Point(173,296);
this.txtTotalPrice.Name ="txtTotalPrice";
this.txtTotalPrice.Size = new System.Drawing.Size(100, 21);
this.txtTotalPrice.TabIndex = 12;
this.Controls.Add(this.lblTotalPrice);
this.Controls.Add(this.txtTotalPrice);

           //#####TotalCost###Decimal
this.lblTotalCost.AutoSize = true;
this.lblTotalCost.Location = new System.Drawing.Point(100,325);
this.lblTotalCost.Name = "lblTotalCost";
this.lblTotalCost.Size = new System.Drawing.Size(41, 12);
this.lblTotalCost.TabIndex = 13;
this.lblTotalCost.Text = "总成本";
//111======325
this.txtTotalCost.Location = new System.Drawing.Point(173,321);
this.txtTotalCost.Name ="txtTotalCost";
this.txtTotalCost.Size = new System.Drawing.Size(100, 21);
this.txtTotalCost.TabIndex = 13;
this.Controls.Add(this.lblTotalCost);
this.Controls.Add(this.txtTotalCost);

           //#####ExpectedQuantity###Int32
//属性测试350ExpectedQuantity

           //#####TotalSendQty###Int32
//属性测试375TotalSendQty

           //#####TotalReQty###Int32
//属性测试400TotalReQty

           //#####50TrackNo###String
this.lblTrackNo.AutoSize = true;
this.lblTrackNo.Location = new System.Drawing.Point(100,425);
this.lblTrackNo.Name = "lblTrackNo";
this.lblTrackNo.Size = new System.Drawing.Size(41, 12);
this.lblTrackNo.TabIndex = 17;
this.lblTrackNo.Text = "物流单号";
this.txtTrackNo.Location = new System.Drawing.Point(173,421);
this.txtTrackNo.Name = "txtTrackNo";
this.txtTrackNo.Size = new System.Drawing.Size(100, 21);
this.txtTrackNo.TabIndex = 17;
this.Controls.Add(this.lblTrackNo);
this.Controls.Add(this.txtTrackNo);

           //#####ShipCost###Decimal
this.lblShipCost.AutoSize = true;
this.lblShipCost.Location = new System.Drawing.Point(100,450);
this.lblShipCost.Name = "lblShipCost";
this.lblShipCost.Size = new System.Drawing.Size(41, 12);
this.lblShipCost.TabIndex = 18;
this.lblShipCost.Text = "运费";
//111======450
this.txtShipCost.Location = new System.Drawing.Point(173,446);
this.txtShipCost.Name ="txtShipCost";
this.txtShipCost.Size = new System.Drawing.Size(100, 21);
this.txtShipCost.TabIndex = 18;
this.Controls.Add(this.lblShipCost);
this.Controls.Add(this.txtShipCost);

           //#####ReApply###Boolean
this.lblReApply.AutoSize = true;
this.lblReApply.Location = new System.Drawing.Point(100,475);
this.lblReApply.Name = "lblReApply";
this.lblReApply.Size = new System.Drawing.Size(41, 12);
this.lblReApply.TabIndex = 19;
this.lblReApply.Text = "是否补领";
this.chkReApply.Location = new System.Drawing.Point(173,471);
this.chkReApply.Name = "chkReApply";
this.chkReApply.Size = new System.Drawing.Size(100, 21);
this.chkReApply.TabIndex = 19;
this.Controls.Add(this.lblReApply);
this.Controls.Add(this.chkReApply);

           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,500);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 20;
this.lblisdeleted.Text = "逻辑删除";
this.chkisdeleted.Location = new System.Drawing.Point(173,496);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 20;
this.Controls.Add(this.lblisdeleted);
this.Controls.Add(this.chkisdeleted);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,525);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 21;
this.lblCreated_at.Text = "创建时间";
//111======525
this.dtpCreated_at.Location = new System.Drawing.Point(173,521);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 21;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64
//属性测试550Created_by

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,575);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 23;
this.lblModified_at.Text = "修改时间";
//111======575
this.dtpModified_at.Location = new System.Drawing.Point(173,571);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 23;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64
//属性测试600Modified_by

           //#####255Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,625);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 25;
this.lblNotes.Text = "备注";
this.txtNotes.Location = new System.Drawing.Point(173,621);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 25;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####200ApprovalOpinions###String
this.lblApprovalOpinions.AutoSize = true;
this.lblApprovalOpinions.Location = new System.Drawing.Point(100,650);
this.lblApprovalOpinions.Name = "lblApprovalOpinions";
this.lblApprovalOpinions.Size = new System.Drawing.Size(41, 12);
this.lblApprovalOpinions.TabIndex = 26;
this.lblApprovalOpinions.Text = "审批意见";
this.txtApprovalOpinions.Location = new System.Drawing.Point(173,646);
this.txtApprovalOpinions.Name = "txtApprovalOpinions";
this.txtApprovalOpinions.Size = new System.Drawing.Size(100, 21);
this.txtApprovalOpinions.TabIndex = 26;
this.Controls.Add(this.lblApprovalOpinions);
this.Controls.Add(this.txtApprovalOpinions);

           //#####Approver_by###Int64
//属性测试675Approver_by

           //#####Approver_at###DateTime
this.lblApprover_at.AutoSize = true;
this.lblApprover_at.Location = new System.Drawing.Point(100,700);
this.lblApprover_at.Name = "lblApprover_at";
this.lblApprover_at.Size = new System.Drawing.Size(41, 12);
this.lblApprover_at.TabIndex = 28;
this.lblApprover_at.Text = "审批时间";
//111======700
this.dtpApprover_at.Location = new System.Drawing.Point(173,696);
this.dtpApprover_at.Name ="dtpApprover_at";
this.dtpApprover_at.ShowCheckBox =true;
this.dtpApprover_at.Size = new System.Drawing.Size(100, 21);
this.dtpApprover_at.TabIndex = 28;
this.Controls.Add(this.lblApprover_at);
this.Controls.Add(this.dtpApprover_at);

           //#####ApprovalStatus###SByte

           //#####ApprovalResults###Boolean
this.lblApprovalResults.AutoSize = true;
this.lblApprovalResults.Location = new System.Drawing.Point(100,750);
this.lblApprovalResults.Name = "lblApprovalResults";
this.lblApprovalResults.Size = new System.Drawing.Size(41, 12);
this.lblApprovalResults.TabIndex = 30;
this.lblApprovalResults.Text = "审批结果";
this.chkApprovalResults.Location = new System.Drawing.Point(173,746);
this.chkApprovalResults.Name = "chkApprovalResults";
this.chkApprovalResults.Size = new System.Drawing.Size(100, 21);
this.chkApprovalResults.TabIndex = 30;
this.Controls.Add(this.lblApprovalResults);
this.Controls.Add(this.chkApprovalResults);

           //#####DataStatus###Int32
//属性测试775DataStatus

           //#####GeneEvidence###Boolean
this.lblGeneEvidence.AutoSize = true;
this.lblGeneEvidence.Location = new System.Drawing.Point(100,800);
this.lblGeneEvidence.Name = "lblGeneEvidence";
this.lblGeneEvidence.Size = new System.Drawing.Size(41, 12);
this.lblGeneEvidence.TabIndex = 32;
this.lblGeneEvidence.Text = "产生凭证";
this.chkGeneEvidence.Location = new System.Drawing.Point(173,796);
this.chkGeneEvidence.Name = "chkGeneEvidence";
this.chkGeneEvidence.Size = new System.Drawing.Size(100, 21);
this.chkGeneEvidence.TabIndex = 32;
this.Controls.Add(this.lblGeneEvidence);
this.Controls.Add(this.chkGeneEvidence);

           //#####Outgoing###Boolean
this.lblOutgoing.AutoSize = true;
this.lblOutgoing.Location = new System.Drawing.Point(100,825);
this.lblOutgoing.Name = "lblOutgoing";
this.lblOutgoing.Size = new System.Drawing.Size(41, 12);
this.lblOutgoing.TabIndex = 33;
this.lblOutgoing.Text = "外发加工";
this.chkOutgoing.Location = new System.Drawing.Point(173,821);
this.chkOutgoing.Name = "chkOutgoing";
this.chkOutgoing.Size = new System.Drawing.Size(100, 21);
this.chkOutgoing.TabIndex = 33;
this.Controls.Add(this.lblOutgoing);
this.Controls.Add(this.chkOutgoing);

           //#####PrintStatus###Int32
//属性测试850PrintStatus

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblMaterialRequisitionNO );
this.Controls.Add(this.txtMaterialRequisitionNO );

                this.Controls.Add(this.lblMONO );
this.Controls.Add(this.txtMONO );

                this.Controls.Add(this.lblDeliveryDate );
this.Controls.Add(this.dtpDeliveryDate );

                
                
                
                
                this.Controls.Add(this.lblMOID );
this.Controls.Add(this.cmbMOID );

                
                this.Controls.Add(this.lblShippingAddress );
this.Controls.Add(this.txtShippingAddress );

                this.Controls.Add(this.lblshippingWay );
this.Controls.Add(this.txtshippingWay );

                this.Controls.Add(this.lblTotalPrice );
this.Controls.Add(this.txtTotalPrice );

                this.Controls.Add(this.lblTotalCost );
this.Controls.Add(this.txtTotalCost );

                
                
                
                this.Controls.Add(this.lblTrackNo );
this.Controls.Add(this.txtTrackNo );

                this.Controls.Add(this.lblShipCost );
this.Controls.Add(this.txtShipCost );

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

                this.Controls.Add(this.lblOutgoing );
this.Controls.Add(this.chkOutgoing );

                
                    
            this.Name = "tb_MaterialRequisitionQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblMaterialRequisitionNO;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtMaterialRequisitionNO;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblMONO;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtMONO;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDeliveryDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpDeliveryDate;

    
        
              
    
        
              
    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblMOID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbMOID;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblShippingAddress;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtShippingAddress;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblshippingWay;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtshippingWay;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalPrice;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalPrice;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotalCost;

    
        
              
    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTrackNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTrackNo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblShipCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtShipCost;

    
        
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

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblOutgoing;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkOutgoing;

    
        
              
    
    
   
 





    }
}


