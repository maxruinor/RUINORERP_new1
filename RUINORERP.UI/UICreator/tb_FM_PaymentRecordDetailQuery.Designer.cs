
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：05/07/2025 15:37:42
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 收款单明细表：记录收款分配到应收单的明细
    /// </summary>
    partial class tb_FM_PaymentRecordDetailQuery
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
     
     this.lblPaymentId = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbPaymentId = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();



this.lblSourceBillNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSourceBillNo = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblExchangeRate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtExchangeRate = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblForeignAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtForeignAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblLocalAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtLocalAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblSummary = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSummary = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtSummary.Multiline = true;

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####PaymentId###Int64
//属性测试25PaymentId
this.lblPaymentId.AutoSize = true;
this.lblPaymentId.Location = new System.Drawing.Point(100,25);
this.lblPaymentId.Name = "lblPaymentId";
this.lblPaymentId.Size = new System.Drawing.Size(41, 12);
this.lblPaymentId.TabIndex = 1;
this.lblPaymentId.Text = "付款单";
//111======25
this.cmbPaymentId.Location = new System.Drawing.Point(173,21);
this.cmbPaymentId.Name ="cmbPaymentId";
this.cmbPaymentId.Size = new System.Drawing.Size(100, 21);
this.cmbPaymentId.TabIndex = 1;
this.Controls.Add(this.lblPaymentId);
this.Controls.Add(this.cmbPaymentId);

           //#####SourceBizType###Int32
//属性测试50SourceBizType

           //#####SourceBilllId###Int64
//属性测试75SourceBilllId

           //#####30SourceBillNo###String
this.lblSourceBillNo.AutoSize = true;
this.lblSourceBillNo.Location = new System.Drawing.Point(100,100);
this.lblSourceBillNo.Name = "lblSourceBillNo";
this.lblSourceBillNo.Size = new System.Drawing.Size(41, 12);
this.lblSourceBillNo.TabIndex = 4;
this.lblSourceBillNo.Text = "来源单号";
this.txtSourceBillNo.Location = new System.Drawing.Point(173,96);
this.txtSourceBillNo.Name = "txtSourceBillNo";
this.txtSourceBillNo.Size = new System.Drawing.Size(100, 21);
this.txtSourceBillNo.TabIndex = 4;
this.Controls.Add(this.lblSourceBillNo);
this.Controls.Add(this.txtSourceBillNo);

           //#####Currency_ID###Int64
//属性测试125Currency_ID

           //#####ExchangeRate###Decimal
this.lblExchangeRate.AutoSize = true;
this.lblExchangeRate.Location = new System.Drawing.Point(100,150);
this.lblExchangeRate.Name = "lblExchangeRate";
this.lblExchangeRate.Size = new System.Drawing.Size(41, 12);
this.lblExchangeRate.TabIndex = 6;
this.lblExchangeRate.Text = "汇率";
//111======150
this.txtExchangeRate.Location = new System.Drawing.Point(173,146);
this.txtExchangeRate.Name ="txtExchangeRate";
this.txtExchangeRate.Size = new System.Drawing.Size(100, 21);
this.txtExchangeRate.TabIndex = 6;
this.Controls.Add(this.lblExchangeRate);
this.Controls.Add(this.txtExchangeRate);

           //#####ForeignAmount###Decimal
this.lblForeignAmount.AutoSize = true;
this.lblForeignAmount.Location = new System.Drawing.Point(100,175);
this.lblForeignAmount.Name = "lblForeignAmount";
this.lblForeignAmount.Size = new System.Drawing.Size(41, 12);
this.lblForeignAmount.TabIndex = 7;
this.lblForeignAmount.Text = "支付金额外币";
//111======175
this.txtForeignAmount.Location = new System.Drawing.Point(173,171);
this.txtForeignAmount.Name ="txtForeignAmount";
this.txtForeignAmount.Size = new System.Drawing.Size(100, 21);
this.txtForeignAmount.TabIndex = 7;
this.Controls.Add(this.lblForeignAmount);
this.Controls.Add(this.txtForeignAmount);

           //#####LocalAmount###Decimal
this.lblLocalAmount.AutoSize = true;
this.lblLocalAmount.Location = new System.Drawing.Point(100,200);
this.lblLocalAmount.Name = "lblLocalAmount";
this.lblLocalAmount.Size = new System.Drawing.Size(41, 12);
this.lblLocalAmount.TabIndex = 8;
this.lblLocalAmount.Text = "支付金额本币";
//111======200
this.txtLocalAmount.Location = new System.Drawing.Point(173,196);
this.txtLocalAmount.Name ="txtLocalAmount";
this.txtLocalAmount.Size = new System.Drawing.Size(100, 21);
this.txtLocalAmount.TabIndex = 8;
this.Controls.Add(this.lblLocalAmount);
this.Controls.Add(this.txtLocalAmount);

           //#####300Summary###String
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

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblPaymentId );
this.Controls.Add(this.cmbPaymentId );

                
                
                this.Controls.Add(this.lblSourceBillNo );
this.Controls.Add(this.txtSourceBillNo );

                
                this.Controls.Add(this.lblExchangeRate );
this.Controls.Add(this.txtExchangeRate );

                this.Controls.Add(this.lblForeignAmount );
this.Controls.Add(this.txtForeignAmount );

                this.Controls.Add(this.lblLocalAmount );
this.Controls.Add(this.txtLocalAmount );

                this.Controls.Add(this.lblSummary );
this.Controls.Add(this.txtSummary );

                    
            this.Name = "tb_FM_PaymentRecordDetailQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPaymentId;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbPaymentId;

    
        
              
    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSourceBillNo;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSourceBillNo;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblExchangeRate;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtExchangeRate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblForeignAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtForeignAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblLocalAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtLocalAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSummary;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSummary;

    
    
   
 





    }
}


