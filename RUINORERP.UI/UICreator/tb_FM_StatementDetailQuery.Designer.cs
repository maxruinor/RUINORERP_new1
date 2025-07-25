
// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：07/24/2025 20:27:24
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 对账单明细（关联应收单） 
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

           //#####Currency_ID###Int64
//属性测试75Currency_ID
this.lblCurrency_ID.AutoSize = true;
this.lblCurrency_ID.Location = new System.Drawing.Point(100,75);
this.lblCurrency_ID.Name = "lblCurrency_ID";
this.lblCurrency_ID.Size = new System.Drawing.Size(41, 12);
this.lblCurrency_ID.TabIndex = 3;
this.lblCurrency_ID.Text = "币别";
//111======75
this.cmbCurrency_ID.Location = new System.Drawing.Point(173,71);
this.cmbCurrency_ID.Name ="cmbCurrency_ID";
this.cmbCurrency_ID.Size = new System.Drawing.Size(100, 21);
this.cmbCurrency_ID.TabIndex = 3;
this.Controls.Add(this.lblCurrency_ID);
this.Controls.Add(this.cmbCurrency_ID);

           //#####ExchangeRate###Decimal
this.lblExchangeRate.AutoSize = true;
this.lblExchangeRate.Location = new System.Drawing.Point(100,100);
this.lblExchangeRate.Name = "lblExchangeRate";
this.lblExchangeRate.Size = new System.Drawing.Size(41, 12);
this.lblExchangeRate.TabIndex = 4;
this.lblExchangeRate.Text = "汇率";
//111======100
this.txtExchangeRate.Location = new System.Drawing.Point(173,96);
this.txtExchangeRate.Name ="txtExchangeRate";
this.txtExchangeRate.Size = new System.Drawing.Size(100, 21);
this.txtExchangeRate.TabIndex = 4;
this.Controls.Add(this.lblExchangeRate);
this.Controls.Add(this.txtExchangeRate);

           //#####IncludedLocalAmount###Decimal
this.lblIncludedLocalAmount.AutoSize = true;
this.lblIncludedLocalAmount.Location = new System.Drawing.Point(100,125);
this.lblIncludedLocalAmount.Name = "lblIncludedLocalAmount";
this.lblIncludedLocalAmount.Size = new System.Drawing.Size(41, 12);
this.lblIncludedLocalAmount.TabIndex = 5;
this.lblIncludedLocalAmount.Text = "对账金额本币";
//111======125
this.txtIncludedLocalAmount.Location = new System.Drawing.Point(173,121);
this.txtIncludedLocalAmount.Name ="txtIncludedLocalAmount";
this.txtIncludedLocalAmount.Size = new System.Drawing.Size(100, 21);
this.txtIncludedLocalAmount.TabIndex = 5;
this.Controls.Add(this.lblIncludedLocalAmount);
this.Controls.Add(this.txtIncludedLocalAmount);

           //#####IncludedForeignAmount###Decimal
this.lblIncludedForeignAmount.AutoSize = true;
this.lblIncludedForeignAmount.Location = new System.Drawing.Point(100,150);
this.lblIncludedForeignAmount.Name = "lblIncludedForeignAmount";
this.lblIncludedForeignAmount.Size = new System.Drawing.Size(41, 12);
this.lblIncludedForeignAmount.TabIndex = 6;
this.lblIncludedForeignAmount.Text = "对账金额外币";
//111======150
this.txtIncludedForeignAmount.Location = new System.Drawing.Point(173,146);
this.txtIncludedForeignAmount.Name ="txtIncludedForeignAmount";
this.txtIncludedForeignAmount.Size = new System.Drawing.Size(100, 21);
this.txtIncludedForeignAmount.TabIndex = 6;
this.Controls.Add(this.lblIncludedForeignAmount);
this.Controls.Add(this.txtIncludedForeignAmount);

           //#####300Summary###String
this.lblSummary.AutoSize = true;
this.lblSummary.Location = new System.Drawing.Point(100,175);
this.lblSummary.Name = "lblSummary";
this.lblSummary.Size = new System.Drawing.Size(41, 12);
this.lblSummary.TabIndex = 7;
this.lblSummary.Text = "摘要";
this.txtSummary.Location = new System.Drawing.Point(173,171);
this.txtSummary.Name = "txtSummary";
this.txtSummary.Size = new System.Drawing.Size(100, 21);
this.txtSummary.TabIndex = 7;
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

    
        
              private ComponentFactory.Krypton.Toolkit.KryptonLabel lblSummary;
private ComponentFactory.Krypton.Toolkit.KryptonTextBox txtSummary;

    
    
   
 





    }
}


