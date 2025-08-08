// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:46:09
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 采购退回单
    /// </summary>
    partial class tb_PurOrderReDetailEdit
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
     this.lblPurRetrunID = new Krypton.Toolkit.KryptonLabel();
this.cmbPurRetrunID = new Krypton.Toolkit.KryptonComboBox();

this.lblProdDetailID = new Krypton.Toolkit.KryptonLabel();
this.cmbProdDetailID = new Krypton.Toolkit.KryptonComboBox();

this.lblLocation_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbLocation_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblproperty = new Krypton.Toolkit.KryptonLabel();
this.txtproperty = new Krypton.Toolkit.KryptonTextBox();
this.txtproperty.Multiline = true;

this.lblQuantity = new Krypton.Toolkit.KryptonLabel();
this.txtQuantity = new Krypton.Toolkit.KryptonTextBox();

this.lblDiscount = new Krypton.Toolkit.KryptonLabel();
this.txtDiscount = new Krypton.Toolkit.KryptonTextBox();

this.lblTransactionPrice = new Krypton.Toolkit.KryptonLabel();
this.txtTransactionPrice = new Krypton.Toolkit.KryptonTextBox();

this.lblTotalAmount = new Krypton.Toolkit.KryptonLabel();
this.txtTotalAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblSummary = new Krypton.Toolkit.KryptonLabel();
this.txtSummary = new Krypton.Toolkit.KryptonTextBox();
this.txtSummary.Multiline = true;

this.lblCustomerType = new Krypton.Toolkit.KryptonLabel();
this.txtCustomerType = new Krypton.Toolkit.KryptonTextBox();

this.lblcommission = new Krypton.Toolkit.KryptonLabel();
this.txtcommission = new Krypton.Toolkit.KryptonTextBox();

this.lblNotes = new Krypton.Toolkit.KryptonLabel();
this.txtNotes = new Krypton.Toolkit.KryptonTextBox();

    
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
     
            //#####PurRetrunID###Int64
//属性测试25PurRetrunID
//属性测试25PurRetrunID
//属性测试25PurRetrunID
this.lblPurRetrunID.AutoSize = true;
this.lblPurRetrunID.Location = new System.Drawing.Point(100,25);
this.lblPurRetrunID.Name = "lblPurRetrunID";
this.lblPurRetrunID.Size = new System.Drawing.Size(41, 12);
this.lblPurRetrunID.TabIndex = 1;
this.lblPurRetrunID.Text = "";
//111======25
this.cmbPurRetrunID.Location = new System.Drawing.Point(173,21);
this.cmbPurRetrunID.Name ="cmbPurRetrunID";
this.cmbPurRetrunID.Size = new System.Drawing.Size(100, 21);
this.cmbPurRetrunID.TabIndex = 1;
this.Controls.Add(this.lblPurRetrunID);
this.Controls.Add(this.cmbPurRetrunID);

           //#####ProdDetailID###Int64
//属性测试50ProdDetailID
//属性测试50ProdDetailID
this.lblProdDetailID.AutoSize = true;
this.lblProdDetailID.Location = new System.Drawing.Point(100,50);
this.lblProdDetailID.Name = "lblProdDetailID";
this.lblProdDetailID.Size = new System.Drawing.Size(41, 12);
this.lblProdDetailID.TabIndex = 2;
this.lblProdDetailID.Text = "货品";
//111======50
this.cmbProdDetailID.Location = new System.Drawing.Point(173,46);
this.cmbProdDetailID.Name ="cmbProdDetailID";
this.cmbProdDetailID.Size = new System.Drawing.Size(100, 21);
this.cmbProdDetailID.TabIndex = 2;
this.Controls.Add(this.lblProdDetailID);
this.Controls.Add(this.cmbProdDetailID);

           //#####Location_ID###Int64
//属性测试75Location_ID
this.lblLocation_ID.AutoSize = true;
this.lblLocation_ID.Location = new System.Drawing.Point(100,75);
this.lblLocation_ID.Name = "lblLocation_ID";
this.lblLocation_ID.Size = new System.Drawing.Size(41, 12);
this.lblLocation_ID.TabIndex = 3;
this.lblLocation_ID.Text = "";
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

           //#####Discount###Decimal
this.lblDiscount.AutoSize = true;
this.lblDiscount.Location = new System.Drawing.Point(100,150);
this.lblDiscount.Name = "lblDiscount";
this.lblDiscount.Size = new System.Drawing.Size(41, 12);
this.lblDiscount.TabIndex = 6;
this.lblDiscount.Text = "折扣";
//111======150
this.txtDiscount.Location = new System.Drawing.Point(173,146);
this.txtDiscount.Name ="txtDiscount";
this.txtDiscount.Size = new System.Drawing.Size(100, 21);
this.txtDiscount.TabIndex = 6;
this.Controls.Add(this.lblDiscount);
this.Controls.Add(this.txtDiscount);

           //#####TransactionPrice###Decimal
this.lblTransactionPrice.AutoSize = true;
this.lblTransactionPrice.Location = new System.Drawing.Point(100,175);
this.lblTransactionPrice.Name = "lblTransactionPrice";
this.lblTransactionPrice.Size = new System.Drawing.Size(41, 12);
this.lblTransactionPrice.TabIndex = 7;
this.lblTransactionPrice.Text = "成交单价";
//111======175
this.txtTransactionPrice.Location = new System.Drawing.Point(173,171);
this.txtTransactionPrice.Name ="txtTransactionPrice";
this.txtTransactionPrice.Size = new System.Drawing.Size(100, 21);
this.txtTransactionPrice.TabIndex = 7;
this.Controls.Add(this.lblTransactionPrice);
this.Controls.Add(this.txtTransactionPrice);

           //#####TotalAmount###Decimal
this.lblTotalAmount.AutoSize = true;
this.lblTotalAmount.Location = new System.Drawing.Point(100,200);
this.lblTotalAmount.Name = "lblTotalAmount";
this.lblTotalAmount.Size = new System.Drawing.Size(41, 12);
this.lblTotalAmount.TabIndex = 8;
this.lblTotalAmount.Text = "小计";
//111======200
this.txtTotalAmount.Location = new System.Drawing.Point(173,196);
this.txtTotalAmount.Name ="txtTotalAmount";
this.txtTotalAmount.Size = new System.Drawing.Size(100, 21);
this.txtTotalAmount.TabIndex = 8;
this.Controls.Add(this.lblTotalAmount);
this.Controls.Add(this.txtTotalAmount);

           //#####255Summary###String
this.lblSummary.AutoSize = true;
this.lblSummary.Location = new System.Drawing.Point(100,225);
this.lblSummary.Name = "lblSummary";
this.lblSummary.Size = new System.Drawing.Size(41, 12);
this.lblSummary.TabIndex = 9;
this.lblSummary.Text = "摘要";
this.txtSummary.Location = new System.Drawing.Point(173,221);
this.txtSummary.Name = "txtSummary";
this.txtSummary.Size = new System.Drawing.Size(100, 21);
this.txtSummary.TabIndex = 9;
this.Controls.Add(this.lblSummary);
this.Controls.Add(this.txtSummary);

           //#####100CustomerType###String
this.lblCustomerType.AutoSize = true;
this.lblCustomerType.Location = new System.Drawing.Point(100,250);
this.lblCustomerType.Name = "lblCustomerType";
this.lblCustomerType.Size = new System.Drawing.Size(41, 12);
this.lblCustomerType.TabIndex = 10;
this.lblCustomerType.Text = "客户型号";
this.txtCustomerType.Location = new System.Drawing.Point(173,246);
this.txtCustomerType.Name = "txtCustomerType";
this.txtCustomerType.Size = new System.Drawing.Size(100, 21);
this.txtCustomerType.TabIndex = 10;
this.Controls.Add(this.lblCustomerType);
this.Controls.Add(this.txtCustomerType);

           //#####commission###Decimal
this.lblcommission.AutoSize = true;
this.lblcommission.Location = new System.Drawing.Point(100,275);
this.lblcommission.Name = "lblcommission";
this.lblcommission.Size = new System.Drawing.Size(41, 12);
this.lblcommission.TabIndex = 11;
this.lblcommission.Text = "抽成金额";
//111======275
this.txtcommission.Location = new System.Drawing.Point(173,271);
this.txtcommission.Name ="txtcommission";
this.txtcommission.Size = new System.Drawing.Size(100, 21);
this.txtcommission.TabIndex = 11;
this.Controls.Add(this.lblcommission);
this.Controls.Add(this.txtcommission);

           //#####200Notes###String
this.lblNotes.AutoSize = true;
this.lblNotes.Location = new System.Drawing.Point(100,300);
this.lblNotes.Name = "lblNotes";
this.lblNotes.Size = new System.Drawing.Size(41, 12);
this.lblNotes.TabIndex = 12;
this.lblNotes.Text = "备注";
this.txtNotes.Location = new System.Drawing.Point(173,296);
this.txtNotes.Name = "txtNotes";
this.txtNotes.Size = new System.Drawing.Size(100, 21);
this.txtNotes.TabIndex = 12;
this.Controls.Add(this.lblNotes);
this.Controls.Add(this.txtNotes);

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
           // this.kryptonPanel1.TabIndex = 12;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblPurRetrunID );
this.Controls.Add(this.cmbPurRetrunID );

                this.Controls.Add(this.lblProdDetailID );
this.Controls.Add(this.cmbProdDetailID );

                this.Controls.Add(this.lblLocation_ID );
this.Controls.Add(this.cmbLocation_ID );

                this.Controls.Add(this.lblproperty );
this.Controls.Add(this.txtproperty );

                this.Controls.Add(this.lblQuantity );
this.Controls.Add(this.txtQuantity );

                this.Controls.Add(this.lblDiscount );
this.Controls.Add(this.txtDiscount );

                this.Controls.Add(this.lblTransactionPrice );
this.Controls.Add(this.txtTransactionPrice );

                this.Controls.Add(this.lblTotalAmount );
this.Controls.Add(this.txtTotalAmount );

                this.Controls.Add(this.lblSummary );
this.Controls.Add(this.txtSummary );

                this.Controls.Add(this.lblCustomerType );
this.Controls.Add(this.txtCustomerType );

                this.Controls.Add(this.lblcommission );
this.Controls.Add(this.txtcommission );

                this.Controls.Add(this.lblNotes );
this.Controls.Add(this.txtNotes );

                            // 
            // "tb_PurOrderReDetailEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_PurOrderReDetailEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblPurRetrunID;
private Krypton.Toolkit.KryptonComboBox cmbPurRetrunID;

    
        
              private Krypton.Toolkit.KryptonLabel lblProdDetailID;
private Krypton.Toolkit.KryptonComboBox cmbProdDetailID;

    
        
              private Krypton.Toolkit.KryptonLabel lblLocation_ID;
private Krypton.Toolkit.KryptonComboBox cmbLocation_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblproperty;
private Krypton.Toolkit.KryptonTextBox txtproperty;

    
        
              private Krypton.Toolkit.KryptonLabel lblQuantity;
private Krypton.Toolkit.KryptonTextBox txtQuantity;

    
        
              private Krypton.Toolkit.KryptonLabel lblDiscount;
private Krypton.Toolkit.KryptonTextBox txtDiscount;

    
        
              private Krypton.Toolkit.KryptonLabel lblTransactionPrice;
private Krypton.Toolkit.KryptonTextBox txtTransactionPrice;

    
        
              private Krypton.Toolkit.KryptonLabel lblTotalAmount;
private Krypton.Toolkit.KryptonTextBox txtTotalAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblSummary;
private Krypton.Toolkit.KryptonTextBox txtSummary;

    
        
              private Krypton.Toolkit.KryptonLabel lblCustomerType;
private Krypton.Toolkit.KryptonTextBox txtCustomerType;

    
        
              private Krypton.Toolkit.KryptonLabel lblcommission;
private Krypton.Toolkit.KryptonTextBox txtcommission;

    
        
              private Krypton.Toolkit.KryptonLabel lblNotes;
private Krypton.Toolkit.KryptonTextBox txtNotes;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

