/**
 * 文件: GlobalStateRulesManager.cs
 * 版本: V4 - 优化版全局状态规则管理器
 * 说明: 统一管理系统中的所有状态转换规则和UI控件规则，采用单例模式确保全局唯一
 * 创建日期: 2024年
 * 作者: RUINOR ERP开发团队
 * 更新日期: 2025-01-12 - V4版本优化，简化规则管理和缓存机制
 */

using RUINORERP.Global;
using RUINORERP.Global.EnumExt;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RUINORERP.Model.Base.StatusManager
{
    /// <summary>
    /// 全局状态规则管理器
    /// 统一管理系统中的所有状态转换规则和UI控件规则，采用单例模式确保全局唯一
    /// 支持全局提交修改模式设置，控制单据提交后的修改权限
    /// </summary>
    /// <remarks>
    /// 使用示例 - 提交修改模式设置：
    /// \code
    /// // 设置为严格模式（提交后不允许修改）
    /// GlobalStateRulesManager.Instance.SetSubmitModifyRuleMode(SubmitModifyRuleMode.严格模式);
    /// 
    /// // 或设置为灵活模式（提交后允许修改）
    /// GlobalStateRulesManager.Instance.SetSubmitModifyRuleMode(SubmitModifyRuleMode.灵活模式);
    /// 
    /// // 检查是否允许在特定状态下修改
    /// bool canModify = GlobalStateRulesManager.Instance.AllowModifyAfterSubmit(isSubmittedStatus);
    /// \endcode
    /// </remarks>
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
            _uiButtonRules = new Dictionary<Type, Dictionary<object, Dictionary<string, bool>>>();
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
                _uiButtonRules = new Dictionary<Type, Dictionary<object, Dictionary<string, bool>>>();
                _actionPermissionRules = new Dictionary<Type, Dictionary<object, List<MenuItemEnums>>>();
                _isInitialized = false;
            }
        }

        #endregion

        #region 字段和属性

        /// <summary>
        /// 全局提交修改规则模式
        /// 控制单据提交后是否允许修改的行为
        /// 默认值：灵活模式（允许修改）
        /// </summary>
        public SubmitModifyRuleMode submitModifyRuleMode { get; set; } = SubmitModifyRuleMode.灵活模式;

        /// <summary>
        /// 设置提交修改模式
        /// 设置后需要重新初始化规则以应用新的模式设置
        /// </summary>
        /// <param name="mode">新的提交修改模式</param>
        public void SetSubmitModifyRuleMode(SubmitModifyRuleMode mode)
        {
            if (submitModifyRuleMode != mode)
            {
                submitModifyRuleMode = mode;

                // 重新初始化规则以应用新模式
                if (_isInitialized)
                {
                    ResetAllRules();
                    InitializeAllRules();
                }
            }
        }


        /// <summary>
        /// 状态转换规则字典
        /// </summary>
        private Dictionary<Type, Dictionary<object, List<object>>> _stateTransitionRules;

        /// <summary>
        /// UI按钮状态规则字典 - 仅包含Enabled状态，Visible由权限系统管理
        /// </summary>
        private Dictionary<Type, Dictionary<object, Dictionary<string, bool>>> _uiButtonRules;

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
            _stateTransitionRules;

        /// <summary>
        /// 获取UI按钮状态规则字典（只读）- 仅包含Enabled状态
        /// </summary>
        public IReadOnlyDictionary<Type, Dictionary<object, Dictionary<string, bool>>> UIButtonRules =>
            _uiButtonRules;

        /// <summary>
        /// 获取操作权限规则字典（只读）
        /// </summary>
        public IReadOnlyDictionary<Type, Dictionary<object, List<MenuItemEnums>>> ActionPermissionRules =>
            _actionPermissionRules;

        /// <summary>
        /// 获取规则是否已初始化
        /// </summary>
        public bool IsInitialized => _isInitialized;

        #endregion

        #region 规则初始化方法

        /// <summary>
        /// 初始化所有规则（应用启动时调用一次）
        /// </summary>
        public void InitializeAllRules()
        {
            if (_isInitialized)
                return;

            // 清空现有规则
            _stateTransitionRules.Clear();
            _uiButtonRules.Clear();
            _actionPermissionRules.Clear();

            // 初始化各类规则
            InitializeStateTransitionRules();
            InitializeUIButtonRules();
            InitializeActionPermissionRules();

            // 标记规则已初始化
            _isInitialized = true;
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
            // 初始化基本状态转换规则
            InitializeDataStatusTransitionRules();
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
                [DataStatus.确认] = new List<object> { DataStatus.新建, DataStatus.完结, DataStatus.作废 },
                [DataStatus.完结] = new List<object> { }, // 完结状态不能再转换
                [DataStatus.作废] = new List<object> { } // 作废状态就不再使用
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
                [ActionStatus.新增] = new List<object> { ActionStatus.无操作 },
                [ActionStatus.修改] = new List<object> { ActionStatus.无操作 },
                [ActionStatus.删除] = new List<object> { ActionStatus.无操作 },
                [ActionStatus.加载] = new List<object> { ActionStatus.无操作, ActionStatus.修改, ActionStatus.删除, ActionStatus.复制 },
                [ActionStatus.复制] = new List<object> { ActionStatus.无操作, ActionStatus.新增 }
            };
        }

        /// <summary>
        /// 初始化业务状态转换规则
        /// </summary>
        private void InitializeBusinessStatusTransitionRules()
        {
            InitializePaymentStatusTransitionRules();
            InitializePrePaymentStatusTransitionRules();
            InitializeARAPStatusTransitionRules();
            InitializeStatementStatusTransitionRules();
        }

        /// <summary>
        /// 初始化付款状态转换规则
        /// </summary>
        private void InitializePaymentStatusTransitionRules()
        {
            var statusType = typeof(PaymentStatus);
            _stateTransitionRules[statusType] = new Dictionary<object, List<object>>
            {
                [PaymentStatus.草稿] = new List<object> { PaymentStatus.待审核, PaymentStatus.草稿 },
                [PaymentStatus.待审核] = new List<object> { PaymentStatus.已支付, PaymentStatus.草稿 },
                [PaymentStatus.已支付] = new List<object> { }
            };
        }

        /// <summary>
        /// 初始化预付款状态转换规则
        /// </summary>
        private void InitializePrePaymentStatusTransitionRules()
        {
            var statusType = typeof(PrePaymentStatus);
            _stateTransitionRules[statusType] = new Dictionary<object, List<object>>
            {
                [PrePaymentStatus.草稿] = new List<object> { PrePaymentStatus.待审核, PrePaymentStatus.草稿 },
                [PrePaymentStatus.待审核] = new List<object> { PrePaymentStatus.已生效, PrePaymentStatus.草稿 },
                [PrePaymentStatus.已生效] = new List<object> { PrePaymentStatus.待核销, PrePaymentStatus.部分核销, PrePaymentStatus.全额核销, PrePaymentStatus.已结案 },
                [PrePaymentStatus.待核销] = new List<object> { PrePaymentStatus.部分核销, PrePaymentStatus.全额核销, PrePaymentStatus.已结案 },
                [PrePaymentStatus.部分核销] = new List<object> { PrePaymentStatus.全额核销, PrePaymentStatus.已结案 },
                [PrePaymentStatus.全额核销] = new List<object> { PrePaymentStatus.已结案 }
            };
        }

        /// <summary>
        /// 初始化应收应付状态转换规则
        /// </summary>
        private void InitializeARAPStatusTransitionRules()
        {
            var statusType = typeof(ARAPStatus);
            _stateTransitionRules[statusType] = new Dictionary<object, List<object>>
            {
                [ARAPStatus.草稿] = new List<object> { ARAPStatus.待审核, ARAPStatus.草稿 },
                [ARAPStatus.待审核] = new List<object> { ARAPStatus.待支付, ARAPStatus.草稿 },
                [ARAPStatus.待支付] = new List<object> { ARAPStatus.部分支付, ARAPStatus.全部支付 },
                [ARAPStatus.部分支付] = new List<object> { ARAPStatus.全部支付, ARAPStatus.坏账, ARAPStatus.已冲销 },
                [ARAPStatus.全部支付] = new List<object> { ARAPStatus.已冲销 },
                [ARAPStatus.坏账] = new List<object> { ARAPStatus.已冲销 }
            };
        }

        /// <summary>
        /// 初始化对账状态转换规则
        /// </summary>
        private void InitializeStatementStatusTransitionRules()
        {
            var statusType = typeof(StatementStatus);
            _stateTransitionRules[statusType] = new Dictionary<object, List<object>>
            {
                // 草稿状态可以转换到新建状态或已作废状态
                [StatementStatus.草稿] = new List<object> { StatementStatus.新建, StatementStatus.已作废 },
                
                // 新建状态可以转换到确认状态或已作废状态
                [StatementStatus.新建] = new List<object> { StatementStatus.确认, StatementStatus.已作废 },
                
                // 确认状态可以转换到部分结算状态、全部结清状态或已作废状态
                [StatementStatus.确认] = new List<object> { StatementStatus.部分结算, StatementStatus.全部结清, StatementStatus.已作废 },
                
                // 部分结算状态可以转换到全部结清状态
                [StatementStatus.部分结算] = new List<object> { StatementStatus.全部结清 },
                
                // 全部结清和已作废是终态，不能再转换
                [StatementStatus.全部结清] = new List<object> { },
                [StatementStatus.已作废] = new List<object> { }
            };
        }

        /// <summary>
        /// 添加状态转换规则
        /// </summary>
        /// <typeparam name="T">状态类型</typeparam>
        /// <param name="fromStatus">源状态</param>
        /// <param name="toStatus">目标状态</param>
        public void AddTransitionRule<T>(T fromStatus, T toStatus) where T : struct
        {
            var statusType = typeof(T);

            // 确保规则字典存在
            if (!_stateTransitionRules.ContainsKey(statusType))
                _stateTransitionRules[statusType] = new Dictionary<object, List<object>>();

            // 确保源状态规则列表存在
            if (!_stateTransitionRules[statusType].ContainsKey(fromStatus))
                _stateTransitionRules[statusType][fromStatus] = new List<object>();

            // 添加目标状态（如果不存在）
            if (!_stateTransitionRules[statusType][fromStatus].Contains(toStatus))
                _stateTransitionRules[statusType][fromStatus].Add(toStatus);
        }

        /// <summary>
        /// 检查状态转换是否合法
        /// </summary>
        /// <typeparam name="T">状态类型</typeparam>
        /// <param name="fromStatus">源状态</param>
        /// <param name="toStatus">目标状态</param>
        /// <returns>是否合法</returns>
        public bool IsValidTransition<T>(T fromStatus, T toStatus) where T : struct
        {
            // 如果源状态和目标状态相同，则始终合法
            if (EqualityComparer<T>.Default.Equals(fromStatus, toStatus))
                return true;

            var statusType = typeof(T);

            // 检查规则字典是否存在
            if (!_stateTransitionRules.ContainsKey(statusType))
                return false;

            // 检查是否包含源状态的转换规则
            if (!_stateTransitionRules[statusType].TryGetValue(fromStatus, out var validTransitions))
                return false;

            // 检查目标状态是否在有效转换列表中
            return validTransitions.Contains(toStatus);
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

            // 初始化业务状态UI按钮规则
            InitializeBusinessStatusUIButtonRules();
        }

        /// <summary>
        /// 初始化数据状态UI按钮规则
        /// 根据全局提交修改模式设置不同状态下的按钮启用规则
        /// </summary>
        private void InitializeDataStatusUIButtonRules()
        {
            // 为不同状态添加通用按钮规则
            //草稿状态：允许所有操作，除了审核和反审核
            AddStandardButtonRules(DataStatus.草稿, true, true, true, true, true, false, false, false, false);

            // 根据全局提交修改模式设置已新建状态的按钮规则
            // 灵活模式：允许修改；严格模式：不允许修改
            bool allowModifyInSubmittedState = submitModifyRuleMode == SubmitModifyRuleMode.灵活模式;
            AddStandardButtonRules(DataStatus.新建, true, allowModifyInSubmittedState, true, true, false, true, false, false, false);

            /// 确认状态：不允许修改和删除，允许反审核
            AddStandardButtonRules(DataStatus.确认, true, false, false, false, false, false, true, false, false);
            // 完结状态：仅允许查看和打印
            AddStandardButtonRules(DataStatus.完结, true, false, false, false, false, false, false, false, false);

            /// 作废状态：仅允许查看操作
            AddStandardButtonRules(DataStatus.作废, true, false, false, false, false, false, false, false, false);
        }

        /// <summary>
        /// 初始化业务状态UI按钮规则
        /// </summary>
        private void InitializeBusinessStatusUIButtonRules()
        {
            InitializePaymentStatusUIButtonRules();
            InitializePrePaymentStatusUIButtonRules();
            InitializeARAPStatusUIButtonRules();
            InitializeStatementStatusUIButtonRules();
        }

        /// <summary>
        /// 初始化付款状态UI按钮规则
        /// </summary>
        private void InitializePaymentStatusUIButtonRules()
        {
            // 添加付款状态按钮规则
            AddStandardButtonRules(PaymentStatus.草稿, true, true, true, true, true, false, false, false, false);
            AddStandardButtonRules(PaymentStatus.待审核, true, true, true, true, false, true, false, false, false);
            AddStandardButtonRules(PaymentStatus.已支付, false, false, false, false, false, false, false, false, false);
        }

        /// <summary>
        /// 初始化预付款状态UI按钮规则
        /// </summary>
        private void InitializePrePaymentStatusUIButtonRules()
        {
            // 添加预付款状态按钮规则
            AddStandardButtonRules(PrePaymentStatus.草稿, true, true, true, true, true, false, false, false, false);
            AddStandardButtonRules(PrePaymentStatus.待审核, true, true, true, true, false, true, false, false, false);
            AddStandardButtonRules(PrePaymentStatus.已生效, false, false, false, false, false, false, true, false, false);
            AddStandardButtonRules(PrePaymentStatus.待核销, false, true, true, false, false, false, false, false, false);
            AddStandardButtonRules(PrePaymentStatus.部分核销, false, true, true, false, false, false, false, false, false);
            AddStandardButtonRules(PrePaymentStatus.全额核销, false, false, false, false, false, false, true, false, false);
            AddStandardButtonRules(PrePaymentStatus.已结案, false, false, false, false, false, false, false, false, false);
        }

        /// <summary>
        /// 初始化应收应付状态UI按钮规则
        /// </summary>
        private void InitializeARAPStatusUIButtonRules()
        {
            // 添加应收应付状态按钮规则
            AddStandardButtonRules(ARAPStatus.草稿, true, true, true, true, true, false, false, false, false);
            AddStandardButtonRules(ARAPStatus.待审核, true, true, true, true, false, true, false, false, false);
            // 待支付状态：允许查看和打印，不允许修改原始数据
            AddStandardButtonRules(ARAPStatus.待支付, true, false, false, false, false, false, false, false, false);
            // 部分支付状态：允许查看和打印，不允许修改原始数据
            AddStandardButtonRules(ARAPStatus.部分支付, true, false, false, false, false, false, false, false, false);
            // 全部支付状态：终态，只允许查看和打印
            AddStandardButtonRules(ARAPStatus.全部支付, true, false, false, false, false, false, false, false, false);
            // 坏账状态：特殊状态，允许查看和打印，可能需要反审核操作
            AddStandardButtonRules(ARAPStatus.坏账, true, false, false, false, false, false, true, false, false);
            // 已冲销状态：终态，只允许查看和打印
            AddStandardButtonRules(ARAPStatus.已冲销, true, false, false, false, false, false, false, false, false);
        }

        /// <summary>
        /// 初始化对账状态UI按钮规则
        /// </summary>
        private void InitializeStatementStatusUIButtonRules()
        {
            // 草稿状态：允许所有操作，除了审核和反审核
            AddStandardButtonRules(StatementStatus.草稿, true, true, true, true, true, false, false, false, false);
            
            // 根据全局提交修改模式设置已新建状态的按钮规则
            // 灵活模式：允许修改；严格模式：不允许修改
            bool allowModifyInSubmittedState = submitModifyRuleMode == SubmitModifyRuleMode.灵活模式;
            AddStandardButtonRules(StatementStatus.新建, true, allowModifyInSubmittedState, true, true, false, true, false, false, false);

            // 确认状态：不允许修改和删除，允许反审核和部分结算
            AddStandardButtonRules(StatementStatus.确认, true, false, false, false, false, false, true, false, false);

            // 部分结算状态：不允许修改和删除，只允许查看和继续结算操作
            AddStandardButtonRules(StatementStatus.部分结算, true, false, false, false, false, false, false, false, false);
            
            // 全部结清状态：终态，仅允许查看和打印
            AddStandardButtonRules(StatementStatus.全部结清, true, false, false, false, false, false, false, false, false);
            
            // 已作废状态：终态，仅允许查看操作
            AddStandardButtonRules(StatementStatus.已作废, true, false, false, false, false, false, false, false, false);
        }

        /// <summary>
        /// 添加标准按钮规则
        /// </summary>
        /// <typeparam name="T">状态类型</typeparam>
        /// <param name="status">状态值</param>
        /// <param name="addEnabled">添加按钮是否启用</param>
        /// <param name="modifyEnabled">修改按钮是否启用</param>
        /// <param name="saveEnabled">保存按钮是否启用</param>
        /// <param name="deleteEnabled">删除按钮是否启用</param>
        /// <param name="submitEnabled">提交按钮是否启用</param>
        /// <param name="reviewEnabled">审核按钮是否启用</param>
        /// <param name="reverseReviewEnabled">反审核按钮是否启用</param>
        /// <summary>
        /// 添加标准按钮规则
        /// </summary>
        /// <typeparam name="T">状态类型</typeparam>
        /// <param name="status">状态值</param>
        /// <param name="addEnabled">添加按钮是否启用</param>
        /// <param name="modifyEnabled">修改按钮是否启用</param>
        /// <param name="saveEnabled">保存按钮是否启用</param>
        /// <param name="deleteEnabled">删除按钮是否启用</param>
        /// <param name="submitEnabled">提交按钮是否启用</param>
        /// <param name="reviewEnabled">审核按钮是否启用</param>
        /// <param name="reverseReviewEnabled">反审核按钮是否启用</param>
        /// <param name="caseClosedEnabled">结案按钮是否启用</param>
        /// <param name="antiClosedEnabled">反结案按钮是否启用</param>
        /// <param name="exportVisible">导出按钮是否可见</param>
        /// <param name="printVisible">打印按钮是否可见</param>
        private void AddStandardButtonRules<T>(T status, bool addEnabled = false, bool modifyEnabled = false, 
            bool saveEnabled = false, bool deleteEnabled = false, bool submitEnabled = false,
            bool reviewEnabled = false, bool reverseReviewEnabled = false, bool caseClosedEnabled = false,
            bool antiClosedEnabled = false,  bool printVisible = true) where T : struct
        {
            AddButtonRule(status, "toolStripbtnAdd", addEnabled);
            AddButtonRule(status, "toolStripbtnModify", modifyEnabled);
            AddButtonRule(status, "toolStripButtonSave", saveEnabled);
            AddButtonRule(status, "toolStripbtnDelete", deleteEnabled);
            AddButtonRule(status, "toolStripbtnSubmit", submitEnabled);
            AddButtonRule(status, "toolStripbtnReview", reviewEnabled);
            AddButtonRule(status, "toolStripBtnReverseReview", reverseReviewEnabled);
            AddButtonRule(status, "toolStripButtonCaseClosed", caseClosedEnabled);
            AddButtonRule(status, "toolStripButtonAntiClosed", antiClosedEnabled);
            AddButtonRule(status, "toolStripButtonPrint", printVisible);
        }

        /// <summary>
        /// 添加按钮规则
        /// </summary>
        /// <typeparam name="T">状态类型</typeparam>
        /// <param name="status">状态值</param>
        /// <param name="buttonName">按钮名称</param>
        /// <param name="enabled">是否启用</param>
        public void AddButtonRule<T>(T status, string buttonName, bool enabled) where T : struct
        {
            var statusType = typeof(T);

            // 确保规则字典存在
            if (!_uiButtonRules.ContainsKey(statusType))
                _uiButtonRules[statusType] = new Dictionary<object, Dictionary<string, bool>>();

            // 确保状态规则字典存在
            if (!_uiButtonRules[statusType].ContainsKey(status))
                _uiButtonRules[statusType][status] = new Dictionary<string, bool>();

            // 添加按钮规则
            _uiButtonRules[statusType][status][buttonName] = enabled;
        }

        /// <summary>
        /// 获取按钮状态 - 仅返回Enabled状态
        /// 注意：Visible状态由权限系统统一管理，不在此处理
        /// </summary>
        /// <typeparam name="T">状态类型</typeparam>
        /// <param name="status">状态值</param>
        /// <param name="buttonName">按钮名称</param>
        /// <returns>按钮Enabled状态（默认禁用）</returns>
        public bool GetButtonState<T>(T status, string buttonName) where T : struct
        {
            var statusType = typeof(T);

            // 检查规则字典是否存在
            if (!_uiButtonRules.ContainsKey(statusType))
                return false;

            // 检查状态规则字典是否存在
            if (!_uiButtonRules[statusType].TryGetValue(status, out var buttonRules))
                return false;

            // 检查按钮规则是否存在
            if (!buttonRules.TryGetValue(buttonName, out var enabled))
                return false;

            return enabled;
        }


        /// <summary>
        /// 获取指定状态类型和状态值的按钮规则
        /// 自动处理int值到枚举类型的转换
        /// 注意：状态管理只控制按钮的可用性(Enabled)，Visible由权限系统管理
        /// </summary>
        /// <param name="statusType">状态类型</param>
        /// <param name="status">状态值</param>
        /// <returns>按钮规则字典，键为按钮名称，值为Enabled状态</returns>
        public Dictionary<string, bool> GetButtonRules(Type statusType, object status)
        {
            if (statusType == null || status == null)
                return new Dictionary<string, bool>();

            // 自动转换：如果状态值是int且状态类型是枚举，则进行转换
            object workingStatus = status;
            if (status is int && statusType.IsEnum)
            {
                try
                {
                    workingStatus = Enum.ToObject(statusType, status);
                }
                catch
                {
                    // 转换失败时使用原始状态值
                }
            }

            if (_uiButtonRules.ContainsKey(statusType) && _uiButtonRules[statusType].ContainsKey(workingStatus))
            {
                return _uiButtonRules[statusType][workingStatus];
            }

            return new Dictionary<string, bool>();
        }


        /// <summary>
        /// 获取指定状态下的所有按钮规则 - 仅包含Enabled状态
        /// 注意：Visible状态由权限系统统一管理，不在此处理
        /// </summary>
        /// <typeparam name="T">状态类型</typeparam>
        /// <param name="status">状态值</param>
        /// <returns>按钮规则字典，键为按钮名称，值为Enabled状态</returns>
        public Dictionary<string, bool> GetButtonRules<T>(T status) where T : struct
        {
            var statusType = typeof(T);

            // 检查规则字典是否存在
            if (!_uiButtonRules.ContainsKey(statusType))
                return new Dictionary<string, bool>();

            // 检查状态规则字典是否存在
            if (!_uiButtonRules[statusType].TryGetValue(status, out var buttonRules))
                return new Dictionary<string, bool>();

            // 返回按钮规则的副本
            return new Dictionary<string, bool>(buttonRules);
        }

        /// <summary>
        /// 检查提交后是否允许修改
        /// 根据全局模式设置和状态判断
        /// </summary>
        /// <param name="isSubmittedStatus">是否为已提交状态</param>
        /// <returns>是否允许修改</returns>
        public bool AllowModifyAfterSubmit(bool isSubmittedStatus)
        {
            // 如果不是已提交状态，始终允许修改
            if (!isSubmittedStatus)
                return true;

            // 根据全局模式设置判断
            // 严格模式：提交后不允许修改
            // 灵活模式：提交后允许修改
            return submitModifyRuleMode == SubmitModifyRuleMode.灵活模式;
        }

        #endregion

        #region 操作权限规则方法

        /// <summary>
        /// 初始化操作权限规则
        /// </summary>
        private void InitializeActionPermissionRules()
        {
            // 为DataStatus添加操作权限规则
            AddDataStatusActionPermissionRules();

            // 为其他状态类型添加操作权限规则
            AddBusinessStatusActionPermissionRules();
        }

        /// <summary>
        /// 添加DataStatus操作权限规则
        /// </summary>
        private void AddDataStatusActionPermissionRules()
        {
            var statusType = typeof(DataStatus);
            if (submitModifyRuleMode == SubmitModifyRuleMode.灵活模式)
            {
                _actionPermissionRules[statusType] = new Dictionary<object, List<MenuItemEnums>>
                {
                    [DataStatus.草稿] = new List<MenuItemEnums> { MenuItemEnums.新增, MenuItemEnums.修改, MenuItemEnums.删除, MenuItemEnums.提交 },
                    [DataStatus.新建] = new List<MenuItemEnums> { MenuItemEnums.新增, MenuItemEnums.修改, MenuItemEnums.删除, MenuItemEnums.审核 },
                    [DataStatus.确认] = new List<MenuItemEnums> { MenuItemEnums.新增, MenuItemEnums.反审, MenuItemEnums.结案 },
                    [DataStatus.完结] = new List<MenuItemEnums> { MenuItemEnums.新增, MenuItemEnums.反结案 },
                    [DataStatus.作废] = new List<MenuItemEnums> { MenuItemEnums.新增 }
                };
            }
            else
            {
                _actionPermissionRules[statusType] = new Dictionary<object, List<MenuItemEnums>>
                {
                    [DataStatus.草稿] = new List<MenuItemEnums> { MenuItemEnums.新增, MenuItemEnums.修改, MenuItemEnums.删除, MenuItemEnums.提交 },
                    [DataStatus.新建] = new List<MenuItemEnums> { MenuItemEnums.新增, MenuItemEnums.删除, MenuItemEnums.审核 },//不能修改
                    [DataStatus.确认] = new List<MenuItemEnums> { MenuItemEnums.新增, MenuItemEnums.反审, MenuItemEnums.结案 },
                    [DataStatus.完结] = new List<MenuItemEnums> { MenuItemEnums.新增, MenuItemEnums.反结案 },
                    [DataStatus.作废] = new List<MenuItemEnums> { MenuItemEnums.新增 }
                };
            }

        }

        /// <summary>
        /// 添加业务状态操作权限规则
        /// </summary>
        private void AddBusinessStatusActionPermissionRules()
        {
            // 为PaymentStatus添加操作权限规则
            AddPaymentStatusActionPermissionRules();

            // 为PrePaymentStatus添加操作权限规则
            AddPrePaymentStatusActionPermissionRules();

            // 为StatementStatus添加操作权限规则
            AddStatementStatusActionPermissionRules();

            // 为ARAPStatus添加操作权限规则
            AddARAPStatusActionPermissionRules();
        }

        /// <summary>
        /// 添加PaymentStatus操作权限规则
        /// </summary>
        private void AddPaymentStatusActionPermissionRules()
        {
            var statusType = typeof(PaymentStatus);

            if (submitModifyRuleMode == SubmitModifyRuleMode.灵活模式)
            {
                _actionPermissionRules[statusType] = new Dictionary<object, List<MenuItemEnums>>
                {
                    [PaymentStatus.草稿] = new List<MenuItemEnums> { MenuItemEnums.新增, MenuItemEnums.修改, MenuItemEnums.删除, MenuItemEnums.提交 },
                    [PaymentStatus.待审核] = new List<MenuItemEnums> { MenuItemEnums.新增, MenuItemEnums.修改, MenuItemEnums.删除, MenuItemEnums.审核 },
                    [PaymentStatus.已支付] = new List<MenuItemEnums> { MenuItemEnums.打印 }
                };
            }
            else
            {
                _actionPermissionRules[statusType] = new Dictionary<object, List<MenuItemEnums>>
                {
                    [PaymentStatus.草稿] = new List<MenuItemEnums> { MenuItemEnums.新增, MenuItemEnums.修改, MenuItemEnums.删除, MenuItemEnums.提交 },
                    [PaymentStatus.待审核] = new List<MenuItemEnums> { MenuItemEnums.新增, MenuItemEnums.删除, MenuItemEnums.审核 },
                    [PaymentStatus.已支付] = new List<MenuItemEnums> { MenuItemEnums.打印 }
                };
            }
        }

        /// <summary>
        /// 添加PrePaymentStatus操作权限规则
        /// </summary>
        private void AddPrePaymentStatusActionPermissionRules()
        {
            var statusType = typeof(PrePaymentStatus);
            if (submitModifyRuleMode == SubmitModifyRuleMode.灵活模式)
            {
                _actionPermissionRules[statusType] = new Dictionary<object, List<MenuItemEnums>>
                {
                    [PrePaymentStatus.草稿] = new List<MenuItemEnums> { MenuItemEnums.新增, MenuItemEnums.修改, MenuItemEnums.删除, MenuItemEnums.提交 },
                    [PrePaymentStatus.待审核] = new List<MenuItemEnums> { MenuItemEnums.新增, MenuItemEnums.修改, MenuItemEnums.删除, MenuItemEnums.审核 },
                    [PrePaymentStatus.已生效] = new List<MenuItemEnums> { MenuItemEnums.反审 },
                    [PrePaymentStatus.待核销] = new List<MenuItemEnums> { MenuItemEnums.反审 },
                    [PrePaymentStatus.部分核销] = new List<MenuItemEnums> { },
                    [PrePaymentStatus.全额核销] = new List<MenuItemEnums> { },
                    [PrePaymentStatus.已结案] = new List<MenuItemEnums> { }
                };
            }
            else
            {
                _actionPermissionRules[statusType] = new Dictionary<object, List<MenuItemEnums>>
                {
                    [PrePaymentStatus.草稿] = new List<MenuItemEnums> { MenuItemEnums.新增, MenuItemEnums.修改, MenuItemEnums.删除, MenuItemEnums.提交 },
                    [PrePaymentStatus.待审核] = new List<MenuItemEnums> { MenuItemEnums.新增, MenuItemEnums.删除, MenuItemEnums.审核 },
                    [PrePaymentStatus.已生效] = new List<MenuItemEnums> { MenuItemEnums.反审 },
                    [PrePaymentStatus.待核销] = new List<MenuItemEnums> { MenuItemEnums.反审 },
                    [PrePaymentStatus.部分核销] = new List<MenuItemEnums> { },
                    [PrePaymentStatus.全额核销] = new List<MenuItemEnums> { },
                    [PrePaymentStatus.已结案] = new List<MenuItemEnums> { }
                };
            }
        }

        /// <summary>
        /// 添加StatementStatus操作权限规则
        /// </summary>
        private void AddStatementStatusActionPermissionRules()
        {
            var statusType = typeof(StatementStatus);
            if (submitModifyRuleMode == SubmitModifyRuleMode.灵活模式)
            {
                _actionPermissionRules[statusType] = new Dictionary<object, List<MenuItemEnums>>
                {
                    // 草稿状态：允许所有基本操作
                    [StatementStatus.草稿] = new List<MenuItemEnums> { MenuItemEnums.新增, MenuItemEnums.修改, MenuItemEnums.删除, MenuItemEnums.提交 },
                    
                    // 新建状态：允许基本操作和审核（灵活模式下允许修改）
                    [StatementStatus.新建] = new List<MenuItemEnums> { MenuItemEnums.新增, MenuItemEnums.修改, MenuItemEnums.删除, MenuItemEnums.审核 },
                    
                    // 确认状态：允许反审核操作
                    [StatementStatus.确认] = new List<MenuItemEnums> { MenuItemEnums.反审 },
                    
                    // 部分结算状态：允许继续结算操作
                    [StatementStatus.部分结算] = new List<MenuItemEnums> { MenuItemEnums.结案 },
                    
                    // 全部结清和已作废是终态，不允许操作
                    [StatementStatus.全部结清] = new List<MenuItemEnums> { },
                    [StatementStatus.已作废] = new List<MenuItemEnums> { }
                };
            }
            else
            {
                _actionPermissionRules[statusType] = new Dictionary<object, List<MenuItemEnums>>
                {
                    // 草稿状态：允许所有基本操作
                    [StatementStatus.草稿] = new List<MenuItemEnums> { MenuItemEnums.新增, MenuItemEnums.修改, MenuItemEnums.删除, MenuItemEnums.提交 },
                    
                    // 新建状态：允许基本操作和审核（严格模式下不允许修改）
                    [StatementStatus.新建] = new List<MenuItemEnums> { MenuItemEnums.新增, MenuItemEnums.删除, MenuItemEnums.审核 },
                    
                    // 确认状态：允许反审核操作
                    [StatementStatus.确认] = new List<MenuItemEnums> { MenuItemEnums.反审 },
                    
                    // 部分结算状态：允许继续结算操作
                    [StatementStatus.部分结算] = new List<MenuItemEnums> { MenuItemEnums.结案 },
                    
                    // 全部结清和已作废是终态，不允许操作
                    [StatementStatus.全部结清] = new List<MenuItemEnums> { },
                    [StatementStatus.已作废] = new List<MenuItemEnums> { }
                };
            }
        }

        /// <summary>
        /// 添加ARAPStatus操作权限规则
        /// </summary>
        private void AddARAPStatusActionPermissionRules()
        {
            var statusType = typeof(ARAPStatus);
            if (submitModifyRuleMode == SubmitModifyRuleMode.灵活模式)
            {
                _actionPermissionRules[statusType] = new Dictionary<object, List<MenuItemEnums>>
                {
                    [ARAPStatus.草稿] = new List<MenuItemEnums> { MenuItemEnums.新增, MenuItemEnums.修改, MenuItemEnums.删除, MenuItemEnums.提交 },
                    [ARAPStatus.待审核] = new List<MenuItemEnums> { MenuItemEnums.新增, MenuItemEnums.修改, MenuItemEnums.删除, MenuItemEnums.审核 },
                    [ARAPStatus.待支付] = new List<MenuItemEnums> { MenuItemEnums.新增 },
                    [ARAPStatus.部分支付] = new List<MenuItemEnums> { MenuItemEnums.新增 },
                    [ARAPStatus.全部支付] = new List<MenuItemEnums> { },
                    [ARAPStatus.坏账] = new List<MenuItemEnums> { },
                    [ARAPStatus.已冲销] = new List<MenuItemEnums> { }
                };
            }
            else
            {
                _actionPermissionRules[statusType] = new Dictionary<object, List<MenuItemEnums>>
                {
                    [ARAPStatus.草稿] = new List<MenuItemEnums> { MenuItemEnums.新增, MenuItemEnums.修改, MenuItemEnums.删除, MenuItemEnums.提交 },
                    [ARAPStatus.待审核] = new List<MenuItemEnums> { MenuItemEnums.新增, MenuItemEnums.删除, MenuItemEnums.审核 },
                    [ARAPStatus.待支付] = new List<MenuItemEnums> { MenuItemEnums.新增 },
                    [ARAPStatus.部分支付] = new List<MenuItemEnums> { MenuItemEnums.新增 },
                    [ARAPStatus.全部支付] = new List<MenuItemEnums> { },
                    [ARAPStatus.坏账] = new List<MenuItemEnums> { },
                    [ARAPStatus.已冲销] = new List<MenuItemEnums> { }
                };
            }
        }

        /// <summary>
        /// 添加操作权限规则
        /// </summary>
        /// <typeparam name="T">状态类型</typeparam>
        /// <param name="status">状态值</param>
        /// <param name="action">操作类型</param>
        public void AddActionPermissionRule<T>(T status, MenuItemEnums action) where T : struct
        {
            var statusType = typeof(T);

            // 确保规则字典存在
            if (!_actionPermissionRules.ContainsKey(statusType))
                _actionPermissionRules[statusType] = new Dictionary<object, List<MenuItemEnums>>();

            // 确保状态规则列表存在
            if (!_actionPermissionRules[statusType].ContainsKey(status))
                _actionPermissionRules[statusType][status] = new List<MenuItemEnums>();

            // 添加操作权限（如果不存在）
            if (!_actionPermissionRules[statusType][status].Contains(action))
                _actionPermissionRules[statusType][status].Add(action);
        }

        /// <summary>
        /// 检查是否允许执行特定操作
        /// </summary>
        /// <typeparam name="T">状态类型</typeparam>
        /// <param name="status">状态值</param>
        /// <param name="action">操作类型</param>
        /// <returns>是否允许执行操作</returns>
        public bool CanExecuteAction<T>(T status, MenuItemEnums action) where T : struct
        {
            var statusType = typeof(T);

            // 检查规则字典是否存在
            if (!_actionPermissionRules.ContainsKey(statusType))
                return false;

            // 检查状态规则列表是否存在
            if (!_actionPermissionRules[statusType].TryGetValue(status, out var allowedActions))
                return false;

            // 检查是否允许执行操作
            return allowedActions.Contains(action);
        }

        #endregion

        #region 新增方法 - 状态终态判断

        /// <summary>
        /// 判断指定状态是否为终态
        /// </summary>
        /// <typeparam name="TStatus">状态类型</typeparam>
        /// <param name="status">状态值</param>
        /// <returns>是否为终态</returns>
        public bool IsFinalStatus<TStatus>(TStatus status) where TStatus : struct
        {
            var statusType = typeof(TStatus);

            // 针对不同状态类型的终态判断
            if (statusType == typeof(DataStatus))
            {
                DataStatus dataStatus = (DataStatus)(object)status;
                return dataStatus == DataStatus.完结 || dataStatus == DataStatus.作废;
            }
            else if (statusType == typeof(PaymentStatus))
            {
                PaymentStatus paymentStatus = (PaymentStatus)(object)status;
                return paymentStatus == PaymentStatus.已支付;
            }
            else if (statusType == typeof(RefundStatus))
            {
                RefundStatus refundStatus = (RefundStatus)(object)status;
                return refundStatus == RefundStatus.已退款已退货 || refundStatus == RefundStatus.已退款未退货 || refundStatus == RefundStatus.部分退款退货;
            }
            else if (statusType == typeof(PrePaymentStatus))
            {
                PrePaymentStatus prepayStatus = (PrePaymentStatus)(object)status;
                return prepayStatus == PrePaymentStatus.已结案;
            }
            else if (statusType == typeof(ARAPStatus))
            {
                ARAPStatus arapStatus = (ARAPStatus)(object)status;
                return arapStatus == ARAPStatus.全部支付 || arapStatus == ARAPStatus.已冲销;
            }
            else if (statusType == typeof(StatementStatus))
            {
                StatementStatus statementStatus = (StatementStatus)(object)status;
                return statementStatus == StatementStatus.全部结清 || statementStatus == StatementStatus.已作废;
            }

            // 默认情况下，不是终态
            return false;
        }

        /// <summary>
        /// 判断指定状态是否可以提交
        /// </summary>
        /// <typeparam name="TStatus">状态类型</typeparam>
        /// <param name="status">状态值</param>
        /// <returns>是否可以提交</returns>
        public bool CanSubmit<TStatus>(TStatus status) where TStatus : struct
        {
            // 终态不能提交
            if (IsFinalStatus(status))
                return false;

            var statusType = typeof(TStatus);

            // 根据不同状态类型判断是否可以提交
            if (statusType == typeof(DataStatus))
            {
                DataStatus dataStatus = (DataStatus)(object)status;
                return dataStatus == DataStatus.草稿;
            }
            else if (statusType == typeof(PaymentStatus))
            {
                PaymentStatus paymentStatus = (PaymentStatus)(object)status;
                return paymentStatus == PaymentStatus.草稿;
            }
            else if (statusType == typeof(PrePaymentStatus))
            {
                PrePaymentStatus prepayStatus = (PrePaymentStatus)(object)status;
                return prepayStatus == PrePaymentStatus.草稿;
            }
            else if (statusType == typeof(ARAPStatus))
            {
                ARAPStatus arapStatus = (ARAPStatus)(object)status;
                return arapStatus == ARAPStatus.草稿;
            }
            else if (statusType == typeof(StatementStatus))
            {
                StatementStatus statementStatus = (StatementStatus)(object)status;
                return statementStatus == StatementStatus.草稿;
            }

            // 其他状态类型默认可提交条件：检查是否有提交相关的状态转换规则
            // 或者检查是否有提交操作权限
            return CanExecuteAction(status, MenuItemEnums.提交) ||
                   (_stateTransitionRules.ContainsKey(statusType) &&
                    _stateTransitionRules[statusType].ContainsKey(status) &&
                    _stateTransitionRules[statusType][status].Count > 0);
        }

        /// <summary>
        /// 判断指定状态是否可以审核
        /// </summary>
        /// <typeparam name="TStatus">状态类型</typeparam>
        /// <param name="status">状态值</param>
        /// <returns>是否可以审核</returns>
        public bool CanApprove<TStatus>(TStatus status) where TStatus : struct
        {
            // 终态不能审核
            if (IsFinalStatus(status))
                return false;

            var statusType = typeof(TStatus);

            // 根据不同状态类型判断是否可以审核
            if (statusType == typeof(DataStatus))
            {
                DataStatus dataStatus = (DataStatus)(object)status;
                return dataStatus == DataStatus.新建;
            }
            else if (statusType == typeof(StatementStatus))
            {
                StatementStatus statementStatus = (StatementStatus)(object)status;
                return statementStatus == StatementStatus.新建;
            }
            else if (statusType == typeof(PaymentStatus))
            {
                PaymentStatus paymentStatus = (PaymentStatus)(object)status;
                return paymentStatus == PaymentStatus.待审核;
            }
            else if (statusType == typeof(PrePaymentStatus))
            {
                PrePaymentStatus prepayStatus = (PrePaymentStatus)(object)status;
                return prepayStatus == PrePaymentStatus.待审核;
            }
            else if (statusType == typeof(ARAPStatus))
            {
                ARAPStatus arapStatus = (ARAPStatus)(object)status;
                return arapStatus == ARAPStatus.待审核;
            }

            // 通用判断：检查是否有待审核状态或有审核操作权限
            return CanExecuteAction(status, MenuItemEnums.审核);
        }

        /// <summary>
        /// 获取状态类型的描述信息
        /// </summary>
        /// <param name="statusType">状态类型</param>
        /// <returns>状态类型描述</returns>
        public string GetStatusTypeDescription(Type statusType)
        {
            if (statusType == typeof(DataStatus))
                return "数据状态";
            else if (statusType == typeof(ActionStatus))
                return "操作状态";
            else if (statusType == typeof(PaymentStatus))
                return "付款状态";
            else if (statusType == typeof(RefundStatus))
                return "退款状态";
            else if (statusType == typeof(PrePaymentStatus))
                return "预付款状态";
            else if (statusType == typeof(ARAPStatus))
                return "应收应付状态";
            else if (statusType == typeof(StatementStatus))
                return "对账状态";

            return statusType.Name;
        }

        #endregion

        #region 新增方法 - 退款状态规则初始化

        /// <summary>
        /// 初始化退款状态转换规则
        /// </summary>
        public void InitializeRefundStatusTransitionRules()
        {
            var statusType = typeof(RefundStatus);
            _stateTransitionRules[statusType] = new Dictionary<object, List<object>>();

            // 初始化不同状态的转换规则
            // 未退款等待退货 -> 可转换到已退款等待退货、未退款已退货、部分退款退货
            _stateTransitionRules[statusType][RefundStatus.未退款等待退货] = new List<object>
            {
                RefundStatus.已退款等待退货,
                RefundStatus.未退款已退货,
                RefundStatus.部分退款退货
            };

            // 未退款已退货 -> 可转换到已退款已退货、部分退款退货
            _stateTransitionRules[statusType][RefundStatus.未退款已退货] = new List<object>
            {
                RefundStatus.已退款已退货,
                RefundStatus.部分退款退货
            };

            // 已退款等待退货 -> 可转换到已退款已退货、部分退款退货
            _stateTransitionRules[statusType][RefundStatus.已退款等待退货] = new List<object>
            {
                RefundStatus.已退款已退货,
                RefundStatus.部分退款退货
            };

            // 已退款未退货 -> 可转换到已退款已退货、部分退款退货
            _stateTransitionRules[statusType][RefundStatus.已退款未退货] = new List<object>
            {
                RefundStatus.已退款已退货,
                RefundStatus.部分退款退货
            };

            // 终态状态不可转换
            _stateTransitionRules[statusType][RefundStatus.已退款已退货] = new List<object>();
            _stateTransitionRules[statusType][RefundStatus.部分退款退货] = new List<object>();
        }

        #endregion
    }
}
