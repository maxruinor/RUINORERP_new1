namespace RUINORERP.UI.SmartReminderClient
{
    partial class UCSafetyStockConfigEdit
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
            this.btnOk = new Krypton.Toolkit.KryptonButton();
            this.btnCancel = new Krypton.Toolkit.KryptonButton();
            this.kryptonPanel1 = new Krypton.Toolkit.KryptonPanel();
            this.kryptonLabel2 = new Krypton.Toolkit.KryptonLabel();
            this.txtCheckIntervalByMinutes = new Krypton.Toolkit.KryptonTextBox();
            this.kryptonLabel1 = new Krypton.Toolkit.KryptonLabel();
            this.clbLocation_IDs = new Krypton.Toolkit.KryptonCheckedListBox();
            this.clbProds = new Krypton.Toolkit.KryptonCheckedListBox();
            this.btnSeleted = new Krypton.Toolkit.KryptonButton();
            this.lblMaxStock = new Krypton.Toolkit.KryptonLabel();
            this.txtMaxStock = new Krypton.Toolkit.KryptonTextBox();
            this.lb作用对象列表 = new Krypton.Toolkit.KryptonLabel();
            this.lblMinStock = new Krypton.Toolkit.KryptonLabel();
            this.txtMinStock = new Krypton.Toolkit.KryptonTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(306, 516);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 25);
            this.btnOk.TabIndex = 0;
            this.btnOk.Values.Text = "确定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(424, 516);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Values.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.kryptonLabel2);
            this.kryptonPanel1.Controls.Add(this.txtCheckIntervalByMinutes);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel1);
            this.kryptonPanel1.Controls.Add(this.clbLocation_IDs);
            this.kryptonPanel1.Controls.Add(this.clbProds);
            this.kryptonPanel1.Controls.Add(this.btnSeleted);
            this.kryptonPanel1.Controls.Add(this.lblMaxStock);
            this.kryptonPanel1.Controls.Add(this.txtMaxStock);
            this.kryptonPanel1.Controls.Add(this.lb作用对象列表);
            this.kryptonPanel1.Controls.Add(this.lblMinStock);
            this.kryptonPanel1.Controls.Add(this.txtMinStock);
            this.kryptonPanel1.Controls.Add(this.btnCancel);
            this.kryptonPanel1.Controls.Add(this.btnOk);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(774, 553);
            this.kryptonPanel1.TabIndex = 2;
            // 
            // kryptonLabel2
            // 
            this.kryptonLabel2.Location = new System.Drawing.Point(431, 213);
            this.kryptonLabel2.Name = "kryptonLabel2";
            this.kryptonLabel2.Size = new System.Drawing.Size(96, 20);
            this.kryptonLabel2.TabIndex = 56;
            this.kryptonLabel2.Values.Text = "检测频率(分钟)";
            // 
            // txtCheckIntervalByMinutes
            // 
            this.txtCheckIntervalByMinutes.Location = new System.Drawing.Point(537, 210);
            this.txtCheckIntervalByMinutes.Name = "txtCheckIntervalByMinutes";
            this.txtCheckIntervalByMinutes.Size = new System.Drawing.Size(97, 23);
            this.txtCheckIntervalByMinutes.TabIndex = 57;
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.Location = new System.Drawing.Point(465, 12);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(62, 20);
            this.kryptonLabel1.TabIndex = 55;
            this.kryptonLabel1.Values.Text = "提醒库位";
            // 
            // clbLocation_IDs
            // 
            this.clbLocation_IDs.Location = new System.Drawing.Point(537, 12);
            this.clbLocation_IDs.Name = "clbLocation_IDs";
            this.clbLocation_IDs.Size = new System.Drawing.Size(185, 117);
            this.clbLocation_IDs.TabIndex = 54;
            // 
            // clbProds
            // 
            this.clbProds.Location = new System.Drawing.Point(140, 12);
            this.clbProds.Name = "clbProds";
            this.clbProds.Size = new System.Drawing.Size(216, 427);
            this.clbProds.TabIndex = 53;
            // 
            // btnSeleted
            // 
            this.btnSeleted.Location = new System.Drawing.Point(140, 445);
            this.btnSeleted.Name = "btnSeleted";
            this.btnSeleted.Size = new System.Drawing.Size(90, 25);
            this.btnSeleted.TabIndex = 51;
            this.btnSeleted.Values.Text = "选择产品对象";
            this.btnSeleted.Click += new System.EventHandler(this.btnSeleted_Click);
            // 
            // lblMaxStock
            // 
            this.lblMaxStock.Location = new System.Drawing.Point(452, 179);
            this.lblMaxStock.Name = "lblMaxStock";
            this.lblMaxStock.Size = new System.Drawing.Size(75, 20);
            this.lblMaxStock.TabIndex = 49;
            this.lblMaxStock.Values.Text = "最大库存量";
            // 
            // txtMaxStock
            // 
            this.txtMaxStock.Location = new System.Drawing.Point(537, 176);
            this.txtMaxStock.Name = "txtMaxStock";
            this.txtMaxStock.Size = new System.Drawing.Size(97, 23);
            this.txtMaxStock.TabIndex = 50;
            // 
            // lb作用对象列表
            // 
            this.lb作用对象列表.Location = new System.Drawing.Point(12, 12);
            this.lb作用对象列表.Name = "lb作用对象列表";
            this.lb作用对象列表.Size = new System.Drawing.Size(114, 20);
            this.lb作用对象列表.TabIndex = 7;
            this.lb作用对象列表.Values.Text = "要检测的产品对象";
            // 
            // lblMinStock
            // 
            this.lblMinStock.Location = new System.Drawing.Point(452, 138);
            this.lblMinStock.Name = "lblMinStock";
            this.lblMinStock.Size = new System.Drawing.Size(75, 20);
            this.lblMinStock.TabIndex = 17;
            this.lblMinStock.Values.Text = "最小库存量";
            // 
            // txtMinStock
            // 
            this.txtMinStock.Location = new System.Drawing.Point(537, 135);
            this.txtMinStock.Name = "txtMinStock";
            this.txtMinStock.Size = new System.Drawing.Size(97, 23);
            this.txtMinStock.TabIndex = 18;
            // 
            // UCSafetyStockConfigEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(774, 553);
            this.Controls.Add(this.kryptonPanel1);
            this.Name = "UCSafetyStockConfigEdit";
            this.Load += new System.EventHandler(this.UCBoxRulesEdit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonButton btnOk;
        private Krypton.Toolkit.KryptonButton btnCancel;
        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private Krypton.Toolkit.KryptonLabel lblMinStock;
        private Krypton.Toolkit.KryptonTextBox txtMinStock;
        private Krypton.Toolkit.KryptonLabel lblMaxStock;
        private Krypton.Toolkit.KryptonTextBox txtMaxStock;
        private Krypton.Toolkit.KryptonButton btnSeleted;
        private Krypton.Toolkit.KryptonCheckedListBox clbProds;
        private Krypton.Toolkit.KryptonLabel lb作用对象列表;
        private Krypton.Toolkit.KryptonCheckedListBox clbLocation_IDs;
        private Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private Krypton.Toolkit.KryptonLabel kryptonLabel2;
        private Krypton.Toolkit.KryptonTextBox txtCheckIntervalByMinutes;
    }
}
