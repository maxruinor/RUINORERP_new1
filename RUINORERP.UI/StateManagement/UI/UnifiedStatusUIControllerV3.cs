/**
 * 文件: UnifiedStatusUIControllerV3.cs
 * 版本: V3.2 - 统一状态UI控制器增强版
 * 说明: 统一状态UI控制器实现 - v3.2版本，负责根据状态上下文更新UI控件状态
 * 创建日期: 2024年
 * 作者: RUINOR ERP开发团队
 * 
 * 版本标识：
 * V3: 支持数据状态、操作状态和业务状态的UI控制
 * V3.1: 增强版，添加缓存机制、完善控件状态处理、增强自定义规则支持
 * V3.2: 增强版，移除硬编码状态判断、完善控件可见性控制、提供配置化状态规则
 * 公共代码: UI层状态管理控制器，所有版本通用
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using RUINORERP.Model.Base.StatusManager;
using Microsoft.Extensions.Logging;
using static RUINORERP.UI.StateManagement.UI.UnifiedStatusUIControllerV3;
using RUINORERP.Model;
using RUINORERP.Global;

namespace RUINORERP.UI.StateManagement.UI
{
    /// <summary>
    /// 统一状态UI控制器实现 - v3.2版本
    /// 负责根据状态上下文更新UI控件状态，支持更灵活的控件可见性控制和配置化规则
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

        // 移除不存在的UIStatusControlOptions依赖

        /// <summary>
        /// 控件状态缓存 - 优化性能，避免重复计算
        /// </summary>
        private readonly Dictionary<string, Dictionary<Control, ControlStateInfo>> _controlStatusCache;

        /// <summary>
        /// 自定义控件状态处理委托字典
        /// </summary>
        private readonly Dictionary<Type, Action<Control, ControlStateInfo>> _customControlHandlers;

        /// <summary>
        /// 状态类型识别缓存
        /// </summary>
        private readonly Dictionary<Type, string> _statusTypeCache = new Dictionary<Type, string>();

        #endregion

        #region 内部类型定义

        /// <summary>
        /// 控件状态信息类，包含启用状态和可见性状态
        /// </summary>
        public class ControlStateInfo
        {
            /// <summary>
            /// 控件是否启用
            /// </summary>
            public bool Enabled { get; set; }

            /// <summary>
            /// 控件是否可见
            /// </summary>
            public bool Visible { get; set; }

            /// <summary>
            /// 控件状态信息的默认构造函数
            /// </summary>
            public ControlStateInfo() : this(true, true) { }

            /// <summary>
            /// 控件状态信息的构造函数
            /// </summary>
            /// <param name="enabled">是否启用</param>
            /// <param name="visible">是否可见</param>
            public ControlStateInfo(bool enabled, bool visible)
            {
                Enabled = enabled;
                Visible = visible;
            }

            /// <summary>
            /// 转换为字符串表示
            /// </summary>
            /// <returns>状态信息字符串</returns>
            public override string ToString()
            {
                return $"{{Enabled={Enabled}, Visible={Visible}}}";
            }
        }

        #endregion

        #region 构造函数

        /// <summary>
        /// 初始化统一状态UI控制器
        /// </summary>
        /// <param name="stateManager">状态管理器</param>
        /// <param name="actionRuleConfiguration">状态操作规则配置</param>
        /// <param name="logger">日志记录器</param>
        /// <summary>
        /// 初始化统一状态UI控制器
        /// </summary>
        /// <param name="stateManager">状态管理器</param>
        /// <param name="actionRuleConfiguration">状态操作规则配置</param>
        /// <param name="logger">日志记录器</param>
        public UnifiedStatusUIControllerV3(IUnifiedStateManager stateManager,
            IStateRuleConfiguration actionRuleConfiguration,
            ILogger<UnifiedStatusUIControllerV3> logger = null)
        {
            _stateManager = stateManager ?? throw new ArgumentNullException(nameof(stateManager));
            _actionRuleConfiguration = actionRuleConfiguration ?? throw new ArgumentNullException(nameof(actionRuleConfiguration));
            _logger = logger;
            _uiRules = new Dictionary<string, IUIStatusRule>();
            _controlStatusCache = new Dictionary<string, Dictionary<Control, ControlStateInfo>>();
            _customControlHandlers = new Dictionary<Type, Action<Control, ControlStateInfo>>();

            // 注册默认规则
            RegisterDefaultRules();

            // 注册默认控件处理器
            RegisterDefaultControlHandlers();
        }


        #endregion

        #region 公共方法

        /// <summary>
        /// 根据状态上下文更新UI状态
        /// </summary>
        /// <param name="statusContext">状态上下文</param>
        /// <param name="controls">控件集合</param>
        /// <param name="applyVisibility">是否应用可见性控制，默认为true</param>
        public void UpdateUIStatus(IStatusTransitionContext statusContext, IEnumerable<Control> controls, bool applyVisibility = true)
        {
            if (statusContext == null)
            {
                _logger?.LogWarning("UpdateUIStatus: 状态上下文为空，跳过UI更新");
                return;
            }
            if (controls == null)
            {
                _logger?.LogWarning("UpdateUIStatus: 控件集合为空，跳过UI更新");
                return;
            }

            try
            {
                // 获取当前状态
                var dataStatus = statusContext.GetDataStatus();
                var businessStatus = statusContext.GetBusinessStatus(statusContext.StatusType);
                var actionStatus = statusContext.GetActionStatus();

                _logger?.LogDebug("UpdateUIStatus: 开始更新UI状态，数据状态={DataStatus}, 业务状态={BusinessStatus}, 操作状态={ActionStatus}, 应用可见性={ApplyVisibility}",
                    dataStatus, businessStatus, actionStatus, applyVisibility);

                // 按优先级顺序更新：业务状态 > 数据状态 > 操作状态
                // 注意：状态优先级设计是为了避免状态冲突
                if (businessStatus != null)
                {
                    UpdateUIForStatus((Enum)businessStatus, controls, applyVisibility);
                }
                else if (dataStatus != null)
                {
                    UpdateUIForStatus(dataStatus, controls, applyVisibility);
                }
                else if (actionStatus != null)
                {
                    UpdateUIForStatus(actionStatus, controls, applyVisibility);
                }
                else
                {
                    _logger?.LogWarning("UpdateUIStatus: 未找到任何有效状态，无法更新UI");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "更新UI状态失败: {Message}", ex.Message);
                // 错误发生时，不抛出异常，避免影响UI交互
            }
        }

        /// <summary>
        /// 根据状态上下文更新UI状态（兼容旧版本）
        /// </summary>
        /// <param name="statusContext">状态上下文</param>
        /// <param name="controls">控件集合</param>
        public void UpdateUIStatus(IStatusTransitionContext statusContext, IEnumerable<Control> controls)
        {
            UpdateUIStatus(statusContext, controls, true);
        }

        /// <summary>
        /// 根据状态更新单个控件状态
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="status">状态</param>
        /// <param name="statusType">状态类型</param>
        /// <param name="applyVisibility">是否应用可见性控制，默认为true</param>
        public void UpdateControlStatus(Control control, Enum status, string statusType, bool applyVisibility = true)
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
                    ApplyDefaultControlRule(control, status, statusType, applyVisibility);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "更新控件状态失败: {ControlName}, {Status}, {StatusType}, 应用可见性={ApplyVisibility}",
                    control.Name, status, statusType, applyVisibility);
            }
        }

        /// <summary>
        /// 根据状态更新单个控件状态（兼容旧版本）
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="status">状态</param>
        /// <param name="statusType">状态类型</param>
        public void UpdateControlStatus(Control control, Enum status, string statusType)
        {
            UpdateControlStatus(control, status, statusType, true);
        }

        /// <summary>
        /// 注册UI状态规则
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="rule">规则</param>
        /// <exception cref="ArgumentNullException">当status或rule为null时抛出</exception>
        public void RegisterUIStatusRule(Enum status, IUIStatusRule rule)
        {
            if (status == null)
                throw new ArgumentNullException(nameof(status));
            if (rule == null)
                throw new ArgumentNullException(nameof(rule));

            var statusType = GetStateTypeName(status);
            var ruleKey = $"{statusType}_{status}";
            _uiRules[ruleKey] = rule;

            // 注册规则后清除相关缓存
            InvalidateCacheForStatus(status);

            _logger?.LogDebug("成功注册UI状态规则: {RuleKey}", ruleKey);
        }

        /// <summary>
        /// 注册自定义控件状态处理器
        /// </summary>
        /// <typeparam name="TControl">控件类型</typeparam>
        /// <param name="handler">控件状态处理委托</param>
        public void RegisterCustomControlHandler<TControl>(Action<TControl, ControlStateInfo> handler) where TControl : Control
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));

            Type controlType = typeof(TControl);
            _customControlHandlers[controlType] = (control, stateInfo) =>
                handler((TControl)control, stateInfo);

            _logger?.LogDebug("成功注册自定义控件处理器: {ControlType}", controlType.Name);
        }

        /// <summary>
        /// 应用状态规则
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="controls">控件集合</param>
        /// <param name="applyVisibility">是否应用可见性控制</param>
        public void ApplyRules(Enum status, IEnumerable<Control> controls, bool applyVisibility = true)
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
                        ApplyDefaultControlRule(control, status, statusType, applyVisibility);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "应用状态规则失败: {Status}, 应用可见性={ApplyVisibility}", status, applyVisibility);
            }
        }

        /// <summary>
        /// 应用状态规则（兼容旧版本）
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="controls">控件集合</param>
        public void ApplyRules(Enum status, IEnumerable<Control> controls)
        {
            ApplyRules(status, controls, true);
        }

        /// <summary>
        /// 检查操作是否可执行
        /// </summary>
        /// <param name="action">操作类型</param>
        /// <param name="statusContext">状态上下文</param>
        /// <returns>是否可执行</returns>
        public bool CanExecuteAction(Enum action, IStatusTransitionContext statusContext)
        {
            // 调用带有消息的版本，只返回布尔结果
            return CanExecuteActionWithMessage(action, statusContext).IsSuccess;
        }

        /// <summary>
        /// 检查操作是否可执行，并返回详细的提示消息
        /// </summary>
        /// <param name="action">操作类型</param>
        /// <param name="statusContext">状态上下文</param>
        /// <returns>包含是否可执行和提示消息的结果对象</returns>
        public RUINORERP.Model.Base.StatusManager.StateTransitionResult CanExecuteActionWithMessage(Enum action, IStatusTransitionContext statusContext)
        {
            if (action == null || statusContext == null)
                return RUINORERP.Model.Base.StatusManager.StateTransitionResult.Failure("操作或状态上下文无效");

            try
            {
                // 获取当前状态
                var dataStatus = statusContext.GetDataStatus();
                var businessStatus = statusContext.GetBusinessStatus(statusContext.StatusType);

                // 检查数据状态下的操作权限
                if (dataStatus != null)
                {
                    if (_actionRuleConfiguration.IsActionAllowed(dataStatus, action.ToString()))
                        return RUINORERP.Model.Base.StatusManager.StateTransitionResult.Success($"数据状态为{dataStatus}时允许执行{action}操作");
                    else
                        return RUINORERP.Model.Base.StatusManager.StateTransitionResult.Failure($"数据状态为{dataStatus}时不允许执行{action}操作");
                }

                // 检查业务状态下的操作权限
                if (businessStatus != null)
                {
                    if (_actionRuleConfiguration.IsActionAllowed(businessStatus as Enum, action.ToString()))
                        return RUINORERP.Model.Base.StatusManager.StateTransitionResult.Success($"业务状态为{businessStatus}时允许执行{action}操作");
                    else
                        return RUINORERP.Model.Base.StatusManager.StateTransitionResult.Failure($"业务状态为{businessStatus}时不允许执行{action}操作");
                }

                return RUINORERP.Model.Base.StatusManager.StateTransitionResult.Failure("无法确定当前状态，不允许执行操作");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "检查操作可执行性失败: {Action}", action);
                return RUINORERP.Model.Base.StatusManager.StateTransitionResult.Failure($"检查操作权限时发生错误: {ex.Message}");
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
        /// 根据状态更新UI（通用方法）
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="controls">控件集合</param>
        /// <param name="applyVisibility">是否应用可见性控制</param>
        private void UpdateUIForStatus(Enum status, IEnumerable<Control> controls, bool applyVisibility)
        {
            // 应用状态规则
            ApplyRules(status, controls, applyVisibility);
        }

        /// <summary>
        /// 应用默认控件规则
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="status">状态</param>
        /// <param name="statusType">状态类型</param>
        /// <param name="applyVisibility">是否应用可见性控制</param>
        private void ApplyDefaultControlRule(Control control, Enum status, string statusType, bool applyVisibility)
        {
            try
            {
                // 先尝试从配置获取规则
                ControlStateInfo stateInfo = ApplyConfiguredRule(control, status, statusType, applyVisibility);

                // 如果配置中没有相应规则或规则返回null，则应用默认规则
                if (stateInfo == null)
                {
                    // 根据状态类型应用不同的默认规则
                    switch (statusType)
                    {
                        case "DataStatus":
                            ApplyDefaultDataStatusRule(control, status, applyVisibility);
                            break;
                        case "BusinessStatus":
                            ApplyDefaultBusinessStatusRule(control, status, applyVisibility);
                            break;
                        case "ActionStatus":
                            ApplyDefaultActionStatusRule(control, status, applyVisibility);
                            break;
                        default:
                            _logger?.LogWarning("未知的状态类型: {StatusType}", statusType);
                            break;
                    }
                }
                else
                {
                    // 应用配置规则计算出的状态
                    ApplyControlState(control, stateInfo);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "应用默认控件规则时发生错误: {ControlName}, {Status}, {StatusType}", control.Name, status, statusType);
            }
        }

        /// <summary>
        /// 应用配置的规则
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="status">状态</param>
        /// <param name="statusType">状态类型</param>
        /// <param name="applyVisibility">是否应用可见性控制</param>
        /// <returns>控件状态信息，如果没有配置规则则返回null</returns>
        private ControlStateInfo ApplyConfiguredRule(Control control, Enum status, string statusType, bool applyVisibility)
        {
            // 配置功能已移除，直接返回null
            _logger?.LogDebug("配置规则功能已移除: 状态类型={StatusType}, 状态={Status}, 控件={Control}",
                statusType, status?.ToString(), control?.Name);
            return null;
        }

        /// <summary>
        /// 应用默认数据状态规则
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="status">状态</param>
        /// <param name="applyVisibility">是否应用可见性控制</param>
        private void ApplyDefaultDataStatusRule(Control control, Enum status, bool applyVisibility)
        {
            // 根据控件名称和类型应用默认规则
            bool isEditable = true;
            bool isVisible = true;

            // 使用默认只读状态关键词
            List<string> readonlyStateKeywords = new List<string> { "确认", "完结", "审核" };

            string statusName = status.ToString();

            // 根据状态确定控件是否可编辑
            if (readonlyStateKeywords.Any(keyword => statusName.Contains(keyword)))
            {
                isEditable = false;

                // 特殊按钮在已审核状态下隐藏
                if (applyVisibility && control is Button button)
                {
                    // 默认的按钮隐藏规则
                    List<string> buttonsToHide = new List<string> { "btnAdd", "btnEdit", "btnDelete" };

                    if (buttonsToHide.Any(prefix => button.Name.Contains(prefix)))
                    {
                        isVisible = false;
                    }
                }
            }

            // 应用规则
            ApplyControlState(control, new ControlStateInfo(isEditable, isVisible));
        }

        /// <summary>
        /// 应用默认业务状态规则
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="status">状态</param>
        /// <param name="applyVisibility">是否应用可见性控制</param>
        private void ApplyDefaultBusinessStatusRule(Control control, Enum status, bool applyVisibility)
        {
            // 业务状态默认规则
            bool isEditable = true;
            bool isVisible = true;

            // 使用默认最终状态关键词
            List<string> finalStateKeywords = new List<string> { "已关闭", "已完成" };

            string statusName = status.ToString();

            // 根据状态确定控件是否可编辑
            if (finalStateKeywords.Any(keyword => statusName.Contains(keyword)))
            {
                isEditable = false;

                // 特殊按钮在已完成状态下隐藏
                if (applyVisibility && control is Button button)
                {
                    // 默认的按钮隐藏规则
                    List<string> buttonsToHide = new List<string> { "btnConfirm", "btnComplete", "btnSubmit" };

                    if (buttonsToHide.Any(prefix => button.Name.Contains(prefix)))
                    {
                        isVisible = false;
                    }
                }
            }

            // 应用规则
            ApplyControlState(control, new ControlStateInfo(isEditable, isVisible));
        }

        /// <summary>
        /// 应用默认操作状态规则
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="status">状态</param>
        /// <param name="applyVisibility">是否应用可见性控制</param>
        private void ApplyDefaultActionStatusRule(Control control, Enum status, bool applyVisibility)
        {
            // 操作状态默认规则
            bool isEditable = true;
            bool isVisible = true;

            string statusName = status.ToString();

            // 使用默认隐藏状态列表
            List<string> hiddenStatuses = new List<string> { "删除" };

            // 根据状态确定控件是否可见
            if (hiddenStatuses.Contains(statusName) && applyVisibility)
            {
                isVisible = false;
            }

            // 应用规则
            ApplyControlState(control, new ControlStateInfo(isEditable, isVisible));
        }

        /// <summary>
        /// 应用控件状态
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="stateInfo">控件状态信息</param>
        /// <exception cref="ArgumentNullException">当control或stateInfo为null时抛出</exception>
        private void ApplyControlState(Control control, ControlStateInfo stateInfo)
        {
            if (control == null)
                throw new ArgumentNullException(nameof(control));
            if (stateInfo == null)
                throw new ArgumentNullException(nameof(stateInfo));

            try
            {
                // 检查是否有自定义处理器
                Type controlType = control.GetType();
                if (_customControlHandlers.TryGetValue(controlType, out var customHandler))
                {
                    // 使用自定义处理器
                    customHandler(control, stateInfo);
                    return;
                }

                // 应用基本状态
                control.Visible = stateInfo.Visible;

                // 检查是否是Krypton控件
                if (IsKryptonControl(control))
                {
                    ApplyKryptonControlState(control, stateInfo);
                    return;
                }

                // 根据控件类型应用不同的状态规则
                switch (control)
                {
                    case Button:
                        ((Button)control).Enabled = stateInfo.Enabled;
                        break;
                    case TextBox:
                        ((TextBox)control).ReadOnly = !stateInfo.Enabled;
                        // 即使只读也保持启用状态，允许复制文本
                        break;
                    case ComboBox:
                        ((ComboBox)control).DropDownStyle = stateInfo.Enabled ? ComboBoxStyle.DropDown : ComboBoxStyle.DropDownList;
                        ((ComboBox)control).Enabled = stateInfo.Enabled;
                        break;
                    case CheckBox:
                    case RadioButton:
                        control.Enabled = stateInfo.Enabled;
                        break;
                    case NumericUpDown:
                        ((NumericUpDown)control).ReadOnly = !stateInfo.Enabled;
                        ((NumericUpDown)control).Enabled = stateInfo.Enabled;
                        break;
                    case DataGridView:
                        ((DataGridView)control).ReadOnly = !stateInfo.Enabled;
                        // 特殊处理：禁用编辑时保留滚动功能
                        foreach (DataGridViewColumn col in ((DataGridView)control).Columns)
                        {
                            col.ReadOnly = !stateInfo.Enabled;
                        }
                        break;
                    case DateTimePicker:
                        ((DateTimePicker)control).Enabled = stateInfo.Enabled;
                        break;
                    case Panel:
                    case GroupBox:
                    //case TabPage:
                    case SplitContainer:
                        // 容器控件特殊处理：保持可见，但禁用/启用子控件
                        control.Enabled = stateInfo.Enabled;

                        // 对容器中的子控件进行特殊处理
                        if (stateInfo.Enabled)
                        {
                            // 如果启用容器，则重置子控件状态（如果有缓存）
                            ResetChildControlsState(control);
                        }
                        else
                        {
                            // 如果禁用容器，确保所有子控件也被禁用
                            foreach (Control child in control.Controls)
                            {
                                // 可以根据需要添加特定子控件的处理逻辑
                                if (child is Button || child is ComboBox)
                                {
                                    child.Enabled = false;
                                }
                            }
                        }
                        break;
                    default:
                        // 其他控件默认处理
                        control.Enabled = stateInfo.Enabled;
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "应用控件状态失败: {ControlName}, 类型={ControlType}, 状态={StateInfo}",
                    control.Name, control.GetType().Name, stateInfo);
            }
        }

        /// \u003csummary\u003e
        /// 重置容器控件中子控件的状态
        /// \u003csummary\u003e
        /// \u003cparam name="container"\u003e容器控件\u003c/param\u003e
        private void ResetChildControlsState(Control container)
        {
            if (container == null || !container.HasChildren)
                return;

            try
            {
                foreach (Control child in container.Controls)
                {
                    // 尝试从缓存中恢复控件状态
                    foreach (var statusEntry in _controlStatusCache)
                    {
                        if (statusEntry.Value.TryGetValue(child, out var cachedState))
                        {
                            ApplyControlState(child, cachedState);
                            break;
                        }
                    }
                    // 如果缓存中没有，则启用控件
                    child.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "重置子控件状态失败: {ContainerName}", container.Name);
            }
        }

     

        /// <summary>
        /// 检查控件是否为Krypton控件
        /// </summary>
        /// <param name="control">控件实例</param>
        /// <returns>是否为Krypton控件</returns>
        private bool IsKryptonControl(Control control)
        {
            Type controlType = control.GetType();
            string typeName = controlType.FullName;
            return typeName != null && typeName.StartsWith("Krypton.Toolkit.Krypton");
        }

        /// <summary>
        /// 应用Krypton控件状态
        /// </summary>
        /// <param name="control">Krypton控件</param>
        /// <param name="stateInfo">控件状态信息</param>
        private void ApplyKryptonControlState(Control control, ControlStateInfo stateInfo)
        {
            if (control == null || stateInfo == null)
                return;

            try
            {
                // 先设置可见性
                control.Visible = stateInfo.Visible;

                Type controlType = control.GetType();
                string typeName = controlType.Name;

                // 使用反射设置Krypton控件的属性
                // 基础状态 - 所有Krypton控件都支持Enabled属性
                control.Enabled = stateInfo.Enabled;

                // 根据不同的Krypton控件类型设置特定属性
                if (typeName == "KryptonTextBox")
                {
                    // KryptonTextBox特殊处理：设置ReadOnly属性
                    System.Reflection.PropertyInfo readOnlyProperty = controlType.GetProperty("ReadOnly");
                    if (readOnlyProperty != null && readOnlyProperty.CanWrite)
                    {
                        readOnlyProperty.SetValue(control, !stateInfo.Enabled);
                    }
                }
                else if (typeName == "KryptonComboBox")
                {
                    // KryptonComboBox特殊处理：设置DropDownStyle和Enabled
                    System.Reflection.PropertyInfo dropDownStyleProperty = controlType.GetProperty("DropDownStyle");
                    if (dropDownStyleProperty != null && dropDownStyleProperty.CanWrite)
                    {
                        // 获取KryptonComboBoxStyle枚举值
                        Type enumType = controlType.Assembly.GetType("Krypton.Toolkit.KryptonComboBoxStyle");
                        if (enumType != null)
                        {
                            object enumValue = stateInfo.Enabled ?
                                Enum.Parse(enumType, "DropDown") :
                                Enum.Parse(enumType, "DropDownList");
                            dropDownStyleProperty.SetValue(control, enumValue);
                        }
                    }
                }
                else if (typeName == "KryptonDataGridView")
                {
                    // KryptonDataGridView特殊处理：设置ReadOnly属性
                    System.Reflection.PropertyInfo readOnlyProperty = controlType.GetProperty("ReadOnly");
                    if (readOnlyProperty != null && readOnlyProperty.CanWrite)
                    {
                        readOnlyProperty.SetValue(control, !stateInfo.Enabled);
                    }

                    // 处理列的ReadOnly属性
                    System.Reflection.PropertyInfo columnsProperty = controlType.GetProperty("Columns");
                    if (columnsProperty != null)
                    {
                        object columns = columnsProperty.GetValue(control);
                        if (columns != null && columns is System.Collections.IEnumerable)
                        {
                            foreach (object column in columns as System.Collections.IEnumerable)
                            {
                                if (column != null)
                                {
                                    Type columnType = column.GetType();
                                    System.Reflection.PropertyInfo colReadOnlyProperty = columnType.GetProperty("ReadOnly");
                                    if (colReadOnlyProperty != null && colReadOnlyProperty.CanWrite)
                                    {
                                        colReadOnlyProperty.SetValue(column, !stateInfo.Enabled);
                                    }
                                }
                            }
                        }
                    }
                }
                else if (typeName == "KryptonPanel" || typeName == "KryptonGroupBox")
                {
                    // 容器控件：保持Enabled状态
                    // Krypton容器控件通常会自动处理子控件的状态
                }

                _logger?.LogDebug("成功应用Krypton控件状态: {ControlName}, 类型={ControlType}, 状态={StateInfo}",
                    control.Name, typeName, stateInfo);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "应用Krypton控件状态失败: {ControlName}, 类型={ControlType}, 状态={StateInfo}",
                    control.Name, control.GetType().Name, stateInfo);
            }
        }

     

        /// <summary>
        /// 获取状态类型名称
        /// </summary>
        /// <param name="status">状态枚举值</param>
        /// <returns>规范化的状态类型名称</returns>
        /// <exception cref="ArgumentNullException">当status为null时抛出</exception>
        private string GetStateTypeName(Enum status)
        {
            if (status == null)
                throw new ArgumentNullException(nameof(status));

            Type statusType = status.GetType();

            // 使用缓存避免重复检查
            if (_statusTypeCache.TryGetValue(statusType, out string cachedTypeName))
            {
                return cachedTypeName;
            }

            string typeName = "DataStatus"; // 默认类型

            // 使用类型继承关系判断
            if (typeof(DataStatus).IsAssignableFrom(statusType))
            {
                typeName = "DataStatus";
            }
            else if (typeof(ActionStatus).IsAssignableFrom(statusType))
            {
                typeName = "ActionStatus";
            }
            // 3. 直接使用命名约定确定状态类型
            else
            {
                // 根据类型名称的后缀或包含的关键词来判断
                if (statusType.Name.EndsWith("DataStatus", StringComparison.OrdinalIgnoreCase) ||
                    statusType.Name.IndexOf("数据状态", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    typeName = "DataStatus";
                }
                else if (statusType.Name.EndsWith("BusinessStatus", StringComparison.OrdinalIgnoreCase) ||
                         statusType.Name.IndexOf("业务状态", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    typeName = "BusinessStatus";
                }
                else if (statusType.Name.EndsWith("ActionStatus", StringComparison.OrdinalIgnoreCase) ||
                         statusType.Name.IndexOf("操作状态", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    typeName = "ActionStatus";
                }
                // 4. 作为最后手段，使用名称约定（减少硬编码，仅作为后备方案）
                else
                {
                    string statusTypeName = statusType.Name;
                    string namespaceName = statusType.Namespace ?? "";

                    if (statusTypeName == "DataStatus" || namespaceName.Contains("DataStatus"))
                        typeName = "DataStatus";
                    else if (namespaceName.Contains("BusinessStatus") ||
                             statusTypeName == "PrePaymentStatus" ||
                             statusTypeName == "ARAPStatus" ||
                             statusTypeName == "PaymentStatus" ||
                             statusTypeName == "StatementStatus")
                        typeName = "BusinessStatus";
                    else if (statusTypeName == "ActionStatus" || namespaceName.Contains("ActionStatus"))
                        typeName = "ActionStatus";
                }

                // 缓存结果
                _statusTypeCache[statusType] = typeName;

                if (typeName == "DataStatus" && !statusType.Name.Contains("DataStatus"))
                {
                    _logger?.LogWarning("无法识别的状态类型: {StatusTypeName}, 默认为DataStatus", statusType.Name);
                }
            }
            return typeName;
        }

        /// <summary>
        /// 注册默认规则
        /// </summary>
        private void RegisterDefaultRules()
        {
            _logger?.LogDebug("注册默认UI状态规则");

            // 注意：默认规则在StateRuleConfiguration中配置
            // 这里可以添加特定于UI的额外规则
        }

        /// <summary>
        /// 注册默认控件处理器
        /// </summary>
        private void RegisterDefaultControlHandlers()
        {
            _logger?.LogDebug("注册默认控件处理器");

            // 可以在这里添加特殊控件的自定义处理器
        }

        /// <summary>
        /// 为指定状态清除缓存
        /// </summary>
        /// <param name="status">需要清除缓存的状态</param>
        public void InvalidateCacheForStatus(Enum status)
        {
            if (status == null) return;

            string statusType = GetStateTypeName(status);
            string cacheKey = $"{statusType}_{status}";

            if (_controlStatusCache.ContainsKey(cacheKey))
            {
                _controlStatusCache[cacheKey].Clear();
                _controlStatusCache.Remove(cacheKey);
                _logger?.LogDebug("已清除状态缓存: {CacheKey}", cacheKey);
            }
        }

        /// <summary>
        /// 清除所有缓存
        /// </summary>
        public void ClearAllCache()
        {
            _controlStatusCache.Clear();
            _statusTypeCache.Clear(); // 同时清除状态类型缓存
            _logger?.LogDebug("已清除所有控件状态缓存和状态类型缓存");
        }

        /// <summary>
        /// 获取控件状态缓存
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="control">控件</param>
        /// <returns>控件状态信息，如果缓存中不存在则返回null</returns>
        public ControlStateInfo GetCachedControlState(Enum status, Control control)
        {
            if (status == null || control == null)
                return null;

            string statusType = GetStateTypeName(status);
            string statusKey = $"{statusType}_{status}";

            if (_controlStatusCache.TryGetValue(statusKey, out var controlStates))
            {
                if (controlStates.TryGetValue(control, out var stateInfo))
                {
                    return stateInfo;
                }
            }
            return null;
        }

        /// <summary>
        /// 设置控件状态缓存
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="control">控件</param>
        /// <param name="stateInfo">控件状态信息</param>
        public void SetCachedControlState(Enum status, Control control, ControlStateInfo stateInfo)
        {
            if (status == null || control == null || stateInfo == null)
                return;

            string statusType = GetStateTypeName(status);
            string statusKey = $"{statusType}_{status}";

            if (!_controlStatusCache.ContainsKey(statusKey))
            {
                _controlStatusCache[statusKey] = new Dictionary<Control, ControlStateInfo>();
            }
            _controlStatusCache[statusKey][control] = stateInfo;
        }

        #endregion
    }

    /// <summary>
    /// UI状态上下文实现 - 增强版支持控件可见性
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

        /// <summary>
        /// 控件状态缓存
        /// </summary>
        private readonly Dictionary<Control, ControlStateInfo> _controlStates;

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
            _controlStates = new Dictionary<Control, ControlStateInfo>();
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

        /// <summary>
        /// 获取控件状态信息
        /// </summary>
        /// <param name="control">控件</param>
        /// <returns>控件状态信息</returns>
        public ControlStateInfo GetControlStateInfo(Control control)
        {
            if (control == null)
                throw new ArgumentNullException(nameof(control));

            if (_controlStates.TryGetValue(control, out ControlStateInfo stateInfo))
                return stateInfo;

            // 默认状态为启用且可见
            return new ControlStateInfo(true, true);
        }

        /// <summary>
        /// 设置控件状态信息
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="enabled">是否启用</param>
        /// <param name="visible">是否可见</param>
        public void SetControlStateInfo(Control control, bool enabled, bool visible)
        {
            if (control == null)
                throw new ArgumentNullException(nameof(control));

            _controlStates[control] = new ControlStateInfo(enabled, visible);
        }

        #endregion
    }
}