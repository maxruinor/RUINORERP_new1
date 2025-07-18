﻿namespace RUINORERP.UI.ForCustomizeGrid
{
    partial class frmColumnsSets
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
            this.listView1 = new System.Windows.Forms.ListView();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.chkReverseSelection = new Krypton.Toolkit.KryptonCheckBox();
            this.chkAll = new Krypton.Toolkit.KryptonCheckBox();
            this.btnRestoreDefaultConfig = new Krypton.Toolkit.KryptonButton();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.CheckBoxes = true;
            this.listView1.Dock = System.Windows.Forms.DockStyle.Top;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(383, 431);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(112, 435);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(72, 33);
            this.btnOk.TabIndex = 5;
            this.btnOk.Text = "确定(&O)";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(189, 435);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(72, 33);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "取消(&C)";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // chkReverseSelection
            // 
            this.chkReverseSelection.Location = new System.Drawing.Point(54, 434);
            this.chkReverseSelection.Name = "chkReverseSelection";
            this.chkReverseSelection.Size = new System.Drawing.Size(49, 20);
            this.chkReverseSelection.TabIndex = 10;
            this.chkReverseSelection.Values.Text = "反选";
            this.chkReverseSelection.CheckedChanged += new System.EventHandler(this.chkReverseSelection_CheckedChanged);
            // 
            // chkAll
            // 
            this.chkAll.Location = new System.Drawing.Point(3, 434);
            this.chkAll.Name = "chkAll";
            this.chkAll.Size = new System.Drawing.Size(49, 20);
            this.chkAll.TabIndex = 9;
            this.chkAll.Values.Text = "全选";
            this.chkAll.CheckedChanged += new System.EventHandler(this.chkAll_CheckedChanged);
            // 
            // btnRestoreDefaultConfig
            // 
            this.btnRestoreDefaultConfig.Location = new System.Drawing.Point(286, 435);
            this.btnRestoreDefaultConfig.Name = "btnRestoreDefaultConfig";
            this.btnRestoreDefaultConfig.Size = new System.Drawing.Size(87, 33);
            this.btnRestoreDefaultConfig.TabIndex = 11;
            this.btnRestoreDefaultConfig.Values.Text = "恢复默认值";
            this.btnRestoreDefaultConfig.Visible = false;
            this.btnRestoreDefaultConfig.Click += new System.EventHandler(this.btnMoreSetting_Click);
            // 
            // frmColumnsSets
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(383, 479);
            this.Controls.Add(this.btnRestoreDefaultConfig);
            this.Controls.Add(this.chkReverseSelection);
            this.Controls.Add(this.chkAll);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.listView1);
            this.Name = "frmColumnsSets";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "自定义列显示";
            this.Load += new System.EventHandler(this.frmColumnsSets_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private Krypton.Toolkit.KryptonCheckBox chkReverseSelection;
        private Krypton.Toolkit.KryptonCheckBox chkAll;
        private Krypton.Toolkit.KryptonButton btnRestoreDefaultConfig;
    }
}