using ComponentFactory.Krypton.Toolkit;
using RUINORERP.UI.UControls;

namespace RUINORERP.UI.ProductEAV
{
    partial class frmEavManager
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.kryptonGroupBox1 = new ComponentFactory.Krypton.Toolkit.KryptonGroupBox();
            this.txtSortOrder = new ComponentFactory.Krypton.Toolkit.KryptonNumericUpDown();
            this.btnSaveProp = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnAddOption = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.txtPropertyName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.btnCancelProp = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.txtInputType = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.lblPropertyName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.lblInputType = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.lblSortOrder = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.txtPropertyDesc = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.lblPropertyDesc = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.dataGridView属性 = new RUINORERP.UI.UControls.NewSumDataGridView();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.kryptonGroupBox2 = new ComponentFactory.Krypton.Toolkit.KryptonGroupBox();
            this.txtValueSort = new ComponentFactory.Krypton.Toolkit.KryptonNumericUpDown();
            this.btnSavePropValue = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnAddOptionValue = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnCancelValue = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.lblProperty_ID = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.cmbOption = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
            this.lblPropertyValueName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.txtPropertyValueName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.lblPropertyValueDesc = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.txtPropertyValueDesc = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.kryptonLabel1 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.dataGridView属性值 = new ComponentFactory.Krypton.Toolkit.KryptonDataGridView();
            this.bindingSourceProperty = new System.Windows.Forms.BindingSource(this.components);
            this.bindingSourcePropertyValue = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1.Panel)).BeginInit();
            this.kryptonGroupBox1.Panel.SuspendLayout();
            this.kryptonGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView属性)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox2.Panel)).BeginInit();
            this.kryptonGroupBox2.Panel.SuspendLayout();
            this.kryptonGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbOption)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView属性值)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceProperty)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourcePropertyValue)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer1.Size = new System.Drawing.Size(750, 545);
            this.splitContainer1.SplitterDistance = 368;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.kryptonGroupBox1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.dataGridView属性);
            this.splitContainer2.Size = new System.Drawing.Size(368, 545);
            this.splitContainer2.SplitterDistance = 185;
            this.splitContainer2.TabIndex = 0;
            // 
            // kryptonGroupBox1
            // 
            this.kryptonGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.kryptonGroupBox1.Name = "kryptonGroupBox1";
            // 
            // kryptonGroupBox1.Panel
            // 
            this.kryptonGroupBox1.Panel.Controls.Add(this.txtSortOrder);
            this.kryptonGroupBox1.Panel.Controls.Add(this.btnSaveProp);
            this.kryptonGroupBox1.Panel.Controls.Add(this.btnAddOption);
            this.kryptonGroupBox1.Panel.Controls.Add(this.txtPropertyName);
            this.kryptonGroupBox1.Panel.Controls.Add(this.btnCancelProp);
            this.kryptonGroupBox1.Panel.Controls.Add(this.txtInputType);
            this.kryptonGroupBox1.Panel.Controls.Add(this.lblPropertyName);
            this.kryptonGroupBox1.Panel.Controls.Add(this.lblInputType);
            this.kryptonGroupBox1.Panel.Controls.Add(this.lblSortOrder);
            this.kryptonGroupBox1.Panel.Controls.Add(this.txtPropertyDesc);
            this.kryptonGroupBox1.Panel.Controls.Add(this.lblPropertyDesc);
            this.kryptonGroupBox1.Size = new System.Drawing.Size(368, 185);
            this.kryptonGroupBox1.TabIndex = 0;
            this.kryptonGroupBox1.Values.Heading = "属性";
            // 
            // txtSortOrder
            // 
            this.txtSortOrder.Location = new System.Drawing.Point(118, 58);
            this.txtSortOrder.Name = "txtSortOrder";
            this.txtSortOrder.Size = new System.Drawing.Size(203, 22);
            this.txtSortOrder.TabIndex = 17;
            // 
            // btnSaveProp
            // 
            this.btnSaveProp.Location = new System.Drawing.Point(226, 127);
            this.btnSaveProp.Name = "btnSaveProp";
            this.btnSaveProp.Size = new System.Drawing.Size(56, 25);
            this.btnSaveProp.TabIndex = 16;
            this.btnSaveProp.Values.Text = "保存";
            this.btnSaveProp.Click += new System.EventHandler(this.btn选项保存_Click);
            // 
            // btnAddOption
            // 
            this.btnAddOption.Location = new System.Drawing.Point(55, 127);
            this.btnAddOption.Name = "btnAddOption";
            this.btnAddOption.Size = new System.Drawing.Size(56, 25);
            this.btnAddOption.TabIndex = 15;
            this.btnAddOption.Values.Text = "添加";
            this.btnAddOption.Click += new System.EventHandler(this.btnAddOption_Click);
            // 
            // txtPropertyName
            // 
            this.txtPropertyName.Location = new System.Drawing.Point(118, 8);
            this.txtPropertyName.Name = "txtPropertyName";
            this.txtPropertyName.Size = new System.Drawing.Size(203, 20);
            this.txtPropertyName.TabIndex = 8;
            // 
            // btnCancelProp
            // 
            this.btnCancelProp.Location = new System.Drawing.Point(142, 127);
            this.btnCancelProp.Name = "btnCancelProp";
            this.btnCancelProp.Size = new System.Drawing.Size(56, 25);
            this.btnCancelProp.TabIndex = 6;
            this.btnCancelProp.Values.Text = "取消";
            this.btnCancelProp.Click += new System.EventHandler(this.btnCancelProp_Click);
            // 
            // txtInputType
            // 
            this.txtInputType.Location = new System.Drawing.Point(118, 85);
            this.txtInputType.Name = "txtInputType";
            this.txtInputType.Size = new System.Drawing.Size(203, 20);
            this.txtInputType.TabIndex = 14;
            // 
            // lblPropertyName
            // 
            this.lblPropertyName.Location = new System.Drawing.Point(42, 8);
            this.lblPropertyName.Name = "lblPropertyName";
            this.lblPropertyName.Size = new System.Drawing.Size(60, 20);
            this.lblPropertyName.TabIndex = 7;
            this.lblPropertyName.Values.Text = "属性名称";
            // 
            // lblInputType
            // 
            this.lblInputType.Location = new System.Drawing.Point(42, 85);
            this.lblInputType.Name = "lblInputType";
            this.lblInputType.Size = new System.Drawing.Size(60, 20);
            this.lblInputType.TabIndex = 13;
            this.lblInputType.Values.Text = "输入类型";
            // 
            // lblSortOrder
            // 
            this.lblSortOrder.Location = new System.Drawing.Point(67, 58);
            this.lblSortOrder.Name = "lblSortOrder";
            this.lblSortOrder.Size = new System.Drawing.Size(35, 20);
            this.lblSortOrder.TabIndex = 11;
            this.lblSortOrder.Values.Text = "排序";
            // 
            // txtPropertyDesc
            // 
            this.txtPropertyDesc.Location = new System.Drawing.Point(118, 33);
            this.txtPropertyDesc.Name = "txtPropertyDesc";
            this.txtPropertyDesc.Size = new System.Drawing.Size(203, 20);
            this.txtPropertyDesc.TabIndex = 10;
            // 
            // lblPropertyDesc
            // 
            this.lblPropertyDesc.Location = new System.Drawing.Point(42, 33);
            this.lblPropertyDesc.Name = "lblPropertyDesc";
            this.lblPropertyDesc.Size = new System.Drawing.Size(60, 20);
            this.lblPropertyDesc.TabIndex = 9;
            this.lblPropertyDesc.Values.Text = "属性描述";
            // 
            // dataGridView属性
            // 
            this.dataGridView属性.AllowUserToAddRows = false;
            this.dataGridView属性.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Beige;
            this.dataGridView属性.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView属性.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView属性.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView属性.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView属性.FieldNameList = null;
            this.dataGridView属性.IsShowSumRow = false;
            this.dataGridView属性.Location = new System.Drawing.Point(0, 0);
            this.dataGridView属性.Name = "dataGridView属性";
            this.dataGridView属性.RowTemplate.Height = 23;
            this.dataGridView属性.Size = new System.Drawing.Size(368, 356);
            this.dataGridView属性.SumColumns = null;
            this.dataGridView属性.SummaryDescription = "2020-08最新 带有合计列功能;";
            this.dataGridView属性.SumRowCellFormat = "N2";
            this.dataGridView属性.TabIndex = 0;
            this.dataGridView属性.Use是否使用内置右键功能 = true;
            this.dataGridView属性.XmlFileName = null;
            this.dataGridView属性.DoubleClick += new System.EventHandler(this.dataGridView属性_DoubleClick);
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.kryptonGroupBox2);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.dataGridView属性值);
            this.splitContainer3.Size = new System.Drawing.Size(378, 545);
            this.splitContainer3.SplitterDistance = 182;
            this.splitContainer3.TabIndex = 0;
            // 
            // kryptonGroupBox2
            // 
            this.kryptonGroupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonGroupBox2.Location = new System.Drawing.Point(0, 0);
            this.kryptonGroupBox2.Name = "kryptonGroupBox2";
            // 
            // kryptonGroupBox2.Panel
            // 
            this.kryptonGroupBox2.Panel.Controls.Add(this.txtValueSort);
            this.kryptonGroupBox2.Panel.Controls.Add(this.btnSavePropValue);
            this.kryptonGroupBox2.Panel.Controls.Add(this.btnAddOptionValue);
            this.kryptonGroupBox2.Panel.Controls.Add(this.btnCancelValue);
            this.kryptonGroupBox2.Panel.Controls.Add(this.lblProperty_ID);
            this.kryptonGroupBox2.Panel.Controls.Add(this.cmbOption);
            this.kryptonGroupBox2.Panel.Controls.Add(this.lblPropertyValueName);
            this.kryptonGroupBox2.Panel.Controls.Add(this.txtPropertyValueName);
            this.kryptonGroupBox2.Panel.Controls.Add(this.lblPropertyValueDesc);
            this.kryptonGroupBox2.Panel.Controls.Add(this.txtPropertyValueDesc);
            this.kryptonGroupBox2.Panel.Controls.Add(this.kryptonLabel1);
            this.kryptonGroupBox2.Size = new System.Drawing.Size(378, 182);
            this.kryptonGroupBox2.TabIndex = 7;
            this.kryptonGroupBox2.Values.Heading = "属性值";
            // 
            // txtValueSort
            // 
            this.txtValueSort.Location = new System.Drawing.Point(145, 58);
            this.txtValueSort.Name = "txtValueSort";
            this.txtValueSort.Size = new System.Drawing.Size(203, 22);
            this.txtValueSort.TabIndex = 17;
            // 
            // btnSavePropValue
            // 
            this.btnSavePropValue.Location = new System.Drawing.Point(274, 124);
            this.btnSavePropValue.Name = "btnSavePropValue";
            this.btnSavePropValue.Size = new System.Drawing.Size(56, 25);
            this.btnSavePropValue.TabIndex = 15;
            this.btnSavePropValue.Values.Text = "保存";
            this.btnSavePropValue.Click += new System.EventHandler(this.btn选项值保存_Click);
            // 
            // btnAddOptionValue
            // 
            this.btnAddOptionValue.Location = new System.Drawing.Point(72, 124);
            this.btnAddOptionValue.Name = "btnAddOptionValue";
            this.btnAddOptionValue.Size = new System.Drawing.Size(56, 25);
            this.btnAddOptionValue.TabIndex = 14;
            this.btnAddOptionValue.Values.Text = "添加";
            this.btnAddOptionValue.Click += new System.EventHandler(this.btnAddOptionValue_Click);
            // 
            // btnCancelValue
            // 
            this.btnCancelValue.Location = new System.Drawing.Point(175, 124);
            this.btnCancelValue.Name = "btnCancelValue";
            this.btnCancelValue.Size = new System.Drawing.Size(56, 25);
            this.btnCancelValue.TabIndex = 7;
            this.btnCancelValue.Values.Text = "取消";
            this.btnCancelValue.Click += new System.EventHandler(this.btnCancelValue_Click);
            // 
            // lblProperty_ID
            // 
            this.lblProperty_ID.Location = new System.Drawing.Point(93, 87);
            this.lblProperty_ID.Name = "lblProperty_ID";
            this.lblProperty_ID.Size = new System.Drawing.Size(35, 20);
            this.lblProperty_ID.TabIndex = 6;
            this.lblProperty_ID.Values.Text = "属性";
            // 
            // cmbOption
            // 
            this.cmbOption.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmbOption.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOption.DropDownWidth = 246;
            this.cmbOption.FormattingEnabled = true;
            this.cmbOption.Location = new System.Drawing.Point(145, 87);
            this.cmbOption.Name = "cmbOption";
            this.cmbOption.Size = new System.Drawing.Size(202, 21);
            this.cmbOption.TabIndex = 6;
            // 
            // lblPropertyValueName
            // 
            this.lblPropertyValueName.Location = new System.Drawing.Point(55, 14);
            this.lblPropertyValueName.Name = "lblPropertyValueName";
            this.lblPropertyValueName.Size = new System.Drawing.Size(73, 20);
            this.lblPropertyValueName.TabIndex = 8;
            this.lblPropertyValueName.Values.Text = "属性值名称";
            // 
            // txtPropertyValueName
            // 
            this.txtPropertyValueName.Location = new System.Drawing.Point(145, 10);
            this.txtPropertyValueName.Name = "txtPropertyValueName";
            this.txtPropertyValueName.Size = new System.Drawing.Size(202, 20);
            this.txtPropertyValueName.TabIndex = 9;
            // 
            // lblPropertyValueDesc
            // 
            this.lblPropertyValueDesc.Location = new System.Drawing.Point(55, 39);
            this.lblPropertyValueDesc.Name = "lblPropertyValueDesc";
            this.lblPropertyValueDesc.Size = new System.Drawing.Size(73, 20);
            this.lblPropertyValueDesc.TabIndex = 10;
            this.lblPropertyValueDesc.Values.Text = "属性值描述";
            // 
            // txtPropertyValueDesc
            // 
            this.txtPropertyValueDesc.Location = new System.Drawing.Point(145, 35);
            this.txtPropertyValueDesc.Name = "txtPropertyValueDesc";
            this.txtPropertyValueDesc.Size = new System.Drawing.Size(202, 20);
            this.txtPropertyValueDesc.TabIndex = 11;
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.Location = new System.Drawing.Point(93, 64);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(35, 20);
            this.kryptonLabel1.TabIndex = 12;
            this.kryptonLabel1.Values.Text = "排序";
            // 
            // dataGridView属性值
            // 
            this.dataGridView属性值.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView属性值.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView属性值.Location = new System.Drawing.Point(0, 0);
            this.dataGridView属性值.Name = "dataGridView属性值";
            this.dataGridView属性值.RowTemplate.Height = 23;
            this.dataGridView属性值.Size = new System.Drawing.Size(378, 359);
            this.dataGridView属性值.TabIndex = 0;
            this.dataGridView属性值.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView属性值_CellFormatting_1);
            this.dataGridView属性值.DoubleClick += new System.EventHandler(this.dataGridView属性值_DoubleClick);
            // 
            // frmEavManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "frmEavManager";
            this.Size = new System.Drawing.Size(750, 545);
            this.Load += new System.EventHandler(this.frmEavManager_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1.Panel)).EndInit();
            this.kryptonGroupBox1.Panel.ResumeLayout(false);
            this.kryptonGroupBox1.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1)).EndInit();
            this.kryptonGroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView属性)).EndInit();
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox2.Panel)).EndInit();
            this.kryptonGroupBox2.Panel.ResumeLayout(false);
            this.kryptonGroupBox2.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox2)).EndInit();
            this.kryptonGroupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cmbOption)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView属性值)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceProperty)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourcePropertyValue)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private KryptonComboBox cmbOption;
        private System.Windows.Forms.BindingSource bindingSourceProperty;
        private System.Windows.Forms.BindingSource bindingSourcePropertyValue;
        private KryptonButton btnCancelProp;
        private KryptonButton btnCancelValue;
        private KryptonDataGridView dataGridView属性值;
        private NewSumDataGridView dataGridView属性;
        private KryptonGroupBox kryptonGroupBox1;
        private KryptonLabel lblPropertyName;
        private KryptonTextBox txtPropertyName;
        private KryptonLabel lblPropertyDesc;
        private KryptonTextBox txtPropertyDesc;
        private KryptonLabel lblSortOrder;
        private KryptonLabel lblInputType;
        private KryptonTextBox txtInputType;
        private KryptonButton btnSaveProp;
        private KryptonButton btnAddOption;
        private KryptonGroupBox kryptonGroupBox2;
        private KryptonLabel lblProperty_ID;
        private KryptonLabel lblPropertyValueName;
        private KryptonTextBox txtPropertyValueName;
        private KryptonLabel lblPropertyValueDesc;
        private KryptonTextBox txtPropertyValueDesc;
        private KryptonLabel kryptonLabel1;
        private KryptonButton btnSavePropValue;
        private KryptonButton btnAddOptionValue;
        private KryptonNumericUpDown txtSortOrder;
        private KryptonNumericUpDown txtValueSort;
    }
}