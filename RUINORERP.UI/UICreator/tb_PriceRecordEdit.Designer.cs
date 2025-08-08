// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:50
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 价格记录表
    /// </summary>
    partial class tb_PriceRecordEdit
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

this.lblEmployee_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbEmployee_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblPurDate = new Krypton.Toolkit.KryptonLabel();
this.dtpPurDate = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblSaleDate = new Krypton.Toolkit.KryptonLabel();
this.dtpSaleDate = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblPurPrice = new Krypton.Toolkit.KryptonLabel();
this.txtPurPrice = new Krypton.Toolkit.KryptonTextBox();

this.lblSalePrice = new Krypton.Toolkit.KryptonLabel();
this.txtSalePrice = new Krypton.Toolkit.KryptonTextBox();

    
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

           //#####Employee_ID###Int64
//属性测试50Employee_ID
//属性测试50Employee_ID
this.lblEmployee_ID.AutoSize = true;
this.lblEmployee_ID.Location = new System.Drawing.Point(100,50);
this.lblEmployee_ID.Name = "lblEmployee_ID";
this.lblEmployee_ID.Size = new System.Drawing.Size(41, 12);
this.lblEmployee_ID.TabIndex = 2;
this.lblEmployee_ID.Text = "经办人";
//111======50
this.cmbEmployee_ID.Location = new System.Drawing.Point(173,46);
this.cmbEmployee_ID.Name ="cmbEmployee_ID";
this.cmbEmployee_ID.Size = new System.Drawing.Size(100, 21);
this.cmbEmployee_ID.TabIndex = 2;
this.Controls.Add(this.lblEmployee_ID);
this.Controls.Add(this.cmbEmployee_ID);

           //#####PurDate###DateTime
this.lblPurDate.AutoSize = true;
this.lblPurDate.Location = new System.Drawing.Point(100,75);
this.lblPurDate.Name = "lblPurDate";
this.lblPurDate.Size = new System.Drawing.Size(41, 12);
this.lblPurDate.TabIndex = 3;
this.lblPurDate.Text = "采购日期";
//111======75
this.dtpPurDate.Location = new System.Drawing.Point(173,71);
this.dtpPurDate.Name ="dtpPurDate";
this.dtpPurDate.ShowCheckBox =true;
this.dtpPurDate.Size = new System.Drawing.Size(100, 21);
this.dtpPurDate.TabIndex = 3;
this.Controls.Add(this.lblPurDate);
this.Controls.Add(this.dtpPurDate);

           //#####SaleDate###DateTime
this.lblSaleDate.AutoSize = true;
this.lblSaleDate.Location = new System.Drawing.Point(100,100);
this.lblSaleDate.Name = "lblSaleDate";
this.lblSaleDate.Size = new System.Drawing.Size(41, 12);
this.lblSaleDate.TabIndex = 4;
this.lblSaleDate.Text = "销售日期";
//111======100
this.dtpSaleDate.Location = new System.Drawing.Point(173,96);
this.dtpSaleDate.Name ="dtpSaleDate";
this.dtpSaleDate.ShowCheckBox =true;
this.dtpSaleDate.Size = new System.Drawing.Size(100, 21);
this.dtpSaleDate.TabIndex = 4;
this.Controls.Add(this.lblSaleDate);
this.Controls.Add(this.dtpSaleDate);

           //#####PurPrice###Decimal
this.lblPurPrice.AutoSize = true;
this.lblPurPrice.Location = new System.Drawing.Point(100,125);
this.lblPurPrice.Name = "lblPurPrice";
this.lblPurPrice.Size = new System.Drawing.Size(41, 12);
this.lblPurPrice.TabIndex = 5;
this.lblPurPrice.Text = "采购价";
//111======125
this.txtPurPrice.Location = new System.Drawing.Point(173,121);
this.txtPurPrice.Name ="txtPurPrice";
this.txtPurPrice.Size = new System.Drawing.Size(100, 21);
this.txtPurPrice.TabIndex = 5;
this.Controls.Add(this.lblPurPrice);
this.Controls.Add(this.txtPurPrice);

           //#####SalePrice###Decimal
this.lblSalePrice.AutoSize = true;
this.lblSalePrice.Location = new System.Drawing.Point(100,150);
this.lblSalePrice.Name = "lblSalePrice";
this.lblSalePrice.Size = new System.Drawing.Size(41, 12);
this.lblSalePrice.TabIndex = 6;
this.lblSalePrice.Text = "销售价";
//111======150
this.txtSalePrice.Location = new System.Drawing.Point(173,146);
this.txtSalePrice.Name ="txtSalePrice";
this.txtSalePrice.Size = new System.Drawing.Size(100, 21);
this.txtSalePrice.TabIndex = 6;
this.Controls.Add(this.lblSalePrice);
this.Controls.Add(this.txtSalePrice);

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
           // this.kryptonPanel1.TabIndex = 6;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblProdDetailID );
this.Controls.Add(this.cmbProdDetailID );

                this.Controls.Add(this.lblEmployee_ID );
this.Controls.Add(this.cmbEmployee_ID );

                this.Controls.Add(this.lblPurDate );
this.Controls.Add(this.dtpPurDate );

                this.Controls.Add(this.lblSaleDate );
this.Controls.Add(this.dtpSaleDate );

                this.Controls.Add(this.lblPurPrice );
this.Controls.Add(this.txtPurPrice );

                this.Controls.Add(this.lblSalePrice );
this.Controls.Add(this.txtSalePrice );

                            // 
            // "tb_PriceRecordEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_PriceRecordEdit";
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

    
        
              private Krypton.Toolkit.KryptonLabel lblEmployee_ID;
private Krypton.Toolkit.KryptonComboBox cmbEmployee_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblPurDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpPurDate;

    
        
              private Krypton.Toolkit.KryptonLabel lblSaleDate;
private Krypton.Toolkit.KryptonDateTimePicker dtpSaleDate;

    
        
              private Krypton.Toolkit.KryptonLabel lblPurPrice;
private Krypton.Toolkit.KryptonTextBox txtPurPrice;

    
        
              private Krypton.Toolkit.KryptonLabel lblSalePrice;
private Krypton.Toolkit.KryptonTextBox txtSalePrice;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

