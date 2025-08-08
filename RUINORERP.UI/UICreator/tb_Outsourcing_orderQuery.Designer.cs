
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
    partial class tb_Outsourcing_orderQuery
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
     
     
this.lblUnit_price = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtUnit_price = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblTotal_amount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtTotal_amount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblOrder_date = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpOrder_date = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblDelivery_date = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpDelivery_date = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####Quantity###Int32

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

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                
                this.Controls.Add(this.lblUnit_price );
this.Controls.Add(this.txtUnit_price );

                this.Controls.Add(this.lblTotal_amount );
this.Controls.Add(this.txtTotal_amount );

                
                this.Controls.Add(this.lblOrder_date );
this.Controls.Add(this.dtpOrder_date );

                this.Controls.Add(this.lblDelivery_date );
this.Controls.Add(this.dtpDelivery_date );

                    
            this.Name = "tb_Outsourcing_orderQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblUnit_price;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtUnit_price;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotal_amount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtTotal_amount;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblOrder_date;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpOrder_date;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblDelivery_date;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpDelivery_date;

    
    
   
 





    }
}


