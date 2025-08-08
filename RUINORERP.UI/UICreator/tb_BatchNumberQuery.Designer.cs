
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:11
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 批次表 在采购入库时和出库时保存批次ID
    /// </summary>
    partial class tb_BatchNumberQuery
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
     
     this.lblBatchNO = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtBatchNO = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lbl采购单号 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txt采购单号 = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lbl入库日期 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtp入库日期 = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();


this.lbl采购单价 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txt采购单价 = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblexpiry_date = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpexpiry_date = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblproduction_date = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.dtpproduction_date = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();

this.lblsale_price = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtsale_price = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


    //for end
    this.SuspendLayout();
    
         //for start
                 //#####50BatchNO###String
this.lblBatchNO.AutoSize = true;
this.lblBatchNO.Location = new System.Drawing.Point(100,25);
this.lblBatchNO.Name = "lblBatchNO";
this.lblBatchNO.Size = new System.Drawing.Size(41, 12);
this.lblBatchNO.TabIndex = 1;
this.lblBatchNO.Text = "";
this.txtBatchNO.Location = new System.Drawing.Point(173,21);
this.txtBatchNO.Name = "txtBatchNO";
this.txtBatchNO.Size = new System.Drawing.Size(100, 21);
this.txtBatchNO.TabIndex = 1;
this.Controls.Add(this.lblBatchNO);
this.Controls.Add(this.txtBatchNO);

           //#####20采购单号###String
this.lbl采购单号.AutoSize = true;
this.lbl采购单号.Location = new System.Drawing.Point(100,50);
this.lbl采购单号.Name = "lbl采购单号";
this.lbl采购单号.Size = new System.Drawing.Size(41, 12);
this.lbl采购单号.TabIndex = 2;
this.lbl采购单号.Text = "";
this.txt采购单号.Location = new System.Drawing.Point(173,46);
this.txt采购单号.Name = "txt采购单号";
this.txt采购单号.Size = new System.Drawing.Size(100, 21);
this.txt采购单号.TabIndex = 2;
this.Controls.Add(this.lbl采购单号);
this.Controls.Add(this.txt采购单号);

           //#####入库日期###DateTime
this.lbl入库日期.AutoSize = true;
this.lbl入库日期.Location = new System.Drawing.Point(100,75);
this.lbl入库日期.Name = "lbl入库日期";
this.lbl入库日期.Size = new System.Drawing.Size(41, 12);
this.lbl入库日期.TabIndex = 3;
this.lbl入库日期.Text = "";
//111======75
this.dtp入库日期.Location = new System.Drawing.Point(173,71);
this.dtp入库日期.Name ="dtp入库日期";
this.dtp入库日期.ShowCheckBox =true;
this.dtp入库日期.Size = new System.Drawing.Size(100, 21);
this.dtp入库日期.TabIndex = 3;
this.Controls.Add(this.lbl入库日期);
this.Controls.Add(this.dtp入库日期);

           //#####供应商###Int32

           //#####采购单价###Decimal
this.lbl采购单价.AutoSize = true;
this.lbl采购单价.Location = new System.Drawing.Point(100,125);
this.lbl采购单价.Name = "lbl采购单价";
this.lbl采购单价.Size = new System.Drawing.Size(41, 12);
this.lbl采购单价.TabIndex = 5;
this.lbl采购单价.Text = "";
//111======125
this.txt采购单价.Location = new System.Drawing.Point(173,121);
this.txt采购单价.Name ="txt采购单价";
this.txt采购单价.Size = new System.Drawing.Size(100, 21);
this.txt采购单价.TabIndex = 5;
this.Controls.Add(this.lbl采购单价);
this.Controls.Add(this.txt采购单价);

           //#####expiry_date###DateTime
this.lblexpiry_date.AutoSize = true;
this.lblexpiry_date.Location = new System.Drawing.Point(100,150);
this.lblexpiry_date.Name = "lblexpiry_date";
this.lblexpiry_date.Size = new System.Drawing.Size(41, 12);
this.lblexpiry_date.TabIndex = 6;
this.lblexpiry_date.Text = "过期日期";
//111======150
this.dtpexpiry_date.Location = new System.Drawing.Point(173,146);
this.dtpexpiry_date.Name ="dtpexpiry_date";
this.dtpexpiry_date.ShowCheckBox =true;
this.dtpexpiry_date.Size = new System.Drawing.Size(100, 21);
this.dtpexpiry_date.TabIndex = 6;
this.Controls.Add(this.lblexpiry_date);
this.Controls.Add(this.dtpexpiry_date);

           //#####production_date###DateTime
this.lblproduction_date.AutoSize = true;
this.lblproduction_date.Location = new System.Drawing.Point(100,175);
this.lblproduction_date.Name = "lblproduction_date";
this.lblproduction_date.Size = new System.Drawing.Size(41, 12);
this.lblproduction_date.TabIndex = 7;
this.lblproduction_date.Text = "";
//111======175
this.dtpproduction_date.Location = new System.Drawing.Point(173,171);
this.dtpproduction_date.Name ="dtpproduction_date";
this.dtpproduction_date.ShowCheckBox =true;
this.dtpproduction_date.Size = new System.Drawing.Size(100, 21);
this.dtpproduction_date.TabIndex = 7;
this.Controls.Add(this.lblproduction_date);
this.Controls.Add(this.dtpproduction_date);

           //#####sale_price###Decimal
this.lblsale_price.AutoSize = true;
this.lblsale_price.Location = new System.Drawing.Point(100,200);
this.lblsale_price.Name = "lblsale_price";
this.lblsale_price.Size = new System.Drawing.Size(41, 12);
this.lblsale_price.TabIndex = 8;
this.lblsale_price.Text = "";
//111======200
this.txtsale_price.Location = new System.Drawing.Point(173,196);
this.txtsale_price.Name ="txtsale_price";
this.txtsale_price.Size = new System.Drawing.Size(100, 21);
this.txtsale_price.TabIndex = 8;
this.Controls.Add(this.lblsale_price);
this.Controls.Add(this.txtsale_price);

           //#####quantity###Int32

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblBatchNO );
this.Controls.Add(this.txtBatchNO );

                this.Controls.Add(this.lbl采购单号 );
this.Controls.Add(this.txt采购单号 );

                this.Controls.Add(this.lbl入库日期 );
this.Controls.Add(this.dtp入库日期 );

                
                this.Controls.Add(this.lbl采购单价 );
this.Controls.Add(this.txt采购单价 );

                this.Controls.Add(this.lblexpiry_date );
this.Controls.Add(this.dtpexpiry_date );

                this.Controls.Add(this.lblproduction_date );
this.Controls.Add(this.dtpproduction_date );

                this.Controls.Add(this.lblsale_price );
this.Controls.Add(this.txtsale_price );

                
                    
            this.Name = "tb_BatchNumberQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblBatchNO;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtBatchNO;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lbl采购单号;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txt采购单号;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lbl入库日期;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtp入库日期;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lbl采购单价;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txt采购单价;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblexpiry_date;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpexpiry_date;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblproduction_date;
private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dtpproduction_date;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblsale_price;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtsale_price;

    
        
              
    
    
   
 





    }
}


