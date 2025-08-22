// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/22/2025 21:05:37
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 损益费用单
    /// </summary>
    partial class tb_FM_ProfitLossDetailEdit
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
     this.lblProfitLossId = new Krypton.Toolkit.KryptonLabel();
this.cmbProfitLossId = new Krypton.Toolkit.KryptonComboBox();

this.lblProdDetailID = new Krypton.Toolkit.KryptonLabel();
this.cmbProdDetailID = new Krypton.Toolkit.KryptonComboBox();

this.lblproperty = new Krypton.Toolkit.KryptonLabel();
this.txtproperty = new Krypton.Toolkit.KryptonTextBox();
this.txtproperty.Multiline = true;

this.lblProfitLossType = new Krypton.Toolkit.KryptonLabel();
this.txtProfitLossType = new Krypton.Toolkit.KryptonTextBox();

this.lblIncomeExpenseDirection = new Krypton.Toolkit.KryptonLabel();
this.txtIncomeExpenseDirection = new Krypton.Toolkit.KryptonTextBox();

this.lblExpenseDescription = new Krypton.Toolkit.KryptonLabel();
this.txtExpenseDescription = new Krypton.Toolkit.KryptonTextBox();
this.txtExpenseDescription.Multiline = true;

this.lblUnitPrice = new Krypton.Toolkit.KryptonLabel();
this.txtUnitPrice = new Krypton.Toolkit.KryptonTextBox();

this.lblQuantity = new Krypton.Toolkit.KryptonLabel();
this.txtQuantity = new Krypton.Toolkit.KryptonTextBox();

this.lblSubtotalAmont = new Krypton.Toolkit.KryptonLabel();
this.txtSubtotalAmont = new Krypton.Toolkit.KryptonTextBox();

this.lblUntaxedSubtotalAmont = new Krypton.Toolkit.KryptonLabel();
this.txtUntaxedSubtotalAmont = new Krypton.Toolkit.KryptonTextBox();

this.lblTaxRate = new Krypton.Toolkit.KryptonLabel();
this.txtTaxRate = new Krypton.Toolkit.KryptonTextBox();

this.lblTaxSubtotalAmont = new Krypton.Toolkit.KryptonLabel();
this.txtTaxSubtotalAmont = new Krypton.Toolkit.KryptonTextBox();

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
     
            //#####ProfitLossId###Int64
//属性测试25ProfitLossId
this.lblProfitLossId.AutoSize = true;
this.lblProfitLossId.Location = new System.Drawing.Point(100,25);
this.lblProfitLossId.Name = "lblProfitLossId";
this.lblProfitLossId.Size = new System.Drawing.Size(41, 12);
this.lblProfitLossId.TabIndex = 1;
this.lblProfitLossId.Text = "损益费用单";
//111======25
this.cmbProfitLossId.Location = new System.Drawing.Point(173,21);
this.cmbProfitLossId.Name ="cmbProfitLossId";
this.cmbProfitLossId.Size = new System.Drawing.Size(100, 21);
this.cmbProfitLossId.TabIndex = 1;
this.Controls.Add(this.lblProfitLossId);
this.Controls.Add(this.cmbProfitLossId);

           //#####ProdDetailID###Int64
//属性测试50ProdDetailID
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

           //#####ProfitLossType###Int32
//属性测试100ProfitLossType
//属性测试100ProfitLossType
this.lblProfitLossType.AutoSize = true;
this.lblProfitLossType.Location = new System.Drawing.Point(100,100);
this.lblProfitLossType.Name = "lblProfitLossType";
this.lblProfitLossType.Size = new System.Drawing.Size(41, 12);
this.lblProfitLossType.TabIndex = 4;
this.lblProfitLossType.Text = "损溢类型";
this.txtProfitLossType.Location = new System.Drawing.Point(173,96);
this.txtProfitLossType.Name = "txtProfitLossType";
this.txtProfitLossType.Size = new System.Drawing.Size(100, 21);
this.txtProfitLossType.TabIndex = 4;
this.Controls.Add(this.lblProfitLossType);
this.Controls.Add(this.txtProfitLossType);

           //#####IncomeExpenseDirection###Int32
//属性测试125IncomeExpenseDirection
//属性测试125IncomeExpenseDirection
this.lblIncomeExpenseDirection.AutoSize = true;
this.lblIncomeExpenseDirection.Location = new System.Drawing.Point(100,125);
this.lblIncomeExpenseDirection.Name = "lblIncomeExpenseDirection";
this.lblIncomeExpenseDirection.Size = new System.Drawing.Size(41, 12);
this.lblIncomeExpenseDirection.TabIndex = 5;
this.lblIncomeExpenseDirection.Text = "收支方向";
this.txtIncomeExpenseDirection.Location = new System.Drawing.Point(173,121);
this.txtIncomeExpenseDirection.Name = "txtIncomeExpenseDirection";
this.txtIncomeExpenseDirection.Size = new System.Drawing.Size(100, 21);
this.txtIncomeExpenseDirection.TabIndex = 5;
this.Controls.Add(this.lblIncomeExpenseDirection);
this.Controls.Add(this.txtIncomeExpenseDirection);

           //#####300ExpenseDescription###String
this.lblExpenseDescription.AutoSize = true;
this.lblExpenseDescription.Location = new System.Drawing.Point(100,150);
this.lblExpenseDescription.Name = "lblExpenseDescription";
this.lblExpenseDescription.Size = new System.Drawing.Size(41, 12);
this.lblExpenseDescription.TabIndex = 6;
this.lblExpenseDescription.Text = "费用说明";
this.txtExpenseDescription.Location = new System.Drawing.Point(173,146);
this.txtExpenseDescription.Name = "txtExpenseDescription";
this.txtExpenseDescription.Size = new System.Drawing.Size(100, 21);
this.txtExpenseDescription.TabIndex = 6;
this.Controls.Add(this.lblExpenseDescription);
this.Controls.Add(this.txtExpenseDescription);

           //#####UnitPrice###Decimal
this.lblUnitPrice.AutoSize = true;
this.lblUnitPrice.Location = new System.Drawing.Point(100,175);
this.lblUnitPrice.Name = "lblUnitPrice";
this.lblUnitPrice.Size = new System.Drawing.Size(41, 12);
this.lblUnitPrice.TabIndex = 7;
this.lblUnitPrice.Text = "单价";
//111======175
this.txtUnitPrice.Location = new System.Drawing.Point(173,171);
this.txtUnitPrice.Name ="txtUnitPrice";
this.txtUnitPrice.Size = new System.Drawing.Size(100, 21);
this.txtUnitPrice.TabIndex = 7;
this.Controls.Add(this.lblUnitPrice);
this.Controls.Add(this.txtUnitPrice);

           //#####Quantity###Decimal
this.lblQuantity.AutoSize = true;
this.lblQuantity.Location = new System.Drawing.Point(100,200);
this.lblQuantity.Name = "lblQuantity";
this.lblQuantity.Size = new System.Drawing.Size(41, 12);
this.lblQuantity.TabIndex = 8;
this.lblQuantity.Text = "数量";
//111======200
this.txtQuantity.Location = new System.Drawing.Point(173,196);
this.txtQuantity.Name ="txtQuantity";
this.txtQuantity.Size = new System.Drawing.Size(100, 21);
this.txtQuantity.TabIndex = 8;
this.Controls.Add(this.lblQuantity);
this.Controls.Add(this.txtQuantity);

           //#####SubtotalAmont###Decimal
this.lblSubtotalAmont.AutoSize = true;
this.lblSubtotalAmont.Location = new System.Drawing.Point(100,225);
this.lblSubtotalAmont.Name = "lblSubtotalAmont";
this.lblSubtotalAmont.Size = new System.Drawing.Size(41, 12);
this.lblSubtotalAmont.TabIndex = 9;
this.lblSubtotalAmont.Text = "金额小计";
//111======225
this.txtSubtotalAmont.Location = new System.Drawing.Point(173,221);
this.txtSubtotalAmont.Name ="txtSubtotalAmont";
this.txtSubtotalAmont.Size = new System.Drawing.Size(100, 21);
this.txtSubtotalAmont.TabIndex = 9;
this.Controls.Add(this.lblSubtotalAmont);
this.Controls.Add(this.txtSubtotalAmont);

           //#####UntaxedSubtotalAmont###Decimal
this.lblUntaxedSubtotalAmont.AutoSize = true;
this.lblUntaxedSubtotalAmont.Location = new System.Drawing.Point(100,250);
this.lblUntaxedSubtotalAmont.Name = "lblUntaxedSubtotalAmont";
this.lblUntaxedSubtotalAmont.Size = new System.Drawing.Size(41, 12);
this.lblUntaxedSubtotalAmont.TabIndex = 10;
this.lblUntaxedSubtotalAmont.Text = "未税小计";
//111======250
this.txtUntaxedSubtotalAmont.Location = new System.Drawing.Point(173,246);
this.txtUntaxedSubtotalAmont.Name ="txtUntaxedSubtotalAmont";
this.txtUntaxedSubtotalAmont.Size = new System.Drawing.Size(100, 21);
this.txtUntaxedSubtotalAmont.TabIndex = 10;
this.Controls.Add(this.lblUntaxedSubtotalAmont);
this.Controls.Add(this.txtUntaxedSubtotalAmont);

           //#####TaxRate###Decimal
this.lblTaxRate.AutoSize = true;
this.lblTaxRate.Location = new System.Drawing.Point(100,275);
this.lblTaxRate.Name = "lblTaxRate";
this.lblTaxRate.Size = new System.Drawing.Size(41, 12);
this.lblTaxRate.TabIndex = 11;
this.lblTaxRate.Text = "税率";
//111======275
this.txtTaxRate.Location = new System.Drawing.Point(173,271);
this.txtTaxRate.Name ="txtTaxRate";
this.txtTaxRate.Size = new System.Drawing.Size(100, 21);
this.txtTaxRate.TabIndex = 11;
this.Controls.Add(this.lblTaxRate);
this.Controls.Add(this.txtTaxRate);

           //#####TaxSubtotalAmont###Decimal
this.lblTaxSubtotalAmont.AutoSize = true;
this.lblTaxSubtotalAmont.Location = new System.Drawing.Point(100,300);
this.lblTaxSubtotalAmont.Name = "lblTaxSubtotalAmont";
this.lblTaxSubtotalAmont.Size = new System.Drawing.Size(41, 12);
this.lblTaxSubtotalAmont.TabIndex = 12;
this.lblTaxSubtotalAmont.Text = "税额";
//111======300
this.txtTaxSubtotalAmont.Location = new System.Drawing.Point(173,296);
this.txtTaxSubtotalAmont.Name ="txtTaxSubtotalAmont";
this.txtTaxSubtotalAmont.Size = new System.Drawing.Size(100, 21);
this.txtTaxSubtotalAmont.TabIndex = 12;
this.Controls.Add(this.lblTaxSubtotalAmont);
this.Controls.Add(this.txtTaxSubtotalAmont);

           //#####300Summary###String
this.lblSummary.AutoSize = true;
this.lblSummary.Location = new System.Drawing.Point(100,325);
this.lblSummary.Name = "lblSummary";
this.lblSummary.Size = new System.Drawing.Size(41, 12);
this.lblSummary.TabIndex = 13;
this.lblSummary.Text = "摘要";
this.txtSummary.Location = new System.Drawing.Point(173,321);
this.txtSummary.Name = "txtSummary";
this.txtSummary.Size = new System.Drawing.Size(100, 21);
this.txtSummary.TabIndex = 13;
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
           // this.kryptonPanel1.TabIndex = 13;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblProfitLossId );
this.Controls.Add(this.cmbProfitLossId );

                this.Controls.Add(this.lblProdDetailID );
this.Controls.Add(this.cmbProdDetailID );

                this.Controls.Add(this.lblproperty );
this.Controls.Add(this.txtproperty );

                this.Controls.Add(this.lblProfitLossType );
this.Controls.Add(this.txtProfitLossType );

                this.Controls.Add(this.lblIncomeExpenseDirection );
this.Controls.Add(this.txtIncomeExpenseDirection );

                this.Controls.Add(this.lblExpenseDescription );
this.Controls.Add(this.txtExpenseDescription );

                this.Controls.Add(this.lblUnitPrice );
this.Controls.Add(this.txtUnitPrice );

                this.Controls.Add(this.lblQuantity );
this.Controls.Add(this.txtQuantity );

                this.Controls.Add(this.lblSubtotalAmont );
this.Controls.Add(this.txtSubtotalAmont );

                this.Controls.Add(this.lblUntaxedSubtotalAmont );
this.Controls.Add(this.txtUntaxedSubtotalAmont );

                this.Controls.Add(this.lblTaxRate );
this.Controls.Add(this.txtTaxRate );

                this.Controls.Add(this.lblTaxSubtotalAmont );
this.Controls.Add(this.txtTaxSubtotalAmont );

                this.Controls.Add(this.lblSummary );
this.Controls.Add(this.txtSummary );

                            // 
            // "tb_FM_ProfitLossDetailEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_FM_ProfitLossDetailEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblProfitLossId;
private Krypton.Toolkit.KryptonComboBox cmbProfitLossId;

    
        
              private Krypton.Toolkit.KryptonLabel lblProdDetailID;
private Krypton.Toolkit.KryptonComboBox cmbProdDetailID;

    
        
              private Krypton.Toolkit.KryptonLabel lblproperty;
private Krypton.Toolkit.KryptonTextBox txtproperty;

    
        
              private Krypton.Toolkit.KryptonLabel lblProfitLossType;
private Krypton.Toolkit.KryptonTextBox txtProfitLossType;

    
        
              private Krypton.Toolkit.KryptonLabel lblIncomeExpenseDirection;
private Krypton.Toolkit.KryptonTextBox txtIncomeExpenseDirection;

    
        
              private Krypton.Toolkit.KryptonLabel lblExpenseDescription;
private Krypton.Toolkit.KryptonTextBox txtExpenseDescription;

    
        
              private Krypton.Toolkit.KryptonLabel lblUnitPrice;
private Krypton.Toolkit.KryptonTextBox txtUnitPrice;

    
        
              private Krypton.Toolkit.KryptonLabel lblQuantity;
private Krypton.Toolkit.KryptonTextBox txtQuantity;

    
        
              private Krypton.Toolkit.KryptonLabel lblSubtotalAmont;
private Krypton.Toolkit.KryptonTextBox txtSubtotalAmont;

    
        
              private Krypton.Toolkit.KryptonLabel lblUntaxedSubtotalAmont;
private Krypton.Toolkit.KryptonTextBox txtUntaxedSubtotalAmont;

    
        
              private Krypton.Toolkit.KryptonLabel lblTaxRate;
private Krypton.Toolkit.KryptonTextBox txtTaxRate;

    
        
              private Krypton.Toolkit.KryptonLabel lblTaxSubtotalAmont;
private Krypton.Toolkit.KryptonTextBox txtTaxSubtotalAmont;

    
        
              private Krypton.Toolkit.KryptonLabel lblSummary;
private Krypton.Toolkit.KryptonTextBox txtSummary;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

