
using WinLib.RegTextBox;
namespace RUINORERP.UI.ProductEAV
{
    partial class UCMultiAttributes
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            WinLib.RegTextBox.RegularAuthenticationSettings regularAuthenticationSettings1 = new WinLib.RegTextBox.RegularAuthenticationSettings();
            WinLib.RegTextBox.RegularAuthenticationSettings regularAuthenticationSettings2 = new WinLib.RegTextBox.RegularAuthenticationSettings();
            WinLib.RegTextBox.RegularAuthenticationSettings regularAuthenticationSettings3 = new WinLib.RegTextBox.RegularAuthenticationSettings();
            WinLib.RegTextBox.RegularAuthenticationSettings regularAuthenticationSettings4 = new WinLib.RegTextBox.RegularAuthenticationSettings();
            WinLib.RegTextBox.RegularAuthenticationSettings regularAuthenticationSettings5 = new WinLib.RegTextBox.RegularAuthenticationSettings();
            WinLib.RegTextBox.RegularAuthenticationSettings regularAuthenticationSettings6 = new WinLib.RegTextBox.RegularAuthenticationSettings();
            this.pictureBoxsku = new System.Windows.Forms.PictureBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSKU = new System.Windows.Forms.TextBox();
            this.dataGridViewMultiAttributes = new System.Windows.Forms.DataGridView();
            this.txtSellPrice = new WinLib.RegTextBox.MyRegTextBox();
            this.txtStock = new WinLib.RegTextBox.MyRegTextBox();
            this.txtPackWeight = new WinLib.RegTextBox.MyRegTextBox();
            this.txtPackQty = new WinLib.RegTextBox.MyRegTextBox();
            this.txtPackSellQty = new WinLib.RegTextBox.MyRegTextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtPackBackUp = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbPackedUnit = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.chk打包销售 = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPurchasePrice = new WinLib.RegTextBox.MyRegTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxsku)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMultiAttributes)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxsku
            // 
            this.pictureBoxsku.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxsku.Location = new System.Drawing.Point(393, 3);
            this.pictureBoxsku.Name = "pictureBoxsku";
            this.pictureBoxsku.Size = new System.Drawing.Size(101, 93);
            this.pictureBoxsku.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxsku.TabIndex = 0;
            this.pictureBoxsku.TabStop = false;
            this.pictureBoxsku.DoubleClick += new System.EventHandler(this.pictureBoxsku_DoubleClick);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(166, 8);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(35, 12);
            this.label12.TabIndex = 189;
            this.label12.Text = "库存:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(166, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 191;
            this.label1.Text = "售价$:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(2, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 194;
            this.label2.Text = "SKU:";
            // 
            // txtSKU
            // 
            this.txtSKU.Location = new System.Drawing.Point(34, 57);
            this.txtSKU.Name = "txtSKU";
            this.txtSKU.Size = new System.Drawing.Size(100, 21);
            this.txtSKU.TabIndex = 193;
            // 
            // dataGridViewMultiAttributes
            // 
            this.dataGridViewMultiAttributes.AllowUserToAddRows = false;
            this.dataGridViewMultiAttributes.AllowUserToDeleteRows = false;
            this.dataGridViewMultiAttributes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.dataGridViewMultiAttributes.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridViewMultiAttributes.Location = new System.Drawing.Point(-1, -1);
            this.dataGridViewMultiAttributes.Name = "dataGridViewMultiAttributes";
            this.dataGridViewMultiAttributes.ReadOnly = true;
            this.dataGridViewMultiAttributes.RowHeadersVisible = false;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.dataGridViewMultiAttributes.RowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewMultiAttributes.RowTemplate.Height = 23;
            this.dataGridViewMultiAttributes.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewMultiAttributes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridViewMultiAttributes.Size = new System.Drawing.Size(164, 48);
            this.dataGridViewMultiAttributes.TabIndex = 195;
            // 
            // txtSellPrice
            // 
            this.txtSellPrice.AllowEmpty = false;
            this.txtSellPrice.Button = null;
            this.txtSellPrice.EmptyMessage = "不能为空";
            this.txtSellPrice.ErrorMessage = "请输入正确的金额格式数据";
            this.txtSellPrice.Location = new System.Drawing.Point(207, 30);
            this.txtSellPrice.Name = "txtSellPrice";
            this.txtSellPrice.RegexExpression = "^([1-9]\\d{0,9}|0)([.]?|(\\.\\d{1,2})?)$";
            regularAuthenticationSettings1.EmptyPopMessage = "不能为空";
            regularAuthenticationSettings1.ErrorMessage = "请输入正确的金额格式数据";
            regularAuthenticationSettings1.RegDescription = "金额";
            regularAuthenticationSettings1.Regularly = "^([1-9]\\d{0,9}|0)([.]?|(\\.\\d{1,2})?)$";
            this.txtSellPrice.SelectRegexExpression = regularAuthenticationSettings1;
            this.txtSellPrice.Size = new System.Drawing.Size(56, 21);
            this.txtSellPrice.TabIndex = 192;
            // 
            // txtStock
            // 
            this.txtStock.AllowEmpty = false;
            this.txtStock.Button = null;
            this.txtStock.EmptyMessage = "不能为空";
            this.txtStock.ErrorMessage = "请输入正确的数字格式数据";
            this.txtStock.Location = new System.Drawing.Point(207, 3);
            this.txtStock.Name = "txtStock";
            this.txtStock.RegexExpression = "^[0-9]*$";
            regularAuthenticationSettings2.EmptyPopMessage = "不能为空";
            regularAuthenticationSettings2.ErrorMessage = "请输入正确的数字格式数据";
            regularAuthenticationSettings2.RegDescription = "数字";
            regularAuthenticationSettings2.Regularly = "^[0-9]*$";
            this.txtStock.SelectRegexExpression = regularAuthenticationSettings2;
            this.txtStock.Size = new System.Drawing.Size(56, 21);
            this.txtStock.TabIndex = 190;
            // 
            // txtPackWeight
            // 
            this.txtPackWeight.AllowEmpty = false;
            this.txtPackWeight.Button = null;
            this.txtPackWeight.EmptyMessage = "不能为空";
            this.txtPackWeight.ErrorMessage = "请输入正确的非零的正整数格式数据";
            this.txtPackWeight.Location = new System.Drawing.Point(339, 27);
            this.txtPackWeight.Name = "txtPackWeight";
            this.txtPackWeight.RegexExpression = "^[1-9]\\d*$";
            regularAuthenticationSettings3.EmptyPopMessage = "不能为空";
            regularAuthenticationSettings3.ErrorMessage = "请输入正确的非零的正整数格式数据";
            regularAuthenticationSettings3.RegDescription = "非零的正整数";
            regularAuthenticationSettings3.Regularly = "^[1-9]\\d*$";
            this.txtPackWeight.SelectRegexExpression = regularAuthenticationSettings3;
            this.txtPackWeight.Size = new System.Drawing.Size(48, 21);
            this.txtPackWeight.TabIndex = 206;
            // 
            // txtPackQty
            // 
            this.txtPackQty.AllowEmpty = false;
            this.txtPackQty.Button = null;
            this.txtPackQty.EmptyMessage = "不能为空";
            this.txtPackQty.ErrorMessage = "请输入正确的数字格式数据";
            this.txtPackQty.Location = new System.Drawing.Point(339, 3);
            this.txtPackQty.Name = "txtPackQty";
            this.txtPackQty.RegexExpression = "^[0-9]*$";
            regularAuthenticationSettings4.EmptyPopMessage = "不能为空";
            regularAuthenticationSettings4.ErrorMessage = "请输入正确的数字格式数据";
            regularAuthenticationSettings4.RegDescription = "数字";
            regularAuthenticationSettings4.Regularly = "^[0-9]*$";
            this.txtPackQty.SelectRegexExpression = regularAuthenticationSettings4;
            this.txtPackQty.Size = new System.Drawing.Size(48, 21);
            this.txtPackQty.TabIndex = 205;
            // 
            // txtPackSellQty
            // 
            this.txtPackSellQty.AllowEmpty = false;
            this.txtPackSellQty.Button = null;
            this.txtPackSellQty.EmptyMessage = "不能为空";
            this.txtPackSellQty.ErrorMessage = "请输入正确的数字格式数据";
            this.txtPackSellQty.Location = new System.Drawing.Point(76, 78);
            this.txtPackSellQty.Name = "txtPackSellQty";
            this.txtPackSellQty.RegexExpression = "^[0-9]*$";
            regularAuthenticationSettings5.EmptyPopMessage = "不能为空";
            regularAuthenticationSettings5.ErrorMessage = "请输入正确的数字格式数据";
            regularAuthenticationSettings5.RegDescription = "数字";
            regularAuthenticationSettings5.Regularly = "^[0-9]*$";
            this.txtPackSellQty.SelectRegexExpression = regularAuthenticationSettings5;
            this.txtPackSellQty.Size = new System.Drawing.Size(57, 21);
            this.txtPackSellQty.TabIndex = 204;
            this.txtPackSellQty.Visible = false;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(136, 78);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(59, 12);
            this.label13.TabIndex = 203;
            this.label13.Text = "包装说明:";
            this.label13.Click += new System.EventHandler(this.label13_Click);
            // 
            // txtPackBackUp
            // 
            this.txtPackBackUp.Location = new System.Drawing.Point(201, 75);
            this.txtPackBackUp.Name = "txtPackBackUp";
            this.txtPackBackUp.Size = new System.Drawing.Size(186, 21);
            this.txtPackBackUp.TabIndex = 202;
            this.txtPackBackUp.TextChanged += new System.EventHandler(this.txtPackBackUp_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(266, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 201;
            this.label3.Text = "包装重量(g):";
            // 
            // cmbPackedUnit
            // 
            this.cmbPackedUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPackedUnit.FormattingEnabled = true;
            this.cmbPackedUnit.Location = new System.Drawing.Point(201, 52);
            this.cmbPackedUnit.Name = "cmbPackedUnit";
            this.cmbPackedUnit.Size = new System.Drawing.Size(62, 20);
            this.cmbPackedUnit.TabIndex = 200;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(136, 55);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(59, 12);
            this.label11.TabIndex = 199;
            this.label11.Text = "包装单位:";
            // 
            // chk打包销售
            // 
            this.chk打包销售.AutoSize = true;
            this.chk打包销售.Location = new System.Drawing.Point(-1, 81);
            this.chk打包销售.Name = "chk打包销售";
            this.chk打包销售.Size = new System.Drawing.Size(72, 16);
            this.chk打包销售.TabIndex = 198;
            this.chk打包销售.Text = "打包销售";
            this.chk打包销售.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(274, 7);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(59, 12);
            this.label10.TabIndex = 197;
            this.label10.Text = "包装数量:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(269, 57);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 12);
            this.label4.TabIndex = 191;
            this.label4.Text = "进价￥:";
            // 
            // txtPurchasePrice
            // 
            this.txtPurchasePrice.AllowEmpty = false;
            this.txtPurchasePrice.Button = null;
            this.txtPurchasePrice.EmptyMessage = "不能为空";
            this.txtPurchasePrice.ErrorMessage = "请输入正确的金额格式数据";
            this.txtPurchasePrice.Location = new System.Drawing.Point(325, 51);
            this.txtPurchasePrice.Name = "txtPurchasePrice";
            this.txtPurchasePrice.RegexExpression = "^([1-9]\\d{0,9}|0)([.]?|(\\.\\d{1,2})?)$";
            regularAuthenticationSettings6.EmptyPopMessage = "不能为空";
            regularAuthenticationSettings6.ErrorMessage = "请输入正确的金额格式数据";
            regularAuthenticationSettings6.RegDescription = "金额";
            regularAuthenticationSettings6.Regularly = "^([1-9]\\d{0,9}|0)([.]?|(\\.\\d{1,2})?)$";
            this.txtPurchasePrice.SelectRegexExpression = regularAuthenticationSettings6;
            this.txtPurchasePrice.Size = new System.Drawing.Size(62, 21);
            this.txtPurchasePrice.TabIndex = 192;
            // 
            // UCMultiAttributes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.txtPackWeight);
            this.Controls.Add(this.txtPackQty);
            this.Controls.Add(this.txtPackSellQty);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.txtPackBackUp);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbPackedUnit);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.chk打包销售);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.dataGridViewMultiAttributes);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtSKU);
            this.Controls.Add(this.txtPurchasePrice);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtSellPrice);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtStock);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.pictureBoxsku);
            this.Name = "UCMultiAttributes";
            this.Size = new System.Drawing.Size(497, 99);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxsku)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMultiAttributes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        internal System.Windows.Forms.DataGridView dataGridViewMultiAttributes;
        internal WinLib.RegTextBox.MyRegTextBox txtStock;
        internal WinLib.RegTextBox.MyRegTextBox txtSellPrice;
        internal System.Windows.Forms.TextBox txtSKU;
        internal System.Windows.Forms.PictureBox pictureBoxsku;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        internal WinLib.RegTextBox.MyRegTextBox txtPackWeight;
        internal WinLib.RegTextBox.MyRegTextBox txtPackQty;
        internal WinLib.RegTextBox.MyRegTextBox txtPackSellQty;
        internal System.Windows.Forms.TextBox txtPackBackUp;
        internal System.Windows.Forms.ComboBox cmbPackedUnit;
        internal System.Windows.Forms.CheckBox chk打包销售;
        private System.Windows.Forms.Label label4;
        internal WinLib.RegTextBox.MyRegTextBox txtPurchasePrice;
    }
}
