/**
 * 文件: StatusActionRuleConfiguration.cs
 * 说明: 状态操作规则配置类 - 简化版
 * 创建日期: 2024年
 * 作者: RUINOR ERP开发团队
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using RUINORERP.Global;
using RUINORERP.Global.EnumExt;


namespace RUINORERP.Model.Base.StatusManager
{
    /// <summary>
    /// 状态操作规则配置类 - 简化版
    /// 负责管理不同状态下的操作权限规则
    /// </summary>
    public class StatusActionRuleConfiguration
    {
        #region 字段

        /// <summary>
        /// 日志记录器
        /// </summary>
        private readonly ILogger<StatusActionRuleConfiguration> _logger;

        /// <summary>
        /// 状态操作规则字典
        /// </summary>
        private readonly Dictionary<string, Dictionary<string, bool>> _statusActionRules;

        #endregion

        #region 构造函数

        /// <summary>
        /// 初始化状态操作规则配置
        /// </summary>
        /// <param name="logger">日志记录器</param>
        public StatusActionRuleConfiguration(ILogger<StatusActionRuleConfiguration> logger = null)
        {
            _logger = logger;
            _statusActionRules = new Dictionary<string, Dictionary<string, bool>>();

            // 初始化默认规则
            InitializeDefaultRules();
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 检查指定操作在当前状态下是否可执行
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="action">操作</param>
        /// <returns>是否可执行</returns>
        public bool IsActionAllowed(Enum status, Enum action)
        {
            if (status == null || action == null)
                return false;

            try
            {
                var statusKey = $"{status.GetType().Name}_{status}";
                var actionKey = $"{action.GetType().Name}_{action}";

                if (_statusActionRules.ContainsKey(statusKey) &&
                    _statusActionRules[statusKey].ContainsKey(actionKey))
                {
                    return _statusActionRules[statusKey][actionKey];
                }

                // 默认情况下，允许操作
                return true;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "检查操作权限失败: {Status}, {Action}", status, action);
                return false;
            }
        }

        /// <summary>
        /// 获取当前状态下可执行的操作列表
        /// </summary>
        /// <param name="status">状态</param>
        /// <returns>可执行的操作列表</returns>
        public IEnumerable<Enum> GetAllowedActions(Enum status)
        {
            if (status == null)
                return Enumerable.Empty<Enum>();

            try
            {
                var statusKey = $"{status.GetType().Name}_{status}";
                var allowedActions = new List<Enum>();

                if (_statusActionRules.ContainsKey(statusKey))
                {
                    var actionRules = _statusActionRules[statusKey];
                    foreach (var actionRule in actionRules.Where(r => r.Value))
                    {
                        // 解析操作类型和值
                        var parts = actionRule.Key.Split('_');
                        if (parts.Length >= 2)
                        {
                            var actionTypeName = parts[0];
                            var actionName = parts[1];

                            // 根据操作类型创建枚举值
                            if (actionTypeName.Contains("ActionStatus"))
                            {
                                if (Enum.TryParse<ActionStatus>(actionName, out var actionValue))
                                {
                                    allowedActions.Add(actionValue);
                                }
                            }
                            // 可以添加其他操作类型的处理
                        }
                    }
                }

                return allowedActions;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "获取可执行操作列表失败: {Status}", status);
                return Enumerable.Empty<Enum>();
            }
        }

        /// <summary>
        /// 注册状态操作规则
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="action">操作</param>
        /// <param name="isAllowed">是否允许</param>
        public void RegisterRule(Enum status, Enum action, bool isAllowed)
        {
            if (status == null || action == null)
                return;

            try
            {
                var statusKey = $"{status.GetType().Name}_{status}";
                var actionKey = $"{action.GetType().Name}_{action}";

                if (!_statusActionRules.ContainsKey(statusKey))
                {
                    _statusActionRules[statusKey] = new Dictionary<string, bool>();
                }

                _statusActionRules[statusKey][actionKey] = isAllowed;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "注册状态操作规则失败: {Status}, {Action}", status, action);
            }
        }

        /// <summary>
        /// 清除所有规则
        /// </summary>
        public void ClearRules()
        {
            _statusActionRules.Clear();
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 初始化默认规则
        /// </summary>
        private void InitializeDefaultRules()
        {
            // 初始化数据状态规则
            InitializeDataStatusRules();

            // 初始化预付款状态规则
            InitializePrePaymentStatusRules();

            // 初始化应收应付状态规则
            InitializeARAPStatusRules();

            // 初始化付款状态规则
            InitializePaymentStatusRules();
        }

        /// <summary>
        /// 初始化数据状态规则
        /// </summary>
        private void InitializeDataStatusRules()
        {
            // 草稿状态允许的操作
            RegisterRule(DataStatus.草稿, ActionStatus.新增, true);
            RegisterRule(DataStatus.草稿, ActionStatus.修改, true);
            RegisterRule(DataStatus.草稿, ActionStatus.删除, true);
            RegisterRule(DataStatus.草稿, ActionStatus.复制, true);
            RegisterRule(DataStatus.草稿, ActionStatus.加载, true);

            // 新建状态允许的操作
            RegisterRule(DataStatus.新建, ActionStatus.修改, true);
            RegisterRule(DataStatus.新建, ActionStatus.删除, true);
            RegisterRule(DataStatus.新建, ActionStatus.复制, true);
            RegisterRule(DataStatus.新建, ActionStatus.加载, true);

            // 确认状态允许的操作
            RegisterRule(DataStatus.确认, ActionStatus.修改, true);
            RegisterRule(DataStatus.确认, ActionStatus.删除, false); // 确认后不能删除
            RegisterRule(DataStatus.确认, ActionStatus.复制, true);
            RegisterRule(DataStatus.确认, ActionStatus.加载, true);

            // 完结状态允许的操作
            RegisterRule(DataStatus.完结, ActionStatus.修改, false); // 完结后不能修改
            RegisterRule(DataStatus.完结, ActionStatus.删除, false); // 完结后不能删除
            RegisterRule(DataStatus.完结, ActionStatus.复制, true);
            RegisterRule(DataStatus.完结, ActionStatus.加载, true);

            // 作废状态允许的操作
            RegisterRule(DataStatus.作废, ActionStatus.修改, false); // 作废后不能修改
            RegisterRule(DataStatus.作废, ActionStatus.删除, true); // 可以删除作废记录
            RegisterRule(DataStatus.作废, ActionStatus.复制, false); // 作废后不能复制
            RegisterRule(DataStatus.作废, ActionStatus.加载, true);
        }

        /// <summary>
        /// 初始化预付款状态规则
        /// </summary>
        private void InitializePrePaymentStatusRules()
        {
            // 草稿状态允许的操作
            RegisterRule(PrePaymentStatus.草稿, ActionStatus.新增, true);
            RegisterRule(PrePaymentStatus.草稿, ActionStatus.修改, true);
            RegisterRule(PrePaymentStatus.草稿, ActionStatus.删除, true);
            RegisterRule(PrePaymentStatus.草稿, ActionStatus.复制, true);
            RegisterRule(PrePaymentStatus.草稿, ActionStatus.加载, true);

            // 待审核状态允许的操作
            RegisterRule(PrePaymentStatus.待审核, ActionStatus.修改, true);
            RegisterRule(PrePaymentStatus.待审核, ActionStatus.删除, true);
            RegisterRule(PrePaymentStatus.待审核, ActionStatus.复制, true);
            RegisterRule(PrePaymentStatus.待审核, ActionStatus.加载, true);

            // 已生效状态允许的操作
            RegisterRule(PrePaymentStatus.已生效, ActionStatus.修改, true);
            RegisterRule(PrePaymentStatus.已生效, ActionStatus.删除, false); // 已生效不能删除
            RegisterRule(PrePaymentStatus.已生效, ActionStatus.复制, true);
            RegisterRule(PrePaymentStatus.已生效, ActionStatus.加载, true);

            // 待核销状态允许的操作
            RegisterRule(PrePaymentStatus.待核销, ActionStatus.修改, true);
            RegisterRule(PrePaymentStatus.待核销, ActionStatus.删除, false); // 待核销不能删除
            RegisterRule(PrePaymentStatus.待核销, ActionStatus.复制, true);
            RegisterRule(PrePaymentStatus.待核销, ActionStatus.加载, true);

            // 部分核销状态允许的操作
            RegisterRule(PrePaymentStatus.部分核销, ActionStatus.修改, true);
            RegisterRule(PrePaymentStatus.部分核销, ActionStatus.删除, false); // 部分核销不能删除
            RegisterRule(PrePaymentStatus.部分核销, ActionStatus.复制, true);
            RegisterRule(PrePaymentStatus.部分核销, ActionStatus.加载, true);

            // 全额核销状态允许的操作
            RegisterRule(PrePaymentStatus.全额核销, ActionStatus.修改, false); // 全额核销不能修改
            RegisterRule(PrePaymentStatus.全额核销, ActionStatus.删除, false); // 全额核销不能删除
            RegisterRule(PrePaymentStatus.全额核销, ActionStatus.复制, true);
            RegisterRule(PrePaymentStatus.全额核销, ActionStatus.加载, true);

            // 已结案状态允许的操作
            RegisterRule(PrePaymentStatus.已结案, ActionStatus.修改, false); // 已结案不能修改
            RegisterRule(PrePaymentStatus.已结案, ActionStatus.删除, false); // 已结案不能删除
            RegisterRule(PrePaymentStatus.已结案, ActionStatus.复制, true);
            RegisterRule(PrePaymentStatus.已结案, ActionStatus.加载, true);
        }

        /// <summary>
        /// 初始化应收应付状态规则
        /// </summary>
        private void InitializeARAPStatusRules()
        {
            // 草稿状态允许的操作
            RegisterRule(ARAPStatus.草稿, ActionStatus.新增, true);
            RegisterRule(ARAPStatus.草稿, ActionStatus.修改, true);
            RegisterRule(ARAPStatus.草稿, ActionStatus.删除, true);
            RegisterRule(ARAPStatus.草稿, ActionStatus.复制, true);
            RegisterRule(ARAPStatus.草稿, ActionStatus.加载, true);

            // 待审核状态允许的操作
            RegisterRule(ARAPStatus.待审核, ActionStatus.修改, true);
            RegisterRule(ARAPStatus.待审核, ActionStatus.删除, true);
            RegisterRule(ARAPStatus.待审核, ActionStatus.复制, true);
            RegisterRule(ARAPStatus.待审核, ActionStatus.加载, true);

            // 待支付状态允许的操作
            RegisterRule(ARAPStatus.待支付, ActionStatus.修改, true);
            RegisterRule(ARAPStatus.待支付, ActionStatus.删除, false); // 待支付不能删除
            RegisterRule(ARAPStatus.待支付, ActionStatus.复制, true);
            RegisterRule(ARAPStatus.待支付, ActionStatus.加载, true);

            // 部分支付状态允许的操作
            RegisterRule(ARAPStatus.部分支付, ActionStatus.修改, true);
            RegisterRule(ARAPStatus.部分支付, ActionStatus.删除, false); // 部分支付不能删除
            RegisterRule(ARAPStatus.部分支付, ActionStatus.复制, true);
            RegisterRule(ARAPStatus.部分支付, ActionStatus.加载, true);

            // 全部支付状态允许的操作
            RegisterRule(ARAPStatus.全部支付, ActionStatus.修改, false); // 全部支付不能修改
            RegisterRule(ARAPStatus.全部支付, ActionStatus.删除, false); // 全部支付不能删除
            RegisterRule(ARAPStatus.全部支付, ActionStatus.复制, true);
            RegisterRule(ARAPStatus.全部支付, ActionStatus.加载, true);

            // 坏账状态允许的操作
            RegisterRule(ARAPStatus.坏账, ActionStatus.修改, false); // 坏账不能修改
            RegisterRule(ARAPStatus.坏账, ActionStatus.删除, false); // 坏账不能删除
            RegisterRule(ARAPStatus.坏账, ActionStatus.复制, true);
            RegisterRule(ARAPStatus.坏账, ActionStatus.加载, true);

            // 已冲销状态允许的操作
            RegisterRule(ARAPStatus.已冲销, ActionStatus.修改, false); // 已冲销不能修改
            RegisterRule(ARAPStatus.已冲销, ActionStatus.删除, false); // 已冲销不能删除
            RegisterRule(ARAPStatus.已冲销, ActionStatus.复制, true);
            RegisterRule(ARAPStatus.已冲销, ActionStatus.加载, true);
        }

        /// <summary>
        /// 初始化付款状态规则
        /// </summary>
        private void InitializePaymentStatusRules()
        {
            // 草稿状态允许的操作
            RegisterRule(PaymentStatus.草稿, ActionStatus.新增, true);
            RegisterRule(PaymentStatus.草稿, ActionStatus.修改, true);
            RegisterRule(PaymentStatus.草稿, ActionStatus.删除, true);
            RegisterRule(PaymentStatus.草稿, ActionStatus.复制, true);
            RegisterRule(PaymentStatus.草稿, ActionStatus.加载, true);

            // 待审核状态允许的操作
            RegisterRule(PaymentStatus.待审核, ActionStatus.修改, true);
            RegisterRule(PaymentStatus.待审核, ActionStatus.删除, true);
            RegisterRule(PaymentStatus.待审核, ActionStatus.复制, true);
            RegisterRule(PaymentStatus.待审核, ActionStatus.加载, true);

            // 已支付状态允许的操作
            RegisterRule(PaymentStatus.已支付, ActionStatus.修改, false); // 已支付不能修改
            RegisterRule(PaymentStatus.已支付, ActionStatus.删除, false); // 已支付不能删除
            RegisterRule(PaymentStatus.已支付, ActionStatus.复制, true);
            RegisterRule(PaymentStatus.已支付, ActionStatus.加载, true);
        }

        #endregion
    }
}