
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
    partial class View_StocktakeItemsQuery
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
     
     this.lblCheckNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCheckNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();






this.lblCheck_date = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCheck_date = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblCarryingDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCarryingDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblNotes = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotes = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;


this.lblApprovalOpinions = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtApprovalOpinions = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtApprovalOpinions.Multiline = true;

this.lblApprovalResults = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkApprovalResults = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkApprovalResults.Values.Text ="";


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






this.lblCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblCarryingSubtotalAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCarryingSubtotalAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblDiffSubtotalAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtDiffSubtotalAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblCheckSubtotalAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCheckSubtotalAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblproperty = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtproperty = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtproperty.Multiline = true;

    //for end
    this.SuspendLayout();
    
         //for start
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

           //#####Employee_ID###Int64

           //#####CheckMode###Int32

           //#####Adjust_Type###Int32

           //#####CheckResult###Int32

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

           //#####Type_ID###Int64

           //#####Unit_ID###Int64

           //#####ProdDetailID###Int64

           //#####Rack_ID###Int64

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

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblCheckNo );
this.Controls.Add(this.txtCheckNo );

                
                
                
                
                
                this.Controls.Add(this.lblCheck_date );
this.Controls.Add(this.dtpCheck_date );

                this.Controls.Add(this.lblCarryingDate );
this.Controls.Add(this.dtpCarryingDate );

                
                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                
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

                
                
                
                
                
                this.Controls.Add(this.lblCost );
this.Controls.Add(this.txtCost );

                
                this.Controls.Add(this.lblCarryingSubtotalAmount );
this.Controls.Add(this.txtCarryingSubtotalAmount );

                
                this.Controls.Add(this.lblDiffSubtotalAmount );
this.Controls.Add(this.txtDiffSubtotalAmount );

                
                this.Controls.Add(this.lblCheckSubtotalAmount );
this.Controls.Add(this.txtCheckSubtotalAmount );

                this.Controls.Add(this.lblproperty );
this.Controls.Add(this.txtproperty );

                    
            this.Name = "View_StocktakeItemsQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCheckNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCheckNo;

    
        
              
    
        
              
    
        
              
    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCheck_date;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCheck_date;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCarryingDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCarryingDate;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprovalOpinions;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtApprovalOpinions;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprovalResults;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkApprovalResults;

    
        
              
    
        
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

    
        
              
    
        
              
    
        
              
    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCost;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCarryingSubtotalAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCarryingSubtotalAmount;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDiffSubtotalAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtDiffSubtotalAmount;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCheckSubtotalAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCheckSubtotalAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblproperty;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtproperty;

    
    
   
 





    }
}


