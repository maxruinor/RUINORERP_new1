
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/22/2025 21:05:37
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 损益费用单
    /// </summary>
    partial class tb_FM_ProfitLossDetailQuery
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
     
     this.lblProfitLossId = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbProfitLossId = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblProdDetailID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbProdDetailID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblproperty = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtproperty = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtproperty.Multiline = true;



this.lblExpenseDescription = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtExpenseDescription = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtExpenseDescription.Multiline = true;

this.lblUnitPrice = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtUnitPrice = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblQuantity = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtQuantity = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSubtotalAmont = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSubtotalAmont = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblUntaxedSubtotalAmont = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtUntaxedSubtotalAmont = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTaxRate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTaxRate = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTaxSubtotalAmont = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTaxSubtotalAmont = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSummary = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSummary = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtSummary.Multiline = true;

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####ProfitLossId###Int64
//属性测试25ProfitLossId
this.lblProfitLossId.AutoSize = true;
this.lblProfitLossId.Location = new System.Drawing.Point(100,25);
this.lblProfitLossId.Name = "lblProfitLossId";
this.lblProfitLossId.Size = new System.Drawing.Size(41, 12);
this.lblProfitLossId.TabIndex = 1;
this.lblProfitLossId.Text = "损益费用单";
//111======25
this.cmbProfitLossId.Location = new System.Drawing.Point(173,21);
this.cmbProfitLossId.Name ="cmbProfitLossId";
this.cmbProfitLossId.Size = new System.Drawing.Size(100, 21);
this.cmbProfitLossId.TabIndex = 1;
this.Controls.Add(this.lblProfitLossId);
this.Controls.Add(this.cmbProfitLossId);

           //#####ProdDetailID###Int64
//属性测试50ProdDetailID
//属性测试50ProdDetailID
this.lblProdDetailID.AutoSize = true;
this.lblProdDetailID.Location = new System.Drawing.Point(100,50);
this.lblProdDetailID.Name = "lblProdDetailID";
this.lblProdDetailID.Size = new System.Drawing.Size(41, 12);
this.lblProdDetailID.TabIndex = 2;
this.lblProdDetailID.Text = "产品";
//111======50
this.cmbProdDetailID.Location = new System.Drawing.Point(173,46);
this.cmbProdDetailID.Name ="cmbProdDetailID";
this.cmbProdDetailID.Size = new System.Drawing.Size(100, 21);
this.cmbProdDetailID.TabIndex = 2;
this.Controls.Add(this.lblProdDetailID);
this.Controls.Add(this.cmbProdDetailID);

           //#####255property###String
this.lblproperty.AutoSize = true;
this.lblproperty.Location = new System.Drawing.Point(100,75);
this.lblproperty.Name = "lblproperty";
this.lblproperty.Size = new System.Drawing.Size(41, 12);
this.lblproperty.TabIndex = 3;
this.lblproperty.Text = "属性";
this.txtproperty.Location = new System.Drawing.Point(173,71);
this.txtproperty.Name = "txtproperty";
this.txtproperty.Size = new System.Drawing.Size(100, 21);
this.txtproperty.TabIndex = 3;
this.Controls.Add(this.lblproperty);
this.Controls.Add(this.txtproperty);

           //#####ProfitLossType###Int32
//属性测试100ProfitLossType
//属性测试100ProfitLossType

           //#####IncomeExpenseDirection###Int32
//属性测试125IncomeExpenseDirection
//属性测试125IncomeExpenseDirection

           //#####300ExpenseDescription###String
this.lblExpenseDescription.AutoSize = true;
this.lblExpenseDescription.Location = new System.Drawing.Point(100,150);
this.lblExpenseDescription.Name = "lblExpenseDescription";
this.lblExpenseDescription.Size = new System.Drawing.Size(41, 12);
this.lblExpenseDescription.TabIndex = 6;
this.lblExpenseDescription.Text = "费用说明";
this.txtExpenseDescription.Location = new System.Drawing.Point(173,146);
this.txtExpenseDescription.Name = "txtExpenseDescription";
this.txtExpenseDescription.Size = new System.Drawing.Size(100, 21);
this.txtExpenseDescription.TabIndex = 6;
this.Controls.Add(this.lblExpenseDescription);
this.Controls.Add(this.txtExpenseDescription);

           //#####UnitPrice###Decimal
this.lblUnitPrice.AutoSize = true;
this.lblUnitPrice.Location = new System.Drawing.Point(100,175);
this.lblUnitPrice.Name = "lblUnitPrice";
this.lblUnitPrice.Size = new System.Drawing.Size(41, 12);
this.lblUnitPrice.TabIndex = 7;
this.lblUnitPrice.Text = "单价";
//111======175
this.txtUnitPrice.Location = new System.Drawing.Point(173,171);
this.txtUnitPrice.Name ="txtUnitPrice";
this.txtUnitPrice.Size = new System.Drawing.Size(100, 21);
this.txtUnitPrice.TabIndex = 7;
this.Controls.Add(this.lblUnitPrice);
this.Controls.Add(this.txtUnitPrice);

           //#####Quantity###Decimal
this.lblQuantity.AutoSize = true;
this.lblQuantity.Location = new System.Drawing.Point(100,200);
this.lblQuantity.Name = "lblQuantity";
this.lblQuantity.Size = new System.Drawing.Size(41, 12);
this.lblQuantity.TabIndex = 8;
this.lblQuantity.Text = "数量";
//111======200
this.txtQuantity.Location = new System.Drawing.Point(173,196);
this.txtQuantity.Name ="txtQuantity";
this.txtQuantity.Size = new System.Drawing.Size(100, 21);
this.txtQuantity.TabIndex = 8;
this.Controls.Add(this.lblQuantity);
this.Controls.Add(this.txtQuantity);

           //#####SubtotalAmont###Decimal
this.lblSubtotalAmont.AutoSize = true;
this.lblSubtotalAmont.Location = new System.Drawing.Point(100,225);
this.lblSubtotalAmont.Name = "lblSubtotalAmont";
this.lblSubtotalAmont.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalAmont.TabIndex = 9;
this.lblSubtotalAmont.Text = "金额小计";
//111======225
this.txtSubtotalAmont.Location = new System.Drawing.Point(173,221);
this.txtSubtotalAmont.Name ="txtSubtotalAmont";
this.txtSubtotalAmont.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalAmont.TabIndex = 9;
this.Controls.Add(this.lblSubtotalAmont);
this.Controls.Add(this.txtSubtotalAmont);

           //#####UntaxedSubtotalAmont###Decimal
this.lblUntaxedSubtotalAmont.AutoSize = true;
this.lblUntaxedSubtotalAmont.Location = new System.Drawing.Point(100,250);
this.lblUntaxedSubtotalAmont.Name = "lblUntaxedSubtotalAmont";
this.lblUntaxedSubtotalAmont.Size = new System.Drawing.Size(41, 12);
this.lblUntaxedSubtotalAmont.TabIndex = 10;
this.lblUntaxedSubtotalAmont.Text = "未税小计";
//111======250
this.txtUntaxedSubtotalAmont.Location = new System.Drawing.Point(173,246);
this.txtUntaxedSubtotalAmont.Name ="txtUntaxedSubtotalAmont";
this.txtUntaxedSubtotalAmont.Size = new System.Drawing.Size(100, 21);
this.txtUntaxedSubtotalAmont.TabIndex = 10;
this.Controls.Add(this.lblUntaxedSubtotalAmont);
this.Controls.Add(this.txtUntaxedSubtotalAmont);

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

           //#####TaxSubtotalAmont###Decimal
this.lblTaxSubtotalAmont.AutoSize = true;
this.lblTaxSubtotalAmont.Location = new System.Drawing.Point(100,300);
this.lblTaxSubtotalAmont.Name = "lblTaxSubtotalAmont";
this.lblTaxSubtotalAmont.Size = new System.Drawing.Size(41, 12);
this.lblTaxSubtotalAmont.TabIndex = 12;
this.lblTaxSubtotalAmont.Text = "税额";
//111======300
this.txtTaxSubtotalAmont.Location = new System.Drawing.Point(173,296);
this.txtTaxSubtotalAmont.Name ="txtTaxSubtotalAmont";
this.txtTaxSubtotalAmont.Size = new System.Drawing.Size(100, 21);
this.txtTaxSubtotalAmont.TabIndex = 12;
this.Controls.Add(this.lblTaxSubtotalAmont);
this.Controls.Add(this.txtTaxSubtotalAmont);

           //#####300Summary###String
this.lblSummary.AutoSize = true;
this.lblSummary.Location = new System.Drawing.Point(100,325);
this.lblSummary.Name = "lblSummary";
this.lblSummary.Size = new System.Drawing.Size(41, 12);
this.lblSummary.TabIndex = 13;
this.lblSummary.Text = "摘要";
this.txtSummary.Location = new System.Drawing.Point(173,321);
this.txtSummary.Name = "txtSummary";
this.txtSummary.Size = new System.Drawing.Size(100, 21);
this.txtSummary.TabIndex = 13;
this.Controls.Add(this.lblSummary);
this.Controls.Add(this.txtSummary);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblProfitLossId );
this.Controls.Add(this.cmbProfitLossId );

                this.Controls.Add(this.lblProdDetailID );
this.Controls.Add(this.cmbProdDetailID );

                this.Controls.Add(this.lblproperty );
this.Controls.Add(this.txtproperty );

                
                
                this.Controls.Add(this.lblExpenseDescription );
this.Controls.Add(this.txtExpenseDescription );

                this.Controls.Add(this.lblUnitPrice );
this.Controls.Add(this.txtUnitPrice );

                this.Controls.Add(this.lblQuantity );
this.Controls.Add(this.txtQuantity );

                this.Controls.Add(this.lblSubtotalAmont );
this.Controls.Add(this.txtSubtotalAmont );

                this.Controls.Add(this.lblUntaxedSubtotalAmont );
this.Controls.Add(this.txtUntaxedSubtotalAmont );

                this.Controls.Add(this.lblTaxRate );
this.Controls.Add(this.txtTaxRate );

                this.Controls.Add(this.lblTaxSubtotalAmont );
this.Controls.Add(this.txtTaxSubtotalAmont );

                this.Controls.Add(this.lblSummary );
this.Controls.Add(this.txtSummary );

                    
            this.Name = "tb_FM_ProfitLossDetailQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProfitLossId;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbProfitLossId;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProdDetailID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbProdDetailID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblproperty;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtproperty;

    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblExpenseDescription;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtExpenseDescription;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblUnitPrice;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtUnitPrice;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblQuantity;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtQuantity;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSubtotalAmont;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSubtotalAmont;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblUntaxedSubtotalAmont;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtUntaxedSubtotalAmont;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTaxRate;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTaxRate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTaxSubtotalAmont;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTaxSubtotalAmont;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSummary;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSummary;

    
    
   
 





    }
}


