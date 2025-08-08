
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:32
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 借出单统计
    /// </summary>
    partial class View_ProdBorrowingQuery
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
     
     
this.lblBorrowNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtBorrowNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();



this.lblOut_date = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpOut_date = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblNotes = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotes = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

this.lblDueDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpDueDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblReason = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtReason = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtReason.Multiline = true;


this.lblCloseCaseOpinions = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCloseCaseOpinions = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblApprovalOpinions = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtApprovalOpinions = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtApprovalOpinions.Multiline = true;


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




this.lblPrice = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPrice = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSubtotalPirceAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSubtotalPirceAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSubtotalCostAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSubtotalCostAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSummary = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSummary = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtSummary.Multiline = true;

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####BorrowID###Int64

           //#####50BorrowNo###String
this.lblBorrowNo.AutoSize = true;
this.lblBorrowNo.Location = new System.Drawing.Point(100,50);
this.lblBorrowNo.Name = "lblBorrowNo";
this.lblBorrowNo.Size = new System.Drawing.Size(41, 12);
this.lblBorrowNo.TabIndex = 2;
this.lblBorrowNo.Text = "";
this.txtBorrowNo.Location = new System.Drawing.Point(173,46);
this.txtBorrowNo.Name = "txtBorrowNo";
this.txtBorrowNo.Size = new System.Drawing.Size(100, 21);
this.txtBorrowNo.TabIndex = 2;
this.Controls.Add(this.lblBorrowNo);
this.Controls.Add(this.txtBorrowNo);

           //#####CustomerVendor_ID###Int64

           //#####Employee_ID###Int64

           //#####Out_date###DateTime
this.lblOut_date.AutoSize = true;
this.lblOut_date.Location = new System.Drawing.Point(100,125);
this.lblOut_date.Name = "lblOut_date";
this.lblOut_date.Size = new System.Drawing.Size(41, 12);
this.lblOut_date.TabIndex = 5;
this.lblOut_date.Text = "";
//111======125
this.dtpOut_date.Location = new System.Drawing.Point(173,121);
this.dtpOut_date.Name ="dtpOut_date";
this.dtpOut_date.ShowCheckBox =true;
this.dtpOut_date.Size = new System.Drawing.Size(100, 21);
this.dtpOut_date.TabIndex = 5;
this.Controls.Add(this.lblOut_date);
this.Controls.Add(this.dtpOut_date);

           //#####1500Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,150);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 6;
this.lblNotes.Text = "";
this.txtNotes.Location = new System.Drawing.Point(173,146);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 6;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####DueDate###DateTime
this.lblDueDate.AutoSize = true;
this.lblDueDate.Location = new System.Drawing.Point(100,175);
this.lblDueDate.Name = "lblDueDate";
this.lblDueDate.Size = new System.Drawing.Size(41, 12);
this.lblDueDate.TabIndex = 7;
this.lblDueDate.Text = "";
//111======175
this.dtpDueDate.Location = new System.Drawing.Point(173,171);
this.dtpDueDate.Name ="dtpDueDate";
this.dtpDueDate.ShowCheckBox =true;
this.dtpDueDate.Size = new System.Drawing.Size(100, 21);
this.dtpDueDate.TabIndex = 7;
this.Controls.Add(this.lblDueDate);
this.Controls.Add(this.dtpDueDate);

           //#####500Reason###String
this.lblReason.AutoSize = true;
this.lblReason.Location = new System.Drawing.Point(100,200);
this.lblReason.Name = "lblReason";
this.lblReason.Size = new System.Drawing.Size(41, 12);
this.lblReason.TabIndex = 8;
this.lblReason.Text = "";
this.txtReason.Location = new System.Drawing.Point(173,196);
this.txtReason.Name = "txtReason";
this.txtReason.Size = new System.Drawing.Size(100, 21);
this.txtReason.TabIndex = 8;
this.Controls.Add(this.lblReason);
this.Controls.Add(this.txtReason);

           //#####DataStatus###Int32

           //#####200CloseCaseOpinions###String
this.lblCloseCaseOpinions.AutoSize = true;
this.lblCloseCaseOpinions.Location = new System.Drawing.Point(100,250);
this.lblCloseCaseOpinions.Name = "lblCloseCaseOpinions";
this.lblCloseCaseOpinions.Size = new System.Drawing.Size(41, 12);
this.lblCloseCaseOpinions.TabIndex = 10;
this.lblCloseCaseOpinions.Text = "";
this.txtCloseCaseOpinions.Location = new System.Drawing.Point(173,246);
this.txtCloseCaseOpinions.Name = "txtCloseCaseOpinions";
this.txtCloseCaseOpinions.Size = new System.Drawing.Size(100, 21);
this.txtCloseCaseOpinions.TabIndex = 10;
this.Controls.Add(this.lblCloseCaseOpinions);
this.Controls.Add(this.txtCloseCaseOpinions);

           //#####500ApprovalOpinions###String
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

           //#####ProdDetailID###Int64

           //#####80SKU###String
this.lblSKU.AutoSize = true;
this.lblSKU.Location = new System.Drawing.Point(100,325);
this.lblSKU.Name = "lblSKU";
this.lblSKU.Size = new System.Drawing.Size(41, 12);
this.lblSKU.TabIndex = 13;
this.lblSKU.Text = "";
this.txtSKU.Location = new System.Drawing.Point(173,321);
this.txtSKU.Name = "txtSKU";
this.txtSKU.Size = new System.Drawing.Size(100, 21);
this.txtSKU.TabIndex = 13;
this.Controls.Add(this.lblSKU);
this.Controls.Add(this.txtSKU);

           //#####1000Specifications###String
this.lblSpecifications.AutoSize = true;
this.lblSpecifications.Location = new System.Drawing.Point(100,350);
this.lblSpecifications.Name = "lblSpecifications";
this.lblSpecifications.Size = new System.Drawing.Size(41, 12);
this.lblSpecifications.TabIndex = 14;
this.lblSpecifications.Text = "";
this.txtSpecifications.Location = new System.Drawing.Point(173,346);
this.txtSpecifications.Name = "txtSpecifications";
this.txtSpecifications.Size = new System.Drawing.Size(100, 21);
this.txtSpecifications.TabIndex = 14;
this.Controls.Add(this.lblSpecifications);
this.Controls.Add(this.txtSpecifications);

           //#####255CNName###String
this.lblCNName.AutoSize = true;
this.lblCNName.Location = new System.Drawing.Point(100,375);
this.lblCNName.Name = "lblCNName";
this.lblCNName.Size = new System.Drawing.Size(41, 12);
this.lblCNName.TabIndex = 15;
this.lblCNName.Text = "";
this.txtCNName.Location = new System.Drawing.Point(173,371);
this.txtCNName.Name = "txtCNName";
this.txtCNName.Size = new System.Drawing.Size(100, 21);
this.txtCNName.TabIndex = 15;
this.Controls.Add(this.lblCNName);
this.Controls.Add(this.txtCNName);

           //#####50Model###String
this.lblModel.AutoSize = true;
this.lblModel.Location = new System.Drawing.Point(100,400);
this.lblModel.Name = "lblModel";
this.lblModel.Size = new System.Drawing.Size(41, 12);
this.lblModel.TabIndex = 16;
this.lblModel.Text = "";
this.txtModel.Location = new System.Drawing.Point(173,396);
this.txtModel.Name = "txtModel";
this.txtModel.Size = new System.Drawing.Size(100, 21);
this.txtModel.TabIndex = 16;
this.Controls.Add(this.lblModel);
this.Controls.Add(this.txtModel);

           //#####Type_ID###Int64

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

           //#####Location_ID###Int64

           //#####Qty###Int32

           //#####ReQty###Int32

           //#####Price###Decimal
this.lblPrice.AutoSize = true;
this.lblPrice.Location = new System.Drawing.Point(100,550);
this.lblPrice.Name = "lblPrice";
this.lblPrice.Size = new System.Drawing.Size(41, 12);
this.lblPrice.TabIndex = 22;
this.lblPrice.Text = "";
//111======550
this.txtPrice.Location = new System.Drawing.Point(173,546);
this.txtPrice.Name ="txtPrice";
this.txtPrice.Size = new System.Drawing.Size(100, 21);
this.txtPrice.TabIndex = 22;
this.Controls.Add(this.lblPrice);
this.Controls.Add(this.txtPrice);

           //#####SubtotalPirceAmount###Decimal
this.lblSubtotalPirceAmount.AutoSize = true;
this.lblSubtotalPirceAmount.Location = new System.Drawing.Point(100,575);
this.lblSubtotalPirceAmount.Name = "lblSubtotalPirceAmount";
this.lblSubtotalPirceAmount.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalPirceAmount.TabIndex = 23;
this.lblSubtotalPirceAmount.Text = "";
//111======575
this.txtSubtotalPirceAmount.Location = new System.Drawing.Point(173,571);
this.txtSubtotalPirceAmount.Name ="txtSubtotalPirceAmount";
this.txtSubtotalPirceAmount.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalPirceAmount.TabIndex = 23;
this.Controls.Add(this.lblSubtotalPirceAmount);
this.Controls.Add(this.txtSubtotalPirceAmount);

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

           //#####SubtotalCostAmount###Decimal
this.lblSubtotalCostAmount.AutoSize = true;
this.lblSubtotalCostAmount.Location = new System.Drawing.Point(100,625);
this.lblSubtotalCostAmount.Name = "lblSubtotalCostAmount";
this.lblSubtotalCostAmount.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalCostAmount.TabIndex = 25;
this.lblSubtotalCostAmount.Text = "";
//111======625
this.txtSubtotalCostAmount.Location = new System.Drawing.Point(173,621);
this.txtSubtotalCostAmount.Name ="txtSubtotalCostAmount";
this.txtSubtotalCostAmount.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalCostAmount.TabIndex = 25;
this.Controls.Add(this.lblSubtotalCostAmount);
this.Controls.Add(this.txtSubtotalCostAmount);

           //#####500Summary###String
this.lblSummary.AutoSize = true;
this.lblSummary.Location = new System.Drawing.Point(100,650);
this.lblSummary.Name = "lblSummary";
this.lblSummary.Size = new System.Drawing.Size(41, 12);
this.lblSummary.TabIndex = 26;
this.lblSummary.Text = "";
this.txtSummary.Location = new System.Drawing.Point(173,646);
this.txtSummary.Name = "txtSummary";
this.txtSummary.Size = new System.Drawing.Size(100, 21);
this.txtSummary.TabIndex = 26;
this.Controls.Add(this.lblSummary);
this.Controls.Add(this.txtSummary);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                
                this.Controls.Add(this.lblBorrowNo );
this.Controls.Add(this.txtBorrowNo );

                
                
                this.Controls.Add(this.lblOut_date );
this.Controls.Add(this.dtpOut_date );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblDueDate );
this.Controls.Add(this.dtpDueDate );

                this.Controls.Add(this.lblReason );
this.Controls.Add(this.txtReason );

                
                this.Controls.Add(this.lblCloseCaseOpinions );
this.Controls.Add(this.txtCloseCaseOpinions );

                this.Controls.Add(this.lblApprovalOpinions );
this.Controls.Add(this.txtApprovalOpinions );

                
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

                
                
                
                this.Controls.Add(this.lblPrice );
this.Controls.Add(this.txtPrice );

                this.Controls.Add(this.lblSubtotalPirceAmount );
this.Controls.Add(this.txtSubtotalPirceAmount );

                this.Controls.Add(this.lblCost );
this.Controls.Add(this.txtCost );

                this.Controls.Add(this.lblSubtotalCostAmount );
this.Controls.Add(this.txtSubtotalCostAmount );

                this.Controls.Add(this.lblSummary );
this.Controls.Add(this.txtSummary );

                    
            this.Name = "View_ProdBorrowingQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblBorrowNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtBorrowNo;

    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblOut_date;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpOut_date;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDueDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpDueDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblReason;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtReason;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCloseCaseOpinions;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCloseCaseOpinions;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprovalOpinions;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtApprovalOpinions;

    
        
              
    
        
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

    
        
              
    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPrice;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPrice;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSubtotalPirceAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSubtotalPirceAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCost;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSubtotalCostAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSubtotalCostAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSummary;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSummary;

    
    
   
 





    }
}


