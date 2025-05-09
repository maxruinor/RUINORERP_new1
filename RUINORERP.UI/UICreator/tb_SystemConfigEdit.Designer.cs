// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：05/08/2025 09:57:44
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
            this.lblQtyDataPrecision = new Krypton.Toolkit.KryptonLabel();
            this.txtQtyDataPrecision = new Krypton.Toolkit.KryptonTextBox();
            this.lblTaxRateDataPrecision = new Krypton.Toolkit.KryptonLabel();
            this.txtTaxRateDataPrecision = new Krypton.Toolkit.KryptonTextBox();
            this.lblMoneyDataPrecision = new Krypton.Toolkit.KryptonLabel();
            this.txtMoneyDataPrecision = new Krypton.Toolkit.KryptonTextBox();
            this.lblCheckNegativeInventory = new Krypton.Toolkit.KryptonLabel();
            this.chkCheckNegativeInventory = new Krypton.Toolkit.KryptonCheckBox();
            this.lblCostCalculationMethod = new Krypton.Toolkit.KryptonLabel();
            this.txtCostCalculationMethod = new Krypton.Toolkit.KryptonTextBox();
            this.lblShowDebugInfo = new Krypton.Toolkit.KryptonLabel();
            this.chkShowDebugInfo = new Krypton.Toolkit.KryptonCheckBox();
            this.lblOwnershipControl = new Krypton.Toolkit.KryptonLabel();
            this.chkOwnershipControl = new Krypton.Toolkit.KryptonCheckBox();
            this.lblSaleBizLimited = new Krypton.Toolkit.KryptonLabel();
            this.chkSaleBizLimited = new Krypton.Toolkit.KryptonCheckBox();
            this.lblDepartBizLimited = new Krypton.Toolkit.KryptonLabel();
            this.chkDepartBizLimited = new Krypton.Toolkit.KryptonCheckBox();
            this.lblPurchsaeBizLimited = new Krypton.Toolkit.KryptonLabel();
            this.chkPurchsaeBizLimited = new Krypton.Toolkit.KryptonCheckBox();
            this.lblCurrencyDataPrecisionAutoAddZero = new Krypton.Toolkit.KryptonLabel();
            this.chkCurrencyDataPrecisionAutoAddZero = new Krypton.Toolkit.KryptonCheckBox();
            this.lblUseBarCode = new Krypton.Toolkit.KryptonLabel();
            this.chkUseBarCode = new Krypton.Toolkit.KryptonCheckBox();
            this.lblQueryPageLayoutCustomize = new Krypton.Toolkit.KryptonLabel();
            this.chkQueryPageLayoutCustomize = new Krypton.Toolkit.KryptonCheckBox();
            this.lblAutoApprovedSaleOrderAmount = new Krypton.Toolkit.KryptonLabel();
            this.txtAutoApprovedSaleOrderAmount = new Krypton.Toolkit.KryptonTextBox();
            this.lblAutoApprovedPurOrderAmount = new Krypton.Toolkit.KryptonLabel();
            this.txtAutoApprovedPurOrderAmount = new Krypton.Toolkit.KryptonTextBox();
            this.lblQueryGridColCustomize = new Krypton.Toolkit.KryptonLabel();
            this.chkQueryGridColCustomize = new Krypton.Toolkit.KryptonCheckBox();
            this.lblBillGridColCustomize = new Krypton.Toolkit.KryptonLabel();
            this.chkBillGridColCustomize = new Krypton.Toolkit.KryptonCheckBox();
            this.lblIsDebug = new Krypton.Toolkit.KryptonLabel();
            this.chkIsDebug = new Krypton.Toolkit.KryptonCheckBox();
            this.SuspendLayout();
            // 
            // lblQtyDataPrecision
            // 
            this.lblQtyDataPrecision.Location = new System.Drawing.Point(105, 39);
            this.lblQtyDataPrecision.Name = "lblQtyDataPrecision";
            this.lblQtyDataPrecision.Size = new System.Drawing.Size(62, 20);
            this.lblQtyDataPrecision.TabIndex = 1;
            this.lblQtyDataPrecision.Values.Text = "数量精度";
            // 
            // txtQtyDataPrecision
            // 
            this.txtQtyDataPrecision.Location = new System.Drawing.Point(178, 35);
            this.txtQtyDataPrecision.Name = "txtQtyDataPrecision";
            this.txtQtyDataPrecision.Size = new System.Drawing.Size(100, 23);
            this.txtQtyDataPrecision.TabIndex = 1;
            // 
            // lblTaxRateDataPrecision
            // 
            this.lblTaxRateDataPrecision.Location = new System.Drawing.Point(105, 64);
            this.lblTaxRateDataPrecision.Name = "lblTaxRateDataPrecision";
            this.lblTaxRateDataPrecision.Size = new System.Drawing.Size(62, 20);
            this.lblTaxRateDataPrecision.TabIndex = 2;
            this.lblTaxRateDataPrecision.Values.Text = "税率精度";
            // 
            // txtTaxRateDataPrecision
            // 
            this.txtTaxRateDataPrecision.Location = new System.Drawing.Point(178, 60);
            this.txtTaxRateDataPrecision.Name = "txtTaxRateDataPrecision";
            this.txtTaxRateDataPrecision.Size = new System.Drawing.Size(100, 23);
            this.txtTaxRateDataPrecision.TabIndex = 2;
            // 
            // lblMoneyDataPrecision
            // 
            this.lblMoneyDataPrecision.Location = new System.Drawing.Point(105, 89);
            this.lblMoneyDataPrecision.Name = "lblMoneyDataPrecision";
            this.lblMoneyDataPrecision.Size = new System.Drawing.Size(62, 20);
            this.lblMoneyDataPrecision.TabIndex = 3;
            this.lblMoneyDataPrecision.Values.Text = "金额精度";
            // 
            // txtMoneyDataPrecision
            // 
            this.txtMoneyDataPrecision.Location = new System.Drawing.Point(178, 85);
            this.txtMoneyDataPrecision.Name = "txtMoneyDataPrecision";
            this.txtMoneyDataPrecision.Size = new System.Drawing.Size(100, 23);
            this.txtMoneyDataPrecision.TabIndex = 3;
            // 
            // lblCheckNegativeInventory
            // 
            this.lblCheckNegativeInventory.Location = new System.Drawing.Point(105, 114);
            this.lblCheckNegativeInventory.Name = "lblCheckNegativeInventory";
            this.lblCheckNegativeInventory.Size = new System.Drawing.Size(75, 20);
            this.lblCheckNegativeInventory.TabIndex = 4;
            this.lblCheckNegativeInventory.Values.Text = "允许负库存";
            // 
            // chkCheckNegativeInventory
            // 
            this.chkCheckNegativeInventory.Checked = true;
            this.chkCheckNegativeInventory.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCheckNegativeInventory.Location = new System.Drawing.Point(178, 110);
            this.chkCheckNegativeInventory.Name = "chkCheckNegativeInventory";
            this.chkCheckNegativeInventory.Size = new System.Drawing.Size(19, 13);
            this.chkCheckNegativeInventory.TabIndex = 4;
            this.chkCheckNegativeInventory.Values.Text = "";
            // 
            // lblCostCalculationMethod
            // 
            this.lblCostCalculationMethod.Location = new System.Drawing.Point(105, 139);
            this.lblCostCalculationMethod.Name = "lblCostCalculationMethod";
            this.lblCostCalculationMethod.Size = new System.Drawing.Size(62, 20);
            this.lblCostCalculationMethod.TabIndex = 5;
            this.lblCostCalculationMethod.Values.Text = "成本方式";
            // 
            // txtCostCalculationMethod
            // 
            this.txtCostCalculationMethod.Location = new System.Drawing.Point(178, 135);
            this.txtCostCalculationMethod.Name = "txtCostCalculationMethod";
            this.txtCostCalculationMethod.Size = new System.Drawing.Size(100, 23);
            this.txtCostCalculationMethod.TabIndex = 5;
            // 
            // lblShowDebugInfo
            // 
            this.lblShowDebugInfo.Location = new System.Drawing.Point(439, 186);
            this.lblShowDebugInfo.Name = "lblShowDebugInfo";
            this.lblShowDebugInfo.Size = new System.Drawing.Size(88, 20);
            this.lblShowDebugInfo.TabIndex = 6;
            this.lblShowDebugInfo.Values.Text = "显示调试信息";
            // 
            // chkShowDebugInfo
            // 
            this.chkShowDebugInfo.Location = new System.Drawing.Point(587, 182);
            this.chkShowDebugInfo.Name = "chkShowDebugInfo";
            this.chkShowDebugInfo.Size = new System.Drawing.Size(19, 13);
            this.chkShowDebugInfo.TabIndex = 6;
            this.chkShowDebugInfo.Values.Text = "";
            // 
            // lblOwnershipControl
            // 
            this.lblOwnershipControl.Location = new System.Drawing.Point(439, 211);
            this.lblOwnershipControl.Name = "lblOwnershipControl";
            this.lblOwnershipControl.Size = new System.Drawing.Size(88, 20);
            this.lblOwnershipControl.TabIndex = 7;
            this.lblOwnershipControl.Values.Text = "数据归属控制";
            // 
            // chkOwnershipControl
            // 
            this.chkOwnershipControl.Checked = true;
            this.chkOwnershipControl.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkOwnershipControl.Location = new System.Drawing.Point(587, 207);
            this.chkOwnershipControl.Name = "chkOwnershipControl";
            this.chkOwnershipControl.Size = new System.Drawing.Size(19, 13);
            this.chkOwnershipControl.TabIndex = 7;
            this.chkOwnershipControl.Values.Text = "";
            // 
            // lblSaleBizLimited
            // 
            this.lblSaleBizLimited.Location = new System.Drawing.Point(439, 236);
            this.lblSaleBizLimited.Name = "lblSaleBizLimited";
            this.lblSaleBizLimited.Size = new System.Drawing.Size(114, 20);
            this.lblSaleBizLimited.TabIndex = 8;
            this.lblSaleBizLimited.Values.Text = "销售业务范围限制";
            // 
            // chkSaleBizLimited
            // 
            this.chkSaleBizLimited.Location = new System.Drawing.Point(587, 232);
            this.chkSaleBizLimited.Name = "chkSaleBizLimited";
            this.chkSaleBizLimited.Size = new System.Drawing.Size(19, 13);
            this.chkSaleBizLimited.TabIndex = 8;
            this.chkSaleBizLimited.Values.Text = "";
            // 
            // lblDepartBizLimited
            // 
            this.lblDepartBizLimited.Location = new System.Drawing.Point(439, 261);
            this.lblDepartBizLimited.Name = "lblDepartBizLimited";
            this.lblDepartBizLimited.Size = new System.Drawing.Size(88, 20);
            this.lblDepartBizLimited.TabIndex = 9;
            this.lblDepartBizLimited.Values.Text = "部门范围限制";
            // 
            // chkDepartBizLimited
            // 
            this.chkDepartBizLimited.Location = new System.Drawing.Point(587, 257);
            this.chkDepartBizLimited.Name = "chkDepartBizLimited";
            this.chkDepartBizLimited.Size = new System.Drawing.Size(19, 13);
            this.chkDepartBizLimited.TabIndex = 9;
            this.chkDepartBizLimited.Values.Text = "";
            // 
            // lblPurchsaeBizLimited
            // 
            this.lblPurchsaeBizLimited.Location = new System.Drawing.Point(439, 286);
            this.lblPurchsaeBizLimited.Name = "lblPurchsaeBizLimited";
            this.lblPurchsaeBizLimited.Size = new System.Drawing.Size(114, 20);
            this.lblPurchsaeBizLimited.TabIndex = 10;
            this.lblPurchsaeBizLimited.Values.Text = "采购业务范围限制";
            // 
            // chkPurchsaeBizLimited
            // 
            this.chkPurchsaeBizLimited.Location = new System.Drawing.Point(587, 282);
            this.chkPurchsaeBizLimited.Name = "chkPurchsaeBizLimited";
            this.chkPurchsaeBizLimited.Size = new System.Drawing.Size(19, 13);
            this.chkPurchsaeBizLimited.TabIndex = 10;
            this.chkPurchsaeBizLimited.Values.Text = "";
            // 
            // lblCurrencyDataPrecisionAutoAddZero
            // 
            this.lblCurrencyDataPrecisionAutoAddZero.Location = new System.Drawing.Point(439, 311);
            this.lblCurrencyDataPrecisionAutoAddZero.Name = "lblCurrencyDataPrecisionAutoAddZero";
            this.lblCurrencyDataPrecisionAutoAddZero.Size = new System.Drawing.Size(114, 20);
            this.lblCurrencyDataPrecisionAutoAddZero.TabIndex = 11;
            this.lblCurrencyDataPrecisionAutoAddZero.Values.Text = "金额精度自动补零";
            // 
            // chkCurrencyDataPrecisionAutoAddZero
            // 
            this.chkCurrencyDataPrecisionAutoAddZero.Checked = true;
            this.chkCurrencyDataPrecisionAutoAddZero.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCurrencyDataPrecisionAutoAddZero.Location = new System.Drawing.Point(587, 307);
            this.chkCurrencyDataPrecisionAutoAddZero.Name = "chkCurrencyDataPrecisionAutoAddZero";
            this.chkCurrencyDataPrecisionAutoAddZero.Size = new System.Drawing.Size(19, 13);
            this.chkCurrencyDataPrecisionAutoAddZero.TabIndex = 11;
            this.chkCurrencyDataPrecisionAutoAddZero.Values.Text = "";
            // 
            // lblUseBarCode
            // 
            this.lblUseBarCode.Location = new System.Drawing.Point(439, 336);
            this.lblUseBarCode.Name = "lblUseBarCode";
            this.lblUseBarCode.Size = new System.Drawing.Size(88, 20);
            this.lblUseBarCode.TabIndex = 12;
            this.lblUseBarCode.Values.Text = "是否启用条码";
            // 
            // chkUseBarCode
            // 
            this.chkUseBarCode.Location = new System.Drawing.Point(587, 332);
            this.chkUseBarCode.Name = "chkUseBarCode";
            this.chkUseBarCode.Size = new System.Drawing.Size(19, 13);
            this.chkUseBarCode.TabIndex = 12;
            this.chkUseBarCode.Values.Text = "";
            // 
            // lblQueryPageLayoutCustomize
            // 
            this.lblQueryPageLayoutCustomize.Location = new System.Drawing.Point(439, 361);
            this.lblQueryPageLayoutCustomize.Name = "lblQueryPageLayoutCustomize";
            this.lblQueryPageLayoutCustomize.Size = new System.Drawing.Size(114, 20);
            this.lblQueryPageLayoutCustomize.TabIndex = 13;
            this.lblQueryPageLayoutCustomize.Values.Text = "查询页布局自定义";
            // 
            // chkQueryPageLayoutCustomize
            // 
            this.chkQueryPageLayoutCustomize.Location = new System.Drawing.Point(587, 357);
            this.chkQueryPageLayoutCustomize.Name = "chkQueryPageLayoutCustomize";
            this.chkQueryPageLayoutCustomize.Size = new System.Drawing.Size(19, 13);
            this.chkQueryPageLayoutCustomize.TabIndex = 13;
            this.chkQueryPageLayoutCustomize.Values.Text = "";
            // 
            // lblAutoApprovedSaleOrderAmount
            // 
            this.lblAutoApprovedSaleOrderAmount.Location = new System.Drawing.Point(439, 386);
            this.lblAutoApprovedSaleOrderAmount.Name = "lblAutoApprovedSaleOrderAmount";
            this.lblAutoApprovedSaleOrderAmount.Size = new System.Drawing.Size(140, 20);
            this.lblAutoApprovedSaleOrderAmount.TabIndex = 14;
            this.lblAutoApprovedSaleOrderAmount.Values.Text = "自动审核销售订单金额";
            // 
            // txtAutoApprovedSaleOrderAmount
            // 
            this.txtAutoApprovedSaleOrderAmount.Location = new System.Drawing.Point(587, 382);
            this.txtAutoApprovedSaleOrderAmount.Name = "txtAutoApprovedSaleOrderAmount";
            this.txtAutoApprovedSaleOrderAmount.Size = new System.Drawing.Size(100, 23);
            this.txtAutoApprovedSaleOrderAmount.TabIndex = 14;
            // 
            // lblAutoApprovedPurOrderAmount
            // 
            this.lblAutoApprovedPurOrderAmount.Location = new System.Drawing.Point(439, 411);
            this.lblAutoApprovedPurOrderAmount.Name = "lblAutoApprovedPurOrderAmount";
            this.lblAutoApprovedPurOrderAmount.Size = new System.Drawing.Size(140, 20);
            this.lblAutoApprovedPurOrderAmount.TabIndex = 15;
            this.lblAutoApprovedPurOrderAmount.Values.Text = "自动审核采购订单金额";
            // 
            // txtAutoApprovedPurOrderAmount
            // 
            this.txtAutoApprovedPurOrderAmount.Location = new System.Drawing.Point(587, 407);
            this.txtAutoApprovedPurOrderAmount.Name = "txtAutoApprovedPurOrderAmount";
            this.txtAutoApprovedPurOrderAmount.Size = new System.Drawing.Size(100, 23);
            this.txtAutoApprovedPurOrderAmount.TabIndex = 15;
            // 
            // lblQueryGridColCustomize
            // 
            this.lblQueryGridColCustomize.Location = new System.Drawing.Point(439, 436);
            this.lblQueryGridColCustomize.Name = "lblQueryGridColCustomize";
            this.lblQueryGridColCustomize.Size = new System.Drawing.Size(114, 20);
            this.lblQueryGridColCustomize.TabIndex = 16;
            this.lblQueryGridColCustomize.Values.Text = "查询表格列自定义";
            // 
            // chkQueryGridColCustomize
            // 
            this.chkQueryGridColCustomize.Location = new System.Drawing.Point(587, 432);
            this.chkQueryGridColCustomize.Name = "chkQueryGridColCustomize";
            this.chkQueryGridColCustomize.Size = new System.Drawing.Size(19, 13);
            this.chkQueryGridColCustomize.TabIndex = 16;
            this.chkQueryGridColCustomize.Values.Text = "";
            // 
            // lblBillGridColCustomize
            // 
            this.lblBillGridColCustomize.Location = new System.Drawing.Point(439, 461);
            this.lblBillGridColCustomize.Name = "lblBillGridColCustomize";
            this.lblBillGridColCustomize.Size = new System.Drawing.Size(114, 20);
            this.lblBillGridColCustomize.TabIndex = 17;
            this.lblBillGridColCustomize.Values.Text = "单据表格列自定义";
            // 
            // chkBillGridColCustomize
            // 
            this.chkBillGridColCustomize.Location = new System.Drawing.Point(587, 457);
            this.chkBillGridColCustomize.Name = "chkBillGridColCustomize";
            this.chkBillGridColCustomize.Size = new System.Drawing.Size(19, 13);
            this.chkBillGridColCustomize.TabIndex = 17;
            this.chkBillGridColCustomize.Values.Text = "";
            // 
            // lblIsDebug
            // 
            this.lblIsDebug.Location = new System.Drawing.Point(439, 486);
            this.lblIsDebug.Name = "lblIsDebug";
            this.lblIsDebug.Size = new System.Drawing.Size(62, 20);
            this.lblIsDebug.TabIndex = 18;
            this.lblIsDebug.Values.Text = "调试模式";
            // 
            // chkIsDebug
            // 
            this.chkIsDebug.Location = new System.Drawing.Point(587, 482);
            this.chkIsDebug.Name = "chkIsDebug";
            this.chkIsDebug.Size = new System.Drawing.Size(19, 13);
            this.chkIsDebug.TabIndex = 18;
            this.chkIsDebug.Values.Text = "";
            // 
            // tb_SystemConfigEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblQtyDataPrecision);
            this.Controls.Add(this.txtQtyDataPrecision);
            this.Controls.Add(this.lblTaxRateDataPrecision);
            this.Controls.Add(this.txtTaxRateDataPrecision);
            this.Controls.Add(this.lblMoneyDataPrecision);
            this.Controls.Add(this.txtMoneyDataPrecision);
            this.Controls.Add(this.lblCheckNegativeInventory);
            this.Controls.Add(this.chkCheckNegativeInventory);
            this.Controls.Add(this.lblCostCalculationMethod);
            this.Controls.Add(this.txtCostCalculationMethod);
            this.Controls.Add(this.lblShowDebugInfo);
            this.Controls.Add(this.chkShowDebugInfo);
            this.Controls.Add(this.lblOwnershipControl);
            this.Controls.Add(this.chkOwnershipControl);
            this.Controls.Add(this.lblSaleBizLimited);
            this.Controls.Add(this.chkSaleBizLimited);
            this.Controls.Add(this.lblDepartBizLimited);
            this.Controls.Add(this.chkDepartBizLimited);
            this.Controls.Add(this.lblPurchsaeBizLimited);
            this.Controls.Add(this.chkPurchsaeBizLimited);
            this.Controls.Add(this.lblCurrencyDataPrecisionAutoAddZero);
            this.Controls.Add(this.chkCurrencyDataPrecisionAutoAddZero);
            this.Controls.Add(this.lblUseBarCode);
            this.Controls.Add(this.chkUseBarCode);
            this.Controls.Add(this.lblQueryPageLayoutCustomize);
            this.Controls.Add(this.chkQueryPageLayoutCustomize);
            this.Controls.Add(this.lblAutoApprovedSaleOrderAmount);
            this.Controls.Add(this.txtAutoApprovedSaleOrderAmount);
            this.Controls.Add(this.lblAutoApprovedPurOrderAmount);
            this.Controls.Add(this.txtAutoApprovedPurOrderAmount);
            this.Controls.Add(this.lblQueryGridColCustomize);
            this.Controls.Add(this.chkQueryGridColCustomize);
            this.Controls.Add(this.lblBillGridColCustomize);
            this.Controls.Add(this.chkBillGridColCustomize);
            this.Controls.Add(this.lblIsDebug);
            this.Controls.Add(this.chkIsDebug);
            this.Name = "tb_SystemConfigEdit";
            this.Size = new System.Drawing.Size(911, 723);
            this.ResumeLayout(false);
            this.PerformLayout();

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






    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

