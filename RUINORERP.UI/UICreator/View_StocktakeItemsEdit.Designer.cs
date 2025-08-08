// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:40
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 盘点明细统计
    /// </summary>
    partial class View_StocktakeItemsEdit
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
     this.lblCheckNo = new Krypton.Toolkit.KryptonLabel();
this.txtCheckNo = new Krypton.Toolkit.KryptonTextBox();

this.lblLocation_ID = new Krypton.Toolkit.KryptonLabel();
this.txtLocation_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblEmployee_ID = new Krypton.Toolkit.KryptonLabel();
this.txtEmployee_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblCheckMode = new Krypton.Toolkit.KryptonLabel();
this.txtCheckMode = new Krypton.Toolkit.KryptonTextBox();

this.lblAdjust_Type = new Krypton.Toolkit.KryptonLabel();
this.txtAdjust_Type = new Krypton.Toolkit.KryptonTextBox();

this.lblCheckResult = new Krypton.Toolkit.KryptonLabel();
this.txtCheckResult = new Krypton.Toolkit.KryptonTextBox();

this.lblCheck_date = new Krypton.Toolkit.KryptonLabel();
this.dtpCheck_date = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblCarryingDate = new Krypton.Toolkit.KryptonLabel();
this.dtpCarryingDate = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblCreated_by = new Krypton.Toolkit.KryptonLabel();
this.txtCreated_by = new Krypton.Toolkit.KryptonTextBox();

this.lblCreated_at = new Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblNotes = new Krypton.Toolkit.KryptonLabel();
this.txtNotes = new Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

this.lblDataStatus = new Krypton.Toolkit.KryptonLabel();
this.txtDataStatus = new Krypton.Toolkit.KryptonTextBox();

this.lblApprovalOpinions = new Krypton.Toolkit.KryptonLabel();
this.txtApprovalOpinions = new Krypton.Toolkit.KryptonTextBox();
this.txtApprovalOpinions.Multiline = true;

this.lblApprovalResults = new Krypton.Toolkit.KryptonLabel();
this.chkApprovalResults = new Krypton.Toolkit.KryptonCheckBox();
this.chkApprovalResults.Values.Text ="";


this.lblSKU = new Krypton.Toolkit.KryptonLabel();
this.txtSKU = new Krypton.Toolkit.KryptonTextBox();

this.lblCNName = new Krypton.Toolkit.KryptonLabel();
this.txtCNName = new Krypton.Toolkit.KryptonTextBox();
this.txtCNName.Multiline = true;

this.lblSpecifications = new Krypton.Toolkit.KryptonLabel();
this.txtSpecifications = new Krypton.Toolkit.KryptonTextBox();
this.txtSpecifications.Multiline = true;

this.lblProductNo = new Krypton.Toolkit.KryptonLabel();
this.txtProductNo = new Krypton.Toolkit.KryptonTextBox();

this.lblModel = new Krypton.Toolkit.KryptonLabel();
this.txtModel = new Krypton.Toolkit.KryptonTextBox();

this.lblCategory_ID = new Krypton.Toolkit.KryptonLabel();
this.txtCategory_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblType_ID = new Krypton.Toolkit.KryptonLabel();
this.txtType_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblUnit_ID = new Krypton.Toolkit.KryptonLabel();
this.txtUnit_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblProdDetailID = new Krypton.Toolkit.KryptonLabel();
this.txtProdDetailID = new Krypton.Toolkit.KryptonTextBox();

this.lblRack_ID = new Krypton.Toolkit.KryptonLabel();
this.txtRack_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblCost = new Krypton.Toolkit.KryptonLabel();
this.txtCost = new Krypton.Toolkit.KryptonTextBox();

this.lblCarryinglQty = new Krypton.Toolkit.KryptonLabel();
this.txtCarryinglQty = new Krypton.Toolkit.KryptonTextBox();

this.lblCarryingSubtotalAmount = new Krypton.Toolkit.KryptonLabel();
this.txtCarryingSubtotalAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblDiffQty = new Krypton.Toolkit.KryptonLabel();
this.txtDiffQty = new Krypton.Toolkit.KryptonTextBox();

this.lblDiffSubtotalAmount = new Krypton.Toolkit.KryptonLabel();
this.txtDiffSubtotalAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblCheckQty = new Krypton.Toolkit.KryptonLabel();
this.txtCheckQty = new Krypton.Toolkit.KryptonTextBox();

this.lblCheckSubtotalAmount = new Krypton.Toolkit.KryptonLabel();
this.txtCheckSubtotalAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblproperty = new Krypton.Toolkit.KryptonLabel();
this.txtproperty = new Krypton.Toolkit.KryptonTextBox();
this.txtproperty.Multiline = true;

    
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
     
            //#####50CheckNo###String
this.lblCheckNo.AutoSize = true;
this.lblCheckNo.Location = new System.Drawing.Point(100,25);
this.lblCheckNo.Name = "lblCheckNo";
this.lblCheckNo.Size = new System.Drawing.Size(41, 12);
this.lblCheckNo.TabIndex = 1;
this.lblCheckNo.Text = "";
this.txtCheckNo.Location = new System.Drawing.Point(173,21);
this.txtCheckNo.Name = "txtCheckNo";
this.txtCheckNo.Size = new System.Drawing.Size(100, 21);
this.txtCheckNo.TabIndex = 1;
this.Controls.Add(this.lblCheckNo);
this.Controls.Add(this.txtCheckNo);

           //#####Location_ID###Int64
this.lblLocation_ID.AutoSize = true;
this.lblLocation_ID.Location = new System.Drawing.Point(100,50);
this.lblLocation_ID.Name = "lblLocation_ID";
this.lblLocation_ID.Size = new System.Drawing.Size(41, 12);
this.lblLocation_ID.TabIndex = 2;
this.lblLocation_ID.Text = "";
this.txtLocation_ID.Location = new System.Drawing.Point(173,46);
this.txtLocation_ID.Name = "txtLocation_ID";
this.txtLocation_ID.Size = new System.Drawing.Size(100, 21);
this.txtLocation_ID.TabIndex = 2;
this.Controls.Add(this.lblLocation_ID);
this.Controls.Add(this.txtLocation_ID);

           //#####Employee_ID###Int64
this.lblEmployee_ID.AutoSize = true;
this.lblEmployee_ID.Location = new System.Drawing.Point(100,75);
this.lblEmployee_ID.Name = "lblEmployee_ID";
this.lblEmployee_ID.Size = new System.Drawing.Size(41, 12);
this.lblEmployee_ID.TabIndex = 3;
this.lblEmployee_ID.Text = "";
this.txtEmployee_ID.Location = new System.Drawing.Point(173,71);
this.txtEmployee_ID.Name = "txtEmployee_ID";
this.txtEmployee_ID.Size = new System.Drawing.Size(100, 21);
this.txtEmployee_ID.TabIndex = 3;
this.Controls.Add(this.lblEmployee_ID);
this.Controls.Add(this.txtEmployee_ID);

           //#####CheckMode###Int32
this.lblCheckMode.AutoSize = true;
this.lblCheckMode.Location = new System.Drawing.Point(100,100);
this.lblCheckMode.Name = "lblCheckMode";
this.lblCheckMode.Size = new System.Drawing.Size(41, 12);
this.lblCheckMode.TabIndex = 4;
this.lblCheckMode.Text = "";
this.txtCheckMode.Location = new System.Drawing.Point(173,96);
this.txtCheckMode.Name = "txtCheckMode";
this.txtCheckMode.Size = new System.Drawing.Size(100, 21);
this.txtCheckMode.TabIndex = 4;
this.Controls.Add(this.lblCheckMode);
this.Controls.Add(this.txtCheckMode);

           //#####Adjust_Type###Int32
this.lblAdjust_Type.AutoSize = true;
this.lblAdjust_Type.Location = new System.Drawing.Point(100,125);
this.lblAdjust_Type.Name = "lblAdjust_Type";
this.lblAdjust_Type.Size = new System.Drawing.Size(41, 12);
this.lblAdjust_Type.TabIndex = 5;
this.lblAdjust_Type.Text = "";
this.txtAdjust_Type.Location = new System.Drawing.Point(173,121);
this.txtAdjust_Type.Name = "txtAdjust_Type";
this.txtAdjust_Type.Size = new System.Drawing.Size(100, 21);
this.txtAdjust_Type.TabIndex = 5;
this.Controls.Add(this.lblAdjust_Type);
this.Controls.Add(this.txtAdjust_Type);

           //#####CheckResult###Int32
this.lblCheckResult.AutoSize = true;
this.lblCheckResult.Location = new System.Drawing.Point(100,150);
this.lblCheckResult.Name = "lblCheckResult";
this.lblCheckResult.Size = new System.Drawing.Size(41, 12);
this.lblCheckResult.TabIndex = 6;
this.lblCheckResult.Text = "";
this.txtCheckResult.Location = new System.Drawing.Point(173,146);
this.txtCheckResult.Name = "txtCheckResult";
this.txtCheckResult.Size = new System.Drawing.Size(100, 21);
this.txtCheckResult.TabIndex = 6;
this.Controls.Add(this.lblCheckResult);
this.Controls.Add(this.txtCheckResult);

           //#####Check_date###DateTime
this.lblCheck_date.AutoSize = true;
this.lblCheck_date.Location = new System.Drawing.Point(100,175);
this.lblCheck_date.Name = "lblCheck_date";
this.lblCheck_date.Size = new System.Drawing.Size(41, 12);
this.lblCheck_date.TabIndex = 7;
this.lblCheck_date.Text = "";
//111======175
this.dtpCheck_date.Location = new System.Drawing.Point(173,171);
this.dtpCheck_date.Name ="dtpCheck_date";
this.dtpCheck_date.ShowCheckBox =true;
this.dtpCheck_date.Size = new System.Drawing.Size(100, 21);
this.dtpCheck_date.TabIndex = 7;
this.Controls.Add(this.lblCheck_date);
this.Controls.Add(this.dtpCheck_date);

           //#####CarryingDate###DateTime
this.lblCarryingDate.AutoSize = true;
this.lblCarryingDate.Location = new System.Drawing.Point(100,200);
this.lblCarryingDate.Name = "lblCarryingDate";
this.lblCarryingDate.Size = new System.Drawing.Size(41, 12);
this.lblCarryingDate.TabIndex = 8;
this.lblCarryingDate.Text = "";
//111======200
this.dtpCarryingDate.Location = new System.Drawing.Point(173,196);
this.dtpCarryingDate.Name ="dtpCarryingDate";
this.dtpCarryingDate.ShowCheckBox =true;
this.dtpCarryingDate.Size = new System.Drawing.Size(100, 21);
this.dtpCarryingDate.TabIndex = 8;
this.Controls.Add(this.lblCarryingDate);
this.Controls.Add(this.dtpCarryingDate);

           //#####Created_by###Int64
this.lblCreated_by.AutoSize = true;
this.lblCreated_by.Location = new System.Drawing.Point(100,225);
this.lblCreated_by.Name = "lblCreated_by";
this.lblCreated_by.Size = new System.Drawing.Size(41, 12);
this.lblCreated_by.TabIndex = 9;
this.lblCreated_by.Text = "";
this.txtCreated_by.Location = new System.Drawing.Point(173,221);
this.txtCreated_by.Name = "txtCreated_by";
this.txtCreated_by.Size = new System.Drawing.Size(100, 21);
this.txtCreated_by.TabIndex = 9;
this.Controls.Add(this.lblCreated_by);
this.Controls.Add(this.txtCreated_by);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,250);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 10;
this.lblCreated_at.Text = "";
//111======250
this.dtpCreated_at.Location = new System.Drawing.Point(173,246);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 10;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####1000Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,275);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 11;
this.lblNotes.Text = "";
this.txtNotes.Location = new System.Drawing.Point(173,271);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 11;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####DataStatus###Int32
this.lblDataStatus.AutoSize = true;
this.lblDataStatus.Location = new System.Drawing.Point(100,300);
this.lblDataStatus.Name = "lblDataStatus";
this.lblDataStatus.Size = new System.Drawing.Size(41, 12);
this.lblDataStatus.TabIndex = 12;
this.lblDataStatus.Text = "";
this.txtDataStatus.Location = new System.Drawing.Point(173,296);
this.txtDataStatus.Name = "txtDataStatus";
this.txtDataStatus.Size = new System.Drawing.Size(100, 21);
this.txtDataStatus.TabIndex = 12;
this.Controls.Add(this.lblDataStatus);
this.Controls.Add(this.txtDataStatus);

           //#####500ApprovalOpinions###String
this.lblApprovalOpinions.AutoSize = true;
this.lblApprovalOpinions.Location = new System.Drawing.Point(100,325);
this.lblApprovalOpinions.Name = "lblApprovalOpinions";
this.lblApprovalOpinions.Size = new System.Drawing.Size(41, 12);
this.lblApprovalOpinions.TabIndex = 13;
this.lblApprovalOpinions.Text = "";
this.txtApprovalOpinions.Location = new System.Drawing.Point(173,321);
this.txtApprovalOpinions.Name = "txtApprovalOpinions";
this.txtApprovalOpinions.Size = new System.Drawing.Size(100, 21);
this.txtApprovalOpinions.TabIndex = 13;
this.Controls.Add(this.lblApprovalOpinions);
this.Controls.Add(this.txtApprovalOpinions);

           //#####ApprovalResults###Boolean
this.lblApprovalResults.AutoSize = true;
this.lblApprovalResults.Location = new System.Drawing.Point(100,350);
this.lblApprovalResults.Name = "lblApprovalResults";
this.lblApprovalResults.Size = new System.Drawing.Size(41, 12);
this.lblApprovalResults.TabIndex = 14;
this.lblApprovalResults.Text = "";
this.chkApprovalResults.Location = new System.Drawing.Point(173,346);
this.chkApprovalResults.Name = "chkApprovalResults";
this.chkApprovalResults.Size = new System.Drawing.Size(100, 21);
this.chkApprovalResults.TabIndex = 14;
this.Controls.Add(this.lblApprovalResults);
this.Controls.Add(this.chkApprovalResults);

           //#####ApprovalStatus###SByte

           //#####80SKU###String
this.lblSKU.AutoSize = true;
this.lblSKU.Location = new System.Drawing.Point(100,400);
this.lblSKU.Name = "lblSKU";
this.lblSKU.Size = new System.Drawing.Size(41, 12);
this.lblSKU.TabIndex = 16;
this.lblSKU.Text = "";
this.txtSKU.Location = new System.Drawing.Point(173,396);
this.txtSKU.Name = "txtSKU";
this.txtSKU.Size = new System.Drawing.Size(100, 21);
this.txtSKU.TabIndex = 16;
this.Controls.Add(this.lblSKU);
this.Controls.Add(this.txtSKU);

           //#####255CNName###String
this.lblCNName.AutoSize = true;
this.lblCNName.Location = new System.Drawing.Point(100,425);
this.lblCNName.Name = "lblCNName";
this.lblCNName.Size = new System.Drawing.Size(41, 12);
this.lblCNName.TabIndex = 17;
this.lblCNName.Text = "";
this.txtCNName.Location = new System.Drawing.Point(173,421);
this.txtCNName.Name = "txtCNName";
this.txtCNName.Size = new System.Drawing.Size(100, 21);
this.txtCNName.TabIndex = 17;
this.Controls.Add(this.lblCNName);
this.Controls.Add(this.txtCNName);

           //#####1000Specifications###String
this.lblSpecifications.AutoSize = true;
this.lblSpecifications.Location = new System.Drawing.Point(100,450);
this.lblSpecifications.Name = "lblSpecifications";
this.lblSpecifications.Size = new System.Drawing.Size(41, 12);
this.lblSpecifications.TabIndex = 18;
this.lblSpecifications.Text = "";
this.txtSpecifications.Location = new System.Drawing.Point(173,446);
this.txtSpecifications.Name = "txtSpecifications";
this.txtSpecifications.Size = new System.Drawing.Size(100, 21);
this.txtSpecifications.TabIndex = 18;
this.Controls.Add(this.lblSpecifications);
this.Controls.Add(this.txtSpecifications);

           //#####40ProductNo###String
this.lblProductNo.AutoSize = true;
this.lblProductNo.Location = new System.Drawing.Point(100,475);
this.lblProductNo.Name = "lblProductNo";
this.lblProductNo.Size = new System.Drawing.Size(41, 12);
this.lblProductNo.TabIndex = 19;
this.lblProductNo.Text = "";
this.txtProductNo.Location = new System.Drawing.Point(173,471);
this.txtProductNo.Name = "txtProductNo";
this.txtProductNo.Size = new System.Drawing.Size(100, 21);
this.txtProductNo.TabIndex = 19;
this.Controls.Add(this.lblProductNo);
this.Controls.Add(this.txtProductNo);

           //#####50Model###String
this.lblModel.AutoSize = true;
this.lblModel.Location = new System.Drawing.Point(100,500);
this.lblModel.Name = "lblModel";
this.lblModel.Size = new System.Drawing.Size(41, 12);
this.lblModel.TabIndex = 20;
this.lblModel.Text = "";
this.txtModel.Location = new System.Drawing.Point(173,496);
this.txtModel.Name = "txtModel";
this.txtModel.Size = new System.Drawing.Size(100, 21);
this.txtModel.TabIndex = 20;
this.Controls.Add(this.lblModel);
this.Controls.Add(this.txtModel);

           //#####Category_ID###Int64
this.lblCategory_ID.AutoSize = true;
this.lblCategory_ID.Location = new System.Drawing.Point(100,525);
this.lblCategory_ID.Name = "lblCategory_ID";
this.lblCategory_ID.Size = new System.Drawing.Size(41, 12);
this.lblCategory_ID.TabIndex = 21;
this.lblCategory_ID.Text = "";
this.txtCategory_ID.Location = new System.Drawing.Point(173,521);
this.txtCategory_ID.Name = "txtCategory_ID";
this.txtCategory_ID.Size = new System.Drawing.Size(100, 21);
this.txtCategory_ID.TabIndex = 21;
this.Controls.Add(this.lblCategory_ID);
this.Controls.Add(this.txtCategory_ID);

           //#####Type_ID###Int64
this.lblType_ID.AutoSize = true;
this.lblType_ID.Location = new System.Drawing.Point(100,550);
this.lblType_ID.Name = "lblType_ID";
this.lblType_ID.Size = new System.Drawing.Size(41, 12);
this.lblType_ID.TabIndex = 22;
this.lblType_ID.Text = "";
this.txtType_ID.Location = new System.Drawing.Point(173,546);
this.txtType_ID.Name = "txtType_ID";
this.txtType_ID.Size = new System.Drawing.Size(100, 21);
this.txtType_ID.TabIndex = 22;
this.Controls.Add(this.lblType_ID);
this.Controls.Add(this.txtType_ID);

           //#####Unit_ID###Int64
this.lblUnit_ID.AutoSize = true;
this.lblUnit_ID.Location = new System.Drawing.Point(100,575);
this.lblUnit_ID.Name = "lblUnit_ID";
this.lblUnit_ID.Size = new System.Drawing.Size(41, 12);
this.lblUnit_ID.TabIndex = 23;
this.lblUnit_ID.Text = "";
this.txtUnit_ID.Location = new System.Drawing.Point(173,571);
this.txtUnit_ID.Name = "txtUnit_ID";
this.txtUnit_ID.Size = new System.Drawing.Size(100, 21);
this.txtUnit_ID.TabIndex = 23;
this.Controls.Add(this.lblUnit_ID);
this.Controls.Add(this.txtUnit_ID);

           //#####ProdDetailID###Int64
this.lblProdDetailID.AutoSize = true;
this.lblProdDetailID.Location = new System.Drawing.Point(100,600);
this.lblProdDetailID.Name = "lblProdDetailID";
this.lblProdDetailID.Size = new System.Drawing.Size(41, 12);
this.lblProdDetailID.TabIndex = 24;
this.lblProdDetailID.Text = "";
this.txtProdDetailID.Location = new System.Drawing.Point(173,596);
this.txtProdDetailID.Name = "txtProdDetailID";
this.txtProdDetailID.Size = new System.Drawing.Size(100, 21);
this.txtProdDetailID.TabIndex = 24;
this.Controls.Add(this.lblProdDetailID);
this.Controls.Add(this.txtProdDetailID);

           //#####Rack_ID###Int64
this.lblRack_ID.AutoSize = true;
this.lblRack_ID.Location = new System.Drawing.Point(100,625);
this.lblRack_ID.Name = "lblRack_ID";
this.lblRack_ID.Size = new System.Drawing.Size(41, 12);
this.lblRack_ID.TabIndex = 25;
this.lblRack_ID.Text = "";
this.txtRack_ID.Location = new System.Drawing.Point(173,621);
this.txtRack_ID.Name = "txtRack_ID";
this.txtRack_ID.Size = new System.Drawing.Size(100, 21);
this.txtRack_ID.TabIndex = 25;
this.Controls.Add(this.lblRack_ID);
this.Controls.Add(this.txtRack_ID);

           //#####Cost###Decimal
this.lblCost.AutoSize = true;
this.lblCost.Location = new System.Drawing.Point(100,650);
this.lblCost.Name = "lblCost";
this.lblCost.Size = new System.Drawing.Size(41, 12);
this.lblCost.TabIndex = 26;
this.lblCost.Text = "";
//111======650
this.txtCost.Location = new System.Drawing.Point(173,646);
this.txtCost.Name ="txtCost";
this.txtCost.Size = new System.Drawing.Size(100, 21);
this.txtCost.TabIndex = 26;
this.Controls.Add(this.lblCost);
this.Controls.Add(this.txtCost);

           //#####CarryinglQty###Int32
this.lblCarryinglQty.AutoSize = true;
this.lblCarryinglQty.Location = new System.Drawing.Point(100,675);
this.lblCarryinglQty.Name = "lblCarryinglQty";
this.lblCarryinglQty.Size = new System.Drawing.Size(41, 12);
this.lblCarryinglQty.TabIndex = 27;
this.lblCarryinglQty.Text = "";
this.txtCarryinglQty.Location = new System.Drawing.Point(173,671);
this.txtCarryinglQty.Name = "txtCarryinglQty";
this.txtCarryinglQty.Size = new System.Drawing.Size(100, 21);
this.txtCarryinglQty.TabIndex = 27;
this.Controls.Add(this.lblCarryinglQty);
this.Controls.Add(this.txtCarryinglQty);

           //#####CarryingSubtotalAmount###Decimal
this.lblCarryingSubtotalAmount.AutoSize = true;
this.lblCarryingSubtotalAmount.Location = new System.Drawing.Point(100,700);
this.lblCarryingSubtotalAmount.Name = "lblCarryingSubtotalAmount";
this.lblCarryingSubtotalAmount.Size = new System.Drawing.Size(41, 12);
this.lblCarryingSubtotalAmount.TabIndex = 28;
this.lblCarryingSubtotalAmount.Text = "";
//111======700
this.txtCarryingSubtotalAmount.Location = new System.Drawing.Point(173,696);
this.txtCarryingSubtotalAmount.Name ="txtCarryingSubtotalAmount";
this.txtCarryingSubtotalAmount.Size = new System.Drawing.Size(100, 21);
this.txtCarryingSubtotalAmount.TabIndex = 28;
this.Controls.Add(this.lblCarryingSubtotalAmount);
this.Controls.Add(this.txtCarryingSubtotalAmount);

           //#####DiffQty###Int32
this.lblDiffQty.AutoSize = true;
this.lblDiffQty.Location = new System.Drawing.Point(100,725);
this.lblDiffQty.Name = "lblDiffQty";
this.lblDiffQty.Size = new System.Drawing.Size(41, 12);
this.lblDiffQty.TabIndex = 29;
this.lblDiffQty.Text = "";
this.txtDiffQty.Location = new System.Drawing.Point(173,721);
this.txtDiffQty.Name = "txtDiffQty";
this.txtDiffQty.Size = new System.Drawing.Size(100, 21);
this.txtDiffQty.TabIndex = 29;
this.Controls.Add(this.lblDiffQty);
this.Controls.Add(this.txtDiffQty);

           //#####DiffSubtotalAmount###Decimal
this.lblDiffSubtotalAmount.AutoSize = true;
this.lblDiffSubtotalAmount.Location = new System.Drawing.Point(100,750);
this.lblDiffSubtotalAmount.Name = "lblDiffSubtotalAmount";
this.lblDiffSubtotalAmount.Size = new System.Drawing.Size(41, 12);
this.lblDiffSubtotalAmount.TabIndex = 30;
this.lblDiffSubtotalAmount.Text = "";
//111======750
this.txtDiffSubtotalAmount.Location = new System.Drawing.Point(173,746);
this.txtDiffSubtotalAmount.Name ="txtDiffSubtotalAmount";
this.txtDiffSubtotalAmount.Size = new System.Drawing.Size(100, 21);
this.txtDiffSubtotalAmount.TabIndex = 30;
this.Controls.Add(this.lblDiffSubtotalAmount);
this.Controls.Add(this.txtDiffSubtotalAmount);

           //#####CheckQty###Int32
this.lblCheckQty.AutoSize = true;
this.lblCheckQty.Location = new System.Drawing.Point(100,775);
this.lblCheckQty.Name = "lblCheckQty";
this.lblCheckQty.Size = new System.Drawing.Size(41, 12);
this.lblCheckQty.TabIndex = 31;
this.lblCheckQty.Text = "";
this.txtCheckQty.Location = new System.Drawing.Point(173,771);
this.txtCheckQty.Name = "txtCheckQty";
this.txtCheckQty.Size = new System.Drawing.Size(100, 21);
this.txtCheckQty.TabIndex = 31;
this.Controls.Add(this.lblCheckQty);
this.Controls.Add(this.txtCheckQty);

           //#####CheckSubtotalAmount###Decimal
this.lblCheckSubtotalAmount.AutoSize = true;
this.lblCheckSubtotalAmount.Location = new System.Drawing.Point(100,800);
this.lblCheckSubtotalAmount.Name = "lblCheckSubtotalAmount";
this.lblCheckSubtotalAmount.Size = new System.Drawing.Size(41, 12);
this.lblCheckSubtotalAmount.TabIndex = 32;
this.lblCheckSubtotalAmount.Text = "";
//111======800
this.txtCheckSubtotalAmount.Location = new System.Drawing.Point(173,796);
this.txtCheckSubtotalAmount.Name ="txtCheckSubtotalAmount";
this.txtCheckSubtotalAmount.Size = new System.Drawing.Size(100, 21);
this.txtCheckSubtotalAmount.TabIndex = 32;
this.Controls.Add(this.lblCheckSubtotalAmount);
this.Controls.Add(this.txtCheckSubtotalAmount);

           //#####255property###String
this.lblproperty.AutoSize = true;
this.lblproperty.Location = new System.Drawing.Point(100,825);
this.lblproperty.Name = "lblproperty";
this.lblproperty.Size = new System.Drawing.Size(41, 12);
this.lblproperty.TabIndex = 33;
this.lblproperty.Text = "";
this.txtproperty.Location = new System.Drawing.Point(173,821);
this.txtproperty.Name = "txtproperty";
this.txtproperty.Size = new System.Drawing.Size(100, 21);
this.txtproperty.TabIndex = 33;
this.Controls.Add(this.lblproperty);
this.Controls.Add(this.txtproperty);

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
           // this.kryptonPanel1.TabIndex = 33;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblCheckNo );
this.Controls.Add(this.txtCheckNo );

                this.Controls.Add(this.lblLocation_ID );
this.Controls.Add(this.txtLocation_ID );

                this.Controls.Add(this.lblEmployee_ID );
this.Controls.Add(this.txtEmployee_ID );

                this.Controls.Add(this.lblCheckMode );
this.Controls.Add(this.txtCheckMode );

                this.Controls.Add(this.lblAdjust_Type );
this.Controls.Add(this.txtAdjust_Type );

                this.Controls.Add(this.lblCheckResult );
this.Controls.Add(this.txtCheckResult );

                this.Controls.Add(this.lblCheck_date );
this.Controls.Add(this.dtpCheck_date );

                this.Controls.Add(this.lblCarryingDate );
this.Controls.Add(this.dtpCarryingDate );

                this.Controls.Add(this.lblCreated_by );
this.Controls.Add(this.txtCreated_by );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblDataStatus );
this.Controls.Add(this.txtDataStatus );

                this.Controls.Add(this.lblApprovalOpinions );
this.Controls.Add(this.txtApprovalOpinions );

                this.Controls.Add(this.lblApprovalResults );
this.Controls.Add(this.chkApprovalResults );

                
                this.Controls.Add(this.lblSKU );
this.Controls.Add(this.txtSKU );

                this.Controls.Add(this.lblCNName );
this.Controls.Add(this.txtCNName );

                this.Controls.Add(this.lblSpecifications );
this.Controls.Add(this.txtSpecifications );

                this.Controls.Add(this.lblProductNo );
this.Controls.Add(this.txtProductNo );

                this.Controls.Add(this.lblModel );
this.Controls.Add(this.txtModel );

                this.Controls.Add(this.lblCategory_ID );
this.Controls.Add(this.txtCategory_ID );

                this.Controls.Add(this.lblType_ID );
this.Controls.Add(this.txtType_ID );

                this.Controls.Add(this.lblUnit_ID );
this.Controls.Add(this.txtUnit_ID );

                this.Controls.Add(this.lblProdDetailID );
this.Controls.Add(this.txtProdDetailID );

                this.Controls.Add(this.lblRack_ID );
this.Controls.Add(this.txtRack_ID );

                this.Controls.Add(this.lblCost );
this.Controls.Add(this.txtCost );

                this.Controls.Add(this.lblCarryinglQty );
this.Controls.Add(this.txtCarryinglQty );

                this.Controls.Add(this.lblCarryingSubtotalAmount );
this.Controls.Add(this.txtCarryingSubtotalAmount );

                this.Controls.Add(this.lblDiffQty );
this.Controls.Add(this.txtDiffQty );

                this.Controls.Add(this.lblDiffSubtotalAmount );
this.Controls.Add(this.txtDiffSubtotalAmount );

                this.Controls.Add(this.lblCheckQty );
this.Controls.Add(this.txtCheckQty );

                this.Controls.Add(this.lblCheckSubtotalAmount );
this.Controls.Add(this.txtCheckSubtotalAmount );

                this.Controls.Add(this.lblproperty );
this.Controls.Add(this.txtproperty );

                            // 
            // "View_StocktakeItemsEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "View_StocktakeItemsEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblCheckNo;
private Krypton.Toolkit.KryptonTextBox txtCheckNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblLocation_ID;
private Krypton.Toolkit.KryptonTextBox txtLocation_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private Krypton.Toolkit.KryptonTextBox txtEmployee_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblCheckMode;
private Krypton.Toolkit.KryptonTextBox txtCheckMode;

    
        
              private Krypton.Toolkit.KryptonLabel lblAdjust_Type;
private Krypton.Toolkit.KryptonTextBox txtAdjust_Type;

    
        
              private Krypton.Toolkit.KryptonLabel lblCheckResult;
private Krypton.Toolkit.KryptonTextBox txtCheckResult;

    
        
              private Krypton.Toolkit.KryptonLabel lblCheck_date;
private Krypton.Toolkit.KryptonDateTimePicker dtpCheck_date;

    
        
              private Krypton.Toolkit.KryptonLabel lblCarryingDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpCarryingDate;

    
        
              private Krypton.Toolkit.KryptonLabel lblCreated_by;
private Krypton.Toolkit.KryptonTextBox txtCreated_by;

    
        
              private Krypton.Toolkit.KryptonLabel lblCreated_at;
private Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              private Krypton.Toolkit.KryptonLabel lblNotes;
private Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private Krypton.Toolkit.KryptonLabel lblDataStatus;
private Krypton.Toolkit.KryptonTextBox txtDataStatus;

    
        
              private Krypton.Toolkit.KryptonLabel lblApprovalOpinions;
private Krypton.Toolkit.KryptonTextBox txtApprovalOpinions;

    
        
              private Krypton.Toolkit.KryptonLabel lblApprovalResults;
private Krypton.Toolkit.KryptonCheckBox chkApprovalResults;

    
        
              
    
        
              private Krypton.Toolkit.KryptonLabel lblSKU;
private Krypton.Toolkit.KryptonTextBox txtSKU;

    
        
              private Krypton.Toolkit.KryptonLabel lblCNName;
private Krypton.Toolkit.KryptonTextBox txtCNName;

    
        
              private Krypton.Toolkit.KryptonLabel lblSpecifications;
private Krypton.Toolkit.KryptonTextBox txtSpecifications;

    
        
              private Krypton.Toolkit.KryptonLabel lblProductNo;
private Krypton.Toolkit.KryptonTextBox txtProductNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblModel;
private Krypton.Toolkit.KryptonTextBox txtModel;

    
        
              private Krypton.Toolkit.KryptonLabel lblCategory_ID;
private Krypton.Toolkit.KryptonTextBox txtCategory_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblType_ID;
private Krypton.Toolkit.KryptonTextBox txtType_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblUnit_ID;
private Krypton.Toolkit.KryptonTextBox txtUnit_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblProdDetailID;
private Krypton.Toolkit.KryptonTextBox txtProdDetailID;

    
        
              private Krypton.Toolkit.KryptonLabel lblRack_ID;
private Krypton.Toolkit.KryptonTextBox txtRack_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblCost;
private Krypton.Toolkit.KryptonTextBox txtCost;

    
        
              private Krypton.Toolkit.KryptonLabel lblCarryinglQty;
private Krypton.Toolkit.KryptonTextBox txtCarryinglQty;

    
        
              private Krypton.Toolkit.KryptonLabel lblCarryingSubtotalAmount;
private Krypton.Toolkit.KryptonTextBox txtCarryingSubtotalAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblDiffQty;
private Krypton.Toolkit.KryptonTextBox txtDiffQty;

    
        
              private Krypton.Toolkit.KryptonLabel lblDiffSubtotalAmount;
private Krypton.Toolkit.KryptonTextBox txtDiffSubtotalAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblCheckQty;
private Krypton.Toolkit.KryptonTextBox txtCheckQty;

    
        
              private Krypton.Toolkit.KryptonLabel lblCheckSubtotalAmount;
private Krypton.Toolkit.KryptonTextBox txtCheckSubtotalAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblproperty;
private Krypton.Toolkit.KryptonTextBox txtproperty;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

