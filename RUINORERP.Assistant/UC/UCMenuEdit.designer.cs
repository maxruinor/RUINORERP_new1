namespace RUINORERP.Assistant
{
    partial class UCMenuEdit
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.tree_MainMenu = new System.Windows.Forms.TreeView();
            this.btn_Refresh = new System.Windows.Forms.Button();
            this.btn_add = new System.Windows.Forms.Button();
            this.btn_save = new System.Windows.Forms.Button();
            this.btn_modify = new System.Windows.Forms.Button();
            this.btn_del = new System.Windows.Forms.Button();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.btn_quit = new System.Windows.Forms.Button();
            this.lblName = new System.Windows.Forms.Label();
            this.lblCaption_C = new System.Windows.Forms.Label();
            this.lblCaption_E = new System.Windows.Forms.Label();
            this.txt_MenuName = new System.Windows.Forms.TextBox();
            this.txt_CaptionE = new System.Windows.Forms.TextBox();
            this.txt_CaptonC = new System.Windows.Forms.TextBox();
            this.chkisview = new System.Windows.Forms.CheckBox();
            this.chkEnable = new System.Windows.Forms.CheckBox();
            this.txtDiscription = new System.Windows.Forms.TextBox();
            this.lblDiscription = new System.Windows.Forms.Label();
            this.lblInoformation = new System.Windows.Forms.Label();
            this.lblOID = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnLoad = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.label2 = new System.Windows.Forms.Label();
            this.txtClassPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtAssembly = new System.Windows.Forms.TextBox();
            this.txtFormName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbMenuType = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxTreeView1 = new RUINOR.WinFormsUI.ComboBoxTreeView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // tree_MainMenu
            // 
            this.tree_MainMenu.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tree_MainMenu.Location = new System.Drawing.Point(1, 25);
            this.tree_MainMenu.Name = "tree_MainMenu";
            this.tree_MainMenu.Size = new System.Drawing.Size(250, 469);
            this.tree_MainMenu.TabIndex = 0;
            this.tree_MainMenu.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tree_MainMenu_AfterSelect);
            this.tree_MainMenu.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tree_MainMenu_NodeMouseClick);
            // 
            // btn_Refresh
            // 
            this.btn_Refresh.Location = new System.Drawing.Point(162, 640);
            this.btn_Refresh.Name = "btn_Refresh";
            this.btn_Refresh.Size = new System.Drawing.Size(75, 23);
            this.btn_Refresh.TabIndex = 19;
            this.btn_Refresh.Text = "刷新(&R)";
            this.btn_Refresh.UseVisualStyleBackColor = true;
            this.btn_Refresh.Click += new System.EventHandler(this.btn_Refresh_Click);
            // 
            // btn_add
            // 
            this.btn_add.Location = new System.Drawing.Point(257, 639);
            this.btn_add.Name = "btn_add";
            this.btn_add.Size = new System.Drawing.Size(75, 23);
            this.btn_add.TabIndex = 20;
            this.btn_add.Text = "添加(&A)";
            this.btn_add.UseVisualStyleBackColor = true;
            this.btn_add.Click += new System.EventHandler(this.btn_add_Click);
            // 
            // btn_save
            // 
            this.btn_save.Enabled = false;
            this.btn_save.Location = new System.Drawing.Point(544, 639);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(75, 23);
            this.btn_save.TabIndex = 22;
            this.btn_save.Text = "保存(&S)";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // btn_modify
            // 
            this.btn_modify.Location = new System.Drawing.Point(352, 640);
            this.btn_modify.Name = "btn_modify";
            this.btn_modify.Size = new System.Drawing.Size(75, 23);
            this.btn_modify.TabIndex = 21;
            this.btn_modify.Text = "修改(&M)";
            this.btn_modify.UseVisualStyleBackColor = true;
            this.btn_modify.Click += new System.EventHandler(this.btn_modify_Click);
            // 
            // btn_del
            // 
            this.btn_del.Location = new System.Drawing.Point(447, 639);
            this.btn_del.Name = "btn_del";
            this.btn_del.Size = new System.Drawing.Size(75, 23);
            this.btn_del.TabIndex = 23;
            this.btn_del.Text = "删除(&D)";
            this.btn_del.UseVisualStyleBackColor = true;
            this.btn_del.Click += new System.EventHandler(this.btn_del_Click);
            // 
            // btn_cancel
            // 
            this.btn_cancel.Enabled = false;
            this.btn_cancel.Location = new System.Drawing.Point(637, 639);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(75, 23);
            this.btn_cancel.TabIndex = 24;
            this.btn_cancel.Text = "取消(&C)";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // btn_quit
            // 
            this.btn_quit.Location = new System.Drawing.Point(718, 640);
            this.btn_quit.Name = "btn_quit";
            this.btn_quit.Size = new System.Drawing.Size(75, 23);
            this.btn_quit.TabIndex = 25;
            this.btn_quit.Text = "退出(&Q)";
            this.btn_quit.UseVisualStyleBackColor = true;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(350, 84);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(53, 12);
            this.lblName.TabIndex = 3;
            this.lblName.Text = "菜单名称";
            // 
            // lblCaption_C
            // 
            this.lblCaption_C.AutoSize = true;
            this.lblCaption_C.Location = new System.Drawing.Point(350, 140);
            this.lblCaption_C.Name = "lblCaption_C";
            this.lblCaption_C.Size = new System.Drawing.Size(89, 12);
            this.lblCaption_C.TabIndex = 5;
            this.lblCaption_C.Text = "菜单标题(中文)";
            // 
            // lblCaption_E
            // 
            this.lblCaption_E.AutoSize = true;
            this.lblCaption_E.Location = new System.Drawing.Point(720, 140);
            this.lblCaption_E.Name = "lblCaption_E";
            this.lblCaption_E.Size = new System.Drawing.Size(89, 12);
            this.lblCaption_E.TabIndex = 7;
            this.lblCaption_E.Text = "菜单标题(英文)";
            // 
            // txt_MenuName
            // 
            this.txt_MenuName.Location = new System.Drawing.Point(350, 99);
            this.txt_MenuName.Name = "txt_MenuName";
            this.txt_MenuName.Size = new System.Drawing.Size(167, 21);
            this.txt_MenuName.TabIndex = 4;
            // 
            // txt_CaptionE
            // 
            this.txt_CaptionE.Location = new System.Drawing.Point(722, 160);
            this.txt_CaptionE.Name = "txt_CaptionE";
            this.txt_CaptionE.Size = new System.Drawing.Size(167, 21);
            this.txt_CaptionE.TabIndex = 8;
            // 
            // txt_CaptonC
            // 
            this.txt_CaptonC.Location = new System.Drawing.Point(350, 158);
            this.txt_CaptonC.Name = "txt_CaptonC";
            this.txt_CaptonC.Size = new System.Drawing.Size(167, 21);
            this.txt_CaptonC.TabIndex = 6;
            // 
            // chkisview
            // 
            this.chkisview.AutoSize = true;
            this.chkisview.Checked = true;
            this.chkisview.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkisview.Location = new System.Drawing.Point(723, 207);
            this.chkisview.Name = "chkisview";
            this.chkisview.Size = new System.Drawing.Size(72, 16);
            this.chkisview.TabIndex = 12;
            this.chkisview.Text = "是否可见";
            this.chkisview.UseVisualStyleBackColor = true;
            // 
            // chkEnable
            // 
            this.chkEnable.AutoSize = true;
            this.chkEnable.Checked = true;
            this.chkEnable.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkEnable.Location = new System.Drawing.Point(815, 207);
            this.chkEnable.Name = "chkEnable";
            this.chkEnable.Size = new System.Drawing.Size(72, 16);
            this.chkEnable.TabIndex = 14;
            this.chkEnable.Text = "是否可用";
            this.chkEnable.UseVisualStyleBackColor = true;
            // 
            // txtDiscription
            // 
            this.txtDiscription.Location = new System.Drawing.Point(346, 499);
            this.txtDiscription.Multiline = true;
            this.txtDiscription.Name = "txtDiscription";
            this.txtDiscription.Size = new System.Drawing.Size(609, 54);
            this.txtDiscription.TabIndex = 18;
            // 
            // lblDiscription
            // 
            this.lblDiscription.AutoSize = true;
            this.lblDiscription.Location = new System.Drawing.Point(346, 480);
            this.lblDiscription.Name = "lblDiscription";
            this.lblDiscription.Size = new System.Drawing.Size(53, 12);
            this.lblDiscription.TabIndex = 17;
            this.lblDiscription.Text = "菜单描述";
            // 
            // lblInoformation
            // 
            this.lblInoformation.AutoSize = true;
            this.lblInoformation.Location = new System.Drawing.Point(-1, 9);
            this.lblInoformation.Name = "lblInoformation";
            this.lblInoformation.Size = new System.Drawing.Size(113, 12);
            this.lblInoformation.TabIndex = 26;
            this.lblInoformation.Text = "最多支持四级菜单位";
            // 
            // lblOID
            // 
            this.lblOID.AutoSize = true;
            this.lblOID.Location = new System.Drawing.Point(530, 27);
            this.lblOID.Name = "lblOID";
            this.lblOID.Size = new System.Drawing.Size(0, 12);
            this.lblOID.TabIndex = 29;
            this.lblOID.Visible = false;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.ActiveCaption;
            this.dataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(346, 307);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(609, 152);
            this.dataGridView1.TabIndex = 30;
            this.dataGridView1.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(787, 16);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(102, 23);
            this.btnLoad.TabIndex = 34;
            this.btnLoad.Text = "载入程序集(&L)";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(346, 255);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 35;
            this.label2.Text = "菜单路径";
            // 
            // txtClassPath
            // 
            this.txtClassPath.Location = new System.Drawing.Point(346, 280);
            this.txtClassPath.Name = "txtClassPath";
            this.txtClassPath.Size = new System.Drawing.Size(609, 21);
            this.txtClassPath.TabIndex = 36;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(350, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 37;
            this.label1.Text = "上级菜单节点";
            // 
            // txtAssembly
            // 
            this.txtAssembly.Location = new System.Drawing.Point(350, 53);
            this.txtAssembly.Name = "txtAssembly";
            this.txtAssembly.Size = new System.Drawing.Size(537, 21);
            this.txtAssembly.TabIndex = 40;
            this.txtAssembly.TextChanged += new System.EventHandler(this.txtAssembly_TextChanged);
            // 
            // txtFormName
            // 
            this.txtFormName.Location = new System.Drawing.Point(718, 99);
            this.txtFormName.Name = "txtFormName";
            this.txtFormName.Size = new System.Drawing.Size(167, 21);
            this.txtFormName.TabIndex = 44;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(718, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 43;
            this.label3.Text = "类型名称";
            // 
            // cmbMenuType
            // 
            this.cmbMenuType.FormattingEnabled = true;
            this.cmbMenuType.Items.AddRange(new object[] {
            "行为菜单",
            "导航菜单",
            "直接操作"});
            this.cmbMenuType.Location = new System.Drawing.Point(352, 205);
            this.cmbMenuType.Name = "cmbMenuType";
            this.cmbMenuType.Size = new System.Drawing.Size(165, 20);
            this.cmbMenuType.TabIndex = 45;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(353, 190);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 46;
            this.label4.Text = "菜单类型";
            // 
            // comboBoxTreeView1
            // 
            this.comboBoxTreeView1.FormattingEnabled = true;
            this.comboBoxTreeView1.Location = new System.Drawing.Point(352, 27);
            this.comboBoxTreeView1.Name = "comboBoxTreeView1";
            this.comboBoxTreeView1.Size = new System.Drawing.Size(371, 20);
            this.comboBoxTreeView1.TabIndex = 47;
            // 
            // UCMenuEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.comboBoxTreeView1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmbMenuType);
            this.Controls.Add(this.txtFormName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtAssembly);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtClassPath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.lblOID);
            this.Controls.Add(this.lblInoformation);
            this.Controls.Add(this.lblDiscription);
            this.Controls.Add(this.txtDiscription);
            this.Controls.Add(this.chkEnable);
            this.Controls.Add(this.chkisview);
            this.Controls.Add(this.txt_CaptonC);
            this.Controls.Add(this.txt_CaptionE);
            this.Controls.Add(this.txt_MenuName);
            this.Controls.Add(this.lblCaption_E);
            this.Controls.Add(this.lblCaption_C);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.btn_quit);
            this.Controls.Add(this.btn_del);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_save);
            this.Controls.Add(this.btn_modify);
            this.Controls.Add(this.btn_add);
            this.Controls.Add(this.btn_Refresh);
            this.Controls.Add(this.tree_MainMenu);
            this.Controls.Add(this.dataGridView1);
            this.Name = "UCMenuEdit";
            this.Size = new System.Drawing.Size(969, 702);
            this.Load += new System.EventHandler(this.frmMenuEdit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView tree_MainMenu;
        private System.Windows.Forms.Button btn_Refresh;
        private System.Windows.Forms.Button btn_add;
        private System.Windows.Forms.Button btn_save;
        private System.Windows.Forms.Button btn_modify;
        private System.Windows.Forms.Button btn_del;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.Button btn_quit;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblCaption_C;
        private System.Windows.Forms.Label lblCaption_E;
        private System.Windows.Forms.TextBox txt_MenuName;
        private System.Windows.Forms.TextBox txt_CaptionE;
        private System.Windows.Forms.TextBox txt_CaptonC;
        private System.Windows.Forms.CheckBox chkisview;
        private System.Windows.Forms.CheckBox chkEnable;
        private System.Windows.Forms.TextBox txtDiscription;
        private System.Windows.Forms.Label lblDiscription;
        private System.Windows.Forms.Label lblInoformation;
        private System.Windows.Forms.Label lblOID;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtClassPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtAssembly;
        private System.Windows.Forms.TextBox txtFormName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbMenuType;
        private System.Windows.Forms.Label label4;
        private RUINOR.WinFormsUI.ComboBoxTreeView comboBoxTreeView1;
    }
}