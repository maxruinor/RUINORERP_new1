using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RUINORERP.Common;
using RUINORERP.UI.UCToolBar;
using RUINORERP.Model;
using Krypton.Toolkit;
using RUINORERP.UI.BaseForm;
using RUINORERP.Business.LogicaService;
using RUINORERP.Business;
using RUINORERP.UI.Common;
using RUINORERP.Global;
using RUINORERP.Business.RowLevelAuthService;
using BizMapperService = RUINORERP.Business.BizMapperService;
using RUINORERP.Common.Helper;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RUINORERP.Business.BizMapperService;
using RUINORERP.Global.CustomAttribute;

namespace RUINORERP.UI.BI
{
    /// <summary>
    /// 增强版行级数据权限规则编辑界面
    /// 提供智能配置功能，帮助用户轻松创建和管理数据权限规则
    /// </summary>
    [MenuAttrAssemblyInfo("智能数据权限规则", true, UIType.单表数据)]
    public partial class UCRowAuthPolicyEditEnhanced : BaseEditGeneric<tb_RowAuthPolicy>
    {
        private IDefaultRowAuthRuleProvider _ruleProvider;
        private BizMapperService.IEntityMappingService _entityInfoService;
        private SmartRuleConfigHelper _smartRuleHelper;
        private List<BizMapperService.BizEntityInfo> _allEntityInfos;
        private Dictionary<string, Type> _entityTypeCache = new Dictionary<string, Type>();
        private List<EntityFieldInfo> _currentEntityFields = new List<EntityFieldInfo>();
        private List<DefaultRuleOption> _defaultRuleOptions = new List<DefaultRuleOption>();
        private bool _isBinding = false; // 防止在绑定数据时触发事件
        private Timer _previewTimer; // 用于延迟更新预览的计时器

        public UCRowAuthPolicyEditEnhanced()
        {
            InitializeComponent();
            _ruleProvider = Startup.GetFromFac<IDefaultRowAuthRuleProvider>();
            _entityInfoService = Startup.GetFromFac<IEntityMappingService>();
            // 使用DI容器获取SmartRuleConfigHelper实例
            _smartRuleHelper = new SmartRuleConfigHelper(_entityInfoService, Startup.GetFromFac<ILoggerFactory>());
            InitializeSmartComponents();
        }

        /// <summary>
        /// 初始化智能组件
        /// </summary>
        private void InitializeSmartComponents()
        {
            // 加载所有注册的业务类型
            LoadBusinessTypes();
            // 添加事件处理
            cmbBizType.SelectedIndexChanged += CmbBizType_SelectedIndexChanged;
            chkIsJoinRequired.CheckedChanged += ChkIsJoinRequired_CheckedChanged;
            cmbTargetTable.SelectedIndexChanged += CmbTargetTable_SelectedIndexChanged;
            cmbJoinField.SelectedIndexChanged += CmbJoinField_SelectedIndexChanged;
            btnGenerateFilterClause.Click += BtnGenerateFilterClause_Click;
            cmbFilterField.SelectedIndexChanged += CmbFilterField_SelectedIndexChanged;
            cmbDefaultRule.SelectedIndexChanged += CmbDefaultRule_SelectedIndexChanged;
            cmbJoinTable.SelectedIndexChanged += CmbJoinTable_SelectedIndexChanged; // 添加新事件处理
            
            // 添加实时预览相关事件
            txtFilterClause.TextChanged += UpdatePreview;
            txtJoinOnClause.TextChanged += UpdatePreview;
            txtJoinType.TextChanged += UpdatePreview;
            cmbJoinTable.SelectedIndexChanged += UpdatePreview; // 添加cmbJoinTable的预览更新事件
            
            // 初始化操作符下拉列表
            cmbOperator.Items.Clear();
            cmbOperator.Items.AddRange(new object[] {
                "=",
                "<>",
                ">",
                "<",
                ">=",
                "<=",
                "LIKE",
                "IN"
            });
            cmbOperator.SelectedIndex = 0;
            
            // 初始化预览计时器
            _previewTimer = new Timer();
            _previewTimer.Interval = 500; // 500毫秒延迟
            _previewTimer.Tick += (s, e) => {
                _previewTimer.Stop();
                GeneratePreview();
            };
        }

        /// <summary>
        /// 加载所有业务类型到下拉列表
        /// </summary>
        private void LoadBusinessTypes()
        {
            try
            {
                _allEntityInfos = _entityInfoService.GetAllEntityInfos().ToList();
                
                // 清空并添加所有业务类型
                cmbBizType.Items.Clear();
                cmbBizType.Items.Add("请选择业务类型");
                
                // 获取所有不重复的业务类型
                var bizTypes = _allEntityInfos
                    .Where(e => e.BizType != BizType.无对应数据)
                    .Select(e => e.BizType)
                    .Distinct()
                    .OrderBy(e => e.ToString())
                    .ToList();
                
                foreach (var bizType in bizTypes)
                {
                    cmbBizType.Items.Add(bizType.ToString());
                }
                
                cmbBizType.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.LogError(ex, "加载业务类型失败");
                MessageBox.Show("加载业务类型失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 业务类型选择变化处理
        /// </summary>
        private void CmbBizType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isBinding || cmbBizType.SelectedIndex <= 0) return;
            
            try
            {
                string bizTypeName = cmbBizType.SelectedItem.ToString();
                BizType bizType = (BizType)Enum.Parse(typeof(BizType), bizTypeName);
                
                // 获取默认规则选项
                _defaultRuleOptions = _ruleProvider.GetDefaultRuleOptions(bizType);
                
                // 更新默认规则下拉框
                UpdateDefaultRuleComboBox();
                
                // 获取实体信息
                BizEntityInfo entityInfo = _entityInfoService.GetEntityInfo(bizType);
                if (entityInfo != null)
                {
                    // 填充表单
                    txtPolicyName.Text = $"{bizTypeName}权限规则";
                    txtTargetTable.Text = entityInfo.TableName;
                    txtTargetEntity.Text = entityInfo.EntityName;
                    txtEntityType.Text = entityInfo.FullTypeName;
                    
                    // 刷新表下拉列表并选中当前表
                    RefreshTableDropdown(entityInfo.TableName);
                    
                    // 加载实体字段
                    LoadEntityFields(entityInfo.EntityType);
                }
                
                // 更新预览
                UpdatePreview(sender, e);
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.LogError(ex, "处理业务类型选择失败");
                MessageBox.Show("处理业务类型选择失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 更新默认规则下拉框
        /// </summary>
        private void UpdateDefaultRuleComboBox()
        {
            cmbDefaultRule.Items.Clear();
            cmbDefaultRule.Items.Add("自定义规则");
            
            foreach (var option in _defaultRuleOptions)
            {
                cmbDefaultRule.Items.Add(new ComboBoxItem { Text = option.Name, Value = option });
            }
            
            cmbDefaultRule.SelectedIndex = 0;
        }

        /// <summary>
        /// 默认规则选项变化处理
        /// </summary>
        private void CmbDefaultRule_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isBinding || cmbDefaultRule.SelectedIndex <= 0) return;
            
            try
            {
                var selectedItem = cmbDefaultRule.SelectedItem as ComboBoxItem;
                if (selectedItem != null && selectedItem.Value is DefaultRuleOption option)
                {
                    // 应用默认规则选项
                    ApplyDefaultRuleOption(option);
                    
                    // 更新预览
                    UpdatePreview(sender, e);
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.LogError(ex, "处理默认规则选项失败");
                MessageBox.Show("处理默认规则选项失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 应用默认规则选项
        /// </summary>
        private void ApplyDefaultRuleOption(DefaultRuleOption option)
        {
            try
            {
                string bizTypeName = cmbBizType.SelectedItem.ToString();
                BizType bizType = (BizType)Enum.Parse(typeof(BizType), bizTypeName);
                
                // 使用规则提供者创建策略
                var policy = _ruleProvider.CreatePolicyFromDefaultOption(bizType, option, 0);
                
                // 应用到界面
                chkIsJoinRequired.Checked = policy.IsJoinRequired ?? false;
                txtJoinType.Text = policy.JoinType ?? "";
                txtJoinOnClause.Text = policy.JoinOnClause ?? "";
                txtFilterClause.Text = policy.FilterClause ?? "";
                txtTargetTableJoinField.Text = policy.TargetTableJoinField ?? "";
                txtJoinTableJoinField.Text = policy.JoinTableJoinField ?? "";
                
                // 如果需要联表，设置相关字段
                if (policy.IsJoinRequired ?? false)
                {
                    cmbJoinTable.Text = policy.JoinTable ?? "";
                }
                
                // 更新规则名称
                if (!string.IsNullOrEmpty(policy.PolicyName))
                {
                    txtPolicyName.Text = policy.PolicyName;
                }
                
                // 显示规则描述
                if (!string.IsNullOrEmpty(option.Description))
                {
                    txtPolicyDescription.Text = option.Description;
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.LogError(ex, "应用默认规则选项失败");
                MessageBox.Show("应用默认规则选项失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 刷新表下拉列表
        /// </summary>
        private void RefreshTableDropdown(string selectedTableName = null)
        {
            cmbTargetTable.Items.Clear();
            cmbTargetTable.Items.Add("请选择表");
            
            var tableNames = _allEntityInfos
                .Where(e => !string.IsNullOrEmpty(e.TableName))
                .Select(e => e.TableName)
                .Distinct()
                .OrderBy(t => t)
                .ToList();
            
            foreach (var tableName in tableNames)
            {
                cmbTargetTable.Items.Add(tableName);
            }
            
            if (!string.IsNullOrEmpty(selectedTableName))
            {
                int index = cmbTargetTable.Items.IndexOf(selectedTableName);
                if (index > 0) cmbTargetTable.SelectedIndex = index;
            }
            else
            {
                cmbTargetTable.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// 表选择变化处理
        /// </summary>
        private void CmbTargetTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isBinding || cmbTargetTable.SelectedIndex <= 0) return;
            
            try
            {
                string tableName = cmbTargetTable.SelectedItem.ToString();
                
                // 根据表名获取实体信息
                BizEntityInfo entityInfo = _entityInfoService.GetEntityInfoByTableName(tableName);
                if (entityInfo != null)
                {
                    txtTargetTable.Text = entityInfo.TableName;
                    txtTargetEntity.Text = entityInfo.EntityName;
                    txtEntityType.Text = entityInfo.FullTypeName;
                    
                    // 加载实体字段
                    LoadEntityFields(entityInfo.EntityType);
                }
                
                // 更新预览
                UpdatePreview(sender, e);
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.LogError(ex, "处理表选择失败");
                MessageBox.Show("处理表选择失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 加载实体字段信息
        /// </summary>
        private void LoadEntityFields(Type entityType)
        {
            if (entityType == null) return;
            
            try
            {
                // 获取实体字段信息
                _currentEntityFields = _smartRuleHelper.GetEntityFields(entityType);
                
                // 清空下拉列表
                cmbFilterField.Items.Clear();
                cmbJoinField.Items.Clear();
                cmbJoinTable.Items.Clear();
                
                cmbFilterField.Items.Add("请选择字段");
                cmbJoinField.Items.Add("请选择关联字段");
                cmbJoinTable.Items.Add("请选择关联表");
                
                // 获取实体的所有属性
                var properties = entityType.GetProperties();
                
                // 存储外键属性信息
                List<PropertyInfo> foreignKeyProperties = new List<PropertyInfo>();
                
                foreach (var property in properties)
                {
                    // 添加到过滤字段下拉列表
                    cmbFilterField.Items.Add(property.Name);
                    
                    // 检查是否是外键属性（通过特性判断）
                    var fkAttr = Attribute.GetCustomAttribute(property, typeof(FKRelationAttribute)) as FKRelationAttribute;
                    if (fkAttr != null)
                    {
                        foreignKeyProperties.Add(property);
                        cmbJoinField.Items.Add($"{property.Name} (关联 {fkAttr.FKTableName})");
                    }
                }
                
                // 设置默认选中项
                cmbFilterField.SelectedIndex = 0;
                cmbJoinField.SelectedIndex = 0;
                
                // 加载关联表选项
                foreach (var fkProp in foreignKeyProperties)
                {
                    var fkAttr = Attribute.GetCustomAttribute(fkProp, typeof(FKRelationAttribute)) as FKRelationAttribute;
                    if (fkAttr != null && !cmbJoinTable.Items.Contains(fkAttr.FKTableName))
                    {
                        cmbJoinTable.Items.Add(fkAttr.FKTableName);
                    }
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.LogError(ex, "加载实体字段失败");
                MessageBox.Show("加载实体字段失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 关联字段选择变化处理
        /// </summary>
        private void CmbJoinField_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isBinding || cmbJoinField.SelectedIndex <= 0) return;
            
            try
            {
                string selectedText = cmbJoinField.SelectedItem.ToString();
                // 提取关联表名
                if (selectedText.Contains("关联 "))
                {
                    string tableName = selectedText.Substring(selectedText.IndexOf("关联 ") + 3).TrimEnd(')');
                    
                    // 在关联表下拉列表中选择对应的表
                    int index = cmbJoinTable.Items.IndexOf(tableName);
                    if (index > 0)
                    {
                        cmbJoinTable.SelectedIndex = index;
                        // 自动生成关联条件
                        GenerateJoinCondition(selectedText.Split(' ')[0], tableName);
                    }
                }
                
                // 更新预览
                UpdatePreview(sender, e);
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.Error(ex, "处理关联字段选择失败");
            }
        }

        /// <summary>
        /// 过滤字段选择变化处理
        /// </summary>
        private void CmbFilterField_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isBinding || cmbFilterField.SelectedIndex <= 0) return;

            try
            {
                string fieldName = cmbFilterField.SelectedItem.ToString();
                var fieldInfo = _currentEntityFields.FirstOrDefault(f => f.FieldName == fieldName);
                
                if (fieldInfo != null)
                {
                    // 根据字段类型自动选择操作符
                    if (fieldInfo.FieldType == typeof(string))
                    {
                        cmbOperator.SelectedItem = "LIKE";
                    }
                    else if (fieldInfo.FieldType == typeof(DateTime) || fieldInfo.FieldType == typeof(DateTime?))
                    {
                        cmbOperator.SelectedItem = ">=";
                    }
                    else
                    {
                        cmbOperator.SelectedItem = "=";
                    }
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.LogError(ex, "处理过滤字段选择失败");
            }
        }

        /// <summary>
        /// 自动生成关联条件
        /// </summary>
        private void GenerateJoinCondition(string foreignKeyField, string joinTableName)
        {
            cmbJoinTable.Text = joinTableName;
            txtJoinType.Text = "INNER JOIN";
            txtJoinOnClause.Text = $"{txtTargetTable.Text}.{foreignKeyField} = {joinTableName}.[{GetPrimaryKeyField(joinTableName)}]";
            
            // 自动填充关联字段
            txtTargetTableJoinField.Text = foreignKeyField;
            txtJoinTableJoinField.Text = GetPrimaryKeyField(joinTableName);
            
            // 更新预览
            GeneratePreview();
        }

        /// <summary>
        /// 获取表的主键字段
        /// </summary>
        private string GetPrimaryKeyField(string tableName)
        {
            try
            {
                BizMapperService.BizEntityInfo entityInfo = _entityInfoService.GetEntityInfoByTableName(tableName);
                return entityInfo?.IdField ?? "ID";
            }
            catch
            {
                return "ID";
            }
        }

        /// <summary>
        /// 生成过滤条件
        /// </summary>
        private void BtnGenerateFilterClause_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbFilterField.SelectedIndex <= 0) return;
                
                string fieldName = cmbFilterField.SelectedItem.ToString();
                string operatorType = cmbOperator.SelectedItem?.ToString() ?? "=";
                string filterValue = txtFilterValue.Text.Trim();
                
                if (string.IsNullOrEmpty(filterValue))
                {
                    MessageBox.Show("请输入过滤值", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtFilterValue.Focus();
                    return;
                }
                
                // 根据字段类型验证输入值
                var fieldInfo = _currentEntityFields.FirstOrDefault(f => f.FieldName == fieldName);
                if (fieldInfo != null && !ValidateFieldValue(fieldInfo, operatorType, filterValue))
                {
                    txtFilterValue.Focus();
                    return;
                }
                
                // 生成过滤条件
                string filterClause = GenerateFilterCondition(fieldName, operatorType, filterValue);
                
                // 使用SmartRuleConfigHelper验证生成的条件
                if (!_smartRuleHelper.ValidateFilterClause(filterClause, out string errorMessage))
                {
                    MessageBox.Show("生成的条件无效: " + errorMessage, "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                // 如果已有条件，则用AND连接
                if (!string.IsNullOrEmpty(txtFilterClause.Text))
                {
                    txtFilterClause.Text += " AND " + filterClause;
                }
                else
                {
                    txtFilterClause.Text = filterClause;
                }
                
                // 如果还没有规则名称，自动生成一个
                if (string.IsNullOrEmpty(txtPolicyName.Text))
                {
                    Type entityType = GetEntityTypeByName(txtTargetEntity.Text);
                    if (entityType != null)
                    {
                        txtPolicyName.Text = _smartRuleHelper.GetSuggestedPolicyName(entityType, txtFilterClause.Text);
                    }
                }
                
                // 清空输入框以便添加下一个条件
                txtFilterValue.Text = "";
                cmbFilterField.Focus();
                
                // 更新预览
                UpdatePreview(sender, e);
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.LogError(ex, "生成过滤条件失败");
                MessageBox.Show("生成过滤条件失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 验证字段值是否符合字段类型要求
        /// </summary>
        private bool ValidateFieldValue(EntityFieldInfo fieldInfo, string operatorType, string value)
        {
            try
            {
                // 对于IN操作符，允许多个值
                if (operatorType == "IN")
                {
                    // 简单检查是否为列表格式
                    if (!value.StartsWith("(") || !value.EndsWith(")"))
                    {
                        MessageBox.Show("IN操作符的值应使用括号括起来，例如: (1,2,3)", "输入格式错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                    return true;
                }
                
                // 对于数值类型字段
                if (fieldInfo.FieldType == typeof(int) || fieldInfo.FieldType == typeof(long) || 
                    fieldInfo.FieldType == typeof(decimal) || fieldInfo.FieldType == typeof(double) ||
                    fieldInfo.FieldType == typeof(int?) || fieldInfo.FieldType == typeof(long?) || 
                    fieldInfo.FieldType == typeof(decimal?) || fieldInfo.FieldType == typeof(double?))
                {
                    if (!double.TryParse(value, out _))
                    {
                        MessageBox.Show($"字段 {fieldInfo.FieldName} 是数值类型，请输入有效的数字", "输入格式错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }
                // 对于布尔类型字段
                else if (fieldInfo.FieldType == typeof(bool) || fieldInfo.FieldType == typeof(bool?))
                {
                    if (value.ToLower() != "true" && value.ToLower() != "false" && value != "1" && value != "0")
                    {
                        MessageBox.Show($"字段 {fieldInfo.FieldName} 是布尔类型，请输入 true/false 或 1/0", "输入格式错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }
                // 对于日期类型字段
                else if (fieldInfo.FieldType == typeof(DateTime) || fieldInfo.FieldType == typeof(DateTime?))
                {
                    if (!DateTime.TryParse(value, out _))
                    {
                        MessageBox.Show($"字段 {fieldInfo.FieldName} 是日期类型，请输入有效的日期", "输入格式错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }
                
                return true;
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.LogError(ex, "验证字段值时发生错误");
                return false;
            }
        }

        /// <summary>
        /// 生成过滤条件表达式
        /// </summary>
        private string GenerateFilterCondition(string fieldName, string operatorType, string filterValue)
        {
            // 根据字段类型处理值的格式
            var fieldInfo = _currentEntityFields.FirstOrDefault(f => f.FieldName == fieldName);
            string formattedValue = filterValue;
            
            if (fieldInfo != null)
            {
                // 对于字符串类型和日期类型，需要加引号
                if (fieldInfo.FieldType == typeof(string) || 
                    fieldInfo.FieldType == typeof(DateTime) || fieldInfo.FieldType == typeof(DateTime?))
                {
                    // IN操作符已经有括号了，不需要额外处理
                    if (operatorType != "IN")
                    {
                        formattedValue = $"'{filterValue.Replace("'", "''")}'"; // 转义单引号
                    }
                }
            }
            
            // 根据不同操作符生成不同的条件表达式
            switch (operatorType)
            {
                case "=":
                    return $"[{fieldName}] = {formattedValue}";
                case "<>":
                    return $"[{fieldName}] <> {formattedValue}";
                case ">":
                    return $"[{fieldName}] > {formattedValue}";
                case "<":
                    return $"[{fieldName}] < {formattedValue}";
                case ">=":
                    return $"[{fieldName}] >= {formattedValue}";
                case "<=":
                    return $"[{fieldName}] <= {formattedValue}";
                case "LIKE":
                    return $"[{fieldName}] LIKE '%{filterValue.Replace("'", "''")}%'";
                case "IN":
                    return $"[{fieldName}] IN {filterValue}";
                default:
                    return $"[{fieldName}] = {formattedValue}";
            }
        }

        /// <summary>
        /// 是否需要联表复选框变化处理
        /// </summary>
        private void ChkIsJoinRequired_CheckedChanged(object sender, EventArgs e)
        {
            // 根据是否需要联表显示或隐藏联表相关控件
            grpJoinTable.Visible = chkIsJoinRequired.Checked;
            
            // 如果不需要联表，清空相关字段
            if (!chkIsJoinRequired.Checked)
            {
                cmbJoinTable.SelectedIndex = -1; // 清空选择而不是选择第一项
                cmbJoinTable.Text = "";
                cmbJoinField.SelectedIndex = -1;
                cmbJoinField.Text = "";
                txtJoinType.Text = "";
                txtJoinOnClause.Text = "";
                txtTargetTableJoinField.Text = "";
                txtJoinTableJoinField.Text = "";
            }
            
            // 更新预览
            UpdatePreview(sender, e);
        }

        /// <summary>
        /// 关联表选择变化处理
        /// </summary>
        private void CmbJoinTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isBinding || cmbJoinTable.SelectedIndex <= 0) return;
            
            try
            {
                // 当用户手动选择关联表时，更新预览
                UpdatePreview(sender, e);
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.Error(ex, "处理关联表选择失败");
            }
        }

        /// <summary>
        /// 更新预览显示
        /// </summary>
        private void UpdatePreview(object sender, EventArgs e)
        {
            // 重启计时器以延迟更新预览
            _previewTimer.Stop();
            _previewTimer.Start();
        }

        /// <summary>
        /// 生成并显示预览
        /// </summary>
        private void GeneratePreview()
        {
            try
            {
                // 构建预览SQL
                StringBuilder previewSql = new StringBuilder();
                previewSql.AppendLine("-- 预览权限规则生成的SQL语句");
                
                if (!string.IsNullOrEmpty(txtTargetTable.Text))
                {
                    previewSql.AppendLine($"SELECT * FROM [{txtTargetTable.Text}]");
                    
                    // 如果需要联表
                    if (chkIsJoinRequired.Checked && !string.IsNullOrEmpty(cmbJoinTable.Text))
                    {
                        previewSql.AppendLine($"  {txtJoinType.Text} [{cmbJoinTable.Text}] ON {txtJoinOnClause.Text}");
                    }
                    
                    // 添加过滤条件
                    if (!string.IsNullOrEmpty(txtFilterClause.Text))
                    {
                        previewSql.AppendLine($"WHERE {txtFilterClause.Text}");
                    }
                }
                
                // 在界面上显示预览
                txtPreview.Text = previewSql.ToString();
            }
            catch (Exception ex)
            {
                // 静默处理预览错误，不影响主功能
                MainForm.Instance.logger.LogDebug(ex, "生成预览时发生错误");
                txtPreview.Text = "-- 生成预览时发生错误: " + ex.Message;
            }
        }

        public override void BindData(BaseEntity entity)
        {
            _isBinding = true; // 设置绑定标志，防止触发事件
            
            try
            {
                tb_RowAuthPolicy _EditEntity = entity as tb_RowAuthPolicy;
                
                DataBindingHelper.BindData4TextBox<tb_RowAuthPolicy>(entity, t => t.PolicyName, txtPolicyName, BindDataType4TextBox.Text, false);
                DataBindingHelper.BindData4TextBox<tb_RowAuthPolicy>(entity, t => t.TargetTable, txtTargetTable, BindDataType4TextBox.Text, false);
                DataBindingHelper.BindData4TextBox<tb_RowAuthPolicy>(entity, t => t.TargetEntity, txtTargetEntity, BindDataType4TextBox.Text, false);
                DataBindingHelper.BindData4CheckBox<tb_RowAuthPolicy>(entity, t => t.IsJoinRequired, chkIsJoinRequired, false);
                // 修复：正确处理cmbJoinTable控件的数据绑定
                // 由于cmbJoinTable是ComboBox控件，需要特殊处理
                if (_EditEntity != null)
                {
                    cmbJoinTable.Text = _EditEntity.JoinTable ?? "";
                }
                DataBindingHelper.BindData4TextBox<tb_RowAuthPolicy>(entity, t => t.JoinType, txtJoinType, BindDataType4TextBox.Text, false);
                DataBindingHelper.BindData4TextBox<tb_RowAuthPolicy>(entity, t => t.JoinOnClause, txtJoinOnClause, BindDataType4TextBox.Text, false);
                DataBindingHelper.BindData4TextBox<tb_RowAuthPolicy>(entity, t => t.TargetTableJoinField, txtTargetTableJoinField, BindDataType4TextBox.Text, false);
                DataBindingHelper.BindData4TextBox<tb_RowAuthPolicy>(entity, t => t.JoinTableJoinField, txtJoinTableJoinField, BindDataType4TextBox.Text, false);
                DataBindingHelper.BindData4TextBox<tb_RowAuthPolicy>(entity, t => t.FilterClause, txtFilterClause, BindDataType4TextBox.Text, false);
                DataBindingHelper.BindData4TextBox<tb_RowAuthPolicy>(entity, t => t.EntityType, txtEntityType, BindDataType4TextBox.Text, false);
                DataBindingHelper.BindData4CheckBox<tb_RowAuthPolicy>(entity, t => t.IsEnabled, chkIsEnabled, false);
                DataBindingHelper.BindData4TextBox<tb_RowAuthPolicy>(entity, t => t.PolicyDescription, txtPolicyDescription, BindDataType4TextBox.Text, false);
                
                // 初始化智能控件状态
                grpJoinTable.Visible = chkIsJoinRequired.Checked;
                
                // 如果已有表名，尝试在下拉列表中选中
                if (!string.IsNullOrEmpty(txtTargetTable.Text))
                {
                    RefreshTableDropdown(txtTargetTable.Text);
                    
                    // 尝试加载实体字段
                    try
                    {
                        Type entityType = GetEntityTypeByName(txtTargetEntity.Text);
                        if (entityType != null)
                        {
                            LoadEntityFields(entityType);
                        }
                    }
                    catch { }
                }
                
                base.BindData(entity);
                
                // 更新预览
                GeneratePreview();
            }
            finally
            {
                _isBinding = false; // 重置绑定标志
            }
        }

        /// <summary>
        /// 根据实体名称获取实体类型
        /// </summary>
        private Type GetEntityTypeByName(string entityName)
        {
            if (string.IsNullOrEmpty(entityName)) return null;
            
            // 先从缓存中查找
            if (_entityTypeCache.TryGetValue(entityName, out Type cachedType))
            {
                return cachedType;
            }
            
            try
            {
                // 查找实体类型
                Type entityType = _allEntityInfos
                    .Where(e => e.EntityName == entityName)
                    .Select(e => e.EntityType)
                    .FirstOrDefault();
                
                if (entityType != null)
                {
                    _entityTypeCache[entityName] = entityType;
                }
                
                return entityType;
            }
            catch
            {
                return null;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            bindingSourceEdit.CancelEdit();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                // 检查必填字段
                if (string.IsNullOrEmpty(txtPolicyName.Text))
                {
                    MessageBox.Show("请输入规则名称", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPolicyName.Focus();
                    return;
                }
                
                if (string.IsNullOrEmpty(txtTargetTable.Text))
                {
                    MessageBox.Show("请选择目标表", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cmbTargetTable.Focus();
                    return;
                }
                
                if (string.IsNullOrEmpty(txtTargetEntity.Text))
                {
                    MessageBox.Show("实体信息不完整", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                // 在提交前验证过滤条件
                if (!string.IsNullOrEmpty(txtFilterClause.Text))
                {
                    if (!_smartRuleHelper.ValidateFilterClause(txtFilterClause.Text, out string errorMessage))
                    {
                        MessageBox.Show("过滤条件无效: " + errorMessage, "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtFilterClause.Focus();
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("请输入过滤条件", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtFilterClause.Focus();
                    return;
                }
                
                // 验证关联配置
                if (chkIsJoinRequired.Checked)
                {
                    // 修复：正确引用cmbJoinTable控件
                    if (string.IsNullOrEmpty(cmbJoinTable.Text))
                    {
                        MessageBox.Show("请选择关联表", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cmbJoinTable.Focus();
                        return;
                    }
                    
                    if (string.IsNullOrEmpty(txtJoinType.Text))
                    {
                        MessageBox.Show("请输入关联类型", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtJoinType.Focus();
                        return;
                    }
                    
                    if (string.IsNullOrEmpty(txtJoinOnClause.Text))
                    {
                        MessageBox.Show("请输入关联条件", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtJoinOnClause.Focus();
                        return;
                    }
                    
                    if (string.IsNullOrEmpty(txtTargetTableJoinField.Text))
                    {
                        MessageBox.Show("请输入目标表关联字段", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtTargetTableJoinField.Focus();
                        return;
                    }
                    
                    if (string.IsNullOrEmpty(txtJoinTableJoinField.Text))
                    {
                        MessageBox.Show("请输入关联表关联字段", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtJoinTableJoinField.Focus();
                        return;
                    }
                }
                
                if (base.Validator())
                {
                    bindingSourceEdit.EndEdit();
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.LogError(ex, "保存规则失败");
                MessageBox.Show("保存规则失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        

    }
    
    /// <summary>
    /// 下拉框项类，用于在下拉框中存储文本和值
    /// </summary>
    public class ComboBoxItem
    {
        public string Text { get; set; }
        public object Value { get; set; }
        
        public override string ToString()
        {
            return Text;
        }
    }
}