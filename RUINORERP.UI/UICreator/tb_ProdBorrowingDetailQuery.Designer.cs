
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:53
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 借出单明细
    /// </summary>
    partial class tb_ProdBorrowingDetailQuery
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
     
     this.lblBorrowID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbBorrowID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblLocation_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbLocation_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblProdDetailID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbProdDetailID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblproperty = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtproperty = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtproperty.Multiline = true;



this.lblPrice = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPrice = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSummary = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSummary = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtSummary.Multiline = true;

this.lblSubtotalCostAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSubtotalCostAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSubtotalPirceAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSubtotalPirceAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####BorrowID###Int64
//属性测试25BorrowID
//属性测试25BorrowID
//属性测试25BorrowID
this.lblBorrowID.AutoSize = true;
this.lblBorrowID.Location = new System.Drawing.Point(100,25);
this.lblBorrowID.Name = "lblBorrowID";
this.lblBorrowID.Size = new System.Drawing.Size(41, 12);
this.lblBorrowID.TabIndex = 1;
this.lblBorrowID.Text = "";
//111======25
this.cmbBorrowID.Location = new System.Drawing.Point(173,21);
this.cmbBorrowID.Name ="cmbBorrowID";
this.cmbBorrowID.Size = new System.Drawing.Size(100, 21);
this.cmbBorrowID.TabIndex = 1;
this.Controls.Add(this.lblBorrowID);
this.Controls.Add(this.cmbBorrowID);

           //#####Location_ID###Int64
//属性测试50Location_ID
//属性测试50Location_ID
this.lblLocation_ID.AutoSize = true;
this.lblLocation_ID.Location = new System.Drawing.Point(100,50);
this.lblLocation_ID.Name = "lblLocation_ID";
this.lblLocation_ID.Size = new System.Drawing.Size(41, 12);
this.lblLocation_ID.TabIndex = 2;
this.lblLocation_ID.Text = "库位";
//111======50
this.cmbLocation_ID.Location = new System.Drawing.Point(173,46);
this.cmbLocation_ID.Name ="cmbLocation_ID";
this.cmbLocation_ID.Size = new System.Drawing.Size(100, 21);
this.cmbLocation_ID.TabIndex = 2;
this.Controls.Add(this.lblLocation_ID);
this.Controls.Add(this.cmbLocation_ID);

           //#####ProdDetailID###Int64
//属性测试75ProdDetailID
this.lblProdDetailID.AutoSize = true;
this.lblProdDetailID.Location = new System.Drawing.Point(100,75);
this.lblProdDetailID.Name = "lblProdDetailID";
this.lblProdDetailID.Size = new System.Drawing.Size(41, 12);
this.lblProdDetailID.TabIndex = 3;
this.lblProdDetailID.Text = "货品";
//111======75
this.cmbProdDetailID.Location = new System.Drawing.Point(173,71);
this.cmbProdDetailID.Name ="cmbProdDetailID";
this.cmbProdDetailID.Size = new System.Drawing.Size(100, 21);
this.cmbProdDetailID.TabIndex = 3;
this.Controls.Add(this.lblProdDetailID);
this.Controls.Add(this.cmbProdDetailID);

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

           //#####Qty###Int32
//属性测试125Qty
//属性测试125Qty
//属性测试125Qty

           //#####ReQty###Int32
//属性测试150ReQty
//属性测试150ReQty
//属性测试150ReQty

           //#####Price###Decimal
this.lblPrice.AutoSize = true;
this.lblPrice.Location = new System.Drawing.Point(100,175);
this.lblPrice.Name = "lblPrice";
this.lblPrice.Size = new System.Drawing.Size(41, 12);
this.lblPrice.TabIndex = 7;
this.lblPrice.Text = "售价";
//111======175
this.txtPrice.Location = new System.Drawing.Point(173,171);
this.txtPrice.Name ="txtPrice";
this.txtPrice.Size = new System.Drawing.Size(100, 21);
this.txtPrice.TabIndex = 7;
this.Controls.Add(this.lblPrice);
this.Controls.Add(this.txtPrice);

           //#####Cost###Decimal
this.lblCost.AutoSize = true;
this.lblCost.Location = new System.Drawing.Point(100,200);
this.lblCost.Name = "lblCost";
this.lblCost.Size = new System.Drawing.Size(41, 12);
this.lblCost.TabIndex = 8;
this.lblCost.Text = "成本";
//111======200
this.txtCost.Location = new System.Drawing.Point(173,196);
this.txtCost.Name ="txtCost";
this.txtCost.Size = new System.Drawing.Size(100, 21);
this.txtCost.TabIndex = 8;
this.Controls.Add(this.lblCost);
this.Controls.Add(this.txtCost);

           //#####500Summary###String
this.lblSummary.AutoSize = true;
this.lblSummary.Location = new System.Drawing.Point(100,225);
this.lblSummary.Name = "lblSummary";
this.lblSummary.Size = new System.Drawing.Size(41, 12);
this.lblSummary.TabIndex = 9;
this.lblSummary.Text = "摘要";
this.txtSummary.Location = new System.Drawing.Point(173,221);
this.txtSummary.Name = "txtSummary";
this.txtSummary.Size = new System.Drawing.Size(100, 21);
this.txtSummary.TabIndex = 9;
this.Controls.Add(this.lblSummary);
this.Controls.Add(this.txtSummary);

           //#####SubtotalCostAmount###Decimal
this.lblSubtotalCostAmount.AutoSize = true;
this.lblSubtotalCostAmount.Location = new System.Drawing.Point(100,250);
this.lblSubtotalCostAmount.Name = "lblSubtotalCostAmount";
this.lblSubtotalCostAmount.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalCostAmount.TabIndex = 10;
this.lblSubtotalCostAmount.Text = "成本小计";
//111======250
this.txtSubtotalCostAmount.Location = new System.Drawing.Point(173,246);
this.txtSubtotalCostAmount.Name ="txtSubtotalCostAmount";
this.txtSubtotalCostAmount.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalCostAmount.TabIndex = 10;
this.Controls.Add(this.lblSubtotalCostAmount);
this.Controls.Add(this.txtSubtotalCostAmount);

           //#####SubtotalPirceAmount###Decimal
this.lblSubtotalPirceAmount.AutoSize = true;
this.lblSubtotalPirceAmount.Location = new System.Drawing.Point(100,275);
this.lblSubtotalPirceAmount.Name = "lblSubtotalPirceAmount";
this.lblSubtotalPirceAmount.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalPirceAmount.TabIndex = 11;
this.lblSubtotalPirceAmount.Text = "金额小计";
//111======275
this.txtSubtotalPirceAmount.Location = new System.Drawing.Point(173,271);
this.txtSubtotalPirceAmount.Name ="txtSubtotalPirceAmount";
this.txtSubtotalPirceAmount.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalPirceAmount.TabIndex = 11;
this.Controls.Add(this.lblSubtotalPirceAmount);
this.Controls.Add(this.txtSubtotalPirceAmount);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblBorrowID );
this.Controls.Add(this.cmbBorrowID );

                this.Controls.Add(this.lblLocation_ID );
this.Controls.Add(this.cmbLocation_ID );

                this.Controls.Add(this.lblProdDetailID );
this.Controls.Add(this.cmbProdDetailID );

                this.Controls.Add(this.lblproperty );
this.Controls.Add(this.txtproperty );

                
                
                this.Controls.Add(this.lblPrice );
this.Controls.Add(this.txtPrice );

                this.Controls.Add(this.lblCost );
this.Controls.Add(this.txtCost );

                this.Controls.Add(this.lblSummary );
this.Controls.Add(this.txtSummary );

                this.Controls.Add(this.lblSubtotalCostAmount );
this.Controls.Add(this.txtSubtotalCostAmount );

                this.Controls.Add(this.lblSubtotalPirceAmount );
this.Controls.Add(this.txtSubtotalPirceAmount );

                    
            this.Name = "tb_ProdBorrowingDetailQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblBorrowID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbBorrowID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblLocation_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbLocation_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProdDetailID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbProdDetailID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblproperty;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtproperty;

    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPrice;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPrice;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCost;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSummary;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSummary;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSubtotalCostAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSubtotalCostAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSubtotalPirceAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSubtotalPirceAmount;

    
    
   
 





    }
}


