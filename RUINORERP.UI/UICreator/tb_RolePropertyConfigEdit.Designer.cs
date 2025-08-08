// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:11
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 角色属性配置不同角色权限功能等不一样
    /// </summary>
    partial class tb_RolePropertyConfigEdit
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
     this.lblRolePropertyName = new Krypton.Toolkit.KryptonLabel();
this.txtRolePropertyName = new Krypton.Toolkit.KryptonTextBox();
this.txtRolePropertyName.Multiline = true;

this.lblQtyDataPrecision = new Krypton.Toolkit.KryptonLabel();
this.txtQtyDataPrecision = new Krypton.Toolkit.KryptonTextBox();

this.lblTaxRateDataPrecision = new Krypton.Toolkit.KryptonLabel();
this.txtTaxRateDataPrecision = new Krypton.Toolkit.KryptonTextBox();

this.lblMoneyDataPrecision = new Krypton.Toolkit.KryptonLabel();
this.txtMoneyDataPrecision = new Krypton.Toolkit.KryptonTextBox();

this.lblCurrencyDataPrecisionAutoAddZero = new Krypton.Toolkit.KryptonLabel();
this.chkCurrencyDataPrecisionAutoAddZero = new Krypton.Toolkit.KryptonCheckBox();
this.chkCurrencyDataPrecisionAutoAddZero.Values.Text ="";
this.chkCurrencyDataPrecisionAutoAddZero.Checked = true;
this.chkCurrencyDataPrecisionAutoAddZero.CheckState = System.Windows.Forms.CheckState.Checked;

this.lblCostCalculationMethod = new Krypton.Toolkit.KryptonLabel();
this.txtCostCalculationMethod = new Krypton.Toolkit.KryptonTextBox();

this.lblShowDebugInfo = new Krypton.Toolkit.KryptonLabel();
this.chkShowDebugInfo = new Krypton.Toolkit.KryptonCheckBox();
this.chkShowDebugInfo.Values.Text ="";

this.lblOwnershipControl = new Krypton.Toolkit.KryptonLabel();
this.chkOwnershipControl = new Krypton.Toolkit.KryptonCheckBox();
this.chkOwnershipControl.Values.Text ="";

this.lblSaleBizLimited = new Krypton.Toolkit.KryptonLabel();
this.chkSaleBizLimited = new Krypton.Toolkit.KryptonCheckBox();
this.chkSaleBizLimited.Values.Text ="";

this.lblDepartBizLimited = new Krypton.Toolkit.KryptonLabel();
this.chkDepartBizLimited = new Krypton.Toolkit.KryptonCheckBox();
this.chkDepartBizLimited.Values.Text ="";

this.lblPurchsaeBizLimited = new Krypton.Toolkit.KryptonLabel();
this.chkPurchsaeBizLimited = new Krypton.Toolkit.KryptonCheckBox();
this.chkPurchsaeBizLimited.Values.Text ="";

this.lblQueryPageLayoutCustomize = new Krypton.Toolkit.KryptonLabel();
this.chkQueryPageLayoutCustomize = new Krypton.Toolkit.KryptonCheckBox();
this.chkQueryPageLayoutCustomize.Values.Text ="";

this.lblQueryGridColCustomize = new Krypton.Toolkit.KryptonLabel();
this.chkQueryGridColCustomize = new Krypton.Toolkit.KryptonCheckBox();
this.chkQueryGridColCustomize.Values.Text ="";

this.lblBillGridColCustomize = new Krypton.Toolkit.KryptonLabel();
this.chkBillGridColCustomize = new Krypton.Toolkit.KryptonCheckBox();
this.chkBillGridColCustomize.Values.Text ="";

this.lblExclusiveLimited = new Krypton.Toolkit.KryptonLabel();
this.chkExclusiveLimited = new Krypton.Toolkit.KryptonCheckBox();
this.chkExclusiveLimited.Values.Text ="";

this.lblDataBoardUnits = new Krypton.Toolkit.KryptonLabel();
this.txtDataBoardUnits = new Krypton.Toolkit.KryptonTextBox();
this.txtDataBoardUnits.Multiline = true;

    
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
     
            //#####255RolePropertyName###String
this.lblRolePropertyName.AutoSize = true;
this.lblRolePropertyName.Location = new System.Drawing.Point(100,25);
this.lblRolePropertyName.Name = "lblRolePropertyName";
this.lblRolePropertyName.Size = new System.Drawing.Size(41, 12);
this.lblRolePropertyName.TabIndex = 1;
this.lblRolePropertyName.Text = "角色名称";
this.txtRolePropertyName.Location = new System.Drawing.Point(173,21);
this.txtRolePropertyName.Name = "txtRolePropertyName";
this.txtRolePropertyName.Size = new System.Drawing.Size(100, 21);
this.txtRolePropertyName.TabIndex = 1;
this.Controls.Add(this.lblRolePropertyName);
this.Controls.Add(this.txtRolePropertyName);

           //#####QtyDataPrecision###Int32
this.lblQtyDataPrecision.AutoSize = true;
this.lblQtyDataPrecision.Location = new System.Drawing.Point(100,50);
this.lblQtyDataPrecision.Name = "lblQtyDataPrecision";
this.lblQtyDataPrecision.Size = new System.Drawing.Size(41, 12);
this.lblQtyDataPrecision.TabIndex = 2;
this.lblQtyDataPrecision.Text = "数量精度";
this.txtQtyDataPrecision.Location = new System.Drawing.Point(173,46);
this.txtQtyDataPrecision.Name = "txtQtyDataPrecision";
this.txtQtyDataPrecision.Size = new System.Drawing.Size(100, 21);
this.txtQtyDataPrecision.TabIndex = 2;
this.Controls.Add(this.lblQtyDataPrecision);
this.Controls.Add(this.txtQtyDataPrecision);

           //#####TaxRateDataPrecision###Int32
this.lblTaxRateDataPrecision.AutoSize = true;
this.lblTaxRateDataPrecision.Location = new System.Drawing.Point(100,75);
this.lblTaxRateDataPrecision.Name = "lblTaxRateDataPrecision";
this.lblTaxRateDataPrecision.Size = new System.Drawing.Size(41, 12);
this.lblTaxRateDataPrecision.TabIndex = 3;
this.lblTaxRateDataPrecision.Text = "税率精度";
this.txtTaxRateDataPrecision.Location = new System.Drawing.Point(173,71);
this.txtTaxRateDataPrecision.Name = "txtTaxRateDataPrecision";
this.txtTaxRateDataPrecision.Size = new System.Drawing.Size(100, 21);
this.txtTaxRateDataPrecision.TabIndex = 3;
this.Controls.Add(this.lblTaxRateDataPrecision);
this.Controls.Add(this.txtTaxRateDataPrecision);

           //#####MoneyDataPrecision###Int32
this.lblMoneyDataPrecision.AutoSize = true;
this.lblMoneyDataPrecision.Location = new System.Drawing.Point(100,100);
this.lblMoneyDataPrecision.Name = "lblMoneyDataPrecision";
this.lblMoneyDataPrecision.Size = new System.Drawing.Size(41, 12);
this.lblMoneyDataPrecision.TabIndex = 4;
this.lblMoneyDataPrecision.Text = "金额精度";
this.txtMoneyDataPrecision.Location = new System.Drawing.Point(173,96);
this.txtMoneyDataPrecision.Name = "txtMoneyDataPrecision";
this.txtMoneyDataPrecision.Size = new System.Drawing.Size(100, 21);
this.txtMoneyDataPrecision.TabIndex = 4;
this.Controls.Add(this.lblMoneyDataPrecision);
this.Controls.Add(this.txtMoneyDataPrecision);

           //#####CurrencyDataPrecisionAutoAddZero###Boolean
this.lblCurrencyDataPrecisionAutoAddZero.AutoSize = true;
this.lblCurrencyDataPrecisionAutoAddZero.Location = new System.Drawing.Point(100,125);
this.lblCurrencyDataPrecisionAutoAddZero.Name = "lblCurrencyDataPrecisionAutoAddZero";
this.lblCurrencyDataPrecisionAutoAddZero.Size = new System.Drawing.Size(41, 12);
this.lblCurrencyDataPrecisionAutoAddZero.TabIndex = 5;
this.lblCurrencyDataPrecisionAutoAddZero.Text = "金额精度自动补零";
this.chkCurrencyDataPrecisionAutoAddZero.Location = new System.Drawing.Point(173,121);
this.chkCurrencyDataPrecisionAutoAddZero.Name = "chkCurrencyDataPrecisionAutoAddZero";
this.chkCurrencyDataPrecisionAutoAddZero.Size = new System.Drawing.Size(100, 21);
this.chkCurrencyDataPrecisionAutoAddZero.TabIndex = 5;
this.Controls.Add(this.lblCurrencyDataPrecisionAutoAddZero);
this.Controls.Add(this.chkCurrencyDataPrecisionAutoAddZero);

           //#####CostCalculationMethod###Int32
this.lblCostCalculationMethod.AutoSize = true;
this.lblCostCalculationMethod.Location = new System.Drawing.Point(100,150);
this.lblCostCalculationMethod.Name = "lblCostCalculationMethod";
this.lblCostCalculationMethod.Size = new System.Drawing.Size(41, 12);
this.lblCostCalculationMethod.TabIndex = 6;
this.lblCostCalculationMethod.Text = "成本方式";
this.txtCostCalculationMethod.Location = new System.Drawing.Point(173,146);
this.txtCostCalculationMethod.Name = "txtCostCalculationMethod";
this.txtCostCalculationMethod.Size = new System.Drawing.Size(100, 21);
this.txtCostCalculationMethod.TabIndex = 6;
this.Controls.Add(this.lblCostCalculationMethod);
this.Controls.Add(this.txtCostCalculationMethod);

           //#####ShowDebugInfo###Boolean
this.lblShowDebugInfo.AutoSize = true;
this.lblShowDebugInfo.Location = new System.Drawing.Point(100,175);
this.lblShowDebugInfo.Name = "lblShowDebugInfo";
this.lblShowDebugInfo.Size = new System.Drawing.Size(41, 12);
this.lblShowDebugInfo.TabIndex = 7;
this.lblShowDebugInfo.Text = "调试信息";
this.chkShowDebugInfo.Location = new System.Drawing.Point(173,171);
this.chkShowDebugInfo.Name = "chkShowDebugInfo";
this.chkShowDebugInfo.Size = new System.Drawing.Size(100, 21);
this.chkShowDebugInfo.TabIndex = 7;
this.Controls.Add(this.lblShowDebugInfo);
this.Controls.Add(this.chkShowDebugInfo);

           //#####OwnershipControl###Boolean
this.lblOwnershipControl.AutoSize = true;
this.lblOwnershipControl.Location = new System.Drawing.Point(100,200);
this.lblOwnershipControl.Name = "lblOwnershipControl";
this.lblOwnershipControl.Size = new System.Drawing.Size(41, 12);
this.lblOwnershipControl.TabIndex = 8;
this.lblOwnershipControl.Text = "数据归属控制";
this.chkOwnershipControl.Location = new System.Drawing.Point(173,196);
this.chkOwnershipControl.Name = "chkOwnershipControl";
this.chkOwnershipControl.Size = new System.Drawing.Size(100, 21);
this.chkOwnershipControl.TabIndex = 8;
this.Controls.Add(this.lblOwnershipControl);
this.Controls.Add(this.chkOwnershipControl);

           //#####SaleBizLimited###Boolean
this.lblSaleBizLimited.AutoSize = true;
this.lblSaleBizLimited.Location = new System.Drawing.Point(100,225);
this.lblSaleBizLimited.Name = "lblSaleBizLimited";
this.lblSaleBizLimited.Size = new System.Drawing.Size(41, 12);
this.lblSaleBizLimited.TabIndex = 9;
this.lblSaleBizLimited.Text = "销售业务范围限制";
this.chkSaleBizLimited.Location = new System.Drawing.Point(173,221);
this.chkSaleBizLimited.Name = "chkSaleBizLimited";
this.chkSaleBizLimited.Size = new System.Drawing.Size(100, 21);
this.chkSaleBizLimited.TabIndex = 9;
this.Controls.Add(this.lblSaleBizLimited);
this.Controls.Add(this.chkSaleBizLimited);

           //#####DepartBizLimited###Boolean
this.lblDepartBizLimited.AutoSize = true;
this.lblDepartBizLimited.Location = new System.Drawing.Point(100,250);
this.lblDepartBizLimited.Name = "lblDepartBizLimited";
this.lblDepartBizLimited.Size = new System.Drawing.Size(41, 12);
this.lblDepartBizLimited.TabIndex = 10;
this.lblDepartBizLimited.Text = "部门范围限制";
this.chkDepartBizLimited.Location = new System.Drawing.Point(173,246);
this.chkDepartBizLimited.Name = "chkDepartBizLimited";
this.chkDepartBizLimited.Size = new System.Drawing.Size(100, 21);
this.chkDepartBizLimited.TabIndex = 10;
this.Controls.Add(this.lblDepartBizLimited);
this.Controls.Add(this.chkDepartBizLimited);

           //#####PurchsaeBizLimited###Boolean
this.lblPurchsaeBizLimited.AutoSize = true;
this.lblPurchsaeBizLimited.Location = new System.Drawing.Point(100,275);
this.lblPurchsaeBizLimited.Name = "lblPurchsaeBizLimited";
this.lblPurchsaeBizLimited.Size = new System.Drawing.Size(41, 12);
this.lblPurchsaeBizLimited.TabIndex = 11;
this.lblPurchsaeBizLimited.Text = "采购业务范围限制";
this.chkPurchsaeBizLimited.Location = new System.Drawing.Point(173,271);
this.chkPurchsaeBizLimited.Name = "chkPurchsaeBizLimited";
this.chkPurchsaeBizLimited.Size = new System.Drawing.Size(100, 21);
this.chkPurchsaeBizLimited.TabIndex = 11;
this.Controls.Add(this.lblPurchsaeBizLimited);
this.Controls.Add(this.chkPurchsaeBizLimited);

           //#####QueryPageLayoutCustomize###Boolean
this.lblQueryPageLayoutCustomize.AutoSize = true;
this.lblQueryPageLayoutCustomize.Location = new System.Drawing.Point(100,300);
this.lblQueryPageLayoutCustomize.Name = "lblQueryPageLayoutCustomize";
this.lblQueryPageLayoutCustomize.Size = new System.Drawing.Size(41, 12);
this.lblQueryPageLayoutCustomize.TabIndex = 12;
this.lblQueryPageLayoutCustomize.Text = "查询页布局自定义";
this.chkQueryPageLayoutCustomize.Location = new System.Drawing.Point(173,296);
this.chkQueryPageLayoutCustomize.Name = "chkQueryPageLayoutCustomize";
this.chkQueryPageLayoutCustomize.Size = new System.Drawing.Size(100, 21);
this.chkQueryPageLayoutCustomize.TabIndex = 12;
this.Controls.Add(this.lblQueryPageLayoutCustomize);
this.Controls.Add(this.chkQueryPageLayoutCustomize);

           //#####QueryGridColCustomize###Boolean
this.lblQueryGridColCustomize.AutoSize = true;
this.lblQueryGridColCustomize.Location = new System.Drawing.Point(100,325);
this.lblQueryGridColCustomize.Name = "lblQueryGridColCustomize";
this.lblQueryGridColCustomize.Size = new System.Drawing.Size(41, 12);
this.lblQueryGridColCustomize.TabIndex = 13;
this.lblQueryGridColCustomize.Text = "查询表格列自定义";
this.chkQueryGridColCustomize.Location = new System.Drawing.Point(173,321);
this.chkQueryGridColCustomize.Name = "chkQueryGridColCustomize";
this.chkQueryGridColCustomize.Size = new System.Drawing.Size(100, 21);
this.chkQueryGridColCustomize.TabIndex = 13;
this.Controls.Add(this.lblQueryGridColCustomize);
this.Controls.Add(this.chkQueryGridColCustomize);

           //#####BillGridColCustomize###Boolean
this.lblBillGridColCustomize.AutoSize = true;
this.lblBillGridColCustomize.Location = new System.Drawing.Point(100,350);
this.lblBillGridColCustomize.Name = "lblBillGridColCustomize";
this.lblBillGridColCustomize.Size = new System.Drawing.Size(41, 12);
this.lblBillGridColCustomize.TabIndex = 14;
this.lblBillGridColCustomize.Text = "单据表格列自定义";
this.chkBillGridColCustomize.Location = new System.Drawing.Point(173,346);
this.chkBillGridColCustomize.Name = "chkBillGridColCustomize";
this.chkBillGridColCustomize.Size = new System.Drawing.Size(100, 21);
this.chkBillGridColCustomize.TabIndex = 14;
this.Controls.Add(this.lblBillGridColCustomize);
this.Controls.Add(this.chkBillGridColCustomize);

           //#####ExclusiveLimited###Boolean
this.lblExclusiveLimited.AutoSize = true;
this.lblExclusiveLimited.Location = new System.Drawing.Point(100,375);
this.lblExclusiveLimited.Name = "lblExclusiveLimited";
this.lblExclusiveLimited.Size = new System.Drawing.Size(41, 12);
this.lblExclusiveLimited.TabIndex = 15;
this.lblExclusiveLimited.Text = "启用责任人独占";
this.chkExclusiveLimited.Location = new System.Drawing.Point(173,371);
this.chkExclusiveLimited.Name = "chkExclusiveLimited";
this.chkExclusiveLimited.Size = new System.Drawing.Size(100, 21);
this.chkExclusiveLimited.TabIndex = 15;
this.Controls.Add(this.lblExclusiveLimited);
this.Controls.Add(this.chkExclusiveLimited);

           //#####500DataBoardUnits###String
this.lblDataBoardUnits.AutoSize = true;
this.lblDataBoardUnits.Location = new System.Drawing.Point(100,400);
this.lblDataBoardUnits.Name = "lblDataBoardUnits";
this.lblDataBoardUnits.Size = new System.Drawing.Size(41, 12);
this.lblDataBoardUnits.TabIndex = 16;
this.lblDataBoardUnits.Text = "";
this.txtDataBoardUnits.Location = new System.Drawing.Point(173,396);
this.txtDataBoardUnits.Name = "txtDataBoardUnits";
this.txtDataBoardUnits.Size = new System.Drawing.Size(100, 21);
this.txtDataBoardUnits.TabIndex = 16;
this.Controls.Add(this.lblDataBoardUnits);
this.Controls.Add(this.txtDataBoardUnits);

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
           // this.kryptonPanel1.TabIndex = 16;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblRolePropertyName );
this.Controls.Add(this.txtRolePropertyName );

                this.Controls.Add(this.lblQtyDataPrecision );
this.Controls.Add(this.txtQtyDataPrecision );

                this.Controls.Add(this.lblTaxRateDataPrecision );
this.Controls.Add(this.txtTaxRateDataPrecision );

                this.Controls.Add(this.lblMoneyDataPrecision );
this.Controls.Add(this.txtMoneyDataPrecision );

                this.Controls.Add(this.lblCurrencyDataPrecisionAutoAddZero );
this.Controls.Add(this.chkCurrencyDataPrecisionAutoAddZero );

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

                this.Controls.Add(this.lblQueryPageLayoutCustomize );
this.Controls.Add(this.chkQueryPageLayoutCustomize );

                this.Controls.Add(this.lblQueryGridColCustomize );
this.Controls.Add(this.chkQueryGridColCustomize );

                this.Controls.Add(this.lblBillGridColCustomize );
this.Controls.Add(this.chkBillGridColCustomize );

                this.Controls.Add(this.lblExclusiveLimited );
this.Controls.Add(this.chkExclusiveLimited );

                this.Controls.Add(this.lblDataBoardUnits );
this.Controls.Add(this.txtDataBoardUnits );

                            // 
            // "tb_RolePropertyConfigEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_RolePropertyConfigEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblRolePropertyName;
private Krypton.Toolkit.KryptonTextBox txtRolePropertyName;

    
        
              private Krypton.Toolkit.KryptonLabel lblQtyDataPrecision;
private Krypton.Toolkit.KryptonTextBox txtQtyDataPrecision;

    
        
              private Krypton.Toolkit.KryptonLabel lblTaxRateDataPrecision;
private Krypton.Toolkit.KryptonTextBox txtTaxRateDataPrecision;

    
        
              private Krypton.Toolkit.KryptonLabel lblMoneyDataPrecision;
private Krypton.Toolkit.KryptonTextBox txtMoneyDataPrecision;

    
        
              private Krypton.Toolkit.KryptonLabel lblCurrencyDataPrecisionAutoAddZero;
private Krypton.Toolkit.KryptonCheckBox chkCurrencyDataPrecisionAutoAddZero;

    
        
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

    
        
              private Krypton.Toolkit.KryptonLabel lblQueryPageLayoutCustomize;
private Krypton.Toolkit.KryptonCheckBox chkQueryPageLayoutCustomize;

    
        
              private Krypton.Toolkit.KryptonLabel lblQueryGridColCustomize;
private Krypton.Toolkit.KryptonCheckBox chkQueryGridColCustomize;

    
        
              private Krypton.Toolkit.KryptonLabel lblBillGridColCustomize;
private Krypton.Toolkit.KryptonCheckBox chkBillGridColCustomize;

    
        
              private Krypton.Toolkit.KryptonLabel lblExclusiveLimited;
private Krypton.Toolkit.KryptonCheckBox chkExclusiveLimited;

    
        
              private Krypton.Toolkit.KryptonLabel lblDataBoardUnits;
private Krypton.Toolkit.KryptonTextBox txtDataBoardUnits;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

