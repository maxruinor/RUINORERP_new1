namespace RUINORERP.UI.BI
{
    partial class UCPropertyValueEdit
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
            this.lblProperty_ID = new Krypton.Toolkit.KryptonLabel();
            this.cmbProperty_ID = new Krypton.Toolkit.KryptonComboBox();
            this.lblPropertyValueName = new Krypton.Toolkit.KryptonLabel();
            this.txtPropertyValueName = new Krypton.Toolkit.KryptonTextBox();
            this.lblPropertyValueDesc = new Krypton.Toolkit.KryptonLabel();
            this.txtPropertyValueDesc = new Krypton.Toolkit.KryptonTextBox();
            this.lblPropertyValueCode = new Krypton.Toolkit.KryptonLabel();
            this.txtPropertyValueCode = new Krypton.Toolkit.KryptonTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbProperty_ID)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(121, 360);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(90, 25);
            this.btnOk.TabIndex = 0;
            this.btnOk.Values.Text = "确定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(239, 360);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Values.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.lblPropertyValueCode);
            this.kryptonPanel1.Controls.Add(this.txtPropertyValueCode);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel1);
            this.kryptonPanel1.Controls.Add(this.txtSortOrder);
            this.kryptonPanel1.Controls.Add(this.lblProperty_ID);
            this.kryptonPanel1.Controls.Add(this.cmbProperty_ID);
            this.kryptonPanel1.Controls.Add(this.lblPropertyValueName);
            this.kryptonPanel1.Controls.Add(this.txtPropertyValueName);
            this.kryptonPanel1.Controls.Add(this.lblPropertyValueDesc);
            this.kryptonPanel1.Controls.Add(this.txtPropertyValueDesc);
            this.kryptonPanel1.Controls.Add(this.btnCancel);
            this.kryptonPanel1.Controls.Add(this.btnOk);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(493, 421);
            this.kryptonPanel1.TabIndex = 2;
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.Location = new System.Drawing.Point(78, 97);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(36, 20);
            this.kryptonLabel1.TabIndex = 18;
            this.kryptonLabel1.Values.Text = "排序";
            // 
            // txtSortOrder
            // 
            this.txtSortOrder.Location = new System.Drawing.Point(119, 97);
            this.txtSortOrder.Name = "txtSortOrder";
            this.txtSortOrder.Size = new System.Drawing.Size(294, 23);
            this.txtSortOrder.TabIndex = 19;
            // 
            // lblProperty_ID
            // 
            this.lblProperty_ID.Location = new System.Drawing.Point(78, 13);
            this.lblProperty_ID.Name = "lblProperty_ID";
            this.lblProperty_ID.Size = new System.Drawing.Size(36, 20);
            this.lblProperty_ID.TabIndex = 10;
            this.lblProperty_ID.Values.Text = "属性";
            // 
            // cmbProperty_ID
            // 
            this.cmbProperty_ID.DropDownWidth = 100;
            this.cmbProperty_ID.IntegralHeight = false;
            this.cmbProperty_ID.Location = new System.Drawing.Point(119, 12);
            this.cmbProperty_ID.Name = "cmbProperty_ID";
            this.cmbProperty_ID.Size = new System.Drawing.Size(294, 21);
            this.cmbProperty_ID.TabIndex = 11;
            // 
            // lblPropertyValueName
            // 
            this.lblPropertyValueName.Location = new System.Drawing.Point(40, 40);
            this.lblPropertyValueName.Name = "lblPropertyValueName";
            this.lblPropertyValueName.Size = new System.Drawing.Size(75, 20);
            this.lblPropertyValueName.TabIndex = 12;
            this.lblPropertyValueName.Values.Text = "属性值名称";
            // 
            // txtPropertyValueName
            // 
            this.txtPropertyValueName.Location = new System.Drawing.Point(119, 39);
            this.txtPropertyValueName.Name = "txtPropertyValueName";
            this.txtPropertyValueName.Size = new System.Drawing.Size(294, 23);
            this.txtPropertyValueName.TabIndex = 13;
            // 
            // lblPropertyValueDesc
            // 
            this.lblPropertyValueDesc.Location = new System.Drawing.Point(40, 131);
            this.lblPropertyValueDesc.Name = "lblPropertyValueDesc";
            this.lblPropertyValueDesc.Size = new System.Drawing.Size(75, 20);
            this.lblPropertyValueDesc.TabIndex = 14;
            this.lblPropertyValueDesc.Values.Text = "属性值描述";
            // 
            // txtPropertyValueDesc
            // 
            this.txtPropertyValueDesc.Location = new System.Drawing.Point(119, 130);
            this.txtPropertyValueDesc.Multiline = true;
            this.txtPropertyValueDesc.Name = "txtPropertyValueDesc";
            this.txtPropertyValueDesc.Size = new System.Drawing.Size(294, 123);
            this.txtPropertyValueDesc.TabIndex = 15;
            // 
            // lblPropertyValueCode
            // 
            this.lblPropertyValueCode.Location = new System.Drawing.Point(40, 69);
            this.lblPropertyValueCode.Name = "lblPropertyValueCode";
            this.lblPropertyValueCode.Size = new System.Drawing.Size(75, 20);
            this.lblPropertyValueCode.TabIndex = 20;
            this.lblPropertyValueCode.Values.Text = "属性值名称";
            // 
            // txtPropertyValueCode
            // 
            this.txtPropertyValueCode.Location = new System.Drawing.Point(119, 68);
            this.txtPropertyValueCode.Name = "txtPropertyValueCode";
            this.txtPropertyValueCode.Size = new System.Drawing.Size(294, 23);
            this.txtPropertyValueCode.TabIndex = 21;
            // 
            // UCPropertyValueEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(493, 421);
            this.Controls.Add(this.kryptonPanel1);
            this.Name = "UCPropertyValueEdit";
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.kryptonPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbProperty_ID)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonButton btnOk;
        private Krypton.Toolkit.KryptonButton btnCancel;
        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private Krypton.Toolkit.KryptonLabel lblProperty_ID;
        private Krypton.Toolkit.KryptonComboBox cmbProperty_ID;
        private Krypton.Toolkit.KryptonLabel lblPropertyValueName;
        private Krypton.Toolkit.KryptonTextBox txtPropertyValueName;
        private Krypton.Toolkit.KryptonLabel lblPropertyValueDesc;
        private Krypton.Toolkit.KryptonTextBox txtPropertyValueDesc;
        private Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private Krypton.Toolkit.KryptonTextBox txtSortOrder;
        private Krypton.Toolkit.KryptonLabel lblPropertyValueCode;
        private Krypton.Toolkit.KryptonTextBox txtPropertyValueCode;
    }
}
