
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:26
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 售后申请明细统计
    /// </summary>
    partial class View_AS_AfterSaleApplyItemsQuery
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
     
     this.lblASApplyNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtASApplyNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();




this.lblApplyDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpApplyDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblCustomerSourceNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCustomerSourceNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();




this.lblRepairEvaluationOpinion = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtRepairEvaluationOpinion = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtRepairEvaluationOpinion.Multiline = true;




this.lblFaultDescription = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtFaultDescription = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtFaultDescription.Multiline = true;



this.lblCustomerPartNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCustomerPartNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();




this.lblSummary = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSummary = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtSummary.Multiline = true;

this.lblproperty = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtproperty = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtproperty.Multiline = true;


this.lblSKU = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSKU = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCNName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCNName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtCNName.Multiline = true;


this.lblSpecifications = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSpecifications = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtSpecifications.Multiline = true;


this.lblprop = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtprop = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblProductNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtProductNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblModel = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtModel = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();





this.lblApprovalResults = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkApprovalResults = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkApprovalResults.Values.Text ="";

this.lblApprovalOpinions = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtApprovalOpinions = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtApprovalOpinions.Multiline = true;

this.lblNotes = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotes = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####50ASApplyNo###String
this.lblASApplyNo.AutoSize = true;
this.lblASApplyNo.Location = new System.Drawing.Point(100,25);
this.lblASApplyNo.Name = "lblASApplyNo";
this.lblASApplyNo.Size = new System.Drawing.Size(41, 12);
this.lblASApplyNo.TabIndex = 1;
this.lblASApplyNo.Text = "";
this.txtASApplyNo.Location = new System.Drawing.Point(173,21);
this.txtASApplyNo.Name = "txtASApplyNo";
this.txtASApplyNo.Size = new System.Drawing.Size(100, 21);
this.txtASApplyNo.TabIndex = 1;
this.Controls.Add(this.lblASApplyNo);
this.Controls.Add(this.txtASApplyNo);

           //#####Employee_ID###Int64

           //#####ProjectGroup_ID###Int64

           //#####CustomerVendor_ID###Int64

           //#####ApplyDate###DateTime
this.lblApplyDate.AutoSize = true;
this.lblApplyDate.Location = new System.Drawing.Point(100,125);
this.lblApplyDate.Name = "lblApplyDate";
this.lblApplyDate.Size = new System.Drawing.Size(41, 12);
this.lblApplyDate.TabIndex = 5;
this.lblApplyDate.Text = "";
//111======125
this.dtpApplyDate.Location = new System.Drawing.Point(173,121);
this.dtpApplyDate.Name ="dtpApplyDate";
this.dtpApplyDate.ShowCheckBox =true;
this.dtpApplyDate.Size = new System.Drawing.Size(100, 21);
this.dtpApplyDate.TabIndex = 5;
this.Controls.Add(this.lblApplyDate);
this.Controls.Add(this.dtpApplyDate);

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,150);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 6;
this.lblCreated_at.Text = "";
//111======150
this.dtpCreated_at.Location = new System.Drawing.Point(173,146);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 6;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64

           //#####50CustomerSourceNo###String
this.lblCustomerSourceNo.AutoSize = true;
this.lblCustomerSourceNo.Location = new System.Drawing.Point(100,200);
this.lblCustomerSourceNo.Name = "lblCustomerSourceNo";
this.lblCustomerSourceNo.Size = new System.Drawing.Size(41, 12);
this.lblCustomerSourceNo.TabIndex = 8;
this.lblCustomerSourceNo.Text = "";
this.txtCustomerSourceNo.Location = new System.Drawing.Point(173,196);
this.txtCustomerSourceNo.Name = "txtCustomerSourceNo";
this.txtCustomerSourceNo.Size = new System.Drawing.Size(100, 21);
this.txtCustomerSourceNo.TabIndex = 8;
this.Controls.Add(this.lblCustomerSourceNo);
this.Controls.Add(this.txtCustomerSourceNo);

           //#####Priority###Int32

           //#####ASProcessStatus###Int32

           //#####TotalConfirmedQuantity###Int32

           //#####500RepairEvaluationOpinion###String
this.lblRepairEvaluationOpinion.AutoSize = true;
this.lblRepairEvaluationOpinion.Location = new System.Drawing.Point(100,300);
this.lblRepairEvaluationOpinion.Name = "lblRepairEvaluationOpinion";
this.lblRepairEvaluationOpinion.Size = new System.Drawing.Size(41, 12);
this.lblRepairEvaluationOpinion.TabIndex = 12;
this.lblRepairEvaluationOpinion.Text = "";
this.txtRepairEvaluationOpinion.Location = new System.Drawing.Point(173,296);
this.txtRepairEvaluationOpinion.Name = "txtRepairEvaluationOpinion";
this.txtRepairEvaluationOpinion.Size = new System.Drawing.Size(100, 21);
this.txtRepairEvaluationOpinion.TabIndex = 12;
this.Controls.Add(this.lblRepairEvaluationOpinion);
this.Controls.Add(this.txtRepairEvaluationOpinion);

           //#####ExpenseAllocationMode###Int32

           //#####ExpenseBearerType###Int32

           //#####TotalDeliveredQty###Int32

           //#####500FaultDescription###String
this.lblFaultDescription.AutoSize = true;
this.lblFaultDescription.Location = new System.Drawing.Point(100,400);
this.lblFaultDescription.Name = "lblFaultDescription";
this.lblFaultDescription.Size = new System.Drawing.Size(41, 12);
this.lblFaultDescription.TabIndex = 16;
this.lblFaultDescription.Text = "";
this.txtFaultDescription.Location = new System.Drawing.Point(173,396);
this.txtFaultDescription.Name = "txtFaultDescription";
this.txtFaultDescription.Size = new System.Drawing.Size(100, 21);
this.txtFaultDescription.TabIndex = 16;
this.Controls.Add(this.lblFaultDescription);
this.Controls.Add(this.txtFaultDescription);

           //#####ProdDetailID###Int64

           //#####Location_ID###Int64

           //#####100CustomerPartNo###String
this.lblCustomerPartNo.AutoSize = true;
this.lblCustomerPartNo.Location = new System.Drawing.Point(100,475);
this.lblCustomerPartNo.Name = "lblCustomerPartNo";
this.lblCustomerPartNo.Size = new System.Drawing.Size(41, 12);
this.lblCustomerPartNo.TabIndex = 19;
this.lblCustomerPartNo.Text = "";
this.txtCustomerPartNo.Location = new System.Drawing.Point(173,471);
this.txtCustomerPartNo.Name = "txtCustomerPartNo";
this.txtCustomerPartNo.Size = new System.Drawing.Size(100, 21);
this.txtCustomerPartNo.TabIndex = 19;
this.Controls.Add(this.lblCustomerPartNo);
this.Controls.Add(this.txtCustomerPartNo);

           //#####ConfirmedQuantity###Int32

           //#####DeliveredQty###Int32

           //#####InitialQuantity###Int32

           //#####1000Summary###String
this.lblSummary.AutoSize = true;
this.lblSummary.Location = new System.Drawing.Point(100,575);
this.lblSummary.Name = "lblSummary";
this.lblSummary.Size = new System.Drawing.Size(41, 12);
this.lblSummary.TabIndex = 23;
this.lblSummary.Text = "";
this.txtSummary.Location = new System.Drawing.Point(173,571);
this.txtSummary.Name = "txtSummary";
this.txtSummary.Size = new System.Drawing.Size(100, 21);
this.txtSummary.TabIndex = 23;
this.Controls.Add(this.lblSummary);
this.Controls.Add(this.txtSummary);

           //#####255property###String
this.lblproperty.AutoSize = true;
this.lblproperty.Location = new System.Drawing.Point(100,600);
this.lblproperty.Name = "lblproperty";
this.lblproperty.Size = new System.Drawing.Size(41, 12);
this.lblproperty.TabIndex = 24;
this.lblproperty.Text = "";
this.txtproperty.Location = new System.Drawing.Point(173,596);
this.txtproperty.Name = "txtproperty";
this.txtproperty.Size = new System.Drawing.Size(100, 21);
this.txtproperty.TabIndex = 24;
this.Controls.Add(this.lblproperty);
this.Controls.Add(this.txtproperty);

           //#####ProdBaseID###Int64

           //#####80SKU###String
this.lblSKU.AutoSize = true;
this.lblSKU.Location = new System.Drawing.Point(100,650);
this.lblSKU.Name = "lblSKU";
this.lblSKU.Size = new System.Drawing.Size(41, 12);
this.lblSKU.TabIndex = 26;
this.lblSKU.Text = "";
this.txtSKU.Location = new System.Drawing.Point(173,646);
this.txtSKU.Name = "txtSKU";
this.txtSKU.Size = new System.Drawing.Size(100, 21);
this.txtSKU.TabIndex = 26;
this.Controls.Add(this.lblSKU);
this.Controls.Add(this.txtSKU);

           //#####255CNName###String
this.lblCNName.AutoSize = true;
this.lblCNName.Location = new System.Drawing.Point(100,675);
this.lblCNName.Name = "lblCNName";
this.lblCNName.Size = new System.Drawing.Size(41, 12);
this.lblCNName.TabIndex = 27;
this.lblCNName.Text = "";
this.txtCNName.Location = new System.Drawing.Point(173,671);
this.txtCNName.Name = "txtCNName";
this.txtCNName.Size = new System.Drawing.Size(100, 21);
this.txtCNName.TabIndex = 27;
this.Controls.Add(this.lblCNName);
this.Controls.Add(this.txtCNName);

           //#####Unit_ID###Int64

           //#####1000Specifications###String
this.lblSpecifications.AutoSize = true;
this.lblSpecifications.Location = new System.Drawing.Point(100,725);
this.lblSpecifications.Name = "lblSpecifications";
this.lblSpecifications.Size = new System.Drawing.Size(41, 12);
this.lblSpecifications.TabIndex = 29;
this.lblSpecifications.Text = "";
this.txtSpecifications.Location = new System.Drawing.Point(173,721);
this.txtSpecifications.Name = "txtSpecifications";
this.txtSpecifications.Size = new System.Drawing.Size(100, 21);
this.txtSpecifications.TabIndex = 29;
this.Controls.Add(this.lblSpecifications);
this.Controls.Add(this.txtSpecifications);

           //#####Quantity###Int32

           //#####-1prop###String
this.lblprop.AutoSize = true;
this.lblprop.Location = new System.Drawing.Point(100,775);
this.lblprop.Name = "lblprop";
this.lblprop.Size = new System.Drawing.Size(41, 12);
this.lblprop.TabIndex = 31;
this.lblprop.Text = "";
this.txtprop.Location = new System.Drawing.Point(173,771);
this.txtprop.Name = "txtprop";
this.txtprop.Size = new System.Drawing.Size(100, 21);
this.txtprop.TabIndex = 31;
this.Controls.Add(this.lblprop);
this.Controls.Add(this.txtprop);

           //#####40ProductNo###String
this.lblProductNo.AutoSize = true;
this.lblProductNo.Location = new System.Drawing.Point(100,800);
this.lblProductNo.Name = "lblProductNo";
this.lblProductNo.Size = new System.Drawing.Size(41, 12);
this.lblProductNo.TabIndex = 32;
this.lblProductNo.Text = "";
this.txtProductNo.Location = new System.Drawing.Point(173,796);
this.txtProductNo.Name = "txtProductNo";
this.txtProductNo.Size = new System.Drawing.Size(100, 21);
this.txtProductNo.TabIndex = 32;
this.Controls.Add(this.lblProductNo);
this.Controls.Add(this.txtProductNo);

           //#####50Model###String
this.lblModel.AutoSize = true;
this.lblModel.Location = new System.Drawing.Point(100,825);
this.lblModel.Name = "lblModel";
this.lblModel.Size = new System.Drawing.Size(41, 12);
this.lblModel.TabIndex = 33;
this.lblModel.Text = "";
this.txtModel.Location = new System.Drawing.Point(173,821);
this.txtModel.Name = "txtModel";
this.txtModel.Size = new System.Drawing.Size(100, 21);
this.txtModel.TabIndex = 33;
this.Controls.Add(this.lblModel);
this.Controls.Add(this.txtModel);

           //#####Category_ID###Int64

           //#####Type_ID###Int64

           //#####DataStatus###Int32

           //#####ApprovalStatus###SByte

           //#####ApprovalResults###Boolean
this.lblApprovalResults.AutoSize = true;
this.lblApprovalResults.Location = new System.Drawing.Point(100,950);
this.lblApprovalResults.Name = "lblApprovalResults";
this.lblApprovalResults.Size = new System.Drawing.Size(41, 12);
this.lblApprovalResults.TabIndex = 38;
this.lblApprovalResults.Text = "";
this.chkApprovalResults.Location = new System.Drawing.Point(173,946);
this.chkApprovalResults.Name = "chkApprovalResults";
this.chkApprovalResults.Size = new System.Drawing.Size(100, 21);
this.chkApprovalResults.TabIndex = 38;
this.Controls.Add(this.lblApprovalResults);
this.Controls.Add(this.chkApprovalResults);

           //#####255ApprovalOpinions###String
this.lblApprovalOpinions.AutoSize = true;
this.lblApprovalOpinions.Location = new System.Drawing.Point(100,975);
this.lblApprovalOpinions.Name = "lblApprovalOpinions";
this.lblApprovalOpinions.Size = new System.Drawing.Size(41, 12);
this.lblApprovalOpinions.TabIndex = 39;
this.lblApprovalOpinions.Text = "";
this.txtApprovalOpinions.Location = new System.Drawing.Point(173,971);
this.txtApprovalOpinions.Name = "txtApprovalOpinions";
this.txtApprovalOpinions.Size = new System.Drawing.Size(100, 21);
this.txtApprovalOpinions.TabIndex = 39;
this.Controls.Add(this.lblApprovalOpinions);
this.Controls.Add(this.txtApprovalOpinions);

           //#####1500Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,1000);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 40;
this.lblNotes.Text = "";
this.txtNotes.Location = new System.Drawing.Point(173,996);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 40;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblASApplyNo );
this.Controls.Add(this.txtASApplyNo );

                
                
                
                this.Controls.Add(this.lblApplyDate );
this.Controls.Add(this.dtpApplyDate );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                
                this.Controls.Add(this.lblCustomerSourceNo );
this.Controls.Add(this.txtCustomerSourceNo );

                
                
                
                this.Controls.Add(this.lblRepairEvaluationOpinion );
this.Controls.Add(this.txtRepairEvaluationOpinion );

                
                
                
                this.Controls.Add(this.lblFaultDescription );
this.Controls.Add(this.txtFaultDescription );

                
                
                this.Controls.Add(this.lblCustomerPartNo );
this.Controls.Add(this.txtCustomerPartNo );

                
                
                
                this.Controls.Add(this.lblSummary );
this.Controls.Add(this.txtSummary );

                this.Controls.Add(this.lblproperty );
this.Controls.Add(this.txtproperty );

                
                this.Controls.Add(this.lblSKU );
this.Controls.Add(this.txtSKU );

                this.Controls.Add(this.lblCNName );
this.Controls.Add(this.txtCNName );

                
                this.Controls.Add(this.lblSpecifications );
this.Controls.Add(this.txtSpecifications );

                
                this.Controls.Add(this.lblprop );
this.Controls.Add(this.txtprop );

                this.Controls.Add(this.lblProductNo );
this.Controls.Add(this.txtProductNo );

                this.Controls.Add(this.lblModel );
this.Controls.Add(this.txtModel );

                
                
                
                
                this.Controls.Add(this.lblApprovalResults );
this.Controls.Add(this.chkApprovalResults );

                this.Controls.Add(this.lblApprovalOpinions );
this.Controls.Add(this.txtApprovalOpinions );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                    
            this.Name = "View_AS_AfterSaleApplyItemsQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblASApplyNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtASApplyNo;

    
        
              
    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApplyDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpApplyDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCustomerSourceNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCustomerSourceNo;

    
        
              
    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRepairEvaluationOpinion;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtRepairEvaluationOpinion;

    
        
              
    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblFaultDescription;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtFaultDescription;

    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCustomerPartNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCustomerPartNo;

    
        
              
    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSummary;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSummary;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblproperty;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtproperty;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSKU;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSKU;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCNName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCNName;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSpecifications;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSpecifications;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblprop;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtprop;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProductNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtProductNo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModel;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtModel;

    
        
              
    
        
              
    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprovalResults;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkApprovalResults;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprovalOpinions;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtApprovalOpinions;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
    
   
 





    }
}


