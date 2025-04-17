// **************************************
// 生成：CodeBuilder (http://www.fireasy.cn/codebuilder)
// 项目：信息系统
// 版权：Copyright RUINOR
// 作者：Watson
// 时间：04/16/2025 12:02:50
// **************************************
using System;
using SqlSugar;
using System.Collections.Generic;
using FluentValidation;


namespace RUINORERP.UI
{
    /// <summary>
    /// 币别换算表
    /// </summary>
    partial class tb_CurrencyExchangeRateEdit
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
            this.lblConversionName = new Krypton.Toolkit.KryptonLabel();
            this.txtConversionName = new Krypton.Toolkit.KryptonTextBox();
            this.lblBaseCurrencyID = new Krypton.Toolkit.KryptonLabel();
            this.txtBaseCurrencyID = new Krypton.Toolkit.KryptonTextBox();
            this.lblTargetCurrencyID = new Krypton.Toolkit.KryptonLabel();
            this.txtTargetCurrencyID = new Krypton.Toolkit.KryptonTextBox();
            this.lblEffectiveDate = new Krypton.Toolkit.KryptonLabel();
            this.dtpEffectiveDate = new Krypton.Toolkit.KryptonDateTimePicker();
            this.lblExpirationDate = new Krypton.Toolkit.KryptonLabel();
            this.dtpExpirationDate = new Krypton.Toolkit.KryptonDateTimePicker();
            this.lblDefaultExchRate = new Krypton.Toolkit.KryptonLabel();
            this.txtDefaultExchRate = new Krypton.Toolkit.KryptonTextBox();
            this.lblExecuteExchRate = new Krypton.Toolkit.KryptonLabel();
            this.txtExecuteExchRate = new Krypton.Toolkit.KryptonTextBox();
            this.lblIs_enabled = new Krypton.Toolkit.KryptonLabel();
            this.chkIs_enabled = new Krypton.Toolkit.KryptonCheckBox();
            this.lblIs_available = new Krypton.Toolkit.KryptonLabel();
            this.chkIs_available = new Krypton.Toolkit.KryptonCheckBox();
            this.SuspendLayout();
            // 
            // lblConversionName
            // 
            this.lblConversionName.Location = new System.Drawing.Point(100, 25);
            this.lblConversionName.Name = "lblConversionName";
            this.lblConversionName.Size = new System.Drawing.Size(62, 20);
            this.lblConversionName.TabIndex = 1;
            this.lblConversionName.Values.Text = "换算名称";
            // 
            // txtConversionName
            // 
            this.txtConversionName.Location = new System.Drawing.Point(173, 21);
            this.txtConversionName.Name = "txtConversionName";
            this.txtConversionName.Size = new System.Drawing.Size(100, 23);
            this.txtConversionName.TabIndex = 1;
            // 
            // lblBaseCurrencyID
            // 
            this.lblBaseCurrencyID.Location = new System.Drawing.Point(100, 50);
            this.lblBaseCurrencyID.Name = "lblBaseCurrencyID";
            this.lblBaseCurrencyID.Size = new System.Drawing.Size(62, 20);
            this.lblBaseCurrencyID.TabIndex = 2;
            this.lblBaseCurrencyID.Values.Text = "基本币别";
            // 
            // txtBaseCurrencyID
            // 
            this.txtBaseCurrencyID.Location = new System.Drawing.Point(173, 46);
            this.txtBaseCurrencyID.Name = "txtBaseCurrencyID";
            this.txtBaseCurrencyID.Size = new System.Drawing.Size(100, 23);
            this.txtBaseCurrencyID.TabIndex = 2;
            // 
            // lblTargetCurrencyID
            // 
            this.lblTargetCurrencyID.Location = new System.Drawing.Point(100, 75);
            this.lblTargetCurrencyID.Name = "lblTargetCurrencyID";
            this.lblTargetCurrencyID.Size = new System.Drawing.Size(62, 20);
            this.lblTargetCurrencyID.TabIndex = 3;
            this.lblTargetCurrencyID.Values.Text = "目标币别";
            // 
            // txtTargetCurrencyID
            // 
            this.txtTargetCurrencyID.Location = new System.Drawing.Point(173, 71);
            this.txtTargetCurrencyID.Name = "txtTargetCurrencyID";
            this.txtTargetCurrencyID.Size = new System.Drawing.Size(100, 23);
            this.txtTargetCurrencyID.TabIndex = 3;
            // 
            // lblEffectiveDate
            // 
            this.lblEffectiveDate.Location = new System.Drawing.Point(100, 100);
            this.lblEffectiveDate.Name = "lblEffectiveDate";
            this.lblEffectiveDate.Size = new System.Drawing.Size(62, 20);
            this.lblEffectiveDate.TabIndex = 4;
            this.lblEffectiveDate.Values.Text = "生效日期";
            // 
            // dtpEffectiveDate
            // 
            this.dtpEffectiveDate.Location = new System.Drawing.Point(173, 96);
            this.dtpEffectiveDate.Name = "dtpEffectiveDate";
            this.dtpEffectiveDate.Size = new System.Drawing.Size(100, 21);
            this.dtpEffectiveDate.TabIndex = 4;
            // 
            // lblExpirationDate
            // 
            this.lblExpirationDate.Location = new System.Drawing.Point(100, 125);
            this.lblExpirationDate.Name = "lblExpirationDate";
            this.lblExpirationDate.Size = new System.Drawing.Size(62, 20);
            this.lblExpirationDate.TabIndex = 5;
            this.lblExpirationDate.Values.Text = "有效日期";
            // 
            // dtpExpirationDate
            // 
            this.dtpExpirationDate.Location = new System.Drawing.Point(173, 121);
            this.dtpExpirationDate.Name = "dtpExpirationDate";
            this.dtpExpirationDate.ShowCheckBox = true;
            this.dtpExpirationDate.Size = new System.Drawing.Size(100, 21);
            this.dtpExpirationDate.TabIndex = 5;
            // 
            // lblDefaultExchRate
            // 
            this.lblDefaultExchRate.Location = new System.Drawing.Point(100, 150);
            this.lblDefaultExchRate.Name = "lblDefaultExchRate";
            this.lblDefaultExchRate.Size = new System.Drawing.Size(62, 20);
            this.lblDefaultExchRate.TabIndex = 6;
            this.lblDefaultExchRate.Values.Text = "预设汇率";
            // 
            // txtDefaultExchRate
            // 
            this.txtDefaultExchRate.Location = new System.Drawing.Point(173, 146);
            this.txtDefaultExchRate.Name = "txtDefaultExchRate";
            this.txtDefaultExchRate.Size = new System.Drawing.Size(100, 23);
            this.txtDefaultExchRate.TabIndex = 6;
            // 
            // lblExecuteExchRate
            // 
            this.lblExecuteExchRate.Location = new System.Drawing.Point(100, 175);
            this.lblExecuteExchRate.Name = "lblExecuteExchRate";
            this.lblExecuteExchRate.Size = new System.Drawing.Size(62, 20);
            this.lblExecuteExchRate.TabIndex = 7;
            this.lblExecuteExchRate.Values.Text = "执行汇率";
            // 
            // txtExecuteExchRate
            // 
            this.txtExecuteExchRate.Location = new System.Drawing.Point(173, 171);
            this.txtExecuteExchRate.Name = "txtExecuteExchRate";
            this.txtExecuteExchRate.Size = new System.Drawing.Size(100, 23);
            this.txtExecuteExchRate.TabIndex = 7;
            // 
            // lblIs_enabled
            // 
            this.lblIs_enabled.Location = new System.Drawing.Point(100, 200);
            this.lblIs_enabled.Name = "lblIs_enabled";
            this.lblIs_enabled.Size = new System.Drawing.Size(62, 20);
            this.lblIs_enabled.TabIndex = 8;
            this.lblIs_enabled.Values.Text = "是否启用";
            // 
            // chkIs_enabled
            // 
            this.chkIs_enabled.Checked = true;
            this.chkIs_enabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIs_enabled.Location = new System.Drawing.Point(173, 196);
            this.chkIs_enabled.Name = "chkIs_enabled";
            this.chkIs_enabled.Size = new System.Drawing.Size(19, 13);
            this.chkIs_enabled.TabIndex = 8;
            this.chkIs_enabled.Values.Text = "";
            // 
            // lblIs_available
            // 
            this.lblIs_available.Location = new System.Drawing.Point(100, 225);
            this.lblIs_available.Name = "lblIs_available";
            this.lblIs_available.Size = new System.Drawing.Size(62, 20);
            this.lblIs_available.TabIndex = 9;
            this.lblIs_available.Values.Text = "是否可用";
            // 
            // chkIs_available
            // 
            this.chkIs_available.Checked = true;
            this.chkIs_available.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIs_available.Location = new System.Drawing.Point(173, 221);
            this.chkIs_available.Name = "chkIs_available";
            this.chkIs_available.Size = new System.Drawing.Size(19, 13);
            this.chkIs_available.TabIndex = 9;
            this.chkIs_available.Values.Text = "";
            // 
            // tb_CurrencyExchangeRateEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblConversionName);
            this.Controls.Add(this.txtConversionName);
            this.Controls.Add(this.lblBaseCurrencyID);
            this.Controls.Add(this.txtBaseCurrencyID);
            this.Controls.Add(this.lblTargetCurrencyID);
            this.Controls.Add(this.txtTargetCurrencyID);
            this.Controls.Add(this.lblEffectiveDate);
            this.Controls.Add(this.dtpEffectiveDate);
            this.Controls.Add(this.lblExpirationDate);
            this.Controls.Add(this.dtpExpirationDate);
            this.Controls.Add(this.lblDefaultExchRate);
            this.Controls.Add(this.txtDefaultExchRate);
            this.Controls.Add(this.lblExecuteExchRate);
            this.Controls.Add(this.txtExecuteExchRate);
            this.Controls.Add(this.lblIs_enabled);
            this.Controls.Add(this.chkIs_enabled);
            this.Controls.Add(this.lblIs_available);
            this.Controls.Add(this.chkIs_available);
            this.Name = "tb_CurrencyExchangeRateEdit";
            this.Size = new System.Drawing.Size(911, 490);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        //for start


        private Krypton.Toolkit.KryptonLabel lblConversionName;
        private Krypton.Toolkit.KryptonTextBox txtConversionName;



       private Krypton.Toolkit.KryptonLabel lblBaseCurrencyID;
        private Krypton.Toolkit.KryptonTextBox txtBaseCurrencyID;



       private Krypton.Toolkit.KryptonLabel lblTargetCurrencyID;
        private Krypton.Toolkit.KryptonTextBox txtTargetCurrencyID;



        private Krypton.Toolkit.KryptonLabel lblEffectiveDate;
        private Krypton.Toolkit.KryptonDateTimePicker dtpEffectiveDate;



        private Krypton.Toolkit.KryptonLabel lblExpirationDate;
        private Krypton.Toolkit.KryptonDateTimePicker dtpExpirationDate;



        private Krypton.Toolkit.KryptonLabel lblDefaultExchRate;
        private Krypton.Toolkit.KryptonTextBox txtDefaultExchRate;



        private Krypton.Toolkit.KryptonLabel lblExecuteExchRate;
        private Krypton.Toolkit.KryptonTextBox txtExecuteExchRate;



        private Krypton.Toolkit.KryptonLabel lblIs_enabled;
        private Krypton.Toolkit.KryptonCheckBox chkIs_enabled;



        private Krypton.Toolkit.KryptonLabel lblIs_available;
        private Krypton.Toolkit.KryptonCheckBox chkIs_available;






        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnOk;
        //private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
        // private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;







    }
}

