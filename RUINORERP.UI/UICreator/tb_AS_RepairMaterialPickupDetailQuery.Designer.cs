
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:09
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 维修领料单明细
    /// </summary>
    partial class tb_AS_RepairMaterialPickupDetailQuery
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
     
     this.lblLocation_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbLocation_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblProdDetailID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbProdDetailID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblRMRID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbRMRID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblproperty = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtproperty = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtproperty.Multiline = true;

this.lblShouldSendQty = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtShouldSendQty = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblActualSentQty = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtActualSentQty = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblSummary = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSummary = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtSummary.Multiline = true;

this.lblPrice = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPrice = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSubtotalPrice = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSubtotalPrice = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSubtotalCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSubtotalCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();



    //for end
    this.SuspendLayout();
    
         //for start
                 //#####Location_ID###Int64
//属性测试25Location_ID
//属性测试25Location_ID
//属性测试25Location_ID
this.lblLocation_ID.AutoSize = true;
this.lblLocation_ID.Location = new System.Drawing.Point(100,25);
this.lblLocation_ID.Name = "lblLocation_ID";
this.lblLocation_ID.Size = new System.Drawing.Size(41, 12);
this.lblLocation_ID.TabIndex = 1;
this.lblLocation_ID.Text = "库位";
//111======25
this.cmbLocation_ID.Location = new System.Drawing.Point(173,21);
this.cmbLocation_ID.Name ="cmbLocation_ID";
this.cmbLocation_ID.Size = new System.Drawing.Size(100, 21);
this.cmbLocation_ID.TabIndex = 1;
this.Controls.Add(this.lblLocation_ID);
this.Controls.Add(this.cmbLocation_ID);

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

           //#####RMRID###Int64
//属性测试75RMRID
this.lblRMRID.AutoSize = true;
this.lblRMRID.Location = new System.Drawing.Point(100,75);
this.lblRMRID.Name = "lblRMRID";
this.lblRMRID.Size = new System.Drawing.Size(41, 12);
this.lblRMRID.TabIndex = 3;
this.lblRMRID.Text = "";
//111======75
this.cmbRMRID.Location = new System.Drawing.Point(173,71);
this.cmbRMRID.Name ="cmbRMRID";
this.cmbRMRID.Size = new System.Drawing.Size(100, 21);
this.cmbRMRID.TabIndex = 3;
this.Controls.Add(this.lblRMRID);
this.Controls.Add(this.cmbRMRID);

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

           //#####ShouldSendQty###Decimal
this.lblShouldSendQty.AutoSize = true;
this.lblShouldSendQty.Location = new System.Drawing.Point(100,125);
this.lblShouldSendQty.Name = "lblShouldSendQty";
this.lblShouldSendQty.Size = new System.Drawing.Size(41, 12);
this.lblShouldSendQty.TabIndex = 5;
this.lblShouldSendQty.Text = "应发数";
//111======125
this.txtShouldSendQty.Location = new System.Drawing.Point(173,121);
this.txtShouldSendQty.Name ="txtShouldSendQty";
this.txtShouldSendQty.Size = new System.Drawing.Size(100, 21);
this.txtShouldSendQty.TabIndex = 5;
this.Controls.Add(this.lblShouldSendQty);
this.Controls.Add(this.txtShouldSendQty);

           //#####ActualSentQty###Decimal
this.lblActualSentQty.AutoSize = true;
this.lblActualSentQty.Location = new System.Drawing.Point(100,150);
this.lblActualSentQty.Name = "lblActualSentQty";
this.lblActualSentQty.Size = new System.Drawing.Size(41, 12);
this.lblActualSentQty.TabIndex = 6;
this.lblActualSentQty.Text = "实发数";
//111======150
this.txtActualSentQty.Location = new System.Drawing.Point(173,146);
this.txtActualSentQty.Name ="txtActualSentQty";
this.txtActualSentQty.Size = new System.Drawing.Size(100, 21);
this.txtActualSentQty.TabIndex = 6;
this.Controls.Add(this.lblActualSentQty);
this.Controls.Add(this.txtActualSentQty);

           //#####CanQuantity###Int32
//属性测试175CanQuantity
//属性测试175CanQuantity
//属性测试175CanQuantity

           //#####255Summary###String
this.lblSummary.AutoSize = true;
this.lblSummary.Location = new System.Drawing.Point(100,200);
this.lblSummary.Name = "lblSummary";
this.lblSummary.Size = new System.Drawing.Size(41, 12);
this.lblSummary.TabIndex = 8;
this.lblSummary.Text = "摘要";
this.txtSummary.Location = new System.Drawing.Point(173,196);
this.txtSummary.Name = "txtSummary";
this.txtSummary.Size = new System.Drawing.Size(100, 21);
this.txtSummary.TabIndex = 8;
this.Controls.Add(this.lblSummary);
this.Controls.Add(this.txtSummary);

           //#####Price###Decimal
this.lblPrice.AutoSize = true;
this.lblPrice.Location = new System.Drawing.Point(100,225);
this.lblPrice.Name = "lblPrice";
this.lblPrice.Size = new System.Drawing.Size(41, 12);
this.lblPrice.TabIndex = 9;
this.lblPrice.Text = "价格";
//111======225
this.txtPrice.Location = new System.Drawing.Point(173,221);
this.txtPrice.Name ="txtPrice";
this.txtPrice.Size = new System.Drawing.Size(100, 21);
this.txtPrice.TabIndex = 9;
this.Controls.Add(this.lblPrice);
this.Controls.Add(this.txtPrice);

           //#####Cost###Decimal
this.lblCost.AutoSize = true;
this.lblCost.Location = new System.Drawing.Point(100,250);
this.lblCost.Name = "lblCost";
this.lblCost.Size = new System.Drawing.Size(41, 12);
this.lblCost.TabIndex = 10;
this.lblCost.Text = "成本";
//111======250
this.txtCost.Location = new System.Drawing.Point(173,246);
this.txtCost.Name ="txtCost";
this.txtCost.Size = new System.Drawing.Size(100, 21);
this.txtCost.TabIndex = 10;
this.Controls.Add(this.lblCost);
this.Controls.Add(this.txtCost);

           //#####SubtotalPrice###Decimal
this.lblSubtotalPrice.AutoSize = true;
this.lblSubtotalPrice.Location = new System.Drawing.Point(100,275);
this.lblSubtotalPrice.Name = "lblSubtotalPrice";
this.lblSubtotalPrice.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalPrice.TabIndex = 11;
this.lblSubtotalPrice.Text = "金额小计";
//111======275
this.txtSubtotalPrice.Location = new System.Drawing.Point(173,271);
this.txtSubtotalPrice.Name ="txtSubtotalPrice";
this.txtSubtotalPrice.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalPrice.TabIndex = 11;
this.Controls.Add(this.lblSubtotalPrice);
this.Controls.Add(this.txtSubtotalPrice);

           //#####SubtotalCost###Decimal
this.lblSubtotalCost.AutoSize = true;
this.lblSubtotalCost.Location = new System.Drawing.Point(100,300);
this.lblSubtotalCost.Name = "lblSubtotalCost";
this.lblSubtotalCost.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalCost.TabIndex = 12;
this.lblSubtotalCost.Text = "成本小计";
//111======300
this.txtSubtotalCost.Location = new System.Drawing.Point(173,296);
this.txtSubtotalCost.Name ="txtSubtotalCost";
this.txtSubtotalCost.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalCost.TabIndex = 12;
this.Controls.Add(this.lblSubtotalCost);
this.Controls.Add(this.txtSubtotalCost);

           //#####ReturnQty###Int32
//属性测试325ReturnQty
//属性测试325ReturnQty
//属性测试325ReturnQty

           //#####ManufacturingOrderDetailRowID###Int64
//属性测试350ManufacturingOrderDetailRowID
//属性测试350ManufacturingOrderDetailRowID
//属性测试350ManufacturingOrderDetailRowID

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblLocation_ID );
this.Controls.Add(this.cmbLocation_ID );

                this.Controls.Add(this.lblProdDetailID );
this.Controls.Add(this.cmbProdDetailID );

                this.Controls.Add(this.lblRMRID );
this.Controls.Add(this.cmbRMRID );

                this.Controls.Add(this.lblproperty );
this.Controls.Add(this.txtproperty );

                this.Controls.Add(this.lblShouldSendQty );
this.Controls.Add(this.txtShouldSendQty );

                this.Controls.Add(this.lblActualSentQty );
this.Controls.Add(this.txtActualSentQty );

                
                this.Controls.Add(this.lblSummary );
this.Controls.Add(this.txtSummary );

                this.Controls.Add(this.lblPrice );
this.Controls.Add(this.txtPrice );

                this.Controls.Add(this.lblCost );
this.Controls.Add(this.txtCost );

                this.Controls.Add(this.lblSubtotalPrice );
this.Controls.Add(this.txtSubtotalPrice );

                this.Controls.Add(this.lblSubtotalCost );
this.Controls.Add(this.txtSubtotalCost );

                
                
                    
            this.Name = "tb_AS_RepairMaterialPickupDetailQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblLocation_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbLocation_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProdDetailID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbProdDetailID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRMRID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbRMRID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblproperty;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtproperty;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblShouldSendQty;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtShouldSendQty;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblActualSentQty;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtActualSentQty;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSummary;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSummary;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPrice;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPrice;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCost;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSubtotalPrice;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSubtotalPrice;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSubtotalCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSubtotalCost;

    
        
              
    
        
              
    
    
   
 





    }
}


