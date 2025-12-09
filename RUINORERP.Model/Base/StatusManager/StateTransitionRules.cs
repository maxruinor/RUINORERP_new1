/**
 * 文件: StateTransitionRules.cs
 * 说明: 统一状态转换规则管理类
 * 创建日期: 2024年
 * 作者: RUINOR ERP开发团队
 * 
 * 迁移指南：
 * 此类现在是状态转换规则的统一管理类，替代了StateRuleConfiguration中的重复规则定义。
 * 
 * 迁移步骤：
 * 1. 使用StateTransitionRules.InitializeDefaultRules初始化所有状态转换规则
 * 2. 使用StateTransitionRules.AddTransitionRule添加自定义规则
 * 3. 使用StateTransitionRules.IsTransitionAllowed验证状态转换
 * 
 * 示例代码：
 * // 初始化状态转换规则
 * var transitionRules = new Dictionary<Type, Dictionary<object, List<object>>>();
 * StateTransitionRules.InitializeDefaultRules(transitionRules);
 * 
 * // 添加自定义规则
 * StateTransitionRules.AddTransitionRule(transitionRules, CustomStatus.草稿, CustomStatus.新建);
 * 
 * // 验证状态转换
 * bool canTransition = StateTransitionRules.IsTransitionAllowed(transitionRules, fromStatus, toStatus);
 */

using RUINORERP.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace RUINORERP.Model.Base.StatusManager
{
    /// <summary>
    /// 统一状态转换规则管理类
    /// </summary>
    public static class StateTransitionRules
    {
        /// <summary>
        /// 初始化默认状态转换规则
        /// </summary>
        /// <param name="transitionRules">转换规则字典</param>
        public static void InitializeDefaultRules(Dictionary<Type, Dictionary<object, List<object>>> transitionRules)
        {
            if (transitionRules == null)
                throw new ArgumentNullException(nameof(transitionRules));

            // 初始化DataStatus转换规则
            InitializeDataStatusRules(transitionRules);

            // 初始化ActionStatus转换规则
            InitializeActionStatusRules(transitionRules);

            // 初始化业务状态转换规则
            InitializeBusinessStatusRules(transitionRules);
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
        /// 获取业务状态规则
        /// </summary>
        /// <param name="statusType">业务状态类型</param>
        /// <param name="status">状态值</param>
        /// <returns>业务状态规则</returns>
        public static BusinessStatusRule GetBusinessStatusRule(Type statusType, object status)
        {
            // 根据状态类型和状态值返回相应的业务规则
            // 这里可以根据实际业务需求扩展
            var key = $"{statusType?.Name}_{status}";
            
            // 返回默认规则，可以根据需要扩展
            return new BusinessStatusRule
            {
                DisplayText = status?.ToString() ?? string.Empty,
                Description = $"{statusType?.Name} - {status}",
                IsEditable = IsEditableStatus(statusType, status),
                IsDeletable = IsDeletableStatus(statusType, status),
                AllowApproval = AllowApprovalStatus(statusType, status),
                AllowPrint = true,
                AllowSubmit = AllowSubmitStatus(statusType, status),
                AllowCancel = AllowCancelStatus(statusType, status),
                AllowModify = IsEditableStatus(statusType, status),
                AllowDelete = IsDeletableStatus(statusType, status),
                AllowView = true,
                AllowExport = true,
                ColorCode = GetStatusColorCode(statusType, status)
            };
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
        /// 颜色编码
        /// </summary>
        public string ColorCode { get; set; }
    }

    #endregion
}