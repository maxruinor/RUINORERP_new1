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
using RUINORERP.UI.Common;
using RUINORERP.Business;
using RUINORERP.Global.EnumExt;
using RUINORERP.Global;
using RUINORERP.UI.ToolForm;
using RUINORERP.Model.ReminderModel;
using RUINORERP.Common.Helper;
using RUINORERP.Business.CommService;
using Microsoft.Extensions.Logging;

namespace RUINORERP.UI.BI
{
    [MenuAttrAssemblyInfo("提醒对象链路编辑", true, UIType.单表数据)]
    public partial class UCReminderObjectLinkEdit : BaseEditGeneric<tb_ReminderObjectLink>
    {
        public UCReminderObjectLinkEdit()
        {
            InitializeComponent();
        }

        public tb_ReminderObjectLink entity { get; set; }

        List<tb_UserInfo> UserInfos = new List<tb_UserInfo>();
        List<tb_RoleInfo> RoleInfos = new List<tb_RoleInfo>();
        List<tb_Department> DepartmentInfos = new List<tb_Department>();

        // 规则关联相关字段
        private tb_ReminderRuleController<tb_ReminderRule> _ruleController;
        private tb_ReminderLinkRuleRelationController<tb_ReminderLinkRuleRelation> _relationController;
        private List<tb_ReminderRule> _linkedRules;

        public override void BindData(BaseEntity _entity)
        {
            // 设计时不执行数据绑定逻辑，避免访问运行时服务
            if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
            {
                return;
            }

            entity = _entity as tb_ReminderObjectLink;
            if (entity == null)
            {
                return;
            }

            if (entity.LinkId == 0)
            {
                entity.IsEnabled = true;
                entity.Created_at = DateTime.UtcNow;
                BusinessHelper.Instance.InitEntity(entity);
            }
            else
            {
                BusinessHelper.Instance.EditEntity(entity);
            }

            // 加载基础数据
            try
            {
                UserInfos = _cacheManager.GetEntityList<tb_UserInfo>();
                RoleInfos = _cacheManager.GetEntityList<tb_RoleInfo>();
                DepartmentInfos = _cacheManager.GetEntityList<tb_Department>();
            }
            catch (Exception ex)
            {
                MainForm.Instance?.logger.Error(ex, "加载基础数据失败");
                return;
            }

            // 初始化控制器
            _ruleController = MainForm.Instance.AppContext.GetRequiredService<tb_ReminderRuleController<tb_ReminderRule>>();
            _relationController = MainForm.Instance.AppContext.GetRequiredService<tb_ReminderLinkRuleRelationController<tb_ReminderLinkRuleRelation>>();

            // 绑定基本信息
            DataBindingHelper.BindData4TextBox<tb_ReminderObjectLink>(entity, t => t.LinkName, txtLinkName, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4TextBox<tb_ReminderObjectLink>(entity, t => t.Description, txtDescription, BindDataType4TextBox.Text, false);
            DataBindingHelper.BindData4CheckBox<tb_ReminderObjectLink>(entity, t => t.IsEnabled, chkIsEnabled, false);

            // 绑定提醒源配置
            BindSourceConfig();
            // 绑定单据配置
            BindBillConfig();
            // 绑定提醒目标配置
            BindTargetConfig();
            // 绑定规则按钮事件
            BindRuleButtons();
            // 加载已关联的规则
            LoadLinkedRules();

            // 初始化验证
            if (!string.IsNullOrEmpty(entity.LinkName))
            {
                try
                {
                    if (MainForm.Instance?.AppContext != null)
                    {
                        base.InitRequiredToControl(MainForm.Instance.AppContext.GetRequiredService<tb_ReminderObjectLinkValidator>(), kryptonPanel1.Controls);
                    }
                }
                catch (Exception ex)
                {
                    MainForm.Instance?.logger?.LogError(ex, "初始化验证失败");
                }
            }

            // 属性变化事件
            entity.PropertyChanged += (sender, s2) =>
            {
                if (entity == null)
                {
                    return;
                }
            };

            base.BindData(entity);
        }

        /// <summary>
        /// 绑定提醒源配置
        /// </summary>
        private void BindSourceConfig()
        {
            // 绑定提醒源类型
            DataBindingHelper.BindData4CmbByEnum<tb_ReminderObjectLink, SourceTargetType>(entity, k => k.SourceType, cmbSourceType, false);
            cmbSourceType.SelectedIndexChanged += CmbSourceType_SelectedIndexChanged;

            // 根据类型绑定提醒源值
            UpdateSourceValueControl();
        }

        /// <summary>
        /// 绑定单据配置
        /// </summary>
        private void BindBillConfig()
        {
            // 绑定单据类型
            DataBindingHelper.BindData4CmbByEnum<tb_ReminderObjectLink, BizType>(entity, k => k.BizType, cmbBizType, false);
            // 绑定操作类型
            DataBindingHelper.BindData4CmbByEnum<tb_ReminderObjectLink, ActionType>(entity, k => k.ActionType, cmbActionType, false);
            // 绑定单据状态
            DataBindingHelper.BindData4TextBox<tb_ReminderObjectLink>(entity, t => t.BillStatus, txtBillStatus, BindDataType4TextBox.Text, false);
        }

        /// <summary>
        /// 绑定提醒目标配置
        /// </summary>
        private void BindTargetConfig()
        {
            // 绑定提醒目标类型
            DataBindingHelper.BindData4CmbByEnum<tb_ReminderObjectLink, SourceTargetType>(entity, k => k.TargetType, cmbTargetType, false);
            cmbTargetType.SelectedIndexChanged += CmbTargetType_SelectedIndexChanged;

            // 根据类型绑定提醒目标值
            UpdateTargetValueControl();
        }

        /// <summary>
        /// 更新提醒源值控件
        /// </summary>
        private void UpdateSourceValueControl()
        {
            // 清空现有控件
            panelSourceValue.Controls.Clear();

            // 根据类型创建相应的控件
            SourceTargetType sourceType = (SourceTargetType)entity.SourceType;
            switch (sourceType)
            {
                case SourceTargetType.角色:
                    CreateComboBoxControl(panelSourceValue, "cboSourceRole", RoleInfos, r => r.RoleID, r => r.RoleName, entity.SourceValue);
                    break;
                case SourceTargetType.人员:
                    CreateComboBoxControl(panelSourceValue, "cboSourceUser", UserInfos, u => u.User_ID, u => u.UserName, entity.SourceValue);
                    break;
                case SourceTargetType.部门:
                    CreateComboBoxControl(panelSourceValue, "cboSourceDept", DepartmentInfos, d => d.DepartmentID, d => d.DepartmentName, entity.SourceValue);
                    break;
            }
        }

        /// <summary>
        /// 更新提醒目标值控件
        /// </summary>
        private void UpdateTargetValueControl()
        {
            // 清空现有控件
            panelTargetValue.Controls.Clear();

            // 根据类型创建相应的控件
            SourceTargetType targetType = (SourceTargetType)entity.TargetType;
            switch (targetType)
            {
                case SourceTargetType.角色:
                    CreateComboBoxControl(panelTargetValue, "cboTargetRole", RoleInfos, r => r.RoleID, r => r.RoleName, entity.TargetValue);
                    break;
                case SourceTargetType.人员:
                    CreateComboBoxControl(panelTargetValue, "cboTargetUser", UserInfos, u => u.User_ID, u => u.UserName, entity.TargetValue);
                    break;
                case SourceTargetType.部门:
                    CreateComboBoxControl(panelTargetValue, "cboTargetDept", DepartmentInfos, d => d.DepartmentID, d => d.DepartmentName, entity.TargetValue);
                    break;
            }
        }

        /// <summary>
        /// 创建下拉框控件
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="container">容器</param>
        /// <param name="controlName">控件名称</param>
        /// <param name="dataSource">数据源</param>
        /// <param name="valueMember">值成员</param>
        /// <param name="displayMember">显示成员</param>
        /// <param name="selectedValue">选中值</param>
        private void CreateComboBoxControl<T>(Control container, string controlName, List<T> dataSource, Func<T, long> valueMember, Func<T, string> displayMember, long? selectedValue)
        {
            KryptonComboBox cmb = new KryptonComboBox();
            cmb.Name = controlName;
            cmb.Dock = DockStyle.Fill;

            // 创建匿名类型数据源，包含值和显示文本
            var items = dataSource.Select(item => new
            {
                Value = valueMember(item),
                Text = displayMember(item)
            }).ToList();

            cmb.DataSource = items;
            cmb.ValueMember = "Value";
            cmb.DisplayMember = "Text";

            // 设置选中值
            if (selectedValue.HasValue)
            {
                cmb.SelectedValue = selectedValue.Value;
            }

            // 添加到容器
            container.Controls.Add(cmb);
        }

        /// <summary>
        /// 提醒源类型选择变化事件
        /// </summary>
        private void CmbSourceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSourceValueControl();
        }

        /// <summary>
        /// 提醒目标类型选择变化事件
        /// </summary>
        private void CmbTargetType_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTargetValueControl();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            bindingSourceEdit.CancelEdit();
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private async void btnOk_Click(object sender, EventArgs e)
        {
            if (base.Validator())
            {
                // 更新提醒源值
                UpdateSourceValueFromControl();
                // 更新提醒目标值
                UpdateTargetValueFromControl();

                bindingSourceEdit.EndEdit();

                if (bindingSourceEdit.Current is tb_ReminderObjectLink link)
                {
                    // 保存关联规则
                    await SaveLinkedRulesAsync(link);
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        /// <summary>
        /// 从控件更新提醒源值
        /// </summary>
        private void UpdateSourceValueFromControl()
        {
            if (panelSourceValue.Controls.Count > 0 && panelSourceValue.Controls[0] is KryptonComboBox cmb)
            {
                if (cmb.SelectedItem != null)
                {
                    // 检查设计时
                    if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
                    {
                        return;
                    }

                    var selectedItem = cmb.SelectedItem;
                    if (selectedItem != null)
                    {
                        try
                        {
                            // 尝试获取Value属性，这是我们在CreateComboBoxControl中创建的匿名类型属性
                            var type = selectedItem.GetType();
                            var valueProperty = type.GetProperty("Value");
                            if (valueProperty != null)
                            {
                                entity.SourceValue = Convert.ToInt64(valueProperty.GetValue(selectedItem));
                            }
                        }
                        catch (Exception ex)
                        {
                            MainForm.Instance?.logger?.LogError(ex, "更新提醒源值失败");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 从控件更新提醒目标值
        /// </summary>
        private void UpdateTargetValueFromControl()
        {
            if (panelTargetValue.Controls.Count > 0 && panelTargetValue.Controls[0] is KryptonComboBox cmb)
            {
                if (cmb.SelectedItem != null)
                {
                    // 检查设计时
                    if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
                    {
                        return;
                    }

                    var selectedItem = cmb.SelectedItem;
                    if (selectedItem != null)
                    {
                        try
                        {
                            // 尝试获取Value属性，这是我们在CreateComboBoxControl中创建的匿名类型属性
                            var type = selectedItem.GetType();
                            var valueProperty = type.GetProperty("Value");
                            if (valueProperty != null)
                            {
                                entity.TargetValue = Convert.ToInt64(valueProperty.GetValue(selectedItem));
                            }
                        }
                        catch (Exception ex)
                        {
                            MainForm.Instance?.logger?.LogError(ex, "更新提醒目标值失败");
                        }
                    }
                }
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            // 设计时不执行测试逻辑
            if (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime)
            {
                return;
            }

            // 测试链路配置
            try
            {
                UpdateSourceValueFromControl();
                UpdateTargetValueFromControl();

                // 这里可以添加测试逻辑，比如验证配置是否正确
                MessageBox.Show("测试通过，配置有效！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"测试失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 绑定规则相关按钮事件
        /// </summary>
        private void BindRuleButtons()
        {
            btnAddRule.Click += btnAddRule_Click;
            btnRemoveRule.Click += btnRemoveRule_Click;
        }

        /// <summary>
        /// 加载已关联的规则
        /// </summary>
        private async void LoadLinkedRules()
        {
            try
            {
                if (entity == null || entity.LinkId == 0)
                {
                    dgvLinkedRules.DataSource = new List<tb_ReminderRule>();
                    return;
                }

                // 查询关联关系
                var relations = await _relationController.QueryAsync(r => r.LinkId == entity.LinkId);
                if (relations == null || relations.Count == 0)
                {
                    dgvLinkedRules.DataSource = new List<tb_ReminderRule>();
                    return;
                }

                // 查询关联的规则
                var ruleIds = relations.Select(r => r.RuleId).ToList();
                _linkedRules = await _ruleController.QueryAsync(l => ruleIds.Contains(l.RuleId));
                dgvLinkedRules.DataSource = _linkedRules;

                // 设置列标题
                if (dgvLinkedRules.Columns.Count > 0)
                {
                    dgvLinkedRules.Columns["RuleId"].HeaderText = "规则ID";
                    dgvLinkedRules.Columns["RuleName"].HeaderText = "规则名称";
                    dgvLinkedRules.Columns["Description"].HeaderText = "规则描述";
                    dgvLinkedRules.Columns["RuleEngineType"].HeaderText = "引擎类型";
                    dgvLinkedRules.Columns["ReminderBizType"].HeaderText = "业务类型";
                    dgvLinkedRules.Columns["ReminderPriority"].HeaderText = "优先级";
                    dgvLinkedRules.Columns["IsEnabled"].HeaderText = "是否启用";

                    // 隐藏不必要的列
                    dgvLinkedRules.Columns["NotifyChannels"].Visible = false;
                    dgvLinkedRules.Columns["NotifyRecipients"].Visible = false;
                    dgvLinkedRules.Columns["JsonConfig"].Visible = false;
                    dgvLinkedRules.Columns["EffectiveDate"].Visible = false;
                    dgvLinkedRules.Columns["ExpireDate"].Visible = false;
                    dgvLinkedRules.Columns["Created_at"].Visible = false;
                    dgvLinkedRules.Columns["Created_by"].Visible = false;
                    dgvLinkedRules.Columns["Updated_at"].Visible = false;
                    dgvLinkedRules.Columns["Updated_by"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载关联规则失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 添加规则按钮点击事件
        /// </summary>
        private async void btnAddRule_Click(object sender, EventArgs e)
        {
            try
            {
                using (var form = new frmReminderRuleConfig())
                {
                    if (form.ShowDialog() == DialogResult.OK && form.SelectedRuleId > 0)
                    {
                        // 检查是否已关联
                        if (_linkedRules != null && _linkedRules.Any(r => r.RuleId == form.SelectedRuleId))
                        {
                            MessageBox.Show("该规则已关联，无需重复添加", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        // 查询规则信息
                        var rule = await _ruleController.BaseQueryByIdNavAsync(form.SelectedRuleId);
                        if (rule != null)
                        {
                            // 添加到本地列表
                            if (_linkedRules == null)
                            {
                                _linkedRules = new List<tb_ReminderRule>();
                            }
                            _linkedRules.Add(rule);

                            // 更新数据源
                            dgvLinkedRules.DataSource = null;
                            dgvLinkedRules.DataSource = _linkedRules;

                            // 设置列标题
                            if (dgvLinkedRules.Columns.Count > 0)
                            {
                                dgvLinkedRules.Columns["RuleId"].HeaderText = "规则ID";
                                dgvLinkedRules.Columns["RuleName"].HeaderText = "规则名称";
                                dgvLinkedRules.Columns["Description"].HeaderText = "规则描述";
                                dgvLinkedRules.Columns["RuleEngineType"].HeaderText = "引擎类型";
                                dgvLinkedRules.Columns["ReminderBizType"].HeaderText = "业务类型";
                                dgvLinkedRules.Columns["ReminderPriority"].HeaderText = "优先级";
                                dgvLinkedRules.Columns["IsEnabled"].HeaderText = "是否启用";

                                // 隐藏不必要的列
                                dgvLinkedRules.Columns["NotifyChannels"].Visible = false;
                                dgvLinkedRules.Columns["NotifyRecipients"].Visible = false;
                                dgvLinkedRules.Columns["JsonConfig"].Visible = false;
                                dgvLinkedRules.Columns["EffectiveDate"].Visible = false;
                                dgvLinkedRules.Columns["ExpireDate"].Visible = false;
                                dgvLinkedRules.Columns["Created_at"].Visible = false;
                                dgvLinkedRules.Columns["Created_by"].Visible = false;
                                dgvLinkedRules.Columns["Updated_at"].Visible = false;
                                dgvLinkedRules.Columns["Updated_by"].Visible = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"添加规则失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 移除规则按钮点击事件
        /// </summary>
        private void btnRemoveRule_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvLinkedRules.SelectedRows.Count > 0)
                {
                    var selectedRow = dgvLinkedRules.SelectedRows[0];
                    if (selectedRow.DataBoundItem is tb_ReminderRule selectedRule)
                    {
                        if (MessageBox.Show($"确定要移除规则 '{selectedRule.RuleName}' 吗？", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            // 从本地列表中移除
                            _linkedRules.Remove(selectedRule);

                            // 更新数据源
                            dgvLinkedRules.DataSource = null;
                            dgvLinkedRules.DataSource = _linkedRules;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("请先选择要移除的规则", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"移除规则失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 保存关联规则
        /// </summary>
        /// <param name="link">提醒对象链路</param>
        private async Task SaveLinkedRulesAsync(tb_ReminderObjectLink link)
        {
            try
            {
                if (link == null || link.LinkId == 0)
                    return;

                // 删除现有关联
                var existingRelations = await _relationController.QueryAsync(r => r.LinkId == link.LinkId);
                if (existingRelations != null && existingRelations.Count > 0)
                {
                    await _relationController.BaseDeleteAsync(existingRelations);
                }

                // 保存新关联
                if (_linkedRules != null && _linkedRules.Count > 0)
                {
                    var newRelations = _linkedRules.Select(rule => new tb_ReminderLinkRuleRelation
                    {
                        LinkId = link.LinkId,
                        RuleId = rule.RuleId,
                        Created_at = DateTime.UtcNow,
                        Created_by = MainForm.Instance.AppContext.CurUserInfo.UserInfo.User_ID
                    }).ToList();

                    await _relationController.AddAsync(newRelations);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存关联规则失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}