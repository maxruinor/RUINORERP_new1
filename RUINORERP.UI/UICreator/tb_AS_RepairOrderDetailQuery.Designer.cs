
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/08/2025 19:05:32
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 维修工单明细
    /// </summary>
    partial class tb_AS_RepairOrderDetailQuery
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

this.lblProdDetailID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbProdDetailID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblLocation_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbLocation_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblproperty = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtproperty = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtproperty.Multiline = true;


this.lblSummary = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSummary = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtSummary.Multiline = true;

this.lblRepairContent = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtRepairContent = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtRepairContent.Multiline = true;

this.lblMaterialCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtMaterialCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblLaborCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtLaborCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSubtotalCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSubtotalCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTaxRate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTaxRate = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSubtotalTaxAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSubtotalTaxAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSubtotalCostAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSubtotalCostAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


    //for end
    this.SuspendLayout();
    
         //for start
                 //#####RepairOrderID###Int64
//属性测试25RepairOrderID
//属性测试25RepairOrderID
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

           //#####ProdDetailID###Int64
//属性测试50ProdDetailID
//属性测试50ProdDetailID
this.lblProdDetailID.AutoSize = true;
this.lblProdDetailID.Location = new System.Drawing.Point(100,50);
this.lblProdDetailID.Name = "lblProdDetailID";
this.lblProdDetailID.Size = new System.Drawing.Size(41, 12);
this.lblProdDetailID.TabIndex = 2;
this.lblProdDetailID.Text = "产品详情";
//111======50
this.cmbProdDetailID.Location = new System.Drawing.Point(173,46);
this.cmbProdDetailID.Name ="cmbProdDetailID";
this.cmbProdDetailID.Size = new System.Drawing.Size(100, 21);
this.cmbProdDetailID.TabIndex = 2;
this.Controls.Add(this.lblProdDetailID);
this.Controls.Add(this.cmbProdDetailID);

           //#####Location_ID###Int64
//属性测试75Location_ID
this.lblLocation_ID.AutoSize = true;
this.lblLocation_ID.Location = new System.Drawing.Point(100,75);
this.lblLocation_ID.Name = "lblLocation_ID";
this.lblLocation_ID.Size = new System.Drawing.Size(41, 12);
this.lblLocation_ID.TabIndex = 3;
this.lblLocation_ID.Text = "库位";
//111======75
this.cmbLocation_ID.Location = new System.Drawing.Point(173,71);
this.cmbLocation_ID.Name ="cmbLocation_ID";
this.cmbLocation_ID.Size = new System.Drawing.Size(100, 21);
this.cmbLocation_ID.TabIndex = 3;
this.Controls.Add(this.lblLocation_ID);
this.Controls.Add(this.cmbLocation_ID);

           //#####255property###String
this.lblproperty.AutoSize = true;
this.lblproperty.Location = new System.Drawing.Point(100,100);
this.lblproperty.Name = "lblproperty";
this.lblproperty.Size = new System.Drawing.Size(41, 12);
this.lblproperty.TabIndex = 4;
this.lblproperty.Text = "属性";
this.txtproperty.Location = new System.Drawing.Point(173,96);
this.txtproperty.Name = "txtproperty";
this.txtproperty.Size = new System.Drawing.Size(100, 21);
this.txtproperty.TabIndex = 4;
this.Controls.Add(this.lblproperty);
this.Controls.Add(this.txtproperty);

           //#####Quantity###Int32
//属性测试125Quantity
//属性测试125Quantity
//属性测试125Quantity

           //#####1000Summary###String
this.lblSummary.AutoSize = true;
this.lblSummary.Location = new System.Drawing.Point(100,150);
this.lblSummary.Name = "lblSummary";
this.lblSummary.Size = new System.Drawing.Size(41, 12);
this.lblSummary.TabIndex = 6;
this.lblSummary.Text = "摘要";
this.txtSummary.Location = new System.Drawing.Point(173,146);
this.txtSummary.Name = "txtSummary";
this.txtSummary.Size = new System.Drawing.Size(100, 21);
this.txtSummary.TabIndex = 6;
this.Controls.Add(this.lblSummary);
this.Controls.Add(this.txtSummary);

           //#####500RepairContent###String
this.lblRepairContent.AutoSize = true;
this.lblRepairContent.Location = new System.Drawing.Point(100,175);
this.lblRepairContent.Name = "lblRepairContent";
this.lblRepairContent.Size = new System.Drawing.Size(41, 12);
this.lblRepairContent.TabIndex = 7;
this.lblRepairContent.Text = "维修内容";
this.txtRepairContent.Location = new System.Drawing.Point(173,171);
this.txtRepairContent.Name = "txtRepairContent";
this.txtRepairContent.Size = new System.Drawing.Size(100, 21);
this.txtRepairContent.TabIndex = 7;
this.Controls.Add(this.lblRepairContent);
this.Controls.Add(this.txtRepairContent);

           //#####MaterialCost###Decimal
this.lblMaterialCost.AutoSize = true;
this.lblMaterialCost.Location = new System.Drawing.Point(100,200);
this.lblMaterialCost.Name = "lblMaterialCost";
this.lblMaterialCost.Size = new System.Drawing.Size(41, 12);
this.lblMaterialCost.TabIndex = 8;
this.lblMaterialCost.Text = "单项材料费用";
//111======200
this.txtMaterialCost.Location = new System.Drawing.Point(173,196);
this.txtMaterialCost.Name ="txtMaterialCost";
this.txtMaterialCost.Size = new System.Drawing.Size(100, 21);
this.txtMaterialCost.TabIndex = 8;
this.Controls.Add(this.lblMaterialCost);
this.Controls.Add(this.txtMaterialCost);

           //#####LaborCost###Decimal
this.lblLaborCost.AutoSize = true;
this.lblLaborCost.Location = new System.Drawing.Point(100,225);
this.lblLaborCost.Name = "lblLaborCost";
this.lblLaborCost.Size = new System.Drawing.Size(41, 12);
this.lblLaborCost.TabIndex = 9;
this.lblLaborCost.Text = "单项人工费用";
//111======225
this.txtLaborCost.Location = new System.Drawing.Point(173,221);
this.txtLaborCost.Name ="txtLaborCost";
this.txtLaborCost.Size = new System.Drawing.Size(100, 21);
this.txtLaborCost.TabIndex = 9;
this.Controls.Add(this.lblLaborCost);
this.Controls.Add(this.txtLaborCost);

           //#####SubtotalCost###Decimal
this.lblSubtotalCost.AutoSize = true;
this.lblSubtotalCost.Location = new System.Drawing.Point(100,250);
this.lblSubtotalCost.Name = "lblSubtotalCost";
this.lblSubtotalCost.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalCost.TabIndex = 10;
this.lblSubtotalCost.Text = "费用小计";
//111======250
this.txtSubtotalCost.Location = new System.Drawing.Point(173,246);
this.txtSubtotalCost.Name ="txtSubtotalCost";
this.txtSubtotalCost.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalCost.TabIndex = 10;
this.Controls.Add(this.lblSubtotalCost);
this.Controls.Add(this.txtSubtotalCost);

           //#####TaxRate###Decimal
this.lblTaxRate.AutoSize = true;
this.lblTaxRate.Location = new System.Drawing.Point(100,275);
this.lblTaxRate.Name = "lblTaxRate";
this.lblTaxRate.Size = new System.Drawing.Size(41, 12);
this.lblTaxRate.TabIndex = 11;
this.lblTaxRate.Text = "税率";
//111======275
this.txtTaxRate.Location = new System.Drawing.Point(173,271);
this.txtTaxRate.Name ="txtTaxRate";
this.txtTaxRate.Size = new System.Drawing.Size(100, 21);
this.txtTaxRate.TabIndex = 11;
this.Controls.Add(this.lblTaxRate);
this.Controls.Add(this.txtTaxRate);

           //#####SubtotalTaxAmount###Decimal
this.lblSubtotalTaxAmount.AutoSize = true;
this.lblSubtotalTaxAmount.Location = new System.Drawing.Point(100,300);
this.lblSubtotalTaxAmount.Name = "lblSubtotalTaxAmount";
this.lblSubtotalTaxAmount.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalTaxAmount.TabIndex = 12;
this.lblSubtotalTaxAmount.Text = "税额";
//111======300
this.txtSubtotalTaxAmount.Location = new System.Drawing.Point(173,296);
this.txtSubtotalTaxAmount.Name ="txtSubtotalTaxAmount";
this.txtSubtotalTaxAmount.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalTaxAmount.TabIndex = 12;
this.Controls.Add(this.lblSubtotalTaxAmount);
this.Controls.Add(this.txtSubtotalTaxAmount);

           //#####SubtotalCostAmount###Decimal
this.lblSubtotalCostAmount.AutoSize = true;
this.lblSubtotalCostAmount.Location = new System.Drawing.Point(100,325);
this.lblSubtotalCostAmount.Name = "lblSubtotalCostAmount";
this.lblSubtotalCostAmount.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalCostAmount.TabIndex = 13;
this.lblSubtotalCostAmount.Text = "成本小计";
//111======325
this.txtSubtotalCostAmount.Location = new System.Drawing.Point(173,321);
this.txtSubtotalCostAmount.Name ="txtSubtotalCostAmount";
this.txtSubtotalCostAmount.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalCostAmount.TabIndex = 13;
this.Controls.Add(this.lblSubtotalCostAmount);
this.Controls.Add(this.txtSubtotalCostAmount);

           //#####TotalReturnedQty###Int32
//属性测试350TotalReturnedQty
//属性测试350TotalReturnedQty
//属性测试350TotalReturnedQty

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblRepairOrderID );
this.Controls.Add(this.cmbRepairOrderID );

                this.Controls.Add(this.lblProdDetailID );
this.Controls.Add(this.cmbProdDetailID );

                this.Controls.Add(this.lblLocation_ID );
this.Controls.Add(this.cmbLocation_ID );

                this.Controls.Add(this.lblproperty );
this.Controls.Add(this.txtproperty );

                
                this.Controls.Add(this.lblSummary );
this.Controls.Add(this.txtSummary );

                this.Controls.Add(this.lblRepairContent );
this.Controls.Add(this.txtRepairContent );

                this.Controls.Add(this.lblMaterialCost );
this.Controls.Add(this.txtMaterialCost );

                this.Controls.Add(this.lblLaborCost );
this.Controls.Add(this.txtLaborCost );

                this.Controls.Add(this.lblSubtotalCost );
this.Controls.Add(this.txtSubtotalCost );

                this.Controls.Add(this.lblTaxRate );
this.Controls.Add(this.txtTaxRate );

                this.Controls.Add(this.lblSubtotalTaxAmount );
this.Controls.Add(this.txtSubtotalTaxAmount );

                this.Controls.Add(this.lblSubtotalCostAmount );
this.Controls.Add(this.txtSubtotalCostAmount );

                
                    
            this.Name = "tb_AS_RepairOrderDetailQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRepairOrderID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbRepairOrderID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProdDetailID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbProdDetailID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblLocation_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbLocation_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblproperty;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtproperty;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSummary;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSummary;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRepairContent;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtRepairContent;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblMaterialCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtMaterialCost;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblLaborCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtLaborCost;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSubtotalCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSubtotalCost;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTaxRate;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTaxRate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSubtotalTaxAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSubtotalTaxAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSubtotalCostAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSubtotalCostAmount;

    
        
              
    
    
   
 





    }
}


