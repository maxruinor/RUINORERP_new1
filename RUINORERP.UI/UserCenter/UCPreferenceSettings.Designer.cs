﻿namespace RUINORERP.UI.UserCenter
{
    partial class UCPreferenceSettings
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
            this.kryptonPanel1 = new Krypton.Toolkit.KryptonPanel();
            this.kryptonHelpCommand1 = new Krypton.Toolkit.KryptonHelpCommand();
            this.kryptonButtonABC = new Krypton.Toolkit.KryptonButton();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.kryptonButtonABC);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(880, 691);
            this.kryptonPanel1.TabIndex = 0;
            // 
            // kryptonHelpCommand1
            // 
            this.kryptonHelpCommand1.Text = "Form Help";
            // 
            // kryptonButtonABC
            // 
            this.kryptonButtonABC.Location = new System.Drawing.Point(95, 108);
            this.kryptonButtonABC.Name = "kryptonButtonABC";
            this.kryptonButtonABC.Size = new System.Drawing.Size(90, 25);
            this.kryptonButtonABC.TabIndex = 0;
            this.kryptonButtonABC.Values.Text = "kryptonButton1";
            // 
            // UCPreferenceSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.kryptonPanel1);
            this.Name = "UCPreferenceSettings";
            this.Size = new System.Drawing.Size(880, 691);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private Krypton.Toolkit.KryptonHelpCommand kryptonHelpCommand1;
        private Krypton.Toolkit.KryptonButton kryptonButtonABC;
    }
}
