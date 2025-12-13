/**
 * 文件: StateTransitionRules.cs
 * 版本: V4 - 统一状态转换规则管理类
 * 说明: 统一状态转换规则管理类 - 适配V4版本的EntityStatus和StateTransitionResult
 * 创建日期: 2024年
 * 作者: RUINOR ERP开发团队
 * 更新日期: 2025-01-12 - V4版本适配EntityStatus和StateTransitionResult
 */

using RUINORERP.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;
using RUINORERP.Global.EnumExt;

namespace RUINORERP.Model.Base.StatusManager
{
    /// <summary>
    /// 统一状态转换规则管理类
    /// </summary>
    public static class StateTransitionRules
    {
        /// <summary>
        /// 线程安全的延迟加载状态转换规则实例
        /// </summary>
        private static readonly Lazy<Dictionary<Type, Dictionary<object, List<object>>>> _lazyTransitionRules =
            new Lazy<Dictionary<Type, Dictionary<object, List<object>>>>(InitializeDefaultRules, true);

        /// <summary>
        /// 获取全局唯一的状态转换规则实例
        /// </summary>
        public static Dictionary<Type, Dictionary<object, List<object>>> Instance => _lazyTransitionRules.Value;

        /// <summary>
        /// 初始化默认状态转换规则（保持向后兼容）
        /// </summary>
        /// <param name="transitionRules">转换规则字典</param>
        [Obsolete("此方法已过时，请使用Instance属性获取规则实例")]
        public static void InitializeDefaultRules(Dictionary<Type, Dictionary<object, List<object>>> transitionRules)
        {
            if (transitionRules == null)
                throw new ArgumentNullException(nameof(transitionRules));

            // 从全局实例复制规则到传入的字典
            var globalRules = Instance;
            foreach (var kvp in globalRules)
            {
                transitionRules[kvp.Key] = new Dictionary<object, List<object>>(kvp.Value);
            }
        }

        /// <summary>
        /// 初始化默认状态转换规则（内部使用）
        /// </summary>
        /// <returns>转换规则字典</returns>
        private static Dictionary<Type, Dictionary<object, List<object>>> InitializeDefaultRules()
        {
            var transitionRules = new Dictionary<Type, Dictionary<object, List<object>>>();

            // 初始化DataStatus转换规则
            InitializeDataStatusRules(transitionRules);

            // 初始化ActionStatus转换规则
            InitializeActionStatusRules(transitionRules);

            // 初始化业务状态转换规则
            InitializeBusinessStatusRules(transitionRules);

            return transitionRules;
        }

        /// <summary>
        /// 初始化业务状态转换规则
        /// </summary>
        /// <param name="transitionRules">转换规则字典</param>
        private static void InitializeBusinessStatusRules(Dictionary<Type, Dictionary<object, List<object>>> transitionRules)
        {
            // 初始化付款状态转换规则
            InitializePaymentStatusRules(transitionRules);

            // 初始化预付款状态转换规则
            InitializePrePaymentStatusRules(transitionRules);

            // 初始化应收应付状态转换规则
            InitializeARAPStatusRules(transitionRules);

            // 初始化对账状态转换规则
            InitializeStatementStatusRules(transitionRules);
        }




        /*
                /// <summary>
                /// 获取实体状态的转换规则（V4版本完整实现）
                /// </summary>
                /// <param name="transitionRules">转换规则字典</param>
                /// <param name="entityStatus">实体状态</param>
                /// <returns>可转换的状态列表</returns>
                public static List<(StatusType statusType, Enum status)> GetAvailableTransitionsForEntityStatusV4(
                    Dictionary<Type, Dictionary<object, List<object>>> transitionRules,
                    EntityStatus entityStatus)
                {
                    var result = new List<(StatusType, Enum)>();

                    if (transitionRules == null || entityStatus == null)
                        return result;

                    // 获取当前状态类型和值
                    var currentStatusType = entityStatus.CurrentStatusType;
                    var currentStatus = entityStatus.CurrentStatus;

                    // 如果当前没有状态，可以设置任何状态
                    if (currentStatusType == null || currentStatus == null)
                    {
                        // 添加所有DataStatus状态
                        foreach (DataStatus status in Enum.GetValues(typeof(DataStatus)))
                        {
                            result.Add((StatusType.Primary, status));
                        }

                        // 添加所有业务状态
                        AddAllBusinessStatuses(result);

                        // 添加所有ActionStatus状态
                        foreach (ActionStatus status in Enum.GetValues(typeof(ActionStatus)))
                        {
                            result.Add((StatusType.Action, status));
                        }

                        return result;
                    }

                    // 根据当前状态类型获取可转换的状态
                    switch (currentStatusType)
                    {
                        case StatusType.Primary:
                            // Primary状态（DataStatus或业务状态）
                            if (currentStatus is DataStatus dataStatus)
                            {
                                // 获取DataStatus的可用转换
                                var availableDataStatuses = GetAvailableTransitions(transitionRules, dataStatus);
                                foreach (var status in availableDataStatuses)
                                {
                                    result.Add((StatusType.Primary, status));
                                }
                            }
                            else if (IsBusinessStatus(currentStatus))
                            {
                                // 获取业务状态的可用转换
                                var availableBusinessStatuses = GetAvailableTransitions(transitionRules, currentStatus);
                                foreach (var status in availableBusinessStatuses)
                                {
                                    result.Add((StatusType.Primary, status));
                                }
                            }
                            break;

                        case StatusType.Action:
                            // Action状态的可用转换
                            if (currentStatus is ActionStatus actionStatus)
                            {
                                var availableActionStatuses = GetAvailableTransitions(transitionRules, actionStatus);
                                foreach (var status in availableActionStatuses)
                                {
                                    result.Add((StatusType.Action, status));
                                }
                            }
                            break;

                        case StatusType.Approval:
                            // Approval状态的可用转换
                            // 这里可以根据需要实现Approval状态的转换规则
                            break;

                        case StatusType.ApprovalResult:
                            // ApprovalResult状态的可用转换
                            // 这里可以根据需要实现ApprovalResult状态的转换规则
                            break;
                    }

                    return result;
                }
        */


        /// <summary>
        /// 添加所有业务状态到结果列表
        /// </summary>
        /// <param name="result">结果列表</param>
        private static void AddAllBusinessStatuses(List<(StatusType, Enum)> result)
        {
            // 添加所有PaymentStatus状态
            foreach (PaymentStatus status in Enum.GetValues(typeof(PaymentStatus)))
            {
                result.Add((StatusType.Primary, status));
            }

            // 添加所有PrePaymentStatus状态
            foreach (PrePaymentStatus status in Enum.GetValues(typeof(PrePaymentStatus)))
            {
                result.Add((StatusType.Primary, status));
            }

            // 添加所有ARAPStatus状态
            foreach (ARAPStatus status in Enum.GetValues(typeof(ARAPStatus)))
            {
                result.Add((StatusType.Primary, status));
            }

            // 添加所有StatementStatus状态
            foreach (StatementStatus status in Enum.GetValues(typeof(StatementStatus)))
            {
                result.Add((StatusType.Primary, status));
            }
        }



        /// <summary>
        /// 判断状态是否为业务状态
        /// </summary>
        /// <param name="status">状态</param>
        /// <returns>是否为业务状态</returns>
        private static bool IsBusinessStatus(Enum status)
        {
            // 检查是否为已知的业务状态类型
            Type statusType = status.GetType();
            return statusType == typeof(PaymentStatus) ||
                   statusType == typeof(PrePaymentStatus) ||
                   statusType == typeof(ARAPStatus) ||
                   statusType == typeof(StatementStatus);
        }

        /// <summary>
        /// 验证状态类型转换是否合法
        /// </summary>
        /// <param name="fromType">源状态类型</param>
        /// <param name="toType">目标状态类型</param>
        /// <returns>是否允许转换</returns>
        private static bool IsStatusTypeTransitionAllowed(StatusType fromType, StatusType toType)
        {
            // 相同状态类型总是允许转换
            if (fromType == toType)
                return true;

            // 从空状态可以转换到任何状态
            if (fromType == null)
                return true;

            // Primary类型状态之间不能直接转换（需要通过清除当前状态）
            if (fromType == StatusType.Primary && toType == StatusType.Primary)
                return false;

            // 其他状态类型转换规则
            switch (fromType)
            {
                case StatusType.Action:
                    // Action状态可以转换到任何状态
                    return true;

                case StatusType.Approval:
                    // Approval状态可以转换到任何状态
                    return true;

                case StatusType.ApprovalResult:
                    // ApprovalResult状态可以转换到任何状态
                    return true;

                default:
                    // 默认情况下，不同状态类型之间不允许直接转换
                    return false;
            }
        }

        /// <summary>
        /// 验证实体状态转换是否合法
        /// </summary>
        /// <param name="transitionRules">转换规则字典</param>
        /// <param name="entityStatus">实体状态</param>
        /// <param name="targetStatus">目标状态</param>
        /// <returns>状态转换结果</returns>
        public static StateTransitionResult IsEntityStatusTransitionAllowed(
            Dictionary<Type, Dictionary<object, List<object>>> transitionRules,
            EntityStatus entityStatus,
            Enum targetStatus)
        {
            if (transitionRules == null || entityStatus == null || targetStatus == null)
                return StateTransitionResult.Denied("参数不能为空");

            // 获取当前状态类型和值
            var statusType = entityStatus.CurrentStatusType;
            var currentStatus = entityStatus.CurrentStatus;

            if (statusType == null || currentStatus == null)
                return StateTransitionResult.Denied("当前状态无效");

            // 检查目标状态类型是否与当前状态类型一致
            if (statusType != targetStatus.GetType())
                return StateTransitionResult.Denied($"状态类型不匹配，当前状态类型为{statusType.Name}，目标状态类型为{targetStatus.GetType().Name}");

            // 验证状态转换
            bool isAllowed = IsTransitionAllowed(transitionRules, statusType, currentStatus, targetStatus);

            if (isAllowed)
            {
                return StateTransitionResult.Allowed();
            }
            else
            {
                return StateTransitionResult.Denied($"不允许从{currentStatus}转换到{targetStatus}");
            }
        }

        /// <summary>
        /// 初始化DataStatus转换规则
        /// </summary>
        /// <param name="transitionRules">转换规则字典</param>
        private static void InitializeDataStatusRules(Dictionary<Type, Dictionary<object, List<object>>> transitionRules)
        {
            transitionRules[typeof(DataStatus)] = new Dictionary<object, List<object>>
            {
                [DataStatus.草稿] = new List<object> { DataStatus.新建, DataStatus.作废 },
                [DataStatus.新建] = new List<object> { DataStatus.确认, DataStatus.作废 },
                [DataStatus.确认] = new List<object> { DataStatus.完结, DataStatus.作废 },
                [DataStatus.完结] = new List<object> { }, // 完结状态不能再转换
                [DataStatus.作废] = new List<object> { DataStatus.草稿 } // 作废状态可以重新激活为草稿
            };
        }

        /// <summary>
        /// 初始化ActionStatus转换规则
        /// </summary>
        /// <param name="transitionRules">转换规则字典</param>
        private static void InitializeActionStatusRules(Dictionary<Type, Dictionary<object, List<object>>> transitionRules)
        {
            transitionRules[typeof(ActionStatus)] = new Dictionary<object, List<object>>
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
        /// 初始化付款状态转换规则
        /// </summary>
        /// <param name="transitionRules">转换规则字典</param>
        private static void InitializePaymentStatusRules(Dictionary<Type, Dictionary<object, List<object>>> transitionRules)
        {
            var paymentStatusType = typeof(RUINORERP.Global.EnumExt.PaymentStatus);
            if (!transitionRules.ContainsKey(paymentStatusType))
            {
                transitionRules[paymentStatusType] = new Dictionary<object, List<object>>();
            }

            var rules = transitionRules[paymentStatusType];

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
        /// <param name="transitionRules">转换规则字典</param>
        private static void InitializePrePaymentStatusRules(Dictionary<Type, Dictionary<object, List<object>>> transitionRules)
        {
            var prePaymentStatusType = typeof(RUINORERP.Global.EnumExt.PrePaymentStatus);
            if (!transitionRules.ContainsKey(prePaymentStatusType))
            {
                transitionRules[prePaymentStatusType] = new Dictionary<object, List<object>>();
            }

            var rules = transitionRules[prePaymentStatusType];

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
        /// <param name="transitionRules">转换规则字典</param>
        private static void InitializeARAPStatusRules(Dictionary<Type, Dictionary<object, List<object>>> transitionRules)
        {
            var arapStatusType = typeof(RUINORERP.Global.EnumExt.ARAPStatus);
            if (!transitionRules.ContainsKey(arapStatusType))
            {
                transitionRules[arapStatusType] = new Dictionary<object, List<object>>();
            }

            var rules = transitionRules[arapStatusType];

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
        /// <param name="transitionRules">转换规则字典</param>
        private static void InitializeStatementStatusRules(Dictionary<Type, Dictionary<object, List<object>>> transitionRules)
        {
            var statementStatusType = typeof(RUINORERP.Global.EnumExt.StatementStatus);
            if (!transitionRules.ContainsKey(statementStatusType))
            {
                transitionRules[statementStatusType] = new Dictionary<object, List<object>>();
            }

            var rules = transitionRules[statementStatusType];

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

        /// <summary>
        /// 添加状态转换规则
        /// </summary>
        /// <param name="transitionRules">转换规则字典</param>
        /// <param name="fromStatus">源状态</param>
        /// <param name="toStatus">目标状态</param>
        public static void AddTransitionRule(Dictionary<Type, Dictionary<object, List<object>>> transitionRules, Enum fromStatus, Enum toStatus)
        {
            if (transitionRules == null || fromStatus == null || toStatus == null)
                return;

            var statusType = fromStatus.GetType();

            if (!transitionRules.ContainsKey(statusType))
            {
                transitionRules[statusType] = new Dictionary<object, List<object>>();
            }

            if (!transitionRules[statusType].ContainsKey(fromStatus))
            {
                transitionRules[statusType][fromStatus] = new List<object>();
            }

            if (!transitionRules[statusType][fromStatus].Contains(toStatus))
            {
                transitionRules[statusType][fromStatus].Add(toStatus);
            }
        }

        /// <summary>
        /// 验证状态转换是否合法
        /// </summary>
        /// <param name="transitionRules">转换规则字典</param>
        /// <param name="statusType">状态类型</param>
        /// <param name="fromStatus">源状态</param>
        /// <param name="toStatus">目标状态</param>
        /// <returns>是否允许转换</returns>
        public static bool IsTransitionAllowed(
            Dictionary<Type, Dictionary<object, List<object>>> transitionRules,
            Type statusType,
            object fromStatus,
            object toStatus)
        {
            if (transitionRules == null || statusType == null || fromStatus == null || toStatus == null)
                return false;

            if (!transitionRules.ContainsKey(statusType))
                return false;

            var statusRules = transitionRules[statusType];
            if (!statusRules.ContainsKey(fromStatus))
                return false;

            return statusRules[fromStatus].Contains(toStatus);
        }

        /// <summary>
        /// 获取可用的状态转换
        /// </summary>
        /// <param name="transitionRules">转换规则字典</param>
        /// <param name="fromStatus">源状态</param>
        /// <returns>可转换的状态列表</returns>
        public static List<Enum> GetAvailableTransitions(Dictionary<Type, Dictionary<object, List<object>>> transitionRules, Enum fromStatus)
        {
            var result = new List<Enum>();

            if (transitionRules == null || fromStatus == null)
                return result;

            var statusType = fromStatus.GetType();

            if (!transitionRules.ContainsKey(statusType))
                return result;

            if (!transitionRules[statusType].ContainsKey(fromStatus))
                return result;

            foreach (var status in transitionRules[statusType][fromStatus])
            {
                if (status is Enum enumStatus)
                {
                    result.Add(enumStatus);
                }
            }

            return result;
        }


        /// <summary>
        /// 获取状态的颜色编码
        /// </summary>
        /// <param name="statusType">状态类型</param>
        /// <param name="status">状态值</param>
        /// <returns>颜色编码</returns>
        private static string GetStatusColorCode(Type statusType, object status)
        {
            if (status == null) return "#000000";

            var statusName = status.ToString();

            // 根据状态名称返回相应的颜色
            if (statusName.Contains("草稿"))
                return "#808080"; // 灰色
            if (statusName.Contains("待审核"))
                return "#FFA500"; // 橙色
            if (statusName.Contains("已生效") || statusName.Contains("已确认"))
                return "#008000"; // 绿色
            if (statusName.Contains("已支付") || statusName.Contains("已结清") || statusName.Contains("全额核销"))
                return "#0000FF"; // 蓝色
            if (statusName.Contains("部分"))
                return "#800080"; // 紫色
            if (statusName.Contains("已作废") || statusName.Contains("坏账"))
                return "#FF0000"; // 红色
            if (statusName.Contains("已冲销") || statusName.Contains("已结案"))
                return "#800000"; // 深红色

            return "#000000"; // 默认黑色
        }

        /// <summary>
        /// 检查状态是否可编辑
        /// </summary>
        /// <param name="statusType">状态类型</param>
        /// <param name="status">状态值</param>
        /// <returns>是否可编辑</returns>
        private static bool IsEditableStatus(Type statusType, object status)
        {
            if (status == null) return false;

            var statusName = status.ToString();

            // 草稿、待审核、待支付等状态可以编辑
            if (statusName.Contains("草稿") ||
                statusName.Contains("待审核") ||
                statusName.Contains("待支付") ||
                statusName.Contains("待核销") ||
                statusName.Contains("部分"))
                return true;

            // 已支付、已结清、已结案、已作废、已冲销等状态不可编辑
            if (statusName.Contains("已支付") ||
                statusName.Contains("已结清") ||
                statusName.Contains("已结案") ||
                statusName.Contains("已作废") ||
                statusName.Contains("已冲销") ||
                statusName.Contains("坏账") ||
                statusName.Contains("全额"))
                return false;

            // 默认可编辑
            return true;
        }

        /// <summary>
        /// 检查状态是否可删除
        /// </summary>
        /// <param name="statusType">状态类型</param>
        /// <param name="status">状态值</param>
        /// <returns>是否可删除</returns>
        private static bool IsDeletableStatus(Type statusType, object status)
        {
            if (status == null) return false;

            var statusName = status.ToString();

            // 草稿状态可以删除
            if (statusName.Contains("草稿"))
                return true;

            // 待审核状态可以删除
            if (statusName.Contains("待审核"))
                return true;

            // 其他状态默认不可删除
            return false;
        }

        /// <summary>
        /// 检查状态是否允许审批
        /// </summary>
        /// <param name="statusType">状态类型</param>
        /// <param name="status">状态值</param>
        /// <returns>是否允许审批</returns>
        private static bool AllowApprovalStatus(Type statusType, object status)
        {
            if (status == null) return false;

            var statusName = status.ToString();

            // 待审核状态允许审批
            if (statusName.Contains("待审核"))
                return true;

            // 其他状态默认不允许审批
            return false;
        }

        /// <summary>
        /// 检查状态是否允许提交
        /// </summary>
        /// <param name="statusType">状态类型</param>
        /// <param name="status">状态值</param>
        /// <returns>是否允许提交</returns>
        private static bool AllowSubmitStatus(Type statusType, object status)
        {
            if (status == null) return false;

            var statusName = status.ToString();

            // 草稿状态允许提交
            if (statusName.Contains("草稿"))
                return true;

            // 其他状态默认不允许提交
            return false;
        }

        /// <summary>
        /// 检查状态是否允许取消
        /// </summary>
        /// <param name="statusType">状态类型</param>
        /// <param name="status">状态值</param>
        /// <returns>是否允许取消</returns>
        private static bool AllowCancelStatus(Type statusType, object status)
        {
            if (status == null) return false;

            var statusName = status.ToString();

            // 待审核、待支付、待核销等状态允许取消
            if (statusName.Contains("待"))
                return true;

            // 其他状态默认不允许取消
            return false;
        }
    }


}