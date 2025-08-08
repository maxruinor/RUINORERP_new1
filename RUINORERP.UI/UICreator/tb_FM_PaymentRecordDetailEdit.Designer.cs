// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：08/08/2025 13:45:31
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 收付款记录明细表
    /// </summary>
    partial class tb_FM_PaymentRecordDetailEdit
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
     this.lblPaymentId = new Krypton.Toolkit.KryptonLabel();
this.cmbPaymentId = new Krypton.Toolkit.KryptonComboBox();

this.lblSourceBizType = new Krypton.Toolkit.KryptonLabel();
this.txtSourceBizType = new Krypton.Toolkit.KryptonTextBox();

this.lblSourceBilllId = new Krypton.Toolkit.KryptonLabel();
this.txtSourceBilllId = new Krypton.Toolkit.KryptonTextBox();

this.lblSourceBillNo = new Krypton.Toolkit.KryptonLabel();
this.txtSourceBillNo = new Krypton.Toolkit.KryptonTextBox();

this.lblIsFromPlatform = new Krypton.Toolkit.KryptonLabel();
this.chkIsFromPlatform = new Krypton.Toolkit.KryptonCheckBox();
this.chkIsFromPlatform.Values.Text ="";

this.lblDepartmentID = new Krypton.Toolkit.KryptonLabel();
this.cmbDepartmentID = new Krypton.Toolkit.KryptonComboBox();

this.lblProjectGroup_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbProjectGroup_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblCurrency_ID = new Krypton.Toolkit.KryptonLabel();
this.cmbCurrency_ID = new Krypton.Toolkit.KryptonComboBox();

this.lblExchangeRate = new Krypton.Toolkit.KryptonLabel();
this.txtExchangeRate = new Krypton.Toolkit.KryptonTextBox();

this.lblForeignAmount = new Krypton.Toolkit.KryptonLabel();
this.txtForeignAmount = new Krypton.Toolkit.KryptonTextBox();

this.lblLocalAmount = new Krypton.Toolkit.KryptonLabel();
this.txtLocalAmount = new Krypton.Toolkit.KryptonTextBox();

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
     
            //#####PaymentId###Int64
//属性测试25PaymentId
//属性测试25PaymentId
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
//属性测试50SourceBizType
//属性测试50SourceBizType
//属性测试50SourceBizType
this.lblSourceBizType.AutoSize = true;
this.lblSourceBizType.Location = new System.Drawing.Point(100,50);
this.lblSourceBizType.Name = "lblSourceBizType";
this.lblSourceBizType.Size = new System.Drawing.Size(41, 12);
this.lblSourceBizType.TabIndex = 2;
this.lblSourceBizType.Text = "来源业务";
this.txtSourceBizType.Location = new System.Drawing.Point(173,46);
this.txtSourceBizType.Name = "txtSourceBizType";
this.txtSourceBizType.Size = new System.Drawing.Size(100, 21);
this.txtSourceBizType.TabIndex = 2;
this.Controls.Add(this.lblSourceBizType);
this.Controls.Add(this.txtSourceBizType);

           //#####SourceBilllId###Int64
//属性测试75SourceBilllId
//属性测试75SourceBilllId
//属性测试75SourceBilllId
//属性测试75SourceBilllId
this.lblSourceBilllId.AutoSize = true;
this.lblSourceBilllId.Location = new System.Drawing.Point(100,75);
this.lblSourceBilllId.Name = "lblSourceBilllId";
this.lblSourceBilllId.Size = new System.Drawing.Size(41, 12);
this.lblSourceBilllId.TabIndex = 3;
this.lblSourceBilllId.Text = "来源单据";
this.txtSourceBilllId.Location = new System.Drawing.Point(173,71);
this.txtSourceBilllId.Name = "txtSourceBilllId";
this.txtSourceBilllId.Size = new System.Drawing.Size(100, 21);
this.txtSourceBilllId.TabIndex = 3;
this.Controls.Add(this.lblSourceBilllId);
this.Controls.Add(this.txtSourceBilllId);

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

           //#####IsFromPlatform###Boolean
this.lblIsFromPlatform.AutoSize = true;
this.lblIsFromPlatform.Location = new System.Drawing.Point(100,125);
this.lblIsFromPlatform.Name = "lblIsFromPlatform";
this.lblIsFromPlatform.Size = new System.Drawing.Size(41, 12);
this.lblIsFromPlatform.TabIndex = 5;
this.lblIsFromPlatform.Text = "平台单";
this.chkIsFromPlatform.Location = new System.Drawing.Point(173,121);
this.chkIsFromPlatform.Name = "chkIsFromPlatform";
this.chkIsFromPlatform.Size = new System.Drawing.Size(100, 21);
this.chkIsFromPlatform.TabIndex = 5;
this.Controls.Add(this.lblIsFromPlatform);
this.Controls.Add(this.chkIsFromPlatform);

           //#####DepartmentID###Int64
//属性测试150DepartmentID
//属性测试150DepartmentID
//属性测试150DepartmentID
//属性测试150DepartmentID
this.lblDepartmentID.AutoSize = true;
this.lblDepartmentID.Location = new System.Drawing.Point(100,150);
this.lblDepartmentID.Name = "lblDepartmentID";
this.lblDepartmentID.Size = new System.Drawing.Size(41, 12);
this.lblDepartmentID.TabIndex = 6;
this.lblDepartmentID.Text = "部门";
//111======150
this.cmbDepartmentID.Location = new System.Drawing.Point(173,146);
this.cmbDepartmentID.Name ="cmbDepartmentID";
this.cmbDepartmentID.Size = new System.Drawing.Size(100, 21);
this.cmbDepartmentID.TabIndex = 6;
this.Controls.Add(this.lblDepartmentID);
this.Controls.Add(this.cmbDepartmentID);

           //#####ProjectGroup_ID###Int64
//属性测试175ProjectGroup_ID
//属性测试175ProjectGroup_ID
this.lblProjectGroup_ID.AutoSize = true;
this.lblProjectGroup_ID.Location = new System.Drawing.Point(100,175);
this.lblProjectGroup_ID.Name = "lblProjectGroup_ID";
this.lblProjectGroup_ID.Size = new System.Drawing.Size(41, 12);
this.lblProjectGroup_ID.TabIndex = 7;
this.lblProjectGroup_ID.Text = "项目组";
//111======175
this.cmbProjectGroup_ID.Location = new System.Drawing.Point(173,171);
this.cmbProjectGroup_ID.Name ="cmbProjectGroup_ID";
this.cmbProjectGroup_ID.Size = new System.Drawing.Size(100, 21);
this.cmbProjectGroup_ID.TabIndex = 7;
this.Controls.Add(this.lblProjectGroup_ID);
this.Controls.Add(this.cmbProjectGroup_ID);

           //#####Currency_ID###Int64
//属性测试200Currency_ID
this.lblCurrency_ID.AutoSize = true;
this.lblCurrency_ID.Location = new System.Drawing.Point(100,200);
this.lblCurrency_ID.Name = "lblCurrency_ID";
this.lblCurrency_ID.Size = new System.Drawing.Size(41, 12);
this.lblCurrency_ID.TabIndex = 8;
this.lblCurrency_ID.Text = "币别";
//111======200
this.cmbCurrency_ID.Location = new System.Drawing.Point(173,196);
this.cmbCurrency_ID.Name ="cmbCurrency_ID";
this.cmbCurrency_ID.Size = new System.Drawing.Size(100, 21);
this.cmbCurrency_ID.TabIndex = 8;
this.Controls.Add(this.lblCurrency_ID);
this.Controls.Add(this.cmbCurrency_ID);

           //#####ExchangeRate###Decimal
this.lblExchangeRate.AutoSize = true;
this.lblExchangeRate.Location = new System.Drawing.Point(100,225);
this.lblExchangeRate.Name = "lblExchangeRate";
this.lblExchangeRate.Size = new System.Drawing.Size(41, 12);
this.lblExchangeRate.TabIndex = 9;
this.lblExchangeRate.Text = "汇率";
//111======225
this.txtExchangeRate.Location = new System.Drawing.Point(173,221);
this.txtExchangeRate.Name ="txtExchangeRate";
this.txtExchangeRate.Size = new System.Drawing.Size(100, 21);
this.txtExchangeRate.TabIndex = 9;
this.Controls.Add(this.lblExchangeRate);
this.Controls.Add(this.txtExchangeRate);

           //#####ForeignAmount###Decimal
this.lblForeignAmount.AutoSize = true;
this.lblForeignAmount.Location = new System.Drawing.Point(100,250);
this.lblForeignAmount.Name = "lblForeignAmount";
this.lblForeignAmount.Size = new System.Drawing.Size(41, 12);
this.lblForeignAmount.TabIndex = 10;
this.lblForeignAmount.Text = "支付金额外币";
//111======250
this.txtForeignAmount.Location = new System.Drawing.Point(173,246);
this.txtForeignAmount.Name ="txtForeignAmount";
this.txtForeignAmount.Size = new System.Drawing.Size(100, 21);
this.txtForeignAmount.TabIndex = 10;
this.Controls.Add(this.lblForeignAmount);
this.Controls.Add(this.txtForeignAmount);

           //#####LocalAmount###Decimal
this.lblLocalAmount.AutoSize = true;
this.lblLocalAmount.Location = new System.Drawing.Point(100,275);
this.lblLocalAmount.Name = "lblLocalAmount";
this.lblLocalAmount.Size = new System.Drawing.Size(41, 12);
this.lblLocalAmount.TabIndex = 11;
this.lblLocalAmount.Text = "支付金额本币";
//111======275
this.txtLocalAmount.Location = new System.Drawing.Point(173,271);
this.txtLocalAmount.Name ="txtLocalAmount";
this.txtLocalAmount.Size = new System.Drawing.Size(100, 21);
this.txtLocalAmount.TabIndex = 11;
this.Controls.Add(this.lblLocalAmount);
this.Controls.Add(this.txtLocalAmount);

           //#####300Summary###String
this.lblSummary.AutoSize = true;
this.lblSummary.Location = new System.Drawing.Point(100,300);
this.lblSummary.Name = "lblSummary";
this.lblSummary.Size = new System.Drawing.Size(41, 12);
this.lblSummary.TabIndex = 12;
this.lblSummary.Text = "摘要";
this.txtSummary.Location = new System.Drawing.Point(173,296);
this.txtSummary.Name = "txtSummary";
this.txtSummary.Size = new System.Drawing.Size(100, 21);
this.txtSummary.TabIndex = 12;
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
           // this.kryptonPanel1.TabIndex = 12;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.Controls.Add(this.lblPaymentId );
this.Controls.Add(this.cmbPaymentId );

                this.Controls.Add(this.lblSourceBizType );
this.Controls.Add(this.txtSourceBizType );

                this.Controls.Add(this.lblSourceBilllId );
this.Controls.Add(this.txtSourceBilllId );

                this.Controls.Add(this.lblSourceBillNo );
this.Controls.Add(this.txtSourceBillNo );

                this.Controls.Add(this.lblIsFromPlatform );
this.Controls.Add(this.chkIsFromPlatform );

                this.Controls.Add(this.lblDepartmentID );
this.Controls.Add(this.cmbDepartmentID );

                this.Controls.Add(this.lblProjectGroup_ID );
this.Controls.Add(this.cmbProjectGroup_ID );

                this.Controls.Add(this.lblCurrency_ID );
this.Controls.Add(this.cmbCurrency_ID );

                this.Controls.Add(this.lblExchangeRate );
this.Controls.Add(this.txtExchangeRate );

                this.Controls.Add(this.lblForeignAmount );
this.Controls.Add(this.txtForeignAmount );

                this.Controls.Add(this.lblLocalAmount );
this.Controls.Add(this.txtLocalAmount );

                this.Controls.Add(this.lblSummary );
this.Controls.Add(this.txtSummary );

                            // 
            // "tb_FM_PaymentRecordDetailEdit"
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 300);
            //this.Controls.Add(this.kryptonPanel1);
            
            this.Name = "tb_FM_PaymentRecordDetailEdit";
            this.Size = new System.Drawing.Size(911, 490);
          
            
            //((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
           // this.kryptonPanel1.ResumeLayout(false);
           // this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);
            //this.PerformLayout();
            
        }

        #endregion
     //for start
     
         
              private Krypton.Toolkit.KryptonLabel lblPaymentId;
private Krypton.Toolkit.KryptonComboBox cmbPaymentId;

    
        
              private Krypton.Toolkit.KryptonLabel lblSourceBizType;
private Krypton.Toolkit.KryptonTextBox txtSourceBizType;

    
        
              private Krypton.Toolkit.KryptonLabel lblSourceBilllId;
private Krypton.Toolkit.KryptonTextBox txtSourceBilllId;

    
        
              private Krypton.Toolkit.KryptonLabel lblSourceBillNo;
private Krypton.Toolkit.KryptonTextBox txtSourceBillNo;

    
        
              private Krypton.Toolkit.KryptonLabel lblIsFromPlatform;
private Krypton.Toolkit.KryptonCheckBox chkIsFromPlatform;

    
        
              private Krypton.Toolkit.KryptonLabel lblDepartmentID;
private Krypton.Toolkit.KryptonComboBox cmbDepartmentID;

    
        
              private Krypton.Toolkit.KryptonLabel lblProjectGroup_ID;
private Krypton.Toolkit.KryptonComboBox cmbProjectGroup_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblCurrency_ID;
private Krypton.Toolkit.KryptonComboBox cmbCurrency_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblExchangeRate;
private Krypton.Toolkit.KryptonTextBox txtExchangeRate;

    
        
              private Krypton.Toolkit.KryptonLabel lblForeignAmount;
private Krypton.Toolkit.KryptonTextBox txtForeignAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblLocalAmount;
private Krypton.Toolkit.KryptonTextBox txtLocalAmount;

    
        
              private Krypton.Toolkit.KryptonLabel lblSummary;
private Krypton.Toolkit.KryptonTextBox txtSummary;

    
            //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
       // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
   
 





    }
}

