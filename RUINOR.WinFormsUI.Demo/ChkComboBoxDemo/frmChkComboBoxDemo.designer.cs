using RUINOR.WinFormsUI.ChkComboBox;

namespace RUINOR.WinFormsUI.Demo.ChkComboBoxDemo
{
    partial class frmChkComboBoxDemo
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
            RUINOR.WinFormsUI.ChkComboBox.CheckBoxProperties checkBoxProperties5 = new RUINOR.WinFormsUI.ChkComboBox.CheckBoxProperties();
            RUINOR.WinFormsUI.ChkComboBox.CheckBoxProperties checkBoxProperties1 = new RUINOR.WinFormsUI.ChkComboBox.CheckBoxProperties();
            RUINOR.WinFormsUI.ChkComboBox.CheckBoxProperties checkBoxProperties2 = new RUINOR.WinFormsUI.ChkComboBox.CheckBoxProperties();
            RUINOR.WinFormsUI.ChkComboBox.CheckBoxProperties checkBoxProperties3 = new RUINOR.WinFormsUI.ChkComboBox.CheckBoxProperties();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnCheckItem1 = new System.Windows.Forms.Button();
            this.btnCheckInserted = new System.Windows.Forms.Button();
            this.btnCheckDDDD = new System.Windows.Forms.Button();
            this.btnCheckItem5 = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.checkBoxComboBox1 = new RUINOR.WinFormsUI.ChkComboBox.CheckBoxComboBox();
            this.cmbDataTableDataSource = new RUINOR.WinFormsUI.ChkComboBox.CheckBoxComboBox();
            this.cmbIListDataSource = new RUINOR.WinFormsUI.ChkComboBox.CheckBoxComboBox();
            this.cmbManual = new RUINOR.WinFormsUI.ChkComboBox.CheckBoxComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(239, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "Populated Manually using ComboBox.Items";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(251, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "Populated using a custom IList DataSource";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(233, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "Populated using a DataTable DataSource";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 133);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(299, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "A different look. Accessed via CheckBoxProperties";
            // 
            // btnCheckItem1
            // 
            this.btnCheckItem1.Location = new System.Drawing.Point(208, 20);
            this.btnCheckItem1.Name = "btnCheckItem1";
            this.btnCheckItem1.Size = new System.Drawing.Size(128, 21);
            this.btnCheckItem1.TabIndex = 8;
            this.btnCheckItem1.Text = "! Check \"Item 1\"";
            this.btnCheckItem1.UseVisualStyleBackColor = true;
            this.btnCheckItem1.Click += new System.EventHandler(this.btnCheckItem1_Click);
            // 
            // btnCheckInserted
            // 
            this.btnCheckInserted.Location = new System.Drawing.Point(208, 60);
            this.btnCheckInserted.Name = "btnCheckInserted";
            this.btnCheckInserted.Size = new System.Drawing.Size(128, 21);
            this.btnCheckInserted.TabIndex = 9;
            this.btnCheckInserted.Text = "! Check \"Inserted\"";
            this.btnCheckInserted.UseVisualStyleBackColor = true;
            this.btnCheckInserted.Click += new System.EventHandler(this.btnCheckInserted_Click);
            // 
            // btnCheckDDDD
            // 
            this.btnCheckDDDD.Location = new System.Drawing.Point(208, 107);
            this.btnCheckDDDD.Name = "btnCheckDDDD";
            this.btnCheckDDDD.Size = new System.Drawing.Size(128, 21);
            this.btnCheckDDDD.TabIndex = 10;
            this.btnCheckDDDD.Text = "! Check \"DDDD\"";
            this.btnCheckDDDD.UseVisualStyleBackColor = true;
            this.btnCheckDDDD.Click += new System.EventHandler(this.btnCheckDDDD_Click);
            // 
            // btnCheckItem5
            // 
            this.btnCheckItem5.Location = new System.Drawing.Point(208, 149);
            this.btnCheckItem5.Name = "btnCheckItem5";
            this.btnCheckItem5.Size = new System.Drawing.Size(128, 21);
            this.btnCheckItem5.TabIndex = 11;
            this.btnCheckItem5.Text = "! Check \"Item 5\"";
            this.btnCheckItem5.UseVisualStyleBackColor = true;
            this.btnCheckItem5.Click += new System.EventHandler(this.btnCheckItem5_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(342, 20);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(135, 21);
            this.btnClear.TabIndex = 12;
            this.btnClear.Text = "Clear && Repopulate";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // checkBoxComboBox1
            // 
            checkBoxProperties5.AutoSize = true;
            checkBoxProperties5.CheckAlign = System.Drawing.ContentAlignment.BottomCenter;
            checkBoxProperties5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            checkBoxProperties5.ForeColor = System.Drawing.SystemColors.ControlText;
            checkBoxProperties5.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.checkBoxComboBox1.CheckBoxProperties = checkBoxProperties5;
            this.checkBoxComboBox1.DisplayMemberSingleItem = "";
            this.checkBoxComboBox1.FormattingEnabled = true;
            this.checkBoxComboBox1.Items.AddRange(new object[] {
            "Item 1",
            "Item 2",
            "Item 3",
            "Item 4",
            "Item 5",
            "Item 6"});
            this.checkBoxComboBox1.Location = new System.Drawing.Point(3, 149);
            this.checkBoxComboBox1.Name = "checkBoxComboBox1";
            this.checkBoxComboBox1.Size = new System.Drawing.Size(151, 20);
            this.checkBoxComboBox1.TabIndex = 7;
            // 
            // cmbDataTableDataSource
            // 
            checkBoxProperties1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbDataTableDataSource.CheckBoxProperties = checkBoxProperties1;
            this.cmbDataTableDataSource.DisplayMemberSingleItem = "";
            this.cmbDataTableDataSource.FormattingEnabled = true;
            this.cmbDataTableDataSource.Location = new System.Drawing.Point(3, 107);
            this.cmbDataTableDataSource.Name = "cmbDataTableDataSource";
            this.cmbDataTableDataSource.Size = new System.Drawing.Size(152, 20);
            this.cmbDataTableDataSource.TabIndex = 5;
            // 
            // cmbIListDataSource
            // 
            checkBoxProperties2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbIListDataSource.CheckBoxProperties = checkBoxProperties2;
            this.cmbIListDataSource.DisplayMemberSingleItem = "";
            this.cmbIListDataSource.FormattingEnabled = true;
            this.cmbIListDataSource.Location = new System.Drawing.Point(3, 62);
            this.cmbIListDataSource.Name = "cmbIListDataSource";
            this.cmbIListDataSource.Size = new System.Drawing.Size(152, 20);
            this.cmbIListDataSource.TabIndex = 3;
            // 
            // cmbManual
            // 
            checkBoxProperties3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmbManual.CheckBoxProperties = checkBoxProperties3;
            this.cmbManual.DisplayMemberSingleItem = "";
            this.cmbManual.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbManual.FormattingEnabled = true;
            this.cmbManual.Location = new System.Drawing.Point(3, 20);
            this.cmbManual.Name = "cmbManual";
            this.cmbManual.Size = new System.Drawing.Size(151, 20);
            this.cmbManual.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(370, 57);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 13;
            this.button1.Text = "query";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // frmChkComboBoxDemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(489, 183);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnCheckItem5);
            this.Controls.Add(this.btnCheckDDDD);
            this.Controls.Add(this.btnCheckInserted);
            this.Controls.Add(this.btnCheckItem1);
            this.Controls.Add(this.checkBoxComboBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmbDataTableDataSource);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbIListDataSource);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbManual);
            this.Name = "frmChkComboBoxDemo";
            this.Text = "DEMO of CheckBoxComboBox";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CheckBoxComboBox cmbManual;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private CheckBoxComboBox cmbIListDataSource;
        private CheckBoxComboBox cmbDataTableDataSource;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private CheckBoxComboBox checkBoxComboBox1;
        private System.Windows.Forms.Button btnCheckItem1;
        private System.Windows.Forms.Button btnCheckInserted;
        private System.Windows.Forms.Button btnCheckDDDD;
        private System.Windows.Forms.Button btnCheckItem5;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button button1;
    }
}

