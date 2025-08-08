// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:08
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 采购订单明细表
    /// </summary>
    partial class tb_PurOrderDetailEdit
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
     this.lblProdDetailID = new Krypton.Toolkit.KryptonLabel();
this.cmbProdDetailID = new Krypton.Toolkit.KryptonComboBox();

this.lblPurOrder_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbPurOrder_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblLocation_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbLocation_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblproperty = new Krypton.Toolkit.KryptonLabel();
this.txtproperty = new Krypton.Toolkit.KryptonTextBox();
this.txtproperty.Multiline = true;

this.lblQuantity = new Krypton.Toolkit.KryptonLabel();
this.txtQuantity = new Krypton.Toolkit.KryptonTextBox();

this.lblUnitPrice = new Krypton.Toolkit.KryptonLabel();
this.txtUnitPrice = new Krypton.Toolkit.KryptonTextBox();

this.lblDiscount = new Krypton.Toolkit.KryptonLabel();
this.txtDiscount = new Krypton.Toolkit.KryptonTextBox();

this.lblTransactionPrice = new Krypton.Toolkit.KryptonLabel();
this.txtTransactionPrice = new Krypton.Toolkit.KryptonTextBox();

this.lblTaxRate = new Krypton.Toolkit.KryptonLabel();
this.txtTaxRate = new Krypton.Toolkit.KryptonTextBox();

this.lblTaxAmount = new Krypton.Toolkit.KryptonLabel();
this.txtTaxAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblCustomizedCost = new Krypton.Toolkit.KryptonLabel();
this.txtCustomizedCost = new Krypton.Toolkit.KryptonTextBox();

this.lblUntaxedCustomizedCost = new Krypton.Toolkit.KryptonLabel();
this.txtUntaxedCustomizedCost = new Krypton.Toolkit.KryptonTextBox();

this.lblUntaxedUnitPrice = new Krypton.Toolkit.KryptonLabel();
this.txtUntaxedUnitPrice = new Krypton.Toolkit.KryptonTextBox();

this.lblSubtotalAmount = new Krypton.Toolkit.KryptonLabel();
this.txtSubtotalAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblSubtotalUntaxedAmount = new Krypton.Toolkit.KryptonLabel();
this.txtSubtotalUntaxedAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblIsGift = new Krypton.Toolkit.KryptonLabel();
this.chkIsGift = new Krypton.Toolkit.KryptonCheckBox();
this.chkIsGift.Values.Text ="";

this.lblPreDeliveryDate = new Krypton.Toolkit.KryptonLabel();
this.dtpPreDeliveryDate = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblVendorModelCode = new Krypton.Toolkit.KryptonLabel();
this.txtVendorModelCode = new Krypton.Toolkit.KryptonTextBox();

this.lblCustomertModel = new Krypton.Toolkit.KryptonLabel();
this.txtCustomertModel = new Krypton.Toolkit.KryptonTextBox();

this.lblDeliveredQuantity = new Krypton.Toolkit.KryptonLabel();
this.txtDeliveredQuantity = new Krypton.Toolkit.KryptonTextBox();

this.lblIncludingTax = new Krypton.Toolkit.KryptonLabel();
this.chkIncludingTax = new Krypton.Toolkit.KryptonCheckBox();
this.chkIncludingTax.Values.Text ="";

this.lblNotes = new Krypton.Toolkit.KryptonLabel();
this.txtNotes = new Krypton.Toolkit.KryptonTextBox();
this.txtNotes.Multiline = true;

this.lblTotalReturnedQty = new Krypton.Toolkit.KryptonLabel();
this.txtTotalReturnedQty = new Krypton.Toolkit.KryptonTextBox();

this.lblUndeliveredQty = new Krypton.Toolkit.KryptonLabel();
this.txtUndeliveredQty = new Krypton.Toolkit.KryptonTextBox();

    
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
     
            //#####ProdDetailID###Int64
//属性测试25ProdDetailID
this.lblProdDetailID.AutoSize = true;
this.lblProdDetailID.Location = new System.Drawing.Point(100,25);
this.lblProdDetailID.Name = "lblProdDetailID";
this.lblProdDetailID.Size = new System.Drawing.Size(41, 12);
this.lblProdDetailID.TabIndex = 1;
this.lblProdDetailID.Text = "货品详情";
//111======25
this.cmbProdDetailID.Location = new System.Drawing.Point(173,21);
this.cmbProdDetailID.Name ="cmbProdDetailID";
this.cmbProdDetailID.Size = new System.Drawing.Size(100, 21);
this.cmbProdDetailID.TabIndex = 1;
this.Controls.Add(this.lblProdDetailID);
this.Controls.Add(this.cmbProdDetailID);

           //#####PurOrder_ID###Int64
//属性测试50PurOrder_ID
//属性测试50PurOrder_ID
//属性测试50PurOrder_ID
this.lblPurOrder_ID.AutoSize = true;
this.lblPurOrder_ID.Location = new System.Drawing.Point(100,50);
this.lblPurOrder_ID.Name = "lblPurOrder_ID";
this.lblPurOrder_ID.Size = new System.Drawing.Size(41, 12);
this.lblPurOrder_ID.TabIndex = 2;
this.lblPurOrder_ID.Text = "";
//111======50
this.cmbPurOrder_ID.Location = new System.Drawing.Point(173,46);
this.cmbPurOrder_ID.Name ="cmbPurOrder_ID";
this.cmbPurOrder_ID.Size = new System.Drawing.Size(100, 21);
this.cmbPurOrder_ID.TabIndex = 2;
this.Controls.Add(this.lblPurOrder_ID);
this.Controls.Add(this.cmbPurOrder_ID);

           //#####Location_ID###Int64
//属性测试75Location_ID
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
this.lblQuantity.AutoSize = true;
this.lblQuantity.Location = new System.Drawing.Point(100,125);
this.lblQuantity.Name = "lblQuantity";
this.lblQuantity.Size = new System.Drawing.Size(41, 12);
this.lblQuantity.TabIndex = 5;
this.lblQuantity.Text = "数量";
this.txtQuantity.Location = new System.Drawing.Point(173,121);
this.txtQuantity.Name = "txtQuantity";
this.txtQuantity.Size = new System.Drawing.Size(100, 21);
this.txtQuantity.TabIndex = 5;
this.Controls.Add(this.lblQuantity);
this.Controls.Add(this.txtQuantity);

           //#####UnitPrice###Decimal
this.lblUnitPrice.AutoSize = true;
this.lblUnitPrice.Location = new System.Drawing.Point(100,150);
this.lblUnitPrice.Name = "lblUnitPrice";
this.lblUnitPrice.Size = new System.Drawing.Size(41, 12);
this.lblUnitPrice.TabIndex = 6;
this.lblUnitPrice.Text = "单价";
//111======150
this.txtUnitPrice.Location = new System.Drawing.Point(173,146);
this.txtUnitPrice.Name ="txtUnitPrice";
this.txtUnitPrice.Size = new System.Drawing.Size(100, 21);
this.txtUnitPrice.TabIndex = 6;
this.Controls.Add(this.lblUnitPrice);
this.Controls.Add(this.txtUnitPrice);

           //#####Discount###Decimal
this.lblDiscount.AutoSize = true;
this.lblDiscount.Location = new System.Drawing.Point(100,175);
this.lblDiscount.Name = "lblDiscount";
this.lblDiscount.Size = new System.Drawing.Size(41, 12);
this.lblDiscount.TabIndex = 7;
this.lblDiscount.Text = "折扣";
//111======175
this.txtDiscount.Location = new System.Drawing.Point(173,171);
this.txtDiscount.Name ="txtDiscount";
this.txtDiscount.Size = new System.Drawing.Size(100, 21);
this.txtDiscount.TabIndex = 7;
this.Controls.Add(this.lblDiscount);
this.Controls.Add(this.txtDiscount);

           //#####TransactionPrice###Decimal
this.lblTransactionPrice.AutoSize = true;
this.lblTransactionPrice.Location = new System.Drawing.Point(100,200);
this.lblTransactionPrice.Name = "lblTransactionPrice";
this.lblTransactionPrice.Size = new System.Drawing.Size(41, 12);
this.lblTransactionPrice.TabIndex = 8;
this.lblTransactionPrice.Text = "成交单价";
//111======200
this.txtTransactionPrice.Location = new System.Drawing.Point(173,196);
this.txtTransactionPrice.Name ="txtTransactionPrice";
this.txtTransactionPrice.Size = new System.Drawing.Size(100, 21);
this.txtTransactionPrice.TabIndex = 8;
this.Controls.Add(this.lblTransactionPrice);
this.Controls.Add(this.txtTransactionPrice);

           //#####TaxRate###Decimal
this.lblTaxRate.AutoSize = true;
this.lblTaxRate.Location = new System.Drawing.Point(100,225);
this.lblTaxRate.Name = "lblTaxRate";
this.lblTaxRate.Size = new System.Drawing.Size(41, 12);
this.lblTaxRate.TabIndex = 9;
this.lblTaxRate.Text = "税率";
//111======225
this.txtTaxRate.Location = new System.Drawing.Point(173,221);
this.txtTaxRate.Name ="txtTaxRate";
this.txtTaxRate.Size = new System.Drawing.Size(100, 21);
this.txtTaxRate.TabIndex = 9;
this.Controls.Add(this.lblTaxRate);
this.Controls.Add(this.txtTaxRate);

           //#####TaxAmount###Decimal
this.lblTaxAmount.AutoSize = true;
this.lblTaxAmount.Location = new System.Drawing.Point(100,250);
this.lblTaxAmount.Name = "lblTaxAmount";
this.lblTaxAmount.Size = new System.Drawing.Size(41, 12);
this.lblTaxAmount.TabIndex = 10;
this.lblTaxAmount.Text = "税额";
//111======250
this.txtTaxAmount.Location = new System.Drawing.Point(173,246);
this.txtTaxAmount.Name ="txtTaxAmount";
this.txtTaxAmount.Size = new System.Drawing.Size(100, 21);
this.txtTaxAmount.TabIndex = 10;
this.Controls.Add(this.lblTaxAmount);
this.Controls.Add(this.txtTaxAmount);

           //#####CustomizedCost###Decimal
this.lblCustomizedCost.AutoSize = true;
this.lblCustomizedCost.Location = new System.Drawing.Point(100,275);
this.lblCustomizedCost.Name = "lblCustomizedCost";
this.lblCustomizedCost.Size = new System.Drawing.Size(41, 12);
this.lblCustomizedCost.TabIndex = 11;
this.lblCustomizedCost.Text = "定制成本";
//111======275
this.txtCustomizedCost.Location = new System.Drawing.Point(173,271);
this.txtCustomizedCost.Name ="txtCustomizedCost";
this.txtCustomizedCost.Size = new System.Drawing.Size(100, 21);
this.txtCustomizedCost.TabIndex = 11;
this.Controls.Add(this.lblCustomizedCost);
this.Controls.Add(this.txtCustomizedCost);

           //#####UntaxedCustomizedCost###Decimal
this.lblUntaxedCustomizedCost.AutoSize = true;
this.lblUntaxedCustomizedCost.Location = new System.Drawing.Point(100,300);
this.lblUntaxedCustomizedCost.Name = "lblUntaxedCustomizedCost";
this.lblUntaxedCustomizedCost.Size = new System.Drawing.Size(41, 12);
this.lblUntaxedCustomizedCost.TabIndex = 12;
this.lblUntaxedCustomizedCost.Text = "未税定制成本";
//111======300
this.txtUntaxedCustomizedCost.Location = new System.Drawing.Point(173,296);
this.txtUntaxedCustomizedCost.Name ="txtUntaxedCustomizedCost";
this.txtUntaxedCustomizedCost.Size = new System.Drawing.Size(100, 21);
this.txtUntaxedCustomizedCost.TabIndex = 12;
this.Controls.Add(this.lblUntaxedCustomizedCost);
this.Controls.Add(this.txtUntaxedCustomizedCost);

           //#####UntaxedUnitPrice###Decimal
this.lblUntaxedUnitPrice.AutoSize = true;
this.lblUntaxedUnitPrice.Location = new System.Drawing.Point(100,325);
this.lblUntaxedUnitPrice.Name = "lblUntaxedUnitPrice";
this.lblUntaxedUnitPrice.Size = new System.Drawing.Size(41, 12);
this.lblUntaxedUnitPrice.TabIndex = 13;
this.lblUntaxedUnitPrice.Text = "未税单价";
//111======325
this.txtUntaxedUnitPrice.Location = new System.Drawing.Point(173,321);
this.txtUntaxedUnitPrice.Name ="txtUntaxedUnitPrice";
this.txtUntaxedUnitPrice.Size = new System.Drawing.Size(100, 21);
this.txtUntaxedUnitPrice.TabIndex = 13;
this.Controls.Add(this.lblUntaxedUnitPrice);
this.Controls.Add(this.txtUntaxedUnitPrice);

           //#####SubtotalAmount###Decimal
this.lblSubtotalAmount.AutoSize = true;
this.lblSubtotalAmount.Location = new System.Drawing.Point(100,350);
this.lblSubtotalAmount.Name = "lblSubtotalAmount";
this.lblSubtotalAmount.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalAmount.TabIndex = 14;
this.lblSubtotalAmount.Text = "成交金额";
//111======350
this.txtSubtotalAmount.Location = new System.Drawing.Point(173,346);
this.txtSubtotalAmount.Name ="txtSubtotalAmount";
this.txtSubtotalAmount.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalAmount.TabIndex = 14;
this.Controls.Add(this.lblSubtotalAmount);
this.Controls.Add(this.txtSubtotalAmount);

           //#####SubtotalUntaxedAmount###Decimal
this.lblSubtotalUntaxedAmount.AutoSize = true;
this.lblSubtotalUntaxedAmount.Location = new System.Drawing.Point(100,375);
this.lblSubtotalUntaxedAmount.Name = "lblSubtotalUntaxedAmount";
this.lblSubtotalUntaxedAmount.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalUntaxedAmount.TabIndex = 15;
this.lblSubtotalUntaxedAmount.Text = "未税金额小计";
//111======375
this.txtSubtotalUntaxedAmount.Location = new System.Drawing.Point(173,371);
this.txtSubtotalUntaxedAmount.Name ="txtSubtotalUntaxedAmount";
this.txtSubtotalUntaxedAmount.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalUntaxedAmount.TabIndex = 15;
this.Controls.Add(this.lblSubtotalUntaxedAmount);
this.Controls.Add(this.txtSubtotalUntaxedAmount);

           //#####IsGift###Boolean
this.lblIsGift.AutoSize = true;
this.lblIsGift.Location = new System.Drawing.Point(100,400);
this.lblIsGift.Name = "lblIsGift";
this.lblIsGift.Size = new System.Drawing.Size(41, 12);
this.lblIsGift.TabIndex = 16;
this.lblIsGift.Text = "赠品";
this.chkIsGift.Location = new System.Drawing.Point(173,396);
this.chkIsGift.Name = "chkIsGift";
this.chkIsGift.Size = new System.Drawing.Size(100, 21);
this.chkIsGift.TabIndex = 16;
this.Controls.Add(this.lblIsGift);
this.Controls.Add(this.chkIsGift);

           //#####PreDeliveryDate###DateTime
this.lblPreDeliveryDate.AutoSize = true;
this.lblPreDeliveryDate.Location = new System.Drawing.Point(100,425);
this.lblPreDeliveryDate.Name = "lblPreDeliveryDate";
this.lblPreDeliveryDate.Size = new System.Drawing.Size(41, 12);
this.lblPreDeliveryDate.TabIndex = 17;
this.lblPreDeliveryDate.Text = "预交日期";
//111======425
this.dtpPreDeliveryDate.Location = new System.Drawing.Point(173,421);
this.dtpPreDeliveryDate.Name ="dtpPreDeliveryDate";
this.dtpPreDeliveryDate.Size = new System.Drawing.Size(100, 21);
this.dtpPreDeliveryDate.TabIndex = 17;
this.Controls.Add(this.lblPreDeliveryDate);
this.Controls.Add(this.dtpPreDeliveryDate);

           //#####50VendorModelCode###String
this.lblVendorModelCode.AutoSize = true;
this.lblVendorModelCode.Location = new System.Drawing.Point(100,450);
this.lblVendorModelCode.Name = "lblVendorModelCode";
this.lblVendorModelCode.Size = new System.Drawing.Size(41, 12);
this.lblVendorModelCode.TabIndex = 18;
this.lblVendorModelCode.Text = "厂商型号";
this.txtVendorModelCode.Location = new System.Drawing.Point(173,446);
this.txtVendorModelCode.Name = "txtVendorModelCode";
this.txtVendorModelCode.Size = new System.Drawing.Size(100, 21);
this.txtVendorModelCode.TabIndex = 18;
this.Controls.Add(this.lblVendorModelCode);
this.Controls.Add(this.txtVendorModelCode);

           //#####50CustomertModel###String
this.lblCustomertModel.AutoSize = true;
this.lblCustomertModel.Location = new System.Drawing.Point(100,475);
this.lblCustomertModel.Name = "lblCustomertModel";
this.lblCustomertModel.Size = new System.Drawing.Size(41, 12);
this.lblCustomertModel.TabIndex = 19;
this.lblCustomertModel.Text = "客户型号";
this.txtCustomertModel.Location = new System.Drawing.Point(173,471);
this.txtCustomertModel.Name = "txtCustomertModel";
this.txtCustomertModel.Size = new System.Drawing.Size(100, 21);
this.txtCustomertModel.TabIndex = 19;
this.Controls.Add(this.lblCustomertModel);
this.Controls.Add(this.txtCustomertModel);

           //#####DeliveredQuantity###Int32
//属性测试500DeliveredQuantity
//属性测试500DeliveredQuantity
//属性测试500DeliveredQuantity
this.lblDeliveredQuantity.AutoSize = true;
this.lblDeliveredQuantity.Location = new System.Drawing.Point(100,500);
this.lblDeliveredQuantity.Name = "lblDeliveredQuantity";
this.lblDeliveredQuantity.Size = new System.Drawing.Size(41, 12);
this.lblDeliveredQuantity.TabIndex = 20;
this.lblDeliveredQuantity.Text = "已交数";
this.txtDeliveredQuantity.Location = new System.Drawing.Point(173,496);
this.txtDeliveredQuantity.Name = "txtDeliveredQuantity";
this.txtDeliveredQuantity.Size = new System.Drawing.Size(100, 21);
this.txtDeliveredQuantity.TabIndex = 20;
this.Controls.Add(this.lblDeliveredQuantity);
this.Controls.Add(this.txtDeliveredQuantity);

           //#####IncludingTax###Boolean
this.lblIncludingTax.AutoSize = true;
this.lblIncludingTax.Location = new System.Drawing.Point(100,525);
this.lblIncludingTax.Name = "lblIncludingTax";
this.lblIncludingTax.Size = new System.Drawing.Size(41, 12);
this.lblIncludingTax.TabIndex = 21;
this.lblIncludingTax.Text = "含税";
this.chkIncludingTax.Location = new System.Drawing.Point(173,521);
this.chkIncludingTax.Name = "chkIncludingTax";
this.chkIncludingTax.Size = new System.Drawing.Size(100, 21);
this.chkIncludingTax.TabIndex = 21;
this.Controls.Add(this.lblIncludingTax);
this.Controls.Add(this.chkIncludingTax);

           //#####1000Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,550);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 22;
this.lblNotes.Text = "备注";
this.txtNotes.Location = new System.Drawing.Point(173,546);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 22;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

           //#####TotalReturnedQty###Int32
//属性测试575TotalReturnedQty
//属性测试575TotalReturnedQty
//属性测试575TotalReturnedQty
this.lblTotalReturnedQty.AutoSize = true;
this.lblTotalReturnedQty.Location = new System.Drawing.Point(100,575);
this.lblTotalReturnedQty.Name = "lblTotalReturnedQty";
this.lblTotalReturnedQty.Size = new System.Drawing.Size(41, 12);
this.lblTotalReturnedQty.TabIndex = 23;
this.lblTotalReturnedQty.Text = "退回数";
this.txtTotalReturnedQty.Location = new System.Drawing.Point(173,571);
this.txtTotalReturnedQty.Name = "txtTotalReturnedQty";
this.txtTotalReturnedQty.Size = new System.Drawing.Size(100, 21);
this.txtTotalReturnedQty.TabIndex = 23;
this.Controls.Add(this.lblTotalReturnedQty);
this.Controls.Add(this.txtTotalReturnedQty);

           //#####UndeliveredQty###Int32
//属性测试600UndeliveredQty
//属性测试600UndeliveredQty
//属性测试600UndeliveredQty
this.lblUndeliveredQty.AutoSize = true;
this.lblUndeliveredQty.Location = new System.Drawing.Point(100,600);
this.lblUndeliveredQty.Name = "lblUndeliveredQty";
this.lblUndeliveredQty.Size = new System.Drawing.Size(41, 12);
this.lblUndeliveredQty.TabIndex = 24;
this.lblUndeliveredQty.Text = "未交数量";
this.txtUndeliveredQty.Location = new System.Drawing.Point(173,596);
this.txtUndeliveredQty.Name = "txtUndeliveredQty";
this.txtUndeliveredQty.Size = new System.Drawing.Size(100, 21);
this.txtUndeliveredQty.TabIndex = 24;
this.Controls.Add(this.lblUndeliveredQty);
this.Controls.Add(this.txtUndeliveredQty);

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
           // this.kryptonPanel1.TabIndex = 24;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblProdDetailID );
this.Controls.Add(this.cmbProdDetailID );

                this.Controls.Add(this.lblPurOrder_ID );
this.Controls.Add(this.cmbPurOrder_ID );

                this.Controls.Add(this.lblLocation_ID );
this.Controls.Add(this.cmbLocation_ID );

                this.Controls.Add(this.lblproperty );
this.Controls.Add(this.txtproperty );

                this.Controls.Add(this.lblQuantity );
this.Controls.Add(this.txtQuantity );

                this.Controls.Add(this.lblUnitPrice );
this.Controls.Add(this.txtUnitPrice );

                this.Controls.Add(this.lblDiscount );
this.Controls.Add(this.txtDiscount );

                this.Controls.Add(this.lblTransactionPrice );
this.Controls.Add(this.txtTransactionPrice );

                this.Controls.Add(this.lblTaxRate );
this.Controls.Add(this.txtTaxRate );

                this.Controls.Add(this.lblTaxAmount );
this.Controls.Add(this.txtTaxAmount );

                this.Controls.Add(this.lblCustomizedCost );
this.Controls.Add(this.txtCustomizedCost );

                this.Controls.Add(this.lblUntaxedCustomizedCost );
this.Controls.Add(this.txtUntaxedCustomizedCost );

                this.Controls.Add(this.lblUntaxedUnitPrice );
this.Controls.Add(this.txtUntaxedUnitPrice );

                this.Controls.Add(this.lblSubtotalAmount );
this.Controls.Add(this.txtSubtotalAmount );

                this.Controls.Add(this.lblSubtotalUntaxedAmount );
this.Controls.Add(this.txtSubtotalUntaxedAmount );

                this.Controls.Add(this.lblIsGift );
this.Controls.Add(this.chkIsGift );

                this.Controls.Add(this.lblPreDeliveryDate );
this.Controls.Add(this.dtpPreDeliveryDate );

                this.Controls.Add(this.lblVendorModelCode );
this.Controls.Add(this.txtVendorModelCode );

                this.Controls.Add(this.lblCustomertModel );
this.Controls.Add(this.txtCustomertModel );

                this.Controls.Add(this.lblDeliveredQuantity );
this.Controls.Add(this.txtDeliveredQuantity );

                this.Controls.Add(this.lblIncludingTax );
this.Controls.Add(this.chkIncludingTax );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                this.Controls.Add(this.lblTotalReturnedQty );
this.Controls.Add(this.txtTotalReturnedQty );

                this.Controls.Add(this.lblUndeliveredQty );
this.Controls.Add(this.txtUndeliveredQty );

                            // 
            // "tb_PurOrderDetailEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_PurOrderDetailEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblProdDetailID;
private Krypton.Toolkit.KryptonComboBox cmbProdDetailID;

    
        
              private Krypton.Toolkit.KryptonLabel lblPurOrder_ID;
private Krypton.Toolkit.KryptonComboBox cmbPurOrder_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblLocation_ID;
private Krypton.Toolkit.KryptonComboBox cmbLocation_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblproperty;
private Krypton.Toolkit.KryptonTextBox txtproperty;

    
        
              private Krypton.Toolkit.KryptonLabel lblQuantity;
private Krypton.Toolkit.KryptonTextBox txtQuantity;

    
        
              private Krypton.Toolkit.KryptonLabel lblUnitPrice;
private Krypton.Toolkit.KryptonTextBox txtUnitPrice;

    
        
              private Krypton.Toolkit.KryptonLabel lblDiscount;
private Krypton.Toolkit.KryptonTextBox txtDiscount;

    
        
              private Krypton.Toolkit.KryptonLabel lblTransactionPrice;
private Krypton.Toolkit.KryptonTextBox txtTransactionPrice;

    
        
              private Krypton.Toolkit.KryptonLabel lblTaxRate;
private Krypton.Toolkit.KryptonTextBox txtTaxRate;

    
        
              private Krypton.Toolkit.KryptonLabel lblTaxAmount;
private Krypton.Toolkit.KryptonTextBox txtTaxAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblCustomizedCost;
private Krypton.Toolkit.KryptonTextBox txtCustomizedCost;

    
        
              private Krypton.Toolkit.KryptonLabel lblUntaxedCustomizedCost;
private Krypton.Toolkit.KryptonTextBox txtUntaxedCustomizedCost;

    
        
              private Krypton.Toolkit.KryptonLabel lblUntaxedUnitPrice;
private Krypton.Toolkit.KryptonTextBox txtUntaxedUnitPrice;

    
        
              private Krypton.Toolkit.KryptonLabel lblSubtotalAmount;
private Krypton.Toolkit.KryptonTextBox txtSubtotalAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblSubtotalUntaxedAmount;
private Krypton.Toolkit.KryptonTextBox txtSubtotalUntaxedAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblIsGift;
private Krypton.Toolkit.KryptonCheckBox chkIsGift;

    
        
              private Krypton.Toolkit.KryptonLabel lblPreDeliveryDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpPreDeliveryDate;

    
        
              private Krypton.Toolkit.KryptonLabel lblVendorModelCode;
private Krypton.Toolkit.KryptonTextBox txtVendorModelCode;

    
        
              private Krypton.Toolkit.KryptonLabel lblCustomertModel;
private Krypton.Toolkit.KryptonTextBox txtCustomertModel;

    
        
              private Krypton.Toolkit.KryptonLabel lblDeliveredQuantity;
private Krypton.Toolkit.KryptonTextBox txtDeliveredQuantity;

    
        
              private Krypton.Toolkit.KryptonLabel lblIncludingTax;
private Krypton.Toolkit.KryptonCheckBox chkIncludingTax;

    
        
              private Krypton.Toolkit.KryptonLabel lblNotes;
private Krypton.Toolkit.KryptonTextBox txtNotes;

    
        
              private Krypton.Toolkit.KryptonLabel lblTotalReturnedQty;
private Krypton.Toolkit.KryptonTextBox txtTotalReturnedQty;

    
        
              private Krypton.Toolkit.KryptonLabel lblUndeliveredQty;
private Krypton.Toolkit.KryptonTextBox txtUndeliveredQty;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

