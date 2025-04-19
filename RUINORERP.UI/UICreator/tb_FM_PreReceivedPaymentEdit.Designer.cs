// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/18/2025 13:55:13
// **************************************
using System;
﻿using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 预收付款单
    /// </summary>
    partial class tb_FM_PreReceivedPaymentEdit
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
            this.lblPrePaymentReason = new Krypton.Toolkit.KryptonLabel();
            this.txtPrePaymentReason = new Krypton.Toolkit.KryptonTextBox();
            this.lblSourceBill_BizType = new Krypton.Toolkit.KryptonLabel();
            this.txtSourceBill_BizType = new Krypton.Toolkit.KryptonTextBox();
            this.lblSourceBill_ID = new Krypton.Toolkit.KryptonLabel();
            this.txtSourceBill_ID = new Krypton.Toolkit.KryptonTextBox();
            this.lblSourceBillNO = new Krypton.Toolkit.KryptonLabel();
            this.txtSourceBillNO = new Krypton.Toolkit.KryptonTextBox();
            this.SuspendLayout();
            // 
            // lblPrePaymentReason
            // 
            this.lblPrePaymentReason.Location = new System.Drawing.Point(17, 299);
            this.lblPrePaymentReason.Name = "lblPrePaymentReason";
            this.lblPrePaymentReason.Size = new System.Drawing.Size(36, 20);
            this.lblPrePaymentReason.TabIndex = 10;
            this.lblPrePaymentReason.Values.Text = "事由";
            // 
            // txtPrePaymentReason
            // 
            this.txtPrePaymentReason.Location = new System.Drawing.Point(90, 295);
            this.txtPrePaymentReason.Name = "txtPrePaymentReason";
            this.txtPrePaymentReason.Size = new System.Drawing.Size(100, 23);
            this.txtPrePaymentReason.TabIndex = 10;
            // 
            // lblSourceBill_BizType
            // 
            this.lblSourceBill_BizType.Location = new System.Drawing.Point(17, 324);
            this.lblSourceBill_BizType.Name = "lblSourceBill_BizType";
            this.lblSourceBill_BizType.Size = new System.Drawing.Size(62, 20);
            this.lblSourceBill_BizType.TabIndex = 11;
            this.lblSourceBill_BizType.Values.Text = "来源业务";
            // 
            // txtSourceBill_BizType
            // 
            this.txtSourceBill_BizType.Location = new System.Drawing.Point(90, 320);
            this.txtSourceBill_BizType.Name = "txtSourceBill_BizType";
            this.txtSourceBill_BizType.Size = new System.Drawing.Size(100, 23);
            this.txtSourceBill_BizType.TabIndex = 11;
            // 
            // lblSourceBill_ID
            // 
            this.lblSourceBill_ID.Location = new System.Drawing.Point(17, 353);
            this.lblSourceBill_ID.Name = "lblSourceBill_ID";
            this.lblSourceBill_ID.Size = new System.Drawing.Size(62, 20);
            this.lblSourceBill_ID.TabIndex = 12;
            this.lblSourceBill_ID.Values.Text = "来源单据";
            // 
            // txtSourceBill_ID
            // 
            this.txtSourceBill_ID.Location = new System.Drawing.Point(90, 349);
            this.txtSourceBill_ID.Name = "txtSourceBill_ID";
            this.txtSourceBill_ID.Size = new System.Drawing.Size(100, 23);
            this.txtSourceBill_ID.TabIndex = 12;
            // 
            // lblSourceBillNO
            // 
            this.lblSourceBillNO.Location = new System.Drawing.Point(17, 378);
            this.lblSourceBillNO.Name = "lblSourceBillNO";
            this.lblSourceBillNO.Size = new System.Drawing.Size(62, 20);
            this.lblSourceBillNO.TabIndex = 13;
            this.lblSourceBillNO.Values.Text = "来源单号";
            // 
            // txtSourceBillNO
            // 
            this.txtSourceBillNO.Location = new System.Drawing.Point(90, 374);
            this.txtSourceBillNO.Name = "txtSourceBillNO";
            this.txtSourceBillNO.Size = new System.Drawing.Size(100, 23);
            this.txtSourceBillNO.TabIndex = 13;
            // 
            // tb_FM_PreReceivedPaymentEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblPrePaymentReason);
            this.Controls.Add(this.txtPrePaymentReason);
            this.Controls.Add(this.lblSourceBill_BizType);
            this.Controls.Add(this.txtSourceBill_BizType);
            this.Controls.Add(this.lblSourceBill_ID);
            this.Controls.Add(this.txtSourceBill_ID);
            this.Controls.Add(this.lblSourceBillNO);
            this.Controls.Add(this.txtSourceBillNO);
            this.Name = "tb_FM_PreReceivedPaymentEdit";
            this.Size = new System.Drawing.Size(911, 601);
            this.Load += new System.EventHandler(this.tb_FM_PreReceivedPaymentEdit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
     //for start









    
        
              private Krypton.Toolkit.KryptonLabel lblPrePaymentReason;
private Krypton.Toolkit.KryptonTextBox txtPrePaymentReason;

    
        
              private Krypton.Toolkit.KryptonLabel lblSourceBill_BizType;
private Krypton.Toolkit.KryptonTextBox txtSourceBill_BizType;

    
        
              private Krypton.Toolkit.KryptonLabel lblSourceBill_ID;
private Krypton.Toolkit.KryptonTextBox txtSourceBill_ID;

    
        
              private Krypton.Toolkit.KryptonLabel lblSourceBillNO;
private Krypton.Toolkit.KryptonTextBox txtSourceBillNO;






















        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
        // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;







    }
}

