// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:46
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 外发加工订单表
    /// </summary>
    partial class tb_Outsourcing_orderEdit
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
     this.lblQuantity = new Krypton.Toolkit.KryptonLabel();
this.txtQuantity = new Krypton.Toolkit.KryptonTextBox();

this.lblUnit_price = new Krypton.Toolkit.KryptonLabel();
this.txtUnit_price = new Krypton.Toolkit.KryptonTextBox();

this.lblTotal_amount = new Krypton.Toolkit.KryptonLabel();
this.txtTotal_amount = new Krypton.Toolkit.KryptonTextBox();

this.lblStatus = new Krypton.Toolkit.KryptonLabel();
this.txtStatus = new Krypton.Toolkit.KryptonTextBox();

this.lblOrder_date = new Krypton.Toolkit.KryptonLabel();
this.dtpOrder_date = new Krypton.Toolkit.KryptonDateTimePicker();

this.lblDelivery_date = new Krypton.Toolkit.KryptonLabel();
this.dtpDelivery_date = new Krypton.Toolkit.KryptonDateTimePicker();

    
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
     
            //#####Quantity###Int32
this.lblQuantity.AutoSize = true;
this.lblQuantity.Location = new System.Drawing.Point(100,25);
this.lblQuantity.Name = "lblQuantity";
this.lblQuantity.Size = new System.Drawing.Size(41, 12);
this.lblQuantity.TabIndex = 1;
this.lblQuantity.Text = "";
this.txtQuantity.Location = new System.Drawing.Point(173,21);
this.txtQuantity.Name = "txtQuantity";
this.txtQuantity.Size = new System.Drawing.Size(100, 21);
this.txtQuantity.TabIndex = 1;
this.Controls.Add(this.lblQuantity);
this.Controls.Add(this.txtQuantity);

           //#####Unit_price###Decimal
this.lblUnit_price.AutoSize = true;
this.lblUnit_price.Location = new System.Drawing.Point(100,50);
this.lblUnit_price.Name = "lblUnit_price";
this.lblUnit_price.Size = new System.Drawing.Size(41, 12);
this.lblUnit_price.TabIndex = 2;
this.lblUnit_price.Text = "";
//111======50
this.txtUnit_price.Location = new System.Drawing.Point(173,46);
this.txtUnit_price.Name ="txtUnit_price";
this.txtUnit_price.Size = new System.Drawing.Size(100, 21);
this.txtUnit_price.TabIndex = 2;
this.Controls.Add(this.lblUnit_price);
this.Controls.Add(this.txtUnit_price);

           //#####Total_amount###Decimal
this.lblTotal_amount.AutoSize = true;
this.lblTotal_amount.Location = new System.Drawing.Point(100,75);
this.lblTotal_amount.Name = "lblTotal_amount";
this.lblTotal_amount.Size = new System.Drawing.Size(41, 12);
this.lblTotal_amount.TabIndex = 3;
this.lblTotal_amount.Text = "";
//111======75
this.txtTotal_amount.Location = new System.Drawing.Point(173,71);
this.txtTotal_amount.Name ="txtTotal_amount";
this.txtTotal_amount.Size = new System.Drawing.Size(100, 21);
this.txtTotal_amount.TabIndex = 3;
this.Controls.Add(this.lblTotal_amount);
this.Controls.Add(this.txtTotal_amount);

           //#####Status###Int32
this.lblStatus.AutoSize = true;
this.lblStatus.Location = new System.Drawing.Point(100,100);
this.lblStatus.Name = "lblStatus";
this.lblStatus.Size = new System.Drawing.Size(41, 12);
this.lblStatus.TabIndex = 4;
this.lblStatus.Text = "";
this.txtStatus.Location = new System.Drawing.Point(173,96);
this.txtStatus.Name = "txtStatus";
this.txtStatus.Size = new System.Drawing.Size(100, 21);
this.txtStatus.TabIndex = 4;
this.Controls.Add(this.lblStatus);
this.Controls.Add(this.txtStatus);

           //#####Order_date###DateTime
this.lblOrder_date.AutoSize = true;
this.lblOrder_date.Location = new System.Drawing.Point(100,125);
this.lblOrder_date.Name = "lblOrder_date";
this.lblOrder_date.Size = new System.Drawing.Size(41, 12);
this.lblOrder_date.TabIndex = 5;
this.lblOrder_date.Text = "";
//111======125
this.dtpOrder_date.Location = new System.Drawing.Point(173,121);
this.dtpOrder_date.Name ="dtpOrder_date";
this.dtpOrder_date.ShowCheckBox =true;
this.dtpOrder_date.Size = new System.Drawing.Size(100, 21);
this.dtpOrder_date.TabIndex = 5;
this.Controls.Add(this.lblOrder_date);
this.Controls.Add(this.dtpOrder_date);

           //#####Delivery_date###DateTime
this.lblDelivery_date.AutoSize = true;
this.lblDelivery_date.Location = new System.Drawing.Point(100,150);
this.lblDelivery_date.Name = "lblDelivery_date";
this.lblDelivery_date.Size = new System.Drawing.Size(41, 12);
this.lblDelivery_date.TabIndex = 6;
this.lblDelivery_date.Text = "";
//111======150
this.dtpDelivery_date.Location = new System.Drawing.Point(173,146);
this.dtpDelivery_date.Name ="dtpDelivery_date";
this.dtpDelivery_date.ShowCheckBox =true;
this.dtpDelivery_date.Size = new System.Drawing.Size(100, 21);
this.dtpDelivery_date.TabIndex = 6;
this.Controls.Add(this.lblDelivery_date);
this.Controls.Add(this.dtpDelivery_date);

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
                this.Controls.Add(this.lblQuantity );
this.Controls.Add(this.txtQuantity );

                this.Controls.Add(this.lblUnit_price );
this.Controls.Add(this.txtUnit_price );

                this.Controls.Add(this.lblTotal_amount );
this.Controls.Add(this.txtTotal_amount );

                this.Controls.Add(this.lblStatus );
this.Controls.Add(this.txtStatus );

                this.Controls.Add(this.lblOrder_date );
this.Controls.Add(this.dtpOrder_date );

                this.Controls.Add(this.lblDelivery_date );
this.Controls.Add(this.dtpDelivery_date );

                            // 
            // "tb_Outsourcing_orderEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_Outsourcing_orderEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblQuantity;
private Krypton.Toolkit.KryptonTextBox txtQuantity;

    
        
              private Krypton.Toolkit.KryptonLabel lblUnit_price;
private Krypton.Toolkit.KryptonTextBox txtUnit_price;

    
        
              private Krypton.Toolkit.KryptonLabel lblTotal_amount;
private Krypton.Toolkit.KryptonTextBox txtTotal_amount;

    
        
              private Krypton.Toolkit.KryptonLabel lblStatus;
private Krypton.Toolkit.KryptonTextBox txtStatus;

    
        
              private Krypton.Toolkit.KryptonLabel lblOrder_date;
private Krypton.Toolkit.KryptonDateTimePicker dtpOrder_date;

    
        
              private Krypton.Toolkit.KryptonLabel lblDelivery_date;
private Krypton.Toolkit.KryptonDateTimePicker dtpDelivery_date;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

