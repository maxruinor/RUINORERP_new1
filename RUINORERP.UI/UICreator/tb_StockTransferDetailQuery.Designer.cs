
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：10/15/2024 18:45:37
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 调拨单明细
    /// </summary>
    partial class tb_StockTransferDetailQuery
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
     
     this.lblStockTransferID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbStockTransferID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblProdDetailID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbProdDetailID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblproperty = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtproperty = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtproperty.Multiline = true;


this.lblTransPrice = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTransPrice = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSummary = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSummary = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtSummary.Multiline = true;

this.lblSubtotalCostAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSubtotalCostAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSubtotalTransferPirceAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSubtotalTransferPirceAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####StockTransferID###Int64
//属性测试25StockTransferID
this.lblStockTransferID.AutoSize = true;
this.lblStockTransferID.Location = new System.Drawing.Point(100,25);
this.lblStockTransferID.Name = "lblStockTransferID";
this.lblStockTransferID.Size = new System.Drawing.Size(41, 12);
this.lblStockTransferID.TabIndex = 1;
this.lblStockTransferID.Text = "";
//111======25
this.cmbStockTransferID.Location = new System.Drawing.Point(173,21);
this.cmbStockTransferID.Name ="cmbStockTransferID";
this.cmbStockTransferID.Size = new System.Drawing.Size(100, 21);
this.cmbStockTransferID.TabIndex = 1;
this.Controls.Add(this.lblStockTransferID);
this.Controls.Add(this.cmbStockTransferID);

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

           //#####Qty###Int32
//属性测试100Qty
//属性测试100Qty

           //#####TransPrice###Decimal
this.lblTransPrice.AutoSize = true;
this.lblTransPrice.Location = new System.Drawing.Point(100,125);
this.lblTransPrice.Name = "lblTransPrice";
this.lblTransPrice.Size = new System.Drawing.Size(41, 12);
this.lblTransPrice.TabIndex = 5;
this.lblTransPrice.Text = "调拨价";
//111======125
this.txtTransPrice.Location = new System.Drawing.Point(173,121);
this.txtTransPrice.Name ="txtTransPrice";
this.txtTransPrice.Size = new System.Drawing.Size(100, 21);
this.txtTransPrice.TabIndex = 5;
this.Controls.Add(this.lblTransPrice);
this.Controls.Add(this.txtTransPrice);

           //#####Cost###Decimal
this.lblCost.AutoSize = true;
this.lblCost.Location = new System.Drawing.Point(100,150);
this.lblCost.Name = "lblCost";
this.lblCost.Size = new System.Drawing.Size(41, 12);
this.lblCost.TabIndex = 6;
this.lblCost.Text = "成本";
//111======150
this.txtCost.Location = new System.Drawing.Point(173,146);
this.txtCost.Name ="txtCost";
this.txtCost.Size = new System.Drawing.Size(100, 21);
this.txtCost.TabIndex = 6;
this.Controls.Add(this.lblCost);
this.Controls.Add(this.txtCost);

           //#####500Summary###String
this.lblSummary.AutoSize = true;
this.lblSummary.Location = new System.Drawing.Point(100,175);
this.lblSummary.Name = "lblSummary";
this.lblSummary.Size = new System.Drawing.Size(41, 12);
this.lblSummary.TabIndex = 7;
this.lblSummary.Text = "摘要";
this.txtSummary.Location = new System.Drawing.Point(173,171);
this.txtSummary.Name = "txtSummary";
this.txtSummary.Size = new System.Drawing.Size(100, 21);
this.txtSummary.TabIndex = 7;
this.Controls.Add(this.lblSummary);
this.Controls.Add(this.txtSummary);

           //#####SubtotalCostAmount###Decimal
this.lblSubtotalCostAmount.AutoSize = true;
this.lblSubtotalCostAmount.Location = new System.Drawing.Point(100,200);
this.lblSubtotalCostAmount.Name = "lblSubtotalCostAmount";
this.lblSubtotalCostAmount.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalCostAmount.TabIndex = 8;
this.lblSubtotalCostAmount.Text = "成本小计";
//111======200
this.txtSubtotalCostAmount.Location = new System.Drawing.Point(173,196);
this.txtSubtotalCostAmount.Name ="txtSubtotalCostAmount";
this.txtSubtotalCostAmount.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalCostAmount.TabIndex = 8;
this.Controls.Add(this.lblSubtotalCostAmount);
this.Controls.Add(this.txtSubtotalCostAmount);

           //#####SubtotalTransferPirceAmount###Decimal
this.lblSubtotalTransferPirceAmount.AutoSize = true;
this.lblSubtotalTransferPirceAmount.Location = new System.Drawing.Point(100,225);
this.lblSubtotalTransferPirceAmount.Name = "lblSubtotalTransferPirceAmount";
this.lblSubtotalTransferPirceAmount.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalTransferPirceAmount.TabIndex = 9;
this.lblSubtotalTransferPirceAmount.Text = "调拨小计";
//111======225
this.txtSubtotalTransferPirceAmount.Location = new System.Drawing.Point(173,221);
this.txtSubtotalTransferPirceAmount.Name ="txtSubtotalTransferPirceAmount";
this.txtSubtotalTransferPirceAmount.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalTransferPirceAmount.TabIndex = 9;
this.Controls.Add(this.lblSubtotalTransferPirceAmount);
this.Controls.Add(this.txtSubtotalTransferPirceAmount);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblStockTransferID );
this.Controls.Add(this.cmbStockTransferID );

                this.Controls.Add(this.lblProdDetailID );
this.Controls.Add(this.cmbProdDetailID );

                this.Controls.Add(this.lblproperty );
this.Controls.Add(this.txtproperty );

                
                this.Controls.Add(this.lblTransPrice );
this.Controls.Add(this.txtTransPrice );

                this.Controls.Add(this.lblCost );
this.Controls.Add(this.txtCost );

                this.Controls.Add(this.lblSummary );
this.Controls.Add(this.txtSummary );

                this.Controls.Add(this.lblSubtotalCostAmount );
this.Controls.Add(this.txtSubtotalCostAmount );

                this.Controls.Add(this.lblSubtotalTransferPirceAmount );
this.Controls.Add(this.txtSubtotalTransferPirceAmount );

                    
            this.Name = "tb_StockTransferDetailQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblStockTransferID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbStockTransferID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProdDetailID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbProdDetailID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblproperty;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtproperty;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTransPrice;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTransPrice;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCost;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSummary;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSummary;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSubtotalCostAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSubtotalCostAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSubtotalTransferPirceAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSubtotalTransferPirceAmount;

    
    
   
 





    }
}


