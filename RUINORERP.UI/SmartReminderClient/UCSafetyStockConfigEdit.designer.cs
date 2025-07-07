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
            this.btnSeleted = new Krypton.Toolkit.KryptonButton();
            this.lblMaxStock = new Krypton.Toolkit.KryptonLabel();
            this.txtMaxStock = new Krypton.Toolkit.KryptonTextBox();
            this.lblIs_enabled = new Krypton.Toolkit.KryptonLabel();
            this.chkIs_enabled = new Krypton.Toolkit.KryptonCheckBox();
            this.lb作用对象列表 = new Krypton.Toolkit.KryptonLabel();
            this.txt_ProductIds = new Krypton.Toolkit.KryptonTextBox();
            this.lblMinStock = new Krypton.Toolkit.KryptonLabel();
            this.txtMinStock = new Krypton.Toolkit.KryptonTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(196, 466);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 25);
            this.btnOk.TabIndex = 0;
            this.btnOk.Values.Text = "确定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(314, 466);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Values.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.btnSeleted);
            this.kryptonPanel1.Controls.Add(this.lblMaxStock);
            this.kryptonPanel1.Controls.Add(this.txtMaxStock);
            this.kryptonPanel1.Controls.Add(this.lblIs_enabled);
            this.kryptonPanel1.Controls.Add(this.chkIs_enabled);
            this.kryptonPanel1.Controls.Add(this.lb作用对象列表);
            this.kryptonPanel1.Controls.Add(this.txt_ProductIds);
            this.kryptonPanel1.Controls.Add(this.lblMinStock);
            this.kryptonPanel1.Controls.Add(this.txtMinStock);
            this.kryptonPanel1.Controls.Add(this.btnCancel);
            this.kryptonPanel1.Controls.Add(this.btnOk);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(643, 543);
            this.kryptonPanel1.TabIndex = 2;
            // 
            // btnSeleted
            // 
            this.btnSeleted.Location = new System.Drawing.Point(541, 300);
            this.btnSeleted.Name = "btnSeleted";
            this.btnSeleted.Size = new System.Drawing.Size(90, 25);
            this.btnSeleted.TabIndex = 51;
            this.btnSeleted.Values.Text = "选择产品对象";
            this.btnSeleted.Click += new System.EventHandler(this.btnSeleted_Click);
            // 
            // lblMaxStock
            // 
            this.lblMaxStock.Location = new System.Drawing.Point(6, 348);
            this.lblMaxStock.Name = "lblMaxStock";
            this.lblMaxStock.Size = new System.Drawing.Size(75, 20);
            this.lblMaxStock.TabIndex = 49;
            this.lblMaxStock.Values.Text = "最大库存量";
            // 
            // txtMaxStock
            // 
            this.txtMaxStock.Location = new System.Drawing.Point(91, 345);
            this.txtMaxStock.Name = "txtMaxStock";
            this.txtMaxStock.Size = new System.Drawing.Size(214, 23);
            this.txtMaxStock.TabIndex = 50;
            // 
            // lblIs_enabled
            // 
            this.lblIs_enabled.Location = new System.Drawing.Point(19, 374);
            this.lblIs_enabled.Name = "lblIs_enabled";
            this.lblIs_enabled.Size = new System.Drawing.Size(62, 20);
            this.lblIs_enabled.TabIndex = 37;
            this.lblIs_enabled.Values.Text = "是否启用";
            // 
            // chkIs_enabled
            // 
            this.chkIs_enabled.Location = new System.Drawing.Point(92, 374);
            this.chkIs_enabled.Name = "chkIs_enabled";
            this.chkIs_enabled.Size = new System.Drawing.Size(19, 13);
            this.chkIs_enabled.TabIndex = 38;
            this.chkIs_enabled.Values.Text = "";
            // 
            // lb作用对象列表
            // 
            this.lb作用对象列表.Location = new System.Drawing.Point(3, 12);
            this.lb作用对象列表.Name = "lb作用对象列表";
            this.lb作用对象列表.Size = new System.Drawing.Size(88, 20);
            this.lb作用对象列表.TabIndex = 7;
            this.lb作用对象列表.Values.Text = "作用对象列表";
            // 
            // txt_ProductIds
            // 
            this.txt_ProductIds.Location = new System.Drawing.Point(93, 12);
            this.txt_ProductIds.Multiline = true;
            this.txt_ProductIds.Name = "txt_ProductIds";
            this.txt_ProductIds.Size = new System.Drawing.Size(538, 282);
            this.txt_ProductIds.TabIndex = 8;
            // 
            // lblMinStock
            // 
            this.lblMinStock.Location = new System.Drawing.Point(6, 319);
            this.lblMinStock.Name = "lblMinStock";
            this.lblMinStock.Size = new System.Drawing.Size(75, 20);
            this.lblMinStock.TabIndex = 17;
            this.lblMinStock.Values.Text = "最小库存量";
            // 
            // txtMinStock
            // 
            this.txtMinStock.Location = new System.Drawing.Point(91, 316);
            this.txtMinStock.Name = "txtMinStock";
            this.txtMinStock.Size = new System.Drawing.Size(214, 23);
            this.txtMinStock.TabIndex = 18;
            // 
            // UCSafetyStockConfigEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(643, 543);
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
        private Krypton.Toolkit.KryptonLabel lb作用对象列表;
        private Krypton.Toolkit.KryptonTextBox txt_ProductIds;
        private Krypton.Toolkit.KryptonLabel lblMinStock;
        private Krypton.Toolkit.KryptonTextBox txtMinStock;
        private Krypton.Toolkit.KryptonLabel lblIs_enabled;
        private Krypton.Toolkit.KryptonCheckBox chkIs_enabled;
        private Krypton.Toolkit.KryptonLabel lblMaxStock;
        private Krypton.Toolkit.KryptonTextBox txtMaxStock;
        private Krypton.Toolkit.KryptonButton btnSeleted;
    }
}
