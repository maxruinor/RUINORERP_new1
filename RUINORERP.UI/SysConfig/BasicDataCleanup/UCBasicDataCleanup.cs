using Krypton.Navigator;
using Krypton.Toolkit;
using RUINORERP.Common;
using RUINORERP.Model;
using RUINORERP.UI.Common;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RUINORERP.UI.SysConfig.BasicDataCleanup
{
    /// <summary>
    /// 基础数据清理UI组件
    /// 用于基础数据的清理操作
    /// </summary>
    [MenuAttrAssemblyInfo("基础数据清理", ModuleMenuDefine.模块定义.系统设置, ModuleMenuDefine.系统设置.系统工具)]
    public partial class UCBasicDataCleanup : UserControl
    {
        private ISqlSugarClient _db;
        private CleanupConfigurationManager _configManager;
        private DataCleanupEngine _cleanupEngine;
        private CleanupConfiguration _currentConfig;
        private Type _selectedEntityType;

        /// <summary>
        /// 实体类型映射字典
        /// </summary>
        public static Dictionary<string, Type> EntityTypeMappings { get; private set; }

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static UCBasicDataCleanup()
        {
            EntityTypeMappings = new Dictionary<string, Type>
            {
                { "供应商表", typeof(tb_CustomerVendor) },
                { "客户表", typeof(tb_CustomerVendor) },
                { "产品类目表", typeof(tb_ProdCategories) },
                { "产品基本信息表", typeof(tb_Prod) },
                { "产品详情信息表", typeof(tb_ProdDetail) },
                { "产品属性表", typeof(tb_ProdProperty) },
                { "产品属性值表", typeof(tb_ProdPropertyValue) },
                { "库位表", typeof(tb_Location) },
                { "货架表", typeof(tb_StorageRack) },
                { "单位表", typeof(tb_Unit) },
                { "产品类型表", typeof(tb_ProductType) },
                { "部门表", typeof(tb_Department) },
            };
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public UCBasicDataCleanup()
        {
            InitializeComponent();
            InitializeData();
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        private void InitializeData()
        {
            _configManager = new CleanupConfigurationManager();
            _currentConfig = new CleanupConfiguration();

            // 初始化数据网格视图
            dgvDataPreview.AutoGenerateColumns = true;
            dgvRules.AutoGenerateColumns = false;

            // 初始化实体类型选择
            InitializeEntityTypes();

            // 加载配置列表
            LoadConfigurationList();

            // 初始化数据库连接
            LoadDbConnection();

            // 绑定事件
            BindEvents();

            // 更新控件状态
            UpdateControlStates();
        }

        /// <summary>
        /// 加载数据库连接
        /// </summary>
        private void LoadDbConnection()
        {
            if (_db == null)
            {
                _db = MainForm.Instance.AppContext.Db;
                _cleanupEngine = new DataCleanupEngine(_db);
                _cleanupEngine.OnLog += CleanupEngine_OnLog;
                _cleanupEngine.OnProgress += CleanupEngine_OnProgress;
            }
        }

        /// <summary>
        /// 绑定事件
        /// </summary>
        private void BindEvents()
        {
            kcmbEntityType.SelectedIndexChanged += KcmbEntityType_SelectedIndexChanged;
            kcmbConfigName.SelectedIndexChanged += KcmbConfigName_SelectedIndexChanged;
            kbtnNewConfig.Click += KbtnNewConfig_Click;
            kbtnEditConfig.Click += KbtnEditConfig_Click;
            kbtnDeleteConfig.Click += KbtnDeleteConfig_Click;
            kbtnSaveConfig.Click += KbtnSaveConfig_Click;
            kbtnAddRule.Click += KbtnAddRule_Click;
            kbtnEditRule.Click += KbtnEditRule_Click;
            kbtnDeleteRule.Click += KbtnDeleteRule_Click;
            kbtnMoveUp.Click += KbtnMoveUp_Click;
            kbtnMoveDown.Click += KbtnMoveDown_Click;
            kbtnPreview.Click += KbtnPreview_Click;
            kbtnTestExecute.Click += KbtnTestExecute_Click;
            kbtnExecute.Click += KbtnExecute_Click;
            kbtnRefresh.Click += KbtnRefresh_Click;
            dgvRules.SelectionChanged += DgvRules_SelectionChanged;
        }

        /// <summary>
        /// 初始化实体类型选择下拉框
        /// </summary>
        private void InitializeEntityTypes()
        {
            try
            {
                kcmbEntityType.Items.Clear();
                kcmbEntityType.Items.Add("请选择");

                foreach (var mapping in EntityTypeMappings)
                {
                    kcmbEntityType.Items.Add(mapping.Key);
                }

                kcmbEntityType.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"初始化实体类型列表失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 加载配置列表
        /// </summary>
        private void LoadConfigurationList()
        {
            try
            {
                string currentSelection = kcmbConfigName.SelectedIndex > 0 ?
                    kcmbConfigName.SelectedItem.ToString() : null;

                kcmbConfigName.Items.Clear();
                kcmbConfigName.Items.Add("请选择");

                var configNames = _configManager.GetAllConfigurationNames();
                foreach (var name in configNames)
                {
                    kcmbConfigName.Items.Add(name);
                }

                // 恢复之前的选择
                if (!string.IsNullOrEmpty(currentSelection))
                {
                    int index = kcmbConfigName.Items.IndexOf(currentSelection);
                    if (index > 0)
                    {
                        kcmbConfigName.SelectedIndex = index;
                    }
                    else
                    {
                        kcmbConfigName.SelectedIndex = 0;
                    }
                }
                else
                {
                    kcmbConfigName.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载配置列表失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 实体类型选择改变事件
        /// </summary>
        private void KcmbEntityType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (kcmbEntityType.SelectedIndex <= 0)
                {
                    _selectedEntityType = null;
                    _currentConfig.TargetEntityType = null;
                    UpdateControlStates();
                    return;
                }

                string selectedText = kcmbEntityType.SelectedItem.ToString();
                if (EntityTypeMappings.ContainsKey(selectedText))
                {
                    _selectedEntityType = EntityTypeMappings[selectedText];
                    _currentConfig.TargetEntityType = _selectedEntityType.Name;
                    _currentConfig.TargetTable = _selectedEntityType.Name;

                    // 加载该实体类型的配置
                    LoadConfigurationsForEntityType(_selectedEntityType.Name);
                }

                UpdateControlStates();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"选择实体类型失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 加载指定实体类型的配置
        /// </summary>
        private void LoadConfigurationsForEntityType(string entityTypeName)
        {
            try
            {
                kcmbConfigName.Items.Clear();
                kcmbConfigName.Items.Add("请选择");

                var configs = _configManager.GetConfigurationsByEntityType(entityTypeName);
                foreach (var config in configs)
                {
                    kcmbConfigName.Items.Add(config.ConfigName);
                }

                kcmbConfigName.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载配置列表失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 配置选择改变事件
        /// </summary>
        private void KcmbConfigName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (kcmbConfigName.SelectedIndex <= 0)
                {
                    _currentConfig = new CleanupConfiguration();
                    if (_selectedEntityType != null)
                    {
                        _currentConfig.TargetEntityType = _selectedEntityType.Name;
                        _currentConfig.TargetTable = _selectedEntityType.Name;
                    }
                    RefreshRulesGrid();
                    UpdateControlStates();
                    return;
                }

                string configName = kcmbConfigName.SelectedItem.ToString();
                var config = _configManager.LoadConfiguration(configName);

                if (config != null)
                {
                    _currentConfig = config;

                    // 更新实体类型选择
                    if (!string.IsNullOrEmpty(config.TargetEntityType))
                    {
                        foreach (var mapping in EntityTypeMappings)
                        {
                            if (mapping.Value.Name == config.TargetEntityType)
                            {
                                int index = kcmbEntityType.Items.IndexOf(mapping.Key);
                                if (index > 0)
                                {
                                    kcmbEntityType.SelectedIndex = index;
                                    _selectedEntityType = mapping.Value;
                                }
                                break;
                            }
                        }
                    }

                    RefreshRulesGrid();
                    UpdateControlStates();

                    MainForm.Instance.ShowStatusText($"已加载配置: {configName}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载配置失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 刷新规则列表
        /// </summary>
        private void RefreshRulesGrid()
        {
            try
            {
                dgvRules.Rows.Clear();

                if (_currentConfig?.CleanupRules == null)
                {
                    return;
                }

                var sortedRules = _currentConfig.GetEnabledRules();
                foreach (var rule in sortedRules)
                {
                    int rowIndex = dgvRules.Rows.Add();
                    var row = dgvRules.Rows[rowIndex];
                    row.Cells["colRuleName"].Value = rule.RuleName;
                    row.Cells["colRuleType"].Value = GetRuleTypeDisplayName(rule.RuleType);
                    row.Cells["colActionType"].Value = GetActionTypeDisplayName(rule.ActionType);
                    row.Cells["colMatchCount"].Value = "-";
                    row.Cells["colStatus"].Value = rule.IsEnabled ? "启用" : "禁用";
                    row.Tag = rule;
                }

                klblRuleCount.Text = $"规则数量: {sortedRules.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"刷新规则列表失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 获取规则类型显示名称
        /// </summary>
        private string GetRuleTypeDisplayName(CleanupRuleType ruleType)
        {
            return CleanupDisplayNames.GetRuleTypeDisplayName(ruleType);
        }

        /// <summary>
        /// 获取操作类型显示名称
        /// </summary>
        private string GetActionTypeDisplayName(CleanupActionType actionType)
        {
            return CleanupDisplayNames.GetActionTypeDisplayName(actionType);
        }

        /// <summary>
        /// 新建配置按钮点击事件
        /// </summary>
        private void KbtnNewConfig_Click(object sender, EventArgs e)
        {
            try
            {
                if (_selectedEntityType == null)
                {
                    MessageBox.Show("请先选择实体类型", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (var frm = new FrmCleanupConfigEdit())
                {
                    frm.IsNew = true;
                    frm.EntityTypeName = _selectedEntityType.Name;

                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        _currentConfig = frm.Configuration;
                        kcmbConfigName.SelectedIndex = 0;
                        RefreshRulesGrid();
                        UpdateControlStates();

                        MessageBox.Show("配置创建成功，请添加清理规则", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"新建配置失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 编辑配置按钮点击事件
        /// </summary>
        private void KbtnEditConfig_Click(object sender, EventArgs e)
        {
            try
            {
                if (_currentConfig == null || string.IsNullOrEmpty(_currentConfig.ConfigName))
                {
                    MessageBox.Show("请先选择或创建一个配置", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (var frm = new FrmCleanupConfigEdit())
                {
                    frm.IsNew = false;
                    frm.Configuration = _currentConfig;

                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        _currentConfig = frm.Configuration;
                        RefreshRulesGrid();
                        UpdateControlStates();

                        MessageBox.Show("配置修改成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"编辑配置失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 删除配置按钮点击事件
        /// </summary>
        private void KbtnDeleteConfig_Click(object sender, EventArgs e)
        {
            try
            {
                if (_currentConfig == null || string.IsNullOrEmpty(_currentConfig.ConfigName))
                {
                    MessageBox.Show("请先选择要删除的配置", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show($"确定要删除配置 '{_currentConfig.ConfigName}' 吗？\n\n此操作不可恢复！",
                    "确认删除", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    _configManager.DeleteConfiguration(_currentConfig.ConfigName);
                    LoadConfigurationList();

                    _currentConfig = new CleanupConfiguration();
                    if (_selectedEntityType != null)
                    {
                        _currentConfig.TargetEntityType = _selectedEntityType.Name;
                    }

                    RefreshRulesGrid();
                    UpdateControlStates();

                    MessageBox.Show("配置已删除", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"删除配置失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 保存配置按钮点击事件
        /// </summary>
        private void KbtnSaveConfig_Click(object sender, EventArgs e)
        {
            try
            {
                if (_currentConfig == null)
                {
                    MessageBox.Show("当前没有可保存的配置", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string errorMessage;
                if (!_currentConfig.Validate(out errorMessage))
                {
                    MessageBox.Show($"配置验证失败: {errorMessage}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 如果配置名称为空，提示输入
                if (string.IsNullOrWhiteSpace(_currentConfig.ConfigName))
                {
                    MessageBox.Show("请先设置配置名称", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _configManager.SaveConfiguration(_currentConfig);
                LoadConfigurationList();

                // 选中新保存的配置
                int index = kcmbConfigName.Items.IndexOf(_currentConfig.ConfigName);
                if (index > 0)
                {
                    kcmbConfigName.SelectedIndex = index;
                }

                MessageBox.Show("配置保存成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存配置失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 添加规则按钮点击事件
        /// </summary>
        private void KbtnAddRule_Click(object sender, EventArgs e)
        {
            try
            {
                if (_selectedEntityType == null)
                {
                    MessageBox.Show("请先选择实体类型", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (var frm = new FrmCleanupRuleEdit())
                {
                    frm.IsNew = true;
                    frm.EntityType = _selectedEntityType;
                    frm.EntityTypeName = _selectedEntityType.Name;

                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        _currentConfig.AddRule(frm.Rule);
                        RefreshRulesGrid();
                        UpdateControlStates();

                        MessageBox.Show("规则添加成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"添加规则失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 编辑规则按钮点击事件
        /// </summary>
        private void KbtnEditRule_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvRules.SelectedRows.Count == 0)
                {
                    MessageBox.Show("请先选择要编辑的规则", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var rule = dgvRules.SelectedRows[0].Tag as CleanupRule;
                if (rule == null)
                {
                    return;
                }

                using (var frm = new FrmCleanupRuleEdit())
                {
                    frm.IsNew = false;
                    frm.Rule = rule;
                    frm.EntityType = _selectedEntityType;
                    frm.EntityTypeName = _selectedEntityType?.Name;

                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        _currentConfig.UpdateRule(frm.Rule);
                        RefreshRulesGrid();
                        UpdateControlStates();

                        MessageBox.Show("规则修改成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"编辑规则失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 删除规则按钮点击事件
        /// </summary>
        private void KbtnDeleteRule_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvRules.SelectedRows.Count == 0)
                {
                    MessageBox.Show("请先选择要删除的规则", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var rule = dgvRules.SelectedRows[0].Tag as CleanupRule;
                if (rule == null)
                {
                    return;
                }

                if (MessageBox.Show($"确定要删除规则 '{rule.RuleName}' 吗？",
                    "确认删除", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    _currentConfig.RemoveRule(rule.RuleId);
                    RefreshRulesGrid();
                    UpdateControlStates();

                    MessageBox.Show("规则已删除", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"删除规则失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 上移规则按钮点击事件
        /// </summary>
        private void KbtnMoveUp_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvRules.SelectedRows.Count == 0)
                {
                    return;
                }

                var rule = dgvRules.SelectedRows[0].Tag as CleanupRule;
                if (rule == null || rule.ExecutionOrder <= 1)
                {
                    return;
                }

                _currentConfig.ReorderRule(rule.RuleId, rule.ExecutionOrder - 1);
                RefreshRulesGrid();

                // 重新选中该规则
                SelectRuleInGrid(rule.RuleId);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"移动规则失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 下移规则按钮点击事件
        /// </summary>
        private void KbtnMoveDown_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvRules.SelectedRows.Count == 0)
                {
                    return;
                }

                var rule = dgvRules.SelectedRows[0].Tag as CleanupRule;
                if (rule == null)
                {
                    return;
                }

                _currentConfig.ReorderRule(rule.RuleId, rule.ExecutionOrder + 1);
                RefreshRulesGrid();

                // 重新选中该规则
                SelectRuleInGrid(rule.RuleId);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"移动规则失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 在网格中选中指定规则
        /// </summary>
        private void SelectRuleInGrid(string ruleId)
        {
            foreach (DataGridViewRow row in dgvRules.Rows)
            {
                var rule = row.Tag as CleanupRule;
                if (rule != null && rule.RuleId == ruleId)
                {
                    row.Selected = true;
                    break;
                }
            }
        }

        /// <summary>
        /// 预览按钮点击事件
        /// </summary>
        private async void KbtnPreview_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateBeforeExecute())
                {
                    return;
                }

                kbtnPreview.Enabled = false;
                MainForm.Instance.ShowStatusText("正在生成清理预览...");

                var previewResult = await _cleanupEngine.PreviewCleanupAsync(_currentConfig, 100);

                // 显示预览结果
                using (var frm = new FrmCleanupPreview())
                {
                    frm.PreviewResult = previewResult;
                    frm.ShowDialog();
                }

                MainForm.Instance.ShowStatusText("清理预览生成完成");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"生成预览失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                kbtnPreview.Enabled = true;
            }
        }

        /// <summary>
        /// 测试执行按钮点击事件
        /// </summary>
        private async void KbtnTestExecute_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateBeforeExecute())
                {
                    return;
                }

                if (MessageBox.Show("测试模式不会实际修改数据，但会模拟执行所有操作。\n\n是否继续？",
                    "确认测试执行", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return;
                }

                SetExecuteButtonsEnabled(false);
                MainForm.Instance.ShowStatusText("正在测试执行数据清理...");

                var result = await _cleanupEngine.ExecuteCleanupAsync(_currentConfig, true);

                // 显示执行结果
                using (var frm = new FrmCleanupResult())
                {
                    frm.ExecutionResult = result;
                    frm.ShowDialog();
                }

                MainForm.Instance.ShowStatusText($"测试执行完成: 匹配 {result.MatchedRecordCount} 条记录");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"测试执行失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetExecuteButtonsEnabled(true);
            }
        }

        /// <summary>
        /// 正式执行按钮点击事件
        /// </summary>
        private async void KbtnExecute_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateBeforeExecute())
                {
                    return;
                }

                // 确认对话框
                StringBuilder confirmMsg = new StringBuilder();
                confirmMsg.AppendLine("即将执行数据清理操作，请确认以下信息：");
                confirmMsg.AppendLine();
                confirmMsg.AppendLine($"配置名称: {_currentConfig.ConfigName}");
                confirmMsg.AppendLine($"目标表: {_currentConfig.TargetTable}");
                confirmMsg.AppendLine($"启用规则数: {_currentConfig.GetEnabledRules().Count}");
                confirmMsg.AppendLine($"数据备份: {(_currentConfig.EnableBackup ? "是" : "否")}");
                confirmMsg.AppendLine();
                confirmMsg.AppendLine("警告：此操作将实际修改数据库数据！");
                confirmMsg.AppendLine();
                confirmMsg.AppendLine("是否继续执行？");

                if (MessageBox.Show(confirmMsg.ToString(), "确认执行", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                {
                    return;
                }

                // 二次确认
                if (MessageBox.Show("请再次确认：是否立即执行数据清理？\n\n此操作不可撤销！",
                    "最终确认", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
                {
                    return;
                }

                SetExecuteButtonsEnabled(false);
                MainForm.Instance.ShowStatusText("正在执行数据清理...");

                var result = await _cleanupEngine.ExecuteCleanupAsync(_currentConfig, false);

                // 显示执行结果
                using (var frm = new FrmCleanupResult())
                {
                    frm.ExecutionResult = result;
                    frm.ShowDialog();
                }

                MainForm.Instance.ShowStatusText($"数据清理执行完成: 成功 {result.SuccessCount}, 失败 {result.FailedCount}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"执行失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetExecuteButtonsEnabled(true);
            }
        }

        /// <summary>
        /// 刷新按钮点击事件
        /// </summary>
        private void KbtnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                LoadConfigurationList();
                RefreshRulesGrid();
                MainForm.Instance.ShowStatusText("数据已刷新");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"刷新失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 规则选择改变事件
        /// </summary>
        private void DgvRules_SelectionChanged(object sender, EventArgs e)
        {
            UpdateRuleButtonStates();
        }

        /// <summary>
        /// 清理引擎日志事件
        /// </summary>
        private void CleanupEngine_OnLog(object sender, string e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => CleanupEngine_OnLog(sender, e)));
                return;
            }

            // 可以在这里添加日志显示逻辑
            System.Diagnostics.Debug.WriteLine(e);
        }

        /// <summary>
        /// 清理引擎进度事件
        /// </summary>
        private void CleanupEngine_OnProgress(object sender, CleanupProgressEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => CleanupEngine_OnProgress(sender, e)));
                return;
            }

            MainForm.Instance.ShowStatusText($"{e.Message} ({e.Percentage}%)");
        }

        /// <summary>
        /// 执行前验证
        /// </summary>
        private bool ValidateBeforeExecute()
        {
            if (_selectedEntityType == null)
            {
                MessageBox.Show("请先选择实体类型", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (_currentConfig == null || _currentConfig.CleanupRules == null || _currentConfig.CleanupRules.Count == 0)
            {
                MessageBox.Show("请至少添加一条清理规则", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            var enabledRules = _currentConfig.GetEnabledRules();
            if (enabledRules.Count == 0)
            {
                MessageBox.Show("请至少启用一条清理规则", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            string errorMessage;
            if (!_currentConfig.Validate(out errorMessage))
            {
                MessageBox.Show($"配置验证失败: {errorMessage}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 更新控件状态
        /// </summary>
        private void UpdateControlStates()
        {
            bool hasEntityType = _selectedEntityType != null;
            bool hasConfig = _currentConfig != null && !string.IsNullOrEmpty(_currentConfig.ConfigName);
            bool hasRules = _currentConfig?.CleanupRules?.Count > 0;

            // 配置操作按钮
            kbtnNewConfig.Enabled = hasEntityType;
            kbtnEditConfig.Enabled = hasConfig;
            kbtnDeleteConfig.Enabled = hasConfig;
            kbtnSaveConfig.Enabled = hasEntityType;

            // 规则操作按钮
            kbtnAddRule.Enabled = hasEntityType;
            UpdateRuleButtonStates();

            // 执行按钮
            kbtnPreview.Enabled = hasEntityType && hasRules;
            kbtnTestExecute.Enabled = hasEntityType && hasRules;
            kbtnExecute.Enabled = hasEntityType && hasRules;
        }

        /// <summary>
        /// 更新规则按钮状态
        /// </summary>
        private void UpdateRuleButtonStates()
        {
            bool hasSelectedRule = dgvRules.SelectedRows.Count > 0;

            kbtnEditRule.Enabled = hasSelectedRule;
            kbtnDeleteRule.Enabled = hasSelectedRule;
            kbtnMoveUp.Enabled = hasSelectedRule;
            kbtnMoveDown.Enabled = hasSelectedRule;
        }

        /// <summary>
        /// 设置执行按钮启用状态
        /// </summary>
        private void SetExecuteButtonsEnabled(bool enabled)
        {
            kbtnPreview.Enabled = enabled;
            kbtnTestExecute.Enabled = enabled;
            kbtnExecute.Enabled = enabled;
            kbtnNewConfig.Enabled = enabled;
            kbtnEditConfig.Enabled = enabled;
            kbtnDeleteConfig.Enabled = enabled;
            kbtnSaveConfig.Enabled = enabled;
            kbtnAddRule.Enabled = enabled;
            kbtnEditRule.Enabled = enabled && dgvRules.SelectedRows.Count > 0;
            kbtnDeleteRule.Enabled = enabled && dgvRules.SelectedRows.Count > 0;
        }
    }
}
