using Krypton.Toolkit;

namespace RUINORERP.UI.AdvancedUIModule
{
    partial class UCAdvDateTimerPickerGroup
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
            this.dtp1 = new Krypton.Toolkit.KryptonDateTimePicker();
            this.dtp2 = new Krypton.Toolkit.KryptonDateTimePicker();
            this.kryptonLabel1 = new Krypton.Toolkit.KryptonLabel();
            this.SuspendLayout();
            // 
            // dtp1
            // 
            this.dtp1.Checked = false;
            this.dtp1.Location = new System.Drawing.Point(0, 2);
            this.dtp1.Name = "dtp1";
            this.dtp1.ShowCheckBox = true;
            this.dtp1.Size = new System.Drawing.Size(118, 21);
            this.dtp1.TabIndex = 0;
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.Location = new System.Drawing.Point(121, 3);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(17, 19);
            this.kryptonLabel1.TabIndex = 2;
            this.kryptonLabel1.Values.Text = "~";
            // 
            // dtp2
            // 
            this.dtp2.Checked = false;
            this.dtp2.Location = new System.Drawing.Point(142, 2);
            this.dtp2.Name = "dtp2";
            this.dtp2.ShowCheckBox = true;
            this.dtp2.Size = new System.Drawing.Size(118, 21);
            this.dtp2.TabIndex = 1;
            // 
            // UCAdvDateTimerPickerGroup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dtp2);
            this.Controls.Add(this.kryptonLabel1);
            this.Controls.Add(this.dtp1);
            this.Name = "UCAdvDateTimerPickerGroup";
            this.Size = new System.Drawing.Size(260, 25);
            this.Load += new System.EventHandler(this.UCAdvDateTimerPickerGroup_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Krypton.Toolkit.KryptonLabel kryptonLabel1;
        internal Krypton.Toolkit.KryptonDateTimePicker dtp1;
        internal Krypton.Toolkit.KryptonDateTimePicker dtp2;
    }
}
