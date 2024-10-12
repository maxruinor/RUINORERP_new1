namespace RUINORERP.UI.UserCenter
{
    partial class frmMenuPersonalization
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.QueryShowColQty = new Krypton.Toolkit.KryptonNumericUpDown();
            this.kryptonLabel1 = new Krypton.Toolkit.KryptonLabel();
            this.btnCancel = new Krypton.Toolkit.KryptonButton();
            this.btnOk = new Krypton.Toolkit.KryptonButton();
            this.kryptonPanel1 = new Krypton.Toolkit.KryptonPanel();
            this.flowLayoutPanelButtonsArea = new System.Windows.Forms.FlowLayoutPanel();
            this.kryptonPanelbtusArea = new Krypton.Toolkit.KryptonPanel();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelbtusArea)).BeginInit();
            this.SuspendLayout();
            // 
            // QueryShowColQty
            // 
            this.QueryShowColQty.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.QueryShowColQty.Location = new System.Drawing.Point(161, 12);
            this.QueryShowColQty.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.QueryShowColQty.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.QueryShowColQty.Name = "QueryShowColQty";
            this.QueryShowColQty.Size = new System.Drawing.Size(68, 22);
            this.QueryShowColQty.TabIndex = 0;
            this.QueryShowColQty.ToolTipValues.Description = "查询条件过多时，显示不完整，可以将数字调大。";
            this.QueryShowColQty.ToolTipValues.EnableToolTips = true;
            this.QueryShowColQty.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.Location = new System.Drawing.Point(25, 13);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(130, 20);
            this.kryptonLabel1.TabIndex = 1;
            this.kryptonLabel1.Values.Text = "查询条件显示列数量:";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(349, 560);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Values.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(182, 560);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 25);
            this.btnOk.TabIndex = 12;
            this.btnOk.Values.Text = "确定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.flowLayoutPanelButtonsArea);
            this.kryptonPanel1.Controls.Add(this.kryptonPanelbtusArea);
            this.kryptonPanel1.Controls.Add(this.QueryShowColQty);
            this.kryptonPanel1.Controls.Add(this.btnCancel);
            this.kryptonPanel1.Controls.Add(this.btnOk);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel1);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(632, 635);
            this.kryptonPanel1.TabIndex = 14;
            // 
            // flowLayoutPanelButtonsArea
            // 
            this.flowLayoutPanelButtonsArea.Location = new System.Drawing.Point(29, 70);
            this.flowLayoutPanelButtonsArea.Name = "flowLayoutPanelButtonsArea";
            this.flowLayoutPanelButtonsArea.Size = new System.Drawing.Size(200, 325);
            this.flowLayoutPanelButtonsArea.TabIndex = 15;
            // 
            // kryptonPanelbtusArea
            // 
            this.kryptonPanelbtusArea.Location = new System.Drawing.Point(349, 21);
            this.kryptonPanelbtusArea.Name = "kryptonPanelbtusArea";
            this.kryptonPanelbtusArea.Size = new System.Drawing.Size(247, 191);
            this.kryptonPanelbtusArea.TabIndex = 14;
            // 
            // frmMenuPersonalization
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(632, 635);
            this.Controls.Add(this.kryptonPanel1);
            this.Name = "frmMenuPersonalization";
            this.Text = "查询条件设置";
            this.Load += new System.EventHandler(this.frmMenuPersonalization_Load);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.kryptonPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanelbtusArea)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private Krypton.Toolkit.KryptonButton btnCancel;
        private Krypton.Toolkit.KryptonButton btnOk;
        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        public Krypton.Toolkit.KryptonNumericUpDown QueryShowColQty;
        internal Krypton.Toolkit.KryptonPanel kryptonPanelbtusArea;
        public System.Windows.Forms.FlowLayoutPanel flowLayoutPanelButtonsArea;
    }
}