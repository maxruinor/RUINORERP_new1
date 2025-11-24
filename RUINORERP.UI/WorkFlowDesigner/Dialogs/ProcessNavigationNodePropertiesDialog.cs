using log4net;
using Microsoft.Extensions.Logging;
using RUINORERP.Common.Log4Net;
using RUINORERP.IServices;
using RUINORERP.Model;
using RUINORERP.SecurityTool;
using RUINORERP.Services;
using RUINORERP.UI.WorkFlowDesigner;
using RUINORERP.UI.WorkFlowDesigner.Nodes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.WorkFlowDesigner.Dialogs
{
    /// <summary>
    /// 流程导航节点属性对话框
    /// </summary>
    public partial class ProcessNavigationNodePropertiesDialog : Form
    {
        #region Fields

        private ProcessNavigationNode _node;
        private Itb_MenuInfoServices _menuInfoService;
        private Itb_ModuleDefinitionServices _moduleService;

        // 控件
        private TextBox txtNodeName;
        private TextBox txtDescription;
        private TextBox txtMenuID;
        private TextBox txtFormName;
        private TextBox txtClassPath;
        private ComboBox cmbBusinessType;
        private ComboBox cmbModule;
        private Button btnSelectMenu;
        private Button btnOK;
        private Button btnCancel;
        private Label lblNodeName;
        private Label lblDescription;
        private Label lblBusinessType;
        private Label lblMenuID;
        private Label lblModule;
        private Label lblFormName;
        private Label lblClassPath;
        private GroupBox grpBasicInfo;
        private GroupBox grpBusinessInfo;
        // 视觉属性相关控件
        private GroupBox grpVisualProperties;
        private CheckBox chkShowCustomText;
        private TextBox txtCustomText;
        private Label label10;
        private ColorDialog colorDialog1;
        private Button btnFontColor;
        private Label label11;
        private Button btnSelectFont;
        private Label label12;
        private ComboBox cboTextAlignment;
        private Label label13;
        private CheckBox chkTextWrap;
        private NumericUpDown numOpacity;
        private Label label14;
        private Button btnSelectBackgroundImage;
        private TextBox txtBackgroundImagePath;
        private Label label15;
        private OpenFileDialog openFileDialog1;

        #endregion

        #region Helper Classes

        /// <summary>
        /// 模块选择项
        /// </summary>
        private class ModuleItem
        {
            public string Text { get; set; }
            public long Value { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }

        #endregion

        #region Constructor

        public ProcessNavigationNodePropertiesDialog(ProcessNavigationNode node,
            Itb_MenuInfoServices menuInfoService = null,
            Itb_ModuleDefinitionServices moduleService = null
            )
        {
            _node = node;
            _menuInfoService = menuInfoService;
            _moduleService = moduleService;
            InitializeComponent();
            LoadData();
        }

        #endregion

        #region Initialization

        private void InitializeComponent()
        {
            // 设计器友好的初始化方式 - 所有控件直接在InitializeComponent中定义
            // 避免调用外部方法，确保设计器序列化能正确识别所有控件
            this.grpBasicInfo = new System.Windows.Forms.GroupBox();
            this.cmbBusinessType = new System.Windows.Forms.ComboBox();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.txtNodeName = new System.Windows.Forms.TextBox();
            this.lblBusinessType = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblNodeName = new System.Windows.Forms.Label();
            this.grpBusinessInfo = new System.Windows.Forms.GroupBox();
            this.txtClassPath = new System.Windows.Forms.TextBox();
            this.txtFormName = new System.Windows.Forms.TextBox();
            this.cmbModule = new System.Windows.Forms.ComboBox();
            this.btnSelectMenu = new System.Windows.Forms.Button();
            this.txtMenuID = new System.Windows.Forms.TextBox();
            this.lblClassPath = new System.Windows.Forms.Label();
            this.lblFormName = new System.Windows.Forms.Label();
            this.lblModule = new System.Windows.Forms.Label();
            this.lblMenuID = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grpBasicInfo.SuspendLayout();
            this.grpBusinessInfo.SuspendLayout();
            this.SuspendLayout();
            
            // 基本信息组
            this.grpBasicInfo.Controls.Add(this.cmbBusinessType);
            this.grpBasicInfo.Controls.Add(this.txtDescription);
            this.grpBasicInfo.Controls.Add(this.txtNodeName);
            this.grpBasicInfo.Controls.Add(this.lblBusinessType);
            this.grpBasicInfo.Controls.Add(this.lblDescription);
            this.grpBasicInfo.Controls.Add(this.lblNodeName);
            this.grpBasicInfo.Location = new System.Drawing.Point(12, 12);
            this.grpBasicInfo.Name = "grpBasicInfo";
            this.grpBasicInfo.Size = new System.Drawing.Size(460, 120);
            this.grpBasicInfo.TabIndex = 0;
            this.grpBasicInfo.TabStop = false;
            this.grpBasicInfo.Text = "基本信息";
            
            // 节点名称标签
            this.lblNodeName.AutoSize = true;
            this.lblNodeName.Location = new System.Drawing.Point(10, 25);
            this.lblNodeName.Name = "lblNodeName";
            this.lblNodeName.Size = new System.Drawing.Size(80, 23);
            this.lblNodeName.TabIndex = 0;
            this.lblNodeName.Text = "节点名称:";
            
            // 节点名称文本框
            this.txtNodeName.Location = new System.Drawing.Point(100, 22);
            this.txtNodeName.Name = "txtNodeName";
            this.txtNodeName.Size = new System.Drawing.Size(340, 23);
            this.txtNodeName.TabIndex = 1;
            
            // 描述标签
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(10, 55);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(80, 23);
            this.lblDescription.TabIndex = 2;
            this.lblDescription.Text = "描述:";
            
            // 描述文本框
            this.txtDescription.Location = new System.Drawing.Point(100, 52);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(340, 23);
            this.txtDescription.TabIndex = 3;
            
            // 业务类型标签
            this.lblBusinessType.AutoSize = true;
            this.lblBusinessType.Location = new System.Drawing.Point(10, 85);
            this.lblBusinessType.Name = "lblBusinessType";
            this.lblBusinessType.Size = new System.Drawing.Size(80, 23);
            this.lblBusinessType.TabIndex = 4;
            this.lblBusinessType.Text = "业务类型:";
            
            // 业务类型下拉框
            this.cmbBusinessType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBusinessType.Location = new System.Drawing.Point(100, 82);
            this.cmbBusinessType.Name = "cmbBusinessType";
            this.cmbBusinessType.Size = new System.Drawing.Size(200, 23);
            this.cmbBusinessType.TabIndex = 5;
            
            // 业务关联组
            this.grpBusinessInfo.Controls.Add(this.txtClassPath);
            this.grpBusinessInfo.Controls.Add(this.txtFormName);
            this.grpBusinessInfo.Controls.Add(this.cmbModule);
            this.grpBusinessInfo.Controls.Add(this.btnSelectMenu);
            this.grpBusinessInfo.Controls.Add(this.txtMenuID);
            this.grpBusinessInfo.Controls.Add(this.lblClassPath);
            this.grpBusinessInfo.Controls.Add(this.lblFormName);
            this.grpBusinessInfo.Controls.Add(this.lblModule);
            this.grpBusinessInfo.Controls.Add(this.lblMenuID);
            this.grpBusinessInfo.Location = new System.Drawing.Point(12, 140);
            this.grpBusinessInfo.Name = "grpBusinessInfo";
            this.grpBusinessInfo.Size = new System.Drawing.Size(460, 180);
            this.grpBusinessInfo.TabIndex = 1;
            this.grpBusinessInfo.TabStop = false;
            this.grpBusinessInfo.Text = "业务关联";
            
            // 菜单ID标签
            this.lblMenuID.AutoSize = true;
            this.lblMenuID.Location = new System.Drawing.Point(10, 25);
            this.lblMenuID.Name = "lblMenuID";
            this.lblMenuID.Size = new System.Drawing.Size(80, 23);
            this.lblMenuID.TabIndex = 0;
            this.lblMenuID.Text = "菜单ID:";
            
            // 菜单ID文本框
            this.txtMenuID.Location = new System.Drawing.Point(100, 22);
            this.txtMenuID.Name = "txtMenuID";
            this.txtMenuID.ReadOnly = true;
            this.txtMenuID.Size = new System.Drawing.Size(150, 23);
            this.txtMenuID.TabIndex = 1;
            
            // 选择菜单按钮
            this.btnSelectMenu.Location = new System.Drawing.Point(260, 21);
            this.btnSelectMenu.Name = "btnSelectMenu";
            this.btnSelectMenu.Size = new System.Drawing.Size(80, 25);
            this.btnSelectMenu.TabIndex = 2;
            this.btnSelectMenu.Text = "选择菜单";
            this.btnSelectMenu.UseVisualStyleBackColor = true;
            this.btnSelectMenu.Click += new System.EventHandler(this.BtnSelectMenu_Click);
            
            // 所属模块标签
            this.lblModule.AutoSize = true;
            this.lblModule.Location = new System.Drawing.Point(10, 55);
            this.lblModule.Name = "lblModule";
            this.lblModule.Size = new System.Drawing.Size(80, 23);
            this.lblModule.TabIndex = 3;
            this.lblModule.Text = "所属模块:";
            
            // 所属模块下拉框
            this.cmbModule.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbModule.Location = new System.Drawing.Point(100, 52);
            this.cmbModule.Name = "cmbModule";
            this.cmbModule.Size = new System.Drawing.Size(200, 23);
            this.cmbModule.TabIndex = 4;
            
            // 窗体名称标签
            this.lblFormName.AutoSize = true;
            this.lblFormName.Location = new System.Drawing.Point(10, 85);
            this.lblFormName.Name = "lblFormName";
            this.lblFormName.Size = new System.Drawing.Size(80, 23);
            this.lblFormName.TabIndex = 5;
            this.lblFormName.Text = "窗体名称:";
            
            // 窗体名称文本框
            this.txtFormName.Location = new System.Drawing.Point(100, 82);
            this.txtFormName.Name = "txtFormName";
            this.txtFormName.Size = new System.Drawing.Size(240, 23);
            this.txtFormName.TabIndex = 6;
            
            // 类路径标签
            this.lblClassPath.AutoSize = true;
            this.lblClassPath.Location = new System.Drawing.Point(10, 115);
            this.lblClassPath.Name = "lblClassPath";
            this.lblClassPath.Size = new System.Drawing.Size(80, 23);
            this.lblClassPath.TabIndex = 7;
            this.lblClassPath.Text = "类路径:";
            
            // 类路径文本框
            this.txtClassPath.Location = new System.Drawing.Point(100, 112);
            this.txtClassPath.Name = "txtClassPath";
            this.txtClassPath.Size = new System.Drawing.Size(340, 23);
            this.txtClassPath.TabIndex = 8;
            
            // 确定按钮
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(325, 330);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(70, 30);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
            
            // 取消按钮
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(405, 330);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(70, 30);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            
            // 视觉属性组
            this.grpVisualProperties = new System.Windows.Forms.GroupBox();
            this.chkShowCustomText = new System.Windows.Forms.CheckBox();
            this.txtCustomText = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.btnFontColor = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.btnSelectFont = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.cboTextAlignment = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.chkTextWrap = new System.Windows.Forms.CheckBox();
            this.numOpacity = new System.Windows.Forms.NumericUpDown();
            this.label14 = new System.Windows.Forms.Label();
            this.btnSelectBackgroundImage = new System.Windows.Forms.Button();
            this.txtBackgroundImagePath = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            
            // 视觉属性组设置
            this.grpVisualProperties.Controls.Add(this.txtBackgroundImagePath);
            this.grpVisualProperties.Controls.Add(this.label15);
            this.grpVisualProperties.Controls.Add(this.btnSelectBackgroundImage);
            this.grpVisualProperties.Controls.Add(this.numOpacity);
            this.grpVisualProperties.Controls.Add(this.label14);
            this.grpVisualProperties.Controls.Add(this.chkTextWrap);
            this.grpVisualProperties.Controls.Add(this.cboTextAlignment);
            this.grpVisualProperties.Controls.Add(this.label13);
            this.grpVisualProperties.Controls.Add(this.btnSelectFont);
            this.grpVisualProperties.Controls.Add(this.label12);
            this.grpVisualProperties.Controls.Add(this.btnFontColor);
            this.grpVisualProperties.Controls.Add(this.label11);
            this.grpVisualProperties.Controls.Add(this.txtCustomText);
            this.grpVisualProperties.Controls.Add(this.label10);
            this.grpVisualProperties.Controls.Add(this.chkShowCustomText);
            this.grpVisualProperties.Location = new System.Drawing.Point(12, 330);
            this.grpVisualProperties.Name = "grpVisualProperties";
            this.grpVisualProperties.Size = new System.Drawing.Size(460, 190);
            this.grpVisualProperties.TabIndex = 4;
            this.grpVisualProperties.TabStop = false;
            this.grpVisualProperties.Text = "视觉属性";
            
            // 显示自定义文本复选框
            this.chkShowCustomText.AutoSize = true;
            this.chkShowCustomText.Location = new System.Drawing.Point(10, 25);
            this.chkShowCustomText.Name = "chkShowCustomText";
            this.chkShowCustomText.Size = new System.Drawing.Size(120, 23);
            this.chkShowCustomText.TabIndex = 0;
            this.chkShowCustomText.Text = "显示自定义文本";
            this.chkShowCustomText.UseVisualStyleBackColor = true;
            this.chkShowCustomText.CheckedChanged += new System.EventHandler(this.chkShowCustomText_CheckedChanged);
            
            // 自定义文本标签
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(10, 55);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(80, 23);
            this.label10.TabIndex = 1;
            this.label10.Text = "自定义文本:";
            
            // 自定义文本文本框
            this.txtCustomText.Location = new System.Drawing.Point(100, 52);
            this.txtCustomText.Multiline = true;
            this.txtCustomText.Name = "txtCustomText";
            this.txtCustomText.Size = new System.Drawing.Size(340, 50);
            this.txtCustomText.TabIndex = 2;
            
            // 文字颜色标签
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(10, 110);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(80, 23);
            this.label11.TabIndex = 3;
            this.label11.Text = "文字颜色:";
            
            // 文字颜色按钮
            this.btnFontColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFontColor.Location = new System.Drawing.Point(100, 107);
            this.btnFontColor.Name = "btnFontColor";
            this.btnFontColor.Size = new System.Drawing.Size(50, 25);
            this.btnFontColor.TabIndex = 4;
            this.btnFontColor.UseVisualStyleBackColor = true;
            this.btnFontColor.Click += new System.EventHandler(this.btnFontColor_Click);
            
            // 字体设置标签
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(160, 110);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(80, 23);
            this.label12.TabIndex = 5;
            this.label12.Text = "字体设置:";
            
            // 选择字体按钮
            this.btnSelectFont.Location = new System.Drawing.Point(250, 107);
            this.btnSelectFont.Name = "btnSelectFont";
            this.btnSelectFont.Size = new System.Drawing.Size(80, 25);
            this.btnSelectFont.TabIndex = 6;
            this.btnSelectFont.Text = "选择字体";
            this.btnSelectFont.UseVisualStyleBackColor = true;
            this.btnSelectFont.Click += new System.EventHandler(this.btnSelectFont_Click);
            
            // 文字对齐标签
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(10, 140);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(80, 23);
            this.label13.TabIndex = 7;
            this.label13.Text = "文字对齐:";
            
            // 文字对齐下拉框
            this.cboTextAlignment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTextAlignment.FormattingEnabled = true;
            this.cboTextAlignment.Items.AddRange(new object[] {
            "居中",
            "左对齐",
            "右对齐",
            "顶部居中",
            "顶部左对齐",
            "顶部右对齐",
            "底部居中",
            "底部左对齐",
            "底部右对齐"});
            this.cboTextAlignment.Location = new System.Drawing.Point(100, 137);
            this.cboTextAlignment.Name = "cboTextAlignment";
            this.cboTextAlignment.Size = new System.Drawing.Size(120, 23);
            this.cboTextAlignment.TabIndex = 8;
            
            // 文字换行复选框
            this.chkTextWrap.AutoSize = true;
            this.chkTextWrap.Location = new System.Drawing.Point(230, 142);
            this.chkTextWrap.Name = "chkTextWrap";
            this.chkTextWrap.Size = new System.Drawing.Size(80, 23);
            this.chkTextWrap.TabIndex = 9;
            this.chkTextWrap.Text = "文字换行";
            this.chkTextWrap.UseVisualStyleBackColor = true;
            
            // 透明度标签
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(320, 110);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(80, 23);
            this.label14.TabIndex = 10;
            this.label14.Text = "透明度:";
            
            // 透明度数值框
            this.numOpacity.DecimalPlaces = 1;
            this.numOpacity.Increment = new decimal(new int[] {1, 0, 0, 65536});
            this.numOpacity.Location = new System.Drawing.Point(400, 108);
            this.numOpacity.Name = "numOpacity";
            this.numOpacity.Size = new System.Drawing.Size(40, 23);
            this.numOpacity.TabIndex = 11;
            this.numOpacity.Value = new decimal(new int[] {1, 0, 0, 0});
            this.numOpacity.Minimum = new decimal(new int[] {1, 0, 0, 65536});
            this.numOpacity.Maximum = new decimal(new int[] {1, 0, 0, 0});
            
            // 背景图像路径标签
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(10, 170);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(100, 23);
            this.label15.TabIndex = 12;
            this.label15.Text = "背景图像路径:";
            
            // 背景图像路径文本框
            this.txtBackgroundImagePath.Location = new System.Drawing.Point(120, 167);
            this.txtBackgroundImagePath.Name = "txtBackgroundImagePath";
            this.txtBackgroundImagePath.Size = new System.Drawing.Size(220, 23);
            this.txtBackgroundImagePath.TabIndex = 13;
            
            // 选择背景图像按钮
            this.btnSelectBackgroundImage.Location = new System.Drawing.Point(350, 166);
            this.btnSelectBackgroundImage.Name = "btnSelectBackgroundImage";
            this.btnSelectBackgroundImage.Size = new System.Drawing.Size(90, 25);
            this.btnSelectBackgroundImage.TabIndex = 14;
            this.btnSelectBackgroundImage.Text = "选择图像";
            this.btnSelectBackgroundImage.UseVisualStyleBackColor = true;
            this.btnSelectBackgroundImage.Click += new System.EventHandler(this.btnSelectBackgroundImage_Click);
            
            // 颜色对话框设置
            this.colorDialog1.AnyColor = true;
            this.colorDialog1.FullOpen = true;
            
            // 文件对话框设置
            this.openFileDialog1.Filter = "图像文件|*.jpg;*.jpeg;*.png;*.bmp;*.gif|所有文件|*.*";
            this.openFileDialog1.Title = "选择背景图像";
            
            // 表单属性
            this.AcceptButton = this.btnOK;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(484, 535);
            this.Controls.Add(this.grpVisualProperties);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.grpBusinessInfo);
            this.Controls.Add(this.grpBasicInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProcessNavigationNodePropertiesDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "流程导航节点属性";
            this.grpBasicInfo.ResumeLayout(false);
            this.grpBasicInfo.PerformLayout();
            this.grpBusinessInfo.ResumeLayout(false);
            this.grpBusinessInfo.PerformLayout();
            this.grpVisualProperties.ResumeLayout(false);
            this.grpVisualProperties.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numOpacity)).EndInit();
            this.ResumeLayout(false);
        }

        // 保留这些方法以确保向后兼容性
        // 但所有控件初始化已移至InitializeComponent方法中
        public void CreateBasicInfoGroup()
        {
            // 此方法已替换为InitializeComponent中的直接控件初始化
            // 保留以确保设计器序列化兼容性
        }

        public void CreateBusinessInfoGroup()
        {
            // 此方法已替换为InitializeComponent中的直接控件初始化
            // 保留以确保设计器序列化兼容性
        }

        public void CreateButtons()
        {
            // 此方法已替换为InitializeComponent中的直接控件初始化
            // 保留以确保设计器序列化兼容性
        }

        #endregion

        #region Data Loading

        private void LoadData()
        {
            try
            {
                // Load business types
                cmbBusinessType.Items.AddRange(Enum.GetNames(typeof(ProcessNavigationNodeBusinessType)));

                // Load modules if service is available
                if (_moduleService != null)
                {
                    LoadModules();
                }

                // Set current values
                if (_node != null)
                {
                    txtNodeName.Text = _node.ProcessName ?? "";
                    txtDescription.Text = _node.Description ?? "";
                    txtMenuID.Text = _node.MenuID ?? "";
                    txtFormName.Text = _node.FormName ?? "";
                    txtClassPath.Text = _node.ClassPath ?? "";

                    if (Enum.TryParse<ProcessNavigationNodeBusinessType>(_node.BusinessType.ToString(), out var businessType))
                    {
                        cmbBusinessType.SelectedItem = businessType.ToString();
                    }
                    
                    // 加载视觉属性
                    chkShowCustomText.Checked = _node.GetType().GetProperty("ShowCustomText")?.GetValue(_node, null) as bool? ?? false;
                    txtCustomText.Text = _node.GetType().GetProperty("CustomText")?.GetValue(_node, null) as string ?? "";
                    btnFontColor.BackColor = _node.GetType().GetProperty("FontColor")?.GetValue(_node, null) as Color? ?? Color.Black;
                    
                    // 设置文本对齐方式
                    ContentAlignment alignment = _node.GetType().GetProperty("TextAlignment")?.GetValue(_node, null) as ContentAlignment? ?? ContentAlignment.MiddleCenter;
                    switch (alignment)
                    {
                        case ContentAlignment.MiddleCenter:
                            cboTextAlignment.SelectedIndex = 0;
                            break;
                        case ContentAlignment.MiddleLeft:
                            cboTextAlignment.SelectedIndex = 1;
                            break;
                        case ContentAlignment.MiddleRight:
                            cboTextAlignment.SelectedIndex = 2;
                            break;
                        case ContentAlignment.TopCenter:
                            cboTextAlignment.SelectedIndex = 3;
                            break;
                        case ContentAlignment.TopLeft:
                            cboTextAlignment.SelectedIndex = 4;
                            break;
                        case ContentAlignment.TopRight:
                            cboTextAlignment.SelectedIndex = 5;
                            break;
                        case ContentAlignment.BottomCenter:
                            cboTextAlignment.SelectedIndex = 6;
                            break;
                        case ContentAlignment.BottomLeft:
                            cboTextAlignment.SelectedIndex = 7;
                            break;
                        case ContentAlignment.BottomRight:
                            cboTextAlignment.SelectedIndex = 8;
                            break;
                        default:
                            cboTextAlignment.SelectedIndex = 0;
                            break;
                    }
                    
                    chkTextWrap.Checked = _node.GetType().GetProperty("TextWrap")?.GetValue(_node, null) as bool? ?? false;
                    // 修复float类型不能直接使用null合并运算符的问题
                    object opacityValue = _node.GetType().GetProperty("Opacity")?.GetValue(_node, null);
                    numOpacity.Value = opacityValue != null ? Convert.ToDecimal(opacityValue) : Convert.ToDecimal(1.0f);
                    txtBackgroundImagePath.Text = _node.GetType().GetProperty("BackgroundImagePath")?.GetValue(_node, null) as string ?? "";
                }
                
                // 设置默认选择
                if (cboTextAlignment.SelectedIndex == -1)
                {
                    cboTextAlignment.SelectedIndex = 0;
                }
                
                // 根据是否显示自定义文本决定是否启用相关控件
                UpdateCustomTextControls();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载数据失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        /// <summary>
        /// 更新自定义文本相关控件的启用状态
        /// </summary>
        private void UpdateCustomTextControls()
        {
            txtCustomText.Enabled = chkShowCustomText.Checked;
        }
        
        private void chkShowCustomText_CheckedChanged(object sender, EventArgs e)
        {
            UpdateCustomTextControls();
        }

        private async void LoadModules()
        {
            try
            {
                if (_moduleService != null)
                {
                    var modules = await _moduleService.QueryAsync();
                    if (modules != null && modules.Any())
                    {
                        cmbModule.Items.Clear();
                        foreach (var module in modules)
                        {
                            cmbModule.Items.Add(new ModuleItem { Text = module.ModuleName, Value = module.ModuleID });
                        }
                        cmbModule.DisplayMember = "Text";
                        cmbModule.ValueMember = "Value";
                    }
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.LogError(ex, "加载模块列表失败");
                
            }
        }

        #endregion

        #region Event Handlers

        private void BtnSelectMenu_Click(object sender, EventArgs e)
        {
            try
            {
                // TODO: 实现菜单选择对话框
                MessageBox.Show("菜单选择功能待实现", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.LogError(ex, "选择菜单失败");
                MessageBox.Show($"选择菜单失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateInput())
                {
                    SaveData();
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.LogError(ex, "保存节点属性失败");
                MessageBox.Show($"保存失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Validation

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtNodeName.Text))
            {
                MessageBox.Show("请输入节点名称", "验证错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNodeName.Focus();
                return false;
            }

            if (cmbBusinessType.SelectedItem == null)
            {
                MessageBox.Show("请选择业务类型", "验证错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbBusinessType.Focus();
                return false;
            }

            return true;
        }

        #endregion

        #region Data Saving

        private void SaveData()
        {
            if (_node != null)
            {
                _node.ProcessName = txtNodeName.Text.Trim();
                _node.Description = txtDescription.Text.Trim();
                _node.MenuID = txtMenuID.Text.Trim();
                _node.FormName = txtFormName.Text.Trim();
                _node.ClassPath = txtClassPath.Text.Trim();

                if (Enum.TryParse<ProcessNavigationNodeBusinessType>(cmbBusinessType.SelectedItem.ToString(), out var businessType))
                {
                    _node.BusinessType = (ProcessNavigationNodeBusinessType)businessType;
                }

                // Set module if selected
                if (cmbModule.SelectedItem is ModuleItem selectedModule && selectedModule.Value != null)
                {
                    _node.ModuleID = selectedModule.Value;
                }
                
                // 保存视觉属性
                if (_node.GetType().GetProperty("ShowCustomText") != null)
                    _node.GetType().GetProperty("ShowCustomText").SetValue(_node, chkShowCustomText.Checked);
                
                if (_node.GetType().GetProperty("CustomText") != null)
                    _node.GetType().GetProperty("CustomText").SetValue(_node, txtCustomText.Text);
                
                if (_node.GetType().GetProperty("FontColor") != null)
                    _node.GetType().GetProperty("FontColor").SetValue(_node, btnFontColor.BackColor);
                
                // 设置文本对齐方式
                ContentAlignment alignment = ContentAlignment.MiddleCenter;
                switch (cboTextAlignment.SelectedIndex)
                {
                    case 0:
                        alignment = ContentAlignment.MiddleCenter;
                        break;
                    case 1:
                        alignment = ContentAlignment.MiddleLeft;
                        break;
                    case 2:
                        alignment = ContentAlignment.MiddleRight;
                        break;
                    case 3:
                        alignment = ContentAlignment.TopCenter;
                        break;
                    case 4:
                        alignment = ContentAlignment.TopLeft;
                        break;
                    case 5:
                        alignment = ContentAlignment.TopRight;
                        break;
                    case 6:
                        alignment = ContentAlignment.BottomCenter;
                        break;
                    case 7:
                        alignment = ContentAlignment.BottomLeft;
                        break;
                    case 8:
                        alignment = ContentAlignment.BottomRight;
                        break;
                }
                if (_node.GetType().GetProperty("TextAlignment") != null)
                    _node.GetType().GetProperty("TextAlignment").SetValue(_node, alignment);
                
                if (_node.GetType().GetProperty("TextWrap") != null)
                    _node.GetType().GetProperty("TextWrap").SetValue(_node, chkTextWrap.Checked);
                
                if (_node.GetType().GetProperty("Opacity") != null)
                    _node.GetType().GetProperty("Opacity").SetValue(_node, (float)numOpacity.Value);
                
                if (_node.GetType().GetProperty("BackgroundImagePath") != null)
                    _node.GetType().GetProperty("BackgroundImagePath").SetValue(_node, txtBackgroundImagePath.Text);
            }
        }
        
        private void btnFontColor_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = btnFontColor.BackColor;
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                btnFontColor.BackColor = colorDialog1.Color;
            }
        }
        
        private void btnSelectFont_Click(object sender, EventArgs e)
        {
            FontDialog fontDialog = new FontDialog();
            // 尝试获取当前字体
            Font currentFont = null;
            try
            {
                currentFont = _node.GetType().GetProperty("TextFont")?.GetValue(_node, null) as Font;
            }
            catch { }
            
            if (currentFont != null)
                fontDialog.Font = currentFont;
            
            fontDialog.ShowColor = false;
            
            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                if (_node.GetType().GetProperty("TextFont") != null)
                    _node.GetType().GetProperty("TextFont").SetValue(_node, fontDialog.Font);
            }
        }
        
        private void btnSelectBackgroundImage_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtBackgroundImagePath.Text = openFileDialog1.FileName;
            }
        }

        #endregion
    }
}