using Krypton.Toolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace RUINORERP.UI.SysConfig.BasicDataCleanup
{
    /// <summary>
    /// 清理规则编辑对话框
    /// </summary>
    public partial class FrmCleanupRuleEdit : KryptonForm
    {
        /// <summary>
        /// 是否为新建模式
        /// </summary>
        public bool IsNew { get; set; }

        /// <summary>
        /// 实体类型
        /// </summary>
        public Type EntityType { get; set; }

        /// <summary>
        /// 实体类型名称
        /// </summary>
        public string EntityTypeName { get; set; }

        /// <summary>
        /// 清理规则
        /// </summary>
        public CleanupRule Rule { get; set; }

        /// <summary>
        /// 实体字段列表
        /// </summary>
        private List<PropertyInfo> _entityProperties;

        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmCleanupRuleEdit()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        private void FrmCleanupRuleEdit_Load(object sender, EventArgs e)
        {
            // 初始化规则类型下拉框
            InitializeRuleTypeCombo();

            // 初始化操作类型下拉框
            InitializeActionTypeCombo();

            // 加载实体字段
            LoadEntityFields();

            if (IsNew)
            {
                this.Text = "新建清理规则";
                Rule = new CleanupRule();
                Rule.TargetEntityType = EntityTypeName;

                // 默认选中第一个规则类型
                kcmbRuleType.SelectedIndex = 0;
                kcmbActionType.SelectedIndex = 0;
            }
            else
            {
                this.Text = "编辑清理规则";
                if (Rule == null)
                {
                    MessageBox.Show("规则对象为空", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                    return;
                }
                LoadRule();
            }

            // 更新界面状态
            UpdateUIState();
        }

        /// <summary>
        /// 初始化规则类型下拉框
        /// </summary>
        private void InitializeRuleTypeCombo()
        {
            kcmbRuleType.Items.Clear();
            var ruleTypes = Enum.GetValues(typeof(CleanupRuleType));
            foreach (CleanupRuleType type in ruleTypes)
            {
                string displayName = GetRuleTypeDisplayName(type);
                kcmbRuleType.Items.Add(new ComboBoxItem { Text = displayName, Value = type });
            }
        }

        /// <summary>
        /// 初始化操作类型下拉框
        /// </summary>
        private void InitializeActionTypeCombo()
        {
            kcmbActionType.Items.Clear();
            var actionTypes = Enum.GetValues(typeof(CleanupActionType));
            foreach (CleanupActionType type in actionTypes)
            {
                string displayName = GetActionTypeDisplayName(type);
                kcmbActionType.Items.Add(new ComboBoxItem { Text = displayName, Value = type });
            }
        }

        /// <summary>
        /// 加载实体字段
        /// </summary>
        private void LoadEntityFields()
        {
            _entityProperties = new List<PropertyInfo>();

            if (EntityType == null)
            {
                return;
            }

            try
            {
                _entityProperties = EntityType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.CanRead && p.CanWrite)
                    .OrderBy(p => p.Name)
                    .ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载实体字段失败: {ex.Message}", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 加载规则数据到界面
        /// </summary>
        private void LoadRule()
        {
            ktxtRuleName.Text = Rule.RuleName;
            ktxtDescription.Text = Rule.Description;
            kchkEnabled.Checked = Rule.IsEnabled;

            // 设置规则类型
            for (int i = 0; i < kcmbRuleType.Items.Count; i++)
            {
                var item = kcmbRuleType.Items[i] as ComboBoxItem;
                if (item != null && (CleanupRuleType)item.Value == Rule.RuleType)
                {
                    kcmbRuleType.SelectedIndex = i;
                    break;
                }
            }

            // 设置操作类型
            for (int i = 0; i < kcmbActionType.Items.Count; i++)
            {
                var item = kcmbActionType.Items[i] as ComboBoxItem;
                if (item != null && (CleanupActionType)item.Value == Rule.ActionType)
                {
                    kcmbActionType.SelectedIndex = i;
                    break;
                }
            }

            // 根据规则类型加载特定配置
            LoadRuleTypeSpecificConfig();
        }

        /// <summary>
        /// 根据规则类型加载特定配置
        /// </summary>
        private void LoadRuleTypeSpecificConfig()
        {
            switch (Rule.RuleType)
            {
                case CleanupRuleType.DuplicateRemoval:
                    LoadDuplicateRemovalConfig();
                    break;
                case CleanupRuleType.EmptyValueRemoval:
                    LoadEmptyValueRemovalConfig();
                    break;
                case CleanupRuleType.ExpiredDataRemoval:
                    LoadExpiredDataRemovalConfig();
                    break;
                case CleanupRuleType.InvalidReferenceRemoval:
                    LoadInvalidReferenceConfig();
                    break;
                case CleanupRuleType.CustomConditionRemoval:
                    LoadCustomConditionConfig();
                    break;
                case CleanupRuleType.DataStandardization:
                    LoadStandardizationConfig();
                    break;
                case CleanupRuleType.DataTruncation:
                    LoadTruncationConfig();
                    break;
            }
        }

        /// <summary>
        /// 加载重复数据清理配置
        /// </summary>
        private void LoadDuplicateRemovalConfig()
        {
            // 这里可以加载重复数据清理的特定配置
        }

        /// <summary>
        /// 加载空值清理配置
        /// </summary>
        private void LoadEmptyValueRemovalConfig()
        {
            // 这里可以加载空值清理的特定配置
        }

        /// <summary>
        /// 加载过期数据清理配置
        /// </summary>
        private void LoadExpiredDataRemovalConfig()
        {
            // 这里可以加载过期数据清理的特定配置
        }

        /// <summary>
        /// 加载无效关联清理配置
        /// </summary>
        private void LoadInvalidReferenceConfig()
        {
            // 这里可以加载无效关联清理的特定配置
        }

        /// <summary>
        /// 加载自定义条件清理配置
        /// </summary>
        private void LoadCustomConditionConfig()
        {
            // 这里可以加载自定义条件清理的特定配置
        }

        /// <summary>
        /// 加载数据标准化配置
        /// </summary>
        private void LoadStandardizationConfig()
        {
            // 这里可以加载数据标准化的特定配置
        }

        /// <summary>
        /// 加载数据截断配置
        /// </summary>
        private void LoadTruncationConfig()
        {
            // 这里可以加载数据截断的特定配置
        }

        /// <summary>
        /// 规则类型改变事件
        /// </summary>
        private void KcmbRuleType_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateUIState();
        }

        /// <summary>
        /// 更新界面状态
        /// </summary>
        private void UpdateUIState()
        {
            if (kcmbRuleType.SelectedItem == null)
            {
                return;
            }

            var selectedType = (CleanupRuleType)((ComboBoxItem)kcmbRuleType.SelectedItem).Value;

            // 根据规则类型显示/隐藏不同的配置面板
            // 这里简化处理，实际应该根据类型切换不同的配置界面
        }

        /// <summary>
        /// 确定按钮点击事件
        /// </summary>
        private void KbtnOK_Click(object sender, EventArgs e)
        {
            try
            {
                // 验证输入
                if (string.IsNullOrWhiteSpace(ktxtRuleName.Text))
                {
                    MessageBox.Show("规则名称不能为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ktxtRuleName.Focus();
                    return;
                }

                if (kcmbRuleType.SelectedItem == null)
                {
                    MessageBox.Show("请选择规则类型", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    kcmbRuleType.Focus();
                    return;
                }

                if (kcmbActionType.SelectedItem == null)
                {
                    MessageBox.Show("请选择操作类型", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    kcmbActionType.Focus();
                    return;
                }

                // 保存规则
                Rule.RuleName = ktxtRuleName.Text.Trim();
                Rule.Description = ktxtDescription.Text;
                Rule.IsEnabled = kchkEnabled.Checked;
                Rule.RuleType = (CleanupRuleType)((ComboBoxItem)kcmbRuleType.SelectedItem).Value;
                Rule.ActionType = (CleanupActionType)((ComboBoxItem)kcmbActionType.SelectedItem).Value;

                // 保存规则类型特定配置
                SaveRuleTypeSpecificConfig();

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存规则失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 保存规则类型特定配置
        /// </summary>
        private void SaveRuleTypeSpecificConfig()
        {
            switch (Rule.RuleType)
            {
                case CleanupRuleType.DuplicateRemoval:
                    SaveDuplicateRemovalConfig();
                    break;
                case CleanupRuleType.EmptyValueRemoval:
                    SaveEmptyValueRemovalConfig();
                    break;
                case CleanupRuleType.ExpiredDataRemoval:
                    SaveExpiredDataRemovalConfig();
                    break;
                case CleanupRuleType.InvalidReferenceRemoval:
                    SaveInvalidReferenceConfig();
                    break;
                case CleanupRuleType.CustomConditionRemoval:
                    SaveCustomConditionConfig();
                    break;
                case CleanupRuleType.DataStandardization:
                    SaveStandardizationConfig();
                    break;
                case CleanupRuleType.DataTruncation:
                    SaveTruncationConfig();
                    break;
            }
        }

        /// <summary>
        /// 保存重复数据清理配置
        /// </summary>
        private void SaveDuplicateRemovalConfig()
        {
            // 这里可以保存重复数据清理的特定配置
        }

        /// <summary>
        /// 保存空值清理配置
        /// </summary>
        private void SaveEmptyValueRemovalConfig()
        {
            // 这里可以保存空值清理的特定配置
        }

        /// <summary>
        /// 保存过期数据清理配置
        /// </summary>
        private void SaveExpiredDataRemovalConfig()
        {
            // 这里可以保存过期数据清理的特定配置
        }

        /// <summary>
        /// 保存无效关联清理配置
        /// </summary>
        private void SaveInvalidReferenceConfig()
        {
            // 这里可以保存无效关联清理的特定配置
        }

        /// <summary>
        /// 保存自定义条件清理配置
        /// </summary>
        private void SaveCustomConditionConfig()
        {
            // 这里可以保存自定义条件清理的特定配置
        }

        /// <summary>
        /// 保存数据标准化配置
        /// </summary>
        private void SaveStandardizationConfig()
        {
            // 这里可以保存数据标准化的特定配置
        }

        /// <summary>
        /// 保存数据截断配置
        /// </summary>
        private void SaveTruncationConfig()
        {
            // 这里可以保存数据截断的特定配置
        }

        /// <summary>
        /// 取消按钮点击事件
        /// </summary>
        private void KbtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// 获取规则类型显示名称
        /// </summary>
        private string GetRuleTypeDisplayName(CleanupRuleType ruleType)
        {
            var descriptions = new Dictionary<CleanupRuleType, string>
            {
                { CleanupRuleType.DuplicateRemoval, "重复数据清理" },
                { CleanupRuleType.EmptyValueRemoval, "空值数据清理" },
                { CleanupRuleType.AbnormalDataRemoval, "异常数据清理" },
                { CleanupRuleType.ExpiredDataRemoval, "过期数据清理" },
                { CleanupRuleType.InvalidReferenceRemoval, "无效关联清理" },
                { CleanupRuleType.CustomConditionRemoval, "自定义条件清理" },
                { CleanupRuleType.DataStandardization, "数据标准化" },
                { CleanupRuleType.DataTruncation, "数据截断" }
            };

            return descriptions.ContainsKey(ruleType) ? descriptions[ruleType] : ruleType.ToString();
        }

        /// <summary>
        /// 获取操作类型显示名称
        /// </summary>
        private string GetActionTypeDisplayName(CleanupActionType actionType)
        {
            var descriptions = new Dictionary<CleanupActionType, string>
            {
                { CleanupActionType.Delete, "删除记录" },
                { CleanupActionType.MarkAsInvalid, "标记为无效" },
                { CleanupActionType.Archive, "归档到历史表" },
                { CleanupActionType.UpdateField, "更新字段值" },
                { CleanupActionType.LogOnly, "仅记录不执行" }
            };

            return descriptions.ContainsKey(actionType) ? descriptions[actionType] : actionType.ToString();
        }

        /// <summary>
        /// 下拉框项类
        /// </summary>
        private class ComboBoxItem
        {
            public string Text { get; set; }
            public object Value { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }
    }
}
