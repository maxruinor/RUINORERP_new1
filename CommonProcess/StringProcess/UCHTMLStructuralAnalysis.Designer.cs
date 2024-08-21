using HLH.WinControl.ComBoBoxEx;
namespace CommonProcess.StringProcess
{
    partial class UCHTMLStructuralAnalysis
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
            this.components = new System.ComponentModel.Container();
            CheckBoxProperties checkBoxProperties5 = new CheckBoxProperties();
            CheckBoxProperties checkBoxProperties1 = new CheckBoxProperties();
            CheckBoxProperties checkBoxProperties2 = new CheckBoxProperties();
            CheckBoxProperties checkBoxProperties3 = new CheckBoxProperties();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.rdbSaveNode = new System.Windows.Forms.RadioButton();
            this.rdbRemoveNode = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.chkInputXpath = new System.Windows.Forms.CheckBox();
            this.txtXpath = new System.Windows.Forms.TextBox();
            this.chkOutPutFindNodesForXpath = new System.Windows.Forms.CheckBox();
            this.chk是否包含标签本身 = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.cmb指定标签 = new CheckBoxComboBox();
            this.cmb空内容标签 = new CheckBoxComboBox();
            this.lvForHtml = new System.Windows.Forms.ListView();
            this.rdb移除指定标签格式 = new System.Windows.Forms.RadioButton();
            this.gb指定要移除的标签 = new System.Windows.Forms.GroupBox();
            this.chkAll指定标签的属性 = new System.Windows.Forms.CheckBox();
            this.chk指定标签属性 = new System.Windows.Forms.CheckBox();
            this.cmb指定标签的属性 = new CheckBoxComboBox();
            this.chkSelectAll指定标签 = new System.Windows.Forms.CheckBox();
            this.chk指定标签 = new System.Windows.Forms.CheckBox();
            this.chkSelectAll空内容标签 = new System.Windows.Forms.CheckBox();
            this.chkSelectAll标签属性 = new System.Windows.Forms.CheckBox();
            this.chk空内容标签 = new System.Windows.Forms.CheckBox();
            this.chk标签属性 = new System.Windows.Forms.CheckBox();
            this.cmb标签属性 = new CheckBoxComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chk只操作标签本身 = new System.Windows.Forms.CheckBox();
            this.chk如果节点内容唯一 = new System.Windows.Forms.CheckBox();
            this.gb指定要移除的标签.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.CheckBoxes = true;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Left;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(293, 253);
            this.treeView1.TabIndex = 0;
            this.toolTip1.SetToolTip(this.treeView1, "如果选择多项，必须是相同层级关系");
            this.treeView1.DoubleClick += new System.EventHandler(this.treeView1_DoubleClick);
            // 
            // rdbSaveNode
            // 
            this.rdbSaveNode.AutoSize = true;
            this.rdbSaveNode.Checked = true;
            this.rdbSaveNode.Location = new System.Drawing.Point(519, 67);
            this.rdbSaveNode.Name = "rdbSaveNode";
            this.rdbSaveNode.Size = new System.Drawing.Size(95, 16);
            this.rdbSaveNode.TabIndex = 1;
            this.rdbSaveNode.TabStop = true;
            this.rdbSaveNode.Text = "保存选择节点";
            this.rdbSaveNode.UseVisualStyleBackColor = true;
            this.rdbSaveNode.CheckedChanged += new System.EventHandler(this.rdbSaveNode_CheckedChanged);
            // 
            // rdbRemoveNode
            // 
            this.rdbRemoveNode.AutoSize = true;
            this.rdbRemoveNode.Location = new System.Drawing.Point(630, 67);
            this.rdbRemoveNode.Name = "rdbRemoveNode";
            this.rdbRemoveNode.Size = new System.Drawing.Size(95, 16);
            this.rdbRemoveNode.TabIndex = 1;
            this.rdbRemoveNode.Text = "移除选择节点";
            this.rdbRemoveNode.UseVisualStyleBackColor = true;
            this.rdbRemoveNode.CheckedChanged += new System.EventHandler(this.rdbRemoveNode_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(306, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(218, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "加载选中的字段内容作为参考结构";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // chkInputXpath
            // 
            this.chkInputXpath.AutoSize = true;
            this.chkInputXpath.Location = new System.Drawing.Point(530, 9);
            this.chkInputXpath.Name = "chkInputXpath";
            this.chkInputXpath.Size = new System.Drawing.Size(102, 16);
            this.chkInputXpath.TabIndex = 3;
            this.chkInputXpath.Text = "手工输入XPath";
            this.chkInputXpath.UseVisualStyleBackColor = true;
            this.chkInputXpath.CheckedChanged += new System.EventHandler(this.chkInputXpath_CheckedChanged);
            // 
            // txtXpath
            // 
            this.txtXpath.Location = new System.Drawing.Point(635, 5);
            this.txtXpath.Name = "txtXpath";
            this.txtXpath.Size = new System.Drawing.Size(218, 21);
            this.txtXpath.TabIndex = 4;
            this.txtXpath.Visible = false;
            // 
            // chkOutPutFindNodesForXpath
            // 
            this.chkOutPutFindNodesForXpath.AutoSize = true;
            this.chkOutPutFindNodesForXpath.Location = new System.Drawing.Point(649, 30);
            this.chkOutPutFindNodesForXpath.Name = "chkOutPutFindNodesForXpath";
            this.chkOutPutFindNodesForXpath.Size = new System.Drawing.Size(96, 16);
            this.chkOutPutFindNodesForXpath.TabIndex = 5;
            this.chkOutPutFindNodesForXpath.Text = "显示节点数据";
            this.chkOutPutFindNodesForXpath.UseVisualStyleBackColor = true;
            // 
            // chk是否包含标签本身
            // 
            this.chk是否包含标签本身.AutoSize = true;
            this.chk是否包含标签本身.Checked = true;
            this.chk是否包含标签本身.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk是否包含标签本身.Location = new System.Drawing.Point(547, 30);
            this.chk是否包含标签本身.Name = "chk是否包含标签本身";
            this.chk是否包含标签本身.Size = new System.Drawing.Size(96, 16);
            this.chk是否包含标签本身.TabIndex = 6;
            this.chk是否包含标签本身.Text = "包含标签本身";
            this.chk是否包含标签本身.UseVisualStyleBackColor = true;
            // 
            // cmb指定标签
            // 
            checkBoxProperties5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmb指定标签.CheckBoxProperties = checkBoxProperties5;
            this.cmb指定标签.DisplayMemberSingleItem = "";
            this.cmb指定标签.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb指定标签.FormattingEnabled = true;
            this.cmb指定标签.Location = new System.Drawing.Point(339, 121);
            this.cmb指定标签.Name = "cmb指定标签";
            this.cmb指定标签.Size = new System.Drawing.Size(168, 20);
            this.cmb指定标签.TabIndex = 72;
            this.toolTip1.SetToolTip(this.cmb指定标签, "只移除标签本身，不包含内容");
            // 
            // cmb空内容标签
            // 
            checkBoxProperties1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmb空内容标签.CheckBoxProperties = checkBoxProperties1;
            this.cmb空内容标签.DisplayMemberSingleItem = "";
            this.cmb空内容标签.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb空内容标签.FormattingEnabled = true;
            this.cmb空内容标签.Location = new System.Drawing.Point(339, 89);
            this.cmb空内容标签.Name = "cmb空内容标签";
            this.cmb空内容标签.Size = new System.Drawing.Size(168, 20);
            this.cmb空内容标签.TabIndex = 69;
            this.toolTip1.SetToolTip(this.cmb空内容标签, "只移除标签本身，包含内容");
            // 
            // lvForHtml
            // 
            this.lvForHtml.CheckBoxes = true;
            this.lvForHtml.Dock = System.Windows.Forms.DockStyle.Left;
            this.lvForHtml.Location = new System.Drawing.Point(3, 17);
            this.lvForHtml.Name = "lvForHtml";
            this.lvForHtml.ShowGroups = false;
            this.lvForHtml.Size = new System.Drawing.Size(211, 140);
            this.lvForHtml.TabIndex = 60;
            this.lvForHtml.UseCompatibleStateImageBehavior = false;
            // 
            // rdb移除指定标签格式
            // 
            this.rdb移除指定标签格式.AutoSize = true;
            this.rdb移除指定标签格式.Location = new System.Drawing.Point(308, 67);
            this.rdb移除指定标签格式.Name = "rdb移除指定标签格式";
            this.rdb移除指定标签格式.Size = new System.Drawing.Size(119, 16);
            this.rdb移除指定标签格式.TabIndex = 61;
            this.rdb移除指定标签格式.Text = "移除指定标签格式";
            this.rdb移除指定标签格式.UseVisualStyleBackColor = true;
            this.rdb移除指定标签格式.CheckedChanged += new System.EventHandler(this.rdb移除指定标签格式_CheckedChanged);
            // 
            // gb指定要移除的标签
            // 
            this.gb指定要移除的标签.Controls.Add(this.chkAll指定标签的属性);
            this.gb指定要移除的标签.Controls.Add(this.chk指定标签属性);
            this.gb指定要移除的标签.Controls.Add(this.cmb指定标签的属性);
            this.gb指定要移除的标签.Controls.Add(this.chkSelectAll指定标签);
            this.gb指定要移除的标签.Controls.Add(this.chk指定标签);
            this.gb指定要移除的标签.Controls.Add(this.cmb指定标签);
            this.gb指定要移除的标签.Controls.Add(this.chkSelectAll空内容标签);
            this.gb指定要移除的标签.Controls.Add(this.chkSelectAll标签属性);
            this.gb指定要移除的标签.Controls.Add(this.chk空内容标签);
            this.gb指定要移除的标签.Controls.Add(this.chk标签属性);
            this.gb指定要移除的标签.Controls.Add(this.cmb空内容标签);
            this.gb指定要移除的标签.Controls.Add(this.cmb标签属性);
            this.gb指定要移除的标签.Controls.Add(this.lvForHtml);
            this.gb指定要移除的标签.Location = new System.Drawing.Point(310, 89);
            this.gb指定要移除的标签.Name = "gb指定要移除的标签";
            this.gb指定要移除的标签.Size = new System.Drawing.Size(558, 160);
            this.gb指定要移除的标签.TabIndex = 62;
            this.gb指定要移除的标签.TabStop = false;
            this.gb指定要移除的标签.Text = "指定要移除的标签";
            this.gb指定要移除的标签.Visible = false;
            // 
            // chkAll指定标签的属性
            // 
            this.chkAll指定标签的属性.AutoSize = true;
            this.chkAll指定标签的属性.Location = new System.Drawing.Point(513, 56);
            this.chkAll指定标签的属性.Name = "chkAll指定标签的属性";
            this.chkAll指定标签的属性.Size = new System.Drawing.Size(48, 16);
            this.chkAll指定标签的属性.TabIndex = 76;
            this.chkAll指定标签的属性.Text = "全选";
            this.chkAll指定标签的属性.UseVisualStyleBackColor = true;
            // 
            // chk指定标签属性
            // 
            this.chk指定标签属性.AutoSize = true;
            this.chk指定标签属性.Location = new System.Drawing.Point(226, 56);
            this.chk指定标签属性.Name = "chk指定标签属性";
            this.chk指定标签属性.Size = new System.Drawing.Size(108, 16);
            this.chk指定标签属性.TabIndex = 74;
            this.chk指定标签属性.Text = "指定标签的属性";
            this.chk指定标签属性.UseVisualStyleBackColor = true;
            // 
            // cmb指定标签的属性
            // 
            checkBoxProperties2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmb指定标签的属性.CheckBoxProperties = checkBoxProperties2;
            this.cmb指定标签的属性.DisplayMemberSingleItem = "";
            this.cmb指定标签的属性.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb指定标签的属性.FormattingEnabled = true;
            this.cmb指定标签的属性.Location = new System.Drawing.Point(339, 54);
            this.cmb指定标签的属性.Name = "cmb指定标签的属性";
            this.cmb指定标签的属性.Size = new System.Drawing.Size(168, 20);
            this.cmb指定标签的属性.TabIndex = 75;
            // 
            // chkSelectAll指定标签
            // 
            this.chkSelectAll指定标签.AutoSize = true;
            this.chkSelectAll指定标签.Location = new System.Drawing.Point(513, 123);
            this.chkSelectAll指定标签.Name = "chkSelectAll指定标签";
            this.chkSelectAll指定标签.Size = new System.Drawing.Size(48, 16);
            this.chkSelectAll指定标签.TabIndex = 73;
            this.chkSelectAll指定标签.Text = "全选";
            this.chkSelectAll指定标签.UseVisualStyleBackColor = true;
            this.chkSelectAll指定标签.CheckedChanged += new System.EventHandler(this.chkSelectAll指定标签_CheckedChanged);
            // 
            // chk指定标签
            // 
            this.chk指定标签.AutoSize = true;
            this.chk指定标签.Location = new System.Drawing.Point(226, 123);
            this.chk指定标签.Name = "chk指定标签";
            this.chk指定标签.Size = new System.Drawing.Size(72, 16);
            this.chk指定标签.TabIndex = 71;
            this.chk指定标签.Text = "指定标签";
            this.chk指定标签.UseVisualStyleBackColor = true;
            // 
            // chkSelectAll空内容标签
            // 
            this.chkSelectAll空内容标签.AutoSize = true;
            this.chkSelectAll空内容标签.Location = new System.Drawing.Point(513, 89);
            this.chkSelectAll空内容标签.Name = "chkSelectAll空内容标签";
            this.chkSelectAll空内容标签.Size = new System.Drawing.Size(48, 16);
            this.chkSelectAll空内容标签.TabIndex = 70;
            this.chkSelectAll空内容标签.Text = "全选";
            this.chkSelectAll空内容标签.UseVisualStyleBackColor = true;
            this.chkSelectAll空内容标签.CheckedChanged += new System.EventHandler(this.chkSelectAll空内容标签_CheckedChanged);
            // 
            // chkSelectAll标签属性
            // 
            this.chkSelectAll标签属性.AutoSize = true;
            this.chkSelectAll标签属性.Location = new System.Drawing.Point(513, 20);
            this.chkSelectAll标签属性.Name = "chkSelectAll标签属性";
            this.chkSelectAll标签属性.Size = new System.Drawing.Size(48, 16);
            this.chkSelectAll标签属性.TabIndex = 70;
            this.chkSelectAll标签属性.Text = "全选";
            this.chkSelectAll标签属性.UseVisualStyleBackColor = true;
            this.chkSelectAll标签属性.CheckedChanged += new System.EventHandler(this.chkSelectAll标签属性_CheckedChanged);
            // 
            // chk空内容标签
            // 
            this.chk空内容标签.AutoSize = true;
            this.chk空内容标签.Location = new System.Drawing.Point(226, 91);
            this.chk空内容标签.Name = "chk空内容标签";
            this.chk空内容标签.Size = new System.Drawing.Size(84, 16);
            this.chk空内容标签.TabIndex = 63;
            this.chk空内容标签.Text = "空内容标签";
            this.chk空内容标签.UseVisualStyleBackColor = true;
            // 
            // chk标签属性
            // 
            this.chk标签属性.AutoSize = true;
            this.chk标签属性.Location = new System.Drawing.Point(226, 20);
            this.chk标签属性.Name = "chk标签属性";
            this.chk标签属性.Size = new System.Drawing.Size(84, 16);
            this.chk标签属性.TabIndex = 63;
            this.chk标签属性.Text = "标签的属性";
            this.chk标签属性.UseVisualStyleBackColor = true;
            // 
            // cmb标签属性
            // 
            checkBoxProperties3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cmb标签属性.CheckBoxProperties = checkBoxProperties3;
            this.cmb标签属性.DisplayMemberSingleItem = "";
            this.cmb标签属性.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb标签属性.FormattingEnabled = true;
            this.cmb标签属性.Location = new System.Drawing.Point(339, 18);
            this.cmb标签属性.Name = "cmb标签属性";
            this.cmb标签属性.Size = new System.Drawing.Size(168, 20);
            this.cmb标签属性.TabIndex = 69;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(310, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(173, 12);
            this.label1.TabIndex = 63;
            this.label1.Text = "结构性处理前，建议先补齐标签";
            // 
            // chk只操作标签本身
            // 
            this.chk只操作标签本身.AutoSize = true;
            this.chk只操作标签本身.Checked = true;
            this.chk只操作标签本身.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk只操作标签本身.Location = new System.Drawing.Point(310, 32);
            this.chk只操作标签本身.Name = "chk只操作标签本身";
            this.chk只操作标签本身.Size = new System.Drawing.Size(228, 16);
            this.chk只操作标签本身.TabIndex = 71;
            this.chk只操作标签本身.Text = "只操作标签本身，不包括子节点及内容";
            this.chk只操作标签本身.UseVisualStyleBackColor = true;
            // 
            // chk如果节点内容唯一
            // 
            this.chk如果节点内容唯一.AutoSize = true;
            this.chk如果节点内容唯一.ForeColor = System.Drawing.Color.Red;
            this.chk如果节点内容唯一.Location = new System.Drawing.Point(493, 51);
            this.chk如果节点内容唯一.Name = "chk如果节点内容唯一";
            this.chk如果节点内容唯一.Size = new System.Drawing.Size(378, 16);
            this.chk如果节点内容唯一.TabIndex = 72;
            this.chk如果节点内容唯一.Text = "如果节点只有一个，且内容唯一，则可忽略结构性,可全文查找替换";
            this.chk如果节点内容唯一.UseVisualStyleBackColor = true;
            // 
            // UCHTMLStructuralAnalysis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chk如果节点内容唯一);
            this.Controls.Add(this.chk只操作标签本身);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.gb指定要移除的标签);
            this.Controls.Add(this.chk是否包含标签本身);
            this.Controls.Add(this.rdb移除指定标签格式);
            this.Controls.Add(this.txtXpath);
            this.Controls.Add(this.chkOutPutFindNodesForXpath);
            this.Controls.Add(this.chkInputXpath);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.rdbRemoveNode);
            this.Controls.Add(this.rdbSaveNode);
            this.Controls.Add(this.treeView1);
            this.Name = "UCHTMLStructuralAnalysis";
            this.Size = new System.Drawing.Size(880, 253);
            this.Load += new System.EventHandler(this.UCHTMLStructuralAnalysis_Load);
            this.gb指定要移除的标签.ResumeLayout(false);
            this.gb指定要移除的标签.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.RadioButton rdbSaveNode;
        private System.Windows.Forms.RadioButton rdbRemoveNode;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox chkInputXpath;
        private System.Windows.Forms.TextBox txtXpath;
        private System.Windows.Forms.CheckBox chkOutPutFindNodesForXpath;
        private System.Windows.Forms.CheckBox chk是否包含标签本身;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ListView lvForHtml;
        private System.Windows.Forms.RadioButton rdb移除指定标签格式;
        private System.Windows.Forms.GroupBox gb指定要移除的标签;
        private HLH.WinControl.ComBoBoxEx.CheckBoxComboBox cmb标签属性;
        private System.Windows.Forms.CheckBox chk标签属性;
        private System.Windows.Forms.CheckBox chk空内容标签;
        private HLH.WinControl.ComBoBoxEx.CheckBoxComboBox cmb空内容标签;
        private System.Windows.Forms.CheckBox chkSelectAll标签属性;
        private System.Windows.Forms.CheckBox chkSelectAll空内容标签;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chk只操作标签本身;
        private System.Windows.Forms.CheckBox chk如果节点内容唯一;
        private System.Windows.Forms.CheckBox chkSelectAll指定标签;
        private System.Windows.Forms.CheckBox chk指定标签;
        private HLH.WinControl.ComBoBoxEx.CheckBoxComboBox cmb指定标签;
        private System.Windows.Forms.CheckBox chkAll指定标签的属性;
        private System.Windows.Forms.CheckBox chk指定标签属性;
        private HLH.WinControl.ComBoBoxEx.CheckBoxComboBox cmb指定标签的属性;
    }
}
