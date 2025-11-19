/**
 * 文件: UnifiedStatusUIControllerV3.cs
 * 版本: V3 - 统一状态UI控制器实现
 * 说明: 统一状态UI控制器实现 - v3版本，负责根据状态上下文更新UI控件状态
 * 创建日期: 2024年
 * 作者: RUINOR ERP开发团队
 * 
 * 版本标识：
 * V3: 支持数据状态、操作状态和业务状态的UI控制
 * 公共代码: UI层状态管理控制器，所有版本通用
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using RUINORERP.Model.Base.StatusManager;
using Microsoft.Extensions.Logging;

namespace RUINORERP.UI.StateManagement.UI
{
    /// <summary>
    /// 统一状态UI控制器实现 - v3版本
    /// 负责根据状态上下文更新UI控件状态
    /// </summary>
    public class UnifiedStatusUIControllerV3 : IStatusUIController
    {
        #region 字段

        /// <summary>
        /// 日志记录器
        /// </summary>
        private readonly ILogger<UnifiedStatusUIControllerV3> _logger;

        /// <summary>
        /// 状态管理器
        /// </summary>
        private readonly IUnifiedStateManager _stateManager;

        /// <summary>
        /// UI状态规则字典
        /// </summary>
        private readonly Dictionary<string, IUIStatusRule> _uiRules;

        /// <summary>
        /// 状态操作规则配置
        /// </summary>
        private readonly IStateRuleConfiguration _actionRuleConfiguration;

        #endregion

        #region 构造函数

        /// <summary>
        /// 初始化统一状态UI控制器
        /// </summary>
        /// <param name="stateManager">状态管理器</param>
        /// <param name="actionRuleConfiguration">状态操作规则配置</param>
        /// <param name="logger">日志记录器</param>
        public UnifiedStatusUIControllerV3(IUnifiedStateManager stateManager, IStateRuleConfiguration actionRuleConfiguration, ILogger<UnifiedStatusUIControllerV3> logger = null)
        {
            _stateManager = stateManager ?? throw new ArgumentNullException(nameof(stateManager));
            _actionRuleConfiguration = actionRuleConfiguration ?? throw new ArgumentNullException(nameof(actionRuleConfiguration));
            _logger = logger;
            _uiRules = new Dictionary<string, IUIStatusRule>();

            // 注册默认规则
            RegisterDefaultRules();
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 根据状态上下文更新UI状态
        /// </summary>
        /// <param name="statusContext">状态上下文</param>
        /// <param name="controls">控件集合</param>
        public void UpdateUIStatus(IStatusTransitionContext statusContext, IEnumerable<Control> controls)
        {
            if (statusContext == null || controls == null)
                return;

            try
            {
                // 获取当前状态
                var dataStatus = statusContext.GetDataStatus();
                var businessStatus = statusContext.GetBusinessStatus(statusContext.StatusType);
                var actionStatus = statusContext.GetActionStatus();

                // 更新数据状态相关的UI
                if (dataStatus != null)
                {
                    UpdateUIForDataStatus(dataStatus, controls);
                }

                // 更新业务状态相关的UI
                if (businessStatus != null)
                {
                    UpdateUIForBusinessStatus((Enum)businessStatus, controls);
                }

                // 更新操作状态相关的UI
                if (actionStatus != null)
                {
                    UpdateUIForActionStatus(actionStatus, controls);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "更新UI状态失败");
            }
        }

        /// <summary>
        /// 根据状态更新单个控件状态
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="status">状态</param>
        /// <param name="statusType">状态类型</param>
        public void UpdateControlStatus(Control control, Enum status, string statusType)
        {
            if (control == null || status == null)
                return;

            try
            {
                // 创建UI状态上下文
                var context = new UIStatusContext(status, statusType, new[] { control }, _stateManager);

                // 查找并应用规则
                var ruleKey = $"{statusType}_{status}";
                if (_uiRules.ContainsKey(ruleKey))
                {
                    var rule = _uiRules[ruleKey];
                    rule.Apply(context);
                }
                else
                {
                    // 应用默认规则
                    ApplyDefaultControlRule(control, status, statusType);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "更新控件状态失败: {ControlName}, {Status}, {StatusType}", 
                    control.Name, status, statusType);
            }
        }

        /// <summary>
        /// 注册UI状态规则
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="rule">规则</param>
        public void RegisterUIStatusRule(Enum status, IUIStatusRule rule)
        {
            if (status == null || rule == null)
                return;

            var statusType = GetStateTypeName(status);
            var ruleKey = $"{statusType}_{status}";
            _uiRules[ruleKey] = rule;
        }

        /// <summary>
        /// 应用状态规则
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="controls">控件集合</param>
        public void ApplyRules(Enum status, IEnumerable<Control> controls)
        {
            if (status == null || controls == null)
                return;

            try
            {
                var statusType = GetStateTypeName(status);
                var ruleKey = $"{statusType}_{status}";

                if (_uiRules.ContainsKey(ruleKey))
                {
                    var rule = _uiRules[ruleKey];
                    var context = new UIStatusContext(status, statusType, controls, _stateManager);
                    rule.Apply(context);
                }
                else
                {
                    // 应用默认规则
                    foreach (var control in controls)
                    {
                        ApplyDefaultControlRule(control, status, statusType);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "应用状态规则失败: {Status}", status);
            }
        }

        /// <summary>
        /// 检查操作是否可执行
        /// </summary>
        /// <param name="action">操作类型</param>
        /// <param name="statusContext">状态上下文</param>
        /// <returns>是否可执行</returns>
        public bool CanExecuteAction(Enum action, IStatusTransitionContext statusContext)
        {
            if (action == null || statusContext == null)
                return false;

            try
            {
                // 获取当前状态
                var dataStatus = statusContext.GetDataStatus();
                var businessStatus = statusContext.GetBusinessStatus(statusContext.StatusType);

                // 检查数据状态下的操作权限
                if (dataStatus != null)
                {
                    if (_actionRuleConfiguration.IsActionAllowed(dataStatus, action.ToString()))
                        return true;
                }

                // 检查业务状态下的操作权限
                if (businessStatus != null)
                {
                    if (_actionRuleConfiguration.IsActionAllowed(businessStatus as Enum, action.ToString()))
                        return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "检查操作可执行性失败: {Action}", action);
                return false;
            }
        }

        /// <summary>
        /// 获取当前状态下可执行的操作列表
        /// </summary>
        /// <param name="statusContext">状态上下文</param>
        /// <returns>可执行的操作列表</returns>
        public IEnumerable<Enum> GetAvailableActions(IStatusTransitionContext statusContext)
        {
            if (statusContext == null)
                return Enumerable.Empty<Enum>();

            try
            {
                var allActions = new List<Enum>();

                // 获取当前状态
                var dataStatus = statusContext.GetDataStatus();
                var businessStatus = statusContext.GetBusinessStatus(statusContext.StatusType);

                // 获取数据状态下的可执行操作
                if (dataStatus != null)
                {
                    allActions.AddRange(_actionRuleConfiguration.GetAllowedActions(dataStatus));
                }

                // 获取业务状态下的可执行操作
                if (businessStatus != null)
                {
                    allActions.AddRange(_actionRuleConfiguration.GetAllowedActions(businessStatus as Enum));
                }

                // 去重并返回
                return allActions.Distinct();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "获取可执行操作列表失败");
                return Enumerable.Empty<Enum>();
            }
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 根据数据状态更新UI
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="controls">控件集合</param>
        private void UpdateUIForDataStatus(Enum status, IEnumerable<Control> controls)
        {
            // 应用状态规则
            ApplyRules(status, controls);
        }

        /// <summary>
        /// 根据业务状态更新UI
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="controls">控件集合</param>
        private void UpdateUIForBusinessStatus(Enum status, IEnumerable<Control> controls)
        {
            // 应用状态规则
            ApplyRules(status, controls);
        }

        /// <summary>
        /// 根据操作状态更新UI
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="controls">控件集合</param>
        private void UpdateUIForActionStatus(Enum status, IEnumerable<Control> controls)
        {
            // 应用状态规则
            ApplyRules(status, controls);
        }

        /// <summary>
        /// 应用默认控件规则
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="status">状态</param>
        /// <param name="statusType">状态类型</param>
        private void ApplyDefaultControlRule(Control control, Enum status, string statusType)
        {
            // 根据控件类型和状态应用默认规则
            switch (statusType)
            {
                case "DataStatus":
                    ApplyDefaultDataStatusRule(control, status);
                    break;
                case "BusinessStatus":
                    ApplyDefaultBusinessStatusRule(control, status);
                    break;
                case "ActionStatus":
                    ApplyDefaultActionStatusRule(control, status);
                    break;
            }
        }

        /// <summary>
        /// 应用默认数据状态规则
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="status">状态</param>
        private void ApplyDefaultDataStatusRule(Control control, Enum status)
        {
            // 根据控件名称和类型应用默认规则
            bool isEditable = true;
            bool isVisible = true;

            // 根据状态确定控件是否可编辑
            if (status.ToString().Contains("确认") || status.ToString().Contains("完结") || status.ToString().Contains("审核"))
            {
                isEditable = false;
            }

            // 应用规则
            ApplyControlState(control, isEditable, isVisible);
        }

        /// <summary>
        /// 应用默认业务状态规则
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="status">状态</param>
        private void ApplyDefaultBusinessStatusRule(Control control, Enum status)
        {
            // 业务状态默认规则
            bool isEditable = true;
            bool isVisible = true;

            // 根据状态确定控件是否可编辑
            if (status.ToString().Contains("已关闭") || status.ToString().Contains("已完成"))
            {
                isEditable = false;
            }

            // 应用规则
            ApplyControlState(control, isEditable, isVisible);
        }

        /// <summary>
        /// 应用默认操作状态规则
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="status">状态</param>
        private void ApplyDefaultActionStatusRule(Control control, Enum status)
        {
            // 操作状态默认规则
            bool isEditable = true;
            bool isVisible = true;

            // 根据状态确定控件是否可编辑
            if (status.ToString().Equals("删除"))
            {
                isVisible = false;
            }

            // 应用规则
            ApplyControlState(control, isEditable, isVisible);
        }

        /// <summary>
        /// 应用控件状态
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="isEditable">是否可编辑</param>
        /// <param name="isVisible">是否可见</param>
        private void ApplyControlState(Control control, bool isEditable, bool isVisible)
        {
            // 应用新状态
            control.Enabled = isEditable;
            control.Visible = isVisible;
            SetControlReadOnly(control, !isEditable);
        }

        /// <summary>
        /// 设置控件只读状态
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="readOnly">是否只读</param>
        private void SetControlReadOnly(Control control, bool readOnly)
        {
            if (control is TextBox textBox)
                textBox.ReadOnly = readOnly;
            else if (control is ComboBox comboBox)
                comboBox.Enabled = !readOnly;
            else if (control is NumericUpDown numericUpDown)
                numericUpDown.ReadOnly = readOnly;
            else if (control is DataGridView dataGridView)
                dataGridView.ReadOnly = readOnly;
        }

        /// <summary>
        /// 获取状态类型名称
        /// </summary>
        /// <param name="status">状态</param>
        /// <returns>状态类型名称</returns>
        private string GetStateTypeName(Enum status)
        {
            var statusTypeName = status.GetType().Name;
            
            if (statusTypeName.Contains("DataStatus"))
                return "DataStatus";
            else if (statusTypeName.Contains("BusinessStatus"))
                return "BusinessStatus";
            else if (statusTypeName.Contains("ActionStatus"))
                return "ActionStatus";
            else
                return "DataStatus"; // 默认为数据状态
        }

        /// <summary>
        /// 注册默认规则
        /// </summary>
        private void RegisterDefaultRules()
        {
            // 这里可以注册一些默认的UI状态规则
            // 例如：草稿状态下的控件规则、确认状态下的控件规则等
        }

        #endregion
    }

    /// <summary>
    /// UI状态上下文实现
    /// </summary>
    internal class UIStatusContext : IUIStatusContext
    {
        #region 属性

        /// <summary>
        /// 状态
        /// </summary>
        public Enum Status { get; }

        /// <summary>
        /// 状态类型
        /// </summary>
        public string StatusType { get; }

        /// <summary>
        /// 控件集合
        /// </summary>
        public IEnumerable<Control> Controls { get; }

        /// <summary>
        /// 状态管理器
        /// </summary>
        public IUnifiedStateManager StateManager { get; }

        #endregion

        #region 构造函数

        /// <summary>
        /// 初始化UI状态上下文
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="statusType">状态类型</param>
        /// <param name="controls">控件集合</param>
        /// <param name="stateManager">状态管理器</param>
        public UIStatusContext(Enum status, string statusType, IEnumerable<Control> controls, IUnifiedStateManager stateManager)
        {
            Status = status;
            StatusType = statusType;
            Controls = controls;
            StateManager = stateManager;
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 获取控件状态
        /// </summary>
        /// <param name="controlName">控件名称</param>
        /// <returns>控件状态</returns>
        public ControlState GetControlState(string controlName)
        {
            // 简化实现：返回默认状态
            return new ControlState();
        }

        /// <summary>
        /// 设置控件状态
        /// </summary>
        /// <param name="controlName">控件名称</param>
        /// <param name="state">控件状态</param>
        public void SetControlState(string controlName, ControlState state)
        {
            // 简化实现：不保存状态
        }

        #endregion
    }
}