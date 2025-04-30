// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/30/2025 13:26:29
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 系统配置表
    /// </summary>
    partial class tb_SystemConfigEdit
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
     this.lblQtyDataPrecision = new Krypton.Toolkit.KryptonLabel();
this.txtQtyDataPrecision = new Krypton.Toolkit.KryptonTextBox();

this.lblTaxRateDataPrecision = new Krypton.Toolkit.KryptonLabel();
this.txtTaxRateDataPrecision = new Krypton.Toolkit.KryptonTextBox();

this.lblMoneyDataPrecision = new Krypton.Toolkit.KryptonLabel();
this.txtMoneyDataPrecision = new Krypton.Toolkit.KryptonTextBox();

this.lblCheckNegativeInventory = new Krypton.Toolkit.KryptonLabel();
this.chkCheckNegativeInventory = new Krypton.Toolkit.KryptonCheckBox();
this.chkCheckNegativeInventory.Values.Text ="";
this.chkCheckNegativeInventory.Checked = true;
this.chkCheckNegativeInventory.CheckState = System.Windows.Forms.CheckState.Checked;

this.lblCostCalculationMethod = new Krypton.Toolkit.KryptonLabel();
this.txtCostCalculationMethod = new Krypton.Toolkit.KryptonTextBox();

this.lblShowDebugInfo = new Krypton.Toolkit.KryptonLabel();
this.chkShowDebugInfo = new Krypton.Toolkit.KryptonCheckBox();
this.chkShowDebugInfo.Values.Text ="";

this.lblOwnershipControl = new Krypton.Toolkit.KryptonLabel();
this.chkOwnershipControl = new Krypton.Toolkit.KryptonCheckBox();
this.chkOwnershipControl.Values.Text ="";
this.chkOwnershipControl.Checked = true;
this.chkOwnershipControl.CheckState = System.Windows.Forms.CheckState.Checked;

this.lblSaleBizLimited = new Krypton.Toolkit.KryptonLabel();
this.chkSaleBizLimited = new Krypton.Toolkit.KryptonCheckBox();
this.chkSaleBizLimited.Values.Text ="";

this.lblDepartBizLimited = new Krypton.Toolkit.KryptonLabel();
this.chkDepartBizLimited = new Krypton.Toolkit.KryptonCheckBox();
this.chkDepartBizLimited.Values.Text ="";

this.lblPurchsaeBizLimited = new Krypton.Toolkit.KryptonLabel();
this.chkPurchsaeBizLimited = new Krypton.Toolkit.KryptonCheckBox();
this.chkPurchsaeBizLimited.Values.Text ="";

this.lblCurrencyDataPrecisionAutoAddZero = new Krypton.Toolkit.KryptonLabel();
this.chkCurrencyDataPrecisionAutoAddZero = new Krypton.Toolkit.KryptonCheckBox();
this.chkCurrencyDataPrecisionAutoAddZero.Values.Text ="";
this.chkCurrencyDataPrecisionAutoAddZero.Checked = true;
this.chkCurrencyDataPrecisionAutoAddZero.CheckState = System.Windows.Forms.CheckState.Checked;

this.lblUseBarCode = new Krypton.Toolkit.KryptonLabel();
this.chkUseBarCode = new Krypton.Toolkit.KryptonCheckBox();
this.chkUseBarCode.Values.Text ="";

this.lblQueryPageLayoutCustomize = new Krypton.Toolkit.KryptonLabel();
this.chkQueryPageLayoutCustomize = new Krypton.Toolkit.KryptonCheckBox();
this.chkQueryPageLayoutCustomize.Values.Text ="";

this.lblAutoApprovedSaleOrderAmount = new Krypton.Toolkit.KryptonLabel();
this.txtAutoApprovedSaleOrderAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblAutoApprovedPurOrderAmount = new Krypton.Toolkit.KryptonLabel();
this.txtAutoApprovedPurOrderAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblQueryGridColCustomize = new Krypton.Toolkit.KryptonLabel();
this.chkQueryGridColCustomize = new Krypton.Toolkit.KryptonCheckBox();
this.chkQueryGridColCustomize.Values.Text ="";

this.lblBillGridColCustomize = new Krypton.Toolkit.KryptonLabel();
this.chkBillGridColCustomize = new Krypton.Toolkit.KryptonCheckBox();
this.chkBillGridColCustomize.Values.Text ="";

this.lblIsDebug = new Krypton.Toolkit.KryptonLabel();
this.chkIsDebug = new Krypton.Toolkit.KryptonCheckBox();
this.chkIsDebug.Values.Text ="";

this.lblEnableVoucherModule = new Krypton.Toolkit.KryptonLabel();
this.chkEnableVoucherModule = new Krypton.Toolkit.KryptonCheckBox();
this.chkEnableVoucherModule.Values.Text ="";
this.chkEnableVoucherModule.Checked = true;
this.chkEnableVoucherModule.CheckState = System.Windows.Forms.CheckState.Checked;

this.lblEnableContractModule = new Krypton.Toolkit.KryptonLabel();
this.chkEnableContractModule = new Krypton.Toolkit.KryptonCheckBox();
this.chkEnableContractModule.Values.Text ="";
this.chkEnableContractModule.Checked = true;
this.chkEnableContractModule.CheckState = System.Windows.Forms.CheckState.Checked;

this.lblEnableInvoiceModule = new Krypton.Toolkit.KryptonLabel();
this.chkEnableInvoiceModule = new Krypton.Toolkit.KryptonCheckBox();
this.chkEnableInvoiceModule.Values.Text ="";

this.lblEnableMultiCurrency = new Krypton.Toolkit.KryptonLabel();
this.chkEnableMultiCurrency = new Krypton.Toolkit.KryptonCheckBox();
this.chkEnableMultiCurrency.Values.Text ="";
this.chkEnableMultiCurrency.Checked = true;
this.chkEnableMultiCurrency.CheckState = System.Windows.Forms.CheckState.Checked;

this.lblEnableFinancialModule = new Krypton.Toolkit.KryptonLabel();
this.chkEnableFinancialModule = new Krypton.Toolkit.KryptonCheckBox();
this.chkEnableFinancialModule.Values.Text ="";
this.chkEnableFinancialModule.Checked = true;
this.chkEnableFinancialModule.CheckState = System.Windows.Forms.CheckState.Checked;

    
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
     
            //#####QtyDataPrecision###Int32
this.lblQtyDataPrecision.AutoSize = true;
this.lblQtyDataPrecision.Location = new System.Drawing.Point(100,25);
this.lblQtyDataPrecision.Name = "lblQtyDataPrecision";
this.lblQtyDataPrecision.Size = new System.Drawing.Size(41, 12);
this.lblQtyDataPrecision.TabIndex = 1;
this.lblQtyDataPrecision.Text = "数量精度";
this.txtQtyDataPrecision.Location = new System.Drawing.Point(173,21);
this.txtQtyDataPrecision.Name = "txtQtyDataPrecision";
this.txtQtyDataPrecision.Size = new System.Drawing.Size(100, 21);
this.txtQtyDataPrecision.TabIndex = 1;
this.Controls.Add(this.lblQtyDataPrecision);
this.Controls.Add(this.txtQtyDataPrecision);

           //#####TaxRateDataPrecision###Int32
this.lblTaxRateDataPrecision.AutoSize = true;
this.lblTaxRateDataPrecision.Location = new System.Drawing.Point(100,50);
this.lblTaxRateDataPrecision.Name = "lblTaxRateDataPrecision";
this.lblTaxRateDataPrecision.Size = new System.Drawing.Size(41, 12);
this.lblTaxRateDataPrecision.TabIndex = 2;
this.lblTaxRateDataPrecision.Text = "税率精度";
this.txtTaxRateDataPrecision.Location = new System.Drawing.Point(173,46);
this.txtTaxRateDataPrecision.Name = "txtTaxRateDataPrecision";
this.txtTaxRateDataPrecision.Size = new System.Drawing.Size(100, 21);
this.txtTaxRateDataPrecision.TabIndex = 2;
this.Controls.Add(this.lblTaxRateDataPrecision);
this.Controls.Add(this.txtTaxRateDataPrecision);

           //#####MoneyDataPrecision###Int32
this.lblMoneyDataPrecision.AutoSize = true;
this.lblMoneyDataPrecision.Location = new System.Drawing.Point(100,75);
this.lblMoneyDataPrecision.Name = "lblMoneyDataPrecision";
this.lblMoneyDataPrecision.Size = new System.Drawing.Size(41, 12);
this.lblMoneyDataPrecision.TabIndex = 3;
this.lblMoneyDataPrecision.Text = "金额精度";
this.txtMoneyDataPrecision.Location = new System.Drawing.Point(173,71);
this.txtMoneyDataPrecision.Name = "txtMoneyDataPrecision";
this.txtMoneyDataPrecision.Size = new System.Drawing.Size(100, 21);
this.txtMoneyDataPrecision.TabIndex = 3;
this.Controls.Add(this.lblMoneyDataPrecision);
this.Controls.Add(this.txtMoneyDataPrecision);

           //#####CheckNegativeInventory###Boolean
this.lblCheckNegativeInventory.AutoSize = true;
this.lblCheckNegativeInventory.Location = new System.Drawing.Point(100,100);
this.lblCheckNegativeInventory.Name = "lblCheckNegativeInventory";
this.lblCheckNegativeInventory.Size = new System.Drawing.Size(41, 12);
this.lblCheckNegativeInventory.TabIndex = 4;
this.lblCheckNegativeInventory.Text = "允许负库存";
this.chkCheckNegativeInventory.Location = new System.Drawing.Point(173,96);
this.chkCheckNegativeInventory.Name = "chkCheckNegativeInventory";
this.chkCheckNegativeInventory.Size = new System.Drawing.Size(100, 21);
this.chkCheckNegativeInventory.TabIndex = 4;
this.Controls.Add(this.lblCheckNegativeInventory);
this.Controls.Add(this.chkCheckNegativeInventory);

           //#####CostCalculationMethod###Int32
this.lblCostCalculationMethod.AutoSize = true;
this.lblCostCalculationMethod.Location = new System.Drawing.Point(100,125);
this.lblCostCalculationMethod.Name = "lblCostCalculationMethod";
this.lblCostCalculationMethod.Size = new System.Drawing.Size(41, 12);
this.lblCostCalculationMethod.TabIndex = 5;
this.lblCostCalculationMethod.Text = "成本方式";
this.txtCostCalculationMethod.Location = new System.Drawing.Point(173,121);
this.txtCostCalculationMethod.Name = "txtCostCalculationMethod";
this.txtCostCalculationMethod.Size = new System.Drawing.Size(100, 21);
this.txtCostCalculationMethod.TabIndex = 5;
this.Controls.Add(this.lblCostCalculationMethod);
this.Controls.Add(this.txtCostCalculationMethod);

           //#####ShowDebugInfo###Boolean
this.lblShowDebugInfo.AutoSize = true;
this.lblShowDebugInfo.Location = new System.Drawing.Point(100,150);
this.lblShowDebugInfo.Name = "lblShowDebugInfo";
this.lblShowDebugInfo.Size = new System.Drawing.Size(41, 12);
this.lblShowDebugInfo.TabIndex = 6;
this.lblShowDebugInfo.Text = "显示调试信息";
this.chkShowDebugInfo.Location = new System.Drawing.Point(173,146);
this.chkShowDebugInfo.Name = "chkShowDebugInfo";
this.chkShowDebugInfo.Size = new System.Drawing.Size(100, 21);
this.chkShowDebugInfo.TabIndex = 6;
this.Controls.Add(this.lblShowDebugInfo);
this.Controls.Add(this.chkShowDebugInfo);

           //#####OwnershipControl###Boolean
this.lblOwnershipControl.AutoSize = true;
this.lblOwnershipControl.Location = new System.Drawing.Point(100,175);
this.lblOwnershipControl.Name = "lblOwnershipControl";
this.lblOwnershipControl.Size = new System.Drawing.Size(41, 12);
this.lblOwnershipControl.TabIndex = 7;
this.lblOwnershipControl.Text = "数据归属控制";
this.chkOwnershipControl.Location = new System.Drawing.Point(173,171);
this.chkOwnershipControl.Name = "chkOwnershipControl";
this.chkOwnershipControl.Size = new System.Drawing.Size(100, 21);
this.chkOwnershipControl.TabIndex = 7;
this.Controls.Add(this.lblOwnershipControl);
this.Controls.Add(this.chkOwnershipControl);

           //#####SaleBizLimited###Boolean
this.lblSaleBizLimited.AutoSize = true;
this.lblSaleBizLimited.Location = new System.Drawing.Point(100,200);
this.lblSaleBizLimited.Name = "lblSaleBizLimited";
this.lblSaleBizLimited.Size = new System.Drawing.Size(41, 12);
this.lblSaleBizLimited.TabIndex = 8;
this.lblSaleBizLimited.Text = "销售业务范围限制";
this.chkSaleBizLimited.Location = new System.Drawing.Point(173,196);
this.chkSaleBizLimited.Name = "chkSaleBizLimited";
this.chkSaleBizLimited.Size = new System.Drawing.Size(100, 21);
this.chkSaleBizLimited.TabIndex = 8;
this.Controls.Add(this.lblSaleBizLimited);
this.Controls.Add(this.chkSaleBizLimited);

           //#####DepartBizLimited###Boolean
this.lblDepartBizLimited.AutoSize = true;
this.lblDepartBizLimited.Location = new System.Drawing.Point(100,225);
this.lblDepartBizLimited.Name = "lblDepartBizLimited";
this.lblDepartBizLimited.Size = new System.Drawing.Size(41, 12);
this.lblDepartBizLimited.TabIndex = 9;
this.lblDepartBizLimited.Text = "部门范围限制";
this.chkDepartBizLimited.Location = new System.Drawing.Point(173,221);
this.chkDepartBizLimited.Name = "chkDepartBizLimited";
this.chkDepartBizLimited.Size = new System.Drawing.Size(100, 21);
this.chkDepartBizLimited.TabIndex = 9;
this.Controls.Add(this.lblDepartBizLimited);
this.Controls.Add(this.chkDepartBizLimited);

           //#####PurchsaeBizLimited###Boolean
this.lblPurchsaeBizLimited.AutoSize = true;
this.lblPurchsaeBizLimited.Location = new System.Drawing.Point(100,250);
this.lblPurchsaeBizLimited.Name = "lblPurchsaeBizLimited";
this.lblPurchsaeBizLimited.Size = new System.Drawing.Size(41, 12);
this.lblPurchsaeBizLimited.TabIndex = 10;
this.lblPurchsaeBizLimited.Text = "采购业务范围限制";
this.chkPurchsaeBizLimited.Location = new System.Drawing.Point(173,246);
this.chkPurchsaeBizLimited.Name = "chkPurchsaeBizLimited";
this.chkPurchsaeBizLimited.Size = new System.Drawing.Size(100, 21);
this.chkPurchsaeBizLimited.TabIndex = 10;
this.Controls.Add(this.lblPurchsaeBizLimited);
this.Controls.Add(this.chkPurchsaeBizLimited);

           //#####CurrencyDataPrecisionAutoAddZero###Boolean
this.lblCurrencyDataPrecisionAutoAddZero.AutoSize = true;
this.lblCurrencyDataPrecisionAutoAddZero.Location = new System.Drawing.Point(100,275);
this.lblCurrencyDataPrecisionAutoAddZero.Name = "lblCurrencyDataPrecisionAutoAddZero";
this.lblCurrencyDataPrecisionAutoAddZero.Size = new System.Drawing.Size(41, 12);
this.lblCurrencyDataPrecisionAutoAddZero.TabIndex = 11;
this.lblCurrencyDataPrecisionAutoAddZero.Text = "金额精度自动补零";
this.chkCurrencyDataPrecisionAutoAddZero.Location = new System.Drawing.Point(173,271);
this.chkCurrencyDataPrecisionAutoAddZero.Name = "chkCurrencyDataPrecisionAutoAddZero";
this.chkCurrencyDataPrecisionAutoAddZero.Size = new System.Drawing.Size(100, 21);
this.chkCurrencyDataPrecisionAutoAddZero.TabIndex = 11;
this.Controls.Add(this.lblCurrencyDataPrecisionAutoAddZero);
this.Controls.Add(this.chkCurrencyDataPrecisionAutoAddZero);

           //#####UseBarCode###Boolean
this.lblUseBarCode.AutoSize = true;
this.lblUseBarCode.Location = new System.Drawing.Point(100,300);
this.lblUseBarCode.Name = "lblUseBarCode";
this.lblUseBarCode.Size = new System.Drawing.Size(41, 12);
this.lblUseBarCode.TabIndex = 12;
this.lblUseBarCode.Text = "是否启用条码";
this.chkUseBarCode.Location = new System.Drawing.Point(173,296);
this.chkUseBarCode.Name = "chkUseBarCode";
this.chkUseBarCode.Size = new System.Drawing.Size(100, 21);
this.chkUseBarCode.TabIndex = 12;
this.Controls.Add(this.lblUseBarCode);
this.Controls.Add(this.chkUseBarCode);

           //#####QueryPageLayoutCustomize###Boolean
this.lblQueryPageLayoutCustomize.AutoSize = true;
this.lblQueryPageLayoutCustomize.Location = new System.Drawing.Point(100,325);
this.lblQueryPageLayoutCustomize.Name = "lblQueryPageLayoutCustomize";
this.lblQueryPageLayoutCustomize.Size = new System.Drawing.Size(41, 12);
this.lblQueryPageLayoutCustomize.TabIndex = 13;
this.lblQueryPageLayoutCustomize.Text = "查询页布局自定义";
this.chkQueryPageLayoutCustomize.Location = new System.Drawing.Point(173,321);
this.chkQueryPageLayoutCustomize.Name = "chkQueryPageLayoutCustomize";
this.chkQueryPageLayoutCustomize.Size = new System.Drawing.Size(100, 21);
this.chkQueryPageLayoutCustomize.TabIndex = 13;
this.Controls.Add(this.lblQueryPageLayoutCustomize);
this.Controls.Add(this.chkQueryPageLayoutCustomize);

           //#####AutoApprovedSaleOrderAmount###Decimal
this.lblAutoApprovedSaleOrderAmount.AutoSize = true;
this.lblAutoApprovedSaleOrderAmount.Location = new System.Drawing.Point(100,350);
this.lblAutoApprovedSaleOrderAmount.Name = "lblAutoApprovedSaleOrderAmount";
this.lblAutoApprovedSaleOrderAmount.Size = new System.Drawing.Size(41, 12);
this.lblAutoApprovedSaleOrderAmount.TabIndex = 14;
this.lblAutoApprovedSaleOrderAmount.Text = "自动审核销售订单金额";
//111======350
this.txtAutoApprovedSaleOrderAmount.Location = new System.Drawing.Point(173,346);
this.txtAutoApprovedSaleOrderAmount.Name ="txtAutoApprovedSaleOrderAmount";
this.txtAutoApprovedSaleOrderAmount.Size = new System.Drawing.Size(100, 21);
this.txtAutoApprovedSaleOrderAmount.TabIndex = 14;
this.Controls.Add(this.lblAutoApprovedSaleOrderAmount);
this.Controls.Add(this.txtAutoApprovedSaleOrderAmount);

           //#####AutoApprovedPurOrderAmount###Decimal
this.lblAutoApprovedPurOrderAmount.AutoSize = true;
this.lblAutoApprovedPurOrderAmount.Location = new System.Drawing.Point(100,375);
this.lblAutoApprovedPurOrderAmount.Name = "lblAutoApprovedPurOrderAmount";
this.lblAutoApprovedPurOrderAmount.Size = new System.Drawing.Size(41, 12);
this.lblAutoApprovedPurOrderAmount.TabIndex = 15;
this.lblAutoApprovedPurOrderAmount.Text = "自动审核采购订单金额";
//111======375
this.txtAutoApprovedPurOrderAmount.Location = new System.Drawing.Point(173,371);
this.txtAutoApprovedPurOrderAmount.Name ="txtAutoApprovedPurOrderAmount";
this.txtAutoApprovedPurOrderAmount.Size = new System.Drawing.Size(100, 21);
this.txtAutoApprovedPurOrderAmount.TabIndex = 15;
this.Controls.Add(this.lblAutoApprovedPurOrderAmount);
this.Controls.Add(this.txtAutoApprovedPurOrderAmount);

           //#####QueryGridColCustomize###Boolean
this.lblQueryGridColCustomize.AutoSize = true;
this.lblQueryGridColCustomize.Location = new System.Drawing.Point(100,400);
this.lblQueryGridColCustomize.Name = "lblQueryGridColCustomize";
this.lblQueryGridColCustomize.Size = new System.Drawing.Size(41, 12);
this.lblQueryGridColCustomize.TabIndex = 16;
this.lblQueryGridColCustomize.Text = "查询表格列自定义";
this.chkQueryGridColCustomize.Location = new System.Drawing.Point(173,396);
this.chkQueryGridColCustomize.Name = "chkQueryGridColCustomize";
this.chkQueryGridColCustomize.Size = new System.Drawing.Size(100, 21);
this.chkQueryGridColCustomize.TabIndex = 16;
this.Controls.Add(this.lblQueryGridColCustomize);
this.Controls.Add(this.chkQueryGridColCustomize);

           //#####BillGridColCustomize###Boolean
this.lblBillGridColCustomize.AutoSize = true;
this.lblBillGridColCustomize.Location = new System.Drawing.Point(100,425);
this.lblBillGridColCustomize.Name = "lblBillGridColCustomize";
this.lblBillGridColCustomize.Size = new System.Drawing.Size(41, 12);
this.lblBillGridColCustomize.TabIndex = 17;
this.lblBillGridColCustomize.Text = "单据表格列自定义";
this.chkBillGridColCustomize.Location = new System.Drawing.Point(173,421);
this.chkBillGridColCustomize.Name = "chkBillGridColCustomize";
this.chkBillGridColCustomize.Size = new System.Drawing.Size(100, 21);
this.chkBillGridColCustomize.TabIndex = 17;
this.Controls.Add(this.lblBillGridColCustomize);
this.Controls.Add(this.chkBillGridColCustomize);

           //#####IsDebug###Boolean
this.lblIsDebug.AutoSize = true;
this.lblIsDebug.Location = new System.Drawing.Point(100,450);
this.lblIsDebug.Name = "lblIsDebug";
this.lblIsDebug.Size = new System.Drawing.Size(41, 12);
this.lblIsDebug.TabIndex = 18;
this.lblIsDebug.Text = "调试模式";
this.chkIsDebug.Location = new System.Drawing.Point(173,446);
this.chkIsDebug.Name = "chkIsDebug";
this.chkIsDebug.Size = new System.Drawing.Size(100, 21);
this.chkIsDebug.TabIndex = 18;
this.Controls.Add(this.lblIsDebug);
this.Controls.Add(this.chkIsDebug);

           //#####EnableVoucherModule###Boolean
this.lblEnableVoucherModule.AutoSize = true;
this.lblEnableVoucherModule.Location = new System.Drawing.Point(100,475);
this.lblEnableVoucherModule.Name = "lblEnableVoucherModule";
this.lblEnableVoucherModule.Size = new System.Drawing.Size(41, 12);
this.lblEnableVoucherModule.TabIndex = 19;
this.lblEnableVoucherModule.Text = "启用凭证模块";
this.chkEnableVoucherModule.Location = new System.Drawing.Point(173,471);
this.chkEnableVoucherModule.Name = "chkEnableVoucherModule";
this.chkEnableVoucherModule.Size = new System.Drawing.Size(100, 21);
this.chkEnableVoucherModule.TabIndex = 19;
this.Controls.Add(this.lblEnableVoucherModule);
this.Controls.Add(this.chkEnableVoucherModule);

           //#####EnableContractModule###Boolean
this.lblEnableContractModule.AutoSize = true;
this.lblEnableContractModule.Location = new System.Drawing.Point(100,500);
this.lblEnableContractModule.Name = "lblEnableContractModule";
this.lblEnableContractModule.Size = new System.Drawing.Size(41, 12);
this.lblEnableContractModule.TabIndex = 20;
this.lblEnableContractModule.Text = "启用合同模块";
this.chkEnableContractModule.Location = new System.Drawing.Point(173,496);
this.chkEnableContractModule.Name = "chkEnableContractModule";
this.chkEnableContractModule.Size = new System.Drawing.Size(100, 21);
this.chkEnableContractModule.TabIndex = 20;
this.Controls.Add(this.lblEnableContractModule);
this.Controls.Add(this.chkEnableContractModule);

           //#####EnableInvoiceModule###Boolean
this.lblEnableInvoiceModule.AutoSize = true;
this.lblEnableInvoiceModule.Location = new System.Drawing.Point(100,525);
this.lblEnableInvoiceModule.Name = "lblEnableInvoiceModule";
this.lblEnableInvoiceModule.Size = new System.Drawing.Size(41, 12);
this.lblEnableInvoiceModule.TabIndex = 21;
this.lblEnableInvoiceModule.Text = "启用发票模块";
this.chkEnableInvoiceModule.Location = new System.Drawing.Point(173,521);
this.chkEnableInvoiceModule.Name = "chkEnableInvoiceModule";
this.chkEnableInvoiceModule.Size = new System.Drawing.Size(100, 21);
this.chkEnableInvoiceModule.TabIndex = 21;
this.Controls.Add(this.lblEnableInvoiceModule);
this.Controls.Add(this.chkEnableInvoiceModule);

           //#####EnableMultiCurrency###Boolean
this.lblEnableMultiCurrency.AutoSize = true;
this.lblEnableMultiCurrency.Location = new System.Drawing.Point(100,550);
this.lblEnableMultiCurrency.Name = "lblEnableMultiCurrency";
this.lblEnableMultiCurrency.Size = new System.Drawing.Size(41, 12);
this.lblEnableMultiCurrency.TabIndex = 22;
this.lblEnableMultiCurrency.Text = "启用多币种";
this.chkEnableMultiCurrency.Location = new System.Drawing.Point(173,546);
this.chkEnableMultiCurrency.Name = "chkEnableMultiCurrency";
this.chkEnableMultiCurrency.Size = new System.Drawing.Size(100, 21);
this.chkEnableMultiCurrency.TabIndex = 22;
this.Controls.Add(this.lblEnableMultiCurrency);
this.Controls.Add(this.chkEnableMultiCurrency);

           //#####EnableFinancialModule###Boolean
this.lblEnableFinancialModule.AutoSize = true;
this.lblEnableFinancialModule.Location = new System.Drawing.Point(100,575);
this.lblEnableFinancialModule.Name = "lblEnableFinancialModule";
this.lblEnableFinancialModule.Size = new System.Drawing.Size(41, 12);
this.lblEnableFinancialModule.TabIndex = 23;
this.lblEnableFinancialModule.Text = "启用财务模块";
this.chkEnableFinancialModule.Location = new System.Drawing.Point(173,571);
this.chkEnableFinancialModule.Name = "chkEnableFinancialModule";
this.chkEnableFinancialModule.Size = new System.Drawing.Size(100, 21);
this.chkEnableFinancialModule.TabIndex = 23;
this.Controls.Add(this.lblEnableFinancialModule);
this.Controls.Add(this.chkEnableFinancialModule);

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
           // this.kryptonPanel1.TabIndex = 23;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblQtyDataPrecision );
this.Controls.Add(this.txtQtyDataPrecision );

                this.Controls.Add(this.lblTaxRateDataPrecision );
this.Controls.Add(this.txtTaxRateDataPrecision );

                this.Controls.Add(this.lblMoneyDataPrecision );
this.Controls.Add(this.txtMoneyDataPrecision );

                this.Controls.Add(this.lblCheckNegativeInventory );
this.Controls.Add(this.chkCheckNegativeInventory );

                this.Controls.Add(this.lblCostCalculationMethod );
this.Controls.Add(this.txtCostCalculationMethod );

                this.Controls.Add(this.lblShowDebugInfo );
this.Controls.Add(this.chkShowDebugInfo );

                this.Controls.Add(this.lblOwnershipControl );
this.Controls.Add(this.chkOwnershipControl );

                this.Controls.Add(this.lblSaleBizLimited );
this.Controls.Add(this.chkSaleBizLimited );

                this.Controls.Add(this.lblDepartBizLimited );
this.Controls.Add(this.chkDepartBizLimited );

                this.Controls.Add(this.lblPurchsaeBizLimited );
this.Controls.Add(this.chkPurchsaeBizLimited );

                this.Controls.Add(this.lblCurrencyDataPrecisionAutoAddZero );
this.Controls.Add(this.chkCurrencyDataPrecisionAutoAddZero );

                this.Controls.Add(this.lblUseBarCode );
this.Controls.Add(this.chkUseBarCode );

                this.Controls.Add(this.lblQueryPageLayoutCustomize );
this.Controls.Add(this.chkQueryPageLayoutCustomize );

                this.Controls.Add(this.lblAutoApprovedSaleOrderAmount );
this.Controls.Add(this.txtAutoApprovedSaleOrderAmount );

                this.Controls.Add(this.lblAutoApprovedPurOrderAmount );
this.Controls.Add(this.txtAutoApprovedPurOrderAmount );

                this.Controls.Add(this.lblQueryGridColCustomize );
this.Controls.Add(this.chkQueryGridColCustomize );

                this.Controls.Add(this.lblBillGridColCustomize );
this.Controls.Add(this.chkBillGridColCustomize );

                this.Controls.Add(this.lblIsDebug );
this.Controls.Add(this.chkIsDebug );

                this.Controls.Add(this.lblEnableVoucherModule );
this.Controls.Add(this.chkEnableVoucherModule );

                this.Controls.Add(this.lblEnableContractModule );
this.Controls.Add(this.chkEnableContractModule );

                this.Controls.Add(this.lblEnableInvoiceModule );
this.Controls.Add(this.chkEnableInvoiceModule );

                this.Controls.Add(this.lblEnableMultiCurrency );
this.Controls.Add(this.chkEnableMultiCurrency );

                this.Controls.Add(this.lblEnableFinancialModule );
this.Controls.Add(this.chkEnableFinancialModule );

                            // 
            // "tb_SystemConfigEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_SystemConfigEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblQtyDataPrecision;
private Krypton.Toolkit.KryptonTextBox txtQtyDataPrecision;

    
        
              private Krypton.Toolkit.KryptonLabel lblTaxRateDataPrecision;
private Krypton.Toolkit.KryptonTextBox txtTaxRateDataPrecision;

    
        
              private Krypton.Toolkit.KryptonLabel lblMoneyDataPrecision;
private Krypton.Toolkit.KryptonTextBox txtMoneyDataPrecision;

    
        
              private Krypton.Toolkit.KryptonLabel lblCheckNegativeInventory;
private Krypton.Toolkit.KryptonCheckBox chkCheckNegativeInventory;

    
        
              private Krypton.Toolkit.KryptonLabel lblCostCalculationMethod;
private Krypton.Toolkit.KryptonTextBox txtCostCalculationMethod;

    
        
              private Krypton.Toolkit.KryptonLabel lblShowDebugInfo;
private Krypton.Toolkit.KryptonCheckBox chkShowDebugInfo;

    
        
              private Krypton.Toolkit.KryptonLabel lblOwnershipControl;
private Krypton.Toolkit.KryptonCheckBox chkOwnershipControl;

    
        
              private Krypton.Toolkit.KryptonLabel lblSaleBizLimited;
private Krypton.Toolkit.KryptonCheckBox chkSaleBizLimited;

    
        
              private Krypton.Toolkit.KryptonLabel lblDepartBizLimited;
private Krypton.Toolkit.KryptonCheckBox chkDepartBizLimited;

    
        
              private Krypton.Toolkit.KryptonLabel lblPurchsaeBizLimited;
private Krypton.Toolkit.KryptonCheckBox chkPurchsaeBizLimited;

    
        
              private Krypton.Toolkit.KryptonLabel lblCurrencyDataPrecisionAutoAddZero;
private Krypton.Toolkit.KryptonCheckBox chkCurrencyDataPrecisionAutoAddZero;

    
        
              private Krypton.Toolkit.KryptonLabel lblUseBarCode;
private Krypton.Toolkit.KryptonCheckBox chkUseBarCode;

    
        
              private Krypton.Toolkit.KryptonLabel lblQueryPageLayoutCustomize;
private Krypton.Toolkit.KryptonCheckBox chkQueryPageLayoutCustomize;

    
        
              private Krypton.Toolkit.KryptonLabel lblAutoApprovedSaleOrderAmount;
private Krypton.Toolkit.KryptonTextBox txtAutoApprovedSaleOrderAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblAutoApprovedPurOrderAmount;
private Krypton.Toolkit.KryptonTextBox txtAutoApprovedPurOrderAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblQueryGridColCustomize;
private Krypton.Toolkit.KryptonCheckBox chkQueryGridColCustomize;

    
        
              private Krypton.Toolkit.KryptonLabel lblBillGridColCustomize;
private Krypton.Toolkit.KryptonCheckBox chkBillGridColCustomize;

    
        
              private Krypton.Toolkit.KryptonLabel lblIsDebug;
private Krypton.Toolkit.KryptonCheckBox chkIsDebug;

    
        
              private Krypton.Toolkit.KryptonLabel lblEnableVoucherModule;
private Krypton.Toolkit.KryptonCheckBox chkEnableVoucherModule;

    
        
              private Krypton.Toolkit.KryptonLabel lblEnableContractModule;
private Krypton.Toolkit.KryptonCheckBox chkEnableContractModule;

    
        
              private Krypton.Toolkit.KryptonLabel lblEnableInvoiceModule;
private Krypton.Toolkit.KryptonCheckBox chkEnableInvoiceModule;

    
        
              private Krypton.Toolkit.KryptonLabel lblEnableMultiCurrency;
private Krypton.Toolkit.KryptonCheckBox chkEnableMultiCurrency;

    
        
              private Krypton.Toolkit.KryptonLabel lblEnableFinancialModule;
private Krypton.Toolkit.KryptonCheckBox chkEnableFinancialModule;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

