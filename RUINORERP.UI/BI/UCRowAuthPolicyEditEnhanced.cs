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
        private BizMapperService.IEntityInfoService _entityInfoService;
        private SmartRuleConfigHelper _smartRuleHelper;
        private List<BizMapperService.EntityInfo> _allEntityInfos;
        private Dictionary<string, Type> _entityTypeCache = new Dictionary<string, Type>();

        public UCRowAuthPolicyEditEnhanced()
        {
            InitializeComponent();
            _ruleProvider = Startup.GetFromFac<IDefaultRowAuthRuleProvider>();
            _entityInfoService = Startup.GetFromFac<IEntityInfoService>();
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
            
            // 初始化操作符下拉列表
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
            if (cmbBizType.SelectedIndex <= 0) return;
            
            try
            {
                string bizTypeName = cmbBizType.SelectedItem.ToString();
                BizType bizType = (BizType)Enum.Parse(typeof(BizType), bizTypeName);
                
                // 获取实体信息
                EntityInfo entityInfo = _entityInfoService.GetEntityInfo(bizType);
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
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.LogError(ex, "处理业务类型选择失败");
                MessageBox.Show("处理业务类型选择失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            if (cmbTargetTable.SelectedIndex <= 0) return;
            
            try
            {
                string tableName = cmbTargetTable.SelectedItem.ToString();
                
                // 根据表名获取实体信息
                EntityInfo entityInfo = _entityInfoService.GetEntityInfoByTableName(tableName);
                if (entityInfo != null)
                {
                    txtTargetTable.Text = entityInfo.TableName;
                    txtTargetEntity.Text = entityInfo.EntityName;
                    txtEntityType.Text = entityInfo.FullTypeName;
                    
                    // 加载实体字段
                    LoadEntityFields(entityInfo.EntityType);
                }
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
            if (cmbJoinField.SelectedIndex <= 0) return;
            
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
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.Error(ex, "处理关联字段选择失败");
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
        }

        /// <summary>
        /// 获取表的主键字段
        /// </summary>
        private string GetPrimaryKeyField(string tableName)
        {
            try
            {
                BizMapperService.EntityInfo entityInfo = _entityInfoService.GetEntityInfoByTableName(tableName);
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
            }
            catch (Exception ex)
            {
                MainForm.Instance.logger.LogError(ex, "生成过滤条件失败");
                MessageBox.Show("生成过滤条件失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 生成过滤条件表达式
        /// </summary>
        private string GenerateFilterCondition(string fieldName, string operatorType, string filterValue)
        {
            // 根据不同操作符生成不同的条件表达式
            switch (operatorType)
            {
                case "=":
                    return $"[{fieldName}] = '{filterValue}'";
                case "<>":
                    return $"[{fieldName}] <> '{filterValue}'";
                case ">":
                    return $"[{fieldName}] > '{filterValue}'";
                case "<":
                    return $"[{fieldName}] < '{filterValue}'";
                case ">=":
                    return $"[{fieldName}] >= '{filterValue}'";
                case "<=":
                    return $"[{fieldName}] <= '{filterValue}'";
                case "LIKE":
                    return $"[{fieldName}] LIKE '%{filterValue}%'";
                case "IN":
                    return $"[{fieldName}] IN ({filterValue})";
                default:
                    return $"[{fieldName}] = '{filterValue}'";
            }
        }

        /// <summary>
        /// 是否需要联表复选框变化处理
        /// </summary>
        private void ChkIsJoinRequired_CheckedChanged(object sender, EventArgs e)
        {
            // 根据是否需要联表显示或隐藏联表相关控件
            grpJoinTable.Visible = chkIsJoinRequired.Checked;
        }

        public override void BindData(BaseEntity entity)
        {
            tb_RowAuthPolicy _EditEntity = entity as tb_RowAuthPolicy;
            
            DataBindingHelper.BindData4TextBox<tb_RowAuthPolicy>(entity, t => t.PolicyName, txtPolicyName, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_RowAuthPolicy>(entity, t => t.TargetTable, txtTargetTable, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_RowAuthPolicy>(entity, t => t.TargetEntity, txtTargetEntity, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4CheckBox<tb_RowAuthPolicy>(entity, t => t.IsJoinRequired, chkIsJoinRequired, false);
            DataBindingHelper.BindData4TextBox<tb_RowAuthPolicy>(entity, t => t.JoinTable, txtJoinTable, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_RowAuthPolicy>(entity, t => t.JoinType, txtJoinType, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_RowAuthPolicy>(entity, t => t.JoinOnClause, txtJoinOnClause, BindDataType4TextBox.Text, false);
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
                // 在提交前验证过滤条件
                if (!string.IsNullOrEmpty(txtFilterClause.Text))
                {
                    if (!_smartRuleHelper.ValidateFilterClause(txtFilterClause.Text, out string errorMessage))
                    {
                        MessageBox.Show("过滤条件无效: " + errorMessage, "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                
                // 验证关联配置
                if (chkIsJoinRequired.Checked)
                {
                    if (string.IsNullOrEmpty(txtJoinTable.Text))
                    {
                        MessageBox.Show("请选择关联表", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    
                    if (string.IsNullOrEmpty(txtJoinOnClause.Text))
                    {
                        MessageBox.Show("请输入关联条件", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
}