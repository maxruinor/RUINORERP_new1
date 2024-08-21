namespace RUINORERP.UI.BI
{
    partial class UCPropertyEdit
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
            this.kryptonLabel1 = new Krypton.Toolkit.KryptonLabel();
            this.txtSortOrder = new Krypton.Toolkit.KryptonTextBox();
            this.lblPropertyName = new Krypton.Toolkit.KryptonLabel();
            this.txtPropertyName = new Krypton.Toolkit.KryptonTextBox();
            this.lblPropertyDesc = new Krypton.Toolkit.KryptonLabel();
            this.txtPropertyDesc = new Krypton.Toolkit.KryptonTextBox();
            this.lblInputType = new Krypton.Toolkit.KryptonLabel();
            this.txtInputType = new Krypton.Toolkit.KryptonTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(140, 249);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 25);
            this.btnOk.TabIndex = 0;
            this.btnOk.Values.Text = "确定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(258, 249);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Values.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.kryptonLabel1);
            this.kryptonPanel1.Controls.Add(this.txtSortOrder);
            this.kryptonPanel1.Controls.Add(this.lblPropertyName);
            this.kryptonPanel1.Controls.Add(this.txtPropertyName);
            this.kryptonPanel1.Controls.Add(this.lblPropertyDesc);
            this.kryptonPanel1.Controls.Add(this.txtPropertyDesc);
            this.kryptonPanel1.Controls.Add(this.lblInputType);
            this.kryptonPanel1.Controls.Add(this.txtInputType);
            this.kryptonPanel1.Controls.Add(this.btnCancel);
            this.kryptonPanel1.Controls.Add(this.btnOk);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(471, 379);
            this.kryptonPanel1.TabIndex = 2;
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.Location = new System.Drawing.Point(61, 150);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(35, 20);
            this.kryptonLabel1.TabIndex = 16;
            this.kryptonLabel1.Values.Text = "排序";
            // 
            // txtSortOrder
            // 
            this.txtSortOrder.Location = new System.Drawing.Point(102, 150);
            this.txtSortOrder.Name = "txtSortOrder";
            this.txtSortOrder.Size = new System.Drawing.Size(312, 20);
            this.txtSortOrder.TabIndex = 17;
            // 
            // lblPropertyName
            // 
            this.lblPropertyName.Location = new System.Drawing.Point(36, 44);
            this.lblPropertyName.Name = "lblPropertyName";
            this.lblPropertyName.Size = new System.Drawing.Size(60, 20);
            this.lblPropertyName.TabIndex = 10;
            this.lblPropertyName.Values.Text = "属性名称";
            // 
            // txtPropertyName
            // 
            this.txtPropertyName.Location = new System.Drawing.Point(102, 40);
            this.txtPropertyName.Name = "txtPropertyName";
            this.txtPropertyName.Size = new System.Drawing.Size(312, 20);
            this.txtPropertyName.TabIndex = 11;
            // 
            // lblPropertyDesc
            // 
            this.lblPropertyDesc.Location = new System.Drawing.Point(36, 79);
            this.lblPropertyDesc.Name = "lblPropertyDesc";
            this.lblPropertyDesc.Size = new System.Drawing.Size(60, 20);
            this.lblPropertyDesc.TabIndex = 12;
            this.lblPropertyDesc.Values.Text = "属性描述";
            // 
            // txtPropertyDesc
            // 
            this.txtPropertyDesc.Location = new System.Drawing.Point(102, 75);
            this.txtPropertyDesc.Multiline = true;
            this.txtPropertyDesc.Name = "txtPropertyDesc";
            this.txtPropertyDesc.Size = new System.Drawing.Size(312, 66);
            this.txtPropertyDesc.TabIndex = 13;
            // 
            // lblInputType
            // 
            this.lblInputType.Location = new System.Drawing.Point(36, 176);
            this.lblInputType.Name = "lblInputType";
            this.lblInputType.Size = new System.Drawing.Size(60, 20);
            this.lblInputType.TabIndex = 14;
            this.lblInputType.Values.Text = "输入类型";
            // 
            // txtInputType
            // 
            this.txtInputType.Location = new System.Drawing.Point(102, 176);
            this.txtInputType.Name = "txtInputType";
            this.txtInputType.Size = new System.Drawing.Size(312, 20);
            this.txtInputType.TabIndex = 15;
            // 
            // UCPropertyEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(471, 379);
            this.Controls.Add(this.kryptonPanel1);
            this.Name = "UCPropertyEdit";
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
        private Krypton.Toolkit.KryptonLabel lblPropertyName;
        private Krypton.Toolkit.KryptonTextBox txtPropertyName;
        private Krypton.Toolkit.KryptonLabel lblPropertyDesc;
        private Krypton.Toolkit.KryptonTextBox txtPropertyDesc;
        private Krypton.Toolkit.KryptonLabel lblInputType;
        private Krypton.Toolkit.KryptonTextBox txtInputType;
        private Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private Krypton.Toolkit.KryptonTextBox txtSortOrder;
    }
}
