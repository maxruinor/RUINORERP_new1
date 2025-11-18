using System;
using System.Collections.Generic;
using System.Linq;
using RUINORERP.Model.Base.StatusManager.Core;

namespace RUINORERP.UI.StateManagement.Core
{
    /// <summary>
    /// 状态辅助工具类 - V3状态管理系统
    /// 提供通用的状态处理方法，减少重复代码
    /// </summary>
    public static class StatusHelper
    {
        /// <summary>
        /// 判断状态是否为可编辑状态
        /// </summary>
        /// <param name="status">状态对象</param>
        /// <returns>是否可编辑</returns>
        public static bool IsEditableStatus(object status)
        {
            if (status == null)
                return false;

            switch (status)
            {
                case DataStatus dataStatus:
                    return dataStatus == DataStatus.草稿 || dataStatus == DataStatus.新建;
                case PrePaymentStatus preStatus:
                    return preStatus == PrePaymentStatus.草稿;
                case ARAPStatus arapStatus:
                    return arapStatus == ARAPStatus.草稿;
                case PaymentStatus paymentStatus:
                    return paymentStatus == PaymentStatus.草稿;
                default:
                    return false;
            }
        }

        /// <summary>
        /// 判断状态是否为已审核状态
        /// </summary>
        /// <param name="status">状态对象</param>
        /// <returns>是否已审核</returns>
        public static bool IsApprovedStatus(object status)
        {
            if (status == null)
                return false;

            switch (status)
            {
                case DataStatus dataStatus:
                    return dataStatus == DataStatus.已审核 || dataStatus == DataStatus.已过账;
                case PrePaymentStatus preStatus:
                    return preStatus == PrePaymentStatus.已审核;
                case ARAPStatus arapStatus:
                    return arapStatus == ARAPStatus.已审核 || arapStatus == ARAPStatus.已结算;
                case PaymentStatus paymentStatus:
                    return paymentStatus == PaymentStatus.已审核;
                default:
                    return false;
            }
        }

        /// <summary>
        /// 判断状态是否为已禁用状态
        /// </summary>
        /// <param name="status">状态对象</param>
        /// <returns>是否已禁用</returns>
        public static bool IsDisabledStatus(object status)
        {
            if (status == null)
                return false;

            switch (status)
            {
                case DataStatus dataStatus:
                    return dataStatus == DataStatus.已禁用;
                case PrePaymentStatus preStatus:
                    return preStatus == PrePaymentStatus.已禁用;
                case ARAPStatus arapStatus:
                    return arapStatus == ARAPStatus.已禁用;
                case PaymentStatus paymentStatus:
                    return paymentStatus == PaymentStatus.已禁用;
                default:
                    return false;
            }
        }

        /// <summary>
        /// 获取状态对应的中文显示名称
        /// </summary>
        /// <param name="status">状态对象</param>
        /// <returns>中文显示名称</returns>
        public static string GetStatusDisplayName(object status)
        {
            if (status == null)
                return string.Empty;

            switch (status)
            {
                case DataStatus dataStatus:
                    return GetDataStatusDisplayName(dataStatus);
                case PrePaymentStatus preStatus:
                    return GetPrePaymentStatusDisplayName(preStatus);
                case ARAPStatus arapStatus:
                    return GetARAPStatusDisplayName(arapStatus);
                case PaymentStatus paymentStatus:
                    return GetPaymentStatusDisplayName(paymentStatus);
                default:
                    return status.ToString();
            }
        }

        /// <summary>
        /// 获取DataStatus对应的中文显示名称
        /// </summary>
        /// <param name="status">数据状态</param>
        /// <returns>中文显示名称</returns>
        private static string GetDataStatusDisplayName(DataStatus status)
        {
            switch (status)
            {
                case DataStatus.新建:
                    return "新建";
                case DataStatus.草稿:
                    return "草稿";
                case DataStatus.已审核:
                    return "已审核";
                case DataStatus.已过账:
                    return "已过账";
                case DataStatus.已禁用:
                    return "已禁用";
                default:
                    return status.ToString();
            }
        }

        /// <summary>
        /// 获取PrePaymentStatus对应的中文显示名称
        /// </summary>
        /// <param name="status">预付款状态</param>
        /// <returns>中文显示名称</returns>
        private static string GetPrePaymentStatusDisplayName(PrePaymentStatus status)
        {
            switch (status)
            {
                case PrePaymentStatus.草稿:
                    return "草稿";
                case PrePaymentStatus.已审核:
                    return "已审核";
                case PrePaymentStatus.已禁用:
                    return "已禁用";
                default:
                    return status.ToString();
            }
        }

        /// <summary>
        /// 获取ARAPStatus对应的中文显示名称
        /// </summary>
        /// <param name="status">应收应付状态</param>
        /// <returns>中文显示名称</returns>
        private static string GetARAPStatusDisplayName(ARAPStatus status)
        {
            switch (status)
            {
                case ARAPStatus.草稿:
                    return "草稿";
                case ARAPStatus.已审核:
                    return "已审核";
                case ARAPStatus.已结算:
                    return "已结算";
                case ARAPStatus.已禁用:
                    return "已禁用";
                default:
                    return status.ToString();
            }
        }

        /// <summary>
        /// 获取PaymentStatus对应的中文显示名称
        /// </summary>
        /// <param name="status">付款状态</param>
        /// <returns>中文显示名称</returns>
        private static string GetPaymentStatusDisplayName(PaymentStatus status)
        {
            switch (status)
            {
                case PaymentStatus.草稿:
                    return "草稿";
                case PaymentStatus.已审核:
                    return "已审核";
                case PaymentStatus.已禁用:
                    return "已禁用";
                default:
                    return status.ToString();
            }
        }

        /// <summary>
        /// 获取状态对应的可用操作列表
        /// </summary>
        /// <param name="status">当前状态</param>
        /// <returns>可用操作列表</returns>
        public static List<ActionType> GetAvailableActions(object status)
        {
            List<ActionType> actions = new List<ActionType>();

            if (status == null)
                return actions;

            // 添加通用操作
            actions.Add(ActionType.查看);
            actions.Add(ActionType.刷新);

            // 根据不同状态添加特定操作
            switch (status)
            {
                case DataStatus dataStatus:
                    AddDataStatusActions(actions, dataStatus);
                    break;
                case PrePaymentStatus preStatus:
                    AddPrePaymentStatusActions(actions, preStatus);
                    break;
                case ARAPStatus arapStatus:
                    AddARAPStatusActions(actions, arapStatus);
                    break;
                case PaymentStatus paymentStatus:
                    AddPaymentStatusActions(actions, paymentStatus);
                    break;
            }

            return actions;
        }

        /// <summary>
        /// 为DataStatus添加可用操作
        /// </summary>
        private static void AddDataStatusActions(List<ActionType> actions, DataStatus status)
        {
            switch (status)
            {
                case DataStatus.新建:
                case DataStatus.草稿:
                    actions.Add(ActionType.编辑);
                    actions.Add(ActionType.审核);
                    break;
                case DataStatus.已审核:
                    actions.Add(ActionType.反审核);
                    actions.Add(ActionType.过账);
                    break;
                case DataStatus.已过账:
                    actions.Add(ActionType.反过账);
                    break;
            }
            
            // 所有非禁用状态都可以禁用
            if (status != DataStatus.已禁用)
                actions.Add(ActionType.禁用);
            else
                actions.Add(ActionType.启用);
        }

        /// <summary>
        /// 为PrePaymentStatus添加可用操作
        /// </summary>
        private static void AddPrePaymentStatusActions(List<ActionType> actions, PrePaymentStatus status)
        {
            switch (status)
            {
                case PrePaymentStatus.草稿:
                    actions.Add(ActionType.编辑);
                    actions.Add(ActionType.审核);
                    break;
                case PrePaymentStatus.已审核:
                    actions.Add(ActionType.反审核);
                    break;
            }
            
            // 所有非禁用状态都可以禁用
            if (status != PrePaymentStatus.已禁用)
                actions.Add(ActionType.禁用);
            else
                actions.Add(ActionType.启用);
        }

        /// <summary>
        /// 为ARAPStatus添加可用操作
        /// </summary>
        private static void AddARAPStatusActions(List<ActionType> actions, ARAPStatus status)
        {
            switch (status)
            {
                case ARAPStatus.草稿:
                    actions.Add(ActionType.编辑);
                    actions.Add(ActionType.审核);
                    break;
                case ARAPStatus.已审核:
                    actions.Add(ActionType.反审核);
                    actions.Add(ActionType.结算);
                    break;
            }
            
            // 所有非禁用状态都可以禁用
            if (status != ARAPStatus.已禁用)
                actions.Add(ActionType.禁用);
            else
                actions.Add(ActionType.启用);
        }

        /// <summary>
        /// 为PaymentStatus添加可用操作
        /// </summary>
        private static void AddPaymentStatusActions(List<ActionType> actions, PaymentStatus status)
        {
            switch (status)
            {
                case PaymentStatus.草稿:
                    actions.Add(ActionType.编辑);
                    actions.Add(ActionType.审核);
                    break;
                case PaymentStatus.已审核:
                    actions.Add(ActionType.反审核);
                    break;
            }
            
            // 所有非禁用状态都可以禁用
            if (status != PaymentStatus.已禁用)
                actions.Add(ActionType.禁用);
            else
                actions.Add(ActionType.启用);
        }

        /// <summary>
        /// 验证状态转换是否合法
        /// </summary>
        /// <param name="fromStatus">源状态</param>
        /// <param name="toStatus">目标状态</param>
        /// <returns>是否合法转换</returns>
        public static bool IsValidStateTransition(object fromStatus, object toStatus)
        {
            // 确保类型相同
            if (fromStatus?.GetType() != toStatus?.GetType())
                return false;

            // 相同状态无需转换
            if (fromStatus.Equals(toStatus))
                return true;

            // 根据不同状态类型进行验证
            if (fromStatus is DataStatus dataFrom && toStatus is DataStatus dataTo)
            {
                return IsValidDataStatusTransition(dataFrom, dataTo);
            }
            else if (fromStatus is PrePaymentStatus preFrom && toStatus is PrePaymentStatus preTo)
            {
                return IsValidPrePaymentStatusTransition(preFrom, preTo);
            }
            else if (fromStatus is ARAPStatus arapFrom && toStatus is ARAPStatus arapTo)
            {
                return IsValidARAPStatusTransition(arapFrom, arapTo);
            }
            else if (fromStatus is PaymentStatus payFrom && toStatus is PaymentStatus payTo)
            {
                return IsValidPaymentStatusTransition(payFrom, payTo);
            }

            return false;
        }

        /// <summary>
        /// 验证DataStatus状态转换是否合法
        /// </summary>
        private static bool IsValidDataStatusTransition(DataStatus from, DataStatus to)
        {
            // 禁用/启用可以从任何状态进行
            if (to == DataStatus.已禁用 || from == DataStatus.已禁用)
                return true;

            switch (from)
            {
                case DataStatus.新建:
                case DataStatus.草稿:
                    return to == DataStatus.已审核 || to == DataStatus.草稿;
                case DataStatus.已审核:
                    return to == DataStatus.草稿 || to == DataStatus.已过账;
                case DataStatus.已过账:
                    return to == DataStatus.已审核;
                default:
                    return false;
            }
        }

        /// <summary>
        /// 验证PrePaymentStatus状态转换是否合法
        /// </summary>
        private static bool IsValidPrePaymentStatusTransition(PrePaymentStatus from, PrePaymentStatus to)
        {
            // 禁用/启用可以从任何状态进行
            if (to == PrePaymentStatus.已禁用 || from == PrePaymentStatus.已禁用)
                return true;

            switch (from)
            {
                case PrePaymentStatus.草稿:
                    return to == PrePaymentStatus.已审核;
                case PrePaymentStatus.已审核:
                    return to == PrePaymentStatus.草稿;
                default:
                    return false;
            }
        }

        /// <summary>
        /// 验证ARAPStatus状态转换是否合法
        /// </summary>
        private static bool IsValidARAPStatusTransition(ARAPStatus from, ARAPStatus to)
        {
            // 禁用/启用可以从任何状态进行
            if (to == ARAPStatus.已禁用 || from == ARAPStatus.已禁用)
                return true;

            switch (from)
            {
                case ARAPStatus.草稿:
                    return to == ARAPStatus.已审核;
                case ARAPStatus.已审核:
                    return to == ARAPStatus.草稿 || to == ARAPStatus.已结算;
                case ARAPStatus.已结算:
                    return to == ARAPStatus.已审核;
                default:
                    return false;
            }
        }

        /// <summary>
        /// 验证PaymentStatus状态转换是否合法
        /// </summary>
        private static bool IsValidPaymentStatusTransition(PaymentStatus from, PaymentStatus to)
        {
            // 禁用/启用可以从任何状态进行
            if (to == PaymentStatus.已禁用 || from == PaymentStatus.已禁用)
                return true;

            switch (from)
            {
                case PaymentStatus.草稿:
                    return to == PaymentStatus.已审核;
                case PaymentStatus.已审核:
                    return to == PaymentStatus.草稿;
                default:
                    return false;
            }
        }
    }
}