﻿
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/24/2025 20:27:19
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 应收应付明细
    /// </summary>
    partial class tb_FM_ReceivablePayableDetailQuery
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
     
     this.lblARAPId = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbARAPId = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblProdDetailID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbProdDetailID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblproperty = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtproperty = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtproperty.Multiline = true;

this.lblSpecifications = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSpecifications = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtSpecifications.Multiline = true;

this.lblUnit_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbUnit_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblIncludeTax = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.chkIncludeTax = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
this.chkIncludeTax.Values.Text ="";

this.lblExchangeRate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtExchangeRate = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblUnitPrice = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtUnitPrice = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblQuantity = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtQuantity = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblCustomerPartNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtCustomerPartNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblDescription = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtDescription = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtDescription.Multiline = true;

this.lblTaxRate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTaxRate = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTaxLocalAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTaxLocalAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblLocalPayableAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtLocalPayableAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSummary = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSummary = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtSummary.Multiline = true;

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####ARAPId###Int64
//属性测试25ARAPId
//属性测试25ARAPId
//属性测试25ARAPId
this.lblARAPId.AutoSize = true;
this.lblARAPId.Location = new System.Drawing.Point(100,25);
this.lblARAPId.Name = "lblARAPId";
this.lblARAPId.Size = new System.Drawing.Size(41, 12);
this.lblARAPId.TabIndex = 1;
this.lblARAPId.Text = "应收付款单";
//111======25
this.cmbARAPId.Location = new System.Drawing.Point(173,21);
this.cmbARAPId.Name ="cmbARAPId";
this.cmbARAPId.Size = new System.Drawing.Size(100, 21);
this.cmbARAPId.TabIndex = 1;
this.Controls.Add(this.lblARAPId);
this.Controls.Add(this.cmbARAPId);

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

           //#####IncludeTax###Boolean
this.lblIncludeTax.AutoSize = true;
this.lblIncludeTax.Location = new System.Drawing.Point(100,150);
this.lblIncludeTax.Name = "lblIncludeTax";
this.lblIncludeTax.Size = new System.Drawing.Size(41, 12);
this.lblIncludeTax.TabIndex = 6;
this.lblIncludeTax.Text = "含税";
this.chkIncludeTax.Location = new System.Drawing.Point(173,146);
this.chkIncludeTax.Name = "chkIncludeTax";
this.chkIncludeTax.Size = new System.Drawing.Size(100, 21);
this.chkIncludeTax.TabIndex = 6;
this.Controls.Add(this.lblIncludeTax);
this.Controls.Add(this.chkIncludeTax);

           //#####ExchangeRate###Decimal
this.lblExchangeRate.AutoSize = true;
this.lblExchangeRate.Location = new System.Drawing.Point(100,175);
this.lblExchangeRate.Name = "lblExchangeRate";
this.lblExchangeRate.Size = new System.Drawing.Size(41, 12);
this.lblExchangeRate.TabIndex = 7;
this.lblExchangeRate.Text = "汇率";
//111======175
this.txtExchangeRate.Location = new System.Drawing.Point(173,171);
this.txtExchangeRate.Name ="txtExchangeRate";
this.txtExchangeRate.Size = new System.Drawing.Size(100, 21);
this.txtExchangeRate.TabIndex = 7;
this.Controls.Add(this.lblExchangeRate);
this.Controls.Add(this.txtExchangeRate);

           //#####UnitPrice###Decimal
this.lblUnitPrice.AutoSize = true;
this.lblUnitPrice.Location = new System.Drawing.Point(100,200);
this.lblUnitPrice.Name = "lblUnitPrice";
this.lblUnitPrice.Size = new System.Drawing.Size(41, 12);
this.lblUnitPrice.TabIndex = 8;
this.lblUnitPrice.Text = "单价";
//111======200
this.txtUnitPrice.Location = new System.Drawing.Point(173,196);
this.txtUnitPrice.Name ="txtUnitPrice";
this.txtUnitPrice.Size = new System.Drawing.Size(100, 21);
this.txtUnitPrice.TabIndex = 8;
this.Controls.Add(this.lblUnitPrice);
this.Controls.Add(this.txtUnitPrice);

           //#####Quantity###Decimal
this.lblQuantity.AutoSize = true;
this.lblQuantity.Location = new System.Drawing.Point(100,225);
this.lblQuantity.Name = "lblQuantity";
this.lblQuantity.Size = new System.Drawing.Size(41, 12);
this.lblQuantity.TabIndex = 9;
this.lblQuantity.Text = "数量";
//111======225
this.txtQuantity.Location = new System.Drawing.Point(173,221);
this.txtQuantity.Name ="txtQuantity";
this.txtQuantity.Size = new System.Drawing.Size(100, 21);
this.txtQuantity.TabIndex = 9;
this.Controls.Add(this.lblQuantity);
this.Controls.Add(this.txtQuantity);

           //#####100CustomerPartNo###String
this.lblCustomerPartNo.AutoSize = true;
this.lblCustomerPartNo.Location = new System.Drawing.Point(100,250);
this.lblCustomerPartNo.Name = "lblCustomerPartNo";
this.lblCustomerPartNo.Size = new System.Drawing.Size(41, 12);
this.lblCustomerPartNo.TabIndex = 10;
this.lblCustomerPartNo.Text = "往来单位料号";
this.txtCustomerPartNo.Location = new System.Drawing.Point(173,246);
this.txtCustomerPartNo.Name = "txtCustomerPartNo";
this.txtCustomerPartNo.Size = new System.Drawing.Size(100, 21);
this.txtCustomerPartNo.TabIndex = 10;
this.Controls.Add(this.lblCustomerPartNo);
this.Controls.Add(this.txtCustomerPartNo);

           //#####300Description###String
this.lblDescription.AutoSize = true;
this.lblDescription.Location = new System.Drawing.Point(100,275);
this.lblDescription.Name = "lblDescription";
this.lblDescription.Size = new System.Drawing.Size(41, 12);
this.lblDescription.TabIndex = 11;
this.lblDescription.Text = "描述";
this.txtDescription.Location = new System.Drawing.Point(173,271);
this.txtDescription.Name = "txtDescription";
this.txtDescription.Size = new System.Drawing.Size(100, 21);
this.txtDescription.TabIndex = 11;
this.Controls.Add(this.lblDescription);
this.Controls.Add(this.txtDescription);

           //#####TaxRate###Decimal
this.lblTaxRate.AutoSize = true;
this.lblTaxRate.Location = new System.Drawing.Point(100,300);
this.lblTaxRate.Name = "lblTaxRate";
this.lblTaxRate.Size = new System.Drawing.Size(41, 12);
this.lblTaxRate.TabIndex = 12;
this.lblTaxRate.Text = "税率";
//111======300
this.txtTaxRate.Location = new System.Drawing.Point(173,296);
this.txtTaxRate.Name ="txtTaxRate";
this.txtTaxRate.Size = new System.Drawing.Size(100, 21);
this.txtTaxRate.TabIndex = 12;
this.Controls.Add(this.lblTaxRate);
this.Controls.Add(this.txtTaxRate);

           //#####TaxLocalAmount###Decimal
this.lblTaxLocalAmount.AutoSize = true;
this.lblTaxLocalAmount.Location = new System.Drawing.Point(100,325);
this.lblTaxLocalAmount.Name = "lblTaxLocalAmount";
this.lblTaxLocalAmount.Size = new System.Drawing.Size(41, 12);
this.lblTaxLocalAmount.TabIndex = 13;
this.lblTaxLocalAmount.Text = "税额";
//111======325
this.txtTaxLocalAmount.Location = new System.Drawing.Point(173,321);
this.txtTaxLocalAmount.Name ="txtTaxLocalAmount";
this.txtTaxLocalAmount.Size = new System.Drawing.Size(100, 21);
this.txtTaxLocalAmount.TabIndex = 13;
this.Controls.Add(this.lblTaxLocalAmount);
this.Controls.Add(this.txtTaxLocalAmount);

           //#####LocalPayableAmount###Decimal
this.lblLocalPayableAmount.AutoSize = true;
this.lblLocalPayableAmount.Location = new System.Drawing.Point(100,350);
this.lblLocalPayableAmount.Name = "lblLocalPayableAmount";
this.lblLocalPayableAmount.Size = new System.Drawing.Size(41, 12);
this.lblLocalPayableAmount.TabIndex = 14;
this.lblLocalPayableAmount.Text = "金额小计";
//111======350
this.txtLocalPayableAmount.Location = new System.Drawing.Point(173,346);
this.txtLocalPayableAmount.Name ="txtLocalPayableAmount";
this.txtLocalPayableAmount.Size = new System.Drawing.Size(100, 21);
this.txtLocalPayableAmount.TabIndex = 14;
this.Controls.Add(this.lblLocalPayableAmount);
this.Controls.Add(this.txtLocalPayableAmount);

           //#####300Summary###String
this.lblSummary.AutoSize = true;
this.lblSummary.Location = new System.Drawing.Point(100,375);
this.lblSummary.Name = "lblSummary";
this.lblSummary.Size = new System.Drawing.Size(41, 12);
this.lblSummary.TabIndex = 15;
this.lblSummary.Text = "摘要";
this.txtSummary.Location = new System.Drawing.Point(173,371);
this.txtSummary.Name = "txtSummary";
this.txtSummary.Size = new System.Drawing.Size(100, 21);
this.txtSummary.TabIndex = 15;
this.Controls.Add(this.lblSummary);
this.Controls.Add(this.txtSummary);

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblARAPId );
this.Controls.Add(this.cmbARAPId );

                this.Controls.Add(this.lblProdDetailID );
this.Controls.Add(this.cmbProdDetailID );

                this.Controls.Add(this.lblproperty );
this.Controls.Add(this.txtproperty );

                this.Controls.Add(this.lblSpecifications );
this.Controls.Add(this.txtSpecifications );

                this.Controls.Add(this.lblUnit_ID );
this.Controls.Add(this.cmbUnit_ID );

                this.Controls.Add(this.lblIncludeTax );
this.Controls.Add(this.chkIncludeTax );

                this.Controls.Add(this.lblExchangeRate );
this.Controls.Add(this.txtExchangeRate );

                this.Controls.Add(this.lblUnitPrice );
this.Controls.Add(this.txtUnitPrice );

                this.Controls.Add(this.lblQuantity );
this.Controls.Add(this.txtQuantity );

                this.Controls.Add(this.lblCustomerPartNo );
this.Controls.Add(this.txtCustomerPartNo );

                this.Controls.Add(this.lblDescription );
this.Controls.Add(this.txtDescription );

                this.Controls.Add(this.lblTaxRate );
this.Controls.Add(this.txtTaxRate );

                this.Controls.Add(this.lblTaxLocalAmount );
this.Controls.Add(this.txtTaxLocalAmount );

                this.Controls.Add(this.lblLocalPayableAmount );
this.Controls.Add(this.txtLocalPayableAmount );

                this.Controls.Add(this.lblSummary );
this.Controls.Add(this.txtSummary );

                    
            this.Name = "tb_FM_ReceivablePayableDetailQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblARAPId;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbARAPId;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblProdDetailID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbProdDetailID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblproperty;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtproperty;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSpecifications;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSpecifications;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblUnit_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbUnit_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIncludeTax;
private ComponentFactory.Krypton.Toolkit.KryptonCheckBox chkIncludeTax;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblExchangeRate;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtExchangeRate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblUnitPrice;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtUnitPrice;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblQuantity;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtQuantity;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCustomerPartNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtCustomerPartNo;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDescription;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtDescription;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTaxRate;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTaxRate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTaxLocalAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTaxLocalAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblLocalPayableAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtLocalPayableAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSummary;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSummary;

    
    
   
 





    }
}


