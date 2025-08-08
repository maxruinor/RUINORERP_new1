
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
    /// 转换明细统计
    /// </summary>
    partial class View_ProdConversionItemsQuery
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
     
     

this.lblConversionNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtConversionNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblConversionDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpConversionDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblReason = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtReason = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtReason.Multiline = true;

this.lblNotes = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtNotes = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

this.lblCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();



this.lblApprovalOpinions = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtApprovalOpinions = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtApprovalOpinions.Multiline = true;


this.lblApprovalResults = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkApprovalResults = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkApprovalResults.Values.Text ="";


this.lblBarCode_from = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtBarCode_from = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtBarCode_from.Multiline = true;

this.lblSKU_from = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSKU_from = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtSKU_from.Multiline = true;


this.lblCNName_from = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCNName_from = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtCNName_from.Multiline = true;

this.lblModel_from = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtModel_from = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSpecifications_from = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSpecifications_from = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtSpecifications_from.Multiline = true;

this.lblproperty_from = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtproperty_from = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtproperty_from.Multiline = true;



this.lblBarCode_to = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtBarCode_to = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtBarCode_to.Multiline = true;

this.lblSKU_to = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSKU_to = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtSKU_to.Multiline = true;


this.lblCNName_to = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCNName_to = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtCNName_to.Multiline = true;

this.lblModel_to = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtModel_to = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSpecifications_to = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSpecifications_to = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtSpecifications_to.Multiline = true;

this.lblproperty_to = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtproperty_to = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtproperty_to.Multiline = true;

this.lblSummary = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSummary = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtSummary.Multiline = true;

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####Employee_ID###Int64

           //#####Location_ID###Int64

           //#####50ConversionNo###String
this.lblConversionNo.AutoSize = true;
this.lblConversionNo.Location = new System.Drawing.Point(100,75);
this.lblConversionNo.Name = "lblConversionNo";
this.lblConversionNo.Size = new System.Drawing.Size(41, 12);
this.lblConversionNo.TabIndex = 3;
this.lblConversionNo.Text = "";
this.txtConversionNo.Location = new System.Drawing.Point(173,71);
this.txtConversionNo.Name = "txtConversionNo";
this.txtConversionNo.Size = new System.Drawing.Size(100, 21);
this.txtConversionNo.TabIndex = 3;
this.Controls.Add(this.lblConversionNo);
this.Controls.Add(this.txtConversionNo);

           //#####ConversionDate###DateTime
this.lblConversionDate.AutoSize = true;
this.lblConversionDate.Location = new System.Drawing.Point(100,100);
this.lblConversionDate.Name = "lblConversionDate";
this.lblConversionDate.Size = new System.Drawing.Size(41, 12);
this.lblConversionDate.TabIndex = 4;
this.lblConversionDate.Text = "";
//111======100
this.dtpConversionDate.Location = new System.Drawing.Point(173,96);
this.dtpConversionDate.Name ="dtpConversionDate";
this.dtpConversionDate.ShowCheckBox =true;
this.dtpConversionDate.Size = new System.Drawing.Size(100, 21);
this.dtpConversionDate.TabIndex = 4;
this.Controls.Add(this.lblConversionDate);
this.Controls.Add(this.dtpConversionDate);

           //#####300Reason###String
this.lblReason.AutoSize = true;
this.lblReason.Location = new System.Drawing.Point(100,125);
this.lblReason.Name = "lblReason";
this.lblReason.Size = new System.Drawing.Size(41, 12);
this.lblReason.TabIndex = 5;
this.lblReason.Text = "";
this.txtReason.Location = new System.Drawing.Point(173,121);
this.txtReason.Name = "txtReason";
this.txtReason.Size = new System.Drawing.Size(100, 21);
this.txtReason.TabIndex = 5;
this.Controls.Add(this.lblReason);
this.Controls.Add(this.txtReason);

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

           //#####Created_at###DateTime
this.lblCreated_at.AutoSize = true;
this.lblCreated_at.Location = new System.Drawing.Point(100,175);
this.lblCreated_at.Name = "lblCreated_at";
this.lblCreated_at.Size = new System.Drawing.Size(41, 12);
this.lblCreated_at.TabIndex = 7;
this.lblCreated_at.Text = "";
//111======175
this.dtpCreated_at.Location = new System.Drawing.Point(173,171);
this.dtpCreated_at.Name ="dtpCreated_at";
this.dtpCreated_at.ShowCheckBox =true;
this.dtpCreated_at.Size = new System.Drawing.Size(100, 21);
this.dtpCreated_at.TabIndex = 7;
this.Controls.Add(this.lblCreated_at);
this.Controls.Add(this.dtpCreated_at);

           //#####Created_by###Int64

           //#####DataStatus###Int32

           //#####500ApprovalOpinions###String
this.lblApprovalOpinions.AutoSize = true;
this.lblApprovalOpinions.Location = new System.Drawing.Point(100,250);
this.lblApprovalOpinions.Name = "lblApprovalOpinions";
this.lblApprovalOpinions.Size = new System.Drawing.Size(41, 12);
this.lblApprovalOpinions.TabIndex = 10;
this.lblApprovalOpinions.Text = "";
this.txtApprovalOpinions.Location = new System.Drawing.Point(173,246);
this.txtApprovalOpinions.Name = "txtApprovalOpinions";
this.txtApprovalOpinions.Size = new System.Drawing.Size(100, 21);
this.txtApprovalOpinions.TabIndex = 10;
this.Controls.Add(this.lblApprovalOpinions);
this.Controls.Add(this.txtApprovalOpinions);

           //#####ApprovalStatus###SByte

           //#####ApprovalResults###Boolean
this.lblApprovalResults.AutoSize = true;
this.lblApprovalResults.Location = new System.Drawing.Point(100,300);
this.lblApprovalResults.Name = "lblApprovalResults";
this.lblApprovalResults.Size = new System.Drawing.Size(41, 12);
this.lblApprovalResults.TabIndex = 12;
this.lblApprovalResults.Text = "";
this.chkApprovalResults.Location = new System.Drawing.Point(173,296);
this.chkApprovalResults.Name = "chkApprovalResults";
this.chkApprovalResults.Size = new System.Drawing.Size(100, 21);
this.chkApprovalResults.TabIndex = 12;
this.Controls.Add(this.lblApprovalResults);
this.Controls.Add(this.chkApprovalResults);

           //#####ProdDetailID_from###Int64

           //#####255BarCode_from###String
this.lblBarCode_from.AutoSize = true;
this.lblBarCode_from.Location = new System.Drawing.Point(100,350);
this.lblBarCode_from.Name = "lblBarCode_from";
this.lblBarCode_from.Size = new System.Drawing.Size(41, 12);
this.lblBarCode_from.TabIndex = 14;
this.lblBarCode_from.Text = "";
this.txtBarCode_from.Location = new System.Drawing.Point(173,346);
this.txtBarCode_from.Name = "txtBarCode_from";
this.txtBarCode_from.Size = new System.Drawing.Size(100, 21);
this.txtBarCode_from.TabIndex = 14;
this.Controls.Add(this.lblBarCode_from);
this.Controls.Add(this.txtBarCode_from);

           //#####255SKU_from###String
this.lblSKU_from.AutoSize = true;
this.lblSKU_from.Location = new System.Drawing.Point(100,375);
this.lblSKU_from.Name = "lblSKU_from";
this.lblSKU_from.Size = new System.Drawing.Size(41, 12);
this.lblSKU_from.TabIndex = 15;
this.lblSKU_from.Text = "";
this.txtSKU_from.Location = new System.Drawing.Point(173,371);
this.txtSKU_from.Name = "txtSKU_from";
this.txtSKU_from.Size = new System.Drawing.Size(100, 21);
this.txtSKU_from.TabIndex = 15;
this.Controls.Add(this.lblSKU_from);
this.Controls.Add(this.txtSKU_from);

           //#####Type_ID_from###Int64

           //#####255CNName_from###String
this.lblCNName_from.AutoSize = true;
this.lblCNName_from.Location = new System.Drawing.Point(100,425);
this.lblCNName_from.Name = "lblCNName_from";
this.lblCNName_from.Size = new System.Drawing.Size(41, 12);
this.lblCNName_from.TabIndex = 17;
this.lblCNName_from.Text = "";
this.txtCNName_from.Location = new System.Drawing.Point(173,421);
this.txtCNName_from.Name = "txtCNName_from";
this.txtCNName_from.Size = new System.Drawing.Size(100, 21);
this.txtCNName_from.TabIndex = 17;
this.Controls.Add(this.lblCNName_from);
this.Controls.Add(this.txtCNName_from);

           //#####50Model_from###String
this.lblModel_from.AutoSize = true;
this.lblModel_from.Location = new System.Drawing.Point(100,450);
this.lblModel_from.Name = "lblModel_from";
this.lblModel_from.Size = new System.Drawing.Size(41, 12);
this.lblModel_from.TabIndex = 18;
this.lblModel_from.Text = "";
this.txtModel_from.Location = new System.Drawing.Point(173,446);
this.txtModel_from.Name = "txtModel_from";
this.txtModel_from.Size = new System.Drawing.Size(100, 21);
this.txtModel_from.TabIndex = 18;
this.Controls.Add(this.lblModel_from);
this.Controls.Add(this.txtModel_from);

           //#####1000Specifications_from###String
this.lblSpecifications_from.AutoSize = true;
this.lblSpecifications_from.Location = new System.Drawing.Point(100,475);
this.lblSpecifications_from.Name = "lblSpecifications_from";
this.lblSpecifications_from.Size = new System.Drawing.Size(41, 12);
this.lblSpecifications_from.TabIndex = 19;
this.lblSpecifications_from.Text = "";
this.txtSpecifications_from.Location = new System.Drawing.Point(173,471);
this.txtSpecifications_from.Name = "txtSpecifications_from";
this.txtSpecifications_from.Size = new System.Drawing.Size(100, 21);
this.txtSpecifications_from.TabIndex = 19;
this.Controls.Add(this.lblSpecifications_from);
this.Controls.Add(this.txtSpecifications_from);

           //#####255property_from###String
this.lblproperty_from.AutoSize = true;
this.lblproperty_from.Location = new System.Drawing.Point(100,500);
this.lblproperty_from.Name = "lblproperty_from";
this.lblproperty_from.Size = new System.Drawing.Size(41, 12);
this.lblproperty_from.TabIndex = 20;
this.lblproperty_from.Text = "";
this.txtproperty_from.Location = new System.Drawing.Point(173,496);
this.txtproperty_from.Name = "txtproperty_from";
this.txtproperty_from.Size = new System.Drawing.Size(100, 21);
this.txtproperty_from.TabIndex = 20;
this.Controls.Add(this.lblproperty_from);
this.Controls.Add(this.txtproperty_from);

           //#####ConversionQty###Int32

           //#####ProdDetailID_to###Int64

           //#####255BarCode_to###String
this.lblBarCode_to.AutoSize = true;
this.lblBarCode_to.Location = new System.Drawing.Point(100,575);
this.lblBarCode_to.Name = "lblBarCode_to";
this.lblBarCode_to.Size = new System.Drawing.Size(41, 12);
this.lblBarCode_to.TabIndex = 23;
this.lblBarCode_to.Text = "";
this.txtBarCode_to.Location = new System.Drawing.Point(173,571);
this.txtBarCode_to.Name = "txtBarCode_to";
this.txtBarCode_to.Size = new System.Drawing.Size(100, 21);
this.txtBarCode_to.TabIndex = 23;
this.Controls.Add(this.lblBarCode_to);
this.Controls.Add(this.txtBarCode_to);

           //#####255SKU_to###String
this.lblSKU_to.AutoSize = true;
this.lblSKU_to.Location = new System.Drawing.Point(100,600);
this.lblSKU_to.Name = "lblSKU_to";
this.lblSKU_to.Size = new System.Drawing.Size(41, 12);
this.lblSKU_to.TabIndex = 24;
this.lblSKU_to.Text = "";
this.txtSKU_to.Location = new System.Drawing.Point(173,596);
this.txtSKU_to.Name = "txtSKU_to";
this.txtSKU_to.Size = new System.Drawing.Size(100, 21);
this.txtSKU_to.TabIndex = 24;
this.Controls.Add(this.lblSKU_to);
this.Controls.Add(this.txtSKU_to);

           //#####Type_ID_to###Int64

           //#####255CNName_to###String
this.lblCNName_to.AutoSize = true;
this.lblCNName_to.Location = new System.Drawing.Point(100,650);
this.lblCNName_to.Name = "lblCNName_to";
this.lblCNName_to.Size = new System.Drawing.Size(41, 12);
this.lblCNName_to.TabIndex = 26;
this.lblCNName_to.Text = "";
this.txtCNName_to.Location = new System.Drawing.Point(173,646);
this.txtCNName_to.Name = "txtCNName_to";
this.txtCNName_to.Size = new System.Drawing.Size(100, 21);
this.txtCNName_to.TabIndex = 26;
this.Controls.Add(this.lblCNName_to);
this.Controls.Add(this.txtCNName_to);

           //#####50Model_to###String
this.lblModel_to.AutoSize = true;
this.lblModel_to.Location = new System.Drawing.Point(100,675);
this.lblModel_to.Name = "lblModel_to";
this.lblModel_to.Size = new System.Drawing.Size(41, 12);
this.lblModel_to.TabIndex = 27;
this.lblModel_to.Text = "";
this.txtModel_to.Location = new System.Drawing.Point(173,671);
this.txtModel_to.Name = "txtModel_to";
this.txtModel_to.Size = new System.Drawing.Size(100, 21);
this.txtModel_to.TabIndex = 27;
this.Controls.Add(this.lblModel_to);
this.Controls.Add(this.txtModel_to);

           //#####1000Specifications_to###String
this.lblSpecifications_to.AutoSize = true;
this.lblSpecifications_to.Location = new System.Drawing.Point(100,700);
this.lblSpecifications_to.Name = "lblSpecifications_to";
this.lblSpecifications_to.Size = new System.Drawing.Size(41, 12);
this.lblSpecifications_to.TabIndex = 28;
this.lblSpecifications_to.Text = "";
this.txtSpecifications_to.Location = new System.Drawing.Point(173,696);
this.txtSpecifications_to.Name = "txtSpecifications_to";
this.txtSpecifications_to.Size = new System.Drawing.Size(100, 21);
this.txtSpecifications_to.TabIndex = 28;
this.Controls.Add(this.lblSpecifications_to);
this.Controls.Add(this.txtSpecifications_to);

           //#####255property_to###String
this.lblproperty_to.AutoSize = true;
this.lblproperty_to.Location = new System.Drawing.Point(100,725);
this.lblproperty_to.Name = "lblproperty_to";
this.lblproperty_to.Size = new System.Drawing.Size(41, 12);
this.lblproperty_to.TabIndex = 29;
this.lblproperty_to.Text = "";
this.txtproperty_to.Location = new System.Drawing.Point(173,721);
this.txtproperty_to.Name = "txtproperty_to";
this.txtproperty_to.Size = new System.Drawing.Size(100, 21);
this.txtproperty_to.TabIndex = 29;
this.Controls.Add(this.lblproperty_to);
this.Controls.Add(this.txtproperty_to);

           //#####1000Summary###String
this.lblSummary.AutoSize = true;
this.lblSummary.Location = new System.Drawing.Point(100,750);
this.lblSummary.Name = "lblSummary";
this.lblSummary.Size = new System.Drawing.Size(41, 12);
this.lblSummary.TabIndex = 30;
this.lblSummary.Text = "";
this.txtSummary.Location = new System.Drawing.Point(173,746);
this.txtSummary.Name = "txtSummary";
this.txtSummary.Size = new System.Drawing.Size(100, 21);
this.txtSummary.TabIndex = 30;
this.Controls.Add(this.lblSummary);
this.Controls.Add(this.txtSummary);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                
                
                this.Controls.Add(this.lblConversionNo );
this.Controls.Add(this.txtConversionNo );

                this.Controls.Add(this.lblConversionDate );
this.Controls.Add(this.dtpConversionDate );

                this.Controls.Add(this.lblReason );
this.Controls.Add(this.txtReason );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                
                
                this.Controls.Add(this.lblApprovalOpinions );
this.Controls.Add(this.txtApprovalOpinions );

                
                this.Controls.Add(this.lblApprovalResults );
this.Controls.Add(this.chkApprovalResults );

                
                this.Controls.Add(this.lblBarCode_from );
this.Controls.Add(this.txtBarCode_from );

                this.Controls.Add(this.lblSKU_from );
this.Controls.Add(this.txtSKU_from );

                
                this.Controls.Add(this.lblCNName_from );
this.Controls.Add(this.txtCNName_from );

                this.Controls.Add(this.lblModel_from );
this.Controls.Add(this.txtModel_from );

                this.Controls.Add(this.lblSpecifications_from );
this.Controls.Add(this.txtSpecifications_from );

                this.Controls.Add(this.lblproperty_from );
this.Controls.Add(this.txtproperty_from );

                
                
                this.Controls.Add(this.lblBarCode_to );
this.Controls.Add(this.txtBarCode_to );

                this.Controls.Add(this.lblSKU_to );
this.Controls.Add(this.txtSKU_to );

                
                this.Controls.Add(this.lblCNName_to );
this.Controls.Add(this.txtCNName_to );

                this.Controls.Add(this.lblModel_to );
this.Controls.Add(this.txtModel_to );

                this.Controls.Add(this.lblSpecifications_to );
this.Controls.Add(this.txtSpecifications_to );

                this.Controls.Add(this.lblproperty_to );
this.Controls.Add(this.txtproperty_to );

                this.Controls.Add(this.lblSummary );
this.Controls.Add(this.txtSummary );

                    
            this.Name = "View_ProdConversionItemsQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblConversionNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtConversionNo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblConversionDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpConversionDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblReason;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtReason;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblNotes;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprovalOpinions;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtApprovalOpinions;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblApprovalResults;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkApprovalResults;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblBarCode_from;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtBarCode_from;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSKU_from;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSKU_from;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCNName_from;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCNName_from;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModel_from;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtModel_from;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSpecifications_from;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSpecifications_from;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblproperty_from;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtproperty_from;

    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblBarCode_to;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtBarCode_to;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSKU_to;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSKU_to;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCNName_to;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCNName_to;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModel_to;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtModel_to;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSpecifications_to;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSpecifications_to;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblproperty_to;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtproperty_to;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSummary;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSummary;

    
    
   
 





    }
}


