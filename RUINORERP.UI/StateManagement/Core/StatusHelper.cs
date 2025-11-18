using System;
using System.Collections.Generic;
using System.Linq;
using RUINORERP.Global;
using RUINORERP.Global.EnumExt;

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
                    return dataStatus == DataStatus.确认 || dataStatus == DataStatus.完结;
                case PrePaymentStatus preStatus:
                    return preStatus == PrePaymentStatus.已生效;
                case ARAPStatus arapStatus:
                    return arapStatus == ARAPStatus.待支付 || arapStatus == ARAPStatus.部分支付 || arapStatus == ARAPStatus.全部支付;
                case PaymentStatus paymentStatus:
                    return paymentStatus == PaymentStatus.已支付;
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
                    return dataStatus == DataStatus.作废;
                case PrePaymentStatus preStatus:
                    return preStatus == PrePaymentStatus.已结案;
                case ARAPStatus arapStatus:
                    return arapStatus == ARAPStatus.坏账 || arapStatus == ARAPStatus.已冲销;
                case PaymentStatus paymentStatus:
                    // PaymentStatus没有明确的禁用状态，返回false
                    return false;
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
                case DataStatus.确认:
                    return "确认";
                case DataStatus.完结:
                    return "完结";
                case DataStatus.作废:
                    return "作废";
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
                case PrePaymentStatus.待审核:
                    return "待审核";
                case PrePaymentStatus.已生效:
                    return "已生效";
                case PrePaymentStatus.待核销:
                    return "待核销";
                case PrePaymentStatus.部分核销:
                    return "部分核销";
                case PrePaymentStatus.全额核销:
                    return "全额核销";
                case PrePaymentStatus.已结案:
                    return "已结案";
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
                case ARAPStatus.待审核:
                    return "待审核";
                case ARAPStatus.待支付:
                    return "待支付";
                case ARAPStatus.部分支付:
                    return "部分支付";
                case ARAPStatus.全部支付:
                    return "全部支付";
                case ARAPStatus.坏账:
                    return "坏账";
                case ARAPStatus.已冲销:
                    return "已冲销";
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
                case PaymentStatus.待审核:
                    return "待审核";
                case PaymentStatus.已支付:
                    return "已支付";
                default:
                    return status.ToString();
            }
        }

        /// <summary>
        /// 获取状态对应的可用操作列表
        /// </summary>
        /// <param name="status">当前状态</param>
        /// <returns>可用操作列表</returns>
        public static List<MenuItemEnums> GetAvailableActions(object status)
        {
            List<MenuItemEnums> actions = new List<MenuItemEnums>();

            if (status == null)
                return actions;

            // 添加通用操作
            actions.Add(MenuItemEnums.查询);
            actions.Add(MenuItemEnums.刷新);

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
        private static void AddDataStatusActions(List<MenuItemEnums> actions, DataStatus status)
        {
            switch (status)
            {
                case DataStatus.新建:
                case DataStatus.草稿:
                    actions.Add(MenuItemEnums.修改);
                    actions.Add(MenuItemEnums.审核);
                    break;
                case DataStatus.确认:
                    actions.Add(MenuItemEnums.反审);
                    actions.Add(MenuItemEnums.结案);
                    break;
                case DataStatus.完结:
                    actions.Add(MenuItemEnums.反结案);
                    break;
            }
            
            // 所有非作废状态都可以作废
            if (status != DataStatus.作废)
                actions.Add(MenuItemEnums.删除);
        }

        /// <summary>
        /// 为PrePaymentStatus添加可用操作
        /// </summary>
        private static void AddPrePaymentStatusActions(List<MenuItemEnums> actions, PrePaymentStatus status)
        {
            switch (status)
            {
                case PrePaymentStatus.草稿:
                    actions.Add(MenuItemEnums.修改);
                    actions.Add(MenuItemEnums.提交);
                    break;
                case PrePaymentStatus.待审核:
                    actions.Add(MenuItemEnums.审核);
                    break;
                case PrePaymentStatus.已生效:
                    actions.Add(MenuItemEnums.反审);
                    break;
            }
        }

        /// <summary>
        /// 为ARAPStatus添加可用操作
        /// </summary>
        private static void AddARAPStatusActions(List<MenuItemEnums> actions, ARAPStatus status)
        {
            switch (status)
            {
                case ARAPStatus.草稿:
                    actions.Add(MenuItemEnums.修改);
                    actions.Add(MenuItemEnums.提交);
                    break;
                case ARAPStatus.待审核:
                    actions.Add(MenuItemEnums.审核);
                    break;
                case ARAPStatus.待支付:
                case ARAPStatus.部分支付:
                    actions.Add(MenuItemEnums.反审);
                    break;
            }
        }

        /// <summary>
        /// 为PaymentStatus添加可用操作
        /// </summary>
        private static void AddPaymentStatusActions(List<MenuItemEnums> actions, PaymentStatus status)
        {
            switch (status)
            {
                case PaymentStatus.草稿:
                    actions.Add(MenuItemEnums.修改);
                    actions.Add(MenuItemEnums.提交);
                    break;
                case PaymentStatus.待审核:
                    actions.Add(MenuItemEnums.审核);
                    break;
            }
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
            switch (from)
            {
                case DataStatus.新建:
                case DataStatus.草稿:
                    return to == DataStatus.确认 || to == DataStatus.作废;
                case DataStatus.确认:
                    return to == DataStatus.完结 || to == DataStatus.作废;
                case DataStatus.完结:
                    return false; // 完结状态不能再转换
                case DataStatus.作废:
                    return to == DataStatus.草稿; // 作废后可以重新编辑
                default:
                    return false;
            }
        }

        /// <summary>
        /// 验证PrePaymentStatus状态转换是否合法
        /// </summary>
        private static bool IsValidPrePaymentStatusTransition(PrePaymentStatus from, PrePaymentStatus to)
        {
            switch (from)
            {
                case PrePaymentStatus.草稿:
                    return to == PrePaymentStatus.待审核 || to == PrePaymentStatus.已结案;
                case PrePaymentStatus.待审核:
                    return to == PrePaymentStatus.已生效 || to == PrePaymentStatus.草稿 || to == PrePaymentStatus.已结案;
                case PrePaymentStatus.已生效:
                    return to == PrePaymentStatus.待核销 || to == PrePaymentStatus.部分核销 || to == PrePaymentStatus.全额核销;
                case PrePaymentStatus.待核销:
                    return to == PrePaymentStatus.部分核销 || to == PrePaymentStatus.全额核销;
                case PrePaymentStatus.部分核销:
                    return to == PrePaymentStatus.全额核销;
                case PrePaymentStatus.全额核销:
                    return to == PrePaymentStatus.已结案;
                case PrePaymentStatus.已结案:
                    return false; // 已结案状态不能再转换
                default:
                    return false;
            }
        }

        /// <summary>
        /// 验证ARAPStatus状态转换是否合法
        /// </summary>
        private static bool IsValidARAPStatusTransition(ARAPStatus from, ARAPStatus to)
        {
            switch (from)
            {
                case ARAPStatus.草稿:
                    return to == ARAPStatus.待审核 || to == ARAPStatus.坏账 || to == ARAPStatus.已冲销;
                case ARAPStatus.待审核:
                    return to == ARAPStatus.待支付 || to == ARAPStatus.草稿 || to == ARAPStatus.坏账 || to == ARAPStatus.已冲销;
                case ARAPStatus.待支付:
                    return to == ARAPStatus.部分支付 || to == ARAPStatus.全部支付 || to == ARAPStatus.坏账 || to == ARAPStatus.已冲销;
                case ARAPStatus.部分支付:
                    return to == ARAPStatus.全部支付 || to == ARAPStatus.坏账 || to == ARAPStatus.已冲销;
                case ARAPStatus.全部支付:
                    return to == ARAPStatus.坏账 || to == ARAPStatus.已冲销;
                case ARAPStatus.坏账:
                case ARAPStatus.已冲销:
                    return false; // 坏账和已冲销状态不能再转换
                default:
                    return false;
            }
        }

        /// <summary>
        /// 验证PaymentStatus状态转换是否合法
        /// </summary>
        private static bool IsValidPaymentStatusTransition(PaymentStatus from, PaymentStatus to)
        {
            switch (from)
            {
                case PaymentStatus.草稿:
                    return to == PaymentStatus.待审核;
                case PaymentStatus.待审核:
                    return to == PaymentStatus.已支付 || to == PaymentStatus.草稿;
                case PaymentStatus.已支付:
                    return false; // 已支付状态不能再转换
                default:
                    return false;
            }
        }
    }
}