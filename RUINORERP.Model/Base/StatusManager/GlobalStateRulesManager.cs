/**
 * 文件: GlobalStateRulesManager.cs
 * 版本: V1.0 - 全局状态规则管理器
 * 说明: 统一管理系统中的所有状态转换规则和UI控件规则，采用单例模式确保全局唯一
 * 创建日期: 2024年
 * 作者: RUINOR ERP开发团队
 * 
 * 功能: 
 * 1. 集中管理状态转换规则和UI控件规则
 * 2. 提供单例模式确保全局唯一性
 * 3. 支持规则的动态添加和修改
 * 4. 提供规则初始化和重置功能
 * 
 * 使用方法:
 * // 获取全局规则管理器实例
 * var rulesManager = GlobalStateRulesManager.Instance;
 * 
 * // 初始化所有规则（应用启动时调用一次）
 * rulesManager.InitializeAllRules();
 * 
 * // 获取状态转换规则
 * var transitionRules = rulesManager.StateTransitionRules;
 * 
 * // 获取UI按钮规则
 * var buttonRules = rulesManager.UIButtonRules;
 * 
 * // 添加自定义规则
 * rulesManager.AddTransitionRule<DataStatus>(DataStatus.草稿, DataStatus.新建);
 * rulesManager.AddButtonRule<DataStatus>(DataStatus.草稿, "btnSave", true);
 */

using RUINORERP.Global;
using RUINORERP.Global.EnumExt;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace RUINORERP.Model.Base.StatusManager
{
    /// <summary>
    /// 全局状态规则管理器
    /// 统一管理系统中的所有状态转换规则和UI控件规则，采用单例模式确保全局唯一
    /// </summary>
    public sealed class GlobalStateRulesManager
    {
        #region 单例实现

        /// <summary>
        /// 线程安全的单例实例
        /// </summary>
        private static readonly Lazy<GlobalStateRulesManager> _lazyInstance = 
            new Lazy<GlobalStateRulesManager>(() => new GlobalStateRulesManager(), true);

        /// <summary>
        /// 获取全局唯一的状态规则管理器实例
        /// </summary>
        public static GlobalStateRulesManager Instance => _lazyInstance.Value;

 
        /// <summary>
        /// 私有构造函数，防止外部实例化
        /// </summary>
        private GlobalStateRulesManager()
        {
            // 初始化规则字典
            _stateTransitionRules = new Dictionary<Type, Dictionary<object, List<object>>>();
            _uiButtonRules = new Dictionary<Type, Dictionary<object, Dictionary<string, (bool Enabled, bool Visible)>>>();
            _actionPermissionRules = new Dictionary<Type, Dictionary<object, List<MenuItemEnums>>>();

            // 标记规则未初始化
            _isInitialized = false;
        }


        /// <summary>
        /// 构造函数，用于测试等特殊场景
        /// </summary>
        /// <param name="useSingleton">是否使用单例模式</param>
        public GlobalStateRulesManager(bool useSingleton)
        {
            if (useSingleton)
            {
                // 如果使用单例模式，直接使用已存在的实例
                var instance = Instance;
                _stateTransitionRules = instance._stateTransitionRules;
                _uiButtonRules = instance._uiButtonRules;
                _actionPermissionRules = instance._actionPermissionRules;
                _isInitialized = instance._isInitialized;
            }
            else
            {
                // 初始化新实例（仅用于测试等特殊场景）
                _stateTransitionRules = new Dictionary<Type, Dictionary<object, List<object>>>();
                _uiButtonRules = new Dictionary<Type, Dictionary<object, Dictionary<string, (bool Enabled, bool Visible)>>>();
                _actionPermissionRules = new Dictionary<Type, Dictionary<object, List<MenuItemEnums>>>();
                _isInitialized = false;
            }
        }

        #endregion

        #region 字段和属性

        /// <summary>
        /// 状态转换规则字典
        /// </summary>
        private Dictionary<Type, Dictionary<object, List<object>>> _stateTransitionRules;

        /// <summary>
        /// UI按钮状态规则字典
        /// </summary>
        private Dictionary<Type, Dictionary<object, Dictionary<string, (bool Enabled, bool Visible)>>> _uiButtonRules;

        /// <summary>
        /// 操作权限规则字典
        /// </summary>
        private Dictionary<Type, Dictionary<object, List<MenuItemEnums>>> _actionPermissionRules;

        /// <summary>
        /// 规则是否已初始化标志
        /// </summary>
        private bool _isInitialized;

        /// <summary>
        /// 获取状态转换规则字典（只读）
        /// </summary>
        public IReadOnlyDictionary<Type, Dictionary<object, List<object>>> StateTransitionRules => 
            new System.Collections.ObjectModel.ReadOnlyDictionary<Type, Dictionary<object, List<object>>>(_stateTransitionRules);

        /// <summary>
        /// 获取UI按钮状态规则字典（只读）
        /// </summary>
        public IReadOnlyDictionary<Type, Dictionary<object, Dictionary<string, (bool Enabled, bool Visible)>>> UIButtonRules => 
            new System.Collections.ObjectModel.ReadOnlyDictionary<Type, Dictionary<object, Dictionary<string, (bool Enabled, bool Visible)>>>(_uiButtonRules);

        /// <summary>
        /// 获取操作权限规则字典（只读）
        /// </summary>
        public IReadOnlyDictionary<Type, Dictionary<object, List<MenuItemEnums>>> ActionPermissionRules => 
            new System.Collections.ObjectModel.ReadOnlyDictionary<Type, Dictionary<object, List<MenuItemEnums>>>(_actionPermissionRules);

        /// <summary>
        /// 获取规则是否已初始化
        /// </summary>
        public bool IsInitialized => _isInitialized;

        #endregion

        #region 规则初始化方法

        /// <summary>
        /// 初始化所有规则（应用启动时调用一次）
        /// 此方法会初始化状态转换规则、UI按钮规则和操作权限规则
        /// </summary>
        public void InitializeAllRules()
        {
            if (_isInitialized)
            {
                // 规则已经初始化，不重复执行
                return;
            }

            try
            {
                // 清空现有规则
                _stateTransitionRules.Clear();
                _uiButtonRules.Clear();
                _actionPermissionRules.Clear();

                // 初始化状态转换规则
                InitializeStateTransitionRules();

                // 初始化UI按钮规则
                InitializeUIButtonRules();

                // 初始化操作权限规则
                InitializeActionPermissionRules();

                // 标记规则已初始化
                _isInitialized = true;

               
               
            }
            catch (Exception ex)
            {
               
            }
        }

        /// <summary>
        /// 重置所有规则（主要用于测试场景）
        /// </summary>
        public void ResetAllRules()
        {
            _stateTransitionRules.Clear();
            _uiButtonRules.Clear();
            _actionPermissionRules.Clear();
            _isInitialized = false;
        }

        #endregion

        #region 状态转换规则方法

        /// <summary>
        /// 初始化状态转换规则
        /// </summary>
        private void InitializeStateTransitionRules()
        {
            // 初始化DataStatus转换规则
            InitializeDataStatusTransitionRules();

            // 初始化ActionStatus转换规则
            InitializeActionStatusTransitionRules();

            // 初始化业务状态转换规则
            InitializeBusinessStatusTransitionRules();
        }

        /// <summary>
        /// 初始化DataStatus状态转换规则
        /// </summary>
        private void InitializeDataStatusTransitionRules()
        {
            var statusType = typeof(DataStatus);
            _stateTransitionRules[statusType] = new Dictionary<object, List<object>>
            {
                [DataStatus.草稿] = new List<object> { DataStatus.新建, DataStatus.作废 },
                [DataStatus.新建] = new List<object> { DataStatus.确认, DataStatus.作废 },
                [DataStatus.确认] = new List<object> { DataStatus.完结, DataStatus.作废 },
                [DataStatus.完结] = new List<object> { }, // 完结状态不能再转换
                [DataStatus.作废] = new List<object> { DataStatus.草稿 } // 作废状态可以重新激活为草稿
            };
        }

        /// <summary>
        /// 初始化ActionStatus状态转换规则
        /// </summary>
        private void InitializeActionStatusTransitionRules()
        {
            var statusType = typeof(ActionStatus);
            _stateTransitionRules[statusType] = new Dictionary<object, List<object>>
            {
                [ActionStatus.无操作] = new List<object> { ActionStatus.新增, ActionStatus.加载, ActionStatus.复制 },
                [ActionStatus.新增] = new List<object> { ActionStatus.修改, ActionStatus.删除 },
                [ActionStatus.修改] = new List<object> { ActionStatus.修改, ActionStatus.删除 },
                [ActionStatus.删除] = new List<object> { ActionStatus.新增 },
                [ActionStatus.加载] = new List<object> { ActionStatus.修改, ActionStatus.删除, ActionStatus.复制 },
                [ActionStatus.复制] = new List<object> { ActionStatus.新增, ActionStatus.修改, ActionStatus.删除 }
            };
        }

        /// <summary>
        /// 初始化业务状态转换规则
        /// </summary>
        private void InitializeBusinessStatusTransitionRules()
        {
            // 初始化付款状态转换规则
            InitializePaymentStatusTransitionRules();

            // 初始化预付款状态转换规则
            InitializePrePaymentStatusTransitionRules();

            // 初始化应收应付状态转换规则
            InitializeARAPStatusTransitionRules();

            // 初始化对账状态转换规则
            InitializeStatementStatusTransitionRules();
        }

        /// <summary>
        /// 初始化付款状态转换规则
        /// </summary>
        private void InitializePaymentStatusTransitionRules()
        {
            var statusType = typeof(RUINORERP.Global.EnumExt.PaymentStatus);
            _stateTransitionRules[statusType] = new Dictionary<object, List<object>>();

            var rules = _stateTransitionRules[statusType];

            // 草稿状态可以转换到：待审核、草稿
            if (Enum.TryParse<RUINORERP.Global.EnumExt.PaymentStatus>("草稿", out var draft))
            {
                rules[draft] = new List<object>();
                if (Enum.TryParse<RUINORERP.Global.EnumExt.PaymentStatus>("待审核", out var pending))
                    rules[draft].Add(pending);
                rules[draft].Add(draft); // 允许自己转换到自己
            }

            // 待审核状态可以转换到：已支付、草稿
            if (Enum.TryParse<RUINORERP.Global.EnumExt.PaymentStatus>("待审核", out var pendingReview))
            {
                rules[pendingReview] = new List<object>();
                if (Enum.TryParse<RUINORERP.Global.EnumExt.PaymentStatus>("已支付", out var paid))
                    rules[pendingReview].Add(paid);
                if (Enum.TryParse<RUINORERP.Global.EnumExt.PaymentStatus>("草稿", out var backToDraft))
                    rules[pendingReview].Add(backToDraft);
            }
        }

        /// <summary>
        /// 初始化预付款状态转换规则
        /// </summary>
        private void InitializePrePaymentStatusTransitionRules()
        {
            var statusType = typeof(RUINORERP.Global.EnumExt.PrePaymentStatus);
            _stateTransitionRules[statusType] = new Dictionary<object, List<object>>();

            var rules = _stateTransitionRules[statusType];

            // 草稿状态可以转换到：待审核、草稿
            if (Enum.TryParse<RUINORERP.Global.EnumExt.PrePaymentStatus>("草稿", out var draft))
            {
                rules[draft] = new List<object>();
                if (Enum.TryParse<RUINORERP.Global.EnumExt.PrePaymentStatus>("待审核", out var pending))
                    rules[draft].Add(pending);
                rules[draft].Add(draft);
            }

            // 待审核状态可以转换到：已生效、草稿
            if (Enum.TryParse<RUINORERP.Global.EnumExt.PrePaymentStatus>("待审核", out var pendingReview))
            {
                rules[pendingReview] = new List<object>();
                if (Enum.TryParse<RUINORERP.Global.EnumExt.PrePaymentStatus>("已生效", out var approved))
                    rules[pendingReview].Add(approved);
                if (Enum.TryParse<RUINORERP.Global.EnumExt.PrePaymentStatus>("草稿", out var backToDraft))
                    rules[pendingReview].Add(backToDraft);
            }

            // 已生效状态可以转换到：待核销、部分核销、全额核销、已结案
            if (Enum.TryParse<RUINORERP.Global.EnumExt.PrePaymentStatus>("已生效", out var approvedStatus))
            {
                rules[approvedStatus] = new List<object>();
                if (Enum.TryParse<RUINORERP.Global.EnumExt.PrePaymentStatus>("待核销", out var pendingVerification))
                    rules[approvedStatus].Add(pendingVerification);
                if (Enum.TryParse<RUINORERP.Global.EnumExt.PrePaymentStatus>("部分核销", out var partial))
                    rules[approvedStatus].Add(partial);
                if (Enum.TryParse<RUINORERP.Global.EnumExt.PrePaymentStatus>("全额核销", out var full))
                    rules[approvedStatus].Add(full);
                if (Enum.TryParse<RUINORERP.Global.EnumExt.PrePaymentStatus>("已结案", out var closed))
                    rules[approvedStatus].Add(closed);
            }

            // 待核销状态可以转换到：部分核销、全额核销、已结案
            if (Enum.TryParse<RUINORERP.Global.EnumExt.PrePaymentStatus>("待核销", out var pendingVer))
            {
                rules[pendingVer] = new List<object>();
                if (Enum.TryParse<RUINORERP.Global.EnumExt.PrePaymentStatus>("部分核销", out var partialVer))
                    rules[pendingVer].Add(partialVer);
                if (Enum.TryParse<RUINORERP.Global.EnumExt.PrePaymentStatus>("全额核销", out var fullVer))
                    rules[pendingVer].Add(fullVer);
                if (Enum.TryParse<RUINORERP.Global.EnumExt.PrePaymentStatus>("已结案", out var closedVer))
                    rules[pendingVer].Add(closedVer);
            }

            // 部分核销状态可以转换到：全额核销、已结案
            if (Enum.TryParse<RUINORERP.Global.EnumExt.PrePaymentStatus>("部分核销", out var varPartialStatus))
            {
                rules[varPartialStatus] = new List<object>();
                if (Enum.TryParse<RUINORERP.Global.EnumExt.PrePaymentStatus>("全额核销", out var fullFromPartial))
                    rules[varPartialStatus].Add(fullFromPartial);
                if (Enum.TryParse<RUINORERP.Global.EnumExt.PrePaymentStatus>("已结案", out var closedFromPartial))
                    rules[varPartialStatus].Add(closedFromPartial);
            }

            // 全额核销状态可以转换到：已结案
            if (Enum.TryParse<RUINORERP.Global.EnumExt.PrePaymentStatus>("全额核销", out var fullStatus))
            {
                rules[fullStatus] = new List<object>();
                if (Enum.TryParse<RUINORERP.Global.EnumExt.PrePaymentStatus>("已结案", out var closedFromFull))
                    rules[fullStatus].Add(closedFromFull);
            }
        }

        /// <summary>
        /// 初始化应收应付状态转换规则
        /// </summary>
        private void InitializeARAPStatusTransitionRules()
        {
            var statusType = typeof(RUINORERP.Global.EnumExt.ARAPStatus);
            _stateTransitionRules[statusType] = new Dictionary<object, List<object>>();

            var rules = _stateTransitionRules[statusType];

            // 草稿状态可以转换到：待审核、草稿
            if (Enum.TryParse<RUINORERP.Global.EnumExt.ARAPStatus>("草稿", out var draft))
            {
                rules[draft] = new List<object>();
                if (Enum.TryParse<RUINORERP.Global.EnumExt.ARAPStatus>("待审核", out var pending))
                    rules[draft].Add(pending);
                rules[draft].Add(draft);
            }

            // 待审核状态可以转换到：待支付、草稿
            if (Enum.TryParse<RUINORERP.Global.EnumExt.ARAPStatus>("待审核", out var pendingReview))
            {
                rules[pendingReview] = new List<object>();
                if (Enum.TryParse<RUINORERP.Global.EnumExt.ARAPStatus>("待支付", out var pendingPayment))
                    rules[pendingReview].Add(pendingPayment);
                if (Enum.TryParse<RUINORERP.Global.EnumExt.ARAPStatus>("草稿", out var backToDraft))
                    rules[pendingReview].Add(backToDraft);
            }

            // 待支付状态可以转换到：部分支付、全部支付
            if (Enum.TryParse<RUINORERP.Global.EnumExt.ARAPStatus>("待支付", out var pendingPaymentStatus))
            {
                rules[pendingPaymentStatus] = new List<object>();
                if (Enum.TryParse<RUINORERP.Global.EnumExt.ARAPStatus>("部分支付", out var partialPayment))
                    rules[pendingPaymentStatus].Add(partialPayment);
                if (Enum.TryParse<RUINORERP.Global.EnumExt.ARAPStatus>("全部支付", out var fullPayment))
                    rules[pendingPaymentStatus].Add(fullPayment);
            }

            // 部分支付状态可以转换到：全部支付、坏账、已冲销
            if (Enum.TryParse<RUINORERP.Global.EnumExt.ARAPStatus>("部分支付", out var partialStatus))
            {
                rules[partialStatus] = new List<object>();
                if (Enum.TryParse<RUINORERP.Global.EnumExt.ARAPStatus>("全部支付", out var fullFromPartial))
                    rules[partialStatus].Add(fullFromPartial);
                if (Enum.TryParse<RUINORERP.Global.EnumExt.ARAPStatus>("坏账", out var badDebt))
                    rules[partialStatus].Add(badDebt);
                if (Enum.TryParse<RUINORERP.Global.EnumExt.ARAPStatus>("已冲销", out var writtenOff))
                    rules[partialStatus].Add(writtenOff);
            }

            // 全部支付状态可以转换到：已冲销
            if (Enum.TryParse<RUINORERP.Global.EnumExt.ARAPStatus>("全部支付", out var fullStatus))
            {
                rules[fullStatus] = new List<object>();
                if (Enum.TryParse<RUINORERP.Global.EnumExt.ARAPStatus>("已冲销", out var writtenOffFromFull))
                    rules[fullStatus].Add(writtenOffFromFull);
            }

            // 坏账状态可以转换到：已冲销
            if (Enum.TryParse<RUINORERP.Global.EnumExt.ARAPStatus>("坏账", out var badDebtStatus))
            {
                rules[badDebtStatus] = new List<object>();
                if (Enum.TryParse<RUINORERP.Global.EnumExt.ARAPStatus>("已冲销", out var writtenOffFromBadDebt))
                    rules[badDebtStatus].Add(writtenOffFromBadDebt);
            }
        }

        /// <summary>
        /// 初始化对账状态转换规则
        /// </summary>
        private void InitializeStatementStatusTransitionRules()
        {
            var statusType = typeof(RUINORERP.Global.EnumExt.StatementStatus);
            _stateTransitionRules[statusType] = new Dictionary<object, List<object>>();

            var rules = _stateTransitionRules[statusType];

            // 草稿状态可以转换到：已发送、已作废、草稿
            if (Enum.TryParse<RUINORERP.Global.EnumExt.StatementStatus>("草稿", out var draft))
            {
                rules[draft] = new List<object>();
                if (Enum.TryParse<RUINORERP.Global.EnumExt.StatementStatus>("已发送", out var sent))
                    rules[draft].Add(sent);
                if (Enum.TryParse<RUINORERP.Global.EnumExt.StatementStatus>("已作废", out var cancelled))
                    rules[draft].Add(cancelled);
                rules[draft].Add(draft);
            }

            // 已发送状态可以转换到：已确认、已结清、部分结算、已作废
            if (Enum.TryParse<RUINORERP.Global.EnumExt.StatementStatus>("已发送", out var sentStatus))
            {
                rules[sentStatus] = new List<object>();
                if (Enum.TryParse<RUINORERP.Global.EnumExt.StatementStatus>("已确认", out var confirmed))
                    rules[sentStatus].Add(confirmed);
                if (Enum.TryParse<RUINORERP.Global.EnumExt.StatementStatus>("已结清", out var settled))
                    rules[sentStatus].Add(settled);
                if (Enum.TryParse<RUINORERP.Global.EnumExt.StatementStatus>("部分结算", out var partiallySettled))
                    rules[sentStatus].Add(partiallySettled);
                if (Enum.TryParse<RUINORERP.Global.EnumExt.StatementStatus>("已作废", out var cancelledFromSent))
                    rules[sentStatus].Add(cancelledFromSent);
            }

            // 已确认状态可以转换到：已结清、部分结算、已作废
            if (Enum.TryParse<RUINORERP.Global.EnumExt.StatementStatus>("已确认", out var confirmedStatus))
            {
                rules[confirmedStatus] = new List<object>();
                if (Enum.TryParse<RUINORERP.Global.EnumExt.StatementStatus>("已结清", out var settledFromConfirmed))
                    rules[confirmedStatus].Add(settledFromConfirmed);
                if (Enum.TryParse<RUINORERP.Global.EnumExt.StatementStatus>("部分结算", out var partiallySettledFromConfirmed))
                    rules[confirmedStatus].Add(partiallySettledFromConfirmed);
                if (Enum.TryParse<RUINORERP.Global.EnumExt.StatementStatus>("已作废", out var cancelledFromConfirmed))
                    rules[confirmedStatus].Add(cancelledFromConfirmed);
            }

            // 部分结算状态可以转换到：已结清
            if (Enum.TryParse<RUINORERP.Global.EnumExt.StatementStatus>("部分结算", out var partiallySettledStatus))
            {
                rules[partiallySettledStatus] = new List<object>();
                if (Enum.TryParse<RUINORERP.Global.EnumExt.StatementStatus>("已结清", out var settledFromPartial))
                    rules[partiallySettledStatus].Add(settledFromPartial);
            }
        }

        #endregion

        #region UI按钮规则方法

        /// <summary>
        /// 初始化UI按钮规则
        /// </summary>
        private void InitializeUIButtonRules()
        {
            // 初始化数据状态UI按钮规则
            InitializeDataStatusUIButtonRules();

            // 初始化付款状态UI按钮规则
            InitializePaymentStatusUIButtonRules();

            // 初始化预付款状态UI按钮规则
            InitializePrePaymentStatusUIButtonRules();

            // 初始化应收应付状态UI按钮规则
            InitializeARAPStatusUIButtonRules();

            // 初始化对账状态UI按钮规则
            InitializeStatementStatusUIButtonRules();

            // 初始化EntityStatus UI按钮规则
            InitializeEntityStatusUIButtonRules();
        }

        /// <summary>
        /// 初始化数据状态UI按钮规则
        /// </summary>
        private void InitializeDataStatusUIButtonRules()
        {
            var statusType = typeof(DataStatus);
            _uiButtonRules[statusType] = new Dictionary<object, Dictionary<string, (bool Enabled, bool Visible)>>();

            // 草稿状态：允许新增、修改、保存、删除、提交
            AddButtonRule(DataStatus.草稿, "toolStripbtnAdd", true, true);
            AddButtonRule(DataStatus.草稿, "toolStripbtnModify", true, true);
            AddButtonRule(DataStatus.草稿, "toolStripButtonSave", true, true);
            AddButtonRule(DataStatus.草稿, "toolStripbtnDelete", true, true);
            AddButtonRule(DataStatus.草稿, "toolStripbtnSubmit", true, true);
            AddButtonRule(DataStatus.草稿, "toolStripbtnReview", false, false);
            AddButtonRule(DataStatus.草稿, "toolStripBtnReverseReview", false, false);
            AddButtonRule(DataStatus.草稿, "toolStripButtonCaseClosed", false, false);
            AddButtonRule(DataStatus.草稿, "toolStripButtonAntiClosed", false, false);
            AddButtonRule(DataStatus.草稿, "toolStripButtonPrint", false, true);
            AddButtonRule(DataStatus.草稿, "toolStripButtonExport", true, true);

            // 新建状态：允许修改、保存、删除、审核
            AddButtonRule(DataStatus.新建, "toolStripbtnAdd", true, true);
            AddButtonRule(DataStatus.新建, "toolStripbtnModify", true, true);
            AddButtonRule(DataStatus.新建, "toolStripButtonSave", true, true);
            AddButtonRule(DataStatus.新建, "toolStripbtnDelete", true, true);
            AddButtonRule(DataStatus.新建, "toolStripbtnSubmit", false, true);
            AddButtonRule(DataStatus.新建, "toolStripbtnReview", true, true);
            AddButtonRule(DataStatus.新建, "toolStripBtnReverseReview", false, false);
            AddButtonRule(DataStatus.新建, "toolStripButtonCaseClosed", false, false);
            AddButtonRule(DataStatus.新建, "toolStripButtonAntiClosed", false, false);
            AddButtonRule(DataStatus.新建, "toolStripButtonPrint", true, true);
            AddButtonRule(DataStatus.新建, "toolStripButtonExport", true, true);

            // 确认状态：允许审核、结案、打印、导出
            AddButtonRule(DataStatus.确认, "toolStripbtnAdd", true, true);
            AddButtonRule(DataStatus.确认, "toolStripbtnModify", false, true);
            AddButtonRule(DataStatus.确认, "toolStripButtonSave", false, true);
            AddButtonRule(DataStatus.确认, "toolStripbtnDelete", false, true);
            AddButtonRule(DataStatus.确认, "toolStripbtnSubmit", false, true);
            AddButtonRule(DataStatus.确认, "toolStripbtnReview", false, true);
            AddButtonRule(DataStatus.确认, "toolStripBtnReverseReview", true, true);
            AddButtonRule(DataStatus.确认, "toolStripButtonCaseClosed", true, true);
            AddButtonRule(DataStatus.确认, "toolStripButtonAntiClosed", false, true);
            AddButtonRule(DataStatus.确认, "toolStripButtonPrint", true, true);
            AddButtonRule(DataStatus.确认, "toolStripButtonExport", true, true);

            // 完结状态：允许反结案、打印、导出
            AddButtonRule(DataStatus.完结, "toolStripbtnAdd", true, true);
            AddButtonRule(DataStatus.完结, "toolStripbtnModify", false, true);
            AddButtonRule(DataStatus.完结, "toolStripButtonSave", false, true);
            AddButtonRule(DataStatus.完结, "toolStripbtnDelete", false, true);
            AddButtonRule(DataStatus.完结, "toolStripbtnSubmit", false, true);
            AddButtonRule(DataStatus.完结, "toolStripbtnReview", false, true);
            AddButtonRule(DataStatus.完结, "toolStripBtnReverseReview", false, true);
            AddButtonRule(DataStatus.完结, "toolStripButtonCaseClosed", false, true);
            AddButtonRule(DataStatus.完结, "toolStripButtonAntiClosed", true, true);
            AddButtonRule(DataStatus.完结, "toolStripButtonPrint", true, true);
            AddButtonRule(DataStatus.完结, "toolStripButtonExport", true, true);

            // 作废状态：允许打印、导出
            AddButtonRule(DataStatus.作废, "toolStripbtnAdd", true, true);
            AddButtonRule(DataStatus.作废, "toolStripbtnModify", false, false);
            AddButtonRule(DataStatus.作废, "toolStripButtonSave", false, false);
            AddButtonRule(DataStatus.作废, "toolStripbtnDelete", false, false);
            AddButtonRule(DataStatus.作废, "toolStripbtnSubmit", false, false);
            AddButtonRule(DataStatus.作废, "toolStripbtnReview", false, false);
            AddButtonRule(DataStatus.作废, "toolStripBtnReverseReview", false, false);
            AddButtonRule(DataStatus.作废, "toolStripButtonCaseClosed", false, false);
            AddButtonRule(DataStatus.作废, "toolStripButtonAntiClosed", false, false);
            AddButtonRule(DataStatus.作废, "toolStripButtonPrint", false, false);
            AddButtonRule(DataStatus.作废, "toolStripButtonExport", false, false);
        }

        /// <summary>
        /// 初始化付款状态UI按钮规则
        /// </summary>
        private void InitializePaymentStatusUIButtonRules()
        {
            // 付款状态按钮规则
            // 草稿状态
            AddButtonRule(PaymentStatus.草稿, "toolStripbtnAdd", true, true);
            AddButtonRule(PaymentStatus.草稿, "toolStripbtnModify", true, true);
            AddButtonRule(PaymentStatus.草稿, "toolStripButtonSave", true, true);
            AddButtonRule(PaymentStatus.草稿, "toolStripbtnDelete", true, true);
            AddButtonRule(PaymentStatus.草稿, "toolStripbtnSubmit", true, true);
            AddButtonRule(PaymentStatus.草稿, "toolStripbtnReview", false, false);
            AddButtonRule(PaymentStatus.草稿, "toolStripBtnReverseReview", false, false);
            AddButtonRule(PaymentStatus.草稿, "toolStripButtonPrint", true, true);

            // 待审核状态
            AddButtonRule(PaymentStatus.待审核, "toolStripbtnAdd", true, true);
            AddButtonRule(PaymentStatus.待审核, "toolStripbtnModify", true, true);
            AddButtonRule(PaymentStatus.待审核, "toolStripButtonSave", true, true);
            AddButtonRule(PaymentStatus.待审核, "toolStripbtnDelete", true, true);
            AddButtonRule(PaymentStatus.待审核, "toolStripbtnSubmit", false, false);
            AddButtonRule(PaymentStatus.待审核, "toolStripbtnReview", true, true);
            AddButtonRule(PaymentStatus.待审核, "toolStripBtnReverseReview", false, false);
            AddButtonRule(PaymentStatus.待审核, "toolStripButtonPrint", true, true);

            // 已支付状态
            AddButtonRule(PaymentStatus.已支付, "toolStripbtnAdd", false, false);
            AddButtonRule(PaymentStatus.已支付, "toolStripbtnModify", false, false);
            AddButtonRule(PaymentStatus.已支付, "toolStripButtonSave", false, false);
            AddButtonRule(PaymentStatus.已支付, "toolStripbtnDelete", false, false);
            AddButtonRule(PaymentStatus.已支付, "toolStripbtnSubmit", false, false);
            AddButtonRule(PaymentStatus.已支付, "toolStripbtnReview", false, false);
            AddButtonRule(PaymentStatus.已支付, "toolStripBtnReverseReview", false, false);
            AddButtonRule(PaymentStatus.已支付, "toolStripButtonPrint", true, true);
        }

        /// <summary>
        /// 初始化预付款状态UI按钮规则
        /// </summary>
        private void InitializePrePaymentStatusUIButtonRules()
        {
            // 预付款状态按钮规则
            // 草稿状态
            AddButtonRule(PrePaymentStatus.草稿, "toolStripbtnAdd", true, true);
            AddButtonRule(PrePaymentStatus.草稿, "toolStripbtnModify", true, true);
            AddButtonRule(PrePaymentStatus.草稿, "toolStripButtonSave", true, true);
            AddButtonRule(PrePaymentStatus.草稿, "toolStripbtnDelete", true, true);
            AddButtonRule(PrePaymentStatus.草稿, "toolStripbtnSubmit", true, true);
            AddButtonRule(PrePaymentStatus.草稿, "toolStripbtnReview", false, false);
            AddButtonRule(PrePaymentStatus.草稿, "toolStripBtnReverseReview", false, false);
            AddButtonRule(PrePaymentStatus.草稿, "toolStripButtonPrint", true, true);

            // 待审核状态
            AddButtonRule(PrePaymentStatus.待审核, "toolStripbtnAdd", true, true);
            AddButtonRule(PrePaymentStatus.待审核, "toolStripbtnModify", true, true);
            AddButtonRule(PrePaymentStatus.待审核, "toolStripButtonSave", true, true);
            AddButtonRule(PrePaymentStatus.待审核, "toolStripbtnDelete", true, true);
            AddButtonRule(PrePaymentStatus.待审核, "toolStripbtnSubmit", false, false);
            AddButtonRule(PrePaymentStatus.待审核, "toolStripbtnReview", true, true);
            AddButtonRule(PrePaymentStatus.待审核, "toolStripBtnReverseReview", false, false);
            AddButtonRule(PrePaymentStatus.待审核, "toolStripButtonPrint", true, true);

            // 已生效状态
            AddButtonRule(PrePaymentStatus.已生效, "toolStripbtnAdd", false, false);
            AddButtonRule(PrePaymentStatus.已生效, "toolStripbtnModify", false, false);
            AddButtonRule(PrePaymentStatus.已生效, "toolStripButtonSave", false, false);
            AddButtonRule(PrePaymentStatus.已生效, "toolStripbtnDelete", false, false);
            AddButtonRule(PrePaymentStatus.已生效, "toolStripbtnSubmit", false, false);
            AddButtonRule(PrePaymentStatus.已生效, "toolStripbtnReview", false, false);
            AddButtonRule(PrePaymentStatus.已生效, "toolStripBtnReverseReview", true, true);
            AddButtonRule(PrePaymentStatus.已生效, "toolStripButtonPrint", true, true);

            // 待核销状态
            AddButtonRule(PrePaymentStatus.待核销, "toolStripbtnAdd", false, false);
            AddButtonRule(PrePaymentStatus.待核销, "toolStripbtnModify", true, true);
            AddButtonRule(PrePaymentStatus.待核销, "toolStripButtonSave", true, true);
            AddButtonRule(PrePaymentStatus.待核销, "toolStripbtnDelete", false, false);
            AddButtonRule(PrePaymentStatus.待核销, "toolStripbtnSubmit", false, false);
            AddButtonRule(PrePaymentStatus.待核销, "toolStripbtnReview", false, false);
            AddButtonRule(PrePaymentStatus.待核销, "toolStripBtnReverseReview", false, false);
            AddButtonRule(PrePaymentStatus.待核销, "toolStripButtonPrint", true, true);

            // 部分核销状态
            AddButtonRule(PrePaymentStatus.部分核销, "toolStripbtnAdd", false, false);
            AddButtonRule(PrePaymentStatus.部分核销, "toolStripbtnModify", true, true);
            AddButtonRule(PrePaymentStatus.部分核销, "toolStripButtonSave", true, true);
            AddButtonRule(PrePaymentStatus.部分核销, "toolStripbtnDelete", false, false);
            AddButtonRule(PrePaymentStatus.部分核销, "toolStripbtnSubmit", false, false);
            AddButtonRule(PrePaymentStatus.部分核销, "toolStripbtnReview", false, false);
            AddButtonRule(PrePaymentStatus.部分核销, "toolStripBtnReverseReview", false, false);
            AddButtonRule(PrePaymentStatus.部分核销, "toolStripButtonPrint", true, true);

            // 全额核销状态
            AddButtonRule(PrePaymentStatus.全额核销, "toolStripbtnAdd", false, false);
            AddButtonRule(PrePaymentStatus.全额核销, "toolStripbtnModify", false, false);
            AddButtonRule(PrePaymentStatus.全额核销, "toolStripButtonSave", false, false);
            AddButtonRule(PrePaymentStatus.全额核销, "toolStripbtnDelete", false, false);
            AddButtonRule(PrePaymentStatus.全额核销, "toolStripbtnSubmit", false, false);
            AddButtonRule(PrePaymentStatus.全额核销, "toolStripbtnReview", false, false);
            AddButtonRule(PrePaymentStatus.全额核销, "toolStripBtnReverseReview", true, true);
            AddButtonRule(PrePaymentStatus.全额核销, "toolStripButtonPrint", true, true);

            // 已结案状态
            AddButtonRule(PrePaymentStatus.已结案, "toolStripbtnAdd", false, false);
            AddButtonRule(PrePaymentStatus.已结案, "toolStripbtnModify", false, false);
            AddButtonRule(PrePaymentStatus.已结案, "toolStripButtonSave", false, false);
            AddButtonRule(PrePaymentStatus.已结案, "toolStripbtnDelete", false, false);
            AddButtonRule(PrePaymentStatus.已结案, "toolStripbtnSubmit", false, false);
            AddButtonRule(PrePaymentStatus.已结案, "toolStripbtnReview", false, false);
            AddButtonRule(PrePaymentStatus.已结案, "toolStripBtnReverseReview", false, false);
            AddButtonRule(PrePaymentStatus.已结案, "toolStripButtonPrint", true, true);
        }

        /// <summary>
        /// 初始化应收应付状态UI按钮规则
        /// </summary>
        private void InitializeARAPStatusUIButtonRules()
        {
            // 应收应付状态按钮规则
            // 草稿状态
            AddButtonRule(ARAPStatus.草稿, "toolStripbtnAdd", true, true);
            AddButtonRule(ARAPStatus.草稿, "toolStripbtnModify", true, true);
            AddButtonRule(ARAPStatus.草稿, "toolStripButtonSave", true, true);
            AddButtonRule(ARAPStatus.草稿, "toolStripbtnDelete", true, true);
            AddButtonRule(ARAPStatus.草稿, "toolStripbtnSubmit", true, true);
            AddButtonRule(ARAPStatus.草稿, "toolStripbtnReview", false, false);
            AddButtonRule(ARAPStatus.草稿, "toolStripBtnReverseReview", false, false);
            AddButtonRule(ARAPStatus.草稿, "toolStripButtonPrint", true, true);

            // 待审核状态
            AddButtonRule(ARAPStatus.待审核, "toolStripbtnAdd", true, true);
            AddButtonRule(ARAPStatus.待审核, "toolStripbtnModify", true, true);
            AddButtonRule(ARAPStatus.待审核, "toolStripButtonSave", true, true);
            AddButtonRule(ARAPStatus.待审核, "toolStripbtnDelete", true, true);
            AddButtonRule(ARAPStatus.待审核, "toolStripbtnSubmit", false, false);
            AddButtonRule(ARAPStatus.待审核, "toolStripbtnReview", true, true);
            AddButtonRule(ARAPStatus.待审核, "toolStripBtnReverseReview", false, false);
            AddButtonRule(ARAPStatus.待审核, "toolStripButtonPrint", true, true);

            // 待支付状态
            AddButtonRule(ARAPStatus.待支付, "toolStripbtnAdd", false, false);
            AddButtonRule(ARAPStatus.待支付, "toolStripbtnModify", true, true);
            AddButtonRule(ARAPStatus.待支付, "toolStripButtonSave", true, true);
            AddButtonRule(ARAPStatus.待支付, "toolStripbtnDelete", false, false);
            AddButtonRule(ARAPStatus.待支付, "toolStripbtnSubmit", false, false);
            AddButtonRule(ARAPStatus.待支付, "toolStripbtnReview", false, false);
            AddButtonRule(ARAPStatus.待支付, "toolStripBtnReverseReview", false, false);
            AddButtonRule(ARAPStatus.待支付, "toolStripButtonPrint", true, true);

            // 部分支付状态
            AddButtonRule(ARAPStatus.部分支付, "toolStripbtnAdd", false, false);
            AddButtonRule(ARAPStatus.部分支付, "toolStripbtnModify", true, true);
            AddButtonRule(ARAPStatus.部分支付, "toolStripButtonSave", true, true);
            AddButtonRule(ARAPStatus.部分支付, "toolStripbtnDelete", false, false);
            AddButtonRule(ARAPStatus.部分支付, "toolStripbtnSubmit", false, false);
            AddButtonRule(ARAPStatus.部分支付, "toolStripbtnReview", false, false);
            AddButtonRule(ARAPStatus.部分支付, "toolStripBtnReverseReview", false, false);
            AddButtonRule(ARAPStatus.部分支付, "toolStripButtonPrint", true, true);

            // 全部支付状态
            AddButtonRule(ARAPStatus.全部支付, "toolStripbtnAdd", false, false);
            AddButtonRule(ARAPStatus.全部支付, "toolStripbtnModify", false, false);
            AddButtonRule(ARAPStatus.全部支付, "toolStripButtonSave", false, false);
            AddButtonRule(ARAPStatus.全部支付, "toolStripbtnDelete", false, false);
            AddButtonRule(ARAPStatus.全部支付, "toolStripbtnSubmit", false, false);
            AddButtonRule(ARAPStatus.全部支付, "toolStripbtnReview", false, false);
            AddButtonRule(ARAPStatus.全部支付, "toolStripBtnReverseReview", true, true);
            AddButtonRule(ARAPStatus.全部支付, "toolStripButtonPrint", true, true);

            // 坏账状态
            AddButtonRule(ARAPStatus.坏账, "toolStripbtnAdd", false, false);
            AddButtonRule(ARAPStatus.坏账, "toolStripbtnModify", false, false);
            AddButtonRule(ARAPStatus.坏账, "toolStripButtonSave", false, false);
            AddButtonRule(ARAPStatus.坏账, "toolStripbtnDelete", false, false);
            AddButtonRule(ARAPStatus.坏账, "toolStripbtnSubmit", false, false);
            AddButtonRule(ARAPStatus.坏账, "toolStripbtnReview", false, false);
            AddButtonRule(ARAPStatus.坏账, "toolStripBtnReverseReview", true, true);
            AddButtonRule(ARAPStatus.坏账, "toolStripButtonPrint", true, true);

            // 已冲销状态
            AddButtonRule(ARAPStatus.已冲销, "toolStripbtnAdd", false, false);
            AddButtonRule(ARAPStatus.已冲销, "toolStripbtnModify", false, false);
            AddButtonRule(ARAPStatus.已冲销, "toolStripButtonSave", false, false);
            AddButtonRule(ARAPStatus.已冲销, "toolStripbtnDelete", false, false);
            AddButtonRule(ARAPStatus.已冲销, "toolStripbtnSubmit", false, false);
            AddButtonRule(ARAPStatus.已冲销, "toolStripbtnReview", false, false);
            AddButtonRule(ARAPStatus.已冲销, "toolStripBtnReverseReview", false, false);
            AddButtonRule(ARAPStatus.已冲销, "toolStripButtonPrint", true, true);
        }

        /// <summary>
        /// 初始化对账状态UI按钮规则
        /// </summary>
        private void InitializeStatementStatusUIButtonRules()
        {
            var statusType = typeof(RUINORERP.Global.EnumExt.StatementStatus);
            _uiButtonRules[statusType] = new Dictionary<object, Dictionary<string, (bool Enabled, bool Visible)>>();

            // 草稿状态：允许新增、修改、保存、删除、提交
            AddButtonRule(RUINORERP.Global.EnumExt.StatementStatus.草稿, "toolStripbtnAdd", true, true);
            AddButtonRule(RUINORERP.Global.EnumExt.StatementStatus.草稿, "toolStripbtnModify", true, true);
            AddButtonRule(RUINORERP.Global.EnumExt.StatementStatus.草稿, "toolStripButtonSave", true, true);
            AddButtonRule(RUINORERP.Global.EnumExt.StatementStatus.草稿, "toolStripbtnDelete", true, true);
            AddButtonRule(RUINORERP.Global.EnumExt.StatementStatus.草稿, "toolStripbtnSubmit", true, true);
            AddButtonRule(RUINORERP.Global.EnumExt.StatementStatus.草稿, "toolStripbtnReview", false, false);
            AddButtonRule(RUINORERP.Global.EnumExt.StatementStatus.草稿, "toolStripBtnReverseReview", false, false);
            AddButtonRule(RUINORERP.Global.EnumExt.StatementStatus.草稿, "toolStripButtonPrint", true, true);

            // 已发送状态：允许修改、保存、确认、结算、作废
            AddButtonRule(RUINORERP.Global.EnumExt.StatementStatus.已发送, "toolStripbtnAdd", true, true);
            AddButtonRule(RUINORERP.Global.EnumExt.StatementStatus.已发送, "toolStripbtnModify", true, true);
            AddButtonRule(RUINORERP.Global.EnumExt.StatementStatus.已发送, "toolStripButtonSave", true, true);
            AddButtonRule(RUINORERP.Global.EnumExt.StatementStatus.已发送, "toolStripbtnDelete", false, false);
            AddButtonRule(RUINORERP.Global.EnumExt.StatementStatus.已发送, "toolStripbtnSubmit", false, false);
            AddButtonRule(RUINORERP.Global.EnumExt.StatementStatus.已发送, "toolStripbtnReview", true, true); // 确认操作
            AddButtonRule(RUINORERP.Global.EnumExt.StatementStatus.已发送, "toolStripBtnReverseReview", false, false);
            AddButtonRule(RUINORERP.Global.EnumExt.StatementStatus.已发送, "toolStripButtonPrint", true, true);

            // 已确认状态：允许结算、打印
            AddButtonRule(RUINORERP.Global.EnumExt.StatementStatus.已确认, "toolStripbtnAdd", false, false);
            AddButtonRule(RUINORERP.Global.EnumExt.StatementStatus.已确认, "toolStripbtnModify", false, false);
            AddButtonRule(RUINORERP.Global.EnumExt.StatementStatus.已确认, "toolStripButtonSave", false, false);
            AddButtonRule(RUINORERP.Global.EnumExt.StatementStatus.已确认, "toolStripbtnDelete", false, false);
            AddButtonRule(RUINORERP.Global.EnumExt.StatementStatus.已确认, "toolStripbtnSubmit", false, false);
            AddButtonRule(RUINORERP.Global.EnumExt.StatementStatus.已确认, "toolStripbtnReview", false, false);
            AddButtonRule(RUINORERP.Global.EnumExt.StatementStatus.已确认, "toolStripBtnReverseReview", true, true);
            AddButtonRule(RUINORERP.Global.EnumExt.StatementStatus.已确认, "toolStripButtonPrint", true, true);

            // 部分结算状态：允许结算、打印
            AddButtonRule(RUINORERP.Global.EnumExt.StatementStatus.部分结算, "toolStripbtnAdd", false, false);
            AddButtonRule(RUINORERP.Global.EnumExt.StatementStatus.部分结算, "toolStripbtnModify", false, false);
            AddButtonRule(RUINORERP.Global.EnumExt.StatementStatus.部分结算, "toolStripButtonSave", false, false);
            AddButtonRule(RUINORERP.Global.EnumExt.StatementStatus.部分结算, "toolStripbtnDelete", false, false);
            AddButtonRule(RUINORERP.Global.EnumExt.StatementStatus.部分结算, "toolStripbtnSubmit", false, false);
            AddButtonRule(RUINORERP.Global.EnumExt.StatementStatus.部分结算, "toolStripbtnReview", false, false);
            AddButtonRule(RUINORERP.Global.EnumExt.StatementStatus.部分结算, "toolStripBtnReverseReview", false, false);
            AddButtonRule(RUINORERP.Global.EnumExt.StatementStatus.部分结算, "toolStripButtonPrint", true, true);

            // 已结清状态：允许打印
            AddButtonRule(RUINORERP.Global.EnumExt.StatementStatus.已结清, "toolStripbtnAdd", false, false);
            AddButtonRule(RUINORERP.Global.EnumExt.StatementStatus.已结清, "toolStripbtnModify", false, false);
            AddButtonRule(RUINORERP.Global.EnumExt.StatementStatus.已结清, "toolStripButtonSave", false, false);
            AddButtonRule(RUINORERP.Global.EnumExt.StatementStatus.已结清, "toolStripbtnDelete", false, false);
            AddButtonRule(RUINORERP.Global.EnumExt.StatementStatus.已结清, "toolStripbtnSubmit", false, false);
            AddButtonRule(RUINORERP.Global.EnumExt.StatementStatus.已结清, "toolStripbtnReview", false, false);
            AddButtonRule(RUINORERP.Global.EnumExt.StatementStatus.已结清, "toolStripBtnReverseReview", false, false);
            AddButtonRule(RUINORERP.Global.EnumExt.StatementStatus.已结清, "toolStripButtonPrint", true, true);

            // 已作废状态：允许打印
            AddButtonRule(RUINORERP.Global.EnumExt.StatementStatus.已作废, "toolStripbtnAdd", false, false);
            AddButtonRule(RUINORERP.Global.EnumExt.StatementStatus.已作废, "toolStripbtnModify", false, false);
            AddButtonRule(RUINORERP.Global.EnumExt.StatementStatus.已作废, "toolStripButtonSave", false, false);
            AddButtonRule(RUINORERP.Global.EnumExt.StatementStatus.已作废, "toolStripbtnDelete", false, false);
            AddButtonRule(RUINORERP.Global.EnumExt.StatementStatus.已作废, "toolStripbtnSubmit", false, false);
            AddButtonRule(RUINORERP.Global.EnumExt.StatementStatus.已作废, "toolStripbtnReview", false, false);
            AddButtonRule(RUINORERP.Global.EnumExt.StatementStatus.已作废, "toolStripBtnReverseReview", false, false);
            AddButtonRule(RUINORERP.Global.EnumExt.StatementStatus.已作废, "toolStripButtonPrint", true, true);
        }

        /// <summary>
        /// 初始化EntityStatus UI按钮规则
        /// 基于EntityStatus的当前状态类型和值，应用对应的UI按钮规则
        /// </summary>
        private void InitializeEntityStatusUIButtonRules()
        {
            // EntityStatus本身不需要定义规则，因为它会根据当前状态类型和值
            // 自动映射到对应的状态枚举规则（DataStatus、PaymentStatus等）
            // GetButtonRules(EntityStatus)方法会处理这种映射

            // 这里可以添加一些通用的EntityStatus规则，如果需要的话
            // 例如，基于审核状态(ApprovalStatus)的按钮规则

            // 审核状态规则
            // 未审核状态
            AddButtonRule(ApprovalStatus.未审核, "toolStripbtnReview", true, true); // 假设0表示未审核
            AddButtonRule(ApprovalStatus.未审核, "toolStripBtnReverseReview", false, false);

            // 已审核通过状态
            AddButtonRule(ApprovalStatus.审核通过, "toolStripbtnReview", false, false); // 假设1表示已审核通过
            AddButtonRule(ApprovalStatus.审核通过, "toolStripBtnReverseReview", true, true);

            // 已审核拒绝状态
            AddButtonRule(ApprovalStatus.审核驳回, "toolStripbtnReview", true, true); // 假设2表示已审核拒绝
            AddButtonRule(ApprovalStatus.审核驳回, "toolStripBtnReverseReview", false, false);
        }

        #endregion

        #region 操作权限规则方法

        /// <summary>
        /// 初始化操作权限规则
        /// </summary>
        private void InitializeActionPermissionRules()
        {
            // 初始化DataStatus操作权限规则
            InitializeDataStatusActionPermissionRules();

            // 初始化PaymentStatus操作权限规则
            InitializePaymentStatusActionPermissionRules();

            // 初始化PrePaymentStatus操作权限规则
            InitializePrePaymentStatusActionPermissionRules();

            // 初始化ARAPStatus操作权限规则
            InitializeARAPStatusActionPermissionRules();

            // 初始化StatementStatus操作权限规则
            InitializeStatementStatusActionPermissionRules();
        }

        /// <summary>
        /// 初始化数据状态操作权限规则
        /// </summary>
        private void InitializeDataStatusActionPermissionRules()
        {
            var statusType = typeof(DataStatus);
            _actionPermissionRules[statusType] = new Dictionary<object, List<MenuItemEnums>>();

            // 草稿状态：允许修改、删除、保存、提交
            AddActionPermissionRule(DataStatus.草稿, new List<MenuItemEnums>
            {
                MenuItemEnums.修改,
                MenuItemEnums.删除,
                MenuItemEnums.保存,
                MenuItemEnums.提交
            });

            // 新建状态：允许修改、删除、保存、提交
            AddActionPermissionRule(DataStatus.新建, new List<MenuItemEnums>
            {
                MenuItemEnums.修改,
                MenuItemEnums.删除,
                MenuItemEnums.保存,
                MenuItemEnums.提交
            });

            // 确认状态：允许修改、保存、提交、审核、反审
            AddActionPermissionRule(DataStatus.确认, new List<MenuItemEnums>
            {
                MenuItemEnums.修改,
                MenuItemEnums.保存,
                MenuItemEnums.提交,
                MenuItemEnums.审核,
                MenuItemEnums.反审
            });

            // 完结状态：允许查询、打印、导出
            AddActionPermissionRule(DataStatus.完结, new List<MenuItemEnums>
            {
                MenuItemEnums.查询,
                MenuItemEnums.打印,
                MenuItemEnums.导出
            });

            // 作废状态：允许查询、删除
            AddActionPermissionRule(DataStatus.作废, new List<MenuItemEnums>
            {
                MenuItemEnums.查询,
                MenuItemEnums.删除
            });
        }

        /// <summary>
        /// 初始化付款状态操作权限规则
        /// </summary>
        private void InitializePaymentStatusActionPermissionRules()
        {
            var statusType = typeof(PaymentStatus);
            _actionPermissionRules[statusType] = new Dictionary<object, List<MenuItemEnums>>();

            // 草稿状态：允许修改、删除、保存、提交
            AddActionPermissionRule(PaymentStatus.草稿, new List<MenuItemEnums>
            {
                MenuItemEnums.修改,
                MenuItemEnums.删除,
                MenuItemEnums.保存,
                MenuItemEnums.提交
            });

            // 待审核状态：允许修改、删除、保存、审核
            AddActionPermissionRule(PaymentStatus.待审核, new List<MenuItemEnums>
            {
                MenuItemEnums.修改,
                MenuItemEnums.删除,
                MenuItemEnums.保存,
                MenuItemEnums.审核
            });

            // 已支付状态：允许查询、打印、导出
            AddActionPermissionRule(PaymentStatus.已支付, new List<MenuItemEnums>
            {
                MenuItemEnums.查询,
                MenuItemEnums.打印,
                MenuItemEnums.导出
            });
        }

        /// <summary>
        /// 初始化预付款状态操作权限规则
        /// </summary>
        private void InitializePrePaymentStatusActionPermissionRules()
        {
            var statusType = typeof(PrePaymentStatus);
            _actionPermissionRules[statusType] = new Dictionary<object, List<MenuItemEnums>>();

            // 草稿状态：允许修改、删除、保存、提交
            AddActionPermissionRule(PrePaymentStatus.草稿, new List<MenuItemEnums>
            {
                MenuItemEnums.修改,
                MenuItemEnums.删除,
                MenuItemEnums.保存,
                MenuItemEnums.提交
            });

            // 待审核状态：允许修改、删除、保存、审核
            AddActionPermissionRule(PrePaymentStatus.待审核, new List<MenuItemEnums>
            {
                MenuItemEnums.修改,
                MenuItemEnums.删除,
                MenuItemEnums.保存,
                MenuItemEnums.审核
            });

            // 已生效状态：允许查询、打印、导出、反审
            AddActionPermissionRule(PrePaymentStatus.已生效, new List<MenuItemEnums>
            {
                MenuItemEnums.查询,
                MenuItemEnums.打印,
                MenuItemEnums.导出,
                MenuItemEnums.反审
            });

            // 待核销状态：允许修改、保存、查询、打印、导出
            AddActionPermissionRule(PrePaymentStatus.待核销, new List<MenuItemEnums>
            {
                MenuItemEnums.修改,
                MenuItemEnums.保存,
                MenuItemEnums.查询,
                MenuItemEnums.打印,
                MenuItemEnums.导出
            });

            // 部分核销状态：允许修改、保存、查询、打印、导出
            AddActionPermissionRule(PrePaymentStatus.部分核销, new List<MenuItemEnums>
            {
                MenuItemEnums.修改,
                MenuItemEnums.保存,
                MenuItemEnums.查询,
                MenuItemEnums.打印,
                MenuItemEnums.导出
            });

            // 全额核销状态：允许查询、打印、导出、反审
            AddActionPermissionRule(PrePaymentStatus.全额核销, new List<MenuItemEnums>
            {
                MenuItemEnums.查询,
                MenuItemEnums.打印,
                MenuItemEnums.导出,
                MenuItemEnums.反审
            });

            // 已结案状态：允许查询、打印、导出
            AddActionPermissionRule(PrePaymentStatus.已结案, new List<MenuItemEnums>
            {
                MenuItemEnums.查询,
                MenuItemEnums.打印,
                MenuItemEnums.导出
            });
        }

        /// <summary>
        /// 初始化应收应付状态操作权限规则
        /// </summary>
        private void InitializeARAPStatusActionPermissionRules()
        {
            var statusType = typeof(ARAPStatus);
            _actionPermissionRules[statusType] = new Dictionary<object, List<MenuItemEnums>>();

            // 草稿状态：允许修改、删除、保存、提交
            AddActionPermissionRule(ARAPStatus.草稿, new List<MenuItemEnums>
            {
                MenuItemEnums.修改,
                MenuItemEnums.删除,
                MenuItemEnums.保存,
                MenuItemEnums.提交
            });

            // 待审核状态：允许修改、删除、保存、审核
            AddActionPermissionRule(ARAPStatus.待审核, new List<MenuItemEnums>
            {
                MenuItemEnums.修改,
                MenuItemEnums.删除,
                MenuItemEnums.保存,
                MenuItemEnums.审核
            });

            // 待支付状态：允许修改、保存、查询、打印、导出
            AddActionPermissionRule(ARAPStatus.待支付, new List<MenuItemEnums>
            {
                MenuItemEnums.修改,
                MenuItemEnums.保存,
                MenuItemEnums.查询,
                MenuItemEnums.打印,
                MenuItemEnums.导出
            });

            // 部分支付状态：允许修改、保存、查询、打印、导出
            AddActionPermissionRule(ARAPStatus.部分支付, new List<MenuItemEnums>
            {
                MenuItemEnums.修改,
                MenuItemEnums.保存,
                MenuItemEnums.查询,
                MenuItemEnums.打印,
                MenuItemEnums.导出
            });

            // 全部支付状态：允许查询、打印、导出、反审
            AddActionPermissionRule(ARAPStatus.全部支付, new List<MenuItemEnums>
            {
                MenuItemEnums.查询,
                MenuItemEnums.打印,
                MenuItemEnums.导出,
                MenuItemEnums.反审
            });

            // 坏账状态：允许查询、打印、导出、反审
            AddActionPermissionRule(ARAPStatus.坏账, new List<MenuItemEnums>
            {
                MenuItemEnums.查询,
                MenuItemEnums.打印,
                MenuItemEnums.导出,
                MenuItemEnums.反审
            });

            // 已冲销状态：允许查询、打印、导出
            AddActionPermissionRule(ARAPStatus.已冲销, new List<MenuItemEnums>
            {
                MenuItemEnums.查询,
                MenuItemEnums.打印,
                MenuItemEnums.导出
            });
        }

        /// <summary>
        /// 初始化对账单状态操作权限规则
        /// </summary>
        private void InitializeStatementStatusActionPermissionRules()
        {
            var statusType = typeof(StatementStatus);
            _actionPermissionRules[statusType] = new Dictionary<object, List<MenuItemEnums>>();

            // 未确认状态：允许修改、删除、保存、提交
            AddActionPermissionRule(StatementStatus.草稿, new List<MenuItemEnums>
            {
                MenuItemEnums.修改,
                MenuItemEnums.删除,
                MenuItemEnums.保存,
                MenuItemEnums.提交
            });

            // 已确认状态：允许查询、打印、导出、反审
            AddActionPermissionRule(StatementStatus.已确认, new List<MenuItemEnums>
            {
                MenuItemEnums.查询,
                MenuItemEnums.打印,
                MenuItemEnums.导出,
                MenuItemEnums.反审
            });

            // 已结案状态：允许查询、打印、导出
            AddActionPermissionRule(StatementStatus.已结清, new List<MenuItemEnums>
            {
                MenuItemEnums.查询,
                MenuItemEnums.打印,
                MenuItemEnums.导出
            });
        }

        #endregion

        #region 规则管理方法

        /// <summary>
        /// 添加状态转换规则
        /// </summary>
        /// <typeparam name="T">状态枚举类型</typeparam>
        /// <param name="fromStatus">源状态</param>
        /// <param name="toStatuses">目标状态列表</param>
        public void AddTransitionRule<T>(T fromStatus, params T[] toStatuses) where T : Enum
        {
            if (fromStatus == null || toStatuses == null)
                return;

            var statusType = typeof(T);
            if (!_stateTransitionRules.ContainsKey(statusType))
            {
                _stateTransitionRules[statusType] = new Dictionary<object, List<object>>();
            }

            if (!_stateTransitionRules[statusType].ContainsKey(fromStatus))
            {
                _stateTransitionRules[statusType][fromStatus] = new List<object>();
            }

            foreach (var toStatus in toStatuses)
            {
                if (!_stateTransitionRules[statusType][fromStatus].Contains(toStatus))
                {
                    _stateTransitionRules[statusType][fromStatus].Add(toStatus);
                }
            }
        }

        /// <summary>
        /// 添加UI按钮状态规则
        /// </summary>
        /// <typeparam name="T">状态枚举类型</typeparam>
        /// <param name="status">状态值</param>
        /// <param name="buttonName">按钮名称</param>
        /// <param name="enabled">是否启用</param>
        /// <param name="visible">是否可见</param>
        public void AddButtonRule<T>(T status, string buttonName, bool enabled, bool visible = true) where T : Enum
        {
            if (status == null || string.IsNullOrEmpty(buttonName))
                return;

            var statusType = typeof(T);
            if (!_uiButtonRules.ContainsKey(statusType))
            {
                _uiButtonRules[statusType] = new Dictionary<object, Dictionary<string, (bool Enabled, bool Visible)>>();
            }

            if (!_uiButtonRules[statusType].ContainsKey(status))
            {
                _uiButtonRules[statusType][status] = new Dictionary<string, (bool Enabled, bool Visible)>();
            }

            _uiButtonRules[statusType][status][buttonName] = (enabled, visible);
        }

        /// <summary>
        /// 添加操作权限规则
        /// </summary>
        /// <typeparam name="T">状态枚举类型</typeparam>
        /// <param name="status">状态值</param>
        /// <param name="actions">允许的操作列表</param>
        public void AddActionPermissionRule<T>(T status, List<MenuItemEnums> actions) where T : Enum
        {
            if (status == null || actions == null)
                return;

            var statusType = typeof(T);
            if (!_actionPermissionRules.ContainsKey(statusType))
            {
                _actionPermissionRules[statusType] = new Dictionary<object, List<MenuItemEnums>>();
            }

            _actionPermissionRules[statusType][status] = actions;
        }

        /// <summary>
        /// 验证状态转换是否合法
        /// </summary>
        /// <typeparam name="T">状态枚举类型</typeparam>
        /// <param name="fromStatus">源状态</param>
        /// <param name="toStatus">目标状态</param>
        /// <returns>是否允许转换</returns>
        public bool IsTransitionAllowed<T>(T fromStatus, T toStatus) where T : Enum
        {
            if (fromStatus == null || toStatus == null)
                return false;

            var statusType = typeof(T);
            if (!_stateTransitionRules.ContainsKey(statusType))
                return false;

            var statusRules = _stateTransitionRules[statusType];
            if (!statusRules.ContainsKey(fromStatus))
                return false;

            return statusRules[fromStatus].Contains(toStatus);
        }

        /// <summary>
        /// 验证状态转换是否合法（非泛型版本）
        /// </summary>
        /// <param name="statusType">状态类型</param>
        /// <param name="fromStatus">源状态</param>
        /// <param name="toStatus">目标状态</param>
        /// <returns>是否允许转换</returns>
        public bool IsTransitionAllowed(Type statusType, object fromStatus, object toStatus)
        {
            if (statusType == null || fromStatus == null || toStatus == null)
                return false;

            if (!_stateTransitionRules.ContainsKey(statusType))
                return false;

            var statusRules = _stateTransitionRules[statusType];
            if (!statusRules.ContainsKey(fromStatus))
                return false;

            return statusRules[fromStatus].Contains(toStatus);
        }

        /// <summary>
        /// 获取可用的状态转换
        /// </summary>
        /// <typeparam name="T">状态枚举类型</typeparam>
        /// <param name="fromStatus">源状态</param>
        /// <returns>可转换的状态列表</returns>
        public List<T> GetAvailableTransitions<T>(T fromStatus) where T : Enum
        {
            var result = new List<T>();

            if (fromStatus == null)
                return result;

            var statusType = typeof(T);
            if (!_stateTransitionRules.ContainsKey(statusType))
                return result;

            if (!_stateTransitionRules[statusType].ContainsKey(fromStatus))
                return result;

            foreach (var status in _stateTransitionRules[statusType][fromStatus])
            {
                if (status is T enumStatus)
                {
                    result.Add(enumStatus);
                }
            }

            return result;
        }

        /// <summary>
        /// 获取按钮状态规则
        /// </summary>
        /// <typeparam name="T">状态枚举类型</typeparam>
        /// <param name="status">状态值</param>
        /// <returns>按钮状态规则字典</returns>
        public Dictionary<string, (bool Enabled, bool Visible)> GetButtonRules<T>(T status) where T : Enum
        {
            if (status == null)
                return new Dictionary<string, (bool Enabled, bool Visible)>();

            var statusType = typeof(T);
            if (_uiButtonRules.ContainsKey(statusType) && _uiButtonRules[statusType].ContainsKey(status))
            {
                return _uiButtonRules[statusType][status];
            }

            return new Dictionary<string, (bool Enabled, bool Visible)>();
        }

        /// <summary>
        /// 获取按钮状态规则（根据状态类型和值）
        /// </summary>
        /// <param name="statusType">状态类型</param>
        /// <param name="status">状态值</param>
        /// <returns>按钮状态规则字典</returns>
        public Dictionary<string, (bool Enabled, bool Visible)> GetButtonRules(Type statusType, object status)
        {
            if (statusType == null || status == null)
                return new Dictionary<string, (bool Enabled, bool Visible)>();

            if (_uiButtonRules.ContainsKey(statusType) && _uiButtonRules[statusType].ContainsKey(status))
            {
                return _uiButtonRules[statusType][status];
            }

            return new Dictionary<string, (bool Enabled, bool Visible)>();
        }

        /// <summary>
        /// 获取按钮状态规则（基于EntityStatus）
        /// </summary>
        /// <param name="entityStatus">实体状态对象</param>
        /// <returns>按钮状态规则字典</returns>
        public Dictionary<string, (bool Enabled, bool Visible)> GetButtonRules(EntityStatus entityStatus)
        {
            if (entityStatus == null)
                return new Dictionary<string, (bool Enabled, bool Visible)>();

            // 获取当前状态类型和值
            var statusType = entityStatus.CurrentStatusType;
            var statusValue = entityStatus.CurrentStatus;

            // 如果状态类型和值都有效，获取对应的按钮规则
            if (statusType != null && statusValue != null && _uiButtonRules.ContainsKey(statusType))
            {
                // 解决类型不匹配问题：将statusValue转换为statusType对应的枚举类型
                object convertedStatusValue;
                try
                {
                    // 如果statusType是枚举类型，尝试将statusValue转换为对应的枚举值
                    if (statusType.IsEnum)
                    {
                        // 如果statusValue已经是枚举类型，直接使用
                        if (statusValue.GetType() == statusType)
                        {
                            convertedStatusValue = statusValue;
                        }
                        else
                        {
                            // 否则尝试转换（例如从整数转换为枚举）
                            convertedStatusValue = Enum.ToObject(statusType, statusValue);
                        }
                    }
                    else
                    {
                        // 非枚举类型直接使用
                        convertedStatusValue = statusValue;
                    }
                }
                catch (Exception)
                {
                    // 如果转换失败，使用原始值
                    convertedStatusValue = statusValue;
                }

                if (_uiButtonRules[statusType].ContainsKey(convertedStatusValue))
                {
                    return _uiButtonRules[statusType][convertedStatusValue];
                }
            }

            // 如果没有找到匹配的规则，返回空字典
            return new Dictionary<string, (bool Enabled, bool Visible)>();
        }

        /// <summary>
        /// 获取操作权限规则（基于状态类型和值）
        /// </summary>
        /// <param name="statusType">状态类型</param>
        /// <param name="status">状态值</param>
        /// <returns>允许的操作列表</returns>
        public List<MenuItemEnums> GetActionPermissionRules(Type statusType, object status)
        {
            if (statusType == null || status == null)
                return new List<MenuItemEnums>();

            if (_actionPermissionRules.ContainsKey(statusType) && _actionPermissionRules[statusType].ContainsKey(status))
            {
                return _actionPermissionRules[statusType][status];
            }

            return new List<MenuItemEnums>();
        }

        /// <summary>
        /// 获取操作权限规则（基于EntityStatus）
        /// </summary>
        /// <param name="entityStatus">实体状态对象</param>
        /// <returns>允许的操作列表</returns>
        public List<MenuItemEnums> GetActionPermissionRules(EntityStatus entityStatus)
        {
            if (entityStatus == null)
                return new List<MenuItemEnums>();

            // 获取当前状态类型和值
            var statusType = entityStatus.CurrentStatusType;
            var statusValue = entityStatus.CurrentStatus;

            // 如果状态类型和值都有效，获取对应的操作权限规则
            if (statusType != null && statusValue != null && _actionPermissionRules.ContainsKey(statusType))
            {
                // 解决类型不匹配问题：将statusValue转换为statusType对应的枚举类型
                object convertedStatusValue;
                try
                {
                    // 如果statusType是枚举类型，尝试将statusValue转换为对应的枚举值
                    if (statusType.IsEnum)
                    {
                        // 如果statusValue已经是枚举类型，直接使用
                        if (statusValue.GetType() == statusType)
                        {
                            convertedStatusValue = statusValue;
                        }
                        else
                        {
                            // 否则尝试转换（例如从整数转换为枚举）
                            convertedStatusValue = Enum.ToObject(statusType, statusValue);
                        }
                    }
                    else
                    {
                        // 非枚举类型直接使用
                        convertedStatusValue = statusValue;
                    }
                }
                catch (Exception)
                {
                    // 如果转换失败，使用原始值
                    convertedStatusValue = statusValue;
                }

                if (_actionPermissionRules[statusType].ContainsKey(convertedStatusValue))
                {
                    return _actionPermissionRules[statusType][convertedStatusValue];
                }
            }

            // 如果没有找到匹配的规则，返回空列表
            return new List<MenuItemEnums>();
        }

        #endregion
    }
}