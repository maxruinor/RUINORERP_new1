
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:31
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 返工退库统计
    /// </summary>
    partial class View_MRP_ReworkReturnQuery
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

this.lblDeliveryBillNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtDeliveryBillNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();




this.lblCloseCaseOpinions = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCloseCaseOpinions = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblApprovalOpinions = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtApprovalOpinions = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblReturnDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpReturnDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblExpectedReturnDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpExpectedReturnDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblReasonForRework = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtReasonForRework = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtReasonForRework.Multiline = true;

this.lblApprover_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpApprover_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();



this.lblApprovalResults = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkApprovalResults = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkApprovalResults.Values.Text ="";

this.lblCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblModified_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpModified_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();



this.lblSKU = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSKU = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSpecifications = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSpecifications = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtSpecifications.Multiline = true;

this.lblCNName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCNName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtCNName.Multiline = true;

this.lblModel = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtModel = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblproperty = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtproperty = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtproperty.Multiline = true;


this.lblSummary = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSummary = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtSummary.Multiline = true;



this.lblReworkFee = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtReworkFee = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSubtotalReworkFee = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSubtotalReworkFee = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblUnitCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtUnitCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSubtotalCostAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSubtotalCostAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCustomertModel = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCustomertModel = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblProductNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtProductNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblNotes = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotes = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####ReworkReturnID###Int64

           //#####50ReworkReturnNo###String
this.lblReworkReturnNo.AutoSize = true;
this.lblReworkReturnNo.Location = new System.Drawing.Point(100,50);
this.lblReworkReturnNo.Name = "lblReworkReturnNo";
this.lblReworkReturnNo.Size = new System.Drawing.Size(41, 12);
this.lblReworkReturnNo.TabIndex = 2;
this.lblReworkReturnNo.Text = "";
this.txtReworkReturnNo.Location = new System.Drawing.Point(173,46);
this.txtReworkReturnNo.Name = "txtReworkReturnNo";
this.txtReworkReturnNo.Size = new System.Drawing.Size(100, 21);
this.txtReworkReturnNo.TabIndex = 2;
this.Controls.Add(this.lblReworkReturnNo);
this.Controls.Add(this.txtReworkReturnNo);

           //#####100DeliveryBillNo###String
this.lblDeliveryBillNo.AutoSize = true;
this.lblDeliveryBillNo.Location = new System.Drawing.Point(100,75);
this.lblDeliveryBillNo.Name = "lblDeliveryBillNo";
this.lblDeliveryBillNo.Size = new System.Drawing.Size(41, 12);
this.lblDeliveryBillNo.TabIndex = 3;
this.lblDeliveryBillNo.Text = "";
this.txtDeliveryBillNo.Location = new System.Drawing.Point(173,71);
this.txtDeliveryBillNo.Name = "txtDeliveryBillNo";
this.txtDeliveryBillNo.Size = new System.Drawing.Size(100, 21);
this.txtDeliveryBillNo.TabIndex = 3;
this.Controls.Add(this.lblDeliveryBillNo);
this.Controls.Add(this.txtDeliveryBillNo);

           //#####CustomerVendor_ID###Int64

           //#####Employee_ID###Int64

           //#####DataStatus###Int32

           //#####200CloseCaseOpinions###String
this.lblCloseCaseOpinions.AutoSize = true;
this.lblCloseCaseOpinions.Location = new System.Drawing.Point(100,175);
this.lblCloseCaseOpinions.Name = "lblCloseCaseOpinions";
this.lblCloseCaseOpinions.Size = new System.Drawing.Size(41, 12);
this.lblCloseCaseOpinions.TabIndex = 7;
this.lblCloseCaseOpinions.Text = "";
this.txtCloseCaseOpinions.Location = new System.Drawing.Point(173,171);
this.txtCloseCaseOpinions.Name = "txtCloseCaseOpinions";
this.txtCloseCaseOpinions.Size = new System.Drawing.Size(100, 21);
this.txtCloseCaseOpinions.TabIndex = 7;
this.Controls.Add(this.lblCloseCaseOpinions);
this.Controls.Add(this.txtCloseCaseOpinions);

           //#####200ApprovalOpinions###String
this.lblApprovalOpinions.AutoSize = true;
this.lblApprovalOpinions.Location = new System.Drawing.Point(100,200);
this.lblApprovalOpinions.Name = "lblApprovalOpinions";
this.lblApprovalOpinions.Size = new System.Drawing.Size(41, 12);
this.lblApprovalOpinions.TabIndex = 8;
this.lblApprovalOpinions.Text = "";
this.txtApprovalOpinions.Location = new System.Drawing.Point(173,196);
this.txtApprovalOpinions.Name = "txtApprovalOpinions";
this.txtApprovalOpinions.Size = new System.Drawing.Size(100, 21);
this.txtApprovalOpinions.TabIndex = 8;
this.Controls.Add(this.lblApprovalOpinions);
this.Controls.Add(this.txtApprovalOpinions);

           //#####DepartmentID###Int64

           //#####ReturnDate###DateTime
this.lblReturnDate.AutoSize = true;
this.lblReturnDate.Location = new System.Drawing.Point(100,250);
this.lblReturnDate.Name = "lblReturnDate";
this.lblReturnDate.Size = new System.Drawing.Size(41, 12);
this.lblReturnDate.TabIndex = 10;
this.lblReturnDate.Text = "";
//111======250
this.dtpReturnDate.Location = new System.Drawing.Point(173,246);
this.dtpReturnDate.Name ="dtpReturnDate";
this.dtpReturnDate.ShowCheckBox =true;
this.dtpReturnDate.Size = new System.Drawing.Size(100, 21);
this.dtpReturnDate.TabIndex = 10;
this.Controls.Add(this.lblReturnDate);
this.Controls.Add(this.dtpReturnDate);

           //#####ExpectedReturnDate###DateTime
this.lblExpectedReturnDate.AutoSize = true;
this.lblExpectedReturnDate.Location = new System.Drawing.Point(100,275);
this.lblExpectedReturnDate.Name = "lblExpectedReturnDate";
this.lblExpectedReturnDate.Size = new System.Drawing.Size(41, 12);
this.lblExpectedReturnDate.TabIndex = 11;
this.lblExpectedReturnDate.Text = "";
//111======275
this.dtpExpectedReturnDate.Location = new System.Drawing.Point(173,271);
this.dtpExpectedReturnDate.Name ="dtpExpectedReturnDate";
this.dtpExpectedReturnDate.ShowCheckBox =true;
this.dtpExpectedReturnDate.Size = new System.Drawing.Size(100, 21);
this.dtpExpectedReturnDate.TabIndex = 11;
this.Controls.Add(this.lblExpectedReturnDate);
this.Controls.Add(this.dtpExpectedReturnDate);

           //#####500ReasonForRework###String
this.lblReasonForRework.AutoSize = true;
this.lblReasonForRework.Location = new System.Drawing.Point(100,300);
this.lblReasonForRework.Name = "lblReasonForRework";
this.lblReasonForRework.Size = new System.Drawing.Size(41, 12);
this.lblReasonForRework.TabIndex = 12;
this.lblReasonForRework.Text = "";
this.txtReasonForRework.Location = new System.Drawing.Point(173,296);
this.txtReasonForRework.Name = "txtReasonForRework";
this.txtReasonForRework.Size = new System.Drawing.Size(100, 21);
this.txtReasonForRework.TabIndex = 12;
this.Controls.Add(this.lblReasonForRework);
this.Controls.Add(this.txtReasonForRework);

           //#####Approver_at###DateTime
this.lblApprover_at.AutoSize = true;
this.lblApprover_at.Location = new System.Drawing.Point(100,325);
this.lblApprover_at.Name = "lblApprover_at";
this.lblApprover_at.Size = new System.Drawing.Size(41, 12);
this.lblApprover_at.TabIndex = 13;
this.lblApprover_at.Text = "";
//111======325
this.dtpApprover_at.Location = new System.Drawing.Point(173,321);
this.dtpApprover_at.Name ="dtpApprover_at";
this.dtpApprover_at.ShowCheckBox =true;
this.dtpApprover_at.Size = new System.Drawing.Size(100, 21);
this.dtpApprover_at.TabIndex = 13;
this.Controls.Add(this.lblApprover_at);
this.Controls.Add(this.dtpApprover_at);

           //#####Approver_by###Int64

           //#####ApprovalStatus###SByte

           //#####ApprovalResults###Boolean
this.lblApprovalResults.AutoSize = true;
this.lblApprovalResults.Location = new System.Drawing.Point(100,400);
this.lblApprovalResults.Name = "lblApprovalResults";
this.lblApprovalResults.Size = new System.Drawing.Size(41, 12);
this.lblApprovalResults.TabIndex = 16;
this.lblApprovalResults.Text = "";
this.chkApprovalResults.Location = new System.Drawing.Point(173,396);
this.chkApprovalResults.Name = "chkApprovalResults";
this.chkApprovalResults.Size = new System.Drawing.Size(100, 21);
this.chkApprovalResults.TabIndex = 16;
this.Controls.Add(this.lblApprovalResults);
this.Controls.Add(this.chkApprovalResults);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,425);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 17;
this.lblCreated_at.Text = "";
//111======425
this.dtpCreated_at.Location = new System.Drawing.Point(173,421);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 17;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64

           //#####Modified_at###DateTime
this.lblModified_at.AutoSize = true;
this.lblModified_at.Location = new System.Drawing.Point(100,475);
this.lblModified_at.Name = "lblModified_at";
this.lblModified_at.Size = new System.Drawing.Size(41, 12);
this.lblModified_at.TabIndex = 19;
this.lblModified_at.Text = "";
//111======475
this.dtpModified_at.Location = new System.Drawing.Point(173,471);
this.dtpModified_at.Name ="dtpModified_at";
this.dtpModified_at.ShowCheckBox =true;
this.dtpModified_at.Size = new System.Drawing.Size(100, 21);
this.dtpModified_at.TabIndex = 19;
this.Controls.Add(this.lblModified_at);
this.Controls.Add(this.dtpModified_at);

           //#####Modified_by###Int64

           //#####ProdDetailID###Int64

           //#####80SKU###String
this.lblSKU.AutoSize = true;
this.lblSKU.Location = new System.Drawing.Point(100,550);
this.lblSKU.Name = "lblSKU";
this.lblSKU.Size = new System.Drawing.Size(41, 12);
this.lblSKU.TabIndex = 22;
this.lblSKU.Text = "";
this.txtSKU.Location = new System.Drawing.Point(173,546);
this.txtSKU.Name = "txtSKU";
this.txtSKU.Size = new System.Drawing.Size(100, 21);
this.txtSKU.TabIndex = 22;
this.Controls.Add(this.lblSKU);
this.Controls.Add(this.txtSKU);

           //#####1000Specifications###String
this.lblSpecifications.AutoSize = true;
this.lblSpecifications.Location = new System.Drawing.Point(100,575);
this.lblSpecifications.Name = "lblSpecifications";
this.lblSpecifications.Size = new System.Drawing.Size(41, 12);
this.lblSpecifications.TabIndex = 23;
this.lblSpecifications.Text = "";
this.txtSpecifications.Location = new System.Drawing.Point(173,571);
this.txtSpecifications.Name = "txtSpecifications";
this.txtSpecifications.Size = new System.Drawing.Size(100, 21);
this.txtSpecifications.TabIndex = 23;
this.Controls.Add(this.lblSpecifications);
this.Controls.Add(this.txtSpecifications);

           //#####255CNName###String
this.lblCNName.AutoSize = true;
this.lblCNName.Location = new System.Drawing.Point(100,600);
this.lblCNName.Name = "lblCNName";
this.lblCNName.Size = new System.Drawing.Size(41, 12);
this.lblCNName.TabIndex = 24;
this.lblCNName.Text = "";
this.txtCNName.Location = new System.Drawing.Point(173,596);
this.txtCNName.Name = "txtCNName";
this.txtCNName.Size = new System.Drawing.Size(100, 21);
this.txtCNName.TabIndex = 24;
this.Controls.Add(this.lblCNName);
this.Controls.Add(this.txtCNName);

           //#####50Model###String
this.lblModel.AutoSize = true;
this.lblModel.Location = new System.Drawing.Point(100,625);
this.lblModel.Name = "lblModel";
this.lblModel.Size = new System.Drawing.Size(41, 12);
this.lblModel.TabIndex = 25;
this.lblModel.Text = "";
this.txtModel.Location = new System.Drawing.Point(173,621);
this.txtModel.Name = "txtModel";
this.txtModel.Size = new System.Drawing.Size(100, 21);
this.txtModel.TabIndex = 25;
this.Controls.Add(this.lblModel);
this.Controls.Add(this.txtModel);

           //#####Type_ID###Int64

           //#####255property###String
this.lblproperty.AutoSize = true;
this.lblproperty.Location = new System.Drawing.Point(100,675);
this.lblproperty.Name = "lblproperty";
this.lblproperty.Size = new System.Drawing.Size(41, 12);
this.lblproperty.TabIndex = 27;
this.lblproperty.Text = "";
this.txtproperty.Location = new System.Drawing.Point(173,671);
this.txtproperty.Name = "txtproperty";
this.txtproperty.Size = new System.Drawing.Size(100, 21);
this.txtproperty.TabIndex = 27;
this.Controls.Add(this.lblproperty);
this.Controls.Add(this.txtproperty);

           //#####Location_ID###Int64

           //#####1000Summary###String
this.lblSummary.AutoSize = true;
this.lblSummary.Location = new System.Drawing.Point(100,725);
this.lblSummary.Name = "lblSummary";
this.lblSummary.Size = new System.Drawing.Size(41, 12);
this.lblSummary.TabIndex = 29;
this.lblSummary.Text = "";
this.txtSummary.Location = new System.Drawing.Point(173,721);
this.txtSummary.Name = "txtSummary";
this.txtSummary.Size = new System.Drawing.Size(100, 21);
this.txtSummary.TabIndex = 29;
this.Controls.Add(this.lblSummary);
this.Controls.Add(this.txtSummary);

           //#####Quantity###Int32

           //#####DeliveredQuantity###Int32

           //#####ReworkFee###Decimal
this.lblReworkFee.AutoSize = true;
this.lblReworkFee.Location = new System.Drawing.Point(100,800);
this.lblReworkFee.Name = "lblReworkFee";
this.lblReworkFee.Size = new System.Drawing.Size(41, 12);
this.lblReworkFee.TabIndex = 32;
this.lblReworkFee.Text = "";
//111======800
this.txtReworkFee.Location = new System.Drawing.Point(173,796);
this.txtReworkFee.Name ="txtReworkFee";
this.txtReworkFee.Size = new System.Drawing.Size(100, 21);
this.txtReworkFee.TabIndex = 32;
this.Controls.Add(this.lblReworkFee);
this.Controls.Add(this.txtReworkFee);

           //#####SubtotalReworkFee###Decimal
this.lblSubtotalReworkFee.AutoSize = true;
this.lblSubtotalReworkFee.Location = new System.Drawing.Point(100,825);
this.lblSubtotalReworkFee.Name = "lblSubtotalReworkFee";
this.lblSubtotalReworkFee.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalReworkFee.TabIndex = 33;
this.lblSubtotalReworkFee.Text = "";
//111======825
this.txtSubtotalReworkFee.Location = new System.Drawing.Point(173,821);
this.txtSubtotalReworkFee.Name ="txtSubtotalReworkFee";
this.txtSubtotalReworkFee.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalReworkFee.TabIndex = 33;
this.Controls.Add(this.lblSubtotalReworkFee);
this.Controls.Add(this.txtSubtotalReworkFee);

           //#####UnitCost###Decimal
this.lblUnitCost.AutoSize = true;
this.lblUnitCost.Location = new System.Drawing.Point(100,850);
this.lblUnitCost.Name = "lblUnitCost";
this.lblUnitCost.Size = new System.Drawing.Size(41, 12);
this.lblUnitCost.TabIndex = 34;
this.lblUnitCost.Text = "";
//111======850
this.txtUnitCost.Location = new System.Drawing.Point(173,846);
this.txtUnitCost.Name ="txtUnitCost";
this.txtUnitCost.Size = new System.Drawing.Size(100, 21);
this.txtUnitCost.TabIndex = 34;
this.Controls.Add(this.lblUnitCost);
this.Controls.Add(this.txtUnitCost);

           //#####SubtotalCostAmount###Decimal
this.lblSubtotalCostAmount.AutoSize = true;
this.lblSubtotalCostAmount.Location = new System.Drawing.Point(100,875);
this.lblSubtotalCostAmount.Name = "lblSubtotalCostAmount";
this.lblSubtotalCostAmount.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalCostAmount.TabIndex = 35;
this.lblSubtotalCostAmount.Text = "";
//111======875
this.txtSubtotalCostAmount.Location = new System.Drawing.Point(173,871);
this.txtSubtotalCostAmount.Name ="txtSubtotalCostAmount";
this.txtSubtotalCostAmount.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalCostAmount.TabIndex = 35;
this.Controls.Add(this.lblSubtotalCostAmount);
this.Controls.Add(this.txtSubtotalCostAmount);

           //#####50CustomertModel###String
this.lblCustomertModel.AutoSize = true;
this.lblCustomertModel.Location = new System.Drawing.Point(100,900);
this.lblCustomertModel.Name = "lblCustomertModel";
this.lblCustomertModel.Size = new System.Drawing.Size(41, 12);
this.lblCustomertModel.TabIndex = 36;
this.lblCustomertModel.Text = "";
this.txtCustomertModel.Location = new System.Drawing.Point(173,896);
this.txtCustomertModel.Name = "txtCustomertModel";
this.txtCustomertModel.Size = new System.Drawing.Size(100, 21);
this.txtCustomertModel.TabIndex = 36;
this.Controls.Add(this.lblCustomertModel);
this.Controls.Add(this.txtCustomertModel);

           //#####40ProductNo###String
this.lblProductNo.AutoSize = true;
this.lblProductNo.Location = new System.Drawing.Point(100,925);
this.lblProductNo.Name = "lblProductNo";
this.lblProductNo.Size = new System.Drawing.Size(41, 12);
this.lblProductNo.TabIndex = 37;
this.lblProductNo.Text = "";
this.txtProductNo.Location = new System.Drawing.Point(173,921);
this.txtProductNo.Name = "txtProductNo";
this.txtProductNo.Size = new System.Drawing.Size(100, 21);
this.txtProductNo.TabIndex = 37;
this.Controls.Add(this.lblProductNo);
this.Controls.Add(this.txtProductNo);

           //#####1500Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,950);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 38;
this.lblNotes.Text = "";
this.txtNotes.Location = new System.Drawing.Point(173,946);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 38;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                
                this.Controls.Add(this.lblReworkReturnNo );
this.Controls.Add(this.txtReworkReturnNo );

                this.Controls.Add(this.lblDeliveryBillNo );
this.Controls.Add(this.txtDeliveryBillNo );

                
                
                
                this.Controls.Add(this.lblCloseCaseOpinions );
this.Controls.Add(this.txtCloseCaseOpinions );

                this.Controls.Add(this.lblApprovalOpinions );
this.Controls.Add(this.txtApprovalOpinions );

                
                this.Controls.Add(this.lblReturnDate );
this.Controls.Add(this.dtpReturnDate );

                this.Controls.Add(this.lblExpectedReturnDate );
this.Controls.Add(this.dtpExpectedReturnDate );

                this.Controls.Add(this.lblReasonForRework );
this.Controls.Add(this.txtReasonForRework );

                this.Controls.Add(this.lblApprover_at );
this.Controls.Add(this.dtpApprover_at );

                
                
                this.Controls.Add(this.lblApprovalResults );
this.Controls.Add(this.chkApprovalResults );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                
                this.Controls.Add(this.lblModified_at );
this.Controls.Add(this.dtpModified_at );

                
                
                this.Controls.Add(this.lblSKU );
this.Controls.Add(this.txtSKU );

                this.Controls.Add(this.lblSpecifications );
this.Controls.Add(this.txtSpecifications );

                this.Controls.Add(this.lblCNName );
this.Controls.Add(this.txtCNName );

                this.Controls.Add(this.lblModel );
this.Controls.Add(this.txtModel );

                
                this.Controls.Add(this.lblproperty );
this.Controls.Add(this.txtproperty );

                
                this.Controls.Add(this.lblSummary );
this.Controls.Add(this.txtSummary );

                
                
                this.Controls.Add(this.lblReworkFee );
this.Controls.Add(this.txtReworkFee );

                this.Controls.Add(this.lblSubtotalReworkFee );
this.Controls.Add(this.txtSubtotalReworkFee );

                this.Controls.Add(this.lblUnitCost );
this.Controls.Add(this.txtUnitCost );

                this.Controls.Add(this.lblSubtotalCostAmount );
this.Controls.Add(this.txtSubtotalCostAmount );

                this.Controls.Add(this.lblCustomertModel );
this.Controls.Add(this.txtCustomertModel );

                this.Controls.Add(this.lblProductNo );
this.Controls.Add(this.txtProductNo );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                    
            this.Name = "View_MRP_ReworkReturnQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblReworkReturnNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtReworkReturnNo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDeliveryBillNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtDeliveryBillNo;

    
        
              
    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCloseCaseOpinions;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCloseCaseOpinions;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprovalOpinions;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtApprovalOpinions;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblReturnDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpReturnDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblExpectedReturnDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpExpectedReturnDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblReasonForRework;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtReasonForRework;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprover_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpApprover_at;

    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprovalResults;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkApprovalResults;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModified_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpModified_at;

    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSKU;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSKU;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSpecifications;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSpecifications;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCNName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCNName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModel;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtModel;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblproperty;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtproperty;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSummary;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSummary;

    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblReworkFee;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtReworkFee;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSubtotalReworkFee;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSubtotalReworkFee;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblUnitCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtUnitCost;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSubtotalCostAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSubtotalCostAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCustomertModel;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCustomertModel;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProductNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtProductNo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
    
   
 





    }
}


