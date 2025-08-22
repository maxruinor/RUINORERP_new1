
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/20/2025 16:08:15
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 对账单明细
    /// </summary>
    partial class tb_FM_StatementDetailQuery
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
     
     this.lblStatementId = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbStatementId = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblARAPId = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbARAPId = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();


this.lblCurrency_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.cmbCurrency_ID = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();

this.lblExchangeRate = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtExchangeRate = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblIncludedLocalAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtIncludedLocalAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblIncludedForeignAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtIncludedForeignAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblWrittenOffLocalAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtWrittenOffLocalAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblWrittenOffForeignAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtWrittenOffForeignAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblRemainingLocalAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtRemainingLocalAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();

this.lblRemainingForeignAmount = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtRemainingForeignAmount = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();


this.lblSummary = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
this.txtSummary = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
this.txtSummary.Multiline = true;

    //for end
    this.SuspendLayout();
    
         //for start
                 //#####StatementId###Int64
//属性测试25StatementId
//属性测试25StatementId
//属性测试25StatementId
this.lblStatementId.AutoSize = true;
this.lblStatementId.Location = new System.Drawing.Point(100,25);
this.lblStatementId.Name = "lblStatementId";
this.lblStatementId.Size = new System.Drawing.Size(41, 12);
this.lblStatementId.TabIndex = 1;
this.lblStatementId.Text = "对账单";
//111======25
this.cmbStatementId.Location = new System.Drawing.Point(173,21);
this.cmbStatementId.Name ="cmbStatementId";
this.cmbStatementId.Size = new System.Drawing.Size(100, 21);
this.cmbStatementId.TabIndex = 1;
this.Controls.Add(this.lblStatementId);
this.Controls.Add(this.cmbStatementId);

           //#####ARAPId###Int64
//属性测试50ARAPId
//属性测试50ARAPId
this.lblARAPId.AutoSize = true;
this.lblARAPId.Location = new System.Drawing.Point(100,50);
this.lblARAPId.Name = "lblARAPId";
this.lblARAPId.Size = new System.Drawing.Size(41, 12);
this.lblARAPId.TabIndex = 2;
this.lblARAPId.Text = "应收付款单";
//111======50
this.cmbARAPId.Location = new System.Drawing.Point(173,46);
this.cmbARAPId.Name ="cmbARAPId";
this.cmbARAPId.Size = new System.Drawing.Size(100, 21);
this.cmbARAPId.TabIndex = 2;
this.Controls.Add(this.lblARAPId);
this.Controls.Add(this.cmbARAPId);

           //#####ReceivePaymentType###Int32
//属性测试75ReceivePaymentType
//属性测试75ReceivePaymentType
//属性测试75ReceivePaymentType

           //#####Currency_ID###Int64
//属性测试100Currency_ID
this.lblCurrency_ID.AutoSize = true;
this.lblCurrency_ID.Location = new System.Drawing.Point(100,100);
this.lblCurrency_ID.Name = "lblCurrency_ID";
this.lblCurrency_ID.Size = new System.Drawing.Size(41, 12);
this.lblCurrency_ID.TabIndex = 4;
this.lblCurrency_ID.Text = "币别";
//111======100
this.cmbCurrency_ID.Location = new System.Drawing.Point(173,96);
this.cmbCurrency_ID.Name ="cmbCurrency_ID";
this.cmbCurrency_ID.Size = new System.Drawing.Size(100, 21);
this.cmbCurrency_ID.TabIndex = 4;
this.Controls.Add(this.lblCurrency_ID);
this.Controls.Add(this.cmbCurrency_ID);

           //#####ExchangeRate###Decimal
this.lblExchangeRate.AutoSize = true;
this.lblExchangeRate.Location = new System.Drawing.Point(100,125);
this.lblExchangeRate.Name = "lblExchangeRate";
this.lblExchangeRate.Size = new System.Drawing.Size(41, 12);
this.lblExchangeRate.TabIndex = 5;
this.lblExchangeRate.Text = "汇率";
//111======125
this.txtExchangeRate.Location = new System.Drawing.Point(173,121);
this.txtExchangeRate.Name ="txtExchangeRate";
this.txtExchangeRate.Size = new System.Drawing.Size(100, 21);
this.txtExchangeRate.TabIndex = 5;
this.Controls.Add(this.lblExchangeRate);
this.Controls.Add(this.txtExchangeRate);

           //#####IncludedLocalAmount###Decimal
this.lblIncludedLocalAmount.AutoSize = true;
this.lblIncludedLocalAmount.Location = new System.Drawing.Point(100,150);
this.lblIncludedLocalAmount.Name = "lblIncludedLocalAmount";
this.lblIncludedLocalAmount.Size = new System.Drawing.Size(41, 12);
this.lblIncludedLocalAmount.TabIndex = 6;
this.lblIncludedLocalAmount.Text = "对账金额本币";
//111======150
this.txtIncludedLocalAmount.Location = new System.Drawing.Point(173,146);
this.txtIncludedLocalAmount.Name ="txtIncludedLocalAmount";
this.txtIncludedLocalAmount.Size = new System.Drawing.Size(100, 21);
this.txtIncludedLocalAmount.TabIndex = 6;
this.Controls.Add(this.lblIncludedLocalAmount);
this.Controls.Add(this.txtIncludedLocalAmount);

           //#####IncludedForeignAmount###Decimal
this.lblIncludedForeignAmount.AutoSize = true;
this.lblIncludedForeignAmount.Location = new System.Drawing.Point(100,175);
this.lblIncludedForeignAmount.Name = "lblIncludedForeignAmount";
this.lblIncludedForeignAmount.Size = new System.Drawing.Size(41, 12);
this.lblIncludedForeignAmount.TabIndex = 7;
this.lblIncludedForeignAmount.Text = "对账金额外币";
//111======175
this.txtIncludedForeignAmount.Location = new System.Drawing.Point(173,171);
this.txtIncludedForeignAmount.Name ="txtIncludedForeignAmount";
this.txtIncludedForeignAmount.Size = new System.Drawing.Size(100, 21);
this.txtIncludedForeignAmount.TabIndex = 7;
this.Controls.Add(this.lblIncludedForeignAmount);
this.Controls.Add(this.txtIncludedForeignAmount);

           //#####WrittenOffLocalAmount###Decimal
this.lblWrittenOffLocalAmount.AutoSize = true;
this.lblWrittenOffLocalAmount.Location = new System.Drawing.Point(100,200);
this.lblWrittenOffLocalAmount.Name = "lblWrittenOffLocalAmount";
this.lblWrittenOffLocalAmount.Size = new System.Drawing.Size(41, 12);
this.lblWrittenOffLocalAmount.TabIndex = 8;
this.lblWrittenOffLocalAmount.Text = "本次已核销本币金额";
//111======200
this.txtWrittenOffLocalAmount.Location = new System.Drawing.Point(173,196);
this.txtWrittenOffLocalAmount.Name ="txtWrittenOffLocalAmount";
this.txtWrittenOffLocalAmount.Size = new System.Drawing.Size(100, 21);
this.txtWrittenOffLocalAmount.TabIndex = 8;
this.Controls.Add(this.lblWrittenOffLocalAmount);
this.Controls.Add(this.txtWrittenOffLocalAmount);

           //#####WrittenOffForeignAmount###Decimal
this.lblWrittenOffForeignAmount.AutoSize = true;
this.lblWrittenOffForeignAmount.Location = new System.Drawing.Point(100,225);
this.lblWrittenOffForeignAmount.Name = "lblWrittenOffForeignAmount";
this.lblWrittenOffForeignAmount.Size = new System.Drawing.Size(41, 12);
this.lblWrittenOffForeignAmount.TabIndex = 9;
this.lblWrittenOffForeignAmount.Text = "本次已核销原币金额";
//111======225
this.txtWrittenOffForeignAmount.Location = new System.Drawing.Point(173,221);
this.txtWrittenOffForeignAmount.Name ="txtWrittenOffForeignAmount";
this.txtWrittenOffForeignAmount.Size = new System.Drawing.Size(100, 21);
this.txtWrittenOffForeignAmount.TabIndex = 9;
this.Controls.Add(this.lblWrittenOffForeignAmount);
this.Controls.Add(this.txtWrittenOffForeignAmount);

           //#####RemainingLocalAmount###Decimal
this.lblRemainingLocalAmount.AutoSize = true;
this.lblRemainingLocalAmount.Location = new System.Drawing.Point(100,250);
this.lblRemainingLocalAmount.Name = "lblRemainingLocalAmount";
this.lblRemainingLocalAmount.Size = new System.Drawing.Size(41, 12);
this.lblRemainingLocalAmount.TabIndex = 10;
this.lblRemainingLocalAmount.Text = "剩余未核销本币金额";
//111======250
this.txtRemainingLocalAmount.Location = new System.Drawing.Point(173,246);
this.txtRemainingLocalAmount.Name ="txtRemainingLocalAmount";
this.txtRemainingLocalAmount.Size = new System.Drawing.Size(100, 21);
this.txtRemainingLocalAmount.TabIndex = 10;
this.Controls.Add(this.lblRemainingLocalAmount);
this.Controls.Add(this.txtRemainingLocalAmount);

           //#####RemainingForeignAmount###Decimal
this.lblRemainingForeignAmount.AutoSize = true;
this.lblRemainingForeignAmount.Location = new System.Drawing.Point(100,275);
this.lblRemainingForeignAmount.Name = "lblRemainingForeignAmount";
this.lblRemainingForeignAmount.Size = new System.Drawing.Size(41, 12);
this.lblRemainingForeignAmount.TabIndex = 11;
this.lblRemainingForeignAmount.Text = "剩余未核销原币金额";
//111======275
this.txtRemainingForeignAmount.Location = new System.Drawing.Point(173,271);
this.txtRemainingForeignAmount.Name ="txtRemainingForeignAmount";
this.txtRemainingForeignAmount.Size = new System.Drawing.Size(100, 21);
this.txtRemainingForeignAmount.TabIndex = 11;
this.Controls.Add(this.lblRemainingForeignAmount);
this.Controls.Add(this.txtRemainingForeignAmount);

           //#####ARAPWriteOffStatus###Int32
//属性测试300ARAPWriteOffStatus
//属性测试300ARAPWriteOffStatus
//属性测试300ARAPWriteOffStatus

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

          
    //for end

            //components = new System.ComponentModel.Container();
            
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblStatementId );
this.Controls.Add(this.cmbStatementId );

                this.Controls.Add(this.lblARAPId );
this.Controls.Add(this.cmbARAPId );

                
                this.Controls.Add(this.lblCurrency_ID );
this.Controls.Add(this.cmbCurrency_ID );

                this.Controls.Add(this.lblExchangeRate );
this.Controls.Add(this.txtExchangeRate );

                this.Controls.Add(this.lblIncludedLocalAmount );
this.Controls.Add(this.txtIncludedLocalAmount );

                this.Controls.Add(this.lblIncludedForeignAmount );
this.Controls.Add(this.txtIncludedForeignAmount );

                this.Controls.Add(this.lblWrittenOffLocalAmount );
this.Controls.Add(this.txtWrittenOffLocalAmount );

                this.Controls.Add(this.lblWrittenOffForeignAmount );
this.Controls.Add(this.txtWrittenOffForeignAmount );

                this.Controls.Add(this.lblRemainingLocalAmount );
this.Controls.Add(this.txtRemainingLocalAmount );

                this.Controls.Add(this.lblRemainingForeignAmount );
this.Controls.Add(this.txtRemainingForeignAmount );

                
                this.Controls.Add(this.lblSummary );
this.Controls.Add(this.txtSummary );

                    
            this.Name = "tb_FM_StatementDetailQuery";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblStatementId;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbStatementId;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblARAPId;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbARAPId;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblCurrency_ID;
private ComponentFactory.Krypton.Toolkit.KryptonComboBox cmbCurrency_ID;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblExchangeRate;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtExchangeRate;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIncludedLocalAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtIncludedLocalAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblIncludedForeignAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtIncludedForeignAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblWrittenOffLocalAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtWrittenOffLocalAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblWrittenOffForeignAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtWrittenOffForeignAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRemainingLocalAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtRemainingLocalAmount;

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRemainingForeignAmount;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtRemainingForeignAmount;

    
        
              
    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSummary;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSummary;

    
    
   
 





    }
}


