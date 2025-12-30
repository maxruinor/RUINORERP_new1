namespace RUINORERP.UI.ProductEAV
{
    partial class UCProductAttrRelationEdit
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
            this.btnReplaceAttrs = new Krypton.Toolkit.KryptonButton();
            this.btnClearAttrs = new Krypton.Toolkit.KryptonButton();
            this.cmbPropertyValue = new Krypton.Toolkit.KryptonComboBox();
            this.cmbProperty = new Krypton.Toolkit.KryptonComboBox();
            this.cmbProdDetail = new Krypton.Toolkit.KryptonComboBox();
            this.cmbProduct = new Krypton.Toolkit.KryptonComboBox();
            this.lblPropertyValue = new Krypton.Toolkit.KryptonLabel();
            this.lblProperty = new Krypton.Toolkit.KryptonLabel();
            this.lblProdDetail = new Krypton.Toolkit.KryptonLabel();
            this.lblProduct = new Krypton.Toolkit.KryptonLabel();
            this.lblRAR_ID = new Krypton.Toolkit.KryptonLabel();
            this.txtRAR_ID = new Krypton.Toolkit.KryptonTextBox();
            this.btnCancel = new Krypton.Toolkit.KryptonButton();
            this.btnOk = new Krypton.Toolkit.KryptonButton();
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPropertyValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbProperty)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbProdDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbProduct)).BeginInit();
            this.SuspendLayout();
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.btnReplaceAttrs);
            this.kryptonPanel1.Controls.Add(this.btnClearAttrs);
            this.kryptonPanel1.Controls.Add(this.cmbPropertyValue);
            this.kryptonPanel1.Controls.Add(this.cmbProperty);
            this.kryptonPanel1.Controls.Add(this.cmbProdDetail);
            this.kryptonPanel1.Controls.Add(this.cmbProduct);
            this.kryptonPanel1.Controls.Add(this.lblPropertyValue);
            this.kryptonPanel1.Controls.Add(this.lblProperty);
            this.kryptonPanel1.Controls.Add(this.lblProdDetail);
            this.kryptonPanel1.Controls.Add(this.lblProduct);
            this.kryptonPanel1.Controls.Add(this.lblRAR_ID);
            this.kryptonPanel1.Controls.Add(this.txtRAR_ID);
            this.kryptonPanel1.Controls.Add(this.btnCancel);
            this.kryptonPanel1.Controls.Add(this.btnOk);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(500, 300);
            this.kryptonPanel1.TabIndex = 2;
            // 
            // btnReplaceAttrs
            // 
            this.btnReplaceAttrs.Location = new System.Drawing.Point(350, 120);
            this.btnReplaceAttrs.Name = "btnReplaceAttrs";
            this.btnReplaceAttrs.Size = new System.Drawing.Size(90, 25);
            this.btnReplaceAttrs.TabIndex = 13;
            this.btnReplaceAttrs.Values.Text = "替换属性";
            this.btnReplaceAttrs.Click += new System.EventHandler(this.btnReplaceAttrs_Click);
            // 
            // btnClearAttrs
            // 
            this.btnClearAttrs.Location = new System.Drawing.Point(350, 166);
            this.btnClearAttrs.Name = "btnClearAttrs";
            this.btnClearAttrs.Size = new System.Drawing.Size(90, 25);
            this.btnClearAttrs.TabIndex = 12;
            this.btnClearAttrs.Values.Text = "清空属性";
            this.btnClearAttrs.Click += new System.EventHandler(this.btnClearAttrs_Click);
            // 
            // cmbPropertyValue
            // 
            this.cmbPropertyValue.DropDownWidth = 150;
            this.cmbPropertyValue.IntegralHeight = false;
            this.cmbPropertyValue.Location = new System.Drawing.Point(130, 166);
            this.cmbPropertyValue.Name = "cmbPropertyValue";
            this.cmbPropertyValue.Size = new System.Drawing.Size(200, 21);
            this.cmbPropertyValue.TabIndex = 11;
            // 
            // cmbProperty
            // 
            this.cmbProperty.DropDownWidth = 150;
            this.cmbProperty.IntegralHeight = false;
            this.cmbProperty.Location = new System.Drawing.Point(130, 120);
            this.cmbProperty.Name = "cmbProperty";
            this.cmbProperty.Size = new System.Drawing.Size(200, 21);
            this.cmbProperty.TabIndex = 10;
            // 
            // cmbProdDetail
            // 
            this.cmbProdDetail.DropDownWidth = 150;
            this.cmbProdDetail.IntegralHeight = false;
            this.cmbProdDetail.Location = new System.Drawing.Point(130, 90);
            this.cmbProdDetail.Name = "cmbProdDetail";
            this.cmbProdDetail.Size = new System.Drawing.Size(200, 21);
            this.cmbProdDetail.TabIndex = 9;
            this.cmbProdDetail.Visible = false;
            // 
            // cmbProduct
            // 
            this.cmbProduct.DropDownWidth = 150;
            this.cmbProduct.IntegralHeight = false;
            this.cmbProduct.Location = new System.Drawing.Point(130, 60);
            this.cmbProduct.Name = "cmbProduct";
            this.cmbProduct.Size = new System.Drawing.Size(200, 21);
            this.cmbProduct.TabIndex = 8;
            this.cmbProduct.Visible = false;
            this.cmbProduct.SelectedIndexChanged += new System.EventHandler(this.cmbProduct_SelectedIndexChanged);
            // 
            // lblPropertyValue
            // 
            this.lblPropertyValue.Location = new System.Drawing.Point(30, 166);
            this.lblPropertyValue.Name = "lblPropertyValue";
            this.lblPropertyValue.Size = new System.Drawing.Size(62, 20);
            this.lblPropertyValue.TabIndex = 7;
            this.lblPropertyValue.Values.Text = "属性值：";
            // 
            // lblProperty
            // 
            this.lblProperty.Location = new System.Drawing.Point(30, 120);
            this.lblProperty.Name = "lblProperty";
            this.lblProperty.Size = new System.Drawing.Size(49, 20);
            this.lblProperty.TabIndex = 6;
            this.lblProperty.Values.Text = "属性：";
            // 
            // lblProdDetail
            // 
            this.lblProdDetail.Location = new System.Drawing.Point(30, 90);
            this.lblProdDetail.Name = "lblProdDetail";
            this.lblProdDetail.Size = new System.Drawing.Size(105, 20);
            this.lblProdDetail.TabIndex = 5;
            this.lblProdDetail.Values.Text = "产品详情(SKU)：";
            this.lblProdDetail.Visible = false;
            // 
            // lblProduct
            // 
            this.lblProduct.Location = new System.Drawing.Point(30, 60);
            this.lblProduct.Name = "lblProduct";
            this.lblProduct.Size = new System.Drawing.Size(88, 20);
            this.lblProduct.TabIndex = 4;
            this.lblProduct.Values.Text = "产品主信息：";
            this.lblProduct.Visible = false;
            // 
            // lblRAR_ID
            // 
            this.lblRAR_ID.Location = new System.Drawing.Point(30, 30);
            this.lblRAR_ID.Name = "lblRAR_ID";
            this.lblRAR_ID.Size = new System.Drawing.Size(61, 20);
            this.lblRAR_ID.TabIndex = 3;
            this.lblRAR_ID.Values.Text = "关系ID：";
            this.lblRAR_ID.Visible = false;
            // 
            // txtRAR_ID
            // 
            this.txtRAR_ID.Location = new System.Drawing.Point(130, 30);
            this.txtRAR_ID.Name = "txtRAR_ID";
            this.txtRAR_ID.ReadOnly = true;
            this.txtRAR_ID.Size = new System.Drawing.Size(100, 23);
            this.txtRAR_ID.TabIndex = 2;
            this.txtRAR_ID.Visible = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(350, 250);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Values.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(250, 250);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 25);
            this.btnOk.TabIndex = 0;
            this.btnOk.Values.Text = "确定";
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // UCProductAttrRelationEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 300);
            this.Controls.Add(this.kryptonPanel1);
            this.Name = "UCProductAttrRelationEdit";
            this.Text = "产品属性关联编辑";
            ((System.ComponentModel.ISupportInitialize)(this.errorProviderForAllInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.kryptonPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbPropertyValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbProperty)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbProdDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbProduct)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private Krypton.Toolkit.KryptonLabel lblRAR_ID;
        private Krypton.Toolkit.KryptonTextBox txtRAR_ID;
        private Krypton.Toolkit.KryptonButton btnCancel;
        private Krypton.Toolkit.KryptonButton btnOk;
        private Krypton.Toolkit.KryptonComboBox cmbPropertyValue;
        private Krypton.Toolkit.KryptonComboBox cmbProperty;
        private Krypton.Toolkit.KryptonComboBox cmbProdDetail;
        private Krypton.Toolkit.KryptonComboBox cmbProduct;
        private Krypton.Toolkit.KryptonLabel lblPropertyValue;
        private Krypton.Toolkit.KryptonLabel lblProperty;
        private Krypton.Toolkit.KryptonLabel lblProdDetail;
        private Krypton.Toolkit.KryptonLabel lblProduct;
        private Krypton.Toolkit.KryptonButton btnReplaceAttrs;
        private Krypton.Toolkit.KryptonButton btnClearAttrs;
    }
}