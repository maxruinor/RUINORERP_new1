// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:34
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 价格调整单明细
    /// </summary>
    partial class tb_FM_PriceAdjustmentDetailEdit
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
        
     //for definition
     // this.btnOk = new ComponentFactory.Krypton.Toolkit.KryptonButton();
     // this.btnCancel = new ComponentFactory.Krypton.Toolkit.KryptonButton();
      //this.kryptonPanel1 = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
      //for definition
     this.lblAdjustId = new Krypton.Toolkit.KryptonLabel();
this.cmbAdjustId = new Krypton.Toolkit.KryptonComboBox();

this.lblProdDetailID = new Krypton.Toolkit.KryptonLabel();
this.cmbProdDetailID = new Krypton.Toolkit.KryptonComboBox();

this.lblproperty = new Krypton.Toolkit.KryptonLabel();
this.txtproperty = new Krypton.Toolkit.KryptonTextBox();
this.txtproperty.Multiline = true;

this.lblSpecifications = new Krypton.Toolkit.KryptonLabel();
this.txtSpecifications = new Krypton.Toolkit.KryptonTextBox();
this.txtSpecifications.Multiline = true;

this.lblUnit_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbUnit_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblExchangeRate = new Krypton.Toolkit.KryptonLabel();
this.txtExchangeRate = new Krypton.Toolkit.KryptonTextBox();

this.lblOriginalUnitPrice = new Krypton.Toolkit.KryptonLabel();
this.txtOriginalUnitPrice = new Krypton.Toolkit.KryptonTextBox();

this.lblAdjustedUnitPrice = new Krypton.Toolkit.KryptonLabel();
this.txtAdjustedUnitPrice = new Krypton.Toolkit.KryptonTextBox();

this.lblDiffUnitPrice = new Krypton.Toolkit.KryptonLabel();
this.txtDiffUnitPrice = new Krypton.Toolkit.KryptonTextBox();

this.lblQuantity = new Krypton.Toolkit.KryptonLabel();
this.txtQuantity = new Krypton.Toolkit.KryptonTextBox();

this.lblCustomerPartNo = new Krypton.Toolkit.KryptonLabel();
this.txtCustomerPartNo = new Krypton.Toolkit.KryptonTextBox();

this.lblSubtotalDiffLocalAmount = new Krypton.Toolkit.KryptonLabel();
this.txtSubtotalDiffLocalAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblDescription = new Krypton.Toolkit.KryptonLabel();
this.txtDescription = new Krypton.Toolkit.KryptonTextBox();
this.txtDescription.Multiline = true;

this.lblTaxRate = new Krypton.Toolkit.KryptonLabel();
this.txtTaxRate = new Krypton.Toolkit.KryptonTextBox();

this.lblTaxDiffLocalAmount = new Krypton.Toolkit.KryptonLabel();
this.txtTaxDiffLocalAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblTaxSubtotalDiffLocalAmount = new Krypton.Toolkit.KryptonLabel();
this.txtTaxSubtotalDiffLocalAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblSummary = new Krypton.Toolkit.KryptonLabel();
this.txtSummary = new Krypton.Toolkit.KryptonTextBox();
this.txtSummary.Multiline = true;

    
    //for end
   // ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
   // this.kryptonPanel1.SuspendLayout();
    this.SuspendLayout();
    
            // 
            // btnOk
            // 
            //this.btnOk.Location = new System.Drawing.Point(126, 355);
            //this.btnOk.Name = "btnOk";
            //this.btnOk.Size = new System.Drawing.Size(90, 25);
            //this.btnOk.TabIndex = 0;
           // this.btnOk.Values.Text = "确定";
            //this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
           // this.btnCancel.Location = new System.Drawing.Point(244, 355);
            //this.btnCancel.Name = "btnCancel";
            //this.btnCancel.Size = new System.Drawing.Size(90, 25);
            //this.btnCancel.TabIndex = 1;
            //this.btnCancel.Values.Text = "取消";
           // this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            
         //for size
     
            //#####AdjustId###Int64
//属性测试25AdjustId
//属性测试25AdjustId
//属性测试25AdjustId
this.lblAdjustId.AutoSize = true;
this.lblAdjustId.Location = new System.Drawing.Point(100,25);
this.lblAdjustId.Name = "lblAdjustId";
this.lblAdjustId.Size = new System.Drawing.Size(41, 12);
this.lblAdjustId.TabIndex = 1;
this.lblAdjustId.Text = "价格调整单";
//111======25
this.cmbAdjustId.Location = new System.Drawing.Point(173,21);
this.cmbAdjustId.Name ="cmbAdjustId";
this.cmbAdjustId.Size = new System.Drawing.Size(100, 21);
this.cmbAdjustId.TabIndex = 1;
this.Controls.Add(this.lblAdjustId);
this.Controls.Add(this.cmbAdjustId);

           //#####ProdDetailID###Int64
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

           //#####1000Specifications###String
this.lblSpecifications.AutoSize = true;
this.lblSpecifications.Location = new System.Drawing.Point(100,100);
this.lblSpecifications.Name = "lblSpecifications";
this.lblSpecifications.Size = new System.Drawing.Size(41, 12);
this.lblSpecifications.TabIndex = 4;
this.lblSpecifications.Text = "规格";
this.txtSpecifications.Location = new System.Drawing.Point(173,96);
this.txtSpecifications.Name = "txtSpecifications";
this.txtSpecifications.Size = new System.Drawing.Size(100, 21);
this.txtSpecifications.TabIndex = 4;
this.Controls.Add(this.lblSpecifications);
this.Controls.Add(this.txtSpecifications);

           //#####Unit_ID###Int64
//属性测试125Unit_ID
//属性测试125Unit_ID
this.lblUnit_ID.AutoSize = true;
this.lblUnit_ID.Location = new System.Drawing.Point(100,125);
this.lblUnit_ID.Name = "lblUnit_ID";
this.lblUnit_ID.Size = new System.Drawing.Size(41, 12);
this.lblUnit_ID.TabIndex = 5;
this.lblUnit_ID.Text = "单位";
//111======125
this.cmbUnit_ID.Location = new System.Drawing.Point(173,121);
this.cmbUnit_ID.Name ="cmbUnit_ID";
this.cmbUnit_ID.Size = new System.Drawing.Size(100, 21);
this.cmbUnit_ID.TabIndex = 5;
this.Controls.Add(this.lblUnit_ID);
this.Controls.Add(this.cmbUnit_ID);

           //#####ExchangeRate###Decimal
this.lblExchangeRate.AutoSize = true;
this.lblExchangeRate.Location = new System.Drawing.Point(100,150);
this.lblExchangeRate.Name = "lblExchangeRate";
this.lblExchangeRate.Size = new System.Drawing.Size(41, 12);
this.lblExchangeRate.TabIndex = 6;
this.lblExchangeRate.Text = "汇率";
//111======150
this.txtExchangeRate.Location = new System.Drawing.Point(173,146);
this.txtExchangeRate.Name ="txtExchangeRate";
this.txtExchangeRate.Size = new System.Drawing.Size(100, 21);
this.txtExchangeRate.TabIndex = 6;
this.Controls.Add(this.lblExchangeRate);
this.Controls.Add(this.txtExchangeRate);

           //#####OriginalUnitPrice###Decimal
this.lblOriginalUnitPrice.AutoSize = true;
this.lblOriginalUnitPrice.Location = new System.Drawing.Point(100,175);
this.lblOriginalUnitPrice.Name = "lblOriginalUnitPrice";
this.lblOriginalUnitPrice.Size = new System.Drawing.Size(41, 12);
this.lblOriginalUnitPrice.TabIndex = 7;
this.lblOriginalUnitPrice.Text = "原始单价";
//111======175
this.txtOriginalUnitPrice.Location = new System.Drawing.Point(173,171);
this.txtOriginalUnitPrice.Name ="txtOriginalUnitPrice";
this.txtOriginalUnitPrice.Size = new System.Drawing.Size(100, 21);
this.txtOriginalUnitPrice.TabIndex = 7;
this.Controls.Add(this.lblOriginalUnitPrice);
this.Controls.Add(this.txtOriginalUnitPrice);

           //#####AdjustedUnitPrice###Decimal
this.lblAdjustedUnitPrice.AutoSize = true;
this.lblAdjustedUnitPrice.Location = new System.Drawing.Point(100,200);
this.lblAdjustedUnitPrice.Name = "lblAdjustedUnitPrice";
this.lblAdjustedUnitPrice.Size = new System.Drawing.Size(41, 12);
this.lblAdjustedUnitPrice.TabIndex = 8;
this.lblAdjustedUnitPrice.Text = "调整后单价";
//111======200
this.txtAdjustedUnitPrice.Location = new System.Drawing.Point(173,196);
this.txtAdjustedUnitPrice.Name ="txtAdjustedUnitPrice";
this.txtAdjustedUnitPrice.Size = new System.Drawing.Size(100, 21);
this.txtAdjustedUnitPrice.TabIndex = 8;
this.Controls.Add(this.lblAdjustedUnitPrice);
this.Controls.Add(this.txtAdjustedUnitPrice);

           //#####DiffUnitPrice###Decimal
this.lblDiffUnitPrice.AutoSize = true;
this.lblDiffUnitPrice.Location = new System.Drawing.Point(100,225);
this.lblDiffUnitPrice.Name = "lblDiffUnitPrice";
this.lblDiffUnitPrice.Size = new System.Drawing.Size(41, 12);
this.lblDiffUnitPrice.TabIndex = 9;
this.lblDiffUnitPrice.Text = "差异单价";
//111======225
this.txtDiffUnitPrice.Location = new System.Drawing.Point(173,221);
this.txtDiffUnitPrice.Name ="txtDiffUnitPrice";
this.txtDiffUnitPrice.Size = new System.Drawing.Size(100, 21);
this.txtDiffUnitPrice.TabIndex = 9;
this.Controls.Add(this.lblDiffUnitPrice);
this.Controls.Add(this.txtDiffUnitPrice);

           //#####Quantity###Decimal
this.lblQuantity.AutoSize = true;
this.lblQuantity.Location = new System.Drawing.Point(100,250);
this.lblQuantity.Name = "lblQuantity";
this.lblQuantity.Size = new System.Drawing.Size(41, 12);
this.lblQuantity.TabIndex = 10;
this.lblQuantity.Text = "数量";
//111======250
this.txtQuantity.Location = new System.Drawing.Point(173,246);
this.txtQuantity.Name ="txtQuantity";
this.txtQuantity.Size = new System.Drawing.Size(100, 21);
this.txtQuantity.TabIndex = 10;
this.Controls.Add(this.lblQuantity);
this.Controls.Add(this.txtQuantity);

           //#####100CustomerPartNo###String
this.lblCustomerPartNo.AutoSize = true;
this.lblCustomerPartNo.Location = new System.Drawing.Point(100,275);
this.lblCustomerPartNo.Name = "lblCustomerPartNo";
this.lblCustomerPartNo.Size = new System.Drawing.Size(41, 12);
this.lblCustomerPartNo.TabIndex = 11;
this.lblCustomerPartNo.Text = "往来单位料号";
this.txtCustomerPartNo.Location = new System.Drawing.Point(173,271);
this.txtCustomerPartNo.Name = "txtCustomerPartNo";
this.txtCustomerPartNo.Size = new System.Drawing.Size(100, 21);
this.txtCustomerPartNo.TabIndex = 11;
this.Controls.Add(this.lblCustomerPartNo);
this.Controls.Add(this.txtCustomerPartNo);

           //#####SubtotalDiffLocalAmount###Decimal
this.lblSubtotalDiffLocalAmount.AutoSize = true;
this.lblSubtotalDiffLocalAmount.Location = new System.Drawing.Point(100,300);
this.lblSubtotalDiffLocalAmount.Name = "lblSubtotalDiffLocalAmount";
this.lblSubtotalDiffLocalAmount.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalDiffLocalAmount.TabIndex = 12;
this.lblSubtotalDiffLocalAmount.Text = "差异金额小计";
//111======300
this.txtSubtotalDiffLocalAmount.Location = new System.Drawing.Point(173,296);
this.txtSubtotalDiffLocalAmount.Name ="txtSubtotalDiffLocalAmount";
this.txtSubtotalDiffLocalAmount.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalDiffLocalAmount.TabIndex = 12;
this.Controls.Add(this.lblSubtotalDiffLocalAmount);
this.Controls.Add(this.txtSubtotalDiffLocalAmount);

           //#####300Description###String
this.lblDescription.AutoSize = true;
this.lblDescription.Location = new System.Drawing.Point(100,325);
this.lblDescription.Name = "lblDescription";
this.lblDescription.Size = new System.Drawing.Size(41, 12);
this.lblDescription.TabIndex = 13;
this.lblDescription.Text = "描述";
this.txtDescription.Location = new System.Drawing.Point(173,321);
this.txtDescription.Name = "txtDescription";
this.txtDescription.Size = new System.Drawing.Size(100, 21);
this.txtDescription.TabIndex = 13;
this.Controls.Add(this.lblDescription);
this.Controls.Add(this.txtDescription);

           //#####TaxRate###Decimal
this.lblTaxRate.AutoSize = true;
this.lblTaxRate.Location = new System.Drawing.Point(100,350);
this.lblTaxRate.Name = "lblTaxRate";
this.lblTaxRate.Size = new System.Drawing.Size(41, 12);
this.lblTaxRate.TabIndex = 14;
this.lblTaxRate.Text = "税率";
//111======350
this.txtTaxRate.Location = new System.Drawing.Point(173,346);
this.txtTaxRate.Name ="txtTaxRate";
this.txtTaxRate.Size = new System.Drawing.Size(100, 21);
this.txtTaxRate.TabIndex = 14;
this.Controls.Add(this.lblTaxRate);
this.Controls.Add(this.txtTaxRate);

           //#####TaxDiffLocalAmount###Decimal
this.lblTaxDiffLocalAmount.AutoSize = true;
this.lblTaxDiffLocalAmount.Location = new System.Drawing.Point(100,375);
this.lblTaxDiffLocalAmount.Name = "lblTaxDiffLocalAmount";
this.lblTaxDiffLocalAmount.Size = new System.Drawing.Size(41, 12);
this.lblTaxDiffLocalAmount.TabIndex = 15;
this.lblTaxDiffLocalAmount.Text = "税额差异";
//111======375
this.txtTaxDiffLocalAmount.Location = new System.Drawing.Point(173,371);
this.txtTaxDiffLocalAmount.Name ="txtTaxDiffLocalAmount";
this.txtTaxDiffLocalAmount.Size = new System.Drawing.Size(100, 21);
this.txtTaxDiffLocalAmount.TabIndex = 15;
this.Controls.Add(this.lblTaxDiffLocalAmount);
this.Controls.Add(this.txtTaxDiffLocalAmount);

           //#####TaxSubtotalDiffLocalAmount###Decimal
this.lblTaxSubtotalDiffLocalAmount.AutoSize = true;
this.lblTaxSubtotalDiffLocalAmount.Location = new System.Drawing.Point(100,400);
this.lblTaxSubtotalDiffLocalAmount.Name = "lblTaxSubtotalDiffLocalAmount";
this.lblTaxSubtotalDiffLocalAmount.Size = new System.Drawing.Size(41, 12);
this.lblTaxSubtotalDiffLocalAmount.TabIndex = 16;
this.lblTaxSubtotalDiffLocalAmount.Text = "税额差异小计";
//111======400
this.txtTaxSubtotalDiffLocalAmount.Location = new System.Drawing.Point(173,396);
this.txtTaxSubtotalDiffLocalAmount.Name ="txtTaxSubtotalDiffLocalAmount";
this.txtTaxSubtotalDiffLocalAmount.Size = new System.Drawing.Size(100, 21);
this.txtTaxSubtotalDiffLocalAmount.TabIndex = 16;
this.Controls.Add(this.lblTaxSubtotalDiffLocalAmount);
this.Controls.Add(this.txtTaxSubtotalDiffLocalAmount);

           //#####300Summary###String
this.lblSummary.AutoSize = true;
this.lblSummary.Location = new System.Drawing.Point(100,425);
this.lblSummary.Name = "lblSummary";
this.lblSummary.Size = new System.Drawing.Size(41, 12);
this.lblSummary.TabIndex = 17;
this.lblSummary.Text = "摘要";
this.txtSummary.Location = new System.Drawing.Point(173,421);
this.txtSummary.Name = "txtSummary";
this.txtSummary.Size = new System.Drawing.Size(100, 21);
this.txtSummary.TabIndex = 17;
this.Controls.Add(this.lblSummary);
this.Controls.Add(this.txtSummary);

        //for 加入到容器
            //components = new System.ComponentModel.Container();
           
            //this.Controls.Add(this.btnCancel);
            //this.Controls.Add(this.btnOk);
            // 
            // kryptonPanel1
            // 
          //  this.kryptonPanel1.Controls.Add(this.btnCancel);
         //   this.kryptonPanel1.Controls.Add(this.btnOk);
           // this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            //this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
           // this.kryptonPanel1.Name = "kryptonPanel1";
           // this.kryptonPanel1.Size = new System.Drawing.Size(404, 300);
           // this.kryptonPanel1.TabIndex = 17;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblAdjustId );
this.Controls.Add(this.cmbAdjustId );

                this.Controls.Add(this.lblProdDetailID );
this.Controls.Add(this.cmbProdDetailID );

                this.Controls.Add(this.lblproperty );
this.Controls.Add(this.txtproperty );

                this.Controls.Add(this.lblSpecifications );
this.Controls.Add(this.txtSpecifications );

                this.Controls.Add(this.lblUnit_ID );
this.Controls.Add(this.cmbUnit_ID );

                this.Controls.Add(this.lblExchangeRate );
this.Controls.Add(this.txtExchangeRate );

                this.Controls.Add(this.lblOriginalUnitPrice );
this.Controls.Add(this.txtOriginalUnitPrice );

                this.Controls.Add(this.lblAdjustedUnitPrice );
this.Controls.Add(this.txtAdjustedUnitPrice );

                this.Controls.Add(this.lblDiffUnitPrice );
this.Controls.Add(this.txtDiffUnitPrice );

                this.Controls.Add(this.lblQuantity );
this.Controls.Add(this.txtQuantity );

                this.Controls.Add(this.lblCustomerPartNo );
this.Controls.Add(this.txtCustomerPartNo );

                this.Controls.Add(this.lblSubtotalDiffLocalAmount );
this.Controls.Add(this.txtSubtotalDiffLocalAmount );

                this.Controls.Add(this.lblDescription );
this.Controls.Add(this.txtDescription );

                this.Controls.Add(this.lblTaxRate );
this.Controls.Add(this.txtTaxRate );

                this.Controls.Add(this.lblTaxDiffLocalAmount );
this.Controls.Add(this.txtTaxDiffLocalAmount );

                this.Controls.Add(this.lblTaxSubtotalDiffLocalAmount );
this.Controls.Add(this.txtTaxSubtotalDiffLocalAmount );

                this.Controls.Add(this.lblSummary );
this.Controls.Add(this.txtSummary );

                            // 
            // "tb_FM_PriceAdjustmentDetailEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_FM_PriceAdjustmentDetailEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblAdjustId;
private Krypton.Toolkit.KryptonComboBox cmbAdjustId;

    
        
              private Krypton.Toolkit.KryptonLabel lblProdDetailID;
private Krypton.Toolkit.KryptonComboBox cmbProdDetailID;

    
        
              private Krypton.Toolkit.KryptonLabel lblproperty;
private Krypton.Toolkit.KryptonTextBox txtproperty;

    
        
              private Krypton.Toolkit.KryptonLabel lblSpecifications;
private Krypton.Toolkit.KryptonTextBox txtSpecifications;

    
        
              private Krypton.Toolkit.KryptonLabel lblUnit_ID;
private Krypton.Toolkit.KryptonComboBox cmbUnit_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblExchangeRate;
private Krypton.Toolkit.KryptonTextBox txtExchangeRate;

    
        
              private Krypton.Toolkit.KryptonLabel lblOriginalUnitPrice;
private Krypton.Toolkit.KryptonTextBox txtOriginalUnitPrice;

    
        
              private Krypton.Toolkit.KryptonLabel lblAdjustedUnitPrice;
private Krypton.Toolkit.KryptonTextBox txtAdjustedUnitPrice;

    
        
              private Krypton.Toolkit.KryptonLabel lblDiffUnitPrice;
private Krypton.Toolkit.KryptonTextBox txtDiffUnitPrice;

    
        
              private Krypton.Toolkit.KryptonLabel lblQuantity;
private Krypton.Toolkit.KryptonTextBox txtQuantity;

    
        
              private Krypton.Toolkit.KryptonLabel lblCustomerPartNo;
private Krypton.Toolkit.KryptonTextBox txtCustomerPartNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblSubtotalDiffLocalAmount;
private Krypton.Toolkit.KryptonTextBox txtSubtotalDiffLocalAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblDescription;
private Krypton.Toolkit.KryptonTextBox txtDescription;

    
        
              private Krypton.Toolkit.KryptonLabel lblTaxRate;
private Krypton.Toolkit.KryptonTextBox txtTaxRate;

    
        
              private Krypton.Toolkit.KryptonLabel lblTaxDiffLocalAmount;
private Krypton.Toolkit.KryptonTextBox txtTaxDiffLocalAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblTaxSubtotalDiffLocalAmount;
private Krypton.Toolkit.KryptonTextBox txtTaxSubtotalDiffLocalAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblSummary;
private Krypton.Toolkit.KryptonTextBox txtSummary;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

