
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:30
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 生产领料统计
    /// </summary>
    partial class View_MaterialRequisitionItemsQuery
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





this.lblCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


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



this.lblproperty = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtproperty = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtproperty.Multiline = true;




this.lblSummary = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSummary = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtSummary.Multiline = true;

this.lblCustomerPartNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCustomerPartNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblPrice = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPrice = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSubtotalPrice = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSubtotalPrice = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSubtotalCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSubtotalCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();



this.lblSKU = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSKU = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblCNName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCNName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtCNName.Multiline = true;

this.lblSpecifications = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSpecifications = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtSpecifications.Multiline = true;

this.lblProductNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtProductNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblModel = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtModel = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();



this.lblShortCode = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtShortCode = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblBarCode = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtBarCode = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####50MaterialRequisitionNO###String
this.lblMaterialRequisitionNO.AutoSize = true;
this.lblMaterialRequisitionNO.Location = new System.Drawing.Point(100,25);
this.lblMaterialRequisitionNO.Name = "lblMaterialRequisitionNO";
this.lblMaterialRequisitionNO.Size = new System.Drawing.Size(41, 12);
this.lblMaterialRequisitionNO.TabIndex = 1;
this.lblMaterialRequisitionNO.Text = "";
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
this.lblMONO.Text = "";
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
this.lblDeliveryDate.Text = "";
//111======75
this.dtpDeliveryDate.Location = new System.Drawing.Point(173,71);
this.dtpDeliveryDate.Name ="dtpDeliveryDate";
this.dtpDeliveryDate.ShowCheckBox =true;
this.dtpDeliveryDate.Size = new System.Drawing.Size(100, 21);
this.dtpDeliveryDate.TabIndex = 3;
this.Controls.Add(this.lblDeliveryDate);
this.Controls.Add(this.dtpDeliveryDate);

           //#####Employee_ID###Int64

           //#####DepartmentID###Int64

           //#####CustomerVendor_ID###Int64

           //#####ProjectGroup_ID###Int64

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,200);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 8;
this.lblCreated_at.Text = "";
//111======200
this.dtpCreated_at.Location = new System.Drawing.Point(173,196);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 8;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64

           //#####255Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,250);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 10;
this.lblNotes.Text = "";
this.txtNotes.Location = new System.Drawing.Point(173,246);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 10;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####200ApprovalOpinions###String
this.lblApprovalOpinions.AutoSize = true;
this.lblApprovalOpinions.Location = new System.Drawing.Point(100,275);
this.lblApprovalOpinions.Name = "lblApprovalOpinions";
this.lblApprovalOpinions.Size = new System.Drawing.Size(41, 12);
this.lblApprovalOpinions.TabIndex = 11;
this.lblApprovalOpinions.Text = "";
this.txtApprovalOpinions.Location = new System.Drawing.Point(173,271);
this.txtApprovalOpinions.Name = "txtApprovalOpinions";
this.txtApprovalOpinions.Size = new System.Drawing.Size(100, 21);
this.txtApprovalOpinions.TabIndex = 11;
this.Controls.Add(this.lblApprovalOpinions);
this.Controls.Add(this.txtApprovalOpinions);

           //#####Approver_by###Int64

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

           //#####ApprovalStatus###SByte

           //#####ApprovalResults###Boolean
this.lblApprovalResults.AutoSize = true;
this.lblApprovalResults.Location = new System.Drawing.Point(100,375);
this.lblApprovalResults.Name = "lblApprovalResults";
this.lblApprovalResults.Size = new System.Drawing.Size(41, 12);
this.lblApprovalResults.TabIndex = 15;
this.lblApprovalResults.Text = "";
this.chkApprovalResults.Location = new System.Drawing.Point(173,371);
this.chkApprovalResults.Name = "chkApprovalResults";
this.chkApprovalResults.Size = new System.Drawing.Size(100, 21);
this.chkApprovalResults.TabIndex = 15;
this.Controls.Add(this.lblApprovalResults);
this.Controls.Add(this.chkApprovalResults);

           //#####DataStatus###Int32

           //#####ProdDetailID###Int64

           //#####255property###String
this.lblproperty.AutoSize = true;
this.lblproperty.Location = new System.Drawing.Point(100,450);
this.lblproperty.Name = "lblproperty";
this.lblproperty.Size = new System.Drawing.Size(41, 12);
this.lblproperty.TabIndex = 18;
this.lblproperty.Text = "";
this.txtproperty.Location = new System.Drawing.Point(173,446);
this.txtproperty.Name = "txtproperty";
this.txtproperty.Size = new System.Drawing.Size(100, 21);
this.txtproperty.TabIndex = 18;
this.Controls.Add(this.lblproperty);
this.Controls.Add(this.txtproperty);

           //#####ShouldSendQty###Int32

           //#####ActualSentQty###Int32

           //#####CanQuantity###Int32

           //#####255Summary###String
this.lblSummary.AutoSize = true;
this.lblSummary.Location = new System.Drawing.Point(100,550);
this.lblSummary.Name = "lblSummary";
this.lblSummary.Size = new System.Drawing.Size(41, 12);
this.lblSummary.TabIndex = 22;
this.lblSummary.Text = "";
this.txtSummary.Location = new System.Drawing.Point(173,546);
this.txtSummary.Name = "txtSummary";
this.txtSummary.Size = new System.Drawing.Size(100, 21);
this.txtSummary.TabIndex = 22;
this.Controls.Add(this.lblSummary);
this.Controls.Add(this.txtSummary);

           //#####50CustomerPartNo###String
this.lblCustomerPartNo.AutoSize = true;
this.lblCustomerPartNo.Location = new System.Drawing.Point(100,575);
this.lblCustomerPartNo.Name = "lblCustomerPartNo";
this.lblCustomerPartNo.Size = new System.Drawing.Size(41, 12);
this.lblCustomerPartNo.TabIndex = 23;
this.lblCustomerPartNo.Text = "";
this.txtCustomerPartNo.Location = new System.Drawing.Point(173,571);
this.txtCustomerPartNo.Name = "txtCustomerPartNo";
this.txtCustomerPartNo.Size = new System.Drawing.Size(100, 21);
this.txtCustomerPartNo.TabIndex = 23;
this.Controls.Add(this.lblCustomerPartNo);
this.Controls.Add(this.txtCustomerPartNo);

           //#####Cost###Decimal
this.lblCost.AutoSize = true;
this.lblCost.Location = new System.Drawing.Point(100,600);
this.lblCost.Name = "lblCost";
this.lblCost.Size = new System.Drawing.Size(41, 12);
this.lblCost.TabIndex = 24;
this.lblCost.Text = "";
//111======600
this.txtCost.Location = new System.Drawing.Point(173,596);
this.txtCost.Name ="txtCost";
this.txtCost.Size = new System.Drawing.Size(100, 21);
this.txtCost.TabIndex = 24;
this.Controls.Add(this.lblCost);
this.Controls.Add(this.txtCost);

           //#####Price###Decimal
this.lblPrice.AutoSize = true;
this.lblPrice.Location = new System.Drawing.Point(100,625);
this.lblPrice.Name = "lblPrice";
this.lblPrice.Size = new System.Drawing.Size(41, 12);
this.lblPrice.TabIndex = 25;
this.lblPrice.Text = "";
//111======625
this.txtPrice.Location = new System.Drawing.Point(173,621);
this.txtPrice.Name ="txtPrice";
this.txtPrice.Size = new System.Drawing.Size(100, 21);
this.txtPrice.TabIndex = 25;
this.Controls.Add(this.lblPrice);
this.Controls.Add(this.txtPrice);

           //#####SubtotalPrice###Decimal
this.lblSubtotalPrice.AutoSize = true;
this.lblSubtotalPrice.Location = new System.Drawing.Point(100,650);
this.lblSubtotalPrice.Name = "lblSubtotalPrice";
this.lblSubtotalPrice.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalPrice.TabIndex = 26;
this.lblSubtotalPrice.Text = "";
//111======650
this.txtSubtotalPrice.Location = new System.Drawing.Point(173,646);
this.txtSubtotalPrice.Name ="txtSubtotalPrice";
this.txtSubtotalPrice.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalPrice.TabIndex = 26;
this.Controls.Add(this.lblSubtotalPrice);
this.Controls.Add(this.txtSubtotalPrice);

           //#####SubtotalCost###Decimal
this.lblSubtotalCost.AutoSize = true;
this.lblSubtotalCost.Location = new System.Drawing.Point(100,675);
this.lblSubtotalCost.Name = "lblSubtotalCost";
this.lblSubtotalCost.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalCost.TabIndex = 27;
this.lblSubtotalCost.Text = "";
//111======675
this.txtSubtotalCost.Location = new System.Drawing.Point(173,671);
this.txtSubtotalCost.Name ="txtSubtotalCost";
this.txtSubtotalCost.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalCost.TabIndex = 27;
this.Controls.Add(this.lblSubtotalCost);
this.Controls.Add(this.txtSubtotalCost);

           //#####ReturnQty###Int32

           //#####Location_ID###Int64

           //#####80SKU###String
this.lblSKU.AutoSize = true;
this.lblSKU.Location = new System.Drawing.Point(100,750);
this.lblSKU.Name = "lblSKU";
this.lblSKU.Size = new System.Drawing.Size(41, 12);
this.lblSKU.TabIndex = 30;
this.lblSKU.Text = "";
this.txtSKU.Location = new System.Drawing.Point(173,746);
this.txtSKU.Name = "txtSKU";
this.txtSKU.Size = new System.Drawing.Size(100, 21);
this.txtSKU.TabIndex = 30;
this.Controls.Add(this.lblSKU);
this.Controls.Add(this.txtSKU);

           //#####ProdBaseID###Int64

           //#####255CNName###String
this.lblCNName.AutoSize = true;
this.lblCNName.Location = new System.Drawing.Point(100,800);
this.lblCNName.Name = "lblCNName";
this.lblCNName.Size = new System.Drawing.Size(41, 12);
this.lblCNName.TabIndex = 32;
this.lblCNName.Text = "";
this.txtCNName.Location = new System.Drawing.Point(173,796);
this.txtCNName.Name = "txtCNName";
this.txtCNName.Size = new System.Drawing.Size(100, 21);
this.txtCNName.TabIndex = 32;
this.Controls.Add(this.lblCNName);
this.Controls.Add(this.txtCNName);

           //#####1000Specifications###String
this.lblSpecifications.AutoSize = true;
this.lblSpecifications.Location = new System.Drawing.Point(100,825);
this.lblSpecifications.Name = "lblSpecifications";
this.lblSpecifications.Size = new System.Drawing.Size(41, 12);
this.lblSpecifications.TabIndex = 33;
this.lblSpecifications.Text = "";
this.txtSpecifications.Location = new System.Drawing.Point(173,821);
this.txtSpecifications.Name = "txtSpecifications";
this.txtSpecifications.Size = new System.Drawing.Size(100, 21);
this.txtSpecifications.TabIndex = 33;
this.Controls.Add(this.lblSpecifications);
this.Controls.Add(this.txtSpecifications);

           //#####40ProductNo###String
this.lblProductNo.AutoSize = true;
this.lblProductNo.Location = new System.Drawing.Point(100,850);
this.lblProductNo.Name = "lblProductNo";
this.lblProductNo.Size = new System.Drawing.Size(41, 12);
this.lblProductNo.TabIndex = 34;
this.lblProductNo.Text = "";
this.txtProductNo.Location = new System.Drawing.Point(173,846);
this.txtProductNo.Name = "txtProductNo";
this.txtProductNo.Size = new System.Drawing.Size(100, 21);
this.txtProductNo.TabIndex = 34;
this.Controls.Add(this.lblProductNo);
this.Controls.Add(this.txtProductNo);

           //#####Unit_ID###Int64

           //#####50Model###String
this.lblModel.AutoSize = true;
this.lblModel.Location = new System.Drawing.Point(100,900);
this.lblModel.Name = "lblModel";
this.lblModel.Size = new System.Drawing.Size(41, 12);
this.lblModel.TabIndex = 36;
this.lblModel.Text = "";
this.txtModel.Location = new System.Drawing.Point(173,896);
this.txtModel.Name = "txtModel";
this.txtModel.Size = new System.Drawing.Size(100, 21);
this.txtModel.TabIndex = 36;
this.Controls.Add(this.lblModel);
this.Controls.Add(this.txtModel);

           //#####Category_ID###Int64

           //#####Type_ID###Int64

           //#####50ShortCode###String
this.lblShortCode.AutoSize = true;
this.lblShortCode.Location = new System.Drawing.Point(100,975);
this.lblShortCode.Name = "lblShortCode";
this.lblShortCode.Size = new System.Drawing.Size(41, 12);
this.lblShortCode.TabIndex = 39;
this.lblShortCode.Text = "";
this.txtShortCode.Location = new System.Drawing.Point(173,971);
this.txtShortCode.Name = "txtShortCode";
this.txtShortCode.Size = new System.Drawing.Size(100, 21);
this.txtShortCode.TabIndex = 39;
this.Controls.Add(this.lblShortCode);
this.Controls.Add(this.txtShortCode);

           //#####50BarCode###String
this.lblBarCode.AutoSize = true;
this.lblBarCode.Location = new System.Drawing.Point(100,1000);
this.lblBarCode.Name = "lblBarCode";
this.lblBarCode.Size = new System.Drawing.Size(41, 12);
this.lblBarCode.TabIndex = 40;
this.lblBarCode.Text = "";
this.txtBarCode.Location = new System.Drawing.Point(173,996);
this.txtBarCode.Name = "txtBarCode";
this.txtBarCode.Size = new System.Drawing.Size(100, 21);
this.txtBarCode.TabIndex = 40;
this.Controls.Add(this.lblBarCode);
this.Controls.Add(this.txtBarCode);

          
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

                
                
                
                
                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                
                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblApprovalOpinions );
this.Controls.Add(this.txtApprovalOpinions );

                
                this.Controls.Add(this.lblApprover_at );
this.Controls.Add(this.dtpApprover_at );

                
                this.Controls.Add(this.lblApprovalResults );
this.Controls.Add(this.chkApprovalResults );

                
                
                this.Controls.Add(this.lblproperty );
this.Controls.Add(this.txtproperty );

                
                
                
                this.Controls.Add(this.lblSummary );
this.Controls.Add(this.txtSummary );

                this.Controls.Add(this.lblCustomerPartNo );
this.Controls.Add(this.txtCustomerPartNo );

                this.Controls.Add(this.lblCost );
this.Controls.Add(this.txtCost );

                this.Controls.Add(this.lblPrice );
this.Controls.Add(this.txtPrice );

                this.Controls.Add(this.lblSubtotalPrice );
this.Controls.Add(this.txtSubtotalPrice );

                this.Controls.Add(this.lblSubtotalCost );
this.Controls.Add(this.txtSubtotalCost );

                
                
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

                
                
                this.Controls.Add(this.lblShortCode );
this.Controls.Add(this.txtShortCode );

                this.Controls.Add(this.lblBarCode );
this.Controls.Add(this.txtBarCode );

                    
            this.Name = "View_MaterialRequisitionItemsQuery";
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

    
        
              
    
        
              
    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprovalOpinions;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtApprovalOpinions;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprover_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpApprover_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprovalResults;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkApprovalResults;

    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblproperty;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtproperty;

    
        
              
    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSummary;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSummary;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCustomerPartNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCustomerPartNo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCost;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPrice;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPrice;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSubtotalPrice;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSubtotalPrice;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSubtotalCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSubtotalCost;

    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSKU;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSKU;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCNName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCNName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSpecifications;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSpecifications;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProductNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtProductNo;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModel;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtModel;

    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblShortCode;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtShortCode;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblBarCode;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtBarCode;

    
    
   
 





    }
}


