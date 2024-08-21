namespace WinLib.RegTextBox
{
    partial class RegEditorSelectControl
    {




        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.regularAuthenticationSettingsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.regDescriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.regularlyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.errorMessageDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.regularAuthenticationSettingsBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.regDescriptionDataGridViewTextBoxColumn,
            this.regularlyDataGridViewTextBoxColumn,
            this.errorMessageDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.regularAuthenticationSettingsBindingSource;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(304, 103);
            this.dataGridView1.TabIndex = 3;
            this.dataGridView1.DoubleClick += new System.EventHandler(this.dataGridView1_DoubleClick);
            // 
            // regularAuthenticationSettingsBindingSource
            // 
            this.regularAuthenticationSettingsBindingSource.DataSource = typeof(WinLib.RegTextBox.RegularAuthenticationSettings);
            // 
            // regDescriptionDataGridViewTextBoxColumn
            // 
            this.regDescriptionDataGridViewTextBoxColumn.DataPropertyName = "RegDescription";
            this.regDescriptionDataGridViewTextBoxColumn.HeaderText = "RegDescription";
            this.regDescriptionDataGridViewTextBoxColumn.Name = "regDescriptionDataGridViewTextBoxColumn";
            this.regDescriptionDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // regularlyDataGridViewTextBoxColumn
            // 
            this.regularlyDataGridViewTextBoxColumn.DataPropertyName = "Regularly";
            this.regularlyDataGridViewTextBoxColumn.HeaderText = "Regularly";
            this.regularlyDataGridViewTextBoxColumn.Name = "regularlyDataGridViewTextBoxColumn";
            this.regularlyDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // errorMessageDataGridViewTextBoxColumn
            // 
            this.errorMessageDataGridViewTextBoxColumn.DataPropertyName = "ErrorMessage";
            this.errorMessageDataGridViewTextBoxColumn.HeaderText = "ErrorMessage";
            this.errorMessageDataGridViewTextBoxColumn.Name = "errorMessageDataGridViewTextBoxColumn";
            this.errorMessageDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // RegEditorSelectControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridView1);
            this.Name = "RegEditorSelectControl";
            this.Size = new System.Drawing.Size(304, 103);
            this.Load += new System.EventHandler(this.RegEditorSelectControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.regularAuthenticationSettingsBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion


 

    }
}
