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
            this.SuspendLayout();

            // Form properties
            this.Text = "流程导航节点属性";
            this.Size = new Size(500, 400);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Create controls
            CreateBasicInfoGroup();
            CreateBusinessInfoGroup();
            CreateButtons();

            // Layout
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void CreateBasicInfoGroup()
        {
            grpBasicInfo = new GroupBox
            {
                Text = "基本信息",
                Location = new Point(12, 12),
                Size = new Size(460, 120)
            };

            // Node Name
            lblNodeName = new Label
            {
                Text = "节点名称:",
                Location = new Point(10, 25),
                Size = new Size(80, 23)
            };
            txtNodeName = new TextBox
            {
                Location = new Point(100, 22),
                Size = new Size(340, 23)
            };

            // Description
            lblDescription = new Label
            {
                Text = "描述:",
                Location = new Point(10, 55),
                Size = new Size(80, 23)
            };
            txtDescription = new TextBox
            {
                Location = new Point(100, 52),
                Size = new Size(340, 23)
            };

            // Business Type
            lblBusinessType = new Label
            {
                Text = "业务类型:",
                Location = new Point(10, 85),
                Size = new Size(80, 23)
            };
            cmbBusinessType = new ComboBox
            {
                Location = new Point(100, 82),
                Size = new Size(200, 23),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            // Add controls to group
            grpBasicInfo.Controls.AddRange(new Control[] {
                lblNodeName, txtNodeName,
                lblDescription, txtDescription,
                lblBusinessType, cmbBusinessType
            });

            this.Controls.Add(grpBasicInfo);
        }

        private void CreateBusinessInfoGroup()
        {
            grpBusinessInfo = new GroupBox
            {
                Text = "业务关联",
                Location = new Point(12, 140),
                Size = new Size(460, 180)
            };

            // Menu ID
            lblMenuID = new Label
            {
                Text = "菜单ID:",
                Location = new Point(10, 25),
                Size = new Size(80, 23)
            };
            txtMenuID = new TextBox
            {
                Location = new Point(100, 22),
                Size = new Size(150, 23),
                ReadOnly = true
            };
            btnSelectMenu = new Button
            {
                Text = "选择菜单",
                Location = new Point(260, 21),
                Size = new Size(80, 25)
            };
            btnSelectMenu.Click += BtnSelectMenu_Click;

            // Module
            lblModule = new Label
            {
                Text = "所属模块:",
                Location = new Point(10, 55),
                Size = new Size(80, 23)
            };
            cmbModule = new ComboBox
            {
                Location = new Point(100, 52),
                Size = new Size(200, 23),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            // Form Name
            lblFormName = new Label
            {
                Text = "窗体名称:",
                Location = new Point(10, 85),
                Size = new Size(80, 23)
            };
            txtFormName = new TextBox
            {
                Location = new Point(100, 82),
                Size = new Size(240, 23)
            };

            // Class Path
            lblClassPath = new Label
            {
                Text = "类路径:",
                Location = new Point(10, 115),
                Size = new Size(80, 23)
            };
            txtClassPath = new TextBox
            {
                Location = new Point(100, 112),
                Size = new Size(340, 23)
            };

            // Add controls to group
            grpBusinessInfo.Controls.AddRange(new Control[] {
                lblMenuID, txtMenuID, btnSelectMenu,
                lblModule, cmbModule,
                lblFormName, txtFormName,
                lblClassPath, txtClassPath
            });

            this.Controls.Add(grpBusinessInfo);
        }

        private void CreateButtons()
        {
            btnOK = new Button
            {
                Text = "确定",
                Location = new Point(325, 330),
                Size = new Size(70, 30),
                DialogResult = DialogResult.OK
            };
            btnOK.Click += BtnOK_Click;

            btnCancel = new Button
            {
                Text = "取消",
                Location = new Point(405, 330),
                Size = new Size(70, 30),
                DialogResult = DialogResult.Cancel
            };

            this.Controls.AddRange(new Control[] { btnOK, btnCancel });
            this.AcceptButton = btnOK;
            this.CancelButton = btnCancel;
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
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载数据失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                    _node.BusinessType = (int)businessType;
                }

                // Set module if selected
                if (cmbModule.SelectedItem is ModuleItem selectedModule && selectedModule.Value != null)
                {
                    _node.ModuleID = selectedModule.Value;
                }
            }
        }

        #endregion
    }
}