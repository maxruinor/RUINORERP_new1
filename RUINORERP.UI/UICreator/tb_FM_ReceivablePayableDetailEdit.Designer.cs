// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/30/2025 15:18:12
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
    partial class tb_FM_ReceivablePayableDetailEdit
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
     this.lblARAPId = new Krypton.Toolkit.KryptonLabel();
this.cmbARAPId = new Krypton.Toolkit.KryptonComboBox();

this.lblBizType = new Krypton.Toolkit.KryptonLabel();
this.txtBizType = new Krypton.Toolkit.KryptonTextBox();

this.lblSourceBill_ID = new Krypton.Toolkit.KryptonLabel();
this.txtSourceBill_ID = new Krypton.Toolkit.KryptonTextBox();

this.lblSourceBillNO = new Krypton.Toolkit.KryptonLabel();
this.txtSourceBillNO = new Krypton.Toolkit.KryptonTextBox();

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

this.lblIncludeTax = new Krypton.Toolkit.KryptonLabel();
this.chkIncludeTax = new Krypton.Toolkit.KryptonCheckBox();
this.chkIncludeTax.Values.Text ="";

this.lblExchangeRate = new Krypton.Toolkit.KryptonLabel();
this.txtExchangeRate = new Krypton.Toolkit.KryptonTextBox();

this.lblUnitPrice = new Krypton.Toolkit.KryptonLabel();
this.txtUnitPrice = new Krypton.Toolkit.KryptonTextBox();

this.lblQuantity = new Krypton.Toolkit.KryptonLabel();
this.txtQuantity = new Krypton.Toolkit.KryptonTextBox();

this.lblCustomerPartNo = new Krypton.Toolkit.KryptonLabel();
this.txtCustomerPartNo = new Krypton.Toolkit.KryptonTextBox();

this.lblDescription = new Krypton.Toolkit.KryptonLabel();
this.txtDescription = new Krypton.Toolkit.KryptonTextBox();
this.txtDescription.Multiline = true;

this.lblTaxRate = new Krypton.Toolkit.KryptonLabel();
this.txtTaxRate = new Krypton.Toolkit.KryptonTextBox();

this.lblTaxLocalAmount = new Krypton.Toolkit.KryptonLabel();
this.txtTaxLocalAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblLocalPayableAmount = new Krypton.Toolkit.KryptonLabel();
this.txtLocalPayableAmount = new Krypton.Toolkit.KryptonTextBox();

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

           //#####BizType###Int32
//属性测试50BizType
//属性测试50BizType
//属性测试50BizType
this.lblBizType.AutoSize = true;
this.lblBizType.Location = new System.Drawing.Point(100,50);
this.lblBizType.Name = "lblBizType";
this.lblBizType.Size = new System.Drawing.Size(41, 12);
this.lblBizType.TabIndex = 2;
this.lblBizType.Text = "来源业务";
this.txtBizType.Location = new System.Drawing.Point(173,46);
this.txtBizType.Name = "txtBizType";
this.txtBizType.Size = new System.Drawing.Size(100, 21);
this.txtBizType.TabIndex = 2;
this.Controls.Add(this.lblBizType);
this.Controls.Add(this.txtBizType);

           //#####SourceBill_ID###Int64
//属性测试75SourceBill_ID
//属性测试75SourceBill_ID
//属性测试75SourceBill_ID
this.lblSourceBill_ID.AutoSize = true;
this.lblSourceBill_ID.Location = new System.Drawing.Point(100,75);
this.lblSourceBill_ID.Name = "lblSourceBill_ID";
this.lblSourceBill_ID.Size = new System.Drawing.Size(41, 12);
this.lblSourceBill_ID.TabIndex = 3;
this.lblSourceBill_ID.Text = "来源单据";
this.txtSourceBill_ID.Location = new System.Drawing.Point(173,71);
this.txtSourceBill_ID.Name = "txtSourceBill_ID";
this.txtSourceBill_ID.Size = new System.Drawing.Size(100, 21);
this.txtSourceBill_ID.TabIndex = 3;
this.Controls.Add(this.lblSourceBill_ID);
this.Controls.Add(this.txtSourceBill_ID);

           //#####30SourceBillNO###String
this.lblSourceBillNO.AutoSize = true;
this.lblSourceBillNO.Location = new System.Drawing.Point(100,100);
this.lblSourceBillNO.Name = "lblSourceBillNO";
this.lblSourceBillNO.Size = new System.Drawing.Size(41, 12);
this.lblSourceBillNO.TabIndex = 4;
this.lblSourceBillNO.Text = "来源单号";
this.txtSourceBillNO.Location = new System.Drawing.Point(173,96);
this.txtSourceBillNO.Name = "txtSourceBillNO";
this.txtSourceBillNO.Size = new System.Drawing.Size(100, 21);
this.txtSourceBillNO.TabIndex = 4;
this.Controls.Add(this.lblSourceBillNO);
this.Controls.Add(this.txtSourceBillNO);

           //#####ProdDetailID###Int64
//属性测试125ProdDetailID
this.lblProdDetailID.AutoSize = true;
this.lblProdDetailID.Location = new System.Drawing.Point(100,125);
this.lblProdDetailID.Name = "lblProdDetailID";
this.lblProdDetailID.Size = new System.Drawing.Size(41, 12);
this.lblProdDetailID.TabIndex = 5;
this.lblProdDetailID.Text = "产品";
//111======125
this.cmbProdDetailID.Location = new System.Drawing.Point(173,121);
this.cmbProdDetailID.Name ="cmbProdDetailID";
this.cmbProdDetailID.Size = new System.Drawing.Size(100, 21);
this.cmbProdDetailID.TabIndex = 5;
this.Controls.Add(this.lblProdDetailID);
this.Controls.Add(this.cmbProdDetailID);

           //#####255property###String
this.lblproperty.AutoSize = true;
this.lblproperty.Location = new System.Drawing.Point(100,150);
this.lblproperty.Name = "lblproperty";
this.lblproperty.Size = new System.Drawing.Size(41, 12);
this.lblproperty.TabIndex = 6;
this.lblproperty.Text = "属性";
this.txtproperty.Location = new System.Drawing.Point(173,146);
this.txtproperty.Name = "txtproperty";
this.txtproperty.Size = new System.Drawing.Size(100, 21);
this.txtproperty.TabIndex = 6;
this.Controls.Add(this.lblproperty);
this.Controls.Add(this.txtproperty);

           //#####1000Specifications###String
this.lblSpecifications.AutoSize = true;
this.lblSpecifications.Location = new System.Drawing.Point(100,175);
this.lblSpecifications.Name = "lblSpecifications";
this.lblSpecifications.Size = new System.Drawing.Size(41, 12);
this.lblSpecifications.TabIndex = 7;
this.lblSpecifications.Text = "规格";
this.txtSpecifications.Location = new System.Drawing.Point(173,171);
this.txtSpecifications.Name = "txtSpecifications";
this.txtSpecifications.Size = new System.Drawing.Size(100, 21);
this.txtSpecifications.TabIndex = 7;
this.Controls.Add(this.lblSpecifications);
this.Controls.Add(this.txtSpecifications);

           //#####Unit_ID###Int64
//属性测试200Unit_ID
//属性测试200Unit_ID
this.lblUnit_ID.AutoSize = true;
this.lblUnit_ID.Location = new System.Drawing.Point(100,200);
this.lblUnit_ID.Name = "lblUnit_ID";
this.lblUnit_ID.Size = new System.Drawing.Size(41, 12);
this.lblUnit_ID.TabIndex = 8;
this.lblUnit_ID.Text = "单位";
//111======200
this.cmbUnit_ID.Location = new System.Drawing.Point(173,196);
this.cmbUnit_ID.Name ="cmbUnit_ID";
this.cmbUnit_ID.Size = new System.Drawing.Size(100, 21);
this.cmbUnit_ID.TabIndex = 8;
this.Controls.Add(this.lblUnit_ID);
this.Controls.Add(this.cmbUnit_ID);

           //#####IncludeTax###Boolean
this.lblIncludeTax.AutoSize = true;
this.lblIncludeTax.Location = new System.Drawing.Point(100,225);
this.lblIncludeTax.Name = "lblIncludeTax";
this.lblIncludeTax.Size = new System.Drawing.Size(41, 12);
this.lblIncludeTax.TabIndex = 9;
this.lblIncludeTax.Text = "含税";
this.chkIncludeTax.Location = new System.Drawing.Point(173,221);
this.chkIncludeTax.Name = "chkIncludeTax";
this.chkIncludeTax.Size = new System.Drawing.Size(100, 21);
this.chkIncludeTax.TabIndex = 9;
this.Controls.Add(this.lblIncludeTax);
this.Controls.Add(this.chkIncludeTax);

           //#####ExchangeRate###Decimal
this.lblExchangeRate.AutoSize = true;
this.lblExchangeRate.Location = new System.Drawing.Point(100,250);
this.lblExchangeRate.Name = "lblExchangeRate";
this.lblExchangeRate.Size = new System.Drawing.Size(41, 12);
this.lblExchangeRate.TabIndex = 10;
this.lblExchangeRate.Text = "汇率";
//111======250
this.txtExchangeRate.Location = new System.Drawing.Point(173,246);
this.txtExchangeRate.Name ="txtExchangeRate";
this.txtExchangeRate.Size = new System.Drawing.Size(100, 21);
this.txtExchangeRate.TabIndex = 10;
this.Controls.Add(this.lblExchangeRate);
this.Controls.Add(this.txtExchangeRate);

           //#####UnitPrice###Decimal
this.lblUnitPrice.AutoSize = true;
this.lblUnitPrice.Location = new System.Drawing.Point(100,275);
this.lblUnitPrice.Name = "lblUnitPrice";
this.lblUnitPrice.Size = new System.Drawing.Size(41, 12);
this.lblUnitPrice.TabIndex = 11;
this.lblUnitPrice.Text = "单价";
//111======275
this.txtUnitPrice.Location = new System.Drawing.Point(173,271);
this.txtUnitPrice.Name ="txtUnitPrice";
this.txtUnitPrice.Size = new System.Drawing.Size(100, 21);
this.txtUnitPrice.TabIndex = 11;
this.Controls.Add(this.lblUnitPrice);
this.Controls.Add(this.txtUnitPrice);

           //#####Quantity###Decimal
this.lblQuantity.AutoSize = true;
this.lblQuantity.Location = new System.Drawing.Point(100,300);
this.lblQuantity.Name = "lblQuantity";
this.lblQuantity.Size = new System.Drawing.Size(41, 12);
this.lblQuantity.TabIndex = 12;
this.lblQuantity.Text = "数量";
//111======300
this.txtQuantity.Location = new System.Drawing.Point(173,296);
this.txtQuantity.Name ="txtQuantity";
this.txtQuantity.Size = new System.Drawing.Size(100, 21);
this.txtQuantity.TabIndex = 12;
this.Controls.Add(this.lblQuantity);
this.Controls.Add(this.txtQuantity);

           //#####100CustomerPartNo###String
this.lblCustomerPartNo.AutoSize = true;
this.lblCustomerPartNo.Location = new System.Drawing.Point(100,325);
this.lblCustomerPartNo.Name = "lblCustomerPartNo";
this.lblCustomerPartNo.Size = new System.Drawing.Size(41, 12);
this.lblCustomerPartNo.TabIndex = 13;
this.lblCustomerPartNo.Text = "往来单位料号";
this.txtCustomerPartNo.Location = new System.Drawing.Point(173,321);
this.txtCustomerPartNo.Name = "txtCustomerPartNo";
this.txtCustomerPartNo.Size = new System.Drawing.Size(100, 21);
this.txtCustomerPartNo.TabIndex = 13;
this.Controls.Add(this.lblCustomerPartNo);
this.Controls.Add(this.txtCustomerPartNo);

           //#####300Description###String
this.lblDescription.AutoSize = true;
this.lblDescription.Location = new System.Drawing.Point(100,350);
this.lblDescription.Name = "lblDescription";
this.lblDescription.Size = new System.Drawing.Size(41, 12);
this.lblDescription.TabIndex = 14;
this.lblDescription.Text = "描述";
this.txtDescription.Location = new System.Drawing.Point(173,346);
this.txtDescription.Name = "txtDescription";
this.txtDescription.Size = new System.Drawing.Size(100, 21);
this.txtDescription.TabIndex = 14;
this.Controls.Add(this.lblDescription);
this.Controls.Add(this.txtDescription);

           //#####TaxRate###Decimal
this.lblTaxRate.AutoSize = true;
this.lblTaxRate.Location = new System.Drawing.Point(100,375);
this.lblTaxRate.Name = "lblTaxRate";
this.lblTaxRate.Size = new System.Drawing.Size(41, 12);
this.lblTaxRate.TabIndex = 15;
this.lblTaxRate.Text = "税率";
//111======375
this.txtTaxRate.Location = new System.Drawing.Point(173,371);
this.txtTaxRate.Name ="txtTaxRate";
this.txtTaxRate.Size = new System.Drawing.Size(100, 21);
this.txtTaxRate.TabIndex = 15;
this.Controls.Add(this.lblTaxRate);
this.Controls.Add(this.txtTaxRate);

           //#####TaxLocalAmount###Decimal
this.lblTaxLocalAmount.AutoSize = true;
this.lblTaxLocalAmount.Location = new System.Drawing.Point(100,400);
this.lblTaxLocalAmount.Name = "lblTaxLocalAmount";
this.lblTaxLocalAmount.Size = new System.Drawing.Size(41, 12);
this.lblTaxLocalAmount.TabIndex = 16;
this.lblTaxLocalAmount.Text = "税额";
//111======400
this.txtTaxLocalAmount.Location = new System.Drawing.Point(173,396);
this.txtTaxLocalAmount.Name ="txtTaxLocalAmount";
this.txtTaxLocalAmount.Size = new System.Drawing.Size(100, 21);
this.txtTaxLocalAmount.TabIndex = 16;
this.Controls.Add(this.lblTaxLocalAmount);
this.Controls.Add(this.txtTaxLocalAmount);

           //#####LocalPayableAmount###Decimal
this.lblLocalPayableAmount.AutoSize = true;
this.lblLocalPayableAmount.Location = new System.Drawing.Point(100,425);
this.lblLocalPayableAmount.Name = "lblLocalPayableAmount";
this.lblLocalPayableAmount.Size = new System.Drawing.Size(41, 12);
this.lblLocalPayableAmount.TabIndex = 17;
this.lblLocalPayableAmount.Text = "金额小计";
//111======425
this.txtLocalPayableAmount.Location = new System.Drawing.Point(173,421);
this.txtLocalPayableAmount.Name ="txtLocalPayableAmount";
this.txtLocalPayableAmount.Size = new System.Drawing.Size(100, 21);
this.txtLocalPayableAmount.TabIndex = 17;
this.Controls.Add(this.lblLocalPayableAmount);
this.Controls.Add(this.txtLocalPayableAmount);

           //#####300Summary###String
this.lblSummary.AutoSize = true;
this.lblSummary.Location = new System.Drawing.Point(100,450);
this.lblSummary.Name = "lblSummary";
this.lblSummary.Size = new System.Drawing.Size(41, 12);
this.lblSummary.TabIndex = 18;
this.lblSummary.Text = "摘要";
this.txtSummary.Location = new System.Drawing.Point(173,446);
this.txtSummary.Name = "txtSummary";
this.txtSummary.Size = new System.Drawing.Size(100, 21);
this.txtSummary.TabIndex = 18;
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
           // this.kryptonPanel1.TabIndex = 18;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblARAPId );
this.Controls.Add(this.cmbARAPId );

                this.Controls.Add(this.lblBizType );
this.Controls.Add(this.txtBizType );

                this.Controls.Add(this.lblSourceBill_ID );
this.Controls.Add(this.txtSourceBill_ID );

                this.Controls.Add(this.lblSourceBillNO );
this.Controls.Add(this.txtSourceBillNO );

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

                            // 
            // "tb_FM_ReceivablePayableDetailEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_FM_ReceivablePayableDetailEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblARAPId;
private Krypton.Toolkit.KryptonComboBox cmbARAPId;

    
        
              private Krypton.Toolkit.KryptonLabel lblBizType;
private Krypton.Toolkit.KryptonTextBox txtBizType;

    
        
              private Krypton.Toolkit.KryptonLabel lblSourceBill_ID;
private Krypton.Toolkit.KryptonTextBox txtSourceBill_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblSourceBillNO;
private Krypton.Toolkit.KryptonTextBox txtSourceBillNO;

    
        
              private Krypton.Toolkit.KryptonLabel lblProdDetailID;
private Krypton.Toolkit.KryptonComboBox cmbProdDetailID;

    
        
              private Krypton.Toolkit.KryptonLabel lblproperty;
private Krypton.Toolkit.KryptonTextBox txtproperty;

    
        
              private Krypton.Toolkit.KryptonLabel lblSpecifications;
private Krypton.Toolkit.KryptonTextBox txtSpecifications;

    
        
              private Krypton.Toolkit.KryptonLabel lblUnit_ID;
private Krypton.Toolkit.KryptonComboBox cmbUnit_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblIncludeTax;
private Krypton.Toolkit.KryptonCheckBox chkIncludeTax;

    
        
              private Krypton.Toolkit.KryptonLabel lblExchangeRate;
private Krypton.Toolkit.KryptonTextBox txtExchangeRate;

    
        
              private Krypton.Toolkit.KryptonLabel lblUnitPrice;
private Krypton.Toolkit.KryptonTextBox txtUnitPrice;

    
        
              private Krypton.Toolkit.KryptonLabel lblQuantity;
private Krypton.Toolkit.KryptonTextBox txtQuantity;

    
        
              private Krypton.Toolkit.KryptonLabel lblCustomerPartNo;
private Krypton.Toolkit.KryptonTextBox txtCustomerPartNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblDescription;
private Krypton.Toolkit.KryptonTextBox txtDescription;

    
        
              private Krypton.Toolkit.KryptonLabel lblTaxRate;
private Krypton.Toolkit.KryptonTextBox txtTaxRate;

    
        
              private Krypton.Toolkit.KryptonLabel lblTaxLocalAmount;
private Krypton.Toolkit.KryptonTextBox txtTaxLocalAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblLocalPayableAmount;
private Krypton.Toolkit.KryptonTextBox txtLocalPayableAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblSummary;
private Krypton.Toolkit.KryptonTextBox txtSummary;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

