
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:41
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 领料单明细
    /// </summary>
    partial class tb_MaterialRequisitionDetailQuery
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
     
     this.lblMR_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbMR_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblLocation_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbLocation_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblProdDetailID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbProdDetailID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblproperty = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtproperty = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtproperty.Multiline = true;




this.lblSummary = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSummary = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtSummary.Multiline = true;

this.lblCustomerPartNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCustomerPartNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblPrice = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtPrice = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSubtotalPrice = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSubtotalPrice = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSubtotalCost = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSubtotalCost = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();



    //for end
    this.SuspendLayout();
    
         //for start
                 //#####MR_ID###Int64
//属性测试25MR_ID
//属性测试25MR_ID
//属性测试25MR_ID
this.lblMR_ID.AutoSize = true;
this.lblMR_ID.Location = new System.Drawing.Point(100,25);
this.lblMR_ID.Name = "lblMR_ID";
this.lblMR_ID.Size = new System.Drawing.Size(41, 12);
this.lblMR_ID.TabIndex = 1;
this.lblMR_ID.Text = "领料单";
//111======25
this.cmbMR_ID.Location = new System.Drawing.Point(173,21);
this.cmbMR_ID.Name ="cmbMR_ID";
this.cmbMR_ID.Size = new System.Drawing.Size(100, 21);
this.cmbMR_ID.TabIndex = 1;
this.Controls.Add(this.lblMR_ID);
this.Controls.Add(this.cmbMR_ID);

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
this.lblProdDetailID.Text = "货品详情";
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

           //#####ShouldSendQty###Int32
//属性测试125ShouldSendQty
//属性测试125ShouldSendQty
//属性测试125ShouldSendQty

           //#####ActualSentQty###Int32
//属性测试150ActualSentQty
//属性测试150ActualSentQty
//属性测试150ActualSentQty

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

           //#####50CustomerPartNo###String
this.lblCustomerPartNo.AutoSize = true;
this.lblCustomerPartNo.Location = new System.Drawing.Point(100,225);
this.lblCustomerPartNo.Name = "lblCustomerPartNo";
this.lblCustomerPartNo.Size = new System.Drawing.Size(41, 12);
this.lblCustomerPartNo.TabIndex = 9;
this.lblCustomerPartNo.Text = "客户型号";
this.txtCustomerPartNo.Location = new System.Drawing.Point(173,221);
this.txtCustomerPartNo.Name = "txtCustomerPartNo";
this.txtCustomerPartNo.Size = new System.Drawing.Size(100, 21);
this.txtCustomerPartNo.TabIndex = 9;
this.Controls.Add(this.lblCustomerPartNo);
this.Controls.Add(this.txtCustomerPartNo);

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

           //#####Price###Decimal
this.lblPrice.AutoSize = true;
this.lblPrice.Location = new System.Drawing.Point(100,275);
this.lblPrice.Name = "lblPrice";
this.lblPrice.Size = new System.Drawing.Size(41, 12);
this.lblPrice.TabIndex = 11;
this.lblPrice.Text = "价格";
//111======275
this.txtPrice.Location = new System.Drawing.Point(173,271);
this.txtPrice.Name ="txtPrice";
this.txtPrice.Size = new System.Drawing.Size(100, 21);
this.txtPrice.TabIndex = 11;
this.Controls.Add(this.lblPrice);
this.Controls.Add(this.txtPrice);

           //#####SubtotalPrice###Decimal
this.lblSubtotalPrice.AutoSize = true;
this.lblSubtotalPrice.Location = new System.Drawing.Point(100,300);
this.lblSubtotalPrice.Name = "lblSubtotalPrice";
this.lblSubtotalPrice.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalPrice.TabIndex = 12;
this.lblSubtotalPrice.Text = "金额小计";
//111======300
this.txtSubtotalPrice.Location = new System.Drawing.Point(173,296);
this.txtSubtotalPrice.Name ="txtSubtotalPrice";
this.txtSubtotalPrice.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalPrice.TabIndex = 12;
this.Controls.Add(this.lblSubtotalPrice);
this.Controls.Add(this.txtSubtotalPrice);

           //#####SubtotalCost###Decimal
this.lblSubtotalCost.AutoSize = true;
this.lblSubtotalCost.Location = new System.Drawing.Point(100,325);
this.lblSubtotalCost.Name = "lblSubtotalCost";
this.lblSubtotalCost.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalCost.TabIndex = 13;
this.lblSubtotalCost.Text = "成本小计";
//111======325
this.txtSubtotalCost.Location = new System.Drawing.Point(173,321);
this.txtSubtotalCost.Name ="txtSubtotalCost";
this.txtSubtotalCost.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalCost.TabIndex = 13;
this.Controls.Add(this.lblSubtotalCost);
this.Controls.Add(this.txtSubtotalCost);

           //#####ReturnQty###Int32
//属性测试350ReturnQty
//属性测试350ReturnQty
//属性测试350ReturnQty

           //#####ManufacturingOrderDetailRowID###Int64
//属性测试375ManufacturingOrderDetailRowID
//属性测试375ManufacturingOrderDetailRowID
//属性测试375ManufacturingOrderDetailRowID

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblMR_ID );
this.Controls.Add(this.cmbMR_ID );

                this.Controls.Add(this.lblLocation_ID );
this.Controls.Add(this.cmbLocation_ID );

                this.Controls.Add(this.lblProdDetailID );
this.Controls.Add(this.cmbProdDetailID );

                this.Controls.Add(this.lblproperty );
this.Controls.Add(this.txtproperty );

                
                
                
                this.Controls.Add(this.lblSummary );
this.Controls.Add(this.txtSummary );

                this.Controls.Add(this.lblCustomerPartNo );
this.Controls.Add(this.txtCustomerPartNo );

                this.Controls.Add(this.lblCost );
this.Controls.Add(this.txtCost );

                this.Controls.Add(this.lblPrice );
this.Controls.Add(this.txtPrice );

                this.Controls.Add(this.lblSubtotalPrice );
this.Controls.Add(this.txtSubtotalPrice );

                this.Controls.Add(this.lblSubtotalCost );
this.Controls.Add(this.txtSubtotalCost );

                
                
                    
            this.Name = "tb_MaterialRequisitionDetailQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblMR_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbMR_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblLocation_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbLocation_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProdDetailID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbProdDetailID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblproperty;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtproperty;

    
        
              
    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSummary;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSummary;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCustomerPartNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCustomerPartNo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCost;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPrice;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtPrice;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSubtotalPrice;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSubtotalPrice;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSubtotalCost;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSubtotalCost;

    
        
              
    
        
              
    
    
   
 





    }
}


