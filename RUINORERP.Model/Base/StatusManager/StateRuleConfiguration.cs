/**
 * 文件: StateRuleConfiguration.cs
 * 版本: V3增强版 - 轻量级规则配置中心实现
 * 说明: 借鉴V4优点，实现V3的轻量级规则配置中心，保持简洁高效
 * 创建日期: 2024年
 * 作者: RUINOR ERP开发团队
 * 
 * 版本标识：
 * V3增强版: 借鉴V4规则配置中心实现，避免过度复杂化
 * 功能: 提供轻量级状态规则配置实现，保持V3架构优势
 */

using Microsoft.Extensions.Logging;
using RUINORERP.Global;
using RUINORERP.Global.EnumExt;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RUINORERP.Model.Base.StatusManager
{
    /// <summary>
    /// 状态规则配置类
    /// 提供状态转换规则、操作规则和UI控件规则的管理
    /// 支持多种业务状态类型：DataStatus、PaymentStatus、PrePaymentStatus、ARAPStatus等
    /// </summary>
    [Obsolete("此类已过时，请使用UnifiedStateManager类替代。此类将在未来版本中移除。", false)]
    internal class StateRuleConfiguration : IStateRuleConfiguration
    {
        #region 私有字段

        /// <summary>
        /// 状态转换规则缓存 - 支持多类型状态
        /// </summary>
        private readonly Dictionary<Type, Dictionary<object, List<TransitionRule>>> _transitionRules;

        /// <summary>
        /// UI控件状态规则缓存 - 支持多类型状态
        /// </summary>
        private readonly Dictionary<Type, Dictionary<object, Dictionary<string, (bool Enabled, bool Visible)>>> _uiControlRules;

        /// <summary>
        /// 自定义验证规则缓存
        /// </summary>
        private readonly Dictionary<string, Func<object, bool>> _customValidationRules;

        /// <summary>
        /// 操作规则缓存 - 支持多类型状态
        /// </summary>
        private readonly Dictionary<Type, Dictionary<object, Dictionary<string, bool>>> _actionRules;

        /// <summary>
        /// 日志记录器
        /// </summary>
        private readonly ILogger<StateRuleConfiguration> _logger;

        #endregion

        #region 构造函数

        /// <summary>
        /// 初始化状态规则配置中心
        /// </summary>
        /// <param name="logger">日志记录器</param>
        public StateRuleConfiguration(ILogger<StateRuleConfiguration> logger = null)
        {
            _logger = logger;
            _transitionRules = new Dictionary<Type, Dictionary<object, List<TransitionRule>>>();
            _uiControlRules = new Dictionary<Type, Dictionary<object, Dictionary<string, (bool Enabled, bool Visible)>>>();
            _customValidationRules = new Dictionary<string, Func<object, bool>>();
            _actionRules = new Dictionary<Type, Dictionary<object, Dictionary<string, bool>>>();

            // 初始化默认规则
            InitializeDefaultRules();
        }

        #endregion

        #region IStateRuleConfiguration 实现

        /// <summary>
        /// 注册状态转换规则
        /// </summary>
        /// <typeparam name="T">状态枚举类型</typeparam>
        /// <param name="fromStatus">源状态</param>
        /// <param name="toStatus">目标状态</param>
        /// <param name="validator">验证函数</param>
        public void RegisterTransitionRule<T>(T fromStatus, T toStatus, Func<object, bool> validator = null) where T : Enum
        {
            var statusType = typeof(T);
            if (!_transitionRules.ContainsKey(statusType))
            {
                _transitionRules[statusType] = new Dictionary<object, List<TransitionRule>>();
            }

            var rules = _transitionRules[statusType];
            if (!rules.ContainsKey(fromStatus))
            {
                rules[fromStatus] = new List<TransitionRule>();
            }

            rules[fromStatus].Add(new TransitionRule { ToStatus = toStatus, Validator = validator });
        }

        /// <summary>
        /// 获取状态转换规则
        /// </summary>
        /// <typeparam name="T">状态枚举类型</typeparam>
        /// <param name="fromStatus">源状态</param>
        /// <returns>允许转换到的状态列表</returns>
        public IEnumerable<T> GetTransitionRules<T>(T fromStatus) where T : Enum
        {
            var statusType = typeof(T);

            if (_transitionRules.ContainsKey(statusType) && 
                _transitionRules[statusType].ContainsKey(fromStatus))
            {
                return _transitionRules[statusType][fromStatus]
                    .Select(rule => (T)rule.ToStatus);
            }

            return Enumerable.Empty<T>();
        }

        /// <summary>
        /// 验证状态转换是否允许
        /// </summary>
        /// <typeparam name="T">状态枚举类型</typeparam>
        /// <param name="fromStatus">源状态</param>
        /// <param name="toStatus">目标状态</param>
        /// <param name="context">验证上下文</param>
        /// <returns>验证结果</returns>
        public bool ValidateTransition<T>(T fromStatus, T toStatus, object context = null) where T : Enum
        {
            var statusType = typeof(T);

            // 检查转换规则
            if (_transitionRules.ContainsKey(statusType) && 
                _transitionRules[statusType].ContainsKey(fromStatus))
            {
                var rules = _transitionRules[statusType][fromStatus];
                var rule = rules.FirstOrDefault(r => r.ToStatus.Equals(toStatus));
                
                if (rule != null)
                {
                    // 如果有验证器，执行验证
                    if (rule.Validator != null)
                    {
                        return rule.Validator(context);
                    }
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 注册UI控件状态规则
        /// </summary>
        /// <typeparam name="T">状态枚举类型</typeparam>
        /// <param name="status">数据状态</param>
        /// <param name="controlName">控件名称</param>
        /// <param name="enabled">是否启用</param>
        /// <param name="visible">是否可见</param>
        public void RegisterUIControlRule<T>(T status, string controlName, bool enabled, bool visible = true) where T : Enum
        {
            var statusType = typeof(T);
            if (!_uiControlRules.ContainsKey(statusType))
            {
                _uiControlRules[statusType] = new Dictionary<object, Dictionary<string, (bool Enabled, bool Visible)>>();
            }

            if (!_uiControlRules[statusType].ContainsKey(status))
            {
                _uiControlRules[statusType][status] = new Dictionary<string, (bool Enabled, bool Visible)>();
            }

            _uiControlRules[statusType][status][controlName] = (enabled, visible);
        }

        /// <summary>
        /// 获取UI控件状态规则
        /// </summary>
        /// <typeparam name="T">状态枚举类型</typeparam>
        /// <param name="status">数据状态</param>
        /// <returns>控件状态配置字典</returns>
        public Dictionary<string, (bool Enabled, bool Visible)> GetUIControlRules<T>(T status) where T : Enum
        {
            var statusType = typeof(T);
            if (_uiControlRules.ContainsKey(statusType) && _uiControlRules[statusType].ContainsKey(status))
            {
                return new Dictionary<string, (bool Enabled, bool Visible)>(_uiControlRules[statusType][status]);
            }

            return new Dictionary<string, (bool Enabled, bool Visible)>();
        }

        /// <summary>
        /// 注册自定义验证规则
        /// </summary>
        public void RegisterCustomValidationRule(string ruleName, Func<object, bool> validator)
        {
            _customValidationRules[ruleName] = validator;
            _logger?.LogInformation("注册自定义验证规则: {RuleName}", ruleName);
        }

        /// <summary>
        /// 执行自定义验证规则
        /// </summary>
        public bool ExecuteCustomValidationRule(string ruleName, object context = null)
        {
            if (_customValidationRules.ContainsKey(ruleName))
            {
                try
                {
                    return _customValidationRules[ruleName](context);
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "执行自定义验证规则失败: {RuleName}", ruleName);
                    return false;
                }
            }

            _logger?.LogWarning("自定义验证规则不存在: {RuleName}", ruleName);
            return false;
        }

        /// <summary>
        /// 清除所有规则
        /// </summary>
        public void ClearAllRules()
        {
            _transitionRules.Clear();
            _uiControlRules.Clear();
            _customValidationRules.Clear();
            _actionRules.Clear();

            _logger?.LogInformation("清除所有状态规则");
        }

        /// <summary>
        /// 获取可用操作列表
        /// </summary>
        /// <typeparam name="T">状态枚举类型</typeparam>
        /// <param name="currentStatus">当前状态</param>
        /// <returns>可用操作列表</returns>
        public List<string> GetAvailableActions<T>(T currentStatus) where T : Enum
        {
            var actions = new List<string>();
            var statusType = typeof(T);

            if (_actionRules.ContainsKey(statusType) && _actionRules[statusType].ContainsKey(currentStatus))
            {
                actions.AddRange(_actionRules[statusType][currentStatus]
                    .Where(kvp => kvp.Value)
                    .Select(kvp => kvp.Key));
            }

            return actions;
        }

        /// <summary>
        /// 注册操作规则
        /// </summary>
        /// <typeparam name="T">状态枚举类型</typeparam>
        /// <param name="status">状态</param>
        /// <param name="actionName">操作名称</param>
        /// <param name="canExecute">是否可以执行</param>
        public void RegisterActionRule<T>(T status, string actionName, bool canExecute) where T : Enum
        {
            var statusType = typeof(T);
            if (!_actionRules.ContainsKey(statusType))
            {
                _actionRules[statusType] = new Dictionary<object, Dictionary<string, bool>>();
            }

            if (!_actionRules[statusType].ContainsKey(status))
            {
                _actionRules[statusType][status] = new Dictionary<string, bool>();
            }

            _actionRules[statusType][status][actionName] = canExecute;
        }

        /// <summary>
        /// 检查操作是否可以执行
        /// </summary>
        /// <typeparam name="T">状态枚举类型</typeparam>
        /// <param name="status">当前状态</param>
        /// <param name="actionName">操作名称</param>
        /// <returns>是否可以执行</returns>
        public bool CanExecuteAction<T>(T status, string actionName) where T : Enum
        {
            var statusType = typeof(T);
            if (_actionRules.ContainsKey(statusType) && _actionRules[statusType].ContainsKey(status) && _actionRules[statusType][status].ContainsKey(actionName))
            {
                return _actionRules[statusType][status][actionName];
            }

            // 默认允许执行
            return false;
        }

        /// <summary>
        /// 获取允许的操作列表（用于UI控制器）
        /// </summary>
        /// <param name="status">状态枚举</param>
        /// <returns>允许的操作枚举列表</returns>
        public IEnumerable<Enum> GetAllowedActions(Enum status)
        {
            if (status == null)
                return Enumerable.Empty<Enum>();

            var statusType = status.GetType();
            var actions = new List<Enum>();

            if (_actionRules.ContainsKey(statusType) && _actionRules[statusType].ContainsKey(status))
            {
                var allowedActionNames = _actionRules[statusType][status]
                    .Where(kvp => kvp.Value)
                    .Select(kvp => kvp.Key)
                    .ToList();

                // 将操作名称转换为枚举值
                foreach (var actionName in allowedActionNames)
                {
                    var actionEnum = GetActionEnum(actionName);
                    if (actionEnum != null)
                    {
                        actions.Add(actionEnum);
                    }
                }
            }

            return actions;
        }

        /// <summary>
        /// 检查操作是否允许（用于UI控制器）
        /// </summary>
        /// <param name="status">状态枚举</param>
        /// <param name="action">操作名称</param>
        /// <returns>是否允许</returns>
        public bool IsActionAllowed(Enum status, string action)
        {
            if (status == null || string.IsNullOrEmpty(action))
                return false;

            try
            {
                // 使用反射调用泛型方法
                var method = typeof(StateRuleConfiguration)
                    .GetMethod(nameof(CanExecuteAction))
                    ?.MakeGenericMethod(status.GetType());
                
                if (method != null)
                {
                    return (bool)method.Invoke(this, new object[] { status, action });
                }
                
                return false;
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "检查操作权限失败: 状态={Status}, 操作={Action}", status, action);
                return false;
            }
        }

        /// <summary>
        /// 获取业务状态规则
        /// </summary>
        /// <typeparam name="T">业务状态枚举类型</typeparam>
        /// <param name="status">业务状态</param>
        /// <returns>业务状态规则</returns>
        public BusinessStatusRule GetBusinessStatusRule<T>(T status) where T : Enum
        {
            return GetBusinessStatusRule(typeof(T), status);
        }

        /// <summary>
        /// 获取业务状态规则（通过类型和值）
        /// </summary>
        /// <param name="businessStatusType">业务状态类型</param>
        /// <param name="status">状态值</param>
        /// <returns>业务状态规则</returns>
        public BusinessStatusRule GetBusinessStatusRule(Type businessStatusType, object status)
        {
            var key = $"{businessStatusType.Name}_{status}";
            
            // 这里可以根据实际业务需求返回相应的规则
            // 目前返回默认规则，可以根据需要扩展
            return new BusinessStatusRule
            {
                DisplayText = status?.ToString() ?? string.Empty,
                Description = $"{businessStatusType.Name} - {status}",
                IsEditable = true,
                IsDeletable = true,
                AllowApproval = true,
                AllowPrint = true,
                AllowSubmit = true,
                AllowCancel = true,
                AllowModify = true,
                AllowDelete = true,
                AllowView = true,
                AllowExport = true,
                ColorCode = "#000000"
            };
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 初始化默认规则
        /// </summary>
        private void InitializeDefaultRules()
        {
            // 初始化付款状态规则
            InitializePaymentStatusRules();
            
            // 初始化预付款状态规则
            InitializePrePaymentStatusRules();
            
            // 初始化应收应付状态规则
            InitializeARAPStatusRules();
            
            // 初始化数据状态转换规则
            InitializeDataStatusTransitionRules();
            
            // 初始化付款状态转换规则
            InitializePaymentStatusTransitionRules();
            
            // 初始化预付款状态转换规则
            InitializePrePaymentStatusTransitionRules();
            
            // 初始化应收应付状态转换规则
            InitializeARAPStatusTransitionRules();
            
            // 初始化UI控件规则
            InitializeUIControlRules();
            
            // 初始化操作规则
            InitializeActionRules();

            _logger?.LogInformation("初始化默认状态规则完成");
        }

        /// <summary>
        /// 初始化数据状态转换规则
        /// </summary>
        private void InitializeDataStatusTransitionRules()
        {
            // 草稿状态可以转换到：新建、草稿
            RegisterTransitionRule(DataStatus.草稿, DataStatus.新建, context => true);
            RegisterTransitionRule(DataStatus.草稿, DataStatus.草稿, context => true);

            // 新建状态可以转换到：确认、作废
            RegisterTransitionRule(DataStatus.新建, DataStatus.确认, context => true);
            RegisterTransitionRule(DataStatus.新建, DataStatus.作废, context => true);

            // 确认状态可以转换到：完结、作废
            RegisterTransitionRule(DataStatus.确认, DataStatus.完结, context => true);
            RegisterTransitionRule(DataStatus.确认, DataStatus.作废, context => true);

            // 完结状态不能再转换
            // 作废状态可以重新激活为草稿
            RegisterTransitionRule(DataStatus.作废, DataStatus.草稿, context => true);
        }

        /// <summary>
        /// 初始化UI控件规则
        /// </summary>
        private void InitializeUIControlRules()
        {
            // 数据状态UI控件规则
            InitializeDataStatusUIControlRules();
            
            // 付款状态UI控件规则
            InitializePaymentStatusUIControlRules();
            
            // 预付款状态UI控件规则
            InitializePrePaymentStatusUIControlRules();
            
            // 应收应付状态UI控件规则
            InitializeARAPStatusUIControlRules();
        }

        /// <summary>
        /// 初始化数据状态UI控件规则
        /// </summary>
        private void InitializeDataStatusUIControlRules()
        {
            // 草稿状态：允许编辑所有字段
            RegisterUIControlRule(DataStatus.草稿, "txtProductName", true);
            RegisterUIControlRule(DataStatus.草稿, "txtQuantity", true);
            RegisterUIControlRule(DataStatus.草稿, "txtPrice", true);
            RegisterUIControlRule(DataStatus.草稿, "dtpDate", true);
            RegisterUIControlRule(DataStatus.草稿, "cmbCustomer", true);

            // 新建状态：只允许查看部分字段
            RegisterUIControlRule(DataStatus.新建, "txtProductName", false);
            RegisterUIControlRule(DataStatus.新建, "txtQuantity", false);
            RegisterUIControlRule(DataStatus.新建, "txtPrice", false);
            RegisterUIControlRule(DataStatus.新建, "dtpDate", false);
            RegisterUIControlRule(DataStatus.新建, "cmbCustomer", false);

            // 确认状态：只允许查看
            RegisterUIControlRule(DataStatus.确认, "txtProductName", false);
            RegisterUIControlRule(DataStatus.确认, "txtQuantity", false);
            RegisterUIControlRule(DataStatus.确认, "txtPrice", false);
            RegisterUIControlRule(DataStatus.确认, "dtpDate", false);
            RegisterUIControlRule(DataStatus.确认, "cmbCustomer", false);

            // 完结状态：只允许查看
            RegisterUIControlRule(DataStatus.完结, "txtProductName", false);
            RegisterUIControlRule(DataStatus.完结, "txtQuantity", false);
            RegisterUIControlRule(DataStatus.完结, "txtPrice", false);
            RegisterUIControlRule(DataStatus.完结, "dtpDate", false);
            RegisterUIControlRule(DataStatus.完结, "cmbCustomer", false);

            // 作废状态：只允许查看
            RegisterUIControlRule(DataStatus.作废, "txtProductName", false);
            RegisterUIControlRule(DataStatus.作废, "txtQuantity", false);
            RegisterUIControlRule(DataStatus.作废, "txtPrice", false);
            RegisterUIControlRule(DataStatus.作废, "dtpDate", false);
            RegisterUIControlRule(DataStatus.作废, "cmbCustomer", false);
        }

        /// <summary>
        /// 初始化付款状态UI控件规则
        /// </summary>
        private void InitializePaymentStatusUIControlRules()
        {
            // 付款状态控件规则
            RegisterUIControlRule(PaymentStatus.草稿, "txtPaymentAmount", true);
            RegisterUIControlRule(PaymentStatus.草稿, "txtPaymentMethod", true);
            RegisterUIControlRule(PaymentStatus.草稿, "dtpPaymentDate", true);
            
            RegisterUIControlRule(PaymentStatus.待审核, "txtPaymentAmount", false);
            RegisterUIControlRule(PaymentStatus.待审核, "txtPaymentMethod", false);
            RegisterUIControlRule(PaymentStatus.待审核, "dtpPaymentDate", false);
            
            RegisterUIControlRule(PaymentStatus.已支付, "txtPaymentAmount", false);
            RegisterUIControlRule(PaymentStatus.已支付, "txtPaymentMethod", false);
            RegisterUIControlRule(PaymentStatus.已支付, "dtpPaymentDate", false);
        }

        /// <summary>
        /// 初始化预付款状态UI控件规则
        /// </summary>
        private void InitializePrePaymentStatusUIControlRules()
        {
            // 预付款状态控件规则
            RegisterUIControlRule(PrePaymentStatus.草稿, "txtPrePaymentAmount", true);
            RegisterUIControlRule(PrePaymentStatus.草稿, "txtPrePaymentType", true);
            
            RegisterUIControlRule(PrePaymentStatus.待审核, "txtPrePaymentAmount", false);
            RegisterUIControlRule(PrePaymentStatus.待审核, "txtPrePaymentType", false);
            
            RegisterUIControlRule(PrePaymentStatus.已生效, "txtPrePaymentAmount", false);
            RegisterUIControlRule(PrePaymentStatus.已生效, "txtPrePaymentType", false);
            
            RegisterUIControlRule(PrePaymentStatus.待核销, "txtPrePaymentAmount", true);
            RegisterUIControlRule(PrePaymentStatus.待核销, "txtPrePaymentType", false);
            
            RegisterUIControlRule(PrePaymentStatus.部分核销, "txtPrePaymentAmount", true);
            RegisterUIControlRule(PrePaymentStatus.部分核销, "txtPrePaymentType", false);
            
            RegisterUIControlRule(PrePaymentStatus.全额核销, "txtPrePaymentAmount", false);
            RegisterUIControlRule(PrePaymentStatus.全额核销, "txtPrePaymentType", false);
            
            RegisterUIControlRule(PrePaymentStatus.已结案, "txtPrePaymentAmount", false);
            RegisterUIControlRule(PrePaymentStatus.已结案, "txtPrePaymentType", false);
            

        }

        /// <summary>
        /// 初始化应收应付状态UI控件规则
        /// </summary>
        private void InitializeARAPStatusUIControlRules()
        {
            // 应收应付状态控件规则
            RegisterUIControlRule(ARAPStatus.草稿, "txtARAPAmount", true);
            RegisterUIControlRule(ARAPStatus.草稿, "txtARAPType", true);
            
            RegisterUIControlRule(ARAPStatus.待审核, "txtARAPAmount", false);
            RegisterUIControlRule(ARAPStatus.待审核, "txtARAPType", false);
            
            RegisterUIControlRule(ARAPStatus.待支付, "txtARAPAmount", true);
            RegisterUIControlRule(ARAPStatus.待支付, "txtARAPType", false);
            
            RegisterUIControlRule(ARAPStatus.部分支付, "txtARAPAmount", true);
            RegisterUIControlRule(ARAPStatus.部分支付, "txtARAPType", false);
            
            RegisterUIControlRule(ARAPStatus.全部支付, "txtARAPAmount", false);
            RegisterUIControlRule(ARAPStatus.全部支付, "txtARAPType", false);
            
            RegisterUIControlRule(ARAPStatus.坏账, "txtARAPAmount", false);
            RegisterUIControlRule(ARAPStatus.坏账, "txtARAPType", false);
            
            RegisterUIControlRule(ARAPStatus.已冲销, "txtARAPAmount", false);
            RegisterUIControlRule(ARAPStatus.已冲销, "txtARAPType", false);
        }

        /// <summary>
        /// 初始化操作规则
        /// </summary>
        private void InitializeActionRules()
        {
            // 草稿状态
            RegisterActionRule(DataStatus.草稿, "Save", true);
            RegisterActionRule(DataStatus.草稿, "Submit", true);
            RegisterActionRule(DataStatus.草稿, "Approve", false);
            RegisterActionRule(DataStatus.草稿, "Reject", false);
            RegisterActionRule(DataStatus.草稿, "Delete", true);

            // 新建状态
            RegisterActionRule(DataStatus.新建, "Save", true);
            RegisterActionRule(DataStatus.新建, "Submit", false);
            RegisterActionRule(DataStatus.新建, "Approve", true);
            RegisterActionRule(DataStatus.新建, "Reject", true);
            RegisterActionRule(DataStatus.新建, "Delete", true);

            // 确认状态
            RegisterActionRule(DataStatus.确认, "Save", false);
            RegisterActionRule(DataStatus.确认, "Submit", false);
            RegisterActionRule(DataStatus.确认, "Approve", false);
            RegisterActionRule(DataStatus.确认, "Reject", false);
            RegisterActionRule(DataStatus.确认, "Delete", false);

            // 完结状态
            RegisterActionRule(DataStatus.完结, "Save", false);
            RegisterActionRule(DataStatus.完结, "Submit", false);
            RegisterActionRule(DataStatus.完结, "Approve", false);
            RegisterActionRule(DataStatus.完结, "Reject", false);
            RegisterActionRule(DataStatus.完结, "Delete", false);

            // 作废状态
            RegisterActionRule(DataStatus.作废, "Save", false);
            RegisterActionRule(DataStatus.作废, "Submit", false);
            RegisterActionRule(DataStatus.作废, "Approve", false);
            RegisterActionRule(DataStatus.作废, "Reject", false);
            RegisterActionRule(DataStatus.作废, "Delete", false);
        }

        /// <summary>
        /// 初始化业务状态规则（从StatusActionRuleConfiguration整合）
        /// 包含PaymentStatus、PrePaymentStatus、ARAPStatus等
        /// </summary>
        private void InitializeBusinessStatusRules()
        {
            // 初始化付款状态规则
            InitializePaymentStatusRules();
            
            // 初始化预付款状态规则
            InitializePrePaymentStatusRules();
            
            // 初始化应收应付状态规则
            InitializeARAPStatusRules();
            
            // 初始化付款状态转换规则
            InitializePaymentStatusTransitionRules();
            
            // 初始化预付款状态转换规则
            InitializePrePaymentStatusTransitionRules();
            
            // 初始化应收应付状态转换规则
            InitializeARAPStatusTransitionRules();
        }

        /// <summary>
        /// 初始化付款状态规则
        /// </summary>
        private void InitializePaymentStatusRules()
        {
            // 草稿状态允许的操作
            RegisterActionRule(PaymentStatus.草稿, "新增", true);
            RegisterActionRule(PaymentStatus.草稿, "修改", true);
            RegisterActionRule(PaymentStatus.草稿, "删除", true);
            RegisterActionRule(PaymentStatus.草稿, "复制", true);
            RegisterActionRule(PaymentStatus.草稿, "加载", true);

            // 待审核状态允许的操作
            RegisterActionRule(PaymentStatus.待审核, "修改", true);
            RegisterActionRule(PaymentStatus.待审核, "删除", true);
            RegisterActionRule(PaymentStatus.待审核, "复制", true);
            RegisterActionRule(PaymentStatus.待审核, "加载", true);

            // 已支付状态允许的操作
            RegisterActionRule(PaymentStatus.已支付, "修改", false); // 已支付不能修改
            RegisterActionRule(PaymentStatus.已支付, "删除", false); // 已支付不能删除
            RegisterActionRule(PaymentStatus.已支付, "复制", true);
            RegisterActionRule(PaymentStatus.已支付, "加载", true);
        }

        /// <summary>
        /// 初始化预付款状态规则
        /// </summary>
        private void InitializePrePaymentStatusRules()
        {
            // 预付款状态的特殊规则
            RegisterActionRule(PrePaymentStatus.草稿, "预付款新增", true);
            RegisterActionRule(PrePaymentStatus.草稿, "预付款修改", true);
            RegisterActionRule(PrePaymentStatus.草稿, "预付款删除", true);
            
            RegisterActionRule(PrePaymentStatus.待审核, "预付款修改", true);
            RegisterActionRule(PrePaymentStatus.待审核, "预付款删除", true);
            
            RegisterActionRule(PrePaymentStatus.已生效, "预付款修改", true);
            RegisterActionRule(PrePaymentStatus.已生效, "预付款删除", false); // 已生效不能删除
            
            RegisterActionRule(PrePaymentStatus.待核销, "预付款修改", true);
            RegisterActionRule(PrePaymentStatus.待核销, "预付款删除", false); // 待核销不能删除
            
            RegisterActionRule(PrePaymentStatus.部分核销, "预付款修改", true);
            RegisterActionRule(PrePaymentStatus.部分核销, "预付款删除", false); // 部分核销不能删除
            
            RegisterActionRule(PrePaymentStatus.全额核销, "预付款修改", false); // 全额核销不能修改
            RegisterActionRule(PrePaymentStatus.全额核销, "预付款删除", false); // 全额核销不能删除
            
            RegisterActionRule(PrePaymentStatus.已结案, "预付款修改", false); // 已结案不能修改
            RegisterActionRule(PrePaymentStatus.已结案, "预付款删除", false); // 已结案不能删除
        }

        /// <summary>
        /// 初始化应收应付状态规则
        /// </summary>
        private void InitializeARAPStatusRules()
        {
            // 草稿状态
            RegisterActionRule(ARAPStatus.草稿, "应收应付新增", true);
            RegisterActionRule(ARAPStatus.草稿, "应收应付修改", true);
            RegisterActionRule(ARAPStatus.草稿, "应收应付删除", true);
            
            // 待审核状态
            RegisterActionRule(ARAPStatus.待审核, "应收应付修改", true);
            RegisterActionRule(ARAPStatus.待审核, "应收应付删除", true);
            
            // 待支付状态
            RegisterActionRule(ARAPStatus.待支付, "应收应付修改", true);
            RegisterActionRule(ARAPStatus.待支付, "应收应付删除", false); // 待支付不能删除
            
            // 部分支付状态
            RegisterActionRule(ARAPStatus.部分支付, "应收应付修改", true);
            RegisterActionRule(ARAPStatus.部分支付, "应收应付删除", false); // 部分支付不能删除
            
            // 全部支付状态
            RegisterActionRule(ARAPStatus.全部支付, "应收应付修改", false); // 全部支付不能修改
            RegisterActionRule(ARAPStatus.全部支付, "应收应付删除", false); // 全部支付不能删除
            
            // 坏账状态
            RegisterActionRule(ARAPStatus.坏账, "应收应付修改", false); // 坏账不能修改
            RegisterActionRule(ARAPStatus.坏账, "应收应付删除", false); // 坏账不能删除
            
            // 已冲销状态
            RegisterActionRule(ARAPStatus.已冲销, "应收应付修改", false); // 已冲销不能修改
            RegisterActionRule(ARAPStatus.已冲销, "应收应付删除", false); // 已冲销不能删除
        }

        #endregion

        #region 内部类

        /// <summary>
        /// 状态转换规则
        /// </summary>
        private class TransitionRule
        {
            /// <summary>
            /// 目标状态
            /// </summary>
            public object ToStatus { get; set; }

            /// <summary>
            /// 验证函数
            /// </summary>
            public Func<object, bool> Validator { get; set; }
        }

        /// <summary>
        /// 初始化付款状态转换规则
        /// </summary>
        private void InitializePaymentStatusTransitionRules()
        {
            // 付款状态转换规则
            RegisterTransitionRule(PaymentStatus.草稿, PaymentStatus.待审核, context => true);
            RegisterTransitionRule(PaymentStatus.草稿, PaymentStatus.草稿, context => true);
            
            RegisterTransitionRule(PaymentStatus.待审核, PaymentStatus.已支付, context => true);
            RegisterTransitionRule(PaymentStatus.待审核, PaymentStatus.草稿, context => true);
        }

        /// <summary>
        /// 初始化预付款状态转换规则
        /// </summary>
        private void InitializePrePaymentStatusTransitionRules()
        {
            // 预付款状态转换规则
            RegisterTransitionRule(PrePaymentStatus.草稿, PrePaymentStatus.待审核, context => true);
            RegisterTransitionRule(PrePaymentStatus.草稿, PrePaymentStatus.草稿, context => true);
            
            RegisterTransitionRule(PrePaymentStatus.待审核, PrePaymentStatus.已生效, context => true);
            RegisterTransitionRule(PrePaymentStatus.待审核, PrePaymentStatus.草稿, context => true);
            
            RegisterTransitionRule(PrePaymentStatus.已生效, PrePaymentStatus.待核销, context => true);
            RegisterTransitionRule(PrePaymentStatus.已生效, PrePaymentStatus.部分核销, context => true);
            RegisterTransitionRule(PrePaymentStatus.已生效, PrePaymentStatus.全额核销, context => true);
            RegisterTransitionRule(PrePaymentStatus.已生效, PrePaymentStatus.已结案, context => true);
            
            RegisterTransitionRule(PrePaymentStatus.待核销, PrePaymentStatus.部分核销, context => true);
            RegisterTransitionRule(PrePaymentStatus.待核销, PrePaymentStatus.全额核销, context => true);
            RegisterTransitionRule(PrePaymentStatus.待核销, PrePaymentStatus.已结案, context => true);
            
            RegisterTransitionRule(PrePaymentStatus.部分核销, PrePaymentStatus.全额核销, context => true);
            RegisterTransitionRule(PrePaymentStatus.部分核销, PrePaymentStatus.已结案, context => true);
            
            RegisterTransitionRule(PrePaymentStatus.全额核销, PrePaymentStatus.已结案, context => true);
        }

        /// <summary>
        /// 初始化应收应付状态转换规则
        /// </summary>
        private void InitializeARAPStatusTransitionRules()
        {
            // 应收应付状态转换规则
            RegisterTransitionRule(ARAPStatus.草稿, ARAPStatus.待审核, context => true);
            RegisterTransitionRule(ARAPStatus.草稿, ARAPStatus.草稿, context => true);
            
            RegisterTransitionRule(ARAPStatus.待审核, ARAPStatus.待支付, context => true);
            RegisterTransitionRule(ARAPStatus.待审核, ARAPStatus.草稿, context => true);
            
            RegisterTransitionRule(ARAPStatus.部分支付, ARAPStatus.全部支付, context => true);
            RegisterTransitionRule(ARAPStatus.部分支付, ARAPStatus.坏账, context => true);
            RegisterTransitionRule(ARAPStatus.部分支付, ARAPStatus.已冲销, context => true);
            
            RegisterTransitionRule(ARAPStatus.全部支付, ARAPStatus.已冲销, context => true);
            RegisterTransitionRule(ARAPStatus.坏账, ARAPStatus.已冲销, context => true);
        }

        /// <summary>
        /// 根据操作名称获取对应的枚举值
        /// </summary>
        /// <param name="actionName">操作名称</param>
        /// <returns>对应的枚举值，如果找不到则返回null</returns>
        private Enum GetActionEnum(string actionName)
        {
            // 这里需要根据实际项目中的操作枚举类型来实现
            // 暂时返回null，实际项目中需要根据操作名称映射到具体的枚举值
            // 例如：
            // if (actionName == "Approve")
            //     return DocumentAction.Approve;
            // else if (actionName == "Reject")
            //     return DocumentAction.Reject;
            return null;
        }

        #endregion

    }

    #region 业务状态规则类

    /// <summary>
    /// 业务状态规则
    /// </summary>
    public class BusinessStatusRule
    {
        /// <summary>
        /// 显示文本
        /// </summary>
        public string DisplayText { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 是否可编辑
        /// </summary>
        public bool IsEditable { get; set; }

        /// <summary>
        /// 是否可删除
        /// </summary>
        public bool IsDeletable { get; set; }

        /// <summary>
        /// 允许审批
        /// </summary>
        public bool AllowApproval { get; set; }

        /// <summary>
        /// 允许打印
        /// </summary>
        public bool AllowPrint { get; set; }

        /// <summary>
        /// 允许提交
        /// </summary>
        public bool AllowSubmit { get; set; }

        /// <summary>
        /// 允许取消
        /// </summary>
        public bool AllowCancel { get; set; }

        /// <summary>
        /// 允许修改
        /// </summary>
        public bool AllowModify { get; set; }

        /// <summary>
        /// 允许删除
        /// </summary>
        public bool AllowDelete { get; set; }

        /// <summary>
        /// 允许查看
        /// </summary>
        public bool AllowView { get; set; }

        /// <summary>
        /// 允许导出
        /// </summary>
        public bool AllowExport { get; set; }

        /// <summary>
        /// 颜色代码
        /// </summary>
        public string ColorCode { get; set; }
    }

    #endregion
}