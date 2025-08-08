
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:35
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 计划单明细统计
    /// </summary>
    partial class View_ProductionPlanItemsQuery
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
     
     this.lblSaleOrderNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSaleOrderNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblPPNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPPNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();





this.lblRequirementDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpRequirementDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblPlanDate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpPlanDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lblCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpCreated_at = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();



this.lblSpecifications = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSpecifications = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtSpecifications.Multiline = true;

this.lblproperty = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtproperty = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtproperty.Multiline = true;




this.lblSKU = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSKU = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCNName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCNName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtCNName.Multiline = true;

this.lblProductNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtProductNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblModel = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtModel = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();



this.lblBarCode = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtBarCode = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblShortCode = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtShortCode = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblisdeleted = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkisdeleted = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkisdeleted.Values.Text ="";

this.lblIsAnalyzed = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIsAnalyzed = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIsAnalyzed.Values.Text ="";



this.lblSummary = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSummary = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtSummary.Multiline = true;

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####50SaleOrderNo###String
this.lblSaleOrderNo.AutoSize = true;
this.lblSaleOrderNo.Location = new System.Drawing.Point(100,25);
this.lblSaleOrderNo.Name = "lblSaleOrderNo";
this.lblSaleOrderNo.Size = new System.Drawing.Size(41, 12);
this.lblSaleOrderNo.TabIndex = 1;
this.lblSaleOrderNo.Text = "";
this.txtSaleOrderNo.Location = new System.Drawing.Point(173,21);
this.txtSaleOrderNo.Name = "txtSaleOrderNo";
this.txtSaleOrderNo.Size = new System.Drawing.Size(100, 21);
this.txtSaleOrderNo.TabIndex = 1;
this.Controls.Add(this.lblSaleOrderNo);
this.Controls.Add(this.txtSaleOrderNo);

           //#####100PPNo###String
this.lblPPNo.AutoSize = true;
this.lblPPNo.Location = new System.Drawing.Point(100,50);
this.lblPPNo.Name = "lblPPNo";
this.lblPPNo.Size = new System.Drawing.Size(41, 12);
this.lblPPNo.TabIndex = 2;
this.lblPPNo.Text = "";
this.txtPPNo.Location = new System.Drawing.Point(173,46);
this.txtPPNo.Name = "txtPPNo";
this.txtPPNo.Size = new System.Drawing.Size(100, 21);
this.txtPPNo.TabIndex = 2;
this.Controls.Add(this.lblPPNo);
this.Controls.Add(this.txtPPNo);

           //#####ProjectGroup_ID###Int64

           //#####DepartmentID###Int64

           //#####Priority###Int32

           //#####Employee_ID###Int64

           //#####RequirementDate###DateTime
this.lblRequirementDate.AutoSize = true;
this.lblRequirementDate.Location = new System.Drawing.Point(100,175);
this.lblRequirementDate.Name = "lblRequirementDate";
this.lblRequirementDate.Size = new System.Drawing.Size(41, 12);
this.lblRequirementDate.TabIndex = 7;
this.lblRequirementDate.Text = "";
//111======175
this.dtpRequirementDate.Location = new System.Drawing.Point(173,171);
this.dtpRequirementDate.Name ="dtpRequirementDate";
this.dtpRequirementDate.ShowCheckBox =true;
this.dtpRequirementDate.Size = new System.Drawing.Size(100, 21);
this.dtpRequirementDate.TabIndex = 7;
this.Controls.Add(this.lblRequirementDate);
this.Controls.Add(this.dtpRequirementDate);

           //#####PlanDate###DateTime
this.lblPlanDate.AutoSize = true;
this.lblPlanDate.Location = new System.Drawing.Point(100,200);
this.lblPlanDate.Name = "lblPlanDate";
this.lblPlanDate.Size = new System.Drawing.Size(41, 12);
this.lblPlanDate.TabIndex = 8;
this.lblPlanDate.Text = "";
//111======200
this.dtpPlanDate.Location = new System.Drawing.Point(173,196);
this.dtpPlanDate.Name ="dtpPlanDate";
this.dtpPlanDate.ShowCheckBox =true;
this.dtpPlanDate.Size = new System.Drawing.Size(100, 21);
this.dtpPlanDate.TabIndex = 8;
this.Controls.Add(this.lblPlanDate);
this.Controls.Add(this.dtpPlanDate);

           //#####DataStatus###Int32

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

           //#####Created_by###Int64

           //#####ProdDetailID###Int64

           //#####1000Specifications###String
this.lblSpecifications.AutoSize = true;
this.lblSpecifications.Location = new System.Drawing.Point(100,325);
this.lblSpecifications.Name = "lblSpecifications";
this.lblSpecifications.Size = new System.Drawing.Size(41, 12);
this.lblSpecifications.TabIndex = 13;
this.lblSpecifications.Text = "";
this.txtSpecifications.Location = new System.Drawing.Point(173,321);
this.txtSpecifications.Name = "txtSpecifications";
this.txtSpecifications.Size = new System.Drawing.Size(100, 21);
this.txtSpecifications.TabIndex = 13;
this.Controls.Add(this.lblSpecifications);
this.Controls.Add(this.txtSpecifications);

           //#####255property###String
this.lblproperty.AutoSize = true;
this.lblproperty.Location = new System.Drawing.Point(100,350);
this.lblproperty.Name = "lblproperty";
this.lblproperty.Size = new System.Drawing.Size(41, 12);
this.lblproperty.TabIndex = 14;
this.lblproperty.Text = "";
this.txtproperty.Location = new System.Drawing.Point(173,346);
this.txtproperty.Name = "txtproperty";
this.txtproperty.Size = new System.Drawing.Size(100, 21);
this.txtproperty.TabIndex = 14;
this.Controls.Add(this.lblproperty);
this.Controls.Add(this.txtproperty);

           //#####Location_ID###Int64

           //#####Quantity###Int32

           //#####BOM_ID###Int64

           //#####80SKU###String
this.lblSKU.AutoSize = true;
this.lblSKU.Location = new System.Drawing.Point(100,450);
this.lblSKU.Name = "lblSKU";
this.lblSKU.Size = new System.Drawing.Size(41, 12);
this.lblSKU.TabIndex = 18;
this.lblSKU.Text = "";
this.txtSKU.Location = new System.Drawing.Point(173,446);
this.txtSKU.Name = "txtSKU";
this.txtSKU.Size = new System.Drawing.Size(100, 21);
this.txtSKU.TabIndex = 18;
this.Controls.Add(this.lblSKU);
this.Controls.Add(this.txtSKU);

           //#####255CNName###String
this.lblCNName.AutoSize = true;
this.lblCNName.Location = new System.Drawing.Point(100,475);
this.lblCNName.Name = "lblCNName";
this.lblCNName.Size = new System.Drawing.Size(41, 12);
this.lblCNName.TabIndex = 19;
this.lblCNName.Text = "";
this.txtCNName.Location = new System.Drawing.Point(173,471);
this.txtCNName.Name = "txtCNName";
this.txtCNName.Size = new System.Drawing.Size(100, 21);
this.txtCNName.TabIndex = 19;
this.Controls.Add(this.lblCNName);
this.Controls.Add(this.txtCNName);

           //#####40ProductNo###String
this.lblProductNo.AutoSize = true;
this.lblProductNo.Location = new System.Drawing.Point(100,500);
this.lblProductNo.Name = "lblProductNo";
this.lblProductNo.Size = new System.Drawing.Size(41, 12);
this.lblProductNo.TabIndex = 20;
this.lblProductNo.Text = "";
this.txtProductNo.Location = new System.Drawing.Point(173,496);
this.txtProductNo.Name = "txtProductNo";
this.txtProductNo.Size = new System.Drawing.Size(100, 21);
this.txtProductNo.TabIndex = 20;
this.Controls.Add(this.lblProductNo);
this.Controls.Add(this.txtProductNo);

           //#####Unit_ID###Int64

           //#####50Model###String
this.lblModel.AutoSize = true;
this.lblModel.Location = new System.Drawing.Point(100,550);
this.lblModel.Name = "lblModel";
this.lblModel.Size = new System.Drawing.Size(41, 12);
this.lblModel.TabIndex = 22;
this.lblModel.Text = "";
this.txtModel.Location = new System.Drawing.Point(173,546);
this.txtModel.Name = "txtModel";
this.txtModel.Size = new System.Drawing.Size(100, 21);
this.txtModel.TabIndex = 22;
this.Controls.Add(this.lblModel);
this.Controls.Add(this.txtModel);

           //#####Category_ID###Int64

           //#####Type_ID###Int64

           //#####50BarCode###String
this.lblBarCode.AutoSize = true;
this.lblBarCode.Location = new System.Drawing.Point(100,625);
this.lblBarCode.Name = "lblBarCode";
this.lblBarCode.Size = new System.Drawing.Size(41, 12);
this.lblBarCode.TabIndex = 25;
this.lblBarCode.Text = "";
this.txtBarCode.Location = new System.Drawing.Point(173,621);
this.txtBarCode.Name = "txtBarCode";
this.txtBarCode.Size = new System.Drawing.Size(100, 21);
this.txtBarCode.TabIndex = 25;
this.Controls.Add(this.lblBarCode);
this.Controls.Add(this.txtBarCode);

           //#####50ShortCode###String
this.lblShortCode.AutoSize = true;
this.lblShortCode.Location = new System.Drawing.Point(100,650);
this.lblShortCode.Name = "lblShortCode";
this.lblShortCode.Size = new System.Drawing.Size(41, 12);
this.lblShortCode.TabIndex = 26;
this.lblShortCode.Text = "";
this.txtShortCode.Location = new System.Drawing.Point(173,646);
this.txtShortCode.Name = "txtShortCode";
this.txtShortCode.Size = new System.Drawing.Size(100, 21);
this.txtShortCode.TabIndex = 26;
this.Controls.Add(this.lblShortCode);
this.Controls.Add(this.txtShortCode);

           //#####isdeleted###Boolean
this.lblisdeleted.AutoSize = true;
this.lblisdeleted.Location = new System.Drawing.Point(100,675);
this.lblisdeleted.Name = "lblisdeleted";
this.lblisdeleted.Size = new System.Drawing.Size(41, 12);
this.lblisdeleted.TabIndex = 27;
this.lblisdeleted.Text = "";
this.chkisdeleted.Location = new System.Drawing.Point(173,671);
this.chkisdeleted.Name = "chkisdeleted";
this.chkisdeleted.Size = new System.Drawing.Size(100, 21);
this.chkisdeleted.TabIndex = 27;
this.Controls.Add(this.lblisdeleted);
this.Controls.Add(this.chkisdeleted);

           //#####IsAnalyzed###Boolean
this.lblIsAnalyzed.AutoSize = true;
this.lblIsAnalyzed.Location = new System.Drawing.Point(100,700);
this.lblIsAnalyzed.Name = "lblIsAnalyzed";
this.lblIsAnalyzed.Size = new System.Drawing.Size(41, 12);
this.lblIsAnalyzed.TabIndex = 28;
this.lblIsAnalyzed.Text = "";
this.chkIsAnalyzed.Location = new System.Drawing.Point(173,696);
this.chkIsAnalyzed.Name = "chkIsAnalyzed";
this.chkIsAnalyzed.Size = new System.Drawing.Size(100, 21);
this.chkIsAnalyzed.TabIndex = 28;
this.Controls.Add(this.lblIsAnalyzed);
this.Controls.Add(this.chkIsAnalyzed);

           //#####AnalyzedQuantity###Int32

           //#####CompletedQuantity###Int32

           //#####1000Summary###String
this.lblSummary.AutoSize = true;
this.lblSummary.Location = new System.Drawing.Point(100,775);
this.lblSummary.Name = "lblSummary";
this.lblSummary.Size = new System.Drawing.Size(41, 12);
this.lblSummary.TabIndex = 31;
this.lblSummary.Text = "";
this.txtSummary.Location = new System.Drawing.Point(173,771);
this.txtSummary.Name = "txtSummary";
this.txtSummary.Size = new System.Drawing.Size(100, 21);
this.txtSummary.TabIndex = 31;
this.Controls.Add(this.lblSummary);
this.Controls.Add(this.txtSummary);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblSaleOrderNo );
this.Controls.Add(this.txtSaleOrderNo );

                this.Controls.Add(this.lblPPNo );
this.Controls.Add(this.txtPPNo );

                
                
                
                
                this.Controls.Add(this.lblRequirementDate );
this.Controls.Add(this.dtpRequirementDate );

                this.Controls.Add(this.lblPlanDate );
this.Controls.Add(this.dtpPlanDate );

                
                this.Controls.Add(this.lblCreated_at );
this.Controls.Add(this.dtpCreated_at );

                
                
                this.Controls.Add(this.lblSpecifications );
this.Controls.Add(this.txtSpecifications );

                this.Controls.Add(this.lblproperty );
this.Controls.Add(this.txtproperty );

                
                
                
                this.Controls.Add(this.lblSKU );
this.Controls.Add(this.txtSKU );

                this.Controls.Add(this.lblCNName );
this.Controls.Add(this.txtCNName );

                this.Controls.Add(this.lblProductNo );
this.Controls.Add(this.txtProductNo );

                
                this.Controls.Add(this.lblModel );
this.Controls.Add(this.txtModel );

                
                
                this.Controls.Add(this.lblBarCode );
this.Controls.Add(this.txtBarCode );

                this.Controls.Add(this.lblShortCode );
this.Controls.Add(this.txtShortCode );

                this.Controls.Add(this.lblisdeleted );
this.Controls.Add(this.chkisdeleted );

                this.Controls.Add(this.lblIsAnalyzed );
this.Controls.Add(this.chkIsAnalyzed );

                
                
                this.Controls.Add(this.lblSummary );
this.Controls.Add(this.txtSummary );

                    
            this.Name = "View_ProductionPlanItemsQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSaleOrderNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSaleOrderNo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPPNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPPNo;

    
        
              
    
        
              
    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRequirementDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpRequirementDate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPlanDate;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpPlanDate;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCreated_at;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpCreated_at;

    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSpecifications;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSpecifications;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblproperty;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtproperty;

    
        
              
    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSKU;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSKU;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCNName;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCNName;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProductNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtProductNo;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblModel;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtModel;

    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblBarCode;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtBarCode;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblShortCode;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtShortCode;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblisdeleted;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkisdeleted;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIsAnalyzed;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIsAnalyzed;

    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSummary;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSummary;

    
    
   
 





    }
}


